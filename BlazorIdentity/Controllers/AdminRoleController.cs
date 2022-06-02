using BlazorIdentity.Models.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BlazorIdentity.Controllers
{

    [Authorize(Roles = "admin")]
    public class AdminRoleController : Controller
    {
        private UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public AdminRoleController(RoleManager<IdentityRole> _roleManager,
            UserManager<ApplicationUser> _userManager)
        {
            roleManager = _roleManager;
            userManager = _userManager;
        }
        public IActionResult Index()
        {
            return View(roleManager.Roles);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(string name)
        {
            if (ModelState.IsValid)
            {

                var result = await roleManager.CreateAsync(new IdentityRole(name));
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            return View(name);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            var role = await roleManager.FindByIdAsync(id);
            if (role != null)
            {
                var result = await roleManager.DeleteAsync(role);
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
                ModelState.AddModelError("", "Role bulunamadı");
            }

            return View("Index", roleManager.Roles);
        }

        public async Task<IActionResult> Edit(string Id)
        {
            IdentityRole role = await roleManager.FindByIdAsync(Id);
            var members = new List<ApplicationUser>();
            var nonmembers = new List<ApplicationUser>();

            foreach (var user in userManager.Users.ToList())
            {
                var list = await userManager.IsInRoleAsync(user, role.Name) ? members : nonmembers;
                list.Add(user);
            }
            var model = new RoleDetails()
            {
                Role = role,
                Members = members,
                NonMembers = nonmembers
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(RoleEditModel model)
        {
            if (!ModelState.IsValid)
            {
                foreach (var userid in model.IdsAdd ?? new string[] { })
                {
                    var user = await userManager.FindByIdAsync(userid);
                    if (user != null)
                    {
                        var result = await userManager.AddToRoleAsync(user, model.RoleName);
                        if (!result.Succeeded)
                        {
                            foreach (var error in result.Errors)
                            {
                                ModelState.AddModelError("", error.Description);
                            }
                        }
                    }
                }

                foreach (var userid in model.IdsDelete ?? new string[] { })
                {
                    var user = await userManager.FindByIdAsync(userid);
                    if (user != null)
                    {
                        var result = await userManager.RemoveFromRoleAsync(user, model.RoleName);
                        if (!result.Succeeded)
                        {
                            foreach (var error in result.Errors)
                            {
                                ModelState.AddModelError("", error.Description);
                            }
                        }
                    }
                }
            }
            // return RedirectToAction("Edit", model.RoleId);
            return RedirectToAction("Index");
        }

    }
}
