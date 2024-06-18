<%@ Page Language="VB" AutoEventWireup="false" CodeFile="ApproveCustomersforWeb.aspx.vb" Inherits="ApproveCustomersforWeb" MasterPageFile="~/MainPageMaster.master" Strict ="true" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="server">

<script language="javascript" type="text/javascript">
function checkNumber(e)
			{	    
			    	
				if ( (event.keyCode < 47 || event.keyCode > 57) )
				{
					return false;
	            }   
	         	
			}

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
        <contenttemplate>
<TABLE style="BORDER-RIGHT: gray 2px solid; BORDER-TOP: gray 2px solid; BORDER-LEFT: gray 2px solid; WIDTH: 784px; BORDER-BOTTOM: gray 2px solid"><TBODY><TR><TD class="td_cell" align=center colSpan=1><asp:Label id="lblHeading" runat="server" Text="Approve Customers for Web" CssClass="field_heading" Width="881px"></asp:Label></TD></TR><TR><TD style="COLOR: #000000"><TABLE><TBODY><TR><TD style="WIDTH: 68px" class="td_cell">Order By</TD><TD style="WIDTH: 84px" class="td_cell"><SELECT style="WIDTH: 147px" id="ddlOrderBy" class="drpdown" runat="server"> <OPTION value="agentcode" selected>Code</OPTION> <OPTION value="agentname">Name</OPTION></SELECT> </TD><TD style="WIDTH: 45px" class="td_cell">&nbsp;<asp:Button 
        id="btnGo" onclick="btnGo_Click" runat="server" Text="Go" 
        CssClass="search_button"></asp:Button></TD><TD style="WIDTH: 171px" class="td_cell"></TD><TD style="WIDTH: 80px" class="td_cell"></TD><TD style="WIDTH: 147px"></TD><TD style="WIDTH: 62px"></TD><TD style="WIDTH: 98px"></TD></TR><TR><TD style="WIDTH: 68px" class="td_cell">Market</TD><TD style="WIDTH: 84px" class="td_cell"><SELECT style="WIDTH: 147px" id="ddlMarket" class="drpdown" onchange="CallWebMethod('marketcode');" runat="server"> <OPTION value="[Select]" selected>[Select]</OPTION></SELECT> </TD><TD style="WIDTH: 45px" class="td_cell">&nbsp;Name</TD><TD style="WIDTH: 171px" class="td_cell"><SELECT style="WIDTH: 147px" id="ddlMarketName" class="drpdown" onchange="CallWebMethod('marketname');" runat="server"> <OPTION value="[Select]" selected>[Select]</OPTION></SELECT> </TD><TD style="WIDTH: 80px" class="td_cell">&nbsp;Selling Type</TD><TD style="WIDTH: 147px"><SELECT style="WIDTH: 147px" id="ddlSellingType" class="drpdown" onchange="CallWebMethod('sellingtypecode');" runat="server"> <OPTION value="[Select]" selected>[Select]</OPTION></SELECT> </TD><TD style="WIDTH: 62px" class="td_cell">&nbsp;Name</TD><TD style="WIDTH: 98px"><SELECT style="WIDTH: 147px" id="ddlSellingTypeName" class="drpdown" onchange="CallWebMethod('sellingtypename');" runat="server"> <OPTION value="[Select]" selected>[Select]</OPTION></SELECT> </TD></TR><TR><TD style="WIDTH: 68px" class="td_cell">Category</TD><TD style="WIDTH: 84px" class="td_cell"><SELECT style="WIDTH: 147px" id="ddlCategory" class="drpdown" onchange="CallWebMethod('categorycode');" runat="server"> <OPTION value="[Select]" selected>[Select]</OPTION></SELECT> </TD><TD style="WIDTH: 45px" class="td_cell">&nbsp;Name</TD><TD style="WIDTH: 171px" class="td_cell"><SELECT style="WIDTH: 147px" id="ddlCategoryName" class="drpdown" onchange="CallWebMethod('categoryname');" runat="server"> <OPTION value="[Select]" selected>[Select]</OPTION></SELECT> </TD><TD style="WIDTH: 80px" class="td_cell">&nbsp;Country</TD><TD style="WIDTH: 147px"><SELECT style="WIDTH: 147px" id="ddlCountry" class="drpdown" onchange="CallWebMethod('countrycode');" runat="server"> <OPTION value="[Select]" selected>[Select]</OPTION></SELECT> </TD><TD style="WIDTH: 62px" class="td_cell">&nbsp;Name</TD><TD style="WIDTH: 98px"><SELECT style="WIDTH: 147px" id="ddlCountryName" class="drpdown" onchange="CallWebMethod('countryname');" runat="server"> <OPTION value="[Select]" selected>[Select]</OPTION></SELECT> </TD></TR><TR><TD style="WIDTH: 68px" class="td_cell">City</TD><TD style="WIDTH: 84px" class="td_cell"><SELECT style="WIDTH: 147px" id="ddlCity" class="drpdown" onchange="CallWebMethod('citycode');" runat="server"> <OPTION value="[Select]" selected>[Select]</OPTION></SELECT> </TD><TD style="WIDTH: 45px" class="td_cell">&nbsp;Name</TD><TD style="WIDTH: 171px" class="td_cell"><SELECT style="WIDTH: 147px" id="ddlCityName" class="drpdown" onchange="CallWebMethod('cityname');" runat="server"> <OPTION value="[Select]" selected>[Select]</OPTION></SELECT> </TD><TD style="WIDTH: 80px" class="td_cell"></TD><TD style="WIDTH: 147px">
    <asp:Button id="btnFillList" onclick="btnFillList_Click" runat="server" 
        Text="Search" CssClass="search_button"></asp:Button>&nbsp;
