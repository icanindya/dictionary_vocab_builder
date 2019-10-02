using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Diagnostics;

namespace Anindya_s_Dictionary
{
    public partial class AboutForm : DevExpress.XtraEditors.XtraForm
    {

        FontStyle defaultFontStyle;

        public AboutForm()
        {
            InitializeComponent();
        }

        private void AboutForm_Load(object sender, EventArgs e)
        {
            defaultFontStyle = vocabPcLabel.Font.Style;
            labelControl2.Appearance.BackColor = System.Drawing.Color.Transparent;
        }

        private void vocabPcLabel_MouseEnter(object sender, EventArgs e)
        {
            LabelControl label = (LabelControl)sender;
            label.Font = new Font(label.Font, vocabPcLabel.Font.Style | FontStyle.Underline);
            label.Cursor = Cursors.Hand;
        }

        private void vocabPcLabel_MouseLeave(object sender, EventArgs e)
        {
            LabelControl label = (LabelControl)sender;
            label.Font = new Font(label.Font, defaultFontStyle);
            label.Cursor = Cursors.Arrow;
        }

        private void vocabPcLabel_Click(object sender, EventArgs e)
        {
            Process.Start(AppConfig.GRE_VOCAB_PC_URL);
        }

        private void vocabAndroidLabel_Click(object sender, EventArgs e)
        {
            Process.Start(AppConfig.GRE_VOCAB_ANDROID_URL);
        }
    }
}