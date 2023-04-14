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
                reaction.NewSubstances = new HashSet<ReactionSubstance>();
            }
        }

        public PathSolution FindPathsToSubstances(HashSet<ReactionSubstance> startSubstances, HashSet<ReactionSubstance> targetSubstances)
        {
            HashSet<ReactionSubstance> visitedSubstances = new HashSet<ReactionSubstance>();
            Dictionary<ReactionSubstance, List<ChemicalReaction>> paths = new Dictionary<ReactionSubstance, List<ChemicalReaction>>();
            Queue<ChemicalReaction> queue = new Queue<ChemicalReaction>();

            foreach (var startSubstance in startSubstances)
            {
                visitedSubstances.Add(startSubstance);
                paths.Add(startSubstance, new List<ChemicalReaction>());
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
                        paths.Add(product, new List<ChemicalReaction>());
                        foreach (var reagent in currentReaction.Reactants) 
                        {
                            var previousPath = paths.Where(item => item.Key.Substance.Substance == reagent.Substance.Substance).Select(item =>item.Value);
                            foreach (var path in previousPath)
                            {
                                paths[product].AddRange(path);
                            }
                        }
                        currentReaction.NewSubstances = currentReaction.Products;
                        currentReaction.NewSubstances.ExceptWith(visitedSubstances);
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
                    return new PathSolution(FindOnlyRightPaths(paths, startSubstances, targetSubstances), startSubstances, targetSubstances);
                }
            }

            return new PathSolution(FindOnlyRightPaths(paths, startSubstances, targetSubstances), startSubstances, targetSubstances);
        }

        private List<ChemicalReaction> FindOnlyRightPaths(Dictionary<ReactionSubstance, List<ChemicalReaction>> allPaths, HashSet<ReactionSubstance> startSubstances, HashSet<ReactionSubstance> targetSubstances)
        {
            List<ChemicalReaction> rightParts = new List<ChemicalReaction>();
            foreach (var target in targetSubstances)
            {
                if (!(allPaths.Any(item=>item.Key.Substance.Substance==target.Substance.Substance)))
                {
                    return null;
                }
            }
            foreach (var path in allPaths)
            {
                if (targetSubstances.Any(item=>item.Substance.Substance == path.Key.Substance.Substance))
                {
                    foreach (var reaction in path.Value)
                    {
                        if (!rightParts.Any(item=>item.reactionName==reaction.reactionName))
                        {
                            rightParts.Add(reaction);
                        }
                    }
                }
            }
            return rightParts;
        }
    }
}
