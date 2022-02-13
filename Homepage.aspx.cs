using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Assignment
{
    public partial class Homepage : System.Web.UI.Page
    {
        string MYDBConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["UserDB"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {

            if (Session["UserID"] != null && Session["AuthToken"] != null && Request.Cookies["AuthToken"] != null)
            {
                if (!Session["AuthToken"].ToString().Equals(Request.Cookies["AuthToken"].Value))
                {
                    Response.Redirect("Login.aspx", false);
                    
                }
                else
                {
                    var userid = Session["UserID"].ToString();
                    DateTime pwChangeTime = checkChangeTime(userid) ?? DateTime.Now;


                    lblMessage.Text = "Congratulations !, you are logged in.";
                    lblMessage.ForeColor = System.Drawing.Color.Green;
                    btnLogout.Visible = true;

                    if (DateTime.Now > pwChangeTime.AddMinutes(10))
                    {
                        lbl_pwchange.Text = "Please change your password";
                        lbl_pwchange.ForeColor = System.Drawing.Color.Red;
                    }
                }
            }
            else
            {
                //Response.Redirect("Login.aspx", false);
                Response.Redirect("~/CustomError/HTTP403.html");
            }
        }

        protected void LogoutMe(object sender, EventArgs e)
        {
            Session.Clear();
            Session.Abandon();
            Session.RemoveAll();

            Response.Redirect("Login.aspx", false);

            if (Request.Cookies["ASP.NET_SessionId"] != null)
            {
                Response.Cookies["ASP.NET_SessionId"].Value = string.Empty;
                Response.Cookies["ASP.NET_SessionId"].Expires = DateTime.Now.AddMonths(-20);
            }
            if (Request.Cookies["AuthToken"] != null)
            {
                Response.Cookies["AuthToken"].Value = string.Empty;
                Response.Cookies["AuthToken"].Expires = DateTime.Now.AddMonths(-20);
            }

        }

        protected void pwChange_Click(object sender, EventArgs e)
        {
            Response.Redirect("ChangePassword.aspx", false);
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
    }
}