<%@ Page Language="VB" MasterPageFile="~/SubPageMaster.master" AutoEventWireup="false" EnableEventValidation="false" CodeFile="ContractMain.aspx.vb" Inherits="ContractMain"  %>

<%@ Register Src="SubMenuUserControl.ascx" TagName="SubMenuUserControl" TagPrefix="uc1" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
    <%@ Register Src="Countrygroup.ascx"  TagName="Countrygroup" TagPrefix="uc2" %>

    <%@ OutputCache location="none" %> 
    <%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
      <%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
      <%@ Register Assembly="EclipseWebSolutions.DatePicker" Namespace="EclipseWebSolutions.DatePicker"
    TagPrefix="ews" %>
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
    
      <script src="../Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>

  <script type="text/javascript" charset="utf-8">
      $(document).ready(function () {


          AutoCompleteExtenderKeyUp();
       visualsearchbox();
          AutoCompleteExtenderUserControlKeyUp();


      });

      function fnClearTextBoxNeeded(keybr) { 
        if (keybr == 9 || keybr == 16 || keybr == 17 || keybr == 18 || keybr == 20 || keybr == 27 || keybr == 45 || keybr == 36 || keybr == 33 || keybr == 35 || keybr == 34 || keybr == 37 || keybr == 38 || keybr == 39 || keybr == 40 || keybr == 93 || keybr == 92 || keybr == 112 || keybr == 123 || keybr == 144 || keybr == 145 || keybr == 19 || keybr == 13) {
            return false;
        }
        else
        {
            return true;
        }
      }

      function AutoCompleteExtenderKeyUp() {
//          $("#< %= txthotelname.ClientID %>").bind("keyup", function (e) {
//              if (fnClearTextBoxNeeded(e.keyCode) == true)
//              {
//              document.getElementById('< %=txthotelcode.ClientID%>').value = '';
//              }
//              //var keybr = e.keyCode; //e.which || e.keyCode;             
//              //if (keybr == 9 || keybr == 16 || keybr == 17 || keybr == 18 || keybr == 20 || keybr == 27 || keybr == 45 || keybr == 36 || keybr == 33 || keybr == 35 || keybr == 34 || keybr == 37 || keybr == 38 || keybr == 39 || keybr == 40 || keybr == 93 || keybr == 92 || keybr == 112 || keybr == 123 || keybr == 144 || keybr == 145 || keybr == 19 || keybr == 13) {
//              //    //text
//              //}
//              //else {
//              //    $("#< %= txthotelcode.ClientID %>").val('');
//              //}
//              //document.getElementById(' < %=txthotelcode.ClientID%>').value = '';
//      });

      $("#<%= txthotelname.ClientID %>").bind("change", function () {
          document.getElementById('<%=txthotelcode.ClientID%>').value = '';
      });

      }

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
          $txtvsprocess.val('CountryGroup:"          " Region:"          " Country:"         " Text:"          "');

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
                          case 'CountryGroup':
                              var asSqlqry = 'select ltrim(rtrim(countrygroupname)) countrygroupname  from countrygroup where active=1';
                              glcallback = callback;
                              //fnTestCallback();
                              ColServices.clsServices.GetListOfArrayVisualSearch(asSqlqry, fnFillSearchVS, ErrorHandler, TimeOutHandler);
                              break;
                          case 'Region':
                              var asSqlqry = 'select ltrim(rtrim(plgrpname)) plgrpname  from plgrpmast where active=1';
                              glcallback = callback;
                              //fnTestCallback();
                              ColServices.clsServices.GetListOfArrayVisualSearch(asSqlqry, fnFillSearchVS, ErrorHandler, TimeOutHandler);
                              break;
                          case 'Country':
                              var asSqlqry = 'select distinct ltrim(rtrim(ctryname)) from ctrymast where active=1';
                              glcallback = callback;
                              //fnTestCallback();
                              ColServices.clsServices.GetListOfArrayVisualSearch(asSqlqry, fnFillSearchVS, ErrorHandler, TimeOutHandler);
                              break;
                      }
                  },
                  facetMatches: function (callback) {
                      callback([
                { label: 'Text' },
                { label: 'CountryGroup', category: 'location' },
                { label: 'Region', category: 'location' },
                { label: 'Country', category: 'location' },
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
    prm.add_initializeRequest(InitializeRequestUserControl);
    prm.add_endRequest(EndRequestUserControl);

    function InitializeRequestUserControl(sender, args) {

    }

    function EndRequestUserControl(sender, args) {
        AutoCompleteExtenderKeyUp();
        // after update occur on UpdatePanel re-init the Autocomplete
        visualsearchbox();
        AutoCompleteExtenderUserControlKeyUp();
    }
</script>

<script language="javascript" type="text/javascript" >

    function SeasonDateSelectCalExt() {

        var grid = document.getElementById("<%=gvSeasonInput.ClientID%>");
        var inputs = grid.getElementsByTagName("input");
        var txtToDate, txtfromDate;
        var calendarBehavior1;
        var calendarBehavior2;
        for (var i = 0; i < inputs.length; i++) {
            if (inputs[i].type == "text") {
                //   alert(inputs[i].name)
//                if (inputs[i].name == "ctl00$Main$gvSeasonInput$ctl01$txtSeasonToDate" || inputs[i].id == "ctl00$Main$gvSeasonInput$ctl01$txtSeasonToDate") {
//                    txtToDate = inputs[i];

//                }
//                if (inputs[i].name == "ctl00$Main$gvSeasonInput$ctl01$txtSeasonFromDate" || inputs[i].id == "ctl00$Main$gvSeasonInput$ctl01$txtSeasonFromDate") {
//                    txtfromDate = inputs[i];

//                }
//                //txtSeasonfromDate_CalendarExtender
//                if (inputs[i].name == "ctl00$Main$gvSeasonInput$ctl01$txtSeasonfromDate_CalendarExtender" || inputs[i].id == "ctl00$Main$gvSeasonInput$ctl01$txtSeasonfromDate_CalendarExtender") {
//                    calendarBehavior1 = inputs[i];

//                }
//                if (inputs[i].name == "ctl00$Main$gvSeasonInput$ctl01$txtSeasonToDate_CalendarExtender" || inputs[i].id == "ctl00$Main$ggvSeasonInput$ctl01$txtSeasonToDate_CalendarExtender") {
//                    calendarBehavior2 = inputs[i];

                //                }

                if (inputs[i].name.indexOf("$txtSeasonfromDate") >= 0 || inputs[i].id.indexOf("$txtSeasonfromDate") >= 0) {
                    txtfromDate = document.getElementById(inputs[i].id); // inputs[i];
                }

                if (inputs[i].name.indexOf("$txtSeasonToDate") >= 0 || inputs[i].id.indexOf("$txtSeasonToDate") >= 0) {
                    txtToDate = document.getElementById(inputs[i].id); // inputs[i];
                }

                if (inputs[i].name.indexOf("$txtSeasonfromDate_CalendarExtender") >= 0 || inputs[i].id.indexOf("$txtSeasonfromDate_CalendarExtender") >= 0) {
                    calendarBehavior1 = inputs[i];
                }
                if (inputs[i].name.indexOf("$txtSeasonToDate_CalendarExtender") >= 0 || inputs[i].id.indexOf("$txtSeasonToDate_CalendarExtender") >= 0) {
                    calendarBehavior2 = inputs[i];
                }
            }
        }
        // var calendarBehavior1 = $find("<%=dpFromDate.ClientID %>");  // document.getElementById("<%=dpFromDate.ClientID%>"); 
        var date = calendarBehavior1._selectedDate;
        var txtfromDate = document.getElementById("<%=txtfromDate.ClientID%>");
        var dp = txtfromDate.value.split("/");
        var newDt = new Date(dp[2] + "/" + dp[1] + "/" + dp[0]);
        newDt = getFormatedDate(newDt);
        newDt = new Date(newDt);
        calendarBehavior1.set_selectedDate(newDt);

        var calendarBehavior2 = $find("<%=dpToDate.ClientID %>");  // document.getElementById("<%=dpFromDate.ClientID%>"); 
        var date2 = calendarBehavior2._selectedDate;
        var dp2 = txtToDate.value.split("/");
        var newDt2 = new Date(dp2[2] + "/" + dp2[1] + "/" + dp2[0]);
        newDt2 = getFormatedDate(newDt2);
        newDt2 = new Date(newDt2);
        calendarBehavior2.set_selectedDate(newDt2);
    }

    function fillSeasontodate(txtfromDateId, txtToDateID) {
        

        var txtfromDate = document.getElementById(txtfromDateId);
        var txtToDate = document.getElementById(txtToDateID);
        var curDate = document.getElementById("<%=hdCurrentDate.ClientID%>");

        if (txtfromDate.value != null) {
            var dp = txtfromDate.value.split("/");

            var newDt = new Date(dp[2] + "/" + dp[1] + "/" + dp[0]);
            var today = new Date();

            newDt = getFormatedDate(newDt);
            today = getFormatedDate(today);

            newDt = new Date(newDt);
            today = new Date(today);
            
            if (newDt < today) {
                alert('From date should not be less than todays date.');
                txtfromDate.value = curDate.value;
                txtToDate.value = txtfromDate.value;
                SeasonDateSelectCalExt();
                return;
            }
            else {
                if (txtToDate.value != "" ) {

                    var dpTo = txtToDate.value.split("/");
                    var newDtTo = new Date(dpTo[2] + "/" + dpTo[1] + "/" + dpTo[0]);
                    var today = new Date();

                    newDtTo = getFormatedDate(newDtTo);
                    today = getFormatedDate(today);

                    newDtTo = new Date(newDtTo);
                    today = new Date(today);

                    if (newDt > newDtTo) {
                        txtToDate.value = txtfromDate.value;
                        SeasonDateSelectCalExt();
                        return;
                    }

                }
                else {
                    txtToDate.value = txtfromDate.value;
                    SeasonDateSelectCalExt();
                    return;
                }
            }
        }

    }

    function ValidateSeasonChkInDate(txtfromDateId, txtToDateID) {
        
        var Fromdate = document.getElementById(txtfromDateId);
        var txtToDate = document.getElementById(txtToDateID);

        if (txtToDate.value != "") {

            if (Fromdate.value == "") {
                alert("Please select From date.");
                txtToDate.value = "";
                return false;
            }

            var dp = Fromdate.value.split("/");
            var newChkInDt = new Date(dp[2] + "/" + dp[1] + "/" + dp[0]);

            var dp1 = txtToDate.value.split("/");
            var newChkOutDt = new Date(dp1[2] + "/" + dp1[1] + "/" + dp1[0]);

            newChkInDt = getFormatedDate(newChkInDt);
            newChkOutDt = getFormatedDate(newChkOutDt);
            
            newChkInDt = new Date(newChkInDt);
            newChkOutDt = new Date(newChkOutDt);

            if (newChkInDt > newChkOutDt) {
                txtToDate.value = Fromdate.value;
                alert("Todate date should not be greater than From date");
                SeasonDateSelectCalExt();
                return false;              
            }
        }
    }

    function DateSelectCalExt() {
        var calendarBehavior1 = $find("<%=dpFromDate.ClientID %>");  // document.getElementById("<%=dpFromDate.ClientID%>"); 
        var date = calendarBehavior1._selectedDate;
        var txtfromDate = document.getElementById("<%=txtfromDate.ClientID%>");
        var dp = txtfromDate.value.split("/");
        var newDt = new Date(dp[2] + "/" + dp[1] + "/" + dp[0]);
        newDt = getFormatedDate(newDt);
        newDt = new Date(newDt);
        calendarBehavior1.set_selectedDate(newDt);

        var calendarBehavior2 = $find("<%=dpToDate.ClientID %>");  // document.getElementById("<%=dpFromDate.ClientID%>"); 
        var date2 = calendarBehavior2._selectedDate;
        var txtfromDate2 = document.getElementById("<%=txtToDate.ClientID%>");
        var dp2 = txtfromDate2.value.split("/");
        var newDt2 = new Date(dp2[2] + "/" + dp2[1] + "/" + dp2[0]);
        newDt2 = getFormatedDate(newDt2);
        newDt2 = new Date(newDt2);
        calendarBehavior2.set_selectedDate(newDt2);
    }
    
    function validateDigitOnly(evt) {
        var theEvent = evt || window.event;
        var key = theEvent.keyCode || theEvent.which;
        key = String.fromCharCode(key);
        var regex = /[0-9]/;
        if (!regex.test(key)) {
            theEvent.returnValue = false;
            if (theEvent.preventDefault) theEvent.preventDefault();
        }
    }

    function FormValidationMainDetail(state) {
//        var txtnameval = document.getElementById("< %=txthotelname.ClientID%>");
//        if (txtnameval.value == '') {
//            alert('Name Cannot be blank');
//            return false;
//        }
//        else {
//            if (state == 'New') { if (confirm('Are you sure you want to save please check supplier type') == false) return false; }
//            if (state == 'Edit') { if (confirm('Are you sure you want to update?') == false) return false; }
//            if (state == 'Delete') { if (confirm('Are you sure you want to delete?') == false) return false; }
//        }
    }


    //    added by sribish
    function formmodecheck() {
        var vartxtcode = document.getElementById("<%=txtcontractid.ClientID%>");
      //  var vartxtname = document.getElementById("<%=txtname.ClientID%>");
        
        if ((vartxtcode.value == '') ) {
            doLinks(false);
        }
        else {
            doLinks(true);
        }

    }

    function doLinks(how) {
        var strName = '';
        for (var l = document.links, i = l.length - 1; i > -1; --i) {
            strName = l[i].outerText;

         

            if (strName != 'Max Occupancy') {
                if (!how)
                    l[i].onclick = function () { alert('Please Save Main details to continue'); return false; };
                else
                    l[i].onclick = function () { return true; };
            }
        }
    }
    function load() {
       
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(formmodecheck);
    }


    function checkfromdates(txtfromdate, txtodate) {

        var fdate = document.getElementById(txtfromdate);
        var tdate = document.getElementById(txtodate);


        if (fdate.value == null || fdate.value == "") {
            alert("Please select from date.");
        }

        if (tdate.value == null || tdate.value == "") {
            tdate.value = fdate.value;
        }

        var dp = fdate.value.split("/");

        var newfdate = new Date(dp[2] + "/" + dp[1] + "/" + dp[0]);



        var dp1 = tdate.value.split("/");
        var newtdate = new Date(dp1[2] + "/" + dp1[1] + "/" + dp1[0]);

        newfdate = getFormatedDate(newfdate);
        newtdate = getFormatedDate(newtdate);

        newfdate = new Date(newfdate);
        newtdate = new Date(newtdate);



        if (newfdate > newtdate) {
            tdate.value = "";
            alert("From date should not be greater than To date");
        }


        setdate();

    }

    
    function filltodate(fDate) {
        var txtfromDate = document.getElementById("<%=txtfromDate.ClientID%>");
        var txtToDate = document.getElementById("<%=txtToDate.ClientID%>");
        var curDate = document.getElementById("<%=hdCurrentDate.ClientID%>");

        if (txtfromDate.value != null) {
            var dp = txtfromDate.value.split("/");

            var newDt = new Date(dp[2] + "/" + dp[1] + "/" + dp[0]);
            var today = new Date();

            newDt = getFormatedDate(newDt);
            today = getFormatedDate(today);

            newDt = new Date(newDt);
            today = new Date(today);

            if (newDt < today) {

                alert('From date should not be less than todays date.');
                txtfromDate.value = curDate.value;
                return;
            }
            else {
                if (txtToDate.value != null) {
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
            }

        }

    }


    function filleffectivetodate(fDate) {
        var txtefffromDate = document.getElementById("<%=txtefffromdate.ClientID%>");
        var txteffToDate = document.getElementById("<%=txtefftodate.ClientID%>");
        var curDate = document.getElementById("<%=hdCurrentDate.ClientID%>");

        if (txtefffromDate.value != null) {
            var dp = txtefffromDate.value.split("/");

            var newDt = new Date(dp[2] + "/" + dp[1] + "/" + dp[0]);
            var today = new Date();

            newDt = getFormatedDate(newDt);
            today = getFormatedDate(today);

            newDt = new Date(newDt);
            today = new Date(today);

            if (newDt < today) {

                alert('From date should not be less than todays date.');
                txtefffromDate.value = curDate.value;
                return;
            }
            else {
                if (txteffToDate.value != null) {
                    var dpTo = txteffToDate.value.split("/");

                    var newDtTo = new Date(dpTo[2] + "/" + dpTo[1] + "/" + dpTo[0]);
                    var today = new Date();

                    newDtTo = getFormatedDate(newDtTo);
                    today = getFormatedDate(today);

                    newDtTo = new Date(newDtTo);
                    today = new Date(today);

                    if (newDt > newDtTo) {

                        txteffToDate.value = txtefffromDate.value;
                        DateSelectCalExt();
                        return;
                    }
                    //}
                }
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


    function ValidateEffToDate() {

        var txtefffromdate = document.getElementById("<%=txtefffromdate.ClientID%>");
        var txtefftodate = document.getElementById("<%=txtefftodate.ClientID%>");
        if (txtefffromdate.value == null || txtefffromdate.value == "") {
            txtefftodate.value = "";
            alert("Please select From date.");
        }

        var dp = txtefffromdate.value.split("/");
        var newChkInDt = new Date(dp[2] + "/" + dp[1] + "/" + dp[0]);

        var dp1 = txtefftodate.value.split("/");
        var newChkOutDt = new Date(dp1[2] + "/" + dp1[1] + "/" + dp1[0]);

        newChkInDt = getFormatedDate(newChkInDt);
        newChkOutDt = getFormatedDate(newChkOutDt);

        newChkInDt = new Date(newChkInDt);
        newChkOutDt = new Date(newChkOutDt);
        if (newChkInDt > newChkOutDt) {
            txtefftodate.value = txtefffromdate.value;
          
            txtefftodate.focus();
            alert("Todate date should not be greater than From date");
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

    function getCheckoutdt(checkindate, checkoutdate) {

        var txtToDate = document.getElementById("<%=txtToDate.ClientID%>");
        var numberOfDaysToAdd = 0;
        var dp = checkindate.value.split("/");
        var newDt = new Date(dp[2] + "/" + dp[1] + "/" + dp[0]);
        newDt.setDate(newDt.getDate() + numberOfDaysToAdd);
        var dd = newDt.getDate();
        if (dd < 10) {
            dd = "0" + dd;
        }
        var mm = newDt.getMonth() + 1;
        if (mm < 10) {
            mm = "0" + mm;
        }
        var y = newDt.getFullYear();
        var someFormattedDate = dd + '/' + mm + '/' + y;

        checkoutdate.value = someFormattedDate;
        txtToDate.value = checkoutdate.value
    }


    function hotelautocompleteselected(source, eventArgs) {
        if (eventArgs != null) {
            document.getElementById('<%=txthotelcode.ClientID%>').value = eventArgs.get_value();
        }
        else {
            document.getElementById('<%=txthotelcode.ClientID%>').value = '';
        }
    }

    function supagentautocompleteselected(source, eventArgs) {
        if (eventArgs != null) {
            document.getElementById('<%=txtsupagentcode.ClientID%>').value = eventArgs.get_value();
        }
        else {
            document.getElementById('<%=txtsupagentcode.ClientID%>').value = '';
        }
    }

    function hotelautocompleteremove(){
        document.getElementById('<%=txthotelcode.ClientID%>').value = '';
    }


    function Enabletax(chkcomm,formula, txtservice, txttax, txtcomm) {

        var txtservice = document.getElementById(txtservice);
        var txttax = document.getElementById(txttax);
        var txtcomm = document.getElementById(txtcomm);
        var chkcomm1 = document.getElementById(chkcomm);
        var str = formula;
        var str1 = ' ' + formula + ' ';
        str1 = str1.replace('+', ' ');
        str1 = str1.replace('-', ' ');
        str1 = str1.replace('*', ' ');
        str1 = str1.replace('/', ' ');
        str1 = str1.replace('=', ' ');
        str1 = str1.replace('|', ' ');
        str1 = str1.replace('=', ' ');
        str1 = str1.replace('-', ' ');

        $get("ctl00_Main_btngAlert").click();
        
//        if (chkcomm1.checked) {
//            //alert(str.indexOf('SR'));

//            if (str1.indexOf(' SR ') > -1 && str1.indexOf(' T ') > -1 && str1.indexOf(' C ') > -1) {
//                txtservice.disabled = false;
//                txttax.disabled = false;
//                txtcomm.disabled = false;
//            }
//            else if (str1.indexOf(' SR ') > -1 && str1.indexOf(' T ') > -1) {
//                txtservice.disabled = false;
//                txttax.disabled = false;
//                txtcomm.disabled = true;
//                txtcomm.value = '';

//            }
//            else if (str1.indexOf(' SR ') > -1 && str1.indexOf(' C ') > -1) {
//                txtservice.disabled = false;
//                txtcomm.disabled = false;
//                txttax.disabled = true;
//                txttax.value = '';

//            }
//            else if (str1.indexOf(' T ') > -1 && str1.indexOf(' C ') > -1) {
//                txttax.disabled = false;
//                txtcomm.disabled = false;
//                txtservice.disabled = true;
//                txtservice.value = '';

//            }
//            else if (str1.indexOf(' C ') > -1) {
//            
//                txttax.disabled = true;
//                txtcomm.disabled = false;
//                txtservice.disabled = true;
//                txtservice.value = '';
//                txttax.value = '';

//            }

//            else {
//                txtservice.disabled = true;
//                txttax.disabled = true;
//                txtcomm.disabled = true;
//               

//            }
//        }
//        else {
//            txtservice.disabled = true;
//            txttax.disabled = true;
//            txtcomm.disabled = true;
//          
//        }
    }

    function checkNumber(evt) {
        evt = (evt) ? evt : event;
        var charCode = (evt.charCode) ? evt.charCode : ((evt.keyCode) ? evt.keyCode :
        ((evt.which) ? evt.which : 0));
        if (charCode != 47 && (charCode > 44 && charCode < 58)) {
            return true;
        }
        return false;
    }

    function showcommission(chkcommission) {
        var chkcommissionid = document.getElementById(chkcommission);
        if (chkcommissionid.checked) {
            $("#" + "<%=divcommision.ClientID %>").slideDown();
            $("#" + "<%=divterms.ClientID %>").slideDown();
        }
        else {
            $("#" + "<%=divcommision.ClientID %>").fadeOut();
            $("#" + "<%=divterms.ClientID %>").fadeOut();
        }
    }

  


    function RadioCheck(rb) {
        var gv = document.getElementById("<%=grdcommission.ClientID%>");
        var rbs = gv.getElementsByTagName("input");

        var row = rb.parentNode.parentNode;
        for (var i = 0; i < rbs.length; i++) {
            if (rbs[i].type == "radio") {
                if (rbs[i].checked && rbs[i] != rb) {
                    rbs[i].checked = false;
                    
                    break;
                }
            }
        }
    }


    function fillgrid(chkseason) {
       // alert('test');
       // $get("ctl00_Main_btnshowcomm").click();

    }

    function alpha(e) {
        var k;
        document.all ? k = e.keyCode : k = e.which;
        return ((k > 64 && k < 91) || (k > 96 && k < 123) || k == 8 || k == 32 || (k >= 48 && k <= 57));
    }
    function confirmapplicable(txtMsg, btnfinal) {

        var x = confirm(txtMsg);
        var btnid = document.getElementById(btnfinal);
        var btnhidden = document.getElementById("<%=btnhidden.ClientID%>");
        if (x) {
            btnid.click();

        }
        else {

            btnhidden.click();
        }

    }


</script>



    <asp:UpdatePanel id="UpdatePanel2" runat="server">
        <contenttemplate>


            <table style="width: 100%; height: 100%; border-right: gray 2px solid; border-top: gray 2px solid;
                border-left: gray 2px solid; border-bottom: gray 2px solid" class="td_cell" align="left">
                <tbody>
                    <tr>
                        <td valign="top" align="center" width="150" colspan="6">
                            <asp:Label ID="lblHeading" runat="server" Text="Contracts" CssClass="field_heading"
                                Width="100%" ForeColor="White"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" valign="top" width="50" rowspan="2">
                            <div id="menudiv" style="height: 402px">
                                <uc1:SubMenuUserControl ID="SubMenuUserControl1" runat="server" />
                            </div>
                        </td>
                        <td align="left" class="td_cell" valign="top" colspan="10">
                            <table style="width: 100%">
                                <tr>
                                    <td width="150px">
                                        &nbsp;
                                    </td>
                                    <td style="width: 203px">
                                        &nbsp;
                                    </td>
                                    <td width="150px">
                                        &nbsp;
                                    </td>
                                    <td width="150px">
                                        &nbsp;
                                    </td>
                                    <td width="150px">
                                        &nbsp;
                                    </td>
                                    <td width="150px">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblHotelName" runat="server" Text="Hotel Name" Width="99px"></asp:Label>
                                    </td>
                                    <td colspan="2">
                                        <%--</td>
                    <td align="left" valign="top" colspan="2" width="300px">--%>
                                        <asp:TextBox ID="txthotelname" runat="server" AutoPostBack="True" CssClass="field_input"
                                            MaxLength="500" TabIndex="3" Width="250px"></asp:TextBox>
                                        <asp:TextBox ID="txthotelcode" runat="server" Style="display: none"></asp:TextBox>
                                        <asp:AutoCompleteExtender ID="txthotelname_AutoCompleteExtender" runat="server" CompletionInterval="10"
                                            CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                            CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="1" DelimiterCharacters=""
                                            EnableCaching="false" Enabled="True" FirstRowSelected="True" MinimumPrefixLength="1"
                                            ServiceMethod="Gethotelslist" TargetControlID="txthotelname" OnClientItemSelected="hotelautocompleteselected">
                                        </asp:AutoCompleteExtender>
                                        <input style="display: none" id="txtCode" class="field_input" type="text" runat="server" />
                                        <input style="display: none" id="txtName" class="field_input" type="text" runat="server" />
                                    </td>
                                    <td width="170px">
                                        <asp:Label ID="lbl4" runat="server" Text="Supplier Agent" Width="121px"></asp:Label>
                                        <asp:TextBox ID="txtsupagentcode" runat="server" Style="display: none"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtsupagentname" runat="server" AutoPostBack="True" CssClass="field_input"
                                            MaxLength="500" TabIndex="3" Width="158px"></asp:TextBox>
                                        <asp:AutoCompleteExtender ID="txtsupagentname_AutoCompleteExtender" runat="server"
                                            CompletionInterval="10" CompletionListCssClass="autocomplete_completionListElement"
                                            CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem" CompletionListItemCssClass="autocomplete_listItem"
                                            CompletionSetCount="1" DelimiterCharacters="" EnableCaching="false" Enabled="True"
                                            FirstRowSelected="True" MinimumPrefixLength="0" OnClientItemSelected="supagentautocompleteselected"
                                            ServiceMethod="Getsupagentlist" TargetControlID="txtsupagentname">
                                        </asp:AutoCompleteExtender>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label1" runat="server" Style="vertical-align: top;" Text="Contract ID"
                                            Width="90px"></asp:Label>
                                    </td>
                                    <td width="300px">
                                        <asp:TextBox ID="txtcontractid" ReadOnly="true" runat="server" Width="250px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label2" runat="server" Style="vertical-align: top;" Text="From Date"
                                            Width="90px"></asp:Label>
                                    </td>
                                    <td align="left" colspan="2">
                                        <%--</td>
                        <td style="width: 150px">--%>
                                        <asp:TextBox ID="txtfromDate" runat="server" CssClass="fiel_input" Width="80px" TabIndex="4"
                                            onchange="filltodate(this);"></asp:TextBox>
                                        <cc1:CalendarExtender ID="dpFromDate" runat="server" Format="dd/MM/yyyy" PopupButtonID="ImgBtnFrmDt"
                                            OnClientDateSelectionChanged="DateSelectCalExt" PopupPosition="Right" TargetControlID="txtfromDate">
                                        </cc1:CalendarExtender>
                                        <cc1:MaskedEditExtender ID="MeFromDate" runat="server" Mask="99/99/9999" MaskType="Date"
                                            TargetControlID="txtfromDate">
                                        </cc1:MaskedEditExtender>
                                        <asp:ImageButton ID="ImgBtnFrmDt" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.png"
                                            TabIndex="-1" />
                                        <cc1:MaskedEditValidator ID="MevFromDate" runat="server" ControlExtender="MeFromDate"
                                            ControlToValidate="txtfromDate" CssClass="field_error" Display="Dynamic" EmptyValueBlurredText="Date is required"
                                            EmptyValueMessage="Date is required" ErrorMessage="MeFromDate" InvalidValueBlurredMessage="Invalid Date"
                                            InvalidValueMessage="Invalid Date" TooltipMessage="Input a Date in Date/Month/Year">
                                        </cc1:MaskedEditValidator>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label3" runat="server" Style="vertical-align: top;" Text="To Date"
                                            Width="90px"></asp:Label>
                                    </td>
                                    <td style="width: 300px" align="left" colspan="2">
                                        <%--</td>
                        <td style="width: 150px">--%>
                                        <asp:TextBox ID="txtToDate" runat="server" CssClass="fiel_input" Width="80px" TabIndex="5"
                                            onchange="ValidateChkInDate();">
                                        </asp:TextBox>
                                        <cc1:CalendarExtender ID="dpToDate" runat="server" Format="dd/MM/yyyy" PopupButtonID="ImgBtnToDt"
                                            OnClientDateSelectionChanged="DateSelectCalExt" PopupPosition="Right" TargetControlID="txtToDate">
                                        </cc1:CalendarExtender>
                                        <cc1:MaskedEditExtender ID="MeToDate" runat="server" Mask="99/99/9999" MaskType="Date"
                                            TargetControlID="txtToDate">
                                        </cc1:MaskedEditExtender>
                                        <asp:ImageButton ID="ImgBtnToDt" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.png"
                                            TabIndex="-1" />
                                        <cc1:MaskedEditValidator ID="MevToDate" runat="server" ControlExtender="MeToDate"
                                            ControlToValidate="txtToDate" CssClass="field_error" Display="Dynamic" EmptyValueBlurredText="Date is required"
                                            EmptyValueMessage="Date is required" ErrorMessage="MeToDate" InvalidValueBlurredMessage="Invalid Date"
                                            InvalidValueMessage="Invalid Date" TooltipMessage="Input a Date in Date/Month/Year">
                                        </cc1:MaskedEditValidator>
                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="lblApplicableTo0" runat="server" Style="vertical-align: top;"
                                            Text="Applicable To" Width="90px"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtApplicableTo" runat="server" Rows="2" Style="margin: 0px; height: 48px;
                                            width: 250px" TabIndex="6" TextMode="MultiLine"></asp:TextBox>
                                    </td>
                                   
                                </tr>
                                <tr>
                                    <td>
                                  <asp:Label ID="lblstatustext" runat="server" Style="vertical-align: top;" 
                                            Text="Status:" Width="43px"></asp:Label>
                                    </td>
                                    <td >
                                        <asp:Label ID="lblstatus" runat="server" Font-Bold="True" ForeColor="#3366FF" 
                                            Style="vertical-align: top;" Text="Status" Width="43px"></asp:Label>
                                    </td>
                
                                    <td >
                                      <asp:CheckBox ID="chkwithdraw" runat="server" class="cls_chkcomm"  Text="With Draw" TabIndex="8" />
                                    </td>
                   
                                    <td>  <asp:Label ID="Label4" runat="server" Style="vertical-align: top;" Text="Title Name"
                                            Width="90px"></asp:Label></td>
                                    <td >
                                        <asp:TextBox ID="txttitle"  runat="server" Width="250px">
                                        </asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 100px" valign="top"   colspan="10">
                            <div style="width: 100%; min-height: 400px" id="iframeINF" runat="server">
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                    <ContentTemplate>
                                        <div class="container" id="VS">
                                            <div id="search_box_container">
                                            </div>
                                        </div>
                                        <br />
                                        <asp:DataList ID="dlList" runat="server" RepeatColumns="4" RepeatDirection="Horizontal">
                                            <ItemTemplate>
                                                <table class="styleDatalist">
                                                    <tr>
                                                        <td>
                                                            <asp:Button ID="lnkCode" class="button button4" runat="server" Text='<%# Eval("Code") %>'
                                                                Style="display: none" />
                                                            <asp:Button ID="lnkValue" class="button button4" runat="server" Text='<%# Eval("Value") %>'
                                                                Style="display: none" />
                                                            <asp:Button ID="lnkCodeAndValue" class="button button4" runat="server" Text='<%# Eval("CodeAndValue") %>'
                                                                 OnClick="lnkCodeAndValue_Click" />
                                                            <asp:Button ID="lbClose" class="buttonClose button4" runat="server" OnClick="lbClose_Click"
                                                                Text="X" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ItemTemplate>
                                        </asp:DataList>
                                        <div style="display: none">
                                            <div id="search_query" runat="server" class="search_query">
                                                &nbsp;</div>
                                            <asp:TextBox ID="txtvsprocess" runat="server" class="cs_txtvsprocess" Style="display: none"></asp:TextBox>
                                            <asp:TextBox ID="txtvsprocesssplit" runat="server" class="cs_txtvsprocesssplit" Style="display: none"></asp:TextBox>
                                            <%--<asp:textbox id="txtvsprocessCity" runat="server"></asp:textbox>
                                <asp:textbox id="txtvsprocessCountry" runat="server"></asp:textbox>--%>
                                            <asp:Button ID="btnvsprocess" runat="server" Style="display: none" />
                                        </div>
                                        <div id="countrygroup1" style="float: left; margin-left: 40px; width: 90%">
                                            <uc2:Countrygroup ID="wucCountrygroup" runat="server" />
                                        </div>
                                        <script language="javascript">
                                            formmodecheck();
                                            load();
                                        </script>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" valign="top" width="150">
                            &nbsp;
                        </td>
                        <td style="width: 900px" valign="top" colspan="3">
                            <asp:UpdatePanel ID="upnlSeasons" runat="server">
                                <ContentTemplate>
                                    <table width="925px">
                                        <tr>
                                           
                                            <td class="field_heading" colspan="4">
                                                <asp:Label ID="Label5" runat="server" Text="Seasons Details"></asp:Label>
                                            </td>
                                        </tr>
                                      
                                        <tr>
                                            <td colspan="4">
                                                <%-- Input Grid --%>
                                                <asp:GridView ID="gvSeasonInput" AllowPaging="false" AllowSorting="false" runat="server"
                                                    AutoGenerateColumns="false" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None"
                                                    BorderWidth="1px" CellPadding="3">
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="Season Name">
                                                            <HeaderTemplate>
                                                                <asp:Label ID="Label6" runat="server" Text="Season Name"></asp:Label>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtSeasonName" runat="server" Width="250px" style="text-transform:uppercase;" onkeypress="return alpha(event)" onpaste="return false"></asp:TextBox>
                                                                <asp:AutoCompleteExtender ID="txtSeasonName_AutoCompleteExtender" runat="server"
                                                                    DelimiterCharacters="" Enabled="True" ServicePath="" CompletionInterval="10"
                                                                    CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                                    CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="1" EnableCaching="false"
                                                                    FirstRowSelected="True" MinimumPrefixLength="1" ServiceMethod="GetSeasonlist"
                                                                    TargetControlID="txtSeasonName">
                                                                </asp:AutoCompleteExtender>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="From Date">
                                                            <HeaderTemplate>
                                                                <asp:Label ID="Label7" runat="server" Text="From Date"></asp:Label>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtSeasonfromDate" runat="server" CssClass="fiel_input" Width="80px"></asp:TextBox>
                                                                <cc1:CalendarExtender ID="txtSeasonfromDate_CalendarExtender" runat="server" Format="dd/MM/yyyy"
                                                                    PopupButtonID="ImgSeasonBtnFrmDt" PopupPosition="Right" TargetControlID="txtSeasonfromDate"
                                                                    OnClientDateSelectionChanged="SeasonDateSelectCalExt">
                                                                </cc1:CalendarExtender>
                                                                <cc1:MaskedEditExtender ID="txtSeasonFromDate_MaskedEditExtender" runat="server"
                                                                    Mask="99/99/9999" MaskType="Date" TargetControlID="txtSeasonfromDate">
                                                                </cc1:MaskedEditExtender>
                                                                <asp:ImageButton ID="ImgSeasonBtnFrmDt" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.png" TabIndex ="-1" />
                                                                <cc1:MaskedEditValidator ID="MevSeasonFromDate" runat="server" ControlExtender="txtSeasonFromDate_MaskedEditExtender"
                                                                    ControlToValidate="txtSeasonfromDate" CssClass="field_error" Display="Dynamic"
                                                                    EmptyValueBlurredText="Date is required" EmptyValueMessage="Date is required"
                                                                    ErrorMessage="txtSeasonFromDate_MaskedEditExtender" InvalidValueBlurredMessage="Invalid Date"
                                                                    InvalidValueMessage="Invalid Date" TooltipMessage="Input a Date in Date/Month/Year">
                                                                </cc1:MaskedEditValidator>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="To Date">
                                                            <HeaderTemplate>
                                                                <asp:Label ID="Label8" runat="server" Text="To Date"></asp:Label>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtSeasonToDate" runat="server" CssClass="fiel_input" Width="80px"></asp:TextBox>
                                                                <cc1:CalendarExtender ID="txtSeasonToDate_CalendarExtender" runat="server" Format="dd/MM/yyyy"
                                                                    PopupButtonID="ImgSeasonBtnTomDt" PopupPosition="Right" TargetControlID="txtSeasonToDate"
                                                                    OnClientDateSelectionChanged="SeasonDateSelectCalExt">
                                                                </cc1:CalendarExtender>
                                                                <cc1:MaskedEditExtender ID="txtSeasonToDate_MaskedEditExtender" runat="server" Mask="99/99/9999"
                                                                    MaskType="Date" TargetControlID="txtSeasonToDate">
                                                                </cc1:MaskedEditExtender>
                                                                <asp:ImageButton ID="ImgSeasonBtnTomDt" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.png" TabIndex ="-1" />
                                                                <cc1:MaskedEditValidator ID="MevSeasontoDate" runat="server" ControlExtender="txtSeasonToDate_MaskedEditExtender"
                                                                    ControlToValidate="txtSeasonToDate" CssClass="field_error" Display="Dynamic"
                                                                    EmptyValueBlurredText="Date is required" EmptyValueMessage="Date is required"
                                                                    ErrorMessage="txtSeasonToDate_MaskedEditExtender" InvalidValueBlurredMessage="Invalid Date"
                                                                    InvalidValueMessage="Invalid Date" TooltipMessage="Input a Date in Date/Month/Year">
                                                                </cc1:MaskedEditValidator>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Min Nights" Visible ="false">
                                                            <HeaderTemplate>
                                                                <asp:Label ID="Label9" runat="server" Text="Min Nights"></asp:Label>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtMinNight" runat="server" onkeypress='validateDigitOnly(event)'
                                                                    Width="70px"></asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Action">
                                                            <ItemTemplate>
                                                                <asp:Button ID="btnAddRowGvS" runat="server" OnClick="btnAddRowGVs_Click" Text="Add Row" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Action">
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="imgSclose" runat="server" ImageUrl="~/Images/crystaltoolbar/DeleteRed.png"
                                                                    OnClick="imgSclose_Click" Width="25px" ToolTip="Delete Current Row" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <HeaderTemplate>
                                                                <asp:ImageButton ID="imgUpdateToNextGrid" runat="server" ImageUrl="~/Images/crystaltoolbar/Saveicon9.jpg"
                                                                    Width="25px" OnClick="imgUpdateToNextGrid_Click" ToolTip="Save" />
                                                            </HeaderTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <FooterStyle BackColor="White" ForeColor="#000066" />
                                                    <HeaderStyle BackColor="#2D7C8A" Font-Bold="True" ForeColor="White" HorizontalAlign="Left" />
                                                    <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                                                    <RowStyle ForeColor="#000066" />
                                                    <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                                                    <SortedAscendingCellStyle BackColor="#F1F1F1" />
                                                    <SortedAscendingHeaderStyle BackColor="#007DBB" />
                                                    <SortedDescendingCellStyle BackColor="#CAC9C9" />
                                                    <SortedDescendingHeaderStyle BackColor="#00547E" />
                                                </asp:GridView>
                                                <%-- End Input Grid --%>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4">
                                                <%-- Show Grid --%>
                                                <asp:GridView ID="GvSeasonShow" AllowPaging="false" AllowSorting="false" runat="server"
                                                    AutoGenerateColumns="false" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None"
                                                    BorderWidth="1px" CellPadding="3">
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="Season Name">
                                                            <HeaderTemplate>
                                                                <asp:Label ID="Label6" runat="server" Text="Season Name"></asp:Label>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtSeasonName" runat="server" Width="250px" TabIndex="-1" ReadOnly="true"
                                                                   style="text-transform:uppercase;" Text='<%# Eval("SeasonName") %>'></asp:TextBox>

                                                                <br />
                                                                <asp:Label ID="lblRowId" runat="server" Text='<%# Eval("RowId") %>' Style="display: none"></asp:Label>
                                                                 <asp:TextBox ID="txtoldseasonname" runat="server" Width="250px" TabIndex="-1" 
                                                                   Style="display: none"  Text='<%# Eval("oldSeasonName") %>'>
                                                                   </asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Detail">
                                                            <ItemTemplate>
                                                                <asp:GridView ID="GvSeasonShowSub" AllowPaging="false" AllowSorting="false" runat="server"
                                                                    AutoGenerateColumns="false" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None"
                                                                    BorderWidth="1px" CellPadding="3" TabIndex="-1">
                                                                    <Columns>
                                                                        <asp:TemplateField HeaderText="From Date">
                                                                            <HeaderTemplate>
                                                                                <asp:Label ID="Label7" runat="server" Text="From Date"></asp:Label>
                                                                            </HeaderTemplate>
                                                                            <ItemTemplate>
                                                                                <asp:TextBox ID="txtSeasonName" runat="server" Width="250px" TabIndex="-1" ReadOnly="true"
                                                                                    Text='<%# Eval("SeasonName") %>' Style="display: none"></asp:TextBox>
                                                                                <asp:TextBox ID="txtSeasonfromDate" runat="server" CssClass="fiel_input" TabIndex="-1"
                                                                                    Width="68px" ReadOnly="true" Text='<%# Eval("FromDate") %>'>
                                                                                    </asp:TextBox>

                                                                                   <asp:TextBox ID="txtoldseasonname" runat="server" Width="250px" TabIndex="-1" style="display:none"
                                                                    Text='<%# Eval("SeasonName") %>'>  </asp:TextBox>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="To Date">
                                                                            <HeaderTemplate>
                                                                                <asp:Label ID="Label8" runat="server" Text="To Date"></asp:Label>
                                                                            </HeaderTemplate>
                                                                            <ItemTemplate>
                                                                                <asp:TextBox ID="txtSeasonToDate" runat="server" CssClass="fiel_input" TabIndex="-1"
                                                                                    Width="68px" ReadOnly="true" Text='<%# Eval("ToDate") %>'></asp:TextBox>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Min Nights" Visible ="false">
                                                                            <HeaderTemplate>
                                                                                <asp:Label ID="Label9" runat="server" Text="Min Nights"></asp:Label>
                                                                            </HeaderTemplate>
                                                                            <ItemTemplate>
                                                                                <asp:TextBox ID="txtMinNight" runat="server" onkeypress='validateDigitOnly(event)'
                                                                                    Width="70px" TabIndex="-1" ReadOnly="true" Text='<%# Eval("MinNight") %>'></asp:TextBox>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                       
                                                                    </Columns>
                                                                    <HeaderStyle BackColor="#2D7C8A" Font-Bold="True" ForeColor="White" HorizontalAlign="Left" />
                                                                    <RowStyle ForeColor="#000066" />
                                                                    <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                                                                </asp:GridView>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Action">
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="imgSeditShow" runat="server" ImageUrl="~/Images/crystaltoolbar/edit.png"
                                                                    OnClick="imgSeditShow_Click" Width="25px" ToolTip="Edit Current Season" />
                                                                <asp:ImageButton ID="imgScancelShow" runat="server" ImageUrl="~/Images/crystaltoolbar/cancel1.jpg"
                                                                    OnClick="imgScancelShow_Click" Width="45px" Visible="false" ToolTip="Cancel Editing season" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Action">
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="imgScloseShow" runat="server" ImageUrl="~/Images/crystaltoolbar/DeleteRed.png"
                                                                    OnClick="imgScloseShow_Click" Width="25px" ToolTip="Delete Current Season" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <FooterStyle BackColor="White" ForeColor="#000066" />
                                                    <HeaderStyle BackColor="#2D7C8A" Font-Bold="True" ForeColor="White" HorizontalAlign="Left" />
                                                    <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                                                    <RowStyle ForeColor="#000066" />
                                                    <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                                                    <SortedAscendingCellStyle BackColor="#F1F1F1" />
                                                    <SortedAscendingHeaderStyle BackColor="#007DBB" />
                                                    <SortedDescendingCellStyle BackColor="#CAC9C9" />
                                                    <SortedDescendingHeaderStyle BackColor="#00547E" />
                                                </asp:GridView>
                                                <%-- End Show Grid --%>
                                            </td>
                                            
                                
                                      
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:HiddenField ID="hdCurrentDate" runat="server" />
                                                <asp:HiddenField ID="hdSeasonName" runat="server" />
                                                <asp:HiddenField ID="hdnpartycode" runat="server" />
                                                 <asp:HiddenField ID="hdncalledfrom" runat="server" />
                                                <asp:HiddenField ID="hdnapplicable" runat="server" />
                                                <asp:Button ID="btnhidden" runat="server" OnClick="btnhidden_Click" CssClass="field_button"
                                                    Style="display: none" Text="" Width="150px" />
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
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                    <td align="left" valign="top" width="150">
                            &nbsp;
                     </td>
                     <td style="display:none">
                     <asp:CheckBox ID="chkcommission" runat="server" class="cls_chkcomm"  Text="Commissionable" TabIndex="8" />
                     </td>
                    </tr>
                    <tr>
                    <td align="left" valign="top" width="150">
                            &nbsp;
                     </td>
                     <td style="width: 900px;display:none" valign="top" >
                            <div id="divcommision" runat="server" style="max-height: 390px; overflow: auto;border-style:single;border-width:6px">
                                <asp:Label ID="lblefffrom" runat="server" Text="Effective From Date"></asp:Label>

                                      <asp:TextBox ID="txtefffromdate" runat="server" CssClass="fiel_input" Width="80px" TabIndex="9"
                                      onchange="filleffectivetodate(this);" ></asp:TextBox>
                                           
                                        <cc1:CalendarExtender ID="dpeffFromDate" runat="server" Format="dd/MM/yyyy" PopupButtonID="ImgBtneffFrmDt"
                                            OnClientDateSelectionChanged="DateSelectCalExt" PopupPosition="Right" TargetControlID="txtefffromdate">
                                        </cc1:CalendarExtender>
                                        <cc1:MaskedEditExtender ID="MeeffFromDate" runat="server" Mask="99/99/9999" MaskType="Date"
                                            TargetControlID="txtefffromdate">
                                        </cc1:MaskedEditExtender>
                                        <asp:ImageButton ID="ImgBtneffFrmDt" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.png"
                                            TabIndex="-1" />
                                        <cc1:MaskedEditValidator ID="MeveffFromDate" runat="server" ControlExtender="MeeffFromDate"
                                            ControlToValidate="txtefffromdate" CssClass="field_error" Display="Dynamic" EmptyValueBlurredText="Date is required"
                                            EmptyValueMessage="Date is required" ErrorMessage="MeeffFromDate" InvalidValueBlurredMessage="Invalid Date"
                                            InvalidValueMessage="Invalid Date" TooltipMessage="Input a Date in Date/Month/Year">
                                        </cc1:MaskedEditValidator>

                                <asp:Label ID="lbleffto" runat="server" Text="Effective To Date"></asp:Label>

                                    <asp:TextBox ID="txtefftodate" runat="server" CssClass="fiel_input" Width="80px" TabIndex="10" onchange="ValidateEffToDate();" ></asp:TextBox>
                                           
                                        <cc1:CalendarExtender ID="dpefftoDate" runat="server" Format="dd/MM/yyyy" PopupButtonID="ImgBtnefftoDt"
                                            OnClientDateSelectionChanged="DateSelectCalExt" PopupPosition="Right" TargetControlID="txtefftodate">
                                        </cc1:CalendarExtender>
                                        <cc1:MaskedEditExtender ID="MeefftoDate" runat="server" Mask="99/99/9999" MaskType="Date"
                                            TargetControlID="txtefftodate">
                                        </cc1:MaskedEditExtender>
                                        <asp:ImageButton ID="ImgBtnefftoDt" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.png"
                                            TabIndex="-1" />
                                        <cc1:MaskedEditValidator ID="MevefftoDate" runat="server" ControlExtender="MeefftoDate"
                                            ControlToValidate="txtefftodate" CssClass="field_error" Display="Dynamic" EmptyValueBlurredText="Date is required"
                                            EmptyValueMessage="Date is required" ErrorMessage="MeefftoDate" InvalidValueBlurredMessage="Invalid Date"
                                            InvalidValueMessage="Invalid Date" TooltipMessage="Input a Date in Date/Month/Year">
                                        </cc1:MaskedEditValidator>
                                <div style="width: 100%; min-height: 10px" id="Div2" runat="server">
                                 </div>

                                <asp:GridView ID="grdcommission" runat="server" AutoGenerateColumns="False" BackColor="White"
                                    BorderColor="#999999" BorderStyle="Solid" BorderWidth="1px" Caption="Select Commision Type"
                                    CellPadding="3" CssClass="td_cell" Font-Bold="true" Font-Size="12px" GridLines="Vertical"
                                    TabIndex="12" Width="700px">
                                    <FooterStyle CssClass="grdfooter" />
                                    <Columns>
                                                   
                                    

                                    <asp:TemplateField HeaderText="Formula Code" Visible ="false">
                                    <EditItemTemplate>
                                    &nbsp;
                                    </EditItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" Width="75px" />
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemTemplate>
                                                        
                                    <asp:Label ID="txtformulacode" runat="server" Text='<%# Bind("formulaid") %>' CssClass="field_input" Width="80px"></asp:Label>
                                           
                                    </ItemTemplate>
                                    </asp:TemplateField>

                                   <asp:TemplateField Visible ="false">
                                        <HeaderStyle HorizontalAlign="Left" BackColor="#06788B" Width="25px" />
                                        <ItemTemplate>
                                            <asp:CheckBox runat="server" ID="chkcomm" Width="10px" />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                     <asp:TemplateField>
                                        <HeaderStyle HorizontalAlign="Left" BackColor="#06788B" Width="25px" />
                                        <ItemTemplate>
                                            <asp:RadioButton  runat="server" ID="optcomm" Width="10px" GroupName="formula" AutoPostBack ="true"  OnCheckedChanged="optcomm_CheckedChanged"  onclick = "RadioCheck(this);" />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                   <asp:TemplateField HeaderText="Formula Name" SortExpression="FormulaName">
                                    <ItemTemplate>
                                        <asp:Label ID="lblFormulaName" runat="server" Text='<%# Bind("FormulaName") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ControlStyle />
                                    <HeaderStyle HorizontalAlign="Left"  />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                      <asp:TemplateField HeaderText="Commission Formula" SortExpression="CommissionFormula">
                                    <ItemTemplate>
                                        <asp:Label ID="lblFormula" runat="server" Text='<%# Bind("Formula") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ControlStyle />
                                    <HeaderStyle HorizontalAlign="Left"  />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:TemplateField>  

                                    </Columns>
                                    <FooterStyle BackColor="White" ForeColor="#000066" />
                                    <RowStyle CssClass="grdRowstyle" />
                                    <SelectedRowStyle CssClass="grdselectrowstyle" />
                                    <PagerStyle CssClass="grdpagerstyle" HorizontalAlign="Center" />
                                    <HeaderStyle CssClass="grdheader" />
                                    <AlternatingRowStyle CssClass="grdAternaterow" />
                                </asp:GridView>
                              
                            
                            <div style="width: 100%; min-height: 10px" id="Div1" runat="server">
                             </div>
                        <table>
                        <tr>
                        <td >
                            <asp:GridView ID="grdcommissiondetail" runat="server" AutoGenerateColumns="False" BackColor="White"
                                    BorderColor="#999999" BorderStyle="Solid" BorderWidth="1px" 
                                    CellPadding="3" CssClass="td_cell" Font-Bold="true" Font-Size="12px" GridLines="Vertical"
                                    TabIndex="13" Width="200px">
                                    <FooterStyle CssClass="grdfooter" />
                                    <Columns>
                                      
                                    <asp:TemplateField HeaderText="Formula Code" Visible ="false">
                                    <EditItemTemplate>
                                    &nbsp;
                                    </EditItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" Width="75px" />
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemTemplate>
                                                        
                                    <asp:Label ID="txtterm1" runat="server" Text='<%# Bind("term1") %>' CssClass="field_input" ></asp:Label>
                                           
                                    </ItemTemplate>
                                    </asp:TemplateField>


                                   <asp:TemplateField HeaderText="Formula Name" SortExpression="termname">
                                    <ItemTemplate>
                                        <asp:Label ID="lblFormulaName" runat="server" Text='<%# Bind("termname") %>' Width="90px"></asp:Label>
                                    </ItemTemplate>
                                    <ControlStyle />
                                    <HeaderStyle HorizontalAlign="Left"  />
                                    <ItemStyle HorizontalAlign="Left" />
                                  </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Value" >
                                    <ItemTemplate>
                                    <asp:TextBox  ID="txtperc" runat="server" Width="50px" Style="text-align: right" Text='<%# Bind("value") %>' ></asp:TextBox>
                                    </ItemTemplate>
                                    <ControlStyle />
                                    <HeaderStyle HorizontalAlign="Left"  />
                                    <ItemStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>  

                                    </Columns>
                                    <FooterStyle BackColor="White" ForeColor="#000066" />
                                    <RowStyle CssClass="grdRowstyle" />
                                    <SelectedRowStyle CssClass="grdselectrowstyle" />
                                    <PagerStyle CssClass="grdpagerstyle" HorizontalAlign="Center" />
                                    <HeaderStyle CssClass="grdheader" />
                                    <AlternatingRowStyle CssClass="grdAternaterow" />
                                </asp:GridView>
                        
                        </td>
                        </tr>
                        <tr>
                        <td style="display:none">Service&nbsp;Rate </td>
                         <td align="left" style="display:none">
                             <asp:TextBox ID="txtservicerate" runat="server"  CssClass="field_input" Style="text-align: right"
                                             TabIndex="14" Width="58px"></asp:TextBox>
                        </td>
                        <td style="display:none">Tax&nbsp;% </td>
                         <td align="left" style="display:none" >
                             <asp:TextBox ID="txttaxperc" runat="server"  CssClass="field_input" Style="text-align: right"
                                             TabIndex="15" Width="58px"></asp:TextBox>
                        </td>
                        <td style="display:none">Commission&nbsp;% </td>
                         <td align="left" style="display:none">
                             <asp:TextBox ID="txtcommperc" runat="server"  CssClass="field_input" Style="text-align: right"
                                             TabIndex="16" Width="58px"></asp:TextBox>
                        </tr>
                        </table>
                         </div>
                        </td>
                     <td valign="top" style="display:none">
                            <div id="divterms" runat="server" style="max-height: 325px; overflow: auto;border-style:single;border-width:6px">
                            <asp:GridView ID="grdCommTerms" runat="server" AutoGenerateColumns="False" BackColor="White"
                                    BorderColor="#999999" BorderStyle="None" BorderWidth="1px" Caption="Commision Terms"
                                    CellPadding="3" CssClass="td_cell" Font-Bold="true" Font-Size="12px" GridLines="Vertical"
                                    TabIndex="17" Width="300px">
                                    <FooterStyle CssClass="grdfooter" />
                                <Columns>
                                    <asp:TemplateField Visible="False" HeaderText="Line No">
                                        <ItemTemplate>
                                            <asp:Label ID="lblFLineNo" runat="server" Text='<%# Bind("RankOrder") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Term Code">
                                        <ItemTemplate>
                                            <asp:Label ID="lblTermCode" runat="server" Text='<%# Bind("TermCode") %>' ></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle Wrap="false" HorizontalAlign="Left" VerticalAlign="Middle" Width="10%" />
                                        <ItemStyle Wrap="true" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Term Name">
                                        <ItemTemplate>
                                            <asp:Label ID="lblTermName" runat="server" Text='<%# Bind("TermName") %>' ></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle Wrap="false" Width="25%" HorizontalAlign="Left" VerticalAlign="Middle"/>
                                        <ItemStyle Wrap="true" />
                                    </asp:TemplateField>
                                </Columns>
                                <RowStyle CssClass="grdRowstyle" />
                                <SelectedRowStyle CssClass="grdselectrowstyle" />
                                <PagerStyle CssClass="grdpagerstyle" HorizontalAlign="Center" />
                                <HeaderStyle CssClass="grdheader" />
                                <AlternatingRowStyle CssClass="grdAternaterow" />
                            </asp:GridView> 
                            </div>
                         </td>
                        
                    </tr>
                    
                    <tr>
                        <td align="left" valign="top" width="150">
                            &nbsp;
                        </td>
                        <td align="left" valign="top" colspan="3">
                       
                        </td>
                       
                    </tr>
                   
                    <tr>
                        <td align="left" valign="top" width="150">
                            &nbsp;
                        </td>
                       
                        <td valign="top" colspan ="3">
                            <asp:Button ID="btnSave" runat="server" CssClass="field_button" 
                                TabIndex="15" Text="Save" Width="93px" />
                            &nbsp;<asp:Button ID="btnCancel" runat="server" CssClass="field_button" TabIndex="18"
                                Text="Return To Search" Width="139px" />
                            &nbsp;<asp:Button ID="btnhelp" runat="server" CssClass="field_button" TabIndex="19"
                                Text="Help" Visible="false" />
                        </td>
                        <td style="width: 100px" valign="top">
                        </td>
                         <td style="width: 100px" valign="top">
                        </td>
                    </tr>
                    <tr>
                        <td align="left" valign="top" width="150">
                            &nbsp;
                        </td>
                        <td style="width: 900px" valign="top" colspan="3">
                            <table style="width: 647px">
                                <tr>
                                    <td align="left" style="width: 140px">
                                        &nbsp;
                                    </td>
                                    <td align="left" style="width: 230px">
                                        &nbsp;&nbsp;
                                    </td>
                                    <td align="left" style="width: 265px">
                                        <input id="txtconnection" runat="server" style="visibility: hidden; width: 0px;"
                                            type="text" />
                                        <asp:Button ID="dummyCity" runat="server" Style="display: none;" />
                                        <asp:Button ID="dummyCityArea" runat="server" Style="display: none;" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <asp:Button ID="btngAlert" runat="server" Text="Fill" OnClick="btngAlert_Click" style="display:none"/>
                    <asp:Button ID="btnshowcomm" runat="server" Text="Fill" OnClick="btnshowcomm_Click" style="display:none"/>
                </tbody>
            </table>
</contenttemplate>
    </asp:UpdatePanel>

  

    <asp:ScriptManagerProxy id="ScriptManagerProxy1" runat="server">
        <services>
<asp:ServiceReference Path="~/clsServices.asmx"></asp:ServiceReference>
</services>
    </asp:ScriptManagerProxy>

</asp:Content>

