<%@ Page Title="Search Food Contents | Search Foos Nutrition" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="food.aspx.cs" Inherits="Fitness_pal.food" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="block account">
        <div class="block" id="err" runat="server"></div>
        <div class="search-holder block">
            <input type="text" name="food_name" value="" class="form-control fsearch-big l" placeholder="Search Food..." />
            <button type="submit" name="Search" class="fsearch-btn tblue l"><span class="glyphicon glyphicon-search"></span></button>
        </div>

        <div class="fresult-holder" id="fresult_holder" runat="server">
                
        </div>

    </div>

</asp:Content>
