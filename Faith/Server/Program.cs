using System.Text;
using Faith.Core.Interfaces;
using Faith.Core.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Hosting.StaticWebAssets;
using Faith.Infrastructure.Data;
using Faith.Infrastructure.Data.Repositories;
using Faith.Server.Utilities;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;
var connectionString = config.GetConnectionString("Default");
var jwtSettings = config.GetSection("JWTSettings");

StaticWebAssetsLoader.UseStaticWebAssets(builder.Environment, builder.Configuration);

// Add services to the container.
builder.Services.AddDbContext<FaithDbContext>(options =>
{
    options.UseMySql(builder.Configuration.GetConnectionString("HostedDevDBConnection"), MySqlServerVersion.LatestSupportedServerVersion,
        sqlOptions =>
        {
            sqlOptions.EnableRetryOnFailure(
                maxRetryCount: 10,
                maxRetryDelay: TimeSpan.FromSeconds(30),
                errorNumbersToAdd: null);
        });
    //Only for dev purpose
    options.EnableDetailedErrors();
    options.EnableSensitiveDataLogging();
});

// adding authentication/authorisation instead of auth0, dont see any value to add third party tools but will add it when there is time left
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<FaithDbContext>();


builder.Services.AddAuthentication(opt =>
{
    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,

        ValidIssuer = jwtSettings["validIssuer"],
        ValidAudience = jwtSettings["validAudience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["securityKey"]))
    };
});


//builder.Services.AddSwaggerGen(c =>
//{
//    c.CustomSchemaIds(x => x.FullName);
//    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Api", Version = "v1" });
//
//});

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IStudentRepository, StudentRepository>();

builder.Services.AddScoped<IStudentService, StudentService>();
builder.Services.AddScoped<IMessageService, MessageService>();
builder.Services.AddScoped<IMentorService, MentorService>();





builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();






var app = builder.Build();

//app.UseSwagger();
//app.UseSwaggerUI(c =>
//    c.SwaggerEndpoint("/swagger/v1/swagger.json","Faith API V1")
//    );
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();


app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

//feed db
await DbInitializer.SeedAdminUser(builder.Services.BuildServiceProvider());

app.Run();