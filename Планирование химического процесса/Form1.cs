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
            clearAndFillListBoxes();
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
                }

                textBox5.Clear();
            }
            ClearAndFillListBox1();
            clearAndFillListBoxes();
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
            clearAndFillListBoxes();
        }

        private void clearAndFillListBoxes()
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

            foreach (ChemicalSubstance substance in substances)
            {
                listBox1.Items.Add(substance.Substance);
            }
        }

        private void RemoveSubstanceFromList(string substanceName)
        {
            foreach (ChemicalSubstance substance in substances)
            {
                if (substance.Substance == substanceName)
                {
                    substances.Remove(substance);
                    break;
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
            clearAndFillListBoxes();
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            // очищаем checkedListBox1 и listBox3
            checkedListBox1.Items.Clear();
            listBox3.Items.Clear();
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
                    checkedListBox1.Items.Clear();
                    checkedListBox2.Items.Clear();
                    listBox3.Items.Clear();
                    listBox4.Items.Clear();
                    // добавляем все реагенты в checkedListBox1
                    foreach (ReactionSubstance substance in reaction.Reactants)
                    {
                        checkedListBox1.Items.Add(substance.Substance);
                    }

                    // добавляем все реагенты в listBox3
                    foreach (ReactionSubstance substance in reaction.Reactants)
                    {
                        listBox3.Items.Add(substance.Substance);
                    }
                    foreach (ReactionSubstance substance in reaction.Products)
                    {
                        checkedListBox2.Items.Add(substance.Substance);
                    }

                    // добавляем все реагенты в listBox3
                    foreach (ReactionSubstance substance in reaction.Products)
                    {
                        listBox4.Items.Add(substance.Substance);
                    }

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
            if (tempReactatns.Any(x => x.Substance.Substance == substanceName))
            {

            }
            else
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
    }
}
