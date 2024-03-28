CREATE TABLE Pessoa (
    IdPessoa int UNSIGNED NOT NULL AUTO_INCREMENT,
    IdEndereco int UNSIGNED NULL,
    InscricaoEstadual varchar(15) NULL,
    Discriminator text NOT NULL,
    Cpf varchar(11) NULL unique,
    Rg varchar(15) NULL,
    Nome varchar(120) NULL,
    Apelido varchar(120) NULL,
    DataNascimento Date NULL,
    Cnpj varchar(14) NULL,
    RazaoSocial varchar(120) NULL,
    NomeFantasia varchar(120) NULL,
    _VersaoLinha int UNSIGNED NOT NULL,
    PRIMARY KEY (IdPessoa),
    CONSTRAINT FK_Pessoa_Endereco_IdEndereco FOREIGN KEY (IdEndereco) REFERENCES Endereco (IdEndereco) ON DELETE RESTRICT ON UPDATE CASCADE
);
CREATE TABLE Celular (
    PosicaoCelular int UNSIGNED NOT NULL,
    NumeroCelular varchar(11) NOT NULL,
    WhatsApp bit NOT NULL DEFAULT 0,
    EnviarCobrancas bit NOT NULL DEFAULT 0,
    EnviarDocs bit NOT NULL DEFAULT 0,
    Observacoes varchar(120) NULL,
    IdPessoa int UNSIGNED NOT NULL,
    _VersaoLinha int UNSIGNED NOT NULL,
    PRIMARY KEY (PosicaoCelular, IdPessoa),
    CONSTRAINT FK_Celular_Pessoa_IdPessoa FOREIGN KEY (IdPessoa) REFERENCES Pessoa (IdPessoa) ON DELETE RESTRICT ON UPDATE CASCADE
);
CREATE TABLE Contato (
    PosicaoContato int UNSIGNED NOT NULL,
    IdPessoaContato int UNSIGNED NOT NULL,
    CargoFuncao varchar(60) NULL,
    Parentesco varchar(60) NULL,
    Observacoes varchar(120) NULL,
    IdPessoa int UNSIGNED NOT NULL,
    _VersaoLinha int NOT NULL,
    PRIMARY KEY (PosicaoContato, IdPessoa),
    CONSTRAINT FK_Contato_Pessoa_IdPessoa FOREIGN KEY (IdPessoa) REFERENCES Pessoa (IdPessoa) ON DELETE RESTRICT,
    CONSTRAINT FK_Contato_Pessoa_IdPessoaContato FOREIGN KEY (IdPessoaContato) REFERENCES Pessoa (IdPessoa) ON DELETE RESTRICT ON UPDATE CASCADE
);
CREATE TABLE Email (
    PosicaoEmail int UNSIGNED NOT NULL,
    Email varchar(60) NOT NULL,
    EnviarCobrancas bit NOT NULL DEFAULT 0,
    EnviarDocs bit NOT NULL DEFAULT 0,
    Observacoes varchar(120) NULL,
    IdPessoa  int UNSIGNED NOT NULL,
    _VersaoLinha int UNSIGNED NOT NULL,
    PRIMARY KEY (PosicaoEmail, IdPessoa),
    CONSTRAINT FK_Email_Pessoa_IdPessoa FOREIGN KEY (IdPessoa) REFERENCES Pessoa (IdPessoa) ON DELETE RESTRICT ON UPDATE CASCADE
);
CREATE TABLE Telefone (
    PosicaoTelefone int UNSIGNED NOT NULL,
    NumeroTelefone varchar(10) NOT NULL,
    Observacoes varchar(120) NULL,
    IdPessoa int UNSIGNED NOT NULL,
    _VersaoLinha int UNSIGNED NOT NULL,
    PRIMARY KEY (PosicaoTelefone, IdPessoa),
    CONSTRAINT FK_Telefone_Pessoa_IdPessoa FOREIGN KEY (IdPessoa) REFERENCES Pessoa (IdPessoa) ON DELETE RESTRICT ON UPDATE CASCADE
);
CREATE INDEX IX_Celular_IdPessoa ON Celular (IdPessoa);
CREATE INDEX IX_Email_IdPessoa ON Email (IdPessoa);
CREATE UNIQUE INDEX IX_Pessoa_IdEndereco ON Pessoa (IdEndereco);
CREATE INDEX IX_Telefone_IdPessoa ON Telefone (IdPessoa);
CREATE UNIQUE INDEX IX_Pessoa_Cpf ON Pessoa (Cpf);
CREATE UNIQUE INDEX IX_Pessoa_Cnpj ON Pessoa (Cnpj);