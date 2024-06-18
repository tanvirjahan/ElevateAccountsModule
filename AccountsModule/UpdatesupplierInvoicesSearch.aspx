<%@ Page Language="VB" MasterPageFile="~/AccountsMaster.master" AutoEventWireup="false" CodeFile="UpdatesupplierInvoicesSearch.aspx.vb"   Inherits="AccountsModule_UpdatesupplierInvoicesSearch"  %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">

<script src="../Content/js/jquery-1.5.1.min.js" type="text/javascript"></script>
<script src="../Content/js/JqueryUI.js" type="text/javascript"></script>
<script src="../Content/js/accounts.js" type="text/javascript"></script>
<link type ="text/css" href ="../Content/css/JqueryUI.css" rel ="Stylesheet" />

<script language="javascript" type="text/javascript">
var ddlTyp=null;
var ddlSuppCode =null;
var ddlSppName =null;
var ddlPostCode =null;
var ddlPostName =null;
var txtPostCd =null;
var txtPostNm =null;


function CallWebMethod(methodType) {
    switch (methodType) {
        case "ddlSupplierCode":
            var select = document.getElementById("<%=ddlSupplierCode.ClientID%>");
            var selectname = document.getElementById("<%=ddlSpplierName.ClientID%>");
            selectname.value = select.options[select.selectedIndex].text;
            break;
        case "ddlSupplierName":
            var select = document.getElementById("<%=ddlSpplierName.ClientID%>");
            var selectname = document.getElementById("<%=ddlSupplierCode.ClientID%>");
            selectname.value = select.options[select.selectedIndex].text;
            break;
    }
}

function FillSupplier(ddlTy,lblCustCd,lblCustnm,ddlSuppCd,ddlSppNm,ddlPostCd,ddlPostNm)
{
    ddlTyp=document.getElementById(ddlTy);
    var strQryCode="";
    var strQryName="";
    var strQryPostToCode="";
    var strQryPostToName="";
    lblcustcode = document.getElementById(lblCustCd);
    lblcustname = document.getElementById(lblCustnm);
    
    ddlSuppCode =document.getElementById(ddlSuppCd);
    ddlSppName =document.getElementById(ddlSppNm);
    ddlPostCode =document.getElementById(ddlPostCd);
    ddlPostName =document.getElementById(ddlPostNm);
    
    var strcap=ddlTyp.options[ddlTyp.selectedIndex].text;
    if (ddlTyp.value=="S")
    {
      lblcustcode.innerHTML=strcap+'Code <font color="Red"> *</font>';
    lblcustname.innerHTML=strcap+'Name';
    strQryCode="select distinct Code,des from view_account where type ='S' order by code ";
    strQryName="select distinct  des,Code from view_account where type ='S'order by des ";
//    strQryPostToCode="select distinct view_account.postaccount,partymast.partyname from view_account,partymast where view_account.postaccount=partymast.partycode and view_account.postaccount is not null AND view_account.TYPE='S'";
//    strQryPostToName="select distinct partymast.partyname,view_account.postaccount from view_account,partymast where view_account.postaccount=partymast.partycode and view_account.postaccount is not null AND view_account.TYPE='S'";
// 
    strQryPostToCode="select distinct  Code as postaccount,des as partyname  from view_account where type ='S' order by code ";
    strQryPostToName="select distinct  des as partyname ,Code as postaccount  from view_account where type ='S'order by des ";

    }
    else if (ddlTyp.value=="A")
    {
     lblcustcode.innerHTML=strcap+'Code <font color="Red"> *</font>';
     lblcustname.innerHTML=strcap+'Name';
     strQryCode="select distinct  Code,des from view_account where type ='A'";
     strQryName="select  distinct des,Code from view_account where type ='A'";
     strQryPostToCode="select distinct  Code as postaccount,des as partyname  from view_account where type ='A' order by code ";
     strQryPostToName="select distinct des as partyname ,Code as postaccount  from view_account where type ='A' order by des ";

//    strQryPostToCode="select distinct view_account.postaccount,supplier_agents.supagentname from view_account,supplier_agents where view_account.postaccount=supplier_agents.supagentcode and view_account.postaccount is not null AND view_account.TYPE='A'";
//    strQryPostToName="select distinct supplier_agents.supagentname,view_account.postaccount from view_account,supplier_agents where view_account.postaccount=supplier_agents.supagentcode and view_account.postaccount is not null AND view_account.TYPE='A'";
    }
    else
    {
    lblcustcode.innerHTML='Code <font color="Red"> *</font>';
    lblcustname.innerHTML='Name';

    strQryCode="select distinct Code,des from view_account  where   type in('S','A') order by code ";
    strQryName="select distinct des,Code from view_account  where   type in('S','A') order by des ";
    strQryPostToCode="select distinct   Code as postaccount,des as partyname  from view_account where   type in('S','A') order by code ";
    strQryPostToName="select distinct  des as partyname ,Code as postaccount  from view_account where   type in('S','A')  order by des ";
    }
var connstr=document.getElementById("<%=txtconnection.ClientID%>");  
constr=connstr.value   

    
    ColServices.clsServices.GetQueryReturnStringListnew(constr,strQryCode,FillSupplierCode,ErrorHandler,TimeOutHandler);
    ColServices.clsServices.GetQueryReturnStringListnew(constr,strQryName,FillSupplierName,ErrorHandler,TimeOutHandler);
    
    ColServices.clsServices.GetQueryReturnStringListnew(constr,strQryPostToCode,FillPostToCode,ErrorHandler,TimeOutHandler);
    ColServices.clsServices.GetQueryReturnStringListnew(constr,strQryPostToName,FillPostToName,ErrorHandler,TimeOutHandler);
    
}
function suppliercodechange(ddlTy,ddlSuppCd,ddlSppNm,ddlPostCd,ddlPostNm)
     {
     ddlTyp=document.getElementById(ddlTy);
     ddlSuppCode =document.getElementById(ddlSuppCd);
     ddlSppName =document.getElementById(ddlSppNm);
     ddlPostCode =document.getElementById(ddlPostCd);
     ddlPostName =document.getElementById(ddlPostNm);
    
     var select=ddlSuppCode;
     var selectname=ddlSppName;
     selectname.value=select.options[select.selectedIndex].text;
     FillPostAccs();
}
             
