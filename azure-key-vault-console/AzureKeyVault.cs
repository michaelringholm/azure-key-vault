
using System;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Extensions.Configuration;

namespace azure_key_vault_console
{
    // https://docs.microsoft.com/en-us/azure/key-vault/key-vault-developers-guide
    // https://docs.microsoft.com/en-us/azure/key-vault/service-to-service-authentication
    //
    // Use az login when running locally and MI (Managed Identity) in Azure
    public class AzureKeyVault : ISecurityVault
    {
        private IConfiguration _configuration;
        private IKeyVaultClient _keyVault;
        private string _keyVaultUrl;

        public AzureKeyVault(IConfigurationRoot configuration) {
            _configuration = configuration;
            var tokenProvider = new AzureServiceTokenProvider();            
            _keyVault = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(tokenProvider.KeyVaultTokenCallback));            
            _keyVaultUrl = _configuration.GetSection("key-vault:url").Value;
            if(String.IsNullOrEmpty(_keyVaultUrl))
                throw new Exception("Please add the url of the key vault in appsettings.json file as key-vault with property url.");            
        }

        public string GetKey(string keyName)
        {
            var key = _keyVault.GetKeyAsync(_keyVaultUrl, keyName).ConfigureAwait(false).GetAwaiter().GetResult();
            return key.Key.ToString();
        }

        public string GetSecret(string secretName)
        {
            var secret = _keyVault.GetSecretAsync(_keyVaultUrl, secretName).ConfigureAwait(false).GetAwaiter().GetResult();
            return secret.Value;
        }
    }
}