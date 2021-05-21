using System.Globalization;
using System.Net;
using System.Net.Mail;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SchoolRegister.DAL.EF;
using SchoolRegister.Model.DataModels;
using SchoolRegister.Services.Interfaces;
using SchoolRegister.Services.Services;
using SchoolRegister.Web.Configuration.Profiles;
using SchoolRegister.Web.Controllers;

namespace SchoolRegister.Web {
    public class Startup {
        public IConfiguration Configuration { get; }
        public Startup (IConfiguration configuration) {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices (IServiceCollection services) {
            services.AddAutoMapper (typeof (MainProfile));
            services.AddDbContext<ApplicationDbContext> (options =>
                options.UseSqlServer (Configuration.GetConnectionString ("DefaultConnection")) //here you can define a database type.
            );
            services.AddDatabaseDeveloperPageExceptionFilter ();
            services.AddDefaultIdentity<User> (options => options.SignIn.RequireConfirmedAccount = false)
                .AddRoles<Role> ()
                .AddRoleManager<RoleManager<Role>> ()
                .AddUserManager<UserManager<User>> ()
                .AddEntityFrameworkStores<ApplicationDbContext> ();
            services.AddTransient (typeof (ILogger), typeof (Logger<Startup>));
            services.AddTransient<IStringLocalizer, StringLocalizer<BaseController>>();
            services.AddScoped<ISubjectService, SubjectService> ();
            services.AddScoped<IEmailSender, EmailSenderService> ();
            services.AddScoped<IEmailSenderService, EmailSenderService> ();
            services.AddScoped<IGradeService, GradeService> ();
            services.AddScoped<IGroupService, GroupService> ();
            services.AddScoped<IStudentService, StudentService> ();
            services.AddScoped<ITeacherService, TeacherService> ();
            services.AddScoped ((serviceProvider) => {
                var config = serviceProvider.GetRequiredService<IConfiguration> ();
                return new SmtpClient () {
                    Host = config.GetValue<string> ("Email:Smtp:Host"),
                        Port = config.GetValue<int> ("Email:Smtp:Port"),
                        EnableSsl = true,
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        UseDefaultCredentials = false,
                        Credentials = new NetworkCredential (
                            config.GetValue<string> ("Email:Smtp:Username"),
                            config.GetValue<string> ("Email:Smtp:Password")
                        )
                };
            });
            services.Configure<RequestLocalizationOptions> (options => {
                var supportedCultures = new [] {
                new CultureInfo ("en"),
                new CultureInfo ("pl-PL")
                };

                options.DefaultRequestCulture = new RequestCulture (culture: "en", uiCulture: "en");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
            });
            services.AddLocalization (options => options.ResourcesPath = "Resources");
            services.AddControllersWithViews ()
                .AddViewLocalization ()
                .AddDataAnnotationsLocalization ();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure (IApplicationBuilder app, IWebHostEnvironment env) {
            if (env.IsDevelopment ()) {
                app.UseDeveloperExceptionPage ();
                app.UseMigrationsEndPoint ();
            } else {
                app.UseExceptionHandler ("/Error");
                app.UseHsts ();
            }
            app.UseHttpsRedirection ();
            app.UseStaticFiles ();
            app.UseRouting ();
            app.UseAuthentication ();
            app.UseAuthorization ();
            var localizationOption = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();
            app.UseRequestLocalization(localizationOption.Value);
            app.UseEndpoints (endpoints => {
                endpoints.MapControllerRoute (
                    name: "default",
                    pattern: "{controller=Subject}/{action=Index}/{id?}");
                endpoints.MapRazorPages ();
            });
        }
    }
}