function suppliernamechange(ddlTy,ddlSuppCd,ddlSppNm,ddlPostCd,ddlPostNm)
{
     ddlTyp=document.getElementById(ddlTy);
     ddlSuppCode =document.getElementById(ddlSuppCd);
     ddlSppName =document.getElementById(ddlSppNm);
     ddlPostCode =document.getElementById(ddlPostCd);
     ddlPostName =document.getElementById(ddlPostNm);
     var select=ddlSppName;
     var selectname=ddlSuppCode;
     selectname.value=select.options[select.selectedIndex].text;
     FillPostAccs();
}

function postcodechange(ddlPostCd,ddlPostNm,txtPoCd,txtPoNm)
     {
           ddlPostCode =document.getElementById(ddlPostCd);
           ddlPostName =document.getElementById(ddlPostNm);
           txtPostCd =document.getElementById(txtPoCd);
           txtPostNm =document.getElementById(txtPoNm);
           var select=ddlPostCode;
           var codeid=select.options[select.selectedIndex].text;
           var selectname=ddlPostName;
           selectname.value=select.options[select.selectedIndex].text;
           txtPostCd.value=select.options[select.selectedIndex].value;
           txtPostNm.value=select.options[select.selectedIndex].text;
}
function postnamechange(ddlPostCd,ddlPostNm,txtPoCd,txtPoNm)
     {
           ddlPostCode =document.getElementById(ddlPostCd);
           ddlPostName =document.getElementById(ddlPostNm);
           txtPostCd =document.getElementById(txtPoCd);
           txtPostNm =document.getElementById(txtPoNm);
           var select=ddlPostName;
           var codeid=select.options[select.selectedIndex].text;
           var selectname=ddlPostCode;
           selectname.value=select.options[select.selectedIndex].text;
           txtPostCd.value=select.options[select.selectedIndex].text;
           txtPostNm.value=select.options[select.selectedIndex].value;
}

