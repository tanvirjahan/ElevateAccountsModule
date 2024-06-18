<%@ Page Language="VB" MasterPageFile="~/AccountsMaster.master" AutoEventWireup="false" CodeFile="RptCashFlow.aspx.vb" Inherits="RptCashFlow"  %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
        
<%@ Register Assembly="EclipseWebSolutions.DatePicker" Namespace="EclipseWebSolutions.DatePicker"
    TagPrefix="ews" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">
<script language ="javascript" type ="text/javascript" >
function FormValidation()
{
  if (document.getElementById("<%=txtFromDate.ClientID%>").value=="")
       {
           if (document.getElementById("<%=txtFromDate.ClientID%>").value=="")
           {
           
             document.getElementById("<%=txtFromDate.ClientID%>").focus(); 
             alert("As On Date field can not be blank.");
            return false;
           }
   }
}

</script>

    <asp:UpdatePanel id="UpdatePanel1" runat="server">
        <contenttemplate>
<TABLE style="BORDER-RIGHT: gray 1px solid; BORDER-TOP: gray 1px solid; BORDER-LEFT: gray 1px solid; WIDTH: 513px; BORDER-BOTTOM: gray 1px solid"><TBODY><TR><TD class="field_heading" align=center>Cash Flow&nbsp;Report</TD></TR><TR><TD><TABLE><TBODY><TR><TD class="td_cell"><TABLE><TBODY><TR><TD ><asp:Label id="lblsupplier" runat="server" Text="As On Date" __designer:wfdid="w1" Width="120px" CssClass="field_caption"></asp:Label></TD><TD>
    <asp:TextBox id="txtFromDate" tabIndex=1 runat="server" __designer:wfdid="w51" 
        Width="80px" CssClass="txtbox"></asp:TextBox> <asp:ImageButton id="ImgBtnFrmDt" runat="server" __designer:wfdid="w52" ImageUrl="~/Images/Calendar_scheduleHS.png"></asp:ImageButton> <cc1:MaskedEditValidator id="MskVFromDt" runat="server" __designer:wfdid="w53" Width="23px" CssClass="field_error" TooltipMessage="Input a date in dd/mm/yyyy format" InvalidValueMessage="Invalid Date" InvalidValueBlurredMessage="*" ErrorMessage="MskVFromDate" EmptyValueMessage="Date is required" EmptyValueBlurredText="*" Display="Dynamic" ControlToValidate="txtFromDate" ControlExtender="MskFromDate"></cc1:MaskedEditValidator></TD></TR></TBODY></TABLE></TD></TR></TBODY></TABLE></TD></TR><TR><TD class="td_cell" align=right>
    <asp:Button id="btnLoadreport" tabIndex=2 onclick="btnLoadreport_Click" 
        runat="server" Text="Load Reports " CssClass="btn"></asp:Button>&nbsp;<asp:Button 
        id="btnExit" tabIndex=3 onclick="btnExit_Click" runat="server" 
        Text=" Exit" CssClass="btn" CausesValidation="False"></asp:Button>&nbsp;<asp:Button 
        id="btnhelp" tabIndex=4 onclick="btnhelp_Click" runat="server" Text="Help" 
        __designer:wfdid="w4" CssClass="btn"></asp:Button></TD></TR></TBODY></TABLE><asp:ScriptManagerProxy id="ScriptManagerProxy1" runat="server" __designer:dtid="5629499534213129" __designer:wfdid="w93"><Services __designer:dtid="5629499534213130">
<asp:ServiceReference Path="~/clsServices.asmx"></asp:ServiceReference>
</Services>
</asp:ScriptManagerProxy> <cc1:CalendarExtender id="ClsExFromDate" runat="server" __designer:dtid="5629499534213125" __designer:wfdid="w94" TargetControlID="txtFromDate" PopupButtonID="ImgBtnFrmDt" Format="dd/MM/yyyy">
    </cc1:CalendarExtender><cc1:MaskedEditExtender id="MskFromDate" runat="server" __designer:dtid="5629499534213127" __designer:wfdid="w96" TargetControlID="txtFromDate" MessageValidatorTip="true" MaskType="Date" Mask="99/99/9999" ErrorTooltipEnabled="True" DisplayMoney="Left" AcceptNegative="Left">
    </cc1:MaskedEditExtender>&nbsp; 
</contenttemplate>
    </asp:UpdatePanel>
    &nbsp;<br />
</asp:Content>

