using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Планирование_химического_процесса
{
    class ChemicalPathFinder
    {
        private List<ChemicalReaction> reactions = new List<ChemicalReaction>();

        public ChemicalPathFinder(List<ChemicalReaction> reactions)
        {
            this.reactions = reactions;
            foreach (var reaction in reactions)
            {
                reaction.IsExecuted = false;
            }
        }

        public Dictionary<ReactionSubstance, List<ChemicalReaction>> FindPathsToSubstances(HashSet<ReactionSubstance> startSubstances, HashSet<ReactionSubstance> targetSubstances)
        {
            HashSet<ReactionSubstance> visitedSubstances = new HashSet<ReactionSubstance>();
            Dictionary<ReactionSubstance, List<ChemicalReaction>> paths = new Dictionary<ReactionSubstance, List<ChemicalReaction>>();
            Queue<ChemicalReaction> queue = new Queue<ChemicalReaction>();

            foreach (var startSubstance in startSubstances)
            {
                visitedSubstances.Add(startSubstance);
            }
            
            foreach (var currentReaction in reactions)
            {
                if (currentReaction.isAbleToExecute(visitedSubstances) && !currentReaction.IsExecuted)
                {
                    queue.Enqueue(currentReaction);
                }
            }

            while ((queue.Count > 0))
            {
                var currentReaction = queue.Dequeue();
                foreach (var product in currentReaction.Products)
                {
                    if (!paths.ContainsKey(product))
                    {
                        //Тут надо ещё подумать над сохранением путей
                        paths.Add(product, new List<ChemicalReaction>());
                        foreach (var reagent in currentReaction.Reactants) 
                        {
                            paths[product].AddRange(paths[product]);
                        }
                        paths[product].Add(currentReaction);
                    }
                    visitedSubstances.UnionWith(currentReaction.Products);
                    currentReaction.IsExecuted = true;
                }


                foreach (var nextReactions in reactions) 
                {
                    if (nextReactions.isAbleToExecute(visitedSubstances) && !nextReactions.IsExecuted)
                    {
                        if (!queue.Contains(nextReactions))
                        {
                            queue.Enqueue(nextReactions);
                        }
                    }
                }
                if (targetSubstances.SetEquals(visitedSubstances))
                {
                    return paths;
                }
            }

            return paths;
        }
    }
}
