using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using System.Collections;

namespace Anindya_s_Dictionary
{
    public class WordNetUSer
    {
         Action<string> callback;

        ArrayList wnMatches = new ArrayList();

        public WordNetUSer(string text, Action<string> callback)
        {
            this.callback = callback;
            wordnet(text);
        }

        public void wordnet(string searchText)
        {

            wnMatches.Clear();


            string wnMeaning = "";

            WordNet wn = new WordNet();

            wn.Overview(searchText, ref wnMeaning);

          //  MessageBox.Show(wnMeaning);

            string[] meaningLines = wnMeaning.Split(new char[] { '\n' });

            string html = "";


            for (int i = 0; i < meaningLines.Length; i++)
            {


                if (meaningLines[i].Length > 1)
                {
                    if (meaningLines[i].IndexOf("Overview") == 0)
                    {

                        string str = meaningLines[i].Substring("Overview of ".Length);
                        string pos = str.Substring(0, str.IndexOf(' '));

                        if (pos.Equals("noun")) pos = "Noun";
                        else if (pos.Equals("verb")) pos = "Verb";
                        else if (pos.Equals("adj")) pos = "Adjective";
                        else if (pos.Equals("adv")) pos = "Adverb";

                        string matchedText = str.Substring(str.IndexOf(' ') + 1).ToLower();
                        if (!searchText.Equals(matchedText)) wnMatches.Add(matchedText.Trim().ToLower());



                        html += wrapPos(pos) + wrapText(matchedText);
                    }

                    else if (meaningLines[i].Contains("."))
                    {

                        string index = "";
                        string mng = "";
                        string exmpl = "";


                        index = meaningLines[i].Substring(0, meaningLines[i].IndexOf('.') + 1);
                        int mngStart = meaningLines[i].IndexOf(" -- ") + 4;
                        int mngLength = meaningLines[i].Length - mngStart;
                        int explStart = -1;
                        if (meaningLines[i].IndexOf("; \"") > 0)
                        {
                            mngLength = meaningLines[i].IndexOf("; \"") - mngStart;
                            explStart = mngStart + mngLength + 2;

                            exmpl = meaningLines[i].Substring(explStart);
                        }


                        //  MessageBox.Show(mngStart + " " + mngLength);
                        mng = meaningLines[i].Substring(mngStart, mngLength);


                      
                        html += "<li>" + wrapMeaning(mng) + wrapExample(exmpl) + "</li>";



                    }
                }
            }


            callback(html);
        }


        private string wrapPos(string str)
        {
            if (str == "") return str;
            else return "<div class=\"pos\">" + str.ToLower() + "</div>";
        }

        private string wrapText(string str)
        {
            if (str == "") return str;
            else return "<div class=\"text\">" + str + "</div>";
        }

        private string wrapMeaning(string str)
        {
            if (str == "") return str;
            else return "<div class=\"meaning\">" + str + "</div>";
        }

        private string wrapExample(string str)
        {
            if (str == "") return str;
            else return "<div class=\"example\">" + str + "</div>";
        }

    }






    public class WordNet
    {

        //------------------Princeton WordNet-------------------


        private static string dictpath = AppConfig.WORDNET_PATH + @"\";//@"D:\Desktop\WCX - CS .Net - Copy\WCXExampleCS\bin\Debug\Data\WordNet\3.0\dict\"; //Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\Data\WordNet\3.0\dict\";
       
        
        
        private WordNetClasses.WN wnc = new WordNetClasses.WN(dictpath);

        ArrayList list = new ArrayList();
        Wnlib.Search se;

        private object pbobject = new object();
        string help = "";

        public Wnlib.SearchSet bobj2;
        public Wnlib.SearchSet bobj3;
        public Wnlib.SearchSet bobj4;
        public Wnlib.SearchSet bobj5;




        /// When the enter key is pressed in the search text field, perform an overview search.

