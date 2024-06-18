<%@ Page Language="VB" AutoEventWireup="false" CodeFile="RptBalanceExceedingCreditLimit.aspx.vb" Inherits="RptBalanceExceedingCreditLimit" MasterPageFile="~/AccountsMaster.master" Strict ="true"  %>

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

    <asp:UpdatePanel id="UpdatePanel1" runat="server">
        <contenttemplate>
<TABLE style="BORDER-RIGHT: gray 2px solid; BORDER-TOP: gray 2px solid; BORDER-LEFT: gray 2px solid; WIDTH: 1px; BORDER-BOTTOM: gray 2px solid"><TBODY><TR><TD class="field_heading" align=center colSpan=1><asp:Label id="lblHeading" runat="server" Text="Balance Exceeding Credit limit" ForeColor="White" Width="693px" CssClass="field_heading"></asp:Label></TD></TR><TR><TD style="WIDTH: 947px; HEIGHT: 24px" class="td_cell"><TABLE style="WIDTH: 694px"><TBODY><TR><TD><asp:Panel id="Panel1" runat="server" Width="475px" CssClass="field_input" GroupingText="Select Date " __designer:wfdid="w5"><TABLE style="WIDTH: 447px">
                            <caption>
                                &#160;&#160;&#160;&#160;&#160;&#160;&#160;
                                <tr>
                                    <td class="td_cell" style="WIDTH: 7px">
                                        <asp:Label ID="Label1" runat="server" __designer:wfdid="w11" Text="To Date" 
                                            Width="45px"></asp:Label>
                                    </td>
                                    <td class="td_cell">
                                        <asp:TextBox ID="txttoDate" runat="server" __designer:wfdid="w12" 
                                            CssClass="txtbox" tabIndex="1" Width="111px"></asp:TextBox>
                                        <asp:ImageButton ID="ImgBtntoDt" runat="server" __designer:wfdid="w13" 
                                            ImageUrl="~/Images/Calendar_scheduleHS.png" />
                                        <cc1:MaskedEditValidator ID="MskVtoDt" runat="server" __designer:wfdid="w14" 
                                            ControlExtender="MsktoDate" ControlToValidate="txttoDate" 
                                            CssClass="field_error" Display="Dynamic" EmptyValueBlurredText="*" 
                                            EmptyValueMessage="Date is required" ErrorMessage="MskVFromDate" 
                                            InvalidValueBlurredMessage="*" InvalidValueMessage="Invalid Date" 
                                            TooltipMessage="Input a date in dd/mm/yyyy format" Width="1px"></cc1:MaskedEditValidator>
                                    </td>
                                </tr>
                            </caption>
                            
                            
                        </TABLE></asp:Panel></TD></TR><TR><TD><asp:Panel id="Panel4" 
            runat="server" Width="675px" CssClass="td_cell" GroupingText="Customer" 
            __designer:wfdid="w1"><TABLE style="WIDTH: 676px">
                                <TBODY>
        <TR><TD class="td_cell">
            <input id="rbncustall" runat="server" checked name="customer" tabindex="2" 
            type="radio" /> All</TD><TD class="td_cell">From Code</TD><TD class="td_cell"><select 
            id="ddlcustfrm" runat="server" class="drpdown" disabled 
            onchange="CallWebMethod('frmcust');" style="WIDTH: 200px" tabindex="4"> 
            <option selected="" value="[Select]">[Select]</option>
        </select>
         </TD>
            <TD class="td_cell">From Name</TD><TD class="td_cell">
            <SELECT style="WIDTH: 225px" id="ddlcustfrmname" class="drpdown" disabled 
            tabIndex=5 onchange="CallWebMethod('frmcustname');" runat="server"> 
                <OPTION value="[Select]" selected>[Select]</OPTION></SELECT> </TD></TR>
        <TR><TD class="td_cell">
            <input id="rbncustrange" runat="server" name="customer" tabindex="3" 
            type="radio" /> Range</TD><TD class="td_cell">To Code</TD><TD class="td_cell">
                <select id="ddlcustto" runat="server" class="drpdown" disabled 
            onchange="CallWebMethod('tocust');" style="WIDTH: 200px" tabindex="6"> 
                    <option selected="" value="[Select]">[Select]</option>
        </select>
         </TD>
            <TD class="td_cell">To Name</TD><TD class="td_cell">
            <SELECT style="WIDTH: 225px" id="ddlcusttoname" class="drpdown" disabled 
            tabIndex=7 onchange="CallWebMethod('tocustname');" runat="server"> <OPTION value="[Select]" selected>[Select]</OPTION></SELECT> </TD></TR></TBODY></TABLE></asp:Panel></TD></TR><TR><TD>
    <asp:Panel id="Panel2" runat="server" Width="675px" CssClass="td_cell" 
        GroupingText="Category"><TABLE style="WIDTH: 676px">
                            <TBODY>
        <TR><TD class="td_cell">
            <input id="rbCatall" runat="server" checked name="Category" tabindex="8" 
            type="radio" /> All</TD><TD class="td_cell">From Code</TD><TD class="td_cell"><select 
            id="ddlFromCategory" runat="server" class="drpdown" disabled 
            onchange="CallWebMethod('fromcategorycode');" style="WIDTH: 200px" 
            tabindex="10"> 
            <option selected="" value="[Select]">[Select]</option>
        </select>
         </TD>
            <TD class="td_cell">From Name</TD><TD class="td_cell">
            <SELECT style="WIDTH: 225px" id="ddlFromCategoryName" class="drpdown" 
            disabled tabIndex=11 onchange="CallWebMethod('fromcategoryname');" 
            runat="server"> <OPTION value="[Select]" selected>[Select]</OPTION></SELECT> </TD></TR>
        <TR><TD class="td_cell">
            <input id="rbCatrange" runat="server" name="Category" tabindex="9" 
            type="radio" /> Range</TD><TD class="td_cell">To Code</TD><TD class="td_cell">
                <select id="ddlToCategory" runat="server" class="drpdown" disabled 
            onchange="CallWebMethod('tocategorycode');" style="WIDTH: 200px" tabindex="12"> 
                    <option selected="" value="[Select]">[Select]</option>
        </select>
         </TD>
            <TD class="td_cell">To Name</TD><TD class="td_cell">
            <SELECT style="WIDTH: 225px" id="ddlToCategoryName" class="drpdown" 
            disabled tabIndex=13 onchange="CallWebMethod('tocategoryname');" runat="server"> <OPTION value="[Select]" selected>[Select]</OPTION></SELECT> </TD></TR></TBODY></TABLE></asp:Panel> </TD></TR><TR><TD>
    <asp:Panel id="Panel6" runat="server" Width="675px" CssClass="td_cell" 
        GroupingText="Market"><TABLE style="WIDTH: 676px">
                            <TBODY>
        <TR><TD class="td_cell">
            <input id="rbMarkall" runat="server" checked name="Market" tabindex="14" 
            type="radio" /> All</TD><TD class="td_cell">From Code</TD><TD class="td_cell">
                <select id="ddlFromMarket" runat="server" class="drpdown" disabled 
            onchange="CallWebMethod('frommarketcode');" style="WIDTH: 200px" tabindex="16"> 
                    <option selected="" value="[Select]">[Select]</option>
        </select>
         </TD>
            <TD class="td_cell">From Name</TD><TD class="td_cell"><SELECT 
            style="WIDTH: 225px" id="ddlFromMarketName" class="drpdown" disabled 
            tabIndex=17 onchange="CallWebMethod('frommarketname');" runat="server"> <OPTION value="[Select]" selected>[Select]</OPTION></SELECT> </TD></TR>
        <TR><TD class="td_cell">
            <input id="rbMarkrange" runat="server" name="Market" tabindex="15" 
            type="radio" /> Range</TD><TD class="td_cell">To Code</TD><TD class="td_cell">
                <select id="ddlToMarket" runat="server" class="drpdown" disabled 
            onchange="CallWebMethod('tomarketcode');" style="WIDTH: 200px" tabindex="18"> 
                    <option selected="" value="[Select]">[Select]</option>
        </select>
         </TD>
            <TD class="td_cell">To Name</TD><TD class="td_cell">
            <SELECT style="WIDTH: 225px" id="ddlToMarketName" class="drpdown" disabled 
            tabIndex=19 onchange="CallWebMethod('tomarketname');" runat="server"> <OPTION value="[Select]" selected>[Select]</OPTION></SELECT> </TD></TR></TBODY></TABLE></asp:Panel> </TD></TR><TR><TD style="HEIGHT: 98px"><asp:Panel id="Panel7" runat="server" Width="679px" CssClass="td_cell" GroupingText="Country"><TABLE style="WIDTH: 676px"><TBODY>
        <TR><TD class="td_cell">
            <input id="rbCtrall" runat="server" checked name="Country" tabindex="20" 
            type="radio" /> All</TD><TD class="td_cell">From Code</TD><TD class="td_cell"><select 
            id="ddlFromCountry" runat="server" class="drpdown" disabled 
            onchange="CallWebMethod('fromcountrycode');" style="WIDTH: 200px" tabindex="22"> 
            <option selected="" value="[Select]">[Select]</option>
        </select>
         </TD>
            <TD class="td_cell">From Name</TD><TD class="td_cell"><SELECT 
            style="WIDTH: 225px" id="ddlFromCountryName" class="drpdown" disabled 
            tabIndex=23 onchange="CallWebMethod('fromcountryname');" runat="server"> <OPTION value="[Select]" selected>[Select]</OPTION></SELECT> </TD></TR>
        <TR><TD class="td_cell">
            <input id="rbCtrrange" runat="server" name="Country" tabindex="21" 
            type="radio" /> Range</TD><TD class="td_cell">To Code</TD><TD class="td_cell">
                <select id="ddlToCountry" runat="server" class="drpdown" disabled 
            onchange="CallWebMethod('tocountrycode');" style="WIDTH: 200px" tabindex="24"> 
                    <option selected="" value="[Select]">[Select]</option>
        </select>
         </TD>
            <TD class="td_cell">To Name</TD><TD class="td_cell">
            <SELECT style="WIDTH: 225px" id="ddlToCountryName" class="drpdown" 
            disabled tabIndex=25 onchange="CallWebMethod('tocountryname');" runat="server"> <OPTION value="[Select]" selected>[Select]</OPTION></SELECT> </TD></TR></TBODY></TABLE></asp:Panel> </TD></TR></TBODY></TABLE></TD></TR>
            <TR><TD class="td_cell"><TABLE style="WIDTH: 351px"><TBODY><TR><TD style="WIDTH: 100px">Report Order</TD><TD>
        <asp:DropDownList id="ddlrptord" tabIndex=26 runat="server" Width="123px" 
            CssClass="drpdown" __designer:wfdid="w2"><asp:ListItem Value="0">Code</asp:ListItem>
