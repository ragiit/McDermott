var builder = DistributedApplication.CreateBuilder(args);


builder.AddProject<Projects.McDermott_Web>("mcdermott-web");

builder.Build().Run();
