<%@ Page Language="VB" MasterPageFile="~/ReservationMaster.master" AutoEventWireup="false" CodeFile="RptSalesreportSearch.aspx.vb" Inherits="RptSalesreportSearch" Strict="true" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
   
 <%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
        
<%@ Register Assembly="EclipseWebSolutions.DatePicker" Namespace="EclipseWebSolutions.DatePicker"
    TagPrefix="ews" %>

    
<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">

<script src="../Content/js/jquery-1.5.1.min.js" type="text/javascript"></script>
<script src="../Content/js/JqueryUI.js" type="text/javascript"></script>
<script src="../Content/js/accounts.js" type="text/javascript"></script>
<link type ="text/css" href ="../Content/css/JqueryUI.css" rel ="Stylesheet" />

<script language ="javascript" type="text/javascript" >

    function FillMkt() {
        var ddlc = document.getElementById("<%=ddlCustomer.ClientID%>");
        ddlm = document.getElementById("<%=ddlMarketCat.ClientID%>");
        var constr = document.getElementById("<%=txtconnection.ClientID%>");
        if (ddlc.value != '[Select]') {
            sqlstr = "select plgrpname,plgrpcode from agentmast where agentcode='" + trim(ddlc.value) + "'";
            ColServices.clsServices.GetQueryReturnStringnew(constr.value, sqlstr, GetMktCat, TimeOutHandler, ErrorHandler);

        }
        else {
            ddlm.value = '[Select]';
        }
    }

    function GetMktCat(result) {

        var ddl = document.getElementById("<%=ddlMarketCat.ClientID%>");
            RemoveAll(ddl);
            for (var i = 0; i < result.length; i++) {
                var option = new Option(result[i].ListText, result[i].ListValue);
                ddl.options.add(option);
            }
         
    }
