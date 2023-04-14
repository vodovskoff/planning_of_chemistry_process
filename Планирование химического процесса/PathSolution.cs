using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Планирование_химического_процесса
{
    class PathSolution
    {
        public List<HashSet<ReactionSubstance>> Substances { get; set; }
        public List<ChemicalReaction> Reactions { get; set; }

        public PathSolution(List<ChemicalReaction> chemicalReactions, HashSet<ReactionSubstance> startSubstances, HashSet<ReactionSubstance> targetSubstances)
        {
            Substances = new List<HashSet<ReactionSubstance>>();
            Substances.Add(startSubstances);
            if (chemicalReactions == null)
            {
                throw new Exception();
            }
            for (int i = 0; i < chemicalReactions.Count; i++)
            {
                var tempSet = new HashSet<ReactionSubstance>();
                tempSet.UnionWith(Substances[i]);
                tempSet.UnionWith(chemicalReactions[i].NewSubstances);
                Substances.Add(tempSet);
            }
            //Убрать те, которые полностью прореагировали и дальше не нужны
            for (int i = 0; i < Substances.Count; i++) //По всем этапам
            {
                foreach (var sub in Substances[i]) //по всем веществам этапа
                {
                    bool isFound = false; ;
                    for (int j=i+1; j < chemicalReactions.Count-1; j++)//по следующим реакциям
                    {
                        if (chemicalReactions[j].Reactants.Any(item=>item.Substance.Substance==sub.Substance.Substance))
                        {
                            isFound = true;
                        }
                    }
                    if (!isFound)
                    {
                        for (int k=i+1;k<Substances.Count; k++)
                        {
                            foreach (var  item in Substances[k].ToList())
                            {
                                if (item.Substance.Substance== sub.Substance.Substance && !targetSubstances.Any(target=>target.Substance.Substance==item.Substance.Substance))
                                {
                                    Substances[k].Remove(item);
                                }
                            }
                        }
                    }
                }
            }
            Reactions = chemicalReactions;
        }
    }
}
