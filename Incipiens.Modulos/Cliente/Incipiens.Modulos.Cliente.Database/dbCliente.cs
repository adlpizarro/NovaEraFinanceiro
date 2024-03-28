using Incipiens.Base.GerenciadorEF;
using Incipiens.Base.GerenciadorEF.InterfacesEntityFramework;
using Incipiens.Modulos.Cliente.Object;
using Incipiens.Modulos.Pessoa.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Incipiens.Base.GerenciadorEF.Funcoes;
using System.Threading.Tasks;
using System.ComponentModel;
using Incipiens.Modulos.Endereco.Database;
using Incipiens.Base.Model.Tipos;

namespace Incipiens.Modulos.Cliente.Database
{
    [DisplayName("Clientes")]
    public class dbCliente : IListar<oCliente>, IBuscar<oCliente>, IGravar<oCliente>,IDeletar<oCliente>
    {
        public dbCliente()
        {
            Ctx=new _Context();
        }
       
        #region IAttach
        public void Attach(oCliente obj, ImplementationContext ctx, EntityState entityState)
        {
            if (obj._IdCliente < 0)
                obj._IdCliente = 0;
            if (entityState == EntityState.Added)
                if (obj._DadosCliente._IdPessoa <= 0)
                    new dbPessoa().Attach(obj._DadosCliente, ctx, EntityState.Added);
                else
                    new dbPessoa().Attach(obj._DadosCliente, ctx, EntityState.Modified);
            else
                new dbPessoa().Attach(obj._DadosCliente, ctx, entityState);
            ctx.Entry(obj).State = entityState;
        }

        #endregion

        #region IListar

        _Context Ctx;
        public Expression<Func<oCliente, bool>> predicado { get; set; }

        public oOrderBy<oCliente> orderBy { get; set; }

        public IQueryable<oCliente> getQuery()
        {
            return from e in Ctx.dbCliente
                   select e;
        }

        public List<oCliente> Listar(int numeroInteracao = 1, int MaxPerLoad = ImplementationContext.MaxPerLoad)
        {
            return getQuery().LoadListFromDatabase(predicado, orderBy, (numeroInteracao - 1) * MaxPerLoad, numeroInteracao * MaxPerLoad).AsNoTracking().ToList();
        }

        public async Task<List<oCliente>> ListarAsync(int numeroInteracao = 1, int MaxPerLoad = ImplementationContext.MaxPerLoad)
        {
            return await getQuery().LoadListFromDatabase(predicado, orderBy, (numeroInteracao - 1) * MaxPerLoad, numeroInteracao * MaxPerLoad).AsNoTracking().ToListAsync();
        }

        public List<object> Listar(Expression<Func<oCliente, object>> properties, int numeroInteracao = 1, int MaxPerLoad = ImplementationContext.MaxPerLoad)
        {
            return getQuery().LoadListFromDatabase(predicado, orderBy, (numeroInteracao - 1) * MaxPerLoad, numeroInteracao * MaxPerLoad).Select(properties).AsNoTracking().ToList();
        }

        public async Task<List<object>> ListarAsync(Expression<Func<oCliente, object>> properties, int numeroInteracao = 1, int MaxPerLoad = ImplementationContext.MaxPerLoad)
        {
            return await getQuery().LoadListFromDatabase(predicado, orderBy, (numeroInteracao - 1) * MaxPerLoad, numeroInteracao * MaxPerLoad).Select(properties).AsNoTracking().ToListAsync();
        }

        public async Task<List<object>> ListarAsync(Expression<Func<oCliente, object>> properties, int take = 1)
        {
            return await getQuery().LoadListFromDatabase(predicado, orderBy, 0, take).Select(properties).AsNoTracking().ToListAsync();
        }

        #endregion

        #region IBuscar

        public oCliente Buscar(object Id)
        {
            if (!(Id is long))
                throw new ApplicationException("Id precisa ser do tipo longo");
            long _id = Convert.ToInt64(Id);
            oCliente cliente = null;
            using (_Context ctx = new _Context())
            {
                
                    cliente = ctx.dbCliente.
                    Include(cli => cli._DadosCliente._Endereco._Municipio._EstadoFederal).
                    Include(cli => cli._DadosCliente._Celulares).
                    Include(cli => cli._DadosCliente._Emails).
                    Include(cli => cli._DadosCliente._Telefones).
                    SingleOrDefault(e => e._IdCliente == _id);

                    ctx.Entry(cliente._DadosCliente).Collection(p => p._Contatos).Query()
                        .Include(c => c._DadosContato._Endereco._Municipio._EstadoFederal)
                        .Include(c => c._DadosContato._Telefones)
                        .Include(c => c._DadosContato._Celulares)
                        .Include(c => c._DadosContato._Emails)
                        .Load();
            }
            return cliente;
        }


        public oCliente Converter(oCliente objeto)
        {
            return objeto;
        }

        public IEnumerable<oCliente> Converter(IEnumerable<oCliente> lista)
        {
            return lista;
        }
        public oCliente BuscarPorIdPessoa(long IdPessoa)
        {
            oCliente cliente = null;
            using (_Context ctx = new _Context())
            {
                cliente = ctx.dbCliente.
                SingleOrDefault(e => e._IdPessoa == IdPessoa);
            }
            return cliente;
        }

        public static void BuscarPorIdPessoa(oCliente cliente)
        {
            if (cliente._DadosCliente._IdPessoa > 0)
            {
                var func = new dbCliente().BuscarPorIdPessoa(cliente._DadosCliente._IdPessoa);
                if (func != null)
                {
                    cliente._IdCliente = func._IdCliente;
                    cliente._IdPessoa = func._IdPessoa;
                    cliente._VersaoLinha = func._VersaoLinha;
                }
            }
        }
        #endregion

        #region IGravar

      


        public bool Salvar(oCliente cliente)
        {
            bool retorno = false;
            using (_Context ctx = new _Context())
            {
                if (cliente._IdPessoa <= 0)
                    Attach(cliente, ctx, EntityState.Added);
                else
                    Attach(cliente, ctx, EntityState.Modified);
                retorno = ctx.Salvar(cliente);
            }
            return retorno;
        }

        #endregion

        #region IDeletar

        public bool Deletar(oCliente Cliente)
        {
            using (_Context ctx = new _Context())
                return Deletar(Cliente, ctx);
        }

        public bool Deletar(object IdCliente)
        {
            using (_Context ctx = new _Context())
            {
                var Cliente = ctx.dbCliente.Single(e => e._IdCliente == (int)IdCliente);
                return Deletar(Cliente, ctx);
            }
        }

        public bool Deletar(oCliente Cliente, ImplementationContext ctx)
        {
            bool retorno;
            Attach(Cliente, ctx, EntityState.Deleted);
            if (!ctx.Salvar(Cliente))
                retorno = false;
            else
                retorno = true;
            return retorno;
        }

        #endregion
    }
}
