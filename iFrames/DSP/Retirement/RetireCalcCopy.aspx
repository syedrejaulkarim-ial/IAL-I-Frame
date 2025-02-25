<%@ Page Language="C#" AutoEventWireup="true" ViewStateEncryptionMode="Always" EnableViewStateMac="true" CodeBehind="RetireCalcCopy.aspx.cs" Inherits="iFrames.DSP.Retirement.RetireCalcCopy" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Retirement Calculator</title>
    <link href="css/master.css" rel="stylesheet" type="text/css" />
    <link href="css/retierment.css" rel="stylesheet" type="text/css" />
    <link href="css/datepicker.css" rel="stylesheet" type="text/css" />
    <script src="js/date.js" type="text/javascript"></script>
    <script src="js/jquery-1.9.1.js" type="text/javascript"></script>
    <!-- <script src="js/jquery.js" type="text/javascript"></script> -->
    <script src="js/bootstrap.min.js" type="text/javascript"></script>
    <script src="js/bootstrap-datepicker.js"></script>
    <!--tooltip-->
    <link rel="stylesheet" href="css/form-field-tooltip.css" media="screen" type="text/css">
    <script type="text/javascript" src="js/rounded-corners.js"></script>
    <script type="text/javascript" src="js/form-field-tooltip.js"></script>
    <!--tooltip-->
    <!--tabs-->
    <%-- <script type="text/javascript" src="js/jquery.min.js"></script>--%>
    <script type="text/javascript" src="js/tytabs.jquery.min.js"></script>
    <script src="js/RetireScript.js" type="text/javascript"></script>
    <script src="js/jquery.number.js" type="text/javascript"></script>
    <style type="text/css">
        .ui-datepicker-trigger {
            position: relative;
            top: 5px;
        }

        .limitText {
            max-width: 10px;
            width: 10px;
        }

        .txtLeft {
            text-align: left;
        }

        .flag {
            background: url("../images/flag-icon.png")no-repeat;
            width: 16px;
            height: 16px;
            text-align: left;
            padding-right: 5px;
        }

        .text-error {
            color: #b94a48;
            text-align: left;
            font-weight: bold;
        }

        .pdf {
            background: #eeeeee;
            border: #0f8ccf solid 1px;
            -webkit-border-radius: 4px;
            -moz-border-radius: 4px;
            border-radius: 4px;
            padding-left: 30px;
        }

        .RBL label {
            display: inline;
        }

        .RBL td {
            text-align: center;
            width: 20px;
        }
    </style>
    <script type="text/javascript">
        var WireUpStartDate = function () {
            $('#txtRetDate').val(new Date().toString('dd/MM/yyyy'));

            $('#txtRetDate').keydown(function (e) {
                e.preventDefault();
                return false;
            });

            $('#icon-start-date').datepicker().on('changeDate', function (e) {
                var selectedDate = new Date(e.date.valueOf());
                $('#txtRetDate').val(selectedDate.toString('dd/MM/yyyy'));
                $('#icon-start-date').datepicker('hide');
            });
        }

        $(document).ready(function () {

            WireUpStartDate();

            //            $('#txtRetDate').datepicker({
            //                dateFormat: 'dd/mm/yy',
            //                changeMonth: true,
            //                changeYear: true,
            //                maxDate: -1
            //            });

            //            $("#txtRetDate").datepicker();

            $("#tabsholder").tytabs({
                fadespeed: "fast"
            });

            //tabinit: "1",
            //            $("#tabsholder2").tytabs({
            //                prefixtabs: "tabz",
            //                prefixcontent: "contentz",
            //                classcontent: "tabscontent",
            //                tabinit: "2",
            //                catchget: "tab1",
            //                fadespeed: "normal"
            //            });


            $counter = 1; // initialize 1 for limitting textboxes //
            $Lscounter = 1;
            $TotalSipcounter = 1; // initialize 1 for limitting textboxes //
            $TotalLscounter = 1;

            $AddSipArray = [0];
            $AddLsArray = [0];

            //checkAuthen();
            //ShowFilledBox();

            ShowHideDiv();
            $('.row_txtbx').number(true, 0);
            $('.txtLeft').number(true, 0);
            $('#divSipMain').fadeOut('slow');
            $('#divLsMain').fadeOut('slow');

            //CallAllMethod();

            $('#divDist input:radio').change(function () {
                var selectedVal = $("#divDist input:radio:checked").val();
                //alert(selectedVal);
                //alert($(this).val());

                var selectedDis = $(this).val();
                //                var sessionValue = '<%= Session["isDistributor"] %>';
                //                alert(sessionValue);
                $("#HidDist").val(selectedDis);
                if (selectedDis == "Distributor") {
                    $('#DistributerDiv').show();
                    //                    <%Session["isDistributor"] = "true";%>  
                }
                else {
                    // $('#DistributerDiv').attr("display", "none");
                    $('#DistributerDiv').attr("style", "display:none");
                    //                     <%Session["isDistributor"] = "False";%>
                    //                      $("#HidDist").val('Not a Distributor');
                }



                //$("#Showpdfdiv").removeAttr("class");

            });

        });


    </script>
