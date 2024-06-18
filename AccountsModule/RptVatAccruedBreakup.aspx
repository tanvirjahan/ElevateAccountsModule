<%@ Page Language="VB" MasterPageFile="~/AccountsMaster.master" AutoEventWireup="false" CodeFile="RptVatAccruedBreakup.aspx.vb" Inherits="AccountsModule_RptVatAccrueBreakup"  %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">
    <script language="javascript" type="text/javascript">
    function ChangeDate() {

        var txtfdate = document.getElementById("<%=txtFromDate.ClientID%>");
        if (txtfdate.value == '') {
            alert("Enter From Date."); txtfdate.focus();
        }
    }
</script>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table style="border-right: gray 2px solid; border-top: gray 2px solid; border-left: gray 2px solid; border-bottom: gray 2px solid">
                <tr>
                    <td align="center" class="field_heading" style="width: 100px; height: 21px">
                        <asp:Label ID="lblHeading" runat="server" CssClass="field_heading" Text="Vat Accrued Breakup"
                            Width="388px"></asp:Label></td>
                </tr>
                <tr>
                    <td style="width: 100px; height: 65px;">
            <table style="width: 453px">
                <tr>
                    <td style="width: 20px; height: 24px;">
                        <asp:Label ID="Label1" runat="server" Text="As On Date" 
                            Width="100px" CssClass="td_cell"></asp:Label></td>
                    <td style="width: 283px; height: 24px;">
                        &nbsp;<asp:TextBox ID="txtFromDate" runat="server" CssClass="txtbox" 
                            Width="80px"></asp:TextBox>
                        <asp:ImageButton ID="ImgBtnFrmDt" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.png" />
                        <cc1:maskededitvalidator id="MEVFromDate" runat="server" controlextender="MEFromDate"
                            controltovalidate="txtFromDate" cssclass="field_error" display="Dynamic" emptyvalueblurredtext="Date is required"
                            emptyvaluemessage="Date is required" invalidvalueblurredmessage="Invalid Date"
                            invalidvaluemessage="Invalid Date" tooltipmessage="Input a date in dd/mm/yyyy format"></cc1:maskededitvalidator>
                    </td>
                    <td style="width: 100px; height: 24px;">
                    </td>
                </tr>
                <tr>
                    <td style="width: 20px">
                    </td>
                    <td style="width: 283px">
                    </td>
                    <td style="width: 100px">
                    </td>
                </tr>
            </table>
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px">
                        <table style="width: 442px">
                            <tr>
                                <td style="width: 100px;">
                                <asp:Button ID="btnvataccrued" runat="server" CssClass="btn" Text="Excel Report" 
                                        OnClick="btnvataccrued_Click" />
                                    <%--<div id="DIV1" runat="server" 
                                        style="width: 343px; height: 112px; background-color: lavender" class="td_cell">
                                    </div>--%>
                                </td>
                            </tr>
                        </table>
                        <table style="width: 444px">
                            <tr>
                                <td style="width: 100px">
                                </td>
                        <%--        <td style="width: 100px">
                                    <asp:Button ID="btnseal" runat="server" CssClass="btn" Text="UnSeal Data" 
                                        OnClick="btnseal_Click" /></td>--%>
                                <td style="width: 100px">
                                    <asp:TextBox ID="txtpdate" runat="server" Visible="False" Width="1px"></asp:TextBox></td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px">
                        <cc1:calendarextender id="CEFromDate" runat="server" format="dd/MM/yyyy" popupbuttonid="ImgBtnFrmDt"
                            targetcontrolid="txtFromDate"></cc1:calendarextender>
                        <cc1:maskededitextender id="MEFromDate" runat="server" targetcontrolid="txtFromDate" AcceptNegative="Left" DisplayMoney="Left" ErrorTooltipEnabled="True" Mask="99/99/9999" MaskType="Date" MessageValidatorTip="true"></cc1:maskededitextender>
                    </td>
                </tr>
            </table>
            <asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server">
                <Services>
                    <asp:ServiceReference Path="~/clsServices.asmx" />
                </Services>
            </asp:ScriptManagerProxy>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

