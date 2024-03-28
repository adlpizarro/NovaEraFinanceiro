using Incipiens.Base.GerenciadorEF;
using Incipiens.Base.GerenciadorEF.Funcoes;
using Incipiens.Base.GerenciadorEF.InterfacesEntityFramework;
using Incipiens.Modulos.Fornecedor.Object;
using Incipiens.Modulos.Pessoa.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Incipiens.Base.Model.Tipos;

namespace Incipiens.Modulos.Fornecedor.Database
{
    public class dbFornecedor : IListar<oFornecedor>, IBuscar<oFornecedor>, IGravar<oFornecedor>,IDeletar<oFornecedor>
    {
        public dbFornecedor()
        { 
            Ctx=new _Context();
        }
        _Context Ctx;

        #region IAttach
        public void Attach(oFornecedor obj, ImplementationContext ctx, EntityState entityState)
        {
            var fornecedor = obj;
            if (fornecedor._IdFornecedor < 0)
                fornecedor._IdFornecedor = 0;

            if (entityState == EntityState.Added)
                if (obj._DadosFornecedor._IdPessoa <= 0)
                    new dbPessoa().Attach(obj._DadosFornecedor, ctx, EntityState.Added);
                else
                    new dbPessoa().Attach(obj._DadosFornecedor, ctx, EntityState.Modified);
            else
                new dbPessoa().Attach(obj._DadosFornecedor, ctx, entityState);
            ctx.Entry(obj).State = entityState;
        }

        #endregion

        #region IListar

        public Expression<Func<oFornecedor, bool>> predicado { get; set; }

        public oOrderBy<oFornecedor> orderBy { get; set; }

        public IQueryable<oFornecedor> getQuery()
        {
            return from e in Ctx.dbFornecedor
                   select e;
        }

        public List<oFornecedor> Listar(int numeroInteracao = 1, int MaxPerLoad = ImplementationContext.MaxPerLoad)
        {
            return getQuery().LoadListFromDatabase(predicado, orderBy, (numeroInteracao - 1) * MaxPerLoad, numeroInteracao * MaxPerLoad).AsNoTracking().ToList();
        }

        public async Task<List<oFornecedor>> ListarAsync(int numeroInteracao = 1, int MaxPerLoad = ImplementationContext.MaxPerLoad)
        {
            return await getQuery().LoadListFromDatabase(predicado, orderBy, (numeroInteracao - 1) * MaxPerLoad, numeroInteracao * MaxPerLoad).AsNoTracking().ToListAsync();
        }

        public List<object> Listar(Expression<Func<oFornecedor, object>> properties, int numeroInteracao = 1, int MaxPerLoad = ImplementationContext.MaxPerLoad)
        {
            return getQuery().LoadListFromDatabase(predicado, orderBy, (numeroInteracao - 1) * MaxPerLoad, numeroInteracao * MaxPerLoad).Select(properties).AsNoTracking().ToList();
        }

        public async Task<List<object>> ListarAsync(Expression<Func<oFornecedor, object>> properties, int numeroInteracao = 1, int MaxPerLoad = ImplementationContext.MaxPerLoad)
        {
            return await getQuery().LoadListFromDatabase(predicado, orderBy, (numeroInteracao - 1) * MaxPerLoad, numeroInteracao * MaxPerLoad).Select(properties).AsNoTracking().ToListAsync();
        }

        #endregion

        #region IBuscar

        public oFornecedor Buscar(object Id)
        {
            if (!(Id is long))
                throw new ApplicationException("O Id precisa ser do tipo long");
            long _id = Convert.ToInt64(Id);
            oFornecedor fornecedor = null;
            using (_Context ctx = new _Context())
            {
                fornecedor = ctx.dbFornecedor.
                   Include(fuc => fuc._DadosFornecedor._Endereco._Municipio._EstadoFederal).
                   Include(fuc => fuc._DadosFornecedor._Celulares).
                   Include(fuc => fuc._DadosFornecedor._Emails).
                   Include(fuc => fuc._DadosFornecedor._Telefones).
                   SingleOrDefault(e => e._IdFornecedor == _id);

                ctx.Entry(fornecedor._DadosFornecedor).Collection(p => p._Contatos).Query()
                    .Include(c => c._DadosContato._Endereco._Municipio._EstadoFederal)
                    .Include(c => c._DadosContato._Telefones)
                    .Include(c => c._DadosContato._Celulares)
                    .Include(c => c._DadosContato._Emails)
                    .Load();
            }
            return fornecedor;
        }

        public oFornecedor Converter(oFornecedor objeto)
        {
            return objeto;
        }

        public IEnumerable<oFornecedor> Converter(IEnumerable<oFornecedor> lista)
        {
            return lista;
        }
        private oFornecedor BuscarPorIdPessoa(long IdPessoa)
        {
            oFornecedor fornecedor = null;
            using (_Context ctx = new _Context())
            {
                fornecedor = ctx.dbFornecedor.
                SingleOrDefault(e => e._IdPessoa == IdPessoa);
            }
            return fornecedor;
        }

        public static void BuscarPorIdPessoa(oFornecedor fornecedor)
        {
            if (fornecedor._DadosFornecedor._IdPessoa > 0)
            {
                var func = new dbFornecedor().BuscarPorIdPessoa(fornecedor._DadosFornecedor._IdPessoa);
                if (func != null)
                {
                    fornecedor._IdFornecedor = func._IdFornecedor;
                    fornecedor._IdPessoa = func._IdPessoa;
                    fornecedor._VersaoLinha = func._VersaoLinha;
                }
            }
        }
        #endregion

        #region IGravar
        public bool Salvar(oFornecedor fornecedor)
        {
            bool retorno = false;
            using (_Context ctx = new _Context())
            {
                if (fornecedor._IdPessoa <= 0)
                    Attach(fornecedor, ctx, EntityState.Added);
                else
                    Attach(fornecedor, ctx, EntityState.Modified);
                retorno = ctx.Salvar(fornecedor);
            }
            return retorno;
        }


        #endregion

        #region IDeletar

        public bool Deletar(oFornecedor Fornecedor)
        {
            using (_Context ctx = new _Context())
                return Deletar(Fornecedor, ctx);
        }

        public bool Deletar(object IdFornecedor)
        {
            using (_Context ctx = new _Context())
            {
                var Fornecedor = ctx.dbFornecedor.Single(e => e._IdFornecedor == (int)IdFornecedor);
                return Deletar(Fornecedor, ctx);
            }
        }

        public bool Deletar(oFornecedor Fornecedor, ImplementationContext ctx)
        {
            bool retorno;
            Attach(Fornecedor, ctx, EntityState.Deleted);
            if (!ctx.Salvar(Fornecedor))
                retorno = false;
            else
                retorno = true;
            return retorno;
        }

        #endregion
    }
}
