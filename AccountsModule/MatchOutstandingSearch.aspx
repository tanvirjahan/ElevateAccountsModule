<%@ Page Language="VB" AutoEventWireup="false" CodeFile="MatchOutstandingSearch.aspx.vb" Inherits="MatchOutstandingSearch" MasterPageFile="~/AccountsMaster.master" Strict="true"  %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
 <%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
    <%@ OutputCache location="none" %> 
    
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<%@ Register Assembly="EclipseWebSolutions.DatePicker" Namespace="EclipseWebSolutions.DatePicker"
    TagPrefix="ews" %>
    
<asp:Content ID="Content2" ContentPlaceHolderID="Main" Runat="Server">

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
        function FormValidation(state) {



            if ((document.getElementById("<%=txtFromDate.ClientID%>").value == '') || (document.getElementById("<%=txtToDate.ClientID%>").value == '')) {

                alert("Select Dates ");
                return false;
            }





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

            var vdivCode = document.getElementById("vdivcode");
            var $txtvsprocess = $(document).find('.cs_txtvsprocess');

            $txtvsprocess.val('MOSNO:" " CUSTOMER:" " SUPPLIER:" " SUPPLIERAGENT:"  " STATUS:" " NARRATION:"  " TEXT:" "');

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
                          case 'MOSNO':
                              var asSqlqry = "select  ltrim(rtrim(tran_id)) from  matchos_master where div_id='" + vdivCode.value + "' order by tran_id ";
                             glcallback = callback;
                             ColServices.clsServices.GetListOfArrayVisualSearch(asSqlqry, fnFillSearchVS, ErrorHandler, TimeOutHandler);
                             break;
                         case 'CUSTOMER':

                             var asSqlqry = " select des FROM view_account WHERE TYPE='C' and div_code='" + vdivCode.value + "' order by des";
                             glcallback = callback;
                             ColServices.clsServices.GetListOfArrayVisualSearch(asSqlqry, fnFillSearchVS, ErrorHandler, TimeOutHandler);
                             break;
                         case 'SUPPLIER':
                             var asSqlqry = " select des FROM view_account WHERE TYPE='S' and div_code='" + vdivCode.value + "' order by des";
                             glcallback = callback;
                             ColServices.clsServices.GetListOfArrayVisualSearch(asSqlqry, fnFillSearchVS, ErrorHandler, TimeOutHandler);
                             break;
                         case 'SUPPLIERAGENT':
                             var asSqlqry = " select des FROM view_account WHERE TYPE='A' and div_code='" + vdivCode.value + "' order by des";
                             glcallback = callback;
                             ColServices.clsServices.GetListOfArrayVisualSearch(asSqlqry, fnFillSearchVS, ErrorHandler, TimeOutHandler);
                             break;
                         case 'STATUS':
                             var asSqlqry = " select ltrim(rtrim('Posted')) status  union all select ltrim(rtrim('UnPosted')) status ";
                             glcallback = callback;
                             ColServices.clsSectorServices.GetListOfArrayVisualSearch(asSqlqry, fnFillSearchVS, ErrorHandler, TimeOutHandler);
                             break;

                         case 'NARRATION':
                             var asSqlqry = "select  distinct narration from  matchos_master where div_id='" + vdivCode.value + "' order by narration ";
                             glcallback = callback;
                             ColServices.clsServices.GetListOfArrayVisualSearch(asSqlqry, fnFillSearchVS, ErrorHandler, TimeOutHandler);
                             break;

                        }
                    },
                    facetMatches: function (callback) {
                        callback([
                { label: 'MOSNO', category: ' MatchOutstanding' },
                { label: 'CUSTOMER', category: 'MatchOutstanding' },
                 { label: 'SUPPLIER', category: 'MatchOutstanding' },
                { label: 'SUPPLIERAGENT', category: 'MatchOutstanding' },
                 { label: 'STATUS', category: 'MatchOutstanding' },
                 { label: 'NARRATION', category: 'MatchOutstanding' },
                { label: 'TEXT', category: 'MatchOutstanding' },


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
    

    
    <script language ="javascript" type ="text/javascript" >
        var ddlcustcode = null;
        var ddlcustname = null;
        function FillCodeName(ddltp, ddlcode, ddlname) {
            ddltyp = document.getElementById(ddltp);
            ddlc = document.getElementById(ddlcode);
            ddln = document.getElementById(ddlname);

            var txtcustcode = document.getElementById("<%=txtcustno.ClientID%>");
            var txtcustname = document.getElementById("<%=txtcustname.ClientID%>");

            if (ddltyp.value != '[Select]') {
                ddln.value = ddlc.options[ddlc.selectedIndex].text;
            }
            else {
                alert('Please Select Type');
                ddlc.value = '[Select]';
                ddln.value = '[Select]';
            }

            txtcustcode.value = ddlc.value
            txtcustname.value = ddln.value


        }
        function FillCustDDL(ddltp, ddlcustcd, ddlcustnm, lblcustcd, lblcustnm) {

            ddltyp = document.getElementById(ddltp);
            lblcustcode = document.getElementById(lblcustcd);
            lblcustname = document.getElementById(lblcustnm);

            ddlcustcode = document.getElementById(ddlcustcd);
            ddlcustname = document.getElementById(ddlcustnm);

            var strtp = ddltyp.value;
            var strcap = ddltyp.options[ddltyp.selectedIndex].text;
            var sqlstr1 = null;
            var sqlstr2 = null;

            var divcode = document.getElementById("<%=txtDivcode.ClientId%>");

            if (ddltyp.value != '[Select]') {

                lblcustcode.innerHTML = strcap + 'Code';
                lblcustname.innerHTML = strcap + 'Name';
                sqlstr1 = "select Code,des from view_account where div_code='" + divcode.value + "' and type = '" + strtp + "' order by code";
                sqlstr2 = "select des,Code from view_account where  div_code='" + divcode.value + "' and type = '" + strtp + "' order by des";
            }
            else {
                lblcustcode.innerHTML = 'Code';
                lblcustname.innerHTML = 'Name';
                sqlstr1 = "select top 10  Code,des from view_account where div_code='" + divcode.value + "' and order by code";
                sqlstr2 = "select top 10 des,Code from view_account where div_code='" + divcode.value + "' and order by des";
            }
            var connstr = document.getElementById("<%=txtconnection.ClientID%>");
            constr = connstr.value

            ColServices.clsServices.GetQueryReturnStringListnew(constr, sqlstr1, FillCustCodes, ErrorHandler, TimeOutHandler);
            ColServices.clsServices.GetQueryReturnStringListnew(constr, sqlstr2, FillCustNames, ErrorHandler, TimeOutHandler);

        }

        function FillCustCodes(result) {
            var ddl = ddlcustcode;

            RemoveAll(ddl)
            for (var i = 0; i < result.length; i++) {
                var option = new Option(result[i].ListText, result[i].ListValue);
                ddl.options.add(option);
            }
            ddl.value = "[Select]";
        }

        function FillCustNames(result) {
            var ddl = ddlcustname;

            RemoveAll(ddl)
            for (var i = 0; i < result.length; i++) {
                var option = new Option(result[i].ListText, result[i].ListValue);
                ddl.options.add(option);
            }
            ddl.value = "[Select]";
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

</script>

    <table style="border-right: gray 2px solid; border-top: gray 2px solid; border-left: gray 2px solid;
         border-bottom: gray 2px solid" width=100%>
        <tr>
            <td align="center" class="field_heading" colspan="4">
                &nbsp;Match Outstanding List</td>
        </tr>
        <tr>
            <td align="center" class="td_cell" colspan="4" style="color: blue; height: 15px">
                Type few characters of code or name and click search &nbsp; &nbsp;</td>
        </tr>
       <tr>
            <td class="td_cell" colspan="4" >
                <asp:UpdatePanel id="UpdatePanel1" runat="server">
                    <contenttemplate>
<TABLE width=100%><TBODY>

                           <TR>
<TD align=center>
    <asp:Button id="btnhelp" 
        tabIndex=3 onclick="btnhelp_Click" runat="server" Text="Help" 
        Font-Bold="False" CssClass="btn"></asp:Button> &nbsp;&nbsp;<asp:Button id="btnAddNew" tabIndex=4 runat="server" Text="Add New" 
        Font-Bold="False" CssClass="btn"></asp:Button>&nbsp;&nbsp;<asp:Button 
        ID="btnPrint_new" TabIndex="5" runat="server" Text="Report" CssClass="btn"></asp:Button>
        
        <asp:DropDownList ID="ddlrpt" runat="server">
            <asp:ListItem Value="Brief">Brief Report</asp:ListItem>
            <asp:ListItem Value="Detailed">Detailed Report</asp:ListItem>
    </asp:DropDownList>
        
        </TD>
        </TR>
		                          
              

<TR><TD style="display:none"> <asp:RadioButton id="rbtnsearch" runat="server" Text="Search" ForeColor="Black" CssClass="td_cell" OnCheckedChanged="rbtnsearch_CheckedChanged" Checked="True" GroupName="GrSearch" AutoPostBack="True"></asp:RadioButton>&nbsp; <asp:RadioButton id="rbtnadsearch" runat="server" Text="Advance Search" ForeColor="Black" CssClass="td_cell" OnCheckedChanged="rbtnadsearch_CheckedChanged" GroupName="GrSearch" AutoPostBack="True"></asp:RadioButton>&nbsp; 
    <asp:Button id="btnSearch" tabIndex=10 onclick="btnSearch_Click" runat="server" 
        Text="Search" Font-Bold="False" CssClass="search_button"></asp:Button>&nbsp; 
    <asp:Button id="btnClear" tabIndex=11 onclick="btnClear_Click" runat="server" 
        Text="Clear" Font-Bold="False" CssClass="search_button"></asp:Button>&nbsp; 
        </td></tr>

   
        
        
        <TR>
        <TD  style="display:none"><SPAN class="td_cell">&nbsp;<asp:Label id="Label1" runat="server" Text=" Document No." Width="110px" CssClass="field_caption"></asp:Label></SPAN>
        <INPUT style="WIDTH: 194px" id="txtDocNo" class="txtbox" tabIndex=1 type=text 
            runat="server" /><SPAN><asp:Label id="Label2" runat="server" Text="Doc Type" Width="110px" CssClass="field_caption"></asp:Label></SPAN>
        <SELECT style="WIDTH: 200px" id="ddlDocType" class="dropdown" runat="server"> <OPTION value="[Select]" selected>[Select]</OPTION><OPTION value="MOS">MOS</OPTION></SELECT></TD></TR>
        
        
      
    <asp:Panel id="pnlSearch" runat="server" Width="738px" Visible="False"><TABLE >
                <TBODY>
        <TR><TD style="display:none">
            <asp:Label ID="lblType" runat="server" CssClass="field_caption" 
        Text="Type" Width="110px"></asp:Label>
          <select id="ddlType" runat="server" class="dropdown" 
        style="WIDTH: 200px" tabindex="2"> 
                <option selected="" value="[Select]">[Select]</option>
         
                <option value="C">Customer</option>
         <option value="S">Supplier</option>
         <option value="A">Supplier Agent</option>
    </select>
    </TD><td  style="display:none">
          <asp:Label ID="Label3" runat="server" 
        CssClass="field_caption" Text="Status" Width="110px"></asp:Label>
           
                <SELECT style="WIDTH: 200px" id="ddlStatus" class="dropdown" 
        tabIndex=3 runat="server"> <OPTION selected value="[Select]">[Select]</OPTION> 
    <option value="P">Posted</option>
     <option value="U">UnPosted</option>
     </SELECT></TD></TR>

        <TR><TD  style="display:none">
            <span class="td_cell"><asp:Label ID="lblCustCode" runat="server" 
        CssClass="field_caption" Text="Code" Width="110px"></asp:Label></span></TD><TD style="display:none">
                <select id="ddlCustomer" runat="server" class="dropdown" 
        style="WIDTH: 200px" tabindex="4"> 
                    <option selected=""></option>
    </select>
    </TD><TD  style="display:none" ><span>
                <asp:Label ID="lblCustName" runat="server" CssClass="field_caption" 
        Text="Name" Width="110px"></asp:Label></span></TD>
            <TD  style="display:none">
                <SELECT style="WIDTH: 300px" id="ddlCustomerName" class="dropdown" 
        tabIndex=5 runat="server"> <OPTION selected></OPTION></SELECT></TD></TR>
        <TR><TD  style="display:none">
            <asp:Label ID="Label5" runat="server" CssClass="field_caption" 
        Text="From Date" Width="110px"></asp:Label></TD>
            <TD  style="display:none"><ews:DatePicker ID="dpFromDate" runat="server" 
        dateformatstring="dd/MM/yyyy" 
        regexerrormessage="Please enter a date in the format: dd/mm/yyyy" tabIndex="6" 
        width="185px"></ews:DatePicker></TD>
            <TD  style="display:none"><asp:Label ID="Label6" runat="server" 
        CssClass="field_caption" Text="To Date" Width="110px"></asp:Label></TD>
            <TD style="display:none"><ews:DatePicker ID="dpToDate" runat="server" 
        dateformatstring="dd/MM/yyyy" 
        regexerrormessage="Please enter a date in the format: dd/mm/yyyy" tabIndex="7" 
        width="185px"></ews:DatePicker></TD></TR>
        <TR><TD  style="display:none" >
            <asp:Label ID="Label7" runat="server" CssClass="field_caption" 
        Text="From Amount" Width="110px"></asp:Label></TD>
            <TD style="display:none" > 
                <input id="txtFromAmount" runat="server" class="txtbox" 
        style="WIDTH: 194px" tabindex="8" type="text" /></TD><TD class="td_cell"><asp:Label 
        ID="Label8" runat="server" CssClass="field_caption" Text="To Amount" 
        Width="110px"></asp:Label></TD>
            <TD  style="display:none">
                <input id="txtToAmount" runat="server" class="txtbox" 
        style="WIDTH: 194px" tabindex="9" type="text" /></TD></TR>
                    <tr>
                        <td  style="display:none"">
                            &nbsp;</td>
                        <td>
                            <INPUT style="WIDTH: 106px; visibility: hidden;" id="txtcustno" class="txtbox" tabIndex=1 type=text 
            runat="server" />
                        </td>
                        <td  style="display:none">
                            <INPUT style="WIDTH: 50; visibility: hidden;" id="txtcustname" class="txtbox" tabIndex=1 type=text 
            runat="server" />
                        </td>
                        <td  style="display:none">
                            &nbsp;</td>
                    </tr>
                </TBODY></TABLE>
    <input id="txtconnection" runat="server" style="visibility: hidden; width: 12px;
        height: 9px" type="text" />
          <input style="visibility: hidden; width: 29px" id="txtDivcode" type="text" maxlength="20"
                                                                          runat="server" />
        </asp:Panel></TBODY>
    <caption>
        &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; 
        &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; 
        &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;&nbsp;</TD></TR></TBODY></caption>
                        </TABLE>
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
                        <asp:Label ID="Label4" runat="server" CssClass="field_caption" 
                            Text="Filter By "></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlOrder" runat="server">
                            <asp:ListItem Value="C">Created Date</asp:ListItem>
                            <asp:ListItem Value="M">Modified Date</asp:ListItem>
                            <asp:ListItem Value="T">Transaction Date</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="Label9" runat="server" CssClass="field_caption" Text="From Date"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtFromDate" runat="server" onchange="filltodate(this);" 
                            Width="75px"></asp:TextBox>
                        <asp:ImageButton ID="ImgBtnFrmDt" runat="server" 
                            ImageUrl="~/Images/Calendar_scheduleHS.png" TabIndex="3" />
                        <cc1:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd/MM/yyyy" 
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
                        <asp:Label ID="Label10" runat="server" CssClass="field_caption" Text="To Date"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtToDate" runat="server" onchange="ValidateChkInDate();" 
                            Width="75px"></asp:TextBox>
                        <asp:ImageButton ID="ImgBtnToDt" runat="server" 
                            ImageUrl="~/Images/Calendar_scheduleHS.png" TabIndex="3" />
                        <cc1:CalendarExtender ID="CalendarExtender2" runat="server" Format="dd/MM/yyyy" 
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
            <td class="td_cell" colspan="4">
                &nbsp;</td>
        </tr>
        <tr>
            <td class="td_cell" colspan="4" style="width: 100%; color: red">
                <asp:UpdatePanel id="UpdatePanel2" runat="server">
                    <contenttemplate>



<TABLE style="BORDER-RIGHT: gray 2px solid; BORDER-TOP: gray 2px solid; BORDER-LEFT: gray 2px solid; WIDTH:100%; BORDER-BOTTOM: gray 2px solid"><TBODY><TR><TD align=left><asp:GridView id="gv_SearchResult" tabIndex=16 runat="server" Width="100%" CssClass="grdstyle" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False"  CellPadding="3" GridLines="Vertical">
<Columns>
<asp:TemplateField Visible="False" HeaderText="Doc No"><EditItemTemplate>
                                            <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("tran_id") %>'></asp:TextBox>
                                        
</EditItemTemplate>
<ItemTemplate>
                                            <asp:Label ID="lblCode" runat="server" Text='<%# Bind("tran_id") %>'></asp:Label>
                                        
</ItemTemplate>
</asp:TemplateField>
<asp:BoundField DataField="tran_id" SortExpression="tran_id" HeaderText="MOS No"></asp:BoundField>
<asp:TemplateField HeaderText="MOS Type" SortExpression="tran_type">
<ItemTemplate>
    <asp:Label ID="lblTrantype" runat="server" Text='<%# Bind("tran_type") %>'></asp:Label>                                        
</ItemTemplate>
</asp:TemplateField>
<asp:BoundField DataField="post_state" HeaderText="Status"></asp:BoundField>
<asp:BoundField HtmlEncode="False" DataFormatString="{0:dd/MM/yyyy}" DataField="tran_date" SortExpression="tran_date" HeaderText="Date"></asp:BoundField>
<asp:BoundField DataField="acc_code" SortExpression="acc_code" HeaderText="A/C Code"></asp:BoundField>
<asp:BoundField DataField="des" SortExpression="des" HeaderText="A/C Name"></asp:BoundField>
<asp:BoundField HtmlEncode="False" DataFormatString="{0:00.00}" DataField="amount" SortExpression="amount" HeaderText="Amount"></asp:BoundField>
<asp:BoundField DataField="narration" SortExpression="narration" HeaderText="Narration"></asp:BoundField>
<asp:BoundField HtmlEncode="False" DataFormatString="{00:dd/MM/yyyy hh:mm:ss tt}" DataField="adddate" SortExpression="adddate" HeaderText="Date Created"></asp:BoundField>
<asp:BoundField DataField="adduser" SortExpression="adduser" HeaderText="User Created"></asp:BoundField>
<asp:BoundField HtmlEncode="False" DataFormatString="{00:dd/MM/yyyy hh:mm:ss tt}" DataField="moddate" SortExpression="moddate" HeaderText="Date Modified"></asp:BoundField>
<asp:BoundField DataField="moduser" SortExpression="moduser" HeaderText="User Modified"></asp:BoundField>
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
<asp:ButtonField HeaderText="Action" Text="View Log" CommandName="ViewLog">
<ItemStyle ForeColor="Blue"></ItemStyle>
</asp:ButtonField>
</Columns>

 <FooterStyle CssClass="grdfooter" />

<RowStyle CssClass="grdRowstyle"></RowStyle>
<SelectedRowStyle CssClass="grdselectrowstyle" ></SelectedRowStyle>
<PagerStyle CssClass="grdpagerstyle" HorizontalAlign="Center"></PagerStyle>
<HeaderStyle  CssClass="grdheader" ForeColor="white"></HeaderStyle>
<AlternatingRowStyle CssClass="grdAternaterow"></AlternatingRowStyle>
</asp:GridView>


</TD></TR><TR><TD align="left"><asp:Label id="lblMsg" runat="server" 
            Text="Records not found, Please redefine search criteria" Font-Size="8pt" 
            Font-Names="Verdana" Font-Bold="True" Width="357px" Visible="False" 
            CssClass="lblmsg"></asp:Label></TD></TR></TBODY></TABLE>
</contenttemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
   <asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server">
        <Services>
            <asp:ServiceReference Path="../clsSectorServices.asmx" />
            <asp:ServiceReference Path="~/clsServices.asmx" />
        </Services>
    </asp:ScriptManagerProxy>
</asp:Content>
