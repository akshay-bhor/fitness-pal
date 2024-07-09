<%@ Page Title="Users" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="users.aspx.cs" Inherits="Fitness_pal.admin.users" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">


    <div class="jumbotron">
        <div class="container">
            <div class="block tmar" id="err" runat="server"></div>
            <h2 class="list-group-item active big tmar-big">Users</h2>
            <div class="block tmar">
                <div class="grid-6 l"><input type="text" name="uname" class="form-control form-c" placeholder="Username..." id="huname" runat="server" /></div>
                <div class="grid-4 l"><input type="submit" name="submit" value="Search" class="btn btn-c btn-block" style="margin-top:0!important" /></div>
            </div>
            <div class="table-responsive">
                <table class="table">
                    <thead>
                        <tr>
                            <th>Username</th>
                            <th>Email</th>
                            <th>Status</th>
                            <th>Rank</th>
                            <th>Modify</th>
                        </tr>
                    </thead>
                    <tbody id="tbody" runat="server">
                    </tbody>
                </table>
            </div>
        </div>
    </div>


</asp:Content>