</head>
<body>
    <form id="Retireform" runat="server">
        <div>
            <table width="700" border="0" cellspacing="0" cellpadding="0">
                <tr>
                    <td>
                        <div class="pageCont">
                            <div class="blueBox">
                                <div class="Boxborder">
                                    <table width="95%" border="0" align="center" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td>
                                                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                    <tr>
                                                        <td class="blank_row">&nbsp;
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <table width="98%" border="0" align="center" cellpadding="0" cellspacing="0">
                                                                <tr class="bottom_margin1">
                                                                    <td class="simple_txt1">Name
                                                                    </td>
                                                                    <td class="txtbox_width1">
                                                                        <input name="" id="txtUserName" runat="server" autocomplete="off" type="text" size="50"
                                                                            onblur="ContainsLetter(this)" rel="tooltip" style="width: 220px;" />
                                                                    </td>
                                                                    <td class="blank_col1">&nbsp;
                                                                    </td>
                                                                    <td class="simple_txt2">Current Age
                                                                    </td>
                                                                    <td class="txtbox_width1">
                                                                        <input name="" id="txtAge" type="text" autocomplete="off" size="300" onkeypress="return isNumber(event);"
                                                                            runat="server" maxlength="2" rel="tooltip" style="width: 220px;" />
                                                                    </td>
                                                                    <%--onblur="CallAllMethod();"--%>
                                                                </tr>
                                                                <tr class="bottom_margin1">
                                                                    <td class="simple_txt1">Email ID
                                                                    </td>
                                                                    <td class="txtbox_width1">
                                                                        <input type="text" id="txtUserEmail" autocomplete="off" runat="server" size="50"
                                                                            rel="tooltip" style="width: 220px;" />
                                                                        <%--onblur="CheckEmail(this.value)"--%>
                                                                    </td>
                                                                    <td>&nbsp;
                                                                    </td>
                                                                    <td class="simple_txt2">Retirement Age
                                                                    </td>
                                                                    <td class="txtbox_width1">
                                                                        <input id="txtWishRetireAge" runat="server" autocomplete="off" maxlength="2" name=""
                                                                            onkeypress="return isNumber(event);" rel="tooltip" size="3" type="text" style="width: 220px;" />
                                                                        <%--onblur="CallAllMethod();"--%>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <%--<tr>
                                                    <td>
                                                        <table width="98%" border="0" align="center" cellpadding="0" cellspacing="0">
                                                            
                                                            <tr>
                                                                <td>
                                                                    <input name="" id="txtUserName" runat="server" autocomplete="off" type="text"
                                                                        size="50" onblur="ContainsLetter(this)" rel="tooltip" tooltiptext="Type Your Name Here" />
                                                                </td>
                                                                <td class="txtaltbox_width">
                                                                    &nbsp;
                                                                </td>
                                                                <td class="txtbox_width">
                                                                    <input name="" id="txtAge" type="text" autocomplete="off" 
                                                                        size="300" onkeypress="return isNumber(event);" runat="server" maxlength="2"
                                                                        onblur="CallAllMethod();" rel="tooltip" tooltiptext="Type Your Age Here" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>--%>
                                                    <%--<tr>
                                                    <td>
                                                        <table width="98%" border="0" align="center" cellpadding="0" cellspacing="0">
                                                            <tr>
                                                                <td class="txtbox_width">
                                                                    <input type="text"  id="txtUserEmail" autocomplete="off"
                                                                        runat="server" size="50" onblur="CheckEmail(this.value)" rel="tooltip" tooltiptext="Type Your Email ID Here" />
                                                                </td>
                                                                <td class="txtaltbox_width">
                                                                    &nbsp;
                                                                </td>
                                                                <td class="txtbox_width">
                                                                    <input name="" type="text" id="txtWishRetireAge" onkeypress="return isNumber(event);"
                                                                        runat="server" maxlength="2" autocomplete="off" 
                                                                        size="3" onblur="CallAllMethod();" rel="tooltip" tooltiptext="Age at Which You Wish to Retire" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>--%>
                                                    <tr>
                                                        <td>
                                                            <table width="97%" border="0" align="center" cellpadding="0" cellspacing="0">
                                                                <tr>
                                                                    <td valign="top" width="50%">
                                                                        <table width="100%" border="0" align="left" cellpadding="0" cellspacing="0">
                                                                            <tr>
                                                                                <td>
                                                                                    <table width="98%" border="0" align="left" cellpadding="0" cellspacing="0">
                                                                                        <tr class="bottom_margin">
                                                                                            <td class="simple_txt">Annual rate of return pre retirement (%)
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td class="txtbox_width">
                                                                                                <input type="text" rel="tooltip" id="txtRatePreRetire" autocomplete="off" runat="server"
                                                                                                    maxlength="6" onkeypress="return isNumeric(event)" onblur=" Javascript:if(!validateDecimalLimit(this)) ResetAllFinalvalue();"
                                                                                                    style="background-repeat: no-repeat;" />
                                                                                                <%--onkeypress="return isNumeric(event); onblur="return validateNumber(this)""--%>
                                                                                                <%--tooltiptext="While you plan for retirement, you can afford to take more risks and hence your estimate can be aggressive"--%>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>
                                                                                    <table width="98%" border="0" align="center" cellpadding="0" cellspacing="0">
                                                                                        <tr class="bottom_margin">
                                                                                            <td class="simple_txt">Annual rate of return post retirement (%)
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td class="txtbox_width">
                                                                                                <input type="text" rel="tooltip" id="txtRatePostRetire" autocomplete="off" runat="server"
                                                                                                    maxlength="6" onkeypress="return isNumeric(event)" onblur=" Javascript:if(!validateDecimalLimit(this)) ResetAllFinalvalue();" />
                                                                                                <%--onkeypress="return isNumeric(event);" onblur="return validateNumber(this)" tooltiptext="Post retirement, avoiding undue risks would be prudent, and hence your estimate should be conservative" --%>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>
                                                                                    <table width="98%" border="0" align="center" cellpadding="0" cellspacing="0">
                                                                                        <tr class="bottom_margin">
                                                                                            <td class="simple_txt">Estimated rate of Inflation (%)
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td class="txtbox_width">
                                                                                                <input type="text" rel="tooltip" id="txtRateEstInflation" autocomplete="off" runat="server"
                                                                                                    maxlength="6" onkeypress="return isNumeric(event)" onblur=" Javascript:if(!validateDecimalLimit(this)) ResetAllFinalvalue();" />
                                                                                                <%-- onblur=" Javascript:if(validateNumberOverloaded(this)) CallAllMethod();else ResetAllFinalvalue();"--%>
                                                                                                <%--onkeypress="return isNumeric(event);"  onblur="return validateNumber(this)" tooltiptext="Inflation rate estimates can range from 6-10% and in some cases, even higher!" --%>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                    <td width="50%">
                                                                        <table width="100%" border="0" align="left" cellpadding="0" cellspacing="0" class="quick">
                                                                            <tr>
                                                                                <td>
                                                                                    <table width="95%" border="0" align="center" cellpadding="0" cellspacing="0" style="margin-bottom: 10px;">
                                                                                        <tr>
                                                                                            <td class="text1" valign="top">Quick Tips:
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td valign="top" class="text2">
                                                                                                <b>Pre Retirement: </b>While you plan for retirement, you can afford to take more
                                                                                            risks and hence your estimate can be aggressive (Can be >10%)
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td valign="top" class="text2">
                                                                                                <b>Post Retirement:</b> Post Retirement, avoiding undue risks would be prudent,
                                                                                            and hence your estimate should be conservative (Can be <10%)
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td valign="top" class="text2">
                                                                                                <b>Inflation Rate: </b>Things are getting expensive, inflation rate estimates can
                                                                                            range widely, depending on your estimate (Can be 5%-15%)
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
                                                            <div id="divError_text">
                                                                <table width="98%" border="0" align="center" cellpadding="0" cellspacing="0">
                                                                    <tr>
                                                                        <td class="flag">&nbsp;<img src="images/flag-icon.png" style="vertical-align: top" />
                                                                        </td>
                                                                        <td class="text-error">
                                                                            <label id="lbspanErrorText">
                                                                                Get started</label>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="btn_margin3">
                                                            <input id="Button1" type="button" class="btn btn-small btn-primary" value="Proceed"
                                                                onclick="Javascript: return checkAuthen();" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <div id="divDisplay" runat="server" style="display: none">
                                                                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                                    <tr>
                                                                        <td>
                                                                            <button type="submit" style="visibility: hidden; display: none;">
                                                                                Submit</button>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="bld_txt">Click any of the buttons below to get started
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="simple_txt">We recommend going through the following steps in order for the best results. You
                                                                        can also generate your <b>customized retirement plan snapshot</b> with the results
                                                                        from this planner, if you go through all the steps below.
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="blank_row">&nbsp;
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <%--<table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                                            <tr>
                                                                                <td id="tab1Content" style="background-color: Gray">
                                                                                    a
                                                                                </td>
                                                                                <td id="tab2Content" style="background-color: Gray">
                                                                                    a
                                                                                </td>
                                                                                <td id="tab3Content" style="background-color: Gray">
                                                                                    a
                                                                                </td>
                                                                                <td id="tab4Content" style="background-color: Gray">
                                                                                    a
                                                                                </td>
                                                                            </tr>
                                                                        </table> class="prgres_bg"--%>
                                                                            <table width="584" border="0" align="left" cellspacing="0" cellpadding="0" class="prgres_bg">
                                                                                <tr>
                                                                                    <td valign="bottom" id="Td1" class="stp" style="background-image: url('../Retirement/images/stp1.jpg');">
                                                                                        <label id="lb1" class="stp_txt">
                                                                                            Step 1</label>
                                                                                    </td>
                                                                                    <td valign="bottom" class="stp" id="Td2" style="background-image: url('../Retirement/images/stp1.jpg');">
                                                                                        <label id="lb2" class="stp_txt">
                                                                                            Step 2</label>
                                                                                    </td>
                                                                                    <td valign="bottom" class="stp" id="Td3" style="background-image: url('../Retirement/images/stp1.jpg');">
                                                                                        <label id="lb3" class="stp_txt">
                                                                                            Step 3</label>
                                                                                    </td>
                                                                                    <td valign="bottom" class="stp" id="Td4" style="background-image: url('../Retirement/images/stp4a.jpg');">
                                                                                        <label id="lb4" class="stp_txt">
                                                                                            Step 4</label>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <div id="tabsholder">
                                                                                <ul class="tabs">
                                                                                    <li id="tab1" onclick="checkAuthentication();">What will your monthly expenses be when you retire?</li>
                                                                                    <li id="tab2" onclick="checkAuthentication()">Will your planned corpus last long enough?</li>
                                                                                    <li id="tab3" onclick="checkAuthentication()">How much of a retirement corpus should you plan for?</li>
                                                                                    <li id="tab4" onclick="checkAuthentication()">Start planning towards your financial
                                                                                    goal</li>
                                                                                </ul>
                                                                                <div class="contents marginbot">
                                                                                    <div id="content1" class="tabscontent">
                                                                                        <table width="95%" border="0" align="left" cellpadding="0" cellspacing="0" class="tab_left">
                                                                                            <tr class="bottom_margin">
                                                                                                <td class="simple_txt" colspan="2">What are your monthly expenses currently?
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td class="rupee">Rs.
                                                                                                </td>
                                                                                                <td valign="top">
                                                                                                    <input name="" id="txtcurrentMonthlyExp" type="text" runat="server" onkeypress="return isNumeric(event)"
                                                                                                        maxlength="14" onblur="Javascript:validateNumber(this);" rel="tooltip" class="row_txtbx" />
                                                                                                    <%--checkAuthen();CallAllMethod()--%>
                                                                                                    <input id="Button2" type="button" class="btn btn-mini btn-primary" value="Submit"
                                                                                                        onclick="Javascript: CallAllMethod();" />
                                                                                                    <%-- tooltiptext="What are your monthly expenses currently?"--%>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td colspan="2" style="padding-top: 20px;">
                                                                                                    <%--  <div id="res1" class="result_txt" style="display: none">
                                                                                                    Your monthly expenses at retirement will rise to: Rs.
                                                                                                    <asp:Label ID="lbtxtExpectedMonthlyExpRetire" runat="server" class="txtLeft" Text="" />
                                                                                                    <asp:HiddenField ID="HidlbtxtExpectedMonthlyExpRetire" runat="server" Value="0" />
                                                                                                    <%--<input name="" id="txtExpectedMonthlyExpRetire" runat="server" type="text" style="border: none;" />
                                                                                                    <input id="btnNext1" style="margin-left:240px;" type="button" class="btn btn-mini" value="Next"
                                                                                                        onclick="Javascript:NextMethod(2);" />
                                                                                                </div>--%>
                                                                                                    <table width="100%" border="0" align="left" cellpadding="0" cellspacing="0" class="result_txt">
                                                                                                        <tr>
                                                                                                            <td>
                                                                                                                <div id="res1" class="result_txt" style="display: none">
                                                                                                                    Your monthly expenses at retirement will rise to: Rs.
                                                                                                                <asp:Label ID="lbtxtExpectedMonthlyExpRetire" runat="server" class="txtLeft" Text="" />
                                                                                                                    <asp:HiddenField ID="HidlbtxtExpectedMonthlyExpRetire" runat="server" Value="0" />
                                                                                                                </div>
                                                                                                            </td>
                                                                                                            <td style="text-align: right; padding-right: 5px;">
                                                                                                                <%-- <input id="btnNext1" style="margin-left: 10px; margin-right: 10px; display: none;"
                                                                                                                type="button" class="next" value="" onclick="Javascript:NextMethod(2);" />--%>
                                                                                                                <img id="btnNext1" style="margin-left: 10px; margin-right: 10px; display: none; cursor: pointer;"
                                                                                                                    src="images/next.png" onclick="Javascript:NextMethod(2);" alt="next" />
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                    </table>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </div>
                                                                                    <div id="content2" class="tabscontent">
                                                                                        <table width="100%" border="0" cellpadding="0" cellspacing="0" align="left">
                                                                                            <tr class="bottom_margin">
                                                                                                <td class="simple_txt" colspan="2">What is your estimated retirement corpus?
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td class="rupee">Rs.
                                                                                                </td>
                                                                                                <td>
                                                                                                    <input name="" type="text" id="txtEstimatedRetirementCorpus" onkeypress="return isNumber(event);"
                                                                                                        maxlength="14" runat="server" onblur="Javascript:CallAllMethod();" rel="tooltip"
                                                                                                        class="row_txtbx" />
                                                                                                    <%--tooltiptext="What is your estimated retirement corpus?"--%>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr class="bottom_margin">
                                                                                                <td class="simple_txt" colspan="2">What monthly income would you like to get post retirement (from the retirement corpus)?
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td class="rupee">Rs.
                                                                                                </td>
                                                                                                <td>
                                                                                                    <input name="" type="text" id="txtMonthlyIncomePostRetire" runat="server" style=""
                                                                                                        onkeypress="return isNumber(event);" maxlength="14" rel="tooltip" class="row_txtbx" />
                                                                                                    <%-- onblur="Javascript:CallAllMethod();"--%>
                                                                                                    <%--tooltiptext="What monthly income would you like to get post retirement (from the retirement corpus)?"--%>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr class="bottom_margin">
                                                                                                <td class="simple_txt" colspan="2">Would you want an increase in monthly income every year post retirement? If yes,
                                                                                                how much?(optional)
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td class="rupee">&nbsp;
                                                                                                </td>
                                                                                                <td>
                                                                                                    <input name="" onkeypress="return isNumeric(event);" type="text" id="txtMonthlyIncrementPostRetire"
                                                                                                        runat="server" maxlength="6" onblur=" Javascript:if(!validateDecimalLimit(this)) ResetAllFinalvalue();"
                                                                                                        rel="tooltip" />
                                                                                                    %
                                                                                                <%--onblur=" Javascript:CallAllMethod();"--%>
                                                                                                    <%-- onblur=" Javascript:if(validateNumberOverloaded(this)) CallAllMethod();else ResetAllFinalvalue();"--%>
                                                                                                    <%--tooltiptext="Would you want an increase in monthly income every year post retirement? If yes, how much? "--%>
                                                                                                &nbsp;&nbsp;
                                                                                                <input id="Button3" type="button" class="btn btn-mini btn-primary" value="Submit"
                                                                                                    onclick="Javascript: CallAllMethod();" />
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td colspan="2" style="text-align: left;">
                                                                                                    <%--<div id="res2" class="result_txt" style="display: none;">
                                                                                                    Considering these assumptions, your retirement corpus will last for
                                                                                                    <asp:Label ID="lbtxtRetireCorpusLast" runat="server" class="txtLeft limitText" Text="" />
                                                                                                    years
                                                                                                    <asp:HiddenField ID="HidlbtxtRetireCorpusLast" runat="server" Value="0" />
                                                                                                    <%--<input name="" id="txtRetireCorpusLast" runat="server" type="text" style="width: 140px; border: none;" />
                                                                                                    <input id="btnNext2" style="margin-left:240px;" type="button" class="btn btn-mini" value="Next"
                                                                                                        onclick="Javascript:NextMethod(3);" />
                                                                                                </div>--%>
                                                                                                    <table width="100%" border="0" align="left" cellpadding="0" cellspacing="0" class="result_txt_t3">
                                                                                                        <tr>
                                                                                                            <td>
                                                                                                                <div id="res2" class="result_txt_t3" style="display: none;">
                                                                                                                    Considering these assumptions, your retirement corpus will last for
                                                                                                                <asp:Label ID="lbtxtRetireCorpusLast" runat="server" class="txtLeft limitText" Text="" />
                                                                                                                    years
                                                                                                                <asp:HiddenField ID="HidlbtxtRetireCorpusLast" runat="server" Value="0" />
                                                                                                                </div>
                                                                                                            </td>
                                                                                                            <td style="text-align: right; padding-right: 5px;">
                                                                                                                <%-- <input id="btnNext2" style="margin-left: 10px; margin-right: 10px; display: none;"
                                                                                                                type="button" class="btn btn-mini" value="Next" onclick="Javascript:NextMethod(3);" />--%>
                                                                                                                <img id="btnNext2" style="margin-left: 10px; margin-right: 10px; display: none; cursor: pointer;"
                                                                                                                    src="images/next.png" onclick="Javascript:NextMethod(3);" alt="next" />
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                    </table>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </div>
                                                                                    <div id="content3" class="tabscontent">
                                                                                        <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                                                                                            <tr class="bottom_margin">
                                                                                                <td class="simple_txt" colspan="2">What monthly income would you like to get post retirement (from the retirement corpus)?
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td class="rupee">Rs.
                                                                                                </td>
                                                                                                <td>
                                                                                                    <input name="" type="text" id="txtExpectedMonthlyIncomePostRetire" runat="server"
                                                                                                        style="" onkeypress="return isNumber(event);" onblur="Javascript:validateNumber(this);"
                                                                                                        rel="tooltip" class="row_txtbx" maxlength="14" />
                                                                                                    <%-- CallAllMethod();--%>
                                                                                                    <%--tooltiptext="What monthly income would you like to get post retirement (from the retirement corpus)? "--%>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr class="bottom_margin">
                                                                                                <td class="simple_txt" colspan="2">How long do you wish for your retirement corpus to last? (post retirement)
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td class="rupee"></td>
                                                                                                <td>
                                                                                                    <input name="" type="text" id="txtExpectdRetireCorpusLast" runat="server" style=""
                                                                                                        maxlength="2" onkeypress="return isNumber(event);" onblur="Javascript:validateNumber(this);"
                                                                                                        rel="tooltip" />
                                                                                                    <%-- CallAllMethod();--%>
                                                                                                    <%--tooltiptext="How long do you wish for your retirement corpus to last? (post retirement) "--%>
                                                                                                    <%--<input id="Button4" type="button" class="btn btn-mini btn-primary" value="Submit"
                                                                                                        onclick="Javascript: CallAllMethod();" />--%>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr class="bottom_margin">
                                                                                                <td class="simple_txt" colspan="2">Would you want an increase in monthly income every year post retirement? If yes,
                                                                                                how much?(optional)
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td class="rupee"></td>
                                                                                                <td>
                                                                                                    <input name="" onkeypress="return isNumeric(event);" type="text" id="txtMonthlyIncrementPostRetire4Stage3"
                                                                                                        runat="server" maxlength="6" onblur=" Javascript:if(!validateDecimalLimit(this)) ResetAllFinalvalue();"
                                                                                                        rel="tooltip" />
                                                                                                    %
                                                                                                    &nbsp;&nbsp;
                                                                                                    <input id="Button4" type="button" class="btn btn-mini btn-primary" value="Submit"
                                                                                                        onclick="Javascript: CallAllMethod();" />
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td colspan="2">
                                                                                                    <%--<div id="res3" class="result_txt" style="display: none; padding-right: 25px;">
                                                                                                    <%--Considering these assumptions, you should plan for a retirement corpus of at least:
                                                                                                    Rs.
                                                                                                    Considering these assumptions, your retirement corpus should be at least:
                                                                                                    Rs.
                                                                                                    <asp:Label ID="lbtxtPlannedRetireCorpus" runat="server" class="txtLeft" Text="" />
                                                                                                    <asp:HiddenField ID="HidlbtxtPlannedRetireCorpus" runat="server" Value="0" />
                                                                                                    <%--<input name="" id="txtPlannedRetireCorpus" runat="server" type="text" style="border: none; width: 100px;" />
                                                                                                    <input id="btnNext3" style="margin-left:10px;" type="button" class="btn btn-mini" value="Next"
                                                                                                        onclick="Javascript:NextMethod(4);" />
                                                                                                </div>--%>
                                                                                                    <table width="100%" border="0" align="left" cellpadding="0" cellspacing="0" class="result_txt_t3">
                                                                                                        <tr>
                                                                                                            <td>
                                                                                                                <div id="res3" class="result_txt" style="display: none;">
                                                                                                                    Considering these assumptions, your retirement corpus should be at least: Rs.
                                                                                                                <asp:Label ID="lbtxtPlannedRetireCorpus" runat="server" class="txtLeft" Text="" />
                                                                                                                    <asp:HiddenField ID="HidlbtxtPlannedRetireCorpus" runat="server" Value="0" />
                                                                                                                </div>
                                                                                                            </td>
                                                                                                            <td style="text-align: right; padding-right: 5px;">
                                                                                                                <%-- <input id="btnNext3" style="margin-left: 10px; margin-right: 10px; display: none;"
                                                                                                                type="btnNext3" class="btn btn-mini" value="Next" onclick="Javascript:NextMethod(4);" />--%>
                                                                                                                <img id="btnNext3" style="margin-left: 10px; margin-right: 10px; display: none; cursor: pointer;"
                                                                                                                    src="images/next.png" onclick="Javascript:NextMethod(4);" alt="next" />
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                    </table>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </div>
                                                                                    <div id="content4" class="tabscontent">
                                                                                        <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                                                                                            <tr class="bottom_margin">
                                                                                                <td class="simple_txt" colspan="2">What is your estimated retirement corpus?
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td class="rupee">Rs.
                                                                                                </td>
                                                                                                <td>
                                                                                                    <input name="" id="txtEstRetCorp" runat="server" onkeypress="return isNumeric(event)"
                                                                                                        maxlength="14" type="text" rel="tooltip" class="row_txtbx" />
                                                                                                    <%--tooltiptext="What is your estimated retirement corpus? "--%>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr class="bottom_margin">
                                                                                                <td class="simple_txt" colspan="2">When would you like to start investing?
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td class="rupee">&nbsp;
                                                                                                </td>
                                                                                                <td>
                                                                                                    <%-- <input id="txtRetDate" type="text" style="width: 250px;" />
                                                                                <%--<input name="" type="text" id="txtRetDate" runat="server" placeholder="When do you wish to start investing?" />
                                                                                <span class="add-on"><i id="icon-start-date" title="Select the starting date" class="icon-calendar"
                                                                                    data-date-format="dd MM yyyy" style="cursor: pointer;"></i></span>--%>
                                                                                                    <div class="control-group">
                                                                                                        <div class="controls input-append date form_datetime">
                                                                                                            <input id="txtRetDate" runat="server" type="text" style="width: 252px; margin-left: 2px;" />
                                                                                                            <span class="add-on"><i id="icon-start-date" title="Select the starting date" class="icon-calendar"
                                                                                                                data-date-format="dd MM yyyy" style="cursor: pointer;"></i></span>
                                                                                                        </div>
                                                                                                        <input type="hidden" id="dtp_input1" value="" />
                                                                                                        <br />
                                                                                                    </div>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <%--<<%--tr>
                                                                                            <td>
                                                                                                &nbsp;
                                                                                            </td>
                                                                                            <td class="questn_txt">
                                                                                                &nbsp;
                                                                                            </td>
                                                                                        </tr>--%>
                                                                                            <tr>
                                                                                                <%--<td>
                                                                                                &nbsp;
                                                                                            </td>style="margin-left: 2px;"--%>
                                                                                                <td class="check_txt" colspan="2">How would you like to start investing ?
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td>&nbsp;
                                                                                                </td>
                                                                                                <td>
                                                                                                    <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0" class="top_margin">
                                                                                                        <tr>
                                                                                                            <td>
                                                                                                                <!--start of chkbx tble-->
                                                                                                                <table width="100%" border="0" align="left" cellpadding="0" cellspacing="0">
                                                                                                                    <tr>
                                                                                                                        <td valign="top">
                                                                                                                            <!--start of SIP tble-->
                                                                                                                            <table width="100%" border="0" align="left" cellpadding="0" cellspacing="0">
                                                                                                                                <tr class="bottom_margin2">
                                                                                                                                    <td valign="top">
                                                                                                                                        <%--<input name="" type="checkbox" id="cbMainSip" runat="server" value="" checked="checked" />--%>
                                                                                                                                        <asp:CheckBox ID="cbMainSipS" runat="server" />
                                                                                                                                        <%--Checked="true"--%>
                                                                                                                                    </td>
                                                                                                                                    <td class="check_txt" valign="top">&nbsp;Via regular, systematic investments (SIP)
                                                                                                                                    </td>
                                                                                                                                </tr>
                                                                                                                                <tr>
                                                                                                                                    <td colspan="2">
                                                                                                                                        <div id="divSipMain" runat="server">
                                                                                                                                            <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                                                                                                                <tr class="bottom_margin">
                                                                                                                                                    <td class="simple_txt" colspan="2">Monthly investment amount
                                                                                                                                                    </td>
                                                                                                                                                </tr>
                                                                                                                                                <tr>
                                                                                                                                                    <td class="rupee">Rs.
                                                                                                                                                    </td>
                                                                                                                                                    <td class="top_margin" valign="top">
                                                                                                                                                        <input name="" id="txtMonthlySipAmount" onkeypress="return isNumeric(event)" maxlength="14"
                                                                                                                                                            runat="server" type="text" onblur="" style="width: 200px;" rel="tooltip" class="row_txtbx" />
                                                                                                                                                        <%--tooltiptext="Monthly Investment amount "--%>
                                                                                                                                                    </td>
                                                                                                                                                </tr>
                                                                                                                                                <tr class="bottom_margin">
                                                                                                                                                    <td class="simple_txt" colspan="2">For how many years
                                                                                                                                                    </td>
                                                                                                                                                </tr>
                                                                                                                                                <tr>
                                                                                                                                                    <td class="rupee"></td>
                                                                                                                                                    <td class="rupee">
                                                                                                                                                        <input name="" id="txtSIPYearly" runat="server" onkeypress="return isNumeric(event)"
                                                                                                                                                            maxlength="2" type="text" style="width: 200px;" rel="tooltip" />
                                                                                                                                                        <%--tooltiptext="How Many Yrs?"--%>
                                                                                                                                                    </td>
                                                                                                                                                </tr>
                                                                                                                                            </table>
                                                                                                                                        </div>
                                                                                                                                    </td>
                                                                                                                                </tr>
                                                                                                                            </table>
                                                                                                                            <!--end of SIP tble-->
                                                                                                                        </td>
                                                                                                                        <td valign="top">
                                                                                                                            <!--start of Lump sum tble-->
                                                                                                                            <table width="100%" border="0" align="left" cellpadding="0" cellspacing="0">
                                                                                                                                <tr class="bottom_margin2">
                                                                                                                                    <td>
                                                                                                                                        <%--<input name="" type="checkbox" id="cbMainLs" runat="server" value="" checked="checked" />--%>
                                                                                                                                        <asp:CheckBox ID="cbMainLsS" runat="server" />
                                                                                                                                        <%--Checked="true" --%>
                                                                                                                                    </td>
                                                                                                                                    <td class="check_txt">&nbsp;Via a one time investment (Lump sum)
                                                                                                                                    </td>
                                                                                                                                </tr>
                                                                                                                                <tr>
                                                                                                                                    <td colspan="2">
                                                                                                                                        <div id="divLsMain" runat="server">
                                                                                                                                            <table width="100%" border="0" align="left" cellpadding="0" cellspacing="0">
                                                                                                                                                <tr class="bottom_margin">
                                                                                                                                                    <td class="simple_txt" colspan="2">Investment amount
                                                                                                                                                    </td>
                                                                                                                                                </tr>
                                                                                                                                                <tr>
                                                                                                                                                    <td class="rupee">Rs.&nbsp;&nbsp;&nbsp;
                                                                                                                                                    </td>
                                                                                                                                                    <td>
                                                                                                                                                        <input name="" id="txtLsInvestAmount" onkeypress="return isNumeric(event)" maxlength="14"
                                                                                                                                                            runat="server" type="text" style="width: 200px;" rel="tooltip" class="row_txtbx" /><%-- tooltiptext="Investment Amount "--%>
                                                                                                                                                    </td>
                                                                                                                                                </tr>
                                                                                                                                                <tr>
                                                                                                                                                    <td class="blank_row1">&nbsp;
                                                                                                                                                    </td>
                                                                                                                                                    <td class="blank_row1">&nbsp;
                                                                                                                                                    </td>
                                                                                                                                                </tr>
                                                                                                                                            </table>
                                                                                                                                        </div>
                                                                                                                                    </td>
                                                                                                                                </tr>
                                                                                                                            </table>
                                                                                                                            <!--end of Lump sum tble-->
                                                                                                                        </td>
                                                                                                                    </tr>
                                                                                                                </table>
                                                                                                                <!--end of chkbx tble-->
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                        <tr>
                                                                                                            <td class="questn_txt">
                                                                                                                <%--<input name="" type="checkbox" value="" id="cbTopUp" checked="checked" runat="server" style="margin-right: 5px;" />--%>
                                                                                                                <asp:CheckBox ID="cbTopUpS" runat="server" />
                                                                                                                &nbsp;<span class="check_txt">Top-up your investments at a later stage </span>
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                        <%-- <tr>
                                                                                        <td class="questn_txt">
                                                                                            &nbsp;
                                                                                            <%--<input name="" type="checkbox" value="" id="cbTopUp" checked="checked" runat="server" style="margin-right: 5px;" />--%>
                                                                                                        <%--   <asp:CheckBox ID="CheckBox1" runat="server" />
                                                                                            Top-up your investments at a later stage
                                                                                        </td>
                                                                                    </tr>--%>
                                                                                                        <tr>
                                                                                                            <td>
                                                                                                                <div id="divTopUp" runat="server">
                                                                                                                    <table width="100%">
                                                                                                                        <tr>
                                                                                                                            <td>
                                                                                                                                <!--start of another SIP-->
                                                                                                                                <table width="100%" border="0" align="left" cellpadding="0" cellspacing="0" class="top_margin">
                                                                                                                                    <tr>
                                                                                                                                        <td width="3%">
                                                                                                                                            <%--<input name="" type="checkbox" value="" id="cbAddSip" checked="false" />--%>
                                                                                                                                            <asp:CheckBox ID="cbAddSipS" runat="server" />
                                                                                                                                        </td>
                                                                                                                                        <td width="97%" class="check_txt">Would you like to start another SIP after some time?
                                                                                                                                        </td>
                                                                                                                                    </tr>
                                                                                                                                    <tr>
                                                                                                                                        <%-- <td class="bottom_margin">
                                                                                                                                        &nbsp;
                                                                                                                                    </td>--%>
                                                                                                                                        <td colspan="2">
                                                                                                                                            <div id="divMainAddedSip" runat="server">
                                                                                                                                                <asp:HiddenField ID="HidAddSipRec" runat="server" Value="" />
                                                                                                                                                <table id="tblMainAddedSip" width="100%" border="0" align="left" cellpadding="0"
                                                                                                                                                    cellspacing="0" class="table table-bordered table-condensed">
                                                                                                                                                    <tr class="info">
                                                                                                                                                        <td class="span4">Monthly investment amount
                                                                                                                                                        </td>
                                                                                                                                                        <td class="span3">After no of years
                                                                                                                                                        </td>
                                                                                                                                                        <td class="span3">SIP will last for (years)
                                                                                                                                                        </td>
                                                                                                                                                        <td class="span2">&nbsp;
                                                                                                                                                        </td>
                                                                                                                                                    </tr>
                                                                                                                                                    <tr id="trdivSipAdd0">
                                                                                                                                                        <td colspan="4">
                                                                                                                                                            <div id="divSipAdd0">
                                                                                                                                                                <table width="100%" border="0" align="left" cellpadding="0" cellspacing="0">
                                                                                                                                                                    <tr>
                                                                                                                                                                        <td style="padding-top: 5px; width: 180px; border-left: 1px solid #dddddd;" class="rupee">Rs.<input name="" type="text" id="ntxtSip0" onkeypress="return isNumeric(event)"
                                                                                                                                                                            maxlength="14" runat="server" style="width: 170px; height: 15px;" class="row_txtbx" />
                                                                                                                                                                        </td>
                                                                                                                                                                        <td style="padding-top: 5px; width: 100px;">
                                                                                                                                                                            <input name="" type="text" id="ntxtAfterYear0" onkeypress="return isNumeric(event)"
                                                                                                                                                                                maxlength="2" runat="server" style="width: 100px; height: 15px;" />
                                                                                                                                                                        </td>
                                                                                                                                                                        <td style="padding-top: 5px; width: 100px;">
                                                                                                                                                                            <input name="" type="text" id="ntxtYearlast0" onkeypress="return isNumeric(event)"
                                                                                                                                                                                maxlength="2" runat="server" style="width: 100px; height: 15px;" />
                                                                                                                                                                        </td>
                                                                                                                                                                        <td style="padding-top: 5px; width: 70px;">
                                                                                                                                                                            <div class="span01">
                                                                                                                                                                                <input id="nbuttonAddSip0" onclick="nAddSipLine(this.id)" name="" type="button" style="background: url(images/add.png) no-repeat; height: 16px; width: 16px;" />
                                                                                                                                                                            </div>
                                                                                                                                                                            <div class="span01">
                                                                                                                                                                                <input id="nbuttonRemoveSip0" onclick="nRemoveSipLine(this.id)" name="" type="button"
                                                                                                                                                                                    style="display: none; background: url(images/remove.png) no-repeat; height: 16px; width: 16px;" />
                                                                                                                                                                            </div>
                                                                                                                                                                        </td>
                                                                                                                                                                    </tr>
                                                                                                                                                                </table>
                                                                                                                                                            </div>
                                                                                                                                                        </td>
                                                                                                                                                    </tr>
                                                                                                                                                </table>
                                                                                                                                            </div>
                                                                                                                                        </td>
                                                                                                                                    </tr>
                                                                                                                                </table>
                                                                                                                                <!--start of another SIP-->
                                                                                                                            </td>
                                                                                                                        </tr>
                                                                                                                        <tr>
                                                                                                                            <td>
                                                                                                                                <!--start of another Lump-->
                                                                                                                                <table width="100%" border="0" align="left" cellpadding="0" cellspacing="0" class="top_margin">
                                                                                                                                    <tr>
                                                                                                                                        <td width="3%">
                                                                                                                                            <%--<input name="" type="checkbox" value="" id="cbAddLs" checked="false" />--%>
                                                                                                                                            <asp:CheckBox ID="cbAddLsS" runat="server" />
                                                                                                                                        </td>
                                                                                                                                        <td width="97%" class="check_txt">Would you like to invest more via Lump sum investments after some time?
                                                                                                                                        </td>
                                                                                                                                    </tr>
                                                                                                                                    <tr>
                                                                                                                                        <%-- <td class="bottom_margin">
                                                                                                                                        &nbsp;
                                                                                                                                    </td>--%>
                                                                                                                                        <td colspan="2">
                                                                                                                                            <div id="divMainAddedLS">
                                                                                                                                                <asp:HiddenField ID="HidAddLsRec" Value="" runat="server" />
                                                                                                                                                <table id="tblMainAddedLS" width="100%" border="0" align="left" cellpadding="0" cellspacing="0"
                                                                                                                                                    class="table table-bordered table-striped">
                                                                                                                                                    <tr class="info">
                                                                                                                                                        <td class="span4">Investment amount
                                                                                                                                                        </td>
                                                                                                                                                        <td class="span4">After no of years
                                                                                                                                                        </td>
                                                                                                                                                        <td class="span2">&nbsp;
                                                                                                                                                        </td>
                                                                                                                                                    </tr>
                                                                                                                                                    <tr id="trdivLsAdd0">
                                                                                                                                                        <td colspan="3">
                                                                                                                                                            <div id="divLsAdd0">
                                                                                                                                                                <table width="100%" border="0" align="left" cellpadding="0" cellspacing="0">
                                                                                                                                                                    <tr>
                                                                                                                                                                        <td style="padding-top: 5px; width: 200px; border-left: 1px solid #dddddd;" class="rupee">Rs.
                                                                                                                                                                        <input name="" id="ntxtLsInvstmnt0" onkeypress="return isNumeric(event)" maxlength="14"
                                                                                                                                                                            runat="server" type="text" style="width: 170px; height: 15px;" class="row_txtbx" />
                                                                                                                                                                        </td>
                                                                                                                                                                        <td style="padding-top: 5px; width: 180px;">
                                                                                                                                                                            <input name="" id="ntxtAfterYearLs0" onkeypress="return isNumeric(event)" maxlength="2"
                                                                                                                                                                                runat="server" type="text" style="width: 170px; height: 15px;" />
                                                                                                                                                                        </td>
                                                                                                                                                                        <td style="padding-top: 5px; width: 70px;">
                                                                                                                                                                            <div class="span01">
                                                                                                                                                                                <input id="nbuttonAddLs0" onclick="nAddLsLine(this.id)" name="" type="button" style="background: url(images/add.png) no-repeat; height: 16px; width: 16px;" />
                                                                                                                                                                            </div>
                                                                                                                                                                            <div class="span01">
                                                                                                                                                                                <input id="nbuttonRemoveLs0" onclick="nRemoveLsLine(this.id)" name="" type="button"
                                                                                                                                                                                    style="display: none; background: url(images/remove.png) no-repeat; height: 16px; width: 16px;" />
                                                                                                                                                                            </div>
                                                                                                                                                                        </td>
                                                                                                                                                                    </tr>
                                                                                                                                                                </table>
                                                                                                                                                            </div>
                                                                                                                                                        </td>
                                                                                                                                                    </tr>
                                                                                                                                                </table>
                                                                                                                                            </div>
                                                                                                                                        </td>
                                                                                                                                    </tr>
                                                                                                                                </table>
                                                                                                                            </td>
                                                                                                                        </tr>
                                                                                                                    </table>
                                                                                                                </div>
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                    </table>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td>&nbsp;
                                                                                                </td>
                                                                                                <td class="questn_txt">
                                                                                                    <table width="100%" border="0" align="left" cellpadding="0" cellspacing="0">
                                                                                                        <tr>
                                                                                                            <td class="btn_margin2">To know how much time it will take to achieve your retirement corpus
                                                                                                            <input id="calculatebtn" type="button" class="btn btn-primary" style="margin-left: 2px;"
                                                                                                                value="Submit" onclick="return CalculateTimeRetrCorpsPage(); CallAllMethod()" />
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                    </table>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td class="btn_margin" colspan="2">&nbsp;
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td colspan="2" align="left" style="padding-right: 45px;">
                                                                                                    <div id="res4" class="result_txt_t3" style="display: none;">
                                                                                                        <span id="txtSpanRes"></span>You are likely to achieve your retirement corpus in
                                                                                                    <%--<input name="" type="text" id="txtTotalTimeReqRetire" runat="server" style="border: none; width: 50px;" />--%>
                                                                                                        <asp:Label ID="lbTotalTimeReqRetire" runat="server" class="result_txt1" Text="" />,
                                                                                                    <asp:HiddenField ID="HidlbTotalTimeReqRetire" runat="server" />
                                                                                                        which will be by
                                                                                                    <asp:Label ID="lbExpTimeReqRetire" runat="server" class="result_txt1" Text="" />
                                                                                                        <asp:HiddenField ID="HidlbExpTimeReqRetire" runat="server" />
                                                                                                    </div>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td class="btn_margin" colspan="2">&nbsp;
                                                                                                </td>
                                                                                            </tr>
                                                                                            <%-- <tr>
                                                                                            <td colspan="2">
                                                                                                <div id="divOutputbluetext" class="blue_txt" style="display: none">
                                                                                                    <img src="images/valid.png" />&nbsp;Congratulations, you will achieve your retirement
                                                                                                    corpus before your retirement age!
                                                                                                </div>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td colspan="2">
                                                                                                <div id="divOutputredtext" class="red_txt" style="display: none">
                                                                                                    <img src="images/error.png" />&nbsp;You will need to re-look at your retirement
                                                                                                    plan, because you will not be able to achieve your retirement corpus before your
                                                                                                    retirement age!
                                                                                                </div>
                                                                                            </td>
                                                                                        </tr>--%>
                                                                                        </table>
                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="btn_margin1" style="width: 90%; padding-left: 30px;">
                                                                            <input id="btnDownload" type="button" class="btn btn-small btn-primary" value="Download Report"
                                                                                onclick="Javascript: return CheckValdPDFGenerate();" style="display: none" />
                                                                            <span id="spanbtnDownload" class="simple_txt" style="display: none">Your customized
                                                                            retirement plan snapshot is now available for download.</span>
                                                                            <p style="height: 5px;" />
                                                                            <%--<asp:Button ID="btnDownloadPdf" runat="server" Text="Download Report" CssClass="btn btn-small btn-primary"
                                                            OnClientClick="Javascript:return CheckValdPDFGenerate();" OnClick="PDFGenerate_Click" />--%>
                                                                            <%--<a class=" btn btn-small btn-primary" href="#"><i class="icon-download icon-white"></i>
                                                            &nbsp;Download Report</a>--%>
                                                                            <%--<span ID="lblPdfReporttext" style="display:inherit"  Text="Please visit the other sections to be able to generate your customized retirement plan report."></span>--%>
                                                                            <%--<label id="lblPdfReporttext" class="note" style="display:inherit; padding-left:30px;">
                                                                            Note: Please go through all the four steps above to generate your customized retirement
                                                                            plan report.</label>--%>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="width: 90%; padding-left: 25px;">
                                                                            <label id="lblPdfReporttext" class="note" style="display: inherit;">
                                                                                Note: Please go through all the four steps above to generate your customized retirement
                                                                            plan report.</label>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <div id="Showpdfdiv" runat="server" style="display: none;">
                                                                                <table width="90%" class="pdf" border="0" align="center" cellpadding="0" cellspacing="0">
                                                                                    <tr>
                                                                                        <td>
                                                                                            <table width="100%" style="padding-top: 40px;">
                                                                                                <tr>
                                                                                                    <td align="left" width="100%">
                                                                                                        <br />
                                                                                                        <asp:RadioButtonList Visible="false" ID="RadioButtonListCustomerType" runat="server"
                                                                                                            OnSelectedIndexChanged="RadioButtonListCustomerType_SelectedIndexChanged" TextAlign="Right"
                                                                                                            RepeatDirection="Horizontal" AutoPostBack="true" BorderColor="#569fd3" BorderWidth="0"
                                                                                                            Width="250px" RepeatLayout="Table" CssClass="RBL" CellSpacing="0" CellPadding="0">
                                                                                                            <asp:ListItem> Distributor</asp:ListItem>
                                                                                                            <asp:ListItem Selected="true"> Not a Distributor</asp:ListItem>
                                                                                                        </asp:RadioButtonList>
                                                                                                        <asp:HiddenField ID="HidDist" runat="server" />
                                                                                                        <%--OnChanged="SetdivVisible();"--%>
                                                                                                        <div id="divDist" class="RBL" runat="server" style="text-align: center">
                                                                                                            <input type="radio" name="radio-choice-1" data-theme="d" id="radioDist" value="Distributor"
                                                                                                                class="RBL" /><label for="radio1" class="RBL" style="margin-left: 10px;">Distributor</label>
                                                                                                            <input type="radio" name="radio-choice-1" data-theme="d" id="radioWoDist" value="Not a Distributor"
                                                                                                                checked="checked" class="RBL" style="margin-left: 30px;" /><label for="radio2" class="RBL"
                                                                                                                    style="margin-left: 10px;">Not a Distributor</label>
                                                                                                        </div>
                                                                                                        <br />
                                                                                                    </td>
                                                                                                </tr>
                                                                                            </table>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <div id="DistributerDiv" runat="server" style="display: none">
                                                                                                <table id="tblDistb" width="100%" align="center">
                                                                                                    <tr>
                                                                                                        <td width="32%" align="left" class="simple_txt" style="padding-left: 30px;" cellpadding="0">ARN No
                                                                                                        </td>
                                                                                                        <td width="68%" align="left">
                                                                                                            <table width="100%" border="0" align="left" cellpadding="0" cellspacing="0">
                                                                                                                <tr>
                                                                                                                    <td class="rupee_blue">&nbsp;
                                                                                                                    </td>
                                                                                                                    <td>
                                                                                                                        <label>
                                                                                                                            <input type="text" name="textfield" id="txtArn1" class="row_txtbxText" autocomplete="off"
                                                                                                                                onkeypress="return isNumber(event);" runat="server" maxlength="30" /></label>
                                                                                                                    </td>
                                                                                                                </tr>
                                                                                                            </table>
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                    <tr>
                                                                                                        <td width="32%" align="left" class="simple_txt" style="padding-left: 30px;">Prepared By
                                                                                                        </td>
                                                                                                        <td width="68%" align="left">
                                                                                                            <table width="100%" border="0" align="left" cellpadding="0" cellspacing="0">
                                                                                                                <tr>
                                                                                                                    <td class="rupee_blue">&nbsp;
                                                                                                                    </td>
                                                                                                                    <td>
                                                                                                                        <label>
                                                                                                                            <input type="text" name="textfield" id="txtPreparedby" autocomplete="off" runat="server"
                                                                                                                                maxlength="40" onblur="ContainsLetterWithMessage(this)" /></label>
                                                                                                                    </td>
                                                                                                                </tr>
                                                                                                            </table>
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                    <tr>
                                                                                                        <td width="32%" align="left" style="padding-left: 30px;" class="simple_txt">Contact No(Mobile)
                                                                                                        </td>
                                                                                                        <td width="68%" align="left">
                                                                                                            <table width="100%" border="0" align="left" cellpadding="0" cellspacing="0">
                                                                                                                <tr>
                                                                                                                    <td class="rupee_blue">&nbsp;
                                                                                                                    </td>
                                                                                                                    <td>
                                                                                                                        <label>
                                                                                                                            <input type="text" name="textfield" id="txtMobile" autocomplete="off" onkeypress="return isNumber(event);"
                                                                                                                                runat="server" maxlength="14" /></label>
                                                                                                                    </td>
                                                                                                                </tr>
                                                                                                            </table>
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                    <tr>
                                                                                                        <td width="32%" align="left" style="padding-left: 30px;" class="simple_txt">Email
                                                                                                        </td>
                                                                                                        <td width="68%" align="left">
                                                                                                            <table width="100%" border="0" align="left" cellpadding="0" cellspacing="0">
                                                                                                                <tr>
                                                                                                                    <td class="rupee_blue">&nbsp;
                                                                                                                    </td>
                                                                                                                    <td>
                                                                                                                        <label>
                                                                                                                            <input type="text" name="textfield" id="txtEmail" autocomplete="off" runat="server"
                                                                                                                                maxlength="30" onblur="CheckEmail(this.value)" /></label>
                                                                                                                    </td>
                                                                                                                </tr>
                                                                                                            </table>
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                    <tr>
                                                                                                        <td width="32%" align="left" style="padding-left: 30px;" class="simple_txt">Prepared For
                                                                                                        </td>
                                                                                                        <td width="68%" align="left">
                                                                                                            <table width="100%" border="0" align="left" cellpadding="0" cellspacing="0">
                                                                                                                <tr>
                                                                                                                    <td class="rupee_blue">&nbsp;
                                                                                                                    </td>
                                                                                                                    <td>
                                                                                                                        <label>
                                                                                                                            <input type="text" name="textfield" id="txtPreparedFor" autocomplete="off" onblur="ContainsLetterWithMessage(this)"
                                                                                                                                runat="server" maxlength="40" /></label>
                                                                                                                    </td>
                                                                                                                </tr>
                                                                                                            </table>
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                    <tr>
                                                                                                        <td colspan="2" style="height: 20px;"></td>
                                                                                                    </tr>
                                                                                                </table>
                                                                                            </div>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <table width="100%" border="0" align="left" cellpadding="0" cellspacing="0">
                                                                                                <tr>
                                                                                                    <td colspan="2">
                                                                                                        <table width="100%" border="0" align="left" cellpadding="0" cellspacing="0">
                                                                                                            <tr>
                                                                                                                <td width="32%" align="left" style="padding-left: 30px;" class="simple_txt">Generate PDF Report:
                                                                                                                </td>
                                                                                                                <td width="68%" align="left">
                                                                                                                    <%--<asp:LinkButton ID="LinkButton1" runat="server" OnClick="LBGenerateReport_Click"
                                                                                                                    ToolTip="Download PDF" OnClientClick="javascript:return pdfcheck();">
                                                                                                                    <img src="images/downloadPDF.png" style="border: 0;" alt="" width="25" height="25" />
                                                                                                                </asp:LinkButton>--%>
                                                                                                                    <asp:Button ID="LinkButton1" runat="server" Text="Download Report" class="btn-small btn-primary1"
                                                                                                                        OnClientClick="javascript:return pdfcheck();" OnClick="LBGenerateReport_Click" />
                                                                                                                    <%--   <input type="button" runat="server" class="btn-small btn-primary1" value="Download Report" id="LinkButton1"
                                                                                                                    onClientClick="javascript:return pdfcheck();" OnClick="LBGenerateReport_Click" />--%>
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                        </table>
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td colspan="2" style="height: 15px;"></td>
                                                                                                </tr>
                                                                                            </table>
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>&nbsp;
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                        </div>
                    </td>
                </tr>
            </table>
        </div>
    </form>
    <script type="text/javascript">

        $('[Placeholder]').focus(function () {
            var input = $(this);
            if (input.val() == input.attr('Placeholder')) {
                input.val('');
                input.removeClass('Placeholder');
            }
        }).blur(function () {
            var input = $(this);
            if (input.val() == '' || input.val() == input.attr('Placeholder')) {
                input.addClass('Placeholder');
                input.val(input.attr('Placeholder'));
            }
        }).blur().parents('form').submit(function () {
            $(this).find('[Placeholder]').each(function () {
                var input = $(this);
                if (input.val() == input.attr('Placeholder')) {
                    input.val('');
                }
            })
        });
        var tooltipObj = new DHTMLgoodies_formTooltip();
        tooltipObj.setTooltipPosition('right');
        tooltipObj.setPageBgColor('#EEEEEE');
        tooltipObj.setTooltipCornerSize(10);
        tooltipObj.initFormFieldTooltip();
    </script>
</body>
</html>
