<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CategoryWiseMutualFund.aspx.cs" Inherits="iFrames.BloomKite.CategoryWiseMutualFund" %>

<!DOCTYPE html>
<html class="no-js">
<head>
     <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta name="viewport" content="width=device-width,initial-scale=1,maximum-scale=1,minimum-scale=1,user-scalable=no,shrink-to-fit=no">
    <meta charset="utf-8">

    <title>Category wise Mutual Fund Returns</title>

    <link href="css/bootstrap.css" rel="stylesheet" type="text/css" />
    <link href="css/IAL_style.css" rel="stylesheet" type="text/css" />

    <script src="https://cdnjs.cloudflare.com/ajax/libs/jquery/2.1.3/jquery.min.js"></script>
    <script type='text/javascript' src="js/bootstrap.js"></script>
    <script src="https://use.fontawesome.com/ea6a7e4db5.js"></script>

    <!-- HTML5 shim, for IE6-8 support of HTML5 elements -->
    <!--[if lt IE 9]>
            <script src="http://html5shim.googlecode.com/svn/trunk/html5.js"></script>
        <![endif]-->

    <style>
        a:hover, a:focus {
            color: #000000;
            /*text-decoration: underline;*/
}
        .nav > li > a{
            padding: 2px 5px;
        }
        .card{
            border:none;
        }
        .card-body {
            padding: 0 !important;
        }
        .card-header {
            padding: 10px !important;
        }
        .card-header h5{
            margin:0;
        }
        label {
            margin-bottom: 0.4rem;
            margin-top:0.5rem;
        }

        #rdbOption table tbody tr td {
            margin-right: 20px;
            padding-right: 20px;
        }

        #rdbOption_0 {
            margin-right: 5px;
            margin-top: 8px;
            font-weight: normal !important;
        }

        #rdbOption_1 {
            margin-right: 5px;
            margin-top: 8px;
            font-weight: normal !important;
        }

        .tooltip {
            font-size: 10px;
        }

        .wrapper {
            position: relative;
            margin: 0 auto;
            overflow: hidden;
            padding: 5px;
            height: 32px;
        }

        .list {
            position: absolute;
            left: 0px;
            top: 0px;
            min-width: 3000px;
            margin-left: 12px;
            margin-top: 0px;
        }

            .list > li {
                display: table-cell;
                position: relative;
                text-align: center;
                cursor: grab;
                cursor: -webkit-grab;
                color: #efefef;
                vertical-align: middle;
                font-weight: 600;
            }
             .list > li.active {
            border-bottom: 2px solid #f3565d;
            position: relative;
            cursor: pointer;
        }
        .scroller {
            text-align: center;
            cursor: pointer;
            display: none;
            padding: 7px;
            padding-top: 8px;
            white-space: no-wrap;
            vertical-align: middle;
            background-color: #fff;
        }

        .scroller-right {
            float: right;
        }

        .scroller-left {
            float: left;
        }
        .table th {
           font-size:12px
        }
        /*Custom Scroll*/
        /*.scrollbar {
            overflow-y: scroll;
             height: 350px;
        }

        .force-overflow {
            height: 350px;
        }

        #style-3::-webkit-scrollbar-track {
            -webkit-box-shadow: inset 0 0 6px rgba(0,0,0,0.3);
            background-color: #F5F5F5;
        }

        #style-3::-webkit-scrollbar {
            width: 6px;
            background-color: #F5F5F5;
        }

        #style-3::-webkit-scrollbar-thumb {
            background-color: #898989;
        }*/

        .top-tab > li > a {
            padding: 10px 20px!important;
            font-size:16px;
            font-weight:600;
            border-radius: 10px;
            color: #251534;
        }
         .top-tab > li.active > a, .top-tab > li.active > a:hover, .top-tab > li.active > a:focus {
            color:#fff ;  
            background-color: #251534;
            border: 1px solid #ddd;
            border-bottom-color: transparent;
            cursor:pointer;
        }
        .nav-tabs .top-tab > li.active > a, .nav-tabs .top-tab > li.active > a:hover, .nav-tabs .top-tab > li.active > a:focus{
          
        }
         .top-tab{
            background: #f0f0f0;
            border-bottom: 1px solid #ddd;
        }
        .nav > li > a:hover, .nav > li > a:focus {
            text-decoration: none;
            background-color: #d6d6d6;
}
        .nav .level-2{
            border: 0;
            margin-right: 0;
            color: #737373;
        }
        .level-2 li a{
            padding:10px 15px;
        }

    </style>
