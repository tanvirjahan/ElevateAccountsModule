<%@ Page Language="VB" MasterPageFile="~/PriceListMaster.master" AutoEventWireup="false" CodeFile="PriceList.aspx.vb" Inherits="PriseList"  %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
    <%@ OutputCache location="none" %> 
<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">
<script type="text/javascript">
    window.moveTo(0, 0);
    window.resizeTo(screen.availWidth, screen.availHeight);

    
</script>
<style type="text/css" >
            .ModalPopupBG
            {
                background-color: gray;
                filter: alpha(opacity=50);
                opacity: 0.7;
            }

            .HellowWorldPopup
            {
                min-width:200px;
                min-height:150px;
                background:white;
                font-size: 10pt;
	            font-weight: bold;
	            border-bottom-style:double;
	            border-width:medium;

	
            }
      
        *{
	outline:none;
           
        }
    .style4
    {
        width: 111px;
    }
    .style5
    {
        width: 124px;
    }
    .NoDisplay
    {
       display:none; 
    }
</style>

    <script language ="javascript" type ="text/javascript" >


function CallWebMethod(methodType)
    {
       switch(methodType)
        {
            case "sptypecode":
            
                var select=document.getElementById("<%=ddlSPTypeCD.ClientID%>");
                var codeid=select.options[select.selectedIndex].text;
                var selectname=document.getElementById("<%=ddlSPTypeNM.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;
                var connstr=document.getElementById("<%=txtconnection.ClientID%>");  
                constr=connstr.value   
                
                ColServices.clsServices.GetSuppAgentCodeListnew(constr,codeid,FillSupplierAgentCode,ErrorHandler,TimeOutHandler);
                ColServices.clsServices.GetSuppAgentNameListnew(constr,codeid,FillSupplierAgentName,ErrorHandler,TimeOutHandler);
              break;
            case "sptypename":
                var select=document.getElementById("<%=ddlSPTypeNM.ClientID%>");
                var codeid=select.options[select.selectedIndex].value;
                var selectname=document.getElementById("<%=ddlSPTypeCD.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;
                var connstr=document.getElementById("<%=txtconnection.ClientID%>");  
                constr=connstr.value   
                
                ColServices.clsServices.GetSuppAgentCodeListnew(constr,codeid,FillSupplierAgentCode,ErrorHandler,TimeOutHandler);
                ColServices.clsServices.GetSuppAgentNameListnew(constr,codeid,FillSupplierAgentName,ErrorHandler,TimeOutHandler);
                
                break;
            case "supagentcode":
                var select=document.getElementById("<%=ddlSupplierAgent.ClientID%>");
                
                var selectname=document.getElementById("<%=ddlSuppierAgentNM.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;
            break;
            case "supagentname":
                var select=document.getElementById("<%=ddlSuppierAgentNM.ClientID%>");
                var selectname=document.getElementById("<%=ddlSupplierAgent.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;
            break;
        case "partycode":
            var select = document.getElementById("<%=ddlSuppierCD.ClientID%>");
            var selectname = document.getElementById("<%=ddlSuppierNM.ClientID%>");
            var connstr = document.getElementById("<%=txtconnection.ClientID%>");
            selectname.value = select.options[select.selectedIndex].text;
            break;
        case "partyname":
            var select = document.getElementById("<%=ddlSuppierNM.ClientID%>");
            var selectname = document.getElementById("<%=ddlSuppierCD.ClientID%>");
            selectname.value = select.options[select.selectedIndex].text;
            break;
        }
    }
   function FillSupplierAgentCode(result)
    {
      	var ddl = document.getElementById("<%=ddlSupplierAgent.ClientID%>");
 		RemoveAll(ddl)
        for(var i=0;i<result.length;i++)
        {
            var option=new Option(result[i].ListText,result[i].ListValue);
            ddl.options.add(option);
        }
        ddl.value="[Select]";
    }

    function FillSupplierAgentName(result)
    {
        var ddl = document.getElementById("<%=ddlSuppierAgentNM.ClientID%>");
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
     function  GetValueFromMarket()
{

	var ddl = document.getElementById("<%=ddlMarketNM.ClientID%>");
	ddl.selectedIndex = -1;
		// Iterate through all dropdown items.
		for (i = 0; i < ddl.options.length; i++) 
		{
			if (ddl.options[i].text == 
			document.getElementById("<%=ddlmarketCD.ClientID%>").value)
			{
				// Item was found, set the selected index.
				ddl.selectedIndex = i;
				return true;
			}
		}
}
function  GetValueCodeMarket()
{
	var ddl = document.getElementById("<%=ddlmarketCD.ClientID%>");
	ddl.selectedIndex = -1;
		// Iterate through all dropdown items.
		for (i = 0; i < ddl.options.length; i++) 
		{
			if (ddl.options[i].text == 
			document.getElementById("<%=ddlMarketNM.ClientID%>").value)
			{
				// Item was found, set the selected index.
				ddl.selectedIndex = i;
				return true;
			}
		}
}
    function  GetValueFromCurrency()
{

	var ddl = document.getElementById("<%=ddlCurrencyNM.ClientID%>");
	ddl.selectedIndex = -1;
		// Iterate through all dropdown items.
		for (i = 0; i < ddl.options.length; i++) 
		{
			if (ddl.options[i].text == 
			document.getElementById("<%=ddlCurrencyCD.ClientID%>").value)
			{
				// Item was found, set the selected index.
				ddl.selectedIndex = i;
				return true;
			}
		}
}
function  GetValueCodeCurrency()
{
	var ddl = document.getElementById("<%=ddlCurrencyCD.ClientID%>");
	ddl.selectedIndex = -1;
		// Iterate through all dropdown items.
		for (i = 0; i < ddl.options.length; i++) 
		{
			if (ddl.options[i].text == 
			document.getElementById("<%=ddlCurrencyNM.ClientID%>").value)
			{
				// Item was found, set the selected index.
				ddl.selectedIndex = i;
				return true;
			}
		}
}

function GetValueFromSubSeason()
{

	var ddl = document.getElementById("<%=ddlSubSeasNM.ClientID%>");
	ddl.selectedIndex = -1;
		// Iterate through all dropdown items.
		for (i = 0; i < ddl.options.length; i++) 
		{
			if (ddl.options[i].text == 
			document.getElementById("<%=ddlSubSeas.ClientID%>").value)
			{
				// Item was found, set the selected index.
				ddl.selectedIndex = i;
				return true;
			}
		}
}
function GetValueCodeSubSeason()
{
	var ddl = document.getElementById("<%=ddlSubSeas.ClientID%>");
	ddl.selectedIndex = -1;
		// Iterate through all dropdown items.
		for (i = 0; i < ddl.options.length; i++) 
		{
			if (ddl.options[i].text == 
			document.getElementById("<%=ddlSubSeasNM.ClientID%>").value)
			{
				// Item was found, set the selected index.
				ddl.selectedIndex = i;
				return true;
			}
		}
}

  function  GetValueFromSupplier()
{
   
	var ddl = document.getElementById("<%=ddlSuppierNM.ClientID%>");
	ddl.selectedIndex = -1;
		// Iterate through all dropdown items.
		for (i = 0; i < ddl.options.length; i++) 
		{
			if (ddl.options[i].text == 
			document.getElementById("<%=ddlSuppierCD.ClientID%>").value)
			{
				// Item was found, set the selected index.
				ddl.selectedIndex = i;
				return true;
			}
		}
}
function  GetValueCodeSupplier() {
    
	var ddl = document.getElementById("<%=ddlSuppierCD.ClientID%>");
	ddl.selectedIndex = -1;
		// Iterate through all dropdown items.
		for (i = 0; i < ddl.options.length; i++) 
		{
			if (ddl.options[i].text == 
			document.getElementById("<%=ddlSuppierNM.ClientID%>").value)
			{
				// Item was found, set the selected index.
				ddl.selectedIndex = i;
				return true;
			}
		}
}


function ChangeDate()
 {
   
     var txtfdate=document.getElementById("<%=txtFromDate.ClientID%>");
     var txttdate=document.getElementById("<%=txtToDate.ClientID%>");
     if (txtfdate.value==''){alert("Enter From Date.");txtfdate.focus();  }
     else {txttdate.value=txtfdate.value;}
 }


 function confirmapprove(plistcode) {
  //   var plistcode = document.getElementById(plistcode);
     var cnf = confirm('Already approved do you want to edit?');
     if (cnf==1) {
         window.open('HeaderInfonew.aspx?State=Edit&RefCode=' + plistcode + '&status=1','HeaderInfo','width='+screen.availWidth+' height='+screen.availHeight+' left=0,top=0 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');

         return false;     
     }
     else
     {
      return false;
     }

}




</script>
                <table style="width: 939px">
                    <tr>
                        <td colspan="4" style="text-align: center; width: 1180px;" align="center" class="field_heading">
                            <asp:Label ID="Label1" runat="server" CssClass="field_heading" Text="Price List"
                                 ></asp:Label></td>
                    </tr>
                    <tr style="font-size: 10pt">
                        <td colspan="4" style="text-align: center; width: 1180px;" align="center">
                            <asp:RadioButton ID="rbtnsearch" runat="server" AutoPostBack="True" Checked="True"
                                CssClass="td_cell" ForeColor="Black" GroupName="GrSearch" OnCheckedChanged="rbtnsearch_CheckedChanged"
                                Text="Search" />
                            &nbsp;
                            <asp:RadioButton ID="rbtnadsearch" runat="server" AutoPostBack="True" CssClass="td_cell"
                                ForeColor="Black" GroupName="GrSearch" OnCheckedChanged="rbtnadsearch_CheckedChanged"
                                Text="Advance Search" />
                            &nbsp;<asp:Button ID="btnSearch" runat="server" CssClass="search_button"
                                    Font-Bold="False" Text="Search" />&nbsp;
                            <asp:Button ID="btnClear" runat="server" CssClass="search_button" 
                                Font-Bold="False" Text="Clear" />&nbsp;<asp:Button ID="btnHelp" runat="server"
                                            CssClass="search_button" OnClick="btnHelp_Click" Text="Help" />
                            &nbsp;&nbsp;&nbsp;
                            <asp:Button ID="btnAddNew" runat="server" CssClass="btn" Font-Bold="False"
                                            OnClick="btnAddNew_Click" Text="Add New" />
                            &nbsp;&nbsp;&nbsp;
                            <asp:Button ID="btnPrint" runat="server" CssClass="btn" Text="Print" 
                                Visible="False" />&nbsp;&nbsp; </td>
                    </tr>
                    <tr style="font-size: 10pt">
                        <td align="center" colspan="4" style="text-align: left; width: 1180px;">
                            <asp:UpdatePanel id="UpdatePanel1" runat="server">
                                <contenttemplate>
<TABLE style="WIDTH: 1181px; TEXT-ALIGN: left" class="td_cell"><TBODY><TR><TD colSpan=4>
    <TABLE style="WIDTH: 1183px" class="td_cell"><TBODY><TR><TD style="WIDTH: 105px">PL Code </TD><TD style="WIDTH: 260px"><asp:TextBox id="TxtPLCD" runat="server" CssClass="txtbox" Width="216px"></asp:TextBox></TD><TD style="WIDTH: 115px">Order by</TD><TD><asp:DropDownList id="ddlOrderBy" runat="server" CssClass="drpdown" AutoPostBack="True" Width="128px"><asp:ListItem Value="0">PList Code Desc</asp:ListItem>
<asp:ListItem Value="1">PList Code Asc</asp:ListItem>
<asp:ListItem Value="2">Supplier Code</asp:ListItem>
<asp:ListItem Value="3">Supplier Name</asp:ListItem>
<asp:ListItem Value="4">Supplier Agent</asp:ListItem>
<asp:ListItem Value="5">Market</asp:ListItem>
<asp:ListItem Value="6">Sub Season</asp:ListItem>
</asp:DropDownList></TD>
        <td>
            &nbsp;</td>
        </TR><TR><TD style="WIDTH: 105px">Supplier Code</TD><TD style="WIDTH: 260px"><SELECT onchange="CallWebMethod('partycode');" style="WIDTH: 223px" id="ddlSuppierCD" class="drpdown" runat="server"> <OPTION selected></OPTION></SELECT></TD><TD style="WIDTH: 115px">Supplier Name</TD><TD><SELECT  onchange="CallWebMethod('partyname');" style="WIDTH: 350px" id="ddlSuppierNM" class="drpdown" runat="server"> <OPTION selected></OPTION></SELECT></TD>
            <td>
                <input id="txtsupname" runat="server"  type="text" class="txtbox" style="width: 437px" maxlength="200" />
            </td>
        </TR><TR><TD style="WIDTH: 105px">Market&nbsp;Code</TD><TD style="WIDTH: 260px">
            <select id="ddlmarketCD" runat="server" class="drpdown" name="D1" 
                onchange="GetValueFromMarket()" style="WIDTH: 223px">
                <option selected=""></option>
            </select>
            </TD><TD style="WIDTH: 115px">Market&nbsp;Name</TD><TD>
            <select id="ddlMarketNM" runat="server" class="drpdown" name="D2" 
                onchange="GetValueCodeMarket()" style="WIDTH: 217px">
                <option selected=""></option>
            </select>
            </TD>
            <td>
                &nbsp;</td>
        </TR>
    <tr>
        <td style="width: 105px">
            Revision From Date</td>
        <td style="width: 260px">
            <asp:TextBox ID="txtFromDate" runat="server" CssClass="field_input" 
                Width="80px"></asp:TextBox><asp:ImageButton
                ID="ImgBtnFrmDt" runat="server" 
                ImageUrl="~/Images/Calendar_scheduleHS.png" />
            <cc1:MaskedEditValidator ID="MEVFromDate" runat="server" ControlExtender="MEFromDate"
                ControlToValidate="txtFromDate" CssClass="field_error" Display="Dynamic" EmptyValueBlurredText="Date is required"
                EmptyValueMessage="Date is required" InvalidValueBlurredMessage="Invalid Date"
                InvalidValueMessage="Invalid Date" 
                TooltipMessage="Input a date in dd/mm/yyyy format"></cc1:MaskedEditValidator></td>
        <td style="width: 115px">
            Revision To Date</td>
        <td>
            <asp:TextBox ID="txtToDate" runat="server" CssClass="fiel_input" Width="80px"></asp:TextBox><asp:ImageButton
                ID="ImgBtnRevDate" runat="server" 
                ImageUrl="~/Images/Calendar_scheduleHS.png" />
            <cc1:MaskedEditValidator ID="MEVToDate" runat="server" ControlExtender="METoDate"
                ControlToValidate="txtToDate" CssClass="field_error" Display="Dynamic" EmptyValueBlurredText="Date is required"
                EmptyValueMessage="Date is required" InvalidValueBlurredMessage="Invalid Date"
                InvalidValueMessage="Invalid Date" 
                TooltipMessage="Input a date in dd/mm/yyyy format"></cc1:MaskedEditValidator></td>
        <td>
            &nbsp;</td>
    </tr>
    <TR><TD style="WIDTH: 105px">From Date</TD><TD style="WIDTH: 260px">
        <asp:TextBox ID="txtPfromdate" runat="server" CssClass="fiel_input" 
            Width="80px"></asp:TextBox>
        &nbsp;<asp:ImageButton ID="ImgPBtnFrmDt" runat="server" 
            ImageUrl="~/Images/Calendar_scheduleHS.png" />
        <cc1:MaskedEditValidator ID="MEVPFromDate" runat="server" 
            ControlExtender="MEPFromDate" ControlToValidate="txtPfromdate" 
            CssClass="field_error" Display="Dynamic" 
            EmptyValueBlurredText="Date is required" EmptyValueMessage="Date is required" 
            ErrorMessage="MEPFromDate" InvalidValueBlurredMessage="Invalid Date" 
            InvalidValueMessage="Invalid Date" 
            TooltipMessage="Input a date in dd/mm/yyyy format">
        </cc1:MaskedEditValidator>
        </TD><TD style="WIDTH: 115px">To Date</TD><TD>
        <asp:TextBox ID="txtPtodate" runat="server" CssClass="fiel_input" Width="80px"></asp:TextBox>
        &nbsp;<asp:ImageButton ID="ImgPBtntoDt" runat="server" 
            ImageUrl="~/Images/Calendar_scheduleHS.png" />
        <cc1:MaskedEditValidator ID="MEVPToDate" runat="server" 
            ControlExtender="MEPToDate" ControlToValidate="txtPToDate" 
            CssClass="field_error" Display="Dynamic" 
            EmptyValueBlurredText="Date is required" EmptyValueMessage="Date is required" 
            ErrorMessage="MEPToDate" InvalidValueBlurredMessage="Invalid Date" 
            InvalidValueMessage="Invalid Date" 
            TooltipMessage="Input a date in dd/mm/yyyy format">
        </cc1:MaskedEditValidator>
        </TD>
        <td>
            &nbsp;</td>
        </TR><TR><TD style="WIDTH: 105px">Price List</TD><TD style="WIDTH: 260px">
            <asp:DropDownList id="ddlPriceList" runat="server" CssClass="drpdown" 
                Width="223px"><asp:ListItem>[Select]</asp:ListItem>
<asp:ListItem Value="Normal Rates 1 Night">Normal Rates 1 Night</asp:ListItem>
<asp:ListItem Value="Weekend Rates 1 Night">Weekend Rates 1 Night</asp:ListItem>
                <asp:ListItem>Weekly Rates 7 Nights</asp:ListItem>
                <asp:ListItem Value="Normal Rates &gt; 1 Night"></asp:ListItem>
                <asp:ListItem>Weekend Rates &gt; 1 Night</asp:ListItem>
</asp:DropDownList></TD><TD style="WIDTH: 115px">Approved /Unapproved</TD><TD>
            <asp:DropDownList ID="DDLstatus" runat="server" CssClass="drpdown" 
                Width="210px">
                <asp:ListItem Value="0">All</asp:ListItem>
                <asp:ListItem Value="1">Unapprove</asp:ListItem>
                <asp:ListItem Value="2">Approve</asp:ListItem>
            </asp:DropDownList>
            </TD>
            <td>
                &nbsp;</td>
        </TR>
        <tr>
            <td style="WIDTH: 105px">
                Show in&nbsp;Web</td>
            <td style="WIDTH: 260px">
                <asp:DropDownList ID="ddlshowweb" runat="server" CssClass="drpdown" 
                    Width="210px">
                    <asp:ListItem Value="0">All</asp:ListItem>
                    <asp:ListItem Value="1">Yes</asp:ListItem>
                    <asp:ListItem Value="2">No</asp:ListItem>
                </asp:DropDownList>
            </td>
            <td style="WIDTH: 115px">
                </td>
            <td>
            </td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td style="WIDTH: 105px">
                Promotion Name</td>
            <td style="WIDTH: 260px">
                <asp:TextBox ID="Txtprmname" runat="server" CssClass="txtbox" Width="260px"></asp:TextBox>
            </td>
            <td style="WIDTH: 115px">
                Promotion Id</td>
            <td>
                <asp:TextBox ID="Txtpromotionid" runat="server" CssClass="txtbox" Width="191px"></asp:TextBox>
            </td>
            <td>
                &nbsp;</td>
        </tr>
        </TBODY></TABLE></TD></TR><TR><TD colSpan=4>
        <asp:Panel id="pnlHeader" runat="server" Visible="False">
        <TABLE style="WIDTH: 768px"><TBODY><TR>
        <TD style="WIDTH: 94px" class="td_cell">Supplier&nbsp;Type&nbsp;Code</TD>
        <TD style="WIDTH: 259px">
        <SELECT style="WIDTH: 223px" id="ddlSPTypeCD" class="drpdown" onchange="CallWebMethod('sptypecode');" runat="server">
         <OPTION selected></OPTION></SELECT> </TD><TD style="WIDTH: 116px" class="td_cell">Supplier&nbsp;Type&nbsp;Name</TD>
         
         <TD><SELECT style="WIDTH: 217px" id="ddlSPTypeNM" class="drpdown" onchange="CallWebMethod('sptypename');" runat="server"> <OPTION selected></OPTION></SELECT> </TD></TR><TR>
            <TD style="WIDTH: 94px" class="td_cell">Supplier&nbsp;Agent&nbsp;Code</TD><TD style="WIDTH: 259px"><SELECT style="WIDTH: 223px" id="ddlSupplierAgent" class="drpdown" onchange="CallWebMethod('supagentcode');" runat="server"> <OPTION selected></OPTION></SELECT> </TD><TD style="WIDTH: 116px" class="td_cell">Supplier&nbsp;Agent&nbsp;Name</TD><TD><SELECT style="WIDTH: 217px" id="ddlSuppierAgentNM" class="drpdown" onchange="CallWebMethod('supagentname');" runat="server"> <OPTION selected></OPTION></SELECT> </TD></TR><TR>
            <TD style="WIDTH: 94px" class="td_cell">Currency&nbsp;Code</TD><TD style="WIDTH: 259px"><SELECT onchange="GetValueFromCurrency()" style="WIDTH: 223px" id="ddlCurrencyCD" class="drpdown" runat="server"> <OPTION selected></OPTION></SELECT> </TD><TD style="WIDTH: 116px" class="td_cell">Currency&nbsp;Name</TD><TD><SELECT onchange="GetValueCodeCurrency()" style="WIDTH: 217px" id="ddlCurrencyNM" class="drpdown" runat="server"> <OPTION selected></OPTION></SELECT> </TD></TR><TR>
            <TD style="WIDTH: 94px" class="td_cell">Sub&nbsp;Season&nbsp;Code</TD><TD style="WIDTH: 259px"><SELECT onchange="GetValueFromSubSeason()" style="WIDTH: 222px" id="ddlSubSeas" class="drpdown" runat="server"> <OPTION selected></OPTION></SELECT> </TD><TD style="WIDTH: 116px" class="td_cell">Sub&nbsp;Season&nbsp;Name</TD><TD><SELECT onchange="GetValueCodeSubSeason()" style="WIDTH: 217px" id="ddlSubSeasNM" class="drpdown" runat="server"> <OPTION selected></OPTION></SELECT> </TD></TR></TBODY></TABLE></asp:Panel></TD></TR></TBODY></TABLE>
                                    &nbsp;
                                    <table style="width: 861px">
                                        <tr>
                                            <td style="width: 100px">
                                                <cc1:CalendarExtender id="CEFromDate" runat="server" Format="dd/MM/yyyy" PopupButtonID="ImgBtnFrmDt" TargetControlID="txtFromDate"></cc1:CalendarExtender> <cc1:MaskedEditExtender id="MEFromDate" runat="server" TargetControlID="txtFromDate" MaskType="Date" Mask="99/99/9999"></cc1:MaskedEditExtender><cc1:CalendarExtender id="CEToDate" runat="server" Format="dd/MM/yyyy" PopupButtonID="ImgBtnRevDate" TargetControlID="txtToDate"></cc1:CalendarExtender> <cc1:MaskedEditExtender id="METoDate" runat="server" TargetControlID="txtToDate" MaskType="Date" Mask="99/99/9999"></cc1:MaskedEditExtender> 
                                            </td>
                                            <td style="width: 100px">
                                                <cc1:CalendarExtender id="CEPFromDate" runat="server" Format="dd/MM/yyyy" PopupButtonID="ImgPBtnFrmDt" TargetControlID="txtPFromDate">
                            </cc1:CalendarExtender>
                            <cc1:MaskedEditExtender id="MEPFromDate" runat="server" TargetControlID="txtPFromDate" MaskType="Date" Mask="99/99/9999">
                            </cc1:MaskedEditExtender><cc1:CalendarExtender id="CEPToDate" runat="server" Format="dd/MM/yyyy" PopupButtonID="ImgPBtntoDt" TargetControlID="txtPToDate">
                            </cc1:CalendarExtender>
                                                <cc1:MaskedEditExtender id="MEPToDate" runat="server" TargetControlID="txtPToDate" MaskType="Date" Mask="99/99/9999">
                                                </cc1:MaskedEditExtender>
                                            </td>
                                        </tr>
                                    </table>
                                    &nbsp;&nbsp;
</contenttemplate>
                            </asp:UpdatePanel></td>
                    </tr>
                    <tr>
                        <td align="left" class="td_cell" colspan="5">
                            <table style="width: 1060px">
                                <tr>
                                    <td colspan="2" class="td_cell" align="left">
                                        &nbsp;
                                        <asp:Button ID="btnExportToExcel" runat="server" CssClass="btn"
                                            Text="Export To Excel" />&nbsp;
                                        <input id="txtconnection" runat="server" style="visibility: hidden; width: 12px;
                                            height: 9px" type="text" /></td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" style="text-align: left; width: 1180px;">
                            <asp:UpdatePanel id="UpdatePanel2" runat="server">
                                <contenttemplate>
<asp:GridView id="gv_SearchResult" runat="server" Font-Size="10px" CssClass="grdstyle" Width="950px" GridLines="Vertical" CellPadding="3" BorderWidth="1px" BorderStyle="None" BorderColor="#999999" AutoGenerateColumns="False" AllowSorting="True" AllowPaging="True">
<FooterStyle CssClass="grdfooter"></FooterStyle>
<Columns>
<asp:TemplateField Visible="False" HeaderText="Price List Code"><EditItemTemplate>
      <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("plistcode") %>'></asp:TextBox>
                                        
</EditItemTemplate>
<ItemTemplate>
<asp:Label id="lblplistcode" runat="server" Text='<%# Bind("plistcode") %>'></asp:Label> 
</ItemTemplate>
</asp:TemplateField >
    <asp:TemplateField HeaderText="Approve" Visible="False">
        <EditItemTemplate>
            <asp:TextBox ID="TextBox2" runat="server" Text='<%# Bind("approve") %>'></asp:TextBox>
        </EditItemTemplate>
        <ItemTemplate>
            <asp:Label ID="lblapprove" runat="server" Text='<%# Bind("approve") %>'></asp:Label>
        </ItemTemplate>
       

    </asp:TemplateField>

        <asp:TemplateField HeaderText="partycode" Visible="False">
        <EditItemTemplate>
            <asp:TextBox ID="TextBox3" runat="server" Text='<%# Bind("partycode") %>'></asp:TextBox>
        </EditItemTemplate>
        <ItemTemplate>
            <asp:Label ID="lblparty" runat="server" Text='<%# Bind("partycode") %>'></asp:Label>
        </ItemTemplate>
       

    </asp:TemplateField>



        <asp:TemplateField HeaderText="plgrpcode" Visible="False">
        <EditItemTemplate>
            <asp:TextBox ID="TextBox4" runat="server" Text='<%# Bind("plgrpcode") %>'></asp:TextBox>
        </EditItemTemplate>
        <ItemTemplate>
            <asp:Label ID="lblmarket" runat="server" Text='<%# Bind("plgrpcode") %>'></asp:Label>
        </ItemTemplate>
       

    </asp:TemplateField>


     <asp:TemplateField HeaderText="agentcode" Visible="False">
        <EditItemTemplate>
            <asp:TextBox ID="TextBox5" runat="server" Text='<%# Bind("supagentname") %>'></asp:TextBox>
        </EditItemTemplate>
        <ItemTemplate>
            <asp:Label ID="lblagent" runat="server" Text='<%# Bind("supagentname") %>'></asp:Label>
        </ItemTemplate>
       

    </asp:TemplateField>


<asp:BoundField DataField="plistcode" SortExpression="plistcode" HeaderText="Price List Code"></asp:BoundField>
<asp:BoundField DataField="partycode" SortExpression="partycode" HeaderText="Supplier Code"></asp:BoundField>
<asp:BoundField DataField="partyname" SortExpression="partyname" HeaderText="Supplier Name">
<ItemStyle Width="1440pt" HorizontalAlign="Left"></ItemStyle>

<HeaderStyle Width="1440pt" HorizontalAlign="Left"></HeaderStyle>
</asp:BoundField>
<asp:BoundField DataField="supagentname" SortExpression="supagentname" HeaderText="Supplier Agent ">
<ItemStyle Width="3500px" HorizontalAlign="Left"></ItemStyle>

<HeaderStyle Width="3500px" HorizontalAlign="Left"></HeaderStyle>
</asp:BoundField>
<asp:BoundField DataField="currcode" SortExpression="currcode" HeaderText="Curr">
<ItemStyle Width="1440px" HorizontalAlign="Left"></ItemStyle>

<HeaderStyle Width="1440px" HorizontalAlign="Left"></HeaderStyle>
</asp:BoundField>
<asp:BoundField DataField="plgrpcode" SortExpression="plgrpcode" HeaderText="Market">
<ItemStyle Width="1440px" HorizontalAlign="Left"></ItemStyle>

<HeaderStyle Width="1440px" HorizontalAlign="Left"></HeaderStyle>
</asp:BoundField>
<asp:BoundField DataField="subseascode" SortExpression="subseascode" HeaderText="Sub Season"></asp:BoundField>
<asp:BoundField DataField="pricecode" SortExpression="pricecode" HeaderText="Prmotion Name"></asp:BoundField>
<asp:TemplateField  HeaderText="Promotionid"  SortExpression="promotionid" ><EditItemTemplate>
 <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("promotionid") %>'></asp:TextBox>
</EditItemTemplate>
<ItemTemplate>
<asp:Label ID="lblpromotionid" runat="server" Text='<%# Bind("promotionid") %>'></asp:Label>
</ItemTemplate>
</asp:TemplateField>
<asp:BoundField HtmlEncode="False" DataFormatString="{0:dd/MM/yyyy}" DataField="revisiondate" SortExpression="revisiondate" HeaderText="Revision Date"></asp:BoundField>
<asp:BoundField HtmlEncode="False" DataFormatString="{0:dd/MM/yyyy}" DataField="frmdate" SortExpression="frmdate" HeaderText="From Date"></asp:BoundField>
<asp:BoundField HtmlEncode="False" DataFormatString="{0:dd/MM/yyyy}" DataField="todate" SortExpression="todate" HeaderText="To Date"></asp:BoundField>
<asp:BoundField DataField="plisttype" SortExpression="plisttype" HeaderText="Price List Type"></asp:BoundField>
<asp:BoundField DataField="plist_mode" SortExpression="plist_mode" HeaderText="Mode"></asp:BoundField>
<asp:BoundField DataField="weekend1" SortExpression="weekend1" HeaderText="Week End1"></asp:BoundField>
<asp:BoundField DataField="weekend2" SortExpression="weekend2" HeaderText="Week End2"></asp:BoundField>
<asp:BoundField DataField="approve" SortExpression="approve" HeaderText="Status"></asp:BoundField>
<asp:BoundField DataField="showagent" SortExpression="ShowAgent" HeaderText="Show Web"></asp:BoundField>

<asp:BoundField HtmlEncode="False" DataFormatString="{00:dd/MM/yyyy hh:mm:ss tt}" DataField="adddate" SortExpression="adddate" HeaderText="Date Created">
<ItemStyle Width="1440px"></ItemStyle>

<HeaderStyle Width="1440px"></HeaderStyle>
</asp:BoundField>
<asp:BoundField DataField="adduser" SortExpression="adduser" HeaderText="User Created">
<ItemStyle Width="1440px"></ItemStyle>

<HeaderStyle Width="1440px"></HeaderStyle>
</asp:BoundField>
<asp:BoundField HtmlEncode="False" DataFormatString="{00:dd/MM/yyyy hh:mm:ss tt}" DataField="moddate" SortExpression="moddate" HeaderText="Date Modified">
<ItemStyle Width="1440px"></ItemStyle>

<HeaderStyle Width="1440px"></HeaderStyle>
</asp:BoundField>
<asp:BoundField DataField="moduser" SortExpression="moduser" HeaderText="User Modified">
<ItemStyle Width="1440px"></ItemStyle>

<HeaderStyle Width="1440px"></HeaderStyle>
</asp:BoundField>
<asp:ButtonField HeaderText="Action" Text="Edit" CommandName="Editrow">
<ItemStyle ForeColor="Blue"></ItemStyle>
</asp:ButtonField>
<asp:ButtonField HeaderText="Action" Text="View" CommandName="View">
<ItemStyle ForeColor="Blue"></ItemStyle>
</asp:ButtonField>
<asp:ButtonField HeaderText="Action" Text="Delete" CommandName="Deleterow">
<ItemStyle ForeColor="Blue"></ItemStyle>
</asp:ButtonField>
<asp:ButtonField HeaderText="Action" Text="Copy" CommandName="Copy">
<ControlStyle ForeColor="Blue"></ControlStyle>
</asp:ButtonField>
<asp:ButtonField HeaderText="Action" Text="Print" CommandName="Print" ShowHeader="True">
<ControlStyle ForeColor="Blue"></ControlStyle>
</asp:ButtonField>
<asp:ButtonField HeaderText="Action" Text="Promotion" CommandName="Promotion">
<ItemStyle ForeColor="Blue"></ItemStyle>
</asp:ButtonField>

<asp:ButtonField HeaderText="Action" Text="PList links" CommandName="plinks">
<ItemStyle ForeColor="Blue"></ItemStyle>
</asp:ButtonField>
</Columns>

<RowStyle CssClass="grdRowstyle"></RowStyle>

<SelectedRowStyle CssClass="grdselectrowstyle"></SelectedRowStyle>

<PagerStyle CssClass="grdpagerstyle" HorizontalAlign="Center"></PagerStyle>

<HeaderStyle CssClass="grdheader" ForeColor="White"></HeaderStyle>

<AlternatingRowStyle CssClass="grdAternaterow"></AlternatingRowStyle>
</asp:GridView> <asp:Label id="lblMsg" runat="server" Text="Records not found. Please redefine search criteria" 
                                        Font-Size="8pt" Font-Names="Verdana" Font-Bold="True" Width="357px" 
                                        Visible="False" CssClass="lblmsg"></asp:Label> 
</contenttemplate>
                            </asp:UpdatePanel>
                            &nbsp;
                        </td>
                    </tr>
                </table>

                
  <cc1:modalpopupextender id="ModalPopupDays" runat="server" BehaviorID="ModalPopupDays"
	cancelcontrolid="btnCancel1" okcontrolid="btnOkay" 
	targetcontrolid="btnInvisibleGuest" popupcontrolid="daysgrid"
	popupdraghandlecontrolid="PopupHeader" drag="true"  
	>
</cc1:modalpopupextender>

      <div id ="daysgrid" style="display:none;background:#CCCCCC " runat ="server">         
            <input id="btnInvisibleGuest" runat="server" type="button" value="Cancel" style="visibility:hidden" />
                    <input id="btnOkay" type="button" value="OK" style="visibility:hidden"  />
                    <input id="btnCancel1" type="button" value="Cancel" style="visibility:hidden" />



 
  <asp:Panel id="Panel4" runat="server"  Width="300px" 
            GroupingText=""  Font-Size="14px" Font-Bold=true BorderColor="#000">

<table >
 
<tr>
    <td><asp:HyperLink id="hypmaxoccu" Font-Size=10  runat ="server"  CssClass="field_caption" text ="Maximum Occupancy"  NavigateUrl="maxaccomodationsearch.aspx"/> </td>
   <td width="25px"> </td>
    <td><asp:HyperLink id="hyphtl" Font-Size=10  runat ="server"  CssClass="field_caption" text ="Hotel Construction"  NavigateUrl="hotelsconstructionsearch.aspx"/> </td>
    <tr><td> </td></tr>
</tr>
<tr>
   <td>
   
   <asp:HyperLink id="hypgenplcy" Font-Size="10"  runat ="server"  CssClass="field_caption" text ="General Policy" NavigateUrl="generalpolicysearch.aspx"  /> 
   
   </td>
  <td width="25px"> </td>
    <td><asp:HyperLink id="hypcanplcy" Font-Size=10  runat ="server"  CssClass="field_caption" text ="Cancellation Policy"  NavigateUrl="CancellationpolicySearch.aspx"/> </td>
     <tr><td> </td></tr>
</tr>

<tr>
   <td><asp:HyperLink id="hyppromo" Font-Size=10  runat ="server"  CssClass="field_caption" text ="Promotion"  NavigateUrl="promotionsearch.aspx?AutoNo"/> </td>
  <td width="25px"> </td>
   <td><asp:HyperLink id="hypblksales" Font-Size=10  runat ="server"  CssClass="field_caption" text ="Blockfull Sales"  NavigateUrl="blockfullsalessearch.aspx"/> </td>
    <tr><td> </td></tr>
</tr>

<tr>
  <td><asp:HyperLink id="hypcomprmks" Font-Size=10  runat ="server"  CssClass="field_caption" text ="Compulosry Remarks"  NavigateUrl="compulsoryremarkssearch.aspx"/> </td>
   <td width="25px"> </td>
   <td><asp:HyperLink id="hypminnights" Font-Size=10  runat ="server"  CssClass="field_caption" text ="Minimum Nights"  NavigateUrl="minimumnightssearch.aspx"/> </td>
   <tr><td> </td></tr>
</tr>
    
<tr>
   <td><asp:HyperLink id="hypspvntprice" Font-Size=10  runat ="server"  CssClass="field_caption" text ="Special Event Pricelist"  NavigateUrl="specialeventpricelistpage.aspx"/> </td>
  <td width="25px"> </td>
 

</tr>


</table>

<table>
<tr> 
<td width ="225px"> </td>
<td>&nbsp<asp:Button ID ="btnexit" Text ="Exit" runat ="server" CssClass ="btn"></asp:button></td>
</tr>
</table>
            
      </asp:Panel>         

            </div>

               
                <asp:ScriptManagerProxy id="ScriptManagerProxy1" runat="server">
        <services>
<asp:ServiceReference Path="~/clsServices.asmx"></asp:ServiceReference>
</services>
    </asp:ScriptManagerProxy>
</asp:Content>

