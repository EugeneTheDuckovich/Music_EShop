namespace eshop.Models;

public class Purchase
{
    public int Id { get; set; }
    public string Status { get; set; }
    public ApplicationUser User { get; set; }
    public virtual ICollection<PurchaseProduct> PurchaseProducts { get; set; } = new List<PurchaseProduct>();
}
