using JLGApps.SignNow.Models;
using JLGApps.SignNow.Models.SignNow;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Serialization;
using System.IO;

namespace JLGApps.SignNow
{
    public class Startup
    {
        public static int Progress { get; set; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddMvc(options => options.EnableEndpointRouting = false);
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0).AddSessionStateTempDataProvider();
           
            services.AddControllers().AddNewtonsoftJson(options =>{options.SerializerSettings.ContractResolver = new DefaultContractResolver();});
            services.AddDbContext<EmailTemplateDetailsContext>(options => options.UseSqlServer(Configuration.GetConnectionString("jlgAppConnection")));
            services.Configure<AuthenticationModel>(Configuration.GetSection("AUTH_SETTINGS"));

            services.Configure<FormOptions>(o => {
                o.ValueLengthLimit = int.MaxValue;
                o.MultipartBodyLengthLimit = int.MaxValue;
                o.MemoryBufferThreshold = int.MaxValue;
            });


            services.AddSession();
            // This prevents 400 Bad Request Browser Error From occuring on form POST
          
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
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
           
            app.UseCors("CorsPolicy");
            app.UseStaticFiles();
         
            app.UseRouting();

            app.UseAuthorization();
            app.UseSession();
            app.UseMvc();



            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapControllerRoute(name: "email",
            //    pattern: "email/{*article}",
            //    defaults: new { controller = "Email", action = "Index" });

            //    endpoints.MapControllerRoute(
            //        name: "default",
            //        pattern: "{controller=Home}/{action=Index}/{id?}");


            //});

      
        }
    }
}
