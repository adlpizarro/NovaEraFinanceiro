using Incipiens.Base.GerenciadorEF;
using Incipiens.Base.GerenciadorEF.InterfacesEntityFramework;
using Microsoft.EntityFrameworkCore;
using System.Linq;

using Incipiens.Modulos.Pessoa.Object.Contato;

namespace Incipiens.Modulos.Pessoa.Database.Contato
{
    public class dbEmail
    {
        public void Attach(oEmail email, ImplementationContext ctx, EntityState state)
        {
            if (!ctx.ChangeTracker.Entries<oEmail>().Any(e => e.Entity._IdPessoa == email._IdPessoa && e.Entity._PosicaoEmail == email._PosicaoEmail))
                ctx.Entry(email).State = state;
        }
    }
}
