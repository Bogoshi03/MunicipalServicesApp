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
    public partial class ReportIssueForm : Form
    {
        private static List<Issue> reportedIssues = new List<Issue>();

        private void UpdateProgress()
        {
            int progress = 0;

            if (!string.IsNullOrWhiteSpace(txtLocation.Text))
                progress += 25;

            if (cmbCategory.SelectedIndex != -1)
                progress += 25;

            if (!string.IsNullOrWhiteSpace(rtbDescription.Text))
                progress += 25;

            if (lblSelectedFile.Text != "No file selected")
                progress += 25;

            progressReport.Value = progress;
            if (progress < 100)
            {
                lblProgress.Text = "Report completion: " + progress + "% - Keep going!";
            }
            else
            {
                lblProgress.Text = "Report completion: 100% - Your report is ready!";
            }

            if (progress == 100)
            {
                lblProgress.Text = "Report completion: 100% - Your report is ready!";
            }
        }
        public ReportIssueForm()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void lblLocation_Click(object sender, EventArgs e)
        {

        }

        private void ReportIssueForm_Load(object sender, EventArgs e)
        {

        }

        private void txtLocation_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnAttachFile_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                lblSelectedFile.Text =
                    openFileDialog.SafeFileName;
                UpdateProgress();
            }

        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtLocation.Text))
            {
                MessageBox.Show("Please enter the location of the issue.",
                    "Missing Information",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                txtLocation.Focus();
                return;
            }

            if (cmbCategory.SelectedIndex == -1)
            {
                MessageBox.Show("Please select an issue category.",
                    "Missing Information",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                cmbCategory.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(rtbDescription.Text))
            {
                MessageBox.Show("Please describe the issue.",
                    "Missing Information",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                rtbDescription.Focus();
                return;
            }

            Issue newIssue = new Issue
            {
                Location = txtLocation.Text.Trim(),
                Category = cmbCategory.SelectedItem.ToString(),
                Description = rtbDescription.Text.Trim(),
                Attachment = lblSelectedFile.Text
            };
            reportedIssues.Add(newIssue);

            int reportNumber = reportedIssues.Count;

            MessageBox.Show(
                "Report #" + reportNumber + " has been submitted successfully.",
                "Report Submitted",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);

            txtLocation.Clear();
            cmbCategory.SelectedIndex = -1;
            rtbDescription.Clear();
            lblSelectedFile.Text = "No file selected";
            progressReport.Value = 0;
            lblProgress.Text = "Report completion: 0%";
        }

        private void lblLocation_TextChanged(object sender, EventArgs e)
        {
            UpdateProgress();
        }

        private void rtbDescription_TextChanged(object sender, EventArgs e)
        {
            UpdateProgress();
        }

        private void btnAttachFile_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
