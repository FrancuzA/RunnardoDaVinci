using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;
using UnityEngine;

namespace Xalamandrus.TCP
{
    [System.Serializable]
    public class TcpPayload
    {
        public string clientId;
        public string text;
        public string sourceFile;
        public string sentAt;
    }

    public class TcpServer : MonoBehaviour
    {
        [Header("Server Config:")]
        [SerializeField] private int _port = 8080;
        [SerializeField] private string _listenIP = "0.0.0.0";

        private TcpListener _tcpListener;
        private Thread _listenerThread;
        private bool _isListening = false;

        private string _receivedText = string.Empty;
        public string ReceivedText => _receivedText;

        #region Unity

        private void Awake() => TCPServerStart();
        private void OnDestroy() => TCPServerStop();

        #endregion

        #region TCP Server

        private void TCPServerStart()
        {
            if (_isListening) return;

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
            while (_isListening)
            {
                try
                {
                    using TcpClient client = _tcpListener.AcceptTcpClient();
                    Debug.Log($"[TCP] Client connected from: {client.Client.RemoteEndPoint}");
                    using NetworkStream stream = client.GetStream();
                    using System.IO.StreamReader reader = new System.IO.StreamReader(stream, Encoding.UTF8);

                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (string.IsNullOrWhiteSpace(line)) continue;

                        TcpPayload payload = JsonUtility.FromJson<TcpPayload>(line);
                        if (payload != null)
                        {
                            _receivedText = payload.text;
                            Debug.Log($"[TCP] From: {payload.clientId} | Text: {payload.text} | At: {payload.sentAt}");
                        }
                    }
                }
                catch (SocketException ex)
                {
                    if (_isListening)
                        Debug.LogError($"Socket exception: {ex.Message}");
                }
                catch (ThreadAbortException)
                {
                    break;
                }
            }
        }

        #endregion
    }
}