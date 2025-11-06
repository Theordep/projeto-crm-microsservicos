# Como Criar as Migrations

Após instalar o `dotnet-ef`, você precisa **reabrir o terminal** para que o comando seja reconhecido.

## Passos para criar as migrations:

### 1. ServicoUsuarios
```bash
cd ServicoUsuarios\ServicoUsuarios
dotnet ef migrations add InitialCreate
```

### 2. ServicoClientes
```bash
cd ServicoUsuarios\ServicoClientes\ServicoClientes
dotnet ef migrations add InitialCreate
```

### 3. ServicoOportunidades
```bash
cd ServicoUsuarios\ServicoClientes\ServicoOportunidades\ServicoOportunidades
dotnet ef migrations add InitialCreate
```

## Após criar as migrations:

As migrations serão aplicadas automaticamente quando você executar os serviços com `dotnet run`, pois o `Program.cs` de cada serviço já está configurado com `context.Database.Migrate()`.

## Verificar se as migrations foram criadas:

Cada serviço terá uma pasta `Migrations` com os arquivos:
- `*_InitialCreate.cs` - Snapshot inicial
- `*_InitialCreate.Designer.cs` - Metadata
- `*ContextModelSnapshot.cs` - Model snapshot

## Comandos úteis:

- **Ver migrations pendentes**: `dotnet ef migrations list`
- **Remover última migration**: `dotnet ef migrations remove`
- **Aplicar migrations manualmente**: `dotnet ef database update`

