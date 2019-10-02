using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Globalization;
using System.Text.RegularExpressions;
using System.IO;
using System.Windows.Forms;

using System.Web.Script.Serialization;
using Newtonsoft.Json.Linq;
using System.Threading;



namespace Anindya_s_Dictionary
{
    class GoogleTranslate
    {
       
    	Thread th;
        Action<string> callback;
        string text;

        public GoogleTranslate(string text, Action<string> callback)
        {
        	this.text = text;
            this.callback = callback;
            
            if(th != null && th.IsAlive) th.Abort();
            
            th = new Thread(fetchJson);
            th.IsBackground = true;
            th.Start();
        }


        private void fetchJson()
        {

            string sourceLanguageCode = Settings.getGoogleLanguageCode(Settings.getGoogleTranslateSource()); ;
            string targetLanguageCode = Settings.getGoogleLanguageCode(Settings.getGoogleTranslateTarget());

            string url = String.Format("http://translate.google.com/translate_a/t?client=mt&q={0}&hl=en&sl={1}&tl={2}", text, sourceLanguageCode, targetLanguageCode);

            using (WebClient wc = new WebClient())
            {

                wc.Encoding = Encoding.UTF8;
                wc.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");
          
                try{
                    string json = wc.DownloadString(url);
                    deserializeJson(json);
                }
                catch(Exception ex){
                	
                }
            }

        }

       
       

        private void deserializeJson(string json)
        {
            try
            {
                JObject jo = JObject.Parse(json);
                if (jo != null) callback(buildHtml(jo));
                else callback("Invalid data from Server.");
                    
            }
            catch {
                
            }
        }


        private string wrapPos(string pos)
        {
            if (pos == "") return "<span class = \"pos\">translation</span>";
            return "<span class = \"pos\">" + pos + "</span>"; 
        }

        private string wrapTerms(string terms)
        {
            if (terms == "") return terms;
            return "<span class = \"terms\">" + terms + "</span>";
        }

        private string wrapWord(string word)
        {
            if (word == "") return word;
            return "<span class = \"word\">" + word + "</span>"; 
        }

        private string wrapReverseTranslation1(string rt)
        {
            if (rt == "") return rt;
            return "<span class = \"reverse-translation\">" + rt + "</span>"; 
        }

        public string buildHtml(JObject jo)
        {
            string meaning = "";

            if (jo["dict"] != null)
            {
                foreach (var dict in jo["dict"])
                {
                    meaning += wrapPos(dict["pos"].ToString());

                    string terms_str = string.Empty;

                    if (dict["terms"] != null)
                    {
                        foreach (var term in dict["terms"])
                        {
                            terms_str += term.ToString() + ", ";
                        }

                        meaning += wrapTerms(trimEnd(terms_str, 2));
                    }

                    string entry_str = string.Empty;

                    if (dict["entry"] != null)
                    {
                        meaning += "<ul>";

                        foreach (var entry in dict["entry"])
                        {
                            meaning += "<li>";

                            meaning += wrapWord(entry["word"].ToString());

                            string reverse_translation_str = string.Empty;

                            if (entry["reverse_translation"] != null)
                            {
                                foreach (var rev_trans in entry["reverse_translation"])
                                {
                                    reverse_translation_str += rev_trans.ToString() + ", ";
                                }

                                meaning += wrapReverseTranslation1(trimEnd(reverse_translation_str, 2));
                            }

                            meaning += "</li>";
                        }

                        meaning += "</ul>";
                    }

                }

                return meaning ;
            }
            else
            {
                return jo["sentences"][0]["trans"].ToString();
            }
 
        }

        private string trimEnd(string str, int num)
        {
            return str.Substring(0, str.Length - num);
        }

    }

}
