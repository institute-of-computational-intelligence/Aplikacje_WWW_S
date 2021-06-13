using System;
using System.Net;
using System.Net.Mail;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SchoolRegister.DAL.EF;
using SchoolRegister.Model.DataModels;
using SchoolRegister.Services.Interfaces;
using SchoolRegister.Services.Services;
using SchoolRegister.ViewModels.VM;
using SchoolRegister.Web.Configuration.Profiles;

namespace SchoolRegister.Api
{
    public class Startup {
        public Startup (IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices (IServiceCollection services) {
            services.AddAutoMapper (typeof (MainProfile));
            services.Configure<JwtOptionsVm> (options => Configuration.GetSection ("JwtOptions").Bind (options));
            services.AddDbContext<ApplicationDbContext> (options =>
                options.UseSqlServer (Configuration.GetConnectionString ("DefaultConnection"))
            );
            services.AddIdentity<User, Role> (options => {
                    options.SignIn.RequireConfirmedAccount = false;
                    options.Password.RequiredLength = 6;
                    options.Password.RequiredUniqueChars = 0;
                    options.Password.RequireNonAlphanumeric = false;
                })
                .AddRoleManager<RoleManager<Role>> ()
                .AddUserManager<UserManager<User>> ()
                .AddEntityFrameworkStores<ApplicationDbContext> ()
                .AddDefaultTokenProviders ();
            services.AddTransient (typeof (ILogger), typeof (Logger<Startup>));
            services.AddScoped<ISubjectService, SubjectService> ();
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

            services.AddAuthentication (options => {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer (JwtBearerDefaults.AuthenticationScheme, options => {
                    options.TokenValidationParameters = new TokenValidationParameters {
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey (Encoding.UTF8.GetBytes (Configuration["JwtOptions:SecretKey"])),
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.FromMinutes (5)
                    };
                });

            services.AddControllers ()
                .AddNewtonsoftJson (options =>
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                );
            services.AddSwaggerGen (c => {
                c.SwaggerDoc ("v1", new OpenApiInfo { Title = "SchoolRegister.Api", Version = "v1" });
            });
        }

        public void Configure (IApplicationBuilder app, IWebHostEnvironment env) {
            if (env.IsDevelopment ()) {
                app.UseDeveloperExceptionPage ();
                app.UseSwagger ();
                app.UseSwaggerUI (c => c.SwaggerEndpoint ("/swagger/v1/swagger.json", "SchoolRegister.Api v1"));
            }
            app.UseHttpsRedirection ();
            app.UseRouting ();
            app.UseAuthentication ();
            app.UseAuthorization ();
            app.UseEndpoints (endpoints => {
                endpoints.MapControllers ();
            });
        }
    }
}