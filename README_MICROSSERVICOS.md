# Arquitetura de Microsserviços - CRM Pré-Moldados

## Estrutura do Projeto

Este projeto implementa uma arquitetura de microsserviços com 3 serviços independentes, cada um com seu próprio banco de dados SQLite:

### 1. ServicoUsuarios (Porta 5001)
- **Banco de dados**: `usuarios.db`
- **Responsabilidade**: Gerencia a identidade e o perfil dos usuários
- **Endpoints**:
  - `POST /api/usuarios` - Cadastra um novo usuário
  - `GET /api/usuarios/{id}` - Busca dados de um usuário
  - `GET /api/usuarios` - Lista todos os usuários

### 2. ServicoClientes (Porta 5002)
- **Banco de dados**: `clientes.db`
- **Responsabilidade**: Gerencia o cadastro dos clientes
- **Endpoints**:
  - `POST /api/clientes` - Cadastra um novo cliente
  - `GET /api/clientes/{id}` - Busca dados de um cliente
  - `GET /api/clientes` - Lista todos os clientes
  - `PATCH /api/clientes/{id}/status` - Altera o status do cliente

### 3. ServicoOportunidades (Porta 5003)
- **Banco de dados**: `oportunidades.db`
- **Responsabilidade**: Gerencia as fichas/oportunidades de negócio (Core do CRM)
- **Endpoints**:
  - `POST /api/fichas` - Cria uma nova oportunidade (dispara integrações a.1 e a.2)
  - `GET /api/fichas/{id}` - Busca dados de uma ficha
  - `GET /api/fichas` - Lista todas as fichas
  - `PATCH /api/fichas/{id}/status` - Muda o status da ficha (dispara integração b)

## Integrações entre Microsserviços

### Integração a.1 e a.2 (Busca de Dados)
Quando `POST /api/fichas` é chamado:
1. O ServicoOportunidades chama `GET /api/usuarios/{id}` no ServicoUsuarios para validar se o representante existe
2. O ServicoOportunidades chama `GET /api/clientes/{id}` no ServicoClientes para validar se o cliente existe e buscar o nome

### Integração b (Alteração de Dados)
Quando `PATCH /api/fichas/{id}/status` é chamado com status "Vendido":
- O ServicoOportunidades chama `PATCH /api/clientes/{id_cliente}/status` no ServicoClientes
- Atualiza o status do cliente para "Ativo"

## Como Executar

### Pré-requisitos
- .NET 8.0 SDK instalado
- Visual Studio Code ou Visual Studio (opcional)

### Executar os Serviços

Abra 3 terminais separados e execute cada serviço:

**Terminal 1 - ServicoUsuarios:**
```bash
cd ServicoUsuarios
dotnet run
```
Acesse: http://localhost:5001/swagger

**Terminal 2 - ServicoClientes:**
```bash
cd ServicoClientes
dotnet run
```
Acesse: http://localhost:5002/swagger

**Terminal 3 - ServicoOportunidades:**
```bash
cd ServicoOportunidades
dotnet run
```
Acesse: http://localhost:5003/swagger

## Testando o Fluxo Completo

### 1. Criar um Usuário (Representante)
```http
POST http://localhost:5001/api/usuarios
Content-Type: application/json

{
  "nome": "João Silva",
  "email": "joao@example.com",
  "senha": "senha123",
  "tipoPerfil": "Representante"
}
```

### 2. Criar um Cliente
```http
POST http://localhost:5002/api/clientes
Content-Type: application/json

{
  "nomeRazaoSocial": "Construtora ABC",
  "cpfCnpj": "12345678000190",
  "representanteId": 1
}
```

### 3. Criar uma Ficha (Integração a.1 e a.2)
```http
POST http://localhost:5003/api/fichas
Content-Type: application/json

{
  "clienteId": 1,
  "representanteId": 1,
  "tituloObra": "Edifício Residencial",
  "descricaoSimples": "Construção de prédio de 10 andares",
  "valorEstimado": 5000000.00,
  "areaM2": 2500.00
}
```

### 4. Atualizar Status para Vendido (Integração b)
```http
PATCH http://localhost:5003/api/fichas/1/status
Content-Type: application/json

{
  "statusFicha": "Vendido"
}
```

Isso automaticamente atualiza o status do cliente para "Ativo" no ServicoClientes.

## Estrutura de Status

### Fichas
- **Em Cadastro**: Status inicial quando a ficha é criada
- **Em Análise**: Ficha sendo analisada pelo comercial
- **Vendido**: Venda concretizada (dispara atualização do cliente)
- **Cancelado**: Ficha cancelada

### Clientes
- **Prospect**: Status inicial
- **Ativo**: Cliente que teve uma venda concretizada (atualizado automaticamente)

## Migrations

O projeto usa **Entity Framework Migrations** para gerenciar o schema dos bancos de dados.

### Migrations já criadas:
- ✅ ServicoUsuarios: `InitialCreate`
- ✅ ServicoClientes: `InitialCreate`
- ✅ ServicoOportunidades: `InitialCreate`

### Como as migrations funcionam:

As migrations são aplicadas **automaticamente** quando você executa os serviços com `dotnet run`, pois cada `Program.cs` está configurado com `context.Database.Migrate()`.

### Comandos úteis para migrations:

**Criar nova migration:**
```bash
# Para ServicoUsuarios
cd ServicoUsuarios
dotnet ef migrations add NomeDaMigration

# Para ServicoClientes
cd ServicoClientes
dotnet ef migrations add NomeDaMigration

# Para ServicoOportunidades
cd ServicoOportunidades
dotnet ef migrations add NomeDaMigration
```

**Ver migrations pendentes:**
```bash
dotnet ef migrations list
```

**Aplicar migrations manualmente:**
```bash
dotnet ef database update
```

**Remover última migration:**
```bash
dotnet ef migrations remove
```

## Observações

- As migrations são aplicadas automaticamente na primeira execução
- Os serviços se comunicam via HTTP usando HttpClient
- As URLs dos serviços estão configuradas no `appsettings.json` do ServicoOportunidades
- Todos os serviços têm Swagger habilitado para testes

