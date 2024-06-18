<%@ Page Language="VB"  MasterPageFile="~/ExcursionMaster.master" AutoEventWireup="false" CodeFile="ExcSectorGroups.aspx.vb" Inherits="ExcSectorGroups" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
    <%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
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

  <script type="text/javascript" charset="utf-8">
      $(document).ready(function () {
          visualsearchbox();
          txtNameAutoCompleteExtenderKeyUp();
          cityAutoCompleteExtenderKeyUp();
      
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
            var $txtvsprocess = $(document).find('.cs_txtvsprocess');
          $txtvsprocess.val('Country:" " City:" " Sector:" " SectorGroup:"  " Text:" "');
          if (ValueRetrieval.value == "S") {
              $txtvsprocess.val('Country:" " City:" " Sector:" " SectorGroup:"  " Text:" "');
          }
          if (ValueRetrieval.value !== "S") {
              $txtvsprocess.val('Sector:" " Text:" "');
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
                      if (ValueRetrieval.value == "S") {
                          switch (category) {
                              case 'Country':
                                  var asSqlqry = '';
                                  glcallback = callback;
                                  //fnTestCallback();
                                  ColServices.clsHotelGroupServices.GetListOfArrayCountryVisualSearch(asSqlqry, fnFillSearchVS, ErrorHandler, TimeOutHandler);
                                  break;
                              case 'City':
                                  var asSqlqry = '';
                                  glcallback = callback;
                                  //fnTestCallback();
                                  ColServices.clsHotelGroupServices.GetListOfArrayCityVisualSearch(asSqlqry, fnFillSearchVS, ErrorHandler, TimeOutHandler);
                                  break;
                              case 'Sector':
                                  var asSqlqry = '';
                                  glcallback = callback;
                                  //fnTestCallback();
                                  ColServices.clsHotelGroupServices.GetListOfArraySectorsVisualSearch(asSqlqry, fnFillSearchVS, ErrorHandler, TimeOutHandler);
                                  break;
                              case 'SectorGroup':
                                  var asSqlqry = '';
                                  glcallback = callback;
                                  ColServices.clsHotelGroupServices.GetListOfSectorGrpsNameVisualSearch(asSqlqry, fnFillSearchVS, ErrorHandler, TimeOutHandler);
                                  break;
                          }
                      }
                      if (ValueRetrieval.value !="S") {
                          switch (category) {
                              case 'Sector':
                                  var asSqlqry = '';
                                  glcallback = callback;
                                  //fnTestCallback();
                                 
                                  ColServices.clsHotelGroupServices.GetListOfArraySectorsVisualSearch(asSqlqry, fnFillSearchVS, ErrorHandler, TimeOutHandler);
                                  break;
                                                    break; 
                          }
                      }
                  },
                  facetMatches: function (callback) {
                      callback([
                { label: 'Text' },
                { label: 'Country', category: 'Sector' },
                { label: 'City', category: 'Sector' },
                { label: 'Sector', category: 'Sector' },
                { label: 'SectorGroup', category: 'Sector' },

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


          function cityAutoCompleteExtenderKeyUp() {
              $("#<%=Txtcityname.ClientID %>").bind("change", function () {

                  document.getElementById('<%=Txtcitycode.ClientID%>').value = '';
              });
          }

          function cityautocompleteselected(source, eventArgs) {
              if (eventArgs != null) {
                  document.getElementById('<%=Txtcitycode.ClientID%>').value = eventArgs.get_value();
              }
              else {
                  document.getElementById('<%=Txtcitycode.ClientID%>').value = '';
              }

          }


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
                  document.getElementById('<%=txtGroupCode.ClientID%>').value = '';

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
            txtNameAutoCompleteExtenderKeyUp();
            visualsearchbox();

        }
        function FormValidationMainDetail(state) {
            var btn = document.getElementById("<%=btnSave.ClientID%>");
            if (btn.value == 'Save') { if (confirm('Are you sure you want to save?') == false) return false; }
            if (btn.value == 'Update') { if (confirm('Are you sure you want to update?') == false) return false; }
            if (btn.value == 'Delete') { if (confirm('Are you sure you want to delete?') == false) return false; }
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

    .style13
    {
        width: 268px;
    }
    .style14
    {
        width: 162px;
    }

    .style16
    {
        width: 196px;
    }

    .style17
    {
        width: 122px;
    }

    </style>
</head>

    <asp:UpdatePanel id="UpdatePanel1" runat="server" style="vertical-align:top;" >
        <contenttemplate>
<TABLE style="BORDER-RIGHT: gray 2px solid; BORDER-TOP: gray 2px solid; BORDER-LEFT: gray 2px solid; WIDTH: 100%px; BORDER-BOTTOM: gray 2px solid;margin-left:35px;vertical-align:top;"  ><TBODY><TR>
    <TD style="HEIGHT: 15px" class="field_heading" align=center colSpan=4 
        width="10">
    <asp:Label id="lblHeading" runat="server" Text="Add\Edit Country Group" 
        Width="738px" CssClass="" style="height: 14px"></asp:Label></TD></TR><TR>
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
            &nbsp;&nbsp;
               &nbsp;<asp:Button ID="btnPrint" runat="server" CssClass="btnExample" 
                Text="Report" />&nbsp;&nbsp;</td>
    </tr>
               <tr>
    <td style="display:none">
            &nbsp;<asp:Button ID="btnExcel" runat="server" CssClass="btnExample" />
    
       </td>
    </tr>
    <tr>
        <td class="style4" colspan="4">
            </td>
    </tr>
    
    <tr>
    
        <td class="style4" colspan="5" valign="top">
            <table class="style1">
          
         
                <tr>
                    <td class="style17">
                        <asp:HiddenField ID="hdFillByGrid" runat="server" />
                    </td>
                    <td class="style17">
                        &nbsp;</td>
                    <td class="style16"   >
                        <asp:HiddenField ID="hdLinkButtonValue" runat="server" />
                    </td>
                    <td class="style13" >
                        <asp:HiddenField ID="hdOPMode" runat="server" />
                    </td>
                          <td class="style14">
                            <asp:HiddenField ID="hdncitycode" runat="server" /></td> 
                            <td>
                             <asp:HiddenField ID="HiddenField1" runat="server" /></td>    
                </tr>
               <tr runat="server" id="trNameAndCode">
           
                    <td class="style17">
                        &nbsp;</td>
                        <td class="style17">
                            <asp:Label ID="Label11" runat="server" Text="Code"></asp:Label>
                    </td>
                        <td >
                          <asp:TextBox ID="txtGroupCode" runat="server" Width="150px" 
                                style="margin-left: 0px"></asp:TextBox>&nbsp;<asp:CheckBox ID="chkActive" 
                                runat="server" Checked="True" Text="Active" />
                    </td>
                                <td>
                                    &nbsp;</td>
                           <td >
                               &nbsp;</td>
                       <td class="style14">
                     <asp:TextBox ID="Txtcurrencycode" runat="server" style="display:none"  ></asp:TextBox>
                    </td>
              
                               </tr>
                <tr>
                    <td class="style17">
                         &nbsp;</td>
                    <td class="style17">
                        <asp:Label ID="lblsectgrp" runat="server" Text="Sector Group"></asp:Label>
                    </td>
                    <td class="style16">
                             
                          <asp:TextBox   ID="TxtCityCode" runat="server"  style="display:none" ></asp:TextBox>
                          <asp:TextBox ID="txtGroupTagName" runat="server" AutoPostBack="True" 
                              Width="300px"></asp:TextBox>
                          <asp:AutoCompleteExtender ID="txtGroupTagName_AutoCompleteExtender" 
                              runat="server" CompletionInterval="10" 
                              CompletionListCssClass="autocomplete_completionListElement" 
                              CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem" 
                              CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="1" 
                              DelimiterCharacters="" EnableCaching="false" Enabled="True" 
                              FirstRowSelected="false" MinimumPrefixLength="0" 
                              OnClientItemSelected="txtNameautocompleteselected" 
                              ServiceMethod="GetSectorGroups" TargetControlID="txtGroupTagName">
                          </asp:AutoCompleteExtender>
                         </td>
                         <td class="style13">
                             &nbsp;</td>
                    <td class="style14">
                      <input style="display:none" id="Text7" class="field_input" type="text"
                           runat="server" />
                      
                      </td>
                      <td>
                        <input style="display:none" id="Text8" class="field_input" type="text"
                             runat="server" />
                       </td>   
                
                </tr>
        
                
                <tr>
                    <td class="style17">
                        &nbsp;</td>
                    <td class="style17">
                        <asp:Label ID="lblcity0" runat="server" Text="City"></asp:Label>
                    </td>
                    <td class="style16">
                        <asp:TextBox ID="TxtCityName" runat="server" AutoPostBack="True" 
                            CssClass="field_input" MaxLength="500" TabIndex="7" Width="301px" 
                            Height="17px"></asp:TextBox>
                        <asp:AutoCompleteExtender ID="TxtCityName_AutoCompleteExtender" runat="server" 
                            CompletionInterval="10" 
                            CompletionListCssClass="autocomplete_completionListElement" 
                            CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem" 
                            CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="1" 
                            DelimiterCharacters="" EnableCaching="false" Enabled="True" 
                            FirstRowSelected="false" MinimumPrefixLength="0" 
                            OnClientItemSelected="cityautocompleteselected" ServiceMethod="Getcitynames" 
                            TargetControlID="TxtCityName">
                        </asp:AutoCompleteExtender>
                    </td>
                    <td class="style13">
                        &nbsp;</td>
                    <td class="style14">
                        &nbsp;</td>
                    <td>
                        &nbsp;</td>
                </tr>
                <tr>
                    <td class="style17">
                        &nbsp;</td>
                    <td class="style17">
                        &nbsp;</td>
                    <td class="style16">
                        &nbsp;</td>
                    <td class="style13">
                        &nbsp;</td>
                    <td class="style14">
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
                <tr>
                    <td colspan="6">
                        &nbsp;</td>
                </tr>
                <tr style="display:none;" >
                    <td class="style17">
                        <asp:Label ID="Label9" runat="server" Text="Search Name"></asp:Label>
                    </td>
                    <td class="style17">
                        &nbsp;</td>
                    <td class="style16">
                        <asp:TextBox ID="txtName" runat="server" AutoPostBack="True" Width="200px" 
                            Height="16px"></asp:TextBox>
                        <asp:AutoCompleteExtender ID="txtName_AutoCompleteExtender" runat="server" 
                            CompletionInterval="10" 
                            CompletionListCssClass="autocomplete_completionListElement" 
                            CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem" 
                            CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="1" 
                            DelimiterCharacters="" EnableCaching="false" Enabled="True" 
                            FirstRowSelected="True" MinimumPrefixLength="1" ServiceMethod="GetCountries" 
                            TargetControlID="txtName">
                        </asp:AutoCompleteExtender>
                    </td>
                    <td colspan="3">
                        <asp:TextBox ID="txtNameNew" runat="server" Visible="False"></asp:TextBox>
                        <asp:Label ID="Label6" runat="server" Font-Names="Verdana" Font-Size="Small" 
                            ForeColor="#990000" Text="Search By: Country, Region and Country Group Tags."></asp:Label>
                    </td>
                </tr>

                <tr>
                    <td colspan="5">
                        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                            <ContentTemplate>
                                <asp:DataList ID="dlList" runat="server" RepeatColumns="4" 
                                    RepeatDirection="Horizontal">
                                    <ItemTemplate>
                                        <table class="style1">
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblType" runat="server" Text='<%# Eval("Code") %>' 
                                                        Visible="false"></asp:Label>
                                                    <asp:LinkButton ID="lbCountry" runat="server" class="button button4" 
                                                        onclick="lbCountry_Click" text='<%# Eval("Sector") %>'>LinkButton</asp:LinkButton>
                                                    <asp:LinkButton ID="lbClose" runat="server" class="buttonClose button4" 
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
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
            </table>
        </td>
    </tr>

    <TR>
<TD class="td_cell" colspan="4">
    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
        <ContentTemplate>
        <div id="dv_SearchResult" runat="server" style="max-height:250px;overflow:auto;" >
           <asp:GridView ID="gv_SearchResult" runat="server" AllowSorting="True" 
                AutoGenerateColumns="False" BackColor="White" BorderColor="#999999" 
                BorderStyle="None" BorderWidth="1px" CellPadding="3" CssClass="td_cell" 
                Font-Size="10px" GridLines="Vertical" tabIndex="12" Width="100%">
                <FooterStyle CssClass="grdfooter" />
                <Columns>
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
      <asp:TemplateField HeaderText="Sector Code" Visible ="false" ><EditItemTemplate>
                                            <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("sectorcode") %>'></asp:TextBox>
                                        
</EditItemTemplate>
<ItemTemplate>
<asp:Label    id="lblSectorCode" runat="server" Text='<%# Bind("sectorcode") %>'></asp:Label> 
</ItemTemplate>
</asp:TemplateField>
<asp:BoundField DataField="sectorcode" SortExpression="sectorcode" HeaderText="Sector Code">
</asp:BoundField>
  <asp:TemplateField HeaderText="Sector Name" SortExpression="sectorname">
                    <ItemTemplate>
                        <asp:Label ID="lblSectorName" runat="server" Text='<%# Bind("sectorname") %>'></asp:Label>
                    </ItemTemplate>
        
                    <HeaderStyle HorizontalAlign="Left" Width="14%" />
                    <ItemStyle CssClass="sectorname" HorizontalAlign="Left" Width="8%" />
                </asp:TemplateField>
                <asp:BoundField  Visible ="false" DataField="sectorgroupcode" HeaderText="Sector Group Code"  ItemStyle-CssClass="sectorgroupcode"
                    HtmlEncode="False" SortExpression="sectorgroupcode" >
                    <ItemStyle HorizontalAlign="Left" Width="6%" />
                    <HeaderStyle HorizontalAlign="Left" Width="6%" />
                </asp:BoundField>
                <asp:TemplateField HeaderText="Sector Group Name" SortExpression="othtypname">
                    <ItemTemplate>
                        <asp:Label ID="lblSectorGroupName" runat="server" Text='<%# Bind("othtypname") %>'></asp:Label>
                    </ItemTemplate>
                                   </asp:TemplateField>
                <asp:BoundField  Visible ="false"  DataField="ctrycode" HeaderText="Country Code"  ItemStyle-CssClass="ctrycode"
                    SortExpression="ctrycode">
                 
                 
                </asp:BoundField>
                <asp:TemplateField   HeaderText="Country Name" SortExpression="ctryname">
                    <ItemTemplate>
                        <asp:Label ID="lblCountryName" runat="server" Text='<%# Bind("ctryname") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Left" Width="12%" />
                    <ItemStyle CssClass="ctryname" HorizontalAlign="Left" Width="12%" />
                </asp:TemplateField>
                <asp:BoundField Visible="false" DataField="citycode" HeaderText="City Code" HtmlEncode="False"  ItemStyle-CssClass="citycode"
                    SortExpression="citycode">
                    <ItemStyle HorizontalAlign="Left" Width="4%" />
                    <HeaderStyle HorizontalAlign="Left" Width="4%" />
                </asp:BoundField>
                <asp:TemplateField HeaderText="City Name" SortExpression="cityname">
                    <ItemTemplate>
                        <asp:Label ID="lblCityName" runat="server" Text='<%# Bind("cityname") %>'></asp:Label>
                    </ItemTemplate>
                     </asp:TemplateField>
                <asp:BoundField DataField="IsActive" HeaderText="Active"  ItemStyle-CssClass="IsActive"
                    SortExpression="IsActive">
                       <HeaderStyle Width="5%" />
                    <ItemStyle CssClass="IsActive" Width="5%" />
                </asp:BoundField>
                <asp:BoundField DataField="adddate" HeaderText="Date Created"  ItemStyle-CssClass="adddate"
                    HtmlEncode="False" SortExpression="adddate" 
                    DataFormatString="{0:dd/MM/yyyy hh:mm:ss tt} " >
                </asp:BoundField>
                <asp:BoundField DataField="adduser" HeaderText="User Created"   ItemStyle-CssClass="adduser"
                    HtmlEncode="False" SortExpression="adduser">
                    <ItemStyle HorizontalAlign="Left" Width="5%" />
                    <HeaderStyle HorizontalAlign="Left" Width="5%" />
                </asp:BoundField>
                <asp:BoundField DataField="moddate" 
                    DataFormatString="{0:dd/MM/yyyy hh:mm:ss tt} " HeaderText="Date Modified"    ItemStyle-CssClass="moddate"
                    HtmlEncode="False" SortExpression="moddate">
                    <ItemStyle HorizontalAlign="Left" Width="7%" />
                    <HeaderStyle HorizontalAlign="Left" Width="7%" />
                </asp:BoundField>
                <asp:BoundField DataField="moduser" HeaderText="User Modified"  ItemStyle-CssClass="moduser" 
                    SortExpression="moduser">
                    <ItemStyle HorizontalAlign="Left" Width="5%" />
                    <HeaderStyle HorizontalAlign="Left" Width="5%" />
                </asp:BoundField>
                </Columns>
                <RowStyle CssClass="grdRowstyle" />
                <SelectedRowStyle CssClass="grdselectrowstyle" />
                <PagerStyle CssClass="grdpagerstyle" HorizontalAlign="Center" />
                <HeaderStyle CssClass="grdheader" ForeColor="white" />
                <AlternatingRowStyle CssClass="grdAternaterow" />
            </asp:GridView>
                         </div>
        </ContentTemplate>
    </asp:UpdatePanel>
      <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                <ContentTemplate>
                                                       
         
         
                     
  <asp:GridView id="gvsearchgrid" tabIndex=16 runat="server" Font-Size="10px" Width="100%" CssClass="grdstyle" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False" BorderColor="#999999" BorderStyle="None" BorderWidth="1px" CellPadding="3" GridLines="Vertical">
<FooterStyle CssClass="grdfooter"></FooterStyle>
<Columns>
<asp:TemplateField Visible="false" HeaderText="Sector Code"><EditItemTemplate>
                                            <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("sectorcode") %>'></asp:TextBox>
                                        
</EditItemTemplate>
<ItemTemplate>
<asp:Label    id="lblSectorCode" runat="server" Text='<%# Bind("sectorcode") %>'></asp:Label> 
</ItemTemplate>
</asp:TemplateField>
<asp:BoundField DataField="sectorcode" SortExpression="sectorcode" HeaderText="Sector Code">
</asp:BoundField>
  <asp:TemplateField HeaderText="Sector Name" SortExpression="sectorname">
                    <ItemTemplate>
                        <asp:Label ID="lblSectorName" runat="server" Text='<%# Bind("sectorname") %>'></asp:Label>
                    </ItemTemplate>
        
                    <HeaderStyle HorizontalAlign="Left" Width="14%" />
                    <ItemStyle CssClass="sectorname" HorizontalAlign="Left" Width="8%" />
                </asp:TemplateField>
                <asp:BoundField  Visible ="false" DataField="sectorgroupcode" HeaderText="Sector Group Code"  ItemStyle-CssClass="sectorgroupcode"
                    HtmlEncode="False" SortExpression="sectorgroupcode" >
                    <ItemStyle HorizontalAlign="Left" Width="6%" />
                    <HeaderStyle HorizontalAlign="Left" Width="6%" />
                </asp:BoundField>
                <asp:TemplateField HeaderText="Sector Group Name" SortExpression="othtypname">
                    <ItemTemplate>
                        <asp:Label ID="lblSectorGroupName" runat="server" Text='<%# Bind("othtypname") %>'></asp:Label>
                    </ItemTemplate>
                                   </asp:TemplateField>
                <asp:BoundField  Visible ="false"  DataField="ctryname" HeaderText="Country Code"  ItemStyle-CssClass="ctrycode"
                    SortExpression="ctrycode">
                 
                
                </asp:BoundField>
                <asp:TemplateField   HeaderText="Country Name" SortExpression="ctryname">
                    <ItemTemplate>
                        <asp:Label ID="lblCountryName" runat="server" Text='<%# Bind("ctryname") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Left" Width="12%" />
                    <ItemStyle CssClass="ctryname" HorizontalAlign="Left" Width="12%" />
                </asp:TemplateField>
                <asp:BoundField Visible="false" DataField="citycode" HeaderText="City Code" HtmlEncode="False"  ItemStyle-CssClass="citycode"
                    SortExpression="citycode">
                    <ItemStyle HorizontalAlign="Left" Width="4%" />
                    <HeaderStyle HorizontalAlign="Left" Width="4%" />
                </asp:BoundField>
                <asp:TemplateField HeaderText="City Name" SortExpression="cityname">
                    <ItemTemplate>
                        <asp:Label ID="lblCityName" runat="server" Text='<%# Bind("cityname") %>'></asp:Label>
                    </ItemTemplate>
                     </asp:TemplateField>
                <asp:BoundField DataField="IsActive" HeaderText="Active"  ItemStyle-CssClass="IsActive"
                    SortExpression="IsActive">
                       <HeaderStyle Width="5%" />
                    <ItemStyle CssClass="IsActive" Width="5%" />
                </asp:BoundField>
                <asp:BoundField DataField="adddate" HeaderText="Date Created"  ItemStyle-CssClass="adddate"
                    HtmlEncode="False" SortExpression="adddate" 
                    DataFormatString="{0:dd/MM/yyyy hh:mm:ss tt} " >
                </asp:BoundField>
                <asp:BoundField DataField="adduser" HeaderText="User Created"   ItemStyle-CssClass="adduser"
                    HtmlEncode="False" SortExpression="adduser">
                    <ItemStyle HorizontalAlign="Left" Width="5%" />
                    <HeaderStyle HorizontalAlign="Left" Width="5%" />
                </asp:BoundField>
                <asp:BoundField DataField="moddate" 
                    DataFormatString="{0:dd/MM/yyyy hh:mm:ss tt} " HeaderText="Date Modified"    ItemStyle-CssClass="moddate"
                    HtmlEncode="False" SortExpression="moddate">
                    <ItemStyle HorizontalAlign="Left" Width="7%" />
                    <HeaderStyle HorizontalAlign="Left" Width="7%" />
                </asp:BoundField>
                <asp:BoundField DataField="moduser" HeaderText="User Modified"  ItemStyle-CssClass="moduser" 
                    SortExpression="moduser">
                    <ItemStyle HorizontalAlign="Left" Width="5%" />
                    <HeaderStyle HorizontalAlign="Left" Width="5%" />
                </asp:BoundField>

            </Columns>

<RowStyle CssClass="grdRowstyle"></RowStyle>

<SelectedRowStyle CssClass="grdselectrowstyle"></SelectedRowStyle>

<PagerStyle CssClass="grdpagerstyle" HorizontalAlign="Center"></PagerStyle>

<HeaderStyle CssClass="grdheader" ForeColor="White"></HeaderStyle>

<AlternatingRowStyle CssClass="grdAternaterow"></AlternatingRowStyle>
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
            <asp:ServiceReference Path="~/clsHotelGroupServices.asmx" />
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