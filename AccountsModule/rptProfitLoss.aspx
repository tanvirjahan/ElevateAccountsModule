<%@ Page Language="VB" MasterPageFile="~/AccountsMaster.master"  AutoEventWireup="false" CodeFile="rptProfitLoss.aspx.vb" Inherits="AccountsModule_rptProfitLoss"  %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">
<script language="javascript" type="text/javascript" >
    function ChangeDate() {

        var txtfdate = document.getElementById("<%=txtFromDate.ClientID%>");
        var txttdate = document.getElementById("<%=txtToDate.ClientID%>");

        if (txtfdate.value == '') { alert("Enter From Date."); txtfdate.focus(); }
        else { txttdate.value = txtfdate.value; }
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

    function showclosing() {
        btn = document.getElementById("<%=btnclose.ClientID%>");
        btn.click()
    }

</script>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <table style="border-right: gray 1px solid; border-top: gray 1px solid; border-left: gray 1px solid;
                width: 100%; border-bottom: gray 1px solid" class="td_cell" align="left">
                <tbody>
                    <tr>
                        <td style="text-align: center" colspan="4" class="field_heading">
                            <strong><span id="spanHeading" runat="server" class="field_heading">Income Statement</span></strong>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 13px">
                            <span style="font-size: 8pt; font-family: Arial">From Date</span> &nbsp<asp:TextBox
                                ID="txtFromDate" TabIndex="1" runat="server" Width="96px" CssClass="txtbox"></asp:TextBox><asp:ImageButton
                                    ID="ImgBtnFrmDt" TabIndex="1" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.png">
                                </asp:ImageButton>
                            <cc1:MaskedEditValidator ID="MskVFromDt" runat="server" Width="1px" TooltipMessage="Input a date in dd/mm/yyyy format"
                                InvalidValueMessage="Invalid Date" InvalidValueBlurredMessage="*" ErrorMessage="MskVFromDate"
                                EmptyValueMessage="Date is required" EmptyValueBlurredText="*" Display="Dynamic"
                                CssClass="field_error" ControlToValidate="txtFromDate" ControlExtender="MskFromDate"></cc1:MaskedEditValidator>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 13px">
                            <span style="font-size: 8pt; font-family: Arial">To Date</span>&nbsp;&nbsp;&nbsp;&nbsp
                            <asp:TextBox ID="txttoDate" TabIndex="3" runat="server" Width="96px" CssClass="txtbox"></asp:TextBox><asp:ImageButton
                                ID="ImgBtntoDt" runat="server" TabIndex="4" ImageUrl="~/Images/Calendar_scheduleHS.png">
                            </asp:ImageButton>
                            <cc1:MaskedEditValidator ID="MskVtoDt" runat="server" Width="1px" TooltipMessage="Input a date in dd/mm/yyyy format"
                                InvalidValueMessage="Invalid Date" InvalidValueBlurredMessage="*" ErrorMessage="MskVFromDate"
                                EmptyValueMessage="Date is required" EmptyValueBlurredText="*" Display="Dynamic"
                                CssClass="field_error" ControlToValidate="txttoDate" ControlExtender="MsktoDate"></cc1:MaskedEditValidator>
                        </td>
                        <td>
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td style="width: 176px">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 13px">
                            <span style="font-size: 8pt; font-family: Arial">
                                <asp:Label Text="Select Level" ID="lbllevel" runat="server"></asp:Label></span>
                        </td>
                        <td style="width: 91px">
                            <asp:DropDownList ID="ddlselect" runat="server" Width="98px" CssClass="drpdown">
                            </asp:DropDownList>
                        </td>
                        <td style="width: 91px">
                            &nbsp;
                        </td>
                        <td style="width: 176px">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 13px">
                            <span style="font-size: 8pt; font-family: Arial">
                                <asp:Label Text="Report type" ID="lblreporttype" runat="server"></asp:Label></span>
                        </td>
                        <td style="width: 91px">
                            <select style="width: 115px" id="ddlrpttype" class="drpdown" runat="Server">
                                <option value="[Select]" selected>[Select]</option>
                                <option value="Month">MonthWise</option>
                            </select>
                        </td>
                        <td style="width: 91px">
                            &nbsp;
                        </td>
                        <td style="width: 176px">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 13px" class="td_cell">
                            <asp:Label ID="Label3" runat="server" Text="Closing Entry" Width="103px"></asp:Label>
                        </td>
                        <td class="td_cell" style="width: 91px">
                            <asp:DropDownList ID="ddlclosing" runat="server" CssClass="drpdown" TabIndex="6">
                                <asp:ListItem Value="0">Include Closing Entry</asp:ListItem>
                                <asp:ListItem Value="1">Exclude Closing Entry</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td class="td_cell" style="width: 91px">
                            &nbsp;
                        </td>
                        <td class="td_cell" style="width: 176px">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 13px" class="td_cell">
                            <input type="checkbox" tabindex="5" id="chkmonth" runat="server" />
                            <label id="lblMonth" runat="server" class="field_caption">Monthwise</label>                             
                        </td>
                        <td style="width: 13px" class="td_cell">
                            <input type="checkbox" tabindex="6" id="ChkRatio" runat="server" style="display:none" />
                            <label id="lblRatio" runat="server" class="field_caption" style="display:none">RatioWise</label>                                 
                        </td>
                    </tr>
                    <tr>
                        <td style="display: none; width: 13px;">
                            <asp:Button ID="Button1" runat="server" __designer:dtid="2533274790395978" __designer:wfdid="w2"
                                CssClass="btn" OnClick="Button1_Click" Text="Export" />
                        </td>
                        <td align="center" colspan="5">
                            <asp:Button ID="btnLoadReprt" TabIndex="6" runat="server" CssClass="btn" OnClick="btnLoadReprt_Click"
                                Style="display: none" Text="Load Report" />
                            &nbsp;
                            <asp:Button ID="btnPdfReprt" TabIndex="6" runat="server" CssClass="btn" OnClick="btnPdfReprt_Click"
                                Text="Pdf Report" />
                            &nbsp;
                            <asp:Button ID="btnExcelReprt" TabIndex="7" runat="server" CssClass="btn" OnClick="btnExcelReprt_Click"
                                Text="Excel Report"   />
                            <asp:Button ID="btnclose" runat="server" Text="close" CssClass="btn" Style="display: none" />
                            &nbsp;
                            <asp:Button ID="btnHelp"    Visible="false"  runat="server" CssClass="btn" Height="20px" OnClick="btnHelp_Click"
                                TabIndex="7" Text="Help" />
                            <asp:Button ID="btnadd"  Visible="false" runat="server" CssClass="field_button" TabIndex="16" />
                        </td>
                        <asp:GridView ID="gv_SearchResult" runat="server" AutoGenerateColumns="False" BackColor="White"
                            BorderColor="#999999" BorderStyle="None" CssClass="td_cell" Font-Size="10px">
                        </asp:GridView>
                    </tr>
                </tbody>
            </table>
            <cc1:CalendarExtender ID="ClsExFromDate" runat="server" TargetControlID="txtFromDate"
                PopupButtonID="ImgBtnFrmDt" Format="dd/MM/yyyy">
            </cc1:CalendarExtender>
            <cc1:MaskedEditExtender ID="MskFromDate" runat="server" TargetControlID="txtFromDate"
                MessageValidatorTip="true" MaskType="Date" Mask="99/99/9999" ErrorTooltipEnabled="True"
                DisplayMoney="Left" AcceptNegative="Left">
            </cc1:MaskedEditExtender>
            <cc1:MaskedEditExtender ID="MsktoDate" runat="server" TargetControlID="txttoDate"
                MessageValidatorTip="true" MaskType="Date" Mask="99/99/9999" ErrorTooltipEnabled="True"
                DisplayMoney="Left" AcceptNegative="Left">
            </cc1:MaskedEditExtender>
            <cc1:CalendarExtender ID="ClsExtoDate" runat="server" TargetControlID="txttoDate"
                PopupButtonID="ImgBtntoDt" Format="dd/MM/yyyy">
            </cc1:CalendarExtender>
            <br />
            <br />
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>

