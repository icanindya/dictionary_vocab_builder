using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Data.SQLite;
using System.Data;
using System.Net;

namespace Anindya_s_Dictionary
{
    class AppConfig
    {

        public static readonly string DESKTOP_PATH = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        public static readonly string APP_DATA_PATH = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Anindya's Dictionary");
        public static readonly string DB_PATH = Path.Combine(APP_DATA_PATH, "Database/new_db.sqlite");
        public static readonly string WORDNET_PATH = Path.Combine(APP_DATA_PATH, @"Database\wordnet\3.0\dict");
        public static readonly string BANGLA_ACADEMY_HTML_PATH = Path.Combine(APP_DATA_PATH, @"HTML\Bangla Academy.html");
        public static readonly string GOOGLE_TRANSLATE_HTML_PATH = Path.Combine(APP_DATA_PATH, @"HTML\Google Translate.html");
        public static readonly string WORDNET_HTML_PATH = Path.Combine(APP_DATA_PATH, @"HTML\WordNet.html");
        public static readonly string THE_FREE_DICTIONARY_HTML_PATH = Path.Combine(APP_DATA_PATH, @"HTML\TheFreeDictionary.html");
        public static readonly string DB_CONNECTION_STRING = "Data Source=" + DB_PATH;// + ";Password=#inc<pass>";

        public static readonly string ANINDYAS_DICTIONARY_FB_URL = "http://www.facebook.com/AnindyasDictionary";

        


        public static Dictionary<string, bool> dictionaryStatus = new Dictionary<string, bool>();
        public static Dictionary<string, string> dictionaries = new Dictionary<string, string>();
        public static string defaultDictionaryKey;

    }
}
