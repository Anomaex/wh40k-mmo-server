using System;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace AuthServer.Configuration
{
    internal static class Config
    {
        internal static ConfigFileData Data { get; private set; }
        internal static X509Certificate2 X509Certificate {  get; private set; }


        internal static bool Start()
        {
            Data = new();
            CreateCertificate();
            return true;
        }

        private static void CreateCertificate()
        {
            RSA rsa = RSA.Create(3072);
            X500DistinguishedName name = new("CN=example.com,O=WH40K_MMO_Server,C=UA");
            CertificateRequest cr = new(name, rsa, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
            X509Certificate2 crt = cr.CreateSelfSigned(DateTimeOffset.Now, DateTimeOffset.Now.AddYears(1));
            X509Certificate = new(crt.Export(X509ContentType.Pfx));
        }
    }
}
