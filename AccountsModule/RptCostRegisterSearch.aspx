<%@ Page Title="" Language="VB" MasterPageFile="~/AccountsMaster.master" AutoEventWireup="false" CodeFile="RptCostRegisterSearch.aspx.vb" Inherits="AccountsModule_RptCostRegisterSearch" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="EclipseWebSolutions.DatePicker" Namespace="EclipseWebSolutions.DatePicker"
    TagPrefix="ews" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">

<script src="../Content/js/jquery-1.5.1.min.js" type="text/javascript"></script>
    <script src="../Content/js/JqueryUI.js" type="text/javascript"></script>
    <script src="../Content/js/accounts.js" type="text/javascript"></script>
    <link type="text/css" href="../Content/css/JqueryUI.css" rel="Stylesheet" />
    <script language="javascript" type="text/javascript">

       
        function ChangeDate() {

            var txtfdate = document.getElementById("<%=txtFromDate.ClientID%>");
            if (txtfdate.value == '') { alert("Enter From Date."); txtfdate.focus(); }
            else { ColServices.clsServices.GetQueryReturnFromToDate('FromDate', 30, txtfdate.value, FillToDate, ErrorHandler, TimeOutHandler); }
        }
        function FillToDate(result) {
            var txttdate = document.getElementById("<%=txtToDate.ClientID%>");
            txttdate.value = result;
        }

        function TimeOutHandler(result) {
            alert("Timeout :" + result);
        }

        function ErrorHandler(result) {
            var msg = result.get_exceptionType() + "\r\n";
            msg += result.get_message() + "\r\n";
            msg += result.get_stackTrace();
            alert(msg);
        }
        function ValidateForm() {
            var txtfdate = document.getElementById("<%=txtFromDate.ClientID%>");
            var txttdate = document.getElementById("<%=txtToDate.ClientID%>");
            if (txtfdate.value == '') { alert("Enter From Date."); txtfdate.focus(); }
            else if (txttdate.value == '') { alert("Enter To Date."); txttdate.focus(); }
            else if (txtfdate.value > txttdate.value) { alert("To date should be greater than from dat."); txttdate.focus(); }

        }

        function ChangeRepType() {
            var lblSuppType = document.getElementById("<%=lblSuppType.ClientID%>");
            var ddlsupptype = document.getElementById("<%=ddlsupptype.ClientID%>");
            var ddlrpttype = document.getElementById("<%=ddlrpttype.ClientID%>");
            if (ddlrpttype.value == 1) {
                lblSuppType.style.display = 'none';
                ddlsupptype.style.display = 'none';
            } else {
                lblSuppType.style.display = 'block';
                ddlsupptype.style.display = 'block';
            }
        }

 
    </script>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table style="border-right: gray 1px solid; border-top: gray 1px solid; border-left: gray 1px solid;
                border-bottom: gray 1px solid">
                <tbody>
                    <tr>
                        <td style="text-align: center" class=" field_heading" colspan="5">
                            Cost Register&nbsp;Report
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <table style="width: 100%; height: 201px" class="td_cell">
                                <tbody>
                                    <tr>
                                        <td colspan="4">
                                            <table style="width: 653px; height: 179px">
                                                <tbody>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="Label1" runat="server" Text="Invoice No" Width="120px" CssClass="field_caption"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <input style="width: 194px" id="txtInvoiceNo" class="txtbox" tabindex="1" type="text"
                                                                runat="server" />
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="Label3" runat="server" Text="File Number" Width="120px" CssClass="field_caption"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <input style="width: 194px" id="txtRequestId" class="txtbox" tabindex="2" type="text"
                                                                runat="server" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="Label2" runat="server" Text="From Invoice Date" Width="120px" CssClass="field_caption"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtFromDate" TabIndex="3" runat="server" Width="80px" CssClass="txtbox"></asp:TextBox>&nbsp;
                                                            <asp:ImageButton ID="ImgBtnFrmDt" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.png">
                                                            </asp:ImageButton>
                                                            <cc1:MaskedEditValidator ID="MEVFromDate" runat="server" CssClass="field_error" ControlExtender="MEFromDate"
                                                                ControlToValidate="txtFromDate" Display="Dynamic" EmptyValueBlurredText="Date is required"
                                                                EmptyValueMessage="Date is required" InvalidValueBlurredMessage="Invalid Date"
                                                                InvalidValueMessage="Invalid Date" TooltipMessage="Input a date in dd/mm/yyyy format"></cc1:MaskedEditValidator>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="Label5" runat="server" Text="To Invoice Date" Width="120px" CssClass="field_caption"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtToDate" TabIndex="4" runat="server" Width="80px" CssClass="txtbox"></asp:TextBox>
                                                            <asp:ImageButton ID="ImgBtnRevDate" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.png">
                                                            </asp:ImageButton>
                                                            <cc1:MaskedEditValidator ID="MEVToDate" runat="server" CssClass="field_error" ControlExtender="METoDate"
                                                                ControlToValidate="txtToDate" Display="Dynamic" EmptyValueBlurredText="Date is required"
                                                                EmptyValueMessage="Date is required" InvalidValueBlurredMessage="Invalid Date"
                                                                InvalidValueMessage="Invalid Date" TooltipMessage="Input a date in dd/mm/yyyy format"></cc1:MaskedEditValidator>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="Label4" runat="server" Text="Status" Width="120px" CssClass="field_caption" Visible="false"></asp:Label>
                                                        </td>
                                                        <%--Added visible false for status--%>
                                                        <td>
                                                            <select style="width: 154px" id="ddlStatus" class="drpdown" tabindex="5" runat="server">
                                                                <option value="[Select]" selected>[Select]</option>
                                                                <option value="P">Posted</option>
                                                                <option value="U">UnPosted</option>
                                                            </select>
                                                        </td>
                                                        <td>
                                                        </td>
                                                        <td>
                                                        </td>
                                                    </tr>
                                                   
                                                     <tr>
                                                        <td>
                                                            <asp:Label ID="Label6" runat="server" Text="Supplier" Width="120px" CssClass="field_caption"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <input type="text" name="suppSearch" class="field_input MyAutosupplierCompleteClass" onfocus="MyAutoSupp_rptFillArray();"
                                                                style="width: 98%; font" id="suppSearch" runat="server" /> 
                                                            <select style="width: 200px" id="ddlSupplier" class="drpdown MyDropDownListSuppValue"
                                                                tabindex="6" runat="server">
                                                                <option selected></option>
                                                            </select>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblSuppType" runat="server" Text="Supplier Type" Width="62px"></asp:Label>
                                                        </td>
                                                        <td class="field_input">
                                                            <select style="width: 105px;" id="ddlsupptype" class="field_input" tabindex="0" runat="server">
                                                                <option value="0" selected>All</option>
                                                                <option value="1">Hotel</option>
                                                                <option value="2">Transfer</option>
                                                                <option value="3">Excursions</option>
                                                                <option value="4">Others</option>
                                                            </select>
                                                        </td>
                                                    </tr>
                                                    <%--Added suppSearch by Archana on 09-04-2015--%>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="Label7" runat="server" Text="From Amount" Width="120px" CssClass="field_caption" Visible="false"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <input id="txtFromAmount" class="txtbox" tabindex="8" type="text" runat="server" />
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="Label10" runat="server" Text="To Amount" Width="120px" CssClass="field_caption" visible="false"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <input id="txtToAmount" class="txtbox" tabindex="9" type="text" runat="server" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="Label11" runat="server" Text="Report Type" Width="120px" CssClass="field_caption" Visible="True"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <select style="width: 165px" id="ddlrpttype" class="field_input" tabindex="11" 
                                                                runat="server">
                                                                <option value="0" selected>Supplierwise</option>
                                                                <option value="1">Marketwise</option>
                                                            </select>
                                                            <%--<asp:DropDownList ID="ddlrpttype" TabIndex="11" runat="server" Width="207px" CssClass="drpdown">
                                                                <asp:ListItem Value="0">Brief</asp:ListItem>
                                                                <asp:ListItem Value="1">Detailed</asp:ListItem>
                                                            </asp:DropDownList>--%>
                                                        </td>

                                                         <td>
                                                            <asp:Label ID="Label9" runat="server" Text="Supplier Ref" Width="120px" CssClass="field_caption" Visible="false"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <input id="txtSuppRef" class="txtbox" tabindex="7" type="text" runat="server" />
                                                        </td>
                                                       
                                                    </tr>
                                                    <tr>
                                                        <td align="right" colspan="4">
                                                            <asp:Button ID="btnLoadreport" TabIndex="12" OnClick="btnLoadreport_Click" runat="server"
                                                                Text="Load Reports " CssClass="btn"></asp:Button>
                                                            &nbsp;
                                                            <asp:Button ID="btnClear" TabIndex="13" OnClick="btnClear_Click" runat="server" Text="Clear"
                                                                Font-Bold="False" CssClass="btn"></asp:Button>&nbsp;
                                                            <asp:Button ID="btnExit" TabIndex="14" OnClick="btnExit_Click" runat="server" Text=" Exit"
                                                                CssClass="btn" CausesValidation="False"></asp:Button>
                                                            &nbsp;
                                                            <asp:Button ID="btnhelp" TabIndex="15" OnClick="btnhelp_Click" runat="server" Text="Help"
                                                                Height="20px" CssClass="btn"></asp:Button>
                                                        </td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                            <input id="txtconnection" runat="server" style="visibility: hidden; width: 12px;
                                                height: 9px" type="text" />
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                            <cc1:CalendarExtender ID="CEFromDate" runat="server" TargetControlID="txtFromDate"
                                PopupButtonID="ImgBtnFrmDt" Format="dd/MM/yyyy">
                            </cc1:CalendarExtender>
                            <cc1:MaskedEditExtender ID="MEFromDate" runat="server" TargetControlID="txtFromDate"
                                MaskType="Date" Mask="99/99/9999">
                            </cc1:MaskedEditExtender>
                            <cc1:CalendarExtender ID="CEToDate" runat="server" TargetControlID="txtToDate" PopupButtonID="ImgBtnRevDate"
                                Format="dd/MM/yyyy">
                            </cc1:CalendarExtender>
                            <cc1:MaskedEditExtender ID="METoDate" runat="server" TargetControlID="txtToDate"
                                MaskType="Date" Mask="99/99/9999">
                            </cc1:MaskedEditExtender>
                        </td>
                    </tr>
                </tbody>
            </table>
            <asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server">
                <Services>
                    <asp:ServiceReference Path="~/clsServices.asmx"></asp:ServiceReference>
                </Services>
            </asp:ScriptManagerProxy>
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>

