<%@ Page Language="VB" MasterPageFile="~/AccountsMaster.master" AutoEventWireup="false" CodeFile="RptSupplierInvoicesNotRcvd.aspx.vb" Inherits="RptSupplierInvoicesNotRcvd" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
        
<%@ Register Assembly="EclipseWebSolutions.DatePicker" Namespace="EclipseWebSolutions.DatePicker"
    TagPrefix="ews" %>


<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">


<script language ="javascript" type ="text/javascript" >

function CallWebMethod(methodType)
    {
   
       switch(methodType)
        {                       
              
              case "fromsuppliercode":
                var select=document.getElementById("<%=ddlFromSupplier.ClientID%>");
                var selectname=document.getElementById("<%=ddlFromSupplierName.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;
                
                var txtfromcd=document.getElementById("<%=txtFromSupplier.ClientID%>");
                var txtfromnm=document.getElementById("<%=txtFromSupplierName.ClientID%>");
                txtfromcd.value=select.options[select.selectedIndex].value;
                txtfromnm.value=select.options[select.selectedIndex].text;
                
                
                var select1=document.getElementById("<%=ddlToSupplier.ClientID%>");
                select1.value=select.options[select.selectedIndex].value;
                var selectname1=document.getElementById("<%=ddlToSupplierName.ClientID%>");
                selectname1.value=select.options[select.selectedIndex].text;
                
                var txttomcd=document.getElementById("<%=txtToSupplier.ClientID%>");
                var txttonm=document.getElementById("<%=txtToSupplierName.ClientID%>");
                txttomcd.value=select.options[select.selectedIndex].value;
                txttonm.value=select.options[select.selectedIndex].text;
                
                break;
            case "fromsuppliername":
                var select=document.getElementById("<%=ddlFromSupplierName.ClientID%>");
                var selectname=document.getElementById("<%=ddlFromSupplier.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;
               
                var txtfromcd=document.getElementById("<%=txtFromSupplier.ClientID%>");
                var txtfromnm=document.getElementById("<%=txtFromSupplierName.ClientID%>");
                txtfromcd.value=select.options[select.selectedIndex].text;
                txtfromnm.value=select.options[select.selectedIndex].value;
                
                
                var select1=document.getElementById("<%=ddlToSupplier.ClientID%>");
                select1.value=select.options[select.selectedIndex].text;
                var selectname1=document.getElementById("<%=ddlToSupplierName.ClientID%>");
                selectname1.value=select.options[select.selectedIndex].value;
             
                var txttomcd=document.getElementById("<%=txtToSupplier.ClientID%>");
                var txttonm=document.getElementById("<%=txtToSupplierName.ClientID%>");
                txttomcd.value=select.options[select.selectedIndex].text;
                txttonm.value=select.options[select.selectedIndex].value;
                break;
            case "tosuppliercode":
                var select=document.getElementById("<%=ddlToSupplier.ClientID%>");
                var selectname=document.getElementById("<%=ddlToSupplierName.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;
           
                var txttomcd=document.getElementById("<%=txtToSupplier.ClientID%>");
                var txttonm=document.getElementById("<%=txtToSupplierName.ClientID%>");
                txttomcd.value=select.options[select.selectedIndex].value;
                txttonm.value=select.options[select.selectedIndex].text;
                
                break;
            case "tosuppliername":
                var select=document.getElementById("<%=ddlToSupplierName.ClientID%>");
                var selectname=document.getElementById("<%=ddlToSupplier.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;
                
                var txttomcd=document.getElementById("<%=txtToSupplier.ClientID%>");
                var txttonm=document.getElementById("<%=txtToSupplierName.ClientID%>");
                txttomcd.value=select.options[select.selectedIndex].text;
                txttonm.value=select.options[select.selectedIndex].value;
                
                break;             
       }
   }

function fillSup(ddlSup)
{
       var sup = document.getElementById(ddlSup);
//       var ddlm1 = document.getElementById("<%=ddlFromSupplier.ClientID%>");
//       var ddlm2 = document.getElementById("<%=ddlFromSupplierName.ClientID%>");
//       var ddlm3 = document.getElementById("<%=ddlToSupplier.ClientID%>");
//       var ddlm4 = document.getElementById("<%=ddlToSupplierName.ClientID%>");
//      
        var strtp = sup.value;          
       if (sup.value != '[Select]') 
       {
          sqlstr1="select Code,des from view_account where type = '"+ strtp +"' order by code";
          sqlstr2="select des,Code from view_account where type = '"+ strtp +"' order by des";
         }
       else
       {
        sqlstr1="select top 10  Code,des from view_account  order by code";
        sqlstr2="select top 10 des,Code from view_account  order by des";
       }
            var connstr=document.getElementById("<%=txtconnection.ClientID%>");  
            constr=connstr.value   


           ColServices.clsServices.GetQueryReturnStringListnew(constr,sqlstr1,FillFromSupCodes,ErrorHandler,TimeOutHandler);
           ColServices.clsServices.GetQueryReturnStringListnew(constr,sqlstr2,FillFromSupNames,ErrorHandler,TimeOutHandler);
           
           ColServices.clsServices.GetQueryReturnStringListnew(constr,sqlstr1,FillToSupCodes,ErrorHandler,TimeOutHandler);
           ColServices.clsServices.GetQueryReturnStringListnew(constr,sqlstr2,FillToSupNames,ErrorHandler,TimeOutHandler);
}
function FillFromSupCodes(result)
    {
      	var ddl = document.getElementById("<%=ddlFromSupplier.ClientID%>");
 		RemoveAll(ddl)
        for(var i=0;i<result.length;i++)
        {
            var option=new Option(result[i].ListText,result[i].ListValue);
            ddl.options.add(option);
        }
        ddl.value="[Select]";
    }

function FillFromSupNames(result)
    {
        var ddl = document.getElementById("<%=ddlFromSupplierName.ClientID%>");
 		RemoveAll(ddl)
        for(var i=0;i<result.length;i++)
        {
            var option=new Option(result[i].ListText,result[i].ListValue);
            ddl.options.add(option);
        }
        ddl.value="[Select]";
    }
 
 function FillToSupCodes(result)
    {
      	var ddl = document.getElementById("<%=ddlToSupplier.ClientID%>");
 		RemoveAll(ddl)
        for(var i=0;i<result.length;i++)
        {
            var option=new Option(result[i].ListText,result[i].ListValue);
            ddl.options.add(option);
        }
        ddl.value="[Select]";
    }

function FillToSupNames(result)
    {
        var ddl = document.getElementById("<%=ddlToSupplierName.ClientID%>");
 		RemoveAll(ddl)
        for(var i=0;i<result.length;i++)
        {
            var option=new Option(result[i].ListText,result[i].ListValue);
            ddl.options.add(option);
        }
        ddl.value="[Select]";
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


//--------Function for diasabled terue/false-----------------------------
function rbevent(rb1,rb2,Opt)
{
var rbcd1 = document.getElementById(rb1);
var rbcd2 = document.getElementById(rb2);
     
var ddlm1 = document.getElementById("<%=ddlFromSupplier.ClientID%>");
var ddlm2 = document.getElementById("<%=ddlFromSupplierName.ClientID%>");
var ddlm3 = document.getElementById("<%=ddlToSupplier.ClientID%>");
var ddlm4 = document.getElementById("<%=ddlToSupplierName.ClientID%>");
        if (Opt == 'A') 
           {
           rbcd1.checked = true;
           rbcd2.checked = false;
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
           rbcd1.checked = false;
           rbcd2.checked = true;
           ddlm1.disabled  = false;
           ddlm2.disabled  = false;
           ddlm3.disabled  = false;
           ddlm4.disabled  = false;
           }
           
}

function ChangeDate()
{
     var txtfdate=document.getElementById("<%=txtFromDate.ClientID%>");
     var txttdate=document.getElementById("<%=txtToDate.ClientID%>");
    
     if (txtfdate.value==''){alert("Enter From Date.");txtfdate.focus();  }
     else {txttdate.value=txtfdate.value;}
}
function FormValidation()
{
  if ((document.getElementById("<%=txtFromDate.ClientID%>").value=="") || (document.getElementById("<%=txtToDate.ClientID%>").value=="") )
       {
           if (document.getElementById("<%=txtFromDate.ClientID%>").value=="")
           {
           
             document.getElementById("<%=txtFromDate.ClientID%>").focus(); 
             alert("From date field can not be blank.");
            return false;
           }
       else if (document.getElementById("<%=txtToDate.ClientID%>").value=="")
           {
           document.getElementById("<%=txtToDate.ClientID%>").focus(); 
           alert("To date field can not be blank.");
            return false;
           }
   }
}

</script>
    <asp:UpdatePanel id="UpdatePanel1" runat="server">
        <contenttemplate>
<TABLE style="BORDER-RIGHT: gray 1px solid; BORDER-TOP: gray 1px solid; BORDER-LEFT: gray 1px solid; BORDER-BOTTOM: gray 1px solid">
<TBODY>
<TR>
<TD class="field_heading" align=center>Supplier Invoices Not Received Report</TD>
</TR>
<TR>
<TD>
<TABLE>
<TBODY>
<TR>
<TD class="field_input">
<TABLE>
<TBODY>
<TR>
<TD style="WIDTH: 100px">
<asp:Label id="lblsupplier" runat="server" Text=" Select Suppiler / Supplier Agent" CssClass="field_caption" Width="157px"></asp:Label>
</TD>
<TD style="WIDTH: 100px">
<SELECT style="WIDTH: 120px" id="ddlSupType" class="field_input" tabIndex=1 runat="server"> 
<OPTION value="S" selected>Supplier</OPTION> <OPTION value="A">SupplierAgent</OPTION>
</SELECT>
</TD>
</TR>
<TR>
<TD style="WIDTH: 100px">
<asp:Label id="Label1" runat="server" Text=" Report Group" CssClass="field_caption" Width="157px"></asp:Label></TD><TD style="WIDTH: 100px"><SELECT style="WIDTH: 120px" id="ddlReportGroup" class="field_input" tabIndex=2 runat="server"> <OPTION value="Code" selected>Code</OPTION><OPTION value="Name">Name</OPTION><OPTION value="[Select]">[Select]</OPTION></SELECT></TD></TR></TBODY></TABLE></TD></TR><TR><TD class="field_input">
    <asp:Panel id="Panel2" runat="server" Height="70px" CssClass="field_input" 
        GroupingText="Select Date Range" Width="490px"><TABLE style="WIDTH: 464px; HEIGHT: 53px"><TBODY><TR><TD style="WIDTH: 55px; HEIGHT: 26px" class="field_input">From Date </TD><TD style="HEIGHT: 26px" class="field_input"><asp:TextBox id="txtFromDate" tabIndex=3 runat="server" CssClass="fiel_input" Width="80px"></asp:TextBox> <asp:ImageButton id="ImgBtnFrmDt" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.png"></asp:ImageButton> <cc1:MaskedEditValidator id="MskVFromDt" runat="server" CssClass="field_error" Width="23px" ControlExtender="MskFromDate" ControlToValidate="txtFromDate" Display="Dynamic" EmptyValueBlurredText="*" EmptyValueMessage="Date is required" ErrorMessage="MskVFromDate" InvalidValueBlurredMessage="*" InvalidValueMessage="Invalid Date" TooltipMessage="Input a date in dd/mm/yyyy format"></cc1:MaskedEditValidator></TD><TD style="HEIGHT: 26px" class="field_input">To Date</TD><TD style="WIDTH: 173px; HEIGHT: 26px" class="field_input"><asp:TextBox id="txtToDate" tabIndex=4 runat="server" CssClass="fiel_input" Width="80px"></asp:TextBox>&nbsp;<asp:ImageButton id="ImgBtnRevDate" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.png"></asp:ImageButton> <cc1:MaskedEditValidator id="MskVToDate" runat="server" CssClass="field_error" ControlExtender="MskToDate" ControlToValidate="txtToDate" Display="Dynamic" EmptyValueBlurredText="*" EmptyValueMessage="Date is required" InvalidValueBlurredMessage="*" InvalidValueMessage="Invalid Date" TooltipMessage="Input a date in dd/mm/yyyy format"></cc1:MaskedEditValidator>&nbsp; </TD></TR></TBODY></TABLE></asp:Panel></TD></TR></TBODY></TABLE><TABLE style="WIDTH: 591px; HEIGHT: 111px"><TBODY><TR><TD class="field_input"><asp:Panel id="Panel3" runat="server" CssClass="td_cell" GroupingText="Select Supplier / Supplier Agent" Width="450px"><TABLE style="WIDTH: 440px"><TBODY><TR><TD class="td_cell"><INPUT id="rbSupall" tabIndex=5 type=radio CHECKED name="Supplier" runat="server" /> All</TD><TD class="td_cell"><asp:Label id="lblacctfrom" runat="server" Text="From"></asp:Label></TD><TD class="td_cell"><SELECT style="WIDTH: 100px" id="ddlFromSupplier" class="field_input" disabled tabIndex=7 onchange="CallWebMethod('fromsuppliercode');" runat="server"> <OPTION value="[Select]" selected>[Select]</OPTION></SELECT> </TD><TD class="td_cell"><SELECT style="WIDTH: 225px" id="ddlFromSupplierName" class="field_input" disabled tabIndex=8 onchange="CallWebMethod('fromsuppliername');" runat="server"> <OPTION value="[Select]" selected>[Select]</OPTION></SELECT> </TD></TR><TR><TD class="td_cell"><INPUT id="rbSuprange" tabIndex=6 type=radio name="Supplier" runat="server" /> Range</TD><TD class="td_cell"><asp:Label id="lblacctto" runat="server" Text="To"></asp:Label></TD><TD class="td_cell"><SELECT style="WIDTH: 100px" id="ddlToSupplier" class="field_input" disabled tabIndex=9 onchange="CallWebMethod('tosuppliercode');" runat="server"> <OPTION value="[Select]" selected>[Select]</OPTION></SELECT> </TD><TD class="td_cell"><SELECT style="WIDTH: 225px" id="ddlToSupplierName" class="field_input" disabled tabIndex=10 onchange="CallWebMethod('tosuppliername');" runat="server"> <OPTION value="[Select]" selected>[Select]</OPTION></SELECT> </TD></TR></TBODY></TABLE></asp:Panel></TD></TR></TBODY></TABLE></TD></TR><TR><TD class="field_input" align=right>
    <input id="txtconnection" runat="server" style="visibility: hidden; width: 12px;
        height: 9px" type="text" />
    <INPUT style="VISIBILITY: hidden; WIDTH: 5px" id="txtFromSupplier" type=text maxLength=100 runat="server" /> <INPUT style="VISIBILITY: hidden; WIDTH: 5px" id="txtFromSupplierName" type=text maxLength=100 runat="server" /> <INPUT style="VISIBILITY: hidden; WIDTH: 5px" id="txtToSupplier" type=text maxLength=100 runat="server" /> <INPUT style="VISIBILITY: hidden; WIDTH: 5px" id="txtToSupplierName" type=text maxLength=100 runat="server" /> 
    <asp:Button id="btnLoadreport" tabIndex=11 onclick="btnLoadreport_Click" 
        runat="server" Text="Load Reports " CssClass="field_button"></asp:Button>&nbsp;<asp:Button 
        id="btnExit" tabIndex=12 onclick="btnExit_Click" runat="server" Text=" Exit" 
        CssClass="field_button" CausesValidation="False"></asp:Button>&nbsp;<asp:Button 
        id="btnhelp" tabIndex=13 onclick="btnhelp_Click" runat="server" Text="Help" 
        CssClass="field_button"></asp:Button></TD></TR></TBODY></TABLE><asp:ScriptManagerProxy id="ScriptManagerProxy1" runat="server"><Services>
<asp:ServiceReference Path="~/clsServices.asmx"></asp:ServiceReference>
</Services>
</asp:ScriptManagerProxy> <cc1:CalendarExtender id="ClsExFromDate" runat="server" Format="dd/MM/yyyy" PopupButtonID="ImgBtnFrmDt" TargetControlID="txtFromDate">
    </cc1:CalendarExtender> <cc1:CalendarExtender id="ClsExToDate" runat="server" Format="dd/MM/yyyy" PopupButtonID="ImgBtnRevDate" TargetControlID="txtToDate">
    </cc1:CalendarExtender> <cc1:MaskedEditExtender id="MskFromDate" runat="server" TargetControlID="txtFromDate" AcceptNegative="Left" DisplayMoney="Left" ErrorTooltipEnabled="True" Mask="99/99/9999" MaskType="Date" MessageValidatorTip="true">
    </cc1:MaskedEditExtender> <cc1:MaskedEditExtender id="MskToDate" runat="server" TargetControlID="txtToDate" AcceptNegative="Left" DisplayMoney="Left" ErrorTooltipEnabled="True" Mask="99/99/9999" MaskType="Date" MessageValidatorTip="true">
    </cc1:MaskedEditExtender> 
</contenttemplate>
    </asp:UpdatePanel>
    &nbsp;<br />
</asp:Content>

