<%@ Page Language="VB" MasterPageFile="~/SubPageMaster.master" AutoEventWireup="false" CodeFile="OtherServiceCategories.aspx.vb" Inherits="OtherServiceCategories"  %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
    <%@ OutputCache location="none" %> 
<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">
    <script language ="javascript" type="text/javascript" >

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
				 //ColServices.clsServices.GetSellCurrCodeListnew(constr,codeid,FillCurrCodes,ErrorHandler,TimeOutHandler);
				/* var str='select ratesbasedonpax from othgrpmast where active=1 and othgrpcode=\''+ ddl.value +'\'';
                var connstr=document.getElementById("<%=txtconnection.ClientID%>");  
                constr=connstr.value   
				
				ColServices.clsServices.GetQueryReturnStringValuenew(constr,str,show) ;*/
	
				
				
				return true;
			}
		}
}

function show(s)
{
        if(s=="1")
			{		
		     document.getElementById("<%=ddladchild.ClientID%>").style.display="block";		 
		     document.getElementById("<%=lbladult.ClientID%>").style.display="block";		 							
	        }
	        
	else
			{

			document.getElementById("<%=ddladchild.ClientID%>").style.display = 'none';
			document.getElementById("<%=lbladult.ClientID%>").style.display = 'none';					
			
	         }
			
				return true;
			
	        



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
			  /*  var str = 'select ratesbasedonpax from othgrpmast where active=1 and othgrpname=\'' + ddl.value + '\'';
			    var connstr = document.getElementById("<%=txtconnection.ClientID%>");
			    constr = connstr.value
			    ColServices.clsServices.GetQueryReturnStringValuenew(constr, str, show);*/
					return true;
			}
		}
}

//function disableadultchild() {
//    var calcby = document.getElementById("<%=ddlcalcpax.ClientID%>").value;
//    var ddl = document.getElementById("<%=ddladchild.ClientID%>");
//    var lbladult = document.getElementById("<%=lbladult.ClientID%>");
//    if (calcby == 'No') {

//        ddl.style.display = 'none';
//        lbladult.style.display = 'none';

//    }
//    else {
//        ddl.style.display = '';
//        lbladult.style.display = '';
//    }
//}			
//			
			
</script> 
    <asp:UpdatePanel id="UpdatePanel1" runat="server">
        <contenttemplate>
