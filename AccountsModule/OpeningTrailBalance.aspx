<%@ Page Language="VB" MasterPageFile="~/SubPageMaster.master" AutoEventWireup="false" CodeFile="OpeningTrailBalance.aspx.vb" Inherits="OpeningTrailBalance"  %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
    <%@ OutputCache location="none" %> 
<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">
    <script src="../Content/js/jquery-1.5.1.min.js" type="text/javascript"></script>
<script src="../Content/js/JqueryUI.js" type="text/javascript"></script>
<script src="../Content/js/accounts.js" type="text/javascript"></script>
<link type ="text/css" href ="../Content/css/JqueryUI.css" rel ="Stylesheet" />

<script language="javascript" type="text/javascript">
//----------------------------------------
//Currates
var txtcurrate=null;
var nodecround =null;
function FillAccountCode(ddlIccd,ddlIcnm,txtcurr,txtcurate,txtnar)
{  

    ddlIccode = document.getElementById(ddlIccd);
    ddlIcname = document.getElementById(ddlIcnm);
           
    var codeid=ddlIccode.options[ddlIccode.selectedIndex].text;
    ddlIcname.value=codeid;
    
    var  curr=document.getElementById("<%=ddlcurr.ClientID%>");
    curr.value=codeid;
    var codeCur=curr.options[ddlIccode.selectedIndex].text;  
    
    txtcurr=document.getElementById(txtcurr);  
    
    txtcurrate=document.getElementById(txtcurate);
    var sqlstr;
    
if ((codeCur == '[Select]')||(trim(codeCur) == ""))
    {
    txtcurr.value='';
    txtcurrate.value=0;
    }
    else
    {    
    txtcurr.value=codeCur; 
    var connstr=document.getElementById("<%=txtconnection.ClientID%>");  
    constr=connstr.value   
    
    sqlstr="select isnull(convrate,0) from currrates where currcode='"+codeCur +"' and tocurr in(select option_selected from reservation_parameters  where param_id=457)";
    ColServices.clsServices.GetQueryReturnStringValuenew(constr,sqlstr,FillCurrrates,ErrorHandler,TimeOutHandler);
    } 
      txtnarr = document.getElementById(txtnar);
      var  txtnarrs=document.getElementById("<%=txtDocDesc.ClientID%>");
      txtnarr.value=txtnarrs.value;
    
}
function FillCurrrates(result)
{
txtcurrate.value=result;
}
 
 
function FillAccountName(ddlIccd,ddlIcnm,txtcurr,txtcurate,txtnar)

{
    
    ddlIccode = document.getElementById(ddlIccd);
    ddlIcname = document.getElementById(ddlIcnm);
    
    var codeid=ddlIcname.options[ddlIcname.selectedIndex].text;
   
    ddlIccode.value=codeid;
    
    var  curr=document.getElementById("<%=ddlcurr.ClientID%>");
    curr.value=codeid;
    var codeCur=curr.options[ddlIccode.selectedIndex].text;  
    
    txtcurr=document.getElementById(txtcurr);  
    txtcurrate=document.getElementById(txtcurate);
    var sqlstr;
    
     if ((codeCur == '[Select]')||(trim(codeCur) == ""))
       {
       txtcurr.value='';    
       txtcurrate.value=0
      }
      else
      {
      txtcurr.value=codeCur; 
    var connstr=document.getElementById("<%=txtconnection.ClientID%>");  
    constr=connstr.value   
       
      sqlstr="select isnull(convrate,0) from currrates where currcode='"+codeCur +"' and tocurr in(select option_selected from reservation_parameters  where param_id=457)";
      ColServices.clsServices.GetQueryReturnStringValuenew(constr,sqlstr,FillCurrrates,ErrorHandler,TimeOutHandler);
      }  
      txtnarr = document.getElementById(txtnar);
      var  txtnarrs=document.getElementById("<%=txtDocDesc.ClientID%>");
      txtnarr.value=txtnarrs.value; 
    
}
function trim(stringToTrim) {
	return stringToTrim.replace(/^\s+|\s+$/g,"");	
}
//-----------------------------------------
//----------------------------------------
function FillCostCentCode(ddlIccd,ddlIcnm)
{  
    ddlIccode = document.getElementById(ddlIccd);
    ddlIcname = document.getElementById(ddlIcnm);
           
    var codeid=ddlIccode.options[ddlIccode.selectedIndex].text;
    ddlIcname.value=codeid;
                      
}

