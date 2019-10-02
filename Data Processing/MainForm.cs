/*
 * Created by SharpDevelop.
 * User: Anindya
 * Date: 6/6/2013
 * Time: 9:51 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Globalization;
using NHunspell;
using System.Threading;

namespace Worker
{
    /// <summary>
    /// Description of MainForm.
    /// </summary>
    public partial class MainForm : Form
    {
        public MainForm()
        {
            //
            // The InitializeComponent() call is required for Windows Forms designer support.
            //
            InitializeComponent();

            //
            // TODO: Add constructor code after the InitializeComponent() call.
            //
        }

        void Button1Click(object sender, EventArgs e)
        {



            //DirectoryInfo di = new DirectoryInfo(@"E:\AllGoogleTranslate");
            //foreach (FileInfo fi in di.GetFiles())
            //{
            //    string st = fi.Name.Substring(0, 1);

            //    File.Move(fi.FullName, @"E:\AllGoogleTranslate\" + st + @"\" + fi.Name); 
            //}


            string[] words = File.ReadAllLines(@"E:\C# Projects\Anindya's Dictionary Assets\Wordlists\wordsEn.txt");

            foreach (string word in words)
            {

                string url = "http://translate.google.com/translate_a/t?client=j&text=" + word + "&hl=en&sl=en&tl=bn";


                using (WebClient wc = new WebClient())
                {
                    wc.Encoding = Encoding.UTF8;
                    try
                    {
                        if (!File.Exists(@"E:\AllGoogleTranslate\" + word.Substring(0, 1) + @"\" + word + ".txt"))
                            File.WriteAllText(@"E:\AllGoogleTranslate\" + word.Substring(0, 1) + @"\" + word + ".txt", wc.DownloadString(url), Encoding.UTF8);
                    }
                    catch (Exception ex)
                    {
                        File.AppendAllLines(@"E:\AllGoogleTranslate\error.txt", new string[] { word }, Encoding.UTF8);
                        // MessageBox.Show(ex.Message);
                    }
                }


            }



        }

        void Button2Click(object sender, EventArgs e)
        {
            Regex rx = new Regex(@"\\[uU]([0-9A-F]{4})");

            DirectoryInfo di = new DirectoryInfo(@"E:\AllGoogleTranslate");

            foreach (DirectoryInfo di2 in di.GetDirectories())
            {
                foreach (FileInfo fi in di2.GetFiles())
                {
                    string content = File.ReadAllText(fi.FullName, Encoding.UTF8);

                    content = rx.Replace(content, delegate(Match match) { return ((char)Int32.Parse(match.Value.Substring(2), NumberStyles.HexNumber)).ToString(); });


                    try
                    {
                        File.WriteAllText(@"E:\PurifiedGoogleTranslate\" + fi.Name.Substring(0, 1) + @"\" + fi.Name, content, Encoding.UTF8);
                    }
                    catch
                    {
                        File.AppendAllLines(@"E:\PurifiedGoogleTransError.txt\n", new string[] { fi.Name }, Encoding.UTF8);

                    }


                }
            }
        }

        void Button3Click(object sender, EventArgs e)
        {
            string[] words = File.ReadAllLines(@"E:\C# Projects\Anindya's Dictionary Assets\Wordlists\wordsEn.txt");

            foreach (string word in words)
            {

                string url = "http://www.google.com/dictionary/json?callback=dict_api.callbacks.id100&q=" + word + "&sl=en&tl=en&restrict=pr%2Csy&client=te";



                using (WebClient wc = new WebClient())
                {
                    wc.Encoding = Encoding.UTF8;
                    try
                    {
                        if (!File.Exists(@"E:\AllGoogleDictionary\" + word.Substring(0, 1) + @"\" + word + ".txt"))
                        {

                            string json = wc.DownloadString(url);

                            Dictionary<string, string> replace = new Dictionary<string, string>();

                            replace.Add("\\x3c", "<");
                            replace.Add("\\x3e", ">");
                            replace.Add("\\x26", "&");
                            replace.Add("\\x22", "\"");
                            replace.Add("\\x27", "'");
                            replace.Add("\\x3d", "=");

                            json = json.Substring(json.IndexOf('{'), json.LastIndexOf('}') - json.IndexOf('{') + 1);

                            foreach (string rep in replace.Keys)
                            {
                                json = json.Replace(rep, replace[rep]);
                            }


                            File.WriteAllText(@"E:\AllGoogleDictionary\" + word.Substring(0, 1) + @"\" + word + ".txt", json, Encoding.UTF8);
                        }
                    }
                    catch (Exception ex)
                    {
                        File.AppendAllLines(@"E:\AllGoogleDictionary\error.txt", new string[] { word }, Encoding.UTF8);
                        MessageBox.Show(ex.Message);
                    }
                }


            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {


        }


        protected override void WndProc(ref Message msg)
        {
            /*
            switch (msg.Msg)
            {
                // If message is of interest, invoke the method on the form that 
                // functions as a callback to perform actions in response to the message. 
                case 0x201:
                    MessageBox.Show("Hi");
                    
                    break;

            }
            // Call the base WndProc method 
            // to process any messages not handled. 
            base.WndProc(ref msg);
             */
        }

        private void comboBoxEdit1_TextChanged(object sender, EventArgs e)
        {
            

            using (Hunspell hunspell = new Hunspell("en_us.aff", "en_us.dic"))
            {

                List<string> suggestions = hunspell.Suggest(comboBoxEdit1.Text.ToString());

                foreach (string suggestion in suggestions)
                {
                    textBox1.Text = textBox1.Text + suggestion + "\n";
                }

            }
        }


    }


}

