using System;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.ServiceModel.Security;
using System.Threading.Tasks;

namespace WCFHelloWorldClientFullFramework
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            Console.WriteLine("Start");

            var binding = new BasicHttpBinding();
            binding.Security.Mode = BasicHttpSecurityMode.Message;
            binding.Security.Message.ClientCredentialType = BasicHttpMessageCredentialType.Certificate;

            var client = new HelloWorldServiceReference.HelloWorldServiceClient(binding, new EndpointAddress("http://localhost:8080/hello"));
            client.ClientCredentials.ClientCertificate.SetCertificate("CN=localhost", StoreLocation.LocalMachine, StoreName.My);
            client.ClientCredentials.ServiceCertificate.SetDefaultCertificate("CN=localhost", StoreLocation.LocalMachine, StoreName.My);

            // Allow self created certificates for testing purposes
            client.ClientCredentials.ServiceCertificate.Authentication.CertificateValidationMode = X509CertificateValidationMode.None;

            Console.WriteLine("Calling service");
            var result = await client.SayHelloAsync("John Doe");

            Console.WriteLine($"Result was: {result}");
            Console.ReadKey();
        }
    }
}