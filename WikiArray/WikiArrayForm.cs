// Kaelan Oxby
// P457653
// Assessment One


using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WikiArray
{
    
    public partial class WikiArrayForm : Form
    {
        
        public WikiArrayForm()
        {
          
           InitializeComponent();

        }

        static int maxRows = 12;
        static int maxCols = 4;
        int arrayMax = 0;
        string[,] wikiArray = new string[maxRows, maxCols];

        // Add Item to the wikiArray
        private void AddToWiki()
        {
            if (arrayMax < maxRows)
            {
                if(!string.IsNullOrEmpty(textBoxName.Text))
                {
                    wikiArray[arrayMax, 0] = textBoxName.Text;
                    wikiArray[arrayMax, 1] = textBoxCategory.Text;
                    wikiArray[arrayMax, 2] = textBoxStructure.Text;
                    wikiArray[arrayMax, 3] = textBoxDefinition.Text;
                    ClearTextFields();
                    arrayMax++;
                    
                }
                else
                {
                    textBoxDefinition.Text = "You have to enter some info first";
                }
            }
            else
            {
                textBoxDefinition.Text = "The array is full";
            }
        }

        // Save wikiArray to a binary file
        private void SaveBinary()
        {
            BinaryWriter bw;
            try
            {
                bw = new BinaryWriter(new FileStream("definitions.dat", FileMode.Create));
            }
            catch (Exception fe)
            {
                MessageBox.Show(fe.Message + "\n Could not save to file");
                return;
            }
            try
            {
                for (int i = 0; i < arrayMax; i++)
                {
                    bw.Write(wikiArray[i, 0]);
                    bw.Write(wikiArray[i, 1]);
                    bw.Write(wikiArray[i, 2]);
                    bw.Write(wikiArray[i, 3]);
                }
            }
            catch (Exception fe)
            {
                MessageBox.Show(fe.Message + "\n Could not write to file");
                return;
            }
            bw.Close();

        }

        // Load the information from the binary file and add it to the wikiArray
        private void LoadBinary()
        {
            BinaryReader br;
            int x = 0;
            listBoxArray.Items.Clear();
            try
            {
                br = new BinaryReader(new FileStream("definitions.dat", FileMode.Open));
            }
            catch (Exception fe)
            {
                MessageBox.Show(fe.Message + "\n Cannot open file for reading");
                return;
            }
            while (br.BaseStream.Position != br.BaseStream.Length)
            {
                try
                {
                    wikiArray[x, 0] = br.ReadString();
                    wikiArray[x, 1] = br.ReadString();
                    wikiArray[x, 2] = br.ReadString();
                    wikiArray[x, 3] = br.ReadString();
                    x++;
                }
                catch (Exception fe)
                {
                    MessageBox.Show("Cannot read data from file or EOF" + fe);
                    break;
                }
                arrayMax = x;
                
            }
            br.Close();
        }

        // Display the items from the wikiArray in the listBox
        private void DisplayWikiArray()
        {
            listBoxArray.Items.Clear();
            string rec = "";
            for (int x = 0; x < maxRows; x++)
            {
                rec = wikiArray[x, 0] + "\t" + wikiArray[x, 1] + "\t" + wikiArray[x, 2];
                listBoxArray.Items.Add(rec);
            }
            
        }

        // Select an item from the listBox.
        private void SelectedItem()
        {
            if (listBoxArray.SelectedIndex >= 0)
            {
                string currentItem = listBoxArray.SelectedItem.ToString();
                int indx = listBoxArray.FindString(currentItem);
                textBoxName.Text = wikiArray[listBoxArray.SelectedIndex, 0];
                textBoxCategory.Text = wikiArray[listBoxArray.SelectedIndex, 1];
                textBoxStructure.Text = wikiArray[listBoxArray.SelectedIndex, 2];
                textBoxDefinition.Text = wikiArray[listBoxArray.SelectedIndex, 3];
            }
            else
            {
                textBoxDefinition.Text = "You have to select something to edit first";
            }
            
        }

        // Select item from the listbox and override the original array item
        private void Edit()
        {
            if (listBoxArray.SelectedIndex >= 0)
            {
                string currentItem = listBoxArray.SelectedItem.ToString();
                int indx = listBoxArray.FindString(currentItem);
                wikiArray[listBoxArray.SelectedIndex, 0] = textBoxName.Text;
                wikiArray[listBoxArray.SelectedIndex, 1] = textBoxCategory.Text;
                wikiArray[listBoxArray.SelectedIndex, 2] = textBoxStructure.Text;
                wikiArray[listBoxArray.SelectedIndex, 3] = textBoxDefinition.Text;
            } else
            {
                textBoxDefinition.Text = "You have to select something to edit first";
            }

        }

        // Gets the index of the element from listbox and moves every element up 1 and returns null for last element.
        private void Remove()
        {
            if (listBoxArray.SelectedItem == null)
            {
                textBoxDefinition.Text = "You have to select something to edit first";
            }
            else
            {
                DialogResult DeleteTask = MessageBox.Show("Are you sure you want to Delete this task?", "Confirmation",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (DeleteTask == DialogResult.Yes)
                {
                    string currentItem = listBoxArray.SelectedItem.ToString();
                    int indx = listBoxArray.FindString(currentItem);
                    for (int i = indx; i < arrayMax - 1; i++)
                    {
                        wikiArray[i, 0] = wikiArray[i + 1, 0];
                        wikiArray[i, 1] = wikiArray[i + 1, 1];
                        wikiArray[i, 2] = wikiArray[i + 1, 2];
                        wikiArray[i, 3] = wikiArray[i + 1, 3];

                    }
                    wikiArray[arrayMax - 1, 0] = null;
                    wikiArray[arrayMax - 1, 1] = null;
                    wikiArray[arrayMax - 1, 2] = null;
                    wikiArray[arrayMax - 1, 3] = null;
                    arrayMax--;
                } else
                {
                    MessageBox.Show("You have chosen not to delete this Item");
                }
                    
            }

        }

        // Clear all the text fields
        private void ClearTextFields()
        {
            textBoxName.Clear();
            textBoxCategory.Clear();    
            textBoxStructure.Clear();
            textBoxDefinition.Clear();
            
        }

        // Sort the wikiArray by name
        private void Sort()
        {
            for (int x = 1; x < maxRows; x++)
            {
                for (int i = 0; i < maxRows - 1; i++)
                {
                    if (!(string.IsNullOrEmpty(wikiArray[i + 1, 0])))
                    {
                        if (string.Compare(wikiArray[i, 0], wikiArray[i + 1, 0]) == 1)
                        {
                            Swap(i);
                        }
                    }
                }
            }
        }

        // Swap Method
        private void Swap(int indx)
        {
            string temp;

            for (int i = 0; i < maxCols; i++)
            {
                temp = wikiArray[indx, i];
                wikiArray[indx, i] = wikiArray[indx + 1, i];
                wikiArray[indx + 1, i] = temp;
            }
        }

        // Binary Search of the wikiArray
        private void Search()
        {
            int startIndex = -1;
            int finalIndex = arrayMax;
            bool flag = false;
            int foundIndex = -1;

            while (!flag && !((finalIndex - startIndex) <= 1))
            {
                int newIndex = (finalIndex + startIndex) / 2;
                if (string.Compare(wikiArray[newIndex, 0], textBoxSearch.Text) == 0) 
                {
                    foundIndex = newIndex;
                    flag = true;
                    break;
                }
                else
                {
                    if (string.Compare(wikiArray[newIndex, 0], textBoxSearch.Text) == 1)
                        finalIndex = newIndex;
                    else
                        startIndex = newIndex;
                }
            }
            if (flag)
            {
                textBoxName.Text = wikiArray[foundIndex, 0];
                textBoxCategory.Text = wikiArray[foundIndex, 1];
                textBoxStructure.Text = wikiArray[foundIndex, 2];
                textBoxDefinition.Text = wikiArray[foundIndex, 3];
            }
            else
            {
                textBoxDefinition.Text = "Not found ";
            }
        }


        private void textBoxSearch_DoubleClick(object sender, EventArgs e)
        {
            textBoxSearch.Clear();

        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            SaveBinary();
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            AddToWiki();
            Sort();
            DisplayWikiArray();
            ClearTextFields();
        }

        private void buttonOpen_Click(object sender, EventArgs e)
        {
            LoadBinary();
            DisplayWikiArray();
        }

        private void listBoxArray_MouseClick(object sender, MouseEventArgs e)
        {
            SelectedItem();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearTextFields();
            listBoxArray.ClearSelected();
        }

        private void buttonSearch_Click(object sender, EventArgs e)
        {
            ClearTextFields();
            listBoxArray.ClearSelected();
            Search();
            textBoxSearch.Clear();
            


        }

        private void listBoxArray_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void buttonEdit_Click(object sender, EventArgs e)
        {
            Edit();
            DisplayWikiArray();
            ClearTextFields();
        }

        private void buttonRemove_Click(object sender, EventArgs e)
        {
            Remove();
            Sort();
            DisplayWikiArray();
            ClearTextFields();
        }
    }
}
