<%@ Page Title="" Language="VB"  AutoEventWireup="false" CodeFile="BookingStatus.aspx.vb" Inherits="BookingStatus"   MasterPageFile="~/MainPageMaster.master"  Strict="true" EnableEventValidation="false" %>

<%@ Register Assembly="EclipseWebSolutions.DatePicker" Namespace="EclipseWebSolutions.DatePicker"
    TagPrefix="ews" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ OutputCache Location="none" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="Server">
    <table style="border-right: gray 2px solid; border-top: gray 2px solid; border-left: gray 2px solid;
        border-bottom: gray 2px solid" width="1000px" height="180px">
        <tr>
            <td align="center" class="field_heading">
                <asp:Label ID="lblHeading" runat="server" CssClass="field_heading" Text=" View Excel"
                    Width="698px"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
               <%-- <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <table>
                            <tbody>--%>
                                <%--<tr>
                                    <td>--%>
                                        <table>
                                            <tbody>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblFromdate" runat="server" Text="From Arrival Date" Width="110px"
                                                            CssClass="field_caption"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtFromDate" TabIndex="1" runat="server" Width="80px" CssClass="txtbox"></asp:TextBox>
                                                        <asp:ImageButton ID="ImgBtnFrmDt" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.png">
                                                        </asp:ImageButton>
                                                        <cc1:MaskedEditValidator ID="MEVFromDate" runat="server" CssClass="field_error" TooltipMessage="Input a date in dd/mm/yyyy format"
                                                            InvalidValueMessage="Invalid Date" InvalidValueBlurredMessage="Invalid Date"
                                                            EmptyValueMessage="Date is required" EmptyValueBlurredText="Date is required"
                                                            Display="Dynamic" ControlToValidate="txtFromDate" ControlExtender="MEFromDate"></cc1:MaskedEditValidator>
                                                            &nbsp; &nbsp;&nbsp;
                                                    </td>  
                                                    <td>
                                                        <asp:Label ID="lblTodate" runat="server" Text="To Arrival Date" ForeColor="Black"
                                                            Width="110px" CssClass="field_caption"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtToDate" TabIndex="2" runat="server" Width="80px" CssClass="txtbox"></asp:TextBox>
                                                        <asp:ImageButton ID="ImgBtnRevDate" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.png">
                                                        </asp:ImageButton>
                                                        <cc1:MaskedEditValidator ID="MEVToDate" runat="server" CssClass="field_error" TooltipMessage="Input a date in dd/mm/yyyy format"
                                                            InvalidValueMessage="Invalid Date" InvalidValueBlurredMessage="Invalid Date"
                                                            EmptyValueMessage="Date is required" EmptyValueBlurredText="Date is required"
                                                            Display="Dynamic" ControlToValidate="txtToDate" ControlExtender="METoDate"></cc1:MaskedEditValidator>
                                                    </td>
                                                </tr>
                                                <tr>
                                                 <td>
                                                            <asp:Label ID="lblBookType" runat="server" Text="Book Type" Width="62px" CssClass="field_caption"></asp:Label>
                                                        </td>
                                                        <td class="field_input">
                                                            <select style="width: 105px;" id="ddlbooktype" class="field_input" tabindex="0" runat="server">
                                                                <option value="0" selected>All</option>
                                                                <option value="1">FIT</option>
                                                                <option value="2">Groups</option>
                                                                <option value="3">Packages</option>
                                                                
                                                            </select>
                                                        </td>   
                                                </tr>
                                               <br /> <br />
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblMsg1" runat="server" Text="Records not found, Please redefine search criteria"
                                                            Font-Size="8pt" Font-Names="Verdana" Font-Bold="True" Width="357px" CssClass="lblmsg"
                                                            Visible="False"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <cc1:CalendarExtender ID="CEFromDate" runat="server" Format="dd/MM/yyyy" PopupButtonID="ImgBtnFrmDt"
                                                            TargetControlID="txtFromDate">
                                                        </cc1:CalendarExtender>
                                                        <cc1:MaskedEditExtender ID="MEFromDate" runat="server" TargetControlID="txtFromDate"
                                                            Mask="99/99/9999" MaskType="Date">
                                                        </cc1:MaskedEditExtender>
                                                        <cc1:CalendarExtender ID="CEToDate" runat="server" Format="dd/MM/yyyy" PopupButtonID="ImgBtnRevDate"
                                                            TargetControlID="txtToDate">
                                                        </cc1:CalendarExtender>
                                                        <cc1:MaskedEditExtender ID="METoDate" runat="server" TargetControlID="txtToDate"
                                                            Mask="99/99/9999" MaskType="Date">
                                                        </cc1:MaskedEditExtender>
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                 <%--   </td>
                                </tr>--%>
            </td>

                                <tr>
                                    <td>
                                    <asp:Button ID="btnExportToExcel" runat="server" CssClass="btn" tabIndex="3" 
                                     Text="Export To Excel" Width="106px" />
                                      
                                        <asp:Button ID="btnClear" TabIndex="3" runat="server" Text="Clear" Font-Bold="False"
                                            CssClass="btn"></asp:Button>
                                    </td>
                                </tr>
                    <%--        </tbody>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>--%>
        </tr>
    </table>
    
</asp:Content>