</head>
<body>
    <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12">
        <div class="card">
                <h2 style="text-align:center">Category wise Mutual Fund Returns (%)</h2>
            <div class="card-body">
                <div class="">
                    <div class="row">
                        <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12 pt-2">
                            <div class="tabbable boxed parentTabs">
                                <ul class="nav nav-tabs top-tab">
                                    <li class="active" onclick="FnFetchTopfundData('25','4')">
                                        <a href="#Equity">Equity</a>
                                    </li>
                                    <li onclick="FnFetchTopfundData('36','3')">
                                        <a href="#Debt">Debt</a>
                                    </li>
                                    <li onclick="FnFetchTopfundData('6','1')">
                                        <a href="#Hybrid">Hybrid</a>
                                    </li>
                                    <li onclick="FnFetchTopfundData('41','6')">
                                        <a href="#Solution_Oriented">Solution Oriented</a>
                                    </li>
                                    <li onclick="FnFetchTopfundData('48','5')">
                                        <a href="#Other">Other</a>
                                    </li>
                                    <p style="text-align:right;padding-top: 10px; padding-right: 5px; margin-bottom:0; font-size:12px">
                                        *On the basis of Last 1 year performance 
                                    </p>
                                </ul>
                                <div id="myTabContent" class="tab-content pt-2">
                                    <div class="tab-pane fade active in" id="Equity">
                                        <div class="tabbable-panel">
                                            <div>
                                                <div class="scroller scroller-left">
                                                    <i class="fa fa-chevron-left"></i>
                                                </div>
                                                <div class="scroller scroller-right">
                                                    <i class="fa fa-chevron-right"></i>
                                                </div>
                                                <div class="tabbable-line wrapper">
                                                    <ul class="nav nav-tabs list" id="myTabEquity">
                                                        <asp:Repeater ID="RepeaterEquity" runat="server">
                                                            <ItemTemplate>
                                                                <li onclick="FnFetchTopfundData('<%#Eval("Sebi_Sub_Nature_ID").ToString()%>','4')"
                                                                    role="presentation" <%# Container.ItemIndex == 0 ? " class='active'" : ""%>>
                                                                    <a href="#SN_<%#Eval("Sebi_Sub_Nature_ID")%>"><%#Eval("Sebi_Sub_Nature")%></a>

                                                                </li>
                                                            </ItemTemplate>
                                                        </asp:Repeater>
                                                    </ul>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="tab-pane fade " id="Debt">
                                        <div class="tabbable-panel">
                                            <div>
                                                <div class="scroller scroller-left">
                                                    <i class="fa fa-chevron-left"></i>
                                                </div>
                                                <div class="scroller scroller-right">
                                                    <i class="fa fa-chevron-right"></i>
                                                </div>
                                                <div class="tabbable-line wrapper">
                                                    <ul class="nav nav-tabs list" id="myTabDebt">
                                                        <asp:Repeater ID="RepeaterDebt" runat="server">
                                                            <ItemTemplate>
                                                                <li onclick="FnFetchTopfundData('<%#Eval("Sebi_Sub_Nature_ID").ToString()%>','3')"
                                                                    role="presentation" <%# Container.ItemIndex == 0 ? " class='active'" : ""%>>
                                                                    <a><%#Eval("Sebi_Sub_Nature")%></a></li>
                                                            </ItemTemplate>
                                                        </asp:Repeater>
                                                    </ul>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="tab-pane fade " id="Hybrid">
                                        <div class="tabbable-panel">
                                            <div>
                                                <div class="scroller scroller-left">
                                                    <i class="fa fa-chevron-left"></i>
                                                </div>
                                                <div class="scroller scroller-right">
                                                    <i class="fa fa-chevron-right"></i>
                                                </div>
                                                <div class="tabbable-line wrapper">
                                                    <ul class="nav nav-tabs list" id="myTabHybrid">
                                                        <asp:Repeater ID="RepeaterHybrid" runat="server">
                                                            <ItemTemplate>
                                                                <li onclick="FnFetchTopfundData('<%#Eval("Sebi_Sub_Nature_ID").ToString()%>','1')"
                                                                    role="presentation" <%# Container.ItemIndex == 0 ? " class='active'" : ""%>>
                                                                    <a><%#Eval("Sebi_Sub_Nature")%></a></li>
                                                            </ItemTemplate>
                                                        </asp:Repeater>
                                                    </ul>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="tab-pane fade " id="Solution_Oriented">
                                        <div class="tabbable-panel">
                                            <div>
                                                <%--<div class="scroller scroller-left"><i class="fa fa-chevron-left"></i></div>
                                                            <div class="scroller scroller-right"><i class="fa fa-chevron-right"></i></div>--%>
                                                <div class="tabbable-line wrapper">
                                                    <ul class="nav nav-tabs list" id="myTabSolution_Oriented">
                                                        <asp:Repeater ID="RepeaterSolution_Oriented" runat="server">
                                                            <ItemTemplate>
                                                                <li onclick="FnFetchTopfundData('<%#Eval("Sebi_Sub_Nature_ID").ToString()%>','6')"
                                                                    role="presentation" <%# Container.ItemIndex == 0 ? " class='active'" : ""%>>
                                                                    <a><%#Eval("Sebi_Sub_Nature")%></a></li>
                                                            </ItemTemplate>
                                                        </asp:Repeater>
                                                    </ul>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="tab-pane fade " id="Other">
                                        <div class="tabbable-panel">
                                            <div>
                                                <%--<div class=""><i class="fa fa-chevron-left"></i></div>
                                                            <div class=""><i class="fa fa-chevron-right"></i></div>--%>
                                                <div class="tabbable-line wrapper">
                                                    <ul class="nav nav-tabs list" id="myTabOther">
                                                        <asp:Repeater ID="RepeaterOther" runat="server">
                                                            <ItemTemplate>
                                                                <li onclick="FnFetchTopfundData('<%#Eval("Sebi_Sub_Nature_ID").ToString()%>','5')"
                                                                    role="presentation" <%# Container.ItemIndex == 0 ? " class='active'" : ""%>>
                                                                    <a><%#Eval("Sebi_Sub_Nature")%></a></li>
                                                            </ItemTemplate>
                                                        </asp:Repeater>
                                                    </ul>
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="tab-content pt-2" id="dvTopFundData">
                                        <!-- Syed table to refresh in subnature click-->
                                       <%-- <div id="dvTopFundData">
                                        </div>--%>
                                        <!-- end-->
                                    </div>
                                    <div style="width: 100%; float: right; text-align: right; font-size: 10px; color: #A7A7A7">
                                        Developed by: <a href="https://www.icraanalytics.com"
                                            target="_blank" style="font-size: 10px; color: #999999">ICRA Analytics Ltd</a>, <a style="font-size: 10px; color: #999999"
                                                href="https://icraanalytics.com/home/Disclaimer"
                                                target="_blank">Disclaimer </a>
                                    </div>

                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript">
        $(document).ready(function () {
            //alert("wliefh");
            subNatureId = 25;
            natureId = 4;
            //debugger;
            var val = '{"SebiSubNatureId":"' + subNatureId + '","SebiNatureId":"' + natureId + '"}';
            $.ajax({
                type: "POST",
                url: "CategoryWiseMutualFund.aspx/getTopFundData",
                async: false,
                contentType: "application/json",
                data: val,
                dataType: "json",
                success: function (msg) {
                    var RtnData = JSON.parse(msg.d);
                    if (RtnData.length > 0)
                        FNSetData(RtnData);
                },
                error: function (msg) {
                    alert("An Error Occured.");
                }
            });
        });

        function FnFetchTopfundData(Sub_id, id) {
            if (id == 3) {
                setTimeout(function () {
                    reAdjust("Debt");
                }, 2000);
            }
            if (id == 1) {
                setTimeout(function () {
                    reAdjust("Hybrid");
                }, 2000);
            }
            if (id == 4) {
                setTimeout(function () {
                    reAdjust("Equity");
                }, 2000);
            }
            //alert("onclick");
            subNatureId = Sub_id;
            natureId = id;
            //debugger;
            var val = '{"SebiSubNatureId":"' + subNatureId + '","SebiNatureId":"' + natureId + '"}';

            $.ajax({
                type: "POST",
                url: "CategoryWiseMutualFund.aspx/getTopFundData",
                async: false,
                contentType: "application/json",
                data: val,
                dataType: "json",
                success: function (msg) {
                    var RtnData = JSON.parse(msg.d);
                    if (RtnData.length > 0)
                        FNSetData(RtnData, subNatureId);
                },
                error: function (msg) {
                    alert("An Error Occured.");
                }
            });
        }

        function FNSetData(data,SN_Id) {
            var Html = "<div class='tab-pane fade active in ' id='SN_" + SN_Id + "'><div class='tabbable-panel'><table width='100%' border='0' cellpadding='0' cellspacing='0' class='table table-bordered'>";
            Html = Html + "<thead><tr><th align='left' id='lstResult_lnkSchName' >Scheme Name</th><th align='center' style='text-align: right'><span id='lstResult_Label1' >AUM (Cr.)</span></th><th id='1Week' style='text-align: right;'>1W</th><th id='1Month' style='text-align: right;'>1M</th><th id='3Month' style='text-align: right;'>3M</th><th id='6Month' style='text-align: right;'>6M</th><th id='1Year' style='text-align: right;'>1Y</th><th id='3Year' style='text-align: right;'>3Y</th><th id='5Year' style='text-align: right;'>5Y</th><th id='SI' style='text-align: right;'>SI</th></tr></thead>";
            for (v = 0; v < data.length; v++) {
                var schName = data[v].Sch_Name;
                var strTr = "<tr class='home_body'>";
                //strTr = strTr + "<td title='" + (schName) + "'><a href='http://localhost:20801/Bloomkite/FactSheet.aspx?param=" + data[v].AmfiCode + "' target='_blank'>" + (schName.length > 56 ? schName.substring(0, 56) + "..." : schName) + "</a></td>";
                //strTr = strTr + "<td title='" + (schName) + "'><a href='http://development-bloomkite.s3-website.us-east-2.amazonaws.com/factsheet?param=" + data[v].AmfiCode + "&title=MutualFunds' target='_blank'>" + (schName.length > 56 ? schName.substring(0, 56) + "..." : schName) + "</a></td>";
                strTr = strTr + "<td title='" + (schName) + "'><a href='https://www.bloomkite.com/factsheet?param=" + data[v].AmfiCode + "&title=MutualFunds' target='_blank'>" + (schName.length > 56 ? schName.substring(0, 56) + "..." : schName) + "</a></td>";

                strTr = strTr + "<td style='text-align: right'>" + (data[v].Fund_Size == null || data[v].Fund_Size === undefined || data[v].Fund_Size == 'null' ? '--' : data[v].Fund_Size.toFixed(2)) + "</td>";
                strTr = strTr + "<td style='text-align: right'>" + (data[v].Per_7_Days == null || data[v].Per_7_Days === undefined || data[v].Per_7_Days == 'null' ? '--' : data[v].Per_7_Days.toFixed(2)) + "</td>";
                strTr = strTr + "<td style='text-align: right'>" + (data[v].Per_30_Days == null || data[v].Per_30_Days === undefined || data[v].Per_30_Days == 'null' ? '--' : data[v].Per_30_Days.toFixed(2)) + "</td>";
                strTr = strTr + "<td style='text-align: right'>" + (data[v].Per_91_Days == null || data[v].Per_91_Days === undefined || data[v].Per_91_Days == 'null' ? '--' : data[v].Per_91_Days.toFixed(2)) + "</td>";
                strTr = strTr + "<td style='text-align: right'>" + (data[v].Per_182_Days == null || data[v].Per_182_Days === undefined || data[v].Per_182_Days == 'null' ? '--' : data[v].Per_182_Days.toFixed(2)) + "</td>";
                strTr = strTr + "<td style='background-color: #ebebeb;text-align: right'>" + (data[v].Per_1_Year == null || data[v].Per_1_Year === undefined || data[v].Per_1_Year == 'null' ? '--' : data[v].Per_1_Year.toFixed(2)) + "</td>";
                strTr = strTr + "<td  style='text-align: right'>" + (data[v].Per_3_Year == null || data[v].Per_3_Year === undefined || data[v].Per_3_Year == 'null' ? '--' : data[v].Per_3_Year.toFixed(2)) + "</td>";
                strTr = strTr + "<td  style='text-align: right'>" + (data[v].Per_5_Year == null || data[v].Per_5_Year === undefined || data[v].Per_5_Year == 'null' ? '--' : data[v].Per_5_Year.toFixed(2)) + "</td>";
                strTr = strTr + "<td  style='text-align: right'>" + (data[v].Since_Inception == null || data[v].Since_Inception === undefined || data[v].Since_Inception == 'null' ? '--' : data[v].Since_Inception.toFixed(2)) + "</td>";
                var strTr = strTr + "</tr>";
                Html = Html + strTr;
            }
            Html = Html + "</table></div></div>";
            $("#dvTopFundData").html(Html);
        }

    </script>
    <script>
        //$('.nav-tabs li').click(function () {
        //    $('.nav-tabs li.active').removeClass('active');
        //    $(this).addClass('active');
        //});

        $("ul.nav-tabs a").click(function (e) {
            e.preventDefault();
            $(this).tab('show');
        });
        var hidWidth;
        var scrollBarWidths = 40;

        var widthOfList = function (Id) {
            var itemsWidth = 0;
            $('#' + Id + ' .list li').each(function () {
                var itemWidth = $(this).outerWidth();
                itemsWidth += itemWidth;
            });
            return itemsWidth;
        };

        //var widthOfHidden = function (Id) {
        //    return (($('#' + Id + ' .wrapper').outerWidth()) - widthOfList(Id) - getLeftPosi(Id)) - scrollBarWidths;
        //};

        //var getLeftPosi = function (Id) {
        //    return $('#' + Id + ' .list').position().left;
        //};
        var widthOfHidden = function (Id) {

            var ww = 0 - $('#' + Id + ' .wrapper').outerWidth();
            var hw = (($('#' + Id + ' .wrapper').outerWidth()) - widthOfList(Id) - getLeftPosi(Id)) - scrollBarWidths;

            if (ww > hw) {
                return ww;
            }
            else {
                return hw;
            }

        };

        var getLeftPosi = function (Id) {

            var ww = 0 - $('#' + Id + ' .wrapper').outerWidth();
            var lp = $('#' + Id + ' .list').position().left;

            if (ww > lp) {
                return ww;
            }
            else {
                return lp;
            }
        };

        var reAdjust = function (Id) {
           // alert(Id);
            if (($('#' + Id + ' .wrapper').outerWidth()) < widthOfList(Id)) {
                $('#' + Id + ' .scroller-right').show();
            }
            else {
                $('#' + Id + ' .scroller-right').hide();
            }

            if (getLeftPosi(Id) < 0) {
                $('#' + Id + ' .scroller-left').show();
            }
            else {
                $('#' + Id + ' .item').animate({ left: "-=" + getLeftPosi(Id) + "px" }, 'slow');
                $('#' + Id + ' .scroller-left').hide();
            }
        }

        reAdjust("Equity");

        //$(window).on('resize', function (e) {
        //    reAdjust("Equity");
        //});

        $('.scroller-right').click(function () {
            var Id = (this).parentElement.parentElement.parentElement.id;
            $('#' + Id + ' .scroller-left').fadeIn('slow');
            $('#' + Id + ' .scroller-right').fadeOut('slow');
            $('#' + Id + ' .list').animate({ left: "+=" + widthOfHidden(Id) + "px" }, 'slow', function () {
                reAdjust(Id);
            });
        });

        $('.scroller-left').click(function () {
            var Id = (this).parentElement.parentElement.parentElement.id;

            $('#' + Id + ' .scroller-right').fadeIn('slow');
            $('#' + Id + ' .scroller-left').fadeOut('slow');

            $('#' + Id + ' .list').animate({ left: "-=" + getLeftPosi(Id) + "px" }, 'slow', function () {
                reAdjust(Id);
            });
        });
        
    </script>
</body>
</html>
