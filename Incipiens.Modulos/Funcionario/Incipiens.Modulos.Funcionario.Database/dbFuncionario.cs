using Incipiens.Base.GerenciadorEF;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using Incipiens.Modulos.Funcionario.Object;
using Incipiens.Base.GerenciadorEF.InterfacesEntityFramework;
using System.Runtime.Serialization;
using System.Runtime.InteropServices.ComTypes;
using Org.BouncyCastle.Security.Certificates;
using Org.BouncyCastle.Asn1.X509;
using System.Collections;
using System.Timers;
using Microsoft.EntityFrameworkCore.Internal;
using Incipiens.Modulos.Pessoa.Object;
using Incipiens.Modulos.Pessoa.Database;

using Incipiens.Base.Funcoes;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Diagnostics.Contracts;
using Incipiens.Base.GerenciadorEF.Funcoes;
using Incipiens.Base.Model.Tipos;

namespace Incipiens.Modulos.Funcionario.Database
{
   public class dbFuncionario: IListar<oFuncionario>, IBuscar<oFuncionario>, IGravar<oFuncionario>,IDeletar<oFuncionario>
    {
        public dbFuncionario()
        {

            Ctx = new _Context();
        }
        _Context Ctx;

        #region IListar

        public Expression<Func<oFuncionario, bool>> predicado { get; set; }

        public oOrderBy<oFuncionario> orderBy { get; set; }

        public IQueryable<oFuncionario> getQuery()
        {
            return from e in Ctx.dbFuncionario
                   select e;
        }


        public List<oFuncionario> Listar(int numeroInteracao = 1, int MaxPerLoad = ImplementationContext.MaxPerLoad)
        {
            return getQuery().LoadListFromDatabase(predicado, orderBy, (numeroInteracao - 1) * MaxPerLoad, numeroInteracao * MaxPerLoad).AsNoTracking().ToList();
        }

        public async Task<List<oFuncionario>> ListarAsync(int numeroInteracao = 1, int MaxPerLoad = ImplementationContext.MaxPerLoad)
        {
            return await getQuery().LoadListFromDatabase(predicado, orderBy, (numeroInteracao - 1) * MaxPerLoad, numeroInteracao * MaxPerLoad).AsNoTracking().ToListAsync();
        }

        public List<object> Listar(Expression<Func<oFuncionario, object>> properties, int numeroInteracao = 1, int MaxPerLoad = ImplementationContext.MaxPerLoad)
        {
            return getQuery().LoadListFromDatabase(predicado, orderBy, (numeroInteracao - 1) * MaxPerLoad, numeroInteracao * MaxPerLoad).Select(properties).AsNoTracking().ToList();
        }

        public async Task<List<object>> ListarAsync(Expression<Func<oFuncionario, object>> properties, int numeroInteracao = 1, int MaxPerLoad = ImplementationContext.MaxPerLoad)
        {
            return await getQuery().LoadListFromDatabase(predicado, orderBy, (numeroInteracao - 1) * MaxPerLoad, numeroInteracao * MaxPerLoad).Select(properties).AsNoTracking().ToListAsync();
        }
        #endregion

        #region IBuscar

        public oFuncionario Buscar(object Id)
        {
            oFuncionario funcionario = null;
            long _id = Convert.ToInt64(Id);
            using (_Context ctx = new _Context())
            {
                    funcionario = ctx.dbFuncionario.
                    Include(fuc => fuc._DadosFuncionario._Endereco._Municipio._EstadoFederal).
                    Include(fuc => fuc._DadosFuncionario._Celulares).
                    Include(fuc => fuc._DadosFuncionario._Emails).
                    Include(fuc => fuc._DadosFuncionario._Telefones).
                    Include(fuc => fuc._DadosFuncionario._Contatos)
                        .ThenInclude(cont => cont._DadosContato._Endereco._Municipio._EstadoFederal)
                    .SingleOrDefault(e => e._IdFuncionario == _id);
            }


            return funcionario;
        }

        public oFuncionario Converter(oFuncionario objeto)
        {
            return objeto;
        }

        public IEnumerable<oFuncionario> Converter(IEnumerable<oFuncionario> lista)
        {
            return lista;
        }

        public oFuncionario BuscarPorIdPessoa(long IdPessoa)
        {
            oFuncionario funcionario = null;
            using (_Context ctx = new _Context())
            {
                funcionario = ctx.dbFuncionario.
                SingleOrDefault(e => e._IdPessoa == IdPessoa);
            }
            return funcionario;
        }

        public static void BuscarPorIdPessoa(oFuncionario funcionario)
        {
            if (funcionario._DadosFuncionario._IdPessoa > 0)
            {
                var func = new dbFuncionario().BuscarPorIdPessoa(funcionario._DadosFuncionario._IdPessoa);
                if (func != null)
                {
                    funcionario._IdFuncionario = func._IdFuncionario;
                    funcionario._IdPessoa = func._IdPessoa;
                    funcionario._VersaoLinha = func._VersaoLinha;
                }
            }
        }

        #endregion

        #region IGravar

        public bool Salvar(oFuncionario funcionario)
        {
            bool retorno;
            using (_Context ctx = new _Context())
            {
                if (funcionario._IdFuncionario <= 0)
                    Attach(funcionario, ctx, EntityState.Added);
                else
                    Attach(funcionario, ctx, EntityState.Modified);
                retorno = ctx.Salvar(funcionario);

            }
            return retorno;

        }

       

        #endregion

        #region IAttach
        public void Attach(oFuncionario funcionario, ImplementationContext ctx, EntityState entityState)
        {
            if (funcionario._IdFuncionario < 0)
                funcionario._IdFuncionario = 0;

            if(entityState == EntityState.Added)
                if(funcionario._DadosFuncionario._IdPessoa <= 0)
                    new dbPessoa().Attach(funcionario._DadosFuncionario, ctx, EntityState.Added);
                else
                    new dbPessoa().Attach(funcionario._DadosFuncionario, ctx, EntityState.Modified);
            else
                new dbPessoa().Attach(funcionario._DadosFuncionario, ctx, entityState);
            ctx.Entry(funcionario).State = entityState;
        }


        #endregion

        #region IDeletar

        public bool Deletar(oFuncionario funcionario)
        {
            using (_Context ctx = new _Context())
                return Deletar(funcionario, ctx);
        }

        public bool Deletar(object Idfuncionario)
        {
            using (_Context ctx = new _Context())
            {
                var funcionario = ctx.dbFuncionario.Single(e => e._IdFuncionario == (int)Idfuncionario);
                return Deletar(funcionario, ctx);
            }
        }

        public bool Deletar(oFuncionario funcionario, ImplementationContext ctx)
        {
            bool retorno;
            Attach(funcionario, ctx, EntityState.Deleted);
            if (!ctx.Salvar(funcionario))
                retorno = false;
            else
                retorno = true;
            return retorno;
        }

        #endregion
    }
}
