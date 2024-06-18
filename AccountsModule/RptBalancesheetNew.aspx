<%@ Page Language="VB" MasterPageFile="~/AccountsMaster.master" AutoEventWireup="false" CodeFile="RptBalancesheetNew.aspx.vb" Inherits="RptBalancesheetNew" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>


<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="Server">


    <table style="border-right: gray 1px solid; border-top: gray 1px solid; border-left: gray 1px solid; width: 100%; border-bottom: gray 1px solid" class="td_cell" align="left">
        <tbody>
            <tr>
                <td style="width: 100%" class="field_heading" align="center" colspan="5">
                    <asp:Label ID="lblHeading" runat="server" Text="Report Balance Sheet New" CssClass="field_heading" Width="100%"></asp:Label></td>
            </tr>

            <tr>
                <td style="width: 100%" class="td_cell" align="left" colspan="5">
                    <table style="width: 100%" class="td_cell" align="left">
                        <tbody>
                            <tr>
                                <td style="width: 77px" class="td_cell" align="left">As on Date</td>
                                <td class="td_cell" valign="middle" align="left">
                                    <asp:TextBox ID="txtFromDate" TabIndex="1" runat="server" CssClass="txtbox"
                                        Width="96px"></asp:TextBox>&nbsp;
                                    <asp:ImageButton ID="ImgBtnFrmDt" TabIndex="2" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.png"></asp:ImageButton>&nbsp;<cc1:MaskedEditValidator ID="MskVFromDt" runat="server" ControlExtender="MskFromDate" ControlToValidate="txtFromDate" CssClass="field_error" Display="Dynamic" EmptyValueBlurredText="*" EmptyValueMessage="Date is required" ErrorMessage="MskVFromDate" InvalidValueBlurredMessage="*" InvalidValueMessage="Invalid Date" TooltipMessage="Input a date in dd/mm/yyyy format" Width="1px"></cc1:MaskedEditValidator></td>
                            </tr>
                            <tr style="display: none">
                                <td style="width: 77px" class="td_cell" align="left">Level</td>
                                <td class="td_cell" valign="middle" align="left">
                                    <asp:DropDownList Style="display: none" ID="ddlselect" TabIndex="3" runat="server" CausesValidation="True"
                                        CssClass="drpdown">
                                    </asp:DropDownList></td>
                                <td style="width: 180px; display: None" colspan="2">

                                    <asp:CheckBox ID="chknew" CssClass="td_cell" AutoPostBack="True"
                                        runat="server" Font-Bold="True" Text="New Format" Checked="True" />

                                </td>
                            </tr>




                            <tr>
                                <td class="td_cell" align="left"></td>
                                <td class="td_cell"
                                    valign="middle" align="center" colspan="5">
                                    <asp:Button ID="btnLoadReprt" Style="display: none" OnClick="btnLoadReprt_Click" runat="server"
                                        Text="Load Report" TabIndex="4" CssClass="btn"></asp:Button>&nbsp;<asp:Button ID="btnPdfReport" OnClick="btnPdfReport_Click" runat="server" Style="display: none;"
                                            Text="Pdf Report" TabIndex="4" CssClass="btn"></asp:Button>&nbsp;<asp:Button ID="btnExcelReport" OnClick="btnExcelReport_Click" runat="server"
                                                Text="Excel Report" TabIndex="4" CssClass="btn"></asp:Button>&nbsp;</td>
                                <td></td>
                                <td></td>
                            </tr>
                            <tr>
                                <td style="width: 77px">
                                    <asp:Button ID="Button1" runat="server" Text="Export" CssClass="btn"
                                        OnClick="Button1_Click1" Visible="False"></asp:Button></td>
                                <td align="right">&nbsp;<asp:Button
                                    ID="btnhelp" OnClick="btnhelp_Click" runat="server" Text="Help"
                                    CssClass="btn" Visible="False"></asp:Button></td>
                                <td style="display: none">
                                    <asp:Button ID="btnadd" TabIndex="16" runat="server"
                                        CssClass="field_button"></asp:Button>
                                </td>

                                <asp:GridView ID="gv_SearchResult" runat="server" AutoGenerateColumns="False"
                                    BackColor="White" BorderColor="#999999" BorderStyle="None"
                                    CssClass="td_cell" Font-Size="10px">
                                </asp:GridView>




                            </tr>
                        </tbody>
                    </table>
                </td>
            </tr>
            <tr>
                <td style="width: 1003px" colspan="5">
                    <asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server">
                        <Services>
                            <asp:ServiceReference Path="~/clsServices.asmx"></asp:ServiceReference>
                        </Services>
                    </asp:ScriptManagerProxy>
                    <cc1:CalendarExtender ID="ClsExFromDate" runat="server"
                        Format="dd/MM/yyyy" PopupButtonID="ImgBtnFrmDt" TargetControlID="txtFromDate"></cc1:CalendarExtender>
                    <cc1:MaskedEditExtender ID="MskFromDate" runat="server" AcceptNegative="Left" DisplayMoney="Left"
                        ErrorTooltipEnabled="True" Mask="99/99/9999" MaskType="Date" MessageValidatorTip="true"
                        TargetControlID="txtFromDate"></cc1:MaskedEditExtender>
                </td>
            </tr>
        </tbody>
    </table>

    <script language="javascript" type="text/javascript">



        function TimeOutHandler(result) {
            alert("Timeout :" + result);
        }

        function ErrorHandler(result) {
            var msg = result.get_exceptionType() + "\r\n";
            msg += result.get_message() + "\r\n";
            msg += result.get_stackTrace();
            alert(msg);
        }


    </script>
</asp:Content>

