<%@ Page Language="VB" AutoEventWireup="false" CodeFile="RptCustasondateOSB.aspx.vb" Inherits="RptCustasondateOSB" MasterPageFile="~/AccountsMaster.master" Strict ="true"  %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="server">

<script language="javascript" type="text/javascript">
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
                
                case "frmcust":
                var select=document.getElementById("<%=ddlcustfrm.ClientID%>");
                var selectname=document.getElementById("<%=ddlcustfrmname.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;                
                var select1=document.getElementById("<%=ddlcustto.ClientID%>");
                var selectname1=document.getElementById("<%=ddlcusttoname.ClientID%>");
                select1.value=selectname.options[selectname.selectedIndex].text;
                selectname1.value=select1.options[select1.selectedIndex].text;         
                 break;
                case "frmcustname":
                 var select=document.getElementById("<%=ddlcustfrm.ClientID%>");
                var selectname=document.getElementById("<%=ddlcustfrmname.ClientID%>");
                select.value=selectname.options[selectname.selectedIndex].text;                
                var select1=document.getElementById("<%=ddlcustto.ClientID%>");
                var selectname1=document.getElementById("<%=ddlcusttoname.ClientID%>");
                select1.value=selectname.options[selectname.selectedIndex].text;
                selectname1.value=select1.options[select1.selectedIndex].text;         
                
                break;
                case "tocust":
                 var select1=document.getElementById("<%=ddlcustto.ClientID%>");
                var selectname1=document.getElementById("<%=ddlcusttoname.ClientID%>");
                selectname1.value=select1.options[select1.selectedIndex].text;        
                break;
                case "tocustname":
                 var select1=document.getElementById("<%=ddlcustto.ClientID%>");
                var selectname1=document.getElementById("<%=ddlcusttoname.ClientID%>");
                select1.value=selectname1.options[selectname1.selectedIndex].text;           
                
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
                
            case "customer":
                var ddlm1 = document.getElementById("<%=ddlcustfrm.ClientID%>");
                var ddlm2 = document.getElementById("<%=ddlcustfrmname.ClientID%>");
                var ddlm3 = document.getElementById("<%=ddlcustto.ClientID%>");
                var ddlm4 = document.getElementById("<%=ddlcusttoname.ClientID%>");
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

<TABLE style="BORDER-RIGHT: gray 2px solid; BORDER-TOP: gray 2px solid; BORDER-LEFT: gray 2px solid; WIDTH: 1px; BORDER-BOTTOM: gray 2px solid"><TBODY><TR><TD class="field_heading" align=center colSpan=1><asp:Label id="lblHeading" runat="server" Text="Customer Outstanding and Collection" ForeColor="White" Width="693px" CssClass="field_heading"></asp:Label></TD></TR><TR><TD style="WIDTH: 947px; HEIGHT: 24px" class="td_cell"><TABLE style="WIDTH: 694px"><TBODY><TR><TD>&nbsp;<asp:Panel id="Panel1" runat="server" Width="475px" CssClass="field_input" GroupingText="Select Date "><TABLE style="WIDTH: 447px"><TBODY>
    <TR><TD style="WIDTH: 7px" class="field_input"><asp:Label id="Label1" runat="server" Text="As On Date :" Width="104px"></asp:Label></TD><TD class="field_input"><asp:TextBox id="txttoDate" runat="server" Width="111px" CssClass="field_input"></asp:TextBox> <asp:ImageButton id="ImgBtntoDt" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.png"></asp:ImageButton> <cc1:MaskedEditValidator id="MskVtoDt" runat="server" Width="1px" CssClass="field_error" ControlExtender="MsktoDate" ControlToValidate="txttoDate" Display="Dynamic" EmptyValueBlurredText="*" EmptyValueMessage="Date is required" ErrorMessage="MskVFromDate" InvalidValueBlurredMessage="*" InvalidValueMessage="Invalid Date" TooltipMessage="Input a date in dd/mm/yyyy format"></cc1:MaskedEditValidator></TD></TR></TBODY></TABLE></asp:Panel></TD></TR><TR><TD>
    <asp:Panel id="Panel4" runat="server" Width="675px" CssClass="td_cell" 
        GroupingText="Customer"><TABLE style="WIDTH: 676px"><TBODY><TR><TD class="td_cell"><INPUT id="rbncustall" type=radio CHECKED name="customer" runat="server" /> All</TD><TD class="td_cell">From Code</TD><TD class="td_cell"><SELECT style="WIDTH: 200px" id="ddlcustfrm" class="field_input" disabled=true  onchange="CallWebMethod('frmcust');" runat="server"> <OPTION value="[Select]" selected>[Select]</OPTION></SELECT> </TD><TD class="td_cell">From Name</TD><TD class="td_cell"><SELECT style="WIDTH: 225px" id="ddlcustfrmname" class="field_input" disabled=true  onchange="CallWebMethod('frmcustname');" runat="server"> <OPTION value="[Select]" selected>[Select]</OPTION></SELECT> </TD></TR><TR><TD class="td_cell"><INPUT id="rbncustrange" type=radio name="customer" runat="server" /> Range</TD><TD class="td_cell">To Code</TD><TD class="td_cell"><SELECT style="WIDTH: 200px" id="ddlcustto" class="field_input" disabled=true  onchange="CallWebMethod('tocust');" runat="server"> <OPTION value="[Select]" selected>[Select]</OPTION></SELECT> </TD><TD class="td_cell">To Name</TD><TD class="td_cell"><SELECT style="WIDTH: 225px" id="ddlcusttoname" class="field_input" disabled=true  onchange="CallWebMethod('tocustname');" runat="server"> <OPTION value="[Select]" selected>[Select]</OPTION></SELECT> </TD></TR></TBODY></TABLE></asp:Panel></TD></TR><TR><TD>
    <asp:Panel id="Panel3" runat="server" Width="675px" CssClass="td_cell" 
        GroupingText="Control Account Code"><TABLE style="WIDTH: 676px"><TBODY><TR><TD style="HEIGHT: 26px" class="td_cell"><INPUT id="rbACall" type=radio CHECKED name="Account" runat="server" /> All</TD><TD style="HEIGHT: 26px" class="td_cell">From Code</TD><TD style="HEIGHT: 26px" class="td_cell"><SELECT style="WIDTH: 200px" id="ddlFromAccount" class="field_input" disabled onchange="CallWebMethod('fromaccountcode');" runat="server"> <OPTION value="[Select]" selected>[Select]</OPTION></SELECT> </TD><TD style="HEIGHT: 26px" class="td_cell">From Name</TD><TD style="HEIGHT: 26px" class="td_cell"><SELECT style="WIDTH: 225px" id="ddlFromAccountName" class="field_input" disabled onchange="CallWebMethod('fromaccountname');" runat="server"> <OPTION value="[Select]" selected>[Select]</OPTION></SELECT> </TD></TR><TR><TD class="td_cell"><INPUT id="rbACrange" type=radio name="Account" runat="server" /> Range</TD><TD class="td_cell">To Code</TD><TD class="td_cell"><SELECT style="WIDTH: 200px" id="ddlToAccount" class="field_input" disabled onchange="CallWebMethod('toaccountcode');" runat="server"> <OPTION value="[Select]" selected>[Select]</OPTION></SELECT> </TD><TD class="td_cell">To Name</TD><TD class="td_cell"><SELECT style="WIDTH: 225px" id="ddlToAccountName" class="field_input" disabled onchange="CallWebMethod('toaccountname');" runat="server"> <OPTION value="[Select]" selected>[Select]</OPTION></SELECT> </TD></TR></TBODY></TABLE></asp:Panel> </TD></TR><TR><TD>
    <asp:Panel id="Panel2" runat="server" Width="675px" CssClass="td_cell" 
        GroupingText="Category"><TABLE style="WIDTH: 676px"><TBODY><TR><TD class="td_cell"><INPUT id="rbCatall" type=radio CHECKED name="Category" runat="server" /> All</TD><TD class="td_cell">From Code</TD><TD class="td_cell"><SELECT style="WIDTH: 200px" id="ddlFromCategory" class="field_input" disabled onchange="CallWebMethod('fromcategorycode');" runat="server"> <OPTION value="[Select]" selected>[Select]</OPTION></SELECT> </TD><TD class="td_cell">From Name</TD><TD class="td_cell"><SELECT style="WIDTH: 225px" id="ddlFromCategoryName" class="field_input" disabled onchange="CallWebMethod('fromcategoryname');" runat="server"> <OPTION value="[Select]" selected>[Select]</OPTION></SELECT> </TD></TR><TR><TD class="td_cell"><INPUT id="rbCatrange" type=radio name="Category" runat="server" /> Range</TD><TD class="td_cell">To Code</TD><TD class="td_cell"><SELECT style="WIDTH: 200px" id="ddlToCategory" class="field_input" disabled onchange="CallWebMethod('tocategorycode');" runat="server"> <OPTION value="[Select]" selected>[Select]</OPTION></SELECT> </TD><TD class="td_cell">To Name</TD><TD class="td_cell"><SELECT style="WIDTH: 225px" id="ddlToCategoryName" class="field_input" disabled onchange="CallWebMethod('tocategoryname');" runat="server"> <OPTION value="[Select]" selected>[Select]</OPTION></SELECT> </TD></TR></TBODY></TABLE></asp:Panel> </TD></TR><TR><TD>
    <asp:Panel id="Panel6" runat="server" Width="675px" CssClass="td_cell" 
        GroupingText="Market"><TABLE style="WIDTH: 676px"><TBODY><TR><TD class="td_cell"><INPUT id="rbMarkall" type=radio CHECKED name="Market" runat="server" /> All</TD><TD class="td_cell">From Code</TD><TD class="td_cell"><SELECT style="WIDTH: 200px" id="ddlFromMarket" class="field_input" disabled onchange="CallWebMethod('frommarketcode');" runat="server"> <OPTION value="[Select]" selected>[Select]</OPTION></SELECT> </TD><TD class="td_cell">From Name</TD><TD class="td_cell"><SELECT style="WIDTH: 225px" id="ddlFromMarketName" class="field_input" disabled onchange="CallWebMethod('frommarketname');" runat="server"> <OPTION value="[Select]" selected>[Select]</OPTION></SELECT> </TD></TR><TR><TD class="td_cell"><INPUT id="rbMarkrange" type=radio name="Market" runat="server" /> Range</TD><TD class="td_cell">To Code</TD><TD class="td_cell"><SELECT style="WIDTH: 200px" id="ddlToMarket" class="field_input" disabled onchange="CallWebMethod('tomarketcode');" runat="server"> <OPTION value="[Select]" selected>[Select]</OPTION></SELECT> </TD><TD class="td_cell">To Name</TD><TD class="td_cell"><SELECT style="WIDTH: 225px" id="ddlToMarketName" class="field_input" disabled onchange="CallWebMethod('tomarketname');" runat="server"> <OPTION value="[Select]" selected>[Select]</OPTION></SELECT> </TD></TR></TBODY></TABLE></asp:Panel> </TD></TR><TR><TD style="HEIGHT: 98px"><asp:Panel id="Panel7" runat="server" Width="679px" CssClass="td_cell" GroupingText="Country"><TABLE style="WIDTH: 676px"><TBODY><TR><TD class="td_cell"><INPUT id="rbCtrall" type=radio CHECKED name="Country" runat="server" /> All</TD><TD class="td_cell">From Code</TD><TD class="td_cell"><SELECT style="WIDTH: 200px" id="ddlFromCountry" class="field_input" disabled onchange="CallWebMethod('fromcountrycode');" runat="server"> <OPTION value="[Select]" selected>[Select]</OPTION></SELECT> </TD><TD class="td_cell">From Name</TD><TD class="td_cell"><SELECT style="WIDTH: 225px" id="ddlFromCountryName" class="field_input" disabled onchange="CallWebMethod('fromcountryname');" runat="server"> <OPTION value="[Select]" selected>[Select]</OPTION></SELECT> </TD></TR><TR><TD class="td_cell"><INPUT id="rbCtrrange" type=radio name="Country" runat="server" /> Range</TD><TD class="td_cell">To Code</TD><TD class="td_cell"><SELECT style="WIDTH: 200px" id="ddlToCountry" class="field_input" disabled onchange="CallWebMethod('tocountrycode');" runat="server"> <OPTION value="[Select]" selected>[Select]</OPTION></SELECT> </TD><TD class="td_cell">To Name</TD><TD class="td_cell"><SELECT style="WIDTH: 225px" id="ddlToCountryName" class="field_input" disabled onchange="CallWebMethod('tocountryname');" runat="server"> <OPTION value="[Select]" selected>[Select]</OPTION></SELECT> </TD></TR></TBODY></TABLE></asp:Panel> </TD></TR></TBODY></TABLE></TD></TR><TR><TD class="td_cell">&nbsp; <TABLE style="WIDTH: 399px"><TBODY><TR><TD style="WIDTH: 72px">Currency</TD><TD style="WIDTH: 137px"><SELECT style="WIDTH: 109px" id="ddlCurrency" class="field_input" runat="server"><OPTION value="[Select]" selected>[Select]</OPTION></SELECT></TD><TD style="WIDTH: 71px"></TD></TR><TR><TD style="WIDTH: 72px">Report Order</TD><TD style="WIDTH: 137px"><asp:DropDownList id="ddlrptord" runat="server" Width="123px" CssClass="field_input"><asp:ListItem Value="0">Code</asp:ListItem>
<asp:ListItem Value="1">Name</asp:ListItem>
</asp:DropDownList></TD><TD style="WIDTH: 71px"></TD></TR><TR><TD style="WIDTH: 72px">Group By</TD><TD style="WIDTH: 137px"><asp:DropDownList id="ddlgpby" runat="server" Width="122px" CssClass="field_input"><asp:ListItem Value="0">None</asp:ListItem>
<asp:ListItem Value="1">Market</asp:ListItem>
<asp:ListItem Value="2">Category</asp:ListItem>
<asp:ListItem Value="4">Control account Code</asp:ListItem>
</asp:DropDownList></TD><TD style="WIDTH: 71px"></TD></TR><TR><TD style="WIDTH: 72px">Include Zero</TD><TD style="WIDTH: 137px"><asp:DropDownList id="ddlinclzero" runat="server" Width="122px" CssClass="field_input"><asp:ListItem Value="0">No</asp:ListItem>
<asp:ListItem Value="1">Yes</asp:ListItem>
</asp:DropDownList></TD><TD style="WIDTH: 71px"></TD></TR></TBODY></TABLE></TD></TR><TR><TD class="td_cell" align=right>&nbsp; <asp:Button id="Button1" onclick="Button1_Click1" runat="server" Text="Export" CssClass="field_button"></asp:Button>&nbsp;<asp:Button 
        id="btnReport" onclick="btnReport_Click" runat="server" Text="Load Report" 
        CssClass="field_button" CausesValidation="False"></asp:Button>
&nbsp; <asp:Button id="btnExit" tabIndex=6 onclick="btnExit_Click" runat="server" 
        Text=" Exit" CssClass="field_button" CausesValidation="False"></asp:Button>&nbsp;<asp:Button 
        id="btnhelp" tabIndex=39 onclick="btnhelp_Click" runat="server" Text="Help" 
        CssClass="field_button"></asp:Button></TD></TR></TBODY></TABLE><asp:ScriptManagerProxy id="ScriptManagerProxy1" runat="server"><Services>
<asp:ServiceReference Path="clsServices.asmx"></asp:ServiceReference>
</Services>
</asp:ScriptManagerProxy><cc1:MaskedEditExtender id="MsktoDate" runat="server" TargetControlID="txttoDate" AcceptNegative="Left" DisplayMoney="Left" ErrorTooltipEnabled="True" Mask="99/99/9999" MaskType="Date" MessageValidatorTip="true"></cc1:MaskedEditExtender><cc1:CalendarExtender id="ClsExtoDate" runat="server" Format="dd/MM/yyyy" PopupButtonID="ImgBtntoDt" TargetControlID="txttoDate"></cc1:CalendarExtender> 

</asp:Content>
