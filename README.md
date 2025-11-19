# CRM Pré-Moldados - Arquitetura de Microsserviços

Sistema de back-end (Prova de Conceito) para um CRM de nicho na indústria de pré-moldados, desenvolvido com arquitetura de microsserviços usando .NET 8.0 e SQLite.

---

## 📄 Documento de Requisitos

### Propósito do Sistema

Desenvolver um sistema de back-end (Prova de Conceito) para um CRM de nicho voltado à indústria de pré-moldados, com o objetivo de gerenciar o cadastro de clientes e o ciclo de vida das oportunidades de negócio (fichas/propostas).

### Usuários do Sistema

**Representante**
- Responsável por cadastrar novos clientes (Prospects)
- Cria as oportunidades de negócio iniciais (fichas)
- Preenche dados técnicos das obras

**Comercial**
- Responsável por analisar as oportunidades criadas pelos representantes
- Atualiza status das oportunidades (aprovar, vender, cancelar)
- Acompanha o ciclo de vida das propostas

### Requisitos Funcionais

**(RF001)** O sistema deve permitir o cadastro de usuários (Representantes e Comerciais).

**(RF002)** O sistema deve permitir o cadastro de Clientes, vinculados a um Representante.

**(RF003)** O sistema deve permitir o cadastro de Oportunidades de negócio (fichas), vinculadas a um Cliente e um Representante.

**(RF004)** Ao ser criada, uma Oportunidade deve ter o status inicial "Em Cadastro".

**(RF005)** O sistema deve permitir a alteração do status de uma Oportunidade (ex: "Em Análise", "Vendido", "Cancelado").

**(RF006)** O sistema deve buscar e validar os dados do Cliente ao criar uma Oportunidade **(Integração a.2)**.

**(RF007)** O sistema deve buscar e validar os dados do Usuário (Representante) ao criar uma Oportunidade **(Integração a.1)**.

**(RF008)** Ao alterar o status de uma Oportunidade para "Vendido", o sistema deve automaticamente atualizar o status do Cliente para "Ativo" **(Integração b)**.

---

## 🏗️ Descritivo Técnico - Microsserviços

O projeto implementa **3 microsserviços independentes**, cada um com seu próprio banco de dados SQLite e responsabilidades bem definidas, seguindo os princípios de arquitetura de microsserviços.

### Microsserviço 1: ServicoUsuarios

**Porta:** 5001

**Função:** Gerencia a identidade e o perfil dos usuários do sistema.

**Banco de Dados:** `usuarios.db`

**Tabela Principal:**
- **usuarios**: id (PK), nome, email (UNIQUE), senha_hash, tipo_perfil ('Representante' ou 'Comercial')

**Endpoints Principais:**
- `POST /api/usuarios` - Cadastra um novo usuário
- `GET /api/usuarios/{id}` - Busca dados de um usuário (usado na **Integração a.1**)
- `GET /api/usuarios` - Lista todos os usuários

**Integrações:**
- É consultado pelo ServicoOportunidades para validar representantes ao criar fichas
- É consultado pelo ServicoClientes para validar representantes ao criar clientes

---

### Microsserviço 2: ServicoClientes

**Porta:** 5002

**Função:** Gerencia o cadastro dos clientes (construtoras, indústrias, etc.).

**Banco de Dados:** `clientes.db`

**Tabela Principal:**
- **clientes**: id (PK), nome_razao_social, cpf_cnpj (UNIQUE), representante_id (FK), status_cliente ('Prospect' ou 'Ativo')

**Endpoints Principais:**
- `POST /api/clientes` - Cadastra um novo cliente (valida representante no ServicoUsuarios)
- `GET /api/clientes/{id}` - Busca dados de um cliente (usado na **Integração a.2**)
- `GET /api/clientes` - Lista todos os clientes
- `PATCH /api/clientes/{id}/status` - Altera o status do cliente (usado na **Integração b**)

**Integrações:**
- Consulta o ServicoUsuarios para validar se o representante existe ao criar um cliente
- É consultado pelo ServicoOportunidades para validar clientes ao criar fichas
- Recebe alteração do ServicoOportunidades quando uma venda é concretizada

---

### Microsserviço 3: ServicoOportunidades (Core do CRM)

**Porta:** 5003

