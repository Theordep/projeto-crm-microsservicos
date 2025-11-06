# Fluxo de Trabalho das Fichas

## Estados e Transições

```
[Não Enviado] 
    ↓ (Representante finaliza ficha)
[Em Cadastro]
    ↓ (Comercial inicia análise)
[Em Análise]
    ↓ (Aprovação)
[Em Andamento]
    ↓ (Orçamento enviado)
[Orçado]
    ↓ (Venda concretizada)
[Vendido] ✅
    ↓ (Atualiza cliente para "Ativo")

OU

[Qualquer Status]
    ↓ (Cancelamento)
[Cancelado] ❌

OU

[Qualquer Status (exceto Vendido)]
    ↓ (Encerramento)
[Encerrado] ⚠️
```

## Ações por Perfil

### Representante
- ✅ Cadastrar clientes
- ✅ Criar fichas (status inicial: "Não Enviado")
- ✅ Editar fichas no status "Não Enviado"
- ✅ Finalizar ficha (mudar para "Em Cadastro")
- ✅ Visualizar fichas próprias
- ❌ Não pode alterar status após enviar
- ❌ Não pode criar revisões (apenas criar nova ficha)

### Comercial
- ✅ Visualizar todas as fichas
- ✅ Analisar fichas (status: "Em Cadastro")
- ✅ Aprovar ficha (mudar para "Em Análise" → "Em Andamento")
- ✅ Rejeitar ficha e solicitar revisão (criar nova revisão, voltar para "Não Enviado")
- ✅ Marcar como "Orçado"
- ✅ Marcar como "Vendido" (dispara atualização do cliente)
- ✅ Cancelar ou encerrar fichas
- ❌ Não pode criar clientes
- ❌ Não pode criar fichas

### Admin
- ✅ Todas as ações dos outros perfis
- ✅ Gerenciar usuários
- ✅ Acessar logs completos
- ✅ Alterar qualquer dado

## Sistema de Revisões

### Quando criar uma revisão?
- Comercial rejeita ficha e solicita alterações
- Representante precisa fazer correções

### Como funciona:
1. Ficha original: idficha="0001", revisao=0, status="Em Cadastro"
2. Comercial rejeita e solicita alterações
3. Sistema cria nova entrada: idficha="0001", revisao=1, status="Não Enviado"
4. Ficha revisão 0 fica como histórico (ou status "Rejeitada")
5. Representante trabalha na revisão 1
6. Quando finaliza, revisão 1 vai para "Em Cadastro"

### Regras:
- Sempre trabalhar na revisão mais recente
- Histórico de revisões mantido para auditoria
- Cada revisão pode ter seu próprio histórico de status

## Campos Importantes das Fichas

### Dados Obrigatórios (para finalizar):
- cliente_id
- tipo_obra_id
- titulo_obra
- valor_estimado OU area_m2 (pelo menos um)

### Dados Opcionais:
- descricao_obra
- endereco_obra
- observacoes

## Histórico e Auditoria

### Tabela historico_status_fichas
Registra todas as mudanças de status com:
- Quem mudou
- Quando mudou
- Status anterior e novo
- Observação (opcional)

### Tabela log_sistema
Registra eventos importantes:
- Criação de clientes
- Criação de fichas
- Mudanças de status
- Criação de revisões
- Logins/logouts
- Erros do sistema
- Ações administrativas

