using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Fiver.Asp.SessionState
{
    public class Startup
    {
        //public void ConfigureServices(
        //    IServiceCollection services)
        //{
        //    services.AddDistributedMemoryCache();
        //    services.AddSession();
        //}

        public void ConfigureServices(
           IServiceCollection services)
        {
            services.AddDistributedMemoryCache();
            services.AddSession(options =>
            {
                options.Cookie.HttpOnly = true;
                options.Cookie.Name = ".Fiver.Session";
                //options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                options.IdleTimeout = TimeSpan.FromMinutes(10);
            });
        }

        //public void Configure(
        //    IApplicationBuilder app, 
        //    IHostingEnvironment env)
        //{
        //    app.UseSession();

        //    app.Use(async (context, next) =>
        //    {
        //        context.Session.SetString("GreetingMessage", "Hello Session State");
        //        await next();
        //    });

        //    app.Run(async (context) =>
        //    {
        //        var message = context.Session.GetString("GreetingMessage");
        //        await context.Response.WriteAsync($"{message}");
        //    });
        //}

        public void Configure(
            IApplicationBuilder app,
            IHostingEnvironment env)
        {
            app.UseSession();

            app.Use(async (context, next) =>
            {
                context.Session.SetObject("CurrentUser",
                    new UserInfo { Username = "James", Email = "james@bond.com" });
                await next();
            });

            app.Run(async (context) =>
            {
                var user = context.Session.GetObject<UserInfo>("CurrentUser");
                await context.Response.WriteAsync($"{user.Username}, {user.Email}");
            });
        }
    }
}
