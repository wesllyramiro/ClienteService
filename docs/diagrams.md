
@startuml C4_Context_ClienteService
' Estilo C4 mínimo usando formas PlantUML para compatibilidade com Draw.io
skinparam backgroundColor #ffffff
skinparam shadowing false
skinparam linetype ortho
skinparam defaultTextAlignment center

title C4 - Contexto do Sistema: ClienteService

rectangle "Usuário\n(Pessoa)" as Person_User #f5f5f5
rectangle "API ClienteService\n(Sistema de Software)" as System_API #dae8fc
rectangle "SQL Server\n(Sistema Externo)" as System_DB #ffe6cc

Person_User -down-> System_API : HTTP/JSON
System_API -down-> System_DB : ADO.NET + Dapper

note right of System_API
Expõe endpoints REST para gerenciar Clientes
- CRUD de Clientes
- Health check (/health)
- Swagger / OpenAPI
end note

@enduml


@startuml C4_Container_ClienteService
skinparam backgroundColor #ffffff
skinparam shadowing false
skinparam linetype ortho
skinparam defaultTextAlignment center

title C4 - Contêineres: ClienteService

package "Sistema ClienteService" {
  rectangle "API ClienteService\n(.NET 8 Web API)" as Api #dae8fc
  rectangle "SQL Server\n(Banco de Dados)" as Db #ffe6cc
}

rectangle "Cliente (Navegador / Ferramenta)" as Client #f5f5f5

Client -down-> Api : HTTP/JSON
Api -down-> Db : SqlConnection + Dapper

note right of Api
- Controllers (Clientes, Health)
- Services (ClienteService)
- Repositories (ClienteRepository)
- Models (Cliente)
- Swagger / OpenAPI
end note

@enduml

@startuml UML_Class_ClienteService
skinparam backgroundColor #ffffff
skinparam shadowing false
skinparam classAttributeIconSize 0

title UML - Classes Principais: ClienteService

class Cliente {
  +int Id
  +string Nome
  +string Email
  +string Telefone
}

interface IClienteService {
  +Task<IEnumerable<Cliente>> GetAllAsync()
  +Task<Cliente?> GetByIdAsync(int id)
  +Task<IEnumerable<Cliente>> FindByNameAsync(string nome)
  +Task<int> CountAsync()
  +Task<Cliente> CreateAsync(Cliente cliente)
  +Task<bool> UpdateAsync(Cliente cliente)
  +Task<bool> DeleteAsync(int id)
}

class ClienteServiceImpl {
}

interface IClienteRepository {
  +Task<IEnumerable<Cliente>> GetAllAsync()
  +Task<Cliente?> GetByIdAsync(int id)
  +Task<IEnumerable<Cliente>> FindByNameAsync(string nome)
  +Task<int> CountAsync()
  +Task<int> CreateAsync(Cliente cliente)
  +Task<bool> UpdateAsync(Cliente cliente)
  +Task<bool> DeleteAsync(int id)
}

class ClienteRepository {
}

class ClientesController {
}

class HealthController {
}

ClientesController --> IClienteService : uses
ClienteServiceImpl ..|> IClienteService
ClienteServiceImpl --> IClienteRepository : uses
IClienteRepository <|.. ClienteRepository
ClientesController --> Cliente : dto
ClienteServiceImpl --> Cliente : model

@enduml
