using System.Security.Cryptography.X509Certificates;
using Grpc.Net.Client;
using GrpcGreeterClient;

// without certificate
// var channel = GrpcChannel.ForAddress("https://localhost:5050/");


// Assume path to a client .pfx file 
var cert = new X509Certificate2(@"grpc-demo.pfx", "demo-grpc");
var handler = new HttpClientHandler();
handler.ClientCertificates.Add(cert);
handler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
handler.SslProtocols = System.Security.Authentication.SslProtocols.Tls12;
var httpClient = new HttpClient(handler);

var channel = GrpcChannel.ForAddress("https://localhost:5050/", new GrpcChannelOptions {
    HttpClient = httpClient
});

// set client
var client = new Greeter.GreeterClient(channel);
var reply = await client.SayHelloAsync(
                  new HelloRequest { Name = "GreeterClient" });
Console.WriteLine("Greeting: " + reply.Message);