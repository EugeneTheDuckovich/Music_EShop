

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eshop.Models;

public class Guitar
{
    [Key, ForeignKey("Instrument")]
    public int Id { get; set; }
    public int NumberOfStrings { get; set; }
    public string BodyMaterial { get; set; }
    public string DeckMaterial { get; set; }

    public virtual Instrument Instrument { get; set; }
}
