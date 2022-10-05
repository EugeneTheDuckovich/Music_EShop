using eshop.Models;

namespace eshop.ViewsModel
{
    public class GuitarCreationViewModel
    {

        public string Producer { get; set; }

        public string Model { get; set; }

        public int Price { get; set; }

        public string BriefDescription { get; set; }

        public string BodyMaterial { get; set; }

        public string DeckMaterial { get; set; }

        public int NumberOfStrings { get; set; }
    }
}
