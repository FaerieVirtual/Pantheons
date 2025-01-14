public class Sword : IItem, ISword
{
    public string name { get; set; }
    public string description { get; set; }
    public int price { get; set; }
    public int quantity { get; set; }
    public int damage { get; set; }
    public float reach { get; set; }
}

