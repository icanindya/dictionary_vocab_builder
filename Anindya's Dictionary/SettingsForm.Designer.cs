namespace Anindya_s_Dictionary
{
    partial class SettingsForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingsForm));
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.defaultDictionaryComboBoxEdit = new DevExpress.XtraEditors.ComboBoxEdit();
            this.groupControl2 = new DevExpress.XtraEditors.GroupControl();
            this.targetLanguageComboBoxEdit = new DevExpress.XtraEditors.ComboBoxEdit();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.sourceLanguageComboBoxEdit = new DevExpress.XtraEditors.ComboBoxEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.theFreeDictionaryCheckEdit = new DevExpress.XtraEditors.CheckEdit();
            this.princetonWordnetCheckEdit = new DevExpress.XtraEditors.CheckEdit();
            this.googleTranslateCheckEdit = new DevExpress.XtraEditors.CheckEdit();
            this.banglaAcademyCheckEdit = new DevExpress.XtraEditors.CheckEdit();
            ((System.ComponentModel.ISupportInitialize)(this.defaultDictionaryComboBoxEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).BeginInit();
            this.groupControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.targetLanguageComboBoxEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sourceLanguageComboBoxEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.theFreeDictionaryCheckEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.princetonWordnetCheckEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.googleTranslateCheckEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.banglaAcademyCheckEdit.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Image = global::Anindya_s_Dictionary.Properties.Resources.settings_3_16;
            this.labelControl1.Appearance.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.labelControl1.ImageAlignToText = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
            this.labelControl1.Location = new System.Drawing.Point(11, 24);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(107, 20);
            this.labelControl1.TabIndex = 15;
            this.labelControl1.Text = "Default Dictionary";
            // 
            // defaultDictionaryComboBoxEdit
            // 
            this.defaultDictionaryComboBoxEdit.Location = new System.Drawing.Point(124, 22);
            this.defaultDictionaryComboBoxEdit.Name = "defaultDictionaryComboBoxEdit";
            this.defaultDictionaryComboBoxEdit.Properties.AllowFocused = false;
            this.defaultDictionaryComboBoxEdit.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 10F);
            this.defaultDictionaryComboBoxEdit.Properties.Appearance.ForeColor = System.Drawing.Color.Black;
            this.defaultDictionaryComboBoxEdit.Properties.Appearance.Options.UseFont = true;
            this.defaultDictionaryComboBoxEdit.Properties.Appearance.Options.UseForeColor = true;
            this.defaultDictionaryComboBoxEdit.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.defaultDictionaryComboBoxEdit.Properties.Items.AddRange(new object[] {
            "Bangla Academy",
            "Google Translate (online)",
            "Princeton Wordnet",
            "TheFreeDictionary.com (online)"});
            this.defaultDictionaryComboBoxEdit.Properties.LookAndFeel.SkinName = "Office 2013";
            this.defaultDictionaryComboBoxEdit.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.defaultDictionaryComboBoxEdit.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.defaultDictionaryComboBoxEdit.Size = new System.Drawing.Size(211, 22);
            this.defaultDictionaryComboBoxEdit.TabIndex = 16;
            this.defaultDictionaryComboBoxEdit.SelectedIndexChanged += new System.EventHandler(this.defaultDictionaryComboBoxEdit_SelectedIndexChanged);
            // 
            // groupControl2
            // 
            this.groupControl2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupControl2.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.groupControl2.Appearance.ForeColor = System.Drawing.Color.Black;
            this.groupControl2.Appearance.Options.UseBackColor = true;
            this.groupControl2.Appearance.Options.UseForeColor = true;
            this.groupControl2.AppearanceCaption.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.groupControl2.AppearanceCaption.ForeColor = System.Drawing.Color.DodgerBlue;
            this.groupControl2.AppearanceCaption.Options.UseFont = true;
            this.groupControl2.AppearanceCaption.Options.UseForeColor = true;
            this.groupControl2.AppearanceCaption.Options.UseImage = true;
            this.groupControl2.CaptionImage = global::Anindya_s_Dictionary.Properties.Resources.settings_2_16;
            this.groupControl2.Controls.Add(this.targetLanguageComboBoxEdit);
            this.groupControl2.Controls.Add(this.labelControl3);
            this.groupControl2.Controls.Add(this.sourceLanguageComboBoxEdit);
            this.groupControl2.Controls.Add(this.labelControl2);
            this.groupControl2.Location = new System.Drawing.Point(12, 171);
            this.groupControl2.LookAndFeel.SkinName = "Office 2013";
            this.groupControl2.LookAndFeel.UseDefaultLookAndFeel = false;
            this.groupControl2.Name = "groupControl2";
            this.groupControl2.Size = new System.Drawing.Size(550, 79);
            this.groupControl2.TabIndex = 17;
            this.groupControl2.Text = "Google Translate Settings";
            // 
            // targetLanguageComboBoxEdit
            // 
            this.targetLanguageComboBoxEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.targetLanguageComboBoxEdit.Location = new System.Drawing.Point(369, 34);
            this.targetLanguageComboBoxEdit.Name = "targetLanguageComboBoxEdit";
            this.targetLanguageComboBoxEdit.Properties.AllowFocused = false;
            this.targetLanguageComboBoxEdit.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 10F);
            this.targetLanguageComboBoxEdit.Properties.Appearance.ForeColor = System.Drawing.Color.Black;
            this.targetLanguageComboBoxEdit.Properties.Appearance.Options.UseFont = true;
            this.targetLanguageComboBoxEdit.Properties.Appearance.Options.UseForeColor = true;
            this.targetLanguageComboBoxEdit.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.targetLanguageComboBoxEdit.Properties.LookAndFeel.SkinName = "Office 2013";
            this.targetLanguageComboBoxEdit.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.targetLanguageComboBoxEdit.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.targetLanguageComboBoxEdit.Size = new System.Drawing.Size(174, 22);
            this.targetLanguageComboBoxEdit.TabIndex = 20;
            this.targetLanguageComboBoxEdit.SelectedIndexChanged += new System.EventHandler(this.targetLanguageComboBoxEdit_SelectedIndexChanged);
            // 
            // labelControl3
            // 
            this.labelControl3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.labelControl3.Location = new System.Drawing.Point(281, 37);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(82, 13);
            this.labelControl3.TabIndex = 19;
            this.labelControl3.Text = "Target Language";
            // 
            // sourceLanguageComboBoxEdit
            // 
            this.sourceLanguageComboBoxEdit.Location = new System.Drawing.Point(96, 36);
            this.sourceLanguageComboBoxEdit.Name = "sourceLanguageComboBoxEdit";
            this.sourceLanguageComboBoxEdit.Properties.AllowFocused = false;
            this.sourceLanguageComboBoxEdit.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 10F);
            this.sourceLanguageComboBoxEdit.Properties.Appearance.ForeColor = System.Drawing.Color.Black;
            this.sourceLanguageComboBoxEdit.Properties.Appearance.Options.UseFont = true;
            this.sourceLanguageComboBoxEdit.Properties.Appearance.Options.UseForeColor = true;
            this.sourceLanguageComboBoxEdit.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.sourceLanguageComboBoxEdit.Properties.LookAndFeel.SkinName = "Office 2013";
            this.sourceLanguageComboBoxEdit.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.sourceLanguageComboBoxEdit.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.sourceLanguageComboBoxEdit.Size = new System.Drawing.Size(174, 22);
            this.sourceLanguageComboBoxEdit.TabIndex = 18;
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(7, 42);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(83, 13);
            this.labelControl2.TabIndex = 16;
            this.labelControl2.Text = "Source Language";
            // 
            // groupControl1
            // 
            this.groupControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupControl1.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.groupControl1.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupControl1.Appearance.ForeColor = System.Drawing.Color.Black;
            this.groupControl1.Appearance.Options.UseBackColor = true;
            this.groupControl1.Appearance.Options.UseFont = true;
            this.groupControl1.Appearance.Options.UseForeColor = true;
            this.groupControl1.AppearanceCaption.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupControl1.AppearanceCaption.ForeColor = System.Drawing.Color.DodgerBlue;
            this.groupControl1.AppearanceCaption.Options.UseFont = true;
            this.groupControl1.AppearanceCaption.Options.UseForeColor = true;
            this.groupControl1.CaptionImage = global::Anindya_s_Dictionary.Properties.Resources.settings_2_16;
            this.groupControl1.Controls.Add(this.theFreeDictionaryCheckEdit);
            this.groupControl1.Controls.Add(this.princetonWordnetCheckEdit);
            this.groupControl1.Controls.Add(this.googleTranslateCheckEdit);
            this.groupControl1.Controls.Add(this.banglaAcademyCheckEdit);
            this.groupControl1.Location = new System.Drawing.Point(12, 66);
            this.groupControl1.LookAndFeel.SkinName = "Office 2013";
            this.groupControl1.LookAndFeel.UseDefaultLookAndFeel = false;
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(550, 79);
            this.groupControl1.TabIndex = 14;
            this.groupControl1.Text = "Enabled Dictionaries";
            // 
            // theFreeDictionaryCheckEdit
            // 
            this.theFreeDictionaryCheckEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.theFreeDictionaryCheckEdit.Location = new System.Drawing.Point(376, 56);
            this.theFreeDictionaryCheckEdit.Name = "theFreeDictionaryCheckEdit";
            this.theFreeDictionaryCheckEdit.Properties.AllowFocused = false;
            this.theFreeDictionaryCheckEdit.Properties.Caption = "TheFreeDictionary.com (online)";
            this.theFreeDictionaryCheckEdit.Properties.LookAndFeel.SkinName = "Office 2013";
            this.theFreeDictionaryCheckEdit.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.theFreeDictionaryCheckEdit.Size = new System.Drawing.Size(174, 19);
            this.theFreeDictionaryCheckEdit.TabIndex = 16;
            this.theFreeDictionaryCheckEdit.CheckedChanged += new System.EventHandler(this.theFreeDictionaryCheckEdit_CheckedChanged);
            // 
            // princetonWordnetCheckEdit
            // 
            this.princetonWordnetCheckEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.princetonWordnetCheckEdit.Location = new System.Drawing.Point(376, 31);
            this.princetonWordnetCheckEdit.Name = "princetonWordnetCheckEdit";
            this.princetonWordnetCheckEdit.Properties.AllowFocused = false;
            this.princetonWordnetCheckEdit.Properties.Caption = "Princeton Wordnet";
            this.princetonWordnetCheckEdit.Properties.LookAndFeel.SkinName = "Office 2013";
            this.princetonWordnetCheckEdit.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.princetonWordnetCheckEdit.Size = new System.Drawing.Size(118, 19);
            this.princetonWordnetCheckEdit.TabIndex = 15;
            this.princetonWordnetCheckEdit.CheckedChanged += new System.EventHandler(this.princetonWordnetCheckEdit_CheckedChanged);
            // 
            // googleTranslateCheckEdit
            // 
            this.googleTranslateCheckEdit.Location = new System.Drawing.Point(5, 49);
            this.googleTranslateCheckEdit.Name = "googleTranslateCheckEdit";
            this.googleTranslateCheckEdit.Properties.AllowFocused = false;
            this.googleTranslateCheckEdit.Properties.Caption = "Google Translate (online)";
            this.googleTranslateCheckEdit.Properties.LookAndFeel.SkinName = "Office 2013";
            this.googleTranslateCheckEdit.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.googleTranslateCheckEdit.Size = new System.Drawing.Size(144, 19);
            this.googleTranslateCheckEdit.TabIndex = 14;
            this.googleTranslateCheckEdit.CheckedChanged += new System.EventHandler(this.googleTranslateCheckEdit_CheckedChanged);
            // 
            // banglaAcademyCheckEdit
            // 
            this.banglaAcademyCheckEdit.Location = new System.Drawing.Point(5, 24);
            this.banglaAcademyCheckEdit.Name = "banglaAcademyCheckEdit";
            this.banglaAcademyCheckEdit.Properties.AllowFocused = false;
            this.banglaAcademyCheckEdit.Properties.Caption = "Bangla Academy";
            this.banglaAcademyCheckEdit.Properties.LookAndFeel.SkinName = "Office 2013";
            this.banglaAcademyCheckEdit.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.banglaAcademyCheckEdit.Size = new System.Drawing.Size(101, 19);
            this.banglaAcademyCheckEdit.TabIndex = 13;
            this.banglaAcademyCheckEdit.CheckedChanged += new System.EventHandler(this.banglaAcademyCheckEdit_CheckedChanged);
            // 
            // SettingsForm
            // 
            this.Appearance.BackColor = System.Drawing.Color.White;
            this.Appearance.ForeColor = System.Drawing.Color.Black;
            this.Appearance.Options.UseBackColor = true;
            this.Appearance.Options.UseForeColor = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(574, 262);
            this.Controls.Add(this.groupControl2);
            this.Controls.Add(this.defaultDictionaryComboBoxEdit);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.groupControl1);
            this.FormBorderEffect = DevExpress.XtraEditors.FormBorderEffect.Glow;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.LookAndFeel.SkinName = "Metropolis";
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SettingsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Settings";
            this.Load += new System.EventHandler(this.PrefrencesForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.defaultDictionaryComboBoxEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).EndInit();
            this.groupControl2.ResumeLayout(false);
            this.groupControl2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.targetLanguageComboBoxEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sourceLanguageComboBoxEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.theFreeDictionaryCheckEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.princetonWordnetCheckEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.googleTranslateCheckEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.banglaAcademyCheckEdit.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.CheckEdit banglaAcademyCheckEdit;
        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraEditors.CheckEdit theFreeDictionaryCheckEdit;
        private DevExpress.XtraEditors.CheckEdit princetonWordnetCheckEdit;
        private DevExpress.XtraEditors.CheckEdit googleTranslateCheckEdit;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.ComboBoxEdit defaultDictionaryComboBoxEdit;
        private DevExpress.XtraEditors.GroupControl groupControl2;
        private DevExpress.XtraEditors.ComboBoxEdit targetLanguageComboBoxEdit;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.ComboBoxEdit sourceLanguageComboBoxEdit;
        private DevExpress.XtraEditors.LabelControl labelControl2;
    }
}