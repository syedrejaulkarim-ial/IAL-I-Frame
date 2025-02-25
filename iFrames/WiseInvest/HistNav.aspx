<%@ Page Title="" Language="C#" MasterPageFile="~/WiseInvest/WiseMain.Master" AutoEventWireup="true" ViewStateEncryptionMode="Always" EnableViewStateMac="true"
    CodeBehind="HistNav.aspx.cs" Inherits="iFrames.WiseInvest.HistNav" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBoby" runat="server">
    <link href="css/jquery-ui.css" rel="stylesheet" type="text/css" />
    <link href="css/style.css" rel="stylesheet" type="text/css" />
    <script src="js/jquery-1.js" type="text/javascript"></script>
    <script src="js/jquery-ui.js" type="text/javascript"></script>
    <script type="text/javascript" src="js/date.js"></script>
    <script type="text/javascript" language="javascript">

        function ValidateControl() {

            var selectedFund = $('#<%=ddlFundHouse.ClientID %>').find(':selected').val();
            if (selectedFund == 0) {
                alert('Please select any Fund House.');
                $('#<%=ddlFundHouse.ClientID %>').focus();
                return false;
            }

            var selectedValue = $('#<%=listboxSchemeName.ClientID%> option:selected').val();
            if (selectedValue == null) {
                alert('Please select any Scheme.');
                $('#<%=listboxSchemeName.ClientID %>').focus();
                return false;
            }


            var regex = new RegExp("^[0-9]{2} (Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec) [0-9]{4}$", "i");

            if ($('#<%=txtfromDate.ClientID %>').val() == '') {
                alert('Please enter From date.');
                $('#<%=txtfromDate.ClientID %>').focus();
                return false;
            }

            if ($('#<%=txtToDate.ClientID %>').val() == '') {
                alert('Please enter To date.');
                $('#<%=txtToDate.ClientID %>').focus();
                return false;
            }

            if (!$('#<%=txtfromDate.ClientID %>').val().match(regex)) {
                alert('Please provide the date in dd MMM yyyy format, e.g. 12 Jan 2012');
                $('#<%=txtfromDate.ClientID %>').focus();
                return false;
            }
            if (Date.parse($('#<%=txtfromDate.ClientID %>').val(), "dd MMM yyyy") == null) {
                alert('Please provide the date in dd MMM yyyy format, e.g. 12 Jan 2012');
                $('#<%=txtfromDate.ClientID %>').focus();
                return false;
            }

            if (!dtvalid(Date.parse($('#<%=txtfromDate.ClientID %>').val(), "dd MMM yyyy").toString("dd/MM/yyyy"))) {
                alert('Please provide the date in dd MMM yyyy format, e.g. 12 Jan 2012');
                $('#<%=txtfromDate.ClientID %>').focus();
                return false;
            }

            if (!$('#<%=txtToDate.ClientID %>').val().match(regex)) {
                alert('Please provide the date in dd MMM yyyy format, e.g. 12 Jan 2012');
                $('#<%=txtToDate.ClientID %>').focus();
                return false;
            }
            if (Date.parse($('#<%=txtToDate.ClientID %>').val(), "dd MMM yyyy") == null) {
                alert('Please provide the date in dd MMM yyyy format, e.g. 12 Jan 2012');
                $('#<%=txtToDate.ClientID %>').focus();
                return false;
            }

            if (!dtvalid(Date.parse($('#<%=txtToDate.ClientID %>').val(), "dd MMM yyyy").toString("dd/MM/yyyy"))) {
                alert('Please provide the date in dd MMM yyyy format, e.g. 12 Jan 2012');
                $('#<%=txtToDate.ClientID %>').focus();
                return false;
            }

            var frmdate = converterdate($('#<%=txtfromDate.ClientID %>').val());
            var todate = converterdate($('#<%=txtToDate.ClientID %>').val());


            if (!IsValidDate(frmdate, todate)) {
                alert("From Date should be Less than To Date ");
                $('#<%=txtToDate.ClientID %>').focus();
                return false;
            }


            if (!LessThanToday(frmdate)) {
                alert("From Date should be Less than Today ");
                $('#<%=txtfromDate.ClientID %>').focus();
                return false;
            }

            if (!LessThanToday(todate)) {
                alert("To Date should be Less than Today ");
                $('#<%=txtToDate.ClientID %>').focus();
                return false;
            }





            return true;
        }


        $(function () {

            $('#<%=txtfromDate.ClientID %>').datepicker({
                showOn: "button",
                buttonImageOnly: true,
                buttonImage: "img/calenderb.jpg",
                //                buttonText: "Select Date",
                dateFormat: 'dd M yy',
                changeMonth: true,
                changeYear: true,
                setDate: "-2 d",
                maxDate: -2
            });

            $('#<%=txtToDate.ClientID %>').datepicker({
                showOn: "button",
                buttonImage: "img/calenderb.jpg",
                buttonImageOnly: true,
                //                buttonText: "Select Date",
                dateFormat: 'dd M yy',
                changeMonth: true,
                changeYear: true,
                setDate: '-1d',
                maxDate: -1
            });

            $('#btnReset').click(function (ev) {
                window.location.reload(true);
            });

            setDates();

            //            $('#<%=txtfromDate.ClientID %>').val(Date.parse('today').add(-8).days().toString("dd MMM yyyy"));
            //            $('#<%=txtToDate.ClientID %>').val(Date.parse('today').add(-2).days().toString("dd MMM yyyy"));
        });

        function setDates() {       
            var dateOffset = (24 * 60 * 60 * 1000);
            var myDate = new Date();         
            var frmdate = new Date( myDate.getTime() - dateOffset * 8);
            var todate = new Date(myDate.getTime() - dateOffset * 2);

            $('#<%=txtfromDate.ClientID %>').datepicker().datepicker('setDate', frmdate);
            $('#<%=txtToDate.ClientID %>').datepicker().datepicker('setDate', todate);           
        }     
    </script>
    <script type="text/javascript">
        document.getElementById('lHistNav').setAttribute('class', 'selected');
    </script>
    <form runat="server" id="form1">
    <table width="710px" border="0" align="left" cellpadding="0" cellspacing="0">
        <tr>
            <td>
                <table>
                    <tr>
                        <td class="top_icon">
                            <img src="img/history_nav.png" width="31" height="32" />
                        </td>
                        <td class="top_title">
                            Historical Nav
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <table width="100%" border="0" align="left" cellpadding="0" cellspacing="0">
                    <tr>
                        <td colspan="2" class="top_line">
                            <table width="75%" border="0" align="left" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td class="top_inputa">
                                        Fund House
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlFundHouse" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlFundHouse_SelectedIndexChanged"
                                            CssClass="top_input_nav">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr class="top_td">
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td class="top_inputa">
                                        Scheme Name
                                    </td>
                                    <td>
                                        <%-- <asp:ListBox ID="listboxSchemeName" runat="server" SelectionMode="Single" OnSelectedIndexChanged="listboxSchemeName_SelectedIndexChanged"
                                            CssClass="top_input3"></asp:ListBox>--%>
                                        <asp:DropDownList ID="listboxSchemeName" runat="server" OnSelectedIndexChanged="listboxSchemeName_SelectedIndexChanged"
                                            CssClass="top_input_nav">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr class="top_td">
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
                                    <td colspan="2">
                                        <table width="100%" border="0" align="left" cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td class="top_inputb">
                                                    From Date
                                                </td>
                                                <td style="width: 127px;">
                                                    <asp:TextBox ID="txtfromDate" runat="server" CssClass="top_txtbox"></asp:TextBox>
                                                </td>
                                                <td class="top_inputb">
                                                    To Date
                                                </td>
                                                <td style="width: 127px;">
                                                    <asp:TextBox ID="txtToDate" runat="server" CssClass="top_txtbox"></asp:TextBox>
                                                </td>
                                                <td class="ui-datepicker-trigger">
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnSubmit" runat="server" Text=">> Submit" OnClientClick="Javascript:return ValidateControl();"
                                                        OnClick="btnSubmit_Click" CssClass="top_button" />
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
                <table width="100%" border="0" align="left" cellpadding="0" cellspacing="0">
                    <tr>
                        <td class="top_text">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:GridView runat="server" ID="gvNavHistDetail" Width="95%" AutoGenerateColumns="false"
                                CssClass="top_table">
                                <Columns>
                                    <asp:TemplateField HeaderText="Scheme Name" HeaderStyle-CssClass="top_tableheader">
                                        <ItemTemplate>
                                            <%#              
                (Eval("Sch_Short_Name") == DBNull.Value) ? "--" : Eval("Sch_Short_Name").ToString()
                                            %>
                                        </ItemTemplate>
                                        <ItemStyle CssClass="top_tablerow" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Nav Date" HeaderStyle-CssClass="top_tableheader">
                                        <ItemTemplate>
                                            <%# Convert.ToDateTime(Eval("Nav_Date")).ToString("dd MMM yyyy")%>
                                        </ItemTemplate>
                                        <ItemStyle CssClass="top_tablerow" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Nav" HeaderStyle-CssClass="top_tableheader">
                                        <ItemTemplate>
                                            <%# Eval("Nav").ToString()  %>
                                        </ItemTemplate>
                                        <ItemStyle CssClass="top_tablerow" />
                                    </asp:TemplateField>
                                </Columns>
                                <EmptyDataTemplate>
                                Data Not Found
                                </EmptyDataTemplate>
                            </asp:GridView>
                        </td>
                    </tr>
                     <%--<tr>
            <td class="disclaimerh">
                Disclaimer
            </td>
        </tr>
        <tr>
            <td class="disclaimer">
                All Mutual Funds and securities investments are subject to market risks and there
                can be no assurance that the scheme’s object will be achieved and the NAV of the
                schemes can go up or down depending upon the factors and forces affecting the securities
                market. Past performance of the schemes do not indicate the future performances.
                The NAV of the schemes may be affected by changes in Interest Rate, trading volumes,
                settlement periods, transfer procedures and performances of individual securities.
                The NAV will be exposed to price/ Interest rate Risk and Credit Risk.
            </td>
        </tr>--%>
                </table>
            </td>
        </tr>
    </table>
    </form>
</asp:Content>
