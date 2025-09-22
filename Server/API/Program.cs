using API.Exstensions;
using Application.Common.Interfaces;
using Infrastructure.Data;
using Infrastructure.Identity;
using Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddHttpContextAccessor();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DB_CONNECTION")));

builder.Services.AddIdentity<IdentityAppUser, IdentityRole<Guid>>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssemblies(Assembly.Load("Application"))
);

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DB_CONNECTION")));

    builder.Services.AddJwtAuthentication(builder.Configuration);
    builder.Services.AddTokenInSwagger();
    builder.Services.AddCors(options =>
    {
        options.AddDefaultPolicy(policy=>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
    });
}

// Services
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IClassroomService, ClassroomService>();

var app = builder.Build();

app.UseCors();
app.CreateRoles();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
