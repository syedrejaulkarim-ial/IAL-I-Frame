<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="schemesearch.aspx.cs" Inherits="iFrames.PrismAdvisory.schemesearch" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <meta name="description" content="" />
    <meta name="author" content="" />
    <link rel="shortcut icon" href="assets/ico/favicon.ico" />

    <title>Scheme Search</title>

    <!-- CSS Plugins -->
    <link rel="stylesheet" href="font-awesome-4.7.0/css/font-awesome.min.css" />
    <link rel="stylesheet" href="css/pe-icon-7-stroke.css" />
    <!-- CSS Global -->
    <link rel="stylesheet" href="css/style.css" />
    <link href="css/auto.css" rel="stylesheet" type="text/css" />
    <link rel='stylesheet' href='http://fonts.googleapis.com/css?family=Open+Sans%3A100%2C100italic%2C200%2C200italic%2C300%2C300italic%2C400%2C400italic%2C500%2C500italic%2C600%2C600italic%2C700%2C700italic%2C800%2C800italic%2C900%2C900italic%7CRoboto%3A100%2C100italic%2C200%2C200italic%2C300%2C300italic%2C400%2C400italic%2C500%2C500italic%2C600%2C600italic%2C700%2C700italic%2C800%2C800italic%2C900%2C900italic&#038;subset=latin%2Clatin-ext&#038;ver=1.0.0' type='text/css' media='all' />
    <link href="css/jquery-ui.css" rel="stylesheet" />
    <!-- / .wrapper -->
    <style>
        .tooltip {
            position: absolute;
            z-index: 1020;
            display: block;
            padding: 5px;
            font-size: 11px;
            visibility: visible;
            margin-top: -2px;
            bottom: 120%;
            margin-left: -1.3em;
            opacity: 1 !important;
        }

            .tooltip .tooltip-arrow {
                bottom: -3px;
                left: 50%;
                margin-left: -8px;
                border-top: 8px solid #000000;
                border-right: 8px solid transparent;
                border-left: 8px solid transparent;
                position: absolute;
                width: 0;
                height: 0;
            }

        .tooltip-inner {
            min-width: 30px;
            padding: 3px 5px;
            color: #ffffff;
            text-align: center;
            text-decoration: none;
            background-color: #000000;
            -webkit-border-radius: 4px;
            -moz-border-radius: 4px;
            border-radius: 4px;
        }

        .ui-slider {
            background: #cc0000 repeat-x bottom left;
            border-bottom: 1px solid #EBEAE5;
            height: 15px;
            margin: 0;
            padding: 0px;
        }

        .ui-slider-handle {
            background: #cc0000;
            border-bottom: 1px solid #a8a79f;
            border-right: 1px solid #a8a79f;
            height: 20px;
            width: 10px;
            margin: 0;
            padding: 0px;
            display: inline-block;
        }

        .ui-state-focus {
            outline: none;
        }

        .ui-slider-range {
            height: 10px;
            border-bottom: 3px solid red;
            position: relative;
        }

        .ui-slider-handle {
            background: #cc0000 !important;
            border-bottom: 1px solid #a8a79f !important;
            border-right: 1px solid #a8a79f !important;
            height: 20px !important;
            width: 10px !important;
            margin: 0 !important;
            padding: 0px !important;
            display: inline-block !important;
            top: -4px !important;
            transition: width .15s,height .15s,background-color .15s;
        }

        .ui-state-default, .ui-widget-content .ui-state-default, .ui-widget-header .ui-state-default {
            border-radius: 50%;
            border: 0 !important;
            width: 15px !important;
            height: 15px !important;
            margin-top: -2px !important;
        }
    </style>

    <!-- JavaScript
    ================================================== -->

    <!-- JS Global -->

    <script src="js/jquery.min.js"></script>
    <script type="text/javascript" src="js/jquery-ui.js"></script>
    <script src="js/bootstrap.min.js"></script>
    <%-- <script src="js/bootstrap-slider.min.js"></script>--%>
    <%--  <script src="js/perfect-scrollbar.jquery.min.js"></script>--%>
    <script src="js/AutoComplete.js" type="text/javascript"></script>
    <!-- JS Plugins -->


    <!-- JS Custom -->
    <%-- <script src="js/theme.min.js"></script>--%>
    <%--  <script src="js/kite.min.js"></script>--%>
    <script type="text/javascript">
        $(function () {
            var initialValue = 500;
            var sliderTooltip = function (event, ui) {
                
                var curValue = ui.value || initialValue;
                var tooltip1 = '<div class="tooltip"><div class="tooltip-inner">' + curValue + '</div><div class="tooltip-arrow"></div></div>';

                $('#unranged-value .ui-slider-handle').html(tooltip1);

            }
            var initialValueSIP = 5;
            var sliderTooltipSIP = function (event, ui) {
               
                var curValue = ui.value || initialValueSIP;
                var tooltip = '<div class="tooltip"><div class="tooltip-inner">' + curValue + '</div><div class="tooltip-arrow"></div></div>';

                $('#unranged-SIreturn .ui-slider-handle').html(tooltip);

            }
            $("#unranged-value").slider({
                value: $("#HiddenMinimumInvesment").val(),
                min: 500,
                max: 10000,
                step: 1,
                range: "max",
                create: sliderTooltip,
                slide: sliderTooltip,
                // stop: repositionTooltip,
                change: function (event, ui) {
                    var MinimumInvesment = $("#unranged-value").slider('value');
                    $("#HiddenMinimumInvesment").val(MinimumInvesment);
                }
            });
            // $("#unranged-value .ui-slider-handle:first").tooltip({ title: $("#unranged-value").slider("value"), trigger: "manual" }).tooltip("show");
            $("#unranged-SIreturn").slider({
                value: $("#HiddenMinimumSIReturn").val(),
                min: 5,
                max: 50,
                step: 1,
                range: "max",
                create: sliderTooltipSIP,
                slide: sliderTooltipSIP,
                //stop: repositionTooltip,
                change: function (event, ui) {
                    var MinimumInvesment = $("#unranged-SIreturn").slider('value');
                    $("#HiddenMinimumSIReturn").val(MinimumInvesment);
                }
            });
            // $("#unranged-SIreturn .ui-slider-handle:first").tooltip({ title: $("#unranged-SIreturn").slider("value"), trigger: "manual" }).tooltip("show");

            $('#rdbOption').find('td:first').after('<td><label></label></td>');

        });

    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="wrapper">

            <!-- SIDEBAR
      ================================================== -->


            <!-- MAIN CONTENT
      ================================================== -->
            <div class="container-fluid">
                <div class="row">
                    <div class="col-xs-12">
                        <h3></h3>
                    </div>
                </div>
                <!-- / .row -->
                <div class="row">
                    <div class="col-xs-12 col-lg-10" style="margin: 0 auto;">

                        <!-- Basic form -->
                        <div class="panel panel-default">
                            <div class="panel-heading"></div>
                            <div class="panel-body">
                                <div class="form-horizontal">
                                    <div class="form-group">
                                        <%--<label class="col-sm-2 control-label-select">Scheme Search</label>--%>
                                        <div class="col-sm-7">
                                            <asp:TextBox ID="txtSearch" runat="server" placeholder="Type scheme name" CssClass="form-control"></asp:TextBox>
                                            <asp:HiddenField ID="hfCustomerId" runat="server" />
                                        </div>
                                        <div class="col-sm-5"></div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-sm-2 control-label-select">Category</label>
                                        <div class="col-sm-5">
                                            <asp:DropDownList ID="ddlCategory" class="form-control" runat="server" OnSelectedIndexChanged="ddlCategory_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                        </div>
                                        <div class="col-sm-5"></div>
                                    </div>
                                    <div class="form-group">
                                        <label for="inputEmail3" class="col-sm-2 control-label-select">Sub-Category</label>
                                        <div class="col-sm-5">
                                            <asp:DropDownList ID="ddlSubCategory" class="form-control" runat="server"></asp:DropDownList>
                                        </div>
                                        <div class="col-sm-5"></div>
                                    </div>
                                    <div class="form-group">
                                        <label for="inputEmail3" class="col-sm-2 control-label-select">Type</label>
                                        <div class="col-sm-5">
                                            <asp:DropDownList ID="ddlType" class="form-control" runat="server"></asp:DropDownList>
                                        </div>
                                        <div class="col-sm-5"></div>
                                    </div>
                                    <div class="form-group m-b-sm">
                                        <label for="inputPassword3" class="col-sm-2 control-label-select">Option</label>
                                        <div class="col-sm-10">
                                            <div class="style-radio2">
                                                <div class="style-radio2">
                                                    <asp:RadioButtonList ID="rdbOption" runat="server" class="radio" RepeatDirection="Horizontal" Style="margin-left: 5px; padding-top: 5px;"></asp:RadioButtonList>
                                                </div>
                                            </div>
                                            <%--<div class="col-sm-2" style="padding-left:0;">
                      <div class="radio">
                        <input type="radio" name="optionsRadios" id="optionsRadios1" value="option1" checked>
                        <label for="optionsRadios1">Growth</label>
                      </div>
                    </div>
                    <div class="col-sm-8">
                      <div class="radio">
                        <input type="radio" name="optionsRadios" id="optionsRadios2" value="option2">
                        <label for="optionsRadios2">Dividend</label>
                      </div>
                    </div>--%>
                                        </div>
                                    </div>

                                    <div class="form-group m-t-md m-b-xs">
                                        <label for="inputEmail3" class="col-sm-2 control-label-select">Min. Investment</label>
                                        <div class="col-sm-5">
                                            <%--<input id="ex2" data-slider-id='ex1Slider' type="text" data-slider-min="500" data-slider-max="10000" data-slider-step="1" data-slider-value="500"/>--%>
                                            <asp:HiddenField ID="HiddenMinimumInvesment" runat="server" Value="500" />
                                            <div class="row">
                                                <div id="unranged-value" class="ui-slider-grey" style="width: 98%; height: 2px; margin-left: 8px; margin-top: 25px; border: 0"></div>
                                                <div class="col-xs-6">
                                                    <div class="slider__caption"><i class="fa fa-inr" aria-hidden="true"></i>500</div>
                                                </div>
                                                <div class="col-xs-6 text-right">
                                                    <div class="slider__caption"><i class="fa fa-inr" aria-hidden="true"></i>10,000</div>
                                                </div>

                                            </div>
                                        </div>
                                    </div>
                                    <div class="form-group m-t-md">
                                        <label for="inputEmail3" class="col-sm-2 control-label-select">Min. SI Return (%)</label>
                                        <div class="col-sm-5">
                                            <%-- <input id="ex8" data-slider-id='ex1Slider' type="text" data-slider-min="5" data-slider-max="50" data-slider-step="1" data-slider-value="5"/>--%>
                                            <asp:HiddenField ID="HiddenMinimumSIReturn" runat="server" Value="5" />
                                            <div class="row">
                                                <div id="unranged-SIreturn" class="ui-slider-grey" style="width: 98%; height: 2px; margin-left: 8px; margin-top: 25px; border: 0"></div>
                                                <div class="col-xs-6">
                                                    <div class="slider__caption">5</div>
                                                </div>
                                                <div class="col-xs-6 text-right">
                                                    <div class="slider__caption">50</div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="form-group">
                                        <div class="col-sm-2"></div>
                                        <div class="col-sm-8">
                                            <div class="btn-group" role="group" aria-label="Justified button group with nested dropdown">
                                                <asp:Button ID="btnSubmit" class="btn btn-primary" runat="server" Text="Submit" OnClick="btnSubmit_Click" />
                                                <asp:Button ID="btnReset" class="btn btn-primary" runat="server" Text="Reset" OnClick="btnResetClick" />
                                            </div>

                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <!-- Bordered table -->
                        <div class="col-xs-12 col-lg-2"></div>
                    </div>
                    <div class="col-xs-12 col-lg-2"></div>
                    <!-- / .row -->
                </div>
                <div class="row" id="Result" runat="server">
                    <div class="col-xs-12 col-lg-12">
                        <div class="panel panel-default">
                            <div class="panel-body">
                                <div class="ribbon ribbon_primary">
                                    <div class="ribbon__title">
                                        <asp:Label ID="lbtopText" runat="server" Text=""></asp:Label>
                                    </div>
                                </div>
                                <div class="table-responsive">
                                    <asp:ListView ID="lstResult" runat="server" OnPagePropertiesChanging="lst_PagePropertiesChanging">
                                        <LayoutTemplate>
                                            <table class="table table-bordered">
                                                <thead>
                                                    <tr class="table-header">
                                                        <th style="text-align: center">
                                                            <asp:Label ID="lnkRank" runat="server" SkinID="lblHeader" CommandName="Sort" CommandArgument="Rank" Text="Sl. No." />
                                                        </th>

                                                        <th style="text-align: left">
                                                            <asp:Label ID="lnkSchName" runat="server" SkinID="lblHeader" CommandName="Sort" CommandArgument="SchName" Text="Scheme Name" />
                                                        </th>

                                                        <th style="text-align: left">
                                                            <asp:Label ID="lnkNature" runat="server" SkinID="lblHeader" CommandName="Sort" CommandArgument="Nature" Text="Category" />
                                                        </th>

                                                        <th style="text-align: left">
                                                            <asp:Label ID="lnkSubnature" runat="server" SkinID="lblHeader" CommandName="Sort" CommandArgument="Subnature" Text="Sub Category" />
                                                        </th>



                                                        <th style="text-align: right">
                                                            <asp:Label ID="lnkNav" runat="server" SkinID="lblHeader" CommandName="Sort" CommandArgument="NAV" Text="NAV" />
                                                        </th>

                                                        <th style="text-align: right">
                                                            <asp:Label ID="lnkPeriod" runat="server" SkinID="lblHeader" CommandName="Sort" CommandArgument="Period" Text="Return % (1 year)"></asp:Label>
                                                        </th>

                                                        <th style="text-align: right">
                                                            <asp:Label ID="lnkInception" runat="server" SkinID="lblHeader" CommandName="Sort" CommandArgument="Inception" Text="Since Inception" />
                                                        </th>
                                                        <th style="text-align: right">
                                                            <asp:Label ID="Label1" runat="server" SkinID="lblHeader" CommandName="Sort" CommandArgument="Fund_Size" Text="Fund Size(Cr)" />
                                                        </th>
                                                        <%--<th style="text-align:right">
                                                <asp:Label ID="lblWatchList" runat="server" SkinID="lblHeader" CommandName="Sort" CommandArgument="WatchList" Text="Watch List"></asp:Label>
                                            </th>--%>
                                                        <th style="text-align: center">
                                                            <asp:Label ID="Label3" runat="server" SkinID="lblHeader" CommandName="Sort" CommandArgument="InvestNow" Text="Invest Now" Style="text-align: center"></asp:Label>
                                                        </th>
                                                    </tr>

                                                    <tr id="itemPlaceholder" runat="server">
                                                    </tr>
                                                </thead>
                                            </table>
                                            <div style="padding-top: 5px;">
                                            </div>
                                            <div>
                                                <table>
                                                    <tr>

                                                        <div style="width: 100%; float: left">
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

                                                    </tr>
                                                </table>
                                            </div>

                                        </LayoutTemplate>
                                        <ItemTemplate>
                                            <tr class='<%# Convert.ToBoolean(Container.DataItemIndex % 2) ? "value_tablerow" : "value_tablerow" %>'>
                                                <td>
                                                    <asp:Label runat="server" ID="lblType"><%#Eval("Rank") %></asp:Label>
                                                </td>
                                                <td style="width: 35%;">
                                                    <%--<a href="#"><%#Eval("Sch_Name")%></a>--%>
                                                    <a href="/Gomutualfund/Factsheet.aspx?param=<%#Eval("SchemeId")%>" target="_blank"><%#Eval("Sch_Name")%></a>
                                                </td>
                                                <td style="text-align: left">
                                                    <asp:Label runat="server" ID="lblNature" Text='<%#Eval("Nature")%>' />
                                                </td>
                                                <td style="text-align: left">
                                                    <asp:Label runat="server" ID="lblSubNature" Text='<%#Eval("SubNature")%>' />
                                                </td>

                                                <td style="text-align: right">
                                                    <asp:Label runat="server" ID="lblNav" Text='<%#Eval("Nav")%>' />
                                                </td>
                                                <td style="text-align: right">
                                                    <asp:Label runat="server" ID="Label3" Text='<%# Convert.ToDouble(Eval("Per_1_Year")).ToString("n2")%>' />
                                                </td>
                                                <td style="text-align: right">
                                                    <%# Convert.ToDouble(Eval("Since_Inception")).ToString("n2")%>
                                                </td>
                                                <td style="text-align: right">
                                                    <%# Convert.ToDouble(Eval("Fund_Size")).ToString("n2") %>
                                                </td>
                                                <%--<td style="text-align:center">
                                            <img src="images/watch.jpg" style="cursor: pointer" alt="" name="imgAdd2Watch" id='<%#Eval("SchemeId")%>' />                                            
                                            
                                        </td>--%>
                                                <td style="text-align: center">
                                                    <a target="_blank" href="http://gomutualfund.com/contact">
                                                        <img src="img/rupee.png" style="cursor: pointer" alt="" onclick="callCross('<%#Eval("SchemeId")%>','<%#Eval("Sch_Name")%>','<%#Eval("OptionId")%>','<%#Eval("Nature")%>','<%#Eval("SubNature")%>')" />
                                                    </a>
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                        <EmptyDataTemplate>
                                            Data not Found
                                        </EmptyDataTemplate>
                                    </asp:ListView>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <!-- Footer -->
                <footer class="page__footer">
                    <div class="row">
                        <div class="col-xs-12">
                            <span class="page__footer__year"></span>Developed for Gomutualfund by: <a href="https://www.icraanalytics.com" target="_blank">ICRA Analytics Ltd</a>&nbsp;<a style="font-size: 10px; color: #999999" href="https://icraanalytics.com/home/Disclaimer" target="_blank">Disclaimer</a>
                        </div>
                    </div>
                </footer>


                <!-- / .container-fluid -->

            </div>
        </div>
        <asp:HiddenField ID="HiddenFundRisk" runat="server" Value="-1" />
        <asp:HiddenField ID="HiddenFundRiskStrColor" runat="server" Value="All" />
        <asp:HiddenField ID="hdIsLoad" runat="server" Value="0" />
        <asp:HiddenField ID="Userid" runat="server" Value="asas" />
    </form>
</body>
</html>
