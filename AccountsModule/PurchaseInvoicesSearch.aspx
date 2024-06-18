<%@ Page Language="VB" MasterPageFile="~/AccountsMaster.master" AutoEventWireup="false" CodeFile="PurchaseInvoicesSearch.aspx.vb" Inherits="AccountsModule_PurchaseInvoicesSearch"  %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">

<script language="javascript" type="text/javascript">
var ddlTyp=null;
var ddlSuppCode =null;
var ddlSppName =null;
var ddlPostCode =null;
var ddlPostName =null;
var txtPostCd =null;
var txtPostNm =null;


function CallWebMethod(methodType)
{
// switch(methodType)
//        {
//            case "partycode":
//                var select=document.getElementById("<%=ddlSupplierCode.ClientID%>");
//                var codeid=select.options[select.selectedIndex].text;
//                var selectname=document.getElementById("<%=ddlSpplierName.ClientID%>");
//                selectname.value=select.options[select.selectedIndex].text;
//                FillPostAccs();
//                break;
//             case "partyname":
//                var select=document.getElementById("<%=ddlSpplierName.ClientID%>");
//                var codeid=select.options[select.selectedIndex].text;
//                var selectname=document.getElementById("<%=ddlSupplierCode.ClientID%>");
//                selectname.value=select.options[select.selectedIndex].text;
//                 FillPostAccs();
//                break;
//             case "posttocode":
//                var select=document.getElementById("<%=ddlPostToCode.ClientID%>");
//                var codeid=select.options[select.selectedIndex].text;
//                var selectname=document.getElementById("<%=ddlPostToName.ClientID%>");
//                selectname.value=select.options[select.selectedIndex].text;
//                break;
//             case "posttoname":
//                var select=document.getElementById("<%=ddlPostToName.ClientID%>");
//                var codeid=select.options[select.selectedIndex].text;
//                var selectname=document.getElementById("<%=ddlPostToCode.ClientID%>");
//                selectname.value=select.options[select.selectedIndex].text;
//                break;
//           }
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
            <td style="width: 100px">
    <asp:UpdatePanel id="UpdatePanel1" runat="server">
        <contenttemplate>
<TABLE style="WIDTH: 912px" class="td_cell"><TBODY><TR><TD style="TEXT-ALIGN: center" class="field_heading" colSpan=4>Purchase Invoice List</TD></TR><TR><TD style="TEXT-ALIGN: center" colSpan=4><asp:RadioButton id="rbtnsearch" onCheckedChanged="rbtnsearch_CheckedChanged" runat="server" Text="Search" ForeColor="Black" CssClass="td_cell" AutoPostBack="True" GroupName="GrSearch" Checked="True"></asp:RadioButton><asp:RadioButton id="rbtnadsearch"  onCheckedChanged="rbtnadsearch_CheckedChanged" runat="server" Text="Advance Search" ForeColor="Black" CssClass="td_cell" AutoPostBack="True" GroupName="GrSearch"></asp:RadioButton>&nbsp;<asp:Button 
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
            CssClass=drpdown AutoPostBack="True"><asp:ListItem Value="0">Puchase Invoice no DESC</asp:ListItem>
<asp:ListItem Value="1">Puchase Invoice no ASC</asp:ListItem>
<asp:ListItem Value="2">Puchase Invoice Date </asp:ListItem>
<asp:ListItem Value="3">Type</asp:ListItem>
<asp:ListItem Value="4">Supplier</asp:ListItem>
<asp:ListItem Value="5">Supplier Invoice No</asp:ListItem>
<asp:ListItem Value="6">From Date</asp:ListItem>
<asp:ListItem Value="7">To Date</asp:ListItem>
</asp:DropDownList></TD></TR></TBODY></TABLE></TD></TR><TR><TD colSpan=4><asp:Panel id="pnlSearch" runat="server" Visible="False"><TABLE style="WIDTH: 746px" class="td_cell"><TBODY><TR><TD style="WIDTH: 1318px"></TD>
            <TD style="height: 22px;"><asp:Label id="Label4" runat="server" Text="Type" Width="140px" CssClass="field_caption"></asp:Label>
                <select id="ddlType" runat="server" class="drpdown" 
            style="WIDTH: 200px" tabindex="2"> 
                    <option selected="" value="[Select]">[Select]</option>
                    <option value="S">Supplier</option>
                    <option value="A">Supplier Agent</option>
        </select>
        </TD>
            <TD style="WIDTH: 345px"><asp:Label ID="Label13" runat="server" 
            CssClass="field_caption" Text="Status" Width="140px"></asp:Label></TD><TD 
            style="height: 22px;">
                <select id="ddlStatus" runat="server" class="td_cell_web" 
            style="WIDTH: 200px" tabindex="3"> 
                    <option selected="" value="[Select]">[Select]</option>
                     
                    <option value="P">Posted</option>
                     <option value="U">UnPosted</option>
        </select>
        </TD></TR>
            <TR><td style="WIDTH: 1318px">
                <asp:Label ID="lblCustCode" runat="server" Text="Supplier  Code " 
                Width="140px"></asp:Label> </td><TD>
                    <SELECT style="WIDTH: 200px" id="ddlSupplierCode" class="drpdown" 
            tabIndex=4  runat="server"></SELECT>&#160;</TD>
                <TD style="width: 345px;"><asp:Label ID="lblCustName" runat="server" 
            CssClass="field_caption" Text=" Supplier Name" Width="140px"></asp:Label></TD><TD 
            colspan="1">
                    <select id="ddlSpplierName" runat="server" class=drpdown 
            style="WIDTH: 300px" tabindex="5">
        </select>
        </TD></TR>
            <TR><TD style="WIDTH: 1318px">
                <asp:Label ID="Label5" runat="server" CssClass="field_caption" 
            Text="Post Account Code" Width="140px"></asp:Label></TD>
                <TD>
                    <select id="ddlPostToCode" runat="server" class=drpdown 
            style="WIDTH: 200px" tabindex="6">
        </select>
        </TD>
                <TD colSpan=1 style="WIDTH: 345px"><asp:Label ID="Label6" 
            runat="server" CssClass="field_caption" Text="Post Account  Name" Width="140px"></asp:Label></TD><TD 
            colspan="1">
                    <select id="ddlPostToName" runat="server" class=drpdown 
            style="WIDTH: 300px" tabindex="7">
        </select>
        </TD></TR>
            <TR><TD style="WIDTH: 1318px">
                <asp:Label ID="Label2" runat="server" CssClass="field_caption" 
            Text="From Purchase Invoice Date" Width="140px"></asp:Label></TD>
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
                <TD colSpan=1 style="WIDTH: 345px">&#160;<br /><asp:Label ID="Label3" 
            runat="server" CssClass="field_caption" Text="To Puchase Invoice Date " 
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
</asp:DropDownList></TD><TD style="WIDTH: 226px"><SPAN style="FONT-SIZE: 7pt; COLOR: #ff0000"><asp:Label id="Label9" runat="server" Text="Applicable Only for the Report" Width="206px" CssClass="field_caption"></asp:Label></SPAN></TD></TR></TBODY></TABLE></TD></TR></TBODY></TABLE><cc1:CalendarExtender id="CEFromDate" runat="server" Format="dd/MM/yyyy" PopupButtonID="imgbtnFromDate" TargetControlID="txtFromDate"></cc1:CalendarExtender> <cc1:MaskedEditExtender id="MEFromDate" runat="server" TargetControlID="txtFromDate" Mask="99/99/9999" MaskType="Date"></cc1:MaskedEditExtender> <cc1:CalendarExtender id="CEToDate" runat="server" Format="dd/MM/yyyy" PopupButtonID="imgbtnToDate" TargetControlID="txtToDate"></cc1:CalendarExtender> <cc1:MaskedEditExtender id="METoDate" runat="server" TargetControlID="txtToDate" Mask="99/99/9999" MaskType="Date"></cc1:MaskedEditExtender> 
</contenttemplate>
    </asp:UpdatePanel></td>
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
<asp:GridView id="gv_SearchResult" tabIndex=18 runat="server" Width="950px" CssClass="grdstyle" GridLines="Vertical" CellPadding="3"  AutoGenerateColumns="False" AllowSorting="True" AllowPaging="True">
<Columns>
<asp:TemplateField SortExpression="tran_id" HeaderText="Purchase Invoice No"><ItemTemplate>
<asp:Label id="lblInvoiceNo" runat="server" Text='<%# Bind("tran_id") %>'></asp:Label>
</ItemTemplate>
</asp:TemplateField>
<asp:BoundField DataField="post_state" HeaderText="Status"></asp:BoundField>
<asp:BoundField HtmlEncode="False" DataFormatString="{0:dd/MM/yyyy}" DataField="tran_date" SortExpression="tran_date" HeaderText="Purchase Invoice Date"></asp:BoundField>
<asp:BoundField DataField="acctype" SortExpression="acctype" HeaderText="Type"></asp:BoundField>
<asp:BoundField DataField="acc_code" SortExpression="acc_code" HeaderText="Supplier"></asp:BoundField>
<asp:BoundField DataField="accname" SortExpression="accname" HeaderText="Supplier Name"></asp:BoundField>
<asp:BoundField DataField="postaccount" SortExpression="postaccount" HeaderText="Post To"></asp:BoundField>
<asp:BoundField DataField="supinvno" SortExpression="supinvno" HeaderText="Supplier Invocie No"></asp:BoundField>
<asp:BoundField HtmlEncode="False" DataFormatString="{0:dd/MM/yyyy}" DataField="fromdate" SortExpression="fromdate" HeaderText="From Date"></asp:BoundField>
<asp:BoundField HtmlEncode="False" DataFormatString="{0:dd/MM/yyyy}" DataField="todate" SortExpression="todate" HeaderText="To Date"></asp:BoundField>
<asp:BoundField DataField="adddate" SortExpression="adddate" HeaderText="Date Created"></asp:BoundField>
<asp:BoundField DataField="adduser" SortExpression="adduser" HeaderText="User Created"></asp:BoundField>
<asp:BoundField DataField="moddate" SortExpression="moddate" HeaderText="Date Modified"></asp:BoundField>
<asp:BoundField DataField="moduser" SortExpression="moduser" HeaderText="User Modified"></asp:BoundField>
<asp:ButtonField HeaderText="Action" Text="Edit" CommandName="EditRow">
<ItemStyle ForeColor="Blue"></ItemStyle>
</asp:ButtonField>
<asp:ButtonField HeaderText="Action" Text="View" CommandName="View">
<ItemStyle ForeColor="Blue"></ItemStyle>
</asp:ButtonField>
<asp:ButtonField HeaderText="Action" Text="Delete" CommandName="DeleteRow">
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

