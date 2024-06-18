<%@ Page Language="VB" AutoEventWireup="false" CodeFile="RptBillStatusQuery.aspx.vb"
    Inherits="RptBillStatusQuery" MasterPageFile="~/AccountsMaster.master" Strict="true" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ OutputCache Location="none" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="EclipseWebSolutions.DatePicker" Namespace="EclipseWebSolutions.DatePicker"
    TagPrefix="ews" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="Server">
    <link rel="stylesheet" href="../Content/lib/css/reset.css" type="text/css" media="screen"
        charset="utf-8">
    <link rel="stylesheet" href="../Content/lib/css/icons.css" type="text/css" media="screen"
        charset="utf-8">
    <link rel="stylesheet" href="../Content/lib/css/workspace.css" type="text/css" media="screen"
        charset="utf-8">
    <link href="../css/VisualSearch.css" rel="stylesheet" type="text/css" />
    <script src="../Content/vendor/jquery-1.11.0.js" type="text/javascript" charset="utf-8"></script>
    <script src="../Content/vendor/jquery.ui.core.js" type="text/javascript" charset="utf-8"></script>
    <script src="../Content/vendor/jquery.ui.widget.js" type="text/javascript" charset="utf-8"></script>
    <script src="../Content/vendor/jquery.ui.position.js" type="text/javascript" charset="utf-8"></script>
    <script src="../Content/vendor/jquery.ui.menu.js" type="text/javascript" charset="utf-8"></script>
    <script src="../Content/vendor/jquery.ui.autocomplete.js" type="text/javascript"
        charset="utf-8"></script>
    <script src="../Content/vendor/underscore-1.5.2.js" type="text/javascript" charset="utf-8"></script>
    <script src="../Content/vendor/backbone-1.1.0.js" type="text/javascript" charset="utf-8"></script>
    <script src="../Content/lib/js/visualsearch.js" type="text/javascript" charset="utf-8"></script>
    <script src="../Content/lib/js/views/search_box.js" type="text/javascript" charset="utf-8"></script>
    <script src="../Content/lib/js/views/search_facet.js" type="text/javascript" charset="utf-8"></script>
    <script src="../Content/lib/js/views/search_input.js" type="text/javascript" charset="utf-8"></script>
    <script src="../Content/lib/js/models/search_facets.js" type="text/javascript" charset="utf-8"></script>
    <script src="../Content/lib/js/models/search_query.js" type="text/javascript" charset="utf-8"></script>
    <script src="../Content/lib/js/utils/backbone_extensions.js" type="text/javascript"
        charset="utf-8"></script>
    <script src="../Content/lib/js/utils/hotkeys.js" type="text/javascript" charset="utf-8"></script>
    <script src="../Content/lib/js/utils/jquery_extensions.js" type="text/javascript"
        charset="utf-8"></script>
    <script src="../Content/lib/js/utils/search_parser.js" type="text/javascript" charset="utf-8"></script>
    <script src="../Content/lib/js/utils/inflector.js" type="text/javascript" charset="utf-8"></script>
    <script src="../Content/lib/js/templates/templates.js" type="text/javascript" charset="utf-8"></script>
    <script type="text/javascript" charset="utf-8">
        $(document).ready(function () {
//            //  txtcustomernameAutoCompleteExtenderKeyUp(); 
//            //  supplierAutoCompleteExtenderKeyUp();  
            AutoCompleteExtender_Line_KeyUp();
            CustomerAutoCompleteExtender_Line_KeyUp();
        });

    </script>
    <script language="javascript" type="text/javascript">
        function EndRequestUserControl(sender, args) {

            CustomerAutoCompleteExtender_Line_KeyUp();
            //  txtcustomernameAutoCompleteExtenderKeyUp(); Tanvir 08112022
            //   supplierAutoCompleteExtenderKeyUp(); Tanvir 08112022
           AutoCompleteExtender_Line_KeyUp();

            // after update occur on UpdatePanel re-init the Autocomplete

        }
