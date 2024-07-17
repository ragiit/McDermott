var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.McHealthCare_Web>("mchealthcare-web");

builder.Build().Run();
