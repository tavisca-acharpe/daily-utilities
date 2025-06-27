namespace Test;

public class Order
{
    public string Id { get; set; }
    public int Index { get; set; }
    public decimal Cash { get; set; }
    public decimal? Points { get; set; }
    public List<string> ItemIds { get; set; }
}
