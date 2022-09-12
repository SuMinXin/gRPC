using System.Net;
using System.Security.Cryptography.X509Certificates;
using Grpc.Net.Client;
using GrpcGreeterClient;

// Assume path to a client .pfx file and password are passed from command line
// On Windows this would probably be a reference to the Certificate Store
// string pfx = System.IO.File.ReadAllText(@"./grpc-demo.pfx");
var cert = new X509Certificate2(@"grpc-demo.pfx", "demo-grpc");
var handler = new HttpClientHandler();
handler.ClientCertificates.Add(cert);
handler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
var httpClient = new HttpClient(handler);

var channel = GrpcChannel.ForAddress("https://localhost:5003/", new GrpcChannelOptions
{
    HttpClient = httpClient
});
var client = new Greeter.GreeterClient(channel);
var reply = await client.SayHelloAsync(
                  new HelloRequest { Name = "GreeterClient" });
Console.WriteLine("Greeting: " + reply.Message);
Console.WriteLine("Press any key to exit...");