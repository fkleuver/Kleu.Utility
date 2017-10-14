using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using Kleu.Utility.Logging;

namespace Kleu.Utility.Identity.Cryptography
{
    public static class CertificateLoader
    {
        private static ILog _logger;
        private static ILog Logger => _logger ?? (_logger = LogProvider.GetCurrentClassLogger());

        public static X509Certificate2 GetFromMachineStoreBySN(string subjectName)
        {
            return GetFromStore(StoreLocation.LocalMachine, subjectName);
        }
        public static X509Certificate2 GetFromMachineStoreByThumbprint(string thumbPrint)
        {
            return GetFromStore(StoreLocation.LocalMachine, null, thumbPrint);
        }

        public static X509Certificate2 GetFromUserStore(string subjectName)
        {
            return GetFromStore(StoreLocation.CurrentUser, subjectName);
        }

        private static X509Certificate2 GetFromStore(StoreLocation storeLocation, string subjectName, string thumbPrint = null)
        {
            var certificate = new X509Certificate2();
            var store = new X509Store(StoreName.My, storeLocation);
            store.Open(OpenFlags.ReadOnly);
            try
            {
                if (thumbPrint == null)
                {
                    Logger.Info($"Retrieving certificate (sn: {subjectName}) from store: {storeLocation}");
                    var certificatesInStore = store.Certificates.Find(X509FindType.FindBySubjectName, subjectName, false);
                    if (certificatesInStore.Count > 0)
                    {
                        certificate = certificatesInStore[0];
                    }

                }
                else
                {
                    Logger.Info($"Retrieving certificate (thumbprint: {thumbPrint}) from store: {storeLocation}");
                    var certificatesInStore = store.Certificates.Find(X509FindType.FindByThumbprint, thumbPrint, false);
                    if (certificatesInStore.Count > 0)
                    {
                        certificate = certificatesInStore[0];
                    }
                }
            }
            catch (CryptographicException ex)
            {
                Logger.ErrorException($"Failed to load certificate subject {subjectName} from store {storeLocation}", ex);
            }
            finally
            {
                store.Close();
            }

            return certificate;
        } 
        public static X509Certificate2 GetTestCertificate()
        {
            var assembly = typeof(CertificateLoader).Assembly;
            using (var stream = assembly.GetManifestResourceStream("Dnk.Core.Security.Cryptography.idsrv3test.pfx"))
            {
                return new X509Certificate2(ReadStream(stream), "idsrv3test");
            }
        }

        private static byte[] ReadStream(Stream input)
        {
            var buffer = new byte[16 * 1024];
            using (var ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }
    }
}