**Função:** Gerencia as fichas/oportunidades de negócio e seu ciclo de vida. É o microsserviço central do sistema.

**Banco de Dados:** `oportunidades.db`

**Tabela Principal:**
- **fichas**: id (PK), cliente_id (FK), representante_id (FK), status_ficha ('Em Cadastro', 'Em Análise', 'Vendido', 'Cancelado'), titulo_obra, descricao_simples, valor_estimado, area_m2

**Endpoints Principais:**
- `POST /api/fichas` - Cria uma nova oportunidade (dispara **Integrações a.1 e a.2**)
- `GET /api/fichas/{id}` - Busca dados de uma ficha
- `GET /api/fichas` - Lista todas as fichas
- `PATCH /api/fichas/{id}/status` - Muda o status da ficha (dispara **Integração b** quando status = "Vendido")

**Integrações:**
- Consulta o ServicoUsuarios para validar se o representante existe (**Integração a.1**)
- Consulta o ServicoClientes para validar se o cliente existe (**Integração a.2**)
- Altera dados no ServicoClientes quando uma venda é concretizada (**Integração b**)

---

## 🔗 Detalhamento das Integrações entre Microsserviços

### Integração a.1 - Busca de Dados: ServicoOportunidades → ServicoUsuarios

**Quando ocorre:** Ao criar uma nova ficha (`POST /api/fichas`)

**Fluxo:**
1. ServicoOportunidades recebe requisição com `representanteId`
2. Faz chamada HTTP: `GET http://localhost:5001/api/usuarios/{representanteId}`
3. Se o representante não existir, retorna erro 400
4. Se existir, continua com a criação da ficha

**Tipo:** Busca de dados simples para validação

---

### Integração a.2 - Busca de Dados: ServicoOportunidades → ServicoClientes

**Quando ocorre:** Ao criar uma nova ficha (`POST /api/fichas`)

**Fluxo:**
1. ServicoOportunidades recebe requisição com `clienteId`
2. Faz chamada HTTP: `GET http://localhost:5002/api/clientes/{clienteId}`
3. Se o cliente não existir, retorna erro 400
4. Se existir, busca o nome do cliente e continua com a criação da ficha

**Tipo:** Busca de dados simples para validação

---

### Integração b - Alteração de Dados: ServicoOportunidades → ServicoClientes

**Quando ocorre:** Ao mudar status de uma ficha para "Vendido" (`PATCH /api/fichas/{id}/status`)

**Fluxo:**
1. ServicoOportunidades recebe requisição para alterar status para "Vendido"
2. Detecta que o novo status é "Vendido"
3. Faz chamada HTTP: `PATCH http://localhost:5002/api/clientes/{clienteId}/status`
4. Envia no body: `{ "StatusCliente": "Ativo" }`
5. ServicoClientes atualiza o status do cliente de "Prospect" para "Ativo"
6. ServicoOportunidades atualiza o status da ficha para "Vendido"

**Tipo:** Alteração de dados em cascata entre microsserviços

---

## 🛠️ Tecnologias Utilizadas

- **.NET 8.0** - Framework principal
- **ASP.NET Core Web API** - Para criar APIs REST
- **Entity Framework Core** - ORM para gerenciamento de banco de dados
- **SQLite** - Banco de dados leve e independente para cada serviço
- **Swagger/OpenAPI** - Documentação automática e interface de testes
- **HttpClient** - Comunicação HTTP entre microsserviços

---

## 🗄️ Bancos de Dados

Cada microsserviço possui seu próprio banco de dados SQLite, garantindo independência e isolamento:

- **ServicoUsuarios**: `usuarios.db`
- **ServicoClientes**: `clientes.db`
- **ServicoOportunidades**: `oportunidades.db`

As migrations do Entity Framework são aplicadas automaticamente na primeira execução de cada serviço.

---

## 🚀 Como Executar o Projeto

### Pré-requisitos

- .NET 8.0 SDK instalado
- Git (opcional)
- Visual Studio Code ou Visual Studio (opcional)

### Executar os 3 Serviços

Abra **3 terminais separados** e execute cada serviço:

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

---

## 🧪 Como Testar as Integrações

### Passo 1: Criar um Usuário (Representante)

Acesse: http://localhost:5001/swagger

