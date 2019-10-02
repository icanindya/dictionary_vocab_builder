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
using System.IO;
using System.Net;
using System.Globalization;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;
using System.Threading;
using HtmlAgilityPack;



namespace Anindya_s_Dictionary
{
	public partial class TestForm : DevExpress.XtraEditors.XtraForm
	{

		List<string> not_available = new List<string>();

		string conStringDatosUsuarios = @"Data Source = C:\Users\Anindya\Desktop\a.sqlite";
		SQLiteConnection con;// = new SQLiteConnection(conStringDatosUsuarios);

		public TestForm()
		{
			InitializeComponent();
		}

		private void button1_Click(object sender, EventArgs e)
		{
			string dbfile = @"C:\Users\Anindya\Desktop\a.sqlite";
			var conn = new SQLiteConnection("Data Source=" + dbfile);
			conn.Open();

			string sql = "SELECT word FROM grewords";

			SQLiteDataAdapter da = new SQLiteDataAdapter(sql, conn);

			DataTable dt = new DataTable();
			da.Fill(dt);

			foreach (DataRow dr in dt.Rows)
			{
				string word = dr["word"].ToString();

				

				if (!File.Exists(@"D:\Meaning\" + word + ".txt")) not_available.Add(word);
			}


			conn.Close();


			MessageBox.Show(not_available.Count.ToString());

			// File.WriteAllLines(@"C:\Users\Anindya\Desktop\not_available.txt", not_available.ToArray());

		}

		private void button2_Click(object sender, EventArgs e)
		{
			string dbfile = @"C:\Users\Anindya\Desktop\a.sqlite";
			var conn = new SQLiteConnection("Data Source=" + dbfile);
			conn.Open();

			string sql = "SELECT word FROM grewords";

			SQLiteDataAdapter da = new SQLiteDataAdapter(sql, conn);

			DataTable dt = new DataTable();
			da.Fill(dt);

			
			foreach (DataRow dr in dt.Rows)
			{
				string word = dr["word"].ToString();

				string url = "http://translate.google.com/translate_a/t?client=j&text=" + word + "&hl=en&sl=en&tl=bn";

				using (WebClient wc = new WebClient())
				{
					try
					{
						string json = wc.DownloadString(url);

						Regex rx = new Regex(@"\\[uU]([0-9A-F]{4})");
						json = rx.Replace(json, delegate(Match match) { return ((char)Int32.Parse(match.Value.Substring(2), NumberStyles.HexNumber)).ToString(); });
						// MessageBox.Show(json);

						string path = @"D:\JSON\" + word + ".txt";

						File.WriteAllText(path, json, Encoding.UTF8);
						
					}
					catch(Exception ex){
						MessageBox.Show(ex.Message);
					}
				}
			}
		}


		public void saveToDB(string word, JObject jo)
		{
			
			if (jo["dict"] != null)
			{
				JArray dict = (JArray)jo["dict"];

				for (int i = 0; i < dict.Count; i++)
				{

					string pos = dict[i]["pos"].ToString();



					string terms_str = string.Empty;

					if (dict[i]["terms"] != null)
					{

						JArray terms = (JArray)dict[i]["terms"];


						for (int j = 0; j < terms.Count; j++)
						{


							terms_str += terms[j].ToString() + ", ";


						}
						terms_str = terms_str.Substring(0, terms_str.Length - 2);

						//  MessageBox.Show(terms_str);

						
					}


					
					SQLiteCommand cmd = con.CreateCommand();
					cmd.CommandText = String.Format("INSERT INTO grewords_bangla (word, pos, meaning) VALUES (@0, @1, @2);");
					SQLiteParameter param = new SQLiteParameter("@0", System.Data.DbType.String);
					param.Value = word;
					cmd.Parameters.Add(param);
					
					param = new SQLiteParameter("@1", System.Data.DbType.String);
					param.Value = pos;
					cmd.Parameters.Add(param);


					param = new SQLiteParameter("@2", System.Data.DbType.String);
					param.Value = terms_str;
					cmd.Parameters.Add(param);

					con.Open();

					try
					{
						cmd.ExecuteNonQuery();
					}
					catch (Exception exc1)
					{
						MessageBox.Show(exc1.Message);
					}
					con.Close();


				}
				

			}
			

			else
			{
				SQLiteCommand cmd = con.CreateCommand();
				cmd.CommandText = String.Format("INSERT INTO grewords_bangla (word, meaning) VALUES (@0, @1);");
				SQLiteParameter param = new SQLiteParameter("@0", System.Data.DbType.String);
				param.Value = word;
				cmd.Parameters.Add(param);


				param = new SQLiteParameter("@1", System.Data.DbType.String);
				param.Value = jo["sentences"][0]["trans"].ToString();
				cmd.Parameters.Add(param);

				con.Open();

				try
				{
					cmd.ExecuteNonQuery();
				}
				catch (Exception exc1)
				{
					MessageBox.Show(exc1.Message);
				}
				con.Close();
			}
		}



		private JObject deserializeJson(string json)
		{
			//JavaScriptSerializer ser = new JavaScriptSerializer();
			// Object jo = ser.Deserialize<jsonObject>(json);


			JObject jo = JObject.Parse(json);
			return jo;

		}

		private void button3_Click(object sender, EventArgs e)
		{

			con = new SQLiteConnection(conStringDatosUsuarios);

			string dbfile = @"C:\Users\Anindya\Desktop\a.sqlite";
			var conn = new SQLiteConnection("Data Source=" + dbfile);
			conn.Open();

			string sql = "SELECT word FROM grewords";

			SQLiteDataAdapter da = new SQLiteDataAdapter(sql, conn);

			DataTable dt = new DataTable();
			da.Fill(dt);


			foreach (DataRow dr in dt.Rows)
			{
				string word = dr["word"].ToString();

				string json = File.ReadAllText(@"D:\JSON\" + word + ".txt");

				saveToDB(word, deserializeJson(json));

			}
		}

		private void button4_Click(object sender, EventArgs e)
		{
			try
			{
				using (WebClient wc = new WebClient())
				{
					wc.DownloadFile("http://www.collinsdictionary.com/dictionary/english/affluent", @"C:\Users\Anindya\Desktop\affluent.html");
				}
			}
			catch { }
		}

		bool mouseDown = false;

		private void TestForm_MouseDown(object sender, MouseEventArgs e)
		{
			mouseDown = true;
		}

		private void TestForm_MouseMove(object sender, MouseEventArgs e)
		{
			// MessageBox.Show(this.)
		}

		private void button5_Click(object sender, EventArgs e)
		{


			string html = string.Empty;



			string exampleDivPat = "(?<=<div\\sid=\"examples_box\">)(.*?)(?=<div\\sid=\"advert_box\"\\sclass=\"term-subsec\">)";
			string sentencePat = "(?<=<blockquote>)(.*?)(?=</blockquote>)";

			string dbfile = @"C:\Users\Anindya\Desktop\a.sqlite";
			var conn = new SQLiteConnection("Data Source=" + dbfile);
			conn.Open();



			DirectoryInfo di = new DirectoryInfo(@"D:\Sentences");
			foreach (FileInfo fi in di.GetFiles())
			{
				html = File.ReadAllText(fi.FullName);


				Regex rex = new Regex(exampleDivPat, RegexOptions.Singleline);
				Match exampleDivMat = rex.Match(html);

				if (exampleDivMat.Success)
				{
					
					rex = new Regex(sentencePat, RegexOptions.Singleline);
					MatchCollection sentenceMat = rex.Matches(exampleDivMat.ToString());

					
					if (sentenceMat.Count > 0)
					{
						
						foreach(Match mat in sentenceMat){

							SQLiteCommand cmd = conn.CreateCommand();
							cmd.CommandText = String.Format("INSERT INTO grewords_sentence (word, sentence) VALUES (@0, @1);");
							SQLiteParameter param = new SQLiteParameter("@0", System.Data.DbType.String);
							param.Value = fi.Name;
							cmd.Parameters.Add(param);


							param = new SQLiteParameter("@1", System.Data.DbType.String);
							param.Value = mat.Value;
							cmd.Parameters.Add(param);


							

							try
							{
								cmd.ExecuteNonQuery();
							}
							catch (Exception exc1)
							{
								MessageBox.Show(exc1.Message);
							}
							

						}
					}
				}
			}
			con.Close();
		}

		

		private void button6_Click(object sender, EventArgs e)
		{
			
			
		}

		private void TestForm_Load(object sender, EventArgs e)
		{

		}

		bool enlarged = false;

		private void button7_Click(object sender, EventArgs e)
		{
			
			
		}

		private void button8_Click(object sender, EventArgs e)
		{
			string dbfile = @"C:\Users\Anindya\Desktop\a.sqlite";
			var conn = new SQLiteConnection("Data Source=" + dbfile);
			conn.Open();

			string sql = "SELECT * FROM grewords ORDER BY word";

			SQLiteDataAdapter da = new SQLiteDataAdapter(sql, conn);
			conn.Close();

			DataTable dt = new DataTable();
			da.Fill(dt);

			string str = "";
			foreach (DataRow dr in dt.Rows)
			{
				string word = dr["word"].ToString().Replace("'", "''");
				string meaning = dr["meaning"].ToString().Replace("'", "''");
				string sentence = dr["sentence"].ToString().Replace("'", "''");


				str += "INSERT INTO \"grewords\" VALUES('" + word + "', '" + meaning + "', '" + sentence + "');" + "\n";

				File.WriteAllText(@"C:\Users\Anindya\Desktop\grewords_sorted.sql", str);
			}

			

		}

		private void button9_Click(object sender, EventArgs e)
		{
			//'\\x3c', '\\x3e', '\\x26', '\\x22', '\\x27', '\\x3d'), array('<', '>', '&', '\"', '\'', '=')
			
			Dictionary<string, string> replace = new Dictionary<string, string>();

			replace.Add("\\x3c", "<");
			replace.Add("\\x3e", ">");
			replace.Add("\\x26", "&");
			replace.Add("\\x22", "\"");
			replace.Add("\\x27", "'");
			replace.Add("\\x3d", "=");

			string json = string.Empty;
			

			DirectoryInfo di = new DirectoryInfo(@"D:\GRE Resources\meaning");
			foreach (FileInfo file in di.GetFiles())
			{
				string word = file.Name.Substring(0, file.Name.LastIndexOf('.'));

				using (WebClient wc = new WebClient())
				{
					//all = "http://www.google.com/dictionary/json?callback=dict_api.callbacks.id100&q=abandon&sl=en&tl=en&restrict=pr%2Cde%2Csy&client=te"


					// string url = "http://www.google.com/dictionary/json?callback=dict_api.callbacks.id100&q=abash&sl=en&tl=en";
					string url = "http://www.google.com/dictionary/json?callback=dict_api.callbacks.id100&q=" + word + "&sl=en&tl=en&restrict=pr%2Csy&client=te";
					wc.Encoding = Encoding.UTF8;
					json = wc.DownloadString(url);


				}

				

				json = json.Substring(json.IndexOf('{'), json.LastIndexOf('}') - json.IndexOf('{') + 1);

				foreach (string rep in replace.Keys)
				{
					json = json.Replace(rep, replace[rep]);
				}

				File.WriteAllText(@"D:\GRE Resources\GDic\" + word + ".txt", json, Encoding.UTF8);
			}
			
		}

		private void button10_Click(object sender, EventArgs e)
		{
			var conn = new SQLiteConnection(AppConfig.DB_CONNECTION_STRING);
			conn.Open();

			string sql = "SELECT * FROM grewords ORDER BY word";

			SQLiteDataAdapter da = new SQLiteDataAdapter(sql, conn);
			conn.Close();

			DataTable dt = new DataTable();
			da.Fill(dt);
			
			foreach(DataRow dr in dt.Rows){
				string word = dr["word"].ToString();
				
				using (WebClient wc = new WebClient()){
					wc.Encoding = Encoding.UTF8;
					File.WriteAllText(@"D:\GRE Resources\DicRef\" + word + ".html", wc.DownloadString("http://dictionary.reference.com/browse/" + word), Encoding.UTF8);
				}

				
			}

			
		}

		private void button11_Click(object sender, EventArgs e)
		{
			SQLiteConnection conn = new SQLiteConnection(AppConfig.DB_CONNECTION_STRING);
			conn.Open();
			string path = @"D:\GRE Resources\DicRef";
			DirectoryInfo di = new DirectoryInfo(path);

			List<string> remove = new List<string>();
			remove.Add("</span><div class=\"sentelipsis\"></div>&nbsp;<span class=\"sentcont\">");
			remove.Add("<span>");
			remove.Add("</span>");


			string sentencePat = "(?<=<div\\sclass=\"exsentences\">)(.*?)(?=</div>)";
			

			Regex rex = new Regex(sentencePat, RegexOptions.Singleline);
			

			foreach (FileInfo fi in di.GetFiles())
			{
				string html = File.ReadAllText(fi.FullName);

				foreach (string str in remove)
				{
					html = html.Replace(str, "");
				}

				MatchCollection sentenceMat = rex.Matches(html);

				if (sentenceMat.Count > 0)
				{

					//MessageBox.Show(sentenceMat.Groups.Count.ToString());
					foreach (Match mat in sentenceMat)
					{

						// MessageBox.Show(mat.Value);
						
						SQLiteCommand cmd = conn.CreateCommand();
						cmd.CommandText = String.Format("INSERT INTO dicref_sentence (word, sentence) VALUES (@0, @1);");
						SQLiteParameter param = new SQLiteParameter("@0", System.Data.DbType.String);
						param.Value = fi.Name.Substring(0, fi.Name.LastIndexOf('.'));
						cmd.Parameters.Add(param);


						param = new SQLiteParameter("@1", System.Data.DbType.String);
						param.Value = mat.Value;
						cmd.Parameters.Add(param);




						try
						{
							cmd.ExecuteNonQuery();
						}
						catch (Exception exc1)
						{
							MessageBox.Show(exc1.Message);
						}


					}
				}
				



			}

			conn.Close();


		}

		private void button12_Click(object sender, EventArgs e)
		{

			List<string> not_found = new List<string>();

			var conn = new SQLiteConnection(AppConfig.DB_CONNECTION_STRING);
			conn.Open();

			string sql = "SELECT * FROM bangla_academy ORDER BY word";

			SQLiteDataAdapter da = new SQLiteDataAdapter(sql, conn);
			conn.Close();

			DataTable dt = new DataTable();
			da.Fill(dt);

			foreach (DataRow dr in dt.Rows)
			{
				string word = dr["word"].ToString();

				string URI = "http://www.bracu.ac.bd/research/crblp/demo/e2bDictionary/e2bv1.5/index.php/application/search_details/";
				string myParameters = "function_name=" + word;

				using (WebClient wc = new WebClient())
				{
					wc.Encoding = System.Text.Encoding.UTF8;
					wc.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
					string HtmlResult = wc.UploadString(URI, myParameters);
					if (HtmlResult.Contains("Sorry, no results returned"))
					{
						not_found.Add(word);
						continue;
					}

					HtmlResult = HtmlResult.Substring(0, HtmlResult.IndexOf("<!--"));

					
					//MessageBox.Show(HtmlResult);

					File.WriteAllText(@"D:\Dictionary Resources\Brac E2B\" + word + ".txt", HtmlResult, Encoding.UTF8);
				}
			}


			File.WriteAllLines(@"D:\Dictionary Resources\Brac E2B\not_found.txt", not_found.ToArray());
		}

		private void button13_Click(object sender, EventArgs e)
		{
			var conn = new SQLiteConnection(AppConfig.DB_CONNECTION_STRING);
			conn.Open();

			string sql = "SELECT * FROM grewords ORDER BY word";

			SQLiteDataAdapter da = new SQLiteDataAdapter(sql, conn);
			

			DataTable dt = new DataTable();
			da.Fill(dt);

			string html;

			foreach (DataRow dr in dt.Rows)
			{
				List<string> not_found = new List<string>();

				string word = dr["word"].ToString();

				string URI = "http://www.merriam-webster.com/thesaurus/" + word;

				using (WebClient wc = new WebClient())
				{
					try
					{
						html = wc.DownloadString(URI);
					}
					catch {

						not_found.Add(word);
						continue;
					}
				}


				string[] realtions = { "Synonyms", "Related Words", "Near Antonyms", "Antonyms" };

				foreach (string relation in realtions)
				{



					string divPat = "(?<=<div><strong>" + relation + "</strong>)(.*?)(?=</div>)";
					string entryPat = "(?<=\">)(.*?)(?=</a>)";


					Regex rex = new Regex(divPat, RegexOptions.Singleline);
					Match divMat = rex.Match(html);

					if (divMat.Success)
					{

						rex = new Regex(entryPat, RegexOptions.Singleline);
						MatchCollection entryMat = rex.Matches(divMat.ToString());


						if (entryMat.Count > 0)
						{

							string entries = string.Empty;

							foreach (Match mat in entryMat)
							{
								entries += mat.Value + "; ";
							}



							SQLiteCommand cmd = conn.CreateCommand();
							cmd.CommandText = String.Format("INSERT INTO synonym_antonym (word, relation, entries) VALUES (@0, @1, @2);");
							SQLiteParameter param = new SQLiteParameter("@0", System.Data.DbType.String);
							param.Value = word;
							cmd.Parameters.Add(param);


							param = new SQLiteParameter("@1", System.Data.DbType.String);
							param.Value = relation;
							cmd.Parameters.Add(param);

							param = new SQLiteParameter("@2", System.Data.DbType.String);
							param.Value = entries;
							cmd.Parameters.Add(param);

							// MessageBox.Show(word + "\n" + relation + "\n" + entries);

							try
							{
								
								cmd.ExecuteNonQuery();
							}
							catch (Exception exc1)
							{
								MessageBox.Show(exc1.Message);
							}



						}
					}
				}

				File.WriteAllLines(@"D:\no_syn.txt", not_found.ToArray());
			}
			conn.Close();

		}

		private void button14_Click(object sender, EventArgs e)
		{
			var conn = new SQLiteConnection(AppConfig.DB_CONNECTION_STRING);
			conn.Open();

			string sql = "SELECT * FROM grewords ORDER BY word";

			SQLiteDataAdapter da = new SQLiteDataAdapter(sql, conn);
			

			DataTable dt = new DataTable();
			da.Fill(dt);

			List<string> not_found = new List<string>();

			
			foreach (DataRow dr in dt.Rows)
			{
				
				string word = dr["word"].ToString();

				sql = "SELECT * FROM synonym_antonym WHERE word = '" + word + "'";

				da = new SQLiteDataAdapter(sql, conn);


				dt = new DataTable();
				da.Fill(dt);

				if (dt.Rows.Count == 0) not_found.Add(word);
			}

			File.WriteAllLines(@"D:\not_found.txt", not_found.ToArray());


		}

		private void button15_Click(object sender, EventArgs e)
		{
			var conn = new SQLiteConnection(AppConfig.DB_CONNECTION_STRING);
			conn.Open();

			string sql = "SELECT * FROM grewords ORDER BY word";

			SQLiteDataAdapter da = new SQLiteDataAdapter(sql, conn);


			DataTable dt = new DataTable();
			da.Fill(dt);

			string html;

			List<string> not_found = new List<string>();
			foreach (DataRow dr in dt.Rows)
			{
				

				string word = dr["word"].ToString();

				string URI = "http://thesaurus.com/browse/" + word;

				using (WebClient wc = new WebClient())
				{
					try
					{
						html = wc.DownloadString(URI);
					}
					catch
					{

						not_found.Add(word);
						continue;
					}
				}


				
				string tablePat = "(<table)(.*?)(</table>)";
				string trPat = "<tr>(.*?)Main Entry(.*?)" + word + "(.*?)(?=Part of Speech)";

				

				Regex rex = new Regex(tablePat, RegexOptions.Singleline);
				MatchCollection tableMats = rex.Matches(html);

				string matchedSection = string.Empty;

				foreach(Match tableMat in tableMats){

					// MessageBox.Show(tableMat.Value);

					rex = new Regex(trPat, RegexOptions.Singleline);
					Match trMat = rex.Match(tableMat.ToString());

					if(trMat.Success){
						string table = tableMat.Value;
						string gurbage = trMat.Value;
						table = table.Replace(gurbage, "");

						//MessageBox.Show(table);
						matchedSection += table;

					}
				}

				File.WriteAllText(@"D:\Dictionary Resources\GRE Resources\SynAnt\" + word + ".txt", matchedSection);

				
			}
			conn.Close();
			File.WriteAllLines(@"D:\no_syn_thresauras.txt", not_found.ToArray());
		}

		private void button16_Click(object sender, EventArgs e)
		{
			DirectoryInfo di = new DirectoryInfo(@"D:\Dictionary Resources\GRE Resources\SynAnt");

			

			foreach (FileInfo fi in di.GetFiles())
			{

				string pos = string.Empty;
				string syn = string.Empty;
				string ant = string.Empty;

				SQLiteConnection conn = new SQLiteConnection(AppConfig.DB_CONNECTION_STRING);
				conn.Open();

				string content = File.ReadAllText(fi.FullName);


				Regex rgx1 = new Regex("(<table)(.*?)(</table>)", RegexOptions.Singleline);
				
				MatchCollection mats = rgx1.Matches(content);

				string write = "";

				foreach (Match mat1 in mats)
				{
					//MessageBox.Show("");
					string text = mat1.Value;

					Regex rgx = new Regex("</?a(.*?)>");
					text = rgx.Replace(mat1.Value, "");
					text = text.Replace("\n", "");
					text = text.Replace("\r", "");
					text = text.Replace("*", "");

					rgx = new Regex("(?<=Part of Speech:</td><td><i>)(.*?)(?=</i></td>)");
					Match mat = rgx.Match(text);
					if (mat.Success) pos = mat.Value;

					rgx = new Regex("(?<=Synonyms:</td><td><span>)(.*?)(?=</span></td>)");
					mat = rgx.Match(text);
					if (mat.Success)
					{
						syn = mat.Value;
						
						string[] entries = syn.Split(new char[] { ',' });
						syn = "";
						foreach (string entry in entries)
						{
							syn += entry.Trim() + ", ";
						}
						syn = syn.Substring(0, syn.Length - 2);

					}

					rgx = new Regex("(?<=Antonyms:</td><td><span>)(.*?)(?=</span></td>)");
					mat = rgx.Match(text);
					if (mat.Success)
					{
						ant = mat.Value;

						string[] entries = ant.Split(new char[] { ',' });
						ant = "";
						foreach (string entry in entries)
						{
							ant += entry.Trim() + ", ";
						}
						ant = ant.Substring(0, ant.Length - 2);
					}



					SQLiteCommand cmd = conn.CreateCommand();
					cmd.CommandText = String.Format("INSERT INTO syn_ant(word, pos, syn, ant) VALUES (@0, @1, @2, @3);");
					SQLiteParameter param = new SQLiteParameter("@0", DbType.String);
					param.Value = fi.Name.Remove(fi.Name.IndexOf('.'));
					cmd.Parameters.Add(param);


					param = new SQLiteParameter("@1", System.Data.DbType.String);
					param.Value = pos;
					cmd.Parameters.Add(param);

					param = new SQLiteParameter("@2", System.Data.DbType.String);
					param.Value = syn;
					cmd.Parameters.Add(param);

					param = new SQLiteParameter("@3", System.Data.DbType.String);
					param.Value = ant;
					cmd.Parameters.Add(param);

					// MessageBox.Show(word + "\n" + relation + "\n" + entries);

					try
					{

						cmd.ExecuteNonQuery();
					}
					catch (Exception exc1)
					{
						MessageBox.Show(exc1.Message);
					}






					text = "POS: " + pos + "\n" + "SYN: " + syn + "\n" + "ANT: " + ant + "\n";
					write += text;
				}
				File.WriteAllText(@"D:\Dictionary Resources\GRE Resources\SynAntRefined\" + fi.Name, write);
			}
		}

		private void button17_Click(object sender, EventArgs e)
		{
			List<string> not_found = new List<string>();

			DirectoryInfo di = new DirectoryInfo(@"D:\Dictionary Resources\GRE Resources\SynAnt");
			foreach (FileInfo fi in di.GetFiles())
			{
				if (fi.Length == 0) not_found.Add(fi.Name.Remove(fi.Name.IndexOf('.')));
			}

			File.WriteAllLines(@"D:\no_syn_thesaurus.txt", not_found.ToArray());
		}

		private void button18_Click(object sender, EventArgs e)
		{
			var conn = new SQLiteConnection(AppConfig.DB_CONNECTION_STRING);
			conn.Open();

			string sql = "SELECT * FROM grewords ORDER BY word";

			SQLiteDataAdapter da = new SQLiteDataAdapter(sql, conn);


			DataTable dt = new DataTable();
			da.Fill(dt);

			string html;

			List<string> not_found = new List<string>();
			foreach (DataRow dr in dt.Rows)
			{


				string word = dr["word"].ToString();

				string URI = "http://dictionary.reverso.net/english-synonyms/" + word;

				using (WebClient wc = new WebClient())
				{
					try
					{
						html = wc.DownloadString(URI);
					}
					catch
					{

						not_found.Add(word);
						continue;
					}
				}



				Regex rgx = new Regex("<div id=\"ctl00_cC_translate_box(.*?)</div>\r\n</font>", RegexOptions.Singleline);
				Match mat = rgx.Match(html);
				if (mat.Success)
				{
					html = mat.Value;
					rgx = new Regex("</?div(.*?)>");
					html = rgx.Replace(html, "");

					rgx = new Regex("</?span(.*?)>");
					html = rgx.Replace(html, "");

					MessageBox.Show(html);
				}
			}

			
		}

		private void button19_Click(object sender, EventArgs e)
		{
			var conn = new SQLiteConnection(AppConfig.DB_CONNECTION_STRING);
			conn.Open();

			string sql = "SELECT * FROM grewords ORDER BY word";

			SQLiteDataAdapter da = new SQLiteDataAdapter(sql, conn);


			DataTable dt = new DataTable();
			da.Fill(dt);

			string html;

			List<string> not_found = new List<string>();
			foreach (DataRow dr in dt.Rows)
			{


				string word = dr["word"].ToString();

				string URI = "http://dictionary.cambridge.org/dictionary/british/" + word;

				using (WebClient wc = new WebClient())
				{
					try
					{
						html = wc.DownloadString(URI);
					}
					catch
					{

						not_found.Add(word);
						continue;
					}
				}

				//File.WriteAllText(@"D:\Dictionary Resources\GRE Resources\Cambridge\" + word + ".html", html, Encoding.UTF8);


				HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
				doc.LoadHtml(html);

				foreach (HtmlNode node in doc.DocumentNode.SelectNodes("//span[@class='examp']"))
				{
					MessageBox.Show(node.InnerHtml);
				}
				
				//Regex rgx = new Regex("<div id=\"ctl00_cC_translate_box(.*?)</div>\r\n</font>", RegexOptions.Singleline);
				//Match mat = rgx.Match(html);
				//if (mat.Success)
				//{
				//    html = mat.Value;
				//    rgx = new Regex("</?div(.*?)>");
				//    html = rgx.Replace(html, "");

				//    rgx = new Regex("</?span(.*?)>");
				//    html = rgx.Replace(html, "");

				//    MessageBox.Show(html);
				//}
			}
		}

		private void button20_Click(object sender, EventArgs e)
		{
			SQLiteConnection conn = new SQLiteConnection(AppConfig.DB_CONNECTION_STRING);
			conn.Open();
			


			DirectoryInfo di = new DirectoryInfo(@"D:\Dictionary Resources\GRE Resources\Mnemonic");
			foreach (FileInfo fi in di.GetFiles())
			{




				string html = File.ReadAllText(fi.FullName);
				HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
				
				html = html.Replace("\r\n", "");
				doc.LoadHtml(html);

				//MessageBox.Show(html);
				//  MessageBox.Show(html.Substring(0, html.Length - 17697));



				foreach (HtmlNode node in doc.DocumentNode.SelectNodes("//div[@class='row-fluid']"))
				{
					//MessageBox.Show(node.InnerHtml);


					if (node.InnerHtml.StartsWith("      <h2>"))
					{

						HtmlAgilityPack.HtmlDocument content = new HtmlAgilityPack.HtmlDocument();
						content.LoadHtml(node.InnerHtml);

						HtmlNode headnode = content.DocumentNode.SelectSingleNode("//h2");

						//MessageBox.Show(headnode.InnerHtml);

						int count = 0;

						if (content.DocumentNode.SelectNodes("//div[@class='span9']") == null) continue;
						foreach (HtmlNode mnemonic in content.DocumentNode.SelectNodes("//div[@class='span9']"))
						{
							// MessageBox.Show(mnemonic.InnerHtml);

							count++;

							Regex rgx = new Regex("</?i(.*?)>", RegexOptions.Singleline);
							string mn = rgx.Replace(mnemonic.InnerHtml, "");



							SQLiteCommand cmd = conn.CreateCommand();
							cmd.CommandText = String.Format("INSERT INTO mnemonic(word, mnemonic) VALUES (@0, @1);");
							SQLiteParameter param = new SQLiteParameter("@0", DbType.String);
							param.Value = headnode.InnerHtml;
							cmd.Parameters.Add(param);


							param = new SQLiteParameter("@1", System.Data.DbType.String);
							param.Value = mn.Trim();
							cmd.Parameters.Add(param);


							try
							{

								cmd.ExecuteNonQuery();
							}
							catch (Exception exc1)
							{
								MessageBox.Show(exc1.Message);
							}




							//MessageBox.Show(mn.Trim());
							if (count == 2) break;

						}


					}
				}




			}
			
		}

		private void button21_Click(object sender, EventArgs e)
		{
			var conn = new SQLiteConnection(AppConfig.DB_CONNECTION_STRING);
			conn.Open();

			string sql = "SELECT word FROM bangla_academy ORDER BY word";

			SQLiteDataAdapter da = new SQLiteDataAdapter(sql, conn);


			DataTable dt = new DataTable();
			da.Fill(dt);
			conn.Close();

			List<string> not_found = new List<string>();

			foreach (DataRow dr in dt.Rows)
			{
				if (!File.Exists(@"D:\Dictionary Resources\Brac E2B\" + dr["word"].ToString() + ".txt"))
				{
					not_found.Add(dr["word"].ToString());
				}
			}


			File.WriteAllLines(@"C:\Users\Anindya\Desktop\BracNotFound.txt", not_found.ToArray());

		}

		private void button21_Click_1(object sender, EventArgs e)
		{
			string[] words = File.ReadAllLines(@"C:\Users\Anindya\Desktop\OxfordSentenceNotFound.txt");

			var conn = new SQLiteConnection(AppConfig.DB_CONNECTION_STRING);
			conn.Open();

			//string sql = "SELECT * FROM grewords ORDER BY word";

			//SQLiteDataAdapter da = new SQLiteDataAdapter(sql, conn);


			//DataTable dt = new DataTable();
			//da.Fill(dt);

			string html;

			List<string> not_found = new List<string>();
			//foreach (DataRow dr in dt.Rows)
			//{


			//    string word = dr["word"].ToString();

			foreach(string word in words){

				string URI = "http://english.oxforddictionaries.com/examplesentences?q=" + "abut";

				using (WebClient wc = new WebClient())
				{
					try
					{
						wc.Encoding = Encoding.UTF8;
						html = wc.DownloadString(URI);
					}
					catch
					{
						not_found.Add(word);
						continue;
					}
				}


				Regex rgx = new Regex("class=\"exampleNumber\">(.*?)</span>", RegexOptions.Singleline);
				html =  rgx.Replace(html, "");



				HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
				doc.LoadHtml(html);

				HtmlNodeCollection nodes = doc.DocumentNode.SelectNodes("//li[@class='exampleResult']");
				if (nodes != null)
				{

					foreach (HtmlNode node in nodes)
					{


						SQLiteCommand cmd = conn.CreateCommand();
						cmd.CommandText = String.Format("INSERT INTO oxford_sentence (word, sentence) VALUES (@0, @1);");
						SQLiteParameter param = new SQLiteParameter("@0", System.Data.DbType.String);
						param.Value = word;
						cmd.Parameters.Add(param);


						param = new SQLiteParameter("@1", System.Data.DbType.String);
						param.Value = node.InnerText;
						cmd.Parameters.Add(param);

						MessageBox.Show(node.InnerText);


						try
						{
							// cmd.ExecuteNonQuery();
						}
						catch(Exception ex)
						{
							//MessageBox.Show("HI");

						}

						//MessageBox.Show(node.InnerText);
					}
				}
				else
				{
					not_found.Add(word);
				}
			}


			File.WriteAllLines(@"C:\Users\Anindya\Desktop\OxfordSentenceNotFound2.txt", not_found.ToArray());


		}

		private void button22_Click(object sender, EventArgs e)
		{
			string URI = "http://www.bracu.ac.bd/research/crblp/demo/e2bDictionary/e2bv1.5/index.php/application/ajaxsearch_navlist";
			string myParameters = "function_name=" + "abacus";

			using (WebClient wc = new WebClient())
			{
				wc.Encoding = System.Text.Encoding.UTF8;
				wc.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
				string HtmlResult = wc.UploadString(URI, myParameters);

				MessageBox.Show(HtmlResult);
			}

			//if (HtmlResult.Contains("Sorry, no results returned"))
			//{
			//   // not_found.Add(word);
			//   // continue;
			//}

			//HtmlResult = HtmlResult.Substring(0, HtmlResult.IndexOf("<!--"));

			

			
		}

		private void button23_Click(object sender, EventArgs e)
		{
			string[] words =
				File.ReadAllLines(@"E:\C# Projects\Anindya's Dictionary Assets\Wordlists\wordsEn.txt");

			List<string> sql = new List<string>();


			SQLiteConnection conn = new SQLiteConnection(AppConfig.DB_CONNECTION_STRING);
			conn.Open();

			foreach (string word in words)
			{
				SQLiteCommand cmd = conn.CreateCommand();
				sql.Add(String.Format("INSERT INTO dictionary_words (word) VALUES ('{0}');", word.Replace("'", "''")));


				//try
				//{
				//    cmd.ExecuteNonQuery();
				//}
				//catch (Exception exc1)
				//{
				//    MessageBox.Show(exc1.Message);
				//}


			}

			File.WriteAllLines(@"C:\Users\Anindya\Desktop\dicwords.sql", sql.ToArray());

		}

		private void button24_Click(object sender, EventArgs e)
		{
			using (WebClient wc = new WebClient())
			{
				wc.DownloadFile("https://encrypted-tbn2.gstatic.com/images?q=tbn:ANd9GcQFvI8aP5hYlzxi_EC1l9q99jQd64eG-ObIJ4PfVbPCYLDtjxuWWHNFHHVX", @"C:\Users\Anindya\Desktop\a.png");
			}
		}
		
		
		
		void Button25Click(object sender, EventArgs e)
		{
			
			List<string> not_found = new List<string>();
			
			int count = 0;
			
			string[] words = File.ReadAllLines(@"C:\Users\Anindya\Desktop\unique.txt", Encoding.UTF8);
			foreach(string word in words){
				count ++;
				if(!File.Exists(@"D:\Dictionary Resources\Brac E2B\" + word + ".txt") &&
				   !File.Exists(@"D:\Dictionary Resources\Brac E2B\New Comers\" + count + ".txt")) {
					
					not_found.Add(word);
					
					
				}
				
				
				continue;
				
				string URI = "http://www.bracu.ac.bd/research/crblp/demo/e2bDictionary/e2bv1.5/index.php/application/search_details/";
				string myParameters = "function_name=" + word;

				using (WebClient wc = new WebClient())
				{
					wc.Encoding = System.Text.Encoding.UTF8;
					wc.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
					string HtmlResult = wc.UploadString(URI, myParameters);
					if (HtmlResult.Contains("Sorry, no results returned"))
					{
						MessageBox.Show(word);
						continue;
					}
					else{
						HtmlResult = HtmlResult.Substring(0, HtmlResult.IndexOf("<!--"));
						File.WriteAllText(@"D:\Dictionary Resources\Brac E2B\New Comers\" + count + ".txt", HtmlResult, Encoding.UTF8);
					}
				}
			}
			
			File.WriteAllLines(@"C:\Users\Anindya\Desktop\not.txt", not_found.ToArray(), Encoding.UTF8);

		}
		
		void Button26Click(object sender, EventArgs e)
		{
			
			List<string> sim_map = new List<string>();
			
			int count = 0;
			
			string[] words = File.ReadAllLines(@"C:\Users\Anindya\Desktop\unique.txt", Encoding.UTF8);
			foreach(string word in words){
				count ++;
				
				string content = "";
				
				if(File.Exists(@"D:\Dictionary Resources\Brac E2B\New Comers\" + count + ".txt")){
					content = File.ReadAllText(@"D:\Dictionary Resources\Brac E2B\New Comers\" + count + ".txt", Encoding.UTF8);
				}
				else{
					content = File.ReadAllText(@"D:\Dictionary Resources\Brac E2B\" + word + ".txt", Encoding.UTF8);
				}
				
				string[] similars =  word.Split(new char[]{','});
				if(similars.Length == 0) similars = new string[]{word};
				
				int sim_index = 0;
				string sim_zero = "";
				
				foreach(string similar in similars){
				

					
					
					string entry = similar;
					
					Regex rgx = new Regex("\\^?[1-9]+");
					entry = rgx.Replace(entry, "");
					
					rgx = new Regex("\\((.*?)\\)");
	                entry = rgx.Replace(entry, "");
					                
	                entry = entry.Trim();
	                
	                
					if(sim_index != 0){
						
						sim_map.Add(entry + " <--> " + sim_zero);
						
						continue;
						
					}
				    sim_zero = entry;
					sim_index++;
	         
	                
	                try{

	            		//File.AppendAllText(@"D:\Dictionary Resources\Brac E2B\Brac All\" + entry + ".txt", content, Encoding.UTF8);
	                	
	                }
		            catch(Exception ex){
		                  	MessageBox.Show( word + " " + count + " " +entry + " :\n" + ex.Message );
		                	
		                
					}
				
				}
			
				
			}
			
			File.WriteAllLines(@"C:\Users\Anindya\Desktop\sim_map.txt", sim_map.ToArray(), Encoding.UTF8);
			
			
		}
        
        void Button27Click(object sender, EventArgs e)
        {
        	List<string> rows = new List<string>();
        	
        	DirectoryInfo di = new DirectoryInfo(@"D:\Dictionary Resources\Brac E2B\Brac All");
        	foreach(FileInfo fi in di.GetFiles()){
        		string word = fi.Name.Substring(0, fi.Name.LastIndexOf(".")).Replace("'", "''").ToLower();
        		string meaning = File.ReadAllText(fi.FullName, Encoding.UTF8).Replace("'", "''");
        		
        		string sql = String.Format("INSERT INTO bangla_academy_text(word, meaning) VALUES ('{0}', '{1}');", word, meaning);
        		
        		rows.Add(sql);
        		
        	}
        	
        	File.WriteAllLines(@"C:\Users\Anindya\Desktop\ba.txt", rows.ToArray(), Encoding.UTF8);
        	
        	

		}
        
        void Button28Click(object sender, EventArgs e)
        {
        	
//        	using(WebClient wc = new WebClient()){
//        		wc.Encoding = Encoding.UTF8;
//        		string all= wc.DownloadString("http://english.oxforddictionaries.com/examplesentences?page=1&pageSize=1000&q=.&type=examplesentencessearch");
//        		File.WriteAllText(@"E:\AllOxfordSentences\all.txt", all);
//        	}
        	
        	List<string> links = new List<string>();
        	for(int i =1 ; i <= 1755; i++){
        		links.Add(String.Format("<a href=\"http://english.oxforddictionaries.com/examplesentences?page={0}&pageSize=1000&q=.&type=examplesentencessearch\">{1}</i>\r\n", i, i + ".html"));
        	}
        	
        	File.WriteAllLines(@"E:\links.html", links.ToArray());
        	
        	
        }
        
        void Button29Click(object sender, EventArgs e)
        {
        	
        	
        	DirectoryInfo di = new DirectoryInfo(@"E:\AllOxfordSentences");
        	foreach(FileInfo fi in di.GetFiles()){
        		
        		if(File.Exists(@"E:\purified\" + fi.Name)) continue;
        		
        		string content = File.ReadAllText(fi.FullName);
        		
        		List<string> examples = new List<string>();
        		
        		
        		
        		
        		HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
				doc.LoadHtml(content);

				HtmlNodeCollection explNodes = doc.DocumentNode.SelectNodes("//li[@class='exampleResult']");
				
				
				
				if (explNodes != null)
				{

					foreach (HtmlNode explNode in explNodes)
					{
						//MessageBox.Show(explNode.InnerText);
						
						HtmlAgilityPack.HtmlDocument body = new HtmlAgilityPack.HtmlDocument();
						body.LoadHtml(explNode.InnerHtml);
						
						HtmlNode numNode = body.DocumentNode.SelectSingleNode("//span[@class='exampleNumber']");
						string num = numNode.InnerText;
						
						HtmlNode contentNode = body.DocumentNode.SelectSingleNode("//span[@class='exampleContent']");
						string sentence = contentNode.InnerHtml;
						
						Regex rgx = new Regex("</?span(.*?)>|</?a(.*?)>", RegexOptions.Singleline);
						sentence = rgx.Replace(sentence, "");
						
						
						//MessageBox.Show(num + ":\n" + sentence);
						
						examples.Add(num + ". " + sentence);
						
					}

        		
				}
        		
				File.WriteAllLines(@"E:\purified\"+fi.Name, examples.ToArray(), Encoding.UTF8);
				
        		//break;
        	}
        }
        
        void Button30Click(object sender, EventArgs e)
        {
        	
        	
        	DirectoryInfo di = new DirectoryInfo(@"E:\purified");
        	foreach(FileInfo fi in di.GetFiles()){
        		
        		string[] content = File.ReadAllLines(fi.FullName);
        		
        		if(content.Length != 1000){
        			MessageBox.Show(fi.Name);
        		}
        		else{
        			string destName = @"E:\pur\" + content[0].Substring(0, content[0].IndexOf(".")) + ".txt";
        			File.Copy(fi.FullName, destName);
        		}
        	}
        }
        
        void Button31Click(object sender, EventArgs e)
        {
        	SQLiteConnection conn = new SQLiteConnection(AppConfig.DB_CONNECTION_STRING);
        	conn.Open();
        	conn.ChangePassword("");
        	
        }
        
        void Button32Click(object sender, EventArgs e)
        {
//        	Regex rx = new Regex(@"\\[uU]([0-9A-F]{4})");
//        	
//        	DirectoryInfo di = new DirectoryInfo(@"E:\AllGoogleTranslate");
//        	
//        	foreach(DirectoryInfo di2 in di.GetDirectories()){
//        		foreach(FileInfo fi in di2.GetFiles()){
//        			string content = File.ReadAllText(fi.FullName, Encoding.UTF8);
//        			
//					content = rx.Replace(content, delegate(Match match) { return ((char)Int32.Parse(match.Value.Substring(2), NumberStyles.HexNumber)).ToString(); });
//					
//
//					try{
//						File.WriteAllText(@"E:\PurifiedGoogleTranslate\" + fi.Name.Substring(0, 1) + @"\" + fi.Name, content, Encoding.UTF8);
//					}
//					catch{
//						File.AppendText(@"E:\PurifiedGoogleTransError.txt\n", fi.Name, Encoding.UTF8);
//                        
//					}
//					
//					
//        		}
//        	}
        }

        private void button33_Click(object sender, EventArgs e)
        {

            //List<string> words = new List<string>();

            //using (WebClient wc = new WebClient())
            //{
            //    wc.Encoding = Encoding.UTF8;
            //    string content = wc.DownloadString("http://quizlet.com/410238/gre-333-words-flash-cards/");


            //    HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            //    doc.LoadHtml(content);

            //    HtmlNodeCollection wordNodes = doc.DocumentNode.SelectNodes("//span[@class='qWord lang-en']");

            //    if (wordNodes != null)
            //    {
            //        foreach (HtmlNode wordNode in wordNodes)
            //        {

            //            words.Add(wordNode.InnerText.ToLower());

            //        }

            //    }
            //}

            //File.WriteAllLines(@"C:\Users\Anindya\Desktop\333.txt", words.ToArray(), Encoding.UTF8);
            List<string> not_found = new List<string>();


            List<string> dbWords = new List<string>();

            var conn = new SQLiteConnection(AppConfig.DB_CONNECTION_STRING);
			conn.Open();

			string sql = "SELECT * FROM grewords ORDER BY word";

			SQLiteDataAdapter da = new SQLiteDataAdapter(sql, conn);


			DataTable dt = new DataTable();
			da.Fill(dt);

            foreach (DataRow dr in dt.Rows)
            {
                dbWords.Add(dr["word"].ToString());
            }

            string[] listWords = File.ReadAllLines(@"C:\Users\Anindya\Desktop\800.txt");

            foreach (string word in listWords)
            {
                if (!dbWords.Contains(word))
                {
                    not_found.Add(word);
                }
            }

            File.WriteAllLines(@"C:\Users\Anindya\Desktop\800_not.txt", not_found.ToArray());
        }
	}
}