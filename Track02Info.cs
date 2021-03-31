using System;

namespace Repos.MSR100Controller
{
    public class Track02Info : ITRack
    {
        public string Raw { get; set; }
        public string PAN { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string ServiceCode { get; set; }
        public string DiscretionaryData { get; set; }
    }
}
