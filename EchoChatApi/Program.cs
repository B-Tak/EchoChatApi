using EchoChatApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Register services
builder.Services.AddSingleton<AuthService>();

// Message services
builder.Services.AddSingleton<MessageService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();