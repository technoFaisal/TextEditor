using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace TextEditor
{
    public partial class Form3 : Form
    {
        private string userName = ""; // user name
        private string accessType = ""; // editable or just view
        private string fileName = "";
        private int size = 9; // initital font sizze
        
        public Form3()
        {
            InitializeComponent();
        }

        public Form3(string user, string password) // dedicated form based on user name and password
        {
            InitializeComponent();
            Login users = new Login();
          
            this.userName = user;
            
            richTextBox1.ReadOnly = true;

            if (users.userEditAccess(userName, password)) // genereate form on the basis of access type
            {
                accessType = "Edit";
                richTextBox1.ReadOnly = false;
            }
            else
            {
                accessType = "View";
                richTextBox1.ReadOnly = true;
            }
        }
        private void newFile()  // create a new empty file
        {
            richTextBox1.Clear();
            fileName = "";
        }
        private void openFile() 
        {
            Stream stream;
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Open a Text File"; // dialog title
            openFileDialog.Filter = "Text Files (*.txt)|*.txt|Rich Text Format (*.rtf)|*.rtf|All Files(*.*)|*.*"; // choose specific type of formaats

            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if ((stream = openFileDialog.OpenFile()) != null)
                {
                    fileName = openFileDialog.FileName; // selected file if not null
                    string fileExt = fileName.Substring(fileName.IndexOf(".") + 1);
                    if (fileExt.Equals("rtf"))
                    {
                        richTextBox1.LoadFile(openFileDialog.FileName); // load on the selected rich text box area
                    }
                    else if (fileExt.Equals("txt"))
                    {
                        Font deFont = new Font("Arial", 9, FontStyle.Regular); // basic font
                        string fileText = File.ReadAllText(fileName);
                        richTextBox1.Text = fileText;
                        richTextBox1.Font = DefaultFont;
                    }
                    else
                    {
                        MessageBox.Show("Unsupported file.");
                    }
                }
                stream.Close();
            }

            toolStripComboBox1.Text = "9";
        }


        private void saveFile()
        {
            if (fileName.Equals("")) // file name not mentioned before
            {
                saveAsFile();
            }
            else if (fileName.Substring(fileName.IndexOf(".") + 1).Equals("txt")) // storing the previously stored file
            {
                saveAsFile();
            }
            else
            {
                richTextBox1.SaveFile(fileName);// storing the file for the first time
            }
        }

        private void saveAsFile()
        {
            Stream myStream;
            SaveFileDialog saveFileDialog1 = new SaveFileDialog(); // create a directory dialog

            saveFileDialog1.Filter = "Rich Text Format (*.rtf)|*.rtf";
            saveFileDialog1.FilterIndex = 1;
            saveFileDialog1.RestoreDirectory = true;

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if ((myStream = saveFileDialog1.OpenFile()) != null) // move to the specified location
                {
                    fileName = saveFileDialog1.FileName;
                    myStream.Close();
                    richTextBox1.SaveFile(fileName);
                }
            }
        }

        private void setFontStyle(FontStyle style)
        {
            if (!accessType.Equals("Edit")) // if the user doesnt has edit access
            {
                return;
            }
            if (style != FontStyle.Bold && style != FontStyle.Italic && style != FontStyle.Underline)
            {
                throw new System.InvalidProgramException("Wrong font style.");
            }

            RichTextBox tempRichTextBox = new RichTextBox();

            int curRtbStart = richTextBox1.SelectionStart;
            int len = richTextBox1.SelectionLength;
            int tempRtbStart = 0;
            Font font = richTextBox1.SelectionFont;
            if (len <= 1 && font != null)
            {
                if (style == FontStyle.Bold && font.Bold || style == FontStyle.Italic && font.Italic || style == FontStyle.Underline && font.Underline)
                {
                    richTextBox1.SelectionFont = new Font(font, font.Style ^ style);
                }
                else if (style == FontStyle.Bold && !font.Bold || style == FontStyle.Italic && !font.Italic || style == FontStyle.Underline && !font.Underline)
                {
                    richTextBox1.SelectionFont = new Font(font, font.Style | style);
                }
                return;
            }
            tempRichTextBox.Rtf = richTextBox1.SelectedRtf;
            tempRichTextBox.Select(len - 1, 1);
            Font tempFont = (Font)tempRichTextBox.SelectionFont.Clone();

            for (int i = 0; i < len; i++)
            {
                tempRichTextBox.Select(tempRtbStart + i, 1);
                if (style == FontStyle.Bold && tempFont.Bold || style == FontStyle.Italic && tempFont.Italic || style == FontStyle.Underline && tempFont.Underline)
                {
                    tempRichTextBox.SelectionFont = new Font(tempRichTextBox.SelectionFont, tempRichTextBox.SelectionFont.Style ^ style);
                }
                else if (style == FontStyle.Bold && !tempFont.Bold || style == FontStyle.Italic && !tempFont.Italic || style == FontStyle.Underline && !tempFont.Underline)
                {
                    tempRichTextBox.SelectionFont = new Font(tempRichTextBox.SelectionFont, tempRichTextBox.SelectionFont.Style | style);
                }
            }
            tempRichTextBox.Select(tempRtbStart, len);
            richTextBox1.SelectedRtf = tempRichTextBox.SelectedRtf;
            richTextBox1.Select(curRtbStart, len);
        }


        private void setFontSize(float fontSize)
        {
            if (!accessType.Equals("Edit"))
            { 
                return;
            }
            if (fontSize <= 0.0)
            {
                throw new InvalidProgramException("The number of font size should be bigger than 0.0.");
            }

            RichTextBox tempRichTextBox = new RichTextBox();

            int curRtbStart = richTextBox1.SelectionStart;
            int len = richTextBox1.SelectionLength;
            int tempRtbStart = 0;

            Font font = richTextBox1.SelectionFont;
            if (len <= 1 && null != font)
            {
                richTextBox1.SelectionFont = new Font(font.Name, fontSize, font.Style);
                return;
            }

            tempRichTextBox.Rtf = richTextBox1.SelectedRtf;
            for (int i = 0; i < len; i++)
            {
                tempRichTextBox.Select(tempRtbStart + i, 1);
                tempRichTextBox.SelectionFont = new Font(tempRichTextBox.SelectionFont.Name, fontSize, tempRichTextBox.SelectionFont.Style);
            }

            tempRichTextBox.Select(tempRtbStart, len);
            richTextBox1.SelectedRtf = tempRichTextBox.SelectedRtf;
            richTextBox1.Select(curRtbStart, len);
            richTextBox1.Focus();
        }

        private void dispAbout()
        {
            Form4 about = new Form4();
            about.Show();
        }

        private void cutText()
        {
            if (!accessType.Equals("Edit"))
            {
                return;
            }
            Clipboard.SetData(DataFormats.Rtf, richTextBox1.SelectedRtf); // stpre the data in the clipboard memory
            richTextBox1.SelectedRtf = ""; // and remove from the rtb
        }

        private void copyText()
        {
            if (!accessType.Equals("Edit"))
            {
                return;
            }
            Clipboard.SetData(DataFormats.Rtf, richTextBox1.SelectedRtf);// stpre the data in the clipboard memory
        }

        private void pasteText()
        {
            if (!accessType.Equals("Edit")) return;
            richTextBox1.Paste();
        }

        private void openCtrlOToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFile();
        }

        private void newCtrlNToolStripMenuItem_Click(object sender, EventArgs e)
        {
            newFile();
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            toolStripLabel1.Text = " " + userName;
        }
        private void Form3_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }
        private void Form3_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.S) // ctrl s,ctrl N, ctrl o
            {
                saveFile();
                e.SuppressKeyPress = true;
            }
            if (e.Control && e.KeyCode == Keys.N)
            {
                newFile();
                e.SuppressKeyPress = true;
            }
            if (e.Control && e.KeyCode == Keys.O)
            {
                openFile();
                e.SuppressKeyPress = true;
            }
            
        }
      

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form1 login = new Form1();
            login.Show();
        }

        private void saveCtrlSToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFile();
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveAsFile();
        }

        private void cutCtrlXToolStripMenuItem_Click(object sender, EventArgs e)
        {
            cutText();
        }

        private void copyCtrlToolStripMenuItem_Click(object sender, EventArgs e)
        {
            copyText();
        }

        private void pasteCtrlVToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pasteText();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dispAbout();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            newFile();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            openFile();
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            saveFile();
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            saveAsFile();
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            setFontStyle(FontStyle.Bold);
        }

        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            setFontStyle(FontStyle.Italic);
        }

        private void toolStripButton7_Click(object sender, EventArgs e)
        {
            setFontStyle(FontStyle.Underline);
        }

        private void toolStripButton8_Click(object sender, EventArgs e)
        {
            dispAbout();
        }

        private void toolStripButton9_Click(object sender, EventArgs e)
        {
            cutText();
        }

        private void toolStripButton10_Click(object sender, EventArgs e)
        {
            copyText();
        }

        private void toolStripButton11_Click(object sender, EventArgs e)
        {
            pasteText();
        }

        private void toolStripComboBox1_TextChanged(object sender, EventArgs e)
        {
            
          //  setFontSize((float)Int32.TryParse(toolStripComboBox1.Text));

            bool success = int.TryParse(toolStripComboBox1.Text, out size);
            if(success) setFontSize(size);

        }
    }
}
