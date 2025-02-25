<%@ Page Language="C#" AutoEventWireup="true" ViewStateEncryptionMode="Always" EnableViewStateMac="true" CodeBehind="CompareFund.aspx.cs" Inherits="iFrames.ValueInvest.CompareFund" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <%--<link href="css/template_css.css" rel="stylesheet" type="text/css" media="all" />--%>
    <link href="css/stylesheet.css" rel="stylesheet" type="text/css" />
    <link href="css/jquery-ui.css" rel="stylesheet" type="text/css" />
    <%--<link href="css/style.css" rel="stylesheet" type="text/css" />--%>
    <script src="../Scripts/jquery-1.9.1.min.js" type="text/javascript"></script>
    <script src="js/jquery-ui.js" type="text/javascript"></script>
    <script src="js/jquery.blockUI.js" type="text/javascript"></script>
    <script type="text/javascript" language="javascript">
        function ValidateControl() {
            var selectedFund = $('#<%=ddlCategory.ClientID %>').find(':selected').val();
            if (selectedFund == 0) {
                alert('Please select any Category.');
                $('#<%=ddlCategory.ClientID %>').focus();
                return false;
            }

            var selectedValue = $('#<%=ddlSchemes.ClientID%> option:selected').val();
            if (selectedValue == null) {
                alert('Please select any Scheme.');
                $('#<%=ddlSchemes.ClientID %>').focus();
                return false;
            }

            var bool = Listcount();
            if (bool == false)
                return false;

            return true;

        }

        function Listcount() {
            var listCount = CountItemList();
            if (listCount == 5) {
                alert("You can compare maximum 5 schemes/indeces at a time");
                return false;
            }
            return true;
        }

        function CountItemList() {

            var schemeCount = 0;
            $('#<%=dglist.ClientID %>').find("input:checkbox").each(function () {
                if (this.id != '') {
                    schemeCount++;
                }
            });

            return schemeCount;
        }

        function validateList() {
            var listCount = 0;
            var datagrid = $('#<%=dglist.ClientID %>');

            //            $('input:checkbox[id$=chkItem]:checked', datagrid).each(function (item, index) {
            //                listCount++;
            //            });

            $('#<%=dglist.ClientID %>').find("input:checkbox").each(function () {
                if (this.checked && this.id != '') {
                    listCount++;
                }
            });




            if (listCount == 0) {
                alert('Please select at least one Item from List.');
                $('#<%=dglist.ClientID %>').focus();
                return false;
            }

            var vlbRetrnMsg = $('#<%=lbRetrnMsg.ClientID %>');


            return true;
        }


        $(function () {

        });



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

        function pop() {
            //            $.blockUI({ message: '<div style="font-size:16px; font-weight:bold">Please wait...</div>' });
            $.blockUI({
                css: {
                    border: 'none',
                    padding: '15px',
                    backgroundColor: '#000',
                    '-webkit-border-radius': '10px',
                    '-moz-border-radius': '10px',
                    opacity: .5,
                    color: '#fff'
                }
            });

            setTimeout($.unblockUI, 15000);
            // window.location = "http://www.google.com";
            return true;
        }

    </script>