function ChangeDate()
{
   
     var txtfdate=document.getElementById("<%=txtFromDate.ClientID%>");
       if (txtfdate.value==''){alert("Enter From Date.");txtfdate.focus();  }
      else {ColServices.clsServices.GetQueryReturnFromToDate('FromDate',30,txtfdate.value,FillToDate,ErrorHandler,TimeOutHandler);}
}
function FillToDate(result)
    {
       	 var txttdate=document.getElementById("<%=txtToDate.ClientID%>");
      	 txttdate.value=result;
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
function ValidateForm()
{
 var txtfdate=document.getElementById("<%=txtFromDate.ClientID%>");
 var txttdate=document.getElementById("<%=txtToDate.ClientID%>");
 if (txtfdate.value==''){alert("Enter From Date.");txtfdate.focus();}
 else if (txttdate.value==''){alert("Enter To Date.");txttdate.focus();}
 else if(txtfdate.value > txttdate.value){alert("To date should be greater than from dat.");txttdate.focus();}
 
}
function trim(stringToTrim) {
    return stringToTrim.replace(/^\s+|\s+$/g, "");
}
</script>
    <asp:UpdatePanel id="UpdatePanel1" runat="server">
        <contenttemplate>
<TABLE style="BORDER-RIGHT: gray 1px solid; BORDER-TOP: gray 1px solid; BORDER-LEFT: gray 1px solid; BORDER-BOTTOM: gray 1px solid"><TBODY><TR><TD style="HEIGHT: 17px; TEXT-ALIGN: center" class=" field_heading" colSpan=5>Sales &nbsp;Report</TD></TR><TR><TD colSpan=3><TABLE style="WIDTH: 562px; HEIGHT: 201px" class="td_cell"><TBODY><TR><TD colSpan=4><TABLE style="WIDTH: 653px; HEIGHT: 179px"><TBODY><TR><TD><asp:Label id="Label1" runat="server" Text="Invoice No" CssClass="field_caption" Width="120px"></asp:Label></TD><TD><INPUT style="WIDTH: 194px" id="txtInvoiceNo" class="field_input" tabIndex=1 type=text runat="server" /></TD><TD><asp:Label id="Label3" runat="server" Text="File Number" CssClass="field_caption" Width="120px"></asp:Label></TD><TD><INPUT style="WIDTH: 194px" id="txtRequestId" class="filed_input" tabIndex=2 type=text runat="server" /></TD></TR><TR><TD><asp:Label id="Label2" runat="server" Text="From Invoice Date" CssClass="field_caption" Width="120px"></asp:Label></TD><TD><asp:TextBox id="txtFromDate" tabIndex=3 runat="server" CssClass="fiel_input" Width="80px"></asp:TextBox>&nbsp; <asp:ImageButton id="ImgBtnFrmDt" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.png"></asp:ImageButton> <cc1:MaskedEditValidator id="MEVFromDate" runat="server" CssClass="field_error" TooltipMessage="Input a date in dd/mm/yyyy format" InvalidValueMessage="Invalid Date" InvalidValueBlurredMessage="Invalid Date" EmptyValueMessage="Date is required" EmptyValueBlurredText="Date is required" Display="Dynamic" ControlToValidate="txtFromDate" ControlExtender="MEFromDate"></cc1:MaskedEditValidator></TD><TD><asp:Label id="Label5" runat="server" Text="To Invoice Date" CssClass="field_caption" Width="120px"></asp:Label></TD><TD><asp:TextBox id="txtToDate" tabIndex=4 runat="server" CssClass="fiel_input" Width="80px"></asp:TextBox> <asp:ImageButton id="ImgBtnRevDate" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.png"></asp:ImageButton> <cc1:MaskedEditValidator id="MEVToDate" runat="server" CssClass="field_error" TooltipMessage="Input a date in dd/mm/yyyy format" InvalidValueMessage="Invalid Date" InvalidValueBlurredMessage="Invalid Date" EmptyValueMessage="Date is required" EmptyValueBlurredText="Date is required" Display="Dynamic" ControlToValidate="txtToDate" ControlExtender="METoDate"></cc1:MaskedEditValidator></TD></TR><TR><TD style="HEIGHT: 26px"><asp:Label id="Label4" runat="server" Text="Status" CssClass="field_caption" Width="120px"></asp:Label></TD><TD style="HEIGHT: 26px"><SELECT style="WIDTH: 154px" id="ddlStatus" class="td_cell" tabIndex=5 runat="server"> <OPTION value="[Select]" selected>[Select]</OPTION> <OPTION value="P">Posted</OPTION> <OPTION value="U">UnPosted</OPTION></SELECT></TD><TD style="HEIGHT: 26px"></TD><TD style="HEIGHT: 26px"></TD></TR>
<TR><TD><asp:Label id="Label6" runat="server" Text="Customer" CssClass="field_caption" Width="120px"></asp:Label></TD><TD>

 <input type="text" name="accSearch"  class="field_input MyAutoCompleteClass"  onfocus="MyAutoCustomer_rptFillArray();" style="width:98% ; font " id="accSearch"  runat="server" /><SELECT style="WIDTH: 200px" id="ddlCustomer" class="field_input MyDropDownListCustValue"  tabIndex=6 runat="server"> <OPTION selected></OPTION></SELECT></TD>
 
 <TD><asp:Label id="Label9" runat="server" Text="Customer Ref" CssClass="field_caption" Width="120px"></asp:Label></TD><TD><INPUT id="txtCustRef" class="field_input" tabIndex=7 type=text runat="server" /></TD></TR><TR><TD><asp:Label id="Label11" runat="server" Text="Report Type" CssClass="field_caption" Width="120px"></asp:Label></TD><TD><asp:DropDownList id="ddlrpttype" tabIndex=11 runat="server" CssClass="field_input" Width="207px"><asp:ListItem Value="0">Brief</asp:ListItem>
<asp:ListItem Value="1">Detailed</asp:ListItem>
</asp:DropDownList></TD><TD><asp:Label id="Label7" runat="server" Text="Market Category" CssClass="field_caption" Width="120px"></asp:Label></TD>
<TD>
<SELECT style="WIDTH: 200px" id="ddlMarketCat" class="field_input"  tabIndex=12 runat="server"><OPTION selected></OPTION></SELECT>
 </TD></TR><TR><TD align=right colSpan=4><INPUT style="VISIBILITY: hidden; WIDTH: 12px; HEIGHT: 9px" id="txtconnection" type=text runat="server" />&nbsp;&nbsp;&nbsp;&nbsp;<asp:Button id="Button1" onclick="Button1_Click1" runat="server" Text="Export" __designer:dtid="4222124650660080" style="display:none" CssClass="field_button" __designer:wfdid="w2"></asp:Button>&nbsp; 
    <asp:Button id="btnLoadreport" tabIndex=13 onclick="btnLoadreport_Click" 
        runat="server" Text="Load Reports " CssClass="field_button"></asp:Button>&nbsp; 
    <asp:Button id="btnClear" tabIndex=14 onclick="btnClear_Click" runat="server" 
        Text="Clear" Font-Bold="True" CssClass="field_button"></asp:Button>&nbsp;
    <asp:Button id="btnExit" tabIndex=15 onclick="btnExit_Click" runat="server" 
        Text=" Exit" CssClass="field_button" CausesValidation="False"></asp:Button>&nbsp; 
    <asp:Button id="btnhelp" tabIndex=16 onclick="btnhelp_Click" runat="server" 
        Text="Help" Height="20px" CssClass="field_button"></asp:Button></TD></TR></TBODY></TABLE></TD></TR></TBODY></TABLE><cc1:CalendarExtender id="CEFromDate" runat="server" Format="dd/MM/yyyy" PopupButtonID="ImgBtnFrmDt" TargetControlID="txtFromDate"></cc1:CalendarExtender><cc1:MaskedEditExtender id="MEFromDate" runat="server" TargetControlID="txtFromDate" Mask="99/99/9999" MaskType="Date"></cc1:MaskedEditExtender><cc1:CalendarExtender id="CEToDate" runat="server" Format="dd/MM/yyyy" PopupButtonID="ImgBtnRevDate" TargetControlID="txtToDate"></cc1:CalendarExtender> &nbsp; <cc1:MaskedEditExtender id="METoDate" runat="server" TargetControlID="txtToDate" Mask="99/99/9999" MaskType="Date"></cc1:MaskedEditExtender></TD></TR></TBODY></TABLE><asp:ScriptManagerProxy id="ScriptManagerProxy1" runat="server">
                    <services>
<asp:ServiceReference Path="~/clsServices.asmx"></asp:ServiceReference>
</services>
                </asp:ScriptManagerProxy> 
</contenttemplate>

    </asp:UpdatePanel>
</asp:Content>

