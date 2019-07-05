using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using TraceServer.Domain.Traces;
using TraceServer.Hubs;

namespace TraceServer
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSignalR();
            services.AddSingleton<IClientTracerFactory, ClientTracerFactory>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSignalR(routes =>
            {
                routes.MapHub<TraceHub>("/Hubs/TraceHub");
            });

            app.UseStaticFiles();

            app.Run(async (context) =>
            {
                if (context.Request.Path == "/")
                {
                    await Task.Run(() => context.Response.Redirect("index.html", false));
                }
                await context.Response.WriteAsync("Hello World! Path: " + context.Request.Path);
            });
        }
    }
}
