using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SQLite;
using System.Data;

namespace Anindya_s_Dictionary
{
    

    class BanglaAcademy
    {
        //Action<Byte[]> callback;

        Action<string> callback;
        
        string[,] suffixes = new string[,]{
                                            //Adverb
                                            {"ily", "y"}, // happily -> happy
                                            {"ly", "le"}, // terribly -> terrible
                                            
                                            //Plural
                                            {"ies", "y"}, // supplies -> supply 
                                            {"ves", "fe"}, // knives -> knife
                                            {"ves", "f"}, // halves -> half
                                            {"ci", "cus"}, // foci -> focus

                                            //Adjective (Comparative & Superlative forms)
                                            {"ier", "y"}, // happier -> happy
                                            {"iest", "y"}, // happiest -> happy
                                            
                                            //Past form
                                            {"ied", "y"}, // replied -> reply
                                            
                                            //Present Participle
                                            {"ing", "e"} // baking -> bake
                                        };
                                                   
        List<string> probable_words = new List<string>();


        public BanglaAcademy(string text, Action<string> callback)
        {
            this.callback = callback;
            searchWord(text);

 
        }

        public void searchWord(string str)
        {

            probable_words.Add(String.Format("'{0}'", str));

            for (int i = 0; i < suffixes.Length / 2; i++)
            {
                if (str.EndsWith(suffixes[i, 0]))
                {
                    probable_words.Add(String.Format("'{0}'", str.Substring(0, str.LastIndexOf(suffixes[i, 0])) + suffixes[i, 1]));
                }

            }


            for (int i = 1; i <= 4; i++)
            {
                if (str.Length - i >= 3)
                {
                    probable_words.Add(String.Format("'{0}'", str.Substring(0, str.Length - i)));
                }
            }
          
            SQLiteConnection conn = new SQLiteConnection(AppConfig.DB_CONNECTION_STRING);
            conn.Open();

            string sql = String.Format("SELECT word FROM bangla_academy_text WHERE word IN ({0});", String.Join(",", probable_words.ToArray()));

           // MessageBox.Show(sql);

            SQLiteDataAdapter da = new SQLiteDataAdapter(sql, conn);

            DataTable dt = new DataTable();
            da.Fill(dt);
            conn.Close();

            if (dt.Rows.Count > 0)
            {
                List<string> matched_words = new List<string>();
                foreach (DataRow row in dt.Rows)
                {
                    matched_words.Add(String.Format("'{0}'", row["word"].ToString()));
                }

                foreach (string word in probable_words)
                {
                    //MessageBox.Show(word);
                    if (matched_words.Contains(word))
                    {

                        conn = new SQLiteConnection(AppConfig.DB_CONNECTION_STRING);
                        conn.Open();

//                        sql = String.Format("SELECT image FROM bangla_academy WHERE word = {0};", word);
//
//                        da = new SQLiteDataAdapter(sql, conn);
//
//                        dt = new DataTable();
//                        da.Fill(dt);
//                        conn.Close();
//
//                        byte[] imageByteStream = dt.Rows[0]["image"] as byte[];
//
//                        callback(imageByteStream);
//                        return;

						sql = String.Format("SELECT meaning FROM bangla_academy_text WHERE word = {0};", word);
						da = new SQLiteDataAdapter(sql, conn);
						
						dt = new DataTable();
						da.Fill(dt);
						
						
						
						callback(dt.Rows[0]["meaning"].ToString());
						return;

                    }
                    
                }
            }
        }

        
    }
}
