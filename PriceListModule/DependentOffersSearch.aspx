<%@ Page Language="VB" MasterPageFile="~/MainPageMaster.master" AutoEventWireup="false" EnableEventValidation="false" CodeFile="DependentOffersSearch.aspx.vb" Inherits="DependentOffersSearch"  %>



<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
    <%@ OutputCache location="none" %> 

        <%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
      <%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
      <%@ Register Assembly="EclipseWebSolutions.DatePicker" Namespace="EclipseWebSolutions.DatePicker"
    TagPrefix="ews" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">


  <meta http-equiv="content-type" content="text/html;charset=UTF-8" />
  <meta http-equiv="X-UA-Compatible" content="chrome=1">

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
   
    <style  type="text/css">
    div.container {
      border:0px;
    }
        .btnExample {
  color: #2D7C8A;
  background: #e7e7e7;
  font-weight: bold;
  border: 1px solid #2D7C8A;
   padding: 5px 5px;
}
 
    #VS code, #VS pre, #VS tt {
      font-family: Monaco, Consolas, "Lucida Console", monospace;
      font-size: 12px;
      line-height: 18px;
      color: #444;
      background: none;
    }
      #VS code {
        margin-left: 8px;
        padding: 0 0 0 12px;
        font-weight: normal;
      }
      #VS pre {
        font-size: 12px;
        padding: 2px 0 2px 0;
        border-left: 6px solid #829C37;
        margin: 12px 0;
      }
    #search_query {
      margin: 18px 0;
      opacity: 0;
    }
      #search_query .raquo {
        font-size: 18px;
        line-height: 12px;
        font-weight: bold;
        margin-right: 4px;
      }
    #search_query2 {
      margin: 18px 0;
      opacity: 0;
    }
      #search_query2 .raquo {
        font-size: 18px;
        line-height: 12px;
        font-weight: bold;
        margin-right: 4px;
      }
  </style>

  <script type="text/javascript" charset="utf-8">

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
          $txtvsprocess.val('Promotionid:" " DependentId:" " PromotionName:" " Hotel:" " Approvedstatus:" " TEXT:" "');
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
                      var hdnparty = document.getElementById("<%=hdnpartycode.ClientID%>");
                      var chkshow = document.getElementById("<%=chkshowall.ClientID%>");
                      switch (category) {
                          case 'PromotionName':

                              var asSqlqry = "select distinct ltrim(rtrim(a.Promotionname)) Promotionname  from view_offer_search(nolock) a ,offers_recalculate o where a.promotionid=o.promotionid  ";
                              glcallback = callback;

                              ColServices.clsServices.GetListOfArrayVisualSearch(asSqlqry, fnFillSearchVS, ErrorHandler, TimeOutHandler);
                              break;
                          case 'Promotionid':
                              var asSqlqry = '';


                              asSqlqry = "select distinct ltrim(rtrim(o.Promotionid)) Promotionid  from view_offer_search(nolock) a ,offers_recalculate o where a.promotionid=o.promotionid  ";


                              glcallback = callback;
                              ColServices.clsServices.GetListOfArrayVisualSearch(asSqlqry, fnFillSearchVS, ErrorHandler, TimeOutHandler);
                              break;
                          case 'DependentId':
                              var asSqlqry = '';


                              asSqlqry = "select distinct ltrim(rtrim(contpromid)) DependentId  from offers_recalculate(nolock)  order by DependentId";


                              glcallback = callback;
                              ColServices.clsServices.GetListOfArrayVisualSearch(asSqlqry, fnFillSearchVS, ErrorHandler, TimeOutHandler);
                              break;
                          case 'Hotel':

                              //  var asSqlqry = 'select ltrim(rtrim(supagentname)) BookingCode  from supplier_agents where active=1 order by supagentname ';
                              var asSqlqry = "select ltrim(rtrim(partyname)) partyname  from partymast(nolock) where active=1  and sptypecode in (select option_selected from reservation_parameters where param_id =458) order by partyname ";
                              glcallback = callback;
                              ColServices.clsServices.GetListOfArrayVisualSearch(asSqlqry, fnFillSearchVS, ErrorHandler, TimeOutHandler);
                              break;
                          case 'Approvedstatus':
                              //                              var vhdDefault_Group = document.getElementById("< %=hdDefault_Group.ClientID%>");
                              //                              var vDefault_Group = vhdDefault_Group.value
                              //                              vDefault_Group = "'" + vDefault_Group + "'";
                              var asSqlqry = "select ltrim(rtrim('All')) status  union all select ltrim(rtrim('Approved')) status  union all select ltrim(rtrim('UnApproved')) status ";
                              glcallback = callback;
                              ColServices.clsServices.GetListOfArrayVisualSearch(asSqlqry, fnFillSearchVS, ErrorHandler, TimeOutHandler);
                              break;
                      }
                  },
                  facetMatches: function (callback) {
                      callback([
                { label: 'TEXT', category: 'Sector' },
                { label: 'PromotionName', category: 'Sector' },
                { label: 'Promotionid', category: 'Sector' },
                { label: 'DependentId', category: 'Sector' },
                { label: 'Hotel', category: 'Sector' },
                { label: 'Approvedstatus', category: 'Sector' },
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


//                        var ModalPopupDays = $find("ModalPopupDays");
//                        if (ModalPopupDays != null) {
//                            ModalPopupDays.hide(500);
//                        }

        }
    </script>



<script type="text/javascript">
    window.moveTo(0, 0);
    window.resizeTo(screen.availWidth, screen.availHeight);

    
</script>


    <script language ="javascript" type ="text/javascript" >


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

        function ShowProgess() {

            var ModalPopupDays = $find("ModalPopupDays");

            ModalPopupDays.show();

            return true;
        }

        function hidenoerror() {

            var ModalPopupError = $find("ModalPopupDays");

            ModalPopupError.hide();
            return true;
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

       


</script>

<script type="text/javascript">
    function RefreshPage() {

        window.location.reload()
    }
</script>




 <asp:UpdatePanel ID="UpdatePanel3" runat="server" >
        <ContentTemplate>
    <table style="width: 100%">
        
        <tr>
            <td>
                <div style="margin-top: -6px; margin-left: 13px;">
                    <table style="border-right: gray 2px solid; border-top: gray 2px solid; border-left: gray 2px solid;
                        border-bottom: gray 2px solid;" class="td_cell" align="left">
                        <tr>
                            <td style="text-align: center; width: 100%;" align="center" class="field_heading">
                                <asp:label id="Label1" runat="server" cssclass="field_heading" text="Dependent Offers Search">
                                </asp:label>
                            </td>
                        </tr>
                        <tr>
                            <td class="td_cell" style="width: 100%; color: blue;" align="center">
                                Type few characters of code or name and click search
                            </td>
                        </tr>
                        <tr style="font-size: 10pt">
                            <td style="text-align: center; width: 100%;" align="center">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:updatepanel id="UpdatePanel11" runat="server">
                                    <contenttemplate>
                                    <TABLE width="100%">
                                    <TBODY>
                                    <TR><TD align=center colSpan="6">
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Button id="btnhelp" 
                                    tabIndex=3 onclick="btnhelp_Click" runat="server" Text="Help" 
                                    Font-Bold="False" CssClass="search_button"></asp:Button> &nbsp;&nbsp;<asp:Button id="btnAddNew" tabIndex=4 runat="server" Text="Add New"  onclick="btnAddNew_Click"
                                    Font-Bold="False" CssClass="btn"></asp:Button>&nbsp;&nbsp;
                                    <asp:Button id="btnPrint" tabIndex=5 runat="server" Text="Report" 
                                    CssClass="btn"></asp:Button>
                                    <asp:Button id="btnAdd" tabIndex=5 runat="server" Text="Add"  Style="display:none"
                                    CssClass="btn"></asp:Button>
                                    <asp:LinkButton id="linkadd" 
                                   Text="Add New" 
                                   Font-Names="Verdana" 
                                   Font-Size="8pt" Style="display:none"
                                   
                                   runat="server"/>
                                    </TD>
                                    </TR>
                                    <tr>
                                    <td style="WIDTH: 80px">
                                    &nbsp;</td>
                                    <td style="WIDTH: 100px">
                                    &nbsp;</td>
                                    <td style="WIDTH: 79px">
                                    &nbsp;</td>
                                    <td style="WIDTH: 100px">
                                    &nbsp;</td>
                                    <td style="WIDTH: 178px">
                                    &nbsp;</td>
                                    <td style="WIDTH: 178px">
                                    &nbsp;</td>
                                    </tr>
                     <tr>
                            <td colspan="6">
                             <div style="width:100%" >
                            <div style="width:80%;display: inline-block; margin: -6px 4px 0 0;">
                                <div ID="VS" class="container" style="border:0px;">
                                    <div ID="search_box_container">
                                    </div>                        
                                    <asp:TextBox ID="txtvsprocess" runat="server" class="cs_txtvsprocess" 
                                        style="display:none"></asp:TextBox>
                                    <asp:TextBox ID="txtvsprocesssplit" runat="server" class="cs_txtvsprocesssplit" 
                                        style="display:none"></asp:TextBox>
                                    <asp:Button ID="btnvsprocess" runat="server" style="display:none" />
                                </div> </div> 
                                 <div style=" width:18%; display: inline-block;vertical-align:top;">
                                     <asp:Button ID="btnResetSelection" runat="server" 
                        CssClass="btn" Font-Bold="False" 
                                                tabIndex="4" Text="Reset Search" /></div></div>
                                <asp:DataList ID="dlList" runat="server" RepeatColumns="4" 
                                    RepeatDirection="Horizontal">
                                    <ItemTemplate>
                                        <table class="styleDatalist" style="border:0px;">
                                            <tr style="">
                                                <td style="border:0px;">
                                                    <asp:Button ID="lnkCode" runat="server" class="button button4" 
                                                        style="display:none" text='<%# Eval("Code") %>' />
                                                    <asp:Button ID="lnkValue" runat="server" class="button button4" 
                                                        style="display:none" text='<%# Eval("Value") %>' />
                                                    <asp:Button ID="lnkCodeAndValue" runat="server" class="button button4" 
                                                        Font-Bold="False" Font-Size="Small" ForeColor="#000099" 
                                                        OnClientClick="return false;" text='<%# Eval("CodeAndValue") %>' />
                                                    <asp:Button ID="lbClose" runat="server" class="buttonClose button4" 
                                                        onclick="lbClose_Click" Text="X" />
                                                </td>
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
                                    &nbsp;</td>
                                <td>
                                    &nbsp;</td>
                                <td>
                                    &nbsp;</td>
                                <td>
                                    &nbsp;</td>
                                <td>
                                    &nbsp;</td>
                                <td>
                                    &nbsp;</td>
                                <td>
                                    &nbsp;</td>
                                <td>
                                    &nbsp;</td>
                                <td>
                                    &nbsp;</td>
                            </tr>
                              <tr>
                                  <td>
                                      &nbsp;
                                  </td>
                                  <td>
                                      &nbsp;
                                  </td>
                                  <td class="td_cell">
                                      <asp:Label ID="Label6" runat="server" CssClass="field_caption" Text="Filter By "></asp:Label>
                                  </td>
                                  <td class="td_cell">
                                      <asp:DropDownList ID="ddlOrder" runat="server">
                                          <asp:ListItem Value="C">Created Date</asp:ListItem>
                                          <asp:ListItem Value="M">Modified Date</asp:ListItem>
                                          <asp:ListItem Value="P">Promotions Date</asp:ListItem>
                                          <%--  <asp:ListItem Value="B">Booking Validity Dates</asp:ListItem>--%>
                                      </asp:DropDownList>
                                  </td>
                                  <td class="td_cell">
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
                                          InvalidValueMessage="Invalid Date" TooltipMessage="Input a Date in Date/Month/Year">
                                      </cc1:MaskedEditValidator>
                                  </td>
                                  <td class="td_cell">
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
                                          InvalidValueMessage="Invalid Date" TooltipMessage="Input a Date in Date/Month/Year">
                                      </cc1:MaskedEditValidator>
                                  </td>
                                  <td>
                                      <asp:Button ID="btnFilter" runat="server" CssClass="btn" Font-Bold="False" TabIndex="4"
                                          Text="Search by Date" />
                                      &nbsp;<asp:Button ID="btnClearDate" runat="server" CssClass="btn" Font-Bold="False"
                                          TabIndex="4" Text="Reset Dates" />
                                  </td>
                                  <td class="td_cell">
                                      <asp:Label ID="RowSelectss" runat="server" CssClass="field_caption" Text="Rows Selected "></asp:Label>
                                      <asp:DropDownList ID="RowsPerPageSS" runat="server" AutoPostBack="true">
                                          <asp:ListItem Value="[Select]">Select</asp:ListItem>
                                          <asp:ListItem Value="5">5</asp:ListItem>
                                          <asp:ListItem Value="10">10</asp:ListItem>
                                          <asp:ListItem Value="15">15</asp:ListItem>
                                          <asp:ListItem Value="20">20</asp:ListItem>
                                          <asp:ListItem Value="25">25</asp:ListItem>
                                          <asp:ListItem Value="30">30</asp:ListItem>
                                      </asp:DropDownList>
                                  </td>
                                  <td>
                                  <asp:CheckBox ID="chkshowall" runat="server" Visible="false"  AutoPostBack="true"  Text ="Show All Promotions" OnCheckedChanged ="chkshowall_CheckedChanged" />
                                  </td>

                              </tr>
                
                    <tr>
                                <td>
                                    &nbsp;</td>
                                <td>
                                    &nbsp;</td>
                                <td>
                                    &nbsp;</td>
                                <td>
                                    &nbsp;</td>
                                <td>
                                    &nbsp;</td>
                                <td>
                                    &nbsp;</td>
                                <td>
                                    &nbsp;</td>
                                <td>
                                    &nbsp;</td>
                                <td>
                                    &nbsp;</td>
                            </tr>
                </table>                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                  
            </td>
            </tr>

                      
   

            </TBODY> 
            </table>
            </contenttemplate>
            </asp:updatepanel>
            </td>
            </tr >
                        <tr style="font-size: 10pt;">
                            <td align="center" style="text-align: left; width: 100%;">
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                    <ContentTemplate>
                                        <table style="width: 1181px; text-align: left" class="td_cell">
                                            <tbody>
                                                <tr>
                                                    <td colspan="4">
                                                        <table style="width: 1183px; display: none;" class="td_cell">
                                                            <tbody>
                                                                <tr>
                                                                    <td style="width: 105px">
                                                                        Contract Id
                                                                    </td>
                                                                    <td style="width: 260px">
                                                                        <asp:TextBox ID="TxtContractId" runat="server" CssClass="txtbox" Width="216px"></asp:TextBox>
                                                                    </td>
                                                                    <td style="width: 115px">
                                                                        Order by
                                                                    </td>
                                                                    <td>
                                                                        <asp:DropDownList ID="ddlOrderBy" runat="server" CssClass="drpdown" AutoPostBack="True"
                                                                            Width="128px">
                                                                            <asp:ListItem Value="0">Contract Id Desc</asp:ListItem>
                                                                            <asp:ListItem Value="1">Contract Id Asc</asp:ListItem>
                                                                            <asp:ListItem Value="2">Supplier Name</asp:ListItem>
                                                                            <asp:ListItem Value="3">Markets</asp:ListItem>
                                                                            <asp:ListItem Value="4">Country Specific</asp:ListItem>
                                                                            <asp:ListItem Value="5">Agent Specific</asp:ListItem>
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                    <td>
                                                                        &nbsp;
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="width: 105px">
                                                                        Supplier Name
                                                                    </td>
                                                                    <td style="width: 260px">
                                                                        <asp:TextBox ID="txtSupName" runat="server" CssClass="txtbox" Width="216px"></asp:TextBox>
                                                                    </td>
                                                                    <td style="width: 115px">
                                                                        Country Group
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtCountryGroup" runat="server" CssClass="txtbox" Width="216px"></asp:TextBox>
                                                                    </td>
                                                                    <td>
                                                                        &nbsp;
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="width: 105px">
                                                                        Country Name
                                                                    </td>
                                                                    <td style="width: 220px">
                                                                        <asp:TextBox ID="txtCoutryName" runat="server" CssClass="txtbox" Width="216px"></asp:TextBox>
                                                                    </td>
                                                                    <td style="width: 115px">
                                                                        Agents
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtAgent" runat="server" CssClass="txtbox" Width="216px"></asp:TextBox>
                                                                        <asp:Button ID="btnClear" runat="server" CssClass="search_button" Font-Bold="False"
                                                                            Text="Clear" />
                                                                    </td>
                                                                    <td>
                                                                        &nbsp;
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="width: 105px">
                                                                        Approved /Unapproved
                                                                    </td>
                                                                    <td style="width: 260px">
                                                                        <asp:TextBox ID="txtApproved" runat="server" CssClass="txtbox" Width="216px"></asp:TextBox>
                                                                    </td>
                                                                    <td style="width: 115px">
                                                                        &nbsp;
                                                                    </td>
                                                                    <td>
                                                                        <asp:RadioButton ID="rbtnsearch" runat="server" AutoPostBack="True" Checked="True"
                                                                            CssClass="td_cell" ForeColor="Black" GroupName="GrSearch" OnCheckedChanged="rbtnsearch_CheckedChanged"
                                                                            Text="Search" />
                                                                        <asp:RadioButton ID="rbtnadsearch" runat="server" AutoPostBack="True" CssClass="td_cell"
                                                                            ForeColor="Black" GroupName="GrSearch" OnCheckedChanged="rbtnadsearch_CheckedChanged"
                                                                            Text="Advance Search" />
                                                                        <asp:Button ID="btnSearch" runat="server" CssClass="search_button" Font-Bold="False"
                                                                            Text="Search" />
                                                                    </td>
                                                                    <td>
                                                                        &nbsp;
                                                                    </td>
                                                                </tr>
                                                            </tbody>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="4">
                                                        <asp:Panel ID="pnlHeader" runat="server" Visible="False">
                                                            <table style="width: 768px">
                                                                <tbody>
                                                                    <tr>
                                                                        <td style="width: 94px" class="td_cell">
                                                                            Supplier&nbsp;Type&nbsp;Code
                                                                        </td>
                                                                        <td style="width: 259px">
                                                                            <select style="width: 223px" id="ddlSPTypeCD" class="drpdown" runat="server">
                                                                                <option selected></option>
                                                                            </select>
                                                                        </td>
                                                                        <td style="width: 116px" class="td_cell">
                                                                            Supplier&nbsp;Type&nbsp;Name
                                                                        </td>
                                                                        <td>
                                                                            <select style="width: 217px" id="ddlSPTypeNM" class="drpdown" onchange="CallWebMethod('sptypename');"
                                                                                runat="server">
                                                                                <option selected></option>
                                                                            </select>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="width: 94px" class="td_cell">
                                                                            Supplier&nbsp;Agent&nbsp;Code
                                                                        </td>
                                                                        <td style="width: 259px">
                                                                            <select style="width: 223px" id="ddlSupplierAgent" class="drpdown" onchange="CallWebMethod('supagentcode');"
                                                                                runat="server">
                                                                                <option selected></option>
                                                                            </select>
                                                                        </td>
                                                                        <td style="width: 116px" class="td_cell">
                                                                            Supplier&nbsp;Agent&nbsp;Name
                                                                        </td>
                                                                        <td>
                                                                            <select style="width: 217px" id="ddlSuppierAgentNM" class="drpdown" onchange="CallWebMethod('supagentname');"
                                                                                runat="server">
                                                                                <option selected></option>
                                                                            </select>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="width: 94px" class="td_cell">
                                                                            Currency&nbsp;Code
                                                                        </td>
                                                                        <td style="width: 259px">
                                                                            <select onchange="GetValueFromCurrency()" style="width: 223px" id="ddlCurrencyCD"
                                                                                class="drpdown" runat="server">
                                                                                <option selected></option>
                                                                            </select>
                                                                        </td>
                                                                        <td style="width: 116px" class="td_cell">
                                                                            Currency&nbsp;Name
                                                                        </td>
                                                                        <td>
                                                                            <select onchange="GetValueCodeCurrency()" style="width: 217px" id="ddlCurrencyNM"
                                                                                class="drpdown" runat="server">
                                                                                <option selected></option>
                                                                            </select>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="width: 94px" class="td_cell">
                                                                            Sub&nbsp;Season&nbsp;Code
                                                                        </td>
                                                                        <td style="width: 259px">
                                                                            <select onchange="GetValueFromSubSeason()" style="width: 222px" id="ddlSubSeas" class="drpdown"
                                                                                runat="server">
                                                                                <option selected></option>
                                                                            </select>
                                                                        </td>
                                                                        <td style="width: 116px" class="td_cell">
                                                                            Sub&nbsp;Season&nbsp;Name
                                                                        </td>
                                                                        <td>
                                                                            <select onchange="GetValueCodeSubSeason()" style="width: 217px" id="ddlSubSeasNM"
                                                                                class="drpdown" runat="server">
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
                                        &nbsp; &nbsp;&nbsp;
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" class="td_cell">
                                <table style="width: 1060px">
                                    <tr>
                                        <td colspan="2" class="td_cell" align="left">
                                            &nbsp;
                                            <asp:Button ID="btnExportToExcel" runat="server" CssClass="btn" Text="Export To Excel" />&nbsp;
                                            <input id="txtconnection" runat="server" style="visibility: hidden; width: 12px;
                                                height: 9px" type="text" />
                                            <asp:HiddenField ID="hdDefault_Group" runat="server" />
                                            <asp:HiddenField ID="hdDefaultValue" runat="server" />
                                            <asp:HiddenField ID="hdDefaultValueText" runat="server" />
                                             <asp:HiddenField ID="hdnpromoid" runat="server" />
                                            <asp:HiddenField ID="hdnpartycode" runat="server" />
                                            <asp:Button ID="btnchecking" runat="server" Text="Start Checking" Font-Bold="False" style="display:none" CssClass="btn"   />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 100%;">
                                <asp:updatepanel id="UpdatePanel2" runat="server">
                                    <contenttemplate>
                                            <asp:GridView id="gv_SearchResult" runat="server" Font-Size="10px" CssClass="grdstyle" Width="100%" 
                                        GridLines="Vertical" CellPadding="3" BorderWidth="1px" BackColor="White" 
                                        AutoGenerateColumns="False" AllowSorting="True" AllowPaging="True" 
                                        BorderStyle="Solid">
                                            <FooterStyle CssClass="grdfooter"></FooterStyle>
                                                        <Columns>

                                                        <asp:BoundField DataField="DependentId" HeaderText="DependentId">
                                                        </asp:BoundField>
                                                       

                                                        <asp:TemplateField  HeaderText="PromotionID" >
                                                         <EditItemTemplate>
                                                                <asp:TextBox ID="txtpromotionid" runat="server" Text='<%# Bind("promotionid") %>'></asp:TextBox>                             
                                                        </EditItemTemplate>
                                                        <ItemTemplate>
                                                        <asp:Label id="lblplistcode" runat="server" Text='<%# Bind("promotionid") %>'></asp:Label> 
                                                        </ItemTemplate>
                                                        </asp:TemplateField >

                                                        <asp:TemplateField HeaderText="partycode" visible="false" >
                                                        <EditItemTemplate>
                                                            <asp:TextBox ID="txtpartycode" runat="server" Text='<%# Bind("partycode") %>'></asp:TextBox>
                                                        </EditItemTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblparty" runat="server" Text='<%# Bind("partycode") %>'></asp:Label>
                                                            <asp:Label ID="lblpartyname" runat="server" Text='<%# Bind("partyname") %>'></asp:Label>
                                                            <asp:Label ID="lblactive" runat="server" Text='<%# Bind("activestate") %>'></asp:Label>
                                                            <asp:Label id="lblDependentId" runat="server" Text='<%# Bind("DependentId") %>'></asp:Label> 
                                                        </ItemTemplate>
       
                                                        </asp:TemplateField>


                                                        <asp:TemplateField HeaderText="agentcode" visible="false"  >
                                                      
                                                        <ItemTemplate>
                                                          
                                                            <asp:Label ID="lblstatus" runat="server" Text='<%# Bind("status") %>'></asp:Label>
                                                        </ItemTemplate>
                                                         </asp:TemplateField>
                                                       
                                                        <asp:TemplateField HeaderText="Hotel Name" >
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblpartyname1" runat="server" Text='<%# Bind("partyname") %>' ></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                     
                                                         <asp:TemplateField HeaderText="Promotion Name" >
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblpromotionname" runat="server" Text='<%# Bind("promotionname") %>' ></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                       

                                                         <asp:TemplateField HeaderText="Applicable To">
                                                             <ItemTemplate>
                                                           
                                                            <asp:Label ID="lblapplicable" runat="server"    Text='<%# Limit(Eval("applicableto"), 25)%>' Tooltip='<%# Eval("applicableto")%>'></asp:Label>
                                                            <br />
                                                             <asp:LinkButton ID="ReadMoreLinkButton" runat="server" Text="More" Visible='<%# SetVisibility(Eval("applicableto"), 25) %>'   OnClick="ReadMoreLinkButton_Click"></asp:LinkButton>

                                                            </ItemTemplate>
                                                         
                                                        </asp:TemplateField>
                                                    

                                                        <asp:BoundField HtmlEncode="False" DataFormatString="{0:dd/MM/yyyy}" DataField="fromdate" SortExpression="fromdate" HeaderText="Min From Date"></asp:BoundField>
                                                        <asp:BoundField HtmlEncode="False" DataFormatString="{0:dd/MM/yyyy}" DataField="todate" SortExpression="todate" HeaderText="Max To Date"></asp:BoundField>

                                                        
                                                      
                                                        <asp:BoundField DataField="status" HeaderText="Approved Status">

                                                        </asp:BoundField>
                                                          <asp:BoundField DataField="activestate" HeaderText="Status">
                                                       
                                                        </asp:BoundField>

                                                        
                                                        <asp:ButtonField HeaderText="Action" Text="View" CommandName="View">
                                                        <ItemStyle ForeColor="Blue"></ItemStyle>
                                                        </asp:ButtonField>
                                                        <asp:TemplateField HeaderText="Action">
                                                        <ItemTemplate>
                                                        <asp:LinkButton ID="lnkapprove" runat="server"  Text="RateCalculation"  OnClientClick="ShowProgess();" OnClick="Rate_Click"></asp:LinkButton>
                                                           </ItemTemplate>
                                                         
                                                        </asp:TemplateField>

                                                         <asp:TemplateField HeaderText="Action">
                                                        <ItemTemplate>
                                                        <asp:LinkButton ID="RateApprove" runat="server"  Text="Approve"  OnClientClick="ShowProgess();" OnClick="Approve_Click"></asp:LinkButton>
                                                           </ItemTemplate>
                                                         
                                                        </asp:TemplateField>



                                                         <asp:TemplateField HeaderText="Action" Visible =false >
                                                        <ItemTemplate>
                                                        <asp:LinkButton ID="DependApprove" runat="server" Text="DependentApprove"  OnClientClick="ShowProgess();" OnClick="DependApprove_Click"></asp:LinkButton>
                                                           </ItemTemplate>
                                                         
                                                        </asp:TemplateField>
                                                      

                                                        <asp:ButtonField HeaderText="Action" Text="Approve" CommandName="Approve" Visible=false >
                                                        <ItemStyle ForeColor="Blue"></ItemStyle>
                                                        </asp:ButtonField>
                                                  

                                                    <asp:BoundField HtmlEncode="False" DataFormatString="{00:dd/MM/yyyy hh:mm:ss tt}" DataField="addate"  HeaderText="Date Created">
                                                   

                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="adduser"  HeaderText="User Created">
                                                   

                                             
                                                    </asp:BoundField>
                                                
                                                    <asp:BoundField DataField="approveddate" HeaderText="Approved Date">
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="approvedby" HeaderText="Approved User">
                                                    </asp:BoundField>
                                                 </Columns>

                                                <RowStyle CssClass="grdRowstyle"></RowStyle>
                                                <SelectedRowStyle CssClass="grdselectrowstyle"></SelectedRowStyle>
                                                <PagerStyle CssClass="grdpagerstyle" HorizontalAlign="Center"></PagerStyle>
                                                <HeaderStyle CssClass="grdheader" ForeColor="White"></HeaderStyle>
                                                <AlternatingRowStyle CssClass="grdAternaterow"></AlternatingRowStyle>
                                                </asp:GridView> 
                                                <asp:Label id="lblMsg" runat="server" Text="Records not found. Please redefine search criteria" 
                                                Font-Size="8pt" Font-Names="Verdana" Font-Bold="True" Width="357px" 
                                                Visible="False" CssClass="lblmsg"></asp:Label> 
                                        </contenttemplate>
                                </asp:updatepanel>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                        <td>
                         <asp:updatepanel id="UpdatePanel5" runat="server">
                           <contenttemplate>
                           <asp:GridView id="grddependent" runat="server" Font-Size="10px" CssClass="grdstyle" Width="100%" 
                                        GridLines="Vertical" CellPadding="3" BorderWidth="1px" BackColor="White" 
                                        AutoGenerateColumns="False" AllowSorting="True" AllowPaging="True" 
                                        BorderStyle="Solid">
                                         <FooterStyle CssClass="grdfooter"></FooterStyle>
                                          <Columns>
                                           <asp:BoundField DataField="DependentId" HeaderText="DependentId">
                                             </asp:BoundField>
                                                       

                                            <asp:TemplateField  HeaderText="PromotionID" Visible=false >
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="txtpromotionid" runat="server" Text='<%# Bind("DependentId") %>'></asp:TextBox>                             
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblparty" runat="server" Text='<%# Bind("partycode") %>'></asp:Label>
                                             <asp:Label id="lblDependentId" runat="server" Text='<%# Bind("DependentId") %>'></asp:Label> 
                                              <asp:Label ID="lblactive" runat="server" Text='<%# Bind("activestate") %>'></asp:Label>
                                            </ItemTemplate>
                                            </asp:TemplateField >
                                              <asp:TemplateField HeaderText="Hotel Name" >
                                            <ItemTemplate>
                                                <asp:Label ID="lblpartyname1" runat="server" Text='<%# Bind("partyname") %>' ></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                                     
                                                <asp:TemplateField HeaderText="Promotion Name/Applicable To" >
                                            <ItemTemplate>
                                                <asp:Label ID="lblpromotionname" runat="server" Text='<%# Bind("promotionname") %>' ></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                             <asp:BoundField DataField="status" HeaderText="Approved Status">
                                             </asp:BoundField>
                                              <asp:BoundField DataField="activestate" HeaderText="Status">
                                                       
                                               </asp:BoundField>
                                           
                                              <asp:ButtonField HeaderText="Action" Text="View" CommandName="View" Visible="false">
                                                <ItemStyle ForeColor="Blue"></ItemStyle>
                                                </asp:ButtonField>
                                                
                                                <asp:TemplateField HeaderText="Action">
                                                <ItemTemplate>
                                                <asp:LinkButton ID="lnkdepapprove" runat="server"  Text="RateCalculation"  OnClientClick="ShowProgess();" OnClick="DepRate_Click"></asp:LinkButton>
                                                    </ItemTemplate>
                                                         
                                                </asp:TemplateField>

                                                
                                                <asp:TemplateField HeaderText="Action">
                                                <ItemTemplate>
                                                <asp:LinkButton ID="DependApprove" runat="server"  Text="Approve"  OnClientClick="ShowProgess();" OnClick="DependApprove_Click"></asp:LinkButton>
                                                    </ItemTemplate>
                                                         
                                                </asp:TemplateField>
                                                      <asp:BoundField HtmlEncode="False" DataFormatString="{00:dd/MM/yyyy hh:mm:ss tt}" DataField="addate"  HeaderText="Date Created">
                                                   

                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="adduser"  HeaderText="User Created">
                                                   

                                             
                                                    </asp:BoundField>
                                                
                                                    <asp:BoundField DataField="approveddate" HeaderText="Approved Date">
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="approvedby" HeaderText="Approved User">
                                                    </asp:BoundField>

                                          </Columns> 
                                           <RowStyle CssClass="grdRowstyle"></RowStyle>
                                                <SelectedRowStyle CssClass="grdselectrowstyle"></SelectedRowStyle>
                                                <PagerStyle CssClass="grdpagerstyle" HorizontalAlign="Center"></PagerStyle>
                                                <HeaderStyle CssClass="grdheader" ForeColor="White"></HeaderStyle>
                                                <AlternatingRowStyle CssClass="grdAternaterow"></AlternatingRowStyle>

                                        </asp:GridView> 


                           </contenttemplate>
                        </asp:updatepanel>
                        </td>
                        
                        </tr>
                    </table>
                </div>
            </td>
        </tr>
    </table>
    <center>
        <div id="Loading1" runat="server" style="height: 150px; width: 500px;">
           <asp:UpdatePanel ID="UpdatePanel4" runat="server">
           <ContentTemplate>
            <img alt="" id="Image1" runat="server" src="~/Images/loader-progressbar.gif" width="200" />
            <h2 style="color: #06788B">
                Processing please wait...</h2>

        </ContentTemplate>
         </asp:UpdatePanel>
        </div>
       
    </center>
     <asp:ModalPopupExtender ID="ModalPopupDays" runat="server" BehaviorID="ModalPopupDays"
        TargetControlID="btnInvisibleGuest" CancelControlID="btnClose" PopupControlID="Loading1"
        BackgroundCssClass="ModalPopupBG">
    </asp:ModalPopupExtender>
                
    
    <input id="btnInvisibleGuest" runat="server" type="button" value="Cancel" style="display: none" />
    <input id="btnClose" type="button" value="Cancel" style="display: none" />

     </ContentTemplate>
    </asp:UpdatePanel>   


 
  <asp:Panel id="Panel4" runat="server"  Width="300px" 
            GroupingText=""  Font-Size="14px" Font-Bold=true BorderColor="#000">

<table >
 
<tr style="display:none">
    <td><asp:HyperLink id="hypmaxoccu" Font-Size=10  runat ="server"  CssClass="field_caption" text ="Maximum Occupancy"  NavigateUrl="maxaccomodationsearch.aspx"/> </td>
   <td width="25px"> </td>
    <td><asp:HyperLink id="hyphtl" Font-Size=10  runat ="server"  CssClass="field_caption" text ="Hotel Construction"  NavigateUrl="hotelsconstructionsearch.aspx"/> </td>
    <tr><td> </td></tr>
</tr>
<tr style="display:none">
   <td>
   
   <asp:HyperLink id="hypgenplcy" Font-Size="10"  runat ="server"  CssClass="field_caption" text ="General Policy" NavigateUrl="generalpolicysearch.aspx"  /> 
   
   </td>
  <td width="25px"> </td>
    <td><asp:HyperLink id="hypcanplcy" Font-Size=10  runat ="server"  CssClass="field_caption" text ="Cancellation Policy"  NavigateUrl="CancellationpolicySearch.aspx"/> </td>
     <tr><td> </td></tr>
</tr>

<tr style="display:none">
   <td><asp:HyperLink id="hyppromo" Font-Size=10  runat ="server"  CssClass="field_caption" text ="Promotion"  NavigateUrl="promotionsearch.aspx?AutoNo"/> </td>
  <td width="25px"> </td>
   <td><asp:HyperLink id="hypblksales" Font-Size=10  runat ="server"  CssClass="field_caption" text ="Blockfull Sales"  NavigateUrl="blockfullsalessearch.aspx"/> </td>
    <tr><td> </td></tr>
</tr>

<tr style="display:none">
  <td><asp:HyperLink id="hypcomprmks" Font-Size=10  runat ="server"  CssClass="field_caption" text ="Compulosry Remarks"  NavigateUrl="compulsoryremarkssearch.aspx"/> </td>
   <td width="25px"> </td>
   <td><asp:HyperLink id="hypminnights" Font-Size=10  runat ="server"  CssClass="field_caption" text ="Minimum Nights"  NavigateUrl="minimumnightssearch.aspx"/> </td>
   <tr><td> </td></tr>
</tr>
    
<tr style="display:none">
   <td><asp:HyperLink id="hypspvntprice" Font-Size=10  runat ="server"  CssClass="field_caption" text ="Special Event Pricelist"  NavigateUrl="specialeventpricelistpage.aspx"/> </td>
  <td width="25px"> </td>
 

</tr>


</table>

<table>
<tr style="display:none"> 
<td width ="225px"> </td>
<td>&nbsp<asp:Button ID ="btnexit" Text ="Exit" runat ="server" CssClass ="btn"></asp:button>

</td>
</tr>
</table>
            
      </asp:Panel>         

     
               
    <asp:ScriptManagerProxy id="ScriptManagerProxy1" runat="server">
        <services>
<asp:ServiceReference Path="~/clsServices.asmx"></asp:ServiceReference>
</services>
    </asp:ScriptManagerProxy>
</asp:Content>

