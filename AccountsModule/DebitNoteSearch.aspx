    <%@ Page Language="VB" AutoEventWireup="false" CodeFile="DebitNoteSearch.aspx.vb" Inherits="DebitNoteSearch" MasterPageFile="~/AccountsMaster.master" Strict="true"  %>


 
 <%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
 <%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
    <%@ OutputCache location="none" %> 
    
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">

  <link rel="stylesheet" href="../Content/lib/css/reset.css" type="text/css" media="screen" charset="utf-8">
  <link rel="stylesheet" href="../Content/lib/css/icons.css" type="text/css" media="screen" charset="utf-8">
  <link rel="stylesheet" href="../Content/lib/css/workspace.css" type="text/css" media="screen" charset="utf-8">
  <link href="../css/VisualSearch.css" rel="stylesheet" type="text/css" />

  <script src="../Content/vendor/jquery-1.11.0.js" type="text/javascript" charset="utf-8"></script>
   
  <script src="../Content/vendor/jquery.ui.core.js" type="text/javascript" charset="utf-8"></script>
  <script src="../Content/vendor/jquery.ui.widget.js" type="text/javascript" charset="utf-8"></script>
  <script src="../Content/vendor/jquery.ui.position.js" type="text/javascript" charset="utf-8"></script>
  <script src="../Content/vendor/jquery.ui.menu.js" type="text/javascript" charset="utf-8"></script>
  <script src="../Content/vendor/jquery.ui.autocomplete.js" type="text/javascript" charset="utf-8"></script>
  <script src="../Content/vendor/underscore-1.5.2.js" type="text/javascript" charset="utf-8"></script>
  <script src="../Content/vendor/backbone-1.1.0.js" type="text/javascript" charset="utf-8"></script>
  <script src="../Content/lib/js/visualsearch.js" type="text/javascript" charset="utf-8"></script>
  <script src="../Content/lib/js/views/search_box.js" type="text/javascript" charset="utf-8"></script>
  <script src="../Content/lib/js/views/search_facet.js" type="text/javascript" charset="utf-8"></script>
  <script src="../Content/lib/js/views/search_input.js" type="text/javascript" charset="utf-8"></script> 
  <script src="../Content/lib/js/models/search_facets.js" type="text/javascript" charset="utf-8"></script> 
  <script src="../Content/lib/js/models/search_query.js" type="text/javascript" charset="utf-8"></script>
  <script src="../Content/lib/js/utils/backbone_extensions.js" type="text/javascript" charset="utf-8"></script>
  <script src="../Content/lib/js/utils/hotkeys.js" type="text/javascript" charset="utf-8"></script>
  <script src="../Content/lib/js/utils/jquery_extensions.js" type="text/javascript" charset="utf-8"></script>
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


         $txtvsprocess.val('DOCUMENTNO:" " CUSTOMER:" " SUPPLIER:" " SUPPLIERAGENT:"  " STATUS:"  " TEXT:" "');

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
                     var divcode = document.getElementById("<%=hdndivcode.ClientID%>");
                     var trantype = document.getElementById("<%=hdntrantype.ClientID%>");

                     switch (category) {
                         case 'DOCUMENTNO':
                             var asSqlqry = "select  ltrim(rtrim(tran_id)) from trdpurchase_master where div_id='" + divcode.value + "' and tran_type='" + trantype.value + "'  order by tran_id";
                             glcallback = callback;
                             // ColServices.clsSectorServices.GetListOfDDocNoVisualSearch(asSqlqry, fnFillSearchVS, ErrorHandler, TimeOutHandler);
                             ColServices.clsServices.GetListOfArrayVisualSearch(asSqlqry, fnFillSearchVS, ErrorHandler, TimeOutHandler);
                             break;
                         case 'CUSTOMER':
                             var asSqlqry = " select des FROM view_account WHERE TYPE='C' and div_code='" + divcode.value + "' order by des";
                             glcallback = callback;
                             ColServices.clsServices.GetListOfArrayVisualSearch(asSqlqry, fnFillSearchVS, ErrorHandler, TimeOutHandler);
                             break;
                         case 'SUPPLIER':
                             var asSqlqry = " select des FROM view_account WHERE TYPE='S' and div_code='" + divcode.value + "' order by des";
                             glcallback = callback;
                             ColServices.clsServices.GetListOfArrayVisualSearch(asSqlqry, fnFillSearchVS, ErrorHandler, TimeOutHandler);
                             break;
                         case 'SUPPLIERAGENT':
                             var asSqlqry = " select des FROM view_account WHERE TYPE='A' and div_code='" + vdivCode.value + "' order by des";
                             glcallback = callback;
                             ColServices.clsServices.GetListOfArrayVisualSearch(asSqlqry, fnFillSearchVS, ErrorHandler, TimeOutHandler);
                             break;
                         case 'STATUS':
                             var asSqlqry = " select ltrim(rtrim('Posted')) status  union all select ltrim(rtrim('UnPosted')) status ";
                             glcallback = callback;
                             ColServices.clsSectorServices.GetListOfArrayVisualSearch(asSqlqry, fnFillSearchVS, ErrorHandler, TimeOutHandler);
                             break;

                     }
                 },
                 facetMatches: function (callback) {
                     callback([
                { label: 'DOCUMENTNO', category: 'DEBIT NOTE' },
                { label: 'CUSTOMER', category: 'DEBIT NOTE' },
                 { label: 'SUPPLIER', category: 'DEBIT NOTE' },
                { label: 'SUPPLIERAGENT', category: 'DEBIT NOTE' },
                 { label: 'STATUS', category: 'DEBIT NOTE' },
             { label: 'TEXT', category: 'DEBIT NOTE' },
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

    <script language="javascript" type ="text/javascript" >
       
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

  <asp:updatepanel id="UpdatePanel1" runat="server">
  <contenttemplate>
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
                                          </td>
                                      </tr>
                                  </tbody>
                              </table>
                              &nbsp;
                          </td>
                          <tr>
                              <td colspan="6">
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
                                                  <asp:ListItem Value="T">Transaction Date</asp:ListItem>
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
                                              <asp:DropDownList ID="RowsPerPageCS" runat="server" AutoPostBack="true">
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
                                          <asp:GridView ID="gv_SearchResult" TabIndex="18" runat="server" Font-Size="10px"
                                              BackColor="White" Width="100%" CssClass="td_cell" GridLines="Vertical" CellPadding="3"
                                              BorderWidth="1px" BorderStyle="None" BorderColor="#999999" AutoGenerateColumns="False"
                                              AllowSorting="True" AllowPaging="True">
                                              <FooterStyle BackColor="#6B6B9A" ForeColor="Black"></FooterStyle>
                                              <Columns>
                                                  <asp:TemplateField Visible="False" HeaderText="Doc No">
                                                      <EditItemTemplate>
                                                          <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("tran_id") %>'></asp:TextBox>
                                                      </EditItemTemplate>
                                                      <ItemTemplate>
                                                          <asp:Label ID="lblCode" runat="server" Text='<%# Bind("tran_id") %>'></asp:Label>
                                                      </ItemTemplate>
                                                  </asp:TemplateField>
                                                  <asp:BoundField DataField="tran_id" SortExpression="tran_id" HeaderText="Doc No">
                                                  </asp:BoundField>
                                                  <asp:BoundField DataField="tran_type" SortExpression="tran_type" HeaderText="Doc Type">
                                                  </asp:BoundField>
                                                  <asp:BoundField DataField="post_state" HeaderText="Status"></asp:BoundField>
                                                  <asp:BoundField DataField="acctype" SortExpression="acctype" HeaderText="Type"></asp:BoundField>
                                                  <asp:BoundField HtmlEncode="False" DataFormatString="{0:dd/MM/yyyy}" DataField="tran_date"
                                                      SortExpression="tran_date" HeaderText="Date"></asp:BoundField>
                                                  <asp:BoundField DataField="supcode" SortExpression="supcode" HeaderText="Code"></asp:BoundField>
                                                  <asp:BoundField DataField="supname" SortExpression="supname" HeaderText="Name"></asp:BoundField>
                                                  <asp:BoundField DataField="total" SortExpression="total" HeaderText="Amount"></asp:BoundField>
                                                  <asp:BoundField HtmlEncode="False" DataFormatString="{00:dd/MM/yyyy hh:mm:ss tt}"
                                                      DataField="adddate" SortExpression="adddate" HeaderText="Date Created"></asp:BoundField>
                                                  <asp:BoundField DataField="adduser" SortExpression="adduser" HeaderText="User Created">
                                                  </asp:BoundField>
                                                  <asp:BoundField HtmlEncode="False" DataFormatString="{00:dd/MM/yyyy hh:mm:ss tt}"
                                                      DataField="moddate" SortExpression="moddate" HeaderText="Date Modified"></asp:BoundField>
                                                  <asp:BoundField DataField="moduser" SortExpression="moduser" HeaderText="User Modified">
                                                  </asp:BoundField>
                                                  <asp:ButtonField HeaderText="Action" Text="Edit" CommandName="EditRow">
                                                      <ItemStyle ForeColor="Blue"></ItemStyle>
                                                  </asp:ButtonField>
                                                  <asp:ButtonField HeaderText="Action" Text="View" CommandName="View">
                                                      <ItemStyle ForeColor="Blue"></ItemStyle>
                                                  </asp:ButtonField>
                                                  <asp:ButtonField HeaderText="Action" Text="Delete" CommandName="DeleteRow">
                                                      <ItemStyle ForeColor="Blue"></ItemStyle>
                                                  </asp:ButtonField>
                                                  <asp:ButtonField HeaderText="Action" Text="Copy" CommandName="Copy">
                                                      <ItemStyle ForeColor="Blue"></ItemStyle>
                                                  </asp:ButtonField>
                                                  <asp:ButtonField HeaderText="Action" Text="Cancel" CommandName="Cancelrow">
                                                      <ItemStyle ForeColor="Blue"></ItemStyle>
                                                  </asp:ButtonField>
                                                  <asp:ButtonField HeaderText="Action" Text="UndoCancel" CommandName="undoCancel">
                                                      <ItemStyle ForeColor="Blue"></ItemStyle>
                                                  </asp:ButtonField>
                                                  <asp:ButtonField HeaderText="Action" Text="View Log" CommandName="ViewLog">
                                                      <ItemStyle ForeColor="Blue"></ItemStyle>
                                                  </asp:ButtonField>
                                              </Columns>
                                              <RowStyle CssClass="grdRowstyle" ForeColor="Black"></RowStyle>
                                              <SelectedRowStyle BackColor="#008A8C" ForeColor="White" Font-Bold="True"></SelectedRowStyle>
                                              <PagerStyle CssClass="grdpagerstyle" HorizontalAlign="Center"></PagerStyle>
                                              <HeaderStyle CssClass="grdheader" ForeColor="white" Font-Bold="True"></HeaderStyle>
                                              <AlternatingRowStyle CssClass="grdAternaterow" Font-Size="10px"></AlternatingRowStyle>
                                          </asp:GridView>
                                          <asp:Label ID="lblMsg" runat="server" Text="Records not found. Please redefine search criteria"
                                              Font-Size="8pt" Font-Names="Verdana" ForeColor="#084573" Font-Bold="True" Width="357px"
                                              __designer:wfdid="w51" Visible="False"></asp:Label>
                                              <asp:HiddenField ID="hdndivcode" runat="server" />
                                              <asp:HiddenField ID="hdntrantype" runat="server" />
                                      </ContentTemplate>
                                  </asp:UpdatePanel>
                              </td>
                          </tr>
                  </table>
              </td>
          </tr>
      </table>

  </contenttemplate>
</asp:UpdatePanel>

     <asp:ScriptManagerProxy id="ScriptManagerProxy1" runat="server">
        <Services>
            <asp:ServiceReference Path="../clsSectorServices.asmx" />
              <asp:ServiceReference Path="~/clsServices.asmx" />

        </Services>
    </asp:ScriptManagerProxy>
    </asp:Content>
