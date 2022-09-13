using System.Security.Cryptography.X509Certificates;
using GrpcGreeter.Services;
using Microsoft.AspNetCore.Server.Kestrel.Core;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseKestrel(options => {
    // add certificate
    var cert = new X509Certificate2(@"./grpc-demo.pfx", "demo-grpc");
    options.ConfigureHttpsDefaults(con => {
        con.ClientCertificateMode = Microsoft.AspNetCore.Server.Kestrel.Https.ClientCertificateMode.RequireCertificate;
        con.ClientCertificateValidation = (certificate, chain, errors) =>
            certificate.Issuer == cert.Issuer;
    });
    // Setup a HTTP/2 endpoint without TLS.
    options.ListenLocalhost(5050, o => o.Protocols = HttpProtocols.Http2);
});

builder.Services.AddGrpc(); // must create after Kestrel config

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<GreeterService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();