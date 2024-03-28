using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;
using System;
using Incipiens.Modulos.Pessoa.Object;
using Incipiens.Modulos.Pessoa.Object.projection;
using Incipiens.Base.GerenciadorEF.InterfacesEntityFramework;
using Incipiens.Base.GerenciadorEF;
using System.Threading.Tasks;
using Incipiens.Base.GerenciadorEF.Funcoes;
using Microsoft.EntityFrameworkCore;
using Incipiens.Base.Model.Tipos;

namespace Incipiens.Modulos.Pessoa.Database.projection
{
    public class dbProjectionPessoa : IListar<projectionPessoa>, IConverter<projectionPessoa, oPessoa>
    {
        public dbProjectionPessoa()
        {
            ctx=new _Context();
        }
        _Context ctx;
        private string msgErro;

        #region IConverter
        public oPessoa Converter(projectionPessoa objeto)
        {
            return new dbPessoa().Buscar(objeto._IdPessoa);
        }

        public projectionPessoa Converter(oPessoa objeto)
        {
            if (objeto is oPessoaFisica pf)
                return new projectionPessoa()
                {
                    _Apelido_NomeFantasia = pf._Apelido,
                    _Cpf_Cnpj = pf._Cpf,
                    _IdPessoa = pf._IdPessoa,
                    _Nome_RazaoSocial = pf._Nome
                };
            if (objeto is oPessoaJuridica pj)
                return new projectionPessoa()
                {
                    _Apelido_NomeFantasia = pj._NomeFantasia,
                    _Cpf_Cnpj = pj._Cnpj,
                    _IdPessoa = pj._IdPessoa,
                    _Nome_RazaoSocial = pj._RazaoSocial
                };
            throw new ApplicationException("É necessário que pessoa seja fisica ou juridica");
        }

        public IEnumerable<oPessoa> Converter(IEnumerable<projectionPessoa> lista)
        {
            var Ids = lista.Select(p => p._IdPessoa).ToList();
            return (IEnumerable<oPessoa>)new dbPessoa().Listar(p => Ids.Contains(p._IdPessoa));
        }

        public IEnumerable<projectionPessoa> Converter(IEnumerable<oPessoa> lista)
        {
            var lsProj = new List<projectionPessoa>();
            lista.ToList().ForEach(p => lsProj.Add(Converter(p)));
            return lsProj;
        }

        #endregion

        public string getErro()
        {
            return msgErro;
        }

        #region IListar

        public Expression<Func<projectionPessoa, bool>> predicado { get; set; }

        public oOrderBy<projectionPessoa> orderBy { get; set; }


        public IQueryable<projectionPessoa> getQuery()
        {

            return from p in ctx.dbPessoa
                   join pf in ctx.dbPessoaFisica
                       on p._IdPessoa equals pf._IdPessoa into g1
                   from pf in g1.DefaultIfEmpty()
                   join pj in ctx.dbPessoaJuridica
                       on p._IdPessoa equals pj._IdPessoa into g2
                   from pj in g2.DefaultIfEmpty()
                   select new projectionPessoa()
                   {
                       _IdPessoa = p._IdPessoa,
                       _Cpf_Cnpj = (pf == null ? pj._Cnpj : pf._Cpf),
                       _Nome_RazaoSocial = (pf == null ? pj._RazaoSocial : pf._Nome),
                       _Apelido_NomeFantasia = (pf == null ? pj._NomeFantasia : pf._Apelido)
                   };
        }

        public List<projectionPessoa> Listar(int numeroInteracao = 1, int MaxPerLoad = ImplementationContext.MaxPerLoad)
        {
            return getQuery().LoadListFromDatabase(predicado, orderBy, (numeroInteracao - 1) * MaxPerLoad, numeroInteracao * MaxPerLoad).AsNoTracking().ToList();
        }

        public async Task<List<projectionPessoa>> ListarAsync(int numeroInteracao = 1, int MaxPerLoad = ImplementationContext.MaxPerLoad)
        {
            return await getQuery().LoadListFromDatabase(predicado, orderBy, (numeroInteracao - 1) * MaxPerLoad, numeroInteracao * MaxPerLoad).AsNoTracking().ToListAsync();
        }

        public List<object> Listar(Expression<Func<projectionPessoa, object>> properties, int numeroInteracao = 1, int MaxPerLoad = ImplementationContext.MaxPerLoad)
        {
            return getQuery().LoadListFromDatabase(predicado, orderBy, (numeroInteracao - 1) * MaxPerLoad, numeroInteracao * MaxPerLoad).Select(properties).AsNoTracking().ToList();
        }

        public async Task<List<object>> ListarAsync(Expression<Func<projectionPessoa, object>> properties, int numeroInteracao = 1, int MaxPerLoad = ImplementationContext.MaxPerLoad)
        {
            return await getQuery().LoadListFromDatabase(predicado, orderBy, (numeroInteracao - 1) * MaxPerLoad, numeroInteracao * MaxPerLoad).Select(properties).AsNoTracking().ToListAsync();
        }

        #endregion
    }
}
