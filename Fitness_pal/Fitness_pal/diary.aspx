<%@ Page Title="Diary" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="diary.aspx.cs" Inherits="Fitness_pal.diary" %>
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
        <div class="block" id="info" runat="server"></div>
        <div class="block cal-info card">
            <div class="grid-2 l text-center"><span class="block" id="cal_info_goal" runat="server"></span><span class="subtext">Goal</span></div>
            <div class="grid-2 l text-center"><span class="big">-</span></div>
            <div class="grid-2 l text-center"><span class="block" id="cal_info_food" runat="server"></span><span class="subtext">Food</span></div>
            <div class="grid-2 l text-center"><span class="big">=</span></div>
            <div class="grid-2 l text-center"><span class="block" id="cal_info_rem" runat="server"></span><span class="subtext">Remaining</span></div>
        </div>
        <div class="block card tmar">
            <div class="ftype-title block bold"><span class="big l">Breakfast</span><span class="big r" id="btcal" runat="server"></span></div>
            <div class="ftype-content block" id="bfcontent" runat="server"></div>
            <div class="ftype-bottom block" id="abmodal" type="1"><span class="big">+</span> Add Food</div>
        </div>
        <div class="block card tmar">
            <div class="ftype-title block bold"><span class="big l">Lunch</span><span class="big r" id="ltcal" runat="server"></span></div>
            <div class="ftype-content block" id="lfcontent" runat="server"></div>
            <div class="ftype-bottom block" id="almodal" type="2"><span class="big">+</span> Add Food</div>
        </div>
        <div class="block card tmar">
            <div class="ftype-title block bold"><span class="big l">Dinner</span><span class="big r" id="dtcal" runat="server"></span></div>
            <div class="ftype-content block" id="dfcontent" runat="server"></div>
            <div class="ftype-bottom block" id="admodal" type="3"><span class="big">+</span> Add Food</div>
        </div>
        <div class="block card tmar">
            <div class="ftype-title block bold"><span class="big l">Snacks</span><span class="big r" id="stcal" runat="server"></span></div>
            <div class="ftype-content block" id="sfcontent" runat="server"></div>
            <div class="ftype-bottom block" id="asmodal" type="4"><span class="big">+</span> Add Food</div>
        </div>
    </div>
        

    <!-- Modal -->
  <div class="modal fade" id="addfood" role="dialog">
    <div class="modal-dialog modal-lg">
      <div class="modal-content card">
        <div class="modal-header">
          <button type="button" class="close" data-dismiss="modal">&times;</button>
          <h4 class="modal-title big bold" id="modal-title"></h4>
        </div>
        <div class="modal-body block">
            <div class="block">
                <div class="grid-8 l"><input type="text" name="food_query" id="fsearch_query" class="form-control form-c" /></div>
                <div class="grid-2 l"><div class="btn btn-c btn-block" style="margin-top:0!important" id="fsearch_btn" type=""><span class="glyphicon glyphicon-search"></span></div></div>
            </div>
            <div class="block fresult-holder" id="fresult_holder"></div>
        </div>
      </div>
    </div>
  </div>


    <script>
        $(document).ready(function () {

            var date = $("#jqdate").html();
            $("#date_chosen").val(date);

            $("#date_chosen").change(function () {
                var date = $("#date_chosen").val();
                var form = $('<form action="/diary" method="POST" class="hidden">' +
                    '<input type="date" name="date_chosen" value="' + date + '">' +
                    '</form>');
                $(document.body).append(form);
                form.submit();
            });

            $("#abmodal, #almodal, #admodal, #asmodal").click(function () {
                var type = $(this).attr("type");
                if (type == 1) {
                    $("#modal-title").html("Add Breakfast");
                    $("#fsearch_btn").attr("type", type);
                }
                if (type == 2) {
                    $("#modal-title").html("Add Lunch");
                    $("#fsearch_btn").attr("type", type);
                }
                if (type == 3) {
                    $("#modal-title").html("Add Dinner");
                    $("#fsearch_btn").attr("type", type);
                }
                if (type == 4) {
                    $("#modal-title").html("Add Snacks");
                    $("#fsearch_btn").attr("type", type);
                }
                $("#fresult_holder").html("");
                $("#addfood").modal('toggle');
            });

            $("#fsearch_btn").click(function () {
                $("#fresult_holder").html("Loading...");
                var search_param = $("#fsearch_query").val();
                var type = $("#fsearch_btn").attr("type");
                $.ajax({
                    url: "/food_search?food=" + search_param + "&type=" + type,
                    type: "get",
                }).done(function(data) {
                    $("#fresult_holder").html(data);
                });
            });

        });

        function addfood(id, type) {
            var servings = $("#" + id + "").val();
            var date = $("#jqdate").html();
            $.ajax({
                url: "add_food?id=" + id + "&type=" + type + "&servings=" + servings + "&date=" + date,
                type: "get",
            }).done(function (data) {
                $("#fresult_holder").html(data);
                var form = $('<form action="/diary" method="POST" class="hidden">' +
                    '<input type="date" name="date_chosen" value="' + date + '">' +
                    '</form>');
                $(document.body).append(form);
                setTimeout(function () { form.submit(); }, 1000);
            });
        }

        function delfood(id, type) {
            var date = $("#jqdate").html();
            $.ajax({
                url: "del_food?id=" + id + "&type=" + type + "&date=" + date,
                type: "get",
            }).done(function (data) {
                $("#err").html(data);
                var form = $('<form action="/diary" method="POST" class="hidden">' +
                    '<input type="date" name="date_chosen" value="' + date + '">' +
                    '</form>');
                $(document.body).append(form);
                setTimeout(function () { form.submit(); }, 1000);
            });
        }
    </script>

</asp:Content>
