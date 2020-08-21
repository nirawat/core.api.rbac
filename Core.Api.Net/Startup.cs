using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Swashbuckle.AspNetCore.Swagger;

using NLog;
using NLog.Extensions.Logging;
using DryIoc;

//Helper
using Core.Api.Net.Helpers;
using Core.Api.Net.Business.Helpers.Logs;
using Core.Api.Net.Business.Helpers.Environment;

//Model
using Core.Api.Net.Business.Models.Configs;
using Core.Api.Net.Business.Models.Auth;
using Core.Api.Net.Business.Models.Response;

//Service
using Core.Api.Net.Business.Services.Auth;
using Core.Api.Net.Business.Services.TokenKey;

//Repository
using Core.Api.Net.Business.Repository.Auth;
using Core.Api.Net.Business.Repository.FuncMenu;
using Core.Api.Net.Business.Repository.TokenKey;
using Core.Api.Net.Business.Services.UserGroup;
using Core.Api.Net.Business.Repository.UserGroup;
using Core.Api.Net.Business.Services.User;
using Core.Api.Net.Business.Repository.User;
using Core.Api.Net.Business.Services.FuncMenu;
using Core.Api.Net.Business.Repository.PermissionGroup;
using Core.Api.Net.Business.Services.PermissionGroup;
using Core.Api.Net.Business.Services.PermissionRole;
using Core.Api.Net.Business.Repository.PermissionRole;
using Core.Api.Net.Business.Services.PermissionFunc;
using Core.Api.Net.Business.Repository.PermissionFunc;
using Core.Api.Net.Business.Services.Register;
using Core.Api.Net.Business.Repository.Register;
using Core.Api.Net.Business.Services.Section;
using Core.Api.Net.Business.Services.Role;
using Core.Api.Net.Business.Repository.Section;
using Core.Api.Net.Business.Repository.Role;
using Core.Api.Net.Business.Repository.Owner;
using Core.Api.Net.Business.Repository.Logs;
using Core.Api.Net.Business.Services.Dashboard;
using Core.Api.Net.Business.Repository.Dashboard;

namespace Core.Api.Net
{
    public class Startup
    {
        private static Logger _logger = LogManager.GetCurrentClassLogger();
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var cultureInfo = new CultureInfo("en-GB");
            CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
            CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

            EnvironmentModel EnvronmentSetting = new EnvironmentModel()
            {
                AppCodes = Configuration.GetSection("AppCode").Get<AppCode>(),
                DBRBAC = Configuration.GetSection("DBRBAC").Get<DBRBAC>(),
            };

            services.AddHttpClient();

            EnvronmentSetting.DBRBAC.ConnectionString =
            GetConnectionString(
                EnvronmentSetting.DBRBAC.ConnectionString,
                EnvronmentSetting.DBRBAC.DatabaseName,
                EnvronmentSetting.DBRBAC.Username,
                EnvronmentSetting.DBRBAC.Password);

