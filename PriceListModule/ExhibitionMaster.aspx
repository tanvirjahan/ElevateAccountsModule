<%@ Page Title="" Language="VB" MasterPageFile="~/SubPageMaster.master" AutoEventWireup="false" CodeFile="ExhibitionMaster.aspx.vb" Inherits="PriceListModule_ExhibitionMaster" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="EclipseWebSolutions.DatePicker" Namespace="EclipseWebSolutions.DatePicker"
    TagPrefix="ews" %>
<%@ OutputCache Location="none" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="server">

<script language="javascript" type="text/javascript">
    //function checkNumber(e)
    //			{	    
    //			    	
    ////				if ( (event.keyCode < 47 || event.keyCode > 57) )
    ////				{
    ////					return false;
    ////	            }   
    //         return true;
    //      }
    // 
    function checkNumber(evt) {
        evt = (evt) ? evt : event;
        var charCode = (evt.charCode) ? evt.charCode : ((evt.keyCode) ? evt.keyCode :
        ((evt.which) ? evt.which : 0));
        if (charCode != 47 && (charCode > 45 && charCode < 58)) {
            //alert("Enter numerals only in this field. "+ charCode);
            return true;
        }
        return false;
    }

    function compulsaryCode() {

    }
    function compulsaryName() {
        if (document.getElementById("<%=txtName.ClientID%>").value == "") {
            alert("Name field can not be blank");
            document.getElementById("<%=txtName.ClientID%>").focus();
            return false;
        }

    }
    function compulsaryCoin() {

    }

    function ValidationForExchate() {

    }


    function FormValidation(state) {
        if ((document.getElementById("<%=txtName.ClientID%>").value == "")) {

            if (document.getElementById("<%=txtName.ClientID%>").value == "") {
                document.getElementById("<%=txtName.ClientID%>").focus();
                alert("Name field can not be blank");
                return false;
            }
        }
        else {
            if (state == 'New') { if (confirm('Are you sure you want to save exhibition Type?') == false) return false; }
            if (state == 'Edit') { if (confirm('Are you sure you want to update exhibition Type?') == false) return false; }
            if (state == 'Delete') { if (confirm('Are you sure you want to delete exhibition Type?') == false) return false; }
        }
    }



    function chgValue() {

    }

</script>

<script type="text/javascript">
    var SelectedRow = null;
    var SelectedRowIndex = null;
    var UpperBound = null;
    var LowerBound = null;

    window.onload = function () {
        LowerBound = 0;
        SelectedRowIndex = -1;
    }

    function SelectRow(CurrentRow, RowIndex) {
        var gridView = document.getElementById("<%=grdDates.ClientID %>"); // *********** Change gridview name
        UpperBound = gridView.getElementsByTagName("tr").length - 2;
        if (SelectedRow == CurrentRow || RowIndex > UpperBound || RowIndex < LowerBound)
            return;

        if (SelectedRow != null) {
            SelectedRow.style.backgroundColor = SelectedRow.originalBackgroundColor;
            SelectedRow.style.color = SelectedRow.originalForeColor;
        }

        if (CurrentRow != null) {
            CurrentRow.originalBackgroundColor = CurrentRow.style.backgroundColor;
            CurrentRow.originalForeColor = CurrentRow.style.color;
            CurrentRow.style.backgroundColor = '#FFCC99';
            CurrentRow.style.color = 'Black';
            var txtFrm = CurrentRow.cells[0].getElementsByTagName("input")[0];
            txtFrm.focus();
            //alert(txtFrm.value);
        }

        SelectedRow = CurrentRow;
        SelectedRowIndex = RowIndex;
        setTimeout("SelectedRow.focus();", 0);
    }

    function SelectSibling(e) {
        var e = e ? e : window.event;
        var KeyCode = e.which ? e.which : e.keyCode;
        if (KeyCode == 40)
            SelectRow(SelectedRow.nextSibling, SelectedRowIndex + 1);
        else if (KeyCode == 38)
            SelectRow(SelectedRow.previousSibling, SelectedRowIndex - 1);

        // return false;
    }
    function LastSelectRow(CurrentRow, RowIndex) {
        var row = document.getElementById(CurrentRow);
        SelectRow(row, RowIndex);

    }
    </script>

    <asp:UpdatePanel id="UpdatePanel1" runat="server">
        <contenttemplate>
