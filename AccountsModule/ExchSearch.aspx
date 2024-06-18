<%@ Page Language="VB" MasterPageFile="~/AccountsMaster.master" AutoEventWireup="false" CodeFile="ExchSearch.aspx.vb" Inherits="ExchSearch" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">

    <TABLE style="BORDER-RIGHT: gray 2px solid; BORDER-TOP: gray 2px solid; BORDER-LEFT: gray 2px solid; BORDER-BOTTOM: gray 2px solid;" class="td_cell"><TBODY><TR><TD  class="field_heading" align=center >
        Exchange Difference List</TD></TR><TR><TD style=" COLOR: blue; " class="td_cell" align=center>Type few characters of code or name and click search &nbsp; &nbsp;</TD></TR>
    <TR><TD >
<asp:UpdatePanel id="UpdatePanel1" runat="server"><contenttemplate>
<TABLE><TBODY><TR><TD class=" " align=center colSpan=6>&nbsp;<asp:RadioButton id="rbsearch" runat="server" Text="Search" ForeColor="Black" CssClass="td_cell" AutoPostBack="True" Checked="True" GroupName="GrSearch" OnCheckedChanged="rbtnsearch_CheckedChanged" __designer:wfdid="w1"></asp:RadioButton> <asp:RadioButton id="rbtnadsearch" runat="server" Text="Advance Search" ForeColor="Black" CssClass="td_cell" AutoPostBack="True" GroupName="GrSearch" OnCheckedChanged="rbtnadsearch_CheckedChanged" __designer:wfdid="w2"></asp:RadioButton> 
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
        CssClass="btn" __designer:wfdid="w8"></asp:Button></TD></TR><TR><TD class="td_cell">&nbsp;<asp:Label id="Label1" runat="server" Text="  Doc No." Width="120px" CssClass="field_caption"></asp:Label></TD><TD style="WIDTH: 601px"><asp:TextBox id="txtTranId" tabIndex=1 runat="server" Width="194px" CssClass="field_input" MaxLength="20"></asp:TextBox></TD><TD class="td_cell"><SPAN></SPAN>&nbsp; </TD><TD>&nbsp;</TD></TR>
    <tr>
        <td class="td_cell">
            Description</td>
        <td style="width: 601px">
            <asp:TextBox ID="txtdesc" runat="server" CssClass="field_input" Width="293px"></asp:TextBox></td>
        <td class="td_cell">
        </td>
        <td>
        </td>
    </tr>
    <TR><TD class="td_cell" colSpan=4><asp:Panel id="pnlSearch" runat="server" Width="0px"><TABLE><TBODY><TR><TD><asp:Label id="Label2" runat="server" Text=" Journal From Date" Width="120px" CssClass="field_caption"></asp:Label></TD><TD><asp:TextBox id="txtFromDate" tabIndex=2 runat="server" Width="80px" CssClass="fiel_input" ValidationGroup="MKE">10/09/2008</asp:TextBox>
    &nbsp;<asp:ImageButton id="ImgBtnFrmDt" tabIndex=4 runat="server" ImageUrl="~/Images/Calendar_scheduleHS.png"></asp:ImageButton>
    <cc1:maskededitvalidator id="MskVFromDt" runat="server" 
            controlextender="MskFromDate" controltovalidate="txtFromDate" 
            cssclass="field_error" display="Dynamic" emptyvalueblurredtext="*" 
            emptyvaluemessage="Date is required" errormessage="MskVFromDate" 
            invalidvalueblurredmessage="*" invalidvaluemessage="Invalid Date" 
            tooltipmessage="Input a date in dd/mm/yyyy format" validationgroup="MKE" 
            width="72px"></cc1:maskededitvalidator></TD>
    <TD><asp:Label id="Label3" runat="server" Text=" Journal To Date&#13;&#10;" Width="120px" CssClass="field_caption"></asp:Label></TD><TD><asp:TextBox id="txtTodate" tabIndex=3 runat="server" Width="80px" CssClass="fiel_input" ValidationGroup="MKE">10/09/2008</asp:TextBox>&nbsp;<asp:ImageButton id="ImageButton1" tabIndex=5 runat="server" ImageUrl="~/Images/Calendar_scheduleHS.png"></asp:ImageButton>
        <cc1:MaskedEditValidator id="MaskedEditValidator1" runat="server" Width="70px" 
            CssClass="field_error" ValidationGroup="MKE" ControlExtender="MskChequeDate" 
            ControlToValidate="txtTodate" Display="Dynamic" EmptyValueBlurredText="*" 
            EmptyValueMessage="Date is required" ErrorMessage="MskVFromDate" 
            InvalidValueBlurredMessage="*" InvalidValueMessage="Invalid Date" 
            TooltipMessage="Input a date in dd/mm/yyyy format"></cc1:MaskedEditValidator></TD></TR><TR><TD><asp:Label id="Label4" runat="server" Text="Status" Width="120px" CssClass="field_caption"></asp:Label></TD><TD>
        <SELECT style="WIDTH: 184px" id="ddlStatus" class="td_cell" tabIndex=4 
            runat="server"> <OPTION value="[Select]" selected>[Select]</OPTION> <OPTION value="P">Posted</OPTION> <OPTION value="U">UnPosted</OPTION>
        <option value="Y">Cancelled</option>
    </SELECT></TD><TD></TD><TD></TD></TR></TBODY></TABLE></asp:Panel></TD></TR><TR><TD class="td_cell" colSpan=4><cc1:MaskedEditExtender id="MskChequeDate" runat="server" TargetControlID="txtTodate" MessageValidatorTip="true" MaskType="Date" Mask="99/99/9999" ErrorTooltipEnabled="True" DisplayMoney="Left" AcceptNegative="Left">
    </cc1:MaskedEditExtender> <cc1:MaskedEditExtender id="MskFromDate" runat="server" TargetControlID="txtFromDate" MessageValidatorTip="true" MaskType="Date" Mask="99/99/9999" ErrorTooltipEnabled="True" DisplayMoney="Left" AcceptNegative="Left">
    </cc1:MaskedEditExtender> <cc1:CalendarExtender id="ClExChequeDate" runat="server" TargetControlID="txtTodate" PopupButtonID="ImageButton1" Format="dd/MM/yyyy">
    </cc1:CalendarExtender> <cc1:CalendarExtender id="ClsExFromDate" runat="server" TargetControlID="txtFromDate" PopupButtonID="ImgBtnFrmDt" Format="dd/MM/yyyy">
    </cc1:CalendarExtender></TD></TR></TBODY></TABLE>
