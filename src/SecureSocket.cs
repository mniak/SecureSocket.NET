using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Runtime;
using System.Collections;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Security.Authentication;
using System.Threading.Tasks;
using System.Threading;

namespace SecureSockets
{
    public class SecureSocket : IDisposable
    {
        private Socket innerSocket;
        private NetworkStream netStream;
        private SslStream sslStream;
        private SslProtocols sslProtocols;

        private bool disposed;
        private bool sslAuthenticated;

        private X509Certificate2 serverCertificate;
        private X509Certificate2 clientCertificate;

        private SecureSocket(SslProtocols sslProtocols)
        {
            if (sslProtocols == SslProtocols.None)
                throw new ArgumentException("The protocol cannot be None", "sslProtocols");

            this.sslProtocols = sslProtocols;
        }
        private SecureSocket(Socket socket, SslProtocols sslProtocols = SslProtocols.Tls)
            : this(sslProtocols)
        {
            this.innerSocket = socket;
        }
        public SecureSocket(AddressFamily addressFamily, SocketType socketType, ProtocolType protocolType, SslProtocols sslProtocols = SslProtocols.Tls)
            : this(sslProtocols)
        {
            this.innerSocket = new Socket(addressFamily, socketType, protocolType);
        }

        private void InitializeAsServer()
        {
            this.netStream = new NetworkStream(this.innerSocket);
            this.sslStream = new SslStream(this.netStream, false, (a, b, c, d) =>
            {
                return true;
            });

            X509Certificate2 certificate = new X509Certificate2(serverCertificate);
            sslStream.AuthenticateAsServer(certificate, true, sslProtocols, true);
            sslAuthenticated = true;
        }
        private void InitializeAsClient(string targetHost)
        {
            this.netStream = new NetworkStream(this.innerSocket);
            this.sslStream = new SslStream(this.netStream, false, (a, b, c, d) =>
            {
                return true;
            });

            X509CertificateCollection certificates = new X509CertificateCollection();
            certificates.Add(clientCertificate);
            sslStream.AuthenticateAsClient(targetHost, certificates, sslProtocols, true);
            sslAuthenticated = true;
        }

        public void LoadServerCertificate(X509Certificate2 certificate)
        {
            this.serverCertificate = certificate;
        }
        public void LoadClientCertificate(X509Certificate2 certificate)
        {
            this.clientCertificate = certificate;
        }

        private void CheckDisposed()
        {
            if (this.disposed)
                throw new ObjectDisposedException(base.GetType().FullName);
        }
        private void CheckConnected()
        {
            if (!sslAuthenticated)
                throw new InvalidOperationException("This socket is not connected.");
        }

        public static bool OSSupportsIPv4
        {
            get
            {
                return Socket.OSSupportsIPv4;
            }
        }
        public static bool OSSupportsIPv6
        {
            get
            {
                return Socket.OSSupportsIPv6;
            }
        }

