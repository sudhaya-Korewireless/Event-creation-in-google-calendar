using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CalendarApi
{
    public partial class RequestForm : Form
    {
        public RequestForm()
        {
            InitializeComponent();
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            DateTime Startdate = StartDate.Value;
            DateTime Enddate = FinishDate.Value;
            string RawMailId = textBox1.Text;
            string[] MailIds = RawMailId.Split(',');
            //validation 
            var isValid = true;
            foreach (var mail in MailIds)
            {
                if (!Regex.IsMatch(mail, @"^([a-zA-Z0-9_\-\.]+)@([a-zA-Z0-9_\-\.]+)\.([a-zA-Z]{2,5})$"))
                {
                    isValid = false;
                    break;
                }
            }
            if (isValid)
            {
                CalendarClass calendar = new CalendarClass();
                richTextBox1.AppendText(calendar.HostAnAppoinment(Startdate, Enddate, MailIds));
            }
            else
            {
                MessageBox.Show("All email is are not valid");
            }

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void RequestForm_Load(object sender, EventArgs e)
        {

        }
    }
}
