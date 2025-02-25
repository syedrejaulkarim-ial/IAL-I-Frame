<%@ Page Language="C#" AutoEventWireup="true" ViewStateEncryptionMode="Always" EnableViewStateMac="true" CodeBehind="WatchList.aspx.cs" Inherits="iFrames.BansalCapital.WatchList" %>

<!DOCTYPE html>
<html class="no-js">
<head>
    <title>Watch List</title>
    <script type='text/javascript' src="js/jquery.js"></script>
    <script type="text/javascript" src="js/jquery-ui.js"></script>
    <script type='text/javascript' src="js/bootstrap.js"></script>
    <script src="js/AutoComplete.js" type="text/javascript"></script>

    <link rel="stylesheet" href="css/jquery-ui-1.10.3.custom.min.css" />
    <link rel="stylesheet" href="css/jquery-slider.css" />
    <link href="css/auto.css" rel="stylesheet" type="text/css" />
    <link href="bootstrap/css/bootstrap.min.css" rel="stylesheet" media="screen" />
    <link href="bootstrap/css/bootstrap-responsive.min.css" rel="stylesheet" media="screen" />
    <link href="bootstrap/css/styles.css" rel="stylesheet" media="screen" />
    <link href='http://fonts.googleapis.com/css?family=Open+Sans:400,600,700,300' rel='stylesheet' type='text/css' />
    <link href='http://fonts.googleapis.com/css?family=Open+Sans:400,600,700,300' rel='stylesheet' type='text/css' />
    <script src="js/modernizr-2.6.2-respond-1.1.0.min.js" type="text/javascript"></script>

    <!-- HTML5 shim, for IE6-8 support of HTML5 elements -->
    <!--[if lt IE 9]>
            <script src="http://html5shim.googlecode.com/svn/trunk/html5.js"></script>
        <![endif]-->
    <script type="text/javascript">
        WebFontConfig = {
            google: { families: ['Open+Sans:400,600,700,300:latin'] }
        };
        (function () {
            var wf = document.createElement('script');
            wf.src = ('https:' == document.location.protocol ? 'https' : 'http') +
                '://ajax.googleapis.com/ajax/libs/webfont/1/webfont.js';
            wf.type = 'text/javascript';
            wf.async = 'true';
            var s = document.getElementsByTagName('script')[0];
            s.parentNode.insertBefore(wf, s);
        })(); </script>


    <!-- Do not delete It will required for login , Invest now -->
    <script type="text/javascript">
        function callCross(schid, schname, OptionId, Nature, SubNature) {
            if (OptionId == "2")
                var option = "Growth";
            else
                var option = "Devidend";
            var data = { 'url': 'http://www.askmefund.com/transaction.aspx?Scheme_Name=' + schname + ',Option=' + option + ',SchemeId=' + schid + ',Category=' + Nature + ',Sub_Category=' + SubNature };
            top.postMessage(data, 'http://www.askmefund.com/watchlist.aspx');
        }

        $(function () {
            //            $.ajax({
            //                cache: false,
            //                dataType: "json",
            //                url: 'BansalProxy.aspx/getSession',
            //                type: 'POST',
            //                contentType: "application/json; charset=utf-8",
            //                success: function (dataConsolidated) {
            //                    var obj = jQuery.parseJSON(dataConsolidated.d);
            //                    if (obj.d == 1) {
            //                        return;
            //                    }
            //                    if (obj.d == 0) {
            //                        window.location("javascript:parent.window.location.href='http://localhost:52348/login.aspx'");
            //                    }
            //                },
            //                error: function (data) {
            //                    //alert(data);
            //                }
            //            });

            $('img[name="imgAdd2Watch"]').click(function () {
                var dataToPush = JSON.stringify({
                    schemeId: $(this).attr('id'),
                    user: $('#Userid').val()
                });
                $.ajax({
                    cache: false,
                    dataType: "json",
                    data: dataToPush,
                    url: 'BansalProxy.aspx/RemoveFrmWatchlist',
                    type: 'POST',
                    contentType: "application/json; charset=utf-8",
                    success: function (dataConsolidated) {
                        var obj = jQuery.parseJSON(dataConsolidated.d);
                        if (obj.d == 1) {
                            alert("Scheme removed from your watch list");
                            var data = { 'url': 'http://www.askmefund.com/watchlist.aspx' };
                            top.postMessage(data, 'http://www.askmefund.com/watchlist.aspx');
                        }
                        else if (obj.d == 0) {
                            //alert(obj.d);
                            //window.location("javascript:parent.window.location.href='http://localhost:52348/login.aspx'");
                            var data = { 'url': 'http://www.askmefund.com/login.aspx' };
                            top.postMessage(data, 'http://www.askmefund.com/watchlist.aspx');
                        }
                    },
                    error: function (data) {
                        // debugger;
                        //alert(data);
                    }
                });

            });
        });
    </script>

    <script type="text/javascript">

        var FundRiskValue;

        $(document).ready(function () {

            if ($("#HiddenFundRiskStrColor").val() != "All") {
                var cssclass = "span3 btn btn-" + $("#HiddenFundRiskStrColor").val() + " btn-large active";

                $("#" + $("#HiddenFundRiskStrColor").val()).attr("class", cssclass);
            }
            if (FundRiskValue == null) {
                $("#low").click(function (Func1) {
                    FundRiskValue = $("#low").attr("for");
                    $("#HiddenFundRisk").val(FundRiskValue);
                    $("#low").attr("class", "span3 btn btn-low btn-large active");
                    $("#mid").attr("class", "span3 btn btn-mid btn-large");
                    $("#high").attr("class", "span3 btn btn-high btn-large");
                    $("#all").attr("class", "span3 btn btn-all btn-large");
                    $("#HiddenFundRiskStrColor").val("low");
                });

                $("#mid").click(function (Func2) {
                    FundRiskValue = $("#mid").attr("for");
                    $("#HiddenFundRisk").val(FundRiskValue);
                    $("#low").attr("class", "span3 btn btn-low btn-large");
                    $("#mid").attr("class", "span3 btn btn-mid btn-large active");
                    $("#high").attr("class", "span3 btn btn-high btn-large");
                    $("#all").attr("class", "span3 btn btn-all btn-large");
                    $("#HiddenFundRiskStrColor").val("mid");
                });

                $("#high").click(function (Func3) {
                    FundRiskValue = $("#high").attr("for");
                    $("#HiddenFundRisk").val(FundRiskValue);
                    $("#low").attr("class", "span3 btn btn-low btn-large");
                    $("#mid").attr("class", "span3 btn btn-mid btn-large");
                    $("#high").attr("class", "span3 btn btn-high btn-large active");
                    $("#all").attr("class", "span3 btn btn-all btn-large");
                    $("#HiddenFundRiskStrColor").val("high");
                });

                $("#all").click(function (Func4) {
                    FundRiskValue = $("#all").attr("for");
                    $("#HiddenFundRisk").val(FundRiskValue);
                    $("#low").attr("class", "span3 btn btn-low btn-large");
                    $("#mid").attr("class", "span3 btn btn-mid btn-large");
                    $("#high").attr("class", "span3 btn btn-high btn-large");
                    $("#all").attr("class", "span3 btn btn-all btn-large active");
                    $("#HiddenFundRiskStrColor").val("all");
                });
            }
        });

    </script>
