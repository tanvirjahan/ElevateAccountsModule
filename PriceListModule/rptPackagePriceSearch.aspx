<%@ Page Language="VB" AutoEventWireup="false" CodeFile="rptPackagePriceSearch.aspx.vb" Inherits="rptPackagePriceSearch"  MasterPageFile="~/PriceListMaster.master" Strict="true"  %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

 <%@ Register Assembly="EclipseWebSolutions.DatePicker" Namespace="EclipseWebSolutions.DatePicker"
    TagPrefix="ews" %>
    <%@ OutputCache location="none" %> 
    

<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">
<%--<script language="javascript" src="js\date-picker.js"></script>  --%>
    <%--<script language="javascript" src="js\datefun.js"></script>--%>
   <%--//    (document.getElementById("<%=txtFromDate.ClientID%>").value=="")||(document.getElementById("<%=txtToDate.ClientID%>").value=="") )--%>

<script type="text/javascript">
<!--
WebForm_AutoFocus('ctl00_SampleContent_WUCCamperMenuOptionsAsTab2_TabStCamperOptions_TbCamperLkp_WUCCamperLookup2_WUCCamperInquiry1_TBLastName');
// -->
</script>
<script language="javascript" type="text/javascript" >
function CallWebMethod(methodType)
    {
        switch(methodType)
        {

            case "marketcode":
                var select=document.getElementById("<%=ddlMarketCode.ClientID%>");
                var plgrp=select.options[select.selectedIndex].text;
                var selectname=document.getElementById("<%=ddlMarketName.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;
                break; 

            case "marketname":
                var select=document.getElementById("<%=ddlMarketName.ClientID%>");
                var plgrp=select.options[select.selectedIndex].value;
                var selectname=document.getElementById("<%=ddlMarketCode.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;
                break;

            case "sellcode":
                var select=document.getElementById("<%=ddlSellCode.ClientID%>");
                var sell=select.options[select.selectedIndex].text;
                var selectname=document.getElementById("<%=ddlSellName.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;
                break; 

            case "sellname":
                var select=document.getElementById("<%=ddlSellName.ClientID%>");
                var sell=select.options[select.selectedIndex].value;
                var selectname=document.getElementById("<%=ddlSellCode.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;
                break;

            case "tktsellcode":
                var select=document.getElementById("<%=ddlTicketSellCode.ClientID%>");
                var tktsell=select.options[select.selectedIndex].text;
                var selectname=document.getElementById("<%=ddlTicketSellName.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;
                break; 

            case "tktsellname":
                var select=document.getElementById("<%=ddlTicketSellName.ClientID%>");
                var tktsell=select.options[select.selectedIndex].value;
                var selectname=document.getElementById("<%=ddlTicketSellCode.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;
                break;

            case "othsellcode":
                var select=document.getElementById("<%=ddlOtherSellCode.ClientID%>");
                var othsell=select.options[select.selectedIndex].text;
                var selectname=document.getElementById("<%=ddlOtherSellName.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;
                break; 

            case "othsellname":
                var select=document.getElementById("<%=ddlOtherSellName.ClientID%>");
                var othsell=select.options[select.selectedIndex].value;
                var selectname=document.getElementById("<%=ddlOtherSellCode.ClientID%>");
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


function checkTelephoneNumber(e)
			{	    
			    	
				if ( (event.keyCode < 45 || event.keyCode > 57) )
				{
					return false;
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

function ChangeDate()
{
   
     var txtfdate=document.getElementById("<%=txtFromDate.ClientID%>");
     //var txttdate=document.getElementById("<%=txtToDate.ClientID%>");
   
       if (txtfdate.value==''){alert("Enter From Date.");txtfdate.focus();  }
      else {ColServices.clsServices.GetQueryReturnFromToDate('FromDate',30,txtfdate.value,FillToDate,ErrorHandler,TimeOutHandler);}
}
function FillToDate(result)
    {
       	 var txttdate=document.getElementById("<%=txtToDate.ClientID%>");
      	 txttdate.value=result;
    }
    
</script> 

    <table>
        <tr>
            <td style="width: 100px">
                <table style="border-right: gray 2px solid; border-top: gray 2px solid; border-left: gray 2px solid;
                    width: 100%; border-bottom: gray 2px solid">
                    <tr>
                        <td class="field_heading" style="text-align: center">
                            Package Price List Report</td>
                    </tr>
                    <tr>
                        <td style="text-align: center">
                            <span class="td_cell" style="color: #ff0000"></span>
                        </td>
                    </tr>
                    <tr>
                        <td>
  
                            <asp:UpdatePanel id="UpdatePanel1" runat="server">
                                <contenttemplate>
<TABLE style="WIDTH: 100%"><TBODY><TR><TD style="WIDTH: 197px"><asp:Label id="Label1" runat="server" Text="Package ID" CssClass="td_cell" Width="100px"></asp:Label></TD><TD><asp:TextBox id="txtpackage" runat="server" CssClass="td_cell" Width="164px"></asp:TextBox></TD><TD></TD><TD></TD></TR><TR><TD style="WIDTH: 197px"><asp:Label id="lblFromDate" runat="server" Text="From Date" Height="16px" CssClass="td_cell"></asp:Label></TD><TD><asp:TextBox id="txtFromDate" runat="server" CssClass="fiel_input" Width="80px"></asp:TextBox><asp:ImageButton id="ImgBtnFrmDt" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.png"></asp:ImageButton><cc1:MaskedEditValidator id="MEVFromDate" runat="server" CssClass="field_error" Display="Dynamic" ControlToValidate="txtFromDate" ControlExtender="MEFromDate" EmptyValueBlurredText="Date is required" EmptyValueMessage="Date is required" InvalidValueBlurredMessage="Invalid Date" InvalidValueMessage="Invalid Date" TooltipMessage="Input a date in dd/mm/yyyy format"></cc1:MaskedEditValidator></TD><TD><asp:Label id="lblTodate" runat="server" Text="To Date" CssClass="td_cell"></asp:Label></TD><TD><asp:TextBox id="txtToDate" runat="server" CssClass="fiel_input" Width="80px"></asp:TextBox><asp:ImageButton id="ImgBtnToDate" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.png"></asp:ImageButton> <cc1:MaskedEditValidator id="MEVToDate" runat="server" CssClass="field_error" Display="Dynamic" ControlToValidate="txtToDate" ControlExtender="METoDate" EmptyValueBlurredText="Date is required" EmptyValueMessage="Date is required" InvalidValueBlurredMessage="Invalid Date" InvalidValueMessage="Invalid Date" TooltipMessage="Input a date in dd/mm/yyyy format"></cc1:MaskedEditValidator></TD></TR><TR><TD style="WIDTH: 197px"><asp:Label id="lblMarketCode" runat="server" Text="Market Code" CssClass="td_cell" Width="100px"></asp:Label></TD><TD><SELECT style="WIDTH: 170px" id="ddlMarketCode" class="drpdown" tabIndex=3 onchange="CallWebMethod('marketcode')" runat="server" Visible="true"> <OPTION selected></OPTION></SELECT></TD><TD><asp:Label id="lblMarketName" runat="server" Text="Market Name" CssClass="td_cell" Width="100px"></asp:Label></TD><TD><SELECT style="WIDTH: 250px" id="ddlMarketName" class="drpdown" tabIndex=4 onchange="CallWebMethod('marketname')" runat="server" Visible="true"> <OPTION selected></OPTION></SELECT></TD></TR><TR><TD style="WIDTH: 197px"><asp:Label id="Label3" runat="server" Text="Hotel Selling Type Code" CssClass="td_cell" Width="150px"></asp:Label></TD><TD><SELECT style="WIDTH: 170px" id="ddlSellCode" class="drpdown" tabIndex=5 onchange="CallWebMethod('sellcode')" runat="server" Visible="true"> <OPTION selected></OPTION></SELECT></TD><TD><asp:Label id="Label6" runat="server" Text="Hotel Selling Type Name" CssClass="td_cell" Width="150px"></asp:Label></TD><TD><SELECT style="WIDTH: 250px" id="ddlSellName" class="drpdown" tabIndex=6 onchange="CallWebMethod('sellname')" runat="server" Visible="true"> <OPTION selected></OPTION></SELECT></TD></TR><TR><TD style="WIDTH: 197px"><asp:Label id="Label4" runat="server" Text="Ticket Selling Type Code" CssClass="td_cell" Width="150px"></asp:Label></TD><TD><SELECT style="WIDTH: 170px" id="ddlTicketSellCode" class="drpdown" tabIndex=7 onchange="CallWebMethod('tktsellcode')" runat="server" Visible="true"> <OPTION selected></OPTION></SELECT></TD><TD><asp:Label id="Label7" runat="server" Text="Ticket Selling Type Name" CssClass="td_cell" Width="150px"></asp:Label></TD><TD><SELECT style="WIDTH: 250px" id="ddlTicketSellName" class="drpdown" tabIndex=8 onchange="CallWebMethod('tktsellname')" runat="server" Visible="true"> <OPTION selected></OPTION></SELECT></TD></TR><TR><TD style="WIDTH: 197px"><asp:Label id="Label5" runat="server" Text="Other Selling Type Code" CssClass="td_cell" Width="150px"></asp:Label></TD><TD><SELECT style="WIDTH: 170px" id="ddlOtherSellCode" class="drpdown" tabIndex=9 onchange="CallWebMethod('othsellcode')" runat="server" Visible="true"> <OPTION selected></OPTION></SELECT></TD><TD><asp:Label id="Label8" runat="server" Text="Other Selling Type Name" CssClass="td_cell" Width="150px"></asp:Label></TD><TD><SELECT style="WIDTH: 250px" id="ddlOtherSellName" class="drpdown" tabIndex=10 onchange="CallWebMethod('othsellname')" runat="server" Visible="true"> <OPTION selected></OPTION></SELECT></TD></TR>
    <tr>
        <td style="width: 197px">
            <asp:Label ID="lblatatus" runat="server" CssClass="td_cell" Text="Approval Status"
                Width="103px"></asp:Label></td>
        <td>
            <asp:DropDownList ID="ddlapprovestatus" runat="server" CssClass="drpdown" Width="191px">
                <asp:ListItem Value="2">All</asp:ListItem>
                <asp:ListItem Value="1">Approve</asp:ListItem>
                <asp:ListItem Value="0">Unapprove</asp:ListItem>
            </asp:DropDownList></td>
        <td>
        </td>
        <td>
        </td>
    </tr>
    <TR><TD style="WIDTH: 197px"><asp:Label id="Label2" runat="server" Text="Report Type" CssClass="td_cell" Width="100px"></asp:Label></TD><TD><asp:RadioButton id="rbBrief" tabIndex=11 runat="server" Text="Brief" CssClass="btn" Width="80px" Checked="True" GroupName="reporttype"></asp:RadioButton>&nbsp;<asp:RadioButton id="RadioButton1" tabIndex=12 runat="server" Text="Detailed" CssClass="btn" Width="80px" GroupName="reporttype"></asp:RadioButton></TD><TD>
        <input id="txtconnection" runat="server" style="visibility: hidden; width: 12px;
            height: 9px" type="text" /></TD><TD></TD></TR><TR><TD style="TEXT-ALIGN: center" colSpan=4>
            <asp:Button id="BtnClear" tabIndex=14 runat="server" Text="Clear" 
                CssClass="btn"></asp:Button>&nbsp;
             <asp:Button id="BtnPrint" tabIndex=13 runat="server" Text="Load Report" CssClass="btn"></asp:Button>&nbsp;
              <asp:Button id="btnhelp" tabIndex=15 onclick="btnhelp_Click" runat="server" 
                Text="Help" CssClass="btn"></asp:Button></TD></TR></TBODY></TABLE> <cc1:CalendarExtender id="CEFromDate" runat="server" TargetControlID="txtFromDate" PopupButtonID="ImgBtnFrmDt" Format="dd/MM/yyyy"></cc1:CalendarExtender> <cc1:MaskedEditExtender id="MEFromDate" runat="server" TargetControlID="txtFromDate" Mask="99/99/9999" MaskType="Date"></cc1:MaskedEditExtender> <cc1:CalendarExtender id="CEToDate" runat="server" TargetControlID="txtToDate" PopupButtonID="ImgBtnToDate" Format="dd/MM/yyyy"></cc1:CalendarExtender> <cc1:MaskedEditExtender id="METoDate" runat="server" TargetControlID="txtToDate" Mask="99/99/9999" MaskType="Date" Enabled="True"></cc1:MaskedEditExtender>
</contenttemplate>
                            </asp:UpdatePanel></td>
                    </tr>
                    <tr>
                        <td style="text-align: center;">
                            &nbsp; &nbsp;
                        </td>
                    </tr>
                </table>
                </td>
        </tr>
    </table>
    <asp:ScriptManagerProxy id="ScriptManagerProxy1" runat="server">
        <services>
<asp:ServiceReference Path="~/clsServices.asmx"></asp:ServiceReference>
</services>
    </asp:ScriptManagerProxy>
    &nbsp; &nbsp; &nbsp; &nbsp;
    <br />

</asp:Content>