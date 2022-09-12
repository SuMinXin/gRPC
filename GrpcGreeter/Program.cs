using GrpcGreeter.Services;
using Microsoft.AspNetCore.Authentication.Certificate;
using Microsoft.AspNetCore.Server.Kestrel.Core;

var builder = WebApplication.CreateBuilder(args);

// Additional configuration is required to successfully run gRPC on macOS.
// For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682
builder.WebHost.ConfigureKestrel(options => {
    // Setup a HTTP/2 endpoint without TLS.
    options.ListenLocalhost(5003, o => {
        o.Protocols = HttpProtocols.Http2;
        o.UseHttps(fileName: "grpc-demo.pfx", password: "demo-grpc");
    });
});

// http
//builder.WebHost.ConfigureKestrel(options => {
//    // Setup a HTTP/2 endpoint without TLS.
//    options.ListenLocalhost(5000,
//    o => o.Protocols = HttpProtocols.Http2);
//});

// Add services to the container.
builder.Services.AddGrpc();
builder.Services
    .AddAuthentication(CertificateAuthenticationDefaults.AuthenticationScheme)
    .AddCertificate(options => {
        options.AllowedCertificateTypes = CertificateTypes.SelfSigned;
    });
//AuthenticationOptions
//CertificateTypes
var app = builder.Build();

app.UseAuthentication();

// Configure the HTTP request pipeline.
app.MapGrpcService<GreeterService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();