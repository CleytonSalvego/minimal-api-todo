
using Microsoft.EntityFrameworkCore;
using minimal_api_todo.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<DataBaseContext>(options => options.UseInMemoryDatabase("BancoDados"));    

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/atividades", async (
    DataBaseContext context) => 
    await context.Atividades.ToListAsync())
    .WithName("GetAtividades")
    .WithTags("Atividades");

app.Run();