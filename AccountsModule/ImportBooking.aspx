<%@ Page Title="" Language="VB" MasterPageFile="~/SubPageMaster.master" AutoEventWireup="false" CodeFile="ImportBooking.aspx.vb" Inherits="ImportBooking" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ OutputCache Location="none" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">
<%--<meta name="viewport" content="width=device-width, initial-scale=1.0">
<link href="../css/progressbar1.css" rel="stylesheet" type="text/css" />--%>

<style type="text/css">
    

</style>

<script type="text/javascript">

    function DateSelectCalExt() {
        var txtfromDate = document.getElementById("<%=txtChkFromDt.ClientID%>");
        if (txtfromDate.value != '') {
            var calendarBehavior1 = $find("<%=ChkFromDt_CalendarExtender.ClientID %>");
            var date = calendarBehavior1._selectedDate;

            var dp = txtfromDate.value.split("/");
            var newDt = new Date(dp[2] + "/" + dp[1] + "/" + dp[0]);
            newDt = getFormatedDate(newDt);
            newDt = new Date(newDt);
            calendarBehavior1.set_selectedDate(newDt);
        }
        var txtfromDate2 = document.getElementById("<%=txtChkToDt.ClientID%>");
        if (txtfromDate2.value != '') {
            var calendarBehavior2 = $find("<%=txtChkToDt_CalendarExtender.ClientID %>");
            var date2 = calendarBehavior2._selectedDate;

            var dp2 = txtfromDate2.value.split("/");
            var newDt2 = new Date(dp2[2] + "/" + dp2[1] + "/" + dp2[0]);
            newDt2 = getFormatedDate(newDt2);
            newDt2 = new Date(newDt2);
            calendarBehavior2.set_selectedDate(newDt2);
        }

    }
    function getFormatedDate(chkdate) {
        var dd = chkdate.getDate();
        var mm = chkdate.getMonth() + 1; //January is 0!
        var yyyy = chkdate.getFullYear();
        if (dd < 10) { dd = '0' + dd };
        if (mm < 10) { mm = '0' + mm };
        chkdate = mm + '/' + dd + '/' + yyyy;
        return chkdate;
    }

    function filltodate(fDate) {
        var txtfromDate = document.getElementById("<%=txtChkFromDt.ClientID%>");
        var txtToDate = document.getElementById("<%=txtChkToDt.ClientID%>");
        txtToDate.value = txtfromDate.value;
        return;

        if ((txtToDate.value != null) && (txtToDate.value != '')) {

            var dpFrom = txtfromDate.value.split("/");
            var newDt = new Date(dpFrom[2] + "/" + dpFrom[1] + "/" + dpFrom[0]);
            newDt = getFormatedDate(newDt);
            newDt = new Date(newDt);
            var dpTo = txtToDate.value.split("/");
            var newDtTo = new Date(dpTo[2] + "/" + dpTo[1] + "/" + dpTo[0]);
            newDtTo = getFormatedDate(newDtTo);
            newDtTo = new Date(newDtTo);
            if (newDt > newDtTo) {

                txtToDate.value = txtfromDate.value;
                DateSelectCalExt();
                return;
            }
        }
    }

    function ValidateDate() {

        var txtfromDate = document.getElementById("<%=txtChkFromDt.ClientID%>");
        var txtToDate = document.getElementById("<%=txtChkToDt.ClientID%>");
        if (txtfromDate.value == null || txtfromDate.value == "") {
            txtToDate.value = "";
            alert("Please select From date.");
        }

        var dp = txtfromDate.value.split("/");
        var newChkInDt = new Date(dp[2] + "/" + dp[1] + "/" + dp[0]);

        var dp1 = txtToDate.value.split("/");
        var newChkOutDt = new Date(dp1[2] + "/" + dp1[1] + "/" + dp1[0]);

        newChkInDt = getFormatedDate(newChkInDt);
        newChkOutDt = getFormatedDate(newChkOutDt);

        newChkInDt = new Date(newChkInDt);
        newChkOutDt = new Date(newChkOutDt);
        if (newChkInDt > newChkOutDt) {
            txtToDate.value = txtfromDate.value;
            alert("To date should not be greater than From date");
        }
    }
    

    function showProgress() {
        var ModalPopupLoading = $find("ModalPopupLoading");
        ModalPopupLoading.show();
        return true;
    }

    function getFileName(fileloader) {
        var lblFileName = document.getElementById("<%=lblFileName.ClientID%>");
        
        if (fileloader.value != "") {
            var strfile = fileloader.value.split("\\");
            var filename = strfile[strfile.length - 1];
            lblFileName.innerHTML = filename;    
        }
        else {
            lblFileName.innerHTML = "No File Chosen";
        }
    }