function FillCostCentName(ddlIccd,ddlIcnm)
{
    ddlIccode = document.getElementById(ddlIccd);
    ddlIcname = document.getElementById(ddlIcnm);
    
    var codeid=ddlIcname.options[ddlIcname.selectedIndex].text;
   
    ddlIccode.value=codeid;
           
}
//-----------------------------------------
function checkNumberDecimal(evt,txt) 
    {
      
    evt = (evt) ? evt : event;
    var charCode = (evt.charCode) ? evt.charCode : ((evt.keyCode) ? evt.keyCode : 
        ((evt.which) ? evt.which : 0));
    //if (charCode != 47 && (charCode > 45 && charCode < 58)) {    
        if (charCode != 47 && (charCode > 44 && charCode < 58)) { 
           var value=txt.value;
           var indx=value.indexOf('.');
           var deci=document.getElementById("<%=txtdecimal.ClientID%>");
           var lngLenght =3;  
           if (indx < 0 ){
           return true;
           }
           
           var digit=value.substring(indx+1);
           if(digit.length>lngLenght-1)
            {
            return false;
                }
            {
            return true;
                }
       }
       return false;
    }

//---------------------------------------------------
function TotalAccgrd1()
{

    var  txtdec=document.getElementById("<%=txtdecimal.ClientID%>");
//    nodecround=Math.pow(10,parseInt(txtdec.value));
    nodecround = Math.pow(10, 3);

    var objGridView = document.getElementById('<%=grdAcc1.ClientID%>');
    var totC = 0 ;
    var totD = 0 ;

    

    var txtrowcnt =document.getElementById('<%=txtgridrows1.ClientID%>');
    intRows = txtrowcnt.value;
    //intRows = 1;
     for(j=1;j<=intRows;j++)
    {

      //  alert(objGridView.rows[j].cells[7].children[0].value);
       

       var valC=objGridView.rows[j].cells[7].children[0].value;
       var valD = objGridView.rows[j].cells[8].children[0].value;

      
     
       if (valC==''){valC=0;}
       if (valD==''){valD=0;}
       totC=parseFloat(totC)+parseFloat(valC);
       totD=parseFloat(totD)+parseFloat(valD);
   }


    var txttotCrB1 = document.getElementById('<%=txtTotCrBace1.ClientID%>');
    var txttotCrD1 = document.getElementById('<%=txtTotDbBace1.ClientID%>');

    
    
    txttotCrB1.value=Math.round(totD*nodecround)/nodecround;
    txttotCrD1.value=Math.round(totC*nodecround)/nodecround;
    
   
    var objGridView2 = document.getElementById('<%=grdAcc2.ClientID%>');
 
    totC=0;
    totD=0;
   
     var txtrowcnt =document.getElementById('<%=txtgridrows2.ClientID%>');
    intRows=txtrowcnt.value;
    for(j=1;j<=intRows;j++)
    {
       var valC=objGridView2.rows[j].cells[7].children[0].value;
       var valD=objGridView2.rows[j].cells[8].children[0].value;
       if (valC==''){valC=0;}
       if (valD==''){valD=0;}
       totC=parseFloat(totC)+parseFloat(valC);
       totD=parseFloat(totD)+parseFloat(valD);
    }
    var txttotCrB2 = document.getElementById('<%=txtTotCrBace2.ClientID%>');
    var txttotCrD2 = document.getElementById('<%=txtTotDbBace2.ClientID%>');
    txttotCrB2.value=Math.round(totD*nodecround)/nodecround;
    txttotCrD2.value=Math.round(totC*nodecround)/nodecround;
    
    var txttotCrD = document.getElementById('<%=txtTotDbBace.ClientID%>');
    var txttotCrB = document.getElementById('<%=txtTotCrBace.ClientID%>');

   

    var totcrb=parseFloat(txttotCrB1.value) + parseFloat(txttotCrB2.value);
    txttotCrB.value=Math.round(totcrb*nodecround)/nodecround;
    
    var totcrd=parseFloat(txttotCrD1.value) + parseFloat(txttotCrD2.value);
    txttotCrD.value=Math.round(totcrd*nodecround)/nodecround;
   
    var txttotNet = document.getElementById('<%=txtTotalNetAmt.ClientID%>');
    var totnet=parseFloat(txttotCrB.value) - parseFloat(txttotCrD.value);
    txttotNet.value=Math.round( totnet*nodecround)/nodecround;
    
    
    var txttotBal = document.getElementById('<%=txtTotalPartyBal.ClientID%>');
    
    var txttotDiff = document.getElementById('<%=txtDiffAmt.ClientID%>');
    totdif=parseFloat(txttotNet.value) -  parseFloat(txttotBal.value);
    txttotDiff.value=Math.round(totdif*nodecround)/nodecround;
    
    var txtDrs = document.getElementById('<%=txtDr.ClientID%>');
    if (totnet> 0)
    {
    txtDrs.value='Cr';
    }
    else
    {
    txtDrs.value='Dr';
    }
     
}
 
