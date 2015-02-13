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

namespace SecureSockets
{
    public class TcpSecureSocket
    {
        private Socket socket;
        private NetworkStream netStream;
        private SslStream sslStream;

        private TcpSecureSocket(Socket socket)
        {
            this.socket = socket;
        }
        //public TcpSecureSocket(SocketInformation socketInformation)
        //{
        //    this.socket = new Socket(socketInformation);
        //}
        public TcpSecureSocket(AddressFamily addressFamily, SocketType socketType, ProtocolType protocolType)
        {
            this.socket = new Socket(addressFamily, socketType, protocolType);
        }

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
        [Obsolete("SupportsIPv4 is obsoleted for this type, please use OSSupportsIPv4 instead. http://go.microsoft.com/fwlink/?linkid=14202")]
        public static bool SupportsIPv4
        {
            get
            {
                return Socket.SupportsIPv4;
            }
        }
        [Obsolete("SupportsIPv6 is obsoleted for this type, please use OSSupportsIPv6 instead. http://go.microsoft.com/fwlink/?linkid=14202")]
        public static bool SupportsIPv6
        {
            get
            {
                return Socket.SupportsIPv6;
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

        public TcpSecureSocket Accept()
        {
            Socket acceptedSocket = this.socket.Accept();
            TcpSecureSocket secureSocket = new TcpSecureSocket(acceptedSocket);
            secureSocket.InitializeAsServer();
            return secureSocket;
        }


        private void InitializeAsServer()
        {
            this.netStream = new NetworkStream(this.socket);
            this.sslStream = new SslStream(this.netStream);

            X509Certificate2 certificate = new X509Certificate2("C:/certs/LabInovacao.pfx");
            sslStream.AuthenticateAsServer(certificate, true, SslProtocols.Tls, true);
        }


        public void Bind(EndPoint localEP)
        {
            this.socket.Bind(localEP);
        }
        public void Close()
        {
            this.socket.Close();
        }
        public void Close(int timeout)
        {
            this.socket.Close(timeout);
        }
        public void Connect(EndPoint remoteEP)
        {
            this.socket.Connect(remoteEP);
        }
        public void Connect(IPAddress address, int port)
        {
            this.socket.Connect(address, port);
        }
        public void Connect(IPAddress[] addresses, int port)
        {
            this.socket.Connect(addresses, port);
        }
        public void Connect(string host, int port)
        {
            this.socket.Connect(host, port);
        }
        public void Disconnect(bool reuseSocket)
        {
            this.socket.Disconnect(reuseSocket);
        }
        public void Dispose()
        {
            this.sslStream.Dispose();
            this.socket.Dispose();
        }
        protected virtual void Dispose(bool disposing)
        {
            this.sslStream.Dispose();
            this.socket.Dispose();
        }
        public SocketInformation DuplicateAndClose(int targetProcessId)
        {
            return this.socket.DuplicateAndClose(targetProcessId);
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
        public void Listen(int backlog)
        {
            this.socket.Listen(backlog);
        }
        public bool Poll(int microSeconds, SelectMode mode)
        {
            return Poll(microSeconds, mode);
        }
        public int Receive(byte[] buffer)
        {
            return this.socket.Receive(buffer);
        }
        //[TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
        //public int Receive(IList<ArraySegment<byte>> buffers)
        //{
        //    return this.socket.Receive(buffers);
        //}
        //public int Receive(byte[] buffer, SocketFlags socketFlags)
        //{
        //    return this.socket.Receive(buffer, socketFlags);
        //}
        //public int Receive(IList<ArraySegment<byte>> buffers, SocketFlags socketFlags)
        //{
        //    return this.socket.Receive(buffers, socketFlags);
        //}
        //[TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
        //public int Receive(byte[] buffer, int size, SocketFlags socketFlags)
        //{
        //    return this.socket.Receive(buffer, size, socketFlags);
        //}
        //public int Receive(IList<ArraySegment<byte>> buffers, SocketFlags socketFlags, out SocketError errorCode)
        //{
        //    return this.socket.Receive(buffers, socketFlags, out errorCode);
        //}
        //public int Receive(byte[] buffer, int offset, int size, SocketFlags socketFlags)
        //{
        //    return this.socket.Receive(buffer, socketFlags);
        //}
        //public int Receive(byte[] buffer, int offset, int size, SocketFlags socketFlags, out SocketError errorCode)
        //{
        //    return this.socket.Receive(buffer, offset, size, socketFlags, out errorCode);
        //}

        //public int ReceiveFrom(byte[] buffer, ref EndPoint remoteEP) { 
        //return this.socket.ReceiveFrom(buffer, 
        //}
        //public int ReceiveFrom(byte[] buffer, SocketFlags socketFlags, ref EndPoint remoteEP);
        //[TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
        //public int ReceiveFrom(byte[] buffer, int size, SocketFlags socketFlags, ref EndPoint remoteEP);
        //public int ReceiveFrom(byte[] buffer, int offset, int size, SocketFlags socketFlags, ref EndPoint remoteEP);
        //public int ReceiveMessageFrom(byte[] buffer, int offset, int size, ref SocketFlags socketFlags, ref EndPoint remoteEP, out IPPacketInformation ipPacketInformation);
        //public bool ReceiveMessageFromAsync(SocketAsyncEventArgs e);
        //public static void Select(IList checkRead, IList checkWrite, IList checkError, int microSeconds);
        //public int Send(byte[] buffer);
        //[TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
        //public int Send(IList<ArraySegment<byte>> buffers);
        //public int Send(byte[] buffer, SocketFlags socketFlags);
        //public int Send(IList<ArraySegment<byte>> buffers, SocketFlags socketFlags);
        //[TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
        //public int Send(byte[] buffer, int size, SocketFlags socketFlags);
        //public int Send(IList<ArraySegment<byte>> buffers, SocketFlags socketFlags, out SocketError errorCode);
        //public int Send(byte[] buffer, int offset, int size, SocketFlags socketFlags);
        //public int Send(byte[] buffer, int offset, int size, SocketFlags socketFlags, out SocketError errorCode);

        //public void SendFile(string fileName);
        //public void SendFile(string fileName, byte[] preBuffer, byte[] postBuffer, TransmitFileOptions flags);
        //public bool SendPacketsAsync(SocketAsyncEventArgs e);
        //public int SendTo(byte[] buffer, EndPoint remoteEP);
        //public int SendTo(byte[] buffer, SocketFlags socketFlags, EndPoint remoteEP);
        //[TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
        //public int SendTo(byte[] buffer, int size, SocketFlags socketFlags, EndPoint remoteEP);
        //public int SendTo(byte[] buffer, int offset, int size, SocketFlags socketFlags, EndPoint remoteEP);

        //public void SetIPProtectionLevel(IPProtectionLevel level);
        //public void SetSocketOption(SocketOptionLevel optionLevel, SocketOptionName optionName, bool optionValue);
        //public void SetSocketOption(SocketOptionLevel optionLevel, SocketOptionName optionName, byte[] optionValue);
        //public void SetSocketOption(SocketOptionLevel optionLevel, SocketOptionName optionName, int optionValue);
        //public void SetSocketOption(SocketOptionLevel optionLevel, SocketOptionName optionName, object optionValue);
        //public void Shutdown(SocketShutdown how);

        //public bool ReceiveAsync(SocketAsyncEventArgs e);
        //public bool ReceiveFromAsync(SocketAsyncEventArgs e);
        //public bool AcceptAsync(SocketAsyncEventArgs e);
        public IAsyncResult BeginAccept(AsyncCallback callback, object state)
        {
            this.socket.BeginAccept(callback(
        }
        //[TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
        //public IAsyncResult BeginAccept(int receiveSize, AsyncCallback callback, object state);
        //public IAsyncResult BeginAccept(Socket acceptSocket, int receiveSize, AsyncCallback callback, object state);
        //public IAsyncResult BeginConnect(EndPoint remoteEP, AsyncCallback callback, object state);
        //public IAsyncResult BeginConnect(IPAddress address, int port, AsyncCallback requestCallback, object state);
        //public IAsyncResult BeginConnect(IPAddress[] addresses, int port, AsyncCallback requestCallback, object state);
        //public IAsyncResult BeginConnect(string host, int port, AsyncCallback requestCallback, object state);
        //public IAsyncResult BeginDisconnect(bool reuseSocket, AsyncCallback callback, object state);
        //public IAsyncResult BeginReceive(IList<ArraySegment<byte>> buffers, SocketFlags socketFlags, AsyncCallback callback, object state);
        //public IAsyncResult BeginReceive(IList<ArraySegment<byte>> buffers, SocketFlags socketFlags, out SocketError errorCode, AsyncCallback callback, object state);
        //public IAsyncResult BeginReceive(byte[] buffer, int offset, int size, SocketFlags socketFlags, AsyncCallback callback, object state);
        //public IAsyncResult BeginReceive(byte[] buffer, int offset, int size, SocketFlags socketFlags, out SocketError errorCode, AsyncCallback callback, object state);
        //public IAsyncResult BeginReceiveFrom(byte[] buffer, int offset, int size, SocketFlags socketFlags, ref EndPoint remoteEP, AsyncCallback callback, object state);
        //public IAsyncResult BeginReceiveMessageFrom(byte[] buffer, int offset, int size, SocketFlags socketFlags, ref EndPoint remoteEP, AsyncCallback callback, object state);
        //public IAsyncResult BeginSend(IList<ArraySegment<byte>> buffers, SocketFlags socketFlags, AsyncCallback callback, object state);
        //public IAsyncResult BeginSend(IList<ArraySegment<byte>> buffers, SocketFlags socketFlags, out SocketError errorCode, AsyncCallback callback, object state);
        //public IAsyncResult BeginSend(byte[] buffer, int offset, int size, SocketFlags socketFlags, AsyncCallback callback, object state);
        //public IAsyncResult BeginSend(byte[] buffer, int offset, int size, SocketFlags socketFlags, out SocketError errorCode, AsyncCallback callback, object state);
        //public IAsyncResult BeginSendFile(string fileName, AsyncCallback callback, object state);
        //public IAsyncResult BeginSendFile(string fileName, byte[] preBuffer, byte[] postBuffer, TransmitFileOptions flags, AsyncCallback callback, object state);
        //public IAsyncResult BeginSendTo(byte[] buffer, int offset, int size, SocketFlags socketFlags, EndPoint remoteEP, AsyncCallback callback, object state);
        //public Socket EndAccept(IAsyncResult asyncResult);
        //public Socket EndAccept(out byte[] buffer, IAsyncResult asyncResult);
        //public Socket EndAccept(out byte[] buffer, out int bytesTransferred, IAsyncResult asyncResult);
        //public void EndConnect(IAsyncResult asyncResult);
        //public void EndDisconnect(IAsyncResult asyncResult);
        //public int EndReceive(IAsyncResult asyncResult);
        //public int EndReceive(IAsyncResult asyncResult, out SocketError errorCode);
        //public int EndReceiveFrom(IAsyncResult asyncResult, ref EndPoint endPoint);
        //public int EndReceiveMessageFrom(IAsyncResult asyncResult, ref SocketFlags socketFlags, ref EndPoint endPoint, out IPPacketInformation ipPacketInformation);
        //public int EndSend(IAsyncResult asyncResult);
        //public int EndSend(IAsyncResult asyncResult, out SocketError errorCode);
        //public void EndSendFile(IAsyncResult asyncResult);
        //public int EndSendTo(IAsyncResult asyncResult);
        //public bool SendAsync(SocketAsyncEventArgs e);
        //public bool SendToAsync(SocketAsyncEventArgs e); 
        //public static void CancelConnectAsync(SocketAsyncEventArgs e);
        //public bool ConnectAsync(SocketAsyncEventArgs e);
        //public static bool ConnectAsync(SocketType socketType, ProtocolType protocolType, SocketAsyncEventArgs e);
        //public bool DisconnectAsync(SocketAsyncEventArgs e);
    }
}
