<%@ Page Language="VB" MasterPageFile="~/SubPageMaster.master" AutoEventWireup="false" CodeFile="Guides.aspx.vb" Inherits="PriceListModule_Guides"  %>

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
        width: 346px;
       
    }
    .style4
    {
        height: 1px;
    }
    .style11
    {
        height: 3px;
        width: 17%;
    }


    .style17
    {
        height: 4px;
    }
    

    .style26
    {
        height: 4px;
        width: 950px;
    }
    .style27
    {
       
       
        font-weight: normal;
        margin-top: 0px;
        height: 22px;
        width: 950px;
    }
    .style28
    {
        font-family: Verdana,Arial, Geneva, ms sans serif;
        font-size: 8pt;
        font-weight: normal;
        margin-top: 0px;
        height: 1px;
        width: 58%;
    }


    .style29
    {
        font-family: Verdana,Arial, Geneva, ms sans serif;
        font-size: 8pt;
        font-weight: normal;
        margin-top: 0px;
        height: 42px;
        width: 58%;
    }
    .style30
    {
        font-family: Verdana,Arial, Geneva, ms sans serif;
        font-size: 8pt;
        font-weight: normal;
        margin-top: 0px;
        height: 42px;
        width: 950px;
    }


    .style31
    {
        font-family: Verdana,Arial, Geneva, ms sans serif;
        font-size: 8pt;
        font-weight: normal;
        margin-top: 0px;
        height: 1px;
        width: 346px;
    }
    .style32
    {
        font-family: Verdana,Arial, Geneva, ms sans serif;
        font-size: 8pt;
        font-weight: normal;
        margin-top: 0px;
        height: 42px;
        width: 346px;
    }
    .style33
    {
        font-family: Verdana,Arial, Geneva, ms sans serif;
        font-size: 8pt;
        font-weight: normal;
        margin-top: 0px;
        height: 16px;
        width: 346px;
    }


