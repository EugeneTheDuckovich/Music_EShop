using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace eshop.Models;

public class Keyboard
{
    [Key, ForeignKey("Instrument")]
    public int Id { get; set; }
    public int NumberOfOctaves { get; set; }
    public string Material { get; set; }
    public virtual Instrument Instrument { get; set; }
}
