<%@ Page Language="C#" AutoEventWireup="true" ViewStateEncryptionMode="Always" EnableViewStateMac="true" CodeBehind="TataUploadExcel.aspx.cs"
    Inherits="iFrames.Tata.TataUploadExcel" %>

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
            $("#txttDate").datepicker({
                dateFormat: 'dd-MM-yy',
                changeMonth: true,
                changeYear: true,
                maxDate: -1
                //                 maxDate: 0
            });
        });

    </script>
    <script type="text/javascript" language="javascript">

        function Validate() {

            var listBoxSelection = document.getElementById('<%=ddlIndex.ClientID%>').value;
            if (listBoxSelection == 0) {
                alert("Please Choose Index");
                return false;
            }


            if (document.getElementById('<%=txttDate.ClientID%>').value == "" || document.getElementById('<%=txttDate.ClientID%>').value == "DD-MMM-YYYY") {
                alert("Date cannot be Blank");
                document.getElementById('<%=txttDate.ClientID%>').focus();
                return false;
            }

            if (document.getElementById('<%=txtIndexValue.ClientID%>').value == "") {
                alert("Index Value cannot be Blank");
                document.getElementById('<%=txtIndexValue.ClientID%>').focus();
                return false;
            }

            var str = document.getElementById('<%=txtIndexValue.ClientID%>').value;
            if (isNaN(str) || str.indexOf(".") < 0) {
                alert(" Index value should be two fractional positions required");
                return false;
            }
            else {
                var check = 100 * str == parseInt(100 * str, 10);
                if (!check) {
                    alert(" Index value should be two fractional positions required");
                    document.getElementById('<%=txtIndexValue.ClientID%>').value = "";
                    document.getElementById('<%=txtIndexValue.ClientID%>').focus();
                }
                return check;
            }
        }
    </script>
    <script type="text/javascript" language="javascript">
        function isNumber(key) {
            var keycode = (key.which) ? key.which : key.keyCode;

            if ((keycode >= 46 && keycode <= 58) || keycode == 8 || keycode == 9) {

                if ((keycode >= 46 && keycode <= 58)) {

                }
                return true;
            }
            return false;
        }
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
        <asp:Panel ID="pnlLogin" runat="server" GroupingText="Log In" Width="485" Style="color: #067DCD;
            font-family: vredana; font-size: 14px; font-weight: bold;">
            <table width="50%" border="0" cellpadding="0" cellspacing="0">
                <tr>
                    <td colspan="2" align="center">
                        <asp:Label ID="lblMsgError" runat="server" Text="" ForeColor="Red" Font-Bold="true"
                            Font-Size="Small"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="right" style="font-family: verdana; font-size: 12px">
                        User ID:
                    </td>
                    <td>
                        <asp:TextBox ID="txtUserid" runat="server"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="*"
                            ControlToValidate="txtUserid" Display="Dynamic" ValidationGroup="login"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td style="height: 10px;">
                    </td>
                </tr>
                <tr>
                    <td align="right" style="font-family: verdana; font-size: 12px">
                        Password:
                    </td>
                    <td>
                        <asp:TextBox ID="txtPassword" runat="server" TextMode="Password"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="*"
                            ControlToValidate="txtPassword" Display="Dynamic" ValidationGroup="login"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td style="height: 10px;">
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td>
                        <asp:Button ID="btnLogin" runat="server" Text="Log In" OnClick="btnLogin_Click" ValidationGroup="login"
                            CssClass="pagebutton" />
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <asp:Panel ID="pnlUpload" runat="server" GroupingText="Upload Excel" Width="450"
            Style="color: #067DCD; font-family: vredana; font-size: 14px;">
            <table width="100%" border="0" cellpadding="2" cellspacing="2">
                <tr>
                    <td colspan="2" align="center">
                        <asp:Label ID="lblMsg" runat="server" Text="" ForeColor="Green" Font-Bold="true"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="left" style="font-family: verdana; font-size: 12px">
                        Index:
                    </td>
                    <td style="width: 20px;" align="left">
                        <asp:DropDownList ID="ddlIndex" runat="server">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td align="left" style="font-family: verdana; font-size: 12px">
                        Date:
                    </td>
                    <td style="width: 20px;" align="left">
                        <asp:TextBox ID="txttDate" runat="server" CssClass="input2" Width="185px" Height="20px"
                            Text="DD-MMM-YYYY"> </asp:TextBox>
                        <br />
                        <span style="font-size: 12px; padding-right: 100px;">(DD/MM/YYYY)</span>
                    </td>
                </tr>
                <tr>
                    <td align="left" style="font-family: verdana; font-size: 12px">
                        Index Value
                    </td>
                    <td style="width: 20px;" align="left">
                        <asp:TextBox ID="txtIndexValue" runat="server" onkeypress="return isNumber(event)"
                            Width="185px" Height="20px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td align="left" style="font-family: verdana; font-size: 12px;">
                        Upload:
                    </td>
                    <td style="width: 20px;">
                        <asp:FileUpload ID="FUExcell" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td align="left">
                        <div>
                            <table>
                                <tr>
                                    <td>
                                        <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" CssClass="pagebutton"
                                            ValidationGroup="SaveIndex" OnClientClick="return Validate();" />
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                        <asp:Button ID="btnUpload" runat="server" Text="Upload" OnClick="btnUpload_Click"
                                            CssClass="pagebutton" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td align="left">
                        <a href="ViewIndexValues.aspx" style="font-family: verdana; font-size: 12px; text-decoration: none;
                            color: #067DCD;">View Uploaded Index Value</a>
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <div style="height: 5px;">
        </div>
        <asp:Panel ID="pnlInstruction" HorizontalAlign="Center" runat="server">
            <table width="100%" border="0" cellpadding="2" cellspacing="2">
                <tr>
                    <td style="width: 52%; font-family: verdana; font-size: 12px; color: #067DCD;" align="right">
                        Excel Format
                    </td>
                    <td align="left" style="font-family: verdana; font-size: 12px; color: #067DCD;">
                        Instruction
                    </td>
                </tr>
                <tr>
                    <td align="right" style="font-family: Verdana; font-size: 12px;">
                        <div>
                            <table border="1" cellpadding="2" cellspacing="4">
                                <tr>
                                    <td>
                                        Index_Id
                                    </td>
                                    <td>
                                        Date(DD/MM/YYYY)
                                    </td>
                                    <td>
                                        Index Value
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        value
                                    </td>
                                    <td>
                                        value
                                    </td>
                                    <td>
                                        value
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        value
                                    </td>
                                    <td>
                                        value
                                    </td>
                                    <td>
                                        value
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3">
                                        **Sheet Name should be Upload Index
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </td>
                    <td style="font-family: Verdana; font-size: 12px;">
                        <div>
                            <table border="1" cellpadding="2" cellspacing="4">
                                <tr>
                                    <td>
                                        index id
                                    </td>
                                    <td>
                                        Index Name
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        1
                                    </td>
                                    <td>
                                        I-Sec Composite
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        2
                                    </td>
                                    <td>
                                        MSCI World Index
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        3
                                    </td>
                                    <td>
                                        MSCI Emerging Market Index
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </center>
    </form>
</body>
</html>
