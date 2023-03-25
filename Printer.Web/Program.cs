using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Printer.Data;
using Printer.Data.Initializer;
using Printer.Data.Repository.Interface;
using Printer.Data.Repository.Service;
using Printer.Utilities;
using Stripe;

public class Program
{
	public Program(IConfiguration configuration)
	{
		Configuration = configuration;
	}
	public IConfiguration Configuration { get; }

	public static void Main(string[] args)
	{
		var builder = WebApplication.CreateBuilder(args);
		var services = builder.Services;
		var configuration = builder.Configuration;


		// Add services to the container.
		builder.Services.Configure<StripeSettings>(builder.Configuration.GetSection("Stripe"));
		builder.Services.AddControllersWithViews();
		builder.Services.AddDbContext<RepositoryContext>(options => options.UseSqlServer(
			builder.Configuration.GetConnectionString("DefaultConnection")
			));
		builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddDefaultTokenProviders()
			.AddEntityFrameworkStores<RepositoryContext>();
		builder.Services.AddSingleton<IEmailSender, EmailSender>();
		builder.Services.AddScoped<IDbInitializer, DbInitializer>();
		builder.Services.ConfigureApplicationCookie(options =>
		{
			options.LoginPath = $"/Identity/Account/Login";
			options.LogoutPath = $"/Identity/Account/Logout";
			options.AccessDeniedPath = $"/Identity/Account/AccessDenied";
		});

		builder.Services.AddDistributedMemoryCache();
		builder.Services.AddSession(options =>
		{
			options.IdleTimeout = TimeSpan.FromMinutes(100);
			options.Cookie.HttpOnly = true;
			options.Cookie.IsEssential = true;
		});


		services.AddAuthentication()
	.AddGoogle(options =>
	{
		options.ClientId = configuration["GoogleAuthOptions:ClientId"];
		options.ClientSecret = configuration["GoogleAuthOptions:ClientSecret"];
	})
		.AddFacebook(options =>
         {
             options.AppId = configuration["FacebookAuthOptions:AppId"];
             options.AppSecret = configuration["FacebookAuthOptions:AppSecret"];
         });

        builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
		builder.Services.AddRazorPages().AddRazorRuntimeCompilation();
		var app = builder.Build();

		// Configure the HTTP request pipeline.
		if (app.Environment.IsDevelopment())
		{
			app.UseDeveloperExceptionPage();
		}
		else
		{
			app.UseExceptionHandler("/Home/Error");
			// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
			app.UseHsts();
		}

		app.UseHttpsRedirection();
		app.UseStaticFiles();

		app.UseRouting();

		StripeConfiguration.ApiKey = builder.Configuration.GetSection("Stripe:SecretKey").Get<string>();
		SeedDatabase();

		app.UseAuthentication();

		app.UseAuthorization();
		app.UseSession();
		app.MapRazorPages();
		app.MapControllerRoute(
			name: "default",
			pattern: "{area=Customer}/{controller=Home}/{action=Index}/{id?}");

		app.Run();


		void SeedDatabase()
		{
			using (var scope = app.Services.CreateScope())
			{
				var dbInitializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
				dbInitializer.Initialize();
			}
		}
	}
}