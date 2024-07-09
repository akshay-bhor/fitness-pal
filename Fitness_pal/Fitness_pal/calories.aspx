<%@ Page Title="Calorie Chart | Nutrition Facts" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="calories.aspx.cs" Inherits="Fitness_pal.calories" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">


    <div class="block account">
        <div class="block" id="err" runat="server"></div>
        <div class="search-holder block">
            <input type="text" name="food_name" value="" class="form-control fsearch-big l" placeholder="Search Food..." />
            <button type="submit" name="Search" class="fsearch-btn tblue l"><span class="glyphicon glyphicon-search"></span></button>
        </div>

        <div class="fresult-holder">
            <h1 class="h1" id="food_heading" runat="server"></h1>
            <span class="subtext" id="ser_size" runat="server"></span>
            <div class="fact-chart block" id="fact_chart" runat="server"></div>
            <div class="goal-holder">
                <h3>Daily Goals</h3>
                <span class="subtext">How does this food fit into your daily goals?</span>
                <div class="fresult-row block">
                    <div class="block tpad"><span class="big iblock l"><b>Calorie Goal</b></span><span class="big iblock r"><b id="rem_cal" runat="server"></b></span></div>
                        <div class="progress" id="pbar" runat="server">
                            
                        </div>
                    <div class="block"><span class="iblock small l" id="comp_cal" runat="server"></span><span class="iblock small r">left</span></div>
               </div>
           </div>
        </div>

        <div class="block nutri-info">
            <h3 class="h3">Nutritional Info</h3>
            <div class="block" id="nutri_info" runat="server">
                
            </div>
        </div>

    </div>


</asp:Content>
