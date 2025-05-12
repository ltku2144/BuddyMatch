using BuddyMatch.Model.Repositories;
using Microsoft.OpenApi.Models;
using BuddyMatch.API.Middleware;
using BuddyMatch.API.Services; // Added for IAuthService and AuthService
using BCrypt.Net; // Added for BCrypt self-test

var builder = WebApplication.CreateBuilder(args);

// --- Services Configuration ---
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddScoped<IAuthService, AuthService>(); // REGISTER THE AUTH SERVICE

// --- Temporary Hash Generation ---
// Console.WriteLine("--- Temporary Hash Generation START ---");
// string passwordToHash = "simplepass123";
// string newHashSimplePass = BCrypt.Net.BCrypt.HashPassword(passwordToHash);
// Console.WriteLine($"[HASH GEN] For 'simplepass123': '{newHashSimplePass}'");
// 
// string passwordToHashAnna = "Anna2024";
// string newHashAnna = BCrypt.Net.BCrypt.HashPassword(passwordToHashAnna);
// Console.WriteLine($"[HASH GEN] For 'Anna2024': '{newHashAnna}'");
// Console.WriteLine("--- Temporary Hash Generation END ---\n");
// --- End Temporary Hash Generation ---

//Console.WriteLine($"BCrypt hash for 'Anna2024': {BCrypt.Net.BCrypt.HashPassword("Anna2024")}"); // Temporarily print hash
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
    var connString = config.GetConnectionString("AppProgDb"); // MODIFIED: Changed "DefaultConnection" to "AppProgDb"
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

// --- BEGIN BCrypt Self-Test ---
// Removed self-test code
// --- END BCrypt Self-Test ---

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
app.UseHttpsRedirection(); // Restore HTTPS redirection
app.UseCors("AllowAngularApp");

// ❗ Auth Middleware ORDER is important:
app.UseMiddleware<BasicAuthMiddleware>(); // Custom auth before .UseAuthorization()
app.UseAuthorization();

// --- Controller Routing ---
app.MapControllers();

app.Run();
