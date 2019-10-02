using System;
using System.Collections.Generic;
using System.Windows.Forms;
using DevExpress.Data.Helpers;
using DevExpress.LookAndFeel;
using System.IO;

namespace Anindya_s_Dictionary
{
    static class Program
    {

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            DevExpress.Skins.SkinManager.EnableFormSkins();
            DevExpress.UserSkins.BonusSkins.Register();
            UserLookAndFeel.Default.SetSkinStyle("Metropolis");
            DevExpress.Skins.SkinManager.DisableMdiFormSkins();
            DevExpress.LookAndFeel.LookAndFeelHelper.ForceDefaultLookAndFeelChanged();


            AppConfig ac = new AppConfig();
            Settings settings = Settings.getInstance();

            Application.Run(new DictionaryForm());
        }
    }
}