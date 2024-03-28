using Incipiens.Base.GerenciadorEF;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Incipiens.Modulos.Endereco.Object;
using Incipiens.Base.GerenciadorEF.InterfacesEntityFramework;
using System.Threading.Tasks;
using Incipiens.Base.GerenciadorEF.Funcoes;
using Incipiens.Base.Model.Tipos;

namespace Incipiens.Modulos.Endereco.Database
{
    public class dbMunicipio: IListar<oMunicipio>, IBuscar<oMunicipio>, IConverter<oMunicipio, oMunicipio>
    {
        public dbMunicipio()
        {
            Ctx = new _Context();
        }

        _Context Ctx;

        #region IListar

        public Expression<Func<oMunicipio, bool>> predicado { get; set; }

        public oOrderBy<oMunicipio> orderBy { get; set; }

        public IQueryable<oMunicipio> getQuery()
        {
            return from e in Ctx.dbMunicipios.Include(m => m._EstadoFederal)
                   select e;
        }

        public List<oMunicipio> Listar(int numeroInteracao = 1, int MaxPerLoad = ImplementationContext.MaxPerLoad)
        {
            return getQuery().LoadListFromDatabase(predicado, orderBy, (numeroInteracao - 1)* MaxPerLoad, MaxPerLoad).AsNoTracking().ToList();
        }

        public async Task<List<oMunicipio>> ListarAsync(int numeroInteracao = 1, int MaxPerLoad = ImplementationContext.MaxPerLoad)
        {
            return await getQuery().LoadListFromDatabase(predicado, orderBy, (numeroInteracao - 1)* MaxPerLoad, MaxPerLoad).AsNoTracking().ToListAsync();
        }

        public List<object> Listar(Expression<Func<oMunicipio, object>> properties, int numeroInteracao = 1, int MaxPerLoad = ImplementationContext.MaxPerLoad)
        {
            return getQuery().LoadListFromDatabase(predicado, orderBy, (numeroInteracao - 1)* MaxPerLoad, MaxPerLoad).Select(properties).AsNoTracking().ToList();
        }

        public Task<List<object>> ListarAsync(Expression<Func<oMunicipio, object>> properties, int numeroInteracao = 1, int MaxPerLoad = ImplementationContext.MaxPerLoad)
        {
            return getQuery().LoadListFromDatabase(predicado, orderBy, (numeroInteracao - 1)* MaxPerLoad, MaxPerLoad).Select(properties).AsNoTracking().ToListAsync();
        }

        #endregion

        #region IBuscar

        public oMunicipio Buscar(object Id)
        {
            if (!(Id is string))
                throw new ApplicationException("O Id precisa ser do tipo string");
            string _id = Convert.ToString(Id);
            oMunicipio municipio = null;
            municipio = Ctx.dbMunicipios.Include(m => m._EstadoFederal).SingleOrDefault(m => m._IdMunicipio == _id);

            return municipio;
        }

        #endregion

        public static void Attach(oMunicipio municipio, ImplementationContext ctx, EntityState entityState = EntityState.Unchanged)
        {
            if (municipio != null)
            {
                dbEstadoFederal.Attach(municipio._EstadoFederal, ctx);
                var e = ctx.ChangeTracker.Entries<oMunicipio>()
                        .Select(x1 => x1.Entity).
                        SingleOrDefault(x2 => x2._IdMunicipio == municipio._IdMunicipio);
                if (e == null)
                    ctx.Entry(municipio).State = EntityState.Unchanged;
            }
        }

        public oMunicipio Converter(oMunicipio objeto)
        {
            return objeto;
        }

        public IEnumerable<oMunicipio> Converter(IEnumerable<oMunicipio> lista)
        {
            return lista;
        }
    }
}