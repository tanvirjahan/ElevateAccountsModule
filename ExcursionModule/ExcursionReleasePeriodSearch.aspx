<%@ Page Title="Excursion Release Period" Language="VB" MasterPageFile="~/ExcursionMaster.master" AutoEventWireup="false" CodeFile="ExcursionReleasePeriodSearch.aspx.vb" Inherits="ExcursionModule_ExcursionReleasePeriodSearch" %>


<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
      <%@ Register Assembly="EclipseWebSolutions.DatePicker" Namespace="EclipseWebSolutions.DatePicker"
    TagPrefix="ews" %>
    <%@ OutputCache location="none" %> 
<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">
    <script language="JavaScript" type="text/javascript" >
        window.history.forward(1);  
</script>
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

            case "marketcode":
                var select = document.getElementById("<%=ddlMarketCD.ClientID%>");
                var codeid = select.options[select.selectedIndex].text;
                var selectname = document.getElementById("<%=ddlMarketNM.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;
                break;
            case "marketname":
                var select = document.getElementById("<%=ddlMarketNM.ClientID%>");
                var codeid = select.options[select.selectedIndex].text;
                var selectname = document.getElementById("<%=ddlMarketCD.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;
                break;

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
    function FillMarketCode(result) {
        var ddl = document.getElementById("<%=ddlMarketCD.ClientID%>");
        RemoveAll(ddl)
        for (var i = 0; i < result.length; i++) {
            var option = new Option(result[i].ListText, result[i].ListValue);
            ddl.options.add(option);
        }
        ddl.value = "[Select]";
    }
    function FillMarketName(result) {
        var ddl = document.getElementById("<%=ddlMarketNM.ClientID%>");
        RemoveAll(ddl)
        for (var i = 0; i < result.length; i++) {
            var option = new Option(result[i].ListText, result[i].ListValue);
            ddl.options.add(option);
        }
        ddl.value = "[Select]";
    }

    function TimeOutHandler(result) {
        alert("Timeout :" + result);
    }

    function ErrorHandler(result) {
        var msg = result.get_exceptionType() + "\r\n";
        msg += result.get_message() + "\r\n";
        msg += result.get_stackTrace();
        alert(msg);
    }
</script>

    <table style="border-right: gray 2px solid; border-top: gray 2px solid;
        border-left: gray 2px solid; border-bottom: gray 2px solid">
        <tr>
            <td align="center" class="field_heading">
                <asp:Label ID="Label1" runat="server" CssClass="field_heading" Height="18px" Text="Excursion - Release Period"
                    Width="100%"></asp:Label></td>
        </tr>
        <tr>
            <td align="center" class="td_cell" style="color: blue">
                Type few characters of code or name and click search
            </td>
        </tr>
        <tr>
            <td>
                <asp:UpdatePanel id="UpdatePanel1" runat="server">
                    <contenttemplate>
<TABLE class="td_cell"><TBODY><TR><TD class=" " align=center colSpan=4><asp:RadioButton id="rbtnsearch" runat="server" Text="Search" ForeColor="Black" CssClass="td_cell" OnCheckedChanged="rbtnsearch_CheckedChanged" GroupName="GrSearch" Checked="True" AutoPostBack="True"></asp:RadioButton> <asp:RadioButton id="rbtnadsearch" runat="server" Text="Advance Search" ForeColor="Black" CssClass="td_cell" OnCheckedChanged="rbtnadsearch_CheckedChanged" GroupName="GrSearch" AutoPostBack="True"></asp:RadioButton>&nbsp; 
    <asp:Button id="btnSearch" tabIndex=13 onclick="btnSearch_Click" runat="server" 
        Text="Search" Font-Bold="False" CssClass="search_button"></asp:Button>&nbsp; 
    <asp:Button id="btnClear" tabIndex=14 onclick="btnClear_Click" runat="server" 
        Text="Clear" Font-Bold="False" CssClass="search_button"></asp:Button>&nbsp; 
    <asp:Button id="cmdhelp" tabIndex=8 onclick="cmdhelp_Click" runat="server" 
        Text="Help" Font-Bold="False" CssClass="search_button"></asp:Button>&nbsp;
<asp:Button id="btnAddNew" tabIndex=15 onclick="btnAddNew_Click" runat="server" 
        Text="Add New" Font-Bold="True" CssClass="field_button"></asp:Button></TD></TR><TR><TD style="WIDTH: 60px" class="td_cell">
        &nbsp;Allot ID</TD><TD style="WIDTH: 51px"><INPUT style="WIDTH: 194px" id="txtAllotmentID" class="field_input" tabIndex=1 type=text maxLength=20 runat="server" /></TD><TD>Order By</TD><TD style="WIDTH: 305px"><asp:DropDownList id="ddlOrderBy" runat="server" CssClass="field_input" AutoPostBack="True"><asp:ListItem Value="0">Allot Id Desc</asp:ListItem>
<asp:ListItem Value="1">Allot Id Asc</asp:ListItem>
<asp:ListItem Value="2">Excursion Group</asp:ListItem>
<asp:ListItem Value="3">Excursion Type</asp:ListItem>

</asp:DropDownList></TD></TR>

<TR><TD style="WIDTH: 60px; HEIGHT: 22px" class="td_cell">&nbsp;<asp:Label 
            id="Label4" runat="server" Text="Excursion Group Code" Width="111px" 
            CssClass="td_cell"></asp:Label></TD><TD style="WIDTH: 51px; HEIGHT: 22px">
            <SELECT style="WIDTH: 200px" id="ddlExGrpCode" class="field_input" tabIndex=1 
                onchange="CallWebMethod('ExGrpCode');" runat="server"> <OPTION selected></OPTION></SELECT></TD><TD style="HEIGHT: 22px" class="td_cell">
        <asp:Label id="Label5" runat="server" Text="Excursion Group Name" Width="120px" 
            CssClass="td_cell"></asp:Label></TD><TD style="WIDTH: 305px; HEIGHT: 22px">
            <SELECT style="WIDTH: 300px" id="ddlExGrpName" class="field_input" tabIndex=2 
                onchange="CallWebMethod('ExGrpName');" runat="server"> <OPTION selected></OPTION></SELECT>&nbsp;</TD></TR><TR><TD colSpan=4>
    <asp:Panel id="Panel1" runat="server" Height="130px" Width="727px"><TABLE><TBODY><TR><TD style="WIDTH: 107px" class="td_cell">
        <asp:Label id="Label2" runat="server" Text="Excursion Type Code" Width="111px" 
            CssClass="td_cell"></asp:Label></TD><TD style="WIDTH: 208px">
            <SELECT style="WIDTH: 200px" id="ddlExTypeCode" class="field_input" tabIndex=3 
                onchange="CallWebMethod('ExTypeCode');" runat="server"> <OPTION selected></OPTION></SELECT></TD><TD class="td_cell">
        <asp:Label id="Label3" runat="server" Text="Excursion Type Name" Width="120px" 
            CssClass="td_cell" ToolTip="Excursion Type Code"></asp:Label></TD><TD>
            <SELECT style="WIDTH: 300px" id="ddlExTypeName" class="field_input" tabIndex=4 
                onchange="CallWebMethod('ExTypeName');" runat="server"> <OPTION selected></OPTION></SELECT></TD></TR><TR><TD style="WIDTH: 107px; HEIGHT: 22px" class="td_cell">Market Code</TD><TD style="WIDTH: 208px; HEIGHT: 22px">
        <SELECT style="WIDTH: 200px" id="ddlMarketCD" class="field_input" tabIndex=5 
            onchange="CallWebMethod('marketcode');" runat="server"> <OPTION selected></OPTION></SELECT></TD><TD class="td_cell">Market Name</TD><TD style="HEIGHT: 22px">
        <SELECT style="WIDTH: 300px" id="ddlMarketNM" class="field_input" tabIndex=6 
            onchange="CallWebMethod('marketname');" runat="server"> <OPTION selected></OPTION></SELECT></TD></TR>
           
        
        <TR><TD style="WIDTH: 107px" class="td_cell">From Date</TD><TD style="WIDTH: 208px">
            <ews:DatePicker id="dpFromdate" tabIndex=7 runat="server" Width="185px" 
                CssClass="field_input" 
                RegExErrorMessage="Please enter a date in the format: dd/mm/yyyy" 
                DateFormatString="dd/MM/yyyy"></ews:DatePicker></TD><TD class="td_cell">To Date</TD><TD>
            <ews:DatePicker id="dpToDate" tabIndex=8 runat="server" Width="185px" 
                CssClass="field_input" 
                RegExErrorMessage="Please enter a date in the format: dd/mm/yyyy" 
                DateFormatString="dd/MM/yyyy"></ews:DatePicker></TD></TR>
                <tr>
                <td>Approve Status</td>

                <td>
                <asp:DropDownList ID="DDLstatus" runat="server" CssClass="field_input" Width="120px">
                <asp:ListItem Value="0">All</asp:ListItem>
                <asp:ListItem Value="1">Unapprove</asp:ListItem>
                <asp:ListItem Value="2">Approve</asp:ListItem>
            </asp:DropDownList>
                </td>
                </tr>
                
                
                <TR><TD style="WIDTH: 119px" class="td_cell">
    <input id="txtconnection" runat="server" style="visibility: hidden; width: 12px;
        height: 9px" type="text" /></TD><TD></TD></TR></TBODY></TABLE></asp:Panel></TD></TR></TBODY></TABLE>&nbsp;&nbsp; 
</contenttemplate>
                </asp:UpdatePanel></td>
        </tr>
        <tr>
            <td>
                <asp:Button ID="btnExportToExcel" runat="server" CssClass="field_button" TabIndex="9"
                    Text="Export To Excel" />&nbsp;
                <asp:Button ID="btnPrint" runat="server" CssClass="field_button" TabIndex="17" Text="Report"
                    Visible="False" /></td>
        </tr>
        <tr>
            <td class=" ">
                <asp:UpdatePanel id="UpdatePanel2" runat="server">
                    <contenttemplate>
<asp:GridView id="gv_SearchResult" tabIndex=10 runat="server" Font-Size="10px" BackColor="White" 
                            Width="100%" CssClass="td_cell" GridLines="Vertical" CellPadding="3" 
                            BorderWidth="1px" BorderStyle="None" BorderColor="#999999" 
                            AutoGenerateColumns="False" AllowSorting="True" AllowPaging="True">
<FooterStyle CssClass="grdfooter"  ForeColor="Black"></FooterStyle>
<Columns>
<asp:TemplateField Visible="False" HeaderText="Allotment ID"><EditItemTemplate>
                                            
                                        
</EditItemTemplate>
<ItemTemplate>
<asp:Label id="lblmstopid" runat="server" Text='<%# Bind("allotid") %>'></asp:Label> 
</ItemTemplate>
    <ItemStyle VerticalAlign="Top" />
</asp:TemplateField>
<asp:BoundField DataField="allotid" SortExpression="allotid" HeaderText="Allot ID">
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
<asp:BoundField DataField="plgrpcode" SortExpression="plgrpcode" HeaderText="Market Code">
<ItemStyle HorizontalAlign="Left" VerticalAlign="Top"></ItemStyle>

<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
</asp:BoundField>


<asp:BoundField HtmlEncode="False" DataFormatString="{00:dd/MM/yyyy}" DataField="frmdatec" SortExpression="frmdate" HeaderText="From Date">
<ItemStyle HorizontalAlign="Left" VerticalAlign="Top"></ItemStyle>

<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
</asp:BoundField>
<asp:BoundField HtmlEncode="False" DataFormatString="{00:dd/MM/yyyy}" DataField="todatec" SortExpression="todate" HeaderText="To Date">
<ItemStyle HorizontalAlign="Left" VerticalAlign="Top"></ItemStyle>

<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
</asp:BoundField>

<asp:BoundField HtmlEncode="False" DataFormatString="{00:dd/MM/yyyy hh:mm:ss tt}" DataField="adddate" SortExpression="adddate" HeaderText="Date Created">
<ItemStyle HorizontalAlign="Left" VerticalAlign="Top"></ItemStyle>
</asp:BoundField>
<asp:BoundField DataField="adduser" SortExpression="adduser" HeaderText="User Created">
<ItemStyle HorizontalAlign="Left" VerticalAlign="Top"></ItemStyle>
</asp:BoundField>

<asp:BoundField HtmlEncode="False" DataFormatString="{00:dd/MM/yyyy hh:mm:ss tt}" DataField="moddate" SortExpression="modate" HeaderText="Date Modified">
<ItemStyle HorizontalAlign="Left" VerticalAlign="Top"></ItemStyle>
</asp:BoundField>
<asp:BoundField DataField="moduser" SortExpression="moduser" HeaderText="User Modified">
<ItemStyle HorizontalAlign="Left" VerticalAlign="Top"></ItemStyle>
</asp:BoundField>


<asp:BoundField DataField="approve" SortExpression="approve" HeaderText="Approved">
<ItemStyle HorizontalAlign="Left" VerticalAlign="Top"></ItemStyle>
</asp:BoundField>
<asp:ButtonField HeaderText="Action" Text="View" CommandName="View">
<ItemStyle ForeColor="Blue" HorizontalAlign="Center" VerticalAlign="Top"></ItemStyle>

<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
</asp:ButtonField>

<asp:ButtonField HeaderText="Action" Text="Edit" CommandName="Editrow">
<ItemStyle ForeColor="Blue" HorizontalAlign="Center" VerticalAlign="Top"></ItemStyle>

<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
</asp:ButtonField>

<asp:ButtonField HeaderText="Action" Text="Delete" CommandName="Deleterow">
<ItemStyle ForeColor="Blue" HorizontalAlign="Center" VerticalAlign="Top"></ItemStyle>

<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
</asp:ButtonField>

</Columns>

<RowStyle CssClass="grdRowstyle"  ForeColor="Black"></RowStyle>

<SelectedRowStyle BackColor="#008A8C" ForeColor="White" Font-Bold="True"></SelectedRowStyle>

<PagerStyle CssClass="grdpagerstyle"  ForeColor="Black" HorizontalAlign="Center"></PagerStyle>

<HeaderStyle CssClass="grdheader"  ForeColor="White" Font-Bold="True"></HeaderStyle>

<AlternatingRowStyle CssClass="grdAternaterow"  Font-Size="10px"></AlternatingRowStyle>
</asp:GridView> <asp:Label id="lblMsg" runat="server" 
                            Text="Records not found. Please redefine search criteria" Font-Size="8pt" 
                            Font-Names="Verdana" Font-Bold="True" Width="357px" Visible="False" 
                            CssClass="lblmsg"></asp:Label> 
</contenttemplate>
                </asp:UpdatePanel></td>
        </tr>
    </table>
    <asp:ScriptManagerProxy id="ScriptManagerProxy1" runat="server"><Services>
<asp:ServiceReference Path="~/clsServices.asmx"></asp:ServiceReference>
</Services>
</asp:ScriptManagerProxy> 

</asp:Content>








