<%@ Page Language="VB" MasterPageFile="~/AccountsMaster.master" AutoEventWireup="false" CodeFile="RptProfitSalesreportSearch.aspx.vb" Inherits="RptProfitSalesreportSearch" %>

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

function AllRange(rb,Opt,Group)
{
       var rb = document.getElementById(rb);
       rb.checked = true;
       switch (Group)
       {
           case "Cntry":
                var ddlm1 = document.getElementById("<%=ddlCountryCD.ClientID%>");
                var ddlm2 = document.getElementById("<%=ddlCountryNM.ClientID%>");
                var ddlm3 = document.getElementById("<%=ddltoCountryCD.ClientID%>");
                var ddlm4 = document.getElementById("<%=ddltoCountryNM.ClientID%>");
                var lbl1 = document.getElementById("<%=lblctryfrom.ClientID%>");
                var lbl2 = document.getElementById("<%=lblctryto.ClientID%>");
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

//           ddlm1.style .visibility="hidden";
//           ddlm2.style .visibility="hidden";
//           ddlm3.style .visibility="hidden";
//           ddlm4.style .visibility="hidden";
//           lbl1.style .visibility="hidden";
//           lbl2.style .visibility="hidden";

       }
       else
       {
           ddlm1.disabled  = false;
           ddlm2.disabled  = false;
           ddlm3.disabled  = false;
           ddlm4.disabled  = false;

//           ddlm1.style .visibility="visible";
//           ddlm2.style .visibility="visible";
//           ddlm3.style .visibility="visible";
//           ddlm4.style .visibility="visible";
//           lbl1.style .visibility="visible";
//           lbl2.style .visibility="visible";

       }
          
}


