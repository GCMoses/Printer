using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Printer.Core.Models;
using Printer.Utilities;

namespace Printer.Data.Initializer;

public class DbInitializer : IDbInitializer
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly RepositoryContext _db;
    public IConfiguration _config { get; }


    public DbInitializer(
        UserManager<IdentityUser> userManager,
        RoleManager<IdentityRole> roleManager,
        RepositoryContext    db, IConfiguration config)
    {
        _roleManager = roleManager;
        _userManager = userManager;
        _db = db;
        _config = config;
    }


    public void Initialize()
    {
        //migrations if they are not applied
        try
        {
            if (_db.Database.GetPendingMigrations().Count() > 0)
            {
                _db.Database.Migrate();
            }
        }
        catch (Exception ex)
        {

        }

        //create roles if they are not created
        if (!_roleManager.RoleExistsAsync(Statics.Role_Admin).GetAwaiter().GetResult())
        {
            _roleManager.CreateAsync(new IdentityRole(Statics.Role_Admin)).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole(Statics.Role_Employee)).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole(Statics.Role_User_Indi)).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole(Statics.Role_User_Comp)).GetAwaiter().GetResult();

            //if roles are not created, then we will create admin user as well

            _userManager.CreateAsync(new AppUser
            {
                UserName = _config.GetSection("BongoMan:UserName").Get<string>(),
                Email = _config.GetSection("BongoMan:Email").Get<string>(),
                Name = _config.GetSection("BongoMan:Name").Get<string>(),
                PhoneNumber = _config.GetSection("BongoMan:PhoneNumber").Get<string>(),
                StreetAddress = _config.GetSection("BongoMan:StreetAddress").Get<string>(),
                State = _config.GetSection("BongoMan:State").Get<string>(),
                PostalCode = _config.GetSection("BongoMan:PostalCode").Get<string>(),
                City = _config.GetSection("BongoMan:City").Get<string>()
            }, _config.GetSection("BongoMan:Pass").Get<string>()).GetAwaiter().GetResult();

            AppUser user = _db.AppUsers.FirstOrDefault(u => u.Email == _config.GetSection("BongoMan")["Email"]);

            _userManager.AddToRoleAsync(user, Statics.Role_Admin).GetAwaiter().GetResult();

        }
        return;
    }
}