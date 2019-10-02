using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NHunspell;
using DevExpress.XtraEditors.Controls;
using System.Threading;

namespace Worker
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void comboBoxEdit1_TextChanged(object sender, EventArgs e)
        {

            String[] a = 
                           { "Man", "Ban", "Pan", "Fan"};

            String[] b = { "Lot", "Pot", "Hot", "Shot" }
                         ;

            String[] c;

            ComboBoxItemCollection itemsCollection = comboBoxEdit1.Properties.Items;
                    itemsCollection.Clear();

                    if (comboBoxEdit1.Text.Length == 1) { c = a;}
                    else c = b;
  

            foreach (string str in c)
                {

                    itemsCollection.Add(str);
                }
            comboBoxEdit1.ShowPopup();

            //using (Hunspell hunspell = new Hunspell("en_us.aff", "en_us.dic"))
            //{
            //    textBox1.Text = "";
            //    List<string> suggestions = hunspell.Suggest(comboBoxEdit1.Text.ToString());
                
         
            //    foreach (string suggestion in suggestions)
            //    {
            //        textBox1.Text = textBox1.Text + suggestion + "\r\n";
            //    }

            //}
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            comboBoxEdit1.Properties.AutoComplete = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.demoThread =
                  new Thread(new ThreadStart(this.ThreadProcSafe));

            this.demoThread.Start();

        }

       

        delegate void SetTextCallback(string text);
        private Thread demoThread = null;


        private void ThreadProcSafe()
        {
            while (true)
            {
                this.SetText("Loading");
                Thread.Sleep(200);
                this.SetText("Loading.");
                Thread.Sleep(200);
                this.SetText("Loading..");
                Thread.Sleep(200);
                this.SetText("Loading...");
                Thread.Sleep(200);
            }
        }

        private void SetText(string text)
        {
            if (this.label1.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetText);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.label1.Text = text;
            }
        }
    }
}
