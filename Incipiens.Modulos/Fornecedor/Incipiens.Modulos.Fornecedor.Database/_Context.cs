using Incipiens.Base.GerenciadorEF;
using Incipiens.Modulos.Fornecedor.Object;
using Incipiens.Modulos.Pessoa.Database;
using Incipiens.Modulos.Pessoa.Object;
using Microsoft.EntityFrameworkCore;
using System;
using Incipiens.Base.GerenciadorEF.MigrationController;
using Incipiens.Base.Model.Tipos;

using Microsoft.EntityFrameworkCore.Query.Internal;
using System.Collections.Generic;
using Incipiens.Modulos.Fornecedor.Database.Properties;
using Incipiens.Base.Model.Modulos;

namespace Incipiens.Modulos.Fornecedor.Database
{
    public class _Context : ImplementationContext
    {
        public _Context() : base()
        {

        }
        public DbSet<oFornecedor> dbFornecedor { get; set; }
        public DbSet<oPessoa> dbPessoa { get; set; }
        public DbSet<oPessoaFisica> dbPessoaFisica { get; set; }
        public DbSet<oPessoaJuridica> dbPessoaJuridica { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            OnModelCreatingFornecedor(modelBuilder);
        }
        public static void OnModelCreatingFornecedor(ModelBuilder modelBuilder)
        {
            Pessoa.Database._Context.OnModelCreatingPessoa(modelBuilder);
            modelBuilder.Entity<oFornecedor>(fornecedor =>
            {
                fornecedor
                    .Property(f => f._IdFornecedor)
                    .HasColumnName("IdFornecedor")
                    .IsRequired();


                fornecedor
                    .Property(f => f._IdPessoa)
                    .HasColumnName("IdPessoa")
                    .IsRequired();

                fornecedor.HasKey(f => f._IdFornecedor);

                fornecedor
                    .HasOne<oPessoa>(e => e._DadosFornecedor)
                    .WithOne()
                    .HasForeignKey<oFornecedor>(f => f._IdPessoa)
                    .OnDelete(DeleteBehavior.Restrict);

                fornecedor.ToTable("fornecedor");
            });
        }

        #region Migration

        //Nunca Alterar
        public const string NomeModulo = "Fornecedor";

        public override void getMigrations(Dictionary<string, List<oModulos>> dicMig)
        {
            new Pessoa.Database._Context().getMigrations(dicMig);

            List<oModulos> migrationControllers = new List<oModulos>();
            migrationControllers.Add(new oModulos
            {
                _ComandosAtualizacao = Resources.FornecedorV1,
                _NomeModulo = NomeModulo,
                _Versao = 1,
            });

            if (!dicMig.ContainsKey(NomeModulo))
                dicMig.Add(NomeModulo, migrationControllers);
        }

        #endregion
    }
}
