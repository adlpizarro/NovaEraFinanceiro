using Incipiens.Base.GerenciadorEF.InterfacesEntityFramework;
using Incipiens.Base.Model;
using Incipiens.Modulos.Cliente.Object;
using Incipiens.Modulos.Cliente.Object.projection;
using Incipiens.Modulos.Pessoa.Object;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Incipiens.Base.GerenciadorEF;
using System.Text;
using Incipiens.Base.GerenciadorEF.Funcoes;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Incipiens.Base.Model.Tipos;

namespace Incipiens.Modulos.Cliente.Database.projection
{
    public class dbClienteProjection : IListar<projectionCliente>, IConverter<projectionCliente,oCliente>
    {
        public dbClienteProjection()
        {
            Ctx=new _Context();
        }
        _Context Ctx;

        #region IListar


        public Expression<Func<projectionCliente, bool>> predicado { get; set; }

        public oOrderBy<projectionCliente> orderBy { get; set; }

        public IQueryable<projectionCliente> getQuery()
        {
            return from f in Ctx.dbCliente
                   join pf in Ctx.dbPessoaFisica
                        on f._IdPessoa equals pf._IdPessoa into g1
                   from pf in g1.DefaultIfEmpty()
                   join pj in Ctx.dbPessoaJuridica
                        on f._IdPessoa equals pj._IdPessoa into g2
                   from pj in g2.DefaultIfEmpty()
                   select new projectionCliente
                   {
                       _ApelidoFantasia = (pf == null ? pj._NomeFantasia : pf._Apelido),
                       _CpfCnpj = (pf == null ? pj._Cnpj : pf._Cpf),
                       _IdCliente = f._IdCliente,
                       _NomeRazaoSocial = (pf == null ? pj._RazaoSocial : pf._Nome)
                   };
        }


        public List<projectionCliente> Listar(int numeroInteracao = 1, int MaxPerLoad = ImplementationContext.MaxPerLoad)
        {
            return getQuery().LoadListFromDatabase(predicado, orderBy, (numeroInteracao - 1) * MaxPerLoad, numeroInteracao * MaxPerLoad).AsNoTracking().ToList();
        }

        public async Task<List<projectionCliente>> ListarAsync(int numeroInteracao = 1, int MaxPerLoad = ImplementationContext.MaxPerLoad)
        {
            return await getQuery().LoadListFromDatabase(predicado, orderBy, (numeroInteracao - 1) * MaxPerLoad, numeroInteracao * MaxPerLoad).AsNoTracking().ToListAsync();
        }

        public List<object> Listar(Expression<Func<projectionCliente, object>> properties, int numeroInteracao = 1, int MaxPerLoad = ImplementationContext.MaxPerLoad)
        {
            return getQuery().LoadListFromDatabase(predicado, orderBy, (numeroInteracao - 1) * MaxPerLoad, numeroInteracao * MaxPerLoad).Select(properties).AsNoTracking().ToList();
        }

        public async Task<List<object>> ListarAsync(Expression<Func<projectionCliente, object>> properties, int numeroInteracao = 1, int MaxPerLoad = ImplementationContext.MaxPerLoad)
        {
            return await getQuery().LoadListFromDatabase(predicado, orderBy, (numeroInteracao - 1) * MaxPerLoad, numeroInteracao * MaxPerLoad).Select(properties).AsNoTracking().ToListAsync();
        }
        #endregion

        #region IBuscar

        public oCliente Buscar(object Id)
        {
            return new dbCliente().Buscar(Id);
        }

        public oCliente Converter(projectionCliente objeto)
        {
            return new dbCliente().Buscar(objeto._IdCliente);
        }

        public projectionCliente Converter(oCliente objeto)
        {

            if (objeto._DadosCliente is oPessoaFisica pf)
                return new projectionCliente()
                {
                    _ApelidoFantasia = pf._Apelido,
                    _CpfCnpj = pf._Cpf,
                    _IdCliente = objeto._IdCliente,
                    _NomeRazaoSocial = pf._Nome
                };
            if (objeto._DadosCliente is oPessoaJuridica pj)
                return new projectionCliente()
                {
                    _ApelidoFantasia = pj._NomeFantasia,
                    _CpfCnpj = pj._Cnpj,
                    _IdCliente = objeto._IdCliente,
                    _NomeRazaoSocial = pj._RazaoSocial
                };
            throw new ApplicationException("É necessário que cliente seja fisica ou juridica");

        }

        public IEnumerable<oCliente> Converter(IEnumerable<projectionCliente> lista)
        {
            var Ids = lista.Select(p => p._IdCliente).ToList();
            return (IEnumerable<oCliente>)new dbCliente().Listar(p => Ids.Contains(p._IdCliente));
        }

        public IEnumerable<projectionCliente> Converter(IEnumerable<oCliente> lista)
        {
            var lsProj = new List<projectionCliente>();
            lista.ToList().ForEach(p => lsProj.Add(Converter(p)));
            return lsProj;
        }

        
        #endregion
    }
}
