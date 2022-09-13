using System.Security.Cryptography.X509Certificates;
using GrpcGreeter.Services;
using Microsoft.AspNetCore.Server.Kestrel.Core;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddGrpc();

builder.WebHost.ConfigureKestrel(options => {
    // add certificate
    var cert = new X509Certificate2(@"./grpc-demo.pfx", "demo-grpc");
    options.ConfigureHttpsDefaults(h => {
        h.ClientCertificateMode = Microsoft.AspNetCore.Server.Kestrel.Https.ClientCertificateMode.AllowCertificate;
        h.CheckCertificateRevocation = false;
        h.ServerCertificate = cert;
    });
    // Setup a HTTP/2 endpoint without TLS.
    options.ListenLocalhost(5050, o => o.Protocols = HttpProtocols.Http2);
});



var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<GreeterService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();