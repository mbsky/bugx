<%@ Page Language="C#" AutoEventWireup="true"  CodeFile="Default.aspx.cs" Inherits="_Default"  %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>EnableViewStateMac="false" EnableEventValidation="false" EnableViewState="false"
    <form id="form1" runat="server">
    <div>
        <asp:RadioButtonList ID="RadioButtonList1" runat="server">
            <asp:ListItem Selected="True" Value="1">Normal</asp:ListItem>
            <asp:ListItem>Crash</asp:ListItem>
            <asp:ListItem Value="0">Crash2</asp:ListItem>
        </asp:RadioButtonList></div>
        <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="Post Data :-)" />
    </form>
</body>
</html>
