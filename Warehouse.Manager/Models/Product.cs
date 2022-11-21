namespace Warehouse.Manager.Models
{
  public class Product
  {
    public int ID { get; set; }
    public string Type { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string ImageB64 { get; set; }
    public float BoughtPrice { get; set; }
    public float SellPrice { get; set; }
    public int Amount { get; set; }
  }
}