</head>
<body>
    <form id="Form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <table border="0" cellspacing="0" cellpadding="0" width="900" align="left" class="main-content">
            <tr>
                <td>
                    <table width="90%" border="0" align="left" cellpadding="0" cellspacing="0">
                        <tr>
                            <td class="value_heading" colspan="2">
                                <img src="img/arw1.jpg" />Compare Fund</td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <table width="100%" border="0" align="left" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td>
                                            <table width="90%" border="0" align="left" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td>&nbsp;</td>
                                                    <td>&nbsp;</td>
                                                </tr>
                                                <tr>
                                                    <td class="value_input">Choose Category</td>
                                                    <td>
                                                        <%--<select name="" class="value_input1">
                                                            <option>Axis Mutual Fund</option>
                                                            <option>Baroda Pioneer Balance Fund</option>
                                                            <option>Birla Sun Life 95- Dividend</option>
                                                        </select>--%>
                                                        <asp:DropDownList ID="ddlCategory" runat="server" Width="400px" CssClass="value_input1" AutoPostBack="true"
                                                            OnSelectedIndexChanged="ddlCategory_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr class="top_td">
                                                    <td>&nbsp;</td>
                                                    <td>&nbsp;</td>
                                                </tr>
                                                <tr>
                                                    <td class="value_input">Choose Fund</td>
                                                    <td>
                                                        <%--<select name="" class="value_input1">
                                                            <option>Axis Banking Debt Fund - Direct - Dly Dividend</option>
                                                            <option>Baroda Pioneer Balance Fund - Direct - Dividend</option>
                                                            <option>Birla Sun Life 95- Dividend</option>
                                                        </select>
                                                        <a href="#">
                                                        <img src="img/add.png" title="Add" /></a>--%>

                                                        <asp:DropDownList ID="ddlSchemes" runat="server" CssClass="value_input1" Width="400px">
                                                            <asp:ListItem Value="0" Selected="True">-Select Scheme-</asp:ListItem>
                                                        </asp:DropDownList>
                                                        <asp:ImageButton ID="btnAddScheme" runat="server" Text="Add" OnClick="btnAddScheme_Click"
                                                            OnClientClick="Javascript:return ValidateControl();" ValidationGroup="scheme" ImageUrl="~/ValueInvest/img/add.png" />
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Please Select Scheme."
                                                            ControlToValidate="ddlSchemes" Display="Dynamic" InitialValue="0" ValidationGroup="scheme"></asp:RequiredFieldValidator>
                                                    </td>
                                                </tr>
                                                <tr class="top_td">
                                                    <td>&nbsp;</td>
                                                    <td>&nbsp;</td>
                                                </tr>
                                                <tr>
                                                    <td class="value_input">Choose Index</td>
                                                    <td>
                                                        <%--<select name="" class="value_input1">
                                                            <option>Axis Banking Debt Fund - Direct - Dly Dividend</option>
                                                            <option>Baroda Pioneer Balance Fund - Direct - Dividend</option>
                                                            <option>Birla Sun Life 95- Dividend</option>
                                                        </select>
                                                        <a href="#"> <img src="img/add.png" title="Add" /></a>--%>

                                                        <asp:DropDownList ID="ddlIndices" runat="server" Width="400px" class="value_input1">
                                                        </asp:DropDownList>
                                                        <asp:ImageButton ID="btnAddIndices" runat="server" Text="Add" ImageUrl="~/ValueInvest/img/add.png" OnClick="btnAddIndices_Click"
                                                            OnClientClick="Javascript:return Listcount();" ValidationGroup="indices" />
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Please Select Indices."
                                                            ControlToValidate="ddlIndices" Display="Dynamic" InitialValue="0" ValidationGroup="indices"></asp:RequiredFieldValidator>
                                                        

                                                    </td>
                                                </tr>
                                                <tr class="top_td">
                                                    <td>&nbsp;</td>
                                                    <td>&nbsp;
                	
                                                    </td>
                                                    <td>&nbsp;</td>
                                                </tr>
                                                <tr class="top_td">
                                                    <td class="top" colspan="2">&nbsp;
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <table width="100%" border="0" align="left" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td class="top_text">&nbsp;</td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <%--<table width="90%" border="0" align="left" cellpadding="0" cellspacing="0" class="value_table">
                                                            <tr class="value_tableheader">
                                                                <td class="value_tableheader">Name</td>
                                                                <td class="value_tableheader">
                                                                    <input type="checkbox" name="checkbox" id="checkbox" />All</td>
                                                                <td class="value_tableheader">Delete</td>
                                                                <tr>
                                                                    <td class="value_tablerow">HDFC Annual Interval Fund - Series I - Plan A</td>
                                                                    <td class="value_tablerow">
                                                                        <input type="checkbox" name="checkbox" id="checkbox1" /></td>
                                                                    <td class="value_tablerow">
                                                                        <img src="img/cross.png" width="14" height="14" /></td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="value_tablerow">HDFC Annual Interval Fund - Series I - Plan A</td>
                                                                    <td class="value_tablerow">
                                                                        <input type="checkbox" name="checkbox" id="checkbox2" /></td>
                                                                    <td class="value_tablerow">
                                                                        <img src="img/cross.png" width="14" height="14" /></td>
                                                                </tr>
                                                        </table>--%>

                                                        <asp:DataGrid class="top_table" ID="dglist" runat="server" AutoGenerateColumns="false"
                                                            HeaderStyle-Font-Bold="true" Width="90%" OnItemCommand="dglist_ItemCommand" CssClass="value_table">
                                                            <Columns>
                                                                <asp:TemplateColumn HeaderText="Name" HeaderStyle-CssClass="value_tableheader">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblSchemeId" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container.DataItem, "SCHEME_ID")%>'></asp:Label>
                                                                        <asp:Label ID="lblIndId" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container.DataItem, "INDEX_ID")%>'></asp:Label>
                                                                        <asp:Label ID="lblSchemeName" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "Sch_Short_Name")%>'></asp:Label>
                                                                        <asp:Label ID="lblIndName" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "INDEX_NAME")%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <%--<HeaderStyle BackColor="#37689a" ForeColor="White" Width="80%"></HeaderStyle>--%>
                                                                    <ItemStyle CssClass="value_tablerow" HorizontalAlign="Left" Width="70%"></ItemStyle>
                                                                </asp:TemplateColumn>
                                                                <asp:TemplateColumn HeaderText="Chart" HeaderStyle-CssClass="value_tableheader">
                                                                    <HeaderTemplate>
                                                                        <input type="checkbox" name="SelectAllCheckBox" onclick="SelectAll(this)">ALL
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:CheckBox ID="chkItem" runat="server"></asp:CheckBox>
                                                                        <asp:HiddenField ID="hdSchID" runat="server" Value='<%#DataBinder.Eval(Container.DataItem, "SCHEME_ID")%>' />
                                                                        <asp:HiddenField ID="hdIndID" runat="server" Value='<%#DataBinder.Eval(Container.DataItem, "INDEX_ID")%>' />
                                                                        <asp:HiddenField ID="hdImgID" runat="server" Value='<%#DataBinder.Eval(Container.DataItem, "AutoID")%>' />
                                                                    </ItemTemplate>
                                                                    <%--<HeaderStyle BackColor="#37689a" ForeColor="White" Width="5%"></HeaderStyle>--%>
                                                                    <ItemStyle CssClass="value_tablerow" HorizontalAlign="Left" Width="10%"></ItemStyle>
                                                                </asp:TemplateColumn>
                                                                <asp:TemplateColumn HeaderText="Auto ID" Visible="false" HeaderStyle-CssClass="value_tableheader">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblAutoID" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "AutoID")%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <%--<HeaderStyle BackColor="#37689a" ForeColor="White" Width="5%"></HeaderStyle>--%>
                                                                    <ItemStyle CssClass="value_tablerow" HorizontalAlign="Left" Width="5%"></ItemStyle>
                                                                </asp:TemplateColumn>
                                                                <asp:TemplateColumn HeaderText="Delete" HeaderStyle-CssClass="value_tableheader">
                                                                    <ItemTemplate>
                                                                        <asp:ImageButton ID="imgbtnDel" runat="server" ImageUrl="~/Images/Delete.jpeg" OnClientClick="javascript:return confirm('Are you sure to delete?');"
                                                                            CommandName="Delete" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "AutoID")%>' />
                                                                    </ItemTemplate>
                                                                    <%--<HeaderStyle BackColor="#37689a" ForeColor="White" Width="5%"></HeaderStyle>--%>
                                                                    <ItemStyle CssClass="value_tablerow" HorizontalAlign="Center" Width="5%"></ItemStyle>
                                                                </asp:TemplateColumn>
                                                            </Columns>
                                                            <HeaderStyle Font-Bold="True"></HeaderStyle>
                                                        </asp:DataGrid>
                                                        
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" style="padding-left: 30px;">
                                                        <asp:Label ID="lblGridMsg" runat="server" Text="" Font-Bold="true" ForeColor="Red"></asp:Label>
                                                        <asp:Label ID="lblErrMsg" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <table width="90%" border="0" align="left" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td class="top_text">&nbsp;</td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <%--<input name="" type="button" class="value_button4" value="Show Performance" />--%>
                                                        <asp:Button ID="btnCompareFund" runat="server" Text="Show Performance" CssClass="value_button4"
                                                        OnClientClick="return validateList();" OnClick="btnCompareFund_Click" Visible="false" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <table width="100%" border="0" align="left" cellpadding="0" cellspacing="0">
                                                            <tr>
                                                                <td class="value_dis">
                                                                    <asp:Label ID="lblSortPeriod" runat="server" Visible="false" CssClass="value_dis">Click on 'Time Period' to rank funds on a particular period of your choice.
                                                                    </asp:Label></td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <%--<table width="100%" border="0" align="left" cellpadding="0" cellspacing="0" class="value_table">
                                                                        <tr class="value_tableheader">
                                                                            <td class="value_tableheader">Scheme Name</td>
                                                                            <td class="value_tableheader">1 Mth</td>
                                                                            <td class="value_tableheader">3 Mths</td>
                                                                            <td class="value_tableheader">6 Mths</td>
                                                                            <td class="value_tableheader">1 Yr</td>
                                                                            <td class="value_tableheader">3 Yrs</td>
                                                                            <td class="value_tableheader">NAV</td>
                                                                            <td class="value_tableheader">Category</td>
                                                                            <td class="value_tableheader">Structure</td>
                                                                            <tr>
                                                                                <td class="value_tablerow">HDFC Annual Interval Fund - Series I - Plan A</td>
                                                                                <td class="value_tablerow">12.24</td>
                                                                                <td class="value_tablerow">12.24</td>
                                                                                <td class="value_tablerow">12.24</td>
                                                                                <td class="value_tablerow">12.24</td>
                                                                                <td class="value_tablerow">12.24</td>
                                                                                <td class="value_tablerow">12.24</td>
                                                                                <td class="value_tablerow">Open Ended</td>
                                                                                <td class="value_tablerow">Balanced</td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="value_tablerow">HDFC Annual Interval Fund - Series I - Plan A</td>
                                                                                <td class="value_tablerow">12.24</td>
                                                                                <td class="value_tablerow">12.24</td>
                                                                                <td class="value_tablerow">12.24</td>
                                                                                <td class="value_tablerow">12.24</td>
                                                                                <td class="value_tablerow">12.24</td>
                                                                                <td class="value_tablerow">12.24</td>
                                                                                <td class="value_tablerow">Open Ended</td>
                                                                                <td class="value_tablerow">Balanced</td>
                                                                            </tr>
                                                                    </table>--%>


                                                                    <asp:GridView ID="GrdCompFund" runat="server" Width="100%" AutoGenerateColumns="false"
                                                                        CssClass="value_table" OnRowCommand="GrdCompFund_RowCommand">
                                                                        <Columns>
                                                                            <asp:TemplateField HeaderText="Scheme Name" HeaderStyle-CssClass="value_tableheader">
                                                                                <ItemTemplate>
                                                                                    <%--  <a href='FundDetails.aspx?id=<%#Eval("Sch_id")%>'> <asp:Label ID="lblSchName" runat="server" Text='<%#Eval("Sch_Short_Name").ToString() != "" ? Eval("Sch_Short_Name") : "NA"%>' /> </a>
                                                 
                                                   <% Eval("Sch_id").ToString().Trim() != "-1" ?
                                                    <a href='FundDetails.aspx?id=<%#Eval("Sch_id")%>'>
                                                        <asp:Label ID="Label4" runat="server" Text='<%#Eval("Sch_Short_Name").ToString() != "" ? Eval("Sch_Short_Name") : "NA"%>' /></a>
                                                    :
                                                    Eval("Sch_Short_Name").ToString()%>--%>
                                                                                    <%# SetHyperlinkFundDetail(Eval("Sch_id").ToString(), Eval("Sch_Short_Name").ToString())%>
                                                                                </ItemTemplate>
                                                                                <ItemStyle CssClass="value_tablerow" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderStyle-CssClass="value_tableheader">
                                                                                <HeaderTemplate>
                                                                                    <asp:LinkButton ID="Lnk1mth" runat="server" Text="1 mth" Font-Overline="false" CommandName="Per_30_Days"
                                                                                        Font-Bold="true" ForeColor="#0C4972" />
                                                                                </HeaderTemplate>
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lbl1Mth" runat="server" Text='<%#Eval("Per_30_Days").ToString() != "" ? Eval("Per_30_Days") : "NA"%>' />
                                                                                </ItemTemplate>
                                                                                <ItemStyle CssClass="value_tablerow" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderStyle-CssClass="value_tableheader">
                                                                                <HeaderTemplate>
                                                                                    <asp:LinkButton ID="Lnk3mth" runat="server" Font-Bold="true" ForeColor="#0C4972"
                                                                                        Text="3 mths" Font-Overline="false" CommandName="Per_91_Days"></asp:LinkButton>
                                                                                </HeaderTemplate>
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lbl3Mth" runat="server">
                                <%#Eval("Per_91_Days").ToString() != "" ? Eval("Per_91_Days") : "NA"%> </asp:Label>
                                                                                </ItemTemplate>
                                                                                <ItemStyle CssClass="value_tablerow" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderStyle-CssClass="value_tableheader">
                                                                                <HeaderTemplate>
                                                                                    <asp:LinkButton ID="Lnk6mth" runat="server" Font-Bold="true" Text="6 mths" ForeColor="#0C4972"
                                                                                        Font-Overline="false" CommandName="Per_182_Days"></asp:LinkButton>
                                                                                </HeaderTemplate>
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lbl6Mth" runat="server">
                                <%#Eval("Per_182_Days").ToString() != "" ? Eval("Per_182_Days") : "NA"%> </asp:Label>
                                                                                </ItemTemplate>
                                                                                <ItemStyle CssClass="value_tablerow" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderStyle-CssClass="value_tableheader">
                                                                                <HeaderTemplate>
                                                                                    <asp:LinkButton ID="Lnk1yr" runat="server" Font-Bold="true" Text="1Yr" ForeColor="#0C4972"
                                                                                        Font-Overline="false" CommandName="Per_1_Year"></asp:LinkButton>
                                                                                </HeaderTemplate>
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lbl1yr" runat="server">
                                <%#Eval("Per_1_Year").ToString() != "" ? Eval("Per_1_Year") : "NA"%> </asp:Label>
                                                                                </ItemTemplate>
                                                                                <ItemStyle CssClass="value_tablerow" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderStyle-CssClass="value_tableheader">
                                                                                <HeaderTemplate>
                                                                                    <asp:LinkButton ID="Lnk3yr" runat="server" Font-Bold="true" Text="3 Yrs" ForeColor="#0C4972"
                                                                                        Font-Overline="false" CommandName="Per_3_Year"></asp:LinkButton>
                                                                                </HeaderTemplate>
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lbl3yr" runat="server">
                                <%#Eval("Per_3_Year").ToString() != "" ? Eval("Per_3_Year") : "NA"%> </asp:Label>
                                                                                </ItemTemplate>
                                                                                <ItemStyle CssClass="value_tablerow" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderStyle-CssClass="value_tableheader">
                                                                                <HeaderTemplate>
                                                                                    <asp:Label ID="Label1" runat="server" SkinID="lblHeader" Text="NAV"></asp:Label>
                                                                                </HeaderTemplate>
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblNav" runat="server">
                                <%#Eval("Nav_Rs").ToString() != "" ? Eval("Nav_Rs") : "NA"%> </asp:Label>
                                                                                </ItemTemplate>
                                                                                <ItemStyle CssClass="value_tablerow" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderStyle-CssClass="value_tableheader">
                                                                                <HeaderTemplate>
                                                                                    <asp:Label ID="Label2" runat="server" SkinID="lblHeader" Text="Category"></asp:Label>
                                                                                </HeaderTemplate>
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblCat" runat="server">
                                <%#Eval("Nature").ToString() != "" ? Eval("Nature") : "NA"%> </asp:Label>
                                                                                </ItemTemplate>
                                                                                <ItemStyle CssClass="value_tablerow" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderStyle-CssClass="value_tableheader">
                                                                                <HeaderTemplate>
                                                                                    <asp:Label ID="Label3" runat="server" SkinID="lblHeader" Text="Structure"></asp:Label>
                                                                                </HeaderTemplate>
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblStruct" runat="server">
                                <%#Eval("Structure_Name").ToString() != "" ? Eval("Structure_Name") : "NA"%> </asp:Label>
                                                                                </ItemTemplate>
                                                                                <ItemStyle CssClass="value_tablerow" />
                                                                            </asp:TemplateField>
                                                                        </Columns>
                                                                        <EmptyDataTemplate>
                                                                            No Data Found
                                                                        </EmptyDataTemplate>
                                                                    </asp:GridView>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="value_dis1">
                                            <asp:Label ID="lbRetrnMsg" Visible="false" runat="server" CssClass="value_dis1">
                        *Note:- Returns calculated for less than 1 year are Absolute returns and returns
                        calculated for more than 1 year are compounded annualized.</asp:Label>

                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="value_dis">Disclaimer: Mutual Fund investments are subject to market risks. Read all scheme related documents carefully before investing. Past performance of the schemes do not indicate the future performance. 
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="value_btm_text">Developed and Maintained by: <a href="https://www.icraanalytics.com/" target="_blank">ICRA Analytics Ltd</a> </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <table width="70%" border="0" align="left" cellpadding="0" cellspacing="0">
                        <tr>
                            <td class="value_btm_text">&nbsp; </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>