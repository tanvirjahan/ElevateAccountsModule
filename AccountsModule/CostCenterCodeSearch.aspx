<%@ Page Language="VB" MasterPageFile="~/AccountsMaster.master" AutoEventWireup="false" CodeFile="CostCenterCodeSearch.aspx.vb" Inherits="CostCenterCodeSearch" %>


     <%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
 <%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ OutputCache Location="none" %>
    <asp:Content ID="Content2" ContentPlaceHolderID="Main" runat="server">
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
        .style11
        {
            width: 68px;
        }
        .style15
        {
            width: 166px;
        }
        .style17
        {
            width: 87px;
        }
        .style18
        {
            width: 156px;
        }
        .style19
        {
            width: 51px;
        }
        .style20
        {
            width: 157px;
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


        }



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
        $txtvsprocess.val('CostCenter:" " Group:" " TEXT:" "');
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
                        case 'CostCenter':
                            var asSqlqry = '';
                            glcallback = callback;
                            ColServices.clsSectorServices.GetListOfCostCenterVisualSearch(asSqlqry, fnFillSearchVS, ErrorHandler, TimeOutHandler);
                            break;
                        case 'Group':
                            var asSqlqry = '';
                            glcallback = callback;
                            ColServices.clsSectorServices.GetListOfCostGroupVisualSearch(asSqlqry, fnFillSearchVS, ErrorHandler, TimeOutHandler);
                            break;
                    }
                },
                facetMatches: function (callback) {
                    callback([
                { label: 'TEXT', category: 'CostCenter' },
                { label: 'Group', category: 'CostCenter' },
                 { label: 'CostCenter', category: 'CostCenter' },
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
    
 

    <table style="border-right: gray 2px solid; border-top: gray 2px solid; border-left: gray 2px solid; border-bottom: gray 2px solid">
        <tr>
            <td style="width: 100%">
                <table width="100%">
                    <tr>
                        <td class="field_heading" style="width: 100%" align="center">
                            Cost Centers List</td>
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
        <TR>
        <TD style="WIDTH: 80px">&nbsp;</TD>
        <TD style="WIDTH: 100px">&nbsp;</TD>
        <TD style="WIDTH: 79px">&nbsp;</TD>
        <TD style="WIDTH: 100px">&nbsp;</TD>
        <TD style="WIDTH: 178px">&nbsp;</TD>
        <TD style="WIDTH: 178px">&nbsp;</TD>
        </TR>
        <TR><TD colspan="6">
            <div style="width:100%">
                <div style="width:80%;display: inline-block; margin: -6px 4px 0 0;">
                    <div ID="VS" class="container" style="border:0px;">
                        <div ID="search_box_container">
                        </div>
                        <asp:TextBox ID="txtvsprocess" runat="server" class="cs_txtvsprocess" 
                            style="display:none"></asp:TextBox>
                        <asp:TextBox ID="txtvsprocesssplit" runat="server" class="cs_txtvsprocesssplit" 
                            style="display:none"></asp:TextBox>
                        <asp:Button ID="btnvsprocess" runat="server" style="display:none" />
                    </div>
                </div>
                <div style=" width:18%; display: inline-block;vertical-align:top;">
                    <asp:Button ID="btnResetSelection" runat="server" CssClass="btn" 
                        Font-Bold="False" tabIndex="4" Text="Reset Search" />
                </div>
            </div>
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
            </TD>
        </TR>
         


        
        <TR><TD colspan="6">
            <table class="style1">
                <tr>
                    <td>
                        &nbsp;</td>
                    <td class="style17">
                        &nbsp;</td>
                    <td class="style11">
                        &nbsp;</td>
                    <td class="style20">
                        &nbsp;</td>
                    <td class="style19">
                        &nbsp;</td>
                    <td class="style18">
                        &nbsp;</td>
                    <td>
                        &nbsp;</td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label6" runat="server" CssClass="field_caption" 
                            Text="Filter By "></asp:Label>
                    </td>
                    <td class="style17">
                        <asp:DropDownList ID="ddlOrder" runat="server">
                            <asp:ListItem Value="C">Created Date</asp:ListItem>
                            <asp:ListItem Value="M">Modified Date</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td class="style11">
                        <asp:Label ID="Label4" runat="server" CssClass="field_caption" Text="From Date"></asp:Label>
                    </td>
                    <td class="style20">
                        <asp:TextBox ID="txtFromDate" runat="server" onchange="filltodate(this);" 
                            Width="75px"></asp:TextBox>
                        <asp:ImageButton ID="ImgBtnFrmDt" runat="server" 
                            ImageUrl="~/Images/Calendar_scheduleHS.png" TabIndex="3" Width="16px" />
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
                    <td class="style19">
                        <asp:Label ID="Label5" runat="server" CssClass="field_caption" Text="To Date"></asp:Label>
                    </td>
                    <td class="style18">
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
                            tabIndex="4" Text="Search by Date" Width="111px" />
                        &nbsp;<asp:Button ID="btnClearDate" runat="server" CssClass="btn" Font-Bold="False" 
                            tabIndex="4" Text="Reset Dates" Width="94px" />
                       &nbsp;  &nbsp; <asp:Label ID="RowSelectss" runat="server" CssClass="field_caption" 
                            Text="Rows Selected "></asp:Label>
                        &nbsp;<asp:DropDownList ID="RowsPerPageSS" runat="server" AutoPostBack="true">
                            <asp:ListItem Value="5">5</asp:ListItem>
                            <asp:ListItem Value="10">10</asp:ListItem>
                            <asp:ListItem Value="15">15</asp:ListItem>
                            <asp:ListItem Value="20">20</asp:ListItem>
                            <asp:ListItem Value="25">25</asp:ListItem>
                            <asp:ListItem Value="30">30</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td>
                        &nbsp;</td>
                    <tr>
                        <td>
                            &nbsp;</td>
                        <td class="style17">
                            &nbsp;</td>
                        <td class="style11">
                            &nbsp;</td>
                        <td class="style20">
                            &nbsp;</td>
                        <td class="style19">
                            &nbsp;</td>
                        <td class="style18">
                            &nbsp;</td>
                        <td>
                            &nbsp;</td>
                    </tr>
                </tr>
            </table>
            </TD>
        </TR>

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
 <asp:GridView id="gv_SearchResult" tabIndex=10 runat="server" Width="100%" CssClass="grdstyle" GridLines="Vertical" CellPadding="3" AutoGenerateColumns="False" AllowSorting="True" AllowPaging="True">
<Columns>
<asp:TemplateField Visible="False" HeaderText="Cost Code"><EditItemTemplate>
                                <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("costcenter_code") %>'></asp:TextBox>
                            
</EditItemTemplate>
<ItemTemplate>
                                <asp:Label ID="lblCode" runat="server" Text='<%# Bind("costcenter_code") %>'></asp:Label>
                            
</ItemTemplate>
</asp:TemplateField>
<asp:BoundField DataField="costcenter_code" SortExpression="costcenter_code" HeaderText="Cost Code"></asp:BoundField>


                                        <asp:TemplateField HeaderText="Cost Name" SortExpression="costcenter_name">
                    <ItemTemplate>
                        <asp:Label ID="lblcostcentername" runat="server" Text='<%# Bind("costcenter_name") %>'></asp:Label>
                    </ItemTemplate>
                    <ControlStyle   />
                    <HeaderStyle HorizontalAlign="Left" Width="8%" />
                    <ItemStyle  HorizontalAlign="Left" Width="8%" />
                </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Group Name" SortExpression="costcentergrp_name">
                    <ItemTemplate>
                        <asp:Label ID="lblgrpname" runat="server" Text='<%# Bind("costcentergrp_name") %>'></asp:Label>
                    </ItemTemplate>
                    <ControlStyle   />
                    <HeaderStyle HorizontalAlign="Left" Width="8%" />
                    <ItemStyle   HorizontalAlign="Left" Width="8%" />
                </asp:TemplateField>


<asp:BoundField DataField="Active" SortExpression="Active" HeaderText="Active"></asp:BoundField>
<asp:BoundField HtmlEncode="False" DataFormatString="{0:dd/MM/yyyy hh:mm:ss tt} " DataField="adddate" SortExpression="adddate" HeaderText="Date Created"></asp:BoundField>
<asp:BoundField DataField="adduser" SortExpression="adduser" HeaderText="User Created"></asp:BoundField>
<asp:BoundField HtmlEncode="False" DataFormatString="{0:dd/MM/yyyy hh:mm:ss tt} " DataField="moddate" SortExpression="moddate" HeaderText="Date Modified"></asp:BoundField>
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
</Columns>
<FooterStyle CssClass="grdfooter" />
<RowStyle CssClass="grdRowstyle"></RowStyle>
<SelectedRowStyle CssClass="grdselectrowstyle" ></SelectedRowStyle>
<PagerStyle CssClass="grdpagerstyle" HorizontalAlign="Center"></PagerStyle>
<HeaderStyle  CssClass="grdheader" ForeColor="white"></HeaderStyle>
<AlternatingRowStyle CssClass="grdAternaterow"></AlternatingRowStyle>
</asp:GridView><asp:Label id="lblMsg" runat="server" 
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


