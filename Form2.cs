using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TextEditor
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
            Form1 homePage = new Form1();
            homePage.Show();
        }

        private void submitButton_Click(object sender, EventArgs e)
        {
            Form1 homePage = new Form1();
            
            bool check = true;
            if (uNameTB.Text.Equals("") || pwdTB.Text.Equals("") || rePwdTB.Text.Equals("") || fNameTB.Text.Equals("") || lNameTB.Text.Equals("") || userTypeCB.Text.Equals(""))
            {
                MessageBox.Show("Please enter all the details!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                check = false;
            }

            else if (pwdTB.Text != rePwdTB.Text)
            {
                MessageBox.Show("Please check the entered and re entered password!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                check = false;
            }

            else
            {
                check = true;
            } 

            if (check)
            {
                string userData = "";
                string userName = uNameTB.Text;
                string password = pwdTB.Text;
                string firstName = fNameTB.Text;
                string lastName = lNameTB.Text;
                string dateOfBirth = dateTimePicker1.Value.ToString("dd-mm-yyyy");
                string userAccessType = userTypeCB.Text;
                userData = "Registration success!! \nUsername:" + userName + "Name:" + firstName + " " + lastName;
                Login users = new Login();
                if (!users.checkUser(userName))
                {
                    DialogResult dialog = MessageBox.Show(userData, "Registration Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    users.addNewUser(userName, password, userAccessType, firstName, lastName, dateOfBirth);
                    if (dialog == DialogResult.OK)
                    {
                        this.Close();
                        homePage.Show();
                    }
                }
                else
                {
                    MessageBox.Show("User already exist!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    this.Hide();

                    homePage.Show();
                }
            }
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }
        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
            Form1 homePage = new Form1();
            homePage.Show();
        }

    }
}
