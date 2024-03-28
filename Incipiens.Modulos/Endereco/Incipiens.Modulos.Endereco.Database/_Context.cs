using Incipiens.Base.GerenciadorEF;
using Microsoft.EntityFrameworkCore;
using Incipiens.Modulos.Endereco.Object;
using System.Collections.Generic;
using System;
using Incipiens.Base.GerenciadorEF.MigrationController;
using Incipiens.Base.Model.Tipos;
using Incipiens.Modulos.Endereco.Database.Properties;
using MySql.Data.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Incipiens.Base.Model.Modulos;

namespace Incipiens.Modulos.Endereco.Database
{
    public class _Context : ImplementationContext
    {
        public _Context()
        {
            
        }

        #region Endereco

        public DbSet<oEndereco> dbEnderecos { get; set; }
        public DbSet<oMunicipio> dbMunicipios { get; set; }
        public DbSet<oEstadoFederal> dbEstadosFederais { get; set; }

        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            OnModelCreatingEndereco(modelBuilder);
        }

        public static void OnModelCreatingEndereco(ModelBuilder modelBuilder)
        {
            //Login.Database._Context.OnModelCreatingLogin(modelBuilder);

            modelBuilder.Entity<oEstadoFederal>(estado =>
            {
                estado
                    .Property(e => e._IdEstado)
                    .HasColumnName("IdEstado")
                    .IsRequired()
                    .HasMaxLength(2);

                estado
                    .Property(e => e._Nome)
                    .HasColumnName("Nome")
                    .HasMaxLength(60)
                    .IsRequired();

                estado
                    .Property(e => e._Uf)
                    .HasColumnName("Uf")
                    .HasMaxLength(2)
                    .IsRequired();

                estado
                    .HasMany(e => e._Municipios)
                    .WithOne(m => m._EstadoFederal)
                    .HasForeignKey(m => m._IdEstado)
                    .OnDelete(DeleteBehavior.Restrict);

                estado
                    .HasKey(e => e._IdEstado);

                estado.ToTable("EstadoFederal");
            });

            modelBuilder.Entity<oMunicipio>(municipio =>
            {
                municipio
                    .Property(m => m._IdMunicipio)
                    .HasColumnName("IdMunicipio")
                    .HasMaxLength(8)
                    .IsRequired();

                municipio
                    .Property(m => m._Nome)
                    .HasColumnName("Nome")
                    .HasMaxLength(120)
                    .IsRequired();

                municipio
                    .Property(m => m._IdEstado)
                    .HasColumnName("IdEstado")
                    .HasMaxLength(2)
                    .IsRequired();

                municipio
                    .HasKey(m => m._IdMunicipio);

                municipio.ToTable("Municipio");

                municipio
                    .HasOne<oEstadoFederal>(m => m._EstadoFederal)
                    .WithMany(e => e._Municipios)
                    .HasForeignKey(m => m._IdEstado)
                    .OnDelete(DeleteBehavior.Restrict)
                    .IsRequired();
            });

            modelBuilder.Entity<oEndereco>(endereco =>
            {

                endereco
                   .Property(e => e._IdEndereco)
                   .HasColumnName("IdEndereco")
                   .IsRequired();

                endereco
                       .Property(e => e._Bairro)
                       .HasColumnName("Bairro")
                       .HasMaxLength(60)
                       .IsRequired();

                endereco
                       .Property(e => e._Cep)
                       .HasColumnName("Cep")
                       .HasMaxLength(8);

                endereco
                       .Property(e => e._Complemento)
                       .HasColumnName("Complemento")
                       .HasMaxLength(60);

                endereco
                       .Property(e => e._Logradouro)
                       .HasColumnName("Logradouro")
                       .HasMaxLength(120)
                       .IsRequired();

                endereco
                       .Property(e => e._Numero)
                       .HasColumnName("Numero")
                       .HasMaxLength(60)
                       .IsRequired();

                endereco
                    .Property(e => e._IdMunicipio)
                    .HasColumnName("IdMunicipio")
                    .HasMaxLength(8)
                    .IsRequired();

                endereco
                    .Property(e => e._VersaoLinha)
                    .IsRequired();
               
                endereco
                    .HasKey(e => e._IdEndereco);

                endereco
                    .HasOne<oMunicipio>(e => e._Municipio)
                    .WithMany()
                    .HasForeignKey(e => e._IdMunicipio)
                    .OnDelete(DeleteBehavior.Restrict);
                

                endereco.ToTable("Endereco");
            });
        }

        #region Migration

        //Nunca alterar
        public const string NomeModulo = "Endereco";

        public override void getMigrations(Dictionary<string, List<oModulos>> dicMig)
        {
            List<oModulos> migrationControllers = new List<oModulos>();

            //new Login.Database._Context().getMigrations(dicMig);
            
            migrationControllers.Add(new oModulos
            {
                _ComandosAtualizacao = Resources.EnderecoV1,
                _NomeModulo = NomeModulo,
                _Versao = 1,
            });

            migrationControllers.Add(new oModulos
            {
                _ComandosAtualizacao = Resources.EnderecoV2,
                _NomeModulo = NomeModulo,
                _Versao = 2,
            });

            if (!dicMig.ContainsKey(NomeModulo))
                dicMig.Add(NomeModulo, migrationControllers);
        }

        #endregion
    }
}
