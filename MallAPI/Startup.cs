using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using MallAPI.DTO.Response;
using MallAPI.Filter;
using MallAPI.Formatter;
using MallAPI.Model;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using MallAPI.Authorization;

namespace MallAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers(option => option.Filters.Add<ExceptionHandlerAttribute>()).ConfigureApiBehaviorOptions(options =>
             {
                 options.InvalidModelStateResponseFactory = context =>
                 {
                     var errors = context.ModelState;
                     var errorMsgs = errors.Values.Select(v => v.Errors.First().ErrorMessage);
                     var msg = string.Join('|', errorMsgs);
                     return new BadRequestObjectResult(new Response(Enum.ResultEnum.Fail, msg));
                 };
             })
             .AddJsonOptions(configure =>
             {
                 //��ʽ��json�����е�datetime
                 configure.JsonSerializerOptions.Converters.Add(new DatetimeJsonConverter());
             });

            services.AddAuthentication(option =>
            {
                option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
             .AddJwtBearer(option =>
             {
                 option.TokenValidationParameters = new TokenValidationParameters
                 {
                     ValidIssuer = Configuration["JwtSettiing:ValidIssuer"],
                     ValidAudience = Configuration["JwtSettiing:ValidAudience"],
                     IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JwtSettiing:IssuerSigningKey"])),
                     //����ʱ�� = expire+clockskew
                     ClockSkew = TimeSpan.FromSeconds(20)
                 };

                 option.Events = new JwtBearerEvents
                 {
                     //�˴�ΪȨ����֤ʧ�ܺ󴥷����¼�
                     OnChallenge = context =>
                     {
                         //�˴�����Ϊ��ֹ.Net CoreĬ�ϵķ������ͺ����ݽ�����������ҪŶ������
                         context.HandleResponse();
                         //�Զ����Լ���Ҫ���ص����ݽ����������Ҫ���ص���Json����ͨ������Newtonsoft.Json�����ת��
                         var payload = JsonConvert.SerializeObject(new Response(Enum.ResultEnum.Fail,"���ȵ�¼"));
                         //�Զ��巵�ص���������
                         context.Response.ContentType = "application/json";
                         //�Զ��巵��״̬�룬Ĭ��Ϊ401 ������ĳ� 200
                         context.Response.StatusCode = StatusCodes.Status200OK;
                         //���Json���ݽ��
                         context.Response.WriteAsync(payload);
                         return Task.CompletedTask;
                     },
                     //�˴�Ϊ��Ȩʧ�ܺ󴥷����¼�
                     OnForbidden = context =>
                     {
                         //�Զ����Լ���Ҫ���ص����ݽ����������Ҫ���ص���Json����ͨ������Newtonsoft.Json�����ת��
                         var payload = JsonConvert.SerializeObject(new Response(Enum.ResultEnum.Fail, "Ȩ�޲���"));
                         //�Զ��巵�ص���������
                         context.Response.ContentType = "application/json";
                         //�Զ��巵��״̬�룬Ĭ��Ϊ401 ������ĳ� 200
                         context.Response.StatusCode = StatusCodes.Status200OK;
                         //���Json���ݽ��
                         context.Response.WriteAsync(payload);
                         return Task.CompletedTask;
                     },
                 };
             });

            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Description = "���¿�����������ͷ����Ҫ���Jwt��ȨToken��Bearer Token",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] { }
                    }
                });

                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);

            });

            services.AddTransient<Product>();
            services.AddTransient<User>();
            //ע����Ȩ������
            services.AddSingleton<IAuthorizationHandler, PermissionHandler>();
            services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            app.UseAuthentication();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });


            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
