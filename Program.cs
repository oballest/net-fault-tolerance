using Polly;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var timeoutPolicy = Policy.TimeoutAsync<HttpResponseMessage>(10);

builder.Services.AddHttpClient("retry", client => 
    {
        client.BaseAddress = new Uri("http://localhost:8090");
        client.DefaultRequestHeaders.Add("Accept","text/plain");
        client.Timeout = TimeSpan.FromSeconds(20);
    }
).AddTransientHttpErrorPolicy( policyBuilder => policyBuilder.WaitAndRetryAsync(new[]
{
    TimeSpan.FromSeconds(1),
    TimeSpan.FromSeconds(5),
    TimeSpan.FromSeconds(10)
}));

builder.Services.AddHttpClient("timeOut", client => 
    {
        client.BaseAddress = new Uri("http://localhost:8090");
        client.DefaultRequestHeaders.Add("Accept","text/plain");
        client.Timeout = TimeSpan.FromSeconds(20);
    }
).AddPolicyHandler(timeoutPolicy);

builder.Services.AddHttpClient("circuitBreaker", client => 
    {
        client.BaseAddress = new Uri("http://localhost:8090");
        client.DefaultRequestHeaders.Add("Accept","text/plain");
        client.Timeout = TimeSpan.FromSeconds(20);
    }
).AddTransientHttpErrorPolicy(policyBuilder => policyBuilder.CircuitBreakerAsync(
       handledEventsAllowedBeforeBreaking: 3,
       durationOfBreak: TimeSpan.FromSeconds(20)
));


var app = builder.Build();

// Configure the HTTP request pipeline.
/*if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}*/

app.UseSwagger();
app.UseSwaggerUI();
//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
