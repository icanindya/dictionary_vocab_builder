using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Data.SQLite;
using System.IO;
using System.Speech.Synthesis;
using System.Text.RegularExpressions;

using System.Runtime.InteropServices;
using System.Net;


namespace GRE_Vocab_Builder
{
    public partial class GRE_Wordlist : DevExpress.XtraEditors.XtraForm
    {
        [ComVisible(true)]
        public class ScriptManager
        {
            // Variable to store the form of type Form1.
            private GRE_Wordlist mForm;

            // Constructor.
            public ScriptManager(GRE_Wordlist form)
            {
                // Save the form so it can be referenced later.
                mForm = form;
            }

            // This method can be called from JavaScript.
            public void changeRate(string word, int rating)
            {
                SQLiteConnection conn = new SQLiteConnection(AppConfig.dbConnString);

                string sql = String.Format("UPDATE grewords SET rating = {0} WHERE word = '{1}'", rating, word);

                SQLiteCommand cmd = new SQLiteCommand(sql, conn);

                conn.Open();

                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    conn.Close();
                }
            }

            // This method can also be called from JavaScript.
            public void AnotherMethod(string message)
            {
                MessageBox.Show(message);
            }
        }




        string currentWord;

        

        string htmlBody = string.Empty;

        public GRE_Wordlist()
        {


            
            InitializeComponent();
           
            
        }

        private void GRE_Wordlist_Load(object sender, EventArgs e)
        {
            webBrowser1.ObjectForScripting = new ScriptManager(this);

            webBrowser1.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(greDocReady);
            webBrowser1.Navigate(AppConfig.greHtmlPath);
            
        }


        private void greDocReady(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            
            HtmlElement head = webBrowser1.Document.GetElementsByTagName("head")[0];
            HtmlElement scriptEl = webBrowser1.Document.CreateElement("script");
            scriptEl.SetAttribute("text", "function disableSelection(){document.body.onselectstart=function(){return false;}; " + "document.body.ondragstart=function(){return false}}");
            head.AppendChild(scriptEl);
            webBrowser1.Document.InvokeScript("disableSelection");

            loadGreWordlistBox();
            
        }

        int offset = 0;

        private void loadGreWordlistBox()
        {

            if (detailedViewCheckItem.Checked)
            {
                greWordListBox.Items.Clear();

                var conn = new SQLiteConnection(AppConfig.dbConnString);
                conn.Open();

                string sql = "SELECT DISTINCT word FROM grewords";

                SQLiteDataAdapter da = new SQLiteDataAdapter(sql, conn);

                conn.Close();

                DataTable dt = new DataTable();
                da.Fill(dt);

                List<string> greWords = new List<string>();


                foreach (DataRow dr in dt.Rows)
                {
                    greWords.Add(dr["word"].ToString());
                }
                greWordListBox.Items.AddRange(greWords.ToArray());
            }
            else
            {
                //MessageBox.Show("HI");
                //greWordListBox.Items.Clear();
                
                //for (int i = 0; i < 26; i++)
                //{
                //    greWordListBox.Items.Add("Wordlist " + (char)((int)'A' + i));
                //}


                greWordListBox.Items.Clear();

                var conn = new SQLiteConnection(AppConfig.dbConnString);
                conn.Open();

                string sql = "SELECT word FROM grewords";

                SQLiteDataAdapter da = new SQLiteDataAdapter(sql, conn);
                conn.Close();
                DataTable dt = new DataTable();
                da.Fill(dt);

                List<string> greWords = new List<string>();



                for (int i = 0; i < dt.Rows.Count; i += 20)
                {
                    if(i < dt.Rows.Count - 20)
                        greWords.Add(dt.Rows[i]["word"] + " - " + dt.Rows[i + 19]["word"]);
                    else greWords.Add(dt.Rows[i]["word"] + " - " + dt.Rows[dt.Rows.Count - 1]["word"]);
                    
                }


                greWordListBox.Items.AddRange(greWords.ToArray());
            }

            greWordListBox.SelectedIndex = 0;
            showWordNumber();
        }

        private void greWordListBoxControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (greWordListBox.SelectedIndex < 0) return;

            htmlBody = string.Empty;
            wordTextBox.Text = currentWord = greWordListBox.SelectedItem.ToString();
                

