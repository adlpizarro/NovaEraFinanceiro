CREATE TABLE  Fornecedor  (
     IdFornecedor int UNSIGNED NOT NULL AUTO_INCREMENT,
     IdPessoa  int UNSIGNED  NOT NULL,
     _VersaoLinha  int UNSIGNED NOT NULL,
    PRIMARY KEY ( IdFornecedor ),
    CONSTRAINT  FK_Fornecedor_Pessoa_IdPessoa  FOREIGN KEY ( IdPessoa ) REFERENCES  Pessoa  ( IdPessoa ) ON DELETE RESTRICT
);
CREATE UNIQUE INDEX IX_Fornecedor_IdPessoa ON Fornecedor (IdPessoa);