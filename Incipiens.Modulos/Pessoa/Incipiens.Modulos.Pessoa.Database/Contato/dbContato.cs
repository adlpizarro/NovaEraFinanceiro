using Incipiens.Base.GerenciadorEF;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System;

using System.Collections.Generic;

using Incipiens.Modulos.Pessoa.Object.Contato;
using Incipiens.Modulos.Pessoa.Object;

using Incipiens.Modulos.Endereco.Object;
using Incipiens.Base.GerenciadorEF.InterfacesEntityFramework;

namespace Incipiens.Modulos.Pessoa.Database.Contato
{
    public class dbContato
    {

        public void Attach(oContato contato, ImplementationContext ctx, EntityState state)
        { 
                ctx.Entry(contato).State = state;

            PessoaParaTentarDeletarSemCpf = null;
            if (state == EntityState.Added || state == EntityState.Modified)
            {
                EntityState stateContato = EntityState.Unchanged;
                //Procura a pessoa se achar marca como modificar,
                //Se não achar marca como adicionar

                if (ctx.Set<oPessoaFisica>().AsNoTracking()
                        .SingleOrDefault(p => p._IdPessoa == contato._DadosContato._IdPessoa)
                    != null)
                {
                    stateContato = EntityState.Modified;
                }
                //Se não encontrar pelo código procura pelo cpf
                else if (contato._DadosContato._Cpf != null && contato._DadosContato._Cpf != String.Empty)
                {
                    if (ctx.Set<oPessoaFisica>().AsNoTracking()
                        .SingleOrDefault(p => p._Cpf == contato._DadosContato._Cpf) != null)
                        stateContato = EntityState.Modified;
                    else
                    {
                        stateContato = EntityState.Added;
                    }

                }
                else
                {
                    stateContato = EntityState.Added;
                }
                new dbPessoa(false).Attach(contato._DadosContato, ctx, stateContato);
            }

            if (state == EntityState.Modified || state == EntityState.Deleted)
            {
                var contatoDatabase = (oContato)ctx.Entry(contato).GetDatabaseValues().ToObject();
                contatoDatabase._DadosContato = (oPessoaFisica)ctx.Entry(contato._DadosContato).GetDatabaseValues().ToObject();
                if (contato._DadosContato._Endereco != null)
                {
                    if (!contato._DadosContato._Endereco.isEmpty())
                        contatoDatabase._DadosContato._Endereco = (oEndereco)ctx.Entry(contato._DadosContato._Endereco).GetDatabaseValues().ToObject();
                    if (contato._DadosContato._Endereco._Municipio != null)
                    {
                        contatoDatabase._DadosContato._Endereco._Municipio = (oMunicipio)ctx.Entry(contato._DadosContato._Endereco._Municipio).GetDatabaseValues().ToObject();
                        contatoDatabase._DadosContato._Endereco._Municipio._EstadoFederal = (oEstadoFederal)ctx.Entry(contato._DadosContato._Endereco._Municipio._EstadoFederal).GetDatabaseValues().ToObject();
                    }
                }
                // Só deleta se não tiver cpf
                if (contato._DadosContato._Cpf != null && contato._DadosContato._Cpf != String.Empty)
                {
                    if (state == EntityState.Modified)
                    {
                        //Se for diferente o usuário trocou a pessoa dos dados do contato
                        //logo marcamos para exclusão
                        if (contatoDatabase._IdPessoaContato != contato._DadosContato._IdPessoa)
                            PessoaParaTentarDeletarSemCpf = contatoDatabase._DadosContato;
                    }
                }
                else if (state == EntityState.Deleted)
                    PessoaParaTentarDeletarSemCpf = contatoDatabase._DadosContato;
            }
        }

        /// <summary>
        /// Nas operações de modificação e exclusão, podem ocorrer de algumas pessoas 
        /// ficarem no banco de dados sem ter relação com nem uma outra tabela, 
        /// caso seu cpf ou cnpj sejam nulos, elas devem ser removidas.
        ///
        /// Como não será possível, nesse módulo, saber se uma pessoa terá 
        /// relação com tabelas de outros módulos, deverá se tentar realizar a exclusão
        /// em uma transação separada, pois caso haja essa realação o sistema retornará um erro
        /// e toda a transação será anulada.
        /// 
        /// Caso haja a relação, o sistema não conseguirá fazer a exclusão, 
        /// porém não atrapalhará as alterações relativas ao contato
        /// </summary>
        public static oPessoaFisica PessoaParaTentarDeletarSemCpf;
    }
}
