<%@ Page Language="VB" MasterPageFile="~/SubPageMaster.master" AutoEventWireup="false" CodeFile="OtherServiceTypes.aspx.vb" Inherits="OtherServiceTypes"  %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
    <%@ OutputCache location="none" %> 
<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">
    <script language ="javascript" type="text/javascript" >

        function Validate(state) {
          
         if (state == 'New'|| state == 'Edit') 
         {
            var ddlcode = document.getElementById("<%=ddlOtherGrpCode.ClientID%>");
            var ddlname = document.getElementById("<%=ddlOtherGrpName.ClientID%>");
            var txtTrns = document.getElementById("<%=txtTrns.ClientID%>");
                if(document.getElementById("<%=txtCode.ClientID%>").value=='')
                {
                    document.getElementById("<%=txtCode.ClientID%>").focus();
                    alert('Please Enter Code');             
                    return false;
                } else if (document.getElementById("<%=txtName.ClientID%>").value == '')
                {
                    document.getElementById("<%=txtName.ClientID%>").focus();
                    alert('Please Enter Name');             
                    return false;
                }else if (ddlname.value == txtTrns.value || ddlcode.options[ddlcode.selectedIndex].text == txtTrns.value)
                {

                    if (document.getElementById("<%=ddlType.ClientID%>").value == '[Select]' || document.getElementById("<%=ddlType.ClientID%>").value == '') {
                        document.getElementById("<%=ddlType.ClientID%>").focus();
                        alert('Please Select Type');
                        return false;
                    }
                    if (document.getElementById("<%=ddlPName.ClientID%>").value == '[Select]' || document.getElementById("<%=ddlPName.ClientID%>").value == '') {
                        document.getElementById("<%=ddlPName.ClientID%>").focus();
                        alert('Please Select PickUp');
                        return false;
                    }
                    if (document.getElementById("<%=ddlDName.ClientID%>").value == '[Select]' || document.getElementById("<%=ddlDName.ClientID%>").value == '') {
                        document.getElementById("<%=ddlDName.ClientID%>").focus();
                        alert('Please Select Drop Off');
                        return false;
                    }               
                }
                else{
                    if (state == 'New') { if (confirm('Are you sure you want to save other types?') == false) return false; }
                    if (state == 'Edit') { if (confirm('Are you sure you want to update ?') == false) return false; }
                    if (state == 'Delete') { if (confirm('Are you sure you want to delete ?') == false) return false; }
                }

         }
    }

function checkNumber(e)
			{	    
			    	
				if ( (event.keyCode < 47 || event.keyCode > 57) )
				{
					return false;
	            }   
	         	
			}
function checkCharacter(e)
			{	    
			    if (event.keyCode == 32 || event.keyCode ==46)
			        return;			
				if ( (event.keyCode < 47 || event.keyCode > 57) && (event.keyCode < 65 || event.keyCode > 91) && (event.keyCode < 96 || event.keyCode > 122))
				{
					return false;
	            }   
	         	
			}
			
