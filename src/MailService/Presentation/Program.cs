using EmailManager;
using EmailManager.Consumers;

using MassTransit;

var builder = Host.CreateApplicationBuilder();
builder.Services.AddEmailSender(builder.Configuration);

builder.Services.AddMassTransit(options =>
        {
            options.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(builder.Configuration.GetConnectionString("QueueConnection") ?? 
                    throw new Exception("Queue connection string not configured"));
                cfg.ConfigureEndpoints(context);
            });
                options.AddConsumer<UserVerificationCodeSentConsumer>();
        });

builder.Build().Run();  