<TABLE style="BORDER-RIGHT: gray 2px solid; BORDER-TOP: gray 2px solid; BORDER-LEFT: gray 2px solid; BORDER-BOTTOM: gray 2px solid" class="td_cell">
<TBODY>
<TR>
<TD class="td_cell" align=center colSpan=3>
<asp:Label id="lblHeading" runat="server" Text="Add New Exhibition Master"  Width="100%" CssClass="field_heading"></asp:Label></TD>
</TR>
<TR>
    <TD style="WIDTH: 130px" class="td_cell">
    <SPAN style="COLOR: black">Exhibition Code</SPAN> <SPAN style="COLOR: red" class="td_cell">*</SPAN></TD>
    <TD style="COLOR: #000000; width: 177px;">
    <INPUT onblur="chgValue()" id="txtCode" class="field_input" tabIndex=1 type=text maxLength=20 runat="server" readonly="readonly" style="width:220px" /> </TD>
    <td style="COLOR: #000000; width: 194px;">
        &nbsp;</td>
</TR>
<TR>
<TD style="WIDTH: 130px; HEIGHT: 24px" class="td_cell">Exhibition Name<SPAN style="COLOR: red" class="td_cell">*</SPAN></TD>
<TD style="width: 210px">
<INPUT id="txtName" class="field_input" tabIndex=2 type=text maxLength=100  style="width:220px" runat="server" /> </TD>
<td style="width: 180">
&nbsp;</td>
</TR>
    
<TR>
<TD class="td_cell" style="width: 114px; ">
<asp:Label ID="Label1" runat="server" CssClass="td_ce" Text="Active" ViewStateMode="Enabled" Width="44px"></asp:Label></TD>
<TD style="width: 177px;"><INPUT id="chkActive" tabIndex=5 type=checkbox CHECKED runat="server" /></TD>
<td style="width: 194px">&nbsp;</td>
</TR>
<tr>
<td valign="top" colspan="2">
<table>
<tbody>
<tr>
<td style="width: 449px" class="td_cell" valign="top">
 <strong>Enter Dates</strong>