//---------------------------------------------------
 function  convertInRate(txtCD,txtDBAmt,txtCnvtRate,txtCnvtAmt)
 {
    var  txtdec=document.getElementById("<%=txtdecimal.ClientID%>");
    nodecround=Math.pow(10,parseInt(txtdec.value));
    
    txtDBAmt = document.getElementById(txtDBAmt);
    txtCD = document.getElementById(txtCD);
    if(txtDBAmt.value>0)
    {
  
     txtCD.readOnly=true; 
      }else
      {
     txtCD.readOnly=false;
    }
     
    txtCnvtRate = document.getElementById(txtCnvtRate);
    txtCnvtAmt = document.getElementById(txtCnvtAmt);  
    var totamt= txtDBAmt.value * txtCnvtRate.value ;
    txtCnvtAmt.value =Math.round(totamt*nodecround)/nodecround;

    
    
    TotalAccgrd1()
 }
//---------------------------------------------------
  function  convertInRateChange(txtDAmt,txtCAmt,txtCnvtRate,txtBDAmt,txtBCAmt)
 {
  
    var  txtdec=document.getElementById("<%=txtdecimal.ClientID%>");
    nodecround=Math.pow(10,parseInt(txtdec.value));
    
  
    
    txtDAmt = document.getElementById(txtDAmt);
    txtCAmt = document.getElementById(txtCAmt);
    txtCnvtRate = document.getElementById(txtCnvtRate);
    txtBDAmt = document.getElementById(txtBDAmt);  
    txtBCAmt = document.getElementById(txtBCAmt);  
  
    if (txtDAmt.value > 0){
    var tot1= txtDAmt.value * txtCnvtRate.value ;
    txtBDAmt.value =Math.round(tot1*nodecround)/nodecround;
    
    } 
    if (txtCAmt.value > 0){
    var tot2=txtCAmt.value * txtCnvtRate.value
       txtBCAmt.value = Math.round(tot2*nodecround)/nodecround ;
    }
    TotalAccgrd1()
 }
//---------------------------------------------------


	
	
				function checkNumber(evt) {
    evt = (evt) ? evt : event;
    var charCode = (evt.charCode) ? evt.charCode : ((evt.keyCode) ? evt.keyCode : 
        ((evt.which) ? evt.which : 0));
    //if (charCode != 47 && (charCode > 45 && charCode < 58)) {    
    if (charCode != 47 && (charCode > 44 && charCode < 58)) {    
        return true;
            }
       return false;
}
	function checkNumber1(evt) {
    evt = (evt) ? evt : event;
    var charCode = (evt.charCode) ? evt.charCode : ((evt.keyCode) ? evt.keyCode : 
        ((evt.which) ? evt.which : 0));
    if ((charCode > 47 && charCode < 58)) {    
    //if (charCode != 47 && (charCode > 44 && charCode < 58)) {    
        return true;
            }
       return false;
}

