<%@ Page Language="VB" MasterPageFile="~/SubPageMaster.master" AutoEventWireup="false" EnableEventValidation="false" CodeFile="ContractHotelConst.aspx.vb" Inherits="ContractHotelConst" %>

<%@ Register Src="SubMenuUserControl.ascx" TagName="SubMenuUserControl" TagPrefix="uc1" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
 

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
    
  
  <script src="../Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
  
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

    function TimeOutHandler(result) {
        alert("Timeout :" + result);
    }

    function ErrorHandler(result) {
        var msg = result.get_exceptionType() + "\r\n";
        msg += result.get_message() + "\r\n";
        msg += result.get_stackTrace();
        alert(msg);
    }

   

 

    function FormValidationMainDetail(state) {
        var txtnameval = document.getElementById("<%=txtname.ClientID%>");
        if (txtnameval.value == '') {
            //            alert('Name Cannot be blank');
            //            return false;
        }
        else {
            if (state == 'New') { if (confirm('Are you sure you want to save Exhibition Supplements  ') == false) return false; }
            if (state == 'Edit') { if (confirm('Are you sure you want to update?') == false) return false; }
            if (state == 'Delete') { if (confirm('Are you sure you want to delete?') == false) return false; }
        }
    }



    function formmodecheck() {
        var vartxtcode = document.getElementById(" <%=txthotelname.ClientID%>");


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






  



    function checkNumber(evt) {
        evt = (evt) ? evt : event;
        var charCode = (evt.charCode) ? evt.charCode : ((evt.keyCode) ? evt.keyCode :
        ((evt.which) ? evt.which : 0));
        if (charCode != 47 && (charCode > 44 && charCode < 58)) {
            return true;
        }
        return false;
    }




  

    function ChangeDate(FromId, ToId) {
        var ResultFrom = document.getElementById(FromId).value;
        var ResultTo = document.getElementById(ToId);
        ResultTo.value = ResultFrom;

    }


    function filltodate(fDate) {
        var txtfromDate = document.getElementById("<%=txtfromDate.ClientID%>");
        var txtToDate = document.getElementById("<%=txtToDate.ClientID%>");
        var curDate = document.getElementById("<%=hdCurrentDate.ClientID%>");

        if (txtfromDate.value != null) {
            var dp = txtfromDate.value.split("/");

            var newDt = new Date(dp[2] + "/" + dp[1] + "/" + dp[0]);
            var today = new Date();

            newDt = getFormatedDate(newDt);
            today = getFormatedDate(today);

            newDt = new Date(newDt);
            today = new Date(today);

            if (newDt < today) {

                alert('From date should not be less than todays date.');
                txtfromDate.value = curDate.value;
                return;
            }
            else {
                if (txtToDate.value != null) {
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
            }

        }

    }

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


    function getFormatedDate(chkdate) {
        var dd = chkdate.getDate();
        var mm = chkdate.getMonth() + 1; //January is 0!
        var yyyy = chkdate.getFullYear();
        if (dd < 10) { dd = '0' + dd };
        if (mm < 10) { mm = '0' + mm };
        chkdate = mm + '/' + dd + '/' + yyyy;
        return chkdate;
    }


    function getCheckoutdt(checkindate, checkoutdate) {

        var txtToDate = document.getElementById("<%=txtToDate.ClientID%>");
        var numberOfDaysToAdd = 0;
        var dp = checkindate.value.split("/");
        var newDt = new Date(dp[2] + "/" + dp[1] + "/" + dp[0]);
        newDt.setDate(newDt.getDate() + numberOfDaysToAdd);
        var dd = newDt.getDate();
        if (dd < 10) {
            dd = "0" + dd;
        }
        var mm = newDt.getMonth() + 1;
        if (mm < 10) {
            mm = "0" + mm;
        }
        var y = newDt.getFullYear();
        var someFormattedDate = dd + '/' + mm + '/' + y;

        checkoutdate.value = someFormattedDate;
        txtToDate.value = checkoutdate.value
    }



    function CheckContract(contractid) {
        var hdncontract = document.getElementById("<%=hdncontractid.ClientID%>");

        if ((hdncontract.value == '')) {
            alert('Please Save Contract Main details to continue');
            return false;
        }

    }
       	
</script>
  <script type="text/javascript">

      var prm = Sys.WebForms.PageRequestManager.getInstance();
      prm.add_initializeRequest(InitializeRequestUserControl);
      prm.add_endRequest(EndRequestUserControl);

      function InitializeRequestUserControl(sender, args) {

      }

      function EndRequestUserControl(sender, args) {
        
      }
</script>

    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
        <ContentTemplate>
            <table style="width: 100%; height: 100%; border-right: gray 2px solid; border-top: gray 2px solid;
                border-left: gray 2px solid; border-bottom: gray 2px solid" class="td_cell" align="left">
                <tbody>
                    <tr>
                        <td valign="top" align="center" width="150px" colspan="4">
                            <asp:Label ID="lblHeading" runat="server" Text="Construction" CssClass="field_heading"
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
                                                        &nbsp;
                                                    </td>
                                                    <td style="display:none">
               <asp:Button id="btnExportToExcel" tabIndex=16  runat="server"  
        CssClass="field_button"></asp:Button>
        <asp:Button id="btnprint" tabIndex=16  runat="server"  
        CssClass="field_button"></asp:Button>
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
                                                                <table width="100%">
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
                                                                                <div id="Showdetails" runat="server" style="width: 100%; border: 3px solid #2D7C8A;
                                                                                    background-color: White;">
                                                                                    <asp:GridView ID="gv_SearchResult" runat="server" Font-Size="10px" Width="100%"
                                                                                        CssClass="grdstyle" __designer:wfdid="w42" GridLines="Vertical" CellPadding="3"
                                                                                        BorderWidth="1px" BorderStyle="None" BorderColor="#999999" AutoGenerateColumns="False"
                                                                                        AllowSorting="True" AllowPaging="True" >
                                                                                        <FooterStyle CssClass="grdfooter"></FooterStyle>
                                                                                        <Columns>
                                                                                            <asp:TemplateField Visible="False" HeaderText="Supplier Code">
                                                                                                <EditItemTemplate>
                                                                                                </EditItemTemplate>
                                                                                                <ItemTemplate>
                                                                                                    <asp:Label ID="lbltran" runat="server" Text='<%# Bind("constructionid") %>' __designer:wfdid="w1"></asp:Label>
                                                                                                </ItemTemplate>
                                                                                            </asp:TemplateField>
                                                                                            <asp:BoundField DataField="constructionid" HeaderText="Tran.ID"></asp:BoundField>
                                                                                            <asp:BoundField DataField="fromdate" HeaderText="From Date"></asp:BoundField>
                                                                                            <asp:BoundField DataField="todate" HeaderText="To Date"></asp:BoundField>
                                                                                            <asp:BoundField DataField="reason" HeaderText="Reason For Construction"></asp:BoundField>
                                                                                             <asp:TemplateField HeaderText="Remarks">
                                                                                                 <ItemTemplate>
                                                                                                <asp:Label ID="lblMiscellaneous" runat="server" Text='<%# Bind("Miscellaneous") %>' Width="300px"></asp:Label>
                                                                                                </ItemTemplate>
                                                         
                                                                                            </asp:TemplateField>

                                                                                      
                                                                                            <asp:ButtonField HeaderText="Action" Text="Edit" CommandName="EditRow">
                                                                                                <ItemStyle ForeColor="Blue"></ItemStyle>
                                                                                            </asp:ButtonField>
                                                                                            <asp:ButtonField HeaderText="Action" Text="View" CommandName="View">
                                                                                                <ItemStyle ForeColor="Blue"></ItemStyle>
                                                                                            </asp:ButtonField>
                                                                                            <asp:ButtonField HeaderText="Action" Text="Delete" CommandName="DeleteRow">
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
                                                Width="100%" Style="display: none">
                                                <table>
                                                   
                                                    <tr>
                                                        <td>
                                                            &nbsp;
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
                                                        <td>
                                                            &nbsp;
                                                        </td>
                                                    </tr>
                                                
                                                    <tr>
                                                        <div id="hoteldetails">
                                                            <td>
                                                                <table>
                                                                    <tr>
                                                                        <td>
                                                                            Hotel&nbsp;Name
                                                                        </td>
                                                                        <td align="left" class="td_cell" valign="top">
                                                                            <asp:TextBox ID="txthotelname" runat="server" CssClass="field_input" TabIndex="1"
                                                                                Width="250px" Enabled="false"></asp:TextBox>
                                                                        </td>
                                                                        <td>
                                                                            Auto&nbsp;ID
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="txttranid" runat="server" ReadOnly="true" Width="120px"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:Label ID="Label2" runat="server" Style="vertical-align: top;" Text="From Date"
                                                                                Width="90px"></asp:Label>
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtFromDate" runat="server" onchange="filltodate(this);"  TabIndex="1"
                                                                                    Width="75px"></asp:TextBox>
                                                                                <asp:ImageButton ID="ImgBtnFrmDt" runat="server" 
                                                                                    ImageUrl="~/Images/Calendar_scheduleHS.png" TabIndex="-1" />
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
                                                                            <asp:Label ID="Label3" runat="server" Style="vertical-align: top;" Text="To Date"
                                                                                Width="90px"></asp:Label>
                                                                        </td>
                                                                        <td align="left">
                                                                        <asp:TextBox ID="txtToDate" runat="server" onchange="ValidateChkInDate();" TabIndex="2"
                                                                            Width="75px"></asp:TextBox>
                                                                        <asp:ImageButton ID="ImgBtnToDt" runat="server" 
                                                                            ImageUrl="~/Images/Calendar_scheduleHS.png" TabIndex="-1" />
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
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </div>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="3" style="width: 900px" valign="top">
                                                            <asp:UpdatePanel ID="upnloccupancy" runat="server">
                                                                <ContentTemplate>
                                                                    <table width="925px">
                                                                        <tr>
                                                                            <td class="field_heading" colspan="4">
                                                                                <div style="margin-left: auto; margin-right: auto; text-align: center;">
                                                                                    <asp:Label ID="Label5" for="fmhead" runat="server" Text="Construction Details"></asp:Label>
                                                                                </div>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>
                                                                                <asp:Label ID="lblreason" runat="server" Style="vertical-align: top;"
                                                                                Text="Reasons" Width="70px"></asp:Label>
                                                                            </td>
                                                                            <td >
                                                                                <TEXTAREA style="WIDTH:752px;height: 48px;" id="txtReason" class="field_input" tabIndex="3"  
                                                                                rows=2 runat="server" ></TEXTAREA>
                                                                            </td>
                                                                            <td></td> 
                                                                           
                                                                            <td>
                                                                                &nbsp;
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                             <td>
                                                                                <asp:Label ID="Label4" runat="server" Style="vertical-align: top;"
                                                                                Text="Remarks" Width="70px"></asp:Label>
                                                                            </td>
                                                                              <td >
                                                                                <TEXTAREA style="WIDTH: 752px;height: 105px;" id="txtremarks" class="field_input" tabIndex="4"  
                                                                                rows=2 runat="server" ></TEXTAREA>
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
                                                                    <asp:HiddenField ID="hdnMainGridRowid" runat="server" />
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </td>
                                                    </tr>
                                                     <tr>
                                                        <td width="250px" colspan="2">
                                                            <asp:Button ID="btnSave" runat="server" CssClass="field_button" TabIndex="5" Text="Save"
                                                                Width="93px" />
                                                            <asp:Button ID="btnreset1" runat="server" CssClass="field_button" TabIndex="6" Text="Return To Search"
                                                                Width="139px" />
                                                        </td>
                                                        <td>
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
                            &nbsp;<asp:Button ID="btnhelp" runat="server" CssClass="field_button" TabIndex="26"
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
                                        <asp:HiddenField ID="hdnpartycode" runat="server" />
                                        <asp:HiddenField ID="hdncontractid" runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </tbody>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>

  

    <asp:ScriptManagerProxy id="ScriptManagerProxy1" runat="server">
        <services>
<asp:ServiceReference Path="~/clsServices.asmx"></asp:ServiceReference>
</services>
    </asp:ScriptManagerProxy>

</asp:Content>