            if (detailedViewCheckItem.Checked)
            {

                string word = currentWord;

                var conn = new SQLiteConnection(AppConfig.dbConnString);
                conn.Open();


                htmlBody += wrapWordSection(word);


               // htmlBody += wrapPictionarySection();


                string sql = String.Format("SELECT * FROM grewords_bangla WHERE word = '{0}'", currentWord);
                SQLiteCommand cmd = new SQLiteCommand(sql, conn);

                SQLiteDataAdapter da = new SQLiteDataAdapter();
                da.SelectCommand = cmd;
                DataTable dt = new DataTable();

                da.Fill(dt);
                string bangla = string.Empty;
                foreach (DataRow dr in dt.Rows)
                {
                    string bangPos = dr["pos"].ToString();
                    string bangMeaning = dr["meaning"].ToString();

                    bangla += wrapBangPos(bangPos) + wrapBangMeaning(bangMeaning);

                }
                htmlBody += wrapBanglaSection(bangla);


                sql = String.Format("SELECT * FROM grewords WHERE word = '{0}'", currentWord);
                cmd = new SQLiteCommand(sql, conn);
                da = new SQLiteDataAdapter();
                da.SelectCommand = cmd;


                dt = new DataTable();
                da.Fill(dt);

                string barron = string.Empty;

                foreach (DataRow dr in dt.Rows)
                {
                    string barMeaning = dr["meaning"].ToString();
                    string barSentence = dr["sentence"].ToString();

                    barron += wrapBarMeaning(barMeaning) + wrapBarSentence(barSentence);

                }
                htmlBody += wrapBarronSection(barron);


                sql = String.Format("SELECT * FROM mnemonic WHERE word = '{0}'", currentWord);
                cmd = new SQLiteCommand(sql, conn); da = new SQLiteDataAdapter();
                da.SelectCommand = cmd;


                dt = new DataTable();
                da.Fill(dt);

                string mnemonics = string.Empty;

                foreach (DataRow dr in dt.Rows)
                {
                    string mnemonic = dr["mnemonic"].ToString();

                    mnemonics += wrapMnemonic(mnemonic);
                }

                htmlBody += wrapMnemonicSection(mnemonics);


                sql = String.Format("SELECT * FROM syn_ant WHERE word = '{0}'", currentWord);
                cmd = new SQLiteCommand(sql, conn);
                da = new SQLiteDataAdapter();
                da.SelectCommand = cmd;


                dt = new DataTable();
                da.Fill(dt);

                string thesaurus = string.Empty;

                foreach (DataRow dr in dt.Rows)
                {
                    string pos = dr["pos"].ToString();
                    string synonym = dr["syn"].ToString();
                    string antonym = dr["ant"].ToString();
                    thesaurus += wrapSynonym(synonym) + wrapAntonym(antonym);
                }

                htmlBody += wrapThesaurusSection(thesaurus);


                sql = String.Format("SELECT * FROM oxford_sentence WHERE word = '{0}'", currentWord);
                cmd = new SQLiteCommand(sql, conn);
                da = new SQLiteDataAdapter();
                da.SelectCommand = cmd;


                dt = new DataTable();
                da.Fill(dt);

                string sentences = string.Empty;

                foreach (DataRow dr in dt.Rows)
                {
                    string extraSentence = dr["sentence"].ToString();
                    sentences += wrapExtraSentence(extraSentence);
                }

                htmlBody += wrapSentencesSection(sentences);


                //sql = String.Format("SELECT rate FROM grewords_rating WHERE word = '{0}'", currentWord);
                //// MessageBox.Show(sql);
                //cmd = new SQLiteCommand(sql, conn);

                //da = new SQLiteDataAdapter();
                //da.SelectCommand = cmd;
                //dt = new DataTable();
                //da.Fill(dt);
                //conn.Close();

                //if (dt.Rows.Count > 0)
                //{

                //    rater1.CurrentRating = Int16.Parse(dt.Rows[0]["rate"].ToString());
                //}
                //else
                //{
                //    rater1.CurrentRating = 0;
                //}

            }
            else
            {



                offset = (greWordListBox.SelectedIndex) * 20;

                string list = greWordListBox.SelectedItem.ToString();
                string letter = list.Substring(list.Length - 1, 1).ToLower();


                SQLiteConnection conn = new SQLiteConnection(AppConfig.dbConnString);
                string sql = String.Format("SELECT DISTINCT grewords.word, grewords.rating, grewords_bangla.meaning, grewords.meaning, grewords.sentence FROM grewords JOIN grewords_bangla ON grewords.word = grewords_bangla.word WHERE grewords_bangla.meaning = (SELECT meaning FROM grewords_bangla WHERE word = grewords.word LIMIT 1) LIMIT 20 OFFSET {0}", offset);
                SQLiteCommand cmd = new SQLiteCommand(sql, conn);

                SQLiteDataAdapter da = new SQLiteDataAdapter();
                da.SelectCommand = cmd;
                DataTable dt = new DataTable();

                da.Fill(dt);
                string row = string.Empty;

                foreach (DataRow dr in dt.Rows)
                {
                    //string bangPos = dr["pos"].ToString();
                    string word = dr[0].ToString();
                    string rate = dr[1].ToString();
                    string bangMeaning = dr[2].ToString();
                    string barMeaning = dr[3].ToString();
                    string barSentence = dr[4].ToString();

                    row += wrapCompWord(word, rate) + wrapCompBangMeaning(bangMeaning) + wrapCompBarMeaning(barMeaning) + wrapCompBarSentence(barSentence);

                }
                htmlBody += row;

            }

            
           webBrowser1.Document.InvokeScript("showContent", new Object[] { htmlBody });
            
