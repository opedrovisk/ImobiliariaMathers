# Imobili-ria
Projeto desenvolvido para um sistema de imobiliária.

## Configuração local (JWT e conexão)

1. Copie `ImobiliariaViviane/ImobiliariaViviane/appsettings.Local.example.json` para `ImobiliariaViviane/ImobiliariaViviane/appsettings.Local.json`.
2. Preencha uma chave JWT forte em `Jwt:Key` (32+ caracteres).
3. Ajuste a connection string local se necessário.
4. Se quiser exibir códigos de recuperação no cadastro apenas localmente, use `Security:ExposeRecoveryCodesOnRegister = true` no `appsettings.Local.json`.

`appsettings.Local.json` está no `.gitignore` e não será enviado ao GitHub.

Alternativa via variável de ambiente:

- `Jwt__Key`

## Banco de dados MySQL

Script idempotente (cria e ajusta schema):

- `tools/mysql/setup_imobiliaria_viviane.sql`

Execução via terminal:

```powershell
mysql -u root -p < .\tools\mysql\setup_imobiliaria_viviane.sql
```

Depois rode a aplicação normalmente.

## Segurança de requisições

As rotas de escrita usam anti-forgery token e exigem header `X-CSRF-TOKEN` nas chamadas `POST`, `PUT` e `DELETE` feitas pelo frontend.
