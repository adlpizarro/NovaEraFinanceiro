using Incipiens.Base.GerenciadorEF;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;
using System;

using Incipiens.Modulos.Pessoa.Object.Contato;
using Incipiens.Modulos.Pessoa.Object;

using Incipiens.Modulos.Pessoa.Database.Contato;
using Incipiens.Modulos.Pessoa.Object.projection;


using Incipiens.Modulos.Endereco.Database;
using Incipiens.Modulos.Endereco.Object;
using Incipiens.Base.GerenciadorEF.InterfacesEntityFramework;
using Microsoft.EntityFrameworkCore.Internal;
using Incipiens.Base.GerenciadorEF.Funcoes;
using System.Threading.Tasks;
using Incipiens.Base.Model.Tipos;

namespace Incipiens.Modulos.Pessoa.Database
{
    public class dbPessoa: IListar<oPessoa>, IBuscar<oPessoa>, IGravar<oPessoa>, IDeletar<oPessoa>
    {
        public dbPessoa(bool ModificaContato = true)
        {
            this.ModificaContato = ModificaContato;
            Ctx = new _Context();
        }
        _Context Ctx;

        #region IAttach

        private bool ModificaContato;

        public void Attach(oPessoa obj, ImplementationContext ctx, EntityState entityState)
        {
            var pessoa = obj;
            if (pessoa._IdPessoa < 0)
                pessoa._IdPessoa = 0;
            if (pessoa._Endereco != null)
            {
                if (!pessoa._Endereco.isEmpty())
                {
                    if(pessoa._Endereco._IdEndereco <= 0 && entityState == EntityState.Modified)
                        dbEndereco.Attach(obj._Endereco, ctx, EntityState.Added);
                    else
                        dbEndereco.Attach(obj._Endereco, ctx, entityState);
                }
                else
                {
                    if (pessoa._Endereco._IdEndereco > 0 && entityState == EntityState.Modified)
                        dbEndereco.Attach(obj._Endereco, ctx, EntityState.Deleted);
                    pessoa._Endereco = null;
                    pessoa._IdEndereco = null;
                }
            }

            if (!ctx.ChangeTracker.Entries<oPessoa>().Any(e => e.Entity._IdPessoa == pessoa._IdPessoa))
                if (entityState == EntityState.Added)
                    ctx.Add(pessoa);
                else
                    ctx.Entry(pessoa).State = entityState;

            if (entityState == EntityState.Modified)
            {
                #region Telefone

                List<oTelefone> telInObject = new List<oTelefone>(pessoa._Telefones);
                List<oTelefone> telInDb = ctx.Set<oTelefone>().AsNoTracking().Where(t => t._IdPessoa == pessoa._IdPessoa).ToList();

                var telAdd = telInObject.Except(telInDb, new oTelefone()).ToList();
                var telDeleted = telInDb.Except(telInObject, new oTelefone()).ToList();
                var telModified = telInObject.Intersect(telInDb, new oTelefone()).ToList();

                telAdd.ForEach(t => new dbTelefone().Attach(t, ctx, EntityState.Added));
                telDeleted.ForEach(t => new dbTelefone().Attach(t, ctx, EntityState.Deleted));
                telModified.ForEach(t => new dbTelefone().Attach(t, ctx, EntityState.Modified));

                #endregion

                #region Celular

                List<oCelular> celInObject = new List<oCelular>(pessoa._Celulares);
                List<oCelular> celInDb = ctx.Set<oCelular>().AsNoTracking().Where(t => t._IdPessoa == pessoa._IdPessoa).ToList();

                var celAdd = celInObject.Except(celInDb, new oCelular()).ToList();
                var celDeleted = celInDb.Except(celInObject, new oCelular()).ToList();
                var celModified = celInObject.Intersect(celInDb, new oCelular()).ToList();

                celAdd.ForEach(t => new dbCelular().Attach(t, ctx, EntityState.Added));
                celDeleted.ForEach(t => new dbCelular().Attach(t, ctx, EntityState.Deleted));
                celModified.ForEach(t => new dbCelular().Attach(t, ctx, EntityState.Modified));

                #endregion

                #region Email

                List<oEmail> mailInObject = new List<oEmail>(pessoa._Emails);
                List<oEmail> mailInDb = ctx.Set<oEmail>().AsNoTracking().Where(t => t._IdPessoa == pessoa._IdPessoa).ToList();

                var mailAdd = mailInObject.Except(mailInDb, new oEmail()).ToList();
                var mailDeleted = mailInDb.Except(mailInObject, new oEmail()).ToList();
                var mailModified = mailInObject.Intersect(mailInDb, new oEmail()).ToList();

                mailAdd.ForEach(t => new dbEmail().Attach(t, ctx, EntityState.Added));
                mailDeleted.ForEach(t => new dbEmail().Attach(t, ctx, EntityState.Deleted));
                mailModified.ForEach(t => new dbEmail().Attach(t, ctx, EntityState.Modified));

                #endregion

                #region Contato


                ///Usado p/ não modificar o contato em caso de o objPessoa do Contato tiver contatos também
                if (ModificaContato)
                {
                    PessoasDoContatoParaTentarDeletarSemCpf = new List<oPessoaFisica>();

                    List<oContato> contatoInDb = ctx.Set<oContato>().AsNoTracking().Where(t => t._IdPessoa == pessoa._IdPessoa).Include(c => c._DadosContato._Endereco._Municipio._EstadoFederal).ToList();
                    List<oContato> contatoInObject = new List<oContato>(pessoa._Contatos);

                    var contatoAdd = contatoInObject.Except(contatoInDb, new oContato()).ToList();
                    var contatoDeleted = contatoInDb.Except(contatoInObject, new oContato()).ToList();
                    var contatoModified = contatoInObject.Intersect(contatoInDb, new oContato()).ToList();

                    contatoAdd.ForEach(t =>
                    {
                        new dbContato().Attach(t, ctx, EntityState.Added);
                    });
                    contatoDeleted.ForEach(t =>
                    {
                        new dbContato().Attach(t, ctx, EntityState.Deleted);
                        if (dbContato.PessoaParaTentarDeletarSemCpf != null)
                            PessoasDoContatoParaTentarDeletarSemCpf.Add(dbContato.PessoaParaTentarDeletarSemCpf);
                    });
                    contatoModified.ForEach(t =>
                    {
                        new dbContato().Attach(t, ctx, EntityState.Modified);
                        if (dbContato.PessoaParaTentarDeletarSemCpf != null)
                            PessoasDoContatoParaTentarDeletarSemCpf.Add(dbContato.PessoaParaTentarDeletarSemCpf);
                    });
                }
                #endregion
            }
            else
            {
                new List<oTelefone>(pessoa._Telefones).ForEach(t => new dbTelefone().Attach(t, ctx, entityState));
                new List<oCelular>(pessoa._Celulares).ForEach(t => new dbCelular().Attach(t, ctx, entityState));
                new List<oEmail>(pessoa._Emails).ForEach(t => new dbEmail().Attach(t, ctx, entityState));
                new List<oContato>(pessoa._Contatos).ForEach(c =>
                {
                    new dbContato().Attach(c, ctx, entityState);
                    if (dbContato.PessoaParaTentarDeletarSemCpf != null)
                        if (PessoasDoContatoParaTentarDeletarSemCpf != null)
                            PessoasDoContatoParaTentarDeletarSemCpf.Add(dbContato.PessoaParaTentarDeletarSemCpf);
                });
            }
        }

