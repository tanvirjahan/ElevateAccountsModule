<%@ Page Language="VB" MasterPageFile="~/SubPageMaster.master" AutoEventWireup="false" CodeFile="ExcursionTypes.aspx.vb" Inherits="Other_Services_Selling_Types"  %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
    <%@ OutputCache location="none" %> 
<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">
<script language ="javascript " type ="text/javascript" >
function checkNumber()
{
   if (event.keyCode < 45 || event.keyCode > 57)
    {
         return false;
    }
}

function FormValidation(state)
{
if ((document.getElementById("<%=txtcode.ClientID%>").value=="")||(document.getElementById("<%=txtname.ClientID%>").value=="")||(document.getElementById("<%=ddlgpcode.ClientID%>").value=="[Select]"))
{
    if (document.getElementById("<%=txtcode.ClientID%>").value=="")
    {
           document.getElementById("<%=txtcode.ClientID%>").focus(); 
             alert("Code field can not be blank");
            return false;
     }
     else if (document.getElementById("<%=txtname.ClientID%>").value=="") 
     {
           document.getElementById("<%=txtname.ClientID%>").focus();
           alert("Name field can not be blank");
            return false;
     }
     else if (document.getElementById("<%=ddlgpname.ClientID%>").value=="[Select]") 
     {
           document.getElementById("<%=ddlgpcode.ClientID%>").focus();
           alert("Select Currency Code");
            return false;
     }
             
 }
 else
 {
       if (state=='New'){if(confirm('Are you sure you want to Save Handling Fees Selling Types?')==false)return false;}
       if (state=='Edit'){if(confirm('Are you sure you want to Update Handling Fees Selling Types?')==false)return false;}
       if (state == 'Delete') { if (confirm('Are you sure you want to Delete Handling Fees Selling Types?') == false) return false; }
 }
}  
   function  GetValueFrom()
{

	var ddl = document.getElementById("<%=ddlgpname.ClientID%>");
	ddl.selectedIndex = -1;
		// Iterate through all dropdown items.
		for (i = 0; i < ddl.options.length; i++) 
		{
			if (ddl.options[i].text == 
			document.getElementById("<%=ddlgpcode.ClientID%>").value)
			{
				// Item was found, set the selected index.
				ddl.selectedIndex = i;
				return true;
			}
		}
}

function getpartyname() {

    var ddl = document.getElementById("<%=ddlpartyname.ClientID%>");
    ddl.selectedIndex = -1;
    // Iterate through all dropdown items.
    for (i = 0; i < ddl.options.length; i++) {
        if (ddl.options[i].text ==
			document.getElementById("<%=ddlpartycode.ClientID%>").value) {
            // Item was found, set the selected index.
            ddl.selectedIndex = i;
            return true;
        }
    }
}


function  GetValueCode()
{
	var ddl = document.getElementById("<%=ddlgpcode.ClientID%>");
	ddl.selectedIndex = -1;
		// Iterate through all dropdown items.
		for (i = 0; i < ddl.options.length; i++) 
		{
			if (ddl.options[i].text == 
			document.getElementById("<%=ddlgpname.ClientID%>").value)
			{
				// Item was found, set the selected index.
				ddl.selectedIndex = i;
				return true;
			}
		}
}


function CallWebMethod(methodType) {
    switch (methodType) {


        case "acccode":
            var select = document.getElementById("<%=ddlAccCode.ClientId%>");
            var selectname = document.getElementById("<%=ddlAccName.ClientId%>");
            selectname.value = select.options[select.selectedIndex].text;
            break;
        case "accname":
            var select = document.getElementById("<%=ddlAccName.ClientId%>");
            var selectname = document.getElementById("<%=ddlAccCode.ClientId%>");
            selectname.value = select.options[select.selectedIndex].text;
            break;
        case "accrualcode":
            var select = document.getElementById("<%=ddlAccrualCode.ClientId%>");
            var selectname = document.getElementById("<%=ddlAccrualname.ClientId%>");
            selectname.value = select.options[select.selectedIndex].text;
            break;
        case "accrualname":
            var select = document.getElementById("<%=ddlAccrualname.ClientId%>");
            var selectname = document.getElementById("<%=ddlAccrualCode.ClientId%>");
            selectname.value = select.options[select.selectedIndex].text;
            break;


    }
}

