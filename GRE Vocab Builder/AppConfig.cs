using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Data.SQLite;
using System.Data;
using System.Net;

namespace GRE_Vocab_Builder
{
    class AppConfig
    {
        private static string makeLink(string path){

            return "file:///" + path.Replace('\\', '/');
        }

        public static string appDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Anindya's Dictionary");
        public static string greCSSPath = Path.Combine(appDataPath, @"CSS\gre.css");
        public static string dbPath = Path.Combine(appDataPath, "Database/vocab_db.sqlite");
        public static string jQueryPath = Path.Combine(appDataPath, @"JS\jquery-1.9.1.min.js");
        public static string greJsPath = Path.Combine(appDataPath, @"JS\gre.js");
        public static string wordnetPath = Path.Combine(appDataPath, @"Database\wordnet\3.0\dict");
        public static string gtCssPath = Path.Combine(appDataPath, @"CSS\gt.css");
        public static string baHtmlPath = Path.Combine(appDataPath, @"HTML\Bangla Academy.html");
        public static string gDicHtmlPath = Path.Combine(appDataPath, @"HTML\Google Dictionary.html");
        public static string gTransHtmlPath = Path.Combine(appDataPath, @"HTML\Google Translate.html");
        public static string wNetHtmlPath = Path.Combine(appDataPath, @"HTML\WordNet.html");
        public static string greHtmlPath = Path.Combine(appDataPath, @"HTML\GRE Wordlist.html");
        public static string welcomeHtmlPath = Path.Combine(appDataPath, @"HTML\welcome.html");

        public static string dbConnString = "Data Source=" + dbPath;// + ";Password=#inc<pass>";

        public static string greCssLink;
        public static string jQueryLink;
        public static string greJsLink;


        public static Dictionary<string, bool> dictionaryStatus = new Dictionary<string, bool>();
        public static Dictionary<string, string> dictionaries = new Dictionary<string, string>();
        public static string defaultDictionaryKey;

        public AppConfig(){

            greCssLink = makeLink(greCSSPath);
            jQueryLink = makeLink(jQueryPath);
            greJsLink = makeLink(greJsPath);

           // this.loadDictionaryStatus();
            
        }

        private void loadDictionaryStatus()
        {
            dictionaries.Add("BA", "Bangla Academy");
            dictionaries.Add("GT", "Google Translate");
            dictionaries.Add("GD", "Google Dictionary");
            dictionaries.Add("PW", "Princeton WordNet");
            dictionaries.Add("TFD", "TheFreeDictionary.com");

            foreach (string key in dictionaries.Keys)
            {
                dictionaryStatus[key] = getDictionaryStatus(key);
            }

            defaultDictionaryKey = getDefauktDictionaryKey();

        }


        private bool getDictionaryStatus(string tag){

            string sql = "SELECT * FROM dictionary_status WHERE tag = @tag";
            SQLiteConnection conn = new SQLiteConnection(dbConnString);
            conn.Open();
            SQLiteCommand cmd = new SQLiteCommand(sql, conn);
            SQLiteParameter param = new SQLiteParameter("@tag", System.Data.DbType.String);
            param.Value = tag;
            cmd.Parameters.Add(param);
            SQLiteDataAdapter da = new SQLiteDataAdapter();
            da.SelectCommand = cmd;

            DataTable dt = new DataTable();
            da.Fill(dt);
            conn.Close();
          
            return (dt.Rows[0]["status"].ToString() == "true") ? true : false;
        }


        private string getDefauktDictionaryKey()
        {
            string sql = "SELECT tag FROM default_dictionary";
            SQLiteConnection conn = new SQLiteConnection(dbConnString);
            conn.Open();
            SQLiteCommand cmd = new SQLiteCommand(sql, conn);
            SQLiteDataAdapter da = new SQLiteDataAdapter();
            da.SelectCommand = cmd;

            DataTable dt = new DataTable();
            da.Fill(dt);
            conn.Close();
            return dt.Rows[0]["tag"].ToString();
        }

        public static void changeDictionaryStatus(string tag, string status)
        {

            dictionaryStatus[tag] = (status == "true") ? true : false;


            string sql = "UPDATE dictionary_status SET status = @status WHERE tag = @tag";
       
            SQLiteConnection conn = new SQLiteConnection(dbConnString);
            conn.Open();
            SQLiteCommand cmd = new SQLiteCommand(sql, conn);
           
            SQLiteParameter param = new SQLiteParameter("@status", System.Data.DbType.String);
            param.Value = status;
            cmd.Parameters.Add(param);

            param = new SQLiteParameter("@tag", System.Data.DbType.String);
            param.Value = tag;
            cmd.Parameters.Add(param);

            
            cmd.ExecuteNonQuery();
            conn.Close();
            
        }

        public static void changeDeafultDictionary(string tag)
        {

            string sql = "UPDATE default_dictionary SET tag = @tag";

            SQLiteConnection conn = new SQLiteConnection(dbConnString);
            conn.Open();
            SQLiteCommand cmd = new SQLiteCommand(sql, conn);

            SQLiteParameter param = new SQLiteParameter("@tag", System.Data.DbType.String);
            param.Value = tag;
            cmd.Parameters.Add(param);

            cmd.ExecuteNonQuery();
            conn.Close();    
        }

       
    }
}
