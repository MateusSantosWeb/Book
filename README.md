# BookShelf API

API em ASP.NET Core 8 com Entity Framework Core e PostgreSQL.

## Banco de dados

- Provider: `Npgsql.EntityFrameworkCore.PostgreSQL`
- A aplicação lê a conexão nesta ordem:
1. `DATABASE_URL` (padrão do Render)
2. `ConnectionStrings:DefaultConnection`

As migrations são executadas automaticamente no startup via `db.Database.Migrate()`.

## Deploy no Render

Este repositório já inclui `render.yaml` com:
- Um banco PostgreSQL gerenciado (`bookshelf-db`)
- Um serviço web Docker (`bookshelf-api`)
- `DATABASE_URL` ligado automaticamente ao banco

## Rodar local com Docker Compose (Postgres)

```bash
docker compose up -d
```
