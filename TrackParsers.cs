using MSR_100_Magnetic_Stripe_Parser;
namespace Repos.MSR100Controller
{
    public static class TrackParsers
    {
        public static void ParseTrack01(this Track01Info cardinfoTrack1, string tracks0)
        {
            cardinfoTrack1.Raw = tracks0;
            if (tracks0.Length <= 79)
            {
                var fieldsTrack01 = tracks0.Substring(2).Split('^');
                cardinfoTrack1.FormatCode = tracks0[1];
                cardinfoTrack1.PAN = fieldsTrack01[0];
                DataUtils.SetNameData(ref cardinfoTrack1, fieldsTrack01[1]);
                DataUtils.SetDiscretionaryData(ref cardinfoTrack1, fieldsTrack01);
            }
        }
        public static string ParseTrack02(this Track02Info cardinfoTrack2, string tracks1)
        {
            cardinfoTrack2.Raw = tracks1;
            var fieldsTrack02 = tracks1.Substring(1).Split('=');
            cardinfoTrack2.PAN = fieldsTrack02[0];
            var year = System.Convert.ToInt16(fieldsTrack02[1].Substring(0, 2)) + 2000;
            var month = System.Convert.ToInt16(fieldsTrack02[1].Substring(2, 2));
            cardinfoTrack2.ExpirationDate = new System.DateTime(year, month, 1);
            cardinfoTrack2.ServiceCode = fieldsTrack02[1].Substring(4, 3);
            return fieldsTrack02[1].Substring(7);
        }
    }
}