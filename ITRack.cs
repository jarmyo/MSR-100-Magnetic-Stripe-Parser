namespace Repos.MSR100Controller
{
    public interface ITRack
    {
        string Raw { get; set; }
        string PAN { get; set; }
        DateTime ExpirationDate { get; set; }
        string ServiceCode { get; set; }
        string DiscretionaryData { get; set; }
    }
}
