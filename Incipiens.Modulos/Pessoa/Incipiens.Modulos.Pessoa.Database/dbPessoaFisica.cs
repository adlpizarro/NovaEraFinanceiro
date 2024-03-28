using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;
using System;
using Incipiens.Base.Funcoes;
using System.ComponentModel;
using Incipiens.Modulos.Pessoa.Object;
using Incipiens.Base.GerenciadorEF.InterfacesEntityFramework;
using Incipiens.Base.GerenciadorEF;
using System.Threading.Tasks;
using Incipiens.Base.GerenciadorEF.Funcoes;
using Incipiens.Base.Model.Tipos;

namespace Incipiens.Modulos.Pessoa.Database
{
    [DisplayName("Pessoa Fisica")]
    public class dbPessoaFisica: IListar<oPessoaFisica>, IGravar<oPessoaFisica>
    {
        _Context Ctx=new _Context();

        #region IListar


        public Expression<Func<oPessoaFisica, bool>> predicado { get; set; }

        public oOrderBy<oPessoaFisica> orderBy { get; set; }

        public IQueryable<oPessoaFisica> getQuery()
        {
            return from e in Ctx.dbPessoaFisica
                   select e;
        }

        public List<oPessoaFisica> Listar(int numeroInteracao = 1, int MaxPerLoad = ImplementationContext.MaxPerLoad)
        {
            return getQuery().LoadListFromDatabase(predicado, orderBy, (numeroInteracao - 1) * MaxPerLoad, numeroInteracao * MaxPerLoad).AsNoTracking().ToList();
        }

        public async Task<List<oPessoaFisica>> ListarAsync(int numeroInteracao = 1, int MaxPerLoad = ImplementationContext.MaxPerLoad)
        {
            return await getQuery().LoadListFromDatabase(predicado, orderBy, (numeroInteracao - 1) * MaxPerLoad, numeroInteracao * MaxPerLoad).AsNoTracking().ToListAsync();
        }

        public List<object> Listar(Expression<Func<oPessoaFisica, object>> properties, int numeroInteracao = 1, int MaxPerLoad = ImplementationContext.MaxPerLoad)
        {
            return getQuery().LoadListFromDatabase(predicado, orderBy, (numeroInteracao - 1) * MaxPerLoad, numeroInteracao * MaxPerLoad).Select(properties).AsNoTracking().ToList();
        }

        public async Task<List<object>> ListarAsync(Expression<Func<oPessoaFisica, object>> properties, int numeroInteracao = 1, int MaxPerLoad = ImplementationContext.MaxPerLoad)
        {
            return await getQuery().LoadListFromDatabase(predicado, orderBy, (numeroInteracao - 1) * MaxPerLoad, numeroInteracao * MaxPerLoad).Select(properties).AsNoTracking().ToListAsync();
        }

        public oPessoaFisica Buscar(long IdPessoa)
        {
            oPessoaFisica pessoa = null;
            using (_Context ctx = new _Context())

                    pessoa = ctx.dbPessoaFisica
                    .Include(p => p._Endereco._Municipio._EstadoFederal)
                    .Include(p => p._Celulares)
                    .Include(p => p._Emails)
                    .Include(p => p._Telefones)
                    .Include(p => p._Contatos)
                        .ThenInclude(c => c._DadosContato._Endereco)
                    .SingleOrDefault(e => e._IdPessoa == IdPessoa);
            return pessoa;
        }

        public oPessoaFisica BuscarPorCpf(string cpf)
        {
            oPessoaFisica pessoa = null;
            using (_Context ctx = new _Context())

                pessoa = ctx.dbPessoaFisica
                .Include(p => p._Endereco._Municipio._EstadoFederal)
                .Include(p => p._Celulares)
                .Include(p => p._Emails)
                .Include(p => p._Telefones)
                .Include(p => p._Contatos)
                    .ThenInclude(c => c._DadosContato._Endereco)
                .SingleOrDefault(e => e._Cpf == cpf);
            return pessoa;
        }

        public static void BuscarPorCpf(oPessoaFisica pf)
        {
            if (!String.IsNullOrEmpty(pf._Cpf))
                if (!pf.HasError(informacoesPropriedade<oPessoaFisica>.GetNameProperty(p => p._Cpf)))
                {
                    var pf2 = new dbPessoaFisica().BuscarPorCpf(pf._Cpf);
                    if (pf2 != null)
                        pf2.CloneDeep(pf);
                }
        }

        #endregion

        #region IGravar

        public bool Salvar(oPessoaFisica obj)
        {
            return new dbPessoa().Salvar(obj);
        }

        public bool Deletar(oPessoaFisica obj)
        {
            return new dbPessoa().Deletar(obj);
        }

        public bool Deletar(object Id)
        {
            return new dbPessoa().Deletar(Id);
        }

        #endregion
    }
}
