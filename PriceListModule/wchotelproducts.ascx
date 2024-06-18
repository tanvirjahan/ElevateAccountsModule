<%@ Control Language="VB" AutoEventWireup="false" CodeFile="wchotelproducts.ascx.vb" Inherits="wchotelproducts"  %>
  <link rel="stylesheet" href="../CSS/Styles.css" type="text/css" />
     <script language="javascript" src="../js/date-picker.js" type="text/javascript"></script>
	 <script language="javascript" src="../js/datefun.js" type="text/javascript"></script>
    


     <style>
 .menuTabs
        {
            position:relative;
            top:1px;
            left:10px;
        }
        .tab
        {
           
            border:Solid 1px white;
            border-bottom:none;
            padding: 5px 10px 5px 10px;
            background-color:#2fa4e7;
            color :White ;
            
            
        }
      
        .selectedTab
        {
            border:Solid 1px black;
            border-bottom:Solid 1px white;
            padding:0px 10px;
            background-color:white;
            
        }
        .tabBody
        {
            border:Solid 1px black;
            padding:20px;
            background-color:white;
        }

     </style>
    
    <style>
         .btnnew 
         {
         	border-top-color: #c0ced6;
             background: #c0ced6;
             color: #5b98a3;
         }
    </style>
    
<table >
    <tr>
        <td>
            <asp:Menu ID="Menu1" runat="server"  CssClass="menuTabs" EnableViewState="true"  Font-Bold="True" Font-Names="Verdana"
                Font-Size="10pt"   
                DynamicHorizontalOffset="4" StaticSubMenuIndent="10px" Orientation ="Horizontal" BackColor="white" ForeColor ="black" >
               
                <DynamicMenuStyle  CssClass="MenuItemStyle"  BackColor="#FFFBD6" HorizontalPadding="3px" VerticalPadding="3px" />
                <DynamicMenuItemStyle  CssClass="MenuItemStyle"  HorizontalPadding="5px"
                    VerticalPadding="2px" />
                <DynamicSelectedStyle BackColor="#FFCC66" HorizontalPadding="3px" VerticalPadding="3px" />
                <StaticMenuItemStyle  HorizontalPadding="5px" VerticalPadding="2px" Cssclass="tab" />
                <StaticSelectedStyle BackColor="#c0ced6" Cssclass="selectedTab" />
                
                <StaticHoverStyle BackColor="#06788B" ForeColor="White" />
            </asp:Menu>
        </td>
    </tr>
</table>

 