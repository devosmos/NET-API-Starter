using '../main.bicep'

param environmentName = 'prod'
param location = 'eastus2'
param authenticationAuthority = 'https://login.microsoftonline.com/<tenant-id>/v2.0'
param authenticationAudience = 'api://devosmos-api-starter-prod'
param corsAllowedOrigin = 'https://api.example.com'
param sqlAdministratorPassword = readEnvironmentVariable('SQL_ADMIN_PASSWORD')
param sqlAadAdminLogin = '<aad-admin-upn-or-group-name>'
param sqlAadAdminObjectId = '<aad-admin-object-id>'
param appServiceSkuName = 'P1v3'
param sqlDatabaseSkuName = 'GP_S_Gen5_2'
param tags = {
  owner: 'platform'
  costCenter: 'devosmos'
  dataClassification: 'internal'
}
