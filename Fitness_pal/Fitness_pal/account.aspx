<%@ Page Title="Account" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="account.aspx.cs" Inherits="Fitness_pal.account" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="list-group account">
        <div class="block" id="err" runat="server"></div>
        <div class="block card ac-usr-det">
                <div class="iblock l usr-icon"><span class="glyphicon glyphicon-user tblue icon-big"></span></div>
                <div class="iblock l usr-info">
                    <span class="block big bold" id="httpunmaeinfo" runat="server"></span>
                    <span class="block" id="httpusreg" runat="server"></span>
                    <span class="block" id="httpusrgoal" runat="server"></span>
                </div>
        </div>
        <div class="block card card-pad">
            <h2 class="tpad">User Details</h2>
            <div class="list-group-item block"><div class="l iblock"><b>Username</b></div><div class="r iblock" id="httpuname" runat="server"></div></div>
            <div class="list-group-item block"><div class="l iblock"><b>Email</b></div><div class="r iblock" id="httpemail" runat="server"></div></div>
            <div class="list-group-item block"><div class="l iblock"><b>Goal</b></div><div class="r iblock" id="httpgoal" runat="server"></div></div>
            <div class="list-group-item block"><div class="l iblock"><b>Daily Calorie Need</b></div><div class="r iblock" id="httpcalneed" runat="server"></div></div>
        </div>
        <form method="post">
        <div class="list-group-item card card-pad">
            <h2 class="tpad">Change Password</h2>
            <label>New Password</label>
            <input type="password" name="pass" id="jqpass" value="" class="form-control form-c" required />
            <label>Confirm Password</label>
            <input type="password" name="rpass" id="jqrpass" value="" class="form-control form-c" required />
            <span class="tred" id="notice"></span>
            <input type="submit" name="cpass" value="CHANGE" id="jqsubmit" class="btn btn-c btn-block" disabled />
       </div>
      </form>
    </div>

    <script>
        $("#jqrpass, #jqpass").keyup(function () {
            var pass = $("#jqpass").val();
            var rpass = $("#jqrpass").val();
            if (pass != rpass) {
                $("#notice").html("Passwords Do Not Match");
                $("#jqsubmit").prop("disabled", true);
            }
            else {
                $("#notice").html("");
                $("#jqsubmit").prop("disabled", false);
            }
        });
    </script>

</asp:Content>
