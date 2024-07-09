<%@ Page Title="Register" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="register.aspx.cs" Inherits="Fitness_pal.register" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

  
    <div class="list-group signup">
        <div class="block" id="err" runat="server"></div>
        <form method="post">
        <div class="list-group-item card card-pad">
            <h2 class="tpad">Register</h2>
            <label>Username</label>
            <input type="text" name="uname" value="" class="form-control form-c" required />
            <label>Email</label>
            <input type="email" name="email" value="" class="form-control form-c" required />
            <label>Password</label>
            <input type="password" name="pass" id="jqpass" value="" class="form-control form-c" required />
            <label>Confirm Password</label>
            <input type="password" name="rpass" id="jqrpass" value="" class="form-control form-c" required />
            <span class="tred" id="notice"></span>
            <input type="submit" name="register" value="REGISTER" id="jqsubmit" class="btn btn-c btn-block" disabled />
            <span class="tpad tblue">Already have an account? <a href="/login">Login</a> now.</span>
       </div>
      </form>
    </div>

    <script>
        $("#jqpass, #jqrpass").keyup(function () {
            var pass = $("#jqpass").val();
            var rpass = $("#jqrpass").val();
            if (pass == rpass) {
                $("#notice").html("");
                $("#jqsubmit").prop("disabled", false);
            }
            else {
                $("#notice").html("Passwords Don't Match");
                $("#jqsubmit").prop("disabled", true);
            }
        });
    </script>

</asp:Content>