<asp:Button id="btnClear" onclick="btnClear_Click" runat="server" Text="Clear" 
        CssClass="search_button"></asp:Button></TD><TD style="WIDTH: 62px">
    <input id="txtconnection" runat="server" style="visibility: hidden; width: 12px;
        height: 9px" type="text" /></TD><TD style="WIDTH: 98px"></TD></TR></TBODY></TABLE></TD></TR><TR><TD style="HEIGHT: 24px" class="td_cell"><asp:GridView id="grdUploadClients" runat="server" Font-Size="10px" CssClass="grdstyle" Width="881px" AllowSorting="True" BorderColor="#999999" BorderStyle="None" BorderWidth="1px" CellPadding="3" GridLines="Vertical" AutoGenerateColumns="False">
<FooterStyle CssClass="grdfooter"></FooterStyle>
<Columns>
<asp:TemplateField Visible="False" HeaderText="S. Agent Code"><EditItemTemplate>
                                                <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("agentcode") %>'></asp:TextBox>
                                            
            
</EditItemTemplate>
<ItemTemplate>
            <asp:Label id="lblagentCode" runat="server" Text='<%# Bind("agentcode") %>'></asp:Label> 
            
</ItemTemplate>
</asp:TemplateField>
<asp:BoundField DataField="agentcode" SortExpression="agentcode" HeaderText="Code"></asp:BoundField>
<asp:BoundField DataField="agentname" SortExpression="agentname" HeaderText="Customer Name"></asp:BoundField>
<asp:TemplateField HeaderText="Approve">
<ItemStyle HorizontalAlign="Center"></ItemStyle>

<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
<ItemTemplate>
                    <asp:CheckBox ID="chkApprove" runat="server" Width="26px" />
                
</ItemTemplate>
</asp:TemplateField>
    <asp:TemplateField HeaderText="Send Mail">
        <ItemStyle HorizontalAlign="Center" />
        <HeaderStyle HorizontalAlign="Center" />
        <ItemTemplate>
            <asp:CheckBox ID="chkSendMail" runat="server" Width="26px" />
        </ItemTemplate>
    </asp:TemplateField>
<asp:BoundField DataField="webemail" SortExpression="webemail" HeaderText="Email Id"></asp:BoundField>
<asp:BoundField DataField="webcontact" SortExpression="webcontact" HeaderText="Contact Person"></asp:BoundField>
<asp:BoundField DataField="webapprove" SortExpression="webapprove" HeaderText="Status"></asp:BoundField>
<asp:TemplateField HeaderText="Remove">
<ItemStyle HorizontalAlign="Center"></ItemStyle>

<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
<ItemTemplate>
                    <asp:CheckBox ID="chkRemove" runat="server" Width="26px" />
                
</ItemTemplate>
</asp:TemplateField>
<asp:BoundField DataField="webpassword" SortExpression="webpassword" HeaderText="Password"></asp:BoundField>
<asp:TemplateField Visible="False" HeaderText="WebPassword"><EditItemTemplate>
                                                <asp:TextBox ID="TextBox2" runat="server" Text='<%# Bind("webpassword") %>'></asp:TextBox>
                                            
            
</EditItemTemplate>
<ItemTemplate>
            <asp:Label id="lblWPassword" runat="server" Text='<%# Bind("webpassword") %>'></asp:Label> 
            
</ItemTemplate>
</asp:TemplateField>
</Columns>

<RowStyle CssClass="grdRowstyle"></RowStyle>

<SelectedRowStyle CssClass="grdselectrowstyle"></SelectedRowStyle>

<PagerStyle CssClass="grdpagerstyle" HorizontalAlign="Center"></PagerStyle>

<HeaderStyle CssClass="grdheader" ForeColor="White"></HeaderStyle>

<AlternatingRowStyle CssClass="grdAternaterow"></AlternatingRowStyle>
</asp:GridView> <asp:Label id="lblMsg" runat="server" Text="Records not found. Please redefine search criteria" Font-Size="8pt" Font-Names="Verdana" ForeColor="084573" Font-Bold="True" Width="357px" Visible="False"></asp:Label></TD></TR><TR><TD style="HEIGHT: 22px">
<asp:Button id="btnSendMail" onclick="btnSendMail_Click" runat="server" 
            Text="Send Mail to Selected" CssClass="btn"></asp:Button>&nbsp;
 <asp:Button id="btnSelectforApprove" onclick="btnSelectforApprove_Click" 
            runat="server" Text="Select All for Approve" CssClass="btn"></asp:Button>&nbsp;
  <asp:Button id="btnApprove" onclick="btnApprove_Click" runat="server" 
            Text="Approve Selected" CssClass="btn"></asp:Button>&nbsp;
   <asp:Button id="btnSelectforRemove" onclick="btnSelectforRemove_Click" 
            runat="server" Text="Select All for Remove" CssClass="btn"></asp:Button>&nbsp;
    <asp:Button id="btnDeletefromWeb" onclick="btnDeletefromWeb_Click" runat="server" 
            Text="Remove Selected From Web " CssClass="btn"></asp:Button>&nbsp;
     <asp:Button id="btnExit" onclick="btnExit_Click" runat="server" Text="Exit" 
            CssClass="btn"></asp:Button></TD></TR></TBODY></TABLE><asp:ScriptManagerProxy id="ScriptManagerProxy1" runat="server"><Services>
<asp:ServiceReference Path="~/clsServices.asmx"></asp:ServiceReference>
</Services>
</asp:ScriptManagerProxy> 
</contenttemplate>
    </asp:UpdatePanel>
</asp:Content>
