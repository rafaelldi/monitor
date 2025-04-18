using MonitorAgent.Counters;
using MonitorAgent.Processes;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc();

builder.Services.AddScoped<ProcessManager>();
builder.Services.AddScoped<CounterManager>();

var app = builder.Build();

app.MapGrpcService<ProcessService>();
app.MapGrpcService<CounterService>();

app.Run();