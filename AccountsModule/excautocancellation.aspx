<%@ Page Language="VB" MasterPageFile="~/AccountsMaster.master" AutoEventWireup="false" CodeFile="excautocancellation.aspx.vb" Inherits="Accounts_excautocancellation" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
    <%@ Register Assembly="EclipseWebSolutions.DatePicker" Namespace="EclipseWebSolutions.DatePicker"   TagPrefix="ews" %>
    <%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>


    <%@ OutputCache location="none" %> 
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">
    <%--For Supplier Code End--%>
  <script src="../Content/js/jquery-1.5.1.min.js" type="text/javascript"></script>
<script src="../Content/js/JqueryUI.js" type="text/javascript"></script>
<script src="../Content/js/accounts.js" type="text/javascript"></script>
<link type ="text/css" href ="../Content/css/JqueryUI.css" rel ="Stylesheet" />

<script language="javascript" type="text/javascript" >

    function selectchk(btn) 
    {
        var btn1 = document.getElementById(btn);
        btn1.click()
        return;
    }
      
</script> 

    <asp:UpdatePanel id="UpdatePanel1" runat="server">
            <contenttemplate>
<table style="WIDTH: 100%">
<tbody>

<%--For Supplier Code Start--%>
<tr>
<td colspan="4">
<center>
    <asp:Label ID="lblHeading" runat="server" CssClass="field_heading" 
        ForeColor="White" Text="Mark Exclude Cancellation" Width="100%"></asp:Label></center>
</td>
</tr>
    <tr>
        <td class="td_cell" style="width: 140px">
            <asp:Label ID="Label12" runat="server" Text="File Number" Width="100px"></asp:Label>
        </td>
        <td style="WIDTH: 176px">
            <INPUT style="WIDTH: 142px" id="txtReqId" class="txtbox" tabIndex=3 type=text 
                maxLength=20 runat="server" visible="true" />
        </td>
        <td>
            &nbsp;</td>
        <td>
            &nbsp;</td>
    </tr>
<%--For Supplier Code End--%>

<%--For Country Code Start--%>
<%--For Country Code End--%>


<%--For City Code Start--%>
<%--For City Code End--%>


<%--For Category Code Start--%>
        
 <tr>      
<td class="td_cell" style="width: 140px">
    <asp:Label ID="Label21" runat="server" Text="From CheckIn Date"></asp:Label>
     </td>        
<td style="WIDTH: 176px">        
    <asp:TextBox ID="txtFromDate" runat="server" CssClass="field_input" 
        Width="80px"></asp:TextBox>
    <cc1:CalendarExtender ID="CEFromDate" runat="server" Format="dd/MM/yyyy" 
        PopupButtonID="ImgBtnFrmDt" TargetControlID="txtFromDate">
    </cc1:CalendarExtender>
    <cc1:MaskedEditExtender ID="MEFromDate" runat="server" Mask="99/99/9999" 
        MaskType="Date" TargetControlID="txtFromDate">
    </cc1:MaskedEditExtender>
    &nbsp;<asp:ImageButton ID="ImgBtnFrmDt" runat="server" 
        ImageUrl="~/Images/Calendar_scheduleHS.png" />
    &nbsp;<cc1:MaskedEditValidator ID="MEVFromDate" runat="server" 
        ControlExtender="MEFromDate" ControlToValidate="txtFromDate" 
        CssClass="field_error" Display="Dynamic" 
        EmptyValueBlurredText="Date is required" EmptyValueMessage="Date is required" 
        InvalidValueBlurredMessage="Invalid Date" InvalidValueMessage="Invalid Date" 
        TooltipMessage="Input a date in dd/mm/yyyy format">
    </cc1:MaskedEditValidator>
     </td>
<td align="right" class="td_cell">
    <asp:Label ID="Label22" runat="server" Text="To CheckIn Date"></asp:Label>
     </td>
<td>
    <asp:TextBox ID="txtToDate" runat="server" CssClass="fiel_input" Width="80px"></asp:TextBox>
    <cc1:CalendarExtender ID="CEToDate" runat="server" Format="dd/MM/yyyy" 
        PopupButtonID="ImgBtnRevDate" TargetControlID="txtToDate">
    </cc1:CalendarExtender>
    <cc1:MaskedEditExtender ID="METoDate" runat="server" Mask="99/99/9999" 
        MaskType="Date" TargetControlID="txtToDate">
    </cc1:MaskedEditExtender>
&nbsp;<asp:ImageButton ID="ImgBtnRevDate" runat="server" 
        ImageUrl="~/Images/Calendar_scheduleHS.png" />
    &nbsp;<cc1:MaskedEditValidator ID="MEVToDate" runat="server" 
        ControlExtender="METoDate" ControlToValidate="txtToDate" CssClass="field_error" 
        Display="Dynamic" EmptyValueBlurredText="Date is required" 
        EmptyValueMessage="Date is required" InvalidValueBlurredMessage="Invalid Date" 
        InvalidValueMessage="Invalid Date" 
        TooltipMessage="Input a date in dd/mm/yyyy format">
    </cc1:MaskedEditValidator>
     </td>
