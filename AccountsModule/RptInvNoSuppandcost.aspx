<%@ Page Title="" Language="VB" MasterPageFile="~/AccountsMaster.master" AutoEventWireup="false"
    CodeFile="RptInvNoSuppandcost.aspx.vb" Inherits="AccountsModule_RptInvNoSuppandcostSearch" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="EclipseWebSolutions.DatePicker" Namespace="EclipseWebSolutions.DatePicker"
    TagPrefix="ews" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="Server">
    <script src="../Content/js/jquery-1.5.1.min.js" type="text/javascript"></script>
    <script src="../Content/js/JqueryUI.js" type="text/javascript"></script>
    <script src="../Content/js/accounts.js" type="text/javascript"></script>
    <link type="text/css" href="../Content/css/JqueryUI.css" rel="Stylesheet" />
    <script language="javascript" type="text/javascript">



 
    </script>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table style="border-right: gray 1px solid; border-top: gray 1px solid; border-left: gray 1px solid;
                border-bottom: gray 1px solid">
                <tbody>
                    <tr>
                        <td style="text-align: center" class=" field_heading" colspan="5">
                            With out Supplier Selection
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <table style="width: 562px; height: 201px" class="td_cell">
                                <tbody>
                                    <tr>
                                        <td colspan="4">
                                            <table style="width: 653px; height: 179px">
                                                <tbody>                                                    
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblFromDate" runat="server" Text="From Date" Width="120px" CssClass="field_caption"></asp:Label>
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
                                                            <asp:Label ID="lblToDate" runat="server" Text="To Date" Width="120px" CssClass="field_caption"></asp:Label>
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
                                                            <asp:Label ID="Label6" runat="server" Text="Supplier" Width="120px" CssClass="field_caption"></asp:Label>
                                                        </td>
                                                        <td colspan="3">
                                                            <input type="text" name="suppSearch" class="field_input MyAutosupplierCompleteClass"
                                                                onfocus="MyAutoSupp_rptFillArray();" style="width: 50%; font" id="suppSearch"
                                                                runat="server" />
                                                            <select style="width: 50%" id="ddlSupplier" class="drpdown MyDropDownListSuppValue"
                                                                tabindex="6" runat="server">
                                                                <option selected></option>
                                                            </select>
                                                        </td>                                                        
                                                    </tr>

                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="Label1" runat="server" Text="Booking Type" Width="120px" CssClass="field_caption"></asp:Label>
                                                        </td>
                                                        <td colspan="3">                                                            
                                                            <asp:DropDownList ID="cbobooktype" runat="server" CssClass="drpdown">
                                                                <asp:ListItem Text="All" Value="0" Selected="True"></asp:ListItem>
                                                                <asp:ListItem Text="Invoiced" Value="1"></asp:ListItem>
                                                                <asp:ListItem Text="NotInvoiced" Value="2"></asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>                                                        
                                                    </tr>

                                                    <tr>
                                                        <td align="right" colspan="4">
                                                            <asp:Button ID="btnLoadreport" TabIndex="12" runat="server" Text="Load Reports "
                                                                CssClass="btn"></asp:Button>
                                                            &nbsp;
                                                            <asp:Button ID="btnClear" TabIndex="13" runat="server" Text="Clear" Font-Bold="False"
                                                                CssClass="btn"></asp:Button>&nbsp;
                                                            <asp:Button ID="btnExit" TabIndex="14" runat="server" Text=" Exit" CssClass="btn"
                                                                CausesValidation="False"></asp:Button>
                                                            &nbsp;
                                                            <asp:Button ID="btnhelp" TabIndex="15" runat="server" Text="Help" Height="20px" CssClass="btn">
                                                            </asp:Button>
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
