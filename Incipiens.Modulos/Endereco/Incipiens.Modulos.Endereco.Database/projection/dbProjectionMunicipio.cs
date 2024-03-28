using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Linq.Expressions;
using Incipiens.Base.GerenciadorEF.InterfacesEntityFramework;
using Incipiens.Modulos.Endereco.Object;
using Incipiens.Modulos.Endereco.Object.projection;
using Microsoft.EntityFrameworkCore;
using Incipiens.Base.Funcoes;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Incipiens.Base.GerenciadorEF.Funcoes;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Google.Protobuf.WellKnownTypes;
using Incipiens.Base.GerenciadorEF;
using Incipiens.Base.Model.Tipos;
using System.Threading.Tasks;
using System.Threading;

namespace Incipiens.Modulos.Endereco.Database.projection
{
    public class dbProjectionMunicipio : 
        IListar<projectionMunicipio>, 
        IConverter<projectionMunicipio, oMunicipio>
    {        
        public dbProjectionMunicipio()
        {
            Ctx = new _Context();
        }

        _Context Ctx;

        #region IListar

        public Expression<Func<projectionMunicipio, bool>> predicado { get; set; }

        public oOrderBy<projectionMunicipio> orderBy { get; set; }

        public IQueryable<projectionMunicipio> getQuery()
        {
            return from municipio in Ctx.dbMunicipios
                   join estado in Ctx.dbEstadosFederais
                       on municipio._IdEstado equals estado._IdEstado into g1
                   from munipio_estado in g1.DefaultIfEmpty()
                   select new projectionMunicipio()
                   {
                       _IdMunicipio = municipio._IdMunicipio,
                       _Nome = municipio._Nome,
                       _Uf = munipio_estado._Uf
                   };
        }

        public List<projectionMunicipio> Listar(int numeroInteracao = 1, int MaxPerLoad = ImplementationContext.MaxPerLoad)
        {
            return getQuery().LoadListFromDatabase(predicado, orderBy, (numeroInteracao - 1)* MaxPerLoad, numeroInteracao * ImplementationContext.MaxPerLoad).AsNoTracking().ToList();
        }

        public async Task<List<projectionMunicipio>> ListarAsync(int numeroInteracao = 1, int MaxPerLoad = ImplementationContext.MaxPerLoad)
        {
            return await getQuery().LoadListFromDatabase(predicado, orderBy, (numeroInteracao - 1)* MaxPerLoad, numeroInteracao * ImplementationContext.MaxPerLoad).AsNoTracking().ToListAsync();
        }

        public List<object> Listar(Expression<Func<projectionMunicipio, object>> properties, int numeroInteracao = 1, int MaxPerLoad = ImplementationContext.MaxPerLoad)
        {
            throw new NotImplementedException();
        }

        public Task<List<object>> ListarAsync(Expression<Func<projectionMunicipio, object>> properties, int numeroInteracao = 1, int MaxPerLoad = ImplementationContext.MaxPerLoad)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IConverter

        public oMunicipio Converter(projectionMunicipio objeto)
        {
            return new dbMunicipio().Buscar(objeto._IdMunicipio);
        }

        public projectionMunicipio Converter(oMunicipio objeto)
        {
            var p = new projectionMunicipio()
            {
                _IdMunicipio = objeto._IdMunicipio,
                _Nome = objeto._Nome,
                _Uf = objeto._EstadoFederal._Uf
            };
            return p;
        }

        public IEnumerable<oMunicipio> Converter(IEnumerable<projectionMunicipio> lista)
        {
            var Ids = lista.Select(p => p._IdMunicipio).ToList();

            predicado = p => Ids.Contains(p._IdMunicipio);
            return (IEnumerable<oMunicipio>)this.Listar();
        }

        public IEnumerable<projectionMunicipio> Converter(IEnumerable<oMunicipio> lista)
        {
            var lsProj = new List<projectionMunicipio>();
            lista.ToList().ForEach(p => lsProj.Add(Converter(p)));
            return lsProj;
        }


        #endregion

        
    }
}
