<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CategoryWiseReturn.aspx.cs" Inherits="iFrames.prithvivallabh.CategoryWiseReturn" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta content="origin-when-cross-origin" name="referrer">
    <meta name="viewport" content="width=device-width, initial-scale=1">

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
        .card {
            border: none;
        }

        .card-body {
            padding: 0 !important;
        }

        .card-header {
            padding: 10px !important;
        }

            .card-header h5 {
                margin: 0;
            }

        label {
            margin-bottom: 0.4rem;
            margin-top: 0.5rem;
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
            margin-bottom: 10px;
        }

        .list {
            position: absolute;
            left: 0px;
            top: 0px;
            min-width: 3000px;
            margin-left: 12px;
            margin-top: 0px;
        }

            .list li {
                display: table-cell;
                position: relative;
                text-align: center;
                cursor: grab;
                cursor: -webkit-grab;
                color: #efefef;
                vertical-align: middle;
            }

        .scroller {
            text-align: center;
            cursor: pointer;
            display: none;
            padding: 7px;
            padding-top: 6px;
            white-space: nowrap;
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
            font-size: 12px
        }

        .color {
            background-color: #ebebeb;
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
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12">
            <div class="card">
                <div class="card-header bg-header">
                    <h5>Category wise Mutual Fund Returns (%)</h5>
                </div>
                <div class="card-body">
                    <div class="">
                        <div class="row">
                            <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12 pt-2">
                                <div class="table-responsive">
                                    <div class="tabbable boxed parentTabs">
                                        <ul class="nav nav-tabs">
                                            <li class="active" onclick="FnFetchTopfundData('25','4','Per_1_Year')">
                                                <a href="#Equity">Equity</a>
                                            </li>
                                            <li onclick="FnFetchTopfundData('36','3','Per_1_Year')">
                                                <a href="#Debt">Debt</a>
                                            </li>
                                            <li onclick="FnFetchTopfundData('6','1','Per_1_Year')">
                                                <a href="#Hybrid">Hybrid</a>
                                            </li>
                                            <li onclick="FnFetchTopfundData('41','6','Per_1_Year')">
                                                <a href="#Solution_Oriented">Solution Oriented</a>
                                            </li>
                                            <li onclick="FnFetchTopfundData('48','5','Per_1_Year')">
                                                <a href="#Other">Other</a>
                                            </li>
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
                                                                        <li onclick="FnFetchTopfundData('<%#Eval("Sebi_Sub_Nature_ID").ToString()%>','4','Per_1_Year')"
                                                                            role="presentation" <%# Container.ItemIndex == 0 ? " class='active'" : ""%>>
                                                                            <a><%#Eval("Sebi_Sub_Nature")%></a></li>
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
                                                                        <li onclick="FnFetchTopfundData('<%#Eval("Sebi_Sub_Nature_ID").ToString()%>','3','Per_1_Year')"
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
                                                                        <li onclick="FnFetchTopfundData('<%#Eval("Sebi_Sub_Nature_ID").ToString()%>','1','Per_1_Year')"
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
                                                                        <li onclick="FnFetchTopfundData('<%#Eval("Sebi_Sub_Nature_ID").ToString()%>','6','Per_1_Year')"
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
                                                                        <li onclick="FnFetchTopfundData('<%#Eval("Sebi_Sub_Nature_ID").ToString()%>','5','Per_1_Year')"
                                                                            role="presentation" <%# Container.ItemIndex == 0 ? " class='active'" : ""%>>
                                                                            <a><%#Eval("Sebi_Sub_Nature")%></a></li>
                                                                    </ItemTemplate>
                                                                </asp:Repeater>
                                                            </ul>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="tab-content">
                                                <!-- Syed table to refresh in subnature click-->
                                                <div id="dvTopFundData">
                                                </div>
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
        </div>
        <script type="text/javascript">
            $(document).ready(function () {
                //alert("wliefh");
                subNatureId = 25;
                natureId = 4;
                //debugger;
                //var val = "{'SebiSubNatureId':" + subNatureId + ",SebiNatureId:'" + natureId + "'}";
                var val = '{"SebiSubNatureId":"' + subNatureId + '","SebiNatureId":"' + natureId + '","period":"Per_1_Year"}';
                $.ajax({
                    type: "POST",
                    url: "CategoryWiseReturn.aspx/getTopFundData",
                    async: false,
                    contentType: "application/json",
                    data: val,
                    dataType: "json",
                    success: function (msg) {
                        var RtnData = JSON.parse(msg.d);
                        if (RtnData.length > 0)
                            FNSetData(RtnData, 'Per_1_Year');
                    },
                    error: function (msg) {
                        alert("An Error Occured.");
                    }
                });
            });



            function shortbyPeriod(period) {
                var val = '{"SebiSubNatureId":"' + subNatureId + '","SebiNatureId":"' + natureId + '","period":"' + period + '"}';
                $.ajax({
                    type: "POST",
                    url: "CategoryWiseReturn.aspx/getTopFundData",
                    async: false,
                    contentType: "application/json",
                    data: val,
                    dataType: "json",
                    success: function (msg) {
                        var RtnData = JSON.parse(msg.d);
                        if (RtnData.length > 0)
                            FNSetData(RtnData, period);
                    },
                    error: function (msg) {
                        alert("An Error Occured.");
                    }
                });

            }

            function FnFetchTopfundData(Sub_id, id, period) {
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
                //var val = "{'SebiSubNatureId':" + subNatureId + ",SebiNatureId:'" + natureId + "'}";
                //var val = "{'SebiSubNatureId':" + subNatureId + ",SebiNatureId:'" + natureId + "','period':'" + period + "'}";
                var val = '{"SebiSubNatureId":"' + subNatureId + '","SebiNatureId":"' + natureId + '","period":"' + period + '"}';
                $.ajax({
                    type: "POST",
                    url: "CategoryWiseReturn.aspx/getTopFundData",
                    async: false,
                    contentType: "application/json",
                    data: val,
                    dataType: "json",
                    success: function (msg) {
                        var RtnData = JSON.parse(msg.d);
                        if (RtnData.length > 0)
                            FNSetData(RtnData, period);
                    },
                    error: function (msg) {
                        alert("An Error Occured.");
                    }
                });
            }

            function FNSetData(data, period) {
                //document.getElementById(period).style.display= "none";

                var Html = "<table width='100%' border='0' cellpadding='0' cellspacing='0' class='table table-bordered'>";
                Html = Html + "<thead><tr>" +
                    "<th align='left' id='lstResult_lnkSchName' >Scheme Name</th>" +
                    //"<th align='center' style='text-align: center'><span id='lstResult_Label1' >AUM (Cr.)</span></th>" +
                    "<th align='center' style='text-align: center'><a onclick='shortbyPeriod(\"" + 'Fund_Size' + "\")' style='text-align:center;cursor:pointer;text-decoration:none; color: #fff;'>AUM (Cr.) &nbsp;<img id='lstResult_Label1' src='/prithvivallabh/images/down-arrow.svg' style='height: 12px; width: 12px;'></a></th>" +

                    "<th id='1Week' style='text-align: center;'><a onclick='shortbyPeriod(\"" + 'Per_7_Days' + "\")' style='text-align:center;cursor:pointer;text-decoration:none; color: #fff;'>1W &nbsp;<img id='Per_7_Days' src='/prithvivallabh/images/down-arrow.svg' style='height: 12px; width: 12px;'></a></th>" +
                    "<th id='1Month' style='text-align: center;'><a onclick='shortbyPeriod(\"" + 'Per_30_Days' + "\")' style='text-align:center;cursor:pointer;text-decoration:none;color: #fff;'>1M &nbsp;<img id='Per_30_Days' src='/prithvivallabh/images/down-arrow.svg' style='height: 12px; width: 12px;'></a></th>" +
                    "<th id='3Month' style='text-align: center;'><a onclick='shortbyPeriod(\"" + 'Per_91_Days' + "\")' style='text-align:center;cursor:pointer;text-decoration:none;color: #fff;'>3M &nbsp;<img id='Per_91_Days' src='/prithvivallabh/images/down-arrow.svg' style='height: 12px; width: 12px;'></a></th>" +
                    "<th id='6Month' style='text-align: center;'><a onclick='shortbyPeriod(\"" + 'Per_182_Days' + "\")' style='text-align:center;cursor:pointer;text-decoration:none;color: #fff;'>6M &nbsp;<img id='Per_182_Days' src='/prithvivallabh/images/down-arrow.svg' style='height: 12px; width: 12px;'></a></th>" +
                    "<th id='1Year' style='text-align: center;'><a onclick='shortbyPeriod(\"" + 'Per_1_Year' + "\")' style='text-align:center;cursor:pointer;text-decoration:none;color: #fff;'>1Y &nbsp;<img id='Per_1_Year' src='/prithvivallabh/images/down-arrow.svg' style='height: 12px; width: 12px;'></a></th>" +
                    "<th id='3Year' style='text-align: center;'><a onclick='shortbyPeriod(\"" + 'Per_3_Year' + "\")' style='text-align:center;cursor:pointer;text-decoration:none;color: #fff;'>3Y &nbsp;<img id='Per_3_Year' src='/prithvivallabh/images/down-arrow.svg' style='height: 12px; width: 12px;'></a></th>" +
                    "<th id='5Year' style='text-align: center;'><a onclick='shortbyPeriod(\"" + 'Per_5_Year' + "\")' style='text-align:center;cursor:pointer;text-decoration:none;color: #fff;'>5Y &nbsp;<img id='Per_5_Year' src='/prithvivallabh/images/down-arrow.svg' style='height: 12px; width: 12px;'></a></th>" +
                    "<th id='SI' style='text-align: center;'><a onclick='shortbyPeriod(\"" + 'Since_Inception' + "\")' style='text-align:center;cursor:pointer;text-decoration:none;color: #fff;'>SI &nbsp;<img id='Since_Inception' src='/prithvivallabh/images/down-arrow.svg' style='height: 12px; width: 12px;'></a></th></tr></thead>";
                for (v = 0; v < data.length; v++) {
                    var schName = data[v].Sch_Name;
                    var strTr = "<tr class='home_body'>";
                    //strTr = strTr + "<td>" + data[v].Sch_Name + "</td>";
                    strTr = strTr + "<td title='" + (schName) + "'><a href='/prithvivallabh/Factsheet.aspx?param=" + data[v].SchemeId + "' target='_blank'>" + (schName.length > 56 ? schName.substring(0, 56) + "..." : schName) + "</a></td>";
                    strTr = strTr + "<td style='text-align: right'>" + (data[v].Fund_Size == null || data[v].Fund_Size === undefined || data[v].Fund_Size == 'null' ? '--' : data[v].Fund_Size.toFixed(2)) + "</td>";
                    strTr = strTr + "<td " + (period == 'Per_7_Days' ? "class='color'" : '') + " style='text-align: right'>" + (data[v].Per_7_Days == null || data[v].Per_7_Days === undefined || data[v].Per_7_Days == 'null' ? '--' : data[v].Per_7_Days.toFixed(2)) + "</td>";
                    strTr = strTr + "<td " + (period == 'Per_30_Days' ? "class='color'" : '') + "style='text-align: right'>" + (data[v].Per_30_Days == null || data[v].Per_30_Days === undefined || data[v].Per_30_Days == 'null' ? '--' : data[v].Per_30_Days.toFixed(2)) + "</td>";
                    strTr = strTr + "<td " + (period == 'Per_91_Days' ? "class='color'" : '') + " style='text-align: right'>" + (data[v].Per_91_Days == null || data[v].Per_91_Days === undefined || data[v].Per_91_Days == 'null' ? '--' : data[v].Per_91_Days.toFixed(2)) + "</td>";
                    strTr = strTr + "<td " + (period == 'Per_182_Days' ? "class='color'" : '') + " style='text-align: right'>" + (data[v].Per_182_Days == null || data[v].Per_182_Days === undefined || data[v].Per_182_Days == 'null' ? '--' : data[v].Per_182_Days.toFixed(2)) + "</td>";
                    strTr = strTr + "<td " + (period == 'Per_1_Year' ? "class='color'" : '') + " style='text-align: right'>" + (data[v].Per_1_Year == null || data[v].Per_1_Year === undefined || data[v].Per_1_Year == 'null' ? '--' : data[v].Per_1_Year.toFixed(2)) + "</td>";
                    strTr = strTr + "<td " + (period == 'Per_3_Year' ? "class='color'" : '') + "  style='text-align: right'>" + (data[v].Per_3_Year == null || data[v].Per_3_Year === undefined || data[v].Per_3_Year == 'null' ? '--' : data[v].Per_3_Year.toFixed(2)) + "</td>";
                    strTr = strTr + "<td " + (period == 'Per_5_Year' ? "class='color'" : '') + " style='text-align: right'>" + (data[v].Per_5_Year == null || data[v].Per_5_Year === undefined || data[v].Per_5_Year == 'null' ? '--' : data[v].Per_5_Year.toFixed(2)) + "</td>";
                    strTr = strTr + "<td " + (period == 'Since_Inception' ? "class='color'" : '') + " style='text-align: right'>" + (data[v].Since_Inception == null || data[v].Since_Inception === undefined || data[v].Since_Inception == 'null' ? '--' : data[v].Since_Inception.toFixed(2)) + "</td>";
                    var strTr = strTr + "</tr>";
                    Html = Html + strTr;
                }
                Html = Html + "</table>";
                $("#dvTopFundData").html(Html);
            }

        </script>
        <script>
            $('.nav-tabs li').click(function () {
                $('.nav-tabs li.active').removeClass('active');
                $(this).addClass('active');
            });

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
    </form>
</body>
</html>
