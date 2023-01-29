using System.ComponentModel.DataAnnotations;

namespace ASP.NET_Core_MVC_Authentication.Models
{
	public class CreateModel
	{
		[Required]
		public string Name { get; set; }
		[Required]
		public string Email { get; set; }
		[Required]
		public string Password { get; set; }
	}
}
