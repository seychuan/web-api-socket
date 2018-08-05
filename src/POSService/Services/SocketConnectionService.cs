using POSService.Interfaces;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace POSService.Services
{
    public class SocketConnectionService : IConnectionService, IDisposable
    {
        private const int _bufferSize = 1024;

        private class SocketState
        {
            public Socket SocketInstance = null;
            public byte[] Buffer = new byte[_bufferSize];
            public StringBuilder Data = new StringBuilder();
            public StringBuilder ErrorMessage = new StringBuilder();
        }

        private readonly Socket _socket = null;
        private static ManualResetEvent _receiveDone = new ManualResetEvent(false);

        public SocketConnectionService(string hostNameOrAddress, int port)
        {
            // Get host related information
            var hostEntry = Dns.GetHostEntry(hostNameOrAddress);

            // Store error code if unable to obtain any supported AddressFamily
            var errorCode = 0;

            // Loop through the AddressList to obtain the supported AddressFamily. This is to avoid
            // an exception that occurs when the host IP Address is not compatible with the address family
            // (typical in the IPv6 case).
            foreach (IPAddress address in hostEntry.AddressList)
            {
                IPEndPoint ipe = new IPEndPoint(address, port);
                Socket tempSocket = new Socket(ipe.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                try
                {
                    tempSocket.Connect(ipe);

                    if (tempSocket.Connected)
                    {
                        _socket = tempSocket;
                        break;
                    }
                }
                catch (SocketException ex) { errorCode = ex.ErrorCode; }
                catch { throw; } // Any other unexpected exception
            }

            if (_socket == null) throw new SocketException(errorCode);
        }

        public void Dispose()
        {
            Close();
        }

        public void Close()
        {
            // Socket was initialized
            if (_socket != null)
            {
                // Socket is connecting to the server
                if (_socket.Connected)
                {
                    // Release the socket
                    _socket.Shutdown(SocketShutdown.Both);
                    _socket.Close();
                }
            }
        }

        public int Send(string data)
        {
            // Encode the data string into a byte array
            var bytesSend = Encoding.ASCII.GetBytes(data);

            // Send the data to the server
            var bytesSent = _socket.Send(bytesSend, bytesSend.Length, SocketFlags.None);
            return bytesSent;


            //// Data buffer for incoming data
            //var bytesReceive = new byte[_bufferSize];


            //// Receive the response from server
            //var bytes = 0;
            //var response = new StringBuilder();

            //// The following will loop until the response is transmitted
            //do
            //{
            //    bytes = _socket.Receive(bytesReceive, bytesReceive.Length, SocketFlags.None);
            //    response.Append(Encoding.ASCII.GetString(bytesReceive, 0, bytes));
            //} while (bytes > 0);

            //return response.ToString();






            //return "This is your response.";
        }

        public string Receive(TimeSpan timeout)
        {
            // Create the state object
            var state = new SocketState() { SocketInstance = _socket };

            // Receive the response from the remote device
            BeginReceive(state);
            var isSignalled = _receiveDone.WaitOne(timeout);

            // Didn't receive response in the specified duration
            if (isSignalled == false) throw new TimeoutException("Receive timeout.");

            if (state.ErrorMessage.Length > 0)
            {
                throw new Exception(state.ErrorMessage.ToString());
            }

            _receiveDone.Reset();

            return state.Data.ToString();
        }

        private void BeginReceive(SocketState state)
        {
            // Begin receiving the data from the server
            state.SocketInstance.BeginReceive(
                state.Buffer,
                0,
                _bufferSize,
                SocketFlags.None,
                new AsyncCallback(ReceiveCallback),
                state);
        }

        private void ReceiveCallback(IAsyncResult ar)
        {
            // Retrieve the state object and the socket
            // from the asynchronous state object
            var state = (SocketState)ar.AsyncState;

            try
            {
                // Read data from the server
                int bytesReceived = state.SocketInstance.EndReceive(ar);

                if (bytesReceived > 0)
                {
                    // There might be more data
                    // Store the data received so far
                    state.Data.Append(Encoding.ASCII.GetString(state.Buffer, 0, bytesReceived));

                    // Continue to get the rest of the data
                    state.SocketInstance.BeginReceive(
                        state.Buffer,
                        0,
                        _bufferSize,
                        SocketFlags.None,
                        new AsyncCallback(ReceiveCallback),
                        state);
                }
                else
                {
                    // Add data has arrived
                    // Signal that all bytes have been received
                    _receiveDone.Set();
                }
            }
            catch (Exception ex)
            {
                state.ErrorMessage.AppendLine(ex.Message);
            }
        }
    }
}