function FillPostAccs()
{
 
 var ddlSCode= ddlSuppCode;
  if ( (ddlTyp.value !='[Select]') && (ddlSCode.value!='[Select]'))
    {
    strQryPostToCode="select distinct  Code as postaccount,des as partyname  from view_account where type ='"+ddlTyp.value +"' and code <> '"+ ddlSCode.options[ddlSCode.selectedIndex].text +"' order by code ";
    strQryPostToName="select distinct   des as partyname ,Code as postaccount  from view_account where type ='"+ddlTyp.value +"' and code <> '"+ ddlSCode.options[ddlSCode.selectedIndex].text +"'  order by des ";
    }
    else if  ( (ddlTyp.value =='[Select]') && (ddlSCode.value!='[Select]'))
    {
    strQryPostToCode="select distinct  Code as postaccount,des as partyname  from view_account where    type in('S','A')and  code <> '"+ ddlSCode.options[ddlSCode.selectedIndex].text +"' order by code ";
    strQryPostToName="select distinct   des as partyname ,Code as postaccount  from view_account where     type in('S','A') and code <> '"+ ddlSCode.options[ddlSCode.selectedIndex].text +"'  order by des ";
    }
    else if ( (ddlTyp.value !='[Select]') && (ddlSCode.value =='[Select]'))
    {
    strQryPostToCode="select distinct  Code as postaccount,des as partyname  from view_account where type ='"+ddlTyp.value +"'  order by code ";
    strQryPostToName="select distinct   des as partyname ,Code as postaccount  from view_account where type ='"+ddlTyp.value +"'  order by des ";
    }
    else
    {
    strQryPostToCode="select distinct Code as postaccount,des as partyname  from view_account  where   type in('S','A') order by code ";
    strQryPostToName="select distinct  des as partyname ,Code as postaccount  from view_account where   type in('S','A')  order by des ";
    }
    
var connstr=document.getElementById("<%=txtconnection.ClientID%>");  
constr=connstr.value   

    
    ColServices.clsServices.GetQueryReturnStringListnew(constr,strQryPostToCode,FillPostToCode,ErrorHandler,TimeOutHandler);
    ColServices.clsServices.GetQueryReturnStringListnew(constr,strQryPostToName,FillPostToName,ErrorHandler,TimeOutHandler);
}
function FillSupplierCode(result)
    {
      var ddlSCode=ddlSuppCode;
        RemoveAll(ddlSCode)
        for(var i=0;i<result.length;i++)
        {
            var option=new Option(result[i].ListText,result[i].ListValue);
            ddlSCode.options.add(option);
        }
        ddlSCode.value="[Select]";
    }

function FillSupplierName(result)
    {
      var ddlSName=ddlSppName;
        RemoveAll(ddlSName)
        for(var i=0;i<result.length;i++)
        {
            var option=new Option(result[i].ListText,result[i].ListValue);
            ddlSName.options.add(option);
        }
        ddlSName.value="[Select]";
    }

    
    function FillPostToCode(result)
    {
      var ddlPCode=ddlPostCode;
        RemoveAll(ddlPCode)
        for(var i=0;i<result.length;i++)
        {
            var option=new Option(result[i].ListText,result[i].ListValue);
            ddlPCode.options.add(option);
        }
        ddlPCode.value="[Select]";
    }

