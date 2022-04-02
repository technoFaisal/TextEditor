using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text;
using System.IO;
using System.Collections;

namespace TextEditor
{
	public class Login
	{
		static ArrayList loginDetails = new ArrayList(); // array list to load login details
		
		public Login()
		{
			loginDetails.Clear(); // clear the loginDetails instance
			if (!File.Exists("login.txt")) // check if the login.txt file already exist
			{
				using (StreamWriter writer = File.CreateText("login.txt")) // else create one
				{
					writer.WriteLine("");
				}
			}
			using (StreamReader reader = new StreamReader("login.txt"))// access the login.txt file
			{
				string line;
				while ((line = reader.ReadLine()) != null) // read all the lines
				{
					loginDetails.Add(line); // and store in the array
				}
			}

		}

		public static bool checkUser(string userName, string password) // based on name and password
		{
            Dictionary<string, string> userData = new Dictionary<string, string>();
            TextReader reader = new StreamReader("login.txt");
            string line = reader.ReadLine();
            while (line != null)
            {
                string[] str = line.Split(new char[] { ',', '\n', '\r' });
                if (!userData.ContainsKey(str[0]))
                {
                    userData.Add(str[0], str[1]);
                };
                line = reader.ReadLine();
            }
            reader.Close();

            if (userData.ContainsKey(userName) && userData[userName] == password)
            {
                return true;
            }
            else
            {
                return false;
            }
            //bool allGood = false;
            //foreach (var data in loginDetails)
            //{
            //	if (data.ToString().Contains(userName + "," + password + ","))
            //	{
            //		allGood  = true;
            //	}
            //}
            //return allGood;

        }
		public bool checkUser(string userName) // based on name only
		{
			bool check = false;
			foreach (var data in loginDetails )
			{

				if (data.ToString().Contains(userName + ","))
				{
					check = true;
				}

			}
			return check;
		}

		public bool userEditAccess(string userName, string password) // check user access limit edit or view only
		{
			bool editAccess = false;
			foreach (var data in loginDetails)
			{
				if (data.ToString().Contains(userName + "," + password + "," + "Edit"))
				{
					editAccess = true;
				}
			}

			return editAccess;
		}

		public void addNewUser(string userName, string password, string accessType, string firstName, string lName, string date )
        {
			string line = userName + "," + password + "," + accessType+ "," + firstName + "," + lName + "," + date;
			using (StreamWriter writer = new StreamWriter("login.txt", true))
			{
				writer.WriteLine( line);
			}
		}




	}
}