<asp:ListItem Value="1">Name</asp:ListItem>
</asp:DropDownList></TD><TD style="WIDTH: 6px"></TD></TR><TR><TD style="WIDTH: 71px">Group By</TD><TD>
        <asp:DropDownList id="ddlgpby" tabIndex=27 runat="server" Width="122px" 
            CssClass="drpdown" __designer:wfdid="w3"><asp:ListItem Value="0">None</asp:ListItem>
<asp:ListItem Value="1">Market</asp:ListItem>
<asp:ListItem Value="2">Category</asp:ListItem>
</asp:DropDownList></TD><TD style="WIDTH: 6px"></TD></TR></TBODY></TABLE></TD></TR><TR><TD class="td_cell" align=right>
    <asp:Button id="btnReport" tabIndex=28 onclick="btnReport_Click" runat="server" 
        Text="Load Report" CssClass="btn" CausesValidation="False"></asp:Button> &nbsp;
        <asp:Button id="btnExit" tabIndex=29 onclick="btnExit_Click" runat="server" 
        Text=" Exit" CssClass="btn" __designer:wfdid="w19" CausesValidation="False"></asp:Button>&nbsp;<asp:Button 
        id="btnhelp" tabIndex=30 onclick="btnhelp_Click" runat="server" Text="Help" 
        CssClass="btn" __designer:wfdid="w7"></asp:Button></TD></TR></TBODY></TABLE><asp:ScriptManagerProxy id="ScriptManagerProxy1" runat="server"><Services>
<asp:ServiceReference Path="clsServices.asmx"></asp:ServiceReference>
</Services>
</asp:ScriptManagerProxy><cc1:MaskedEditExtender id="MsktoDate" runat="server" __designer:wfdid="w17" AcceptNegative="Left" DisplayMoney="Left" ErrorTooltipEnabled="True" Mask="99/99/9999" MaskType="Date" MessageValidatorTip="true" TargetControlID="txttoDate"></cc1:MaskedEditExtender><cc1:CalendarExtender id="ClsExtoDate" runat="server" __designer:wfdid="w18" TargetControlID="txttoDate" Format="dd/MM/yyyy" PopupButtonID="ImgBtntoDt"></cc1:CalendarExtender> 
</contenttemplate>
    </asp:UpdatePanel>
</asp:Content>
