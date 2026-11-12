IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261112172810_CriacaoInicial'
)
BEGIN
    CREATE TABLE [Tutores] (
        [Id] int NOT NULL IDENTITY,
        [Nome] nvarchar(100) NOT NULL,
        [Email] nvarchar(160) NOT NULL,
        [CpfCriptografado] nvarchar(500) NOT NULL,
        [Telefone] nvarchar(20) NULL,
        CONSTRAINT [PK_Tutores] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261112172810_CriacaoInicial'
)
BEGIN
    CREATE TABLE [Usuarios] (
        [Id] int NOT NULL IDENTITY,
        [Nome] nvarchar(80) NOT NULL,
        [Email] nvarchar(160) NOT NULL,
        [SenhaHash] nvarchar(max) NOT NULL,
        CONSTRAINT [PK_Usuarios] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261112172810_CriacaoInicial'
)
BEGIN
    CREATE TABLE [Pets] (
        [Id] int NOT NULL IDENTITY,
        [Nome] nvarchar(80) NOT NULL,
        [Especie] nvarchar(50) NULL,
        [DataNascimento] date NULL,
        [TutorId] int NOT NULL,
        CONSTRAINT [PK_Pets] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Pets_Tutores_TutorId] FOREIGN KEY ([TutorId]) REFERENCES [Tutores] ([Id]) ON DELETE NO ACTION
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261112172810_CriacaoInicial'
)
BEGIN
    CREATE TABLE [Agendamentos] (
        [Id] int NOT NULL IDENTITY,
        [DataHora] datetime2 NOT NULL,
        [Servico] nvarchar(300) NOT NULL,
        [PetId] int NOT NULL,
        CONSTRAINT [PK_Agendamentos] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Agendamentos_Pets_PetId] FOREIGN KEY ([PetId]) REFERENCES [Pets] ([Id]) ON DELETE NO ACTION
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261112172810_CriacaoInicial'
)
BEGIN
    CREATE INDEX [IX_Agendamentos_PetId] ON [Agendamentos] ([PetId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261112172810_CriacaoInicial'
)
BEGIN
    CREATE INDEX [IX_Pets_TutorId] ON [Pets] ([TutorId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261112172810_CriacaoInicial'
)
BEGIN
    CREATE INDEX [IX_Tutores_Email] ON [Tutores] ([Email]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261112172810_CriacaoInicial'
)
BEGIN
    CREATE UNIQUE INDEX [IX_Usuarios_Email] ON [Usuarios] ([Email]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261112172810_CriacaoInicial'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20261112172810_CriacaoInicial', N'10.0.15');
END;

COMMIT;
GO

-- Dados demonstrativos protegidos e hashes de senha.
IF NOT EXISTS (SELECT 1 FROM [Usuarios])
BEGIN
    SET IDENTITY_INSERT [Usuarios] ON;
    INSERT INTO [Usuarios] ([Id], [Nome], [Email], [SenhaHash]) VALUES
    (1, N'Administrador', N'admin@clinicavet.local', N'AQAAAAAAAYagAAAAIK9M6hl7r7yeKB2/pWt1pvruPZ/nw4ePrgKh73T3J/1XJrK4fdt3OUMOwpnptrTGiA=='),
    (2, N'Recepção', N'recepcao@clinicavet.local', N'AQAAAAAAAYagAAAAIBDn8GBtvKvlO8C5XD58Ua2O7MiGbiDsuLuB8A5bG4Wwr3CXiXp2DPShtpyExQetBQ=='),
    (3, N'Veterinário', N'vet@clinicavet.local', N'AQAAAAAAAYagAAAAIFUBrq537vlZL1y4F64Z1iyP8LdYBFcGc8qIQNZz4G3JefDFTBvPsJKcjhlFzwC5iQ==');
    SET IDENTITY_INSERT [Usuarios] OFF;
END;

IF NOT EXISTS (SELECT 1 FROM [Tutores])
BEGIN
    SET IDENTITY_INSERT [Tutores] ON;
    INSERT INTO [Tutores] ([Id], [Nome], [Email], [CpfCriptografado], [Telefone]) VALUES
    (1, N'Ana Souza', N'ana@example.com', N'CfDJ8EZtvrWqcPNDnAXaFgSW8wqaFz5FmPFVEHYWRuELjlJDuEVakOg-cKqlI7MhZivijfottjtpfXovp9l4DRdwoQ7utZ7D4G-WXEkxT9fI8K8bizb0_54GMmoupqfCQrkuug', N'(11) 99999-0001'),
    (2, N'Bruno Lima', N'bruno@example.com', N'CfDJ8EZtvrWqcPNDnAXaFgSW8wpMEp3B8ef9YUMFOkB3jTyLEGJmn4nKFQlpf30Cmvti4Dz3W2XW3LA3hWtfzGmxK8Ak5t5l3M_3bszMDtThqcf2aPuDZASUe-UNSmDBrVZEtQ', N'(11) 99999-0002'),
    (3, N'Carla Mendes', N'carla@example.com', N'CfDJ8EZtvrWqcPNDnAXaFgSW8wpuXScKzHaWEkayldOEPHFWhA1rrq9-04H3VdbbB3_kYPZEUGRWGfqV8XHSWXj5ixyVj53DMHenwgUFYeNTy1IGTs1-4wDHvyoYVri-LbUp1A', N'(11) 99999-0003');
    SET IDENTITY_INSERT [Tutores] OFF;
END;

IF NOT EXISTS (SELECT 1 FROM [Pets])
BEGIN
    SET IDENTITY_INSERT [Pets] ON;
    INSERT INTO [Pets] ([Id], [Nome], [Especie], [DataNascimento], [TutorId]) VALUES
    (1, N'Mel', N'Cachorro', '2020-05-10', 1),
    (2, N'Nina', N'Gato', '2021-08-02', 2),
    (3, N'Thor', N'Cachorro', '2019-02-20', 3);
    SET IDENTITY_INSERT [Pets] OFF;
END;

IF NOT EXISTS (SELECT 1 FROM [Agendamentos])
BEGIN
    SET IDENTITY_INSERT [Agendamentos] ON;
    INSERT INTO [Agendamentos] ([Id], [DataHora], [Servico], [PetId]) VALUES
    (1, DATEADD(hour, 9, DATEADD(day, 1, CAST(GETDATE() AS date))), N'Consulta clínica', 1),
    (2, DATEADD(hour, 14, DATEADD(day, 2, CAST(GETDATE() AS date))), N'Vacinação', 2),
    (3, DATEADD(hour, 11, DATEADD(day, 3, CAST(GETDATE() AS date))), N'Retorno', 3);
    SET IDENTITY_INSERT [Agendamentos] OFF;
END;
GO
