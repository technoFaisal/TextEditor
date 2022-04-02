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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void loginButton_Click(object sender, EventArgs e)
        {
            string userNameG = uNameTB.Text.ToString();
            string password = pwdTB.Text.ToString();
            if (Login.checkUser(userNameG,password) && userNameG!="" && password!="")
            {
                MessageBox.Show("Login Successful!", "Login", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Form3 textEditor = new Form3(userNameG, password);
                textEditor.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Invalid Credentials!", "Login", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void exitButton_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void newUserButton_Click(object sender, EventArgs e)
        {
            Form2 user = new Form2();
            user.Show();
            this.Hide();
        }
    }
}
