using System;
using System.Reflection;
using Domain.Contracts;
using FluentValidation;
using Infraestructure.Base;
using Infraestructure.Context;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WebApi.Infrastructure;

namespace WebApi {
	public class Startup {
		public Startup (IConfiguration configuration) {
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices (IServiceCollection services) {

			var connectionString = Configuration.GetConnectionString ("ExpedientesContext");
			services.AddDbContext<FinanzasContext>
				(opt => opt.UseSqlServer (connectionString));

			services.AddTransient (typeof (IPipelineBehavior<,>), typeof (ValidatorPipelineBehavior<,>));

			InyeccionFluentValidations (services);
			services.AddScoped<IDbContext, FinanzasContext> ();
			services.AddScoped<IUnitOfWork, UnitOfWork> ();

			services.AddMediatR (Assembly.Load ("Application"));
			services.AddMediatR (Assembly.Load ("WebApi"));
			services.AddControllersWithViews ();
			// In production, the Angular files will be served from this directory
			services.AddSpaStaticFiles (configuration => {
				configuration.RootPath = "ClientApp/dist";
			});

			#region SwaggerOpen Api
			//Register the Swagger services
			services.AddSwaggerGen (c => {
				c.SwaggerDoc ("v1", new OpenApiInfo {
					Version = "v1",
						Title = "Task API",
						Description = "Task API - ASP.NET Core Web API",
						TermsOfService = new Uri ("https://cla.dotnetfoundation.org/"),
						Contact = new OpenApiContact {
							Name = "Unicesar",
								Email = string.Empty,
								Url = new Uri ("https://github.com/borisgr04/CrudNgDotNetCore3"),
						},
						License = new OpenApiLicense {
							Name = "Licencia dotnet foundation",
								Url = new Uri ("https://www.byasystems.co/license"),
						}
				});
			});

			#endregion
		}
		private void InyeccionFluentValidations (IServiceCollection services) {
			AssemblyScanner.FindValidatorsInAssembly (Assembly.Load ("Application")).ForEach (pair => {
				// RegisterValidatorsFromAssemblyContaing does this:
				services.Add (ServiceDescriptor.Scoped (pair.InterfaceType, pair.ValidatorType));
				// Also register it as its concrete type as well as the interface type
				services.Add (ServiceDescriptor.Scoped (pair.ValidatorType, pair.ValidatorType));
			});

			AssemblyScanner.FindValidatorsInAssembly (Assembly.Load ("WebApi")).ForEach (pair => {
				// RegisterValidatorsFromAssemblyContaing does this:
				services.Add (ServiceDescriptor.Scoped (pair.InterfaceType, pair.ValidatorType));
				// Also register it as its concrete type as well as the interface type
				services.Add (ServiceDescriptor.Scoped (pair.ValidatorType, pair.ValidatorType));
			});
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure (IApplicationBuilder app, IWebHostEnvironment env) {

			if (env.IsDevelopment ()) {
				app.UseDeveloperExceptionPage ();

			} else {
				app.UseExceptionHandler ("/Error");
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts ();
			}
			app.UseMiddleware<ExceptionMiddleware> ();

			app.UseHttpsRedirection ();

			app.UseStaticFiles ();
			if (!env.IsDevelopment ()) {
				app.UseSpaStaticFiles ();
			}
			app.UseRouting ();
			app.UseAuthentication();
			app.UseAuthorization();
			#region Activar SwaggerUI
			app.UseSwagger ();
			app.UseSwaggerUI (
				options => {
					options.SwaggerEndpoint ("/swagger/v1/swagger.json", "Signus Presupuesto v1");
				}
			);
			#endregion	

			app.UseEndpoints (endpoints => {
				endpoints.MapControllerRoute (
					name: "default",
					pattern: "{controller}/{action=Index}/{id?}");
			});

			app.UseSpa (spa => {
				// To learn more about options for serving an Angular SPA from ASP.NET Core,
				// see https://go.microsoft.com/fwlink/?linkid=864501

				spa.Options.SourcePath = "ClientApp";

				if (env.IsDevelopment())
				{
					//Time limit extended
					spa.UseProxyToSpaDevelopmentServer("http://localhost:4200");
				}
			});
            InicializarDatabase(app, env);
		}
        private static void InicializarDatabase(IApplicationBuilder app, IWebHostEnvironment env)
        {
            using var scope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope();

            scope.ServiceProvider.GetRequiredService<FinanzasContext>().Database.Migrate();
        }
	}
}