</td>
</tr>
<tr>
<td style="width: 300px" valign="top">
<asp:GridView ID="grdDates" runat="server" AllowSorting="True" AutoGenerateColumns="False"
BorderStyle="None" BorderWidth="1px" CellPadding="3" CssClass="grdstyle" Font-Size="10px"
GridLines="Vertical" TabIndex="12" Width="1px">
<FooterStyle CssClass="grdfooter" />
<Columns>
<asp:TemplateField Visible="False" HeaderText="LineNo">
<EditItemTemplate>
<asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("SrNo") %>' CssClass="field_input"></asp:TextBox>
</EditItemTemplate>
<ItemTemplate>
<asp:Label ID="lblCLineNo" runat="server" Text='<%# Bind("SrNo") %>'></asp:Label>
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="From Date">
<ItemTemplate>
<%--<ews:DatePicker id="dpFromDate" tabIndex=9 runat="server" CssClass="field_input" Width="171px" RegExErrorMessage="Please enter a date in the format: dd/mm/yyyy" DateFormatString="dd/MM/yyyy"></ews:DatePicker>--%>
<asp:TextBox ID="txtfromDate" runat="server" CssClass="fiel_input" Width="80px">
</asp:TextBox>
<asp:CalendarExtender ID="dpFromDate" runat="server" Format="dd/MM/yyyy" PopupButtonID="ImgBtnFrmDt"
PopupPosition="Right" TargetControlID="txtfromDate">
</asp:CalendarExtender>
<asp:MaskedEditExtender ID="MeFromDate" runat="server" Mask="99/99/9999" MaskType="Date"
TargetControlID="txtfromDate">
</asp:MaskedEditExtender>
<asp:ImageButton ID="ImgBtnFrmDt" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.png" />
<br />
<asp:MaskedEditValidator ID="MevFromDate" runat="server" ControlExtender="MeFromDate"
ControlToValidate="txtfromDate" CssClass="field_error" Display="Dynamic" EmptyValueBlurredText="Date is required"
EmptyValueMessage="Date is required" ErrorMessage="MeFromDate" InvalidValueBlurredMessage="Invalid Date"
InvalidValueMessage="Invalid Date" TooltipMessage="Input a Date in Date/Month/Year">
</asp:MaskedEditValidator>
</ItemTemplate>
<HeaderStyle Wrap="False" />
<ItemStyle Wrap="False" />
</asp:TemplateField>
<asp:TemplateField HeaderText="To Date">
<ItemTemplate>
<%--<ews:DatePicker id="dpFromDate" tabIndex=9 runat="server" CssClass="field_input" Width="171px" RegExErrorMessage="Please enter a date in the format: dd/mm/yyyy" DateFormatString="dd/MM/yyyy"></ews:DatePicker>--%>
<asp:TextBox ID="txtToDate" runat="server" CssClass="fiel_input" Width="80px">
</asp:TextBox>
<asp:CalendarExtender ID="dpToDate" runat="server" Format="dd/MM/yyyy" PopupButtonID="ImgBtnToDt"
PopupPosition="Right" TargetControlID="txtToDate">
</asp:CalendarExtender>
<asp:MaskedEditExtender ID="MeToDate" runat="server" Mask="99/99/9999" MaskType="Date"
TargetControlID="txtToDate">
</asp:MaskedEditExtender>
<asp:ImageButton ID="ImgBtnToDt" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.png" />
<br />
<asp:MaskedEditValidator ID="MevToDate" runat="server" ControlExtender="MeToDate"
ControlToValidate="txtToDate" CssClass="field_error" Display="Dynamic" EmptyValueBlurredText="Date is required"
EmptyValueMessage="Date is required" ErrorMessage="MeToDate" InvalidValueBlurredMessage="Invalid Date"
InvalidValueMessage="Invalid Date" TooltipMessage="Input a Date in Date/Month/Year">
</asp:MaskedEditValidator>
</ItemTemplate>
<HeaderStyle Wrap="False" />
<ItemStyle Wrap="False" />
</asp:TemplateField>
<asp:TemplateField HeaderText="Delete">
<ItemTemplate>
<asp:CheckBox ID="chckDeletion" runat="server" Width="10px"></asp:CheckBox>
</ItemTemplate>
</asp:TemplateField>
</Columns>
<RowStyle CssClass="grdRowstyle" />
<SelectedRowStyle CssClass="grdselectrowstyle" />
<PagerStyle CssClass="grdpagerstyle" HorizontalAlign="Center" />
<HeaderStyle CssClass="grdheader" />
<AlternatingRowStyle CssClass="grdAternaterow" />
</asp:GridView>
                                                                       
</td>
                                                                  


<tr>
<td style="height: 22px">
<asp:Button ID="btnAddLineDates" TabIndex="25" OnClick="btnAddLineDates_Click" runat="server" Text="Add Row" CssClass="btn"></asp:Button>&nbsp;
<asp:Button ID="btnDeleteLineDates" TabIndex="26" OnClick="btnDeleteLineDates_Click" runat="server" Text="Delete Row" CssClass="btn"></asp:Button>
</td>
</tr>
</tbody>
</table>
</td>
    </tr>
      <tr>
     <td></td>
       </tr>
       <tr>
       <td></td>
       </tr>
        <tr>
            <td class="td_cell" style="width: 80px; height: 23px;" colspan="2">
                <asp:Button ID="btnSave" runat="server" CssClass="btn" tabIndex="6" 
                    Text="Save" />
            
           
                <asp:Button ID="btnCancel" runat="server" CssClass="btn" tabIndex="7" 
                    Text="Return to Search" />
                &nbsp;
          
                <asp:Button ID="btnHelp" runat="server" CssClass="btn" tabIndex="8" 
                    Text="Help" />
            </td>
       </tr>

    </TBODY></TABLE>
</contenttemplate>
    </asp:UpdatePanel>
</asp:Content>

