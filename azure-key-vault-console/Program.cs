using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace azure_key_vault_console
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Azure Key Vault demo Started!");

            Console.WriteLine();
            Console.WriteLine("*********** Using DI ************");
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            var diContainer = new ServiceCollection();
            diContainer.AddSingleton<ISecurityVault, AzureKeyVault>();
            diContainer.AddSingleton<IConfigurationRoot>(builder.Build());
            diContainer.AddSingleton<SomeClass, SomeClass>(); // Added to show constructor injection

            var diProvider = diContainer.BuildServiceProvider();
            var securityVault = diProvider.GetService<ISecurityVault>(); 
            var someObject = diProvider.GetService<SomeClass>(); // The above will often be injected in the constructor
            //var secret = securityVault.GetSecret("my-secret");            
            //Console.WriteLine($"secret={secret}");
            var secret = File.ReadAllText($"..\\local\\rsa-pair1-prv-key.key");
            securityVault.CreateSecret("sample-key", secret);
            var rsaPrivateKey = securityVault.GetSecret("sample-key");            
            Console.WriteLine($"key={rsaPrivateKey}");

            Console.WriteLine();
            Console.WriteLine("*********** Not using DI ************");
            var rsaPrivateKeyNonDI = new AzureKeyVault(builder.Build()).GetSecret("rsa-pair1-prv-key");
            Console.WriteLine($"key={rsaPrivateKeyNonDI}");

            Console.WriteLine("Azure Key Vault demo Ended!");
        }
    }
}
