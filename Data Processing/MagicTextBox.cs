using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SQLite;

namespace Anindya_s_Dictionary
{
    public partial class MagicTextBox : Form
    {
        public MagicTextBox()
        {
            InitializeComponent();
         
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

            if (listBox1.Visible)
            {

                SQLiteConnection conn = new SQLiteConnection(AppConfig.DB_CONNECTION_STRING);
                conn.Open();

                string sql = String.Format("SELECT word FROM dictionary_words WHERE word LIKE '{0}%' LIMIT 500", textBox1.Text.Trim());

                SQLiteDataAdapter da = new SQLiteDataAdapter(sql, conn);

                DataTable dt = new DataTable();
                da.Fill(dt);
                conn.Close();
                List<string> words = new List<string>();

                foreach (DataRow dr in dt.Rows)
                {
                    words.Add(dr["word"].ToString());
                }
                listBox1.Items.Clear();
                listBox1.Items.AddRange(words.ToArray());
            }

            if (listBox1.Items.Count == 0) listBox1.Visible = false;
           
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                if (listBox1.SelectedIndex >= 0)
                {
                    textBox1.Text = listBox1.SelectedItem.ToString();
                    textBox1.Select(textBox1.TextLength, 0);

                }
                listBox1.SelectedIndex = -1;
                listBox1.Visible = false;
            }

            else if (e.KeyChar == (char)Keys.Escape)
            {
                listBox1.SelectedIndex = -1;
                listBox1.Visible = false;
            }

            else listBox1.Visible = true;
            
        }

       
        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == (char)Keys.Down)
            {
                if (listBox1.Items.Count > 0)
                {
                    listBox1.Visible = true;
                    if (listBox1.SelectedIndex < listBox1.Items.Count - 1) listBox1.SelectedIndex += 1;
                }
                
                e.Handled = true;

            }
            else if (e.KeyValue == (char)Keys.Up)
            {
                if (listBox1.Items.Count > 0)
                {
                    //listBox1.Visible = true;
                    if (listBox1.SelectedIndex < listBox1.Items.Count - 1) listBox1.SelectedIndex -= 1;
                    if (listBox1.SelectedIndex == -1) listBox1.Visible = false;
                }

                e.Handled = true;
            }

           

          
            
           
        }

        private void textBox1_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
           
        }


    }
}
