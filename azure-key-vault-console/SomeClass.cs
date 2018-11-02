using System;

namespace azure_key_vault_console
{
    public class SomeClass
    {
        private ISecurityVault _securityVault;
        public SomeClass(ISecurityVault securityVault) {
            _securityVault = securityVault;
            Console.WriteLine($"vaultType={_securityVault.ToString()}");
        }
    }
}