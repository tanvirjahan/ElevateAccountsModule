<%@ Page Language="VB" MasterPageFile="~/AccountsMaster.master"   AutoEventWireup="false" CodeFile="ReservationInvoiceSearch.aspx.vb"
    Inherits="ReservationInvoiceSearch"    %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="EclipseWebSolutions.DatePicker" Namespace="EclipseWebSolutions.DatePicker"
    TagPrefix="ews" %>
<asp:Content ContentPlaceHolderID="Main" runat="server">

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

        function openwindow(reqlist,chkrow) {
            var i;
            var reqid = reqlist.split("|");
             for (i = 0; i <= chkrow - 1; i++) {
                window.open('rptConfirmaton.aspx?reqid=' + reqid[i] + '&typ=Invoice', "_blank", 'PopUp', i, 'width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');                        
            }
        }

//        Added openwindow function by Archana on 13/05/2015 for opening multiple windows for button report

    </script>
    <table style="border-right: gray 1px solid; border-top: gray 1px solid; border-left: gray 1px solid;
        border-bottom: gray 1px solid" id="table1">
        <tbody>
            <tr>
                <td style="height: 2px">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <table class="td_cell" width="100%">
                                <tbody>
                                    <tr>
                                        <td style="text-align: center" class="field_heading" colspan="4">
                                            Reservation Invoice List
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center" colspan="4">
                                            <asp:RadioButton ID="rbsearch" TabIndex="1" runat="server" Text="Search" ForeColor="Black"
                                                Width="60px" CssClass="td_cell" OnCheckedChanged="rbsearch_CheckedChanged" Checked="True"
                                                GroupName="GrSearch" AutoPostBack="True"></asp:RadioButton>
                                            <asp:RadioButton ID="rbnadsearch" TabIndex="2" runat="server" Text="Advance Search"
                                                ForeColor="Black" Width="139px" CssClass="td_cell" OnCheckedChanged="rbnadsearch_CheckedChanged"
                                                GroupName="GrSearch" AutoPostBack="True" wfdid="w6"></asp:RadioButton><asp:Button
                                                    ID="btnSearch" TabIndex="12" runat="server" Text="Search" Font-Bold="False" Width="48px"
                                                    CssClass="search_button"></asp:Button>&nbsp;
                                            <asp:Button ID="btnClear" TabIndex="13" runat="server" Text="Clear" Font-Bold="False"
                                                CssClass="search_button"></asp:Button>&nbsp;<asp:Button ID="btnHelp" TabIndex="14"
                                                    OnClick="btnHelp_Click" runat="server" Text="Help" CssClass="search_button">
                                            </asp:Button>
                                            &nbsp;
                                            <asp:Button ID="btnAddNew" TabIndex="15" runat="server" Text="Add New" Font-Bold="False"
                                                CssClass="btn"></asp:Button>
                                            &nbsp;
                                            <asp:Button ID="btnPrint" TabIndex="16" runat="server" Text="Report" CssClass="btn"
                                                Visible="False"></asp:Button>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4">
                                            <table>
                                                <tbody>
                                                    <tr>
                                                        <td style="width: 130px">
                                                            <asp:Label ID="Label1" runat="server" Text="Invoice No" CssClass="field_caption"></asp:Label>
                                                        </td>
                                                        <td style="width: 242px">
                                                            <input style="width: 194px" id="txtInvoiceNo" class="txtbox" tabindex="1" type="text"
                                                                runat="server" />
                                                        </td>
                                                        <td style="width: 130px">
                                                            <asp:Label ID="Label3" runat="server" Text="File Number " Width="64px" CssClass="field_caption"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <input style="width: 194px" id="txtRequestId" class="filed_input" tabindex="2" type="text"
                                                                runat="server" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 130px">
                                                            <asp:Label ID="Label2" runat="server" Text="From Invoice Date" CssClass="field_caption"></asp:Label>
                                                        </td>
                                                        <td style="width: 242px">
                                                            <asp:TextBox ID="txtFromDate" TabIndex="3" runat="server" Width="80px" CssClass="fiel_input"></asp:TextBox>&nbsp;
                                                            <asp:ImageButton ID="ImgBtnFrmDt" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.png">
                                                            </asp:ImageButton>
                                                            <cc1:MaskedEditValidator ID="MEVFromDate" runat="server" CssClass="field_error" TooltipMessage="Input a date in dd/mm/yyyy format"
                                                                InvalidValueMessage="Invalid Date" EmptyValueMessage="Date is required" ControlExtender="MEFromDate"
                                                                ControlToValidate="txtFromDate" Display="Dynamic" InvalidValueBlurredMessage="Invalid Date"
                                                                EmptyValueBlurredText="Date is required"></cc1:MaskedEditValidator>
                                                        </td>
                                                        <td style="width: 130px">
                                                            <asp:Label ID="Label5" runat="server" Text="To Invoice Date" CssClass="field_caption"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtToDate" TabIndex="4" runat="server" Width="80px" CssClass="fiel_input"></asp:TextBox>
                                                            &nbsp;
                                                            <asp:ImageButton ID="ImgBtnRevDate" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.png">
                                                            </asp:ImageButton>
                                                            <cc1:MaskedEditValidator ID="MEVToDate" runat="server" CssClass="field_error" TooltipMessage="Input a date in dd/mm/yyyy format"
                                                                InvalidValueMessage="Invalid Date" EmptyValueMessage="Date is required" ControlExtender="METoDate"
                                                                ControlToValidate="txtToDate" Display="Dynamic" InvalidValueBlurredMessage="Invalid Date"
                                                                EmptyValueBlurredText="Date is required"></cc1:MaskedEditValidator>
                                                        </td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4">
                                            <asp:Panel ID="pnlAdvSearch" runat="server" Visible="False">
                                                <table class="td_cell">
                                                    <tbody>
                                                        <tr>
                                                            <td style="width: 130px">
                                                                <asp:Label ID="Label4" runat="server" Text="Status" Width="120px" CssClass="field_caption"></asp:Label>
                                                            </td>
                                                            <td style="width: 240px">
                                                                <select style="width: 154px" id="ddlStatus" class="td_cell" tabindex="5" runat="server">
                                                                    <option value="[Select]" selected>[Select]</option>
                                                                    <option value="P">Posted</option>
                                                                    <option value="U">UnPosted</option>
                                                                    <option value="D">Deleted</option>
                                                                </select>
                                                            </td>
                                                            <td style="width: 134px">
                                                            </td>
                                                            <td style="width: 199px">
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 130px">
                                                                <asp:Label ID="Label6" runat="server" Text="Customer" Width="120px" CssClass="field_caption"></asp:Label>
                                                            </td>
                                                            <td style="width: 240px">
                                                                <select style="width: 200px" id="ddlCustomer" class="drpdown" tabindex="6" runat="server">
                                                                    <option selected></option>
                                                                </select>
                                                            </td>
                                                            <td style="width: 134px">
                                                                <asp:Label ID="Label9" runat="server" Text="Customer Ref" Width="120px" CssClass="field_caption"></asp:Label>
                                                            </td>
                                                            <td style="width: 199px">
                                                                <input id="txtCustRef" class="txtbox" tabindex="7" type="text" runat="server" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 130px">
                                                                <asp:Label ID="Label7" runat="server" Text="From Amount" Width="120px" CssClass="field_caption"></asp:Label>
                                                            </td>
                                                            <td style="width: 240px">
                                                                <input id="txtFromAmount" class="txtbox" tabindex="8" type="text" runat="server" />
                                                            </td>
                                                            <td style="width: 134px">
                                                                <asp:Label ID="Label10" runat="server" Text="To Amount" Width="120px" CssClass="field_caption"></asp:Label>
                                                            </td>
                                                            <td style="width: 199px">
                                                                <input id="txtToAmount" class="txtbox" tabindex="9" type="text" runat="server" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 130px">
                                                                <asp:Label ID="Label8" runat="server" Text="Order By" Width="120px" CssClass="field_caption"></asp:Label>
                                                            </td>
                                                            <td title=" " style="width: 240px">
                                                                <select style="width: 200px" id="ddlOrderBy" class="drpdown" tabindex="10" runat="server">
                                                                    <option value="0" selected>Invoice No Desc</option>
                                                                    <option value="1">Invoice No Asc</option>
                                                                    <option value="2">Customer Code</option>
                                                                    <option value="3">Customer Name</option>
                                                                    <option value="4">Invoice Date</option>
                                                                    <option value="5">File No</option>
                                                                </select>
                                                            </td>
                                                            <td style="width: 134px">
                                                            </td>
                                                            <td style="width: 199px">
                                                                <input id="txtconnection" runat="server" style="visibility: hidden; width: 12px;
                                                                    height: 9px" type="text" />
                                                            </td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                            <table cols="3">
                                <tbody>
                                    <tr>
                                        <td style="width: 131px">
                                            <span>&nbsp;<asp:Label ID="Label11" runat="server" Text="Report Type" Width="120px"
                                                CssClass="field_caption"></asp:Label></span>
                                        </td>
                                        <td style="width: 223px">
                                            <asp:DropDownList ID="ddlrpttype" TabIndex="11" runat="server" Width="207px" CssClass="drpdown">
                                                <asp:ListItem Value="0">Brief</asp:ListItem>
                                                <asp:ListItem Value="1">Detailed</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <span>
                                                <asp:Label ID="Label12" runat="server" Text="Applicable Only for the Report" ForeColor="Red"
                                                    Width="238px" CssClass="field_caption"></asp:Label></span>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                            <cc1:CalendarExtender ID="CEFromDate" runat="server" Format="dd/MM/yyyy" PopupButtonID="ImgBtnFrmDt"
                                TargetControlID="txtFromDate">
                            </cc1:CalendarExtender>
                            <cc1:MaskedEditExtender ID="MEFromDate" runat="server" TargetControlID="txtFromDate"
                                MaskType="Date" Mask="99/99/9999">
                            </cc1:MaskedEditExtender>
                            <cc1:CalendarExtender ID="CEToDate" runat="server" Format="dd/MM/yyyy" PopupButtonID="ImgBtnRevDate"
                                TargetControlID="txtToDate">
                            </cc1:CalendarExtender>
                            <cc1:MaskedEditExtender ID="METoDate" runat="server" TargetControlID="txtToDate"
                                MaskType="Date" Mask="99/99/9999">
                            </cc1:MaskedEditExtender>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Button ID="BtnExportToExcel" runat="server" CssClass="btn" TabIndex="17" Text="Export To Excel" /><input
                        id="txtSearchExport" runat="server" style="visibility: hidden; width: 8px" type="text" />
                    <asp:Button ID="BtnReports" runat="server" CssClass="btn" TabIndex="18" Text="Reports" />
                   <%-- Added BtnReports by Archana on 10/5/2015 for multiple windows when click on button reports--%>
                </td>                
            </tr>
            <tr>
                <td class="td_cell">
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                        <ContentTemplate>
                            <asp:GridView ID="gvResult" TabIndex="19" runat="server" Font-Size="10px" CssClass="grdstyle"
                                GridLines="Vertical" Font-Italic="False" BorderWidth="1px" BorderStyle="None"
                                AutoGenerateColumns="False" AllowPaging="True" CellPadding="3">
                                <Columns>
                                    <asp:TemplateField HeaderText="Select">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkSelect" runat="server" />
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Center" Width="60px" />
                                        <ItemStyle HorizontalAlign="Center" Width="60px" />
                                    </asp:TemplateField>
                                    <%-- Added chkSelect by Archana on 10/05/2015 for multiple windows when click on button reports--%>
                                    <asp:TemplateField HeaderText="Invoice No">
                                        <ItemTemplate>
                                            <asp:Label ID="lblInvoiceNo" runat="server" Text='<%# Bind("invoiceno") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="post_state" HeaderText="Status"></asp:BoundField>
                                    <asp:BoundField DataFormatString="{0:dd/MM/yyyy }" DataField="invoicedate" HeaderText="Invoice Date">
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderText="File Number ">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("requestid") %>'></asp:TextBox>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblRequestId" runat="server" Text='<%# Bind("requestid") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="agentcode" HeaderText="Customer Code"></asp:BoundField>
                                    <asp:BoundField DataField="agentname" HeaderText="Customer Name"></asp:BoundField>
                                    <asp:BoundField DataField="agentref" HeaderText="Customer Ref"></asp:BoundField>
                                    <asp:BoundField DataField="currcode" HeaderText="Currency"></asp:BoundField>
                                    <asp:BoundField DataField="salecurrency" HeaderText="Sale Amount"></asp:BoundField>
                                    <asp:BoundField DataField="salevalue" HeaderText="Amount"></asp:BoundField>
                                    <asp:BoundField DataFormatString="{0:dd/MM/yyyy}" DataField="adddate" HeaderText="Date Created">
                                    </asp:BoundField>
                                    <asp:BoundField DataField="adduser" HeaderText="User Created"></asp:BoundField>
                                    <asp:BoundField DataFormatString="{0:dd/MM/yyyy}" DataField="moddate" HeaderText="Date Modified">
                                    </asp:BoundField>
                                    <asp:BoundField DataField="moduser" HeaderText="User Modified"></asp:BoundField>
                                    <asp:ButtonField HeaderText="Action" Text="Edit" Visible="false" CommandName="RowEdit">
                                        <ItemStyle ForeColor="Blue"></ItemStyle>
                                    </asp:ButtonField>
                                    <asp:ButtonField HeaderText="Action" Text="View" CommandName="RowView">
                                        <ItemStyle ForeColor="Blue"></ItemStyle>
                                    </asp:ButtonField>
                                    <asp:ButtonField HeaderText="Action" Text="Print" CommandName="RowPrint">
                                        <ItemStyle ForeColor="Blue"></ItemStyle>
                                    </asp:ButtonField>
                                    <asp:ButtonField Text="Print" Visible="false">
                                        <ItemStyle ForeColor="Blue"></ItemStyle>
                                    </asp:ButtonField>
                                    <asp:ButtonField HeaderText="Action" Text="E1-mail" Visible="false" CommandName="RowEmail">
                                        <ItemStyle ForeColor="Blue"></ItemStyle>
                                    </asp:ButtonField>
                                    <asp:ButtonField HeaderText="Action" Text="PrintP/L" CommandName="RowPrintpl">
                                        <ItemStyle ForeColor="Blue"></ItemStyle>
                                    </asp:ButtonField>
                                    <asp:ButtonField Text="PrintP/L" Visible="false">
                                        <ItemStyle ForeColor="Blue"></ItemStyle>
                                    </asp:ButtonField>
                                    <asp:ButtonField HeaderText="Action" Text="Authorize Amend" Visible="false" CommandName="RowAuthorizeAmendpl">
                                        <ItemStyle ForeColor="Blue"></ItemStyle>
                                    </asp:ButtonField>
                                    <asp:ButtonField HeaderText="Action" Text="Authorize cancel" Visible="false" CommandName="RowAuthorizecancelpl">
                                        <ItemStyle ForeColor="Blue"></ItemStyle>
                                    </asp:ButtonField>
                                    <asp:ButtonField HeaderText="Action" Text="View Amend log" Visible="false" CommandName="RowviewAmendpl">
                                        <ItemStyle ForeColor="Blue"></ItemStyle>
                                    </asp:ButtonField>
                                    <asp:ButtonField HeaderText="Action" Text="Print Journal" CommandName="Rowprintjournal">
                                        <ItemStyle ForeColor="Blue"></ItemStyle>
                                    </asp:ButtonField>
                                </Columns>
                                <RowStyle CssClass="grdRowstyle" Wrap="False"></RowStyle>
                                <PagerStyle CssClass="grdpagerstyle" HorizontalAlign="Center"></PagerStyle>
                                <HeaderStyle CssClass="grdheader" Wrap="False"></HeaderStyle>
                                <AlternatingRowStyle CssClass="grdAternaterow"></AlternatingRowStyle>
                            </asp:GridView>
                            <asp:Label ID="lblMsg" runat="server" Font-Size="9pt" Font-Names="Verdana" Font-Bold="True"
                                Visible="False" CssClass="lblmsg">Records Not Found.</asp:Label>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td style="width: 100px">
                    <asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server">
                        <Services>
                            <asp:ServiceReference Path="~/clsServices.asmx"></asp:ServiceReference>
                        </Services>
                    </asp:ScriptManagerProxy>
                </td>
            </tr>
        </tbody>
    </table>
</asp:Content>