        // Properties
        public AddressFamily AddressFamily
        {
            get
            {
                return this.innerSocket.AddressFamily;
            }
        }
        public int Available
        {
            get
            {
                return this.innerSocket.Available;
            }
        }
        public bool Blocking
        {
            get
            {
                return this.innerSocket.Blocking;
            }
            set
            {
                this.innerSocket.Blocking = value;
            }
        }
        public bool Connected
        {
            get
            {
                return this.innerSocket.Connected;
            }
        }
        public bool DontFragment
        {
            get
            {
                return this.innerSocket.DontFragment;
            }
            set
            {
                this.innerSocket.DontFragment = value;
            }
        }
        public bool EnableBroadcast
        {
            get
            {
                return this.innerSocket.EnableBroadcast;
            }
            set
            {
                this.innerSocket.EnableBroadcast = value;
            }
        }
        public bool ExclusiveAddressUse
        {
            get
            {
                return this.innerSocket.ExclusiveAddressUse;
            }
            set
            {
                this.innerSocket.ExclusiveAddressUse = value;
            }
        }
        public IntPtr Handle
        {
            get
            {
                return this.innerSocket.Handle;
            }
        }
        public bool IsBound
        {
            get
            {
                return this.innerSocket.IsBound;
            }
        }
        public LingerOption LingerState
        {
            get
            {
                return this.innerSocket.LingerState;
            }
            set
            {
                this.innerSocket.LingerState = value;
            }
        }
        public EndPoint LocalEndPoint
        {
            get
            {
                return this.innerSocket.LocalEndPoint;
            }
        }
        public bool MulticastLoopback
        {
            get
            {
                return this.innerSocket.MulticastLoopback;
            }
            set
            {
                this.innerSocket.MulticastLoopback = value;
            }
        }
        public bool NoDelay
        {
            get
            {
                return this.innerSocket.NoDelay;
            }
            set
            {
                this.innerSocket.NoDelay = value;
            }
        }
        public ProtocolType ProtocolType
        {
            get
            {
                return this.innerSocket.ProtocolType;
            }
        }
        public int ReceiveBufferSize
        {
            get
            {
                return this.innerSocket.ReceiveBufferSize;
            }
            set
            {
                this.innerSocket.ReceiveBufferSize = value;
            }
        }
        public int ReceiveTimeout
        {
            get
            {
                return this.innerSocket.ReceiveTimeout;
            }
            set
            {
                this.innerSocket.ReceiveTimeout = value;
            }
        }
        public EndPoint RemoteEndPoint
        {
            get
            {
                return this.innerSocket.RemoteEndPoint;
            }
        }
        public int SendBufferSize
        {
            get
            {
                return this.innerSocket.SendBufferSize;
            }
            set
            {
                this.innerSocket.SendBufferSize = value;
            }
        }
        public int SendTimeout
        {
            get
            {
                return this.innerSocket.SendTimeout;
            }
            set
            {
                this.innerSocket.SendTimeout = value;
            }
        }
        public SocketType SocketType
        {
            get
            {
                return this.innerSocket.SocketType;
            }
        }
        public short Ttl
        {
            get
            {
                return this.innerSocket.Ttl;
            }
            set
            {
                this.innerSocket.Ttl = value;
            }
        }
        public bool UseOnlyOverlappedIO
        {
            get
            {
                return this.innerSocket.UseOnlyOverlappedIO;
            }
            set
            {
                this.innerSocket.UseOnlyOverlappedIO = value;
            }
        }

        public void Bind(EndPoint localEP)
        {
            this.innerSocket.Bind(localEP);
        }
        public void Listen(int backlog)
        {
            this.innerSocket.Listen(backlog);
        }

        public void Close()
        {
            this.innerSocket.Close();
        }
        public void Close(int timeout)
        {
            this.innerSocket.Close(timeout);
        }
        public void Disconnect(bool reuseSocket)
        {
            this.sslAuthenticated = false;
            this.innerSocket.Disconnect(reuseSocket);
        }
        public void Dispose()
        {
            this.sslStream.Dispose();
            this.innerSocket.Dispose();
            this.disposed = true;
        }
        public void Shutdown(SocketShutdown how)
        {
            this.innerSocket.Shutdown(how);
        }

        public object GetSocketOption(SocketOptionLevel optionLevel, SocketOptionName optionName)
        {
            return this.innerSocket.GetSocketOption(optionLevel, optionName);
        }
        public void GetSocketOption(SocketOptionLevel optionLevel, SocketOptionName optionName, byte[] optionValue)
        {
            this.innerSocket.GetSocketOption(optionLevel, optionName, optionValue);
        }
        public byte[] GetSocketOption(SocketOptionLevel optionLevel, SocketOptionName optionName, int optionLength)
        {
            return this.innerSocket.GetSocketOption(optionLevel, optionName, optionLength);
        }
        public int IOControl(int ioControlCode, byte[] optionInValue, byte[] optionOutValue)
        {
            return this.innerSocket.IOControl(ioControlCode, optionInValue, optionOutValue);
        }
        public int IOControl(IOControlCode ioControlCode, byte[] optionInValue, byte[] optionOutValue)
        {
            return this.innerSocket.IOControl(ioControlCode, optionInValue, optionOutValue);
        }

