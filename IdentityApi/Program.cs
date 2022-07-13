using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Sql.Data;
using Sql.Data.Model;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<MirDbContext>(opt =>
{
    opt.UseSqlServer("Server=ChrisServer;Initial Catalog=IdentityDb;Persist Security Info=False;User ID=sa;Password=1234;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=True;Connection Timeout=300;");
});

builder.Services.AddDataProtection();
builder.Services.AddIdentityCore<User>(opt =>
{
    opt.Password.RequireUppercase = false;
    opt.Password.RequireLowercase = false;
    opt.Password.RequireNonAlphanumeric = false;
    opt.Password.RequiredLength = 6; 
    opt.Tokens.PasswordResetTokenProvider = TokenOptions.DefaultEmailProvider;
    opt.Tokens.EmailConfirmationTokenProvider = TokenOptions.DefaultEmailProvider;
});

IdentityBuilder identityBuilder = new IdentityBuilder(typeof(User), typeof(Role), builder.Services);
identityBuilder.AddEntityFrameworkStores<MirDbContext>().
    AddDefaultTokenProviders().
    AddRoleManager<RoleManager<Role>>().
    AddUserManager<UserManager<User>>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
