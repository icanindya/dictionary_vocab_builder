using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Data.SQLite;

using System.IO;
using System.Threading;
using System.Runtime.InteropServices;
using WordCapture;
using System.Net;
using System.Speech.Synthesis;
using System.Text;
using System.Timers;
using System.Text.RegularExpressions;
using DevExpress.XtraEditors.Controls;
using System.Diagnostics;

namespace Anindya_s_Dictionary
{
    public partial class DictionaryForm : DevExpress.XtraEditors.XtraForm
    {

        ComboBoxItemCollection suggestionCollection;
        [DllImport("user32.dll")]
        public static extern bool RegisterHotKey(IntPtr hWnd, int id,
           int fsModifiers, Keys virtualKey);

        [DllImport("user32.dll")]
        public static extern bool UnregisterHotKey(IntPtr hWnd, int id);


        private Boolean exit = false;

        private WCaptureX m_wCapture = ComFactory.Instance.NewWCaptureX();
        private WMonitorX m_wMonitor = ComFactory.Instance.NewWMonitorX();

        public const uint E_CAPTURE_NONE = 0x00;
        public const uint E_CAPTURE_MOUSE = 0x01;
        public const uint E_CAPTURE_MOUSE_GEST = 0x02;
        public const uint E_CAPTURE_CARET = 0x04;
        public const uint E_CAPTURE_CURSOR = 0x08;
        public const uint E_CAPTURE_HOVER = 0x10;
        public const uint E_CAPTURE_HOTKEY = 0x20;
        public const uint E_CAPTURE_SELECTED_TEXT = 0x40;

        public const int HOTKEYF_ALT = 0x04;
        public const int HOTKEYF_SHIFT = 0x01;
        public const int HOTKEYF_CONTROL = 0x02;
        public const int HOTKEYF_EXT = 0x08;

        private int m_nHotkeyCursorId;
        private int m_nHotkeyCaretId;
        private int m_nHotkeySelectedTextId;


        public static ushort LowWord(uint value)
        {
            return (ushort)(value & 0xFFFF);
        }
        public static ushort HighWord(uint value)
        {
            return (ushort)(value >> 16);
        }
        public static byte LowByte(ushort value)
        {
            return (byte)(value & 0xFF);
        }
        public static byte HighByte(ushort value)
        {
            return (byte)(value >> 8);
        }

        public void Hotkey2MonitorParams(int dwHotkey, out int nModifier, out int nKey)
        {
            nModifier = 0;
            nKey = (int)LowByte(LowWord((uint)dwHotkey));

            int wModifiers = HighByte(LowWord((uint)dwHotkey));
            if ((wModifiers & HOTKEYF_ALT) != 0)
                nModifier |= (int)W_KEY.wmKeyAlt;
            if ((wModifiers & HOTKEYF_CONTROL) != 0)
                nModifier |= (int)W_KEY.wmKeyCtrl;
            if ((wModifiers & HOTKEYF_EXT) != 0)
                nModifier |= (int)W_KEY.wmKeyWin;
            if ((wModifiers & HOTKEYF_SHIFT) != 0)
                nModifier |= (int)W_KEY.wmKeyShift;
        }

        //replace the functions from the C# example
        //with these to run


        public void StartMonitoring()
        {

            int color = 61440;
            W_LINE_STYLE wLineStyle = W_LINE_STYLE.wmLineDot;
            W_KEY wKey = W_KEY.wmKeyCtrl;
            W_MOUSE wMouse = W_MOUSE.wmMouseRight;
            m_wMonitor.LineStyle = (int)wLineStyle;
            m_wMonitor.LineColor = (uint)color;

            m_wMonitor.Start((int)wKey, (int)wMouse, true);
        }

        public WResult PerformCapture(int hWnd, int x1, int y1, int x2, int y2)
        {
            WInput objInput = ComFactory.Instance.NewWInput();

            // set capture options
            int wOptions = 0;


            // set the getContext flag
            wOptions |= (int)W_CAPTURE_OPTIONS.wCaptureOptionsGetContext;

            //set capture parameters
            objInput.Hwnd = hWnd;
            objInput.StartX = x1;
            objInput.StartY = y1;
            objInput.EndX = x2;
            objInput.EndY = y2;
            objInput.Options = wOptions;



            // set the number of context words
            objInput.ContextWordsLeft = 1;
            objInput.ContextWordsRight = 1;

            // declare the string which will get the results
            string strResult;

            WResult objResult;
            objResult = m_wCapture.Capture(objInput);

            // get the text from the capture
            strResult = objResult.Text;


            //use OCR if native method fails
            if (strResult == string.Empty)
            {
                wOptions |= (int)W_CAPTURE_OPTIONS.wCaptureOptionsGetTessOCRText;
                objInput.OCRLanguage = "eng";

                objInput.Options = wOptions;
                objResult = m_wCapture.Capture(objInput);
                strResult = objResult.Text;
            }
            return objResult;
        }

