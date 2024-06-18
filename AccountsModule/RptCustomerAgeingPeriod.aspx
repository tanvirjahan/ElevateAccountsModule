<%@ Page Language="VB" AutoEventWireup="false" CodeFile="RptCustomerAgeingPeriod.aspx.vb" Inherits="RptCustomerAgeingPeriod" MasterPageFile="~/AccountsMaster.master" Strict ="true" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="server">

<script language="javascript" type="text/javascript">

function FormValidation()
{
  var period1=document.getElementById("<%=period1.ClientID%>");
  var period2=document.getElementById("<%=period2.ClientID%>");
  
  if (period1.value=="[Select]")
  {
  period1.focus();
  alert("Select From Period");
  return false;
  }

  if (period2.value=="[Select]")
  {
  period2.focus();
  alert("Select To Period");
  return false;
  }
  

  if ((document.getElementById("<%=txtFromDate.ClientID%>").value=="")  || (document.getElementById("<%=ddlReportGroup.ClientID%>").value=="[Select]")  || (document.getElementById("<%=ddlCurrency.ClientID%>").value=="[Select]")||(document.getElementById("<%=ddlReportOrder.ClientID%>").value=="[Select]")||(document.getElementById("<%=ddlAgeingType.ClientID%>").value=="[Select]")) 
     {

    //   alert(document.getElementById("<%=period1.ClientID%>").value);
     
          if (document.getElementById("<%=txtFromDate.ClientID%>").value=="")
           {
            document.getElementById("<%=txtFromDate.ClientID%>").focus(); 
            alert("As on date field can not be blank.");
            return false;
           }
           else if (document.getElementById("<%=ddlReportGroup.ClientID%>").value=="[Select]") 
           {
           document.getElementById("<%=ddlReportGroup.ClientID%>").focus();
           alert("Select report group.");
           return false;
           }
           
         else if (document.getElementById("<%=ddlCurrency.ClientID%>").value=="[Select]")
           {
            document.getElementById("<%=ddlCurrency.ClientID%>").focus();
            alert("Select currency type.");
            return false;
        }

          else if (document.getElementById("<%=ddlReportOrder.ClientID%>").value=="[Select]")
           {
            document.getElementById("<%=ddlReportOrder.ClientID%>").focus();
            alert("Select report order.");
            return false;
            }
          else if (document.getElementById("<%=ddlAgeingType.ClientID%>").value=="[Select]")
           {
            document.getElementById("<%=ddlAgeingType.ClientID%>").focus();
            alert("Select ageing type.");
            return false;
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
			
function CallWebMethod(methodType)
    {
       switch(methodType)
        {
            case "fromaccountcode":
                var select=document.getElementById("<%=ddlFromAccount.ClientID%>");
                var codeid=select.options[select.selectedIndex].text;
                var selectname=document.getElementById("<%=ddlFromAccountName.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;
                
                var select1=document.getElementById("<%=ddlToAccount.ClientID%>");
                select1.value=select.options[select.selectedIndex].value;
                var selectname1=document.getElementById("<%=ddlToAccountName.ClientID%>");
                selectname1.value=select.options[select.selectedIndex].text;
                
                break;
            case "fromaccountname":
                var select=document.getElementById("<%=ddlFromAccountName.ClientID%>");
                var codeid=select.options[select.selectedIndex].value;
                var selectname=document.getElementById("<%=ddlFromAccount.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;
                
                var select1=document.getElementById("<%=ddlToAccount.ClientID%>");
                select1.value=select.options[select.selectedIndex].text;
                var selectname1=document.getElementById("<%=ddlToAccountName.ClientID%>");
                selectname1.value=select.options[select.selectedIndex].value;
                
                break;
            case "toaccountcode":
                var select=document.getElementById("<%=ddlToAccount.ClientID%>");
                var codeid=select.options[select.selectedIndex].text;
                var selectname=document.getElementById("<%=ddlToAccountName.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;
                
                break;
            case "toaccountname":
                var select=document.getElementById("<%=ddlToAccountName.ClientID%>");
                var codeid=select.options[select.selectedIndex].value;
                var selectname=document.getElementById("<%=ddlToAccount.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;
                
                break;
            case "fromcontrolcode":
                var select=document.getElementById("<%=ddlFromControl.ClientID%>");
                var codeid=select.options[select.selectedIndex].text;
                var selectname=document.getElementById("<%=ddlFromControlName.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;
                
                var select1=document.getElementById("<%=ddlToControl.ClientID%>");
                select1.value=select.options[select.selectedIndex].value;
                var selectname1=document.getElementById("<%=ddlToControlName.ClientID%>");
                selectname1.value=select.options[select.selectedIndex].text;
                
                break;
            case "fromcontrolname":
                var select=document.getElementById("<%=ddlFromControlName.ClientID%>");
                var codeid=select.options[select.selectedIndex].value;
                var selectname=document.getElementById("<%=ddlFromControl.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;
                
                var select1=document.getElementById("<%=ddlToControl.ClientID%>");
                select1.value=select.options[select.selectedIndex].text;
                var selectname1=document.getElementById("<%=ddlToControlName.ClientID%>");
                selectname1.value=select.options[select.selectedIndex].value;
                
                break;
            case "tocontrolcode":
                var select=document.getElementById("<%=ddlToControl.ClientID%>");
                var codeid=select.options[select.selectedIndex].text;
                var selectname=document.getElementById("<%=ddlToControlName.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;
                
                break;
            case "tocontrolname":
                var select=document.getElementById("<%=ddlToControlName.ClientID%>");
                var codeid=select.options[select.selectedIndex].value;
                var selectname=document.getElementById("<%=ddlToControl.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;
                break;
            case "fromcategorycode":
                var select=document.getElementById("<%=ddlFromCategory.ClientID%>");
                var codeid=select.options[select.selectedIndex].text;
                var selectname=document.getElementById("<%=ddlFromCategoryName.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;
                
                var select1=document.getElementById("<%=ddlToCategory.ClientID%>");
                select1.value=select.options[select.selectedIndex].value;
                var selectname1=document.getElementById("<%=ddlToCategoryName.ClientID%>");
                selectname1.value=select.options[select.selectedIndex].text;
                
                break;
            case "fromcategoryname":
                var select=document.getElementById("<%=ddlFromCategoryName.ClientID%>");
                var codeid=select.options[select.selectedIndex].value;
                var selectname=document.getElementById("<%=ddlFromCategory.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;
                
                var select1=document.getElementById("<%=ddlToCategory.ClientID%>");
                select1.value=select.options[select.selectedIndex].text;
                var selectname1=document.getElementById("<%=ddlToCategoryName.ClientID%>");
                selectname1.value=select.options[select.selectedIndex].value;
                
                break;
            case "tocategorycode":
                var select=document.getElementById("<%=ddlToCategory.ClientID%>");
                var codeid=select.options[select.selectedIndex].text;
                var selectname=document.getElementById("<%=ddlToCategoryName.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;
                
                break;
            case "tocategoryname":
                var select=document.getElementById("<%=ddlToCategoryName.ClientID%>");
                var codeid=select.options[select.selectedIndex].value;
                var selectname=document.getElementById("<%=ddlToCategory.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;
                
                break;
            case "frommarketcode":
                var select=document.getElementById("<%=ddlFromMarket.ClientID%>");
                var codeid=select.options[select.selectedIndex].text;
                var selectname=document.getElementById("<%=ddlFromMarketName.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;
                 
                var select1=document.getElementById("<%=ddlToMarket.ClientID%>");
                select1.value=select.options[select.selectedIndex].value;
                var selectname1=document.getElementById("<%=ddlToMarketName.ClientID%>");
                selectname1.value=select.options[select.selectedIndex].text;
                
                break;
            case "frommarketname":
                var select=document.getElementById("<%=ddlFromMarketName.ClientID%>");
                var codeid=select.options[select.selectedIndex].value;
                var selectname=document.getElementById("<%=ddlFromMarket.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;
                 
                var select1=document.getElementById("<%=ddlToMarket.ClientID%>");
                select1.value=select.options[select.selectedIndex].text;
                var selectname1=document.getElementById("<%=ddlToMarketName.ClientID%>");
                selectname1.value=select.options[select.selectedIndex].value;
                
                break;
            case "tomarketcode":
                var select=document.getElementById("<%=ddlToMarket.ClientID%>");
                var codeid=select.options[select.selectedIndex].text;
                var selectname=document.getElementById("<%=ddlToMarketName.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;
                
                break;
            case "tomarketname":
                var select=document.getElementById("<%=ddlToMarketName.ClientID%>");
                var codeid=select.options[select.selectedIndex].value;
                var selectname=document.getElementById("<%=ddlToMarket.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;
                
                break;
            case "fromcountrycode":
                var select=document.getElementById("<%=ddlFromCountry.ClientID%>");
                var codeid=select.options[select.selectedIndex].text;
                var selectname=document.getElementById("<%=ddlFromCountryName.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;
                 
                var select1=document.getElementById("<%=ddlToCountry.ClientID%>");
                select1.value=select.options[select.selectedIndex].value;
                var selectname1=document.getElementById("<%=ddlToCountryName.ClientID%>");
                selectname1.value=select.options[select.selectedIndex].text;
                
                break;
            case "fromcountryname":
                var select=document.getElementById("<%=ddlFromCountryName.ClientID%>");
                var codeid=select.options[select.selectedIndex].value;
                var selectname=document.getElementById("<%=ddlFromCountry.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;
                 
                var select1=document.getElementById("<%=ddlToCountry.ClientID%>");
                select1.value=select.options[select.selectedIndex].text;
                var selectname1=document.getElementById("<%=ddlToCountryName.ClientID%>");
                selectname1.value=select.options[select.selectedIndex].value;
                
                break;
            case "tocountrycode":
                var select=document.getElementById("<%=ddlToCountry.ClientID%>");
                var codeid=select.options[select.selectedIndex].text;
                var selectname=document.getElementById("<%=ddlToCountryName.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;
                
                break;
            case "tocountryname":
                var select=document.getElementById("<%=ddlToCountryName.ClientID%>");
                var codeid=select.options[select.selectedIndex].value;
                var selectname=document.getElementById("<%=ddlToCountry.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;
                
                break;
            
        }
    }

    
function rbevent(rb1,rb2,Opt,Group)
{
       var rb2 = document.getElementById(rb2);
       rb1.checked = true;
       rb2.checked = false;
       switch (Group)
       {
           case "Account":
                var ddlm1 = document.getElementById("<%=ddlFromAccount.ClientID%>");
                var ddlm2 = document.getElementById("<%=ddlFromAccountName.ClientID%>");
                var ddlm3 = document.getElementById("<%=ddlToAccount.ClientID%>");
                var ddlm4 = document.getElementById("<%=ddlToAccountName.ClientID%>");
                break; 
           case "Control":
                var ddlm1 = document.getElementById("<%=ddlFromControl.ClientID%>");
                var ddlm2 = document.getElementById("<%=ddlFromControlName.ClientID%>");
                var ddlm3 = document.getElementById("<%=ddlToControl.ClientID%>");
                var ddlm4 = document.getElementById("<%=ddlToControlName.ClientID%>");
                break; 
           case "Category":
                var ddlm1 = document.getElementById("<%=ddlFromCategory.ClientID%>");
                var ddlm2 = document.getElementById("<%=ddlFromCategoryName.ClientID%>");
                var ddlm3 = document.getElementById("<%=ddlToCategory.ClientID%>");
                var ddlm4 = document.getElementById("<%=ddlToCategoryName.ClientID%>");
                break; 
           case "Market":
                var ddlm1 = document.getElementById("<%=ddlFromMarket.ClientID%>");
                var ddlm2 = document.getElementById("<%=ddlFromMarketName.ClientID%>");
                var ddlm3 = document.getElementById("<%=ddlToMarket.ClientID%>");
                var ddlm4 = document.getElementById("<%=ddlToMarketName.ClientID%>");
                break; 
           case "Country":
                var ddlm1 = document.getElementById("<%=ddlFromCountry.ClientID%>");
                var ddlm2 = document.getElementById("<%=ddlFromCountryName.ClientID%>");
                var ddlm3 = document.getElementById("<%=ddlToCountry.ClientID%>");
                var ddlm4 = document.getElementById("<%=ddlToCountryName.ClientID%>");
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

</script>

    <asp:UpdatePanel id="UpdatePanel1" runat="server">
        <contenttemplate>
<TABLE style="BORDER-RIGHT: gray 2px solid; BORDER-TOP: gray 2px solid; BORDER-LEFT: gray 2px solid; WIDTH: 1px; BORDER-BOTTOM: gray 2px solid" id="TABLE1"><TBODY>
<TR><TD class="field_heading" align=center colSpan=1><asp:Label id="lblHeading" runat="server" Text="Customer Ageing Summary Period" ForeColor="White" CssClass="field_heading" Width="693px"></asp:Label></TD></TR>
<TR><TD class="td_cell"><TABLE style="WIDTH: 694px"><TBODY><TR><TD vAlign=top><TABLE style="WIDTH: 500px; HEIGHT: 60px"><TBODY><TR><TD class="td_cell">&nbsp;As On Date</TD><TD class="td_cell">&nbsp;
<asp:TextBox id="txtFromDate" tabIndex=1 runat="server" CssClass="fiel_input" Width="80px" __designer:wfdid="w9"></asp:TextBox> <asp:ImageButton id="ImgBtnFrmDt" runat="server" __designer:wfdid="w10" ImageUrl="~/Images/Calendar_scheduleHS.png"></asp:ImageButton> 
<cc1:MaskedEditValidator id="MskVFromDt" runat="server" CssClass="field_error" __designer:wfdid="w11" TooltipMessage="Input a date in dd/mm/yyyy format" InvalidValueMessage="Invalid Date" InvalidValueBlurredMessage="*" ErrorMessage="MskVFromDate" EmptyValueMessage="Date is required" EmptyValueBlurredText="*" Display="Dynamic" ControlToValidate="txtFromDate" ControlExtender="MskFromDate"></cc1:MaskedEditValidator></TD>
<TD class="td_cell">&nbsp;Report Group</TD><TD class="td_cell"><SELECT style="WIDTH: 109px" id="ddlReportGroup" class="field_input" tabIndex=2 runat="server"> <OPTION value="[Select]" selected>[Select]</OPTION><OPTION value="None">None</OPTION><OPTION value="Control A/C Code">Control A/C Code</OPTION><OPTION value="Category">Category</OPTION><OPTION value="Country">Country</OPTION><OPTION value="Market">Market</OPTION></SELECT></TD></TR><TR><TD class="td_cell">Currency Type</TD><TD class="td_cell"><SELECT style="WIDTH: 109px" id="ddlCurrency" class="field_input" tabIndex=3 runat="server"> <OPTION value="[Select]" selected>[Select]</OPTION></SELECT></TD><TD class="td_cell">Ageing Type</TD><TD class="td_cell"><SELECT style="WIDTH: 109px" id="ddlAgeingType" class="field_input" tabIndex=4 runat="server"> <OPTION value="Date" selected>Date</OPTION><OPTION value="Month">Month</OPTION><OPTION value="Due Date">Due Date</OPTION><OPTION value="[Select]">[Select]</OPTION></SELECT></TD></TR><TR><TD class="td_cell">From Period</TD><TD class="td_cell"><SELECT style="WIDTH: 109px" id="Period1" class="field_input" tabIndex=5 runat="server"> <OPTION value="[Select]" selected>[Select]</OPTION><OPTION value="1">0</OPTION><OPTION value="2">30</OPTION><OPTION value="3">60</OPTION><OPTION value="4">90</OPTION><OPTION value="5">120</OPTION><OPTION value="6">&gt;150</OPTION><OPTION></OPTION></SELECT></TD><TD class="td_cell">To Period</TD><TD class="td_cell"><SELECT style="WIDTH: 109px" id="period2" class="field_input" tabIndex=5 runat="server"> <OPTION value="[Select]" selected>[Select]</OPTION><OPTION value="1">30</OPTION><OPTION value="2">60</OPTION><OPTION value="3">90</OPTION><OPTION value="4">120</OPTION><OPTION value="5">&gt;150</OPTION></SELECT></TD></TR><TR><TD class="td_cell">Report Order</TD><TD class="td_cell"><SELECT style="WIDTH: 109px" id="ddlReportOrder" class="field_input" tabIndex=5 runat="server">
 <OPTION value="[Select]" selected>[Select]</OPTION><OPTION value="Code">Code</OPTION><OPTION value="Name">Name</OPTION></SELECT></TD><TD class="td_cell"></TD><TD class="td_cell"><SELECT style="WIDTH: 109px" id="ddlIncludeZero" class="field_input" tabIndex=6 runat="server" Visible="false"> <OPTION value="Yes" selected>Yes</OPTION><OPTION value="No">No</OPTION><OPTION value="[Select]">[Select]</OPTION></SELECT></TD></TR><TR><TD class="td_cell">Report Type</TD><TD class="td_cell"><SELECT style="WIDTH: 199px" id="ddlType" class="field_input" onchange="CallWebMethod('SupplierAgentCode');" runat="server"> <OPTION value="Summary" selected>Summary</OPTION><OPTION value="Detailed">Detailed</OPTION></SELECT></TD><TD class="td_cell"></TD><TD class="td_cell"></TD></TR></TBODY></TABLE></TD><TD vAlign=top><asp:Panel id="Panel2" runat="server" CssClass="td_cell" Width="455px" __designer:wfdid="w21" GroupingText="Category"><TABLE style="WIDTH: 457px; HEIGHT: 1px"><TBODY><TR><TD class="td_cell"><INPUT id="rbCatall" tabIndex=7 type=radio CHECKED name="Category" runat="server" /> All</TD><TD class="td_cell">From</TD><TD class="td_cell"><SELECT style="WIDTH: 100px" id="ddlFromCategory" class="field_input" disabled tabIndex=9 onchange="CallWebMethod('fromcategorycode');" runat="server"> <OPTION value="[Select]" selected>[Select]</OPTION></SELECT> </TD><TD class="td_cell">From</TD><TD class="td_cell"><SELECT style="WIDTH: 225px" id="ddlFromCategoryName" class="field_input" disabled tabIndex=10 onchange="CallWebMethod('fromcategoryname');" runat="server"> <OPTION value="[Select]" selected>[Select]</OPTION></SELECT> </TD></TR><TR><TD class="td_cell"><INPUT id="rbCatrange" tabIndex=8 type=radio name="Category" runat="server" /> Range</TD><TD class="td_cell">To</TD><TD class="td_cell"><SELECT style="WIDTH: 100px" id="ddlToCategory" class="field_input" disabled tabIndex=11 onchange="CallWebMethod('tocategorycode');" runat="server"> <OPTION value="[Select]" selected>[Select]</OPTION></SELECT> </TD><TD class="td_cell">To</TD><TD class="td_cell"><SELECT style="WIDTH: 225px" id="ddlToCategoryName" class="field_input" disabled tabIndex=12 onchange="CallWebMethod('tocategoryname');" runat="server"> <OPTION value="[Select]" selected>[Select]</OPTION></SELECT> </TD></TR></TBODY></TABLE></asp:Panel></TD></TR><TR><TD>
    <asp:Panel id="Panel3" runat="server" Height="69px" CssClass="td_cell" 
        Width="469px" __designer:wfdid="w12" GroupingText="Customer  Code"><TABLE style="WIDTH: 456px; HEIGHT: 28px"><TBODY><TR><TD class="td_cell"><INPUT id="rbACall" tabIndex=13 type=radio CHECKED name="Account" runat="server" /> All</TD><TD  class="td_cell">From</TD><TD  class="td_cell"><SELECT style="WIDTH: 100px" id="ddlFromAccount" class="field_input" disabled tabIndex=15 onchange="CallWebMethod('fromaccountcode');" runat="server"> <OPTION value="[Select]" selected>[Select]</OPTION></SELECT> </TD><TD  class="td_cell">From</TD><TD  class="td_cell"><SELECT style="WIDTH: 225px" id="ddlFromAccountName" class="field_input" disabled tabIndex=16 onchange="CallWebMethod('fromaccountname');" runat="server"> <OPTION value="[Select]" selected>[Select]</OPTION></SELECT> </TD></TR><TR><TD class="td_cell"><INPUT id="rbACrange" tabIndex=14 type=radio name="Account" runat="server" /> Range</TD><TD class="td_cell">To</TD><TD class="td_cell"><SELECT style="WIDTH: 100px" id="ddlToAccount" class="field_input" disabled tabIndex=17 onchange="CallWebMethod('toaccountcode');" runat="server"> <OPTION value="[Select]" selected>[Select]</OPTION></SELECT> </TD><TD class="td_cell">To</TD><TD class="td_cell"><SELECT style="WIDTH: 225px" id="ddlToAccountName" class="field_input" disabled tabIndex=18 onchange="CallWebMethod('toaccountname');" runat="server"> <OPTION value="[Select]" selected>[Select]</OPTION></SELECT> </TD></TR></TBODY></TABLE></asp:Panel></TD><TD><asp:Panel id="Panel4" runat="server" CssClass="td_cell" Width="455px" GroupingText="Control A/C"><TABLE style="WIDTH: 456px; HEIGHT: 19px"><TBODY><TR><TD class="td_cell"><INPUT id="rbControlall" tabIndex=19 type=radio CHECKED name="Control" runat="server" /> All</TD><TD class="td_cell">From</TD><TD class="td_cell"><SELECT style="WIDTH: 100px" id="ddlFromControl" class="field_input" disabled tabIndex=21 onchange="CallWebMethod('fromcontrolcode');" runat="server"> <OPTION value="[Select]" selected>[Select]</OPTION></SELECT> </TD><TD class="td_cell">From</TD><TD class="td_cell"><SELECT style="WIDTH: 225px" id="ddlFromControlName" class="field_input" disabled tabIndex=22 onchange="CallWebMethod('fromcontrolname');" runat="server"> <OPTION value="[Select]" selected>[Select]</OPTION></SELECT> </TD></TR><TR><TD class="td_cell"><INPUT id="rbControlrange" tabIndex=20 type=radio name="Control" runat="server" /> Range</TD><TD class="td_cell">To</TD><TD class="td_cell"><SELECT style="WIDTH: 100px" id="ddlToControl" class="field_input" disabled tabIndex=23 onchange="CallWebMethod('tocontrolcode');" runat="server"> <OPTION value="[Select]" selected>[Select]</OPTION></SELECT> </TD><TD class="td_cell">To</TD><TD class="td_cell"><SELECT style="WIDTH: 225px" id="ddlToControlName" class="field_input" disabled tabIndex=24 onchange="CallWebMethod('tocontrolname');" runat="server"> <OPTION value="[Select]" selected>[Select]</OPTION></SELECT> </TD></TR></TBODY></TABLE></asp:Panel> </TD></TR><TR><TD><asp:Panel id="Panel6" runat="server" CssClass="td_cell" Width="455px" __designer:wfdid="w14" GroupingText="Market"><TABLE style="WIDTH: 455px; HEIGHT: 37px"><TBODY><TR><TD  class="td_cell"><INPUT id="rbMarkall" tabIndex=25 type=radio CHECKED name="Market" runat="server" /> All</TD><TD class="td_cell">From</TD><TD  class="td_cell"><SELECT style="WIDTH: 100px" id="ddlFromMarket" class="field_input" disabled tabIndex=27 onchange="CallWebMethod('frommarketcode');" runat="server"> <OPTION value="[Select]" selected>[Select]</OPTION></SELECT> </TD><TD  class="td_cell">From</TD><TD  class="td_cell"><SELECT style="WIDTH: 225px" id="ddlFromMarketName" class="field_input" disabled tabIndex=28 onchange="CallWebMethod('frommarketname');" runat="server"> <OPTION value="[Select]" selected>[Select]</OPTION></SELECT> </TD></TR><TR><TD class="td_cell"><INPUT id="rbMarkrange" tabIndex=26 type=radio name="Market" runat="server" /> Range</TD><TD class="td_cell">To</TD><TD class="td_cell"><SELECT style="WIDTH: 100px" id="ddlToMarket" class="field_input" disabled tabIndex=29 onchange="CallWebMethod('tomarketcode');" runat="server"> <OPTION value="[Select]" selected>[Select]</OPTION></SELECT> </TD><TD class="td_cell">To</TD><TD class="td_cell"><SELECT style="WIDTH: 225px" id="ddlToMarketName" class="field_input" disabled tabIndex=30 onchange="CallWebMethod('tomarketname');" runat="server"> <OPTION value="[Select]" selected>[Select]</OPTION></SELECT> </TD></TR></TBODY></TABLE></asp:Panel></TD><TD><asp:Panel id="Panel7" runat="server" CssClass="td_cell" Width="455px" __designer:wfdid="w22" GroupingText="Country"><TABLE style="WIDTH: 455px; HEIGHT: 28px"><TBODY><TR><TD class="td_cell"><INPUT id="rbCtrall" tabIndex=31 type=radio CHECKED name="Country" runat="server" /> All</TD><TD class="td_cell">From</TD><TD  class="td_cell"><SELECT style="WIDTH: 100px" id="ddlFromCountry" class="field_input" disabled tabIndex=33 onchange="CallWebMethod('fromcountrycode');" runat="server"> <OPTION value="[Select]" selected>[Select]</OPTION></SELECT> </TD><TD class="td_cell">From</TD><TD class="td_cell"><SELECT style="WIDTH: 225px" id="ddlFromCountryName" class="field_input" disabled tabIndex=34 onchange="CallWebMethod('fromcountryname');" runat="server"> <OPTION value="[Select]" selected>[Select]</OPTION></SELECT> </TD></TR><TR><TD class="td_cell"><INPUT id="rbCtrrange" tabIndex=32 type=radio name="Country" runat="server" /> Range</TD><TD class="td_cell">To</TD><TD  class="td_cell"><SELECT style="WIDTH: 100px" id="ddlToCountry" class="field_input" disabled tabIndex=35 onchange="CallWebMethod('tocountrycode');" runat="server"> <OPTION value="[Select]" selected>[Select]</OPTION></SELECT> </TD><TD class="td_cell">To</TD><TD class="td_cell"><SELECT style="WIDTH: 225px" id="ddlToCountryName" class="field_input" disabled tabIndex=36 onchange="CallWebMethod('tocountryname');" runat="server"> <OPTION value="[Select]" selected>[Select]</OPTION></SELECT> </TD></TR></TBODY></TABLE></asp:Panel></TD></TR><TR><TD style="HEIGHT: 15px"></TD><TD style="HEIGHT: 15px">&nbsp;</TD></TR></TBODY></TABLE></TD></TR><TR><TD class="td_cell" align=center>
 <asp:Button id="Button1" onclick="Button1_Click1" runat="server" Text="Export" __designer:dtid="4222124650660080" CssClass="field_button" __designer:wfdid="w5"></asp:Button>&nbsp;&nbsp;
 <asp:Button id="btnReport" tabIndex=37 onclick="btnReport_Click" runat="server" 
        Text="Load Report" CssClass="field_button" CausesValidation="False"></asp:Button>&nbsp;
 <asp:Button id="btnhelp" tabIndex=38 onclick="btnhelp_Click" runat="server" 
        Text="Help" CssClass="field_button" __designer:wfdid="w19"></asp:Button>&nbsp;
 <asp:Button id="btnExit" tabIndex=39 onclick="btnExit_Click" runat="server" 
        Text=" Exit" CssClass="field_button" __designer:wfdid="w20" 
        CausesValidation="False"></asp:Button></TD></TR></TBODY></TABLE>
 <asp:ScriptManagerProxy id="ScriptManagerProxy1" runat="server"><Services>
<asp:ServiceReference Path="clsServices.asmx"></asp:ServiceReference>
</Services>
</asp:ScriptManagerProxy> <cc1:CalendarExtender id="ClsExFromDate" runat="server" TargetControlID="txtFromDate" PopupButtonID="ImgBtnFrmDt" Format="dd/MM/yyyy">
            </cc1:CalendarExtender> <cc1:MaskedEditExtender id="MskFromDate" runat="server" TargetControlID="txtFromDate" MessageValidatorTip="true" MaskType="Date" Mask="99/99/9999" ErrorTooltipEnabled="True" DisplayMoney="Left" AcceptNegative="Left">
            </cc1:MaskedEditExtender> &nbsp; 
</contenttemplate>

<Triggers>
<asp:PostBackTrigger ControlID="Button1"></asp:PostBackTrigger>
</Triggers>

    </asp:UpdatePanel>
</asp:Content>

