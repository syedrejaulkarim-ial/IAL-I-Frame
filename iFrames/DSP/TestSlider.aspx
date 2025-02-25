<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="NPSCalc.aspx.cs" Inherits="iFrames.DSP.NPSCalc" %>
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
    <script type='text/javascript' src='js/jquery-1.8.3.js'></script>
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
                                        <td class="row_gap1">
                                            <div class="span8">
                                                <div class="span3">
                                                    <img src="images/age.jpg" />
                                                </div>
                                               
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