        #endregion

        #region IListar

        public Expression<Func<oPessoa, bool>> predicado { get; set; }

        public oOrderBy<oPessoa> orderBy { get; set; }

        public IQueryable<oPessoa> getQuery()
        {
            return from e in Ctx.dbPessoa
                   select e;
        }

        public List<oPessoa> Listar(int numeroInteracao = 1, int MaxPerLoad = ImplementationContext.MaxPerLoad)
        {
            return getQuery().LoadListFromDatabase(predicado, orderBy, (numeroInteracao - 1) * MaxPerLoad, numeroInteracao * MaxPerLoad).AsNoTracking().ToList();
        }

        public async Task<List<oPessoa>> ListarAsync(int numeroInteracao = 1, int MaxPerLoad = ImplementationContext.MaxPerLoad)
        {
            return await getQuery().LoadListFromDatabase(predicado, orderBy, (numeroInteracao - 1) * MaxPerLoad, numeroInteracao * MaxPerLoad).AsNoTracking().ToListAsync();
        }

        public List<object> Listar(Expression<Func<oPessoa, object>> properties, int numeroInteracao = 1, int MaxPerLoad = ImplementationContext.MaxPerLoad)
        {
            return getQuery().LoadListFromDatabase(predicado, orderBy, (numeroInteracao - 1) * MaxPerLoad, numeroInteracao * MaxPerLoad).Select(properties).AsNoTracking().ToList();
        }