</script>

 <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div style="height:100%">
                <table>
                        <tr>
                            <td style="width: 100%" align="center" colspan="8">
                                <asp:Label ID="lblHeading" runat="server" Text="Import Bookings" CssClass="field_heading"
                                    Width="100%"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="width:12%">
                                <label id="lblFileUpload" runat="server" class="field_caption" style="vertical-align: top">
                                    Select File To Import <span style="color: red" class="td_cell">&nbsp;*</span></label>
                            </td>
                            <td style="width: 25%">
                                <asp:FileUpload ID="FileUploadExcel" runat="server" ViewStateMode="Enabled" TabIndex="1"
                                    onchange="getFileName(this);" Style="color: transparent; width: 90px" />
                                <asp:Label ID="lblFilePath" runat="server" Style="display: none"></asp:Label>
                                <asp:Label ID="lblFileName" runat="server"></asp:Label>
                            </td>
                            <td style="width:10%"> <asp:Button ID="btnImportExcel" runat="server" CssClass="field_button" 
                                    OnClientClick="return showProgress();" TabIndex="3" Text="Import Excel" />
                                &nbsp;</td> 
                            <td style="width:9%">
                            <label class="field_caption">
                                       From Arrival Date</label><span style="color: red" class="td_cell">&nbsp;*</span>
                            </td>
                            <td style="width: 10%">
                                <asp:TextBox ID="txtChkFromDt" CssClass="field_input" runat="server" TabIndex="1"
                                    onchange="filltodate(this);" Width="75px"></asp:TextBox>
                                <asp:CalendarExtender ID="ChkFromDt_CalendarExtender" runat="server" Format="dd/MM/yyyy"
                                    OnClientDateSelectionChanged="DateSelectCalExt" PopupButtonID="ImgBtnChkFromDt"
                                    PopupPosition="Right" TargetControlID="txtChkFromDt"></asp:CalendarExtender>
                                <asp:MaskedEditExtender ID="ChkFromDt_MaskedEditExtender" runat="server" Mask="99/99/9999"
                                    MaskType="Date" TargetControlID="txtChkFromDt"></asp:MaskedEditExtender>
                                <asp:ImageButton ID="ImgBtnChkFromDt" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.png"
                                    TabIndex="-1" /><br />
                                <asp:MaskedEditValidator ID="MevChkFromDt" runat="server" ControlExtender="ChkFromDt_MaskedEditExtender"
                                    ControlToValidate="txtChkFromDt" CssClass="field_error" Display="Dynamic" EmptyValueBlurredText="Date is required"
                                    EmptyValueMessage="Date is required" ErrorMessage="ChkFromDt_MaskedEditExtender"
                                    InvalidValueBlurredMessage="Invalid Date" InvalidValueMessage="Invalid Date"
                                    TooltipMessage="Date/Month/Year">
                                </asp:MaskedEditValidator>
                            </td>
                            <td style="width:9%">
                            <label class="field_caption">
                                       To Arrival Date</label><span style="color: red" class="td_cell">&nbsp;*</span>
                            </td>
                            <td style="width: 10%">
                                <asp:TextBox ID="txtChkToDt" CssClass="field_input" onchange="ValidateDate();" runat="server"
                                    TabIndex="2" Width="75px"></asp:TextBox>
                                <asp:CalendarExtender ID="txtChkToDt_CalendarExtender" runat="server" Format="dd/MM/yyyy"
                                    OnClientDateSelectionChanged="DateSelectCalExt" PopupButtonID="ImgBtnChkToDt"
                                    PopupPosition="Right" TargetControlID="txtChkToDt"></asp:CalendarExtender>
                                <asp:MaskedEditExtender ID="ChkToDt_MaskedEditExtender" runat="server" Mask="99/99/9999"
                                    MaskType="Date" TargetControlID="txtChkToDt"></asp:MaskedEditExtender>
                                <asp:ImageButton ID="imgBtnChkToDt" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.png"
                                    TabIndex="-1" /><br />
                                <asp:MaskedEditValidator ID="MevChkToDt" runat="server" ControlExtender="ChkToDt_MaskedEditExtender"
                                    ControlToValidate="txtChkToDt" CssClass="field_error" Display="Dynamic" EmptyValueBlurredText="Date is required"
                                    EmptyValueMessage="Date is required" ErrorMessage="ChkToDt_MaskedEditExtender"
                                    InvalidValueBlurredMessage="Invalid Date" InvalidValueMessage="Invalid Date"
                                    TooltipMessage="Date/Month/Year">
                                </asp:MaskedEditValidator>
                            </td>                                                       
                            <td style="width:15%">
                             <label id="lblID" runat="server" class="field_caption" style="vertical-align: top">ID</label>
                                <asp:TextBox ID="txtID" runat="server" Enabled="false" ></asp:TextBox>
                                <asp:TextBox ID="txtCount" runat="server" style="display:none"></asp:TextBox>
                                <asp:TextBox ID="txtDecno" runat="server" style="display:none"></asp:TextBox>
                            </td>
                            
                        </tr>
                        <tr>
                            <td class="td_cell" colspan="2">
                               <asp:CheckBox ID="chkIgnoreCancel" runat="server" Checked="true" /> Ignore Cancellation
                                
                            </td>
                            <td colspan="4"></td>
                        </tr>
                        <tr>
                            <td colspan="6">
                            <asp:Label ID="lblNotemapping" runat="server" CssClass="field_caption" style="background-color:#e3e17f;" Text="Note : Rows where Agent or Supplier is not mapped has been highlighted. This file can be imported only after mapping has been completed."></asp:Label> <br />
                            <asp:Label ID="lblNotemissing" runat="server" CssClass="field_caption" style="background-color:#00FFFF;" Text="Note : Rows where Mandatory fields missing  has been highlighted. This file can be imported only with these fields."></asp:Label>                                                                                                     
                            </td>
                            <td align="center" colspan="2" style="display:none">

                           

                            <asp:Button ID="btnRefresh" runat="server" CssClass="field_button" Text="Refresh Mapping"
                                    TabIndex="3" OnClientClick="return showProgress();" style="display:none" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="8">
                                <asp:TabContainer ID="TabHotelInfo" runat="server" ActiveTabIndex="0">
                                    <asp:TabPanel ID="panHotelInfo" runat="server" HeaderText="New Bookings">
                                        <ContentTemplate>
                                            <div id="divscrollBooking" style="height: 70vh; width: 95vw; padding: 4px; overflow: scroll">
                                                <asp:GridView ID="gvBookingDetails" runat="server" AutoGenerateColumns="False" BackColor="White"
                                                    BorderColor="#999999" CssClass="td_cell" ShowHeaderWhenEmpty="true" Width="99%"
                                                    TabIndex="4">
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="">
                                                            <ItemTemplate>               
                                                               <asp:CheckBox ID="chkSelect" runat="server" Enabled="false" />                                              
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Booking Code">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblBookingNo" runat="server" Text='<%# Bind("Bookingcode") %>'></asp:Label>
                                                                <asp:TextBox ID="txtMapAgent" runat="server" Text='<%# Bind("Mapagentcode") %>' Style="display: none"></asp:TextBox>
                                                                <asp:TextBox ID="txtMapparty" runat="server" Text='<%# Bind("Mappartycode") %>' Style="display: none"></asp:TextBox>
                                                                <asp:TextBox ID="txtMapsalecurr" runat="server" Text='<%# Bind("Mapsalecurr") %>'
                                                                    Style="display: none"></asp:TextBox>
                                                                <asp:TextBox ID="txtUniqueId" runat="server" Text='<%# Bind("UniqueId") %>' Style="display: none"></asp:TextBox>
                                                                <asp:TextBox ID="txtMapAgentCtry" runat="server" Text='<%# Bind("Mapctryagent") %>'
                                                                    Style="display: none"></asp:TextBox>
                                                                <asp:TextBox ID="txtCheckHotelSupplier" runat="server" Text='<%# Bind("checkHotelSupplier") %>'
                                                                    Style="display: none"></asp:TextBox>
                                                                <asp:TextBox ID="txtIgnore" runat="server" Text='<%# Bind("checkIgnore") %>'
                                                                    Style="display: none"></asp:TextBox>
                                                                <asp:TextBox ID="txtMapService" runat="server" Text='<%# Bind("mapService") %>'
                                                                    Style="display: none"></asp:TextBox>
                                                                <asp:TextBox ID="txtPaxNumber" runat="server" Text='<%# Bind("PaxNumber") %>'
                                                                    Style="display: none"></asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Start date">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblstartdate" CssClass="field_input" Text='<%# Bind("Startdate") %>'
                                                                    runat="server"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="End date">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblenddate" CssClass="field_input" Text='<%# Bind("Enddate") %>' runat="server"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Agency">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblAgency" runat="server" Text='<%# Bind("Agency") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Agency booking reference">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblAgentBookingRef" runat="server" Text='<%# Bind("Agencybookingref") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Main passenger name">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblMainpssgrname" runat="server" Text='<%# Bind("Mainpssgrname") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Description">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lbldescription" runat="server" Text='<%# Bind("Description") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Product group">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblprodgroup" runat="server" Text='<%# Bind("Prodgrp") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Sales currency">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblsalescurr" runat="server" Text='<%# Bind("Salescurr") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Sales Price">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblsalesprice" runat="server" Text='<%# Bind("SalesPrice") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Supplier name">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblsuppname" runat="server" Text='<%# Bind("Suppname") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Cost currency">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblcostcurr" runat="server" Text='<%# Bind("Costcurr") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Cost Price">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblCostprice" runat="server" Text='<%# Bind("Costprice") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Date">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblDate" runat="server" Text='<%# Bind("Date") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Line booking date">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lbllinebookdate" runat="server" Text='<%# Bind("Linebookingdate") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Supplier ID">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblsuppid" runat="server" Text='<%# Bind("SuppID") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Client ID">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblClientId" runat="server" Text='<%# Bind("ClientID") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Country of the Agency">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblAgencyCtry" runat="server" Text='<%# Bind("CtryAgency") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Supplier reference Number" Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblsupprefno" runat="server" Text='<%# Bind("SupprefNum") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Nationality" Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblNationality" runat="server" Text='<%# Bind("Nationality") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Exchange Rate" Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblexchangerate" runat="server" Text='<%# Bind("ExchRate") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="No. of Rooms" Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblNoofRooms" runat="server" Text='<%# Bind("NoofRooms") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Booking Element ID">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblBookElementId" runat="server" Text='<%# Bind("bookingElementId") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <RowStyle CssClass="grdRowstyle" BackColor="#EEEEEE" ForeColor="Black"></RowStyle>
                                                    <SelectedRowStyle CssClass="grdselectrowstyle" BackColor="#008A8C" ForeColor="White"
                                                        Font-Bold="True"></SelectedRowStyle>
                                                    <PagerStyle CssClass="grdpagerstyle" BackColor="#999999" ForeColor="Black" HorizontalAlign="Center">
                                                    </PagerStyle>
                                                    <HeaderStyle CssClass="grdheader" BackColor="#06788B" ForeColor="White" Font-Bold="True"
                                                        HorizontalAlign="Center" VerticalAlign="Middle" BorderColor="LightGray"></HeaderStyle>
                                                    <AlternatingRowStyle CssClass="grdAternaterow" BackColor="Transparent" Font-Size="12px">
                                                    </AlternatingRowStyle>
                                                </asp:GridView>                                               
                                            </div>
                                        </ContentTemplate>
                                    </asp:TabPanel>
                                    <asp:TabPanel ID="panAmendInfo" runat="server" HeaderText="Amended Bookings" TabIndex="56">
                                        <ContentTemplate>
                                            <div id="divAmendBooking" style="height: 70vh; width: 95vw; padding: 4px; overflow: scroll">
                                            <asp:GridView ID="gvAmendBooking" runat="server" AutoGenerateColumns="False" BackColor="White"
                                                BorderColor="#999999" CssClass="td_cell" ShowHeaderWhenEmpty="true" Width="99%"
                                                TabIndex="4">
                                                  <Columns>
                                                        <asp:TemplateField HeaderText="">
                                                            <ItemTemplate>               
                                                               <asp:CheckBox ID="chkSelect" runat="server" Enabled="false" />                                              
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Booking Code">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblBookingNo" runat="server" Text='<%# Bind("Bookingcode") %>'></asp:Label>
                                                                <asp:TextBox ID="txtMapAgent" runat="server" Text='<%# Bind("Mapagentcode") %>' Style="display: none"></asp:TextBox>
                                                                <asp:TextBox ID="txtMapparty" runat="server" Text='<%# Bind("Mappartycode") %>' Style="display: none"></asp:TextBox>
                                                                <asp:TextBox ID="txtMapsalecurr" runat="server" Text='<%# Bind("Mapsalecurr") %>'
                                                                    Style="display: none"></asp:TextBox>
                                                                <asp:TextBox ID="txtUniqueId" runat="server" Text='<%# Bind("UniqueId") %>' Style="display: none"></asp:TextBox>
                                                                <asp:TextBox ID="txtMapAgentCtry" runat="server" Text='<%# Bind("Mapctryagent") %>'
                                                                    Style="display: none"></asp:TextBox>
                                                                <asp:TextBox ID="txtCheckHotelSupplier" runat="server" Text='<%# Bind("checkHotelSupplier") %>'
                                                                    Style="display: none"></asp:TextBox>
                                                                <asp:TextBox ID="txtIgnore" runat="server" Text='<%# Bind("checkIgnore") %>'
                                                                    Style="display: none"></asp:TextBox>
                                                                <asp:TextBox ID="txtMapService" runat="server" Text='<%# Bind("mapService") %>'
                                                                    Style="display: none"></asp:TextBox>
                                                                <asp:TextBox ID="txtPaxNumber" runat="server" Text='<%# Bind("PaxNumber") %>'
                                                                    Style="display: none"></asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Start date">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblstartdate" CssClass="field_input" Text='<%# Bind("Startdate") %>'
                                                                    runat="server"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="End date">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblenddate" CssClass="field_input" Text='<%# Bind("Enddate") %>' runat="server"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Agency">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblAgency" runat="server" Text='<%# Bind("Agency") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Agency booking reference">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblAgentBookingRef" runat="server" Text='<%# Bind("Agencybookingref") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Main passenger name">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblMainpssgrname" runat="server" Text='<%# Bind("Mainpssgrname") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Description">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lbldescription" runat="server" Text='<%# Bind("Description") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Product group">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblprodgroup" runat="server" Text='<%# Bind("Prodgrp") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Sales currency">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblsalescurr" runat="server" Text='<%# Bind("Salescurr") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Sales Price">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblsalesprice" runat="server" Text='<%# Bind("SalesPrice") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Supplier name">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblsuppname" runat="server" Text='<%# Bind("Suppname") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Cost currency">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblcostcurr" runat="server" Text='<%# Bind("Costcurr") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Cost Price">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblCostprice" runat="server" Text='<%# Bind("Costprice") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Date">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblDate" runat="server" Text='<%# Bind("Date") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Line booking date">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lbllinebookdate" runat="server" Text='<%# Bind("Linebookingdate") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Supplier ID">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblsuppid" runat="server" Text='<%# Bind("SuppID") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Client ID">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblClientId" runat="server" Text='<%# Bind("ClientID") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Country of the Agency">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblAgencyCtry" runat="server" Text='<%# Bind("CtryAgency") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Supplier reference Number" Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblsupprefno" runat="server" Text='<%# Bind("SupprefNum") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Nationality" Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblNationality" runat="server" Text='<%# Bind("Nationality") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Exchange Rate" Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblexchangerate" runat="server" Text='<%# Bind("ExchRate") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="No. of Rooms" Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblNoofRooms" runat="server" Text='<%# Bind("NoofRooms") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Booking Element ID">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblBookElementId" runat="server" Text='<%# Bind("bookingElementId") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                <RowStyle CssClass="grdRowstyle" BackColor="#EEEEEE" ForeColor="Black"></RowStyle>
                                                <SelectedRowStyle CssClass="grdselectrowstyle" BackColor="#008A8C" ForeColor="White"
                                                    Font-Bold="True"></SelectedRowStyle>
                                                <PagerStyle CssClass="grdpagerstyle" BackColor="#999999" ForeColor="Black" HorizontalAlign="Center">
                                                </PagerStyle>
                                                <HeaderStyle CssClass="grdheader" BackColor="#06788B" ForeColor="White" Font-Bold="True"
                                                    HorizontalAlign="Center" VerticalAlign="Middle" BorderColor="LightGray"></HeaderStyle>
                                                <AlternatingRowStyle CssClass="grdAternaterow" BackColor="Transparent" Font-Size="12px">
                                                </AlternatingRowStyle>
                                            </asp:GridView>
                                            </div>
                                        </ContentTemplate>
                                    </asp:TabPanel>
                                    <asp:TabPanel ID="panCancellInfo" runat="server" HeaderText="Cancelled Bookings" TabIndex="56">
                                        <ContentTemplate>
                                            <div id="divCancelBooking" style="height: 70vh; width: 95vw; padding: 4px; overflow: scroll">
                                            <asp:GridView ID="GvCancelBooking" runat="server" AutoGenerateColumns="False" BackColor="White"
                                                BorderColor="#999999" CssClass="td_cell" ShowHeaderWhenEmpty="true" Width="99%"
                                                TabIndex="4">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="" HeaderStyle-Width="12%" Visible="false">
                                                        <ItemTemplate>                                                            
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Booking Code">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblBookingNo" runat="server" Text='<%# Bind("Bookingcode") %>'></asp:Label>
                                                            <asp:TextBox ID="txtMapAgent" runat="server" Text='<%# Bind("Mapagentcode") %>' Style="display: none"></asp:TextBox>
                                                            <asp:TextBox ID="txtMapparty" runat="server" Text='<%# Bind("Mappartycode") %>' Style="display: none"></asp:TextBox>
                                                            <asp:TextBox ID="txtMapsalecurr" runat="server" Text='<%# Bind("Mapsalecurrcode") %>'
                                                                Style="display: none"></asp:TextBox>
                                                            <asp:TextBox ID="txtUniqueId" runat="server" Text='<%# Bind("importlineno") %>' Style="display: none"></asp:TextBox>
                                                            <asp:TextBox ID="txtctryagent" runat="server" Text='<%# Bind("mapAgentCtryCode") %>'
                                                                Style="display: none"></asp:TextBox>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Start date">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblstartdate" CssClass="field_input" Text='<%# Bind("Startdate","{0:dd/MM/yyyy}") %>'
                                                                runat="server"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="End date">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblenddate" CssClass="field_input" Text='<%# Bind("Enddate","{0:dd/MM/yyyy}") %>' runat="server"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Agency">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblAgency" runat="server" Text='<%# Bind("agent") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Agency booking reference">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblAgentBookingRef" runat="server" Text='<%# Bind("agentBookingRef") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Main passenger name">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblMainpssgrname" runat="server" Text='<%# Bind("guestName") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Description">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbldescription" runat="server" Text='<%# Bind("servDescription") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Product group">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblprodgroup" runat="server" Text='<%# Bind("ProductGroup") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Sales currency">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblsalescurr" runat="server" Text='<%# Bind("salescurrcode") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Sales Price">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblsalesprice" runat="server" Text='<%# Bind("SalesPrice") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Supplier name">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblsuppname" runat="server" Text='<%# Bind("SupplierName") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Cost currency">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblcostcurr" runat="server" Text='<%# Bind("costCurrCode") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Cost Price">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblCostprice" runat="server" Text='<%# Bind("Costprice") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Date">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblDate" runat="server" Text='<%# Bind("bookingDate") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Line booking date">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbllinebookdate" runat="server" Text='<%# Bind("Linebookingdate") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Supplier ID">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblsuppid" runat="server" Text='<%# Bind("SupplierID") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Client ID">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblClientId" runat="server" Text='<%# Bind("agentCode") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Country of the Agency">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblAgencyCtry" runat="server" Text='<%# Bind("agencyCtry") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Supplier reference Number" Visible="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblsupprefno" runat="server" Text='<%# Bind("SupplierRefNo") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Nationality" Visible="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblNationality" runat="server" Text='<%# Bind("Nationality") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Exchange Rate" Visible="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblexchangerate" runat="server" Text='<%# Bind("convrate") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="No. of Rooms" Visible="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblNoofRooms" runat="server" Text='<%# Bind("NoofRooms") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Booking Element ID">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblBookElementId" runat="server" Text='<%# Bind("bookingElementId") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                                <RowStyle CssClass="grdRowstyle" BackColor="#EEEEEE" ForeColor="Black"></RowStyle>
                                                <SelectedRowStyle CssClass="grdselectrowstyle" BackColor="#008A8C" ForeColor="White"
                                                    Font-Bold="True"></SelectedRowStyle>
                                                <PagerStyle CssClass="grdpagerstyle" BackColor="#999999" ForeColor="Black" HorizontalAlign="Center">
                                                </PagerStyle>
                                                <HeaderStyle CssClass="grdheader" BackColor="#06788B" ForeColor="White" Font-Bold="True"
                                                    HorizontalAlign="Center" VerticalAlign="Middle" BorderColor="LightGray"></HeaderStyle>
                                                <AlternatingRowStyle CssClass="grdAternaterow" BackColor="Transparent" Font-Size="12px">
                                                </AlternatingRowStyle>
                                            </asp:GridView>
                                            </div>
                                        </ContentTemplate>
                                    </asp:TabPanel>
                                    <asp:TabPanel ID="panImportedInfo" runat="server" HeaderText="Already Imported Bookings" TabIndex="56">
                                        <ContentTemplate>
                                            <asp:HiddenField ID="hdnNoOfBookings" runat="server" />
                                            <asp:HiddenField ID="hdnSalevalue" runat="server" />
                                            <asp:HiddenField ID="hdnCostvalue" runat="server" />
                                            <div id="div1" style="height: 70vh; width: 95vw; padding: 4px; overflow: scroll">
                                            <asp:GridView ID="gvImportedBooking" runat="server" AutoGenerateColumns="False" BackColor="White"
                                                BorderColor="#999999" CssClass="td_cell" ShowHeaderWhenEmpty="true" Width="99%"  DataKeyNames="ispurchaseinvoice"
                                                TabIndex="4">
                                                  <Columns>
                                                        <asp:TemplateField HeaderText="" Visible="false">
                                                            <ItemTemplate>                                                                
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Booking Code">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblBookingNo" runat="server" Text='<%# Bind("Bookingcode") %>'></asp:Label>
                                                                <asp:TextBox ID="txtMapAgent" runat="server" Text='<%# Bind("Mapagentcode") %>' Style="display: none"></asp:TextBox>
                                                                <asp:TextBox ID="txtMapparty" runat="server" Text='<%# Bind("Mappartycode") %>' Style="display: none"></asp:TextBox>
                                                                <asp:TextBox ID="txtMapsalecurr" runat="server" Text='<%# Bind("Mapsalecurr") %>'
                                                                    Style="display: none"></asp:TextBox>
                                                                <asp:TextBox ID="txtUniqueId" runat="server" Text='<%# Bind("UniqueId") %>' Style="display: none"></asp:TextBox>
                                                                <asp:TextBox ID="txtMapAgentCtry" runat="server" Text='<%# Bind("Mapctryagent") %>'
                                                                    Style="display: none"></asp:TextBox>
                                                                <asp:TextBox ID="txtCheckHotelSupplier" runat="server" Text='<%# Bind("checkHotelSupplier") %>'
                                                                    Style="display: none"></asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Start date">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblstartdate" CssClass="field_input" Text='<%# Bind("Startdate") %>'
                                                                    runat="server"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="End date">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblenddate" CssClass="field_input" Text='<%# Bind("Enddate") %>' runat="server"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Agency">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblAgency" runat="server" Text='<%# Bind("Agency") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Agency booking reference">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblAgentBookingRef" runat="server" Text='<%# Bind("Agencybookingref") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Main passenger name">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblMainpssgrname" runat="server" Text='<%# Bind("Mainpssgrname") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Description">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lbldescription" runat="server" Text='<%# Bind("Description") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Product group">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblprodgroup" runat="server" Text='<%# Bind("Prodgrp") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Sales currency">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblsalescurr" runat="server" Text='<%# Bind("Salescurr") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Sales Price">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblsalesprice" runat="server" Text='<%# Bind("SalesPrice") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Supplier name">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblsuppname" runat="server" Text='<%# Bind("Suppname") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Cost currency">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblcostcurr" runat="server" Text='<%# Bind("Costcurr") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Cost Price">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblCostprice" runat="server" Text='<%# Bind("Costprice") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Date">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblDate" runat="server" Text='<%# Bind("Date") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Line booking date">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lbllinebookdate" runat="server" Text='<%# Bind("Linebookingdate") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Supplier ID">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblsuppid" runat="server" Text='<%# Bind("SuppID") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Client ID">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblClientId" runat="server" Text='<%# Bind("ClientID") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Country of the Agency">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblAgencyCtry" runat="server" Text='<%# Bind("CtryAgency") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Supplier reference Number" Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblsupprefno" runat="server" Text='<%# Bind("SupprefNum") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Nationality" Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblNationality" runat="server" Text='<%# Bind("Nationality") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Exchange Rate" Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblexchangerate" runat="server" Text='<%# Bind("ExchRate") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="No. of Rooms" Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblNoofRooms" runat="server" Text='<%# Bind("NoofRooms") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Booking Element ID">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblBookElementId" runat="server" Text='<%# Bind("bookingElementId") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                <RowStyle CssClass="grdRowstyle" BackColor="#EEEEEE" ForeColor="Black"></RowStyle>
                                                <SelectedRowStyle CssClass="grdselectrowstyle" BackColor="#008A8C" ForeColor="White"
                                                    Font-Bold="True"></SelectedRowStyle>
                                                <PagerStyle CssClass="grdpagerstyle" BackColor="#999999" ForeColor="Black" HorizontalAlign="Center">
                                                </PagerStyle>
                                                <HeaderStyle CssClass="grdheader" BackColor="#06788B" ForeColor="White" Font-Bold="True"
                                                    HorizontalAlign="Center" VerticalAlign="Middle" BorderColor="LightGray"></HeaderStyle>
                                                <AlternatingRowStyle CssClass="grdAternaterow" BackColor="Transparent" Font-Size="12px">
                                                </AlternatingRowStyle>
                                            </asp:GridView>
                                            </div>
                                        </ContentTemplate>
                                    </asp:TabPanel>
                                     <asp:TabPanel ID="panReconcile" runat="server" HeaderText="Reconciliation">
                                        <ContentTemplate>
                                            <div id="div2" style="height: 60vh; width: 60vw; padding: 4px;" >
                                            <asp:GridView ID="gvReconcile" runat="server" AutoGenerateColumns="False" BackColor="White"
                                                BorderColor="#999999" CssClass="td_cell" ShowHeaderWhenEmpty="true" Width="99%"
                                                TabIndex="4">
                                                  <Columns>
                                                        <asp:TemplateField HeaderText="Classification" >
                                                            <ItemTemplate>                                                                
                                                                <asp:Label ID="lblClass" runat="server" Text='<%# Bind("Classification") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="No of Bookings">
                                                            <ItemTemplate>                                                                
                                                                <asp:Label ID="lblBookingCnt" runat="server" Text='<%# Bind("NoOfBookings") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Right" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Total Sale Value">
                                                            <ItemTemplate>                                                                
                                                                <asp:Label ID="lblSaleValue" runat="server" Text='<%# Bind("TotalSaleValue") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Right" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Total Cost Value">
                                                            <ItemTemplate>                                                                
                                                                <asp:Label ID="lblCostValue" runat="server" Text='<%# Bind("TotalCostValue") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Right" />
                                                        </asp:TemplateField>                                                        
                                                    </Columns>
                                                <RowStyle CssClass="grdRowstyle" BackColor="#EEEEEE" ForeColor="Black"></RowStyle>
                                                <SelectedRowStyle CssClass="grdselectrowstyle" BackColor="#008A8C" ForeColor="White"
                                                    Font-Bold="True"></SelectedRowStyle>
                                                <PagerStyle CssClass="grdpagerstyle" BackColor="#999999" ForeColor="Black" HorizontalAlign="Center">
                                                </PagerStyle>
                                                <HeaderStyle CssClass="grdheader" BackColor="#06788B" ForeColor="White" Font-Bold="True"
                                                    HorizontalAlign="Center" VerticalAlign="Middle" BorderColor="LightGray"></HeaderStyle>
                                                <AlternatingRowStyle CssClass="grdAternaterow" BackColor="Transparent" Font-Size="12px">
                                                </AlternatingRowStyle>
                                            </asp:GridView>
                                            </div>
                                        </ContentTemplate>
                                    </asp:TabPanel>

                                    <asp:TabPanel ID="panReconcileSummary" runat="server" HeaderText="Reconciliation Summary">
                                        <ContentTemplate>
                                            <div id="div2Summary" style="height: 70vh; width: 95vw; padding: 4px; overflow: scroll" >
                                            <asp:GridView ID="gvReconcileSummary" runat="server" AutoGenerateColumns="False" BackColor="White"
                                                BorderColor="#999999" CssClass="td_cell" ShowHeaderWhenEmpty="true" Width="99%"
                                                TabIndex="4">
                                                  <Columns>
                                                        <asp:TemplateField HeaderText="Booking Code" >
                                                            <ItemTemplate>                                                                
                                                                <asp:Label ID="lblBookingNo" runat="server" Text='<%# Bind("BookingCode") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Sale Value">
                                                            <ItemTemplate>                                                                
                                                                <asp:Label ID="lblSaleValue" runat="server" Text='<%# Bind("SaleValue") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Right" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Selling Currency">
                                                            <ItemTemplate>                                                                
                                                                <asp:Label ID="lblSaleCurrency" runat="server" Text='<%# Bind("salecurrency") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Right" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Non Taxable" Visible="false">
                                                            <ItemTemplate>                                                                
                                                                <asp:Label ID="lblSaleNonTaxable" runat="server" Text='<%# Bind("SaleNonTaxable") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Right" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Taxable" Visible="false">
                                                            <ItemTemplate>                                                                
                                                                <asp:Label ID="lblSaleTaxable" runat="server" Text='<%# Bind("SaleTaxable") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Right" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="VAT" Visible="false">
                                                            <ItemTemplate>                                                                
                                                                <asp:Label ID="lblSaleVAT" runat="server" Text='<%# Bind("SaleVAT") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Right" />
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText="Cost Value">
                                                            <ItemTemplate>                                                                
                                                                <asp:Label ID="lblCostValue" runat="server" Text='<%# Bind("CostValue") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Right" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Cost Non Taxable" Visible="false">
                                                            <ItemTemplate>                                                                
                                                                <asp:Label ID="lblCostNonTaxable" runat="server" Text='<%# Bind("CostNonTaxable") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Right" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Cost Taxable" Visible="false">
                                                            <ItemTemplate>                                                                
                                                                <asp:Label ID="lblCostTaxable" runat="server" Text='<%# Bind("CostTaxable") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Right" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Cost VAT" Visible="false">
                                                            <ItemTemplate>                                                                
                                                                <asp:Label ID="lblCostVAT" runat="server" Text='<%# Bind("CostVAT") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Right" />
                                                        </asp:TemplateField>
                                                        
                                                        <asp:TemplateField HeaderText="Booking Type" >
                                                            <ItemTemplate>                                                                
                                                                <asp:Label ID="lblBookingType" runat="server" Text='<%# Bind("BookingType") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Arrival Date" >
                                                            <ItemTemplate>                                                                
                                                                <asp:Label ID="lblstartdate" runat="server"  Text='<%# Bind("arrivalDate") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Departure Date" >
                                                            <ItemTemplate>                                                                
                                                                <asp:Label ID="lblenddate" runat="server"  Text='<%# Bind("departureDate") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                         <asp:TemplateField HeaderText="Invoice Sealed" >
                                                            <ItemTemplate>                                                                
                                                                <asp:Label ID="lblInvoiceSealed" runat="server"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                <RowStyle CssClass="grdRowstyle" BackColor="#EEEEEE" ForeColor="Black"></RowStyle>
                                                <SelectedRowStyle CssClass="grdselectrowstyle" BackColor="#008A8C" ForeColor="White"
                                                    Font-Bold="True"></SelectedRowStyle>
                                                <PagerStyle CssClass="grdpagerstyle" BackColor="#999999" ForeColor="Black" HorizontalAlign="Center">
                                                </PagerStyle>
                                                <HeaderStyle CssClass="grdheader" BackColor="#06788B" ForeColor="White" Font-Bold="True"
                                                    HorizontalAlign="Center" VerticalAlign="Middle" BorderColor="LightGray"></HeaderStyle>
                                                <AlternatingRowStyle CssClass="grdAternaterow" BackColor="Transparent" Font-Size="12px">
                                                </AlternatingRowStyle>
                                            </asp:GridView>
                                            </div>
                                        </ContentTemplate>
                                    </asp:TabPanel> 
                                   <%-- --sharfudeen 24/10/2022--%>
                                     <asp:TabPanel ID="panPurchaseinvoiceDetails" runat="server" HeaderText="Mismatch PI">
                                        <ContentTemplate>
                                            <div id="div3" style="height: 70vh; width: 95vw; padding: 4px; overflow: scroll" >
                                            <asp:GridView ID="gvMismatchpI" runat="server" AutoGenerateColumns="False" BackColor="White"
                                                BorderColor="#999999" CssClass="td_cell" ShowHeaderWhenEmpty="true" Width="99%"
                                                TabIndex="4">
                                                  <Columns>
                                                        <asp:TemplateField HeaderText="Booking Code" >
                                                            <ItemTemplate>                                                                
                                                                <asp:Label ID="lblBookingNo" runat="server" Text='<%# Bind("BookingCode") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Sale Value">
                                                            <ItemTemplate>                                                                
                                                                <asp:Label ID="lblSaleValue" runat="server" Text='<%# Bind("SaleValue") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Right" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Selling Currency">
                                                            <ItemTemplate>                                                                
                                                                <asp:Label ID="lblSaleCurrency" runat="server" Text='<%# Bind("salecurrency") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Right" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Non Taxable" Visible="false">
                                                            <ItemTemplate>                                                                
                                                                <asp:Label ID="lblSaleNonTaxable" runat="server" Text='<%# Bind("SaleNonTaxable") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Right" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Taxable" Visible="false">
                                                            <ItemTemplate>                                                                
                                                                <asp:Label ID="lblSaleTaxable" runat="server" Text='<%# Bind("SaleTaxable") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Right" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="VAT" Visible="false">
                                                            <ItemTemplate>                                                                
                                                                <asp:Label ID="lblSaleVAT" runat="server" Text='<%# Bind("SaleVAT") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Right" />
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText="Cost Value">
                                                            <ItemTemplate>                                                                
                                                                <asp:Label ID="lblCostValue" runat="server" Text='<%# Bind("CostValue") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Right" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Cost Non Taxable" Visible="false">
                                                            <ItemTemplate>                                                                
                                                                <asp:Label ID="lblCostNonTaxable" runat="server" Text='<%# Bind("CostNonTaxable") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Right" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Cost Taxable" Visible="false">
                                                            <ItemTemplate>                                                                
                                                                <asp:Label ID="lblCostTaxable" runat="server" Text='<%# Bind("CostTaxable") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Right" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Cost VAT" Visible="false">
                                                            <ItemTemplate>                                                                
                                                                <asp:Label ID="lblCostVAT" runat="server" Text='<%# Bind("CostVAT") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Right" />
                                                        </asp:TemplateField>
                                                        
                                                        <asp:TemplateField HeaderText="Booking Type" >
                                                            <ItemTemplate>                                                                
                                                                <asp:Label ID="lblBookingType" runat="server" Text='<%# Bind("BookingType") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Arrival Date" >
                                                            <ItemTemplate>                                                                
                                                                <asp:Label ID="lblstartdate" runat="server"  Text='<%# Bind("arrivalDate") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Departure Date" >
                                                            <ItemTemplate>                                                                
                                                                <asp:Label ID="lblenddate" runat="server"  Text='<%# Bind("departureDate") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                         <asp:TemplateField HeaderText="Invoice Sealed" >
                                                            <ItemTemplate>                                                                
                                                                <asp:Label ID="lblInvoiceSealed" runat="server"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>

                                                   <asp:TemplateField HeaderText="Mapping PI">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lbtnMatchlineno" runat="server" Text='Mapping PI' 
                                                                CommandName="MappingPI" CommandArgument='<%# Container.DisplayIndex %>'></asp:LinkButton>
                                                        </ItemTemplate>
                                                         <ItemStyle HorizontalAlign="Center"  />
                                                      </asp:TemplateField>

                                                    </Columns>
                                                <RowStyle CssClass="grdRowstyle" BackColor="#EEEEEE" ForeColor="Black"></RowStyle>
                                                <SelectedRowStyle CssClass="grdselectrowstyle" BackColor="#008A8C" ForeColor="White"
                                                    Font-Bold="True"></SelectedRowStyle>
                                                <PagerStyle CssClass="grdpagerstyle" BackColor="#999999" ForeColor="Black" HorizontalAlign="Center">
                                                </PagerStyle>
                                                <HeaderStyle CssClass="grdheader" BackColor="#06788B" ForeColor="White" Font-Bold="True"
                                                    HorizontalAlign="Center" VerticalAlign="Middle" BorderColor="LightGray"></HeaderStyle>
                                                <AlternatingRowStyle CssClass="grdAternaterow" BackColor="Transparent" Font-Size="12px">
                                                </AlternatingRowStyle>
                                            </asp:GridView>
                                            </div>
                                        </ContentTemplate>
                                    </asp:TabPanel> 
                                </asp:TabContainer>
                                
                            </td>
                        </tr> 
                        <tr>
                        <td colspan="8" align="center">                                
                            &nbsp;&nbsp; &nbsp;&nbsp;                                
                        </td>
                        </tr>                       
                        <tr id="trSave" runat="server">
                            <td colspan="8" align="center">  
                                <asp:HiddenField ID="hdnValidBooking" runat="server" />          
                                <asp:Button ID="btnReleaseInvoiceSealed" runat="server" CssClass="field_button" Text="Identify Invoice Sealed" OnClientClick="return showProgress();"/>
                                <asp:Button ID="btnExcelNewBooking" runat="server" CssClass="field_button" Text="Export to Excel"/>                       
                                <asp:Button ID="btnSave" TabIndex="7" runat="server" Text="Save" CssClass="field_button" OnClientClick="return showProgress();"></asp:Button>&nbsp;&nbsp;
                                <asp:Button ID="btnCancel" TabIndex="8" runat="server" Text="Return To Search" CssClass="field_button"></asp:Button>&nbsp;&nbsp;
                                <asp:Button ID="btnhelp" TabIndex="9" runat="server" Text="Help"
                                    CssClass="field_button"></asp:Button>&nbsp;
                            </td>                            
                        </tr>                   
                </table>              
            </div>
            <div>
            <center>              
                    <div id="Loading1" runat="server" style="height: 150px; width: 500px; vertical-align: middle">
                    <img alt="" id="Img1" runat="server" src="~/Images/loader-progressbar.gif" width="150" />
                    <h2 style="color: #06788B">
                        Processing please wait...</h2>
                </div>                 
                </center>
                <asp:ModalPopupExtender ID="ModalPopupLoading" runat="server" BehaviorID="ModalPopupLoading"
                    TargetControlID="btnInvisibleLoading" CancelControlID="btnCloseLoading" PopupControlID="Loading1"
                    BackgroundCssClass="ModalPopupBG">
                </asp:ModalPopupExtender>
                <input id="btnInvisibleLoading" runat="server" type="button" value="Cancel" style="display: none" />
                <input id="btnCloseLoading" type="button" value="Cancel" style="display: none" />
            </div>

           <%-- --sharfudeen 21/10/2022--%>
             <div id="ShowMappurchseinvoice" runat="server" style="overflow-y: scroll; height: 650px;
                width: 80%; border: 3px solid green; background-color: White; display: none;">
                <table  width="100%">
                    <tr>
                        <td align="center">
                             <asp:Label ID="Label1" runat="server"  Text="Mapping Purchase Invoice Details" Style="padding-top: 4px;
                                padding-bottom: 5px;" CssClass="field_heading" Width="100%"></asp:Label>
                        </td>
                    </tr>
                </table>
                <table cellpadding="7px" width="100%">
                    <tr>
                     <td>
                        <label class="field_caption">Booking No</label>
                         &nbsp;&nbsp;
                        <asp:TextBox ID="txtBookingNo" CssClass="field_input" runat="server" TabIndex="1" Width="150px" style="background-color:#DCDCDC" ReadOnly="true"></asp:TextBox>
                     </td>
                     <td>
                     </td>
                    </tr>
                     <tr style="padding-top: 0%">
                        <td align="left">
                         <div  style="text-align:center">
                           <asp:Label ID="lblgvMapdetail1" runat="server" Text="Previously imported"   class="field_heading"  Width="100%" ></asp:Label>
                        </div>
                        <div>
                            <asp:GridView ID="gvMapdetail1" runat="server" AutoGenerateColumns="False" BackColor="White"
                                BorderColor="#999999" CssClass="grdstyle"  Width="100%"   ShowHeader="true"  ShowHeaderWhenEmpty="true"
                                TabIndex="25" Font-Bold="true" >
                                <Columns>
                                     <asp:TemplateField HeaderText="ElementId" HeaderStyle-Width="18%" >
                                        <ItemTemplate>
                                            <asp:Label ID="lblelementid" runat="server" Text='<%# Bind("bookingElementId") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Invoice no." HeaderStyle-Width="18%">
                                        <ItemTemplate>
                                            <asp:Label ID="lblinvoiceno" runat="server" Text='<%# Bind("invoiceno") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Lineno" HeaderStyle-Width="14%" >
                                        <ItemTemplate>
                                            <asp:Label ID="lblPaxType" runat="server" Text='<%# Bind("acc_line_no") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Product Group"  >
                                        <ItemTemplate>
                                            <asp:Label ID="lblPrdgrp" runat="server" Text='<%# Bind("ProductGroup") %>'></asp:Label>
										    <asp:HiddenField ID="hdnPrdgrp" runat="server" value='<%# Bind("ProductGroup") %>'/>
										     <ItemStyle Wrap="true" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Supplier" >
                                        <ItemTemplate>
                                            <asp:Label ID="lblsupplier" runat="server" Text='<%# Bind("supplier") %>'></asp:Label>
											<asp:HiddenField ID="hdnpartycode" runat="server" value='<%# Bind("partycode") %>'/>
                                            <asp:HiddenField ID="hdnsupplierid" runat="server" value='<%# Bind("supplierid") %>'/>
										 <ItemStyle Wrap="true" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Acc_ref1" HeaderStyle-Width="18%">
                                        <ItemTemplate>
                                            <asp:Label ID="lblaccref1" runat="server" Text='<%# Bind("acc_ref2") %>'></asp:Label>
                                        </ItemTemplate>
                                          <FooterStyle HorizontalAlign="right" />
                                    </asp:TemplateField>
	                                   <asp:TemplateField HeaderText="PIno." HeaderStyle-Width="16%">
                                        <ItemTemplate>
                                            <asp:Label ID="lblpurchaseinvoiceno" runat="server" Text='<%# Bind("Purchaseinvoiceno") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>			
                                      <asp:TemplateField HeaderText="PIlineno" HeaderStyle-Width="18%" >
                                        <ItemTemplate>
                                            <asp:Label ID="lblPIlineno" runat="server" Text='<%# Bind("PIlineno") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField> 								
	                                   <asp:TemplateField HeaderText="Reasons" HeaderStyle-Width="18%">
                                        <ItemTemplate>
                                            <asp:Label ID="lblreasons" runat="server" Text='<%# Bind("Reason") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField> 								
                                </Columns>
                                <RowStyle CssClass="grdRowstyle"></RowStyle>
                                <SelectedRowStyle CssClass="grdselectrowstyle"></SelectedRowStyle>
                                <PagerStyle CssClass="grdpagerstyle"></PagerStyle>
                                <HeaderStyle CssClass="grdheader" ForeColor="White" BorderColor="LightGray"></HeaderStyle>
                                <AlternatingRowStyle CssClass="grdAternaterow"></AlternatingRowStyle>
                                <FooterStyle CssClass="grdfooter" />
                            </asp:GridView>
                        </div>
                               <asp:Label ID="lblMsgMapPi1" runat="server" Text="Records not found, Please check booking details"
                                Font-Size="8pt" Font-Names="Verdana" ForeColor="#084573" Font-Bold="True" Width="357px"
                                Visible="False"></asp:Label>
                        </td>
                        <td>   
                        <div  style="text-align:center "  >
                           <asp:Label ID="lblgvMapdetail2" runat="server" Text="New imported from excel"   class="field_heading"  Width="100%" ></asp:Label>
                        </div>
                         <div>
                            <asp:GridView ID="gvMapdetail2"  runat="server" AutoGenerateColumns="False" BackColor="White"
                                BorderColor="#999999" CssClass="grdstyle" Width="100%"    ShowHeaderWhenEmpty="true" 
                                TabIndex="25" Font-Bold="true" >
                                <Columns>
                                     <asp:TemplateField HeaderText="ElementId" HeaderStyle-Width="18%" >
                                        <ItemTemplate>
                                            <asp:Label ID="lblelementid" runat="server" Text='<%# Bind("bookingElementId") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Invoice no." HeaderStyle-Width="18%" >
                                        <ItemTemplate>
                                            <asp:Label ID="lblinvoiceno" runat="server" Text='<%# Bind("invoiceno") %>'></asp:Label>
                                            <asp:HiddenField ID="hdnacctranlineno" runat="server" value='<%# Bind("acc_line_no") %>'/>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                     <asp:TemplateField HeaderText="Product Group" >
                                        <ItemTemplate>
                                            <asp:Label ID="lblPrdgrp" runat="server" Text='<%# Bind("ProductGroup") %>'></asp:Label>
										    <asp:HiddenField ID="hdnPrdgrp" runat="server" value='<%# Bind("ProductGroup") %>'/>
										     <ItemStyle Wrap="true" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Supplier" >
                                        <ItemTemplate>
                                            <asp:Label ID="lblsupplier" runat="server" Text='<%# Bind("supplier") %>'></asp:Label>
											<asp:HiddenField ID="hdnpartycode" runat="server" value='<%# Bind("partycode") %>'/>
                                            <asp:HiddenField ID="hdnsupplierid" runat="server" value='<%# Bind("supplierid") %>'/>
										 <ItemStyle Wrap="true" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Acc_ref1" >
                                        <ItemTemplate>
                                            <asp:Label ID="lblaccref1" runat="server" Text='<%# Bind("acc_ref2") %>'></asp:Label>
                                        </ItemTemplate>
                                          <FooterStyle HorizontalAlign="right" />
                                    </asp:TemplateField>                                                               						
                                   <asp:TemplateField HeaderText="Maplineno" HeaderStyle-Width="14%">
                                        <ItemTemplate>
                                          <asp:Label ID="lblMapacclineno" runat="server" Text='<%# Bind("acc_line_no") %>'></asp:Label>
                                            </asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>	
                                   <asp:TemplateField HeaderText="Reasons" HeaderStyle-Width="18%">
                                        <ItemTemplate>
                                            <asp:Label ID="lblreasons" runat="server" Text='<%# Bind("Reason") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>                                   								
                                </Columns>
                                <RowStyle CssClass="grdRowstyle"></RowStyle>
                                <SelectedRowStyle CssClass="grdselectrowstyle"></SelectedRowStyle>
                                <PagerStyle CssClass="grdpagerstyle"></PagerStyle>
                                <HeaderStyle CssClass="grdheader" ForeColor="White" BorderColor="LightGray"></HeaderStyle>
                                <AlternatingRowStyle CssClass="grdAternaterow"></AlternatingRowStyle>
                                <FooterStyle CssClass="grdfooter" />
                            </asp:GridView>
                         </div>
                               <asp:Label ID="lblMsgMapPi2" runat="server" Text="Records not found, Please check booking details"
                                Font-Size="8pt" Font-Names="Verdana" ForeColor="#084573" Font-Bold="True" Width="357px"
                                Visible="False"></asp:Label>                       
                        </td>
                    </tr>
                    <tr>
                        <td></td>
                        <td></td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Button ID="btnmappiSave" runat="server"  Visible="false"  
                                CssClass="field_button" Text="Save" Width="80px" TabIndex="26" />
                            &nbsp;&nbsp;
                        </td>
                         <td>    <asp:Button ID="btnmappiClose" runat="server" CssClass="field_button" Text="Cancel" Width="80px"/></td>
                    </tr>
                </table>
                <input id="btnInvisibleMappi" runat="server" type="button" value="Cancel" style="visibility: hidden" />
                <input id="btnOkayMappi" type="button" value="OK" style="visibility: hidden" />
                <input id="btnCancelMappi" type="button" value="Cancel" style="visibility: hidden" />
            </div>
            <asp:ModalPopupExtender ID="ModalExtraPopup" runat="server" BehaviorID="ModalExtraPopup"
                CancelControlID="btnCancelMappi" OkControlID="btnOkayMappi" TargetControlID="btnInvisibleMappi"
                PopupControlID="ShowMappurchseinvoice" PopupDragHandleControlID="ShowMappurchseinvoice" Drag="true"
                BackgroundCssClass="ModalPopupBG">
            </asp:ModalPopupExtender>

            <asp:HiddenField ID ="hdnChkDtFlag" runat="server" />
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnImportExcel" />
        </Triggers>
    </asp:UpdatePanel>

</asp:Content>



