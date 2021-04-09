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

        private static void ParseTrack01(ref Track01Info cardinfoTrack1, string tracks0 )
        {
            cardinfoTrack1.Raw = tracks0;
            if (tracks0.Length <= 79)
            {
                int year, month = 0;

                var fieldsTrack01 = tracks0.Substring(2).Split('^');
                cardinfoTrack1.FormatCode = tracks0[1];
                cardinfoTrack1.PAN = fieldsTrack01[0];

                cardinfoTrack1.Name = fieldsTrack01[1];
                var namesplit = cardinfoTrack1.Name.Split('/');

                cardinfoTrack1.LastName = namesplit[0].Trim();
                cardinfoTrack1.FirstName = string.Empty;
                if (namesplit.Length > 1)
                    cardinfoTrack1.FirstName = namesplit[1].Trim();
                cardinfoTrack1.Name = cardinfoTrack1.FirstName + " " + cardinfoTrack1.LastName;

                if (fieldsTrack01.Length == 3)
                {
                    year = Convert.ToInt16(fieldsTrack01[2].Substring(0, 2)) + 2000;
                    month = Convert.ToInt16(fieldsTrack01[2].Substring(2, 2));
                    cardinfoTrack1.ExpirationDate = new DateTime(year, month, 1);
                    cardinfoTrack1.ServiceCode = fieldsTrack01[2].Substring(4, 3);
                    cardinfoTrack1.DiscretionaryData = fieldsTrack01[2].Substring(7);
                }
                else
                {
                    if (fieldsTrack01.Length == 4)
                    {
                        if (fieldsTrack01[2].Length > 1) //has date
                        {
                            year = Convert.ToInt16(fieldsTrack01[2].Substring(0, 2)) + 2000;
                            month = Convert.ToInt16(fieldsTrack01[2].Substring(2, 2));
                            cardinfoTrack1.ExpirationDate = new DateTime(year, month, 1);
                            cardinfoTrack1.ServiceCode = null;
                        }
                        else // has service code
                        {
                            cardinfoTrack1.ExpirationDate = new DateTime();
                            cardinfoTrack1.ServiceCode = fieldsTrack01[3]; ;
                        }
                        cardinfoTrack1.DiscretionaryData = fieldsTrack01[3];
                    }
                    else if (fieldsTrack01.Length == 5)
                    {
                        cardinfoTrack1.ExpirationDate = new DateTime();
                        cardinfoTrack1.ServiceCode = null;
                        cardinfoTrack1.DiscretionaryData = fieldsTrack01[4];
                    }
                }
            }

        }

        private static void ParseTrack02(ref Track02Info cardinfoTrack2, string tracks1)
        {
            if (tracks1.Length <= 40 && tracks1.Length > 1)
            {
                cardinfoTrack2.Raw = tracks1;

                var fieldsTrack02 = tracks1.Substring(1).Split('=');

                cardinfoTrack2.PAN = fieldsTrack02[0];
                var year = Convert.ToInt16(fieldsTrack02[1].Substring(0, 2)) + 2000;
                var month = Convert.ToInt16(fieldsTrack02[1].Substring(2, 2));
                cardinfoTrack2.ExpirationDate = new DateTime(year, month, 1);
                cardinfoTrack2.ServiceCode = fieldsTrack02[1].Substring(4, 3);              
            }
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
                ParseTrack01(ref cardinfo.Track1, tracks[0]);
           
                if (tracks.Length > 1)
                {
                    ParseTrack02(ref cardinfo.Track2, tracks[1]);
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
