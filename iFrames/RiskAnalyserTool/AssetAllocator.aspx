<%@ Page Language="C#" AutoEventWireup="true" ViewStateEncryptionMode="Always" EnableViewStateMac="true" CodeBehind="AssetAllocator.aspx.cs" Inherits="iFrames.RiskAnalyserTool.AssetAllocator" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Asset Allocator</title>
    
    <meta http-equiv="X-UA-Compatible" content="IE=edge"/>
    <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1">
    <link href='http://fonts.googleapis.com/css?family=Open+Sans+Condensed:300,700' rel='stylesheet' type='text/css'/>   
    <link href="css/bootstrap.css" rel="stylesheet" type="text/css" media="all" />
    <link href="css/bootstrap.min.css" rel="stylesheet" type="text/css" media="all" />
    <link href="css/bootstrap-responsive.min.css" rel="stylesheet" type="text/css" media="all" />
    <link href="css/jquery.jqplot.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" href="css/jquery-slider.css" />
    <link href="css/stylesheet.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" href="css/jquery-ui-1.10.3.custom.min.css" />    
    <link href="css/style.css" rel="stylesheet" type="text/css" media="all" />
    <!--[if lt IE 9]><link href="css/styleIE8.css" rel="stylesheet" type="text/css" media="all" /><![endif]-->
   
    <!--[if lt IE 9]><script src="js/respond.js"></script><![endif]-->
     
    <script type='text/javascript' src='js/jquery-5.js'></script>
    <script type="text/javascript" src="js/jquery-ui.js"></script>
    <script src="js/jquery.confirn_box.js"></script>
    <script src="https://www.gstatic.com/charts/loader.js"></script>
    <%--<script type="text/javascript" src="../Scripts/JsApi/jsapi.js"></script>--%>
    <script src="js/bootstrap.min.js"></script>
   <script type="text/javascript">
       WebFontConfig = {
           google: { families: ['Open+Sans+Condensed:300,700:latin'] }
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

    <style type="text/css">
        body, td, th {
            font-family: 'Open Sans Condensed', sans-serif;
            /*overflow:hidden;*/
            height:100%;
            overflow-x:hidden;
            overflow-y: scroll;

        }

        a:link {
            color: #fff;
            text-decoration: underline;
        }

        a:visited {
            text-decoration: underline;
            color: #fff;
        }

        a:hover {
            text-decoration: none;
            color: #fff;
        }

        a:active {
            text-decoration: underline;
        }


    </style>

    <script type="text/javascript">

        //google.load("visualization", "1", { packages: ["corechart"] });
        google.charts.load('current', { packages: ['corechart'] });
        google.charts.setOnLoadCallback(drawChart)
        function drawChart(data) {
            var titlePositionLeft = '38%';
            //debugger;
            if (/Android|webOS|iPhone|iPad|iPod|BlackBerry|IEMobile|Opera Mini/i.test(navigator.userAgent)) {
                titlePositionLeft = '10%';
                //alert("ok");
            }
            var dataPlot = [[]];
            var labels = [];
            var TotVal = 0;
            dataPlot.push(['ajsdkjas', 'ahsdjahs']);
            for (var i = 0; i < data.length; i += 1) {
                TotVal = TotVal + parseInt(data[i].Value);
            }
            for (var i = 0; i < data.length; i += 1) {
                var TextVal = [];
                //TextVal.push(data[i].Name + ' - ' + ((parseInt(data[i].Value) / parseInt(TotVal)) * 100).toFixed(0) + '%');
                TextVal.push(data[i].Name);
                TextVal.push(parseInt(data[i].Value));
                dataPlot.push(TextVal);
            }
            dataPlot.shift();
            var data1 = google.visualization.arrayToDataTable(dataPlot);

            var options = {
                title: 'Your Ideal Asset Allocation',
                titleTextStyle:{fontName:['Roboto+Condensed:400,700,300:latin'],fontSize:18},
                //titlePosition:'centre',
                backgroundColor: { stroke: "none", strokeWidth: 5, fill: "none" },
                colors: ['#617EC2', '#FB6163', '#DC69D0', '#69E6E0', '#FDD514'],
                is3D: true,
                //height: 500,
                chartArea:{left:'10%',top:100,width:'80%',height:'60%'},
                fontName: ['Roboto+Condensed:400,700,300:latin'],
                fontSize: 14,
                interpolateNulls: true,
                slices: {
                    0: { offset: 0.05 },
                    1: { offset: 0.05 },
                    2: { offset: 0.05 },
                    3: { offset: 0.1 },
                    4: { offset: 0.05 },
                },
                legend: {
                    position: 'labeled',
                    maxLines: 5,
                    //bar: { groupWidth: '100%', groupHeight: '100%' },
                    //isStacked: false,
                    //height: 200,
                    alignment: 'start',
                    textStyle: { fontName:['Roboto+Condensed:400,700,300:latin'],width:20,fontSize: 14 }
                },

                //legend: { position: 'bottom', textStyle: { color: 'blue', fontSize: 16 }, maxLines: 5 },
            };

            var chart = new google.visualization.PieChart(document.getElementById('chartAssetAllocationContainer'));
            chart.draw(data1, options);
            //$("text:contains(" + options.title + ")").attr({ 'x': '380', 'y': '20' })
            $("text:contains(" + options.title + ")").attr({ 'x': titlePositionLeft, 'y': '500' })
        }
        $(function () {
            //to b delete
            if (/Android|webOS|iPhone|iPad|iPod|BlackBerry|IEMobile|Opera Mini/i.test(navigator.userAgent)) {
                //alert("ok");
                $("#MfIframe").css('overflow', 'scroll');
            }
            
            $('#trResult').hide();
            if ($("#hdRiskAppetite").val() != 0) {
                $("input radio[name='RdbAns2'],[value=" + $("#hdRiskAppetite").val() + "]").attr('checked', 'checked');
            }
            if ($("#hdInvestmentHorizon").val() != 0) {
                $("input radio[name='RdbAns3'],[value=" + $("#hdInvestmentHorizon").val() + "]").attr('checked', 'checked');

            }
            if ($("#hdYourView").val() != 0) {
                $("input radio[name='RdbAns4'],[value=" + $("#hdYourView").val() + "]").attr('checked', 'checked');
            }
            // responsive-------//

            $('#RdbAns2').replaceWith($('table').html()
            .replace(/<tbody>/gi, "")
            .replace(/<tr>/gi, "")
            .replace(/<td/gi, "<div class='span2 txt_box1 gap'")
            .replace(/<\/td>/gi, "</div>")
            .replace(/<\/tr>/gi, "")
            .replace(/<\/tbody>/gi, "")
            );

            $('#RdbAns3').replaceWith($('table').html()
           .replace(/<tbody>/gi, "")
           .replace(/<tr>/gi, "")
           .replace(/<td/gi, "<div class='span3 txt_box1 gap'")
           .replace(/<\/td>/gi, "</div>")
           .replace(/<\/tr>/gi, "")
           .replace(/<\/tbody>/gi, "")
           );
            $('label[for="RdbAns3_0"]').parent().removeAttr('class');
            $('label[for="RdbAns3_0"]').parent().attr('class', 'span2 txt_box1 gap');
            $('#RdbAns4').replaceWith($('table').html()
            .replace(/<tbody>/gi, "")
            .replace(/<tr>/gi, "")
            .replace(/<td/gi, "<div class='span2 txt_box1 gap'")
            .replace(/<\/td>/gi, "</div>")
            .replace(/<\/tr>/gi, "")
            .replace(/<\/tbody>/gi, "")
            );
            // end responsive


            var labelArr = new Array("--", "Under 30", "30-40", "41-50", "51-60", "60 or over", "");
            $('#lblSlider1').attr('style', 'color:#ffb21c');

            $("#slider").slider({
                value: $("#hdAssetAllocatorAge").val(),
                min: 0,
                max: 6,
                step: 1,
                range: "max",
                slide: function (event, ui) {
                    //$("#days").val(ui.value);
                    //$("#label").html(labelArr[ui.value]);
                },
                change: function (event, ui) {
                    if (event.originalEvent) {
                        for (i = 1; i <= 5; i++) {
                            $('#lblSlider' + i).attr('style', 'color:white');
                        }
                        $('#lblSlider' + $('#slider').slider('value')).attr('style', 'color:#ffb21c');

                        if ($('#slider').slider('value') == 0) {
                            $('#lblSlider1').attr('style', 'color:#ffb21c');
                            $("#slider").slider({
                                value: 1,
                                min: 0,
                                max: 6,
                                step: 1,
                                range: "max",
                                slide: function (event, ui) {
                                    //$("#days").val(ui.value);
                                    //$("#label").html(labelArr[ui.value]);
                                },
                            });
                        }
                        if ($('#slider').slider('value') == 6) {
                            $('#lblSlider5').attr('style', 'color:#ffb21c');
                            $("#slider").slider({
                                value: 5,
                                min: 0,
                                max: 6,
                                step: 1,
                                range: "max",
                                slide: function (event, ui) {
                                    //$("#days").val(ui.value);
                                    //$("#label").html(labelArr[ui.value]);
                                },
                            });
                        }
                    }
                },
            });

            $("#btnCal").click(function (ev) {
                //now 
                //$("#trResult").focus();
                $("#chartAssetAllocationContainer").animate({ scrollTop: 0 }, 'slow');
                //$('html, body').animate({ scrollTop: 0 }, 'slow');
                /// now
                //debugger;
                var Jparam = jQuery.parseJSON($("#hdJsonParam").val());
                var CurrAge = $('#slider').slider("option", "value").toString();
                var ApiAge = "";

                var TotSum = 0;
                switch (CurrAge) {
                    case "0":
                        TotSum = 10;
                        break;
                    case "1":
                        TotSum = 10;
                        ApiAge = "Under 30";
                        break;
                    case "2":
                        TotSum = 8;
                        ApiAge = "31-40";
                        break;
                    case "3":
                        TotSum = 6;
                        ApiAge = "41-50";
                        break;
                    case "4":
                        TotSum = 4;
                        ApiAge = "51-60";
                        break;
                    case "5":
                        TotSum = 2;
                        ApiAge = "Over 60";
                        break;
                    case "6":
                        TotSum = 2;
                        break;
                }
                
              
                var CheckCount = 0;
                var test = $('#container input:radio:checked').map(function () {
                    TotSum = parseInt(TotSum) + parseInt(this.value);
                    CheckCount = CheckCount + 1;
                    return TotSum;
                }).get();
                if (CheckCount < 3) {
                    OpenAlert("Please answer the entire question");
                    return false;
                }

                if (!$("#disclaimer-check").is(':checked')) {
                    OpenAlert("Please agree to the terms of the disclaimer");
                    return false;
                }
                //************ for Api *********//
                var ApiRiskApetiteId = $("#Q2 input:radio:checked").attr('id');
                //alert($('label[for=' + ApiRiskApetiteId + ']').text());
                //alert($('label[for=' + ApiRiskApetiteId + ']')[0].innerText.trim());
                var ApiRiskApetite = $('label[for=' + ApiRiskApetiteId + ']').text().trim();

                var ApiInvestmentPlanId = $("#Q3 input:radio:checked").attr('id');
                var ApiInvestmentPlan = $('label[for=' + ApiInvestmentPlanId + ']').text().trim();

                var ApiViewGrowthId = $("#Q4 input:radio:checked").attr('id');
                var ApiViewGrowth = $('label[for=' + ApiViewGrowthId + ']').text().trim();

                //********** End for Api********//
                $('#trResult').show();

                $("#trOption").hide();
                var modVal = Math.round((TotSum / 65) * 100, 2);
                //alert(modVal);
                var InvestProfile = "";
                var DataAssetAllocation = [[]];
                var DataRecAssetAllocation = [[]];
                for (i = 0; i < Jparam.length ; i++) {
                    if (Jparam[i].StartVal <= modVal && Jparam[i].EndVal >= modVal) {
                        //var ApiResult = Jparam[i].Profile;
                        InvestProfile = 'Investment Profile: ' + Jparam[i].Profile;
                        $("#DvShowProfile").html(InvestProfile);
                        //debugger;
                        // for Api data
                        var ApiResult = "";
                        for (v = 0; v < Jparam[i].LstAsset.length; v++) {
                            var strVal = Jparam[i].LstAsset[v].Name.replace(/\ /g, '_');
                            strVal = strVal.replace(/\//g, '_');
                            strVal = strVal.replace(/\&/g, '_');
                            strVal = strVal.replace("__", "_");
                            strVal = strVal.replace("__", "_");
                            strVal = strVal.replace("__", "_");
                            strVal = strVal.replace("__", "_");
                            strVal = strVal.replace("__", "_");
                            strVal = strVal.replace("__", "_");
                            if(v==Jparam[i].LstAsset.length-1)
                            {
                             ApiResult = ApiResult + strVal + "=" + Jparam[i].LstAsset[v].Value;
                             }
                             else
                             {
                            ApiResult = ApiResult + strVal + "=" + Jparam[i].LstAsset[v].Value + ",";
                            }
                        }
                        ApiResult = "'" + ApiResult + "'";
                        ApiResult = ApiResult.replace(/\,/g, ' ');
                        //debugger;
                        var dataToPush = JSON.stringify({
                            user_id: $("#hdUserId").val(),
                            age: ApiAge,
                            risk_appetite_calculated: $("#hdIsCal").val(),
                            risk_appetite: ApiRiskApetite,
                            investment_plan: ApiInvestmentPlan,
                            view_growth: ApiViewGrowth,
                            result: ApiResult
                        });
                        var finaldata = JSON.stringify({
                            user_data: dataToPush
                        });
                        var ApiURL = 'http://www.jpmalphabet.com/api/v1/asset_users.json?user_data={user_id:' + $("#hdUserId").val() + ',age:' + ApiAge + ',risk_appetite_calculated:' + $("#hdIsCal").val() + ',risk_appetite:' + ApiRiskApetite + ',investment_plan:' + ApiInvestmentPlan + ',view_growth:' + ApiViewGrowth + ',result:' + ApiResult + '}';
                        $.ajax({
                            cache: false,
                            //dataType: "json",
                            //data: {},
                            //url: 'http://www.jpmalphabet.com/api/v1/asset_users.json?user_data={user_id:1,age:25,risk_appetite_calculated:no,risk_appetite:aggressive,investment_plan:3,view_growth:positive,result:pass}',
                            url:ApiURL,
                            type: 'POST',
                            //contentType: "application/json; charset=utf-8",
                            success: function (dataConsolidated) {
                                var obj = jQuery.parseJSON(dataConsolidated.d);
                                
                            },
                            error: function (data) {
                                // debugger;
                                //alert(data);
                            }
                        });

                        // end Api data
                        //debugger;
                        drawChart(Jparam[i].LstAsset);
                        break;
                    }
                }
            });

            $("#Click2RiskApp").click(function (ev) {
                var url = "/RiskAnalyserTool/AssetAllocatorRiskQue.aspx?user_id=" + $("#hdUserId").val() + "&CurrentAge=" + $('#slider').slider("option", "value");
                if ($("input:radio[name='RdbAns2']:checked").length != 0) {
                    url = url + "&RiskAppetite=" + $("input:radio[name='RdbAns2']:checked").val();
                }
                else {
                    url = url + "&RiskAppetite=0"
                }
                if ($("input:radio[name='RdbAns3']:checked").length != 0) {
                    url = url + "&InvestmentHorizon=" + $("input:radio[name='RdbAns3']:checked").val();
                }
                else {
                    url = url + "&InvestmentHorizon=0"
                }
                if ($("input:radio[name='RdbAns4']:checked").length != 0) {
                    url = url + "&YourView=" + $("input:radio[name='RdbAns4']:checked").val();
                }
                else {
                    url = url + "&Yourview=0"
                }
                window.location = url;
            });

            //-	Step by step tab selection process
            $("input[type='radio']").click(function (event) {
                //alert(event.target.id);

                var index = event.target.id.substring(6, event.target.id.indexOf('_'));
                if (index > 2) {
                    var CheckCount = 0;
                    var PrevIndex = parseInt(index) - 1;
                    var test = $('#Q' + PrevIndex + ' input:radio:checked').map(function () {
                        CheckCount = CheckCount + 1;
                        return CheckCount;
                    }).get();
                    if (CheckCount == 0) {
                        OpenAlert('Please answer previous question');
                        event.target.checked = false;
                    }
                }
            });

            //end -	Step by step tab selection process

            //-- reset------------------------------//

            $("#btnClear").click(function (ev) {
                $("#trOption input[type='radio']").removeAttr('checked');
                for (i = 1; i <= 5; i++) {
                    $('#lblSlider' + i).attr('style', 'color:white');
                }
                $('#lblSlider1').attr('style', 'color:#ffb21c');
                $("#disclaimer-check").prop("checked", false);
                $("#slider").slider({
                    value: $("#hdAssetAllocatorAge").val(),
                    min: 0,
                    max: 6,
                    step: 1,
                    range: "max",
                    change: function (event, ui) {
                        if (event.originalEvent) {
                            for (i = 1; i <= 5; i++) {
                                $('#lblSlider' + i).attr('style', 'color:white');
                            }
                            $('#lblSlider' + $('#slider').slider('value')).attr('style', 'color:#ffb21c');

                            if ($('#slider').slider('value') == 0) {
                                $('#lblSlider1').attr('style', 'color:#ffb21c');
                                $("#slider").slider({
                                    value: 1,
                                    min: 0,
                                    max: 6,
                                    step: 1,
                                    range: "max",
                                });
                            }
                            if ($('#slider').slider('value') == 6) {
                                $('#lblSlider5').attr('style', 'color:#ffb21c');
                                $("#slider").slider({
                                    value: 5,
                                    min: 0,
                                    max: 6,
                                    step: 1,
                                    range: "max",
                                });
                            }
                        }
                    },
                });

            });

            //----end reset //
        });

        function OpenAlert(messageBody) {
            
            $.confirm({
                'title': 'Message',
                'message': messageBody,
                'buttons': {
                    //'Yes': {
                    //    'class': 'blue',
                    //    'action': function () {
                    //        //elem.slideUp();
                    //    }
                    //},
                    'OK': {
                        'class': 'button',
                        'action': function () { }	// Nothing to do in this case. You can as well omit the action property.
                    }
                }
            });
            return false;
        }


        ///*****************************************Do not remove*************************************///
        ///****************************************Code for Jqplot************************************///
        //function PlotAssetAllocation(data) {

        //    var dataPlot = [[]];
        //    var labels = [];
        //    var TotVal = 0;
        //    for (var i = 0; i < data.length; i += 1) {
        //        var TextVal = [];
        //        TextVal.push(data[i].Name);
        //        TextVal.push(parseInt(data[i].Value));
        //        TotVal = TotVal + parseInt(data[i].Value);
        //        dataPlot.push(TextVal);
        //    }
        //    for (var i = 0; i < data.length; i += 1) {
        //        labels.push(data[i].Name + '  ' + ((parseInt(data[i].Value) / parseInt(TotVal)) * 100).toFixed(2) + ' %');
        //    }
        //    dataPlot.shift();
        //    $('#chartAssetAllocationContainer').html('');
        //    //debugger;
        //    var plot1 = $.jqplot('chartAssetAllocationContainer', [dataPlot], {
        //        seriesColors: ["#6481c5", "#fa6163", "#da66ce", "#69e6e0", "#fdd414"],
        //        seriesDefaults: {
        //            renderer: jQuery.jqplot.PieRenderer,
        //            rendererOptions: {
        //                showDataLabels: true,
        //                dataLabelFormatString: '%.2f%%',
        //                dataLabels: 'value',
        //                dataLabelThreshold: 0,
        //                dataLabelPositionFactor: 1.19,
        //                sliceMargin: 6,
        //                shadowDepth: 5,
        //                dataLabelCenterOn: true,
        //                dataLabelPositionFactor: 0.5,
        //                //fontSize:'0.5em',
        //            }
        //        },
        //        legend: {
        //            renderer: $.jqplot.EnhancedLegendRenderer,
        //            show: true,
        //            location: 's',
        //            fontFamily: 'Helvetica,Arial,sans-serif',
        //            fontSize: '12px',
        //            labels: labels,
        //            border: '0px',
        //            rendererOptions: {
        //                numberRows: 2,
        //                numberColumns: 2
        //            },
        //        },
        //    });
        //}
        //***********************************************End*********************************************//

    </script>

</head>
<body id="MfIframe">
    <form id="form1" runat="server">
        <div id="trOption">
            <div class="container" id="container">
                <div class="row-fluid">
                    <div class="span12 top_box" style="margin-top: 20px;">
                        <div class="span7 left_box"></div>
                        <div class="span5" style="margin-top: 15px; padding-right: 19px; text-align:justify ">
                             <p class="left_txt1" style="padding-left:10px">A smart investor understands that it is unwise to invest all your money in one type of investment instrument. It makes better sense to spread your investments across</p>
                            <ul style="line-height: 21px; margin:0; padding:0; padding-left:10px; list-style-type:none;margin-top:8px; margin-bottom:8px;">
                                <li class="highlight1">Large Caps</li>
                                <li class="highlight1">Mid Caps/Small Caps</li>
                                <li class="highlight1">Sector Funds</li>
                                <li class="highlight1">Cash/ Bank FD/ Liquid Funds</li>
                                <li  class="highlight1">FMPs & Debt Funds</li>
                            </ul>
                             <p class="left_txt1" style="padding-left:10px; padding-bottom:5px;">This is an easy to use calculator. You can slide & select your answers & then click on Calculate to see your ideal asset allocation.</p>
                        </div>
                    </div>
                    <div class="span12 gap" style="margin: 0; line-height: 45px;">
                        <div class="box1" id="Q1">
                            <asp:Label ID="lblQ1" runat="server"></asp:Label><br />
                            <div id="slider" style="width: 85%; margin-left: 33px; height: 9px;"></div>
                            <div class="span3 txt_box1" style="text-align: center">
                                <label id="lblSlider1">Under 30</label>
                            </div>
                            <div class="span1 txt_box1" style="text-align: center">
                                <label id="lblSlider2">30-40</label></div>
                            <div class="span2 txt_box1" style="text-align: center">
                                <label id="lblSlider3">41-50</label></div>
                            <div class="span2 txt_box1" style="text-align: center;">
                                <label id="lblSlider4">51-60</label></div>
                            <div class="span2 txt_box1" style="text-align: center">
                                <label id="lblSlider5">60 or over</label></div>
                        </div>
                    </div>
                    <div class="span12 gap" style="margin: 0;">
                        <div class="box2" id="Q2">
                            <asp:Label ID="lblQ2" runat="server"></asp:Label><br />
                            <asp:RadioButtonList runat="server" ID="RdbAns2" RepeatDirection="Vertical" RepeatColumns="5" Width="100%"></asp:RadioButtonList>
                            <br />
                            <br />
                            <div align="center" style="font-size: 16px; font-weight: 400;">If you dont know your risk appetite we can help you determine it. <a id="Click2RiskApp" href="#">Click Here</a></div>
                        </div>
                    </div>
                    <div class="span12 gap" style="margin: 0; ">
                        <div class="box3" id="Q3">
                            <asp:Label ID="lblQ3" runat="server"></asp:Label><br />
                            <asp:RadioButtonList runat="server" ID="RdbAns3" CssClass="testClass" RepeatDirection="Vertical" RepeatColumns="4" Width="100%"></asp:RadioButtonList>
                        </div>
                    </div>
                    <div class="span12 gap" style="margin: 0; ">
                        <div class="box4" id="Q4">
                            <asp:Label ID="lblQ4" runat="server"></asp:Label><br>
                            <asp:RadioButtonList runat="server" ID="RdbAns4" CssClass="testClass" RepeatDirection="Vertical" RepeatColumns="5" Width="100%"></asp:RadioButtonList>
                        </div>
                    </div>
                    <div class="span12" style="padding-right: 25px;">
                        <div class="span6" >
                            <div class="i-agree" style="padding-top:15px;">

                                <input type="checkbox" name="expert-disclaimer" id="disclaimer-check" />

                                 <a style="color:#54301a; font-size:14px; font-weight:700; padding-left:5px; text-decoration:underline; color: #424242; "
                                      href="#" id="LinkComents" class="" data-toggle="modal" data-target="#asset-allocator-disclaimers">I agree to the terms of the disclaimer</a>

                                <div class="clear">
                                    </div>
                            </div>
                            </div>

                       
                        <div class="span6" align="right">
                            <input type="button" id="btnClear" value="Clear" class="button" />
                            <input type="button" id="btnCal" class="button" value="Calculate" />
                        </div>
                         </div>
                    </div>
                </div>
            </div>
        <div id="trResult">
            
            <div style="height:70%;">
                &nbsp;
            </div><br />
            <div style="height:70%">
                &nbsp;
            </div>
            <div style="height:70%">
                &nbsp;
            </div>
            <div  class="container" align="center">
                <div id="DvShowProfile" class="bar">
                </div>
                <div id="chartAssetAllocationContainer" style="height: 550px; width: 100%; border: #533118 solid 2px; -webkit-border-bottom-left-radius: 10px; -moz-border-radius-bottomright: 10px; -moz-border-radius-bottomleft: 10px; border-bottom-left-radius: 10px; border-bottom-right-radius: 10px;">
                </div>
                <div>
                    <asp:HiddenField ID="hdJsonParam" runat="server" />
                    <asp:HiddenField ID="hdAssetAllocatorAge" runat="server" Value="1" />
                    <asp:HiddenField ID="hdRiskAppetite" runat="server" Value="0" />
                    <asp:HiddenField ID="hdInvestmentHorizon" runat="server" Value="0" />
                    <asp:HiddenField ID="hdYourView" runat="server" Value="0" />
                    <asp:HiddenField ID="hdUserId" runat="server" Value="" />
                    <asp:HiddenField ID="hdIsCal" runat="server" Value="no" />
                </div>
            </div>
        </div>
        <div id="asset-allocator-disclaimers" class="modal fade" tabindex="-1" role="dialog" aria-labelledby="asset-allocator-disclaimers" aria-hidden="true">
            <div class="modal-dialog" id="tblGrd">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                        <h1 class="modal-title" id="myModalLabel">Disclaimers: </h1>
                    </div>
                    <div class="modal-body" align="justify">

                        
                        <div class="" id="terms">
                            <p>
    JPMorgan Chase &amp; Co. or its affiliates and/or subsidiaries (collectively J.P. Morgan) do not warrant the completeness or accuracy of the details provided in the response and accepts no liability arising from the use of the information contained therein. They are not intended as an offer or solicitation for financial advice and for purchase or sale of any financial instrument in any jurisdiction. This is only an investor education initiative by JPMorgan Mutual Fund, for information purposes only and the views provided herein are without taking into account your objectives, financial situation or needs.&nbsp; 
  </p>
                            <p>
    The views expressed by Alpha Scholars on www.jpmalphabet.com  are their own, and not that of this website or its management. J.P. Morgan advises users to check with certified experts before taking any investment decision. However, J. P Morgan does not guarantee the accuracy, adequacy or completeness of any information and is not responsible for any errors or omissions or for the results obtained from the use of such information. J.P. Morgan is not be construed as promoting the views of the Alpha Scholars. Any investment decision taken based on the views of the Alpha Scholars is solely at the risk of the recipient. J.P. Morgan especially states that it has no financial liability whatsoever to any user on account of the use of information provided on its website.
  </p>
                            <p>
    J.P. Morgan is not an advisor to any person who may have transacted based on views of the Alpha Scholars. All financial products carry certain risks. Please read the scheme information document and product features of the respective product before investing. 
  </p>
                            <p>
    The recipient of this report must make its own independent decisions regarding any securities or financial instruments mentioned herein. 
  </p>
                            <p>
    In some products, there is a risk of losing the principal. As an investor you are advised to conduct your own verification and consult your own financial advisor before investing. By using jpmalphabet.com.com including any software and content contained therein, you agree that use of the Service is entirely at your own risk. You understand and acknowledge that there is a very high degree of risk involved in investing in securities.
  </p>
                            <p>
    Tax implications are different based on the products. J.P. Morgan accepts no liability with respect to the investment decision being taken. Please consult your tax advisor before investing.
  </p>
                            <strong>Mutual Fund investments are subject to market risks, read all scheme related documents carefully.</strong>
                            <div class="clear"></div>
                        </div>
                        <div class="clear"></div>
                    </div>
                </div>
            </div>
        </div>
        </form>
</body>
</html>