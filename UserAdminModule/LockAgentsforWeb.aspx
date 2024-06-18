<%@ Page Language="VB" AutoEventWireup="false" CodeFile="LockAgentsforWeb.aspx.vb" Inherits="LockAgentsforWeb" MasterPageFile="~/UserAdminMaster.master" Strict ="true"  %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
    
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="server">

<script language="JavaScript" type="text/javascript" >
    window.history.forward(1);  
</script>
<script language="javascript" type="text/javascript">

function CallWebMethod(methodType)
    {
       switch(methodType)
        {
            case "marketcode":
            
                var select=document.getElementById("<%=ddlMarket.ClientID%>");
                var codeid=select.options[select.selectedIndex].text;
                var selectname=document.getElementById("<%=ddlMarketName.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;
                
                var connstr=document.getElementById("<%=txtconnection.ClientID%>");  
                constr=connstr.value  

                
                ColServices.clsServices.GetCountryCodeListnew(constr,codeid,FillCountryCodes,ErrorHandler,TimeOutHandler);
                ColServices.clsServices.GetCountryNameListnew(constr,codeid,FillCountryNames,ErrorHandler,TimeOutHandler);
                                
                ColServices.clsServices.GetSelltypeCodeListnew(constr,codeid,FillSellingTypeCodes,ErrorHandler,TimeOutHandler);
                ColServices.clsServices.GetSelltypeNameListnew(constr,codeid,FillSellingTypeNames,ErrorHandler,TimeOutHandler);
                
               break;
            case "marketname":
                var select=document.getElementById("<%=ddlMarketName.ClientID%>");
                var codeid=select.options[select.selectedIndex].value;
                var selectname=document.getElementById("<%=ddlMarket.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;
                
                var connstr=document.getElementById("<%=txtconnection.ClientID%>");  
                constr=connstr.value  

                
                ColServices.clsServices.GetCountryCodeListnew(constr,codeid,FillCountryCodes,ErrorHandler,TimeOutHandler);
                ColServices.clsServices.GetCountryNameListnew(constr,codeid,FillCountryNames,ErrorHandler,TimeOutHandler);
                                
                ColServices.clsServices.GetSelltypeCodeListnew(constr,codeid,FillSellingTypeCodes,ErrorHandler,TimeOutHandler);
                ColServices.clsServices.GetSelltypeNameListnew(constr,codeid,FillSellingTypeNames,ErrorHandler,TimeOutHandler);
                break;
            case "countrycode":
                var select=document.getElementById("<%=ddlCountry.ClientID%>");
                var codeid=select.options[select.selectedIndex].text;
                var selectname=document.getElementById("<%=ddlCountryName.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;
                
                var connstr=document.getElementById("<%=txtconnection.ClientID%>");  
                constr=connstr.value  

                
                ColServices.clsServices.GetCityCodeListnew(constr,codeid,FillCityCodes,ErrorHandler,TimeOutHandler);
                ColServices.clsServices.GetCityNameListnew(constr,codeid,FillCityNames,ErrorHandler,TimeOutHandler);
                break;
            case "countryname":
                var select=document.getElementById("<%=ddlCountryName.ClientID%>");
                var codeid=select.options[select.selectedIndex].value;
                var selectname=document.getElementById("<%=ddlCountry.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;
                
                var connstr=document.getElementById("<%=txtconnection.ClientID%>");  
                constr=connstr.value  
              
                ColServices.clsServices.GetCityCodeListnew(constr,codeid,FillCityCodes,ErrorHandler,TimeOutHandler);
                ColServices.clsServices.GetCityNameListnew(constr,codeid,FillCityNames,ErrorHandler,TimeOutHandler);
                break;
            case "sellingtypecode":
                var select=document.getElementById("<%=ddlSellingType.ClientID%>");
                var codeid=select.options[select.selectedIndex].text;
                var selectname=document.getElementById("<%=ddlSellingTypeName.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;
                
                var connstr=document.getElementById("<%=txtconnection.ClientID%>");  
                constr=connstr.value  

                
                ColServices.clsServices.GetCategoryCodeListnew(constr,codeid,FillCategoryCodes,ErrorHandler,TimeOutHandler);
                ColServices.clsServices.GetCategoryNameListnew(constr,codeid,FillCategoryNames,ErrorHandler,TimeOutHandler);
                break;
            case "sellingtypename":
                var select=document.getElementById("<%=ddlSellingTypeName.ClientID%>");
                var codeid=select.options[select.selectedIndex].value;
                var selectname=document.getElementById("<%=ddlSellingType.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;
                var connstr=document.getElementById("<%=txtconnection.ClientID%>");  
                constr=connstr.value  

                
                ColServices.clsServices.GetCategoryCodeListnew(constr,codeid,FillCategoryCodes,ErrorHandler,TimeOutHandler);
                ColServices.clsServices.GetCategoryNameListnew(constr,codeid,FillCategoryNames,ErrorHandler,TimeOutHandler);
                break;
           case "citycode":
                var select=document.getElementById("<%=ddlCity.ClientID%>");
                var selectname=document.getElementById("<%=ddlCityName.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;
              break;
            case "cityname":
                var select=document.getElementById("<%=ddlCityName.ClientID%>");
                var selectname=document.getElementById("<%=ddlCity.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;
               break; 
            case "categorycode":
                var select=document.getElementById("<%=ddlCategory.ClientID%>");
                var selectname=document.getElementById("<%=ddlCategoryName.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;
              break;
            case "categoryname":
                var select=document.getElementById("<%=ddlCategoryName.ClientID%>");
                var selectname=document.getElementById("<%=ddlCategory.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;
               break;      
        }
    }
    
 function FillCountryCodes(result)
    {
      	var ddl = document.getElementById("<%=ddlCountry.ClientID%>");
 		RemoveAll(ddl)
        for(var i=0;i<result.length;i++)
        {
            var option=new Option(result[i].ListText,result[i].ListValue);
            ddl.options.add(option);
        }
        ddl.value="[Select]";
    }

    function FillCountryNames(result)
    {
        var ddl = document.getElementById("<%=ddlCountryName.ClientID%>");
 		RemoveAll(ddl)
        for(var i=0;i<result.length;i++)
        {
            var option=new Option(result[i].ListText,result[i].ListValue);
            ddl.options.add(option);
        }
        ddl.value="[Select]";
    }


    function FillSellingTypeCodes(result)
    {
      	var ddl = document.getElementById("<%=ddlSellingType.ClientID%>");
 		RemoveAll(ddl)
        for(var i=0;i<result.length;i++)
        {
            var option=new Option(result[i].ListText,result[i].ListValue);
            ddl.options.add(option);
        }
        ddl.value="[Select]";
    }

    function FillSellingTypeNames(result)
    {
        var ddl = document.getElementById("<%=ddlSellingTypeName.ClientID%>");
 		RemoveAll(ddl)
        for(var i=0;i<result.length;i++)
        {
            var option=new Option(result[i].ListText,result[i].ListValue);
            ddl.options.add(option);
        }
        ddl.value="[Select]";
    }
    
    function FillCityCodes(result)
    {
      	var ddl = document.getElementById("<%=ddlCity.ClientID%>");
 		RemoveAll(ddl)
        for(var i=0;i<result.length;i++)
        {
            var option=new Option(result[i].ListText,result[i].ListValue);
            ddl.options.add(option);
        }
        ddl.value="[Select]";
    }

    function FillCityNames(result)
    {
        var ddl = document.getElementById("<%=ddlCityName.ClientID%>");
 		RemoveAll(ddl)
        for(var i=0;i<result.length;i++)
        {
            var option=new Option(result[i].ListText,result[i].ListValue);
            ddl.options.add(option);
        }
        ddl.value="[Select]";
    }
    
    function FillCategoryCodes(result)
    {
      	var ddl = document.getElementById("<%=ddlCategory.ClientID%>");
 		RemoveAll(ddl)
        for(var i=0;i<result.length;i++)
        {
            var option=new Option(result[i].ListText,result[i].ListValue);
            ddl.options.add(option);
        }
        ddl.value="[Select]";
    }

    function FillCategoryNames(result)
    {
        var ddl = document.getElementById("<%=ddlCategoryName.ClientID%>");
 		RemoveAll(ddl)
        for(var i=0;i<result.length;i++)
        {
            var option=new Option(result[i].ListText,result[i].ListValue);
            ddl.options.add(option);
        }
        ddl.value="[Select]";
    }
    
    function TimeOutHandler(result)
    {
        alert("Timeout :" + result);
    }

    function ErrorHandler(result)
    {
        var msg=result.get_exceptionType() + "\r\n";
        msg += result.get_message() + "\r\n";
        msg += result.get_stackTrace();
        alert(msg);
    }		

</script>

    <asp:UpdatePanel id="UpdatePanel1" runat="server">
    <Triggers>
        <asp:PostBackTrigger ControlID="btnExportToExcel"/>
    </Triggers>
    <contenttemplate>
<TABLE style="BORDER-RIGHT: gray 2px solid; BORDER-TOP: gray 2px solid; BORDER-LEFT: gray 2px solid; WIDTH: 1px; BORDER-BOTTOM: gray 2px solid; HEIGHT: 324px"><TBODY><TR><TD style="WIDTH: 917px; HEIGHT: 18px" class="td_cell" align=center colSpan=1><asp:Label id="lblHeading" runat="server" Text="Lock Agents for Web" CssClass="field_heading" Width="601px"></asp:Label></TD></TR><TR><TD style="WIDTH: 917px; COLOR: #000000; HEIGHT: 31px" vAlign=top><TABLE style="WIDTH: 516px"><TBODY><TR><TD style="WIDTH: 362px; HEIGHT: 6px" class="td_cell">Market</TD><TD style="WIDTH: 476px; HEIGHT: 6px" class="td_cell" colSpan=2><SELECT style="WIDTH: 153px" id="ddlMarket" class="drpdown" onchange="CallWebMethod('marketcode');" runat="server"> <OPTION value="[Select]" selected>[Select]</OPTION></SELECT></TD><TD style="WIDTH: 95px; HEIGHT: 6px" class="td_cell">Name</TD><TD style="WIDTH: 431px; HEIGHT: 6px" class="td_cell"><SELECT style="WIDTH: 195px" id="ddlMarketName" class="drpdown" onchange="CallWebMethod('marketname');" runat="server"> <OPTION value="[Select]" selected>[Select]</OPTION></SELECT></TD></TR><TR><TD style="WIDTH: 362px; HEIGHT: 6px" class="td_cell">Selling Type</TD><TD style="WIDTH: 476px; HEIGHT: 6px" class="td_cell" colSpan=2><SELECT style="WIDTH: 153px" id="ddlSellingType" class="drpdown" onchange="CallWebMethod('sellingtypecode');" runat="server"> <OPTION value="[Select]" selected>[Select]</OPTION></SELECT> </TD><TD style="WIDTH: 95px; HEIGHT: 6px" class="td_cell">Name</TD><TD style="WIDTH: 431px; HEIGHT: 6px" class="td_cell"><SELECT style="WIDTH: 195px" id="ddlSellingTypeName" class="drpdown" onchange="CallWebMethod('sellingtypename');" runat="server"> <OPTION value="[Select]" selected>[Select]</OPTION></SELECT> </TD></TR><TR><TD style="WIDTH: 362px; HEIGHT: 6px" class="td_cell">Category</TD><TD style="WIDTH: 476px; HEIGHT: 6px" class="td_cell" colSpan=2><SELECT style="WIDTH: 153px" id="ddlCategory" class="drpdown" onchange="CallWebMethod('categorycode');" runat="server"> <OPTION value="[Select]" selected>[Select]</OPTION></SELECT> </TD><TD style="WIDTH: 95px; HEIGHT: 6px" class="td_cell">Name</TD><TD style="WIDTH: 431px; HEIGHT: 6px" class="td_cell"><SELECT style="WIDTH: 195px" id="ddlCategoryName" class="drpdown" onchange="CallWebMethod('categoryname');" runat="server"> <OPTION value="[Select]" selected>[Select]</OPTION></SELECT> </TD></TR><TR><TD style="WIDTH: 362px; HEIGHT: 6px" class="td_cell">Country</TD><TD style="WIDTH: 476px; HEIGHT: 6px" class="td_cell" colSpan=2><SELECT style="WIDTH: 153px" id="ddlCountry" class="drpdown" onchange="CallWebMethod('countrycode');" runat="server"> <OPTION value="[Select]" selected>[Select]</OPTION></SELECT></TD><TD style="WIDTH: 95px; HEIGHT: 6px" class="td_cell">Name</TD><TD style="WIDTH: 431px; HEIGHT: 6px" class="td_cell"><SELECT style="WIDTH: 195px" id="ddlCountryName" class="drpdown" onchange="CallWebMethod('countryname');" runat="server"> <OPTION value="[Select]" selected>[Select]</OPTION></SELECT></TD></TR><TR><TD style="WIDTH: 362px; HEIGHT: 6px" class="td_cell">City</TD><TD style="WIDTH: 476px; HEIGHT: 6px" class="td_cell" colSpan=2><SELECT style="WIDTH: 153px" id="ddlCity" class="drpdown" onchange="CallWebMethod('citycode');" runat="server"> <OPTION value="[Select]" selected>[Select]</OPTION></SELECT></TD><TD style="WIDTH: 95px; HEIGHT: 6px" class="td_cell">Name</TD><TD style="WIDTH: 431px; HEIGHT: 6px" class="td_cell"><SELECT style="WIDTH: 195px" id="ddlCityName" class="drpdown" onchange="CallWebMethod('cityname');" runat="server"> <OPTION value="[Select]" selected>[Select]</OPTION></SELECT></TD></TR><TR><TD style="WIDTH: 362px; HEIGHT: 25px" class="td_cell">
    <input id="txtconnection" runat="server" style="visibility: hidden; width: 12px;
        height: 9px" type="text" /></TD><TD style="WIDTH: 476px; HEIGHT: 25px" class="td_cell" colSpan=2></TD><TD style="WIDTH: 95px; HEIGHT: 25px" class="td_cell">
        <asp:Button id="btnDisplay" onclick="btnDisplay_Click" runat="server" 
        Text="Display" CssClass="btn"></asp:Button></TD>
        <TD style="WIDTH: 485px; HEIGHT: 25px" class="td_cell" vAlign=bottom>
    <asp:Button id="btnClear" 
        onclick="btnClear_Click" runat="server" Text="Clear" CssClass="btn"></asp:Button>&nbsp;<asp:Button id="btnExportToExcel" onclick="btnExportToExcel_Click" 
        runat="server" Text="Export To Excel" CssClass="btn"></asp:Button></TD></TR><TR><TD style="HEIGHT: 15px" class="td_cell" align=center colSpan=5></TD></TR><TR><TD class="td_cell" vAlign=top align=left colSpan=5><asp:GridView id="grdSupplier" runat="server" Font-Size="10px" CssClass="grdstyle" Width="594px" AllowSorting="True" BorderColor="#999999" BorderStyle="None" BorderWidth="1px" CellPadding="3" GridLines="Vertical" AutoGenerateColumns="False">
<FooterStyle CssClass="grdfooter"></FooterStyle>
<Columns>
<asp:BoundField DataField="agentcode" HeaderText="Customer Code" SortExpression="agentcode"></asp:BoundField>
<asp:BoundField DataField="agentname" HeaderText="Customer Name" SortExpression="agentname"></asp:BoundField>
<asp:TemplateField HeaderText="Lock"><ItemTemplate>
<asp:CheckBox id="chkSelect" runat="server" CssClass="chkbox"></asp:CheckBox>
</ItemTemplate>
</asp:TemplateField>
    <asp:TemplateField HeaderText="Reason">
        <ItemTemplate>
            <asp:TextBox ID="txtReason" runat="server" CssClass="txtbox" MaxLength="200"
                Width="203px"></asp:TextBox>
        </ItemTemplate>
    </asp:TemplateField>
</Columns>

<RowStyle CssClass="grdRowstyle"></RowStyle>

<SelectedRowStyle CssClass="grdselectrowstyle"></SelectedRowStyle>

<PagerStyle CssClass="grdpagerstyle" HorizontalAlign="Center"></PagerStyle>

<HeaderStyle CssClass="grdheader" ForeColor="white"></HeaderStyle>

<AlternatingRowStyle CssClass="grdAternaterow"></AlternatingRowStyle>
</asp:GridView> </TD></TR><TR><TD style="HEIGHT: 22px" class="td_cell" align=left colSpan=5>
<asp:Label id="lblMsg" runat="server" 
            Text="Records not found, Please redefine search criteria" Font-Size="8pt" 
            Font-Names="Verdana" Font-Bold="True" Width="353px" Visible="False" 
            CssClass="lblmsg"></asp:Label></TD></TR><TR>
<TD style="HEIGHT: 22px" class="td_cell" align=right colSpan=5><asp:Button id="btnSave" onclick="btnSave_Click" runat="server" Text="Save" CssClass="btn" Width="48px"></asp:Button>&nbsp;
<asp:Button id="btnExit" onclick="btnExit_Click" runat="server" Text="Exit" CssClass="btn" Width="48px"></asp:Button> </TD></TR></TBODY></TABLE></TD></TR></TBODY></TABLE><asp:ScriptManagerProxy id="ScriptManagerProxy1" runat="server"><Services>
<asp:ServiceReference Path="~/clsServices.asmx"></asp:ServiceReference>
</Services>
</asp:ScriptManagerProxy> 
</contenttemplate>
    </asp:UpdatePanel>
</asp:Content>