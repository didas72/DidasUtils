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
        public event EventHandler<byte[]> MessageReceived;
        /// <summary>
        /// Checks wether the receive thread is running.
        /// </summary>
        public bool IsListening { get => !(receiveThread == null || !receiveThread.IsAlive); }
        /// <summary>
        /// Checks wether the receiver is working on an incoming message.
        /// </summary>
        public  bool ReceiverBusy { get; private set; }
        /// <summary>
        /// Tracks the number of messages received
        /// </summary>
        public long ReceivedMessages { get; private set; } = 0;


        private readonly NetworkStream netStream;
        private Thread receiveThread;
        private volatile bool doListen = false;

        private const ushort blockSize = 4096;



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
        /// Starts listening for incoming messages.
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
        /// Sends an byte array thorugh the TcpClient.
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
            catch (Exception e)
            {
                if (Logging.Log.Initialized)
                    Logging.Log.LogException("Failed to send message.", "Client", e);

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
                        ReceivedMessages++;
                        ReceiverBusy = true;
                        byte[] buffer = SegmentedData.ReadFromStream(netStream, blockSize);
                        MessageReceived?.Invoke(this, buffer);
                        ReceiverBusy = false;
                    }
                    else
                    {
                        Thread.Sleep(10);
                    }
                }
                catch (Exception e)
                {
                    if (Logging.Log.Initialized)
                        Logging.Log.LogException("Failed to receive message.", "Client", e);
                }
            }
        }
    }
}