</tr>
<%--For Category Code End--%>
 
 
 <%--For Selling Code Start--%>

<%--For Selling Code End--%>
 
 <%--For Button  Start--%>
    <tr>
        <td class="td_cell" style="width: 140px">
            <asp:Label ID="Label24" runat="server" Text=" From Timelimit"></asp:Label>
        </td>
        <td style="WIDTH: 176px">
            <asp:TextBox ID="txtPfromdate" runat="server" CssClass="fiel_input" 
                Width="80px"></asp:TextBox>
            <cc1:CalendarExtender ID="CEPFromDate" runat="server" Format="dd/MM/yyyy" 
                PopupButtonID="ImgPBtnFrmDt" TargetControlID="txtPFromDate">
            </cc1:CalendarExtender>
            <cc1:MaskedEditExtender ID="MEPFromDate" runat="server" Mask="99/99/9999" 
                MaskType="Date" TargetControlID="txtPFromDate">
            </cc1:MaskedEditExtender>
            &nbsp;<asp:ImageButton ID="ImgPBtnFrmDt" runat="server" 
                ImageUrl="~/Images/Calendar_scheduleHS.png" />
            &nbsp;<cc1:MaskedEditValidator ID="MEVPFromDate" runat="server" 
                ControlExtender="MEPFromDate" ControlToValidate="txtPfromdate" 
                CssClass="field_error" Display="Dynamic" 
                EmptyValueBlurredText="Date is required" EmptyValueMessage="Date is required" 
                ErrorMessage="MEPFromDate" InvalidValueBlurredMessage="Invalid Date" 
                InvalidValueMessage="Invalid Date" 
                TooltipMessage="Input a date in dd/mm/yyyy format">
            </cc1:MaskedEditValidator>
        </td>
        <td align="right" class="td_cell">
            <asp:Label ID="Label25" runat="server" Text="To Timelimit"></asp:Label>
        </td>
        <td>
            <asp:TextBox ID="txtPtodate" runat="server" CssClass="fiel_input" Width="80px"></asp:TextBox>
            <cc1:CalendarExtender ID="CEPToDate" runat="server" Format="dd/MM/yyyy" 
                PopupButtonID="ImgPBtntoDt" TargetControlID="txtPToDate">
            </cc1:CalendarExtender>
            <cc1:MaskedEditExtender ID="MEPToDate" runat="server" Mask="99/99/9999" 
                MaskType="Date" TargetControlID="txtPToDate">
            </cc1:MaskedEditExtender>
            &nbsp;<asp:ImageButton ID="ImgPBtntoDt" runat="server" 
                ImageUrl="~/Images/Calendar_scheduleHS.png" />
            &nbsp;<cc1:MaskedEditValidator ID="MEVPToDate" runat="server" 
                ControlExtender="MEPToDate" ControlToValidate="txtPToDate" 
                CssClass="field_error" Display="Dynamic" 
                EmptyValueBlurredText="Date is required" EmptyValueMessage="Date is required" 
                ErrorMessage="MEPToDate" InvalidValueBlurredMessage="Invalid Date" 
                InvalidValueMessage="Invalid Date" 
                TooltipMessage="Input a date in dd/mm/yyyy format">
            </cc1:MaskedEditValidator>
        </td>
    </tr>
    <tr>
        <td class="td_cell" style="width: 140px">
            <asp:Label ID="Label15" runat="server" Text="Guest First Name" Width="100px"></asp:Label>
        </td>
        <td style="WIDTH: 176px">
            <INPUT style="WIDTH: 142px" id="txtGFname" class="txtbox" tabIndex=16 type=text maxLength=20 runat="server" visible="true" />
        </td>
        <td align="right" class="td_cell">
            <asp:Label ID="Label16" runat="server" Text="Guest Last Name" Width="100px"></asp:Label>
        </td>
        <td>
            <INPUT style="WIDTH: 142px" id="txtGLname" class="txtbox" tabIndex=17 type=text maxLength=20 runat="server" visible="true" />
        </td>
    </tr>
    <tr>
        <td class="td_cell" style="width: 140px">
            &nbsp;</td>
        <td style="WIDTH: 176px">
            &nbsp;</td>
        <td align="right" class="td_cell">
            <asp:Label ID="Label23" runat="server" Text="Customers" Width="100px"></asp:Label>
        </td>
        <td>
            <input id="accSearch" runat="server" 
                                                      
                class="field_input MyAutoCompleteClass" name="accSearch" 
                                                      
                onfocus="MyAutoCustomer_rptFillArray();" style="width:98% ; font " 
                                                      type="text" />
        </td>
    </tr>