function checkNumber2(evt) {
    evt = (evt) ? evt : event;
    var charCode = (evt.charCode) ? evt.charCode : ((evt.keyCode) ? evt.keyCode : 
        ((evt.which) ? evt.which : 0));
    if ((charCode > 46 && charCode < 58)) {    
       
        return true;
            }
       return false;
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

    <br />

    <asp:UpdatePanel id="UpdatePanel1" runat="server">
        <contenttemplate>
          <script type="text/javascript">
              var prm = Sys.WebForms.PageRequestManager.getInstance();
              prm.add_beginRequest(function () {

              });

              prm.add_endRequest(function () {
                

              });




                </script>
<TABLE class="td_cell"><TBODY><TR><TD class="field_heading" align=center colSpan=8><asp:Label id="lblHeading" runat="server" Text="Add Account Code" CssClass="field_heading" Width="766px"></asp:Label> </TD></TR><TR><TD clas="td_cell">Doucument No</TD><TD><INPUT id="txtDocNo" tabIndex=1 readOnly type=text maxLength=100 runat="server" /></TD><TD style="WIDTH: 207px"></TD><TD align=center><asp:Label id="lblPostmsg" runat="server" Text="UnPosted" Font-Size="12px" Font-Names="Verdana" ForeColor="Green" Font-Bold="True" BackColor="#E0E0E0" CssClass="field_caption" Width="73px"></asp:Label></TD><TD></TD><TD style="WIDTH: 257px"></TD><TD></TD><TD></TD></TR><TR><TD clas="td_cell">Posting Date</TD><TD><INPUT id="txtPostDate" tabIndex=2 readOnly type=text maxLength=100 runat="server" /></TD><TD style="WIDTH: 207px"></TD><TD></TD><TD></TD><TD style="WIDTH: 257px"></TD><TD></TD><TD></TD></TR><TR><TD clas="td_cell">Doc Description</TD><TD colSpan=7><INPUT style="WIDTH: 235px" id="txtDocDesc" tabIndex=3 type=text maxLength=500 runat="server" /></TD></TR><TR><TD clas="td_cell">Account Type</TD><TD><INPUT id="txtAccType" tabIndex=4 readOnly type=text maxLength=5 runat="server" /></TD><TD style="WIDTH: 207px">Account Name</TD><TD style="WIDTH: 168px"><INPUT style="WIDTH: 200px" id="txtAccName" tabIndex=5 readOnly type=text maxLength=100 runat="server" /></TD><TD>
    <asp:Button ID="btnFillGrid" runat="server" CssClass="btn" TabIndex="6" 
        Text="Fill Grid" />
    </TD><TD style="WIDTH: 257px"><asp:Label id="Label3" runat="server" Text="Order By" CssClass="field_caption" Width="156px"></asp:Label></TD><TD><asp:DropDownList id="ddlOrderBy" tabIndex=7 runat="server" CssClass="field_input" Width="104px" AutoPostBack="True" OnSelectedIndexChanged="ddlOrderBy_SelectedIndexChanged"><asp:ListItem>[Select]</asp:ListItem>
<asp:ListItem Value="code asc">A/C Code</asp:ListItem>
<asp:ListItem Value="acctname asc">A/C Name</asp:ListItem>
</asp:DropDownList></TD><TD></TD></TR><TR><TD colSpan=8><DIV style="HEIGHT: 400px" class="container"><asp:GridView id="grdAcc1" tabIndex=8 runat="server" Font-Size="10px" BackColor="White" CssClass="td_cell" Width="1116px" GridLines="Vertical" CellPadding="3" BorderWidth="1px" BorderStyle="None" BorderColor="#999999" AutoGenerateColumns="False">
<FooterStyle BackColor="#6B6B9A" ForeColor="Black"></FooterStyle>
<Columns>
<asp:TemplateField Visible="False" HeaderText="LineNo"><EditItemTemplate>
<asp:TextBox runat="server" Text='<%# Bind("tran_lineno") %>' id="TextBox8"></asp:TextBox>
</EditItemTemplate>
<ItemTemplate>
<asp:Label runat="server" Text='<%# Bind("tran_lineno") %>' id="lblLineNo"></asp:Label>
</ItemTemplate>
</asp:TemplateField>
<asp:BoundField DataField="code" HeaderText="A/C Code" >
<HeaderStyle Width="15%"></HeaderStyle>
</asp:BoundField>
<asp:BoundField DataField="acctname" HeaderText="A/C Name">
<HeaderStyle Width="35%"></HeaderStyle>
</asp:BoundField>
<asp:BoundField DataField="costcenter_code" HeaderText="Cost Center Code" visible="false">
<HeaderStyle Width="15%"></HeaderStyle>
</asp:BoundField>
<asp:BoundField DataField="costcenter_name" HeaderText="Cost Center Name" visible="false">
<HeaderStyle Width="30%"></HeaderStyle>
</asp:BoundField>
<asp:TemplateField HeaderText="Narration"><EditItemTemplate>
<asp:TextBox id="TextBox1" runat="server" Text=" " Width="95px"></asp:TextBox>
</EditItemTemplate>

<HeaderStyle Width="10%"></HeaderStyle>
<ItemTemplate>
<INPUT style="WIDTH: 93px" id="txtnarration1" type=text maxLength=200 value='<%# Bind("Narration") %>' runat="server" />
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="Debit"><EditItemTemplate>
<asp:TextBox id="TextBox2" runat="server" Text='<%# Bind("Debit") %>' Width="99px"></asp:TextBox> 
</EditItemTemplate>

<HeaderStyle Width="8%"></HeaderStyle>
<ItemTemplate>
<INPUT style="WIDTH: 99px; TEXT-ALIGN: right" id="txtDebit1" type=text maxLength=15 value='<%# Bind("Debit") %>' runat="server" />
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="Credit"><EditItemTemplate>
                                <asp:TextBox ID="TextBox3" runat="server" Text='<%# Bind("Credit") %>'></asp:TextBox>
                            
</EditItemTemplate>

<HeaderStyle Width="8%"></HeaderStyle>
<ItemTemplate>
                                <input id="txtCredit1" maxlength="15" value='<%# Bind("Credit") %>' runat="server"  style="width: 99px; TEXT-ALIGN: right" type="text" />
                            
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="Currency"><EditItemTemplate>
                                <asp:TextBox ID="TextBox4"  runat="server" Text='<%# Bind("Currency") %>'></asp:TextBox>
                            
</EditItemTemplate>

<HeaderStyle Width="8%"></HeaderStyle>
<ItemTemplate>
                                <input id="txtCurrency1" maxlength="15"  runat="server" value='<%# Bind("Currency") %>'  readonly="readonly" style="width: 64px" type="text" />
                            
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="Conv.rate"><EditItemTemplate>
                                <asp:TextBox ID="TextBox5"  runat="server"></asp:TextBox>
                            
</EditItemTemplate>

<HeaderStyle Width="5%"></HeaderStyle>
<ItemTemplate>
                                <input id="txtconvrate1" maxlength="15"  value='<%# Bind("Currency_rate") %>' runat="server"  style="width: 41px" type="text" />
                            
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="Base Debit"><EditItemTemplate>
<asp:TextBox id="TextBox6" runat="server" Text='<%# Bind("BaseDebit") %>'></asp:TextBox> 
</EditItemTemplate>

<HeaderStyle Width="6%"></HeaderStyle>
<ItemTemplate>
                                <input id="txtBaseDebit1" maxlength="20" value='<%# Bind("BaseDebit") %>' runat="server"  readonly="readonly" style="width: 96px; TEXT-ALIGN: right"
                                    type="text" />
                            
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="Base Credit"><EditItemTemplate>
<asp:TextBox id="TextBox7" runat="server" Text='<%# Bind("basecredit") %>'></asp:TextBox> 
</EditItemTemplate>

<HeaderStyle Width="6%"></HeaderStyle>
<ItemTemplate>
<input id="txtbaseCredit1" maxlength="15" value='<%# Bind("basecredit") %>'  runat="server"  readonly="readonly" style="width: 94px; TEXT-ALIGN: right"
type="text" />
</ItemTemplate>
</asp:TemplateField>
</Columns>

<RowStyle  CssClass="grdRowstyle"></RowStyle>

<SelectedRowStyle BackColor="#008A8C" ForeColor="White" Font-Bold="True"></SelectedRowStyle>

<PagerStyle CssClass="grdpagerstyle" ForeColor="Black" HorizontalAlign="Center"></PagerStyle>

<HeaderStyle CssClass="grdheader"  ForeColor="white" Font-Bold="True"></HeaderStyle>

<AlternatingRowStyle CssClass="grdAternaterow" Font-Size="10px"></AlternatingRowStyle>
</asp:GridView></DIV></TD></TR>


<TR>
<TD >
<INPUT style="VISIBILITY: hidden; WIDTH: 7px; HEIGHT: 4px" id="txtgridrows2" type=text runat="server" /> </TD>
<TD style="WIDTH: 97px"><INPUT style="VISIBILITY: hidden; WIDTH: 23px; HEIGHT: 5px" id="txtgridrows1" type=text runat="server" /></TD>
<TD style="VISIBILITY: hidden; WIDTH: 207px"><INPUT style="VISIBILITY: hidden; WIDTH: 11px; HEIGHT: 1px" id="txtdecimal" type=text runat="server" /></TD>
<TD></TD>
<TD>Total Base Amount</TD>
    <TD align=left ><INPUT style="WIDTH: 117px; TEXT-ALIGN: right" id="txtTotDbBace1" readOnly type=text maxLength=100 runat="server" />
    &nbsp;
    <INPUT style="WIDTH: 115px; TEXT-ALIGN: right" id="txtTotCrBace1" readOnly type=text maxLength=100 runat="server" />
    </TD>
    <TD align=left></TD>
<TD>
    <asp:TextBox ID="txtpdate" runat="server" Visible="False" Width="1px"></asp:TextBox></TD>
    
    
    </TR>
    
    <TR><TD colSpan=8>
    <%--<DIV style="HEIGHT: 200px">--%>
    <asp:Panel ID="pnlgrd" runat="server" height ="200px" ScrollBars ="Auto" BorderColor ="Black"  >
               <br />
    <asp:GridView id="grdAcc2" tabIndex=9 runat="server" Font-Size="10px" Height="170px" BackColor="White" CssClass="td_cell" Width="1116px" GridLines="Vertical" CellPadding="3" BorderWidth="1px" BorderStyle="None" BorderColor="#999999" AutoGenerateColumns="False">
<FooterStyle CssClass="grdfooter" ForeColor="Black"></FooterStyle>
<Columns>
<asp:BoundField DataField="tran_lineno" Visible="False" HeaderText="LineNo"></asp:BoundField>
<asp:TemplateField HeaderText="A/C Code"><EditItemTemplate>
<asp:TextBox id="TextBox1" runat="server" Text='<%# Bind("code") %>'></asp:TextBox> 
</EditItemTemplate>
<ItemTemplate>
<SELECT style="WIDTH: 100px" id="ddlAccCode" runat="server"> <OPTION selected></OPTION></SELECT> 
</ItemTemplate>

<FooterStyle Width="15%"></FooterStyle>
</asp:TemplateField>
<asp:TemplateField HeaderText="A/C Name">
<ItemTemplate>
<input type="text" name="accSearch"  class="field_input MyAutoCompleteClass" style="width:98% ; font " id="accSearch"  runat="server" />
   
<SELECT style="WIDTH: 125px" id="ddlAccName" class="field_input MyDropDownListCustValue" runat="server"> <OPTION selected></OPTION></SELECT> 
</ItemTemplate>

<FooterStyle Width="25%"></FooterStyle>
</asp:TemplateField>
<asp:TemplateField HeaderText="Cost Center Code" visible="false"><EditItemTemplate>
<asp:TextBox id="TextBox3" runat="server" Text='<%# Bind("costcenter_code") %>' Width="54px"></asp:TextBox> 
</EditItemTemplate>
<ItemTemplate>
<SELECT style="WIDTH: 100px" id="ddlContCntCode" runat="server"> <OPTION selected></OPTION></SELECT> 
</ItemTemplate>

<FooterStyle Width="15%"></FooterStyle>
</asp:TemplateField>
<asp:TemplateField HeaderText="Cost Center Name" visible="false"><EditItemTemplate>
<asp:TextBox id="TextBox4" runat="server" Text='<%# Bind("costcenter_name") %>'></asp:TextBox> 
</EditItemTemplate>
<ItemTemplate>
                                <select id="ddlCostCntName" style="width: 115px" runat="server">
                                    <option selected="selected"></option>
                                </select>
                            
</ItemTemplate>

<FooterStyle Width="20%"></FooterStyle>
</asp:TemplateField>
<asp:TemplateField HeaderText="Narration"><EditItemTemplate>
<asp:TextBox id="TextBox5" runat="server" Text='<%# Bind("Narration") %>' Width="52px"></asp:TextBox> 
</EditItemTemplate>
<ItemTemplate>
<INPUT style="WIDTH: 75px" id="txtNarration" type=text maxLength=200 runat="server" /> 
</ItemTemplate>

<FooterStyle Width="8%"></FooterStyle>
</asp:TemplateField>
<asp:TemplateField HeaderText="Debit"><EditItemTemplate>
<asp:TextBox id="TextBox6" runat="server" Text='<%# Bind("Debit") %>'></asp:TextBox> 
</EditItemTemplate>
<ItemTemplate>
<INPUT style="WIDTH: 98px; TEXT-ALIGN: right" id="txtDebit" type=text maxLength=15 runat="server" /> 
</ItemTemplate>

<FooterStyle Width="8%"></FooterStyle>
</asp:TemplateField>
<asp:TemplateField HeaderText="Credit"><EditItemTemplate>
<asp:TextBox id="TextBox7" runat="server" Text='<%# Bind("Credit") %>'></asp:TextBox> 
</EditItemTemplate>
<ItemTemplate>
<INPUT style="WIDTH: 98px; TEXT-ALIGN: right" id="txtCredit" type=text maxLength=15 runat="server" /> 
</ItemTemplate>

<FooterStyle Width="8%"></FooterStyle>
</asp:TemplateField>
<asp:TemplateField HeaderText="Currency"><EditItemTemplate>
<asp:TextBox id="TextBox8" runat="server" Text='<%# Bind("Currency") %>'></asp:TextBox> 
</EditItemTemplate>
<ItemTemplate>
<INPUT style="WIDTH: 75px" id="txtCurrency" readOnly type=text maxLength=15 runat="server" /> 
</ItemTemplate>

<FooterStyle Width="8%"></FooterStyle>
</asp:TemplateField>
<asp:TemplateField HeaderText="Conv.rate"><EditItemTemplate>
<asp:TextBox id="TextBox9" runat="server"></asp:TextBox> 
</EditItemTemplate>
<ItemTemplate>
                                <input id="txtConvRate" style="width: 43px" type="text" runat="server" />
                            
</ItemTemplate>

<FooterStyle Width="8%"></FooterStyle>
</asp:TemplateField>
<asp:TemplateField HeaderText="Base Debit"><EditItemTemplate>
                                <asp:TextBox ID="TextBox10" runat="server" Text='<%# Bind("BaseDebit") %>'></asp:TextBox>
                            
</EditItemTemplate>
<ItemTemplate>
                                <input id="txtBaseDebit" style="width: 85px; TEXT-ALIGN: right" type="text" maxlength="15" readonly="readOnly" runat="server" />
                            
</ItemTemplate>

<FooterStyle Width="8%"></FooterStyle>
</asp:TemplateField>
<asp:TemplateField HeaderText="Base Credit"><EditItemTemplate>
                                <asp:TextBox ID="TextBox11" runat="server" Text='<%# Bind("basecredit") %>'></asp:TextBox>
                            
</EditItemTemplate>
<ItemTemplate>
                                <input id="txtBaseCredit" style="width: 85px; TEXT-ALIGN: right" type="text" maxlength="20" readonly="readOnly" runat="server" />
                            
</ItemTemplate>

<FooterStyle Width="8%"></FooterStyle>
</asp:TemplateField>
</Columns>

<RowStyle CssClass="grdRowstyle" ></RowStyle>

<SelectedRowStyle BackColor="#008A8C" ForeColor="White" Font-Bold="True"></SelectedRowStyle>

<PagerStyle CssClass="grdpagerstyle" ForeColor="Black" HorizontalAlign="Center"></PagerStyle>

<HeaderStyle CssClass="grdheader" ForeColor="white" Font-Bold="True"></HeaderStyle>

<AlternatingRowStyle CssClass="grdAternaterow" Font-Size="10px"></AlternatingRowStyle>
</asp:GridView>
</asp:Panel>

<%--</DIV>--%>
</TD></TR>
<TR>
<TD><SELECT style="VISIBILITY: hidden; WIDTH: 26px" id="ddlcurr" runat="server">
 <OPTION selected></OPTION></SELECT> 
    <input id="txtconnection" runat="server" style="visibility: hidden; width: 12px;
        height: 9px" type="text" /></TD>
        <TD></TD><TD style="WIDTH: 207px; HEIGHT: 16px"></TD>
        <TD style="WIDTH:168px; HEIGHT: 16px"></TD>
        <TD style="WIDTH: 142px; HEIGHT: 16px"></TD>
        <TD class="td_cell">Total Base Amount</TD>
        <TD><INPUT style="WIDTH: 117px; TEXT-ALIGN: right" id="txtTotDbBace2" readOnly type=text maxLength=100 runat="server" /></TD>
        <TD><INPUT style="WIDTH: 117px; TEXT-ALIGN: right" id="txtTotCrBace2" readOnly type=text maxLength=100 runat="server" /></TD></TR>
        <TR><TD style="WIDTH: 111px; HEIGHT: 31px"></TD><TD style="HEIGHT: 31px"></TD><TD style="WIDTH: 207px; HEIGHT: 31px"></TD>
        <TD style="WIDTH: 168px; HEIGHT: 31px"></TD><TD style="HEIGHT: 31px" align=right>
        <INPUT style="WIDTH: 27px" id="txtDr" readOnly type=text value="Dr" runat="server" /></TD>
        <TD style="WIDTH: 257px; HEIGHT: 31px" clas="td_cell">Total Base Amount</TD><TD style="HEIGHT: 31px">
        <INPUT style="WIDTH: 117px; TEXT-ALIGN: right" id="txtTotDbBace" readOnly type=text maxLength=100 runat="server" />
        </TD>
        <TD style="HEIGHT: 31px"><INPUT style="WIDTH: 117px; TEXT-ALIGN: right" id="txtTotCrBace" readOnly type=text maxLength=100 runat="server" /></TD></TR><TR><TD style="WIDTH: 111px; HEIGHT: 28px"><asp:Label id="Label1" runat="server" Text="Total Net Amount" Height="12px" CssClass="field_caption" Width="88px"></asp:Label></TD><TD style="HEIGHT: 28px"><INPUT style="TEXT-ALIGN: right" id="txtTotalNetAmt" readOnly type=text maxLength=100 runat="server" /></TD><TD style="WIDTH: 207px; HEIGHT: 28px" clas="td_cell">Total Party Bal.</TD><TD style="WIDTH: 168px; HEIGHT: 28px"><INPUT style="TEXT-ALIGN: right" id="txtTotalPartyBal" readOnly type=text maxLength=100 runat="server" /></TD><TD style="HEIGHT: 28px" clas="td_cell">Difference</TD><TD style="WIDTH: 257px; HEIGHT: 28px"><INPUT style="TEXT-ALIGN: right" id="txtDiffAmt" readOnly type=text maxLength=100 runat="server" /></TD><TD style="HEIGHT: 28px" align=right colSpan=2>&nbsp;&nbsp;</TD></TR><TR><TD style="WIDTH: 111px; HEIGHT: 21px"></TD><TD style="HEIGHT: 21px"></TD><TD style="WIDTH: 207px; HEIGHT: 21px" clas="td_cell"></TD><TD style="WIDTH: 168px; HEIGHT: 21px"></TD><TD style="HEIGHT: 21px" clas="td_cell"></TD><TD align=right colSpan=3><asp:CheckBox id="chkPost" tabIndex=24 runat="server" Text="Post/UnPost" Font-Size="10px" Font-Names="Verdana" ForeColor="Black" Font-Bold="True" BackColor="#FFC0C0" CssClass="field_caption" Width="103px"></asp:CheckBox> 
    <asp:Button id="btnSave" onclick="btnSave_Click" tabIndex=10 runat="server" 
        Text="Save" Font-Bold="True" CssClass="field_button"></asp:Button>&nbsp; 
    <asp:Button id="btnExit"  onclick="btnExit_Click" tabIndex=11 runat="server" 
        Text="Exit" Font-Bold="True" CssClass="field_button"></asp:Button>&nbsp;
         <asp:Button id="btnhelp" tabIndex=12 onclick="btnhelp_Click" runat="server" Text="Help" CssClass="field_button" Width="39px"></asp:Button></TD></TR></TBODY></TABLE>
</contenttemplate>
    </asp:UpdatePanel>
    <asp:ScriptManagerProxy id="ScriptManagerProxy1" runat="server"><Services>
<asp:ServiceReference Path="~/clsServices.asmx"></asp:ServiceReference>
</Services>
</asp:ScriptManagerProxy> 

</asp:Content>
