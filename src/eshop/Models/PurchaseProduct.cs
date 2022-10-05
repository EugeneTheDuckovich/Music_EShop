using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eshop.Models
{
    public class PurchaseProduct
    {
        public int Id { get; set; }
        public int PurchaseId { get; set; }
        public int InstrumentId { get; set; }
        public int Amount { get; set; }
        public virtual Purchase Purchase { get; set; }
        public virtual Instrument Instrument { get; set; }
    }
}
