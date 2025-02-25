<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AssetAllocator.aspx.cs" Inherits="iFrames.PrismAdvisory.AssetAllocator" %>

<!DOCTYPE html>

<html lang="en">

<head>
  <meta charset="utf-8">
  <meta http-equiv="X-UA-Compatible" content="IE=edge">
  <meta name="viewport" content="width=device-width, initial-scale=1">
  <meta name="description" content="">
  <meta name="author" content="">
  <link rel="shortcut icon" href="assets/ico/favicon.ico">

  <title>Prism Advisory</title>

  <!-- CSS Plugins -->
  <link rel="stylesheet" href="font-awesome-4.7.0/css/font-awesome.min.css">
  <link rel="stylesheet" href="css/pe-icon-7-stroke.css">
  <!-- CSS Global -->
  <link rel="stylesheet" href="css/style.css">
    
  <link rel='stylesheet' id='ratio_edge_google_fonts-css'  href='http://fonts.googleapis.com/css?family=Open+Sans%3A100%2C100italic%2C200%2C200italic%2C300%2C300italic%2C400%2C400italic%2C500%2C500italic%2C600%2C600italic%2C700%2C700italic%2C800%2C800italic%2C900%2C900italic%7CRoboto%3A100%2C100italic%2C200%2C200italic%2C300%2C300italic%2C400%2C400italic%2C500%2C500italic%2C600%2C600italic%2C700%2C700italic%2C800%2C800italic%2C900%2C900italic&#038;subset=latin%2Clatin-ext&#038;ver=1.0.0' type='text/css' media='all' />

     <script src="js/jquery.min.js"></script>
    
    <script src="js/bootstrap.min.js"></script>
    <script src="js/jquery.confirn_box.js"></script>
    <!-- JS Plugins -->
    <script src="js/perfect-scrollbar.jquery.min.js"></script>
    <script src="js/bootstrap-slider.min.js"></script>

    <!-- JS Custom -->
    <script src="js/theme.min.js"></script>
    <script src="js/kite.min.js"></script>
   
 <script type="text/javascript" src="js/jsapi.js"></script>
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
    <!-- JavaScript
    ================================================== -->
     
    <!-- JS Global -->
  
     <script type="text/javascript">

        google.load("visualization", "1", { packages: ["corechart"] });
         function drawChart(data) {
             //debugger;
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
                 titleTextStyle: { fontName: ['Roboto+Condensed:400,700,300:latin'], fontSize: 18 },
                 //titlePosition:'centre',
                 backgroundColor: { stroke: "none", strokeWidth: 5, fill: "none" },
                 colors: ['#617EC2', '#FB6163', '#DC69D0', '#69E6E0', '#FDD514'],
                 is3D: true,
                 //height: 500,
                 chartArea: { left: '10%', top: 100, width: '80%', height: '60%' },
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
                     textStyle: { fontName: ['Roboto+Condensed:400,700,300:latin'], width: 20, fontSize: 14 }
                 },

                 //legend: { position: 'bottom', textStyle: { color: 'blue', fontSize: 16 }, maxLines: 5 },
             };

             var chart = new google.visualization.PieChart(document.getElementById('chartAssetAllocationContainer'));
             chart.draw(data1, options);
             //$("text:contains(" + options.title + ")").attr({ 'x': '380', 'y': '20' })
             $("text:contains(" + options.title + ")").attr({ 'x': titlePositionLeft, 'y': '500' })
         }
         $(function () {
             //debugger;
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
                 .replace(/<td/gi, "<div class='col-lg-2 txt_box1 gap'")
             .replace(/<\/td>/gi, "</div>")
             .replace(/<\/tr>/gi, "")
             .replace(/<\/tbody>/gi, "")
             );

             $('#RdbAns3').replaceWith($('table').html()
            .replace(/<tbody>/gi, "")
            .replace(/<tr>/gi, "")
                 .replace(/<td/gi, "<div class='col-lg-3 txt_box1 gap'")
            .replace(/<\/td>/gi, "</div>")
            .replace(/<\/tr>/gi, "")
            .replace(/<\/tbody>/gi, "")
            );
             $('label[for="RdbAns3_0"]').parent().removeAttr('class');
             $('label[for="RdbAns3_0"]').parent().attr('class', 'col-lg-2 txt_box1 gap');
             $('#RdbAns4').replaceWith($('table').html()
             .replace(/<tbody>/gi, "")
             .replace(/<tr>/gi, "")
                 .replace(/<td/gi, "<div class='col-lg-2 txt_box1 gap'")
             .replace(/<\/td>/gi, "</div>")
             .replace(/<\/tr>/gi, "")
             .replace(/<\/tbody>/gi, "")
             );
             $("#RdbAns2_0").parent().removeClass('col-lg-2 txt_box1 gap').addClass('col-lg-3 txt_box1 gap');
             $("#RdbAns3_0").parent().removeClass('col-lg-2 txt_box1 gap').addClass('col-lg-3 txt_box1 gap');
             $("#RdbAns4_0").parent().removeClass('col-lg-2 txt_box1 gap').addClass('col-lg-3 txt_box1 gap');
             $("#RdbAns4_4").parent().removeClass('col-lg-2 txt_box1 gap').addClass('col-lg-3 txt_box1 gap');
             // end responsive


             //var labelArr = new Array("--", "Under 30", "30-40", "41-50", "51-60", "60 or over", "");
             //$('#lblSlider1').attr('style', 'color:#ffb21c');

             //$("#slider").slider({
             //    value: $("#hdAssetAllocatorAge").val(),
             //    min: 0,
             //    max: 6,
             //    step: 1,
             //    range: "max",
             //    slide: function (event, ui) {
             //        //$("#days").val(ui.value);
             //        //$("#label").html(labelArr[ui.value]);
             //    },
             //    change: function (event, ui) {
             //        if (event.originalEvent) {
             //            for (i = 1; i <= 5; i++) {
             //                $('#lblSlider' + i).attr('style', 'color:white');
             //            }
             //            $('#lblSlider' + $('#slider').slider('value')).attr('style', 'color:#ffb21c');

             //            if ($('#slider').slider('value') == 0) {
             //                $('#lblSlider1').attr('style', 'color:#ffb21c');
             //                $("#slider").slider({
             //                    value: 1,
             //                    min: 0,
             //                    max: 6,
             //                    step: 1,
             //                    range: "max",
             //                    slide: function (event, ui) {
             //                        //$("#days").val(ui.value);
             //                        //$("#label").html(labelArr[ui.value]);
             //                    },
             //                });
             //            }
             //            if ($('#slider').slider('value') == 6) {
             //                $('#lblSlider5').attr('style', 'color:#ffb21c');
             //                $("#slider").slider({
             //                    value: 5,
             //                    min: 0,
             //                    max: 6,
             //                    step: 1,
             //                    range: "max",
             //                    slide: function (event, ui) {
             //                        //$("#days").val(ui.value);
             //                        //$("#label").html(labelArr[ui.value]);
             //                    },
             //                });
             //            }
             //        }
             //    },
             //});

             $("#btnCal").click(function (ev) {
                 //debugger;
                 
                 $("#chartAssetAllocationContainer").animate({ scrollTop: 0 }, 'slow');                 
                 var Jparam = jQuery.parseJSON($("#hdJsonParam").val());
                 var CurrAge = $('#ex8').val();
                 var ApiAge = "";

                 var TotSum = 0;

                 if (CurrAge < 31)
                 {
                     TotSum = 10;
                     ApiAge = "Under 30";
                 }
                 else if (CurrAge >= 31 && CurrAge <= 40)
                 {
                     TotSum = 8;
                     ApiAge = "31-40";
                 }
                 else if (CurrAge >= 41 && CurrAge <= 50) {
                     TotSum = 6;
                     ApiAge = "41-50";
                 }
                 else if (CurrAge >= 51 && CurrAge <= 60) {
                     TotSum = 4;
                     ApiAge = "51-60";
                 }
                 else if (CurrAge >= 61 ) {
                     TotSum = 2;
                     ApiAge = "Over 60";
                 }
                 
               // switch (CurrAge) {
               //     case "0":
               //         TotSum = 10;
               //         break;
               //     case "1":
               //         TotSum = 10;
               //         ApiAge = "Under 30";
               //         break;
               //     case "2":
               //         TotSum = 8;
               //         ApiAge = "31-40";
               //         break;
               //     case "3":
               //         TotSum = 6;
               //         ApiAge = "41-50";
               //         break;
               //     case "4":
               //         TotSum = 4;
               //         ApiAge = "51-60";
               //         break;
               //     case "5":
               //         TotSum = 2;
               //         ApiAge = "Over 60";
               //         break;
               //     case "6":
               //         TotSum = 2;
               //         break;
               // }


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

               // if (!$("#checkbox_1").is(':checked')) {
               //      OpenAlert("Please agree to the terms of the disclaimer");
               //    // alert('Please agree to the terms of the disclaimer');
               //     return false;
               // }
               
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
                             if (v == Jparam[i].LstAsset.length - 1) {
                                 ApiResult = ApiResult + strVal + "=" + Jparam[i].LstAsset[v].Value;
                             }
                             else {
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
                             url: ApiURL,
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

            
             $("input[type='radio']").click(function (event) {
                 //alert(event.target.id);
                 //debugger;
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
                         //alert('Please answer previous question');
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
                 // $('#lblSlider1').attr('style', 'color:#ffb21c');
                 var mySlider = $("#ex8").slider();
                 mySlider.slider('setValue', 40);
                // $('#ex8').val('40');
                // $("#checkbox_1").prop("checked", false);
              // $("#slider").slider({
              //     value: $("#hdAssetAllocatorAge").val(),
              //     min: 0,
              //     max: 6,
              //     step: 1,
              //     range: "max",
              //     change: function (event, ui) {
              //         if (event.originalEvent) {
              //             for (i = 1; i <= 5; i++) {
              //                 $('#lblSlider' + i).attr('style', 'color:white');
              //             }
              //             $('#lblSlider' + $('#slider').slider('value')).attr('style', 'color:#ffb21c');
              //
              //             if ($('#slider').slider('value') == 0) {
              //                 $('#lblSlider1').attr('style', 'color:#ffb21c');
              //                 $("#slider").slider({
              //                     value: 1,
              //                     min: 0,
              //                     max: 6,
              //                     step: 1,
              //                     range: "max",
              //                 });
              //             }
              //             if ($('#slider').slider('value') == 6) {
              //                 $('#lblSlider5').attr('style', 'color:#ffb21c');
              //                 $("#slider").slider({
              //                     value: 5,
              //                     min: 0,
              //                     max: 6,
              //                     step: 1,
              //                     range: "max",
              //                 });
              //             }
              //         }
              //     },
              // });

             });

             //----end reset //
         });

         function OpenAlert(messageBody) {

             $.confirm({
                 'title': 'Message',
                 'message': messageBody,
                 'buttons': {
                   
                     'OK': {
                         'class': 'button',
                         'action': function () { }	
                     }
                 }
             });
             return false;
         }


       

    </script>
</head>

<body>
 <form id="form1" runat="server">
  <!-- NAVBAR
    ================================================== -->


  <!-- WRAPPER
    ================================================== -->
  <div class="wrapper">

 
    <div class="container-fluid">
      <!-- / .row -->
      <div class="row">
       
        <div class="col-xs-12 col-lg-8 m-t-xs" id="trOption">
            <div class="alert">
              <strong>1. Your age is?</strong> 
              <p class="m-t-md">
                <input id="ex8" data-slider-id='ex1Slider' type="text" data-slider-min="0" data-slider-max="100" data-slider-step="1" data-slider-value="40"/>
              </p>
            </div>
            <div  id="container">
                <div class="alert">
                    <strong>2. What is your Risk Appetite?</strong>

                    <div class="form-group m-t-md" style="padding-bottom: 5px;">
                        <div class="col-sm-12" id="Q2">
                           
                            <asp:Label ID="lblQ2" runat="server"></asp:Label>
                            <asp:RadioButtonList runat="server" ID="RdbAns2" RepeatDirection="Vertical" RepeatColumns="5" Width="100%"></asp:RadioButtonList>
                           
                        </div>
                    </div>

                </div>
                <div class="alert">
                    <strong>3. What is your Investment Horizon?</strong>

                    <div class="form-group m-t-md" style="padding-bottom: 5px;">
                        <div class="col-sm-12" id="Q3">
                            <asp:Label ID="lblQ3" runat="server"></asp:Label>
                            <asp:RadioButtonList runat="server" ID="RdbAns3" CssClass="testClass" RepeatDirection="Vertical" RepeatColumns="4" Width="100%"></asp:RadioButtonList>

                        </div>
                    </div>

                </div>
                <div class="alert">
                    <strong>4. Your view on the future economic growth of India</strong>

                    <div class="form-group m-t-md" style="padding-bottom: 5px;">
                        <div class="col-sm-12" id="Q4">
                            <asp:Label ID="lblQ4" runat="server"></asp:Label>
                            <asp:RadioButtonList runat="server" ID="RdbAns4" CssClass="testClass" RepeatDirection="Vertical" RepeatColumns="5" Width="100%"></asp:RadioButtonList>

                        </div>
                    </div>

                </div>
            </div>
            <div>
               <%-- <div class="col-xs-12 col-lg-5">
                   
                        <div class="" align="center">
                            <input type="checkbox" id="checkbox_1">
                           
                                 <a style="color:#54301a; font-size:14px; font-weight:700; padding-left:5px; text-decoration:underline; color: #424242; "
                                      href="#" id="LinkComents" class="" data-toggle="modal" data-target="#asset-allocator-disclaimers">
                                     I agree to the terms of the disclaimer</a>

                           
                        </div>
                  
                </div>--%>
                <div class="col-xs-12 col-lg-7" align="right" style="padding-right:0;">
                    <div class="btn-group" role="group" aria-label="Justified button group with nested dropdown">
                        <a href="javascript:void(0)"id="btnCal" class="btn btn-primary" role="button">Calculate</a>
                        <a href="javascript:void(0)"id="btnClear"  class="btn btn-primary" role="button">Clear</a>

                    
                    </div>
                </div>
            </div>
            
        </div>
        <div class="col-xs-12 col-lg-4"></div>

      </div>
        
          <div class="" id="trResult">
              <div class="container" align="center">
                  <div id="DvShowProfile" class="bar">
                  </div>
                  <div id="chartAssetAllocationContainer" style="height: 550px; width: 100%; border: #cc0000 solid 2px; -webkit-border-bottom-left-radius: 10px; -moz-border-radius-bottomright: 10px; -moz-border-radius-bottomleft: 10px; border-bottom-left-radius: 10px; border-bottom-right-radius: 10px;">
                  </div>
                  <asp:HiddenField ID="hdJsonParam" runat="server" />
                    <asp:HiddenField ID="hdAssetAllocatorAge" runat="server" Value="1" />
                    <asp:HiddenField ID="hdRiskAppetite" runat="server" Value="0" />
                    <asp:HiddenField ID="hdInvestmentHorizon" runat="server" Value="0" />
                    <asp:HiddenField ID="hdYourView" runat="server" Value="0" />
                    <asp:HiddenField ID="hdUserId" runat="server" Value="" />
                    <asp:HiddenField ID="hdIsCal" runat="server" Value="no" />
              </div>
              
                <!-- Small modal -->
    
        </div>
            
        <!-- Footer -->
        <footer class="page__footer m-t-lg">
          <div class="row">
            <div class="col-xs-12">
              <span class="page__footer__year"></span> Developed for Prism Advisory by: <a href="https://www.icraanalytics.com" target="_blank" >ICRA Analytics Ltd</a> &nbsp;<a style="font-size: 10px; color: #999999" href="https://icraanalytics.com/home/Disclaimer" target="_blank" >Disclaimer</a>
            </div>
          </div>
        </footer>

     
      <!-- / .container-fluid -->

    </div>
   </div>
    <!-- / .wrapper -->

<div class="modal fade" id="asset-allocator-disclaimers" tabindex="-1" role="dialog" aria-labelledby="modal_small__heading">
      <div class="modal-dialog modal-md" role="document">
        <div class="modal-content">
          <div class="modal-header">
            <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
            <h4 class="modal-title" id="modal_small__heading">Disclaimers:</h4>
          </div>
          <div class="modal-body">
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
          </div>
          <div class="modal-footer">
            <button type="button" class="btn btn-primary" data-dismiss="modal">Close</button>
            <button type="button" class="btn btn-primary">Save changes</button>
          </div>
        </div>
      </div>
    </div>
    
 </form>
</body>

</html>
