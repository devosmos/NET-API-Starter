targetScope = 'resourceGroup'

@description('Short workload name used for Azure resource names.')
param workloadName string = 'devosmos-api'

@allowed([
  'dev'
  'prod'
])
@description('Deployment environment name.')
param environmentName string

@description('Azure region for all regional resources.')
param location string = resourceGroup().location

@description('Entra ID authority used by JWT bearer authentication.')
param authenticationAuthority string

@description('Expected audience/application ID URI for JWT bearer authentication.')
param authenticationAudience string

@description('Single allowed CORS origin. Leave empty to disable CORS origins.')
param corsAllowedOrigin string = ''

@description('SQL logical server administrator login for break-glass access.')
param sqlAdministratorLogin string = 'sqladminuser'

@secure()
@description('SQL logical server administrator password. Use a secret source, not a checked-in parameter value.')
param sqlAdministratorPassword string

@description('Display name or UPN for the Microsoft Entra SQL administrator.')
param sqlAadAdminLogin string

@description('Object ID for the Microsoft Entra SQL administrator.')
param sqlAadAdminObjectId string

@description('Optional tags applied to all resources.')
param tags object = {}

@description('App Service plan SKU.')
param appServiceSkuName string = environmentName == 'prod' ? 'P1v3' : 'B1'

@description('Azure SQL database SKU.')
param sqlDatabaseSkuName string = environmentName == 'prod' ? 'GP_S_Gen5_2' : 'Basic'

@description('Global fixed-window rate limit.')
param rateLimitingPermitLimit int = environmentName == 'prod' ? 300 : 100

@description('Global fixed-window rate-limit window in seconds.')
param rateLimitingWindowSeconds int = 60

var suffix = toLower(uniqueString(resourceGroup().id, workloadName, environmentName))
var baseName = toLower('${workloadName}-${environmentName}-${suffix}')
var sanitizedBaseName = replace(baseName, '-', '')
var appName = take('app-${baseName}', 60)
var planName = take('asp-${baseName}', 60)
var kvName = take('kv${sanitizedBaseName}', 24)
var logName = take('log-${baseName}', 63)
var appInsightsName = take('appi-${baseName}', 60)
var sqlServerName = take('sql-${baseName}', 63)
var sqlDatabaseName = 'DevosmosApiStarter'
var vnetName = take('vnet-${baseName}', 64)
var privateEndpointName = take('pe-sql-${baseName}', 64)
var dnsZoneName = 'privatelink${environment().suffixes.sqlServerHostname}'
var commonTags = union(tags, {
  workload: workloadName
  environment: environmentName
  managedBy: 'bicep'
})

resource logAnalytics 'Microsoft.OperationalInsights/workspaces@2023-09-01' = {
  name: logName
  location: location
  tags: commonTags
  properties: {
    sku: {
      name: 'PerGB2018'
    }
    retentionInDays: environmentName == 'prod' ? 90 : 30
  }
}

resource appInsights 'Microsoft.Insights/components@2020-02-02' = {
  name: appInsightsName
  location: location
  kind: 'web'
  tags: commonTags
  properties: {
    Application_Type: 'web'
    WorkspaceResourceId: logAnalytics.id
  }
}

resource vnet 'Microsoft.Network/virtualNetworks@2023-11-01' = {
  name: vnetName
  location: location
  tags: commonTags
  properties: {
    addressSpace: {
      addressPrefixes: [
        '10.42.0.0/16'
      ]
    }
    subnets: [
      {
        name: 'app'
        properties: {
          addressPrefix: '10.42.1.0/24'
          delegations: [
            {
              name: 'webapp-delegation'
              properties: {
                serviceName: 'Microsoft.Web/serverFarms'
              }
            }
          ]
        }
      }
      {
        name: 'private-endpoints'
        properties: {
          addressPrefix: '10.42.2.0/24'
          privateEndpointNetworkPolicies: 'Disabled'
        }
      }
    ]
  }
}

resource appSubnet 'Microsoft.Network/virtualNetworks/subnets@2023-11-01' existing = {
  parent: vnet
  name: 'app'
}

resource privateEndpointSubnet 'Microsoft.Network/virtualNetworks/subnets@2023-11-01' existing = {
  parent: vnet
  name: 'private-endpoints'
}

