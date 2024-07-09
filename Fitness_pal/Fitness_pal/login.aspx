<%@ Page Title="Login" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="login.aspx.cs" Inherits="Fitness_pal.login" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">


    <div class="list-group signup">
        <div class="block" id="err" runat="server"></div>
        <form method="post">
        <div class="list-group-item card card_pad">
            <h2 class="tpad">Login</h2>
            <label>Username</label>
            <input type="text" name="uname" value="" class="form-control form-c" required />
            <label>Password</label>
            <input type="password" name="pass" value="" class="form-control form-c" required />
            <input type="submit" name="login" value="Login" class="btn btn-c btn-block" />
            <span class="tpad tblue">Don't have an account? <a href="/register">Register</a> now.</span>
        </form>
       </div>
    </div>


</asp:Content>
