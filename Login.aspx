<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Assignment.Login" ValidateRequest="false" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .auto-style1 {
            width: 70%;
        }
        .auto-style2 {
            width: 316px;
        }
        .auto-style3 {
            width: 316px;
            height: 26px;
        }
        .auto-style4 {
            height: 26px;
        }
    </style>

    <script src="https://www.google.com/recaptcha/api.js?render=6LeCcmkeAAAAALuVlk5m5ssZzg-4PEo08SUEMbOK"></script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            Login Page<br />
            <br />
            <table class="auto-style1">
                <tr>
                    <td class="auto-style3" colspan="2">
                        <asp:Label ID="lbl_error" runat="server"></asp:Label>
                    </td>
                    <td class="auto-style3"></td>
                    <td class="auto-style4"></td>
                </tr>
                <tr>
                    <td class="auto-style2">Email</td>
                    <td class="auto-style2">
                        <asp:TextBox ID="email_tb" runat="server" Width="316px"></asp:TextBox>
                    </td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td class="auto-style2">Password</td>
                    <td class="auto-style2">
                        <asp:TextBox ID="password_tb" runat="server" Width="316px" TextMode="Password"></asp:TextBox>
                    </td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td class="auto-style2">&nbsp;</td>
                    <td class="auto-style2">
                        &nbsp;</td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td class="auto-style2">&nbsp;</td>
                    <td class="auto-style2">
                        <asp:Button ID="submitBtn" runat="server" OnClick="submitBtn_Click" Text="Log In" Width="323px" />
                    </td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td class="auto-style2">&nbsp;</td>
                    <td class="auto-style2">
                        &nbsp;</td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td class="auto-style2" colspan="2">Don&#39;t have an account? Register here! </td>
                    <td class="auto-style2">
                        &nbsp;</td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td class="auto-style2" colspan="2">
                        <asp:Button ID="registerBtn" runat="server" OnClick="registerBtn_Click" Text="Register" Width="299px" />
                    </td>
                    <td class="auto-style2">
                        &nbsp;</td>
                    <td>&nbsp;</td>
                </tr>
            </table>

            <input type="hidden" id="g-recaptcha-response" name="g-recaptcha-response"/>
        </div>
    </form>

    <script>
        grecaptcha.ready(function () {
            grecaptcha.execute('6LeCcmkeAAAAALuVlk5m5ssZzg-4PEo08SUEMbOK', { action: 'Login' }).then(function (token) {
            document.getElementById("g-recaptcha-response").value = token;
            });
        });
    </script>
</body>
</html>
