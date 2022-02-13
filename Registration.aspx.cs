using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using System.Drawing;
using System.Security.Cryptography;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.IO;

namespace Assignment
{
    public partial class Registration : Page
    {

        string MYDBConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["UserDB"].ConnectionString;
        static string finalHash;
        static string salt;
        byte[] Key;
        byte[] IV;

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        private int checkPassword(string password)
        {
            int score = 0;

            if (password.Length < 12)
            {
                return 1;
            }
            else
            {
                score = 1;
            }

            if (Regex.IsMatch(password, "[a-z]"))
            {
                score++;
            }
            else
            {
                score = 1;
            }

            if (Regex.IsMatch(password, "[A-Z]"))
            {
                score++;
            }
            else
            {
                score = 1;
            }

            if (Regex.IsMatch(password, "[0-9]"))
            {
                score++;
            }
            else
            {
                score = 1;
            }

            if (Regex.IsMatch(password, "[^A-Za-z0-9]"))
            {
                score++;
            }
            else
            {
                score = 1;
            }


            return score;
        }

        protected byte[] encryptData(string data)
        {
            byte[] cipherText = null;
            try
            {
                RijndaelManaged cipher = new RijndaelManaged();
                cipher.IV = IV;
                cipher.Key = Key;
                ICryptoTransform encryptTransform = cipher.CreateEncryptor();
                byte[] plainText = Encoding.UTF8.GetBytes(data);
                cipherText = encryptTransform.TransformFinalBlock(plainText, 0, plainText.Length);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally { }

            return cipherText;
        }

        protected void createAccount()
        {
            HttpPostedFile image = FileUpload1.PostedFile;
            string imageName = Path.GetFileName(image.FileName);
            FileUpload1.SaveAs(Server.MapPath("UploadedImages/" + imageName));

            try
            {
                using (SqlConnection con = new SqlConnection(MYDBConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("INSERT INTO Accounts VALUES(@fname, @lname, @cc, @email, @password, @passwordSalt, @image, @dob, @IV, @key, @attempt, @lockoutTime, @password1, @password2, @passwordCreated)"))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        { 

                            cmd.CommandType = CommandType.Text;
                            cmd.Parameters.AddWithValue("@fname", fname_tb.Text.Trim());
                            cmd.Parameters.AddWithValue("@lname", lname_tb.Text.Trim());
                            cmd.Parameters.AddWithValue("@cc", Convert.ToBase64String(encryptData(creditcard_tb.Text.Trim())));
                            cmd.Parameters.AddWithValue("@email", email_tb.Text.Trim());
                            cmd.Parameters.AddWithValue("@password", finalHash);
                            cmd.Parameters.AddWithValue("@passwordSalt", salt);
                            cmd.Parameters.AddWithValue("@image", imageName);
                            cmd.Parameters.AddWithValue("@dob", dob_tb.Text.Trim());
                            cmd.Parameters.AddWithValue("@IV", Convert.ToBase64String(IV));
                            cmd.Parameters.AddWithValue("@key", Convert.ToBase64String(Key));
                            cmd.Parameters.AddWithValue("@attempt", 0);
                            cmd.Parameters.AddWithValue("@lockoutTime", DateTime.Now);
                            cmd.Parameters.AddWithValue("@password1", finalHash);
                            cmd.Parameters.AddWithValue("@password2", DBNull.Value);
                            cmd.Parameters.AddWithValue("@passwordCreated", DateTime.Now);

                            cmd.Connection = con;
                            con.Open();
                            cmd.ExecuteNonQuery();  
                            con.Close();
                        }

                    }
                }
            }

            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }
        protected void Button1_Click(object sender, EventArgs e)
        {
            var fname = HttpUtility.HtmlEncode(fname_tb.Text);
            var lname = HttpUtility.HtmlEncode(lname_tb.Text);
            var cc = HttpUtility.HtmlEncode(creditcard_tb.Text);
            var email = HttpUtility.HtmlEncode(email_tb.Text);
            var password = HttpUtility.HtmlEncode(password_tb.Text);
            var dob = HttpUtility.HtmlEncode(dob_tb.Text);
            int scores = checkPassword(password);
            string status = "";
            bool checkFname = false;
            bool checkLname = false;
            bool checkCC = false;
            bool checkEmail = false;
            bool checkDOB = false;
            bool checkImage = false;
            bool checkPW = false;


            switch (scores)
            {
                case 1:
                    status = "Very Weak";
                    break;
                case 2:
                    status = "Weak";
                    break;
                case 3:
                    status = "Medium";
                    break;
                case 4:
                    status = "Strong";
                    break;
                case 5:
                    status = "Excellent";
                    break;
                default:
                    break;
            }


            lbl_pwdchecker.Text = "Status: " + status;


            if (fname.Length == 0)
            {
                lbl_fname.Text = "First name is required";
                lbl_fname.ForeColor = Color.Red;
                checkFname = false;
            }
            else if (fname.Length > 0)
            {
                lbl_fname.Text = "Valid!";
                lbl_fname.ForeColor = Color.Green;
                checkFname = true;
            }
            if (lname.Length == 0)
            {
                lbl_lname.Text = "Last name is required";
                lbl_lname.ForeColor = Color.Red;
                checkLname = false;
            }
            else if (lname.Length > 0)
            {
                lbl_lname.Text = "Valid!";
                lbl_lname.ForeColor = Color.Green;
                checkLname = true;
            }

            if (cc.Length == 0)
            {
                lbl_cc.Text = "Credit card number is required";
                lbl_cc.ForeColor = Color.Red;
                checkCC = false;
            }
            else if (cc.Length < 16 || creditcard_tb.Text.ToString().Trim().Length > 16)
            {
                lbl_cc.Text = "Invalid credit card number!";
                lbl_cc.ForeColor = Color.Red;
                checkCC = false;
            }
            else if (cc.Length == 16)
            {
                lbl_cc.Text = "Valid!";
                lbl_cc.ForeColor = Color.Green;
                checkCC = true;
            }

            if (email.Length == 0)
            {
                lbl_email.Text = "Email is required";
                lbl_email.ForeColor = Color.Red;
                checkEmail = false;
            }
            else if (Regex.IsMatch(email, @"/^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$/"))
            {
                lbl_email.Text = "Invalid email";
                lbl_email.ForeColor = Color.Red;
                checkEmail = false;
            }
            else if (email.Length > 0 && !Regex.IsMatch(email, @"/^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$/"))
            {
                lbl_email.Text = "Valid!";
                lbl_email.ForeColor = Color.Green;
                checkEmail = true;
            }

            if (password.Length == 0)
            {
                lbl_pwdchecker.Text = "Password required";
                lbl_pwdchecker.ForeColor = Color.Red;
            }
            if (scores < 5)
            {
                lbl_pwdchecker.ForeColor = Color.Red;
                checkPW = false;
            }
            else if (scores == 5)
            {
                lbl_pwdchecker.ForeColor = Color.Green;
                checkPW = true;
            }

            if (dob.Length == 0)
            {
                lbl_dob.Text = "Date of birth is required";
                lbl_dob.ForeColor = Color.Red;
                checkDOB = false;
            }
            else if (dob.Length > 0)
            {
                lbl_dob.Text = "Valid!";
                lbl_dob.ForeColor = Color.Green;
                checkDOB = true;
            }

            if (!FileUpload1.HasFile)
            {
                lbl_image.Text = "Please select an image";
                lbl_image.ForeColor = Color.Red;
                checkImage = false;
            }
            else if (FileUpload1.HasFile)
            {
                lbl_image.Text = "File selected!";
                lbl_image.ForeColor = Color.Green;
                checkImage = true;
            }

            if (checkFname == true && checkLname == true && checkCC == true && checkEmail == true && checkPW == true && checkDOB == true && checkImage == true)
            {
                RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
                byte[] saltByte = new byte[8];

                rng.GetBytes(saltByte);
                salt = Convert.ToBase64String(saltByte);

                SHA512Managed hashing = new SHA512Managed();

                string pwdWithSalt = password + salt;
                byte[] plainHash = hashing.ComputeHash(Encoding.UTF8.GetBytes(password));
                byte[] hashWithSalt = hashing.ComputeHash(Encoding.UTF8.GetBytes(pwdWithSalt));

                finalHash = Convert.ToBase64String(hashWithSalt);

                RijndaelManaged cipher = new RijndaelManaged();
                cipher.GenerateKey();
                Key = cipher.Key;
                IV = cipher.IV;

                createAccount();
                Response.Redirect("Login.aspx");
            }
               
        }
    }
}