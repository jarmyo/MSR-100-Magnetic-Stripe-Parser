using System;
using System.IO.Ports;
namespace Repos.MSR100Controller
{
    public delegate void CardHandler(MagneticCardInfo cardinfo);
    public class MSR100Controller : IDisposable
    {
        public MSR100Controller(string PortName, int baudRate)
        {
            _SerialPort = new SerialPort(PortName, baudRate);
            _SerialPort.DataReceived += DataRecived;
            _SerialPort.Open();
        }
        public MSR100Controller()
        {
            _SerialPort = new SerialPort("COM1", 9600);
            _SerialPort.DataReceived += DataRecived;
            _SerialPort.Open();
        }
        private readonly SerialPort _SerialPort;
        public event CardHandler OnCardSwiped;
        void DataRecived(object sender, SerialDataReceivedEventArgs e)
        {
            var sp = (SerialPort)sender;
            var data = sp.ReadLine();
            var cardinfo = ParseData(data);
            OnCardSwiped?.Invoke(cardinfo);
        }
        public static MagneticCardInfo ParseData(string data)
        {
            data = data.Replace("\r", null);
            var cardinfo = new MagneticCardInfo
            {
                Raw = data
            };

            var tracks = data.Split('?'); //split end sentinel

            if (tracks.Length > 0)
            {
                cardinfo.Track1.ParseTrack01(tracks[0]);

                if (tracks.Length > 1)
                {
                    if (tracks[1].Length <= 40 && tracks[1].Length > 1)
                    {
                        cardinfo.Track1.DiscretionaryData = cardinfo.Track2.ParseTrack02(tracks[1]);
                    }
                }
            }
            return cardinfo;
        }
        public void Dispose()
        {
            GC.SuppressFinalize(this);
            _SerialPort.Close();
        }
    }
}