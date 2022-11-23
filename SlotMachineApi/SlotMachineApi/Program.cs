using Microsoft.Extensions.Options;
using SlotMachineApi.DbSettings;
using SlotMachineApi.Services;
using SlotMachineApi.Services.Impl;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<SlotMachineDatabaseSettings>(
    builder.Configuration.GetSection(nameof(SlotMachineDatabaseSettings)));

builder.Services.AddSingleton<ISlotMachineDatabaseSettings>(provider =>
    provider.GetRequiredService<IOptions<SlotMachineDatabaseSettings>>().Value);

builder.Services.AddScoped<IPlayerService, PlayerService>();
builder.Services.AddScoped<IMachineService, MachineService>();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
