<%@ Page Language="VB" MasterPageFile="~/AccountsMaster.master" AutoEventWireup="false"
    CodeFile="ReceiptsSearch1.aspx.vb" Inherits="ReceiptsSearch" %>
    <%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
 <%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
    <%@ OutputCache location="none" %> 
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="Server">
    <link rel="stylesheet" href="../Content/lib/css/reset.css" type="text/css" media="screen"
        charset="utf-8">
    <link rel="stylesheet" href="../Content/lib/css/icons.css" type="text/css" media="screen"
        charset="utf-8">
    <link rel="stylesheet" href="../Content/lib/css/workspace.css" type="text/css" media="screen"
        charset="utf-8">
    <link href="../css/VisualSearch.css" rel="stylesheet" type="text/css" />
    <script src="../Content/vendor/jquery-1.11.0.js" type="text/javascript" charset="utf-8"></script>
    <script src="../Content/vendor/jquery.ui.core.js" type="text/javascript" charset="utf-8"></script>
    <script src="../Content/vendor/jquery.ui.widget.js" type="text/javascript" charset="utf-8"></script>
    <script src="../Content/vendor/jquery.ui.position.js" type="text/javascript" charset="utf-8"></script>
    <script src="../Content/vendor/jquery.ui.menu.js" type="text/javascript" charset="utf-8"></script>
    <script src="../Content/vendor/jquery.ui.autocomplete.js" type="text/javascript"
        charset="utf-8"></script>
    <script src="../Content/vendor/underscore-1.5.2.js" type="text/javascript" charset="utf-8"></script>
    <script src="../Content/vendor/backbone-1.1.0.js" type="text/javascript" charset="utf-8"></script>
    <script src="../Content/lib/js/visualsearch.js" type="text/javascript" charset="utf-8"></script>
    <script src="../Content/lib/js/views/search_box.js" type="text/javascript" charset="utf-8"></script>
    <script src="../Content/lib/js/views/search_facet.js" type="text/javascript" charset="utf-8"></script>
    <script src="../Content/lib/js/views/search_input.js" type="text/javascript" charset="utf-8"></script>
    <script src="../Content/lib/js/models/search_facets.js" type="text/javascript" charset="utf-8"></script>
    <script src="../Content/lib/js/models/search_query.js" type="text/javascript" charset="utf-8"></script>
    <script src="../Content/lib/js/utils/backbone_extensions.js" type="text/javascript"
        charset="utf-8"></script>
    <script src="../Content/lib/js/utils/hotkeys.js" type="text/javascript" charset="utf-8"></script>
    <script src="../Content/lib/js/utils/jquery_extensions.js" type="text/javascript"
        charset="utf-8"></script>
    <script src="../Content/lib/js/utils/search_parser.js" type="text/javascript" charset="utf-8"></script>
    <script src="../Content/lib/js/utils/inflector.js" type="text/javascript" charset="utf-8"></script>
    <script src="../Content/lib/js/templates/templates.js" type="text/javascript" charset="utf-8"></script>
    <script type="text/javascript" charset="utf-8">

        function DateSelectCalExt() {
            var txtfromDate = document.getElementById("<%=txtfromDate.ClientID%>");
            if (txtfromDate.value != '') {
                var calendarBehavior1 = $find("<%=dpFromDate.ClientID %>");  // document.getElementById("<%=dpFromDate.ClientID%>"); 
                var date = calendarBehavior1._selectedDate;

                var dp = txtfromDate.value.split("/");
                var newDt = new Date(dp[2] + "/" + dp[1] + "/" + dp[0]);
                newDt = getFormatedDate(newDt);
                newDt = new Date(newDt);
                calendarBehavior1.set_selectedDate(newDt);
            }
            var txtfromDate2 = document.getElementById("<%=txtToDate.ClientID%>");
            if (txtfromDate2.value != '') {
                var calendarBehavior2 = $find("<%=dpToDate.ClientID %>");  // document.getElementById("<%=dpFromDate.ClientID%>"); 
                var date2 = calendarBehavior2._selectedDate;

                var dp2 = txtfromDate2.value.split("/");
                var newDt2 = new Date(dp2[2] + "/" + dp2[1] + "/" + dp2[0]);
                newDt2 = getFormatedDate(newDt2);
                newDt2 = new Date(newDt2);
                calendarBehavior2.set_selectedDate(newDt2);
            }

        }
        function getFormatedDate(chkdate) {
            var dd = chkdate.getDate();
            var mm = chkdate.getMonth() + 1; //January is 0!
            var yyyy = chkdate.getFullYear();
            if (dd < 10) { dd = '0' + dd };
            if (mm < 10) { mm = '0' + mm };
            chkdate = mm + '/' + dd + '/' + yyyy;
            return chkdate;
        }

        function filltodate(fDate) {
            var txtfromDate = document.getElementById("<%=txtFromDate.ClientID%>");
            var txtToDate = document.getElementById("<%=txtToDate.ClientID%>");

            if ((txtToDate.value != null) && (txtToDate.value != '')) {

                var dpTo = txtToDate.value.split("/");

                var newDtTo = new Date(dpTo[2] + "/" + dpTo[1] + "/" + dpTo[0]);
                var today = new Date();

                newDtTo = getFormatedDate(newDtTo);
                today = getFormatedDate(today);
                newDtTo = new Date(newDtTo);
                today = new Date(today);

                if (newDt > newDtTo) {

                    txtToDate.value = txtfromDate.value;
                    DateSelectCalExt();
                    return;
                }

                //}
            }

            //            if (txtfromDate.value != null) {
            //                var dp = txtfromDate.value.split("/");

            //                var newDt = new Date(dp[2] + "/" + dp[1] + "/" + dp[0]);
            //                var today = new Date();
            ////                alert(newDt);
            ////                alert(today);
            //                newDt = getFormatedDate(newDt);
            //                alert(newDt);
            //                today = getFormatedDate(today);

            //                newDt = new Date(newDt);
            //                today = new Date(today);
            //             
            //                if (newDt < today) {

            //                    alert('From date should not be less than todays date.');
            //                  //  txtfromDate.value = curDate.value;
            //                    return;
            //                }
            //                else {
            //             
            //                }


        }

        function ValidateChkInDate() {

            var txtfromDate = document.getElementById("<%=txtfromDate.ClientID%>");
            var txtToDate = document.getElementById("<%=txtToDate.ClientID%>");
            if (txtfromDate.value == null || txtfromDate.value == "") {
                txtToDate.value = "";
                alert("Please select From date.");
            }

            var dp = txtfromDate.value.split("/");
            var newChkInDt = new Date(dp[2] + "/" + dp[1] + "/" + dp[0]);

            var dp1 = txtToDate.value.split("/");
            var newChkOutDt = new Date(dp1[2] + "/" + dp1[1] + "/" + dp1[0]);

            newChkInDt = getFormatedDate(newChkInDt);
            newChkOutDt = getFormatedDate(newChkOutDt);

            newChkInDt = new Date(newChkInDt);
            newChkOutDt = new Date(newChkOutDt);
            if (newChkInDt > newChkOutDt) {
                txtToDate.value = txtfromDate.value;
                alert("Todate date should not be greater than From date");
            }
        }

        $(document).ready(function () {
            visualsearchbox();
        });

        var glcallback;

        function TimeOutHandler(result) {
            alert("Timeout :" + result);
        }

        function ErrorHandler(result) {
            var msg = result.get_exceptionType() + "\r\n";
            msg += result.get_message() + "\r\n";
            msg += result.get_stackTrace();
            alert(msg);
        }

        function visualsearchbox() {


            var $txtvsprocess = $(document).find('.cs_txtvsprocess');

            $txtvsprocess.val('DocumentNo:" " AccountName: " "  BankName:" " Status:" "TEXT:" "');

            window.visualSearch = VS.init({
                container: $('#search_box_container'),
                query: $txtvsprocess.val(),
                showFacets: true,
                readOnly: false,
                unquotable: [
            'text',
            'account',
            'filter',
            'access'
          ],
                placeholder: 'Search for..',
                callbacks: {
                    search: function (query, searchCollection) {
                        var $txtvsprocess = $(document).find('.cs_txtvsprocess');
                        $txtvsprocess.val(visualSearch.searchQuery.serialize());

                        var $txtvsprocesssplit = $(document).find('.cs_txtvsprocesssplit');
                        $txtvsprocesssplit.val(visualSearch.searchQuery.serializetosplit());

                        var btnvsprocess = document.getElementById("<%=btnvsprocess.ClientID%>");
                        btnvsprocess.click();

                        clearTimeout(window.queryHideDelay);
                        window.queryHideDelay = setTimeout(function () {
                            $query.animate({
                                opacity: 0
                            }, {
                                duration: 1000,
                                queue: false
                            });
                        }, 2000);
                    },
                    valueMatches: function (category, searchTerm, callback) {
                        var hdndivcode = document.getElementById("<%=txtDivcode.ClientID%>");
                        switch (category) {
                            case 'DocumentNo':
                                var asSqlqry = "select ltrim(rtrim(tran_id)) tranid  from receipt_master_new (nolock) where tran_type='RV' and receipt_div_id = '" + hdndivcode.value + "'order by tranid   ";
                                glcallback = callback;
                                ColServices.clsServices.GetListOfArrayVisualSearch(asSqlqry, fnFillSearchVS, ErrorHandler, TimeOutHandler);
                                break;
                            case 'BankName':
                                var asSqlqry = "select ltrim(rtrim(acctname)) from acctmast ,bank_master_type where  acctmast.bank_master_type_code = bank_master_type.bank_master_type_code and bank_master_type.cashbanktype='C'  and  bankyn='Y' and acctmast.div_code= '" + hdndivcode.value + "' order by acctcode  ";
                                glcallback = callback;
                                ColServices.clsServices.GetListOfArrayVisualSearch(asSqlqry, fnFillSearchVS, ErrorHandler, TimeOutHandler);
                                break;
                            case 'AccountName':
                                var asSqlqry = '';
                                glcallback = callback;
                                ColServices.clsSectorServices.GetListOfacctnameVisualSearch(asSqlqry, fnFillSearchVS, ErrorHandler, TimeOutHandler);
                                break;
                            case 'Status':
                                var asSqlqry = '';
                                glcallback = callback;
                                ColServices.clsSectorServices.GetListOfCustBankVisualSearch(asSqlqry, fnFillSearchVS, ErrorHandler, TimeOutHandler);
                                break;
                        }
                    },
                    facetMatches: function (callback) {
                        callback([
                { label: 'Country', category: 'Cities' },
                { label: 'City', category: 'Cities' },
                { label: 'Text', category: 'Cities' },
              ]);
                    }
                }
            });
        }

        function fnFillSearchVS(result) {
            glcallback(result, {
                preserveOrder: true // Otherwise the selected value is brought to the top
            });
        }
    </script>
    <script type="text/javascript">

        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_initializeRequest(InitializeRequest);
        prm.add_endRequest(EndRequest);

        function InitializeRequest(sender, args) {

        }

        function EndRequest(sender, args) {
            // after update occur on UpdatePanel re-init the Autocomplete
            visualsearchbox();
        }
    </script>
    <script language="javascript" type="text/javascript">

        function checkNumber(e) {

            if (event.keycode = 13) {
                return true;
            }
            if ((event.keyCode < 47 || event.keyCode > 57)) {
                return false;
            }

        }
        function checkCharacter(e) {
            if (event.keycode = 13) {
                return true;
            }

            if (event.keyCode == 32 || event.keyCode == 46)
                return;
            if ((event.keyCode < 47 || event.keyCode > 57) && (event.keyCode < 65 || event.keyCode > 91) && (event.keyCode < 96 || event.keyCode > 122)) {
                return false;
            }

        }
				
    </script>
    <table style="border-right: gray 2px solid; border-top: gray 2px solid; border-left: gray 2px solid;
        border-bottom: gray 2px solid">
        <tr>
            <td style="width: 100%; height: 11px">
                <table style="width: 100%">
                    <tr>
                 <td valign="top" align="center" width="150" colspan="2" class="style1">
                            <asp:Label ID="lblHeading" runat="server" Text="Suppliers" CssClass="field_heading"
                                Width="100%" ForeColor="White"></asp:Label>
                        </td>
                    </tr>
                    <tr style="display: none">
                        <td align="center" class="td_cell" style="color: blue">
                            Type few characters of code or name and click search
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 21px">
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                <ContentTemplate>
                                    <table style="width: 872px">
                                        <tbody>
                                            <tr>
                                                <td align="center">
                                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                    &nbsp; &nbsp;&nbsp; &nbsp;<asp:Button ID="btnhelp" TabIndex="3" OnClick="btnhelp_Click"
                                                        runat="server" Text="Help" Font-Bold="False" CssClass="search_button"></asp:Button>
                                                    &nbsp;&nbsp;<asp:Button ID="btnAddNew" TabIndex="4" runat="server" Text="Add New"
                                                        Font-Bold="False" CssClass="btn"></asp:Button>&nbsp;&nbsp;<asp:Button ID="btnPrint"
                                                            TabIndex="5" runat="server" Text="Report" CssClass="btn"></asp:Button>
                                        
                                                   <%-- <asp:Label runat="server" id="txtDivcode" style="display:none;"/>--%>
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                    &nbsp;
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <tr>
                            <td colspan="6">
                                <div style="width: 100%">
                                    <div style="width: 80%; display: inline-block; margin: -6px 4px 0 0;">
                                        <div id="VS" class="container" style="border: 0px;">
                                            <div id="search_box_container">
                                            </div>
                                            <asp:TextBox ID="txtvsprocess" runat="server" class="cs_txtvsprocess" Style="display: none"></asp:TextBox>
                                            <asp:TextBox ID="txtvsprocesssplit" runat="server"  
                                                class="cs_txtvsprocesssplit" ReadOnly="True"></asp:TextBox>
                                            <asp:Button ID="btnvsprocess" runat="server" />
                                        </div>
                                    </div>
                                    <div style="width: 18%; display: inline-block; vertical-align: top;">
                                        <asp:Button ID="btnResetSelection" runat="server" CssClass="btn" Font-Bold="False"
                                            TabIndex="4" Text="Reset Search" /></div>
                                </div>
                                <asp:DataList ID="dlList" runat="server" RepeatColumns="4" RepeatDirection="Horizontal">
                                    <ItemTemplate>
                                        <table class="styleDatalist" style="border: 0px;">
                                            <tr style="">
                                                <td style="border: 0px;">
                                                    <asp:Button ID="lnkCode" runat="server" class="button button4" Style="display: none"
                                                        Text='<%# Eval("Code") %>' />
                                                    <asp:Button ID="lnkValue" runat="server" class="button button4" Style="display: none"
                                                        Text='<%# Eval("Value") %>' />
                                                    <asp:Button ID="lnkCodeAndValue" runat="server" class="button button4" Font-Bold="False"
                                                        Font-Size="Small" ForeColor="#000099" OnClientClick="return false;" Text='<%# Eval("CodeAndValue") %>' />
                                                    <asp:Button ID="lbClose" runat="server" class="buttonClose button4" OnClick="lbClose_Click"
                                                        Text="X" />
                                            </tr>
                                        </table>
                                    </ItemTemplate>
                                </asp:DataList>
                            </td>
                        </tr>
                    <tr>
                        <td colspan="6">
                            <table class="style1">
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        <asp:Label ID="Label6" runat="server" CssClass="field_caption" Text="Filter By "></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlOrder" runat="server">
                                            <asp:ListItem Value="C">Created Date</asp:ListItem>
                                            <asp:ListItem Value="M">Modified Date</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label4" runat="server" CssClass="field_caption" Text="From Date"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtFromDate" runat="server" onchange="filltodate(this);" Width="75px"></asp:TextBox>
                                        <asp:ImageButton ID="ImgBtnFrmDt" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.png"
                                            TabIndex="3" />
                                        <cc1:CalendarExtender ID="dpFromDate" runat="server" Format="dd/MM/yyyy" OnClientDateSelectionChanged="DateSelectCalExt"
                                            PopupButtonID="ImgBtnFrmDt" PopupPosition="Right" TargetControlID="txtFromDate">
                                        </cc1:CalendarExtender>
                                        <cc1:MaskedEditExtender ID="MeFromDate" runat="server" Mask="99/99/9999" MaskType="Date"
                                            TargetControlID="txtFromDate">
                                        </cc1:MaskedEditExtender>
                                        <cc1:MaskedEditValidator ID="MevFromDate" runat="server" ControlExtender="MeFromDate"
                                            ControlToValidate="txtFromDate" CssClass="field_error" Display="Dynamic" EmptyValueBlurredText="Date is required"
                                            EmptyValueMessage="Date is required" ErrorMessage="MeFromDate" InvalidValueBlurredMessage="Invalid Date"
                                            InvalidValueMessage="Invalid Date" TooltipMessage="Input a Date in Date/Month/Year"></cc1:MaskedEditValidator>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label5" runat="server" CssClass="field_caption" Text="To Date"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtToDate" runat="server" onchange="ValidateChkInDate();" Width="75px"></asp:TextBox>
                                        <asp:ImageButton ID="ImgBtnToDt" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.png"
                                            TabIndex="3" />
                                        <cc1:CalendarExtender ID="dpToDate" runat="server" Format="dd/MM/yyyy" OnClientDateSelectionChanged="DateSelectCalExt"
                                            PopupButtonID="ImgBtnToDt" PopupPosition="Right" TargetControlID="txtToDate">
                                        </cc1:CalendarExtender>
                                        <cc1:MaskedEditExtender ID="MeToDate" runat="server" Mask="99/99/9999" MaskType="Date"
                                            TargetControlID="txtToDate">
                                        </cc1:MaskedEditExtender>
                                        <cc1:MaskedEditValidator ID="MevToDate" runat="server" ControlExtender="MeToDate"
                                            ControlToValidate="txtToDate" CssClass="field_error" Display="Dynamic" EmptyValueBlurredText="Date is required"
                                            EmptyValueMessage="Date is required" ErrorMessage="MeFromDate" InvalidValueBlurredMessage="Invalid Date"
                                            InvalidValueMessage="Invalid Date" TooltipMessage="Input a Date in Date/Month/Year"></cc1:MaskedEditValidator>
                                    </td>
                                    <td>
                                        <asp:Button ID="btnFilter" runat="server" CssClass="btn" Font-Bold="False" TabIndex="4"
                                            Text="Search by Date" />
                                        &nbsp;<asp:Button ID="btnClearDate" runat="server" CssClass="btn" Font-Bold="False"
                                            TabIndex="4" Text="Reset Dates" />
                                    </td>
                                    <td>
                                        <asp:Label ID="RowSelectcs" runat="server" CssClass="field_caption" Text="Rows Selected "></asp:Label>
                                        <asp:DropDownList ID="RowsPerPageCUS" runat="server" AutoPostBack="true">
                                            <asp:ListItem Value="5">5</asp:ListItem>
                                            <asp:ListItem Value="10">10</asp:ListItem>
                                            <asp:ListItem Value="15">15</asp:ListItem>
                                            <asp:ListItem Value="20">20</asp:ListItem>
                                            <asp:ListItem Value="25">25</asp:ListItem>
                                            <asp:ListItem Value="30">30</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <tr>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                    </tr>
                              </table>
             
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 15px; width: 100%">
                            &nbsp;<asp:Button ID="btnExportToExcel" runat="server" CssClass="btn" Text="Export To Excel"
                                TabIndex="8" />
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 33px; width: 100%">
                            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                <ContentTemplate>
                                        <asp:HiddenField ID="txtdivcode" runat="server" />
                                 <asp:GridView ID="gv_SearchResult" TabIndex="19" runat="server" Width="100%" AllowPaging="True"
                                        AllowSorting="True" AutoGenerateColumns="false" CellPadding="3" GridLines="Vertical">
                                        <Columns>
                                 <asp:TemplateField HeaderText="Tran ID" SortExpression="tran_id">
                                                <ItemTemplate>
                                                    <asp:Label ID="Label1" runat="server" Text='<%# Bind("tran_id") %>'></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="TextBox2" runat="server" Text='<%# Bind("tran_id") %>'></asp:TextBox>
                                                </EditItemTemplate>
                                                <ItemStyle Wrap="False" />
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="tran_type" SortExpression="tran_type" HeaderText="Tran. Type">
                                            </asp:BoundField>
                                            <asp:BoundField DataField="post_state" HeaderText="Status"></asp:BoundField>
                                            <asp:BoundField HtmlEncode="False" DataFormatString="{0:dd/MM/yyyy}" DataField="receipt_tran_date"
                                                SortExpression="receipt_tran_date" HeaderText="Transaction Date"></asp:BoundField>
                                            <asp:BoundField HtmlEncode="False" DataFormatString="{0:dd/MM/yyyy}" DataField="receipt_date"
                                                SortExpression="receipt_date" HeaderText="Receipt Date"></asp:BoundField>
                                            <asp:BoundField DataField="receipt_credit" SortExpression="receipt_credit" HeaderText="Amount Received">
                                            </asp:BoundField>
                                            <asp:BoundField DataField="receipt_currency_id" SortExpression="receipt_currency_id"
                                                HeaderText="Currency"></asp:BoundField> <%-- OnDataBinding="TemplateFieldBind"--%>
                                            <asp:TemplateField HeaderText="Received From">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblRecievedFrom" runat="server"></asp:Label> 
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="receipt_cheque_number" SortExpression="receipt_cheque_number"
                                                HeaderText="Cheque Number"></asp:BoundField>
                                            <asp:BoundField DataField="receipt_cashbank_code" SortExpression="receipt_cashbank_code"
                                                HeaderText="Cash/Bank Code"></asp:BoundField>
                                            <asp:BoundField DataField="other_bank_master_des" SortExpression="other_bank_master_des"
                                                HeaderText="Cash/Bank Name"></asp:BoundField>
                                            <asp:BoundField HtmlEncode="False" DataFormatString="{0:dd/MM/yyyy hh:mm:ss tt} "
                                                DataField="adddate" SortExpression="adddate" HeaderText="Date Created"></asp:BoundField>
                                            <asp:BoundField DataField="adduser" SortExpression="adduser" HeaderText="User Created">
                                            </asp:BoundField>
                                            <asp:BoundField DataFormatString="{0:dd/MM/yyyy hh:mm:ss tt} " DataField="moddate"
                                                SortExpression="moddate" HeaderText="Date Modified"></asp:BoundField>
                                            <asp:BoundField DataField="moduser" SortExpression="moduser" HeaderText="User Modified">
                                            </asp:BoundField>
                                            <asp:TemplateField HeaderText="Action">
                                                <ItemStyle ForeColor="Blue"></ItemStyle>
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkEdit" runat="server" CommandName="EditRow" Text="Edit" CommandArgument='<%# Bind("tranidcomarg") %>'></asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Action">
                                                <ItemStyle ForeColor="Blue"></ItemStyle>
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkView" runat="server" CommandName="View"
                                                        Text="View" CommandArgument='<%# Bind("tranidcomarg") %>'></asp:LinkButton> <%-- OnDataBinding="TemplateFieldBind"--%>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Action">
                                                <ItemStyle ForeColor="Blue"></ItemStyle>
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkDeleteRow" runat="server" CommandName="DeleteRow" Text="DeleteRow"
                                                        CommandArgument='<%# Bind("tranidcomarg") %>'></asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Action">
                                                <ItemStyle ForeColor="Blue"></ItemStyle>
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkCopy" runat="server" CommandName="Copy" Text="Copy" CommandArgument='<%# Bind("tranidcomarg") %>'></asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Action">
                                                <ItemStyle ForeColor="Blue"></ItemStyle>
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkViewLog" runat="server" CommandName="ViewLog" Text="View Log"
                                                        CommandArgument='<%# Bind("tranidcomarg") %>'></asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Action">
                                                <ItemStyle ForeColor="Blue"></ItemStyle>
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkChkPrint" runat="server"
                                                        CommandName="lnkChkPrint" Text="Cheque Print" CommandArgument='<%# Bind("tranidcomarg") %>'></asp:LinkButton> <%-- OnDataBinding="TemplateFieldBind"--%>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <FooterStyle CssClass="grdfooter" />
                                        <RowStyle CssClass="grdRowstyle"></RowStyle>
                                        <SelectedRowStyle CssClass="grdselectrowstyle"></SelectedRowStyle>
                                        <PagerStyle CssClass="grdpagerstyle" HorizontalAlign="Center"></PagerStyle>
                                        <HeaderStyle CssClass="grdheader" ForeColor="white"></HeaderStyle>
                                        <AlternatingRowStyle CssClass="grdAternaterow"></AlternatingRowStyle>
                                    </asp:GridView>
                                    <asp:Label ID="lblMsg" runat="server" Text="Records not found. Please redefine search criteria"
                                        Font-Size="8pt" Font-Names="Verdana" ForeColor="#084573" Font-Bold="True" Width="357px"
                                        __designer:wfdid="w51" Visible="False"></asp:Label>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server">
        <Services>
            <asp:ServiceReference Path="../clsSectorServices.asmx" />
            <asp:ServiceReference Path="~/clsServices.asmx" />
        </Services>
    </asp:ScriptManagerProxy>
</asp:Content>
