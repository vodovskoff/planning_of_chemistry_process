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
            double molarMass;
            double mass;

            // проверяем, что введены корректные значения молярной массы и массы
            if (!double.TryParse(textBox6.Text, out molarMass) || molarMass <= 0)
            {
                MessageBox.Show("Введите корректное значение молярной массы!");
                return;
            }
            if (!double.TryParse(textBox7.Text, out mass) || mass <= 0)
            {
                MessageBox.Show("Введите корректное значение массы!");
                return;
            }

            // проверяем, что вещество с таким названием еще не добавлено
            if (inputReactants.Any(p => p.Substance.Substance == substanceName))
            {
                MessageBox.Show("Вещество с таким названием уже добавлено!");
                return;
            }

            // создаем новый объект ReactionSubstance
            ChemicalSubstance newSubstance = substances.Find(x => x.Substance == substanceName);
            newSubstance.MolarMass = molarMass;
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
            if (!string.IsNullOrWhiteSpace(textBox1.Text))
            {
                string newSubstance = textBox1.Text.Trim();

                if (!substances.Any(s => s.Substance == newSubstance))
                {
                    substances.Add(new ChemicalSubstance(newSubstance, null));
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
                string newSubstance = textBox5.Text.Trim();

                if (!substances.Any(s => s.Substance == newSubstance))
                {
                    int index = listBox1.SelectedIndex;
                    substances[index].Substance = newSubstance;
                    listBox1.Items[index] = newSubstance;
                } else
                {
                    MessageBox.Show($"{newSubstance} Уже существует");
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
                if (checkedListBox1.CheckedItems.Contains(reactant.Substance.Substance))
                {
                    reactants.Add(reactant);
                }
            }

            HashSet<ReactionSubstance> products = new HashSet<ReactionSubstance>();
            foreach (var product in tempProducts)
            {
                if (checkedListBox2.CheckedItems.Contains(product.Substance.Substance))
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
            clearAndFillCheckListBoxes();
        }

        private void clearAndFillCheckListBoxes()
        {
            checkedListBox1.Items.Clear();
            checkedListBox2.Items.Clear();

            foreach (ChemicalSubstance substance in substances)
            {
                checkedListBox1.Items.Add(substance.Substance);
                checkedListBox2.Items.Add(substance.Substance);
            }
        }

        private void ClearAndFillListBox2()
        {
            listBox2.Items.Clear();
            foreach (ChemicalReaction reaction in reactions)
            {
                listBox2.Items.Add(reaction.reactionName);
            }
        }

        private void ClearAndFillListBox1()
        {
            listBox1.Items.Clear();
            listBox5.Items.Clear();
            listBox6.Items.Clear();
            ChemicalSubstanceStorage storage = new ChemicalSubstanceStorage(substances);
            storage.Save();
            foreach (ChemicalSubstance substance in substances)
            {
                listBox1.Items.Add(substance.Substance);
                listBox5.Items.Add(substance.Substance);
                listBox6.Items.Add(substance.Substance);
            }
        }

        private void RemoveSubstanceFromList(string substanceName)
        {
            for (int i = substances.Count - 1; i >= 0; i--)
            {
                if (substances[i].Substance == substanceName)
                {
                    if (reactions.Any(r => r.Reactants.Any(rs => rs.Substance.Substance == substanceName)
                      || r.Products.Any(rp => rp.Substance.Substance == substanceName)))
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
            string substanceName = listBox3.SelectedItem.ToString();

            // находим соответствующий объект ChemicalSubstance
            ChemicalSubstance substance = substances.Find(x => x.Substance == substanceName);

            // получаем коэффициент из textBox2
            int coefficient = int.Parse(textBox2.Text);
            if (!tempReactatns.Any(x => x.Substance.Substance == substanceName))
            {
                // создаем объект ReactionSubstance
                ReactionSubstance reactionSubstance = new ReactionSubstance(substance, substance.MolarMass, coefficient);

                // добавляем объект в список reactionSubstances
                tempReactatns.Add(reactionSubstance);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            string substanceName = listBox4.SelectedItem.ToString();

            // находим соответствующий объект ChemicalSubstance
            ChemicalSubstance substance = substances.Find(x => x.Substance == substanceName);

            // получаем коэффициент из textBox2
            int coefficient = int.Parse(textBox3.Text);
            if (tempProducts.Any(x => x.Substance.Substance == substanceName)) {

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
            double molarMass;
            double mass;

            // проверяем, что введены корректные значения молярной массы и массы
            if (!double.TryParse(textBox9.Text, out molarMass) || molarMass <= 0)
            {
                MessageBox.Show("Введите корректное значение молярной массы!");
                return;
            }
            if (!double.TryParse(textBox8.Text, out mass) || mass <= 0)
            {
                MessageBox.Show("Введите корректное значение массы!");
                return;
            }

            // проверяем, что вещество с таким названием еще не добавлено
            if (inputProducts.Any(p => p.Substance.Substance == substanceName))
            {
                MessageBox.Show("Вещество с таким названием уже добавлено!");
                return;
            }

            // создаем новый объект ReactionSubstance
            ChemicalSubstance newSubstance = substances.Find(x => x.Substance == substanceName);
            newSubstance.MolarMass = molarMass;
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
            inputReactants.RemoveAll(p => p.Substance.Substance == substanceName);
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
            inputProducts.RemoveAll(p => p.Substance.Substance == substanceName);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ChemicalSubstanceStorage storage = new ChemicalSubstanceStorage(substances);
            ChemicalReactionStorage re = new ChemicalReactionStorage(reactions);
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
                var ChemicalPathFinder = new ChemicalPathFinder(reactions);
                var Solution = ChemicalPathFinder.FindPathsToSubstances(inputReactants.ToHashSet(), inputProducts.ToHashSet());
                dataGridView3.Rows.Clear();
                dataGridView4.Rows.Clear();
                for (int i = 0; i < Solution.Reactions.Count; i++)
                {
                    dataGridView4.Rows.Add(i + 1, Solution.Reactions[i].reactionName, Solution.Reactions[i].ToString());
                }
                for (int i = 0; i < Solution.Substances.Count; i++)
                {
                    dataGridView3.Rows.Add(i + 1,
                        string.Join(", ",
                        Solution.Substances[i].Select(rs => rs.isExecuting ? $"{rs.Substance.Substance}" : "")));
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
    }
}
