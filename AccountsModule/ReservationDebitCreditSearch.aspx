<%@ Page Language="VB" MasterPageFile="~/AccountsMaster.master" AutoEventWireup="false" CodeFile="ReservationDebitCreditSearch.aspx.vb" Inherits="AccountsModule_ReservationDebitCreditSearch"%>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="EclipseWebSolutions.DatePicker" Namespace="EclipseWebSolutions.DatePicker"   TagPrefix="ews" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="server" >
<script language="javascript" type="text/javascript">
function ChangeDate()
{
    var txtfdate=document.getElementById("<%=txtFromDate.ClientID%>");
    if (txtfdate.value=='')
    {
        alert("Enter From Date.");
        txtfdate.focus();  
    }
    else
    {
        ColServices.clsServices.GetQueryReturnFromToDate('FromDate',30,txtfdate.value,FillToDate,ErrorHandler,TimeOutHandler);
    }
}
function FillToDate(result)
{
    var txttdate=document.getElementById("<%=txtToDate.ClientID%>");
    txttdate.value=result;
}
function TimeOutHandler(result)
{
    alert("Timeout :" + result);
}
function ErrorHandler(result)
{
    var msg=result.get_exceptionType() + "\r\n";
    msg += result.get_message() + "\r\n";
    msg += result.get_stackTrace();
    alert(msg);
}
function ValidateForm()
{
    var txtfdate=document.getElementById("<%=txtFromDate.ClientID%>");
    var txttdate=document.getElementById("<%=txtToDate.ClientID%>");
    if (txtfdate.value=='')
    {
        alert("Enter From Date.");
        txtfdate.focus();
    }
    else if (txttdate.value=='')
    {
        alert("Enter To Date.");
        txttdate.focus();
    }
    else if(txtfdate.value > txttdate.value)
    {
        alert("To date should be greater than from dat.");
        txttdate.focus();
    }
}
</script>
    <table style="border-right: gray 1px solid; border-top: gray 1px solid; border-left: gray 1px solid; border-bottom: gray 1px solid" id="TABLE1">
        <tr>
            <td style="height: 2px">
    <asp:UpdatePanel id="UpdatePanel1" runat="server">
        <contenttemplate>
<TABLE class="td_cell" width=950><TBODY><TR><TD style="TEXT-ALIGN: center" class="field_heading" colSpan=4>Reservation Credit Note List</TD></TR><TR><TD align=center colSpan=4><asp:RadioButton id="rbsearch" tabIndex=1 runat="server" Text="Search" ForeColor="Black" Width="56px" CssClass="td_cell" OnCheckedChanged="rbsearch_CheckedChanged" Checked="True" GroupName="GrSearch" AutoPostBack="True"></asp:RadioButton>
    <asp:RadioButton id="rbnadsearch" tabIndex=2 runat="server" 
        Text="Advance Search" ForeColor="Black" Width="137px" CssClass="td_cell" 
        OnCheckedChanged="rbnadsearch_CheckedChanged" GroupName="GrSearch" 
        AutoPostBack="True" wfdid="w6"></asp:RadioButton><asp:Button id="btnSearch" 
        tabIndex=12 runat="server" Text="Search" Font-Bold="False" 
        CssClass="search_button"></asp:Button>&nbsp;<asp:Button id="btnClear" 
        tabIndex=13 runat="server" Text="Clear" Font-Bold="False" 
        CssClass="search_button"></asp:Button>&nbsp;<asp:Button id="btnHelp" tabIndex=14 onclick="btnHelp_Click" runat="server" Text="Help" CssClass="search_button"></asp:Button> 
   &nbsp; 
    <asp:Button id="btnAddNew" tabIndex=15 runat="server" Text="Add New" 
        Font-Bold="True" CssClass="btn"></asp:Button> &nbsp;
    <asp:Button id="btnPrint" tabIndex=16 runat="server" Text="Report" 
        CssClass="btn" Visible="False"></asp:Button></TD></TR><TR><TD colSpan=4><TABLE><TBODY><TR><TD style="WIDTH: 130px"><asp:Label id="Label1" runat="server" Text="Credit Note" CssClass="field_caption"></asp:Label></TD><TD style="WIDTH: 242px">
        <INPUT style="WIDTH: 180px" id="txtCreditNoteNo" class="txtbox" tabIndex=1 
            type=text runat="server" /></TD><TD style="WIDTH: 130px"><asp:Label id="Label3" runat="server" Text="File Number" Width="64px" CssClass="field_caption"></asp:Label></TD><TD>
        <INPUT style="WIDTH: 180px" id="txtRequestId" class="txtbox" tabIndex=2 
            type=text runat="server" /></TD>
    <td style="width: 47px">
        <asp:Label ID="lblInvoiceNo" runat="server" CssClass="field_caption" Text="Invoice No"
            Width="64px"></asp:Label></td>
    <td style="width: 47px">
        <INPUT style="WIDTH: 180px" id="txtInvoiceNo" class="txtbox" tabIndex=2 
            type=text runat="server" /></td>
