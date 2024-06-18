<%@ Page Title="" Language="VB" MasterPageFile="~/SubPageMaster.master" AutoEventWireup="false" CodeFile="PaymentModes.aspx.vb" Inherits="ExcursionModule_PaymentModes" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
    <%@ OutputCache location="none" %> 

<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="server">

<script language="javascript" type="text/javascript">



    function compulsaryCode() {
        if (document.getElementById("<%=txtCode.ClientID%>").value == "") {
            alert("Code field can not be blank");
            document.getElementById("<%=txtCode.ClientID%>").focus();
            //                 txtCode.focus();
            return false;
        }
        else {
            document.getElementById("<%=txtName.ClientID%>").focus();
        }
    }






    function FormValidation(state) {



        if ((document.getElementById("<%=txtCode.ClientID%>").value == "") || (document.getElementById("<%=txtName.ClientID%>").value == "")) {
            if (document.getElementById("<%=txtCode.ClientID%>").value == "") {
                document.getElementById("<%=txtCode.ClientID%>").focus();
                alert("Code field can not be blank");
                return false;
            }
            else (document.getElementById("<%=txtName.ClientID%>").value == "")
            {
                document.getElementById("<%=txtName.ClientID%>").focus();
                alert("Name field can not be blank");
                return false;
            }
        }
        else {
            if (state == 'New') { if (confirm('Are you sure you want to save Payment Mode ?') == false) return false; }
            if (state == 'Edit') { if (confirm('Are you sure you want to update Payment Mode?') == false) return false; }
            if (state == 'Delete') { if (confirm('Are you sure you want to delete Payment Mode ?') == false) return false; }
        }
    }


 



    



   


</script>

    <asp:UpdatePanel id="UpdatePanel1" runat="server">
        <contenttemplate>
<TABLE style="BORDER-RIGHT: gray 2px solid; BORDER-TOP: gray 2px solid; BORDER-LEFT: gray 2px solid; BORDER-BOTTOM: gray 2px solid" class="td_cell">
<TBODY><TR><TD class="td_cell" align=center colSpan=3><asp:Label id="lblHeading" runat="server" Text="Add New Payment Modes"  Width="700px" CssClass="field_heading"></asp:Label></TD></TR><TR>
    <TD style="WIDTH: 150px" class="td_cell"><SPAN style="COLOR: black">Payment Mode Code</SPAN> <SPAN style="COLOR: red" class="td_cell">*</SPAN></TD>
    <TD style="COLOR: #000000; width: 177px;">
    <INPUT  id="txtCode" class="field_input" tabIndex=1 type=text maxLength=20 runat="server" /> </TD>
    <td style="COLOR: #000000; width: 194px;">
        &nbsp;</td>
    </TR><TR>
    <TD style="WIDTH: 150px; HEIGHT: 24px" class="td_cell">Payment Mode Name <SPAN style="COLOR: red" class="td_cell">*</SPAN></TD>
        <TD style="width: 177px">
        <INPUT id="txtName" class="field_input" tabIndex=2 type=text maxLength=100 runat="server" /> </TD>
        <td style="width: 194px">
            &nbsp;</td>
    </TR>

    <TR>
    <TD class="td_cell" style="width: 150px; ">
        <asp:Label ID="lblPerforma" runat="server" CssClass="td_ce" Text="Performa Required" 
            ViewStateMode="Enabled" Width="110px"></asp:Label>
        </TD>
    <TD style="width: 177px;">
        <INPUT id="chkPayment" tabIndex=5 type=checkbox CHECKED runat="server" />
        </TD>
        <td style="width: 194px">
            &nbsp;</td>
    </TR>



    
   
    <TR>
    <TD class="td_cell" style="width: 114px; ">
        <asp:Label ID="Label1" runat="server" CssClass="td_ce" Text="Active" 
            ViewStateMode="Enabled" Width="44px"></asp:Label>
        </TD>
    <TD style="width: 177px;">
        <INPUT id="chkActive" tabIndex=5 type=checkbox 
                CHECKED runat="server" />
        </TD>
        <td style="width: 194px">
            &nbsp;</td>
    </TR><tr><td class="td_cell" style="width: 114px; height: 23px;">
        <asp:Button ID="btnSave" runat="server" CssClass="btn" 
            tabIndex="6" Text="Save" />
        </td>
        <td style="height: 23px; width: 177px;">
            <asp:Button ID="btnCancel" runat="server"  CssClass="btn" 
                tabIndex="7" Text="Return to Search" />
            &nbsp;
            </td>
        <td style="height: 23px; ">
            <asp:Button ID="btnHelp" runat="server" 
                 CssClass="btn" tabIndex="8" Text="Help" />
        </td>
    </tr>
    
    </TBODY></TABLE>
</contenttemplate>
    </asp:UpdatePanel>
</asp:Content>



