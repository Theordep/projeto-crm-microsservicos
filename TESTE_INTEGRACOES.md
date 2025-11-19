# 🧪 Guia de Teste - Integrações entre Microsserviços

## ✅ Os 3 serviços devem estar rodando:
- ServicoUsuarios: http://localhost:5001
- ServicoClientes: http://localhost:5002
- ServicoOportunidades: http://localhost:5003

## 📋 TESTE COMPLETO - PASSO A PASSO

### PASSO 1: Criar um Usuário (Representante)

**Via Swagger:**
1. Abra: http://localhost:5001/swagger
2. Clique em `POST /api/usuarios`
3. Clique em "Try it out"
4. Cole este JSON:

```json
{
  "nome": "João Silva",
  "email": "joao@example.com",
  "senha": "senha123",
  "tipoPerfil": "Representante"
}
```

5. Clique em "Execute"
6. **✅ Anote o ID retornado** (provavelmente será 1)

---

### PASSO 2: Criar um Cliente

**Via Swagger:**
1. Abra: http://localhost:5002/swagger
2. Clique em `POST /api/clientes`
3. Clique em "Try it out"
4. Cole este JSON (use o ID do representante criado acima):

```json
{
  "nomeRazaoSocial": "Construtora ABC Ltda",
  "cpfCnpj": "12345678000190",
  "representanteId": 1
}
```

5. Clique em "Execute"
6. **✅ Anote o ID retornado** (provavelmente será 1)
7. **✅ Note que StatusCliente é "Prospect"**

---

### PASSO 3: Criar uma Ficha 🔥 (TESTA INTEGRAÇÕES a.1 e a.2)

**Via Swagger:**
1. Abra: http://localhost:5003/swagger
2. Clique em `POST /api/fichas`
3. Clique em "Try it out"
4. Cole este JSON:

```json
{
  "clienteId": 1,
  "representanteId": 1,
  "tituloObra": "Edifício Residencial Premium",
  "descricaoSimples": "Construção de prédio de 10 andares com área comercial",
  "valorEstimado": 5000000.00,
  "areaM2": 2500.00
}
```

5. Clique em "Execute"

**🔍 O QUE ESTÁ ACONTECENDO NOS BASTIDORES:**
```
ServicoOportunidades recebe a requisição
    ↓
Chama GET http://localhost:5001/api/usuarios/1
    ↓ (valida se representante existe)
Chama GET http://localhost:5002/api/clientes/1
    ↓ (valida se cliente existe)
Cria a ficha no banco oportunidades.db
```

6. **✅ Anote o ID da ficha** (provavelmente será 1)
7. **✅ Note que StatusFicha é "Em Cadastro"**

---

### PASSO 4: Verificar Status Atual do Cliente

**Antes de mudar a ficha para "Vendido", vamos confirmar o status do cliente:**

1. Abra: http://localhost:5002/swagger
2. Clique em `GET /api/clientes/{id}`
3. Clique em "Try it out"
4. Digite: `1`
5. Clique em "Execute"
6. **✅ Confirme que StatusCliente ainda é "Prospect"**

---

### PASSO 5: Mudar Status da Ficha para "Vendido" 🔥 (TESTA INTEGRAÇÃO b)

**Via Swagger:**
1. Abra: http://localhost:5003/swagger
2. Clique em `PATCH /api/fichas/{id}/status`
3. Clique em "Try it out"
4. Digite o ID da ficha: `1`
5. Cole este JSON:

```json
{
  "statusFicha": "Vendido"
}
```

6. Clique em "Execute"

**🔍 O QUE ESTÁ ACONTECENDO NOS BASTIDORES:**
```
ServicoOportunidades recebe a requisição
    ↓
Detecta que novo status é "Vendido"
    ↓
Chama PATCH http://localhost:5002/api/clientes/1/status
    Body: { "StatusCliente": "Ativo" }
    ↓ (atualiza cliente automaticamente)
Atualiza a ficha para "Vendido"
```

7. **✅ Verifique que o response mostra StatusFicha = "Vendido"**

---

### PASSO 6: Verificar que Cliente foi Atualizado Automaticamente! ✨

**Via Swagger:**
1. Volte para: http://localhost:5002/swagger
2. Clique em `GET /api/clientes/{id}`
3. Clique em "Try it out"
4. Digite: `1`
5. Clique em "Execute"
6. **✅ CONFIRME: StatusCliente agora é "Ativo"** 🎉

---

## 🎯 TESTE DE VALIDAÇÃO (deve dar erro)

### Testar Integração a.1 - Representante Inválido

1. Abra: http://localhost:5003/swagger
2. Clique em `POST /api/fichas`
3. Tente criar com representanteId que não existe:

```json
{
  "clienteId": 1,
  "representanteId": 999,
  "tituloObra": "Teste",
  "descricaoSimples": "Teste",
  "valorEstimado": 10000.00,
  "areaM2": 100.00
}
```

**✅ Deve retornar erro 400 - "Representante não encontrado"**

---

### Testar Integração a.2 - Cliente Inválido

1. Abra: http://localhost:5003/swagger
2. Clique em `POST /api/fichas`
3. Tente criar com clienteId que não existe:

```json
{
  "clienteId": 999,
  "representanteId": 1,
  "tituloObra": "Teste",
  "descricaoSimples": "Teste",
  "valorEstimado": 10000.00,
  "areaM2": 100.00
}
```

**✅ Deve retornar erro 400 - "Cliente não encontrado"**

---

## 📊 RESUMO DO QUE FOI TESTADO

✅ **Integração a.1**: ServicoOportunidades → ServicoUsuarios (valida representante)  
✅ **Integração a.2**: ServicoOportunidades → ServicoClientes (valida cliente)  
✅ **Integração b**: ServicoOportunidades → ServicoClientes (atualiza status automaticamente)  
✅ **Validações**: Erros quando dados não existem  



---

## 💡 DICAS

- Use o Swagger - é visual e fácil de testar
- Teste primeiro o fluxo feliz (tudo funcionando)
- Depois teste os erros (IDs inválidos)
- Observe os logs no terminal de cada serviço
- Use as URLs do Swagger para ver a documentação completa

**Acesse:**
- 🟢 http://localhost:5001/swagger (ServicoUsuarios)
- 🔵 http://localhost:5002/swagger (ServicoClientes)
- 🟣 http://localhost:5003/swagger (ServicoOportunidades)

