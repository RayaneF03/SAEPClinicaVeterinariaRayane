# ClinicaVet

Sistema ASP.NET Core MVC + API para tutores, pets e agendamentos.

## Executar

1. Configure a senha sem gravá-la no repositório:

```bash
dotnet user-secrets init
dotnet user-secrets set "ConnectionStrings:ClinicaConnection" "Server=...;Database=saep_Prova;User Id=sa;Password=SUA_SENHA;TrustServerCertificate=True;Encrypt=True"
```

2. Crie/aplique o banco e execute:

```bash
dotnet ef database update
dotnet run
```

Para aplicar o banco, a senha deve estar configurada no armazenamento local de User Secrets:

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
