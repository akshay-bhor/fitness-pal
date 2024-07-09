<%@ Page Title="Fitness Pal | Fitness Tracker" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Fitness_pal._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="jumbotron home-main">
        <div class="block">
        <div class="col-md-6 home-main-l">
            <div class="home-main-text r">
                <h1 class="h1">Fitness starts with what you eat.</h1>
                <span class="home-main-subtext">
                    Set your goals, count your calorie intake, get nutrition info, log it into your diary with Fitness Pal.
                </span>
                <a href="/login" class="btn btn-c btn-lg btn-block">START FOR FREE</a>
            </div>
        </div>
        <div class="col-md-6 main-imgback"></div>
        </div>
    </div>

    <div class="block home-mid">
       <div class="home-mid-text block">
        <h2 class="h2">Search over 1 million foods in our database.</h2>
        <span class="home-main-subtext">
            What does you eat? Search for nutritional content, calorie count and many more.
        </span>
       </div>
       <div class="mid-form">
           <form method="post">
               <div class="col-md-12 block">
                <input type="text" name="food_name" placeholder="Get nutrition for any food, e.g. type banana" class="form-control fsearch-big l" />
                <button type="submit" name="food_search" class="fsearch-btn tblue l"><span class="glyphicon glyphicon-search"></span></button>
               </div>
           </form>
       </div>
    </div>
    <div class="block home-bottom">
        <div class="hbottom-text-up text-center">
        <h2 class="h2">Best Tools for Your Goals</h2>
        <span class="subtext tpad">Struggling to lose weight, tone up, gain weight, or keep your overall health? All the tools under one roof.</span>
        </div>
        <div class="block bot-cols">
            <div class="col-md-4">
                <div class="block text-center"><span class="glyphicon glyphicon-cutlery tblue icon-big"></span></div>
                <div class="block"><h3 class="h3 text-center">Learn</h3></div>
                <div class="subtext text-center">Learn abount food and its nutritional content, understand your habits, and plan your diat.</div>
            </div>
            <div class="col-md-4 mob-pad">
                <div class="block text-center"><span class="glyphicon glyphicon-tasks tblue icon-big"></span></div>
                <div class="block"><h3 class="h3 text-center">Track</h3></div>
                <div class="subtext text-center">Log your daily routine in your diary, keep track of your daily nutrients and calorie intake.</div>
            </div>
            <div class="col-md-4 mob-pad">
                <div class="block text-center"><span class="glyphicon glyphicon-scale tblue icon-big"></span></div>
                <div class="block"><h3 class="h3 text-center">Improve</h3></div>
                <div class="subtext text-center">Calculate nutrition intake, improve your plan, and take another step towards your goal.</div>
            </div>
        </div>
        <div class="block bot-cols text-center">
            <a href="/login" class="btn btn-c btn-lg">START YOUR JOURNEY TODAY</a>
        </div>
    </div>
    
</asp:Content>
