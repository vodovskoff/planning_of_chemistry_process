using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Планирование_химического_процесса
{
    public class ChemicalReaction
    {
        public String reactionName {  get; set; }
        public HashSet<ReactionSubstance> Reactants { get; set; }
        public HashSet<ReactionSubstance> Products { get; set; }
        public bool IsExecuted { get; set; }
        public override string ToString()
        {
            string reactantsString = string.Join(" + ", Reactants.Select(rs =>
                rs.Coefficient == 1 ? rs.Substance.Substance :
                $"{rs.Coefficient}*{rs.Substance.Substance}"));

            string productsString = string.Join(" + ", Products.Select(rs =>
                rs.Coefficient == 1 ? rs.Substance.Substance :
                $"{rs.Coefficient}*{rs.Substance.Substance}"));

            return $"{reactantsString} → {productsString}";
        }

        public bool isAbleToExecute(HashSet<ReactionSubstance> visitedSubstances)
        {
            foreach (var reactant in Reactants)
            {
                if (!(visitedSubstances.Any(rs => rs.Substance.Substance == reactant.Substance.Substance))) {
                    return false;
                }
            }
            return true;
        }
    }
}
