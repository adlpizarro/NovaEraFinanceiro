using Incipiens.Base.Model.Tipos;
using Incipiens.Base.GerenciadorEF;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System;
using Incipiens.Modulos.Funcionario.Database;
using Incipiens.Modulos.Funcionario.Object;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Internal;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Org.BouncyCastle.Asn1.Crmf;
using Incipiens.Modulos.Pessoa.Object;
using Microsoft.Extensions.Options;
using Incipiens.Base.GerenciadorEF.MigrationController;
using Incipiens.Modulos.Funcionario.Database.Properties;
using Incipiens.Base.Model.Modulos;

namespace Incipiens.Modulos.Funcionario.Database
{
    public class _Context : ImplementationContext
    {

        public _Context(): base()
        {
            
        }

        public DbSet<oFuncionario> dbFuncionario { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            OnModelCreatingFuncionario(modelBuilder);
        }

        public static void OnModelCreatingFuncionario(ModelBuilder modelBuilder)
        {
            Pessoa.Database._Context.OnModelCreatingPessoa(modelBuilder);

            modelBuilder.Entity<oFuncionario>(funcionario =>
            {
                funcionario
                    .Property(f => f._IdFuncionario)
                    .HasColumnName("IdFuncionario")
                    .IsRequired();


                funcionario
                    .Property(f => f._IdPessoa)
                    .HasColumnName("IdPessoa")
                    .IsRequired();

                funcionario.HasKey(f => f._IdFuncionario);

                funcionario
                    .HasOne<oPessoaFisica>(e => e._DadosFuncionario)
                    .WithOne()
                    .HasForeignKey<oFuncionario>(f => f._IdPessoa)
                    .OnDelete(DeleteBehavior.Restrict);

                funcionario.ToTable("Funcionario");
            });
        }

        #region Migration

        //Nunca Alterar
        public const string NomeModulo = "Funcionario";

        public override void getMigrations(Dictionary<string, List<oModulos>> dicMig)
        {
            new Pessoa.Database._Context().getMigrations(dicMig);

            List<oModulos> migrationControllers = new List<oModulos>();
            migrationControllers.Add(new oModulos
            {
                _ComandosAtualizacao = Resources.FuncionarioV1,
                _NomeModulo = NomeModulo,
                _Versao = 1,
            });

            if (!dicMig.ContainsKey(NomeModulo))
                dicMig.Add(NomeModulo, migrationControllers);
        }

        #endregion

    }
}

