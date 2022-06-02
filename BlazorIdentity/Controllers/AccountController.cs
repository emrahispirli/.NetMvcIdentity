using BlazorIdentity.Models.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BlazorIdentity.Controllers
{

    //[Authorize(Roles = "admin")]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signinManager;
        private readonly RoleManager<IdentityRole> roleManager;
        public AccountController(UserManager<ApplicationUser> _userManager, SignInManager<ApplicationUser> _signinManager, RoleManager<IdentityRole> _roleManager)
        {
            userManager = _userManager;
            signinManager = _signinManager;
            roleManager = _roleManager;
        }

        [AllowAnonymous]
        public IActionResult Login(string? returnUrl)
        {
            ViewBag.returnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LoginAsync(LoginModel model, string returnUrl)
        {

            if (ModelState.IsValid)
            {
                var user = await userManager.FindByEmailAsync(model.Email);
                if (user != null)
                {
                    var result = await signinManager.PasswordSignInAsync(user, model.Password, false, false);
                    if (result.Succeeded)
                    {
                        return Redirect(returnUrl?? "Home/Index");
                    }
                }
                ModelState.AddModelError("Email", "Geçersiz e-mail veya şifre");

                // baslangıc asamasında kullanıcı adı ve roller tanımladım 
                var usersay = userManager.Users.Count();
                if (usersay == 0)
                {
                    ApplicationUser usernull = new ApplicationUser();
                    usernull.UserName = "emrah";
                    usernull.Email = "emrahispirli@hotmail.com.tr";
                    var result2 = await userManager.CreateAsync(usernull, "Em:531486");
                    if (result2.Succeeded)
                    {
                        var rolenull = roleManager.Roles.Count();
                        if (rolenull == 0)
                        {
                            string name = "admin";
                            await roleManager.CreateAsync(new IdentityRole(name));
                            //string name2 = "user";
                            //await roleManager.CreateAsync(new IdentityRole(name2));
                        }
                        return RedirectToAction("Login");
                    }
                }
            }
            //return View(model);
            return Redirect(returnUrl ?? "Home/Index");
        }

        [AllowAnonymous]
        public async Task<IActionResult> Logout()
        {
            await signinManager.SignOutAsync();
            //return RedirectToAction("Login", "Account");
            return RedirectToAction("Index", "Admin");
        }

        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }



    }
}
