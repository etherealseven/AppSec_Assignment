<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Registration.aspx.cs" Inherits="Assignment.Registration" ValidateRequest="false"%>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .auto-style1 {
            width: 70%;
        }
        .auto-style2 {
            width: 319px;
        }
        .auto-style3 {
            width: 319px;
        }
    </style>

    <script>
        function validate_fname() {
            var str = document.getElementById('<%=fname_tb.ClientID %>').value;

            if (str.length == 0) {
                document.getElementById("lbl_fname").innerHTML = "Required";
                document.getElementById("lbl_fname").style.color = "Red";
            }
            else {
                document.getElementById("lbl_fname").innerHTML = "Valid!";
                document.getElementById("lbl_fname").style.color = "Green";
            }
        }

        function validate_lname() {
            var str = document.getElementById('<%=lname_tb.ClientID %>').value;

            if (str.length == 0) {
                document.getElementById("lbl_lname").innerHTML = "Required";
                document.getElementById("lbl_lname").style.color = "Red";
            }

            else {
                document.getElementById("lbl_lname").innerHTML = "Valid!";
                document.getElementById("lbl_lname").style.color = "Green";
            }
        }

        function validate_cc() {
            var str = document.getElementById('<%=creditcard_tb.ClientID %>').value;

            if (str.length == 0) {
                document.getElementById("lbl_cc").innerHTML = "Required";
                document.getElementById("lbl_cc").style.color = "Red";
            }

            if (str.length < 16 || str.length > 16) {
                document.getElementById("lbl_cc").innerHTML = "Invalid credit card number!";
                document.getElementById("lbl_cc").style.color = "Red";
            }

            else {
                document.getElementById("lbl_cc").innerHTML = "Valid!";
                document.getElementById("lbl_cc").style.color = "Green";
            }
        }    

        function validate_email() {
            var str = document.getElementById('<%=email_tb.ClientID %>').value;
            var regex = /^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$/;

            if (str.length == 0) {
                document.getElementById("lbl_email").innerHTML = "Required";
                document.getElementById("lbl_email").style.color = "Red";
            }

            if (regex.test(str)) {
                document.getElementById("lbl_email").innerHTML = "Valid!";
                document.getElementById("lbl_email").style.color = "Green";
            }
            else {
                document.getElementById("lbl_email").innerHTML = "Invalid Email Address";
                document.getElementById("lbl_email").style.color = "Red";
            }
        }

        function validate_password() {
            var str = document.getElementById('<%=password_tb.ClientID %>').value;

            if (str.length == 0) {
                document.getElementById("lbl_pwdchecker").innerHTML = "Required";
                document.getElementById("lbl_pwdchecker").style.color = "Red";
            }

            if (str.length < 12) {
                document.getElementById("lbl_pwdchecker").innerHTML = "Password Length must be at least 12 characters";
                document.getElementById("lbl_pwdchecker").style.color = "Red";
            }

            else if (str.search(/[0-9]/) == -1) {
                document.getElementById("lbl_pwdchecker").innerHTML = "Password requires at least 1 number";
                document.getElementById("lbl_pwdchecker").style.color = "Red";
            }

            else if (str.search(/[A-Z]/) == -1) {
                document.getElementById("lbl_pwdchecker").innerHTML = "Password requires at least 1 uppercase character";
                document.getElementById("lbl_pwdchecker").style.color = "Red";
            }

            else if (str.search(/[a-z]/) == -1) {
                document.getElementById("lbl_pwdchecker").innerHTML = "Password requires at least 1 lowercase character";
                document.getElementById("lbl_pwdchecker").style.color = "Red";
            }

            else if (str.search(/[^A-Za-z0-9]/) == -1) {
                document.getElementById("lbl_pwdchecker").innerHTML = "Password requires at least 1 special character";
                document.getElementById("lbl_pwdchecker").style.color = "Red";
            }

            else {
                document.getElementById("lbl_pwdchecker").innerHTML = "Excellent!";
                document.getElementById("lbl_pwdchecker").style.color = "Green";
            }
        }

        function validate_dob() {
            var str = document.getElementById('<%=dob_tb.ClientID %>').value;

            if (str.length == 0) {
                document.getElementById("lbl_dob").innerHTML = "Required";
                document.getElementById("lbl_dob").style.color = "Red";
            }
            else {
                document.getElementById("lbl_dob").innerHTML = "Valid!";
                document.getElementById("lbl_dob").style.color = "Green";
            }   
        }

        function validate_image() {
            var img = document.getElementById('<%=FileUpload1.ClientID%>').value;

            if (img.length == 0) {
                document.getElementById("lbl_image").innerHTML = "Required";
                document.getElementById("lbl_image").style.color = "Red";
            }
            else {
                document.getElementById("lbl_image").innerHTML = "Valid!";
                document.getElementById("lbl_image").style.color = "Green";
            }
        }

    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            Account Registration<br />
            <br />
            <table class="auto-style1">
                <tr>
                    <td class="auto-style2">First Name</td>
                    <td class="auto-style3">
                        <asp:TextBox ID="fname_tb" runat="server" Width="315px"  onkeyup="javascript:validate_fname()"></asp:TextBox>
                    </td>
                    <td>
                        <asp:Label ID="lbl_fname" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style2">Last Name</td>
                    <td class="auto-style3">
                        <asp:TextBox ID="lname_tb" runat="server" Width="315px"  onkeyup="javascript:validate_lname()"></asp:TextBox>
                    </td>
                    <td>
                        <asp:Label ID="lbl_lname" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style2">Credit Card Info</td>
                    <td class="auto-style3">
                        <asp:TextBox ID="creditcard_tb" runat="server" Width="315px" TextMode="Number" onkeyup="javascript:validate_cc()"></asp:TextBox>
                    </td>
                    <td>
                        <asp:Label ID="lbl_cc" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style2">Email Address</td>
                    <td class="auto-style3">
                        <asp:TextBox ID="email_tb" runat="server" Width="315px" onkeyup="javascript:validate_email()"></asp:TextBox>
                    </td>
                    <td>
                        <asp:Label ID="lbl_email" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style2">Password</td>
                    <td class="auto-style3">
                        <asp:TextBox ID="password_tb" runat="server" Width="315px" onkeyup="javascript:validate_password()" TextMode="Password"></asp:TextBox>
                    </td>
                    <td>
                        <asp:Label ID="lbl_pwdchecker" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style2">Date of Birth</td>
                    <td class="auto-style3">
                        <asp:TextBox ID="dob_tb" runat="server" Width="315px" onkeyup="javascript:validate_dob()"></asp:TextBox>
                    </td>
                    <td>
                        <asp:Label ID="lbl_dob" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style2">Photo</td>
                    <td class="auto-style3">
                        <asp:FileUpload ID="FileUpload1" runat="server" Width="322px" onkeyup="javascript:validate_image()"/>
                    </td>
                    <td>
                        <asp:Label ID="lbl_image" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style2">&nbsp;</td>
                    <td class="auto-style3">
                        &nbsp;</td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td class="auto-style2">&nbsp;</td>
                    <td class="auto-style3">
                        <asp:Button ID="submitBtn" runat="server" Text="Register" Width="323px" OnClick="Button1_Click" />
                    </td>
                    <td>&nbsp;</td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
