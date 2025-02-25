<%@ Page Language="C#" AutoEventWireup="true" ViewStateEncryptionMode="Always" EnableViewStateMac="true"
    CodeBehind="sipCalc_old.aspx.cs" Inherits="iFrames.Edelweiss.SipCalc_old" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>SIP Calculator</title>
    <link href="CSS/edelstyles.css" rel="stylesheet" type="text/css" media="all" />
    <link href="CSS/tabcontent.css" rel="stylesheet" type="text/css" media="all" />
    <link href="css/jquery-ui.css" rel="stylesheet" type="text/css" />
    <script src="Resource/EdelweissChecks.js" type="text/javascript"></script>
    <script src="Resource/jquery1.js" type="text/javascript"></script>
    <script src="Resource/jquery.ui.core.js" type="text/javascript"></script>
    <script src="Resource/jquery.ui.datepicker.js" type="text/javascript"></script>
    <script src="Resource/tabcontent.js" type="text/javascript"></script>
    
    <script type="text/javascript" language="javascript">

        $(function () {
            var values = eval('<%=Values%>');
            var values1 = eval('<%=SelectedValues%>');
            if (values == null) {
                addOnLoadHtml();
            }

            if (values != null) {
                var html = "";
                var i = 0;
                $(values).each(function () {
                    var div = $("<div />");
                    var p = GettxtDynSwitchAmt(this);
                    var selValues = String(values1).split(",");
                    p = p.replace('<option value="' + selValues[i] + '">', '<option value="' + selValues[i] + '" selected="selected">');
                    div.html(p);
                    $("#TextBoxContainer").append(div);
                    i++;

                });
            }
            $("#btnAdd").bind("click", function () {
                //addButtonClick();
            });
            $("body").on("click", ".remove", function () {
                $(this).closest("div").remove();
                var count = $('input[name*="txtDynSwitchAmt"]').length;
                if (count < 1) { addOnLoadHtml(); }

            });

            $("body").on("click", ".add", function () {
                var count = $('input[name*="txtDynSwitchAmt"]').length;

                if (count == 1) {
                    var Element = document.getElementById("btnRemove");
                    if (Element != null)
                    { Element.style.visibility = 'visible'; }
                }
                if (count < 3)
                { addButtonClick(); }
            });

            $("#txtfrdt").datepicker({
                dateFormat: 'dd/mm/yy',
                changeMonth: true,
                changeYear: true,
                maxDate: -1,
                minDate: mdate()
            });

            function mdate() {
              
                var ddScheme = document.getElementById("ddlFromScheme");

                if (ddScheme === null) {
                    var ddToScheme = document.getElementById("ddlToScheme");
                    var Process = document.getElementById("ddlSIPType");
                    if (ddToScheme === null)
                    { return; }
                    if (Process.value == 'Prepaid SIP') {
                        var allotdate = ddToScheme.options[ddToScheme.selectedIndex].title;
                        var Day = parseInt(allotdate.substring(0, 2), 10);
                        var Mn = parseInt(allotdate.substring(3, 5), 10);
                        var Yr = parseInt(allotdate.substring(6, 10), 10);
                        var DateVal = Mn + "/" + Day + "/" + Yr;
                        var dt = new Date(DateVal);
                        return dt;
                    }
                    else
                    { return; }
                }
                var allotdate = ddScheme.options[ddScheme.selectedIndex].title;

                var Day = parseInt(allotdate.substring(0, 2), 10);
                var Mn = parseInt(allotdate.substring(3, 5), 10);
                var Yr = parseInt(allotdate.substring(6, 10), 10);
                var DateVal = Mn + "/" + Day + "/" + Yr;
                var dt = new Date(DateVal);


                return dt;
            }
            $("#ddlFromScheme").change(function () {
                //debugger;
                $("#txtfrdt").datepicker("destroy");
                $("#txtfrdt").datepicker({
                    dateFormat: 'dd/mm/yy',
                    changeMonth: true,
                    changeYear: true,
                    maxDate: -1,
                    minDate: mdate()

                });

            });
            $("#txttodt").datepicker({                
                dateFormat: 'dd/mm/yy',
                changeMonth: true,
                changeYear: true,
                maxDate: -1,
                minDate: mdate()
                //                 maxDate: 0
            });

            $("#txtcurdt").datepicker({
                dateFormat: 'dd/mm/yy',
                changeMonth: true,
                changeYear: true,
                maxDate: -1,
                minDate: mdate()
                //                 maxDate: 0
            });
            var Process = document.getElementById("ddlSIPType");
            if (Process.value == 'Prepaid SIP') {
                $('input[name*="txtDynSwitchAmt"]').attr("placeholder", "SIP Amount");
                $('#btnRemove').hide();
                $('#btnAdd').hide();
                $('#btnAdd4Add').hide();
                $('#btnRemove4Add').hide();
                $('select[name*="dynTriggerAmt"]').css("width", "80");
                $('input[name*="txtDynSwitchAmt"]').css("width", "80");
                $('#TrNoInvestmentSIP').show();
                $('#TrTriggerGain').hide();
                $('#NoOfInvestmenttxt').css("width", "30%");
            }
            if (Process.value == 'Prepaid STP') {
                $('#btnRemove').show();
                $('#btnAdd').show();
                $('#btnAdd4Add').show();
                $('#btnRemove4Add').show();
                $('#TrNoInvestmentSIP').hide();
                $('#TrTriggerGain').hide();
            }
            if (Process.value == 'Gain Switch Mechanism') {
                $('#TrTriggerGain').show();
                $('#TrNoInvestmentSIP').hide();
            }
            //$('#NoOfInvestmenttxt').css("width","30");
        });

        function showTab(selected, total) {
            for (i = 1; i <= total; i += 1) {
                document.getElementById('country-' + i).style.display = 'none';
            }

            document.getElementById('country-' + selected).style.display = 'block';
        }

        function addOnLoadHtml() {
            var div = $("<div />");
            var p = GettxtDynSwitchAmtOnLoad("");
            div.html(p);
            $("#TextBoxContainer").append(div);
        }

        function GettxtDynSwitchAmt(value) {
            return '<select  name="dynTriggerAmt" Style="font-family: Verdana; width: 69px;"><option value="-0.5">-0.5 %</option><option value="-1.0">-1.0 %</option><option value="-2.0">-2.0 %</option></select> &nbsp;' +
                    '<input name = "txtDynSwitchAmt" placeholder="Switch Amount" Style="font-family: Verdana; width: 100px;" type="text" value = "' + value + '" />&nbsp;' +
                    '<input type="button" id="btnAdd4Add"  value="add" class="add" />&nbsp;' +
                    '<input type="button" id="btnRemove4Add"  value="Remove" class="remove" />'
        }
        function GettxtDynSwitchAmtOnLoad(value) {
            return '<select  name="dynTriggerAmt" Style="font-family: Verdana; width: 69px;"><option value="-0.5">-0.5 %</option><option value="-1.0">-1.0 %</option><option value="-2.0">-2.0 %</option></select> &nbsp;' +
                    '<input name = "txtDynSwitchAmt" placeholder="Switch Amount" Style="font-family: Verdana; width: 100px;" type="text" value = "' + value + '" />&nbsp;' +
                    '<input id="btnAdd"  type="button" value="add" class="add" />&nbsp;' +
                    '<input id="btnRemove"  type="button" value="Remove" style="visibility:hidden" class="remove" />'
        }

        function addButtonClick() {
            var div = $("<div />");
            div.html(GettxtDynSwitchAmt(""));
            $("#TextBoxContainer").append(div);
            var count = $('input[name*="txtDynSwitchAmt"]').length;
            //if (count == 3) { $("#btnAdd").prop('disabled', true); }
        }
        

    </script>
    <style type="text/css">
        .table_format
        {
            margin-left: 40px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table width="800" border="0" align="center" cellpadding="0" cellspacing="0">
            <tr>
                <td style="vertical-align: top;">
                    <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0" style="margin-bottom: 0px">
                        <tr style="height: 35px;">
                            <td class="FieldHead" style="width: 30%;">
                                Investment Mode
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlSIPType" runat="server" AutoPostBack="true" Style="font-family: Verdana;"
                                    Width="170px" OnSelectedIndexChanged="ddlSIPType_SelectedIndexChanged">
                                    <%--  <asp:ListItem>Prepaid SIP</asp:ListItem>
                                    <asp:ListItem>Gain Switch Mechanism</asp:ListItem>
                                    <asp:ListItem>Prepaid Plus</asp:ListItem>--%>
                                    <asp:ListItem>Prepaid STP</asp:ListItem>
                                    <asp:ListItem>Gain Switch Mechanism</asp:ListItem>
                                    <asp:ListItem>Prepaid SIP</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr style="height: 35px;" id="sourceSchemeselection" runat="server">
                            <td height="30" class="FieldHead">
                                Source Scheme
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlFromScheme" Style="font-family: Verdana;" runat="server"
                                    Width="500px" OnSelectedIndexChanged="ddlFromScheme_SelectedIndexChanged" AutoPostBack="True"
                                    OnPreRender="ddlFromScheme_PreRender">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr style="height: 35px;">
                            <td height="30" class="FieldHead">
                                Target Scheme
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlToScheme" Style="font-family: Verdana;" runat="server" Width="500px"
                                    OnPreRender="ddlToScheme_PreRender" AutoPostBack="True" OnSelectedIndexChanged="ddlToScheme_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr style="height: 35px;" runat="server" id="IntInvtRow">
                            <td colspan="2">
                                <table width="100%" border="0" align="left" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td height="30" class="FieldHead" style="width: 30%;">
                                            Amount Invested (Rs.)
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtInitialInvestment" onkeypress="return isNumber(event)" runat="server"
                                                Style="width: 170px"></asp:TextBox>
                                        </td>
                                        <td style="font-family: Verdana; font-size: 12px; padding-left: 1px">
                                            <asp:RangeValidator ID="rvInitialInvestment" ControlToValidate="txtInitialInvestment"
                                                MinimumValue="25000" MaximumValue="1800000" Enabled="false" Type="Integer" runat="server"
                                                ErrorMessage="&nbsp*Amount Invested Range 25,000 to 18 Lacs"></asp:RangeValidator>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr style="height: 35px;" id="TrTriggerGain" runat="server">
                            <td colspan="2">
                                <table width="100%" border="0" align="left" cellpadding="0" cellspacing="0">
                                    <tr>                                       
                                        <td class="FieldHead" style="width: 30%;" id="benchmarkChangeText" runat="server">
                                            Trigger<br />
                                            <span style="font-size: 9px;" id="changetype" runat="server">(benchmark change %)</span>
                                        </td>
                                        <td id="triggarAmt" runat="server">
                                            <asp:DropDownList ID="ddlTriggerAmt" Style="font-family: Verdana; width: 115px;"
                                                runat="server">
                                                <asp:ListItem Value="0.5">0.50%</asp:ListItem>
                                                <asp:ListItem Value="1">1%</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                         <td class="FieldHead" style="width: 30%;display:none">
                                            Benchmark Index
                                        </td>
                                        <td style="width: 28%;display:none">
                                            <%--<asp:DropDownList ID="ddlBenchmark" Style="font-family: Verdana; width: 153px;" runat="server">
                                                <asp:ListItem Value="Nifty 50" Text="">Nifty 50</asp:ListItem>                                                
                                                <asp:ListItem Value="Nifty Free Float Midcap 100" Text="">Nifty Free Float Midcap 100</asp:ListItem>
                                            </asp:DropDownList>--%>
                                            <asp:TextBox ID="txtIndex" runat="server" Style="width: 170px;" ReadOnly="true"></asp:TextBox>
                                            <asp:HiddenField ID="hdnIndexID" runat="server" />
                                            <asp:HiddenField ID="HdnIndexName" runat="server" />
                                             <asp:HiddenField ID="HdnIndexNameWithOutTRI" runat="server" />
                                            <asp:HiddenField ID="HdnAddBenchId" runat="server" />
                                            <asp:HiddenField ID="HdnAddBenchName" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr style="height: 35px;" id="TrNoInvestmentSIP" runat="server">
                            <td colspan="2">
                                <table width="100%" border="0" align="left" cellpadding="0" cellspacing="0">
                                    <tr>                                        
                                        <td class="FieldHead" id="NoOfInvestmenttxt" runat="server" style="width: 30%;">
                                            No Of Investment
                                        </td>
                                        <td style="width: 28%;" id="NoOfInvestmentId" runat="server">
                                            <asp:DropDownList ID="ddlNoOfInvestment" Style="font-family: Verdana; width: 115px;"
                                                runat="server">
                                                <asp:ListItem Value="3" Text="">3</asp:ListItem>
                                                <asp:ListItem Value="5" Text="">5</asp:ListItem>
                                                <asp:ListItem Value="7" Text="">7</asp:ListItem>
                                                <asp:ListItem Value="0" Text="">Infinite</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td class="FieldHead" style="width: 30%;display:none">
                                            Additional Benchmark Index
                                        </td>
                                        <td style="width: 28%;display:none" align="left">
                                            <asp:DropDownList ID="ddlAdditionalBenchmark" Style="font-family: Verdana; width: 175px;"
                                                runat="server">
                                                <asp:ListItem Value="Crisil Liquid Fund Index" Text="">Crisil Liquid Fund Index</asp:ListItem>
                                                <asp:ListItem Value="Crisil MIP Blended Index" Text="">Crisil MIP Blended Index</asp:ListItem>
                                                <asp:ListItem Value="Crisil Balanced Fund - Aggressive Index" Text="">Crisil Balanced Fund - Aggressive Index</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr style="height: 35px;" id="switchamountid" runat="server">
                            <td height="30" class="FieldHead">
                                <span id="switchText" runat="server">Switch Amount </span>
                            </td>
                            <td>
                                <%--<asp:DropDownList ID="ddlSwitchAmt" Style="font-family: Verdana; width: 152px; margin-left: 0.5px;"
                                    runat="server">
                                    <asp:ListItem>5</asp:ListItem>
                                    <asp:ListItem>10</asp:ListItem>
                                </asp:DropDownList>--%>
                                <asp:TextBox ID="txtSwitchAmt" runat="server" CssClass="textbox1" Style="font-family: Verdana;
                                    width: 170px;" onChange="Javascript: setFocus(); "></asp:TextBox>
                            </td>
                        </tr>
                        <tr style="height: 35px;">
                            <td colspan="2">
                                <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td class="FieldHead" style="width: 30%;">
                                            From Date
                                        </td>
                                        <td class="FieldHead" style="width: 28%;">
                                            <asp:TextBox ID="txtfrdt" runat="server" CssClass="textbox1" Style="font-family: Verdana;
                                                width: 170px;" onChange="Javascript: setFocus(); "></asp:TextBox>
                                        </td>
                                        <td class="FieldHead" style="width: 20%; margin-right: 15px;">
                                            To Date
                                        </td>
                                        <td class="FieldHead">
                                            <asp:TextBox ID="txttodt" runat="server" CssClass="textbox1" Style="font-family: Verdana;
                                                width: 115px;">
                                            </asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr style="height: 35px;">
                            <td class="FieldHead">
                                Current Date
                            </td>
                            <td class="FieldHead">
                                <asp:TextBox ID="txtcurdt" runat="server" CssClass="textbox1" Style="font-family: Verdana;
                                    width: 170px;">
                                </asp:TextBox><span style="font-size: 11px;">&nbsp;&nbsp;&nbsp;('Current Date' should
                                    be greater than 'To Date')</span>
                            </td>
                        </tr>
                        <tr style="height: 35px;" id="switchAmtSIPPlus" runat="server" >
                            <td class="FieldHead">
                                Trigger<br />
                                            <span style="font-size: 9px;" id="Span1" runat="server">(benchmark change %)</span>
                            </td>
                            <td class="FieldHead">
                                <div id="TextBoxContainer">
                                    <%-- <select name="dynTriggerAmt" style="font-family: Verdana; width: 80px;">
                                        <option value="-0.5">-0.5 %</option>
                                        <option value="-1.0">-1.0 %</option>
                                        <option value="-2.0">-2.0 %</option>
                                    </select>&nbsp;
                                    <input name="txtDynSwitchAmt" style="font-family: Verdana; width: 80px;" type="text"
                                        value="" />
                                    <input id="btnAdd" type="button" value="add" />--%>
                                    <!--Textboxes will be added here -->
                                </div>
                            </td>

                        </tr>
                        <tr style="margin-bottom: 20px;">
                            <td colspan="2">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0" style="margin-bottom: 25px;
                                    margin-left: 14px;">
                                    <tr>
                                        <td style="width: 6%;">
                                            &nbsp;
                                        </td>
                                        <td align="right">
                                            <asp:Button ID="btnCalculate" OnClientClick="return validateEdelweissInput();" runat="server"
                                                class="btn_cal" Text="Calculate" OnClick="btnCalculate_Click" />
                                        </td>
                                        <td align="left" style="width: 90px;">
                                            <input id="btnReset" class="btn_cal" type="reset" onclick="javascript:window.location.assign(window.location.href);"
                                                value="Reset" />
                                        </td>
                                        <td align="left">
                                            <asp:Button ID="btnExport" class="btn_cal1" runat="server" Text="Export To Excel"
                                                OnClick="btnExport_Click" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" align="left">
                                <asp:GridView ID="gridSummary" runat="server" AutoGenerateColumns="False" CssClass="grdHead"
                                    Visible="true" Width="100%" BorderStyle="Solid" BorderColor="#495e91" BorderWidth="1px">
                                    <Columns>
                                        <asp:TemplateField HeaderStyle-HorizontalAlign="left" HeaderText="     " HeaderStyle-BorderColor="#495e91"
                                            HeaderStyle-BorderWidth="1px" HeaderStyle-BorderStyle="Solid">
                                            <ItemTemplate>
                                                <%# (Eval("Return"))%>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="left"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="left" CssClass="gridtd1" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="ARF" ControlStyle-CssClass="grdRow"
                                            HeaderStyle-Font-Bold="false" HeaderStyle-Height="25px" HeaderStyle-BorderColor="#495e91"
                                            HeaderStyle-BorderWidth="1px" HeaderStyle-BorderStyle="Solid">
                                            <ItemTemplate>
                                                <%# (Eval("ARF"))%>
                                            </ItemTemplate>
                                            <ControlStyle></ControlStyle>
                                            <HeaderStyle HorizontalAlign="Center" CssClass="gridtd"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="EDGE" HeaderStyle-Font-Bold="false"
                                            HeaderStyle-Height="25px" HeaderStyle-BorderColor="#495e91" HeaderStyle-BorderWidth="1px"
                                            HeaderStyle-BorderStyle="Solid">
                                            <ItemTemplate>
                                                <%# (Eval("Edge"))%>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Prepaid STP"
                                            HeaderStyle-Font-Bold="false" HeaderStyle-Height="25px" HeaderStyle-BorderColor="#495e91"
                                            HeaderStyle-BorderWidth="1px" HeaderStyle-BorderStyle="Solid">
                                            <ItemTemplate>
                                                <%# (Eval("PrepaidSIP"))%>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Monthly STP >"
                                            HeaderStyle-Font-Bold="false" HeaderStyle-Height="25px" HeaderStyle-BorderColor="#495e91"
                                            HeaderStyle-BorderWidth="1px" HeaderStyle-BorderStyle="Solid">
                                            <ItemTemplate>
                                                <%# (Eval("MonthlySIP"))%>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Benchmark @"
                                            HeaderStyle-Font-Bold="false" HeaderStyle-Height="25px" HeaderStyle-BorderColor="#495e91"
                                            HeaderStyle-BorderWidth="1px" HeaderStyle-BorderStyle="Solid">
                                            <ItemTemplate>
                                                <%# (Eval("Benchmark"))%>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Additional <br /> Benchmark $"
                                            HeaderStyle-Font-Bold="false" HeaderStyle-Height="25px" HeaderStyle-BorderColor="#495e91"
                                            HeaderStyle-BorderWidth="1px" HeaderStyle-BorderStyle="Solid">
                                            <ItemTemplate>
                                                <%# (Eval("AdditionalBenchmark"))%>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                    </Columns>
                                    <RowStyle CssClass="grdRow" />
                                </asp:GridView>
                            </td>
                        </tr>
                        <tr>
                            <td height="30" class="rslt_text" colspan="2" align="left">
                                <div runat="server" id="div_text" visible="false">
                                    * Returns are ABSOLUTE for <1 yr and COMPOUND ANNUALIZED for >=1 yr
                                   <%--* Return calculated using XIRR calculation--%>
                                </div>
                                <div runat="server" id="fromschemefull" visible="false">
                                </div>
                                <div runat="server" id="toschemefull" visible="false">
                                </div>
                                <div runat="server" id="benchmarkfull" visible="false">
                                </div>
                                <div runat="server" id="addbenchmarkfull" visible="false">
                                </div>
                                <div runat="server" id="MonthlySIPid" visible="false">
                                    > STP is a systematic investment from the source scheme to the target scheme on
                                    the first of every month
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" align="center">
                                <div runat="server" id="Div_tab" visible="false">
                                    <ul id="countrytabs" class="shadetabs">
                                        <li><a onclick="showTab(1,2)" rel="country1" class="active">Detail Report</a></li>
                                        <li><a onclick="showTab(2,2)" rel="country2">View Graph</a></li>
                                    </ul>
                                </div>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <div id="country-1" class="tabcontent">
            <table width="800" border="0" align="center" cellpadding="0" cellspacing="0">
                <tr>
                    <td>
                        <table border="0" align="left" id="tblFromScheme" runat="server" cellpadding="0"
                            cellspacing="0" width="100%">
                            <tr>
                                <td>
                                    <tr>
                                        <td height="30">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td height="30" class="FieldHead1" colspan="2">
                                            From Scheme :
                                            <asp:Label ID="lblFromScheme" runat="server" Text="Label"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" align="left" class="table_format">
                                            <asp:GridView ID="gridDetailsFrom" runat="server" AutoGenerateColumns="False" CssClass="grdHead"
                                                Width="800px" Visible="true">
                                                <Columns>
                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderStyle-Font-Bold="false"
                                                        HeaderText="Date" HeaderStyle-Height="25px" HeaderStyle-BorderColor="#495e91"
                                                        HeaderStyle-BorderWidth="1px" HeaderStyle-BorderStyle="Solid">
                                                        <ItemTemplate>
                                                            <%# (Eval("Date"))%>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="NAV" ControlStyle-CssClass="grdRow"
                                                        HeaderStyle-Font-Bold="false" HeaderStyle-Height="25px" HeaderStyle-BorderColor="#495e91"
                                                        HeaderStyle-BorderWidth="1px" HeaderStyle-BorderStyle="Solid">
                                                        <ItemTemplate>
                                                            <%# (Eval("Nav"))%>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" CssClass="gridtd2" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Index Change (%)"
                                                        HeaderStyle-Font-Bold="false" HeaderStyle-Height="25px" HeaderStyle-BorderColor="#495e91"
                                                        HeaderStyle-BorderWidth="1px" HeaderStyle-BorderStyle="Solid">
                                                        <ItemTemplate>
                                                            <%# (Eval("Index_Change"))%>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" CssClass="gridtd2" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="SIP Amount (Rs.)"
                                                        HeaderStyle-Font-Bold="false" HeaderStyle-Height="25px" HeaderStyle-BorderColor="#495e91"
                                                        HeaderStyle-BorderWidth="1px" HeaderStyle-BorderStyle="Solid">
                                                        <ItemTemplate>
                                                            <%# (Eval("SIP_Amount"))%>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" CssClass="gridtd2" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Scheme Units"
                                                        HeaderStyle-Font-Bold="false" HeaderStyle-Height="25px" HeaderStyle-BorderColor="#495e91"
                                                        HeaderStyle-BorderWidth="1px" HeaderStyle-BorderStyle="Solid">
                                                        <ItemTemplate>
                                                            <%# (Eval("Scheme_Units"))%>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" CssClass="gridtd2" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Fund Value (Rs.)"
                                                        HeaderStyle-Font-Bold="false" HeaderStyle-Height="25px" HeaderStyle-BorderColor="#495e91"
                                                        HeaderStyle-BorderWidth="1px" HeaderStyle-BorderStyle="Solid">
                                                        <ItemTemplate>
                                                            <%# (Eval("Cumulative_Fund_Value"))%>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" CssClass="gridtd2" />
                                                    </asp:TemplateField>
                                                </Columns>
                                                <RowStyle CssClass="grdRow" />
                                            </asp:GridView>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            &nbsp;
                                        </td>
                                    </tr>
                                </td>
                            </tr>
                        </table>
                        <table border="0" align="left" cellpadding="0" cellspacing="0" width="100%">
                            <tr>
                                <td colspan="2">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td height="30" class="FieldHead1" colspan="2">
                                    To Scheme :
                                    <asp:Label ID="lblToScheme" runat="server" Text="Label"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" align="left" class="table_format">
                                    <asp:GridView ID="gridDetailsTo" runat="server" AutoGenerateColumns="False" CssClass="grdHead"
                                        Width="800px" Visible="true">
                                        <Columns>
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Date" HeaderStyle-Font-Bold="false"
                                                HeaderStyle-Height="25px" HeaderStyle-BorderColor="#495e91" HeaderStyle-BorderWidth="1px"
                                                HeaderStyle-BorderStyle="Solid">
                                                <ItemTemplate>
                                                    <%# (Eval("Date"))%>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" CssClass="gridtd2" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="NAV" ControlStyle-CssClass="grdRow"
                                                HeaderStyle-Font-Bold="false" HeaderStyle-Height="25px" HeaderStyle-BorderColor="#495e91"
                                                HeaderStyle-BorderWidth="1px" HeaderStyle-BorderStyle="Solid">
                                                <ItemTemplate>
                                                    <%# (Eval("Nav"))%>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" CssClass="gridtd2" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Index Change (%)"
                                                HeaderStyle-Font-Bold="false" HeaderStyle-Height="25px" HeaderStyle-BorderColor="#495e91"
                                                HeaderStyle-BorderWidth="1px" HeaderStyle-BorderStyle="Solid">
                                                <ItemTemplate>
                                                    <%# (Eval("Index_Change"))%>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" CssClass="gridtd2" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="SIP Amount (Rs.)"
                                                HeaderStyle-Font-Bold="false" HeaderStyle-Height="25px" HeaderStyle-BorderColor="#495e91"
                                                HeaderStyle-BorderWidth="1px" HeaderStyle-BorderStyle="Solid">
                                                <ItemTemplate>
                                                    <%# (Eval("SIP_Amount"))%>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" CssClass="gridtd2" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Scheme Units"
                                                HeaderStyle-Font-Bold="false" HeaderStyle-Height="25px" HeaderStyle-BorderColor="#495e91"
                                                HeaderStyle-BorderWidth="1px" HeaderStyle-BorderStyle="Solid">
                                                <ItemTemplate>
                                                    <%# (Eval("Scheme_Units"))%>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" CssClass="gridtd2" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Fund Value (Rs.)"
                                                HeaderStyle-Font-Bold="false" HeaderStyle-Height="25px" HeaderStyle-BorderColor="#495e91"
                                                HeaderStyle-BorderWidth="1px" HeaderStyle-BorderStyle="Solid">
                                                <ItemTemplate>
                                                    <%# (Eval("Cumulative_Fund_Value"))%>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" CssClass="gridtd2" />
                                            </asp:TemplateField>
                                        </Columns>
                                        <RowStyle CssClass="grdRow" />
                                    </asp:GridView>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    &nbsp;
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </div>
        <div id="country-2" class="tabcontent">
            <table width="800" border="0" align="center" cellpadding="0" cellspacing="0">
                <tr>
                    <td height="30">
                        &nbsp;
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td height="30" class="FieldHead1" id="charid" runat="server">
                        Prepaid SIP Performance Chart
                    </td>
                    <td>
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td colspan="4" align="left">
                        <asp:Chart ID="chartSIP" Visible="false" runat="server" Width="685px" BorderlineColor="128, 128, 255">
                            <Series>
                                <asp:Series Name="Series1" ChartType="Line" YValuesPerPoint="6">
                                </asp:Series>
                            </Series>
                            <ChartAreas>
                                <asp:ChartArea Name="ChartArea1">
                                    <AxisY>
                                        <MajorGrid LineWidth="0" />
                                    </AxisY>
                                    <AxisX>
                                        <MajorGrid LineWidth="0" />
                                    </AxisX>
                                </asp:ChartArea>
                            </ChartAreas>
                        </asp:Chart>
                        <asp:Image ID="ImgChrt" runat="server" Width="685px" BorderlineColor="128, 128, 255" />
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <table width="800" border="0" align="center" cellpadding="0" cellspacing="0">
        <tr>
            <td>
                <div id="disclaimerDiv" runat="server">
                    <table width="100%">
                        <tr align="left">
                            <td valign="top" class="rslt_text1">
                            </td>
                        </tr>
                        <tr align="left">
                            <td valign="top" class="rslt_text1">
                                <div align="justify">
                                    <asp:Label ID="lblDisclaimer" runat="server" Text=""></asp:Label>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                        </tr>
                        <tr align="left">
                            <td align="left" valign="top" class="rslt_text1">
                                <div align="justify">
                                    <b><span style="text-decoration: underline;">Disclaimer:</span></b>
                                    <br />
                                    <br />
                                    Past Performance may or may not be sustained in future. The calculator is meant
                                    only for illustration purpose and should not be construed as an investment advice.
                                    The return calculator has been developed and is maintained by ICRA Analytics Limited.
                                    Edelweiss Asset Management Limited (EAML) / ICRA Analytics Ltd. do not endorse the
                                    suitability, authenticity and/or accuracy of the information, figures based on which
                                    the returns are calculated; nor shall they be held responsible or liable for any
                                    error or inaccuracy or for any losses suffered by any investor as a direct or indirect
                                    consequence of relying upon the data displayed by the calculator.
                                    <br />
                                    <br />
                                    In the preparation of the calculator, information that is publicly available and
                                    information developed in-house has been used. It is designed to assist you in determining
                                    the appropriate amount of prospective investments. This calculator alone is not
                                    sufficient and shouldn’t be used for the development or implementation of any investment
                                    strategy. The various taxes, fees, load, expenses and /or any charges are not considered
                                    in the calculation provided by the calculator. SIP does not assure a profit or guarantee
                                    protection against loss in a declining market. EAML does not take the responsibility
                                    / liability nor does it undertake the authenticity of the figures calculated therein.
                                    EAML makes no warranty about the accuracy of the calculators/reckoners. ICRA Analytics
                                    Ltd. is not involved nor has any opinion on any recommendation(s) made on this site
                                    and hereby disclaims all warranties and conditions with regard to this information,
                                    recommendation, including all implied warranties and conditions of merchantability,
                                    fitness for a particular purpose, title and non-infringement. You assume the sole
                                    risk of making use and/or relying on the materials made available on this site.
                                    The examples do not claim to represent the performance of any security or investments.
                                    In view of individual nature of tax consequences, each investor is advised to consult
                                    his/ her own professional tax advisor before making any investment decisions on
                                    the basis of the results provided through the use of this calculator.
                                    <br />
                                    <br />
                                    <span style="font-size: 12px; font-weight: bold">Mutual Fund investments are subject
                                        to market risks, read all scheme related documents carefully.</span>
                                    <br />
                                    <br />
                                </div>
                                <table id="Table2" border="0" cellspacing="0" cellpadding="0" width="100%" align="left">
                                    <tr>
                                        <td class="text" align="right">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="text" align="right">
                                            <span style="text-align: right" class="rslt_text1">Developed by:<a class="text" href="https://www.icraanalytics.com"
                                                target="_blank"> ICRA Analytics Ltd </a>, <a href="https://icraanalytics.com/home/Disclaimer"
                                                                                                            target="_blank">Disclaimer </a></span>
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
    </form>
</body>
</html>
