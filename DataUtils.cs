using MSR_100_Magnetic_Stripe_Parser;
namespace Repos.MSR100Controller
{
    internal static class DataUtils
    {
        internal static void SetNameData(ref Track01Info cardinfoTrack1, string nameInfo)
        {
            cardinfoTrack1.Name = nameInfo;
            var namesplit = cardinfoTrack1.Name.Split('/');

            cardinfoTrack1.LastName = namesplit[0].Trim();
            cardinfoTrack1.FirstName = string.Empty;
            if (namesplit.Length > 1)
            {
                cardinfoTrack1.FirstName = namesplit[1].Trim();
            }

            cardinfoTrack1.Name = $"{cardinfoTrack1.FirstName} {cardinfoTrack1.LastName}";
        }
        internal static System.DateTime ExpirationDate(string date)
        {
            var year = System.Convert.ToInt16(date.Substring(0, 2)) + 2000;
            var month = System.Convert.ToInt16(date.Substring(2, 2));
            return new System.DateTime(year, month, 1);
        }
        internal static void SetDiscretionaryData(ref Track01Info cardinfoTrack1, string[] fieldsTrack01)
        {
            cardinfoTrack1.ExpirationDate = new System.DateTime();
            cardinfoTrack1.ServiceCode = null;

            if (fieldsTrack01.Length == 3)
            {
                cardinfoTrack1.ExpirationDate = ExpirationDate(fieldsTrack01[2]);
                cardinfoTrack1.ServiceCode = fieldsTrack01[2].Substring(4, 3);
                cardinfoTrack1.DiscretionaryData = fieldsTrack01[2].Substring(7);
            }
            else
            {
                if (fieldsTrack01.Length == 4)
                {
                    if (fieldsTrack01[2].Length > 1) //has date                          
                    {
                        cardinfoTrack1.ExpirationDate = ExpirationDate(fieldsTrack01[2]);
                    }
                    else // has service code                      
                    {
                        cardinfoTrack1.ServiceCode = fieldsTrack01[3];
                    }

                    cardinfoTrack1.DiscretionaryData = fieldsTrack01[3];
                }
                else if (fieldsTrack01.Length == 5)
                {
                    cardinfoTrack1.DiscretionaryData = fieldsTrack01[4];
                }
            }
        }
    }
}