
<%@ Page Language="VB" MasterPageFile="~/MainPageMaster.master" AutoEventWireup="false" EnableEventValidation="false"  CodeFile="SectorSearch.aspx.vb" Inherits="SectorSearch"  %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
      <%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
    <%@ OutputCache location="none" %> 
<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">
      <meta http-equiv="content-type" content="text/html;charset=UTF-8" />
  <meta http-equiv="X-UA-Compatible" content="chrome=1">

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

         <script language="javascript" type="text/javascript">

             $("body").on("click", "[id*=gv_SearchResult] .Update", function () {
                 var row = $(this).closest("tr");
                 var secCode = row.find(".cssCode1").find("span").html();
                 window.open('Sector.aspx?State=Edit&RefCode=' + secCode.trim());
             });

             $("body").on("click", "[id*=gv_SearchResult] .View", function () {
                 var row = $(this).closest("tr");
                 var secCode = row.find(".cssCode1").find("span").html();
                 window.open('Sector.aspx?State=View&RefCode=' + secCode.trim());
             });

             $("body").on("click", "[id*=gv_SearchResult] .Delete", function () {
                 var row = $(this).closest("tr");
                 var secCode = row.find(".cssCode1").find("span").html();
                 window.open('Sector.aspx?State=Delete&RefCode=' + secCode.trim());
             });

    </script>
  <script type="text/javascript" charset="utf-8">

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
          $txtvsprocess.val('Country:" " City:" " Sector:" " SectorGroup:" " TEXT:" "');
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
                          case 'City':
                              var asSqlqry = '';
                              glcallback = callback;
            
                              ColServices.clsSectorServices.GetListOfArrayCityVisualSearch(asSqlqry, fnFillSearchVS, ErrorHandler, TimeOutHandler);
                              break;
                          case 'Country':
                              var asSqlqry = '';
                              glcallback = callback;
                              ColServices.clsSectorServices.GetListOfArrayCountryVisualSearch(asSqlqry, fnFillSearchVS, ErrorHandler, TimeOutHandler);
                              break;
                          case 'Sector':
                              var asSqlqry = '';
                              glcallback = callback;
                              ColServices.clsSectorServices.GetListOfArraySectorVisualSearch(asSqlqry, fnFillSearchVS, ErrorHandler, TimeOutHandler);
                              break;
                          case 'SectorGroup':
                              var vhdDefault_Group = document.getElementById("<%=hdDefault_Group.ClientID%>");
                              var vDefault_Group = vhdDefault_Group.value
                              vDefault_Group = "'" + vDefault_Group + "'";
                              var asSqlqry = '';
                              glcallback = callback;
                              ColServices.clsSectorServices.GetListOfArraySectorGroupVisualSearch(vDefault_Group, fnFillSearchVS, ErrorHandler, TimeOutHandler);
                              break;
                      }
                  },
                  facetMatches: function (callback) {
                      callback([
                { label: 'TEXT', category: 'Sector' },
                { label: 'City', category: 'Sector' },
                { label: 'Country', category: 'Sector' },
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


    <script language="javascript" type="text/javascript">
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

        


    </script>


    <script language="javascript" type="text/javascript">

    function CallWebMethod(methodType)
    {
        switch(methodType) {
       
            case "othtypcode":
                var select = document.getElementById("<%=ddlSelectGroupCode.ClientID%>");
                var codeid = select.options[select.selectedIndex].text;
                var selectname = document.getElementById("<%=ddlSelectGroupName.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;

                var connstr = document.getElementById("<%=txtconnection.ClientID%>");
                constr = connstr.value

               

                break;
            case "othtypname":
                var select = document.getElementById("<%=ddlSelectGroupName.ClientID%>");
                var codeid = select.options[select.selectedIndex].value;
                var selectname = document.getElementById("<%=ddlSelectGroupCode.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;
                var connstr = document.getElementById("<%=txtconnection.ClientID%>");
                constr = connstr.value
                
               
                break;

            case "ctrycode":
                var select = document.getElementById("<%=ddlCountryCode.ClientID%>");
                var codeid = select.options[select.selectedIndex].text;
                var selectname = document.getElementById("<%=ddlCountryName.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;
                var connstr = document.getElementById("<%=txtconnection.ClientID%>");
                constr = connstr.value
                
                ColServices.clsServices.GetCityCodeListnew(constr, codeid, FillCityCodes, ErrorHandler, TimeOutHandler);
                ColServices.clsServices.GetCityNameListnew(constr, codeid, FillCityNames, ErrorHandler, TimeOutHandler);
                break;
            case "ctryname":
                var select=document.getElementById("<%=ddlCountryName.ClientID%>");
                var codeid=select.options[select.selectedIndex].value;
                var selectname=document.getElementById("<%=ddlCountryCode.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;
                var connstr=document.getElementById("<%=txtconnection.ClientID%>");  
                constr=connstr.value   
                
                ColServices.clsServices.GetCityCodeListnew(constr,codeid,FillCityCodes,ErrorHandler,TimeOutHandler);
                ColServices.clsServices.GetCityNameListnew(constr,codeid,FillCityNames,ErrorHandler,TimeOutHandler);
                break;
            case "citycode":
                var select=document.getElementById("<%=ddlCityCode.ClientID%>");
                var selectname=document.getElementById("<%=ddlCityName.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;
            case "cityname":
                var select=document.getElementById("<%=ddlCityName.ClientID%>");
                var selectname=document.getElementById("<%=ddlCityCode.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;
        }
    }

    function FillCityCodes(result) {
   
       	var ddl = document.getElementById("<%=ddlCityCode.ClientID%>");    	
 		RemoveAll(ddl) 		
        for(var i=0;i<result.length;i++)
        {
       
            var option=new Option(result[i].ListText,result[i].ListValue);
            ddl.options.add(option);
        }
        ddl.value="[Select]";
    }

    function FillCityNames(result)
    {
    	var ddl = document.getElementById("<%=ddlCityName.ClientID%>");
 		RemoveAll(ddl)
        for(var i=0;i<result.length;i++)
        {
            var option=new Option(result[i].ListText,result[i].ListValue);
            ddl.options.add(option);
        }
        ddl.value="[Select]";
    }


    function TimeOutHandler(result)
    {
        alert("Timeout :" + result);
    }

    function ErrorHandler(result)
    {
        var msg=result.get_exceptionType() + "\r\n";
        msg += result.get_message() + "\r\n";
        msg += result.get_stackTrace();
        alert(msg);
    }

</script>

    <table style="border-right: gray 2px solid; border-top: gray 2px solid; border-left: gray 2px solid; border-bottom: gray 2px solid;width: 100%"">
        <tr>
            <td style="width: 100%">
                <table width="100%">
                    <tr>
                        <td class="field_heading" style="width: 100%" align="center">
                Sector List</td>
                    </tr>
                    <tr>
                        <td class="td_cell" style="width: 100%; color: blue; " align="center">
                Type few characters of code or name and click search 
                        </td>
                    </tr>
                    <tr>
                        <td>
                        <asp:UpdatePanel id="UpdatePanel1" runat="server">
                    <contenttemplate>
<TABLE width="100%"><TBODY><TR><TD align=center colSpan=6>
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp; &nbsp;&nbsp; &nbsp;<asp:Button id="btnhelp" 
        tabIndex=3 onclick="btnhelp_Click" runat="server" Text="Help" 
        Font-Bold="False" CssClass="search_button"></asp:Button> &nbsp;&nbsp;<asp:Button id="btnAddNew" tabIndex=4 runat="server" Text="Add New" 
        Font-Bold="False" CssClass="btn"></asp:Button>&nbsp;&nbsp;<asp:Button id="btnPrint" tabIndex=5 runat="server" Text="Report" 
        CssClass="btn"></asp:Button></TD></TR>
        <TR style="display:none;">
        <TD style="WIDTH: 80px"><asp:Label id="Label2" runat="server" Text="Sector Code" ForeColor="Black" Width="84px" CssClass="field_caption"></asp:Label></TD>
        <TD style="WIDTH: 100px"><asp:TextBox id="TxtSectorCode" tabIndex=6 runat="server" 
                Width="212px" CssClass="txtbox" MaxLength="20"></asp:TextBox></TD>
        <TD style="WIDTH: 79px"><asp:Label id="Label1" runat="server" Text="Sector Name" ForeColor="Black" Width="84px" CssClass="field_caption"></asp:Label></TD>
        <TD style="WIDTH: 100px"><asp:TextBox id="TxtSecorName" tabIndex=7 runat="server" 
                Width="300px"></asp:TextBox></TD>
        <TD style="WIDTH: 178px"><asp:Label id="Label3" runat="server" Text="Order By" Width="50px" CssClass="field_caption"></asp:Label></TD>
        <TD style="WIDTH: 178px"><asp:DropDownList id="ddlOrderBy" runat="server" 
                Width="104px" CssClass="drpdown" AutoPostBack="True" 
                OnSelectedIndexChanged="ddlOrderBy_SelectedIndexChanged" TabIndex="8"></asp:DropDownList></TD>
        </TR>
        <TR style="display:none;"><TD style="WIDTH: 80px"><asp:Label id="lblsectorCode" runat="server" Text="Sector Group Code" ForeColor="Black" Width="76px" CssClass="field_caption" Visible="true"></asp:Label></TD>
        <TD style="WIDTH: 100px">
        <SELECT style="WIDTH: 219px" id="ddlSelectGroupCode" class="drpdown" tabIndex=9 
                onchange="CallWebMethod('othtypcode');" runat="server" visible="true"> <OPTION selected></OPTION></SELECT> </TD><TD style="WIDTH: 79px"><asp:Label id="lblSectorGroupname" runat="server" Text="Sector Group Name" ForeColor="Black" Width="72px" CssClass="field_caption" Visible="True"></asp:Label></TD>
        <TD style="WIDTH: 100px"><SELECT style="WIDTH: 306px" id="ddlSelectGroupName" 
                class="drpdown" tabIndex=10 onchange="CallWebMethod('othtypname');" 
                runat="server" visible="true"> <OPTION selected></OPTION></SELECT> </TD>
            <TD style="WIDTH: 178px">
            <asp:Button ID="btnSearch" runat="server" CssClass="search_button" 
                Font-Bold="False" tabIndex="1" Text="Search" />
            </TD>
        <TD style="WIDTH: 178px">
                <asp:RadioButton ID="rbtnsearch" runat="server" AutoPostBack="True" 
                    Checked="True" CssClass="td_cell" ForeColor="Black" GroupName="GrSearch" 
                    OnCheckedChanged="rbtnsearch_CheckedChanged" Text="Search" />
            </TD>
        </TR>
         


        
        <TR style="display:none;"><TD style="WIDTH: 80px"><asp:Label id="lblCtryCode" runat="server" Text="Country Code" ForeColor="Black" Width="76px" CssClass="field_caption" Visible="False"></asp:Label></TD>
        <TD style="WIDTH: 100px">
        <SELECT style="WIDTH: 219px" id="ddlCountryCode" class="drpdown" tabIndex=11 
                onchange="CallWebMethod('ctrycode');" runat="server" visible="false"> <OPTION selected></OPTION></SELECT> </TD><TD style="WIDTH: 79px"><asp:Label id="lblCtryName" runat="server" Text="Country Name" ForeColor="Black" Width="72px" CssClass="field_caption" Visible="False"></asp:Label></TD>
        <TD style="WIDTH: 100px"><SELECT style="WIDTH: 306px" id="ddlCountryName" 
                class="drpdown" tabIndex=12 onchange="CallWebMethod('ctryname');" 
                runat="server" visible="false"> <OPTION selected></OPTION></SELECT> </TD>
            <TD style="WIDTH: 178px">
            <asp:Button ID="btnClear" runat="server" CssClass="search_button" 
                Font-Bold="False" tabIndex="2" Text="Clear" />
            </TD>
        <TD style="WIDTH: 178px">
                <asp:RadioButton ID="rbtnadsearch" runat="server" AutoPostBack="True" 
                    CssClass="td_cell" ForeColor="Black" GroupName="GrSearch" 
                    OnCheckedChanged="rbtnadsearch_CheckedChanged" Text="Advance Search" />
            </TD>
        </TR>

        <TR style="display:none;"><TD style="WIDTH: 80px"><asp:Label id="lblCityCode" runat="server" Text="City Code  " ForeColor="Black" Width="48px" CssClass="field_caption" Visible="False"></asp:Label></TD><TD style="WIDTH: 100px">
            <SELECT style="WIDTH: 219px" id="ddlCityCode" class="drpdown" tabIndex=13 
                onchange="CallWebMethod('citycode');" runat="server" visible="false" 
                onserverchange="ddlCityCode_ServerChange"> <OPTION selected></OPTION></SELECT> </TD><TD style="WIDTH: 79px"><asp:Label id="lblCityName" runat="server" Text="City Name" ForeColor="Black" Width="60px" CssClass="field_caption" Visible="False"></asp:Label></TD><TD style="WIDTH: 100px">
            <SELECT style="WIDTH: 306px" id="ddlCityName" class="drpdown" tabIndex=14 
                onchange="CallWebMethod('cityname');" runat="server" visible="false"> <OPTION selected></OPTION></SELECT> </TD><TD style="WIDTH: 178px">
    <input id="txtconnection" runat="server" style="visibility: hidden; width: 12px;
        height: 9px" type="text" /></TD><TD style="WIDTH: 178px">
            </TD></TR>
    <tr>
        <td style="WIDTH: 80px">
            &nbsp;</td>
        <td style="WIDTH: 100px">
            &nbsp;</td>
        <td style="WIDTH: 79px">
            &nbsp;</td>
        <td style="WIDTH: 100px">
            &nbsp;</td>
        <td style="WIDTH: 178px">
            &nbsp;</td>
        <td style="WIDTH: 178px">
            &nbsp;</td>
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
    CssClass="btn" Font-Bold="False" 
                            tabIndex="4" Text="Reset Search" /></div></div>
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
                            </td>
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
                            TooltipMessage="Input a Date in Date/Month/Year">
                        </cc1:MaskedEditValidator>
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
                            TooltipMessage="Input a Date in Date/Month/Year">
                        </cc1:MaskedEditValidator>
                    </td>
                    <td>
                        <asp:Button ID="btnFilter" runat="server" CssClass="btn" Font-Bold="False" 
                            tabIndex="4" Text="Search by Date" />
                        &nbsp;<asp:Button ID="btnClearDate" runat="server" CssClass="btn" Font-Bold="False" 
                            tabIndex="4" Text="Reset Dates" />
                    </td>
                    
                               <td>  <asp:Label ID="RowSelectss" runat="server" CssClass="field_caption" 
                            Text="Rows Selected "></asp:Label>
                     <asp:DropDownList ID="RowsPerPageSS" runat="server" AutoPostBack="true">
                   
                          <asp:ListItem Value="5">5</asp:ListItem>
                            <asp:ListItem Value="10">10</asp:ListItem>
                             <asp:ListItem Value="15">15</asp:ListItem>
                            <asp:ListItem Value="20">20</asp:ListItem>
                             <asp:ListItem Value="25">25</asp:ListItem>
                            <asp:ListItem Value="30">30</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                
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
            </table>
        </td>
    </tr>
    </TBODY></TABLE>
</contenttemplate>                
                </asp:UpdatePanel></td>                        
                    </tr>
                    <tr>
                        <td style="height: 15px;width:100%">
                            <asp:Button ID="btnExportToExcel" runat="server" CssClass="btn" TabIndex="15"
                                            Text="Export To Excel" />
                                        <asp:HiddenField ID="hdDefault_Group" runat="server" />
                                        <asp:HiddenField ID="hdDefaultValue" runat="server" />
                                        <asp:HiddenField ID="hdDefaultValueText" runat="server" />
                                        </td>
                    </tr>
                    <tr>
                        <td style="width: 100%">
                <asp:UpdatePanel id="UpdatePanel2" runat="server">
                    <contenttemplate>
<asp:GridView id="gv_SearchResult" tabIndex=16 runat="server" Font-Size="10px" Width="100%" CssClass="grdstyle" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False" BorderColor="#999999" BorderStyle="None" BorderWidth="1px" CellPadding="3" GridLines="Vertical">
<FooterStyle CssClass="grdfooter"></FooterStyle>
<Columns>
<asp:TemplateField Visible="False" HeaderText="Sector Code"><EditItemTemplate>
                                            <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("sectorcode") %>'></asp:TextBox>
                                        
</EditItemTemplate>
<ItemTemplate>
<asp:Label id="lblSectorCode" runat="server" Text='<%# Bind("sectorcode") %>'></asp:Label> 
</ItemTemplate>
</asp:TemplateField>
<asp:BoundField DataField="sectorcode" SortExpression="sectorcode" HeaderText="Sector Code">
<ItemStyle HorizontalAlign="Left"></ItemStyle>

<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
</asp:BoundField>
  <asp:TemplateField HeaderText="Sector Name" SortExpression="sectorname">
                    <ItemTemplate>
                        <asp:Label ID="lblSectorName" runat="server" Text='<%# Bind("sectorname") %>'></asp:Label>
                    </ItemTemplate>
                    <ControlStyle Width="8%" />
                    <HeaderStyle HorizontalAlign="Left" Width="8%" />
                    <ItemStyle CssClass="sectorname" HorizontalAlign="Left" Width="8%" />
                </asp:TemplateField>
                <asp:BoundField Visible="false" DataField="sectorgroupcode" HeaderText="Sector Group Code"  ItemStyle-CssClass="sectorgroupcode"
                    HtmlEncode="False" SortExpression="sectorgroupcode" >
                    <ItemStyle HorizontalAlign="Left" Width="6%" />
                    <HeaderStyle HorizontalAlign="Left" Width="6%" />
                </asp:BoundField>
                <asp:TemplateField HeaderText="Sector Group Name" SortExpression="othtypname">
                    <ItemTemplate>
                        <asp:Label ID="lblSectorGroupName" runat="server" Text='<%# Bind("othtypname") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Left" Width="12%" />
                    <ItemStyle CssClass="othtypname" HorizontalAlign="Left" Width="12%" />
                </asp:TemplateField>
                <asp:BoundField Visible="false" DataField="ctrycode" HeaderText="Country Code"  ItemStyle-CssClass="ctrycode"
                    SortExpression="ctrycode">
                    <ItemStyle HorizontalAlign="Left" Width="4%" />
                    <HeaderStyle HorizontalAlign="Left" Width="4%" />
                </asp:BoundField>
                <asp:TemplateField visible="false" HeaderText="Country Name" SortExpression="ctryname">
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
                    <HeaderStyle HorizontalAlign="Left" Width="7%" />
                    <ItemStyle CssClass="cityname" HorizontalAlign="Left" Width="7%" />
                </asp:TemplateField>
                <asp:BoundField DataField="IsActive" HeaderText="Active"  ItemStyle-CssClass="IsActive"
                    SortExpression="IsActive">
                    <ItemStyle HorizontalAlign="Left" Width="7%" />
                    <HeaderStyle HorizontalAlign="Left" Width="7%" />
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

<HeaderStyle CssClass="grdheader" ForeColor="White"></HeaderStyle>

<AlternatingRowStyle CssClass="grdAternaterow"></AlternatingRowStyle>
</asp:GridView> <asp:Label id="lblMsg" runat="server" 
                            Text="Records not found. Please redefine search criteria" Font-Size="8pt" 
                            Font-Names="Verdana" Font-Bold="True" Width="357px" Visible="False" 
                            CssClass="lblmsg"></asp:Label> 
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