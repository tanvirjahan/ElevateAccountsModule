<%@ Page Title="" Language="VB" MasterPageFile="~/ExcursionMaster.master" AutoEventWireup="false"
    CodeFile="DeskExcursionRequestSearch.aspx.vb" Inherits="ExcursionModule_DeskExcursionRequestSearch" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ OutputCache Location="none" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="Server">
    <script language="JavaScript" type="text/javascript">
        window.history.forward(1);  
    </script>
    <script src="../Content/js/jquery-1.5.1.min.js" type="text/javascript"></script>
    <script src="../Content/js/JqueryUI.js" type="text/javascript"></script>
    <script src="../Content/js/AutoComplete.js" type="text/javascript"></script>
    <link type="text/css" href="../Content/css/JqueryUI.css" rel="Stylesheet" />
    <script language="javascript" type="text/javascript">


        function MyAutoCustomer() {
            var type = jQuery(".MyAutoCompleteTypeClass").val();
            jQuery.ajax({
                url: "../ClsServices.asmx/CustomerAutoCompleteExcursionRequest",
                data: "{ para1:'" + type + "',para2:''}",
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                dataFilter: function (data) { return data; },
                success: function (data) {

                    if (data.d.length > 0) {
                        var result = data.d;
                        // alert(result.length);

                        if (result.length == 0)
                            return;

                        jQuery(".MyAutoCompleteClass").autocomplete({
                            source: jQuery.map(result, function (item) {
                                return {
                                    value: item.Name,
                                    text: item.Id

                                }

                            }),
                            minLength: 1,
                            select: function (event, ui) {

                                jQuery(".MyDropDownListCustValue").val(ui.item.text);
                                jQuery(".MyDropDownListCustValue").change();

                            }


                        });

                    }
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert(errorThrown);
                }
            });
        }


        function chkTextLock(e) {
            return false;
        }

        function checkNumber(e) {

            if ((event.keyCode < 47 || event.keyCode > 57)) {
                return false;
            }

        }
        function checkCharacter(e) {
            if (event.keyCode == 32 || event.keyCode == 46)
                return;
            if ((event.keyCode < 47 || event.keyCode > 57) && (event.keyCode < 65 || event.keyCode > 91) && (event.keyCode < 96 || event.keyCode > 122)) {
                return false;
            }

        }
        function confirmInvoice(url) {

            var x = confirm('This Booking Already Invoiced Do You want to Edit?');

            if (x) {

                window.open(url);

            }

            return x;

        }

        function CallWebMethod(methodType) {
            switch (methodType) {



                case "ExTypeCode":
                    var select = document.getElementById("<%=ddlExTypeCode.ClientID%>");
                    var selectname = document.getElementById("<%=ddlExTypeName.ClientID%>");
                    selectname.value = select.options[select.selectedIndex].text;
                    break;
                case "ExTypeName":
                    var select = document.getElementById("<%=ddlExTypeName.ClientID%>");
                    var selectname = document.getElementById("<%=ddlExTypeCode.ClientID%>");
                    selectname.value = select.options[select.selectedIndex].text;
                    break;

            }
        }
    </script>
    <table style="border: 2px solid gray; width: 100%">
        <tr>
            <td align="center" class="field_heading">
                <asp:Label ID="Label1" runat="server" CssClass="field_heading" Height="18px" Text="Desk Excursion Request"
                    Width="100%"></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="center" class="td_cell" style="color: blue">
                Type few characters of code or name and click search
            </td>
        </tr>
        <tr>
            <td colspan="4">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <table class="td_cell">
                            <tbody>
                                <tr>
                                    <td align="center" colspan="4">
                                        <asp:RadioButton ID="rbtnsearch" runat="server" Text="Search" ForeColor="Black" CssClass="td_cell"
                                            OnCheckedChanged="rbtnsearch_CheckedChanged" GroupName="GrSearch" Checked="True"
                                            AutoPostBack="True"></asp:RadioButton>
                                        <asp:RadioButton ID="rbtnadsearch" runat="server" Text="Advance Search" ForeColor="Black"
                                            CssClass="td_cell" OnCheckedChanged="rbtnadsearch_CheckedChanged" GroupName="GrSearch"
                                            AutoPostBack="True" Visible="false"></asp:RadioButton>&nbsp;
                                           <%-- Changed by Archana on 07/03/2015--Kept Advance Search as hidden as assigned by Shahul sir--%>
                                        <asp:Button ID="btnSearch" OnClick="btnSearch_Click" runat="server" Text="Search"
                                            Font-Bold="False" CssClass="search_button"></asp:Button>&nbsp;
                                        <asp:Button ID="btnClear" OnClick="btnClear_Click" runat="server" Text="Clear" Font-Bold="False"
                                            CssClass="search_button"></asp:Button>&nbsp;
                                        <asp:Button ID="cmdhelp" OnClick="cmdhelp_Click" runat="server" Text="Help" Font-Bold="False"
                                            CssClass="search_button"></asp:Button>&nbsp;
                                        <asp:Button ID="btnAddNew" OnClick="btnAddNew_Click" runat="server" Text="Add New"
                                            Font-Bold="True" CssClass="field_button"></asp:Button>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 130px" class="td_cell">
                                        Excursion ID
                                    </td>
                                    <td style="width: 250px">
                                        <asp:TextBox ID="txtExcursionID" Width="220px" CssClass="field_input" runat="server"
                                            TabIndex="1" />
                                    </td>
                                    <td>
                                        Ticket No
                                    </td>
                                    <td style="width: 300px">
                                        <asp:TextBox ID="txtTicketNo" Width="140px" CssClass="field_input" runat="server"
                                            TabIndex="2" />
                                        <asp:DropDownList ID="ddlOrderBy" runat="server" CssClass="field_input" AutoPostBack="True">
                                            <asp:ListItem Value="0">Excursion ID Desc</asp:ListItem>
                                            <asp:ListItem Value="1">Excursion ID Asc</asp:ListItem>
                                            <asp:ListItem Value="2">Excursion Type</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4">
                                        <asp:Panel ID="Panel1" runat="server" Height="150px" Width="850px">
                                            <table>
                                                <tbody>
                                                    <tr>
                                                        <td style="width: 130px" class="td_cell">
                                                            Service Code
                                                        </td>
                                                        <td>
                                                            <select style="width: 220px" id="ddlExTypeCode" class="field_input" onchange="CallWebMethod('ExTypeCode');"
                                                                runat="server" tabindex="3">
                                                                <option selected="selected"></option>
                                                            </select>
                                                        </td>
                                                        <td class="td_cell" style="width: 120px">
                                                            Service Name
                                                        </td>
                                                        <td>
                                                            <select style="width: 220px" id="ddlExTypeName" class="field_input" onchange="CallWebMethod('ExTypeName');"
                                                                runat="server" tabindex="4">
                                                                <option selected="selected"></option>
                                                            </select>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 125px" class="td_cell">
                                                            From Request Date
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtFromDate" runat="server" CssClass="txtbox" TabIndex="5" ValidationGroup="MKE"
                                                                Width="80px" />
                                                            <asp:ImageButton ID="ImgBtnFromDate" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.png" />
                                                            <cc1:MaskedEditValidator ID="MEVFromDate" runat="server" ControlExtender="MEEFromDate"
                                                                ControlToValidate="txtFromDate" CssClass="field_error" Display="Dynamic" EmptyValueBlurredText="*"
                                                                EmptyValueMessage="Date is required" ErrorMessage="" InvalidValueBlurredMessage="Invalid Date"
                                                                InvalidValueMessage="Invalid Date" TooltipMessage="Input a date in dd/mm/yyyy format"
                                                                ValidationGroup="MKE" Width="23px">
                                                            </cc1:MaskedEditValidator>
                                                            <cc1:MaskedEditExtender ID="MEEFromDate" TargetControlID="txtFromDate" runat="server"
                                                                AcceptNegative="Left" DisplayMoney="Left" ErrorTooltipEnabled="True" Mask="99/99/9999"
                                                                MaskType="Date" MessageValidatorTip="true">
                                                            </cc1:MaskedEditExtender>
                                                            <cc1:CalendarExtender ID="CLFromDate" TargetControlID="txtFromDate" PopupButtonID="ImgBtnFromDate"
                                                                runat="server" Format="dd/MM/yyyy" />
                                                        </td>
                                                        <td class="td_cell">
                                                            To Request Date
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtTodate" runat="server" CssClass="txtbox" TabIndex="6" ValidationGroup="MKE"
                                                                Width="80px" />
                                                            <asp:ImageButton ID="ImgBtnToDate" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.png" />
                                                            <cc1:MaskedEditValidator ID="MEVToDate" ControlExtender="MEEToDate" ControlToValidate="txtTodate"
                                                                runat="server" CssClass="field_error" Display="Dynamic" EmptyValueBlurredText="*"
                                                                EmptyValueMessage="Date is required" ErrorMessage="MskVFromDate" InvalidValueBlurredMessage="Invalid Date"
                                                                InvalidValueMessage="Invalid Date" TooltipMessage="Input a date in dd/mm/yyyy format"
                                                                ValidationGroup="MKE" Width="23px">
                                                            </cc1:MaskedEditValidator>
                                                            <cc1:MaskedEditExtender ID="MEEToDate" TargetControlID="txtTodate" runat="server"
                                                                AcceptNegative="Left" MaskType="Date" DisplayMoney="Left" ErrorTooltipEnabled="True"
                                                                Mask="99/99/9999" MessageValidatorTip="true">
                                                            </cc1:MaskedEditExtender>
                                                            <cc1:CalendarExtender ID="CLToDate" TargetControlID="txtTodate" PopupButtonID="ImgBtnToDate"
                                                                runat="server" Format="dd/MM/yyyy" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 107px" class="td_cell">
                                                            From Tour Date
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtFromTourDate" runat="server" CssClass="txtbox" TabIndex="7" ValidationGroup="MKE"
                                                                Width="80px" />
                                                            <asp:ImageButton ID="ImgBtnFromTourDate" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.png" />
                                                            <cc1:MaskedEditValidator ID="MEVFromTourDate" runat="server" ControlExtender="MEEFromTourDate"
                                                                ControlToValidate="txtFromTourDate" CssClass="field_error" Display="Dynamic"
                                                                EmptyValueBlurredText="*" EmptyValueMessage="Date is required" ErrorMessage=""
                                                                InvalidValueBlurredMessage="Invalid Date" InvalidValueMessage="Invalid Date"
                                                                TooltipMessage="Input a date in dd/mm/yyyy format" ValidationGroup="MKE" Width="23px">
                                                            </cc1:MaskedEditValidator>
                                                            <cc1:MaskedEditExtender ID="MEEFromTourDate" TargetControlID="txtFromTourDate" runat="server"
                                                                AcceptNegative="Left" DisplayMoney="Left" ErrorTooltipEnabled="True" Mask="99/99/9999"
                                                                MaskType="Date" MessageValidatorTip="true">
                                                            </cc1:MaskedEditExtender>
                                                            <cc1:CalendarExtender ID="CLFromTourDate" TargetControlID="txtFromTourDate" PopupButtonID="ImgBtnFromTourDate"
                                                                runat="server" Format="dd/MM/yyyy" />
                                                        </td>
                                                        <td class="td_cell">
                                                            To Tour Date
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtToTourdate" runat="server" CssClass="txtbox" TabIndex="8" ValidationGroup="MKE"
                                                                Width="80px" />
                                                            <asp:ImageButton ID="ImgBtnToTourDate" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.png" />
                                                            <cc1:MaskedEditValidator ID="MEVToTourDate" ControlExtender="MEEToTourDate" ControlToValidate="txtToTourdate"
                                                                runat="server" CssClass="field_error" Display="Dynamic" EmptyValueBlurredText="*"
                                                                EmptyValueMessage="Date is required" ErrorMessage="MskVToTourDate" InvalidValueBlurredMessage="Invalid Date"
                                                                InvalidValueMessage="Invalid Date" TooltipMessage="Input a date in dd/mm/yyyy format"
                                                                ValidationGroup="MKE" Width="23px">
                                                            </cc1:MaskedEditValidator>
                                                            <cc1:MaskedEditExtender ID="MEEToTourDate" TargetControlID="txtToTourdate" runat="server"
                                                                AcceptNegative="Left" MaskType="Date" DisplayMoney="Left" ErrorTooltipEnabled="True"
                                                                Mask="99/99/9999" MessageValidatorTip="true">
                                                            </cc1:MaskedEditExtender>
                                                            <cc1:CalendarExtender ID="CLToTourDate" TargetControlID="txtToTourdate" PopupButtonID="ImgBtnToTourDate"
                                                                runat="server" Format="dd/MM/yyyy" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 107px" class="td_cell">
                                                            Guest Name
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtGuestName" CssClass="field_input" Width="220px" runat="server"
                                                                TabIndex="9"></asp:TextBox>
                                                        </td>
                                                        <td class="td_cell">
                                                            Prepaid Id
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtPrepaidID" CssClass="field_input" Width="220px" runat="server"
                                                                TabIndex="10"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 107px" class="td_cell">
                                                            Client
                                                        </td>
                                                        <td>
                                                            <input type="text" name="accSearch" class="field_input MyAutoCompleteClass" style="width: 220px"
                                                                id="accSearch" runat="server" tabindex="11" />
                                                            <select style="width: 220px" id="ddlCustomer" class="field_input MyDropDownListCustValue"
                                                                runat="server">
                                                                <option selected></option>
                                                            </select>
                                                        </td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                        </asp:Panel>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Button ID="btnExportToExcel" runat="server" CssClass="field_button" Text="Export To Excel" />
            </td>
        </tr>
        <tr>
            <td class=" ">
                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                    <ContentTemplate>
                        <asp:GridView ID="gv_SearchResult" TabIndex="10" runat="server" Font-Size="10px"
                            BackColor="White" Width="100%" CssClass="td_cell" GridLines="Vertical" CellPadding="3"
                            BorderWidth="1px" BorderStyle="None" BorderColor="#999999" AutoGenerateColumns="False"
                            AllowSorting="False" AllowPaging="True">
                            <Columns>
                                <asp:TemplateField Visible="False" HeaderText="ID">
                                    <ItemTemplate>
                                        <asp:Label ID="lblExcursionId" runat="server" Text='<%# Bind("excid") %>'></asp:Label>
                                        <asp:Label ID="lblRlineNo" runat="server" Text='<%# Bind("rlineno") %>'></asp:Label>
                                        <asp:Label ID="lblTicketNo" runat="server" Text='<%# Bind("ticketno") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle VerticalAlign="Top" />
                                </asp:TemplateField>
                                <asp:BoundField DataField="excid" SortExpression="excid" HeaderText="Excursion ID">
                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Top"></ItemStyle>
                                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                </asp:BoundField>
                                <asp:BoundField HtmlEncode="False" DataField="RequestDate" HeaderText="Request Date">
                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Top"></ItemStyle>
                                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                </asp:BoundField>
                                <asp:BoundField HtmlEncode="False" DataField="TourDate" HeaderText="Tour Date">
                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Top"></ItemStyle>
                                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="agentname" SortExpression="agentname" HeaderText="Client Name">
                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Top"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="ServiceName" SortExpression="ServiceName" HeaderText="Service Name">
                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Top"></ItemStyle>
                                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="partyname" SortExpression="partyname" HeaderText="Hotel Name">
                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Top"></ItemStyle>
                                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="guestname" SortExpression="guestname" HeaderText="Guest Name">
                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Top"></ItemStyle>
                                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="roomno" SortExpression="roomno" HeaderText="Room No">
                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Top"></ItemStyle>
                                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="adults" SortExpression="adults" HeaderText="Adults">
                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Top"></ItemStyle>
                                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="child" SortExpression="child" HeaderText="Child">
                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Top"></ItemStyle>
                                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="market" SortExpression="market" HeaderText="Market">
                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Top"></ItemStyle>
                                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="ticketno" SortExpression="ticketno" HeaderText="Ticket No">
                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Top"></ItemStyle>
                                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="PaymentMode" SortExpression="PaymentMode" HeaderText="Payment Mode">
                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Top"></ItemStyle>
                                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="prepaidid" SortExpression="prepaidid" HeaderText="Prepaid ID">
                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Top"></ItemStyle>
                                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="status" SortExpression="status" HeaderText="Status">
                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Top"></ItemStyle>
                                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                </asp:BoundField>
                                <asp:BoundField HtmlEncode="False" DataFormatString="{00:dd/MM/yyyy hh:mm:ss tt}"
                                    DataField="adddate" SortExpression="adddate" HeaderText="Date Created">
                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Top"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="adduser" SortExpression="adduser" HeaderText="User Created">
                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Top"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField HtmlEncode="False" DataFormatString="{00:dd/MM/yyyy hh:mm:ss tt}"
                                    DataField="moddate" SortExpression="moddate" HeaderText="Date Modified">
                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Top"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="moduser" SortExpression="moduser" HeaderText="User Modified">
                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Top"></ItemStyle>
                                </asp:BoundField>
                                <asp:ButtonField HeaderText="Action" Text="Edit" CommandName="EditRow">
                                    <ItemStyle ForeColor="Blue" HorizontalAlign="Center" VerticalAlign="Top"></ItemStyle>
                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                </asp:ButtonField>
                                <asp:ButtonField HeaderText="Action" Text="View" CommandName="ViewRow">
                                    <ItemStyle ForeColor="Blue" HorizontalAlign="Center" VerticalAlign="Top"></ItemStyle>
                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                </asp:ButtonField>
                                <asp:ButtonField HeaderText="Action" Text="Print" CommandName="PrintRow">
                                    <ItemStyle ForeColor="Blue" HorizontalAlign="Center" VerticalAlign="Top"></ItemStyle>
                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                </asp:ButtonField>
                            </Columns>
                            <RowStyle CssClass="grdRowstyle" ForeColor="Black"></RowStyle>
                            <SelectedRowStyle BackColor="#008A8C" ForeColor="White" Font-Bold="True"></SelectedRowStyle>
                            <PagerStyle CssClass="grdpagerstyle" ForeColor="Black" HorizontalAlign="Center">
                            </PagerStyle>
                            <HeaderStyle CssClass="grdheader" ForeColor="White" Font-Bold="True"></HeaderStyle>
                            <AlternatingRowStyle CssClass="grdAternaterow" Font-Size="10px"></AlternatingRowStyle>
                            <FooterStyle CssClass="grdfooter" ForeColor="Black"></FooterStyle>
                        </asp:GridView>
                        <asp:Label ID="lblMsg" runat="server" Text="Records not found. Please redefine search criteria"
                            Font-Size="8pt" Font-Names="Verdana" Font-Bold="True" Width="357px" Visible="False"
                            CssClass="lblmsg"></asp:Label>
                        <div id="divExcursion" runat="server" style="border: 1px solid #04a205; left: 392px;
                            background-color: #FFFFFF; width: 220px; display: none">
                            <table style="border-right: gray 2px solid; border-top: gray 2px solid; border-left: gray 2px solid;
                                width: 200px; border-bottom: gray 2px solid"> 
                                <%--Changed width size in divExcursion by Archana on 19/03/2015 to show pop up big--%>
                                <tr>
                                    <td class="field_heading" align="center" colspan="1">
                                        <asp:Label ID="Label2" runat="server" Text="Print Excursion Vouchers" ForeColor="White"
                                            Width="210px" CssClass="field_heading"></asp:Label>
                                             <%--Changed width size by Archana on 19/03/2015 to show pop up big--%>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_cell">
                                        <asp:RadioButton ID="rbNormalRequest" Style="display: none" GroupName="Excursion"
                                            runat="server" Text="Normal Request" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_cell">
                                        <asp:RadioButton ID="rbBurjAlArabRequest" Style="display: none" GroupName="Excursion"
                                            runat="server" Text="Burj Al Arab Request" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_cell">
                                        <asp:RadioButton ID="rbDowCruiseRequest" Style="display: none" GroupName="Excursion"
                                            runat="server" Text="Dhow Cruise Request" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_cell">
                                        <asp:RadioButton ID="rbRentCarRequest" Style="display: none" GroupName="Excursion"
                                            runat="server" Text="Rent a Car Request" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_cell">
                                        <asp:RadioButton ID="radexcdet" GroupName="Excursion" runat="server" Text="Excursions Detail"  Visible="false"/>
                                    </td>
                                </tr>
                                <%--Changed by Archana on 07/03/2015--Kept Excursions Detail as hidden as assigned by Shahul sir--%>
                                <tr>
                                    <td class="td_cell">
                                        <asp:RadioButton ID="rbTicket" GroupName="Excursion" runat="server" Text="Ticket" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_cell">
                                        <asp:RadioButton ID="rbInvoice" GroupName="Excursion" runat="server" Text="Invoice" Visible="false" />
                                    </td>
                                </tr>
                                 <%--Changed by Archana on 07/03/2015--Kept Invoice as hidden as assigned by Shahul sir--%>
                                <tr>
                                    <td class="td_cell">
                                        <asp:RadioButton ID="rbproforma" GroupName="Excursion" runat="server" Text="Proforma Invoice" />
                                    </td>
                                </tr>

                                <tr>
                                    <td class="td_cell">
                                        <asp:RadioButton ID="rbemail" GroupName="Excursion" runat="server" Text="Email"  Visible="false"/>
                                    </td>
                                </tr>
                                <%--Added email option by Archana on 07/03/2015--%>

                                <tr>
                                    <td class="td_cell">
                                        
                                        <asp:Button ID="btnReport" runat="server" Text="Load Report" Width="90px" CssClass="btn">
                                        </asp:Button>
                                        <asp:Button ID="btnCancel" runat="server" Text="Close" Width="90px" CssClass="btn">
                                        </asp:Button>
                                        <asp:Button ID="btnsend" runat="server" Text="Send" Width="90px" CssClass="btn" Visible="false">
                                        </asp:Button>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
    <script type="text/javascript">
        $(document).ready(function () {
            var prm = Sys.WebForms.PageRequestManager.getInstance();
            prm.add_initializeRequest(InitializeRequest);
            prm.add_endRequest(EndRequest);

            // Place here the first init of the autocomplete
            MyAutoCustomer();
        });

        function InitializeRequest(sender, args) {

        }

        function EndRequest(sender, args) {
            // after update occur on UpdatePanel re-init the Autocomplete
            MyAutoCustomer();
        }
    </script>
</asp:Content>
