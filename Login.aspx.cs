using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.IO;
using System.Web.Script.Serialization;
using System.Web.Services;


namespace Assignment
{
    public partial class Login : System.Web.UI.Page
    {

        private string errorMsg;
        string MYDBConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["UserDB"].ConnectionString;
        static string finalHash;
        static string salt;
        byte[] Key;
        byte[] IV;

        public class MyObject
        {
            public string success { get; set; }
            public List<string> ErrorMessage { get; set; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public bool ValidateCaptcha()
        {
            bool result = true;

            //When user submits the recaptcha form, the user gets a response POST parameter. 
            //captchaResponse consist of the user click pattern. Behaviour analytics! AI :) 
            string captchaResponse = Request.Form["g-recaptcha-response"];

            //To send a GET request to Google along with the response and Secret key.
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create
           (" https://www.google.com/recaptcha/api/siteverify?secret=6LeCcmkeAAAAAAniXfp379ITqv1GacDNev-3mL7m &response=" + captchaResponse);


            try
            {

                //Codes to receive the Response in JSON format from Google Server
                using (WebResponse wResponse = req.GetResponse())
                {
                    using (StreamReader readStream = new StreamReader(wResponse.GetResponseStream()))
                    {
                        //The response in JSON format
                        string jsonResponse = readStream.ReadToEnd();

                        //To show the JSON response string for learning purpose
                        //lbl_error.Text = jsonResponse.ToString();

                        JavaScriptSerializer js = new JavaScriptSerializer();

                        //Create jsonObject to handle the response e.g success or Error
                        //Deserialize Json
                        MyObject jsonObject = js.Deserialize<MyObject>(jsonResponse);

                        //Convert the string "False" to bool false or "True" to bool true
                        result = Convert.ToBoolean(jsonObject.success);//

                    }
                }

                return result;
            }
            catch (WebException ex)
            {
                throw ex;
            }
        }

        protected void submitBtn_Click(object sender, EventArgs e)
        {
            var pwd = HttpUtility.HtmlEncode(password_tb.Text);
            var userid = HttpUtility.HtmlEncode(email_tb.Text);
            SHA512Managed hashing = new SHA512Managed();
            string dbHash = getDBHash(userid);
            string dbSalt = getDBSalt(userid);
            int attempt = getAttempt(userid);
            DateTime lockoutTime = checkLockout(userid) ?? DateTime.Now;

            if (ValidateCaptcha())
            {
                try
                {
                    if (dbSalt != null && dbSalt.Length > 0 && dbHash != null && dbHash.Length > 0)
                    {
                        string pwdWithSalt = pwd + dbSalt;
                        byte[] hashWithSalt = hashing.ComputeHash(Encoding.UTF8.GetBytes(pwdWithSalt));
                        string userHash = Convert.ToBase64String(hashWithSalt);

                        if (userHash.Equals(dbHash))
                        {
                            Session["UserID"] = userid;

                            string guid = Guid.NewGuid().ToString();
                            Session["AuthToken"] = guid;

                            Response.Cookies.Add(new HttpCookie("AuthToken", guid));

                            Response.Redirect("Homepage.aspx", false);
                        }

                        else if (attempt < 3)
                        {
                            try
                            {
                                using (SqlConnection con = new SqlConnection(MYDBConnectionString))
                                {
                                    using (SqlCommand updateAttempt = new SqlCommand("UPDATE Accounts SET attempt = @attempt WHERE Email=@USERID"))
                                    {
                                        using (SqlDataAdapter sda = new SqlDataAdapter())
                                        {
                                            updateAttempt.CommandType = CommandType.Text;
                                            updateAttempt.Parameters.AddWithValue("@USERID", userid);
                                            updateAttempt.Parameters.AddWithValue("@attempt", attempt + 1);

                                            updateAttempt.Connection = con;
                                            con.Open();
                                            updateAttempt.ExecuteNonQuery();
                                            con.Close();
                                        }
                                    }
                                }

                                errorMsg = "Userid or password is not valid. Please try again.";
                                lbl_error.Text = errorMsg;
                                lbl_error.ForeColor = Color.Red;
                                return;
                            }
                            catch (Exception ex)
                            {
                                throw new Exception(ex.ToString());
                            }
                        }
                        else if (attempt >= 3)
                        {
                            try
                            {
                                using (SqlConnection con = new SqlConnection(MYDBConnectionString))
                                {
                                    using (SqlCommand duration = new SqlCommand("UPDATE Accounts SET lockoutTime=@TIME WHERE Email=@USERID"))
                                    {
                                        using (SqlDataAdapter sda = new SqlDataAdapter())
                                        {
                                            duration.CommandType = CommandType.Text;
                                            duration.Parameters.AddWithValue("@USERID", userid);
                                            duration.Parameters.AddWithValue("@TIME", DateTime.Now);

                                            duration.Connection = con;
                                            con.Open();
                                            duration.ExecuteNonQuery();
                                            con.Close();
                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                throw new Exception(ex.ToString());
                            }

                            if (DateTime.Now > lockoutTime.AddMinutes(30))
                            {
                                try
                                {
                                    using (SqlConnection con = new SqlConnection(MYDBConnectionString))
                                    {
                                        using (SqlCommand clearAttempt = new SqlCommand("UPDATE Accounts SET attempt = @attempt WHERE Email=@USERID"))
                                        {
                                            using (SqlDataAdapter sda = new SqlDataAdapter())
                                            {
                                                clearAttempt.CommandType = CommandType.Text;
                                                clearAttempt.Parameters.AddWithValue("@USERID", userid);
                                                clearAttempt.Parameters.AddWithValue("@attempt", 0);

                                                clearAttempt.Connection = con;
                                                con.Open();
                                                clearAttempt.ExecuteNonQuery();
                                                con.Close();
                                            }
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    throw new Exception(ex.ToString());
                                }

                                try
                                {
                                    using (SqlConnection con = new SqlConnection(MYDBConnectionString))
                                    {
                                        using (SqlCommand clearDuration = new SqlCommand("UPDATE Accounts SET lockoutTime=@TIME WHERE Email=@USERID"))
                                        {
                                            using (SqlDataAdapter sda = new SqlDataAdapter())
                                            {
                                                clearDuration.CommandType = CommandType.Text;
                                                clearDuration.Parameters.AddWithValue("@USERID", userid);
                                                clearDuration.Parameters.AddWithValue("@TIME", DBNull.Value);

                                                clearDuration.Connection = con;
                                                con.Open();
                                                clearDuration.ExecuteNonQuery();
                                                con.Close();
                                            }
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    throw new Exception(ex.ToString());
                                }

                                password_tb.Attributes.Remove("readonly");
                            }
                            else
                            {
                                errorMsg = "Your account has been locked. Please try again in 30 minutes.";
                                lbl_error.Text = errorMsg;
                                password_tb.Attributes.Add("readonly", "readonly");
                                return;
                            }  
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.ToString());
                }
                finally { }
            }
            else
            {
                lbl_error.Text = "Validate captcha to prove that you are a human.";
            }
        }

        protected DateTime? checkLockout(string userid)
        {
            DateTime? h = null;

            SqlConnection connection = new SqlConnection(MYDBConnectionString);
            string sql = "select lockoutTime FROM Accounts WHERE Email=@USERID";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@USERID", userid);

            try
            {
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["lockoutTime"] != null)
                        {
                            if (reader["lockoutTime"] != DBNull.Value)
                            {
                                h = DateTime.Parse(reader["lockoutTime"].ToString());
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
        protected int getAttempt(string userid)
        {
            int h = 0;

            SqlConnection connection = new SqlConnection(MYDBConnectionString);
            string sql = "select attempt FROM Accounts WHERE Email=@USERID";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@USERID", userid);

            try
            {
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["attempt"] != null)
                        {
                            if (reader["attempt"] != DBNull.Value)
                            {
                                h = int.Parse(reader["attempt"].ToString());
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

        protected void registerBtn_Click(object sender, EventArgs e)
        {
            Response.Redirect("Registration.aspx", false);
        }
    }


}