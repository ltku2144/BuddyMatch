using BuddyMatch.Model.Repositories;
using Microsoft.OpenApi.Models;
using BuddyMatch.API.Middleware;

var builder = WebApplication.CreateBuilder(args);

// --- Services Configuration ---
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
//Console.WriteLine(BCrypt.Net.BCrypt.HashPassword("test1234"));


// --- Swagger Docs ---
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "BuddyMatch API",
        Version = "v1"
    });
});

// --- Dependency Injection: UserRepository with Config ---
builder.Services.AddScoped<UserRepository>(provider =>
{
    var config = provider.GetRequiredService<IConfiguration>();
    var connString = config.GetConnectionString("DefaultConnection");
    Console.WriteLine($"📡 Loaded DB connection string: {connString}");
    return new UserRepository(config);
});

// --- Enable CORS for Angular frontend ---
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

// --- Enable Swagger UI in Development ---
if (app.Environment.IsDevelopment())
{
    Console.WriteLine("🛠️  Running in Development mode. Swagger enabled.");
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "BuddyMatch API V1");
    });
}

// --- HTTP Request Pipeline ---
app.UseHttpsRedirection();
app.UseCors("AllowAngularApp");

// ❗ Auth Middleware ORDER is important:
app.UseMiddleware<BasicAuthMiddleware>(); // Custom auth before .UseAuthorization()
app.UseAuthorization();

// --- Controller Routing ---
app.MapControllers();

app.Run();
