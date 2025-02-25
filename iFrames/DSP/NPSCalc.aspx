<%@ Page Language="C#" AutoEventWireup="true" ViewStateEncryptionMode="Always" EnableViewStateMac="true" CodeBehind="NPSCalc.aspx.cs" Inherits="iFrames.DSP.NPSCalc" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<!DOCTYPE html PUBLIC "-//W3C//DTD HTML 4.01 Transitional//EN">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <%--  ie issue--%>
    <%--<meta http-equiv="X-UA-Compatible" content="IE=9" />--%>
    <%--<meta http-equiv="X-UA-Compatible" content="IE=8" />--%>
    <meta http-equiv="X-UA-Compatible" content="IE=EDGE" />
    <%--end ie issue--%>
    <title>NPS</title>
    <script type='text/javascript' src='js/jquery.js'></script>
    <script type="text/javascript" src="js/jquery-ui.js"></script>
    <script type='text/javascript' src="js/bootstrap.js"></script>
    <link href="css/bootstrap.css" rel="stylesheet" />
    <link href="css/bootstrap.min.css" rel="stylesheet" />
    <link href="css/font-awesome.min.css" rel="stylesheet" />
    <link rel="stylesheet" href="css/jquery-ui-1.10.3.custom.min.css" />
    <link rel="stylesheet" href="css/jquery-slider.css" />
    <link href="css/extend.css" rel="stylesheet" />
    <link href="css/master.css" rel="stylesheet" />
    <link href="css/nps.css" rel="stylesheet" />
    <link href="css/dspstyles.css" rel="stylesheet" />
    <script src="http://cdn.webrupee.com/js" type="text/javascript"></script>
    <script src="../Scripts/jqplot/jquery.jqplot.min.js" type="text/javascript"></script>
    <script src="../Scripts/jqplot/plugins/jqplot.cursor.min.js" type="text/javascript"></script>
    <script src="~/Scripts/jqplot/plugins/jqplot.pointLabels.min.js" type="text/javascript"></script>
    <script src="../Scripts/jqplot/plugins/jqplot.pieRenderer.min.js" type="text/javascript"></script>
    <link href="../Styles/jquery.jqplot.css" rel="stylesheet" type="text/css" />
    <script src="Script/jquery.base64.js" type="text/javascript"></script>
    <script src="Retirement/js/RetireScript.js" type="text/javascript"></script>
      <script src="Retirement/js/jquery.number.js" type="text/javascript"></script>
    <!--[if IE]><script type="text/javascript" src="../Scripts/jqplot/excanvas.js"></script><![endif]-->
    

    <style type='text/css'>
        .container {
            margin-top: 100px;
        }
    </style>
    <script type='text/javascript'>
        $(window).load(function () {

            $('.row_txtbx').number(true, 0);
            $('input:radio[name=rdbTax][id=rdbTax10]').prop('checked', true);

            $("#div_Pdf").mouseover(function () {

                var graphicImage = $('#plotted_image_div');
                var divGraph = $('#PieChart1').jqplotToImageStr({});
                //var divElem = $('<img/>').attr('src', divGraph);
                //graphicImage.html(divElem);
                //$('#plotted_image_div').css('display', 'block');

                //// dont delete the statement(2-1)
                //var dataToPush = JSON.stringify({
                //    baseimg: divGraph
                //});

                //$.ajax({
                //    type: "POST",
                //    url: "Retirement/WebMethod.aspx/setNPSChartimg",
                //    async: false,
                //    contentType: "application/json",
                //    data: dataToPush,
                //    dataType: "json",
                //    success: function (msg) {
                //        return true;
                //    },
                //    error: function (msg) {
                //       // alert("Error! Try again...");
                //    }
                //});
                //// end (2-1)
                NPSSaveImgFrmDotNetCharting();
                $("#hdCurrentAge").val($('#slider_currentAge').slider('value'));
                $("#hdExpReturn").val($('#slider_ExpRtnPer').slider('value'));
                $("#hdCurpus").val($('#slider_curpusPer').slider('value'));
                $("#hdWealthOnRetirement").val($('#lblTotWealthOnretirement').val());
                $("#hdCurpusWithdralAmt").val($('#lblCurpusWithdralAmt').val());
                $("#hdCorpusAnnuitiMonthPension").val($('#lblCorpusMonPension').val());
                $("#hdExpectedRateOfAnnuiti").val($('#slider_annuityRates').slider('value'));
                $("#hdPensionEarnedPerMonth").val($('#lblMonthlyPension').val());
                $('#hdExpRetirementAge').val($('#retirementAge').val());
            })

            function NPSSaveImgFrmDotNetCharting() {
                var dataToPush = JSON.stringify({
                    totInvest: $("#hdTotInvest").val(),
                    interestEarned: $('#hdInterestEarned').val()
                });

                //var dataToPush = "{ 'totInvest': '" + $("#hdTotInvest").val() + "', 'interestEarned': '" + $('#hdInterestEarned').val() + "'}";
                $.ajax({
                    type: "POST",
                    url: "Retirement/WebMethod.aspx/NpsGenereteChartImg",
                    async: false,
                    contentType: "application/json",
                    data: dataToPush,
                    dataType: "json",
                    success: function (msg) {

                    },
                    error: function (msg) {
                        alert("Error! Try again...");
                    }
                });
            }
            $('#hdCurrentAge').val(30);
            $('#hdExpRetirementAge').val(60);
            $('#hdMonthlySIP').val(500);

            NSP_Return_CalculationOnload();
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
                //$(".ui-slider-handle ui-state-default ui-corner-all ui-state-focus").click();
            }

            $("#slider_currentAge").slider(
                {
                    min: 18,
                    max: 60,
                    //  range:true,
                    value: 30,
                    slide: repositionTooltip,
                    stop: repositionTooltip,
                    change: function (event, ui) {
                        if (event.originalEvent) {
                            //NSP_Return_Calculation()
                            if ($('#slider_currentAge').slider('value') < $('#retirementAge').val()) {
                                NSP_Return_Calculation()
                            }
                            else {
                                $("#slider_currentAge").slider(
                                                                {
                                                                    min: 18,
                                                                    max: 60,
                                                                    value: $('#hdCurrentAge').val(),
                                                                    slide: repositionTooltip,
                                                                    stop: repositionTooltip,
                                                                });
                            }
                        }
                    }
                });
            $("#slider_currentAge .ui-slider-handle:first").tooltip({ title: $("#slider_currentAge").slider("value"), trigger: "manual" }).tooltip("show");

            $('#retirementAge').change(function () {

                if ($('#retirementAge').val() == '') {
                    $('#retirementAge').val($('#hdExpRetirementAge').val());
                    return;
                }
                if (!$.isNumeric($('#retirementAge').val())) {
                    $('#retirementAge').val($('#hdExpRetirementAge').val());
                    return;
                }

                if (($('#retirementAge').val() <= $('#slider_currentAge').slider('value')) || $('#retirementAge').val() > 60) {
                    $('#retirementAge').val($('#hdExpRetirementAge').val());
                }
                else {
                    $('#hdExpRetirementAge').val($('#retirementAge').val());

                    if ($('#retirementAge').val() >= 60) {
                        if ($('#slider_curpusPer').slider('value') < 40) {
                            $("#slider_curpusPer").slider({
                                value: 40,
                                min: 0,
                                max: 100,
                                slide: repositionTooltip,
                                stop: repositionTooltip,
                            });
                        }
                    }
                    else {
                        if ($('#slider_curpusPer').slider('value') < 80) {
                            $("#slider_curpusPer").slider({
                                value: 80,
                                min: 0,
                                max: 100,
                                slide: repositionTooltip,
                                stop: repositionTooltip,
                            });
                        }
                    }
                    NSP_Return_Calculation();
                }
            });
            $('#sipAmt').blur(function () {
                //alert('aksdja');
                if ((!$.isNumeric($('#sipAmt').val())) || ($('#sipAmt').val() == '')) {
                    $('#sipAmt').val($('#hdMonthlySIP').val());
                    return;
                }
                //if ($('#sipAmt').val() == '') {
                //    $('#sipAmt').val($('#hdMonthlySIP').val());
                //    return;
                //}
                if ($('#sipAmt').val() < 500) {
                    $('#sipAmt').val($('#hdMonthlySIP').val());
                }
                else {
                    $('#hdMonthlySIP').val($('#sipAmt').val());
                    //$('#sipAmt').text(addCommas($('#hdMonthlySIP').val()));
                    //addCommas
                    NSP_Return_Calculation();
                }
            });

            $('input:radio[name="rdbTax"]').change(function () {
                var totalyear = $('#retirementAge').val() - $('#slider_currentAge').slider('value');
                var SipAmt = $('#sipAmt').val();
                var totInvest = SipAmt * totalyear * 12;
                NPS_Tax_Calculation(totInvest);
            });

            $("#slider_ExpRtnPer").slider(
                {
                    min: 0,
                    max: 15,
                    step: 0.1,
                    value: 10,
                    slide: repositionTooltip,
                    stop: repositionTooltip,
                    change: function (event, ui) {
                        if (event.originalEvent) {
                            NSP_Return_Calculation()
                        }
                    }
                });
            $("#slider_ExpRtnPer .ui-slider-handle:first").tooltip({ title: $("#slider_ExpRtnPer").slider("value"), trigger: "manual" }).tooltip("show");
            
            $("#slider_curpusPer").slider(
                {
                    value: 40,
                    min: 0,
                    max: 100,
                    slide: repositionTooltip,
                    stop: repositionTooltip,
                    change: function (event, ui) {
                        //if (event.originalEvent) {
                        //NSP_Withdraw_Calculation();
                        //NSP_Pension_Calculation();

                        if (event.originalEvent) {
                            if ($('#retirementAge').val() >= 60) {
                                if ($('#slider_curpusPer').slider('value') < 40) {
                                    $("#slider_curpusPer").slider(
                                                            {
                                                                value: 40,
                                                                min: 0,
                                                                max: 100,
                                                                slide: repositionTooltip,
                                                                stop: repositionTooltip,
                                                            });
                                    return;
                                    //$("#slider_curpusPer").slider('value', 40);
                                }
                                else {
                                    $("#hdCurpus").val($('#slider_curpusPer').slider('value'));
                                    NSP_Withdraw_Calculation();
                                    NSP_Pension_Calculation();
                                }
                            }
                            else {
                                if ($('#slider_curpusPer').slider('value') < 80) {
                                    //$("#slider_curpusPer").remove();
                                    //$("#slider_curpusPer_Parent").append("<div id='slider_curpusPer' class='ui-slider-purple'></div>");
                                    //$("#slider_curpusPer .ui-slider-handle:first").tooltip({ title: $("#slider_curpusPer").slider("value"), trigger: "manual" }).tooltip("show");

                                    $("#slider_curpusPer").slider(
                                                            {
                                                                value: 80,
                                                                min: 0,
                                                                max: 100,
                                                                slide: repositionTooltip,
                                                                stop: repositionTooltip,
                                                            });
                                    return;
                                    //$("#slider_curpusPer").slider('value', 80);

                                }
                                else {
                                    $("#hdCurpus").val($('#slider_curpusPer').slider('value'));
                                    NSP_Withdraw_Calculation();
                                    NSP_Pension_Calculation();
                                }
                            }
                        }
                        //}
                    }
                });
            $("#slider_curpusPer .ui-slider-handle:first").tooltip({ title: $("#slider_curpusPer").slider("value"), trigger: "manual" }).tooltip("show");

            $("#slider_annuityRates").slider(
                {
                    min: 7,
                    max: 9,
                    step: 0.1,
                    value: 30,
                    slide: repositionTooltip,
                    stop: repositionTooltip,
                    change: function (event, ui) {
                        if (event.originalEvent) {
                            NSP_Pension_Calculation()
                        }
                    }
                });
            $("#slider_annuityRates .ui-slider-handle:first").tooltip({ title: $("#slider_annuityRates").slider("value"), trigger: "manual" }).tooltip("show");

            function NSP_Return_Calculation() {
                $('#hdCurrentAge').val($('#slider_currentAge').slider('value'));
                var totalyear = $('#retirementAge').val() - $('#slider_currentAge').slider('value');
                var SipAmt = $('#sipAmt').val();
                var InterestRate = $('#slider_ExpRtnPer').slider('value');

                //var dataToPush = {};
                //dataToPush["IntrestRate"] = InterestRate;
                //dataToPush["SipAmount"] = SipAmt;
                //dataToPush["TotalYear"] = totalyear;
                //dataToPush["RoundOff"] = 2;
                //4 ie issue
                var dataToPush = JSON.stringify({
                    IntrestRate: InterestRate,
                    SipAmount: SipAmt,
                    TotalYear: totalyear,
                    RoundOff: 0
                });
                //end ie issue
                var totInvest = SipAmt * totalyear * 12;
                $.ajax({
                    type: "POST",
                    url: "Retirement/WebMethod.aspx/GetExpectedReturn",
                    async: false,
                    contentType: "application/json",
                    data: dataToPush,
                    dataType: "json",
                    success: function (msg) {
                        plotPiechart(msg.d, totInvest);
                        $('#lblTotWealthOnretirement').text(addCommas(msg.d));
                        $('#lblTotWealthOnretirement').val(msg.d);
                        NSP_Withdraw_Calculation();
                        NSP_Pension_Calculation();
                    },
                    error: function (msg) {
                        alert("Error! Try again...");
                    }
                });


                NPS_Tax_Calculation(totInvest);
            }

            function NSP_Return_CalculationOnload() {

                var totalyear = 30;
                var SipAmt = 500;
                var InterestRate = 10;

                //var dataToPush = {};
                //dataToPush["IntrestRate"] = InterestRate;
                //dataToPush["SipAmount"] = SipAmt;
                //dataToPush["TotalYear"] = totalyear;
                //dataToPush["RoundOff"] = 2;

                ////4 ie issue
                //var dataToPush = {};
                //{
                //    IntrestRate: InterestRate.toString(),
                //    SipAmount: SipAmt.toString(),
                //    TotalYear: totalyear.toString(),
                //    RoundOff: 2
                //};
                ////end ie issue
                var dataToPush = "{ 'IntrestRate': '" + InterestRate + "', 'SipAmount': '" + SipAmt + "', 'TotalYear': '" + totalyear + "', 'RoundOff': '0' }";
                //alert(t);
                var totInvest = SipAmt * totalyear * 12;
                $.ajax({
                    type: "POST",
                    url: "Retirement/WebMethod.aspx/GetExpectedReturn",
                    async: false,
                    contentType: "application/json",
                    data: dataToPush,
                    dataType: "json",
                    success: function (msg) {
                        plotPiechart(msg.d, totInvest);
                        $('#lblTotWealthOnretirement').text(addCommas(msg.d.toFixed(0)));
                        $('#lblTotWealthOnretirement').val(msg.d.toFixed(0));

                        NSP_Withdraw_Calculation_Load();
                        NSP_Pension_Calculation_Load();
                    },
                    error: function (msg) {
                        alert("Error! Try again...");
                    }
                });
                NPS_Tax_Calculation(totalyear * SipAmt * 12);
            }

            function plotPiechart(data, totInvest) {
                var totEarned = data - totInvest;
                var dataPlot = [[]];
                for (var i = 0; i < 2; i += 1) {
                    var TextVal = [];
                    if (i == 0) {
                        TextVal.push('Principal Invested');
                        TextVal.push(totInvest);
                    }
                    else {
                        TextVal.push('Interest Earned');
                        TextVal.push(totEarned);
                    }
                    dataPlot.push(TextVal);
                }
                $("#hdTotInvest").val(totInvest);
                $("#hdInterestEarned").val(totEarned);
                dataPlot.shift();

                var plot1 = $.jqplot('PieChart1', [dataPlot], {
                    seriesColors: ["#2895ce", "#d9192b"],
                    gridPadding: { top: 0, bottom: 0, left: 0, right: 0 },
                    grid: {
                        drawGridLines: true,        // wether to draw lines across the grid or not.
                        gridLineColor: '#cccccc',
                        background: '#ffffff',
                        borderColor: '#ffffff',     // CSS color spec for border around grid.
                        borderWidth: 2.0,           // pixel width of border around grid.
                        shadow: true,               // draw a shadow for grid.
                        shadowAngle: 75,            // angle of the shadow.  Clockwise from x axis.
                        shadowOffset: 1.5,          // offset from the line of the shadow.
                        shadowWidth: 3,             // width of the stroke for the shadow.
                        shadowDepth: 3
                    },
                    seriesDefaults: {
                        renderer: jQuery.jqplot.PieRenderer,
                        rendererOptions: {
                            //showDataLabels: true,
                            sliceMargin: 2,
                            startAngle: 0,
                            dataLabelFormatString: '%.2f%%',
                            dataLabels: 'value',
                            dataLabelThreshold: 0,
                            dataLabelPositionFactor: 1.3
                        }
                    },
                    legend: {
                        show: false,
                        // placement: 'outside',
                        rendererOptions: {
                            numberRows: 2
                        },
                        location: 'e',
                        fontFamily: 'Helvetica,Arial,sans-serif',
                        fontSize: '12px'
                    }
                });
            }

            function NSP_Withdraw_Calculation() {
                var curpusPercentage = $('#slider_curpusPer').slider('value')
                var withdrawVal = $('#lblTotWealthOnretirement').val() * (100 - curpusPercentage) / 100;
                $('#lblCurpusWithdralAmt').text(addCommas(withdrawVal.toFixed(0)));
                $('#lblCurpusWithdralAmt').val(withdrawVal.toFixed(0));
                var CorpusMonPension = $('#lblTotWealthOnretirement').val() - withdrawVal;
                $('#lblCorpusMonPension').text(addCommas(CorpusMonPension.toFixed(0)));
                $('#lblCorpusMonPension').val(CorpusMonPension.toFixed(0));
            }

            function NSP_Pension_Calculation() {
                var annuityRate = $('#slider_annuityRates').slider('value') / 100;
                var pension = $('#lblCorpusMonPension').val() * annuityRate / 12;
                $('#lblMonthlyPension').text(addCommas(pension.toFixed(0)));
                $('#lblMonthlyPension').val(pension.toFixed(0));
            }

            function NSP_Withdraw_Calculation_Load() {
                var curpusPercentage = 30
                var withdrawVal = $('#lblTotWealthOnretirement').val() * (100 - curpusPercentage) / 100;
                $('#lblCurpusWithdralAmt').text(addCommas(withdrawVal.toFixed(0)));
                $('#lblCurpusWithdralAmt').val(withdrawVal.toFixed(0));
                var CorpusMonPension = $('#lblTotWealthOnretirement').val() - withdrawVal;
                $('#lblCorpusMonPension').text(addCommas(CorpusMonPension.toFixed(0)));
                $('#lblCorpusMonPension').val(CorpusMonPension.toFixed(0));
            }

            function NSP_Pension_Calculation_Load() {
                var annuityRate = 9 / 100;
                var pension = $('#lblCorpusMonPension').val() * annuityRate / 12;
                $('#lblMonthlyPension').text(addCommas(pension.toFixed(0)));
                $('#lblMonthlyPension').val(pension.toFixed(0));
                $('#hdTax').val(10);
            }

            function NPS_Tax_Calculation(totInvest) {
                if ($("#rdbTax10").is(":checked")) {
                    $('#LblTaxSaved').val($('#rdbTax10').val() * totInvest / 100);
                    $('#LblTaxSaved').text(addCommas($('#rdbTax10').val() * totInvest / 100));
                    $('#hdTaxSaved').val($('#rdbTax10').val() * totInvest / 100);
                    $('#hdTax').val($('#rdbTax10').val());
                }
                if ($("#rdbTax20").is(":checked")) {
                    $('#LblTaxSaved').val($('#rdbTax20').val() * totInvest / 100);
                    $('#LblTaxSaved').text(addCommas($('#rdbTax20').val() * totInvest / 100));
                    $('#hdTaxSaved').val($('#rdbTax20').val() * totInvest / 100);
                    $('#hdTax').val($('#rdbTax20').val());
                }
                if ($("#rdbTax30").is(":checked")) {
                    $('#LblTaxSaved').val($('#rdbTax30').val() * totInvest / 100);
                    $('#LblTaxSaved').text(addCommas($('#rdbTax30').val() * totInvest / 100));
                    $('#hdTaxSaved').val($('#rdbTax30').val() * totInvest / 100);
                    $('#hdTax').val($('#rdbTax30').val());
                }
            }


            function addCommas(nStr) {
                ////US format-- dont delete---
                //nStr += '';
                //x = nStr.split('.');
                //x1 = x[0];
                //x2 = x.length > 1 ? '.' + x[1] : '';
                //var rgx = /(\d+)(\d{3})/;
                //while (rgx.test(x1)) {
                //    x1 = x1.replace(rgx, '$1' + ',' + '$2');
                //}
                //return x1 + x2;
                ////end us format

                var x = nStr;
                x = x.toString();
                var afterPoint = '';
                if (x.indexOf('.') > 0)
                    afterPoint = x.substring(x.indexOf('.'), x.length);
                x = Math.floor(x);
                x = x.toString();
                var lastThree = x.substring(x.length - 3);
                var otherNumbers = x.substring(0, x.length - 3);
                if (otherNumbers != '')
                    lastThree = ',' + lastThree;
                var res = otherNumbers.replace(/\B(?=(\d{2})+(?!\d))/g, ",") + lastThree + afterPoint;

                //alert(res);
                return res;
            }
        });
    </script>
