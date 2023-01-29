//Kabluchkov DS (c) 2023
using ASP.NET_Core_MVC_Authentication.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ASP.NET_Core_MVC_Authentication.Controllers
{
    public class RoleAdminController : Controller
    {
        private RoleManager<IdentityRole> roleManager;
        //ввод менеджера пользователей
		private UserManager<AppUser> userManager;
		public RoleAdminController(RoleManager<IdentityRole> roleMgr, UserManager<AppUser> userMgr)
        {
            roleManager = roleMgr;
			userManager = userMgr;
		}
        //просмотр текущих ролей
        public ViewResult Index() => View(roleManager.Roles);
        //создание ролей
        public IActionResult Create() => View();
        [HttpPost]
        public async Task<IActionResult> Create([Required] string name)
        {
            if (ModelState.IsValid)
            {
                IdentityResult result = await roleManager.CreateAsync(new IdentityRole(name));
                if (result.Succeeded) return RedirectToAction("Index");
                else AddErrorsFromResult(result);
            }
            return View(name);
        }
        [HttpPost]
        //удаление ролей
        public async Task<IActionResult> Delete(string id)
        {
            IdentityRole role = await roleManager.FindByIdAsync(id);
            if (role != null)
            {
                IdentityResult result = await roleManager.DeleteAsync(role);
                if (result.Succeeded) return RedirectToAction("Index");
                else AddErrorsFromResult(result);
            }
            else ModelState.AddModelError("", "No role found");
            return View("Index", roleManager.Roles);
        }
        //правка ролей
		public async Task<IActionResult> Edit(string id)
		{
			IdentityRole role = await roleManager.FindByIdAsync(id);
			List<AppUser> members = new List<AppUser>();
			List<AppUser> nonМembers = new List<AppUser>();
			foreach (AppUser user in userManager.Users)
			{
				bool bIsInRole = await userManager.IsInRoleAsync(user, role.Name);
				if (bIsInRole)
					members.Add(user);
				else
					nonМembers.Add(user);
			}
			var roleEditModel = new RoleEditModel
			{
				Role = role,
				Мembers = members,
				NonМembers = nonМembers
			};
			return View(roleEditModel);
		}

		[HttpPost]
		public async Task<IActionResult> Edit(RoleМodificationМodel model)
		{
			IdentityResult result;
			if (ModelState.IsValid)
			{
				if (model.IdsToAdd != null)
				{
					foreach (string userid in model.IdsToAdd)
					{
						AppUser user = await userManager.FindByIdAsync(userid);
						if (user != null)
						{
							result = await userManager.AddToRoleAsync(user,
							model.RoleName);
							if (!result.Succeeded)
								AddErrorsFromResult(result);
						}
					}
				}
				if (model.IdsToDelete != null)
				{
					foreach (string userid in model.IdsToDelete)
					{
						AppUser user = await userManager.FindByIdAsync(userid);
						if (user != null)
						{
							result = await userManager.RemoveFromRoleAsync(user,
							model.RoleName);
							if (!result.Succeeded)
								AddErrorsFromResult(result);
						}
					}
				}
			}
			if (ModelState.IsValid)
				return RedirectToAction(nameof(Index));
			else
				return await Edit(model.RoleId);
		}

		private void AddErrorsFromResult(IdentityResult result)
        {
            foreach (IdentityError error in result.Errors)
            ModelState.AddModelError("", error.Description);
        }
    }
}
