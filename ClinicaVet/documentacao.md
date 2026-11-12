# Documentação - ClinicaVet

## Objetivo
Controlar tutores, pets e agendamentos, reduzindo perdas e duplicidades.

## Requisitos funcionais
- **RF01:** autenticar usuário, validar falha e expirar sessão.
- **RF02:** cadastrar, listar, buscar e editar tutores.
- **RF03:** cadastrar e editar pets vinculados a tutor.
- **RF04:** listar agendamentos por data, com pet e tutor, e editar a data.

## Regras de negócio
- **RN01:** CPF é cifrado antes de persistir.
- **RN02:** o seed cria ao menos três registros em cada tabela.
- **RN03:** pet exige tutor e agendamento exige pet.

## Tecnologias
.NET 10, ASP.NET Core MVC, Entity Framework Core, SQL Server e VS Code.

## DER
`der.svg` representa as tabelas, chaves e relacionamentos do sistema.