//        //
        function Supplier_OnClientItemSelected(source, eventArgs) {
            if (eventArgs != null) {
                document.getElementById('<%=txtSupplierCode.ClientID%>').value = eventArgs.get_value();
            }
            else {
                document.getElementById('<%=txtSupplierCode.ClientID%>').value = '';
            }
        }

        function Supplier_OnClientPopulating(sender, args) {
         //  alert(document.getElementById('<%=txtsuppliercode.ClientID%>').value);
           document.getElementById('<%=txtsuppliercode.ClientID%>').value = '';
        }
        function Customer_OnClientPopulating(sender, args) {
                     document.getElementById('<%=txtcustomercode.ClientID%>').value = '';
        }
    
//        //T
        function supplierautocompleteselected(source, eventArgs) {
            if (eventArgs != null) {
                document.getElementById('<%=txtsuppliercode.ClientID%>').value = eventArgs.get_value();
                //alert(eventArgs.get_value());
            }
            else {
                document.getElementById('<%=txtsuppliercode.ClientID%>').value = '';
            }

        }
    

        function txtcustomernameautocompleteselected(source, eventArgs) {
            if (eventArgs != null) {
                document.getElementById('<%=txtcustomercode.ClientID%>').value = eventArgs.get_value();
              
            }
            else {
                document.getElementById('<%=txtcustomercode.ClientID%>').value = '';
            }

        }
//       //
        function AutoCompleteExtender_Line_KeyUp() {
            $("#<%= txtsuppliername.ClientID %>").bind("change", function () {
                var hiddenfieldID1 = document.getElementById('<%=txtsuppliername.ClientID%>');
                var hiddenfieldID = document.getElementById('<%=txtsuppliercode.ClientID%>');
                if (hiddenfieldID1.value == '') {
                    hiddenfieldID.value = '';
                }
            });

            $("#<%= txtsuppliername.ClientID %>").keyup(function (event) {
                var hiddenfieldID1 = document.getElementById('<%=txtsuppliername.ClientID%>');
                var hiddenfieldID = document.getElementById('<%=txtsuppliercode.ClientID%>');
                if (hiddenfieldID1.value == '') {
                    hiddenfieldID.value = '';
                }
            });
        }
        function CustomerAutoCompleteExtender_Line_KeyUp() {
            $("#<%= txtcustomername.ClientID %>").bind("change", function () {
                var hiddenfieldID1 = document.getElementById('<%=txtcustomername.ClientID%>');
                var hiddenfieldID = document.getElementById('<%=txtcustomercode.ClientID%>');
                if (hiddenfieldID1.value == '') {
                    hiddenfieldID.value = '';
                }
            });

            $("#<%= txtcustomername.ClientID %>").keyup(function (event) {
                var hiddenfieldID1 = document.getElementById('<%=txtcustomername.ClientID%>');
                var hiddenfieldID = document.getElementById('<%=txtcustomercode.ClientID%>');
                if (hiddenfieldID1.value == '') {
                    hiddenfieldID.value = '';
                }
            });
        }

