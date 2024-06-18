<%@ Page Title="" Language="VB" MasterPageFile="~/SubPageMaster.master" AutoEventWireup="false" CodeFile="CustomerWhiteLabelling.aspx.vb" Inherits="CustomerWhiteLabelling" %>

 <%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">
    <script language="javascript" type="text/javascript">
    function GetSellingStr(txtamount,ddloper,txtresult) {
      
        if (ddloper.value != "[Select]") {
            txtresult.value = ddloper.value + parseFloat(txtamount.value).toFixed(4);
        }
    }

    function Disabledfalse() {
        document.getElementById("<%=txthotelstr.ClientID%>").disabled = true;
    }
    function FormValidation(state) {


        if (document.getElementById("<%=txtmhotel.ClientID%>").value != "" && document.getElementById("<%=ddlhotelopr.ClientID%>").value == "[Select]") {
        
            document.getElementById("<%=ddlhotelopr.ClientID%>").focus();
            alert("Hotel Operator field can not be blank");
            return false;
        }
        else if (document.getElementById("<%=txtmtansfer.ClientID%>").value != "" && document.getElementById("<%=ddltransferopr.ClientID%>").value == "[Select]") {

            document.getElementById("<%=ddltransferopr.ClientID%>").focus();
            alert("Tansfer Operator field can not be blank");
            return false;
        }

        else if (document.getElementById("<%=txtmcar.ClientID%>").value != "" && document.getElementById("<%=ddlcaropr.ClientID%>").value == "[Select]") {

            document.getElementById("<%=ddlcaropr.ClientID%>").focus();
            alert("Car Operator field can not be blank");
            return false;
        }
        else if (document.getElementById("<%=txtmvisa.ClientID%>").value != "" && document.getElementById("<%=ddlvisa.ClientID%>").value == "[Select]") {

            document.getElementById("<%=ddlvisa.ClientID%>").focus();
            alert("Visa Operator field can not be blank");
            return false;
        }
        else if (document.getElementById("<%=txtmexcursion.ClientID%>").value != "" && document.getElementById("<%=ddlexcursion.ClientID%>").value == "[Select]") {

            document.getElementById("<%=ddlexcursion.ClientID%>").focus();
            alert("Excursion Operator field can not be blank");
            return false;
        }
        else if (document.getElementById("<%=txtmguide.ClientID%>").value != "" && document.getElementById("<%=ddlguide.ClientID%>").value == "[Select]") {

            document.getElementById("<%=ddlguide.ClientID%>").focus();
            alert("Guide Operator field can not be blank");
            return false;
        }
        else if (document.getElementById("<%=txtmentrance.ClientID%>").value != "" && document.getElementById("<%=ddlentrance.ClientID%>").value == "[Select]") {

            document.getElementById("<%=ddlentrance.ClientID%>").focus();
            alert("Entrance Operator field can not be blank");
            return false;
        }

        else if (document.getElementById("<%=txtmjeep.ClientID%>").value != "" && document.getElementById("<%=ddljeep.ClientID%>").value == "[Select]") {

            document.getElementById("<%=ddljeep.ClientID%>").focus();
            alert("Jeep Operator field can not be blank");
            return false;
        }
        else if (document.getElementById("<%=txtmmeal.ClientID%>").value != "" && document.getElementById("<%=ddlmeal.ClientID%>").value == "[Select]") {

            document.getElementById("<%=ddlmeal.ClientID%>").focus();
            alert("Meal Operator field can not be blank");
            return false;
        }
        else if (document.getElementById("<%=txtmothers.ClientID%>").value != "" && document.getElementById("<%=ddlothers.ClientID%>").value == "[Select]") {

            document.getElementById("<%=ddlothers.ClientID%>").focus();
            alert("Others Operator field can not be blank");
            return false;
        }
        else if (document.getElementById("<%=txtmairmeet.ClientID%>").value != "" && document.getElementById("<%=ddlairmeet.ClientID%>").value == "[Select]") {

            document.getElementById("<%=ddlairmeet.ClientID%>").focus();
            alert("Airmeet Operator field can not be blank");
            return false;
        }


        else if (document.getElementById("<%=txtmhandlingfee.ClientID%>").value != "" && document.getElementById("<%=ddlhandlingfee.ClientID%>").value == "[Select]") {

            document.getElementById("<%=ddlhandlingfee.ClientID%>").focus();
            alert("Handlingfee Operator field can not be blank");
            return false;
        }

        else if (document.getElementById("<%=fpthumpnail.ClientID%>").value == "" && state == 'New') {
            alert("Please select Thumpmail Image!");
            return false;
        }

        else if (document.getElementById("<%=fpmissinghotel.ClientID%>").value == "" && state == 'New') {
            alert("Please select MissingHotel Image!");
            return false;
        }

        else if (document.getElementById("<%=fpreport.ClientID%>").value == "" && state == 'New') {
            alert("Please select Report Image!");
            return false;
         }

         else {
            //alert(state);
            if (state == 'New') {
                if (confirm('Are you sure you want to save  ?') == false) return false;
            }
            if (state == 'Edit') { if (confirm('Are you sure you want to update  ?') == false) return false; }
            if (state == 'Delete') { if (confirm('Are you sure you want to delete  ?') == false) return false; }
        }
}

