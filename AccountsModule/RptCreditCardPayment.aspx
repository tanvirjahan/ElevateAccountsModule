<%@ Page Title="" Language="VB" MasterPageFile="~/AccountsMaster.master" AutoEventWireup="false" CodeFile="RptCreditCardPayment.aspx.vb" Inherits="AccountsModule_RptCreditCardPayment" %>
<%@ Register Assembly="EclipseWebSolutions.DatePicker" Namespace="EclipseWebSolutions.DatePicker"
    TagPrefix="ews" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ OutputCache Location="none" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

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
    </script>

 <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>

 <table style="border-right: gray 2px solid; border-top: gray 2px solid; border-left: gray 2px solid;
        border-bottom: gray 2px solid" width="1000px" height="180px">
        <tbody>
        <tr>
            <td align="center" class="field_heading">
                <asp:Label ID="lblHeading" runat="server" CssClass="field_heading" Text="Credit Card Payment Details"
                    Width="698px"></asp:Label>
            </td>
        </tr>

        <tr>
        
        <td>

        <table>
        
        <tbody>
        
        
        <tr>
                                                    <td>
                                                        <asp:Label ID="lblFromdate" runat="server" Text="From Date" Width="110px"
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
                                                        <asp:Label ID="lblTodate" runat="server" Text="To Date" ForeColor="Black"
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
         <asp:Label ID="lblAgent" runat="server" Text="Agent" Width="120px" CssClass="field_caption"></asp:Label>
         </td>
          <td>
           <input type="text" name="accSearch" class="field_input MyAutoCompleteClass" onfocus="MyAutoCustomer_rptFillArray();"
            style="width: 98%; font" id="accSearch" runat="server" />
            <select style="width: 200px" id="ddlAgent" class="drpdown MyDropDownListCustValue"
            tabindex="6" runat="server">
          <option selected></option>
            </select>
        </td>
        
        </tr>


     <tr>
        <td align="right" colspan="4">
        <asp:Button ID="btnLoadreport" TabIndex="4" OnClick="btnLoadreport_Click" runat="server"
         Text="Load Reports " CssClass="btn"></asp:Button>
         &nbsp;
     <asp:Button ID="btnExit" TabIndex="5" OnClick="btnExit_Click" runat="server" Text=" Exit"
        CssClass="btn" CausesValidation="False"></asp:Button>
        &nbsp;
        <input id="txtconnection" runat="server" style="visibility: hidden; width: 12px;
                                                height: 9px" type="text" />
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
        
        
        </td>
        
        
        </tr>

        </tbody>
        </table>

          </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

