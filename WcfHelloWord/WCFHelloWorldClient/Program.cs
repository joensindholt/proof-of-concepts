using System;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.Threading;
using System.Threading.Tasks;

namespace WCFHelloWorldClient
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            Console.WriteLine("Start");

            var binding = new BasicHttpBinding();
            //binding.Security.Mode = BasicHttpSecurityMode.Message;
            //binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Certificate;

            var client = new HelloWorldServiceReference.HelloWorldServiceClient(binding, new EndpointAddress("http://localhost:8080/basichttp"));
            //client.ClientCredentials.ClientCertificate.SetCertificate("CN=localhost", StoreLocation.LocalMachine, StoreName.My);

            Console.WriteLine("Calling service");
            var result = await client.SayHelloAsync("John Doe");

            Console.WriteLine($"Result was: {result}");
        }
    }
}