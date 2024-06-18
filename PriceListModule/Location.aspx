<%@ Page Title="" Language="VB" MasterPageFile="~/SubPageMaster.master" AutoEventWireup="false" CodeFile="Location.aspx.vb" Inherits="PriceListModule_Location" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
    <%@ OutputCache location="none" %> 

<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">

<script language ="javascript" type="text/javascript" >
function CallWebMethod(methodType) {
        switch (methodType) {

            case "citycode":
                var select = document.getElementById("<%=ddlCityCode.ClientID%>");
                var selectname = document.getElementById("<%=ddlCityName.ClientID%>");
                var codeid = select.options[select.selectedIndex].text;
                var connstr = document.getElementById("<%=txtconnection.ClientID%>");
                constr = connstr.value

                selectname.value = select.options[select.selectedIndex].text;
                // ColServices.clsServices.GetCtyCountryCodeListnew(constr,codeid,FillCtryCode,ErrorHandler,TimeOutHandler); 
                //ColServices.clsServices.GetCtyCountryNameListnew(constr,selectname,FillCtryName,ErrorHandler,TimeOutHandler);
            case "cityname":
                var select = document.getElementById("<%=ddlCityName.ClientID%>");
                var selectname = document.getElementById("<%=ddlCityCode.ClientID%>");
                var codeid = select.options[select.selectedIndex].text;
                selectname.value = select.options[select.selectedIndex].text;
                var connstr = document.getElementById("<%=txtconnection.ClientID%>");
                constr = connstr.value

                //  ColServices.clsServices.GetCtyCountryNameListnew(constr,codeid,FillCtryName,ErrorHandler,TimeOutHandler);
                //ColServices.clsServices.GetCtyCountryCodeListnew(constr,selectname,FillCtryCode,ErrorHandler,TimeOutHandler);

        }
    }

     function FillCityCodes(result) {
        var ddl = document.getElementById("<%=ddlCityCode.ClientID%>");
        RemoveAll(ddl)
        for (var i = 0; i < result.length; i++) {
            var option = new Option(result[i].ListText, result[i].ListValue);
            ddl.options.add(option);
        }
        ddl.value = "[Select]";
    }

    function FillCityNames(result) {
        var ddl = document.getElementById("<%=ddlCityName.ClientID%>");

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

    function FormValidation(state) {
        if ((document.getElementById("<%=txtAreaCode.ClientID%>").value == "") || (document.getElementById("<%=txtAreaName.ClientID%>").value == "") || (document.getElementById("<%=ddlCityCode.ClientID%>").value == "[Select]")) {
            if (document.getElementById("<%=txtAreaCode.ClientID%>").value == "") {
                document.getElementById("<%=txtAreaCode.ClientID%>").focus();
                alert("Code field can not be blank");
                return false;
            }
            else if (document.getElementById("<%=txtAreaName.ClientID%>").value == "") {
                document.getElementById("<%=txtAreaName.ClientID%>").focus();
                alert("Name field can not be blank");
                return false;
            }

            else if (document.getElementById("<%=ddlCityCode.ClientID%>").value == "[Select]") {
                document.getElementById("<%=ddlCityCode.ClientID%>").focus();
                alert("Select City Code");
                return false;
            }
        }
        else {
            if (state == 'New') { if (confirm('Are you sure you want to save Area type?') == false) return false; }
            if (state == 'Edit') { if (confirm('Are you sure you want to update Area type?') == false) return false; }
            if (state == 'Delete') { if (confirm('Are you sure you want to delete Area type?') == false) return false; }
        }
    }
 

  
</script>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
<TABLE style="BORDER-RIGHT: gray 2px solid; BORDER-TOP: gray 2px solid; BORDER-LEFT: gray 2px solid; WIDTH: 742px; BORDER-BOTTOM: gray 2px solid">
<TBODY>
<TR>
<TD class="td_cell" align=center colSpan=4><asp:Label id="lblHeading" runat="server" Text="Add New Area" Width="731px" CssClass="field_heading"></asp:Label>
</TD>
</TR>
<TR style="COLOR: #ff0000">
 <TD style="WIDTH: 176px" class="td_cell">
    <SPAN style="COLOR: #000000">Area Code </SPAN>
      <SPAN style="COLOR: #ff0000" class="td_cell">*</SPAN></TD>
   <TD style="WIDTH: 203px; COLOR: #000000">
       <INPUT style="WIDTH: 196px" id="txtAreaCode" class="field_input" tabIndex=1 type=text maxLength=20 runat="server"/> 
   </TD>
</TR>
<TR>
<TD style="WIDTH: 176px" class="td_cell">Area Name <SPAN style="COLOR: #ff0000" class="td_cell">*</SPAN>
</TD>
<TD style="WIDTH: 203px; COLOR: #000000">
<INPUT style="WIDTH: 196px" id="txtAreaName" class="field_input" tabIndex=2 type=text maxLength=150 runat="server" />
 </TD>
</TR>
<TR>
<TD style="WIDTH: 176px" class="td_cell">City&nbsp; Code <SPAN style="COLOR: red" class="td_cell">*</SPAN>
</TD>
<TD style="WIDTH: 203px" class="td_cell">
<SELECT style="WIDTH: 202px" id="ddlCityCode" class="field_input" tabIndex=5 onchange="CallWebMethod('citycode');" runat="server">
 <OPTION selected></OPTION></SELECT>
 </TD>
 <TD style="WIDTH: 168px" class="td_cell">City Name</TD>
 <TD style="WIDTH: 139px" class="td_cell">
 <SELECT style="WIDTH: 306px" id="ddlCityName" class="field_input" tabIndex=6 onchange="CallWebMethod('cityname');" runat="server">
  <OPTION selected></OPTION></SELECT></TD></TR>
 
 <TR>
 <TD style="WIDTH: 176px" class="td_cell">Active</TD>
 <TD style="WIDTH: 203px"><INPUT id="chkActive" tabIndex=7 type=checkbox CHECKED runat="server" />
    <input id="txtconnection" runat="server" style="visibility: hidden; width: 12px;
        height: 9px" type="text" /></TD>
        </TR>
        
        <TR><TD style="WIDTH: 176px">
        <asp:Button id="btnSave" tabIndex=8 runat="server" Text="Save" 
            CssClass="field_button"></asp:Button></TD><TD style="WIDTH: 225px">
            <asp:Button id="btnCancel" tabIndex=9 onclick="btnCancel_Click" runat="server" Text="Return To Search" CssClass="field_button"></asp:Button>&nbsp; 
            <asp:Button id="btnhelp" tabIndex=10 onclick="btnhelp_Click" runat="server" 
                Text="Help" CssClass="field_button"></asp:Button></TD></TR>
                
                </TBODY></TABLE>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
</ContentTemplate>
    </asp:UpdatePanel>

    <asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server">
        <Services>
<asp:ServiceReference Path="~/clsServices.asmx"></asp:ServiceReference>
</Services>
    </asp:ScriptManagerProxy>

</asp:Content>

