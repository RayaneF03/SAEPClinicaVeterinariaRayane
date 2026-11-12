# ClinicaVet

Sistema ASP.NET Core MVC + API para tutores, pets e agendamentos.

## Teste local

No desenvolvimento, o projeto usa SQLite automaticamente. Não é necessário SQL Server:

```bash
cd ClinicaVet
dotnet restore
dotnet build
dotnet run
```

Abra a URL exibida pelo terminal, normalmente `http://localhost:5183`.
O banco local fica em `clinica-local.db`.

Usuários de demonstração:

- `admin@clinicavet.local` / `Admin123!`
- `recepcao@clinicavet.local` / `Recepcao123!`
- `vet@clinicavet.local` / `Vet123!`

## SQL Server para entrega

O arquivo `appsettings.json` contém somente a configuração documentada, sem senha.
Substitua `SEU_NOME_AQUI` pelo nome definido para o banco e informe a senha somente
via User Secrets:

```bash
dotnet user-secrets init
dotnet user-secrets set "ConnectionStrings:ClinicaConnection" "Server=10.93.135.3\SQLExpress,1433;Database=saep_SEU_NOME_AQUI;User Id=sa;Password=SUA_SENHA;TrustServerCertificate=True;Encrypt=True;MultipleActiveResultSets=True"
```

Para usar o SQL Server, altere `DatabaseProvider` para `SqlServer` no arquivo
`appsettings.Development.json` ou execute com essa configuração definida no ambiente.

## 10. Gerar banco com Models e EF Core

Depois de salvar as models, execute os comandos abaixo na pasta que contém o `.csproj`:

```bash
DatabaseProvider=SqlServer dotnet ef database update
DatabaseProvider=SqlServer dotnet ef migrations script --idempotent --output saep_db.sql
```

1. A migration `CriacaoInicial` já existe na pasta `Migrations`; não execute `migrations add CriacaoInicial` novamente.
2. `database update` executa as migrations no SQL Server.
3. `migrations script` produz o arquivo `saep_db.sql` para acompanhar a entrega.

O projeto já contém a migration inicial e o script gerado em `saep_db.sql`.

Se as models forem alteradas no futuro, crie uma migration com outro nome:

```bash
DatabaseProvider=SqlServer dotnet ef migrations add AjustesDoModelo
DatabaseProvider=SqlServer dotnet ef database update
```

O prefixo `DatabaseProvider=SqlServer` é obrigatório para esses comandos porque o
modo de desenvolvimento usa SQLite local. O script idempotente não é suportado pelo SQLite.

## Funcionalidades

- Login com cookie, falha explícita, logout POST e expiração de 30 minutos.
- CPF protegido com ASP.NET Data Protection; nunca é exibido em telas ou na API.
- CRUD completo de tutores, pets e agendamentos.
- Busca de tutores por nome/e-mail.
- Pets associados obrigatoriamente a tutores.
- Agendamentos ordenados por data com pet e tutor.
- API autenticada em `/api/tutores`.

## Teste funcional obrigatório

Com a plataforma rodando (`dotnet run`), faça login e confirme:

1. Login inválido mostra mensagem de erro; login válido mostra o usuário no menu.
2. Crie três tutores, três pets associados a tutores e três agendamentos associados a pets.
3. Teste busca, edição, detalhes e exclusão respeitando os relacionamentos.
4. Confira a agenda em ordem de data e teste a API autenticada em `/api/tutores`.

O seed automático já cria três registros de cada tabela no primeiro uso do banco local.

## Entrega

Inclua `documentacao.pdf`, `der.png`, `saep_db.sql`, `README.md` e a pasta do projeto.
Não envie `bin`, `obj`, `.vs`, User Secrets, senhas ou backups do banco.
