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
        private Socket socket;
        private NetworkStream netStream;
        private SslStream sslStream;
        private SslProtocols sslProtocols;

        private bool disposed;
        private bool sslAuthenticated;

        private SecureSocket(SslProtocols sslProtocols)
        {
            this.sslProtocols = sslProtocols;
        }
        private SecureSocket(Socket socket, SslProtocols sslProtocols = SslProtocols.Tls)
            : this(sslProtocols)
        {
            this.socket = socket;
        }
        public SecureSocket(AddressFamily addressFamily, SocketType socketType, ProtocolType protocolType, SslProtocols sslProtocols = SslProtocols.Tls)
            : this(sslProtocols)
        {
            this.socket = new Socket(addressFamily, socketType, protocolType);
        }

        private void InitializeAsServer()
        {
            this.netStream = new NetworkStream(this.socket);
            this.sslStream = new SslStream(this.netStream, false, (a, b, c, d) =>
            {
                return true;
            });

            X509Certificate2 certificate = new X509Certificate2("C:/certs/Server.pfx");
            sslStream.AuthenticateAsServer(certificate, true, sslProtocols, true);
            sslAuthenticated = true;
        }
        private void InitializeAsClient(string targetHost)
        {
            this.netStream = new NetworkStream(this.socket);
            this.sslStream = new SslStream(this.netStream, false, (a, b, c, d) =>
            {
                return true;
            });

            X509CertificateCollection certificates = new X509CertificateCollection();
            certificates.Add(new X509Certificate2("C:/certs/Client.pfx"));
            sslStream.AuthenticateAsClient(targetHost, certificates, sslProtocols, true);
            sslAuthenticated = true;
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

        // Propriedades
        public AddressFamily AddressFamily
        {
            get
            {
                return this.socket.AddressFamily;
            }
        }
        public int Available
        {
            get
            {
                return this.socket.Available;
            }
        }
        public bool Blocking
        {
            get
            {
                return this.socket.Blocking;
            }
            set
            {
                this.socket.Blocking = value;
            }
        }
        public bool Connected
        {
            get
            {
                return this.socket.Connected;
            }
        }
        public bool DontFragment
        {
            get
            {
                return this.socket.DontFragment;
            }
            set
            {
                this.socket.DontFragment = value;
            }
        }
        public bool EnableBroadcast
        {
            get
            {
                return this.socket.EnableBroadcast;
            }
            set
            {
                this.socket.EnableBroadcast = value;
            }
        }
        public bool ExclusiveAddressUse
        {
            get
            {
                return this.socket.ExclusiveAddressUse;
            }
            set
            {
                this.socket.ExclusiveAddressUse = value;
            }
        }
        public IntPtr Handle
        {
            get
            {
                return this.socket.Handle;
            }
        }
        public bool IsBound
        {
            get
            {
                return this.socket.IsBound;
            }
        }
        public LingerOption LingerState
        {
            get
            {
                return this.socket.LingerState;
            }
            set
            {
                this.socket.LingerState = value;
            }
        }
        public EndPoint LocalEndPoint
        {
            get
            {
                return this.socket.LocalEndPoint;
            }
        }
        public bool MulticastLoopback
        {
            get
            {
                return this.socket.MulticastLoopback;
            }
            set
            {
                this.socket.MulticastLoopback = value;
            }
        }
        public bool NoDelay
        {
            get
            {
                return this.socket.NoDelay;
            }
            set
            {
                this.socket.NoDelay = value;
            }
        }
        public ProtocolType ProtocolType
        {
            get
            {
                return this.socket.ProtocolType;
            }
        }
        public int ReceiveBufferSize
        {
            get
            {
                return this.socket.ReceiveBufferSize;
            }
            set
            {
                this.socket.ReceiveBufferSize = value;
            }
        }
        public int ReceiveTimeout
        {
            get
            {
                return this.socket.ReceiveTimeout;
            }
            set
            {
                this.socket.ReceiveTimeout = value;
            }
        }
        public EndPoint RemoteEndPoint
        {
            get
            {
                return this.socket.RemoteEndPoint;
            }
        }
        public int SendBufferSize
        {
            get
            {
                return this.socket.SendBufferSize;
            }
            set
            {
                this.socket.SendBufferSize = value;
            }
        }
        public int SendTimeout
        {
            get
            {
                return this.socket.SendTimeout;
            }
            set
            {
                this.socket.SendTimeout = value;
            }
        }
        public SocketType SocketType
        {
            get
            {
                return this.socket.SocketType;
            }
        }
        public short Ttl
        {
            get
            {
                return this.socket.Ttl;
            }
            set
            {
                this.socket.Ttl = value;
            }
        }
        public bool UseOnlyOverlappedIO
        {
            get
            {
                return this.socket.UseOnlyOverlappedIO;
            }
            set
            {
                this.socket.UseOnlyOverlappedIO = value;
            }
        }

        public void Bind(EndPoint localEP)
        {
            this.socket.Bind(localEP);
        }
        public void Listen(int backlog)
        {
            this.socket.Listen(backlog);
        }

        public void Close()
        {
            this.socket.Close();
        }
        public void Close(int timeout)
        {
            this.socket.Close(timeout);
        }
        public void Disconnect(bool reuseSocket)
        {
            this.sslAuthenticated = false;
            this.socket.Disconnect(reuseSocket);
        }
        public void Dispose()
        {
            this.sslStream.Dispose();
            this.socket.Dispose();
            this.disposed = true;
        }
        public void Shutdown(SocketShutdown how)
        {
            this.socket.Shutdown(how);
        }

        public object GetSocketOption(SocketOptionLevel optionLevel, SocketOptionName optionName)
        {
            return this.socket.GetSocketOption(optionLevel, optionName);
        }
        public void GetSocketOption(SocketOptionLevel optionLevel, SocketOptionName optionName, byte[] optionValue)
        {
            this.socket.GetSocketOption(optionLevel, optionName, optionValue);
        }
        public byte[] GetSocketOption(SocketOptionLevel optionLevel, SocketOptionName optionName, int optionLength)
        {
            return this.socket.GetSocketOption(optionLevel, optionName, optionLength);
        }
        public int IOControl(int ioControlCode, byte[] optionInValue, byte[] optionOutValue)
        {
            return this.socket.IOControl(ioControlCode, optionInValue, optionOutValue);
        }
        public int IOControl(IOControlCode ioControlCode, byte[] optionInValue, byte[] optionOutValue)
        {
            return this.socket.IOControl(ioControlCode, optionInValue, optionOutValue);
        }

        public SecureSocket Accept()
        {
            Socket acceptedSocket = this.socket.Accept();
            SecureSocket secureSocket = new SecureSocket(acceptedSocket);
            secureSocket.InitializeAsServer();
            return secureSocket;
        }
        public Task<SecureSocket> AcceptAsync()
        {
            return new Task<SecureSocket>(Accept);
        }
        public IAsyncResult BeginAccept(AsyncCallback callback, object state)
        {
            return AcceptAsync().ToApm(callback, state);
        }
        public SecureSocket EndAccept(IAsyncResult asyncResult)
        {
            return ((Task<SecureSocket>)asyncResult).Result;
        }

        public void Connect(string host, int port)
        {
            this.socket.Connect(host, port);
            this.InitializeAsClient(host.ToString());
        }
        public Task<bool> ConnectAsync(string host, int port)
        {
            return new Task<bool>(() =>
            {
                Connect(host, port);
                return true;
            });
        }
        public IAsyncResult BeginConnect(string host, int port, AsyncCallback callback, object state)
        {
            return ConnectAsync(host, port).ToApm(callback, state);
        }
        public bool EndConnect(IAsyncResult asyncResult)
        {
            return ((Task<bool>)asyncResult).Result;
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
            return new Task<int>(() => Send(buffer, offset, size));
        }
        public IAsyncResult BeginSend(byte[] buffer, int offset, int size, AsyncCallback callback, object state)
        {
            return SendAsync(buffer, offset, size).ToApm(callback, state);
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

        // Coisas UDP
        //public int ReceiveFrom(byte[] buffer, ref EndPoint remoteEP);
        //public bool SendPacketsAsync(SocketAsyncEventArgs e);
        //public int SendTo(byte[] buffer, EndPoint remoteEP);

        //public void SendFile(string fileName);
        //public void SendFile(string fileName, byte[] preBuffer, byte[] postBuffer, TransmitFileOptions flags);

        //public int EndSendTo(IAsyncResult asyncResult);
        //public bool SendToAsync(SocketAsyncEventArgs e);        

        // Coisas Assíncronas
        //public bool ReceiveMessageFromAsync(SocketAsyncEventArgs e);
        //public bool ReceiveAsync(SocketAsyncEventArgs e);
        //public bool ReceiveFromAsync(SocketAsyncEventArgs e);
        //public bool AcceptAsync(SocketAsyncEventArgs e);
        //public IAsyncResult BeginAccept(AsyncCallback callback, object state);
        //[TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
        //public IAsyncResult BeginAccept(int receiveSize, AsyncCallback callback, object state);
        //public IAsyncResult BeginAccept(Socket acceptSocket, int receiveSize, AsyncCallback callback, object state);
        //public IAsyncResult BeginConnect(EndPoint remoteEP, AsyncCallback callback, object state);
        //public IAsyncResult BeginConnect(IPAddress address, int port, AsyncCallback requestCallback, object state);
        //public IAsyncResult BeginConnect(IPAddress[] addresses, int port, AsyncCallback requestCallback, object state);
        //public IAsyncResult BeginConnect(string host, int port, AsyncCallback requestCallback, object state);
        //public IAsyncResult BeginDisconnect(bool reuseSocket, AsyncCallback callback, object state);
        //public IAsyncResult BeginSendFile(string fileName, AsyncCallback callback, object state);
        //public IAsyncResult BeginSendFile(string fileName, byte[] preBuffer, byte[] postBuffer, TransmitFileOptions flags, AsyncCallback callback, object state);
        //public Socket EndAccept(IAsyncResult asyncResult);
        //public Socket EndAccept(out byte[] buffer, IAsyncResult asyncResult);
        //public Socket EndAccept(out byte[] buffer, out int bytesTransferred, IAsyncResult asyncResult);
        //public void EndConnect(IAsyncResult asyncResult);
        //public void EndDisconnect(IAsyncResult asyncResult);
        //public int EndReceive(IAsyncResult asyncResult);
        //public int EndReceive(IAsyncResult asyncResult, out SocketError errorCode);
        //public int EndReceiveFrom(IAsyncResult asyncResult, ref EndPoint endPoint);
        //public int EndSend(IAsyncResult asyncResult, out SocketError errorCode);
        //public void EndSendFile(IAsyncResult asyncResult);
        //public bool SendAsync(SocketAsyncEventArgs e);
        //public static void CancelConnectAsync(SocketAsyncEventArgs e);
        //public bool ConnectAsync(SocketAsyncEventArgs e);
        //public static bool ConnectAsync(SocketType socketType, ProtocolType protocolType, SocketAsyncEventArgs e);
        //public bool DisconnectAsync(SocketAsyncEventArgs e);
    }
}
