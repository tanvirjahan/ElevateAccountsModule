 

<%@ Page Language="VB" MasterPageFile="~/SubPageMaster.master" AutoEventWireup="false" CodeFile="SalesInvoice.aspx.vb" Inherits="SalesInvoice"  %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
      <%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
    <%@ OutputCache location="none" %> 

    <%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
   <%@ Register Assembly="EclipseWebSolutions.DatePicker" Namespace="EclipseWebSolutions.DatePicker"
    TagPrefix="ews" %> 
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
          $txtvsprocess.val('INVOICENO:" "STATUS:" "CUSTOMER:" "BOOKINGNO:" " TEXT:" "');
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
                          },{ 
                              duration: 1000,
                              queue: false
                          });
                      }, 2000);
                  },
                  valueMatches: function (category, searchTerm, callback) {
                      switch (category) {
                          case 'SUPPLIER':

                              glcallback = callback;
                              asSqlqry = ""
                              alert('aa');
                              ColServices.clsExcTrackingServices.GetListOfOpeSupplierVisualSearch(asSqlqry, fnFillSearchVS, ErrorHandler, TimeOutHandler);

                              break;
                      }
                  },
                  facetMatches: function (callback) {
                      callback([
                { label: 'TEXT', category: 'CATEGORY' },
                      { label: 'Company', category: 'CATEGORY' },
                { label: 'EXCURSION', category: 'CATEGORY' },
                 { label: 'SUPPLIER', category: 'CATEGORY' },
                  { label: 'CUSTOMER', category: 'CATEGORY' },
                   { label: 'REQUESTID', category: 'CATEGORY' },

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



 <script type="text/javascript" charset="utf-8">
     $(document).ready(function () {

/        transuppAutoCompleteExtenderKeyUp();

     });

     function transsuppautocompleteselected(source, eventArgs) {
 

   }
     function transuppAutoCompleteExtenderKeyUp() {

         

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
   <script type="text/javascript">
       var prm = Sys.WebForms.PageRequestManager.getInstance();
       prm.add_endRequest(EndRequestUserControl);
       function EndRequestUserControl(sender, args) {
           transuppAutoCompleteExtenderKeyUp();

       }
</script>
<%--<script language="javascript" type="text/javascript">
    function GetSpTypeValueFrom() {

        var ddl = document.getElementById("<%=ddlSupplierTypeName.ClientID%>");
        ddl.selectedIndex = -1;
        // Iterate through all dropdown items.
        for (i = 0; i < ddl.options.length; i++) {
            if (ddl.options[i].text ==
			document.getElementById("<%=ddlSupplierType.ClientID%>").value) {
                // Item was found, set the selected index.
                ddl.selectedIndex = i;
                return true;
            }
        }
    }
    function GetSpTypeValueCode() {
        var ddl = document.getElementById("<%=ddlSupplierType.ClientID%>");
        ddl.selectedIndex = -1;
        // Iterate through all dropdown items.
        for (i = 0; i < ddl.options.length; i++) {
            if (ddl.options[i].text ==
			document.getElementById("<%=ddlSupplierTypeName.ClientID%>").value) {
                // Item was found, set the selected index.
                ddl.selectedIndex = i;
                return true;
            }
        }
    }

</script>--%>
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

    <table>
        <tr>
            <td align="left" >
                <table style="border-right: gray 2px solid; border-top: gray 2px solid; border-left: gray 2px solid;
                     border-bottom: gray 2px solid">
                    <tr>
                        <td align="center" class="field_heading" colspan="4" >
                           Sales Invoice</td>
                    </tr>
                    <tr>
                        <td align="center" class="td_cell" colspan="4" style="color: blue; ">
                            Type few characters of code or name and click search &nbsp; &nbsp;</td>
                    </tr>
                    <tr>
                         <td style="width: 100%">
                            <asp:UpdatePanel id="UpdatePanel1" runat="server">
                                <contenttemplate><TABLE style="WIDTH: 811px"><TBODY>
                                <TR>

<TD align=center colSpan=6>
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp; &nbsp;&nbsp; &nbsp;<asp:Button id="btnhelp" 
        tabIndex=3   runat="server" Text="Help" 
        Font-Bold="False" CssClass="search_button">
  
        </asp:Button> 
        <asp:Button id="btnPrint" tabIndex=5 runat="server" Text="Report" 
        CssClass="btn"></asp:Button>&nbsp;&nbsp;<asp:Button id="btnAddNew" tabIndex=4 runat="server" Text="Add New" 
        Font-Bold="False" CssClass="btn"></asp:Button>&nbsp;&nbsp;</TD>
          &nbsp; 
        </TR>

<TR style="display:none">
<TD style="TEXT-ALIGN: center" class="td_cell" colSpan=6>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
 <asp:RadioButton id="rbtnsearch" runat="server" Text="Search" ForeColor="Black" CssClass="td_cell" Checked="True"   GroupName="GrSearch" AutoPostBack="True"></asp:RadioButton>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
  <asp:RadioButton id="rbtnadsearch" runat="server" Text="Advance Search" ForeColor="Black" CssClass="td_cell" __designer:wfdid="w43" OnCheckedChanged="rbtnadsearch_CheckedChanged" GroupName="GrSearch" AutoPostBack="True"></asp:RadioButton>&nbsp; 
  <asp:Button id="btnSearch" tabIndex=5 runat="server" Text="Search" 
        Font-Bold="False" CssClass="search_button" ></asp:Button>&nbsp; 
    <asp:Button id="btnClear" tabIndex=6 runat="server" Text="Clear" 
        Font-Bold="False" CssClass="search_button"></asp:Button>&nbsp;
  <asp:Button id="btnhelp1" tabIndex=10 onclick="btnhelp_Click" runat="server" 
        Text="Help" Font-Bold="False" CssClass="search_button"></asp:Button>&nbsp; 
  <asp:Button id="btnAddNew1" tabIndex=7 runat="server" Text="Add New" 
        Font-Bold="False" __designer:dtid="4222124650659856" CssClass="btn" 
        __designer:wfdid="w3"></asp:Button>&nbsp;
  <asp:Button id="btnPrint1" tabIndex=9 runat="server" Text="Report" __designer:dtid="4222124650659858" CssClass="btn" __designer:wfdid="w4"></asp:Button></TD>
  </TR>
  <TR style="display:none" ><TD class="td_cell"><SPAN style="COLOR: black">Supplier&nbsp;Category&nbsp;Code</SPAN></TD><TD>
  <asp:TextBox id="txtSupplierCategoriesCode" tabIndex=1 runat="server" Width="168px" CssClass="txtbox" __designer:wfdid="w46" MaxLength="20"></asp:TextBox></TD>
  <TD class="td_cell" style="width: 200px"><SPAN style="COLOR: black">Supplier&nbsp;Category&nbsp;Name</SPAN></TD><TD class="td_cell"><asp:TextBox id="txtSupplierCategoriesName" tabIndex=2 runat="server" Width="340px" CssClass="txtbox" __designer:wfdid="w47" MaxLength="100"></asp:TextBox></TD>
  <TD class="td_cell"><asp:Label id="Label3" runat="server" Text="Order By" Width="50px" CssClass="field_caption" __designer:wfdid="w1"></asp:Label>
  </TD><TD class="td_cell"><asp:DropDownList id="ddlOrderBy" runat="server" Width="125px" CssClass="drpdown" __designer:wfdid="w2" AutoPostBack="True" ></asp:DropDownList>
  </TD></TR><TR style="display:none"><TD class="td_cell"><asp:Label id="lblSupTypeCode" runat="server" Text="Supplier Type Code" Height="19px" Width="110px" CssClass="field_caption" __designer:wfdid="w8" Visible="False"></asp:Label></TD>
  <TD><SELECT onchange="GetSpTypeValueFrom()" style="WIDTH: 174px" id="ddlSupplierType" class="drpdown" tabIndex=3 runat="server" __designer:dtid="2533274790395929" Visible="false"> <OPTION selected __designer:dtid="2533274790395930"></OPTION></SELECT>
  </TD><TD class="td_cell"><asp:Label id="lblSupTypeName" runat="server" Text="Supplier Type Name" Height="19px" Width="110px" CssClass="field_caption" __designer:wfdid="w9" Visible="False"></asp:Label>
  </TD><TD class="td_cell"><SELECT onchange="GetSpTypeValueCode()" style="WIDTH: 345px" id="ddlSupplierTypeName" class="drpdown" tabIndex=4 runat="server" Visible="false"> <OPTION selected></OPTION></SELECT></TD>
  <TD class="td_cell"></TD><TD class="td_cell">
  </TD></TR></TBODY></TABLE>
              </contenttemplate>
                            </asp:UpdatePanel>
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
    CssClass="btn" Font-Bold="False" tabIndex="4" Text="Reset Search"  /></div></div>
           
           
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

                        <asp:Label ID="Label2" runat="server" CssClass="field_caption" Text=" Filter By"></asp:Label>

                                     <asp:DropDownList ID="ddlOrder" runat="server"    >
                            <asp:ListItem Value="T">Invoice Date</asp:ListItem>
                            <asp:ListItem Value="R">Amount</asp:ListItem>
                        </asp:DropDownList>
                        <asp:Label ID="Label4" runat="server" CssClass="field_caption" Text=" From Date"></asp:Label>

 
                        <asp:TextBox ID="txtFromDate" runat="server" onchange="filltodate(this);" 
                            Width="75px"></asp:TextBox>
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
                            TooltipMessage="Input a Date in Date/Month/Year"></cc1:MaskedEditValidator>
                               <asp:Label ID="Label5" runat="server" CssClass="field_caption" Text="To Date"></asp:Label>
                                <asp:TextBox ID="txtToDate" runat="server" onchange="ValidateChkInDate();" 
                            Width="75px"></asp:TextBox>
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
                            TooltipMessage="Input a Date in Date/Month/Year"></cc1:MaskedEditValidator>
                             <asp:Button ID="btnFilter" runat="server" CssClass="btn" Font-Bold="False" 
                            tabIndex="4" Text="Search by Date" />
                        &nbsp;<asp:Button ID="btnClearDate" runat="server" CssClass="btn" Font-Bold="False" 
                            tabIndex="4" Text="Reset Dates" />
                            <asp:Label ID="RowSelectscs" runat="server" CssClass="field_caption" 
                            Text="Rows Selected "></asp:Label>
                                               <asp:DropDownList ID="RowsPerPageSCS" runat="server" AutoPostBack="true">
                       
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
    </tr>
<%--  </TBODY>
    </TABLE
</contenttemplate>         
            </asp:UpdatePanel>--%>
               </td>                        
                    </tr>
                    <tr>
                                              <td style="width: 100%">
                          &nbsp;&nbsp;   
                           &nbsp;<asp:Button ID="btnExportToExcel" runat="server" CssClass="btn" 
                                Text="Export  to Excel" TabIndex="8" />
                                
                            </td></tr>
                    
                    <tr>
                        <td class="td_cell"  style="width: 100%; height: 3px">
                <asp:UpdatePanel id="UpdatePanel2" runat="server">
                    <contenttemplate>

<asp:GridView id="gv_SearchResult" tabIndex=10 runat="server" Font-Size="10px" Width="100%" CssClass="grdstyle" __designer:wfdid="w61" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False" BorderColor="#999999" BorderStyle="None" BorderWidth="1px" CellPadding="3" GridLines="Vertical">
<FooterStyle CssClass="grdfooter"></FooterStyle>
<Columns>

<asp:TemplateField  HeaderText="Cashpaid">
<ItemStyle HorizontalAlign="Center"></ItemStyle>
<ItemTemplate>
<INPUT id="ChkCashPaid" type="checkbox" runat="server" />
</ItemTemplate>
</asp:TemplateField>

<asp:BoundField DataField="Invoiceno" SortExpression="catcode" HeaderText="InvoiceNo."></asp:BoundField>

<asp:BoundField DataField="Status" SortExpression="catname" HeaderText="Status"></asp:BoundField>
<asp:BoundField DataField="Invoicedate" SortExpression="catcode" HeaderText="InvoiceDate"></asp:BoundField>
<asp:BoundField DataField="FileNumber" SortExpression="catname" HeaderText="FileNumber"></asp:BoundField>
 
 <asp:TemplateField    HeaderText="Cust Code" visible="false" >
<EditItemTemplate>
<asp:TextBox id="TextBox1" runat="server" Text='<%# bind("Customercode") %>' __designer:wfdid="w7"></asp:TextBox>
</EditItemTemplate>
<ItemTemplate>
<asp:Label id="lblcatcode" runat="server" Text='<%# bind("Customercode") %>' __designer:wfdid="w6"></asp:Label>
</ItemTemplate>
</asp:TemplateField>

<asp:BoundField DataField="Customername" SortExpression="catname" HeaderText="CustomerName"></asp:BoundField>
<asp:BoundField DataField="Customerref" SortExpression="catname" HeaderText="CustomerRef."></asp:BoundField>
<asp:BoundField DataField="Currency" SortExpression="catname" HeaderText="Currency"></asp:BoundField>
<asp:BoundField DataField="Amount" SortExpression="catname" HeaderText="Amount"></asp:BoundField>

<asp:BoundField DataField="Salesamount" SortExpression="catname" HeaderText="Sales Amount(AED)"></asp:BoundField>
<asp:BoundField DataField="Datecreated" SortExpression="catname" HeaderText="DateCreated"></asp:BoundField>
<asp:BoundField DataField="Usercreated" SortExpression="catname" HeaderText="UserCreated"></asp:BoundField>


<asp:BoundField DataField="Datemodified" SortExpression="catname" HeaderText="DateModified"></asp:BoundField>
<asp:BoundField DataField="Usermodified" SortExpression="catname" HeaderText="UserModified"></asp:BoundField>


 <asp:ButtonField HeaderText="Action"    Text="View" CommandName="DeleteRow">
<ItemStyle ForeColor="Blue"></ItemStyle>
</asp:ButtonField>

<asp:ButtonField  HeaderText ="Action"   Text="Print"   CommandName="DeleteRow">
<ItemStyle ForeColor ="Blue" ></ItemStyle>
</asp:ButtonField>
 
 <asp:ButtonField HeaderText ="Action" Text="PrintP/L" >
 <ItemStyle ForeColor ="Blue" />
 </asp:Buttonfield>
 <asp:ButtonField HeaderText ="Action" Text ="PrintJournal" >
<ItemStyle ForeColor ="Blue" />
 </asp:Buttonfield>


<%--<asp:TemplateField  HeaderText ="Assign Transfers">
<ItemTemplate >
 
  <asp:Button ID="btnAssTransfer" runat="server" CssClass="btn" TabIndex="14" Text=".." Width="40px"  OnClick="btnAssTransfer_Click" />
                                                                                       
</ItemTemplate>
</asp:TemplateField>--%>
 

</Columns>

<RowStyle CssClass="grdRowstyle"></RowStyle>

<SelectedRowStyle CssClass="grdselectrowstyle"></SelectedRowStyle>

<PagerStyle CssClass="grdpagerstyle" HorizontalAlign="Center"></PagerStyle>

<HeaderStyle CssClass="grdheader" ForeColor="White"></HeaderStyle>

<AlternatingRowStyle CssClass="grdAternaterow"></AlternatingRowStyle>
</asp:GridView> 


 



<asp:Label id="lblMsg" runat="server" 
                            Text="Records not found. Please redefine search criteria" Font-Size="8pt" 
                            Font-Names="Verdana" Font-Bold="True" Width="357px" __designer:wfdid="w60" 
                            Visible="False" CssClass="lblmsg"></asp:Label> 
</contenttemplate>
                </asp:UpdatePanel></td>
                    </tr>
                    <tr>
                    <td style="width: 100%">
                    </td>
                    </tr>
                            <tr>
                    <td style="width: 100%">
                    </td>
                    </tr>
                    <tr>

                          <td style="width: 100%">
                           
                            </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <input type="hidden"  id="hdnappid" runat="server" value="" />

     <asp:ScriptManagerProxy id="ScriptManagerProxy1" runat="server">
        <Services>
           <asp:ServiceReference Path="../clsExcTrackingServices.asmx" />
           </Services>
    </asp:ScriptManagerProxy>

</asp:Content>
