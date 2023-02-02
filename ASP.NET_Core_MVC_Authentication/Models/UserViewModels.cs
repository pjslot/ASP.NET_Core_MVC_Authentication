using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ASP.NET_Core_MVC_Authentication.Models
{
	//сущности для работы с информацией о ролях
	public class RoleEditModel
	{
		public IdentityRole Role { get; set; }
		public IEnumerable<AppUser> Мembers { get; set; }
		public IEnumerable<AppUser> NonМembers { get; set; }
	}
	public class RoleМodificationМodel
	{
	//	[Required]
		public string RoleName { get; set; }
		public string RoleId { get; set; }
		public string[]? IdsToAdd { get; set; }
		public string[]? IdsToDelete { get; set; }
	}

}
