<%@ Page Title="Add Food" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="add-food.aspx.cs" Inherits="Fitness_pal.admin.add_food" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">


    <div class="block account">
        <div class="block tmar" id="err" runat="server"></div>
        <h2 class="list-group-item active big">Add Food</h2>
        <div class="block tmar">
            <label>Food Name:</label>
            <input type="text" name="fname" class="form-control form-c" required />
            <label>Water (g):</label>
            <input type="number" name="water" class="form-control form-c" required />
            <label>Energy (Kcal):</label>
            <input type="number" name="energy" class="form-control form-c" required />
            <label>Protein (g):</label>
            <input type="number" name="protein" class="form-control form-c" required />
            <label>Lipid (g):</label>
            <input type="number" name="lipid" class="form-control form-c" required />
            <label>Ash (g):</label>
            <input type="number" name="ash" class="form-control form-c" required />
            <label>Carbohydrates (g):</label>
            <input type="number" name="carbs" class="form-control form-c" required />
            <label>Fiber (g):</label>
            <input type="number" name="fiber" class="form-control form-c" required />
            <label>Sugar (g):</label>
            <input type="number" name="sugar" class="form-control form-c" required />
            <label>Calcium (mg):</label>
            <input type="number" name="calcium" class="form-control form-c" required />
            <label>Iron (mg):</label>
            <input type="number" name="iron" class="form-control form-c" required />
            <label>Magnesium (mg):</label>
            <input type="number" name="magnesium" class="form-control form-c" required />
            <label>Phosphorus (mg):</label>
            <input type="number" name="phosphorus" class="form-control form-c" required />
            <label>Potassium (mg):</label>
            <input type="number" name="potassium" class="form-control form-c" required />
            <label>Sodium (mg):</label>
            <input type="number" name="sodium" class="form-control form-c" required />
            <label>Zinc (mg):</label>
            <input type="number" name="zinc" class="form-control form-c" required />
            <label>Copper (mg):</label>
            <input type="number" name="copper" class="form-control form-c" required />
            <label>Manganese (mg):</label>
            <input type="number" name="manganese" class="form-control form-c" required />
            <label>Vitamin C (mg):</label>
            <input type="number" name="vitc" class="form-control form-c" required />
            <label>Vitamin B6 (mg):</label>
            <input type="number" name="vitb6" class="form-control form-c" required />
            <label>Vitamin E (mg):</label>
            <input type="number" name="vite" class="form-control form-c" required />
            <label>Serving Unit:</label>
            <input type="text" name="unit" class="form-control form-c" required />
            <input type="submit" name="submit" value="Submit" class="btn btn-c btn-block" />
        </div>
    </div>


</asp:Content>
