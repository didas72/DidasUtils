using System;
using System.Net.Sockets;
using System.Threading;

using DidasUtils.Data;

namespace DidasUtils.Net
{
    /// <summary>
    /// Wrapper class that contains a TcpClient and useful networking methods.
    /// </summary>
    public class Client
    {
        /// <summary>
        /// The wrapped TcpClient instance.
        /// </summary>
        public TcpClient socket;
        /// <summary>
        /// The callback for when a message is received.
        /// </summary>
        public MessageReceived messageReceived;

        private readonly NetworkStream netStream;
        private Thread receiveThread;
        private volatile bool doListen = false;

        private const ushort blockSize = ushort.MaxValue;



        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public delegate void MessageReceived(byte[] message);



        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="socket">The TcpClient to wrap.</param>
        public Client(TcpClient socket)
        {
            this.socket = socket;
            socket.SendBufferSize = blockSize;
            socket.ReceiveBufferSize = blockSize;
            netStream = socket.GetStream();
        }



        /// <summary>
        /// Starts listening for incoming messages, calling <see cref="messageReceived"/> callback when needed.
        /// </summary>
        public void StartListening()
        {
            doListen = true;
            receiveThread = new Thread(ReceiveLoop);
            receiveThread.Start();
        }
        /// <summary>
        /// Stops listening for incoming messages.
        /// </summary>
        public void StopListening()
        {
            doListen = false;
            receiveThread.Join();
        }



        /// <summary>
        /// Sends an <see cref="IMessage"/> thorugh the TcpClient.
        /// </summary>
        /// <param name="message">The message to be sent.</param>
        /// <returns>Boolean indicating the operation's success.</returns>
        public bool SendMessage(byte[] message)
        {
            try
            {
                if (socket.Connected)
                {
                    SegmentedData.SendToStream(message, netStream, blockSize);
                }
                else
                    return false;
            }
            catch
            {
                return false;
            }

            return true;
        }
        /// <summary>
        /// Disconnects the TcpClient and disposes all resources.
        /// </summary>
        public void Disconnect()
        {
            StopListening();
            netStream.Dispose();
            socket.Close();
        }



        private void ReceiveLoop()
        {
            while (doListen)
            {
                try
                {
                    if (netStream.DataAvailable)
                    {
                        byte[] buffer = SegmentedData.ReadFromStream(netStream, blockSize);

                        messageReceived.BeginInvoke(buffer, null, this);
                    }
                }
                catch { }

                Thread.Sleep(10);
            }
        }
    }
}
