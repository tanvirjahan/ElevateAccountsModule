<%@ Page Language="VB" MasterPageFile="~/SubPageMaster.master" AutoEventWireup="false" EnableEventValidation="false" CodeFile="ContractApprove.aspx.vb" Inherits="ContractApprove" %>

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
  <script src="//code.jquery.com/jquery-1.10.2.js"></script>
  
<script language="javascript" type="text/javascript" >

    function BindGrid() {
        $("#dialog").dialog();
        $("#divBackground").show();
    }

   


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
        var vartxtcode = document.getElementById(" <%=hdnpartycode.ClientID%>");


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


    function CheckContract(contractid) {
        var hdncontract = document.getElementById("<%=hdncontractid.ClientID%>");

        if ((hdncontract.value == '')) {
            alert('Please Save Contract Main details to continue');
            return false;
        }

    }

    function CheckPromotion(promotionid) {
       
        var hdnpromotionid = document.getElementById(promotionid);

        if ((hdnpromotionid.value == '')) {
            alert('Please Save Promotion Main details to continue');
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

      function ShowProgess() {

          var ModalPopupDays = $find("ModalPopupDays");

          ModalPopupDays.show();
          return true;
      }

      function hidenoerror() {

          var ModalPopupError = $find("ModalPopupError");

          ModalPopupError.hide();
          return true;
      }

</script>
<style type="text/css">
    .modalBackground
    {
        background-color: Black;
        filter: alpha(opacity=90);
        opacity: 0.8;
    }
    .modalPopup
    {
        background-color: #FFFFFF;
        border-width: 3px;
        border-style: solid;
        border-color: black;
        padding-top: 10px;
        padding-left: 10px;
        width: 300px;
        height: 140px;
    }
</style>

    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
        <ContentTemplate>
            <table style="width: 100%; height: 100%; border-right: gray 2px solid; border-top: gray 2px solid;
                border-left: gray 2px solid; border-bottom: gray 2px solid" class="td_cell" align="left">
                <tbody>
                    <tr>
                        <td valign="top" align="center" width="150px" colspan="4">
                            <asp:Label ID="lblHeading" runat="server" Text="Contract Approval" CssClass="field_heading"
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
                                        <asp:Panel ID="Panelsearch" runat="server" Font-Bold="true" GroupingText="Approval Details"
                                            Width="100%">
                                            <table style="width: 100%">
                                                <tr>
                                                    <td>
                                                    <table>
                                                        <tr>
                                                            <td style ="display:none">
                                                            <asp:Button ID="btnchecking" runat="server"  Text="Start Checking" Font-Bold="False" CssClass="btn" OnClientClick="ShowProgess();"   />
                                                            </td>
                                                            <td>
                                                             <asp:Button ID="btncheckingnew" runat="server"  Text="Start Checking" Font-Bold="False" CssClass="btn" OnClientClick="ShowProgess();"   />
                                                            </td>
                                                            <td><asp:Button ID="btnrefresh" runat="server" Text="Refresh" Font-Bold="False" CssClass="btn">
                                                                </asp:Button>
                                                            </td>
                                                             <td>
                                                             <asp:Button ID="btncalculaterate" runat="server"  Text="Calculate" Font-Bold="False" CssClass="btn" OnClientClick="ShowProgess();"   />
                                                            </td>
                                                            <td><asp:Button ID="btnApprove" runat="server" CssClass="btn" 
                                                                    Font-Bold="False" Text="Approve" OnClientClick="ShowProgess();"/>
                                                            </td>
                                                    
                                                        </tr>
                                                    </table>
                                                        
                                                                                                      
                                                       
                                                    </td>
                                                  
                                                </tr>
                                                <tr style ="display:none">
                                                    <td style="width: 100%" valign="top" colspan="5" >
                                                        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                                            <ContentTemplate>
                                                                <table width="100%">
                                                                    <tr>
                                                                        <td class="field_heading" colspan="4">
                                                                            <asp:Label ID="Label1" runat="server" Text="Validation Errors"></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <div id="searchresults" style="display:none" >
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
                                                                                        AllowSorting="True" AllowPaging="True" PageSize="20">
                                                                                        <FooterStyle CssClass="grdfooter"></FooterStyle>
                                                                                        <Columns>
                                                                                          

                                                                                             <asp:TemplateField  HeaderText="Option Name">
                                                                                                <ItemTemplate>
                                                                                                    <asp:Label ID="lbloptions" runat="server" Text='<%# Bind("optionname") %>' __designer:wfdid="w1"></asp:Label>
                                                                                                </ItemTemplate>
                                                                                                 <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                                                                                              <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                                                                            </asp:TemplateField>

                                                                                             <asp:TemplateField  HeaderText="Contract No">
                                                                                                <ItemTemplate>
                                                                                                    <asp:Label ID="lblContractid" runat="server" Text='<%# Bind("Contractid") %>' __designer:wfdid="w1"></asp:Label>
                                                                                                </ItemTemplate>
                                                                                                 <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                                                                                              <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                                                                            </asp:TemplateField>

                                                                                             <asp:TemplateField  HeaderText="Tran.Id">
                                                                                           
                                                                                                <ItemTemplate>
                                                                                                    <asp:Label ID="lblTranid" runat="server" Text='<%# Bind("tranid") %>' __designer:wfdid="w1"></asp:Label>
                                                                                                   
                                                                                                </ItemTemplate>
                                                                                                 <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                                                                                              <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                                                                            </asp:TemplateField>

                                                                                             <asp:TemplateField  HeaderText="Error Message">
                                                                                                <ItemTemplate>
                                                                                                <asp:Label ID="lblerrormsg" runat="server"    Text='<%# Limit(Eval("errormsg"), 50)%>' Tooltip='<%# Eval("errormsg")%>' ></asp:Label>
                                                                                                <br />
                                                                                                <asp:LinkButton ID="ReadMoreLinkButton" runat="server" Text="More" Visible='<%# SetVisibility(Eval("errormsg"), 50) %>'  OnClick="ReadMoreLinkButton_Click"></asp:LinkButton>


                                                                                                  
                                                                                                </ItemTemplate>
                                                                                                 <HeaderStyle HorizontalAlign="Left"  />
                                                                                              <ItemStyle HorizontalAlign="Left"  />
                                                                                            </asp:TemplateField>

                                                                                             <asp:TemplateField  HeaderText="Error Type" Visible ="false" >
                                                                                                <ItemTemplate>
                                                                                                    <asp:Label ID="lblErrType" runat="server" Text='<%# Bind("errtype") %>' __designer:wfdid="w1"></asp:Label>
                                                                                                </ItemTemplate>
                                                                                                 <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                                                                                              <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                                                                            </asp:TemplateField>

                                                                                              <asp:TemplateField HeaderText="Details">
                                                                                                <ItemTemplate>
                                                                                                    <asp:LinkButton ID="lnkDetails" runat="server" Font-Size="9pt" CommandName="Details" CommandArgument ='<%# Ctype(Container,GridViewRow).RowIndex %>' >Details</asp:LinkButton>
                                                                                                </ItemTemplate>
                                                                                            </asp:TemplateField>

                                                                                      
                                                                                        
                                                                                          
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
                                            <div id="dialog" title="Active Logs">
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <asp:GridView ID="gvErrorlist" runat="server" AutoGenerateColumns="False" BorderColor="#999999"
                                                                BorderStyle="None" BorderWidth="1px" CellPadding="3" CssClass="grdstyle" Font-Size="10px"
                                                                GridLines="Vertical" TabIndex="0" PageSize="20">
                                                                <RowStyle CssClass="grdRowstyle" />
                                                                <Columns>
                                                                    <asp:BoundField ItemStyle-Width="100px" DataField="Tranid" HeaderText="Tran ID" />
                                                                    <asp:BoundField ItemStyle-Width="100px" DataField="fromdate" HeaderText="From Date" />
                                                                    <asp:BoundField ItemStyle-Width="100px" DataField="Todate" HeaderText="To Date" />
                                                                    <asp:BoundField ItemStyle-Width="150px" DataField="ctryname" HeaderText="Country" />
                                                                    <asp:BoundField ItemStyle-Width="150px" DataField="agentname" HeaderText="Agent" />
                                                                </Columns>
                                                                <FooterStyle CssClass="grdfooter" />
                                                                <PagerStyle CssClass="grdpagerstyle" HorizontalAlign="Center" />
                                                                <SelectedRowStyle CssClass="grdselectrowstyle" />
                                                                <HeaderStyle CssClass="grdheader" />
                                                                <AlternatingRowStyle CssClass="grdAternaterow" />
                                                            </asp:GridView>
                                                            <asp:Label ID="lblInfoMsg" runat="server" ForeColor="Red"></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </asp:Panel>
                                    </td>
                                </tr>
                            </table>
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
                                        <asp:HiddenField ID="hdnpromotionid" runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </tbody>
            </table>

             <center>
                            <div id="Loading1" runat="server" style="height: 150px; width: 500px;">
                                <img alt="" id="Image1" runat="server" src="~/Images/loader-progressbar.gif" width="200" />
                                <h2 style="color: #06788B">
                                    Processing please wait...</h2>
                            </div>
                             <div id="divnoerror" runat="server" CssClass="modalPopup" align="center">
                            <table style="float: left">
                            <tr>
                               <td>
                               <asp:Label  ID="lblerrorid" runat="server" class="cls_chkcomm"  Text="No Errors in Selected Records  Now calculating the Rates !." TabIndex="8" ForeColor="Red"  />
                               </td>
                               </tr>
                               <tr>
                               <td align ="center">
                                  <asp:Button ID="btnOk1"  runat="server" CssClass="field_button" Text="Ok" Width="80px" OnClientClick="hidenoerror();showprogess();" />&nbsp;
                                </td>
                                </tr>
                            </table>
                            </div>
                        </center>
                        <asp:ModalPopupExtender ID="ModalPopupDays" runat="server" BehaviorID="ModalPopupDays"
                            TargetControlID="btnInvisibleGuest" CancelControlID="btnClose" PopupControlID="Loading1"
                            BackgroundCssClass="ModalPopupBG">
                        </asp:ModalPopupExtender>

                           <asp:ModalPopupExtender ID="ModalPopupError" runat="server" BehaviorID="ModalPopupError"
                            TargetControlID="btnInvisibleGuest" CancelControlID="btnClose" PopupControlID="divnoerror"
                            BackgroundCssClass="modalBackground">
                        </asp:ModalPopupExtender>

                         <input id="btnInvisibleGuest" runat="server" type="button" value="Cancel" style="display: none" />
                        <input id="btnClose" type="button" value="Cancel" style="display: none" />

        </ContentTemplate>
    </asp:UpdatePanel>

  

    <asp:ScriptManagerProxy id="ScriptManagerProxy1" runat="server">
        <services>
<asp:ServiceReference Path="~/clsServices.asmx"></asp:ServiceReference>
</services>
    </asp:ScriptManagerProxy>

</asp:Content>