<%--    <script type="text/javascript">
        $(document).ready(function () { if (!$.jqplot._noCodeBlock) { $("script.code").each(function (c) { if ($("pre.code").eq(c).length) { $("pre.code").eq(c).text($(this).html()) } else { var d = $('<pre class="code prettyprint brush: js"></pre>'); $("div.jqplot-target").eq(c).after(d); d.text($(this).html()); d = null } }); $("script.common").each(function (c) { $("pre.common").eq(c).text($(this).html()) }); var b = ""; if ($("script.include, link.include").length > 0) { if ($("pre.include").length == 0) { var a = ['<div class="code prettyprint include">', '<p class="text">The charts on this page depend on the following files:</p>', '<pre class="include prettyprint brush: html gutter: false"></pre>', "</div>"]; a = $(a.join("\n")); $("div.example-content").append(a); a = null } $("script.include").each(function (c) { if (b !== "") { b += "\n" } b += '<script type="text/javascript" src="' + $(this).attr("src") + '"><\/script>' }); $("link.include").each(function (c) { if (b !== "") { b += "\n" } b += '<link rel="stylesheet" type="text/css" hrf="' + $(this).attr("href") + '" />' }); $("pre.include").text(b) } else { $("pre.include").remove(); $("div.include").remove() } } if (!$.jqplot.use_excanvas) { $("div.jqplot-target").each(function () { var d = $(document.createElement("div")); var g = $(document.createElement("div")); var f = $(document.createElement("div")); d.append(g); d.append(f); d.addClass("jqplot-image-container"); g.addClass("jqplot-image-container-header"); f.addClass("jqplot-image-container-content"); g.html("Right Click to Save Image As..."); var e = $(document.createElement("a")); e.addClass("jqplot-image-container-close"); e.html("Close"); e.attr("href", "#"); e.click(function () { $(this).parents("div.jqplot-image-container").hide(500) }); g.append(e); $(this).after(d); d.hide(); d = g = f = e = null; if (!$.jqplot._noToImageButton) { var c = $(document.createElement("button")); c.text("View Plot Image"); c.addClass("jqplot-image-button"); c.bind("click", { chart: $(this) }, function (h) { var j = h.data.chart.jqplotToImageElem(); var i = $(this).nextAll("div.jqplot-image-container").first(); i.children("div.jqplot-image-container-content").empty(); i.children("div.jqplot-image-container-content").append(j); i.show(500); i = null }); $(this).after(c); c.after("<br />"); c = null } }) } $(document).unload(function () { $("*").unbind() }) });
    </script>--%>
