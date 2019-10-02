using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SQLite;
using System.Data;
using System.Net;
using System.Windows.Forms;
namespace Anindya_s_Dictionary
{
    class Settings
    {

        public static readonly int BANGLA_ACADEMY_ID = 0;
        public static readonly int GOOGLE_TRANSLATE_ID = 1;
        public static readonly int PRINCETON_WORDNET_ID = 2;
        public static readonly int THE_FREE_DICTIONARY_ID = 3;

        bool banglaAcademyEnabled;
        bool googleTranslateEnabled;
        bool princetonWordnetEnabled;
        bool theFreeDictionaryEnabled;

        string googleTranslateSource;
        string googleTranslateTarget;

        Dictionary<string, string> googleLanguageCode = new Dictionary<string,string>();

        int defaultDictionary;

        private static Settings settings = null;

        private Settings()
        {

        }

        public static Settings getInstance()
        {
            if (settings == null)
            {
                settings = new Settings();
                readSettingsFromDB();
            }
            return settings;
        }

        public static bool isBanglaAcademyEnabled()
        {
            return settings.banglaAcademyEnabled;
        }

        public static void setBanglaAcademyEnabled(bool enabled)
        {
            settings.banglaAcademyEnabled = enabled;
        }

        public static bool isGoogleTranslateEnabled()
        {
            return settings.googleTranslateEnabled;
        }

        public static void setGoogleTranslateEnabled(bool enabled)
        {
            settings.googleTranslateEnabled = enabled;
        }

        public static bool isPrincetonWordnetEnabled()
        {
            return settings.princetonWordnetEnabled;
        }

        public static void setPrincetonWordnetEnabled(bool enabled)
        {
            settings.princetonWordnetEnabled = enabled;
        }

        public static bool isTheFreeDictionaryEnabled()
        {
            return settings.theFreeDictionaryEnabled;
        }

        public static void setTheFreeDictionaryEnabled(bool enabled)
        {
            settings.theFreeDictionaryEnabled = enabled;
        }


        public static string getGoogleTranslateSource()
        {
            return settings.googleTranslateSource;
        }

        public static void setGoogleTranslateSource(string sourceLanguage)
        {
            settings.googleTranslateSource = sourceLanguage;
        }

        public static string getGoogleTranslateTarget()
        {
            return settings.googleTranslateTarget;
        }

        public static void setGoogleTranslateTarget(string targetLanguage)
        {
            string sql = "UPDATE google_translate_settings SET target_language = @targetLanguage";
            SQLiteConnection conn = new SQLiteConnection(AppConfig.DB_CONNECTION_STRING);
            conn.Open();
            SQLiteCommand cmd = new SQLiteCommand(sql, conn);
            SQLiteParameter param = new SQLiteParameter("@targetLanguage", System.Data.DbType.String);
            param.Value = targetLanguage;
            cmd.Parameters.Add(param);
            cmd.ExecuteNonQuery();
            conn.Close();

            settings.googleTranslateTarget = targetLanguage;
        }

        public static int getDefaultDictionary()
        {
            return settings.defaultDictionary;
        }

        private static void readSettingsFromDB()
        {
            initDefaultDictionary();
            initDictionaryStatus();
            initGoogleTranslateSettings();
            initGoogleLanguageCode();
        }


        public static void initDefaultDictionary()
        {
            string sql = "SELECT * FROM default_dictionary";
            SQLiteConnection conn = new SQLiteConnection(AppConfig.DB_CONNECTION_STRING);
            conn.Open();
            SQLiteCommand cmd = new SQLiteCommand(sql, conn);
            SQLiteDataAdapter da = new SQLiteDataAdapter();
            da.SelectCommand = cmd;

            DataTable dt = new DataTable();
            da.Fill(dt);
            conn.Close();
            
            DataRow dr = dt.Rows[0];
            settings.defaultDictionary = Int32.Parse(dr["id"].ToString());
        }

