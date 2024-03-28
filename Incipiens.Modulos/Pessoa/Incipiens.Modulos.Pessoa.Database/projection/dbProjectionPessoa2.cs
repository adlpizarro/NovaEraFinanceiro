using System;
using System.Collections.Generic;
using Incipiens.Base.GerenciadorEF.InterfacesEntityFramework;
using Incipiens.Modulos.Pessoa.Object.projection;
using System.Text;
using System.Linq.Expressions;
using System.Linq;
using Incipiens.Modulos.Pessoa.Object;
using Incipiens.Base.GerenciadorEF;
using Incipiens.Base.Model.Tipos;
using Incipiens.Base.GerenciadorEF.Funcoes;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Incipiens.Modulos.Pessoa.Database.projection
{

    //Por pessoa ser abstract precisamos fazer isso
    public class dbProjectionPessoa2: IListar<projectionPessoa>, IGravar<projectionPessoa>, IBuscar<projectionPessoa>, IConverter<projectionPessoa, projectionPessoa>
    {
        public dbProjectionPessoa2()
        {
            Ctx=new _Context();
        }
        private string msgErro;
        public string getErro()
        {
            return msgErro;
        }
        _Context Ctx;
        public IQueryable<projectionPessoa> getQuery()
        {
            return from p in Ctx.dbPessoa
                   join pf in Ctx.dbPessoaFisica
                       on p._IdPessoa equals pf._IdPessoa into g1
                   from pf in g1.DefaultIfEmpty()
                   join pj in Ctx.dbPessoaJuridica
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

        #region IListar


        public Expression<Func<projectionPessoa, bool>> predicado { get; set; }

        public oOrderBy<projectionPessoa> orderBy { get; set; }

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

        #region IBuscar

        public projectionPessoa Buscar(object Id)
        {
            if (!(Id is long))
                throw new ApplicationException("Id precisa do tipo long");
            long _id = (long)Id;
            return (projectionPessoa)this.Listar(p => p._IdPessoa == _id).Single();
        }


        

        #endregion

        #region IGravar

        public bool Salvar(projectionPessoa obj)
        {
            dbPessoa dbPessoa = new dbPessoa();
            return dbPessoa.Salvar(dbPessoa.Buscar(obj._IdPessoa));
        }

        public bool Deletar(projectionPessoa obj)
        {
            dbPessoa dbPessoa = new dbPessoa();
            return dbPessoa.Deletar(dbPessoa.Buscar(obj._IdPessoa));
        }

        public bool Deletar(object Id)
        {
            if (!(Id is long))
                throw new ApplicationException("Id precisa ser do tipo long");
            var _id = Convert.ToInt64(Id);
            var proj = new projectionPessoa()
            {
                _IdPessoa = _id
            };
            return Deletar(proj);
        }

        public projectionPessoa Converter(projectionPessoa objeto)
        {
            return objeto;
        }

        public IEnumerable<projectionPessoa> Converter(IEnumerable<projectionPessoa> lista)
        {
            return lista;
        }
        #endregion
    }
}
