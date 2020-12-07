using System.Collections.Generic;
using Adams.Domain;

namespace Adams.Services.Smoking.Domain.AggregatesModel.RecipeAggregate
{
    public class Protein : Enumeration
    {
        public static Protein None = new(1, nameof(None).ToLowerInvariant());
        public static Protein Pork = new(2, nameof(Pork).ToLowerInvariant());
        public static Protein Beef = new(3, nameof(Beef).ToLowerInvariant());
        public static Protein Poultry = new(4, nameof(Poultry).ToLowerInvariant());
        public static Protein Lamb = new(5, nameof(Lamb).ToLowerInvariant());

        public Protein(int id, string name)
            : base(id, name)
        {
        }

        public static IEnumerable<Protein> List()
        {
            return new[] {None, Pork, Beef, Poultry, Lamb};
        }
    }
}