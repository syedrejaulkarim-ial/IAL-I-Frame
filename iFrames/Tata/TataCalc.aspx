<%@ Page Language="C#" AutoEventWireup="true" ViewStateEncryptionMode="Always" EnableViewStateMac="true" CodeBehind="TataCalc.aspx.cs" Inherits="iFrames.Tata.TataCalc" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="../Styles/jquery-ui-1.8.14.customtata.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" language="javascript" src="../Scripts/jquery110.js"></script>
    <script type="text/javascript" language="javascript" src="../Scripts/jquery-ui.min110.js"></script>
    <style type="text/css">
        body
        {
            background: url(../Images/tatabg.gif) repeat;
            font-family: "Verdana" , Geneva, sans-serif;
            font-size: 14px;
            margin: 0;
            padding: 0;
        }
        
        .table1
        {
            border-left: #265599 solid 1px;
            border-right: #265599 solid 1px;
            border-bottom: #265599 solid 1px;
            background: #FFFFFF;
        }
        .gridHeader
        {
            background: #265599;
            color: #FFFFFF;
            font-weight: bold;
        }
        .row1
        {
            background: #f5f3f3;
            color: #353131;
            border-bottom: #999999 1px dotted;
        }
        .row2
        {
            background: #fff;
            color: #353131;
            border-bottom: #999999 1px dotted;
        }
        
        .border_drop
        {
            border: #002f73 solid 1px;
            padding: 0;
            width: 600px;
            text-align: left;
            overflow: auto;
            height: 110px;
        }
        .button
        {
            background: url(../Images/btn.jpg) repeat-x;
            color: #fff;
            font-family: Verdana;
            font-size: 11;
            font-weight: bold;
            height: 25px;
            width: 139px;
            padding: 0;
            border: none;
            margin-left: 0px;
            margin-right: 5px;
        }
        .disclaimercss
        {
            text-align: justify; font-family: Verdana;
                                        font-size: 11px;
        }
        .style12
        {
            color: #FFFFFF;
            font-weight: bold;
        }
        .style4
        {
            font-family: Tahoma;
            font-size: 12px;
        }
        -- ></style>
    <script type="text/javascript" language="javascript">
        function SelectAll(CheckBoxControl) {
            // alert("test");
            if (CheckBoxControl.checked == true) {

                var i;
                for (i = 0; i < document.forms[0].elements.length; i++) {
                    if ((document.forms[0].elements[i].type == 'checkbox') && (document.forms[0].elements[i].name.indexOf('dgSchemeList') > -1)) {

                        document.forms[0].elements[i].checked = true;
                    }
                }
            }
            else {
                var i;
                for (i = 0; i < document.forms[0].elements.length; i++) {
                    if ((document.forms[0].elements[i].type == 'checkbox') && (document.forms[0].elements[i].name.indexOf('dgSchemeList') > -1)) {
                        document.forms[0].elements[i].checked = false;
                    }
                }
            }
        }

        function mouseEnter(controlName, staticLength) {
            //alert(staticLength);
            var maxlength = 0;
            var mySelect = document.getElementById(controlName);
            for (var i = 0; i < mySelect.options.length; i++) {
                if (mySelect[i].text.length > maxlength) {
                    maxlength = mySelect[i].text.length;
                }
            }

            var ddlwidth = maxlength * 6;
            if (ddlwidth < 300)
                ddlwidth = 300;
           // alert(ddlwidth);

            if (maxlength != 0)
                mySelect.style.width = ddlwidth;

        }
        function focusOut(controlName, staticLength) {
           // alert(staticLength);
            var mySelect = document.getElementById(controlName);
            if (staticLength < 300)
                staticLength = 300;
            //alert(mySelect.style.width);
            mySelect.style.width = staticLength;
        }

        //        function selectAll
        //        {
        //            SelectAll('SelectAllCheckBox');
        //            alert("5");
        //        }
    </script>
    <script type="text/javascript" language="javascript">



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

        function date_validate() {

            var sipStartdata = document.getElementById('<%=ddlQtrEnd.ClientID%>').value + document.getElementById('<%=ddlYearEnd.ClientID%>').value;
            // alert(sipStartdata);
            if (sipStartdata == "") {
                alert("Value as of Date cannot be Blank");

                return false;
            }


            var selectedScheme = document.getElementById('<%=ddlSchemeList.ClientID%>').value

            if (selectedScheme == "--") {
                alert("Please select a Scheme..");
                return false;
            }

            var sipamunt = document.getElementById('<%=txtInvestment.ClientID%>').value;
            if (sipamunt == "") {
                alert("Value of Investment cannot be Blank");
                document.getElementById('<%=txtInvestment.ClientID%>').focus();
                return false;
            }

            if (sipamunt.indexOf(".") > -1) {
                alert("Please Enter Integral value");
                document.getElementById('<%=txtInvestment.ClientID%>').value = "";
                document.getElementById('<%=txtInvestment.ClientID%>').focus();
                return false;
            }

            if (isNaN(sipamunt)) {
                alert("Please enter Numeric value.");
                document.getElementById('<%=txtInvestment.ClientID%>').value = "";
                document.getElementById('<%=txtInvestment.ClientID%>').focus();
                return false;
            }
            else {
                if (sipamunt == 0) {
                    alert("Please enter Valid Numeric value.");
                    document.getElementById('<%=txtInvestment.ClientID%>').value = "";
                    document.getElementById('<%=txtInvestment.ClientID%>').focus();
                    return false;
                }
            }

            var Day = parseInt(sipStartdata.substring(3, 5), 10);
            var Mn = parseInt(sipStartdata.substring(0, 2), 10);
            var Yr = parseInt(sipStartdata.substring(6, 10), 10);
            var DateVal = Mn + "/" + Day + "/" + Yr;
            var dt = new Date(DateVal);

            var todaydate = new Date();

            var i = DateDiff.inDays(dt, todaydate)
            if (i <= 0) {
                alert("Value as of Date should be Less than Today");
                return false;
            }

            var startsensexdate = new Date("01/01/1980");
            if (dt < startsensexdate) {
                alert("Please select Value as of Date from 1st January 1980");
                return false;
            }

            //            var booltest = false;

            //            for (i = 0; i < document.forms[0].elements.length; i++) {
            //                if ((document.forms[0].elements[i].type == 'checkbox') && (document.forms[0].elements[i].name != "SelectAllCheckBox")) {
            //                    //alert(document.forms[0].elements[i].name);
            //                    if (document.forms[0].elements[i].checked == true) {
            //                        booltest = true;

            //                    }

            //                }
            //            }

            //            if (booltest == false) {
            //                alert("Please Select at Least One Scheme from the List");
            //                return false;
            //            }

            return true;
        }

    </script>
    <style type="text/css">
        .paneltext
        {
            margin: 0 0 0 10px;
            width: 95%;
        }
        
        .sipGrid2
        {
            left: auto;
            margin-left: 3%;
            width: 94%;
            border: 1px solid #99CCFF;
            font-family: Verdana;
            font-size: 11px;
        }
        .CssTableNote
        {
            text-align: justify;
            font-family: Arial;
            font-size: 12px;
            width: 84%;
            margin-left: 8%;
            margin-right: 8%;
        }
        .collapsibleContainer
        {
            font-family: Arial;
        }
        .resultgrid
        {
            width: 99%;
            font-family: Verdana;
            font-size: 11px;
            height: 53px;
            margin-left: .5%;
            margin-right: .5%;
            top: -50px;
        }
        .ListRow
        {
            background-color: #A0C0DF;
            font-size: 11px;
        }
        
        .AltRow
        {
            background-color: #f4fbff;
            font-size: 11px;
        }
        
        .headerCss
        {
            background: #1068B2;
            color: #FFFFFF;
            font-weight: bold;
            border: #012258 solid 1px;
        }
        
        .Ldivstyle
        {
            border: 1px solid #99CCFF;
            background-color: #99CCFF;
        }
        
        
        #SipCalDiv
        {
            height: 92px;
        }
        .txtboxstyle
        {
            width: 290px;
            font-size: 12px;
            color: #265599;
        }
        .tdstyle1
        {
            padding-left: 12px;
            text-align: left;
            font-size: 11px;
            width: 30%;
            vertical-align: middle;
            padding-top: 10px;
            padding-bottom: 10px;
        }
        .tdstyle2
        {
            text-align: left;
            width: 70%;
            font-size: 11px;
            vertical-align: middle;
        }
        .tdstyleSch
        {
            text-align: left;
            width: 70%;
            font-size: 11px;
            vertical-align: middle;
            padding: 0;
        }
        
        .ddlstyle
        {
            width: 300px;
            font-family: Verdana;
            font-size: 11px;
            height: 20px;
            color: #265599;
        }
        
        .ddlstyleInvest
        {
            width: 170px;
            font-family: Arial;
            font-size: 11px;
            height: 20px;
            color: #265599;
        }
        
        .pnlCSS
        {
            font-weight: bold;
            cursor: pointer;
            border: solid 1px #c0c0c0;
            width: 30%;
        }
        .cpHeader
        {
            color: white;
            background-color: #719DDB;
            font: bold 11px auto "Trebuchet MS" , Arial;
            font-size: 12px;
            cursor: pointer;
            width: 450px;
            height: 18px;
            padding: 4px;
        }
        .cpBody
        {
            background-color: #DCE4F9;
            font: normal 11px auto Arial;
            border: 1px gray;
            width: 450px;
            padding: 4px;
            padding-top: 7px;
        }
        
        .blackfnt
        {
            font: 11px;
            font-family: Tahoma;
            padding-left: 12px;
            font-style: bold;
        }
        .ddlcolor
        {
            color: #265599;
            max-height: 30;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table width="736px" border="0" cellspacing="0" cellpadding="0" align="center" class="table1">
            <tr>
                <td>
                    <table width="100%" border="0" cellspacing="0" cellpadding="0" align="center">
                        <%--<tr>
                            <td>
                                <table width="100%" border="0" cellspacing="0" cellpadding="0" align="center">
                                    <tr>
                                        <td width="50%" style="padding-left: 15px; padding-bottom: 5px; padding-top: 5px;">
                                            <img src="../Images/tatamf.jpg" />
                                        </td>
                                        <td width="50%" align="right" style="padding-right: 20px;">
                                            <img src="../Images/tata.gif" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>--%>
                       <%-- <tr>
                            <td background="../Images/heading.jpg" height="26" width="960">
                                <span class="style12">&nbsp;&nbsp;<span class="style4">TATA Mutual Fund </span></span>
                            </td>
                        </tr>--%>
                        <tr>
                            <td>
                                <table id="ContentTable" cellspacing="0px" width="100%">
                                    <tr>
                                        <td colspan="2" style="height: 16px;">
                                        </td>
                                    </tr>
                                    <tr style="display:none">
                                        <td class="tdstyle1">
                                            <span class="blackfnt" id="lblScheme"><b>Scheme Type</b></span>
                                        </td>
                                        <td class="tdstyle2">
                                            <asp:DropDownList ID="ddlNature" runat="server" AutoPostBack="true" DataTextField="Nature"
                                                DataValueField="Nature" CssClass="ddlstyle" OnSelectedIndexChanged="ddlNature_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr style="display:none">
                                        <td class="tdstyle1">
                                            <span class="blackfnt" id="Span2"><b>Option of the Investment</b></span>
                                        </td>
                                        <td class="tdstyle2">
                                            <asp:DropDownList ID="ddlOption" runat="server" AutoPostBack="true" CssClass="ddlstyle"
                                                OnSelectedIndexChanged="ddlOption_SelectedIndexChanged">
                                                <asp:ListItem Selected="True">--</asp:ListItem>
                                                <asp:ListItem Value="2">Growth </asp:ListItem>
                                                <asp:ListItem Value="3">Dividend Reinvestment</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdstyle1">
                                            <span id="Label28" class="blackfnt"><b>Name of the Scheme</b></span>
                                        </td>
                                        <td class="tdstyleSch">
                                            <asp:DropDownList ID="ddlSchemeList" runat="server" DataTextField="Sch_Short_Name"
                                                DataValueField="Scheme_Id" OnSelectedIndexChanged="ddlSchemeList_SelectedIndexChanged"
                                                AutoPostBack="true" CssClass="ddlstyle">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdstyle1">
                                            <span id="Span4" class="blackfnt"><b>Scheme BenchMark</b></span>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddBenchMark" runat="server" CssClass="ddlstyle">
                                                <asp:ListItem Selected="True">--</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdstyle1">
                                            <span id="Span1" class="blackfnt"><b>Fund Manager</b></span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="FundmanegerText" class="txtboxstyle" runat="server" ReadOnly="True">
                                            </asp:TextBox>
                                            <%--<asp:DropDownList ID="ddlFunManager" runat="server" AutoPostBack="True" CssClass="ddlstyle"
                                                OnSelectedIndexChanged="ddlFunManager_SelectedIndexChanged">
                                            </asp:DropDownList>--%>
                                            <%--onChange ="javascript: SelectAll(this);"--%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdstyle1">
                                            <span id="Span5" class="blackfnt"><b>Value of Investment( Rs.)</b></span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtInvestment" runat="server" CssClass="txtboxstyle" Text="10000">
                                            </asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdstyle1">
                                            <span id="Span3" class="blackfnt"><b>Value as of Date</b></span>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlQtrEnd" runat="server" CssClass="ddlcolor">
                                            </asp:DropDownList>
                                            <asp:DropDownList ID="ddlYearEnd" runat="server" CssClass="ddlcolor" AutoPostBack="true"
                                                onselectedindexchanged="ddlYearEnd_SelectedIndexChanged">
                                            </asp:DropDownList>
                                            <%-- onmouseover="this.size=10;" onmouseout="this.size=1;"--%>
                                            <asp:Label Style="vertical-align: middle; color: Red; font-weight: bold; margin-left: 20px;"
                                                ID="lblMessage" runat="server" Text=""></asp:Label>
                                        </td>
                                    </tr>
                                    <tr id="trset" runat="server" visible="false">
                                       <td class="tdstyle1">
                                            <span id="Span6" class="blackfnt"><b>Dividend Type</b></span>
                                        </td>
                                        <td align="left"  style="font-size: 12px;">
                                            <asp:RadioButtonList runat="server" Id="radioList" CssClass="blackfnt" 
                                                RepeatDirection="Horizontal" Width="263px">
                                                <asp:ListItem Selected="True" Value="Individual & HUF">
                                                </asp:ListItem>
                                                <asp:ListItem Value="Corporate">
                                                </asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <%--<asp:Button ID="btnExcel" runat="server" Text="ExporttoExcel" 
                                            onclick="ExportExcelClick" Visible="false" />--%>
                                        </td>
                                        <td style="padding-bottom: 10px; padding-top: 10px;">
                                            <asp:Button ID="btnLumpCal" runat="server" Text=" Calculate Return" CssClass="button"
                                                title="Calculate return" OnClientClick="return date_validate();" OnClick="btnLumpCal_Click" />
                                            <asp:Button ID="Resetbtn" runat="server" Text="Reset" OnClick="btnResetForm" title="Reset"
                                                CssClass="button" />
                                            <asp:Button ID="lnkbtnDownload" runat="server" CssClass="button" Text="Download Excel"
                                                OnClick="ExportExcelClick" Visible="false"></asp:Button>
                                            <%--<asp:LinkButton ID="lnkbtnDownload" runat="server" OnClick="ExportExcelClick" Visible="false">
                                                                <img src="../Images/excel_icon.png" style="border: 0; margin-left:250px " alt="" width="25" height="25"  title="Download excel"/></asp:LinkButton>--%>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:Literal ID="LiteralFinalReturns" runat="server"></asp:Literal>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table width='100%'>
                                    <tr>
                                        <td colspan='6'>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align='left' colspan='6' background='../Images/heading.jpg' height='22' width='500'>
                                            <span class='style12'>&nbsp;&nbsp;<span class='style4'><img src='../Images/arrow.jpg' />
                                                Disclaimer</span></span><br />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align='centre' style='font: verdena; font-size: 11px; padding-left: 20px;'>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            </tr>
                              <tr>
                            <td style="padding-left:20px; padding-right:20px;">                          
                            
                                <asp:Label ID="LiteralDisclaimer" runat="server" Text="" CssClass="disclaimercss"></asp:Label>
                            </td>
                        </tr>
                            <tr>
                                <td style="padding-left:20px; padding-right:20px;">
                                    <div id="disclaimerDiv" runat="server" class="disclaimercss" >
                                        • <b>Past performance may or may not be sustained in future.</b> • Absolute returns is
                                        computed on investment is of Rs 10,000. • For computation of since inception returns
                                        the allotment NAV has been taken as Rs. 10.00 (Except for Tata Liquid Fund, Tata
                                        Floater Fund, Tata Treasury Manager Fund & Tata Liquidity Management Fund where
                                        NAV is taken as Rs. 1,000). • All payouts during the period have been reinvested
                                        in the units of the scheme at the then prevailing NAV. • Load is not considered
                                        for computation of returns. While calculating returns dividend distribution tax
                                        is excluded. • In case, the start/end date of the concerned period is non-business
                                        date, the benchmark value of the previous date is considered for computation of
                                        returns. • “N/A” - Not Available. • Schemes in existence for > 1 year performance
                                        provided for as many 12 months period as possible. • Please also refer to performance
                                        details of other schemes of Tata Mutual Fund managed by the Fund Manager(s)
                                        of this Scheme. • The Calculators provided on the website is for information purposes
                                        only and is not an offer to sell or a solicitation to buy any mutual fund units/securities.
                                        The recipient of this information should rely on their investigations and take professional
                                        advice before making any investment. • The Calculators alone are not sufficient
                                        and shouldn’t be used for the development or implementation of an investment strategy.
                                        It should not be construed as investment advice to any party. • Neither Tata Asset
                                        Management Ltd, nor any person connected with it, accepts any liability arising
                                        from the use of this information. • In view of individual nature of tax consequences,
                                        each investor is advised to consult his/ her own professional tax advisor. • <b>Mutual
                                        Fund Investments are subject to market risks, read all Scheme related documents
                                        carefully.</b>
                                    </div>
                                    <br />
                                </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
