<%@ Page Language="VB" MasterPageFile="~/SubPageMaster.master" AutoEventWireup="false" CodeFile="OtherServicesCostPriceList2.aspx.vb" Inherits="OtherServicesCostPriceList2"   %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
      <%@ Register Assembly="EclipseWebSolutions.DatePicker" Namespace="EclipseWebSolutions.DatePicker"
    TagPrefix="ews" %>
    <%@ OutputCache location="none" %> 
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">
<script language="JavaScript" type="text/javascript" >
    window.history.forward(1);  
</script>
<script language="javascript" type="text/javascript" >
function chkTextLock(evt)
	{
         return false;
 	}
	function chkTextLock1(evt)
	{
       if ( evt.keyCode =9 )
        { 
         return true;
        }
        else
        {
       return false;
        }
       return false;
      
	}
					function checkNumber(evt) {
    evt = (evt) ? evt : event;
    var charCode = (evt.charCode) ? evt.charCode : ((evt.keyCode) ? evt.keyCode : 
        ((evt.which) ? evt.which : 0));
    //if (charCode != 47 && (charCode > 45 && charCode < 58)) {    
    if (charCode != 47 && (charCode > 44 && charCode < 58)) {    
        return true;
            }
       return false;
}
	function checkNumber1(evt) {
    evt = (evt) ? evt : event;
    var charCode = (evt.charCode) ? evt.charCode : ((evt.keyCode) ? evt.keyCode : 
        ((evt.which) ? evt.which : 0));
    if ((charCode > 47 && charCode < 58)) {    
    //if (charCode != 47 && (charCode > 44 && charCode < 58)) {    
        return true;
            }
       return false;
}

function checkNumber2(evt) {
    evt = (evt) ? evt : event;
    var charCode = (evt.charCode) ? evt.charCode : ((evt.keyCode) ? evt.keyCode : 
        ((evt.which) ? evt.which : 0));
    if ((charCode > 46 && charCode < 58)) {    
       
        return true;
            }
       return false;
}
	
	
function checkTelephoneNumber(e)
			{	    
			    	
				if ( (event.keyCode < 45 || event.keyCode > 57) )
				{
					return false;
	            }   
	         	
			}
//function checkNumber(e)
//			{	    
//			    	
//				if ( (event.keyCode < 45 || event.keyCode > 57) )
//				{
//					return false;
//	            }   
//	         	
//			}
function checkCharacter(e)
			{	    
			    if (event.keyCode == 32 || event.keyCode ==46)
			        return;			
				if ( (event.keyCode < 47 || event.keyCode > 57) && (event.keyCode < 65 || event.keyCode > 91) && (event.keyCode < 96 || event.keyCode > 122))
				{
					return false;
	            }

}


function changedate1() {
    var objGridView = document.getElementById('<%=gv_SearchResult.ClientID%>');
    var rcount = objGridView.rows.length;
    var ccount = objGridView.rows[0].cells.length
    var rowIdx = 1;

    var rowElement;
    var txtBox;
    var i = 0;

    var dt1 = document.getElementById('<%=dpFromDate.ClientID%>');


    for (rowIdx; rowIdx <= rcount - 1; rowIdx++) {
        rowElement = objGridView.rows[rowIdx];
        txtBox = rowElement.cells[ccount - 3].firstChild;
        txtBox.value = dt1.value;
    }

}

function changedate2() {
    var objGridView = document.getElementById('<%=gv_SearchResult.ClientID%>');
    var rcount = objGridView.rows.length;
    var ccount = objGridView.rows[0].cells.length
    var rowIdx = 1;
    var rowElement;
    var txtBox;
    var i = 0;

    var dt1 = document.getElementById('<%=dpToDate.ClientID%>');


    for (rowIdx; rowIdx <= rcount - 1; rowIdx++) {
        rowElement = objGridView.rows[rowIdx];
        txtBox = rowElement.cells[ccount - 2].firstChild;
        txtBox.value = dt1.value;
    }

}



			</script>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
