using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using System.Data.SQLite;

namespace Anindya_s_Dictionary
{
    
    

    public partial class SettingsForm : DevExpress.XtraEditors.XtraForm
    {

        bool formLoaded = false;
        ComboBoxItemCollection dictionaryCollection, sourceLanguageCollection, targetLanguageCollection;


        public SettingsForm()
        {
            InitializeComponent();
        }


        private void PrefrencesForm_Load(object sender, EventArgs e)
        {
            if (Settings.isBanglaAcademyEnabled()) banglaAcademyCheckEdit.Checked = true;
            else banglaAcademyCheckEdit.Checked = false;

            if (Settings.isGoogleTranslateEnabled()) googleTranslateCheckEdit.Checked = true;
            else googleTranslateCheckEdit.Checked = false;

            if (Settings.isPrincetonWordnetEnabled()) princetonWordnetCheckEdit.Checked = true;
            else princetonWordnetCheckEdit.Checked = false;

            if (Settings.isTheFreeDictionaryEnabled()) theFreeDictionaryCheckEdit.Checked = true;
            else theFreeDictionaryCheckEdit.Checked = false;

            dictionaryCollection = defaultDictionaryComboBoxEdit.Properties.Items;
            sourceLanguageCollection = sourceLanguageComboBoxEdit.Properties.Items;
            targetLanguageCollection = targetLanguageComboBoxEdit.Properties.Items;

            defaultDictionaryComboBoxEdit.SelectedIndex = Settings.getDefaultDictionary();
            getGoogleLanguages();
            formLoaded = true;
            
        }


        private void getGoogleLanguages()
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

            sourceLanguageCollection.BeginUpdate();
            targetLanguageCollection.BeginUpdate();
            try
            {
                foreach (DataRow dr in dt.Rows)
                {
                    sourceLanguageCollection.Add(dr["language"].ToString());
                    targetLanguageCollection.Add(dr["language"].ToString());
                }
            }
            finally
            {
                sourceLanguageCollection.EndUpdate();
                targetLanguageCollection.EndUpdate();
            }

            sourceLanguageComboBoxEdit.SelectedIndex = sourceLanguageCollection.IndexOf(Settings.getGoogleTranslateSource());
            targetLanguageComboBoxEdit.SelectedIndex = targetLanguageCollection.IndexOf(Settings.getGoogleTranslateTarget());
                

        }

        private void targetLanguageComboBoxEdit_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(formLoaded == false) return;
            Settings.setGoogleTranslateTarget(targetLanguageComboBoxEdit.SelectedItem.ToString());
        }

        private void banglaAcademyCheckEdit_CheckedChanged(object sender, EventArgs e)
        {
            if (formLoaded == false) return;
            Settings.changeDictionaryEnableState(Settings.BANGLA_ACADEMY_ID, ((CheckEdit)sender).Checked);
        }

        private void googleTranslateCheckEdit_CheckedChanged(object sender, EventArgs e)
        {
            if (formLoaded == false) return;
            Settings.changeDictionaryEnableState(Settings.GOOGLE_TRANSLATE_ID, ((CheckEdit)sender).Checked);
        }

        private void princetonWordnetCheckEdit_CheckedChanged(object sender, EventArgs e)
        {
            if (formLoaded == false) return;
            Settings.changeDictionaryEnableState(Settings.PRINCETON_WORDNET_ID, ((CheckEdit)sender).Checked);
        }

        private void theFreeDictionaryCheckEdit_CheckedChanged(object sender, EventArgs e)
        {
            if (formLoaded == false) return;
            Settings.changeDictionaryEnableState(Settings.THE_FREE_DICTIONARY_ID, ((CheckEdit)sender).Checked);
        }

        private void defaultDictionaryComboBoxEdit_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (formLoaded == false) return;
            Settings.changeDefaultDictionary(((ComboBoxEdit)sender).SelectedIndex);
        }

       


    
    }
   
}