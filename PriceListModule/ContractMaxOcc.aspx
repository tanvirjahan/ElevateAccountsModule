<%@ Page Language="VB" MasterPageFile="~/SubPageMaster.master" AutoEventWireup="false" EnableEventValidation="false" CodeFile="ContractMaxOcc.aspx.vb" Inherits="ContractMaxOcc" %>

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
    
<%--    d = new Date();
$("#myimg").attr("src", "/myimg.jpg?"+d.getTime());--%>
    <%--*** Danny 8/3/18 >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>--%>
<script type="text/javascript">
    function uploadStarted() {

        $get("imgDisplay").style.display = "none";
    }
    function uploadComplete(sender, args) {
        var Btn_Sve = document.getElementById("<%=Btn_Save.ClientID%>");
        Btn_Sve.style.display = "none";
        var imgDisplay = $get("imgDisplay");
        imgDisplay.src = "images/loader.gif";
        imgDisplay.style.cssText = "";
        var img = new Image();
        img.onload = function () {
            imgDisplay.style.cssText = "height:100px;width:100px";
            imgDisplay.src = img.src;
        };

        img.src = "<%=ResolveUrl(UploadFolderPath) %>" + $get("<%=hdnpartycode.ClientID %>").value + '_' + $get("<%=RoomImgRow.ClientID %>").value + args.get_fileName();

        var extension = img.src.substr((img.src.lastIndexOf('.') + 1));
        switch (extension) {
            case 'jpg':
                Btn_Sve.style.display = "block";
                break;
            case 'png':
                Btn_Sve.style.display = "block";
                break;
            case 'jpeg':
                Btn_Sve.style.display = "block";
                break;
            case 'bmp':
                Btn_Sve.style.display = "block";
                break;
            default:
                alert('Not a valid Image.\n You can select only following file types (*.jpeg, *.jpg, *.png. *.bmp)');
                Btn_Sve.style.display = "none";


        }
        var FileName = document.getElementById("<%=SelectedRoomImgPath.ClientID%>");
        FileName.value = args.get_fileName();

        $('#<%=ImgRoomImage.ClientID%>').fadeOut(1000);

    }
</script>
<%--img.src = "<%=ResolveUrl(UploadFolderPath) %>" + $get("<%=hdnpartycode.ClientID %>").value + '_' + $get("<%=RoomImgRow.ClientID %>").value + '_main.jpg';--%>

    <style type="text/css">
        .FileUploadClass
        {
           
            font-size: 5px;
            width:100% !important;
        }
        .FileUploadClass input
        {
            background-color: transparent;
            border: solid 0px #000000;
            margin-top: 0px;
        }
        
        .ruFakeInput
        {
            
            width: 100;
            padding: 0;
        }
        .FileUploadClass div
        {
            width:100px !important;
            display: table-row !important;
        }
        .FileUploadClass div input
        {
             opacity:none  !important;
        }-
        .FileUploadClass div div
        {
            margin-top:0px  !important;
            
        }
        .FileUploadClass div div input
        {
           
        }
    </style>
