using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace Anindya_s_Dictionary
{
    public partial class TestForm2 : Form
    {
        string[] words;
        DirectoryInfo di = new DirectoryInfo(@"D:\Dictionary Resources\Brac E2B");
            
        public TestForm2()
        {
            InitializeComponent();
            words = File.ReadAllLines(@"C:\Users\Anindya\Desktop\BracNotFound.txt");

            DirectoryInfo di = new DirectoryInfo(@"D:\Dictionary Resources\Brac E2B\Missing");
            foreach(FileInfo fi in di.GetFiles()){
                listBox1.Items.Add(fi.Name.Substring(0, fi.Name.LastIndexOf('.')));
            }


            //listBox1.Items.AddRange(words);
        }

        private void button1_Click(object sender, EventArgs e)
        {
           



           
        }

        private void nextButton_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex < listBox1.Items.Count - 1)
                listBox1.SelectedIndex++;
            else listBox1.SelectedIndex = 0;
        }

        private void prevButton_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex > 0)
                listBox1.SelectedIndex--;
            else listBox1.SelectedIndex = listBox1.Items.Count - 1;
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            wordTextBox.Text = listBox1.SelectedItem.ToString();


            //foreach (FileInfo fi in di.GetFiles())
            //{
            //    string content = File.ReadAllText(fi.FullName);

            //    if (content.Contains(String.Format("{0} [", wordTextBox.Text)))
            //    {
            //        meaningTextBox.Text = content;
            //        break;
                    
            //    }
            //}


                
                string meaning = File.ReadAllText(@"D:\Dictionary Resources\Brac E2B\Missing\" + listBox1.SelectedItem + ".txt", Encoding.UTF8);
                meaning = meaning.Replace("<br /><br>", "");

                meaningTextBox.Text = meaning; 
        }

        private void button2_Click(object sender, EventArgs e)
        {

            foreach (string word in words)
            {
                foreach (FileInfo fi in di.GetFiles())
                {
                    string content = File.ReadAllText(fi.FullName);

                    if (content.Contains(String.Format("{0} [", word)))
                    {
                        File.WriteAllText(@"D:\Dictionary Resources\Brac E2B\Missing\" + word + ".txt", content);
                        break;
                    }
                }

            }

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            File.WriteAllText(@"D:\Dictionary Resources\Brac E2B\Manual\" + listBox1.SelectedItem + ".txt", meaningTextBox.SelectedText.Trim(), Encoding.UTF8);
            listBox1.SelectedIndex += 1;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (File.Exists(@"D:\Dictionary Resources\Brac E2B\Manual\" + listBox1.SelectedItem + ".txt"))
            {
                File.Delete(@"D:\Dictionary Resources\Brac E2B\Manual\" + listBox1.SelectedItem + ".txt");
            }
        }






    }
}
