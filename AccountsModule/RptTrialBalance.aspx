<%@ Page Language="VB" MasterPageFile="~/AccountsMaster.master" AutoEventWireup="false" CodeFile="RptTrialBalance.aspx.vb" Inherits="AccountsModule_RptTrialBalance"  %>

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

            controlacctAutoCompleteExtenderKeyUp();

        });

    </script>
    <script type="text/javascript">

        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_initializeRequest(InitializeRequestUserControl);
        prm.add_endRequest(EndRequestUserControl);


        function EndRequestUserControl(sender, args) {

            controlacctAutoCompleteExtenderKeyUp();


            // after update occur on UpdatePanel re-init the Autocomplete

        }
        function controlacctautocompleteselected(source, eventArgs) {
            if (eventArgs != null) {
                document.getElementById('<%=txtbankcode.ClientID%>').value = eventArgs.get_value();
            }
            else {
                document.getElementById('<%=txtbankcode.ClientID%>').value = '';
            }

        }

        function controlacctAutoCompleteExtenderKeyUp() {
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
        function enabledatectrl() {
            if (document.getElementById("<%=ddlwithmovmt.ClientID%>").value == 1)//without ason
            {
                document.getElementById("<%=label2.ClientID%>").innerText = "AsOnDate";
                document.getElementById("<%=label1.ClientID%>").style.display = 'none';
                document.getElementById("<%=txttoDate.ClientID%>").style.display = 'none';
                document.getElementById("<%=ImgBtntoDt.ClientID%>").style.display = 'none';
                return true;
            }
            else {
                document.getElementById("<%=label2.ClientID%>").innerText = "FromDate";
                document.getElementById("<%=label1.ClientID%>").style.display = '';
                document.getElementById("<%=txttoDate.ClientID%>").style.display = '';
                document.getElementById("<%=ImgBtntoDt.ClientID%>").style.display = '';
                return true;
            }
        }

        function fromtext(day, lday) {
            date = document.getElementById("<%=txtFromDate.ClientID%>").value;
            var txttdate = document.getElementById("<%=txttoDate.ClientID%>");
            if (date == '') { alert("Enter From Date."); }
            else { txttdate.value = date; }

            var month = date.substring(3, 5);
            var cday = date.substring(0, 2);

            //if(document.getElementById("<%=ddlwithmovmt.ClientID%>").value==1)
            //{           
            if ((month == day) && (cday == lday)) {
                document.getElementById("<%=Label3.ClientID%>").style.display = 'block';
                document.getElementById("<%=ddlclosing.ClientID%>").style.display = 'block';
            }
            else {
                document.getElementById("<%=Label3.ClientID%>").style.display = 'none';
                document.getElementById("<%=ddlclosing.ClientID%>").style.display = 'none';
            }

            //} 


        }


        function showclosing() {
            btn = document.getElementById("<%=btnclose.ClientID%>");
            btn.click()
        }

        function totext(day, lday) {

            //date=document.getElementById("<%=txtFromDate.ClientID%>").value; 
            date = document.getElementById("<%=txttoDate.ClientID%>").value;

            var month = date.substring(3, 5);
            var cday = date.substring(0, 2);

            if (document.getElementById("<%=ddlwithmovmt.ClientID%>").value == 0) {
                if ((month == day) && (cday == lday)) {
                    document.getElementById("<%=Label3.ClientID%>").style.display = 'block';
                    document.getElementById("<%=ddlclosing.ClientID%>").style.display = 'block';
                }
                else {
                    document.getElementById("<%=Label3.ClientID%>").style.display = 'none';
                    document.getElementById("<%=ddlclosing.ClientID%>").style.display = 'none';
                }

            }
        }


        function rbevent(rb1, rb2, Opt, Group) {
            var rb2 = document.getElementById(rb2);
            rb1.checked = true;
            rb2.checked = false;
            switch (Group) {



            }

            if (Opt == 'A') {

                ddlm1.value = '[Select]';
                ddlm2.value = '[Select]';
                ddlm3.value = '[Select]';
                ddlm4.value = '[Select]';

                ddlm1.disabled = true;
                ddlm2.disabled = true;
                ddlm3.disabled = true;
                ddlm4.disabled = true;

            }
            else {
                ddlm1.disabled = false;
                ddlm2.disabled = false;
                ddlm3.disabled = false;
                ddlm4.disabled = false;

            }

        }

</script>

 <%--<asp:UpdatePanel ID="UpdatePanel1" runat="server" >
    <contenttemplate>--%>
 
    <table style="width: 100%; height:100%; border-right: gray 2px solid; border-top: gray 2px solid; border-left: gray 2px solid; border-bottom: gray 2px solid;">
        <tr>
            <td class=" field_heading" colspan="2" style="text-align: center">
                GL Trial Balance</td>
        </tr>
        
        <tr>
            <td>
                 <asp:Panel ID="Panel1" runat="server" CssClass="td_cell" GroupingText="Select Date"
                    Height="166px" >
                <table style=" height: 150px">
                    <tr>
                        <td class="td_cell" style=" height: 26px;">
                            Report Type</td>
                        <td class="td_cell" style=" height: 26px; ">
                            <asp:DropDownList ID="ddlwithmovmt" runat="server" onchange="enabledatectrl()" 
                                Width="119px" CssClass="drpdown" TabIndex="1" Height="20px">
                                <asp:ListItem Value="0">Transaction</asp:ListItem>
                                <asp:ListItem Value="1">Balance</asp:ListItem>
                            </asp:DropDownList></td>
                                 <td class="td_cell" style=" height: 26px;">
                            </td>
                                     <td class="td_cell" style="height: 26px">
                            </td>
                    </tr>
                    <tr>
                        <td class="td_cell" style=" height: 26px;">
                            <asp:Label ID="Label2" runat="server">From Date</asp:Label></td>
                        <td class="td_cell" style="height: 26px; ">
                            <asp:TextBox ID="txtFromDate"   runat="server" CssClass="txtbox"  
                                TabIndex="2"   Width="114px"  Height="16px"></asp:TextBox>
                            <asp:ImageButton ID="ImgBtnFrmDt" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.png" TabIndex="3" />
                            <cc1:maskededitvalidator id="MskVFromDt" runat="server" controlextender="MskFromDate"
                                controltovalidate="txtFromDate" cssclass="field_error" display="Dynamic" emptyvalueblurredtext="*"
                                emptyvaluemessage="Date is required" errormessage="MskVFromDate" invalidvalueblurredmessage="*"
                                invalidvaluemessage="Invalid Date" tooltipmessage="Input a date in dd/mm/yyyy format"
                                width="1px"></cc1:maskededitvalidator></td>
                                   <td class="td_cell" style=" height: 26px;">
                                                <asp:Label ID="Label1" runat="server" Text="To Date" Width="71px"></asp:Label></td>
                        <td class="td_cell" style="height: 26px">
                                <asp:TextBox ID="txttoDate" runat="server" CssClass="txtbox" TabIndex="3" 
                                    Width="114px" style="margin-left: 0px" ></asp:TextBox>
                                <cc1:MaskedEditExtender ID="MsktoDate" runat="server" acceptnegative="Left" 
                                    displaymoney="Left" errortooltipenabled="True" mask="99/99/9999" 
                                    masktype="Date" messagevalidatortip="true" targetcontrolid="txttoDate">
                                </cc1:MaskedEditExtender>
                                <cc1:CalendarExtender ID="ClsExtoDate" runat="server" format="dd/MM/yyyy" 
                                    popupbuttonid="ImgBtntoDt" targetcontrolid="txttoDate">
                                </cc1:CalendarExtender>
                                &nbsp;
                                <asp:ImageButton ID="ImgBtntoDt" runat="server" 
                                    ImageUrl="~/Images/Calendar_scheduleHS.png" TabIndex="5" />
                                                         <cc1:maskededitvalidator id="MskVtoDt" runat="server" controlextender="MsktoDate"
                                controltovalidate="txttoDate" cssclass="field_error" display="Dynamic" emptyvalueblurredtext="*"
                                emptyvaluemessage="Date is required" errormessage="MskVFromDate" invalidvalueblurredmessage="*"
                                invalidvaluemessage="Invalid Date" tooltipmessage="Input a date in dd/mm/yyyy format"
                                width="1px"></cc1:maskededitvalidator>
                                </td>
                                 <td class="td_cell" style="height: 26px">
               
       </td>
                    </tr>
                    <tr>
                                                   <td class="td_cell" >
                            Select Level</td>
                               <td class="td_cell">
                            <asp:DropDownList ID="ddlselect" runat="server" CausesValidation="True" 
                                CssClass="drpdown" TabIndex="4" Height="22px" >
                            </asp:DropDownList></td>
                        <td class="td_cell"  >
                            <asp:Label ID="Label3" runat="server" Text="Closing Entry"></asp:Label></td>
                        <td class="td_cell" >
                            <asp:DropDownList ID="ddlclosing" runat="server" CssClass="drpdown" 
                                TabIndex="6" Height="16px" Width="126px">
                                <asp:ListItem Value="0">Include Closing Entry</asp:ListItem>
                                <asp:ListItem Value="1">Exclude Closing Entry</asp:ListItem>
                            </asp:DropDownList></td>
     
                    </tr>
                   
                </table>
                </asp:Panel> 
            </td>
          
             
           
        </tr>
        <tr><td></td>
        </tr>
        <tr><td>&nbsp;</td>
        </tr>
        <tr>
        <td class="td_cell">
          <asp:Panel ID="Panel2" runat="server" 
                GroupingText="Selection Criteria" style="width: 100%" 
                   >
                   
                    <table  >
                     <tr>
                                                            <td class="td_cell" style="height: 17px; ">
                                                                Control A\c</td>
                                                            <td class="td_cell" style="height: 17px; ">
                                                                <asp:TextBox ID="TxtBankName" runat="server" CssClass="field_input"
                                                                    MaxLength="500" TabIndex="5" Width="248px" Height="16px"></asp:TextBox>
                                                                <asp:HiddenField ID="HiddenField1" runat="server" />
                                                                <asp:AutoCompleteExtender ID="TxtBankName_AutoCompleteExtender" runat="server" CompletionInterval="10"
                                                                    CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                                    CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="1" DelimiterCharacters=""
                                                                    EnableCaching="false" Enabled="True" FirstRowSelected="false" MinimumPrefixLength="0"
                                                                    ServiceMethod="Getbankslist" TargetControlID="TxtBankName" OnClientItemSelected="controlacctautocompleteselected">
                                                                </asp:AutoCompleteExtender>
                                                                     </td>
                                                            <td class="td_cell" style="height: 17px">
                                                               <asp:TextBox ID="TxtBankCode" style="Display: None" runat="server" Width="100px"
                                                               ></asp:TextBox>
                                                            </td>
                                                            <td class="td_cell" style="height: 17px">
                                                                &nbsp;
                                                            </td>
                         
                        <tr>
                         <td colspan="2" style="Display: None">
                                                <input type="checkbox" id="chkTree" TabIndex="6" runat="server" />
                                                Tree Format
                                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                </td>
        </tr>
         <tr>
                 
                        <td class="td_cell" align="center">
                            <asp:Button ID="btnloadreport" runat="server"  style ="display:none" CssClass="btn" Text="Load Report" 
                                TabIndex="7" />&nbsp;
                                <asp:Button ID="btnPdfReport" runat="server" CssClass="btn" Text="Pdf Report" 
                                TabIndex="8" />&nbsp;
                                  <asp:Button ID="btnExcelReport" runat="server" CssClass="btn" Text="Excel Report" 
                                TabIndex="9" />
                            <asp:Button ID="btnhelp" runat="server" CssClass="btn" OnClick="btnhelp_Click"
                                TabIndex="10" Text="Help" />&nbsp;&nbsp;
                                      <asp:Button ID="Button1" runat="server" Style="Display:None" Text="Export" CssClass="btn" /></td>
                                                        <td class="td_cell">
                                                            &nbsp;</td> 
                                <td>
                                       <asp:Button ID="btnclose" runat="server" Text="close" CssClass="btn"  style="display:none"/></td>
                                       <td style="display:none">
               <asp:Button id="btnadd" tabIndex=16  runat="server"  
        CssClass="field_button"></asp:Button>
       </td>

<asp:GridView ID="gv_SearchResult" runat="server" AutoGenerateColumns="False" 
                                                BackColor="White" BorderColor="#999999" BorderStyle="None" 
                                               CssClass="td_cell" Font-Size="10px">
                                                 </asp:GridView>

                    </tr>
        <tr>
            <td>
          
                   
                </td>
       
           
        </tr>
        </table>
    <cc1:calendarextender id="ClsExFromDate" runat="server" format="dd/MM/yyyy" popupbuttonid="ImgBtnFrmDt"
        targetcontrolid="txtFromDate">
    </cc1:calendarextender>
    <cc1:maskededitextender id="MskFromDate" runat="server" acceptnegative="Left" displaymoney="Left"
        errortooltipenabled="True" mask="99/99/9999" masktype="Date" messagevalidatortip="true"
        targetcontrolid="txtFromDate"></cc1:maskededitextender>
    <asp:ScriptManagerProxy id="ScriptManagerProxy1" runat="server">
        <Services>
            <asp:ServiceReference Path="~/clsServices.asmx" />
        </Services>
    </asp:ScriptManagerProxy>

  <%-- </contenttemplate>
 </asp:UpdatePanel>--%>

</asp:Content>

