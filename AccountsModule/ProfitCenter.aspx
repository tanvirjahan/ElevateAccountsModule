<%@ Page Language="VB" MasterPageFile="~/SubPageMaster.master" AutoEventWireup="false" CodeFile="ProfitCenter.aspx.vb" Inherits="ProfitCenterMaster"  %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
    <%@ OutputCache location="none" %> 
<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">
<script language ="javascript" type ="text/javascript" >
function CallWebMethod(methodType)
{
    switch(methodType)
    {
        case "acctcode":
            var select=document.getElementById("<%=ddlIncomecode.ClientID%>");
            var codeid=select.options[select.selectedIndex].text;
            var selectname=document.getElementById("<%=ddlIncomename.ClientID%>");
            selectname.value=select.options[select.selectedIndex].text;
            break;
        case "acctname":
            var select=document.getElementById("<%=ddlIncomename.ClientID%>");
            var codeid=select.options[select.selectedIndex].value;
            var selectname=document.getElementById("<%=ddlIncomecode.ClientID%>");
            selectname.value=select.options[select.selectedIndex].text;
            break;        
        case "costcode":
            var select=document.getElementById("<%=ddlCostcode.ClientID%>");
            var codeid=select.options[select.selectedIndex].text;
            var selectname=document.getElementById("<%=ddlCostname.ClientID%>");
            selectname.value=select.options[select.selectedIndex].text;
            break;
        case "costname":
            var select=document.getElementById("<%=ddlCostname.ClientID%>");
            var codeid=select.options[select.selectedIndex].value;
            var selectname=document.getElementById("<%=ddlCostcode.ClientID%>");
            selectname.value=select.options[select.selectedIndex].text;
            break;
        case "refincomecode":
            var select=document.getElementById("<%=ddlRefIncomecode.ClientID%>");
            var codeid=select.options[select.selectedIndex].text;
            var selectname=document.getElementById("<%=ddlRefIncomename.ClientID%>");
            selectname.value=select.options[select.selectedIndex].text;
            break;
        case "refincomename":
            var select=document.getElementById("<%=ddlRefIncomename.ClientID%>");
            var codeid=select.options[select.selectedIndex].value;
            var selectname=document.getElementById("<%=ddlRefIncomecode.ClientID%>");
            selectname.value=select.options[select.selectedIndex].text;
            break;        
        case "refcostcode":
            var select=document.getElementById("<%=ddlRefCostcode.ClientID%>");
            var codeid=select.options[select.selectedIndex].text;
            var selectname=document.getElementById("<%=ddlRefCostname.ClientID%>");
            selectname.value=select.options[select.selectedIndex].text;
            break;
        case "refcostname":
            var select=document.getElementById("<%=ddlRefCostname.ClientID%>");
            var codeid=select.options[select.selectedIndex].value;
            var selectname=document.getElementById("<%=ddlRefCostcode.ClientID%>");
            selectname.value=select.options[select.selectedIndex].text;
            break;

        case "refcompcode":
            var select = document.getElementById("<%=ddlCompCode.ClientID%>");
            var codeid = select.options[select.selectedIndex].text;
            var selectname = document.getElementById("<%=ddlCompName.ClientID%>");
            selectname.value = select.options[select.selectedIndex].text;
            break;
        case "refcompname":
            var select = document.getElementById("<%=ddlCompName.ClientID%>");
            var codeid = select.options[select.selectedIndex].value;
            var selectname = document.getElementById("<%=ddlCompCode.ClientID%>");
            selectname.value = select.options[select.selectedIndex].text;
            break;                                                   
    }
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
function FormValidation(state)
{
    if ((document.getElementById("<%=ddlServicecode.ClientID%>").value == "[Select]") || (document.getElementById("<%=ddlIncomecode.ClientID%>").value == "[Select]") || (document.getElementById("<%=ddlCostcode.ClientID%>").value == "[Select]") || (document.getElementById("<%=ddlRefIncomecode.ClientID%>").value == "[Select]") || (document.getElementById("<%=ddlRefCostcode.ClientID%>").value == "[Select]") || (document.getElementById("<%=ddlCompCode.ClientID%>").value == "[Select]"))
    {
       if (document.getElementById("<%=ddlServicecode.ClientID%>").value=="[Select]")
	    {
            document.getElementById("<%=ddlServicecode.ClientID%>").focus(); 
             alert("Select Service Category");
            return false;
         }
         else if(document.getElementById("<%=ddlIncomecode.ClientID%>").value=="[Select]") 
	     {
           document.getElementById("<%=ddlIncomecode.ClientID%>").focus();
           alert("Select Income Code");
            return false;
         }
          else if (document.getElementById("<%=ddlCostcode.ClientID%>").value=="[Select]") 
	     {
           document.getElementById("<%=ddlCostcode.ClientID%>").focus();
           alert("Select Cost of Sale Code");
            return false;
         }
         if(document.getElementById("<%=ddlRefIncomecode.ClientID%>").value=="[Select]") 
        {
            document.getElementById("<%=ddlRefIncomecode.ClientID%>").focus();
            alert("Select Refund Income Code");
            return false;
        }
        if (document.getElementById("<%=ddlRefCostcode.ClientID%>").value=="[Select]") 
        {
            document.getElementById("<%=ddlRefCostcode.ClientID%>").focus();
            alert("Select Refund Cost Code");
            return false;
        }
        if (document.getElementById("<%=ddlCompCode.ClientID%>").value == "[Select]")
         {
             document.getElementById("<%=ddlCompCode.ClientID%>").focus();
            alert("Select Complementery Code");
            return false;
        }
    
    }
    else
    {
        //       alert(state);
       if (state=='New'){if(confirm('Are you sure you want to save Profit Center ?')==false)return false;}
       if (state=='Edit'){if(confirm('Are you sure you want to update Profit Center ?')==false)return false;}
       if (state=='Delete'){if(confirm('Are you sure you want to delete Profit Center ?')==false)return false;}
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
</script>
    <asp:UpdatePanel id="UpdatePanel1" runat="server">
        <contenttemplate>
<TABLE style="BORDER-RIGHT: gray 2px solid; BORDER-TOP: gray 2px solid; BORDER-LEFT: gray 2px solid; BORDER-BOTTOM: gray 2px solid; width: 800px;"><TBODY><TR><TD align=center colSpan=7><asp:Label id="lblHeading" runat="server" Text="Add New Profit Center" CssClass="field_heading" Width="100%"></asp:Label></TD></TR>
<TR><TD style="WIDTH: 250px" class="td_cell"><SPAN style="COLOR: #000000">Service Category <SPAN style="COLOR: #ff0000">*</SPAN></SPAN></TD>
<TD><SELECT style="WIDTH: 159px" id="ddlServicecode" class="field_input" tabIndex=1 onchange="CallWebMethod('usercode');" runat="server"> <OPTION selected></OPTION></SELECT></TD>
<TD class="td_cell"></TD><TD style="width: 303px"></TD></TR><TR style="COLOR: #000000"><TD class="td_cell" style="width: 233px">Display Name</TD>
<TD style="COLOR: #000000"><INPUT style="WIDTH: 278px" id="txtDisplayname" class="field_input" tabIndex=2 type=text maxLength=25 runat="server" /></TD>
<TD class="td_cell"></TD><TD style="width: 303px"></TD></TR><TR><TD style="WIDTH: 233px" class="td_cell">Income Code <SPAN style="COLOR: #ff0000">*</SPAN></TD>
<TD><SELECT style="WIDTH: 159px" id="ddlIncomecode" class="field_input" tabIndex=3 onchange="CallWebMethod('acctcode');" runat="server"> <OPTION selected></OPTION></SELECT></TD>
<TD class="td_cell">Name</TD><TD style="COLOR: #000000; width: 303px;"><SELECT style="WIDTH: 300px" id="ddlIncomename" class="field_input" tabIndex=4 onchange="CallWebMethod('acctname');" runat="server"> <OPTION selected></OPTION></SELECT></TD></TR>
<TR><TD class="td_cell" style="width: 250px">Cost of Sale code <SPAN style="COLOR: #ff0000">*</SPAN>&nbsp;</TD><TD><SELECT style="WIDTH: 159px" id="ddlCostcode" class="field_input" tabIndex=5 onchange="CallWebMethod('costcode');" runat="server"> <OPTION selected></OPTION></SELECT></TD>
<TD class="td_cell">Name</TD><TD style="width: 303px"><SELECT style="WIDTH: 300px" id="ddlCostname" class="field_input" tabIndex=6 onchange="CallWebMethod('costname');" runat="server"> <OPTION selected></OPTION></SELECT></TD></TR>
    <tr>
        <td class="td_cell" style="width: 250px">
            Refund Income Code <SPAN style="COLOR: #ff0000">*</span></td>
        <td>
            <SELECT style="WIDTH: 159px" id="ddlRefIncomecode" class="field_input" tabIndex=3 onchange="CallWebMethod('refincomecode');" runat="server">
                <OPTION selected></option>
            </select>
        </td>
        <td class="td_cell">
            Name</td>
        <td style="width: 303px">
            <SELECT style="WIDTH: 300px" id="ddlRefIncomename" class="field_input" tabIndex=4 onchange="CallWebMethod('refincomename');" runat="server">
                <OPTION selected></option>
            </select>
        </td>
    </tr>
    <tr>
        <td class="td_cell" style="width: 233px">
            Refund Cost code <SPAN style="COLOR: #ff0000">*</span>&nbsp;</td>
        <td>
            <SELECT style="WIDTH: 159px" id="ddlRefCostcode" class="field_input" tabIndex=5 onchange="CallWebMethod('refcostcode');" runat="server">
                <OPTION selected></option>
            </select>
        </td>
        <td class="td_cell">
            Name</td>
        <td style="width: 303px">
            <SELECT style="WIDTH: 300px" id="ddlRefCostname" class="field_input" tabIndex=6 onchange="CallWebMethod('refcostname');" runat="server">
                <OPTION selected></option>
            </select>
        </td>
    </tr>

    <tr>
        <td class="td_cell" style="width: 233px">
            Complementery Code <SPAN style="COLOR: #ff0000">*</span>&nbsp;</td>
        <td>
            <SELECT style="WIDTH: 159px" id="ddlCompCode" class="field_input" tabIndex=5 onchange="CallWebMethod('refcompcode');" runat="server">
                <OPTION selected></option>
            </select>
        </td>
        <td class="td_cell">
            Name</td>
        <td style="width: 303px">
            <SELECT style="WIDTH: 300px" id="ddlCompName" class="field_input" tabIndex=6 onchange="CallWebMethod('refcompname');" runat="server">
                <OPTION selected></option>
            </select>
        </td>
    </tr>

    <TR><TD class="td_cell" style="width: 233px">Active</TD><TD><INPUT id="chkActive" tabIndex=7 type=checkbox CHECKED runat="server" /></TD><TD class="td_cell"></TD><TD style="width: 303px">&nbsp;</TD></TR><TR><TD style="WIDTH: 233px"><asp:Button id="btnSave" tabIndex=8 runat="server" Text="Save" CssClass="field_button"></asp:Button></TD>
    <TD><asp:Button id="btnCancel" tabIndex=9 runat="server" Text="Return To Search" CssClass="field_button"></asp:Button>&nbsp;<asp:Button id="btnhelp" tabIndex=10 onclick="btnhelp_Click" runat="server" Text="Help" CssClass="field_button"></asp:Button></TD><TD class="td_cell"></TD><TD style="width: 303px">&nbsp;</TD></TR></TBODY></TABLE>
</contenttemplate>
    </asp:UpdatePanel>
</asp:Content>