<TABLE style="BORDER-RIGHT: gray 2px solid; BORDER-TOP: gray 2px solid; BORDER-LEFT: gray 2px solid; WIDTH: 622px; BORDER-BOTTOM: gray 2px solid; HEIGHT: 446px"><TBODY><TR><TD style="HEIGHT: 4px" class="td_cell" align=center colSpan=5><asp:Label id="lblHeading" runat="server" Text="Add New Other Service Categories" ForeColor="White" CssClass="field_heading" Width="725px"></asp:Label></TD><TD style="HEIGHT: 4px" class="td_cell" align=center colSpan=1></TD></TR><TR style="COLOR: #ff0000"><TD style="WIDTH: 130px; HEIGHT: 7px" class="td_cell"><SPAN style="COLOR: #000000">Code </SPAN><SPAN style="COLOR: #ff0000" class="td_cell">*&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </SPAN></TD><TD style="WIDTH: 1%; COLOR: #000000; HEIGHT: 7px"><INPUT style="WIDTH: 196px" id="txtCode" class="field_input" tabIndex=1 type=text maxLength=20 runat="server" /><SPAN style="COLOR: #000000"></SPAN></TD><TD style="WIDTH: 1%; COLOR: #000000; HEIGHT: 7px"></TD><TD style="WIDTH: 10%; COLOR: #000000; HEIGHT: 7px"></TD><TD style="WIDTH: 23px; COLOR: #000000; HEIGHT: 7px"></TD><TD style="COLOR: #000000; HEIGHT: 7px"></TD></TR><TR style="COLOR: #000000"><TD style="WIDTH: 130px; HEIGHT: 2px" class="td_cell"><SPAN style="COLOR: #000000">Name</SPAN><SPAN style="COLOR: #ff0000"> <SPAN class="td_cell">*</SPAN></SPAN></TD><TD style="WIDTH: 1%; COLOR: #ff0000; HEIGHT: 2px"><INPUT style="WIDTH: 196px" id="txtName" class="field_input" tabIndex=2 type=text maxLength=100 runat="server" /></TD><TD style="WIDTH: 1%; COLOR: #ff0000; HEIGHT: 2px"></TD><TD style="WIDTH: 10%; COLOR: #ff0000; HEIGHT: 2px"></TD><TD style="WIDTH: 23px; COLOR: #ff0000; HEIGHT: 2px"></TD><TD style="COLOR: #ff0000; HEIGHT: 2px"></TD></TR><TR><TD style="WIDTH: 130px; HEIGHT: 10px" class="td_cell">Group Code&nbsp;<SPAN style="COLOR: red" class="td_cell">*</SPAN></TD><TD style="WIDTH: 1%; HEIGHT: 10px" class="td_cell" align=left><SPAN style="COLOR: #ff0000"><SELECT style="WIDTH: 200px" id="ddlOtherGrpCode" class="field_input" tabIndex=3 onchange="GetOtherGrpValueFrom()" runat="server"> <OPTION selected></OPTION></SELECT></SPAN></TD><TD style="WIDTH: 1%; HEIGHT: 10px" class="td_cell" align=left></TD><TD style="WIDTH: 10%; HEIGHT: 10px" class="td_cell" align=left>Group&nbsp;Name&nbsp;&nbsp;</TD><TD style="WIDTH: 23px; HEIGHT: 10px" class="td_cell" align=left><SELECT style="WIDTH: 350px" id="ddlOtherGrpName" class="field_input" tabIndex=4 onchange="GetOtherGrpValueCode()" runat="server"> <OPTION selected></OPTION></SELECT></TD><TD style="HEIGHT: 10px" class="td_cell" align=left></TD></TR><TR><TD style="WIDTH: 130px" class="td_cell">Order&nbsp;&nbsp;<SPAN style="COLOR: red" class="td_cell">*</SPAN></TD><TD style="WIDTH: 1%"><INPUT style="TEXT-ALIGN: right" id="txtOrder" tabIndex=5 type=text maxLength=10 runat="server" /></TD><TD style="WIDTH: 1%"></TD><TD style="WIDTH: 10%"></TD><TD style="WIDTH: 23px"></TD><TD></TD></TR><TR><TD style="WIDTH: 130px" class="td_cell">Min Pax&nbsp;&nbsp;<SPAN style="COLOR: red" class="td_cell">*</SPAN></TD><TD style="WIDTH: 1%"><INPUT style="TEXT-ALIGN: right" id="txtMinPax" tabIndex=6 type=text runat="server" /></TD><TD style="WIDTH: 1%"></TD><TD style="WIDTH: 10%"></TD><TD style="WIDTH: 23px"></TD><TD></TD></TR><TR><TD style="WIDTH: 130px; HEIGHT: 8px" class="td_cell">Max Pax&nbsp;&nbsp;<SPAN style="COLOR: red" class="td_cell">*</SPAN></TD><TD style="WIDTH: 1%; HEIGHT: 8px"><INPUT style="TEXT-ALIGN: right" id="txtMaxPax" tabIndex=7 type=text runat="server" /></TD><TD style="WIDTH: 1%; HEIGHT: 8px"></TD><TD style="WIDTH: 10%; HEIGHT: 8px"></TD><TD style="WIDTH: 23px; HEIGHT: 8px"></TD><TD style="HEIGHT: 8px"></TD></TR><TR><TD style="WIDTH: 130px" class="td_cell">Remark&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </TD><TD colSpan=4><asp:TextBox id="txtRemark" tabIndex=8 runat="server" Height="44px" Width="613px" TextMode="MultiLine"></asp:TextBox></TD><TD colSpan=1></TD></TR>
    <tr>
        <td class="td_cell" style="width: 130px">
            Display Name</td>
        <td colspan="4">
            <INPUT style="width: 329px;" id="txtdispname" tabIndex=9 type="text" runat="server" maxlength="50" /></td>
        <td colspan="1">
        </td>
    </tr>

    <TR><TD style="WIDTH: 130px; HEIGHT: 2px" class="td_cell">Unit Name</TD><TD style="WIDTH: 1%; HEIGHT: 2px">
        <INPUT style="WIDTH: 151px" id="txtUnitName" tabIndex=10 type="text" maxLength=10 runat="server" /></TD><TD style="WIDTH: 1%; HEIGHT: 2px"></TD><TD style="WIDTH: 10%; HEIGHT: 2px"></TD><TD style="WIDTH: 23px; HEIGHT: 2px">&nbsp;</TD><TD style="HEIGHT: 2px"></TD></TR>
        
        <TR><TD style="WIDTH: 130px; HEIGHT: 2px" class="td_cell"><%--Calculated by Pax/ Unit&nbsp;&nbsp;--%></TD><TD style="WIDTH: 1%; HEIGHT: 2px">
                <asp:DropDownList id="ddlCalcPax" tabIndex=11 runat="server" Visible="false"  Width="60px" ><asp:ListItem Selected="True">Yes</asp:ListItem>
            <asp:ListItem>No</asp:ListItem>
            </asp:DropDownList>&nbsp; </TD>
            <TD style="WIDTH: 1%; HEIGHT: 2px"></TD><TD style="WIDTH: 10%; HEIGHT: 2px"><asp:Label id="lbladult" runat="server" Text="Adult/ Child" Font-Size="8pt" Font-Names="Arial" Visible="false" Width="55px"></asp:Label></TD><TD style="WIDTH: 23px; HEIGHT: 2px">
                <asp:DropDownList id="ddladchild" tabIndex=12 runat="server" Visible="false" Width="95px"><asp:ListItem Selected="True" Value="A">Adult</asp:ListItem>
            <asp:ListItem Value="C">Child</asp:ListItem>
            </asp:DropDownList></TD><TD style="HEIGHT: 2px"></TD></TR>

