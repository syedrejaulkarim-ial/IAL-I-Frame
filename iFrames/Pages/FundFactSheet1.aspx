<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FundFactSheet1.aspx.cs"
    Inherits="iFrames.Pages.FundFactSheet1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table width="540px" cellpadding="2" cellspacing="2">
            <tr>
                <td align="justify" colspan="2" style="height: 55px;">
                    <span class="mainHeader">FACT SHEET - </span><SPAN class="SubHeader"><%=schName%></SPAN>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <div id="menu">
                        <ul>
                            <li><a href='FundFactSheet1.aspx?sch=<%=schCode%>&comID=<%=this.PropCompID %>'>Fund Facts</a></li>
                            <li><a href='FundFactSheet2.aspx?sch=<%=schCode%>&comID=<%=this.PropCompID %>'>NAV</a></li>
                            <li><a href='FundFactSheet3.aspx?sch=<%=schCode%>&comID=<%=this.PropCompID %>'>Risk Return</a></li>
                            <li><a href='FundFactSheet4.aspx?sch=<%=schCode%>&comID=<%=this.PropCompID %>'>Portfolio</a></li>
                            <li><a href='FundFactSheet5.aspx?sch=<%=schCode%>&comID=<%=this.PropCompID %>'>News</a></li>
                        </ul>
                    </div>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:DetailsView ID="dtvSchemeType" runat="server" Height="50px" AutoGenerateRows="false"
                        Width="100%">
                        <Fields>
                            <asp:BoundField DataField="type1" HeaderText="Type of Scheme" />
                            <asp:BoundField DataField="nature" HeaderText="Nature" />
                            <asp:BoundField DataField="type3" HeaderText="Option" />
                            <asp:TemplateField HeaderText="Inception Date">
                                <ItemTemplate>
                                    <asp:Label ID="lblIncpDate" runat="server" Text='<%#Convert.ToDateTime(Eval("InceptionDate").ToString()).ToString("MMM dd, yyyy") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="FaceVal" HeaderText="Face Value (Rs/Unit)" />
                            <asp:TemplateField HeaderText="Fund Size in Rs. Cr.">
                                <ItemTemplate>
                                    <asp:Label ID="lblFundSize" runat="server" Text='<%#Eval("fund_Size") +" as on "+ Convert.ToDateTime(Eval("FundSizeDate").ToString()).ToString("MMM dd, yyyy")+" (Quartely Avg.)" %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Fields>
                    </asp:DetailsView>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <hr width="95%" />
                </td>
            </tr>
            <tr>
                <td width="50%" align="justify" valign="top">
                    <asp:DetailsView ID="dtvLastDivdend" runat="server" Height="50px" AutoGenerateRows="false"
                        Width="95%">
                        <Fields>
                            <asp:TemplateField HeaderText="Last Divdend Declared">
                                <ItemTemplate>
                                    <asp:Label ID="lblLastDiv" runat="server" Text='<%#Eval("Divid_pt")==""?"NA":Eval("Divid_pt")+" as on "+Eval("DivDate") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Minimum Investment (Rs)">
                                <ItemTemplate>
                                    <asp:Label ID="lblMinInv" runat="server" Text='<%#(Convert.ToDouble(Eval("min_invt"))-53)/76%>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="pur_redm" HeaderText="Purchase Redemptions" />
                            <asp:BoundField DataField="nav_calc" HeaderText="NAV Calculation" />
                            <asp:TemplateField HeaderText="Entry Load">
                                <ItemTemplate>
                                    <asp:Label ID="lblEntryLoad" runat="server" Text='Nil' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Exit Load">
                                <ItemTemplate>
                                    <asp:Label ID="lblExitLoad" runat="server" Text='Nil' />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Fields>
                    </asp:DetailsView>
                </td>
                <td align="justify" width="50%" valign="top">
                    <asp:DetailsView ID="dtvFundManager" runat="server" Height="50px" AutoGenerateRows="false"
                        Width="100%">
                        <Fields>
                            <asp:BoundField DataField="Fund_Manager1" HeaderText="Fund Manager" />
                            <asp:TemplateField HeaderText="SIP">
                                <ItemTemplate>
                                    <asp:Image ID="imgSip" runat="server" ImageUrl='<%#Eval("sip").ToString().ToUpper()=="YES"?"~/Images/tru_img.jpg":"~/Images/fal_img.jpg" %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="STP">
                                <ItemTemplate>
                                    <asp:Image ID="imgStp" runat="server" ImageUrl='<%#Eval("STPInAllowedFlag").ToString().ToUpper()=="YES"?"~/Images/tru_img.jpg":"~/Images/fal_img.jpg" %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="SWP">
                                <ItemTemplate>
                                    <asp:Image ID="imgSwp" runat="server" ImageUrl='<%#Eval("SWPAllowedFlag").ToString().ToUpper()=="YES"?"~/Images/tru_img.jpg":"~/Images/fal_img.jpg" %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="ExpenseRatio" HeaderText="Expense ratio(%)" />
                            <asp:BoundField DataField="ratio" HeaderText="Portfolio Turnover Ratio(%)" />
                        </Fields>
                    </asp:DetailsView>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <hr width="95%" />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:DetailsView ID="dtvFundFacts" runat="server" Height="50px" AutoGenerateRows="false">
                        <Fields>
                            <asp:TemplateField HeaderText="Increase/Decrease in Fund Size  (Rs. in crores)">
                                <ItemTemplate>
                                    <asp:Label ID="lblIncDecFundSize" runat="server" Text='<%#Eval("IncDecFundSize")+"(since "+Convert.ToDateTime(Eval("FundSizeDate").ToString()).ToString("MMM dd, yyyy")+")"%>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Mutual Fund">
                                <ItemTemplate>
                                    <asp:Label ID="lblMutualFund" runat="server" Text='<%#Eval("Mut_Name")+" "+Eval("Reg_Add1")+" "+Eval("Reg_Add2")+" "+Eval("Reg_city")+" Tel.- "+Eval("Reg_Phone1")+", "+Eval("Reg_Phone2")+", "+Eval("Reg_Phone3") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Asset Management Company">
                                <ItemTemplate>
                                    <asp:Label ID="lblAMC" runat="server" Text='<%#Eval("AMC_Name")+" "+Eval("amcAddress1")+" "+Eval("amcAddress2")+" "+Eval("amcCity")+" "+Eval("amcPin")+" Tel.- "+Eval("amcPhone1") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Registrar">
                                <ItemTemplate>
                                    <asp:Label ID="lblRegistrar" runat="server" Text='<%#Eval("Reg_Name")+" "+Eval("Add1")+" "+Eval("Add2")+" "+Eval("City")+" Tel.- "+Eval("Phone1")+", "+Eval("Phone2")+", "+Eval("Phone3") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Fields>
                    </asp:DetailsView>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
