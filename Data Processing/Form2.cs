/*
 * Created by SharpDevelop.
 * User: Anindya
 * Date: 6/1/2013
 * Time: 11:45 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace Anindya_s_Dictionary
{
	/// <summary>
	/// Description of Form2.
	/// </summary>
	public partial class TestForm3 : Form
	{
		public TestForm3()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		
		String[] words = File.ReadAllLines(@"C:\Users\Anindya\Desktop\unique.txt", Encoding.UTF8);
		int currentIndex = -1;
		
		void TestForm3Load(object sender, EventArgs e)
		{
			cursor();
		}
		
		void cursor(){
			for(int i = currentIndex + 1; i < words.Length; i++){
				
				if(words[i].Contains("(")){
					if(words[i].Substring(0, words[i].IndexOf("(")).Trim().Contains("^")){
					currentIndex = i;
					textBox1.Text = words[i];
					textBox2.Text = words[i].Substring(0, words[i].IndexOf("(")).Trim();
					
					Regex rgx = new Regex("[a-zA-Z]+");
					MatchCollection mc = rgx.Matches(textBox1.Text);
					textBox3.Text = mc[mc.Count-1].Value.Trim();
					break;
					}
				}
			}
		}
		
		
		
		void Button2Click(object sender, EventArgs e)
		{
			//File.AppendAllText(@"C:\Users\Anindya\Desktop\sim_map.txt", textBox3.Text + " <--> " + textBox2.Text + "\r\n");
			//File
			
			cursor();
		}
		
		void Button1Click(object sender, EventArgs e)
		{
			cursor();
		}
	}
}
