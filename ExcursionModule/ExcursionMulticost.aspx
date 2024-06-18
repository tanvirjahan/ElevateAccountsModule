<%@ Page Title="" Language="VB" MasterPageFile="~/SubPageMaster.master" AutoEventWireup="false" CodeFile="ExcursionMulticost.aspx.vb" Inherits="ExcursionMulticost" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
   
   <%@ Register Src="../PriceListModule/Countrygroup.ascx" TagName="Countrygroup" TagPrefix="uc2" %>
    <%@ OutputCache location="none" %> 


<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="EclipseWebSolutions.DatePicker" Namespace="EclipseWebSolutions.DatePicker"
    TagPrefix="ews" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">

<link rel="stylesheet" href="../Content/lib/css/reset.css" type="text/css" media="screen"
        charset="utf-8">
    <link rel="stylesheet" href="../Content/lib/css/icons.css" type="text/css" media="screen"
        charset="utf-8">
    <link rel="stylesheet" href="../Content/lib/css/workspace.css" type="text/css" media="screen"
        charset="utf-8">
    <link href="../css/VisualSearch.css" rel="stylesheet" type="text/css" />
    <script src="../Content/vendor/jquery-1.11.0.js" type="text/javascript" charset="utf-8"></script>
    <script src="../Content/vendor/jquery.ui.core.js" type="text/javascript" charset="utf-8"></script>
    <script src="../Content/vendor/jquery.ui.widget.js" type="text/javascript" charset="utf-8"></script>
    <script src="../Content/vendor/jquery.ui.position.js" type="text/javascript" charset="utf-8"></script>
    <script src="../Content/vendor/jquery.ui.menu.js" type="text/javascript" charset="utf-8"></script>
    <script src="../Content/vendor/jquery.ui.autocomplete.js" type="text/javascript"
        charset="utf-8"></script>
    <script src="../Content/vendor/underscore-1.5.2.js" type="text/javascript" charset="utf-8"></script>
    <script src="../Content/vendor/backbone-1.1.0.js" type="text/javascript" charset="utf-8"></script>
    <script src="../Content/lib/js/visualsearch.js" type="text/javascript" charset="utf-8"></script>
    <script src="../Content/lib/js/views/search_box.js" type="text/javascript" charset="utf-8"></script>
    <script src="../Content/lib/js/views/search_facet.js" type="text/javascript" charset="utf-8"></script>
    <script src="../Content/lib/js/views/search_input.js" type="text/javascript" charset="utf-8"></script>
    <script src="../Content/lib/js/models/search_facets.js" type="text/javascript" charset="utf-8"></script>
    <script src="../Content/lib/js/models/search_query.js" type="text/javascript" charset="utf-8"></script>
    <script src="../Content/lib/js/utils/backbone_extensions.js" type="text/javascript"
        charset="utf-8"></script>
    <script src="../Content/lib/js/utils/hotkeys.js" type="text/javascript" charset="utf-8"></script>
    <script src="../Content/lib/js/utils/jquery_extensions.js" type="text/javascript"
        charset="utf-8"></script>
    <script src="../Content/lib/js/utils/search_parser.js" type="text/javascript" charset="utf-8"></script>
    <script src="../Content/lib/js/utils/inflector.js" type="text/javascript" charset="utf-8"></script>
    <script src="../Content/lib/js/templates/templates.js" type="text/javascript" charset="utf-8"></script>
    <script src="../Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
      <script type="text/javascript" charset="utf-8">


          $(document).ready(function () {


              //AutoCompleteExtenderKeyUp();
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

          var glcallback;



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
      <script type="text/javascript">

          var prm = Sys.WebForms.PageRequestManager.getInstance();
          prm.add_initializeRequest(InitializeRequestUserControl);
          prm.add_endRequest(EndRequestUserControl);

          function InitializeRequestUserControl(sender, args) {

          }

          function EndRequestUserControl(sender, args) {
              //AutoCompleteExtenderKeyUp();
              // after update occur on UpdatePanel re-init the Autocomplete
              visualsearchbox();
              AutoCompleteExtenderUserControlKeyUp();
          }


          function exctypenameautocompleteselected(source, eventArgs) {
              if (eventArgs != null) {
                  document.getElementById('<%=txtexctypecode.ClientID%>').value = eventArgs.get_value();
              }
              else {
                  document.getElementById('<%=txtexctypecode.ClientID%>').value = '';
              }
          }

          function CurrNameautocompleteselected(source, eventArgs) {
              if (eventArgs != null) {
                  document.getElementById('<%=txtCurrCode.ClientID%>').value = eventArgs.get_value();
              }
              else {
                  document.getElementById('<%=txtCurrCode.ClientID%>').value = '';
              }
          }


          var ltxtexhicode;
          function suppnameautocompleteselected(source, eventArgs) {


              var hiddenfieldID = source.get_id().replace("txtsuppname_AutoCompleteExtender", "txtsuppcode");
              ltxtexhicode = hiddenfieldID;
              $get(hiddenfieldID).value = eventArgs.get_value();

              var exhicode = eventArgs.get_value();


          }

          function vehiclenameautocompleteselected(source, eventArgs) {
              if (eventArgs != null) {
                  document.getElementById('<%=txtvehiclecode.ClientID%>').value = eventArgs.get_value();
              }
              else {
                  document.getElementById('<%=txtvehiclecode.ClientID%>').value = '';
              }
          }

          function enablemarkuptype() {
              var ddlMarkuptype = document.getElementById("<%=ddlMarkuptype.ClientID%>");
              var txtoperator = document.getElementById("<%=txtoperator.ClientID%>");

              var tdMarkupChild = document.getElementById("<%=tdMarkupChild.ClientID%>");
              var tdMarkupSenior = document.getElementById("<%=tdMarkupSenior.ClientID%>");
              var tdMarkupChild1 = document.getElementById("<%=tdMarkupChild1.ClientID%>");
              var tdMarkupSenior1 = document.getElementById("<%=tdMarkupSenior1.ClientID%>");

              var lblAdultMarkup = document.getElementById("<%=lblAdultMarkup.ClientID%>");
              if (ddlMarkuptype.value == 'Unit') {


                  txtoperator.value = "*";
                  lblAdultMarkup.innerHTML = "Markup"
                  tdMarkupChild.style.display = "none";
                  tdMarkupSenior.style.display = "none";
                  tdMarkupChild1.style.display = "none";
                  tdMarkupSenior1.style.display = "none";
              }
              if (ddlMarkuptype.value == 'Per Pax') {

                  lblAdultMarkup.innerHTML = "Markup for Adult"
                  txtoperator.value = "+";
                  tdMarkupChild.removeAttribute('style');
                  tdMarkupSenior.removeAttribute('style');
                  tdMarkupChild1.removeAttribute('style');
                  tdMarkupSenior1.removeAttribute('style');
         
              }
          }


</script>

<script language ="javascript" type ="text/javascript" >

    function CalculateTax() {

        var VatPerc = document.getElementById("<%=txtVAT.ClientID%>").value;
        var chkPriceWithTax = document.getElementById("<%=chkPriceWithTax.ClientID%>");

        if ((VatPerc).trim() == "") {
//            alert("Vat Percentage can not be empty");
//            VatPerc.focus();
//            return false;
            document.getElementById("<%=txtVAT.ClientID%>").value = '0';
        }
        var txt = document.getElementById(arguments[0]);
        var txtTax = document.getElementById(arguments[1]);
        // var txtNonTax = document.getElementById(arguments[2]);
        var txtVat = document.getElementById(arguments[2]);

        if ((txt.value).trim() == '') {

            txtTax.value = '';
            txtVat.value = '';
        }
        else if (chkPriceWithTax.checked == false) {
            txtTax.value = parseFloat(txt.value).toFixed(2);
            var vat = parseFloat(txt.value).toPrecision() * (parseFloat(VatPerc).toPrecision() / 100);

            txtVat.value = vat.toFixed(2);
        }
        else {

            var Taxable = parseFloat(txt.value).toPrecision() / (1 + (parseFloat(VatPerc).toPrecision() / 100));
            var vat = parseFloat(txt.value).toPrecision() - Taxable;

            txtTax.value = Taxable.toFixed(2);
            txtVat.value = vat.toFixed(2);
        }

    }
    function fnReadOnly(txt) {
        event.preventDefault();
    }
    function chkTextLock(evt) {
        return false;
    }
    function chkTextLock1(evt) {
        if (evt.keyCode = 9) {
            return true;
        }
        else {
            return false;
        }
        return false;
    }
    function checkTelephoneNumber(e) {

        if ((event.keyCode < 45 || event.keyCode > 57)) {
            return false;
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
    function validateDecimalOnly(evt, txt) {
        var theEvent = evt || window.event;
        var key = theEvent.keyCode || theEvent.which;
        if (key == 13) {
        }
        else {
            key = String.fromCharCode(key);

            var regex = /[0-9]/;
            if (!regex.test(key)) {
                theEvent.returnValue = false;
                if (theEvent.preventDefault) theEvent.preventDefault();
            }

            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode == 46) {
                var inputValue = txt.value
                if (inputValue.indexOf('.') < 1) {
                    txt.value = txt.value + '.';
                    return true;
                }
                else {
                    return false;
                }
            }

        }

    }
    function checkCharacter(e) {
        if (event.keyCode == 32 || event.keyCode == 46)
            return;
        if ((event.keyCode < 47 || event.keyCode > 57) && (event.keyCode < 65 || event.keyCode > 91) && (event.keyCode < 96 || event.keyCode > 122)) {
            return false;
        }
    }

    function CallWebMethod(methodType) {
        switch (methodType) {

            case "sptypecode":
                var select = document.getElementById("<%=ddlSPType.ClientID%>");
                var codeid = select.options[select.selectedIndex].value;

                 var codetext =select.options[select.selectedIndex].text;

                var selectname = document.getElementById("<%=ddlSPTypeName.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;
               
                var connstr = document.getElementById("<%=txtconnection.ClientID%>");
                constr = connstr.value

                //ColServices.clsServices.GetSellingCurrNameexc(constr, codeid, FillSupplierCurrName, ErrorHandler, TimeOutHandler);
                ColServices.clsServices.GetSellingCurrCodeexc(constr, codetext, FillSupplierCurrCode, ErrorHandler, TimeOutHandler);
                ColServices.clsServices.GetSellingCurrNameexc(constr, codeid, FillSupplierCurrName, ErrorHandler, TimeOutHandler);
                break;
            case "sptypename":
                var select = document.getElementById("<%=ddlSPTypeName.ClientID%>");
                var codeid = select.options[select.selectedIndex].value;
                var selectname = document.getElementById("<%=ddlSPType.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;

                var codetext = select.options[select.selectedIndex].text;

                var connstr = document.getElementById("<%=txtconnection.ClientID%>");
                constr = connstr.value

                ColServices.clsServices.GetSellingCurrCodeexc(constr, codeid, FillSupplierCurrCode, ErrorHandler, TimeOutHandler);
                ColServices.clsServices.GetSellingCurrNameexc(constr, codetext, FillSupplierCurrName, ErrorHandler, TimeOutHandler);
                //ColServices.clsServices.GetSellingCurrCodeexc(constr, codeid, FillSupplierCurrCode, ErrorHandler, TimeOutHandler);

                break;


            case "exccode":
                var select = document.getElementById("<%=ddlexccode.ClientID%>");
                var codeid = select.options[select.selectedIndex].text ;
                var selectname = document.getElementById("<%=ddlexcname.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;

                var connstr = document.getElementById("<%=txtconnection.ClientID%>");
                constr = connstr.value

                break;

            case "excname":
                var select = document.getElementById("<%=ddlexcname.ClientID%>");
                var codeid = select.options[select.selectedIndex].text;
                var selectname = document.getElementById("<%=ddlexccode.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;

                var connstr = document.getElementById("<%=txtconnection.ClientID%>");
                constr = connstr.value
                break;
            

            case "groupcd":
                var select = document.getElementById("<%=ddlGroupCode.ClientID%>");
                codeid = select.options[select.selectedIndex].value;
                var selectname = document.getElementById("<%=ddlGroupName.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;

                var connstr = document.getElementById("<%=txtconnection.ClientID%>");
                constr = connstr.value
                break;

            case "groupnm":
                var select = document.getElementById("<%=ddlGroupName.ClientID%>");
                codeid = select.options[select.selectedIndex].value;
                var selectname = document.getElementById("<%=ddlGroupCode.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;
                
                var connstr = document.getElementById("<%=txtconnection.ClientID%>");
                constr = connstr.value
                break;

        }
    }


    function FillSupplierCodes(result) {
        var ddl = document.getElementById("<%=ddlSPType.ClientID%>");
        RemoveAll(ddl)
        for (var i = 0; i < result.length; i++) {
            var option = new Option(result[i].ListText, result[i].ListValue);
            ddl.options.add(option);
        }
        ddl.value = "[Select]";
    }

    function FillSupplierNames(result) {
        var ddl = document.getElementById("<%=ddlSPTypeName.ClientID%>");
        RemoveAll(ddl)
        for (var i = 0; i < result.length; i++) {
            var option = new Option(result[i].ListText, result[i].ListValue);
            ddl.options.add(option);
        }
        ddl.value = "[Select]";
    }


    function FillSupplierCurrCode(result) {
        var txt = document.getElementById("<%=txtCurrCode.ClientID%>");
        txt.value = result;
    }

    function FillSupplierCurrName(result) {
        var txt = document.getElementById("<%=txtCurrName.ClientID%>");
        txt.value = result;
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

    function FormValidation(state) {
//        if (document.getElementById("<%=ddlSPType.ClientID%>").value == "[Select]") {
//            document.getElementById("<%=ddlSPType.ClientID%>").focus();
//            alert("Select Selling Type Code.");
//            return false;
//        }

//        else if (document.getElementById("<%=ddlGroupCode.ClientID%>").value == "[Select]") {
//            document.getElementById("<%=ddlGroupCode.ClientID%>").focus();
//            alert("Select Group Code.");
//            return false;

//            if ((document.getElementById("<%=ddlexccode.ClientID%>").style.visibility = "hidden")){

//            
//                    if (document.getElementById("<%=ddlexccode.ClientID%>").value == "[Select]") {
////                    document.getElementById("<%=ddlexccode.ClientID%>").focus();
////                    alert("Select Airport Code.");
////                    return false;
//                    }
//            }
//        }

//        else {
//            //alert(state);
//            if (state == 'New') { if (confirm('Are you sure you want to generate Excursion Price list?') == false) return false; }
//            if (state == 'Edit') { if (confirm('Are you sure you want to generate Excursion Price list?') == false) return false; }
//            if (state == 'Delete') { if (confirm('Are you sure you want to generate Excursion Price list?') == false) return false; }

//        }
    }



    function ddlSPType_onclick() {

    }

</script>
   <script type="text/javascript">
       var SelectedRow = null;
       var SelectedRowIndex = null;
       var UpperBound = null;
       var LowerBound = null;
       var selectedgrdname = null;

       window.onload = function () {
           LowerBound = 0;
           SelectedRowIndex = -1;
       }

       function SelectRow(CurrentRow, RowIndex, gridname, focusIndex) {

           // alert(gridname);
           // alert(selectedgrdname);
           // alert(RowIndex);

           if (gridname != selectedgrdname) {
               if (SelectedRow != null) {
                   SelectedRow.style.backgroundColor = SelectedRow.originalBackgroundColor;
                   SelectedRow.style.color = SelectedRow.originalForeColor;
               }
               SelectedRow = null;
               SelectedRowIndex = null;
               selectedgrdname = null;
           }
           selectedgrdname = gridname;
           var gridView = document.getElementById(gridname);
           UpperBound = gridView.getElementsByTagName("tr").length - 2;
           if (SelectedRow == CurrentRow || RowIndex > UpperBound || RowIndex < LowerBound) {
               //  alert('selectedrow' + SelectedRow);
               return;
           }
           if (SelectedRow != null) {

               SelectedRow.style.backgroundColor = SelectedRow.originalBackgroundColor;
               SelectedRow.style.color = SelectedRow.originalForeColor;
               //alert('background original');
           }

           if (CurrentRow != null) {
               CurrentRow.originalBackgroundColor = CurrentRow.style.backgroundColor;
               CurrentRow.originalForeColor = CurrentRow.style.color;
               //  alert('background changed color');
               CurrentRow.style.backgroundColor = '#FFCC99';
               CurrentRow.style.color = 'Black';
               var txtFrm = CurrentRow.cells[focusIndex].getElementsByTagName("input")[0];
               txtFrm.focus();
               // alert(txtFrm.value);

           }

           SelectedRow = CurrentRow;
           SelectedRowIndex = RowIndex;

           setTimeout("SelectedRow.focus();", 0);
       }

       function SelectSibling(e, gridname, focusIndex, Cur_row) {

           //                   alert(Cur_row.rowIndex-1);
           //                   alert(SelectedRowIndex);
           var iflag = 0;
           if (SelectedRowIndex != Cur_row.rowIndex - 1) {
               //                       SelectedRow = Cur_row;
               //                       SelectedRowIndex = Cur_row.rowIndex - 1;
               iflag = 1;
           }
           var e = e ? e : window.event;
           var KeyCode = e.which ? e.which : e.keyCode;
           // alert(SelectedRowIndex);
           if (KeyCode == 40 || KeyCode == 38 || KeyCode == 9) {
               //                       alert(Cur_row.rowIndex - 1);
               //                       alert(SelectedRowIndex);
               if (KeyCode == 40) {
                   SelectRow(SelectedRow.nextSibling, SelectedRowIndex + 1, gridname, focusIndex);
               }
               else if (KeyCode == 38) {
                   SelectRow(SelectedRow.previousSibling, SelectedRowIndex - 1, gridname, focusIndex);
               }
               else if ((KeyCode == 9) && (iflag == 1)) {
                   //  alert('9');
                   SelectRow(Cur_row, Cur_row.rowIndex - 1, gridname, focusIndex);
               }
           }
           return true;
       }
       function LastSelectRow(CurrentRow, RowIndex, gridname, focusIndex) {
           var row = document.getElementById(CurrentRow);
           SelectRow(row, RowIndex, gridname, focusIndex);

       }
    </script>


 <asp:UpdatePanel id="UpdatePanel1" runat="server">
   <contenttemplate>
   <TABLE style="BORDER-RIGHT: gray 2pt solid; BORDER-TOP: gray 2pt solid; BORDER-LEFT: gray 2pt solid; BORDER-BOTTOM: gray 2pt solid">
     <TBODY><TR><TD style="HEIGHT: 18px; TEXT-ALIGN: center" class="field_heading" align=left colSpan=4>
          <asp:Label id="lblHeading" runat="server" Text="Add New Excursion price List" CssClass="field_heading" Width="744px"></asp:Label></TD>
         <td align="left" class="field_heading" style="HEIGHT: 18px; TEXT-ALIGN: center">
             &nbsp;</td>
         </TR>
     
     <tr><td style="width: 201px" class="td_cell" align=left>
        <SPAN style="FONT-FAMILY: Arial">PL Code</SPAN></TD><TD style="WIDTH: 122px">
            <INPUT style="WIDTH: 194px" id="txtPlcCode" class="field_input" disabled tabIndex=1 type=text runat="server" /></TD>
               <td style="WIDTH: 190px" class="td_cell" align=right>
                <span style="FONT-FAMILY: Arial">Currency Name</span> </TD>
            <TD>
                <asp:TextBox ID="txtCurrName" runat="server" CssClass="field_input" 
                    tabIndex="1" Width="258px" ></asp:TextBox>
                    <asp:TextBox ID="txtCurrCode" runat="server" style="display:none"
                     Width="194px" ></asp:TextBox>
                        <asp:AutoCompleteExtender ID="txtCurrName_AutoCompleteExtender" runat="server"
                        CompletionInterval="10" CompletionListCssClass="autocomplete_completionListElement"
                        CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem" CompletionListItemCssClass="autocomplete_listItem"
                        CompletionSetCount="1" DelimiterCharacters="" EnableCaching="false" Enabled="True"
                        FirstRowSelected="True" MinimumPrefixLength="0" OnClientItemSelected="CurrNameautocompleteselected"
                        ServiceMethod="Getcurrlist" TargetControlID="txtCurrName">
                    </asp:AutoCompleteExtender>
                </TD>
           
            
            <td>
                &nbsp;</td>
           
            
            </TR>
            <tr>
                <td align="left" class="td_cell" style="WIDTH: 190px; HEIGHT: 3px">
                     Applicable to</td>
           
               <td><asp:TextBox ID="txtApplicableTo" runat="server" Rows="2" Style="margin: 0px; height: 48px;
                                            width: 300px" TabIndex="2" TextMode="MultiLine"></asp:TextBox>
            </td>
         
              <td align="right" class="td_cell"  >
                     Select Excursion</td>
                     <td>
                    <asp:TextBox ID="txtexctypename" runat="server" AutoPostBack="True" CssClass="field_input"
                        MaxLength="500" TabIndex="3" Width="258px"></asp:TextBox>
                        <asp:TextBox ID="txtexctypecode" runat="server" Style="display: none"></asp:TextBox>
                          <asp:AutoCompleteExtender ID="txtexctypename_AutoCompleteExtender" runat="server"
                        CompletionInterval="10" CompletionListCssClass="autocomplete_completionListElement"
                        CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem" CompletionListItemCssClass="autocomplete_listItem"
                        CompletionSetCount="1" DelimiterCharacters="" EnableCaching="false" Enabled="True"
                        FirstRowSelected="True" MinimumPrefixLength="0" OnClientItemSelected="exctypenameautocompleteselected"
                        ServiceMethod="GetExcursionlist" TargetControlID="txtexctypename">
                    </asp:AutoCompleteExtender>
                </td>
                <td>
                    &nbsp;</td>
            </tr>
            <tr>
                <td></td>
                <td></td>
                 <td align="right" class="td_cell"  >
                     Select Vehicle</td>
                     <td>
                    <asp:TextBox ID="txtvehiclename" runat="server" AutoPostBack="True" CssClass="field_input"
                        MaxLength="500" TabIndex="3" Width="258px"></asp:TextBox>
                        <asp:TextBox ID="txtvehiclecode" runat="server" Style="display: none"></asp:TextBox>
                          <asp:AutoCompleteExtender ID="txtvehiclename_AutoCompleteExtender" runat="server"
                        CompletionInterval="10" CompletionListCssClass="autocomplete_completionListElement"
                        CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem" CompletionListItemCssClass="autocomplete_listItem"
                        CompletionSetCount="1" DelimiterCharacters="" EnableCaching="false" Enabled="True"
                        FirstRowSelected="True" MinimumPrefixLength="0" OnClientItemSelected="vehiclenameautocompleteselected"
                        ServiceMethod="Getvehiclelist" TargetControlID="txtvehiclename">
                    </asp:AutoCompleteExtender>
                </td>

                <td>
                    &nbsp;</td>

            </tr>
             <tr>
             <td>
                 &nbsp;</td>
                 <td>
                   <cc1:ModalPopupExtender ID="ModalSelectsector" runat="server" BehaviorID="ModalSelectsector"
                        CancelControlID="TdSectorClose" TargetControlID="hdSectorPopup" PopupControlID="dvSectorPopup"
                        PopupDragHandleControlID="PopupSectorHeader" Drag="true" BackgroundCssClass="ModalPopupBG">
                    </cc1:ModalPopupExtender>
                    <div id="dvSectorPopup" runat="server" style="height: 500px; Width:750px; border: 3px solid #06788B;display:none;
                        background-color: White;">
                        <table style="width: 99%; padding: 5px 5px 5px 5px">
                            <tr>
                                <td id="PopupSectorHeader" bgcolor="#06788B">
                                    <asp:Label ID="Label1" runat="server" CssClass="field_heading" Style="padding: 3px 0px 3px 3px"
                                        Text="Sector Selection" Width="205px"></asp:Label>
                                </td>
                                <td align="center" id="TdSectorClose" bgcolor="#06788B">
                                    <asp:Label ID="Label2" Text="X" runat="server" Font-Bold="True" Font-Names="Verdana"
                                        Font-Size="Large" ForeColor="White"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                <table>
                                <tr>
                                <td>
                                    <div style="height: 500px; overflow: auto;Width:100%">
                                        <asp:DataList ID="gvSectors" runat="server" RepeatColumns="2" 
                                            BackColor="White" BorderColor="#999999" BorderStyle="None" BorderWidth="1px"
                                            RepeatDirection="Horizontal" CellPadding="3" CssClass="td_cell" Font-Bold="true"
                                            Font-Size="12px" Width="717px">
                                            <FooterStyle CssClass="grdfooter" />
                                            <HeaderStyle HorizontalAlign="Left" BackColor="#06788B" Width="2%" />
                                            <HeaderTemplate>
                                                <asp:CheckBox ID="chkAll" runat="server"  TabIndex="4"
                                                    />
                                                <asp:Label ID="lblPromotionType" runat="server" Text="Sector"></asp:Label>
                                            </HeaderTemplate>
                                            <ItemStyle HorizontalAlign="Left" Width="2%"></ItemStyle>
                                            <ItemTemplate>
                                                <table class="styleDatalist" style="border: 10px; font-family: Arial,Verdana, Geneva, ms sans serif;
                                                    font-size: 10pt; border-collapse: collapse;">
                                                    <tr style="">
                                                        <td style="border: 0px;">
                                                            <asp:CheckBox ID="chksectorSelect" runat="server"  />
                                                            <asp:Label ID="lblsectorname" runat="server"  Text='<%# Bind("sectorname") %>'></asp:Label>
                                                           
                                                        </td>
                                                        <td style="display:none"> 
                                                        <asp:Label ID="lblsector" runat="server"  Text='<%# Bind("sectorcode") %>'></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ItemTemplate>
                                            <ItemStyle CssClass="grdRowstyle" />
                                            <HeaderStyle CssClass="grdheader" />
                                            <AlternatingItemStyle CssClass="grdAternaterow" />
                                            <SelectedItemStyle CssClass="grdselectrowstyle" />
                                      </asp:DataList>
                                       
                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        <div class="clear"></div>
                                         <div class="clear"></div>
                                      <div style="text-align:center !important;margin-left:50px"  >
                                      <asp:Button ID="btnoksector" runat="server" CssClass="btn"  Text="OK" />
                                      </div>
                                       
                                    </div>
                                   
                                     
                                    </td>
                                    
                                    </tr>
                                 
                                    </table>
                                    
                                </td>
                                 
                            </tr>
                            
                        </table>
                    </div></td>
                 <td align="right" class="td_cell">

                    Sector Name</td>
                 <td>
                    <asp:TextBox ID="txtsectorname" runat="server" ReadOnly="true"  CssClass="field_input"
                        MaxLength="500" TabIndex="3" Width="258px" Height="48px" TextMode="MultiLine"></asp:TextBox>
                         <asp:TextBox ID="txtsectorcode" runat="server" Style="display: none"></asp:TextBox>

                           
                        <asp:HiddenField ID="hdSectorPopup" runat="server" />
                        <asp:HiddenField ID="hdnairport" runat="server" />
                        <asp:HiddenField ID="HiddenField2" runat="server" />
                        </td>
                 <td>
                      <asp:Button ID="btnSecotorselect" runat="server" CssClass="field_button" 
                       tabIndex="4" Text="Select" /></td>
         </tr>
             <tr>
             <td style="width: 100px" valign="top" colspan="4">
                 <div style="width: 100%; min-height: 400px" id="iframeINF" runat="server">
                     <asp:UpdatePanel ID="UpdatePanel2" runat="server">
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
                         </ContentTemplate>
                     </asp:UpdatePanel>
                 </div>
             </td>
                 <td style="width: 100px" valign="top">
                     &nbsp;</td>
         </tr>
            <tr style ="display:none">
           
            
                 <td></td>
                 <td>
                     <asp:GridView ID="gvSelectVehicle" runat="server" AllowPaging="True" 
                         AllowSorting="True" AutoGenerateColumns="False" BorderStyle="Solid" 
                         BorderWidth="1px" CellPadding="3" CssClass="grdstyle" Font-Size="10px" 
                         GridLines="Vertical" PageSize="20" TabIndex="9" Width="100%">
                         <FooterStyle CssClass="grdfooter" />
                         <Columns>
                             <asp:TemplateField HeaderText="Supplier Code" Visible="False">
                                 <EditItemTemplate>
                                 </EditItemTemplate>
                                 <ItemTemplate>
                                     <asp:Label ID="lblvehiclecode" runat="server" __designer:wfdid="w1" 
                                         Text='<%# Bind("othcatcode") %>'></asp:Label>
                                 </ItemTemplate>
                             </asp:TemplateField>
                             <asp:TemplateField HeaderText="Select">
                                 <ItemTemplate>
                                     <asp:CheckBox ID="chkvehicleSelect" runat="server" />
                                 </ItemTemplate>
                                 <ItemStyle HorizontalAlign="Center" />
                             </asp:TemplateField>
                             <asp:BoundField DataField="othcatname" HeaderStyle-HorizontalAlign="Left" 
                                 HeaderText="Vehicle" SortExpression="othcatname"></asp:BoundField>
                         </Columns>
                         <RowStyle CssClass="grdRowstyle" />
                         <SelectedRowStyle CssClass="grdselectrowstyle" />
                         <PagerStyle CssClass="grdpagerstyle" HorizontalAlign="Center" />
                         <HeaderStyle CssClass="grdheader" ForeColor="white" />
                         <AlternatingRowStyle CssClass="grdAternaterow" />
                     </asp:GridView>
                 </td>
             
                <td align="left" colspan="2" valign="top">
                    <asp:Button ID="btnselectvehicle" runat="server" CssClass="field_button" TabIndex="5"
                        Text="Select Vehicle" />
                    <asp:HiddenField ID="hdMarkUp" runat="server" />
                    <asp:HiddenField ID="hdnvehicle" runat="server" />
                    <cc1:ModalPopupExtender ID="ModalSelectVehicle" runat="server" BehaviorID="ModalSelectVehicle"
                        CancelControlID="TdMarkupClose" TargetControlID="hdMarkUp" PopupControlID="dvVehiclePopup"
                        PopupDragHandleControlID="PopupMarkupHeader" Drag="true" BackgroundCssClass="ModalPopupBG">
                    </cc1:ModalPopupExtender>
                    <div id="dvVehiclePopup" runat="server" style="height: 470px; width: 650px; border: 3px solid #06788B;
                        background-color: White;">
                        <table style="width: 99%; padding: 5px 5px 5px 5px">
                            <tr>
                                <td id="PopupMarkupHeader" bgcolor="#06788B">
                                    <asp:Label ID="Label3" runat="server" CssClass="field_heading" Style="padding: 3px 0px 3px 3px"
                                        Text="Vehicles Selection" Width="205px"></asp:Label>
                                </td>
                                <td align="center" id="TdMarkupClose" bgcolor="#06788B">
                                    <asp:Label ID="Label4" Text="X" runat="server" Font-Bold="True" Font-Names="Verdana"
                                        Font-Size="Large" ForeColor="White"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <div style="height: 400px; overflow: auto;">
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td align="center" colspan="2" style="padding-top: 5px">
                                    <asp:Button ID="btnokvehicle" runat="server" CssClass="btn" Text="OK" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>

                 <td align="left" valign="top">
                     &nbsp;</td>

            </tr>
               <tr style="display:none" >
            <td style="WIDTH: 201px" align=left><SPAN style="FONT-FAMILY: Arial">
                <span style="FONT-SIZE: 8pt">Currency Code</span> </SPAN></TD>
            <td style="WIDTH: 122px">
             
                </TD>
           
             </TR>
        
            <TR style="display:none">
            <TD style="WIDTH: 201px; HEIGHT: 3px" class="td_cell" align=left>
            <SPAN style="FONT-FAMILY: Arial">Selling &nbsp;Type Code&nbsp;<SPAN style="COLOR: #ff0000">* </SPAN></SPAN></TD>
            <td style="WIDTH: 122px; HEIGHT: 3px"><SELECT style="WIDTH: 200px" id="ddlSPType" class="field_input" tabIndex=2 onchange="CallWebMethod('sptypecode');" runat="server" onclick="return ddlSPType_onclick()">
             <OPTION selected></OPTION></SELECT></TD>
            <td style="WIDTH: 190px; HEIGHT: 3px" class="td_cell" align=left>Selling<SPAN style="FONT-FAMILY: Arial">
                &nbsp;Type Name</SPAN></TD>
            <td style="HEIGHT: 3px"><SELECT style="WIDTH: 300px" id="ddlSPTypeName" class="field_input" tabIndex=3 onchange="CallWebMethod('sptypename');" runat="server"> <OPTION selected></OPTION></SELECT></TD>
                <td style="HEIGHT: 3px">
                    &nbsp;</td>
         </TR>
             <TR style ="display:none">
           
                 <td align="left" class="td_cell" style="WIDTH: 201px; HEIGHT: 3px">
                     Group Code<span style="COLOR: #ff0000">*</span>
                 </td>
                 <td style="WIDTH: 122px; HEIGHT: 3px">
                     <select ID="ddlGroupCode" runat="server" class="field_input" name="D1" 
                         onchange="CallWebMethod('groupcd');" style="WIDTH: 200px" tabindex="10">
                         <option selected=""></option>
                     </select>
                 </td>
               
                 <td style="HEIGHT: 3px">
                     <select ID="ddlGroupName" runat="server" class="field_input" name="D2" 
                         onchange="CallWebMethod('groupnm');" style="WIDTH: 300px" tabindex="11">
                         <option selected=""></option>
                     </select>
                 </td>
           
           </TR>
            <tr ID="airport" style="visibility:visible">
                <td align="left" class="td_cell" style="WIDTH: 201px; ">
                    <SPAN style="font-family: Arial;">
                        <asp:Label ID="airportcode" runat="server" Text="Excursion Code" 
                            Visible="false"></asp:Label>
                    </SPAN>
                </td>
                <td style="WIDTH: 122px; ">
                    <select id="ddlexccode" runat="server" class="field_input" 
                        onchange="CallWebMethod('airportcd');" 
                        style="WIDTH: 200px" tabindex="4" visible="false">
                        <option selected=""></option>
                    </select></td>
                <td align="left" class="td_cell" style="WIDTH: 190px; ">
                    <asp:Label ID="lblexcname" runat="server" visible="false"> Excursion Name</asp:Label>
                </td>
                <td>
                    <select id="ddlexcname" runat="server" class="field_input" 
                        onchange="CallWebMethod('airportnm');" style="WIDTH: 300px" tabindex="5" 
                        onclick="return ddlAirportnm_onclick()" visible="false">
                        <option selected=""></option>
                    </select></td>
                <td>
                    &nbsp;</td>
         </tr>
            
         
         <tr>
             <td align="left" class="td_cell" style="width: 190px; height: 3px">
                 Remarks
             </td>
             <td align="left" style="width: 183px; height: 100px">
                 <textarea id="txtRemark" runat="server" class="field_input" style="width: 363px;
                     height: 60px" tabindex="4">
            </textarea>
             </td>
            
             <td valign="top" >
                 <div id="dv_SearchResult" runat="server" style="max-height: 250px; overflow: auto;">
                     <asp:GridView ID="grdDates" runat="server" AllowSorting="True" AutoGenerateColumns="False"
                         BackColor="White" BorderColor="#999999" BorderStyle="Solid" BorderWidth="1px"
                         Caption="Price list Dates" CellPadding="3" CssClass="td_cell"
                         Font-Bold="true" Font-Size="12px" GridLines="Vertical" TabIndex="5">
                         <FooterStyle CssClass="grdfooter" />
                         <Columns>
                             <asp:BoundField DataField="clinenno" HeaderText="Sr No" Visible="False" />
                             <asp:TemplateField HeaderText="From Date">
                                 <ItemTemplate>
                                     <asp:TextBox ID="txtfromDate" runat="server" CssClass="fiel_input" Width="80px"></asp:TextBox>
                                     <cc1:CalendarExtender ID="dpFromDate" runat="server" Format="dd/MM/yyyy" PopupButtonID="ImgBtnFrmDt"
                                         TargetControlID="txtfromDate" PopupPosition="Right">
                                     </cc1:CalendarExtender>
                                     <cc1:MaskedEditExtender ID="MeFromDate" runat="server" Mask="99/99/9999" MaskType="Date"
                                         TargetControlID="txtfromDate">
                                     </cc1:MaskedEditExtender>
                                     <asp:ImageButton ID="ImgBtnFrmDt" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.png"
                                         TabIndex="-1" /><br />
                                     <cc1:MaskedEditValidator ID="MevFromDate" runat="server" ControlExtender="MeFromDate"
                                         ControlToValidate="txtfromDate" CssClass="field_error" Display="Dynamic" EmptyValueBlurredText="Date is required"
                                         EmptyValueMessage="Date is required" ErrorMessage="MeFromDate" InvalidValueBlurredMessage="Invalid Date"
                                         InvalidValueMessage="Invalid Date" TooltipMessage="Input a Date in Date/Month/Year">
                                         &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</cc1:MaskedEditValidator>
                                 </ItemTemplate>
                                 <HeaderStyle Wrap="False" />
                                 <ItemStyle Wrap="False" />
                             </asp:TemplateField>
                             <asp:TemplateField HeaderText="To Date">
                                 <ItemTemplate>
                                     <asp:TextBox ID="txtToDate" runat="server" CssClass="fiel_input" Width="80px"></asp:TextBox>
                                     <cc1:CalendarExtender ID="dpToDate" runat="server" Format="dd/MM/yyyy" PopupButtonID="ImgBtnToDt"
                                         TargetControlID="txtToDate" PopupPosition="Right">
                                     </cc1:CalendarExtender>
                                     <cc1:MaskedEditExtender ID="MeToDate" runat="server" Mask="99/99/9999" MaskType="Date"
                                         TargetControlID="txtToDate">
                                     </cc1:MaskedEditExtender>
                                     <asp:ImageButton ID="ImgBtnToDt" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.png"
                                         TabIndex="-1" /><br />
                                     <cc1:MaskedEditValidator ID="MevToDate" runat="server" ControlExtender="MeToDate"
                                         ControlToValidate="txtToDate" CssClass="field_error" Display="Dynamic" EmptyValueBlurredText="Date is required"
                                         EmptyValueMessage="Date is required" ErrorMessage="MeToDate" InvalidValueBlurredMessage="Invalid Date"
                                         InvalidValueMessage="Invalid Date" TooltipMessage="Input a Date in Date/Month/Year">
                                         &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</cc1:MaskedEditValidator>
                                 </ItemTemplate>
                                 <HeaderStyle Wrap="False" />
                                 <ItemStyle Wrap="False" />
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
                         <FooterStyle BackColor="White" ForeColor="#000066" />
                         <RowStyle CssClass="grdRowstyle" />
                         <SelectedRowStyle CssClass="grdselectrowstyle" />
                         <PagerStyle CssClass="grdpagerstyle" HorizontalAlign="Center" />
                         <HeaderStyle CssClass="grdheader" />
                         <AlternatingRowStyle CssClass="grdAternaterow" />
                     </asp:GridView>
                 </div>
             </td>
               <td align="left" class="td_cell"  style="HEIGHT: 22px">
                  <asp:Button ID="btnGenerate" runat="server" CssClass="field_button" 
                       onclick="btnGenerate_Click" tabIndex="5" Text="Generate" />
                       <asp:Button ID="Button1" runat="server" CssClass="field_button" Height="20px" 
                       onclick="btnhelp_Click" tabIndex="22" Text="Help" />

                       </td>
             <td align="left" class="td_cell" style="HEIGHT: 22px">
                 &nbsp;</td>
         </tr>
        
       </TBODY>
       <caption>
           <input id="txtconnection" runat="server" style="visibility: hidden; width: 12px;
        height: 9px" type="text" />
           </TD>
           </TR>
           <tr style ="display:none">
               <td align="left" class="td_cell" style="WIDTH: 201px">
                   <%--<SPAN style="FONT-FAMILY: Arial">Active</SPAN>--%>
                   <INPUT id="ChkActive" visible =false tabIndex=19  type=checkbox CHECKED 
           runat="server" />
                   
<asp:Label ID='lblactive' Text='Active' Visible=false  runat="server" ></asp:Label></td>
               
               <td style=" HEIGHT: 23px ">
                   <asp:CheckBox ID="chkConsdierForMarkUp" runat="server" Font-Bold="False" 
                       Text="Consider this supplier for markup " Visible="False" />
               </td>
           </tr>
           <tr>
             <td style ="display:none">
                   &nbsp;
                   <asp:Button ID="btnCancel1" runat="server" CssClass="field_button" 
                        tabIndex="21" Text="Return to Search" />
                   &nbsp;
                  
               </td>
           </tr>
             <tr>
                 <td align="left" class="td_cell" colspan="4" style="height: 22px">
                     <asp:Panel ID="Panel1" runat="server" ScrollBars="Auto" Width="1200px">
                         <table>
                             <tbody>
                             <tr>
                 <td style="background-color:#06788B;color:White;" align="left" colspan="2">
                     &nbsp;Tax Details</td>
           </tr>
                                 <tr>
                                     <td>
                                        <table>
                                                <tr>
                 <td align="right">
                     <asp:Label ID="Label5" runat="server" Text="VAT"></asp:Label>
                     <asp:Label ID="Label6" runat="server" ForeColor="#CC3300" Text="*"></asp:Label>
                 </td>
                 <td>
                     <asp:TextBox ID="txtVAT" runat="server"     AutoComplete="off" AutoPostBack="true" 
                                                             onkeypress="return   validateDecimalOnly(event,this)"  Width="75px"></asp:TextBox>%
                     &nbsp;<asp:CheckBox ID="chkPriceWithTax" runat="server" 
                         Text="All price are including tax" AutoPostBack="True" />
                 </td>
           </tr>
                                               </table></td>
                                 </tr>
                                 <tr>
                                     <td>
                                         <asp:GridView ID="gv_ExRates" runat="server" AutoGenerateColumns="False" 
                                             BackColor="White" BorderColor="#999999" BorderStyle="None" BorderWidth="1px" 
                                             CellPadding="3" CssClass="td_cell" Font-Size="10px" GridLines="Vertical" 
                                             tabIndex="20">
                                             <FooterStyle BackColor="#6B6B9A" ForeColor="Black" />
                                             <Columns>
                                                 <asp:TemplateField HeaderText=" ">
                                                     <ItemTemplate>
                                                         <asp:CheckBox ID="chkSelect" runat="server" onclick="CheckGridList(this);" />
                                                         <asp:Label ID="lblCLineno" runat="server" CssClass="displaynone" Text=""></asp:Label>
                                                     </ItemTemplate>
                                                     <ItemStyle HorizontalAlign="Center" />
                                                 </asp:TemplateField>
                                                 <asp:TemplateField HeaderText="Supplier">
                                                     <ItemStyle HorizontalAlign="Center" />
                                                     <HeaderStyle HorizontalAlign="Left" Width="60px" />
                                                     <ItemTemplate>
                                                         <asp:TextBox ID="txtsuppname" runat="server" CssClass="field_input" 
                                                             onkeyup="SetContextKey()" Style="text-align: left" Width="220px"></asp:TextBox>
                                                         <asp:TextBox ID="txtsuppcode" runat="server" Style="display: none"></asp:TextBox>
                                                         <asp:AutoCompleteExtender ID="txtsuppname_AutoCompleteExtender" runat="server" 
                                                             CompletionInterval="10" 
                                                             CompletionListCssClass="autocomplete_completionListElement" 
                                                             CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem" 
                                                             CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="1" 
                                                             DelimiterCharacters="" EnableCaching="false" Enabled="True" 
                                                             FirstRowSelected="false" MinimumPrefixLength="0" 
                                                             OnClientItemSelected="suppnameautocompleteselected" 
                                                             ServiceMethod="Getsupplierlist" TargetControlID="txtsuppname" 
                                                             UseContextKey="true">
                                                         </asp:AutoCompleteExtender>
                                                     </ItemTemplate>
                                                     <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                                                     <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                                 </asp:TemplateField>
                                                 <asp:TemplateField HeaderText="Service Type">
                                                     <ItemTemplate>
                                                         &nbsp;<asp:TextBox ID="txtservicetype" runat="server" CssClass="field_input" 
                                                             Style="text-align: left" TextMode="MultiLine" Width="400px"></asp:TextBox>
                                                     </ItemTemplate>
                                                     <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                                                     <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                                 </asp:TemplateField>
                                                 <asp:TemplateField HeaderText="Per Adult   <br />TV VAT">
                                                     <ItemStyle HorizontalAlign="Center" />
                                                     <HeaderStyle HorizontalAlign="Left" Width="40px" />
                                                     <ItemTemplate>
                                                         &nbsp;<asp:TextBox ID="txtperAdult" runat="server" CssClass="field_input" 
                                                             Style="text-align: center" Width="74px"></asp:TextBox>
                                                               <br />
                                                         <div style="padding-top:3px;width:83px;">
                                                         <asp:TextBox ID="txtAdultTV"  CssClass="field_input"  AutoComplete="off" placeholder="&nbsp;&nbsp;TV"  onkeydown="fnReadOnly(event)"   runat="server" Width="36px"></asp:TextBox>
                                                         <asp:TextBox ID="txtAdultVAT"  CssClass="field_input"  AutoComplete="off" placeholder="&nbsp;VAT"  onkeydown="fnReadOnly(event)"  runat="server" Width="36px"></asp:TextBox>
                                                         </div>
                                                     </ItemTemplate>
                                                     <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                                                     <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                                 </asp:TemplateField>
                                                 <asp:TemplateField HeaderText="Per Child   <br />TV VAT">
                                                     <ItemStyle HorizontalAlign="Center" />
                                                     <HeaderStyle HorizontalAlign="Left" Width="40px" />
                                                     <ItemTemplate>
                                                         &nbsp;<asp:TextBox ID="txtperChild" runat="server" CssClass="field_input" 
                                                             Style="text-align: center" Width="74px"></asp:TextBox>
                                                              <br />
                                                         <div style="padding-top:3px;width:83px;">
                                                         <asp:TextBox ID="txtChildTV"  CssClass="field_input"  AutoComplete="off" placeholder="&nbsp;&nbsp;TV"  onkeydown="fnReadOnly(event)"   runat="server" Width="36px"></asp:TextBox>
                                                         <asp:TextBox ID="txtChildVAT"  CssClass="field_input"  AutoComplete="off" placeholder="&nbsp;VAT"  onkeydown="fnReadOnly(event)"  runat="server" Width="36px"></asp:TextBox>
                                                         </div>
                                                     </ItemTemplate>
                                                     <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                                                     <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                                 </asp:TemplateField>
                                                 <asp:TemplateField HeaderText="Per Senior   <br />TV VAT">
                                                     <ItemStyle HorizontalAlign="Center" />
                                                     <HeaderStyle HorizontalAlign="Left" Width="40px" />
                                                     <ItemTemplate>
                                                         &nbsp;<asp:TextBox ID="txtperSenior" runat="server" CssClass="field_input" 
                                                             Style="text-align: center" Width="74px"></asp:TextBox>
                                                                 <br />
                                                         <div style="padding-top:3px;width:83px;">
                                                         <asp:TextBox ID="txtSeniorTV"  CssClass="field_input"  AutoComplete="off" placeholder="&nbsp;&nbsp;TV"  onkeydown="fnReadOnly(event)"  runat="server" Width="36px"></asp:TextBox>
                                                         <asp:TextBox ID="txtSeniorVAT"  CssClass="field_input"  AutoComplete="off" placeholder="&nbsp;VAT"  onkeydown="fnReadOnly(event)"  runat="server" Width="36px"></asp:TextBox>
                                                         </div>
                                                     </ItemTemplate>
                                                     <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                                                     <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                                 </asp:TemplateField>
                                                 <asp:TemplateField HeaderText="Per Unit   <br />TV VAT">
                                                     <ItemStyle HorizontalAlign="Center" />
                                                     <HeaderStyle HorizontalAlign="Left" Width="40px" />
                                                     <ItemTemplate>
                                                         &nbsp;<asp:TextBox ID="txtperunit" runat="server" CssClass="field_input" 
                                                             Style="text-align: center" Width="74px"></asp:TextBox>
                                                                  <br />
                                                         <div style="padding-top:3px;width:83px;">
                                                         <asp:TextBox ID="txtUnitTV"  CssClass="field_input"  AutoComplete="off" placeholder="&nbsp;&nbsp;TV"  onkeydown="fnReadOnly(event)"   runat="server" Width="36px"></asp:TextBox>
                                                         <asp:TextBox ID="txtUnitVAT"  CssClass="field_input"  AutoComplete="off" placeholder="&nbsp;VAT"  onkeydown="fnReadOnly(event)"  runat="server" Width="36px"></asp:TextBox>
                                                         </div>
                                                     </ItemTemplate>
                                                     <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                                                     <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                                 </asp:TemplateField>
                                                 <asp:TemplateField HeaderText="Child Free Upto">
                                                     <ItemStyle HorizontalAlign="Center" />
                                                     <HeaderStyle HorizontalAlign="Left" Width="40px" />
                                                     <ItemTemplate>
                                                         &nbsp;<asp:TextBox ID="txtchildfreeupto" runat="server" CssClass="field_input" 
                                                             Style="text-align: center" Width="74px"></asp:TextBox>
                                                           
                                                     </ItemTemplate>
                                                     <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                                                     <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                                 </asp:TemplateField>
                                             </Columns>
                                             <RowStyle CssClass="grdRowstyle" ForeColor="Black" />
                                             <SelectedRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" />
                                             <PagerStyle CssClass="grdpagerstyle" ForeColor="Black" 
                                                 HorizontalAlign="Center" />
                                             <HeaderStyle CssClass="grdheader" Font-Bold="True" ForeColor="white" 
                                                 HorizontalAlign="Left" />
                                             <AlternatingRowStyle CssClass="grdAternaterow" Font-Size="10px" />
                                         </asp:GridView>
                                     </td>
                                 </tr>
                             </tbody>
                         </table>
                     </asp:Panel>
                 </td>
                 <td align="left" class="td_cell" style="height: 22px">
                     &nbsp;</td>
                </tr>
                  <tr>
                     <td colspan="4">
                         &nbsp;</td>
                    
                 
                      <td> 
                          &nbsp;</td>
                    
                 
                </tr>
   

                <tr>
                    <td colspan="4">
                        <table style="width: 100%">
                            <tr>
                                <td>
                                    <asp:Button ID="btnAddrow" runat="server" CssClass="btn" TabIndex="9" Text="Add Row" />
                                </td>
                                <td>
                                    <asp:Button ID="btndeleterow" runat="server" CssClass="btn" TabIndex="10" Text="Delete Row" />
                                </td>
                                <td style="width: 100%">
                                    <div id="divmarkup1" runat="server" >
                                        <table>
                                            <tr>
                                                <td class="td_cell">
                                                    Markup&nbsp;Type
                                                </td>
                                                <td class="td_cell">
                                                    <select id="ddlMarkuptype" runat="server" class="drpdown" name="D3" onchange="enablemarkuptype();">
                                                        <option value="Unit">Unit</option>
                                                        <option selected="selected"  value="Per Pax">Based on Pax</option>
                                                    </select>
                                                </td>
                                                <td class="td_cell">
                                                    Operator
                                                </td>
                                                <td>
                                                    <input id="txtoperator" runat="server" class="txtbox" tabindex="0" type="text" style="width: 40px;
                                                        text-align: center" />
                                                </td>
                                                <td align="right" class="td_cell">
                                                    <asp:Label ID="lblAdultMarkup" runat="server" Text="Markup for Adult"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtmarkup" runat="server" CssClass="field_input" Style="text-align: center"
                                                        Width="60px"></asp:TextBox>
                                                          <br />
                                                         <div style="padding-top:3px;width:83px;">
                                                         <asp:TextBox ID="txtMarkupAdultTV"  CssClass="field_input"  AutoComplete="off" placeholder="&nbsp;&nbsp;TV"  onkeydown="fnReadOnly(event)"   runat="server" Width="36px"></asp:TextBox>
                                                         <asp:TextBox ID="txtMarkupAdultVAT"  CssClass="field_input"  AutoComplete="off" placeholder="&nbsp;VAT"  onkeydown="fnReadOnly(event)"  runat="server" Width="36px"></asp:TextBox>
                                                         </div>
                                                </td>
                                                <td align="right" id="tdMarkupChild" runat="server" class="td_cell">
                                                    Markup for Child
                                                </td>
                                                <td class="td_cell" id="tdMarkupChild1" runat="server">
                                                    <asp:TextBox ID="txtChildMarkup" runat="server" CssClass="field_input" Style="text-align: center"
                                                        Width="60px"></asp:TextBox>
                                                           <br />
                                                         <div style="padding-top:3px;width:83px;">
                                                         <asp:TextBox ID="txtMarkupChildTV"  CssClass="field_input"  AutoComplete="off" placeholder="&nbsp;&nbsp;TV"  onkeydown="fnReadOnly(event)"   runat="server" Width="36px"></asp:TextBox>
                                                         <asp:TextBox ID="txtMarkupChildVAT"  CssClass="field_input"  AutoComplete="off" placeholder="&nbsp;VAT"  onkeydown="fnReadOnly(event)"  runat="server" Width="36px"></asp:TextBox>
                                                         </div>
                                                </td>
                                                <td align="right" id="tdMarkupSenior" runat="server" class="td_cell">
                                                    Markup for Senior
                                                </td>
                                                <td class="td_cell" id="tdMarkupSenior1" runat="server" >
                                                    <asp:TextBox ID="txtSeniorMarkup" runat="server" CssClass="field_input" Style="text-align: center"
                                                        Width="60px"></asp:TextBox>
                                                           <br />
                                                         <div style="padding-top:3px;width:83px;">
                                                         <asp:TextBox ID="txtMarkupSeniorTV"  CssClass="field_input"  AutoComplete="off" placeholder="&nbsp;&nbsp;TV"  onkeydown="fnReadOnly(event)"   runat="server" Width="36px"></asp:TextBox>
                                                         <asp:TextBox ID="txtMarkupSeniorVAT"  CssClass="field_input"  AutoComplete="off" placeholder="&nbsp;VAT"  onkeydown="fnReadOnly(event)"  runat="server" Width="36px"></asp:TextBox>
                                                         </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </td>
                                <td>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td>
                    </td>
           </tr>
           <tr>
               <td colspan="4">
              </td>
               <td>
                   &nbsp;</td>
           </tr>
   

                <tr>
                    <td>
                        &nbsp;</td>
                    <td>
                          <asp:CheckBox ID="chkapprove" runat="server" Font-Bold="False" 
                      class="td_cell"  Text="Approve/Unapprove"  /></td>
                    <td align="right">
                        <asp:Button ID="btnSave" runat="server" CssClass="field_button" tabIndex="21" 
                            Text="Save" />  </td>
                    <td>
                        <asp:Button ID="btncancel" runat="server" CssClass="field_button" 
                            onclick="btnCancel_Click" tabIndex="22" Text="Return to Search" /></td>
                    <td>
                        &nbsp;</td>
           </tr>
                <tr>
                  <td></td>
                  <td align="left" style="WIDTH: 103px">
                   
                      </td>
              
                </tr>
               
           <tr>
               <td>
                   &nbsp;</td>
               <td align="left" style="WIDTH: 103px">
                   &nbsp;</td>
           </tr>
               
           </TBODY>
       </caption>
       </TABLE>
           <%-- <SELECT style="WIDTH: 200px" id="ddlGroupCode" class="field_input" tabIndex=10 onchange="CallWebMethod('GroupCode');" visible="false" runat="server"> <OPTION selected></OPTION></SELECT>--%>
</contenttemplate>
</asp:UpdatePanel>
                  <asp:ScriptManagerProxy id="ScriptManagerProxy1" runat="server">
                    <services>
<asp:ServiceReference Path="~/clsServices.asmx"></asp:ServiceReference>
</services>
                </asp:ScriptManagerProxy>

</asp:Content>