function  GetOtherGrpValueFrom()
{

	var ddl = document.getElementById("<%=ddlOtherGrpName.ClientID%>");
	ddl.selectedIndex = -1;
		// Iterate through all dropdown items.
            for (i = 0; i < ddl.options.length; i++) 
            {
                if (ddl.options[i].text == 
                document.getElementById("<%=ddlOtherGrpCode.ClientID%>").value)
                {
                    // Item was found, set the selected index.
                    ddl.selectedIndex = i;
                    showdiv();
                    return true;
                }
            }
            showdiv();
}
function  GetOtherGrpValueCode()
{
	var ddl = document.getElementById("<%=ddlOtherGrpCode.ClientID%>");
	ddl.selectedIndex = -1;
		// Iterate through all dropdown items.
		for (i = 0; i < ddl.options.length; i++) 
		{
			if (ddl.options[i].text == 
			document.getElementById("<%=ddlOtherGrpName.ClientID%>").value)
			{
				// Item was found, set the selected index.
			    ddl.selectedIndex = i;
			    showdiv();
				return true;
			}
}
showdiv();

}
function showdiv() {
    var ddlcode = document.getElementById("<%=ddlOtherGrpCode.ClientID%>");
    var ddlname = document.getElementById("<%=ddlOtherGrpName.ClientID%>");
    var txtTrns = document.getElementById("<%=txtTrns.ClientID%>");
    var ddlPName = document.getElementById("<%=ddlPName.ClientID%>");
    var ddlDName = document.getElementById("<%=ddlDName.ClientID%>");
    var ddlType = document.getElementById("<%=ddlType.ClientID%>");
    
    
        if (ddlname.value == txtTrns.value || ddlcode.options[ddlcode.selectedIndex].text == txtTrns.value) {
            ddlPName.disabled = false;
            ddlDName.disabled = false;
            ddlType.disabled = false;
    }
        else {
            ddlPName.disabled = true;
            ddlDName.disabled = true;
            ddlType.disabled = true;
            ddlType.value = '[Select]';
    }
    }
    function loadpickdropoff() {
        var ddlType = document.getElementById("<%=ddlType.ClientID%>");
        var connstr = document.getElementById("<%=txtconnection.ClientID%>");
        constr = connstr.value

        if (ddlType.value == '0') {

            ColServices.clsServices.GetSerPointName(constr, 'A', FillPickUpName, ErrorHandler, TimeOutHandler);
            ColServices.clsServices.GetSerPointName(constr, 'S', FillDropOffName, ErrorHandler, TimeOutHandler);
        }
        if (ddlType.value == '1') {
            ColServices.clsServices.GetSerPointName(constr, 'S', FillPickUpName, ErrorHandler, TimeOutHandler);
            ColServices.clsServices.GetSerPointName(constr, 'A', FillDropOffName, ErrorHandler, TimeOutHandler)

        }
        if (ddlType.value == '2') {
            ColServices.clsServices.GetSerPointName(constr, 'S', FillPickUpName, ErrorHandler, TimeOutHandler);
            ColServices.clsServices.GetSerPointName(constr, 'S', FillDropOffName, ErrorHandler, TimeOutHandler)
        }
        if (ddlType.value == '3') {
            ColServices.clsServices.GetSerPointName(constr, 'A', FillPickUpName, ErrorHandler, TimeOutHandler);
            ColServices.clsServices.GetSerPointName(constr, 'A', FillDropOffName, ErrorHandler, TimeOutHandler)
        }
    }
    function FillPickUpName(result) {
        var ddl = document.getElementById("<%=ddlPName.ClientID%>");
        RemoveAll(ddl)
        for (var i = 0; i < result.length; i++) {
            var option = new Option(result[i].ListText, result[i].ListValue);
            ddl.options.add(option);
        }
        ddl.value = "[Select]";
    }
   function FillDropOffName(result) {
        var ddl = document.getElementById("<%=ddlDName.ClientID%>");
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
    function setvalue() {

        var ddlPName = document.getElementById("<%=ddlPName.ClientID%>");
        var ddlDName = document.getElementById("<%=ddlDName.ClientID%>");
        var hdnP = document.getElementById("<%=hdnP.ClientID%>");
        var hdnD = document.getElementById("<%=hdnD.ClientID%>");
        hdnP.value = ddlPName.value;
        hdnD.value = ddlDName.value;
    }

			
</script> 
    <asp:UpdatePanel id="UpdatePanel1" runat="server">
        <contenttemplate>
<TABLE style="BORDER-RIGHT: gray 2px solid; BORDER-TOP: gray 2px solid; BORDER-LEFT: gray 2px solid; WIDTH: 714px; BORDER-BOTTOM: gray 2px solid; TEXT-ALIGN: left">
<TBODY><TR><TD style="HEIGHT: 3px; TEXT-ALIGN: center" class="td_cell" colSpan=4>
<asp:Label id="lblHeading" runat="server" Text="Add New Other Service Types" 
        ForeColor="White" __designer:wfdid="w17" Width="100%" CssClass="field_heading"></asp:Label></TD></TR>
<TR style="COLOR: #ff0000"><TD style="WIDTH: 100%; HEIGHT: 4px" class="td_cell"><SPAN style="COLOR: #000000">Code</SPAN> <SPAN style="COLOR: #ff0000" class="td_cell">*</SPAN></TD>
<TD style="WIDTH: 2px; COLOR: #000000; HEIGHT: 4px"><INPUT style="WIDTH: 226px" id="txtCode" class="field_input" tabIndex=1 type=text maxLength=20 runat="server" />
<SPAN style="COLOR: #ff0000"></SPAN></TD><TD style="WIDTH: 3px; COLOR: #000000; HEIGHT: 4px"></TD><TD style="WIDTH: 4px; COLOR: #000000; HEIGHT: 4px">
    <INPUT style="WIDTH: 81px; TEXT-ALIGN: right; visibility:hidden" id="txtTrns" 
            tabIndex=0 type=text 
                    runat="server" class="field_input" />
    </TD></TR>
<TR style="COLOR: #ff0000"><TD style="HEIGHT: 1px" class="td_cell"><SPAN style="COLOR: #000000">Name</SPAN> <SPAN class="td_cell">*</SPAN></TD>
<TD style="WIDTH: 2px; COLOR: #ff0000; HEIGHT: 1px"><INPUT style="WIDTH: 226px" id="txtName" class="field_input" tabIndex=2 type=text maxLength=100 runat="server" /></TD>
<TD style="WIDTH: 3px; COLOR: #ff0000; HEIGHT: 1px"></TD><TD style="WIDTH: 4px; COLOR: #ff0000; HEIGHT: 1px">
    <INPUT style="WIDTH: 81px; TEXT-ALIGN: right; visibility: hidden;" 
            id="txtconnection" tabIndex=7 type=text 
                    runat="server" class="field_input" />
    </TD></TR><TR><TD style="HEIGHT: 22px" class="td_cell">Group Code &nbsp;
<SPAN style="COLOR: red" class="td_cell">*&nbsp;&nbsp;</SPAN></TD><TD style="WIDTH: 2px; HEIGHT: 22px" class="td_cell" align=left><SPAN style="COLOR: #ff0000">
<SELECT style="WIDTH: 232px" id="ddlOtherGrpCode" class="field_input" tabIndex=3 onchange="GetOtherGrpValueFrom()" runat="server"> <OPTION selected></OPTION></SELECT></SPAN></TD>
<TD style="WIDTH: 3px; HEIGHT: 22px" class="td_cell" align=left>Group&nbsp;Name&nbsp;&nbsp;</TD><TD style="WIDTH: 4px; HEIGHT: 22px" class="td_cell" align=left>
<SELECT style="WIDTH: 315px" id="ddlOtherGrpName" class="field_input" tabIndex=4 onchange="GetOtherGrpValueCode()" runat="server"> <OPTION selected></OPTION></SELECT></TD></TR><TR>
<TD  class="td_cell" style="HEIGHT: 22px">
    Transfer type</TD>
    <td align="left" class="td_cell" style="WIDTH: 2px; HEIGHT: 22px">
    <select ID="ddlType" runat="server" class="fiel_input" onchange="loadpickdropoff()"  style="WIDTH: 232px">
              <option selected ="selected"  value="[Select]">[Select]</option>
              <option value="0">Arrival Borders</option>
              <option value="1">Departure Borders</option>
              <option value="2">Internal Transfer/Excursion</option>
              <option value="3">Arrival/Departure Transfer Borders</option>
             </select>
    </td>
    <td align="left" class="td_cell" style="WIDTH: 3px; HEIGHT: 22px">
        &nbsp;</td>
    <td align="left" class="td_cell" style="WIDTH: 4px; HEIGHT: 22px">
        &nbsp;</td>
    </TR>
    <tr>
        <td class="td_cell" style="HEIGHT: 22px">
            Pick Up Point</td>
        <td align="left" class="td_cell" style="WIDTH: 2px; HEIGHT: 22px">
            <span style="COLOR: #ff0000">
            <select ID="ddlPName" runat="server" class="field_input" name="D2" onchange="setvalue();"
                style="WIDTH: 232px" tabindex="3">
                <option selected=""></option>
            </select></span></td>
        <td align="left" class="td_cell" style="WIDTH: 3px; HEIGHT: 22px">
            &nbsp;</td>
        <td align="left" class="td_cell" style="WIDTH: 4px; HEIGHT: 22px">
            <asp:HiddenField ID="hdnP" runat="server" />
        </td>
    </tr>
    <tr>
        <td class="td_cell" style="HEIGHT: 22px">
            Drop Off Point</td>
        <td align="left" class="td_cell" style="WIDTH: 2px; HEIGHT: 22px">
            <span style="COLOR: #ff0000">
            <select ID="ddlDName" runat="server" class="field_input" name="D4" onchange="setvalue();"
                style="WIDTH: 232px" tabindex="3">
                <option selected=""></option>
            </select></span></td>
        <td align="left" class="td_cell" style="WIDTH: 3px; HEIGHT: 22px">
            &nbsp;</td>
        <td align="left" class="td_cell" style="WIDTH: 4px; HEIGHT: 22px">
            <asp:HiddenField ID="hdnD" runat="server" />
        </td>
    </tr>
<TR><TD style="HEIGHT: 4px" class="td_cell">Order <span style="COLOR: #ff0000">*</span> </TD>
    <td style="HEIGHT: 4px; width: 2px;">
        <INPUT style="WIDTH: 226px; TEXT-ALIGN: right" id="txtOrder" tabIndex=5 
        type=text runat="server" class="field_input" />
    </td>
    <td style="WIDTH: 3px; HEIGHT: 4px">
    </td>
    <td style="WIDTH: 4px; HEIGHT: 4px">
        &nbsp;</td>
    </TR><TR><TD style="HEIGHT: 3px; " class="td_cell">
        Remark&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        </TD>
        <td colspan="3" style="HEIGHT: 3px">
            <asp:TextBox ID="txtRemark" runat="server" __designer:wfdid="w20" 
                CssClass="field_input" Height="36px" tabIndex="6" TextMode="MultiLine" 
                Width="538px"></asp:TextBox>
        </td>
    </TR>
    <tr>
        <td style="HEIGHT: 20px; width: 74px;" class="td_cell" colspan="4">
            <table border="0" style="WIDTH: 438px">
                <tbody>
                    <tr>
                        <td style="WIDTH: 58px; HEIGHT: 1px">
                            Min Pax <span style="COLOR: #ff0000">*</span>
                        </td>
                        <td style="WIDTH: 1px; HEIGHT: 1px">
                            <INPUT style="WIDTH: 81px; TEXT-ALIGN: right" id="txtMinPax" tabIndex=7 type=text 
                    runat="server" class="field_input" />
                        </td>
                        <td colspan="2" style="WIDTH: 193px; HEIGHT: 1px">
                            <INPUT id="ChkPakReq" tabIndex=8 type=checkbox CHECKED runat="server" />
                            Pax Check Required</td>
                    </tr>
                    <tr>
                        <td colspan="2" style="HEIGHT: 1px">
                            <INPUT id="ChkInactive" tabIndex=9 type=checkbox CHECKED runat="server" />
                            active</td>
                        <td colspan="2" style="WIDTH: 193px; HEIGHT: 1px">
                            <INPUT id="ChkPrnRemark" tabIndex=10 type=checkbox CHECKED runat="server" />
                            &nbsp;Print Remark</td>
                    </tr>
                    <tr>
                        <td colspan="2" style="HEIGHT: 4px">
                            <INPUT id="ChkPrnConfirm" tabIndex=11 type=checkbox CHECKED runat="server" />
                            Print in Confirmation</td>
                        <td colspan="2" style="WIDTH: 193px; HEIGHT: 4px">
                            <INPUT id="ChkAutoCancel" tabIndex=12 type=checkbox CHECKED runat="server" />
                            &nbsp;Auto Cancellation Required</td>
                    </tr>
                </tbody>
            </table>
        </td>
    </tr>
    <tr>
        <td style="HEIGHT: 3px">
            <asp:Button ID="btnSave" runat="server" CssClass="field_button" 
                onclick="btnSave_Click" tabIndex="13" Text="Save" />
        </td>
        <td style="WIDTH: 237px">
            <asp:Button ID="btnCancel" runat="server" CssClass="field_button" 
                onclick="btnCancel_Click" tabIndex="14" Text="Return To Search" />
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Button ID="btnhelp" runat="server" __designer:wfdid="w8" 
                CssClass="field_button" onclick="btnhelp_Click" tabIndex="15" Text="Help" />
        </td>
    </tr>
    </TBODY></TABLE>
</contenttemplate>
    </asp:UpdatePanel>
    <asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server">
             <Services>
                 <asp:ServiceReference Path="~/clsServices.asmx" />
             </Services>
         </asp:ScriptManagerProxy>
</asp:Content>

