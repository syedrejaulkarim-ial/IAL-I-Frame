<%@ Page Language="C#" AutoEventWireup="true" ViewStateEncryptionMode="Always" EnableViewStateMac="true" CodeBehind="NavGraph.aspx.cs" Inherits="iFrames.Chart.NavGraph" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link type="text/css" rel="Stylesheet" href="css/NavGraph.css" />
    <link href="../Styles/jquery-ui.css"
        rel="stylesheet" type="text/css" />
    <script type="text/javascript" language="javascript" src="../Scripts/jquery-1.6.2.js"></script>
    <script type="text/javascript" language="javascript" src="../Scripts/jquery-ui-1.8.14.custom.min.js"></script>
    <script type="text/javascript" language="javascript">
        $(function() {
            $("#datepickerStart").datepicker({
                changeMonth: true,
                changeYear: true
            });
            $("#datepickerEnd").datepicker({
                changeMonth: true,
                changeYear: true,
               
            });
        });

    </script>
    <script type="text/javascript" type="text/javascript" language="javascript">
        function GetDate() {
            var x = $('#datepickerStart').val();
            $('#<%= hdnStartDate.ClientID %>').val(x);
            var y = $('#datepickerEnd').val();
            $('#<%= hdnEndDate.ClientID %>').val(y);


            var rblCaseControl1 = $('input[type=radio][id*=rbTime]:checked').val();
            var rblCaseControl2 = $('input[type=radio][id*=rbDateRange]:checked').val();
            if (rblCaseControl2 == 'rbDateRange') {
                if ($('#<%= hdnStartDate.ClientID %>').val() == '' || $('#<%= hdnEndDate.ClientID %>').val() == '') {
                    $('#<%= lblDateRangeMsg.ClientID %>').html("Please provide date range.");

                    return false;
                }
                else {
                    return true;
                }
            }
            else {
                return true;
            }

        }
    </script>
    <script type="text/javascript" language="javascript">
        $(document).ready(function () {
            loaded();
        });
        function loaded() {
            if ($('#<%= hdnStartDate.ClientID %>').val() != '') {
                var a = $('#<%= hdnStartDate.ClientID %>').val();
                $('#datepickerStart').val(a);
            }

            if ($('#<%= hdnEndDate.ClientID %>').val() != '') {
                var b = $('#<%= hdnEndDate.ClientID %>').val();
                $('#datepickerEnd').val(b);
            }
        }
    </script>
    <script type="text/javascript" language="javascript">
        function SelectAll(CheckBoxControl) {

            if (CheckBoxControl.checked == true) {

                var i;
                for (i = 0; i < document.forms[0].elements.length; i++) {
                    //alert(document.forms[0].elements[i].type);
                    //alert(document.forms[0].elements[i].name.indexOf('dgdept_details'));
                    if ((document.forms[0].elements[i].type == 'checkbox') && (document.forms[0].elements[i].name.indexOf('dglist') > -1)) {

                        document.forms[0].elements[i].checked = true;
                    }
                }
            }
            else {
                var i;
                for (i = 0; i < document.forms[0].elements.length; i++) {
                    if ((document.forms[0].elements[i].type == 'checkbox') && (document.forms[0].elements[i].name.indexOf('dglist') > -1)) {
                        document.forms[0].elements[i].checked = false;
                    }
                }
            }
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <center>
    </center>
    <table id="tblmain" class="percentage100" border="0" cellpadding="0" cellspacing="0">
        <tr>
            <td>
                <div>
                    <table class="percentage50" border="0" cellpadding="0" cellspacing="0">
                        <tr>
                            <td>
                                <span class="header">NAV GRAPH</span>
                                <hr />
                            </td>
                        </tr>
                    </table>
                </div>
            </td>
        </tr>
        <tr>
            <td class="hight10" colspan="2">
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <div id="dvControls">
                    <table id="tblControls" class="percentage100" border="0" cellpadding="0" cellspacing="4">
                        <tr>
                            <td class="title" style="padding-left: 4px;">
                                Select Mutual Fund:
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlMutualFund" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlMutualFund_SelectedIndexChanged">
                                </asp:DropDownList>
                                <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                                    ErrorMessage="Please Select Mutual Fund." ControlToValidate="ddlMutualFund" 
                                    Display="Dynamic" InitialValue="0" ValidationGroup="chart"></asp:RequiredFieldValidator>--%>
                            </td>
                        </tr>
                        <tr>
                            <td class="title" style="padding-left: 4px;">
                                Select Scheme:
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlSchemes" runat="server">
                                    <asp:ListItem Value="0" Selected="True">-Select Scheme-</asp:ListItem>
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Please Select Scheme."
                                    ControlToValidate="ddlSchemes" Display="Dynamic" InitialValue="0" ValidationGroup="scheme"></asp:RequiredFieldValidator>
                                <asp:Button ID="btnAddScheme" runat="server" Text="Add" CssClass="button" OnClick="btnAddScheme_Click"
                                    ValidationGroup="scheme" />
                            </td>
                        </tr>
                        <tr>
                            <td class="title" style="padding-left: 4px;">
                                Select Index:
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlIndices" runat="server">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Please Select Indices."
                                    ControlToValidate="ddlIndices" Display="Dynamic" InitialValue="0" ValidationGroup="indices"></asp:RequiredFieldValidator>
                                <asp:Button ID="btnAddIndices" runat="server" Text="Add" CssClass="button" OnClick="btnAddIndices_Click"
                                    ValidationGroup="indices" />
                            </td>
                        </tr>
                    </table>
                </div>
            </td>
        </tr>
        <tr>
            <td class="hight10" colspan="2">
            </td>
        </tr>
        <tr>
            <td colspan="2" style="padding-left: 2px;">
                <div>
                    <table class="percentage100" border="0" cellpadding="0" cellspacing="0">
                        <tr>
                            <td class="percentage100" align="left" style="padding-left: 2px;">
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                    <ContentTemplate>
                                        <asp:DataGrid class="title" ID="dglist" runat="server" AutoGenerateColumns="false"
                                            HeaderStyle-Font-Bold="true" Width="50%" BorderColor="#4879AC" BorderWidth="1"
                                            OnItemCommand="dglist_ItemCommand">
                                            <Columns>
                                                <asp:TemplateColumn HeaderText="Name">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblSchemeCode" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container.DataItem, "SchemeCode")%>'></asp:Label>
                                                        <asp:Label ID="lblIndCode" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container.DataItem, "IndCode")%>'></asp:Label>
                                                        <asp:Label ID="lblSchemeName" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "SchemeName")%>'></asp:Label>
                                                        <asp:Label ID="lblIndName" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "IndName")%>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle BackColor="#37689a" ForeColor="White" Width="80%"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Left" Width="80%"></ItemStyle>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="Chart">
                                                    <HeaderTemplate>
                                                        <input type="checkbox" name="SelectAllCheckBox" onclick="SelectAll(this)">
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkItem" runat="server"></asp:CheckBox>
                                                    </ItemTemplate>
                                                    <HeaderStyle BackColor="#37689a" ForeColor="White" Width="5%"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Left" Width="5%"></ItemStyle>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="Auto ID" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblAutoID" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "AutoID")%>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle BackColor="#37689a" ForeColor="White" Width="5%"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Left" Width="5%"></ItemStyle>
                                                </asp:TemplateColumn>
                                                <%--<asp:TemplateColumn HeaderText="Scheme Code" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblSchemeCode" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "SchemeCode")%>'></asp:Label>
                                                        <asp:Label ID="lblIndCode" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "IndCode")%>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle BackColor="#37689a" ForeColor="White" Width="5%"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Left" Width="5%"></ItemStyle>
                                                </asp:TemplateColumn>--%>
                                                <asp:TemplateColumn>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="imgbtnDel" runat="server" ImageUrl="~/Images/Delete.jpeg" OnClientClick="javascript:return confirm('Are you sure to delete?');"
                                                            CommandName="Delete" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "AutoID")%>' />
                                                    </ItemTemplate>
                                                    <HeaderStyle BackColor="#37689a" ForeColor="White" Width="5%"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Center" Width="5%"></ItemStyle>
                                                </asp:TemplateColumn>
                                            </Columns>
                                            <HeaderStyle Font-Bold="True"></HeaderStyle>
                                        </asp:DataGrid>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" style="padding-left: 30px;">
                                <asp:Label ID="lblGridMsg" runat="server" Text="" Font-Bold="true" ForeColor="Red"></asp:Label>
                                <%--<asp:Button ID="btnPlotChart" runat="server" Text="Plot Chart" CssClass="button"
                                    ValidationGroup="chart" OnClientClick="return GetDate();" OnClick="btnPlotChart_Click" />--%>
                            </td>
                        </tr>
                    </table>
                </div>
            </td>
        </tr>
        <tr>
            <td class="hight5">
            </td>
        </tr>
        <tr>
            <td>
                <div>
                    <table class="percentage100" border="0" cellpadding="0" cellspacing="0">
                        <tr>
                            <td style="width: 15%; padding-left: 30px;">
                                <asp:RadioButton ID="rbTime" runat="server" GroupName="Time" Checked="true" />
                                <asp:Label ID="Label1" class="title" runat="server">Time:</asp:Label>
                                <asp:DropDownList ID="ddlTime" runat="server" Width="50%">
                                    <asp:ListItem Value="7">7 Days</asp:ListItem>
                                    <asp:ListItem Value="30" Selected="True">1 Month</asp:ListItem>
                                    <asp:ListItem Value="90">3 Months</asp:ListItem>
                                    <asp:ListItem Value="182">6 Months</asp:ListItem>
                                    <asp:ListItem Value="365">1 Year</asp:ListItem>
                                    <asp:ListItem Value="1095">3 Years</asp:ListItem>
                                    <asp:ListItem Value="1825">5 Years</asp:ListItem>
                                    <asp:ListItem Value="3650">10 Years</asp:ListItem>
                                    <asp:ListItem Value="5471">15 Years</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td style="width: 85%;">
                                <asp:RadioButton ID="rbDateRange" runat="server" GroupName="Time" />
                                <asp:Label ID="Label2" class="title" runat="server">From Date:</asp:Label>
                                <input type="text" id="datepickerStart" style="width: 80px" />
                                <asp:HiddenField ID="hdnStartDate" runat="server" />
                                <asp:Label ID="Label3" class="title" runat="server">To Date:</asp:Label>
                                <input type="text" id="datepickerEnd" style="width: 80px" />
                                <asp:HiddenField ID="hdnEndDate" runat="server" />
                                <asp:Button ID="btnPlotChart" runat="server" Text="Plot Chart" CssClass="button"
                                    ValidationGroup="chart" OnClientClick="return GetDate();" OnClick="btnPlotChart_Click" />
                                <br />
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td style="padding-left: 100px;">
                                <asp:Label ID="lblDateRangeMsg" runat="server" Text="" ForeColor="Red" Font-Bold="true"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </div>
            </td>
        </tr>
        <tr>
            <td class="percentage100" colspan="2" style="padding-left: 2px;">
                <asp:Chart ID="chrtNavGraph" runat="server" Height="350px" Palette="none" ImageLocation="~/TempImages/ChartPic_#SEQ(300,3)"
                    BackColor="Silver" BackGradientStyle="Center" BorderDashStyle="Solid" BorderWidth="2"
                    Width="680">
                    <%--<Legends>
                        <asp:Legend Enabled="true" IsTextAutoFit="true" Name="Legend1" BackColor="Transparent"
                            Alignment="Center" LegendStyle="Row" Docking="Top" Font="Trebuchet MS, 8.25pt, style=Bold">
                        </asp:Legend>
                    </Legends>--%>
                    <ChartAreas>
                        <asp:ChartArea Name="ChartArea1" BorderColor="64, 64, 64, 64" BorderDashStyle="Solid"
                            BackSecondaryColor="White" BackColor="#ECF4F9" ShadowColor="Transparent" BackGradientStyle="TopBottom">
                            <%--<Area3DStyle Rotation="10" Perspective="10" Inclination="15" IsRightAngleAxes="False"
                                WallWidth="0" IsClustered="False"></Area3DStyle>
                            <AxisY LineColor="64, 64, 64, 64" IsLabelAutoFit="False">
                                <LabelStyle Font="Trebuchet MS, 8.25pt, style=Bold" />
                                <MajorGrid LineColor="64, 64, 64, 64" />
                            </AxisY>
                            <AxisX LineColor="64, 64, 64, 64" IsLabelAutoFit="False">
                                <LabelStyle Font="Trebuchet MS, 8.25pt, style=Bold" />
                                <MajorGrid LineColor="64, 64, 64, 64" />
                            </AxisX>--%>
                        </asp:ChartArea>
                    </ChartAreas>
                </asp:Chart>
            </td>
        </tr>
        <tr>
            <td class="percentage100" colspan="2" style="padding-left: 65px;">
                <div id="divLegands" runat="server" style="width: 500px; overflow: auto;">
                </div>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
