using deflix.monolithic.api.Interfaces;
using deflix.monolithic.api.Services;

var corsPolicy = "CorsPolicy";

var apiEnvironment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")?.ToUpper() ?? "PRODUCTION";

var isDevelopment = apiEnvironment is "DEVELOPMENT" or "DEBUG";

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddScoped<IDatabaseService, DatabaseService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserPreferencesService, UserPreferencesService>();
builder.Services.AddScoped<IMovieService, MovieService>();
builder.Services.AddScoped<ISubscriptionsService, SubscriptionsService>();
builder.Services.AddScoped<IRecommendationService, RecommendationService>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddCors(opt =>
{
    opt.AddPolicy(corsPolicy, policy =>
    {
        policy.AllowAnyMethod().AllowAnyHeader().AllowAnyOrigin();
    });
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (isDevelopment)
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Deflix API V1");
    });
}
else
{
    // Production-specific middleware (e.g., exception handler)
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
app.UseCors(corsPolicy);

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
