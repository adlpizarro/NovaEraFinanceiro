CREATE TABLE  Funcionario  (
     IdFuncionario int UNSIGNED NOT NULL AUTO_INCREMENT,
     IdPessoa  int UNSIGNED  NOT NULL,
     _VersaoLinha  int UNSIGNED NOT NULL,
    PRIMARY KEY ( IdFuncionario ),
    CONSTRAINT  FK_Funcionario_Pessoa_IdPessoa  FOREIGN KEY ( IdPessoa ) REFERENCES  Pessoa  ( IdPessoa ) ON DELETE RESTRICT
);
CREATE UNIQUE INDEX IX_Funcionario_IdPessoa ON Funcionario (IdPessoa);