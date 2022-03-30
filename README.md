# Como criar uma Minial Web API com ASP.NET Core e EF 6
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