```json
POST /api/usuarios
{
  "nome": "João Silva",
  "email": "joao@example.com",
  "senha": "senha123",
  "tipoPerfil": "Representante"
}
```

✅ Anote o **ID retornado** (exemplo: 1)

### Passo 2: Criar um Cliente

Acesse: http://localhost:5002/swagger

```json
POST /api/clientes
{
  "nomeRazaoSocial": "Construtora ABC Ltda",
  "cpfCnpj": "12345678000190",
  "representanteId": 1
}
```

✅ Anote o **ID retornado** (exemplo: 1)
✅ Note que o status inicial é **"Prospect"**

### Passo 3: Criar uma Ficha (Testa Integrações a.1 e a.2)

Acesse: http://localhost:5003/swagger

```json
POST /api/fichas
{
  "clienteId": 1,
  "representanteId": 1,
  "tituloObra": "Edifício Residencial Premium",
  "descricaoSimples": "Construção de prédio de 10 andares",
  "valorEstimado": 5000000.00,
  "areaM2": 2500.00
}
```

**O que acontece nos bastidores:**
- ✅ Valida representante no ServicoUsuarios (Integração a.1)
- ✅ Valida cliente no ServicoClientes (Integração a.2)
- ✅ Cria a ficha com status "Em Cadastro"

### Passo 4: Mudar Status para "Vendido" (Testa Integração b)

Ainda em: http://localhost:5003/swagger

```json
PATCH /api/fichas/1/status
{
  "statusFicha": "Vendido"
}
```

**O que acontece nos bastidores:**
- ✅ Atualiza a ficha para "Vendido"
- ✅ Chama automaticamente o ServicoClientes e atualiza o cliente para "Ativo" (Integração b)

### Passo 5: Verificar Cliente Atualizado

Volte para: http://localhost:5002/swagger

```
GET /api/clientes/1
```

✅ **Confirme que o status mudou de "Prospect" para "Ativo"** automaticamente!

### Testes de Validação (Casos de Erro)

**Teste com Representante Inválido:**
```json
POST /api/fichas
{
  "clienteId": 1,
  "representanteId": 999,
  ...
}
```
❌ Deve retornar erro 400 - "Representante não encontrado"

**Teste com Cliente Inválido:**
```json
POST /api/fichas
{
  "clienteId": 999,
  "representanteId": 1,
  ...
}
```
❌ Deve retornar erro 400 - "Cliente não encontrado"

---

## 📊 Estrutura do Projeto

```
AppTemplate/
│
├── ServicoUsuarios/              # Microsserviço 1
│   ├── Controllers/
│   ├── Models/
│   ├── Services/
│   ├── Data/
│   ├── DTOs/
│   ├── Migrations/
│   └── usuarios.db
│
├── ServicoClientes/              # Microsserviço 2
│   ├── Controllers/
│   ├── Models/
│   ├── Services/
│   │   └── IntegracaoService.cs  # Integra com ServicoUsuarios
│   ├── Data/
│   ├── DTOs/
│   ├── Migrations/
│   └── clientes.db
│
├── ServicoOportunidades/         # Microsserviço 3 (Core)
│   ├── Controllers/
│   ├── Models/
│   ├── Services/
│   │   └── IntegracaoService.cs  # Integra com Usuarios e Clientes
│   ├── Data/
│   ├── DTOs/
│   ├── Migrations/
│   └── oportunidades.db
│
└── README.md                     # Este arquivo
```

---

## 📝 Observações Importantes

- **Independência**: Cada microsserviço pode ser executado, desenvolvido e deployado independentemente
- **Comunicação**: Os serviços se comunicam via HTTP/REST usando `HttpClient`
- **Resiliência**: Se um serviço cair, os outros continuam funcionando (embora as integrações falhem)
- **Escalabilidade**: Cada serviço pode ser escalado horizontalmente de forma independente
- **Bancos Separados**: Cada serviço tem seu próprio banco, seguindo o princípio de Database per Service

---

## 📚 Documentação Adicional

- [Fluxo de Fichas](Document/FluxoFichas.md) - Fluxo de trabalho e regras de negócio
- [Estrutura de Banco de Dados](Document/EstruturaBancoDados.md) - Schema completo planejado

---

## 👨‍💻 Autores

Pedro Ernesto, Octavio Da Silva Demos, Ana Laura Vicenzi 
---