        public async Task<List<object>> ListarAsync(Expression<Func<oPessoa, object>> properties, int numeroInteracao = 1, int MaxPerLoad = ImplementationContext.MaxPerLoad)
        {
            return await getQuery().LoadListFromDatabase(predicado, orderBy, (numeroInteracao - 1) * MaxPerLoad, numeroInteracao * MaxPerLoad).Select(properties).AsNoTracking().ToListAsync();
        }

        #endregion

        #region IBuscar

        public oPessoa Buscar(object Id)
        {
            if (!(Id is long))
                throw new ApplicationException("Id precisa do tipo long");
            long _id = (long)Id;
            oPessoa pessoa = null;
            using (_Context ctx = new _Context())
            {

                pessoa = ctx.dbPessoa
                .Include(p => p._Endereco._Municipio._EstadoFederal)
                .Include(p => p._Celulares)
                .Include(p => p._Emails)
                .Include(p => p._Telefones)
                .Include(p => p._Contatos).ThenInclude(c => c._DadosContato._Endereco._Municipio._EstadoFederal)
                .Single(e => e._IdPessoa == _id);
            }
            return pessoa;
        }

        public oPessoa Converter(oPessoa objeto)
        {
            return objeto;
        }

        public IEnumerable<oPessoa> Converter(IEnumerable<oPessoa> lista)
        {
            return lista;
        }

        #endregion      

        #region IGravar



        private List<oPessoaFisica> PessoasDoContatoParaTentarDeletarSemCpf;
        public bool Salvar(oPessoa pessoa)
        {
            bool retorno = false;
            PessoasDoContatoParaTentarDeletarSemCpf = new List<oPessoaFisica>();
            using (_Context ctx = new _Context())
            {
                
                if(pessoa._IdPessoa <= 0)
                    Attach(pessoa, ctx, EntityState.Added);
                else
                    Attach(pessoa, ctx, EntityState.Modified);
                retorno = ctx.Salvar(pessoa);

                //Tentando Excluir Pessoa sem cpf, caso haja relação em alguma tabela haverá erro
                //Utilizado para não deixar contato sozinho no banco de dados sem cpf, 
                //uma vez que sem cpf ele jamais será localizado e se transformara em lixo
                if (retorno)
                    if (PessoasDoContatoParaTentarDeletarSemCpf != null)
                        try
                        {
                            PessoasDoContatoParaTentarDeletarSemCpf.ForEach(p => Deletar(p));
                        }
                        catch { }
            }

            return retorno;
        }

        #endregion

        #region IDeletar

        public bool Deletar(oPessoa pessoa)
        {
            using (_Context ctx = new _Context())
                return Deletar(pessoa, ctx);
        }

        public bool Deletar(object IdPessoa)
        {
            using (_Context ctx = new _Context())
            {
                var pessoa = ctx.dbPessoa.Single(e => e._IdPessoa == (int)IdPessoa);
                return Deletar(pessoa, ctx);
            }
        }

        public bool Deletar(oPessoa pessoa, ImplementationContext ctx)
        {
            bool retorno;
            Attach(pessoa, ctx, EntityState.Deleted);
            if (!ctx.Salvar(pessoa))
                retorno = false;
            else
                retorno = true;
            return retorno;
        }

        
        #endregion
    }
}
