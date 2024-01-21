using Microsoft.EntityFrameworkCore;
using TestTask.Data;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<TestTaskDBContext>(options =>
options.UseNpgsql(builder.Configuration.GetConnectionString("ValueContext") ?? throw new InvalidOperationException("Connection string 'ValueContext' not found.")));
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