</TR><TR><TD style="WIDTH: 130px"><asp:Label id="Label2" runat="server" Text="From Credit Note Date" CssClass="field_caption"></asp:Label></TD><TD style="WIDTH: 242px">
            <asp:TextBox id="txtFromDate" tabIndex=3 runat="server" Width="80px" 
                CssClass="txtbox"></asp:TextBox>&nbsp; <asp:ImageButton id="ImgBtnFrmDt" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.png"></asp:ImageButton> <cc1:MaskedEditValidator id="MEVFromDate" runat="server" CssClass="field_error" TooltipMessage="Input a date in dd/mm/yyyy format" InvalidValueMessage="Invalid Date" EmptyValueMessage="Date is required" ControlExtender="MEFromDate" ControlToValidate="txtFromDate" Display="Dynamic" InvalidValueBlurredMessage="Invalid Date" EmptyValueBlurredText="Date is required"></cc1:MaskedEditValidator></TD><TD style="WIDTH: 130px"><asp:Label id="Label5" runat="server" Text="To Credit Note Date" CssClass="field_caption"></asp:Label></TD><TD>
            <asp:TextBox id="txtToDate" tabIndex=4 runat="server" Width="80px" 
                CssClass="txtbox"></asp:TextBox> <asp:ImageButton id="ImgBtnRevDate" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.png"></asp:ImageButton> <cc1:MaskedEditValidator id="MEVToDate" runat="server" CssClass="field_error" TooltipMessage="Input a date in dd/mm/yyyy format" InvalidValueMessage="Invalid Date" EmptyValueMessage="Date is required" ControlExtender="METoDate" ControlToValidate="txtToDate" Display="Dynamic" InvalidValueBlurredMessage="Invalid Date" EmptyValueBlurredText="Date is required"></cc1:MaskedEditValidator></TD>
    <td style="width: 47px">
    </td>
    <td style="width: 47px">
    </td>
