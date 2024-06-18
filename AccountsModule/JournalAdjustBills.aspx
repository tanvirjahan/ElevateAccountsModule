<%@ Page Language="VB"  AutoEventWireup="false" CodeFile="JournalAdjustBills.aspx.vb" Inherits="JournalAdjustBills"  %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
 
<%@ Register Assembly="EclipseWebSolutions.DatePicker" Namespace="EclipseWebSolutions.DatePicker"    TagPrefix="ews" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title> </title>
   <link rel="stylesheet" href="CSS/Styles.css" type="text/css" />
     <script language="javascript" src="js/date-picker.js" type="text/javascript"></script>
	 <script language="javascript" src="js/datefun.js" type="text/javascript"></script>
	 <script language="javascript" src="js/FillDropDown.js" type="text/javascript"></script>
	  </head>
<script language="javascript" type="text/javascript">

  //--------------------------------------------------------------------------------
  //--------------------------------------------------------------------------------
function OnClickOk()
{
         var txtBal = document.getElementById('<%=txtBalAdu.ClientID%>');
         var valAd=txtBal.value;
         if (valAd==''){valAd=0;}
         if (isNaN(valAd)==true){valAd=0;}
       
         if (valAd==0)
         {
             window.close();
         }else{
         var txtfl = document.getElementById('<%=txtflag.ClientID%>');
         if(confirm('Do you want to generate auto advance entry for Balance to Adjust : ' +  valAd )==false)
             {
                txtfl.value=0;
             }
             else
             {
                txtfl.value=1;
             }
         }
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
//-----------------------------------------

 function  OnChangeAdPay(txtCRAmt,txtDBAmt,txtBaseAmt,txtRate)
 {
    txtCRAmt = document.getElementById(txtCRAmt);
    txtDBAmt = document.getElementById(txtDBAmt);
    txtBaseAmt = document.getElementById(txtBaseAmt);  
    txtRate = document.getElementById(txtRate);  
    
    if(txtDBAmt.value>0)
    {
       txtCRAmt.readOnly=true; 
    }else
    {
     txtCRAmt.readOnly=false;
    }
   
    
    if (txtDBAmt.value==''){txtDBAmt.value=0;}
    if (txtCRAmt.value==''){txtCRAmt.value=0;}
     
    var cAmt=(parseFloat(txtDBAmt.value) + parseFloat(txtCRAmt.value)) *  parseFloat(txtRate.value) ;
    txtBaseAmt.value =cAmt.toFixed(3);
   
  
       var valTotal=0;
        var objGridView = document.getElementById('<%=grdAdjustBill.ClientID%>');
        var txtrowcnt =document.getElementById('<%=txtgrdAdjRows.ClientID%>');
        intRows=txtrowcnt.value;
  
        for(j=1;j<=intRows;j++)
        {
           if(objGridView.rows[j].cells[0].children[0].checked == true)
             { 
                var valAd=objGridView.rows[j].cells[8].children[0].value;
                if (valAd==''){valAd=0;}
                if (isNaN(valAd)==true){valAd=0;}
                valTotal=parseFloat(valTotal)+parseFloat(valAd);
             }
        }
        //Total grid
        var objGridView = document.getElementById('<%=grdAdPay.ClientID%>');
        var txtrowcnt =document.getElementById('<%=txtgrdAdpayRows.ClientID%>');
        intRows=txtrowcnt.value;
  
        for(j=1;j<=intRows;j++)
        {
                var valAd=objGridView.rows[j].cells[7].children[0].value;
                if (valAd==''){valAd=0;}
                if (isNaN(valAd)==true){valAd=0;}
                valTotal=parseFloat(valTotal)+parseFloat(valAd);
             
        }
        var txtrAmount =document.getElementById('<%=txtADAmount.ClientID%>');
        var txtrAmtAdj =document.getElementById('<%=txtAmountAdjust.ClientID%>');
        var txtBalanceAdj =document.getElementById('<%=txtBalAdu.ClientID%>');

        if (parseFloat(txtrAmtAdj.value) - parseFloat(valTotal)  >= 0)
        {
            txtBalanceAdj.value=parseFloat(txtrAmtAdj.value) - parseFloat(valTotal)
            
        }else
        {
         alert("You have only " +  txtBalanceAdj.value  + " Amount to Adjust");
         txtCRAmt.value=0;
         txtDBAmt.value=0;
         txtBaseAmt.value=0;
        }
        
 }

//----------------------------------------------------------------------------
  function OnchangeChk(txtAdj,chk)
  {
   chk = document.getElementById(chk);
   txtAdj = document.getElementById(txtAdj);
 
   if(chk.checked == true)
   {
      txtAdj.readOnly=false;
      
    }else
    {
     txtAdj.value='';
     txtAdj.readOnly=true;
     }
     
  AdjestBillChange(txtAdj)
  }
  
//----------------------------------------------------------------------------
 function AdjestBillChange(txtAdj,ValBal)
   {
     
   
        txtAdj = document.getElementById(txtAdj);
        if (txtAdj.value==''){txtAdj.value=0;}
        if (isNaN(txtAdj.value)==true){txtAdj.value=0;}
        if(txtAdj.value > ValBal)
        {
            alert("You have only " +  ValBal  + " Amount to Adjust");
            txtAdj.value='';
            return false;
        }
        
        if(txtAdj.value!=0)
        {
        
        var valTotal=0;
        var objGridView = document.getElementById('<%=grdAdjustBill.ClientID%>');
        var txtrowcnt =document.getElementById('<%=txtgrdAdjRows.ClientID%>');
        intRows=txtrowcnt.value;
  
        for(j=1;j<=intRows;j++)
        {
           if(objGridView.rows[j].cells[0].children[0].checked == true)
             { 
                var valAd=objGridView.rows[j].cells[8].children[0].value;
                if (valAd==''){valAd=0;}
                if (isNaN(valAd)==true){valAd=0;}
                valTotal=parseFloat(valTotal)+parseFloat(valAd);
             }
        }
        var objGridView = document.getElementById('<%=grdAdPay.ClientID%>');
        var txtrowcnt =document.getElementById('<%=txtgrdAdpayRows.ClientID%>');
        intRows=txtrowcnt.value;
  
        for(j=1;j<=intRows;j++)
        {
                var valAd=objGridView.rows[j].cells[7].children[0].value;
                if (valAd==''){valAd=0;}
                if (isNaN(valAd)==true){valAd=0;}
                valTotal=parseFloat(valTotal)+parseFloat(valAd);
             
        }
        
        
        var txtrAmount =document.getElementById('<%=txtADAmount.ClientID%>');
        var txtrAmtAdj =document.getElementById('<%=txtAmountAdjust.ClientID%>');
        var txtBalanceAdj =document.getElementById('<%=txtBalAdu.ClientID%>');


        if (parseFloat(txtrAmtAdj.value) - parseFloat(valTotal)  >= 0)
        {
            txtBalanceAdj.value=parseFloat(txtrAmtAdj.value) - parseFloat(valTotal)
            
        }else
        {
         alert("You have only " +  txtBalanceAdj.value  + " Amount to Adjust");
            txtAdj.value='';
        }
        }else
        {
         alert("Enter The Adjustment Amount");
         txtAdj.focus();  
        }
  }
  //--------------------------------------------------------------------------------
  //--------------------------------------------------------------------------------
</script>	

<body>
    <form id="form1" runat="server">
    <div style="height: 512px">
        <asp:ScriptManager id="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
        <table class="td_cell" style="border-left-color: dimgray; border-bottom-color: dimgray; border-top-style: solid; border-top-color: dimgray; border-right-style: solid; border-left-style: solid; border-right-color: dimgray; border-bottom-style: solid">
            <tr>
                <td align="center"   colspan="6">
                    <asp:Label ID="Label2" runat="server" CssClass="field_heading" Text="Adjust Bills"
                        Width="100%"></asp:Label></td>
            </tr>
            <tr>
                <td style="width: 176px">
                    Account Type</td>
                <td style="width: 216px">
                    <input id="txtAdAccountType" runat="server" class="field_input" readonly="readonly"
                        type="text" /></td>
                <td style="width: 150px">
                </td>
                <td style="width: 100px">
                </td>
                <td style="width: 100px">
                </td>
                <td style="width: 100px">
                </td>
            </tr>
            <tr>
                <td style="width: 176px">
                    Account Code</td>
                <td style="width: 216px">
                    <input id="txtAccCode" runat="server" class="field_input" readonly="readonly" type="text" /></td>
                <td style="width: 150px">
                    Account Name</td>
                <td>
                    <input id="txtAccName" runat="server" class="field_input" readonly="readonly" style="width: 239px"
                        type="text" /></td>
                <td style="width: 100px">
                </td>
                <td style="width: 100px">
                </td>
            </tr>
            <tr>
                <td style="width: 176px">
                    SalesMan Code</td>
                <td style="width: 216px">
                    <input id="txtSalmanCode" runat="server" class="field_input" readonly="readonly"
                        type="text" /></td>
                <td style="width: 150px">
                    SalesMan Name</td>
                <td style="width: 100px">
                    <input id="txtSalmanName" runat="server" class="field_input" readonly="readonly"
                        style="width: 239px" type="text" /></td>
                <td style="width: 100px">
                </td>
                <td style="width: 100px">
                </td>
            </tr>
            <tr>
                <td style="width: 176px">
                    Currency Code</td>
                <td style="width: 216px">
                    <input id="txtCurrencyCode" runat="server" class="field_input" readonly="readonly"
                        type="text" /></td>
                <td style="width: 150px">
                    Exachange Rate</td>
                <td style="width: 100px">
                    <input id="txtExchangeRate" runat="server" class="field_input" readonly="readonly"
                        style="width: 76px" type="text" /></td>
                <td style="width: 100px">
                    Amount</td>
                <td style="width: 100px">
                    <input id="txtADAmount" runat="server" class="field_input" readonly="readonly" type="text" /></td>
            </tr>
            <tr>
                <td colspan="6" style="height:410px">
                     <cc1:TabContainer ID="adjustBillTab" runat="server" ActiveTabIndex="1" 
                         Height="200px" EnableTheming="False" Width="950px" AutoPostBack="True">
                        <cc1:TabPanel ID="TabPanel1" runat="server" HeaderText="TabPanel1">
                            <ContentTemplate>
                                <asp:Panel ID="Panel1" runat="server" Height="185px" ScrollBars="Vertical" 
                                    Width="950px" GroupingText=" ">
                                <asp:GridView ID="grdAdjustBill" runat="server" AutoGenerateColumns="False" BackColor="White"
                                    BorderColor="#999999" BorderStyle="None" BorderWidth="1px" CellPadding="3" CssClass="grdstyle"
                                    Font-Size="10px" GridLines="Vertical" TabIndex="21" Width="950px">
                                    <HeaderStyle BackColor="#06788B" Font-Bold="True" ForeColor="White" />
                                    <FooterStyle BackColor="White" ForeColor="Black" />
                                    <SelectedRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" />
                                    <AlternatingRowStyle BackColor="#DDD9CF" Font-Size="10px" />
                                    <Columns>
                                        <asp:BoundField DataField="acc_tran_lineno" HeaderText="LineNo" Visible="False" />
                                        <asp:TemplateField>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <input id="chkBill" runat="server" type="checkbox" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="DocNo" HeaderText="Doc No" />
                                     <%--   <asp:BoundField DataField="DocNo1" HeaderText="Doc No" />--%>
                                        <asp:BoundField DataField="Type" HeaderText="Doc Type" />
                                        <asp:BoundField DataField="field1" HeaderText="Reference no" />
                                        <asp:BoundField HtmlEncode="False" DataFormatString="{0:dd/MM/yyyy}"  DataField="Date" HeaderText="Date" />
                                        <asp:BoundField HtmlEncode="False" DataFormatString="{0:dd/MM/yyyy}"  DataField="DueDate" HeaderText="Due Date" />
                                        <asp:BoundField DataField="currrate" HeaderText="Exchg Rate" />
                                        <asp:BoundField DataField="amount" HeaderText="Balance Amount" />
                                        <asp:TemplateField HeaderText="Amount To Adjust">
                                            <EditItemTemplate>
                                                <asp:TextBox ID="TextBox2" runat="server"></asp:TextBox>
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <input id="txtAdujustAmt" value='<%# Bind("Adjustedamount") %>' runat="server" class="field_input" maxlength="12"
                                                    style="width: 86px; text-align: right;" type="text" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="field2" HeaderText="Field 2" />
                                        <asp:BoundField DataField="field3" HeaderText="Field 3" />
                                        <asp:BoundField DataField="field4" HeaderText="Field 4" />
                                        <asp:BoundField DataField="field5" HeaderText="Field 5" />
                                    </Columns>
                                    <RowStyle BackColor="White" ForeColor="Black" />
                                    <PagerStyle BackColor="#F7FBFF" ForeColor="Black" HorizontalAlign="Center" />
                                </asp:GridView>
                                </asp:Panel>
                            </ContentTemplate>
                            <HeaderTemplate>
                                Adjust Bills
                            </HeaderTemplate>
                        </cc1:TabPanel>
                        <cc1:TabPanel ID="TabPanel2" runat="server" HeaderText="TabPanel2">
                            <ContentTemplate>
                                <asp:Panel ID="Panel2" runat="server" Height="225px" ScrollBars="Both" Width="950px" GroupingText=" ">
                                <asp:GridView ID="grdAdPay" runat="server" AutoGenerateColumns="False" BackColor="White"
                                    BorderColor="#999999" BorderStyle="None" BorderWidth="1px" CellPadding="3" CssClass="td_cell"
                                    Font-Size="10px" GridLines="Vertical" TabIndex="21" Width="984px">
                                    <HeaderStyle BackColor="#06788B" Font-Bold="True" ForeColor="White" />
                                    <FooterStyle BackColor="White" ForeColor="#06788B" />
                                    <SelectedRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" />
                                    <AlternatingRowStyle BackColor="#DDD9CF" Font-Size="10px" />
                                    <Columns>
                                        <asp:BoundField DataField="LineNo" HeaderText="LineNo" Visible="False" />
                                        <asp:TemplateField HeaderText="Doc No">
                                            <EditItemTemplate>
                                                <asp:TextBox ID="TextBox3" runat="server" Text='<%# Bind("DocNo") %>'></asp:TextBox>
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <input id="txtADocNo" class="field_input" maxlength="20" style="width: 70px" type="text" runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Date">
                                            <EditItemTemplate>
                                                <asp:TextBox ID="TextBox6" runat="server" Text='<%# Bind("Date") %>'></asp:TextBox>
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <ews:DatePicker ID="dtDate" runat="server" CalendarPosition="DisplayRight" DateFormatString="dd/MM/yyyy"
                                                    RegExErrorMessage="Please enter a date in the format: dd/mm/yyyy" Width="174px" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Doc Type">
                                            <EditItemTemplate>
                                                <asp:TextBox ID="TextBox4" runat="server" Text='<%# Bind("Type") %>'></asp:TextBox>
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <input id="txtDocType" class="field_input" maxlength="10" style="width: 35px" type="text" runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Reference no">
                                            <EditItemTemplate>
                                                <asp:TextBox ID="TextBox5" runat="server" Text='<%# Bind("field1") %>'></asp:TextBox>
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <input id="txtRefNo" class="field_input" maxlength="100" style="width: 75px" type="text" runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Due Date">
                                            <EditItemTemplate>
                                                <asp:TextBox ID="TextBox13" runat="server"></asp:TextBox>
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <ews:DatePicker ID="dtDueDate" runat="server" CalendarPosition="DisplayRight"
                                                    DateFormatString="dd/MM/yyyy" RegExErrorMessage="Please enter a date in the format: dd/mm/yyyy"
                                                    Width="174px" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Debit">
                                            <EditItemTemplate>
                                                <asp:TextBox ID="TextBox7" runat="server"></asp:TextBox>
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <input id="txtADebit" class="field_input" maxlength="10" style="width: 75px; text-align: right;" type="text" runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Credit">
                                            <EditItemTemplate>
                                                <asp:TextBox ID="TextBox2" runat="server"></asp:TextBox>
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <input id="txtACredit" class="field_input" maxlength="10" style="width: 75px; text-align: right;" type="text" runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Base Amount">
                                            <EditItemTemplate>
                                                <asp:TextBox ID="TextBox8" runat="server"></asp:TextBox>
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <input id="txtBaseAmount" class="field_input" maxlength="10" style="text-align: right; width: 85px;"
                                                    type="text" readonly="readOnly" runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Field 2">
                                            <EditItemTemplate>
                                                <asp:TextBox ID="TextBox9" runat="server" Text='<%# Bind("field2") %>'></asp:TextBox>
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <input id="txtField2" class="field_input" maxlength="100" style="width: 86px" type="text" runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Field 3">
                                            <EditItemTemplate>
                                                <asp:TextBox ID="TextBox10" runat="server" Text='<%# Bind("field3") %>'></asp:TextBox>
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <input id="txtField3" class="field_input" maxlength="10" style="width: 86px" type="text" runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Field 4">
                                            <EditItemTemplate>
                                                <asp:TextBox ID="TextBox11" runat="server" Text='<%# Bind("field4") %>'></asp:TextBox>
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <input id="txtfield4" class="field_input" maxlength="100" style="width: 86px" type="text" runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Field 5">
                                            <EditItemTemplate>
                                                <asp:TextBox ID="TextBox12" runat="server" Text='<%# Bind("field5") %>'></asp:TextBox>
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <input id="txtField5" class="field_input" maxlength="100" style="width: 86px" type="text" runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <RowStyle BackColor="White" ForeColor="Black"/>
                                    <PagerStyle BackColor="#F7FBFF" ForeColor="Black" HorizontalAlign="Center" />
                                </asp:GridView>
                                </asp:Panel>
                            </ContentTemplate>
                            <HeaderTemplate>
                                Advanced Payment
                            </HeaderTemplate>
                        </cc1:TabPanel>
                    </cc1:TabContainer></td>
            </tr>
            <tr>
                <td style="width: 176px; height: 16px">
                    &nbsp;Amount Adjust</td>
                <td style="width: 216px; height: 16px">
                    <input id="txtAmountAdjust" runat="server" class="field_input" readonly="readonly"
                        type="text" /></td>
                <td style="width: 150px; height: 16px">
                    &nbsp;Balance To Adujust</td>
                <td style="width: 100px; height: 16px">
                    <input id="txtBalAdu" runat="server" class="field_input" readonly="readonly" type="text" /></td>
                <td style="width: 100px; height: 16px">
                </td>
                <td style="width: 100px; height: 16px">
                </td>
            </tr>
            <tr>
                <td style="width: 176px; height: 14px"><input id="txtflag" runat="server" style="visibility: hidden; width: 81px"
                        type="text" /></td>
                <td style="visibility: hidden; width: 216px">
                    <input id="txtgrdAdjRows" runat="server" style="visibility: hidden; width: 131px"
                        type="text" /></td>
                <td style="width: 150px; height: 14px">
                    <input id="txtgrdAdpayRows" runat="server" style="visibility: hidden; width: 117px"
                        type="text" /></td>
                <td>
                    <asp:Button ID="btnOk" runat="server" CssClass="field_button" Font-Bold="False" Height="24px"
                        TabIndex="5" Text="Ok" Width="96px" />
                    &nbsp;
                    <asp:Button ID="btnAExit" runat="server" CssClass="field_button" Font-Bold="False"
                        Height="24px" TabIndex="5" Text="Exit" Width="96px" /></td>
                <td style="width: 100px; height: 14px">
                    </td>
                <td style="width: 100px; height: 14px">
                    </td>
            </tr>
        </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    
    </div>
    </form>
</body>
</html>