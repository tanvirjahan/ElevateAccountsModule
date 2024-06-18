<%@ Page Language="VB" MasterPageFile="~/SubPageMaster.master" AutoEventWireup="false" CodeFile="OtherServicesPriceList2.aspx.vb" Inherits="OtherServicesPriceList2"  %>

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
<script type="text/javascript">
<!--
//WebForm_AutoFocus('ctl00_SampleContent_WUCCamperMenuOptionsAsTab2_TabStCamperOptions_TbCamperLkp_WUCCamperLookup2_WUCCamperInquiry1_TBLastName');
// -->
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

function checkTelephoneNumber(e)
			{	    
			    	
				if ( (event.keyCode < 45 || event.keyCode > 57) )
				{
					return false;
	            }   
	         	
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

    var dt1=document.getElementById('<%=dpFromDate.ClientID%>');


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
        txtBox = rowElement.cells[ccount-2].firstChild;
        txtBox.value = dt1.value;
    }     

}
			</script>
    <asp:UpdatePanel id="UpdatePanel1" runat="server">
        <contenttemplate>
<TABLE style="BORDER-RIGHT: gray 2px solid; BORDER-TOP: gray 2px solid; BORDER-LEFT: gray 2px solid; BORDER-BOTTOM: gray 2px solid" align=left><TBODY><TR><TD class="field_heading" align=center><asp:Label id="lblHeading" runat="server" Text="Other Services Price List" CssClass="field_heading" Width="358px"></asp:Label></TD></TR><TR><TD style="WIDTH: 1049px; HEIGHT: 209px"><TABLE style="WIDTH: 742px"><TBODY><TR><TD style="WIDTH: 174px" class="td_cell" align=left>PL Code</TD><TD style="WIDTH: 183px; HEIGHT: 22px"><INPUT style="WIDTH: 194px" id="txtPlcCode" class="field_input" disabled tabIndex=1 type=text runat="server" /></TD><TD style="WIDTH: 156px; HEIGHT: 22px" class="td_cell" align=left></TD><TD></TD></TR><TR>
<TD style="WIDTH: 174px" class="td_cell" align=left>Market Code</TD><TD style="WIDTH: 183px; HEIGHT: 22px"><SELECT style="WIDTH: 200px" id="ddlMarketCode" class="field_input" tabIndex=2 onchange="CallWebMethod('marketcode');" runat="server"> <OPTION selected></OPTION></SELECT></TD><TD style="WIDTH: 156px; HEIGHT: 22px" class="td_cell" align=left>Market Name</TD>
<TD><SELECT style="WIDTH: 300px" id="ddlMarketName" class="field_input" tabIndex=3 onchange="CallWebMethod('marketname');" runat="server"> <OPTION selected></OPTION></SELECT></TD></TR><TR><TD style="WIDTH: 174px" class="td_cell" align=left>Sell Type Code</TD>
<TD style="WIDTH: 183px; HEIGHT: 22px"><SELECT style="WIDTH: 200px" id="ddlSellTypeCode" class="field_input" tabIndex=4 onchange="CallWebMethod('SellTypeCode');" runat="server"> <OPTION selected></OPTION></SELECT></TD><TD style="WIDTH: 156px; HEIGHT: 22px" class="td_cell" align=left>Sell Type Name</TD><TD><SELECT style="WIDTH: 300px" id="ddlSellTypeName" class="field_input" tabIndex=5 onchange="CallWebMethod('SellTypeName');" runat="server"> <OPTION selected></OPTION></SELECT></TD></TR><TR><TD style="WIDTH: 174px" class="td_cell" align=left>Group Code</TD><TD style="WIDTH: 183px"><SELECT style="WIDTH: 200px" id="ddlGroupCode" class="field_input" tabIndex=6 onchange="CallWebMethod('GroupCode');" runat="server"> <OPTION selected></OPTION></SELECT></TD><TD style="WIDTH: 156px" class="td_cell" align=left>Group Name</TD><TD><SELECT style="WIDTH: 300px" id="ddlGroupName" class="field_input" tabIndex=7 onchange="CallWebMethod('GroupName');" runat="server"> <OPTION selected></OPTION></SELECT></TD></TR>
<TR><TD style="WIDTH: 174px" class="td_cell" align=left>Currency Code</TD><TD style="WIDTH: 183px"><asp:TextBox id="txtCurrCode" tabIndex=8 runat="server" CssClass="field_input" Width="194px"></asp:TextBox></TD><TD style="WIDTH: 156px" class="td_cell" align=left>Currency Name</TD><TD><asp:TextBox id="txtCurrName" tabIndex=9 runat="server" CssClass="field_input" Width="294px"></asp:TextBox></TD></TR><TR><TD style="WIDTH: 174px" class="td_cell" align=left>Sub&nbsp;Season&nbsp;Code</TD><TD style="WIDTH: 183px; HEIGHT: 22px"><SELECT style="WIDTH: 200px" id="ddlSubSeasCode" class="field_input" tabIndex=10 onchange="CallWebMethod('SeasCode');" runat="server"> <OPTION selected></OPTION></SELECT></TD><TD style="WIDTH: 156px; HEIGHT: 22px" class="td_cell" align=left>Sub Season Name</TD><TD><SELECT style="WIDTH: 300px" id="ddlSubSeasName" class="field_input" tabIndex=11 onchange="CallWebMethod('SeasName');" runat="server"> <OPTION selected></OPTION></SELECT></TD></TR><TR><TD style="WIDTH: 174px; HEIGHT: 27px" class="td_cell" align=left>From Date</TD>
<TD style="WIDTH: 183px; HEIGHT: 27px">
    <asp:TextBox ID="dpFromDate" runat="server" CssClass="fiel_input" Height="18px" 
        Width="103px" ></asp:TextBox>
    <cc1:CalendarExtender ID="dpFromDate_CalendarExtender" runat="server" 
        Enabled="True" Format="dd/MM/yyyy" PopupButtonID="btnCal1" 
        TargetControlID="dpFromDate" onclienthidden="changedate1">
    </cc1:CalendarExtender>
    <asp:Button ID="btnCal1" runat="server" Height="23px" Width="25px" />
    </TD><TD style="WIDTH: 156px; HEIGHT: 27px" class="td_cell" align=left>To Date</TD><TD style="HEIGHT: 27px">
    <asp:TextBox ID="dpToDate" runat="server" CssClass="fiel_input" Height="16px" 
        Width="91px"></asp:TextBox>
    <cc1:CalendarExtender ID="dpToDate_CalendarExtender" runat="server" 
        Enabled="True" Format="dd/MM/yyyy" PopupButtonID="btnCal2" 
        TargetControlID="dpToDate" onclienthidden="changedate2">
    </cc1:CalendarExtender>
    <asp:Button ID="btnCal2" runat="server" Height="23px" Width="25px" />
    </TD></TR><TR><TD style="WIDTH: 174px" class="td_cell" align=left>Remark</TD><TD style="WIDTH: 183px" align=left colSpan=3><TEXTAREA style="WIDTH: 607px; HEIGHT: 59px" id="txtRemark" class="field_input" disabled tabIndex=14 runat="server"></TEXTAREA></TD></TR><TR><TD style="WIDTH: 174px" class="td_cell" align=left>Active</TD><TD style="WIDTH: 183px" align=left colSpan=3><INPUT id="ChkActive" tabIndex=15 type=checkbox CHECKED runat="server" /></TD></TR>