            showWordNumber();

            
        }

        private void showWordNumber(){
            wordNumberLabel.Text = String.Format("Showing {0} of {1}", greWordListBox.SelectedIndex + 1, greWordListBox.Items.Count);
        }


        private string wrapWordSection(string str)
        {
            return "<div class=\"section\"><div class=\"heading\">Word</div><div class=\"word\">" + str + "</div></div>";
        }

        public string wrapPictionarySection()
        {
            return "<div class=\"section\"><div class=\"heading\" id=\"pic-heading\">Pictionary</div><div id=\"pic-content\"></div></div>";
        }

        private string wrapBanglaSection(string bangla)
        {
            return "<div class=\"section\"><div class=\"heading\">Bengali Meaning</div>" + bangla + "</div>";
        }

        private string wrapBarronSection(string barron)
        {
            return "<div class=\"section\"><div class=\"heading\">Barron's GRE</div>" + barron + "</div>";
        }

        private string wrapMnemonicSection(string str)
        {
            if (str == "") return str;
            return "<div class=\"section\"><div class=\"heading\">Mnemonic (Memory Aid)</div><ul>" + str + "</ul></div>";
        }

        private string wrapThesaurusSection(string thesaurus)
        {
            if (thesaurus == "") return thesaurus;
            else return "<div class=\"section\"><div class=\"heading\">Thesaurus</div>" + thesaurus + "</div>";
        }

        private string wrapSentencesSection(string sentences)
        {
            if (sentences == "") return sentences;
            else return "<div class=\"section\"><div class=\"heading\">Example Sentences</div><ul>" + sentences + "</ul></div>";
        }

        private string wrapBody(string body)
        {
            return "<body>" + body + "</body>";
        }


        private string wrapBangPos(string pos)
        {
            return "<div class=\"bang-pos\">" + pos + ":</div>";
        }

        private string wrapBangMeaning(string meaning)
        {
            return "<div class=\"bang-meaning\">" + meaning + "</div>";
        }

        private string wrapBarMeaning(string meaning)
        {
           
            return "<div class=\"bar-meaning-label\">Meaning:</div><div class=\"bar-meaning\">" + meaning + "</div>";
        }

        private string wrapBarSentence(string sentence)
        {
            
            sentence = Regex.Replace(sentence, currentWord, "<u>" + currentWord + "</u>", RegexOptions.IgnoreCase); ;
            
            return "<div class=\"bar-sentence-label\">Example:</div><div class=\"bar-sentence\">" + sentence + "</div>";
        }

     

        private string wrapExtraSentence(string sentence)
        {

            sentence = Regex.Replace(sentence, currentWord, "<u>" + currentWord + "</u>", RegexOptions.IgnoreCase); 

            return "<li class=\"extra-sentence\">" + sentence + "</li>";
        }

        private string wrapSynonym(string synonym)
        {
            if (synonym == "") return synonym;
            else return "<div class=\"synonym-label\">Synonym:</div><div class=\"synonym\">" + synonym + "</div>";
        }

        private string wrapAntonym(string antonym)
        {
            if (antonym == "") return antonym;
            else return "<div class=\"antonym-label\">Antonym:</div><div class=\"antonym\">" + antonym + "</div>";  
        }


        private string wrapMnemonic(string str)
        {
            if (str == "") return str;


            else
            {
                str = Regex.Replace(str, @"[^\u0000-\u007F]", "");
                return "<li class=\"mnemonic\">" + str + "</li>";
            }
        }

        

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("" + greWordListBoxControl.SelectedIndex);
            greWordListBox.SelectedIndex = (greWordListBox.SelectedIndex + 1) % (greWordListBox.ItemCount - 1);
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            greWordListBox.SelectedIndex = (greWordListBox.SelectedIndex - 1) % (greWordListBox.ItemCount - 1);

        }

        private void nextButton_Click(object sender, EventArgs e)
        {
         


            if (greWordListBox.SelectedIndex < greWordListBox.ItemCount - 1)
                greWordListBox.SelectedIndex++;
            else greWordListBox.SelectedIndex = 0;

            
        }

        private void prevButton_Click(object sender, EventArgs e)
        {
            


          

            if (greWordListBox.SelectedIndex > 0)
                greWordListBox.SelectedIndex--;
            else greWordListBox.SelectedIndex = greWordListBox.ItemCount - 1;

           
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Create a PromptBuilder object and add content.
            PromptBuilder style = new PromptBuilder();
            PromptStyle ps = new PromptStyle();
            ps.Emphasis = PromptEmphasis.Strong;
            ps.Volume = PromptVolume.ExtraLoud;
            ps.Rate = PromptRate.Slow;
            style.StartStyle(ps);
            style.AppendText(greWordListBox.SelectedItem.ToString());
            style.EndStyle();
            
            SpeechSynthesizer synthesizer = new SpeechSynthesizer();
            synthesizer.SelectVoiceByHints(VoiceGender.Male, VoiceAge.Adult);
            synthesizer.SpeakAsync(style);

        }

        private void listSearchTextBox_TextChanged(object sender, EventArgs e)
        {
            int index = greWordListBox.FindString(listSearchTextBox.Text.Trim());

            if(index >= 0){
                greWordListBox.SelectedIndex = index;
            }
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            webBrowser1.Document.InvokeScript("increaseFont");
        }

        private void gaugeControl1_Click(object sender, EventArgs e)
        {

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.facebook.com/icanindya"); 
        }

        private void rater1_CurrentRatingChanged(object sender, EventArgs e)
        {
           
            SQLiteConnection conn = new SQLiteConnection(AppConfig.dbConnString);

            string sql = String.Format("UPDATE grewords_rating SET rate = {0} WHERE word = '{1}'", rater1.CurrentRating, currentWord);

            SQLiteCommand cmd = new SQLiteCommand(sql, conn);

            conn.Open();

            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conn.Close();
            }

        }

	//	private void wrapCompWord


        private string wrapCompWord(string word, string rate)
        {
            return "<div class=\"comp-heading\"><div class=\"comp-word\">" + word + "</div></div><div class=\"rateit\" data-rateit-value=\"" + rate + "\" data-word=\"" + word + "\"></div>";
        }

        private string wrapCompBangMeaning(string str)
        {
            return "<div class=\"comp-bang-meaning\">" + str + "</div>";
        
        }

        private string wrapCompBarMeaning(string str)
        {
            return "<div class=\"comp-bar-meaning\">" + str + "</div>";
        }

        private string wrapCompBarSentence(string str)
        {
            return "<div class=\"comp-bar-sentence\">" + str + "</div>";
        }

        
        void Button2Click(object sender, EventArgs e)
        {
        	
        	
        	
        	
        	
        	
        	
        	
        }
        
     
        void CompactViewRadioButtonCheckedChanged(object sender, EventArgs e)
        {
        	
        	
        }


        private void compactViewCheckItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //MessageBox.Show("1");
            detailedViewCheckItem.Checked = false;
        }

        private void detailedViewCheckItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //MessageBox.Show("2");
            compactViewCheckItem.Checked = false;
        }

        bool changeLock = false;

        private void detailedViewCheckItem_CheckedChanged(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (changeLock) return;
            //MessageBox.Show("3");
            if (detailedViewCheckItem.Checked)
            {
                changeLock = true;
                compactViewCheckItem.Checked = false;
                changeLock = false;
                loadGreWordlistBox();
            }
            else
            {
                changeLock = true;
                if (!compactViewCheckItem.Checked) detailedViewCheckItem.Checked = true;
                changeLock = false;
            }
        }

        private void compactViewCheckItem_CheckedChanged(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            
            if (changeLock) return;
            //MessageBox.Show("4");
            if (compactViewCheckItem.Checked)
            {
                changeLock = true;
                detailedViewCheckItem.Checked = false;
                changeLock = false;
                loadGreWordlistBox();
            }
            else
            {
                changeLock = true;
                if (!detailedViewCheckItem.Checked) compactViewCheckItem.Checked = true;
                changeLock = false;
            }
        }

        private void barButtonItem6_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            AboutForm af = new AboutForm();
            af.ShowDialog();
        }

        private void GRE_Wordlist_Shown(object sender, EventArgs e)
        {
           // 


           
        }
    }


    
}