<%@ Page Language="VB" AutoEventWireup="false" CodeFile="CustomerCategoriesSearch.aspx.vb"
    Inherits="CustomerCategoriesSearch" MasterPageFile="~/MainPageMaster.master"
    Strict="true" EnableEventValidation="false" %>


     <%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
 <%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ OutputCache Location="none" %>
<asp:Content ContentPlaceHolderID="Main" runat="server">

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
          $txtvsprocess.val('Name:" " TEXT:" "');
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
                        
                          case 'Name':
                              var asSqlqry = '';
                              glcallback = callback;
                              ColServices.clsSectorServices.GetListOfAgentCatNameVisualSearch(asSqlqry, fnFillSearchVS, ErrorHandler, TimeOutHandler);
                              break;
                      }
                  },
                  facetMatches: function (callback) {
                      callback([
                { label: 'TEXT', category: 'Name' },
                { label: 'Name', category: 'Name' },
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
        border-bottom: gray 2px solid">
        <tr>
            <td style="text-align: center;" class="field_heading" id="tblheading">
                Customer Categories
            </td>
        </tr>
        <tr>
            <td style="text-align: center;">
                <span class="td_cell" style="font-size: 8pt; color: blue; font-family: Arial">Type few
                    characters of code or name and click search</span>
            </td>
        </tr>
        <tr>
            <td>
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <table>
                            <tbody>
                                <tr style ="display:none">
                                    <td style="text-align: center" colspan="6">
                                        <asp:RadioButton ID="rbtnSearch"  runat="server" Text="Search" ForeColor="Black"
                                            CssClass="td_cell" __designer:wfdid="w19" AutoPostBack="True" GroupName="GrSearch"
                                            OnCheckedChanged="rbtnSearch_CheckedChanged" Checked="True"></asp:RadioButton>&nbsp;<asp:RadioButton
                                                ID="rbtnAdvance" TabIndex="12" runat="server" Text="Advance Search" ForeColor="Black"
                                                CssClass="td_cell" __designer:wfdid="w20" AutoPostBack="True" GroupName="GrSearch">
                                            </asp:RadioButton>&nbsp;
                                        <asp:Button ID="btnSearch" OnClick="btnSearch_Click" runat="server"
                                            Text="Search" Font-Bold="False" CssClass="search_button" __designer:wfdid="w21">
                                        </asp:Button>&nbsp;<asp:Button ID="btnClear" TabIndex="6" OnClick="btnClear_Click"
                                            runat="server" Text="Clear" Font-Bold="False" CssClass="search_button" __designer:wfdid="w22">
                                        </asp:Button>&nbsp; &nbsp;
                                        </td>
                                        </tr>
                                        <tr>
                                          <td style="text-align: center" colspan="6">
                                          <asp:Button ID="btnHelp" TabIndex="1" OnClick="btnHelp_Click"
                                            runat="server" Text="Help" __designer:dtid="1688858450198528" CssClass="search_button"
                                            __designer:wfdid="w1"></asp:Button>
                                        &nbsp;
                                        <asp:Button ID="btnAddNew" TabIndex="2" OnClick="btnAddNew_Click" runat="server"
                                            Text="Add New" Font-Bold="False" __designer:dtid="22236536045043712" CssClass="btn"
                                            __designer:wfdid="w15"></asp:Button>
                                        &nbsp;
                                        <asp:Button ID="btnPrint" TabIndex="3" runat="server" Text="Report" __designer:dtid="22236531750076416"
                                            CssClass="btn" __designer:wfdid="w16"></asp:Button>
                                    </td>
                                </tr>
                                <tr>
                                <td>
                        &nbsp;</td></tr>
                                   <tr style="display:none">
                                    <td style="width: 100px">
                                        <span style="font-size: 8pt"><span style="font-family: Arial">Category Code<strong></strong></span></span>
                                    </td>
                                    <td style="width: 99px">
                                        <asp:TextBox Style="position: static" ID="txtCusCode" TabIndex="1" runat="server"
                                            Width="199px" __designer:wfdid="w15" CssClass="field_input"></asp:TextBox>
                                    </td>
                                    <td style="width: 100px">
                                        <span style="font-size: 8pt; font-family: Arial">Category<strong> </strong>Name</span>
                                    </td>
                                    <td style="width: 100px">
                                        <asp:TextBox Style="position: static" ID="txtCusName" TabIndex="2" runat="server"
                                            Width="207px" __designer:wfdid="w16" CssClass="field_input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label3" runat="server" Text="Order By" Width="50px" CssClass="field_caption"
                                            __designer:wfdid="w21"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlOrderBy" runat="server" Width="104px" CssClass="drpdown"
                                            __designer:wfdid="w22" AutoPostBack="True" OnSelectedIndexChanged="ddlOrderBy_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr style="display :none">
                                    <td style="width: 100px">
                                        <asp:Label ID="lblSellingTypeCode" runat="server" Text="Selling Type Code" Width="112px"
                                            CssClass="td_cell" __designer:wfdid="w17"></asp:Label>
                                    </td>
                                    <td style="width: 99px">
                                        <select onchange="GetValueFromName()" style="width: 204px" id="ddlSellingType" class="drpdown"
                                           runat="server" visible="True">
                                            <option selected></option>
                                        </select>
                                    </td>
                                    <td style="width: 100px">
                                        <asp:Label ID="lblSellingTypeName" runat="server" Text="Selling Type Name" Width="121px"
                                            CssClass="td_cell" __designer:wfdid="w18"></asp:Label>
                                    </td>
                                    <td style="width: 100px">
                                        <select onchange="GetValueFromCode()" style="width: 214px" id="ddlSellingName" class="drpdown"
                                            runat="server" visible="True">
                                            <option selected></option>
                                        </select>
                                    </td>
                                    <td>
                                    </td>
                                    <td>
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
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;<asp:Button ID="btnExport" runat="server" CssClass="btn" OnClick="btnExport_Click"
                    TabIndex="11" Text="Export To Excel" />
            </td>
        </tr>
        <tr>
            <td>
                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                    <ContentTemplate>
                        <asp:GridView ID="gv_SearchResult" TabIndex="12" runat="server" Font-Size="10px"
                            BackColor="White" Width="100%" CssClass="td_cell" __designer:wfdid="w32" GridLines="Vertical"
                            CellPadding="3" BorderWidth="1px" BorderStyle="None" BorderColor="#999999" AllowSorting="True"
                            AllowPaging="True" AutoGenerateColumns="False">
                            <FooterStyle CssClass="grdfooter"></FooterStyle>
                            <Columns>
                                <asp:TemplateField SortExpression="agentcatcode" Visible="False" HeaderText="Category Code">
                                    <EditItemTemplate>
                                        <asp:TextBox runat="server" Text='<%# Bind("agentcatcode") %>' ID="TextBox1" CssClass="field_input"></asp:TextBox>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblCode" runat="server" Text='<%# Bind("agentcatcode") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="agentcatcode" SortExpression="agentcatcode" HeaderText="Category Code">
                                </asp:BoundField>
                             


                                <asp:TemplateField HeaderText="Category Name" SortExpression="agentcatname">
                    <ItemTemplate>
                        <asp:Label ID="lblagentcatname" runat="server" Text='<%# Bind("agentcatname") %>'></asp:Label>
                    </ItemTemplate>
                    <ControlStyle Width="15%" />
                    <HeaderStyle HorizontalAlign="Left" Width="15%" />
                    <ItemStyle CssClass="agentcatname" HorizontalAlign="Left" Width="12%" />
                </asp:TemplateField>


                           <%--     <asp:BoundField DataField="sellcode" SortExpression="sellcode" HeaderText="Sell Code">
                                </asp:BoundField>
                                <asp:BoundField DataField="sellname" SortExpression="sellname" HeaderText="Sell Name">
                                </asp:BoundField>
                                <asp:BoundField DataField="creditaction" SortExpression="creditaction" HeaderText="Credit Action">
                                </asp:BoundField>
                                <asp:BoundField DataField="commissionperc" SortExpression="commissionperc" HeaderText="Commission">
                                </asp:BoundField>--%>
                                <asp:BoundField DataField="active" SortExpression="active" HeaderText="Active"></asp:BoundField>
                                <asp:BoundField DataFormatString="{0:dd/MM/yyyy HH:mm:ss }" DataField="adddate" SortExpression="adddate"
                                    HeaderText="Date Created"></asp:BoundField>
                                <asp:BoundField DataField="adduser" SortExpression="adduser" HeaderText="User Created">
                                </asp:BoundField>
                                <asp:BoundField DataFormatString="{0:dd/MM/yyyy HH:mm:ss }" DataField="moddate" SortExpression="moddate"
                                    HeaderText="Date Modified"></asp:BoundField>
                                <asp:BoundField DataField="moduser" SortExpression="moduser" HeaderText="User Modified">
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
                        <asp:Label ID="lblMsg" runat="server" Text="Records not found, Please redefine search criteria"
                            Font-Size="8pt" Font-Names="Verdana" ForeColor="#084573" Font-Bold="True" Width="357px"
                            __designer:wfdid="w33" Visible="False"></asp:Label>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>

     <asp:ScriptManagerProxy id="ScriptManagerProxy1" runat="server">
        <Services>
            <asp:ServiceReference Path="../clsSectorServices.asmx" />
              <asp:ServiceReference Path="~/clsServices.asmx" />

        </Services>
    </asp:ScriptManagerProxy>

</asp:Content>