<TR><TD style="WIDTH: 174px; HEIGHT: 15px" class="td_cell" align=left></TD><TD style="WIDTH: 183px; HEIGHT: 15px"></TD><TD style="WIDTH: 156px; HEIGHT: 15px" class="td_cell" align=left></TD><TD style="HEIGHT: 15px" align=right>&nbsp;</TD></TR></TBODY></TABLE><asp:Panel id="Panel1" runat="server" Font-Size="8pt" Font-Names="Verdana" Height="500px" Font-Bold="False" Width="900px" ScrollBars="Auto" GroupingText="Enter Rates"><TABLE><TBODY><TR><TD style="WIDTH: 100px"><asp:GridView id="gv_SearchResult" tabIndex=16 runat="server" Font-Size="10px" BackColor="White" Width="1px" CssClass="td_cell" AutoGenerateColumns="False" BorderColor="#999999" BorderStyle="None" BorderWidth="1px" CellPadding="3" GridLines="Vertical">
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
</asp:GridView> </TD></TR></TBODY></TABLE></asp:Panel> <asp:Label id="lblError" runat="server" Font-Size="8pt" Font-Names="Verdana" Font-Bold="True" Width="734px" Visible="False"></asp:Label> <TABLE style="WIDTH: 884px"><TBODY><TR><TD style="WIDTH: 133px"></TD><TD style="WIDTH: 100px">
    <asp:CheckBox ID="chkshowweb" runat="server" Font-Bold="False" Text="Don't show in Web"
        Width="140px" /></TD><TD align=right></TD></TR><TR><TD style="WIDTH: 133px"><TABLE style="BORDER-RIGHT: blue 1px solid; BORDER-TOP: blue 1px solid; BORDER-LEFT: blue 1px solid; WIDTH: 356px; BORDER-BOTTOM: blue 1px solid" class="td_cell" align=left><TBODY><TR><TD style="WIDTH: 100px">-1 Incl</TD><TD style="WIDTH: 100px">-2 N/Incl</TD><TD style="WIDTH: 100px">-3 Free</TD><TD style="WIDTH: 100px">-4 N/A</TD><TD style="WIDTH: 100px">-5 On Request</TD></TR></TBODY></TABLE></TD><TD style="WIDTH: 100px">
    <asp:CheckBox ID="chkapprove" runat="server" Font-Bold="False" Text="Approve/Unapprove"
        Width="146px" /></TD><TD align=right><asp:Button id="BtnClearFormula" tabIndex=17 onclick="BtnClearFormula_Click" runat="server" Text="Clear Prices" CssClass="field_button"></asp:Button>&nbsp;
        <asp:Button id="btnSave" tabIndex=18 onclick="btnSave_Click" runat="server" 
                Text="Save" CssClass="field_button"></asp:Button>
        &nbsp; <asp:Button id="btnCancel" tabIndex=19 onclick="btnCancel_Click" 
                runat="server" Text="Return to Search" CssClass="field_button"></asp:Button>
        &nbsp; <asp:Button id="btnhelp" tabIndex=8 onclick="btnhelp_Click" runat="server" 
                Text="Help" CssClass="field_button"></asp:Button></TD></TR></TBODY></TABLE></TD></TR></TBODY></TABLE>&nbsp; <BR />
</contenttemplate>
    </asp:UpdatePanel>
</asp:Content>