function FillPostToName(result)
    {
      var ddlPName=ddlPostName;
        RemoveAll(ddlPName)
        for(var i=0;i<result.length;i++)
        {
            var option=new Option(result[i].ListText,result[i].ListValue);
            ddlPName.options.add(option);
        }
        ddlPName.value="[Select]";
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


    <table>
        <tr>
            <td style="width: 100%">
    <asp:UpdatePanel id="UpdatePanel1" runat="server">
        <contenttemplate>
<script type="text/javascript">
//       var prm = Sys.WebForms.PageRequestManager.getInstance();
//            prm.add_beginRequest(function () {
//
//            });

//            prm.add_endRequest(function () {
//                MyAutoRouteFillArray();

//            }); 
</script>


<TABLE style="WIDTH: 912px" class="td_cell">
<TBODY><TR><TD style="TEXT-ALIGN: center" class="field_heading" colSpan=4>
    Update Supplier Invoices</TD></TR><TR><TD style="TEXT-ALIGN: center" colSpan=4><asp:RadioButton id="rbtnsearch" onCheckedChanged="rbtnsearch_CheckedChanged" runat="server" Text="Search" ForeColor="Black" CssClass="td_cell" AutoPostBack="True" GroupName="GrSearch" Checked="True"></asp:RadioButton><asp:RadioButton id="rbtnadsearch"  onCheckedChanged="rbtnadsearch_CheckedChanged" runat="server" Text="Advance Search" ForeColor="Black" CssClass="td_cell" AutoPostBack="True" GroupName="GrSearch"></asp:RadioButton>&nbsp;<asp:Button 
        id="btnSearch" tabIndex=12 runat="server" Text="Search" Font-Bold="False" 
        CssClass="search_button"></asp:Button>&nbsp;<asp:Button id="btnClear" 
        tabIndex=13 runat="server" Text="Clear" Font-Bold="False" 
        CssClass="search_button"></asp:Button>&nbsp;<asp:Button id="btnHelp" tabIndex=14 onclick="btnHelp_Click" runat="server" Text="Help" CssClass="search_button"></asp:Button> 
  &nbsp;  
    <asp:Button id="btnAddNew" tabIndex=15 runat="server" Text="Add New" 
        Font-Bold="True" CssClass="btn"></asp:Button> &nbsp;
    <asp:Button id="btnPrint" tabIndex=17 runat="server" Text="Report" 
        CssClass="btn"></asp:Button></TD></TR><TR><TD colSpan=4><TABLE style="WIDTH: 644px" class="td_cell"><TBODY><TR><TD style="WIDTH: 139px"><asp:Label id="Label1" runat="server" Text="Purchase Invoice No" Width="140px" CssClass="field_caption"></asp:Label></TD><TD style="WIDTH: 179px">
        <INPUT style="WIDTH: 194px" id="txtInvoiceNo" class="txtbox" tabIndex=1 
            type=text runat="server" /></TD><TD><asp:Label id="Label7" runat="server" Text="Order By" Width="140px" CssClass="field_caption"></asp:Label></TD><TD>
        <asp:DropDownList id="ddlOrderBy" tabIndex=11 runat="server" Width="200px" 
            CssClass=drpdown AutoPostBack="True"><asp:ListItem Value="0">Tran Id DESC</asp:ListItem>
<asp:ListItem Value="1">Tran Id ASC</asp:ListItem>
<asp:ListItem Value="2">Tran Date Date </asp:ListItem>
<asp:ListItem Value="3">Type</asp:ListItem>
<asp:ListItem Value="4">Supplier</asp:ListItem>
<asp:ListItem Value="6">From Date</asp:ListItem>
<asp:ListItem Value="7">To Date</asp:ListItem>
</asp:DropDownList></TD></TR></TBODY></TABLE></TD></TR><TR><TD colSpan=4>
        <asp:Panel id="pnlSearch" runat="server"><TABLE style="WIDTH: 746px" class="td_cell"><TBODY><TR><TD style="WIDTH: 1318px"></TD>
            <TD style="height: 22px;"><asp:Label id="Label4" runat="server" Text="Type" Width="140px" CssClass="field_caption"></asp:Label>
                <select id="ddlType" runat="server" class="drpdown" 
            style="WIDTH: 200px" tabindex="2"> 
                    <option selected="" value="[Select]">[Select]</option>
                    <option value="S">Supplier</option>
                    <option value="A">Supplier Agent</option>
        </select>
        </TD>
            <TD style="WIDTH: 345px">&nbsp;</TD><TD 
            style="height: 22px;">           
            <input id="suppsearch" runat="server" 
            class="field_input MyAutosupplierCompleteClass" name="suppSearch" 
            onfocus="MyAutosupplier_rptFillArray();" style="width:98% ; font " 
            type="text" />

            </TD></TR>
            <TR><td style="WIDTH: 1318px">
                <asp:Label ID="lblCustCode" runat="server" Text="Supplier  Code " 
                Width="140px"></asp:Label> </td><TD>
                    <SELECT style="WIDTH: 200px" id="ddlSupplierCode"  class="drpdown" 
            tabIndex=4  runat="server" onchange="CallWebMethod('ddlSupplierCode');"></SELECT>&#160;</TD>
                <TD style="width: 345px;"><asp:Label ID="lblCustName" runat="server" 
            CssClass="field_caption" Text=" Supplier Name" Width="140px"></asp:Label></TD><TD 
            colspan="1">
                    <select id="ddlSpplierName" runat="server"  class="drpdown MyDropDownListsuppValue"
            style="WIDTH: 300px" tabindex="5" onchange="CallWebMethod('ddlSupplierName');">
        </select>
        </TD></TR>
            <TR><TD style="WIDTH: 1318px">
                &nbsp;</TD>
                <TD>
                    &nbsp;</TD>
                <TD colSpan=1 style="WIDTH: 345px">&nbsp;</TD><TD 
            colspan="1">
                    &nbsp;</TD></TR>
            <TR><TD style="WIDTH: 1318px">
                <asp:Label ID="Label2" runat="server" CssClass="field_caption" 
            Text="From Date" Width="140px"></asp:Label></TD>
                <TD><asp:TextBox ID="txtFromDate" runat="server" CssClass="txtbox" 
            tabIndex="8" ValidationGroup="MKE" Width="88px"></asp:TextBox>
                    <asp:ImageButton ID="imgbtnFromDate" runat="server" 
            ImageUrl="~/Images/Calendar_scheduleHS.png">
                    </asp:ImageButton>&#160;<cc1:MaskedEditValidator ID="MEVFromDate" 
            runat="server" ControlExtender="MEFromDate" ControlToValidate="txtFromDate" 
            CssClass="field_error" Display="Dynamic" 
            EmptyValueMessage="From date is required" 
            InvalidValueBlurredMessage="Invalid from date" 
            InvalidValueMessage="Invalid Date" 
            TooltipMessage="Enter a date in dd/mm/yyyy format"></cc1:MaskedEditValidator></TD>
                <TD colSpan=1 style="WIDTH: 345px">&#160;<br />
                    <asp:Label ID="Label3" 
            runat="server" CssClass="field_caption" Text="To Date " 
            Width="140px"></asp:Label></TD><TD colSpan=1>
                    <asp:TextBox ID="txtToDate" runat="server" CssClass="txtbox" 
            tabIndex="9" ValidationGroup="MKE" Width="88px"></asp:TextBox> 
                    <asp:ImageButton ID="imgbtnToDate" runat="server" 
            ImageUrl="~/Images/Calendar_scheduleHS.png">
                    </asp:ImageButton> 
                    <cc1:MaskedEditValidator ID="MEVToDate" runat="server" 
            ControlExtender="METoDate" ControlToValidate="txtToDate" CssClass="field_error" 
            Display="Dynamic" EmptyValueMessage="From date is required" 
            InvalidValueBlurredMessage="Invalid from date" 
            InvalidValueMessage="Invalid Date" 
            TooltipMessage="Enter a date in dd/mm/yyyy format"></cc1:MaskedEditValidator>
                    <input id="txtPostCode" runat="server" maxlength="20" 
            style="VISIBILITY: hidden; WIDTH: 9px; HEIGHT: 3px" type="text" /><input 
            id="txtPostName" runat="server" maxlength="20" 
            style="VISIBILITY: hidden; WIDTH: 9px; HEIGHT: 3px" type="text" />
    <input id="txtconnection" runat="server" style="visibility: hidden; width: 12px;
        height: 9px" type="text" /></TD></TR></TBODY></TABLE></asp:Panel></TD></TR><TR><TD colSpan=4><TABLE style="WIDTH: 434px" cols=3><TBODY><TR><TD style="WIDTH: 129px"><SPAN style="FONT-SIZE: 8pt; FONT-FAMILY: Arial"><asp:Label id="Label8" runat="server" Text="Report Type" Width="140px" CssClass="field_caption"></asp:Label></SPAN></TD><TD style="WIDTH: 23px">
        <asp:DropDownList id="ddlReportType" tabIndex=10 runat="server" 
            CssClass=drpdown><asp:ListItem Value="Brief">Brief</asp:ListItem>
<asp:ListItem Value="Detailed">Detailed</asp:ListItem>
</asp:DropDownList></TD><TD style="WIDTH: 226px"><SPAN style="FONT-SIZE: 7pt; COLOR: #ff0000"><asp:Label id="Label9" runat="server" Text="Applicable Only for the Report" Width="206px" CssClass="field_caption"></asp:Label></SPAN></TD></TR></TBODY></TABLE></TD></TR></TBODY></TABLE>
<cc1:CalendarExtender id="CEFromDate" runat="server" Format="dd/MM/yyyy" PopupButtonID="imgbtnFromDate" TargetControlID="txtFromDate"></cc1:CalendarExtender> <cc1:MaskedEditExtender id="MEFromDate" runat="server" TargetControlID="txtFromDate" Mask="99/99/9999" MaskType="Date"></cc1:MaskedEditExtender> <cc1:CalendarExtender id="CEToDate" runat="server" Format="dd/MM/yyyy" PopupButtonID="imgbtnToDate" TargetControlID="txtToDate"></cc1:CalendarExtender> <cc1:MaskedEditExtender id="METoDate" runat="server" TargetControlID="txtToDate" Mask="99/99/9999" MaskType="Date"></cc1:MaskedEditExtender> 
 </contenttemplate>
    </asp:UpdatePanel>
    
    </td>
        </tr>
        <tr>
            <td>
                <asp:Button ID="btnExportToExcel" runat="server"
                        CssClass="btn" TabIndex="16" Text="Export To Excel" />&nbsp;</td>
        </tr>
        <tr>
            <td>

                <asp:UpdatePanel id="UpdatePanel2" runat="server">
                    <contenttemplate>

<asp:GridView id="gv_SearchResult" tabIndex=18 runat="server" Width="100%" CssClass="grdstyle" GridLines="Vertical" CellPadding="3"  AutoGenerateColumns="False" AllowSorting="True" AllowPaging="True">
<Columns>
<asp:TemplateField SortExpression="tran_id" HeaderText="Tran Id"><ItemTemplate>
<asp:Label id="lblInvoiceNo" runat="server" Text='<%# Bind("tran_id") %>'></asp:Label>
</ItemTemplate>
</asp:TemplateField>
<asp:BoundField DataField="tran_type" HeaderText="Status"></asp:BoundField>
<asp:BoundField HtmlEncode="False" DataFormatString="{0:dd/MM/yyyy}" DataField="tran_date" SortExpression="tran_date" HeaderText="Date"></asp:BoundField>
<asp:BoundField DataField="partycode" SortExpression="partycode" HeaderText="Supplier"></asp:BoundField>
<asp:BoundField DataField="partyname" SortExpression="partyname" HeaderText="Supplier Name"></asp:BoundField>
<asp:BoundField DataField="glcode" SortExpression="glcode" HeaderText="Gl Code"></asp:BoundField>
<asp:BoundField HtmlEncode="False" DataFormatString="{0:dd/MM/yyyy}" DataField="fromdate" SortExpression="fromdate" HeaderText="From Date"></asp:BoundField>
<asp:BoundField HtmlEncode="False" DataFormatString="{0:dd/MM/yyyy}" DataField="todate" SortExpression="todate" HeaderText="To Date"></asp:BoundField>
<asp:BoundField DataField="currcode" SortExpression="currcode" HeaderText="Currcode"></asp:BoundField>
<asp:BoundField DataField="convrate" SortExpression="convrate" HeaderText="Convrate"></asp:BoundField>
<asp:BoundField DataField="remarks" SortExpression="remarks" HeaderText="Remarks"></asp:BoundField>
<asp:BoundField DataField="adddate" SortExpression="adddate" HeaderText="Date Created"></asp:BoundField>
<asp:BoundField DataField="adduser" SortExpression="adduser" HeaderText="User Created"></asp:BoundField>
<asp:BoundField DataField="moddate" SortExpression="moddate" HeaderText="Date Modified"></asp:BoundField>
<asp:BoundField DataField="moduser" SortExpression="moduser" HeaderText="User Modified"></asp:BoundField>

<asp:ButtonField HeaderText="Action" Text="Edit" CommandName="EditRow">
<ItemStyle ForeColor="Blue"></ItemStyle>
</asp:ButtonField>

<asp:ButtonField HeaderText="Action" Text="Delete" CommandName="DeleteRow">
<ItemStyle ForeColor="Blue"></ItemStyle>
</asp:ButtonField>

<asp:ButtonField HeaderText="Action" Text="View" CommandName="View">

<ItemStyle ForeColor="Blue"></ItemStyle>
</asp:ButtonField>
</Columns>

    <FooterStyle CssClass="grdfooter" />

<RowStyle CssClass="grdRowstyle"></RowStyle>
<SelectedRowStyle CssClass="grdselectrowstyle" ></SelectedRowStyle>
<PagerStyle CssClass="grdpagerstyle" HorizontalAlign="Center"></PagerStyle>
<HeaderStyle  CssClass="grdheader" ForeColor="white"></HeaderStyle>
<AlternatingRowStyle CssClass="grdAternaterow"></AlternatingRowStyle>
</asp:GridView>

<asp:Label id="lblMsg" runat="server" Text="Records not found, Please redefine search criteria" 
                            Font-Size="8pt" Font-Names="Verdana" Font-Bold="True" Width="357px" 
                            Visible="False" CssClass="lblmsg"></asp:Label> 
</contenttemplate>
                </asp:UpdatePanel></td>
        </tr>
        <tr>
            <td>
                <asp:ScriptManagerProxy id="ScriptManagerProxy1" runat="server">
                <services>
<asp:ServiceReference Path="~/clsServices.asmx"></asp:ServiceReference>
</services>
                </asp:ScriptManagerProxy>
            </td>
        </tr>
    </table>


</asp:Content>

