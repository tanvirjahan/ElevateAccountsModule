<%@ Page Language="VB" MasterPageFile="~/SubPageMaster.master" AutoEventWireup="false" CodeFile="SupResv.aspx.vb" Inherits="SupResv"  %>

<%@ Register Src="SubMenuUserControl.ascx" TagName="SubMenuUserControl" TagPrefix="uc1" %>
<%@ Register Src="wchotelproducts.ascx" TagName="hoteltab" TagPrefix="whc" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
    <%@ OutputCache location="none" %> 
<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">
<script language="javascript"   type ="text/javascript" >

    function CallWebMethod(methodType) {
        switch (methodType) {
            case "partycode":
                var select = document.getElementById("<%=ddlSuppierCD.ClientID%>");
                var selectname = document.getElementById("<%=ddlSuppierNM.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;
                break;
            case "partyname":
                var select = document.getElementById("<%=ddlSuppierNM.ClientID%>");
                var selectname = document.getElementById("<%=ddlSuppierCD.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;
                break;
        }
    }
</script>
 <style>
 .bgrow
 {
 background-color:white; 
 height:30px;
 margin-left:none;
 }

 </style>


    <asp:UpdatePanel id="UpdatePanel1" runat="server">
        <Triggers>
            <asp:PostBackTrigger ControlID="BtnResSave" /> 
        </Triggers>

    
        <ContentTemplate>
<table style="" align="left">
<tbody>
    <tr>
    <td align ="left">
                  
        <whc:hoteltab ID="whotelatbcontrol" runat="server" />
               
                
    </td>
    </tr>
    <tr>
    <td>
    <div style="margin-top:-6px;margin-left:13px;">
     <table style="BORDER-RIGHT: gray 2px solid; BORDER-TOP: gray 2px solid; BORDER-LEFT: gray 2px solid; BORDER-BOTTOM: gray 2px solid;" class="td_cell" align=left>
     <tr>
<td vAlign=top align=center width=150 colSpan=2><asp:Label id="lblHeading" runat="server" Text="Supplier " CssClass="field_heading" Width="100%" ForeColor="White"></asp:Label>
</td></tr>
<tr><td vAlign=top align=left width=150>Code <span style="color: #ff0000" class="td_cell">*</span></td>
<td class="td_cell" vAlign=top align=left><input style="width: 196px" id="txtCode" class="field_input" tabIndex=1 type=text maxLength=20 runat="server" /> &nbsp; Name&nbsp; <input style="width: 213px" id="txtName" class="field_input" tabIndex=4 type=text maxLength=100 runat="server" /></td>
</tr>

 <tr>
        <td align="left" valign="top" width="150">
            &nbsp;</td>
        <td align="left" class="td_cell" valign="top">
            <table style="width: 100%">
                <tr>
                    <td>
                        Supplier</td>
                    <td>
                        <select id="ddlSuppierCD" runat="server" class="field_input" name="D1" 
                            onchange="CallWebMethod('partycode');" style="WIDTH: 220px">
                            <option selected=""></option>
                        </select>
                    </td>
                    <td>
                        <select id="ddlSuppierNM" runat="server" class="field_input" name="D2" 
                            onchange="CallWebMethod('partyname');" style="WIDTH: 310px">
                            <option selected=""></option>
                        </select>
                    </td>
                    <td>
                        <asp:Button ID="btnfilldetail" runat="server" CssClass="field_button" 
                            Text="Fill Details" />
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr><td vAlign=top align=left width=150><uc1:SubMenuUserControl id="SubMenuUserControl1" runat="server"></uc1:SubMenuUserControl> </td><td style="WIDTH: 100px" vAlign=top><DIV style="WIDTH: 824px; HEIGHT: 450px" id="iframeINF" runat="server"><asp:UpdatePanel id="UpdatePanel2" runat="server"><ContentTemplate>
<asp:Panel id="PanelReservation" runat="server" Visible="False" Width="743px" GroupingText="Reservation Details"><TABLE style="WIDTH: 784px" align=center><TBODY><tr><td style="WIDTH: 126px" align=left>Address <SPAN style="COLOR: #ff0000" class="td_cell">*</SPAN></td><td style="WIDTH: 329px" align=left><INPUT style="WIDTH: 297px" id="txtResAddress1" class="field_input" tabIndex=28 type=text maxLength=100 runat="server" /></td>
 <td style="WIDTH: 13px" align=left></td></tr><tr><td style="WIDTH: 126px; HEIGHT: 24px" align=left></td><td style="WIDTH: 329px; HEIGHT: 24px" align=left><INPUT style="WIDTH: 297px" id="txtResAddress2" class="field_input" tabIndex=29 type=text maxLength=100 runat="server" /></td><td style="WIDTH: 274px; HEIGHT: 24px" align=left>
      
                  <td style="WIDTH: 13px" align=left></td></tr><tr><td style="WIDTH: 126px" align=left></td><td style="WIDTH: 329px" align=left><INPUT style="WIDTH: 297px" id="txtResAddress3" class="field_input" tabIndex=30 type=text maxLength=100 runat="server" /></td>
                    <td style="WIDTH: 274px;display:none" align=left>Auto Email &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;&nbsp; 
    <asp:DropDownList id="ddlAutoEmail" tabIndex=43 runat="server" 
        CssClass="field_input" Width="100px">
                        <asp:ListItem>[Select]</asp:ListItem>
                        <asp:ListItem Selected="True">Yes</asp:ListItem>
                        <asp:ListItem>No</asp:ListItem>
                    </asp:DropDownList></td><td style="WIDTH: 13px" align=left></td></tr><tr><td style="WIDTH: 126px" align=left>Telephone <SPAN style="COLOR: #ff0000" class="td_cell">*</SPAN></td><td style="WIDTH: 329px" align=left><INPUT style="WIDTH: 297px" id="txtResPhone1" class="field_input" tabIndex=31 type=text maxLength=50 runat="server" /></td><td style="WIDTH: 274px" align=left><INPUT id="chkprintpricesinrequest" type=checkbox runat="server" />&nbsp;Print Price In Request</td><td style="WIDTH: 13px" align=left></td></tr><tr><td style="WIDTH: 126px" align=left></td><td style="WIDTH: 329px" align=left><INPUT style="WIDTH: 297px" id="txtResPhone2" class="field_input" tabIndex=32 type=text maxLength=50 runat="server" /></td><td style="WIDTH: 274px" align=left></td><td style="WIDTH: 13px" align=left></td></tr><tr><td style="WIDTH: 126px" align=left>
    Mobile No.</td><td style="WIDTH: 329px" align=left><INPUT style="WIDTH: 297px" id="txtResMob" class="field_input" tabIndex=32 type=text maxLength=50 runat="server" /></td><td style="WIDTH: 274px" align=left></td><td style="WIDTH: 13px" align=left></td></tr><tr><td style="WIDTH: 126px" align=left>Fax </td><td style="WIDTH: 329px" align=left><INPUT style="WIDTH: 297px" id="txtResFax" class="field_input" tabIndex=33 type=text maxLength=50 runat="server" /></td><td style="WIDTH: 274px" align=left></td><td style="WIDTH: 13px" align=left></td></tr><tr>
    <td style="WIDTH: 126px" align=left>Contact
    </td>
    <td style="WIDTH: 329px" align=left><INPUT style="WIDTH: 297px" id="txtResContact1" class="field_input" tabIndex=34 type=text maxLength=100 runat="server" /></td>

    <td style="WIDTH: 13px" align=left></td></tr><tr><td style="WIDTH: 126px" align=left></td><td style="WIDTH: 329px" align=left><INPUT style="WIDTH: 297px" id="txtResContact2" class="field_input" tabIndex=35 type=text maxLength=100 runat="server" /></td>
 
 <td style="WIDTH: 13px" align=left></td></tr><tr><td style="WIDTH: 126px" align=left>E-mail <SPAN style="COLOR: #ff0000" class="td_cell">*</SPAN></td><td style="WIDTH: 329px" align=left><INPUT style="WIDTH: 297px" id="txtResEmail" class="field_input" tabIndex=36 type=text maxLength=100 runat="server" /></td> 
 
 <td style="WIDTH: 13px" align=left></td></tr><tr><td style="WIDTH: 126px" align=left>Web Site</td><td style="WIDTH: 329px" align=left><INPUT style="WIDTH: 297px" id="txtResWebSite" class="field_input" tabIndex=37 type=text maxLength=100 runat="server" /></td>

 </tr><tr><td>Cancellation Policy</td><td style="WIDTH: 274px; HEIGHT: 15px" align=left><asp:DropDownList id="ddlWO2from" tabIndex=10 runat="server" CssClass="field_input" Width="126px"><asp:ListItem>[Select]</asp:ListItem>
<asp:ListItem Value="Highest Season">Highest Season</asp:ListItem>
<asp:ListItem Value="Lowest Season">Lowest Season</asp:ListItem>
<asp:ListItem Value="Maximum Seasonality">Maximum Seasonality</asp:ListItem>
</asp:DropDownList></td></tr>
        
<tr><td style="WIDTH: 126px" align=left>
    <asp:Button id="BtnResSave" tabIndex=50 onclick="BtnResSave_Click" 
        runat="server" Text="Save" CssClass="field_button"></asp:Button></td><td style="WIDTH: 230px" align=left><asp:Button id="BtnResCancel" tabIndex=51 onclick="BtnResCancel_Click" runat="server" Text="Return to Search" CssClass="field_button"></asp:Button>&nbsp; <asp:Button id="btnhelp" tabIndex=52 onclick="btnhelp_Click" runat="server" Text="Help" CssClass="field_button"></asp:Button></td><td style="WIDTH: 274px" align=left></td><td style="WIDTH: 13px" align=left></td></tr></TBODY></TABLE></asp:Panel> 
</ContentTemplate>
</asp:UpdatePanel> </DIV></td></tr>
     </table>
    </div>
    </td>
    </tr>

   

</TBODY></TABLE>
</ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

