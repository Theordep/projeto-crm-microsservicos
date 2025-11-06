# CRM Pré-Moldados - Arquitetura de Microsserviços

Sistema de back-end (Prova de Conceito) para um CRM de nicho na indústria de pré-moldados, desenvolvido com arquitetura de microsserviços usando .NET 8.0 e SQLite.

## 🏗️ Arquitetura

O projeto implementa 3 microsserviços independentes, cada um com seu próprio banco de dados SQLite:

1. **ServicoUsuarios** - Gerencia usuários (Representantes e Comerciais)
2. **ServicoClientes** - Gerencia cadastro de clientes
3. **ServicoOportunidades** - Gerencia fichas/oportunidades de negócio (Core do CRM)

## 📋 Requisitos

- .NET 8.0 SDK
- Git
- Visual Studio Code ou Visual Studio (opcional)

## 🚀 Como Executar

### 1. Clone o repositório
```bash
git clone <url-do-repositorio>
cd AppTemplate
```

### 2. Execute os 3 serviços

Abra 3 terminais separados:

**Terminal 1 - ServicoUsuarios:**
```bash
cd ServicoUsuarios\ServicoUsuarios
dotnet run
```
Acesse: http://localhost:5001/swagger

**Terminal 2 - ServicoClientes:**
```bash
cd ServicoUsuarios\ServicoClientes\ServicoClientes
dotnet run
```
Acesse: http://localhost:5002/swagger

**Terminal 3 - ServicoOportunidades:**
```bash
cd ServicoUsuarios\ServicoClientes\ServicoOportunidades\ServicoOportunidades
dotnet run
```
Acesse: http://localhost:5003/swagger

## 🔄 Integrações entre Microsserviços

### Integração a.1 e a.2 (Busca de Dados)
Ao criar uma ficha (`POST /api/fichas`):
- Valida representante no ServicoUsuarios
- Valida cliente no ServicoClientes

### Integração b (Alteração de Dados)
Ao marcar ficha como "Vendido" (`PATCH /api/fichas/{id}/status`):
- Atualiza automaticamente o cliente para "Ativo" no ServicoClientes

## 📚 Documentação

- [README Microsserviços](README_MICROSSERVICOS.md) - Documentação completa dos microsserviços
- [Estrutura de Banco de Dados](Document/EstruturaBancoDados.md) - Schema das tabelas
- [Fluxo de Fichas](Document/FluxoFichas.md) - Fluxo de trabalho e regras de negócio

## 🗄️ Banco de Dados

Cada serviço possui seu próprio banco SQLite:
- `usuarios.db` - ServicoUsuarios
- `clientes.db` - ServicoClientes
- `oportunidades.db` - ServicoOportunidades

As migrations são aplicadas automaticamente na primeira execução.

## 🛠️ Tecnologias

- .NET 8.0
- ASP.NET Core Web API
- Entity Framework Core
- SQLite
- Swagger/OpenAPI

## 📝 Licença

Este projeto é uma prova de conceito para fins educacionais.
