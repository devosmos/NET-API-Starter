using '../main.bicep'

param environmentName = 'dev'
param location = 'eastus2'
param authenticationAuthority = 'https://login.microsoftonline.com/<tenant-id>/v2.0'
param authenticationAudience = 'api://devosmos-api-starter-dev'
param corsAllowedOrigin = 'https://dev.example.com'
param sqlAdministratorPassword = readEnvironmentVariable('SQL_ADMIN_PASSWORD')
param sqlAadAdminLogin = '<aad-admin-upn-or-group-name>'
param sqlAadAdminObjectId = '<aad-admin-object-id>'
param tags = {
  owner: 'platform'
  costCenter: 'devosmos'
}
