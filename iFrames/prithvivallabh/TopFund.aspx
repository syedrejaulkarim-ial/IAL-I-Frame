<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TopFund.aspx.cs" Inherits="iFrames.prithvivallabh.TopFund" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml" class="no-js">
<head  runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta content="origin-when-cross-origin" name="referrer">
    <meta name="viewport" content="width=device-width, initial-scale=1">

    <title>Top fund</title>

    <link href="css/bootstrap.css" rel="stylesheet" type="text/css" />
    <link href="css/IAL_style.css" rel="stylesheet" type="text/css" />

    <script src="https://use.fontawesome.com/ea6a7e4db5.js"></script>

    <link rel="stylesheet" href="css/jquery-ui-1.10.3.custom.min.css" />
    <link rel="stylesheet" href="css/jquery-slider.css" />

    <script type='text/javascript' src="js/jquery.js"></script>
    <script type="text/javascript" src="js/jquery-ui.js"></script>
    <script type='text/javascript' src="js/bootstrap.js"></script>


    <script src="js/AutoComplete.js" type="text/javascript"></script>
    <script src="js/modernizr2.js" type="text/javascript"></script>
    <link type="text/css" rel="stylesheet" href="css/bootstrap-multiselect.css" />
    <script src="js/bootstrap-multiselect.js"></script>

    <!-- HTML5 shim, for IE6-8 support of HTML5 elements -->
    <!--[if lt IE 9]>
            <script src="http://html5shim.googlecode.com/svn/trunk/html5.js"></script>
            
        <![endif]-->
    <script type="text/javascript">
