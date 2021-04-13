using Repos.MSR100Controller;
using System;
using System.IO.Ports;

namespace Repos.MSR100Controller
{
    public delegate void CardHandler(MagneticCardInfo cardinfo);
    public class MSR100Controller : IDisposable
    {
        public MSR100Controller(string PortName = "COM1", int baudRate = 9600)
        {
            _SerialPort = new SerialPort(PortName, baudRate);
            _SerialPort.DataReceived += DataRecived;
            _SerialPort.Open();
        }
        private SerialPort _SerialPort;
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

           // int year, month = 0;

            if (tracks.Length > 0)
            {
                TrackParsers.ParseTrack01(ref cardinfo.Track1, tracks[0]);
           
                if (tracks.Length > 1)
                {
                    if (tracks[1].Length <= 40 && tracks[1].Length > 1)
                    {
                        cardinfo.Track1.DiscretionaryData = TrackParsers.ParseTrack02(ref cardinfo.Track2, tracks[1]);                       
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