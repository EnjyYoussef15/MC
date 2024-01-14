using MCSHiPPERS_Task.Models;
using MCSHiPPERS_Task.Repository.IProduct;
using MCSHiPPERS_Task.Repository.IRepository;
using MCSHiPPERS_Task.Repository.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<DataContext>(options =>options.UseSqlServer(connectionString));

//--same as above
//builder.Services.AddDbContext<DataContext>(options =>
//options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));



builder.Services.AddAuthentication();
//builder.Services.ConfigureIdentity();

builder.Services.AddTransient(typeof(IProductRepository<>), typeof(ProductRepository<>));
builder.Services.AddIdentity<User, IdentityRole>().AddEntityFrameworkStores<DataContext>().AddDefaultTokenProviders(); 

builder.Services.AddScoped<IAccountRepository, AccountRepository>();

builder.Services.AddScoped<IEmailService, EmailService>();

builder.Services.Configure<IdentityOptions>(options => options.SignIn.RequireConfirmedEmail=true);

//life time of link to beexpired

builder.Services.Configure<DataProtectionTokenProviderOptions>(opts => opts.TokenLifespan = TimeSpan.FromHours(10));


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
               .AddJwtBearer(options =>
               {
                   options.TokenValidationParameters = new TokenValidationParameters
                   {

                       ValidateLifetime = true,
                       ValidateIssuer = false,
                       ValidateAudience = false,
                       ValidateIssuerSigningKey = true,
                       ClockSkew = TimeSpan.Zero,
                       IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes
                   (builder.Configuration["secrestkey"]))
                   };

               });

//builder.Services.ConfigurationJWT(builder.Configuration);

builder.Services.AddCors(corsOptions =>
{
    corsOptions.AddPolicy("allowAllPolicy", corsBuilder =>
    {
        corsBuilder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});


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