<TABLE style="BORDER-RIGHT: gray 2px solid; BORDER-TOP: gray 2px solid; BORDER-LEFT: gray 2px solid; BORDER-BOTTOM: gray 2px solid" align=left><TBODY><TR><TD class="field_heading" align=center><asp:Label id="lblHeading" runat="server" Text="Other Services Cost  Price List" Width="358px" CssClass="field_heading"></asp:Label></TD></TR><TR><TD style="WIDTH: 1049px; HEIGHT: 87px"><TABLE style="WIDTH: 742px"><TBODY><TR><TD style="WIDTH: 251px" class="td_cell" align=left><SPAN class="td_cell">PL Code</SPAN></TD><TD style="WIDTH: 179px"><INPUT style="WIDTH: 194px" id="txtPlcCode" class="field_input" disabled tabIndex=1 type=text runat="server" /></TD><TD class="td_cell"></TD><TD></TD></TR><TR><TD style="WIDTH: 251px; HEIGHT: 22px" class="td_cell" align=left><SPAN style="FONT-FAMILY: Arial">Supplier &nbsp;Type Code</SPAN></TD><TD style="WIDTH: 179px; HEIGHT: 22px"><SELECT style="WIDTH: 200px" id="ddlSPType" class="field_input" tabIndex=2 onchange="CallWebMethod('sptypecode');" runat="server"> <OPTION selected></OPTION></SELECT></TD><TD style="WIDTH: 156px; HEIGHT: 22px" class="td_cell" align=left><SPAN class="td_cell">Supplier &nbsp;Type Name</SPAN></TD><TD style="HEIGHT: 22px"><SELECT style="WIDTH: 300px" id="ddlSPTypeName" class="field_input" tabIndex=3 onchange="CallWebMethod('sptypename');" runat="server"> <OPTION selected></OPTION></SELECT></TD></TR><TR><TD style="WIDTH: 251px" class="td_cell" align=left><SPAN style="FONT-FAMILY: Arial">Supplier Code</SPAN></TD><TD style="WIDTH: 179px; HEIGHT: 22px"><SELECT style="WIDTH: 200px" id="ddlSupplierCode" class="field_input" tabIndex=4 onchange="CallWebMethod('SellTypeCode');" runat="server"> <OPTION selected></OPTION></SELECT> </TD><TD style="WIDTH: 156px; HEIGHT: 22px" class="td_cell" align=left><SPAN class="td_cell">Supplier Name</SPAN></TD><TD><SELECT style="WIDTH: 300px" id="ddlSupplierName" class="field_input" tabIndex=5 onchange="CallWebMethod('SellTypeName');" runat="server"> <OPTION selected></OPTION></SELECT> </TD></TR><TR><TD style="WIDTH: 251px" class="td_cell" align=left><SPAN class="td_cell">Supplier Agent Code</SPAN></TD><TD style="WIDTH: 179px"><SELECT style="WIDTH: 200px" id="ddlSupplierAgentCode" class="field_input" tabIndex=6 onchange="CallWebMethod('SellTypeCode');" runat="server"> <OPTION selected></OPTION></SELECT> </TD><TD style="WIDTH: 156px" class="td_cell" align=left><SPAN class="td_cell">Supplier Agent Name</SPAN></TD><TD><SELECT style="WIDTH: 300px" id="ddlSupplierAgentName" class="field_input" tabIndex=7 onchange="CallWebMethod('SellTypeCode');" runat="server"> <OPTION selected></OPTION></SELECT> </TD></TR><TR><TD style="WIDTH: 251px" class=" td_cell">Market Code</TD><TD style="WIDTH: 179px"><SELECT style="WIDTH: 200px" id="ddlMarketCode" class="field_input" tabIndex=8 onchange="CallWebMethod('marketcode');" runat="server"> <OPTION selected></OPTION></SELECT></TD><TD class=" td_cell">Market Name</TD><TD><SELECT style="WIDTH: 300px" id="ddlMarketName" class="field_input" tabIndex=9 onchange="CallWebMethod('marketname');" runat="server"> <OPTION selected></OPTION></SELECT></TD></TR><TR><TD style="WIDTH: 251px" class="td_cell" align=left><SPAN class="td_cell">Group Code</SPAN></TD><TD style="WIDTH: 179px"><SELECT style="WIDTH: 200px" id="ddlGroupCode" class="field_input" tabIndex=10 onchange="CallWebMethod('GroupCode');" runat="server"> <OPTION selected></OPTION></SELECT> </TD><TD style="WIDTH: 156px" class="td_cell" align=left><SPAN class="td_cell">Group Name</SPAN></TD><TD><SELECT style="WIDTH: 300px" id="ddlGroupName" class="field_input" tabIndex=11 onchange="CallWebMethod('GroupName');" runat="server"> <OPTION selected></OPTION></SELECT> </TD></TR><TR><TD style="WIDTH: 251px" class="td_cell" align=left><SPAN class="td_cell">Currency Code</SPAN></TD><TD style="WIDTH: 179px"><asp:TextBox id="txtCurrCode" tabIndex=12 runat="server" Width="194px" CssClass="field_input"></asp:TextBox></TD><TD style="WIDTH: 156px" class="td_cell" align=left><SPAN class="td_cell">Currency Name</SPAN></TD><TD><asp:TextBox id="txtCurrName" tabIndex=13 runat="server" Width="294px" CssClass="field_input"></asp:TextBox></TD></TR><TR><TD style="WIDTH: 251px" class="td_cell" align=left><SPAN class="td_cell">Sub Season Code</SPAN></TD><TD style="WIDTH: 179px"><SELECT style="WIDTH: 200px" id="ddlSubSeasCode" class="field_input" tabIndex=14 onchange="CallWebMethod('SeasCode');" runat="server"> <OPTION selected></OPTION></SELECT> </TD><TD style="WIDTH: 156px" class="td_cell" align=left><SPAN class="td_cell">Sub Season Code</SPAN></TD><TD><SELECT style="WIDTH: 300px" id="ddlSubSeasName" class="field_input" tabIndex=15 onchange="CallWebMethod('SeasName');" runat="server"> <OPTION selected></OPTION></SELECT> </TD></TR><TR><TD style="WIDTH: 251px" class="td_cell" align=left><SPAN class="td_cell">From Date</SPAN></TD><TD style="WIDTH: 179px">
    <asp:TextBox ID="dpFromDate" runat="server" CssClass="fiel_input" Height="18px" 
        Width="103px"></asp:TextBox>
    <cc1:CalendarExtender ID="dpFromDate_CalendarExtender" runat="server" 
        Enabled="True" Format="dd/MM/yyyy" onclienthidden="changedate1" 
        PopupButtonID="btnCal1" TargetControlID="dpFromDate">
    </cc1:CalendarExtender>
    <asp:Button ID="btnCal1" runat="server" Height="23px" Width="25px" />
    </TD><TD style="WIDTH: 156px" class="td_cell" align=left><SPAN class="td_cell">To Date</SPAN></TD><TD>
    <asp:TextBox ID="dpToDate" runat="server" CssClass="fiel_input" Height="16px" 
        Width="91px"></asp:TextBox>
    <cc1:CalendarExtender ID="dpToDate_CalendarExtender" runat="server" 
        Enabled="True" Format="dd/MM/yyyy" onclienthidden="changedate2" 
        PopupButtonID="btnCal2" TargetControlID="dpToDate">
    </cc1:CalendarExtender>
    <asp:Button ID="btnCal2" runat="server" Height="23px" Width="25px" />
    </TD></TR><TR><TD style="WIDTH: 251px" class="td_cell" align=left><SPAN class="td_cell">Remark</SPAN></TD><TD style="WIDTH: 183px" align=left colSpan=3>
    <TEXTAREA style="WIDTH: 607px; HEIGHT: 59px" id="txtRemark" class="field_input" disabled tabIndex=18 runat="server"></TEXTAREA></TD></TR><TR><TD style="WIDTH: 251px" class="td_cell" align=left><SPAN id="td_cell">Active</SPAN></TD><TD style="WIDTH: 183px" align=left colSpan=3><INPUT id="ChkActive" tabIndex=19 type=checkbox CHECKED runat="server" /></TD></TR><TR><TD style="WIDTH: 251px; height: 21px;" class="td_cell" align=left></TD><TD style="WIDTH: 179px; height: 21px;"></TD><TD style="WIDTH: 156px; height: 21px;" class="td_cell" align=left></TD><TD align=right style="height: 21px">&nbsp;</TD></TR>
    </TBODY></TABLE>
    
    <asp:Panel id="Panel1" runat="server" Height="393px" Width="900px" ScrollBars="Auto" GroupingText="Enter Rates">
    <TABLE><TBODY><TR><TD style="WIDTH: 100px"><asp:GridView id="gv_SearchResult" tabIndex=20 runat="server" Font-Size="10px" BackColor="White" Width="1px" CssClass="td_cell" AutoGenerateColumns="False" BorderColor="#999999" BorderStyle="None" BorderWidth="1px" CellPadding="3" GridLines="Vertical">