resource keyVault 'Microsoft.KeyVault/vaults@2023-07-01' = {
  name: kvName
  location: location
  tags: commonTags
  properties: {
    tenantId: tenant().tenantId
    sku: {
      family: 'A'
      name: 'standard'
    }
    enableRbacAuthorization: true
    enabledForTemplateDeployment: false
    enableSoftDelete: true
    softDeleteRetentionInDays: 90
    publicNetworkAccess: 'Enabled'
  }
}

resource sqlServer 'Microsoft.Sql/servers@2023-08-01-preview' = {
  name: sqlServerName
  location: location
  tags: commonTags
  properties: {
    administratorLogin: sqlAdministratorLogin
    administratorLoginPassword: sqlAdministratorPassword
    version: '12.0'
    minimalTlsVersion: '1.2'
    publicNetworkAccess: 'Disabled'
  }
}

resource sqlAadAdministrator 'Microsoft.Sql/servers/administrators@2023-08-01-preview' = {
  parent: sqlServer
  name: 'ActiveDirectory'
  properties: {
    administratorType: 'ActiveDirectory'
    login: sqlAadAdminLogin
    sid: sqlAadAdminObjectId
    tenantId: tenant().tenantId
  }
}

resource sqlDatabase 'Microsoft.Sql/servers/databases@2023-08-01-preview' = {
  parent: sqlServer
  name: sqlDatabaseName
  location: location
  tags: commonTags
  sku: {
    name: sqlDatabaseSkuName
  }
  properties: {
    collation: 'SQL_Latin1_General_CP1_CI_AS'
    zoneRedundant: environmentName == 'prod'
  }
}

resource sqlPrivateDnsZone 'Microsoft.Network/privateDnsZones@2020-06-01' = {
  name: dnsZoneName
  location: 'global'
  tags: commonTags
}

resource sqlPrivateDnsLink 'Microsoft.Network/privateDnsZones/virtualNetworkLinks@2020-06-01' = {
  parent: sqlPrivateDnsZone
  name: 'link-${vnet.name}'
  location: 'global'
  properties: {
    registrationEnabled: false
    virtualNetwork: {
      id: vnet.id
    }
  }
}

resource sqlPrivateEndpoint 'Microsoft.Network/privateEndpoints@2023-11-01' = {
  name: privateEndpointName
  location: location
  tags: commonTags
  properties: {
    subnet: {
      id: privateEndpointSubnet.id
    }
    privateLinkServiceConnections: [
      {
        name: 'sql'
        properties: {
          privateLinkServiceId: sqlServer.id
          groupIds: [
            'sqlServer'
          ]
        }
      }
    ]
  }
}

resource sqlPrivateDnsZoneGroup 'Microsoft.Network/privateEndpoints/privateDnsZoneGroups@2023-11-01' = {
  parent: sqlPrivateEndpoint
  name: 'default'
  properties: {
    privateDnsZoneConfigs: [
      {
        name: dnsZoneName
        properties: {
          privateDnsZoneId: sqlPrivateDnsZone.id
        }
      }
    ]
  }
  dependsOn: [
    sqlPrivateDnsLink
  ]
}

resource appServicePlan 'Microsoft.Web/serverfarms@2023-12-01' = {
  name: planName
  location: location
  tags: commonTags
  kind: 'linux'
  sku: {
    name: appServiceSkuName
  }
  properties: {
    reserved: true
  }
}

var sqlConnectionString = 'Server=tcp:${sqlServer.properties.fullyQualifiedDomainName},1433;Initial Catalog=${sqlDatabase.name};Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;Authentication=Active Directory Default;'

resource dbConnectionSecret 'Microsoft.KeyVault/vaults/secrets@2023-07-01' = {
  parent: keyVault
  name: 'ConnectionStrings--Default'
  properties: {
    value: sqlConnectionString
    contentType: 'text/plain'
  }
}

