using System;
using System.Globalization;
using System.IO;
using System.Net.Security;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CMIUICXCore.Services
{
    public class AsyncTcpClient : IDisposable
    {
        private TcpClient tcpClient;
        private Stream stream;

        private int bufferSize = 8192;
        private bool disposed = false;

        public event EventHandler<byte[]> OnDataReceived;
        public event EventHandler OnDisconnected;
        public event EventHandler<string> OnMessage;

        private int BufferSize
        {
            get
            {
                return bufferSize;
            }
            set
            {
                bufferSize = value;
                if (tcpClient != null)
                    tcpClient.ReceiveBufferSize = value;
            }
        }

        public bool IsReceiving { get; private set; }
        public int MinBufferSize { get; set; } = 8192;
        public int MaxBufferSize { get; set; } = 15 * 1024 * 1024;
        public bool IsConnected => tcpClient != null && tcpClient.Connected;

        public int SendBufferSize
        {
            get
            {
                if (tcpClient != null)
                    return tcpClient.SendBufferSize;
                else
                    return 0;
            }
            set
            {
                if (tcpClient != null)
                    tcpClient.SendBufferSize = value;
            }
        }

        public async Task SendAsync(byte[] data, CancellationToken cancellationToken = default)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            try
            {
                var registration = cancellationToken.Register(() => CloseIfCanceled(cancellationToken), useSynchronizationContext: false);
                try
                {
                    await stream.WriteAsync(data.AsMemory(0, data.Length), cancellationToken);
                    await stream.FlushAsync(cancellationToken);
                }
                finally
                {
                    registration.Dispose();
                }
            }
            catch (IOException ex)
            {
                if (ex.InnerException is ObjectDisposedException)
                    OnMessage?.Invoke(this, "innocuous ssl stream error"); // for SSL streams
                else
                    OnDisconnected?.Invoke(this, EventArgs.Empty);
            }
            catch
            {
                CloseIfCanceled(cancellationToken);
                throw;
            }
        }

        public async Task ConnectAsync(string host, int port, bool ssl = false, CancellationToken cancellationToken = default)
        {
            try
            {
                var registration = cancellationToken.Register(() => CloseIfCanceled(cancellationToken), useSynchronizationContext: false);
                try
                {
                    await CloseAsync();
                    tcpClient = new TcpClient();
                    await tcpClient.ConnectAsync(host, port, cancellationToken);
                    // get stream and do SSL handshake if applicable
                    stream = tcpClient.GetStream();
                    if (ssl)
                    {
                        var sslStream = new SslStream(stream);
                        await sslStream.AuthenticateAsClientAsync(host);
                        stream = sslStream;
                    }
                }
                finally
                {
                    registration.Dispose();
                }
            }
            catch
            {
                CloseIfCanceled(cancellationToken);
                throw;
            }
        }

        public async Task ReceiveAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                if (!IsConnected || IsReceiving)
                    throw new InvalidOperationException();

                var registration = cancellationToken.Register(() => CloseIfCanceled(cancellationToken), useSynchronizationContext: false);
                try
                {
                    IsReceiving = true;
                    byte[] buffer = new byte[bufferSize];
                    while (IsConnected)
                    {
                        int bytesRead = await stream.ReadAsync(buffer.AsMemory(0, buffer.Length), cancellationToken);
                        if (bytesRead > 0)
                        {
                            if (bytesRead == buffer.Length)
                                BufferSize = Math.Min(BufferSize * 10, MaxBufferSize);
                            else
                            {
                                do
                                {
                                    int reducedBufferSize = Math.Max(BufferSize / 10, MinBufferSize);
                                    if (bytesRead < reducedBufferSize)
                                        BufferSize = reducedBufferSize;
                                }
                                while (bytesRead > MinBufferSize);
                            }

                            if (OnMessage != null)
                            {
                                StringBuilder sb = new("Data received: ");
                                StringBuilder sba = new("<");
                                for (int i = 0; i < bytesRead; i++)
                                {
                                    sb.AppendFormat(CultureInfo.CurrentCulture, "{0:X2} ", buffer[i]);
                                    char ch = Convert.ToChar(buffer[i]);
                                    if (char.IsLetterOrDigit(ch) || char.IsPunctuation(ch) || char.IsSymbol(ch) || (ch == ' '))
                                        sba.Append(ch);
                                    else
                                        sba.Append('.');
                                }
                                sba.Append('>');
                                OnMessage.Invoke(this, sb.ToString() + sba.ToString());
                            }

                            if (OnDataReceived != null)
                            {
                                byte[] data = new byte[bytesRead];
                                Array.Copy(buffer, data, bytesRead);
                                OnDataReceived.Invoke(this, data);
                            }
                        }
                        buffer = new byte[bufferSize];
                    }
                }
                finally
                {
                    registration.Dispose();
                }
            }
            catch (ObjectDisposedException)
            {
                // This should be ok
                OnMessage?.Invoke(this, "ObjectDisposed Exception in receive");
            }
            catch (IOException ex)
            {
                if (ex.InnerException != null && ex.InnerException is ObjectDisposedException)
                    OnMessage?.Invoke(this, "innocuous ssl stream error"); // for SSL streams
                OnDisconnected?.Invoke(this, EventArgs.Empty);
            }
            catch
            {
                CloseIfCanceled(cancellationToken);
                throw;
            }
            finally
            {
                IsReceiving = false;
            }
        }

        public async Task CloseAsync(Action onClosed = null)
        {
            await Task.Run(() => Close(onClosed));
        }

        public void Close(Action onClosed = null)
        {
            tcpClient?.Close();
            onClosed?.Invoke();
        }

        private void CloseIfCanceled(CancellationToken cancellationToken, Action onClosed = null)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                Close(onClosed);
                cancellationToken.ThrowIfCancellationRequested();
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (!disposed)
                {
                    tcpClient?.Dispose();
                    stream?.Dispose();
                }
            }
            disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
