<%@ Page Language="VB" MasterPageFile="~/MainPageMaster.master" AutoEventWireup="false"  EnableEventValidation="false" CodeFile="CountriesSearch.aspx.vb" Inherits="CountriesSearch"  %>
 <%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
 <%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
    <%@ OutputCache location="none" %> 
    
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
    
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
         $txtvsprocess.val('Region:" " Country:" " Currency:" " TEXT:" "');
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
                         case 'Region':
                             var asSqlqry = '';
                             glcallback = callback;

                             ColServices.clsSectorServices.GetListOfRegionVisualSearch(asSqlqry, fnFillSearchVS, ErrorHandler, TimeOutHandler);
                             break;
                         case 'Country':
                             var asSqlqry = '';
                             glcallback = callback;
                             ColServices.clsSectorServices.GetListOfArrayCountryVisualSearch(asSqlqry, fnFillSearchVS, ErrorHandler, TimeOutHandler);
                             break;
                         case 'Currency':
                             var asSqlqry = '';
                             glcallback = callback;
                             ColServices.clsSectorServices.GetListOfCurrencyVisualSearch(asSqlqry, fnFillSearchVS, ErrorHandler, TimeOutHandler);
                             break;
                     }
                 },
                 facetMatches: function (callback) {
                     callback([
                { label: 'Region', category: 'Countries' },
                { label: 'Country', category: 'Countries' },
                { label: 'Currency', category: 'Countries' },
                { label: 'Text', category: 'Countries' },

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

<style>
    .btnExample {
  color: #2D7C8A;
  background: #e7e7e7;
  font-weight: bold;
  border: 1px solid #2D7C8A;
   padding: 5px 5px;
}
 
.btnExample:hover {
  color: #FFF;
  background: #2D7C8A;
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

<script language="javascript" type ="text/javascript" >
    function GetValueFrom() {

        var ddl = document.getElementById("<%=ddlSCurName.ClientID%>");
        ddl.selectedIndex = -1;
        // Iterate through all dropdown items.
        for (i = 0; i < ddl.options.length; i++) {
            if (ddl.options[i].text ==
			document.getElementById("<%=ddlSCurCode.ClientID%>").value) {
                // Item was found, set the selected index.
                ddl.selectedIndex = i;
                return true;
            }
        }
    }
    function GetValueCode() {
        var ddl = document.getElementById("<%=ddlSCurCode.ClientID%>");
        ddl.selectedIndex = -1;
        // Iterate through all dropdown items.
        for (i = 0; i < ddl.options.length; i++) {
            if (ddl.options[i].text ==
			document.getElementById("<%=ddlSCurName.ClientID%>").value) {
                // Item was found, set the selected index.
                ddl.selectedIndex = i;
                return true;
            }
        }
    }
    function GetValueFromMkt() {

        var ddl = document.getElementById("<%=ddlSMktName.ClientID%>");
        ddl.selectedIndex = -1;
        // Iterate through all dropdown items.
        for (i = 0; i < ddl.options.length; i++) {
            if (ddl.options[i].text ==
			document.getElementById("<%=ddlSMktCode.ClientID%>").value) {
                // Item was found, set the selected index.
                ddl.selectedIndex = i;
                return true;
            }
        }
    }
    function GetValueCodeMkt() {
        var ddl = document.getElementById("<%=ddlSMktCode.ClientID%>");
        ddl.selectedIndex = -1;
        // Iterate through all dropdown items.
        for (i = 0; i < ddl.options.length; i++) {
            if (ddl.options[i].text ==
			document.getElementById("<%=ddlSMktName.ClientID%>").value) {
                // Item was found, set the selected index.
                ddl.selectedIndex = i;
                return true;
            }
        }
    }
</script> 
    <table style="width: 100%">
        <tr>
            <td align="left" style="width: 100%; height: 39px">
                <table style="border-right: gray 2px solid; border-top: gray 2px solid; border-left: gray 2px solid;
                    width: 100%; border-bottom: gray 2px solid; height: 157px;">
                    <tr>
                        <td align="center" class="field_heading" colspan="4" style="width:100%; height: 1px">
                            Countries List</td>
                    </tr>
                    <tr>
                        <td align="center" class="td_cell" colspan="4" style="color: blue; height: 6px; width:100%">
                            Type few characters of code or name and click search &nbsp; &nbsp;</td>
                    </tr>
                    <tr>
                        <td class="td_cell" colspan="4" style="width: 100%; color: red; height: 1px">
                            <asp:UpdatePanel id="UpdatePanel2" runat="server">
                                <contenttemplate>
<TABLE style="WIDTH: 912px"><TBODY>

<TR>
<TD align=center colSpan=6>
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp; &nbsp;&nbsp; &nbsp;<asp:Button id="cmdhelp" 
        tabIndex=3 onclick="btnhelp_Click" runat="server" Text="Help" 
        Font-Bold="False" CssClass="search_button"></asp:Button> &nbsp;&nbsp;<asp:Button id="btnAddNew" tabIndex=4 runat="server" Text="Add New" 
        Font-Bold="False" CssClass="btn"></asp:Button>&nbsp;&nbsp;<asp:Button id="btnPrint"
        tabIndex=5 runat="server" Text="Report" 
        CssClass="btn"></asp:Button></TD>
        </TR>



<TR style="display:none"><TD  WIDTH: "882px" class="td_cell" colSpan=9>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <asp:RadioButton id="rbsearch" runat="server" Text="Search" ForeColor="Black" CssClass="td_cell" __designer:wfdid="w72" Checked="True" OnCheckedChanged="rbsearch_CheckedChanged" GroupName="GrSearch" AutoPostBack="True"></asp:RadioButton>&nbsp;&nbsp;&nbsp;&nbsp;<asp:RadioButton id="rbnadsearch" runat="server" Text="Advance Search" ForeColor="Black" CssClass="td_cell" __designer:wfdid="w73" GroupName="GrSearch" AutoPostBack="True" wfdid="w6"></asp:RadioButton>&nbsp;&nbsp;

<asp:Button id="btnSearch" tabIndex=7 runat="server" Text="Search" 
        Font-Bold="False" CssClass="search_button" __designer:wfdid="w74"></asp:Button>&nbsp;
 <asp:Button id="btnClear" tabIndex=8 runat="server" Text="Clear" Font-Bold="False" 

        CssClass="search_button" __designer:wfdid="w75"></asp:Button>&nbsp;
        
        </tr>

      
        <tr style="display:none"><td>

 <asp:Button id="btnAddNew1" tabIndex=9 runat="server" Text="Add New" 
        Font-Bold="False" __designer:dtid="2533274790395920" CssClass="btn" 
        __designer:wfdid="w2"></asp:Button>&nbsp;
 <asp:Button id="btnPrint1" tabIndex=11 runat="server" Text="Report" 
        __designer:dtid="2533274790395922" CssClass="btn" __designer:wfdid="w3"></asp:Button>
    <asp:HiddenField ID="hdSDBName" runat="server" />
    </TD></TR>
    <TR style="display:none"><TD style="WIDTH: 67px; HEIGHT: 24px" class="td_cell"><SPAN style="COLOR: black">Country&nbsp;Code</SPAN></TD><TD style="WIDTH: 130px; HEIGHT: 24px">
<asp:TextBox id="txtcountrycode" tabIndex=1 runat="server" Width="160px" 
            CssClass="field_input" __designer:wfdid="w76" MaxLength="20"></asp:TextBox> </TD><TD style="WIDTH: 88px; HEIGHT: 24px" class="td_cell"><SPAN style="COLOR: black">Country Name</SPAN></TD><TD style="WIDTH: 257px; HEIGHT: 24px" class="td_cell">
      
       
       
       
        <asp:AutoCompleteExtender ID="txtcountryname_AutoCompleteExtender"   
         CompletionListCssClass="autocomplete_completionListElement" 
         CompletionListItemCssClass="autocomplete_listItem"
        CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem" 
           CompletionInterval="10"  EnableCaching="false"  CompletionSetCount="1" runat="server" 
           FirstRowSelected="True"  DelimiterCharacters="" Enabled="True"  ServiceMethod="GetCountries" 
            MinimumPrefixLength="1" 
            TargetControlID="txtcountryname">
        </asp:AutoCompleteExtender>  <asp:TextBox id="txtcountryname" tabIndex=2 
            runat="server" Width="267px" 
            CssClass="field_input" __designer:wfdid="w77" MaxLength="100" 
            AutoPostBack="True"></asp:TextBox> 
        </TD><TD style="WIDTH: 52px; HEIGHT: 24px" class="td_cell"><asp:Label id="Label3" runat="server" Text="Order By" ForeColor="Black" Width="58px" CssClass="field_caption" __designer:wfdid="w1"></asp:Label></TD><TD style="HEIGHT: 24px" class="td_cell"><asp:DropDownList id="ddlOrderBy" runat="server" Width="114px" CssClass="drpdown" __designer:wfdid="w9" AutoPostBack="True" OnSelectedIndexChanged="ddlOrderBy_SelectedIndexChanged"></asp:DropDownList></TD></TR></TBODY></TABLE><asp:Panel id="PnlCurr" runat="server" Width="703px" __designer:wfdid="w78" Visible="False"><TABLE><TBODY><TR><TD style="WIDTH: 74px; HEIGHT: 22px"><SPAN style="COLOR: black">CurrencyCode </SPAN></TD><TD style="WIDTH: 186px; HEIGHT: 22px"><SELECT onchange="GetValueFrom()" style="WIDTH: 184px" id="ddlSCurCode" class="drpdown" tabIndex=3 runat="server" enableviewstate="true" visible="true"> <OPTION selected></OPTION></SELECT> </TD><TD style="WIDTH: 100px; HEIGHT: 22px"><SPAN style="COLOR: black">Currency Name</SPAN></TD><TD style="WIDTH: 211px; HEIGHT: 22px"><SELECT onchange="GetValueCode()" style="WIDTH: 274px; HEIGHT: 20px" id="ddlSCurName" class="drpdown" tabIndex=4 runat="server" enableviewstate="true" visible="true"> <OPTION selected></OPTION></SELECT></TD></TR><TR><TD style="WIDTH: 74px; HEIGHT: 22px"><SPAN style="COLOR: black">Region Code</SPAN></TD><TD style="WIDTH: 186px; HEIGHT: 22px"><SELECT onchange="GetValueFromMkt()" style="WIDTH: 183px" id="ddlSMktCode" class="drpdown" tabIndex=5 runat="server" enableviewstate="true" visible="true"> <OPTION selected></OPTION></SELECT></TD><TD style="WIDTH: 100px; HEIGHT: 22px"><SPAN style="COLOR: black">Region Name</SPAN></TD><TD style="WIDTH: 211px; HEIGHT: 22px"><SELECT onchange="GetValueCodeMkt()" style="WIDTH: 275px; HEIGHT: 20px" id="ddlSMktName" class="drpdown" tabIndex=6 runat="server" enableviewstate="true" visible="true"> <OPTION selected></OPTION></SELECT></TD></TR></TBODY></TABLE></asp:Panel> 
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
                        <asp:Label ID="Label5" runat="server" CssClass="field_caption" Text="To Date"></asp:Label>
                    </td>
                    <td>
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
                    </td>
                    <td>
                        <asp:Button ID="btnFilter" runat="server" CssClass="btn" Font-Bold="False" 
                            tabIndex="4" Text="Search by Date" />
                        &nbsp;<asp:Button ID="btnClearDate" runat="server" CssClass="btn" Font-Bold="False" 
                            tabIndex="4" Text="Reset Dates" />
                    </td>
                               <td>  <asp:Label ID="RowSelectccs" runat="server" CssClass="field_caption" 
                            Text="Rows Selected "></asp:Label>
                     <asp:DropDownList ID="RowsPerPageCCS" runat="server" AutoPostBack="true">
                      
                          <asp:ListItem Value="5">5</asp:ListItem>
                            <asp:ListItem Value="10">10</asp:ListItem>
                             <asp:ListItem Value="15">15</asp:ListItem>
                            <asp:ListItem Value="20">20</asp:ListItem>
                             <asp:ListItem Value="25">25</asp:ListItem>
                            <asp:ListItem Value="30">30</asp:ListItem>
                        </asp:DropDownList>
                  
                    </td><tr>
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
    </TBODY></TABLE>
</contenttemplate>                
                </asp:UpdatePanel></td>                        
                    </tr>
                    
                    
                    <tr>


                        <td class="td_cell" colspan="4" style="width: 866px; color: red; height: 1px">
                            &nbsp;<asp:Button ID="btnExportToExcel" runat="server" CssClass="btn" TabIndex="10"
                                            Text="Export To Excel" />
                                        </td>
                    </tr>


                    <tr>
                        <td class="td_cell" colspan="4" style="width: 100%; color: red; height: 1px">
            
                <asp:UpdatePanel id="UpdatePanel1" runat="server">
                    <contenttemplate>
<asp:GridView id="gv_SearchResult" tabIndex=12 runat="server" Font-Size="10px" BackColor="White" Width="100%" CssClass="td_cell" __designer:wfdid="w79" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False" BorderColor="#999999" BorderStyle="None" BorderWidth="1px" CellPadding="3" GridLines="Vertical">
<FooterStyle CssClass="grdfooter"></FooterStyle>
<Columns>
<asp:TemplateField Visible="False" HeaderText="ctrycode"><EditItemTemplate>
<asp:TextBox runat="server" Text='<%# Bind("ctrycode") %>' id="TextBox1" 
        CssClass="field_input"></asp:TextBox>
</EditItemTemplate>
<ItemTemplate>
<asp:Label id="lblcntryCode" runat="server" Text='<%# Bind("ctrycode") %>' __designer:wfdid="w26"></asp:Label>
</ItemTemplate>
</asp:TemplateField>
<asp:BoundField DataField="ctrycode" SortExpression="ctrycode" HeaderText="Country Code"></asp:BoundField>

 <asp:TemplateField  HeaderText="Country Name" SortExpression="ctryname">
                    <ItemTemplate>
                        <asp:Label ID="lblCountryName" runat="server" Text='<%# Bind("ctryname") %>'></asp:Label>
                    </ItemTemplate>
                       <HeaderStyle HorizontalAlign="Left" Width="12%" />
                    <ItemStyle CssClass="ctryname" HorizontalAlign="Left" Width="12%" />
                </asp:TemplateField>
                     
                     <asp:TemplateField  HeaderText="Currency Name" SortExpression="currname">
                    <ItemTemplate>
                        <asp:Label ID="lblCurrencyName" runat="server" Text='<%# Bind("currname") %>'></asp:Label>
                    </ItemTemplate>
                     <HeaderStyle HorizontalAlign="Left" Width="12%" />
                    <ItemStyle CssClass="currname" HorizontalAlign="Left" Width="12%" />
                </asp:TemplateField>

                  <asp:TemplateField  HeaderText="Region Name" SortExpression="plgrpname">
                    <ItemTemplate>
                        <asp:Label ID="lblRegionName" runat="server" Text='<%# Bind("plgrpname") %>'></asp:Label>
                    </ItemTemplate>
                     <HeaderStyle HorizontalAlign="Left" Width="12%" />
                    <ItemStyle CssClass="plgrpname" HorizontalAlign="Left" Width="12%" />
                </asp:TemplateField>




<%--<asp:BoundField DataField="wkfrmday1" SortExpression="wkfrmday1" HeaderText="Weekend From1"></asp:BoundField>
<asp:BoundField HtmlEncode="False" DataField="wktoday1" SortExpression="wktoday1" HeaderText="Weekend To1"></asp:BoundField>
<asp:BoundField HtmlEncode="False" DataField="wkfrmday2" SortExpression="wkfrmday2" HeaderText="Weekend From2"></asp:BoundField>
<asp:BoundField DataField="wktoday2" SortExpression="wktoday2" HeaderText="Weekend To2"></asp:BoundField>--%>

<asp:BoundField DataField="nationality" Visible="False" SortExpression="nationality" HeaderText="Nationality"></asp:BoundField>
<asp:BoundField DataField="inclpromotion" Visible="False" SortExpression="inclpromotion" HeaderText="Include in Promotions"></asp:BoundField>
<asp:BoundField DataField="inclearlypromotion" Visible="False" SortExpression="inclearlypromotion" HeaderText="Include in Early Bird Promotions"></asp:BoundField>
<asp:BoundField DataField="isactive" SortExpression="active" HeaderText="Active"></asp:BoundField>
<asp:BoundField HtmlEncode="False" DataFormatString="{0:dd/MM/yyyy hh:mm:ss tt} " DataField="adddate" HeaderText="Date Created"></asp:BoundField>
<asp:BoundField DataField="adduser" HeaderText="User Created"></asp:BoundField>
<asp:BoundField HtmlEncode="False" DataFormatString="{0:dd/MM/yyyy hh:mm:ss tt} " DataField="moddate" HeaderText="Date Modified"></asp:BoundField>
<asp:BoundField DataField="moduser" HeaderText="User Modified"></asp:BoundField>
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

<RowStyle CssClass="grdRowstyle"></RowStyle>

<SelectedRowStyle CssClass="grdselectrowstyle"></SelectedRowStyle>

<PagerStyle CssClass="grdpagerstyle" HorizontalAlign="Center"></PagerStyle>

<HeaderStyle CssClass="grdheader" ForeColor="white"></HeaderStyle>

<AlternatingRowStyle CssClass="grdAternaterow"></AlternatingRowStyle>
</asp:GridView> 
<asp:Label id="lblMsg" runat="server" Text="Records not found. Please redefine search criteria" 
                            Font-Size="8pt" Font-Names="Verdana" Font-Bold="True" Width="357px" 
                            __designer:wfdid="w80" Visible="False" ForeColor="#084573"></asp:Label> 
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

