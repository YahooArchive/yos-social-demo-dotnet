<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="member.aspx.cs" Inherits="OpenIdRelyingPartyWebForms.member" %>

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
    
        Member Area<br />
        <table>
          <tr>
            <td>Name:</td>
            <td><asp:Label ID="lname" runat="server" Text=""></asp:Label></td>
          </tr>
        </table>
        
&nbsp;
    </div>
    </form>
</body>
</html>
