<%@ Page Language="VB" MasterPageFile="~/SubPageMaster.master" AutoEventWireup="false"
    CodeFile="ExcOccHtWtMaster.aspx.vb" Inherits="ExcOccHtWtMaster" %>

<%@ Register Src="../PriceListModule/SubMenuUserControl.ascx" TagName="SubMenuUserControl"
    TagPrefix="uc1" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ OutputCache Location="none" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="server">
    <script language="javascript" type="text/javascript">
 function checkNumber()
{
   if (event.keyCode < 45 || event.keyCode > 57)
    {
            return false;
    }
}
    function formmodecheck() {
        var vartxtcode = document.getElementById("<%=txtCode.ClientID%>");
        if (vartxtcode.value == '') {
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
                l[i].onclick = function () { return; };
    }

  
    function FormValidation(state) {
        if (state == "Edit") {
            if (document.getElementById("<%=txtCode.ClientID%>").value == "") {
                document.getElementById("<%=txtCode.ClientID%>").focus();
                alert("Code field can not be blank");
                return false;
            }
        }

        if ((document.getElementById("<%=txtName.ClientID%>").value == "") || (document.getElementById("<%=ddlSupplierType.ClientID%>").value == "[Select]") || (document.getElementById("<%=ddlchildallowed.ClientID%>").value!= "[Select]")) {

         if (document.getElementById("<%=ddlchildallowed.ClientID%>").value!= "[Select]"){
            if (document.getElementById("<%=txtName.ClientID%>").value == "") {
                document.getElementById("<%=txtName.ClientID%>").focus();
                alert("Name field can not be blank");
                return false;
            }

            else if (document.getElementById("<%=ddlchildallowed.ClientID%>").value == "Yes") {
                if (document.getElementById("<%=txtchildagefrm.ClientID%>").value == "") {
                    document.getElementById("<%=txtchildagefrm.ClientID%>").focus();
                alert("Please Enter Child Age From.");
                return false;
            }
            if (document.getElementById("<%=txtchildageto.ClientID%>").value == "") {
                document.getElementById("<%=txtchildageto.ClientID%>").focus();
                alert("Please Enter Child Age To.");
                return false;
            }
            if (document.getElementById("<%=txtmaxchild.ClientID%>").value == "") {
                document.getElementById("<%=txtmaxchild.ClientID%>").focus();
                alert("Please Enter Max Child.");
                return false;
            }
            }
            }
            elseif (document.getElementById("<%=ddlsrcitizen.ClientID%>").value != "[Select]"))
            {
                if (document.getElementById("<%=txtsrcitizenage.ClientID%>").value == "") {
                document.getElementById("<%=txtsrcitizenage.ClientID%>").focus();
                alert("Please Enter Sr Citizen Age.");
                return false;
            }
           }
     
           return true;
            if (state == 'New') { if (confirm('Are you sure you want to save Excursion Type?') == false) return false; }
            if (state == 'Edit') { if (confirm('Are you sure you want to update Excursion Type?') == false) return false; }
            if (state == 'Delete') { if (confirm('Are you sure you want to delete Excursion Type?') == false) return false; }
        }
    }

   


    

    </script>
  
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table style="border-right: gray 2px solid; border-top: gray 2px solid; border-left: gray 2px solid;
                width: 800px; border-bottom: gray 2px solid" class="td_cell" align="left">
                <tbody>
                    <tr>
                        <td valign="top" align="center" width="150" colspan="2">
                            <asp:Label ID="lblHeading" runat="server" Text="Excursion -Occupancy Height/Weight" ForeColor="White"
                                Width="800px" CssClass="field_heading"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 15%" valign="top" align="left">
                            <span style="color: #ff0000" class="td_cell"></span>
                        </td>
                        <td style="width: 85%;" class="td_cell" valign="top" align="left">
                            Code <span style="color: #ff0000" class="td_cell">&nbsp; </span>
                            <asp:TextBox ID="txtCode" ReadOnly="true" runat="server" Width="190px" Height="16px"></asp:TextBox>
                            &nbsp; Name&nbsp;
                            <asp:TextBox ID="txtName" TabIndex="1" runat="server" CssClass="field_input" MaxLength="100"
                                Width="275px" Height="16px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 15%" valign="top" align="left">
                            <span style="color: #ff0000" class="td_cell"></span>
                        </td>
                        <td style="width: 85%;" class="td_cell" valign="top" align="left">
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td align="left" style="width: 15%" valign="top">
                            &nbsp;<uc1:SubMenuUserControl ID="SubMenuUserControl1" runat="server" />
                        </td>
                        <td style="width: 85%" valign="top">
                            <div id="iframeINF" runat="server" style="width: 705px">
                                <asp:Panel ID="PanelMain" runat="server" GroupingText="Occupancy Height/Weight Details" 
                                    Width="652px">
                                    <table style="width: 644px; margin-right: 0px;">
                                        <tbody>
                                            <%--<TD style="HEIGHT: 17px" class="td_cell" align=center colSpan=4><asp:Label id="lblHeading" runat="server" Text="Add New Supplier Category" ForeColor="White" CssClass="field_heading" Width="100%"></asp:Label></TD></TR>--%>
                                            <tr style="display: none">
                                                <td style="width: 3775px; height: 16px" class="td_cell">
                                                    Rate Basis&nbsp;
                                                </td>
                                                <td style="height: 16px" colspan="2">
                                                    <select id="ddlSupplierType" runat="server" class="drpdown" onchange="GetSpTypeValueFrom()"
                                                        style="width: 128px" tabindex="3">
                                                        <option selected=""></option>
                                                    </select>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                </td>
                                                <td style="width: 1170px">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td   class="td_cell" height="13px">
                                                    Min Pax Per Booking</td>
                                                <td    align="left" valign="top" colspan="2" height="13px">
                                                    <asp:TextBox ID="txtminpax" runat="server" Height="16px" 
                                                        onkeypress="return checkNumber()" style="TEXT-ALIGN: right" tabindex="4" 
                                                        Width="120px"></asp:TextBox>
                                                </td>
                                                <td >
                                                    &nbsp;</td>
                                                <td  >
                                                    &nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td class="td_cell" height="13px">
                                                    Max Pax Per Booking
                                                </td>
                                                <td align="left" colspan="2" height="13px" valign="top">
                                                    <asp:TextBox ID="txtmaxpax" runat="server" Height="16px" 
                                                        onkeypress="return checkNumber()" style="TEXT-ALIGN: right" tabindex="4" 
                                                        Width="120px"></asp:TextBox>
                                                </td>
                                                <td>
                                                </td>
                                                <td>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="4">
                                                    <span style="color: #ff0000; font: small-caption">(Leave blank if max pax checking not
                                                        required)</span>
                                                </td>
                                            </tr>
                                              <tr>
                        <td style=" height: 16px" class="td_cell">
                            Child Allowed <span style="color: #ff0000"></span>
                        </td>
                        <td align="left" valign="top" colspan="2">
                            <asp:DropDownList ID="ddlchildallowed"  tabindex="5" runat="server" AutoPostBack="true"  
                                Width="124px" Height="19px">
                                <asp:ListItem Text="[Select]">
                                </asp:ListItem>
                                <asp:ListItem Text="YES" Value="Yes">
                                </asp:ListItem>
                                <asp:ListItem Text="NO" Value="No">
                                </asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                                            <tr>
                                                <td class="td_cell" style="  height: 16px">
                                                    Max Child <span style="color: #ff0000"></span>
                                                </td>
                                                <td align="left" colspan="2" valign="top" tabindex="6">
                                                    <asp:TextBox ID="txtmaxchild"  tabindex="6"  style="TEXT-ALIGN: right"  onkeypress="return checkNumber()" runat="server" Height="16px" Width="120px"></asp:TextBox>
                                                </td>
                                                <td  >
                                                </td>
                                                <td  >
                                                </td>
                                                <tr>
                                                    <td style="  height: 16px" class="td_cell">
                                                        Child Charge From<span style="color: #ff0000"></span>
                                                    </td>
                                                    <td align="left" valign="top" colspan="2" style="height: 16px">
                                                        <asp:TextBox ID="txtchildagefrm"  style="TEXT-ALIGN: right"  tabindex="7"  onkeypress="return checkNumber()" runat="server" Height="16px" Width="120px"></asp:TextBox>
                                                    </td>
                                                    <td class="td_cell" style=" height: 16px">
                                                        To<span style="color: #ff0000"></span>
                                                    </td>
                                                    <td align="left" valign="top"  >
                                                        <asp:TextBox ID="txtchildageto"  style="TEXT-ALIGN: right" tabindex="8"  onkeypress="return checkNumber()" runat="server" Height="16px" Width="119px"></asp:TextBox>
                                                    </td>
                                                    <tr>
                                                        <td class="td_cell" style=" height: 16px">
                                                            Min. Height
                                                        </td>
                                                        <td align="left" colspan="2" valign="top">
                                                            <asp:TextBox ID="txtchildminht"  style="TEXT-ALIGN: right"   tabindex="9"  onkeypress="return checkNumber()" runat="server" Height="16px" Width="120px"></asp:TextBox>
                                                        </td>
                                                        <td  >
                                                            Max. Height
                                                        </td>
                                                        <td  >
                                                            <asp:TextBox ID="txtchildmaxht"   style="TEXT-ALIGN: right"    tabindex="10" onkeypress="return checkNumber()" runat="server" Height="16px" Width="120px"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                            <tr>
                                                <td colspan="4">
                                                    <span style="color: #ff0000; font: small-caption">(leave blank if Min Height not required)</span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="td_cell" style="  height: 16px">
                                                    Min. Weight<span style="color: #ff0000"></span>
                                                </td>
                                                <td align="left" colspan="2" valign="top" style="height: 16px">
                                                    <asp:TextBox ID="txtchildminwt" runat="server" Height="16px" 
                                                        onkeypress="return checkNumber()"   tabindex="11"  style="TEXT-ALIGN: right" Width="120px"></asp:TextBox>
                                                </td>
                                                <td class="td_cell" style=" height: 16px">
                                                    Max. Weight<span style="color: #ff0000"> </span>
                                                </td>
                                                <td style=" height: 16px">
                                                    <asp:TextBox ID="txtchildmaxwt"   tabindex="12"   onkeypress="return checkNumber()" style="TEXT-ALIGN: right"  runat="server" Height="16px" Width="120px"></asp:TextBox>
                                                </td>
                                            </tr>
                                             <tr>
                                                <td colspan="4">
                                                    <span style="color: #ff0000; font: small-caption">(Leave blank if  Min Weight  not
                                                        required)</span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="td_cell" style="height: 16px">
                                                    Senior Citizens Allowed
                                                </td>
                                                <td align="left" colspan="2" valign="top" style="height: 16px">
                                                    <asp:DropDownList ID="ddlsrcitizen"  tabindex="13" runat="server" AutoPostBack="true"  
                                                        Width="125px" Height="20px">
                                                        <asp:ListItem Text="[Select]">
                                                        </asp:ListItem>
                                                        <asp:ListItem Text="YES" Value="Yes">
                                                        </asp:ListItem>
                                                        <asp:ListItem Text="NO" Value="No">
                                                        </asp:ListItem>
                                                 </asp:DropDownList>
                                                </td>
                                                <td style="height: 16px; ">
                                                </td>
                                                <td style=" height: 16px">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="td_cell" style="  height: 22px;">
                                                    Senior Citizens From Age</td>
                                                <td style="  height: 22px;">
                                                    <asp:TextBox ID="txtsrcitizenage" tabindex="14" runat="server" Height="16px" 
                                                        onkeypress="return checkNumber()"  style="TEXT-ALIGN: right"  Width="120px"></asp:TextBox>
                                                </td>
                                                <td  >
                                                </td>
                                                <td  >
                                                </td>
                                            </tr>
                                                    <tr>
                                                        <td class="td_cell" style="  height: 22px;">
                                                            &nbsp;</td>
                                                        <td style="  height: 22px;">
                                                            &nbsp;</td>
                                                        <td>
                                                            &nbsp;</td>
                                                        <td>
                                                            &nbsp;</td>
                                                    </tr>
                                                    <tr>
                                                        <td class="td_cell" style="  height: 22px;">
                                                            &nbsp;</td>
                                                        <td style="  height: 22px;">
                                                            &nbsp;<asp:Button ID="btnSave" tabindex="16" runat="server" CssClass="btn"  
                                                                Text="Save" />
                                                            &nbsp;&nbsp;
                                                            <asp:Button ID="btnhelp0" runat="server" __designer:wfdid="w14" CssClass="btn" 
                                                                OnClick="btnhelp_Click" TabIndex="17" Text="Help" />
                                                        </td>
                                                        <td  >
                                                            &nbsp;</td>
                                                        <td  >
                                                            <asp:Button ID="btnCancel" runat="server" CssClass="btn" tabindex="15" 
                                                                Text="Return To Search" />
                                                        </td>
                                                    </tr>
                                            <tr>
                                                <td style= "height: 22px">
                                                    &nbsp;
                                                    </td>
                                                <td style=" height: 22px">
                                                    &nbsp;
                                                </td>
                                                <td style="  height: 22px">
                                                    &nbsp;
                                                </td>
                                                <td style="  height: 22px">
                                                </td>
                        </td>
                        <script language="javascript">
                            load();
                            formmodecheck();
                        </script>
                    </tr>
                    </tr>
                    <tr>
                        <td style="height: 16px" class="td_cell" colspan="3">
                            &nbsp;</td>
                    </tr>
                  
                </tbody>
            </table>
            </asp:Panel> </div> </td> </tbody> </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
