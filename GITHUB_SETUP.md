# Como Subir o Projeto no GitHub

## Passo 1: Criar o Repositório no GitHub

1. Acesse https://github.com
2. Clique no botão **"+"** no canto superior direito
3. Selecione **"New repository"**
4. Preencha:
   - **Repository name**: `crm-pre-moldados` (ou outro nome de sua escolha)
   - **Description**: "Sistema CRM para indústria de pré-moldados com arquitetura de microsserviços"
   - Selecione **Public** ou **Private** (conforme preferir)
   - **NÃO** marque "Initialize this repository with a README" (já temos arquivos)
5. Clique em **"Create repository"**

## Passo 2: Conectar o Repositório Local ao GitHub

Após criar o repositório no GitHub, você verá uma página com instruções. Execute os comandos abaixo no terminal:

### Se você escolheu HTTPS:
```bash
git remote add origin https://github.com/SEU_USUARIO/crm-pre-moldados.git
git branch -M main
git push -u origin main
```

### Se você escolheu SSH:
```bash
git remote add origin git@github.com:SEU_USUARIO/crm-pre-moldados.git
git branch -M main
git push -u origin main
```

**⚠️ IMPORTANTE**: Substitua `SEU_USUARIO` pelo seu nome de usuário do GitHub!

## Passo 3: Verificar se está tudo certo

Após o push, acesse seu repositório no GitHub e verifique se todos os arquivos foram enviados corretamente.

## Comandos Adicionais Úteis

### Ver status do repositório:
```bash
git status
```

### Ver histórico de commits:
```bash
git log --oneline
```

### Fazer novo commit após alterações:
```bash
git add .
git commit -m "Descrição das alterações"
git push
```

### Ver remotes configurados:
```bash
git remote -v
```

## ⚠️ Arquivos que NÃO serão enviados

O arquivo `.gitignore` está configurado para ignorar:
- Pastas `bin/` e `obj/` (arquivos compilados)
- Arquivos `.db` (bancos de dados SQLite)
- Arquivos de configuração do Visual Studio (`.vs/`)
- Arquivos temporários e de log

Isso é importante para manter o repositório limpo e seguro.

## 📝 Nota sobre Migrations

As migrations **serão enviadas** para o GitHub (estão na pasta `Migrations/` de cada serviço). Isso é recomendado para que outros desenvolvedores possam recriar o banco de dados.

