<%@ Page Title="" Language="VB" MasterPageFile="~/SubPageMaster.master" AutoEventWireup="false"
    CodeFile="AirportMATypes.aspx.vb" Inherits="PriceListModule_AirportMATypes" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
    <%@ OutputCache location="none" %> 
    <%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="server">

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
        $(document).ready(function () {
            sectorGroupAutoCompleteExtenderKeyUp();

        });
</script>

    <script language="javascript" type="text/javascript">

        function PopUpImageView(code) {
         
            var FileName = document.getElementById("<%=hdnFileName.ClientID%>");
            var lblfilename = document.getElementById("<%=txtimg.ClientID%>");
        
            if (FileName.value == "") {
                FileName.value = code;
            }

            if (lblfilename != "") {
               
                popWin = open('../PriceListModule/ImageViewWindow.aspx?code=' + FileName.value, 'myWin', 'resizable=yes,scrollbars=yes,width=500,height=350,left=450,top=80');
                popWin.focus();
                FileName.value = "";
                return false

            }
            else {

                popWin = open('../PriceListModule/ImageViewWindow.aspx?', 'myWin', 'resizable=yes,scrollbars=yes,width=500,height=350,left=450,top=80');
                popWin.focus();
            }
        }
        function Validate(state) {

            if (state == 'New' || state == 'Edit') {

                //             if (document.getElementById("<%=txtCode.ClientID%>").value == '') {
                //                 document.getElementById("<%=txtCode.ClientID%>").focus();
                //                 alert('Please Enter Code');
                //                 return false;
                //             } else 
                if (document.getElementById("<%=txtName.ClientID%>").value == '') {
                    document.getElementById("<%=txtName.ClientID%>").focus();
                    alert('Please Enter Name');
                    return false;
                }
                else {
                    if (state == 'New') { if (confirm('Are you sure you want to save this Type?') == false) return false; }
                    if (state == 'Edit') { if (confirm('Are you sure you want to update ?') == false) return false; }
                    if (state == 'Delete') { if (confirm('Are you sure you want to delete ?') == false) return false; }
                }

            }
        }

        function checkNumberdecimal(evt) {
            evt = (evt) ? evt : event;
            var charCode = (evt.charCode) ? evt.charCode : ((evt.keyCode) ? evt.keyCode :
        ((evt.which) ? evt.which : 0));
            if (charCode != 47 && (charCode > 44 && charCode < 58)) {
                return true;
            }
            return false;
        }

        function validateage() {
           
            var frmage = document.getElementById("<%=txtChildFrmAge.ClientID%>").value;
            var toage = document.getElementById("<%=txtChildToAge.ClientID%>").value;
                 if (toage != '') {
                if (parseFloat(toage) < parseFloat(frmage)) {
             
                    alert('Child To Age Should be Greater than From Age');
                    document.getElementById("<%=txtChildToAge.ClientID%>").value = '';
                    document.getElementById("<%=txtChildToAge.ClientID%>").focus();
                }
            }
        }
       
        function checkNumber(e) {

            if ((event.keyCode < 47 || event.keyCode > 57)) {
                return false;
            }

        }
        function checkCharacter(e) {
            if (event.keyCode == 32 || event.keyCode == 46)
                return;
            if ((event.keyCode < 47 || event.keyCode > 57) && (event.keyCode < 65 || event.keyCode > 91) && (event.keyCode < 96 || event.keyCode > 122)) {
                return false;
            }

        }

        function GetOtherGrpValueFrom() {

            var ddl = document.getElementById("<%=ddlOtherGrpName.ClientID%>");
            ddl.selectedIndex = -1;
            // Iterate through all dropdown items.
            for (i = 0; i < ddl.options.length; i++) {
                if (ddl.options[i].text ==
                document.getElementById("<%=ddlOtherGrpCode.ClientID%>").value) {
                    // Item was found, set the selected index.
                    ddl.selectedIndex = i;

                    return true;
                }
            }

        }
        function GetOtherGrpValueCode() {
            var ddl = document.getElementById("<%=ddlOtherGrpCode.ClientID%>");
            ddl.selectedIndex = -1;
            // Iterate through all dropdown items.
            for (i = 0; i < ddl.options.length; i++) {
                if (ddl.options[i].text ==
			document.getElementById("<%=ddlOtherGrpName.ClientID%>").value) {
                    // Item was found, set the selected index.
                    ddl.selectedIndex = i;

                    return true;
                }
            }


        }


        function CallWebMethod(methodType) {
            switch (methodType) {


                case "acccode":
                    var select = document.getElementById("<%=ddlAccCode.ClientId%>");
                    var selectname = document.getElementById("<%=ddlAccName.ClientId%>");
                    selectname.value = select.options[select.selectedIndex].text;
                    break;
                case "accname":
                    var select = document.getElementById("<%=ddlAccName.ClientId%>");
                    var selectname = document.getElementById("<%=ddlAccCode.ClientId%>");
                    selectname.value = select.options[select.selectedIndex].text;
                    break;
                case "accrualcode":
                    var select = document.getElementById("<%=ddlAccrualCode.ClientId%>");
                    var selectname = document.getElementById("<%=ddlAccrualname.ClientId%>");
                    selectname.value = select.options[select.selectedIndex].text;
                    break;
                case "accrualname":
                    var select = document.getElementById("<%=ddlAccrualname.ClientId%>");
                    var selectname = document.getElementById("<%=ddlAccrualCode.ClientId%>");
                    selectname.value = select.options[select.selectedIndex].text;
                    break;


            }
        }

        function prefpartyautocompleteselected(source, eventArgs) {
            if (eventArgs != null) {
                document.getElementById('<%=txtPrefPartyCode.ClientID%>').value = eventArgs.get_value();
            }
            else {
                document.getElementById('<%=txtPrefPartyCode.ClientID%>').value = '';
            }

        }
        function sectorGroupAutoCompleteExtenderKeyUp() {
            $("#<%= txtPrefPartyname.ClientID %>").bind("change", function () {

                if (document.getElementById('<%=txtPrefPartyname.ClientID%>').value == '') {

                    document.getElementById('<%=txtPrefPartyCode.ClientID%>').value = '';
                }

            });

            $("#<%= txtPrefPartyname.ClientID %>").keyup(function (event) {

                if (document.getElementById('<%=txtPrefPartyname.ClientID%>').value == '') {

                    document.getElementById('<%=txtPrefPartyCode.ClientID%>').value = '';
                }

            });
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



        function lockcontrols(ddlrate) 
        {

            if (ddlrate.value == '0') {
                document.getElementById('<%=TxtCompAdult.ClientID%>').value = '';
                document.getElementById('<%=txtcompchild.ClientID%>').value = '';
                document.getElementById('<%=TxtCompAdult.ClientID%>').setAttribute("disabled", true);
                document.getElementById('<%=txtcompchild.ClientID%>').setAttribute("disabled", true);

                document.getElementById('<%=ddlAddPaxChkReqd.ClientID%>').style.display = 'none';
                document.getElementById('<%=lbladdpaxreqd.ClientID%>').style.display = 'none';

                document.getElementById('<%=txtPaxForUnit.ClientID%>').style.display = 'none';// Added by Abin on 17/04/2018
                document.getElementById('<%=lblPaxForUnit.ClientID%>').style.display = 'none';

                      }

            else if (ddlrate.value == '1') {
                document.getElementById('<%=ddlAddPaxChkReqd.ClientID%>').style.display = 'block';
                document.getElementById('<%=lbladdpaxreqd.ClientID%>').style.display = 'block';
                  document.getElementById('<%=TxtCompAdult.ClientID%>').removeAttribute("disabled");
                  document.getElementById('<%=txtcompchild.ClientID%>').removeAttribute("disabled");

                  document.getElementById('<%=txtPaxForUnit.ClientID%>').style.display = 'block'; // Added by Abin on 17/04/2018
                  document.getElementById('<%=lblPaxForUnit.ClientID%>').style.display = 'block';
            }
        }





    

    </script>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <Triggers>
            <asp:PostBackTrigger ControlID="btnSave" />
        </Triggers>
        <ContentTemplate>
    <TABLE style="BORDER-RIGHT: gray 2px solid; BORDER-TOP: gray 2px solid; BORDER-LEFT: gray 2px solid; WIDTH: 950px; BORDER-BOTTOM: gray 2px solid">

                <tbody>
                   
                            <tr>
                                <td class="td_cell"style=" height: 24px; color: #000000;">
                                    <asp:Label ID="lblHeading" runat="server" align="center" CssClass="field_heading" 
                                        ForeColor="White" Text="Add New AirportMATypes" Width="100%"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                            <td>
                            <table>
                            <tbody>
                            <tr>
                                <td class="td_cell" style="HEIGHT: 24px;  ">
                                    Code <span class="td_cell" style="COLOR: red">*</span></td>
                                <td style="COLOR: #000000; HEIGHT: 24px;  ">
                                    <INPUT id="txtCode" class="txtbox" tabIndex=1 type=text maxLength=20 
                               runat="server" />
                                </td>
                                <td class="td_cell" style="HEIGHT: 24px; ">
                                    Name <span style="COLOR: red">*</span>
                                </td>
                                <td colspan="2">
                                    <INPUT id="txtName"   class="field_input"  tabIndex=2 type=text maxLength=150 style="width:300px" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td class="td_cell" style="HEIGHT: 24px; " >
                                    Group Code &nbsp;<span style="color: red">*&nbsp;&nbsp;</span>
                                </td>
                                <td class="td_cell" >
                                    <span style="color: #ff0000">
                                    <select ID="ddlOtherGrpCode" runat="server" class="field_input" 
                                        onchange="GetOtherGrpValueFrom()" style="width:122px" tabindex="3">
                                        <option selected=""></option>
                                    </select></span>
                                </td>
                                <td class="td_cell" >
                                    Group&nbsp;Name&nbsp;&nbsp;
                                </td>
                                <td class="td_cell" colspan="2">
                                    <select ID="ddlOtherGrpName" runat="server" class="field_input" 
                                        onchange="GetOtherGrpValueCode()"  tabindex="4" style="width:300px;">
                                        <option selected=""></option>
                                    </select>
                                </td>
                            </tr>
                            <tr>
                                <td   style="HEIGHT: 24px; " class="td_cell" align="left" >
                                    Min Pax <span style="color: #ff0000">*</span>
                                </td>
                                <td class="td_cell"  >
                                    <input   tabindex="5" style=" text-align: right; width:81px;" id="txtMinPax"  type="text"
                                                runat="server" class="field_input" />


                                </td>
                                <td align="left" class="td_cell"  >
                                    Max Pax&nbsp; <span style="color: #ff0000">*</span>&nbsp;&nbsp;
                                </td>
                                <td class="td_cell" colspan="2">
                                    <input  tabindex="6" style="text-align: right ;width:81px;" id="txtMaxPax" type="text"
                                                runat="server" class="field_input" />
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td class="td_cell" colspan="2" style="HEIGHT: 24px; ">
                                    <input id="ChkAutoCancel" tabindex="7" type="checkbox" checked 
                                                    runat="server" />
                                    &nbsp;&nbsp; Auto Cancellation&nbsp; Required</td>
                                <td class="td_cell" style="HEIGHT: 24px; ">
                                    &nbsp;</td>
                                <td class="td_cell" colspan="2"  >
                                    &nbsp;</td>
                            </tr>
                            <tr>
                                <td class="td_cell" style="HEIGHT: 24px; ">
                                    Rates based on
                                </td>
                                <td >
                                    <select ID="ddlRateType" runat="server" name="D7" 
                                        onchange="lockcontrols(this);" tabindex="8">
                                        <option value="0">Adult/Child</option>
                                        <option value="1">Unit</option>
                                    </select></td>
                                <td class="td_cell" style="HEIGHT: 24px; ">
                                    &nbsp;<asp:Label ID="lbladdpaxreqd" runat="server" Width="120px"
                                        text=" Additional Pax Charge Required "></asp:Label>
                                </td>
                                <td class="td_cell" >
                                    &nbsp;<select ID="ddlAddPaxChkReqd" runat="server"  style="width :83px" class="field_input" name="D6" 
                                        tabindex="9">
                                        <option value="0">No</option>
                                        <option value="1">Yes</option>
                                    </select>
                                    &nbsp;&nbsp;
                                </td>
                                <td class="td_cell" style="width: 218px"><%--Added by abin on 20180418--%>
                                <div style="width:200px;"><div style="width:100px;float:left;padding-left:15px;"><asp:Label ID="lblPaxforUnit" runat="server" text="Pax for Unit"></asp:Label></div> 
                                <div style="width:76px;float:left;">  <input style="width:75px; text-align: right" tabindex="10" 
                                        id="TxtPaxForUnit"    type="text"
                                                runat="server" class="field_input"  /></div></div>
                                    
                             </td>
                            </tr>
                            <tr>
                                <td class="td_cell" style="height:24px;">
                                    Compulsory Adults
                                </td>
                                <td class="td_cell" style=" height:24px; ">
                                    <input style="width: 81px; text-align: right" tabindex="10" id="TxtCompAdult"    type="text"
                                                runat="server" class="field_input"  />
                                </td>
                                <td   class="td_cell">
                                    Compulsory Child</td>
                                <td class="td_cell" colspan="2" >
                                    <input style=" text-align: right; width:81px;"  tabindex="11" 
                                id="TxtCompChild"   type="text"
                                                runat="server" class="field_input"   />
                                </td>
                            </tr>
                            <tr>
                                <td class="td_cell">
                                    Child From Age</td>
                                <td class="td_cell" style=" height: 24px; ">
                                    <input style=" text-align: right; width:81px;" id="txtChildFrmAge" tabindex="12" type="text"
                                                runat="server" class="field_input"   
                                onchange="validateage();"  />
                                </td>
                                <td class="td_cell">
                                    ChildTo Age</td>
                                <td class="td_cell" colspan="2">
                                    <input style="text-align: right; width:81px;" id="txtChildToAge" 
                                             tabindex="13" type="text"
                                            onchange="validateage();"    runat="server" 
                                class="field_input" />
                                </td>
                               
                            </tr>
                            <tr>
                                <td style=" height: 24px;" class="td_cell">
                                    Type <span style="color: #ff0000">*</span> &nbsp;
                                </td>
                                <td class="td_cell" style="height: 30px; ">
                                    <select ID="ddltype" runat="server" class="field_input" name="D1" 
                                        style="width:122px" tabindex="14">
                                        <option value="0">[Select]</option>
                                        <option value="1">Arrival</option>
                                        <option value="2">Departure</option>
                                        <option value="3">Arrival or Departure</option>
                                        <option value="4">Transit</option>
                                    </select></td>
                                <td class="td_cell" style=" height: 30px;">
                                    <input  tabindex="15" 
                                id="ChkInactive"   type="checkbox"
                                                runat="server" checked   />
                                    Active</td>
                                <td class="td_cell" style="height:24px; " colspan="2">
                                </td>
                            </tr>
                            <tr>
                                <td class="td_cell" style="height: 24px;">
                                    Select Preferred Supplier <span style="color: #ff0000">*</span>
                                </td>
                                <td class="td_cell" style=" height:24px; " align="left" colspan="2" 
                                    valign="top">
                                    <asp:TextBox ID="txtprefpartyname" runat="server" CssClass="field_input" 
                                        MaxLength="200" TabIndex="17" Width="250px"></asp:TextBox>
                                    <asp:HiddenField ID="hdnprefparty" runat="server" />
                                    <asp:AutoCompleteExtender ID="Party_AutoCompleteExtender" runat="server" 
                                        CompletionInterval="10" 
                                        CompletionListCssClass="autocomplete_completionListElement" 
                                        CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem" 
                                        CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="1" 
                                        DelimiterCharacters="" EnableCaching="false" Enabled="True" 
                                        FirstRowSelected="false" MinimumPrefixLength="0" 
                                        OnClientItemSelected="prefpartyautocompleteselected" 
                                        ServiceMethod="GetPrefPartylist" TargetControlID="txtprefpartyname">
                                    </asp:AutoCompleteExtender>
                                </td>
                                <td colspan="2">
                                    <asp:TextBox ID="txtprefpartycode" runat="server" style="display:none"></asp:TextBox>
                                </td>
                            </tr>
                       </tbody> 
                       </table> 
                       </td> 
                       </tr>   
                   
                  <tr> 
                     <td>
                          <table  >
                              <tbody>
                                  <tr>
                                      <td colspan="2">
                                          <asp:Panel ID="Panel" runat="server" BorderStyle="None" Height="200px" 
                                              ScrollBars="Vertical">
                                              <br />
                                              <asp:GridView ID="grdairports" runat="server" AllowSorting="True" 
                                                  AutoGenerateColumns="False" BorderStyle="None" BorderWidth="1px" 
                                                  CellPadding="3" CssClass="grdstyle" Font-Size="10px" GridLines="Vertical" 
                                                  >
                                                  <FooterStyle CssClass="grdfooter" />
                                                  <Columns>
                                                      <asp:TemplateField HeaderText="Select">
                                                          <ItemTemplate>
                                                              <input id="chk" type="checkbox" runat="server" tabindex="18" />
                                                          </ItemTemplate>
                                                      </asp:TemplateField>
                                                      <asp:BoundField DataField="airportbordercode" HeaderText="Airport Borders">
                                                          <ItemStyle HorizontalAlign="Left" />
                                                      </asp:BoundField>
                                                      <asp:BoundField DataField="airportbordername" HeaderText="Name ">
                                                          <ItemStyle HorizontalAlign="Left" />
                                                      </asp:BoundField>
                                                  </Columns>
                                                  <RowStyle CssClass="grdRowstyle" />
                                                  <SelectedRowStyle CssClass="grdselectrowstyle" />
                                                  <PagerStyle CssClass="grdpagerstyle" HorizontalAlign="Center" />
                                                  <HeaderStyle CssClass="grdheader" />
                                                  <AlternatingRowStyle CssClass="grdAternaterow" />
                                              </asp:GridView>
                                          </asp:Panel>
                                      </td>
                                  </tr>
                                  <tr>
                                      <td>
                                          <asp:Button ID="Btnselectall" runat="server" CssClass="btn" TabIndex="19" 
                                              Text="SelectAll" />
                                      </td>
                                      <td>
                                          <asp:Button ID="Btnunselectall" runat="server" CssClass="btn" TabIndex="20" 
                                              Text="Un Select" />
                                      </td>
                                  </tr>
                              </tbody>
                          </table>
                      
                          </td>     
                    </tr>

                 <tr> 
                            <td>
                                
                                <table>
                                    <tbody>
                                        <tr>
                                            <td class="td_cell" style="width: 389px">
                                                Image</td>
                                            <td align="left" valign="top">
                                                <asp:FileUpload ID="fileVehicleImage" TabIndex="21"  runat="server" />(203 X151)
                                                &nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td class="td_cell" style=" height: 22px; width: 389px;">
                                                </td>
                                            <td align="left" valign="top" style="height: 22px">
                                                <input style="width: 329px" id="txtimg"  readonly ="true"
    TabIndex="22"  type="text" maxlength="30" runat="server" />
                                      <asp:Button ID="btnViewimage" runat="server" CssClass="field_button" 
                                                                                Text="View" Width="64px" />
                                                                            &nbsp;<asp:Button ID="Btnremove" runat="server" CssClass="field_button" Text="Remove" 
                                                                                Width="77px" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="td_cell" style=" height: 73px; width: 389px;">
                                                Info For Online</td>
                                            <td align="left" valign="top">
                                                <asp:TextBox ID="txtRemark" runat="server" __designer:wfdid="w20" 
                                                    CssClass="field_input" Height="111px" TabIndex="23" TextMode="MultiLine" 
                                                    Width="620px"></asp:TextBox>
                                                
                                            </td>
                                        </tr>
                                      
                                    </tbody>
                                </table>
                                </td>
                                </tr>
                                  <tr>
                                            <td class="td_cell" style=" height: 73px; " colspan="2">
                                                <asp:Button ID="btnSave" runat="server" CssClass="field_button" 
                                                    OnClick="btnSave_Click" TabIndex="24" Text="Save" />
                                                <asp:Button ID="btnCancel" runat="server" CssClass="field_button" 
                                                    OnClick="btnCancel_Click" TabIndex="25" Text="Return To Search" />
                                                &nbsp;&nbsp;&nbsp;
                                             <asp:Button id="btnhelp" 
        tabIndex=26 onclick="btnhelp_Click" runat="server" Text="Help" 
        CssClass="field_button" ></asp:Button>
                                              
                                            </td>
                                          
                                        </tr>
      <tr style="display: none;">
                    <td style="width: 121px">
                        IncomeCode &nbsp;<span class="td_cell" style="color: red">*</span></td>
                    <td style="width: 173px">
                        <span class="td_cell" style="color: red"><span style="color: #ff0000">
                        <select ID="ddlAccCode" runat="server" class="field_input" name="D2" 
                            onchange="CallWebMethod('acccode')" onclick="return ddlAccCode_onclick()" 
                            style="width: 102px" tabindex="3">
                            <option selected=""></option>
                        </select></span></span></td>
                    <td colspan="2">
                        <select ID="ddlAccName" runat="server" class="field_input" name="D3" 
                            onchange="CallWebMethod('accname')" style="width: 180px" tabindex="4">
                            <option selected=""></option>
                        </select></td>
                    <td style="width: 91px">
                        &nbsp;</td>
                    </tr>
                   
              
              
              
                             <tr style="display: none;">
                        <td style="display: none;">
                            Expense Code<span class="td_cell" style="color: red">*&nbsp;&nbsp;</span>
                            <span style="color: #ff0000">
                            <select ID="ddlAccrualCode" runat="server" class="field_input" name="D4" 
                                onchange="CallWebMethod('accrualcode')" style="width: 102px" tabindex="3">
                                <option selected=""></option>
                            </select></span></td>
                        <td style="display: none;">
                            &nbsp;<select ID="ddlAccrualName" runat="server" class="field_input" name="D5" 
                                onchange="CallWebMethod('accrualname')" style="width: 180px" tabindex="4">
                                <option selected=""></option>
                            </select></td>
                        <td style="width: 3px; height: 4px">
                                 </td>
                        <td style="width: 4px; height: 4px;">
                            &nbsp; </td>
                    </tr>
                          
   </tbody>    
                 </TABLE> 
        </ContentTemplate>
    </asp:UpdatePanel>
        <asp:TextBox ID="hdnFileName" Text="" runat="server" Style="display: none" />
    <asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server">
        <Services>
            <asp:ServiceReference Path="~/clsServices.asmx" />
        </Services>
    </asp:ScriptManagerProxy>
</asp:Content>
