using GameServices;
using Microsoft.EntityFrameworkCore;
using SharedModels;using GameServices.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<IDungeonStore, InMemoryDungeonStore>();


builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.Authority = "http://localhost:8180/realms/blazorGame";
        
        options.RequireHttpsMetadata = false;

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false,
        };
    });

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase("GameDB"));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddAuthorization();


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBlazorClient", policy =>
    {
        policy
            .WithOrigins("https://localhost:5062", "http://localhost:5062")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    db.Database.EnsureDeleted();
    db.Database.EnsureCreated();

    if (!db.Ennemies.Any())
    {
        db.Ennemies.AddRange(
            new Ennemie(0, "Gobelin", 30, 5),
            new Ennemie(0, "Orc", 50, 10),
            new Ennemie(0, "Dragon", 100, 20)
        );
    }

    if (!db.Players.Any())
    {
        db.Players.Add(new Player(0, "Hero", 100, 20, 0));
        db.Players.Add(new Player(0, "Player 1", 100, 20, 0));
    }

    if (!db.Users.Any())
    {
        db.Users.Add(new User(0, "admin", "admin", true));
    }

    db.SaveChanges();

    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine("✅ [GameServices] Base InMemory initialisée avec succès !");
    Console.ResetColor();
    Console.WriteLine($"📦 Ennemis : {db.Ennemies.Count()} | Joueurs : {db.Players.Count()} | Utilisateurs : {db.Users.Count()}");
}
app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseCors("AllowBlazorClient");
app.UseAuthentication(); 
app.UseAuthorization();
app.MapControllers();
app.Run("http://0.0.0.0:5001");
