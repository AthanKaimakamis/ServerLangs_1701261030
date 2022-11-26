namespace Warehouse.Manager.Models
{
  public class Product
  {
    public int pID { get; set; }
    public string pType { get; set; }
    public string pName { get; set; }
    public string pDesc { get; set; }
    public string pImg { get; set; }
    public double pBPrice { get; set; }
    public double pSPrice { get; set; }
    public int pAmount { get; set; }
    public string? Error { get; set; } = string.Empty;
  }
}