<FooterStyle BackColor="#6B6B9A" ForeColor="Black"></FooterStyle>
<Columns>
<asp:BoundField DataField="Sr No" HeaderText="Sr No"></asp:BoundField>
<asp:BoundField DataField="Service Type Code" HeaderText="Service Type Code"></asp:BoundField>
<asp:BoundField DataField="Service Type Name" HeaderText="Service Type Name"></asp:BoundField>
</Columns>

<RowStyle CssClass="grdRowstyle" ForeColor="Black"></RowStyle>

<SelectedRowStyle BackColor="#008A8C" ForeColor="White" Font-Bold="True"></SelectedRowStyle>

<PagerStyle CssClass="grdpagerstyle" ForeColor="Black" HorizontalAlign="Center"></PagerStyle>

<HeaderStyle CssClass="grdheader" ForeColor="white" HorizontalAlign="Left" Font-Bold="True"></HeaderStyle>

<AlternatingRowStyle CssClass="grdAternaterow" Font-Size="10px"></AlternatingRowStyle>
</asp:GridView> </TD></TR></TBODY></TABLE></asp:Panel><TABLE style="WIDTH: 894px"><TBODY><TR><TD colSpan=3><asp:Label id="lblError" runat="server" Font-Size="8pt" Font-Names="Verdana" Font-Bold="True" Width="734px" Visible="False"></asp:Label></TD></TR><TR><TD style="WIDTH: 133px; HEIGHT: 23px"></TD><TD style="WIDTH: 101px; HEIGHT: 23px"></TD><TD style="HEIGHT: 23px" align=right></TD></TR><TR><TD style="WIDTH: 133px; HEIGHT: 23px"><TABLE style="BORDER-RIGHT: blue 1px solid; BORDER-TOP: blue 1px solid; BORDER-LEFT: blue 1px solid; WIDTH: 356px; BORDER-BOTTOM: blue 1px solid" class="td_cell" align=left><TBODY><TR><TD style="WIDTH: 100px">-1 Incl</TD><TD style="WIDTH: 100px">-2 N/Incl</TD><TD style="WIDTH: 100px">-3 Free</TD><TD style="WIDTH: 100px">-4 N/A</TD><TD style="WIDTH: 100px">-5 On Request</TD></TR></TBODY></TABLE></TD>
    <TD >
    <asp:CheckBox ID="chkapprove" runat="server" Font-Bold="False" Text="Approve/Unapprove"
         /></TD><TD><asp:Button id="BtnClearFormula" tabIndex=23 
                onclick="BtnClearFormula_Click" runat="server" Text="Clear Prices" 
                CssClass="field_button"></asp:Button>&nbsp; <asp:Button id="btnSave" tabIndex=21 
                onclick="btnSave_Click" runat="server" Text="Save" CssClass="field_button"></asp:Button>&nbsp;<asp:Button 
                id="btnCancel" tabIndex=22 onclick="btnCancel_Click" runat="server" 
                Text="Return to Search" CssClass="field_button"></asp:Button>&nbsp;<asp:Button 
                id="btnhelp" tabIndex=23 onclick="btnhelp_Click" runat="server" Text="Help" 
                Height="20px" CssClass="field_button"></asp:Button></TD></TR></TBODY></TABLE></TD></TR></TBODY></TABLE>
</ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

