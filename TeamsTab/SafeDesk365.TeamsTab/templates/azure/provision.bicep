@secure()
param provisionParameters object
// Resources for frontend hosting
module frontendHostingProvision './provision/frontendHosting.bicep' = {
  name: 'frontendHostingProvision'
  params: {
    provisionParameters: provisionParameters
    userAssignedIdentityId: userAssignedIdentityProvision.outputs.identityResourceId
  }
}

output frontendHostingOutput object = {
  teamsFxPluginId: 'fx-resource-frontend-hosting'
  domain: frontendHostingProvision.outputs.domain
  endpoint: frontendHostingProvision.outputs.endpoint
  webAppResourceId: frontendHostingProvision.outputs.resourceId
}

// Resources for identity
module userAssignedIdentityProvision './provision/identity.bicep' = {
  name: 'userAssignedIdentityProvision'
  params: {
    provisionParameters: provisionParameters
  }
}

output identityOutput object = {
  teamsFxPluginId: 'fx-resource-identity'
  identityName: userAssignedIdentityProvision.outputs.identityName
  identityResourceId: userAssignedIdentityProvision.outputs.identityResourceId
  identityClientId: userAssignedIdentityProvision.outputs.identityClientId
}
