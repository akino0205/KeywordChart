using Microsoft.EntityFrameworkCore;
using RelatedKeywordLibrary.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

//데이터베이스 공급자 : https://docs.microsoft.com/ko-kr/ef/core/providers/?tabs=dotnet-core-cli
//UseMySQL by nuget "MySql.EntityFrameworkCore"
builder.Services.AddDbContext<UserContext>(options =>
       options.UseMySQL(builder.Configuration.GetConnectionString("UserContext")));

//builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    //app.UseSwagger();
    //app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
