<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Homepage.aspx.cs" Inherits="Assignment.Homepage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .auto-style1 {
            width: 50%;
        }
        .auto-style2 {
            width: 295px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div>

            <fieldset>
                <legend>Homepage</legend>
                <br />

                <asp:Label ID="lblMessage" runat="server" EnableViewState="false" />
                <br />
                <asp:Label ID="lbl_pwchange" runat="server"></asp:Label>
                <br />
                <br />
                <table class="auto-style1">
                    <tr>
                        <td class="auto-style2">

                <asp:Button ID="btnLogout" runat="server" Text="Logout" OnClick="LogoutMe" Visible="false" Width="310px" />
               
                        </td>
                        <td>
                            <asp:Button ID="pwChange" runat="server" OnClick="pwChange_Click" Text="Change Password" Width="310px" />
                        </td>
                    </tr>
                </table>
                <br />
                <br />

            </fieldset>    
        </div>
    </form>
</body>
</html>