function checkNumberDecimal1(evt, txt) {

    evt = (evt) ? evt : event;
    var charCode = (evt.charCode) ? evt.charCode : ((evt.keyCode) ? evt.keyCode :
        ((evt.which) ? evt.which : 0));
    if (charCode != 47 && (charCode > 44 && charCode < 58)) {
        var value = txt.value;
        var indx = value.indexOf('.');
        var deci = document.getElementById("<%=txtdecimal1.ClientID%>");
        var lngLenght = deci.value;
        if (indx < 0) {
            return true;
        }

        var digit = value.substring(indx + 1);
        if (digit.length > lngLenght - 1) {
            return false;
        }
        else {
            return true;
        }
    }
    return false;
}


</script>
<table class="td_cell" style="width:100%">

<tr>
   <td align="center"> <asp:Label ID="lblHeading" runat="server" CssClass="field_heading" Text="Customer White Labelling" width="100%" ></asp:Label></td>
                  
</tr>
 <tr>
  <td height="10px"></td>
</tr>
<tr>
<td align="center">
  <table width="100%">
  <tr>
    <td class="td_cell" align="left">Agent Code </td>
    <td class="td_cell" align="left">
    <asp:TextBox ID="txtagentcode" runat="server"  ReadOnly="true" CssClass="txtbox"></asp:TextBox>
  
    </td>
    <td class="td_cell" align="left">Agent Name</td>
    <td class="td_cell" align="left"><asp:TextBox ID="txtagentname" runat="server" ReadOnly="true"  CssClass="txtbox" Width="250px"></asp:TextBox></td>
  </tr>

   <tr>
    <td class="td_cell" align="left">Markup Hotel </td>
    <td class="td_cell" align="left">
    <asp:TextBox ID="txtmhotel" runat="server"  CssClass="txtbox" width="100px"></asp:TextBox>
   
     <SELECT style="WIDTH: 75px" id="ddlhotelopr" class="field_input"  runat="server">
             <OPTION value="[Select]" selected>[Select]</OPTION></SELECT>
   <asp:TextBox ID="txthotelstr" runat="server"  CssClass="txtbox" width="100px" Enabled="false"></asp:TextBox>
    
    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender7" runat="server" FilterType="Numbers, Custom"    ValidChars="." TargetControlID="txtmhotel" />
   
    </td>
    <td class="td_cell" align="left">Markup Transfer </td>
    <td class="td_cell" align="left"><asp:TextBox ID="txtmtansfer" runat="server"  CssClass="txtbox" width="100px"></asp:TextBox>
    
    <SELECT style="WIDTH: 75px" id="ddltransferopr" class="field_input"   runat="server">
             <OPTION value="[Select]" selected>[Select]</OPTION></SELECT>
    <asp:TextBox ID="txttransferstr" runat="server"  CssClass="txtbox" width="100px"></asp:TextBox>
    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" FilterType="Numbers, Custom"    ValidChars="." TargetControlID="txtmtansfer" /></td>
  
  </tr>
   <tr>
    <td class="td_cell" align="left">Markup Car </td>
    <td class="td_cell" align="left">
   
    <asp:TextBox ID="txtmcar" runat="server"  CssClass="txtbox" width="100px"></asp:TextBox>
    
     <SELECT style="WIDTH: 75px" id="ddlcaropr" class="field_input"   runat="server">
             <OPTION value="[Select]" selected>[Select]</OPTION></SELECT>
    <asp:TextBox ID="txtcarstr" runat="server"  CssClass="txtbox" width="100px"></asp:TextBox>
     <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" FilterType="Numbers, Custom"    ValidChars="." TargetControlID="txtmcar" />
    
    </td>
    <td class="td_cell" align="left">Markup Visa</td>
    <td class="td_cell" align="left"><asp:TextBox ID="txtmvisa" runat="server"  CssClass="txtbox" width="100px"></asp:TextBox>
   
    <SELECT style="WIDTH: 75px" id="ddlvisa" class="field_input"  runat="server">
             <OPTION value="[Select]" selected>[Select]</OPTION></SELECT>
    <asp:TextBox ID="txtvisastr" runat="server"  CssClass="txtbox" width="100px"></asp:TextBox>

     <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" FilterType="Numbers, Custom"    ValidChars="." TargetControlID="txtmvisa" />
    </td>
  </tr>
   <tr>
    <td class="td_cell" align="left">Markup Excursion </td>
    <td class="td_cell" align="left"><asp:TextBox ID="txtmexcursion" runat="server"  CssClass="txtbox" width="100px"></asp:TextBox>
    
      <SELECT style="WIDTH: 75px" id="ddlexcursion" class="field_input"   runat="server">
             <OPTION value="[Select]" selected>[Select]</OPTION></SELECT>
      <asp:TextBox ID="txtexcursionstr" runat="server"  CssClass="txtbox" width="100px"></asp:TextBox>
    
     <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" FilterType="Numbers, Custom"    ValidChars="." TargetControlID="txtmexcursion" />
    </td>
    <td class="td_cell" align="left">Markup Guide</td>
    <td class="td_cell" align="left"><asp:TextBox ID="txtmguide" runat="server"  CssClass="txtbox" width="100px"></asp:TextBox>
      
      <SELECT style="WIDTH: 75px" id="ddlguide" class="field_input"   runat="server">
             <OPTION value="[Select]" selected>[Select]</OPTION></SELECT>
      <asp:TextBox ID="txtgudiestr" runat="server"  CssClass="txtbox" width="100px"></asp:TextBox>
    
     
     <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server" FilterType="Numbers, Custom"    ValidChars="." TargetControlID="txtmguide" />
    </td>
  </tr>

  <tr>
    <td class="td_cell" align="left">Markup Entrance </td>
    <td class="td_cell" align="left"><asp:TextBox ID="txtmentrance" runat="server"  CssClass="txtbox" width="100px"></asp:TextBox>
     
      <SELECT style="WIDTH: 75px" id="ddlentrance" class="field_input"   runat="server">
             <OPTION value="[Select]" selected>[Select]</OPTION></SELECT>
      <asp:TextBox ID="txtentrancestr" runat="server"  CssClass="txtbox" width="100px"></asp:TextBox>
    
     <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender6" runat="server" FilterType="Numbers, Custom"    ValidChars="." TargetControlID="txtmentrance" />
    </td>
    <td class="td_cell" align="left">Markup Jeep </td>
    <td class="td_cell" align="left"><asp:TextBox ID="txtmjeep" runat="server"  CssClass="txtbox" width="100px"></asp:TextBox>
   
    <SELECT style="WIDTH: 75px" id="ddljeep" class="field_input"   runat="server">
             <OPTION value="[Select]" selected>[Select]</OPTION></SELECT>
      <asp:TextBox ID="txtjeepstr" runat="server"  CssClass="txtbox" width="100px"></asp:TextBox>
    
     <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender8" runat="server" FilterType="Numbers, Custom"    ValidChars="." TargetControlID="txtmjeep" />
    </td>
  </tr>
  <tr>
    <td class="td_cell" align="left">Markup Meal</td>
    <td class="td_cell" align="left"><asp:TextBox ID="txtmmeal" runat="server"  CssClass="txtbox" width="100px"></asp:TextBox>
     
    <SELECT style="WIDTH: 75px" id="ddlmeal" class="field_input"   runat="server">
             <OPTION value="[Select]" selected>[Select]</OPTION></SELECT>
      <asp:TextBox ID="txtmealstr" runat="server"  CssClass="txtbox" width="100px"></asp:TextBox>

     <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender9" runat="server" FilterType="Numbers, Custom"    ValidChars="." TargetControlID="txtmmeal" />
    </td>
    <td class="td_cell" align="left">Markup Others </td>
    <td class="td_cell" align="left"><asp:TextBox ID="txtmothers" runat="server"  CssClass="txtbox" width="100px"></asp:TextBox>
  
    <SELECT style="WIDTH: 75px" id="ddlothers" class="field_input"   runat="server">
             <OPTION value="[Select]" selected>[Select]</OPTION></SELECT>
      <asp:TextBox ID="txtothersstr" runat="server"  CssClass="txtbox" width="100px"></asp:TextBox>

     <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender10" runat="server" FilterType="Numbers, Custom"    ValidChars="." TargetControlID="txtmothers" />
    </td>
  </tr>
  <tr>
    <td class="td_cell" align="left">Markup Airmeet </td>
    <td class="td_cell" align="left"><asp:TextBox ID="txtmairmeet" runat="server"  CssClass="txtbox" width="100px"></asp:TextBox>

      <SELECT style="WIDTH: 75px" id="ddlairmeet" class="field_input"   runat="server">
             <OPTION value="[Select]" selected>[Select]</OPTION></SELECT>
      <asp:TextBox ID="txtairmeetstr" runat="server"  CssClass="txtbox" width="100px"></asp:TextBox>

     <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender11" runat="server" FilterType="Numbers, Custom"    ValidChars="." TargetControlID="txtmairmeet" />
    </td>
    <td class="td_cell" align="left">Markup Handlingfee</td>
    <td class="td_cell" align="left"><asp:TextBox ID="txtmhandlingfee" runat="server"  CssClass="txtbox" width="100px"></asp:TextBox>
    <SELECT style="WIDTH: 75px" id="ddlhandlingfee" class="field_input"   runat="server">
             <OPTION value="[Select]" selected>[Select]</OPTION></SELECT>
      <asp:TextBox ID="txthandlingfeestr" runat="server"  CssClass="txtbox" width="100px"></asp:TextBox>

    </td>
  </tr>

  <tr>
    <td class="td_cell" align="left">Address1</td>
    <td class="td_cell" align="left">
      <asp:TextBox ID="txtaddress" runat="server"  CssClass="txtbox" width="300px"></asp:TextBox>

    </td>
    <td class="td_cell" align="left">Telephone</td>
    <td class="td_cell" align="left">
      <asp:TextBox ID="txttelephone" runat="server"  CssClass="txtbox" width="200px"></asp:TextBox>

    </td>
  </tr>

  <tr>
    <td class="td_cell" align="left">Fax</td>
    <td class="td_cell" align="left">
      <asp:TextBox ID="txtfax" runat="server"  CssClass="txtbox" width="200px"></asp:TextBox>

    </td>
    <td class="td_cell" align="left">Email</td>
    <td class="td_cell" align="left">
      <asp:TextBox ID="txtemail" runat="server"  CssClass="txtbox" width="300px"></asp:TextBox>

    </td>
  </tr>

  <tr>
    <td class="td_cell" align="left">Web</td>
    <td class="td_cell" align="left">
      <asp:TextBox ID="txtweb" runat="server"  CssClass="txtbox" width="300px"></asp:TextBox>

    </td>
    <td class="td_cell" align="left">&nbsp;</td>
    <td class="td_cell" align="left">&nbsp;</td>
  </tr>

  <tr>
  <td colspan="4" align="left">
  <asp:panel id="pnlfileupload" runat="server">
  <table width="100%" >
    <tr>
      <td class="td_cell" align="left">Thumpnail Image </td>
      <td class="td_cell" align="left">
        <asp:FileUpload ID="fpthumpnail" runat="server" CssClass="field_input" TabIndex="17"  />&nbsp;
        (Size 64 X 70 Pixels)
        
        <br></br>
        <asp:TextBox ID="txtthumpnail" runat="server" Width="352px" Visible="False" CssClass="txtbox"></asp:TextBox>
      </td>
      <td class="td_cell" align="left">Missing Hotel Image </td>
      <td class="td_cell" align="left">
        <asp:FileUpload ID="fpmissinghotel" runat="server" CssClass="field_input" TabIndex="17"  />&nbsp;
        
          (Size 64 X 70 Pixels)<br></br>
        <asp:TextBox ID="txtmissinghotel" runat="server" Width="352px" Visible="False" CssClass="txtbox"></asp:TextBox>
      </td>
  
    </tr>

    <tr>
        <td class="td_cell" align="left">Report Image </td>
        <td class="td_cell" align="left">
           <asp:FileUpload ID="fpreport" runat="server" CssClass="field_input" TabIndex="17"   />&nbsp;
           (Maximum Size should be 108 x 331 Pixels)<br></br>
           <asp:TextBox ID="txtreport" runat="server" Width="352px" Visible="False" CssClass="txtbox"></asp:TextBox>
        </td>
        <td class="td_cell" align="left">Other Image</td>
        <td class="td_cell" align="left">
           <asp:FileUpload ID="fpother" runat="server" CssClass="field_input" TabIndex="17"  />&nbsp;<br></br>
           <asp:TextBox ID="txtother" runat="server" Width="352px" Visible="False" CssClass="txtbox"></asp:TextBox>
        </td>
    </tr>
  
  </table>
  
  </asp:panel>
  
  
  </td>

   
  </tr>
  <tr>
   <td colspan="4" align="left" height="20px" >
        
   </td>
  </tr>
  <tr>
   <td colspan="4" align="left" height="20px" >
    &nbsp;Active

       <asp:CheckBox ID="chkactive" runat="server" Checked="true" />
   </td>
  </tr>
   <tr>
   <td colspan="4" align="left" height="20px" >
        
   </td>
  </tr>
  <tr>
   <td colspan="4" align="left" ><b>
       <asp:Label ID="lblmsg" runat="server" Text="Uploaded Images" Font-Bold="true"></asp:Label></b></td>
  </tr>
  <tr>
   <td colspan="4" align="center">
   <asp:Panel ID="pnlview" runat="server">
    <table width="70%" style="border: 1px solid black;border-collapse:collapse;">


        <tr>
           <td align="left" class="td_cell" width="15%" style="border: 1px solid black; border-collapse:collapse;"> Thumpnail Image</td>
           <td align="left" class="td_cell"  width="35%" style="border: 1px solid black; border-collapse:collapse;">
               <asp:Image ID="Imgthumpnail" runat="server" />
               <asp:Label ID="lblImgthumpnail" runat="server" Text="Label" Visible="false" Font-Bold="true" ForeColor="Red"></asp:Label>
            </td>
           <td align="left" class="td_cell"  width="15%" style="border: 1px solid black; border-collapse:collapse;"> Missing Hotel Image</td>
           <td align="left" class="td_cell" width="35%" style="border: 1px solid black; border-collapse:collapse;">
               <asp:Image ID="Imgmissinghotel" runat="server" />
               <asp:Label ID="lblImgmissinghotel" runat="server" Text="Label" Visible="false" Font-Bold="true" ForeColor="Red"></asp:Label>
            </td>
        </tr>
       <tr>
           <td align="left" class="td_cell" style="border: 1px solid black; border-collapse:collapse;">Report Image</td>
           <td align="left" class="td_cell" style="border: 1px solid black; border-collapse:collapse;">
               <asp:Image ID="Imagreport" runat="server" />
               <asp:Label ID="lblImagreport" runat="server" Text="Label" Visible="false" Font-Bold="true" ForeColor="Red"></asp:Label>
            </td>
           <td align="left" class="td_cell" style="border: 1px solid black; border-collapse:collapse;"> Other Image</td>
           <td align="left" class="td_cell" style="border: 1px solid black; border-collapse:collapse;">
               <asp:Image ID="Imgother" runat="server" />
               <asp:Label ID="lblImgother" runat="server" Text="Label" Visible="false" Font-Bold="true" ForeColor="Red"></asp:Label>
            </td>
        </tr>
    </table>
    </asp:Panel>
   </td>
  
  </tr>

  <tr>
  <td colspan="4" height="20px"></td>
  </tr>
  <tr>
  <td colspan="4" align="left" style="height: 25px">
  <asp:Button ID="btnSave" runat="server" CssClass="btn" TabIndex="6" Text="Save" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
  <asp:Button ID="btnExit" runat="server" CssClass="btn" TabIndex="7" 
                    Text="Return to Search" />&nbsp;
      <asp:TextBox ID="txtdecimal1" runat="server" style="display:none"  Text="4"> </asp:TextBox>               

  </td>
  </tr>

  </table>

</td>

</tr>
     
       
       
    </table>
   <%-- <script type="text/javascript" language="javascript">
        Disabledfalse();
    
    </script>--%>
</asp:Content>

