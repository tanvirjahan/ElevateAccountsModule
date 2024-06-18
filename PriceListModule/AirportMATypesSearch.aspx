﻿<%@ Page Title="" Language="VB" MasterPageFile="~/PriceListMaster.master" AutoEventWireup="false" CodeFile="AirportMATypesSearch.aspx.vb" Inherits="PriceListModule_AirportMATypesSearch" %>
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

         $txtvsprocess.val('TYPENAME:" " AIRPORT:"  " RATEBASIS:"  " AIRPORTMEETTYPE:"  " SUPPLIER:"  " TEXT:" "');
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
                         case 'TYPENAME':
                             var asSqlqry = '';
                             glcallback = callback;
                             ColServices.clsSectorServices.GetListOfTypeNameVisualSearch(asSqlqry, fnFillSearchVS, ErrorHandler, TimeOutHandler);
                             break;

                         case 'AIRPORT':
                             var asSqlqry = '';
                             glcallback = callback;
                             ColServices.clsSectorServices.GetListOfAirportNameVisualSearch(asSqlqry, fnFillSearchVS, ErrorHandler, TimeOutHandler);
                             break;

                         case 'RATEBASIS':
                             var asSqlqry = "SELECT 'ADULT/CHILD' AS RATEBASIS UNION SELECT 'UNIT' AS RATEBASIS order by ratebasis";
                             glcallback = callback;
                             //ColServices.clsSectorServices.GetListOfTranDocVisualSearchStrNew(asSqlqry, fnFillSearchVS, ErrorHandler, TimeOutHandler);
                             ColServices.clsServices.GetListOfArrayVisualSearch(asSqlqry, fnFillSearchVS, ErrorHandler, TimeOutHandler);
                             break;

                         case 'AIRPORTMEETTYPE':
                             var asSqlqry = "SELECT 'ARRIVAL' AS TYPE UNION SELECT 'DEPARTURE' AS TYPE UNION SELECT 'ARRIVAL OR DEPARTURE' AS TYPE UNION SELECT  'TRANSIT' AS TYPE order by type";
                             glcallback = callback;
                             //ColServices.clsSectorServices.GetListOfTranDocVisualSearchStrNew(asSqlqry, fnFillSearchVS, ErrorHandler, TimeOutHandler);
                             ColServices.clsServices.GetListOfArrayVisualSearch(asSqlqry, fnFillSearchVS, ErrorHandler, TimeOutHandler);
                             break;
                         case 'SUPPLIER':
                             var asSqlqry = "select partyname from partymast where sptypecode =(select option_selected from reservation_parameters where  param_id='564') order by partyname";
                             glcallback = callback;
                             //ColServices.clsSectorServices.GetListOfTranDocVisualSearchStrNew(asSqlqry, fnFillSearchVS, ErrorHandler, TimeOutHandler);
                             ColServices.clsServices.GetListOfArrayVisualSearch(asSqlqry, fnFillSearchVS, ErrorHandler, TimeOutHandler);
                             break;
                     }
                 },
                 facetMatches: function (callback) {
                     callback([
                 { label: 'TYPENAME', category: 'TYPE' },
                { label: 'AIRPORT', category: 'TYPE' },
                { label: 'RATEBASIS', category: 'TYPE' },
                { label: 'AIRPORTMEETTYPE', category: 'TYPE' },
                { label: 'SUPPLIER', category: 'TYPE' },
                { label: 'TEXT', category: 'TYPE' },
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

<script language="javascript" type="text/javascript" >


    function GetValueFromGrpName() {

        var ddl = document.getElementById("<%=ddlGrpName.ClientID%>");
        ddl.selectedIndex = -1;
        // Iterate through all dropdown items.
        for (i = 0; i < ddl.options.length; i++) {
            if (ddl.options[i].text == document.getElementById("<%=ddlGrpCode.ClientID%>").value) {
                // Item was found, set the selected index.
                ddl.selectedIndex = i;
                return true;
            }
        }
    }

    function GetValueFromGrpCode() {
        var ddl = document.getElementById("<%=ddlGrpCode.ClientID%>");
        ddl.selectedIndex = -1;
        // Iterate through all dropdown items.
        for (i = 0; i < ddl.options.length; i++) {
            if (ddl.options[i].text == document.getElementById("<%=ddlGrpName.ClientID%>").value) {
                // Item was found, set the selected index.
                ddl.selectedIndex = i;
                return true;
            }
        }
    }
   

</script>

<table style="border-right: gray 2px solid; border-top: gray 2px solid; border-left: gray 2px solid;
        width: 100%; border-bottom: gray 2px solid">
        <tr>
            <td align="center" class="field_heading" colspan="4" style="width:100%; height: 7px">
                <asp:Label id="lblHeading" runat="server" 
                    Text="Airport Meet &amp; Assist Types List" ForeColor="White" 
                    __designer:wfdid="w17" Width="100%" CssClass="field_heading"></asp:Label></td>
        </tr>
        <tr>
            <td align="center" class="td_cell" colspan="4" style="color: blue; width:100%; height: 7px;">
                Type few characters of code or name and click search &nbsp; &nbsp;</td>
        </tr>
        <tr>
            <td class="td_cell" colspan="4" style="width: 100%; color: red;">
                <asp:UpdatePanel id="UpdatePanel1" runat="server">
                    <contenttemplate>
<TABLE style="WIDTH: 666px"><TBODY>


                                          <TR>
<TD align=center colSpan=6>
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp; &nbsp;&nbsp; &nbsp;<asp:Button id="btnhelp" 
        tabIndex=3 onclick="btnhelp_Click" runat="server" Text="Help" 
        Font-Bold="False" CssClass="search_button"></asp:Button> &nbsp;&nbsp;<asp:Button id="btnAddNew" tabIndex=4 runat="server" Text="Add New" 
        Font-Bold="False" CssClass="btn"></asp:Button>&nbsp;&nbsp;<asp:Button id="btnPrint"
        tabIndex=5 runat="server" Text="Report" 
        CssClass="btn"></asp:Button></TD>
        </TR>
	

        <TR>

   <td  style="display:none">
   
       &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:RadioButton 
           ID="rbtnsearch" runat="server" __designer:dtid="2814749767106575" 
           __designer:wfdid="w6" AutoPostBack="True" Checked="True" CssClass="td_cell" 
           ForeColor="Black" GroupName="GrSearch" 
           OnCheckedChanged="rbtnsearch_CheckedChanged" Text="Search" />
       &nbsp;
       <asp:RadioButton ID="rbtnadsearch" runat="server" 
           __designer:dtid="2814749767106576" __designer:wfdid="w7" AutoPostBack="True" 
           CssClass="td_cell" ForeColor="Black" GroupName="GrSearch" 
           OnCheckedChanged="rbtnadsearch_CheckedChanged" Text="Advance Search" 
           Visible="false" />
       <asp:Button ID="btnSearch" runat="server" CssClass="search_button" 
           Font-Bold="False" tabIndex="5" Text="Search" />
       &nbsp;<asp:Button ID="btnClear" runat="server" CssClass="search_button" 
           Font-Bold="False" tabIndex="6" Text="Clear" />
       &nbsp; </TD>
     
       

<TR><TD style="display:none">
    <INPUT style="WIDTH: 163px" id="txtCode" tabIndex=1 type=text maxLength=20 
           runat="server" class="field_input" />
    <asp:Label id="Label1" runat="server" Text="Type Name" __designer:wfdid="w1" 
        Width="100px"></asp:Label>
    <INPUT style="WIDTH: 241px" id="txtName" tabIndex=2 type=text maxLength=100 
           runat="server" class="field_input" />
   <asp:Label id="Label3" runat="server" Text="Order By" ForeColor="Black" 
        Width="50px" CssClass="field_caption" __designer:wfdid="w1"></asp:Label>
   </TD>
    <td style="display:none">
        <asp:DropDownList ID="ddlOrderBy" runat="server" __designer:wfdid="w23" 
            AutoPostBack="True" CssClass="drpdown" 
            OnSelectedIndexChanged="ddlOrderBy_SelectedIndexChanged" Width="130px">
        </asp:DropDownList>
    </td>
    </TR>



    <tr>
        <td style="display:none">
            <asp:Label ID="lblgrpcode" runat="server" __designer:wfdid="w17" 
                CssClass="td_cell" ForeColor="Black" Text="Group Code" Visible="False"></asp:Label>
            <select id="ddlGrpCode" runat="server" class="drpdown" 
                onblur="GetValueFromGrpName()" style="WIDTH: 148px" tabindex="5" 
                visible="false">
                <option selected=""></option>
            </select>
            <asp:Label ID="lblgrpname" runat="server" __designer:wfdid="w17" 
                CssClass="td_cell" ForeColor="Black" Text="Group Name" Visible="False" 
                Width="100px"></asp:Label>
            <select id="ddlGrpName" runat="server" class="drpdown" 
                onblur="GetValueFromGrpCode()" style="WIDTH: 205px" tabindex="6" 
                visible="false">
                <option selected=""></option>
            </select>
        </td>
    </tr>



<%--<TR><TD style="WIDTH: 76px; HEIGHT: 16px" class="td_cell"><SPAN style="COLOR: black">
<asp:Label id="lblgrpcode" runat="server" Text="Group Code" __designer:wfdid="w1" Visible="False"></asp:Label></SPAN></TD>
<TD style="WIDTH: 11px"><SELECT onblur="GetOtherGrpValueFrom()" style="WIDTH: 170px" id="ddlOtherGrpCode" class="drpdown" tabIndex=3 runat="server" Visible="false"> <OPTION selected></OPTION></SELECT></TD>
<TD style="WIDTH: 54px; HEIGHT: 16px" class=" "><SPAN style="COLOR: black">
    <asp:Label id="lblgrpname" runat="server" Text="Group Name" Height="16px" 
        Width="93px" __designer:wfdid="w2" Visible="False"></asp:Label></SPAN></TD>
        <TD style="WIDTH: 118px" class="td_cell"><SELECT onblur="GetOtherGrpValueCode()" style="WIDTH: 247px" id="ddlOtherGrpName" class="drpdown" tabIndex=4 runat="server" Visible="false"> <OPTION selected></OPTION></SELECT></TD>
        <TD style="WIDTH: 151px" class="td_cell"></TD><TD style="WIDTH: 137px" class="td_cell"></TD></TR>--%>
        
        
        </TBODY></TABLE>
</contenttemplate>
                </asp:UpdatePanel></td>
        </tr>
 
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
            <td class="td_cell" colspan="4" style="width:100%; color: red;">
                &nbsp;<asp:Button ID="btnExportToExcel" runat="server" CssClass="btn" 
                    TabIndex="8" Text="Export To Excel" />
                </td>
        </tr>
        <tr>
            <td class="td_cell" colspan="4" style="width:100%; color: red">
                <asp:UpdatePanel id="UpdatePanel2" runat="server">
                    <contenttemplate>
<asp:GridView id="gv_SearchResult" tabIndex=10 runat="server" Font-Size="10px" Width="100%" CssClass="grdstyle" GridLines="Vertical" CellPadding="3" BorderWidth="1px" BorderStyle="None" BorderColor="#999999" AutoGenerateColumns="False" AllowSorting="True" AllowPaging="True">
<FooterStyle CssClass="grdfooter"></FooterStyle>
<Columns>

<%--<asp:BoundField DataField="printconf" SortExpression="printconf" HeaderText="Print In Confirmation"></asp:BoundField>--%>
<%--<asp:BoundField DataField="printremarks" SortExpression="printremarks" HeaderText="Print Remarks"></asp:BoundField>--%>
<asp:TemplateField Visible="False" HeaderText="Type Code"><EditItemTemplate>
                                            <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("othtypcode") %>'></asp:TextBox>
                                        
</EditItemTemplate>
<ItemTemplate>
<asp:Label id="lblothtypcode" runat="server" Text='<%# Bind("othtypcode") %>'></asp:Label> 
</ItemTemplate>
</asp:TemplateField>
<asp:BoundField DataField="othtypcode" ItemStyle-Width="8%" SortExpression="othtypcode" HeaderText="Type Code">
<ItemStyle HorizontalAlign="Left"></ItemStyle>

<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
</asp:BoundField>

				                  <asp:TemplateField  HeaderText="Type Name" SortExpression="othtypname">
                    <ItemTemplate>
                        <asp:Label ID="lblTypeName" runat="server" Text='<%# Bind("othtypname") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Left" Width="12%" />
                    <ItemStyle CssClass="othtypname" HorizontalAlign="Left" Width="12%" />
                </asp:TemplateField>
<%--<asp:BoundField DataField="othtypname" SortExpression="othtypname" HeaderText="Type Name">
<ItemStyle HorizontalAlign="Left"></ItemStyle>

<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
</asp:BoundField>
--%>
<asp:BoundField DataField="rankorder" SortExpression="rankorder" visible=false HeaderText=" Display Order">
<ItemStyle HorizontalAlign="Right"></ItemStyle>
<HeaderStyle HorizontalAlign="Right"></HeaderStyle>
</asp:BoundField>
<asp:BoundField DataField="othgrpcode" visible=false SortExpression="othgrpcode" HeaderText="Group Code">
<ItemStyle HorizontalAlign="Left"></ItemStyle>

<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
</asp:BoundField>
<asp:BoundField DataField="othgrpname" SortExpression="othgrpname" HeaderText="Group Name">
<ItemStyle HorizontalAlign="Left"></ItemStyle>

<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
</asp:BoundField>

<asp:BoundField DataField="minpax" SortExpression="minpax" HeaderText="Min Pax">
<ItemStyle HorizontalAlign="Right"></ItemStyle>

<HeaderStyle HorizontalAlign="Right"></HeaderStyle>
</asp:BoundField>
<asp:BoundField DataField="maxpax" SortExpression="maxpax" HeaderText="Max Pax">
<ItemStyle HorizontalAlign="Right"></ItemStyle>

<HeaderStyle HorizontalAlign="Right"></HeaderStyle>
</asp:BoundField>
<%--<asp:BoundField DataField="ratebasis" SortExpression="ratebasis" HeaderText="Rate Basis">
<ItemStyle HorizontalAlign="Left"></ItemStyle>

<HeaderStyle HorizontalAlign="Left"></HeaderStyle></asp:BoundField>--%>
		                  <asp:TemplateField  HeaderText="Rate Basis" SortExpression="ratebasis">
                    <ItemTemplate>
                        <asp:Label ID="lblratebasis" runat="server" Text='<%# Bind("ratebasis") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Left" Width="12%" />
                    <ItemStyle CssClass="ratebasis" HorizontalAlign="Left" Width="12%" />
                </asp:TemplateField>
 
			                  <asp:TemplateField  HeaderText="Airport Meet Type" SortExpression="servicetype">
                    <ItemTemplate>
                        <asp:Label ID="lblserviceName" runat="server" Text='<%# Bind("servicetype") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Left" Width="12%" />
                    <ItemStyle CssClass="servicetype" HorizontalAlign="Left" Width="12%" />
                </asp:TemplateField>


<%--<asp:BoundField DataField="partyname" SortExpression="partyname" HeaderText="Preferred Supplier"></asp:BoundField>--%>

		                  <asp:TemplateField  HeaderText="Preferred Supplier" SortExpression="partyname">
                    <ItemTemplate>
                        <asp:Label ID="lblpartyname" runat="server" Text='<%# Bind("partyname") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Left" Width="12%" />
                    <ItemStyle CssClass="partyname" HorizontalAlign="Left" Width="12%" />
                </asp:TemplateField>

<asp:BoundField DataField="autocancelreq" SortExpression="autocancelreq" HeaderText="Auto Cancellation Req."></asp:BoundField>

<asp:BoundField DataField="active" SortExpression="active" HeaderText="Active"></asp:BoundField>
<asp:BoundField HtmlEncode="False" DataFormatString="{0:dd/MM/yyyy hh:mm:ss tt} " DataField="adddate" SortExpression="adddate" HeaderText="Date Created">
<ItemStyle HorizontalAlign="Left"></ItemStyle>

<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
</asp:BoundField>
<asp:BoundField HtmlEncode="False" DataField="adduser" SortExpression="adduser" HeaderText="User Created">
<ItemStyle HorizontalAlign="Left"></ItemStyle>

<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
</asp:BoundField>
<asp:BoundField HtmlEncode="False" DataFormatString="{0:dd/MM/yyyy hh:mm:ss tt} " DataField="moddate" SortExpression="moddate" HeaderText="Date Modified">
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
</asp:GridView> <asp:Label id="lblMsg" runat="server" 
                            Text="Records not found. Please redefine search criteria" Font-Size="8pt" 
                            Font-Names="Verdana" Font-Bold="True" Width="357px" Visible="False" 
                            CssClass="lblmsg"></asp:Label> 
</contenttemplate>
                </asp:UpdatePanel></td>
        </tr>
    </table>

   <asp:ScriptManagerProxy id="ScriptManagerProxy1" runat="server">
        <Services>
            <asp:ServiceReference Path="../clsSectorServices.asmx" />
              <asp:ServiceReference Path="~/clsServices.asmx" />

        </Services>
    </asp:ScriptManagerProxy>
    </asp:Content>




