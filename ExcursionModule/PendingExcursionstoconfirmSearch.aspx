<%@ Page Title="" Language="VB" MasterPageFile="~/ExcursionMaster.master" AutoEventWireup="false"
    CodeFile="PendingExcursionstoconfirmSearch.aspx.vb" Inherits="ExcursionModule_AssignDriversExcursionsSearch" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="EclipseWebSolutions.DatePicker" Namespace="EclipseWebSolutions.DatePicker"
    TagPrefix="ews" %>
<%@ OutputCache Location="none" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="Server">
    <script src="../Content/js/jquery-1.5.1.min.js" type="text/javascript"></script>
    <script src="../Content/js/JqueryUI.js" type="text/javascript"></script>
    <script src="../Content/js/AutoComplete.js" type="text/javascript"></script>
    <link type="text/css" href="../Content/css/JqueryUI.css" rel="Stylesheet" />
    <script type="text/javascript">
        var telephoneGrid;
        var hdnTelevalue = null;


        function hidediv() {
            var divtransfer = document.getElementById("<%=divassigntransfer.ClientID%>");
            divtransfer.style.display = "none";
            divtransfer.style.visibility = "hidden";
            return false;

        }

        function gettelehpone() {
            var drivercode = document.getElementById("<%=ddlDriverName.ClientID%>");
            var codeid = drivercode.options[drivercode.selectedIndex].value;


            ColServices.clsServices.GetDriverPhone(codeid, FillDriverTelValue, ErrorHandler, TimeOutHandler);
        }

        function FillDriverTelValue(result) {

            var telephone = document.getElementById("<%=txtTelephone.ClientID%>");
            document.getElementById("<%=hdndritelep.ClientID%>").value = result;
            telephone.value = result;

        }

        function gettelehponeforgrid(ddlDrivercode, txttelephone, hdnfield, hdntelephone) {

            var drivercode = document.getElementById(ddlDrivercode);

            telephoneGrid = document.getElementById(txttelephone);
            hdnTelevalue = document.getElementById(hdntelephone);

            document.getElementById(hdnfield).value = drivercode.value;


            ColServices.clsServices.GetDriverPhone(drivercode.value, FillDriverTelValuegrid, ErrorHandler, TimeOutHandler);
        }

        function FillDriverTelValuegrid(result) {


            telephoneGrid.value = result;
            hdnTelevalue.value = result;
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




    </script>
    <style type="text/css">
        .ModalPopupBG
        {
            background-color: gray;
            filter: alpha(opacity=50);
            opacity: 0.9;
        }
    </style>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table style="border-right: gray 2px solid; border-top: gray 2px solid; border-left: gray 2px solid;
                width: 100%; border-bottom: gray 2px solid" class="td_cell" align="left">
                <tr style="color: #000000">
                    <td style="text-align: center; width: 110px" class="td_cell">
                        <asp:Label ID="lblHeading" runat="server" Text="Prepaid Excursions to Confirm" CssClass="field_heading"
                            Width="100%"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <table style="width: 100%;">
                            <tr>
                                <td>
                                    <table style="width: 990px;">
                                        <tr>
                                            <td style="width: 100px" align="left" class="td_cell">
                                                From Tour Date
                                            </td>
                                            <td style="width: 100px" align="left" class="td_cell">
                                                <asp:TextBox ID="fromdate" runat="server" AutoPostBack="false" CssClass="fiel_input"
                                                    Width="60px"></asp:TextBox>
                                                <cc1:CalendarExtender ID="CEPFromDate" runat="server" Format="dd/MM/yyyy" PopupButtonID="ImgPBtnFrmDt"
                                                    TargetControlID="fromdate">
                                                </cc1:CalendarExtender>
                                                <cc1:MaskedEditExtender ID="MEPFromDate" runat="server" Mask="99/99/9999" MaskType="Date"
                                                    TargetControlID="fromdate">
                                                </cc1:MaskedEditExtender>
                                                <asp:ImageButton ID="ImgPBtnFrmDt" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.png"
                                                    Height="15px" TabIndex="1" /><cc1:MaskedEditValidator ID="MEVPFromDate" runat="server"
                                                        ControlExtender="MEPFromDate" ControlToValidate="fromdate" CssClass="field_error"
                                                        Display="Dynamic" EmptyValueBlurredText="Date is required" EmptyValueMessage="Date is required"
                                                        ErrorMessage="MEPFromDate" InvalidValueBlurredMessage="Invalid Date" InvalidValueMessage="Invalid Date"
                                                        TabIndex="-1" TooltipMessage="Date in dd/mm/yyyy format"> </cc1:MaskedEditValidator>
                                            </td>
                                            <td style="width: 100px" align="left" class="td_cell">
                                                To Tour Date
                                            </td>
                                            <td style="width: 100px" align="left" class="td_cell">
                                                <asp:TextBox ID="txtodate" runat="server" AutoPostBack="false" CssClass="fiel_input"
                                                    Width="60px" Height="15px" TabIndex="2"></asp:TextBox>
                                                <cc1:CalendarExtender ID="CEPToDate" runat="server" Format="dd/MM/yyyy" PopupButtonID="ImgPBtnToDt"
                                                    TargetControlID="txtodate">
                                                </cc1:CalendarExtender>
                                                <cc1:MaskedEditExtender ID="MEPToDate" runat="server" Mask="99/99/9999" MaskType="Date"
                                                    TargetControlID="txtodate">
                                                </cc1:MaskedEditExtender>
                                                &nbsp;<asp:ImageButton ID="ImgPBtnToDt" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.png"
                                                    Height="15px" TabIndex="3" />
                                                <cc1:MaskedEditValidator ID="MEVPToDate" runat="server" ControlExtender="MEPToDate"
                                                    ControlToValidate="txtodate" CssClass="field_error" Display="Dynamic" EmptyValueBlurredText="Date is required"
                                                    EmptyValueMessage="Date is required" ErrorMessage="MEPToDate" InvalidValueBlurredMessage="Invalid Date"
                                                    InvalidValueMessage="Invalid Date" TabIndex="-1" TooltipMessage="Date in dd/mm/yyyy format">
                                                </cc1:MaskedEditValidator>
                                            </td>
                                            <td style="width: 100px" align="left" class="td_cell">
                                                Excursion Type
                                            </td>
                                            <td style="width: 80px">
                                                <asp:DropDownList ID="ddlTransferType" runat="server" class="field_input" Style="width: 150px;"
                                                    TabIndex="4">
                                                </asp:DropDownList>
                                            </td>
                                            <td style="width: 22px">
                                                <asp:CheckBox ID="chkDate" runat="server" AutoPostBack="True" Width="20px" TabIndex="5" />
                                            </td>
                                            <td rowspan="2" style="width: 200px">
                                                <asp:TextBox ID="txtRemarks" runat="server" CssClass="field_input" Width="260px"
                                                    Height="50px" Font-Bold="True" Font-Size="Large" TextMode="MultiLine" TabIndex="6"
                                                    ReadOnly="True"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <%--    </table>

                     <table style="width:600px;border:2px solid blue">--%>
                                        <tr>
                                            <td style="width: 120px;" align="left" class="td_cell">
                                                From Request Date
                                            </td>
                                            <td style="width: 100px" align="left" class="td_cell">
                                                <asp:TextBox ID="txtTransFrmDate" runat="server" AutoPostBack="false" CssClass="fiel_input"
                                                    Width="60px" TabIndex="7"></asp:TextBox>
                                                <cc1:CalendarExtender ID="CEPTransFromDate" runat="server" Format="dd/MM/yyyy" PopupButtonID="ImgPBtnTransFrmDt"
                                                    TargetControlID="txtTransFrmDate">
                                                </cc1:CalendarExtender>
                                                <cc1:MaskedEditExtender ID="MEPTransFromDate" runat="server" Mask="99/99/9999" MaskType="Date"
                                                    TargetControlID="txtTransFrmDate">
                                                </cc1:MaskedEditExtender>
                                                &nbsp;<asp:ImageButton ID="ImgPBtnTransFrmDt" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.png"
                                                    Height="15px" TabIndex="8" />
                                                <cc1:MaskedEditValidator ID="MEVPTransDate" runat="server" ControlExtender="MEPTransFromDate"
                                                    ControlToValidate="txtTransFrmDate" CssClass="field_error" Display="Dynamic"
                                                    EmptyValueBlurredText="Date is required" EmptyValueMessage="Date is required"
                                                    ErrorMessage="MEPTransFromDate" InvalidValueBlurredMessage="Invalid Date" InvalidValueMessage="Invalid Date"
                                                    TabIndex="-1" TooltipMessage="Date in dd/mm/yyyy format">
                                                </cc1:MaskedEditValidator>
                                            </td>
                                            <td style="width: 110px;" align="left" class="td_cell">
                                                To Request Date
                                            </td>
                                            <td style="width: 100px" align="left" class="td_cell">
                                                <asp:TextBox ID="txtTransferTodate" runat="server" AutoPostBack="false" CssClass="fiel_input"
                                                    Width="60px" Height="15px" TabIndex="9"></asp:TextBox>
                                                <cc1:CalendarExtender ID="CEPTransToDate" runat="server" Format="dd/MM/yyyy" PopupButtonID="ImgPBtnTransToDt"
                                                    TargetControlID="txtTransferTodate">
                                                </cc1:CalendarExtender>
                                                <cc1:MaskedEditExtender ID="MEPTransToDate" runat="server" Mask="99/99/9999" MaskType="Date"
                                                    TargetControlID="txtTransferTodate">
                                                </cc1:MaskedEditExtender>
                                                &nbsp;<asp:ImageButton ID="ImgPBtnTransToDt" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.png"
                                                    Height="15px" TabIndex="10" />
                                                <cc1:MaskedEditValidator ID="MEVPTransToDate" runat="server" ControlExtender="MEPTransToDate"
                                                    ControlToValidate="txtTransferTodate" CssClass="field_error" Display="Dynamic"
                                                    EmptyValueBlurredText="Date is required" EmptyValueMessage="Date is required"
                                                    ErrorMessage="MEPTransToDate" InvalidValueBlurredMessage="Invalid Date" InvalidValueMessage="Invalid Date"
                                                    TabIndex="-1" TooltipMessage="Date in dd/mm/yyyy format">
                                                </cc1:MaskedEditValidator>
                                            </td>
                                            <td style="width: 22px">
                                                <asp:CheckBox ID="chkTransferDate" runat="server" AutoPostBack="True" Width="20px"
                                                    TabIndex="11" />
                                            </td>
                                        </tr>
                                    </table>
                                    <table style="width: 880px;">
                                        <tr>
                                            <td style="width: 50px" align="left" class="td_cell">
                                                Hotel
                                            </td>
                                            <td class="td_cell" style="width: 350px">
                                                <input type="text" class="field_input MyAutoCompleteHotelClass MyAutoCompleteHotelTypeClass"
                                                    id="txthotelName" runat="server" style="width: 245px" tabindex="12" />
                                                <select id="ddlHotelName" class="field_input MyDropDownListsuppValue" runat="server"
                                                    style="width: 250px" tabindex="13">
                                                </select>
                                            </td>
                                            <td style="width: 160px">
                                                Guest
                                            </td>
                                            <td style="width: 140px" class="td_cell">
                                                <asp:TextBox ID="txtGuestname" runat="server" CssClass="field_input" Width="200px"
                                                    Enabled="True" TabIndex="14"></asp:TextBox>
                                            </td>
                                            <td style="width: 180px">
                                                Excursion Id
                                            </td>
                                            <td style="width: 140px" class="td_cell">
                                                <asp:TextBox ID="txtExcursionId" runat="server" CssClass="field_input" Width="200px"
                                                    Enabled="True" TabIndex="15"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 50px" align="left" class="td_cell">
                                                Client
                                            </td>
                                            <td class="td_cell" style="width: 350px">
                                                <input type="text" class="field_input MyAutoCompleteClass" id="txtagent" runat="server"
                                                    style="width: 245px" tabindex="16" />
                                                <select id="ddlAgent" class="field_input  MyDropDownListCustValue" runat="server"
                                                    style="width: 250px" tabindex="17">
                                                </select>
                                            </td>
                                            <td style="width: 140px">
                                                Ticket No
                                            </td>
                                            <td style="width: 140px" class="td_cell">
                                                <asp:TextBox ID="txtTicketNo" runat="server" CssClass="field_input" Width="200px"
                                                    Enabled="True" TabIndex="18"></asp:TextBox>
                                            </td>
                                            <td style="width: 180px" colspan="2">
                                                <input type="checkbox" id="chkTourGuideReq" runat="server" />
                                                Tour Guide Required
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <asp:Button ID="btnResult" runat="server" CssClass="btn" Font-Bold="False" Text="Fill Results"
                                                    Width="100px" />
                                                <asp:Button ID="btnReport" runat="server" CssClass="btn" Font-Bold="False" Text="Report"
                                                    Width="100px" />
                                            </td>                                            
                                        </tr>
                                    </table>
                                    <table style="width: 900px;">
                                        <tr>
                                            <td style="width: 900px;">
                                                <%--<asp:Panel ID="pngv" runat="server"  Width="900px" Height="300px"  scrollbars="Auto">--%>
                                                <asp:GridView ID="gv_SearchResult" Width="985px" TabIndex="9" runat="server" Font-Size="10px"
                                                    CssClass="grdstyle" GridLines="Vertical" CellPadding="3" BorderWidth="1px" BorderStyle="None"
                                                    AutoGenerateColumns="False" AllowPaging="True">
                                                    <FooterStyle CssClass="grdfooter"></FooterStyle>
                                                    <Columns>
                                                        <asp:TemplateField Visible="false" HeaderText="RequestLineNo">
                                                            <EditItemTemplate>
                                                                <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("rlineno") %>' ReadOnly="true"></asp:TextBox></EditItemTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblCode" runat="server" Text='<%# Bind("rlineno") %>' ReadOnly="true"></asp:Label></ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="excid" HeaderText="Excursion Id" ReadOnly="True"></asp:BoundField>
                                                        <asp:BoundField HtmlEncode="False" DataField="HotelName" HeaderText="HotelName" ReadOnly="true">
                                                            <HeaderStyle Width="15%" Wrap="true" />
                                                            <ItemStyle Width="15%" Wrap="true" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="Telephone" HeaderText="Telephone" ReadOnly="True"></asp:BoundField>
                                                        <asp:BoundField DataField="guestname" HeaderText="Guest Name" ReadOnly="True"></asp:BoundField>
                                                        <asp:BoundField HtmlEncode="False" DataField="othtypname" HeaderText="Excursion"
                                                            ReadOnly="true">
                                                            <HeaderStyle Width="20%" Wrap="true" />
                                                            <ItemStyle Width="20%" Wrap="true" />
                                                        </asp:BoundField>
                                                        <asp:BoundField HtmlEncode="False" DataFormatString="{00:dd/MM/yyyy}" DataField="tourdate"
                                                            HeaderText="Tour Date" ReadOnly="true">
                                                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Top"></ItemStyle>
                                                            <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="roomno" HeaderText="Room No" ReadOnly="True"></asp:BoundField>
                                                        <asp:BoundField DataField="adults" HeaderText="Adult" ReadOnly="True"></asp:BoundField>
                                                        <asp:BoundField DataField="child" HeaderText="Child" ReadOnly="True"></asp:BoundField>
                                                        <asp:BoundField DataField="status" HeaderText="Status" ReadOnly="True"></asp:BoundField>
                                                        <asp:TemplateField Visible="false"></asp:TemplateField>
                                                        <asp:TemplateField Visible="false" HeaderText="HotelCode">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblhotel" runat="server" Text='<%# Bind("Hotel") %>' ReadOnly="true"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Confirm">
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="lnkConfirm" runat="server" CommandName="Confirm" CommandArgument='<%# Ctype(Container,GridViewRow).RowIndex %>'>Confirm</asp:LinkButton>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Cancel">
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="lnlCancel" runat="server" CommandName="CancelRow" CommandArgument='<%# Ctype(Container,GridViewRow).RowIndex %>'>Cancel</asp:LinkButton>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField Visible="false" HeaderText="Driver Code">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblsdrivercode" runat="server" Text='<%# Bind("drivercode") %>' ReadOnly="true"></asp:Label>
                                                                <br />
                                                                <asp:Label ID="lblstatus" runat="server" Text='<%# Bind("status") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <RowStyle CssClass="grdRowstyle"></RowStyle>
                                                    <SelectedRowStyle CssClass="grdselectrowstyle"></SelectedRowStyle>
                                                    <PagerStyle CssClass="grdpagerstyle" HorizontalAlign="Center"></PagerStyle>
                                                    <HeaderStyle CssClass="grdheader" ForeColor="White"></HeaderStyle>
                                                    <AlternatingRowStyle CssClass="grdAternaterow"></AlternatingRowStyle>
                                                </asp:GridView>
                                                <asp:Label ID="lblMsg" runat="server" Text="Records not found. Please redefine search criteria"
                                                    Font-Size="8pt" Font-Names="Verdana" Font-Bold="True" Width="357px" Visible="False"
                                                    CssClass="lblmsg"></asp:Label>
                                                <%-- </asp:Panel>--%>
                                            </td>
                                        </tr>
                                    </table>
                                    <div id="divassigntransfer" runat="server" style="border: 3px solid #04a205; background-color: #FFFFFF;
                                        width: 900px">
                                        <table style="width: 900px;">
                                            <tr>
                                                <td style="width: 110px" class="td_cell">
                                                    Excursion Id
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtTransferIDvalue" runat="server" ReadOnly="true" CssClass="fiel_input"
                                                        Width="100px"></asp:TextBox>
                                                </td>
                                                <td style="width: 130px" class="td_cell">
                                                    Excursion Type
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtTransferTypevalue" runat="server" ReadOnly="true" CssClass="fiel_input"
                                                        Width="150px"></asp:TextBox>
                                                </td>
                                                <td style="width: 70px" class="td_cell">
                                                    Agent Name
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtagentName" runat="server" ReadOnly="true" CssClass="fiel_input"
                                                        Width="150px"></asp:TextBox>
                                                </td>
                                                <td style="width: 80px" class="td_cell">
                                                    Ticket No
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtclientRefvalue" runat="server" ReadOnly="true" CssClass="fiel_input"
                                                        Width="100px"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 95px" class="td_cell">
                                                    Tour Date
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtTransferDatevalue" runat="server" ReadOnly="true" CssClass="fiel_input"
                                                        Width="100px"></asp:TextBox>
                                                </td>
                                                <td id="Td1" style="width: 70px" runat="server" class="td_cell">
                                                    Hotel Name
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtHotelNameValue" runat="server" ReadOnly="true" CssClass="fiel_input"
                                                        Width="150px" TextMode="MultiLine"></asp:TextBox>
                                                </td>
                                                <td id="Td6" runat="server" class="td_cell" style="width: 120px">
                                                    Guest Name
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtGuestNameValue" runat="server" CssClass="fiel_input" ReadOnly="true"
                                                        Width="150px"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 75px">
                                                    Driver Name
                                                </td>
                                                <td>
                                                    <select id="ddlDriverName" runat="server" class="field_input" onchange="gettelehpone();"
                                                        style="width: 100px;" tabindex="19">
                                                    </select>
                                                </td>
                                                <td style="width: 80px">
                                                    Driver Tel No
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtTelephone" runat="server" CssClass="fiel_input" Width="100px"
                                                        ReadOnly="true" TabIndex="20"></asp:TextBox>
                                                </td>
                                            </tr>
                                        </table>
                                        <table style="width: 900px;">
                                            <tr>
                                                <td class="td_cell" style="width: 80px">
                                                    <asp:Panel ID="PanelRoomType" runat="server" GroupingText="Additional Drivers" Width="300px">
                                                        <table>
                                                            <tr>
                                                                <td style="width: 100px">
                                                                    <asp:GridView ID="Gv_DriverName" runat="server" AutoGenerateColumns="False" Width="300px">
                                                                        <Columns>
                                                                            <asp:TemplateField HeaderText="Driver Code" Visible="False">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblDriverCode" runat="server" Text='<%# Bind("drivercode") %>'></asp:Label>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField>
                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                                <ItemTemplate>
                                                                                    &nbsp;<asp:CheckBox ID="ChkSelect" runat="server" />
                                                                                </ItemTemplate>
                                                                                <FooterTemplate>
                                                                                    <input id="ChkInactiveold" runat="server" type="checkbox" />
                                                                                </FooterTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Driver Name">
                                                                                <ItemTemplate>
                                                                                    <select id="ddldrivernamevalue" runat="server" class="field_input" style="width: 115px">
                                                                                        <option selected=""></option>
                                                                                    </select>
                                                                                </ItemTemplate>
                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Tele Phone">
                                                                                <ItemTemplate>
                                                                                    <asp:TextBox ID="txttelephonevalue" runat="server" CssClass="field_input" ReadOnly="true"
                                                                                        Width="80px"></asp:TextBox>
                                                                                    <asp:HiddenField ID="hdndrivercode" runat="server" />
                                                                                    <asp:HiddenField ID="hdntelephone" runat="server" />
                                                                                </ItemTemplate>
                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                            </asp:TemplateField>
                                                                        </Columns>
                                                                    </asp:GridView>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:Button ID="btnAddRow" runat="server" CssClass="field_button" Text="Add Row" />
                                                                    &nbsp;
                                                                    <asp:Button ID="btnDeleteRow" runat="server" CssClass="field_button" Text="Delete Row" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </asp:Panel>
                                                </td>
                                            </tr>
                                        </table>
                                        <table>
                                            <tr>
                                                <td style="width: 80px">
                                                    <asp:Button ID="btnSave" runat="server" CssClass="btn" Text="Save" />
                                                </td>
                                                <td style="width: 80px">
                                                    <asp:Button ID="btnCancel" runat="server" CssClass="btn" Text="Cancel" />
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Button ID="btnAddNew" runat="server" CssClass="btn" Width="110px" Text="Add New" /></button>&nbsp;
                        <asp:Button ID="btnPrint" runat="server" CssClass="btn" Width="110px" Text="Print" /></button>&nbsp;
                        <asp:Button ID="btnExportToExcel" runat="server" CssClass="btn" Width="110px" Text="Export to Excel" /></button>&nbsp;
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:HiddenField ID="hdnvlineno" runat="server"></asp:HiddenField>
                        <asp:HiddenField ID="hdntransaction" runat="server"></asp:HiddenField>
                        <asp:HiddenField ID="hdndritelep" runat="server" />
                    </td>
                </tr>
            </table>
            <cc1:ModalPopupExtender ID="ModalPopupDays" runat="server" BehaviorID="ModalPopupDays"
                CancelControlID="btnCancel" OkControlID="btnOkay" TargetControlID="btnInvisibleGuest"
                PopupControlID="divassigntransfer" PopupDragHandleControlID="PopupHeader" Drag="true"
                BackgroundCssClass="ModalPopupBG">
            </cc1:ModalPopupExtender>
            <input id="btnInvisibleGuest" runat="server" type="button" value="Cancel" style="visibility: hidden" />
            <input id="btnOkay" type="button" value="OK" style="visibility: hidden" />
            <input id="btnCancel1" type="button" value="Cancel" style="visibility: hidden" />
            <asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server">
                <Services>
                    <asp:ServiceReference Path="~/clsServices.asmx" />
                </Services>
            </asp:ScriptManagerProxy>
        </ContentTemplate>
    </asp:UpdatePanel>
    <script type="text/javascript">
        $(document).ready(function () {
            var prm = Sys.WebForms.PageRequestManager.getInstance();
            prm.add_initializeRequest(InitializeRequest);
            prm.add_endRequest(EndRequest);

            // Place here the first init of the autocomplete
            MyAutoCustomer();
            MyAutohotel_rptFillArray();


        });

        function InitializeRequest(sender, args) {

        }

        function EndRequest(sender, args) {
            // after update occur on UpdatePanel re-init the Autocomplete
            MyAutoCustomer();
            MyAutohotel_rptFillArray();

        }
    </script>
</asp:Content>
