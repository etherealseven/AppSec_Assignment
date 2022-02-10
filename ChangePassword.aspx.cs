using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Assignment
{
    public partial class ChangePassword : System.Web.UI.Page
    {
        string MYDBConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["UserDB"].ConnectionString;
        static string finalHash;
        static string salt;
        byte[] Key;
        byte[] IV;

        protected void Page_Load(object sender, EventArgs e)
        {
            var userid = Session["UserID"].ToString();
            DateTime pwChangeTime = checkChangeTime(userid) ?? DateTime.Now;

            if (Session["UserID"] != null && Session["AuthToken"] != null && Request.Cookies["AuthToken"] != null)
            {
                if (!Session["AuthToken"].ToString().Equals(Request.Cookies["AuthToken"].Value))
                {
                    Response.Redirect("Login.aspx", false);
                }
                else
                {
                    form1.Visible = true;
                }
            }
            else
            {
                Response.Redirect("Login.aspx", false);
            }

            if (DateTime.Now < pwChangeTime.AddMinutes(2))
            {
                lbl_error.Text = "You are not allowed to change your password yet.";
                lbl_error.ForeColor = Color.Red;
            }
        }

        protected DateTime? checkChangeTime(string userid)
        {
            DateTime? h = null;

            SqlConnection connection = new SqlConnection(MYDBConnectionString);
            string sql = "select passwordCreated FROM Accounts WHERE Email=@USERID";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@USERID", userid);

            try
            {
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["passwordCreated"] != null)
                        {
                            if (reader["passwordCreated"] != DBNull.Value)
                            {
                                h = DateTime.Parse(reader["passwordCreated"].ToString());
                            }
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }

            finally
            {
                connection.Close();
            }

            return h;
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

        protected string getDBHash(string userid)
        {
            string h = null;

            SqlConnection connection = new SqlConnection(MYDBConnectionString);
            string sql = "select password FROM Accounts WHERE Email=@USERID";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@USERID", userid);

            try
            {
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["password"] != null)
                        {
                            if (reader["password"] != DBNull.Value)
                            {
                                h = reader["password"].ToString();
                            }
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }

            finally
            {
                connection.Close();
            }

            return h;
        }

        protected string getPassword1(string userid)
        {
            string h = null;

            SqlConnection connection = new SqlConnection(MYDBConnectionString);
            string sql = "select password1 FROM Accounts WHERE Email=@USERID";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@USERID", userid);

            try
            {
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["password1"] != null)
                        {
                            if (reader["password1"] != DBNull.Value)
                            {
                                h = reader["password1"].ToString();
                            }
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }

            finally
            {
                connection.Close();
            }

            return h;
        }

        protected string getPassword2(string userid)
        {
            string h = null;

            SqlConnection connection = new SqlConnection(MYDBConnectionString);
            string sql = "select password2 FROM Accounts WHERE Email=@USERID";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@USERID", userid);

            try
            {
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["password2"] != null)
                        {
                            if (reader["password2"] != DBNull.Value)
                            {
                                h = reader["password2"].ToString();
                            }
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }

            finally
            {
                connection.Close();
            }

            return h;
        }

        protected string getDBSalt(string userid)
        {
            string s = null;
            SqlConnection connection = new SqlConnection(MYDBConnectionString);
            string sql = "select passwordSalt FROM Accounts WHERE Email=@USERID";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@USERID", userid);
            try
            {
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["passwordSalt"] != null)
                        {
                            if (reader["passwordSalt"] != DBNull.Value)
                            {
                                s = reader["passwordSalt"].ToString();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally { connection.Close(); }
            return s;
        }

        protected void changeBtn_Click(object sender, EventArgs e)
        {
            var old_pw = HttpUtility.HtmlEncode(oldpw_tb.Text);
            var new_pw = HttpUtility.HtmlEncode(newpw_tb.Text);
            var confirm_pw = HttpUtility.HtmlEncode(confirmpw_tb.Text);
            SHA512Managed hashing = new SHA512Managed();

            var userid = Session["UserID"].ToString();
            DateTime pwChangeTime = checkChangeTime(userid) ?? DateTime.Now;

            string dbHash = getDBHash(userid);
            string dbSalt = getDBSalt(userid);
            string password1 = getPassword1(userid);
            string password2 = getPassword2(userid);

            int scores = checkPassword(new_pw);
            string status = "";

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

            lbl_newpw.Text = "Status: " + status;
            if (scores < 5)
            {
                lbl_newpw.ForeColor = Color.Red;
                return;
            }

            if (old_pw.Length == 0)
            {
                lbl_oldpw.Text = "Required";
                lbl_oldpw.ForeColor = Color.Red;
            }
            else if (new_pw.Length == 0)
            {
                lbl_newpw.Text = "Required";
                lbl_newpw.ForeColor = Color.Red;
            }
            else if (confirm_pw.Length == 0)
            {
                lbl_confirmpw.Text = "Required";
                lbl_confirmpw.ForeColor = Color.Red;
            }

            try
            {
                if (dbSalt != null && dbSalt.Length > 0 && dbHash != null && dbHash.Length > 0)
                {
                    string pwdWithSalt = old_pw + dbSalt;
                    byte[] hashWithSalt = hashing.ComputeHash(Encoding.UTF8.GetBytes(pwdWithSalt));
                    string userHash = Convert.ToBase64String(hashWithSalt);

                    if (userHash.Equals(dbHash))
                    {
                        RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
                        byte[] saltByte = new byte[8];

                        rng.GetBytes(saltByte);
                        salt = Convert.ToBase64String(saltByte);

                        string newpwWithSalt = new_pw + dbSalt;
                        byte[] plainHash = hashing.ComputeHash(Encoding.UTF8.GetBytes(new_pw));
                        byte[] newhashWithSalt = hashing.ComputeHash(Encoding.UTF8.GetBytes(newpwWithSalt));

                        finalHash = Convert.ToBase64String(newhashWithSalt);

                        RijndaelManaged cipher = new RijndaelManaged();
                        cipher.GenerateKey();
                        Key = cipher.Key;
                        IV = cipher.IV;
                        
                        if (DateTime.Now < pwChangeTime.AddMinutes(2))
                        {
                            lbl_error.Text = "You are not allowed to change your password yet.";
                            lbl_error.ForeColor = Color.Red;

                            Response.Redirect("ChangePassword.aspx", false);
                        }
                        else
                        {
                            if (password1 == finalHash || password2 == finalHash)
                            {
                                lbl_newpw.Text = "Password cannot be reused";
                                lbl_newpw.ForeColor = Color.Red;
                            }
                            else
                            {
                                if (new_pw == confirm_pw)
                                {

                                    try
                                    {
                                        using (SqlConnection con = new SqlConnection(MYDBConnectionString))
                                        {
                                            using (SqlCommand updateNew = new SqlCommand("UPDATE Accounts SET password=@new_pw WHERE Email=@USERID"))
                                            {
                                                using (SqlDataAdapter sda = new SqlDataAdapter())
                                                {
                                                    updateNew.CommandType = CommandType.Text;
                                                    updateNew.Parameters.AddWithValue("@USERID", userid);
                                                    updateNew.Parameters.AddWithValue("@new_pw", finalHash);

                                                    updateNew.Connection = con;
                                                    con.Open();
                                                    updateNew.ExecuteNonQuery();
                                                    con.Close();
                                                }
                                            }

                                            if (password1 != null && password2 == null)
                                            {
                                                using (SqlCommand updateNew = new SqlCommand("UPDATE Accounts SET password2=@new_pw WHERE Email=@USERID"))
                                                {
                                                    using (SqlDataAdapter sda = new SqlDataAdapter())
                                                    {
                                                        updateNew.CommandType = CommandType.Text;
                                                        updateNew.Parameters.AddWithValue("@USERID", userid);
                                                        updateNew.Parameters.AddWithValue("@new_pw", finalHash);

                                                        updateNew.Connection = con;
                                                        con.Open();
                                                        updateNew.ExecuteNonQuery();
                                                        con.Close();
                                                    }
                                                }
                                            }

                                            else if (password1 != null && password2 != null)
                                            {
                                                using (SqlCommand updateNew = new SqlCommand("UPDATE Accounts SET password1=@new_pw WHERE Email=@USERID"))
                                                {
                                                    using (SqlDataAdapter sda = new SqlDataAdapter())
                                                    {
                                                        updateNew.CommandType = CommandType.Text;
                                                        updateNew.Parameters.AddWithValue("@USERID", userid);
                                                        updateNew.Parameters.AddWithValue("@new_pw", finalHash);

                                                        updateNew.Connection = con;
                                                        con.Open();
                                                        updateNew.ExecuteNonQuery();
                                                        con.Close();
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        throw new Exception(ex.ToString());
                                    }

                                    Response.Redirect("Homepage.aspx", false);
                                }
                                else
                                {
                                    lbl_confirmpw.Text = "Passwords do not match!";
                                    lbl_confirmpw.ForeColor = Color.Red;
                                }
                            }
                        }
                    }
                    else
                    {
                        lbl_oldpw.Text = "Incorrect password";
                        lbl_oldpw.ForeColor = Color.Red;
                    }
                   
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }
    }
}