<tr>
<td style="TEXT-ALIGN: center" colspan="4">
 <asp:Button id="Btndisplay" tabindex="29" runat="server" Text="Display" CssClass="btn"></asp:Button>&nbsp;
 </td>
  </tr>
  <%--For Button End --%>
 </tbody>
  </table>
  <asp:UpdatePanel id="UpdatePanel3" runat="server" >

  <ContentTemplate>
 
   <asp:Panel ID="pnlparty" runat="server" BackColor="Transparent" 
                BorderStyle="None" Height="225px" ScrollBars="Auto" Width="900px">
  <table>
  
  <tbody>

   <tr>
        <td align="left" class="td_cell" style="WIDTH: 1172px; ">
      
                <asp:GridView ID="grdParty" runat="server" AutoGenerateColumns="False" 
                    BackColor="White" BorderColor="#999999" BorderStyle="None" BorderWidth="1px" 
                    CaptionAlign="Top" CellPadding="3" CssClass="td_cell" Font-Size="10px" 
                    GridLines="Vertical" tabIndex="19" UseAccessibleHeader="False">
                    <FooterStyle CssClass="grdfooter" />
                    <Columns>
                        <asp:TemplateField HeaderText="Select">
                            <ItemStyle HorizontalAlign="Center" Width="60px" VerticalAlign="Top" />
                            <HeaderStyle HorizontalAlign="Center" Width="60px" />
                            <ItemTemplate>
                                <asp:CheckBox ID="chkSelect" runat="server"  Checked='<%# Bind("Printyn") %>' />
                                <asp:HiddenField ID="hdnCustcode" runat="server"  Value ='<%# Bind("agentcode") %>' />
                                <asp:CheckBox ID="chkselet1" runat="server" Checked='<%# Bind("Printyn") %>'  style="visibility:hidden" />
                                <asp:Button ID="btn" runat="server" Text="Button" onclick="btn_Click"  style="Visibility:hidden"/>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="requestid" HeaderText="Fileno">
                            <ItemStyle Width="125px" VerticalAlign="Top" />
                            <HeaderStyle Width="125px" />
                        </asp:BoundField>
                          <asp:BoundField DataField="datein" HeaderText="Arriva Date">
                            <ItemStyle Width="100px" VerticalAlign="Top" />
                            <HeaderStyle Width="100px" />
                         </asp:BoundField>
                        <asp:BoundField DataField="agentname" HeaderText="Agent Name">
                              <ItemStyle Width="350px" VerticalAlign="Top" />
                            <HeaderStyle Width="350px" />
                        </asp:BoundField>
                    <asp:BoundField DataField="salevalue" HeaderText="Salevalue">
                        <ItemStyle Width="75px" VerticalAlign="Top" />
                        <HeaderStyle Width="75px" />
                    </asp:BoundField>
                      <asp:BoundField DataField="guestname" HeaderText="Guest name">
                        <ItemStyle Width="400px" VerticalAlign="Top" />
                        <HeaderStyle Width="400px" />
                    </asp:BoundField>

                    </Columns>
                    <RowStyle CssClass="grdRowstyle" />
                    <SelectedRowStyle CssClass="grdselectrowstyle" />
                    <PagerStyle CssClass="grdpagerstyle" HorizontalAlign="Center" />
                    <HeaderStyle CssClass="grdheader" />
                    <AlternatingRowStyle CssClass="grdAternaterow" />
                </asp:GridView>
                <br />
                &nbsp;
        
        </td>
    </tr>

    </tbody>

    </table>
   </asp:Panel>
    </ContentTemplate>
     </asp:UpdatePanel>
    <table>
    <tbody>
     <tr>
        <td align="left" class="td_cell" style="WIDTH: 1172px; ">
            <asp:Button ID="btnSelectAll" runat="server" CssClass="btn" 
                onclick="btnSelectAll_Click" tabIndex="20" Text="Select All" />
            &nbsp;
            <asp:Button ID="btnUnselectAll" runat="server" CssClass="btn" 
                onclick="btnUnselectAll_Click" tabIndex="21" Text="Unselect All" />
        </td>
        <tr>
      
          <td align="center" class="td_cell" style="WIDTH: 1172px; ">
 <asp:Button ID="btnSave" runat="server" CssClass="btn" tabIndex="50" Text="Save"  /> 
              <asp:HiddenField ID="hdncustcode" runat="server" />
            </td>

 </tr>
     </tr>
    </tbody>
    
    </table>
 
</contenttemplate>
        <triggers>
<%--<asp:PostBackTrigger ControlID="BtnSaveInfoWeb"></asp:PostBackTrigger>--%>
</triggers>
    </asp:UpdatePanel>
    &nbsp; &nbsp;
    <asp:ScriptManagerProxy id="ScriptManagerProxy1" runat="server">
        <services>
<asp:ServiceReference Path="~/clsServices.asmx"></asp:ServiceReference>
</services>
    </asp:ScriptManagerProxy>
</asp:Content>

