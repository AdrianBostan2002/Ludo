using Ludo.Domain.Interfaces;
using Ludo.Domain.Entities;
using Ludo.Server;
using Ludo.Business.UseCases.TestUseCase;
using Microsoft.OpenApi.Validations;
using Ludo.Business;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "CorsPolicy",
        policy =>
        {
            policy.WithOrigins("http://localhost:4200")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
        });
});

builder.Services.AddSignalR();

builder.Services.AddSingleton<IMediator, MediatoR>();

builder.Services.AddSingleton(builder.Services);

//var requestHandlerTypes = RegisterHandlers.RegisterHandlersFromAssembly(builder.Services);

// Set up the MediatoR with the requestHandlerTypes
//var mediator = builder.Services.BuildServiceProvider().GetRequiredService<IMediator>();

//// Assign the requestHandlerTypes to the MediatoR
//mediator.SetRequestHandlerTypes(requestHandlerTypes);

var requestHandlerTypes = RegisterHandlers.RegisterHandlersFromAssembly(builder.Services);


//builder.Services.AddSingleton<IRequestHandler<TestRequest, string>, TestHandler>();

// Retrieve the IMediator instance from the service provider

// Set up the MediatoR with the requestHandlerTypes
//mediator.SetRequestHandlerTypes(requestHandlerTypes);

var app = builder.Build();
//var app = builder.Build();

var serviceProvider = builder.Services.BuildServiceProvider();
var mediator = serviceProvider.GetRequiredService<IRequestHandler<TestRequest, string>>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("CorsPolicy");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapHub<TestHub>("/testHub");

app.Run();
