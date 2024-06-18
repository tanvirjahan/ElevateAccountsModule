<%@ Page Language="VB" MasterPageFile="~/MainPageMaster.master" AutoEventWireup="false"
     Strict="true" CodeFile="CustomerSectorSearch.aspx.vb" Inherits="CustomerSectorSearch" EnableEventValidation="false" %>

    


     <%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
 <%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>


<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ OutputCache Location="none" %>


<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="server">

<link rel="stylesheet" href="../Content/lib/css/reset.css" type="text/css" media="screen" charset="utf-8">
  <link rel="stylesheet" href="../Content/lib/css/icons.css" type="text/css" media="screen" charset="utf-8">
  <link rel="stylesheet" href="../Content/lib/css/workspace.css" type="text/css" media="screen" charset="utf-8">
    
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
          .styleDatalist
    {
        width: 100%;
    }
    
    div.container {
      border:0px;
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
      margin: 8px 0;
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
        .style1
        {
            width: 100%;
        }
  </style>
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
          $txtvsprocess.val('Sector:" "Country:" " Text:" "');
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


                          case 'Sector':
                              var asSqlqry = '';
                              glcallback = callback;
                           
                              ColServices.clsSectorServices.GetListOfSectorsVisualSearch(asSqlqry, fnFillSearchVS, ErrorHandler, TimeOutHandler);
                              break;

                          case 'Country':
                              var asSqlqry = '';
                              glcallback = callback;
                              ColServices.clsSectorServices.GetListOfArrayCountryVisualSearch(asSqlqry, fnFillSearchVS, ErrorHandler, TimeOutHandler);
                              // 
                              //    ColServices.clsSectorServices.GetListOfAgentCatNameVisualSearch(asSqlqry, fnFillSearchVS, ErrorHandler, TimeOutHandler);
                              break;


                      }
                  },
                  facetMatches: function (callback) {
                      callback([
                         { label: 'Text', category: 'Name' },
                { label: 'Country', category: 'Name' },
                { label: 'Sector', category: 'Name' },


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

       
    <table style="border-right: gray 2px solid; border-top: gray 2px solid; border-left: gray 2px solid;
        border-bottom: gray 2px solid;">
        <tr>
            <td colspan="1" style="text-align: center;" class="field_heading">
                Customer Sector
            </td>
        </tr>
        <tr>
            <td colspan="1" style="color: blue;" align="center" class="td_cell">
                Type few characters of code or name and click search
            </td>
        </tr>
        <tr>
            <td>
                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                    <ContentTemplate>
                        <table>
                            <tbody>
                                <tr>
                                    <td style="text-align: center" class="td_cell" colspan="7">
                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:RadioButton ID="rbtnsearch" runat="server" Text="Search" ForeColor="Black" CssClass="td_cell"
                                            AutoPostBack="True" GroupName="GrSearch" style="display:none" Checked="True"></asp:RadioButton>&nbsp;<asp:RadioButton
                                                ID="rbtnadsearch" runat="server" Text="Advance Search" ForeColor="Black" CssClass="td_cell"
                                                AutoPostBack="True" GroupName="GrSearch" OnCheckedChanged="rbtnadsearch_CheckedChanged" style="display:none">
                                            </asp:RadioButton>&nbsp;&nbsp;&nbsp;<asp:Button ID="btnSearch" TabIndex="3" runat="server"
                                                Text="Search" Font-Bold="False" CssClass="search_button" style="display:none"></asp:Button>&nbsp;<asp:Button
                                                    ID="btnClear" TabIndex="4" runat="server" Text="Clear" Font-Bold="False" CssClass="search_button" style="display:none">
                                                </asp:Button>&nbsp;<asp:Button ID="btnHelp" TabIndex="8" OnClick="btnHelp_Click"
                                                    runat="server" Text="Help" CssClass="search_button"></asp:Button>&nbsp;<asp:Button
                                                        ID="btnAddNew" TabIndex="5" runat="server" Text="Add New" Font-Bold="False" CssClass="btn">
                                                    </asp:Button>&nbsp;<asp:Button ID="btnPrint" TabIndex="7" runat="server" Text="Report"
                                                        CssClass="btn"></asp:Button>
                                    </td>
                                </tr>
                                
                                <tr>
                                    <td class="td_cell" colspan="7">
                                        <table>
                                            <tbody>
                                                <tr style="display:none">
                                                    <td>
                                                        <asp:Label ID="Label1" runat="server" Text="Sector Code" Height="14px" Width="107px"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="TxtSectorCode" TabIndex="1" runat="server" Width="183px" CssClass="field_input"
                                                            MaxLength="20"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblsectorname" runat="server" Text="Sector Name" Width="102px"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="TxtSecorName" TabIndex="2" runat="server" Width="173px" CssClass="field_input"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="Label3" runat="server" Text="Order By" Width="50px" CssClass="field_caption"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlOrderBy" runat="server" Width="104px" CssClass="drpdown"
                                                            AutoPostBack="True" OnSelectedIndexChanged="ddlOrderBy_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblmarketcode" runat="server" Text="Market Code" Width="114px" Visible="False"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <select style="width: 188px" id="ddlmarketcd" class="drpdown" tabindex="6" onchange="CallWebMethod('mktcode');"
                                                            runat="server" visible="false">
                                                            <option selected></option>
                                                        </select>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblmarketname" runat="server" Text="Market Name" Width="97px" Visible="False"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <select style="width: 178px" id="ddlmarketnm" class="drpdown" tabindex="6" onchange="CallWebMethod('mktname');"
                                                            runat="server" visible="false">
                                                            <option selected></option>
                                                        </select>
                                                    </td>
                                                    <td>
                                                    </td>
                                                    <td>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblctrycode" runat="server" Text="Country Code" Width="110px" Visible="False"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <select style="width: 188px" id="ddlcountrycd" class="drpdown" tabindex="6" onchange="CallWebMethod('ctrycode');"
                                                            runat="server" visible="false">
                                                            <option selected></option>
                                                        </select>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblCtryname" runat="server" Text="Country name" Width="98px" Visible="False"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <select style="width: 178px" id="ddlcountrynm" class="drpdown" tabindex="6" onchange="CallWebMethod('ctryname');"
                                                            runat="server" visible="false">
                                                            <option selected></option>
                                                        </select>
                                                    </td>
                                                    <td>
                                                    </td>
                                                    <td>
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </td>
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
    CssClass="btn" Font-Bold="False" tabIndex="4" Text="Reset Search" /></div></div>



            <asp:DataList ID="dlList" runat="server" RepeatColumns="4" 
                RepeatDirection="Horizontal">


                <ItemTemplate>
                    <table class="styleDatalist" style="border:0px;">
                        <tr style="">
                            <td style="border:0px;">
                                <asp:Button ID="lnkCode" runat="server" class="button button4" 
                                  style="display:none"   text='<%# Eval("Code") %>' />
                                <asp:Button ID="lnkValue" runat="server" class="button button4" 
                                 style="display:none"   text='<%# Eval("Value") %>' />
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
                        &nbsp;</td>
                    <td>
                        &nbsp;</td>
                    <td>
                        <asp:Label ID="Label6" runat="server" CssClass="field_caption" 
                            Text="Filter By "></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlOrder" runat="server" TabIndex="5">
                            <asp:ListItem Value="C">Created Date</asp:ListItem>
                            <asp:ListItem Value="M">Modified Date</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="Label4" runat="server" CssClass="field_caption" Text="From Date"></asp:Label>


                    </td>
                    <td>
                        <asp:TextBox ID="txtFromDate" runat="server" onchange="filltodate(this);" 
                            Width="75px" TabIndex="6"></asp:TextBox>
                        <asp:ImageButton ID="ImgBtnFrmDt" runat="server" 
                            ImageUrl="~/Images/Calendar_scheduleHS.png" TabIndex="3" />
                        <cc1:CalendarExtender ID="dpFromDate" runat="server" Format="dd/MM/yyyy" 
                            OnClientDateSelectionChanged="DateSelectCalExt" PopupButtonID="ImgBtnFrmDt" 
                            PopupPosition="Right" TargetControlID="txtFromDate">
                        </cc1:CalendarExtender>
                        <cc1:MaskedEditExtender ID="MeFromDate" runat="server" Mask="99/99/9999" 
                            MaskType="Date" TargetControlID="txtFromDate">
                        </cc1:MaskedEditExtender>
                        <cc1:MaskedEditValidator ID="MevFromDate" runat="server" 
                            ControlExtender="MeFromDate" ControlToValidate="txtFromDate" 
                            CssClass="field_error" Display="Dynamic" 
                            EmptyValueBlurredText="Date is required" EmptyValueMessage="Date is required" 
                            ErrorMessage="MeFromDate" InvalidValueBlurredMessage="Invalid Date" 
                            InvalidValueMessage="Invalid Date" 
                            TooltipMessage="Input a Date in Date/Month/Year">
                        </cc1:MaskedEditValidator>
                    </td>
                    <td>
                        <asp:Label ID="Label5" runat="server" CssClass="field_caption" Text="To Date"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtToDate" runat="server" onchange="ValidateChkInDate();" 
                            Width="75px" TabIndex="7"></asp:TextBox>
                        <asp:ImageButton ID="ImgBtnToDt" runat="server" 
                            ImageUrl="~/Images/Calendar_scheduleHS.png" TabIndex="3" />
                        <cc1:CalendarExtender ID="dpToDate" runat="server" Format="dd/MM/yyyy" 
                            OnClientDateSelectionChanged="DateSelectCalExt" PopupButtonID="ImgBtnToDt" 
                            PopupPosition="Right" TargetControlID="txtToDate">
                        </cc1:CalendarExtender>
                        <cc1:MaskedEditExtender ID="MeToDate" runat="server" Mask="99/99/9999" 
                            MaskType="Date" TargetControlID="txtToDate">
                        </cc1:MaskedEditExtender>
                        <cc1:MaskedEditValidator ID="MevToDate" runat="server" 
                            ControlExtender="MeToDate" ControlToValidate="txtToDate" CssClass="field_error" 
                            Display="Dynamic" EmptyValueBlurredText="Date is required" 
                            EmptyValueMessage="Date is required" ErrorMessage="MeFromDate" 
                            InvalidValueBlurredMessage="Invalid Date" InvalidValueMessage="Invalid Date" 
                            TooltipMessage="Input a Date in Date/Month/Year">
                        </cc1:MaskedEditValidator>
                    </td>
                   
                    <td>
                        <asp:Button ID="btnFilter" runat="server" CssClass="btn" Font-Bold="False" 
                            tabIndex="8" Text="Search by Date" />
                        &nbsp;<asp:Button ID="btnClearDate" runat="server" CssClass="btn" Font-Bold="False" 
                            tabIndex="9" Text="Reset Dates" />
                    </td>              <td>  <asp:Label ID="RowSelectms" runat="server" CssClass="field_caption" 
                            Text="Rows Selected "></asp:Label>
                     <asp:DropDownList ID="RowsPerPageMS" runat="server" AutoPostBack="true" 
                            TabIndex="10">
                  
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






                            </tbody>
                        </table>
                        <input id="txtconnection" runat="server" style="visibility: hidden; width: 12px;
                            height: 9px" type="text" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td style="width: 100%">
                &nbsp;<asp:Button ID="btnExportToExcel" runat="server" CssClass="btn" TabIndex="6" OnClick="btnExporttoexcel_Click"
                    Text="Export To Excel" />
            </td>
        </tr>
        <tr>
            <td style="width: 100%">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <asp:GridView ID="gv_SearchResult" TabIndex="8" runat="server" Font-Size="10px" Width="100%"
                            CssClass="grdstyle" GridLines="Vertical" CellPadding="3" BorderWidth="1px" BorderStyle="None"
                            AutoGenerateColumns="False" AllowSorting="True" AllowPaging="True">
                            <FooterStyle CssClass="grdfooter"></FooterStyle>
                            <Columns>
                               <asp:TemplateField Visible="False" HeaderText="sectorcode"><EditItemTemplate>
<asp:TextBox runat="server" Text='<%# Bind("sectorcode") %>' id="TextBox1" 
        CssClass="field_input"></asp:TextBox>
</EditItemTemplate>
<ItemTemplate>
<asp:Label id="lblsectorCode" runat="server" Text='<%# Bind("sectorcode") %>' __designer:wfdid="w26"></asp:Label>
</ItemTemplate>
</asp:TemplateField>
<asp:BoundField DataField="sectorcode" SortExpression="sectorcode" HeaderText="Sector Code"></asp:BoundField>

 <asp:TemplateField  HeaderText="Sector Name" SortExpression="ctryname">
                    <ItemTemplate>
                        <asp:Label ID="lblSectorName" runat="server" Text='<%# Bind("sectorname") %>'></asp:Label>
                    </ItemTemplate>
                       <HeaderStyle HorizontalAlign="Left" Width="12%" />
                    <ItemStyle CssClass="ctryname" HorizontalAlign="Left" Width="12%" />
                </asp:TemplateField>
                     
                     <asp:TemplateField  HeaderText="Country Name" SortExpression="ctryname">
                    <ItemTemplate>
                        <asp:Label ID="lblCountryName" runat="server" Text='<%# Bind("ctryname") %>'></asp:Label>
                    </ItemTemplate>
                     <HeaderStyle HorizontalAlign="Left" Width="12%" />
                    <ItemStyle CssClass="ctryname" HorizontalAlign="Left" Width="12%" />
                </asp:TemplateField>

                    <%--            <asp:BoundField HtmlEncode="False" DataField="plgrpcode" SortExpression="plgrpcode"
                                    HeaderText="Market Code">
                                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                </asp:BoundField>--%>
                         <%--       <asp:BoundField HtmlEncode="False" DataField="plgrpname" SortExpression="plgrpname"
                                    HeaderText="Market Name">
                                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                </asp:BoundField>--%>
                                <asp:BoundField DataField="Active" SortExpression="Active" HeaderText="Active"></asp:BoundField>
                                <asp:BoundField HtmlEncode="False" DataFormatString="{0:dd/MM/yyyy hh:mm:ss tt} "
                                    DataField="adddate" SortExpression="adddate" HeaderText="Date Created">
                                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                </asp:BoundField>
                                <asp:BoundField HtmlEncode="False" DataField="adduser" SortExpression="adduser" HeaderText="User Created">
                                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                </asp:BoundField>
                                <asp:BoundField HtmlEncode="False" DataFormatString="{0:dd/MM/yyyy hh:mm:ss tt} "
                                    DataField="moddate" SortExpression="moddate" HeaderText="Date Modified">
                                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="moduser" SortExpression="moduser" HeaderText="User Modified">
                                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                </asp:BoundField>
                                <asp:ButtonField HeaderText="Action" Text="Edit" CommandName="Editrow">
                                    <ItemStyle ForeColor="Blue"></ItemStyle>
                                </asp:ButtonField>
                                <asp:ButtonField HeaderText="Action" Text="View" CommandName="View">
                                    <ItemStyle ForeColor="Blue"></ItemStyle>
                                </asp:ButtonField>
                                <asp:ButtonField HeaderText="Action" Text="Delete" CommandName="Deleterow">
                                    <ItemStyle ForeColor="Blue"></ItemStyle>
                                </asp:ButtonField>
                            </Columns>
                            <RowStyle CssClass="grdRowstyle"></RowStyle>
                            <SelectedRowStyle CssClass="grdselectrowstyle"></SelectedRowStyle>
                            <PagerStyle CssClass="grdpagerstyle" HorizontalAlign="Center"></PagerStyle>
                            <HeaderStyle CssClass="grdheader" ForeColor="white"></HeaderStyle>
                            <AlternatingRowStyle CssClass="grdAternaterow"></AlternatingRowStyle>
                        </asp:GridView>
                        <asp:Label ID="lblMsg" runat="server" Text="Records not found. Please redefine search criteria"
                            Font-Size="8pt" Font-Names="Verdana" ForeColor="#084573" Font-Bold="True" Width="921px"
                            Visible="False"></asp:Label>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
    <script language="javascript" type="text/javascript">


        function CallWebMethod(methodType) {
            switch (methodType) {
                case "mktcode":
                    var select = document.getElementById("<%=ddlmarketcd.ClientID%>");
                    var codeid = select.options[select.selectedIndex].text;
                    var selectname = document.getElementById("<%=ddlmarketnm.ClientID%>");
                    selectname.value = select.options[select.selectedIndex].text;
                    var connstr = document.getElementById("<%=txtconnection.ClientID%>");
                    constr = connstr.value

                    ColServices.clsServices.GetCountryCodeListnew(constr, codeid, FillCountryCodes, ErrorHandler, TimeOutHandler);
                    ColServices.clsServices.GetCountryNameListnew(constr, codeid, FillCountryNames, ErrorHandler, TimeOutHandler);
                    break;
                case "mktname":
                    var select = document.getElementById("<%=ddlmarketnm.ClientID%>");
                    var codeid = select.options[select.selectedIndex].value;
                    var selectname = document.getElementById("<%=ddlmarketcd.ClientID%>");
                    selectname.value = select.options[select.selectedIndex].text;
                    var connstr = document.getElementById("<%=txtconnection.ClientID%>");
                    constr = connstr.value

                    ColServices.clsServices.GetCountryCodeListnew(constr, codeid, FillCountryCodes, ErrorHandler, TimeOutHandler);
                    ColServices.clsServices.GetCountryNameListnew(constr, codeid, FillCountryNames, ErrorHandler, TimeOutHandler);
                    break;
                case "ctrycode":
                    var select = document.getElementById("<%=ddlcountrycd.ClientID%>");
                    var codeid = select.options[select.selectedIndex].text;
                    var selectname = document.getElementById("<%=ddlcountrynm.ClientID%>");
                    selectname.value = select.options[select.selectedIndex].text;
                    break;
                case "ctryname":
                    var select = document.getElementById("<%=ddlcountrynm.ClientID%>");
                    var codeid = select.options[select.selectedIndex].value;
                    var selectname = document.getElementById("<%=ddlcountrycd.ClientID%>");
                    selectname.value = select.options[select.selectedIndex].text;
                    break;
            }
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


        function FillCountryCodes(result) {
            var ddl = document.getElementById("<%=ddlcountrycd.ClientID%>");
            RemoveAll(ddl)
            for (var i = 0; i < result.length; i++) {
                var option = new Option(result[i].ListText, result[i].ListValue);
                ddl.options.add(option);
            }
            ddl.value = "[Select]";
        }
        function FillCountryNames(result) {
            var ddl = document.getElementById("<%=ddlcountrynm.ClientID%>");
            RemoveAll(ddl)
            for (var i = 0; i < result.length; i++) {
                var option = new Option(result[i].ListText, result[i].ListValue);
                ddl.options.add(option);
            }
            ddl.value = "[Select]";
        }
    </script>
    <asp:ScriptManagerProxy id="ScriptManagerProxy1" runat="server">
        <Services>
            <asp:ServiceReference Path="../clsSectorServices.asmx" />
              <asp:ServiceReference Path="~/clsServices.asmx" />

        </Services>
    </asp:ScriptManagerProxy>
    </asp:Content>