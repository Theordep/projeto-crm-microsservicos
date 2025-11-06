4. Documento de Requisitos
a. Propósito do sistema: Desenvolver um sistema de back-end (Prova de Conceito) para um CRM de nicho (indústria de pré-moldados), com o objetivo de gerenciar o cadastro de clientes e o ciclo de vida das oportunidades de negócio (propostas).

b. Quais são os usuários:

Representante: Responsável por cadastrar novos clientes (Prospects) e criar as oportunidades de negócio iniciais.

Comercial: Responsável por analisar as oportunidades criadas e atualizar seus status (ex: aprovar, vender, cancelar).

c. Listar os requisitos funcionais:

(RF001) O sistema deve permitir o cadastro de usuários (Representantes e Comerciais).

(RF002) O sistema deve permitir o cadastro de Clientes, vinculados a um Representante.

(RF003) O sistema deve permitir o cadastro de Oportunidades de negócio, vinculadas a um Cliente.

(RF004) Ao ser criada, uma Oportunidade deve ter o status "Em Cadastro".

(RF005) O sistema deve permitir a alteração do status de uma Oportunidade (ex: "Em Análise", "Vendido", "Cancelado").

(RF006) O sistema deve buscar e validar os dados do Cliente ao criar uma Oportunidade.

(RF007) O sistema deve buscar e validar os dados do Usuário (Representante) ao criar uma Oportunidade.

(RF008) Ao alterar o status de uma Oportunidade para "Vendido", o sistema deve automaticamente atualizar o status do Cliente para "Ativo".

5. Descritivo Técnico (Microsserviços)
Aqui está a divisão em 3 serviços, cada um com seu próprio banco de dados SQLite (como manda a arquitetura) e suas responsabilidades.

Microsserviço 1: ServicoUsuarios
Função: Gerencia a identidade e o perfil dos usuários.

Database (usuarios.db):

usuarios: id (PK), nome, email (UNIQUE), senha_hash, tipo_perfil (Texto: 'Representante' ou 'Comercial')

Endpoints Principais:

POST /usuarios (Cadastra um novo usuário)

GET /usuarios/{id} (Busca dados de um usuário. Usado na Integração a.1)

Microsserviço 2: ServicoClientes
Função: Gerencia o cadastro dos clientes (as construtoras, indústrias, etc.).

Database (clientes.db):

clientes: id (PK), nome_razao_social, cpf_cnpj (UNIQUE), representante_id (ID do usuário que o cadastrou), status_cliente (Texto: 'Prospect' ou 'Ativo')

Endpoints Principais:

POST /clientes (Cadastra um novo cliente)

GET /clientes/{id} (Busca dados de um cliente. Usado na Integração a.2)

PATCH /clientes/{id}/status (Altera o status. Usado na Integração b)

Microsserviço 3: ServicoOportunidades (O "Core" do CRM)
Função: Gerencia as fichas/oportunidades de negócio e seu ciclo de vida.

Database (oportunidades.db):

fichas: id (PK), cliente_id, representante_id, status_ficha (Texto: 'Em Cadastro', 'Em Análise', 'Vendido', 'Cancelado'), titulo_obra (Texto), descricao_simples (Texto), valor_estimado (REAL), area_m2 (REAL)

Endpoints Principais:

POST /fichas (Cria uma nova oportunidade. Dispara as Integrações a.1 e a.2)

PATCH /fichas/{id}/status (Muda o status da ficha. Dispara a Integração b)

🎯 Como isso cumpre os Requisitos de Integração (Req. 2)
Este é o ponto-chave para o seu professor:

a. Duas buscas de dados simples:

Quando alguém chama POST /fichas (no ServicoOportunidades), este serviço chama o ServicoUsuarios (GET /usuarios/{id}) para validar se o representante_id existe.

Ao mesmo tempo, o ServicoOportunidades também chama o ServicoClientes (GET /clientes/{id}) para validar se o cliente_id existe e buscar o nome dele.

b. Uma alteração de dados:

Quando alguém chama PATCH /fichas/{id}/status (no ServicoOportunidades) e o novo status é "Vendido":

O ServicoOportunidades dispara uma alteração no ServicoClientes, chamando o endpoint PATCH /clientes/{id_do_cliente}/status com o corpo { "status_cliente": "Ativo" }.

Isso fecha perfeitamente com o que foi pedido, é simples de implementar com .NET e SQLite.