</contenttemplate>
        </asp:UpdatePanel>
    </TD></TR>
        <tr>
            <td >
                &nbsp;<asp:Button id="btnExportToExcel" tabIndex=9 runat="server" 
                    Text="Export To Excel" CssClass="field_button"></asp:Button> </td>
        </tr>
        <tr>
            <td >
     <asp:UpdatePanel id="UpdatePanel2" runat="server">
        <contenttemplate>
<asp:GridView id="gv_SearchResult" tabIndex=11 runat="server" BackColor="White" Width="930px" CssClass="td_cell" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False" BorderColor="#999999" BorderStyle="None" BorderWidth="1px" CellPadding="3" GridLines="Vertical">
<FooterStyle BackColor="#6B6B9A" ForeColor="Black"></FooterStyle>
<Columns>
<asp:TemplateField Visible="False" HeaderText="Transaction Id"><EditItemTemplate>
<asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("tran_id") %>'></asp:TextBox>
                            
</EditItemTemplate>
<ItemTemplate>
<asp:Label ID="lblTranID" runat="server" Text='<%# Bind("tran_id") %>'></asp:Label>
                            
</ItemTemplate>
</asp:TemplateField>
<asp:BoundField DataField="tran_id" SortExpression="tran_id" HeaderText="Document No"></asp:BoundField>
<asp:BoundField DataField="tran_type" SortExpression="tran_type" HeaderText="Doc Type"></asp:BoundField>
<asp:BoundField DataField="post_state" HeaderText="Status"></asp:BoundField>
<asp:BoundField HtmlEncode="False" DataFormatString="{0:dd/MM/yyyy}" DataField="journal_date" SortExpression="journal_date" HeaderText="Journal Date"></asp:BoundField>
<asp:BoundField HtmlEncode="False" DataFormatString="{0:dd/MM/yyyy}" DataField="journal_tran_date" SortExpression="journal_tran_date" HeaderText="Posted Date"></asp:BoundField>
<asp:BoundField DataField="journal_narration" SortExpression="journal_narration" HeaderText="Narration"></asp:BoundField>
<asp:BoundField HtmlEncode="False" DataFormatString="{0:dd/MM/yyyy hh:mm:ss tt} " DataField="adddate" SortExpression="adddate" HeaderText="Date Created"></asp:BoundField>
<asp:BoundField DataField="adduser" SortExpression="adduser" HeaderText="User Created"></asp:BoundField>
<asp:BoundField HtmlEncode="False" DataFormatString="{0:dd/MM/yyyy hh:mm:ss tt} " DataField="moddate" SortExpression="moddate" HeaderText="Date Modified"></asp:BoundField>
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
<asp:ButtonField HeaderText="Action" Text="UndoCancel" CommandName="UndoCancel">
<ItemStyle ForeColor="Blue"></ItemStyle>
</asp:ButtonField>
</Columns>

<RowStyle CssClass="grdRowstyle" ForeColor="#084573"></RowStyle>

<SelectedRowStyle BackColor="#008A8C" ForeColor="White" Font-Bold="True"></SelectedRowStyle>

<PagerStyle  CssClass="grdpagerstyle" ForeColor="Black" HorizontalAlign="Center"></PagerStyle>

<HeaderStyle CssClass="grdheader" ForeColor="White" Font-Bold="True"></HeaderStyle>

<AlternatingRowStyle CssClass="grdAternaterow" ></AlternatingRowStyle>
</asp:GridView> <asp:Label id="lblMessg" runat="server" 
                Text="Records not found. Please redefine search criteria" Font-Size="8pt" 
                Font-Names="Verdana" Font-Bold="True" Width="357px" CssClass="lblmsg"></asp:Label> 
</contenttemplate>
    </asp:UpdatePanel>&nbsp; &nbsp;
            </td>
        </tr>
    </TBODY></TABLE>
    &nbsp;&nbsp;
</asp:Content>

