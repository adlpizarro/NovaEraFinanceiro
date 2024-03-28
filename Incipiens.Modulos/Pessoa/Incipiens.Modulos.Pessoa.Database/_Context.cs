using Incipiens.Base.GerenciadorEF;
using Microsoft.EntityFrameworkCore;
using Incipiens.Modulos.Endereco.Object;
using System.Collections.Generic;
using System;
using Incipiens.Base.GerenciadorEF.MigrationController;
using Incipiens.Base.Model.Tipos;
using System.Data.Common;

using Incipiens.Base.Funcoes;

using Incipiens.Modulos.Pessoa.Object;
using Incipiens.Modulos.Pessoa.Object.Contato;
using Incipiens.Modulos.Pessoa.Database.Properties;
using Incipiens.Base.Model.Modulos;

namespace Incipiens.Modulos.Pessoa.Database
{
    public class _Context : ImplementationContext
    {
        public _Context()
        {
            
        }

        #region Pessoa

        public DbSet<oPessoa> dbPessoa { get; set; }
        public DbSet<oPessoaFisica> dbPessoaFisica { get; set; }
        public DbSet<oPessoaJuridica> dbPessoaJuridica { get; set; }

        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            OnModelCreatingPessoa(modelBuilder);
        }

        public static void OnModelCreatingPessoa(ModelBuilder modelBuilder)
        {
            Endereco.Database._Context.OnModelCreatingEndereco(modelBuilder);

            modelBuilder.Entity<oTelefone>(telefone => {
                telefone
                    .Property(t => t._PosicaoTelefone)
                    .HasColumnName("PosicaoTelefone")
                    .IsRequired();

                telefone
                    .Property(t => t._NumeroTelefone)
                    .HasColumnName("NumeroTelefone")
                    .IsRequired()
                    .HasMaxLength(10);

                telefone
                    .Property(t => t._Observacoes)
                    .HasColumnName("Observacoes")
                    .HasMaxLength(120);

                telefone
                    .HasKey(
                        informacoesPropriedade<oTelefone>.GetNameProperty(t => t._IdPessoa),
                        informacoesPropriedade<oTelefone>.GetNameProperty(t => t._PosicaoTelefone));

                telefone
                    .Property(t => t._IdPessoa)
                    .HasColumnName("IdPessoa")
                    .IsRequired();

                telefone
                    .HasOne<oPessoa>()
                    .WithMany(p => p._Telefones)
                    .HasForeignKey(t => t._IdPessoa)
                    .OnDelete(DeleteBehavior.Cascade);
                    

                telefone.ToTable("Telefone");
            });

            modelBuilder.Entity<oCelular>(celular =>
            {
                celular
                    .Property(c => c._PosicaoCelular)
                    .HasColumnName("PosicaoCelular")
                    .IsRequired();

                celular
                    .Property(c => c._NumeroCelular)
                    .HasColumnName("NumeroCelular")
                    .IsRequired()
                    .HasMaxLength(11);

                celular
                    .Property(c => c._WhatsApp)
                    .HasColumnName("WhatsApp")
                    .HasColumnType("bit")
                    .HasDefaultValueSql("0");

                celular
                    .Property(c => c._EnviarCobrancas)
                    .HasColumnName("EnviarCobrancas")
                    .HasColumnType("bit")
                    .HasDefaultValueSql("0");

                celular
                    .Property(c => c._EnviarDocs)
                    .HasColumnName("EnviarDocs")
                    .HasColumnType("bit")
                    .HasDefaultValueSql("0");

                celular
                    .Property(c => c._Observacoes)
                    .HasColumnName("Observacoes")
                    .HasMaxLength(120);

                celular
                    .Property(t => t._IdPessoa)
                    .HasColumnName("IdPessoa")
                    .IsRequired();
                
                celular
                 .HasKey(
                        informacoesPropriedade<oCelular>.GetNameProperty(t => t._IdPessoa),
                        informacoesPropriedade<oCelular>.GetNameProperty(t => t._PosicaoCelular));

                celular
                    .HasOne<oPessoa>(t => t._Pessoa)
                    .WithMany(p => p._Celulares)
                    .HasForeignKey(t => t._IdPessoa)
                    .OnDelete(DeleteBehavior.Restrict);

                celular.ToTable("Celular");

            });

            modelBuilder.Entity<oEmail>(email =>
            {
                email
                    .Property(c => c._PosicaoEmail)
                    .HasColumnName("PosicaoEmail")
                    .IsRequired();

                email
                    .Property(c => c._Email)
                    .HasColumnName("Email")
                    .IsRequired()
                    .HasMaxLength(60);

                email
                    .Property(c => c._EnviarCobrancas)
                    .HasColumnName("EnviarCobrancas")
                    .HasColumnType("bit")
                    .HasDefaultValueSql("0");

                email
                    .Property(c => c._EnviarDocs)
                    .HasColumnName("EnviarDocs")
                    .HasColumnType("bit")
                    .HasDefaultValueSql("0");

                email
                    .Property(c => c._Observacoes)
                    .HasColumnName("Observacoes")
                    .HasMaxLength(120);


                email
                    .Property(t => t._IdPessoa)
                    .HasColumnName("IdPessoa")
                    .IsRequired();

                email.HasKey(
                        informacoesPropriedade<oEmail>.GetNameProperty(t => t._IdPessoa),
                        informacoesPropriedade<oEmail>.GetNameProperty(t => t._PosicaoEmail));

                email
                    .HasOne<oPessoa>()
                    .WithMany(p => p._Emails)
                    .HasForeignKey(t => t._IdPessoa)
                    .OnDelete(DeleteBehavior.Restrict);

                email.ToTable("Email");

            });

            modelBuilder.Entity<oContato>(contato =>
            {
                contato
                    .Property(c => c._PosicaoContato)
                    .HasColumnName("PosicaoContato")
                    .IsRequired();

                contato
                    .Property(c => c._Parentesco)
                    .HasColumnName("Parentesco")
                    .HasMaxLength(60);

                contato
                    .Property(c => c._CargoFuncao)
                    .HasColumnName("CargoFuncao")
                    .HasMaxLength(60);

                contato
                    .Property(c => c._Observacoes)
                    .HasColumnName("Observacoes")
                    .HasMaxLength(120);

                contato
                    .Property(c => c._IdPessoa)
                    .HasColumnName("IdPessoa")
                    .IsRequired();

                contato
                    .Property(c => c._IdPessoaContato)
                    .HasColumnName("IdPessoaContato")
                    .IsRequired();

                contato.HasKey(
                    informacoesPropriedade<oContato>.GetNameProperty(t => t._IdPessoa),
                        informacoesPropriedade<oContato>.GetNameProperty(t => t._PosicaoContato));

                contato
                    .HasOne<oPessoa>()
                    .WithMany(p => p._Contatos)
                    .HasForeignKey(c => c._IdPessoa)
                    .OnDelete(DeleteBehavior.Restrict);

                contato
                    .HasOne<oPessoaFisica>(c => c._DadosContato)
                    .WithOne()
                    .HasForeignKey<oContato>(c => c._IdPessoaContato);

                contato.ToTable("Contato");

            });

            modelBuilder.Entity<oPessoa>(pessoa => {

                pessoa
                    .Property(p => p._IdPessoa)
                    .HasColumnName("IdPessoa")
                    .IsRequired();

                pessoa
                    .Property(p => p._InscricaoEstadual)
                    .HasColumnName("InscricaoEstadual")
                    .HasMaxLength(15);

                pessoa
                    .Property(p => p._IdEndereco)
                    .HasColumnName("IdEndereco");

                pessoa.HasKey(p => p._IdPessoa);

                pessoa
                    .HasOne<oEndereco>(p => p._Endereco)
                    .WithOne()
                    .HasForeignKey<oPessoa>(p => p._IdEndereco)
                    .OnDelete(DeleteBehavior.Restrict);

                pessoa.ToTable("Pessoa");
            });

            modelBuilder.Entity<oPessoaFisica>(fisica =>
            {
                fisica
                    .Property(f => f._Cpf)
                    .HasColumnName("Cpf")
                    .HasMaxLength(11);

                fisica
                    .Property(f => f._Rg)
                    .HasColumnName("Rg")
                    .HasMaxLength(15);

                fisica
                    .Property(f => f._Nome)
                    .HasColumnName("Nome")
                    .HasMaxLength(120);

                fisica
                    .Property(f => f._Apelido)
                    .HasColumnName("Apelido")
                    .HasMaxLength(120);

                fisica
                    .Property(f => f._DataNascimento)
                    .HasColumnName("DataNascimento")
                    .HasColumnType("Date");

                fisica.HasBaseType<oPessoa>();
            });

            modelBuilder.Entity<oPessoaJuridica>(juridica =>
            {
                juridica
                    .Property(f => f._Cnpj)
                    .HasColumnName("Cnpj")
                    .HasMaxLength(14);

                juridica
                    .Property(f => f._RazaoSocial)
                    .HasColumnName("RazaoSocial")
                    .HasMaxLength(120);

                juridica
                    .Property(f => f._NomeFantasia)
                    .HasColumnName("NomeFantasia")
                    .HasMaxLength(120);

                juridica.HasBaseType<oPessoa>();
            });
        }

        #region Migration

        //Nunca Alterar
        public const string NomeModulo = "Pessoa";

        public override void getMigrations(Dictionary<string, List<oModulos>> dicMig)
        {
            List<oModulos> migrationControllers = new List<oModulos>();

            new Endereco.Database._Context().getMigrations(dicMig);

            migrationControllers.Add(new oModulos
            {
                _ComandosAtualizacao = Resources.PessoaV1,
                _NomeModulo = NomeModulo,
                _Versao = 1,
            });
            if (!dicMig.ContainsKey(NomeModulo))
                dicMig.Add(NomeModulo, migrationControllers);
        }

        #endregion
    }
}
