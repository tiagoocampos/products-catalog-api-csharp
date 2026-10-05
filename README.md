# ProductCatalog

API REST de cadastro de produtos, feita em **C# / .NET 10** com arquitetura em camadas, PostgreSQL e testes unitários.

Projeto de estudo para praticar Clean Code, SOLID e boas práticas de back-end em .NET.

## Tecnologias

- C# / .NET 10 (ASP.NET Core Web API)
- Entity Framework Core + PostgreSQL (Npgsql)
- xUnit + Moq (testes unitários)
- Docker / Docker Compose

## Arquitetura

```
Controller  →  Service  →  Repository  →  Banco (PostgreSQL)
  (HTTP)       (regras)     (acesso a dados)
```

- **Controllers**: recebem a requisição e devolvem o status HTTP.
- **Services**: regras de negócio e validações.
- **Repositories**: acesso ao banco via EF Core (atrás de interface, para permitir testes com mock).
- **DTOs**: separam o que entra e sai da API das entidades do banco.
- **Middleware global de exceções**: transforma erros de regra de negócio em `400` com mensagem clara.

## Como rodar

Pré-requisitos: .NET 10 SDK e Docker.

```bash
# 1. Subir o banco
docker compose up -d

# 2. Aplicar as migrations (dentro da pasta ProductCatalog)
cd ProductCatalog
dotnet ef database update

# 3. Rodar a API
dotnet run
```

O PostgreSQL sobe na porta `5433`. As credenciais em `appsettings.json` são apenas para desenvolvimento local.

## Endpoints

| Método | Rota | Descrição |
|--------|------|-----------|
| GET | `/api/products` | Lista produtos ativos |
| GET | `/api/products/{id}` | Busca produto por id |
| POST | `/api/products` | Cadastra produto |
| PUT | `/api/products/{id}` | Atualiza produto |
| DELETE | `/api/products/{id}` | Desativa produto (soft delete) |

Exemplo de corpo (`POST` / `PUT`):

```json
{
  "name": "Teclado",
  "price": 150.90,
  "stock": 10
}
```

Regras de validação: nome obrigatório, preço maior que zero e estoque não negativo.

## Testes

```bash
dotnet test
```

Os testes do `ProductService` usam xUnit e Moq para isolar o repositório (sem banco de dados).

## Próximos passos

- Paginação e filtro por nome
- Autenticação com JWT
- Front-end em React consumindo a API

## Autor

Tiago Campos da Silva — [LinkedIn](https://linkedin.com/in/tiagocamposdasilva) · [GitHub](https://github.com/tiagoocampos)