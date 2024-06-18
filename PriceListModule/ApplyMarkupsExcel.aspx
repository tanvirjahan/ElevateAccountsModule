<%@ Page Language="VB" AutoEventWireup="false" CodeFile="ApplyMarkupsExcel.aspx.vb" Inherits="PriceListModule_ApplyMarkupsExcel" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
    <%@ OutputCache location="none" %> 

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>View Markups</title>

    <meta http-equiv="X-UA-Compatible" content="IE=EmulateIE7" />
   
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>

    <script language="javascript" type="text/javascript">
        window.moveTo(0, 0)
      
    </script>

    <style type="text/css">
        .grdstyle1
        {
            font-family: Arial,Verdana, Geneva, ms sans serif;
            font-size: 15px;
            background: White;
            color: #000000;
        }
        
        .ModalPopupBG
        {
            background-color: gray;
            filter: alpha(opacity=50);
            opacity: 0.7;
        }
        
        .ErrorMsg 
        {
	        font-family: Verdana, Arial, Geneva, ms sans serif;
	        font-size: 9pt;
	        font-weight:bold;
	        font-style: normal;
	        font-weight: normal;
	        color :Red ;
        }
        .grdstyle
        {
	        border: 1px none #999999;
	        font-family: Arial,Verdana, Geneva, ms sans serif;
	        font-size :15px;
	        background :White;
	        color:#DDD9CF;
	        font-weight: normal;
	        margin-top: 0px;
        }
        .grdheader
        {
	        font-family: Arial,Verdana, Geneva, ms sans serif;
	        font-size :15px;
	        font-weight:bold;
	        background-color:#5B9BD5; 	
	        color :White;

        }
        .grdheader a:link {color :#800040;}
        .grdheader a:active {color :#800040;}
        .grdheader a:visited {color :#800040;}
        .grdheader a:hover {color :#800040;}
        .grdfooter
        {
	        font-family: Arial,Verdana, Geneva, ms sans serif;
	        font-size :15px;
	        background :#DDD9CF;
	        color :#06788B ;
        }
        .grdfooter a:link {color :#800040;}
        .grdfooter a:active {color :#800040;}
        .grdfooter a:visited {color :#800040;}
        .grdfooter a:hover {color :#800040;}

        .grdAternaterow
        {
	          font-family: Arial,Verdana, Geneva, ms sans serif;
	        background:#DEEBF6;
	        font-weight:normal;
	        color :black;
	        Font-Size:15px;
	
        }
        .grdRowstyle
        {
	        font-family: Arial,Verdana, Geneva, ms sans serif;
	        background:white;
	        font-weight:normal;
	        color :black;
	        Font-Size:15px;
        }
        .grdselectrowstyle
        {
	        font-family: Arial,Verdana, Geneva, ms sans serif;
	        background:#008A8C;
	        color :White ;
	        Font-Size:15px;
	        font-weight:bold;
        }
        .grdpagerstyle
        {
	        font-family: Arial,Verdana, Geneva, ms sans serif;
	        background:white;
	        color :Black;
	        Font-Size:15px;
        }
        .search_button
        {
	        border: 1px solid #280000;
	        font-family: Arial;
	        font-size: 8pt;
	        font-weight: bold;
	        font-style: normal;
	        font-weight: normal;
	        font-variant: normal;
	        background-color: white;
	        color:#06788B;
	    }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        <asp:GridView ID="gvSearch" runat="server" AutoGenerateColumns="False" BackColor="White"
            BorderColor="#999999" BorderStyle="None" BorderWidth="1px" CellPadding="3" CssClass="td_cell" AllowPaging="true"
                Font-Size="10px" GridLines="Vertical" TabIndex="12" Width="100%">
            <FooterStyle CssClass="grdfooter" />
            <Columns>
                <asp:TemplateField HeaderText="ApplyMarkup Id">
                    <ItemTemplate>
                        <asp:Label ID="lblApplyMarkupId" Text='<%# Bind("ApplyMarkupId") %>' runat="server"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField HeaderText="Applicable To " DataField="ApplicableTo" />
                <asp:BoundField HeaderText="Inventory Types" DataField="InventoryTypes" />
                <asp:BoundField HeaderText="Days Of the Week" DataField="DaysOfTheWeek" />
                <asp:BoundField HeaderText="Hotel Name" DataField="partyname" />
                <asp:BoundField HeaderText="Room Classification" DataField="RoomClassName" />
                <asp:BoundField HeaderText="MarkUp From Date"   DataFormatString="{0:dd/MM/yyyy}" DataField="MarkUpFromDate" />
                    <asp:BoundField HeaderText="MarkUp To Date" DataFormatString="{0:dd/MM/yyyy}"  DataField="MarkUpToDate" />
                <asp:BoundField HeaderText="Formula Id" DataField="FormulaId" />
                <asp:BoundField HeaderText="Formula String" DataField="FormulaString" />
                             
            </Columns>

            <RowStyle CssClass="grdRowstyle" ForeColor="Black"></RowStyle>
            <HeaderStyle CssClass="grdheader" ForeColor="white" Font-Bold="True"></HeaderStyle>
            <AlternatingRowStyle CssClass="grdAternaterow"></AlternatingRowStyle>

            <%--<RowStyle CssClass="grdRowstyle"></RowStyle>
            <SelectedRowStyle CssClass="grdselectrowstyle"></SelectedRowStyle>
            <PagerStyle CssClass="grdpagerstyle" HorizontalAlign="Center"></PagerStyle>
            <HeaderStyle CssClass="grdheader" ForeColor="white"></HeaderStyle>
            <AlternatingRowStyle CssClass="grdAternaterow"></AlternatingRowStyle>--%>
        </asp:GridView>

    </div>
    </form>
</body>
</html>
