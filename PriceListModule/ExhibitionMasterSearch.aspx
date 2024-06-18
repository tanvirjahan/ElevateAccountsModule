﻿<%@ Page Title="" Language="VB" MasterPageFile="~/PriceListMaster.master" AutoEventWireup="false" CodeFile="ExhibitionMasterSearch.aspx.vb" Inherits="PriceListModule_ExhibitionMasterSearch" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ OutputCache location="none" %>
    <%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
 <%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">
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
              alert("Todate  should  be greater than From date");
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
          $txtvsprocess.val('ExhibitionName:" " TEXT:" "');
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
                          case 'ExhibitionName':
                              var asSqlqry = '';
                              glcallback = callback;
                              ColServices.clsSectorServices.GetListOfExhibitionVisualSearch(asSqlqry, fnFillSearchVS, ErrorHandler, TimeOutHandler);
                              break;
                      }
                  },
                  facetMatches: function (callback) {
                      callback([
                { label:'TEXT', category: 'Exhibition' },
                { label:'ExhibitionName', category: 'Exhibition' },
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

























 <table style="border-right: gray 2px solid; border-top: gray 2px solid; border-left: gray 2px solid; border-bottom: gray 2px solid">
        <tr>
            <td>
                <table>
                    <tr>
                        <td style="width: 100%; height: 11px" align="center" class="field_heading">
                            Exhibition Master List</td>
                    </tr>
                    <tr>
                        <td style="width: 100%; color: blue;" align="center" class="td_cell">
                            Type few characters of code or name and click search</td>
                    </tr>
                    <tr>
                        <td style="width: 100%;">
 <asp:UpdatePanel id="UpdatePanel1" runat="server">
 <contenttemplate>
<TABLE style="WIDTH: 468px">
<TBODY>

<TR>
<TD align=center colSpan=6>
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp; &nbsp;&nbsp; &nbsp;<asp:Button id="btnHelp" 
        tabIndex=3 onclick="btnhelp_Click" runat="server" Text="Help" 
        Font-Bold="False" CssClass="search_button"></asp:Button> &nbsp;&nbsp;<asp:Button id="btnAddNew" tabIndex=4 runat="server" Text="Add New" 
        Font-Bold="False" CssClass="btn"></asp:Button>&nbsp;&nbsp;<asp:Button 
        id="btnPrint" tabIndex=5 runat="server" Text="Report" 
        CssClass="btn"></asp:Button></TD>
        </TR>





<TR>
<TD style="WIDTH: 37px;display:none" class="td_cell">
<SPAN style="COLOR: black">Exhibition&nbsp;Code</SPAN></TD>
<TD style="WIDTH: 46px;display:none">
    <asp:TextBox id="txtExhibitionCode" tabIndex=1 runat="server" CssClass="field_input" MaxLength="20"></asp:TextBox></TD>
    <TD style="WIDTH: 42px; HEIGHT: 10px;display:none" class="td_cell">
    <SPAN style="COLOR: black">Exhibition&nbsp;Name</SPAN></TD>
    <TD style="WIDTH: 42px; HEIGHT: 10px;display:none" class="td_cell">
    <asp:TextBox id="txtExhibitionName" tabIndex=2 runat="server" Width="300px" CssClass="field_input" MaxLength="100"></asp:TextBox></TD>
    <TD style="WIDTH: 42px; HEIGHT: 10px;display:none" class="td_cell">
<asp:Button id="btnSearch" tabIndex=3 runat="server" Text="Search" Font-Bold="False" CssClass="search_button"></asp:Button></TD>
<TD style="WIDTH: 42px; HEIGHT: 10px;display:none" class="td_cell">
<asp:Button id="btnClear" tabIndex=4 runat="server" Text="Clear" Font-Bold="False" CssClass="search_button"></asp:Button></TD>
<TD style="WIDTH: 42px; HEIGHT: 10px;display:none" class="td_cell">
<asp:Button id="btnHelp1" tabIndex=8 runat="server" Text="Help" CssClass="search_button"></asp:Button></TD>
<td class="td_cell" style="width: 42px; height: 10px;display:none">
<asp:Button ID="btnAddNew1" runat="server" CssClass="btn" Font-Bold="False" Text="Add New" TabIndex="5" style="position: relative; top: 0px; left: 0px;" /></td>
<td class="td_cell" style="width: 42px; height: 10px;display:none">
<asp:Button ID="btnPrint1" runat="server" CssClass="btn" Text="Report" TabIndex="7" style="position: relative; top: 0px; left: 0px;" /></td>
</TR>
<TR>
<TD style="WIDTH: 37px; HEIGHT: 10px;display:none" class="td_cell">
<asp:Label id="Label3" runat="server" Text="Order By" Width="72px" CssClass="field_caption"></asp:Label>
</TD>
<TD style="WIDTH: 46px; HEIGHT: 10px;display:none">
<asp:DropDownList id="ddlOrderBy" runat="server" Width="150px" CssClass="drpdown"  
        AutoPostBack="true" OnSelectedIndexChanged="ddlOrderBy_SelectedIndexChanged"></asp:DropDownList></TD>
<TD style="WIDTH: 42px; HEIGHT: 10px;display:none" class="td_cell"></TD>
<TD style="WIDTH: 42px; HEIGHT: 10px;display:none" class="td_cell"></TD>
<TD style="WIDTH: 42px; HEIGHT: 10px;display:none" class="td_cell"></TD>
<TD style="WIDTH: 42px; HEIGHT: 10px;display:none" class="td_cell"></TD>
<TD style="WIDTH: 42px; HEIGHT: 10px;display:none" class="td_cell"></TD>
<td class="td_cell" style="width: 42px;display:none; height: 10px">
</td>
<td class="td_cell" style="width: 42px;display:none; height: 10px">
</td>
</TR>
</TBODY>
</TABLE> &nbsp;
</contenttemplate>
 </asp:UpdatePanel></td>
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
                        &nbsp;</td>
                    <td>
                        &nbsp;</td>
                    <td>
                        <asp:Label ID="Label6" runat="server" CssClass="field_caption" 
                            Text="Filter By "></asp:Label>
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
                        <asp:TextBox ID="txtFromDate" runat="server" onchange="filltodate(this);" 
                            Width="75px"></asp:TextBox>
                        <asp:ImageButton ID="ImgBtnFrmDt" runat="server" 
                            ImageUrl="~/Images/Calendar_scheduleHS.png" TabIndex="3" />
                        <cc1:calendarextender ID="dpFromDate" runat="server" Format="dd/MM/yyyy" 
                            OnClientDateSelectionChanged="DateSelectCalExt" PopupButtonID="ImgBtnFrmDt" 
                            PopupPosition="Right" TargetControlID="txtFromDate">
                        </cc1:calendarextender>
                        <cc1:maskededitextender ID="MeFromDate" runat="server" Mask="99/99/9999" 
                            MaskType="Date" TargetControlID="txtFromDate">
                        </cc1:maskededitextender>
                        <cc1:maskededitvalidator ID="MevFromDate" runat="server" 
                            ControlExtender="MeFromDate" ControlToValidate="txtFromDate" 
                            CssClass="field_error" Display="Dynamic" 
                            EmptyValueBlurredText="Date is required" EmptyValueMessage="Date is required" 
                            ErrorMessage="MeFromDate" InvalidValueBlurredMessage="Invalid Date" 
                            InvalidValueMessage="Invalid Date" 
                            TooltipMessage="Input a Date in Date/Month/Year">
                        </cc1:maskededitvalidator>
                    </td>
                    <td>
                        <asp:Label ID="Label5" runat="server" CssClass="field_caption" Text="To Date"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtToDate" runat="server" onchange="ValidateChkInDate();" 
                            Width="75px"></asp:TextBox>
                        <asp:ImageButton ID="ImgBtnToDt" runat="server" 
                            ImageUrl="~/Images/Calendar_scheduleHS.png" TabIndex="3" />
                        <cc1:calendarextender ID="dpToDate" runat="server" Format="dd/MM/yyyy" 
                            OnClientDateSelectionChanged="DateSelectCalExt" PopupButtonID="ImgBtnToDt" 
                            PopupPosition="Right" TargetControlID="txtToDate">
                        </cc1:calendarextender>
                        <cc1:maskededitextender ID="MeToDate" runat="server" Mask="99/99/9999" 
                            MaskType="Date" TargetControlID="txtToDate">
                        </cc1:maskededitextender>
                        <cc1:maskededitvalidator ID="MevToDate" runat="server" 
                            ControlExtender="MeToDate" ControlToValidate="txtToDate" CssClass="field_error" 
                            Display="Dynamic" EmptyValueBlurredText="Date is required" 
                            EmptyValueMessage="Date is required" ErrorMessage="MeFromDate" 
                            InvalidValueBlurredMessage="Invalid Date" InvalidValueMessage="Invalid Date" 
                            TooltipMessage="Input a Date in Date/Month/Year">
                        </cc1:maskededitvalidator>
                    </td>
                    <td>
                        <asp:Button ID="btnFilter" runat="server" CssClass="btn" Font-Bold="False" 
                            tabIndex="4" Text="Search by Date" />
                        &nbsp;<asp:Button ID="btnClearDate" runat="server" CssClass="btn" Font-Bold="False" 
                            tabIndex="4" Text="Reset Dates" />
                    </td>
                   
                               <td>  <asp:Label ID="RowSelectcos" runat="server" CssClass="field_caption" 
                            Text="Rows Selected "></asp:Label>
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


                    <tr>
                        <td style="width: 100%;">
                            <asp:Button ID="btnExportToExcel" runat="server" CssClass="btn" 
                                Text="Export To Excel" TabIndex="6" />
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td style="width: 100%">
                <asp:UpdatePanel id="UpdatePanel2" runat="server">
                    <contenttemplate>
<asp:GridView id="gv_SearchResult" tabIndex=9 runat="server" Font-Size="10px" Width="100%" CssClass="grdstyle" GridLines="Vertical" CellPadding="3" BorderWidth="1px" BorderStyle="None" AutoGenerateColumns="False" AllowSorting="True" AllowPaging="True">
<FooterStyle CssClass="grdfooter"></FooterStyle>
<Columns>
<asp:TemplateField Visible="False" HeaderText="Exhibition Code"><EditItemTemplate>
<asp:TextBox ID="txtexhibitionCode" runat="server" Text='<%# Bind("exhibitioncode") %>'></asp:TextBox>
</EditItemTemplate>
<ItemTemplate>
<asp:Label id="lblCode" runat="server" Text='<%# Bind("exhibitioncode") %>'></asp:Label> 
</ItemTemplate>
</asp:TemplateField>
<asp:BoundField DataField="exhibitioncode" SortExpression="exhibitioncode" HeaderText="Exhibition Code">
<ItemStyle HorizontalAlign="Left"></ItemStyle>
<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
</asp:BoundField>
<asp:BoundField DataField="exhibitionname" SortExpression="exhibitionname" HeaderText="Exhibition Name">
<ItemStyle HorizontalAlign="Left"></ItemStyle>
<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
</asp:BoundField>
<asp:BoundField DataField="IsActive" SortExpression="IsActive" HeaderText="Active"></asp:BoundField>
<asp:BoundField HtmlEncode="False" DataFormatString="{0:dd/MM/yyyy HH:mm:ss }" DataField="adddate" SortExpression="adddate" HeaderText="Date Created"></asp:BoundField>
<asp:BoundField DataField="adduser" SortExpression="adduser" HeaderText="User Created"></asp:BoundField>
<asp:BoundField HtmlEncode="False" DataFormatString="{0:dd/MM/yyyy HH:mm:ss }" DataField="moddate" SortExpression="moddate" HeaderText="Date Modified"></asp:BoundField>
<asp:BoundField DataField="moduser" SortExpression="moduser" HeaderText="User Modified"></asp:BoundField>
<asp:ButtonField HeaderText="Action" Text="Edit" CommandName="EditRow">
<ItemStyle ForeColor="Blue"></ItemStyle>
</asp:ButtonField>
<asp:ButtonField HeaderText="Action" Text="View" CommandName="View">
<ItemStyle ForeColor="Blue"></ItemStyle>
</asp:ButtonField>
<asp:ButtonField HeaderText="Action" Text="Delete" CommandName="DeleteRow">
<ItemStyle ForeColor="Blue"></ItemStyle>
</asp:ButtonField>

<asp:ButtonField HeaderText="Action" Text="Copy" CommandName="CopyRow">
<ItemStyle ForeColor="Blue"></ItemStyle>
</asp:ButtonField>
</Columns>
<RowStyle CssClass="grdRowstyle"></RowStyle>
<SelectedRowStyle CssClass="grdselectrowstyle"></SelectedRowStyle>
<PagerStyle CssClass="grdpagerstyle" HorizontalAlign="Center"></PagerStyle>
<HeaderStyle CssClass="grdheader" ForeColor="white"></HeaderStyle>
<AlternatingRowStyle CssClass="grdAternaterow"></AlternatingRowStyle>
</asp:GridView> <asp:Label id="lblMsg" runat="server" Text="Records not found, Please redefine search criteria" Font-Size="8pt" Font-Names="Verdana" ForeColor="#084573" Font-Bold="True" Width="357px" Visible="False"></asp:Label> 
</contenttemplate>
                </asp:UpdatePanel></td>
                    </tr>
                </table>
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



