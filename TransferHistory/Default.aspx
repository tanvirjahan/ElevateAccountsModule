<%@ Page Language="VB" MasterPageFile="~/MainPageMaster.master" AutoEventWireup="false"
    CodeFile="Default.aspx.vb" Inherits="TransferHistory_Default" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="EclipseWebSolutions.DatePicker" Namespace="EclipseWebSolutions.DatePicker"
    TagPrefix="ews" %>
<%@ OutputCache Location="none" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="Server">
    <script src="../Content/js/jquery-1.11.2.min.js" type="text/javascript"></script>
    <script type="text/javascript">

        function confirmation() {
            if (window.confirm("Are You Sure?")) {
                var btnsrc = document.getElementById("<%=btnSearch.ClientID %>");
                btnsrc.click();                
            } else {
                return false;
            }
        }
        function validatePage() {
            var imgicon = document.getElementById("<%=imgicon.ClientID%>");
            imgicon.style.visibility = "visible";
            return true;
        } 
    </script>
    <table>
        <tr>
            <td>
                <asp:DropDownList ID="ddlTransferOption" runat="server" Width="104px" CssClass="drpdown"
                    AutoPostBack="True">
                    <asp:ListItem Text="- Select -" Value="0"></asp:ListItem>
                    <asp:ListItem Text="Price List" Value="1"></asp:ListItem>
                    <asp:ListItem Text="Child Policy New" Value="2"></asp:ListItem>
                    <asp:ListItem Text="Cancellation Policy" Value="3"></asp:ListItem>
                    <asp:ListItem Text="Block Full Sales" Value="4"></asp:ListItem>
                    <asp:ListItem Text="Compulsory Remarks" Value="5"></asp:ListItem>
                    <asp:ListItem Text="Minimum Nights" Value="6"></asp:ListItem>
                    <asp:ListItem Text="Promotion" Value="7"></asp:ListItem>
                    <asp:ListItem Text="Special Events Price List" Value="8"></asp:ListItem>
                    <asp:ListItem Text="Allotments" Value="9"></asp:ListItem>
                </asp:DropDownList>
            </td>
            <td style="padding-top: 25px">
                <ews:DatePicker ID="dpFromDate" runat="server" DateFormatString="dd/MM/yyyy" RegExErrorMessage="Please enter a date in the format: dd/mm/yyyy"
                    TabIndex="8" Visible="True" Width="185px" />
            </td>
            <td>
                <input type="button" onclick="confirmation();" id="btnDummy" value="Transfer" class="search_button" />
                <asp:Button ID="btnSearch" Style="display: none" TabIndex="10" runat="server" Text="Transfer"
                    Font-Bold="False" CssClass="search_button"></asp:Button>
            </td>
            <td><IMG style="WIDTH: 414px; HEIGHT: 21px" id="imgicon" src="../Images/loading.gif" runat="server" /></td>
            <td>
            </td>
        </tr>
        <tr>
            <td colspan="3" style="font-family: Verdana; font-size: 9pt;">
                <div>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" Display="Dynamic" InitialValue="0"
                        ControlToValidate="ddlTransferOption" ErrorMessage="Please select Transfer Option"
                        runat="server" />
                    <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
                </div>
            </td>
        </tr>
    </table>
</asp:Content>
