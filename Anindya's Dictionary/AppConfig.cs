using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Data.SQLite;
using System.Data;
using System.Net;
using System.Reflection;

namespace Anindya_s_Dictionary
{
    class AppConfig
    {

        public static readonly string DESKTOP_PATH = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        public static readonly string APPDATA_PATH = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        public static readonly string EXECUTABLE_PATH = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        public static readonly string DICTIONARY_DATA_PATH = Path.Combine(EXECUTABLE_PATH, @"Anindya's Dictionary\Dictionary");
        public static readonly string DB_PATH = Path.Combine(DICTIONARY_DATA_PATH, @"DB\db.sqlite");
        public static readonly string WORDNET_PATH = Path.Combine(DICTIONARY_DATA_PATH, @"DB\wordnet\3.0\dict");
        public static readonly string BANGLA_ACADEMY_HTML_PATH = Path.Combine(DICTIONARY_DATA_PATH, @"HTML\Bangla Academy.html");
        public static readonly string GOOGLE_TRANSLATE_HTML_PATH = Path.Combine(DICTIONARY_DATA_PATH, @"HTML\Google Translate.html");
        public static readonly string WORDNET_HTML_PATH = Path.Combine(DICTIONARY_DATA_PATH, @"HTML\WordNet.html");
        public static readonly string THE_FREE_DICTIONARY_HTML_PATH = Path.Combine(DICTIONARY_DATA_PATH, @"HTML\TheFreeDictionary.html");
        public static readonly string DB_CONNECTION_STRING = "Data Source=" + DB_PATH;// + ";Password=#inc<pass>";

        public static readonly string ANINDYAS_DICTIONARY_FB_URL = "http://www.facebook.com/AnindyasDictionary";
        public static readonly string GRE_VOCAB_PC_URL = "https://www.dropbox.com/s/4bhf4rdy69epxoc/GRE%20Vocab%20Builder%20Setup.exe";
        public static readonly string GRE_VOCAB_ANDROID_URL = "https://play.google.com/store/apps/details?id=com.tonmoy.grevocabbuilder&hl=en";
       
        

    }
}
