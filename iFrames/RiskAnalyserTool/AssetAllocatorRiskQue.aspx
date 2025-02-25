<%@ Page Language="C#" AutoEventWireup="true" ViewStateEncryptionMode="Always" EnableViewStateMac="true" CodeBehind="AssetAllocatorRiskQue.aspx.cs" Inherits="iFrames.RiskAnalyserTool.AssetAllocatorRiskQue" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Risk Questions</title>     
    <meta http-equiv="X-UA-Compatible" content="IE=edge"/>
    <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1"/>
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
    <link href='http://fonts.googleapis.com/css?family=Open+Sans+Condensed:300,700' rel='stylesheet' type='text/css'/>
    <script type='text/javascript' src='js/jquery-5.js'></script>
    <script type="text/javascript" src="js/jquery-ui.js"></script>
    
    
    <script src="../Scripts/jqplot/jquery.jqplot.min.js" type="text/javascript"></script>
    <script src="../Scripts/jqplot/plugins/jqplot.pieRenderer.min.js" type="text/javascript"></script>
    
    <script src="js/jquery.confirn_box.js"></script>
   
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
        $(function () {
            if (/Android|webOS|iPhone|iPad|iPod|BlackBerry|IEMobile|Opera Mini/i.test(navigator.userAgent)) {
                //alert("ok");
                $("#MfIframeQA").css('overflow', 'scroll');
            }
           
            for (k = 2; k <= 19; k++) {

                $('#RdbAns' + k).replaceWith($('table').html()
           .replace(/<tbody>/gi, "")
           .replace(/<tr>/gi, "")
           .replace(/<td>/gi, "<div class='span12 txt_box1'>")
           .replace(/<\/td>/gi, "</div>")
           .replace(/<\/tr>/gi, "")
           .replace(/<\/tbody>/gi, "")
           );

            }
            // alert("asdjkfcasdg");
            for (k = 2; k <= 19; k++) {
                for (g = 0; g <= 4; g++) {
                    var len = $("label[for^='RdbAns" + k + "_" + g + "']").length;
                    if (!(len == 0 || len == undefined)) {
                        //alert(len);
                        $("label[for^='RdbAns" + k + "_" + g + "']").replaceWith($("label[for^='RdbAns" + k + "_" + g + "']").text());
                    }
                }
            }

            $('#lblSlider' + $("#hdAssetAllocatorAge").val()).attr('style', 'color:#ffb21c');
            
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
                    for (i = 1; i <= 5; i++) {
                        $('#lblSlider' + i).attr('style', 'color:white');
                    }
                    $('#lblSlider' + $('#slider').slider('value')).attr('style', 'color:#ffb21c');
                    if (event.originalEvent) {
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


            for (i = 1; i <= 7; i++) {
                $('#risk' + i).hide();
            }

            $('#risk1').show();

            $("#btnNext").click(function (ev) {
                $("#btnPrev").removeAttr('disabled');
                $("#btnPrev").attr('class', 'button');

                //now//
                //now 

                $('html, body').animate({ scrollTop: 0 }, 'slow');
                /// now
                var chkPageQueCnt=1
                $('#risk' + $("#hdCurrentRiskDiv").val()).find("div[id^='Q']").each(function (d) {
                   
                   // alert(this.value);
                    var QuestionId = this.id;
                    var CheckCount = 0;
                    //var PrevIndex = parseInt(index) - 1;
                    var test = $('#' + QuestionId + ' input:radio:checked').map(function () {
                        CheckCount = CheckCount + 1;
                        return CheckCount;
                    }).get();
                    if (CheckCount == 0) {
                        chkPageQueCnt = 0;
                        OpenAlert('Please answer all the question');
                        return false;
                    }
                });

                if (chkPageQueCnt == 0) {
                    return false;
                }
                //--now//


                $("#hdCurrentRiskDiv").val(parseInt($("#hdCurrentRiskDiv").val()) + 1);

                for (i = 1; i <= 7; i++) {
                    $('#risk' + i).hide();
                }
                $('#risk' + $("#hdCurrentRiskDiv").val()).show();
                if ($("#hdCurrentRiskDiv").val() == 7) {
                    $("#btnNext").attr('disabled', 'disabled');
                    $("#btnNext").attr('class', 'button1');
                }

                
            });
            $("#btnPrev").click(function (ev) {
                $("#btnNext").removeAttr('disabled');
                $("#btnNext").attr('class', 'button');

                $("#hdCurrentRiskDiv").val(parseInt($("#hdCurrentRiskDiv").val()) - 1);

                for (i = 1; i <= 7; i++) {
                    $('#risk' + i).hide();
                }
                $('#risk' + $("#hdCurrentRiskDiv").val()).show();
                if ($("#hdCurrentRiskDiv").val() == 1) {
                    $("#btnPrev").attr('disabled', 'disabled');
                    $("#btnPrev").attr('class', 'button1');

                }
            });

            $("#btnOk").click(function (ev) {

                var Jparam = jQuery.parseJSON($("#hdJsonParam").val());
                var TotSum = 0;
                var CurrAge = $('#slider').slider("option", "value").toString();
                switch (CurrAge) {
                    case "0":
                        TotSum = 10;
                        break;
                    case "1":
                        TotSum = 10;
                        break;
                    case "2":
                        TotSum = 8;
                        break;
                    case "3":
                        TotSum = 6;
                        break;
                    case "4":
                        TotSum = 4;
                        break;
                    case "5":
                        TotSum = 2;
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
                if (CheckCount < 18) {
                    OpenAlert("Please answer the entire question");
                    return false;
                }
                var modVal = Math.round((TotSum / 156) * 100, 2);
                var InvestProfile = "";
                var RiskAppetiteValue = 0;
                var DataAssetAllocation = [[]];
                var DataRecAssetAllocation = [[]];
                for (i = 0; i < Jparam.length ; i++) {
                    if (Jparam[i].StartVal <= modVal && Jparam[i].EndVal >= modVal) {
                        InvestProfile = Jparam[i].Name;
                        RiskAppetiteValue = Jparam[i].RiskAppetiteValue;
                        //$("#DvShowProfile").html(InvestProfile + "-(" + modVal + ")");
                        break;
                    }
                }
                var url = "/RiskAnalyserTool/AssetAllocator.aspx?user_id=" + $("#hdUserId").val() + "&CurrentAge=" + $("#hdAssetAllocatorAge").val() + "&RiskAppetite=" + RiskAppetiteValue + "&InvestmentHorizon=" + $("#hdInvestmentHorizon").val() + "&Yourview=" + $("#hdYourView").val() + "&IsCal=yes";
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
    </script>
</head>
<body id="MfIframeQA">
    <form id="form1" runat="server">
        <div class="container" id="Div1">
            <div class="row-fluid" id="container">
                <div class="span12 top_box" style="margin-top: 20px;margin-bottom:15px;">
                    <div class="span7 left_box1"></div>
                    <div class="span5" style="margin-top: 15px; padding-right: 15px; padding-left: 5px;">
                        <span class="left_txt1" style="margin-bottom: 15px; font-size: 26px;">
                            
                            Risk Analyser Tool
                            <br/>
                        </span>
                        <p class="left_txt1" style="word-spacing: 0; text-align: left; padding-top:15px;">
                            Risk Profiling combines two key areas:<br/>
                            <p class="highlight1">Estimating financial risk-taking capacity</p>
                            <p class="highlight1">Understanding the (psychological) risk <br />tolerance level of an individual.</p>
                        </p>
                        <%--<table width="100%" style="margin: 0; padding: 0; font-size: 22px; line-height: 30px;">
                            <tr>
                                <td style="vertical-align: top;"><img src="img/arrow.png"></td>
                                <td>Estimating financial risk-taking capacity and</td>
                            </tr>
                            <tr>
                                <td style="vertical-align: top;"><img src="img/arrow.png"></td>
                                <td>Understanding the (psychological) risk tolerance level of an individual.</td>
                            </tr>
                        </table>--%>
                        <%--<div class="row-fluid">
                        <div class="span1" style="margin-top:8px;margin-left:3px" ><img src="img/arrow.png"></div
                        <div class="span11 highlight">Estimating financial risk-taking capacity</div><br />
                        </div>
                        <div class="row-fluid">
                        <div class="span1" style="margin-top:22px;margin-left:3px" ><img src="img/arrow.png"></div>
                        <div  class="span10" style="margin: 0;margin-top:14px; padding: 0; font-size: 22px; line-height: 30px;">Understanding the (psychological) risk tolerance level of an individual.</div>
                    </div>--%>
                        </div>                   


                </div>
                <div style="font-size: 16px; line-height: 23px; color: #54301a; padding-top: 15px; padding-bottom: 10px; text-align:justify; font-weight:700;">
                        <br />We have compiled 19 questions that will help you evaluate yourself on both these parameters. Also,
                         based on your risk profile, we will recommend an asset allocation structure best suited for you.
                    </div>
                <div id="risk1">
                    <div class="span12 gap" style="margin: 0; line-height: 45px; padding-top:5px;">
                        <div class="box1b">
                            <asp:Label ID="lblQ1" runat="server"></asp:Label><br />
                            <div id="slider" style="width: 85%; margin-left: 33px; height: 9px;"></div>
                            <div class="span3 txt_box1" style="text-align: center">
                                <label id="lblSlider1">Under 30</label>
                            </div>
                            <div class="span1 txt_box1" style="text-align: center">
                                <label id="lblSlider2">30-40</label>
                            </div>
                            <div class="span2 txt_box1" style="text-align: center">
                                <label id="lblSlider3">41-50</label>
                            </div>
                            <div class="span2 txt_box1" style="text-align: center">
                                <label id="lblSlider4">51-60</label>
                            </div>
                            <div class="span2 txt_box1" style="text-align: center">
                                <label id="lblSlider5">60 or over</label>
                            </div>
                        </div>
                    </div>
                    <div class="span12 gap" style="margin: 0; line-height: 45px;">
                        <div class="box2a" id="Q2">
                            <asp:Label ID="lblQ2" runat="server"></asp:Label><br />
                            <asp:RadioButtonList runat="server" ID="RdbAns2" RepeatDirection="Vertical" RepeatColumns="4" Width="100%"></asp:RadioButtonList>
                        </div>
                    </div>
                    <div class="span12 gap" style="margin: 0; line-height: 45px;">
                        <div class="box3b" id="Q3">
                            <asp:Label ID="lblQ3" runat="server"></asp:Label><br />
                            <asp:RadioButtonList runat="server" ID="RdbAns3" RepeatDirection="Vertical" RepeatColumns="5" Width="100%"></asp:RadioButtonList>
                        </div>
                    </div>
                </div>
                <div id="risk2">
                    <div class="span12 gap" style="margin: 0;">
                        <div class="box1c" id="Q4">
                            <div class="span1" style="width: 0.3%;">4.</div>
                            <div class="span11">
                                <asp:Label ID="lblQ4" runat="server"></asp:Label></div>
                            <br />
                            <asp:RadioButtonList runat="server" ID="RdbAns4" RepeatDirection="Vertical" RepeatColumns="4" Width="100%"></asp:RadioButtonList>
                        </div>
                    </div>
                    <div class="span12 gap" style="margin: 0; line-height: 45px;">
                        <div class="box2a" id="Q5">
                            <asp:Label ID="lblQ5" runat="server"></asp:Label><br />
                            <asp:RadioButtonList runat="server" ID="RdbAns5" RepeatDirection="Vertical" RepeatColumns="4" Width="100%"></asp:RadioButtonList>
                        </div>
                    </div>
                    <div class="span12 gap" style="margin: 0; line-height: 45px;">
                        <div class="box3a" id="Q6">
                            <asp:Label ID="lblQ6" runat="server"></asp:Label><br />
                            <asp:RadioButtonList runat="server" ID="RdbAns6" RepeatDirection="Vertical" RepeatColumns="4" Width="100%"></asp:RadioButtonList>
                        </div>
                    </div>
                </div>
                <div id="risk3">
                    <div class="span12 gap" style="margin: 0; line-height: 45px;">
                        <div class="box1a" id="Q7">
                            <asp:Label ID="lblQ7" runat="server"></asp:Label><br />
                            <asp:RadioButtonList runat="server" ID="RdbAns7" RepeatDirection="Vertical" RepeatColumns="4" Width="100%"></asp:RadioButtonList>
                        </div>
                    </div>
                    <div class="span12 gap" style="margin: 0; line-height: 45px;">
                        <div class="box2a" id="Q8">
                            <asp:Label ID="lblQ8" runat="server"></asp:Label><br />
                            <asp:RadioButtonList runat="server" ID="RdbAns8" RepeatDirection="Vertical" RepeatColumns="4" Width="100%"></asp:RadioButtonList>
                        </div>
                    </div>
                    <div class="span12 gap" style="margin: 0; line-height: 45px;">
                        <div class="box3a" id="Q9">
                            <asp:Label ID="lblQ9" runat="server"></asp:Label><br />
                            <asp:RadioButtonList runat="server" ID="RdbAns9" RepeatDirection="Vertical" RepeatColumns="4" Width="100%"></asp:RadioButtonList>
                        </div>
                    </div>
                </div>
                <div id="risk4">
                    <div class="span12 gap" style="margin: 0; line-height: 45px;">
                        <div class="box1a" id="Q10">
                            <asp:Label ID="lblQ10" runat="server"></asp:Label><br />
                            <asp:RadioButtonList runat="server" ID="RdbAns10" RepeatDirection="Vertical" RepeatColumns="4" Width="100%"></asp:RadioButtonList>
                        </div>
                    </div>
                    <div class="span12 gap" style="margin: 0; line-height: 45px;">
                        <div class="box2b" id="Q11">
                            <asp:Label ID="lblQ11" runat="server"></asp:Label><br />
                            <asp:RadioButtonList runat="server" ID="RdbAns11" RepeatDirection="Vertical" RepeatColumns="5" Width="100%"></asp:RadioButtonList>
                        </div>
                    </div>
                    <div class="span12 gap" style="margin: 0; line-height: 45px;">
                        <div class="box3b" id="Q12">
                            <asp:Label ID="lblQ12" runat="server"></asp:Label><br />
                            <asp:RadioButtonList runat="server" ID="RdbAns12" RepeatDirection="Vertical" RepeatColumns="5" Width="100%"></asp:RadioButtonList>
                        </div>
                    </div>
                </div>
                <div id="risk5">
                    <div class="span12 gap" style="margin: 0; line-height: 45px;">
                        <div class="box1a" id="Q13">
                            <asp:Label ID="lblQ13" runat="server"></asp:Label><br />
                            <asp:RadioButtonList runat="server" ID="RdbAns13" RepeatDirection="Vertical" RepeatColumns="4" Width="100%"></asp:RadioButtonList>
                        </div>
                    </div>
                    <div class="span12 gap" style="margin: 0; line-height: 45px;">
                        <div class="box2b" id="Q14">
                            <asp:Label ID="lblQ14" runat="server"></asp:Label><br />
                            <asp:RadioButtonList runat="server" ID="RdbAns14" RepeatDirection="Vertical" RepeatColumns="5" Width="100%"></asp:RadioButtonList>
                        </div>
                    </div>
                    <div class="span12 gap" style="margin: 0; line-height: 45px;">
                        <div class="box3b" id="Q15">
                            <asp:Label ID="lblQ15" runat="server"></asp:Label><br />
                            <asp:RadioButtonList runat="server" ID="RdbAns15" RepeatDirection="Vertical" RepeatColumns="5" Width="100%"></asp:RadioButtonList>
                        </div>
                    </div>
                </div>
                <div id="risk6">
                    <div class="span12 gap" style="margin: 0; line-height: 45px;">
                        <div class="box1a" id="Q16">
                            <asp:Label ID="lblQ16" runat="server"></asp:Label><br />
                            <asp:RadioButtonList runat="server" ID="RdbAns16" RepeatDirection="Vertical" RepeatColumns="4" Width="100%"></asp:RadioButtonList>
                        </div>
                    </div>
                    <div class="span12 gap" style="margin: 0; line-height: 45px;">
                        <div class="box2b" id="Q17">
                            <asp:Label ID="lblQ17" runat="server"></asp:Label><br />
                            <asp:RadioButtonList runat="server" ID="RdbAns17" RepeatDirection="Vertical" RepeatColumns="5" Width="100%"></asp:RadioButtonList>
                        </div>
                    </div>
                    <div class="span12 gap" style="margin: 0; line-height: 45px;">
                        <div class="box3a" id="Q18">
                            <asp:Label ID="lblQ18" runat="server"></asp:Label><br />
                            <asp:RadioButtonList runat="server" ID="RdbAns18" RepeatDirection="Vertical" RepeatColumns="4" Width="100%"></asp:RadioButtonList>
                        </div>
                    </div>
                </div>
                <div id="risk7">
                    <div class="span12 gap" style="margin: 0; line-height: 45px;">
                        <div class="box1c" id="Q19">
                            <asp:Label ID="lblQ19" runat="server"></asp:Label><br />
                            <asp:RadioButtonList runat="server" ID="RdbAns19" RepeatDirection="Vertical" RepeatColumns="5" Width="100%"></asp:RadioButtonList>
                        </div>
                    </div>
                </div>
                <div class="span12" align="right" style="padding-right: 0px;width: 97%;">
                    <input type="button" value="Prev" class="button1" id="btnPrev" />
                    <input type="button" class="button" value="Next" id="btnNext" />
                    <input type="button" class="button" style="width: 150px" value="Measure Risk" id="btnOk" />
                    <asp:HiddenField ID="hdJsonParam" runat="server" />
                    <asp:HiddenField ID="hdAssetAllocatorAge" runat="server" />
                    <asp:HiddenField ID="hdRiskAppetite" runat="server" />
                    <asp:HiddenField ID="hdInvestmentHorizon" runat="server" />
                    <asp:HiddenField ID="hdYourView" runat="server" />
                    <asp:HiddenField ID="hdUserId" runat="server" Value="" />
                    <input type="hidden" id="hdCurrentRiskDiv" value="1" />
                </div>
            </div>
        </div>
        <%--<div class="span11"></div>--%>
        
    </form>
</body>
</html>
