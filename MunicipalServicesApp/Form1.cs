using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MunicipalServicesApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void btnReportIssues_Click(object sender, EventArgs e)
        {
            ReportIssueForm reportForm = new ReportIssueForm();
            reportForm.Show();
        }

        private void btnLocalEvents_Click(object sender, EventArgs e)
        {
     
            EventsForm eventsForm = new EventsForm();
            eventsForm.ShowDialog();
    }
    }
}
