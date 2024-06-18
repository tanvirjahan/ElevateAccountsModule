<%@ Page Language="VB" MasterPageFile="~/MainPageMaster.master"AutoEventWireup="false" CodeFile="HotelChainMaster.aspx.vb" Inherits="HotelChainMaster" %>
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
      }var ValueRetrieval

      function visualsearchbox() {
          var ValueRetrieval = document.getElementById("<%=hdOPMode.ClientID%>");

          var $txtvsprocess = $(document).find('.cs_txtvsprocess');
        

            if (ValueRetrieval.value == "S") {
             $txtvsprocess.val('Hotels:" " City:" " Sector:" " Category:" " HotelChain:" " Text:" "');
            }
         if (ValueRetrieval.value !== "S") {
             $txtvsprocess.val('Hotels:" " City:" " Sector:" " Category:" " Text:" "');
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

                              case 'Hotels':
                                  var asSqlqry = '';
                                  glcallback = callback;
                                  //fnTestCallback();
                                  ColServices.clsHotelGroupServices.GetListOfArrayHotelsVisualSearch(asSqlqry, fnFillSearchVS, ErrorHandler, TimeOutHandler);
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
                                  ColServices.clsHotelGroupServices.GetListOfArraySectorVisualSearch(asSqlqry, fnFillSearchVS, ErrorHandler, TimeOutHandler);
                                  break;
                              case 'Category':
                                  var asSqlqry = '';
                                  glcallback = callback;
                                  //fnTestCallback();
                                  ColServices.clsHotelGroupServices.GetListOfArrayCategoryVisualSearch(asSqlqry, fnFillSearchVS, ErrorHandler, TimeOutHandler);
                                  break;

                              case 'HotelChain':
                                  var asSqlqry = '';
                                  glcallback = callback;
                                  //fnTestCallback();
                                  ColServices.clsHotelGroupServices.GetListOfArrayHotelChainVisualSearch(asSqlqry, fnFillSearchVS, ErrorHandler, TimeOutHandler);
                                  break;
                          } 
                      }
                      //                          case 'Hotels':
                      //                              var asSqlqry = '';
                      //                              glcallback = callback;
                      //                              //fnTestCallback();
                    if (ValueRetrieval.value !== "S") {
                        switch (category) {
                      case 'Hotels':
                                  var asSqlqry = '';
                                  glcallback = callback;
                                  //fnTestCallback();
                                  ColServices.clsHotelGroupServices.GetListOfArrayHotelNameVisualSearch(asSqlqry, fnFillSearchVS, ErrorHandler, TimeOutHandler);
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
                                  ColServices.clsHotelGroupServices.GetListOfArraySectorVisualSearch(asSqlqry, fnFillSearchVS, ErrorHandler, TimeOutHandler);
                                  break;
                              case 'Category':
                                  var asSqlqry = '';
                                  glcallback = callback;
                                  //fnTestCallback();
                                  ColServices.clsHotelGroupServices.GetListOfArrayCategoryVisualSearch(asSqlqry, fnFillSearchVS, ErrorHandler, TimeOutHandler);
                                  break;

                              case 'HotelChain':
                                  var asSqlqry = '';
                                  glcallback = callback;
                                  //fnTestCallback();
                                  ColServices.clsHotelGroupServices.GetListOfArrayHotelChainVisualSearch(asSqlqry, fnFillSearchVS, ErrorHandler, TimeOutHandler);
                                  break;}} //                              ColServices.clsHotelGroupServices.GetListOfArrayHotelsVisualSearch(asSqlqry, fnFillSearchVS, ErrorHandler, TimeOutHandler);
                      //                              break;
                                     //fnTestCallback();
                      //                              ColServices.clsHotelGroupServices.GetListOfArrayHotelsVisualSearch(asSqlqry, fnFillSearchVS, ErrorHandler, TimeOutHandler);
                      //                              break;
                  }
              },
              facetMatches: function (callback) {

                  if (ValueRetrieval.value !== "S")
                  //  {
                      callback([
                                { label: 'Text' },
                                { label: 'Hotels', category: 'Hotels' },
                                { label: 'City', category: 'Hotels' },
                                { label: 'Sector', category: 'Hotels' },
                                { label: 'Category', category: 'Hotels' },
                                { label: 'HotelChain', category: 'Hotels' },
                                  ]);
                  //  }


                  //                           if(ValueRetrieval.value !=="")
                  //                           {
                  //                              callback([
                  //                                { label: 'Text' },
                  //                                { label: 'Hotels', category: 'Hotels' },
                  //                                { label: 'City', category: 'Hotels' },
                  //                                { label: 'Sector', category: 'Hotels' },
                  //                                { label: 'Category', category: 'Hotels' },
                  //                                //    { label: 'HotelChain', category: 'Hotels' },
                  //                
                  //                        ]);
                  //                      }

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
      
   
        prm.add_endRequest(EndRequestUserControl);
    


          function EndRequestUserControl(sender, args) {
             
              txtNameAutoCompleteExtenderKeyUp();
             visualsearchbox();
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
            &nbsp;<asp:Button ID="btnAddHotel"  Visible="false" runat="server" CssClass="btn" Font-Bold="False" 
                tabIndex="4" Text="Add New Hotel" />
            &nbsp;<asp:Button ID="btnHotelReport" runat="server" CssClass="btn" 
                Font-Bold="False"  Visible="true"  tabIndex="4" Text="Hotel Report" />
        </td>
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
        <td class="style4" colspan="4" valign="top">
            <table class="style1">
                <tr>
                    <td>
                        &nbsp;</td>
                    <td>
                        &nbsp;</td>
                    <td colspan="3">
                        <asp:HiddenField ID="hdFillByGrid" runat="server" />
                        <asp:HiddenField ID="hdLinkButtonValue" runat="server" />
                        <asp:HiddenField ID="hdOPMode" runat="server" />
                    </td>
                    <td>
                        &nbsp;</td>
              </tr>
                <tr runat="server" id="trNameAndCode">
                    <td>
                        &nbsp;</td>
                    <td colspan="2">
                        <asp:Label ID="Label10" runat="server" Text="Code"></asp:Label>
                        &nbsp;
                        <asp:TextBox ID="txtGroupCode" runat="server"></asp:TextBox>

                      
                    </td>  
                    <td colspan="2">
                        <asp:Label ID="Label7" runat="server" Text="Hotel Chain"></asp:Label>
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
                            FirstRowSelected="false" MinimumPrefixLength="0" ServiceMethod="GetHotelchain" 
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
                        &nbsp;</td>
                    <td>
                        <asp:Label ID="Label9" runat="server" Text="Search Name"></asp:Label>
                    </td>
                   
                    <TD style="WIDTH: 318px; HEIGHT: 24px">
  
                    <asp:TextBox ID="txtName" runat="server" AutoPostBack="True"
                            CssClass="field_input" MaxLength="500" TabIndex="3" Width="176px"></asp:TextBox>
                           <asp:TextBox ID="tCode" runat="server" style="display:none"  ></asp:TextBox>
                            <asp:HiddenField ID="hdnptCode" runat="server"  />
                        <asp:AutoCompleteExtender ID="txtName_AutoCompleteExtender" runat="server" CompletionInterval="10"
                            CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                            CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="1" DelimiterCharacters=""
                            EnableCaching="false" Enabled="True" FirstRowSelected="false" MinimumPrefixLength="0"
                            ServiceMethod="GetHotelChain" TargetControlID="txtName" OnClientItemSelected="txtNameautocompleteselected">
                        </asp:AutoCompleteExtender>
                        <input style="display:none" id="Text1" class="field_input" type="text"
                           runat="server" />
                        <input style="display:none" id="Text2" class="field_input" type="text"
                             runat="server" /> 
                                           
                    </td>
                    <td>
                    <asp:TextBox ID="txtNameNew" runat="server" Visible="False"></asp:TextBox>
                        <asp:Label ID="Label6" runat="server" Font-Names="Verdana" Font-Size="Small" 
                            ForeColor="#990000" Text="Search By: Hotel chain Tags."></asp:Label>
                    </td>
                       
                    <td>
                   
                    


                        &nbsp;</td>
                </tr>

                <tr>
                    <td>
                        &nbsp;</td>
                    <td colspan="4">
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
                                                        text='<%# Eval("Hotels") %>' onclick="lbCountry_Click">LinkButton</asp:LinkButton>
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
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <td>
                        &nbsp;</td>
                </tr>
                <tr>
                    <td>
                        &nbsp;</td>
                    <td colspan="4">
                        &nbsp;</td>
                    <td>
                        &nbsp;</td>
                </tr>
            </table>
        </td>
    </tr>

    <TR>
<TD class="td_cell" colspan="4">
    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
        <ContentTemplate>
        <div id="dv_SearchResult" runat="server" style="max-height:250px;overflow:auto;" >
            <asp:GridView ID="gv_SearchResult" runat="server" 
                AllowSorting="True" AutoGenerateColumns="False" 
                BackColor="White" BorderColor="#999999" BorderStyle="None" BorderWidth="1px" 
                CellPadding="3" CssClass="td_cell" Font-Size="10px" GridLines="Vertical" 
                tabIndex="12" Width="100%">
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
                                                         <asp:TemplateField HeaderText="HotelCode" Visible="False">
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="TextBox3" runat="server" CssClass="field_input" 
                                                        Text='<%# Bind("partycode") %>'></asp:TextBox>
                                                </EditItemTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblHotelCode" runat="server" 
                                                        Text='<%# Bind("partycode") %>'></asp:Label>
                        <asp:Label ID="lblHotelName" runat="server" 
                                                        Text='<%# Bind("partyname") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                <asp:BoundField DataField="partycode" HeaderText="Hotel Code">
                                                                                            <ItemStyle HorizontalAlign="Center" />
                                            </asp:BoundField>
                                             <asp:TemplateField HeaderText="Hotel Name">
                                                                           <ItemTemplate>
                                                    <asp:Label ID="lblPartyName" runat="server" Text='<%# Bind("partyname") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Category">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCatName" runat="server" Text='<%# Bind("catname") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="City">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCityName" runat="server" Text='<%# Bind("cityname") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Sector">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSectName" runat="server" Text='<%# Bind("sectorname") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Hotel Chain">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblHotelGroup" runat="server" Text='<%# Bind("hotelchainname") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="tel1" HeaderText="Telephone" />
                                            <asp:BoundField DataField="contact1" HeaderText="Contact" />
                                        
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
                                            <asp:TemplateField HeaderText="HotelCode" Visible="False">
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="TextBox3" runat="server" CssClass="field_input" 
                                                        Text='<%# Bind("partycode") %>'></asp:TextBox>
                                                </EditItemTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblHotelCode" runat="server" 
                                                        Text='<%# Bind("partycode") %>'></asp:Label>
                                                                  </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="partycode" HeaderText="Hotel Code">
                                                                                            <ItemStyle HorizontalAlign="Center" />
                                            </asp:BoundField>
                                            <asp:TemplateField HeaderText="Hotel Name">
                                                                           <ItemTemplate>
                                                    <asp:Label ID="lblPartyName" runat="server" Text='<%# Bind("partyname") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Category">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCatName" runat="server" Text='<%# Bind("catname") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="City">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCityName" runat="server" Text='<%# Bind("cityname") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Sector">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSectName" runat="server" Text='<%# Bind("sectorname") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Hotel Chain">
                                                <ItemTemplate>
                                             <asp:Label ID="lblHotelGroup" runat="server" Text='<%# Bind("hotelchainname") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="tel1" HeaderText="Telephone" />
                                            <asp:BoundField DataField="contact1" HeaderText="Contact" />
                                            <asp:TemplateField visible="false">
                                                <ItemTemplate >
                                                     <asp:LinkButton ID="lnkEdit"  runat="server" onclick="lnkEdit_Click">Edit</asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField visible="false">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkView"   runat="server" onclick="lnkView_Click">View</asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField  visible="false">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkDelete" runat="server" onclick="lnkDelete_Click">Delete</asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField visible="false">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkCopy"   runat="server" onclick="lnkCopy_Click">Copy</asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField> 
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
            <asp:ServiceReference Path="~/clsHotelGroupServices.asmx" />
        </services>
    </asp:ScriptManagerProxy>
            </div>

 <script type="text/javascript">
  
 </script>
   
</contenttemplate>


    </asp:UpdatePanel>
   
     </asp:Content>
            
               
                       
                 
                               
       

 