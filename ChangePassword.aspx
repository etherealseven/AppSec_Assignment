<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ChangePassword.aspx.cs" Inherits="Assignment.ChangePassword" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .auto-style1 {
            width: 70%;
        }
        .auto-style3 {
            width: 290px;
        }
        .auto-style4 {
            width: 275px;
        }
    </style>

    <script>
        function validate_newpassword() {
            var str = document.getElementById('<%=newpw_tb.ClientID %>').value;

            if (str.length == 0) {
                document.getElementById("lbl_newpw").innerHTML = "Required";
                document.getElementById("lbl_newpw").style.color = "Red";
            }

            if (str.length < 12) {
                document.getElementById("lbl_newpw").innerHTML = "Password Length must be at least 12 characters";
                document.getElementById("lbl_newpw").style.color = "Red";
            }

            else if (str.search(/[0-9]/) == -1) {
                document.getElementById("lbl_newpw").innerHTML = "Password requires at least 1 number";
                document.getElementById("lbl_newpw").style.color = "Red";
            }

            else if (str.search(/[A-Z]/) == -1) {
                document.getElementById("lbl_newpw").innerHTML = "Password requires at least 1 uppercase character";
                document.getElementById("lbl_newpw").style.color = "Red";
            }

            else if (str.search(/[a-z]/) == -1) {
                document.getElementById("lbl_newpw").innerHTML = "Password requires at least 1 lowercase character";
                document.getElementById("lbl_newpw").style.color = "Red";
            }

            else if (str.search(/[^A-Za-z0-9]/) == -1) {
                document.getElementById("lbl_newpw").innerHTML = "Password requires at least 1 special character";
                document.getElementById("lbl_newpw").style.color = "Red";
            }

            else {
                document.getElementById("lbl_newpw").innerHTML = "Excellent!";
                document.getElementById("lbl_newpw").style.color = "Green";
            }
        }

        function validate_confirmpassword() {
            var str = document.getElementById('<%=confirmpw_tb.ClientID %>').value;
            var str2 = document.getElementById('<%=newpw_tb.ClientID %>').value;

            if (str.length == 0) {
                document.getElementById("lbl_confirmpw").innerHTML = "Required";
                document.getElementById("lbl_confirmpw").style.color = "Red";
            }
            if (str != str2) {
                document.getElementById("lbl_confirmpw").innerHTML = "Passwords do not match!";
                document.getElementById("lbl_confirmpw").style.color = "Red";
            }
            else {
                document.getElementById("lbl_confirmpw").innerHTML = "Match!";
                document.getElementById("lbl_confirmpw").style.color = "Green";
            }
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            Change Password<br />
            <br />
            <asp:Label ID="lbl_error" runat="server"></asp:Label>
            <br />
            <table class="auto-style1">
                <tr>
                    <td class="auto-style4">Old Password</td>
                    <td class="auto-style3">
                        <asp:TextBox ID="oldpw_tb" runat="server" Width="320px"></asp:TextBox>
                    </td>
                    <td>
                        <asp:Label ID="lbl_oldpw" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style4">New Password</td>
                    <td class="auto-style3">
                        <asp:TextBox ID="newpw_tb" runat="server" Width="320px" onkeyup="javascript:validate_newpassword()"></asp:TextBox>
                    </td>
                    <td>
                        <asp:Label ID="lbl_newpw" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style4">Confirm New Password</td>
                    <td class="auto-style3">
                        <asp:TextBox ID="confirmpw_tb" runat="server" Width="320px" onkeyup="javascript:validate_confirmpassword()"></asp:TextBox>
                    </td>
                    <td>
                        <asp:Label ID="lbl_confirmpw" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style4">&nbsp;</td>
                    <td class="auto-style3">&nbsp;</td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td class="auto-style4">&nbsp;</td>
                    <td class="auto-style3">
                        <asp:Button ID="changeBtn" runat="server" OnClick="changeBtn_Click" Text="Change" Width="326px" />
                    </td>
                    <td>&nbsp;</td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
