<%@ Page Language="C#" AutoEventWireup="true" ViewStateEncryptionMode="Always" EnableViewStateMac="true" CodeBehind="TopFund.aspx.cs" Inherits="iFrames.BansalCapital.TopFund" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html class="no-js">

<head>
    <title>Top fund</title>

    <%--  <script type='text/javascript' src="../DSP/js/jquery-1.8.3.js"></script>
        <script type="text/javascript" src="../DSP/js/jquery-ui.js"></script>
        <script type='text/javascript' src="../DSP/js/bootstrap.js"></script>--%>

    <script type='text/javascript' src="js/jquery.js"></script>
    <script type="text/javascript" src="js/jquery-ui.js"></script>
    <script type='text/javascript' src="js/bootstrap.js"></script>
    
    <script src="js/AutoComplete.js" type="text/javascript"></script>

    <%--<link href="../DSP/css/bootstrap.css" rel="stylesheet" />
        <link href="../DSP/css/bootstrap.min.css" rel="stylesheet" />
        <link rel="stylesheet" href="../DSP/css/jquery-ui-1.10.3.custom.min.css" />
        <link rel="stylesheet" href="../DSP/css/jquery-slider.css" />--%>

    <%--<link href="css/bootstrap.css" rel="stylesheet" />--%>
    <%--<link href="css/bootstrap.min.css" rel="stylesheet" />--%>
    <link rel="stylesheet" href="css/jquery-ui-1.10.3.custom.min.css" />
    <link rel="stylesheet" href="css/jquery-slider.css" />
    <link href="css/auto.css" rel="stylesheet" type="text/css" />
    <link href="bootstrap/css/bootstrap.min.css" rel="stylesheet" media="screen" />
    <link href="bootstrap/css/bootstrap-responsive.min.css" rel="stylesheet" media="screen" />
    <%-- <link href="bootstrap/css/styles.css" rel="stylesheet" media="screen" /> 
        <link href="bootstrap/css/DT_bootstrap.css" rel="stylesheet" media="screen" />--%>
    <link href="bootstrap/css/styles.css" rel="stylesheet" media="screen" />
    <link href='http://fonts.googleapis.com/css?family=Open+Sans:400,600,700,300' rel='stylesheet' type='text/css' />
    <link href='http://fonts.googleapis.com/css?family=Open+Sans:400,600,700,300' rel='stylesheet' type='text/css' />
    <%--<link rel="stylesheet" href="http://code.jquery.com/ui/1.11.2/themes/smoothness/jquery-ui.css" />--%>
    <script src="js/modernizr2.js" type="text/javascript"></script>   
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
    <script type="text/javascript">        '<%#Eval("SchemeId")%>', '<%#Eval("Sch_Name")%>', '<%#Eval("OptionId")%>', '<%#Eval("Nature")%>', '<%#Eval("SubNature")%>'
        function callCross(schid, schname, OptionId, Nature, SubNature)
        {
            if (OptionId == "2")
                var option = "Growth";
            else
                var option = "Devidend";
            var data = { 'url': 'http://www.askmefund.com/transaction.aspx?Scheme_Name=' + schname + ',Option=' + option + ',SchemeId=' + schid + ',Category=' + Nature + ',Sub_Category=' + SubNature };
            top.postMessage(data, 'http://www.askmefund.com/top-performing-mutual-funds.aspx');
        }
        function Menuclick(url) {
            var data = { 'url': url };
            top.postMessage(data, 'http://www.askmefund.com/top-performing-mutual-funds.aspx');
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
            //                        //window.location("javascript:parent.window.location.href='http://localhost:52348/login.aspx'");
            //                        var data = { 'url': 'http://www.askmefund.com/login.aspx' };
            //                        top.postMessage(data, 'http://www.askmefund.com/top-performing-mutual-funds.aspx');
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
                    url: 'BansalProxy.aspx/Add2Watchlist',
                    type: 'POST',
                    contentType: "application/json; charset=utf-8",
                    success: function (dataConsolidated) {
                        var obj = jQuery.parseJSON(dataConsolidated.d);
                        if (obj.d == 1) {
                            alert("Scheme added to your watch list");
                        }
                        else if (obj.d == 2) {
                            alert("You cannot add more than 10 scheme in watch list");
                        }
                        else if (obj.d == 3) {
                            alert("Scheme already exist in your watchlist");
                        }
                        else if (obj.d == 4) {
                            //window.location("javascript:parent.window.location.href='http://localhost:52348/login.aspx'");
                            var data = { 'url': 'http://www.askmefund.com/login.aspx' };
                            top.postMessage(data, 'http://www.askmefund.com/top-performing-mutual-funds.aspx');
                        }
                        else if (obj.d == 0) {
                            alert("Some Error occured. Please contact askmefund.com");
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

    <script type="text/javascript">
       // alert($("#txtSearch").val());
        $("#<%=txtSearch.ClientID%>").autocomplete({
            source: function (request, response) {
                debugger;
                $.ajax({
                    type: "POST",
                    url: "SchemeSearch.aspx/GetCustomers",
                    async: false,
                    contentType: "application/json",
                    data: "{ 'prefix': '" + request.term + "'}",
                    dataType: "json",
                    success: function (data) {
                        debugger;
                        response($.map(data.d, function (item) {
                            return {
                                label: item.split('#')[0],
                                val: item.split('#')[1]
                            }
                        }))
                    },
                    error: function (data) {
                        alert("Error! Try again...");
                    }
                });
            }
        });
         

       
    </script>


    <%--Region: Value stores in HiddenField from Web.Config for FundRisk Button and fixed background-color when button is clicked--%>
    <script type="text/javascript">

        var FundRiskValue;

        $(document).ready(function () {

           var cssclass = "btn_nowrap btn-" + $("#HiddenFundRiskStrColor").val() + " active";

            $("#" + $("#HiddenFundRiskStrColor").val().replace("-", "_")).attr("class", cssclass);

             if (FundRiskValue == null) {
                 $("#low").click(function (Func1) {
                    // debugger;
                    FundRiskValue = $("#low").attr("data-risk");
                    $("#HiddenFundRisk").val(FundRiskValue);
                    $("#low").attr("class", "btn-low btn_nowrap active");
                    $("#mod_low").attr("class", "btn-mod-low btn_nowrap");
                    $("#mod").attr("class", "btn-mod btn_nowrap");
                    $("#mod_high").attr("class", "btn-mod-high btn_nowrap");
                    $("#high").attr("class", "btn-high btn_nowrap");
                    $("#all").attr("class", "btn-all btn_nowrap");
                    $("#HiddenFundRiskStrColor").val("low");
                });

                $("#mod_low").click(function (Func2) {
                    FundRiskValue = $("#mod_low").attr("data-risk");
                    $("#HiddenFundRisk").val(FundRiskValue);
                    $("#low").attr("class", "btn-low btn_nowrap");
                    $("#mod_low").attr("class", "btn-mod-low btn_nowrap active");
                    $("#mod").attr("class", "btn-mod btn_nowrap");
                    $("#mod_high").attr("class", "btn-mod-high btn_nowrap");
                    $("#high").attr("class", "btn-high btn_nowrap");
                    $("#all").attr("class", "btn-all btn_nowrap");
                    $("#HiddenFundRiskStrColor").val("mod-low");
                });

                $("#mod").click(function (Func2) {
                    FundRiskValue = $("#mod").attr("data-risk");
                    $("#HiddenFundRisk").val(FundRiskValue);
                    $("#low").attr("class", "btn-low btn_nowrap");
                    $("#mod_low").attr("class", "btn-mod-low btn_nowrap");
                    $("#mod").attr("class", "btn-mod btn_nowrap active");
                    $("#mod_high").attr("class", "btn-mod-high btn_nowrap");
                    $("#high").attr("class", "btn-high btn_nowrap");
                    $("#all").attr("class", "btn-all btn_nowrap");
                    $("#HiddenFundRiskStrColor").val("mod");
                });

                $("#mod_high").click(function (Func2) {
                    FundRiskValue = $("#mod_high").attr("data-risk");
                    $("#HiddenFundRisk").val(FundRiskValue);
                    $("#low").attr("class", "btn-low btn_nowrap");
                    $("#mod_low").attr("class", "btn-mod-low btn_nowrap");
                    $("#mod").attr("class", "btn-mod btn_nowrap");
                    $("#mod_high").attr("class", "btn-mod-high btn_nowrap active");
                    $("#high").attr("class", "btn-high btn_nowrap");
                    $("#all").attr("class", "btn-all btn_nowrap");
                    $("#HiddenFundRiskStrColor").val("mod-high");
                });

                $("#high").click(function (Func3) {
                    FundRiskValue = $("#high").attr("data-risk");
                    $("#HiddenFundRisk").val(FundRiskValue);
                    $("#low").attr("class", "btn-low btn_nowrap");
                    $("#mod_low").attr("class", "btn-mod-low btn_nowrap");
                    $("#mod").attr("class", "btn-mod btn_nowrap ");
                    $("#mod_high").attr("class", "btn-mod-high btn_nowrap");
                    $("#high").attr("class", "btn-high btn_nowrap active");
                    $("#all").attr("class", "btn-all btn_nowrap");
                    $("#HiddenFundRiskStrColor").val("high");
                });

                $("#all").click(function (Func4) {
                    FundRiskValue = $("#all").attr("for");
                    $("#HiddenFundRisk").val(FundRiskValue);
                    $("#low").attr("class", "btn-low btn_nowrap");
                    $("#mod_low").attr("class", "btn-mod-low btn_nowrap");
                    $("#mod").attr("class", "btn-mod btn_nowrap ");
                    $("#mod_high").attr("class", "btn-mod-high btn_nowrap");
                    $("#high").attr("class", "btn-high btn_nowrap ");
                    $("#all").attr("class", "btn-all btn_nowrap active");
                    $("#HiddenFundRiskStrColor").val("all");
                });
             }
        });
            
    </script>

    <%--Range Slider--%>
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

    <%--Performance Rating--%>
   <%-- <script type="text/javascript">

        $(document).ready(function () {

            $("#1").click(function () {
                if ($("#HiddenFieldRating1").val() == "0") {
                    $(this).FuncRating1();
                }
                else if ($("#HiddenFieldRating1").val() == "1") {
                    $("#1").attr("src", "images/icon_star_empty.gif");
                    $("#HiddenFieldRating1").val("0");
                }
            });

            $.fn.FuncRating1 = function () {
                $("#1").attr("src", "images/icon_star_filled1.gif");
                $("#HiddenFieldRating1").val("1");
            }

            $("#2").click(function () {
                if ($("#HiddenFieldRating2").val() == "0") {
                    $(this).FuncRating2();
                }
                else if ($("#HiddenFieldRating2").val() == "2") {
                    $("#2").attr("src", "images/icon_star_empty.gif");
                    $("#HiddenFieldRating2").val("0");
                }
            });

            $.fn.FuncRating2 = function () {
                $("#2").attr("src", "images/icon_star_filled1.gif");
                $("#HiddenFieldRating2").val("2");
            }

            $("#3").click(function () {
                if ($("#HiddenFieldRating3").val() == "0") {
                    $(this).FuncRating3();
                }
                else if ($("#HiddenFieldRating3").val() == "3") {
                    $("#3").attr("src", "images/icon_star_empty.gif");
                    $("#HiddenFieldRating3").val("0");
                }
            });

            $.fn.FuncRating3 = function () {
                $("#3").attr("src", "images/icon_star_filled1.gif");
                $("#HiddenFieldRating3").val("3");
            }

            $("#4").click(function () {
                if ($("#HiddenFieldRating4").val() == "0") {
                    $(this).FuncRating4();
                }
                else if ($("#HiddenFieldRating4").val() == "4") {
                    $("#4").attr("src", "images/icon_star_empty.gif");
                    $("#HiddenFieldRating4").val("0");
                }
            });

            $.fn.FuncRating4 = function () {
                $("#4").attr("src", "images/icon_star_filled1.gif");
                $("#HiddenFieldRating4").val("4");
            }

            $("#5").click(function () {
                if ($("#HiddenFieldRating5").val() == "0") {
                    $(this).FuncRating5();
                }
                else if ($("#HiddenFieldRating5").val() == "5") {
                    $("#5").attr("src", "images/icon_star_empty.gif");
                    $("#HiddenFieldRating5").val("0");
                }
            });

            $.fn.FuncRating5 = function () {
                $("#5").attr("src", "images/icon_star_filled1.gif");
                $("#HiddenFieldRating5").val("5");
            }

            var RatingValue1, RatingValue2, RatingValue3, RatingValue4, RatingValue5;

            RatingValue1 = "<%= HiddenFieldRating1.Value %>"
                 RatingValue2 = "<%= HiddenFieldRating2.Value %>"
                 RatingValue3 = "<%= HiddenFieldRating3.Value %>"
                 RatingValue4 = "<%= HiddenFieldRating4.Value %>"
                 RatingValue5 = "<%= HiddenFieldRating5.Value %>"

                 if (RatingValue1 == "0") {
                     $("#1").attr("src", "images/icon_star_empty.gif");
                     $("#HiddenFieldRating1").val("0");
                 }
                 if (RatingValue2 == "0") {
                     $("#2").attr("src", "images/icon_star_empty.gif");
                     $("#HiddenFieldRating2").val("0");
                 }
                 if (RatingValue3 == "0") {
                     $("#3").attr("src", "images/icon_star_empty.gif");
                     $("#HiddenFieldRating3").val("0");
                 }
                 if (RatingValue4 == "0") {
                     $("#4").attr("src", "images/icon_star_empty.gif");
                     $("#HiddenFieldRating4").val("0");
                 }
                 if (RatingValue5 == "0") {
                     $("#5").attr("src", "images/icon_star_empty.gif");
                     $("#HiddenFieldRating5").val("0");
                 }
             });

    </script>--%>
    <!--[if !IE]><!-->
    <script type="text/javascript">
        if (/*@cc_on!@*/false) {
            document.documentElement.className += ' ie10';
        }
    </script>
    <!--<![endif]-->
</head>

<body>
    <div style="width: 960px">
        <form id="topform1" runat="server">
            <div class="navbar" align="center" style="width: 953px; margin-left: 7px; margin-top: -55px;">
                <div class="navbar-inner">
                    <div class="container-fluid">
                        <div class="brand">Mutual Fund Analyzer</div>
                       <div style="float: right;margin-top: 8px;position: relative;left: 20px;">
                            <img src="images/search.png" style="margin-top: 3px;" /><asp:TextBox ID="txtSearch" runat="server" Width="275px" placeholder="Type your text here"></asp:TextBox>
                            <asp:HiddenField ID="hfCustomerId" runat="server" />

                        </div>
                        <!--/.nav-collapse -->
                    </div>
                </div>
            </div>

            <div class="container-fluid" style="padding: 0; padding-left: 18px; padding-right: 10px;">
                <div class="row-fluid">
                    <div class="span3" id="sidebar">
<ul class="nav nav-list bs-docs-sidenav nav-collapse collapse" style="margin-left: -10px; padding-top: 0;">
<li class="active"><a href='http://www.askmefund.com/top-performing-mutual-funds.aspx' target="_blank">Top Funds</a></li>
<li ><a href='http://www.askmefund.com/compare-mutual-funds.aspx'  target="_blank">Compare Funds                     </a></li>
<li><a href='http://www.askmefund.com/recommended-mutual-fund.aspx'  target="_blank">Recommended Funds                               </a></li>
<li><a href='http://www.askmefund.com/recommended-new-fund-offer.aspx'  target="_blank">Recommended NFO                               </a></li>
<li><a href='http://www.askmefund.com/mutual-fund-nav-graph.aspx'  target="_blank">Funds NAV Graph                                </a></li>
<li><a href= 'http://www.askmefund.com/schemesearch.aspx'  target="_blank">Scheme Search</a></li>
<li><a href="http://www.askmefund.com/mfwatchlist.aspx" target="_blank">MF Watch List                         </a></li>
</ul>
                    </div>
                    <div class="span9" id="content">

                        <div class="row-fluid" style="margin-top: 0px; margin-left: 25px;">
                            <div class="span6">

                                <!-- block -->
                                <div class="block1">
                                    <div>
                                        <%--<div class="muted pull-left" style="color:#cc0000; font-weight:700;">Top Funds</div>
                                    
                                    <div class="pull-right"></div>--%>
                                    </div>

                                    <div class="block-content collapse in">

                                        <div class="controls">
                                            <p class="span5 lebel-drop">Category</p>

                                            <asp:DropDownList ID="ddlCategory" runat="server" OnSelectedIndexChanged="ddlCategory_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                        </div>

                                        <div class="controls">
                                            <p class="span5 lebel-drop">Sub-Category</p>

                                            <asp:DropDownList ID="ddlSubCategory" runat="server"></asp:DropDownList>
                                        </div>

                                        <div class="controls">
                                            <p class="span5 lebel-drop">Type</p>

                                            <asp:DropDownList ID="ddlType" runat="server"></asp:DropDownList>
                                        </div>
                                        <div class="controls" style="margin-bottom: 10px;">
                                            <div class="span5 lebel-drop">Option</div>
                                            <div class="style-radio2">
                                                <asp:RadioButtonList ID="rdbOption" runat="server" class="radio" RepeatDirection="Horizontal" Style="margin-left: 5px; padding-top: 5px;"></asp:RadioButtonList></div>

                                        </div>
                                        <div class="controls">
                                            <p class="span5 lebel-drop">Period</p>

                                            <asp:DropDownList ID="ddlPeriod" runat="server">
                                                <asp:ListItem Value="Per_7_Days" Text="Last 1 Week" />
                                                <asp:ListItem Value="Per_30_Days" Text="Last 1 Month" />
                                                <asp:ListItem Value="Per_91_Days" Text="Last 3 Months" />
                                                <asp:ListItem Value="Per_182_Days" Text="Last 6 Months" />
                                                <asp:ListItem Value="Per_1_Year" Text="Last 12 Months" Selected="True" />
                                                <asp:ListItem Value="Per_3_Year" Text="Last 3 Years" />
                                                <asp:ListItem Value="Per_5_Year" Text="Last 5 Years" />
                                            </asp:DropDownList>
                                        </div>

                                        <div class="controls">
                                            <p class="span5 lebel-drop">Rank</p>

                                            <asp:DropDownList ID="ddlRank" runat="server">
                                                <asp:ListItem Text="All" Value="1000" />
                                                <asp:ListItem Text="Top 5" Value="5" />
                                                <asp:ListItem Text="Top 10" Value="10" />
                                                <asp:ListItem Text="Top 15" Value="15" />
                                                <asp:ListItem Text="Top 20" Value="20" />
                                                <asp:ListItem Text="Top 25" Value="25" />
                                            </asp:DropDownList>
                                        </div>

                                    </div>
                                </div>

                                <asp:Button ID="btnReset" class="btn-sub" style="margin-right: 15px;" runat="server" Text="Reset" OnClick="btnReset_Click" />&nbsp;&nbsp;
                                <asp:Button ID="btnSubmit" class="btn-sub" Style="margin-right: 10px" runat="server" Text="Submit" OnClick="btnSubmit_Click" />&nbsp&nbsp;
                                
                            <!-- /block -->
                            </div>

                            <div class="span6" style="margin-right: -20px;">
                                <!-- block -->
                                <div class="block1">


                                    <div class="block-content collapse1 in">
                                        <div style="margin-bottom: 25px;">
                                            <h6 style="margin-top: -5px; padding-bottom: 10px; margin-bottom: 15px;">Minimum Investment </h6>
                                        </div>

                                        <div id="unranged-value" class="ui-slider-grey" style="width: 95%; height: 10px; margin-left: 8px; margin-top: 52px; z-index:1"></div>

                                        <div style="color: #989898; margin-bottom: 25px; margin-left: 12px; padding-top: 8px;">
                                            <img src="images/rupee.png" width="12" height="16" alt="Rs." />500
                                        </div>

                                        <div align="right" style="color: #989898; margin-top: -45px; margin-right: 10px;">
                                            <img src="images/rupee.png" width="12" height="16" alt="Rs." />10,000
                                        </div>
                                    </div>

                                    <asp:HiddenField ID="HiddenMinimumInvesment" runat="server" Value="500" />

                                    <div class="block-content collapse in" style="margin-top: 13px;">
                                        
                                        <%--<div style="width: 45%; float: left;">
                                            <h6>Perfomance Ranking :</h6>
                                        </div>--%>
                                        <div style="margin-bottom: 25px;">
                                            <h6 style="margin-top: -5px; padding-bottom: 10px; margin-bottom: 15px;">Minimum SI Return(%) :</h6>
                                        </div>

                                        <div id="unranged-SIreturn" class="ui-slider-grey" style="width: 95%; height: 10px; margin-left: 8px; margin-top: 45px; z-index:1"></div>

                                        <div style="color: #989898; margin-bottom: 25px; margin-left: 12px; padding-top: 8px;">
                                            5
                                        </div>

                                        <div align="right" style="color: #989898; margin-top: -45px; margin-right: 10px;">
                                            50
                                        </div>
                                        <asp:HiddenField ID="HiddenMinimumSIReturn" runat="server" Value="5" />
                                        <asp:HiddenField ID="hdIsLoad" runat="server" Value="0" />
                                            <asp:HiddenField ID="Userid" runat="server" Value="asas" />
                                        <%--<div class="span12" align="left" style="width: 40%; margin-top:10px; padding-bottom:50px;">--%>

                                            <%--<img id="1" src="images/icon_star_filled1.gif" title="1" style="cursor: pointer;" alt="Rating star" />
                                            <img id="2" src="images/icon_star_filled1.gif" title="2" style="cursor: pointer;" alt="Rating star" />
                                            <img id="3" src="images/icon_star_filled1.gif" title="3" style="cursor: pointer;" alt="Rating star" />
                                            <img id="4" src="images/icon_star_filled1.gif" title="4" style="cursor: pointer;" alt="Rating star" />
                                            <img id="5" src="images/icon_star_filled1.gif" title="5" style="cursor: pointer;" alt="Rating star" /><br />

                                            <img src="images/value.png" alt="StarRanking" />--%>


                                            <%--<asp:HiddenField ID="HiddenFieldRating1" runat="server" Value="1" />
                                            <asp:HiddenField ID="HiddenFieldRating2" runat="server" Value="2" />
                                            <asp:HiddenField ID="HiddenFieldRating3" runat="server" Value="3" />
                                            <asp:HiddenField ID="HiddenFieldRating4" runat="server" Value="4" />
                                            <asp:HiddenField ID="HiddenFieldRating5" runat="server" Value="5" />--%>
                                            

                                       <%-- </div>--%>

                                    </div>

                                    <div class="block-content collapse in" style="margin-top: 0px; margin-right: 5px; padding-bottom:16px;">
                                        <%--<img src="images/line.jpg" />--%>
                                        <div>
                                            <h6 style="margin-top: -4px;">Fund Risk :</h6>
                                        </div>

                                        <div class="span12" style="margin:0 1px">
                                            <div id="low" runat="server" title="Low Riskometer" data-risk="1,6" class="btn-low btn_nowrap" for="1" style="width:28px;">Low</div>
                                            <div id="mod_low" runat="server" title="Moderately Low Riskometer" data-risk="2" class="btn-mod-low btn_nowrap" for="2">Mod Low</div>
                                            <div id="mod" runat="server" title="Moderate Riskometer" class="btn-mod btn_nowrap" data-risk="3" for="3" style="width:30px;">Mod</div>
                                            <div id="mod_high" runat="server" title="Moderately High Riskometer" class="btn-mod-high btn_nowrap" data-risk="4" for="4">Mod High</div>
                                            <div id="high" runat="server" title="High Riskometer" class="btn-high btn_nowrap" data-risk="5,10" for="5">High</div>
                                            <div id="all" runat="server" title="All" class="btn-all btn_nowrap" for="-1" style="width:28px;">All</div>
                                            <asp:HiddenField ID="HiddenFundRisk" runat="server" Value="-1" />
                                            <asp:HiddenField ID="HiddenFundRiskStrColor" runat="server" Value="All" />
                                        </div>
                                    </div>

                                    <!-- /block -->
                                   <%-- <img src="images/line.jpg" />--%>
                                    

                                </div>
                                <!-- /block -->
                            </div>

                        </div>

                    </div>


                </div>
                <div style="width: 960px;">
                    <!-- block -->
                    <div id="Result" class="block" style="margin-top: 20px; margin-right: 25px; margin-left: -10px;" runat="server">
                        <asp:HiddenField ID="HiddenDivValue" runat="server" />
                        <div class="navbar navbar-inner block-header">
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
                                                <asp:Label ID="lnkRank" runat="server" SkinID="lblHeader" CommandName="Sort" CommandArgument="Rank" Text="Sl. No." />
                                            </th>

                                            <th align="left">
                                                <asp:Label ID="lnkSchName" runat="server" SkinID="lblHeader" CommandName="Sort" CommandArgument="SchName" Text="Scheme Name" />
                                            </th>

                                            <th align="center">
                                                <asp:Label ID="lnkNature" runat="server" SkinID="lblHeader" CommandName="Sort" CommandArgument="Nature" Text="Category" />
                                            </th>

                                            <th align="center">
                                                <asp:Label ID="lnkSubnature" runat="server" SkinID="lblHeader" CommandName="Sort" CommandArgument="Subnature" Text="Sub Category" />
                                            </th>
                                            
                                            <%--<th align="center">
                                                <asp:Label ID="Label1" runat="server" SkinID="lblHeader" CommandName="Sort" CommandArgument="ICRON_Rank" Text="ICRON Rank" />
                                            </th>--%>

                                            <%--<th align="left">
                                                    <asp:Label ID="lnkDate" runat="server" SkinID="lblHeader" CommandName="Sort" CommandArgument="Date" Text="Date" />      
                                                </th>--%>

                                            <th align="center">
                                                <asp:Label ID="lnkNav" runat="server" SkinID="lblHeader" CommandName="Sort" CommandArgument="NAV" Text="NAV" />
                                            </th>

                                            <th align="center">
                                                <asp:Label ID="lnkPeriod" runat="server" SkinID="lblHeader" CommandName="Sort" CommandArgument="<%=ddlPeriod.SelectedValue%>"><%=ddlPeriod.SelectedItem.Text%></asp:Label>
                                            </th>

                                            <th align="center">
                                                <asp:Label ID="lnkInception" runat="server" SkinID="lblHeader" CommandName="Sort" CommandArgument="Inception" Text="Since Inception" />
                                            </th>
                                             <th align="center">
                                                <asp:Label ID="Label1" runat="server" SkinID="lblHeader" CommandName="Sort" CommandArgument="Fund_Size" Text="Fund Size(Cr)" />
                                            </th>
                                            <th align="center">
                                                <asp:Label ID="lblWatchList" runat="server" SkinID="lblHeader" CommandName="Sort" CommandArgument="WatchList" Text="Watch List"></asp:Label>
                                            </th>
                                            <th align="center">
                                                <asp:Label ID="Label3" runat="server" SkinID="lblHeader" CommandName="Sort" CommandArgument="InvestNow" Text="Invest Now" Style="text-align: center"></asp:Label>
                                            </th>
                                        </tr>
                                        <tr id="itemPlaceholder" runat="server">
                                        </tr>
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
                                                <div class="value_input" style="width: 60%; float: left; font-size: 10px; color: #A7A7A7">Note : Returns less than or equal to 1 year are absolute return and returns more than 1 year are compound annualized.</div>
                                                <div class="value_input" style="width: 30%; float: right; text-align: right; font-size: 10px; color: #A7A7A7">
                                                   Developed for Askmefund by: <a href="https://www.icraanalytics.com" target="_blank" style="font-size: 10px; color: #999999">ICRA Analytics Ltd</a>, <a style="font-size: 10px; color: #999999" href="https://icraanalytics.com/home/Disclaimer"
                                                                                                            target="_blank">Disclaimer </a>
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
                                            <a href="http://www.askmefund.com/factsheet.aspx?param=<%#Eval("SchemeId")%>" target="_blank"><%#Eval("Sch_Name")%></a>
                                            <%-- <asp:Label runat="server" ID="lblSchName" Text='<%#Eval("Sch_Name")%>' />--%>
                                        </td>
                                        <td align="center">
                                            <asp:Label runat="server" ID="lblNature" Text='<%#Eval("Nature")%>' />
                                        </td>
                                        <td align="center">
                                            <asp:Label runat="server" ID="lblSubNature" Text='<%#Eval("SubNature")%>' />
                                        </td>
                                        
                                        <%--<td align="center" style="width: 75px;">
                                            <asp:Label runat="server" ID="Label2" Text='<%#Eval("MF_Rank_Html")%>' />
                                        </td>--%>
                                        <%--<td>
                                                                                <asp:Label runat="server" ID="lblDate" Text='<%#Convert.ToDateTime(Eval("Calculation_Date").ToString()).ToString("MMM dd, yyyy")%>' />
                                                                            </td>--%>
                                        <td align="center">
                                            <asp:Label runat="server" ID="lblNav" Text='<%#Eval("Nav")%>' />
                                        </td>
                                        <td align="center">
                                            <%# Convert.ToDouble(Eval(ddlPeriod.SelectedValue)).ToString("n2") %>
                                        </td>
                                        <td align="center">
                                            <%# Convert.ToDouble(Eval("Since_Inception")).ToString("n2")%>
                                        </td>
                                        <td align="center">
                                            <%# Convert.ToDouble(Eval("Fund_Size")).ToString("n2") %>
                                        </td>
                                        <td align="center">
                                            <img src="images/watch.jpg" style="cursor: pointer" alt="" name="imgAdd2Watch" id='<%#Eval("SchemeId")%>' />                                            
                                            
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
                    </div>
                    <!-- /block -->
                </div>
            </div>
            <!--  <hr>
                <footer>
                    <p>&copy; Vincent Gabriel 2013</p>
                </footer>-->


        </form>
    </div>
</body>

</html>
