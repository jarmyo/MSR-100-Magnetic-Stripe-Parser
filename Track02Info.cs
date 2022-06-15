using Repos.MSR100Controller;
namespace MSR_100_Magnetic_Stripe_Parser
{
    public class Track02Info : ITRack
    {
        public string Raw { get; set; }
        public string PAN { get; set; }
        public System.DateTime ExpirationDate { get; set; }
        public string ServiceCode { get; set; }
        public string DiscretionaryData { get; set; }
    }
}