using ApiAggregator.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();


// Register your services with Dependency Injection
builder.Services.AddHttpClient<IWeatherService, WeatherService>();
builder.Services.AddHttpClient<IMediastackNewsService, MediastackNewsService>();
builder.Services.AddHttpClient<ICryptoService, CryptoService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();  // Optional, in case you have authorization in your app

app.MapControllers();  // This is critical to map controller routes

app.Run();