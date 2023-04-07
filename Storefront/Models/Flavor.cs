namespace Storefront.Models
{
  public class Flavor
  {
    public int FlavorId { get; set; }
    public string Name { get; set; }
    public List<FlavorTreat> FlavorTreatJoinEntities { get; }
  }
}
