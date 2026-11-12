# ClinicaVet

Sistema ASP.NET Core MVC + API para tutores, pets e agendamentos.

## Executar

1. Configure a senha sem gravá-la no repositório:

```bash
dotnet user-secrets init
dotnet user-secrets set "ConnectionStrings:ClinicaConnection" "Server=...;Database=saep_Prova;User Id=sa;Password=SUA_SENHA;TrustServerCertificate=True;Encrypt=True"
```

2. Para testar localmente, o projeto usa SQLite automaticamente e não precisa de SQL Server:

```bash
dotnet run
```

O banco local é criado em `clinica-local.db`. Para usar o SQL Server da escola, altere
`DatabaseProvider` para `SqlServer` em `appsettings.json`, configure a senha via User Secrets
e execute `dotnet ef database update`.

Para usar o SQL Server, a senha deve estar configurada no armazenamento local de User Secrets:

```bash
dotnet user-secrets set "ConnectionStrings:ClinicaConnection" "Server=10.93.135.3\SQLExpress,1433;Database=saep_Prova;User Id=sa;Password=SUA_SENHA;TrustServerCertificate=True;Encrypt=True"
dotnet ef database update
```

Usuários de demonstração: `admin@clinicavet.local` / `Admin123!`, `recepcao@clinicavet.local` / `Recepcao123!` e `vet@clinicavet.local` / `Vet123!`.

## Funcionalidades

- Login com cookie, falha explícita, logout POST e expiração de 30 minutos.
- CPF protegido com ASP.NET Data Protection; nunca é exibido em telas ou na API.
- CRUD de tutores, pets associados obrigatoriamente a tutores e agendamentos associados a pets.
- Busca de tutores por nome/e-mail, ordenação de agenda e API autenticada em `/api/tutores`.

Não versionar `bin`, `obj`, User Secrets, senhas ou backups do banco.

Arquivos de entrega: `documentacao.pdf`, `der.png` e `saep_db.sql`.
