<%@ Page Language="VB" MasterPageFile="~/AccountsMaster.master" AutoEventWireup="false"
    CodeFile="RptReceiptsRegister.aspx.vb" Inherits="RptReceiptsRegister" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ OutputCache Location="none" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="EclipseWebSolutions.DatePicker" Namespace="EclipseWebSolutions.DatePicker"
    TagPrefix="ews" %>
<asp:Content ID="Content2" ContentPlaceHolderID="Main" runat="Server">
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
    <script src="../Content/vendor/jquery.ui.position.js" type="text/javascript" charset="utf-8"></scrip>t
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

            controlacctAutoCompleteExtenderKeyUp();
            bankAutoCompleteExtenderKeyUp();

        });

    </script>
    <script type="text/javascript">

        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_initializeRequest(InitializeRequestUserControl);
        prm.add_endRequest(EndRequestUserControl);



        function EndRequestUserControl(sender, args) {


            controlacctAutoCompleteExtenderKeyUp();
            bankAutoCompleteExtenderKeyUp();

            // after update occur on UpdatePanel re-init the Autocomplete

        }

    </script>
    <script language="javascript" type="text/javascript">
        function controlacctautocompleteselected(source, eventArgs) {
            if (eventArgs != null) {
                document.getElementById('<%=txtcontacccode.ClientID%>').value = eventArgs.get_value();
            }
            else {
                document.getElementById('<%=txtcontacccode.ClientID%>').value = '';
            }

        }
        function bankautocompleteselected(source, eventArgs) {
            if (eventArgs != null) {
                document.getElementById('<%=txtbankcode.ClientID%>').value = eventArgs.get_value();
            }
            else {
                document.getElementById('<%=txtbankcode.ClientID%>').value = '';
            }

        }
        function controlacctAutoCompleteExtenderKeyUp() {
            $("#<%= txtcontaccname.ClientID %>").bind("change", function () {

                if (document.getElementById('<%=txtcontaccname.ClientID%>').value == '') {

                    document.getElementById('<%=txtcontacccode.ClientID%>').value = '';
                }

            });

            $("#<%= txtcontaccname.ClientID %>").keyup(function (event) {

                if (document.getElementById('<%=txtcontaccname.ClientID%>').value == '') {

                    document.getElementById('<%=txtcontacccode.ClientID%>').value = '';
                }

            });
        }


        function bankAutoCompleteExtenderKeyUp() {
            $("#<%= txtbankname.ClientID %>").bind("change", function () {

                if (document.getElementById('<%=txtbankname.ClientID%>').value == '') {

                    document.getElementById('<%=txtbankcode.ClientID%>').value = '';
                }

            });

            $("#<%= txtbankname.ClientID %>").keyup(function (event) {

                if (document.getElementById('<%=txtbankname.ClientID%>').value == '') {

                    document.getElementById('<%=txtbankcode.ClientID%>').value = '';
                }

            });
        }
        //----------------------------------------
        var ddlAcCode = null;
        var ddlAcName = null;

      
        function FillCode(ddlIccd, ddlIcnm) {
            ddlIccode = document.getElementById(ddlIccd);
            ddlIcname = document.getElementById(ddlIcnm);
            var codeid = ddlIccode.options[ddlIccode.selectedIndex].text;
            ddlIcname.value = codeid;
        }
        //----------------------------------------
        function FillName(ddlIccd, ddlIcnm) {
            ddlIccode = document.getElementById(ddlIccd);
            ddlIcname = document.getElementById(ddlIcnm);

            var codeid = ddlIcname.options[ddlIcname.selectedIndex].text;
            ddlIccode.value = codeid;
        }
        function FillBankCashDet(ddlcashbn, ddlAccCd, ddlAccNm) {

          
        }

        function FillBankCodes(result) {

            var ddlbankcd = ddlAcCode;
            RemoveAll(ddlbankcd)
            for (var i = 0; i < result.length; i++) {

                var option = new Option(result[i].ListText, result[i].ListValue);
                ddlbankcd.options.add(option);
            }
            ddlbankcd.value = "[Select]";

        }
        function FillBankNames(result) {
            var ddlbankcd = ddlAcName;
            RemoveAll(ddlbankcd)
            for (var i = 0; i < result.length; i++) {

                var option = new Option(result[i].ListText, result[i].ListValue);
                ddlbankcd.options.add(option);
            }
            ddlbankcd.value = "[Select]";
        }

        function ChangeDate() {

            var txtfdate = document.getElementById("<%=txtFromDate.ClientID%>");
            if (txtfdate.value == '') { alert("Enter From Date."); txtfdate.focus(); }
            else { ColServices.clsServices.GetQueryReturnFromToDate('FromDate', 30, txtfdate.value, FillToDate, ErrorHandler, TimeOutHandler); }
        }
        function FillToDate(result) {
            var txttdate = document.getElementById("<%=txtToDate.ClientID%>");
            txttdate.value = result;
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

        //----------------------------------------
    </script>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table style="border-right: gray 1px solid; border-top: gray 1px solid; border-left: gray 1px solid;
                border-bottom: gray 1px solid" class="td_cell" align="left" Width="100%">
                <tbody>
                    <tr>
                        <td class="field_heading" align="center">
                            <asp:Label ID="lblHeading" runat="server" Text="Report Receipts Register" CssClass="field_heading"
                                Width="100%"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="td_cell" align="left">
                            <table  Width="100%">
                                <tbody>
                                <tr>
                                      <td Width="100%">
                          <asp:Panel id="Panel5" runat="server" CssClass="td_cell"  Width="100%" __designer:wfdid="w22" GroupingText="Select">
                          <table >
                          <tr>
                          
                          
                          <td> <asp:Label ID="Label1" runat="server" Text="  Doc No." 
                                  CssClass="field_caption" Width="69px"></asp:Label></td>
                          <td style="width: 169px">  <asp:TextBox ID="txtTranId" TabIndex="1" runat="server" CssClass="txtbox" Width="79px"
                                                MaxLength="20"></asp:TextBox></td>
                                                <td> 
                                                    <asp:Label ID="Label13" runat="server" CssClass="field_caption" Text="Status" 
                                                Width="66px"></asp:Label></td>
                                                <td><select ID="ddlStatus" runat="server" class="dropdown" name="D2" 
                                                style="width: 79px" tabindex="2">
                                                <option selected="" value="[Select]">[Select]</option>
                                                <option value="P">Posted</option>
                                                <option value="U">UnPosted</option>
                                            </select></td>
                                            

                          </tr>
                          <tr>
                          
                          <td>  
                              <asp:Label ID="Label4" runat="server" CssClass="field_caption" 
                                                Text="  From" Width="79px"></asp:Label></td>
                                                <td style="width: 169px"><asp:TextBox ID="txtFromDate" runat="server" CssClass="txtbox" 
                                                TabIndex="3" ValidationGroup="MKE" Width="80px">10/09/2008</asp:TextBox>
                                            <asp:ImageButton ID="ImgBtnFrmDt" runat="server" 
                                                ImageUrl="~/Images/Calendar_scheduleHS.png" TabIndex="4" />
                                            <cc1:MaskedEditValidator ID="MskVFromDt" runat="server" 
                                                ControlExtender="MskFromDate" ControlToValidate="txtFromDate" 
                                                CssClass="field_error" Display="Dynamic" EmptyValueBlurredText="*" 
                                                EmptyValueMessage="Date is required" ErrorMessage="MskVFromDate" 
                                                InvalidValueBlurredMessage="*" InvalidValueMessage="Invalid Date" 
                                                TooltipMessage="Input a date in dd/mm/yyyy format" ValidationGroup="MKE" 
                                                Width="23px">
                                            </cc1:MaskedEditValidator></td>
                                            <td> 
                                                <asp:Label ID="Label8" runat="server" CssClass="field_caption" 
                                                Text="  To" Width="67px"></asp:Label></td>
                                                <td><asp:TextBox ID="txtTodate" runat="server" CssClass="txtbox" TabIndex="4" 
                                                ValidationGroup="MKE" Width="80px">10/09/2008</asp:TextBox>
                                            <asp:ImageButton ID="ImageButton1" runat="server" 
                                                ImageUrl="~/Images/Calendar_scheduleHS.png" TabIndex="5" />
                                            <cc1:MaskedEditValidator ID="MaskedEditValidator1" runat="server" 
                                                ControlExtender="MskChequeDate" ControlToValidate="txtTodate" 
                                                CssClass="field_error" Display="Dynamic" EmptyValueBlurredText="*" 
                                                EmptyValueMessage="Date is required" ErrorMessage="MskVFromDate" 
                                                InvalidValueBlurredMessage="*" InvalidValueMessage="Invalid Date" 
                                                TooltipMessage="Input a date in dd/mm/yyyy format" ValidationGroup="MKE" 
                                                Width="23px">
                                            </cc1:MaskedEditValidator></td>
                          
                          </tr>
                          <tr><td>  
                              <asp:Label ID="Label11" runat="server" Text="Report Type" CssClass="field_caption"
                                                Width="77px"></asp:Label></td><td style="width: 169px">
                                            <asp:DropDownList ID="ddlrpttype" runat="server" CssClass="dropdown" 
                                                TabIndex="5" Width="80px" Height="16px">
                                                <asp:ListItem Value="0">Brief</asp:ListItem>
                                                <asp:ListItem Value="1">Detailed</asp:ListItem>
                                            </asp:DropDownList></td></tr>
                          
                          
                          </table>

                          </asp:Panel>
                                
                                </td>
                                
                                
                                
                                </tr>
                                   
                                    <tr><td>
                                <asp:Panel id="Panel1" runat="server" CssClass="td_cell"  Width="100%" __designer:wfdid="w22" GroupingText="Accounts">
                                    
                                    <table Width="100%">
                                    
                                    <tr>
                                    
                                    <td class="td_cell" style="height: 17px; width: 121px;">
                                                               <asp:TextBox ID="TxtcontaccCode" runat="server" style="Display:none" Width="100px"
                                                               ></asp:TextBox>
                                                                <asp:Label ID="Label2" runat="server" Text=" Account Type" Width="110px"></asp:Label>
                                                            </td>
                                                            <td class="td_cell" style="height: 17px">

                                                                <select ID="ddlType" runat="server" class="dropdown" name="D3" 
                                                                    style="width: 124px" tabindex="6">
                                                                    <option selected="" value="[Select]">[Select]</option>
                                                                    <option value="C">Cash</option>
                                                                    <option value="B">Bank</option>
                                                                </select></td>
                                                                <td></td>
                                 
                                    </tr>
                                      <tr>
                                         <td class="td_cell" style="height: 17px; width: 127px;">
                                                                Account 
                                                            </td>
                                                           
                                                            <td class="td_cell" style="height: 17px; width: 260px;">
                                                                <asp:TextBox ID="TxtcontaccName" runat="server" CssClass="field_input"
                                                                    MaxLength="500" TabIndex="7" Width="189px" Height="17px"></asp:TextBox>
                                                                <asp:HiddenField ID="HiddenField1" runat="server" />
                                                                <asp:AutoCompleteExtender ID="TxtcontaccName_AutoCompleteExtender" runat="server" CompletionInterval="10"
                                                                    CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                                    CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="1" DelimiterCharacters=""
                                                                    EnableCaching="false" Enabled="True" FirstRowSelected="false" MinimumPrefixLength="0"
                                                                    ServiceMethod="Getcontacclist" TargetControlID="TxtcontaccName" OnClientItemSelected="controlacctautocompleteselected">
                                                                </asp:AutoCompleteExtender>
                                                                     </td>
                                        <td style="width: 121px; height: 17px;" class="td_cell">
                                            Bank</td>
                                        <td style="width: 260px; height: 17px;" class="td_cell">
                                   <asp:TextBox ID="txtbankname" runat="server" CssClass="field_input" 
                                                Height="16px" MaxLength="500" TabIndex="8" Width="190px"></asp:TextBox>
                                            <asp:HiddenField ID="HiddenField2" runat="server" />
                                            <asp:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" 
                                                CompletionInterval="10" 
                                                CompletionListCssClass="autocomplete_completionListElement" 
                                                CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem" 
                                                CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="1" 
                                                DelimiterCharacters="" EnableCaching="false" Enabled="True" 
                                                FirstRowSelected="false" MinimumPrefixLength="0" 
                                                OnClientItemSelected="bankautocompleteselected" ServiceMethod="Getbankslist" 
                                                TargetControlID="txtbankname">
                                            </asp:AutoCompleteExtender>
                                        </td>
                                        <td class="td_cell" style="height: 17px">
                                            <asp:TextBox ID="txtbankcode" runat="server" style="Display:None" Width="100px"></asp:TextBox>
                                        </td>
                                       
                                    </tr>
                                     <tr>
                                        <td style="width: 121px">
                                            <asp:Label ID="Label5" runat="server" CssClass="field_caption" 
                                                Text="From Amount" Width="131px"></asp:Label>
                                        </td>
                                        <td style="width: 260px">
                                            <asp:TextBox ID="txtFromRecvAmt" runat="server" CssClass="txtbox" 
                                                MaxLength="20" TabIndex="9"  Width="190px"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:Label ID="Label9" runat="server" CssClass="field_caption" 
                                                Text="To Amount" Width="110px"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtToRecvAmt" runat="server" CssClass="txtbox" MaxLength="20" 
                                                TabIndex="10"  Width="190px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    
                                    </table>
                                    </asp:Panel>
                                    </td></tr>
                                   
                                  
                                    <tr>
                                        <td style="width: 127px">
                                         
                                         
                                        </td>
                                        <td style="width: 260px">
                                            &nbsp;</td>
                                        <td>
                                            <asp:Label ID="Label12" runat="server" Text="Report Choice" Visible="False" 
                                                Width="96px"></asp:Label>
                                        </td>
                                        <td>
                                            <select ID="DDLchoice" runat="server" class="dropdown" name="D1" 
                                                style="width: 105px" visible="false">
                                                <option selected="selected" value="0">Cash</option>
                                                <option value="1">Bank</option>
                                                <option value="2">Deposit</option>
                                            </select></td>
                                    </tr>
                                </tbody>
                            </table>
                            <input id="txtconnection" runat="server" style="visibility: hidden; width: 12px;
                                height: 9px" type="text" />
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Button ID="btnLoadReprt" TabIndex="11" OnClick="btnLoadReprt_Click" runat="server"
                                Text="Load Report" CssClass="btn"></asp:Button>&nbsp;
                          
                            <asp:Button ID="btnhelp" TabIndex="14" OnClick="btnhelp_Click" runat="server" Text="Help"
                                CssClass="btn"></asp:Button> 
                                 <asp:Button ID="btnClear" TabIndex="12" visible=false OnClick="btnClear_Click" runat="server" Text="Clear"
                                Font-Bold="True" CssClass="btn"></asp:Button>&nbsp;<asp:Button ID="btnExit" TabIndex="13"
                                    OnClick="btnExit_Click"  Visible=false runat="server" Text=" Exit" CssClass="btn" CausesValidation="False">
                                </asp:Button>&nbsp;
                        </td>
                    <td style="display:none">
               <asp:Button id="btnadd" tabIndex=16  runat="server"  
        CssClass="field_button"></asp:Button>
         <asp:Button id="Button1" tabIndex=16 runat="server" 
        CssClass="field_button"></asp:Button></td>
       </td>

<asp:GridView ID="gv_SearchResult" runat="server" AutoGenerateColumns="False" 
                                                BackColor="White" BorderColor="#999999" BorderStyle="None" 
                                               CssClass="td_cell" Font-Size="10px">
                                                 </asp:GridView>
                    </tr>
                    <tr>
                        <td>
                            <cc1:MaskedEditExtender ID="MskFromDate" runat="server" TargetControlID="txtFromDate"
                                MaskType="Date" Mask="99/99/9999" MessageValidatorTip="true" ErrorTooltipEnabled="True"
                                DisplayMoney="Left" AcceptNegative="Left">
                            </cc1:MaskedEditExtender>
                            <cc1:MaskedEditExtender ID="MskChequeDate" runat="server" TargetControlID="txtTodate"
                                MaskType="Date" Mask="99/99/9999" MessageValidatorTip="true" ErrorTooltipEnabled="True"
                                DisplayMoney="Left" AcceptNegative="Left">
                            </cc1:MaskedEditExtender>
                            <cc1:CalendarExtender ID="ClExChequeDate" runat="server" Format="dd/MM/yyyy" PopupButtonID="ImageButton1"
                                TargetControlID="txtTodate">
                            </cc1:CalendarExtender>
                            <cc1:CalendarExtender ID="ClsExFromDate" runat="server" Format="dd/MM/yyyy" PopupButtonID="ImgBtnFrmDt"
                                TargetControlID="txtFromDate">
                            </cc1:CalendarExtender>
                        </td>
                    </tr>
                  
                </tbody>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server">
        <Services>
            <asp:ServiceReference Path="~/clsServices.asmx"></asp:ServiceReference>
        </Services>
    </asp:ScriptManagerProxy>
</asp:Content>
