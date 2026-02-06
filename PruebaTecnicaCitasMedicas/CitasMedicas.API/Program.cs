using CitasMedicas.API.Extensions;
using CitasMedicas.API.Filters;

var builder = WebApplication.CreateBuilder(args);
builder.Services.ConfigureCors();
builder.Services.AddApplicationServices();
builder.Services.AddValidationServices();
builder.Services.ConfigureServices(builder.Configuration);
builder.Services.AddControllers(options =>
{
    options.Filters.Add<FluentValidationFilter>();
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("CorsPolicy");
app.UseAuthorization();
app.MapControllers();
app.Run();