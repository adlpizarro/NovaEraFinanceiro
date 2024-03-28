using Microsoft.EntityFrameworkCore;
using Incipiens.Modulos.Endereco.Object;
using System.Collections.Generic;
using System;

using Incipiens.Base.GerenciadorEF.MigrationController;
using Incipiens.Base.Model.Tipos;
using Incipiens.Base.GerenciadorEF;

using Incipiens.Modulos.Pessoa.Object;

using Incipiens.Modulos.Cliente.Object;
using Incipiens.Modulos.Cliente.Database.Properties;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Incipiens.Base.Model.Modulos;

namespace Incipiens.Modulos.Cliente.Database
{
    public class _Context: ImplementationContext
    {
        public _Context():base()
        {
            
        }

        public DbSet<oCliente> dbCliente { get; set; }
        public DbSet<oPessoa> dbPessoa { get; set; }
        public DbSet<oPessoaFisica> dbPessoaFisica { get; set; }
        public DbSet<oPessoaJuridica> dbPessoaJuridica { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            OnModelCreatingCliente(modelBuilder);
        }

        public static void OnModelCreatingCliente(ModelBuilder modelBuilder)
        {
            Pessoa.Database._Context.OnModelCreatingPessoa(modelBuilder);
            modelBuilder.Entity<oCliente>(cliente =>
            {
                cliente
                    .Property(f => f._IdCliente)
                    .HasColumnName("IdCliente")
                    .IsRequired();


                cliente
                    .Property(f => f._IdPessoa)
                    .HasColumnName("IdPessoa")
                    .IsRequired();

                cliente.HasKey(f => f._IdCliente);

                cliente
                    .HasOne<oPessoa>(e => e._DadosCliente)
                    .WithOne()
                    .HasForeignKey<oCliente>(f => f._IdPessoa)
                    .OnDelete(DeleteBehavior.Restrict);

                cliente.ToTable("Cliente");
            });
        }

        #region Migration

        //Nunca Alterar
        public const string NomeModulo = "Cliente";

        public override void getMigrations(Dictionary<string, List<oModulos>> dicMig)
        {
            new Pessoa.Database._Context().getMigrations(dicMig);

            List<oModulos> migrationControllers = new List<oModulos>();
            migrationControllers.Add(new oModulos
            {
                _ComandosAtualizacao = Resources.ClienteV1,
                _NomeModulo = NomeModulo,
                _Versao = 1,
            });

            if (!dicMig.ContainsKey(NomeModulo))
                dicMig.Add(NomeModulo, migrationControllers);
        }

        #endregion        
    }
}