//        function BlockUIpage() {            
//            $.blockUI({ message: '<div style="font-size:16px; font-weight:bold">Please wait...</div>' });
//        }
//        function UnBlockUIpage() {
//            $.unblockUI({ message: '<div style="font-size:16px; font-weight:bold">Please wait...</div>' });
//        }
    </script>
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
        })();
    </script>

    <!-- Do not delete It will required for login , Invest now -->

      <script type="text/javascript">
          $(function () {
              $('#ddlFundHouse').attr("multiple", "multiple");
              $('[id=ddlFundHouse]').multiselect({
                  includeSelectAllOption: true,
                  enableFiltering: true,
                  enableCaseInsensitiveFiltering: true,
                  nonSelectedText: 'Select Mutual Fund',
                  numberDisplayed: 1,
                  filterPlaceholder: 'Search Here..',
                  deselectAll: false
              });
              //$("#ddlFundHouse").val([]);
              var chkPostBack = '<%= Page.IsPostBack ? "true" : "false" %>';

            if (chkPostBack == 'false') {
                $('#ddlFundHouse').multiselect('deselectAll', false);
                $('#ddlFundHouse').multiselect('rebuild');
            }
          });
    </script>

    <script type="text/javascript">
        var dminyear = 1900;
        var dmaxyear = 2200;
        var chsep = "/";
        var DateDiff = {

            inDays: function (d1, d2) {
                var t2 = d2.getTime();
                var t1 = d1.getTime();

                return parseInt((t2 - t1) / (24 * 3600 * 1000));
            },

            inWeeks: function (d1, d2) {
                var t2 = d2.getTime();
                var t1 = d1.getTime();

                return parseInt((t2 - t1) / (24 * 3600 * 1000 * 7));
            },

            inMonths: function (d1, d2) {
                var d1Y = d1.getFullYear();
                var d2Y = d2.getFullYear();
                var d1M = d1.getMonth();
                var d2M = d2.getMonth();
                return (d2M + 12 * d2Y) - (d1M + 12 * d1Y);
            },

            inYears: function (d1, d2) {
                return d2.getFullYear() - d1.getFullYear();
            }
        }

        function checkinteger(str1) {
            var x;
            for (x = 0; x < str1.length; x++) {
                // verify current character is number or not !   
                var cr = str1.charAt(x);
                if (((cr < "0") || (cr > "9")))
                    return false;
            }
            return true;
        }
        function getcharacters(s, chsep1) {
            var x;
            var Stringreturn = "";
            for (x = 0; x < s.length; x++) {
                var cr = s.charAt(x);
                if (chsep.indexOf(cr) == -1)
                    Stringreturn += cr;
            }
            return Stringreturn;
        }
        function februarycheck(cyear) {
            return (((cyear % 4 == 0) && ((!(cyear % 100 == 0)) || (cyear % 400 == 0))) ? 29 : 28);
        }
        function finaldays(nr) {
            for (var x = 1; x <= nr; x++) {
                this[x] = 31
                if (x == 4 || x == 6 || x == 9 || x == 11) {
                    this[x] = 30
                }
                if (x == 2) {
                    this[x] = 29
                }
            }
            return this
        }

        function converterdate(string) {
            var d = string.split(/[ :\s]/);
            var months = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'sep', 'Oct', 'Nov', 'Dec'];
            var monthval = 1;

            for (var i = 0; i < months.length; i++) {
                if (months[i] == d[1]) {
                    monthval = parseInt(i + 1);
                    break;
                }
            }

            if (monthval < 10) {
                monthval = '0' + monthval;
            }

            return d[0] + '/' + monthval + '/' + d[2];
        }

        function dtvalid(strdate) {
            var monthdays = finaldays(12)
            var cpos1 = strdate.indexOf(chsep)
            var cpos2 = strdate.indexOf(chsep, cpos1 + 1)
            var daystr = strdate.substring(0, cpos1)
            var monthstr = strdate.substring(cpos1 + 1, cpos2)
            var yearstr = strdate.substring(cpos2 + 1)
            strYr = yearstr
            if (strdate.charAt(0) == "0" && strdate.length > 1) strdate = strdate.substring(1)
            if (monthstr.charAt(0) == "0" && monthstr.length > 1) monthstr = monthstr.substring(1)
            for (var i = 1; i <= 3; i++) {
                if (strYr.charAt(0) == "0" && strYr.length > 1) strYr = strYr.substring(1)
            }
            // The parseInt is used to get a numeric value from a string   
            pmonth = parseInt(monthstr)
            pday = parseInt(daystr)
            pyear = parseInt(strYr)
            if (cpos1 == -1 || cpos2 == -1) {
                //alert("The date format must be : dd/mm/yyyy")
                return false
            }
            if (monthstr.length < 1 || pmonth < 1 || pmonth > 12) {
                //alert("Input a valid month")
                return false
            }
            if (daystr.length < 1 || pday < 1 || pday > 31 || (pmonth == 2 && pday > februarycheck(pyear)) || pday > monthdays[pmonth]) {
                //alert("Input a valid day")
                return false
            }
            if (yearstr.length != 4 || pyear == 0 || pyear < dminyear || pyear > dmaxyear) {
                //alert("Input a valid 4 digit year between " + dminyear + " and " + dmaxyear)
                return false
            }
            if (strdate.indexOf(chsep, cpos2 + 1) != -1 || checkinteger(getcharacters(strdate, chsep)) == false) {
                //alert("Input a valid date")
                return false
            }
            return true
        }

        function IsValidDate(str1, str2) {
            var Day = parseInt(str1.substring(0, 2), 10);
            var Mn = parseInt(str1.substring(3, 5), 10);
            var Yr = parseInt(str1.substring(6, 10), 10);
            var DateVal = Mn + "/" + Day + "/" + Yr;
            var dt = new Date(DateVal);
            var Day1 = parseInt(str2.substring(0, 2), 10);
            var Mn1 = parseInt(str2.substring(3, 5), 10);
            var Yr1 = parseInt(str2.substring(6, 10), 10);
            var DateVal1 = Mn1 + "/" + Day1 + "/" + Yr1;
            var dt1 = new Date(DateVal1);

            if (dt.getDate() != Day) {
                alert('Invalid Date');
                return false;
            }
            else if (dt.getMonth() != Mn - 1) {
                //this is for the purpose JavaScript starts the month from 0
                alert('Invalid Date');
                return false;
            }
            else if (dt.getFullYear() != Yr) {
                alert('Invalid Date');
                return false;
            }

            if (dt1.getDate() != Day1) {
                alert('Invalid Date');
                return false;
            }
            else if (dt1.getMonth() != Mn1 - 1) {
                //this is for the purpose JavaScript starts the month from 0
                alert('Invalid Date');
                return false;
            }
            else if (dt1.getFullYear() != Yr1) {
                alert('Invalid Date');
                return false;
            }
            // alert(dt); alert(dt1);
            if (dt >= dt1) {
                return false;
            }
            return true;
        }

        function LessThanToday(strDate) {
            var Day = parseInt(strDate.substring(0, 2), 10);
            var Mn = parseInt(strDate.substring(3, 5), 10);
            var Yr = parseInt(strDate.substring(6, 10), 10);
            var DateVal = Mn + "/" + Day + "/" + Yr;
            var dt = new Date(DateVal);

            var todaydate = new Date();
            //alert(todaydate);alert(dt);
            var i = DateDiff.inDays(dt, todaydate)
            if (i <= 0) {
                return false;
            }
            return true;
        }
    </script>

    <!-- Range Slider-->
    <script type="text/javascript">

        $(document).ready(function () {

            function repositionTooltip(e, ui) {
                var div = $(ui.handle).data("tooltip").$tip[0];
                var pos = $.extend({}, $(ui.handle).offset(), {
                    width: $(ui.handle).get(0).offsetWidth,
                    height: $(ui.handle).get(0).offsetHeight
                });

                var actualWidth = div.offsetWidth;

                tp = { left: pos.left + pos.width / 2 - actualWidth / 2 }
                $(div).offset(tp);

                $(div).find(".tooltip-inner").text(ui.value);
            }

            $("#unranged-value").slider({
                value: $("#HiddenMinimumInvesment").val(),
                min: 500,
                max: 10000,
                step: 500,
                range: "max",
                slide: repositionTooltip,
                stop: repositionTooltip,
                change: function (event, ui) {
                    var MinimumInvesment = $("#unranged-value").slider('value');
                    $("#HiddenMinimumInvesment").val(MinimumInvesment);
                }
            });
            $("#unranged-value .ui-slider-handle:first").tooltip({ title: $("#unranged-value").slider("value"), trigger: "manual" }).tooltip("show");
            $("#unranged-SIreturn").slider({
                value: $("#HiddenMinimumSIReturn").val(),
                min: 5,
                max: 50,
                step: 1,
                range: "max",
                slide: repositionTooltip,
                stop: repositionTooltip,
                change: function (event, ui) {
                    var MinimumInvesment = $("#unranged-SIreturn").slider('value');
                    $("#HiddenMinimumSIReturn").val(MinimumInvesment);
                }
            });
            $("#unranged-SIreturn .ui-slider-handle:first").tooltip({ title: $("#unranged-SIreturn").slider("value"), trigger: "manual" }).tooltip("show");
        });

    </script>

    <style>
        label {
            margin-bottom: 0.5rem;
        }
        
        .tooltip {
            font-size: 10px;
        }

        .radio, .checkbox {
            display: inline-table;
        }

            .radio input[type="radio"], .radio-inline input[type="radio"],  .checkbox-inline input[type="checkbox"] {
                margin-left: 0px;
            }
    
        .multiselect-container {
            padding: 5px;
            height: 300px;
            overflow-y: scroll;
            overflow-x: hidden;
        }

        .checkbox {
            position: relative;
            display: block;
            margin-top: 1px;
            margin-bottom: 1px;
        }

        .btn-group-justified > .btn, .btn-group-justified > .btn-group {
            display: table-cell;
            float: none;
            width: 100%;
        }
        .dropdown-menu > .active > a, .dropdown-menu > .active > a:hover, .dropdown-menu > .active > a:focus {
            color: #fff;
            text-decoration: none;
            background-color: transparent;
            outline: 0;
        }
        .card-header {
            border-bottom: 1px solid rgba(0, 0, 0, 0.125);
        }
    </style>

