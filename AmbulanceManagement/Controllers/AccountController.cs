using AmbulanceManagement.Data;
using AmbulanceManagement.ViewModels;
using AmbulanceManagement.Data;
using AmbulanceManagement.Models;
using AmbulanceManagement.Utility;
using AmbulanceManagement.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.AspNetCore.Authorization;
using System.Xml.Linq;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagement.Controllers
{
	public class AccountController : Controller
	{

		private ApplicationDbContext _dbContext;
		UserManager<ApplicationUser> _userManager;
		SignInManager<ApplicationUser> _signInManager;
		RoleManager<IdentityRole> _roleManager;

		public AccountController(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager,
		SignInManager<ApplicationUser> signInManager,
		RoleManager<IdentityRole> roleManager)
		{
			_dbContext = dbContext;
			_userManager = userManager;
			_signInManager = signInManager;
			_roleManager = roleManager;
		}

		public IActionResult Login()
		{
			return View();
		}

		[HttpPost]
		
		public async Task<IActionResult> Login([FromForm] LoginViewModel loginViewModel)
		{

			//Creating First user Admin
			if (_dbContext.Users.Count() > 0)
			{
                var user = new ApplicationUser()
                {
                    UserName = "Admin",
                    Email = "Admin@admin.com",
                    Name = "Admin",
                    Number = 00,
                    DateOfBirth = new DateTime(2008, 3, 15),
                    Gender = Gender.Male,
                    Education = "Admin",
                    Type = "Admin",
                    Biography = "Admin"
                };

                var res = await _userManager.CreateAsync(user, "Admin123@");

                if (res.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, Helper.Admin);
                    await _signInManager.SignInAsync(user, isPersistent: false);

                }
            }
     

            if (!ModelState.IsValid)
				return View(loginViewModel);

			var result = await _signInManager.PasswordSignInAsync(loginViewModel.Email, loginViewModel.Password, loginViewModel.RememberMe, false);

			if (result.Succeeded)
			{
				return RedirectToAction("Index", "Home");
			}

			ModelState.AddModelError("", "Invalid Login");

			return View();
		}

		[HttpPost]
		public async Task<IActionResult> LogOff()
		{
			await _signInManager.SignOutAsync();
			return RedirectToAction("Login", "Account");
		}

		[Authorize(Roles = "Admin")]
		public IActionResult Register()
		{
			return View();
		}


		[HttpPost]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> Register(RegisterViewModel model)
		{
			if (!ModelState.IsValid)
			{
				return View();

			}

			if (!_roleManager.RoleExistsAsync(Helper.Admin).GetAwaiter().GetResult())
			{
				await _roleManager.CreateAsync(new IdentityRole(Helper.Admin));
				await _roleManager.CreateAsync(new IdentityRole(Helper.Nurse));
				await _roleManager.CreateAsync(new IdentityRole(Helper.Doctor));
			}

			var user = new ApplicationUser()
			{
				UserName = model.Email,
				Email = model.Email,
				Name = model.Name,
				Number = model.Number,
				DateOfBirth = model.DateOfBirth,
				Gender = model.Gender,
				Education = model.Education,
				Type = model.Type,
				Biography = model.Biography
			};

			var result = await _userManager.CreateAsync(user, model.Password);

			if (result.Succeeded)
			{
				await _userManager.AddToRoleAsync(user, model.RoleName);
				await _signInManager.SignInAsync(user, isPersistent: false);
				RedirectToAction("Index", "Home");
			}
			foreach (var error in result.Errors)
			{
				ModelState.AddModelError("", error.Description);
			}

			return View(model);
		}

        public async Task<IActionResult> ListAll()
        {
            return _dbContext.Users != null ?
                        View(await _dbContext.Users.ToListAsync()) :
                        Problem("Entity set 'ApplicationDbContext.User'  is null.");
        }

        [Authorize(Roles = "Admin")]
		public async Task<IActionResult> Edit(string id)
		{
			var user = await _userManager.FindByIdAsync(id);

			if (user == null)
			{
				return NotFound();
			}

			var editViewModel = new EditAccountViewModel
			{
				Id = user.Id,
				Name = user.Name,
				Number = user.Number,
				DateOfBirth = user.DateOfBirth,
				Gender = user.Gender,
				Education = user.Education,
				Type = user.Type,
				Biography = user.Biography
			};

			return View(editViewModel);
		}

		[HttpPost]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> Edit(EditAccountViewModel model)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}

			var user = await _userManager.FindByIdAsync(model.Id);

			if (user == null)
			{
				return NotFound();
			}

			// Update user properties based on the changes in the model
			user.Name = model.Name;
			user.Number = model.Number;
			user.DateOfBirth = model.DateOfBirth;
			user.Gender = model.Gender;
			user.Education = model.Education;
			user.Type = model.Type;
			user.Biography = model.Biography;

			//if(model.Password != "")
			//{
   //             user.PasswordHash = _userManager.PasswordHasher.HashPassword(user,model.Password);
   //             var res = await _userManager.UpdateAsync(user);
   //             if (!res.Succeeded)
   //             {
   //                 //throw exception......
   //             }
   //         }
			// Update other properties as needed

			var result = await _userManager.UpdateAsync(user);

			if (result.Succeeded)
			{
				// Redirect to user profile or another appropriate page
				return RedirectToAction("Index", "Home");
			}

			foreach (var error in result.Errors)
			{
				ModelState.AddModelError("", error.Description);
			}

			return View(model);
		}


		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> Delete(string id)
		{
			var user = await _userManager.FindByIdAsync(id);

			if (user == null)
			{
				return NotFound();
			}

			return View(user);
		}

		[HttpPost, ActionName("Delete")]
		[Authorize(Roles = "Admin")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(string id)
		{
			var user = await _userManager.FindByIdAsync(id);

			if (user == null)
			{
				return NotFound();
			}

			var result = await _userManager.DeleteAsync(user);

			if (result.Succeeded)
			{
				// Redirect to the user list or another appropriate page
				return RedirectToAction("Index", "Home");
			}

			foreach (var error in result.Errors)
			{
				ModelState.AddModelError("", error.Description);
			}

			return View(user);
		}

	}
}