</TR></TBODY></TABLE></TD></TR><TR><TD colSpan=4><asp:Panel id="pnlAdvSearch" runat="server" Visible="False"><TABLE class="td_cell"><TBODY><TR>
        <TD style="width: 130px" ><asp:Label id="Label4" runat="server" Text="Status" Width="120px" CssClass="field_caption"></asp:Label></TD><TD style="width: 240px" >
                <SELECT style="WIDTH: 154px" id="ddlStatus" class="td_cell" tabIndex=5 runat="server"> 
                <OPTION value="[Select]" selected>[Select]</OPTION> 
                <OPTION value="P">Posted</OPTION> <OPTION value="U">UnPosted</OPTION></SELECT></TD>
        <TD style="width: 134px" ></TD><TD style="width: 199px" ></TD></TR><TR>
            <TD style="width: 130px" ><asp:Label id="Label6" runat="server" Text="Customer" Width="120px" CssClass="field_caption"></asp:Label></TD><TD style="width: 240px" >
                    <SELECT style="WIDTH: 200px" id="ddlCustomer" class="drpdown" 
            tabIndex=6 runat="server"> 
                    <OPTION selected></OPTION></SELECT></TD>
            <TD style="width: 134px" ><asp:Label id="Label9" runat="server" Text="Customer Ref" Width="120px" CssClass="field_caption"></asp:Label></TD><TD style="width: 199px" >
                    <INPUT id="txtCustRef" class="txtbox" tabIndex=7 type=text 
            runat="server" /></TD></TR>
        <TR><TD style="width: 130px" >
            <asp:Label id="Label7" runat="server" Text="From Amount" Width="120px" CssClass="field_caption"></asp:Label></TD>
            <TD style="width: 240px" ><INPUT id="txtFromAmount" class="txtbox" 
            tabIndex=8 type=text runat="server" /></TD>
            <TD style="width: 134px" ><asp:Label id="Label10" runat="server" Text="To Amount" Width="120px" CssClass="field_caption"></asp:Label></TD><TD style="width: 199px" >
                <INPUT id="txtToAmount" class="txtbox" tabIndex=9 type=text 
            runat="server" /></TD></TR>
        <TR><TD style="width: 130px" >
            <asp:Label id="Label8" runat="server" Text="Order By" Width="120px" CssClass="field_caption"></asp:Label></TD>
            <TD  title=" " style="width: 240px">
                <SELECT style="WIDTH: 200px" id="ddlOrderBy" class="drpdown" 
            tabIndex=10 runat="server"> <OPTION value="0" selected>Invoice No Desc</OPTION><OPTION value="1">Invoice No Asc</OPTION><OPTION value="2">Customer Code</OPTION><OPTION value="3">Customer Name</OPTION><OPTION value="4">Invoice Date</OPTION><OPTION value="5">File Number</OPTION></SELECT></TD>
            <TD style="width: 134px" >
    <input id="txtconnection" runat="server" style="visibility: hidden; width: 12px;
        height: 9px" type="text" /></TD><TD style="width: 199px" ></TD></TR></TBODY></TABLE></asp:Panel></TD></TR></TBODY></TABLE><TABLE cols=3><TBODY><TR><TD style="WIDTH: 131px"><SPAN>&nbsp;<asp:Label id="Label11" runat="server" Text="Report Type" Width="120px" CssClass="field_caption"></asp:Label></SPAN></TD><TD style="WIDTH: 223px">
                <asp:DropDownList id="ddlrpttype" tabIndex=11 runat="server" Width="207px" 
                    CssClass="drpdown"><asp:ListItem Value="0">Brief</asp:ListItem>
<asp:ListItem Value="1">Detailed</asp:ListItem>
</asp:DropDownList></TD><TD><SPAN><asp:Label id="Label12" runat="server" Text="Applicable Only for the Report" ForeColor="Red" Width="238px" CssClass="field_caption"></asp:Label></SPAN></TD></TR></TBODY></TABLE><cc1:CalendarExtender id="CEFromDate" runat="server" Format="dd/MM/yyyy" PopupButtonID="ImgBtnFrmDt" TargetControlID="txtFromDate"></cc1:CalendarExtender> <cc1:MaskedEditExtender id="MEFromDate" runat="server" TargetControlID="txtFromDate" MaskType="Date" Mask="99/99/9999"></cc1:MaskedEditExtender> <cc1:CalendarExtender id="CEToDate" runat="server" Format="dd/MM/yyyy" PopupButtonID="ImgBtnRevDate" TargetControlID="txtToDate"></cc1:CalendarExtender> <cc1:MaskedEditExtender id="METoDate" runat="server" TargetControlID="txtToDate" MaskType="Date" Mask="99/99/9999"></cc1:MaskedEditExtender> 
</contenttemplate>
    </asp:UpdatePanel></td>
        </tr>
        <tr>
            <td>
                &nbsp;
                <asp:Button ID="BtnExportToExcel" runat="server" CssClass="btn" TabIndex="17"
                    Text="Export To Excel" /><input id="txtSearchExport" runat="server"
                        style="visibility: hidden; width: 8px" type="text" /></td>
        </tr>
        <tr>
            <td class="td_cell">
                <asp:UpdatePanel id="UpdatePanel2" runat="server">
                    <contenttemplate>
