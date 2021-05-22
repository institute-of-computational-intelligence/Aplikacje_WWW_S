using System;
using System.IO;
using System.Net;
using System.Net.Mail;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SchoolRegister.DAL.EF;
using SchoolRegister.Model.DataModels;
using SchoolRegister.Services.Interfaces;
using SchoolRegister.Services.Services;
using SchoolRegister.Web.Configuration.Profiles;

namespace SchoolRegister.Tests {
    public class Startup {
        public Startup () {
       
        }
        public void ConfigureServices (IServiceCollection services) {
            services.AddAutoMapper (typeof (MainProfile));
            services.AddEntityFrameworkInMemoryDatabase ()
                .AddDbContext<ApplicationDbContext> (options =>
                    options.UseInMemoryDatabase ("InMemoryDb")
                );
            services.AddIdentity<User, Role> (options => {
                    options.SignIn.RequireConfirmedAccount = false;
                    options.Password.RequiredLength = 6;
                    options.Password.RequiredUniqueChars = 0;
                    options.Password.RequireNonAlphanumeric = false;
                })
                .AddRoleManager<RoleManager<Role>> ()
                .AddUserManager<UserManager<User>> ()
                .AddEntityFrameworkStores<ApplicationDbContext> ();
            services.AddTransient (typeof (ILogger), typeof (Logger<Startup>));
            services.AddTransient<ISubjectService, SubjectService> ();
            services.AddTransient<IEmailSenderService, EmailSenderService> ();
            services.AddTransient<IGradeService, GradeService> ();
            services.AddTransient<IGroupService, GroupService> ();
            services.AddTransient<IStudentService, StudentService> ();
            services.AddTransient<ITeacherService, TeacherService> ();
            services.AddSingleton<IConfiguration>(x=> {
                     var builder = new ConfigurationBuilder ()
                .SetBasePath (Directory.GetCurrentDirectory ())
                .AddJsonFile ("appsettings.json", optional : false, reloadOnChange : true)
                .AddEnvironmentVariables ();
                return builder.Build ();
            });
            services.AddTransient ((serviceProvider) => {
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
            services.SeedData ();
        }

    }

}