</style>

   <script language ="javascript" type="text/javascript" >


       function Validate(state) {

           if (state == 'New' || state == 'Edit') {
             
              
             if (document.getElementById("<%=txtName.ClientID%>").value == '') {
                   document.getElementById("<%=txtName.ClientID%>").focus();
                   alert('Please Enter Name');
                   return false;
               }
               else if ((document.getElementById("<%=txtTel.ClientID%>").value == '') && (document.getElementById("<%=txtMobile.ClientID%>").value == ''))
               {
                   document.getElementById("<%=txtTel.ClientID%>").focus();
                   alert('Please Enter Telephone Or Mobile Number');
                   return false;
               }
               else {
                   if (state == 'New') { if (confirm('Are you sure you want to save guide details?') == false) return false; }
                   if (state == 'Edit') { if (confirm('Are you sure you want to update ?') == false) return false; }
                   if (state == 'Delete') { if (confirm('Are you sure you want to delete ?') == false) return false; }
               }

           }
       }

       function checkNumber(e) {
           if (event.keyCode == 32 || event.keyCode == 45)
               return;
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


     
      
       function TimeOutHandler(result) {
           alert("Timeout :" + result);
       }

       function ErrorHandler(result) {
           var msg = result.get_exceptionType() + "\r\n";
           msg += result.get_message() + "\r\n";
           msg += result.get_stackTrace();
           alert(msg);
       }
  

    </script> 
   
    <asp:UpdatePanel id="UpdatePanel1" runat="server">
        <contenttemplate>
<table style="BORDER-RIGHT: gray 2px solid; BORDER-TOP: gray 2px solid; BORDER-LEFT: gray 2px solid; WIDTH: 100%;  TEXT-ALIGN: left">
<tbody>
<tr>
<td style="HEIGHT: 3px; TEXT-ALIGN: center" class="td_cell" colspan="4">
<asp:Label id="lblHeading" runat="server" Text="Add New Guide"  ForeColor="White" Width="100%" CssClass="field_heading"></asp:Label>
 </td>
        
        </tr>
<tr style="COLOR: #ff0000">

<td class="style1" ><span style="COLOR: #000000">
     Code</span> <span style="COLOR: #ff0000" class="td_cell">*</span>
    
    </td>
<td style="COLOR: #000000; " class="style26"><input style="WIDTH: 226px;" id="txtCode" class="field_input" tabindex="1" type="text" maxlength="20" runat="server" readonly="readonly" />
<span style="COLOR: #ff0000"></span></td>
   
  
        
    </tr>
<tr style="COLOR: #ff0000">

<td class="style31"><span style="COLOR: #000000">
    Name</span> <span class="td_cell">*</span></td>
<td style="COLOR: #ff0000; " class="style4" colspan="2">

<input style="WIDTH: 469px" id="txtName" class="field_input" tabindex="2" type="text" maxlength="200" runat="server" />

    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;

</td>
    <td style="COLOR: #ff0000; " class="style4">
  
    <input style="WIDTH: 81px; TEXT-ALIGN: right; visibility: hidden;" 
            id="txtconnection" tabindex=7 type="text" 
                    runat="server" class="field_input" />
    &nbsp;
    </td>
    
    
    </tr>
    
    <tr>
   <td class="style31">
   <span>
    Address </span></td>


<td class="style27" align="left">


    <asp:TextBox ID="txtAddress" tabIndex=9 runat="server" CssClass="field_input" TextMode="MultiLine" 
        Width="469px" maxlength="500"></asp:TextBox>
        </td>

        </tr>

        <tr>
   <td class="style32">
   <span>
    Telephone Number<span style="COLOR: #ff0000" class="td_cell">*</span></span></td>


<td class="style30" align="left">


  <input style="WIDTH: 226px" id="txtTel" class="field_input" tabindex="2" type="text" maxlength="100" runat="server" />
        </td>

        </tr>

           <tr>
   <td class="style32">
   <span>
    Mobile Number<span style="COLOR: #ff0000" class="td_cell">*</span> </span></td>


<td class="style30" align="left">


  <input style="WIDTH: 226px" id="txtMobile" class="field_input" tabindex="2" type="text" maxlength="100" runat="server" />
        </td>

        </tr>
        <tr>
        
       
            <td class="style33">Active</td>
            <td style="HEIGHT: 16px"><INPUT id="chkActive" tabIndex=3 type=checkbox CHECKED runat="server" /></td>
    
        </tr>

         <tr>
   <td class="style31">
   <span>
   Remarks </span></td>


<td align="left">


    <asp:TextBox ID="txtRemarks" tabIndex=9 runat="server" CssClass="field_input" TextMode="MultiLine" 
        Width="469px" maxlength="500"></asp:TextBox>
        </td>

        </tr>

        </tbody>

        </Table>

        <table style="BORDER-RIGHT: gray 2px solid;BORDER-LEFT: gray 2px solid;TEXT-ALIGN: left ; WIDTH: 100%;">
<tbody>
     


    <tr>
   <td>
    <span class="td_cell" >  &nbsp;<br />
       <br />
       Known Languages <span style="COLOR: #ff0000" class="td_cell">*</span> 
       <br />
       </span>  
    <asp:Panel ID="Panel1" runat="server" Height="200px" 
                ScrollBars="Auto" Width="500px">
               
            
             
                <asp:GridView ID="gv_SearchResult" runat="server" AutoGenerateColumns="False" 
                    BackColor="White" BorderColor="#999999" BorderStyle="None" BorderWidth="1px" 
                        CaptionAlign="Top" CellPadding="3" CssClass="td_cell" Font-Size="10px" 
                        GridLines="Vertical" tabIndex="19" UseAccessibleHeader="False" Visible="False" 
                        Width="530px">
                    <FooterStyle CssClass="grdfooter" />
                    <Columns>
                        <asp:TemplateField HeaderText="select">
                            <ItemStyle HorizontalAlign="Center" Width="60px" />
                            <HeaderStyle HorizontalAlign="Center" Width="60px" />
                            <ItemTemplate>
                                <asp:CheckBox ID="chkselect" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                         <asp:BoundField DataField="nationalitycode" HeaderText="Code">
                                <ItemStyle Width="400px" />
                                <HeaderStyle Width="400px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="nationLanguage" HeaderText="Language">
                                <ItemStyle Width="1000px" />
                                <HeaderStyle Width="1000px" />
                            </asp:BoundField>
                    </Columns>
                    <RowStyle CssClass="grdRowstyle" />
                    <selectedRowStyle CssClass="grdselectrowstyle" />
                    <PagerStyle CssClass="grdpagerstyle" HorizontalAlign="Center" />
                    <HeaderStyle CssClass="grdheader" />
                    <AlternatingRowStyle CssClass="grdAternaterow" />
                </asp:GridView>
           
            </asp:Panel>
     <asp:Button ID="btnselect" runat="server" CssClass="btn" tabindex="20" 
                                                    Text="select All" />
                                                &nbsp;&nbsp;
                                                <asp:Button ID="btnunselect" runat="server" CssClass="btn" tabindex="21" 
                                                    Text="Unselect All" />
                                                    &nbsp;&nbsp;
                                                     <asp:Button ID="btnshowselect" runat="server" CssClass="btn" tabindex="20" 
                                                    Text="Show Selected" />
                                                &nbsp;&nbsp;
                                                 <asp:Button ID="btnshowall" runat="server" CssClass="btn" tabindex="20" 
                                                    Text="Show All" />
    </td>
    
   
    </tr>

         </tbody>

        </Table>

            <table style=" BORDER-BOTTOM: gray 2px solid;BORDER-RIGHT: gray 2px solid; BORDER-LEFT: gray 2px solid; width:100%; TEXT-ALIGN: left">
<tbody>
    

  
    <tr>
       <td class="style11">
            <asp:Button ID="btnSave" runat="server" CssClass="field_button" 
                onclick="btnSave_Click" tabindex="15" Text="Save" />
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
    <tr>
    <td></td></tr>
    </TBODY>
   
            </TABLE>
</contenttemplate>
    </asp:UpdatePanel>
    <asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server">
             <Services>
                 <asp:ServiceReference Path="~/clsServices.asmx" />
             </Services>
         </asp:ScriptManagerProxy>



       
</asp:Content>