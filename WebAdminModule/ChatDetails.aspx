<%@ Page Title="" Language="VB" MasterPageFile="~/WebAdminMaster.master" AutoEventWireup="false" CodeFile="ChatDetails.aspx.vb" Inherits="WebAdminModule_ChatDetails" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">
    <script src="Js/Jquery-1.7.2.min.js" type="text/javascript"></script>
    <link href="Css/TableCss.css" rel="stylesheet" type="text/css" />
    <script src="../AgentsOnline/js/JqueryUI.js" type="text/javascript"></script>
    <link href="../AgentsOnline/css/JqueryUI.css" rel="stylesheet" type="text/css" />
     <script src="Js/Custom.js" type="text/javascript"></script>
<script type="text/javascript">

   
    function OpenNewWindow(Url, PageTitle, Left, Top, Width, height, Resizable, Scrollable) {
        window.open(Url, PageTitle, 'left=' + Left + ',top=' + Top + ',width=' + Width + ',height=' + height + ',resizable=' + Resizable + ',scrollbars=' + Scrollable);
    }

    function OpenChatWindow(MyPage) {
        var TotUrl = MyPage + '?FromDate=' + jQuery('.FromDate').val() + '&ToDate=' + jQuery('.ToDate').val() + '&SalesPerson=ALL&AgentOrSub=' + jQuery('.AgentType').val() + '&Message=' + jQuery('.MessageLike').val() + '&ChatUser=' + jQuery('.ChatUserName').val();
        OpenNewWindow(TotUrl, 'ChatDetail', 0, 0, screen.availwidth - 6, screen.availheight - 70, 1, 1);
    }
    function PrintPage() {
        window.print();
    }


     function CallWebMethod(methodType) {
        switch (methodType) {
            case "ddlCustomerName":
                var select = document.getElementById("<%=ddlCustomerName.ClientID%>");
                var codeid = select.options[select.selectedIndex].text;
                var selectname = document.getElementById("<%=hdnagentcode.ClientID%>");
                selectname.value = select.value//select.options[select.selectedIndex].text;
                break;
            case "ddlUserName":
                var select = document.getElementById("<%=ddlUserName.ClientID%>");
                var selectname = document.getElementById("<%=hdnuser.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;
                break;
      }
  }
</script>


<table width="770" cellpadding="2" border="1" style="border-collapse: collapse; font-family: Verdana; font-size: 10pt" id="maintabel">
	<tbody>
	<tr>

		<td width="694" bgcolor="#06788B" align="left" style="height: 14px" colspan="3">
            <input id="txttodaydate" runat="server" style="display: none; width: 33px" type="text" />
                <span style="display:inline-block;color:Black;font-family:Verdana;font-size:10pt;font-weight:bold;width:331px;" id="LabelTitle">Live Chat Admin</span></td>
		<td width="694" bgcolor="#06788B" align="right" style="height: 14px" colspan="3">
                <span style="font-weight: 700; font-size: 9pt"></span>
                <a style="color:Maroon;background-color:Transparent;font-family:Verdana;font-size:8pt;font-weight:bold;text-decoration: none" href="javascript:void(0)" id="OnlineUsers" onclick="Javascript:OpenNewWindow('OnlineUsers.aspx', 'OnlineUsers', 0, 0, screen.availwidth - 6, screen.availheight - 70, 1, 1);">Online Users | </a>
                <a style="color:Maroon;background-color:Transparent;font-family:Verdana;font-size:8pt;font-weight:bold;text-decoration: none" href="javascript:void(0)" id="Print" onclick="Javascript:PrintPage();">Print | </a>
             <!--   <a onclick="Javascript:window.close();" style="cursor:hand; font-size :8pt; font-weight: bold; color: #800000; font-family: Verdana; background-color: transparent;" id="Close">Close</a> -->
            </td>
	</tr>
	<tr>
		<td bgcolor="#06788B" align="left" style="height: 22px; width: 123px;">From date</td>
		<td bgcolor="#06788B" align="left" style="width: 147px; height: 22px;">
        
           <asp:TextBox ID="dcChkin" style="width:70%;" runat="server" class='FromDate'   
                     Cssclass="TextClass FromDate" ></asp:TextBox>
              <cc1:CalendarExtender id="CalendarExtender2" runat="server" PopupButtonID="imgbtn1" Format="dd/MM/yyyy" TargetControlID="dcChkin"></cc1:CalendarExtender><cc1:MaskedEditExtender id="MaskedEditExtender2" runat="server" TargetControlID="dcChkin" MaskType="Date" Mask="99/99/9999"></cc1:MaskedEditExtender>          
              <asp:ImageButton id="imgbtn1" runat="Server" ImageUrl="~/AgentsOnline/images/calendar.png" AlternateText="Click to show calendar"></asp:ImageButton>
                
                <font color="#FF0000">*</font>
           </td>
		<td bgcolor="#06788B" align="left" style="width: 110px; height: 22px;">To date</td>
		<td bgcolor="#06788B" align="left" style="width: 148px; height: 22px;">
         
                <asp:TextBox ID="dcChkout" style="width:70%;" runat="server" class='ToDate'  
                    Cssclass="TextClass ToDate" ></asp:TextBox>
              <cc1:CalendarExtender id="CalendarExtender1" runat="server" PopupButtonID="imgbtn2" Format="dd/MM/yyyy" TargetControlID="dcChkout"></cc1:CalendarExtender><cc1:MaskedEditExtender id="MaskedEditExtender1" runat="server" TargetControlID="dcChkout" MaskType="Date" Mask="99/99/9999"></cc1:MaskedEditExtender>
                <asp:ImageButton id="imgbtn2" runat="Server" ImageUrl="~/AgentsOnline/images/calendar.png" AlternateText="Click to show calendar"></asp:ImageButton>
            
            <font color="#FF0000">*
            </font></td>
		<td bgcolor="#06788B" align="left" style="height: 22px; width: 121px;">Message like</td>
		<td bgcolor="#06788B" align="left" style="height: 22px; width: 129px;">
            <input type="text" style="width: 92px" id="txtmsglike" class='MessageLike' name="T1"/>
                </td>
	</tr>
	<tr>
		<td height="22" bgcolor="#06788B" align="left" style="width: 123px">Chat user type</td>
		<td height="22" bgcolor="#06788B" align="left" style="width: 147px">
          
            <select  size="1" id="cmbtype" name="cmbtype" class='AgentType'>
			<option value="All" selected="selected">All</option>
			<option value="AGENT">AGENT</option>
			<option value="SUBAGENT">SUB AGENT</option>
		</select></td>
		<td height="22" bgcolor="#06788B" align="left" style="width: 110px">&nbsp;</td>
		<td height="22" bgcolor="#06788B" align="left" style="width: 148px">
            &nbsp;</td>
		<td height="22" bgcolor="#06788B" align="left" style="width: 121px">&nbsp;</td>
		<td height="22" bgcolor="#06788B" align="left" style="width: 129px">
            <input type="text" id="txtchatuser" class='ChatUserName' 
                style="width: 90px; visibility: hidden;" name="T1"/>
          </td>
	</tr>

	<tr>
		<td valign="top" height="22" bgcolor="#06788B" style="width: 123px">Customer chat 
            Sales person</td>
		<td height="22" bgcolor="#06788B" align="left" colspan="5" id="tdchatlist">
                                          <SELECT style="WIDTH: 382px" id="ddlUserName" class="field_input" 
                onchange="CallWebMethod('ddlUserName');" runat="server" name="D1"> <OPTION value="[Select]" selected>[Select]</OPTION></SELECT>
     </td>
	</tr>

	<tr>
		<td valign="top" height="22" bgcolor="#06788B" style="width: 123px">&nbsp;</td>
		<td height="22" bgcolor="#06788B" align="left" colspan="5" id="tdchatlist">
        
         
         <div class="ui-widget MyWidgetHotel">
        <asp:TextBox ID="TxtHotel" runat="server" Width = "250px" CssClass = "field_input MyAutoCompleteClass"></asp:TextBox>
         <asp:TextBox ID="TxtHotelValue" runat="server"  CssClass = "AutoAgentClassValue" Text=""></asp:TextBox>
	
</div> 
         
         </td>
	</tr>

	<tr>
		<td valign="top" height="22" bgcolor="#06788B" style="width: 123px">Customer</td>
		<td height="22" bgcolor="#06788B" align="left" colspan="5" id="tdchatlist">
            <SELECT style="WIDTH: 382px" id="ddlCustomerName"  class="drpdown MyDropDownListCustValue"
                onchange="CallWebMethod('ddlCustomerName');" runat="server" name="D1"> <OPTION value="[Select]" selected>[Select]</OPTION></SELECT></td>
                 
	</tr>
	<tr>
		<td height="22" bgcolor="#06788B" style="width: 123px"></td>
		<td width="610" height="22" bgcolor="#06788B" align="center" colspan="5">
        <input type="button" id="Button2" style="font-weight: bold" name="B3" value="View all details" onclick="OpenChatWindow('ListChatDetail.aspx')" />
        <asp:HiddenField ID="hdnagentcode" runat="server" />
        <asp:HiddenField ID="hdnuser" runat="server" />
        </td>
	</tr>
	<tr>
		<td valign="top" height="22" bgcolor="#06788B" style="width: 123px">&nbsp;</td>
		<td height="22" bgcolor="#06788B" align="left" colspan="5">
             
      </td>
	</tr>
	<tr>
		<td width="694" height="14" bgcolor="#DDD9CF" align="center" colspan="6">
        <span style="font-size: 9pt; font-weight: 700;">View current chat<br/>
            <br/>
            <input type="button" style="font-weight: bold; width: 92px" class="LiveChatShow" value="Live Chat" id="btnlivechat"  />
           <input type="button" style="font-weight: bold; width: 120px" class="CloseLiveChat" disabled="disabled" value="Close Live Chat" id="btncloselive"/>
        </span></td>

	</tr>
		
</tbody></table>

<div class='UserChatTypeclass' style='display:none'> 

<table class='ChatTableAddLive' style='border:1px solid #000;width:400px;'>


</table>

</div>


</asp:Content>

