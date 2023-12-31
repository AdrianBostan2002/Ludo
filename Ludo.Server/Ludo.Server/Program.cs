using Ludo.Business.Options;
using Ludo.Business.Services;
using Ludo.Domain.Entities;
using Ludo.Domain.Interfaces;
using Ludo.Server;
using Ludo.Server.Hubs;

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

builder.Services.AddMediatR();

builder.Services.AddValidator();

builder.Services.AddSingleton(builder.Services);

builder.Services.AddSingleton<ILobbyService, LobbyService>();

builder.Services.AddSingleton<ICellFactory, CellFactory>();

builder.Services.AddSingleton<IPieceService, PieceService>();

builder.Services.AddSingleton<IBoardService, BoardService>();

builder.Services.AddSingleton<IGameService, GameService>();

builder.Services
    .AddOptions<LudoGameOptions>()
    .Bind(builder.Configuration.GetSection(LudoGameOptions.Key))
    .ValidateDataAnnotations()
    .ValidateOnStart();

var app = builder.Build();

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

app.MapHub<LobbyHub>("/lobbyHub");

app.MapHub<GameHub>("/gameHub");

app.Run();
