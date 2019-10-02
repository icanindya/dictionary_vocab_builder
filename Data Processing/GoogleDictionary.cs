using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Windows.Forms;
using System.Threading;


namespace Anindya_s_Dictionary
{
    class GoogleDictionary
    {
        Action<string> callback;

        string json;
        string text;
        Thread th;

        public GoogleDictionary(string text, Action<string> callback)
        {
        	this.text = text;
            this.callback = callback;
            
            if(th != null && th.IsAlive) th.Abort();
			
            th = new Thread(fetchJson);
            th.IsBackground = true;
            th.Start();

       
        }
        
        private void fetchJson(){
	        string url = "http://www.google.com/dictionary/json?callback=dict_api.callbacks.id100&q=" + text + "&sl=en&tl=en&restrict=pr%2Csy&client=te";
            
            using (WebClient wc = new WebClient())
            {
                wc.Encoding = System.Text.Encoding.UTF8;
				try{
				    string json = wc.DownloadString(url);
					deserializeJson(json);
				}
				catch(Exception ex){
					
				}
            }
        }
        

        private string wrapPos(string str)
        {
            return "<div class=\"pos\">" + str.ToLower() + "</div>";
        }

        private string wrapPhonetic(string str)
        {
            return "<div class=\"phonetic\">" + str + "</div>";
        }

        private string wrapSound(string str)
        {
            return "<div class=\"sound\">" + str + "</div>";
        }


        private string wrapRelationLabel(string str)
        {
            return "<div class=\"rel-label\">" + str + "</div>";
        }

        private string wrapRelatedTerm(string str)
        {
            return "<div class=\"rel-term\">" + str + "</div>";
        }

        private string wrapRelatedSection(string str)
        {
            return "<div class=\"related-section\">" + str + "</div>";
        }

        private string wrapSynonymSection(string str)
        {
            return "<div class=\"syn\">Synonyms:</div>" + str;
        }

        private string wrapMeaning(string str)
        {
            return "<div class=\"meaning\">" + str + "</div>";
        }

        private string wrapExample(string str)
        {
            return "<div class=\"example\">" + str + "</div>";
        }

        private string wrapSynonymLabel(string str)
        {
            return "<div class=\"syn-label\">" + str + "</div>";
        }

        private string wrapSynonymTerm(string str)
        {
            return "<div class=\"syn-term\">" + str + "</div>";
        }
        
        
        private void deserializeJson(string json){
        	Dictionary<string, string> replace = new Dictionary<string, string>();

                replace.Add("\\x3c", "<");
                replace.Add("\\x3e", ">");
                replace.Add("\\x26", "&");
                replace.Add("\\x22", "\"");
                replace.Add("\\x27", "'");
                replace.Add("\\x3d", "=");

                //json = e.Result;

                json = json.Substring(json.IndexOf('{'), json.LastIndexOf('}') - json.IndexOf('{') + 1);

                foreach (string rep in replace.Keys)
                {
                    json = json.Replace(rep, replace[rep]);
                }

                //MessageBox.Show(json);

                JObject jo = JObject.Parse(json);

                string text = "";


                if (jo["primaries"] != null)
                {
                    foreach (var primary in jo["primaries"])
                    {
                        text += "<div class=\"section\">";

                        if (primary["terms"] != null)
                        {
                            foreach (var term in primary["terms"])
                            {

                                switch (term["type"].ToString())
                                {
                                    case "text":

                                        if (term["labels"] != null)
                                        {
                                            foreach (var label in term["labels"])
                                            {

                                                text += wrapPos(label["text"].ToString());
                                            }
                                        }


                                        break;

                                    case "phonetic":
                                        text += wrapPhonetic(term["text"].ToString());
                                        break;
                                    case "sound":
                                        text += wrapSound(term["text"].ToString());
                                        break;
                                }

                            }
                            


                        }


                        if (primary["entries"] != null)
                        {
                            bool listStart = true;
                            
                            foreach (var entry in primary["entries"])
                            {


                                switch (entry["type"].ToString())
                                {
                                    case "related":
                                        if (entry["terms"] != null)
                                        {
                                            string related = "";
                                            foreach (var term in entry["terms"])
                                            {
                                                if (term["labels"] != null)
                                                {
                                                    foreach (var label in term["labels"])
                                                    {
                                                        related += wrapRelationLabel(label["text"].ToString());
                                                    }
                                                }

                                                related += wrapRelatedTerm(term["text"].ToString());
                                            }
                                            text += wrapRelatedSection(related);
                                        }
                                        break;

                                    case "meaning":
                                        
                                        if (listStart)
                                        {
                                            text += "<ol>";
                                            listStart = false;
                                        }

                                        text += "<li>";

                                        if (entry["terms"] != null)
                                        {
                                            foreach (var term in entry["terms"])
                                            {
                                                text += wrapMeaning(term["text"].ToString());
                                            }
                                        }

                                        if (entry["entries"] != null)
                                        {
                                            foreach (var entry2 in entry["entries"])
                                            {
                                                switch (entry2["type"].ToString())
                                                {
                                                    case "example":

                                                        if (entry2["terms"] != null)
                                                        {
                                                            foreach (var term in entry2["terms"])
                                                            {
                                                                text += wrapExample(term["text"].ToString());
                                                            }
                                                        }
                                                        break;
                                                }

                                            }
                                        }

                                        text += "</li>";

                                        break;
                                }

                            }
                            text += "</ol>";
                        }
                        text += "</div>";

                    }
                }

                if (jo["synonyms"] != null)
                {
                    string syn = "";
                    
                    foreach (var synonym in jo["synonyms"])
                    {

                        //MessageBox.Show(synonym.ToString());

                        if (synonym["entries"] != null)
                        {
                            foreach (var entry in synonym["entries"])
                            {
                                if (entry["labels"] != null)
                                {
                                    foreach (var label in entry["labels"])
                                    {
                                        syn += wrapSynonymLabel(label["text"].ToString());
                                    }
                                }

                                if (entry["terms"] != null)
                                {
                                    foreach (var term in entry["terms"])
                                    {
                                        syn += wrapSynonymTerm(term["text"].ToString());
                                    }
                                }
                            }
                        }
                    }

                    text += wrapSynonymSection(syn);

                   
                }

                callback(text);

                File.WriteAllText(AppConfig.APP_DATA_PATH + @"\HTML\gDic.html", text);
            }
        
        

     
    


    }
}