<%--'*** Danny 8/3/18 <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<--%>
   <script type="text/javascript">


       function PopUpImageView(code) {

           var FileName = document.getElementById("<%=hdnFileName.ClientID%>");
           var lblfilename = document.getElementById("<%=lblimage.ClientID%>");
           if (FileName.value == "") {
               FileName.value = code;
           }
           if (lblfilename.innerText != "") {

               popWin = open('../PriceListModule/ImageViewWindow.aspx?code=' + FileName.value, 'myWin', 'resizable=yes,scrollbars=yes,width=500,height=350,left=450,top=80');
               popWin.focus();
               FileName.value = "";
               return false

           }
           else {

               popWin = open('../PriceListModule/ImageViewWindow.aspx?', 'myWin', 'resizable=yes,scrollbars=yes,width=500,height=350,left=450,top=80');
               popWin.focus();
           }
       }
       


    </script>
  <script type="text/javascript" charset="utf-8">
      $(document).ready(function () {


          AutoCompleteExtenderKeyUp();
          visualsearchbox();
          AutoCompleteExtenderUserControlKeyUp();


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
          $txtvsprocess.val('CountryGroup:" " Region:" " Country:" " Text:" "');

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

      function AutoCompleteExtenderKeyUp() {

          $("#<%=gv_Filldata.ClientID%> tr input[id*='txtrmclassname']").each(function () {
              $(this).change(function (event) {
                  var hiddenfieldID1 = $(this).attr("id").replace("txtrmclassname", "txtrmclassname");
                  var hiddenfieldID = $(this).attr("id").replace("txtrmclassname", "txtclasscode");

                  if ($get(hiddenfieldID1).value == '') {

                      $get(hiddenfieldID).value = '';
                  }
              });

              $(this).keyup(function (event) {
                  var hiddenfieldID1 = $(this).attr("id").replace("txtrmclassname", "txtrmclassname");
                  var hiddenfieldID = $(this).attr("id").replace("txtrmclassname", "txtclasscode");



                  if ($get(hiddenfieldID1).value == '') {

                      $get(hiddenfieldID).value = '';
                  }

              });


          });



          $("#<%= txtpromotionname.ClientID %>").bind("change", function () {
              document.getElementById('< %=txtpromotionid.ClientID%>').value = '';
          });




          //          });

      }
      var txtcombination;
      var txtcombinationnew;
      var rw;
      function getmaxcombination(txtadt, txtchd, txtmaxocc, partycode, rmtypcode, txtcombnew, txtcomb, startbase, rowind) {
          rw = parseInt(rowind);
          var connstr = document.getElementById("<%=txtconnection.ClientID%>");
          constr = connstr.value;
          var txtmaxadt = document.getElementById(txtadt);
          var txtmaxchd = document.getElementById(txtchd);
          var txtmaxocc = document.getElementById(txtmaxocc);
          var txtpartycode = document.getElementById(partycode);
          var txtrmtypcode = document.getElementById(rmtypcode);
          txtcombination = document.getElementById(txtcomb);
          txtcombinationnew = document.getElementById(txtcombnew);
          var txtstartbased = document.getElementById(startbase);

          txtcombination.value = '';
          txtcombinationnew = '';

          // ColServices.clsServices.Fillcombination(constr, txtmaxadt.value, txtmaxchd.value, txtmaxocc.value, txtstartbased.textContent, txtpartycode.value, txtrmtypcode.value, fillcombination, ErrorHandler, TimeOutHandler);

      }

      //      function fillcombination(result) {
      //          var objGridView = document.getElementById('< %=gv_Filldata.ClientID%>');

      //          if (result == '') {
      //              txtcombination.value = '';
      //              txtcombinationnew = '';
      //          }
      //          else {

      //  
      //              txtcombination.value = result; 
      //              txtcombinationnew = result;  

      //     

      //          }

      //      }

      function Enablepricepax(chkunit, txtpricepax, txtnounit, rowind) {
          rw = parseInt(rowind);
          var chkunit = document.getElementById(chkunit);
          var txtpricepax = document.getElementById(txtpricepax);
          var txtnounit = document.getElementById(txtnounit);

          if (chkunit.checked) {
              txtpricepax.disabled = false;
              txtnounit.disabled = false;
          }
          else {
              txtpricepax.disabled = true;
              txtnounit.disabled = true;

          }
      }

      function showusercontrol(chkctrygrpid) {
          var chkctrygrp = document.getElementById(chkctrygrpid);
          if (chkctrygrp.checked) {
              $("#" + "<%=divuser.ClientID %>").slideDown();
          }
          else {
              $("#" + "<%=divuser.ClientID %>").fadeOut();
          }
      }


      function setrowcolor(chkactive) {

          if (chkactive.checked)
              chkactive.parentNode.parentNode.style.backgroundColor = "#F8CBAD";
          else
              chkactive.parentNode.parentNode.style.backgroundColor = "";
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
              return;
          }
          if (SelectedRow != null) {

              SelectedRow.style.backgroundColor = SelectedRow.originalBackgroundColor;
              SelectedRow.style.color = SelectedRow.originalForeColor;
          }

          if (CurrentRow != null) {
              CurrentRow.originalBackgroundColor = CurrentRow.style.backgroundColor;
              CurrentRow.originalForeColor = CurrentRow.style.color;
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

          var iflag = 0;
          if (SelectedRowIndex != Cur_row.rowIndex - 1) {
              iflag = 1;
          }
          var e = e ? e : window.event;
          var KeyCode = e.which ? e.which : e.keyCode;
          if (KeyCode == 40 || KeyCode == 38 || KeyCode == 9) {
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

<script language="javascript" type="text/javascript" >



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

    function checkfromdates(txtfromdate, txtodate) {

        var fdate = document.getElementById(txtfromdate);
        var tdate = document.getElementById(txtodate);

        if (fdate.value == null || fdate.value == "") {
            alert("Please select from date.");
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

    function checkdates(txtfromdate, txtodate) {

        var fdate = document.getElementById(txtfromdate);
        var tdate = document.getElementById(txtodate);



        if (fdate.value == null || fdate.value == "") {
            alert("Please select from date.");
        }

        var dp = fdate.value.split("/");
        var newfdate = new Date(dp[2] + "/" + dp[1] + "/" + dp[0]);

        var dp1 = tdate.value.split("/");
        var newtdate = new Date(dp1[2] + "/" + dp1[1] + "/" + dp1[0]);

        newfdate = getFormatedDate(newfdate);
        newtdate = getFormatedDate(newtdate);

        newfdate = new Date(newfdate);
        newtdate = new Date(newtdate);
        if (newtdate < newfdate) {
            tdate.value = "";
            alert("To date should  be greater than From date");
        }



        setdate();
    }

    function setdate() {
        var btnDummy = document.getElementById('<%=btnDummy.ClientID %>');
        btnDummy.click();
    }

    function FormValidationMainDetail(state) {
        var txtnameval = document.getElementById("<%=txtname.ClientID%>");
        if (txtnameval.value == '') {
            //            alert('Name Cannot be blank');
            //            return false;
        }
        else {
            if (state == 'New') { if (confirm('Are you sure you want to save Max Occupancy ') == false) return false; }
            if (state == 'Edit') { if (confirm('Are you sure you want to update?') == false) return false; }
            if (state == 'Delete') { if (confirm('Are you sure you want to delete?') == false) return false; }
        }
    }



    function formmodecheck() {
        var vartxtcode = document.getElementById("<%=txthotelname.ClientID%>");


        if ((vartxtcode.value == '')) {
            doLinks(false);
        }
        else {
            doLinks(true);
        }


    }

    function doLinks(how) {
        for (var l = document.links, i = l.length - 1; i > -1; --i)
            if (!how)
                l[i].onclick = function () { alert('Please Save Main details to continue'); return false; };
            else
                l[i].onclick = function () { return true; };
    }
    function load() {

        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(formmodecheck);
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




    function SetContextKey() {
        $find('< %=AutoCompleteExtender1.ClientID%>').set_contextKey($get("<%=hdnpartycode.ClientID %>").value);
    }

    function checkNumber(e) {


        if ((event.keyCode < 48 || event.keyCode > 57)) {
            return false;
        }

    }




    function seasonautocompleteselected(source, eventArgs) {

        var hiddenfieldID = source.get_id().replace("AutoCompleteExtender2", "txtclasscode");
        $get(hiddenfieldID).value = eventArgs.get_value();

    }
    function promotionautocompleteselected(source, eventArgs) {

        if (eventArgs != null) {
            document.getElementById('<%=txtpromotionid.ClientID%>').value = eventArgs.get_value();
        }

        else {

            document.getElementById('<%=txtpromotionid.ClientID%>').value = '';
        }
    }


    
        	
</script>


 <style>
     
          .displaynone
        {
        	display:none;
        }
        
        .autocomplete_completionListElement
        {
            visibility: hidden;
            margin: 1px 0px 0px 0px !important;
            background-color: #FFFFFF;
            color: windowtext;
            border: buttonshadow;
            border-width: 1px;
            border-style: solid;
            cursor: 'default';
            overflow: auto;
            height: 200px;
            width: 100px;
            text-align: left;
            list-style-type: none;
            font-family: Verdana;
            font-size: small;
        }
        
        
        /* AutoComplete highlighted item */
        
        
        .autocomplete_highlightedListItem
        {
            background-color: Silver;
            color: black;
            margin-left: -35px;
            font-weight: bold;
        }
        
        
        /* AutoComplete item */
        
        .autocomplete_listItem
        {
            background-color: window;
            color: windowtext;
            margin-left: -35px;
        }
    </style>
    <style type="text/css">
        .ModalPopupBG
        {
            background-color: gray;
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
        }
    </style>

    <asp:UpdatePanel id="UpdatePanel2" runat="server">
        <contenttemplate>
            <table style="width: 100%; height: 100%; border-right: gray 2px solid; border-top: gray 2px solid;
                border-left: gray 2px solid; border-bottom: gray 2px solid" class="td_cell" align="left">
                <tbody>
                    <tr>
                        <td valign="top" align="center" width="150" colspan="4">
                            <asp:Label ID="lblHeading" runat="server" Text="Max Occupancy" CssClass="field_heading"
                                Width="100%" ForeColor="White"></asp:Label>
                        </td>
                    </tr>
                    <tr style="display: none">
                        <td valign="top" align="left" width="150">
                            &nbsp;
                        </td>
                        <td class="td_cell" valign="top" align="left" colspan="3">
                            &nbsp;
                        </td>
                    </tr>
                    <tr style="display: none">
                        <td align="left" valign="top" width="150">
                            &nbsp;
                        </td>
                        <td align="left" class="td_cell" valign="top" colspan="3">
                            Hotel <span class="td_cell" style="color: #ff0000">*&nbsp; </span>
                            <input style="width: 196px" id="txtCode" class="field_input" tabindex="1" type="text"
                                maxlength="20" runat="server" />
                            &nbsp; Name&nbsp;
                            <input style="width: 213px" id="txtName" class="field_input" tabindex="2" type="text"
                                maxlength="100" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td align="left" valign="top" width="150" rowspan="2">
                            <div id="Div1" style="height: 402px;">
                                <uc1:SubMenuUserControl ID="SubMenuUserControl1" runat="server" />
                            </div>
                        </td>
                        <td align="left" class="td_cell" valign="top" colspan="3">
                            <table style="width: 100%">
                                <tr>
                                    <td align="left" class="td_cell" valign="top">
                                        <asp:Panel ID="Panelsearch" runat="server" Font-Bold="true" GroupingText="Search Details"
                                            Width="100%">
                                            <table style="width: 100%">
                                                <tr>
                                                    <td width="120px">
                                                        <asp:Button ID="btnAddNew" runat="server" Text="Add New" Font-Bold="False" CssClass="btn">
                                                        </asp:Button>
                                                    </td>
                                                     <td class="td_cell" valign="top" align="left">
                                                        <asp:Button ID="btncopycontract" runat="server" Text="Copy From Another Max Occupancy"
                                                            Font-Bold="False" CssClass="btn" Width="250px"></asp:Button>
                                                    </td>
                                                      <td class="td_cell" valign="top" align="left">
                                                        <asp:Button ID="btnselect" runat="server" Text="Copy From Another Offer" Font-Bold="False"
                                                            CssClass="btn" OnClick="btnselect_Click"></asp:Button>
                                                    </td>
                                                 
                                                    <td class="td_cell">
                                                        <asp:Label ID="lblsortby" runat="server" Style="width: 20px;" CssClass="field_caption"
                                                            Text="Sort By "></asp:Label>
                                                        <asp:DropDownList ID="ddlOrder" AutoPostBack="true" runat="server">
                                                            <asp:ListItem Value="I">Max Occ.ID</asp:ListItem>
                                                            <asp:ListItem Value="F">From Date</asp:ListItem>
                                                            <asp:ListItem Value="T">To Date</asp:ListItem>
                                                            <asp:ListItem Value="A">Approved</asp:ListItem>
                                                            <asp:ListItem Value="C">Created Date</asp:ListItem>
                                                            <asp:ListItem Value="CU">Created User</asp:ListItem>
                                                            <asp:ListItem Value="M">Modified Date</asp:ListItem>
                                                            <asp:ListItem Value="MU">Modified User</asp:ListItem>
                                                        </asp:DropDownList>
                                                        <asp:DropDownList ID="ddlorderby" AutoPostBack="true" runat="server">
                                                            <asp:ListItem Value="A">ASC</asp:ListItem>
                                                            <asp:ListItem Value="D">DESC</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                   
                                                    <td style="display: none">
                                                        <asp:Button ID="btnExportToExcel" TabIndex="16" runat="server" CssClass="field_button">
                                                        </asp:Button>
                                                        <asp:Button ID="btnprint" TabIndex="16" runat="server" CssClass="field_button"></asp:Button>
                                                    </td>
                                                    <td>
                                                        &nbsp;
                                                    </td>
                                                    <td>
                                                        &nbsp;
                                                    </td>



                                                </tr>
                                                <tr>
                                                    <td style="width: 100%" valign="top" colspan="5">
                                                        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                                            <ContentTemplate>
                                                                <table width="925px">
                                                                    <tr>
                                                                        <td class="field_heading" colspan="4">
                                                                            <asp:Label ID="Label1" runat="server" Text="List Of Entries"></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <div id="searchresults">
                                                        <td style="width: 100%" valign="top" colspan="5">
                                                            <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                                                <ContentTemplate>
                                                                    <table width="100%">
                                                                        <tr>
                                                                            <td>
                                                                                <div id="Showdetails" runat="server" style="width: 900px; border: 3px solid #2D7C8A;
                                                                                    background-color: White;">
                                                                                    <asp:GridView ID="gv_SearchResult" runat="server" Font-Size="10px" Width="890px"
                                                                                        CssClass="grdstyle" __designer:wfdid="w42" GridLines="Vertical" CellPadding="3"
                                                                                        BorderWidth="1px" BorderStyle="Solid" BorderColor="#999999" AutoGenerateColumns="False"
                                                                                        AllowSorting="True" AllowPaging="True">
                                                                                        <FooterStyle CssClass="grdfooter"></FooterStyle>
                                                                                        <Columns>
                                                                                            <asp:TemplateField Visible="False" HeaderText="Supplier Code">
                                                                                                <EditItemTemplate>
                                                                                                </EditItemTemplate>
                                                                                                <ItemTemplate>
                                                                                                    <asp:Label ID="lbltran" runat="server" Text='<%# Bind("tranid") %>' __designer:wfdid="w1"></asp:Label>
                                                                                                </ItemTemplate>
                                                                                            </asp:TemplateField>
                                                                                            <asp:BoundField DataField="tranid" HeaderText="Max Occ.ID"></asp:BoundField>
                                                                                            <asp:BoundField DataField="FrmDate" HeaderText="From Date"></asp:BoundField>
                                                                                            <asp:BoundField DataField="todate" HeaderText="To Date"></asp:BoundField>
                                                                                            <asp:BoundField DataField="countrygroups"  Visible ="false" HeaderText="Country Group" ></asp:BoundField>
                                                                                            <asp:BoundField DataField="promotionid" HeaderText="Promotion ID"></asp:BoundField>
                                                                                            <asp:BoundField DataField="promotionname" HeaderText="Promotion Name"></asp:BoundField>
                                                                                            <asp:BoundField DataField="status" HeaderText="Approved"></asp:BoundField>
                                                                                            <asp:ButtonField HeaderText="Action" Text="Edit" CommandName="EditRow">
                                                                                                <ItemStyle ForeColor="Blue"></ItemStyle>
                                                                                            </asp:ButtonField>
                                                                                            <asp:ButtonField HeaderText="Action" Text="View" CommandName="View">
                                                                                                <ItemStyle ForeColor="Blue"></ItemStyle>
                                                                                            </asp:ButtonField>
                                                                                            <asp:ButtonField HeaderText="Action" Text="Delete" CommandName="DeleteRow">
                                                                                                <ItemStyle ForeColor="Blue"></ItemStyle>
                                                                                            </asp:ButtonField>
                                                                                            <asp:ButtonField HeaderText="Action" Text="Copy" CommandName="Copy">
                                                                                                <ItemStyle ForeColor="Blue"></ItemStyle>
                                                                                            </asp:ButtonField>
                                                                                            <asp:BoundField HtmlEncode="False" DataFormatString="{0:dd/MM/yyyy HH:mm:ss }" DataField="adddate"
                                                                                                SortExpression="adddate" HeaderText="Date Created"></asp:BoundField>
                                                                                            <asp:BoundField DataField="adduser" SortExpression="adduser" HeaderText="User Created">
                                                                                            </asp:BoundField>
                                                                                            <asp:BoundField HtmlEncode="False" DataFormatString="{0:dd/MM/yyyy HH:mm:ss }" DataField="moddate"
                                                                                                SortExpression="moddate" HeaderText="Date Modified"></asp:BoundField>
                                                                                            <asp:BoundField DataField="moduser" SortExpression="moduser" HeaderText="User Modified">
                                                                                            </asp:BoundField>
                                                                                        </Columns>
                                                                                        <FooterStyle BackColor="White" ForeColor="#000066" />
                                                                                        <RowStyle CssClass="grdRowstyle"></RowStyle>
                                                                                        <SelectedRowStyle CssClass="grdselectrowstyle"></SelectedRowStyle>
                                                                                        <PagerStyle CssClass="grdpagerstyle" HorizontalAlign="Center"></PagerStyle>
                                                                                        <HeaderStyle CssClass="grdheader" ForeColor="white"></HeaderStyle>
                                                                                        <AlternatingRowStyle CssClass="grdAternaterow"></AlternatingRowStyle>
                                                                                    </asp:GridView>
                                                                                </div>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </td>
                                                    </div>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                    </td>
                                </tr>
                              
                              
                                <tr> 
                                    <td colspan="2">
                                           <div id="Divmain" runat="server">
                                            <asp:Panel ID="PanelMain" runat="server" Font-Bold="true" GroupingText="Entry Details"
                                                Width="100%" style="display:none">
                                                <table>
                                                   
                                                    <tr>
                                                        <td>
                                                            <div style="width: 100%; min-height: 25px" id="Div3" runat="server">
                                                            </div>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <div id="hoteldetails">
                                                            <td>
                                                                Hotel&nbsp;Name
                                                            </td>
                                                            <td align="left" class="td_cell" valign="top">
                                                                <asp:TextBox ID="txthotelname" runat="server" CssClass="field_input" TabIndex="1"
                                                                    Width="250px" Enabled="false"></asp:TextBox>
                                                                Auto&nbsp;ID
                                                                <asp:TextBox ID="txtMaxid" runat="server" ReadOnly="true" Width="120px" TabIndex="2"></asp:TextBox>
                                                            
                                                            </td>
                                                            <td style ="display:none">
                                                                  &nbsp;Country Groups&nbsp;
                                                            <asp:CheckBox ID="chkctrygrp" runat="server" class="cls_chkctrygrp" TabIndex="4" />
                                                            </td>
                                                            <td>
                                                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                            </td>
                                                            <td>
                                                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                            </td>
                                                        </div>
                                                    </tr>
                                                    <tr>
                                                     
                                        
                                                        <td>
                                                            <asp:Label ID="lblpromotion" Text =" Promotion Link" Style="display: none" runat ="server"></asp:Label>
                                                            
                                                        </td>
                                                        <td align="left" class="td_cell" valign="top" colspan="2">
                                                            <asp:TextBox ID="txtpromotionname" runat="server" __designer:wfdid="w77" AutoPostBack="True"
                                                                CssClass="field_input" MaxLength="500"  Style="display: none" TabIndex="3" onkeyup="SetContextKey()"
                                                                Width="250px"></asp:TextBox>
                                                            <asp:HiddenField ID="hdnpartycode" runat="server" />
                                                            <asp:TextBox ID="txtpromotionid" runat="server" Style="display: none"></asp:TextBox>
                                                            <asp:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" CompletionInterval="10"
                                                                CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                                CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="1" DelimiterCharacters=""
                                                                EnableCaching="false" Enabled="True" FirstRowSelected="True" MinimumPrefixLength="1"
                                                                OnClientItemSelected="promotionautocompleteselected" ServiceMethod="Getpromotionlist"
                                                                TargetControlID="txtpromotionname" UseContextKey="true">
                                                            </asp:AutoCompleteExtender>
                                                          
                                                        </td>
                                                       
                                                        <td>
                                                            <asp:TextBox ID="dpTxtFromDate" runat="server" CssClass="fiel_input" Width="80px"
                                                                Style="display: none"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="dptxtTodate" runat="server" CssClass="fiel_input" Width="80px" Style="display: none"></asp:TextBox>
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
                                                    </tr>
                                                    <tr>
                                                                                   
                                                    <td colspan ="20">
                                                        <div id="divoffer" runat="server">
                                                        <table>
                                                        <tr>
                                                        <td>
                                                            <asp:Label ID="lblselect" runat="server" Style="vertical-align: bottom;" Text="Offers"
                                                                Width="100px"></asp:Label>
                                                        </td>
                                                        <td>
                                                                <asp:TextBox ID="txtpromotionidnew" runat="server" ReadOnly="true" Width="130px" TabIndex="2"></asp:TextBox>
                                                            <asp:TextBox ID="txtpromotionnamenew" runat="server" ReadOnly="true" Width="300px" TabIndex="2"></asp:TextBox>
                                                        </td>
                                                       
                                                        </tr>
                                                                                      

                                                        </table>

                                                        </div>
                                                    </td>
                                                                                   
                                                    </tr>
                                                    <tr>
                                                    <td  colspan="2">
                                                    <table width="80%">
                                                     <tr>
                                                     <td>
                                                     <div id="divuser" runat="server">
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
                                                                    <asp:Button ID="btnvsprocess" runat="server" Style="display: none" />
                                                                </div>
                                                                <div id="countrygroup1" style="float: left; margin-left: 10px; width: 100%">
                                                                    <uc2:Countrygroup ID="wucCountrygroup" runat="server" />
                                                                </div>
                                                            </div>
                                                      </td>
                                                     </tr>
                                                    </table>
                                                    
                                                    </td>
                                                    </tr>  
                                                    <tr>
                                                        <td colspan="2" >
                                                            <table>
                                                                <tr>
                                                                    <td width="280px">
                                                                    <div id="divdates" runat="server">
                                                                        <asp:GridView ID="grdDates" runat="server" AllowSorting="True" AutoGenerateColumns="False"
                                                                            BorderStyle="Solid" BorderWidth="1px" CellPadding="3" CssClass="grdstyle" Font-Size="10px"
                                                                            GridLines="Vertical" TabIndex="26" Width="1px">
                                                                            <FooterStyle CssClass="grdfooter" />
                                                                            <Columns>
                                                                                <asp:BoundField DataField="SrNo" HeaderText="Sr No" Visible="False"></asp:BoundField>
                                                                                <asp:TemplateField HeaderText="From Date">
                                                                                    <ItemTemplate>
                                                                                        <%--<ews:DatePicker id="dpFromDate" tabIndex=9 runat="server" CssClass="field_input" Width="171px" RegExErrorMessage="Please enter a date in the format: dd/mm/yyyy" DateFormatString="dd/MM/yyyy"></ews:DatePicker>--%>
                                                                                        <asp:TextBox ID="txtfromDate" runat="server" CssClass="fiel_input" Width="80px">
                                                                                        </asp:TextBox>
                                                                                        <asp:CalendarExtender ID="dpFromDate" runat="server" Format="dd/MM/yyyy" PopupButtonID="ImgBtnFrmDt"
                                                                                            PopupPosition="Right" TargetControlID="txtfromDate">
                                                                                        </asp:CalendarExtender>
                                                                                        <asp:MaskedEditExtender ID="MeFromDate" runat="server" Mask="99/99/9999" MaskType="Date"
                                                                                            TargetControlID="txtfromDate">
                                                                                        </asp:MaskedEditExtender>
                                                                                        <asp:ImageButton ID="ImgBtnFrmDt" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.png" TabIndex="-1" />
                                                                                        <br />
                                                                                        <asp:MaskedEditValidator ID="MevFromDate" runat="server" ControlExtender="MeFromDate"
                                                                                            ControlToValidate="txtfromDate" CssClass="field_error" Display="Dynamic" EmptyValueBlurredText="Date is required"
                                                                                            EmptyValueMessage="Date is required" ErrorMessage="MeFromDate" InvalidValueBlurredMessage="Invalid Date"
                                                                                            InvalidValueMessage="Invalid Date" TooltipMessage="Input a Date in Date/Month/Year">
                                                                                        </asp:MaskedEditValidator>
                                                                                    </ItemTemplate>
                                                                                    <HeaderStyle Wrap="False" />
                                                                                    <ItemStyle Wrap="False" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="To Date">
                                                                                    <ItemTemplate>
                                                                                        <%--<ews:DatePicker id="dpFromDate" tabIndex=9 runat="server" CssClass="field_input" Width="171px" RegExErrorMessage="Please enter a date in the format: dd/mm/yyyy" DateFormatString="dd/MM/yyyy"></ews:DatePicker>--%>
                                                                                        <asp:TextBox ID="txtToDate" runat="server" CssClass="fiel_input" Width="80px">
                                                                                        </asp:TextBox>
                                                                                        <asp:CalendarExtender ID="dpToDate" runat="server" Format="dd/MM/yyyy" PopupButtonID="ImgBtnToDt"
                                                                                            PopupPosition="Right" TargetControlID="txtToDate">
                                                                                        </asp:CalendarExtender>
                                                                                        <asp:MaskedEditExtender ID="MeToDate" runat="server" Mask="99/99/9999" MaskType="Date"
                                                                                            TargetControlID="txtToDate">
                                                                                        </asp:MaskedEditExtender>
                                                                                        <asp:ImageButton ID="ImgBtnToDt" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.png" TabIndex="-1" />
                                                                                        <br />
                                                                                        <asp:MaskedEditValidator ID="MevToDate" runat="server" ControlExtender="MeToDate"
                                                                                            ControlToValidate="txtToDate" CssClass="field_error" Display="Dynamic" EmptyValueBlurredText="Date is required"
                                                                                            EmptyValueMessage="Date is required" ErrorMessage="MeToDate" InvalidValueBlurredMessage="Invalid Date"
                                                                                            InvalidValueMessage="Invalid Date" TooltipMessage="Input a Date in Date/Month/Year">
                                                                                        </asp:MaskedEditValidator>
                                                                                    </ItemTemplate>
                                                                                    <HeaderStyle Wrap="False" />
                                                                                    <ItemStyle Wrap="False" />
                                                                                </asp:TemplateField>
                                                                                  <asp:TemplateField HeaderText=" ">
                                                                                <ItemTemplate>
                                                                                    <asp:CheckBox ID="chkSelect" runat="server" CssClass="field_input"></asp:CheckBox>
                                                                                </ItemTemplate>
                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                            </asp:TemplateField>
                                                                            </Columns>
                                                                            <FooterStyle BackColor="White" ForeColor="#000066" />
                                                                            <RowStyle CssClass="grdRowstyle" />
                                                                            <SelectedRowStyle CssClass="grdselectrowstyle" />
                                                                            <PagerStyle CssClass="grdpagerstyle" HorizontalAlign="Center" />
                                                                            <HeaderStyle CssClass="grdheader" />
                                                                            <AlternatingRowStyle CssClass="grdAternaterow" />
                                                                        </asp:GridView>
                                                                        <br />
                                                                        <asp:Button ID="btnAddLinesDates" runat="server" CssClass="btn" TabIndex="7" Text="Add Row" />
                                                                        <asp:Button ID="btndeletedates" runat="server" CssClass="btn" TabIndex="10" Text="Delete Row" />
                                                                        <br />
                                                                    </div>
                                                                    </td>
                                                                    <td valign="top" align="left" width="450px">
                                                                        <asp:Label ID="lbltext" runat="server" Visible="false" Font-Bold="true" Text="Leave dates blank for hotels which give same max 
                               occupancy for all dates Enter specific dates if hotel has given a different max 
                               occupancy for a certain period"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="3" style="width: 900px" valign="top">
                                                            <asp:UpdatePanel ID="upnloccupancy" runat="server">
                                                                <ContentTemplate>
                                                                    <table width="925px">
                                                                        <tr>
                                                                            <td class="field_heading" colspan="4">
                                                                                <asp:Label ID="Label5" runat="server" Text="Occupancy Details"></asp:Label>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td colspan="2" style="vertical-align: middle">
                                                                                <asp:Panel ID="pnlCopy" runat="server" Visible="true">
                                                                                    <asp:Label ID="lblCopyCri" runat="server" class="td_cell" Style="vertical-align: middle;"
                                                                                        Text="Copy Selected To :"></asp:Label>
                                                                                    <asp:DropDownList ID="ddlCopyCriteria" runat="server" CssClass="drpdown" onchange="CopySelected();"
                                                                                        TabIndex="8">
                                                                                        <asp:ListItem Value="0">All</asp:ListItem>
                                                                                        <asp:ListItem Value="1">Room Type</asp:ListItem>
                                                                                    </asp:DropDownList>
                                                                                    &nbsp;
                                                                                    <asp:Label ID="lblSelectRm" runat="server" class="td_cell" Style="vertical-align: middle;"
                                                                                        Text="Select Room"></asp:Label>
                                                                                    &nbsp;
                                                                                    <select id="ddlCopyRoom" runat="server" class="field_input" style="width: 221px">
                                                                                    </select>
                                                                                    &nbsp;
                                                                                    <asp:CheckBox ID="chkoccupancydetail" runat="server" AutoPostBack="true" CssClass="td_cell"
                                                                                        TabIndex="9" Text="Copy Occupancy Detail" />
                                                                                    &nbsp;&nbsp; &nbsp;
                                                                                    <asp:Button ID="btnCopySelected" runat="server" CssClass="btn" Text="Copy" />
                                                                                </asp:Panel>
                                                                                <br />
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
                                                                        <tr>
                                                                            <td colspan="4">

                                                                                <asp:GridView ID="gv_FillData" runat="server" AutoGenerateColumns="False" BorderStyle="Solid"
                                                                                    BorderWidth="1px" CellPadding="3" CssClass="grdstyle" Font-Size="10px" GridLines="Vertical"
                                                                                    TabIndex="10" Width="999px">
                                                                                    <FooterStyle CssClass="grdfooter" />
                                                                                    <Columns>
                                                                                    <%-- *** Danny 3/3/18>>>>>>>>>>>>>>>>>>>>>>>>>>--%>
                                                                                    <asp:TemplateField>
                                                                                            <ItemTemplate>
                                                                                                    <asp:HiddenField ID="RoomImgPath" runat="server" />  
                                                                                                        <style>
                                                                                                            .imgcls
                                                                                                            {
                                                                                                                width:25px;
                                                                                                                height :25px;
                                                                                                                border-radius: 5px;
                                                                                                                box-shadow: 1px 1px #333;
                                                                                                            }
                                                                                                        </style>                                                                                          
                                                                                                <asp:ImageButton ID="RoomImages" runat="server" AlternateText="" ImageAlign="left"
                                                                                                    ImageUrl='<%# Bind("RoomImages") %>' OnClick="RoomImages_Click" ToolTip="Click to Add/Edit Image" CssClass="imgcls"/>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <%-- *** Danny 3/3/18<<<<<<<<<<<<<<<<<<<--%>
                                                                                        <asp:TemplateField>
                                                                                            <ItemTemplate>
                                                                                                <asp:CheckBox ID="chkSelect" runat="server" />
                                                                                            </ItemTemplate>
                                                                                            <HeaderStyle Width="30px" />
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Room Type Code" Visible="false">
                                                                                            <ItemTemplate>
                                                                                                <asp:Label ID="lblRoomTypeCode" runat="server" Text='<%# Bind("rmtypcode") %>'></asp:Label>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Room Name">
                                                                                            <EditItemTemplate>
                                                                                                &nbsp;<asp:Label ID="lblRoomTypename" runat="server" Text='<%# Bind("rmtypname") %>'></asp:Label>
                                                                                            </EditItemTemplate>
                                                                                            <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                                                                                            <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                                                                            <ItemTemplate>
                                                                                                <asp:TextBox ID="txtrmtype" runat="server" Style="display: none" Text='<%# Bind("rmtypcode") %>'></asp:TextBox>
                                                                                                <asp:TextBox ID="txtrmtypename" runat="server"  CssClass="field_input" Text='<%# Bind("rmtypname") %>'
                                                                                                    Width="200px"  style="text-transform:uppercase;" ></asp:TextBox>
                                                                                                    <asp:ImageButton ID="imgbEditnew" runat="server" 
                                                                                                        ImageUrl="~/Images/crystaltoolbar/edit.png" onclick="imgbEditnew_Click" 
                                                                                                        Width="25px" ToolTip="Edit Room Type" />
                                                                                                   
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Room Classification" Visible ="false">
                                                                                            <EditItemTemplate>
                                                                                                <asp:Label ID="lblrmclasscode" runat="server" Text='<%# Bind("roomclasscode") %>'></asp:Label>
                                                                                            </EditItemTemplate>
                                                                                            <ItemTemplate>
                                                                                                
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Room Classification">
                                                                                            <EditItemTemplate>
                                                                                                <asp:Label ID="lblrmclassname" runat="server" Text='<%# Bind("roomclassname") %>'></asp:Label>
                                                                                                
                                                                                            </EditItemTemplate>
                                                                                            <ItemStyle HorizontalAlign="Left" />
                                                                                            <HeaderStyle HorizontalAlign="Left" Width="60px" />
                                                                                            <ItemTemplate>
                                                                                               <asp:TextBox ID="txtrmclassname" runat="server"   class="cls_txtrmclassname"
                                                                                                    CssClass="field_input " Text='<%# Bind("roomclassname") %>' Width="150px" ></asp:TextBox>
                                                                                                <asp:TextBox ID="txtclasscode" runat="server" Text='<%# Bind("roomclasscode") %>' style="display:none" ></asp:TextBox>
                                                                                                <asp:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server" CompletionInterval="10"
                                                                                                    CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                                                                    CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="1" DelimiterCharacters=""
                                                                                                    EnableCaching="false" Enabled="True" FirstRowSelected="false" MinimumPrefixLength="0"
                                                                                                    OnClientItemSelected="seasonautocompleteselected" ServiceMethod="Getroomclasslist"
                                                                                                    TargetControlID="txtrmclassname">
                                                                                                </asp:AutoCompleteExtender>
                                                                                                <asp:TextBox ID="txtrmclasscode" runat="server" Text='<%# Bind("roomclasscode") %>'  style="display:none"></asp:TextBox>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Unit Yes/No" Visible="true">
                                                                                            <ItemStyle HorizontalAlign="Center" />
                                                                                            <ItemTemplate>
                                                                                                <asp:CheckBox ID="chkunit" runat="server"  />
                                                                                                <asp:Label ID="lblunit" runat="server" Style="display: none" Text='<%# Bind("unityesno") %>'></asp:Label>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Price Adult Occupancy only for Unit" Visible="True">
                                                                                            <EditItemTemplate>
                                                                                                <asp:Label ID="lblpricepax" runat="server" Text='<%# Bind("pricepax") %>'></asp:Label>
                                                                                            </EditItemTemplate>
                                                                                            <ItemStyle HorizontalAlign="Left" />
                                                                                            <HeaderStyle HorizontalAlign="left" Width="80px" />
                                                                                            <ItemTemplate>
                                                                                               <asp:TextBox ID="txtpricepax" runat="server" CssClass="field_input" Style="text-align: center"
                                                                                                    Text='<%# Bind("pricepax") %>' Width="50px" ></asp:TextBox>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Max Adults">
                                                                                            <EditItemTemplate>
                                                                                                <asp:Label ID="lblMaxAdult" runat="server" Text='<%# Bind("maxadults") %>'></asp:Label>
                                                                                            </EditItemTemplate>
                                                                                            <ItemStyle HorizontalAlign="Left" />
                                                                                            <HeaderStyle HorizontalAlign="Left" Width="60px" />
                                                                                            <ItemTemplate>
                                                                                                <asp:TextBox ID="txtadult" runat="server" CssClass="field_input" Style="text-align: center"
                                                                                                    Text='<%# Bind("maxadults") %>' Width="50px"></asp:TextBox>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Max Child">
                                                                                            <EditItemTemplate>
                                                                                                <asp:Label ID="lblMaxChild" runat="server" Text='<%# Bind("maxchilds") %>'></asp:Label>
                                                                                            </EditItemTemplate>
                                                                                            <ItemStyle HorizontalAlign="Left" />
                                                                                            <HeaderStyle HorizontalAlign="Left" Width="60px" />
                                                                                            <ItemTemplate>
                                                                                               <asp:TextBox ID="txtchild" runat="server" CssClass="field_input" Style="text-align: center"
                                                                                                    Text='<%# Bind("maxchilds") %>' Width="50px"></asp:TextBox>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Max Infant">
                                                                                            <EditItemTemplate>
                                                                                                <asp:Label ID="lblMaxInfant" runat="server" Text='<%# Bind("maxinfant") %>'></asp:Label>
                                                                                            </EditItemTemplate>
                                                                                            <ItemStyle HorizontalAlign="Left" />
                                                                                            <HeaderStyle HorizontalAlign="Left" Width="60px" />
                                                                                            <ItemTemplate>
                                                                                               <asp:TextBox ID="txtinfant" runat="server" CssClass="field_input" Style="text-align: center"
                                                                                                    Text='<%# Bind("maxinfant") %>' Width="50px"></asp:TextBox>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Max EB">
                                                                                            <EditItemTemplate>
                                                                                               <asp:Label ID="lblMaxEB" runat="server" Text='<%# Bind("maxeb") %>'></asp:Label>
                                                                                            </EditItemTemplate>
                                                                                            <ItemStyle HorizontalAlign="Left" />
                                                                                            <HeaderStyle HorizontalAlign="Left" Width="60px" />
                                                                                            <ItemTemplate>
                                                                                                <asp:TextBox ID="txtMaxEB" runat="server" CssClass="field_input" Style="text-align: center"
                                                                                                    Text='<%# Bind("maxeb") %>' Width="50px"></asp:TextBox>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="No of Extra Person Supplement for Unit Only">
                                                                                            <EditItemTemplate>
                                                                                               <asp:Label ID="lblexsuppunit" runat="server" Text='<%# Bind("noofextraperson") %>'></asp:Label>
                                                                                            </EditItemTemplate>
                                                                                            <ItemStyle HorizontalAlign="Left" />
                                                                                            <HeaderStyle HorizontalAlign="Left" Width="60px" />
                                                                                            <ItemTemplate>
                                                                                                <asp:TextBox ID="txtExsuppunit" runat="server" CssClass="field_input" Style="text-align: center"
                                                                                                    Text='<%# Bind("noofextraperson") %>' Width="50px"></asp:TextBox>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Max Total Occupancy without infant">
                                                                                            <EditItemTemplate>
                                                                                                <asp:Label ID="lblMaxocctotal" runat="server" Text='<%# Bind("maxoccupancy") %>'></asp:Label>
                                                                                            </EditItemTemplate>
                                                                                            <ItemStyle HorizontalAlign="Left" />
                                                                                            <HeaderStyle HorizontalAlign="Left" Width="60px" />
                                                                                            <ItemTemplate>
                                                                                                <asp:TextBox ID="txtMaxocctotal" runat="server" CssClass="field_input" Style="text-align: center"
                                                                                                    Text='<%# Bind("maxoccupancy") %>' Width="50px"></asp:TextBox>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Start with 0 based " Visible="true">
                                                                                            <ItemTemplate>
                                                                                                <asp:CheckBox ID="chkstart0based" runat="server" />
                                                                                                <asp:Label ID="lblstart0based" runat="server" Style="display: none" Text='<%# Bind("start0based") %>'></asp:Label>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Occupancy Combinations">
                                                                                            <EditItemTemplate>
                                                                                                <asp:Label ID="lblMaxocccombination" runat="server" Text='<%# Bind("combinations") %>'></asp:Label>
                                                                                            </EditItemTemplate>
                                                                                            <ItemStyle HorizontalAlign="Left" />
                                                                                            <HeaderStyle HorizontalAlign="Left" />
                                                                                            <ItemTemplate>
                                                                                                <asp:TextBox ID="txtMaxocccombination" runat="server" CssClass="field_input" MaxLength="500"
                                                                                                    Text='<%# Bind("combinations") %>' Width="150px"></asp:TextBox>
                                                                                                <asp:Button ID="btncombination" runat="server" CssClass="btn" OnClick="btncombination_Click"
                                                                                                    Text=".." Width="20px" />
                                                                                                <asp:HiddenField ID="hdncombination" runat="server"></asp:HiddenField>
                                                                                                <asp:TextBox ID="txtcombinationcat" runat="server" CssClass="displaynone" 
                                                                                                    Text='<%# Bind("combinationscat") %>' Width="150px"></asp:TextBox>
                                                                                                 <%--    <asp:TextBox ID="txtage1" runat="server" CssClass="displaynone" 
                                                                                                    Text='<%# Bind("childage1") %>' Width="25px"></asp:TextBox>
                                                                                                     <asp:TextBox ID="txtage2" runat="server" CssClass="displaynone" 
                                                                                                    Text='<%# Bind("childage2") %>' Width="25px"></asp:TextBox>
                                                                                                     <asp:TextBox ID="txtage3" runat="server" CssClass="displaynone" 
                                                                                                    Text='<%# Bind("childage3") %>' Width="25px"></asp:TextBox>
                                                                                                     <asp:TextBox ID="txtage4" runat="server" CssClass="displaynone" 
                                                                                                    Text='<%# Bind("childage4") %>' Width="25px"></asp:TextBox>
                                                                                                     <asp:TextBox ID="txtage5" runat="server" CssClass="displaynone" 
                                                                                                    Text='<%# Bind("childage5") %>' Width="25px"></asp:TextBox>
                                                                                                     <asp:TextBox ID="txtage6" runat="server" CssClass="displaynone" 
                                                                                                    Text='<%# Bind("childage6") %>' Width="25px"></asp:TextBox>
                                                                                                     <asp:TextBox ID="txtage7" runat="server" CssClass="displaynone" 
                                                                                                    Text='<%# Bind("childage7") %>' Width="25px"></asp:TextBox>
                                                                                                     <asp:TextBox ID="txtage8" runat="server" CssClass="displaynone" 
                                                                                                    Text='<%# Bind("childage8") %>' Width="25px"></asp:TextBox>
                                                                                                     <asp:TextBox ID="txtage9" runat="server" CssClass="displaynone" 
                                                                                                    Text='<%# Bind("childage9") %>' Width="25px"></asp:TextBox>--%>
                                                                                                     <asp:HiddenField ID="hdnchildcomb" runat="server"></asp:HiddenField>

                                                                                                <asp:Button ID="btngAlert" runat="server" Style="display: none" Text="Fill" />
                                                                                            </ItemTemplate>
                                                                                            <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                                                                                            <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Rank Order">
                                                                                            <EditItemTemplate>
                                                                                                <asp:Label ID="lblrankorder" runat="server" Text='<%# Bind("rankord") %>'></asp:Label>
                                                                                            </EditItemTemplate>
                                                                                            <ItemStyle HorizontalAlign="Left" />
                                                                                            <HeaderStyle HorizontalAlign="Left" Width="60px" />
                                                                                            <ItemTemplate>
                                                                                                <asp:TextBox ID="txtrankorder" runat="server" CssClass="field_input" Style="text-align: center"
                                                                                                    Text='<%# Bind("rankord") %>' Width="50px"></asp:TextBox>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="InActive " Visible="true">
                                                                                            <ItemStyle HorizontalAlign="Center" />
                                                                                            <ItemTemplate>
                                                                                               <asp:CheckBox ID="chkactive" runat="server" onclick="setrowcolor(this)" />
                                                                                                &nbsp;
                                                                                                <asp:TextBox ID="txtactive" runat="server" Style="display: none" Text='<%# Bind("inactive") %>'></asp:TextBox>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Room details for online"></asp:TemplateField>
                                                                                        
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
                                                                            <td colspan="10">
                                                                            <table>
                                                                            <tr>
                                                                            <td><asp:Button ID="btnAddrow" runat="server" CssClass="btn" TabIndex="10" Text="Add Row" /></td>
                                                                            <td> <asp:Button ID="btndeleterow" runat="server" CssClass="btn" TabIndex="11" Text="Delete Row" /></td>
                                                                            <td><asp:Button ID="btnclear" runat="server" CssClass="btn" TabIndex="12" Text="Clear Row" /></td>
                                                                            <td><asp:Button ID="btnaddroomclass" runat="server" CssClass="btn" TabIndex="13" Text="Add New Room Classification" /></td>
                                                                            </tr>
                                                                            </table>
                                                                              
                                                                                
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>
                                                                                <asp:HiddenField ID="hdCurrentDate" runat="server" />
                                                                                <asp:HiddenField ID="hdSeasonName" runat="server" />
                                                                                <asp:Button ID="btnDummy" runat="server" Style="display: none" Text="" />
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
                                                                    <asp:ModalPopupExtender ID="ModalPopupOccupancy" runat="server" BackgroundCssClass="ModalPopupBG"
                                                                        BehaviorID="ModalPopupOccupancy" CancelControlID="btnOccupancyCancel" Drag="true"
                                                                        OkControlID="btnOkay" PopupControlID="PanelOccupancy" PopupDragHandleControlID="PopupHeader"
                                                                        TargetControlID="btnInvisibleOccupancy">
                                                                    </asp:ModalPopupExtender>
                                                                    <asp:Panel ID="PanelOccupancy" runat="server" BorderStyle="Double" BorderWidth="6px"
                                                                        Style="display: none">
                                                                        <div class="HellowWorldPopup" style="font-family: Arial, Helvetica, sans-serif">
                                                                            <div id="Div2" class="PopupHeader">
                                                                                <center style="background-color: #CCCCCC;">
                                                                                    Occupancy
                                                                                </center>
                                                                            </div>
                                                                            <div class="PopupBody">
                                                                                <center>
                                                                                    <br />
                                                                                    <p>
                                                                                        Room Type :
                                                                                        <asp:Label ID="lblRoomTypeText" runat="server" CssClass="td_cell " Text="[lblRoomTypeText]"></asp:Label>
                                                                                        &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; Room Classfication :
                                                                                        <asp:Label ID="lblRoomCategoryText" runat="server" CssClass="td_cell " Text="[lblRoomCategoryText]"></asp:Label>
                                                                                    </p>
                                                                                    <asp:Panel ID="PanelO" runat="server" BorderStyle="Solid" BorderWidth="1px" Height="300px"
                                                                                        ScrollBars="Vertical" Width="550px">
                                                                                        <asp:GridView ID="gvOccupancy" runat="server" AutoGenerateColumns="False" CellPadding="4"
                                                                                          BorderStyle ="Solid"   Font-Names="Verdana" Font-Size="12px" ForeColor="#333333" GridLines="None">
                                                                                            <AlternatingRowStyle BackColor="White" />
                                                                                            <FooterStyle CssClass="grdfooter" />
                                                                                            <Columns>
                                                                                                <asp:TemplateField>
                                                                                                    <ItemTemplate>
                                                                                                        <input id="chk" type="checkbox" runat="server" />
                                                                                                    </ItemTemplate>
                                                                                                    <HeaderStyle Width="30px" />
                                                                                                </asp:TemplateField>
                                                                                                <asp:BoundField ControlStyle-Width="30px" DataField="adult" HeaderStyle-Width="30px"
                                                                                                    HeaderText="Adult" />
                                                                                                <asp:BoundField ControlStyle-Width="30px" DataField="child" HeaderStyle-Width="30px"
                                                                                                    HeaderText="Child" />
                                                                                                     <asp:TemplateField HeaderText="Accommodation Category">
                                                                                                        <ItemStyle Width="100px" />
                                                                                                        <HeaderStyle Width="100px" />
                                                                                                        <ItemTemplate>
                                                                                                            <select ID="ddlRmcat" runat="server" class="drpdown" style="WIDTH: 70px">
                                                                                                               
                                                                                                            </select>
                                                                                                        </ItemTemplate>
                                                                                                    </asp:TemplateField>
                                                                                                     <asp:TemplateField HeaderText="Child1 Age" Visible ="false">
                                                                                                        <ItemStyle HorizontalAlign="Left" />
                                                                                                        <HeaderStyle HorizontalAlign="Left" Width="60px" />
                                                                                                        <ItemTemplate>
                                                                                                            <asp:TextBox ID="txtchild1age" runat="server" CssClass="field_input" Style="text-align: center"
                                                                                                                Text='<%# Bind("childage1") %>'  Width="50px"></asp:TextBox>
                                                                                                        </ItemTemplate>
                                                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                                                        <ItemStyle HorizontalAlign="Left" />
                                                                                                    </asp:TemplateField>
                                                                                                    <asp:TemplateField HeaderText="Child2 Age" Visible ="false">
                                                                                                        <ItemStyle HorizontalAlign="Left" />
                                                                                                        <HeaderStyle HorizontalAlign="Left" Width="60px" />
                                                                                                        <ItemTemplate>
                                                                                                            <asp:TextBox ID="txtchild2age" runat="server" CssClass="field_input" Style="text-align: center"
                                                                                                                Text='<%# Bind("childage2") %>'  Width="50px"></asp:TextBox>
                                                                                                        </ItemTemplate>
                                                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                                                        <ItemStyle HorizontalAlign="Left" />
                                                                                                    </asp:TemplateField>
                                                                                                    <asp:TemplateField HeaderText="Child3 Age" Visible ="false">
                                                                                                        <ItemStyle HorizontalAlign="Left" />
                                                                                                        <HeaderStyle HorizontalAlign="Left" Width="60px" />
                                                                                                        <ItemTemplate>
                                                                                                            <asp:TextBox ID="txtchild3age" runat="server" CssClass="field_input" Style="text-align: center"
                                                                                                              Text='<%# Bind("childage3") %>'    Width="50px"></asp:TextBox>
                                                                                                        </ItemTemplate>
                                                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                                                        <ItemStyle HorizontalAlign="Left" />
                                                                                                    </asp:TemplateField>
                                                                                                    <asp:TemplateField HeaderText="Child4 Age" Visible ="false">
                                                                                                        <ItemStyle HorizontalAlign="Left" />
                                                                                                        <HeaderStyle HorizontalAlign="Left" Width="60px" />
                                                                                                        <ItemTemplate>
                                                                                                            <asp:TextBox ID="txtchild4age" runat="server" CssClass="field_input" Style="text-align: center"
                                                                                                                Text='<%# Bind("childage4") %>'  Width="50px"></asp:TextBox>
                                                                                                        </ItemTemplate>
                                                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                                                        <ItemStyle HorizontalAlign="Left" />
                                                                                                    </asp:TemplateField>
                                                                                                    <asp:TemplateField HeaderText="Child5 Age" Visible ="false">
                                                                                                        <ItemStyle HorizontalAlign="Left" />
                                                                                                        <HeaderStyle HorizontalAlign="Left" Width="60px" />
                                                                                                        <ItemTemplate>
                                                                                                            <asp:TextBox ID="txtchild5age" runat="server" CssClass="field_input" Style="text-align: center"
                                                                                                                 Text='<%# Bind("childage5") %>' Width="50px"></asp:TextBox>
                                                                                                        </ItemTemplate>
                                                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                                                        <ItemStyle HorizontalAlign="Left" />
                                                                                                    </asp:TemplateField>
                                                                                                    <asp:TemplateField HeaderText="Child6 Age" Visible ="false">
                                                                                                        <ItemStyle HorizontalAlign="Left" />
                                                                                                        <HeaderStyle HorizontalAlign="Left" Width="60px" />
                                                                                                        <ItemTemplate>
                                                                                                            <asp:TextBox ID="txtchild6age" runat="server" CssClass="field_input" Style="text-align: center"
                                                                                                                 Text='<%# Bind("childage6") %>' Width="50px"></asp:TextBox>
                                                                                                        </ItemTemplate>
                                                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                                                        <ItemStyle HorizontalAlign="Left" />
                                                                                                    </asp:TemplateField>
                                                                                                    <asp:TemplateField HeaderText="Child7 Age" Visible ="false">
                                                                                                        <ItemStyle HorizontalAlign="Left" />
                                                                                                        <HeaderStyle HorizontalAlign="Left" Width="60px" />
                                                                                                        <ItemTemplate>
                                                                                                            <asp:TextBox ID="txtchild7age" runat="server" CssClass="field_input" Style="text-align: center"
                                                                                                                Text='<%# Bind("childage7") %>'  Width="50px"></asp:TextBox>
                                                                                                        </ItemTemplate>
                                                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                                                        <ItemStyle HorizontalAlign="Left" />
                                                                                                    </asp:TemplateField>
                                                                                                    <asp:TemplateField HeaderText="Child8 Age" Visible ="false">
                                                                                                        <ItemStyle HorizontalAlign="Left" />
                                                                                                        <HeaderStyle HorizontalAlign="Left" Width="60px" />
                                                                                                        <ItemTemplate>
                                                                                                            <asp:TextBox ID="txtchild8age" runat="server" CssClass="field_input" Style="text-align: center"
                                                                                                                Text='<%# Bind("childage8") %>'  Width="50px"></asp:TextBox>
                                                                                                        </ItemTemplate>
                                                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                                                        <ItemStyle HorizontalAlign="Left" />
                                                                                                    </asp:TemplateField>
                                                                                                    <asp:TemplateField HeaderText="Child9 Age" Visible ="false">
                                                                                                        <ItemStyle HorizontalAlign="Left" />
                                                                                                        <HeaderStyle HorizontalAlign="Left" Width="60px" />
                                                                                                        <ItemTemplate>
                                                                                                            <asp:TextBox ID="txtchild9age" runat="server" CssClass="field_input" Style="text-align: center"
                                                                                                                Text='<%# Bind("childage9") %>'  Width="50px"></asp:TextBox>
                                                                                                        </ItemTemplate>
                                                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                                                        <ItemStyle HorizontalAlign="Left" />
                                                                                                    </asp:TemplateField>
                                                                                            </Columns>
                                                                                            <FooterStyle BackColor="White" ForeColor="#000066" />
                                                                                            <EditRowStyle BackColor="#7C6F57" />
                                                                                            <FooterStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
                                                                                            <HeaderStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
                                                                                            <PagerStyle BackColor="#666666" ForeColor="White" HorizontalAlign="Center" />
                                                                                            <RowStyle BackColor="#E3EAEB" />
                                                                                            <SelectedRowStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
                                                                                            <SortedAscendingCellStyle BackColor="#F8FAFA" />
                                                                                            <SortedAscendingHeaderStyle BackColor="#246B61" />
                                                                                            <SortedDescendingCellStyle BackColor="#D4DFE1" />
                                                                                            <SortedDescendingHeaderStyle BackColor="#15524A" />
                                                                                        </asp:GridView>
                                                                                    </asp:Panel>
                                                                                    <asp:Button ID="btnSelectAll" runat="server" CssClass="btn" TabIndex="14" Text="Select All" />
                                                                                    &nbsp;
                                                                                    <asp:Button ID="btnUnselectAll" runat="server" CssClass="btn" TabIndex="15" Text="Unselect All" />
                                                                                    <br />
                                                                                    <br />
                                                                                    <asp:Button ID="btnDeleteRowcomb" runat="server" CssClass="btn" TabIndex="16" Text="Delete Row" />
                                                                                    <asp:HiddenField ID="hdnMainGridRowid" runat="server" />
                                                                                    <br />
                                                                                    <br />
                                                                                    <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                                                                    </asp:UpdatePanel>
                                                                                    &nbsp;&nbsp;<asp:Button ID="btnOccupancyOK" runat="server" CssClass="field_button"
                                                                                        Text="OK" Width="70px" />
                                                                                    &nbsp;
                                                                                    <asp:Button ID="btnOccupancyCancel" runat="server" CssClass="field_button" Text="Cancel"
                                                                                        Width="70px" />
                                                                                    <p>
                                                                                    </p>
                                                                                    </p>
                                                                                </center>
                                                                                <input id="btnInvisibleOccupancy" runat="server" type="button" value="Cancel" style="visibility: hidden" />
                                                                                <input id="Button5" type="button" value="Cancel" style="visibility: hidden" />
                                                                                <input id="Button6" runat="server" type="button" value="Cancel" style="visibility: hidden" />
                                                                                <input id="btnOkay" type="button" value="OK" style="visibility: hidden" />
                                                                                <input id="Button2" type="button" value="Cancel" style="visibility: hidden" />
                                                                                <input style="visibility: hidden; width: 12px; height: 9px" id="Text1" type="text"
                                                                                    runat="server" />
                                                                            </div>
                                                                        </div>
                                                                    </asp:Panel>
                                                                    <%--*** Danny Image Popup>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>--%>
                                                                    <asp:TextBox ID="hdnFileName" Text="" runat="server" Style="display: none" />
                                                                    <asp:HiddenField ID="SelectedRoomImgPath" runat="server" />
                                                                    <asp:ModalPopupExtender ID="ModalPopupExtender2" runat="server" BackgroundCssClass="ModalPopupBG"
                                                                        BehaviorID="ModalPopupExtender2" CancelControlID="Button1" Drag="true"
                                                                        OkControlID="Btn_IMGOK" PopupControlID="PanelImage" PopupDragHandleControlID="IMGPopupHeader"
                                                                        TargetControlID="BtnSaveInfoWeb">
                                                                    </asp:ModalPopupExtender>
                                                                    <asp:Panel ID="PanelImage" runat="server" BorderStyle="Double" BorderWidth="6px"
                                                                        Style="display: none">
                                                                        <div class="HellowWorldPopup" style="font-family: Arial, Helvetica, sans-serif">
                                                                            <div id="Div4" class="IMGPopupHeader">
                                                                                <center style="background-color: #CCCCCC;">
                                                                                    Room Image Uploade
                                                                                </center>
                                                                            </div>
                                                                            <div class="PopupBody">
                                                                                <center>                                                                                    
                                                                                    <p>
                                                                                        Room Type :
                                                                                        <asp:Label ID="lblRoomCategoryTextIMG" runat="server" CssClass="td_cell " Text="[lblRoomCategoryTextIMG]"></asp:Label>
                                                                                        &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; Room Classfication :
                                                                                        <asp:Label ID="lblRoomTypeTextIMG" runat="server" CssClass="td_cell " Text="[lblRoomTypeTextIMG]"></asp:Label>
                                                                                    </p>
                                                                                    <asp:Panel ID="PanelInfoForWEb1" runat="server" Width="809px" Height="400px" GroupingText="Select / Change Image">
                                                                                        <table width="100%">
                                                                                            <tbody>
                                                                                                <tr align="right" valign="top">
                                                                                                    <td width="50%" valign="top">
                                                                                                        Select Image  
                                                                                                        <asp:HiddenField ID="RoomImgRow" runat="server" />
                                                                                                        <asp:Label ID="lblimage" runat="server" ForeColor="Blue" Width="220px"  style="display:none"></asp:Label>
                                                                                                    </td>
                                                                                                    <td valign="top" align="left">
                                                                                                        
                                                                                                        <table>
                                                                                                            <tr>
                                                                                                                <td width="25%">
                                                                                                                    <cc1:AsyncFileUpload OnClientUploadComplete="uploadComplete" runat="server" ID="AsyncFileUpload1" CssClass="FileUploadClass"
                                                                                                            Width="100%" UploaderStyle="Modern" CompleteBackColor="White" UploadingBackColor="#CCFFFF"
                                                                                                            ThrobberID="imgLoader" OnUploadedComplete="FileUploadComplete" OnClientUploadStarted="uploadStarted"   />
                                                                                                                </td>
                                                                                                                <td width="25%">
                                                                                                                     <asp:Image ID="imgLoader" runat="server" ImageUrl="~/images/loader.gif" /><br />
                                                                                                                </td>
                                                                                                                <td width="25%">
                                                                                                                    <img id = "imgDisplay" alt="" src="" style = "width:50px; height:50px; display:none"/>
                                                                                                                </td>
                                                                                                                <td width="25%"></td>
                                                                                                            </tr>
                                                                                                        </table>
                                                                                                        
                                                                                                                                                                                                      </td>
                                                                                                </tr>
                                                                                               

                                                                                                <tr align="center">
                                                                                                    <td colspan="2" style="height: 31px">
                                                                                                     
                                                                                                    <table>
                                                                                                        <tr>
                                                                                                            <td width="25%"><asp:Button ID="Btn_Save" runat="server" CssClass="field_button"  Text="OK" Width="70px" /></td>
                                                                                                            <td width="25%"><asp:Button ID="btnViewimage" runat="server" CssClass="field_button" Text="View"   Width="64px" /></td>
                                                                                                            <td width="25%"><asp:Button ID="Btnrmv7" runat="server" CssClass="field_button" Text="Remove" Visible=false   Width="77px" /></td>
                                                                                                            <td width="25%"><asp:Button ID="Btn_IMGCance" runat="server" CssClass="field_button" Text="Cancel" Width="70px" /></td>
                                                                                                            
                                                                                                        </tr>
                                                                                                    </table>
                                                                                                       
                                                                                        
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr align="center">
                                                                                                    <td colspan="2" id="lblimage1" text="fdsfasdF" >
                                                                                                        <asp:ImageButton ID="ImgRoomImage" runat="server" width="299px" Height="299px" style="cursor:default; " /> 
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td>
                                                                                                    </td>
                                                                                                    <td>
                                                                                                    </td>
                                                                                                </tr>
                                                                                            </tbody>
                                                                                        </table>
                                                                                    </asp:Panel>
                                                                                    <%--<asp:UpdatePanel runat="server" ID="upanel1" BorderStyle="Solid" BorderWidth="1px" Height="300px"
                                                                                        ScrollBars="Vertical" Width="450px">
                                                                                        <ContentTemplate>
                                                                                            <ajaxtoolkit:ajaxfileupload id="ajaxUpload1" runat="server" onuploadcomplete="ajaxUpload1_UploadComplete" />
                                                                                            <asp:Image ID="imgAjax" runat="server" />
                                                                                        </ContentTemplate>
                                                                                    </asp:UpdatePanel>--%>
                                                                                    <asp:HiddenField ID="HiddenField1" runat="server" />     

                                                                                </center>
                                                                                 
                                                                               
                                                                               <asp:Button ID="BtnSaveInfoWeb" runat="server" CssClass="field_button" TabIndex="113" Text="OK" Width="66px" style="visibility: hidden"/>
                                                                                <input id="Btn_IMGOK" type="button" value="OK" style="visibility: hidden" />
                                                                                <asp:Button ID="Button1" runat="server" CssClass="field_button" Text="Cancel"
                                                                                        Width="70px" style="visibility: hidden"/>
                                                                                
                                                                            </div>
                                                                        </div>
                                                                    </asp:Panel>
                                                                    <%--*** Danny Image Popup<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<--%>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </td>
                                                    </tr>
                                                     <tr>
                                                        <td width="120px">
                                                            <asp:Button ID="btnSave" runat="server" CssClass="field_button" TabIndex="4" Text="Save11"
                                                                Width="93px" />
                                                        </td>
                                                        <td>
                                                            &nbsp;
                                                            <asp:Button ID="btnreset1" runat="server" CssClass="field_button" TabIndex="5" Text="Return To Search"
                                                                Width="139px" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
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
                        <td style="width: 100px" valign="top">
                        </td>
                        <td valign="top">
                            &nbsp;<asp:Button ID="btnhelp" runat="server" CssClass="field_button" TabIndex="17"
                                Text="Help" Visible="false" />
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
                                         <asp:HiddenField ID="hdncontractid" runat="server" />
                                        <asp:HiddenField ID="hdnconfromdate" runat="server" />
                                        <asp:HiddenField ID="hdncontodate" runat="server" />
                                        <asp:HiddenField ID="hdncopypromotionid" runat="server" />
                                         <asp:HiddenField ID="hdnpromotionid" runat="server" />
                                          <asp:HiddenField ID="hdncommtype" runat="server" />
                                           <asp:HiddenField ID="hdnpromofrmdate" runat="server" />
                                        <asp:HiddenField ID="hdnpromotodate" runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </tbody>
            </table>
              <div id="Copycontract" runat="server" style="overflow: scroll; height: 500px; width: 600px;
                border: 3px solid green; background-color: White; display: none">
                <table>
                    <tr>
                      <td valign="top" align="center" colspan="2">
                            <asp:Label ID="Label2" runat="server" Text="Copy From Another Max Occupancy" CssClass="field_heading"
                                Width="600px" ForeColor="White"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                       <td colspan="2">
                            <asp:GridView ID="grdviewrates" runat="server" AutoGenerateColumns="False" BackColor="White"
                                BorderColor="#999999" CssClass="td_cell" Width="550px">
                                <Columns>
                                    <asp:TemplateField Visible="False" HeaderText="Supplier Code">
                                        <EditItemTemplate>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblplistcode" runat="server" Text='<%# Bind("plistcode") %>' __designer:wfdid="w1"></asp:Label>
                                            <asp:Label ID="lblcontract" runat="server" Text='<%# Bind("contractid") %>' __designer:wfdid="w1"></asp:Label>
                                            <asp:Label ID="lblpromotionid" runat="server" Text='<%# Bind("promotionid") %>' __designer:wfdid="w1"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:ButtonField HeaderText="" Text="Select" CommandName="Select">
                                        <ItemStyle ForeColor="Blue"></ItemStyle>
                                    </asp:ButtonField>
                                    <asp:BoundField DataField="Contractid" SortExpression="Contractid" HeaderText="Contract ID">
                                    </asp:BoundField>
                                    <asp:BoundField DataField="plistcode" SortExpression="plistcode" HeaderText="Tran ID">
                                    </asp:BoundField>
                                    
                                     <asp:BoundField DataField="promotionid" SortExpression="promotionid" HeaderText="Promotion ID">
                                    </asp:BoundField>

                                    <asp:BoundField DataField="promotionname" SortExpression="promotionname" HeaderText="Promotion Name">
                                    </asp:BoundField>
                                  
                                    <asp:BoundField DataField="fromdate" SortExpression="fromdate" HeaderText="From Date">
                                    </asp:BoundField>
                                    <asp:BoundField DataField="todate" SortExpression="todate" HeaderText="To Date">
                                    </asp:BoundField>
                              
                                  
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
                        <td align="center">
                            <asp:Button ID="btnclose" runat="server" CssClass="field_button" Text="Close" Width="75px" />
                        </td>
                    </tr>
                </table>
                <input id="btnokviewrates" runat="server" type="button" value="OK" style="display:none" />
                <input id="btncloseviewrates" runat="server" type="button" value="Cancel" style="display:none" />
                    <input id="btnviewchild" runat="server" type="button" value="Cancel" style="display:none" />
                                                           

                                                             
            </div>
             <cc1:ModalPopupExtender ID="ModalViewrates" runat="server" BehaviorID="ModalViewrates"
                    CancelControlID="btncloseviewrates" OkControlID="btnokviewrates" TargetControlID="btnviewchild"
                    PopupControlID="Copycontract" PopupDragHandleControlID="PopupHeader" Drag="true"
                    BackgroundCssClass="ModalPopupBG">
                </cc1:ModalPopupExtender>

             <table>
                <tr>
                    <td colspan="2">
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>
                                <div id="Div14" runat="server" style="overflow: scroll; height: 400px; width: 750px;
                                    border: 3px solid green; background-color: White; display: none">
                                    <table>
                                        <tr>
                                            <td colspan="2">
                                                <asp:GridView ID="grdpromotion" runat="server" AutoGenerateColumns="False" BackColor="White"
                                                    BorderColor="#999999" CssClass="td_cell" Width="720px">
                                                    <Columns>
                                                         <asp:ButtonField HeaderText="" Text="Select" CommandName="Select">
                                                            <ItemStyle ForeColor="Blue"></ItemStyle>
                                                        </asp:ButtonField>

                                                           <asp:TemplateField HeaderText="Tran ID" >
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                            <ItemTemplate>
                                                                <asp:Label ID="lbltranid" runat="server" Text='<%# Bind("tranid") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>


                                                        <asp:TemplateField HeaderText="Promotion ID" >
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblpromotionid" runat="server" Text='<%# Bind("promotionid") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                         <asp:TemplateField HeaderText="Promotion Name" >
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblpromotionname" runat="server" Text='<%# Bind("promotionname") %>'></asp:Label>
                                                               
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                         <asp:TemplateField HeaderText="Applicable" Visible ="false"  >
                                                          <ItemTemplate>
                                                             <asp:Label ID="lblapplicableto" runat="server"   Text='<%# Bind("applicableto") %>'    ></asp:Label>
                                                            </ItemTemplate>
                                                         </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Applicable To">
                                                                <ItemTemplate>
                                                           
                                                            <asp:Label ID="lblapplicable" runat="server"    Text='<%# Limit(Eval("applicableto"), 10)%>' Tooltip='<%# Eval("applicableto")%>'  ></asp:Label>
                                                            <br />
                                                                <asp:LinkButton ID="ReadMoreLinkButtonpromotion" runat="server" CommandName ="moreless" Text="More" Visible='<%# SetVisibility(Eval("applicableto"), 5) %>'  OnClick="ReadMoreLinkButtonpromotion_Click"></asp:LinkButton>

                                                            </ItemTemplate>
                                                         
                                                        </asp:TemplateField>
                                                           <asp:BoundField DataField="fromdate" SortExpression="fromdate" HeaderText="MinFrom Date">
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="todate" SortExpression="todate" HeaderText="MaxTo Date">
                                                        </asp:BoundField>
                                                          <asp:BoundField DataField="status" SortExpression="status" HeaderText="Status">
                                                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Top"></ItemStyle>
                                                            <HeaderStyle BackColor="#06788B" HorizontalAlign="Left" Width="50px"></HeaderStyle>
                                                        </asp:BoundField>
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
                                            <td align="center">
                                              
                                                <asp:Button ID="btncpromolose" runat="server" CssClass="field_button" Text="Close" Width="75px" />
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
</contenttemplate>
    </asp:UpdatePanel>

  

    <asp:ScriptManagerProxy id="ScriptManagerProxy1" runat="server">
        <services>
<asp:ServiceReference Path="~/clsServices.asmx"></asp:ServiceReference>
</services>
    </asp:ScriptManagerProxy>

</asp:Content>

