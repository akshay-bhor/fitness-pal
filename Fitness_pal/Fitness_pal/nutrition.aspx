<%@ Page Title=" Nutrition" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="nutrition.aspx.cs" Inherits="Fitness_pal.nutrition" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">


    <div class="block account">
        <div class="block" id="err" runat="server"></div>
        <div class="datepicker block">
            <div class="grid-2 iblock l"><input type="submit" name="prv" class="btn btn-c btn-block" id="prev_day" value="Prev" /></div>
            <div class="grid-6 iblock l"><input type="date" name="date_chosen" id="date_chosen" class="form-control date-ip-big" /></div>
            <div class="grid-2 iblock l"><input type="submit" name="nxt" class="btn btn-c btn-block" id="next_day" value="Next" /></div>
        </div>
        <div class="block diary-content">
            <h2 class="list-group-item active big" id="day_head" runat="server"></h2>
        </div>
        <div class="block nutri-info">
            <div class="block tpad"><div class="grid-4 bold big l">Nutrients</div><div class="grid-2 bold big l txt-right">Total</div><div class="grid-2 bold big l txt-right">Goal</div><div class="grid-2 bold big l txt-right">Left</div></div>
            <div class="block" id="nutri_bar" runat="server">
                
            </div>
        </div>
    </div>

    <script>
        $(document).ready(function () {

            var date = $("#jqdate").html();
            $("#date_chosen").val(date);

            $("#date_chosen").change(function () {
                var date = $("#date_chosen").val();
                var form = $('<form action="/nutrition" method="POST" class="hidden">' +
                    '<input type="date" name="date_chosen" value="' + date + '">' +
                    '</form>');
                $(document.body).append(form);
                form.submit();
            });
        });
    </script>

</asp:Content>
