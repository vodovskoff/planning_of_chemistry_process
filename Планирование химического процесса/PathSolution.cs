using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Планирование_химического_процесса
{
    class PathSolution
    {
        public List<HashSet<ReactionSubstance>> Substances { get; set; }
        public List<ChemicalReaction> Reactions { get; set; }
        public List<Dictionary<String, double?>> ProductMasses = new List<Dictionary<String, double?>>();
        public List<Dictionary<String, double?>> ReactantMasses = new List<Dictionary<String, double?>>();
        public List<Dictionary<String, double?>> ProductAmountOfSubstance = new List<Dictionary<String, double?>>();
        public List<Dictionary<String, double?>> ReactantAmountOfSubstance = new List<Dictionary<String, double?>>();

        public PathSolution(List<ChemicalReaction> chemicalReactions, HashSet<ReactionSubstance> startSubstances, HashSet<ReactionSubstance> targetSubstances, List<ChemicalSubstance> allSubstances)
        {
            FillSubstances(chemicalReactions, startSubstances, targetSubstances);
            Reactions = chemicalReactions;
            InitializeDeicts(startSubstances, allSubstances);
            CountMasses(targetSubstances, allSubstances);
        }

        private void CountMasses(HashSet<ReactionSubstance> targetSubstances, List<ChemicalSubstance> allSubstances)
        {
            for (int i = Reactions.Count - 1; i >= 0; i--) //по всем реакциям начиная от последней
            {
                foreach (var subToCountMass in Reactions[i].Products)//По всем продуктам в текущей реакции
                {
                    if (targetSubstances.Any(item => item.Substance.SubstanceName == subToCountMass.Substance.SubstanceName))
                    {
                        var matched = targetSubstances.Where(item => item.Substance.SubstanceName.Equals(subToCountMass.Substance.SubstanceName)).ToList()[0];

                        ProductMasses[i + 1][subToCountMass.Substance.SubstanceName] += matched.Substance.Mass;
                        ProductAmountOfSubstance[i + 1][subToCountMass.Substance.SubstanceName] += ProductMasses[i + 1][subToCountMass.Substance.SubstanceName] / subToCountMass.Substance.MolarMass;
                    }
                }
                if (ProductAmountOfSubstance[i + 1].Count() > 0)
                {
                    var SubstanceWithMaxAmount = ProductAmountOfSubstance[i + 1].First();

                    foreach (var productAmountOfSubstance in ProductAmountOfSubstance[i + 1])
                    {
                        if (productAmountOfSubstance.Value > SubstanceWithMaxAmount.Value)
                        {
                            SubstanceWithMaxAmount = productAmountOfSubstance;
                        }
                    }

                    double? sumMassProduct = 0;
                    double? sumReactantAmount = 0;

                    //рассчитать количества вещества остальных продуктов
                    foreach (var product in Reactions[i].Products)
                    {
                        //если не макс то увеличиваем чтобы соответствовал макс
                        if (product.Substance.SubstanceName != SubstanceWithMaxAmount.Key && Reactions[i].Products.Any(item => item.Substance.SubstanceName.Equals(SubstanceWithMaxAmount.Key)))
                        {

                            var realSubWithMaxAmount = Reactions[i].Products.Where(item => item.Substance.SubstanceName.Equals(SubstanceWithMaxAmount.Key)).ToList()[0];
                            ProductAmountOfSubstance[i + 1][product.Substance.SubstanceName] += SubstanceWithMaxAmount.Value * (product.Coefficient / realSubWithMaxAmount.Coefficient);
                            ProductMasses[i + 1][product.Substance.SubstanceName] += ProductAmountOfSubstance[i + 1][product.Substance.SubstanceName].Value * product.Substance.MolarMass;

                        }
                        else
                        {
                            if (Reactions[i].Products.Any(item => item.Substance.SubstanceName.Equals(SubstanceWithMaxAmount.Key)) && !targetSubstances.Any(item => item.Substance.SubstanceName == product.Substance.SubstanceName))
                            {
                                ProductMasses[i + 1][product.Substance.SubstanceName] += ProductAmountOfSubstance[i + 1][product.Substance.SubstanceName].Value * product.Substance.MolarMass;
                            }
                        }
                    }
                    double sumOfReactantsCoeffs = 0.0;
                    foreach (var reactant in Reactions[i].Reactants)
                    {
                        sumReactantAmount += reactant.Coefficient * reactant.MolarMass;
                        sumOfReactantsCoeffs += reactant.Coefficient;
                    }
                    foreach (var t in ProductMasses[i + 1])
                    {
                        sumMassProduct += t.Value;
                    }
                    //рассчитать количетсва вещества реагентов
                    foreach (var reactant in Reactions[i].Reactants)
                    {
                        ReactantAmountOfSubstance[i + 1][reactant.Substance.SubstanceName] += sumMassProduct * (reactant.Coefficient / sumOfReactantsCoeffs);
                        ReactantMasses[i + 1][reactant.Substance.SubstanceName] += sumMassProduct / (sumReactantAmount / (reactant.Coefficient * reactant.MolarMass));
                    }
                    //для предыдущих прибавить массы
                    for (int j = 0; j <= i; j++)
                    {
                        foreach (var reactantAmount in ReactantAmountOfSubstance[i + 1])
                        {
                            var realReactant = allSubstances.Where(item => item.SubstanceName.Equals(reactantAmount.Key)).ToList()[0];

                            var temp1 = ProductAmountOfSubstance[j][reactantAmount.Key];
                            var temp2 = ProductMasses[i + 1][reactantAmount.Key] / realReactant.MolarMass;
                            if (temp1 < temp2)
                            {
                                ProductAmountOfSubstance[j][reactantAmount.Key] += ReactantMasses[i + 1][reactantAmount.Key] / realReactant.MolarMass;
                            }
                        }
                        foreach (var reactantAmount in ProductAmountOfSubstance[i + 1])
                        {
                            var realReactant = allSubstances.Where(item => item.SubstanceName.Equals(reactantAmount.Key)).ToList()[0];
                            var temp1 = ProductAmountOfSubstance[j][reactantAmount.Key];
                            var temp2 = ProductMasses[i + 1][reactantAmount.Key] / realReactant.MolarMass;
                            if (temp1 < temp2)
                            {
                                ProductAmountOfSubstance[j][reactantAmount.Key] += ProductMasses[i + 1][reactantAmount.Key] / realReactant.MolarMass;
                            }
                        }
                    }
                }
                restoreMassesFromNextStages();
            }
            //Костыль: если где-то массса нуль взять следующу ненулевую
        }

        private void restoreMassesFromNextStages()
        {
            for (int i = 0; i < ProductMasses.Count; i++)
            {
                foreach (var prod in ProductMasses[i].ToArray())
                {
                    if (prod.Value == 0 && i + 1 < ProductMasses.Count)
                    {
                        int r = i + 1;
                        while (prod.Value == 0 && r < ProductMasses.Count)
                        {
                            var nextSuchProd = ProductMasses[r].Where(item => item.Key == prod.Key).ToList()[0];
                            ProductMasses[i][nextSuchProd.Key] += nextSuchProd.Value;
                            r++;
                        }
                    }
                }
            }
            for (int i = 0; i < ReactantMasses.Count; i++)
            {
                foreach (var prod in ReactantMasses[i].ToArray())
                {
                    if (prod.Value == 0 && i + 1 < ReactantMasses.Count)
                    {
                        int r = i + 1;
                        while (prod.Value == 0 && r < ReactantMasses.Count)
                        {
                            var nextSuchProd = ReactantMasses[r].Where(item => item.Key == prod.Key).ToList()[0];
                            ReactantMasses[i][nextSuchProd.Key] += nextSuchProd.Value;
                            r++;
                        }
                    }
                }
            }
            for (int i = 0; i < ProductMasses.Count; i++)
            {
                foreach (var prod in ProductMasses[i].ToArray())
                {
                    if (prod.Value == 0 && i + 1 < ProductMasses.Count)
                    {
                        int r = i + 1;
                        while (prod.Value == 0 && r < ProductMasses.Count)
                        {
                            var nextSuchProd = ReactantMasses[r].Where(item => item.Key == prod.Key).ToList()[0];
                            ProductMasses[i][nextSuchProd.Key] += nextSuchProd.Value;
                            r++;
                        }
                    }
                }
            }
            for (int i = 0; i < ReactantMasses.Count; i++)
            {
                foreach (var prod in ReactantMasses[i].ToArray())
                {
                    if (prod.Value == 0 && i + 1 < ReactantMasses.Count)
                    {
                        int r = i + 1;
                        while (prod.Value == 0 && r < ReactantMasses.Count)
                        {
                            var nextSuchProd = ProductMasses[r].Where(item => item.Key == prod.Key).ToList()[0];
                            ReactantMasses[i][nextSuchProd.Key] += nextSuchProd.Value;
                            r++;
                        }
                    }
                }
            }
        }

        private void InitializeDeicts(HashSet<ReactionSubstance> startSubstances, List<ChemicalSubstance> allSubstances)
        {
            foreach (var substance in startSubstances)
            {
                ProductMasses.Add(new Dictionary<String, double?>());
                ReactantMasses.Add(new Dictionary<String, double?>());
                ProductAmountOfSubstance.Add(new Dictionary<String, double?>());
                ReactantAmountOfSubstance.Add(new Dictionary<String, double?>());
            }
            ProductMasses.Add(new Dictionary<String, double?>());
            ReactantMasses.Add(new Dictionary<String, double?>());
            ProductAmountOfSubstance.Add(new Dictionary<String, double?>());
            ReactantAmountOfSubstance.Add(new Dictionary<String, double?>());
            ProductMasses.Add(new Dictionary<String, double?>());
            ReactantMasses.Add(new Dictionary<String, double?>());
            ProductAmountOfSubstance.Add(new Dictionary<String, double?>());
            ReactantAmountOfSubstance.Add(new Dictionary<String, double?>());
            for (int k = 0; k < ProductMasses.Count; k++)
            {
                foreach (var sub in allSubstances)
                {
                    ProductMasses[k].Add(sub.SubstanceName, 0);
                    ReactantMasses[k].Add(sub.SubstanceName, 0);
                    ProductAmountOfSubstance[k].Add(sub.SubstanceName, 0);
                    ReactantAmountOfSubstance[k].Add(sub.SubstanceName, 0);
                }
            }
        }

        public void FillSubstances(List<ChemicalReaction> chemicalReactions, HashSet<ReactionSubstance> startSubstances, HashSet<ReactionSubstance> targetSubstances)
        {
            Substances = new List<HashSet<ReactionSubstance>>();
            Substances.Add(startSubstances);
            var realSubs = new List<HashSet<ReactionSubstance>>();
            realSubs.Add(startSubstances);
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
                realSubs.Add(new HashSet<ReactionSubstance>());
                foreach (var sub in Substances[i].ToArray()) //по всем веществам этапа
                {
                    bool isFound = false; ;
                    for (int j = i; j < chemicalReactions.Count; j++)//по следующим реакциям
                    {
                        if (chemicalReactions[j].Reactants.Any(item => item.Substance.SubstanceName == sub.Substance.SubstanceName))
                        {
                            isFound = true;
                        }
                    }
                    if (!isFound)
                    {
                        Substances[i].RemoveWhere(item => item.Substance.SubstanceName == sub.Substance.SubstanceName && !targetSubstances.Any(target => target.Substance.SubstanceName == item.Substance.SubstanceName));
                    } else
                    {
                        realSubs[realSubs.Count - 1].Add(sub);
                    }
                }
            }
            Reactions = chemicalReactions;
        }
    }
}
