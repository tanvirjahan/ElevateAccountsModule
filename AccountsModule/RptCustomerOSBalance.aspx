<%@ Page Language="VB" AutoEventWireup="false" CodeFile="RptCustomerOSBalance.aspx.vb" Inherits="RptCustomerOSBalance" MasterPageFile="~/AccountsMaster.master" Strict ="true"  %>

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
			
			
			
function enabledatectrl()
{
    if(document.getElementById("<%=ddlwithmovmt.ClientID%>").value==1)//without ason
    {
        document.getElementById("<%=label2.ClientID%>").innerText="AsOnDate";
        document.getElementById("<%=label1.ClientID%>").style.display = 'none';
        document.getElementById("<%=txttoDate.ClientID%>").style.display = 'none';
        document.getElementById("<%=ImgBtntoDt.ClientID%>").style.display = 'none';
        return true;
    }
    else
    {
        document.getElementById("<%=label2.ClientID%>").innerText="FromDate";
        document.getElementById("<%=label1.ClientID%>").style.display = 'block';
        document.getElementById("<%=txttoDate.ClientID%>").style.display = 'block';
        document.getElementById("<%=ImgBtntoDt.ClientID%>").style.display = 'block';
        return true;
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
function ChangeDate()
{
   
     var txtfdate=document.getElementById("<%=txtFromDate.ClientID%>");
     var txttdate=document.getElementById("<%=txtToDate.ClientID%>");
    
     if (txtfdate.value==''){alert("Enter From Date.");txtfdate.focus();  }
     else {txttdate.value=txtfdate.value;}
}
</script>

<TABLE style="BORDER-RIGHT: gray 2px solid; BORDER-TOP: gray 2px solid; BORDER-LEFT: gray 2px solid; WIDTH: 1px; BORDER-BOTTOM: gray 2px solid"><TBODY><TR><TD class="field_heading" align=center colSpan=1><asp:Label id="lblHeading" runat="server" Text="Customer Outstanding and Collection" ForeColor="White" Width="693px" CssClass="field_heading"></asp:Label></TD></TR><TR><TD style="WIDTH: 947px; HEIGHT: 24px" class="td_cell"><TABLE style="WIDTH: 694px"><TBODY><TR><TD>&nbsp;<asp:Panel id="Panel1" runat="server" Width="475px" CssClass="field_input" GroupingText="Select Date "><TABLE style="WIDTH: 447px"><TBODY>
    <tr>
        <td class="field_input" style="width: 7px">
        </td>
        <td class="field_input">
            <asp:DropDownList ID="ddlwithmovmt" runat="server" CssClass="drpdown" onchange="enabledatectrl()"
                Visible="False" Width="120px">
                <asp:ListItem Value="0">Transactions</asp:ListItem>
                <asp:ListItem Value="1">Balances</asp:ListItem>
            </asp:DropDownList></td>
    </tr>
    <TR><TD style="WIDTH: 7px" class="field_input"><asp:Label id="Label2" runat="server" Width="104px">From Date</asp:Label></TD><TD class="field_input"><asp:TextBox 
        id="txtFromDate" runat="server" Width="111px" CssClass="txtbox"></asp:TextBox>&nbsp;<asp:ImageButton id="ImgBtnFrmDt" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.png"></asp:ImageButton> 
            <cc1:MaskedEditValidator id="MskVFromDt" runat="server" Width="1px" CssClass="field_error" ControlExtender="MskFromDate" ControlToValidate="txtFromDate" Display="Dynamic" EmptyValueBlurredText="*" EmptyValueMessage="Date is required" ErrorMessage="MskVFromDate" InvalidValueBlurredMessage="*" InvalidValueMessage="Invalid Date" TooltipMessage="Input a date in dd/mm/yyyy format"></cc1:MaskedEditValidator></TD></TR>
        <tr><td class="field_input" style="WIDTH: 7px"><asp:Label ID="Label1" 
        runat="server" Text="To Date" Width="45px"></asp:Label></td>
            <td class="field_input"><asp:TextBox ID="txttoDate" runat="server" 
        CssClass="txtbox" Width="111px"></asp:TextBox> 
                <asp:ImageButton ID="ImgBtntoDt" runat="server" 
        ImageUrl="~/Images/Calendar_scheduleHS.png">
    </asp:ImageButton> 
                <cc1:MaskedEditValidator ID="MskVtoDt" runat="server" 
        ControlExtender="MsktoDate" ControlToValidate="txttoDate" 
        CssClass="field_error" Display="Dynamic" EmptyValueBlurredText="*" 
        EmptyValueMessage="Date is required" ErrorMessage="MskVFromDate" 
        InvalidValueBlurredMessage="*" InvalidValueMessage="Invalid Date" 
        TooltipMessage="Input a date in dd/mm/yyyy format" Width="1px"></cc1:MaskedEditValidator></td></tr></TBODY></TABLE></asp:Panel></TD></TR><TR><TD>
        <asp:Panel id="Panel4" runat="server" Width="679px" CssClass="td_cell" 
            GroupingText="Customer"><TABLE style="WIDTH: 676px"><TBODY>
        <tr><td class="td_cell">
            <input id="rbncustall" runat="server" checked name="customer" 
            type="radio" /> All</td><td class="td_cell">From Code</td><td 
            class="td_cell"><select id="ddlcustfrm" runat="server" class="drpdown" 
            disabled="true" onchange="CallWebMethod('frmcust');" style="WIDTH: 200px"> <option 
            selected="" value="[Select]">[Select]</option></select> </td>
            <td class="td_cell">From Name</td><td class="td_cell"><select 
            id="ddlcustfrmname" runat="server" class="drpdown" disabled="true" 
            onchange="CallWebMethod('frmcustname');" style="WIDTH: 225px"> <option 
            selected="" value="[Select]">[Select]</option></select> </td></tr>
        <tr><td class="td_cell">
            <input id="rbncustrange" runat="server" name="customer" type="radio" /> Range</td><td 
            class="td_cell">To Code</td><td class="td_cell"><select id="ddlcustto" 
            runat="server" class="drpdown" disabled="true" 
            onchange="CallWebMethod('tocust');" style="WIDTH: 200px"> <option 
            selected="" value="[Select]">[Select]</option></select> </td>
            <td class="td_cell">To Name</td><td class="td_cell"><select 
            id="ddlcusttoname" runat="server" class="drpdown" disabled="true" 
            onchange="CallWebMethod('tocustname');" style="WIDTH: 225px"> <option 
            selected="" value="[Select]">[Select]</option></select> </td></tr></TBODY></TABLE></asp:Panel></TD></TR><TR><TD>
        <asp:Panel id="Panel3" runat="server" Width="679px" CssClass="td_cell" 
            GroupingText="Control Account Code"><TABLE style="WIDTH: 676px"><TBODY>
        <tr><td class="td_cell" style="HEIGHT: 26px"><input id="rbACall" runat="server" 
            checked name="Account" type="radio" /> All</td>
            <td class="td_cell" style="HEIGHT: 26px">From Code</td>
            <td class="td_cell" style="HEIGHT: 26px">
                <select id="ddlFromAccount" runat="server" class="drpdown" disabled 
            onchange="CallWebMethod('fromaccountcode');" style="WIDTH: 200px"> <option 
            selected="" value="[Select]">[Select]</option></select> </td>
            <td class="td_cell" style="HEIGHT: 26px">From Name</td>
            <td class="td_cell" style="HEIGHT: 26px">
                <select id="ddlFromAccountName" runat="server" class="drpdown" 
            disabled onchange="CallWebMethod('fromaccountname');" style="WIDTH: 225px"> <option 
            selected="" value="[Select]">[Select]</option></select> </td></tr>
        <tr><td class="td_cell">
            <input id="rbACrange" runat="server" name="Account" type="radio" /> Range</td><td 
            class="td_cell">To Code</td><td class="td_cell"><select id="ddlToAccount" 
            runat="server" class="drpdown" disabled 
            onchange="CallWebMethod('toaccountcode');" style="WIDTH: 200px"> <option 
            selected="" value="[Select]">[Select]</option></select> </td>
            <td class="td_cell">To Name</td><td class="td_cell"><select 
            id="ddlToAccountName" runat="server" class="drpdown" disabled 
            onchange="CallWebMethod('toaccountname');" style="WIDTH: 225px"> <option 
            selected="" value="[Select]">[Select]</option></select> </td></tr></TBODY></TABLE></asp:Panel> </TD></TR><TR><TD>
        <asp:Panel id="Panel2" runat="server" Width="679px" CssClass="td_cell" 
            GroupingText="Category"><TABLE style="WIDTH: 676px"><TBODY>
        <tr><td class="td_cell">
            <input id="rbCatall" runat="server" checked name="Category" type="radio" /> All</td><td 
            class="td_cell">From Code</td><td class="td_cell"><select 
            id="ddlFromCategory" runat="server" class="drpdown" disabled 
            onchange="CallWebMethod('fromcategorycode');" style="WIDTH: 200px"> <option 
            selected="" value="[Select]">[Select]</option></select> </td>
            <td class="td_cell">From Name</td><td class="td_cell"><select 
            id="ddlFromCategoryName" runat="server" class="drpdown" disabled 
            onchange="CallWebMethod('fromcategoryname');" style="WIDTH: 225px"> <option 
            selected="" value="[Select]">[Select]</option></select> </td></tr>
        <tr><td class="td_cell">
            <input id="rbCatrange" runat="server" name="Category" type="radio" /> Range</td><td 
            class="td_cell">To Code</td><td class="td_cell"><select id="ddlToCategory" 
            runat="server" class="drpdown" disabled 
            onchange="CallWebMethod('tocategorycode');" style="WIDTH: 200px"> <option 
            selected="" value="[Select]">[Select]</option></select> </td>
            <td class="td_cell">To Name</td><td class="td_cell"><select 
            id="ddlToCategoryName" runat="server" class="drpdown" disabled 
            onchange="CallWebMethod('tocategoryname');" style="WIDTH: 225px"> <option 
            selected="" value="[Select]">[Select]</option></select> </td></tr></TBODY></TABLE></asp:Panel> </TD></TR><TR><TD>
        <asp:Panel id="Panel6" runat="server" Width="679px" CssClass="td_cell" 
            GroupingText="Market"><TABLE style="WIDTH: 676px"><TBODY>
        <tr><td class="td_cell">
            <input id="rbMarkall" runat="server" checked name="Market" type="radio" /> All</td><td 
            class="td_cell">From Code</td><td class="td_cell"><select 
            id="ddlFromMarket" runat="server" class="drpdown" disabled 
            onchange="CallWebMethod('frommarketcode');" style="WIDTH: 200px"> <option 
            selected="" value="[Select]">[Select]</option></select> </td>
            <td class="td_cell">From Name</td><td class="td_cell"><select 
            id="ddlFromMarketName" runat="server" class="drpdown" disabled 
            onchange="CallWebMethod('frommarketname');" style="WIDTH: 225px"> <option 
            selected="" value="[Select]">[Select]</option></select> </td></tr>
        <tr><td class="td_cell">
            <input id="rbMarkrange" runat="server" name="Market" type="radio" /> Range</td><td 
            class="td_cell">To Code</td><td class="td_cell"><select id="ddlToMarket" 
            runat="server" class="drpdown" disabled 
            onchange="CallWebMethod('tomarketcode');" style="WIDTH: 200px"> <option 
            selected="" value="[Select]">[Select]</option></select> </td>
            <td class="td_cell">To Name</td><td class="td_cell"><select 
            id="ddlToMarketName" runat="server" class="drpdown" disabled 
            onchange="CallWebMethod('tomarketname');" style="WIDTH: 225px"> <option 
            selected="" value="[Select]">[Select]</option></select> </td></tr></TBODY></TABLE></asp:Panel> </TD></TR><TR><TD style="HEIGHT: 98px"><asp:Panel id="Panel7" runat="server" Width="679px" CssClass="td_cell" GroupingText="Country"><TABLE style="WIDTH: 676px"><TBODY>
        <tr><td class="td_cell">
            <input id="rbCtrall" runat="server" checked name="Country" type="radio" /> All</td><td 
            class="td_cell">From Code</td><td class="td_cell"><select 
            id="ddlFromCountry" runat="server" class="drpdown" disabled 
            onchange="CallWebMethod('fromcountrycode');" style="WIDTH: 200px"> <option 
            selected="" value="[Select]">[Select]</option></select> </td>
            <td class="td_cell">From Name</td><td class="td_cell"><select 
            id="ddlFromCountryName" runat="server" class="drpdown" disabled 
            onchange="CallWebMethod('fromcountryname');" style="WIDTH: 225px"> <option 
            selected="" value="[Select]">[Select]</option></select> </td></tr>
        <tr><td class="td_cell">
            <input id="rbCtrrange" runat="server" name="Country" type="radio" /> Range</td><td 
            class="td_cell">To Code</td><td class="td_cell"><select id="ddlToCountry" 
            runat="server" class="drpdown" disabled 
            onchange="CallWebMethod('tocountrycode');" style="WIDTH: 200px"> <option 
            selected="" value="[Select]">[Select]</option></select> </td>
            <td class="td_cell">To Name</td><td class="td_cell"><select 
            id="ddlToCountryName" runat="server" class="drpdown" disabled 
            onchange="CallWebMethod('tocountryname');" style="WIDTH: 225px"> <option 
            selected="" value="[Select]">[Select]</option></select> </td></tr></TBODY></TABLE></asp:Panel> </TD></TR></TBODY></TABLE></TD></TR><TR><TD class="td_cell">&nbsp; <TABLE style="WIDTH: 399px"><TBODY><TR><TD style="WIDTH: 72px">Currency</TD><TD style="WIDTH: 137px">
        <SELECT style="WIDTH: 109px" id="ddlCurrency" class="drpdown" runat="server"><OPTION value="[Select]" selected>[Select]</OPTION></SELECT></TD><TD style="WIDTH: 71px"></TD></TR><TR><TD style="WIDTH: 100px">Report Order</TD><TD style="WIDTH: 137px">
        <asp:DropDownList id="ddlrptord" runat="server" Width="123px" 
            CssClass="drpdown"><asp:ListItem Value="0">Code</asp:ListItem>
<asp:ListItem Value="1">Name</asp:ListItem>
</asp:DropDownList></TD><TD style="WIDTH: 71px"></TD></TR><TR><TD style="WIDTH: 72px">Group By</TD><TD style="WIDTH: 137px">
        <asp:DropDownList id="ddlgpby" runat="server" Width="122px" CssClass="drpdown"><asp:ListItem Value="0">None</asp:ListItem>
<asp:ListItem Value="1">Market</asp:ListItem>
<asp:ListItem Value="2">Category</asp:ListItem>
<asp:ListItem Value="4">Control account Code</asp:ListItem>
</asp:DropDownList></TD><TD style="WIDTH: 100px"></TD></TR><TR><TD style="WIDTH: 72px">Include&nbsp;Zero</TD><TD style="WIDTH: 137px">
        <asp:DropDownList id="ddlinclzero" runat="server" Width="122px" 
            CssClass="drpdown"><asp:ListItem Value="0">No</asp:ListItem>
<asp:ListItem Value="1">Yes</asp:ListItem>
</asp:DropDownList></TD><TD style="WIDTH: 71px"></TD></TR></TBODY></TABLE></TD></TR><TR><TD class="td_cell" align=right>&nbsp; 
    <asp:Button id="Button1" onclick="Button1_Click1" runat="server" Text="Export" 
        CssClass="btn"></asp:Button>&nbsp;<asp:Button id="btnReport" 
        onclick="btnReport_Click" runat="server" Text="Load Report" 
        CssClass="btn" CausesValidation="False"></asp:Button> &nbsp;
    <asp:Button id="btnExit" tabIndex=6 onclick="btnExit_Click" runat="server" 
        Text=" Exit" CssClass="btn" CausesValidation="False"></asp:Button>&nbsp;<asp:Button 
        id="btnhelp" tabIndex=39 onclick="btnhelp_Click" runat="server" 
        Text="Help" CssClass="btn"></asp:Button></TD></TR></TBODY></TABLE><asp:ScriptManagerProxy id="ScriptManagerProxy1" runat="server"><Services>
<asp:ServiceReference Path="clsServices.asmx"></asp:ServiceReference>
</Services>
</asp:ScriptManagerProxy>&nbsp;&nbsp;&nbsp; <cc1:CalendarExtender id="ClsExFromDate" runat="server" Format="dd/MM/yyyy" PopupButtonID="ImgBtnFrmDt" TargetControlID="txtFromDate">
    </cc1:CalendarExtender><cc1:MaskedEditExtender id="MskFromDate" runat="server" TargetControlID="txtFromDate" AcceptNegative="Left" DisplayMoney="Left" ErrorTooltipEnabled="True" Mask="99/99/9999" MaskType="Date" MessageValidatorTip="true"></cc1:MaskedEditExtender><cc1:MaskedEditExtender id="MsktoDate" runat="server" TargetControlID="txttoDate" AcceptNegative="Left" DisplayMoney="Left" ErrorTooltipEnabled="True" Mask="99/99/9999" MaskType="Date" MessageValidatorTip="true"></cc1:MaskedEditExtender><cc1:CalendarExtender id="ClsExtoDate" runat="server" Format="dd/MM/yyyy" PopupButtonID="ImgBtntoDt" TargetControlID="txttoDate"></cc1:CalendarExtender> 

</asp:Content>
