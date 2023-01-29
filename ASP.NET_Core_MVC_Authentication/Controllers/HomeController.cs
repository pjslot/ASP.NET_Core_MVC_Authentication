//(c)Kabluchkov DS 2023
using ASP.NET_Core_MVC_Authentication.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ASP.NET_Core_MVC_Authentication.Controllers
{
	public class HomeController : Controller
	{
		AppIdentityDbContext db;
		//private readonly ILogger<HomeController> _logger;

		public HomeController(AppIdentityDbContext ctx)
		{
			//получение ссылки на контекст
			db = ctx;
			//_logger = logger;
		}

		//авторизация
		//открыто только для админов!
		[Authorize(Roles = "Admin")]
		//[Authorize]
		public IActionResult Index()
		{
			return View();
		}
		//заглушка экшна эбаут - только для юзеров
		[Authorize(Roles = "Users")]
		public IActionResult About()
		{
			return Content("This is About page. Only for users");
		}

		public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}