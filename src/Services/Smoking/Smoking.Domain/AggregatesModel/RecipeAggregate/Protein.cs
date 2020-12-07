using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Adams.Domain;

namespace Adams.Services.Smoking.Domain.AggregatesModel.RecipeAggregate
{
    public class Protein : Enumeration
    {
        public static Protein None = new Protein(1, nameof(None).ToLowerInvariant());
        public static Protein Pork = new Protein(2, nameof(Pork).ToLowerInvariant());
        public static Protein Beef = new Protein(3, nameof(Beef).ToLowerInvariant());
        public static Protein Poultry = new Protein(4, nameof(Poultry).ToLowerInvariant());
        public static Protein Lamb = new Protein(5, nameof(Lamb).ToLowerInvariant());
        public Protein(int id, string name)
            : base(id, name)
        {
        }

        public static IEnumerable<Protein> List() => new[] {None, Pork, Beef, Poultry, Lamb};
    }
}