        //replace the functions from the C# example
        //with these to run

       

        private void CaptureEvent(int hwnd, int x1, int y1, int x2, int y2)
        {
            WResult objResult = PerformCapture(hwnd, x1, y1, x2, y2);

            if (objResult == null)
            {
                return;
            }
            processClickResult(objResult);
            
        }

        private void CaptureFullText(int x, int y)
        {
            UIControl spUIC = ComFactory.Instance.NewUIControl();

            spUIC.CreateFromScreenPoint(x, y);
            string strRes = spUIC.Value;
            if (strRes == string.Empty)
            {
                strRes = spUIC.Name;
            }
            //processClickResult(objResult);
        }

        private void startWordCapture()
        {
            StartMonitoring();
            m_wMonitor.WEvent += new _IWMonitorXEvents_WEventEventHandler(CaptureEvent);
        }

        private void endWordCapture()
        {
            m_wMonitor.Stop();
           // m_wCapture.EndCaptureSession();
        }

        protected override void WndProc(ref Message m)
        {
            const int WM_HOTKEY = 0x0312;
            WCaptureX capture = ComFactory.Instance.NewWCaptureX();
            int Hwnd, X, Y;
            switch (m.Msg)
            {
                case WM_HOTKEY:
                    {
                        int wp = m.WParam.ToInt32();
                        if (wp == m_nHotkeyCursorId)
                        {
                            capture.GetCursorInfo(out Hwnd, out X, out Y);
                            CaptureEvent(Hwnd, X, Y, X, Y);
                        }
                        else if (wp == m_nHotkeyCaretId)
                        {
                            capture.GetCaretInfo(out Hwnd, out X, out Y);
                            CaptureEvent(Hwnd, X, Y, X, Y);
                            break;
                        }
                        else if (wp == m_nHotkeySelectedTextId)
                        {
                            capture.GetCursorInfo(out Hwnd, out X, out Y);
                            CaptureEvent(Hwnd, X, Y, X, Y);
                            break;
                        }
                        break;
                    }
                default:
                    base.WndProc(ref m);
                    break;
            }
        }
    
        #region Interop Functions

        const int FEATURE_DISABLE_NAVIGATION_SOUNDS = 21;
        const int SET_FEATURE_ON_PROCESS = 0x00000002;

        [DllImport("urlmon.dll")]
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        static extern int CoInternetSetFeatureEnabled(
            int FeatureEntry,
            [MarshalAs(UnmanagedType.U4)] int dwFlags,
            bool fEnable);

        static void DisableClickSounds()
        {
            CoInternetSetFeatureEnabled(
                FEATURE_DISABLE_NAVIGATION_SOUNDS,
                SET_FEATURE_ON_PROCESS,
                true);
        }
        #endregion

        public DictionaryForm()
        {
            InitializeComponent();
        }

        private void DictionaryForm_Load(object sender, EventArgs e)
        {
            this.Width = (int)(SystemInformation.VirtualScreen.Width / 2);
            this.Height = (int)(SystemInformation.VirtualScreen.Height / 2);
            DisableClickSounds();
            startWordCapture();
            banglaAcademyBrowser.Navigate(AppConfig.BANGLA_ACADEMY_HTML_PATH);
            googleTranslateBrowser.Navigate(AppConfig.GOOGLE_TRANSLATE_HTML_PATH);
            wordNetBrowser.Navigate(AppConfig.WORDNET_HTML_PATH);
            theFreeDictionaryBrowser.Navigate(AppConfig.THE_FREE_DICTIONARY_HTML_PATH);

            searchTextBox.Properties.AutoComplete = false;
            suggestionCollection = searchTextBox.Properties.Items;
            dictionaryTabControl.SelectedTabPageIndex = Settings.getDefaultDictionary();
        }



        private void DictionaryForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            endWordCapture();
        }


        private void processClickResult(WResult objResult)
        {
            //MessageBox.Show("|" + objResult.LeftContext +  "|" + objResult.Text + "|" + objResult.RightContext + "|");

            string left = "", middle = "", right = "";

            searchTextBox.Text = middle = objResult.Text.Replace("ﬂ", "fl");

            suggestionCollection.Clear();

            suggestionCollection.Add(middle);

            Regex rgx = new Regex(@"\w+-?");
            Match mat = rgx.Match(objResult.LeftContext);

            if (mat.Success) left = mat.Value;
            else left = "";

            rgx = new Regex(@"-?\w+");
            mat = rgx.Match(objResult.RightContext);

            if (mat.Success) right = mat.Value;
            else right = "";

            if (!left.Equals(""))
            {
                if (!left.EndsWith("-")) left = left + " ";
                suggestionCollection.Add(left + middle);
            }

            if (!right.Equals(""))
            {
                if (!right.StartsWith("-")) right = " " + right;
                suggestionCollection.Add(middle + right);
            }

            if (!left.Equals("") && !right.Equals(""))
            {
                suggestionCollection.Add(left + middle + right);
            }

            if (this.WindowState == FormWindowState.Minimized) this.WindowState = FormWindowState.Normal;
            this.Show();
            this.Activate();
            lookup();


        }

