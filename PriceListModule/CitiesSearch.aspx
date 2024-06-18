    <%@ Page Language="VB" MasterPageFile="~/MainPageMaster.master" AutoEventWireup="false"  EnableEventValidation="false" CodeFile="CitiesSearch.aspx.vb" Inherits="CitiesSearch"  %>
 
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
         
          $txtvsprocess.val('Country:" " City:"  " TEXT:" "');

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
                          case 'Country':
                              var asSqlqry = '';
                             
                              glcallback = callback;
                             
                              ColServices.clsSectorServices.GetListOfCountryVisualSearch(asSqlqry, fnFillSearchVS, ErrorHandler, TimeOutHandler);
                              break;
                          case 'City':
                              var asSqlqry = '';
                              glcallback = callback;
                              ColServices.clsSectorServices.GetListOfCityVisualSearch(asSqlqry, fnFillSearchVS, ErrorHandler, TimeOutHandler);
                              break;

                      }
                  },
                  facetMatches: function (callback) {
                      callback([
                { label: 'Country', category: 'Cities' },
                { label: 'City', category: 'Cities' },
                { label: 'Text', category: 'Cities' },
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

    <script language="javascript" type ="text/javascript" >
        function CallWebMethod(methodType) {
            switch (methodType) {
                case "ctryname":
                    var select = document.getElementById("<%=ddlscname.ClientID%>");
                    var selectname = document.getElementById("<%=ddlscname.ClientID%>");
                    selectname.value = select.options[select.selectedIndex].text;
                    break;
                case "cityname":
                    var select = document.getElementById("<%=ddlsccode.ClientID%>");
                    var selectname = document.getElementById("<%=ddlsccode.ClientID%>");
                    selectname.value = select.options[select.selectedIndex].text;
                    break;
            }
        }

        function checkNumber(e) {

            if (event.keycode = 13) {
                return true;
            }
            if ((event.keyCode < 47 || event.keyCode > 57)) {
                return false;
            }

        }
        function checkCharacter(e) {
            if (event.keycode = 13) {
                return true;
            }

            if (event.keyCode == 32 || event.keyCode == 46)
                return;
            if ((event.keyCode < 47 || event.keyCode > 57) && (event.keyCode < 65 || event.keyCode > 91) && (event.keyCode < 96 || event.keyCode > 122)) {
                return false;
            }

        }
				
</script> 


    <table style="border-right: gray 2px solid; border-top: gray 2px solid; border-left: gray 2px solid; border-bottom: gray 2px solid">
        <tr>
            <td style="width: 100%; height: 11px">
                <table style="width: 100%">
                    <tr>
                        <td align="center" class="field_heading" style="width: 100%; height: 1px">
                Cities List</td>
                    </tr>
                    <tr style="display:none" >
                        <td align="center" class="td_cell" style="color: blue">
                            Type few characters of code or name and click search 
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 21px">
                            <asp:updatepanel id="UpdatePanel1" runat="server">
                                <contenttemplate>
                                  

                                    <TABLE style="WIDTH: 872px"> <TBODY>
	                           <TR>
<TD align=center colSpan=6>
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp; &nbsp;&nbsp; &nbsp;<asp:Button id="btnhelp" 
        tabIndex=3 onclick="btnhelp_Click" runat="server" Text="Help" 
        Font-Bold="False" CssClass="search_button"></asp:Button> &nbsp;&nbsp;<asp:Button id="btnAddNew" tabIndex=4 runat="server" Text="Add New" 
        Font-Bold="False" CssClass="btn"></asp:Button>&nbsp;&nbsp;<asp:Button id="btnPrint"
        tabIndex=5 runat="server" Text="Report" 
        CssClass="btn"></asp:Button></TD>
        </TR>
		                          
                                  
                                            <TR style="display:none">
			                                    <TD style="HEIGHT: 22px; TEXT-ALIGN: center" colSpan=4>
				                                    <asp:RadioButton id="rbtnsearch" runat="server" Text="Search" ForeColor="Black" CssClass="td_cell" Checked="True" OnCheckedChanged="rbtnsearch_CheckedChanged" GroupName="GrSearch" AutoPostBack="True" __designer:wfdid="w40"></asp:RadioButton>&nbsp;

				                                    <asp:RadioButton id="rbtnadsearch" runat="server" Text="Advance Search" ForeColor="Black" Width="180px" CssClass="td_cell" OnCheckedChanged="rbtnadsearch_CheckedChanged" GroupName="GrSearch" AutoPostBack="True" __designer:wfdid="w41" ></asp:RadioButton>&nbsp;

				                                    <asp:Button id="btnSearch" tabIndex=5 runat="server" Text="Search" 
                                            Font-Bold="False" CssClass="search_button" __designer:wfdid="w42" 
                                            EnableTheming="True" Font-Italic="False" style="display:none"></asp:Button>&nbsp;

				                                    <asp:Button id="btnClear" tabIndex=6 runat="server" Text="Clear" 
                                            CssClass="search_button" __designer:wfdid="w43" style="display:none"></asp:Button>&nbsp;
                                           
				                                    <asp:Button id="btnhelp1" tabIndex=10 onclick="btnhelp_Click" runat="server" 
                                            Text="Help" CssClass="search_button" __designer:wfdid="w3"></asp:Button>&nbsp;

				                                    <asp:Button id="btnAddNew1" tabIndex=7 runat="server" Text="Add New" 
                                            Font-Bold="False" __designer:dtid="14636698788954128" CssClass="btn" 
                                            __designer:wfdid="w4"></asp:Button>&nbsp;

                                                    <asp:Button id="btnPrint1" tabIndex=9 runat="server" Text="Report" CssClass="btn" ></asp:Button>
			                                    </TD>
			                                    <TD style="WIDTH: 63px; HEIGHT: 22px; TEXT-ALIGN: center">
				                                    &nbsp;
			                                    </TD>
			                                    <TD style="WIDTH: 100px; HEIGHT: 22px; TEXT-ALIGN: center" colSpan=1></TD>
		                                    </TR>
		                                        <TR style="display:none">
			                                    <TD style="WIDTH: 62px">
				                                    <asp:Label id="Label1" runat="server" Text="City Code" Width="72px" CssClass="field_caption" __designer:wfdid="w44"></asp:Label>
			                                    </TD>
			                                    <TD style="WIDTH: 73px">
				                                    <asp:TextBox id="txtcitycode" tabIndex=1 runat="server" Width="203px" 
                                            CssClass="field_input" __designer:wfdid="w45" MaxLength="20"></asp:TextBox>
			                                    </TD>
			                                    <TD style="WIDTH: 100px">
				                                    <asp:Label id="Label2" runat="server" Text="City Name" Width="72px" CssClass="field_caption" __designer:wfdid="w46"></asp:Label>
			                                    </TD>
			                                    <TD style="WIDTH: 100px">
				                                    <asp:TextBox id="txtcityname" tabIndex=2 runat="server" Width="263px" 
                                            CssClass="field_input" __designer:wfdid="w47"></asp:TextBox>
			                                    </TD>
			                                    <TD style="WIDTH: 63px">
				                                    <asp:Label id="Label3" runat="server" Text="Order By" Width="72px" CssClass="field_caption" __designer:wfdid="w3"></asp:Label>
			                                    </TD>
			                                    <TD style="WIDTH: 100px">
				                                    <asp:DropDownList id="ddlOrderBy" runat="server" Width="104px" CssClass="drpdown" AutoPostBack="True" __designer:wfdid="w4" OnSelectedIndexChanged="ddlOrderBy_SelectedIndexChanged"></asp:DropDownList>
			                                    </TD>
		                                    </TR>
		                                    <TR style="display:none">
			                                    <TD style="WIDTH: 62px; HEIGHT: 22px">
				                                    <asp:Label id="lblctrycode" runat="server" Text="Country Code" Width="72px" CssClass="field_caption" __designer:wfdid="w48" Visible="False"></asp:Label>
			                                    </TD>
			                                    <TD style="WIDTH: 73px; HEIGHT: 22px">
				                                    <SELECT style="WIDTH: 209px" id="ddlsccode" class="drpdown" tabIndex=3 onchange="CallWebMethod('ctrycode')" runat="server" visible="false" enableviewstate="true">
					                                    <OPTION selected></OPTION>
				                                    </SELECT>
			                                    </TD>
			                                    <TD style="WIDTH: 100px; HEIGHT: 22px">
				                                    <asp:Label id="lblctryname" runat="server" Text="Country  Name" Width="74px" CssClass="field_caption" __designer:wfdid="w49" Visible="False"></asp:Label>
			                                    </TD>
			                                    <TD style="WIDTH: 100px; HEIGHT: 22px">
				                                    <SELECT style="WIDTH: 269px" id="ddlscname" class="drpdown" tabIndex=4 onchange="CallWebMethod('ctryname')" runat="server" Visible="false">
					                                    <OPTION selected></OPTION>
				                                    </SELECT>
			                                    </TD>
			                                    <TD style="WIDTH: 63px; HEIGHT: 22px"></TD>
			                                    <TD style="WIDTH: 100px; HEIGHT: 22px"></TD>
		                                 
           </TR>
</TBODY>
</TABLE> &nbsp;
</contenttemplate>
</asp:UpdatePanel>
</td>
       
          <TR>                                        
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
                               <td>  <asp:Label ID="RowSelectcs" runat="server" CssClass="field_caption" 
                            Text="Rows Selected "></asp:Label>
                     <asp:DropDownList ID="RowsPerPageCS" runat="server" AutoPostBack="true">
                      
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
                        <td style="height: 15px;width:100%">
                            &nbsp;<asp:Button ID="btnExportToExcel" runat="server" CssClass="btn" 
                                Text="Export To Excel" TabIndex="8" />
                            </td>
                    </tr>
                    <tr>
                        <td style="height: 33px; width: 100%">
	                    <asp:updatepanel id="UpdatePanel2" runat="server">
		                    <contenttemplate>
			                    <asp:GridView id="gv_SearchResult" tabIndex=10 runat="server" Font-Size="10px" Width="100%" CssClass="grdstyle" __designer:wfdid="w50" GridLines="Vertical" CellPadding="3" BorderWidth="1px" BorderStyle="None" AutoGenerateColumns="False" AllowSorting="True" AllowPaging="True">
				                    <FooterStyle CssClass="grdfooter"></FooterStyle>
				                    <Columns>
					                    <asp:TemplateField Visible="False" HeaderText="City Code">
						                    <EditItemTemplate>
							                    <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("citycode") %>' CssClass="field_input">
							                    </asp:TextBox>
						                    </EditItemTemplate>
						                    <ItemTemplate>
							                    <asp:Label id="lblCityCode" runat="server" Text='<%# Bind("citycode") %>'>
							                    </asp:Label>
						                    </ItemTemplate>
					                    </asp:TemplateField>
					                    <asp:BoundField DataField="citycode" SortExpression="citycode" HeaderText="City Code">
						                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
						                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
					                    </asp:BoundField>
					                     <asp:TemplateField HeaderText="City Name" SortExpression="cityname">
                    <ItemTemplate>
                        <asp:Label ID="lblCityName" runat="server" Text='<%# Bind("cityname") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Left" Width="7%" />
                    <ItemStyle CssClass="cityname" HorizontalAlign="Left" Width="7%" />
                </asp:TemplateField>
					                  <asp:TemplateField  HeaderText="Country Name" SortExpression="ctryname">
                    <ItemTemplate>
                        <asp:Label ID="lblCountryName" runat="server" Text='<%# Bind("ctryname") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Left" Width="12%" />
                    <ItemStyle CssClass="ctryname" HorizontalAlign="Left" Width="12%" />
                </asp:TemplateField>
					                    <asp:BoundField DataField="rankorder" SortExpression="rankorder" HeaderText="Rank Order">
						                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
						                    <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
					                    </asp:BoundField>
					                    <asp:BoundField HtmlEncode="False" DataField="showweb" SortExpression="showweb" HeaderText="Show in Web">
						                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
						                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
					                    </asp:BoundField>
					                    <asp:BoundField HtmlEncode="False" DataField="isactive" SortExpression="isactive" HeaderText="Active">
						                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
						                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
					                    </asp:BoundField>
					                    <asp:BoundField DataField="adddate" SortExpression="adddate" HeaderText="Date Created">
						                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
						                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
					                    </asp:BoundField>
					                    <asp:BoundField HtmlEncode="False" DataField="adduser" SortExpression="adduser" HeaderText="User Created">
						                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
						                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
					                    </asp:BoundField>
					                    <asp:BoundField DataField="moddate" SortExpression="moddate" HeaderText="Date Modified">
						                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
						                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
					                    </asp:BoundField>
					                    <asp:BoundField DataField="moduser" SortExpression="moduser" HeaderText="User Modified">
						                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
						                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
					                    </asp:BoundField>
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
			                    <asp:Label id="lblMsg" runat="server" Text="Records not found. Please redefine search criteria" Font-Size="8pt" Font-Names="Verdana" ForeColor="#084573" Font-Bold="True" Width="357px" __designer:wfdid="w51" Visible="False"></asp:Label> 
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
