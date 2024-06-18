<%@ Page Title="" Language="VB" MasterPageFile="~/WebAdminMaster.master" AutoEventWireup="false" CodeFile="CustomerWhiteLabellingSearch.aspx.vb" Inherits="CustomerWhiteLabellingSearch" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ OutputCache location="none" %> 
<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">
<script language="javascript" type="text/javascript">
    function checkNumber(e) {

        if ((event.keyCode < 47 || event.keyCode > 57)) {
            return false;
        }

    }

    function CallWebMethod(methodType) {
        switch (methodType) {
            case "marketcode":

                var select = document.getElementById("<%=ddlMarket.ClientID%>");
                var codeid = select.options[select.selectedIndex].text;
                var selectname = document.getElementById("<%=ddlMarketName.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;
                var connstr = document.getElementById("<%=txtconnection.ClientID%>");
                constr = connstr.value


                ColServices.clsServices.GetCountryCodeListnew(constr, codeid, FillCountryCodes, ErrorHandler, TimeOutHandler);
                ColServices.clsServices.GetCountryNameListnew(constr, codeid, FillCountryNames, ErrorHandler, TimeOutHandler);

                ColServices.clsServices.GetSelltypeCodeListnew(constr, codeid, FillSellingTypeCodes, ErrorHandler, TimeOutHandler);
                ColServices.clsServices.GetSelltypeNameListnew(constr, codeid, FillSellingTypeNames, ErrorHandler, TimeOutHandler);

                break;
            case "marketname":
                var select = document.getElementById("<%=ddlMarketName.ClientID%>");
                var codeid = select.options[select.selectedIndex].value;
                var selectname = document.getElementById("<%=ddlMarket.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;
                var connstr = document.getElementById("<%=txtconnection.ClientID%>");
                constr = connstr.value


                ColServices.clsServices.GetCountryCodeListnew(constr, codeid, FillCountryCodes, ErrorHandler, TimeOutHandler);
                ColServices.clsServices.GetCountryNameListnew(constr, codeid, FillCountryNames, ErrorHandler, TimeOutHandler);

                ColServices.clsServices.GetSelltypeCodeListnew(constr, codeid, FillSellingTypeCodes, ErrorHandler, TimeOutHandler);
                ColServices.clsServices.GetSelltypeNameListnew(constr, codeid, FillSellingTypeNames, ErrorHandler, TimeOutHandler);
                break;
            case "countrycode":
                var select = document.getElementById("<%=ddlCountry.ClientID%>");
                var codeid = select.options[select.selectedIndex].text;
                var selectname = document.getElementById("<%=ddlCountryName.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;
                var connstr = document.getElementById("<%=txtconnection.ClientID%>");
                constr = connstr.value


                ColServices.clsServices.GetCityCodeListnew(constr, codeid, FillCityCodes, ErrorHandler, TimeOutHandler);
                ColServices.clsServices.GetCityNameListnew(constr, codeid, FillCityNames, ErrorHandler, TimeOutHandler);
                break;
            case "countryname":
                var select = document.getElementById("<%=ddlCountryName.ClientID%>");
                var codeid = select.options[select.selectedIndex].value;
                var selectname = document.getElementById("<%=ddlCountry.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;
                var connstr = document.getElementById("<%=txtconnection.ClientID%>");
                constr = connstr.value


                ColServices.clsServices.GetCityCodeListnew(constr, codeid, FillCityCodes, ErrorHandler, TimeOutHandler);
                ColServices.clsServices.GetCityNameListnew(constr, codeid, FillCityNames, ErrorHandler, TimeOutHandler);
                break;
            case "sellingtypecode":
                var select = document.getElementById("<%=ddlSellingType.ClientID%>");
                var codeid = select.options[select.selectedIndex].text;
                var selectname = document.getElementById("<%=ddlSellingTypeName.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;
                var connstr = document.getElementById("<%=txtconnection.ClientID%>");
                constr = connstr.value


                ColServices.clsServices.GetCategoryCodeListnew(constr, codeid, FillCategoryCodes, ErrorHandler, TimeOutHandler);
                ColServices.clsServices.GetCategoryNameListnew(constr, codeid, FillCategoryNames, ErrorHandler, TimeOutHandler);
                break;
            case "sellingtypename":
                var select = document.getElementById("<%=ddlSellingTypeName.ClientID%>");
                var codeid = select.options[select.selectedIndex].value;
                var selectname = document.getElementById("<%=ddlSellingType.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;
                var connstr = document.getElementById("<%=txtconnection.ClientID%>");
                constr = connstr.value


                ColServices.clsServices.GetCategoryCodeListnew(constr, codeid, FillCategoryCodes, ErrorHandler, TimeOutHandler);
                ColServices.clsServices.GetCategoryNameListnew(constr, codeid, FillCategoryNames, ErrorHandler, TimeOutHandler);
                break;
            case "citycode":
                var select = document.getElementById("<%=ddlCity.ClientID%>");
                var selectname = document.getElementById("<%=ddlCityName.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;
                break;
            case "cityname":
                var select = document.getElementById("<%=ddlCityName.ClientID%>");
                var selectname = document.getElementById("<%=ddlCity.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;
                break;
            case "categorycode":
                var select = document.getElementById("<%=ddlCategory.ClientID%>");
                var selectname = document.getElementById("<%=ddlCategoryName.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;
                break;
            case "categoryname":
                var select = document.getElementById("<%=ddlCategoryName.ClientID%>");
                var selectname = document.getElementById("<%=ddlCategory.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;
                break;
        }
    }

    function FillCountryCodes(result) {
        var ddl = document.getElementById("<%=ddlCountry.ClientID%>");
        RemoveAll(ddl)
        for (var i = 0; i < result.length; i++) {
            var option = new Option(result[i].ListText, result[i].ListValue);
            ddl.options.add(option);
        }
        ddl.value = "[Select]";
    }

    function FillCountryNames(result) {
        var ddl = document.getElementById("<%=ddlCountryName.ClientID%>");
        RemoveAll(ddl)
        for (var i = 0; i < result.length; i++) {
            var option = new Option(result[i].ListText, result[i].ListValue);
            ddl.options.add(option);
        }
        ddl.value = "[Select]";
    }


    function FillSellingTypeCodes(result) {
        var ddl = document.getElementById("<%=ddlSellingType.ClientID%>");
        RemoveAll(ddl)
        for (var i = 0; i < result.length; i++) {
            var option = new Option(result[i].ListText, result[i].ListValue);
            ddl.options.add(option);
        }
        ddl.value = "[Select]";
    }

    function FillSellingTypeNames(result) {
        var ddl = document.getElementById("<%=ddlSellingTypeName.ClientID%>");
        RemoveAll(ddl)
        for (var i = 0; i < result.length; i++) {
            var option = new Option(result[i].ListText, result[i].ListValue);
            ddl.options.add(option);
        }
        ddl.value = "[Select]";
    }

    function FillCityCodes(result) {
        var ddl = document.getElementById("<%=ddlCity.ClientID%>");
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

    function FillCategoryCodes(result) {
        var ddl = document.getElementById("<%=ddlCategory.ClientID%>");
        RemoveAll(ddl)
        for (var i = 0; i < result.length; i++) {
            var option = new Option(result[i].ListText, result[i].ListValue);
            ddl.options.add(option);
        }
        ddl.value = "[Select]";
    }

    function FillCategoryNames(result) {
        var ddl = document.getElementById("<%=ddlCategoryName.ClientID%>");
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

</script>
<asp:UpdatePanel id="UpdatePanel1" runat="server">

        <contenttemplate>
<TABLE style="BORDER-RIGHT: gray 2px solid; BORDER-TOP: gray 2px solid; BORDER-LEFT: gray 2px solid; WIDTH: 945px; BORDER-BOTTOM: gray 2px solid">
<TBODY>
<TR>
    <TD class="td_cell" align="center" colSpan="1">
    <asp:Label id="lblHeading" runat="server" Text="Customer White Labelling"  CssClass="field_heading" width="100%"></asp:Label>
    </TD>
</TR>
<TR>
<TD style="COLOR: #000000; HEIGHT: 104px">
<TABLE>
<TBODY>
    <TR>
        <TD style="WIDTH: 68px" class="td_cell">Order By</TD>
        <TD style="WIDTH: 196px" class="td_cell"><SELECT style="WIDTH: 147px" id="ddlOrderBy" class="field_input" runat="server"> <OPTION value="angentcodedis" selected>Code</OPTION> <OPTION value="agentname">Name</OPTION></SELECT> </TD>
        <TD style="WIDTH: 39px" class="td_cell">&nbsp;<asp:Button 
                id="btnGo" onclick="btnGo_Click" runat="server" Text="Go" Width="32px" 
                CssClass="search_button"></asp:Button></TD>
        <TD style="WIDTH: 274px" class="td_cell"></TD>
        <TD style="WIDTH: 80px" class="td_cell">Agent Code</TD>
        <TD style="WIDTH: 148px"><INPUT style="WIDTH: 136px" id="txtCustomerCode" class="field_input" type=text runat="server" /></TD>
        <TD style="WIDTH: 15px" class="td_cell">Name</TD><TD style="WIDTH: 98px" class="td_cell"><asp:TextBox id="txtcustomername" tabIndex=2 runat="server" Width="219px" CssClass="field_input" MaxLength="100"></asp:TextBox></TD>
    </TR>
<TR>
    <TD style="WIDTH: 68px" class="td_cell">Market</TD>
    <TD style="WIDTH: 196px" class="td_cell"><SELECT style="WIDTH: 147px" id="ddlMarket" class="field_input" onchange="CallWebMethod('marketcode');" runat="server"> <OPTION value="[Select]" selected>[Select]</OPTION></SELECT> </TD>
    <TD style="WIDTH: 39px" class="td_cell">&nbsp;Name</TD>
    <TD style="WIDTH: 274px" class="td_cell"><SELECT style="WIDTH: 147px" id="ddlMarketName" class="field_input" onchange="CallWebMethod('marketname');" runat="server"> <OPTION value="[Select]" selected>[Select]</OPTION></SELECT> </TD>
    <TD style="WIDTH: 80px" class="td_cell">&nbsp;Selling Type</TD>
    <TD style="WIDTH: 148px"><SELECT style="WIDTH: 147px" id="ddlSellingType" class="field_input" onchange="CallWebMethod('sellingtypecode');" runat="server"> <OPTION value="[Select]" selected>[Select]</OPTION></SELECT> </TD>
    <TD style="WIDTH: 15px" class="td_cell">&nbsp;Name</TD>
    <TD style="WIDTH: 98px"><SELECT style="WIDTH: 147px" id="ddlSellingTypeName" class="field_input" onchange="CallWebMethod('sellingtypename');" runat="server"> <OPTION value="[Select]" selected>[Select]</OPTION></SELECT> </TD></TR>
    <TR><TD style="WIDTH: 68px" class="td_cell">Category</TD><TD style="WIDTH: 196px" class="td_cell"><SELECT style="WIDTH: 147px" id="ddlCategory" class="field_input" onchange="CallWebMethod('categorycode');" runat="server"> <OPTION value="[Select]" selected>[Select]</OPTION></SELECT> </TD>
    <TD style="WIDTH: 39px" class="td_cell">&nbsp;Name</TD>
    <TD style="WIDTH: 274px" class="td_cell"><SELECT style="WIDTH: 147px" id="ddlCategoryName" class="field_input" onchange="CallWebMethod('categoryname');" runat="server"> <OPTION value="[Select]" selected>[Select]</OPTION></SELECT> </TD>
    <TD style="WIDTH: 80px" class="td_cell">&nbsp;Country</TD><TD style="WIDTH: 148px"><SELECT style="WIDTH: 147px" id="ddlCountry" class="field_input" onchange="CallWebMethod('countrycode');" runat="server"> <OPTION value="[Select]" selected>[Select]</OPTION></SELECT> </TD>
    <TD style="WIDTH: 15px" class="td_cell">&nbsp;Name</TD>
    <TD style="WIDTH: 98px"><SELECT style="WIDTH: 147px" id="ddlCountryName" class="field_input" onchange="CallWebMethod('countryname');" runat="server"> <OPTION value="[Select]" selected>[Select]</OPTION></SELECT> </TD></TR><TR><TD style="WIDTH: 68px; HEIGHT: 22px" class="td_cell">City</TD><TD style="WIDTH: 196px; HEIGHT: 22px" class="td_cell"><SELECT style="WIDTH: 147px" id="ddlCity" class="field_input" onchange="CallWebMethod('citycode');" runat="server"> <OPTION value="[Select]" selected>[Select]</OPTION></SELECT> </TD><TD style="WIDTH: 39px; HEIGHT: 22px" class="td_cell">&nbsp;Name</TD><TD style="WIDTH: 274px; HEIGHT: 22px" class="td_cell"><SELECT style="WIDTH: 147px" id="ddlCityName" class="field_input" onchange="CallWebMethod('cityname');" runat="server"> <OPTION value="[Select]" selected>[Select]</OPTION></SELECT> </TD>
   <TD style=" HEIGHT: 22px"  colspan="2" class="td_cell">
       <asp:CheckBox ID="chkwhitelabelagent" runat="server" Text="White Labelling Agent" />   
   </TD>
      
     <TD style=" HEIGHT: 22px" colspan="2" class="td_cell">

      <asp:CheckBox ID="chkactive" runat="server" Text="Active"/>  
    </TD>
    
 </TR>
    
</TBODY></TABLE></TD>
</TR>
<tr>
 
<td colspan="6" height="10px" align="left" class="td_cell">
<asp:CheckBox ID="chkPwhitelabel" runat="server" Text="Penting White Labelling"/> 

</td>
</tr>
<tr>
  <td colspan="6" height="10px" align="center">
   <INPUT style="VISIBILITY: hidden; WIDTH: 12px; HEIGHT: 9px" id="txtconnection" type="text" runat="server" />
   <asp:Button id="btnFillList"  runat="server" Text="Search" Width="50px" CssClass="search_button"></asp:Button>&nbsp;
   <asp:Button id="btnClear"  runat="server" Text="Clear" Width="40px" CssClass="search_button"></asp:Button>&nbsp;
      <asp:Button ID="btnPrint" runat="server" CssClass="field_button"   Text="Report" />
   
  </td>

</tr>
<TR>
<TD class="td_cell" colspan="6">

          <table width="100%">
                    <tr>
                    <td>
                    <asp:GridView id="grdUploadClients" runat="server" Font-Size="10px" BackColor="White" Width="881px" CssClass="td_cell" AutoGenerateColumns="False" GridLines="Vertical" CellPadding="3" BorderWidth="1px" BorderStyle="None" BorderColor="#999999" AllowSorting="True" AllowPaging="True">
                    <FooterStyle BackColor="#6B6B9A" ForeColor="Black"></FooterStyle>
                    <Columns>
                    <asp:TemplateField visible="false" HeaderText="Code">
                        <ItemTemplate>
                            <asp:Label ID="lblagentCode" runat="server" Text='<%# Bind("angentcodedis") %>'></asp:Label>
                          
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:BoundField DataField="angentcodedis"  HeaderText="Code"></asp:BoundField>
                    <asp:BoundField DataField="agentname"  HeaderText="Agent Name"></asp:BoundField>
                   
                    <asp:TemplateField  HeaderText="White Lablling">
                        <ItemTemplate>
                            <asp:Label ID="lblwhitelable" runat="server" Text='<%# Bind("whitelablesdis") %>'></asp:Label>
                          
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:BoundField DataField="Markuphotelstring"  HeaderText="MarkupHotel"></asp:BoundField>
                    <asp:BoundField DataField="MarkupTransferstring"  HeaderText="MarkupTransfer"></asp:BoundField>
                    <asp:BoundField DataField="MarkupCarstring"  HeaderText="Markup CarRental"></asp:BoundField>
                    <asp:BoundField DataField="MarkupVisastring"  HeaderText="MarkupVisa"></asp:BoundField>
                    <asp:BoundField DataField="MarkupExcursionstring"  HeaderText="MarkupExcursion"></asp:BoundField>
                    <asp:BoundField DataField="MarkupGuidestring"  HeaderText="MarkupGuide"></asp:BoundField>
                    <asp:BoundField DataField="MarkupEntrancestring"  HeaderText="MarkupEntrance "></asp:BoundField>
                    <asp:BoundField DataField="MarkupJeepstring"  HeaderText="Markup Jeepride"></asp:BoundField>
                   
                    <asp:BoundField DataField="MarkupMealstring"  HeaderText="MarkupMeals"></asp:BoundField>
                    <asp:BoundField DataField="MarkupOthersstring"  HeaderText="Markupothers"></asp:BoundField>
                    <asp:BoundField DataField="MarkupAirmeetstring"  HeaderText="Markup Airport/Meet and greet"></asp:BoundField>
                    <asp:BoundField DataField="MarkupHandlingfeestring"  HeaderText="Markup HandlingFee"></asp:BoundField>
                    <asp:BoundField DataField="Activedis"  HeaderText="Active"></asp:BoundField>

                   
                    <%--<asp:ButtonField HeaderText="Action" Text="Add" CommandName="AddRow">
                    <ItemStyle ForeColor="Blue"></ItemStyle>
                    </asp:ButtonField>
                   
                    <asp:ButtonField HeaderText="Action" Text="Edit" CommandName="EditRow">
                    <ItemStyle ForeColor="Blue"></ItemStyle>
                    </asp:ButtonField>

                     <asp:ButtonField HeaderText="Action" Text="Remove" CommandName="DeleteRow">
                    <ItemStyle ForeColor="Blue"></ItemStyle>
                    </asp:ButtonField>
                    --%>
                    <asp:TemplateField  HeaderText="Action">
                        <ItemTemplate>
                             <asp:LinkButton ID="lnkadd" runat="server" CommandName="AddRow" CommandArgument ='<%# Ctype(Container,GridViewRow).RowIndex %>'>Add</asp:LinkButton>
                         </ItemTemplate>
                    </asp:TemplateField>   
                    
                      <asp:TemplateField  HeaderText="Action">
                        <ItemTemplate>
                             <asp:LinkButton ID="lnkedit" runat="server" CommandName="EditRow" CommandArgument ='<%# Ctype(Container,GridViewRow).RowIndex %>' >Edit</asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>   


                     <asp:TemplateField  HeaderText="Action">
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkdelete" runat="server" CommandName="DeleteRow" CommandArgument ='<%# Ctype(Container,GridViewRow).RowIndex %>' >Remove</asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>   
           
                     
                    <asp:ButtonField HeaderText="Action" Text="View" CommandName="ViewRow">
                    <ItemStyle ForeColor="Blue"></ItemStyle>
                    </asp:ButtonField>
                    
                    </Columns>

                    <RowStyle CssClass="grdRowstyle"  ForeColor="Black"></RowStyle>

                    <SelectedRowStyle BackColor="#008A8C" ForeColor="White" Font-Bold="True"></SelectedRowStyle>

                    <PagerStyle CssClass="grdpagerstyle"  ForeColor="Black" HorizontalAlign="Center"></PagerStyle>

                    <HeaderStyle CssClass="grdheader" ForeColor="White" Font-Bold="True"></HeaderStyle>

                    <AlternatingRowStyle CssClass="grdAternaterow"  Font-Size="10px"></AlternatingRowStyle>
                    </asp:GridView> <asp:Label id="lblMsg" runat="server" 
                                Text="Records not found. Please redefine search criteria" Font-Size="8pt" 
                                Font-Names="Verdana" Font-Bold="True" Width="357px" Visible="False" 
                                CssClass="lblmsg"></asp:Label>

                    </td>
                    </tr>
 </table>


 </TD>
 </TR>
            <TR>
            <TD style="HEIGHT: 22px"><asp:Button id="btnExit"  runat="server" Text="Exit" Width="42px" CssClass="field_button"></asp:Button>
            </TD>
            </TR>
          </TBODY>
        </TABLE>
            <asp:ScriptManagerProxy id="ScriptManagerProxy1" runat="server"><Services>
<asp:ServiceReference Path="~/clsServices.asmx"></asp:ServiceReference>
</Services>
</asp:ScriptManagerProxy> 
            
</contenttemplate>
    </asp:UpdatePanel>
</asp:Content>

