<%@ Page Title="" Language="VB" MasterPageFile="~/SubPageMaster.master" AutoEventWireup="false" CodeFile="RoomOnline.aspx.vb" Inherits="PriceListModule_RoomOnline" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI" TagPrefix="asp" %>
 <%@ OutputCache location="none" %> 

<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">
<script language="javascript" type="text/javascript">

    function PopUpImageView(code) {
        var FileName = document.getElementById("<%=hdnFileName.ClientID%>");
        if (FileName.value == "") {
            FileName.value = code;
        }
        popWin = open('../PriceListModule/ImageViewWindow.aspx?pagename=RoomOnline.aspx&code=' + FileName.value, 'myWin', 'resizable=yes,scrollbars=yes,width=500,height=350,left=450,top=80');
        popWin.focus();
        FileName.value = "";
        return false
    }



    function checkNumber(evt) {
        evt = (evt) ? evt : event;
        var charCode = (evt.charCode) ? evt.charCode : ((evt.keyCode) ? evt.keyCode :
        ((evt.which) ? evt.which : 0));
        if (charCode != 47 && (charCode > 45 && charCode < 58)) {
            //alert("Enter numerals only in this field. "+ charCode);
            return true;
        }
        return false;
    }

    function validate() {
        var txtCode = document.getElementById("<%=txtCode.ClientID%>");
        var txtName = document.getElementById("<%=txtName.ClientID%>");

    
        var txtdesc = document.getElementById("<%=txtdescription.ClientID%>");


        if (txtCode.value == '') {
            alert("Supplier code cannot be blank");
            return false;
        }
        if (txtName.value == '') {
            alert("Supplier Name cannot be blank");
            return false;
        }

     
        if (txtdesc.value == '') {
            alert("Description cannot be blank");
            return false;
        }

    }
    function GetValueFrom() {

        var ddl = document.getElementById("<%=ddlrmtypname.ClientID%>");
        ddl.selectedIndex = -1;
        // Iterate through all dropdown items.
        for (i = 0; i < ddl.options.length; i++) {
            if (ddl.options[i].text ==
			document.getElementById("<%=ddlrmtypcode.ClientID%>").value) {
                // Item was found, set the selected index.
                ddl.selectedIndex = i;
                return true;
            }
        }
    }
    function GetValueCode() {
        var ddl = document.getElementById("<%=ddlrmtypcode.ClientID%>");

        ddl.selectedIndex = -1;
        // Iterate through all dropdown items.
        for (i = 0; i < ddl.options.length; i++) {
            //alert(ddl.options[i].text);
            if (ddl.options[i].text ==
			document.getElementById("<%=ddlrmtypname.ClientID%>").value) {
                // Item was found, set the selected index.
                ddl.selectedIndex = i;
                return true;
            }
        }
    }



</script>

<asp:UpdatePanel id="UpdatePanel1" runat="server">
<contenttemplate>
<TABLE style="BORDER-RIGHT: gray 2px solid; BORDER-TOP: gray 2px solid; BORDER-LEFT: gray 2px solid; WIDTH: 950px; BORDER-BOTTOM: gray 2px solid" class="td_cell">
<TBODY>
<TR>
<TD style="width:150px" class="td_cell"  colspan="5" align="center" ><asp:Label id="lblHeading" runat="server" Text="Supplier Room Types Online" Width="950px" CssClass="field_heading"></asp:Label></TD>
<td style="width:500px"></td>
<td style="width:300px"></td>
</TR>

<TR>
<TD style="width:150px">Supplier</TD>
<TD>
<INPUT  id="txtCode" class="field_input" tabIndex=1 readOnly type=text maxLength=20 runat="server" />&nbsp;
<INPUT style="WIDTH: 360px" id="txtName" class="field_input" tabIndex=2 readOnly type=text maxLength=100 runat="server" />
</TD>
<td ></td>
</TR>

<TR>
<TD >Room Type</TD>
<TD >
<SELECT style="WIDTH: 130px" id="ddlrmtypcode" class="field_input" tabIndex=3 onchange="GetValueFrom()" runat="server"> <OPTION selected></OPTION></SELECT>&nbsp;
<SELECT style="WIDTH: 360px" id="ddlrmtypname" class="field_input" tabIndex=4 onchange="GetValueCode()" runat="server"> <OPTION selected></OPTION></SELECT>
</TD>
<td ></td>
</TR>

<TR>
<TD  class="td_cell">Description</TD>
<TD ><asp:TextBox id="txtdescription" runat="server" Width="500px" TextMode="MultiLine" Height="71px"></asp:TextBox></TD>
<td ></td>
</TR>

<TR>
<TD  class="td_cell">Room Star</TD>
<TD >
<asp:DropDownList id="ddlStarNo" tabIndex=106 runat="server" Width="49px">
                        <asp:ListItem>1</asp:ListItem>
                        <asp:ListItem>2</asp:ListItem>
                        <asp:ListItem>3</asp:ListItem>
                        <asp:ListItem>4</asp:ListItem>
                        <asp:ListItem>5</asp:ListItem>
                    </asp:DropDownList>
</TD>
<td ></td>
</TR>

