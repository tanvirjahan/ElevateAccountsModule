<%@ Page Language="VB" MasterPageFile="~/SubPageMaster.master" AutoEventWireup="false" CodeFile="Routes.aspx.vb" Inherits="PriceListModule_Routes"  %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
    <%@ OutputCache location="none" %> 
   
<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">
<style type="text/css">

.GVFixedHeader { font-weight:bold;  position:relative; 
                font-family: Verdana, Arial, Geneva, ms sans serif;
            	font-size :10px;
	            font-weight:bold;
	            background-color :#06788B; 	
	            color :White;
	            
    }

.GVFixedHeader a:link {color :#800040;}
.GVFixedHeader a:active {color :#800040;}
.GVFixedHeader a:visited {color :#800040;}
.GVFixedHeader a:hover {color :#800040;}


    .style1
    {
        font-family: Verdana,Arial, Geneva, ms sans serif;
        font-size: 8pt;
        font-weight: normal;
        margin-top: 0px;
        height: 4px;
        width: 24%;
    }
    .style3
    {
        font-family: Verdana,Arial, Geneva, ms sans serif;
        font-size: 8pt;
        font-weight: normal;
        margin-top: 0px;
        height: 1px;
        width: 24%;
    }
    .style4
    {
        height: 1px;
    }
    .style7
    {
        height: 1px;
    }
    .style8
    {
        font-family: Verdana,Arial, Geneva, ms sans serif;
        font-size: 8pt;
        font-weight: normal;
        margin-top: 0px;
        height: 22px;
        width: 17%;
    }
    .style10
    {
        font-family: Verdana,Arial, Geneva, ms sans serif;
        font-size: 8pt;
        font-weight: normal;
        margin-top: 0px;
        height: 3px;
        width: 17%;
    }
    .style11
    {
        height: 3px;
        width: 17%;
    }


    .style12
    {
        width: 100%;
    }
    .style13
    {
        font-family: Verdana,Arial, Geneva, ms sans serif;
        font-size: 8pt;
        font-weight: normal;
        margin-top: 0px;
        height: 1px;
    }
    .style14
    {
        font-family: Verdana,Arial, Geneva, ms sans serif;
        font-size: 8pt;
        font-weight: normal;
        margin-top: 0px;
        height: 4px;
        width: 17%;
    }


    .style15
    {
        height: 4px;
        width: 426px;
    }
    .style16
    {
        font-family: Verdana,Arial, Geneva, ms sans serif;
        font-size: 8pt;
        font-weight: normal;
        margin-top: 0px;
        height: 22px;
        width: 426px;
    }
    .style17
    {
        height: 4px;
    }
    .style18
    {
        font-family: Verdana,Arial, Geneva, ms sans serif;
        font-size: 8pt;
        font-weight: normal;
        margin-top: 0px;
        height: 22px;
    }
    .style19
    {
        font-family: Verdana,Arial, Geneva, ms sans serif;
        font-size: 8pt;
        font-weight: normal;
        margin-top: 0px;
        height: 22px;
        width: 50%;
    }
    .style20
    {
        width: 50%;
    }


</style>

<script src="../Content/js/jquery-1.5.1.min.js" type="text/javascript"></script>
<script src="../Content/js/JqueryUI.js" type="text/javascript"></script>
<script src="../Content/js/RouteCustom.js" type="text/javascript"></script>
<link type ="text/css" href ="../Content/css/JqueryUI.css" rel ="Stylesheet" />
 
    <script language ="javascript" type="text/javascript" >
     

        function Validate(state) {

            if (state == 'New' || state == 'Edit') {
                var ddlcode = document.getElementById("<%=ddlOtherGrpCode.ClientID%>");
                var ddlname = document.getElementById("<%=ddlOtherGrpName.ClientID%>");
                var txtTrns = document.getElementById("<%=txtTrns.ClientID%>");
                 if (document.getElementById("<%=txtName.ClientID%>").value == '') {
                    document.getElementById("<%=txtName.ClientID%>").focus();
                    alert('Please Enter Name');
                    return false;
                } 
                else {
                    if (state == 'New') { if (confirm('Are you sure you want to save other types?') == false) return false; }
                    if (state == 'Edit') { if (confirm('Are you sure you want to update ?') == false) return false; }
                    if (state == 'Delete') { if (confirm('Are you sure you want to delete ?') == false) return false; }
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

//        function GetOtherGrpValueFrom() {

//            var ddl = document.getElementById("<%=ddlOtherGrpName.ClientID%>");
//            ddl.selectedIndex = -1;
//            // Iterate through all dropdown items.
//            for (i = 0; i < ddl.options.length; i++) {
//                if (ddl.options[i].text ==
//                document.getElementById("<%=ddlOtherGrpCode.ClientID%>").value) {
//                    // Item was found, set the selected index.
//                    ddl.selectedIndex = i;
//                    showdiv();
//                    return true;
//                }
//            }
//            showdiv();
//        }
//        function GetOtherGrpValueCode() {
//            var ddl = document.getElementById("<%=ddlOtherGrpCode.ClientID%>");
//            ddl.selectedIndex = -1;
//            // Iterate through all dropdown items.
//            for (i = 0; i < ddl.options.length; i++) {
//                if (ddl.options[i].text ==
//			document.getElementById("<%=ddlOtherGrpName.ClientID%>").value) {
//                    // Item was found, set the selected index.
//                    ddl.selectedIndex = i;
//                    showdiv();
//                    return true;
//                }
//            }
//            showdiv();

//        }
//        function showdiv() {
//            var ddlcode = document.getElementById("<%=ddlOtherGrpCode.ClientID%>");
//            var ddlname = document.getElementById("<%=ddlOtherGrpName.ClientID%>");
//            var txtTrns = document.getElementById("<%=txtTrns.ClientID%>");
//            var ddlPName = document.getElementById("<%=ddlPName.ClientID%>");
//            var ddlDName = document.getElementById("<%=ddlDName.ClientID%>");
//            var ddlType = document.getElementById("<%=ddlType.ClientID%>");


//            if (ddlname.value == txtTrns.value || ddlcode.options[ddlcode.selectedIndex].text == txtTrns.value) {
//                ddlPName.disabled = false;
//                ddlDName.disabled = false;
//                ddlType.disabled = false;
//            }
//            else {
//                ddlPName.disabled = true;
//                ddlDName.disabled = true;
//                ddlType.disabled = true;
//                ddlType.value = '[select]';
//            }
//        }
        function loadpickdropoff() {
            var ddlType = document.getElementById("<%=ddlType.ClientID%>");
            var ddlType1 = document.getElementById("<%=ddlNewserverType.ClientID%>");
            var ddlType2 = document.getElementById("<%=ddlNewserverType0.ClientID%>");
           
             if (ddlType.selectedIndex == 1) {

                 ddlType1.selectedIndex = 1;
                 ddlType2.selectedIndex = 2;
            }
            if (ddlType.selectedIndex == 2) {
                ddlType1.selectedIndex = 2;
                ddlType2.selectedIndex = 1;

            }
            if (ddlType.selectedIndex == 3) {
                ddlType1.selectedIndex = 2;
                ddlType2.selectedIndex = 2;
               
            }
            if (ddlType.selectedIndex == 4) {
                ddlType1.selectedIndex = 1;
                ddlType2.selectedIndex = 1;
            }
        }
        function FillPickUpName(result) {
            var ddl = document.getElementById("<%=ddlPName.ClientID%>");
            RemoveAll(ddl)
            for (var i = 0; i < result.length; i++) {
                var option = new Option(result[i].ListText, result[i].ListValue);
                ddl.options.add(option);
            }
            ddl.value = "[select]";
        }
        function FillDropOffName(result) {
            var ddl = document.getElementById("<%=ddlDName.ClientID%>");
            RemoveAll(ddl)
            for (var i = 0; i < result.length; i++) {
                var option = new Option(result[i].ListText, result[i].ListValue);
                ddl.options.add(option);
            }
            ddl.value = "[select]";
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
        function setvalue() {

            var ddlPName = document.getElementById("<%=ddlPName.ClientID%>");
            var ddlDName = document.getElementById("<%=ddlDName.ClientID%>");
            var hdnP = document.getElementById("<%=hdnP.ClientID%>");
            var hdnD = document.getElementById("<%=hdnD.ClientID%>");
            hdnP.value = ddlPName.value;
            hdnD.value = ddlDName.value;
        }


        function ddlDName_onclick() {

        }

    </script> 
   
    <asp:UpdatePanel id="UpdatePanel1" runat="server">
        <contenttemplate>
         <script type="text/javascript">
             var prm = Sys.WebForms.PageRequestManager.getInstance();
             prm.add_beginRequest(function () {

             });

             prm.add_endRequest(function () {
                 MyAutoRouteFillArray();

             }); 
                    
</script>
<table style="BORDER-RIGHT: gray 2px solid; BORDER-TOP: gray 2px solid; BORDER-LEFT: gray 2px solid; WIDTH: 100%;  TEXT-ALIGN: left">
<tbody>
<tr>
<td style="HEIGHT: 3px; TEXT-ALIGN: center" class="td_cell" colspan="4">
<asp:Label id="lblHeading" runat="server" Text="Add New Routes"  ForeColor="White" Width="100%" CssClass="field_heading"></asp:Label>
 </td>
        
        </tr>
<tr style="COLOR: #ff0000">

<td class="style1"><span style="COLOR: #000000">
    Code</span> <span style="COLOR: #ff0000" class="td_cell">*</span>
    
    </td>
<td style="WIDTH: 2px; COLOR: #000000; HEIGHT: 4px"><input style="WIDTH: 226px" id="txtCode" class="field_input" tabindex="1" type="text" maxlength="20" runat="server" />
<span style="COLOR: #ff0000"></span></td>
   <td style="COLOR: #000000; " class="style15">
       &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;</td>
    <td style="COLOR: #000000; " class="style17">
    <input style="WIDTH: 81px; TEXT-ALIGN: right; visibility:hidden" id="txtTrns" 
            tabindex=0 type="text" 
                    runat="server" class="field_input" />
    </td>
        <td></td>
    </tr>
<tr style="COLOR: #ff0000">

<td class="style3"><span style="COLOR: #000000">
    Name</span> <span class="td_cell">*</span></td>
<td style="COLOR: #ff0000; " class="style4" colspan="2">

<input style="WIDTH: 969px" id="txtName" class="field_input MyAutoCompleteClass" tabindex="2" type="text" maxlength="100" runat="server" />

    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;

</td>
    <td style="COLOR: #ff0000; " class="style4">
  
    <input style="WIDTH: 81px; TEXT-ALIGN: right; visibility: hidden;" 
            id="txtconnection" tabindex=7 type="text" 
                    runat="server" class="field_input" />
    &nbsp;
    </td>
    <td class="style7"></td>
    
    </tr>
    
    <tr>
   <td class="style3">
   <span>
    Transfer type  <span style="COLOR: #ff0000" class="td_cell">*</span></span></td>


<td style="WIDTH: 114%;; HEIGHT: 22px" class="td_cell" align="left">


<asp:DropDownList ID="ddlServerType"  runat="server"  
            CssClass="fiel_input" Height="22px" tabindex="3" Width="226px" 
        AutoPostBack="true">
        </asp:DropDownList>


        &nbsp;</td>
<td class="style16" align="left">&nbsp;&nbsp;</td>

<td class="style18" align="left">
        &nbsp;</td>
        
        
        </tr>

        </tbody>

        </Table>

        <table style="BORDER-RIGHT: gray 2px solid; BORDER-LEFT: gray 2px solid;TEXT-ALIGN: left">
<tbody>
        
        <tr>


        
<td  class="style13">
  <table>
  <tr>
 <td  valign="middle">
     Pick Up types  <span style="COLOR: #ff0000" class="td_cell">*</span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
  </td>
 <td  valign="middle">
  <asp:DropDownList ID="ddlNewServerType" runat="server" AutoPostBack="true" 
        CssClass="fiel_input" Height="22px" tabindex="4" Width="226px">
    </asp:DropDownList>
  </td>
  </tr>
  </table>
       
   </td>
   <td align="left" class="style19">
  <table>
  <tr>
 <td  valign="middle">
     &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<table class="style12">
         <tr>
             <td>
                 Drop types <span class="td_cell" style="COLOR: #ff0000">*</span>&nbsp;&nbsp;</td>
             <td>
                 <asp:DropDownList ID="ddlNewServerType0" runat="server" AutoPostBack="true" 
                     CssClass="fiel_input" Height="22px" tabindex="5" Width="228px">
                 </asp:DropDownList>
             </td>
         </tr>
     </table>
     &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
  </td>
 <td valign="middle">
     <asp:HiddenField ID="hdnP" runat="server" />
             </td>
  </tr>
  </table>
        
    </td>
   <td align="left" class="td_cell" style="WIDTH: 963px; HEIGHT: 22px">
        </td>
   <td align="left" class="td_cell" style="WIDTH: 4px; HEIGHT: 22px">
        &nbsp;</td>
    </tr>
   


    <tr>
   <td>
    <span class="td_cell" >  &nbsp;Pick Up Point </span>  
    <asp:Panel ID="Panel1" runat="server" Height="200px" 
                ScrollBars="Auto" Width="500px">
               
            
             
                <asp:GridView ID="gv_SearchResult" runat="server" AutoGenerateColumns="True" 
                    BackColor="White" BorderColor="#999999" BorderStyle="None" BorderWidth="1px" 
                    CellPadding="3" CssClass="td_cell" Font-Size="10px" GridLines="Vertical" 
                    tabindex="5" Width="95%">
                    <FooterStyle CssClass="grdfooter" />
                    <Columns>
                        <asp:TemplateField HeaderText="select">
                            <ItemStyle HorizontalAlign="Center" Width="60px" />
                            <HeaderStyle HorizontalAlign="Center" Width="60px" />
                            <ItemTemplate>
                                <asp:CheckBox ID="chkselect" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <RowStyle CssClass="grdRowstyle" />
                    <selectedRowStyle CssClass="grdselectrowstyle" />
                    <PagerStyle CssClass="grdpagerstyle" HorizontalAlign="Center" />
                    <HeaderStyle CssClass="GVFixedHeader" ForeColor="white" />
                    <AlternatingRowStyle CssClass="grdAternaterow" />
                </asp:GridView>
           
            </asp:Panel>
     <asp:Button ID="btnselect" runat="server" CssClass="btn" tabindex="20" 
                                                    Text="select All" />
                                                &nbsp;&nbsp;
                                                <asp:Button ID="btnunselect" runat="server" CssClass="btn" tabindex="21" 
                                                    Text="Unselect All" />
    </td>
    
   <td class="style20">
    <span class="td_cell" >  &nbsp;Drop Off Point </span>

       <asp:Panel ID="Panel2" runat="server" Height="200px"  CssClass="NewPan" ScrollBars="Auto" 
                Width="500px">
                
            
                <asp:GridView ID="GrdPicDrop" runat="server" 
                    AutoGenerateColumns="True" BackColor="White" BorderColor="#999999" 
                    BorderStyle="None" BorderWidth="1px" CellPadding="3" CssClass="td_cell" 
                    Font-Size="10px" GridLines="Vertical" tabindex="6" Width="95%">
                    <FooterStyle CssClass="grdfooter" />
                    <Columns>
                        <asp:TemplateField HeaderText="select">
                            <ItemStyle HorizontalAlign="Center" Width="60px" />
                            <HeaderStyle HorizontalAlign="Center" Width="60px" />
                            <ItemTemplate>
                                <asp:CheckBox ID="chkselect" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <RowStyle CssClass="grdRowstyle" />
                    <selectedRowStyle CssClass="grdselectrowstyle" />
                    <PagerStyle CssClass="grdpagerstyle" HorizontalAlign="Center" />
                    <HeaderStyle CssClass="GVFixedHeader" ForeColor="white" />
                    <AlternatingRowStyle CssClass="grdAternaterow" />
                </asp:GridView>
              
            </asp:Panel>
     <asp:Button ID="btnselectAll" runat="server" CssClass="btn" tabindex="20" 
                                                    Text="select All" />
                                                &nbsp;&nbsp;
                                                <asp:Button ID="btnUnselectAll" runat="server" CssClass="btn" tabindex="21" 
                                                    Text="Unselect All" />
    </td>
    </tr>

         </tbody>

        </Table>

            <table style=" BORDER-BOTTOM: gray 2px solid;BORDER-RIGHT: gray 2px solid; BORDER-LEFT: gray 2px solid; width:100%; TEXT-ALIGN: left">
<tbody>
    <tr>
       <td class="style8">
           </td>
       <td align="left" class="td_cell" style="WIDTH: 2px; HEIGHT: 22px">
            &nbsp;</td>
       <td align="left" class="td_cell" style="WIDTH: 963px; HEIGHT: 22px">
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<select ID="ddlDName" runat="server" class="field_input" name="D4" 
                tabindex="3" visible="false">
            </select>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;</td>
       <td align="left" class="td_cell" style="WIDTH: 4px; HEIGHT: 22px">
            <asp:HiddenField ID="hdnD" runat="server" />
        </td>
    </tr>
<tr><td class="style14">Order <span style="COLOR: #ff0000">*</span> </td>
   <td style="HEIGHT: 4px; width: 2px;">
        <input style="WIDTH: 226px; TEXT-ALIGN: right" id="txtOrder" tabindex="7" 
        type="text" runat="server" class="field_input" />
    </td>
   <td style="WIDTH: 963px; HEIGHT: 4px">
       &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;</td>
   <td style="WIDTH: 4px; HEIGHT: 4px">
        &nbsp;</td>
    </tr><tr><td class="style10">
        Brief description of Itinerary
        </td>
       <td colspan="3" style="HEIGHT: 10px">
            <asp:TextBox ID="txtRemark" runat="server" 
                CssClass="field_input" Height="76px" tabindex="8" TextMode="MultiLine" 
                Width="538px"></asp:TextBox>
                <%--<textarea style="WIDTH: 607px; HEIGHT:76px" id="txtRemark" class="field_input" tabindex=8 runat="server"></TEXTAREA>--%>
        </td>
        <td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </td>
    </tr>
    <tr>
       <td style="HEIGHT: 20px; width: 74px;" class="td_cell" colspan="4">
            <table border="0" style="WIDTH: 438px">
                <tbody>
                <tr>
                <td colspan="5">
                
                   <input id="Chkshuttle" tabindex="14" type="checkbox"  runat="server" />
                Shuttle
                </td>
                
                </tr>
                    <tr>
                       <td style="WIDTH: 58px; HEIGHT: 1px">
                            Min Pax <span style="COLOR: #ff0000">*</span>
                        </td>
                       <td style="WIDTH: 1px; HEIGHT: 1px">
                            <input style="WIDTH: 81px; TEXT-ALIGN: right" id="txtMinPax" tabindex="9" type="text" 
                    runat="server" class="field_input" />
                        </td>
                       <td colspan="2" style="WIDTH: 193px; HEIGHT: 1px">
                            <input id="ChkPakReq" tabindex="12" type="checkbox" visible="false" checked="Checked" runat="server" />
                           <%-- Pax Check Required--%></td>

                             <td>&nbsp;&nbsp;</td>
                    </tr>
                    <tr>
                       <td colspan="2" style="HEIGHT: 1px">
                            <input id="ChkInactive" tabindex=10 type="checkbox" visible="true" checked="Checked" runat="server" />
                            active</td>
                       <td colspan="2" style="WIDTH: 193px; HEIGHT: 1px">
                            <input id="ChkPrnRemark" tabindex="13" type="checkbox" visible="false" checked="Checked" runat="server" />
                           <%-- &nbsp;Print Remark--%></td>
                    </tr>
                    <tr>
                       <td colspan="2" style="HEIGHT: 4px">
                            <input id="ChkPrnConfirm" tabindex="11" type="checkbox"  visible="false" checked="Checked" runat="server" />
                            <%--Print in Confirmation--%></td>
                       <td colspan="2" style="WIDTH: 193px; HEIGHT: 4px">
                            <input id="ChkAutoCancel" tabindex="14" type="checkbox" visible="false"  checked="Checked" runat="server" />
                            &nbsp;<%--Auto Cancellation Required--%></td>

                             <td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;</td>
                    </tr>
                </tbody>
            </table>
        </td>
    </tr>
    <tr>
       <td class="style11">
            <asp:Button ID="btnSave" runat="server" CssClass="field_button" 
                 tabindex="15" Text="Save" />
               <%--  onclick="btnSave_Click"--%>
        </td>
       <td style="WIDTH: 237px">
            <asp:Button ID="btnCancel" runat="server" CssClass="field_button" 
                onclick="btnCancel_Click" tabindex="15" Text="Return To Search" 
                Width="131px" />
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Button ID="btnhelp" runat="server"  
                CssClass="field_button" onclick="btnhelp_Click" tabindex="16" 
                Text="Help" />
        </td>
    </tr>
    </TBODY>
    <caption>
        &nbsp;</TBODY></caption>
            </TABLE>
</contenttemplate>
    </asp:UpdatePanel>
    <asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server">
             <Services>
                 <asp:ServiceReference Path="~/clsServices.asmx" />
             </Services>
         </asp:ScriptManagerProxy>
<span  style="COLOR: #ff0000"><select id="ddlOtherGrpCode" runat="server"  
                       class="field_input" name="D7" onchange="GetOtherGrpValueFrom()" 
                       style="WIDTH: 232px" tabindex="3" visible="False">
                       <option  selected=""></option>
                   </select><select ID="ddlPName" runat="server" 
        class="field_input" name="D2" 
        onchange="setvalue();" style="WIDTH: 232px" tabindex="3" visible="false" 
       >
        <option selected="" ></option>
    </select>
    <select style="WIDTH: 315px" id="ddlOtherGrpName" class="field_input" tabindex=4 
        onchange="GetOtherGrpValueCode()" runat="server"  
        name="D5" visible="False"> <OPTION selected ></OPTION></select>
    </span>


        <select 
        ID="ddlType" runat="server" class="fiel_input" 
        onchange="loadpickdropoff()" visible="false"  style="WIDTH: 232px" 
        name="D6" >
              <option selected ="selected"  value="[select]" >[select]</option>
              <option value="0" >Arrival Borders</option>
              <option value="1">Departure Borders</option>
              <option value="2">Internal Transfer/Excursion</option>
              <option value="3">Arrival/Departure Transfer Borders</option>
             </select>
</asp:Content>


