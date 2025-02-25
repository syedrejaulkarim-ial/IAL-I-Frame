<%@ Page Language="C#" AutoEventWireup="true" ViewStateEncryptionMode="Always" EnableViewStateMac="true" CodeBehind="DSPtaxCalc.aspx.cs"
    Inherits="iFrames.DSP.DSPtaxCalc" EnableEventValidation="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Tax Calculator</title>
    <link href="css/master.css" rel="stylesheet" type="text/css" media="all" />
    <script src="../Scripts/jquery-19.js" type="text/javascript"></script>
    <script src="Script/jquery.number.js" type="text/javascript"></script>
    <script src="Script/check.js" type="text/javascript"></script>
    <script type="text/javascript" language="javascript">


        $(function () {


            $('#txtuser').blur(function () {

                calculateRgess();
            });

            $('#salaryInput').blur(function () {

                //SetConfigXml();
                var salinput = Number($('#salaryInput').val());


                if (salinput > maxIncomeLimit) {
                    alert("You can invest in RGESS but a tax benefit can only be availed by individuals with a gross total income of <= " + maxIncomeLimit);
                    // $('#salaryInput').val(300000);
                    //return false;
                }

                var benift = regessTaxBenefit(Number($('#txtRgess').val()));

                $('#txtbenefitRgess').val(benift);
                calculateRgess();
            });

            $('#licInput').blur(function () {
                var licinput = Number($('#licInput').val());
                var salinput = Number($('#salaryInput').val());

                //                if (licinput > salinput) {
                //                    alert('Investment Amount cannot exceed Income');
                //                    $('#licInput').val('0'); $('#licInputWo').val('0');
                //                    return;
                //                }

                if (licinput > maxTaxExempt80c) {
                    // alert('You wil only get Tax Exemption upto' + maxTaxExempt80c);
                }

                $('#licInputWo').val($('#licInput').val());
                calculateRgess();
            });


            $('#medInput').blur(function () {

                var salinput = Number($('#salaryInput').val());
                var medcinput = Number($('#medInput').val());

                //                if (medcinput > salinput) {
                //                    alert('Medical Claim Amount cannot exceed Income');
                //                    $('#medInput').val('0'); $('#medInputWo').val('0');
                //                    return;
                //                }

                if (medcinput > maxMedicalClaim) {
                    //alert('You wil only get Tax Exemption upto' + maxMedicalClaim);
                }

                $('#medInputWo').val($('#medInput').val());
                calculateRgess();
            });

            $('#txtRgess').blur(function () {
                var benift = regessTaxBenefit(Number($('#txtRgess').val()));
                $('#txtbenefitRgess').val(benift);
                calculateRgess();
            });

            // ReadXml();

            $('#textAge').blur(function () {

                if ($('#textAge').val() >= 80) {
                    $("#rowtaxFreelimit").css("display", "none");
                }
                else
                    $("#rowtaxFreelimit").css("display", "");

                // $("#rowtaxFreelimit").css("display", "inline");

                //SetConfig();
                SetConfigXml();
                calculateRgess();
            });


            $('#txtHomeLoanInt').blur(function () {

                var salinput = Number($('#salaryInput').val());
                var txthloanint = Number($('#txtHomeLoanInt').val());

                //                if (txthloanint > salinput) {
                //                    alert('Home Loan Interest Amount cannot exceed Income');
                //                    $('#txtOtherinput').val('0'); $('#txtOtherinputWo').val('0');
                //                    return;
                //                }

                $('#txtHomeLoanIntWo').val($('#txtHomeLoanInt').val());
                calculateRgess();
            });




            //SetConfig();
            SetConfigXml();
            $('.row_txtbx').number(true, 0);
            $('.row_txtbx1').number(true, 0);
            $('.row_txtbx2').number(true, 0);
            $('.row_txtbx3').number(true, 0);
            $('.lebelnumberformat').number(true, 0);

        });


        function SetConfigXml() {
            // g_maxIncomeLimit = null;

            $.ajax({
                type: "GET",
                url: "Excel/DspTaxConfig.xml",
                dataType: "xml",
                async: false,
                success: function (xml) {

                    maxIncomeLimit = parseInt($(xml).find("MaxIncomeLimit").text());
                    maxRgessLimit = parseInt($(xml).find("RgessLimit").text());
                    maxRgessExempt = parseInt($(xml).find("RgessTaxExempt").text());
                    maxTaxExempt80c = parseInt($(xml).find("TaxExempt80c").text());
                    eduCess = parseInt($(xml).find("EducationCessRate").text());
                    maxTaxExemptOthers = parseInt($(xml).find("TaxExemptOthers").text());

                    var age = $('#textAge').val();

                    if (age == "" || age < 60) {

                        maxTaxLimit = parseInt($(xml).find('Age60below').find("TaxFreeLimit")[0].firstChild.nodeValue);
                        maxMedicalClaim = parseInt($(xml).find('Age60below').find("MedicalClaim")[0].firstChild.nodeValue);

                        // maxTaxSlab1 = parseInt($(xml).find('Age60below').find('TaxSlab').find("TaxLimit").find('SalaryRange')[0].textContent);
                        //  maxTaxSlab1TaxRate = parseInt($(xml).find('Age60below').find('TaxSlab').find("TaxLimit").find('Taxrate')[0].textContent);
                        //                        $(xml).find('Age60below').find('TaxSlab').find("TaxLimit").each(function () {
                        //                            var l_TaxLimit = $(this).find('SalaryRange').text();//                          
                        //                            var l_Taxrate = $(this).find('Taxrate').text();
                        //                        });

                        $l_TaxLimit = $(xml).find('Age60below').find('TaxSlab').find("TaxLimit");

                        maxTaxSlab1 = parseInt($l_TaxLimit.find('SalaryRange')[0].firstChild.nodeValue);
                        maxTaxSlab1TaxRate = parseInt($l_TaxLimit.find('Taxrate')[0].firstChild.nodeValue);

                        maxTaxSlab2 = parseInt($l_TaxLimit.find('SalaryRange')[1].firstChild.nodeValue);
                        maxTaxSlab2TaxRate = parseInt($l_TaxLimit.find('Taxrate')[1].firstChild.nodeValue);

                        maxTaxSlab3 = parseInt($l_TaxLimit.find('SalaryRange')[2].firstChild.nodeValue);
                        maxTaxSlab3TaxRate = parseInt($l_TaxLimit.find('Taxrate')[2].firstChild.nodeValue);
                    }
                    else if (age >= 60 && age < 80) {

                        $l_TaxLimit2 = $(xml).find('Age80below').find('TaxSlab').find("TaxLimit");

                        maxTaxLimit = parseInt($(xml).find('Age80below').find("TaxFreeLimit")[0].firstChild.nodeValue);
                        maxMedicalClaim = parseInt($(xml).find('Age80below').find("MedicalClaim")[0].firstChild.nodeValue);

                        maxTaxSlab1 = parseInt($l_TaxLimit2.find('SalaryRange')[0].firstChild.nodeValue);
                        maxTaxSlab1TaxRate = parseInt($l_TaxLimit2.find('Taxrate')[0].firstChild.nodeValue);

                        maxTaxSlab2 = parseInt($l_TaxLimit2.find('SalaryRange')[1].firstChild.nodeValue);
                        maxTaxSlab2TaxRate = parseInt($l_TaxLimit2.find('Taxrate')[1].firstChild.nodeValue);

                        maxTaxSlab3 = parseInt($l_TaxLimit2.find('SalaryRange')[2].firstChild.nodeValue);
                        maxTaxSlab3TaxRate = parseInt($l_TaxLimit2.find('Taxrate')[2].firstChild.nodeValue);

                    }
                    else {
                        $l_TaxLimit3 = $(xml).find('Age80Above').find('TaxSlab').find("TaxLimit");

                        maxTaxLimit = parseInt($(xml).find('Age80Above').find("TaxFreeLimit")[0].firstChild.nodeValue);
                        maxMedicalClaim = parseInt($(xml).find('Age80Above').find("MedicalClaim")[0].firstChild.nodeValue);

                        maxTaxSlab1 = parseInt($l_TaxLimit3.find('SalaryRange')[0].firstChild.nodeValue);
                        maxTaxSlab1TaxRate = parseInt($l_TaxLimit3.find('Taxrate')[0].firstChild.nodeValue);

                        maxTaxSlab2 = parseInt($l_TaxLimit3.find('SalaryRange')[1].firstChild.nodeValue);
                        maxTaxSlab2TaxRate = parseInt($l_TaxLimit3.find('Taxrate')[1].firstChild.nodeValue);

                        maxTaxSlab3 = parseInt($l_TaxLimit3.find('SalaryRange')[2].firstChild.nodeValue);
                        maxTaxSlab3TaxRate = parseInt($l_TaxLimit3.find('Taxrate')[2].firstChild.nodeValue);

                    }

                }

            });

            setLabelValue();
            $('.lebelnumberformat').number(true, 0);

        }


        function setLabelValue() {

            $('#spanRgessTaxExempt').text(maxRgessExempt);
            $('#HidspanRgessTaxExempt').val($('#spanRgessTaxExempt').text());
            // $('#spanTaxfreeLimit').text(maxTaxLimit);
            $("span[id$=spanTaxfreeLimit]").html(maxTaxLimit);
            $('#HidspanTaxfreeLimit').val($('#spanTaxfreeLimit').text());

            $('#spanTaxfreeLimit1').text(Number(maxTaxLimit) + 1);
            $('#HidspanTaxfreeLimit1').val($('#spanTaxfreeLimit1').text());
            $('#spanTaxfreeLimit2').text(maxTaxSlab1);
            $('#HidspanTaxfreeLimit2').val($('#spanTaxfreeLimit2').text());
            $('#spanTaxLimit1Rate').text(maxTaxSlab1TaxRate);
            $('#HidspanTaxLimit1Rate').val($('#spanTaxLimit1Rate').text());

            $('#spanTaxfreeLimit3').text(Number(maxTaxSlab1) + 1);
            $('#HidspanTaxfreeLimit3').val($('#spanTaxfreeLimit3').text());
            $('#spanTaxfreeLimit4').text(maxTaxSlab2);
            $('#HidspanTaxfreeLimit4').val($('#spanTaxfreeLimit4').text());

            $('#spanTaxLimit2Rate').text(maxTaxSlab2TaxRate);
            $('#HidspanTaxLimit2Rate').val($('#spanTaxLimit2Rate').text());


            $('#spanTaxfreeLimit5').text(Number(maxTaxSlab2) + 1);
            $('#HidspanTaxfreeLimit5').val($('#spanTaxfreeLimit5').text());

            $('#spanTaxLimit3Rate').text(maxTaxSlab3TaxRate);
            $('#HidspanTaxLimit3Rate').val($('#spanTaxLimit3Rate').text());


            $('#spanEducationCessRate').text(eduCess);

            $('#spanTaxExempt80c').text(maxTaxExempt80c);
            $('#HidspanTaxExempt80c').val($('#spanTaxExempt80c').text());

            $('#spanMedicalClaim').text(maxMedicalClaim);
            $('#HidspanMedicalClaim').val($('#spanMedicalClaim').text());


        }



        function SetConfig() {
            //alert('Start reading config');   
            test = 345;
            var xmlDoc = new ActiveXObject("MSXML.DOMDocument");
            xmlDoc.async = "false";
            xmlDoc.load("Excel/DspTaxConfig.xml");
            var TaxDeatails = xmlDoc.documentElement;
            var taxpoints = TaxDeatails; //.childNodes(1);

            maxIncomeLimit = taxpoints.getElementsByTagName("MaxIncomeLimit")[0].text;
            maxRgessLimit = taxpoints.getElementsByTagName("RgessLimit")[0].text;
            maxRgessExempt = taxpoints.getElementsByTagName("RgessTaxExempt")[0].text;
            maxTaxExempt80c = taxpoints.getElementsByTagName("TaxExempt80c")[0].text;
            eduCess = taxpoints.getElementsByTagName("EducationCessRate")[0].text;
            maxTaxExemptOthers = taxpoints.getElementsByTagName("TaxExemptOthers")[0].text;

            var age = $('#textAge').val();
            if (age == "" || age < 60) {

                maxTaxLimit = taxpoints.getElementsByTagName("TaxFreeLimit")[0].text;
                maxMedicalClaim = taxpoints.getElementsByTagName("MedicalClaim")[0].text;

                maxTaxSlab1 = TaxDeatails.getElementsByTagName("TaxLimit")[0].childNodes[0].text;
                maxTaxSlab1TaxRate = TaxDeatails.getElementsByTagName("TaxLimit")[0].childNodes[1].text;

                maxTaxSlab2 = TaxDeatails.getElementsByTagName("TaxLimit")[1].childNodes[0].text;
                maxTaxSlab2TaxRate = TaxDeatails.getElementsByTagName("TaxLimit")[1].childNodes[1].text;


                maxTaxSlab3 = TaxDeatails.getElementsByTagName("TaxLimit")[2].childNodes[0].text;
                maxTaxSlab3TaxRate = TaxDeatails.getElementsByTagName("TaxLimit")[2].childNodes[1].text;

            }
            else if (age >= 60 && age < 80) {


                maxTaxLimit = taxpoints.getElementsByTagName("Age80below")[0].getElementsByTagName("TaxFreeLimit")[0].text;
                maxMedicalClaim = taxpoints.getElementsByTagName("Age80below")[0].getElementsByTagName("MedicalClaim")[0].text;

                maxTaxSlab1 = TaxDeatails.getElementsByTagName("Age80below")[0].getElementsByTagName("TaxLimit")[0].childNodes[0].text;
                maxTaxSlab1TaxRate = TaxDeatails.getElementsByTagName("Age80below")[0].getElementsByTagName("TaxLimit")[0].childNodes[1].text;

                maxTaxSlab2 = TaxDeatails.getElementsByTagName("Age80below")[0].getElementsByTagName("TaxLimit")[1].childNodes[0].text;
                maxTaxSlab2TaxRate = TaxDeatails.getElementsByTagName("Age80below")[0].getElementsByTagName("TaxLimit")[1].childNodes[1].text;

                maxTaxSlab3 = TaxDeatails.getElementsByTagName("Age80below")[0].getElementsByTagName("TaxLimit")[2].childNodes[0].text;
                maxTaxSlab3TaxRate = TaxDeatails.getElementsByTagName("Age80below")[0].getElementsByTagName("TaxLimit")[2].childNodes[1].text;


            }
            else {
                maxTaxLimit = taxpoints.getElementsByTagName("Age80Above")[0].getElementsByTagName("TaxFreeLimit")[0].text;
                maxMedicalClaim = taxpoints.getElementsByTagName("Age80Above")[0].getElementsByTagName("MedicalClaim")[0].text;

                maxTaxSlab1 = TaxDeatails.getElementsByTagName("Age80Above")[0].getElementsByTagName("TaxLimit")[0].childNodes[0].text;
                maxTaxSlab1TaxRate = TaxDeatails.getElementsByTagName("Age80Above")[0].getElementsByTagName("TaxLimit")[0].childNodes[1].text;

                maxTaxSlab2 = TaxDeatails.getElementsByTagName("Age80Above")[0].getElementsByTagName("TaxLimit")[1].childNodes[0].text;
                maxTaxSlab2TaxRate = TaxDeatails.getElementsByTagName("Age80Above")[0].getElementsByTagName("TaxLimit")[1].childNodes[1].text;

                maxTaxSlab3 = TaxDeatails.getElementsByTagName("Age80Above")[0].getElementsByTagName("TaxLimit")[2].childNodes[0].text;
                maxTaxSlab3TaxRate = TaxDeatails.getElementsByTagName("Age80Above")[0].getElementsByTagName("TaxLimit")[2].childNodes[1].text;


            }

            setLabelValue();
        }



        function calculateRgess() {

            // alert("test for global var");
            var txtsal = Number($('#salaryInput').val());
            var txtlic = Number($('#licInput').val());
            var txtmed = Number($('#medInput').val());
            var txtRgsBnfit = Number($('#txtbenefitRgess').val());
            var txtOthersBnfit = Number($('#txtHomeLoanInt').val());



            txtlic = InvestmentTaxBenefit(txtlic);
            txtmed = MedicalTaxBenefit(txtmed);
            txtOthersBnfit = OthersTaxBenefit(txtOthersBnfit);

            //            if (txtsal < 200000) {
            //                $('#form1')[0].reset();
            //                return false;
            //            }

            $('#salaryInputWo').val(txtsal);


            var taxIncome = txtsal - txtlic - txtmed - txtOthersBnfit;
            taxIncome = Math.round(taxIncome);




            if (taxIncome > 0) {
                $('#txtTaxIncome').val(taxIncome);
                $('#txtTaxIncomeWo').val(taxIncome);
            }
            else {
                $('#txtTaxIncome').val('0');
                $('#txtTaxIncomeWo').val('0');
                $('#txtTaxIncomeR').val('0');
                $('#txtTaxIncomeRWo').val('0');
            }





            var txIncomeR = taxIncome - txtRgsBnfit;
            //alert(txIncomeR);
            if (txIncomeR > 0)
                $('#txtTaxIncomeR').val(txIncomeR);
            else
                $('#txtTaxIncomeR').val('0');

            if (taxIncome > 0)
                $('#txtTaxIncomeRWo').val(taxIncome);
            else
                $('#txtTaxIncomeRWo').val('0');




            //alert(taxSalb1(txIncomeR));
            $('#taxLimit2').val(taxSalb1(txIncomeR, txtsal));
            $('#taxLimit2Wo').val(taxSalb1(taxIncome, txtsal));


            $('#taxLimit3').val(taxSalb2(txIncomeR));
            $('#taxLimit3Wo').val(taxSalb2(taxIncome));

            $('#taxLimit4').val(taxSalb3(txIncomeR));
            $('#taxLimit4Wo').val(taxSalb3(taxIncome));


            var ttax2 = $('#taxLimit2').val();
            var ttax3 = $('#taxLimit3').val();
            var ttax4 = $('#taxLimit4').val();

            var sumtx = Number(ttax2) + Number(ttax3) + Number(ttax4);

            var ttax2w = $('#taxLimit2Wo').val();
            var ttax3w = $('#taxLimit3Wo').val();
            var ttax4w = $('#taxLimit4Wo').val();
            var sumtxw = Number(ttax2w) + Number(ttax3w) + Number(ttax4w);

            $('#txtTotalTax').val(sumtx);
            $('#txtTotalTaxWo').val(sumtxw);

            var taxEduCess = (1 + eduCess / 100);

            $('#txtECess').val(Math.round(sumtx * eduCess / 100));
            $('#txtECessWo').val(Math.round(sumtxw * eduCess / 100));

            $('#txtEduCess').val(Math.round(sumtx * taxEduCess));
            $('#txtEduCessWo').val(Math.round(sumtxw * taxEduCess));

            var tSaved = $('#txtEduCessWo').val() - $('#txtEduCess').val();


            $('#textWoRgess').val($('#txtEduCessWo').val());
            $('#textRgess').val($('#txtEduCess').val());


            $('#txtTaxSaved').val(tSaved);
            $('#textSaved').val(tSaved);



            $('#lblUser').text($('#txtuser').val());

            return true;
        }


        function taxSalb1(txIncomeR, usersal) {
            var txslb1 = 0;
            if (txIncomeR > maxTaxLimit) {

                if (txIncomeR < maxTaxSlab1) {
                    txslb1 = (txIncomeR - maxTaxLimit) * (maxTaxSlab1TaxRate / 100);
                }
                else {
                    txslb1 = (maxTaxSlab1 - maxTaxLimit) * (maxTaxSlab1TaxRate / 100);
                }
            }
            //special Benifit
            if (usersal >= 200000 && usersal <= 500000) {
                if (txslb1 >= 2000) {
                    txslb1 -= 2000;
                }
            }

            return Math.round(txslb1);
        }

        function taxSalb2(txIncomeR) {
            var txslb1 = 0;
            if (txIncomeR > maxTaxSlab1) {

                if (txIncomeR < maxTaxSlab2) {
                    txslb1 = (txIncomeR - maxTaxSlab1) * (maxTaxSlab2TaxRate / 100);
                }
                else {
                    txslb1 = (maxTaxSlab2 - maxTaxSlab1) * (maxTaxSlab2TaxRate / 100);
                }
            }
            return Math.round(txslb1);
        }

        function taxSalb3(txIncomeR) {
            var txslb1 = 0;
            if (txIncomeR > maxTaxSlab2) {

                if (txIncomeR < maxTaxSlab3) {
                    txslb1 = (txIncomeR - maxTaxSlab2) * (maxTaxSlab3TaxRate / 100);
                }
                else {
                    txslb1 = (maxTaxSlab3 - maxTaxSlab2) * (maxTaxSlab3TaxRate / 100);
                }
            }
            return Math.round(txslb1);
        }

        function isNumber(key) {
            var keycode = (key.which) ? key.which : key.keyCode;
            if ((keycode >= 48 && keycode <= 57) || keycode == 8 || keycode == 9) {
                return true;
            }
            return false;
        }

        function regessTaxBenefit(Investment) {

            var salinput = Number($('#salaryInput').val());
            if (salinput > maxIncomeLimit) {
                return 0;
            }
            if (Investment > maxRgessLimit) {
                return maxRgessExempt;
            }
            else {
                return Investment / 2;
            }
        }

        function InvestmentTaxBenefit(Investment) {
            // alert(maxTaxExempt80c);         
            try {
                if (Investment > maxTaxExempt80c) {
                    return maxTaxExempt80c;
                }
                else {
                    return Investment;
                }
            }

            catch (e) {
                alert(e.Description);
            }

        }

        function MedicalTaxBenefit(Investment) {
            // alert(maxMedicalClaim);
            try {

                if (Investment > maxMedicalClaim) {
                    return maxMedicalClaim;
                }
                else {
                    return Investment;
                }

            } catch (e) {
                alert(e.Description);

            }

        }

        function OthersTaxBenefit(Investment) {
            // alert(maxMedicalClaim);
            try {

                if (Investment > maxTaxExemptOthers) {
                    return maxTaxExemptOthers;
                }
                else {
                    return Investment;
                }

            } catch (e) {
                alert(e.Description);

            }

        }


        function commaText(strText) {
            var l = strText.len;
            var outputStr = "";
            for (i = l; i > 0; i--) {

            }

        }

        // This function will assign value to variable after reading from Xml
        //        function ReadXml() {
        //            oRgessTaxExempt = 0;
        //            $.ajax({
        //                type: "GET",
        //                url: "Excel/WiseConfig.xml",
        //                dataType: "xml",
        //                success: function (xml) {
        //                    $(xml).find('wise').each(function () {
        //                        alert($(this).find('RgessTaxExempt').text());
        //                        var oTaxLimit = $(this).find('TaxLimit').text();                        
        //                        var oRgessLimit = $(this).find('RgessLimit').text();
        //                        oRgessTaxExempt = $(this).find('RgessTaxExempt').text();
        //                        //  window.maxRgessExempt = $(this).find('RgessTaxExempt').text();
        //                        //  window['maxTaxLimit'] = oTaxLimit;
        //                        // maxRgessExempt = oRgessTaxExempt;
        //                        // maxRgessLimit = oRgessLimit;
        //                        //                        var url = $(this).find('url').text();
        //                        //                        $(this).find('desc').each(function () {
        //                        //                            var brief = $(this).find('brief').text();   
        //                        //                        });
        //                    });
        //                }
        //            });
        //          //  maxRgessExempt = oRgessTaxExempt;
        //         //   alert(oRgessTaxExempt);
        //        }


        //       function ReadXml2() {
        //           var  check ;
        //            $.ajax({
        //                async: false, // !!!MAKE SURE THIS ONE IS SET!!!
        //                type: "GET",
        //                url: "Excel/WiseConfig.xml",
        //                dataType: "xml",             
        //                success: function (data) {
        //                    if (data.status == 'ok') {
        //                    $(data).find('wise').each(function () {
        //                       check = $(this).find('RgessTaxExempt').text();
        //                });
        //            }
        //            alert(check);
        //        });


        function txtTaxIncomeR_onclick() {

        }

        function txtbenefitRgess_onclick() {

        }

        function ShowPdfSection() {

            //$("#Showpdfdiv").toggle();
           
            //            if ($("#Showpdfdiv").css("display", "none")) {
            //                $("#Showpdfdiv").css("display", "none");
            //            }
            //            else
            //                $("#Showpdfdiv").css("display", "");

//            $("#Showpdfdiv").toggle(function (e) {
//                if ($(this).is(":visible")) {
//                    $("#Showpdfdiv").show();                 
//                }
//                else {
//                    $("#Showpdfdiv").hide();
//                };
//                //e.preventDefault();
//            });

            if ($('#Showpdfdiv').css('display') == 'none') {
                $('#Showpdfdiv').show('slow');
            } else {
                $('#Showpdfdiv').hide('slow');
            }

//            if ($('#Showpdfdiv').is(":visible"))
//                return true;
            
            return false;
        }

        

    </script>