</head>
<body>
    <form id="form1" runat="server">
        <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12">
            <div class="card">
                <div class="card-header">
                    <h6>Top Fund</h6>
                </div>
                <div class="card-body">
                    <div class="card">
                        <div class="card-body">
                            <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12 p-0">
                                <div class="row">
                                    <div class="form-group col-xs-12 col-sm-12 col-md-6 col-lg-6">
                                        <label>Category</label>
                                        <asp:DropDownList ID="ddlCategory" runat="server" OnSelectedIndexChanged="ddlCategory_SelectedIndexChanged"
                                            AutoPostBack="true" class="form-control form-control-sm">
                                        </asp:DropDownList>
                                    </div>
                                    <div class="form-group col-xs-12 col-sm-12 col-md-6 col-lg-6">
                                        <label>Sub-Category</label>
                                        <asp:DropDownList ID="ddlSubCategory" runat="server"
                                            class="form-control form-control-sm">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                            <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12 p-0">
                                <div class="row">
                                    <div class="form-group col-xs-12 col-sm-12 col-md-6 col-lg-6">
                                        <label>Type</label>
                                        <asp:DropDownList ID="ddlType" runat="server" class="form-control form-control-sm">
                                        </asp:DropDownList>
                                    </div>
                                    <div class="form-group col-xs-12 col-sm-12 col-md-6 col-lg-6">
                                        <label>Mutual Fund Name</label>
                                        <%--<select id="ddlFundHouse" class="form-control form-control-sm">
                                                            <option selected="selected" value=" Select AMC Name">Select
                                                                AMC Name</option>
                                                            <option value="3">Aditya Birla Sun Life Mutual Fund</option>
                                                            <option value="46">Axis Mutual Fund</option>
                                                            <option value="4">Baroda Mutual Fund</option>
                                                        </select>--%>

                                        <div>
                                            <asp:ListBox ID="ddlFundHouse" runat="server"
                                                class="form-control form-control-sm" SelectionMode="multiple"></asp:ListBox>
                                        </div>

                                    </div>

                                    <div class="form-group col-xs-12 col-sm-12 col-md-6 col-lg-6">
                                        <label>Option</label>

                                        <asp:RadioButtonList ID="rdbOption" runat="server"
                                            class="radio" RepeatDirection="Horizontal" Style="padding-top: 15px;
                                            text-align: left; font-size: 14px; font-weight: normal"
                                            Width="100%">
                                        </asp:RadioButtonList>
                                    </div>
                                    <div class="form-group col-xs-12 col-sm-12 col-md-6 col-lg-6">
                                        <label>Period</label>
                                        <asp:DropDownList ID="ddlPeriod" runat="server" class="form-control form-control-sm">
                                            <asp:ListItem Value="Per_7_Days" Text="Last 1 Week" />
                                            <asp:ListItem Value="Per_30_Days" Text="Last 1 Month" />
                                            <asp:ListItem Value="Per_91_Days" Text="Last 3 Months" />
                                            <asp:ListItem Value="Per_182_Days" Text="Last 6 Months" />
                                            <asp:ListItem Value="Per_1_Year" Text="Last 12 Months"
                                                Selected="True" />
                                            <asp:ListItem Value="Per_3_Year" Text="Last 3 Years" />
                                            <asp:ListItem Value="Per_5_Year" Text="Last 5 Years" />
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                            <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12 p-0">
                                <div class="row">
                                    <div class="form-group col-xs-12 col-sm-12 col-md-6 col-lg-6">
                                        <label>Rank</label>
                                        <asp:DropDownList ID="ddlRank" runat="server" class="form-control form-control-sm">
                                            <asp:ListItem Text="All" Value="1000" />
                                            <asp:ListItem Text="Top 5" Value="5" />
                                            <asp:ListItem Text="Top 10" Value="10" />
                                            <asp:ListItem Text="Top 15" Value="15" />
                                            <asp:ListItem Text="Top 20" Value="20" />
                                            <asp:ListItem Text="Top 25" Value="25" />
                                        </asp:DropDownList>
                                    </div>
                                    <div class="form-group col-xs-12 col-sm-12 col-md-6 col-lg-6">
                                        <%--<label>Minimum Investment</label>
                                                        <div id="unranged-value" class="ui-slider-grey" style="width: 95%; height: 10px; margin-left: 8px; margin-top: 18px; z-index: 1">
                                                        </div>
                                                        <div style="color: #989898; margin-bottom: 25px; margin-left: 0px; padding-top: 0px;">
                                                            ₹500
                                                        </div>

                                                        <div align="right" style="color: #989898; margin-top: -45px; margin-right: 10px;">
                                                           ₹10,000
                                                        </div>--%>

                                        <%--<asp:HiddenField ID="HiddenMinimumInvesment" runat="server"
                                                            Value="500" />--%>
                                    </div>
                                    <div class="form-group col-xs-12 col-sm-12 col-md-6 col-lg-6">
                                        <%--<label>Minimum SI Return(%)</label>
                                                        <div id="unranged-SIreturn" class="ui-slider-grey"
                                                            style="width: 95%; height: 10px; margin-left: 0px; margin-top: 18px; z-index: 1">
                                                        </div>

                                                        <div style="color: #989898; margin-bottom: 25px; margin-left: 0px; padding-top: 0px;">
                                                            5
                                                        </div>

                                                        <div align="right" style="color: #989898; margin-top: -45px; margin-right: 10px;">
                                                            50
                                                        </div>
                                                        <asp:HiddenField ID="HiddenMinimumSIReturn" runat="server"
                                                            Value="5" />--%>
                                        <asp:HiddenField ID="hdIsLoad" runat="server" Value="0" />
                                        <asp:HiddenField ID="Userid" runat="server" Value="asas" />
                                    </div>
                                </div>
                            </div>
                            <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12 p-0">
                                <div class="row">
                                    <div class="col-xs-12 col-sm-12 col-md-7 col-lg-7">
                                    </div>
                                    <div class="col-xs-12 col-sm-12 col-md-5 col-lg-5 text-right">

                                        <asp:Button ID="btnSubmit" class="btn btn-danger"
                                            Style="margin-right: 10px"
                                            runat="server" Text="Submit" OnClick="btnSubmit_Click" />
                                        <asp:Button ID="btnReset" class="btn btn-light" Style="margin-right: 15px;"
                                            runat="server" Text="Reset" OnClick="btnReset_Click" />
                                    </div>
                                </div>
                            </div>
                            <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12 p-0">
                                <div id="Result" class="block" runat="server">
                                    <asp:HiddenField ID="HiddenDivValue" runat="server" />
                                    <div>
                                        <asp:Label ID="lbtopText" runat="server" Text=""></asp:Label>
                                    </div>

                                    <div>
                                        <asp:ListView ID="lstResult" runat="server" OnPagePropertiesChanging="lst_PagePropertiesChanging">
                                            <LayoutTemplate>
                                                <div class="table-responsive">
                                                <table width="100%" border="0" cellpadding="0" cellspacing="0" class="table table-bordered">
                                                    <thead>
                                                        <tr>

                                                            <th align="left" rowspan="2">
                                                                <asp:Label ID="lnkSchName" runat="server" SkinID="lblHeader"
                                                                    CommandName="Sort" CommandArgument="SchName" Text="Scheme Name" />
                                                            </th>

                                                            <%--<th align="center">
                                                <asp:Label ID="Label1" runat="server" SkinID="lblHeader" CommandName="Sort" CommandArgument="ICRON_Rank" Text="ICRON Rank" />
                                            </th>--%>

                                                            <%--<th align="left">
                                                    <asp:Label ID="lnkDate" runat="server" SkinID="lblHeader" CommandName="Sort" CommandArgument="Date" Text="Date" />      
                                                </th>--%>


                                                            <th align="center" rowspan="2" style="text-align: center">
                                                                <asp:Label ID="Label1" runat="server" SkinID="lblHeader"
                                                                    CommandName="Sort" CommandArgument="Fund_Size"
                                                                    Text="AUM (in ₹ Cr.)" />
                                                            </th>
                                                            <th colspan="8" style="text-align: center; padding-top: 0px; padding-bottom: 0px">Return (%) </th>
                                                        </tr>
                                                        <tr style="height: 25px;">
                                                            <th id="1Week" style="text-align: center;">1W</th>
                                                            <th id="1Month" style="text-align: center;">1M</th>
                                                            <th id="3Month" style="text-align: center;">3M</th>
                                                            <th id="6Month" style="text-align: center;">6M</th>
                                                            <th id="1Year" style="text-align: center;">1Y</th>
                                                            <th id="3Year" style="text-align: center;">3Y</th>
                                                            <th id="5Year" style="text-align: center;">5Y</th>
                                                            <th id="SI" style="text-align: center;">SI</th>
                                                        </tr>
                                                    </thead>
                                                    <tbody>

                                                        <tr id="itemPlaceholder" runat="server"></tr>
                                                    </tbody>

                                                </table>
                                                </div>
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

                                                    </table>
                                                </div>


                                            </LayoutTemplate>
                                            <ItemTemplate>
                                                <tr class='<%# Convert.ToBoolean(Container.DataItemIndex % 2) ? "value_tablerow" : "value_tablerow" %>'>
                                                    <%--<td>
                                                                        <asp:Label runat="server" ID="lblType"><%#Eval("Rank") %></asp:Label>
                                                                    </td>--%>
                                                    <td style="width: 35%;">                                                        
                                                        <a href="/prithvivallabh/Factsheet.aspx?param=<%#Eval("SchemeId")%>" target="_blank"><%#Eval("Sch_Name")%></a>
                                                        <%-- <asp:Label runat="server" ID="lblSchName" Text='<%#Eval("Sch_Name")%>' />--%>
                                                    </td>
                                                    <td align="center">
                                                        <%--<%# Convert.ToDouble(Eval("Fund_Size")).ToString("n2") %>--%>
                                                        <%#Eval("Fund_Size").ToString() != "" ? Eval("Fund_Size", "{0:0.00}") : "--"%>
                                                    </td>
                                                    <td align="center">
                                                        <%--<asp:Label runat="server" ID="lblNature" Text='<%#Eval("Nature")%>' />--%>

                                                        <%#Eval("Per_7_Days").ToString() != "" ? Eval("Per_7_Days", "{0:0.00}") : "--"%>

                                                    </td>
                                                    <td align="center">
                                                        <%--<asp:Label runat="server" ID="lblSubNature" Text='<%#Eval("SubNature")%>' />--%>

                                                        <%#Eval("Per_30_Days").ToString() != "" ? Eval("Per_30_Days", "{0:0.00}") : "--"%>
                                                    </td>
                                                    <td align="center">
                                                        <%--<asp:Label runat="server" ID="lblSubNature" Text='<%#Eval("SubNature")%>' />--%>

                                                        <%#Eval("Per_91_Days").ToString() != "" ? Eval("Per_91_Days", "{0:0.00}") : "--"%>
                                                    </td>
                                                    <td align="center">
                                                        <%--<asp:Label runat="server" ID="lblSubNature" Text='<%#Eval("SubNature")%>' />--%>

                                                        <%#Eval("Per_182_Days").ToString() != "" ? Eval("Per_182_Days", "{0:0.00}") : "--"%>
                                                    </td>
                                                    <td align="center">
                                                        <%--<asp:Label runat="server" ID="lblSubNature" Text='<%#Eval("SubNature")%>' />--%>

                                                        <%#Eval("Per_1_Year").ToString() != "" ? Eval("Per_1_Year", "{0:0.00}") : "--"%>
                                                    </td>
                                                    <td align="center">
                                                        <%--<asp:Label runat="server" ID="lblSubNature" Text='<%#Eval("SubNature")%>' />--%>

                                                        <%#Eval("Per_3_Year").ToString() != "" ? Eval("Per_3_Year", "{0:0.00}") : "--"%>
                                                    </td>
                                                    <td align="center">
                                                        <%--<asp:Label runat="server" ID="lblSubNature" Text='<%#Eval("SubNature")%>' />--%>

                                                        <%#Eval("Per_5_Year").ToString() != "" ? Eval("Per_5_Year", "{0:0.00}") : "--"%>
                                                    </td>
                                                    <td align="center">
                                                        <%--<asp:Label runat="server" ID="lblSubNature" Text='<%#Eval("SubNature")%>' />--%>

                                                        <%#Eval("Since_Inception").ToString() != "" ? Eval("Since_Inception", "{0:0.00}") : "--"%>
                                                    </td>

                                                    
                                                </tr>
                                            </ItemTemplate>
                                            <EmptyDataTemplate>
                                                Data not Found
                                            </EmptyDataTemplate>
                                        </asp:ListView>
                                    </div>
                                    <div class="pt-5">
                                        <div style="font-size: 10px; color: #A7A7A7">
                                            Note: Returns less than or equal to 1 year are absolute
                                                            return and returns more than 1 year are compound annualized.
                                        </div>
                                        <div style="font-size: 10px; color: #A7A7A7">
                                            Developed by: <a href="https://www.icraanalytics.com"
                                                target="_blank" style="font-size: 10px; color: #999999">ICRA Analytics Ltd</a>, <a style="font-size: 10px; color: #999999" href="https://icraanalytics.com/home/Disclaimer" target="_blank">Disclaimer </a>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