</head>

<body>
    <div style="width: 860px; padding: 0; padding: 0;">
        <form id="form1" runat="server">
            <asp:HiddenField ID="Userid" runat="server" Value="asas" />
            <div class="container-fluid" style="padding: 0;">
                <!-- commented by syed -->

                <div id="DivWatchList" class="row-fluid" runat="server">
                    <!-- block -->
                    <div id="Result" class="block" style="margin-top: -40px;" runat="server">
                        <div>
                            <div class="muted pull-left" style="color: #cc0000; font-weight: 700;">
                                <asp:Label ID="lbtopText" runat="server" Text=""></asp:Label>
                                <div class="pull-right"></div>
                            </div>

                            <%--<div class="navbar navbar-inner block-header">--%>

                            <div class="pull-right"></div>

                        </div>

                        <div class="block-content collapse in">

                            <asp:ListView ID="lstResult" runat="server" OnPagePropertiesChanging="lst_PagePropertiesChanging">
                                <LayoutTemplate>
                                    <table width="100%" border="0" cellpadding="0" cellspacing="0" class="table table-striped table-bordered" style="margin-top: 15px; font-size: 12px;">
                                        <tr>
                                            <th align="left">
                                                <asp:Label ID="lnkRank" runat="server" SkinID="lblHeader" CommandName="Sort" CommandArgument="Rank"
                                                    Text="Sl. No." />

                                            </th>
                                            <th align="left">
                                                <asp:Label ID="lnkSchName" runat="server" SkinID="lblHeader" CommandName="Sort" CommandArgument="SchName"
                                                    Text="Scheme Name" />
                                            </th>
                                            <%--<th align="left">
                                                    <asp:Label ID="lnkNature" runat="server" SkinID="lblHeader" CommandName="Sort" CommandArgument="Nature"
                                                        Text="Category" />
                                                </th>
                                                   <th align="left">
                                                    <asp:Label ID="lnkSubnature" runat="server" SkinID="lblHeader" CommandName="Sort" CommandArgument="Subnature"
                                                        Text="Sub Category" />
                                                </th>--%>
                                            <%--<th align="center">
                                                    <asp:Label ID="Label1" runat="server" SkinID="lblHeader" CommandName="Sort" CommandArgument="ICRON_Rank"
                                                        Text="ICRON Rank" />
                                                </th>--%>

                                            <th align="center">
                                                <asp:Label ID="lnkNav" runat="server" SkinID="lblHeader" CommandName="Sort" CommandArgument="NAV"
                                                    Text="NAV" />
                                            </th>
                                            <th align="center">
                                                <asp:Label ID="lnkPeriod" runat="server" SkinID="lblHeader" CommandName="Sort" CommandArgument="Period" Text="Return % (1 Year)"></asp:Label>
                                            </th>
                                            <th align="center">
                                                <asp:Label ID="Label1" runat="server" SkinID="lblHeader" CommandName="Sort" CommandArgument="Fund_Size" Text="Fund Size(Cr)" />
                                            </th>
                                            <th align="center">
                                                <asp:Label ID="lblWatchList" runat="server" SkinID="lblHeader" CommandName="Sort" CommandArgument="WatchList" Text="Watch List"></asp:Label>
                                            </th>
                                            <th align="center">
                                                <asp:Label ID="lblInvestNow" runat="server" SkinID="lblHeader" CommandName="Sort" CommandArgument="InvestNow" Text="Invest Now"></asp:Label>
                                            </th>
                                        </tr>
                                        <tr id="itemPlaceholder" runat="server">
                                        </tr>
                                    </table>
                                    <div style="padding-top: 5px;">
                                    </div>
                                    <div>
                                        <table style="width: 100%">
                                            <tr>
                                                <td class="value_input" style="width: 90%">
                                                    <div style="width: 65%; float: left; text-align: left; font-size: 12px">
                                                        <asp:DataPager ID="dpTopFund" runat="server" PageSize="10" PagedControlID="lstResult">
                                                            <Fields>
                                                                <asp:NextPreviousPagerField ShowFirstPageButton="True" ShowNextPageButton="False" />
                                                                <asp:NumericPagerField CurrentPageLabelCssClass="news_header" />
                                                                <asp:NextPreviousPagerField ShowLastPageButton="True" ShowPreviousPageButton="False" />
                                                                <asp:TemplatePagerField>
                                                                    <PagerTemplate>
                                                                        <span style="padding-left: 40px; text-align: right">Page
                                    <asp:Label runat="server" ID="CurrentPageLabel" Text="<%# Container.TotalRowCount>0 ? (Container.StartRowIndex / Container.PageSize) + 1 : 0 %>" />
                                                                            of
                                    <asp:Label runat="server" ID="TotalPagesLabel" Text="<%# Math.Ceiling (System.Convert.ToDouble(Container.TotalRowCount) / Container.PageSize) %>" />
                                                                            (
                                    <asp:Label runat="server" ID="TotalItemsLabel" Text="<%# Container.TotalRowCount%>" />
                                                                            records)
                                    <br />
                                                                        </span>
                                                                    </PagerTemplate>

                                                                </asp:TemplatePagerField>

                                                            </Fields>

                                                        </asp:DataPager>
                                                    </div>
                                                    <div class="value_input" style="width: 35%; float: right; text-align: right; font-size: 10px; color: #A7A7A7">
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>

                                </LayoutTemplate>
                                <ItemTemplate>
                                    <tr class='<%# Convert.ToBoolean(Container.DataItemIndex % 2) ? "value_tablerow" : "value_tablerow" %>'>
                                        <td>
                                            <asp:Label runat="server" ID="lblType"><%#Eval("Rank") %></asp:Label>
                                        </td>
                                        <td style="width: 45%;">
                                            <a href="Factsheet.aspx?param=<%#Eval("SchemeId")%>" target="_blank"><%#Eval("Sch_Name")%></a>
                                            <%-- <asp:Label runat="server" ID="lblSchName" Text='<%#Eval("sch_name")%>' />--%>
                                        </td>
                                        <%--<td>
                                                                                <asp:Label runat="server" ID="lblNature" Text='<%#Eval("Nature")%>' />
                                                                            </td>
                                                                            <td>
                                                                                <asp:Label runat="server" ID="lblSubNature" Text='<%#Eval("SubNature")%>' />
                                                                            </td>--%>
                                        <%--<td align="center">
                                                                                <asp:Label runat="server" ID="Label2" Text='<%#Eval("MF_Rank_Html")%>' />
                                                                            </td>--%>

                                        <td align="center">
                                            <asp:Label runat="server" ID="lblNav" Text='<%#Eval("Nav")%>' />  
                                        </td>
                                        <td align="center">
                                            <asp:Label runat="server" ID="Label3" Text='<%# (Eval("Per_1_Year") == DBNull.Value ? "--" : Convert.ToDouble(Eval("Per_1_Year")).ToString("N2") ) %>' />
                                        </td>
                                        <td align="center">
                                            <%# Convert.ToDouble(Eval("Fund_Size")).ToString("n2") %>
                                        </td>
                                        <td align="center">
                                            <img src="images/watch1.jpg" style="cursor: pointer" alt="" name="imgAdd2Watch" id='<%#Eval("SchemeId")%>' />
                                        </td>
                                        <td align="center">
                                            <%--<a href="javascript:parent.window.location.href='http://localhost:52348/InvestNow.aspx?schId=<%#Eval("SchemeId")%>&SchName=<%#Eval("Sch_Name")%>'">
                                                                                    <img src="images/invest.jpg" alt="" /></a>--%>

                                            <img src="images/invest.jpg" style="cursor: pointer" alt="" onclick="callCross('<%#Eval("SchemeId")%>','<%#Eval("Sch_Name")%>','<%#Eval("OptionId")%>','<%#Eval("Nature")%>','<%#Eval("SubNature")%>')" />
                                        </td>
                                    </tr>
                                </ItemTemplate>
                                <EmptyDataTemplate>
                                    Data not Found
                                </EmptyDataTemplate>
                            </asp:ListView>
                        </div>
                        <asp:HiddenField ID="hfCustomerId" runat="server" />
                        <asp:HiddenField ID="HdSelectedScheme" Value="" runat="server" />
                    </div>

                    <!-- /block -->
                </div>


                <%--<footer style="font-size:12px; margin-top: 80px; text-align:center">
                    Disclaimer: Mutual Fund investments are subject to market risks. Read all scheme
                                related documents carefully before investing. Past performance of the schemes do
                                not indicate the future performance.<br/>
                    Developed and Maintained by: ICRA Analytics Ltd
                          </footer>--%>
            </div>
            <!--/.fluid-container-->
            <%--         <script src="bootstrap/js/jquery-1.9.1.min.js" type="text/javascript"></script>
        <script src="bootstrap/js/bootstrap.min.js" type="text/javascript"></script>
         <script src="bootstrap/js/bootstrap-slider.js" type="text/javascript"></script>
        <script src="bootstrap/js/bootstrap.min.js" type="text/javascript"></script>
        <script src="bootstrap/js/scripts.js" type="text/javascript"></script>
        <script src="bootstrap/js/DT_bootstrap.js" type="text/javascript"></script>
        <script src="bootstrap/js/modernizr-2.6.2-respond-1.1.0.min.js" type="text/javascript"></script>--%>
        </form>
    </div>
</body>

</html>
