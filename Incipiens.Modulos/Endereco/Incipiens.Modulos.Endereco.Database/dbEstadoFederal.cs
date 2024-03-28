using Incipiens.Base.GerenciadorEF;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Data;
using Incipiens.Modulos.Endereco.Object;
using Incipiens.Base.GerenciadorEF.InterfacesEntityFramework;
using Org.BouncyCastle.Crypto.Modes.Gcm;

using Microsoft.EntityFrameworkCore.Internal;
using Incipiens.Base.GerenciadorEF.Funcoes;
using Incipiens.Base.Model.Tipos;
using Microsoft.Extensions.Caching.Memory;
using System.Threading.Tasks;

namespace Incipiens.Modulos.Endereco.Database
{
    public  class dbEstadoFederal: IListar<oEstadoFederal>, IBuscar<oEstadoFederal>
    {
        public dbEstadoFederal(bool IncluiMunicipios)
        {
            this.IncluiMunicipios = IncluiMunicipios;
            Ctx = new _Context();
        }   

        public dbEstadoFederal()
        {
            this.IncluiMunicipios = false;
            Ctx = new _Context();
            
        }

        _Context Ctx;

        private bool IncluiMunicipios;

        #region IListar

        public IQueryable<oEstadoFederal> getQuery()
        {
            if (IncluiMunicipios)
                return from e in Ctx.dbEstadosFederais.Include(e => e._Municipios)
                       select e;
            else
                return from e in Ctx.dbEstadosFederais
                       select e;
        }

        public Expression<Func<oEstadoFederal, bool>> predicado { get; set; }
        public oOrderBy<oEstadoFederal> orderBy { get;set; }

        public List<oEstadoFederal> Listar(int numeroInteracao = 1, int MaxPerLoad = ImplementationContext.MaxPerLoad)
        {
            return getQuery().LoadListFromDatabase(predicado, orderBy, (numeroInteracao - 1)* MaxPerLoad, numeroInteracao * ImplementationContext.MaxPerLoad).AsNoTracking().ToList();
        }

        public async Task<List<oEstadoFederal>> ListarAsync(int numeroInteracao = 1, int MaxPerLoad = ImplementationContext.MaxPerLoad)
        {
            return await getQuery().LoadListFromDatabase(predicado, orderBy, (numeroInteracao - 1)* MaxPerLoad, numeroInteracao * ImplementationContext.MaxPerLoad).AsNoTracking().ToListAsync();
        }

        public List<object> Listar(Expression<Func<oEstadoFederal, object>> properties, int numeroInteracao = 1, int MaxPerLoad = ImplementationContext.MaxPerLoad)
        {
            return getQuery().LoadListFromDatabase(predicado, orderBy, (numeroInteracao - 1)* MaxPerLoad, MaxPerLoad).Select(properties).AsNoTracking().ToList();
        }

        public Task<List<object>> ListarAsync(Expression<Func<oEstadoFederal, object>> properties, int numeroInteracao = 1, int MaxPerLoad = ImplementationContext.MaxPerLoad)
        {
            return getQuery().LoadListFromDatabase(predicado, orderBy, numeroInteracao -1, MaxPerLoad).Select(properties).AsNoTracking().ToListAsync();
        }

        #endregion

        #region IBuscar

        public oEstadoFederal Buscar(object Id)
        {
            if (!(Id is string))
                throw new ApplicationException("O Id precisa ser do tipo string");
            string _id = Convert.ToString(Id);
            oEstadoFederal estado = null;
            using (_Context ctx = new _Context())
                estado = ctx.dbEstadosFederais.SingleOrDefault(e => e._IdEstado == _id);
            return estado;
        }


        public oEstadoFederal BuscarUf(string Uf)
        {
            oEstadoFederal estado = null;
            using (_Context ctx = new _Context())
                estado = ctx.dbEstadosFederais.SingleOrDefault(e => e._Uf.ToUpper() == Uf.ToUpper());
            return estado;
        }


        #endregion

        public static void Attach(oEstadoFederal estadoFederal, ImplementationContext ctx, EntityState entity = EntityState.Unchanged)
        {
            if (estadoFederal != null)
            {
                var e = ctx.ChangeTracker.Entries<oEstadoFederal>()
                        .Select(x1 => x1.Entity).
                        SingleOrDefault(x2 => x2._IdEstado == estadoFederal._IdEstado);
                if (e == null)
                    ctx.Entry(estadoFederal).State = EntityState.Unchanged;
            }
        }

    }
}
