
using Incipiens.Base.GerenciadorEF;
using Incipiens.Base.GerenciadorEF.InterfacesEntityFramework;
using Microsoft.EntityFrameworkCore;
using System.Linq;

using Incipiens.Modulos.Pessoa.Object.Contato;

namespace Incipiens.Modulos.Pessoa.Database.Contato
{
    public class dbCelular
    {
        public void Attach(oCelular celular, ImplementationContext ctx, EntityState state)
        {
            if (!ctx.ChangeTracker.Entries<oCelular>().Any(e => e.Entity._IdPessoa == celular._IdPessoa && e.Entity._PosicaoCelular == celular._PosicaoCelular))
                ctx.Entry(celular).State = state;
        }
    }
}
