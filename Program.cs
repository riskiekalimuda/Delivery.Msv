
// Pengaturan ini memaksa .NET dan Npgsql menyelaraskan format DateTime lama/lokal menjadi kompatibel dengan pemformatan database
using Delivery.Msv.Extensions;
using Delivery.Msv.Models;
using Delivery.Msv.Profiles;
using Delivery.Msv.Services;
using Microsoft.EntityFrameworkCore;

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddAutoMapper(x => { }, typeof(MappingProfile));
builder.Services.AddDbContext<DeliveryMsvDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DeliveryMsvDBConnection")));   
builder.Services.AddScoped<DeliveryService>();  
builder.Services.AddCustomMassTransit(builder.Configuration);


var app = builder.Build();
app.UseRouting();
app.MapControllers();

app.Run();

