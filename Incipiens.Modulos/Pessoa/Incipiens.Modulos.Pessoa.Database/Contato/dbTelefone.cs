using Incipiens.Base.GerenciadorEF;
using Incipiens.Base.GerenciadorEF.InterfacesEntityFramework;
using Microsoft.EntityFrameworkCore;
using System.Linq;

using Incipiens.Modulos.Pessoa.Object.Contato;

namespace Incipiens.Modulos.Pessoa.Database.Contato
{
    public class dbTelefone
    {
        public void Attach(oTelefone telefone, ImplementationContext ctx, EntityState state)
        {
            if (!ctx.ChangeTracker.Entries<oTelefone>().Any(e => e.Entity._IdPessoa == telefone._IdPessoa && e.Entity._PosicaoTelefone == telefone._PosicaoTelefone))
                ctx.Entry(telefone).State = state;
        }
    }
}
