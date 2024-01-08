using System;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Security;

namespace WCFHelloWorldServiceFullFramework
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Uri baseAddress = new Uri("http://localhost:8080/hello");

            // Create the ServiceHost.
            using (ServiceHost host = new ServiceHost(typeof(HelloWorldService), baseAddress))
            {
                // Enable metadata publishing.
                ServiceMetadataBehavior smb = new ServiceMetadataBehavior();
                smb.HttpGetEnabled = true;
                smb.MetadataExporter.PolicyVersion = PolicyVersion.Policy15;
                host.Description.Behaviors.Add(smb);

                var binding = new BasicHttpBinding();
                binding.Security.Mode = BasicHttpSecurityMode.Message;
                binding.Security.Message.ClientCredentialType = BasicHttpMessageCredentialType.Certificate;

                host.AddServiceEndpoint(typeof(IHelloWorldService), binding, "");

                host.Credentials.ServiceCertificate.SetCertificate(
                    StoreLocation.LocalMachine,
                    StoreName.My,
                    X509FindType.FindBySubjectName,
                    "localhost");

                // Allow self created certificates for testing purposes
                host.Credentials.ClientCertificate.Authentication.CertificateValidationMode = X509CertificateValidationMode.None;

                host.Open();

                Console.WriteLine("The service is ready at {0}", baseAddress);
                Console.WriteLine("Press <Enter> to stop the service.");
                Console.ReadLine();

                // Close the ServiceHost.
                host.Close();
            }
        }
    }
}