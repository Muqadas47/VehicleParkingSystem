using Newtonsoft.Json;
using System;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using VehicleParkingSystem.Data;
using VehicleParkingSystem.Models;
using VehicleParkingSystem.Services;

namespace VehicleParkingSystem.Controllers
{
	public class AccountController : Controller
	{
		private readonly ApplicationDbContext db =
			new ApplicationDbContext();

		private readonly PasswordHasher passwordHasher =
			new PasswordHasher();

		private readonly JwtService jwtService =
			new JwtService();


		[HttpGet]
		public ActionResult Login()
		{
			return View();
		}

		[HttpPost]
		public JsonResult Login(LoginRequest model)
		{
			try
			{
				if (model == null ||
					string.IsNullOrWhiteSpace(model.Email) ||
					string.IsNullOrWhiteSpace(model.Password))
				{
					return Json(new
					{
						success = false,
						message = "Email and password are required."
					});
				}


				if (string.IsNullOrWhiteSpace(model.CaptchaToken))
				{
					return Json(new
					{
						success = false,
						message = "Please verify the CAPTCHA."
					});
				}


				
				bool captchaValid = VerifyCaptcha(model.CaptchaToken);

				if (!captchaValid)
				{
					return Json(new
					{
						success = false,
						message = "CAPTCHA verification failed."
					});
				}
				string email = model.Email.Trim();

				var user = db.Users.FirstOrDefault(x => x.Email == email);

				if (user == null)
				{
					return Json(new
					{
						success = false,
						message = "Invalid password or  email "
					});
				}


				
				if (!user.IsActive)
				{
					return Json(new
					{
						success = false,
						message = "Your account is inactive."
					});
				}


				
				bool passwordValid =
					passwordHasher.VerifyPassword(
						model.Password,
						user.PasswordHash
					);

				if (!passwordValid)
				{
					return Json(new
					{
						success = false,
						message = "Invalid  password."
					});
				}


				string token = jwtService.GenerateToken(user);


				return Json(new
				{
					success = true,
					message = "Login successful.",
					token = token,
					user = new
					{
						userId = user.UserId,
						name = user.FullName,
						email = user.Email,
						role = user.Role
					}
				});
			}
			catch (Exception)
			{
				return Json(new
				{
					success = false,
					message = "Something went wrong. Please try again."
				});
			}
		}


		private bool VerifyCaptcha(string captchaToken)
		{
			try
			{
				string secretKey =
					ConfigurationManager.AppSettings["RecaptchaSecretKey"];

				using (var client = new WebClient())
				{
					string url =
						"https://www.google.com/recaptcha/api/siteverify" +
						"?secret=" + Uri.EscapeDataString(secretKey) +
						"&response=" + Uri.EscapeDataString(captchaToken);

					string response = client.DownloadString(url);

					var captchaResponse =
						JsonConvert.DeserializeObject<RecaptchaResponse>(
							response
						);

					return captchaResponse != null &&
						   captchaResponse.Success;
				}
			}
			catch
			{
				return false;
			}
		}


		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				db.Dispose();
			}

			base.Dispose(disposing);
		}
	}
}