<asp:GridView id="gvResult" tabIndex=18 runat="server" CssClass="grdstyle" GridLines="Vertical" AutoGenerateColumns="False" AllowPaging="True" CellPadding="3"><Columns>
<asp:TemplateField HeaderText="Credit Note No">
    <ItemTemplate>
        <asp:Label ID="lblCreditNoteNo" runat="server" Text='<%# Bind("creditnoteno") %>'></asp:Label>
    </ItemTemplate>
</asp:TemplateField>
<asp:BoundField DataField="post_state" HeaderText="Status"></asp:BoundField>
<asp:BoundField DataFormatString="{0:dd/MM/yyyy }" DataField="creditnotedate" HeaderText="Credit Note Date"></asp:BoundField>
<asp:TemplateField HeaderText="File Number">
    <ItemTemplate>
        <asp:Label ID="lblRequestId" runat="server" Text='<%# Bind("requestid") %>'></asp:Label>
    </ItemTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="Invoice No">
    <ItemTemplate>
        <asp:Label ID="lblinvoiceno" runat="server" Text='<%# Bind("invoiceno") %>'></asp:Label>
    </ItemTemplate>
</asp:TemplateField>
<asp:BoundField DataField="agentcode" HeaderText="Customer Code"></asp:BoundField>
<asp:BoundField DataField="agentname" HeaderText="Customer Name"></asp:BoundField>
<asp:BoundField DataField="agentref" HeaderText="Customer Ref"></asp:BoundField>
<asp:BoundField DataField="currcode" HeaderText="Currency"></asp:BoundField>
<asp:BoundField DataField="refundsalecurrency" HeaderText="Sale Amount"></asp:BoundField>
<asp:BoundField DataField="refundsalevalue" HeaderText="BaseCurr Amount"></asp:BoundField>
<asp:BoundField DataFormatString="{0:dd/MM/yyyy}" DataField="adddate" HeaderText="Date Created"></asp:BoundField>
<asp:BoundField DataField="adduser" HeaderText="User Created"></asp:BoundField>
<asp:BoundField DataFormatString="{0:dd/MM/yyyy}" DataField="moddate" HeaderText="Date Modified"></asp:BoundField>
<asp:BoundField DataField="moduser" HeaderText="User Modified"></asp:BoundField>
<asp:ButtonField HeaderText="Action" Text="Edit" CommandName="RowEdit">
<ItemStyle ForeColor="Blue"></ItemStyle>
</asp:ButtonField>
<asp:ButtonField HeaderText="Action" Text="View" CommandName="RowView">
<ItemStyle ForeColor="Blue"></ItemStyle>
</asp:ButtonField>
<asp:ButtonField HeaderText="Action" Text="Print" CommandName="RowPrint">
<ItemStyle ForeColor="Blue"></ItemStyle>
</asp:ButtonField>
</Columns>

    <FooterStyle CssClass="grdfooter" />

<RowStyle CssClass="grdRowstyle"></RowStyle>
<SelectedRowStyle CssClass="grdselectrowstyle" ></SelectedRowStyle>
<PagerStyle CssClass="grdpagerstyle" HorizontalAlign="Center"></PagerStyle>
<HeaderStyle  CssClass="grdheader"></HeaderStyle>
<AlternatingRowStyle CssClass="grdAternaterow"></AlternatingRowStyle>
</asp:GridView>

 <asp:Label id="lblMsg" runat="server" Font-Size="9pt" Font-Names="Verdana" Font-Bold="True" 
                            Visible="False" CssClass="lblmsg">Records Not Found.</asp:Label> 
</contenttemplate>
                </asp:UpdatePanel></td>
        </tr>
        <tr>
            <td style="width: 100px">
<asp:ScriptManagerProxy id="ScriptManagerProxy1" runat="server">
    <services>
        <asp:ServiceReference Path="~/clsServices.asmx">
        </asp:ServiceReference>
    </services>
</asp:ScriptManagerProxy>
            </td>
        </tr>
    </table>
</asp:Content>