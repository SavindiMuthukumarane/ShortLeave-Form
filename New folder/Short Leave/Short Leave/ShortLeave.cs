using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Xml.Linq;
using System.Text.RegularExpressions;
using System.Net;
using System.Net.Mail;
using System.Diagnostics;


namespace Short_Leave
{
    
    public partial class ShortLeave: Form
    {
        string connString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\savin\OneDrive - NSBM\Attachments\New folder\Database11.accdb";

        public ShortLeave()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void clear_Click(object sender, EventArgs e)
        {
            //clear all inputs
            txtStudentName.Text = "";
            txtStudentID.Text = "";
            txtRoomNumber.Text = "";
            txtLeaveDate.Text = "";
            txtReason.Text = "";
            txtmail.Text = "";

           
    

        }

        private void Submit_Click(object sender, EventArgs e)
        {

            
            // Get values from input fields
            string studentName = txtStudentName.Text;
            string studentID = txtStudentID.Text;
            string roomNumber = txtRoomNumber.Text;
            string leaveDate = txtLeaveDate.Text;
            string reason = txtReason.Text;
            string parentmail = txtmail.Text.Trim();
            
            //prevent double @gmail.com if user typed it
            if (parentmail.EndsWith("@gmail.com"))
                parentmail = parentmail.Substring(0, parentmail.Length - 10);
            //Add @gmail.com
            parentmail += "@gmail.com";

            // Check if any field is empty
            if (string.IsNullOrWhiteSpace(studentName) ||
                string.IsNullOrWhiteSpace(studentID) ||
                string.IsNullOrWhiteSpace(roomNumber) ||
                string.IsNullOrWhiteSpace(leaveDate) ||
                string.IsNullOrWhiteSpace(reason)||
                string.IsNullOrWhiteSpace(parentmail))
            {
                MessageBox.Show("Please fill in all required fields.",
                                "Missing Information",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                return;
            }
            //Email Format Validation for Gamil address
            string emailPattern = @"^[a-zA-Z0-9._%+-]+@gmail\.com$";
            if(!Regex.IsMatch(parentmail,emailPattern))
            {
                MessageBox.Show("Please enter a valid Gmail username(e.g.,jonh.doe).Do not type @gmail.com-it is added automatically.", "Invalid Email",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            // Insert in to database
            using (OleDbConnection conn = new OleDbConnection(connString))
            {
                string query = "INSERT INTO [Short Leave] ([Student Name],[Student ID],[Room Number],[Short Leave Date],[Reason]) VALUES (?, ?, ?,?,?)";

                using (OleDbCommand cmd = new OleDbCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("?", studentName);
                    cmd.Parameters.AddWithValue("?", studentID);
                    cmd.Parameters.AddWithValue("?", roomNumber);
                    cmd.Parameters.AddWithValue("?", leaveDate);
                    cmd.Parameters.AddWithValue("?", reason);


                    try
                    {
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Leave request submitted successfully!");
                    
                    
        
                        //compose email

                        string Subject = $"Short Leave Notification for {studentName}(ID:{studentID})";
                        string Body = $" Dear Parent,\n\nThis is to inform you that your child {studentName} (ID:{studentID}) has requested a " +
                           $" short leave on {leaveDate} for the following reason:\n\n{reason}\n\n .Thank You for your cooperation.";
                        
                        //encode for URL

                       string encodedSubject =Uri.EscapeDataString(Subject);
                       string encodedBody = Uri.EscapeDataString(Body);

                        string mailtoLink = $"https://mail.google.com/mail/?view=cm&fs=1&to={parentmail}&su={Uri.EscapeDataString(Subject)}&body={Body}";


                        //open Gmail compose window
                        Process.Start(new ProcessStartInfo(mailtoLink)
                        { UseShellExecute = true });
                    }
                    

                    catch (Exception ex)
                    {
                        MessageBox.Show("Leave saved,but failed to open mail " + ex.Message);
                    }
                }
            }

            


        }

        private void Submit_MouseEnter(object sender, EventArgs e)
        {
            Submit.BackColor = ColorTranslator.FromHtml("#66CC00");

        }

        private void Submit_MouseLeave(object sender, EventArgs e)
        {
            Submit.BackColor = SystemColors.Control;

        }

        private void clear_MouseEnter(object sender, EventArgs e)
        {
           clear.BackColor = ColorTranslator.FromHtml("#C0C0C0");

        }

        private void clear_MouseLeave(object sender, EventArgs e)
        {
            clear.BackColor = SystemColors.Control;

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void txtStudentID_TextChanged(object sender, EventArgs e)
        {
            if (txtStudentID.Text.Length > 6)
            {
                MessageBox.Show("Student ID should not be longer than 6 digits.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtStudentID.Text = txtStudentID.Text.Substring(0, 6);  // Trim to 6 digits
                txtStudentID.SelectionStart = txtStudentID.Text.Length;  // Move cursor to the end
            }
        }
    }
}
    

