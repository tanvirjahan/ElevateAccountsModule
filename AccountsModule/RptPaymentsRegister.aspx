<%@ Page Language="VB" MasterPageFile="~/AccountsMaster.master" AutoEventWireup="false" CodeFile="RptPaymentsRegister.aspx.vb" Inherits="RptPaymentsRegister" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="EclipseWebSolutions.DatePicker" Namespace="EclipseWebSolutions.DatePicker"
    TagPrefix="ews" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">


    <asp:UpdatePanel id="UpdatePanel1" runat="server">
        <contenttemplate>
<TABLE style="BORDER-RIGHT: gray 1px solid; BORDER-TOP: gray 1px solid; BORDER-LEFT: gray 1px solid; WIDTH: 583px; BORDER-BOTTOM: gray 1px solid" class="td_cell" align=left><TBODY><TR><TD style="WIDTH: 1003px" class="field_heading" align=center colSpan=5><asp:Label id="lblHeading" runat="server" Text="Payment Receipts Register" CssClass="field_heading" Width="388px"></asp:Label></TD></TR><TR><TD style="WIDTH: 1003px" class="td_cell" align=left colSpan=5><asp:Panel id="pnlDate" runat="server" Font-Bold="True" GroupingText="Date"><TABLE style="WIDTH: 520px" class="td_cell" align=left><TBODY><TR><TD class="td_cell" align=left>From Date</TD><TD class="td_cell" align=left><asp:TextBox id="txtFromDate" runat="server" CssClass="txtbox" Width="80px" __designer:wfdid="w31"></asp:TextBox><asp:ImageButton id="ImgBtnFrmDt" runat="server" __designer:wfdid="w32" ImageUrl="~/Images/Calendar_scheduleHS.png"></asp:ImageButton> <cc1:MaskedEditValidator id="MEVFromDate" runat="server" CssClass="field_error" __designer:wfdid="w35" Display="Dynamic" ControlToValidate="txtFromDate" ControlExtender="MEFromDate" EmptyValueBlurredText="Date is required" EmptyValueMessage="Date is required" InvalidValueBlurredMessage="Invalid Date" InvalidValueMessage="Invalid Date" TooltipMessage="Input a date in dd/mm/yyyy format"></cc1:MaskedEditValidator></TD>
<TD class="td_cell" vAlign=middle align=left colSpan=2>&nbsp;&nbsp;To Date</TD><TD class="td_cell" vAlign=middle align=left colSpan=1><asp:TextBox id="txtToDate" runat="server" CssClass="txtbox" Width="80px" __designer:wfdid="w33"></asp:TextBox><asp:ImageButton id="ImgBtnRevDate" runat="server" __designer:wfdid="w34" ImageUrl="~/Images/Calendar_scheduleHS.png"></asp:ImageButton> <cc1:MaskedEditValidator id="MEVToDate" runat="server" CssClass="field_error" __designer:wfdid="w38" Display="Dynamic" ControlToValidate="txtToDate" ControlExtender="METoDate" EmptyValueBlurredText="Date is required" EmptyValueMessage="Date is required" InvalidValueBlurredMessage="Invalid Date" InvalidValueMessage="Invalid Date" TooltipMessage="Input a date in dd/mm/yyyy format"></cc1:MaskedEditValidator></TD></TR></TBODY></TABLE></asp:Panel></TD></TR><TR><TD style="WIDTH: 1003px" class="td_cell" align=left colSpan=5><asp:Panel id="pnlCash" runat="server" Font-Bold="True" GroupingText="Cash / Bank"><TABLE style="WIDTH: 542px" class="td_cell" align=left><TBODY><TR><TD style="WIDTH: 77px" align=left rowSpan=2><TABLE><TBODY><TR><TD style="WIDTH: 100px"><INPUT id="rdbtnAll" onclick="Call()" type=radio CHECKED name="1" runat="server" />&nbsp;All</TD></TR><TR><TD style="WIDTH: 100px"><INPUT id="rdbtnRange" type=radio name="1" runat="server" />&nbsp;Range</TD></TR></TBODY></TABLE></TD><TD style="WIDTH: 302px" align=left>From <SELECT style="WIDTH: 199px" id="ddlAccCD" class="drpdown" onchange="CallWebMethod('acccode');" runat="server"> <OPTION selected></OPTION></SELECT></TD><TD style="WIDTH: 202px" align=left><SELECT style="WIDTH: 199px" id="ddlAccNM" class="drpdown" onchange="CallWebMethod('accname');" runat="server"> <OPTION selected></OPTION></SELECT></TD></TR><TR><TD style="WIDTH: 302px">To&nbsp;&nbsp;&nbsp;&nbsp; <SELECT style="WIDTH: 199px" id="ddltoAccCD" class="drpdown" onchange="CallWebMethod('toacccode');" runat="server"> <OPTION selected></OPTION></SELECT></TD><TD style="WIDTH: 202px" align=left><SELECT style="WIDTH: 199px" id="ddltoAccNM" class="drpdown" onchange="CallWebMethod('toaccname');" runat="server"> <OPTION selected></OPTION></SELECT></TD></TR><TR><TD style="WIDTH: 77px" class="td_cell" align=left></TD><TD class="td_cell" vAlign=middle align=left colSpan=2></TD></TR><TR><TD style="WIDTH: 77px" class="td_cell" align=left>Report Type</TD><TD class="td_cell" vAlign=middle align=left colSpan=2><SELECT style="WIDTH: 199px" id="ddlType" class="drpdown" onchange="CallWebMethod('SupplierAgentCode');" runat="server"> <OPTION value="Detailed" selected>Detailed</OPTION><OPTION value="Summary">Summary</OPTION></SELECT></TD></TR><TR><TD style="WIDTH: 100px" class="td_cell" align=left>Report Order</TD><TD class="td_cell" vAlign=middle align=left colSpan=2><SELECT style="WIDTH: 199px" id="ddlRptOrder" class="drpdown" onchange="CallWebMethod('SupplierAgentCode');" runat="server"> <OPTION value="Number" selected>Number</OPTION><OPTION value="Date">Date</OPTION><OPTION value="Acount">Account</OPTION><OPTION value="Cheque No">Cheque No</OPTION></SELECT></TD></TR><TR><TD style="WIDTH: 77px"></TD><TD align=right colSpan=2>
    <asp:Button id="btnLoadReprt" runat="server" Text="Load Report" CssClass="btn"></asp:Button>&nbsp;<asp:Button id="btnhelp" tabIndex=39 onclick="btnhelp_Click" runat="server" Text="Help" __designer:dtid="2814749767106618" CssClass="btn" Width="39px" __designer:wfdid="w3"></asp:Button>&nbsp;</TD></TR></TBODY></TABLE></asp:Panel></TD></TR><TR><TD style="WIDTH: 1003px" colSpan=5><cc1:CalendarExtender id="CEFromDate" runat="server" __designer:wfdid="w36" TargetControlID="txtFromDate" PopupButtonID="ImgBtnFrmDt" Format="dd/MM/yyyy"></cc1:CalendarExtender> <cc1:MaskedEditExtender id="MEFromDate" runat="server" __designer:wfdid="w37" TargetControlID="txtFromDate" Mask="99/99/9999" MaskType="Date"></cc1:MaskedEditExtender> <cc1:CalendarExtender id="CEToDate" runat="server" __designer:wfdid="w39" TargetControlID="txtToDate" PopupButtonID="ImgBtnRevDate" Format="dd/MM/yyyy"></cc1:CalendarExtender> <cc1:MaskedEditExtender id="METoDate" runat="server" __designer:wfdid="w40" TargetControlID="txtToDate" Mask="99/99/9999" MaskType="Date"></cc1:MaskedEditExtender></TD></TR><TR><TD style="WIDTH: 1003px" colSpan=5><asp:ScriptManagerProxy id="ScriptManagerProxy1" runat="server"><Services>