var appSiteConfig = {
  alwaysOn: environmentName == 'prod'
  ftpsState: 'Disabled'
  healthCheckPath: '/health/ready'
  http20Enabled: true
  linuxFxVersion: 'DOTNETCORE|10.0'
  minTlsVersion: '1.2'
  remoteDebuggingEnabled: false
  appSettings: [
    {
      name: 'ASPNETCORE_ENVIRONMENT'
      value: environmentName == 'prod' ? 'Production' : 'Development'
    }
    {
      name: 'Authentication__Authority'
      value: authenticationAuthority
    }
    {
      name: 'Authentication__Audience'
      value: authenticationAudience
    }
    {
      name: 'ConnectionStrings__Default'
      value: '@Microsoft.KeyVault(SecretUri=${dbConnectionSecret.properties.secretUriWithVersion})'
    }
    {
      name: 'Cors__AllowedOrigins__0'
      value: corsAllowedOrigin
    }
    {
      name: 'OpenApi__Enabled'
      value: 'false'
    }
    {
      name: 'RateLimiting__PermitLimit'
      value: string(rateLimitingPermitLimit)
    }
    {
      name: 'RateLimiting__WindowSeconds'
      value: string(rateLimitingWindowSeconds)
    }
    {
      name: 'Observability__ApplicationInsights__Enabled'
      value: 'true'
    }
    {
      name: 'APPLICATIONINSIGHTS_CONNECTION_STRING'
      value: appInsights.properties.ConnectionString
    }
    {
      name: 'KeyVault__Uri'
      value: keyVault.properties.vaultUri
    }
    {
      name: 'WEBSITES_ENABLE_APP_SERVICE_STORAGE'
      value: 'false'
    }
    {
      name: 'ASPNETCORE_FORWARDEDHEADERS_ENABLED'
      value: 'true'
    }
  ]
}

resource webApp 'Microsoft.Web/sites@2023-12-01' = {
  name: appName
  location: location
  tags: commonTags
  kind: 'app,linux'
  identity: {
    type: 'SystemAssigned'
  }
  properties: {
    serverFarmId: appServicePlan.id
    httpsOnly: true
    clientAffinityEnabled: false
    virtualNetworkSubnetId: appSubnet.id
    siteConfig: appSiteConfig
  }
}

resource stagingSlot 'Microsoft.Web/sites/slots@2023-12-01' = {
  parent: webApp
  name: 'staging'
  location: location
  tags: commonTags
  kind: 'app,linux'
  identity: {
    type: 'SystemAssigned'
  }
  properties: {
    serverFarmId: appServicePlan.id
    httpsOnly: true
    clientAffinityEnabled: false
    virtualNetworkSubnetId: appSubnet.id
    siteConfig: appSiteConfig
  }
}

var keyVaultSecretsUserRoleDefinitionId = subscriptionResourceId(
  'Microsoft.Authorization/roleDefinitions',
  '4633458b-17de-408a-b874-0445c86b69e6')

resource webAppKeyVaultAccess 'Microsoft.Authorization/roleAssignments@2022-04-01' = {
  name: guid(keyVault.id, webApp.id, 'key-vault-secrets-user')
  scope: keyVault
  properties: {
    roleDefinitionId: keyVaultSecretsUserRoleDefinitionId
    principalId: webApp.identity.principalId
    principalType: 'ServicePrincipal'
  }
}

resource stagingSlotKeyVaultAccess 'Microsoft.Authorization/roleAssignments@2022-04-01' = {
  name: guid(keyVault.id, stagingSlot.id, 'key-vault-secrets-user')
  scope: keyVault
  properties: {
    roleDefinitionId: keyVaultSecretsUserRoleDefinitionId
    principalId: stagingSlot.identity.principalId
    principalType: 'ServicePrincipal'
  }
}

resource appDiagnostics 'Microsoft.Insights/diagnosticSettings@2021-05-01-preview' = {
  name: 'appservice-to-loganalytics'
  scope: webApp
  properties: {
    workspaceId: logAnalytics.id
    logs: [
      {
        category: 'AppServiceHTTPLogs'
        enabled: true
      }
      {
        category: 'AppServiceConsoleLogs'
        enabled: true
      }
      {
        category: 'AppServiceAppLogs'
        enabled: true
      }
    ]
    metrics: [
      {
        category: 'AllMetrics'
        enabled: true
      }
    ]
  }
}

output webAppName string = webApp.name
output stagingSlotName string = stagingSlot.name
output appUrl string = 'https://${webApp.properties.defaultHostName}'
output stagingUrl string = 'https://${stagingSlot.properties.defaultHostName}'
output keyVaultName string = keyVault.name
output sqlServerName string = sqlServer.name
output sqlDatabaseName string = sqlDatabase.name
