﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Сatalog.Models;

namespace Сatalog
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
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });


            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);


            var sqlConnectionString = Configuration.GetConnectionString("DefaultConnection");


            // TODO: Я не знаю по какой причине данный подход не срабатывает.
            // Как вариант можно загружать json файл конфигурации вручную c диска и полуать необходимое значение.
            //string connection = Configuration.GetConnectionString("postgresql");

            string connection = "Host=127.0.0.1;Port=5432;Username=postgres;Password=RrTtYy1739;Database=company_directory;";

            /*services.AddEntityFrameworkNpgsql().AddDbContext<ApplicationContext>(opt =>
               opt.UseNpgsql(connectionString: connect)
            );*/

            services.AddEntityFrameworkNpgsql()
               .AddDbContext<ApplicationContext>()
               .BuildServiceProvider();
            
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