<asp:ServiceReference Path="clsServices.asmx"></asp:ServiceReference>
</Services>
</asp:ScriptManagerProxy></TD></TR></TBODY></TABLE>
</contenttemplate>
    </asp:UpdatePanel>
<script language="javascript" type="text/javascript">

function AllRange(rb,Opt,Group)
{
       var rb = document.getElementById(rb);
       rb.checked = true;
       switch (Group)
       {
           case "AC":
                var ddlm1 = document.getElementById("<%=ddlAccCD.ClientID%>");
                var ddlm2 = document.getElementById("<%=ddlAccNM.ClientID%>");
                var ddlm3 = document.getElementById("<%=ddltoAccCD.ClientID%>");
                var ddlm4 = document.getElementById("<%=ddltoAccNM.ClientID%>");
           break; 
       }
       
       if (Opt == 'A') 
       {
           ddlm1.value = '[Select]';
           ddlm2.value = '[Select]';
           ddlm3.value = '[Select]';
           ddlm4.value = '[Select]';
           
           ddlm1.disabled  = true;
           ddlm2.disabled  = true;
           ddlm3.disabled  = true;
           ddlm4.disabled  = true;
       }
       else
       {
           ddlm1.disabled  = false;
           ddlm2.disabled  = false;
           ddlm3.disabled  = false;
           ddlm4.disabled  = false;
       }
          
}
function CallWebMethod(methodType)
    {
       switch(methodType)
        {
            case "acccode":
                var select=document.getElementById("<%=ddlAccCD.ClientID%>");
                var codeid=select.options[select.selectedIndex].text;
                var selectname=document.getElementById("<%=ddlAccNM.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;
                break;
            case "accname":
                var select=document.getElementById("<%=ddlAccNM.ClientID%>");
                var codeid=select.options[select.selectedIndex].text;
                var selectname=document.getElementById("<%=ddlAccCD.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;
            break;
             case "toacccode":
                var select=document.getElementById("<%=ddltoAccCD.ClientID%>");
                var codeid=select.options[select.selectedIndex].text;
                var selectname=document.getElementById("<%=ddltoAccNM.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;
                break;
            case "toaccname":
                var select=document.getElementById("<%=ddltoAccNM.ClientID%>");
                var codeid=select.options[select.selectedIndex].text;
                var selectname=document.getElementById("<%=ddltoAccCD.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;
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
function ChangeDate()
{
   
     var txtfdate=document.getElementById("<%=txtFromDate.ClientID%>");
     var txttdate=document.getElementById("<%=txtToDate.ClientID%>");
    
     if (txtfdate.value==''){alert("Enter From Date.");txtfdate.focus();  }
     else {txttdate.value=txtfdate.value;}
}


</script>
</asp:Content>


