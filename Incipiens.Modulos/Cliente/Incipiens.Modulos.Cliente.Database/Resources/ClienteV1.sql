CREATE TABLE  Cliente  (
     IdCliente int UNSIGNED NOT NULL AUTO_INCREMENT,
     IdPessoa  int UNSIGNED  NOT NULL,
     _VersaoLinha  int UNSIGNED NOT NULL,
    PRIMARY KEY ( IdCliente ),
    CONSTRAINT  FK_Cliente_Pessoa_IdPessoa  FOREIGN KEY ( IdPessoa ) REFERENCES  Pessoa  ( IdPessoa ) ON DELETE RESTRICT
);
CREATE UNIQUE INDEX IX_Cliente_IdPessoa ON Cliente (IdPessoa);