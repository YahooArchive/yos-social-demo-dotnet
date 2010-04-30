<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="yahoo.aspx.cs" Inherits="OpenIdRelyingPartyWebForms.yahoo" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        <asp:Button ID="btn1" runat="server" onclick="btn1_Click" 
            Text="Login with Yahoo!" />
        <asp:Button ID="Button1" runat="server" onclick="Button1_Click" Text="Button" />
        <br />
        <br />
        <asp:TextBox ID="txt1" runat="server" Height="236px" TextMode="MultiLine" 
            Width="637px"></asp:TextBox>
        <br />
    
    </div>
    <asp:PlaceHolder ID="PlaceHolder1" runat="server"></asp:PlaceHolder>
    </form>
</body>
</html>
