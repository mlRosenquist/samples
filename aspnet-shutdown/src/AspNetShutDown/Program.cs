using Serilog;
using Serilog.Formatting.Compact;
using Serilog.Sinks.Grafana.Loki;

var builder = WebApplication.CreateBuilder(args);
builder.Host
    .UseSerilog((ctx, cfg) =>
    {
        
        Console.WriteLine(ctx.Configuration["loki"]!);
        cfg
            .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Error)
            .WriteTo.GrafanaLoki(ctx.Configuration["loki"]!, 
            [
                new LokiLabel { Key = "machine_name", Value = Environment.MachineName }, 
                new LokiLabel { Key = "service_name", Value = ctx.HostingEnvironment.ApplicationName } 
            ]);
    });

var app = builder.Build();

app.UseHttpsRedirection();
app.UseSerilogRequestLogging();
app.MapGet("/fast", async () =>
{
    await Task.Delay(100);
});

app.MapGet("/slow", async () =>
{
    await Task.Delay(2000);
});

app.Run();