</head>
<body>
    <form id="form1" runat="server">
    <table width="700" border="0" align="center" cellpadding="0" cellspacing="0" class="pageCont">
        <tr align="left">
            <td valign="top">
                <div class="blueBox">
                    <div id="divTax" runat="server">
                        <table width="98%" border="0" align="center" cellpadding="0" cellspacing="0">
                            <tr>
                                <td>
                                    <table width="95%" border="0" align="center" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td colspan="3" class="heading_text">
                                                <table width="8%" border="0" align="right" cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td class="heading_text">
                                                            <asp:LinkButton ID="LinkButtonGenerateReport" runat="server" OnClick="LinkButtonGenerateReport_Click"
                                                                ToolTip="Download PDF" Visible="false"><img src="IMG/pdf.png" /></asp:LinkButton>
                                                            <a href="#">
                                                                <img src="IMG/pdf.png" title="Download PDF" onclick="ShowPdfSection()" /></a>
                                                        </td>
                                                        <td>
                                                            <asp:ImageButton ID="btnExcelCalculation" runat="server" ImageUrl="~/DSP/IMG/excel2.png"
                                                                ToolTip="Download Excel" Text="Show Excel " Visible="true" OnClick="ExcelCalculation_Click" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="3">
                                                <div id="Showpdfdiv" runat="server" class="FieldHead" style="display: none;">
                                                    <div style="border: 1px solid #569fd3;">
                                                    <%-- <b>Please select your Credential:</b>--%>
                                                    <table width="100%" style="padding-top: 20px;">
                                                        <tr>
                                                            <td align="left" width="100%">
                                                                <asp:RadioButtonList ID="RadioButtonListCustomerType" runat="server" OnSelectedIndexChanged="RadioButtonListCustomerType_SelectedIndexChanged"
                                                                    TextAlign="Right" RepeatDirection="Horizontal" AutoPostBack="true" BorderColor="#569fd3"
                                                                    BorderWidth="0" Width="250px" CssClass="lefft">
                                                                    <asp:ListItem> Distributor</asp:ListItem>
                                                                    <asp:ListItem Selected="true"> Not a Distributor</asp:ListItem>
                                                                </asp:RadioButtonList>
                                                                <br />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <div id="DistributerDiv" runat="server" visible="false">
                                                        <table id="tblDistb" width="100%" align="center">
                                                            <tr>
                                                                <td width="32%" align="left" style="padding-left: 30px;">
                                                                    ARN No
                                                                </td>
                                                                <td width="68%" align="left">
                                                                    <table width="100%" border="0" align="left" cellpadding="0" cellspacing="0" class="row3">
                                                                        <tr>
                                                                            <td class="rupee_blue">
                                                                                &nbsp;
                                                                            </td>
                                                                            <td>
                                                                                <label>
                                                                                    <input type="text" name="textfield" id="txtArn1" class="row_txtbxText" autocomplete="off"
                                                                                        runat="server" maxlength="30" /></label>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td width="32%" align="left" style="padding-left: 30px;">
                                                                    Prepared By
                                                                </td>
                                                                <td width="68%" align="left">
                                                                    <table width="100%" border="0" align="left" cellpadding="0" cellspacing="0" class="row3">
                                                                        <tr>
                                                                            <td class="rupee_blue">
                                                                                &nbsp;
                                                                            </td>
                                                                            <td>
                                                                                <label>
                                                                                    <input type="text" name="textfield" id="txtPreparedby" class="row_txtbxText" autocomplete="off"
                                                                                        runat="server" maxlength="40" /></label>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td width="32%" align="left" style="padding-left: 30px;">
                                                                    Contact No(Mobile)
                                                                </td>
                                                                <td width="68%" align="left">
                                                                    <table width="100%" border="0" align="left" cellpadding="0" cellspacing="0" class="row3">
                                                                        <tr>
                                                                            <td class="rupee_blue">
                                                                                &nbsp;
                                                                            </td>
                                                                            <td>
                                                                                <label>
                                                                                    <input type="text" name="textfield" id="txtMobile" class="row_txtbxText" autocomplete="off"
                                                                                        runat="server" maxlength="14" /></label>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td width="32%" align="left" style="padding-left: 30px;">
                                                                    Email
                                                                </td>
                                                                <td width="68%" align="left">
                                                                    <table width="100%" border="0" align="left" cellpadding="0" cellspacing="0" class="row3">
                                                                        <tr>
                                                                            <td class="rupee_blue">
                                                                                &nbsp;
                                                                            </td>
                                                                            <td>
                                                                                <label>
                                                                                    <input type="text" name="textfield" id="txtEmail" class="row_txtbxText" autocomplete="off"
                                                                                        runat="server" maxlength="30" /></label>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td width="32%" align="left" style="padding-left: 30px;">
                                                                    Prepared For
                                                                </td>
                                                                <td width="68%" align="left">
                                                                    <table width="100%" border="0" align="left" cellpadding="0" cellspacing="0" class="row3">
                                                                        <tr>
                                                                            <td class="rupee_blue">
                                                                                &nbsp;
                                                                            </td>
                                                                            <td>
                                                                                <label>
                                                                                    <input type="text" name="textfield" id="txtPreparedFor" class="row_txtbxText" autocomplete="off"
                                                                                        runat="server" maxlength="40" /></label>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                            <%--<tr>
                                                                                                                                                                <td>
                                                                                                                                                                    ARN No
                                                                                                                                                                </td>
                                                                                                                                                                <td>
                                                                                                                                                                    <asp:TextBox ID="txtxARNo" runat="server"></asp:TextBox>
                                                                                                                                                                </td>
                                                                                                                                                            </tr>--%>
                                                        </table>
                                                    </div>
                                                    <table width="100%">
                                                        <tr>
                                                            <td width="32%" align="left" style="padding-left: 30px;">
                                                                Generate PDF Report:
                                                            </td>
                                                            <td width="68%" align="left">
                                                                <asp:LinkButton ID="LinkButton1" runat="server" OnClick="LinkButtonGenerateReport_Click"
                                                                    ToolTip="Download PDF" OnClientClick="javascript:return pdfcheck();"><img src="IMG/downloadPDF.jpg" style="border: 0;" alt="" width="25" height="25" /></asp:LinkButton>
                                                            </td>
                                                        </tr>
                                                    </table>                                                    
                                                    </div>
                                                    <br />
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="row2">
                                                Your Name
                                            </td>
                                            <td>
                                                <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0" class="row3">
                                                    <tr>
                                                        <td class="rupee_blue">
                                                            &nbsp;
                                                        </td>
                                                        <td>
                                                            <label>
                                                                <input type="text" name="textfield" id="txtuser" class="row_txtbxText" autocomplete="off"
                                                                    runat="server" style="border-color: Red" /></label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td>
                                                <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0" class="row4"
                                                    style="display: none">
                                                    <tr>
                                                        <td class="rupee_blue">
                                                            &nbsp;
                                                        </td>
                                                        <td>
                                                            <label>
                                                                <input type="text" name="textfield" id="textfield" class="row_txtbx2" readonly="readonly" />
                                                            </label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="row2">
                                                Age
                                            </td>
                                            <td>
                                                <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0" class="row3">
                                                    <tr>
                                                        <td class="rupee_blue">
                                                            &nbsp;
                                                        </td>
                                                        <td>
                                                            <label>
                                                                <input type="text" name="textfield" id="textAge" class="row_txtbxText" autocomplete="off"
                                                                    onkeypress="return isNumber(event);" runat="server" style="border-color: Red"
                                                                    maxlength="2" /></label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td>
                                                <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0" class="row4"
                                                    style="display: none">
                                                    <tr>
                                                        <td class="rupee_blue">
                                                            &nbsp;
                                                        </td>
                                                        <td>
                                                            <label>
                                                                <input type="text" name="textfield" id="textAgeWo" class="row_txtbx2" readonly="readonly" />
                                                            </label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td class="heading_text">
                                                (Invested in RGESS)
                                            </td>
                                            <td class="heading_text">
                                                (No RGESS Investment)
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="row2">
                                                Gross Income
                                            </td>
                                            <td>
                                                <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0" class="row3">
                                                    <tr>
                                                        <td class="rupee_blue">
                                                            <img src="img/rupee_blue.png" width="5" height="9" />
                                                        </td>
                                                        <td>
                                                            <label>
                                                                <input type="text" name="textfield" id="salaryInput" class="row_txtbx" value="0"
                                                                    onkeypress="Javascript:return isNumber(event);" autocomplete="off" runat="server" />
                                                                <%--onchange="Javascript:return calculateRgess();" --%>
                                                                <%--onkeypress="Javascript:return isNumber(event);"--%>
                                                            </label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td>
                                                <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0" class="row4">
                                                    <tr>
                                                        <td class="rupee_blue">
                                                            <img src="img/rupee_gray.png" width="5" height="9" />
                                                        </td>
                                                        <td>
                                                            <label>
                                                                <input type="text" name="textfield" class="row_txtbx2" id="salaryInputWo" runat="server" readonly="readonly"
                                                                    value="0" />
                                                            </label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="row2">
                                                Less: Deduction U/S
                                            </td>
                                            <td>
                                                <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0" class="row31">
                                                    <tr>
                                                        <td class="rupee_blue">
                                                            &nbsp;
                                                        </td>
                                                        <td>
                                                            <label>
                                                                &nbsp;</label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td>
                                                <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0" class="row4">
                                                    <tr>
                                                        <td class="rupee_blue">
                                                        </td>
                                                        <td>
                                                            <label>
                                                                &nbsp;</label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="row2">
                                                80 C
                                            </td>
                                            <td>
                                                <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0" class="row3">
                                                    <tr>
                                                        <td class="rupee_blue">
                                                            <img src="img/rupee_blue.png" width="5" height="9" />
                                                        </td>
                                                        <td>
                                                            <label>
                                                                <input type="text" id="licInput" runat="server" value="0" onkeypress="return isNumber(event);"
                                                                    autocomplete="off" name="textfield" class="row_txtbx" />
                                                            </label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td>
                                                <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0" class="row4">
                                                    <tr>
                                                        <td class="rupee_blue">
                                                            <img src="img/rupee_gray.png" width="5" height="9" />
                                                        </td>
                                                        <td>
                                                            <label>
                                                                <input type="text" id="licInputWo" value="0" runat="server" readonly="readonly" name="textfield"
                                                                    class="row_txtbx2" />
                                                            </label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="row2">
                                                80 D (Medical Insurance Premium)
                                            </td>
                                            <td>
                                                <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0" class="row3">
                                                    <tr>
                                                        <td class="rupee_blue">
                                                            <img src="img/rupee_blue.png" width="5" height="9" />
                                                        </td>
                                                        <td>
                                                            <label>
                                                                <input id="medInput" runat="server" autocomplete="off" class="row_txtbx" name="textfield"
                                                                    onkeypress="return isNumber(event);" type="text" value="0" />
                                                            </label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td>
                                                <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0" class="row4">
                                                    <tr>
                                                        <td class="rupee_blue">
                                                            <img src="img/rupee_gray.png" width="5" height="9" />
                                                        </td>
                                                        <td>
                                                            <label>
                                                                <input type="text" name="salaryInput4" id="medInputWo" runat="server" value="0" readonly="readonly"
                                                                     class="row_txtbx2" />
                                                            </label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="row2">
                                                24 (Interest Part of Home Loan)
                                                <%--Other Deduction (Interest Part of Home Loan or House Rent)--%>
                                            </td>
                                            <td>
                                                <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0" class="row3">
                                                    <tr>
                                                        <td class="rupee_blue">
                                                            <img src="img/rupee_blue.png" width="5" height="9" />
                                                        </td>
                                                        <td>
                                                            <label>
                                                                <input id="txtHomeLoanInt" runat="server" autocomplete="off" class="row_txtbx" name="textfield"
                                                                    onkeypress="return isNumber(event);" type="text" value="0" />
                                                            </label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td>
                                                <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0" class="row4">
                                                    <tr>
                                                        <td class="rupee_blue">
                                                            <img src="img/rupee_gray.png" width="5" height="9" />
                                                        </td>
                                                        <td>
                                                            <label>
                                                                <input type="text" name="salaryInput4" id="txtHomeLoanIntWo" value="0" readonly="readonly"
                                                                   runat="server" class="row_txtbx2" />
                                                            </label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="row2">
                                                Total Taxable Income
                                            </td>
                                            <td>
                                                <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0" class="row31">
                                                    <tr>
                                                        <td class="rupee_blue">
                                                            <img src="img/rupee_gray.png" width="5" height="9" />
                                                        </td>
                                                        <td>
                                                            <label>
                                                                <input type="text" id="txtTaxIncome" value="0" runat="server" readonly="readonly"
                                                                    name="textfield" class="row_txtbx1" />
                                                            </label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td>
                                                <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0" class="row4">
                                                    <tr>
                                                        <td class="rupee_blue">
                                                            <img src="img/rupee_gray.png" width="5" height="9" />
                                                        </td>
                                                        <td>
                                                            <label>
                                                                <input type="text" id="txtTaxIncomeWo" value="0" runat="server" readonly="readonly"
                                                                    name="textfield" class="row_txtbx2" />
                                                            </label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="heading_text2">
                                                Investment in RGESS
                                            </td>
                                            <td>
                                                <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0" class="row3">
                                                    <tr>
                                                        <td class="rupee_blue">
                                                            <img src="img/rupee_blue.png" width="5" height="9" />
                                                        </td>
                                                        <td>
                                                            <label>
                                                                <input type="text" name="textfield" id="txtRgess" runat="server" value="0" onkeypress="return isNumber(event);"
                                                                    autocomplete="off" class="row_txtbx" />
                                                            </label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td>
                                                <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0" class="row4">
                                                    <tr>
                                                        <td class="rupee_blue">
                                                            <img src="img/rupee_gray.png" width="5" height="9" />
                                                        </td>
                                                        <td>
                                                            <label>
                                                                <input type="text" name="textfield" runat="server" id="txtRgessWo" value="Nil" readonly="readonly"
                                                                    class="row_txtbx2" />
                                                            </label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="row5">
                                                80CCG (Tax benefit: Deduction of 50% of amount invested. Deduction not to exceed
                                                <img src="img/rupee_gray.png" width="5" height="9" />
                                                <asp:Label ID="spanRgessTaxExempt" CssClass="lebelnumberformat" runat="server" Text="25,000"></asp:Label>)
                                                <asp:HiddenField ID="HidspanRgessTaxExempt" runat="server" Value="25,000" />
                                            </td>
                                            <td>
                                                <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0" class="row10">
                                                    <tr>
                                                        <td class="rupee_blue">
                                                            <img src="img/rupee_gray.png" width="5" height="9" />
                                                        </td>
                                                        <td>
                                                            <label>
                                                                <input type="text" name="textfield" runat="server" id="txtbenefitRgess" value="0"
                                                                    readonly="readonly" class="row_txtbx1" onclick="return txtbenefitRgess_onclick()" />
                                                            </label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td>
                                                <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0" class="row11">
                                                    <tr>
                                                        <td class="rupee_blue">
                                                            <img src="img/rupee_gray.png" width="5" height="9" />
                                                        </td>
                                                        <td>
                                                            <label>
                                                                <input type="text" name="textfield3" value="Nil" runat="server" id="txtbenefitRgessWo" readonly="readonly"
                                                                    class="row_txtbx2" />
                                                            </label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr class="row6">
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="row2">
                                                Total Taxable Income
                                            </td>
                                            <td>
                                                <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0" class="row31">
                                                    <tr>
                                                        <td class="rupee_blue">
                                                            <img src="img/rupee_gray.png" width="5" height="9" />
                                                        </td>
                                                        <td>
                                                            <input type="text" runat="server" value="0" name="textfield" id="txtTaxIncomeR" readonly="readonly"
                                                                class="row_txtbx1" onclick="return txtTaxIncomeR_onclick()" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td>
                                                <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0" class="row4">
                                                    <tr>
                                                        <td class="rupee_blue">
                                                            <img src="img/rupee_gray.png" width="5" height="9" />
                                                        </td>
                                                        <td>
                                                            <label>
                                                                <input type="text" name="textfield" value="0" id="txtTaxIncomeRWo" readonly="readonly"
                                                                    runat="server" class="row_txtbx2" />
                                                            </label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="row2">
                                                (Upto -
                                                <img src="img/rupee_gray.png" width="5" height="9" />
                                                <asp:Label ID="spanTaxfreeLimit" CssClass="lebelnumberformat" runat="server" Text="200,000"
                                                    EnableViewState="false"></asp:Label>)-NIL
                                                <asp:HiddenField ID="HidspanTaxfreeLimit" runat="server" Value="200,000" />
                                            </td>
                                            <td>
                                                <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0" class="row31">
                                                    <tr>
                                                        <td class="rupee_blue">
                                                            <img src="img/rupee_gray.png" width="5" height="9" />
                                                        </td>
                                                        <td>
                                                            <input type="text" name="textfield" id="taxLimit1" value="0" readonly="readonly"
                                                                class="row_txtbx1" runat="server" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td>
                                                <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0" class="row4">
                                                    <tr>
                                                        <td class="rupee_blue">
                                                            <img src="img/rupee_gray.png" width="5" height="9" />
                                                        </td>
                                                        <td>
                                                            <input type="text" name="textfield" id="taxLimit1Wo" value="0" readonly="readonly"
                                                                runat="server" class="row_txtbx2" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr id="rowtaxFreelimit" runat="server">
                                            <td class="row2">
                                                (
                                                <img src="img/rupee_gray.png" width="5" height="9" />
                                                <asp:Label ID="spanTaxfreeLimit1" CssClass="lebelnumberformat" runat="server" Text="250,001"></asp:Label>-
                                                <img src="img/rupee_gray.png" width="5" height="9" />
                                                <asp:Label ID="spanTaxfreeLimit2" CssClass="lebelnumberformat" runat="server" Text="500,000"></asp:Label>)-
                                                <asp:Label ID="spanTaxLimit1Rate" CssClass="lebelnumberformat" runat="server" Text="10"></asp:Label>%
                                                <asp:HiddenField ID="HidspanTaxfreeLimit1" runat="server" Value="250,001" />
                                                <asp:HiddenField ID="HidspanTaxfreeLimit2" runat="server" Value="500,000" />
                                                <asp:HiddenField ID="HidspanTaxLimit1Rate" runat="server" Value="10" />
                                            </td>
                                            <td>
                                                <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0" class="row31">
                                                    <tr>
                                                        <td class="rupee_blue">
                                                            <img src="img/rupee_gray.png" width="5" height="9" />
                                                        </td>
                                                        <td>
                                                            <label>
                                                                <input type="text" name="textfield" value="0" id="taxLimit2" runat="server" readonly="readonly"
                                                                    class="row_txtbx1" />
                                                            </label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td>
                                                <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0" class="row4">
                                                    <tr>
                                                        <td class="rupee_blue">
                                                            <img src="img/rupee_gray.png" width="5" height="9" />
                                                        </td>
                                                        <td>
                                                            <label>
                                                                <input type="text" name="textfield" value="0" runat="server" id="taxLimit2Wo" readonly="readonly"
                                                                    class="row_txtbx2" />
                                                            </label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="row2">
                                                (
                                                <img src="img/rupee_gray.png" width="5" height="9" />
                                                <asp:Label ID="spanTaxfreeLimit3" CssClass="lebelnumberformat" runat="server" Text="500,001"></asp:Label>-
                                                <img src="img/rupee_gray.png" width="5" height="9" />
                                                <asp:Label ID="spanTaxfreeLimit4" CssClass="lebelnumberformat" runat="server" Text="1,000,000"></asp:Label>)-
                                                <asp:Label ID="spanTaxLimit2Rate" CssClass="lebelnumberformat" runat="server" Text="20"></asp:Label>%
                                                <asp:HiddenField ID="HidspanTaxfreeLimit3" runat="server" Value="500,001" />
                                                <asp:HiddenField ID="HidspanTaxfreeLimit4" runat="server" Value="1,000,000" />
                                                <asp:HiddenField ID="HidspanTaxLimit2Rate" runat="server" Value="20" />
                                            </td>
                                            <td>
                                                <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0" class="row31">
                                                    <tr>
                                                        <td class="rupee_blue">
                                                            <img src="img/rupee_gray.png" width="5" height="9" />
                                                        </td>
                                                        <td>
                                                            <input id="taxLimit3" class="row_txtbx1" value="0" name="textfield" runat="server"
                                                                readonly="readonly" type="text" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td>
                                                <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0" class="row4">
                                                    <tr>
                                                        <td class="rupee_blue">
                                                            <img src="img/rupee_gray.png" width="5" height="9" />
                                                        </td>
                                                        <td>
                                                            <label>
                                                                <input type="text" name="textfield" value="0" runat="server" id="taxLimit3Wo" readonly="readonly"
                                                                    class="row_txtbx2" />
                                                            </label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="row2">
                                                (
                                                <img src="img/rupee_gray.png" width="5" height="9" />
                                                <asp:Label ID="spanTaxfreeLimit5" CssClass="lebelnumberformat" runat="server" Text="1,000,001"></asp:Label>
                                                &nbsp;& Above )-
                                                <asp:Label ID="spanTaxLimit3Rate" runat="server" Text="30"></asp:Label>%
                                                <asp:HiddenField ID="HidspanTaxfreeLimit5" runat="server" Value="1,000,001" />
                                                <asp:HiddenField ID="HidspanTaxLimit3Rate" runat="server" Value="30" />
                                            </td>
                                            <td>
                                                <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0" class="row31">
                                                    <tr>
                                                        <td class="rupee_blue">
                                                            <img src="img/rupee_gray.png" width="5" height="9" />
                                                        </td>
                                                        <td>
                                                            <input id="taxLimit4" class="row_txtbx1" value="0" name="textfield" runat="server"
                                                                readonly="readonly" type="text" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td>
                                                <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0" class="row4">
                                                    <tr>
                                                        <td class="rupee_blue">
                                                            <img src="img/rupee_gray.png" width="5" height="9" />
                                                        </td>
                                                        <td>
                                                            <label>
                                                                <input type="text" name="textfield" value="0" runat="server" id="taxLimit4Wo" readonly="readonly"
                                                                    class="row_txtbx2" />
                                                            </label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="row2">
                                                Tax Payable
                                            </td>
                                            <td>
                                                <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0" class="row31">
                                                    <tr>
                                                        <td class="rupee_blue">
                                                            <img src="img/rupee_gray.png" width="5" height="9" />
                                                        </td>
                                                        <td>
                                                            <input type="text" name="textfield" value="0" runat="server" id="txtTotalTax" readonly="readonly"
                                                                class="row_txtbx1" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td>
                                                <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0" class="row4">
                                                    <tr>
                                                        <td class="rupee_blue">
                                                            <img src="img/rupee_gray.png" width="5" height="9" />
                                                        </td>
                                                        <td>
                                                            <label>
                                                                <input type="text" name="textfield" value="0" id="txtTotalTaxWo" runat="server" readonly="readonly"
                                                                    class="row_txtbx2" />
                                                            </label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="row2">
                                                Education Cess & Secondary and Higher Education Cess
                                                <asp:Label ID="spanEducationCessRate" runat="server" Text="3"></asp:Label>%
                                                <asp:HiddenField ID="HidspanEducationCessRate" runat="server" Value="3" />
                                            </td>
                                            <td>
                                                <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0" class="row31">
                                                    <tr>
                                                        <td class="rupee_blue">
                                                            <img src="img/rupee_gray.png" width="5" height="9" />
                                                        </td>
                                                        <td>
                                                            <input type="text" name="textfield" value="0" runat="server" id="txtECess" readonly="readonly"
                                                                class="row_txtbx1" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td>
                                                <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0" class="row4">
                                                    <tr>
                                                        <td class="rupee_blue">
                                                            <img src="img/rupee_gray.png" width="5" height="9" />
                                                        </td>
                                                        <td>
                                                            <input type="text" name="textfield" value="0" id="txtECessWo" runat="server" readonly="readonly"
                                                                class="row_txtbx2" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="row2">
                                                Total Tax Payable
                                            </td>
                                            <td>
                                                <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0" class="row31">
                                                    <tr>
                                                        <td class="rupee_blue">
                                                            <img src="img/rupee_gray.png" width="5" height="9" />
                                                        </td>
                                                        <td>
                                                            <input type="text" name="textfield" value="0" runat="server" id="txtEduCess" readonly="readonly"
                                                                class="row_txtbx1" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td>
                                                <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0" class="row4">
                                                    <tr>
                                                        <td class="rupee_blue">
                                                            <img src="img/rupee_gray.png" width="5" height="9" />
                                                        </td>
                                                        <td>
                                                            <input type="text" name="textfield" value="0" id="txtEduCessWo" runat="server" readonly="readonly"
                                                                class="row_txtbx2" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="row2">
                                                Tax Saved for
                                                <asp:Label ID="lblUser" runat="server" Text=""></asp:Label>
                                            </td>
                                            <td>
                                                <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0" class="row31">
                                                    <tr>
                                                        <td class="rupee_blue">
                                                            <img src="img/rupee_gray.png" width="5" height="9" />
                                                        </td>
                                                        <td>
                                                            <input type="text" name="textfield" value="0" id="txtTaxSaved" runat="server" readonly="readonly"
                                                                class="row_txtbx1" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td>
                                                <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0" class="row4">
                                                    <tr>
                                                        <td class="rupee_blue">
                                                            <img src="img/rupee_gray.png" width="5" height="9" />
                                                        </td>
                                                        <td>
                                                            <input type="text" name="textfield" id="Text10" value="Nil" runat="server" readonly="readonly" class="row_txtbx2" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr class="row6">
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="3">
                                                <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0" class="blueBox1">
                                                    <tr>
                                                        <td>
                                                            <table width="98%" border="0" cellspacing="0" cellpadding="0">
                                                                <tr>
                                                                    <td class="bottom_row1">
                                                                        Total Tax Payable without RGESS
                                                                    </td>
                                                                    <td>
                                                                        <table width="100%" border="0" align="left" cellpadding="0" cellspacing="0" class="row8">
                                                                            <tr>
                                                                                <td class="rupee_blue">
                                                                                    <img src="img/rupee_gray.png" width="5" height="9" />
                                                                                </td>
                                                                                <td>
                                                                                    <label>
                                                                                        <input type="text" runat="server" name="textfield2" value="0" id="textWoRgess" class="row_txtbx3"
                                                                                            readonly="readonly" />
                                                                                    </label>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="bottom_row1">
                                                                        Total Tax Payable with RGESS
                                                                    </td>
                                                                    <td>
                                                                        <table width="100%" border="0" align="left" cellpadding="0" cellspacing="0" class="row8">
                                                                            <tr>
                                                                                <td class="rupee_blue">
                                                                                    <img src="img/rupee_gray.png" width="5" height="9" />
                                                                                </td>
                                                                                <td>
                                                                                    <label>
                                                                                        <input type="text" runat="server" name="textfield2" value="0" id="textRgess" class="row_txtbx3"
                                                                                            readonly="readonly" />
                                                                                    </label>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="bottom_altrow1">
                                                                        Tax Saved
                                                                    </td>
                                                                    <td>
                                                                        <table width="100%" border="0" align="left" cellpadding="0" cellspacing="0" class="row9">
                                                                            <tr>
                                                                                <td class="rupee_blue">
                                                                                    <img src="img/rupee_gray.png" width="5" height="9" />
                                                                                </td>
                                                                                <td>
                                                                                    <label>
                                                                                        <input type="text" runat="server" name="textfield2" value="0" id="textSaved" class="row_txtbx3"
                                                                                            readonly="readonly" />
                                                                                    </label>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="3" style="color: #333333">
                                                <strong>&nbsp;Disclaimer: </strong>
                                                <ul>
                                                    <li> Maximum deduction of
                                                        <img src="img/rupee_gray.png" width="5" height="9" />
                                                        <asp:Label ID="spanTaxExempt80c" runat="server" CssClass="lebelnumberformat" Text="150000"></asp:Label>
                                                        <asp:HiddenField ID="HidspanTaxExempt80c" runat="server" Value="150,000" />
                                                        is allowed for 80C. </li>
                                                    <li> Maximum deduction of
                                                        <img src="img/rupee_gray.png" width="5" height="9" />
                                                        <asp:Label ID="spanMedicalClaim" CssClass="lebelnumberformat" runat="server" Text="40000"></asp:Label>
                                                        <asp:HiddenField ID="HidspanMedicalClaim" runat="server" Value="40,000" />
                                                        is allowed for 80D. </li>
                                                    <li> 80C includes Public Provident Fund (PPF), National Savings Certificate (NSC),
                                                        Insurance Premium Paid, Tuition Fees ( up to 2 children),Housing Loan-Principal
                                                        payment, Tax saving Mutual Funds, Fixed Deposit (Tax Saving) and etc. </li>
                                                    <li> Deduction of an amount equal to hundred percent of income-tax payable or an amount
                                                        of
                                                        <img src="img/rupee_gray.png" width="5" height="9" />
                                                        2,000, whichever is less, is allowed to an individual resident in India whose total
                                                        income does not exceed
                                                        <img src="img/rupee_gray.png" width="5" height="9" />
                                                        500,000. </li>
                                                    <li> Surcharge @ 10% is also applicable for taxable income exceeding
                                                        <img src="img/rupee_gray.png" width="5" height="9" />
                                                        1 crore. However, marginal relief is also allowed to ensure that the surcharge payable
                                                        on excess of income over
                                                        <img src="img/rupee_gray.png" width="5" height="9" />
                                                        1 crore is limited to the amount by which the income exceed
                                                        <img src="img/rupee_gray.png" width="5" height="9" />
                                                        1 crore. </li>
                                                    <li> To avail tax benefit under RGESS, investors shall comply with condition as prescribed
                                                        under Rajiv Gandhi Equity Savings Scheme, 2012. </li>
                                                    <li> This is not to be considered as a tax advice document. Please reach out to a tax
                                                        consultant for further tax related issues. </li>
                                                </ul>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
