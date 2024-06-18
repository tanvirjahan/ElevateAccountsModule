<%@ Page Language="VB" MasterPageFile="~/SubPageMaster.master" AutoEventWireup="false" CodeFile="CustSalesDet.aspx.vb" Inherits="CustSalesDet"  %>

<%@ Register Src="SubMenuUserControl.ascx" TagName="SubMenuUserControl" TagPrefix="uc1" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
    <%@ OutputCache location="none" %> 
<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">

<script type="text/javascript">
<!--
//WebForm_AutoFocus('ctl00_SampleContent_WUCCamperMenuOptionsAsTab2_TabStCamperOptions_TbCamperLkp_WUCCamperLookup2_WUCCamperInquiry1_TBLastName');
// -->
</script>
<script language="javascript" type="text/javascript" >
        
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
			
		
function checkNumber(e)
{
    if ( event.keyCode < 45 || event.keyCode > 57 )
    {
    return false;
    }
}



function FormValidationMainDetail(state) {
    if ((document.getElementById("<%=txtSaleTelephone1.ClientID%>").value == "")) {
        alert("Enter Telephone.");
        document.getElementById("<%=txtSaleTelephone1.ClientID%>").focus();
        return false;
    }
  
    else {
        if (state == 'New') { if (confirm('Are you sure you want to save customer Sales details?') == false) return false; }
        if (state == 'Edit') { if (confirm('Are you sure you want to update customer Sales details?') == false) return false; }
     }
}


</script>

    <asp:UpdatePanel id="UpdatePanel2" runat="server">
        <contenttemplate>
<TABLE style="BORDER-RIGHT: gray 2px solid; BORDER-TOP: gray 2px solid; BORDER-LEFT: gray 2px solid; WIDTH: 800px; 
    BORDER-BOTTOM: gray 2px solid" class="td_cell" align=left>
<TBODY>
<TR><TD vAlign=top align=center width=150 colSpan=2><asp:Label id="lblHeading" 
        runat="server" Text="Customers" CssClass="field_heading" Width="729px" 
        ForeColor="White"></asp:Label></TD></TR>
<TR><TD style="WIDTH: 15%" vAlign=top align=left><SPAN style="COLOR: #ff0000" class="td_cell"></SPAN></TD><TD style="WIDTH: 85%" class="td_cell" vAlign=top align=left>Code <SPAN style="COLOR: #ff0000" class="td_cell">*&nbsp; </SPAN><INPUT style="WIDTH: 196px" id="txtCustomerCode" class="field_input" tabIndex=1 type=text maxLength=20 runat="server" /> &nbsp; Name&nbsp; <INPUT style="WIDTH: 213px" id="txtCustomerName" class="field_input" tabIndex=2 type=text maxLength=100 runat="server" /></TD></TR>
<TR><TD style="WIDTH: 15%" vAlign=top align=left>&nbsp;<uc1:SubMenuUserControl id="SubMenuUserControl1" runat="server"></uc1:SubMenuUserControl> </TD><TD vAlign=top>
    <DIV style="WIDTH: 547px" id="iframeINF" runat="server"><asp:UpdatePanel id="UpdatePanel1" runat="server"><ContentTemplate>
        <asp:Panel id="PanelSales" runat="server" Width="540px" 
            GroupingText="Sales Details"><TABLE style="WIDTH: 412px" border=0><TBODY><TR><TD style="WIDTH: 100px" align=left><SPAN style="FONT-SIZE: 8pt; FONT-FAMILY: Arial">Recommended By</SPAN></TD><TD style="WIDTH: 100px" align=left>
    <INPUT style="WIDTH: 295px" id="txtSaleRecommended" class="field_input" tabIndex=1 type=text maxLength=50 runat="server" /></TD></TR><TR><TD style="WIDTH: 100px" align=left><SPAN style="FONT-SIZE: 8pt; FONT-FAMILY: Arial">Telephone
    <span class="td_cell" style="COLOR: #ff0000">*&nbsp;</span></SPAN></TD>
    <TD style="WIDTH: 100px" align=left>
        <INPUT style="WIDTH: 295px" id="txtSaleTelephone1" class="field_input" tabIndex=2 type=text maxLength=50 runat="server" /></TD></TR>
    <TR><TD style="WIDTH: 100px" align=left></TD><TD style="WIDTH: 100px" align=left>
        <INPUT style="WIDTH: 295px" id="txtSaleTelephone2" class="field_input" tabIndex=3 type=text maxLength=50 runat="server" /></TD></TR><TR><TD style="WIDTH: 100px" align=left> <SPAN style="FONT-SIZE: 8pt; FONT-FAMILY: Arial">Mobile no.</SPAN> </TD><TD style="WIDTH: 100px" align=left>
        <INPUT style="WIDTH: 295px" id="txtsalesmob" class="field_input" tabIndex=4 type=text maxLength=50 runat="server" /></TD></TR><TR><TD style="WIDTH: 100px" align=left><SPAN style="FONT-SIZE: 8pt; FONT-FAMILY: Arial">Fax</SPAN></TD><TD style="WIDTH: 100px" align=left>
        <INPUT style="WIDTH: 295px" id="txtSaleFax" class="field_input" tabIndex=5 type=text maxLength=50 runat="server" /></TD></TR><TR><TD style="WIDTH: 100px" align=left></TD><TD style="WIDTH: 100px" align=left></TD></TR><TR><TD style="WIDTH: 100px" align=left><SPAN style="FONT-SIZE: 8pt; FONT-FAMILY: Arial">Contact</SPAN></TD><TD style="WIDTH: 100px" align=left>
    <INPUT style="WIDTH: 295px" id="txtSaleContact1" class="field_input" tabIndex=6 type=text maxLength=100 runat="server" /></TD></TR><TR><TD style="WIDTH: 100px" align=left></TD><TD style="WIDTH: 100px" align=left>
        <INPUT style="WIDTH: 295px" id="txtSaleContact2" class="field_input" tabIndex=7 type=text maxLength=100 runat="server" /></TD></TR><TR><TD style="WIDTH: 100px; HEIGHT: 26px" align=left><SPAN style="FONT-SIZE: 8pt; FONT-FAMILY: Arial">E-mail</SPAN></TD><TD style="WIDTH: 100px; HEIGHT: 26px" align=left>
        <INPUT style="WIDTH: 295px" id="txtSaleEmail" class="field_input" tabIndex=8 type=text maxLength=100 runat="server" /></TD></TR><TR><TD style="WIDTH: 100px" align=left></TD><TD style="WIDTH: 100px" align=left></TD></TR>
    <!--conceirge-->
    <tr>
