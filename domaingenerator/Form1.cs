using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Security.Cryptography;
using System.Globalization;
using System.Web;
using System.Net;

namespace domaingenerator
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            //MessageBox.Show(dateTimePicker1.Value.ToShortDateString());
            DateTime dt = this.dateTimePicker1.Value.Date;
            
            textBox1.Text = get_dom(dt, "mesachnii");
            textBox2.Text = get_dom(dt, "nedelnii");
            textBox3.Text = get_dom(dt, "dnevnoi");
           
        }


        public string get_dom(DateTime dt2, string typedom)
        {
            DateTime time = dt2;
            string date_string = "";
            string hash = "";
            string week = "";
            switch (typedom)
            {
                case "mesachnii":
                    date_string = String.Format("{0:MMyyyy}", time);
                    break;
                case "nedelnii":
                    week = GetWeekOfMonth(time).ToString();
                    date_string = week + String.Format("{0:MMyyyy}", time);
                    break;
                case "dnevnoi":
                    week = GetWeekOfMonth(time).ToString();
                    date_string = time.Day + week + String.Format("{0:MMyyyy}", time);
                    break;
                default:
                    return typedom;
                    break;
            }
            using (MD5 md5Hash = MD5.Create())
            {
                hash = GetMd5Hash(md5Hash, date_string + "0");
            }
            if (hash.Length >= 12)
            {
                return "http://" + hash.Substring(0, 12) + ".ru";
            }
            return typedom;
        }

        public int GetWeekOfYear(DateTime date)
        {
            if (date == null)
                return 0;

            DateTimeFormatInfo dfi = DateTimeFormatInfo.CurrentInfo;
            Calendar cal = dfi.Calendar;

            return cal.GetWeekOfYear(date, dfi.CalendarWeekRule, dfi.FirstDayOfWeek);
        }


        public int GetWeekOfMonth(DateTime date)
        {
            if (date == null)
                return 0;

            return GetWeekOfYear(date) - GetWeekOfYear(new DateTime(date.Year, date.Month, 1)) + 1;
        }

        string GetMd5Hash(MD5 md5Hash, string input)
        {

            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            DateTime dt = this.dateTimePicker1.Value.Date;

            textBox1.Text = get_dom(dt, "mesachnii");
            textBox2.Text = get_dom(dt, "nedelnii");
            textBox3.Text = get_dom(dt, "dnevnoi");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            WebClient wc = new WebClient();
            string html = wc.DownloadString("https://twitter.com/fbc5650c4036");
            webBrowser1.DocumentText = html;
        }
    }
}