        private void lookup()
        {
            if (Settings.isPrincetonWordnetEnabled()) searchPrincetonWordnet();
            if (Settings.isBanglaAcademyEnabled()) searchBanglaAcademy();
            if (Settings.isGoogleTranslateEnabled()) searchGoogleTranslate();
            if (Settings.isTheFreeDictionaryEnabled()) searchTheFreeDictionary();
        }

        private void searchBanglaAcademy()
        {
            BanglaAcademy ba = new BanglaAcademy(searchTextBox.Text.Trim().ToLower(), showBanglaAcademy);
        }

        private void searchGoogleTranslate()
        {
            GoogleTranslate gt = new GoogleTranslate(searchTextBox.Text.Trim(), showGoogleTranslate);
        }

        private void searchPrincetonWordnet()
        {
            WordNetUSer wnu = new WordNetUSer(searchTextBox.Text.Trim(), showWordNet);
        }


       
        private void searchTheFreeDictionary()
        {
            new Thread(new ThreadStart(this.temp)).Start();
        }

        public delegate void setContentCallback(string str);

        public void temp(){
            string url = String.Format("http://www.thefreedictionary.com/{0}", searchTextBox.Text.Trim());
           // theFreeDictionaryBrowser.Navigate(url, null, null, "User-Agent: Mozilla/5.0 (Linux; U; Android 4.1.2; nl-nl; GT-I9300 Build/JZO54K)\r\n");
            theFreeDictionaryBrowser.ScriptErrorsSuppressed = true;
            theFreeDictionaryBrowser.Navigate(url);
            
        }

        


        
        public void showBanglaAcademy(string str){
        	banglaAcademyBrowser.Document.InvokeScript("showContent", new Object[] { str }); 
        }
        
        public void showGoogleTranslate(string str)
        {
        	if (googleTranslateBrowser.InvokeRequired)
			{	
        		setContentCallback d = new setContentCallback(showGoogleTranslate);
				Invoke(d, new object[] { str });
			}
			else
			{
				googleTranslateBrowser.Document.InvokeScript("showContent", new object[]{str});
			}
    	}

        public void showWordNet(string str)
        {
            wordNetBrowser.Document.InvokeScript("showContent", new Object[] { str });
        }

        private void facebookLabel_Click(object sender, EventArgs e)
        {
            Process.Start(AppConfig.ANINDYAS_DICTIONARY_FB_URL);
        }

        private void searchTextBox_TextChanged(object sender, EventArgs e)
        {
            suggestionCollection.Clear();

            SQLiteConnection conn = new SQLiteConnection(AppConfig.DB_CONNECTION_STRING);
            conn.Open();

            string sql = String.Format("SELECT word FROM dictionary_words WHERE word LIKE '{0}%' LIMIT 500", searchTextBox.Text.Trim());

            SQLiteDataAdapter da = new SQLiteDataAdapter(sql, conn);

            DataTable dt = new DataTable();
            da.Fill(dt);
            conn.Close();
            List<string> words = new List<string>();

            foreach (DataRow dr in dt.Rows)
            {
                words.Add(dr["word"].ToString());
            }
            suggestionCollection.BeginUpdate();
            try
            {
                suggestionCollection.AddRange(words);
            }
            finally
            {
                suggestionCollection.EndUpdate();
            }
            searchTextBox.ShowPopup();
            
        }

        private void searchTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {

                suggestionCollection.Clear();
                lookup();
            }
        }

        private void searchButton_Click(object sender, EventArgs e)
        {
            lookup();
        }

        private void speakButton_Click(object sender, EventArgs e)
        {
            PromptBuilder style = new PromptBuilder();
            PromptStyle ps = new PromptStyle();
            ps.Emphasis = PromptEmphasis.Strong;
            ps.Volume = PromptVolume.ExtraLoud;
            ps.Rate = PromptRate.Medium;
            style.StartStyle(ps);
            style.AppendText(searchTextBox.Text);
            style.EndStyle();
            SpeechSynthesizer reader = new SpeechSynthesizer();
            reader.SpeakAsync(style);
        }

        private void settingsBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            SettingsForm pf = new SettingsForm();
            pf.ShowDialog();
        }

        private void restartBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            exit = true;
            Application.Restart();
        }

        private void exitBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            exit = true;
            Application.Exit();
        }

        private void aboutBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            AboutForm af = new AboutForm();
            af.ShowDialog();
        }

        private void DictionaryForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!exit)
            {
         //       this.MinimizeBox = true;
                this.Hide();
                e.Cancel = true;
                notifyIcon1.Visible = true;
            }
        }

    }
}