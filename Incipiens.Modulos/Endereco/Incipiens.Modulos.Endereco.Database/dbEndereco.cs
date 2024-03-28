using Incipiens.Base.GerenciadorEF;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using Incipiens.Modulos.Endereco.Object;
using Incipiens.Base.GerenciadorEF.InterfacesEntityFramework;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Incipiens.Base.GerenciadorEF.Funcoes;
using Incipiens.Base.Model.Tipos;

namespace Incipiens.Modulos.Endereco.Database
{

    public class dbEndereco: 
        IListar<oEndereco>,
        IBuscar<oEndereco>, 
        IGravar<oEndereco>, 
        IDeletar<oEndereco>

    {
        public dbEndereco()
        {
            Ctx = new _Context();
        }

        _Context Ctx;

        #region IListar

        public Expression<Func<oEndereco, bool>> predicado { get; set; }

        public oOrderBy<oEndereco> orderBy { get; set; }

        public IQueryable<oEndereco> getQuery()
        {
            return from e in Ctx.dbEnderecos
                   select e;
        }

        public List<oEndereco> Listar(int numeroInteracao = 1, int MaxPerLoad = ImplementationContext.MaxPerLoad)
        {
            return getQuery().LoadListFromDatabase(predicado, orderBy, (numeroInteracao - 1)* MaxPerLoad, numeroInteracao * MaxPerLoad).AsNoTracking().ToList();
        }

        public async Task<List<oEndereco>> ListarAsync(int numeroInteracao = 1, int MaxPerLoad = ImplementationContext.MaxPerLoad)
        {
            return await getQuery().LoadListFromDatabase(predicado, orderBy, (numeroInteracao - 1)* MaxPerLoad, numeroInteracao * MaxPerLoad).AsNoTracking().ToListAsync();
        }

        public List<object> Listar(Expression<Func<oEndereco, object>> properties, int numeroInteracao = 1, int MaxPerLoad = ImplementationContext.MaxPerLoad)
        {
            return getQuery().LoadListFromDatabase(predicado, orderBy, (numeroInteracao - 1)* MaxPerLoad, numeroInteracao * MaxPerLoad).Select(properties).AsNoTracking().ToList();
        }

        public async Task<List<object>> ListarAsync(Expression<Func<oEndereco, object>> properties, int numeroInteracao = 1, int MaxPerLoad = ImplementationContext.MaxPerLoad)
        {
            return await getQuery().LoadListFromDatabase (predicado, orderBy, (numeroInteracao - 1)* MaxPerLoad, numeroInteracao * MaxPerLoad).Select(properties).AsNoTracking().ToListAsync();
        }

        #endregion

        #region IBuscar

        public oEndereco Buscar(object Id)
        {
            if (!(Id is long))
                throw new ApplicationException("O Id precisa ser do tipo long");
            long _id = Convert.ToUInt32(Id);
            oEndereco endereco = null;
            endereco = Ctx.dbEnderecos.Include(end => end._Municipio._EstadoFederal).AsNoTracking().SingleOrDefault(e => e._IdEndereco == _id);
            return endereco;
        }

        public oEndereco Converter(oEndereco objeto)
        {
            return objeto;
        }

        public IEnumerable<oEndereco> Converter(IEnumerable<oEndereco> lista)
        {
            return lista;
        }

        #endregion

        #region IGravar

        public bool Salvar(oEndereco endereco)
        {
            if (endereco.isEmpty())
            {
                endereco.msgErroPrincipal = "Endereço está em branco";
                return false;
            }
            bool retorno;

            if (endereco._IdEndereco <= 0)
                Attach(endereco, Ctx, EntityState.Added);
            else
                Attach(endereco, Ctx, EntityState.Modified);
            retorno = Ctx.Salvar(endereco);
            return retorno;
        }


        #endregion

        #region IDeletar

        public bool Deletar(oEndereco endereco)
        {
            return Deletar(endereco, Ctx);
        }

        public bool Deletar(object IdEndereco)
        {
            var endereco = Ctx.dbEnderecos.Single(e => e._IdEndereco == (int)IdEndereco);
            return Deletar(endereco, Ctx);
        }

        public bool Deletar(oEndereco endereco, ImplementationContext ctx)
        {
            bool retorno;
            Attach(endereco, ctx, EntityState.Deleted);
            if (!ctx.Salvar(endereco))
                retorno = false;
            else
                retorno = true;
            return retorno;
        }

        #endregion

        #region IAttach

        public static void Attach(oEndereco endereco, ImplementationContext ctx, EntityState entityState)
        {
            if (endereco._Municipio != null)
                dbMunicipio.Attach(endereco._Municipio, ctx);
            if (!ctx.ChangeTracker.Entries<oEndereco>().Any(e => e.Entity._IdEndereco == endereco._IdEndereco))
                ctx.Entry(endereco).State = entityState;
        }

        #endregion

        /// <summary>
        /// Retorna Nulo Quando o Cep Não for encontrado
        /// </summary>
        /// <param name="cep"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public static async Task<oEndereco> BuscaCep(string cep, CancellationToken token)
        {
            var endereco = new oEndereco() { _Cep = cep };
            var erros = endereco.GetErrors(() => endereco._Cep);
            if (erros == null)
            {
                HttpClient client = new HttpClient();
                string str = $"https://viacep.com.br/ws/" + endereco._Cep + "/xml";
                var resposta = await client.GetAsync(str, token);
                if (token.IsCancellationRequested)
                    return null;
                if (resposta.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    string strXml = await resposta.Content.ReadAsStringAsync();
                    if (token.IsCancellationRequested)
                        return null;
                    XmlReader reader = XmlReader.Create(new StringReader(strXml), new XmlReaderSettings() { Async = true });
                    while (await reader.ReadAsync())
                    {
                        if (token.IsCancellationRequested)
                            return null;
                        if (reader.NodeType == XmlNodeType.Element)
                            if (reader.Name != "erro")
                                switch (reader.Name)
                                {
                                    case "logradouro":
                                        endereco._Logradouro = reader.ReadElementContentAsString().ToUpper();
                                        break;

                                    case "complemento":
                                        endereco._Complemento = reader.ReadElementContentAsString().ToUpper();
                                        break;

                                    case "bairro":
                                        endereco._Bairro = reader.ReadElementContentAsString().ToUpper();
                                        break;

                                    case "ibge":
                                        endereco._Municipio = new dbMunicipio().Buscar(reader.ReadElementContentAsString());
                                        endereco._IdMunicipio = endereco._Municipio._IdMunicipio;
                                        break;
                                }
                    }
                    return endereco;
                }
            }
            return null;
        }

    }
}

