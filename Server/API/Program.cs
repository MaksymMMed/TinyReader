using API.Exstensions;
using Application.Common.Interfaces;
using Infrastructure.Data;
using Infrastructure.Identity;
using Infrastructure.RabbitMq;
using Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client;
using System.Diagnostics;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddHttpContextAccessor();

//builder.Services.AddDbContext<AppDbContext>(options =>
//    options.UseSqlServer(builder.Configuration.GetConnectionString("DB_CONNECTION")));

builder.Services.AddIdentity<IdentityAppUser, IdentityRole<Guid>>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssemblies(Assembly.Load("Application"))
);


if (builder.Environment.EnvironmentName == "Docker")
{
    builder.Services.AddSingleton(factory => new ConnectionFactory
    {
        HostName = "rabbitmq",
        UserName = "guest",
        Password = "guest",
    });

    builder.Services.AddSingleton<IRabbitAudioService, RabbitAudioService>();
    builder.Services.AddHostedService(sp => (RabbitAudioService)sp.GetRequiredService<IRabbitAudioService>());

    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseSqlServer(Environment.GetEnvironmentVariable("DB_CONNECTION")));
}

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DB_CONNECTION")));
}


builder.Services.AddTokenInSwagger();
builder.Services.AddJwtAuthentication(builder.Configuration);

// CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Services
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IClassroomService, ClassroomService>();

var app = builder.Build();

if (app.Environment.EnvironmentName == "Docker")
{
    using (var scope = app.Services.CreateScope())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var databaseExists = await dbContext.Database.CanConnectAsync();

        if (!databaseExists)
        {
            await dbContext.Database.MigrateAsync();
            app.CreateRoles();
        }
    }
}

app.UseCors();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
