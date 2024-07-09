<%@ Page Title="Progress" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="progress.aspx.cs" Inherits="Fitness_pal.progress" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <!-- GOOGLE CHARTS SCRIPT -->
    <script type="text/javascript" src="https://www.gstatic.com/charts/loader.js"></script>

    <div class="block account">
        <div class="block" id="err" runat="server"></div>
        <h2 class="list-group-item active big">Your Progress</h2>
        <div class="list-group-item" id="chart_div">
        </div>
        <h2 class="list-group-item active big tmar-big">Entries</h2>
        <div class="block" id="wt_entries" runat="server">
        </div>
    </div>



</asp:Content>
