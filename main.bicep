param location string = resourceGroup().location //note that the deployment is created in scope/ our scope here is resourcegroup
param containerAppName string = 'todo'
param frontendImage string='ammargamal001/todo-frontend:latest'
param backendImage string='ammargamal001/todo-backend:latest'

resource logAnalytics 'Microsoft.OperationalInsights/workspaces@2026-03-01' = {
  name: '${containerAppName}-log'
  location: location
  properties: {
    retentionInDays: 30
    sku: {
      name: 'PerGB2018'
    }
  }
}
resource environment 'Microsoft.App/managedEnvironments@2026-01-01' = {
  name: '${containerAppName}-env'
  location: location
  properties: {
    appLogsConfiguration: {
      destination: 'log-analytics'
      logAnalyticsConfiguration: {
        customerId: logAnalytics.properties.customerId
        sharedKey: logAnalytics.listKeys().primarySharedKey
      }
    }
  }
}

resource backend 'Microsoft.App/containerApps@2026-01-01' = {
  name: '${containerAppName}-backend'
  location: location

  properties: {
    managedEnvironmentId: environment.id

    configuration: {
      ingress: {
        external: false
        targetPort: 8080
        transport: 'auto'
      }
    }

    template: {
      containers: [
        {
          name: 'api'
          image: backendImage

          resources: {
            cpu: any('0.25')
            memory: '0.5Gi'
          }
        }
      ]

      scale: {
        minReplicas: 0
        maxReplicas: 2
      }
    }
  }
}

resource frontend 'Microsoft.App/containerApps@2026-01-01' = {
  name: '${containerAppName}-frontend'
  location: location

  properties: {
    managedEnvironmentId: environment.id

    configuration: {
      ingress: {
        external: true
        targetPort: 8080
        transport: 'auto'
      }
    }

    template: {
      containers: [
        {
          name: 'api'
          image: frontendImage
          env: [
            {
              name: 'BackendUrl'
              value: 'http://${backend.properties.configuration.ingress.fqdn}'
            }
          ]

          resources: {
            cpu: any('0.25')
            memory: '0.5Gi'
          }
        }
      ]

      scale: {
        minReplicas: 0
        maxReplicas: 2
      }
    }
  }
}
