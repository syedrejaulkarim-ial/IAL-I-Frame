<%@ Page Language="C#" AutoEventWireup="true" ViewStateEncryptionMode="Always" EnableViewStateMac="true" CodeBehind="CompareFund_New.aspx.cs" Inherits="iFrames.WiseInvest.CompareFund_New" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="css/template_css.css" rel="stylesheet" type="text/css" media="all" />
    <link href="css/stylesheet.css" rel="stylesheet" type="text/css" />
    <link href="css/jquery-ui.css" rel="stylesheet" type="text/css" />
    <link href="css/style.css" rel="stylesheet" type="text/css" />
    <%--<script src="js/accor.js" type="text/javascript"></script>--%>
    <script src="js/jquery-1.js" type="text/javascript"></script>
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
            if (listCount == 2) {
                alert("You can compare maximum 2 scheme at a time");
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

            if (listCount == 1) {
                alert("Please select 2 scheme for comparison");
                return false;
            }
            var vlbRetrnMsg = $('#<%=lbRetrnMsg.ClientID %>');

            pop();
            return true;
        }


        $(function () {
            function runEffect() {
                // run the effect
               // $(".toggleClass").toggle();
                var options = {};
                // run the effect
                //$(".toggleClass").toggle('blind', options, 500);
                $("#top3secDiv").toggle('blind', options, 500);
                
            };


            $("#top3sec").click(function () {
                runEffect();
                return false;
            });
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
            $.blockUI({ css: {
                border: 'none',
                padding: '15px',
                backgroundColor: '#000',
                '-webkit-border-radius': '10px',
                '-moz-border-radius': '10px',
                opacity: .5,
                color: '#fff'
            }
            });

            setTimeout($.unblockUI, 5000);         
        }
                    
    </script>
</head>
<body>
    <form id="Form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>    
    <table width="710" border="0" align="left" cellpadding="0" cellspacing="0">
        <tr>
            <td>
                <table width="100%">
                    <tr>
                        <td class="top_icon">
                            <img src="img/compare_fund.png" width="32" height="32" />
                        </td>
                        <td class="top_title">
                            Compare Fund
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" class="top_line_compare">
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <table width="100%" border="0" align="left" cellpadding="0" cellspacing="0">
                    <tr>
                        <td colspan="2">
                            <table width="100%" border="0" align="left" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td class="top_inputcom">
                                        Choose Category
                                    </td>
                                    <td class="top_input3">
                                        <asp:DropDownList ID="ddlCategory" runat="server" Width="400px" AutoPostBack="true"
                                            OnSelectedIndexChanged="ddlCategory_SelectedIndexChanged">
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
                                    <td class="top_input">
                                        Choose Fund
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlSchemes" runat="server" Width="400px">
                                            <asp:ListItem Value="0" Selected="True">-Select Scheme-</asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Please Select Scheme."
                                            ControlToValidate="ddlSchemes" Display="Dynamic" InitialValue="0" ValidationGroup="scheme"></asp:RequiredFieldValidator>
                                        <asp:Button ID="btnAddScheme" runat="server" CssClass="add" OnClick="btnAddScheme_Click"
                                            OnClientClick="Javascript:return ValidateControl();" ValidationGroup="scheme" />
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
                                <tr style="display:none;">
                                    <td class="top_input">
                                        Choose Index
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlIndices" runat="server" Width="400px">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Please Select Indices."
                                            ControlToValidate="ddlIndices" Display="Dynamic" InitialValue="0" ValidationGroup="indices"></asp:RequiredFieldValidator>
                                        <asp:Button ID="btnAddIndices" runat="server" CssClass="add" OnClick="btnAddIndices_Click"
                                            OnClientClick="Javascript:return Listcount();" ValidationGroup="indices" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="top_td">
                                        &nbsp;
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
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <table width="100%" border="0" align="left" cellpadding="0" cellspacing="0">
                            <tr>
                                <td>
                                    <asp:DataGrid class="top_table" ID="dglist" runat="server" AutoGenerateColumns="false"
                                        HeaderStyle-Font-Bold="true" Width="100%" OnItemCommand="dglist_ItemCommand">
                                        <Columns>
                                            <asp:TemplateColumn HeaderText="Name" HeaderStyle-CssClass="top_tableheader">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSchemeId" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container.DataItem, "SCHEME_ID")%>'></asp:Label>
                                                    <asp:Label ID="lblIndId" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container.DataItem, "INDEX_ID")%>'></asp:Label>
                                                    <asp:Label ID="lblSchemeName" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "Sch_Short_Name")%>'></asp:Label>
                                                    <asp:Label ID="lblIndName" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "INDEX_NAME")%>'></asp:Label>
                                                </ItemTemplate>
                                                <%--<HeaderStyle BackColor="#37689a" ForeColor="White" Width="80%"></HeaderStyle>--%>
                                                <ItemStyle CssClass="top_tablerow" HorizontalAlign="Left" Width="70%"></ItemStyle>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn HeaderText="Chart" HeaderStyle-CssClass="top_tableheader">
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
                                                <ItemStyle CssClass="top_tablerow" HorizontalAlign="Left" Width="10%"></ItemStyle>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn HeaderText="Auto ID" Visible="false" HeaderStyle-CssClass="top_tableheader">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblAutoID" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "AutoID")%>'></asp:Label>
                                                </ItemTemplate>
                                                <%--<HeaderStyle BackColor="#37689a" ForeColor="White" Width="5%"></HeaderStyle>--%>
                                                <ItemStyle CssClass="top_tablerow" HorizontalAlign="Left" Width="5%"></ItemStyle>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn HeaderText="Delete" HeaderStyle-CssClass="top_tableheader">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="imgbtnDel" runat="server" ImageUrl="~/Images/Delete.jpeg" OnClientClick="javascript:return confirm('Are you sure to delete?');"
                                                        CommandName="Delete" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "AutoID")%>' />
                                                </ItemTemplate>
                                                <%--<HeaderStyle BackColor="#37689a" ForeColor="White" Width="5%"></HeaderStyle>--%>
                                                <ItemStyle CssClass="top_tablerow" HorizontalAlign="Center" Width="5%"></ItemStyle>
                                            </asp:TemplateColumn>
                                        </Columns>
                                        <HeaderStyle Font-Bold="True"></HeaderStyle>
                                    </asp:DataGrid>
                                    <table>
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
                                <td class="top_td">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td class="top_td">
                                    <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td class="tr_width1" align="right">
                                                <asp:Button ID="btnCompareFund" runat="server" Text=">> Show Performance" CssClass="top_button2"
                                                    OnClientClick="return validateList();" OnClick="btnCompareFund_Click" Visible="false" />
                                            </td>
                                        </tr>
                                        <tr>
                                        <td><hr /></td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblSortPeriod" runat="server" Visible="false" CssClass="heading_compare">
                        Click on 'Time Period' to rank funds on a particular period of your choice.
                                                </asp:Label>
                                               
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td class="top_td">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:GridView ID="GrdCompFund" runat="server" Width="95%" AutoGenerateColumns="false" Visible="false"
                                        CssClass="top_table" OnRowCommand="GrdCompFund_RowCommand">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Scheme Name" HeaderStyle-CssClass="top_tableheader">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSchName" runat="server" Text=' <%#Eval("Sch_Short_Name").ToString() != "" ? Eval("Sch_Short_Name") : "NA"%> ' />
                                                </ItemTemplate>
                                                <ItemStyle CssClass="top_tablerow" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-CssClass="top_tableheader">
                                                <HeaderTemplate>
                                                    <asp:LinkButton ID="Lnk1mth" runat="server" Text="1 mth" Font-Overline="false" CommandName="Per_30_Days"
                                                        Font-Bold="true" ForeColor="#0C4972" />
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl1Mth" runat="server" Text='<%#Eval("Per_30_Days").ToString() != "" ? Eval("Per_30_Days") : "NA"%>' />
                                                </ItemTemplate>
                                                <ItemStyle CssClass="top_tablerow" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-CssClass="top_tableheader">
                                                <HeaderTemplate>
                                                    <asp:LinkButton ID="Lnk3mth" runat="server" Font-Bold="true" ForeColor="#0C4972"
                                                        Text="3 mths" Font-Overline="false" CommandName="Per_91_Days"></asp:LinkButton>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl3Mth" runat="server">
                                <%#Eval("Per_91_Days").ToString() != "" ? Eval("Per_91_Days") : "NA"%> </asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle CssClass="top_tablerow" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-CssClass="top_tableheader">
                                                <HeaderTemplate>
                                                    <asp:LinkButton ID="Lnk6mth" runat="server" Font-Bold="true" Text="6 mths" ForeColor="#0C4972"
                                                        Font-Overline="false" CommandName="Per_182_Days"></asp:LinkButton>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl6Mth" runat="server">
                                <%#Eval("Per_182_Days").ToString() != "" ? Eval("Per_182_Days") : "NA"%> </asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle CssClass="top_tablerow" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-CssClass="top_tableheader">
                                                <HeaderTemplate>
                                                    <asp:LinkButton ID="Lnk1yr" runat="server" Font-Bold="true" Text="1Yr" ForeColor="#0C4972"
                                                        Font-Overline="false" CommandName="Per_1_Year"></asp:LinkButton>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl1yr" runat="server">
                                <%#Eval("Per_1_Year").ToString() != "" ? Eval("Per_1_Year") : "NA"%> </asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle CssClass="top_tablerow" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-CssClass="top_tableheader">
                                                <HeaderTemplate>
                                                    <asp:LinkButton ID="Lnk3yr" runat="server" Font-Bold="true" Text="3 Yrs" ForeColor="#0C4972"
                                                        Font-Overline="false" CommandName="Per_3_Year"></asp:LinkButton>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl3yr" runat="server">
                                <%#Eval("Per_3_Year").ToString() != "" ? Eval("Per_3_Year") : "NA"%> </asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle CssClass="top_tablerow" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-CssClass="top_tableheader">
                                                <HeaderTemplate>
                                                    <asp:Label ID="Label1" runat="server" SkinID="lblHeader" Text="NAV"></asp:Label>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblNav" runat="server">
                                <%#Eval("Nav_Rs").ToString() != "" ? Eval("Nav_Rs") : "NA"%> </asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle CssClass="top_tablerow" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-CssClass="top_tableheader">
                                                <HeaderTemplate>
                                                    <asp:Label ID="Label2" runat="server" SkinID="lblHeader" Text="Category"></asp:Label>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCat" runat="server">
                                <%#Eval("Nature").ToString() != "" ? Eval("Nature") : "NA"%> </asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle CssClass="top_tablerow" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-CssClass="top_tableheader">
                                                <HeaderTemplate>
                                                    <asp:Label ID="Label3" runat="server" SkinID="lblHeader" Text="Structure"></asp:Label>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblStruct" runat="server">
                                <%#Eval("Structure_Name").ToString() != "" ? Eval("Structure_Name") : "NA"%> </asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle CssClass="top_tablerow" />
                                            </asp:TemplateField>
                                        </Columns>
                                        <EmptyDataTemplate>
                                            No Data Found</EmptyDataTemplate>
                                    </asp:GridView>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lbRetrnMsg" Visible="false" runat="server" CssClass="heading_compare">
                        *Note:- Returns calculated for less than 1 year are Absolute returns and returns
                        calculated for more than 1 year are compounded annualized.</asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <div id="resultCompDiv" runat="server">
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
