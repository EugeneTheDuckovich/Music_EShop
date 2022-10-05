using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace eshop.Models
{
    public class Instrument
    {
        public int Id { get; set; }  
        public string Producer { get; set; }
        public string Model { get; set; }
        public string BriefDescription { get; set; }

        [DataType(DataType.Upload)]
        [Display(Name = "Upload File")]
        [Required(ErrorMessage = "Please choose file to upload.")]
        public string? ImageSource { get; set; }
        public int Price { get; set; }
        //[ForeignKey("Creator")]
        //public string CreatorId { get; set; }
        //public User Creator { get; set; }
        public virtual ICollection<PurchaseProduct> PurchaseProducts { get; set; } = new List<PurchaseProduct>();

        public virtual Guitar? Guitar { get; set; }
        public virtual Keyboard? Keyboard { get; set; }
    }
}
