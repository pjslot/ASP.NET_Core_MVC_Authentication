//(c) Kabluchkov DS 2023
using ASP.NET_Core_MVC_Authentication.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ASP.NET_Core_MVC_Authentication.Controllers
{
	[Authorize]
	public class AccountController : Controller
	{
		//менеджер пользователей и менеджер входов
		private UserManager<AppUser> userManager;
		private SignInManager<AppUser> signInManager;

		//получение и сохранение зависимости от инфраструктуры в конструкторе контроллера
		public AccountController(UserManager<AppUser> userMgr, SignInManager<AppUser> signInMgr)
		{
			userManager = userMgr; signInManager = signInMgr;
		}

		public IActionResult Index()
		{
			return View();
		}


		[AllowAnonymous]
		public IActionResult Login(string returnUrl)
		{
			ViewBag.returnUrl = returnUrl;
			return View();
		}

		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Login(LoginModel details, string returnUrl)
		{
			if (ModelState.IsValid)
			{
				AppUser user = await userManager.FindByEmailAsync(details.Email);
				if (user != null)
				{
					await signInManager.SignOutAsync();
					Microsoft.AspNetCore.Identity.SignInResult result =
					await signInManager.PasswordSignInAsync(user, details.Password, false, false);
					if (result.Succeeded)
						return Redirect(returnUrl ?? "/");
				}
                //ModelState.AddModelError(nameof(LoginModel.Email), "Неверное имя пользователя или пароль.");
                return RedirectToAction("AccessDenied");
            }
			return View(details);
		}

        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            ViewBag.word = "ВХОД ВОСПРЕЩЁН!";
            return View();
        }

    }
}
