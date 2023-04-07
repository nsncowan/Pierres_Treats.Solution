namespace Storefront.Models
{
  public class Treat
  {
    public int TreatId { get; set; }
    public string Name { get; set; }
    public List<FlavorTreat> FlavorTreatJoinEntities { get; }
  }
}