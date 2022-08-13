using Microsoft.EntityFrameworkCore;
using PlatformService.SyncDataServices.Http;
using PlatformService.Data;
using PlatformService.Interfaces;
using PlatformService.Repositories;
using PlatformService.AsyncDataServices;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//configure db
if (builder.Environment.IsProduction())
{
    Console.WriteLine("Use in SqlServer Db");

    Console.WriteLine(builder.Configuration.GetConnectionString("PlatformConnection"));

    builder.Services.AddDbContext<AppDbContext>(opt =>
        opt.UseSqlServer(builder.Configuration.GetConnectionString("PlatformConnection")));
}
else
{
    Console.WriteLine("Use in Memory Db");
    builder.Services.AddDbContext<AppDbContext>(opt =>
        opt.UseInMemoryDatabase("InMemory"));
}


builder.Services.AddScoped<IPlatformRepository, PlatformRepository>();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddHttpClient<ICommandDataClient, HttpCommandDataClient>();

builder.Services.AddSingleton<IMessageBusClient, MessageBusClient>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

PrepareDbContext.PrepareDb(app, app.Environment.IsProduction());

app.UseAuthorization();

app.MapControllers();

app.Run();