<td class="td_cell"><SPAN style="FONT-SIZE: 8pt; FONT-FAMILY: Arial">Concierge for
    <br />
    Excursions </SPAN> </td>
<td><select style="width:200px" id="ddlConcierge" class="field_input" 
        runat="server" tabindex="9" > <option selected="selected"></option></select></td>
</tr>
<tr>
<td class="td_cell"> <SPAN style="FONT-SIZE: 8pt; FONT-FAMILY: Arial">Sales Expert for
    <br />
    Excursions </SPAN> </td>
<td><select style="width:200px" id="ddlSalesExpert" class="field_input" 
        runat="server" tabindex="10" > <option selected="selected"></option></select></td>
</tr>
<tr><td></td></tr>
<tr> 
    <td align="left" style="WIDTH: 100px">
        <asp:Button ID="BtnSaleSave" runat="server" __designer:wfdid="w10" 
            CssClass="field_button" tabIndex="11" Text="Save" />
    </td>
    <td  style="WIDTH: 230px">
        <asp:Button ID="BtnSaleCancel" runat="server" __designer:wfdid="w11" 
            CssClass="field_button"  tabIndex="12" 
            Text="Return to Search"/>
     
        <asp:Button ID="btnHelp" runat="server" __designer:dtid="1688858450198528" 
            __designer:wfdid="w1" onclick="btnHelp_Click" CssClass="field_button" 
            tabIndex="13" Text="Help" />
    </td>
    </tr>
    <TR><TD style="WIDTH: 100px" align=left>
        </TD>
        <TD style="WIDTH: 100px" align=left></TD></TR>
        <TR><TD style="WIDTH: 100px" align=left></TD><TD style="WIDTH: 100px" align=left></TD></TR>
        </TBODY></TABLE></asp:Panel> 
</ContentTemplate>
</asp:UpdatePanel> </DIV></TD></TR><tr><td><asp:Label ID="lblwebserviceerror" runat="server"  style="display:none" Text="Webserviceerror"></asp:Label></td></tr></TBODY></TABLE>
</contenttemplate>
    </asp:UpdatePanel>
    <asp:ScriptManagerProxy id="ScriptManagerProxy1" runat="server">
        <services>
<asp:ServiceReference Path="~/clsServices.asmx"></asp:ServiceReference>
</services>
    </asp:ScriptManagerProxy>
</asp:Content>

