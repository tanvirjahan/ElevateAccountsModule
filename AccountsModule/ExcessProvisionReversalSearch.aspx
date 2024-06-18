<%@ Page Title="" Language="VB" MasterPageFile="~/AccountsMaster.master" AutoEventWireup="false"
    CodeFile="ExcessProvisionReversalSearch.aspx.vb" Inherits="ExcessProvisionReversalSearch" %>
        <%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
 <%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ OutputCache Location="none" %>
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
            }
        }


        function FormValidation(state) {

            if ((document.getElementById("<%=txtFromDate.ClientID%>").value == '') || (document.getElementById("<%=txtToDate.ClientID%>").value == '')) {

                alert("Select Dates ");
                return false;
            }
            
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

            var vdivCode = document.getElementById("vdivcode");
            var $txtvsprocess = $(document).find('.cs_txtvsprocess');

            $txtvsprocess.val('JOURNALNO:" " NARRATION: " " TEXT:" "');

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

                        switch (category) {
                            case 'JOURNALNO':
                                var asSqlqry = "select  ltrim(rtrim(tran_id)) from  provisionReversal_master(nolock) where divcode='" + vdivCode.value + "' order by tran_id ";
                                glcallback = callback;
                                ColServices.clsServices.GetListOfArrayVisualSearch(asSqlqry, fnFillSearchVS, ErrorHandler, TimeOutHandler);
                                break;
                            case 'NARRATION':
                                var asSqlqry = "select  ltrim(rtrim(narration)) from provisionReversal_master(nolock) where divcode='" + vdivCode.value + "' order by narration ";
                                glcallback = callback;
                                ColServices.clsServices.GetListOfArrayVisualSearch(asSqlqry, fnFillSearchVS, ErrorHandler, TimeOutHandler);
                                break;                            
                        }
                    },
                    facetMatches: function (callback) {
                        callback([
                { label: 'JOURNALNO', category: 'Journal' },
                { label: 'NARRATION', category: 'Journal' },               
                { label: 'TEXT', category: 'Journal' },
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
          <asp:UpdatePanel ID="UpdatePanel3" runat="server">
     <ContentTemplate>
    <table style="border-right: gray 2px solid; width: 100%; border-top: gray 2px solid;
        border-left: gray 2px solid; border-bottom: gray 2px solid;" class="td_cell">
        <tbody>
            <tr>
                <td class="field_heading" align="center">
                    Excess Provision Reversal
                </td>
            </tr>
            <tr>
                <td style="color: blue;" class="td_cell" align="center">
                    Type few characters of code or name and click search &nbsp; &nbsp;
                </td>
            </tr>
            <tr>
                <td>
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <table width="100%">
                                <tbody>
                                    <tr>
                                        <td align="center" colspan="4">
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:Button ID="btnhelp" TabIndex="3" OnClick="btnhelp_Click" runat="server" Text="Help"
                                                            Font-Bold="False" CssClass="btn"></asp:Button>
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnAddNew" TabIndex="4" runat="server" Text="Add New" Font-Bold="False"
                                                            CssClass="btn"></asp:Button>
                                                    </td>
                                                    <td>
                                                        &nbsp;&nbsp;<asp:Button ID="btnPrint_new" TabIndex="5" runat="server" Text="Report"
                                                            CssClass="btn"></asp:Button>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td align="center">
                                            &nbsp;
                                        </td>
                                        <td>
                                        </td>
                                    </tr>                                   
                                </tbody>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
              </TD>
    </TR>
     <tr>                                  
        <td>
                    <div style="width: 100%">
                        <div style="width: 80%; display: inline-block; margin: -6px 4px 0 0;">
                            <div id="VS" class="container" style="border: 0px;">
                                <div id="search_box_container">
                                </div>

                                <asp:TextBox ID="txtvsprocess" runat="server" class="cs_txtvsprocess" Style="display: none"></asp:TextBox>
                                <asp:TextBox ID="txtvsprocesssplit" runat="server" class="cs_txtvsprocesssplit" Style="display: none"></asp:TextBox>
                                <asp:Button ID="btnvsprocess" runat="server" Style="display: none" />
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
                <td>
                    <table class="style1">
                        <tr>
                            <td>
                               <asp:HiddenField ID="hdnappid" runat="server" />
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
                                    <asp:ListItem Value="T">Transaction Date</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="Label2" runat="server" CssClass="field_caption" Text="From Date"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtFromDate" runat="server" onchange="filltodate(this);" Width="75px"></asp:TextBox>
                                <asp:ImageButton ID="ImgBtnFrmDt" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.png"
                                    TabIndex="3" />
                                <asp:calendarextender id="dpFromDate" runat="server" format="dd/MM/yyyy" onclientdateselectionchanged="DateSelectCalExt"
                                    popupbuttonid="ImgBtnFrmDt" popupposition="Right" targetcontrolid="txtFromDate">
                        </asp:calendarextender>
                                <asp:maskededitextender id="MeFromDate" runat="server" mask="99/99/9999" masktype="Date"
                                    targetcontrolid="txtFromDate">
                        </asp:maskededitextender>
                                <asp:maskededitvalidator id="MevFromDate" runat="server" controlextender="MeFromDate"
                                    controltovalidate="txtFromDate" cssclass="field_error" display="Dynamic" emptyvalueblurredtext="Date is required"
                                    emptyvaluemessage="Date is required" errormessage="MeFromDate" invalidvalueblurredmessage="Invalid Date"
                                    invalidvaluemessage="Invalid Date" tooltipmessage="Input a Date in Date/Month/Year"></asp:maskededitvalidator>
                            </td>
                            <td>
                                <asp:Label ID="Label5" runat="server" CssClass="field_caption" Text="To Date"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtToDate" runat="server" onchange="ValidateChkInDate();" Width="75px"></asp:TextBox>
                                <asp:ImageButton ID="ImgBtnToDt" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.png"
                                    TabIndex="3" />
                                <asp:calendarextender id="dpToDate" runat="server" format="dd/MM/yyyy" onclientdateselectionchanged="DateSelectCalExt"
                                    popupbuttonid="ImgBtnToDt" popupposition="Right" targetcontrolid="txtToDate">
                        </asp:calendarextender>
                                <asp:maskededitextender id="MeToDate" runat="server" mask="99/99/9999" masktype="Date"
                                    targetcontrolid="txtToDate">
                        </asp:maskededitextender>
                                <asp:maskededitvalidator id="MevToDate" runat="server" controlextender="MeToDate"
                                    controltovalidate="txtToDate" cssclass="field_error" display="Dynamic" emptyvalueblurredtext="Date is required"
                                    emptyvaluemessage="Date is required" errormessage="MeFromDate" invalidvalueblurredmessage="Invalid Date"
                                    invalidvaluemessage="Invalid Date" tooltipmessage="Input a Date in Date/Month/Year"></asp:maskededitvalidator>
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
        </tbody>
    </table>
    </contenttemplate> </asp:UpdatePanel>
<%--    </td> </tr>--%>
    <tr>
        <td>
            &nbsp;
        </td>
    </tr>
    <tr>
        <td>
         <%--   <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                <ContentTemplate>--%>
                    <asp:GridView ID="gv_SearchResult" TabIndex="11" runat="server" Width="100%" CssClass="grdstyle"
                        AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False" CellPadding="3"
                        GridLines="Vertical">
                        <Columns>
                            <asp:TemplateField Visible="False" HeaderText="Transaction Id">
                                <ItemTemplate>
                                    <asp:Label ID="lblTranID" runat="server" Text='<%# Bind("tran_id") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="tran_id" SortExpression="tran_id" HeaderText="Journal No">
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="Journal Type" SortExpression="tran_type">
                                <ItemTemplate>
                                    <asp:Label ID="lblTranType" runat="server" Text='<%# Bind("tran_type") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>                            
                            <asp:BoundField HtmlEncode="False" DataFormatString="{0:dd/MM/yyyy}" DataField="journal_date"
                                SortExpression="journal_date" HeaderText="Journal Date"></asp:BoundField>
                            <asp:BoundField DataField="journal_narration" SortExpression="journal_narration"
                                HeaderText="Narration"></asp:BoundField>
                            <asp:TemplateField HeaderText="Reversal Amount">                                
                                <ItemTemplate>
                                    <asp:Label ID="lblReversalAmt" runat="server" Text='<%# Bind("totalReversalAmount") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Reversal VAT Amount">                                
                                <ItemTemplate>
                                    <asp:Label ID="lblReversalVatAmt" runat="server" Text='<%# Bind("totalReversalVatAmount") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField HtmlEncode="False" DataFormatString="{0:dd/MM/yyyy hh:mm:ss tt} "
                                DataField="adddate" SortExpression="adddate" HeaderText="Date Created"></asp:BoundField>
                            <asp:BoundField DataField="adduser" SortExpression="adduser" HeaderText="User Created">
                            </asp:BoundField>
                            <asp:BoundField HtmlEncode="False" DataFormatString="{0:dd/MM/yyyy hh:mm:ss tt} "
                                DataField="moddate" SortExpression="moddate" HeaderText="Date Modified"></asp:BoundField>
                            <asp:BoundField DataField="moduser" SortExpression="moduser" HeaderText="User Modified">
                            </asp:BoundField>
                                <asp:TemplateField  HeaderText="Action">
                                                            <ItemTemplate>
                                                                              <asp:LinkButton ID="lbEditDate" runat="server" OnClick="lbEditDate_Click" Text="Edit DocumentDate"></asp:LinkButton>
                                                                 <asp:Label ID="lblTranTypePop" runat="server" Visible="false"   Text='<%# Bind("tran_type") %>'></asp:Label>
                                                      <asp:Label ID="lbltranidPop"  Visible="false" runat="server" Text='<%# Bind("tran_id") %>'></asp:Label>
                                                             <asp:Label ID="lblTrandatePop" runat="server" Visible="false"   Text='<%# Bind("journal_date") %>'></asp:Label>
                        
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                            <asp:ButtonField HeaderText="Action" Text="Edit" CommandName="EditRow">
                                <ItemStyle ForeColor="Blue"></ItemStyle>
                            </asp:ButtonField>
                            <asp:ButtonField HeaderText="Action" Text="View" CommandName="View">
                                <ItemStyle ForeColor="Blue"></ItemStyle>
                            </asp:ButtonField>
                            <asp:ButtonField HeaderText="Action" Text="Delete" CommandName="DeleteRow">
                                <ItemStyle ForeColor="Blue"></ItemStyle>
                            </asp:ButtonField>
                            <asp:ButtonField HeaderText="Action" Text="View Log" CommandName="ViewLog">
                                <ItemStyle ForeColor="Blue"></ItemStyle>
                            </asp:ButtonField>
                        </Columns>
                        <FooterStyle CssClass="grdfooter" />
                        <RowStyle CssClass="grdRowstyle"></RowStyle>
                        <SelectedRowStyle CssClass="grdselectrowstyle"></SelectedRowStyle>
                        <PagerStyle CssClass="grdpagerstyle" HorizontalAlign="Center"></PagerStyle>
                        <HeaderStyle CssClass="grdheader" ForeColor="white"></HeaderStyle>
                        <AlternatingRowStyle CssClass="grdAternaterow"></AlternatingRowStyle>
                    </asp:GridView>
                    <asp:Label ID="lblMessg" runat="server" Text="Records not found. Please redefine search criteria"
                        Font-Size="8pt" Font-Names="Verdana" Font-Bold="True" Width="357px" CssClass="lblmsg"></asp:Label>
                    <asp:HiddenField ID="hdnDecimalplaces" runat="server" />
                     <asp:HiddenField ID="txtdivcode" runat="server" />
                                                   <asp:HiddenField ID="hdntrantype" runat="server" />

                                                                 <asp:HiddenField ID="hdFlightDetails" runat="server" />
                                                                   <asp:ModalPopupExtender ID="ModalFlightDetails" runat="server" BehaviorID="ModalFlightDetails"
                                CancelControlID="Td2" TargetControlID="hdFlightDetails" PopupControlID="dvFlightDetails"
                                PopupDragHandleControlID="Td1" Drag="true" BackgroundCssClass="ModalPopupBG">
                            </asp:ModalPopupExtender>
                                 <div id="dvFlightDetails" runat="server" style="min-height: 150px; max-height: 200px;
                                width: 25%; border: 3px solid #06788B; background-color: White;">
                                <table style="width: 98%; padding: 5px 5px 5px 5px">
                                    <tr>
                                      
                                        <td id="Td1" bgcolor="#06788B">
                                            <asp:Label ID="lblViewDetailsPopupHeading" runat="server" CssClass="field_heading"
                                                Style="padding: 3px 0px 3px 3px" Text="Edit Receipt Date" Width="205px"></asp:Label>
                                        </td>
                                        <td align="center" id="Td2" bgcolor="#06788B">
                                            <asp:Label ID="Label3" Text="X" runat="server" Font-Bold="True" Font-Names="Verdana"
                                                Font-Size="Large" ForeColor="White"></asp:Label>
                                        </td>
                                    </tr>
                                   
                                    <tr>
                                     
                                  
                                            <td>
                                           <div style="padding-top: 15px; padding-left: 20%;">
                                        
                                                <asp:Label ID="lblpopupreceipt" runat="server" Text="Receipt Date." Width="100px" CssClass="field_caption"></asp:Label>
                                               
                                       
                                                 <asp:TextBox ID="txtdate" runat="server" onchange="filltodate(this);" Width="75px"></asp:TextBox>
                                                   <asp:ImageButton ID="ImgBtnFrmDt1" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.png"
                                                     TabIndex="3" />
                                                 <cc1:CalendarExtender ID="dpFromDate1" runat="server" Format="dd/MM/yyyy" OnClientDateSelectionChanged="DateSelectCalExt"
                                                     PopupButtonID="ImgBtnFrmDt1" PopupPosition="Right" TargetControlID="txtdate">
                                                 </cc1:CalendarExtender>
                                                 <cc1:MaskedEditExtender ID="MeFromDate1" runat="server" Mask="99/99/9999" MaskType="Date"
                                                     TargetControlID="txtdate">
                                                 </cc1:MaskedEditExtender>
                                                 <cc1:MaskedEditValidator ID="MevFromDate1" runat="server" ControlExtender="MeFromDate"
                                                     ControlToValidate="txtdate" CssClass="field_error" Display="Dynamic" EmptyValueBlurredText="Date is required"
                                                     EmptyValueMessage="Date is required" ErrorMessage="MeFromDate" InvalidValueBlurredMessage="Invalid Date"
                                                     InvalidValueMessage="Invalid Date" TooltipMessage="Input a Date in Date/Month/Year"></cc1:MaskedEditValidator>
                                                   </div>
                              </td>
                                    </tr>


                                    <tr>
                                    <td>
                                    </td>
                                    </tr>
                                       <tr>
                                    <td>
                                    </td>
                                    </tr>
                                    <tr  >
                                    <td >


                                  <div style="padding-top: 15px; padding-left: 40%;">
                                           <asp:Button ID="btnUpdate" TabIndex="37" runat="server" CssClass="btn" Font-Bold="True"    Style="margin-top: 2px;"
                                    OnClick="btnUpdate_Click" Text="Save" />
                                          <asp:Button ID="btnFlightCancel" CssClass="btn" Style="margin-top: 2px;" OnClick="btnFlightCancel_Click"
                                        runat="server" Text="Cancel" />
                                        <asp:HiddenField ID="hdntranid" runat="server" /></td>
                                                     <asp:HiddenField ID="hdntrantypeDate" runat="server" /></td>
                                                 
                                    </div>

                                        </td>                              
                                    </tr>
                                   <%--   </table>--%>
                            
                 
                        </div>
                        
                                 </td>
                             </tr>
                             <tr>
                       
                                                </tr>
                     </table>
                 </td>
  <%--               <td>  <asp:HiddenField ID="hdOPMode" runat="server" /></td>--%>
             </tr>
                   <tr>
                             <asp:TextBox ID="txtpdate" runat="server" Visible="False"
                                                TabIndex="32" Width="1px"></asp:TextBox>
                                                </tr>




                </ContentTemplate>
            </asp:UpdatePanel>
          
 <%--       </td>
    </tr>
       </table>--%>
    <asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server">
        <Services>
            <asp:ServiceReference Path="../clsSectorServices.asmx" />
            <asp:ServiceReference Path="~/clsServices.asmx" />
        </Services>
    </asp:ScriptManagerProxy>

</asp:Content>
