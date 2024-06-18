<%@ Control Language="VB" AutoEventWireup="false" CodeFile="SubMenuUserControl.ascx.vb" Inherits="SubMenuUserControl" %>
  <link rel="stylesheet" href="../CSS/Styles.css" type="text/css" />
     <script language="javascript" src="../js/date-picker.js" type="text/javascript"></script>
	 <script language="javascript" src="../js/datefun.js" type="text/javascript"></script>
<table>
    <tr>
        <td align="left">
            <asp:Menu ID="Menu1" runat="server"  CssClass="MenuStyle" BackColor="#DDD9CF" Font-Bold="True" Font-Names="Verdana"
                Font-Size="Small" ForeColor="#06788B" Height="80px" 
                DynamicHorizontalOffset="4" StaticSubMenuIndent="10px">
                <DynamicHoverStyle BackColor="#06788B" ForeColor="White" />
                <DynamicMenuStyle  CssClass="MenuStyle"  BackColor="#FFFBD6" HorizontalPadding="3px" VerticalPadding="3px" />
                <DynamicMenuItemStyle  CssClass="MenuItemStyle" BackColor="#E5E1DA" ForeColor="Black" HorizontalPadding="5px"
                    VerticalPadding="2px" />
                <DynamicSelectedStyle BackColor="#FFCC66" HorizontalPadding="3px" VerticalPadding="3px" />
                <StaticMenuItemStyle HorizontalPadding="5px" VerticalPadding="2px" />
                <StaticSelectedStyle BackColor="#06788B"  ForeColor="White"  />
                <StaticHoverStyle BackColor="#06788B" ForeColor="White" />
            </asp:Menu>
        </td>
    </tr>
</table>
