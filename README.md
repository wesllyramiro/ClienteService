## Pré-requisitos

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Docker](https://www.docker.com/products/docker-desktop) (para rodar o SQL Server localmente)
- [VS Code](https://code.visualstudio.com/) (opcional, mas recomendado)

## Como executar o projeto localmente

1. **Suba o banco de dados SQL Server com Docker:**

   ```sh
   docker-compose up -d db
   ```

2. **(Opcional) Execute o script de seed para criar tabelas e dados iniciais:**

   ```sh
   docker-compose run --rm db-init
   ```

3. **Rode a aplicação Web API:**

   ```sh
   dotnet run --project ClienteService/ClienteService.csproj
   ```

4. **Acesse o Swagger:**

   - Abra o navegador e acesse: [http://localhost:5000/swagger](http://localhost:5000/swagger)  
     ou  
     [https://localhost:5001/swagger](https://localhost:5001/swagger)

   O Swagger UI permite testar todos os endpoints da API de forma interativa.
   
## Estrutura de Pastas (MVC)

Estrutura simplificada do repositório e do projeto Web API seguindo MVC (Controllers + Services + Repositories + Models):

```
.
├─ docker-compose.yml
├─ README.md
├─ docs/
│  └─ diagrams/
│     ├─ c4-context.puml
│     ├─ c4-container.puml
│     └─ uml-class.puml
├─ ClienteService/                 # Projeto Web API (.NET 8)
│  ├─ Controllers/                 # Controladores REST (ex.: ClientesController, HealthController)
│  ├─ Models/                      # Modelos de domínio/DTOs (ex.: Cliente)
│  ├─ Services/                    # Regras de negócio/orquestração (IClienteService, ClienteServiceImpl)
│  ├─ Repositories/                # Acesso a dados via Dapper (IClienteRepository, ClienteRepository)
│  ├─ scripts/                     # Scripts SQL (criação/semente)
│  │  └─ create_tables_and_seed.sql
│  ├─ appsettings.json             # ConnString/dev configs
│  ├─ appsettings.Docker.json      # ConnString para Docker (SQL Server container)
│  ├─ Program.cs                   # Bootstrap, DI, Swagger, init DB
│  └─ ClienteService.csproj
└─ ClienteService.Tests/           # Testes (xUnit + Moq)
	 └─ Controllers/
			└─ ClientesControllerTests.cs
```

## Explicação da Estrutura e dos Elementos

- Controllers
	- Camada de entrada HTTP (MVC). Recebe requisições, valida entrada básica e delega ao Service.
	- `ClientesController` expõe CRUD: GET/POST/PUT/DELETE. `HealthController` faz verificação de saúde/DB.

- Services
	- Regras de negócio e orquestração de casos de uso. Não conhece detalhes de persistência.
	- `IClienteService` define o contrato; `ClienteServiceImpl` implementa chamando o repositório.

- Repositories
	- Isolam o acesso a dados usando Dapper sobre `SqlConnection` (ADO.NET).
	- `IClienteRepository` define operações (GetAll, GetById, FindByName, Count, Create, Update, Delete).
	- `ClienteRepository` implementa SQL parametrizado e métodos assíncronos (QueryAsync/ExecuteAsync).

- Models
	- Objetos de domínio/DTO transportados entre camadas e no payload JSON da API (ex.: `Cliente`).

- Program.cs (Bootstrap)
	- Configura DI (Dependency Injection), Swagger, Controllers e a fábrica de `IDbConnection`.
	- Registra mapeamentos: `IClienteRepository -> ClienteRepository` e `IClienteService -> ClienteServiceImpl`.
	- Opcionalmente executa `scripts/create_tables_and_seed.sql` no start para garantir o schema.

- appsettings.json / appsettings.Docker.json
	- Connection strings e demais configurações por ambiente. Para Docker, usar `Server=localhost,1433` (ou o host do container) e `TrustServerCertificate=True` em dev.

- scripts/
	- SQL de criação da tabela `Clientes` e inserção de dados iniciais (seed) para desenvolvimento/local.

- docs/diagrams/
	- Diagramas de Arquitetura (C4: Context, Container, Component) e UML (classes), em PlantUML prontos para o Draw.io.

- docker-compose.yml
	- Sobe o SQL Server (imagem oficial) e um job de init para aplicar o script SQL.

- Testes (ClienteService.Tests)
	- Testes unitários de controller com xUnit + Moq. É possível evoluir para testes de integração com SQL Docker.

## Fluxo de Requisição (alto nível)

1) Cliente chama `HTTP` (JSON) um endpoint do `ClientesController`.
2) Controller delega para `IClienteService` (regra de negócio/validação adicional).
3) Service chama `IClienteRepository` para acessar o banco via Dapper.
4) Repositório executa SQL no `SQL Server` e retorna os dados ao Service.
5) Service retorna DTO/resultado ao Controller, que responde ao cliente.

Benefícios
- Separação de responsabilidades (Controller vs Service vs Repository).
- Testabilidade: Services e Controllers podem ser testados com mocks.
- Simplicidade e performance com Dapper para acesso a dados.\