function getpartycode() {
    var ddl = document.getElementById("<%=ddlpartycode.ClientID%>");
    ddl.selectedIndex = -1;
    // Iterate through all dropdown items.
    for (i = 0; i < ddl.options.length; i++) {
        if (ddl.options[i].text ==
			document.getElementById("<%=ddlpartyname.ClientID%>").value) {
            // Item was found, set the selected index.
            ddl.selectedIndex = i;
            return true;
        }
    }
}
   
</script>
    <asp:UpdatePanel id="UpdatePanel1" runat="server">
     <Triggers> 
        <asp:PostBackTrigger ControlID="btnSave" /> 
    </Triggers> 
        <contenttemplate>
<TABLE style="BORDER-RIGHT: gray 2px solid; BORDER-TOP: gray 2px solid; BORDER-LEFT: gray 2px solid; WIDTH: 656px; BORDER-BOTTOM: gray 2px solid"><TBODY><TR><TD class="td_cell" align=center colSpan=4>
    <asp:Label id="lblHeading" runat="server" Text="Add New Excursion Types" 
        CssClass="field_heading" Width="646px" Height="18px"></asp:Label></TD>

    </TR>
    <TR style="COLOR: #ff0000">
    <TD style="WIDTH: 545px; height: 24px;" class="td_cell"><SPAN style="COLOR: #000000">Code </SPAN><SPAN style="COLOR: #ff0000" class="td_cell">*</SPAN></TD>
    <TD style="COLOR: #000000; height: 24px; width: 504px;">
        <INPUT style="WIDTH: 196px" id="txtcode" class="txtbox" tabIndex=1 type=text maxLength=150 runat="server" /></TD>
        <td style="height: 24px; width: 466px;"></td></td></TR><TR>
    <TD style="WIDTH: 545px; HEIGHT: 23px" class="td_cell">
    Name <SPAN style="COLOR: #ff0000" class="td_cell">*</SPAN></TD>
    <TD style="COLOR: #000000; width: 504px; height: 23px;">
        <INPUT style="WIDTH: 406px" id="txtname" class="txtbox" tabIndex=2 type=text maxLength=150 runat="server" /> </TD>
    <td style="height: 23px; width: 466px"></td></TR><TR>
    <TD style="WIDTH: 545px; HEIGHT: 22px" class="td_cell">Group Code</TD>
   
    <TD style="HEIGHT: 22px; width: 504px;" align=left><SELECT 
            onchange="GetValueFrom()" style="WIDTH: 197px" id="ddlgpcode" class="drpdown" 
            tabIndex=3 runat="server"> <OPTION selected></OPTION></SELECT></td> 

             <td class="td_cell" style="width: 466px; height: 29px;">
        Group Name</td>
        <td style="height: 22px; width: 294px;">
        <SELECT 
            onchange="GetValueCode()" style="WIDTH: 201px" id="ddlgpname" class="drpdown" 
            tabIndex=4 runat="server"> <OPTION selected></OPTION></SELECT></TD></TR>
     <tr>
         <td class="td_cell" style="WIDTH: 545px; HEIGHT: 22px">
             Party Code</td>
         <td align="left" style="HEIGHT: 22px; width: 504px;">
             <select id="ddlpartycode" runat="server" class="drpdown" name="D1" 
                 onchange="getpartyname()" style="WIDTH: 197px" tabindex="3">
                 <option selected=""></option>
             </select>
         </td>
         <td class="td_cell" style="width: 466px; height: 29px;">
             Party&nbsp; Name</td>
         <td style="height: 22px; width: 294px;">
             <select id="ddlpartyname" runat="server" class="drpdown" name="D2" 
                 onchange="getpartycode()" style="WIDTH: 201px" tabindex="4">
                 <option selected=""></option>
             </select>
         </td>
    </tr>
    <tr>
        <td class="td_cell" style="WIDTH: 545px; HEIGHT: 29px">
            Attention</td>
        <td align="left" style="HEIGHT: 29px; width: 504px;">
            <INPUT style="WIDTH: 198px; height: 28px;" id="txtattention" class="txtbox" tabIndex=2 type=text 
            maxLength=200 runat="server" />
        </td>
        <td class="td_cell" style="width: 466px; height: 29px;">
        </td>
        <td style="height: 29px; width: 294px;">
        </td>
    </tr>
     <tr>
   
     
    <td class="td_cell" style="width: 545px; height: 29px;">
        Tickets Required</td><td style=" height: 29px; width: 504px;" >
        <INPUT id="chktkt" tabIndex=5 type=checkbox CHECKED runat="server" visible="True" />
    </td>
     <td class="td_cell" style="width: 466px; height: 29px;">
        Upon Request</td><td style="height: 29px; width: 294px;" >
        <INPUT id="chkreq" tabIndex=5 type=checkbox CHECKED runat="server" visible="True" />
    </td>

    </tr>
    <tr>
       <td class="td_cell" style="width: 130px; height: 21px;">
           Image</td>
        <td colspan="4" style="height: 21px">
            <asp:FileUpload ID="fileVehicleImage" runat="server" /><span class="td_cell">(203 X 151)</span>
            </td>
    </tr>
    <tr>
    <td style="width: 130px; height: 2px" class="td_cell"></TD><TD style="width: 1%; height: 2px">
        <input style="width: 329px" id="txtimg" tabindex="10" type="text" maxlength="30" runat="server" /></TD>
    
  </TR>
    <TR><TD style="WIDTH: 545px; HEIGHT: 24px" class="td_cell">Active</TD>
        <TD style="HEIGHT: 24px; width: 504px;">
        <INPUT id="chkActive" tabIndex=5 type=checkbox CHECKED runat="server" /></TD><td class="td_cell" style="width: 466px; height: 29px;">
        Print Confirmation</td><td>
            <INPUT id="chkprntconf" tabIndex=5 type=checkbox CHECKED runat="server" visible="True" dir="ltr" />
        </td></TR>
        
           <TR>
            <TD style="WIDTH: 545px; HEIGHT: 22px" class="td_cell">Income Code &nbsp;<SPAN style="COLOR: red" class="td_cell">*</SPAN></TD>
            <TD style="WIDTH: 2px; HEIGHT: 22px" class="td_cell" align=left><SPAN style="COLOR: #ff0000">
                <SELECT style="WIDTH: 102px" id="ddlAccCode" class="field_input" tabIndex=3 onchange="CallWebMethod('acccode')" runat="server"> <OPTION selected></OPTION></SELECT></SPAN></TD>
                   <td class="td_cell" style="width: 466px; height: 29px;">
         Name</td>
           <TD style="WIDTH: 4px; HEIGHT: 22px" class="td_cell" align=left>
                <SELECT style="WIDTH: 180px" id="ddlAccName" class="field_input" tabIndex=4 onchange="CallWebMethod('accname')" runat="server"> <OPTION selected></OPTION></SELECT></TD></TR>

                    <TR>
            <TD style="HEIGHT: 22px" class="td_cell">ExpenseCode<SPAN style="COLOR: red" class="td_cell">*&nbsp;&nbsp;</SPAN></TD>
            <TD style="WIDTH: 2px; HEIGHT: 22px" class="td_cell" align=left><SPAN style="COLOR: #ff0000">
                <SELECT style="WIDTH: 102px" id="ddlAccrualCode" class="field_input" tabIndex=3 onchange="CallWebMethod('accrualcode')" runat="server"> <OPTION selected></OPTION></SELECT></SPAN></TD>
                 <td class="td_cell" style="width: 466px; height: 29px;">
         Expense Name</td>
           <TD style="WIDTH: 4px; HEIGHT: 22px" class="td_cell" align=left>
                <SELECT style="WIDTH: 180px" id="ddlAccrualName" class="field_input" tabIndex=4 onchange="CallWebMethod('accrualname')" runat="server"> <OPTION selected></OPTION></SELECT></TD></TR>
        
        
        <TR>
        <TD style="WIDTH: 545px; height: 27px;"><asp:Button id="btnSave" tabIndex=6 runat="server" Text="Save" CssClass="btn" Width="46px"></asp:Button></TD>
        <TD style="width: 504px; height: 27px;"><asp:Button id="btnCancel" tabIndex=7 onclick="btnCancel_Click" runat="server" Text="Return To Search" CssClass="btn"></asp:Button>&nbsp;&nbsp; 
    <asp:Button id="btnhelp" tabIndex=8 onclick="btnhelp_Click" runat="server"
    
        Text="Help" CssClass="btn" __designer:wfdid="w20"></asp:Button><td></td><td></td>
        
        <asp:Label ID="lblwebserviceerror" runat="server"  style="display:none" Text="Webserviceerror"></asp:Label>
  
        </TD></TR></TBODY></TABLE>
</contenttemplate>
    </asp:UpdatePanel>
</asp:Content>

