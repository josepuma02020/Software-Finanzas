using System;
using System.Reflection;
using Domain.Contracts;
using FluentValidation;
using Infraestructure.Base;
using Infraestructure.Context;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WebApi.Infrastructure;
WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<FinanzasContext>((_, optionsBuilder) => optionsBuilder
    .UseSqlServer(builder.Configuration["ConnectionString"]));

builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidatorPipelineBehavior<,>));
AssemblyScanner.FindValidatorsInAssembly(Assembly.Load("Application")).ForEach(pair =>
{
    // RegisterValidatorsFromAssemblyContaing does this:
    builder.Services.Add(ServiceDescriptor.Scoped(pair.InterfaceType, pair.ValidatorType));
    // Also register it as its concrete type as well as the interface type
    builder.Services.Add(ServiceDescriptor.Scoped(pair.ValidatorType, pair.ValidatorType));
});

AssemblyScanner.FindValidatorsInAssembly(Assembly.Load("SoftwareFinanzas")).ForEach(pair =>
{
    // RegisterValidatorsFromAssemblyContaing does this:
    builder.Services.Add(ServiceDescriptor.Scoped(pair.InterfaceType, pair.ValidatorType));
    // Also register it as its concrete type as well as the interface type
    builder.Services.Add(ServiceDescriptor.Scoped(pair.ValidatorType, pair.ValidatorType));
});
builder.Services.AddScoped<IDbContext, FinanzasContext>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddMediatR(Assembly.Load("Application"));
builder.Services.AddMediatR(Assembly.Load("SoftwareFinanzas"));
// In production, the Angular files will be served from this directory
builder.Services.AddSpaStaticFiles(configuration =>
{
    configuration.RootPath = "ClientApp/dist";
});

#region SwaggerOpen Api
//Register the Swagger services
builder.Services.AddSwaggerGen( );
			#endregion

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseMiddleware<ExceptionMiddleware>();

app.UseHttpsRedirection();

app.UseStaticFiles();
if (!app.Environment.IsDevelopment())
{
    app.UseSpaStaticFiles();
}

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
			#region Activar SwaggerUI
app.UseSwagger(Options =>
{
    Options.SerializeAsV2 = true;
});
app.UseSwaggerUI(
    options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Signus Presupuesto v1");
    }
);
			#endregion

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller}/{action=Index}/{id?}");
});

app.UseSpa(spa =>
{
    // To learn more about options for serving an Angular SPA from ASP.NET Core,
    // see https://go.microsoft.com/fwlink/?linkid=864501

    spa.Options.SourcePath = "ClientApp";

    if (builder.Environment.IsDevelopment())
    {
        //Time limit extended
        spa.UseProxyToSpaDevelopmentServer("http://localhost:4200");
    }
});

var scope = app.Services.CreateScope();

var context = scope.ServiceProvider.GetRequiredService<FinanzasContext>();
context.Database.Migrate();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");


app.MapFallbackToFile("index.html");


app.Run();
