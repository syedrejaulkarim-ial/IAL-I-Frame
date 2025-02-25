<%@ Page Language="C#" AutoEventWireup="true" ViewStateEncryptionMode="Always" EnableViewStateMac="true" CodeBehind="ReturnCalc.aspx.cs" Inherits="iFrames.Top3choice.ReturnCalc" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Return Calculator</title>
    <link href="../Styles/jquery-ui-1.8.14.customtata.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" language="javascript" src="../Scripts/jquery-14min.js"></script>
    <script type="text/javascript" language="javascript" src="../Scripts/jquery-ui-18custom.js"></script>
    <style type="text/css">
        body
        {
            background: transparent;
             font-family: Arial, Helvetica, sans-serif;
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
            border:#000 solid 1px;
            padding: 0;
            width: 600px;
            text-align: left;
            overflow: auto;
            height: 110px;
        }
        .button
        {
            background:#df0101;
            color: #fff;
            font-family: Verdana;
            font-size: 13px;
            padding:5px;
            padding-left:8px;
            padding-right:8px;
            border-radius:4px;
            border:#980000 solid 1px;
           
        }
        .heading_nw{background:#004fa3; height:22px; color:#fff; border-bottom:#df0101 solid 3px; font-size:15px;}
        .rslt_text1
        {
            font-family: ,Arial, Helvetica, sans-serif;
            color: #333333;
            font-size: 11px;
            font-weight: normal;
            padding: 4px;
        }
        .style12
        {
            color: #FFFFFF;
            font-weight: bold;
        }
        .style4
        {
             font-family: Arial, Helvetica, sans-serif;
            font-size: 12px;
        }
          .disclaimercss
        {
            text-align: justify;  font-family: Arial, Helvetica, sans-serif;
                                        font-size: 11px;
        }
       </style>
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



        //        function selectAll
        //        {
        //            SelectAll('SelectAllCheckBox');
        //            alert("5");
        //        }
    </script>
    <script type="text/javascript" language="javascript">
        $(function () {
            $("#txtStartDate").datepicker({
                dateFormat: 'dd-MM-yy',
                changeMonth: true,
                changeYear: true,
                maxDate: 0
            });
        });


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
            var sipStartdata = document.getElementById('<%=txtStartDate.ClientID%>').value;
            if (sipStartdata == "") {
                alert("Value as of Date cannot be Blank");
                document.getElementById('<%=txtStartDate.ClientID%>').focus();
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
                document.getElementById('<%=txtStartDate.ClientID%>').value = "";
                document.getElementById('<%=txtStartDate.ClientID%>').focus();
                return false;
            }

            var startsensexdate = new Date("01/01/1980");
            if (dt < startsensexdate) {
                alert("Please select Value as of Date from 1st January 1980");
                document.getElementById('<%=txtStartDate.ClientID%>').value = "";
                document.getElementById('<%=txtStartDate.ClientID%>').focus();
                return false;
            }

            var booltest = false;

            for (i = 0; i < document.forms[0].elements.length; i++) {
                if ((document.forms[0].elements[i].type == 'checkbox') && (document.forms[0].elements[i].name != "SelectAllCheckBox")) {
                    //alert(document.forms[0].elements[i].name);
                    if (document.forms[0].elements[i].checked == true) {
                        booltest = true;

                    }

                }
            }

            if (booltest == false) {
                alert("Please Select at Least One Scheme from the List");
                return false;
            }

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
            font-family: Verdana;
            font-size: 12px;
            width: 84%;
            margin-left: 8%;
            margin-right: 8%;
        }
        .collapsibleContainer
        {
            font-family: Verdana;
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
            background: #004fa3;
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
            color: #000000;
            vertical-align: bottom;
        }
        .tdstyle1
        {
            
            text-align: left;
            font-size: 11px;
            width: 35%;
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
            width: 65%;
            font-size: 11px;
            vertical-align: middle;
            padding-bottom: 15px:;
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
            font-family: Verdana;
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
            font: bold 11px auto "Trebuchet MS" , Verdana;
            font-size: 12px;
            cursor: pointer;
            width: 450px;
            height: 18px;
            padding: 4px;
        }
        .cpBody
        {
            background-color: #DCE4F9;
            font: normal 11px auto Verdana;
            border: 1px gray;
            width: 450px;
            padding: 4px;
            padding-top: 7px;
        }
        
        .blackfnt
        {
            font: 11px;
            font-family: Tahoma;
            padding-left: 5px;
            font-style: bold;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table width="736px" border="0" cellspacing="0" cellpadding="0" align="center" >
            <tr>
                <td>
                    <table width="100%" border="0" cellspacing="0" cellpadding="0" align="center">                       
                        <tr>
                            <td>
                                <table id="ContentTable" cellspacing="0px" width="100%">
                                    <tr>
                                        <td colspan="2" style="height: 16px;">
                                        </td>
                                    </tr>
                                    <tr style="display: none">
                                        <td class="tdstyle1">
                                            <span class="blackfnt" id="lblScheme"><b>Scheme Type</b></span>
                                        </td>
                                        <td class="tdstyle2">
                                            <asp:DropDownList ID="ddlNature" runat="server" AutoPostBack="true" DataTextField="Nature"
                                                DataValueField="Nature" CssClass="ddlstyle" OnSelectedIndexChanged="ddlNature_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr style="display: none">
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
                                    <tr style="display: none">
                                        <td colspan="2" align="left" style="padding-left: 25px; padding-top: 10px; padding-bottom: 10px;">
                                            <span style="color: #265599; font-family: Verdana; font-weight: bold; font-size: 10px">
                                                ------------------------------------------------------OR-------------------------------------------------------</span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdstyle1">
                                            <span id="Span1" class="blackfnt"><b>Fund Manager</b></span>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlFunManager" runat="server" AutoPostBack="True" CssClass="ddlstyle"
                                                OnSelectedIndexChanged="ddlFunManager_SelectedIndexChanged">
                                            </asp:DropDownList>
                                            <%--onChange ="javascript: SelectAll(this);"--%>
                                        </td>
                                    </tr>
                                    <tr style="display: none">
                                        <td colspan="2" style="height: 16px; vertical-align: middle;">
                                            <div style="border-bottom: #265599 solid 1px;">
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdstyle1">
                                            <span id="Label28" class="blackfnt"><b>Scheme Name</b></span>
                                        </td>
                                        <td class="tdstyleSch">
                                            <div class="border_drop">
                                                <asp:DataGrid class="title" ID="dgSchemeList" runat="server" AutoGenerateColumns="false"
                                                    HeaderStyle-Font-Bold="true" Width="100%" Font-Size="11px" Font-Names="Arial"
                                                    BorderColor="transparent" BorderWidth="1" OnItemDataBound="dgItemdatabound">
                                                    <columns>
                                                        <asp:TemplateColumn HeaderText="Name of the Scheme">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblSchemeID" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container.DataItem, "Scheme_Id")%>'></asp:Label>
                                                                <asp:Label ID="lblSchemeName" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "Sch_Short_Name")%>'></asp:Label>
                                                            </ItemTemplate>
                                                            <HeaderStyle BackColor="#004fa3" ForeColor="White" BorderColor="Black" Width="80%">
                                                            </HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Left" Font-Size="11px" Font-Names="Arial" Width="80%">
                                                            </ItemStyle>
                                                        </asp:TemplateColumn>
                                                        <asp:TemplateColumn HeaderText="Chart">
                                                            <HeaderTemplate>
                                                                <%--<input type="checkbox" id="idSelectAllCheckBox" runat="server" name="SelectAllCheckBox" onclick="SelectAll(this)">--%>
                                                                <asp:CheckBox ID="idSelectAllCheckBox" runat="server" name="SelectAllCheckBox" onclick="SelectAll(this)" />
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="chkItem" runat="server"></asp:CheckBox>
                                                            </ItemTemplate>
                                                            <HeaderStyle BackColor="#004fa3" ForeColor="White" BorderColor="Black" Width="5%">
                                                            </HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Left" Width="5%"></ItemStyle>
                                                        </asp:TemplateColumn>
                                                    </columns>
                                                    <headerstyle font-bold="True"></headerstyle>
                                                </asp:DataGrid>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr style="display: none">
                                        <td class="tdstyle1">
                                            <span id="Span5" class="blackfnt"><b>Value of Investment( Rs.)</b></span>
                                        </td>
                                        <td style="vertical-align: middle">
                                            <asp:TextBox ID="txtInvestment" runat="server" CssClass="txtboxstyle" Text="10000">
                                            </asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <br />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdstyle1">
                                            <span id="Span4" class="blackfnt"><b>Value as of Date</b></span>
                                        </td>
                                        <td style="vertical-align: top">
                                            <asp:TextBox ID="txtStartDate" runat="server" CssClass="txtboxstyle" Width="178px">
                                            </asp:TextBox>
                                            <asp:Label Style="vertical-align: middle; color: Red; font-weight: bold; margin-left: 20px;"
                                                ID="lblMessage" runat="server" Text=""></asp:Label>
                                        </td>
                                    </tr>
                                    <tr id="trset" runat="server" visible="false">
                                        <td class="tdstyle1">
                                            <span id="Span6" class="blackfnt"><b>Dividend Type</b></span>
                                        </td>
                                        <td align="left" style="font-size: 12px;">
                                            <asp:RadioButtonList runat="server" Id="radioList" CssClass="blackfnt" RepeatDirection="Horizontal"
                                                Width="263px">
                                                <asp:ListItem Selected="True" Value="Individual & HUF">
                                                </asp:ListItem>
                                                <asp:ListItem Value="Corporate">
                                                </asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                        <td style="padding-bottom: 10px; padding-top: 10px;">
                                            <asp:Button ID="btnLumpCal" runat="server" CssClass="button" Text="Calculate Return"
                                                OnClientClick="return date_validate();" OnClick="btnLumpCal_Click" />
                                            <asp:Button ID="Resetbtn" runat="server" Text="Reset" OnClick="btnResetForm" title="Reset"
                                                CssClass="button" />
                                            <asp:Button ID="lnkbtnDownload" runat="server" CssClass="button" Text="Download Excel"
                                                OnClick="ExportExcelClick" Visible="false"></asp:Button>
                                            <%-- <img src="../Images/excel_icon.png" style="border: 0; margin-left:150px " alt="" width="25" height="25"  title="download excel"/>--%>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
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
                                        <td align='left' colspan='6' class="heading_nw" height='22' width='500'>
                                            <span class='style12'>&nbsp;&nbsp;<span class='style4'>
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
                                <asp:Label ID="LiteralDisclaimer" runat="server" CssClass="disclaimercss"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                                <td>
                                    <div id="disclaimerDiv" runat="server" class="rslt_text1" >
                                        • <b>Past performance may or may not be sustained in future.</b> • Absolute returns is
                                        computed on investment is of Rs 10,000. • All payouts during the period have been reinvested
                                        in the units of the scheme at the then prevailing NAV. • Load is not considered
                                        for computation of returns. While calculating returns dividend distribution tax
                                        is excluded. • In case, the start/end date of the concerned period is non-business
                                        date, the benchmark value of the previous date is considered for computation of
                                        returns. • “N/A” - Not Available. • Schemes in existence for > 1 year performance
                                        provided for as many 12 months period as possible.• The Calculators provided on the website is for information purposes
                                        only and is not an offer to sell or a solicitation to buy any mutual fund units/securities.
                                        The recipient of this information should rely on their investigations and take professional
                                        advice before making any investment. • The Calculators alone are not sufficient
                                        and shouldn’t be used for the development or implementation of an investment strategy.
                                        It should not be construed as investment advice to any party. • Neither Top3choice, nor any person connected with it, accepts any liability arising
                                        from the use of this information. • In view of individual nature of tax consequences,
                                        each investor is advised to consult his/ her own professional tax advisor. • <b>Mutual
                                        Fund Investments are subject to market risks, read all Scheme related documents
                                        carefully.</b>                                        
                                    </div>
                                    
                                </td>
                        </tr>
                        <tr>
                                <td style="text-align: right">                                    
                                        <span style="text-align: right" class="rslt_text1">Developed by:<a style="text-decoration:none;" href="https://www.icraanalytics.com"
                                                                                                                target="_blank"> ICRA Analytics Ltd </a></span>  
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