<TR><TD style="WIDTH: 129px; HEIGHT: 1px" class="td_cell" colSpan=5><TABLE style="WIDTH: 334px; HEIGHT: 52px" border=0><TBODY><TR><TD style="WIDTH: 100%; HEIGHT: 4px">
    <INPUT id="ChkPakReq" tabIndex=13 type=checkbox CHECKED runat="server" />Pax Check Required</TD></TR><TR><TD style="WIDTH: 103px; HEIGHT: 1px" colSpan=2>
        <INPUT id="ChkPrnRemark" tabIndex=14 type=checkbox CHECKED runat="server" />Print Remark</TD></TR><TR><TD style="WIDTH: 100%">
        <INPUT id="ChkActive" tabIndex=15 type=checkbox CHECKED runat="server" />Active</TD></TR></TBODY></TABLE></TD><TD style="HEIGHT: 1px" class="td_cell" colSpan=1></TD></TR><TR><TD style="WIDTH: 130px; HEIGHT: 22px">
    <asp:Button id="btnSave" tabIndex=16 onclick="btnSave_Click" runat="server" 
        Text="Save" CssClass="field_button"></asp:Button></TD><TD style="WIDTH: 1%; HEIGHT: 22px">
        <asp:Button id="btnCancel" tabIndex=17 onclick="btnCancel_Click" runat="server" 
            Text="Return To Search" CssClass="field_button"></asp:Button></TD><TD style="WIDTH: 1%; HEIGHT: 22px"></TD><TD style="WIDTH: 10%; HEIGHT: 22px">
    <asp:Button id="btnhelp" tabIndex=18 onclick="btnhelp_Click" runat="server" 
        Text="Help" CssClass="field_button" Width="46px"></asp:Button></TD><TD style="WIDTH: 23px; HEIGHT: 22px">
    <input id="txtconnection" runat="server" style="visibility: hidden; width: 12px;
        height: 9px" type="text" class="field_input" /></TD><TD style="HEIGHT: 22px"></TD></TR></TBODY></TABLE>
</contenttemplate>
    </asp:UpdatePanel>
    
     <asp:ScriptManagerProxy id="ScriptManagerProxy1" runat="server">
        <services>
<asp:ServiceReference Path="~/clsServices.asmx"></asp:ServiceReference>
</services>
    </asp:ScriptManagerProxy>
</asp:Content>

