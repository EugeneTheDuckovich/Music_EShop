using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using eshop.Models;

namespace eshop.Data
{
    public class Purchases
	{
		[Key]
		public int Id { get; set; }
		[ForeignKey("ApplicationUserId")]

		public ApplicationUser ApplicationUser { get; set; }
		public string ApplicationUserId { get; set; }
		public Status Status { get; set; }
		
	}
}
