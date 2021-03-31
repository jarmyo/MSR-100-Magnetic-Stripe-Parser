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

            int year, month = 0;

            if (tracks.Length > 0)
            {
                cardinfo.Track1.Raw = tracks[0];
                if (tracks[0].Length <= 79)
                {
                    var fieldsTrack01 = tracks[0].Substring(2).Split('^');
                    cardinfo.Track1.FormatCode = tracks[0][1];
                    cardinfo.Track1.PAN = fieldsTrack01[0];

                    cardinfo.Track1.Name = fieldsTrack01[1];
                    var namesplit = cardinfo.Track1.Name.Split('/');

                    cardinfo.Track1.LastName = namesplit[0].Trim();
                    cardinfo.Track1.FirstName = string.Empty;
                    if (namesplit.Length > 1)
                        cardinfo.Track1.FirstName = namesplit[1].Trim();
                    cardinfo.Track1.Name = cardinfo.Track1.FirstName + " " + cardinfo.Track1.LastName;

                    if (fieldsTrack01.Length == 3)
                    {
                        year = Convert.ToInt16(fieldsTrack01[2].Substring(0, 2)) + 2000;
                        month = Convert.ToInt16(fieldsTrack01[2].Substring(2, 2));
                        cardinfo.Track1.ExpirationDate = new DateTime(year, month, 1);
                        cardinfo.Track1.ServiceCode = fieldsTrack01[2].Substring(4, 3);
                        cardinfo.Track1.DiscretionaryData = fieldsTrack01[2].Substring(7);
                    }
                    else
                    {
                        if (fieldsTrack01.Length == 4)
                        {                            
                            if (fieldsTrack01[2].Length > 1) //has date
                            {
                                year = Convert.ToInt16(fieldsTrack01[2].Substring(0, 2)) + 2000;
                                month = Convert.ToInt16(fieldsTrack01[2].Substring(2, 2));
                                cardinfo.Track1.ExpirationDate = new DateTime(year, month, 1);
                                cardinfo.Track1.ServiceCode = null;
                            }
                            else // has service code
                            {
                                cardinfo.Track1.ExpirationDate = new DateTime();
                                cardinfo.Track1.ServiceCode = fieldsTrack01[3];;
                            }                            
                            cardinfo.Track1.DiscretionaryData = fieldsTrack01[3];
                        }
                        else if (fieldsTrack01.Length == 5)
                        {
                            cardinfo.Track1.ExpirationDate = new DateTime();
                            cardinfo.Track1.ServiceCode = null;
                            cardinfo.Track1.DiscretionaryData = fieldsTrack01[4];
                        }
                    }
                }

                if (tracks.Length > 1)
                {
                    if (tracks[1].Length <= 40 && tracks[1].Length > 1)
                    {
                        cardinfo.Track2.Raw = tracks[1];

                        var fieldsTrack02 = tracks[1].Substring(1).Split('=');

                        cardinfo.Track2.PAN = fieldsTrack02[0];
                        year = Convert.ToInt16(fieldsTrack02[1].Substring(0, 2)) + 2000;
                        month = Convert.ToInt16(fieldsTrack02[1].Substring(2, 2));
                        cardinfo.Track2.ExpirationDate = new DateTime(year, month, 1);
                        cardinfo.Track2.ServiceCode = fieldsTrack02[1].Substring(4, 3);
                        cardinfo.Track1.DiscretionaryData = fieldsTrack02[1].Substring(7);
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
