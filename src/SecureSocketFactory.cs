using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;

namespace SecureSockets
{
    public class SecureSocketFactory
    {
        public static SecureSocket CreateUnsecureSocket(AddressFamily addressFamily, SocketType socketType, ProtocolType protocolType)
        {
            return new SecureSocket(false, addressFamily, socketType, protocolType);
        }
        public static SecureSocket CreateSecureSocket(AddressFamily addressFamily, SocketType socketType, ProtocolType protocolType, X509Certificate2 certificate)
        {
            return new SecureSocket(true, addressFamily, socketType, protocolType)
            {
                clientCertificate = certificate,
                serverCertificate = certificate
            };
        }

    }
}