<TR>
<TD style="width:150px" class="td_cell">Room Image 1</TD>
<TD style="width:500px" >
<asp:FileUpload id="FileUpload1"  runat="server" Width="250px" CssClass="field_input" __designer:wfdid="w17"></asp:FileUpload>(200 X 120)
<asp:Label id="lblimage1" runat="server" Width="200px" __designer:wfdid="w18" ForeColor="Blue"></asp:Label>
</TD>
<TD style="width:300px">
<asp:Button id="btnViewImg1" Width="100px"  runat="server"  CssClass="field_button" Text="View" />
<asp:Button id="Btnrmv1" Width="100px"  onclick="Btnrmv1_Click" runat="server" Text="Remove" CssClass="field_button" ></asp:Button>
</TD>
</TR>

<TR>
<TD  class="td_cell">Room Image 2</TD>
<TD >
<asp:FileUpload id="FileUpload2"  runat="server" Width="250px" CssClass="field_input" __designer:wfdid="w20"></asp:FileUpload>(200 X 120)
<asp:Label id="lblimage2" runat="server" Width="200px" __designer:wfdid="w21" ForeColor="Blue"></asp:Label>
</TD>
<TD style="width:300px">
<asp:Button id="btnViewImg2" Width="100px" runat="server"  CssClass="field_button" Text="View" />
<asp:Button id="Btnrmv2" Width="100px" onclick="Btnrmv2_Click" runat="server" Text="Remove" CssClass="field_button" __designer:wfdid="w22"></asp:Button>
</TD>
</TR>

<TR>
<TD   class="td_cell">Room Image 3</TD>
<TD >
<asp:FileUpload id="FileUpload3"  runat="server" Width="251px" CssClass="field_input" __designer:wfdid="w23"></asp:FileUpload>(200 X 120)
<asp:Label id="lblimage3" runat="server" Width="273px" __designer:wfdid="w24" ForeColor="Blue"></asp:Label>
</td>
<td>
<asp:Button id="btnViewImg3" Width="100px" runat="server"  CssClass="field_button" Text="View" />
<asp:Button id="btnrmv3" Width="100px" onclick="Btnrmv3_Click" runat="server" Text="Remove" CssClass="field_button" __designer:wfdid="w25"></asp:Button>
</TD>
</TR>
  
<TR>
<TD  class="td_cell">Room Image 4</TD>
<TD  >
<asp:FileUpload id="FileUpload4" tabIndex=111 runat="server" Width="251px" CssClass="field_input" __designer:wfdid="w26"></asp:FileUpload>(200 X 120)
<asp:Label id="lblimage4" runat="server" Width="288px" __designer:wfdid="w27" ForeColor="Blue"></asp:Label>
</TD>
<td >
<asp:Button id="btnViewImg4" Width="100px" runat="server"  CssClass="field_button" Text="View" />
<asp:Button id="btnrmv4" Width="100px" onclick="Btnrmv4_Click" runat="server" Text="Remove" CssClass="field_button" __designer:wfdid="w28"></asp:Button>
</td>
</TR>
    
<TR>
<TD  class="td_cell" colspan="3">
<TABLE cellSpacing=0 cellPadding=0 border=0>
<TBODY>
<TR>
<TD style="WIDTH: 134px; HEIGHT: 22px">
<asp:Button id="btnSave" tabIndex=6 runat="server" Text="Save" CssClass="field_button" OnClientClick="return validate();"></asp:Button></TD>
<TD style="WIDTH: 148px; HEIGHT: 22px"><asp:Button id="btnCancel" tabIndex=7 runat="server" Text="Return to Search" Width="128px" CssClass="field_button" Height="19px"></asp:Button></TD>
<TD style="WIDTH: 116px; HEIGHT: 22px"><asp:Button id="btnHelp" tabIndex=8 runat="server" Text="Help" CssClass="field_button"></asp:Button></TD>
<TD style="WIDTH: 581px; HEIGHT: 22px">&nbsp;&nbsp;
<INPUT style="VISIBILITY: hidden; WIDTH: 12px; HEIGHT: 9px" id="txtconnection" type=text runat="server" />
<asp:TextBox id="txtimg1" runat="server" Visible="False" Width="2px" __designer:wfdid="w29"></asp:TextBox> 
<asp:TextBox id="txtimg2" runat="server" Visible="False" Width="5px" __designer:wfdid="w30"></asp:TextBox>
<asp:TextBox id="txtimg3" runat="server" Visible="False" Width="2px" __designer:wfdid="w31"></asp:TextBox>
<asp:TextBox id="txtimg4" runat="server" Visible="False" Width="17px" __designer:wfdid="w32"></asp:TextBox>
</TD>
</TR>
</TBODY>
</TABLE>
</TD>
</TR>

</TBODY>
</TABLE>
   
<asp:Textbox ID="hdnFileName" Text="" runat="server" style="display:none"/>
        
<asp:ScriptManagerProxy id="ScriptManagerProxy1" runat="server">
<Services>
<asp:ServiceReference Path="~/clsServices.asmx" />
</Services>
</asp:ScriptManagerProxy> 
</contenttemplate>

<Triggers>
<asp:PostBackTrigger ControlID="btnSave" /> 
</Triggers>
</asp:UpdatePanel>
</asp:Content>



