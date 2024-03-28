using Incipiens.Base.GerenciadorEF;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;
using System;

using Incipiens.Base.Funcoes;

using Incipiens.Modulos.Pessoa.Object.Contato;
using Incipiens.Modulos.Pessoa.Object;

using Incipiens.Modulos.Pessoa.Database.Contato;
using Incipiens.Base.GerenciadorEF.InterfacesEntityFramework;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Internal;
using System.Threading.Tasks;
using Incipiens.Base.GerenciadorEF.Funcoes;
using Incipiens.Base.Model.Tipos;

namespace Incipiens.Modulos.Pessoa.Database
{
    public class dbPessoaJuridica: IListar<oPessoaJuridica>, IGravar<oPessoaJuridica>
    {
        public dbPessoaJuridica()
        {
            Ctx=new _Context();
        }
        _Context Ctx;

        #region IListar


        public Expression<Func<oPessoaJuridica, bool>> predicado { get; set; }

        public oOrderBy<oPessoaJuridica> orderBy { get; set; }

        public IQueryable<oPessoaJuridica> getQuery()
        {
            return from e in Ctx.dbPessoaJuridica
                   select e;
        }

        public List<oPessoaJuridica> Listar(int numeroInteracao = 1, int MaxPerLoad = ImplementationContext.MaxPerLoad)
        {
            return getQuery().LoadListFromDatabase(predicado, orderBy, (numeroInteracao - 1) * MaxPerLoad, numeroInteracao * MaxPerLoad).AsNoTracking().ToList();
        }

        public async Task<List<oPessoaJuridica>> ListarAsync(int numeroInteracao = 1, int MaxPerLoad = ImplementationContext.MaxPerLoad)
        {
            return await getQuery().LoadListFromDatabase(predicado, orderBy, (numeroInteracao - 1) * MaxPerLoad, numeroInteracao * MaxPerLoad).AsNoTracking().ToListAsync();
        }

        public List<object> Listar(Expression<Func<oPessoaJuridica, object>> properties, int numeroInteracao = 1, int MaxPerLoad = ImplementationContext.MaxPerLoad)
        {
            return getQuery().LoadListFromDatabase(predicado, orderBy, (numeroInteracao - 1) * MaxPerLoad, numeroInteracao * MaxPerLoad).Select(properties).AsNoTracking().ToList();
        }

        public async Task<List<object>> ListarAsync(Expression<Func<oPessoaJuridica, object>> properties, int numeroInteracao = 1, int MaxPerLoad = ImplementationContext.MaxPerLoad)
        {
            return await getQuery().LoadListFromDatabase(predicado, orderBy, (numeroInteracao - 1) * MaxPerLoad, numeroInteracao * MaxPerLoad).Select(properties).AsNoTracking().ToListAsync();
        }

        public oPessoaJuridica Buscar(long IdPessoa)
        {
            oPessoaJuridica pessoa = null;
            using (_Context ctx = new _Context())
                pessoa = ctx.dbPessoaJuridica
                .Include(p => p._Endereco._Municipio._EstadoFederal)
                .Include(p => p._Celulares)
                .Include(p => p._Emails)
                .Include(p => p._Telefones)
                .Include(p => p._Contatos)
                    .ThenInclude(c => c._DadosContato._Endereco)
                .SingleOrDefault(e => e._IdPessoa == IdPessoa);
            return pessoa;
        }

        public oPessoaJuridica BuscarPorCnpj(string cnpj)
        {
            oPessoaJuridica pessoa = null;
            using (_Context ctx = new _Context())
                pessoa = ctx.dbPessoaJuridica
                .Include(p => p._Endereco._Municipio._EstadoFederal)
                .Include(p => p._Celulares)
                .Include(p => p._Emails)
                .Include(p => p._Telefones)
                .Include(p => p._Contatos)
                    .ThenInclude(c => c._DadosContato._Endereco)
                .SingleOrDefault(e => e._Cnpj == cnpj);
            return pessoa;
        }

        public static void BuscarPorCnpj(oPessoaJuridica pj)
        {
            if (!pj.HasError(informacoesPropriedade<oPessoaJuridica>.GetNameProperty(p => p._Cnpj)))
            {
                var pf2 = new dbPessoaJuridica().BuscarPorCnpj(pj._Cnpj);
                if (pf2 != null)
                    pf2.CloneDeep(pj);
            }
        }

        #endregion

        #region IGravar

        public bool Salvar(oPessoaJuridica obj)
        {
            return new dbPessoa().Salvar(obj);
        }

        public bool Deletar(oPessoaJuridica obj)
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
