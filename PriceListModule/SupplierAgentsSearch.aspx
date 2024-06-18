<%@ Page Language="VB" MasterPageFile="~/MainPageMaster.master" AutoEventWireup="false" CodeFile="SupplierAgentsSearch.aspx.vb" Inherits="SupplierAgentsSearch"    EnableEventValidation="false"%>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
      <%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
    <%@ OutputCache location="none" %> 
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
          $txtvsprocess.val('AGENTNAME:" " TEXT:" "');
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
                          case 'AGENTNAME':
                              var asSqlqry = '';
                              glcallback = callback;
                              ColServices.clsSectorServices.GetListOfSupplierAgentVisualSearch(asSqlqry, fnFillSearchVS, ErrorHandler, TimeOutHandler);
                              break;
                      }
                  },
                  facetMatches: function (callback) {
                      callback([
                { label: 'TEXT', category: 'AGENTS' },
                { label: 'AGENTNAME', category: 'AGENTS' },
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
function CallWebMethod(methodType)
    {
        switch(methodType)
        {
         case "sptype":
                var select=document.getElementById("<%=ddlType.ClientID%>");
                var codeid=select.options[select.selectedIndex].text;
                var selectname=document.getElementById("<%=ddlTName.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;                
                var connstr=document.getElementById("<%=txtconnection.ClientID%>");  
                constr=connstr.value   
                
                ColServices.clsServices.GetCatCodeListnew(constr,codeid,FillCatCodes,ErrorHandler,TimeOutHandler);
                ColServices.clsServices.GetCatNameListnew(constr,codeid,FillCatNames,ErrorHandler,TimeOutHandler);
                
                ColServices.clsServices.GetSellCatCodeListnew(constr,codeid,FillSellCatCodes,ErrorHandler,TimeOutHandler);
                ColServices.clsServices.GetSellCatNameListnew(constr,codeid,FillSellCatNames,ErrorHandler,TimeOutHandler);
             break;
           
           case "sptypename":
                var select=document.getElementById("<%=ddlTName.ClientID%>");
                var codeid=select.options[select.selectedIndex].value;
                var selectname=document.getElementById("<%=ddlType.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;
                
                var connstr=document.getElementById("<%=txtconnection.ClientID%>");  
                constr=connstr.value   
                
                ColServices.clsServices.GetCatCodeListnew(constr,codeid,FillCatCodes,ErrorHandler,TimeOutHandler);
                ColServices.clsServices.GetCatNameListnew(constr,codeid,FillCatNames,ErrorHandler,TimeOutHandler);
                
                ColServices.clsServices.GetSellCatCodeListnew(constr,codeid,FillSellCatCodes,ErrorHandler,TimeOutHandler);
                ColServices.clsServices.GetSellCatNameListnew(constr,codeid,FillSellCatNames,ErrorHandler,TimeOutHandler);

                break;
        case "catcode":
                var select=document.getElementById("<%=ddlCCode.ClientID%>");                
                var selectname=document.getElementById("<%=ddlCatName.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;
                break;
        case "catname":
                var select=document.getElementById("<%=ddlCatName.ClientID%>");                
                var selectname=document.getElementById("<%=ddlCCode.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;
                break;
        
                     
       
         case "ctrycode":
                var select=document.getElementById("<%=ddlContCode.ClientID%>");
                var codeid=select.options[select.selectedIndex].text;
                var selectname=document.getElementById("<%=ddlcontName.ClientID%>");
                
                selectname.value=select.options[select.selectedIndex].text;
                var connstr=document.getElementById("<%=txtconnection.ClientID%>");  
                constr=connstr.value   
                
                ColServices.clsServices.GetSectorCodeCtryListnew(constr,codeid,FillSectorCodes,ErrorHandler,TimeOutHandler);
                ColServices.clsServices.GetSectorNameCtryListnew(constr,codeid,FillSectorNames,ErrorHandler,TimeOutHandler);                
                
                break;
            case "ctryname":
                var select=document.getElementById("<%=ddlcontName.ClientID%>");
                var codeid=select.options[select.selectedIndex].value;
                var selectname=document.getElementById("<%=ddlContCode.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;

                var connstr=document.getElementById("<%=txtconnection.ClientID%>");  
                constr=connstr.value   

                ColServices.clsServices.GetSectorCodeCtryListnew(constr,codeid,FillSectorCodes,ErrorHandler,TimeOutHandler);
                ColServices.clsServices.GetSectorNameCtryListnew(constr,codeid,FillSectorNames,ErrorHandler,TimeOutHandler);                

                break;

            case "sectorcode":
                var select=document.getElementById("<%=ddlSectorCode.ClientID%>");
                var selectname=document.getElementById("<%=ddlSectorName.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;
                break; 
            case "sectorname":
                var select=document.getElementById("<%=ddlSectorName.ClientID%>");
                var selectname=document.getElementById("<%=ddlSectorCode.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;
                break;
         
        }
        }
        
        
function FillCatCodes(result)
    {
    	var ddl = document.getElementById("<%=ddlCCode.ClientID%>");
 		RemoveAll(ddl)
        for(var i=0;i<result.length;i++)
        {
            var option=new Option(result[i].ListText,result[i].ListValue);
            ddl.options.add(option);
        }
        ddl.value="[Select]";
    }
function FillCatNames(result)
    {
    	var ddl = document.getElementById("<%=ddlCatName.ClientID%>");
 		RemoveAll(ddl)
        for(var i=0;i<result.length;i++)
        {
            var option=new Option(result[i].ListText,result[i].ListValue);
            ddl.options.add(option);
        }
        ddl.value="[Select]";
    }


    
function FillSectorCodes(result)
    {
    	var ddl = document.getElementById("<%=ddlSectorCode.ClientID%>");
 		RemoveAll(ddl)
        for(var i=0;i<result.length;i++)
        {
            var option=new Option(result[i].ListText,result[i].ListValue);
            ddl.options.add(option);
        }
        ddl.value="[Select]";
    }

function FillSectorNames(result)
    {
    	var ddl = document.getElementById("<%=ddlSectorName.ClientID%>");
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


function checkTelephoneNumber(e)
			{	    
			    	
				if ( (event.keyCode < 45 || event.keyCode > 57) )
				{
					return false;
	            }   
	         	
			}
function checkNumber(e)
			{	    
			    	
				if ( (event.keyCode < 47 || event.keyCode > 57) )
				{
					return false;
	            }   
	         	
			}
function checkCharacter(e)
			{	    
			    if (event.keyCode == 32 || event.keyCode ==46)
			        return;			
				if ( (event.keyCode < 47 || event.keyCode > 57) && (event.keyCode < 65 || event.keyCode > 91) && (event.keyCode < 96 || event.keyCode > 122))
				{
					return false;
	            }

}





</script>
    <table style=" border-right: gray 2px solid; border-top: gray 2px solid; border-left: gray 2px solid; border-bottom: gray 2px solid;">
        <tr>
            <td class="field_heading" style="text-align: center; border-right: 2px solid; border-top: 2px solid; border-left: 2px solid; border-bottom: 2px solid; width:100%;">
                Supplier Agents List</td>
        </tr>
        <tr>
            <td class="td_cell" style="text-align: center; color: blue;  ">
                Type few characters of code or name and click search</td>
        </tr>
        <tr>
           <td style="width: 100%">
                <asp:UpdatePanel id="UpdatePanel1" runat="server">
                    <contenttemplate>
<TABLE>
<TBODY>


                      <tr><TD align=center colSpan=6>
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp; &nbsp;&nbsp; &nbsp;<asp:Button id="btnhelp" 
        tabIndex=3 onclick="btnhelp_Click" runat="server" Text="Help" 
        Font-Bold="False" CssClass="search_button"></asp:Button> &nbsp;&nbsp;<asp:Button id="btnAddNew" tabIndex=4 runat="server" Text="Add New" 
        Font-Bold="False" CssClass="btn"></asp:Button>&nbsp;&nbsp;<asp:Button id="btnPrint" tabIndex=5 runat="server" Text="Report" 
        CssClass="btn"></asp:Button></TD>
        </TR>
        <tr style="display:none">
<TD style="TEXT-ALIGN: center" colSpan=4>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <asp:RadioButton id="rbsearch" tabIndex=30 runat="server" Text="Search" ForeColor="Black" CssClass="td_cell" AutoPostBack="True" Checked="True" GroupName="GrSearch"></asp:RadioButton>&nbsp;<asp:RadioButton id="rbnadsearch" tabIndex=31 runat="server" Text="Advance Search" ForeColor="Black" CssClass="td_cell" AutoPostBack="True" GroupName="GrSearch" wfdid="w6"></asp:RadioButton>&nbsp;<asp:Button 
        id="btnSearch" tabIndex=14 runat="server" Text="Search" Font-Bold="False" 
        CssClass="search_button"></asp:Button>&nbsp;<asp:Button id="btnClear" 
        tabIndex=15 runat="server" Text="Clear" Font-Bold="False" 
        CssClass="search_button"></asp:Button>&nbsp;<asp:Button id="btnhelp1" 
        tabIndex=19 onclick="btnhelp_Click" runat="server" Text="Help" 
        Font-Bold="False" CssClass="search_button"></asp:Button>&nbsp; 
    <asp:Button id="btnAddNew1" tabIndex=16 runat="server" Text="Add New" 
        Font-Bold="False" CssClass="btn"></asp:Button>&nbsp;
         <asp:Button id="btnPrint1" tabIndex=18 runat="server" Text="Report" CssClass="btn"></asp:Button></TD><TD style="TEXT-ALIGN: center" colSpan=1></TD><TD style="TEXT-ALIGN: center" colSpan=1></TD>
         </TR><tr style="display:none">
        <td class="td_cell">
            Supplier Agent Code</td>
        <td>
            <asp:TextBox ID="txtAgentCode" runat="server" CssClass="txtbox" MaxLength="20" 
                tabIndex="1" Width="217px"></asp:TextBox>
        </td>
        <td class="td_cell">
            Supplier Agent Name</td>
        <td>
            <asp:TextBox ID="txtAgentName" runat="server" CssClass="txtbox" tabIndex="2" 
                Width="232px"></asp:TextBox>
        </td>
        <td>
            <asp:Label ID="Label3" runat="server" CssClass="field_caption" Text="Order By" 
                Width="50px"></asp:Label>
        </td>
        <td>
            <asp:DropDownList ID="ddlOrderBy" runat="server" AutoPostBack="True" 
                CssClass="drpdown" OnSelectedIndexChanged="ddlOrderBy_SelectedIndexChanged" 
                Width="149px">
            </asp:DropDownList>
        </td></tr>
    <tr style="display:none">
        <td>
            <asp:Label ID="lblSupplierTypeCode" runat="server" CssClass="td_cell" 
                Text="Supplier Type Code" Visible="False" Width="128px"></asp:Label>
        </td>
        <td style="WIDTH: 229px">
            <select id="ddlType" runat="server" class="drpdown" 
                onchange="CallWebMethod('sptype')" style="WIDTH: 223px" tabindex="3" 
                visible="false">
                <option selected=""></option>
            </select>
        </td>
        <td>
            <asp:Label ID="lblSupplierTypeName" runat="server" CssClass="td_cell" 
                Text="Supplier Type Name" Visible="False" Width="132px"></asp:Label>
        </td>
        <td style="WIDTH: 241px">
            <select id="ddlTName" runat="server" class="drpdown" 
                onchange="CallWebMethod('sptypename')" style="WIDTH: 238px" tabindex="4" 
                visible="false">
                <option selected=""></option>
            </select>
        </td>
        <td>
        </td>
        <td>
        </td>
    </tr>
    <tr style="display:none">
        <td>
            <asp:Label ID="lblCategoryCode" runat="server" CssClass="td_cell" 
                Text="Category Code" Visible="False" Width="110px"></asp:Label>
        </td>
        <td style="WIDTH: 229px">
            <select id="ddlCCode" runat="server" class="drpdown" 
                onchange="CallWebMethod('catcode')" style="WIDTH: 223px" tabindex="5" 
                visible="false">
                <option selected=""></option>
            </select>
        </td>
        <td>
            <asp:Label ID="lblcategoryname" runat="server" CssClass="td_cell" 
                Text="Category Name" Visible="False" Width="132px"></asp:Label>
        </td>
        <td style="WIDTH: 241px">
            <select id="ddlCatName" runat="server" class="drpdown" 
                onchange="CallWebMethod('catname')" style="WIDTH: 237px" tabindex="6" 
                visible="false">
                <option selected=""></option>
            </select>
        </td>
        <td>
        </td>
        <td>
        </td>
    </tr>
  
    <tr style="display:none">
        <td>
            <asp:Label ID="lblCountryCode" runat="server" CssClass="td_cell" 
                Text="Country Code" Visible="False" Width="110px"></asp:Label>
        </td>
        <td style="WIDTH: 229px">
            <select id="ddlContCode" runat="server" class="drpdown" 
                onchange="CallWebMethod('ctrycode')" style="WIDTH: 223px" tabindex="9" 
                visible="false">
                <option selected=""></option>
            </select>
        </td>
        <td>
            <asp:Label ID="lblcountryname" runat="server" CssClass="td_cell" 
                Text="Country Name" Visible="False" Width="132px"></asp:Label>
        </td>
        <td style="WIDTH: 241px">
            <select id="ddlcontName" runat="server" class="drpdown" 
                onchange="CallWebMethod('ctryname')" style="WIDTH: 237px" tabindex="10" 
                visible="false">
                <option selected=""></option>
            </select>
        </td>
        <td>
        </td>
        <td>
        </td>
    </tr>
    <tr style="display:none">
        <td>
            <asp:Label ID="lblSectorCode" runat="server" CssClass="td_cell" 
                Text="Sector Code" Visible="False" Width="110px"></asp:Label>
        </td>
        <td style="WIDTH: 229px">
            <select id="ddlSectorCode" runat="server" class="drpdown" 
                onchange="CallWebMethod('sectorcode')" style="WIDTH: 223px" tabindex="11" 
                visible="false">
                <option selected=""></option>
            </select>
        </td>
        <td>
            <asp:Label ID="lblSectorName" runat="server" CssClass="td_cell" 
                Text="Sector Name" Visible="False" Width="132px"></asp:Label>
        </td>
        <td style="WIDTH: 241px">
            <select id="ddlSectorName" runat="server" class="drpdown" 
                onchange="CallWebMethod('sectorname')" style="WIDTH: 237px" tabindex="12" 
                visible="false">
                <option selected=""></option>
            </select>
        </td>
        <td>
        </td>
        <td>
        </td>
    </tr>
    <tr style="display:none">
        <td>
            <asp:Label ID="lblcontrolACCode" runat="server" CssClass="td_cell" 
                Text="Control Account Code" Visible="False" Width="110px"></asp:Label>
        </td>
        <td style="WIDTH: 229px">
            <select id="ddlControlCode" runat="server" class="drpdown" style="WIDTH: 223px" 
                tabindex="13" visible="false">
                <option selected=""></option>
            </select>
        </td>
        <td>
            <input id="txtconnection" runat="server" style="visibility: hidden; width: 12px;
        height: 9px" type="text" />
        </td>
        <td style="WIDTH: 241px">
        </td>
        <td>
        </td>
        <td>
        </td>
    </tr>
    </TBODY></TABLE>
</contenttemplate>
                </asp:UpdatePanel></td>
        </tr> <TR>                                        
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
                               <td>  <asp:Label ID="RowSelectsas" runat="server" CssClass="field_caption" 
                            Text="Rows Selected "></asp:Label>
                     <asp:DropDownList ID="RowsPerPageAS" runat="server" AutoPostBack="true">
                      
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
            <td style="width:100%">
                &nbsp;<asp:Button ID="btnExportToExcel" runat="server" CssClass="btn" 
                    TabIndex="17" Text="Export To Excel" />
                </td>
        </tr>
        <tr>
            <td style="width:100%">
                <asp:UpdatePanel id="UpdatePanel2" runat="server">
                    <contenttemplate>
<asp:GridView id="gv_SearchResult" tabIndex=19 runat="server" Font-Size="10px" Width="100%" CssClass="grdstyle" GridLines="Vertical" CellPadding="3" BorderWidth="1px" BorderStyle="None" AutoGenerateColumns="False" AllowSorting="True" AllowPaging="True">
<FooterStyle CssClass="grdfooter"></FooterStyle>
<Columns>
<asp:TemplateField Visible="False" HeaderText="S. Agent Code"><EditItemTemplate>
                                            <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("supagentcode") %>'></asp:TextBox>
                                        
</EditItemTemplate>
<ItemTemplate>
<asp:Label id="lblagentCode" runat="server" Text='<%# Bind("supagentcode") %>'></asp:Label> 
</ItemTemplate>
</asp:TemplateField>
<asp:BoundField DataField="supagentcode" SortExpression="supagentcode" HeaderText="S. Agent Code">
<ItemStyle HorizontalAlign="Left"></ItemStyle>

<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
</asp:BoundField>
 <asp:TemplateField HeaderText="S. Agent Name" SortExpression="sptypename">
 <ItemTemplate>
                        <asp:Label ID="lblagentname" runat="server" Text='<%# Bind("supagentname") %>'></asp:Label>
                    </ItemTemplate>
                    <ControlStyle Width="8%" />
                    <HeaderStyle HorizontalAlign="Left" Width="8%" />
                    <ItemStyle CssClass="supagentname" HorizontalAlign="Left" Width="8%" />
          </asp:TemplateField>

<asp:BoundField DataField="sptypecode" Visible="true" SortExpression="sptypecode" HeaderText="Supplier Type Code">
<ItemStyle HorizontalAlign="Left"></ItemStyle>

<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
</asp:BoundField>
<asp:BoundField DataField="sptypename" Visible="false" SortExpression="sptypename" HeaderText="Supplier Type Name">
<ItemStyle HorizontalAlign="Left"></ItemStyle>

<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
</asp:BoundField>
<asp:BoundField HtmlEncode="False" DataField="catcode" SortExpression="catcode" HeaderText="Category Code">
<ItemStyle HorizontalAlign="Left"></ItemStyle>

<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
</asp:BoundField>
<asp:BoundField DataField="scatcode" Visible="False" SortExpression="scatcode" HeaderText="Selling Category Code">
<ItemStyle HorizontalAlign="Left"></ItemStyle>

<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
</asp:BoundField>
<asp:BoundField DataField="currcode" SortExpression="currcode" HeaderText="Currency">
<ItemStyle HorizontalAlign="Left"></ItemStyle>

<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
</asp:BoundField>
<asp:BoundField DataField="sectorcode" Visible="False" SortExpression="sectorcode" HeaderText="Sector Code">
<ItemStyle HorizontalAlign="Left"></ItemStyle>

<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
</asp:BoundField>
<asp:BoundField HtmlEncode="False" DataField="active" HeaderText="Active">
<ItemStyle HorizontalAlign="Left"></ItemStyle>

<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
</asp:BoundField>
<asp:BoundField DataField="add1" Visible="False" SortExpression="add1" HeaderText="Address Line 1">
<ItemStyle HorizontalAlign="Left"></ItemStyle>

<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
</asp:BoundField>
<asp:BoundField DataField="tel1" SortExpression="tel1" HeaderText="Telephone1">
<ItemStyle HorizontalAlign="Left"></ItemStyle>

<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
</asp:BoundField>
<asp:BoundField DataField="fax" SortExpression="fax" HeaderText="Fax">
<ItemStyle HorizontalAlign="Left"></ItemStyle>

<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
</asp:BoundField>
<asp:BoundField DataField="stel1" Visible="False" SortExpression="stel1" HeaderText="Sales Telephone">
<ItemStyle HorizontalAlign="Left"></ItemStyle>

<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
</asp:BoundField>
<asp:BoundField DataField="scontact1" Visible="False" SortExpression="scontact1" HeaderText="Sales Contact Person1">
<ItemStyle HorizontalAlign="Left"></ItemStyle>

<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
</asp:BoundField>
<asp:BoundField DataField="atel1" Visible="False" SortExpression="atel1" HeaderText="Accounts Telephone1">
<ItemStyle HorizontalAlign="Left"></ItemStyle>

<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
</asp:BoundField>
<asp:BoundField DataField="acontact1" Visible="False" SortExpression="acontact1" HeaderText="Accounts Contact Person1">
<ItemStyle HorizontalAlign="Left"></ItemStyle>

<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
</asp:BoundField>
<asp:BoundField DataField="controlacctcode" Visible="False" SortExpression="controlacctcode" HeaderText="Control A/C Code">
<ItemStyle HorizontalAlign="Left"></ItemStyle>

<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
</asp:BoundField>
<asp:BoundField DataField="crdays" SortExpression="crdays" HeaderText="Credit Days">
<ItemStyle HorizontalAlign="Left"></ItemStyle>

<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
</asp:BoundField>
<asp:BoundField DataField="crlimit" SortExpression="crlimit" HeaderText="Credit Limit">
<ItemStyle HorizontalAlign="Left"></ItemStyle>

<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
</asp:BoundField>
<asp:BoundField DataField="agtcode" Visible="False" SortExpression="agtcode" HeaderText="DMC Code">
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

<HeaderStyle CssClass="grdheader" ForeColor="White"></HeaderStyle>

<AlternatingRowStyle CssClass="grdAternaterow"></AlternatingRowStyle>
</asp:GridView> <asp:Label id="lblMsg" runat="server" Text="Records not found. Please redefine search criteria" Font-Size="8pt" Font-Names="Verdana" ForeColor="#084573" Font-Bold="True" Width="357px" Visible="False"></asp:Label> 
</contenttemplate>
                </asp:UpdatePanel>
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

