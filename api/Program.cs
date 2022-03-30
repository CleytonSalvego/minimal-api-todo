
using Microsoft.EntityFrameworkCore;
using minimal_api_todo.Data;
using minimal_api_todo.Models;
using MiniValidation;

#region "::: CONFIGURAÇÃO :::"

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

#endregion

#region "::: ENDPOINTS :::"

#region "::: GET :::"

app.MapGet("/atividades", async (
    DataBaseContext context) => 
    await context.Atividades.ToListAsync())
    .WithName("GetAtividades")
    .WithTags("Atividades");

#endregion

#region "::: GET BY ID :::"

app.MapGet("/atividades/{id}", async (
    Guid  id,
    DataBaseContext context) =>
    await context.Atividades.AsNoTracking<AtividadeModel>()
                            .FirstOrDefaultAsync(x=>x.Id == id)
    
    is AtividadeModel atividade 
        ? Results.Ok(atividade) 
        : Results.NotFound())

    .Produces(StatusCodes.Status200OK)
    .Produces(StatusCodes.Status404NotFound)
    .WithName("GetAtividadesById")
    .WithTags("Atividades");

#endregion

#region "::: POST :::"

app.MapPost("/atividades", async (
    DataBaseContext context,
    AtividadeModel atividade) =>
    {

    if (!MiniValidator.TryValidate(atividade, out var errors))
        return Results.ValidationProblem(errors);

        context.Atividades.Add(atividade);
        var result = await context.SaveChangesAsync();

        return result > 0 
            ? Results.Created($"/atividades/{atividade.Id}", atividade) 
            : Results.BadRequest("Houve um problema a tentar salvar o registro.");
    })
    .ProducesValidationProblem()
    .Produces<AtividadeModel>(StatusCodes.Status201Created)
    .Produces(StatusCodes.Status400BadRequest)
    .WithName("PostAtividades")
    .WithTags("Atividades");

#endregion

#region "::: PUT :::"

app.MapPut("/atividades/{id}", async (
    Guid id,
    DataBaseContext context,
    AtividadeModel atividade) =>
    {
    var _atividade = await context.Atividades.AsNoTracking<AtividadeModel>()
                                             .FirstOrDefaultAsync(x => x.Id == id);

        if (_atividade == null) return Results.NotFound();

        if (!MiniValidator.TryValidate(atividade, out var errors))
            return Results.ValidationProblem(errors);

        context.Atividades.Update(atividade);
        var result = await context.SaveChangesAsync();

        return result > 0
            ? Results.NoContent()
            : Results.BadRequest("Houve um problema a tentar atualizar o registro.");

    })
    .ProducesValidationProblem()
    .Produces(StatusCodes.Status204NoContent)
    .Produces(StatusCodes.Status400BadRequest)
    .WithName("PutAtividades")
    .WithTags("Atividades");

#endregion

#region "::: DELETE :::"

app.MapDelete("/atividades/{id}", async (
    Guid id,
    DataBaseContext context) =>
{
    var atividade = await context.Atividades.FindAsync(id);
    if (atividade == null) return Results.NotFound();

    context.Atividades.Remove(atividade);
    var result = await context.SaveChangesAsync();

    return result > 0
        ? Results.NoContent()
        : Results.BadRequest("Houve um problema a tentar remover o registro.");

})
    .ProducesValidationProblem()
    .Produces(StatusCodes.Status204NoContent)
    .Produces(StatusCodes.Status400BadRequest)
    .Produces(StatusCodes.Status204NoContent)
    .WithName("DeleteAtividades")
    .WithTags("Atividades");

#endregion

#endregion

app.Run();