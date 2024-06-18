﻿<%@ Page Title="Manage Tickets Recieved Search" Language="VB" MasterPageFile="~/ExcursionMaster.master" AutoEventWireup="false" CodeFile="ManageTicketsReceivedEnterSearch.aspx.vb" Inherits="ExcursionModule_ManageTicketsReceivedEnterSearch" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="EclipseWebSolutions.DatePicker" Namespace="EclipseWebSolutions.DatePicker" TagPrefix="ews" %>
<%@ OutputCache location="none" %> 

<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">

<script language="JavaScript" type="text/javascript" >
window.history.forward(1);  
</script>

<script src="../Content/js/jquery-1.5.1.min.js" type="text/javascript"></script>
<script src="../Content/js/JqueryUI.js" type="text/javascript"></script>
<script src="../Content/js/AutoComplete.js" type="text/javascript"></script>
<link type ="text/css" href ="../Content/css/JqueryUI.css" rel ="Stylesheet" />

<script language="javascript" type="text/javascript">
    function chkTextLock(e) {
        return false;
    }

    function checkNumber(e) {

        if ((event.keyCode < 47 || event.keyCode > 57)) {
            return false;
        }

    }
    function checkCharacter(e) {
        if (event.keyCode == 32 || event.keyCode == 46)
            return;
        if ((event.keyCode < 47 || event.keyCode > 57) && (event.keyCode < 65 || event.keyCode > 91) && (event.keyCode < 96 || event.keyCode > 122)) {
            return false;
        }

    }

    function CallWebMethod(methodType) {
        switch (methodType) {

          
            case "ExGrpCode":
                var select = document.getElementById("<%=ddlExGrpCode.ClientID%>");
                codeid = select.options[select.selectedIndex].text;
                var selectname = document.getElementById("<%=ddlExGrpName.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;

                break;

            case "ExGrpName":
                var select = document.getElementById("<%=ddlExGrpName.ClientID%>");
                codeid = select.options[select.selectedIndex].value;
                var selectname = document.getElementById("<%=ddlExGrpCode.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;

                break;

            case "ExTypeCode":
                var select = document.getElementById("<%=ddlExTypeCode.ClientID%>");
                var selectname = document.getElementById("<%=ddlExTypeName.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;
                break;
            case "ExTypeName":
                var select = document.getElementById("<%=ddlExTypeName.ClientID%>");
                var selectname = document.getElementById("<%=ddlExTypeCode.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;
                break;

        }
    }
  
  
</script>

<table style="border-right: gray 2px solid; border-top: gray 2px solid;border-left: gray 2px solid; border-bottom: gray 2px solid">
<tr>
<td align="center" class="field_heading">
<asp:Label ID="Label1" runat="server" CssClass="field_heading" Height="18px" Text="Manage Tickets Received" Width="440px"></asp:Label></td>
</tr>

<tr>
<td align="center" class="td_cell" style="color: blue">Type few characters of code or name and click search</td>
</tr>

<tr>
<td>
<asp:UpdatePanel id="UpdatePanel1" runat="server">
<contenttemplate>
<TABLE class="td_cell">
<TBODY>
<TR>
<TD  align="center" colSpan=4>
<asp:RadioButton id="rbtnsearch" runat="server" Text="Search" ForeColor="Black" CssClass="td_cell" OnCheckedChanged="rbtnsearch_CheckedChanged" GroupName="GrSearch" Checked="True" AutoPostBack="True"></asp:RadioButton> 
<asp:RadioButton id="rbtnadsearch" runat="server" Text="Advance Search" ForeColor="Black" CssClass="td_cell" OnCheckedChanged="rbtnadsearch_CheckedChanged" GroupName="GrSearch" AutoPostBack="True"></asp:RadioButton>&nbsp; 
<asp:Button id="btnSearch" tabIndex=13 onclick="btnSearch_Click" runat="server" Text="Search" Font-Bold="False" CssClass="search_button"></asp:Button>&nbsp; 
<asp:Button id="btnClear" tabIndex=14 onclick="btnClear_Click" runat="server" Text="Clear" Font-Bold="False" CssClass="search_button"></asp:Button>&nbsp; 
<asp:Button id="cmdhelp" tabIndex=8 onclick="cmdhelp_Click" runat="server" Text="Help" Font-Bold="False" CssClass="search_button"></asp:Button>&nbsp;
<asp:Button id="btnAddNew" tabIndex=15 onclick="btnAddNew_Click" runat="server" Text="Add New" Font-Bold="True" CssClass="field_button"></asp:Button>
</TD>
</TR>

<TR>
<TD style="WIDTH: 60px" class="td_cell">&nbsp;Ticket ID</TD>
<TD style="WIDTH: 51px"><INPUT style="WIDTH: 194px" id="txtAllotmentID" class="field_input" tabIndex=1 type=text maxLength=20 runat="server" /></TD>
<TD>Order By</TD>
<TD style="WIDTH: 305px">
<asp:DropDownList id="ddlOrderBy" runat="server" CssClass="field_input" 
        AutoPostBack="True" TabIndex="2">
<asp:ListItem Value="0">Ticket ID Desc</asp:ListItem>
<asp:ListItem Value="1">Ticket ID Asc</asp:ListItem>
<asp:ListItem Value="2">Excursion Group</asp:ListItem>
<asp:ListItem Value="3">Excursion Type</asp:ListItem>
</asp:DropDownList>
</TD>
</TR>

<TR>
<TD style="WIDTH: 60px; HEIGHT: 22px" class="td_cell">&nbsp;<asp:Label id="Label4" runat="server" Text="Excursion Group Code" Width="111px" CssClass="field_caption"></asp:Label></TD>
<TD style="WIDTH: 51px; HEIGHT: 22px">
<SELECT style="WIDTH: 200px" id="ddlExGrpCode" class="field_input" tabIndex=3 
        onchange="CallWebMethod('ExGrpCode');" runat="server"> <OPTION selected></OPTION></SELECT>
</TD>
<TD style="HEIGHT: 22px" class="td_cell"><asp:Label id="Label5" runat="server" Text="Excursion Group Name" Width="120px" CssClass="field_caption"></asp:Label></TD>
<TD style="WIDTH: 305px; HEIGHT: 22px">
<SELECT style="WIDTH: 300px" id="ddlExGrpName" class="field_input" tabIndex=4 
        onchange="CallWebMethod('ExGrpName');" runat="server"> <OPTION selected></OPTION></SELECT>&nbsp;
</TD>
</TR>

<TR>
<TD style="WIDTH: 107px" class="td_cell"><asp:Label id="Label2" runat="server" Text="Excursion Type Code" Width="111px" CssClass="field_caption"></asp:Label></TD>
<TD style="WIDTH: 208px">
<SELECT style="WIDTH: 200px" id="ddlExTypeCode" class="field_input" tabIndex=5 
        onchange="CallWebMethod('ExTypeCode');" runat="server"> <OPTION selected></OPTION></SELECT>
</TD>
<TD class="td_cell"><asp:Label id="Label3" runat="server" Text="Excursion Type Name" Width="120px" CssClass="field_caption" ToolTip="Excursion Type Code"></asp:Label></TD>
<TD>
<SELECT style="WIDTH: 300px" id="ddlExTypeName" class="field_input" tabIndex=6 
        onchange="CallWebMethod('ExTypeName');" runat="server"> <OPTION selected></OPTION></SELECT>
</TD>
</TR>

<TR>
<TD colSpan=4>
<asp:Panel id="Panel1" runat="server" Height="70px" Width="727px">
<TABLE>
<TBODY>
      
<TR>
<TD style="WIDTH: 107px" class="td_cell">From Ticket Date</TD>
<TD style="WIDTH: 208px">
<ews:DatePicker id="dpFromdate" tabIndex=7 runat="server" Width="185px" CssClass="field_input" RegExErrorMessage="Please enter a date in the format: dd/mm/yyyy" DateFormatString="dd/MM/yyyy"></ews:DatePicker></TD>
<TD class="td_cell">To Ticket Date</TD>
<TD><ews:DatePicker id="dpToDate" tabIndex=8 runat="server" Width="185px" CssClass="field_input" RegExErrorMessage="Please enter a date in the format: dd/mm/yyyy" DateFormatString="dd/MM/yyyy"></ews:DatePicker></TD>
</TR>

<TR>
<TD style="WIDTH: 107px" class="td_cell">From Ticket No</TD>
<TD style="WIDTH: 208px"><asp:TextBox id="txtFromTicketNo" CssClass="field_input" 
        Width="185px" runat="server" TabIndex="9"></asp:TextBox></TD>
<TD class="td_cell">To Ticket No</TD>
<TD><asp:TextBox id="txtToTicketNo" CssClass="field_input" Width="185px" 
        runat="server" TabIndex="10"></asp:TextBox></TD>
</TR>

<TR style="display:none">
<TD style="WIDTH: 107px" class="td_cell">Assigned To</TD>
<TD style="WIDTH: 208px">
<input type="text" name="accSearch"  class="field_input MyAutoCompleteClass" style="width:195px" id="accSearch"  runat="server" />
<SELECT style="WIDTH: 200px" id="ddlCustomer" class="field_input MyDropDownListCustValue"  tabIndex=11 runat="server"> <OPTION selected></OPTION></SELECT>
 </TD>
</TR>


</TBODY>
</TABLE>
</asp:Panel>
</TD>
</TR>
</TBODY>
</TABLE>&nbsp;&nbsp; 
</contenttemplate>
</asp:UpdatePanel></td>
</tr>

<tr>
<td>
<asp:Button ID="btnExportToExcel" runat="server" CssClass="field_button" TabIndex="9" Text="Export To Excel" />&nbsp;
<asp:Button ID="btnPrint" runat="server" CssClass="field_button" TabIndex="17" Text="Report"  /></td>
</tr>

<tr>
<td class=" ">
<asp:UpdatePanel id="UpdatePanel2" runat="server">
<contenttemplate>
<asp:GridView id="gv_SearchResult" tabIndex=10 runat="server" Font-Size="10px" BackColor="White" 
Width="900px" CssClass="td_cell" GridLines="Vertical" CellPadding="3" 
BorderWidth="1px" BorderStyle="None" BorderColor="#999999" 
AutoGenerateColumns="False" AllowSorting="False" AllowPaging="True">


<Columns>
<asp:TemplateField Visible="False" HeaderText="Allotment ID">
<ItemTemplate>
<asp:Label id="lblticketid" runat="server" Text='<%# Bind("ticketid") %>'></asp:Label> 
</ItemTemplate>
<ItemStyle VerticalAlign="Top" />
</asp:TemplateField>

<asp:BoundField DataField="ticketid" SortExpression="ticketid" HeaderText="Ticket ID">
<ItemStyle HorizontalAlign="Left" VerticalAlign="Top"></ItemStyle>
<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
</asp:BoundField>

<asp:BoundField HtmlEncode="False" DataFormatString="{00:dd/MM/yyyy hh:mm:ss tt}"   DataField="datereceived"  HeaderText="Date Received">
<ItemStyle HorizontalAlign="Left" VerticalAlign="Top"></ItemStyle>
<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
</asp:BoundField>

<asp:BoundField DataField="othgrpname" SortExpression="othgrpname" HeaderText="Excursion Group">
<ItemStyle HorizontalAlign="Left" VerticalAlign="Top"></ItemStyle>
</asp:BoundField>

<asp:BoundField DataField="othtypname" SortExpression="othtypname" HeaderText="Excursion Type">
<ItemStyle HorizontalAlign="Left" VerticalAlign="Top"></ItemStyle>
<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
</asp:BoundField>



<asp:BoundField HtmlEncode="False" DataFormatString="{00:dd/MM/yyyy hh:mm:ss tt}" DataField="fromticketno" SortExpression="fromticketno" HeaderText="From Ticket">
<ItemStyle HorizontalAlign="Left" VerticalAlign="Top"></ItemStyle>
<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
</asp:BoundField>

<asp:BoundField HtmlEncode="False" DataFormatString="{00:dd/MM/yyyy hh:mm:ss tt}" DataField="toticketno" SortExpression="toticketno" HeaderText="To Ticket">
<ItemStyle HorizontalAlign="Left" VerticalAlign="Top"></ItemStyle>
<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
</asp:BoundField>

<asp:BoundField HtmlEncode="False" DataFormatString="{00:dd/MM/yyyy}" DataField="ticketdate" SortExpression="ticketdate" HeaderText="Ticket Date">
<ItemStyle HorizontalAlign="Left" VerticalAlign="Top"></ItemStyle>
<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
</asp:BoundField>

<asp:BoundField HtmlEncode="False" DataFormatString="{00:dd/MM/yyyy hh:mm:ss tt}" DataField="adddate" SortExpression="adddate" HeaderText="Date Created">
<ItemStyle HorizontalAlign="Left" VerticalAlign="Top"></ItemStyle>
</asp:BoundField>

<asp:BoundField DataField="adduser" SortExpression="adduser" HeaderText="User Created">
<ItemStyle HorizontalAlign="Left" VerticalAlign="Top"></ItemStyle>
</asp:BoundField>



<asp:BoundField HtmlEncode="False" DataFormatString="{00:dd/MM/yyyy hh:mm:ss tt}" DataField="moddate" SortExpression="moddate" HeaderText="Date Modified">
<ItemStyle HorizontalAlign="Left" VerticalAlign="Top"></ItemStyle>
</asp:BoundField>

<asp:BoundField DataField="moduser" SortExpression="moduser" HeaderText="User Modified">
<ItemStyle HorizontalAlign="Left" VerticalAlign="Top"></ItemStyle>
</asp:BoundField>



<asp:ButtonField HeaderText="Action" Text="Edit" CommandName="EditRow">
<ItemStyle ForeColor="Blue" HorizontalAlign="Center" VerticalAlign="Top"></ItemStyle>
<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
</asp:ButtonField>

<asp:ButtonField HeaderText="Action" Text="Delete" CommandName="DeleteRow">
<ItemStyle ForeColor="Blue" HorizontalAlign="Center" VerticalAlign="Top"></ItemStyle>
<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
</asp:ButtonField>

<asp:ButtonField HeaderText="Action" Text="View" CommandName="View">
<ItemStyle ForeColor="Blue" HorizontalAlign="Center" VerticalAlign="Top"></ItemStyle>
<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
</asp:ButtonField>

<asp:ButtonField HeaderText="Action" Text="Assign" CommandName="Assign">
<ItemStyle ForeColor="Blue" HorizontalAlign="Center" VerticalAlign="Top"></ItemStyle>
<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
</asp:ButtonField>

<asp:ButtonField HeaderText="Action" Text="Transfer" CommandName="Transfer">
<ItemStyle ForeColor="Blue" HorizontalAlign="Center" VerticalAlign="Top"></ItemStyle>
<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
</asp:ButtonField>



</Columns>
<RowStyle CssClass="grdRowstyle"  ForeColor="Black"></RowStyle>
<SelectedRowStyle BackColor="#008A8C" ForeColor="White" Font-Bold="True"></SelectedRowStyle>
<PagerStyle CssClass="grdpagerstyle"  ForeColor="Black" HorizontalAlign="Center"></PagerStyle>
<HeaderStyle CssClass="grdheader"  ForeColor="White" Font-Bold="True"></HeaderStyle>
<AlternatingRowStyle CssClass="grdAternaterow"  Font-Size="10px"></AlternatingRowStyle>
<FooterStyle CssClass="grdfooter"  ForeColor="Black"></FooterStyle>
</asp:GridView> 

<asp:Label id="lblMsg" runat="server" Text="Records not found. Please redefine search criteria" Font-Size="8pt" Font-Names="Verdana" Font-Bold="True" Width="357px" Visible="False" CssClass="lblmsg"></asp:Label> 
</contenttemplate>
</asp:UpdatePanel>
</td>
</tr>

</table>

<script type="text/javascript">
    $(document).ready(function () {
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_initializeRequest(InitializeRequest);
        prm.add_endRequest(EndRequest);

        // Place here the first init of the autocomplete
        MyAutoCustomer();
    });

    function InitializeRequest(sender, args) {

    }

    function EndRequest(sender, args) {
        // after update occur on UpdatePanel re-init the Autocomplete
        MyAutoCustomer();
    }
 </script>

</asp:Content>


