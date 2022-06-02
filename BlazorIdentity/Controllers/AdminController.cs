using BlazorIdentity.Models.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BlazorIdentity.Controllers
{

    [Authorize(Roles = "admin")]
    public class AdminController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IPasswordValidator<ApplicationUser> passwordValidator;
        private readonly IPasswordHasher<ApplicationUser> passwordHasher;

        public AdminController(UserManager<ApplicationUser> _userManager, 
            IPasswordValidator<ApplicationUser> passValidator, IPasswordHasher<ApplicationUser> passHasher)
        {
            userManager = _userManager;
            passwordValidator = passValidator;
            passwordHasher = passHasher;
        }
        public IActionResult Index()
        {
            return View(userManager.Users);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(RegisterModel p1)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = new ApplicationUser();
                user.UserName = p1.UserName;
                user.Email = p1.Email;
                var result = await userManager.CreateAsync(user, p1.Password);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    foreach (var item in result.Errors)
                    {
                        ModelState.AddModelError("", item.Description);
                    }
                }
            }

            return View(p1);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            var user = await userManager.FindByIdAsync(id);
            if (user != null)
            {
                var result = await userManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    RedirectToAction("Index");
                }
                else
                {
                    foreach (var item in result.Errors)
                    {
                        ModelState.AddModelError("", item.Description);
                    }
                }
            }
            else
            {
                ModelState.AddModelError("", "Kullanıcı bulunamadı");
            }

            return View("Index", userManager.Users);
        }

        [HttpGet]
        public async Task<IActionResult> Update(string id)
        {
            var user = await userManager.FindByIdAsync(id);
            if (user != null)
            {
                return View(user);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Update(string id, string Password, string Email)
        {
            var user = await userManager.FindByIdAsync(id);
            if (user != null)
            {
                user.Email = Email;
                IdentityResult validPass = null;
                if (!string.IsNullOrEmpty(Password))
                {
                    validPass = await passwordValidator.ValidateAsync(userManager, user, Password);
                    if (validPass.Succeeded)
                    {
                        user.PasswordHash = passwordHasher.HashPassword(user, Password);
                    }
                    else
                    {
                        foreach (var item in validPass.Errors)
                        {
                            ModelState.AddModelError("", item.Description);
                        }
                    }
                }
                if (validPass.Succeeded)
                {
                    var result = await userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        foreach (var item in result.Errors)
                        {
                            ModelState.AddModelError("", item.Description);
                        }
                    }
                }
            }
            else
            {
                ModelState.AddModelError("", "Kullanıcı bulunamadı");
            }
            return View(user);
        }


    }
}
