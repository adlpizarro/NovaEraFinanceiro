using System.Linq;
using System.Linq.Expressions;
using System;
using System.Collections.Generic;
using System.Text;
using Incipiens.Modulos.Funcionario.Object;
using Incipiens.Base.GerenciadorEF.InterfacesEntityFramework;
using Incipiens.Modulos.Pessoa.Database;
using Incipiens.Modulos.Pessoa.Object;
using Incipiens.Modulos.Funcionario.Object.projection;
using Incipiens.Base.GerenciadorEF;
using Incipiens.Base.GerenciadorEF.Funcoes;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Incipiens.Base.Model.Tipos;

namespace Incipiens.Modulos.Funcionario.Database.projection
{
    public class dbFuncionarioProjection : IListar<projectionFuncionario>,IConverter<projectionFuncionario,oFuncionario>
    {
        public dbFuncionarioProjection()
        {
            _Ctx=new _Context();
        }
        _Context _Ctx;
       

        #region IListar

        public Expression<Func<projectionFuncionario, bool>> predicado { get; set; }

        public oOrderBy<projectionFuncionario> orderBy { get; set; }

        public IQueryable<projectionFuncionario> getQuery()
        {
            return from f in _Ctx.dbFuncionario
                   select new projectionFuncionario
                   {
                       _Apelido = f._DadosFuncionario._Apelido,
                       _Cpf = f._DadosFuncionario._Cpf,
                       _IdFuncionario = f._IdFuncionario,
                       _Nome = f._DadosFuncionario._Nome
                   };
        }


        public List<projectionFuncionario> Listar(int numeroInteracao = 1, int MaxPerLoad = ImplementationContext.MaxPerLoad)
        {
            return getQuery().LoadListFromDatabase(predicado, orderBy, (numeroInteracao - 1) * MaxPerLoad, numeroInteracao * MaxPerLoad).AsNoTracking().ToList();
        }

        public async Task<List<projectionFuncionario>> ListarAsync(int numeroInteracao = 1, int MaxPerLoad = ImplementationContext.MaxPerLoad)
        {
            return await getQuery().LoadListFromDatabase(predicado, orderBy, (numeroInteracao - 1) * MaxPerLoad, numeroInteracao * MaxPerLoad).AsNoTracking().ToListAsync();
        }

        public List<object> Listar(Expression<Func<projectionFuncionario, object>> properties, int numeroInteracao = 1, int MaxPerLoad = ImplementationContext.MaxPerLoad)
        {
            return getQuery().LoadListFromDatabase(predicado, orderBy, (numeroInteracao - 1) * MaxPerLoad, numeroInteracao * MaxPerLoad).Select(properties).AsNoTracking().ToList();
        }

        public async Task<List<object>> ListarAsync(Expression<Func<projectionFuncionario, object>> properties, int numeroInteracao = 1, int MaxPerLoad = ImplementationContext.MaxPerLoad)
        {
            return await getQuery().LoadListFromDatabase(predicado, orderBy, (numeroInteracao - 1) * MaxPerLoad, numeroInteracao * MaxPerLoad).Select(properties).AsNoTracking().ToListAsync();
        }
        #endregion

        #region IBuscar

        public oFuncionario Buscar(object Id)
        {
            return new dbFuncionario().Buscar(Id);
        }


        public oFuncionario Converter(projectionFuncionario objeto)
        {
            return new dbFuncionario().Buscar(objeto._IdFuncionario);
        }

        public projectionFuncionario Converter(oFuncionario objeto)
        {
                return new projectionFuncionario()
                {
                    _Apelido = objeto._DadosFuncionario._Apelido,
                    _Cpf = objeto._DadosFuncionario._Cpf,
                    _IdFuncionario = objeto._IdFuncionario,
                    _Nome = objeto._DadosFuncionario._Nome
                };
        }

        public IEnumerable<oFuncionario> Converter(IEnumerable<projectionFuncionario> lista)
        {
            var Ids = lista.Select(p => p._IdFuncionario).ToList();
            return (IEnumerable<oFuncionario>)new dbFuncionario().Listar(p => Ids.Contains(p._IdFuncionario));
        }

        public IEnumerable<projectionFuncionario> Converter(IEnumerable<oFuncionario> lista)
        {
            var lsProj = new List<projectionFuncionario>();
            lista.ToList().ForEach(p => lsProj.Add(Converter(p)));
            return lsProj;
        }

        #endregion

    }
}
