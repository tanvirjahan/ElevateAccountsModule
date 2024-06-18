<%@ Page Language="VB" AutoEventWireup="false" CodeFile="InterDeptTransferPosting.aspx.vb"
    Inherits="InterDeptTransferPosting" MasterPageFile="~/SubPageMaster.master" Strict="true" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="EclipseWebSolutions.DatePicker" Namespace="EclipseWebSolutions.DatePicker"
    TagPrefix="ews" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="server">
    <script src="../Content/js/jquery-1.5.1.min.js" type="text/javascript"></script>
    <script src="../Content/js/JqueryUI.js" type="text/javascript"></script>
    <script src="../Content/js/accounts.js" type="text/javascript"></script>
    <script language="JavaScript" type="text/javascript">
        window.history.forward(1);

        //        function checkNumber(evt,ctrl) {
        //            evt = (evt) ? evt : event;
        //            var charCode = (evt.charCode) ? evt.charCode : ((evt.keyCode) ? evt.keyCode :
        //        ((evt.which) ? evt.which : 0));            
        //            if (charCode != 47 && (charCode > 44 && charCode < 58)) {
        //                return true;
        //            }
        //            return false;
        //        }

        function checkNumber(evt, txt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode > 31 && (charCode < 48 || charCode > 57)) {
                if (charCode == 46) {
                    return true;
                }
                return false;
            } else {
                return true;
            }


        }

        function f_grid_selectAll(sender) {
            var value = sender.checked;
            var grid = document.getElementById("<%= grdTransferPost.ClientID %>");
            var inputList = grid.getElementsByTagName("input");
            for (var i = 0; i < inputList.length; i++) {
                if (inputList[i].type == "checkbox") {
                    inputList[i].checked = value;
                }
            }
            var total = 0.0;
            var txttot = document.getElementById("<%= txtTotal.ClientID %>");
            txttot.value = total;
            if (grid != null) {
                if (value == true) {
                    var inputList = grid.getElementsByTagName("input");
                    for (var i = 0; i < inputList.length; i++) {
                        if (inputList[i].type == "text") {
                            var IdList = inputList[i].id.split("_");
                            var item = IdList.length;
                            if (IdList[item - 1] == "txtCostValue") {
                                if (inputList[i].value == '') { inputList[i].value = 0 }
                                total = parseFloat(total) + parseFloat(inputList[i].value);
                            }
                        }
                    }
                    txttot.value = total;
                } else {
                    txttot.value = "0.00";
                }
            }
            var IsValidate = 1;

            var isIE = navigator.appName;
            if (isIE == "Microsoft Internet Explorer") {
                if (grid.rows.length > 0) {
                    for (Row = 1; Row < grid.rows.length; Row++) {
                        if (grid.rows[Row].cells[0].childNodes[0].checked == true) {
                            if (grid.rows[Row].cells[11].childNodes[0].value == "0.0000") {
                                IsValidate = 0;
                                grid.rows[Row].cells[0].childNodes[0].checked = false;
                            }
                        }
                    }
                }

            } else {
                if (grid.rows.length > 0) {
                    for (Row = 1; Row < grid.rows.length; Row++) {
                        if (grid.rows[Row].cells[0].children[0].checked == true) {
                            if (grid.rows[Row].cells[11].children[0].value == "0.0000") {
                                IsValidate = 0;
                                grid.rows[Row].cells[0].children[0].checked = false;
                            }
                        }
                    }
                }
            }
            if (IsValidate == 0) {
                alert('cost value should not be equal to zero. Please enter value greater than zero and select the row');
            }
        }

        function CalculateTotal(chk, txtcostval, inde) {
            var chk1 = document.getElementById(chk);
            var txtcostval1 = document.getElementById(txtcostval);
            var txttotval1 = document.getElementById("<%= txtTotal.ClientID %>");
            var hdntotval = document.getElementById("<%= hdnTotalVal.ClientID %>");
            var hdnCurInd = document.getElementById("<%= hdnCurInd.ClientID %>");

            if (hdntotval.value == "NaN" || hdntotval.value == '') {
                hdntotval.value = "0.00";
            }
            if (txttotval1.value == "NaN" || txttotval1.value == '') {
                txttotval1.value = "0.00"
            }
            if (chk1.checked == true) {
                if (hdnCurInd.value != inde) {
                    if (txtcostval1.value == "0.0000") {
                        alert('cost value should not be zero');
                    }
                    else {
                        hdnCurInd.value = inde;
                        hdntotval.value = parseFloat(hdntotval.value) + parseFloat(txtcostval1.value);
                        if (isNaN(hdntotval.value)) { hdntotval.value = 0.00; }
                        txttotval1.value = hdntotval.value;
                    }
                }
            } else {
                hdntotval.value = parseFloat(hdntotval.value) - parseFloat(txtcostval1.value);
                if (isNaN(hdntotval.value)) { hdntotval.value = 0.00; }
                txttotval1.value = hdntotval.value;
            }
        }


        function GetSelectedValue() {
            var grid = document.getElementById("<%= grdTransferPost.ClientID %>");
            var txttotval1 = document.getElementById("<%= txtTotal.ClientID %>");
            var costval = 0.00
            if (txttotval1.value == "NaN" || txttotval1.value == '') {
                txttotval1.value = "0.00"
            }
            var isIE = navigator.appName;
            if (isIE == "Microsoft Internet Explorer") {
                if (grid.rows.length > 0) {
                    for (Row = 1; Row < grid.rows.length; Row++) {
                        if (grid.rows[Row].cells[0].childNodes[0].checked == true) {
                            var valu = parseFloat(grid.rows[Row].cells[11].childNodes[0].value).toFixed(2);
                            if (valu <= 0) {
                                alert('cost value should not be zero');
                            }
                            costval = parseFloat(costval) + parseFloat(grid.rows[Row].cells[11].childNodes[0].value);
                        }
                    }
                }
            } else {
                if (grid.rows.length > 0) {
                    for (Row = 1; Row < grid.rows.length; Row++) {
                        if (grid.rows[Row].cells[0].children[0].checked == true) {
                            var valu = parseFloat(grid.rows[Row].cells[11].children[0].value).toFixed(2);
                            if (valu <= 0) {      
                                alert('cost value should not be zero');
                            }
                            costval = parseFloat(costval) + parseFloat(grid.rows[Row].cells[11].children[0].value);
                        }
                    }
                }
            }
            txttotval1.value = costval;
        }

        function validaterows() {
            var IsValidate = 1;
            var grid = document.getElementById("<%= grdTransferPost.ClientID %>");
            var txttotval1 = document.getElementById("<%= txtTotal.ClientID %>");

            var isIE = navigator.appName;
            if (isIE == "Microsoft Internet Explorer") {
                if (grid.rows.length > 0) {
                    for (Row = 1; Row < grid.rows.length; Row++) {
                        if (grid.rows[Row].cells[0].childNodes[0].checked == true) {
                            var valu = parseFloat(grid.rows[Row].cells[11].childNodes[0].value).toFixed(2);
                            if (valu <= 0) {
                                IsValidate = 0;
                                grid.rows[Row].cells[0].childNodes[0].checked = false;
                            }
                        }
                    }
                }

            } else {
                if (grid.rows.length > 0) {
                    for (Row = 1; Row < grid.rows.length; Row++) {
                        if (grid.rows[Row].cells[0].children[0].checked == true) {
                            var valu = parseFloat(grid.rows[Row].cells[11].children[0].value).toFixed(2);
                            if (valu <= 0) {                           
                                IsValidate = 0;
                                grid.rows[Row].cells[0].children[0].checked = false;
                            }
                        }
                    }
                }
            }
            if (IsValidate == 0) {
                alert('cost value should not be equal to zero. Only the checked row will get saved. Please enter value greater than zero and select the row and then save.');
            }

        }

    </script>
    <style type="text/css">
        .MyDivClass
        {
            height: 600px;
            width: 100%;
            overflow-x: hidden;
        }
        
        .hiddencol
        {
            display: none;
        }
        
        .spanclass
        {
            text-decoration: none;
            display: inline;
            margin: 0;
            padding: 0px 0px 0px 30px;
            _padding: 0px 0px 0px 30px; /* this did the trick. Only IE6 should 
	padding-left:90px;*/
        }
        
        .ModalPopupBG
        {
            background-color: gray;
            filter: alpha(opacity=50);
            opacity: 0.7;
        }
        
        .HellowWorldPopup
        {
            min-width: 200px;
            min-height: 150px;
            background: white;
            font-size: 10pt;
            font-weight: bold;
            border-bottom-style: double;
            border-width: medium;
        }
        
        #TextArea1
        {
            height: 142px;
            width: 418px;
        }
        #Text1
        {
            width: 91px;
        }
        #Select1
        {
            width: 225px;
        }
        #Select3
        {
            width: 166px;
        }
        #txtNotes
        {
            height: 22px;
            width: 181px;
        }
        
        
        #ddlArrDep
        {
            width: 35px;
        }
        
        
        .HideControl
        {
            display: none;
        }
        .style1
        {
            width: 1045px;
            text-align: right;
        }
        .style2
        {
            width: 849px;
            text-align: center;
        }
        .style3
        {
            width: 1045px;
            text-align: center;
        }
        .scrlbr
        {
            width:100%;
            overflow-y:scroll;
            overflow-x:hidden;                        
            }
    </style>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <table cellpadding="0" cellspacing="0" width="100%">
                <tr>
                    <td align="center" class="field_heading" colspan="6">
                        &nbsp;Inter Department Transfer Posting
                    </td>
                </tr>
                <tr style="padding-top:8px">
                    <td align="center" colspan="6">
                        <table>
                            <tr>
                                <td style="width: 80px">
                                    From Date:
                                </td>
                                <td style="width: 100px">
                                    <asp:TextBox ID="txtfromDate" runat="server" CssClass="fiel_input" Width="60px"></asp:TextBox>
                                    <cc1:CalendarExtender ID="dpFromDate" runat="server" Format="dd/MM/yyyy" PopupButtonID="ImgBtnFrmDt"
                                        TargetControlID="txtfromDate" PopupPosition="Right">
                                    </cc1:CalendarExtender>
                                    <cc1:MaskedEditExtender ID="MeFromDate" runat="server" Mask="99/99/9999" MaskType="Date"
                                        TargetControlID="txtfromDate">
                                    </cc1:MaskedEditExtender>
                                    <asp:ImageButton ID="ImgBtnFrmDt" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.png" /><br />
                                    <cc1:MaskedEditValidator ID="MevFromDate" runat="server" ControlExtender="MeFromDate"
                                        ControlToValidate="txtfromDate" CssClass="field_error" Display="Dynamic" EmptyValueBlurredText="Date is required"
                                        EmptyValueMessage="Date is required" ErrorMessage="MeFromDate" InvalidValueBlurredMessage="Invalid Date"
                                        InvalidValueMessage="Invalid Date" TooltipMessage="Input a Date in Date/Month/Year"></cc1:MaskedEditValidator>
                                    <%--<ews:DatePicker ID="dpFromDate" runat="server" DateFormatString="dd/MM/yyyy" RegExErrorMessage="Please enter a date in the format: dd/mm/yyyy"
                            TabIndex="6" Width="185px"></ews:DatePicker>--%>
                                </td>
                                <td style="width: 80px">
                                    To Date:
                                </td>
                                <td style="width: 100px">
                                    <asp:TextBox ID="txtToDate" runat="server" CssClass="fiel_input" Width="60px" Text='<%# Bind("todate") %>'></asp:TextBox>
                                    <cc1:CalendarExtender ID="dpToDate" runat="server" Format="dd/MM/yyyy" PopupButtonID="ImgBtnToDt"
                                        TargetControlID="txtToDate" PopupPosition="Right">
                                    </cc1:CalendarExtender>
                                    <cc1:MaskedEditExtender ID="MeToDate" runat="server" Mask="99/99/9999" MaskType="Date"
                                        TargetControlID="txtToDate">
                                    </cc1:MaskedEditExtender>
                                    <asp:ImageButton ID="ImgBtnToDt" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.png" /><br />
                                    <cc1:MaskedEditValidator ID="MevToDate" runat="server" ControlExtender="MeToDate"
                                        ControlToValidate="txtToDate" CssClass="field_error" Display="Dynamic" EmptyValueBlurredText="Date is required"
                                        EmptyValueMessage="Date is required" ErrorMessage="MeToDate" InvalidValueBlurredMessage="Invalid Date"
                                        InvalidValueMessage="Invalid Date" TooltipMessage="Input a Date in Date/Month/Year"></cc1:MaskedEditValidator>
                                    <%--<ews:DatePicker ID="dpToDate" runat="server" DateFormatString="dd/MM/yyyy" RegExErrorMessage="Please enter a date in the format: dd/mm/yyyy"
                            TabIndex="6" Width="185px"></ews:DatePicker>--%>
                                </td>
                                <td>
                                    <asp:Button ID="btnDisplay" runat="server" Text="Display" CssClass="btn" />
                                </td>
                            </tr>
                        </table>                        
                    </td>
                </tr>
                <tr>
                    <td colspan="6" style="height: 10px">
                    </td>
                </tr>
                <tr style="padding-top: 6px">
                    <td colspan="6">
                        <asp:Panel ID="pnlGrid" runat="server" Height="500px" CssClass="scrlbr">
                            <asp:GridView ID="grdTransferPost" TabIndex="10" runat="server" Font-Size="10px"
                                CssClass="td_cell" Width="100%" BackColor="White" AutoGenerateColumns="False"
                                BorderColor="#999999" BorderStyle="None" CellPadding="3" GridLines="Vertical">
                                <FooterStyle CssClass="grdfooter"></FooterStyle>
                                <Columns>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <input type="checkbox" runat="server" id="chkAll" onclick="f_grid_selectAll(this)" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <input type="checkbox" runat="server" id="chkItem" onclick='GetSelectedValue()' />
                                        </ItemTemplate>
                                        <ItemStyle VerticalAlign="middle" Width="1px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="TransactionID">
                                        <ItemTemplate>
                                            <asp:Label ID="lblTransactionID" runat="server" OnDataBinding="TemplateFieldBind"
                                                Text=""></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle VerticalAlign="middle" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Excursion ID">
                                        <ItemTemplate>
                                            <asp:Label ID="lblExcursionID" runat="server" OnDataBinding="TemplateFieldBind" Text=""></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle VerticalAlign="middle" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Transfer Type">
                                        <ItemTemplate>
                                            <asp:Label ID="lblTransType" runat="server" OnDataBinding="TemplateFieldBind" Text=""></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle VerticalAlign="middle" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Transfer Date">
                                        <ItemTemplate>
                                            <asp:Label ID="lblTransferDate" runat="server" OnDataBinding="TemplateFieldBind"
                                                Text=""></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle VerticalAlign="middle" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Pickup">
                                        <ItemTemplate>
                                            <asp:Label ID="lblPickupDate" runat="server" OnDataBinding="TemplateFieldBind" Text=""></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle VerticalAlign="middle" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Drop Off">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDepdate" runat="server" OnDataBinding="TemplateFieldBind" Text=""></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle VerticalAlign="middle" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Car Type">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCarType" runat="server" OnDataBinding="TemplateFieldBind" Text=""></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle VerticalAlign="middle" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Supplier Code">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSupplierCode" runat="server" OnDataBinding="TemplateFieldBind"
                                                Text=""></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle VerticalAlign="middle" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Supplier Name">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSupplierName" runat="server" OnDataBinding="TemplateFieldBind"
                                                Text=""></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle VerticalAlign="Middle" Width="80px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="InHouse">
                                        <ItemTemplate>
                                            <asp:Label ID="lblInhousesuppler" runat="server" OnDataBinding="TemplateFieldBind"
                                                Text=""></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle VerticalAlign="middle" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Cost Value">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtCostValue" runat="server" OnDataBinding="TemplateFieldBind" Text=""
                                                Width="50px"></asp:TextBox>
                                        </ItemTemplate>
                                        <ItemStyle VerticalAlign="middle" />
                                    </asp:TemplateField>
                                </Columns>
                                <FooterStyle CssClass="grdfooter" />
                                <RowStyle CssClass="grdRowstyle"></RowStyle>
                                <SelectedRowStyle CssClass="grdselectrowstyle"></SelectedRowStyle>
                                <PagerStyle CssClass="grdpagerstyle" HorizontalAlign="Center"></PagerStyle>
                                <HeaderStyle CssClass="grdheader" ForeColor="white"></HeaderStyle>
                                <AlternatingRowStyle CssClass="grdAternaterow"></AlternatingRowStyle>
                            </asp:GridView>
                        </asp:Panel>
                    </td>
                </tr>
            </table>
            <table>
                <tr style="padding-top:8px">
                    <td class="style1">
                        <b>Total:</b>
                    </td>
                    <td>
                        <input id="txtTotal" type="text" style="font-weight:bold; width: 116px; height:20px; font-size:14px;" runat="server" class="txtbox" readonly="readonly" />
                    </td>
                </tr>
                <tr>
                    <td class="style3" colspan="6">
                        <asp:Button ID="btnSave" runat="server" OnClientClick="validaterows()" Text="Save" CssClass="btn" />
                        &nbsp;<asp:Button ID="btnExit" runat="server" Text="Exit" CssClass="btn" />
                    </td>
                </tr>
            </table>
            <table>
                <tr>
                    <td class="style2">
                        <asp:HiddenField ID="hdnTotalVal" runat="server" />
                        <asp:HiddenField ID="hdnCurInd" runat="server" />
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server">
        <Services>
            <asp:ServiceReference Path="~/clsServices.asmx"></asp:ServiceReference>
        </Services>
    </asp:ScriptManagerProxy>
</asp:Content>
