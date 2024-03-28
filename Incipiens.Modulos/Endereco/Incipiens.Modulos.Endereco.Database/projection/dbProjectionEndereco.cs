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
    public class dbProjectionEndereco : IListar<projectionEndereco>, IConverter<projectionEndereco, oEndereco>
    {        
        public dbProjectionEndereco()
        {
            Ctx = new _Context();
        }

        _Context Ctx;

        #region IListar

        public Expression<Func<projectionEndereco, bool>> predicado { get; set; }

        public oOrderBy<projectionEndereco> orderBy { get; set; }

        public IQueryable<projectionEndereco> getQuery()
        {
            return from endereco in Ctx.dbEnderecos
                   join municipioAux in Ctx.dbMunicipios
                        on endereco._IdMunicipio equals municipioAux._IdMunicipio
                        into g1
                   from municipio in g1.DefaultIfEmpty()
                   join estadoAux in Ctx.dbEstadosFederais
                        on municipio._IdEstado equals estadoAux._IdEstado
                        into g2
                   from estado in g2.DefaultIfEmpty()
                   select new projectionEndereco()
                   {
                       _Bairro = endereco._Bairro,
                       _Cep = endereco._Cep,
                       _Complemento = endereco._Complemento,
                       _IdEndereco = endereco._IdEndereco,
                       _IdEstadoFederal = municipio != null ? municipio._IdEstado : "",
                       _IdMunicipio = municipio != null ? municipio._IdMunicipio : "",
                       _Logradouro = endereco._Logradouro,
                       _Municipio = municipio != null ? municipio._Nome : "",
                       _Numero = endereco._Numero,
                       _Uf = estado != null ? estado._Uf : ""
                   };
        }

        public List<projectionEndereco> Listar(int numeroInteracao = 1, int MaxPerLoad = ImplementationContext.MaxPerLoad)
        {
            return getQuery().LoadListFromDatabase(predicado, orderBy, (numeroInteracao - 1)* MaxPerLoad, numeroInteracao * ImplementationContext.MaxPerLoad).AsNoTracking().ToList();
        }

        public async Task<List<projectionEndereco>> ListarAsync(int numeroInteracao = 1, int MaxPerLoad = ImplementationContext.MaxPerLoad)
        {
            return await getQuery().LoadListFromDatabase(predicado, orderBy, (numeroInteracao - 1)* MaxPerLoad, numeroInteracao * ImplementationContext.MaxPerLoad).AsNoTracking().ToListAsync();
        }

        public List<object> Listar(Expression<Func<projectionEndereco, object>> properties, int numeroInteracao = 1, int MaxPerLoad = ImplementationContext.MaxPerLoad)
        {
            throw new NotImplementedException();
        }

        public Task<List<object>> ListarAsync(Expression<Func<projectionEndereco, object>> properties, int numeroInteracao = 1, int MaxPerLoad = ImplementationContext.MaxPerLoad)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IConverter

        public oEndereco Converter(projectionEndereco objeto)
        {
            return new dbEndereco().Buscar(objeto._IdEndereco);
        }

        public projectionEndereco Converter(oEndereco objeto)
        {
            var p = new projectionEndereco()
            {
                _Bairro = objeto._Bairro,
                _Cep = objeto._Cep,
                _Complemento = objeto._Complemento,
                _IdEndereco = objeto._IdEndereco,
                _IdEstadoFederal = objeto._Municipio != null ? objeto._Municipio._IdEstado : "",
                _IdMunicipio = objeto._Municipio != null ? objeto._Municipio._IdMunicipio : "",
                _Logradouro = objeto._Logradouro,
                _Municipio = objeto._Municipio != null ? objeto._Municipio._Nome : "",
                _Numero = objeto._Numero,
                _Uf = objeto._Municipio._EstadoFederal != null ? objeto._Municipio._EstadoFederal._Uf : ""
            };
            return p;
        }

        public IEnumerable<oEndereco> Converter(IEnumerable<projectionEndereco> lista)
        {
            var Ids = lista.Select(p => p._IdEndereco).ToList();

            predicado = p => Ids.Contains(p._IdEndereco);
            return (IEnumerable<oEndereco>)this.Listar();
        }

        public IEnumerable<projectionEndereco> Converter(IEnumerable<oEndereco> lista)
        {
            var lsProj = new List<projectionEndereco>();
            lista.ToList().ForEach(p => lsProj.Add(Converter(p)));
            return lsProj;
        }


        #endregion
    }
}
