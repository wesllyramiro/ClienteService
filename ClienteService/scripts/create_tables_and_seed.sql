
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Clientes]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[Clientes](
        [Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        [Nome] NVARCHAR(200) NOT NULL,
        [Email] NVARCHAR(200) NULL,
        [Telefone] NVARCHAR(50) NULL
    );
END

IF NOT EXISTS (SELECT 1 FROM dbo.Clientes)
BEGIN
    INSERT INTO dbo.Clientes (Nome, Email, Telefone) VALUES
    (N'João Silva', 'joao.silva@example.com', '11999990000'),
    (N'Maria Oliveira', 'maria.oliveira@example.com', '11988881111');
END