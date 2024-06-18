<%@ Page Language="VB" MasterPageFile="~/AccountsMaster.master" AutoEventWireup="false" CodeFile="closingyear.aspx.vb" Inherits="AccountsModule_closingyear"  %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>

        <%@ Register Assembly="EclipseWebSolutions.DatePicker" Namespace="EclipseWebSolutions.DatePicker"
    TagPrefix="ews" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">


 <script type="text/javascript" charset="utf-8">
     $(document).ready(function () {
              txtacnameAutoCompleteExtenderKeyUp();
          });
</script>

<script language="javascript" type="text/javascript">


    function txtacnameautocompleteselected(source, eventArgs) {
        if (eventArgs != null) {
            document.getElementById('<%=txtaccode.ClientID%>').value = eventArgs.get_value();
        }
        else {
            document.getElementById('<%=txtaccode.ClientID%>').value = '';
        }
    }
    function txtacnameAutoCompleteExtenderKeyUp() {
        $("#<%=txtaccode.ClientID %>").bind("change", function () {
            document.getElementById('<%=txtaccode.ClientID%>').value = '';
        });
    }


function ChangeDate()
{
   
     var txtfdate=document.getElementById("<%=txtFromDate.ClientID%>");
         if (txtfdate.value=='')
     {
     alert("Enter From Date.");txtfdate.focus();  
     }
}

	function disp_confirm()
    {
        var r=confirm("Are you sure to close the year?");
        return r
    }


</script>

   <script type="text/javascript">
       var prm = Sys.WebForms.PageRequestManager.getInstance();
       prm.add_endRequest(EndRequestUserControl);
       function EndRequestUserControl(sender, args) {
           txtacnameAutoCompleteExtenderKeyUp();

       }
</script>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table style="border-right: gray 2px solid; border-top: gray 2px solid; border-left: gray 2px solid; border-bottom: gray 2px solid">
                <tr>
                    <td align="center" class="field_heading" style="width: 100%; height: 21px">
                        <asp:Label ID="lblHeading" runat="server" CssClass="field_heading" Text="Closing Year"
                            Width="388px"></asp:Label></td>
                </tr>
                <tr>
                    <td style="width: 100px; height: 65px;">
            <table style="width: 671px">
                <tr>
                    <td style="width: 333px; height: 24px;">
                        <asp:Label ID="Label1" runat="server" Text="Closing Date" Width="84px" 
                            CssClass="td_cell"></asp:Label></td>
                    <td style="width: 2041px; height: 24px;">
                        &nbsp;<asp:TextBox ID="txtFromDate" runat="server" CssClass="txtbox" 
                            Width="80px"></asp:TextBox>
                        <asp:ImageButton ID="ImgBtnFrmDt" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.png" />
                        <cc1:maskededitvalidator id="MEVFromDate" runat="server" controlextender="MEFromDate"
                            controltovalidate="txtFromDate" cssclass="field_error" display="Dynamic" emptyvalueblurredtext="Date is required"
                            emptyvaluemessage="Date is required" invalidvalueblurredmessage="Invalid Date"
                            invalidvaluemessage="Invalid Date" tooltipmessage="Input a date in dd/mm/yyyy format"></cc1:maskededitvalidator>
                    </td>
                    <td style="width: 190px; height: 24px;">
                    </td>
                </tr>
                 
                 <tr>
                   <td style="width: 140px" align="left">
                  <asp:Label ID="lblType" runat="server" Text="Transfer" Width="130px"></asp:Label>
                    </td>
                        <td align="left" valign="top" colspan="2" width="300px">
                      <asp:TextBox ID="txtacname" runat="server"  CssClass="field_input"
                       MaxLength="500" TabIndex="3" Width="300px"></asp:TextBox>
                         <asp:TextBox ID="txtaccode" runat="server"></asp:TextBox>
                           <asp:HiddenField ID="hdnpartycode" runat="server" />
                          <asp:AutoCompleteExtender ID="txtacname_AutoCompleteExtender" runat="server" CompletionInterval="10"
                           CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                            CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="1" DelimiterCharacters=""
                              EnableCaching="false" Enabled="True" FirstRowSelected="false" MinimumPrefixLength="0"
                               ServiceMethod="Getactype" TargetControlID="txtacname" OnClientItemSelected="txtacnameautocompleteselected">
                               </asp:AutoCompleteExtender>
                                  <input style="display: none" id="Text1" class="field_input" type="text" runat="server" />
                                       <input style="display: none" id="Text2" class="field_input" type="text" runat="server" />
                                        </td>
                                       </tr>
             
               
                
            </table>
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px">
                        <table style="width: 668px">
                            <tr>
                                <td style="width: 100px; height: 128px;">
                                    <div id="DIV1" runat="server" 
                                        style="width: 663px; height: 112px; background-color: lavender" class="td_cell">
                                    </div>
                                </td>
                            </tr>
                        </table>
                        <table style="width: 444px">
                            <tr>
                                <td style="width: 100px">
                                </td>
                                <td style="width: 100px">
                                    <asp:Button ID="btnseal" runat="server" CssClass="btn" Text="Close Year" OnClick="btnseal_Click" 
                                        OnClientClick="return disp_confirm();" /></td>
                                <td style="width: 100px">
                                    <asp:TextBox ID="txtpdate" runat="server" Visible="False" Width="1px"></asp:TextBox>
                                    <input id="txtDivCode" runat="server" maxlength="20" style="visibility: hidden; width: 5px"
                                        type="text" /></td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px">
                        <cc1:calendarextender id="CEFromDate" runat="server" format="dd/MM/yyyy" popupbuttonid="ImgBtnFrmDt"
                            targetcontrolid="txtFromDate"></cc1:calendarextender>
                        <cc1:maskededitextender id="MEFromDate" runat="server" targetcontrolid="txtFromDate" AcceptNegative="Left" DisplayMoney="Left" ErrorTooltipEnabled="True" Mask="99/99/9999" MaskType="Date" MessageValidatorTip="true"></cc1:maskededitextender>
                    </td>
                </tr>
            </table>
            <asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server">
                <Services>
                    <asp:ServiceReference Path="~/clsServices.asmx" />
                </Services>
            </asp:ScriptManagerProxy>
        </ContentTemplate>
    </asp:UpdatePanel>  
 
</asp:Content>