            JwtConfigs jwtConfigs = GetJwtTokenConfigs(EnvronmentSetting, EnvronmentSetting.AppCodes.JwtCode);
            services.Configure<JwtConfigs>(Configuration.GetSection(nameof(JwtConfigs)));
            // services.AddSingleton<IJwtConfigs>(sp => sp.GetRequiredService<IOptions<JwtConfigs>>().Value);
            services.AddSingleton<IJwtConfigs>(jwtConfigs);
            var key = Encoding.ASCII.GetBytes(Encoding.UTF8.GetString(Convert.FromBase64String(jwtConfigs.Secret)));
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme; // allow roles
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidIssuer = jwtConfigs.Issuer,
                    ValidAudience = jwtConfigs.Audience,
                    ClockSkew = TimeSpan.Zero
                };
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Core API", Version = "v1" });
                var securityScheme = new OpenApiSecurityScheme()
                {
                    Description = "Please enter into field the word 'Bearer' followed by a space and the JWT value",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "bearer"
                };
                c.AddSecurityDefinition("Bearer", securityScheme);
                c.AddSecurityRequirement(new OpenApiSecurityRequirement { { new OpenApiSecurityScheme { Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" } }, new string[] { } } });
            });

            services.AddControllers();

            #region NLog
            Environment.SetEnvironmentVariable("ELASTICSEARCH_URL", "");
            #endregion
            var container = new Container();
            #region NLog
            container.UseInstance
            (
                new LoggerFactory().AddNLog(new NLogProviderOptions { CaptureMessageTemplates = true, CaptureMessageProperties = true })
            );
            container.Register(typeof(ILogger<>), typeof(Logger<>), Reuse.Singleton);
            container.Register<ILogs, Logs>(Reuse.Singleton);
            services.AddScoped(typeof(ILogs), typeof(Logs));

            #endregion

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            // Register Service
            services.AddScoped<IEnvironmentConfigs>(r => new EnvironmentConfigs(EnvronmentSetting));
            services.AddScoped<IDashboardService, DashboardService>();
            services.AddScoped<IRegisterService, RegisterService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<ITokenKeyService, TokenKeyService>();
            services.AddScoped<IUserGroupService, UserGroupService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IFuncMenuService, FuncMenuService>();
            services.AddScoped<IPermissionGroupService, PermissionGroupService>();
            services.AddScoped<IPermissionRoleService, PermissionRoleService>();
            services.AddScoped<IPermissionFuncService, PermissionFuncService>();
            services.AddScoped<ISectionService, SectionService>();
            services.AddScoped<IRoleService, RoleService>();

            // Register Repository
            services.AddScoped<IDashboardRepository, DashboardRepository>();
            services.AddScoped<IRegisterRepository, RegisterRepository>();
            services.AddScoped<IAuthRepository, AuthRepository>();
            services.AddScoped<ILogRepository, LogRepository>();
            services.AddScoped<IOwnerRepository, OwnerRepository>();
            services.AddScoped<IFuncMenuRepository, FuncMenuRepository>();
            services.AddScoped<ITokenKeyRepository, TokenKeyRepository>();
            services.AddScoped<IUserGroupRepository, UserGroupRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IFuncMenuRepository, FuncMenuRepository>();
            services.AddScoped<IPermissionGroupRepository, PermissionGroupRepository>();
            services.AddScoped<IPermissionRoleRepository, PermissionRoleRepository>();
            services.AddScoped<IPermissionFuncRepository, PermissionFuncRepository>();
            services.AddScoped<ISectionRepository, SectionRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();

        }

        private string GetConnectionString(string conn, string dbName, string user, string passw)
        {
            string conn_string = Encoding.UTF8.GetString(Convert.FromBase64String(conn));
            string db_name = Encoding.UTF8.GetString(Convert.FromBase64String(dbName));
            string db_username = Encoding.UTF8.GetString(Convert.FromBase64String(user));
            string db_password = Encoding.UTF8.GetString(Convert.FromBase64String(passw));
            string db_conn_string = string.Format(conn_string, db_name, db_username, db_password);
            return db_conn_string;
        }

        private JwtConfigs GetJwtTokenConfigs(EnvironmentModel setting, string AppCode)
        {
            DataTable dt = new DataTable();
            JwtConfigs jwt = new JwtConfigs();

            string conn_string = setting.DBRBAC.ConnectionString;
            string db_name = setting.DBRBAC.DatabaseName;
            string db_username = setting.DBRBAC.Username;
            string db_password = setting.DBRBAC.Password;
            string db_conn_string = string.Format(conn_string, db_name, db_username, db_password);

            string query = string.Format("SELECT TOP(1) * FROM SYS_JWT_TOKEN WHERE Code='{0}'", AppCode);

            using (var cnn = new SqlConnection(db_conn_string))
            {
                cnn.Open();
                using (var cmd = new SqlCommand(query, cnn))
                {
                    cmd.CommandText = query;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandTimeout = 60;
                    SqlDataAdapter adp = new SqlDataAdapter(cmd);
                    adp.Fill(dt);
                }
                cnn.Close();
            }
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    jwt.Secret = dr["Secret"].ToString();
                    jwt.Issuer = dr["Issuer"].ToString();
                    jwt.Audience = dr["Audience"].ToString();
                    jwt.Expires = Convert.ToInt32(dr["Expires"]);
                }
            }
            return jwt;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Api Core Service V.1");
            });


            //global cors policy
            app.UseCors(x => x
               .AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader());

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseMiddleware(typeof(ErrorHandlingMiddleware));
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

        }
    }
}
