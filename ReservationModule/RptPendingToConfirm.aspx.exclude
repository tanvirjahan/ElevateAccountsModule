<%@ Page Title="" Language="VB" MasterPageFile="~/ReservationMaster.master" AutoEventWireup="false" CodeFile="RptPendingToConfirm.aspx.vb" Inherits="RptPendingToConfirm" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ OutputCache Location="none" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">
<meta name="viewport" content="width=device-width, initial-scale=1.0">
<script src="../Content/vendor/jquery-1.11.0.js" type="text/javascript" charset="utf-8"></script>
<script src="../Content/vendor/jquery-1.8.3.min.js" type="text/javascript"></script>

<script type="text/javascript">

    function DateSelectCalExt() {
        var txtfromDate = document.getElementById("<%=txtFromDt.ClientID%>");
        if (txtfromDate.value != '') {
            var calendarBehavior1 = $find("<%=FromDt_CalendarExtender.ClientID %>");
            var date = calendarBehavior1._selectedDate;

            var dp = txtfromDate.value.split("/");
            var newDt = new Date(dp[2] + "/" + dp[1] + "/" + dp[0]);
            newDt = getFormatedDate(newDt);
            newDt = new Date(newDt);
            calendarBehavior1.set_selectedDate(newDt);
        }
        var txtfromDate2 = document.getElementById("<%=txtToDt.ClientID%>");
        if (txtfromDate2.value != '') {
            var calendarBehavior2 = $find("<%=ToDt_CalendarExtender.ClientID %>");
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
        var txtfromDate = document.getElementById("<%=txtFromDt.ClientID%>");
        var txtToDate = document.getElementById("<%=txtToDt.ClientID%>");

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

        var txtfromDate = document.getElementById("<%=txtFromDt.ClientID%>");
        var txtToDate = document.getElementById("<%=txtToDt.ClientID%>");
        if (txtfromDate.value == null || txtfromDate.value == "") {
            txtfromDate.value = txtToDate.value;
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
            alert("To date should be greater than or equal to From date");
        }
    }

    function validation() {

        var txtfromDate = document.getElementById("<%=txtFromDt.ClientID%>");
        var txttoDate = document.getElementById("<%=txtToDt.ClientID%>");

        if ((txtfromDate.value == null) || (txtfromDate.value == '')) {
            alert("Please select from date.");
            txtfromDate.focus();
            return false;
        }

        if ((txttoDate.value == null) || (txttoDate.value == '')) {
            alert("Please select to date.");
            txttoDate.focus();
            return false;
        }

        return true;

    }


</script>

<asp:UpdatePanel ID="UpdatePanel2" runat="server">
    <ContentTemplate>
        <div style="margin-top: -6px; width: 60%;">
            <table style="border: gray 2px solid; width: 100%;" class="td_cell" align="left">
                <tr>
                    <td valign="top" align="center" style="width: 100%;">
                        <asp:Label ID="lblHeading" runat="server" Text="Pending Confirmation" CssClass="field_heading"
                            Width="100%" ForeColor="White" Style="padding: 2px"></asp:Label>
                    </td>
                </tr>
                <tr style="display: none">
                    <td style="width: 100%; padding: 10px 0px 12px 0px" align="center">
                        <asp:Button ID="btnHelp" runat="server" Text="Help" Font-Bold="False" CssClass="search_button"
                            Style="display: none"></asp:Button>
                        &nbsp;&nbsp;<asp:Button ID="btnAddNew" runat="server" Text="Add New" Font-Bold="False"
                            CssClass="btn" Style="display: none"></asp:Button>
                        <asp:Button ID="btnExportToExcel" runat="server" CssClass="btn" Font-Bold="False"
                            Text="Export To Excel" Style="display: none" />
                        <asp:GridView ID="gvSearchResult" runat="server" Style="display: none">
                        </asp:GridView>
                        <asp:TextBox ID="txtRptType" runat="server" Style="display: none"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td style="width: 100%">
                        <table cellpadding="7" width="100%">
                            <tr>
                                <td style="width: 25%;">
                                    <asp:Label ID="frmdate" runat="server" class="field_caption" Text="From Date"></asp:Label>
                                </td>
                                <td style="width: 75%;">
                                    <asp:TextBox ID="txtFromDt" CssClass="field_input" runat="server" TabIndex="1" onchange="filltodate(this);"
                                        Width="75px"></asp:TextBox>
                                    <asp:CalendarExtender ID="FromDt_CalendarExtender" runat="server" Format="dd/MM/yyyy"
                                        OnClientDateSelectionChanged="DateSelectCalExt" PopupButtonID="ImgBtnFromDt"
                                        PopupPosition="Right" TargetControlID="txtFromDt">
                                    </asp:CalendarExtender>
                                    <asp:MaskedEditExtender ID="FromDt_MaskedEditExtender" runat="server" Mask="99/99/9999"
                                        MaskType="Date" TargetControlID="txtFromDt">
                                    </asp:MaskedEditExtender>
                                    <asp:ImageButton ID="ImgBtnFromDt" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.png"
                                        TabIndex="-1" /><br />
                                    <asp:MaskedEditValidator ID="MevFromDt" runat="server" ControlExtender="FromDt_MaskedEditExtender"
                                        ControlToValidate="txtFromDt" CssClass="field_error" Display="Dynamic" EmptyValueBlurredText="Date is required"
                                        EmptyValueMessage="Date is required" ErrorMessage="FromDt_MaskedEditExtender"
                                        InvalidValueBlurredMessage="Invalid Date" InvalidValueMessage="Invalid Date"
                                        TooltipMessage="Input a Date in Date/Month/Year">
                                    </asp:MaskedEditValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label class="field_caption" ID="lbltodate" runat="server" Text=" To Date"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtToDt" CssClass="field_input" onchange="ValidateDate();" runat="server"
                                        TabIndex="2" Width="75px"></asp:TextBox>
                                    <asp:CalendarExtender ID="ToDt_CalendarExtender" runat="server" Format="dd/MM/yyyy"
                                        OnClientDateSelectionChanged="DateSelectCalExt" PopupButtonID="ImgBtnToDt" PopupPosition="Right"
                                        TargetControlID="txtToDt">
                                    </asp:CalendarExtender>
                                    <asp:MaskedEditExtender ID="ToDt_MaskedEditExtender" runat="server" Mask="99/99/9999"
                                        MaskType="Date" TargetControlID="txtToDt">
                                    </asp:MaskedEditExtender>
                                    <asp:ImageButton ID="imgBtnToDt" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.png"
                                        TabIndex="-1" /><br />
                                    <asp:MaskedEditValidator ID="MevToDt" runat="server" ControlExtender="ToDt_MaskedEditExtender"
                                        ControlToValidate="txtToDt" CssClass="field_error" Display="Dynamic" EmptyValueBlurredText="Date is required"
                                        EmptyValueMessage="Date is required" ErrorMessage="ToDt_MaskedEditExtender" InvalidValueBlurredMessage="Invalid Date"
                                        InvalidValueMessage="Invalid Date" TooltipMessage="Input a Date in Date/Month/Year">
                                    </asp:MaskedEditValidator>
                                </td>
                            </tr>
                            <tr>
                            <td>
                                <asp:Label class="field_caption" ID="lblDdlCaption" runat="server" Text="Invoice Type"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlInvoiceStateType" CssClass="field_input" runat="server"
                                    Width="50%" TabIndex="3">                                    
                                </asp:DropDownList>
                            </td>
                            </tr>
                            <tr runat="server" id="trDivision">
                            <td>
                                <asp:Label class="field_caption" ID="Label1" runat="server" Text="Division"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlDivision" CssClass="field_input" runat="server"
                                    Width="50%" TabIndex="4">                                    
                                </asp:DropDownList>
                            </td>
                            </tr>
                            <tr>
                                <td align="center" colspan="2">
                                    <asp:Button ID="btnLoadReport" runat="server" TabIndex="9" CssClass="btn" Text="Report" OnClientClick="return validation();"/>
                                    &nbsp;&nbsp;&nbsp;
                                    <asp:Button ID="btnReset" runat="server" CssClass="btn" Text="Reset" TabIndex="10" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </div>
    </ContentTemplate>
    <Triggers>
        <asp:PostBackTrigger ControlID="btnLoadReport" />
         <asp:PostBackTrigger ControlID="btnReset" />
    </Triggers>
    </asp:UpdatePanel>





</asp:Content>

