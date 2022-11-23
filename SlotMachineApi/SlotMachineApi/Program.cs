using Microsoft.Extensions.Options;
using SlotMachineApi.DbSettings;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<SlotMachineDatabaseSettings>(
    builder.Configuration.GetSection(nameof(SlotMachineDatabaseSettings)));

builder.Services.AddSingleton<ISlotMachineDatabaseSettings>(provider =>
    provider.GetRequiredService<IOptions<SlotMachineDatabaseSettings>>().Value);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
