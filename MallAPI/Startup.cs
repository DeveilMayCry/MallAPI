using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MallAPI.DataModel.Response;
using MallAPI.Filter;
using MallAPI.Model;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

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
            services.AddControllers(option=> option.Filters.Add<ExceptionHandlerAttribute>()).ConfigureApiBehaviorOptions(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    var errors = context.ModelState;
                    var errorMsgs= errors.Values.Select(v => v.Errors.First().ErrorMessage);
                    var msg = string.Join('|',errorMsgs);
                    return new BadRequestObjectResult(new Response(Enum.ResultEnum.Fail, msg));
                };
            });
            services.AddTransient<Product>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