</head>
<body>
    <form id="form1" runat="server">
        <table width="700" border="0" align="center" cellpadding="0" cellspacing="0">
            <tr>
                <td>
                    <div class="pageCont">
                        <div class="blueBox">
                            <div class="Boxborder">
                                <table width="93%" border="0" align="center" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td style="text-align: right; padding-top: 10px;">
                                            <div id="div_Pdf">
                                                <asp:LinkButton ID="LinkButtonGenerateReport" runat="server" ToolTip="Download PDF"
                                                    OnClick="LinkButtonGenerateReport_Click"><img src="images/pdf.jpg" alt="" /></asp:LinkButton>
                                                <%--OnClientClick="javascript:return pdfcheck();"--%>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="row_gap1">
                                            <div class="span8">
                                                <div class="span3">
                                                    <img src="images/age.jpg" />
                                                </div>
                                                <%--                                                <div class="span4">
                                                    <div class="slidr_top_margin">
                                                        <div id="slider_currentAge" class="ui-slider-purple">
                                                        </div>
                                                    </div>
                                                </div>--%>
                                                <div class="span4">
                                                    <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                        <tr>
                                                            <td>
                                                                <div class="slidr_top_margin">
                                                                    <div id="slider_currentAge" class="ui-slider-purple">
                                                                    </div>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="row_gap">
                                            <div class="span8">
                                                <div class="span3">
                                                    <img src="images/expct_age.jpg" />
                                                </div>
                                                <div class="span4">
                                                    <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                        <tr>
                                                            <td>
                                                                <table width="122" border="0" align="left" cellpadding="0" cellspacing="0" class="txtbx_bg">
                                                                    <tr>
                                                                        <td>
                                                                            <input name="input" type="text" id="retirementAge" style="width: 85px; height: 25px; border: none; background: #e9e6e6; margin-top: 4px; margin-left: 6px; padding: 0;"
                                                                                maxlength="2" value="60" />
                                                                        </td>
                                                                        <td class="rupee_blue">yrs.
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="tip_txt">This should be greater than your age and less than or equal to 60 years
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <div class="span8">
                                                <div class="span3">
                                                    <img src="images/mnth_nps.jpg" />
                                                </div>
                                                <div class="span4">
                                                    <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                        <tr>
                                                            <td>
                                                                <table width="122" border="0" align="left" cellpadding="0" cellspacing="0" class="txtbx_bg">
                                                                    <tr>
                                                                        <td>
                                                                            <img src="images/rs.png" width="13" height="15" />
                                                                        </td>
                                                                        <td>
                                                                            <input name="input2" value="500" type="text" id="sipAmt" style="width: 90px; height: 25px; border: none;
                                                                                 background: #e9e6e6; margin-top: 4px; margin-left: 6px; padding: 0;"
                                                                                maxlength="7" runat="server" class="row_txtbx"/>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="tip_txt">Min amount is Rs.  500/month
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="row_gap1">
                                            <div class="span8">
                                                <div class="span3">
                                                    <img src="images/tx_brckt.jpg" />
                                                </div>
                                                <div class="span4">
                                                    <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                        <tr>
                                                            <td>
                                                                <table width="195" border="0" align="left" cellpadding="0" cellspacing="0">
                                                                    <tr>
                                                                        <td class="chkbx">
                                                                            <input name="rdbTax" id="rdbTax10" type="radio" value="10" title="10" />10%
                                                                        </td>
                                                                        <td class="chkbx">
                                                                            <input name="rdbTax" id="rdbTax20" type="radio" value="20" title="20" />20%
                                                                        </td>
                                                                        <td class="chkbx">
                                                                            <input name="rdbTax" id="rdbTax30" type="radio" value="30" title="30" />30%
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="tip_txt">Tax benefit u/s 80 CCD (2) is limited to an amount up to 10% of (basic salary +D.A)
                                                            and/or under section 80 CCC is cumulatively limited to an amount of Rs. 1 lakh
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="row_gap1">
                                            <div class="span8">
                                                <div class="span3">
                                                    <img src="images/exp_retrn.jpg" />
                                                </div>
                                                <div class="span4">
                                                    <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                        <tr>
                                                            <td>
                                                                <div class="slidr_top_margin">
                                                                    <div id="slider_ExpRtnPer" class="ui-slider-purple">
                                                                    </div>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="tip_txt1">Your corpus will generate each year
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="row_gap">&nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="ans_bg">
                                            <p id="TotWealthOnretirement" class="ans_txt">
                                                Congrats your pension wealth on retirement will be <span class="WebRupee">Rs.</span>&nbsp;<span
                                                    id="lblTotWealthOnretirement"></span>
                                            </p>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                <tr>
                                                    <td style="width: 22%" class="lady">
                                                        <img src="images/lady.jpg" alt="" />
                                                    </td>
                                                    <td style="width: 36%" class="chart" id="tdPieChart1">
                                                        <div id="PieChart1" runat="server" style="height: 190px">
                                                        </div>
                                                    </td>
                                                    <td style="width: 20%; vertical-align: bottom; padding-bottom: 40px">
                                                        <img src="images/ChartLeagend.jpg" alt="" />
                                                    </td>
                                                    <td style="width: 22%" class="man">
                                                        <img src="images/man.jpg" alt="" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="ans_bg">
                                            <p class="ans_txt">
                                                Tax Saved:<span class="WebRupee">Rs.</span>&nbsp;<span id="LblTaxSaved"></span>
                                            </p>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tip_txt2">Tax is calculated based on the assumption that monthly contribution by your employer is 10 % of your basic salary + D.A
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="row_gap1">&nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tip_txt2">NPS rule: If your retirement age is 60, you are required to compulsorily annuitize
                at least 40% of your savings towards your monthly pension. If your retirement age
                is earlier than 60, you are required to compulsorily annuitize at least 80% of your
                savings towards your monthly pension.
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="row_gap">
                                            <div class="span8">
                                                <div class="span3">
                                                    <img src="images/corpus.jpg" />
                                                </div>
                                                <div class="span4">
                                                    <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                        <tr>
                                                            <td>
                                                                <div class="slidr_top_margin">
                                                                    <div id="slider_curpusPer" class="ui-slider-purple">
                                                                    </div>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="tip_txt1">You will invest to earn monthly pension
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="row_gap1">&nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="ans_bg">
                                            <p class="ans_txt">
                                                Corpus you will be able to withdraw <span class="WebRupee">Rs.</span>&nbsp;<span id="lblCurpusWithdralAmt"></span>
                                            </p>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="row_gap1">&nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="ans_bg">
                                            <p class="ans_txt">
                                                Corpus annuitised for monthly pension <span class="WebRupee">Rs.</span>&nbsp;<span
                                                    id="lblCorpusMonPension"></span>
                                            </p>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="row_gap">
                                            <div class="span8">
                                                <div class="span3">
                                                    <img src="images/annunity.jpg" />
                                                </div>
                                                <div class="span4">
                                                    <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                        <tr>
                                                            <td>
                                                                <div class="slidr_top_margin">
                                                                    <div id="slider_annuityRates" class="ui-slider-purple">
                                                                    </div>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="tip_txt1">Annuity rates range from 7-9%
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="row_gap1">&nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="result">
                                            <p class="result_txt">
                                                Pension earned per month <span class="WebRupee">Rs.</span>&nbsp;<span id="lblMonthlyPension"></span>
                                            </p>
                                        </td>
                                    </tr>

        <tr>
            <td class="tip_txt2" style="padding-top:20px"><b>Disclaimer: The following information is provided for general information purposes only and applies to the scheme. In view of the individual nature of Tax benefits, each NPS contributors is advised to consult his or her own tax consultant with respect to the specific tax implication arising out of his or her participation in the scheme. </b>
            </td>
        </tr>
                                    <tr>
                                        <td class="row_gap1">
                                            <input type="hidden" id="hdCurrentAge" runat="server" />
                                            <input type="hidden" id="hdExpRetirementAge" runat="server" />
                                            <input type="hidden" id="hdMonthlySIP" runat="server" />
                                            <input type="hidden" id="hdChart" runat="server" />
                                            <input type="hidden" id="hdExpReturn" runat="server" />
                                            <input type="hidden" id="hdChartImg" runat="server" />
                                            <input type="hidden" id="hdWealthOnRetirement" runat="server" />
                                            <input type="hidden" id="hdCurpus" runat="server" />
                                            <input type="hidden" id="hdCurpusWithdralAmt" runat="server" />
                                            <input type="hidden" id="hdCorpusAnnuitiMonthPension" runat="server" />
                                            <input type="hidden" id="hdExpectedRateOfAnnuiti" runat="server" />
                                            <input type="hidden" id="hdPensionEarnedPerMonth" runat="server" />
                                            <input type="hidden" id="hdTaxSaved" runat="server" />
                                            <input type="hidden" id="hdTax" runat="server" />
                                            <input type="hidden" id="decode" runat="server" />
                                            <input type="hidden" id="hdTotInvest" runat="server" />
                                            <input type="hidden" id="hdInterestEarned" runat="server" />
                                            <%--<asp:HiddenField ID="Hidfld" runat="server" />--%>
                                            
                                        </td>
                                    </tr>

                                    <tr>
                                        <td>
                                            <div id="plotted_image_div" class="" style="display: none;" runat="server"></div>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                    </div>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
