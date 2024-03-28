using Incipiens.Base.GerenciadorEF.InterfacesEntityFramework;
using Incipiens.Modulos.Fornecedor.Object;
using Incipiens.Modulos.Fornecedor.Object.projection;
using Incipiens.Modulos.Pessoa.Object;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Incipiens.Base.GerenciadorEF;
using Incipiens.Base.GerenciadorEF.Funcoes;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Incipiens.Base.Model.Tipos;

namespace Incipiens.Modulos.Fornecedor.Database.projection
{
    public class dbFornecedorProjection : IListar<projectionFornecedor>, IConverter<projectionFornecedor, oFornecedor>
    {

        public dbFornecedorProjection()
        {
            _Ctx=new _Context();
        }
        _Context _Ctx;


        #region IListar

        public Expression<Func<projectionFornecedor, bool>> predicado { get; set; }

        public oOrderBy<projectionFornecedor> orderBy { get; set; }

        public IQueryable<projectionFornecedor> getQuery()
        {
            return from f in _Ctx.dbFornecedor
                   join pf in _Ctx.dbPessoaFisica
                        on f._IdPessoa equals pf._IdPessoa into g1
                   from pf in g1.DefaultIfEmpty()
                   join pj in _Ctx.dbPessoaJuridica
                        on f._IdPessoa equals pj._IdPessoa into g2
                   from pj in g2.DefaultIfEmpty()
                   select new projectionFornecedor
                   {
                       _ApelidoFantasia = (pf == null ? pj._NomeFantasia : pf._Apelido),
                       _CpfCnpj = (pf == null ? pj._Cnpj : pf._Cpf),
                       _IdFornecedor = f._IdFornecedor,
                       _NomeRazaoSocial = (pf == null ? pj._RazaoSocial : pf._Nome)
                   };
        }


        public List<projectionFornecedor> Listar(int numeroInteracao = 1, int MaxPerLoad = ImplementationContext.MaxPerLoad)
        {
            return getQuery().LoadListFromDatabase(predicado, orderBy, (numeroInteracao - 1) * MaxPerLoad, numeroInteracao * MaxPerLoad).AsNoTracking().ToList();
        }

        public async Task<List<projectionFornecedor>> ListarAsync(int numeroInteracao = 1, int MaxPerLoad = ImplementationContext.MaxPerLoad)
        {
            return await getQuery().LoadListFromDatabase(predicado, orderBy, (numeroInteracao - 1) * MaxPerLoad, numeroInteracao * MaxPerLoad).AsNoTracking().ToListAsync();
        }

        public List<object> Listar(Expression<Func<projectionFornecedor, object>> properties, int numeroInteracao = 1, int MaxPerLoad = ImplementationContext.MaxPerLoad)
        {
            return getQuery().LoadListFromDatabase(predicado, orderBy, (numeroInteracao - 1) * MaxPerLoad, numeroInteracao * MaxPerLoad).Select(properties).AsNoTracking().ToList();
        }

        public async Task<List<object>> ListarAsync(Expression<Func<projectionFornecedor, object>> properties, int numeroInteracao = 1, int MaxPerLoad = ImplementationContext.MaxPerLoad)
        {
            return await getQuery().LoadListFromDatabase(predicado, orderBy, (numeroInteracao - 1) * MaxPerLoad, numeroInteracao * MaxPerLoad).Select(properties).AsNoTracking().ToListAsync();
        }

        #endregion

        #region IBuscar

        public oFornecedor Buscar(object Id)
        {
            return new dbFornecedor().Buscar(Id);
        }


        public oFornecedor Converter(projectionFornecedor objeto)
        {
            return new dbFornecedor().Buscar(objeto._IdFornecedor);
        }

        public projectionFornecedor Converter(oFornecedor objeto)
        {

            if (objeto._DadosFornecedor is oPessoaFisica pf)
                return new projectionFornecedor()
                {
                    _ApelidoFantasia = pf._Apelido,
                    _CpfCnpj = pf._Cpf,
                    _IdFornecedor = objeto._IdFornecedor,
                    _NomeRazaoSocial = pf._Nome
                };
            if (objeto._DadosFornecedor is oPessoaJuridica pj)
                return new projectionFornecedor()
                {
                    _ApelidoFantasia = pj._NomeFantasia,
                    _CpfCnpj = pj._Cnpj,
                    _IdFornecedor = objeto._IdFornecedor,
                    _NomeRazaoSocial = pj._RazaoSocial
                };
            throw new ApplicationException("É necessário que o fornecedor seja fisica ou juridica");

        }

        public IEnumerable<oFornecedor> Converter(IEnumerable<projectionFornecedor> lista)
        {
            var Ids = lista.Select(p => p._IdFornecedor).ToList();
            return (IEnumerable<oFornecedor>)new dbFornecedor().Listar(p => Ids.Contains(p._IdFornecedor));
        }

        public IEnumerable<projectionFornecedor> Converter(IEnumerable<oFornecedor> lista)
        {
            var lsProj = new List<projectionFornecedor>();
            lista.ToList().ForEach(p => lsProj.Add(Converter(p)));
            return lsProj;
        }

        #endregion
    }
}