        public static void initDictionaryStatus(){

            string sql = "SELECT * FROM dictionary_status";
            SQLiteConnection conn = new SQLiteConnection(AppConfig.DB_CONNECTION_STRING);
            conn.Open();
            SQLiteCommand cmd = new SQLiteCommand(sql, conn);
            SQLiteDataAdapter da = new SQLiteDataAdapter();
            da.SelectCommand = cmd;

            DataTable dt = new DataTable();
            da.Fill(dt);
            conn.Close();

            if (dt.Rows[0]["enabled"].ToString() == "1") settings.banglaAcademyEnabled = true;
            else settings.banglaAcademyEnabled = false;
               
            if (dt.Rows[1]["enabled"].ToString() == "1") settings.googleTranslateEnabled = true;
            else settings.googleTranslateEnabled = false;

            if (dt.Rows[2]["enabled"].ToString() == "1") settings.princetonWordnetEnabled = true;
            else settings.princetonWordnetEnabled = false;

            if (dt.Rows[3]["enabled"].ToString() == "1") settings.theFreeDictionaryEnabled = true;
            else settings.theFreeDictionaryEnabled = false;
        }

        public static void initGoogleTranslateSettings()
        {
            string sql = "SELECT * FROM google_translate_settings";
            SQLiteConnection conn = new SQLiteConnection(AppConfig.DB_CONNECTION_STRING);
            conn.Open();
            SQLiteCommand cmd = new SQLiteCommand(sql, conn);
            SQLiteDataAdapter da = new SQLiteDataAdapter();
            da.SelectCommand = cmd;

            DataTable dt = new DataTable();
            da.Fill(dt);
            conn.Close();

            settings.googleTranslateSource = dt.Rows[0]["source_language"].ToString();
            settings.googleTranslateTarget = dt.Rows[0]["target_language"].ToString();

        }

        public static void initGoogleLanguageCode()
        {
            string sql = "SELECT * FROM google_language_code";
            SQLiteConnection conn = new SQLiteConnection(AppConfig.DB_CONNECTION_STRING);
            conn.Open();
            SQLiteCommand cmd = new SQLiteCommand(sql, conn);
            SQLiteDataAdapter da = new SQLiteDataAdapter();
            da.SelectCommand = cmd;

            DataTable dt = new DataTable();
            da.Fill(dt);
            conn.Close();

            foreach (DataRow dr in dt.Rows)
            {
                settings.googleLanguageCode.Add(dr["language"].ToString(), dr["code"].ToString());
            }

        }

        public static string getGoogleLanguageCode(string language)
        {
            return settings.googleLanguageCode[language];
        }

        public static void changeDefaultDictionary(int dictionaryId)
        {
            string sql = "UPDATE default_dictionary SET id = @id";
            SQLiteConnection conn = new SQLiteConnection(AppConfig.DB_CONNECTION_STRING);
            conn.Open();
            SQLiteCommand cmd = new SQLiteCommand(sql, conn);
            SQLiteParameter param = new SQLiteParameter("@id", System.Data.DbType.Int16);
            param.Value = dictionaryId;
            cmd.Parameters.Add(param);
            cmd.ExecuteNonQuery();
            conn.Close();
        }


        public static void changeDictionaryEnableState(int dictionaryId, bool enabled)
        {
            switch (dictionaryId)
            {
                case 0:
                    settings.banglaAcademyEnabled = enabled;
                    break;
                case 1:
                    settings.googleTranslateEnabled = enabled;
                    break;
                case 2:
                    settings.princetonWordnetEnabled = enabled;
                    break;
                case 3:
                    settings.theFreeDictionaryEnabled = enabled;
                    break;
            }

            string sql = "UPDATE dictionary_status SET enabled = @enabled WHERE id = @id";
            SQLiteConnection conn = new SQLiteConnection(AppConfig.DB_CONNECTION_STRING);
            conn.Open();
            SQLiteCommand cmd = new SQLiteCommand(sql, conn);
            SQLiteParameter param = new SQLiteParameter("@enabled", System.Data.DbType.Int16);
            param.Value = enabled ? 1 : 0;
            cmd.Parameters.Add(param);
            param = new SQLiteParameter("@id", System.Data.DbType.Int16);
            param.Value = dictionaryId;
            cmd.Parameters.Add(param);
            cmd.ExecuteNonQuery();
            conn.Close();
        }
    }
}
