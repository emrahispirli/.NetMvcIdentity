using BlazorIdentity.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationIdentityDbContext>(options =>
           options.UseSqlServer("Server =.\\SQLEXPRESS; Database = aaa; Trusted_Connection = True; MultipleActiveResultSets = true"));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>
    (option =>
    {
        option.User.RequireUniqueEmail = true;
        option.Password.RequireDigit = false;
        option.Password.RequireLowercase = false;
        option.Password.RequireUppercase = false;
        option.Password.RequireNonAlphanumeric = false;
    })
    .AddEntityFrameworkStores<ApplicationIdentityDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddControllersWithViews();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();


app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
          //pattern: "{controller=Account}/{action=Login}/{id?}");
          pattern: "{controller=Home}/{action=Index}/{id?}");
});
//app.MapRazorPages();
app.Run();
