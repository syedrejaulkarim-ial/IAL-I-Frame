<%@ Page Language="C#" AutoEventWireup="true" ViewStateEncryptionMode="Always" EnableViewStateMac="true" CodeBehind="ViewIndexValues.aspx.cs"
    Inherits="iFrames.Tata.ViewIndexValues" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link type="text/css" rel="Stylesheet" href="../Chart/css/Chart.css" />
    <link href="../Styles/jquery-ui-1.8.14.custom2.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" language="javascript" src="../Scripts/jquery-1.10.2.js"></script>
    <script type="text/javascript" language="javascript" src="../Scripts/jquery-ui.min-1.10.2.js"></script>
    <script type="text/javascript" language="javascript">
        $(function () {
            $("#txtStarttDate").datepicker({
                dateFormat: 'dd-MM-yy',
                changeMonth: true,
                changeYear: true,
                maxDate: -1
                //                 maxDate: 0
            });
            $("#txtEndDate").datepicker({
                dateFormat: 'dd-MM-yy',
                changeMonth: true,
                changeYear: true,
                maxDate: -1
                //                 maxDate: 0
            });

        });

    </script>
    <style type="text/css">
        .pagebutton
        {
            background-color: #067DCD;
            color: White;
            font-weight: bold;
            text-align: center;
            border: 1;
            border-width: 1;
            cursor: pointer;
            font-family: Verdana;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <center>
            <table width="50%" border="0" cellpadding="0" cellspacing="0">
                <tr>
                    <td align="right">
                        <asp:LinkButton ID="lnlLogout" runat="server" Visible="false" Font-Bold="true" ForeColor="#067DCD"
                            OnClick="lnlLogout_Click">logout</asp:LinkButton>
                    </td>
                </tr>
            </table>
        </center>
        <center>
            <table width="100%" cellpadding="0" cellspacing="0" border="0">
                <tr>
                    <td align="center">
                        <asp:Panel ID="pnlViewIndexValue" runat="server" GroupingText="View Uploaded Index Values"
                            Width="750" Font-Bold="False" HorizontalAlign="Center">
                            <table width="100%" border="0" cellpadding="2" cellspacing="2">
                                <tr>
                                    <td style="width: 25%;">
                                        &nbsp;
                                    </td>
                                    <td style="width: 25%; font-family: verdana; font-size: 12px; color: #067DCD;" align="right"
                                        valign="top">
                                        Index
                                    </td>
                                    <td style="width: 25%;">
                                        <asp:DropDownList ID="ddlIndex" runat="server">
                                        </asp:DropDownList>
                                        <br />
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Please choose index."
                                            ControlToValidate="ddlIndex" Display="Dynamic" InitialValue="0" ValidationGroup="CheckIndex"></asp:RequiredFieldValidator>
                                    </td>
                                    <td style="width: 25%;">
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 25%; color: #067DCD; font-family: verdana; font-size: 12px;" align="right"
                                        valign="top">
                                        Start Date :
                                    </td>
                                    <td style="width: 25%; font-family: verdana; font-size: 12px; color: #067DCD;" align="right"
                                        valign="top">
                                        <asp:TextBox ID="txtStarttDate" runat="server" CssClass="input2" Width="178px" Height="20px"
                                            Text="DD-MMM-YYYY" Style="font-size: 12px;"> </asp:TextBox>
                                        <br />
                                        <span style="font-size: 12px; padding-right: 100px;">(DD/MM/YYYY)</span>
                                    </td>
                                    <td style="width: 25%; color: #067DCD; font-family: verdana; font-size: 12px;" align="right"
                                        valign="top">
                                        End Date :
                                    </td>
                                    <td style="width: 25%; font-family: verdana; font-size: 12px; color: #067DCD;" align="right"
                                        valign="top">
                                        <asp:TextBox ID="txtEndDate" runat="server" CssClass="input2" Width="178px" Height="20px"
                                            Text="DD-MMM-YYYY" Style="font-size: 12px;"> </asp:TextBox>
                                        <br />
                                        <span style="font-size: 12px; padding-right: 100px;">(DD/MM/YYYY)</span>
                                    </td>
                                    <td style="width: 25%;" valign="top">
                                        <asp:Button ID="btnShow" runat="server" Text="Show" OnClick="btnShow_Click" CssClass="pagebutton"
                                            ValidationGroup="CheckIndex" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="height: 5px;" colspan="4">
                                        <asp:Label ID="lblMsg" runat="server" ForeColor="Red" Text=""></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <asp:Panel ID="pnlList" runat="server" GroupingText="View Uploaded Index Values"
                            HorizontalAlign="Center"  Width="750" Visible="false">
                            <table width="100%" border="0" cellpadding="2" cellspacing="2">
                                <tr>
                                    <td align="center">
                                        <asp:GridView ID="gvIndexValue" runat="server" Width="80%" AutoGenerateColumns="False"
                                            Font-Size="Small" BorderWidth="1px" BackColor="White" CellPadding="4" BorderStyle="None"
                                            BorderColor="#3366CC" GridLines="Both" OnPageIndexChanging="gvCompanyShare_PageIndexChanging">
                                            <PagerStyle ForeColor="White" HorizontalAlign="Right" BackColor="#007AC2" CssClass="pager">
                                            </PagerStyle>
                                            <HeaderStyle ForeColor="White" Font-Bold="True" BackColor="#007AC2" Font-Names="Verdana"
                                                Font-Size="9"></HeaderStyle>
                                            <FooterStyle ForeColor="#003399" BackColor="#84AFC8"></FooterStyle>
                                            <Columns>
                                                <asp:TemplateField HeaderText="Index Id" HeaderStyle-Width="10%" Visible="false">
                                                    <ItemTemplate>
                                                        <%#DataBinder.Eval(Container.DataItem, "CUSTOM_INDEX_ID")%>
                                                    </ItemTemplate>
                                                    <ItemStyle Font-Names="verdana" Font-Size="9pt" Width="10%" HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Record date" HeaderStyle-Width="5%">
                                                    <ItemTemplate>
                                                        <%#FomatDate(DataBinder.Eval(Container.DataItem, "RECORD_DATE"))%>
                                                    </ItemTemplate>
                                                    <ItemStyle Font-Names="verdana" Font-Size="9pt" Width="5%" HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Index Value" HeaderStyle-Width="10%">
                                                    <ItemTemplate>
                                                        <%#DataBinder.Eval(Container.DataItem, "CUSTOM_INDEX_VALUE")%>
                                                    </ItemTemplate>
                                                    <ItemStyle Font-Names="verdana" Font-Size="9pt" Width="10%" HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
            </table>
        </center>
    </div>
    </form>
</body>
</html>