        public SecureSocket Accept()
        {
            if (serverCertificate == null)
                throw new InvalidOperationException("The client certificate must be loaded before using this operation");

            Socket acceptedSocket = this.innerSocket.Accept();
            SecureSocket secureSocket = new SecureSocket(acceptedSocket);
            secureSocket.InitializeAsServer();
            return secureSocket;
        }
        public Task<SecureSocket> AcceptAsync()
        {
            return AsyncExtensions.ToStartedTask(Accept);
        }
        public IAsyncResult BeginAccept(AsyncCallback callback, object state)
        {
            return AcceptAsync().AsApm(callback, state);
        }
        public SecureSocket EndAccept(IAsyncResult asyncResult)
        {
            return ((Task<SecureSocket>)asyncResult).Result;
        }

        public void Connect(string host, int port)
        {
            if (clientCertificate == null)
                throw new InvalidOperationException("The client certificate must be loaded before using this operation");

            this.innerSocket.Connect(host, port);
            this.InitializeAsClient(host.ToString());
        }
        public Task<bool> ConnectAsync(string host, int port)
        {
            return AsyncExtensions.ToStartedTask(() =>
            {
                Connect(host, port);
                return true;
            });
        }
        public IAsyncResult BeginConnect(string host, int port, AsyncCallback callback, object state)
        {
            return ConnectAsync(host, port).AsApm(callback, state);
        }
        public bool EndConnect(IAsyncResult asyncResult)
        {
            return asyncResult.GetResult<bool>();
        }

        public int Send(byte[] buffer, int offset, int size)
        {
            CheckConnected();
            CheckDisposed();
            this.sslStream.Write(buffer, offset, size);
            this.sslStream.Flush();
            return size;
        }
        public int Send(byte[] buffer)
        {
            return Send(buffer, 0, buffer.Length);
        }
        private Task<int> SendAsync(byte[] buffer, int offset, int size)
        {
            return AsyncExtensions.ToStartedTask(() => Send(buffer, offset, size));
        }
        public IAsyncResult BeginSend(byte[] buffer, int offset, int size, AsyncCallback callback, object state)
        {
            return SendAsync(buffer, offset, size).AsApm(callback, state);
        }
        public int EndSend(IAsyncResult asyncResult)
        {
            return ((Task<int>)asyncResult).Result;
        }

        public int Receive(byte[] buffer, int offset, int count)
        {
            CheckConnected();
            CheckDisposed();
            return this.sslStream.Read(buffer, offset, count);
        }
        public int Receive(byte[] buffer)
        {
            return Receive(buffer, 0, buffer.Length);
        }
        private Task<int> ReceiveAsync(byte[] buffer, int offset, int size)
        {
            return AsyncExtensions.ToStartedTask(() => Receive(buffer, offset, size));
        }
        public IAsyncResult BeginReceive(byte[] buffer, int offset, int size, AsyncCallback callback, object state)
        {
            return ReceiveAsync(buffer, offset, size).AsApm(callback, state);
        }
        public int EndReceive(IAsyncResult asyncResult)
        {
            return ((Task<int>)asyncResult).Result;
        }

        // TODO:
        //public int ReceiveFrom(byte[] buffer, ref EndPoint remoteEP);
        //public bool SendPacketsAsync(SocketAsyncEventArgs e);
        //public int SendTo(byte[] buffer, EndPoint remoteEP);

        //public void SendFile(string fileName);
        //public void SendFile(string fileName, byte[] preBuffer, byte[] postBuffer, TransmitFileOptions flags);

        //public int EndSendTo(IAsyncResult asyncResult);
        //public bool SendToAsync(SocketAsyncEventArgs e);        

        //public bool ReceiveMessageFromAsync(SocketAsyncEventArgs e);
        //public bool ReceiveFromAsync(SocketAsyncEventArgs e);
        //public bool AcceptAsync(SocketAsyncEventArgs e);
        //public IAsyncResult BeginAccept(AsyncCallback callback, object state);
        //public IAsyncResult BeginDisconnect(bool reuseSocket, AsyncCallback callback, object state);
        //public IAsyncResult BeginSendFile(string fileName, AsyncCallback callback, object state);
        //public IAsyncResult BeginSendFile(string fileName, byte[] preBuffer, byte[] postBuffer, TransmitFileOptions flags, AsyncCallback callback, object state);
        //public void EndDisconnect(IAsyncResult asyncResult);
        //public void EndSendFile(IAsyncResult asyncResult);
        //public static void CancelConnectAsync(SocketAsyncEventArgs e);
        //public bool DisconnectAsync(SocketAsyncEventArgs e);
    }
}
