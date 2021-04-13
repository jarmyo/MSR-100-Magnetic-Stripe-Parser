namespace Repos.MSR100Controller
{
    public class MagneticCardInfo
    {
        public MagneticCardInfo()
        {
            track1 = new Track01Info();
            track2 = new Track02Info();
        }

        private Track01Info track1;
        private Track02Info track2;
        private string raw;

        public Track01Info Track1 { get => track1; set => track1 = value; }
        public Track02Info Track2 { get => track2; set => track2 = value; }
        public string Raw { get => raw; set => raw = value; }
    }
}
