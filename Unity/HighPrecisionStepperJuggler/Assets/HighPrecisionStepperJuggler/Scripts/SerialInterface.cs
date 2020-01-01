using UnityEngine;
using System.IO.Ports;
using System.Threading;

namespace HighPrecisionStepperJuggler
{
    public class SerialInterface : MonoBehaviour
    {
        public bool IsOpen => _port != null && _port.IsOpen;
        
        [SerializeField] private string[] _availablePorts;
        [SerializeField] private string _portName;
        
        private SerialPort _port;
        Thread _receiveDataThread;
        readonly object _lockObject = new object();

        private void Awake()
        {
            _availablePorts = SerialPort.GetPortNames();
        }

        private void Open()
        {
            Debug.LogFormat("try to connect on port {0} with a baudrate of: {1}.", _portName, Constants.BaudRate);
            
            _port = new SerialPort(_portName, Constants.BaudRate, Parity.None, 8, StopBits.One);
            _port.Handshake = Handshake.None;
            _port.Open();
            
            _receiveDataThread = new Thread(RecieveData);
            _receiveDataThread.Start();
        }

        public void Send(string s)
        {
            if (!IsOpen) Open();
            
            _port.Write(s);
        }
        
        private void RecieveData()
        {
            while (_port.IsOpen)
            {
                var str = _port.ReadLine();
                lock (_lockObject)
                {
                    Debug.Log(str);
                }
                Thread.Sleep(20);
            }
        }

        private void OnDestroy()
        {
            if (_port != null && _port.IsOpen)
            {
                Debug.Log("closing port");
                _port.Close();
            }

            if (_receiveDataThread != null && _receiveDataThread.IsAlive)
            {
                _receiveDataThread.Abort();
            }
        }
    }
}
