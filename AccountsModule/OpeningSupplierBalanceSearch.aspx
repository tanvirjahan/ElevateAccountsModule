<%@ Page Language="VB" MasterPageFile="~/AccountsMaster.master" AutoEventWireup="false" CodeFile="OpeningSupplierBalanceSearch.aspx.vb" Inherits="OpeningSupplierBalanceSearch"  %>

     <%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
 <%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
   
     <%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp"%>
    <%@ OutputCache location="none" %> 
<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">
      <meta http-equiv="content-type" content="text/html;charset=UTF-8" />
  <meta http-equiv="X-UA-Compatible" content="chrome=1">
<link href="../../css/Styles.css" rel="stylesheet" type="text/css" />
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
    <script type="text/jscript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.0/jquery.min.js">
    </script>

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
        .style4
        {
            width: 160px;
        }
        .style6
        {
            width: 50px;
        }
        .field_heading
        {}
        .style7
        {
            width: 4px;
        }
        .style8
        {
            width: 125px;
            text-align: right;
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


    function FormValidation(state) {



        if ((document.getElementById("<%=txtFromDate.ClientID%>").value == '') || (document.getElementById("<%=txtToDate.ClientID%>").value == '')) {

            alert("Select Dates ");
            return false;
        }





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
        var ValueRetrieval = document.getElementById("<%=hdOPMode.ClientID%>");
//        alert(ValueRetrieval.value);
                var $txtvsprocess = $(document).find('.cs_txtvsprocess');              
//              $txtvsprocess.val('DocumentNo:" "Supplier:" " Currency:" " TEXT:" "');
        if (ValueRetrieval.value == "S") {
            $txtvsprocess.val('OBSNo:" "Supplier:" " Currency:" " TEXT:" "');
        }
        if (ValueRetrieval.value == "C") {
            $txtvsprocess.val('OBCNo:" "Customer:" " Currency:" " TEXT:" "');
        }
        if (ValueRetrieval.value == "A") {
            $txtvsprocess.val('OBSANo:" "SupplierAgent:" " Currency:" " TEXT:" "');
        }

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
                    var $query = $('.search_query');
                    $query.stop().animate({ opacity: 1 }, { duration: 300, queue: false });
                    $query.html('<span class="raquo">&raquo;</span> You searched for: <b>' + searchCollection.serialize() + '</b>');

                    var $txtvsprocess = $(document).find('.cs_txtvsprocess');
                    $txtvsprocess.val(visualSearch.searchQuery.serialize());

                    var $txtvsprocesssplit = $(document).find('.cs_txtvsprocesssplit');
                    $txtvsprocesssplit.val(visualSearch.searchQuery.serializetosplit());

                    var btnvsprocess = document.getElementById("<%=btnvsprocess.ClientID%>");
                    btnvsprocess.click(visualSearch.searchQuery.serializetosplit());

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
                    if (ValueRetrieval.value == "S") {
                                 switch (category) {
                            case 'OBSNo':
                              var asSqlqry = "select ltrim(rtrim(tran_id)) tranid  from openparty_master(nolock) where open_type='S' order by tranid  ";
                                glcallback = callback;
                                ColServices.clsServices.GetListOfArrayVisualSearch(asSqlqry, fnFillSearchVS, ErrorHandler, TimeOutHandler);
                                break;
                            case 'Supplier':
                                var asSqlqry = "select ltrim(rtrim(des)) partyname  from view_account(nolock) where div_code= '" + hdndivcode.value + "'and type='S'  order by partyname ";
                                glcallback = callback;
                                ColServices.clsServices.GetListOfArrayVisualSearch(asSqlqry, fnFillSearchVS, ErrorHandler, TimeOutHandler);
                                break;
                            case 'Currency':
                                var asSqlqry = '';
                                glcallback = callback;
                                ColServices.clsSectorServices.GetListOfCurrencyVisualSearch(asSqlqry, fnFillSearchVS, ErrorHandler, TimeOutHandler);
                                break;
                        }
                    }
                    if (ValueRetrieval.value == "A") {
                        switch (category) {
                            case 'OBSANo':
                                var asSqlqry = "select ltrim(rtrim(tran_id)) tranid  from openparty_master(nolock) where open_type='A'  order by tranid ";
                                glcallback = callback;
                                ColServices.clsServices.GetListOfArrayVisualSearch(asSqlqry, fnFillSearchVS, ErrorHandler, TimeOutHandler);
                                break;
                            case 'SupplierAgent':
                                var asSqlqry = "select ltrim(rtrim(des)) partyname  from  view_account(nolock) where type='A' and div_code= '" + hdndivcode.value + "' order  by partyname ";
                                glcallback = callback;
                                ColServices.clsServices.GetListOfArrayVisualSearch(asSqlqry, fnFillSearchVS, ErrorHandler, TimeOutHandler);
                                break;
                            case 'Currency':
                                var asSqlqry = '';
                                glcallback = callback;
                                 ColServices.clsSectorServices.GetListOfCurrencyVisualSearch(asSqlqry, fnFillSearchVS, ErrorHandler, TimeOutHandler);
                                break;
                        }
                    }

                    if (ValueRetrieval.value == "C") {
                        switch (category) {
                            case 'OBCNo':
                                var asSqlqry = "select ltrim(rtrim(tran_id)) tranid  from openparty_master(nolock) where  open_type='C'  order by tranid ";
                                glcallback = callback;
                                ColServices.clsServices.GetListOfArrayVisualSearch(asSqlqry, fnFillSearchVS, ErrorHandler, TimeOutHandler);
                                break;

                            case 'Customer':
                                var asSqlqry = "select ltrim(rtrim(des)) partyname  from  view_account(nolock) where type='C' and div_code= '" + hdndivcode.value + "' order  by partyname ";
                                glcallback = callback;
                                ColServices.clsServices.GetListOfArrayVisualSearch(asSqlqry, fnFillSearchVS, ErrorHandler, TimeOutHandler);
                                break;
                            case 'Currency':
                                var asSqlqry = '';
                                glcallback = callback;
                                //fnTestCallback();
                                //                                alert('ff');
                                ColServices.clsSectorServices.GetListOfCurrencyVisualSearch(asSqlqry, fnFillSearchVS, ErrorHandler, TimeOutHandler);
                                break;
                        }
                    }
                },
                facetMatches: function (callback) {
                   if (ValueRetrieval.value == "S") {
                    callback([

 { label: 'OBSNo', category: 'OPBALANCE' },
  { label: 'Supplier', category: 'OPBALANCE' },
                { label: ' Currency', category: 'OPBALANCE' },
             { label: 'Text', category: 'OPBALANCE' },
             
              ]);
                }
                if (ValueRetrieval.value == "A") {
                    callback([

 { label: 'OBSANo', category: 'OPBALANCE' },
  { label: 'SupplierAgent', category: 'OPBALANCE' },
                { label: ' Currency', category: 'OPBALANCE' },
             { label: 'Text', category: 'OPBALANCE' },

              ]);
                }
                if (ValueRetrieval.value == "C") {
                    callback([

 { label: 'OBCNo', category: 'OPBALANCE' },
  { label: 'Customer', category: 'OPBALANCE' },
                { label: ' Currency', category: 'OPBALANCE' },
             { label: 'Text', category: 'OPBALANCE' },

              ]);
                }
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
      <%-- <asp:UpdatePanel id="UpdatePanel1" runat="server">
                    <contenttemplate> --%>
    <table style="border-right: gray 2px solid; border-top: gray 2px solid; border-left: gray 2px solid; border-bottom: gray 2px solid">
        <tr>
 
            <td>
       
<TABLE style="width:100%"><TBODY><TR>           <td valign="top" align="center"  class="field_heading"  Width="100%">     <asp:Label ID="lblHeading" runat="server" 
        Text="Opening Balance For Suppliers" ForeColor="White" 
                                 CssClass="field_heading"></asp:Label></TD></TR>
        
        <TR>
<TD align=center>
    &nbsp;</TD>
        </TR>
        
        <tr>
            <td align="center">
      
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp; &nbsp;&nbsp; &nbsp;<asp:Button ID="btnhelp" runat="server" 
                    CssClass="search_button" Font-Bold="False" onclick="btnhelp_Click" tabIndex="3" 
                    Text="Help" />
                &nbsp;&nbsp;<asp:Button ID="btnAddNew" runat="server" CssClass="btn" Font-Bold="False" 
                    tabIndex="4" Text="Add New" />
                &nbsp;&nbsp;&nbsp;&nbsp;<asp:Button ID="btnPrint_new" TabIndex="5" 
                    runat="server" Text="Report" CssClass="btn"></asp:Button>
                      
                                                         <asp:DropDownList ID="ddlrpt" runat="server">
                                                             <asp:ListItem Value="Brief">Brief Report</asp:ListItem>
                                                             <asp:ListItem Value="Detailed">Detailed Report</asp:ListItem>
                                                         </asp:DropDownList>
       <%--                               <asp:UpdatePanel id="UpdatePanel1" runat="server">
                    <contenttemplate>--%>
                <input style="visibility: hidden; width: 29px" id="txtDivcode" type="text" maxlength="20"
                                                                          
        runat="server" />
         
            <asp:HiddenField ID="hdFillByGrid" runat="server" />
          <asp:HiddenField ID="hdLinkButtonValue" runat="server" />
                <asp:HiddenField ID="hdOPMode" runat="server" />
            <%--        </contenttemplate>
                </asp:UpdatePanel>--%>
            </td>
      
    </tr>
        
        <TR>
        <TD style="TEXT-ALIGN: center" class="field_input">
<SPAN style="COLOR: blue">Type few characters of code or name and click search </SPAN>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </TD></TR></TBODY></TABLE>

            
                </td>
        </tr>
                 
                            <tr>
                   
        <td colspan="6">

         <div style="width:100%" >
        <div style="width:80%;display: inline-block; margin: -6px 4px 0 0;">
            <div ID="VS" class="container" style="border:0px;">
                <div ID="search_box_container">
                </div>   
                                 <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                            <ContentTemplate>                      
                <asp:TextBox ID="txtvsprocess" runat="server" class="cs_txtvsprocess" 
                    style="display:none"></asp:TextBox>
                <asp:TextBox ID="txtvsprocesssplit" runat="server" class="cs_txtvsprocesssplit" 
                   style="display:none"></asp:TextBox>
                <asp:Button ID="btnvsprocess" runat="server" style="display:none" />
                                            </ContentTemplate>
                        </asp:UpdatePanel>    
     
            </div> </div> 
             <div style=" width:18%; display: inline-block;vertical-align:top;">
                 <asp:Button ID="btnResetSelection" runat="server" 
    CssClass="btn" Font-Bold="False" tabIndex="4" Text="Reset Search" /></div></div>
     
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>                  
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
                                    </ContentTemplate>
                        </asp:UpdatePanel>    
     
        </td>
    </tr>

                      <tr>
        <td>
            <table class="style1">

                <tr>
 
                 
                    <td class="style8">
                        <asp:Label ID="Label6" runat="server" CssClass="field_caption" 
                            Text="Filter By Transaction"></asp:Label>
              </td>
                    <td class="style7">
                        &nbsp;</td>
                    <td style="text-align: right" >
                        <asp:Label ID="Label4" runat="server" CssClass="field_caption" Text="From Date"></asp:Label>


                    </td>
                    <td >
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
                            TooltipMessage="Input a Date in Date/Month/Year">
                        </cc1:MaskedEditValidator>
                    </td>
                    <td class="style6">
                        <asp:Label ID="Label5" runat="server" CssClass="field_caption" Text="To Date"></asp:Label>
                    </td>
                    <td class="style4">
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
                            TooltipMessage="Input a Date in Date/Month/Year">
                        </cc1:MaskedEditValidator>
                    </td>
                   
                    <td>
                        <asp:Button ID="btnFilter" runat="server" CssClass="btn" Font-Bold="False" 
                            tabIndex="4" Text="Search by Date" Width="122px" />
                        &nbsp;<asp:Button ID="btnClearDate" runat="server" CssClass="btn" Font-Bold="False" 
                            tabIndex="4" Text="Reset Dates" Width="98px" />
                        <asp:Label ID="RowSelectms" runat="server" CssClass="field_caption" 
                            Text="Rows Selected "></asp:Label>
                     &nbsp;<asp:DropDownList ID="RowsPerPageCUS" runat="server" AutoPostBack="true">
                  
                          <asp:ListItem Value="5">5</asp:ListItem>
                            <asp:ListItem Value="10">10</asp:ListItem>
                             <asp:ListItem Value="15">15</asp:ListItem>
                            <asp:ListItem Value="20">20</asp:ListItem>
                             <asp:ListItem Value="25">25</asp:ListItem>
                            <asp:ListItem Value="30">30</asp:ListItem>
                        </asp:DropDownList>
                    </td>              <td>  &nbsp;</td>
                 <tr>
                <td class="style8">
                    &nbsp;</td></tr>
            </table>
        </td>
    </tr>
<tr>

  <td>
    <table style="border-right: gray 1px solid; border-top: gray 1px solid; border-left: gray 1px solid;
        width: 100%; border-bottom: gray 1px solid">
        <tr>
            <td style="width: 100%; height: 21px">
    <asp:UpdatePanel id="UpdatePanel2" runat="server">
        <contenttemplate>
<asp:GridView id="gv_SearchResult" tabIndex=13 runat="server"  Width="100%" CssClass="grdstyle" GridLines="Vertical" CellPadding="3"  AutoGenerateColumns="False" AllowSorting="True" AllowPaging="True">
<Columns>
<asp:TemplateField Visible="False" HeaderText="Transaction Id"><EditItemTemplate>
                                <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("tran_id") %>'></asp:TextBox>
                            
</EditItemTemplate>
<ItemTemplate>
                                <asp:Label ID="lblTranID" runat="server" Text='<%# Bind("tran_id") %>'></asp:Label>
                            
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField Visible="False" HeaderText="Supplier"><EditItemTemplate>
                                <asp:TextBox ID="TextBox2" runat="server" Text='<%# Bind("open_code") %>'></asp:TextBox>
                            
</EditItemTemplate>
<ItemTemplate>
                                <asp:Label ID="lblpartycode" runat="server" Text='<%# Bind("open_code") %>'></asp:Label>
                            
</ItemTemplate>
</asp:TemplateField>

                                         <asp:TemplateField HeaderText="Document No." >
                    <ItemTemplate>
                        <asp:Label ID="lblTransID" runat="server" Text='<%# Bind("tran_id") %>'></asp:Label>
                    </ItemTemplate>
                    <ControlStyle  />
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle  HorizontalAlign="Left" />
                </asp:TemplateField>

<asp:BoundField DataField="post_state" HeaderText="Status"></asp:BoundField>
<asp:BoundField DataField="tran_type" SortExpression="tran_type" HeaderText="Transaction Type"></asp:BoundField>
<asp:BoundField HtmlEncode="False" DataFormatString="{00:dd/MM/yyyy}" DataField="tran_date" SortExpression="tran_date" HeaderText="Transaction Date"></asp:BoundField>
<asp:BoundField DataField="open_type" SortExpression="open_type" HeaderText="Type"></asp:BoundField>
<%--<asp:BoundField DataField="des" SortExpression="des" HeaderText="Name"></asp:BoundField>--%>
                                   <asp:TemplateField HeaderText="Name" >
                    <ItemTemplate>
                        <asp:Label ID="lbldes" runat="server" Text='<%# Bind("des") %>'></asp:Label>
                    </ItemTemplate>
                    <ControlStyle  />
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle  HorizontalAlign="Left" />
                </asp:TemplateField>
<asp:BoundField DataField="acctname" SortExpression="acctname" HeaderText="Control A/c"></asp:BoundField>
<asp:BoundField DataField="open_narration" SortExpression="open_narration" HeaderText="Narration"></asp:BoundField>
<asp:BoundField DataField="currcode" SortExpression="currcode" HeaderText="Currency"></asp:BoundField>
  
<asp:BoundField DataFormatString="{0:F3}" DataField="currrate" SortExpression="currrate" HeaderText="Conversion Rate">
<ItemStyle HorizontalAlign="Right"></ItemStyle>
</asp:BoundField>
<asp:BoundField HtmlEncode="False" DataFormatString="{0:F3}" DataField="open_debit" SortExpression="open_debit" HeaderText="Debit">
<ItemStyle HorizontalAlign="Right"></ItemStyle>
</asp:BoundField>
<asp:BoundField HtmlEncode="False" DataFormatString="{0:F3}" DataField="open_credit" SortExpression="open_credit" HeaderText="Credit">
<ItemStyle HorizontalAlign="Right"></ItemStyle>
</asp:BoundField>
<asp:BoundField HtmlEncode="False" DataFormatString="{0:F3}" DataField="openbase_debit" SortExpression="openbase_debit" HeaderText="Base Debit">
<ItemStyle HorizontalAlign="Right"></ItemStyle>
</asp:BoundField>
<asp:BoundField HtmlEncode="False" DataFormatString="{0:F3}" DataField="openbase_credit" SortExpression="openbase_credit" HeaderText="Base Credit">
<ItemStyle HorizontalAlign="Right"></ItemStyle>
</asp:BoundField>
<asp:BoundField HtmlEncode="False" DataFormatString="{0:dd/MM/yyyy hh:mm:ss tt} " DataField="adddate" SortExpression="adddate" HeaderText="Date Created"></asp:BoundField>
<asp:BoundField DataField="adduser" SortExpression="adduser" HeaderText="User Created"></asp:BoundField>
<asp:BoundField DataFormatString="{0:dd/MM/yyyy hh:mm:ss tt} " DataField="moddate" SortExpression="moddate" HeaderText="Date Modified"></asp:BoundField>
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
</Columns>


    <FooterStyle CssClass="grdfooter" />

<RowStyle CssClass="grdRowstyle"></RowStyle>
<SelectedRowStyle CssClass="grdselectrowstyle" ></SelectedRowStyle>
<PagerStyle CssClass="grdpagerstyle" HorizontalAlign="Center"></PagerStyle>
<HeaderStyle  CssClass="grdheader" ForeColor="white"></HeaderStyle>
<AlternatingRowStyle CssClass="grdAternaterow"></AlternatingRowStyle>
</asp:GridView>
 <asp:Label id="lblMsg" runat="server" 
                Text="Records not found. Please redefine search criteria" Font-Size="8pt" 
                Font-Names="Verdana" Font-Bold="True" Width="357px" 
                __designer:wfdid="w28" Visible="False" CssClass="lblmsg"></asp:Label> 
</contenttemplate>
    </asp:UpdatePanel></td>
        </tr>
    </table>

    </td>

    </tr>    </table>

     <asp:ScriptManagerProxy id="ScriptManagerProxy1" runat="server">
        <Services>
            <asp:ServiceReference Path="../clsSectorServices.asmx" />
              <asp:ServiceReference Path="~/clsServices.asmx" />

        </Services>
    </asp:ScriptManagerProxy>
<%--       </contenttemplate>
                </asp:UpdatePanel> --%>
</asp:Content>