//        // Tanvir 08112022
//        function supplierAutoCompleteExtenderKeyUp() {
//            $("#<%= txtsuppliername.ClientID %>").bind("change", function () {
//                document.getElementById('<%=txtsuppliercode.ClientID%>').value = '';
//            });
//        }
//        function txtcustomernameAutoCompleteExtenderKeyUp() {
//            $("#<%= txtcustomername.ClientID %>").bind("change", function () {
//                document.getElementById('<%=txtcustomercode.ClientID%>').value = '';
//            });
//        }
        var ddlcustcode = null;
        var ddlcustname = null;
        function validatePage() {
            var imgicon = document.getElementById("<%=imgicon.ClientID%>");
            imgicon.style.visibility = "visible";
            return true;
        }
        function FillCodeName(ddltp, ddlcode, ddlname, txtcd, txtnm) {
            ddltyp = document.getElementById(ddltp);
            ddlc = document.getElementById(ddlcode);
            ddln = document.getElementById(ddlname);
            txtcscode = document.getElementById(txtcd);
            txtcsname = document.getElementById(txtnm);
            if (ddltyp.value != '[Select]') {
                ddln.value = ddlc.options[ddlc.selectedIndex].text;
                txtcscode.value = ddlc.options[ddlc.selectedIndex].value;
                txtcsname.value = ddlc.options[ddlc.selectedIndex].text;
            }
            else {
                alert('Please Select Type');
                ddlc.value = '[Select]';
                ddln.value = '[Select]';
                txtcscode.value = "";
                txtcsname.value = "";
            }
        }
        function FillNameCode(ddltp, ddlcode, ddlname, txtcd, txtnm) {
            ddltyp = document.getElementById(ddltp);
            ddlc = document.getElementById(ddlcode);
            ddln = document.getElementById(ddlname);
            txtcscode = document.getElementById(txtcd);
            txtcsname = document.getElementById(txtnm);
            if (ddltyp.value != '[Select]') {
                ddlc.value = ddln.options[ddln.selectedIndex].text;
                txtcscode.value = ddln.options[ddln.selectedIndex].text;
                txtcsname.value = ddln.options[ddln.selectedIndex].value;
            }
            else {
                alert('Please Select Type');
                ddlc.value = '[Select]';
                ddln.value = '[Select]';
                txtcscode.value = "";
                txtcsname.value = "";
            }
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

            if (ddltyp.value != '[Select]') {
                lblcustcode.innerHTML = strcap + 'Code';
                lblcustname.innerHTML = strcap + 'Name';
                sqlstr1 = "select Code,des from view_account where type = '" + strtp + "' order by code";
                sqlstr2 = "select des,Code from view_account where type = '" + strtp + "' order by des";
            }
            else {
                lblcustcode.innerHTML = 'Code';
                lblcustname.innerHTML = 'Name';
                sqlstr1 = "select top 10  Code,des from view_account  order by code";
                sqlstr2 = "select top 10 des,Code from view_account  order by des";
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
 
     <%--<script type="text/javascript">
         var prm = Sys.WebForms.PageRequestManager.getInstance();
         prm.add_initializeRequest(InitializeRequestUserControl);
         prm.add_endRequest(EndRequestUserControl);

         function InitializeRequestUserControl(sender, args) {

         }
         function EndRequestUserControl(sender, args) {
          //   AutoCompleteSupplier_Supplier_KeyUp();
         }
         //Tanvir  08/11/2022     
</script>--%>

    <table style="border-right: gray 2px solid; border-top: gray 2px solid; border-left: gray 2px solid;
        border-bottom: gray 2px solid"  Width="100%">
        <tr>
            <td align="center" class="field_heading">
                <asp:Label ID="lblHeading" runat="server" CssClass="field_heading" Text=" " Width="100%"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <table width=100%>
                            <tbody>
                                <tr>
                                    <td>
                                        <table>
                                            <tbody>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="Label9" runat="server" Text="From Date" Width="110px" class="td_cell"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtFromDate" TabIndex="1" runat="server" Width="80px" CssClass="txtbox"></asp:TextBox><asp:ImageButton
                                                            ID="ImgBtnFrmDt" runat="server" 
                                                            ImageUrl="~/Images/Calendar_scheduleHS.png" TabIndex="1">
                                                        </asp:ImageButton><cc1:MaskedEditValidator ID="MEVFromDate" runat="server" CssClass="field_error"
                                                            TooltipMessage="Input a date in dd/mm/yyyy format" InvalidValueMessage="Invalid Date"
                                                            InvalidValueBlurredMessage="Invalid Date" EmptyValueMessage="Date is required"
                                                            EmptyValueBlurredText="Date is required" Display="Dynamic" ControlToValidate="txtFromDate"
                                                            ControlExtender="MEFromDate"></cc1:MaskedEditValidator>
                                                    </td>
                                                    <td style="width: 197px">
                                                        <asp:Label ID="lblTodate" runat="server" Text="To Date" ForeColor="Black" Width="110px"
                                                           class="td_cell"></asp:Label>
                                                    </td>
                                                    <td style="width: 126px">
                                                        <asp:TextBox ID="txtToDate" TabIndex="2" runat="server" Width="80px" CssClass="txtbox"></asp:TextBox><asp:ImageButton
                                                            ID="ImgBtnRevDate" runat="server" 
                                                            ImageUrl="~/Images/Calendar_scheduleHS.png" TabIndex="2">
                                                        </asp:ImageButton><cc1:MaskedEditValidator ID="MEVToDate" runat="server" CssClass="field_error"
                                                            TooltipMessage="Input a date in dd/mm/yyyy format" InvalidValueMessage="Invalid Date"
                                                            InvalidValueBlurredMessage="Invalid Date" EmptyValueMessage="Date is required"
                                                            EmptyValueBlurredText="Date is required" Display="Dynamic" ControlToValidate="txtToDate"
                                                            ControlExtender="METoDate"></cc1:MaskedEditValidator>
                                                    </td>
                                                    <td>
                                                        &nbsp;</td>
                                                </tr>
                                                <tr>
                                                    <td style="height: 35px" valign="top">
                                                        <asp:Label ID="lblType" runat="server" Text="Type" Width="110px" class="td_cell"></asp:Label>
                                                    </td>
                                                    <td style="height: 35px" valign="top">
                                                        <select style="width: 101px" id="ddlType" class="drpdown" tabindex="3" 
                                                            runat="server">
                                                            <option value="[Select]" selected>[Select]</option>
                                                            <option value="C">Customer</option>
                                                            <option value="S">Supplier</option>
                                                            <option value="A">Supplier Agent</option>
                                                        </select>
                                                    </td>
                                                    <td style="height: 35px; width: 197px;" valign="top">
                                                        <asp:Label ID="Label5" runat="server" Height="16px" Text="Customer" class="td_cell"
                                                            Width="81px"></asp:Label>
                                                            <asp:Label ID="lblsuptype" runat="server" class="td_cell" 
                                                                    Text="Supplier/Supplier Agent" Width="195px"></asp:Label>
                                                    </td>
                                                    <td style="height: 35px; width: 126px;" valign="top">
<%--                                                      <asp:TextBox ID="txtcustomername" runat="server"  CssClass="field_input"
                       MaxLength="500" TabIndex="4" Width="200px"></asp:TextBox>
                         <asp:TextBox ID="txtcustomercode" runat="server"  Width="100px"></asp:TextBox>  
                           <asp:HiddenField ID="HiddenField5" runat="server" />
                          <asp:AutoCompleteExtender ID="txtcustomername_AutoCompleteExtender" runat="server" CompletionInterval="10"
                           CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                            CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="1" DelimiterCharacters=""
                              EnableCaching="false" Enabled="True" FirstRowSelected="false" MinimumPrefixLength="0"
                               ServiceMethod="Getcustomer" TargetControlID="txtcustomername" OnClientItemSelected="txtcustomernameautocompleteselected" OnClientPopulating="Customer_OnClientPopulating">  
                               </asp:AutoCompleteExtender>
--%>
 <asp:TextBox ID="txtcustomername" runat="server" CssClass="field_input"
                                                                    Height="16px" MaxLength="500" TabIndex="4" Width="300px"></asp:TextBox>
                                            <asp:TextBox ID="txtcustomercode" runat="server"  style="display:none" ></asp:TextBox>
                                            <asp:AutoCompleteExtender ID="txtcustomername_AutoCompleteExtender" runat="server" CompletionInterval="10"
                                                CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="1"
                                                DelimiterCharacters="" EnableCaching="false" Enabled="True" FirstRowSelected="True"
                                                MinimumPrefixLength="-1" ServiceMethod="Getcustomer" TargetControlID="txtcustomername"
                                                OnClientItemSelected="txtcustomernameautocompleteselected"    OnClientPopulating="Customer_OnClientPopulating">  
                                            </asp:AutoCompleteExtender>
<%--                                <asp:TextBox ID="txtsuppliername" runat="server" CssClass="field_input"
                                                                    Height="16px" MaxLength="500" TabIndex="4" Width="248px"></asp:TextBox>
                                                                     <asp:TextBox ID="txtsuppliercode" runat="server"   Width="100px"></asp:TextBox>
                                                                <asp:AutoCompleteExtender ID="txtsuppliername_AutoCompleteExtender" runat="server"
                                                                    CompletionInterval="10" CompletionListCssClass="autocomplete_completionListElement"
                                                                    CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem" CompletionListItemCssClass="autocomplete_listItem"
                                                                    CompletionSetCount="1" DelimiterCharacters="" EnableCaching="false" Enabled="True"
                                                                    FirstRowSelected="false" MinimumPrefixLength="0"  OnClientItemSelected="supplierautocompleteselected"
                                                                  OnClientPopulating="Supplier_OnClientPopulating"  ServiceMethod="Getsupplierlist" TargetControlID="txtsuppliername">  
                                                                </asp:AutoCompleteExtender>
                                                                <asp:HiddenField ID="HiddenField2" runat="server" />
--%>
 
                                                                   <asp:TextBox ID="txtsuppliername" runat="server" CssClass="field_input"
                                                                    Height="16px" MaxLength="500" TabIndex="4" Width="300px"></asp:TextBox>
                                            <asp:TextBox ID="txtsuppliercode" runat="server"   style="display:none" ></asp:TextBox>
                                            <asp:AutoCompleteExtender ID="txtsuppliername_AutoCompleteExtender" runat="server" CompletionInterval="10"
                                                CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="1"
                                                DelimiterCharacters="" EnableCaching="false" Enabled="True" FirstRowSelected="True"
                                                MinimumPrefixLength="-1" ServiceMethod="Getsupplierlist" TargetControlID="txtsuppliername"
                                                OnClientItemSelected="supplierautocompleteselected"  OnClientPopulating="Supplier_OnClientPopulating"  >
                                            </asp:AutoCompleteExtender>
 

              <%--                                        <asp:TextBox ID="txtsuppliername" runat="server"  CssClass="field_input"
                       MaxLength="500" TabIndex="4" Width="200px"></asp:TextBox>
                         <asp:TextBox ID="txtsuppliercode" runat="server"  Width="100px"></asp:TextBox> 
                           <asp:HiddenField ID="HiddenField1" runat="server" />
                          <asp:AutoCompleteExtender ID="txtsuppliername_AutoCompleteExtender" runat="server" CompletionInterval="10"
                           CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                            CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="1" DelimiterCharacters=""
                              EnableCaching="false" Enabled="True" FirstRowSelected="false" MinimumPrefixLength="0"
                               ServiceMethod="Getcustomer" TargetControlID="txtsuppliername" OnClientItemSelected="txtcustomernameautocompleteselected">
                               </asp:AutoCompleteExtender>--%>
                                                    </td>
                                                    <td style="height: 35px" valign="top">
                                                        &nbsp;</td>
                                                </tr>
                              
                                                
                                                
                                                
                                                   <tr>
                                                            <td class="td_cell" colspan="2" style="height: 39px;border: Solid 1px grey; text-align: center">
                                                                <asp:Button ID="btnDisplayBill" runat="server" CssClass="btn" 
                                                                    OnClick="btnDisplayBill_Click" width="163.71" TabIndex="5" Text="Display Bills" />
                                                         
                                                            </td>
                                                            <td class="td_cell" style="width: 197px; height: 39px;">
                                                               
                                                                
                                                                      <img style="width: 183px; height: 21px" id="imgicon" src="../Images/loading.gif"
                                                            runat="server" />
                                                                      </td>
                                                            <td class="td_cell" 
                                                                style="height: 39px;border : Solid 1px grey; text-align: center;" colspan="2">
                                                                <asp:Button ID="btnDisplaySettle" runat="server" CssClass="btn" 
                                                                    OnClick="btnDisplaySettle_Click" TabIndex="7" Text="Display Settlements" />
                                                                &nbsp;
                                                            </td>
                                                        </tr>
                                              
                                                <tr >
                                                    <td style="border : Solid 1px grey">
                                                        <asp:Label ID="lblFromAmount"  runat="server" Text="Search Doc No" Width="119px" 
                                                            class="td_cell"></asp:Label>
                                                    </td>
                                                    <td style="border : Solid 1px grey">
                                                        <input style="width: 150px" id="txtBillTranId" class="txtbox" tabindex="5" type="text"
                                                            value=" " runat="server" />
                                                        <asp:Button ID="btnBillSearch" TabIndex="6" OnClick="btnBillSearch_Click" runat="server"
                                                            Text="Search" Font-Bold="True" CssClass="btn"></asp:Button>
                                                    </td>
                                                    <td style="text-align: right; width: 197px">
                                                        &nbsp;</td>
                                                    <td style="border : Solid 1px grey">
                                                        &nbsp;<asp:Label ID="lblToAmount" runat="server" class="td_cell" 
                                                            Text="Search Doc No" Width="119px"></asp:Label>
                                                    </td>
                                                    <td style="border : Solid 1px grey">
                                                        <input style="width: 150px" id="txtSettleTranId" class="txtbox" tabindex="8" type="text"
                                                            runat="server" />
                                                        <asp:Button ID="btnSettleSearch" runat="server" CssClass="btn" Font-Bold="True" 
                                                            OnClick="btnSettleSearch_Click" TabIndex="9" Text="Search" />
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 100%">
                                        <table>
                                            <tbody>
                                                <tr>
                                                    <td style="width: 45%" valign="top">
                                                        <asp:Panel ID="pnlBills" runat="server" Height="300px" ScrollBars="Both" Visible="False">
                                                            <asp:GridView ID="gv_BillSearchResult" TabIndex="12" runat="server" Font-Size="10px"
                                                                CssClass="grdstyle" GridLines="Vertical" CellPadding="3" BorderWidth="1px" BorderStyle="None"
                                                                BorderColor="#999999" AutoGenerateColumns="False">
                                                                <FooterStyle CssClass="grdfooter"></FooterStyle>
                                                                <Columns>
                                                                    <asp:TemplateField Visible="False" HeaderText="Doc No">
                                                                        <EditItemTemplate>
                                                                            &nbsp;
                                                                        </EditItemTemplate>
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblCode" runat="server" Text='<%# Bind("tranid") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:ButtonField Text="Settlements" CommandName="View">
                                                                        <ControlStyle ForeColor="Blue"></ControlStyle>
                                                                    </asp:ButtonField>
                                                                    <asp:TemplateField Visible="False">
                                                                        <ItemTemplate>
                                                                            <asp:CheckBox ID="chkBillSelection" runat="server" CssClass="chkbox"></asp:CheckBox>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:BoundField DataField="fileno" HeaderText="File No"></asp:BoundField>
                                                                    <asp:TemplateField SortExpression="trantype" HeaderText="Doc Type">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblBillDocType" runat="server" Text='<%# Bind("trantype") %>' CssClass="field_input"></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField SortExpression="tranid" HeaderText="Doc No">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblBillDocNo" runat="server" Text='<%# Bind("tranid") %>' CssClass="field_input"></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:BoundField HtmlEncode="False" DataFormatString="{0:dd/MM/yyyy}" DataField="tran_date"
                                                                        SortExpression="tran_date" HeaderText="Date"></asp:BoundField>
                                                                    <asp:TemplateField SortExpression="amount" HeaderText="Amount">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblBillAmout" runat="server" Text='<%# Bind("amount") %>' CssClass="field_input"></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField>
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblbillCrDr" runat="server" Text='<%# bind("DrCr") %>' CssClass="field_input"></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField Visible="False" HeaderText="tranlineno">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblBilltranlineno" runat="server" Text='<%# Bind("tranlineno") %>'
                                                                                CssClass="field_input"></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                                <RowStyle CssClass="grdRowstyle"></RowStyle>
                                                                <SelectedRowStyle CssClass="grdselectrowstyle"></SelectedRowStyle>
                                                                <PagerStyle CssClass="grdpagerstyle" HorizontalAlign="Center"></PagerStyle>
                                                                <HeaderStyle CssClass="grdheader"></HeaderStyle>
                                                                <AlternatingRowStyle CssClass="grdAternaterow"></AlternatingRowStyle>
                                                            </asp:GridView>
                                                        </asp:Panel>
                                                        <asp:Label ID="lblMsg" runat="server" Text="Records not found, Please redefine search criteria"
                                                            Font-Size="8pt" Font-Names="Verdana" Font-Bold="True" Width="357px" CssClass="lblmsg"
                                                            Visible="False"></asp:Label>
                                                    </td>
                                                    <td style="width: 55%" valign="top">
                                                        <asp:Panel ID="pnlSettlements" runat="server" Height="300px" ScrollBars="Both" Visible="False">
                                                            <asp:GridView ID="gv_SettleSearchResult" TabIndex="13" runat="server" Font-Size="10px"
                                                                CssClass="grdstyle" GridLines="Vertical" CellPadding="3" BorderWidth="1px" BorderStyle="None"
                                                                BorderColor="#999999" AutoGenerateColumns="False">
                                                                <FooterStyle CssClass="grdfooter"></FooterStyle>
                                                                <Columns>
                                                                    <asp:TemplateField Visible="False" HeaderText="Doc No">
                                                                        <EditItemTemplate>
                                                                            &nbsp;
                                                                        </EditItemTemplate>
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblCode" runat="server" Text='<%# Bind("tranid") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:ButtonField Text="Adjusted Bills " CommandName="View">
                                                                        <ControlStyle ForeColor="Blue"></ControlStyle>
                                                                    </asp:ButtonField>
                                                                    <asp:BoundField DataField="fileno" HeaderText="File No"></asp:BoundField>
                                                                    <asp:TemplateField SortExpression="trantype" HeaderText="Doc Type">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblSettleDocType" runat="server" Text='<%# Bind("trantype") %>' CssClass="field_input"></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField SortExpression="tranid" HeaderText="Doc No">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblSettleDocNo" runat="server" Text='<%# Bind("tranid") %>' CssClass="field_input"></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:BoundField HtmlEncode="False" DataFormatString="{0:dd/MM/yyyy}" DataField="tran_date"
                                                                        SortExpression="tran_date" HeaderText="Date"></asp:BoundField>
                                                                    <asp:TemplateField SortExpression="amount" HeaderText="Amount">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblSettleAmount" runat="server" Text='<%# Bind("amount") %>' CssClass="field_input"></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField>
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblSettleCrDr" runat="server" Text='<%# bind("DrCr") %>' CssClass="field_input"></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Cheque No">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblChequeno" runat="server" CssClass="field_input"></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField Visible="False" HeaderText="tranlineno">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblSettletranlineno" runat="server" Text='<%# Bind("tranlineno") %>'
                                                                                CssClass="field_input"></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField Visible="False">
                                                                        <ItemTemplate>
                                                                            <asp:CheckBox ID="chkSettleSelection" runat="server" CssClass="field_input"></asp:CheckBox>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                                <RowStyle CssClass="grdRowstyle"></RowStyle>
                                                                <SelectedRowStyle CssClass="grdselectrowstyle"></SelectedRowStyle>
                                                                <PagerStyle CssClass="grdpagerstyle" HorizontalAlign="Center"></PagerStyle>
                                                                <HeaderStyle CssClass="grdheader"></HeaderStyle>
                                                                <AlternatingRowStyle CssClass="grdAternaterow"></AlternatingRowStyle>
                                                            </asp:GridView>
                                                        </asp:Panel>
                                                        <asp:Label ID="lblMsg1" runat="server" Text="Records not found, Please redefine search criteria"
                                                            Font-Size="8pt" Font-Names="Verdana" Font-Bold="True" Width="357px" CssClass="lblmsg"
                                                            Visible="False"></asp:Label>
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <table style="width: 100%">
                                            <tr>
                                                <td align="left" style="width: 25%">
                                                    <asp:Label ID="lblSett" runat="server" Font-Size="8pt" Font-Names="Verdana" Font-Bold="True"
                                                        Width="222px" Visible="false" />
                                                </td>
                                                <td align="left" style="width: 25%">
                                                </td>
                                                <td align="left" style="width: 25%">
                                                    <asp:Label ID="lblAdj" runat="server" Font-Size="8pt" Font-Names="Verdana" Font-Bold="True"
                                                        Width="222px" Visible="false"></asp:Label>
                                                </td>
                                                <td align="right" style="width: 25%">
                                                    <input id="txtconnection" runat="server" style="visibility: hidden; width: 12px;
                                                        height: 9px" type="text" />
                                                    <input style="visibility: hidden; width: 5px" id="txtcustcode" type="text" maxlength="100"
                                                        runat="server" /><input style="visibility: hidden; width: 5px" id="txtcustname" type="text"
                                                            maxlength="200" runat="server" />
                                                    &nbsp; &nbsp;</td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr width="100%">
                                    <td style="text-align: center">
                                        <cc1:CalendarExtender ID="CEFromDate" runat="server" Format="dd/MM/yyyy" PopupButtonID="ImgBtnFrmDt"
                                            TargetControlID="txtFromDate">
                                        </cc1:CalendarExtender>
                                        <cc1:MaskedEditExtender ID="MEFromDate" runat="server" TargetControlID="txtFromDate"
                                            Mask="99/99/9999" MaskType="Date">
                                        </cc1:MaskedEditExtender>
                                        <cc1:CalendarExtender ID="CEToDate" runat="server" Format="dd/MM/yyyy" PopupButtonID="ImgBtnRevDate"
                                            TargetControlID="txtToDate">
                                        </cc1:CalendarExtender>
                                        <cc1:MaskedEditExtender ID="METoDate" runat="server" TargetControlID="txtToDate"
                                            Mask="99/99/9999" MaskType="Date">
                                        </cc1:MaskedEditExtender>
                                        <asp:Button ID="btnExit" runat="server" CausesValidation="False" CssClass="btn" 
                                            OnClick="btnExit_Click" TabIndex="14" Text=" Exit" />
                                        <asp:Button ID="btnClear" runat="server" CssClass="btn" Font-Bold="False" 
                                            OnClick="btnClear_Click" TabIndex="15" Text="Clear" />
                                        <asp:Button ID="btnhelp" runat="server" CssClass="btn" OnClick="btnhelp_Click" 
                                            TabIndex="16" Text="Help" />
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
    <asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server">
        <Services>
            <asp:ServiceReference Path="~/clsServices.asmx"></asp:ServiceReference>
        </Services>
    </asp:ScriptManagerProxy>
</asp:Content>