function CallWebMethod(methodType)
    {
       switch(methodType)
        {
              case "cntrycode":
                var select=document.getElementById("<%=ddlCountryCD.ClientID%>");
                var codeid=select.options[select.selectedIndex].text;
                var selectname=document.getElementById("<%=ddlCountryNM.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;
                
                 var select=document.getElementById("<%=ddltoCountryCD.ClientID%>");
                select.options[select.selectedIndex].text=codeid;
                var selectname=document.getElementById("<%=ddltoCountryNM.ClientID%>");
                selectname.value=codeid;                
               
                var txttocode=document.getElementById("<%=txtTocountyCode.ClientID%>");
                 txttocode.value= select.options[select.selectedIndex].text;
                 var txttoname=document.getElementById("<%=txtToCountryName.ClientID%>");
                 txttoname.value= selectname.options[selectname.selectedIndex].text;
               

                
                break;
            case "cntryname":
                var select=document.getElementById("<%=ddlCountryNM.ClientID%>");
                var codeid=select.options[select.selectedIndex].text;
                var selectname=document.getElementById("<%=ddlCountryCD.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;
                
                 var select=document.getElementById("<%=ddltoCountryNM.ClientID%>");
                select.options[select.selectedIndex].text=codeid;
                var selectname=document.getElementById("<%=ddltoCountryCD.ClientID%>");
                selectname.value=codeid;
              
                  var txttocode=document.getElementById("<%=txtTocountyCode.ClientID%>");
                 txttocode.value=selectname.options[selectname.selectedIndex].text;
                 var txttoname=document.getElementById("<%=txtToCountryName.ClientID%>");
                 txttoname.value= select.options[select.selectedIndex].text;

            break;
             case "tocntrycode":
                var select=document.getElementById("<%=ddltoCountryCD.ClientID%>");
                var codeid=select.options[select.selectedIndex].text;
                var selectname=document.getElementById("<%=ddltoCountryNM.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;
                
//                var select=document.getElementById("<%=ddlCountryCD.ClientID%>");
//                select.options[select.selectedIndex].text=codeid;
//                var selectname=document.getElementById("<%=ddlCountryNM.ClientID%>");
//                selectname.value=codeid;  

                  var txttocode=document.getElementById("<%=txtTocountyCode.ClientID%>");
                 txttocode.value= select.options[select.selectedIndex].text;
                 var txttoname=document.getElementById("<%=txtToCountryName.ClientID%>");
                 txttoname.value= selectname.options[selectname.selectedIndex].text;
               

              
             break;
            case "tocntryname":
                var select=document.getElementById("<%=ddltoCountryNM.ClientID%>");
                var codeid=select.options[select.selectedIndex].text;
                var selectname=document.getElementById("<%=ddltoCountryCD.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;
                
//                 var select=document.getElementById("<%=ddlCountryNM.ClientID%>");
//                select.options[select.selectedIndex].text=codeid;
//                var selectname=document.getElementById("<%=ddlCountryCD.ClientID%>");
//                selectname.value=codeid;

                var txttocode=document.getElementById("<%=txtTocountyCode.ClientID%>");
                 txttocode.value=selectname.options[selectname.selectedIndex].text;
                 var txttoname=document.getElementById("<%=txtToCountryName.ClientID%>");
                 txttoname.value= select.options[select.selectedIndex].text;

            break;
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
 
</script>
    <asp:UpdatePanel id="UpdatePanel1" runat="server">
        <contenttemplate>
<TABLE style="BORDER-RIGHT: gray 1px solid; BORDER-TOP: gray 1px solid; BORDER-LEFT: gray 1px solid; BORDER-BOTTOM: gray 1px solid"><TBODY><TR><TD style="HEIGHT: 17px; TEXT-ALIGN: center" class=" field_heading" colSpan=5>Sales - Profit & Loss &nbsp;Report</TD></TR><TR><TD colSpan=3><TABLE style="WIDTH: 562px; HEIGHT: 201px" class="td_cell"><TBODY><TR><TD colSpan=4><TABLE style="WIDTH: 653px; HEIGHT: 179px"><TBODY><TR><TD><asp:Label id="Label1" runat="server" Text="Invoice No" CssClass="field_caption" Width="120px"></asp:Label></TD><TD><INPUT style="WIDTH: 194px" id="txtInvoiceNo" class="field_input" tabIndex=1 type=text runat="server" /></TD><TD></TD><TD></TD></TR><TR><TD><asp:Label id="Label2" runat="server" Text="From Invoice Date" CssClass="field_caption" Width="120px"></asp:Label></TD><TD><asp:TextBox id="txtFromDate" tabIndex=3 runat="server" CssClass="fiel_input" Width="80px"></asp:TextBox>&nbsp; <asp:ImageButton id="ImgBtnFrmDt" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.png"></asp:ImageButton> <cc1:MaskedEditValidator id="MEVFromDate" runat="server" CssClass="field_error" TooltipMessage="Input a date in dd/mm/yyyy format" InvalidValueMessage="Invalid Date" InvalidValueBlurredMessage="Invalid Date" EmptyValueMessage="Date is required" EmptyValueBlurredText="Date is required" Display="Dynamic" ControlToValidate="txtFromDate" ControlExtender="MEFromDate"></cc1:MaskedEditValidator></TD><TD><asp:Label id="Label5" runat="server" Text="To Invoice Date" CssClass="field_caption" Width="120px"></asp:Label></TD><TD><asp:TextBox id="txtToDate" tabIndex=4 runat="server" CssClass="fiel_input" Width="80px"></asp:TextBox> <asp:ImageButton id="ImgBtnRevDate" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.png"></asp:ImageButton> <cc1:MaskedEditValidator id="MEVToDate" runat="server" CssClass="field_error" TooltipMessage="Input a date in dd/mm/yyyy format" InvalidValueMessage="Invalid Date" InvalidValueBlurredMessage="Invalid Date" EmptyValueMessage="Date is required" EmptyValueBlurredText="Date is required" Display="Dynamic" ControlToValidate="txtToDate" ControlExtender="METoDate"></cc1:MaskedEditValidator></TD></TR><TR><TD style="HEIGHT: 26px"><asp:Label id="Label4" runat="server" Text="Status" CssClass="field_caption" Width="120px"></asp:Label></TD><TD style="HEIGHT: 26px"><SELECT style="WIDTH: 154px" id="ddlStatus" class="td_cell" tabIndex=5 runat="server"> <OPTION value="[Select]" selected>[Select]</OPTION> <OPTION value="P">Posted</OPTION> <OPTION value="U">UnPosted</OPTION></SELECT></TD><TD style="HEIGHT: 26px"></TD><TD style="HEIGHT: 26px"></TD></TR><TR><TD><asp:Label id="Label6" runat="server" Text="Customer" CssClass="field_caption" Width="120px"></asp:Label></TD><TD>

<input type="text" name="accSearch"  class="field_input MyAutoCompleteClass"  onfocus="MyAutoCustomer_rptFillArray();" style="width:98% ; font " id="accSearch"  runat="server" />
<SELECT style="WIDTH: 200px" id="ddlCustomer" class="field_input MyDropDownListCustValue" tabIndex=6 runat="server"> <OPTION selected></OPTION></SELECT></TD><TD></TD><TD></TD></TR><TR><TD>Report Type</TD><TD><asp:DropDownList id="ddlrpttype" runat="server" CssClass="field_input" Width="216px" ><asp:ListItem Value="0">Summary</asp:ListItem>
<asp:ListItem Value="1">Detail</asp:ListItem>
<asp:ListItem Value="2">Top order agent sales </asp:ListItem>
</asp:DropDownList></TD><TD></TD><TD>&nbsp;</TD></TR>
    <tr>
        <td colspan="4">
            <asp:Panel ID="Panel4" runat="server" Font-Bold="True" GroupingText="Country" Width="700px">
                <table align="left" class="td_cell" style="width: 495px">
                    <tbody>
                        <tr>
                            <td align="left" rowspan="2" style="width: 77px">
                                <table>
                                    <tbody>
                                        <tr>
                                            <td style="width: 100px">
                                                <input id="rdbtnCountryAll" runat="server"  name="5" type="radio" CHECKED />
                                                All</td>
                                        </tr>
                                        <tr>
                                            <td style="width: 100px; height: 22px">
                                                <input id="rdbtnCountryRange" runat="server" name="5" type="radio" />Range</td>
                                        </tr>
                                    </tbody>
                                </table>
                            </td>
                            <td align="left" style="width: 188px">
                                <asp:Label ID="lblctryfrom" runat="server" Text="From"></asp:Label></td>
                            <td align="left" style="width: 188px">
                                <select id="ddlCountryCD" runat="server" class="field_input" onchange="CallWebMethod('cntrycode');"
                                    style="width: 158px">
                                    <option selected="selected"></option>
                                </select>
                            </td>
                            <td align="left">
                                <select id="ddlCountryNM" runat="server" class="field_input" onchange="CallWebMethod('cntryname');"
                                    style="width: 199px">
                                    <option selected="selected"></option>
                                </select>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 188px">
                                <asp:Label ID="lblctryto" runat="server" Text="To"></asp:Label></td>
                            <td style="width: 188px">
                                <select id="ddltoCountryCD" runat="server" class="field_input" onchange="CallWebMethod('tocntrycode');"
                                    style="width: 158px">
                                    <option selected="selected"></option>
                                </select>
                            </td>
                            <td align="left">
                                <select id="ddltoCountryNM" runat="server" class="field_input" onchange="CallWebMethod('tocntryname');"
                                    style="width: 199px">
                                    <option selected="selected"></option>
                                </select>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </asp:Panel>
        </td>
    </tr>
    <TR><TD align=right colSpan=4>
        &nbsp;
        <input id="txtTocountyCode" runat="server" style="visibility: hidden; width: 1px"
            type="text" />
        <input id="txtToCountryName" runat="server" style="visibility: hidden;
                width: 1px" type="text" />
        &nbsp;&nbsp;
        <INPUT style="VISIBILITY: hidden; WIDTH: 12px; HEIGHT: 9px" id="txtconnection" type=text runat="server" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
        <asp:Button id="btnLoadreport" tabIndex=12 onclick="btnLoadreport_Click" 
            runat="server" Text="Load Reports " CssClass="field_button"></asp:Button>&nbsp;
         <asp:Button id="btnClear" tabIndex=13 onclick="btnClear_Click" runat="server" 
            Text="Clear" Font-Bold="True" CssClass="field_button"></asp:Button>&nbsp;
          <asp:Button id="btnExit" tabIndex=14 onclick="btnExit_Click" runat="server" 
            Text=" Exit" CssClass="field_button" CausesValidation="False"></asp:Button>
         &nbsp; <asp:Button id="btnhelp" tabIndex=15 onclick="btnhelp_Click" 
            runat="server" Text="Help" Height="20px" CssClass="field_button"></asp:Button></TD></TR></TBODY></TABLE></TD></TR></TBODY></TABLE><cc1:CalendarExtender id="CEFromDate" runat="server" Format="dd/MM/yyyy" PopupButtonID="ImgBtnFrmDt" TargetControlID="txtFromDate"></cc1:CalendarExtender><cc1:MaskedEditExtender id="MEFromDate" runat="server" TargetControlID="txtFromDate" Mask="99/99/9999" MaskType="Date"></cc1:MaskedEditExtender><cc1:CalendarExtender id="CEToDate" runat="server" Format="dd/MM/yyyy" PopupButtonID="ImgBtnRevDate" TargetControlID="txtToDate"></cc1:CalendarExtender> &nbsp; <cc1:MaskedEditExtender id="METoDate" runat="server" TargetControlID="txtToDate" Mask="99/99/9999" MaskType="Date"></cc1:MaskedEditExtender></TD></TR></TBODY></TABLE><asp:ScriptManagerProxy id="ScriptManagerProxy1" runat="server">
                    <services>
<asp:ServiceReference Path="~/clsServices.asmx"></asp:ServiceReference>
</services>
                </asp:ScriptManagerProxy> 
</contenttemplate>

    </asp:UpdatePanel>
</asp:Content>

