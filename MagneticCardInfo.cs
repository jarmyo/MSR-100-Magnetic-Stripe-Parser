namespace Repos.MSR100Controller
{
    public class MagneticCardInfo
    {
        public MagneticCardInfo()
        {
            Track1 = new Track01Info();
            Track2 = new Track02Info();
        }

        public Track01Info Track1;
        public Track02Info Track2;
        public string Raw;
    }
}
