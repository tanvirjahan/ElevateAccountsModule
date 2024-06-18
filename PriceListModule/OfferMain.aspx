<%@ Page Language="VB" MasterPageFile="~/SubPageMaster.master" AutoEventWireup="false" EnableEventValidation="false" CodeFile="OfferMain.aspx.vb" Inherits="OfferMain"  %>

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
          else {
              return true;
          }
      }
      function showDiv() {
          var ddlBookingValidity = document.getElementById("<%=ddlBookingValidity.ClientID%>");
          var DivFrom = document.getElementById("<%=DivFrom.ClientID%>");
          var DivTo = document.getElementById("<%=DivTo.ClientID%>");
          var DivDays = document.getElementById("<%=DivDays.ClientID%>");
          var lblFrom = document.getElementById("<%=lblFrom.ClientID%>");
          var lblBookVal = document.getElementById("<%=lblBookVal.ClientID%>");

          var txtfromDate = document.getElementById("<%=dpFrom.ClientID%>");
          var txtToDate = document.getElementById("<%=dpTo.ClientID%>");
          var txtDays = document.getElementById("<%=txtDays.ClientID%>");

          txtfromDate.value = ""
          txtToDate.value = ""
          txtDays.value = ""

          lblBookVal.innerHTML = 'Book By';
          DivDays.style.visibility = "hidden";

          if (ddlBookingValidity.value == '2') {
              DivFrom.style.visibility = "hidden";
              DivTo.style.visibility = "hidden";
              DivDays.style.visibility = "visible";
              lblBookVal.innerHTML = 'Booking Validity Days';
              lblBookVal.style.width = "auto";
             
          }
          else if (ddlBookingValidity.value == '3') {
              DivFrom.style.visibility = "hidden";
              DivTo.style.visibility = "hidden";
              DivDays.style.visibility = "visible";
              lblBookVal.innerHTML = 'Booking Validity Months';
              lblBookVal.style.width = "auto";
              
          }
          else if (ddlBookingValidity.value == '4') {
              DivFrom.style.visibility = "visible";
              DivTo.style.visibility = "visible";
              DivDays.style.visibility = "hidden";
             
              DivFrom.style.visibility = "visible";
              lblBookVal.style.visibility = "visible";
              lblBookVal.innerHTML = 'From';
              lblBookVal.style.width = "auto";

          }

          else if (ddlBookingValidity.value == '5') {
              DivFrom.style.visibility = "visible";
              DivTo.style.visibility = "hidden";
              lblFrom.style.visibility = "visible";
              DivDays.style.visibility = "hidden";
              lblBookVal.innerHTML = 'Book By';
              lblBookVal.style.width = "auto";

          }

          else if (ddlBookingValidity.value == '1') {
              DivFrom.style.visibility = "visible";
              DivTo.style.visibility = "hidden";
              lblFrom.style.visibility = "visible";
              DivDays.style.visibility = "hidden";
              lblBookVal.innerHTML = 'Book By';
              lblBookVal.style.width = "auto";
             
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


    function ShowProgess() {

        var ModalPopupDays = $find("ModalPopupDays");

        ModalPopupDays.show();
        return true;
    }
</script>

<script language="javascript" type="text/javascript" >
    function StayDateSelectCalExt() {
        var grid = document.getElementById("<%=grdpromotiondetail.ClientID%>");
        var inputs = grid.getElementsByTagName("input");
        var txtToDate, txtfromDate;
        var calendarBehavior1;
        var calendarBehavior2;
        for (var i = 0; i < inputs.length; i++) {
            if (inputs[i].type == "text") {
                if (inputs[i].name.indexOf("$txtStayfromDate") >= 0 || inputs[i].id.indexOf("$txtStayfromDate") >= 0) {
                    txtfromDate = document.getElementById(inputs[i].id); // inputs[i];
                }

                if (inputs[i].name.indexOf("$txtStayToDate") >= 0 || inputs[i].id.indexOf("$txtStayToDate") >= 0) {
                    txtToDate = document.getElementById(inputs[i].id); // inputs[i];
                }

                if (inputs[i].name.indexOf("$txtStayfromDate_CalendarExtender") >= 0 || inputs[i].id.indexOf("$txtStayfromDate_CalendarExtender") >= 0) {
                    calendarBehavior1 = inputs[i];
                }
                if (inputs[i].name.indexOf("$txtStayToDate_CalendarExtender") >= 0 || inputs[i].id.indexOf("$txtStayToDate_CalendarExtender") >= 0) {
                    calendarBehavior2 = inputs[i];
                }
            }
        }
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
                if (txtToDate.value != "") {

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
                alert("Todate  should  be greater than From date");
                SeasonDateSelectCalExt();
                return false;
            }
        }
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
        var vartxtcode = document.getElementById("<%=txtpromotionid.ClientID%>");


        if ((vartxtcode.value == '')) {
            doLinks(false);
        }
        else {
            doLinks(true);
        }

    }
    //    added by sribish
    function doLinks(how) {
        var strName = '';
        for (var l = document.links, i = l.length - 1; i > -1; --i) {
            strName = l[i].outerText;

            if (strName != 'X') {
                if (!how)
                    l[i].onclick = function () { alert('Please Save Main details to continue'); return false; };
                else
                    l[i].onclick = function () { return true; };
            }
        }
    }
    function load() {
        //    added by sribish
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(formmodecheck);
    }

    function fnClearTextBoxNeeded(keybr) {
        if (keybr == 9 || keybr == 16 || keybr == 17 || keybr == 18 || keybr == 20 || keybr == 27 || keybr == 45 || keybr == 36 || keybr == 33 || keybr == 35 || keybr == 34 || keybr == 37 || keybr == 38 || keybr == 39 || keybr == 40 || keybr == 93 || keybr == 92 || keybr == 112 || keybr == 123 || keybr == 144 || keybr == 145 || keybr == 19 || keybr == 13) {
            return false;
        }
        else {
            return true;
        }
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



    function SetContextKey() {
        $find('< %=AutoCompleteExtender4.ClientID%>').set_contextKey($get("<%=hdnpartycode.ClientID %>").value);
    }
    function SetContextKeyValue() {
        //        alert("C");
        //        $find('< %=AutoCompleteExtender4.ClientID%>').set_contextKey($get("<%=hdnpartycode.ClientID %>").value);
    }


    function hotelautocompleteselected(source, eventArgs) {
        if (eventArgs != null) {
            document.getElementById('<%=txthotelcode.ClientID%>').value = eventArgs.get_value();
        }
        else {
            document.getElementById('<%=txthotelcode.ClientID%>').value = '';
        }
    }

    function interhotelnautocompleteselected(source, eventArgs) {

        var hiddenfieldID = source.get_id().replace("AutoCompleteExtender2", "txtinterhotelcode");
        $get(hiddenfieldID).value = eventArgs.get_value();

    }



    function promonautocompleteselected(source, eventArgs) {

        var hiddenfieldID = source.get_id().replace("AutoCompleteExtender4", "txtpromotioncode");
        $get(hiddenfieldID).value = eventArgs.get_value();

    }

    function arrivalautocompleteselected(source, eventArgs) {

        var hiddenfieldID = source.get_id().replace("arrivalterminal_AutoCompleteExtender", "txtarrivalterminal");
        $get(hiddenfieldID).value = eventArgs.get_value();


    }

    function departureautocompleteselected(source, eventArgs) {

        var hiddenfieldID = source.get_id().replace("departureterminal_AutoCompleteExtender", "txtdepartureterminal");
        $get(hiddenfieldID).value = eventArgs.get_value();

    }


    function hotelautocompleteremove() {
        document.getElementById('<%=txthotelcode.ClientID%>').value = '';
    }


    $("[id*=chkrmtypeAll]").live("click", function () {
        var chkHeader = $(this);
        var grid = $(this).closest("table");
        $("input[type=checkbox]", grid).each(function () {
            if (chkHeader.is(":checked")) {
                $(this).attr("checked", "checked");
                $("td", $(this).closest("tr")).addClass("selected");

            } else {
                $(this).removeAttr("checked");
                $("td", $(this).closest("tr")).removeClass("selected");

            }
        });
    });

    $("[id*=chkflightAll]").live("click", function () {
        var chkHeader = $(this);
        var grid = $(this).closest("table");
        $("input[type=checkbox]", grid).each(function () {
            if (chkHeader.is(":checked")) {
                $(this).attr("checked", "checked");
                $("td", $(this).closest("tr")).addClass("selected");

            } else {
                $(this).removeAttr("checked");
                $("td", $(this).closest("tr")).removeClass("selected");

            }
        });
    });


    $("[id*=chkrmtypAll]").live("click", function () {
        var chkHeader = $(this);
        var grid = $(this).closest("table");
        $("input[type=checkbox]", grid).each(function () {
            if (chkHeader.is(":checked")) {
                $(this).attr("checked", "checked");
                $("td", $(this).closest("tr")).addClass("selected");

            } else {
                $(this).removeAttr("checked");
                $("td", $(this).closest("tr")).removeClass("selected");

            }
        });
    });

    $("[id*=chkmealAll]").live("click", function () {
        var chkHeader = $(this);
        var grid = $(this).closest("table");
        $("input[type=checkbox]", grid).each(function () {
            if (chkHeader.is(":checked")) {
                $(this).attr("checked", "checked");
                $("td", $(this).closest("tr")).addClass("selected");

            } else {
                $(this).removeAttr("checked");
                $("td", $(this).closest("tr")).removeClass("selected");

            }
        });
    });

    $("[id*=chkrmcatAll]").live("click", function () {
        var chkHeader = $(this);
        var grid = $(this).closest("table");
        $("input[type=checkbox]", grid).each(function () {
            if (chkHeader.is(":checked")) {
                $(this).attr("checked", "checked");
                $("td", $(this).closest("tr")).addClass("selected");

            } else {
                $(this).removeAttr("checked");
                $("td", $(this).closest("tr")).removeClass("selected");

            }
        });
    });

    $("[id*=chksuppAll]").live("click", function () {
        var chkHeader = $(this);
        var grid = $(this).closest("table");
        $("input[type=checkbox]", grid).each(function () {
            if (chkHeader.is(":checked")) {
                $(this).attr("checked", "checked");
                $("td", $(this).closest("tr")).addClass("selected");

            } else {
                $(this).removeAttr("checked");
                $("td", $(this).closest("tr")).removeClass("selected");

            }
        });
    });


    $("[id*=chkcombineAll]").live("click", function () {
        var chkHeader = $(this);
        var grid = $(this).closest("table");
        $("input[type=checkbox]", grid).each(function () {
            if (chkHeader.is(":checked")) {
                $(this).attr("checked", "checked");
                $("td", $(this).closest("tr")).addClass("selected");

            } else {
                $(this).removeAttr("checked");
                $("td", $(this).closest("tr")).removeClass("selected");

            }
        });
    });

    $("[id*=chkdatesAll]").live("click", function () {
        var chkHeader = $(this);
        var grid = $(this).closest("table");
        $("input[type=checkbox]", grid).each(function () {
            if (chkHeader.is(":checked")) {
                $(this).attr("checked", "checked");
                $("td", $(this).closest("tr")).addClass("selected");

            } else {
                $(this).removeAttr("checked");
                $("td", $(this).closest("tr")).removeClass("selected");

            }
        });
    });





    function showintergrid(chkinterhotel) {
        alert("B");
        var chkinter = document.getElementById(chkinterhotel);
        if (chkinter.checked) {
            $("#" + "<%=divinter.ClientID %>").slideDown();
        }
        else {
            $("#" + "<%=divinter.ClientID %>").fadeOut();
        }
    }

    function selectrmtype(ddlrmtyp, hdnrmtyp) {
        var ddlrmtyp = document.getElementById(ddlrmtyp);
        var hdnrmtyp = document.getElementById(hdnrmtyp);
        hdnrmtyp.value = ddlrmtyp.value;
       
    }
    function selectrmcat(ddlrmcat, hdnrmcat) {
        var ddlrmcat = document.getElementById(ddlrmcat);
        var hdnrmcat = document.getElementById(hdnrmcat);
        hdnrmcat.value = ddlrmcat.value;
    }
    function selectmeal(ddlmeal, hdnmealcode) {
       var ddlmeal = document.getElementById(ddlmeal);
      
        var hdnmealcode = document.getElementById(hdnmealcode);
        hdnmealcode.value = ddlmeal.value;
    }
    function selectrmcatsup(ddlrmcatsup, hdnrmcatsup) {
        var ddlrmcatsup = document.getElementById(ddlrmcatsup);
        var hdnrmcatsup = document.getElementById(hdnrmcat);
        hdnrmcatsup.value = ddlrmcatsup.value;
    }


    function showcontrolfill(chkseason) {
        //        $get("ctl00_Main_btngAlert").click();

    }

    function showpromotion() {

        var ddlchkpromotion = document.getElementById("<%=ddlcombine.ClientID%>");
        var divcombine = document.getElementById("<%=divcombine.ClientID%>");
        if (ddlchkpromotion.selectedIndex == 2 || ddlchkpromotion.selectedIndex == 4) {
            divcombine.style.display = "block";
            divcombine.style.visibility = "visible";
        }
        else {
            divcombine.style.display = "none";
        }

    }

    function showcontract() {


        var divapplydiscount = document.getElementById("<%=ddlapplydiscount.ClientID%>");
        var btnseelct = document.getElementById("<%=btnselectcontract.ClientID%>");
        if (divapplydiscount.selectedIndex == 1 || divapplydiscount.selectedIndex == 2 || divapplydiscount.selectedIndex == 3) {
            btnseelct.style.display = "block";
            btnseelct.style.visibility = "visible";
        }
        else {
            btnseelct.style.display = "none";
        }

    }


    var rw;
    function showcolumns(ddlopt, rowid) {
        rw = parseInt(rowind);
        var ddloptions = document.getElementById(ddlopt);
        var myGridView = document.getElementById("<%=grdpromotiondetail.ClientID %>");

       // alert('test');
        //        if (ddloptions == '2') {

        //            $("table tr").find("th:eq(" + (7 - 1) + ")").toggle();
        //            $("table tr").find("th:eq(" + (8 - 1) + ")").toggle();
        //        }
    }



    function checkdates(txtfromdate, txtodate) {

        var fdate = document.getElementById(txtfromdate);
        var tdate = document.getElementById(txtodate);


     //   var confromdate = document.getElementById('< %=hdnconfromdate.ClientID %>');
    //    var contodate = document.getElementById('< %=hdncontodate.ClientID %>');



        if (fdate.value == null || fdate.value == "") {
            alert("Please select from date.");
            setTimeout(function () { fdate.focus(); }, 1);
            fdate.value = "";
        }

        var dp = fdate.value.split("/");
        var newfdate = new Date(dp[2] + "/" + dp[1] + "/" + dp[0]);

        var dp1 = tdate.value.split("/");
        var newtdate = new Date(dp1[2] + "/" + dp1[1] + "/" + dp1[0]);


        var dp2 = confromdate.value.split("/");
        var newcfdate = new Date(dp2[2] + "/" + dp2[1] + "/" + dp2[0]);

        var dp3 = contodate.value.split("/");
        var newctdate = new Date(dp3[2] + "/" + dp3[1] + "/" + dp3[0]);

        newfdate = getFormatedDate(newfdate);
        newtdate = getFormatedDate(newtdate);

        newcfdate = getFormatedDate(newcfdate);
        newctdate = getFormatedDate(newctdate);

        newfdate = new Date(newfdate);
        newtdate = new Date(newtdate);

        newcfdate = new Date(newcfdate);
        newctdate = new Date(newctdate);

        if (newtdate < newfdate) {
            alert("To date should  be greater than From date");
            setTimeout(function () { tdate.focus(); }, 1);
            tdate.value = "";
        }

     

        setdate();
    }


    function checkfromdates(txtfromdate, txtodate) {

        var fdate = document.getElementById(txtfromdate);
        var tdate = document.getElementById(txtodate);
        
       
//        var confromdate = document.getElementById('< %=hdnconfromdate.ClientID %>');
//        var contodate = document.getElementById('< %=hdncontodate.ClientID %>');

        var dp = fdate.value.split("/");
        var newfdate = new Date(dp[2] + "/" + dp[1] + "/" + dp[0]);

        var dp1 = tdate.value.split("/");
        var newtdate = new Date(dp1[2] + "/" + dp1[1] + "/" + dp1[0]);


        var dp2 = confromdate.value.split("/");
        var newcfdate = new Date(dp2[2] + "/" + dp2[1] + "/" + dp2[0]);

        var dp3 = contodate.value.split("/");
        var newctdate = new Date(dp3[2] + "/" + dp3[1] + "/" + dp3[0]);

        newcfdate = getFormatedDate(newcfdate);
        newctdate = getFormatedDate(newctdate);

        newfdate = getFormatedDate(newfdate);
        newtdate = getFormatedDate(newtdate);

        newfdate = new Date(newfdate);
        newtdate = new Date(newtdate);

        newcfdate = new Date(newcfdate);
        newctdate = new Date(newctdate);

        if (newfdate > newtdate) {
            alert("From date should not be greater than To date");
            setTimeout(function () { tdate.focus(); }, 1);
            tdate.value = "";
        }


        setdate();
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

<script type="text/javascript">
    var SelectedRow = null;
    var SelectedRowIndex = null;
    var UpperBound = null;
    var LowerBound = null;

    window.onload = function () {
        LowerBound = 0;
        SelectedRowIndex = -1;
    }

    function SelectRow(CurrentRow, RowIndex) {
        var gridView = document.getElementById("<%=grdpromotiondetail.ClientID %>"); // *********** Change gridview name
        UpperBound = gridView.getElementsByTagName("tr").length - 2;
        if (SelectedRow == CurrentRow || RowIndex > UpperBound || RowIndex < LowerBound)
            return;

        if (SelectedRow != null) {
            SelectedRow.style.backgroundColor = SelectedRow.originalBackgroundColor;
            SelectedRow.style.color = SelectedRow.originalForeColor;
        }

        if (CurrentRow != null) {
            CurrentRow.originalBackgroundColor = CurrentRow.style.backgroundColor;
            CurrentRow.originalForeColor = CurrentRow.style.color;
            CurrentRow.style.backgroundColor = '#FFCC99';
            CurrentRow.style.color = 'Black';
            var txtFrm = CurrentRow.cells[1].getElementsByTagName("input")[0];
            txtFrm.focus();
            //alert(txtFrm.value);
        }

        SelectedRow = CurrentRow;
        SelectedRowIndex = RowIndex;
        setTimeout("SelectedRow.focus();", 0);
    }

    function SelectSibling(e) {
        var e = e ? e : window.event;
        var KeyCode = e.which ? e.which : e.keyCode;
        if (KeyCode == 40)
            SelectRow(SelectedRow.nextSibling, SelectedRowIndex + 1);
        else if (KeyCode == 38)
            SelectRow(SelectedRow.previousSibling, SelectedRowIndex - 1);

        // return false;
    }
    function LastSelectRow(CurrentRow, RowIndex) {
        var row = document.getElementById(CurrentRow);
        SelectRow(row, RowIndex);

    }


    $("[id*=chkAll]").live("click", function () {
        var chkHeader = $(this);
        var grid = $(this).closest("table");
        $("input[type=checkbox]", grid).each(function () {
            if (chkHeader.is(":checked")) {
                $(this).attr("checked", "checked");
                $("td", $(this).closest("tr")).addClass("selected");
            } else {
                $(this).removeAttr("checked");
                $("td", $(this).closest("tr")).removeClass("selected");
            }
        });
    });



    </script>

<style>
     
          .displaynone
        {
        	display:none;
        }
     
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
         <style type="text/css">
        .ModalPopupBG
        {
            background-color: #ffff;
            filter: alpha(opacity=50);
            opacity: 0.7;
        }
        
         .ModalPopupBGnew
        {
            background-color: #ffff;
            filter: alpha(opacity=50);
            opacity: 0.7;
        }
        
        .HellowWorldPopup
        {
            min-width: 200px;
            min-height: 150px;
            background: white;
            font-size: 10pt;
            font-weight: bold;
            border-bottom-style: double;
            border-width: medium;
        }
        
        *
        {
            outline: none;
                 margin-left: 0px;
             }
        
        .fmhead 
        {
        	display: block;
         text-align: center;
         
        }
        
          .ModalPopupBG
        {
            filter: alpha(opacity=50);
            opacity: 0.9;
        }
        
            .ModalPopupBGnew
        {
            filter: alpha(opacity=50);
            opacity: 0.9;
        }
           .ModalPopupBGmeal
        {
            filter: alpha(opacity=50);
            opacity: 0.9;
        }
             .style3
             {
                 font-family: Verdana, Arial, Geneva, "ms sans serif";
                 font-size: 10pt;
                 font-weight: normal;
                 width: 115px;
                 height: 21px;
             }
             .style7
             {
                 font-family: Verdana, Arial, Geneva, "ms sans serif";
                 font-size: 10pt;
                 font-weight: normal;
                 width: 239px;
                 height: 20px;
             }
             .style8
             {
                 width: 139px;
                 height: 21px;
             }
             .style10
             {
                 font-family: Verdana, Arial, Geneva, "ms sans serif";
                 font-size: 10pt;
                 font-weight: normal;
                 width: 80px;
                 height: 21px;
             }
             .style12
             {
                 font-family: Verdana, Arial, Geneva, "ms sans serif";
                 font-size: 10pt;
                 font-weight: normal;
                 width: 120px;
                 height: 20px;
             }
             .style14
             {
                 font-family: Verdana, Arial, Geneva, "ms sans serif";
                 font-size: 10pt;
                 font-weight: normal;
                 width: 209px;
                 height: 20px;
             }
             .style15
             {
                 height: 20px;
                 width: 115px;
             }
             .style18
             {
                 font-family: Verdana, Arial, Geneva, "ms sans serif";
                 font-size: 10pt;
                 font-weight: normal;
                 width: 133px;
                 height: 20px;
             }
             .style19
             {
                 width: 129px;
             }
             .style20
             {
                 height: 20px;
             }
             .style21
             {
                 width: 209px;
             }
             .style22
             {
                 width: 98px;
             }
    </style>


    <asp:UpdatePanel id="UpdatePanel12" runat="server">
        <ContentTemplate>
            <table style="width: 100%; height: 100%; border-right: gray 2px solid; border-top: gray 2px solid;
                border-left: gray 2px solid; border-bottom: gray 2px solid" class="td_cell" align="left">
                <tbody>
                    <tr>
                        <td valign="top" align="center" width="150" colspan="6">
                            <asp:Label ID="lblHeading" runat="server" Text="Offer" CssClass="field_heading" Width="100%"
                                ForeColor="White"></asp:Label>
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
                                        <asp:TextBox ID="txthotelname" runat="server" AutoPostBack="True" CssClass="field_input"
                                            MaxLength="500" Width="250px"></asp:TextBox>
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
                                    <td>
                                        Promotion&nbsp;Name
                                    </td>
                                    <td colspan="2">
                                        <asp:TextBox ID="txtpromotionname" runat="server" CssClass="field_input" MaxLength="500"
                                            TabIndex="1" Width="400px"></asp:TextBox>
                                    </td>
                                    <td>
                                    </td>
                                    <td width="100px">
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label2" runat="server" Style="vertical-align: top;" Text="Applicable To"
                                            Width="90px"></asp:Label>
                                    </td>
                                    <td align="left" colspan="2">
                                        <asp:TextBox ID="txtApplicableTo" runat="server" Rows="2" Style="margin: 0px; height: 48px;
                                            width: 250px" TabIndex="2" TextMode="MultiLine"></asp:TextBox>
                                    </td>
                                      <td>
                                        <asp:Label ID="Label6" runat="server" Style="vertical-align: top;" Text="Short Name"
                                            Width="90px"></asp:Label>
                                    </td>
                                    <td>
                                     <asp:TextBox ID="txtshortname"  runat="server" Width="200px"></asp:TextBox>
                                    </td>
                                    <td colspan="4">
                                        <asp:Label ID="Label1" runat="server" Style="vertical-align: top;" Text="Promotion ID"
                                                                            Width="90px"></asp:Label>
                                        <asp:TextBox ID="txtpromotionid" ReadOnly="true" runat="server" Width="80px"></asp:TextBox>
                                         <asp:CheckBox ID="chkactive" runat="server" Text="With Drawn" TabIndex="3" Checked="false" />
                                    </td>
                                    <td  >
                                        
                                        
                                    </td>
                                                                      
                                </tr>
                                <tr>
                                <td></td>
                                <td></td>
                                <td></td>
                                  <td>
                                        
                                        <asp:Label ID="lblstatustext" runat="server"  Text="Status:"
                                            Width="43px"></asp:Label>
                                             <asp:Label ID="lblstatus" runat="server" Font-Bold="True" ForeColor="#3366FF" Style="vertical-align: top;"
                                            Text="Status" Width="43px"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 100px" valign="top" colspan="10">
                            <div style="width: 100%; min-height: 400px" id="iframeINF" runat="server">
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                    <ContentTemplate>
                                        <div class="container" id="VS">
                                            <div id="search_box_container">
                                            </div>
                                        </div>
                                        <br />
                                        <asp:DataList ID="dlList" runat="server" RepeatColumns="4" RepeatDirection="Horizontal"
                                            Width="366px">
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
                                            <asp:Button ID="btnvsprocess" runat="server" Style="display: none" />
                                        </div>
                                        <div id="countrygroup1" style="float: left; margin-left: 40px; width: 99%">
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
                        <td colspan="20">
                            <table>
                                <tr>
                                    <td valign="top" width="100px" colspan="1">
                                        <asp:UpdatePanel ID="UpdatePanepromo" runat="server">
                                            <ContentTemplate>
                                                <div id="divpromotype" runat="server" style="max-height: 310px; overflow: auto;">
                                                    <asp:DataList ID="dlPromotionType" runat="server" RepeatColumns="4" Caption="Promotion Type"
                                                        BackColor="White" BorderColor="#999999" BorderStyle="None" BorderWidth="1px"
                                                        RepeatDirection="Horizontal" CellPadding="3" CssClass="td_cell" Font-Bold="true"
                                                        Font-Size="12px" Width="717px">
                                                        <FooterStyle CssClass="grdfooter" />
                                                        <HeaderStyle HorizontalAlign="Left" BackColor="#06788B" Width="2%" />
                                                        <HeaderTemplate>
                                                            <asp:CheckBox ID="chkAll" runat="server" OnCheckedChanged="check_changed" TabIndex="4"
                                                                AutoPostBack="true" />
                                                            <asp:Label ID="lblPromotionType" runat="server" Text="Promotion Type"></asp:Label>
                                                        </HeaderTemplate>
                                                        <ItemStyle HorizontalAlign="Left" Width="2%"></ItemStyle>
                                                        <ItemTemplate>
                                                            <table class="styleDatalist" style="border: 10px; font-family: Arial,Verdana, Geneva, ms sans serif;
                                                                font-size: 10pt; border-collapse: collapse;">
                                                                <tr style="">
                                                                    <td style="border: 0px;">
                                                                        <asp:CheckBox ID="chkpromotiontype" runat="server" AutoPostBack="true" OnCheckedChanged="check_changed" />
                                                                        <asp:Label ID="txtpromtoiontype" runat="server" Text='<%# Bind("promtoiontype") %>'></asp:Label>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </ItemTemplate>
                                                        <ItemStyle CssClass="grdRowstyle" />
                                                        <HeaderStyle CssClass="grdheader" />
                                                        <AlternatingItemStyle CssClass="grdAternaterow" />
                                                        <SelectedItemStyle CssClass="grdselectrowstyle" />
                                                    </asp:DataList>
                                                </div>
                                                <asp:Button ID="btngAlert" runat="server" Text="Fill" OnClick="btngAlert_Click" Style="display: none" />
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                    <td width="120px" valign="top">
                                        <asp:DataList ID="dlWeekDays" runat="server" RepeatColumns="3" Caption="Select Days"
                                            BackColor="White" BorderColor="#999999" BorderStyle="None" BorderWidth="1px"
                                            RepeatDirection="Horizontal" CellPadding="3" CssClass="td_cell" Font-Bold="true"
                                            Font-Size="12px" Width="350px" Height="78px">
                                            <FooterStyle CssClass="grdfooter" />
                                            <HeaderStyle HorizontalAlign="Left" BackColor="#06788B" Width="2%" />
                                            <HeaderTemplate>
                                                <asp:CheckBox ID="chkAll" runat="server" TabIndex="5" />
                                                <asp:Label ID="lblPromotionType" runat="server" Text="Days Of Week"></asp:Label>
                                                <asp:Label ID="lblSrNo" runat="server" Text='<%# Bind("SrNo") %>'></asp:Label>
                                            </HeaderTemplate>
                                            <ItemStyle HorizontalAlign="Left" Width="2%"></ItemStyle>
                                            <ItemTemplate>
                                                <table class="styleDatalist" style="border: 10px; font-family: Arial,Verdana, Geneva, ms sans serif;
                                                    font-size: 10pt; border-collapse: collapse;">
                                                    <tr style="">
                                                        <td style="border: 0px;">
                                                            <asp:CheckBox ID="chkSelect" runat="server" AutoPostBack="true" />
                                                            <asp:Label ID="lblDaysOfWeek" runat="server" Text='<%#Bind("days") %>'></asp:Label>
                                                    </tr>
                                                </table>
                                            </ItemTemplate>
                                            <ItemStyle CssClass="grdRowstyle" />
                                            <HeaderStyle CssClass="grdheader" />
                                            <AlternatingItemStyle CssClass="grdAternaterow" />
                                            <SelectedItemStyle CssClass="grdselectrowstyle" />
                                        </asp:DataList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div style="width: 100%; min-height: 25px" id="Div11" runat="server">
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                  
                                    <td class="field_heading" colspan="4">
                                        <asp:Label ID="Label5" runat="server" Text="Promotion Details"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_cell" colspan="11">
                                        <div runat="server" id="DivBooking">
                                           <table bordercolor="#999999">
                                                <tr>
                                                    <td >
                                                        Booking&nbsp;ValidityOptions
                                                    </td>
                                                    <td >
                                                        <select style="width: 220px" id="ddlBookingValidity" onchange="showDiv();" class="drpdown"
                                                            name="D1" runat="server" tabindex ="4">
                                                            <option value="1" selected>Book By</option>
                                                            <option value="2">Book Before Days from Checkin</option>
                                                            <option value="3">Book Before Months from Check In</option>
                                                            <option value="4">Range of Dates</option>
                                                            <option value="5">Book By and Book Before Days From Checkin</option>
                                                        </select>
                                                    </td>
                                                    <td  >
                                                        <asp:Label ID="lblBookVal" runat="server" CssClass="field_caption" 
                                                           text="Booking Validity From/Book By" ></asp:Label>
                                                            <asp:Label ID="lblFrom" runat="server" Visible ="false" CssClass="field_caption" Width="6px" Height="16px"></asp:Label>
                                                    </td>
                                                    <td colspan ="10" >
                                                       
                                                            <table>
                                                                <tr>
                                                                    <td align="left" colspan="2">
                                                                        <div id="DivFrom" runat="server">
                                                                            <asp:TextBox ID="dpFrom" runat="server" CssClass="fiel_input" Width="75px" Wrap="false" TabIndex ="5"></asp:TextBox>
                                                                            <asp:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd/MM/yyyy" PopupButtonID="imgdpFrom"
                                                                                TargetControlID="dpFrom">
                                                                            </asp:CalendarExtender>
                                                                            <asp:MaskedEditExtender ID="MaskedEditExtender1" runat="server" Mask="99/99/9999"
                                                                                MaskType="Date" TargetControlID="dpFrom">
                                                                            </asp:MaskedEditExtender>
                                                                            <asp:ImageButton ID="imgdpFrom" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.png" />
                                                                        </div>
                                                                    </td>
                                                                 
                                                                    <td>
                                                                        <div id="DivDays" style="visibility: hidden; width: .5px;" runat="server">
                                                                            <input id="txtDays" class="txtbox" tabindex="6" onkeypress="return checkNumber(event);"
                                                                                type="text" maxlength="3" runat="server" size="3" />
                                                                        </div>
                                                                    </td>
                                                                    <td align="left" colspan="2">
                                                                        <div id="DivTo" style="visibility: hidden; width: 150px; height: 18px;" runat="server">
                                                                           
                                                                                   
                                                                                      To&nbsp;
                                                                                
                                                                                        <asp:TextBox ID="dpTo" runat="server" CssClass="fiel_input" Width="75px" TabIndex ="7"></asp:TextBox>
                                                                                        <asp:CalendarExtender ID="CalendarExtender2" runat="server" Format="dd/MM/yyyy" PopupButtonID="imgdpTo"
                                                                                            TargetControlID="dpTo">
                                                                                        </asp:CalendarExtender>
                                                                                        <asp:MaskedEditExtender ID="MaskedEditExtender2" runat="server" Mask="99/99/9999"
                                                                                            MaskType="Date" TargetControlID="dpTo">
                                                                                        </asp:MaskedEditExtender>
                                                                                        <asp:ImageButton ID="imgdpTo" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.png" />
                                                                                  
                                                                           
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        
                                                    </td>
                                                   
                                                    <td >
                                                        <asp:Button ID="btnfillgrid" runat="server" CssClass="btn" Text="Fill Booking Validity to All Rows"
                                                            Width="240px" TabIndex="8" />
                                                    </td>
                                                    
                                                </tr>
                                            
                                           
                                                <tr>
                                                   
                                                   
                                                    <td class="style3">
                                                        Hotel&nbsp;Booking&nbsp; Code
                                                    </td>
                                                    <td class="style21" colspan ="2">
                                                        <input id="txtbookingcode" class="txtbox" type="text" tabindex="9" runat="server"
                                                            size="4" />
                                                        <asp:Button ID="btnBookingCode" runat="server" CssClass="btn" Text="Fill Booking Code"
                                                            Width="144px" TabIndex="10" />
                                                    </td>
                                                     
                                                      <td >
                                                        Nights
                                                    </td>
                                                    <td >
                                                        <select style="width: 120px" id="ddlfillnights"  class="drpdown"
                                                            name="D1" runat="server" tabindex ="4">
                                                            <option value="1" selected>Min Nights</option>
                                                            <option value="2">Max Nights</option>
                                                           
                                                        </select>
                                                    </td>
                                                    <td>
                                                     <input id="txtminnights" class="txtbox" onkeypress="return checkNumber(event);" type="text"
                                                            maxlength="5" tabindex="11" runat="server" size="6" />
                                                        <asp:Button ID="btnminFill" runat="server" CssClass="btn" TabIndex="12" Text="Fill" />
                                                    </td>
                                                   
                                                    
                                                </tr>
                                          
                                           </table>
                                           </div> 
                                    </td>
                                </tr>
                            </table>
                            
                        </td>
                    </tr>
                    <tr>
                      <td align="left" valign="top" width="150">
                            &nbsp;
                        </td>
                        <td colspan="4">
                            <asp:UpdatePanel ID="updatenew" runat="server">
                                <ContentTemplate>
                                    <table>
                                        <tr>
                                            <td>
                                            </td>
                                            <td>
                                                <asp:GridView ID="grdpromotiondetail" runat="server" AutoGenerateColumns="False"
                                                    BackColor="White" BorderColor="#999999" BorderStyle="None" CellPadding="3" CssClass="td_cell"
                                                    Font-Size="10px" GridLines="Vertical" TabIndex="35" Width="900">
                                                    <FooterStyle CssClass="grdfooter" />
                                                    <Columns>
                                                        <asp:TemplateField>
                                                            <HeaderStyle BackColor="#06788B" HorizontalAlign="Left" Width="2%" />
                                                            <HeaderTemplate>
                                                                <asp:CheckBox ID="chkpromAll" TabIndex="11" runat="server" />
                                                            </HeaderTemplate>
                                                            <ItemStyle HorizontalAlign="Left" Width="2%" />
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="chkpromdet" runat="server" Width="10px" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="From Date" Visible="true">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtfromDate" runat="server" CssClass="fiel_input" Width="80px"></asp:TextBox>
                                                                <cc1:CalendarExtender ID="dpFromDate" runat="server" Format="dd/MM/yyyy" PopupButtonID="ImgBtnFrmDt"
                                                                    PopupPosition="Right" TargetControlID="txtfromDate">
                                                                </cc1:CalendarExtender>
                                                                <cc1:MaskedEditExtender ID="MeFromDate" runat="server" Mask="99/99/9999" MaskType="Date"
                                                                    TargetControlID="txtfromDate">
                                                                </cc1:MaskedEditExtender>
                                                                <asp:ImageButton ID="ImgBtnFrmDt" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.png"
                                                                    TabIndex="-1" />
                                                                <br />
                                                                <cc1:MaskedEditValidator ID="MevFromDate" runat="server" ControlExtender="MeFromDate"
                                                                    ControlToValidate="txtfromDate" CssClass="field_error" Display="Dynamic" EmptyValueBlurredText="Date is required"
                                                                    EmptyValueMessage="Date is required" ErrorMessage="MeFromDate" InvalidValueBlurredMessage="Invalid Date"
                                                                    InvalidValueMessage="Invalid Date" TooltipMessage="Input a Date in Date/Month/Year">
                                                                </cc1:MaskedEditValidator>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                                                            <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="To Date" Visible="true">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtToDate" runat="server" CssClass="fiel_input" Width="80px"></asp:TextBox>
                                                                <cc1:CalendarExtender ID="dpToDate" runat="server" Format="dd/MM/yyyy" PopupButtonID="ImgBtnToDt"
                                                                    PopupPosition="Right" TargetControlID="txtToDate">
                                                                </cc1:CalendarExtender>
                                                                <cc1:MaskedEditExtender ID="MeToDate" runat="server" Mask="99/99/9999" MaskType="Date"
                                                                    TargetControlID="txtToDate">
                                                                </cc1:MaskedEditExtender>
                                                                <asp:ImageButton ID="ImgBtnToDt" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.png"
                                                                    TabIndex="-1" />
                                                                <br />
                                                                <cc1:MaskedEditValidator ID="MevToDate" runat="server" ControlExtender="MeToDate"
                                                                    ControlToValidate="txtToDate" CssClass="field_error" Display="Dynamic" EmptyValueBlurredText="Date is required"
                                                                    EmptyValueMessage="Date is required" ErrorMessage="MeToDate" InvalidValueBlurredMessage="Invalid Date"
                                                                    InvalidValueMessage="Invalid Date" TooltipMessage="Input a Date in Date/Month/Year">
                                                                </cc1:MaskedEditValidator>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                                                            <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Booking Code">
                                                            <ItemStyle HorizontalAlign="Left" />
                                                            <HeaderStyle HorizontalAlign="Left" Width="60px" />
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txthotelbookingcode" runat="server" CssClass="field_input" Style="text-align: left"
                                                                    Width="80px"></asp:TextBox>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                                                            <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                                        </asp:TemplateField>
                                                         <asp:TemplateField HeaderText="Select RoomType">
                                                            <FooterTemplate>
                                                                &nbsp;
                                                            </FooterTemplate>
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtrmtypname" runat="server" Enabled="false" CssClass="field_input"
                                                                    Width="160px"></asp:TextBox>

                                                                     <asp:TextBox ID="txtuprmtypname" runat="server"  CssClass="displaynone"  
                                                                    Width="160px"></asp:TextBox>
                                                                    
                                                                      <asp:TextBox ID="txtrmcombination" runat="server"  CssClass="displaynone"
                                                                                                   Width="150px"></asp:TextBox>

                                                                <asp:Button ID="btnrmtypnosow" runat="server" CssClass="btn" TabIndex="14" Text=".."
                                                                    Width="20px" OnClick="btnrmtypnoshow_Click"  />
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                                                            <ItemStyle HorizontalAlign="Left" Width="180px" Wrap="False" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Meal Plan">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtmealcode" runat="server" Enabled="false" CssClass="field_input"
                                                                    Width="70px"></asp:TextBox>
                                                                        <asp:TextBox ID="txtupmealcode" runat="server" CssClass="displaynone"
                                                                    Width="160px"></asp:TextBox>
                                                                      <asp:TextBox ID="txtmealcombination" runat="server"  CssClass="displaynone"
                                                                                                   Width="150px"></asp:TextBox>
                                                                <asp:Button ID="btnmealnoshow" runat="server" CssClass="btn" 
                                                                    TabIndex="14" Text=".." Width="20px" OnClick="btnmealnoshow_Click" />
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                                                            <ItemStyle HorizontalAlign="Left" Width="100px" Wrap="False" />
                                                        </asp:TemplateField>
                                                         <asp:TemplateField HeaderText="Accom.Category">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtrmcatcode" runat="server" Enabled="false" CssClass="field_input"
                                                                    Width="70px"></asp:TextBox>
                                                                        <asp:TextBox ID="txtuprmcatcode" runat="server" Visible="false"  CssClass="field_input"
                                                                    Width="160px"></asp:TextBox>
                                                                <asp:Button ID="btnrmcatshow" runat="server" CssClass="btn" 
                                                                    TabIndex="14" Text=".." Width="20px" OnClick="btnaccrmcatcode_Click" />
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                                                            <ItemStyle HorizontalAlign="Left" Width="100px" Wrap="False" />
                                                        </asp:TemplateField>
                                                         <asp:TemplateField HeaderText="MealSupp Category">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtmealrmcatcode" runat="server" Enabled="false" CssClass="field_input"
                                                                    Width="70px"></asp:TextBox>
                                                                        <asp:TextBox ID="txtupmealrmcatcode" runat="server" Visible="false"  CssClass="field_input"
                                                                    Width="160px"></asp:TextBox>
                                                                <asp:Button ID="btnmealrmcatshow" runat="server" CssClass="btn" 
                                                                    TabIndex="14" Text=".." Width="20px" OnClick="btnmealrmcatshow_Click" />
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                                                            <ItemStyle HorizontalAlign="Left" Width="100px" Wrap="False" />
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText="Discount Type" Visible="false">
                                                            <ItemStyle HorizontalAlign="Left" />
                                                            <HeaderStyle HorizontalAlign="Left" Width="80px" />
                                                            <ItemTemplate>
                                                                <asp:DropDownList ID="ddloptions" runat="server" AutoPostBack="true" OnSelectedIndexChanged="SelectedItemchange">
                                                                    <asp:ListItem Text="Percentage" Value="0"></asp:ListItem>
                                                                    <asp:ListItem Text="Value" Value="1"></asp:ListItem>
                                                                    <%--<asp:ListItem Text="Multiple Rooms" Value="2"></asp:ListItem>--%>
                                                                </asp:DropDownList>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Discount % or Value" Visible="false">
                                                            <ItemStyle HorizontalAlign="Left" />
                                                            <HeaderStyle HorizontalAlign="Left" Width="60px" />
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtdiscount" runat="server" CssClass="field_input" Style="text-align: left"
                                                                    Width="80px"></asp:TextBox>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Additional Discount % Or Value" Visible="false">
                                                            <ItemStyle HorizontalAlign="Left" />
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtadddiscount" runat="server" CssClass="field_input" Style="text-align: left"
                                                                    Width="80px"></asp:TextBox>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="No.Of Rooms" Visible="false">
                                                            <ItemStyle HorizontalAlign="Left" />
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtnoofrooms" runat="server" CssClass="field_input" Style="text-align: left"
                                                                    Width="100px"></asp:TextBox>
                                                                <asp:Button ID="btncombination" runat="server" CssClass="btn" OnClick="btncombination_Click"
                                                                    Text=".." Width="20px" />
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Multiples Yes/No " Visible="false">
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="chkmultiyes" runat="server" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Booking Validity Options">
                                                            <ItemStyle HorizontalAlign="Left" />
                                                            <HeaderStyle HorizontalAlign="Left" Width="80px" />
                                                            <ItemTemplate>
                                                                <asp:DropDownList ID="ddlbookoptions" runat="server" AutoPostBack="true" OnSelectedIndexChanged="BookingoptionSelectedchange">
                                                                    <asp:ListItem Text="Book By" Value="0"></asp:ListItem>
                                                                    <asp:ListItem Text="Book before days from check in" Value="1"></asp:ListItem>
                                                                    <asp:ListItem Text="Book before months from check in" Value="2"></asp:ListItem>
                                                                    <asp:ListItem Text="Range of dates" Value="3"></asp:ListItem>
                                                                    <asp:ListItem Text="No Booking Validity" Value="4"></asp:ListItem>
                                                                     <asp:ListItem Text="Book By and book before days from check in" Value="5"></asp:ListItem>
                                                                    
                                                                </asp:DropDownList>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Booking Validity From/Book By">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtbookfromDate" runat="server" CssClass="fiel_input" Width="80px"></asp:TextBox>
                                                                <cc1:CalendarExtender ID="dpBookFromDate" runat="server" Format="dd/MM/yyyy" PopupButtonID="ImgBtnBookFrmDt"
                                                                    PopupPosition="Right" TargetControlID="txtbookfromDate">
                                                                </cc1:CalendarExtender>
                                                                <cc1:MaskedEditExtender ID="MeBookFromDate" runat="server" Mask="99/99/9999" MaskType="Date"
                                                                    TargetControlID="txtbookfromDate">
                                                                </cc1:MaskedEditExtender>
                                                                <asp:ImageButton ID="ImgBtnBookFrmDt" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.png"
                                                                    TabIndex="-1" />
                                                                <cc1:MaskedEditValidator ID="MevBookFromDate" runat="server" ControlExtender="MeBookFromDate"
                                                                    ControlToValidate="txtbookfromDate" CssClass="field_error" Display="Dynamic"
                                                                    EmptyValueBlurredText="Date is required" EmptyValueMessage="Date is required"
                                                                    ErrorMessage="MeBookFromDate" InvalidValueBlurredMessage="Invalid Date" InvalidValueMessage="Invalid Date"
                                                                    TooltipMessage="Input a Date in Date/Month/Year">
                                                                </cc1:MaskedEditValidator>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Booking Validity To">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtBookToDate" runat="server" CssClass="fiel_input" Width="80px"></asp:TextBox>
                                                                <cc1:CalendarExtender ID="dpBookToDate" runat="server" Format="dd/MM/yyyy" PopupButtonID="ImgBtnBookToDt"
                                                                    PopupPosition="Right" TargetControlID="txtBookToDate">
                                                                </cc1:CalendarExtender>
                                                                <cc1:MaskedEditExtender ID="MeBookToDate" runat="server" Mask="99/99/9999" MaskType="Date"
                                                                    TargetControlID="txtBookToDate">
                                                                </cc1:MaskedEditExtender>
                                                                <asp:ImageButton ID="ImgBtnBookToDt" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.png"
                                                                    TabIndex="-1" />
                                                                <br />
                                                                <cc1:MaskedEditValidator ID="MevBookToDate" runat="server" ControlExtender="MeBookToDate"
                                                                    ControlToValidate="txtBookToDate" CssClass="field_error" Display="Dynamic" EmptyValueBlurredText="Date is required"
                                                                    EmptyValueMessage="Date is required" ErrorMessage="MeBookToDate" InvalidValueBlurredMessage="Invalid Date"
                                                                    InvalidValueMessage="Invalid Date" TooltipMessage="Input a Date in Date/Month/Year">
                                                                </cc1:MaskedEditValidator>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Booking Validity Days/Months">
                                                            <ItemStyle HorizontalAlign="Left" />
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtbookdays" runat="server" CssClass="field_input" Style="text-align: center"
                                                                    Width="80px"></asp:TextBox>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Min.Nights">
                                                            <ItemStyle HorizontalAlign="Left" />
                                                            <HeaderStyle HorizontalAlign="Left" Width="60px" />
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtminnights" runat="server" CssClass="field_input" Style="text-align: center"
                                                                    Width="80px"></asp:TextBox>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:TemplateField>
                                                         <asp:TemplateField HeaderText="Max.Nights">
                                                            <ItemStyle HorizontalAlign="Left" />
                                                            <HeaderStyle HorizontalAlign="Left" Width="60px" />
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtmaxnights" runat="server" CssClass="field_input" Style="text-align: center"
                                                                    Width="80px"></asp:TextBox>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:TemplateField>

                                                         <asp:TemplateField HeaderText="Apply To">
                                                            <ItemStyle HorizontalAlign="Left" />
                                                            <HeaderStyle HorizontalAlign="Left" Width="100px" />
                                                            <ItemTemplate>
                                                               <asp:DropDownList ID="ddlapply" runat="server"  >
                                                                    <asp:ListItem Text="Every slab of stay" Value="0"></asp:ListItem>
                                                                    <asp:ListItem Text="Beginning of stay" Value="1"></asp:ListItem>
                                                                    <asp:ListItem Text="End of stay" Value="2"></asp:ListItem>
                                                                    <asp:ListItem Text="Cheapest Rate" Value="3"></asp:ListItem>
                                                                </asp:DropDownList>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:TemplateField>

                                                             <asp:TemplateField HeaderText="Allow Mutli Stay"  >
                                                            <ItemStyle HorizontalAlign="Left" />
                                                            <HeaderStyle HorizontalAlign="Left" Width="60px" />
                                                            <ItemTemplate>
                                                                     <asp:CheckBox ID="chkmultiples" runat="server" />
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:TemplateField>

                                                         <asp:TemplateField HeaderText="Stay For">
                                                            <ItemStyle HorizontalAlign="Left" />
                                                            <HeaderStyle HorizontalAlign="Left" Width="60px" />
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtstayfor" runat="server"  Style="text-align: center"  Width="50px"></asp:TextBox>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:TemplateField>

                                                         <asp:TemplateField HeaderText="Pay For">
                                                            <ItemStyle HorizontalAlign="Left" />
                                                            <HeaderStyle HorizontalAlign="Left" Width="60px" />
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtpayfor" runat="server"  Style="text-align: center"  Width="50px"></asp:TextBox>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:TemplateField>

                                                          <asp:TemplateField HeaderText="Max FreeNights">
                                                            <ItemStyle HorizontalAlign="Left" />
                                                            <HeaderStyle HorizontalAlign="Left" Width="60px" />
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtmaxfreents" runat="server"  Style="text-align: center"  Width="50px"></asp:TextBox>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:TemplateField>

                                                         <asp:TemplateField HeaderText="Max Multiples">
                                                            <ItemStyle HorizontalAlign="Left" />
                                                            <HeaderStyle HorizontalAlign="Left" Width="60px" />
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtmaxmultiples" runat="server"  Style="text-align: center"  Width="50px"></asp:TextBox>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText="Action">
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="imgStayAdd" runat="server" ImageUrl="~/Images/PlusGreen.ico"
                                                                    Width="18px" OnClick="imgStayAdd_Click" />
                                                                <asp:ImageButton ID="imgSclose" runat="server" ImageUrl="~/Images/crystaltoolbar/DeleteRed.png"
                                                                    Width="18px" OnClick="imgSclose_Click" ToolTip="Delete Current Row" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <RowStyle CssClass="grdRowstyle" />
                                                    <SelectedRowStyle CssClass="grdselectrowstyle" />
                                                    <PagerStyle CssClass="grdpagerstyle" HorizontalAlign="Center" />
                                                    <HeaderStyle CssClass="grdheader" />
                                                    <AlternatingRowStyle CssClass="grdAternaterow" />
                                                </asp:GridView>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                            </td>
                                            <td>
                                                <br />
                                                <asp:Button ID="btnaddrow" runat="server" CssClass="btn" TabIndex="36" Text="Add Row"
                                                    Style="display: none" />
                                                <asp:Button ID="btndelrow" runat="server" CssClass="btn" TabIndex="37" Text="Delete Row"
                                                    Style="display: none" />
                                                <asp:Button ID="btncopyrow" runat="server" CssClass="btn" TabIndex="38" Text="Copy Row" />
                                                <asp:Button ID="btnseason" runat="server" CssClass="btn" TabIndex="38" Text="Select Season"
                                                    OnClick="btnselect_Click" />
                                                <br />
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                        </td>
                    </tr>
                    <tr>
                       <td align="left" valign="top" width="150">
                            &nbsp;
                        </td>
                        <td colspan="20">
                            <asp:UpdatePanel ID="UpdatePanelsupin" runat="server">
                                <ContentTemplate>
                                    <table>
                                        <tr>
                                            
                                            <td valign="top" style="display:none" >
                                                <div id="div4" runat="server" style="max-height: 250px; overflow: auto;">
                                                    <asp:GridView ID="gridrmtype" runat="server" AutoGenerateColumns="False" BackColor="White"
                                                        BorderColor="#999999" BorderStyle="None" BorderWidth="1px" Caption="Room Type"
                                                        CellPadding="3" CssClass="td_cell" Font-Bold="true" Font-Size="12px" GridLines="Vertical"
                                                        TabIndex="31" Width="400px">
                                                        <FooterStyle CssClass="grdfooter" />
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="rmtype" Visible="false">
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemTemplate>
                                                                    <asp:Label ID="txtrmtypcode" runat="server" Text='<%# Bind("rmtypcode") %>'></asp:Label>
                                                                    <asp:Label ID="lblrankorder" runat="server" Text='<%# Bind("rankord") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField>
                                                                <HeaderStyle BackColor="#06788B" HorizontalAlign="Left" Width="2%" />
                                                                <HeaderTemplate>
                                                                    <asp:CheckBox ID="chkrmtypAll" TabIndex="12" runat="server" />
                                                                </HeaderTemplate>
                                                                <ItemStyle HorizontalAlign="Left" Width="2%" />
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chkrmtype" runat="server" Width="10px" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="rmtypname" HeaderText="Room Type" SortExpression="rmtypname">
                                                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                                                                <HeaderStyle BackColor="#06788B" HorizontalAlign="Left" Width="150px" />
                                                            </asp:BoundField>
                                                            <asp:TemplateField HeaderText="Upgrade To" Visible ="false"  >
                                                                <ItemTemplate>
                                                                    <select id="ddlUpgradermtyp" runat="server" class="drpdown" style="width: 180px">
                                                                        <option selected=""></option>
                                                                    </select>
                                                                    <asp:HiddenField ID="hdnrmtypcode" runat="server" Value='<%# Bind("rmtypcode") %>' />
                                                                </ItemTemplate>
                                                                <HeaderStyle Wrap="true" />
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
                                            <td valign="top" style="display:none"  >
                                                <div id="div5" runat="server" style="max-height: 250px; overflow: auto;">
                                                    <asp:GridView ID="grdmealplan" runat="server" AutoGenerateColumns="False" BackColor="White"
                                                        BorderColor="#999999" BorderStyle="None" BorderWidth="1px" Caption="Meal" CellPadding="3"
                                                        CssClass="td_cell" Font-Bold="true" Font-Size="12px" GridLines="Vertical" TabIndex="32"
                                                        Width="300px">
                                                        <FooterStyle CssClass="grdfooter" />
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="mealplan" Visible="false">
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemTemplate>
                                                                    <asp:Label ID="txtmealcode" runat="server" Text='<%# Bind("mealcode") %>'></asp:Label>
                                                                    <asp:Label ID="lblrankorder" runat="server" Text='<%# Bind("rankorder") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField>
                                                                <HeaderStyle BackColor="#06788B" HorizontalAlign="Left" Width="2%" />
                                                                <HeaderTemplate>
                                                                    <asp:CheckBox ID="chkmealAll" runat="server" TabIndex="13" />
                                                                </HeaderTemplate>
                                                                <ItemStyle HorizontalAlign="Left" Width="2%" />
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chkmeal" runat="server" Width="10px" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="mealname" HeaderText="Meal Plan" SortExpression="mealname">
                                                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                                                                <HeaderStyle BackColor="#06788B" HorizontalAlign="Left" Width="150px" />
                                                            </asp:BoundField>
                                                            <asp:TemplateField HeaderText="Upgrade To" Visible="false">
                                                                <ItemTemplate>
                                                                    <select id="ddlUpgrademeal" runat="server" class="drpdown" style="width: 100px">
                                                                        <option selected=""></option>
                                                                    </select>
                                                                    <asp:HiddenField ID="hdnmealcode" runat="server" Value='<%# Bind("mealcode") %>' />
                                                                </ItemTemplate>
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
                                            <td valign="top" style="display:none"  >
                                                <div id="div6" runat="server" style="max-height: 250px; overflow: auto;">
                                                    <asp:GridView ID="grdrmcat" runat="server" AutoGenerateColumns="False" BackColor="White"
                                                        BorderColor="#999999" BorderStyle="None" BorderWidth="1px" Caption="Accomodation"
                                                        CellPadding="3" CssClass="td_cell" Font-Bold="true" Font-Size="12px" GridLines="Vertical"
                                                        TabIndex="33" Width="200px">
                                                        <FooterStyle CssClass="grdfooter" />
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="mealplan" Visible="false">
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemTemplate>
                                                                    <asp:Label ID="txtrmcatcode" runat="server" Text='<%# Bind("rmcatcode") %>'></asp:Label>
                                                                    <asp:Label ID="lblrankorder" runat="server" Text='<%# Bind("units") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField>
                                                                <HeaderStyle BackColor="#06788B" HorizontalAlign="Left" Width="2%" />
                                                                <HeaderTemplate>
                                                                    <asp:CheckBox ID="chkrmcatAll" runat="server" TabIndex="14" />
                                                                </HeaderTemplate>
                                                                <ItemStyle HorizontalAlign="Left" Width="2%" />
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chkrmcat" runat="server" Width="10px" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="rmcatcode" HeaderText="Accomodation" SortExpression="rmcatcode">
                                                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                                                                <HeaderStyle BackColor="#06788B" HorizontalAlign="Left" Width="75px" />
                                                            </asp:BoundField>
                                                            <asp:TemplateField HeaderText="Upgrade To" Visible="false">
                                                                <ItemTemplate>
                                                                    <select id="ddlUpgradermcat" runat="server" class="drpdown" style="width: 100px">
                                                                        <option selected=""></option>
                                                                    </select>
                                                                    <asp:HiddenField ID="hdnrmcatcode" runat="server" Value='<%# Bind("rmcatcode") %>' />
                                                                </ItemTemplate>
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
                                            <td valign="top" style="display:none"  >
                                                <div id="div7" runat="server" style="max-height: 250px; overflow: auto;">
                                                    <asp:GridView ID="grdsupplement" runat="server" AutoGenerateColumns="False" BackColor="White"
                                                        BorderColor="#999999" BorderStyle="None" BorderWidth="1px" Caption="Meal Supplements"
                                                        CellPadding="3" CssClass="td_cell" Font-Bold="true" Font-Size="12px" GridLines="Vertical"
                                                        TabIndex="34" Width="100px">
                                                        <FooterStyle CssClass="grdfooter" />
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="mealplan" Visible="false">
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblrmcatcode" runat="server" Text='<%# Bind("rmcatcode") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField>
                                                                <HeaderStyle BackColor="#06788B" HorizontalAlign="Left" Width="2%" />
                                                                <HeaderTemplate>
                                                                    <asp:CheckBox ID="chksuppAll" runat="server" TabIndex="15" />
                                                                </HeaderTemplate>
                                                                <ItemStyle HorizontalAlign="Left" Width="2%" />
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chksuprmcat" runat="server" Width="10px" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="rmcatcode" HeaderText="Meal Supplements" SortExpression="rmcatcode">
                                                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                                                                <HeaderStyle BackColor="#06788B" HorizontalAlign="Left" Width="75px" />
                                                            </asp:BoundField>
                                                            <asp:TemplateField HeaderText="Upgrade To" Visible="false">
                                                                <ItemTemplate>
                                                                    <select id="ddlUpgradermcatsup" runat="server" class="drpdown" style="width: 75px">
                                                                        <option selected=""></option>
                                                                    </select>
                                                                    <asp:HiddenField ID="hdnrmcatcodesup" runat="server" Value='<%# Bind("rmcatcode") %>' />
                                                                </ItemTemplate>
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
                                    </table>
                                    <div id="divinv" runat="server" style="width: 100%; min-height: 25px">
                                        <table>
                                            <tr>
                                                <td align="left" valign="top">
                                                    Inventory&nbsp;Type
                                                </td>
                                                <td align="left" valign="top">
                                                    <select id="ddlinventorytype" runat="server" class="drpdown" tabindex="13">
                                                        <option value="All">All</option>
                                                        <option value="General">General</option>
                                                        <option value="Free Sale">Free Sale</option>
                                                        <option value="Financial">Financial</option>
                                                        <option value="B2B">B2B</option>
                                                    </select>
                                                </td>
                                                <td align="left" colspan="2" valign="top">
                                                </td>
                                                <td colspan="3" valign="top">
                                                </td>
                                                <td valign="top">
                                                    <div id="divsplocc" runat="server">
                                                        <table bordercolor="#999999">
                                                            <tr>
                                                                <td valign="top">
                                                                    <asp:Label ID="lblapply" runat="server" Text="Apply To"></asp:Label>
                                                                </td>
                                                                <td colspan="3">
                                                                    <select id="ddlapply" runat="server" class="drpdown" tabindex="14" width="100px">
                                                                        <option selected="selected" value="Every slab of stay">Every slab of stay </option>
                                                                        <option value="Beginning of stay">Beginning of stay</option>
                                                                        <option value="End of stay">End of stay</option>
                                                                    </select>
                                                                </td>
                                                                <td>
                                                                    <asp:CheckBox ID="chkmultiples" runat="server" TabIndex="15" Text="Allow Mutli Stay" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Stay&nbsp;For
                                                                </td>
                                                                <td colspan="2">
                                                                    &nbsp;<asp:TextBox ID="txtstayfor" runat="server" TabIndex="16" Width="50px"></asp:TextBox>
                                                                </td>
                                                                <td colspan="2">
                                                                    Pay&nbsp;For
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtpayfor" runat="server" TabIndex="17" Width="50px"></asp:TextBox>
                                                                    &nbsp;
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="2" valign="top">
                                                                    Max&nbsp;Free&nbsp;Nights
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtmaxfreents" runat="server" TabIndex="18" Width="50px"></asp:TextBox>
                                                                </td>
                                                                <td colspan="2" valign="top">
                                                                    Max&nbsp;Multiples
                                                                </td>
                                                                <td valign="top">
                                                                    <asp:TextBox ID="txtmaxmultiples" runat="server" TabIndex="19" Width="50px"></asp:TextBox>
                                                                    &nbsp;<asp:CheckBox ID="chkinter" runat="server" Style="display: none" Text="Inter Hotels" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" colspan="2" valign="top">
                                                    Combine&nbsp;
                                                    <select id="ddlcombine" runat="server" class="drpdown" tabindex="20" width="150px">
                                                        <option selected="selected" value="Not Combinable">Not Combinable</option>
                                                        <option value="Combinable with All">Combinable with All</option>
                                                        <option value="Combinable with Specific">Combinable with Specific</option>
                                                        <option value="Combinable with only Contracts">Combinable with only Contracts</option>
                                                        <option value="Combine Mandatory With">Combine Mandatory With</option>
                                                    </select>
                                                </td>
                                                <td colspan="3" valign="top">
                                                    <div id="divcombine" runat="server" style="max-height: 250px; overflow: auto;">
                                                        <asp:GridView ID="grdcombinable" runat="server" AutoGenerateColumns="False" BackColor="White"
                                                            BorderColor="#999999" BorderStyle="None" BorderWidth="1px" CellPadding="3" CssClass="td_cell"
                                                            Font-Bold="true" Font-Size="12px" GridLines="Vertical" TabIndex="13" Width="250px">
                                                            <FooterStyle CssClass="grdfooter" />
                                                            <Columns>
                                                                <asp:TemplateField>
                                                                    <HeaderStyle BackColor="#06788B" HorizontalAlign="Left" Width="2%" />
                                                                    <HeaderTemplate>
                                                                        <asp:CheckBox ID="chkcombineAll" runat="server" />
                                                                    </HeaderTemplate>
                                                                    <ItemStyle HorizontalAlign="Left" Width="2%" />
                                                                    <ItemTemplate>
                                                                        <asp:CheckBox ID="chkcombine" runat="server" Width="10px" />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Promotion Name">
                                                                    <EditItemTemplate>
                                                                        &nbsp;
                                                                    </EditItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Left" Width="250px" />
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtpromotioncode" runat="server" Style="display: none"></asp:TextBox>
                                                                        <asp:TextBox ID="txtpromotionname" runat="server" AutoPostBack="True" CssClass="field_input"
                                                                             Width="250px"></asp:TextBox>
                                                                        <asp:AutoCompleteExtender ID="AutoCompleteExtender4" runat="server" CompletionInterval="10"
                                                                            CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                                            CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="1" DelimiterCharacters=""
                                                                            EnableCaching="false" Enabled="True" FirstRowSelected="false" MinimumPrefixLength="0"
                                                                            OnClientItemSelected="promonautocompleteselected" ServiceMethod="Getpromotionlist"
                                                                            TargetControlID="txtpromotionname" >
                                                                        </asp:AutoCompleteExtender>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                            </Columns>
                                                            <RowStyle CssClass="grdRowstyle" />
                                                            <SelectedRowStyle CssClass="grdselectrowstyle" />
                                                            <PagerStyle CssClass="grdpagerstyle" HorizontalAlign="Center" />
                                                            <HeaderStyle CssClass="grdheader" />
                                                            <AlternatingRowStyle CssClass="grdAternaterow" />
                                                        </asp:GridView>
                                                        <br />
                                                        <asp:Button ID="btnaddcombine" runat="server" CssClass="btn" TabIndex="21" Text="Add Row" />
                                                        <asp:Button ID="btndelcombine" runat="server" CssClass="btn" TabIndex="22" Text="Delete Row" />
                                                        <br />
                                                    </div>
                                                </td>
                                                <td align="left" colspan="2" valign="top">
                                                 <div id="divcomm" runat ="server">
                                                    Commission&nbsp;
                                                    <select id="ddlcommission" runat="server" class="drpdown" tabindex="20" width="150px">
                                                        <option selected="selected" value="Not Commissionable">Not Commissionable</option>
                                                        <option value="Commissionable As per contract">Commissionable As per contract</option>
                                                        <option value="Special commissionable Rates">Special commissionable Rates</option>
                                                    </select>
                                                    </div>
                                                </td>
                                                <td>
                                                <div id="divapplydiscount" runat="server">
                                                <table>
                                                <tr>
                                                <td align="left" colspan="2" valign="top">
                                                    Apply&nbsp;Offer&nbsp;To&nbsp;
                                                    <select id="ddlapplydiscount" runat="server" class="drpdown" tabindex="21" width="75px">
                                                        <option selected="selected" value="None">None</option>
                                                        <option  value="Contracts">Contracts</option>
                                                        <option value="Offers">Offers</option>
                                                        <option value="Both">Both</option>
                                                    </select>
                                                </td>
                                                <td align="left" colspan="2" valign="top">

                                                     <asp:Button ID="btnselectcontract" runat="server" CssClass="btn" TabIndex="22" Text="Select" />
                                                      <asp:HiddenField ID="hdMarkUp" runat="server" />
                                                       <asp:HiddenField ID="hdnapplydiscount" runat="server" />
                                                      <cc1:ModalPopupExtender ID="ModalSelectMarkup" runat="server" BehaviorID="ModalSelectMarkup"
                                                        CancelControlID="TdMarkupClose" TargetControlID="hdMarkUp" PopupControlID="dvMarkupPopup"
                                                        PopupDragHandleControlID="PopupMarkupHeader" Drag="true" BackgroundCssClass="ModalPopupBG">
                                                    </cc1:ModalPopupExtender>
                                                     <div id="dvMarkupPopup" runat="server" style="height: 470px; width: 650px; border: 3px solid #06788B;
                                                        background-color: White;">
                                                        <table style="width: 99%; padding: 5px 5px 5px 5px">
                                                            <tr>
                                                                <td id="PopupMarkupHeader" bgcolor="#06788B">
                                                                    <asp:Label ID="Label3" runat="server" CssClass="field_heading" Style="padding: 3px 0px 3px 3px"
                                                                        Text="Apply Discount" Width="205px"></asp:Label>
                                                                </td>
                                                                <td align="center" id="TdMarkupClose" bgcolor="#06788B">
                                                                    <asp:Label ID="Label4" Text="X" runat="server" Font-Bold="True" Font-Names="Verdana"
                                                                        Font-Size="Large" ForeColor="White"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="2">
                                                                    <div style="height: 400px; overflow: auto;">
                                                                        <asp:GridView ID="gvMarkupFormulas" TabIndex="9" runat="server" Font-Size="10px"
                                                                            Width="100%" CssClass="grdstyle" GridLines="Vertical" CellPadding="3" BorderWidth="1px"
                                                                            BorderStyle="Solid" AutoGenerateColumns="False" AllowSorting="True" >
                                                                            <FooterStyle CssClass="grdfooter"></FooterStyle>
                                                                            <Columns>
                                                                                <asp:TemplateField Visible="False" HeaderText="Supplier Code">
                                                                                        <EditItemTemplate>
                                                                                        </EditItemTemplate>
                                                                                        <ItemTemplate>
                                                                                          
                                                                                            <asp:Label ID="lblcontract" runat="server" Text='<%# Bind("contractid") %>' __designer:wfdid="w1"></asp:Label>
                                                                                            <asp:Label ID="lblpromotionid" runat="server" Text='<%# Bind("promotionid") %>' __designer:wfdid="w1"></asp:Label>
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                                      <asp:TemplateField HeaderText="Select">
                                                                                    <ItemTemplate>
                                                                                        <asp:CheckBox ID="chkmarkupSelect" runat="server" />
                                                                                        
                                                                                    </ItemTemplate>
                                                                                     <ItemStyle HorizontalAlign="Center"  />
                                                                                </asp:TemplateField>
                                                                                    <asp:BoundField DataField="Contractid" SortExpression="Contractid" HeaderText="Contract ID" HeaderStyle-HorizontalAlign="Left"   >
                                                                                    </asp:BoundField>
                                                                                 
                                                                                     <asp:BoundField DataField="promotionid" SortExpression="promotionid" HeaderText="Promotion ID" HeaderStyle-HorizontalAlign="Left">
                                                                                    </asp:BoundField>

                                                                                    <asp:BoundField DataField="promotionname" SortExpression="promotionname" HeaderText="Promotion Name" HeaderStyle-HorizontalAlign="Left">
                                                                                    </asp:BoundField>

                                                                                     <asp:BoundField DataField="Titlename" SortExpression="Titlename" HeaderText="Title" HeaderStyle-HorizontalAlign="Left">
                                                                                    </asp:BoundField>
                                  
                                                                                    <asp:BoundField DataField="Period" SortExpression="Period" HeaderText="Date Period" HeaderStyle-HorizontalAlign="Left">
                                                                                    </asp:BoundField>
                                                                                  

                                                                                      <asp:TemplateField HeaderText="Applicable To" HeaderStyle-HorizontalAlign="Left">
                                                                                            <ItemTemplate>
                                                           
                                                                                        <asp:Label ID="lblapplicable" runat="server"    Text='<%# Eval("applicableto")%>'  ></asp:Label>
                                                                                      
                                                                                        </ItemTemplate>
                                                         
                                                                                    </asp:TemplateField>

                                                                            </Columns>
                                                                            <RowStyle CssClass="grdRowstyle"></RowStyle>
                                                                            <SelectedRowStyle CssClass="grdselectrowstyle"></SelectedRowStyle>
                                                                            <PagerStyle CssClass="grdpagerstyle" HorizontalAlign="Center"></PagerStyle>
                                                                            <HeaderStyle CssClass="grdheader" ForeColor="white"></HeaderStyle>
                                                                            <AlternatingRowStyle CssClass="grdAternaterow"></AlternatingRowStyle>
                                                                        </asp:GridView>
                                                                    </div>
                                                                </td>
                                                            </tr>
                                                             <tr>
                                                                <td align="center" colspan="2" style="padding-top: 5px">
                                                                   
                                                                    <asp:Button ID="btnokcontract" runat="server" CssClass="btn" Text="OK" />
                                                                   
                                                                </td>
                                                            </tr>
                                                            
                                                        </table>
                                                    </div>
                                                </td>
                                                </tr>
                                                </table>
                                                </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                       <td align="left" valign="top" width="150">
                            &nbsp;
                        </td>
                        <td colspan="10">
                            <asp:UpdatePanel ID="Updatedis" runat="server">
                                <ContentTemplate>
                                    <div style="width: 100%; min-height: 25px" id="div12" runat="server">
                                        <table>
                                            <tr>
                                                <td align="left" valign="top">
                                                    <asp:CheckBox ID="chkrefund" runat="server" Text="Non-Refundable" TabIndex="23" />
                                                </td>
                                                <td align="left" valign="top">
                                                    <asp:CheckBox ID="chkdiscount" runat="server" Text="Apply Discount to Exhibition supplement"
                                                        Style="display: none" TabIndex="16" />
                                                </td>
                                              
                                                <td align="center" valign="top" >
                                                    <div style="width: 100%; min-height: 25px" id="divcomptrf" runat="server">
                                                    <table>
                                                    <tr><td align="left" valign="top" colspan="5" >
                                                                <asp:CheckBox ID="chkarrival" runat="server" Text="Arrival Transfer" AutoPostBack="true"
                                                                    TabIndex="24" />
                                                                  <div id="divArrivalTransfer" runat="server" style="max-height: 250px; overflow: auto;">
                                        <asp:GridView ID="grdArrivalTransfer" runat="server" AutoGenerateColumns="False"
                                            BackColor="White" BorderColor="#999999" BorderStyle="None" BorderWidth="1px"
                                            CellPadding="3" CssClass="td_cell" Font-Bold="true" Font-Size="12px" GridLines="Vertical"
                                            TabIndex="13" Width="250px">
                                            <FooterStyle CssClass="grdfooter" />
                                            <Columns>
                                              
                                                <asp:TemplateField HeaderText=" ">
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkArrivalTerminal" runat="server" CssClass="field_input"></asp:CheckBox>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Arrival Airport">
                                                    <EditItemTemplate>
                                                        &nbsp;
                                                    </EditItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" Width="250px" />
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtarrivalterminal" runat="server" Style="display: none"></asp:TextBox>
                                                        <asp:TextBox ID="txtarrivalAirportName" runat="server" CssClass="field_input" AutoPostBack="True"
                                                            onkeyup="SetContextKeyValue()" Width="250px"></asp:TextBox>
                                                        <asp:AutoCompleteExtender ID="arrivalterminal_AutoCompleteExtender" runat="server"
                                                            CompletionInterval="10" CompletionListCssClass="autocomplete_completionListElement"
                                                            CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem" CompletionListItemCssClass="autocomplete_listItem"
                                                            CompletionSetCount="1" DelimiterCharacters="" EnableCaching="false" Enabled="True"
                                                            FirstRowSelected="false" MinimumPrefixLength="0" OnClientItemSelected="arrivalautocompleteselected"
                                                            ServiceMethod="Getairportlist" TargetControlID="txtarrivalAirportName" UseContextKey="true">
                                                        </asp:AutoCompleteExtender>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                  <asp:TemplateField HeaderText="Flight">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtflightcode" runat="server" Enabled="false" CssClass="field_input"
                                                            Width="70px"></asp:TextBox>
                                                        <asp:Button ID="btnflight" runat="server" CssClass="btn" OnClick="btnflight_Click" TabIndex="14"
                                                            Text=".." Width="20px" />
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                                                    <ItemStyle HorizontalAlign="Left" Width="100px" Wrap="False" />
                                                </asp:TemplateField>
                                            </Columns>
                                            <RowStyle CssClass="grdRowstyle" />
                                            <SelectedRowStyle CssClass="grdselectrowstyle" />
                                            <PagerStyle CssClass="grdpagerstyle" HorizontalAlign="Center" />
                                            <HeaderStyle CssClass="grdheader" />
                                            <AlternatingRowStyle CssClass="grdAternaterow" />
                                        </asp:GridView>
                                        <br />
                                        <asp:Button ID="btnaddArrival" runat="server" CssClass="btn" TabIndex="7" Text="Add Row" />
                                        <asp:Button ID="btndelArrival" runat="server" CssClass="btn" TabIndex="10" Text="Delete Row" />
                                        <br />
                                    </div>
                                                        </td>
                                                        <td align="left" valign="top" colspan ="4">
                                                        <asp:CheckBox ID="chkdeparture" runat="server" Text="Departure Transfer" AutoPostBack="true"
                                                            TabIndex="25" />
                                                             <div id="divDepartureTransfer" runat="server" style="max-height: 250px; overflow: auto;">
                                        <asp:GridView ID="grdDepartureTransfer" runat="server" AutoGenerateColumns="False"
                                            BackColor="White" BorderColor="#999999" BorderStyle="None" BorderWidth="1px"
                                            CellPadding="3" CssClass="td_cell" Font-Bold="true" Font-Size="12px" GridLines="Vertical"
                                            TabIndex="13" Width="250px">
                                            <FooterStyle CssClass="grdfooter" />
                                            <Columns>
                                              
                                                <asp:TemplateField HeaderText=" ">
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkDepartureTerminal" runat="server" CssClass="field_input"></asp:CheckBox>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Departure Airport">
                                                    <EditItemTemplate>
                                                        &nbsp;
                                                    </EditItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" Width="250px" />
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtdepartureterminal" runat="server" Style="display: none"></asp:TextBox>
                                                        <asp:TextBox ID="txtdepartureAirportName" runat="server" CssClass="field_input" AutoPostBack="True"
                                                            onkeyup="SetContextKeyValue()" Width="250px"></asp:TextBox>
                                                        <asp:AutoCompleteExtender ID="departureterminal_AutoCompleteExtender" runat="server"
                                                            CompletionInterval="10" CompletionListCssClass="autocomplete_completionListElement"
                                                            CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem" CompletionListItemCssClass="autocomplete_listItem"
                                                            CompletionSetCount="1" DelimiterCharacters="" EnableCaching="false" Enabled="True"
                                                            FirstRowSelected="false" MinimumPrefixLength="0" OnClientItemSelected="departureautocompleteselected"
                                                            ServiceMethod="Getairportlist" TargetControlID="txtdepartureAirportName" UseContextKey="true">
                                                        </asp:AutoCompleteExtender>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                  <asp:TemplateField HeaderText="Flight">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtdepflightcode" runat="server" Enabled="false" CssClass="field_input"
                                                            Width="70px"></asp:TextBox>
                                                        <asp:Button ID="btndepflight" runat="server" CssClass="btn" OnClick="btndepflight_Click" TabIndex="14"
                                                            Text=".." Width="20px" />
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                                                    <ItemStyle HorizontalAlign="Left" Width="100px" Wrap="False" />
                                                </asp:TemplateField>
                                            </Columns>
                                            <RowStyle CssClass="grdRowstyle" />
                                            <SelectedRowStyle CssClass="grdselectrowstyle" />
                                            <PagerStyle CssClass="grdpagerstyle" HorizontalAlign="Center" />
                                            <HeaderStyle CssClass="grdheader" />
                                            <AlternatingRowStyle CssClass="grdAternaterow" />
                                        </asp:GridView>
                                        <br />
                                        <asp:Button ID="btnaddDeparture" runat="server" CssClass="btn" TabIndex="7" Text="Add Row" />
                                        <asp:Button ID="btndelDeparture" runat="server" CssClass="btn" TabIndex="10" Text="Delete Row" />
                                        <br />
                                    </div>
                                                        </td>
                                                    </tr>
                                                    </table>
                                                  </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                      <div id="ShowRoomtypes" runat="server" style="overflow: scroll; height: 300px; width: 550px;
                                                            border: 3px solid green; background-color: White; display: none">
                                                            <asp:GridView ID="gv_Showroomtypes" runat="server" AutoGenerateColumns="False" BackColor="White"
                                                                BorderColor="#999999" CssClass="td_cell" Width="450px">
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="rmtypcode" Visible="false">
                                                                        <HeaderStyle HorizontalAlign="Center" />
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="txtrmtypcode" runat="server" Text='<%# Bind("rmtypcode") %>'></asp:Label>
                                                                            <asp:Label ID="lblrmtypname" runat="server" Text='<%# Bind("rmtypname") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField>
                                                                        <HeaderStyle HorizontalAlign="Left" BackColor="#06788B" Width="4%" />
                                                                        <HeaderTemplate >
                                                                            <asp:CheckBox runat="server" ID="chkrmtypeAll"  />
                                                                        </HeaderTemplate>
                                                                        <ItemStyle HorizontalAlign="Left" Width="2%"></ItemStyle>
                                                                        <ItemTemplate>
                                                                            <asp:CheckBox runat="server" ID="chkrmtype" Width="10px" />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:BoundField DataField="rmtypcode" SortExpression="rmtypcode" HeaderText="RoomType Code"
                                                                        Visible="false">
                                                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top"></ItemStyle>
                                                                        <HeaderStyle BackColor="#06788B" HorizontalAlign="Left" Width="80px"></HeaderStyle>
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="rmtypname" SortExpression="rmtypname" HeaderText="Name">
                                                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top"></ItemStyle>
                                                                        <HeaderStyle BackColor="#06788B" HorizontalAlign="Left" Width="150px"></HeaderStyle>
                                                                    </asp:BoundField>
                                                                     <asp:TemplateField HeaderText="Upgrade To" >
                                                                      <HeaderStyle HorizontalAlign="Left" BackColor="#06788B" Width="4%" />
                                                                        <ItemTemplate>
                                                                            <select id="ddlUpgrade" runat="server" class="drpdown" style="width: 180px">
                                                                                <option selected=""></option>
                                                                            </select>
                                                                            <asp:HiddenField ID="hdnupgradecode" runat="server" Value='<%# Bind("rmtypcode") %>' />
                                                                              <asp:Label ID="lblrankorder" runat="server" style="display:none" Text='<%# Bind("rankord") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle Wrap="true" />
                                                                  </asp:TemplateField>
                                                                </Columns>
                                                                <RowStyle BackColor="#EEEEEE" ForeColor="Black"></RowStyle>
                                                                <SelectedRowStyle BackColor="#008A8C" ForeColor="White" Font-Bold="True"></SelectedRowStyle>
                                                                <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center"></PagerStyle>
                                                                <HeaderStyle BackColor="#454580" ForeColor="White" Font-Bold="True"></HeaderStyle>
                                                                <AlternatingRowStyle BackColor="Transparent" Font-Size="12px"></AlternatingRowStyle>
                                                            </asp:GridView>
                                                            <table style="float: left">
                                                                <tr>
                                                                    <td>
                                                                        <asp:Button ID="btnOk1" runat="server" CssClass="field_button" Text="Ok" Width="80px" />&nbsp;
                                                                    </td>
                                                                    <td>
                                                                        <asp:Button ID="btnmealok" runat="server" CssClass="field_button" Text="Ok" Width="80px"
                                                                            Style="display: none" />&nbsp;
                                                                    </td>
                                                                    <td>
                                                                        <asp:Button ID="btnClear1" runat="server" CssClass="field_button" Text="Close" Width="80px" />&nbsp;
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                            <cc1:ModalPopupExtender ID="ModalExtraPopup" runat="server" BehaviorID="ModalExtraPopup"
                                                                CancelControlID="btnCancelEB" OkControlID="btnOkayEB" TargetControlID="btnInvisibleEBGuest"
                                                                PopupControlID="ShowRoomtypes" PopupDragHandleControlID="PopupHeader" Drag="true"
                                                                BackgroundCssClass="ModalPopupBG">
                                                            </cc1:ModalPopupExtender>
                                                            <cc1:ModalPopupExtender ID="ModalPopupNoshow" runat="server" BehaviorID="ModalPopupNoshow"
                                                                CancelControlID="btnCancelEB" OkControlID="btnOkayEB" TargetControlID="btnInvisibleEBGuest"
                                                                PopupControlID="ShowRoomtypes" PopupDragHandleControlID="PopupHeader" Drag="true"
                                                                BackgroundCssClass="ModalPopupBG">
                                                            </cc1:ModalPopupExtender>
                                                            <input id="btnInvisibleEBGuest" runat="server" type="button" value="Cancel" style="visibility: hidden" />
                                                            <input id="btnOkayEB" type="button" value="OK" style="visibility: hidden" />
                                                            <input id="btnCancelEB" type="button" value="Cancel" style="visibility: hidden" />
                                                        </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" valign="top" width="150">
                            &nbsp;
                        </td>
                        <td valign="top" colspan="10">
                            <asp:UpdatePanel ID="UpdatePanelflight" runat="server">
                                <ContentTemplate>
                                <table>
                                <tr>
                                <td align="left" valign="top">
                                    <div id="divflight" runat="server" style="max-height: 250px; overflow: auto;">
                                        <asp:GridView ID="grdflight" runat="server" AutoGenerateColumns="False" BackColor="White"
                                            BorderColor="#999999" BorderStyle="None" BorderWidth="1px" Caption="Select Flight"
                                            CellPadding="3" CssClass="td_cell" Font-Bold="true" Font-Size="12px" GridLines="Vertical"
                                            TabIndex="19" Width="150px">
                                            <FooterStyle CssClass="grdfooter" />
                                            <Columns>
                                                <asp:TemplateField>
                                                    <HeaderStyle HorizontalAlign="Left" BackColor="#06788B" Width="2%" />
                                                    <HeaderTemplate>
                                                        <asp:CheckBox runat="server" ID="chkflightAll" />
                                                    </HeaderTemplate>
                                                    <ItemStyle HorizontalAlign="Left" Width="2%"></ItemStyle>
                                                    <ItemTemplate>
                                                        <asp:CheckBox runat="server" ID="chkflight" Width="10px" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Flight">
                                                    <EditItemTemplate>
                                                        &nbsp;
                                                    </EditItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" Width="75px" />
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtflightcode" runat="server" CssClass="field_input" Width="150px"></asp:TextBox>
                                                        <asp:AutoCompleteExtender ID="AutoCompleteExtender3" runat="server" CompletionInterval="10"
                                                            CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                            CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="1" DelimiterCharacters=""
                                                            EnableCaching="false" Enabled="True" FirstRowSelected="false" MinimumPrefixLength="0"
                                                            ServiceMethod="Getflightlist" TargetControlID="txtflightcode">
                                                        </asp:AutoCompleteExtender>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <RowStyle CssClass="grdRowstyle" />
                                            <SelectedRowStyle CssClass="grdselectrowstyle" />
                                            <PagerStyle CssClass="grdpagerstyle" HorizontalAlign="Center" />
                                            <HeaderStyle CssClass="grdheader" />
                                            <AlternatingRowStyle CssClass="grdAternaterow" />
                                        </asp:GridView>
                                        <br />
                                        <asp:Button ID="btnaddflight" runat="server" CssClass="btn" TabIndex="20" Text="Add Row" />
                                        <asp:Button ID="btndelflight" runat="server" CssClass="btn" TabIndex="21" Text="Delete Row" />
                                        <br />
                                    </div>
                                </td>
                                <td align="left" valign="top">
                                     <div id="divinter" runat="server" style="max-height: 250px; overflow: auto;">
                                        <asp:GridView ID="grdinterhotel" runat="server" AutoGenerateColumns="False" BackColor="White"
                                            BorderColor="#999999" BorderStyle="None" BorderWidth="1px" Caption="Select InterHotel"
                                            CellPadding="3" CssClass="td_cell" Font-Bold="true" Font-Size="12px" GridLines="Vertical"
                                            TabIndex="22" Width="350px">
                                            <FooterStyle CssClass="grdfooter" />
                                            <Columns>
                                                <asp:TemplateField HeaderText=" ">
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkinterSelect" runat="server" CssClass="field_input"></asp:CheckBox>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Hotel Name">
                                                    <EditItemTemplate>
                                                        &nbsp;
                                                    </EditItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" Width="350px" />
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtinterhotelcode" runat="server" Style="display: none"></asp:TextBox>
                                                        <asp:TextBox ID="txtinterhotelname" runat="server" CssClass="field_input" Width="350px"></asp:TextBox>
                                                        <asp:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server" CompletionInterval="10"
                                                            CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                            CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="1" DelimiterCharacters=""
                                                            EnableCaching="false" Enabled="True" FirstRowSelected="false" MinimumPrefixLength="0"
                                                            OnClientItemSelected="interhotelnautocompleteselected" ServiceMethod="Getinterhotellist"
                                                            TargetControlID="txtinterhotelname">
                                                        </asp:AutoCompleteExtender>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Min.Stay">
                                                    <ItemStyle HorizontalAlign="Left" />
                                                    <HeaderStyle HorizontalAlign="Left" Width="60px" />
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtinterminnights" runat="server" CssClass="field_input" Style="text-align: center"
                                                            Width="80px"></asp:TextBox>
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                            </Columns>
                                            <RowStyle CssClass="grdRowstyle" />
                                            <SelectedRowStyle CssClass="grdselectrowstyle" />
                                            <PagerStyle CssClass="grdpagerstyle" HorizontalAlign="Center" />
                                            <HeaderStyle CssClass="grdheader" />
                                            <AlternatingRowStyle CssClass="grdAternaterow" />
                                        </asp:GridView>
                                        <br />
                                        <asp:Button ID="btnaddhotel" runat="server" CssClass="btn" TabIndex="23" Text="Add Row" />
                                        <asp:Button ID="btndeletehotel" runat="server" CssClass="btn" TabIndex="24" Text="Delete Row" />
                                        <br />
                                    </div>
                                 </td>
                                 <td align="left" valign="top">
                                     <div style="width: 100%; min-height: 25px" id="divstay" runat="server">
                                        <table>
                                            <tr>
                                                <td valign="top">
                                                    Special&nbsp;Occasion
                                                </td>
                                                <td valign="top">
                                                    <textarea id="txtsplocc" runat="server" class="field_input" rows="2" style="width: 350px;
                                                        height: 75px" tabindex="25" textmode="MultiLine">
                                         </textarea>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </td>
                                </tr>
                                </table>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                      
                    </tr>
                    <tr>
                        <td>
                            <div style="width: 100%; min-height: 25px" id="Div3" runat="server">
                            </div>
                        </td>
                    </tr>
               
                    <tr>
                    <td align="left" valign="top" width="150">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div style="width: 100%; min-height: 25px" id="Div8" runat="server">
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div style="width: 100%; min-height: 25px" id="Div9" runat="server">
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" valign="top" width="150">
                            &nbsp;
                        </td>
                        <td>
                            <table>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblview" runat="server" Text="Remarks" Font-Bold="True"> </asp:Label>
                                        <span style="color: #ff0000">* </span>
                                    </td>
                                    <td colspan="4">
                                        <textarea id="txtremarks" runat="server" class="field_input" cols="20" rows="2" style="width: 750px;
                                            height: 100px" tabindex="26">
                                         </textarea>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" valign="top" width="150">
                            &nbsp;
                        </td>
                        <td valign="top" colspan="10">
                            <asp:UpdatePanel ID="UpdatePanesave" runat="server">
                                <ContentTemplate>
                                    <asp:Button ID="btnSave" runat="server" CssClass="field_button" TabIndex="27" Text="Save" OnClientClick="ShowProgess();"
                                        Width="93px" />
                                    &nbsp;<asp:Button ID="btnCancel" runat="server" CssClass="field_button" TabIndex="28"
                                        Text="Return To Search" Width="139px" />
                                    &nbsp;<asp:Button ID="btnhelp" runat="server" CssClass="field_button" TabIndex="29"
                                        Text="Help" Visible="false" />
                                </ContentTemplate>
                            </asp:UpdatePanel>
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
                                <tr>
                                    <td>
                                        <asp:HiddenField ID="hdCurrentDate" runat="server" />
                                        <asp:HiddenField ID="hdSeasonName" runat="server" />
                                        <asp:HiddenField ID="hdnpartycode" runat="server" />
                                        <asp:HiddenField ID="hdnMainGridRowid" runat="server" />
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
                        </td>
                    </tr>
                </tbody>
            </table>
            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                <ContentTemplate>
                    <div id="divcombinations" runat="server" style="overflow: scroll; height: 300px;
                        width: 300px; border: 3px solid green; background-color: White; display: none">
                        <table>
                            <tr>
                                <td colspan="2">
                                    <asp:GridView ID="grdcombinations" runat="server" AutoGenerateColumns="False" BackColor="White"
                                        BorderColor="#999999" CssClass="td_cell" Width="250px">
                                        <Columns>
                                            <asp:TemplateField>
                                                <HeaderStyle HorizontalAlign="Left" BackColor="#06788B" Width="2%" />
                                                <HeaderTemplate>
                                                </HeaderTemplate>
                                                <ItemStyle HorizontalAlign="center" Width="2%"></ItemStyle>
                                                <ItemTemplate>
                                                    <asp:CheckBox runat="server" ID="chkcombineselect" Width="10px" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Room No">
                                                <HeaderStyle HorizontalAlign="Left" BackColor="#06788B" Width="20px" />
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtroomno" runat="server" Width="75px" Style="text-align: center"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Adults">
                                                <HeaderStyle HorizontalAlign="Left" BackColor="#06788B" Width="20px" />
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtadults" runat="server" Width="75px" Style="text-align: center"></asp:TextBox>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left" Width="20px"></ItemStyle>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Child">
                                                <HeaderStyle HorizontalAlign="Left" BackColor="#06788B" Width="20px" />
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtchild" runat="server" Width="75px" Style="text-align: center"></asp:TextBox>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left" Width="20px"></ItemStyle>
                                            </asp:TemplateField>
                                        </Columns>
                                        <RowStyle BackColor="#EEEEEE" ForeColor="Black"></RowStyle>
                                        <SelectedRowStyle BackColor="#008A8C" ForeColor="White" Font-Bold="True"></SelectedRowStyle>
                                        <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center"></PagerStyle>
                                        <HeaderStyle BackColor="#454580" ForeColor="White" Font-Bold="True"></HeaderStyle>
                                        <AlternatingRowStyle BackColor="Transparent" Font-Size="12px"></AlternatingRowStyle>
                                    </asp:GridView>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Button ID="btnaddcomb" runat="server" CssClass="field_button" Text="Add" Width="75px" />
                                    <asp:Button ID="btndelcomb" runat="server" CssClass="field_button" Text="Delete"
                                        Width="75px" />
                                </td>
                                <td>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <div style="width: 100%; min-height: 25px" id="Div10" runat="server">
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Button ID="btncombinationok" runat="server" CssClass="field_button" Text="OK"
                                        Width="75px" />
                                    <asp:Button ID="btncombincancel" runat="server" CssClass="field_button" Text="Close"
                                        Width="75px" />
                                </td>
                                <td>
                                </td>
                            </tr>
                        </table>
                        <input id="btnInvisibleEBGuestnew" runat="server" type="button" value="Close" style="visibility: hidden" />
                        <input id="btnOkayEBnew" type="button" value="Save" style="visibility: hidden" />
                        <input id="btnCancelEBnew" type="button" value="Close" style="visibility: hidden" />
                    </div>
                    <cc1:ModalPopupExtender ID="ModalRoomPopup" runat="server" BehaviorID="ModalRoomPopup"
                        CancelControlID="btnCancelEBnew" OkControlID="btnOkayEBnew" TargetControlID="btnInvisibleEBGuestnew"
                        PopupControlID="divcombinations" PopupDragHandleControlID="PopupHeader" Drag="true"
                        BackgroundCssClass="ModalPopupBG">
                    </cc1:ModalPopupExtender>
                </ContentTemplate>
            </asp:UpdatePanel>
            <table>
                <tr>
                    <td colspan="2">
                        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                            <ContentTemplate>
                                <div id="Div14" runat="server" style="overflow: scroll; height: 400px; width: 750px;
                                    border: 3px solid green; background-color: White; display: none">
                                    <table>
                                        <tr>
                                            <td colspan="2">
                                                <asp:GridView ID="gvShowdates" runat="server" AutoGenerateColumns="False" BackColor="White"
                                                    BorderColor="#999999" CssClass="td_cell" Width="720px">
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="contractid" Visible="false">
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblcontractid" runat="server" Text='<%# Bind("contractid") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <HeaderStyle HorizontalAlign="Left" BackColor="#06788B" Width="2%" />
                                                            <HeaderTemplate>
                                                                <asp:CheckBox runat="server" ID="chkdatesAll" />
                                                            </HeaderTemplate>
                                                            <ItemStyle HorizontalAlign="Left" Width="2%"></ItemStyle>
                                                            <ItemTemplate>
                                                                <asp:CheckBox runat="server" ID="chkdateselect" OnCheckedChanged="CheckBox1_CheckedChanged"
                                                                    Width="10px" AutoPostBack="True" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="contractid" SortExpression="contractid" HeaderText="Contract ID">
                                                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Top"></ItemStyle>
                                                            <HeaderStyle BackColor="#06788B" HorizontalAlign="Left" Width="50px"></HeaderStyle>
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="applicableto" SortExpression="applicableto" HeaderText="Applicable To">
                                                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Top"></ItemStyle>
                                                            <HeaderStyle BackColor="#06788B" HorizontalAlign="Left" Width="100px"></HeaderStyle>
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="seasonname" SortExpression="seasonname" HeaderText="Season">
                                                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Top"></ItemStyle>
                                                            <HeaderStyle BackColor="#06788B" HorizontalAlign="Left" Width="50px"></HeaderStyle>
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="fromdate" SortExpression="fromdate" HeaderText="From Date">
                                                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Top"></ItemStyle>
                                                            <HeaderStyle BackColor="#06788B" HorizontalAlign="Left" Width="50px"></HeaderStyle>
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="todate" SortExpression="todate" HeaderText="To Date">
                                                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Top"></ItemStyle>
                                                            <HeaderStyle BackColor="#06788B" HorizontalAlign="Left" Width="50px"></HeaderStyle>
                                                        </asp:BoundField>
                                                    </Columns>
                                                    <RowStyle BackColor="#EEEEEE" ForeColor="Black"></RowStyle>
                                                    <SelectedRowStyle BackColor="#008A8C" ForeColor="White" Font-Bold="True"></SelectedRowStyle>
                                                    <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center"></PagerStyle>
                                                    <HeaderStyle BackColor="#454580" ForeColor="White" Font-Bold="True"></HeaderStyle>
                                                    <AlternatingRowStyle BackColor="Transparent" Font-Size="12px"></AlternatingRowStyle>
                                                </asp:GridView>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center">
                                                <asp:CheckBox ID="chkApplicable" runat="server" Text="Same Applicable To" Font-Bold="true"
                                                    AutoPostBack="true" OnCheckedChanged="AvailableSeason" />&nbsp;&nbsp;
                                                <asp:CheckBox ID="chkSeason" runat="server" Text="Same Season" Font-Bold="true" OnCheckedChanged="AvailableSeason"
                                                    AutoPostBack="true" />&nbsp;&nbsp;
                                                <asp:Button ID="btnSelectDate" runat="server" CssClass="field_button" Text="Select"
                                                    Width="75px" />
                                                <asp:Button ID="btnclose" runat="server" CssClass="field_button" Text="Close" Width="75px" />
                                            </td>
                                        </tr>
                                    </table>
                                    <input id="btnInvisibleEBGuest1" runat="server" type="button" value="Cancel" style="visibility: hidden" />
                                    <input id="btnOkayEB1" type="button" value="OK" style="visibility: hidden" />
                                    <input id="btnCancelEB1" type="button" value="Cancel" style="visibility: hidden" />
                                </div>
                                <asp:ModalPopupExtender ID="ModalExtraPopup1" runat="server" BehaviorID="ModalExtraPopup1"
                                    CancelControlID="btnCancelEB1" OkControlID="btnOkayEB1" TargetControlID="btnInvisibleEBGuest1"
                                    PopupControlID="Div14" PopupDragHandleControlID="PopupHeader" Drag="true" BackgroundCssClass="ModalPopupBG">
                                </asp:ModalPopupExtender>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
            </table>

              <%--Added shahul 08/08/18--%>
             <center>
                            <div id="Loading1" runat="server" style="height: 150px; width: 500px;">
                                <img alt="" id="Image1" runat="server" src="~/Images/loader-progressbar.gif" width="200" />
                                <h2 style="color: #06788B">
                                    Processing please wait...</h2>
                            </div>

                             
                        </center>
                        <asp:ModalPopupExtender ID="ModalPopupDays" runat="server" BehaviorID="ModalPopupDays"
                            TargetControlID="btnInvisibleGuest" CancelControlID="btnClose" PopupControlID="Loading1"
                            BackgroundCssClass="ModalPopupBG">
                        </asp:ModalPopupExtender>

                           <input id="btnInvisibleGuest" runat="server" type="button" value="Cancel" style="display: none" />
                        <input id="Button1" type="button" value="Cancel" style="display: none" />
              
        </ContentTemplate>
    </asp:UpdatePanel>

  

    <asp:ScriptManagerProxy id="ScriptManagerProxy1" runat="server">
        <services>
<asp:ServiceReference Path="~/clsServices.asmx"></asp:ServiceReference>
</services>
    </asp:ScriptManagerProxy>

</asp:Content>



