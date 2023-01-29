//(c)Kabluchkov DS 2023
using ASP.NET_Core_MVC_Authentication.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ASP.NET_Core_MVC_Authentication.Controllers
{
	public class AdminController : Controller
	{
		//доп. поля для апдейта пользователя
		private IUserValidator<AppUser> userValidator;
		private IPasswordValidator<AppUser> passwordValidator;
		private IPasswordHasher<AppUser> passwordНasher;

		//управление пользователем
		private UserManager<AppUser> userManager;
		public AdminController(UserManager<AppUser> usrMgr,
			IUserValidator<AppUser> userValid,
			IPasswordValidator<AppUser> passValid,
			IPasswordHasher<AppUser> passwordНash)
		{
			this.userManager = usrMgr;
			this.userValidator = userValid;
			this.passwordValidator = passValid;
			this.passwordНasher = passwordНash;
		}

		//метод наполнения модели текущими ошибками
		public void AddErrorsFromResult(IdentityResult incomingResult)
		{
			foreach (var error in incomingResult.Errors)
			{
				ModelState.AddModelError("", error.Description);
				//ModelState.AddModelError(error.Code, error.Description); посмотреть варианты
			}
		}


		//просмотр пользователей
		public ViewResult Index() => View(userManager.Users);

		//создание пользователя
		public ViewResult Create() => View();

		[HttpPost]
		public async Task<IActionResult> Create(CreateModel model)
		{
			if (ModelState.IsValid)
			{
				AppUser user = new AppUser
				{
					UserName = model.Name,
					Email = model.Email
				};
				IdentityResult result = await userManager.CreateAsync(user, model.Password);
				if (result.Succeeded) return RedirectToAction("Index");
				else
				{
					foreach (IdentityError error in result.Errors) ModelState.AddModelError("", error.Description);
				}
			}
			return View(model);
		}

		//редактирование пользователя
		public async Task<IActionResult> Edit(string id)
		{
			AppUser user = await userManager.FindByIdAsync(id);
			if (user != null)
				return View(user);
			else
				return RedirectToAction("Index");
		}
		[HttpPost]
		public async Task<IActionResult> Edit(string Id, string email, string password, string name)
		{

			AppUser user = await userManager.FindByIdAsync(Id);
			if (user != null)
			{
				user.UserName = name;
				user.Email = email;
				IdentityResult validEmail = await userValidator.ValidateAsync(userManager, user);
				if (!validEmail.Succeeded) AddErrorsFromResult(validEmail);
				IdentityResult validPass = null;
				if (!string.IsNullOrEmpty(password))
				{
					validPass = await passwordValidator.ValidateAsync(userManager, user, password);
					if (validPass.Succeeded) user.PasswordHash = passwordНasher.HashPassword(user, password);
					else AddErrorsFromResult(validPass);
				}
				if ((validEmail.Succeeded && validPass == null) || (validEmail.Succeeded && password != string.Empty && validPass.Succeeded))
				{
					IdentityResult result = await userManager.UpdateAsync(user);
					if (result.Succeeded) return RedirectToAction("Index");
					else AddErrorsFromResult(result);
				}
			}
			else
				ModelState.AddModelError("", "User Not Found");
			return View(user);
		}

		//удаление пользователя
		public async Task<IActionResult> Delete(string id)
		{
			AppUser user = await userManager.FindByIdAsync(id);
			if (user != null)
			{
				IdentityResult deleteUser = await userManager.DeleteAsync(user);
				if (!deleteUser.Succeeded) AddErrorsFromResult(deleteUser);
				return RedirectToAction("Index");
			}
			else
				return RedirectToAction("Index");
		}

	}
}
