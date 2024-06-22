using Libaray_Management_System.Common;
using Libaray_Management_System.Data;
using Libaray_Management_System.Interfaces;
using Libaray_Management_System.Services;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
/*builder.Services.AddEntityFrameworkMySQL()
                .AddDbContext<MemberDBContext>(options =>
                {
                    options.UseMySQL(builder.Configuration.GetConnectionString("DefaultConnection"));
                });*/

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("MySqlDBString");
builder.Services.AddDbContext<MemberDBContext>(options => options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));
builder.Services.AddDbContext<BookDBContext>(options => options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));
builder.Services.AddDbContext<IssueBookDBContext>(options => options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

builder.Services.AddScoped<IMemberService,MemberService>();
builder.Services.AddScoped<IIssueBookService,IssueBookService>();
builder.Services.AddScoped<IBookService,BookService>();
builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

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
