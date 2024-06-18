<%@ Page Language="VB"  MasterPageFile="~/MainPageMaster.master" AutoEventWireup="false" CodeFile="HolidayCalendar.aspx.vb" Inherits="PriceListModule_Default" %>

  <%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
    <%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
     <%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

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
<script language ="javascript" type="text/javascript">

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
       function checkedvalue() {
                  var checkedValue = document.getElementById("<%=chkbox.ClientID%>");
           var lblfromdate = document.getElementById("<%=lblFromDate.ClientID%>");
           var lbltodate = document.getElementById("<%=lblTodate.ClientID%>");
           var txtfromdate = document.getElementById("<%=txtFromDate.ClientID%>");
           var txttodate = document.getElementById("<%=txtToDate.ClientID%>");
           var imgfromdate = document.getElementById("<%=ImgBtnFrmDt.ClientID%>");
           var imgtodate = document.getElementById("<%=ImgBtnToDt.ClientID%>");

           if (checkedValue.checked == true) {
               alert("No");
               lblfromdate.enabled = true
               txtfromdate.enabled = true
               imgfromdate.enabled = true
               lbltodate.enabled=false
               txttodate.enabled = false
               imgtodate.enabled = false
           }
           else {

               lblfromdate.enabled = true
               txtfromdate.enabled = true
               imgfromdate.enabled = true
               lbltodate.enabled = true
               txttodate.enabled = true
               imgtodate.enabled = true

           }
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

    function FormValidation(state) {


    }

    function GetValueFrom() {


    }
    function GetValueCode() {

    }

    function assignvalue(source, eventargs) {

    }
    //$(document).ready(function() {

    //    $('.btnExample').on('click', function () {
    //    alert('test')
    //        $('.btnExample').removeClass('selected');
    //        $(this).addClass('selected');
    //    });


    ////    $(".btnExample").click(function () {
    ////          $('btnExample').removeClass('selected');
    ////          $(this).addClass('btnExampleHold');
    ////    });
    //});
</script>
  <script type="text/javascript" charset="utf-8">
      $(document).ready(function () {

          txtNameAutoCompleteExtenderKeyUp();

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
          $txtvsprocess.val('HolidayName:" " CountryGroup:" "  Region:" " Country:" " Text:" "');

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
              placeholder: 'Search for',
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
                          case 'HolidayName':
                              var asSqlqry = '';
                              glcallback = callback;
                              //fnTestCallback();
                              ColServices.clsCountryGroupServices.GetListOfArrayHolidayNameVisualSearch(asSqlqry, fnFillSearchVS, ErrorHandler, TimeOutHandler);
                              break;
                          case 'CountryGroup':
                              var asSqlqry = '';
                              glcallback = callback;
                              //fnTestCallback();
                              ColServices.clsCountryGroupServices.GetListOfArrayCountryGroupVisualSearch(asSqlqry, fnFillSearchVS, ErrorHandler, TimeOutHandler);
                              break;
                          case 'Region':
                              var asSqlqry = '';
                              glcallback = callback;
                              //fnTestCallback();
                              ColServices.clsCountryGroupServices.GetListOfArrayRegionrVisualSearch(asSqlqry, fnFillSearchVS, ErrorHandler, TimeOutHandler);
                              break;
                          case 'Country':
                              var asSqlqry = '';
                              glcallback = callback;
                              //fnTestCallback();
                              ColServices.clsCountryGroupServices.GetListOfArrayCountryVisualSearch(asSqlqry, fnFillSearchVS, ErrorHandler, TimeOutHandler);
                              break;
                      }
                  },
                  facetMatches: function (callback) {
                      callback([
                { label: 'Text' },
                { label: 'HolidayName', category: 'Holiday' },
                  { label: 'CountryGroup', category: 'Holiday' },
                { label: 'Region', category: 'Holiday' },
       { label: 'Country', category: 'Holiday' },
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
         function txtNameautocompleteselected(source, eventArgs) {
             if (eventArgs != null) {

                 document.getElementById('<%=txtGroupTagName.ClientID%>').value = eventArgs.get_value();

             }
             else {
                 document.getElementById('<%=txtGroupTagName.ClientID%>').value = '';

             }
         }

         function txtNameAutoCompleteExtenderKeyUp() {

             $("#<%=txtGroupTagName.ClientID %>").bind("change", function () {

                 document.getElementById('<%=Txtcurrencycode.ClientID%>').value = '';

             }
           )
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
            txtNameAutoCompleteExtenderKeyUp();
        }
    </script>


<head>

<style>
    
    .btnExample {
  color: #2D7C8A;
  background: #e7e7e7;
  font-weight: bold;
  border: 1px solid #2D7C8A;
   padding: 5px 5px;
}
 
.btnExample:hover,.btnExample:hover {
  color: #FFF;
  background: #2D7C8A;
  transition: all 500ms ease-in-out;
}
.btnExample.selected{
  color:  #FFF;
  background: #2D7C8A;
  font-weight: bold;
  border: 1px solid #2D7C8A;
  padding: 5px 5px;
}

.btnExampleHold {
  color:  #FFF;
  background: #2D7C8A;
  font-weight: bold;
  border: 1px solid #2D7C8A;
   padding: 5px 5px;
}


</style>

<style>
    
    
.autocomplete_completionListElement
        {
	        visibility : hidden;
	        margin : 1px 0px 0px 0px!important;
	        background-color : #FFFFFF;
	        color : windowtext;
	        border : buttonshadow;
	        border-width : 1px;
	        border-style : solid;
	        cursor : 'default';
	        overflow : auto;
	        height : 200px;
            width:100px;
            text-align:left;
            list-style-type: none;
	           font-family:Verdana;
            font-size:small;
    
        }


        /* AutoComplete highlighted item */


        .autocomplete_highlightedListItem
        {
	        background-color:Silver;
	        color: black;
	          margin-left: -35px;
	          font-weight:bold;
        }


        /* AutoComplete item */

        .autocomplete_listItem
        {
	        background-color : window;
            color : windowtext;
	       margin-left: -35px;
        }

        </style>


<style >
    
    
    
   .AutoExtender
        {
            font-family: Verdana, Helvetica, sans-serif;
            font-size: .8em;
            font-weight: normal;
            border: solid 1px #006699;
            line-height: 20px;
            padding: 10px;
            background-color: White;
            margin-left:10px;
        }
        .AutoExtenderList
        {
            border-bottom: dotted 1px #006699;
            cursor: pointer;
            color: Maroon;
        }
        .AutoExtenderHighlight
        {
            color: White;
            background-color: #006699;
            cursor: pointer;
        }
        #divwidth
        {
          width: 150px !important;    
        }
        #divwidth div
       {
        width: 150px !important;   
       }

    .style1
    {
        width: 100%;
    }

    .style4
    {
        font-family: Verdana,Arial, Geneva, ms sans serif;
        font-size: 10pt;
        font-weight: normal;
        height: 5px;
        }
    .style5
    {
        width: 891px;
    }

</style>
</head>

    <asp:UpdatePanel id="UpdatePanel1" runat="server" style="vertical-align:top;" >
        <contenttemplate>
<TABLE style="BORDER-RIGHT: gray 2px solid; BORDER-TOP: gray 2px solid; BORDER-LEFT: gray 2px solid; WIDTH: 100%px; BORDER-BOTTOM: gray 2px solid;margin-left:35px;vertical-align:top;"  ><TBODY><TR>
    <TD style="HEIGHT: 15px" class="field_heading" align=center colSpan=4 
        width="10">
    <asp:Label id="lblHeading" runat="server" Text="Add\Edit Country Group" 
        Width="738px" CssClass=""></asp:Label></TD></TR><TR>
        <TD style="HEIGHT: 3px; text-align: center;" align="center" colspan="4">
            &nbsp;</TD>
    </TR>

    <tr>
        <td align="center" colspan="4" style="HEIGHT: 3px; text-align: center;">
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:Button ID="btnNew" runat="server" Text="New" CssClass="btnExample" />
            &nbsp;<asp:Button ID="btnEdit" runat="server" Text="Edit" CssClass="btnExample" />
            &nbsp;<asp:Button ID="btnDelete" runat="server" Text="Delete" CssClass="btnExample" />
            &nbsp;<asp:Button ID="btnView" runat="server" CssClass="btnExample" 
                Text="View" />
            &nbsp;<asp:Button ID="btnCancel_new" runat="server" Text="Cancel" 
                CssClass="btnExample" />
              &nbsp;<asp:Button ID="btnHotelReport" runat="server" CssClass="btn" 
                Font-Bold="False"  Visible="true"  tabIndex="4" Text="Report" />
        </td>
    </tr>

    <tr>
        <td class="style4" colspan="4">
            </td>
    </tr>
    <tr>
    <td style="display:none">
            &nbsp;<asp:Button ID="btnExcel" runat="server" CssClass="btnExample" />
    
       </td>
    </tr>
    <tr>
        <td class="style4" colspan="4" valign="top">
            <table class="style1">
                <tr>
                   
                  
                    <td colspan="3">
                        <asp:HiddenField ID="hdFillByGrid" runat="server" />
                        <asp:HiddenField ID="hdLinkButtonValue" runat="server" />
                        <asp:HiddenField ID="hdOPMode" runat="server" />
                    </td>
                    <td>
                        &nbsp;</td>
                </tr>
                <tr runat="server" id="trNameAndCode">
                    <td colspan="2">
                        <asp:Label ID="Label10" runat="server" Text="Code"></asp:Label>
                        &nbsp;
                        <asp:TextBox ID="txtGroupCode" runat="server"></asp:TextBox>

                         </td>
                       <td colspan="2">
                        <asp:Label ID="Label1" runat="server" Text="Holiday Name"></asp:Label>
                        &nbsp;&nbsp;<asp:TextBox ID="txtGroupTagName" runat="server" AutoPostBack="True" 
                            Width="300px"></asp:TextBox>
                            <asp:TextBox ID="Txtcurrencycode" runat="server" style="display:none"  ></asp:TextBox>
                            <asp:HiddenField ID="HiddenField1" runat="server"  />
                              <asp:AutoCompleteExtender ID="txtGroupTagName_AutoCompleteExtender" runat="server" 
                            CompletionInterval="10" 
                            CompletionListCssClass="autocomplete_completionListElement" 
                            CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem" 
                            CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="1" 
                            DelimiterCharacters="" EnableCaching="false" Enabled="True" 
                            FirstRowSelected="false" MinimumPrefixLength="0" ServiceMethod="GetHolidayName" 
                            TargetControlID="txtGroupTagName" OnClientItemSelected="txtNameautocompleteselected">
                        </asp:AutoCompleteExtender>
                        
                        &nbsp;<asp:CheckBox ID="chkActive" runat="server" Checked="True" Text="Active" />
                    </td>
                    <td>
                        &nbsp;</td>
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
                </tr>
                <tr>
                    <td colspan="6">
                            <div style="width:100%" >
        <div style="width:80%;display: inline-block; margin: -6px 4px 0 0;">
            <div ID="VS" class="container" style="border:0px;">
                <div ID="search_box_container">
                </div>                        
                    
                <%--      <div ID="search_query">
                &nbsp;</div>--%>
                <asp:TextBox ID="txtvsprocess" runat="server" class="cs_txtvsprocess" 
                    style="display:none"></asp:TextBox>
                <asp:TextBox ID="txtvsprocesssplit" runat="server" class="cs_txtvsprocesssplit" 
                    style="display:none"></asp:TextBox>
                <%--<asp:textbox id="txtvsprocessCity" runat="server"></asp:textbox>
                                    <asp:textbox id="txtvsprocessCountry" runat="server"></asp:textbox>--%>
                <asp:Button ID="btnvsprocess" runat="server" style="display:none" />
            </div> </div> 
             <div style=" width:18%; display: inline-block;vertical-align:top;">
                 <asp:Button ID="btnResetSelection" runat="server" 
    CssClass="btn" Font-Bold="False" 
                            tabIndex="4" Text="Reset Search" /></div></div>
                    </td>
                </tr>
                <tr style="display:none;" >
                   <td>
                        <asp:Label ID="Label9" runat="server" Text="Search Name"></asp:Label>
                    </td>
                    <td colspan="3">
                        <asp:TextBox ID="txtName" runat="server" AutoPostBack="True" Width="300px"></asp:TextBox>
                        <asp:AutoCompleteExtender ID="txtName_AutoCompleteExtender" runat="server" 
                            CompletionInterval="10" 
                            CompletionListCssClass="autocomplete_completionListElement" 
                            CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem" 
                            CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="1" 
                            DelimiterCharacters="" EnableCaching="false" Enabled="True" 
                            FirstRowSelected="True" MinimumPrefixLength="1" ServiceMethod="GetHolidayName" 
                            TargetControlID="txtName">
                        </asp:AutoCompleteExtender>
                        <asp:TextBox ID="txtNameNew" runat="server" Visible="False"></asp:TextBox>
                        <asp:Label ID="Label6" runat="server" Font-Names="Verdana" Font-Size="Small" 
                            ForeColor="#990000" Text="Search By: Country, Region and Country Group Tags."></asp:Label>
                    </td>
                    <td>
                        &nbsp;</td>
                        </tr>
                        </table>
                        </td>
                </tr>

                <tr>
                  
                    <td colspan="3">
                        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                            <ContentTemplate>
                              <asp:DataList ID="dlList" runat="server" RepeatColumns="4" 
                                    RepeatDirection="Horizontal">
                                    <ItemTemplate>
                                        <table class="style1">
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblType" runat="server" Visible="false" Text='<%# Eval("Code") %>'></asp:Label>
                                                    <asp:LinkButton ID="lbCountry" class="button button4" runat="server" 
                                                        text='<%# Eval("Country") %>' onclick="lbCountry_Click">LinkButton</asp:LinkButton>
                                                    <asp:LinkButton ID="lbClose" class="buttonClose button4" runat="server" 
                                                        onclick="lbClose_Click">X</asp:LinkButton>
                                                </td>
                                            </tr>
                                        </table>
                                    </ItemTemplate>
                                </asp:DataList>
                                <asp:DataList ID="dlListSearch" runat="server" RepeatColumns="4" 
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
                                    onclick="lbCloseSearch_Click" Text="X" />
                            </td>
                        </tr>
                    </table>
                                    </ItemTemplate>
                                </asp:DataList>
                           
                 <%--     </td></tr>--%>
                    
                    
              
                                    
<%--<tr>
<td colspan="6">--%>
<table class="style1">
   <tbody>
   <td> 
                
                  
    <%-- <input type="checkbox" name="single" onchange="checked();" value="single">single date</td>--%>
     <asp:CheckBox ID="chkbox" runat="server" Text="Single Date" AutoPostBack =true  /></td>  <%--onchange="checkedvalue()"--%>
     

                          <td>
                        <asp:Label ID="lblFromDate" runat="server" CssClass="field_caption" Text="From Date"></asp:Label>
                    <%--</td>
                    <td>--%>
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
                    </td>
                    <td>
                        <asp:Label ID="lblTodate" runat="server" CssClass="field_caption" Text="To Date"></asp:Label>
                    <%--</td>--%><%--<td>--%><asp:TextBox ID="txtToDate" runat="server" onchange="ValidateChkInDate();" 
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
        </td>          
                    
    </tbody></table>
</contenttemplate>                
                </asp:UpdatePanel></td>

                 
    <TR>
<TD class="td_cell" colspan="4">
    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
        <ContentTemplate>
        <div id="dv_SearchResult" runat="server" style="max-height:250px;overflow:auto;" >
            <asp:GridView ID="gv_SearchResult" runat="server" __designer:wfdid="w79" 
                AllowSorting="True" AutoGenerateColumns="False" 
                BackColor="White" BorderColor="#999999" BorderStyle="None" BorderWidth="1px" 
                CellPadding="3" CssClass="td_cell" Font-Size="10px" GridLines="Vertical" 
                tabIndex="12" Width="100%">
                <FooterStyle CssClass="grdfooter" />
                <Columns>
                    <asp:TemplateField HeaderText="ctrycode" Visible="False">
                        <EditItemTemplate>
                            <asp:TextBox ID="TextBox3" runat="server" CssClass="field_input" 
                                Text='<%# Bind("ctrycode") %>'></asp:TextBox>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblcntryCode" runat="server" __designer:wfdid="w26" 
                                Text='<%# Bind("ctrycode") %>'></asp:Label>
                            <br />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <EditItemTemplate>
                            <asp:TextBox ID="TextBox2" runat="server"></asp:TextBox>
                        </EditItemTemplate>
                        <HeaderTemplate>
                            <asp:CheckBox ID="chkSelectAll" runat="server" AutoPostBack="True" 
                                oncheckedchanged="chkSelectAll_CheckedChanged" Text="Select All" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="Label5" runat="server"></asp:Label>
                            <asp:CheckBox ID="chkSelect" runat="server" AutoPostBack="True" 
                                oncheckedchanged="chkSelect_CheckedChanged" />
                        </ItemTemplate>
                        <HeaderStyle Width="6%" />
                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" Width="6%" />
                    </asp:TemplateField>
                    <asp:BoundField DataField="ctrycode" HeaderText="Country Code">
                        <ControlStyle Width="10%" />
                        <ItemStyle Width="10%" />
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="Country Name">
                        <EditItemTemplate>
                            <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("country") %>'></asp:TextBox>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblCountryName" runat="server" Text='<%# Bind("country") %>' 
                                Width="100%"></asp:Label>
                        </ItemTemplate>
                        <HeaderStyle Width="15%" />
                        <ItemStyle Width="15%" />
                    </asp:TemplateField>
                    <asp:BoundField DataField="region" HeaderText="Region">
                        <HeaderStyle Width="15%" />
                    </asp:BoundField>
                    <asp:BoundField DataField="countrygroups" HeaderText="Country Group ">
                        <HeaderStyle Width="15%" />
                        <ItemStyle Width="15%" />
                  </asp:BoundField>
                    <asp:BoundField DataField="Holidays" HeaderText="Holiday Name">
                        <HeaderStyle Width="15%" />
                        <ItemStyle Width="15%" />
                   </asp:BoundField>
                </Columns>
                <RowStyle CssClass="grdRowstyle" />
                <SelectedRowStyle CssClass="grdselectrowstyle" />
                <PagerStyle CssClass="grdpagerstyle" HorizontalAlign="Center" />
                <HeaderStyle CssClass="grdheader" ForeColor="white" />
                <AlternatingRowStyle CssClass="grdAternaterow" />
            </asp:GridView></div>
        </ContentTemplate>
    </asp:UpdatePanel>
      <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                <ContentTemplate>
                                    <asp:GridView ID="gvSearchGrid" runat="server" __designer:wfdid="w79" 
                                        AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False" 
                                        BackColor="White" BorderColor="#999999" BorderStyle="None" BorderWidth="1px" 
                                        CellPadding="3" CssClass="td_cell" Font-Size="10px" GridLines="Vertical" 
                                        tabIndex="12" Width="100%">
                                        <FooterStyle CssClass="grdfooter" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="ctrycode" Visible="False">
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="TextBox3" runat="server" CssClass="field_input" 
                                                        Text='<%# Bind("ctrycode") %>'></asp:TextBox>
                                                </EditItemTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblcntryCode" runat="server" __designer:wfdid="w26" 
                                                        Text='<%# Bind("ctrycode") %>'></asp:Label>
                            <br />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField Visible="False">
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="TextBox2" runat="server"></asp:TextBox>
                                                </EditItemTemplate>
                                                <HeaderTemplate>
                                                    <asp:CheckBox ID="chkSelectAll" runat="server" AutoPostBack="True" 
                                                        oncheckedchanged="chkSelectAll_CheckedChanged" Text="Select All" />
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="Label5" runat="server"></asp:Label>
                            <br />
                                                    <asp:CheckBox ID="chkSelect" runat="server" />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" />
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="ctrycode" HeaderText="Country Code">
                                                <ControlStyle Width="10%" />
                                                <HeaderStyle Width="10%" />
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="country" HeaderText="Country Name">
                                                <ControlStyle Width="20%" />
                                                <HeaderStyle HorizontalAlign="Left" />
                                                <ItemStyle Width="20%" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="region" HeaderText="Region">
                                                <ControlStyle Width="20%" />
                                                <HeaderStyle HorizontalAlign="Left" />
                                                <ItemStyle Width="20%" />
                                            </asp:BoundField>
                                         <asp:BoundField DataField="countrygroups" HeaderText="Country Group">
                                                <HeaderStyle HorizontalAlign="Left" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Holidays" HeaderText="Holiday Name">
                                                <HeaderStyle HorizontalAlign="Left" />
                                            </asp:BoundField>
                                        </Columns>
                                        <RowStyle CssClass="grdRowstyle" />
                                        <SelectedRowStyle CssClass="grdselectrowstyle" />
                                        <PagerStyle CssClass="grdpagerstyle" HorizontalAlign="Center" />
                                        <HeaderStyle CssClass="grdheader" ForeColor="white" />
                                        <AlternatingRowStyle CssClass="grdAternaterow" />
                                    </asp:GridView>
                                </ContentTemplate>
                            </asp:UpdatePanel>
        </TD>
    </TR>

    <TR>
    <TD class="td_cell" colspan="3">
        <asp:Label ID="lblMsg" runat="server" __designer:wfdid="w51" Font-Bold="True" 
            Font-Names="Verdana" Font-Size="8pt" ForeColor="#084573" 
            Text="Records not found. Please redefine search criteria" Visible="False" 
            Width="357px"></asp:Label>
    </TD>
    <td style="WIDTH: 279px">
    </td>
    </TR>
    <tr>
        <td class="style5" align="right">
            <asp:Button ID="btnSave" runat="server" CssClass="btn" tabIndex="8" 
                Text="Save" />
        </td>
        <td style="WIDTH: 37px">
            &nbsp;</td>
        <td style="WIDTH: 855px">
            <asp:Button ID="btnhelp" runat="server" __designer:wfdid="w4" CssClass="btn" 
                onclick="btnhelp_Click" tabIndex="10" Text="Help" />
            <asp:Button ID="btnCancel" runat="server" CssClass="btn" 
                onclick="btnCancel_Click" tabIndex="9" Text="Return To Search" Visible="False" 
                Width="196px" />
        </td>
        <td>
            <asp:Label ID="lblwebserviceerror" runat="server" style="display:none" 
                Text="Webserviceerror"></asp:Label>
        </td>
    </tr>
    <tr>
        <td class="style5">
            &nbsp;</td>
        <td style="WIDTH: 37px">
            &nbsp;</td>
        <td style="WIDTH: 855px">
            &nbsp;</td>
        <td>
            &nbsp;</td>
    </tr>
    </TBODY></TABLE>

<div ID="divwidth"></div>

 
<div ID="div1">
    <asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server">
        <services>
            <asp:ServiceReference Path="~/clsCountryGroupServices.asmx" />
        </services>
    </asp:ScriptManagerProxy>
            </div>

 <script type="text/javascript">
     function SetContextKey() {


     }
 </script>
   
</contenttemplate>


    </asp:UpdatePanel>

     </asp:Content>