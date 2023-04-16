using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Collections.Specialized.BitVector32;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Планирование_химического_процесса
{
    public partial class Form1 : Form
    {
        private List<ChemicalSubstance> substances = new List<ChemicalSubstance>();
        private List<ChemicalReaction> reactions = new List<ChemicalReaction>();
        private List<ReactionSubstance> tempReactatns = new List<ReactionSubstance>();
        private List<ReactionSubstance> tempProducts = new List<ReactionSubstance>();
        private List<ReactionSubstance> inputProducts = new List<ReactionSubstance>();
        private List<ReactionSubstance> inputReactants = new List<ReactionSubstance>();

        public Form1()
        {
            InitializeComponent();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                textBox5.Text = listBox1.SelectedItem.ToString();
                foreach (var subu in substances)
                {
                    if (subu.SubstanceName == textBox5.Text)
                    {
                        textBox10.Text = subu.MolarMass.ToString();
                    }
                }
            }
        }

        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button8_Click(object sender, EventArgs e)
        {
            // проверяем, что выбрано название вещества
            if (listBox5.SelectedIndex == -1)
            {
                MessageBox.Show("Выберите название вещества!");
                return;
            }

            string substanceName = listBox5.SelectedItem.ToString();
            double molarMass=1;
            double mass=1;

            // проверяем, что вещество с таким названием еще не добавлено
            if (inputReactants.Any(p => p.Substance.SubstanceName == substanceName))
            {
                MessageBox.Show("Вещество с таким названием уже добавлено!");
                return;
            }

            // создаем новый объект ReactionSubstance
            ChemicalSubstance newSubstanceTemp = substances.Find(x => x.SubstanceName == substanceName);
            ChemicalSubstance newSubstance = new ChemicalSubstance(newSubstanceTemp.SubstanceName, newSubstanceTemp.MolarMass);
            newSubstance.Mass = mass;

            // добавляем его в список inputProducts
            inputReactants.Add(new ReactionSubstance(newSubstance, molarMass, 1));

            // добавляем строку в dataGridView1
            dataGridView1.Rows.Add(substanceName, molarMass, mass);
        }

        private void groupBox13_Enter(object sender, EventArgs e)
        {

        }

        private void groupBox12_Enter(object sender, EventArgs e)
        {

        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(textBox1.Text) && !string.IsNullOrWhiteSpace(textBox11.Text))
            {
                string newSubstance = textBox1.Text.Trim();
                double MolarMass = 0;
                if (!(Double.TryParse(textBox11.Text.Trim(), out MolarMass)))
                {
                    return;
                }

                if (!substances.Any(s => s.SubstanceName == newSubstance))
                {
                    substances.Add(new ChemicalSubstance(newSubstance, MolarMass));
                    listBox1.Items.Add(newSubstance);
                }
                Реакции_Enter(sender, e);
                textBox1.Clear();
            }
            ClearAndFillListBox1();   
            clearAndFillCheckListBoxes();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(textBox5.Text))
            {
                double MolarMass = 0;
                if (!(Double.TryParse(textBox10.Text.Trim(), out MolarMass)))
                {
                    return;
                }
                string newSubstance = textBox5.Text.Trim();

                if (!substances.Any(s => s.SubstanceName == newSubstance))
                {
                    int index = listBox1.SelectedIndex;
                    substances[index].SubstanceName = newSubstance;
                    substances[index].MolarMass = MolarMass;
                    listBox1.Items[index] = newSubstance;
                } else
                {
                    int index = listBox1.SelectedIndex;
                    substances[index].MolarMass = MolarMass;
                    listBox1.Items[index] = newSubstance;
                }

                textBox5.Clear();
            }
            ClearAndFillListBox1();
            clearAndFillCheckListBoxes();
        }

        private void checkedListBox1_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (e.NewValue == CheckState.Checked)
            {
                string substance = checkedListBox1.Items[e.Index].ToString();
                if (!listBox3.Items.Contains(substance))
                {
                    listBox3.Items.Add(substance);
                }
            }
            else
            {
                string substance = checkedListBox1.Items[e.Index].ToString();
                if (listBox3.Items.Contains(substance))
                {
                    listBox3.Items.Remove(substance);
                }
            }
        }

        private void checkedListBox2_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (e.NewValue == CheckState.Checked)
            {
                string substance = checkedListBox2.Items[e.Index].ToString();
                if (!listBox4.Items.Contains(substance))
                {
                    listBox4.Items.Add(substance);
                }
            }
            else
            {
                string substance = checkedListBox2.Items[e.Index].ToString();
                if (listBox4.Items.Contains(substance))
                {
                    listBox4.Items.Remove(substance);
                }
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox4.Text))
            {
                MessageBox.Show("Введите название реакции.");
                return;
            }

            if (listBox3.Items.Count == 0 || listBox4.Items.Count == 0)
            {
                MessageBox.Show("Выберите начальные и конечные вещества для реакции.");
                return;
            }

            string reactionName = textBox4.Text.Trim();

            if (reactions.Any(r => r.reactionName == reactionName))
            {
                MessageBox.Show("Реакция с таким названием уже существует.");
                return;
            }

            HashSet<ReactionSubstance> reactants = new HashSet<ReactionSubstance>();
            foreach (var reactant in tempReactatns)
            {
                if (checkedListBox1.CheckedItems.Contains(reactant.Substance.SubstanceName))
                {
                    reactants.Add(reactant);
                }
            }

            HashSet<ReactionSubstance> products = new HashSet<ReactionSubstance>();
            foreach (var product in tempProducts)
            {
                if (checkedListBox2.CheckedItems.Contains(product.Substance.SubstanceName))
                {
                    products.Add(product);
                }
            }

            reactions.Add(new ChemicalReaction { reactionName = reactionName, Reactants = reactants, Products = products });

            ChemicalReactionStorage re = new ChemicalReactionStorage(reactions);
            re.Save();
            
            ClearAndFillListBox2();
            textBox4.Clear();
            textBox2.Clear();
            textBox3.Clear();
            listBox3.Items.Clear();
            listBox4.Items.Clear();
            tempProducts.Clear();
            tempReactatns.Clear();
        }

        private void Реакции_Enter(object sender, EventArgs e)
        {
            treeView1.Nodes.Clear();
            var isFull = checkIsDataFull();
            clearAndFillCheckListBoxes();
        }

        private bool checkIsDataFull()
        {
            var isAllOk = true;
            // Получить список названий реакций
            List<string> reactionNames = new List<string>();
            foreach (ChemicalReaction reaction in reactions)
            {
                if (IsDataFullChecker.CheckReactions(reaction) != "ok")
                {
                    isAllOk = false;
                }
                reactionNames.Add(reaction.reactionName + ": " + IsDataFullChecker.CheckReactions(reaction));
            }

            // Добавить каждое название в TreeView в качестве корневого узла
            foreach (string reactionName in reactionNames)
            {
                TreeNode rootNode = new TreeNode(reactionName);
                treeView1.Nodes.Add(rootNode);
            }

            List<string> substancesStr = new List<string>();
            foreach (var sub in substances)
            {
                if (IsDataFullChecker.CheckChemicalSubstance(sub) != "ok")
                {
                    isAllOk = false;
                }
                substancesStr.Add(sub.SubstanceName + ": " + IsDataFullChecker.CheckChemicalSubstance(sub));
            }

            // Добавить каждое название в TreeView в качестве корневого узла
            foreach (string substance in substancesStr)
            {
                TreeNode rootNode = new TreeNode(substance);
                treeView2.Nodes.Add(rootNode);
            }


            if (isAllOk)
            {
                label3.Text = "Полнота не нарушена!";
            }
            else
            {
                label3.Text = "Полнота нарушена!";
            }
            return isAllOk;
        }

        private void clearAndFillCheckListBoxes()
        {
            checkedListBox1.Items.Clear();
            checkedListBox2.Items.Clear();

            if (substances!=null)
            {
                foreach (ChemicalSubstance substance in substances)
                {
                    checkedListBox1.Items.Add(substance.SubstanceName);
                    checkedListBox2.Items.Add(substance.SubstanceName);
                }
            }
        }

        private void ClearAndFillListBox2()
        {
            listBox2.Items.Clear();
            if (reactions!=null)
            {
                foreach (ChemicalReaction reaction in reactions)
                {
                    listBox2.Items.Add(reaction.reactionName);
                }
            }
        }

        private void ClearAndFillListBox1()
        {
            listBox1.Items.Clear();
            listBox5.Items.Clear();
            listBox6.Items.Clear();
            ChemicalSubstanceStorage storage = new ChemicalSubstanceStorage(substances);
            if (substances!=null)
            {
                storage.Save();
                foreach (ChemicalSubstance substance in substances)
                {
                    listBox1.Items.Add(substance.SubstanceName);
                    listBox5.Items.Add(substance.SubstanceName);
                    listBox6.Items.Add(substance.SubstanceName);
                }
            }

        }

        private void RemoveSubstanceFromList(string substanceName)
        {
            for (int i = substances.Count - 1; i >= 0; i--)
            {
                if (substances[i].SubstanceName == substanceName)
                {
                    if (reactions.Any(r => r.Reactants.Any(rs => rs.Substance.SubstanceName == substanceName)
                      || r.Products.Any(rp => rp.Substance.SubstanceName == substanceName)))
                    {
                        MessageBox.Show($"{substanceName} используется в реакции");
                    }
                    else
                    {
                        substances.RemoveAt(i);
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                string selectedSubstance = listBox1.SelectedItem.ToString();
                RemoveSubstanceFromList(selectedSubstance);
            }
            ClearAndFillListBox1();
            clearAndFillCheckListBoxes();
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            // очищаем checkedListBox1 и listBox3aa
            tempReactatns.Clear();
            tempProducts.Clear();

            // получаем выбранную реакцию
            string reactionName = listBox2.SelectedItem.ToString();

            // проходимся по всем реакциям
            foreach (ChemicalReaction reaction in reactions)
            {
                // если название реакции совпадает с выбранным
                if (reaction.reactionName == reactionName)
                {
                    label1.Text = (string) reaction.ToString();
                }
            }
        }

        private void listBox3_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            string substanceName = listBox3.SelectedItem.ToString().Split(' ')[0];

            // находим соответствующий объект ChemicalSubstance
            ChemicalSubstance substance = substances.Find(x => x.SubstanceName == substanceName);

            // получаем коэффициент из textBox2
            int coefficient;
            
           
            if (Int32.TryParse(textBox2.Text, out coefficient) && !tempReactatns.Any(x => x.Substance.SubstanceName == substanceName))
            {
                // создаем объект ReactionSubstance
                ReactionSubstance reactionSubstance = new ReactionSubstance(substance, substance.MolarMass, coefficient);

                // добавляем объект в список reactionSubstances
                tempReactatns.Add(reactionSubstance);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            string substanceName = listBox4.SelectedItem.ToString().Split(' ')[0];

            // находим соответствующий объект ChemicalSubstance
            ChemicalSubstance substance = substances.Find(x => x.SubstanceName == substanceName);

            // получаем коэффициент из textBox2
            int coefficient = int.Parse(textBox3.Text);
            if (tempProducts.Any(x => x.Substance.SubstanceName == substanceName)) {

            } else
            {
                // создаем объект ReactionSubstance
            ReactionSubstance reactionSubstance = new ReactionSubstance(substance, substance.MolarMass, coefficient);

                // добавляем объект в список reactionSubstances
            tempProducts.Add(reactionSubstance);
            }
        }

        private void listBox4_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (listBox6.SelectedIndex == -1)
            {
                MessageBox.Show("Выберите название вещества!");
                return;
            }

            string substanceName = listBox6.SelectedItem.ToString();
            double molarMass=1;
            double mass;

            if (!double.TryParse(textBox8.Text, out mass) || mass <= 0)
            {
                MessageBox.Show("Введите корректное значение массы!");
                return;
            }

            // проверяем, что вещество с таким названием еще не добавлено
            if (inputProducts.Any(p => p.Substance.SubstanceName == substanceName))
            {
                MessageBox.Show("Вещество с таким названием уже добавлено!");
                return;
            }

            // создаем новый объект ReactionSubstance
            ChemicalSubstance newSubstanceTemp = substances.Find(x => x.SubstanceName == substanceName);
            ChemicalSubstance newSubstance = new ChemicalSubstance(newSubstanceTemp.SubstanceName, newSubstanceTemp.MolarMass);
            newSubstance.Mass = mass;

            // добавляем его в список inputProducts
            inputProducts.Add(new ReactionSubstance(newSubstance, molarMass, 1));

            // добавляем строку в dataGridView1
            dataGridView2.Rows.Add(substanceName, molarMass, mass);
        }

        private void button12_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите строку для удаления!");
                return;
            }

            // получаем индекс выбранной строки
            int rowIndex = dataGridView1.SelectedRows[0].Index;

            // получаем название вещества из выбранной строки
            string substanceName = dataGridView1.Rows[rowIndex].Cells[0].Value.ToString();

            // удаляем строку из dataGridView1
            dataGridView1.Rows.RemoveAt(rowIndex);

            // удаляем вещество из списка inputProducts
            inputReactants.RemoveAll(p => p.Substance.SubstanceName == substanceName);
        }

        private void button13_Click(object sender, EventArgs e)
        {
            if (dataGridView2.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите строку для удаления!");
                return;
            }

            // получаем индекс выбранной строки
            int rowIndex = dataGridView2.SelectedRows[0].Index;

            // получаем название вещества из выбранной строки
            string substanceName = dataGridView2.Rows[rowIndex].Cells[0].Value.ToString();

            // удаляем строку из dataGridView1
            dataGridView2.Rows.RemoveAt(rowIndex);

            // удаляем вещество из списка inputProducts
            inputProducts.RemoveAll(p => p.Substance.SubstanceName == substanceName);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ChemicalSubstanceStorage storage = new ChemicalSubstanceStorage(substances);
            ChemicalReactionStorage re = new ChemicalReactionStorage(reactions);
            InputDataSaver inputDataSaver = new InputDataSaver(inputReactants, inputProducts);
            inputProducts = inputDataSaver.LoadTargetSubstances();
            inputReactants = inputDataSaver.LoadStartSubstances();
            foreach (var prod in inputProducts)
            {
                dataGridView2.Rows.Add(prod.Substance.SubstanceName, prod.Substance.MolarMass, prod.Substance.Mass);
            }

            foreach (var react in inputReactants)
            {
                dataGridView1.Rows.Add(react.Substance.SubstanceName);
            }

            reactions = re.Load();
            substances = storage.Load();
            ClearAndFillListBox1();
            ClearAndFillListBox2();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (listBox2.SelectedItem != null)
            {
                // Получаем выбранную реакцию
                string selectedReactionName = listBox2.SelectedItem.ToString();
                ChemicalReaction selectedReaction = reactions.FirstOrDefault(r => r.reactionName == selectedReactionName);
                // Удаляем выбранную реакцию из списка реакций
                reactions.Remove(selectedReaction);

                // Обновляем содержимое listBox2
                listBox2.Items.Remove(selectedReaction);

                ClearAndFillListBox2();
                ChemicalReactionStorage re = new ChemicalReactionStorage(reactions);
                re.Save(); 
            }
        }

        private void listBox5_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button11_Click(object sender, EventArgs e)
        {
            try
            {
                if (!checkIsDataFull())
                {
                    MessageBox.Show("Проверьте полноту знаний");
                    return;
                }
                var ChemicalPathFinder = new ChemicalPathFinder(reactions);
                var Solution = ChemicalPathFinder.FindPathsToSubstances(inputReactants.ToHashSet(), inputProducts.ToHashSet(), substances);
                dataGridView3.Rows.Clear();
                dataGridView4.Rows.Clear();
                for (int i = 0; i < Solution.Reactions.Count; i++)
                {
                    dataGridView4.Rows.Add(i + 1, Solution.Reactions[i].reactionName, Solution.Reactions[i].ToString());
                }
                for (int i = 0; i < Solution.Substances.Count; i++)
                {
                    string subs = "";
                    foreach (var sb in Solution.Substances[i])
                    {
                        if (sb.isExecuting)
                        {
                            subs += sb.Substance.SubstanceName;
                            var prodMass = Solution.ProductMasses[i][sb.Substance.SubstanceName];
                            var reactMass = Solution.ReactantMasses[i][sb.Substance.SubstanceName];
                            double? mass = 0.0;
                            if (reactMass>prodMass)
                            {
                                mass = reactMass;
                            } else
                            {
                                mass = prodMass;
                            }
                            if (mass > 0)
                            {
                                double mass1 = (double) mass;
                                subs +=" " + Math.Round(mass1, 2) + " грамм";
                            }
                            subs += ",";
                        }
                    }
                    label5.Text = "Множество конечных веществ достижимо. Доказательство:";
                    dataGridView3.Rows.Add(i + 1, subs);
                }
                //Console.WriteLine(ChemicalPathFinder.FindPathsToSubstances(inputReactants.ToHashSet(), inputProducts.ToHashSet()));
                //Console.WriteLine(ChemicalPathFinder.FindPathsToSubstances(inputReactants.ToHashSet(), inputProducts.ToHashSet()));
            }
            catch (Exception ex)
            {
                MessageBox.Show("Не получилось составить план процесса. Скорее всего не хватает начальных веществ и/или реакций");
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView3_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            var InputDataSaver = new InputDataSaver(inputReactants, inputProducts);
            InputDataSaver.Save();
        }
    }
}
