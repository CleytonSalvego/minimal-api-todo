# Como criar uma Minimal Web API com ASP.NET 6 Core e EF 6 e autorização por Token com JWT Bearer
Diferente de uma API Rest usual, a minial API é enxuta, direta com o mínimo de configurações possíveis e não utiliza Controllers.
<br>
E por não utilizar Controllers devemos nos ter atenção com algumas adaptações necessárias para usar autenticação e o Swagger e realizar o mapeamento puro dos endpoints.
<br>
## Como criar um novo projeto no modelo Minial API.
<br>
Abra o visual studio 2022 e selecione para criar um novo projeto, em seguida selecione a opção API Web do ASP.NET Core com C# e clique no botão próximo.

![image](https://user-images.githubusercontent.com/48839351/160740584-ba8ef634-3090-4049-be54-f7b1d45e728a.png)

Informe o nome e o local onde ficará seu projeto.

![image](https://user-images.githubusercontent.com/48839351/160740638-e70363df-b151-45bd-b8da-e73730a6c32b.png)

### Nesta próxima etapa, se atente as informações abaixo.<br>
Framework: .NET 6 <br>
Tipo de Autenticação: None (Faremos a autenticação manualmente) <br>
Configurar para HTTPS: SIM <br>
Habilitar Docker: Opcional <br>
Usar Controladores (desmarque para usar APIs mínimas): Aqui está o segredo, se ficar selecionado irá criar uma API normal com Controllers, como queremos uma minimal API devemos não marca-la. <br>
Habilitar o suporte a OpenAPI (Swagger): SIM. <br>

![image](https://user-images.githubusercontent.com/48839351/160740735-b0c4899e-abf4-4cc3-8ea1-7318c8a9ee9b.png)

Feito todas as configurações clique no botão criar e o Visual Studio irá criar o nosso projeto.  <br>
Importante: Com o ASP.NET 6 a classe Startup deixou de ser usada e foi integrada diretamente na classe Program.cs. <br>
### Propreties
Nesta pasta na estrutura do programa fica o arquivo launchSettings.json onde estão as informações da url e porta que será carrega a nossa API.  <br>
Por padrão o url do locahost e as portas são  <br>
https://localhost:7143  <br>
http://localhost:5143  <br>

### appsettings.json
Esse arquivo é onde iremos colocar nossas secret Keys, para serem utilizadas em nossa API. <br>

### Classe Program.cs
É onde tudo acontece quando falamos de minimal API.  <br>
```
var builder = WebApplication.CreateBuilder(args); 
```
CreateBuilder -> A classe Program.cs inicia com a criação de uma variável builder, e é nela onde o container da aplicação é criado e os serviços e configurações serão adicionados, é a base da nossa aplicação web API.  <br>
```
builder.Services.AddEndpointsApiExplorer();  <br>
```
Como não utilizamos controllers em APIs mínimas, tudo é feito manualmente.  <br>
Assim esse comando cria um metadados para a criação dos endpoints.  <br>
```
builder.Services.AddSwaggerGen();  <br>
```
O Swagger irá utilizar os metadados criados para os endpoints acima para a geração da documentação dos endpoints da nossa API.  <br>
```
var app = builder.Build();  <br>
```
Esse comando será sempre realizado por último, após as configurações dos nossos serviços.  <br>
Com esse comando será gerado o build da nossa aplicação web. Esse app nada mais é que a construção da no API com todos os serviços configurados e adicionados anteriormente.  <br>
Os próximos comando definirão todo o fluxo das requisições, como as requisições irão trabalhar.  <br>

```
if (app.Environment.IsDevelopment())
  {
     app.UseSwagger();
     app.UseSwaggerUI();
  }
```

Nesse comando ele verifica se estamos em ambiente de desenvolvimento e adiciona o Swagger.  <br>
```
app.UseHttpsRedirection();  <br>
```
Neste comando informa que as requisições serão sempre redirecionadas para HTTPS.  <br>
```
app.MapGet("/endpoint"...  <br>
```
Como não existem controllers, usamos a sintaxe app.Map+Verbo(GET,POST,PUT ou DELETE) e o nome do nosso endpoint onde as requisições deverão ser direcionadas.  <br>
E em seguida colocamos a ação que nosso endpoint irá realizar.  <br>
```
.WithName("GetPelidoEndpoint");  <br>
```
Podemos através do WithName setar apelidos para nossos endpoints, ou seja, podemos acessar pelo endpoint original ou pelo apelido declarado no WithName.  <br>

```app.Run  <br>
```
Por fim temos app.Run que realização a execução da API, deixando-a funcionando para podermos acessá-la.  <br>
### Executando a aplicação  <br>
Ao executar a aplicação no Visual Studio será aberto no navegador algo próximo a isso.  <br>
Sendo acessado pelo endpoint: https://localhost:7143/swagger/index.html   <br>
Esse endpoint é criado automaticamente pelo Visual Studio, onde através de uma requisição GET você obtém a resposta como nas imagens demonstradas abaixo.  <br>

![image](https://user-images.githubusercontent.com/48839351/160749095-97751ddd-08ca-43d9-b3f6-1dea577941f6.png)
![image](https://user-images.githubusercontent.com/48839351/160749099-0a5d1c39-db1a-43b9-a9de-a47b4308c779.png)


### Personalizando nossa API
Para podermos criar nossos próprios endpoints, precisamos primeiramente remover o endpoint criado pelo Visual Studio, devendo nossa classe Program.cs ficar semelhante a imagem abaixo.  <br>

![image](https://user-images.githubusercontent.com/48839351/160749125-f4c012da-5345-4cf3-ac37-5d024c82da98.png)

### Criando o modelo de entidade de nossa Atividade
Primeiramente vamos criar uma pasta chamada Model, onde iremos colocar nossos modelos e dentro dessa pasta iremos criar uma nova classe chamada AtividadeModel.cs com o código abaixo.<br>
```
namespace minimal_api_todo.Models
{
    public class AtividadeModel
    {
        public Guid Id { get; set; }
        public string Titulo { get; set; }
        public string Descricao { get; set; }
        public int Status { get; set; }
        public bool Ativo{ get; set; }

    }
}
```
### Criando nosso mapeamento com o banco de dados
Primeiramente vamos criar uma pasta chamada Data, onde iremos colocar nossos dataContext  e dentro dessa pasta iremos criar uma nova classe chamada DataBaseContext.cs com o código abaixo.
```
using Microsoft.EntityFrameworkCore;
using minimal_api_todo.Models;

namespace minimal_api_todo.Data
{
    public class DataBaseContext : DbContext
    {
        public DataBaseContext(DbContextOptions<DataBaseContext> options) : base(options) { }

        public DbSet<AtividadeModel> Atividades { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AtividadeModel>()
                .ToTable("tb_cad_atividade");

            modelBuilder.Entity<AtividadeModel>()
                .HasKey(p => p.Id);

            modelBuilder.Entity<AtividadeModel>()
                        .Property(p => p.Titulo)
                        .IsRequired()
                        .HasColumnType("varchar(100)");

            modelBuilder.Entity<AtividadeModel>()
                        .Property(p => p.Descricao)
                        .IsRequired()
                        .HasColumnType("varchar(250)");

            modelBuilder.Entity<AtividadeModel>()
                        .Property(p => p.Status)
                        .IsRequired()
                        .HasColumnType("integer");



            base.OnModelCreating(modelBuilder);
        }
    }
}
```

Iremos trabalhar com EntityFramework.Core e EntityFramework.InMemory para neste momento criarmos um banco de dados em memória e conseguir realizar nossos testes sem a necessidade de ter um banco de dados SQL Server.
Para isso teremos que instalar esses dois pacotes, você realizar a intalação por package install, Nuget ou no arquivo .csproj inserir as informações abaixo dentro da tag 
```
<ItemGroup>
    <PackageReference Include="Microsoft.EntityFrameworkCore" Version="6.0.3" />
    <PackageReference Include="Microsoft.EntityFrameworkCore.InMemory" Version="6.0.3" />
    <PackageReference Include="Microsoft.EntityFrameworkCore.Relational.Design" Version="1.1.6" />
	  <PackageReference Include="Microsoft.EntityFrameworkCore.Tools" Version="6.0.3">
  		<PrivateAssets>all</PrivateAssets>
	  	<IncludeAssets>runtime; build; native; contentfiles; analyzers; buildtransitive</IncludeAssets>
	  </PackageReference>	
```

Em seguida rode o comando dotnet retore para que seja realizada a restauração e instalação de todos as dependências dos pacotes.
Com nosso DataBaseContext criado e todas as dependências do EF Core instaladas, devemos colocar nossa string de conexão entre a API e nosso banco de dados em memória.
Para isso vá até a classe Program.cs e logo abaixo do comando ### builder.Services.AddSwaggerGen(); 
insira o código abaixo.
```
builder.Services.AddDbContext<DataBaseContext>(options => options.UseInMemoryDatabase("BancoDados"));   
```
Esse commando irá adicionar o serviço de DbContext e criar uma banco em memória chamado BancoDados.


### Imagem do código da classe Program.cs

![image](https://user-images.githubusercontent.com/48839351/160749670-e94c7072-1720-4591-a82b-d114ee0ab2c7.png)

### Imagem da classe DataBaseContext.cs

![image](https://user-images.githubusercontent.com/48839351/160749691-c3a104c1-e98b-4364-9b5b-e0967dc5be40.png)

### Criando um endpoint para carregas as atividades
Como não existem controllers, os endpoints serão criados diretamente em nossa classe Program.cs
Iremos criar um endpoint chamado “atividades” para listar todas as atividades de nossa tabela chamada tb_cad_atividade.
Para isso utilizamos o código abaixo antes do comando app.Run().
```
app.MapGet("/atividades", async (
    DataBaseContext context) => 
    await context.Atividades.ToListAsync())
    .WithName("GetAtividades")
    .WithTags("Atividades");

```
### Explicando o código
app.MapGet("/atividades", async (): Cria um endpoint chamado atividades para uma requisição assincrona do tipo GET .
DataBaseContext context) =>: Adiciona nosso contexto para ações no banco de dados.

await context.Atividades.ToListAsync()): Realiza a listagem de forma assíncrona de todos os dados da tabela tb_cad_atividade.
.WithName("GetAtividades"): Insere um apelido ao nosso endpoint.
.WithTags("Atividades"): Insere uma tag para podermos agruparmos nossos endpoints em nossoa documentação.

### Imagem do código da classe Program.cs após a criação do nosso endpoint

![image](https://user-images.githubusercontent.com/48839351/160749930-9a8cf827-3fd7-48e3-8dd0-f3c948caabbe.png)

 
Agora ao rodar a aplicação, devemos ter um endpoint chamado atividades e devemos conseguir realizar uma requisição GET para listar os dados, não irá carregar nada porque não temos dados inserido ainda, mas a requisição deve retornar um http status 200 – Sucess.
 
 ![image](https://user-images.githubusercontent.com/48839351/160749958-8751ad08-ada1-418e-a046-6f86ac1a405b.png)
![image](https://user-images.githubusercontent.com/48839351/160749966-3a0f8542-a3be-4442-9b5b-2131b074d296.png)


### Criando um endpoint para carregas as atividades através do Id.
Para carregar uma atividades através do Id, devemos passar como parâmetro através do endpoint, buscar essa informação no banco e verificar se existe ou não. Nosso código ficará.
```
app.MapGet("/atividades/{id}", async (
    Guid  id,
    DataBaseContext context) =>
    await context.Atividades.FindAsync(id)
    is AtividadeModel atividade ? Results.Ok(atividade) : Results.NotFound())
    .Produces(StatusCodes.Status200OK)
    .Produces(StatusCodes.Status404NotFound)
    .WithName("GetAtividadesById")
    .WithTags("Atividades");

```


### Criando um endpoint para inserir uma determinada atividade
Para criar nosso endpoint com requisição POST, precisaremos validar se o Json recebido pelo body é valido e para isso precisaremos utilizar um pacote chamado MiniValitaion que pode ser encontrado no github do DamianEdwards pelo link https://github.com/DamianEdwards/MiniValidation 
Para sua instalação basta utilizar o comando abaixo.
dotnet add package MinimalApis.Extensions  --prerelease

Em seguida nosso endpoint para o POST ficará da seguinte forma.
```
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
```
Para a validação do JSON recebido estamos utilizando o seguinte trecho do código.
```
    if (!MiniValidator.TryValidate(atividade, out var errors))
        return Results.ValidationProblem(errors);
```


### Criando um endpoint para atualizar uma determinada atividade
O código para nosso PUT ficou da seguinte forma.
```
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
```


### Criando um endpoint para remover uma determinada atividade
E por último, nosso endpoint DELETE ficou desse jeito.
```
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

```
 
Por fim, com todos os endpoints criados podemos analisar a versão final de nossa documentação.

![image](https://user-images.githubusercontent.com/48839351/160750043-bb06cbff-7117-4ef2-b408-1bb777581f96.png)

 















