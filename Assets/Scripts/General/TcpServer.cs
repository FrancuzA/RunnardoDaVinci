using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;

using UnityEngine;

namespace Xalamandrus.TCP
{
    public class TcpServer : MonoBehaviour
    {
        [Header("Server Config:")]
        [SerializeField] private int _port = 8080;
        [SerializeField] private string _listenIP = "127.0.0.1";

        private TcpListener _tcpListener;
        private Thread _listenerThread;
        private bool _isListening = false;

        private string _receivedData = string.Empty;
        public string ReceivedData => _receivedData;

        #region Unity

        private void Awake() => TCPServerStart();

        private void OnDestroy() => TCPServerStop();

        #endregion

        #region TCP Server

        private void TCPServerStart()
        {
            if (_isListening)
                return;

            try
            {
                IPAddress ipAddress = IPAddress.Parse(_listenIP);
                _tcpListener = new TcpListener(ipAddress, _port);

                _tcpListener.Start();

                _listenerThread = new Thread(ListenForConnections);
                _listenerThread.IsBackground = true;
                _listenerThread.Start();

                _isListening = true;
                Debug.Log($"TCP Server started on {_listenIP}:{_port}");
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"Error starting TCP listener: {ex.Message}");
            }
        }

        private void TCPServerStop()
        {
            _isListening = false;
            _tcpListener?.Stop();
            _listenerThread?.Abort();
            Debug.Log("TCP Server stopped.");
        }

        private void ListenForConnections()
        {
            byte[] buffer = new byte[1024];

            while (_isListening)
            {
                try
                {
                    using NetworkStream stream = _tcpListener.AcceptTcpClient().GetStream();
                    int bytesRead = stream.Read(buffer, 0, buffer.Length);
                    if (bytesRead > 0)
                    {
                        _receivedData = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                        Debug.Log($"Received data: {_receivedData}");
                    }
                }
                catch (SocketException ex)
                {
                    Debug.LogError($"Socket exception: {ex.Message}");
                }
                catch (ThreadAbortException)
                {
                    // Thread was aborted, exit gracefully
                    break;
                }
            }
        }

        #endregion
    }
}