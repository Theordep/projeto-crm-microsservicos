# Estrutura de Banco de Dados - CRM Pré-Moldados

## Proposta de Tabelas

### usuarios
- id (INTEGER PK, AUTOINCREMENT)
- nome (TEXT NOT NULL)
- email (TEXT NOT NULL UNIQUE)
- senha_hash (TEXT NOT NULL)
- tipo_perfil (TEXT NOT NULL) - Valores: 'Representante', 'Comercial', 'Admin'
- ativo (INTEGER DEFAULT 1) - 0 = Inativo, 1 = Ativo
- data_criacao (DATETIME DEFAULT CURRENT_TIMESTAMP)
- data_ultimo_acesso (DATETIME NULL)

### clientes
- id (INTEGER PK, AUTOINCREMENT)
- nome_razao_social (TEXT NOT NULL)
- cpf_cnpj (TEXT NOT NULL UNIQUE)
- telefone (TEXT NULL)
- email (TEXT NULL)
- endereco (TEXT NULL)
- cidade (TEXT NULL)
- estado (TEXT NULL)
- cep (TEXT NULL)
- representante_id (INTEGER NOT NULL, FK -> usuarios.id)
- status_cliente (TEXT NOT NULL DEFAULT 'Prospect') - Valores: 'Prospect', 'Ativo', 'Inativo'
- observacoes (TEXT NULL)
- data_criacao (DATETIME DEFAULT CURRENT_TIMESTAMP)
- data_atualizacao (DATETIME NULL)

### tipo_obra
- id (INTEGER PK, AUTOINCREMENT)
- descricao (TEXT NOT NULL UNIQUE) - Valores: 'Mercantil', 'Global', 'Estacas', 'Lajes'
- ativo (INTEGER DEFAULT 1)

### fichas_entrada
- id (INTEGER PK, AUTOINCREMENT)
- idficha (TEXT NOT NULL) - Formato: 0001, 0002, etc. (único por idficha+revisao)
- revisao_ficha (INTEGER NOT NULL DEFAULT 0) - 0 = Original, 1+ = Revisões
- cliente_id (INTEGER NOT NULL, FK -> clientes.id)
- representante_id (INTEGER NOT NULL, FK -> usuarios.id)
- tipo_obra_id (INTEGER NOT NULL, FK -> tipo_obra.id)
- status_ficha (TEXT NOT NULL DEFAULT 'Não Enviado') - Valores: 'Não Enviado', 'Em Cadastro', 'Em Análise', 'Em Andamento', 'Orçado', 'Vendido', 'Cancelado', 'Encerrado'
- titulo_obra (TEXT NOT NULL)
- descricao_obra (TEXT NULL)
- valor_estimado (REAL NULL)
- area_m2 (REAL NULL)
- endereco_obra (TEXT NULL)
- cidade_obra (TEXT NULL)
- estado_obra (TEXT NULL)
- observacoes (TEXT NULL)
- data_criacao (DATETIME DEFAULT CURRENT_TIMESTAMP)
- data_envio (DATETIME NULL) - Quando mudou de "Não Enviado" para "Em Cadastro"
- data_ultima_atualizacao (DATETIME NULL)
- usuario_ultima_atualizacao_id (INTEGER NULL, FK -> usuarios.id)

### historico_status_fichas
- id (INTEGER PK, AUTOINCREMENT)
- ficha_id (INTEGER NOT NULL, FK -> fichas_entrada.id)
- status_anterior (TEXT NULL)
- status_novo (TEXT NOT NULL)
- observacao_mudanca (TEXT NULL)
- usuario_id (INTEGER NOT NULL, FK -> usuarios.id)
- data_mudanca (DATETIME DEFAULT CURRENT_TIMESTAMP)

### log_sistema
- id (INTEGER PK, AUTOINCREMENT)
- tipo_log (TEXT NOT NULL) - Valores: 'CadastroCliente', 'CadastroFicha', 'MudancaStatus', 'RevisaoFicha', 'Login', 'Logout', 'Erro', 'AcaoAdmin'
- entidade_tipo (TEXT NULL) - 'Cliente', 'Ficha', 'Usuario', etc.
- entidade_id (INTEGER NULL) - ID da entidade relacionada
- usuario_id (INTEGER NULL, FK -> usuarios.id)
- descricao (TEXT NOT NULL)
- dados_anteriores (TEXT NULL) - JSON com dados antes da mudança (opcional)
- dados_novos (TEXT NULL) - JSON com dados após a mudança (opcional)
- ip_address (TEXT NULL)
- data_log (DATETIME DEFAULT CURRENT_TIMESTAMP)

## Observações sobre o Fluxo

### Status das Fichas:
1. **Não Enviado**: Ficha criada, mas ainda não finalizada pelo representante
2. **Em Cadastro**: Ficha finalizada e enviada para análise
3. **Em Análise**: Comercial está analisando a ficha
4. **Em Andamento**: Ficha aprovada, em processo de orçamento
5. **Orçado**: Orçamento enviado ao cliente
6. **Vendido**: Venda concretizada (ao mudar para este status, atualizar cliente para "Ativo")
7. **Cancelado**: Ficha cancelada em qualquer etapa
8. **Encerrado**: Ficha encerrada sem venda (ex: cliente desistiu)

### Sistema de Revisão:
- Cada ficha tem um `idficha` único (ex: "0001")
- Quando o comercial não aprova e pede alterações, cria-se uma nova revisão
- Exemplo: 
  - Ficha 0001, Revisão 0 (original)
  - Ficha 0001, Revisão 1 (após primeira alteração)
  - Ficha 0001, Revisão 2 (após segunda alteração)
- A revisão mais recente é sempre a ativa

### Regras de Negócio:
- Ao mudar status para "Vendido": atualizar `status_cliente` para "Ativo"
- Representante só pode criar fichas para clientes que ele cadastrou
- Comercial pode alterar status de fichas de qualquer representante
- Admin tem acesso total
- Todas as mudanças de status devem ser registradas em `historico_status_fichas`
- Todos os eventos importantes devem ser registrados em `log_sistema`

