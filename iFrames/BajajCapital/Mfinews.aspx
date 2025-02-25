<%@ Page Language="C#" AutoEventWireup="true" ViewStateEncryptionMode="Always" EnableViewStateMac="true" CodeBehind="Mfinews.aspx.cs" Inherits="iFrames.BajajCapital.Mfinews" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>MFI News</title>
    <script src="js/jqueryNew.js" type="text/javascript"></script>
    <script src="js/navigation.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            $("[name^='Clickable']").on('click', function (event) {
                // debugger;
                $(this).toggleClass("main");
                //alert($(this).closest('td').find('span:first').text());                               
                //$(this).closest('td').find('span:first').toggle();
                if ($(this).closest('td').find('span:first').css('display') == 'none') {
                    $("span.collapse").hide();
                    $(this).closest('td').find('span:first').toggle(400);
                } else {
                    $(this).closest('td').find('span:first').hide();
                }
            });
        });
    </script>
    <style type="text/css">
        body{
            font-family: 'Open Sans', sans-serif !important;
            font-size: 80% !important;
        }
        a:hover {
            text-decoration: none !important;
        }

        .main {
            width: 100%;
            display: inline-block;
            margin-bottom: 6px;
            padding-bottom: 5px;
        }

        .collapse {
            line-height: 22px;
        }

        .border0 {
            border: 0 !important
        }

        .pad0 {
            padding: 0 !important
        }

        .btn_theme {
            background: #E77817;
            color: #fff;
            padding: 3px 10px;
            border: 0;
            border-radius: 5px;
        }

        #dpNews {
            position: relative;
            left: -18px;
        }

        #tbl tr td:first-child {
            /*border-left: 0 !important;*/
            text-align: center;
        }

        #tbl tr td:last-child {
            border-right: 1px solid #ddd !important;
            text-align: center;
        }

        input[type="text"], select {
            margin-bottom: 0 !important;
        }

        .table th, .table td {
            vertical-align: middle !important;
            line-height: 15px !important;
        }

        /*#tbl tr:nth-child(odd) {
            background: #f7f7f7;
        }*/

        #dpNews a {
            background: rgb(165, 139, 34);
            padding: 4px 10px;
            border-radius: 5px;
            cursor: pointer;
            text-decoration: none;
            color: #ecebeb;
            font-size: 12px;
        }

        #dpNews .news_header {
            background: rgb(88, 75, 19) !important;
            padding: 4px 10px;
            border-radius: 5px;
            cursor: pointer;
            text-decoration: none;
            color: #ecebeb;
            font-size: 12px;
        }

        .news_header {
            background: rgb(251, 201, 0) !important;
        }

        #txtSearchBox, select {
            width: auto !important;
        }
        .news_header td{
            font-weight:bold;
        }
    </style>
    <link href="bootstrap/css/bootstrap.css" rel="stylesheet" />
    <link href="https://fonts.googleapis.com/css?family=Open+Sans" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <table border="0" cellspacing="0" cellpadding="0" width="650" align="left" class="table table-bordered" style="width: 650px!important">
            <tr>
                <td style="display: none">
                    <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                        <tr class="news_head">
                            <td width="2%">
                                <img src="img/news_arw.png" width="10" height="9" />
                            </td>
                            <td width="98%" align="left" valign="middle">
                                <strong>&nbsp;</strong>Mutual Fund News Update
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td style="padding:0">
                    <table width="100%" border="0" align="left" cellpadding="0" cellspacing="0">
                        <tr>
                            <td class="top_icon" style="border: 0">
                                <img src="img/mfi_news.png" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <table border="0" align="left" cellpadding="0" cellspacing="0" style="width: 100%">
                        <tr>
                            <td style="margin: 0; padding: 0;" class="border0">
                                <asp:TextBox ID="txtSearchBox" runat="server" placeholder="News title" autoComplete="off"></asp:TextBox>
                            </td>
                            <td style="margin: 0; padding: 0;" class="border0">
                                <asp:DropDownList ID="NewsYear" runat="server" AutoPostBack="false">
                                </asp:DropDownList>
                            </td>
                            <td class="border0 pad0">
                                <asp:DropDownList ID="NewsMonth" runat="server" AutoPostBack="false">
                                    <asp:ListItem Value="">Choose a month</asp:ListItem>
                                    <asp:ListItem Value="1">January</asp:ListItem>
                                    <asp:ListItem Value="2">February</asp:ListItem>
                                    <asp:ListItem Value="3">March</asp:ListItem>
                                    <asp:ListItem Value="4">April</asp:ListItem>
                                    <asp:ListItem Value="5">May</asp:ListItem>
                                    <asp:ListItem Value="6">June</asp:ListItem>
                                    <asp:ListItem Value="7">July</asp:ListItem>
                                    <asp:ListItem Value="8">August</asp:ListItem>
                                    <asp:ListItem Value="9">September</asp:ListItem>
                                    <asp:ListItem Value="10">October</asp:ListItem>
                                    <asp:ListItem Value="11">November</asp:ListItem>
                                    <asp:ListItem Value="12">December</asp:ListItem>
                                </asp:DropDownList>
                            </td>

                            <td class="border0 pad0">
                                <asp:Button ID="btnSearch" runat="server" Text="Go" OnClick="btnSearch_Click" class="btn_theme" AutoPostBack="false" />
                                <%--OnClientClick="Javascript:Encode();"--%>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td style="padding: 10px 0; text-align: right; border-left: 1px solid #dddddd;">
                    <asp:ListView ID="lvMfinews" runat="server" ItemPlaceholderID="itemPlaceholder" OnPagePropertiesChanging="lvMfinews_PagePropertiesChanging">
                        <%--OnPreRender="dpNews_PreRender"--%>
                        <LayoutTemplate>
                            <table id="tbl" style="width: 95%; margin-left: 10px; border-bottom: 1px solid #ddd;">
                                <tr class="news_header">
                                    <td class="text_border1" style="border-radius: 0"></td>
                                    <td align="left" class="text_border1">
                                        <asp:Label ID="Label1" runat="server">Article Title</asp:Label>
                                    </td>
                                    <td class="text_border1" style="border-radius: 0;">
                                        <asp:Label ID="Label2" runat="server">Report Date</asp:Label>
                                    </td>
                                </tr>
                                <tr runat="server" id="itemPlaceholder" />
                            </table>
                        </LayoutTemplate>
                        <ItemTemplate>
                            <%--<tr class='<%# Convert.ToBoolean(Container.DisplayIndex % 2) ? "news_altrow" : "news_row" %>'>--%>
                            <tr class="news_row1">
                                <td>
                                    <img src="../DSP/IMG/news_arw3.png" />&nbsp;&nbsp;
                                </td>
                                <td>
                                    <a href="javascript:void(0)" class="Clickable" name="Clickable">
                                        <%#Eval("NEWS_HEADLINE")%></a><br />
                                    <span class="collapse" style="display: none">
                                        <%#Eval("MATTER")%>
                                    </span>
                                </td>
                                <td style="white-space: nowrap">
                                    <%# Convert.ToDateTime(((System.Data.DataRowView)Container.DataItem)["DISPLAY_DATE"]).ToString("dd MMM yyyy")%>
                                </td>
                            </tr>
                        </ItemTemplate>
                        <EmptyDataTemplate>
                            Data not Found
                        </EmptyDataTemplate>
                    </asp:ListView>
                    <div style="padding-top: 10px;">
                    </div>
                    <asp:DataPager ID="dpNews" runat="server" PagedControlID="lvMfinews" PageSize="10"
                        EnableViewState="false" ViewStateMode="Disabled">
                        <%--OnPreRender="dpNews_PreRender"--%>
                        <Fields>
                            <asp:NextPreviousPagerField ShowFirstPageButton="True" ShowNextPageButton="False" />
                            <asp:NumericPagerField CurrentPageLabelCssClass="news_header" />
                            <asp:NextPreviousPagerField ShowLastPageButton="True" ShowPreviousPageButton="False" />
                            <asp:TemplatePagerField>
                                <PagerTemplate>
                                    <span style="padding-left: 40px;">Page
                                        <asp:Label runat="server" ID="CurrentPageLabel" Text="<%# Container.TotalRowCount>0 ? (Container.StartRowIndex / Container.PageSize) + 1 : 0 %>" />
                                        of
                                        <asp:Label runat="server" ID="TotalPagesLabel" Text="<%# Math.Ceiling (System.Convert.ToDouble(Container.TotalRowCount) / Container.PageSize) %>" />
                                        (
                                        <asp:Label runat="server" ID="TotalItemsLabel" Text="<%# Container.TotalRowCount%>" />
                                        records)
                                        <br />
                                    </span>
                                </PagerTemplate>
                            </asp:TemplatePagerField>
                        </Fields>
                    </asp:DataPager>
                    <div style="padding-top: 10px;">
                    </div>
                </td>
            </tr>
            <tr>
                <td style="padding: 0">
                    <table>
                        <tr>
                            <td class="disclaimerh" style="border-left: 0">Disclaimer
                            </td>
                        </tr>
                        <tr>
                            <td class="disclaimer border0"><small>Mutual Fund investments are subject to market risks. Read all scheme related documents carefully before investing.  Past performance of the schemes do not indicate the future performance.</small>
                            </td>
                        </tr>
                        <tr>
                            <td class="text" align="right" style="padding-left: 20px; text-align: right; padding: 10px 25px 5px; border: 0;">
                                <small style="text-align: right" class="rslt_text1">Developed for Bajaj Capital by:<a
                                    class="text" href="https://www.icraanalytics.com" target="_blank"> ICRA Analytics Ltd,</a>
                                    <a
                                        class="text" href="https://icraanalytics.com/home/Disclaimer" target="_blank">Disclaimer</a>
                                </small>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
