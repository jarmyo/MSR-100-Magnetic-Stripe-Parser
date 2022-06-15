namespace Repos.MSR100Controller;
public class Track01Info : ITRack
{
    public char FormatCode { get; set; }
    public string LastName { get; set; }
    public string FirstName { get; set; }
    public string Name { get; set; }
    public string Raw { get; set; }
    public string PAN { get; set; }
    public DateTime ExpirationDate { get; set; }
    public string ServiceCode { get; set; }
    public string DiscretionaryData { get; set; }
}