        /// This is an overview search - the basis for any advanced search.
        public void Overview(string searchText, ref string wnMeaning)
        {

            //overview for 'search'
            string t = null;
            WordNetClasses.WN wnc = new WordNetClasses.WN(dictpath);

            se = null;
            // prevent the treeview from being overwritten by old results in showresults
            t = searchText;

            try
            {
                bool b = false;
                // sets the visibility of noun, verb, adj, adv when showing buttons for a word

                list = new ArrayList();
                wnc.OverviewFor(t, "noun", ref b, ref bobj2, list);
                //			btnNoun.Visible = b;

                wnc.OverviewFor(t, "verb", ref b, ref bobj3, list);
                //			btnVerb.Visible = b;

                wnc.OverviewFor(t, "adj", ref b, ref bobj4, list);
                //			btnAdj.Visible = b;

                wnc.OverviewFor(t, "adv", ref b, ref bobj5, list);
                //			btnAdv.Visible = b;

                //wordTextBox.Text = t;
                //txtSenses.Text = "0";


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + Constants.vbCrLf + Constants.vbCrLf + "Princeton's WordNet not pre-installed to default location?");
            }

            //FixDisplay(null);
            ShowResults(ref wnMeaning);
        }

        ///// Helper for displaying output and associated housekeeping.
        //public void FixDisplay(Wnlib.Opt opt)
        //{
        //    //pbobject = "";
        //    ShowResults(opt);

        //    //wordTextBox.Focus();
        //}


        /// Displays the results of the search.
        private void ShowResults(ref string wnMeaning)
        {
            string tmpstr = "";

            if (list.Count == 0)
            {

                //showFeedback("Search for " + txtSearchWord.Text + " returned 0 results.");
                wnMeaning = "Not found";
                return;
            }

            Overview tmptbox = new Overview();

            if ((!object.ReferenceEquals(pbobject.GetType(), tmptbox.GetType())))
            {

                Overview tb = new Overview();
                //    txtOutput.Text = "";
                tb.useList(list, help, ref tmpstr);
                if ((help != null) & !string.IsNullOrEmpty(help))
                {
                    tmpstr = help + Constants.vbCrLf + Constants.vbCrLf + tmpstr;
                }
                tmpstr = Strings.Replace(tmpstr, Constants.vbLf, Constants.vbCrLf);
                tmpstr = Strings.Replace(tmpstr, Constants.vbCrLf, "", 1, 1);
                tmpstr = Strings.Replace(tmpstr, "_", " ");
                //showFeedback(tmpstr);

                if (string.IsNullOrEmpty(tmpstr) | tmpstr == "<font color='green'><br />" + Constants.vbCr + " " + /*wordTextBox.Text + */" has no senses </font>")
                {
                    //showFeedback("Search for " + txtSearchWord.Text + " returned 0 results.");
                    wnMeaning = "Not found";
                }

                //txtOutput.Visible = true;
                //pbobject = tb;// culprit
            }

            //wordnetTextBox.Text = tmpstr;
            wnMeaning = tmpstr;

        }




    }


    /// <summary>
    /// Displays the basic overview text which is the 'buf' result returned from the WordNet.Net library.
    /// </summary>
    public class Overview
    {
        private ArrayList cont;
        private int totLines;
        private string sw;

        private int helpLines;
        public void usePassage(string passage, ref string tmpstr)
        {
            tmpstr += passage;
        }

        public void useList(ArrayList w, string help, ref string tmpstr)
        {
            cont = new ArrayList();
            totLines = 0;
            sw = null;

            if ((help != null) && !(string.IsNullOrEmpty(help)))
            {
                usePassage(help, ref tmpstr);
            }
            helpLines = totLines;
            int j = 0;
            while (j < w.Count)
            {
                Wnlib.Search se = (Wnlib.Search)w[j];
                sw = se.word;
                usePassage(se.buf, ref tmpstr);
                System.Math.Min(System.Threading.Interlocked.Increment(ref j), j - 1);
            }
        }
    } 
}
