# Próximos Passos - CRM Pré-Moldados

## ✅ O que já foi feito:
- [x] Estrutura de 3 microsserviços criada
- [x] Bancos de dados SQLite configurados
- [x] Migrations implementadas
- [x] Integrações entre microsserviços funcionando
- [x] Projeto no GitHub
- [x] Documentação básica

## 🎯 Próximos Passos Recomendados

### 1. Testar o Fluxo Completo (PRIORIDADE ALTA)
Testar todas as integrações funcionando:

- [ ] Criar usuários (Representante e Comercial)
- [ ] Criar clientes
- [ ] Criar fichas e verificar integrações a.1 e a.2
- [ ] Mudar status para "Vendido" e verificar integração b
- [ ] Testar casos de erro (usuário inexistente, cliente inexistente, etc.)

### 2. Melhorar o Código (PRIORIDADE MÉDIA)

#### 2.1 Corrigir Warnings
- [ ] Corrigir warnings de "referência possivelmente nula" nos Controllers
- [ ] Adicionar validações de null safety

#### 2.2 Melhorar Tratamento de Erros
- [ ] Criar classes de exceção customizadas
- [ ] Implementar middleware de tratamento de erros global
- [ ] Retornar mensagens de erro mais descritivas

#### 2.3 Adicionar Validações
- [ ] Validar CPF/CNPJ
- [ ] Validar email
- [ ] Validar dados de entrada (DTOs)
- [ ] Usar Data Annotations ou FluentValidation

### 3. Implementar Funcionalidades Faltantes (PRIORIDADE ALTA)

Baseado no documento `IdeiaRefinada.md`, ainda falta:

#### 3.1 Sistema de Revisões de Fichas
- [ ] Implementar campo `idficha` e `revisao_ficha`
- [ ] Lógica para criar novas revisões quando comercial rejeita
- [ ] Histórico de revisões

#### 3.2 Status Completos
- [ ] Implementar todos os status: "Não Enviado", "Em Cadastro", "Em Análise", "Em Andamento", "Orçado", "Vendido", "Cancelado", "Encerrado"
- [ ] Validações de transição de status (ex: não pode ir de "Em Cadastro" direto para "Vendido")

#### 3.3 Tipo de Obra
- [ ] Criar tabela `tipo_obra` no ServicoOportunidades
- [ ] Endpoint para listar tipos de obra
- [ ] Vincular ficha ao tipo de obra

#### 3.4 Histórico de Status
- [ ] Criar tabela `historico_status_fichas`
- [ ] Registrar todas as mudanças de status
- [ ] Endpoint para consultar histórico

#### 3.5 Log do Sistema
- [ ] Criar tabela `log_sistema`
- [ ] Registrar eventos importantes (cadastro, mudanças, erros)
- [ ] Endpoint para consultar logs (apenas Admin)

### 4. Melhorar Segurança (PRIORIDADE MÉDIA)

- [ ] Implementar autenticação JWT
- [ ] Implementar autorização por perfil (Representante, Comercial, Admin)
- [ ] Validar permissões nos endpoints
- [ ] Criptografar senhas com BCrypt (já está com SHA256, mas BCrypt é melhor)

### 5. Melhorar Documentação (PRIORIDADE BAIXA)

- [ ] Adicionar exemplos de requisições HTTP
- [ ] Criar diagrama de arquitetura
- [ ] Documentar fluxos de negócio mais detalhadamente
- [ ] Adicionar comentários XML nos métodos

### 6. Testes (PRIORIDADE MÉDIA)

- [ ] Adicionar testes unitários
- [ ] Adicionar testes de integração
- [ ] Testar integrações entre microsserviços

### 7. Melhorias de UX/API (PRIORIDADE BAIXA)

- [ ] Adicionar paginação nas listagens
- [ ] Adicionar filtros e busca
- [ ] Adicionar ordenação
- [ ] Melhorar mensagens de erro da API

### 8. DevOps (PRIORIDADE BAIXA)

- [ ] Adicionar Docker e Docker Compose
- [ ] Criar GitHub Actions para CI/CD
- [ ] Adicionar health checks nos serviços

## 🚀 Ordem Sugerida de Implementação

### Fase 1 - Testes e Correções (1-2 dias)
1. Testar fluxo completo
2. Corrigir warnings
3. Melhorar tratamento de erros

### Fase 2 - Funcionalidades Core (3-5 dias)
1. Sistema de revisões
2. Status completos com validações
3. Tipo de obra
4. Histórico de status

### Fase 3 - Segurança e Qualidade (2-3 dias)
1. Autenticação JWT
2. Autorização por perfil
3. Validações robustas

### Fase 4 - Melhorias (2-3 dias)
1. Log do sistema
2. Paginação e filtros
3. Testes

## 📝 Checklist Rápido

**Hoje:**
- [ ] Testar os 3 serviços funcionando
- [ ] Testar criar usuário, cliente e ficha
- [ ] Testar integração de mudança de status

**Esta Semana:**
- [ ] Implementar sistema de revisões
- [ ] Implementar todos os status
- [ ] Adicionar validações

**Próxima Semana:**
- [ ] Implementar autenticação
- [ ] Adicionar logs
- [ ] Melhorar documentação

