<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Aums.aspx.cs" Inherits="iFrames.Pages.Aums" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript" language="javascript">
        function back() {
            history.go(-1);
        }
       
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <cc1:ToolkitScriptManager ID="ScriptManager1" runat="server">
    </cc1:ToolkitScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
     <table width="550px" cellpadding="3" cellspacing="3">
        <tr class="mainHeader">
            <td>
                ASSET UNDER MANAGEMENT
                <hr />
                <br style="line-height: 25px; color: #bd2027;" />
            </td>
        </tr>
    
    <tr class="SubHeader">
            <td align="center">
                Asset Under Management as on <asp:Label ID="LblAsonDate" SkinID="lblHeader" runat="server" Text="Label"></asp:Label>
            </td>
        </tr>
        <tr class="content">
            <td align="right">
                Amount in Rs. Crores 
            </td>
        </tr>
        <tr>
            <td>
            <asp:ListView ID="LstNatureAum" runat="server" ItemPlaceholderID="itemPlaceHolder1" >
            <layouttemplate>
            
                        <table border="1" cellpadding="3" cellspacing="3" width="550px"  style="border-collapse:collapse; border-color:Black;">
                                    <tr class="ListtableHead">
                                    <th align="center" rowspan="2">Nature</th>                                    
                                    <th colspan="3" align="center">Structure</th>
                                    </tr>
                                    <tr class="ListtableHead">
                                        <th> Open End</th>
                                         <th>Close End</th>
                                         <th>Total</th>                                      
                                    </tr>                   
                               <asp:PlaceHolder ID="itemPlaceHolder1" runat="server" />                    
                         </table>
                </layouttemplate>
                <ItemTemplate>
                                 <tr class='<%# Convert.ToBoolean(Container.DataItemIndex % 2) ? "AlternateRow" : "ListtableRow" %>'>
                                    <td width="15%" align="center" class='<%#Container.DataItemIndex==lastrow-1 ? "SubHeader":"nul"%>'>
                                   <%#Eval("nature").ToString()%>                                        
                                    </td>
                                    <td width="15%"  align="center" class='<%#Container.DataItemIndex==lastrow-1 ? "SubHeader":"nul"%>'>
                                    <%# Eval("openaum").ToString() != "0" ? GetOpenEnd(Int64.Parse(Eval("openaum").ToString())).ToString() : "--"%>    
                                    </td>
                                    <td width="10%"  align="center" class='<%#Container.DataItemIndex==lastrow-1 ? "SubHeader":"nul"%>'>
                                    
                                    <%# Eval("closeaum").ToString() != "0" ? GetCloseEnd(Int64.Parse(Eval("closeaum").ToString())).ToString() : "--"%>    
                                    </td>
                                    <td width="10%"  align="center" class='<%#Container.DataItemIndex==lastrow-1 ? "SubHeader":"nul"%>'>
                                    
                                    <%# GetTotal(Int64.Parse(Eval("Total").ToString()))%>     
                                    </td>
                                                                                     
                                </tr>
           </ItemTemplate>
           <EmptyDataTemplate>
                           No data Found
            </EmptyDataTemplate>
          <%-- <InsertItemTemplate>
            <tr class="AlternateRow">
                                    <td width="15%" style="font-size:small" align="center">
                                    <strong>Total </strong>
                                                                            
                                    </td>
                                    <td width="15%" style="font-size:small" align="center">
                                     <strong><%#GetTotalOpenEnd()%> </strong>   
                                    </td>
                                    <td width="10%" style="font-size:small" align="center">
                                    <strong><%#GetTotalCloseEnd()%> </strong>  
                                    </td>
                                    <td width="10%" style="font-size:small" align="center">
                                     <strong><%#GetTotalTotal()%> </strong>   
                                    </td>
                                                                              
         </tr>
           </InsertItemTemplate>--%>
            </asp:ListView>
            </td>
        </tr>
        <tr align="right">
            <td>
            <asp:Button ID="BtnBack" runat="server" Text="BACK" onclick="BtnBack_Click" />
            </td>
        </tr>
    </table>
    </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
