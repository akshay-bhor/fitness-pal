<%@ Page Title="Fitness Goal" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="goal.aspx.cs" Inherits="Fitness_pal.goal" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">


    <div class="list-group goal">
        <div class="block" id="err" runat="server"></div>
        <div class="block" id="ginfo" runat="server"></div>
        <form method="post">
        <div class="list-group-item card card-pad">
            <h2 class="tpad">Personal Info</h2>
            <label>Current Weight (kg)</label>
            <input runat="server" type="number" name="wt" id="htmlwt" class="form-control form-c" placeholder="Kilograms" required />
            <label>Current Height (cm)</label>
            <input runat="server" type="number" name="ht" id="htmlht" class="form-control form-c" placeholder="Centimeters" required />
            <label>Age</label>
            <input runat="server" type="number" name="age" id="htmlage" class="form-control form-c" placeholder="Years" required />
            <label>Gender</label>
            <select runat="server" name="gender" id="htmlgender" class="form-control form-c" required>
                <option value="">Select</option>
                <option value="1">Male</option>
                <option value="2">Female</option>
            </select>
        </div>
        <div class="list-group-item card card-pad">
            <label>Activity Level</label>
            <select runat="server" name="actlvl" id="htmlactlvl" class="form-control form-c" required>
                <option value="">Select</option>
                <option value="1">Very Light - Typical office job (sitting, studying, little walking throughout the day)</option>
                <option value="2">Light - Any job where you mostly stand or walk (teaching, shop/lab work, some walking throughout the day)</option>
                <option value="3">Moderate - Jobs requring physical activity (landscaping, cleaning, maintenance, jogging/biking/working out 2 hours/day)</option>
                <option value="4">Heavy - Heavy manual labor (construction, dancer, athlete, hard physical activity min 4 hours/day)</option>
                <option value="5">Very Heavy - Moderate to hard physical activity min 8 hours/day</option>
            </select>
            <label>Goal</label>
            <select runat="server" name="goal" id="htmlgoal" class="form-control form-c" required>
                <option value="">Select</option>
                <option value="1">Gain Weight</option>
                <option value="2">Keep Weight</option>
                <option value="3">Lose Weight</option>
            </select>
            <input type="submit" runat="server" name="save" id="sbtn" class="btn btn-c btn-block" />
        </div>
        </form>
    </div>


</asp:Content>
