<%@ Page Title="" Language="VB" MasterPageFile="~/AccountsMaster.master" AutoEventWireup="false" CodeFile="SalesInvoiceSearch.aspx.vb" Inherits="AccountsModule_SalesInvoiceSearch" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
    
<%@ Register Assembly="EclipseWebSolutions.DatePicker" Namespace="EclipseWebSolutions.DatePicker"
    TagPrefix="ews" %>
<%@ OutputCache location="none" %> 

<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">

<script language ="javascript" type ="text/javascript" >

    var ddlcustcode = null;
    var ddlcustname = null;
    function FillCodeName(ddltp, ddlcode, ddlname, txtcd, txtnm) {
        ddltyp = document.getElementById(ddltp);
        ddlc = document.getElementById(ddlcode);
        ddln = document.getElementById(ddlname);
        txtcscode = document.getElementById(txtcd);
        txtcsname = document.getElementById(txtnm);
        if (ddltyp.value != '[Select]') {
            ddln.value = ddlc.options[ddlc.selectedIndex].text;
            txtcscode.value = ddlc.options[ddlc.selectedIndex].value;
            txtcsname.value = ddlc.options[ddlc.selectedIndex].text;
        }
        else {
            alert('Please Select Type');
            ddlc.value = '[Select]';
            ddln.value = '[Select]';
            txtcscode.value = "";
            txtcsname.value = "";
        }
    }
    function FillNameCode(ddltp, ddlcode, ddlname, txtcd, txtnm) {
        ddltyp = document.getElementById(ddltp);
        ddlc = document.getElementById(ddlcode);
        ddln = document.getElementById(ddlname);
        txtcscode = document.getElementById(txtcd);
        txtcsname = document.getElementById(txtnm);
        if (ddltyp.value != '[Select]') {
            ddlc.value = ddln.options[ddln.selectedIndex].text;
            txtcscode.value = ddln.options[ddln.selectedIndex].text;
            txtcsname.value = ddln.options[ddln.selectedIndex].value;
        }
        else {
            alert('Please Select Type');
            ddlc.value = '[Select]';
            ddln.value = '[Select]';
            txtcscode.value = "";
            txtcsname.value = "";
        }
    }

    function FillCustDDL(ddltp, ddlcustcd, ddlcustnm, lblcustcd, lblcustnm) {
        ddltyp = document.getElementById(ddltp);
        lblcustcode = document.getElementById(lblcustcd);
        lblcustname = document.getElementById(lblcustnm);

        ddlcustcode = document.getElementById(ddlcustcd);
        ddlcustname = document.getElementById(ddlcustnm);

        var strtp = ddltyp.value;
        var strcap = ddltyp.options[ddltyp.selectedIndex].text;
        var sqlstr1 = null;
        var sqlstr2 = null;
        var connstr = document.getElementById("<%=txtconnection.ClientID%>");
        constr = connstr.value


        if (ddltyp.value != '[Select]') {
            lblcustcode.innerHTML = strcap + 'Code';
            lblcustname.innerHTML = strcap + 'Name';
            sqlstr1 = "select Code,des from view_account where type = '" + strtp + "' order by code";
            sqlstr2 = "select des,Code from view_account where type = '" + strtp + "' order by des";
        }
        else {
            lblcustcode.innerHTML = 'Code';
            lblcustname.innerHTML = 'Name';
            sqlstr1 = "select top 10  Code,des from view_account  order by code";
            sqlstr2 = "select top 10 des,Code from view_account  order by des";
        }
        ColServices.clsServices.GetQueryReturnStringListnew(constr, sqlstr1, FillCustCodes, ErrorHandler, TimeOutHandler);
        ColServices.clsServices.GetQueryReturnStringListnew(constr, sqlstr2, FillCustNames, ErrorHandler, TimeOutHandler);

    }

    function FillCustCodes(result) {
        var ddl = ddlcustcode;

        RemoveAll(ddl)
        for (var i = 0; i < result.length; i++) {
            var option = new Option(result[i].ListText, result[i].ListValue);
            ddl.options.add(option);
        }
        ddl.value = "[Select]";
    }

    function FillCustNames(result) {
        var ddl = ddlcustname;

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

    z<table style="border-right: gray 2px solid; border-top: gray 2px solid; border-left: gray 2px solid;
         border-bottom: gray 2px solid">
        <tr>
            <td align="center" class="field_heading" >
                <asp:Label ID="lblHeading" runat="server" CssClass="field_heading" Text=" " Width="698px"></asp:Label></td>
        </tr>
        <tr>
            <td align="center" class="td_cell" style="color: blue;">
                Type few characters of code or name and click search &nbsp; &nbsp;</td>
        </tr>
        <tr>
            <td >
                <asp:UpdatePanel id="UpdatePanel1" runat="server">
                    <contenttemplate>
<TABLE><TBODY><TR><TD><TABLE><TBODY><TR><TD class=" " align=center colSpan=6>&nbsp;<asp:RadioButton id="rbsearch" runat="server" Text="Search" ForeColor="Black" CssClass="td_cell" AutoPostBack="True" Checked="True" GroupName="GrSearch" OnCheckedChanged="rbtnsearch_CheckedChanged" __designer:wfdid="w1"></asp:RadioButton> <asp:RadioButton id="rbtnadsearch" runat="server" Text="Advance Search" ForeColor="Black" CssClass="td_cell" AutoPostBack="True" GroupName="GrSearch" OnCheckedChanged="rbtnadsearch_CheckedChanged" __designer:wfdid="w2"></asp:RadioButton> 
    <asp:Button id="BtnSearch" tabIndex=5 onclick="BtnSearch_Click" runat="server" 
        Text="Search" CssClass="search_button" __designer:wfdid="w3" 
        Font-Bold="True"></asp:Button>&nbsp;
         <asp:Button id="BtnClear" tabIndex=6 
        onclick="BtnClear_Click" runat="server" Text="Clear" 
        CssClass="search_button" __designer:wfdid="w4" Font-Bold="True"></asp:Button>&nbsp;<asp:Button 
        id="btnhelp" tabIndex=10 onclick="btnhelp_Click" runat="server" 
        Text="Help" CssClass="search_button" __designer:wfdid="w7" Font-Bold="True"></asp:Button> &nbsp;
    <asp:Button id="BtnAddNew" tabIndex=7 runat="server" Text="Add New" 
        __designer:dtid="8444249301319694" CssClass="btn" 
        __designer:wfdid="w7"></asp:Button>&nbsp;
         <asp:Button id="BtnPrint" tabIndex=9 
        runat="server" Text="Report" __designer:dtid="8444249301319696" 
        CssClass="btn" __designer:wfdid="w8"></asp:Button></TD></TR><TR><TD><asp:Label id="Label8" runat="server" Text=" Doc. No." ForeColor="Black" Width="110px" CssClass="field_caption"></asp:Label></TD><TD><INPUT style="WIDTH: 194px" id="txtDocNo" class="field_input" tabIndex=1 type=text runat="server" /></TD><TD><asp:Label id="Label2" runat="server" Text="Doc Type" Width="110px" CssClass="field_caption"></asp:Label></TD><TD><SELECT style="WIDTH: 200px" id="ddlDocType" class="field_input" disabled runat="server" Visible="true"> <OPTION value="[Select]" selected>[Select]</OPTION><OPTION value="DN">DN</OPTION><OPTION value="CN">CN</OPTION></SELECT></TD></TR><TR><TD><asp:Label id="Label9" runat="server" Text="From Date" Width="110px" CssClass="field_caption"></asp:Label></TD><TD><ews:datepicker id="dpFromDate" tabIndex=2 runat="server" CssClass="field_input" width="185px" regexerrormessage="Please enter a date in the format: dd/mm/yyyy" dateformatstring="dd/MM/yyyy"></ews:datepicker></TD><TD><asp:Label id="lblTodate" runat="server" Text="To Date" ForeColor="Black" Width="110px" CssClass="field_caption"></asp:Label></TD><TD><ews:datepicker id="dpToDate" tabIndex=3 runat="server" CssClass="field_input" width="185px" regexerrormessage="Please enter a date in the format: dd/mm/yyyy" dateformatstring="dd/MM/yyyy"></ews:datepicker></TD></TR><TR><TD><asp:Label id="Label1" runat="server" Text="Report Option" Width="110px" CssClass="field_caption"></asp:Label></TD><TD><SELECT style="WIDTH: 200px" id="ddlRepoption" class="td_cell" tabIndex=4 runat="server"> <OPTION value="Brief" selected>Brief</OPTION> <OPTION value="Detail">Detail</OPTION> <OPTION value="[Select]">[Select]</OPTION></SELECT> </TD><TD><asp:Label id="Label10" runat="server" Text="Order by " ForeColor="Black" Width="110px" CssClass="field_caption"></asp:Label></TD><TD><asp:DropDownList id="ddlOrderBy" tabIndex=5 runat="server" Width="300px" CssClass="field_input" AutoPostBack="True"><asp:ListItem Value="0">Doc No Desc</asp:ListItem>
<asp:ListItem Value="1">Doc No Asc</asp:ListItem>
<asp:ListItem Value="2">Type</asp:ListItem>
<asp:ListItem Value="3">Code</asp:ListItem>
<asp:ListItem Value="4">Name</asp:ListItem>
<asp:ListItem Value="5">Amount</asp:ListItem>
</asp:DropDownList></TD></TR><TR><TD><asp:Label id="lblType" runat="server" Text="Type" Width="110px" CssClass="field_caption"></asp:Label></TD><TD><SELECT style="WIDTH: 200px" id="ddlType" class="td_cell" tabIndex=6 runat="server"> <OPTION value="[Select]" selected>[Select]</OPTION> <OPTION value="C">Customer</OPTION> <OPTION value="S">Supplier</OPTION> <OPTION value="A">Supplier Agent</OPTION></SELECT></TD><TD><asp:Label id="lblStatus" runat="server" Text="Status" Width="110px" CssClass="field_caption"></asp:Label></TD><TD><SELECT style="WIDTH: 200px" id="ddlStatus" class="td_cell" tabIndex=7 runat="server"> <OPTION value="[Select]" selected>[Select]</OPTION> <OPTION value="P">Posted</OPTION> <OPTION value="U">UnPosted</OPTION>
    <option value="Y">Cancelled</option>
</SELECT></TD></TR><TR><TD><asp:Label id="lblCustCode" runat="server" Text="Code" Width="110px" CssClass="field_caption"></asp:Label></TD><TD><SELECT style="WIDTH: 200px" id="ddlCustomer" class="td_cell" tabIndex=8 runat="server"> <OPTION selected></OPTION></SELECT></TD><TD><asp:Label id="lblCustName" runat="server" Text="Name" Width="110px" CssClass="field_caption"></asp:Label></TD><TD><SELECT style="WIDTH: 300px" id="ddlCustomerName" class="td_cell" tabIndex=9 runat="server"> <OPTION selected></OPTION></SELECT></TD></TR><TR><TD style="HEIGHT: 26px"><asp:Label id="lblFromAmount" runat="server" Text="From Amount" Width="110px" CssClass="field_caption"></asp:Label></TD><TD style="HEIGHT: 26px"><INPUT style="WIDTH: 194px" id="txtFromAmount" class="field_input" tabIndex=10 type=text value=" " runat="server" /></TD><TD style="HEIGHT: 26px"><asp:Label id="lblToAmount" runat="server" Text="To Amount" Width="110px" CssClass="field_caption"></asp:Label></TD><TD><INPUT style="WIDTH: 194px" id="txtToAmount" class="field_input" tabIndex=11 type=text runat="server" /><INPUT style="VISIBILITY: hidden; WIDTH: 5px" id="txtcustcode" type=text maxLength=100 runat="server" /><INPUT style="VISIBILITY: hidden; WIDTH: 5px" id="txtcustname" type=text maxLength=200 runat="server" />
    <input id="txtconnection" runat="server" style="visibility: hidden; width: 12px;
        height: 9px" type="text" /></TD></TR></TBODY></TABLE></TD></TR></TBODY></TABLE>
</contenttemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Button ID="btnExportToExcel" runat="server" CssClass="btn" TabIndex="16"
                    Text="Export To Excel" Width="120px" /></td>
        </tr>
        <tr>
            <td>
                <asp:UpdatePanel id="UpdatePanel2" runat="server">
                    <contenttemplate>
<asp:GridView id="gv_SearchResult" tabIndex=18 runat="server" Font-Size="10px" BackColor="White" Width="950px" CssClass="td_cell" GridLines="Vertical" CellPadding="3" BorderWidth="1px" BorderStyle="None" BorderColor="#999999" AutoGenerateColumns="False" AllowSorting="True" AllowPaging="True">
<FooterStyle BackColor="#6B6B9A" ForeColor="Black"></FooterStyle>
<Columns>
<asp:TemplateField Visible="False" HeaderText="Doc No"><EditItemTemplate>
                                            <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("tran_id") %>'></asp:TextBox>
                                        
</EditItemTemplate>
<ItemTemplate>
                                            <asp:Label ID="lblCode" runat="server" Text='<%# Bind("tran_id") %>'></asp:Label>
                                        
</ItemTemplate>
</asp:TemplateField>
<asp:BoundField DataField="tran_id" SortExpression="tran_id" HeaderText="Doc No"></asp:BoundField>
<asp:BoundField DataField="tran_type" SortExpression="tran_type" HeaderText="Doc Type"></asp:BoundField>
<asp:BoundField DataField="post_state" HeaderText="Status"></asp:BoundField>
<asp:BoundField DataField="acctype" SortExpression="acctype" HeaderText="Type"></asp:BoundField>
<asp:BoundField HtmlEncode="False" DataFormatString="{0:dd/MM/yyyy}" DataField="tran_date" SortExpression="tran_date" HeaderText="Date"></asp:BoundField>
<asp:BoundField DataField="supcode" SortExpression="supcode" HeaderText="Code"></asp:BoundField>
<asp:BoundField DataField="supname" SortExpression="supname" HeaderText="Name"></asp:BoundField>
<asp:BoundField DataField="total" SortExpression="total" HeaderText="Amount"></asp:BoundField>
<asp:BoundField HtmlEncode="False" DataFormatString="{00:dd/MM/yyyy hh:mm:ss tt}" DataField="adddate" SortExpression="adddate" HeaderText="Date Created"></asp:BoundField>
<asp:BoundField DataField="adduser" SortExpression="adduser" HeaderText="User Created"></asp:BoundField>
<asp:BoundField HtmlEncode="False" DataFormatString="{00:dd/MM/yyyy hh:mm:ss tt}" DataField="moddate" SortExpression="moddate" HeaderText="Date Modified"></asp:BoundField>
<asp:BoundField DataField="moduser" SortExpression="moduser" HeaderText="User Modified"></asp:BoundField>
<asp:ButtonField HeaderText="Action" Text="Edit" CommandName="EditRow">
<ItemStyle ForeColor="Blue"></ItemStyle>
</asp:ButtonField>
<asp:ButtonField HeaderText="Action" Text="View" CommandName="View">
<ItemStyle ForeColor="Blue"></ItemStyle>
</asp:ButtonField>
<asp:ButtonField HeaderText="Action" Text="Delete" CommandName="DeleteRow">
<ItemStyle ForeColor="Blue"></ItemStyle>
</asp:ButtonField>
<asp:ButtonField HeaderText="Action" Text="Copy" CommandName="Copy">
<ItemStyle ForeColor="Blue"></ItemStyle>
</asp:ButtonField>

<asp:ButtonField HeaderText="Action" Text="Cancel" CommandName="Cancelrow">
<ItemStyle ForeColor="Blue"></ItemStyle>
</asp:ButtonField>

<asp:ButtonField HeaderText="Action" Text="UndoCancel" CommandName="undoCancel">
<ItemStyle ForeColor="Blue"></ItemStyle>
</asp:ButtonField>


<asp:ButtonField HeaderText="Action" Text="View Log" CommandName="ViewLog">
<ItemStyle ForeColor="Blue"></ItemStyle>
</asp:ButtonField>

</Columns>

<RowStyle  CssClass="grdRowstyle" ForeColor="Black"></RowStyle>

<SelectedRowStyle BackColor="#008A8C" ForeColor="White" Font-Bold="True"></SelectedRowStyle>

<PagerStyle  CssClass="grdpagerstyle" HorizontalAlign="Center"></PagerStyle>

<HeaderStyle CssClass="grdheader"  ForeColor="white" Font-Bold="True"></HeaderStyle>

<AlternatingRowStyle  CssClass="grdAternaterow" Font-Size="10px"></AlternatingRowStyle>
</asp:GridView> <asp:Label id="lblMsg" runat="server" 
                            Text="Records not found, Please redefine search criteria" Font-Size="8pt" 
                            Font-Names="Verdana" Font-Bold="True" Width="357px" CssClass="lblmsg" 
                            Visible="False"></asp:Label> 
</contenttemplate>
                </asp:UpdatePanel></td>
        </tr>
    </table>
<asp:ScriptManagerProxy id="ScriptManagerProxy1" runat="server"><Services>
<asp:ServiceReference Path="~/clsServices.asmx"></asp:ServiceReference>
</Services>
</asp:ScriptManagerProxy> 
</asp:Content>

