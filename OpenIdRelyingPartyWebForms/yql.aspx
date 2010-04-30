<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="yql.aspx.cs" Inherits="OpenIdRelyingPartyWebForms.yql" validateRequest="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
    <style type="text/css">
body {
    color: white;
    background-color: purple; 
    font-family: Georgia, "Times New Roman", Times, serif;

}

</style> 

</head>
<body>
    <form id="form1" runat="server">
    <div>
        <h2>YQL Console using .Net</h2>
        Please write your YQL command here:
        <table border="0" cellpadding="3">
        <tr>
          <td valign="top"><asp:TextBox ID="tyql" runat="server" Height="90px" TextMode="MultiLine" 
            Width="682px"></asp:TextBox></td>
          <td valign="top"><asp:Button ID="bquery" runat="server" Text="Query" 
                  onclick="bquery_Click" /></td>
        </tr>
        </table>
         
        <br />
        Result:<br />
        <asp:TextBox ID="tresult" runat="server" Height="310px" TextMode="MultiLine" 
            Width="778px"></asp:TextBox>
    
    </div>
    </form>
</body>
</html>
