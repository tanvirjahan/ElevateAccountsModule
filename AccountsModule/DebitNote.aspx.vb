'------------================--------------=======================------------------================
'   Module Name    :    DebitNote.aspx
'   Developer Name :    Govardhan
'   Date           :    
'   
'
'------------================--------------=======================------------------================
#Region "Namespace"
Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports System.IO.File
Imports System.Collections
Imports System.Collections.Generic
Imports System.Globalization
#End Region
Partial Class DebitNote
    Inherits System.Web.UI.Page
#Region "Global Declaration"
    Dim objUtils As New clsUtils
    Dim objUser As New clsUser
    Dim ObjDate As New clsDateTime
    Dim strSqlQry As String
    Dim strWhereCond As String
    Dim SqlConn As SqlConnection
    Dim myCommand As SqlCommand
    Dim myDataAdapter As SqlDataAdapter
    Dim mySqlReader As SqlDataReader
    Dim sqlTrans As SqlTransaction
    Dim gvRow As GridViewRow
    Dim chckDeletion As CheckBox
    'For accounts posting
    Dim caccounts As clssave = Nothing
    Dim cacc As clsAccounts = Nothing
    Dim ctran As clstran = Nothing
    Dim csubtran As clsSubTran = Nothing
    Dim mbasecurrency As String = ""
    Dim ScriptOpenModalDialog As String = "OpenModalDialog('{0}','{1}');"

    'For accounts posting
#End Region
#Region "Numbers"
    Public Sub Numbers(ByVal txtbox As TextBox)
        txtbox.Attributes.Add("onkeypress", "return checkNumber(event)")
        'txtbox.Attributes.Add("onkeypress", "return checkNumberDecimal(event,this)")
    End Sub
    Public Sub NumbersHtml(ByVal txtbox As HtmlInputText)
        txtbox.Attributes.Add("onkeypress", "return checkNumber(event)")
        'txtbox.Attributes.Add("onkeypress", "return checkNumberDecimal(event,this)")
    End Sub
#End Region
#Region "NumbersDecimalRound"
    Public Sub NumbersDecimalRound(ByVal txtbox As TextBox)
        txtbox.Attributes.Add("onkeypress", "return checkNumberDecimal(event,this)")
    End Sub
    Public Sub NumbersDecimalRoundHtml(ByVal txtbox As HtmlInputText)
        txtbox.Attributes.Add("onkeypress", "return checkNumberDecimal(event,this)")
    End Sub
#End Region
#Region "NumbersInt"
    Public Sub NumbersIntHtml(ByVal txtbox As HtmlInputText)
        Try
            txtbox.Attributes.Add("onkeypress", "return checkNumber1(event)")
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        End Try
    End Sub
    Public Sub NumbersInt(ByVal txtbox As TextBox)
        Try
            txtbox.Attributes.Add("onkeypress", "return checkNumber1(event)")
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        End Try
    End Sub
    Public Sub NumbersDateInt(ByVal txtbox As TextBox)
        Try
            txtbox.Attributes.Add("onkeypress", "return checkNumber2(event)")
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        End Try
    End Sub
#End Region
#Region "TextLock"
    Public Sub TextLock(ByVal txtbox As TextBox)
        txtbox.Attributes.Add("onKeyPress", "return chkTextLock(event)")
        txtbox.Attributes.Add("onKeyDown", "return chkTextLock1(event)")
        txtbox.Attributes.Add("onKeyUp", "return chkTextLock(event)")
    End Sub
#End Region
#Region "TextLock"
    Public Sub TextLockhtml(ByVal txtbox As HtmlInputText)
        txtbox.Attributes.Add("onKeyPress", "return chkTextLock(event)")
        txtbox.Attributes.Add("onKeyDown", "return chkTextLock1(event)")
        txtbox.Attributes.Add("onKeyUp", "return chkTextLock(event)")
    End Sub
#End Region
    Private Sub initialclass(ByVal con As SqlConnection, ByVal stran As SqlTransaction)
        caccounts = Nothing
        cacc = Nothing
        ctran = Nothing
        csubtran = Nothing
        caccounts = New clssave
        cacc = New clsAccounts
        cacc.clropencol()
        cacc.tran_mode = IIf(ViewState("DebitNoteState") = "New", 1, 2)
        mbasecurrency = CType(objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id=457"), String)
        cacc.start()

    End Sub
    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

        ViewState.Add("DebitNoteState", Request.QueryString("State"))
        ViewState.Add("DebitNoteRefCode", Request.QueryString("RefCode"))
        ViewState.Add("CNDNOpen_type", Request.QueryString("CNDNOpen_type"))
        ViewState.Add("divcode", Request.QueryString("divid"))

        If IsPostBack = False Then
            If Not Session("CompanyName") Is Nothing Then
                Me.Page.Title = CType(Session("CompanyName"), String)
            End If

            If CType(Session("GlobalUserName"), String) = "" Then
                Response.Redirect("~/Login.aspx", False)
                Exit Sub
            End If
            Try
                txtconnection.Value = Session("dbconnectionName")

                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSalesman, "UserCode", "UserName", "select * from UserMaster where active = 1 order by UserCode", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSalesmanName, "UserName", "UserCode", "select * from UserMaster where active = 1 order by UserName", True)

                strSqlQry = "select  narration,narration from narration where active=1"
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlNarration, "narration", "narration", strSqlQry, True)

                'strSqlQry = " select distinct top 10 view_account.controlacctcode, acctmast.acctname  from acctmast ,view_account where   view_account.controlacctcode= acctmast.acctcode  and type= 'C'  order by  view_account.controlacctcode"
                strSqlQry = " select acctcode controlacctcode,acctname from acctmast where div_code='" & ViewState("divcode") & "' and  isnull(controlyn,'')='Y' order by acctcode"
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlConAccCode, "controlacctcode", "acctname", strSqlQry, True)
                'strSqlQry = " select distinct top 10  acctmast.acctname,view_account.controlacctcode  from acctmast ,view_account where   view_account.controlacctcode= acctmast.acctcode  and type= 'C'  order by  acctmast.acctname"
                strSqlQry = " select acctname,acctcode controlacctcode from acctmast where div_code='" & ViewState("divcode") & "' and isnull(controlyn,'')='Y' order by acctname"
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlConAccName, "acctname", "controlacctcode", strSqlQry, True)

                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCustomer, "code", "des", "select [Select]", False)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCustomerName, "des", "code", "select [Select]", False)

                txtdecimal.Value = objUtils.GetDBFieldFromLongnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", 509)
                txtbasecurr.Value = objUtils.GetDBFieldFromLongnew(Session("dbconnectionName"), "reservation_parameters", "option_Selected", "param_id", 457)

                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSMktCode, "plgrpcode", "plgrpname", "select plgrpcode,plgrpname from plgrpmast where active=1 order by plgrpcode", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSMktName, "plgrpname", "plgrpcode", "select plgrpname,plgrpcode from plgrpmast where active=1 order by plgrpname", True)

                txtDivCode.Value = ViewState("divcode")

                Dim lablstr As String
                lablstr = ""
                'Session.Add("Collection", "")
                'Session.Add("CNDNsperponcode", "")
                Dim adjcolno As String
                adjcolno = objUtils.GetAutoDocNoWTnew(Session("dbconnectionName"), "ADJCOL")
                txtAdjcolno.Value = adjcolno
                Session.Add("Collection" & ":" & adjcolno, "")


                Dim ds As DataSet = objUtils.ExecuteQuerySqlnew(Session("dbconnectionName"), "select top 1  sealdate from  sealing_master where div_code='" & ViewState("divcode") & "'")
                If ds.Tables.Count > 0 Then
                    If ds.Tables(0).Rows.Count > 0 Then
                        If IsDBNull(ds.Tables(0).Rows(0)("sealdate")) = False Then
                            txtpdate.Text = CType(ds.Tables(0).Rows(0)("sealdate"), String)
                        End If
                    Else
                        txtpdate.Text = objUtils.GetDBFieldFromLongnew(Session("dbconnectionName"), "reservation_parameters", "option_Selected", "param_id", 508)
                    End If
                End If


                If ViewState("CNDNOpen_type") = "DN" Then
                    lablstr = "Debit Note"

                ElseIf ViewState("CNDNOpen_type") = "CN" Then
                    lablstr = "Credit Note"
                End If
                

                If ViewState("DebitNoteState") = "New" Then
                    SetFocus(txtDate)
                    txtDate.Text = Format("dd/MM/yyyy", CType(ObjDate.GetSystemDateOnly(Session("dbconnectionName")), Date))
                    lblHeading.Text = "Add New " & lablstr
                    ' objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"),ddlCustomer, "code", "des", "select * from view_account where type = '" & ddlType.Value & "' order by code", True)
                    ' objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"),ddlCustomerName, "des", "code", "select * from view_account where type = '" & ddlType.Value & "' order by des", True)
                    btnSave.Text = "Save"
                    FillGrids()
                    btnSave.Attributes.Add("onclick", "return FormValidation('New')")
                ElseIf ViewState("DebitNoteState") = "Copy" Then
                    SetFocus(txtDate)
                    lblHeading.Text = "Copy " & lablstr
                    btnSave.Text = "Save"
                    FillGrids()
                    ShowRecord(CType(ViewState("DebitNoteRefCode"), String))
                    ShowGridDebitNote(CType(ViewState("DebitNoteRefCode"), String))
                    btnSave.Attributes.Add("onclick", "return FormValidation('New')")
                ElseIf ViewState("DebitNoteState") = "Edit" Then
                    SetFocus(txtDate)
                    lblHeading.Text = "Edit " & lablstr
                    btnSave.Text = "Update"
                    FillGrids()
                    ShowRecord(CType(ViewState("DebitNoteRefCode"), String))
                    ShowGridDebitNote(CType(ViewState("DebitNoteRefCode"), String))
                    fillcollection(CType(ViewState("DebitNoteRefCode"), String))
                    btnSave.Attributes.Add("onclick", "return FormValidation('Edit')")
                ElseIf ViewState("DebitNoteState") = "View" Then
                    SetFocus(btnCancel)
                    lblHeading.Text = "View " & lablstr
                    btnSave.Visible = False
                    FillGrids()
                    ShowRecord(CType(ViewState("DebitNoteRefCode"), String))
                    ShowGridDebitNote(CType(ViewState("DebitNoteRefCode"), String))
                    fillcollection(CType(ViewState("DebitNoteRefCode"), String))
                ElseIf ViewState("DebitNoteState") = "Delete" Then
                    SetFocus(btnSave)
                    lblHeading.Text = "Delete " & lablstr
                    btnSave.Text = "Delete"
                    FillGrids()
                    ShowRecord(CType(ViewState("DebitNoteRefCode"), String))
                    ShowGridDebitNote(CType(ViewState("DebitNoteRefCode"), String))
                    fillcollection(CType(ViewState("DebitNoteRefCode"), String))
                    btnSave.Attributes.Add("onclick", "return FormValidation('Delete')")
                ElseIf ViewState("DebitNoteState") = "Cancel" Then
                    SetFocus(btnSave)
                    lblHeading.Text = "Cancel " & lablstr
                    btnSave.Text = "Cancel"
                    FillGrids()
                    ShowRecord(CType(ViewState("DebitNoteRefCode"), String))
                    ShowGridDebitNote(CType(ViewState("DebitNoteRefCode"), String))
                    fillcollection(CType(ViewState("DebitNoteRefCode"), String))
                    btnSave.Attributes.Add("onclick", "return FormValidation('Cancel')")

                ElseIf ViewState("DebitNoteState") = "undoCancel" Then
                    SetFocus(btnSave)
                    lblHeading.Text = "Undo Cancel " & lablstr
                    btnSave.Text = "Undo"
                    FillGrids()
                    ShowRecord(CType(ViewState("DebitNoteRefCode"), String))
                    ShowGridDebitNote(CType(ViewState("DebitNoteRefCode"), String))
                    fillcollection(CType(ViewState("DebitNoteRefCode"), String))
                    btnSave.Attributes.Add("onclick", "return FormValidation('UndoCancel')")


                End If
                txtDivCode.Value = ViewState("divcode")
                'btnPrint.Attributes.Add("onclick", "return ReprintDoc()")
                Dim appname As String = ""
                Dim appidnew As String = ""
                If txtDivCode.Value = "01" Then
                    appname = "ColumbusCommon" + " " + CType("Accounts Module", String)
                    appidnew = "4"
                Else
                    appname = "ColumbusCommon Gulf " + CType("Accounts Module", String)
                    appidnew = "14"
                End If

                CheckPostUnpostRight(CType(Session("GlobalUserName"), String), CType(Session("Userpwd"), String), CType(appname, String), "AccountsModule\DebitNoteSearch.aspx?tran_type=" & ViewState("CNDNOpen_type") & "&appid=" + appidnew)
                DisableControl()

                'ddlType.Attributes.Add("onchange", "javascript:FillCustDDL('" + CType(ddlType.ClientID, String) + "','" + CType(ddlCustomer.ClientID, String) + "','" + CType(ddlCustomerName.ClientID, String) + "',,'" + CType(lblCustCode.ClientID, String) + "','" + CType(lblCustName.ClientID, String) + "')")
                ddlType.Attributes.Add("onchange", "javascript:FillCustDDL('" + CType(ddlType.ClientID, String) + "','" + CType(lblCustCode.ClientID, String) + "','" + CType(lblCustName.ClientID, String) + "')")
                txtDate.Attributes.Add("onchange", "javascript:CallWebMethod('customercode')")
                btnCancel.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to exit?')==false)return false;")
                txtConversion.Attributes.Add("onchange", "javascript:chnagerate()")
                ddlNarration.Attributes.Add("onchange", "javascript:FillCombotoText('" + CType(ddlNarration.ClientID, String) + "','" + CType(txtNarration.ClientID, String) + "')")
                NumbersHtml(txtConversion)
                Dim typ As Type
                typ = GetType(DropDownList)

                'If (Page.ClientScript.IsClientScriptIncludeRegistered(typ, "TADDScript.js")) = False Then
                Page.ClientScript.RegisterClientScriptInclude(typ, "TADDScript.js", "TADDScript.js")
                ddlCustomer.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                ddlCustomerName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                ddlType.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                ddlSalesman.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                ddlSalesmanName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                ddlNarration.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                'End If
                btnPrint.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to print?')==false)return false;")
            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objUtils.WritErrorLog("DebitNote.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            End Try
        End If

    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try


            If check_Privilege() = 1 Then

                chkPost.Enabled = True
                chkPost.Checked = True
            Else

                chkPost.Enabled = False
                chkPost.Checked = True
            End If

            'Added check_Privilege() and chkpost enabled by Archana on 01/04/2015

            If IsPostBack = True Then
                'If txtDate.Text <> "" Then
                '    txtdt.Text = CType(Format(CType(txtDate.Text, Date), "yyyy/MM/dd"), String)
                'End If
                'FillGridDebitNote()

                If ddlType.Value <> "[Select]" Then

                    lblCustCode.Text = ddlType.Items(ddlType.SelectedIndex).Text & " Code <font color='Red'> *</font>"
                    lblCustName.Text = ddlType.Items(ddlType.SelectedIndex).Text & " Name "

                    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCustomer, "code", "des", "select code, des from view_account where div_code='" & ViewState("divcode") & "' and  type = '" & ddlType.Value & "' order by code", True, txtcustcode.Value)
                    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCustomerName, "des", "code", "select  des,code from view_account where div_code='" & ViewState("divcode") & "' and  type = '" & ddlType.Value & "' order by des", True, txtcustname.Value)
                    FillControlACCODENames()
                Else
                    lblCustCode.Text = "Code <font color='Red'> *</font>"
                    lblCustName.Text = "Name "

                    'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"),ddlCustomer, "code", "des", "select top 10  code,des from view_account   order by code", True)
                    'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"),ddlCustomer, "code", "des", "select [Select]", False)
                    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCustomer, "code", "des", "select top 10  code,des from view_account where div_code='" & ViewState("divcode") & "'    order by code", False)
                    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCustomerName, "des", "code", "select top 10 des,code from view_account where div_code='" & ViewState("divcode") & "'    order by des", True)

                    'strSqlQry = " select distinct top 10 view_account.controlacctcode, acctmast.acctname  from acctmast ,view_account where   view_account.controlacctcode= acctmast.acctcode    order by  view_account.controlacctcode"
                    strSqlQry = " select acctcode controlacctcode,acctname from acctmast where div_code='" & ViewState("divcode") & "' and isnull(controlyn,'')='Y' order by acctcode"
                    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlConAccCode, "controlacctcode", "acctname", strSqlQry, True)
                    strSqlQry = " select acctname,acctcode controlacctcode from acctmast where div_code='" & ViewState("divcode") & "' and isnull(controlyn,'')='Y' order by acctname"
                    '                    strSqlQry = " select distinct top 10  acctmast.acctname,view_account.controlacctcode  from acctmast ,view_account where   view_account.controlacctcode= acctmast.acctcode    order by  acctmast.acctname"
                    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlConAccName, "acctname", "controlacctcode", strSqlQry, True)
                End If
                txtdecimal.Value = objUtils.GetDBFieldFromLongnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", 509)
                txtConversion.Disabled = False
                If Trim(txtbasecurr.Value) = Trim(txtCurrency.Value) Then
                    txtConversion.Disabled = True
                End If


                If (Me.IsPostBack And Me.Request("__EVENTTARGET") = "AdjBillWindowPostBack") Then
                    If DecRound(Session("AmountAdjusted")) <> 0 And DecRound(Session("BaseAmountAdjusted")) <> 0 Then
                        'txtCurrTotal.Value = DecRound(Session("AmountAdjusted"))
                        'txtKWDTotal.Value = DecRound(Session("BaseAmountAdjusted"))
                        txtConversion.Value = CType(Session("BaseAmountAdjusted"), Decimal) / CType(Session("AmountAdjusted"), Decimal)
                        txtConversion.Value = Math.Round(CType(txtConversion.Value, Decimal), 8)
                    End If

                    '  GrandToatal()
                    ChangeKWDValuesinGrid()
                End If

            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("DebitNote.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub

    Private Sub ChangeKWDValuesinGrid()
        Dim gvrow As GridViewRow
        Dim txtcvalue, txtbasevalue As TextBox
        Dim ctotal, cbasetotal As Decimal

        For Each gvrow In grdDebitNote.Rows
            txtcvalue = gvrow.FindControl("txtCurrValue")
            txtcvalue.Text = DecRound(txtcvalue.Text)
            txtbasevalue = gvrow.FindControl("txtKWDValue")
            If txtcvalue.Text = "" Then
                txtcvalue.Text = 0
            End If
            txtbasevalue.Text = DecRound(txtcvalue.Text) * CType(txtConversion.Value, Decimal)
            txtbasevalue.Text = DecRound(txtbasevalue.Text)

            ctotal = ctotal + DecRound(txtcvalue.Text)
            cbasetotal = cbasetotal + DecRound(txtbasevalue.Text)
        Next
        txtCurrTotal.Value = DecRound(ctotal)
        txtKWDTotal.Value = DecRound(cbasetotal)
    End Sub
    Private Sub FillControlACCODENames()

        Dim sqlstr1, sqlstr2, strtp, codeid As String
        sqlstr1 = ""
        sqlstr2 = ""
        strtp = ddlType.Value
        codeid = ddlCustomer.Items(ddlCustomer.SelectedIndex).Text
        If ddlType.Value = "C" Then
            sqlstr1 = " select acctcode controlacctcode,acctname from acctmast where div_code='" & ViewState("divcode") & "' and  isnull(controlyn,'')='Y' and isnull(cust_supp,'')='C' order by acctcode"
            sqlstr2 = " select acctname,acctcode controlacctcode from acctmast where div_code='" & ViewState("divcode") & "' and  isnull(controlyn,'')='Y' and isnull(cust_supp,'')='C' order by acctname"
        ElseIf ddlType.Value = "S" Then
            sqlstr1 = " select distinct partymast.controlacctcode  , acctmast.acctname  from acctmast ,partymast where   partymast.controlacctcode= acctmast.acctcode   and partymast.partycode='" + codeid + "' and acctmast.div_code='" & ViewState("divcode") & "' "

            sqlstr2 = " select distinct acctmast.acctname ,partymast.controlacctcode      from acctmast ,partymast where   partymast.controlacctcode= acctmast.acctcode   and partymast.partycode='" + codeid + "' and acctmast.div_code='" & ViewState("divcode") & "' order by acctmast.acctname"
            sqlstr1 = " select acctcode controlacctcode,acctname from acctmast where div_code='" & ViewState("divcode") & "' and isnull(controlyn,'')='Y' and isnull(cust_supp,'')='S' order by acctcode"
            sqlstr2 = " select acctname,acctcode controlacctcode from acctmast where div_code='" & ViewState("divcode") & "' and  isnull(controlyn,'')='Y' and isnull(cust_supp,'')='S' order by acctname"
        ElseIf ddlType.Value = "A" Then
            sqlstr1 = " select distinct supplier_agents.controlacctcode   , acctmast.acctname  from acctmast ,supplier_agents where   supplier_agents.controlacctcode= acctmast.acctcode   and supplier_agents.supagentcode='" + codeid + "' and acctmast.div_code='" & ViewState("divcode") & "' order by controlacctcode"

            sqlstr2 = " select distinct acctmast.acctname ,supplier_agents.controlacctcode     from acctmast ,supplier_agents where   supplier_agents.controlacctcode= acctmast.acctcode   and supplier_agents.supagentcode ='" + codeid + "' and acctmast.div_code='" & ViewState("divcode") & "' order by acctmast.acctname"
            sqlstr1 = " select acctcode controlacctcode,acctname from acctmast where  isnull(controlyn,'')='Y' and isnull(cust_supp,'')='S' order by acctcode"
            sqlstr2 = " select acctname,acctcode controlacctcode from acctmast where  isnull(controlyn,'')='Y' and isnull(cust_supp,'')='S' order by acctname"

        End If
        objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlConAccCode, "controlacctcode", "acctname", sqlstr1, True, txtConAccCode.Value)
        objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlConAccName, "acctname", "controlacctcode", sqlstr2, True, txtConAccName.Value)
    End Sub
#Region "Public Sub FillGrids()"
    Public Sub FillGrids()
        If ViewState("DebitNoteState") = "New" Then
            fillgrdDebitNote(grdDebitNote, True)
            'FillGridHotel()
            FillGridDebitNoteDefault()
        ElseIf ViewState("DebitNoteState") <> "New" Then
            Dim lngCnt As Long
            lngCnt = objUtils.GetDBFieldFromStringnewdiv(Session("dbconnectionName"), "trdpurchase_other", "count(tran_id)", "tran_id", CType(ViewState("DebitNoteRefCode"), String), "div_code", ViewState("divcode"))
            If lngCnt > 1 Then
                fillgrdDebitNote(grdDebitNote, False, lngCnt)
                FillGridDebitNoteDefault()
            Else
                fillgrdDebitNote(grdDebitNote, True)
                FillGridDebitNoteDefault()
            End If
        End If

    End Sub
#End Region
#Region "Private Function CreateDataSource(ByVal lngcount As Long) As DataView"
    Private Function CreateDataSource(ByVal lngcount As Long) As DataView
        Dim dt As DataTable
        Dim dr As DataRow
        Dim i As Integer
        dt = New DataTable
        dt.Columns.Add(New DataColumn("SrNo", GetType(Integer)))
        For i = 1 To lngcount
            dr = dt.NewRow()
            dr(0) = i
            dt.Rows.Add(dr)
        Next
        'return a DataView to the DataTable
        CreateDataSource = New DataView(dt)
        'End If
    End Function
#End Region
#Region "Public Sub fillgrdDebitNote(ByVal grd As GridView, Optional ByVal blnload As Boolean = False, Optional ByVal count As Integer = 1)"
    Public Sub fillgrdDebitNote(ByVal grd As GridView, Optional ByVal blnload As Boolean = False, Optional ByVal count As Integer = 1)
        Dim lngcnt As Long
        If blnload = True Then
            lngcnt = 1
        Else
            lngcnt = count
        End If

        grd.DataSource = CreateDataSource(lngcnt)
        grd.DataBind()
        grd.Visible = True
    End Sub
#End Region
#Region "Private Sub AddLinesToDebitNote()"
    Private Sub AddLinesToDebitNote()
        Dim count As Integer
        Dim ddlAccCode As HtmlSelect
        Dim ddlAccName As HtmlSelect
        Dim ddlCCCode As HtmlSelect
        Dim ddlCCName As HtmlSelect

        Dim txtPartic As TextBox
        Dim txtCValue As TextBox
        Dim txtKWValue As TextBox

        count = grdDebitNote.Rows.Count + 1
        Dim AccCode(count) As String
        Dim AccName(count) As String
        Dim CCCode(count) As String
        Dim CCName(count) As String
        Dim Partic(count) As String
        Dim CValue(count) As String
        Dim KWValue(count) As String
        Dim n As Integer = 0
        Dim i As Integer
        Try
            'If txtDate.Text <> "" Then
            '    txtdt.Text = CType(Format(CType(txtDate.Text, Date), "yyyy/MM/dd"), String)
            'End If
            For Each gvRow In grdDebitNote.Rows
                ddlAccCode = gvRow.FindControl("ddlAccountCode")
                ddlAccName = gvRow.FindControl("ddlAccountName")
                ddlCCCode = gvRow.FindControl("ddlCostCode")
                ddlCCName = gvRow.FindControl("ddlCostName")
                txtPartic = gvRow.FindControl("txtParticulars")
                txtCValue = gvRow.FindControl("txtCurrValue")
                txtKWValue = gvRow.FindControl("txtKWDValue")

                AccCode(n) = CType(ddlAccCode.Value, String)
                AccName(n) = CType(ddlAccName.Value, String)
                CCCode(n) = CType(ddlCCCode.Value, String)
                CCName(n) = CType(ddlCCName.Value, String)
                Partic(n) = CType(txtPartic.Text, String)
                CValue(n) = CType(txtCValue.Text, String)
                KWValue(n) = CType(txtKWValue.Text, String)

                n = n + 1
            Next
            fillgrdDebitNote(grdDebitNote, False, grdDebitNote.Rows.Count + 1)
            FillGridDebitNoteDefault()
            i = n
            n = 0
            For Each gvRow In grdDebitNote.Rows
                If n = i Then
                    Exit For
                End If
                ddlAccCode = gvRow.FindControl("ddlAccountCode")
                ddlAccName = gvRow.FindControl("ddlAccountName")
                ddlCCCode = gvRow.FindControl("ddlCostCode")
                ddlCCName = gvRow.FindControl("ddlCostName")
                txtPartic = gvRow.FindControl("txtParticulars")
                txtCValue = gvRow.FindControl("txtCurrValue")
                txtKWValue = gvRow.FindControl("txtKWDValue")

                'If AccCode(n) <> "[Select]" Then
                'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"),ddlAccCode, "acctcode", "acctname", "select acctcode, acctname from acctmast where controlyn='N' and bankyn='N' order by acctcode ", True)
                ddlAccCode.Value = AccCode(n)

                'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"),ddlAccName, "acctname", "acctcode", "select acctcode, acctname from acctmast where controlyn='N' and bankyn='N' order by acctname ", True)
                ddlAccName.Value = AccName(n)

                'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"),ddlCCCode, "costcenter_code", "costcenter_name", "select costcenter_code, costcenter_name from costcenter_master where active=1 order by costcenter_code ", True)
                ddlCCCode.Value = CCCode(n)

                'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"),ddlCCName, "costcenter_name", "costcenter_code", "select costcenter_code, costcenter_name from costcenter_master where active=1 order by costcenter_name ", True)
                ddlCCName.Value = CCName(n)

                txtPartic.Text = Partic(n)
                txtCValue.Text = CValue(n)
                If Val(KWValue(n)) = 0 Then
                    txtKWValue.Text = Math.Round(Val(txtConversion.Value) * Val(txtCValue.Text), CType(txtdecimal.Value, Integer))
                Else
                    txtKWValue.Text = KWValue(n)
                End If

                n = n + 1
            Next

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        End Try

    End Sub
#End Region
#Region "Private Sub DelLinesToDebitNote()"
    Private Sub DelLinesToDebitNote()
        Dim count As Integer
        Dim ddlAccCode As HtmlSelect
        Dim ddlAccName As HtmlSelect
        Dim ddlCCCode As HtmlSelect
        Dim ddlCCName As HtmlSelect

        Dim txtPartic As TextBox
        Dim txtCValue As TextBox
        Dim txtKWValue As TextBox

        count = grdDebitNote.Rows.Count + 1
        Dim AccCode(count) As String
        Dim AccName(count) As String
        Dim CCCode(count) As String
        Dim CCName(count) As String
        Dim Partic(count) As String
        Dim CValue(count) As String
        Dim KWValue(count) As String
        Dim n As Integer = 0
        Dim i As Integer
        Dim ctot, ktot As Decimal
        ctot = 0
        ktot = 0
        Try
            'If txtDate.Text <> "" Then
            '    txtdt.Text = CType(Format(CType(txtDate.Text, Date), "yyyy/MM/dd"), String)
            'End If
            For Each gvRow In grdDebitNote.Rows
                chckDeletion = gvRow.FindControl("chckDeletion")
                If chckDeletion.Checked = False Then
                    ddlAccCode = gvRow.FindControl("ddlAccountCode")
                    ddlAccName = gvRow.FindControl("ddlAccountName")
                    ddlCCCode = gvRow.FindControl("ddlCostCode")
                    ddlCCName = gvRow.FindControl("ddlCostName")
                    txtPartic = gvRow.FindControl("txtParticulars")
                    txtCValue = gvRow.FindControl("txtCurrValue")
                    txtKWValue = gvRow.FindControl("txtKWDValue")

                    AccCode(n) = CType(ddlAccCode.Value, String)
                    AccName(n) = CType(ddlAccName.Value, String)
                    CCCode(n) = CType(ddlCCCode.Value, String)
                    CCName(n) = CType(ddlCCName.Value, String)
                    Partic(n) = CType(txtPartic.Text, String)
                    CValue(n) = CType(txtCValue.Text, String)
                    KWValue(n) = CType(txtKWValue.Text, String)
                    n = n + 1
                End If
            Next
            count = n
            If count = 0 Then
                count = 1
            End If
            fillgrdDebitNote(grdDebitNote, False, count)
            FillGridDebitNoteDefault()
            i = n
            n = 0
            For Each gvRow In grdDebitNote.Rows
                If n = i Then
                    Exit For
                End If
                ddlAccCode = gvRow.FindControl("ddlAccountCode")
                ddlAccName = gvRow.FindControl("ddlAccountName")
                ddlCCCode = gvRow.FindControl("ddlCostCode")
                ddlCCName = gvRow.FindControl("ddlCostName")
                txtPartic = gvRow.FindControl("txtParticulars")
                txtCValue = gvRow.FindControl("txtCurrValue")
                txtKWValue = gvRow.FindControl("txtKWDValue")

                'If AccCode(n) <> "[Select]" Then
                'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"),ddlAccCode, "acctcode", "acctname", "select acctcode, acctname from acctmast where controlyn='N' and bankyn='N' order by acctcode ", True)
                ddlAccCode.Value = AccCode(n)

                'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"),ddlAccName, "acctname", "acctcode", "select acctcode, acctname from acctmast where controlyn='N' and bankyn='N' order by acctname ", True)
                ddlAccName.Value = AccName(n)

                'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"),ddlCCCode, "costcenter_code", "costcenter_name", "select costcenter_code, costcenter_name from costcenter_master where active=1 order by costcenter_code ", True)
                ddlCCCode.Value = CCCode(n)

                'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"),ddlCCName, "costcenter_name", "costcenter_code", "select costcenter_code, costcenter_name from costcenter_master where active=1 order by costcenter_name ", True)
                ddlCCName.Value = CCName(n)

                txtPartic.Text = Partic(n)
                txtCValue.Text = CValue(n)
                If Val(KWValue(n)) = 0 Then
                    txtKWValue.Text = Math.Round(Val(txtConversion.Value) * Val(txtCValue.Text), CType(txtdecimal.Value, Integer))
                Else
                    txtKWValue.Text = KWValue(n)
                End If
                ctot = ctot + Val(txtCValue.Text)
                ktot = ktot + Val(txtKWValue.Text)

                n = n + 1
            Next
            txtCurrTotal.Value = Math.Round(ctot, CType(txtdecimal.Value, Integer))
            txtKWDTotal.Value = Math.Round(ktot, CType(txtdecimal.Value, Integer))
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        End Try

    End Sub
#End Region
#Region "Private Sub FillGridDebitNote()"
    Private Sub FillGridDebitNote()
        Dim count As Integer
        Dim ddlAccCode As HtmlSelect
        Dim ddlAccName As HtmlSelect
        Dim ddlCCCode As HtmlSelect
        Dim ddlCCName As HtmlSelect

        Dim txtPartic As TextBox
        Dim txtCValue As TextBox
        Dim txtKWValue As TextBox

        count = grdDebitNote.Rows.Count + 1
        Dim AccCode(count) As String
        Dim AccName(count) As String
        Dim CCCode(count) As String
        Dim CCName(count) As String
        Dim Partic(count) As String
        Dim CValue(count) As String
        Dim KWValue(count) As String
        Dim n As Integer = 0
        Try
            'If txtDate.Text <> "" Then
            '    txtdt.Text = CType(Format(CType(txtDate.Text, Date), "yyyy/MM/dd"), String)
            'End If
            For Each gvRow In grdDebitNote.Rows
                ddlAccCode = gvRow.FindControl("ddlAccountCode")
                ddlAccName = gvRow.FindControl("ddlAccountName")
                ddlCCCode = gvRow.FindControl("ddlCostCode")
                ddlCCName = gvRow.FindControl("ddlCostName")
                txtPartic = gvRow.FindControl("txtParticulars")
                txtCValue = gvRow.FindControl("txtCurrValue")
                txtKWValue = gvRow.FindControl("txtKWDValue")

                AccCode(n) = CType(ddlAccCode.Value, String)
                AccName(n) = CType(ddlAccName.Value, String)
                CCCode(n) = CType(ddlCCCode.Value, String)
                CCName(n) = CType(ddlCCName.Value, String)
                Partic(n) = CType(txtPartic.Text, String)
                CValue(n) = CType(txtCValue.Text, String)
                KWValue(n) = CType(txtKWValue.Text, String)
                n = n + 1
            Next
            Dim i As Integer = n
            n = 0
            For Each gvRow In grdDebitNote.Rows
                ddlAccCode = gvRow.FindControl("ddlAccountCode")
                ddlAccName = gvRow.FindControl("ddlAccountName")
                ddlCCCode = gvRow.FindControl("ddlCostCode")
                ddlCCName = gvRow.FindControl("ddlCostName")
                txtPartic = gvRow.FindControl("txtParticulars")
                txtCValue = gvRow.FindControl("txtCurrValue")
                txtKWValue = gvRow.FindControl("txtKWDValue")

                If AccCode(n) <> "[Select]" Then
                    'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"),ddlAccCode, "acctcode", "acctname", "select acctcode, acctname from acctmast where controlyn='N' and bankyn='N' order by acctcode ", True)
                    ddlAccCode.Value = AccCode(n)

                    'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"),ddlAccName, "acctname", "acctcode", "select acctcode, acctname from acctmast where controlyn='N' and bankyn='N' order by acctname ", True)
                    ddlAccName.Value = AccName(n)

                    'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"),ddlCCCode, "costcenter_code", "costcenter_name", "select costcenter_code, costcenter_name from costcenter_master where active=1 order by costcenter_code ", True)
                    ddlCCCode.Value = CCCode(n)

                    'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"),ddlCCName, "costcenter_name", "costcenter_code", "select costcenter_code, costcenter_name from costcenter_master where active=1 order by costcenter_name ", True)
                    ddlCCName.Value = CCName(n)

                    txtPartic.Text = Partic(n)
                    txtCValue.Text = CValue(n)
                    txtKWValue.Text = KWValue(n)
                End If
            Next
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("Debitnote.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#End Region
#Region "Private Sub FillGridDebitNoteDefault()"
    Private Sub FillGridDebitNoteDefault()
        Try
            For Each gvRow In grdDebitNote.Rows

                'Dim count As Integer
                Dim ddlAccCode As HtmlSelect
                Dim ddlAccName As HtmlSelect
                Dim ddlCCCode As HtmlSelect
                Dim ddlCCName As HtmlSelect
                Dim txtauto As HtmlInputText
                Dim txtPartic As TextBox
                Dim txtCValue As TextBox
                Dim txtKWValue As TextBox

                ddlAccCode = gvRow.FindControl("ddlAccountCode")
                ddlAccName = gvRow.FindControl("ddlAccountName")
                ddlCCCode = gvRow.FindControl("ddlCostCode")
                ddlCCName = gvRow.FindControl("ddlCostName")
                txtPartic = gvRow.FindControl("txtParticulars")
                txtCValue = gvRow.FindControl("txtCurrValue")
                txtKWValue = gvRow.FindControl("txtKWDValue")
                txtauto = gvRow.FindControl("accSearch")

                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlAccCode, "acctcode", "acctname", "select acctcode, acctname from acctmast where div_code='" & ViewState("divcode") & "' and controlyn='N' and bankyn='N' order by acctcode ", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlAccName, "acctname", "acctcode", "select acctcode, acctname from acctmast where div_code='" & ViewState("divcode") & "' and controlyn='N' and bankyn='N' order by acctname ", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCCCode, "costcenter_code", "costcenter_name", "select costcenter_code, costcenter_name from costcenter_master where active=1 order by costcenter_code ", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCCName, "costcenter_name", "costcenter_code", "select costcenter_code, costcenter_name from costcenter_master where active=1 order by costcenter_name ", True)

                NumbersDecimalRound(txtCValue)
                NumbersDecimalRound(txtKWValue)

                ddlAccCode.Attributes.Add("onchange", "javascript:FillACCodeName('" + CType(ddlAccCode.ClientID, String) + "','" + CType(ddlAccName.ClientID, String) + "','" + CType(txtPartic.ClientID, String) + "')")
                ddlAccName.Attributes.Add("onchange", "javascript:FillACCodeName('" + CType(ddlAccName.ClientID, String) + "','" + CType(ddlAccCode.ClientID, String) + "','" + CType(txtPartic.ClientID, String) + "')")

                ddlCCCode.Attributes.Add("onchange", "javascript:FillCodeName('" + CType(ddlCCCode.ClientID, String) + "','" + CType(ddlCCName.ClientID, String) + "')")
                ddlCCName.Attributes.Add("onchange", "javascript:FillCodeName('" + CType(ddlCCName.ClientID, String) + "','" + CType(ddlCCCode.ClientID, String) + "')")
                txtCValue.Attributes.Add("onchange", "javascript:FillPriceChanges('" + CType(txtKWValue.ClientID, String) + "','" + CType(txtCValue.ClientID, String) + "','" + CType(txtConversion.ClientID, String) + "')")
                txtauto.Attributes.Add("onfocus", "javascript:MyAutodebitsFillArray('" + CType(txtauto.ClientID, String) + "','" + CType(ddlAccName.ClientID, String) + "')")

                Dim typ As Type
                typ = GetType(DropDownList)

                If (Page.ClientScript.IsClientScriptIncludeRegistered(typ, "TADDScript.js")) = False Then
                    Page.ClientScript.RegisterClientScriptInclude(typ, "TADDScript.js", "TADDScript.js")

                    ddlAccCode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlAccName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlCCCode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlCCName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                End If
            Next
            txtgridrows.Value = grdDebitNote.Rows.Count
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("DebitNote.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try

    End Sub
#End Region
    Protected Sub btnAddLine_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        AddLinesToDebitNote()
    End Sub
    Protected Sub btnDelLine_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        DelLinesToDebitNote()
    End Sub
#Region "Private Sub ShowRecord(ByVal RefCode As String)"
    Private Sub ShowRecord(ByVal RefCode As String)
        Try
            chkPost.Checked = False
            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            myCommand = New SqlCommand("Select * from trdpurchase_master Where div_id='" & ViewState("divcode") & "' and tran_id='" & RefCode & "' and tran_type='" & CType(ViewState("CNDNOpen_type"), String) & "'", SqlConn)
            mySqlReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection)
            If mySqlReader.HasRows Then
                If mySqlReader.Read() = True Then
                    If ViewState("DebitNoteState") <> "Copy" Then
                        If IsDBNull(mySqlReader("post_state")) = False Then
                            If CType(mySqlReader("post_state"), String) = "P" Then
                                lblPostmsg.Text = "Posted"
                                lblPostmsg.ForeColor = Drawing.Color.Red
                                chkPost.Checked = True
                            Else
                                lblPostmsg.Text = "UnPosted"
                                lblPostmsg.ForeColor = Drawing.Color.Green
                            End If
                        Else
                            lblPostmsg.Text = "UnPosted"
                            lblPostmsg.ForeColor = Drawing.Color.Green
                        End If
                        If IsDBNull(mySqlReader("tran_id")) = False Then
                            Me.txtDocNo.Text = CType(mySqlReader("tran_id"), String)
                        Else
                            Me.txtDocNo.Text = ""
                        End If
                    End If

                    If IsDBNull(mySqlReader("cancel_state")) = False Then
                        If CType(mySqlReader("cancel_state"), String) = "Y" Then
                            lblPostmsg.Text = "Cancelled"
                            lblPostmsg.ForeColor = Drawing.Color.Green
                        End If
                    End If

                    If IsDBNull(mySqlReader("tran_date")) = False Then
                        Me.txtDate.Text = CType(Format(CType(mySqlReader("tran_date"), Date), "dd/MM/yyyy"), String)
                    Else
                        Me.txtDate.Text = ""
                    End If
                    If IsDBNull(mySqlReader("supref")) = False Then
                        Me.txtReferenceNo.Text = CType(mySqlReader("supref"), String)
                    Else
                        Me.txtReferenceNo.Text = ""
                    End If
                    If IsDBNull(mySqlReader("due_date")) = False Then
                        Me.txtDueDate.Text = CType(Format(CType(mySqlReader("due_date"), Date), "dd/MM/yyyy"), String)
                    Else
                        Me.txtDueDate.Text = ""
                    End If
                    If IsDBNull(mySqlReader("acc_type")) = False Then
                        Me.ddlType.Value = CType(mySqlReader("acc_type"), String)
                    Else
                        ddlType.Value = "[Select]"
                    End If

                    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCustomer, "code", "des", "select * from view_account where div_code='" & ViewState("divcode") & "' and type = '" & ddlType.Value & "' order by code", True)
                    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCustomerName, "des", "code", "select * from view_account where div_code='" & ViewState("divcode") & "' and type = '" & ddlType.Value & "' order by des", True)

                    If IsDBNull(mySqlReader("supcode")) = False Then
                        ddlCustomerName.Value = CType(mySqlReader("supcode"), String)
                        ddlCustomer.Value = objUtils.GetDBFieldFromMultipleCriterianew(Session("dbconnectionName"), "view_account", "des", "code='" & CType(mySqlReader("supcode") & "' and type='" & Me.ddlType.Value & "'", String))
                        txtcustcode.Value = objUtils.GetDBFieldFromMultipleCriterianew(Session("dbconnectionName"), "view_account", "des", "code='" & CType(mySqlReader("supcode") & "' and type='" & Me.ddlType.Value & "'", String))
                        txtcustname.Value = CType(mySqlReader("supcode"), String)
                    Else
                        Me.ddlCustomerName.Value = "[Select]"
                        Me.ddlCustomer.Value = "[Select]"
                    End If

                    If ddlType.Value <> "[Select]" Then
                        lblCustCode.Text = ddlType.Items(ddlType.SelectedIndex).Text & " Code <font color='Red'> *</font>"
                        lblCustName.Text = ddlType.Items(ddlType.SelectedIndex).Text & " Name "
                    Else
                        lblCustCode.Text = "Code <font color='Red'> *</font>"
                        lblCustName.Text = "Name "
                    End If


                    If IsDBNull(mySqlReader("currcode")) = False Then
                        Me.txtCurrency.Value = CType(mySqlReader("currcode"), String)
                    Else
                        Me.txtCurrency.Value = ""
                    End If
                    If IsDBNull(mySqlReader("convrate")) = False Then
                        Me.txtConversion.Value = CType(mySqlReader("convrate"), String)
                    Else
                        Me.txtConversion.Value = ""
                    End If
                    If IsDBNull(mySqlReader("total")) = False Then
                        Me.txtCurrTotal.Value = CType(mySqlReader("total"), String)
                    Else
                        Me.txtCurrTotal.Value = ""
                    End If
                    If IsDBNull(mySqlReader("Basetotal")) = False Then
                        Me.txtKWDTotal.Value = CType(mySqlReader("Basetotal"), String)
                    Else
                        Me.txtKWDTotal.Value = ""
                    End If
                    If IsDBNull(mySqlReader("remarks")) = False Then
                        Me.txtNarration.Value = CType(mySqlReader("remarks"), String)
                    Else
                        Me.txtNarration.Value = ""
                    End If
                    If IsDBNull(mySqlReader("sperson")) = False Then
                        Me.ddlSalesmanName.Value = CType(mySqlReader("sperson"), String)
                        Me.ddlSalesman.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "UserMaster", "UserName", "UserCode", CType(mySqlReader("sperson"), String))
                    Else
                        Me.ddlSalesmanName.Value = "[Select]"
                        Me.ddlSalesman.Value = "[Select]"
                    End If
                    If IsDBNull(mySqlReader("trd_gl_code")) = False Then
                        'ddlConAccName.Value = mySqlReader("trd_gl_code").ToString
                        'ddlConAccCode.Value = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"),"select acctname from acctmast where acctcode ='" & mySqlReader("trd_gl_code").ToString & "' ")
                        txtConAccName.Value = mySqlReader("trd_gl_code").ToString
                        txtConAccCode.Value = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select acctname from acctmast where div_code='" & ViewState("divcode") & "' and acctcode ='" & mySqlReader("trd_gl_code").ToString & "' ")
                    Else
                        ddlConAccName.Value = "[Select]"
                        ddlConAccCode.Value = "[Select]"
                        txtConAccName.Value = ""
                        txtConAccCode.Value = ""
                    End If

                    If IsDBNull(mySqlReader("plgrpcode")) = False Then
                        ddlSMktName.Value = mySqlReader("plgrpcode").ToString

                        ddlSMktCode.Value = ddlSMktName.Items(ddlSMktName.SelectedIndex).Text

                    End If

                    FillControlACCODENames()
                    txtConversion.Disabled = False
                    If Trim(txtbasecurr.Value) = Trim(txtCurrency.Value) Then
                        txtConversion.Disabled = True
                    End If

                End If

            End If

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("DebitNote.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            clsDBConnect.dbCommandClose(myCommand)                  'sql command disposed
            clsDBConnect.dbReaderClose(mySqlReader)                 'sql reader disposed    
            clsDBConnect.dbConnectionClose(SqlConn)                 'connection close           
        End Try
    End Sub
#End Region
#Region "Private Sub ShowGridDebitNote(ByVal RefCode As String)"
    Private Sub ShowGridDebitNote(ByVal RefCode As String)
        Try
            Dim lblLineNo As Label
            Dim ddlAccCode As HtmlSelect
            Dim ddlAccName As HtmlSelect
            Dim ddlCCCode As HtmlSelect
            Dim ddlCCName As HtmlSelect

            Dim txtPartic As TextBox
            Dim txtCValue As TextBox
            Dim txtKWValue As TextBox

            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            myCommand = New SqlCommand("Select * from trdpurchase_other Where div_code='" & ViewState("divcode") & "' and tran_id='" & RefCode & "' and tran_type='" & CType(ViewState("CNDNOpen_type"), String) & "'", SqlConn)
            mySqlReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection)
            If mySqlReader.HasRows Then
                While mySqlReader.Read()
                    For Each gvRow In grdDebitNote.Rows
                        lblLineNo = gvRow.FindControl("lblLineID")
                        If lblLineNo.Text = CType(mySqlReader("Sno"), String) - 1 Then
                            ddlAccCode = gvRow.FindControl("ddlAccountCode")
                            ddlAccName = gvRow.FindControl("ddlAccountName")
                            ddlCCCode = gvRow.FindControl("ddlCostCode")
                            ddlCCName = gvRow.FindControl("ddlCostName")
                            txtPartic = gvRow.FindControl("txtParticulars")
                            txtCValue = gvRow.FindControl("txtCurrValue")
                            txtKWValue = gvRow.FindControl("txtKWDValue")

                            If IsDBNull(mySqlReader("acc_code")) = False Then
                                ddlAccCode.Value = objUtils.GetDBFieldFromStringnewdiv(Session("dbconnectionName"), "acctmast", "acctname", "acctcode", CType(mySqlReader("acc_code"), String), "div_code", ViewState("divcode"))
                                ddlAccName.Value = CType(mySqlReader("acc_code"), String)
                            End If

                            If IsDBNull(mySqlReader("costcenter_code")) = False Then
                                ddlCCCode.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "costcenter_master", "costcenter_name", "costcenter_code", CType(mySqlReader("costcenter_code"), String))
                                ddlCCName.Value = CType(mySqlReader("costcenter_code"), String)
                            End If

                            If IsDBNull(mySqlReader("particulars")) = False Then
                                txtPartic.Text = CType(mySqlReader("particulars"), String)
                            End If

                            If IsDBNull(mySqlReader("amount")) = False Then
                                txtCValue.Text = DecRound(CType(mySqlReader("amount"), String))
                            End If

                            If IsDBNull(mySqlReader("baseamount")) = False Then
                                txtKWValue.Text = DecRound(CType(mySqlReader("baseamount"), String))
                            End If

                            Exit For
                        End If
                    Next
                End While
            End If

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("DebitNote.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            clsDBConnect.dbCommandClose(myCommand)               'sql command disposed
            clsDBConnect.dbReaderClose(mySqlReader)              'sql reader disposed    
            clsDBConnect.dbConnectionClose(SqlConn)              'connection close           
        End Try
    End Sub
#End Region

    Public Function Validateseal() As Boolean
        Try

            Validateseal = True
            Dim invdate As DateTime
            Dim sealdate As DateTime
            Dim MyCultureInfo As New CultureInfo("fr-Fr")
            invdate = DateTime.Parse(txtDate.Text, MyCultureInfo, DateTimeStyles.None)
            sealdate = DateTime.Parse(txtpdate.Text, MyCultureInfo, DateTimeStyles.None)
            If invdate <= sealdate Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Document has sealed in this period cannot make entry.Close the entry and make with another date')", True)
                Validateseal = False
            End If

        Catch ex As Exception
            Validateseal = False
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("DebitNote.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Function
#Region "ValidateGrids()"
    Public Function ValidateGrids() As Boolean
        Try
            Dim flgCheck As Boolean = False
            Dim marketcheck As String



            Dim ddlAccCode As HtmlSelect
            Dim ddlAccName As HtmlSelect
            Dim ddlCCCode As HtmlSelect
            Dim ddlCCName As HtmlSelect

            Dim txtPartic As TextBox
            Dim txtCValue As TextBox
            Dim txtKWValue As TextBox

            For Each gvRow In grdDebitNote.Rows
                ddlAccCode = gvRow.FindControl("ddlAccountCode")
                ddlAccName = gvRow.FindControl("ddlAccountName")
                ddlCCCode = gvRow.FindControl("ddlCostCode")
                ddlCCName = gvRow.FindControl("ddlCostName")
                txtPartic = gvRow.FindControl("txtParticulars")
                txtCValue = gvRow.FindControl("txtCurrValue")
                txtKWValue = gvRow.FindControl("txtKWDValue")

                If ddlAccCode.Value <> "[Select]" Or ddlCCCode.Value <> "[Select]" Or txtCValue.Text <> "" Or txtKWValue.Text <> "" Then

                    marketcheck = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select 't'  from acctgroup where div_code='" & ViewState("divcode") & "' and childid in (115,120) and acctcode ='" & CType(ddlAccCode.Items(ddlAccCode.SelectedIndex).Text, String) & "' ")

                    'If marketcheck <> "" And ddlSMktCode.Value = "[Select]" Then
                    '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Select Market.');", True)

                    '    ValidateGrids = False
                    '    Exit Function
                    'End If

                    If ddlAccCode.Value = "[Select]" Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Select Account Code for debit note.');", True)
                        SetFocus(grdDebitNote)
                        ValidateGrids = False
                        Exit Function
                    End If
                    'If ddlCCCode.Value = "[Select]" Then
                    '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Select Cost Center Code for debit note.');", True)
                    '    SetFocus(grdDebitNote)
                    '    ValidateGrids = False
                    '    Exit Function
                    'End If
                    If txtCValue.Text = "" Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Enter Curr. Vlaue for debit note.');", True)
                        SetFocus(grdDebitNote)
                        ValidateGrids = False
                        Exit Function
                    End If
                    'If txtKWValue.Text = "" Then
                    '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert(' KWD. Vlaue for debit note should not be blank.');", True)
                    '    SetFocus(grdDebitNote)
                    '    ValidateGrids = False
                    '    Exit Function
                    'End If
                    flgCheck = True
                End If

            Next
            If flgCheck = False Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Debit Note grid should not be blank.');", True)
                SetFocus(grdDebitNote)
                ValidateGrids = False
                Exit Function
            End If


            If CheckAdjustBill() = True Then
                If ddlType.Value <> "[Select]" Then
                    If validate_AdjustBill(1, ddlCustomer.Items(ddlCustomer.SelectedIndex).Text, ddlType.Value, ddlConAccCode.Items(ddlConAccCode.SelectedIndex).Text, CType(Val(txtKWDTotal.Value), Decimal)) = False Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please enter valid Bill Adjust amount.');", True)
                        ValidateGrids = False
                        Exit Function
                    End If
                End If
            End If
            ValidateGrids = True

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        End Try
    End Function
#End Region
    Private Function CheckAdjustBill() As Boolean
        CheckAdjustBill = False
        Dim intLineNo As Integer = 1
        Dim strLineKey As String

        strLineKey = "AgainstTranLineNo" & intLineNo & ":" & 1
        Dim collectionDate As Collection
        ' If Not Session("Collection") Is Nothing Then
        'If Session("Collection").ToString <> "" Then
        'collectionDate = CType(Session("Collection"), Collection)
        collectionDate = GetCollectionFromSession()
        If collectionDate.Count <> 0 Then
            If colexists(collectionDate, strLineKey) = True Then
                CheckAdjustBill = True
                Exit Function
            End If
        End If
        'End If
        'End If
    End Function
#Region "Private Sub DisableControl()"
    Private Sub DisableControl()
        If ViewState("DebitNoteState") = "New" Then
            btnPrint.Visible = False
            btnSave.Visible = True
        ElseIf ViewState("DebitNoteState") = "Copy" Then
            btnPrint.Visible = False
            btnSave.Visible = True
        ElseIf ViewState("DebitNoteState") = "Edit" Then
            btnPrint.Visible = False
            btnSave.Visible = True
        ElseIf ViewState("DebitNoteState") = "Delete" Or ViewState("DebitNoteState") = "View" Or ViewState("DebitNoteState") = "Cancel" Or ViewState("DebitNoteState") = "undoCancel" Then
            txtDocNo.Enabled = False
            txtDate.Enabled = False
            txtDueDate.Enabled = False
            txtReferenceNo.Enabled = False
            txtNarration.Disabled = False
            txtCurrency.Disabled = True
            txtConversion.Disabled = True
            ddlType.Disabled = True
            ddlCustomer.Disabled = True
            ddlCustomerName.Disabled = True
            ddlSalesman.Disabled = True
            ddlSalesmanName.Disabled = True
            ImgBtnFrmDt.Enabled = False
            ImgBtnRevDate.Enabled = False
            ddlConAccCode.Disabled = True
            ddlConAccName.Disabled = True
            ddlSMktCode.Disabled = True
            ddlSMktName.Disabled = True

            DisableGrid()
            btnAddLine.Visible = False
            btnDelLine.Visible = False
            btnAdjustBill.Visible = True
            btnPrint.Visible = False
            btnSave.Visible = False
            chkPost.Visible = False
        End If
        If ViewState("DebitNoteState") = "View" Then
            btnPrint.Visible = True
            btnSave.Visible = False
        ElseIf ViewState("DebitNoteState") = "Delete" Then
            btnSave.Visible = True
        ElseIf ViewState("DebitNoteState") = "Cancel" Or ViewState("DebitNoteState") = "undoCancel" Then
            btnPrint.Visible = False
            btnSave.Visible = True
        End If
    End Sub

#End Region
    Private Sub DisableGrid()
        'Disable for DebitNote
        Dim ddlAccCode As HtmlSelect
        Dim ddlAccName As HtmlSelect
        Dim ddlCCCode As HtmlSelect
        Dim ddlCCName As HtmlSelect

        Dim txtPartic As TextBox
        Dim txtCValue As TextBox
        Dim txtKWValue As TextBox

        Try
            For Each gvRow In grdDebitNote.Rows
                ddlAccCode = gvRow.FindControl("ddlAccountCode")
                ddlAccName = gvRow.FindControl("ddlAccountName")
                ddlCCCode = gvRow.FindControl("ddlCostCode")
                ddlCCName = gvRow.FindControl("ddlCostName")
                txtPartic = gvRow.FindControl("txtParticulars")
                txtCValue = gvRow.FindControl("txtCurrValue")
                txtKWValue = gvRow.FindControl("txtKWDValue")
                chckDeletion = gvRow.FindControl("chckDeletion")

                ddlAccCode.Disabled = True
                ddlAccName.Disabled = True
                ddlCCCode.Disabled = True
                ddlCCName.Disabled = True
                txtPartic.Enabled = False
                txtCValue.Enabled = False
                txtKWValue.Enabled = False

                chckDeletion.Enabled = False
            Next

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        End Try

    End Sub
#Region "Public Sub Save_Open_detail(ByVal intReceiptLinNo As String, ByVal SqlConn As SqlConnection, ByVal sqlTrans As SqlTransaction)"
    Public Sub Save_Open_detail(ByVal intReceiptLinNo As String, ByVal strAccCode As String, ByVal strAccType As String, ByVal strGlCode As String, ByVal SqlConn As SqlConnection, ByVal sqlTrans As SqlTransaction)
        Dim collectionDate As Collection
        Dim spersoncode As String
        Dim strdiv As String
        Dim strLineKey As String
        Dim MainGrdCount As Integer = 1

        'If Session("Collection").ToString <> "" Then
        'collectionDate = CType(Session("Collection"), Collection)
        collectionDate = GetCollectionFromSession()
        If collectionDate.Count <> 0 Then
            strdiv = ViewState("divcode") ' objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", 511)
            Dim intcount As Integer = collectionDate.Count / 21
            Dim intLinNo, MainRowidx As Integer
            MainRowidx = 1
            For MainRowidx = 1 To MainGrdCount
                If MainRowidx = intReceiptLinNo Then
                    For intLinNo = 1 To intcount
                        strLineKey = intLinNo & ":" & intReceiptLinNo
                        If colexists(collectionDate, "AgainstTranLineNo" & strLineKey) = True Then
                            If collectionDate("AccCode" & strLineKey).ToString = strAccCode And collectionDate("AccType" & strLineKey).ToString = strAccType And collectionDate("AccGLCode" & strLineKey).ToString = strGlCode Then
                                myCommand = New SqlCommand("sp_add_open_detail_new", SqlConn, sqlTrans)
                                myCommand.CommandType = CommandType.StoredProcedure
                                myCommand.Parameters.Add(New SqlParameter("@tran_id", SqlDbType.VarChar, 20)).Value = collectionDate("TranId" & strLineKey).ToString
                                myCommand.Parameters.Add(New SqlParameter("@tran_type", SqlDbType.VarChar, 10)).Value = collectionDate("TranType" & strLineKey).ToString
                                myCommand.Parameters.Add(New SqlParameter("@tran_date ", SqlDbType.DateTime)).Value = Format(CType(collectionDate("TranDate" & strLineKey).ToString, Date), "yyyy/MM/dd")
                                myCommand.Parameters.Add(New SqlParameter("@tran_lineno", SqlDbType.Int)).Value = collectionDate("AccTranLineNo" & strLineKey)

                                myCommand.Parameters.Add(New SqlParameter("@against_tran_id", SqlDbType.VarChar, 20)).Value = txtDocNo.Text
                                myCommand.Parameters.Add(New SqlParameter("@against_tran_lineno", SqlDbType.Int)).Value = intReceiptLinNo 'collectionDate("AgainstTranLineNo" & strLineKey) 'intReceiptLinNo
                                myCommand.Parameters.Add(New SqlParameter("@against_tran_type", SqlDbType.VarChar, 10)).Value = CType(ViewState("CNDNOpen_type"), String)
                                myCommand.Parameters.Add(New SqlParameter("@against_tran_date ", SqlDbType.DateTime)).Value = Format(CType(txtDate.Text, Date), "yyyy/MM/dd")
                                If collectionDate("DueDate" & strLineKey).ToString = "" Then
                                    myCommand.Parameters.Add(New SqlParameter("@open_due_date ", SqlDbType.DateTime)).Value = DBNull.Value
                                Else
                                    myCommand.Parameters.Add(New SqlParameter("@open_due_date ", SqlDbType.DateTime)).Value = Format(CType(collectionDate("DueDate" & strLineKey).ToString, Date), "yyyy/MM/dd")
                                End If

                                If strAccType = "C" Then
                                    If ddlSalesman.Value <> "[Select]" Then
                                        myCommand.Parameters.Add(New SqlParameter("@open_sales_code", SqlDbType.VarChar, 10)).Value = ddlSalesman.Items(ddlSalesman.SelectedIndex).Text
                                    Else
                                        'spersoncode = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"),"agentmast", "spersoncode", "agentcode", strAccCode)
                                        myCommand.Parameters.Add(New SqlParameter("@open_sales_code", SqlDbType.VarChar, 10)).Value = DBNull.Value
                                    End If
                                Else
                                    myCommand.Parameters.Add(New SqlParameter("@open_sales_code", SqlDbType.VarChar, 10)).Value = DBNull.Value
                                End If
                                myCommand.Parameters.Add(New SqlParameter("@open_debit", SqlDbType.Money)).Value = DecRound(CType(collectionDate("Debit" & strLineKey), Decimal))
                                myCommand.Parameters.Add(New SqlParameter("@open_credit", SqlDbType.Money)).Value = DecRound(CType(collectionDate("Credit" & strLineKey), Decimal))

                                myCommand.Parameters.Add(New SqlParameter("@open_field1", SqlDbType.VarChar, 100)).Value = collectionDate("RefNo" & strLineKey).ToString
                                myCommand.Parameters.Add(New SqlParameter("@open_field2", SqlDbType.VarChar, 100)).Value = collectionDate("Field2" & strLineKey).ToString
                                myCommand.Parameters.Add(New SqlParameter("@open_field3", SqlDbType.VarChar, 100)).Value = collectionDate("Field3" & strLineKey).ToString
                                myCommand.Parameters.Add(New SqlParameter("@open_field4", SqlDbType.VarChar, 100)).Value = collectionDate("Field4" & strLineKey).ToString
                                myCommand.Parameters.Add(New SqlParameter("@open_field5", SqlDbType.VarChar, 100)).Value = collectionDate("Field5" & strLineKey).ToString
                                myCommand.Parameters.Add(New SqlParameter("@open_mode", SqlDbType.Char, 1)).Value = collectionDate("OpenMode" & strLineKey).ToString
                                myCommand.Parameters.Add(New SqlParameter("@open_exchg_diff", SqlDbType.Money)).Value = 0
                                myCommand.Parameters.Add(New SqlParameter("@dr_cr", SqlDbType.Char, 1)).Value = DBNull.Value
                                myCommand.Parameters.Add(New SqlParameter("@div_id", SqlDbType.VarChar, 10)).Value = CType(ViewState("divcode"), String)
                                myCommand.Parameters.Add(New SqlParameter("@currency_rate", SqlDbType.Decimal, 18, 12)).Value = CType(collectionDate("CurrRate" & strLineKey), Decimal)
                                myCommand.Parameters.Add(New SqlParameter("@base_debit", SqlDbType.Money)).Value = DecRound(CType(collectionDate("BaseDebit" & strLineKey), Decimal))
                                myCommand.Parameters.Add(New SqlParameter("@base_credit", SqlDbType.Money)).Value = DecRound(CType(collectionDate("BaseCredit" & strLineKey), Decimal))
                                myCommand.Parameters.Add(New SqlParameter("@acc_type", SqlDbType.Char, 1)).Value = collectionDate("AccType" & strLineKey).ToString
                                myCommand.Parameters.Add(New SqlParameter("@acc_code", SqlDbType.VarChar, 20)).Value = collectionDate("AccCode" & strLineKey).ToString
                                myCommand.Parameters.Add(New SqlParameter("@acc_gl_code", SqlDbType.VarChar, 20)).Value = collectionDate("AccGLCode" & strLineKey).ToString

                                myCommand.ExecuteNonQuery()
                            End If
                        End If
                    Next
                End If
            Next
        End If
        ' End If
    End Sub
#End Region
    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Try
            Dim strdiv, strcostcentercode As String


            If Page.IsValid = True Then
                If ViewState("DebitNoteState") = "Edit" Or ViewState("DebitNoteState") = "Delete" Or ViewState("DebitNoteState") = "Cancel" Or ViewState("DebitNoteState") = "undoCancel" Then
                    If validate_BillAgainst() = False Then
                        Exit Sub
                    End If
                End If
                If ViewState("DebitNoteState") = "New" Or ViewState("DebitNoteState") = "Edit" Or ViewState("DebitNoteState") = "Copy" Or ViewState("DebitNoteState") = "undoCancel" Then
                    If ValidateGrids() = False Then
                        Exit Sub
                    End If

                    If ViewState("DebitNoteState") = "New" Or ViewState("DebitNoteState") = "Edit" Or ViewState("DebitNoteState") = "Delete" Or ViewState("DebitNoteState") = "Cancel" Or ViewState("DebitNoteState") = "undoCancel" Then
                        If Validateseal() = False Then
                            Exit Sub
                        End If
                    End If


                End If

                strdiv = ViewState("divcode") 'objUtils.GetDBFieldFromLongnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", 511)
                strcostcentercode = objUtils.GetDBFieldFromLongnew(Session("dbconnectionName"), "reservation_parameters", "option_Selected", "param_id", 510)
                SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                sqlTrans = SqlConn.BeginTransaction           'SQL  Trans start
                If chkPost.Checked = True Then
                    'For Accounts posting
                    initialclass(SqlConn, sqlTrans)
                    'For Accounts posting
                End If

                If ViewState("DebitNoteState") = "New" Or ViewState("DebitNoteState") = "Edit" Or ViewState("DebitNoteState") = "Copy" Then
                    If ViewState("DebitNoteState") = "New" Or ViewState("DebitNoteState") = "Copy" Then
                        Dim optionval As String
                        txtDocNo.Text = ""
                        optionval = objUtils.GetAutoDocNodiv(CType(ViewState("CNDNOpen_type"), String), SqlConn, sqlTrans, ViewState("divcode"))
                        txtDocNo.Text = optionval.Trim
                        myCommand = New SqlCommand("sp_add_trdpurchase_master", SqlConn, sqlTrans)
                        myCommand.CommandType = CommandType.StoredProcedure
                    ElseIf ViewState("DebitNoteState") = "Edit" Then
                        myCommand = New SqlCommand("sp_mod_trdpurchase_master", SqlConn, sqlTrans)
                        myCommand.CommandType = CommandType.StoredProcedure
                    End If

                    myCommand.Parameters.Add(New SqlParameter("@div_code", SqlDbType.VarChar, 10)).Value = CType(ViewState("divcode"), String)
                    myCommand.Parameters.Add(New SqlParameter("@tran_id", SqlDbType.VarChar, 20)).Value = CType(txtDocNo.Text.Trim, String)
                    myCommand.Parameters.Add(New SqlParameter("@tran_type", SqlDbType.VarChar, 10)).Value = CType(ViewState("CNDNOpen_type"), String)
                    If txtDate.Text = "" Then
                        myCommand.Parameters.Add(New SqlParameter("@tran_date", SqlDbType.DateTime)).Value = DBNull.Value
                    Else
                        myCommand.Parameters.Add(New SqlParameter("@tran_date", SqlDbType.DateTime)).Value = CType(Format(CType(txtDate.Text, Date), "yyyy/MM/dd"), String)
                    End If
                    myCommand.Parameters.Add(New SqlParameter("@supref", SqlDbType.VarChar, 20)).Value = CType(txtReferenceNo.Text.Trim, String)
                    myCommand.Parameters.Add(New SqlParameter("@othref", SqlDbType.VarChar, 20)).Value = DBNull.Value
                    If txtDueDate.Text = "" Then
                        myCommand.Parameters.Add(New SqlParameter("@due_date", SqlDbType.DateTime)).Value = DBNull.Value
                    Else
                        myCommand.Parameters.Add(New SqlParameter("@due_date", SqlDbType.DateTime)).Value = CType(Format(CType(txtDueDate.Text, Date), "yyyy/MM/dd"), String)
                    End If
                    If ddlType.Value <> "[Select]" Then
                        myCommand.Parameters.Add(New SqlParameter("@acc_type", SqlDbType.VarChar, 1)).Value = ddlType.Value
                    Else
                        myCommand.Parameters.Add(New SqlParameter("@acc_type", SqlDbType.VarChar, 1)).Value = DBNull.Value
                    End If
                    If ddlCustomer.Value <> "[Select]" Then
                        myCommand.Parameters.Add(New SqlParameter("@supcode", SqlDbType.VarChar, 20)).Value = CType(ddlCustomer.Items(ddlCustomer.SelectedIndex).Text, String)
                    Else
                        myCommand.Parameters.Add(New SqlParameter("@supcode", SqlDbType.VarChar, 20)).Value = DBNull.Value
                    End If
                    myCommand.Parameters.Add(New SqlParameter("@currcode", SqlDbType.VarChar, 20)).Value = CType(txtCurrency.Value, String)
                    myCommand.Parameters.Add(New SqlParameter("@convrate", SqlDbType.Decimal, 18, 12)).Value = CType(Val(txtConversion.Value), Decimal)
                    myCommand.Parameters.Add(New SqlParameter("@total", SqlDbType.Money)).Value = CType(Val(txtCurrTotal.Value), Double)
                    myCommand.Parameters.Add(New SqlParameter("@basetotal", SqlDbType.Money)).Value = CType(Val(txtKWDTotal.Value), Double)
                    myCommand.Parameters.Add(New SqlParameter("@remarks", SqlDbType.VarChar, 500)).Value = CType(txtNarration.Value.Trim, String)
                    myCommand.Parameters.Add(New SqlParameter("@paymentno", SqlDbType.VarChar, 20)).Value = ""
                    myCommand.Parameters.Add(New SqlParameter("@paymenttype", SqlDbType.VarChar, 10)).Value = ""
                    myCommand.Parameters.Add(New SqlParameter("@tran_state", SqlDbType.VarChar, 1)).Value = "U"
                    myCommand.Parameters.Add(New SqlParameter("@adddate", SqlDbType.DateTime)).Value = ObjDate.GetSystemDateTime(Session("dbconnectionName"))
                    myCommand.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                    If ddlSalesman.Value <> "[Select]" Then
                        myCommand.Parameters.Add(New SqlParameter("@sperson", SqlDbType.VarChar, 10)).Value = CType(ddlSalesman.Items(ddlSalesman.SelectedIndex).Text, String)
                    Else
                        myCommand.Parameters.Add(New SqlParameter("@sperson", SqlDbType.VarChar, 10)).Value = DBNull.Value
                    End If
                    If ddlConAccCode.Value <> "[Select]" Then
                        myCommand.Parameters.Add(New SqlParameter("@trd_gl_code", SqlDbType.VarChar, 10)).Value = CType(ddlConAccCode.Items(ddlConAccCode.SelectedIndex).Text, String)
                    Else
                        myCommand.Parameters.Add(New SqlParameter("@trd_gl_code", SqlDbType.VarChar, 10)).Value = DBNull.Value
                    End If
                    If chkPost.Checked = True Then
                        myCommand.Parameters.Add(New SqlParameter("@post_state", SqlDbType.VarChar, 1)).Value = "P"
                    Else
                        myCommand.Parameters.Add(New SqlParameter("@post_state", SqlDbType.VarChar, 1)).Value = "U"
                    End If
                    If ddlSMktCode.Value <> "[Select]" Then
                        myCommand.Parameters.Add(New SqlParameter("@plgrpcode", SqlDbType.VarChar, 20)).Value = ddlSMktCode.Items(ddlSMktCode.SelectedIndex).Text
                    Else
                        myCommand.Parameters.Add(New SqlParameter("@plgrpcode", SqlDbType.VarChar, 20)).Value = DBNull.Value
                    End If

                    myCommand.ExecuteNonQuery()

                    If ViewState("DebitNoteState") = "Edit" Then
                        If chkPost.Checked = False Then
                            myCommand = New SqlCommand("sp_delvoucher", SqlConn, sqlTrans)
                            myCommand.CommandType = CommandType.StoredProcedure
                            myCommand.Parameters.Add(New SqlParameter("@tablename", SqlDbType.VarChar, 20)).Value = "trdpurchase_master"
                            myCommand.Parameters.Add(New SqlParameter("@tranid", SqlDbType.VarChar, 20)).Value = txtDocNo.Text
                            myCommand.Parameters.Add(New SqlParameter("@trantype ", SqlDbType.VarChar, 10)).Value = CType(ViewState("CNDNOpen_type"), String)
                            myCommand.Parameters.Add(New SqlParameter("@div_id", SqlDbType.VarChar, 10)).Value = CType(ViewState("divcode"), String)
                            myCommand.ExecuteNonQuery()
                        End If


                        myCommand = New SqlCommand("sp_del_open_detail_new", SqlConn, sqlTrans)
                        myCommand.CommandType = CommandType.StoredProcedure
                        myCommand.Parameters.Add(New SqlParameter("@div_id", SqlDbType.VarChar, 10)).Value = CType(ViewState("divcode"), String)
                        myCommand.Parameters.Add(New SqlParameter("@tran_id", SqlDbType.VarChar, 20)).Value = CType(txtDocNo.Text.Trim, String)
                        myCommand.Parameters.Add(New SqlParameter("@tran_type ", SqlDbType.VarChar, 10)).Value = CType(ViewState("CNDNOpen_type"), String)
                        myCommand.ExecuteNonQuery()

                        myCommand = New SqlCommand("sp_del_trdpurchase_other", SqlConn, sqlTrans)
                        myCommand.CommandType = CommandType.StoredProcedure
                        myCommand.Parameters.Add(New SqlParameter("@tran_id", SqlDbType.VarChar, 20)).Value = CType(txtDocNo.Text.Trim, String)
                        myCommand.Parameters.Add(New SqlParameter("@tran_type", SqlDbType.VarChar, 10)).Value = CType(ViewState("CNDNOpen_type"), String)
                        myCommand.Parameters.Add(New SqlParameter("@div_id", SqlDbType.VarChar, 10)).Value = CType(ViewState("divcode"), String)
                        myCommand.ExecuteNonQuery()
                    End If
                    If CheckAdjustBill() = True Then
                        Save_Open_detail(1, ddlCustomer.Items(ddlCustomer.SelectedIndex).Text, ddlType.Value, ddlConAccCode.Items(ddlConAccCode.SelectedIndex).Text, SqlConn, sqlTrans)
                    End If
                    If chkPost.Checked = True Then
                        'For Accounts Posting
                        caccounts.clraccounts()
                        cacc.acc_tran_id = txtDocNo.Text
                        cacc.acc_tran_type = CType(ViewState("CNDNOpen_type"), String)
                        cacc.acc_tran_date = Format(CType(txtDate.Text, Date), "yyyy/MM/dd")
                        cacc.acc_div_id = strdiv

                        'Posting for the Header Level
                        ctran = New clstran
                        ctran.acc_tran_id = cacc.acc_tran_id
                        ctran.acc_code = ddlCustomer.Items(ddlCustomer.SelectedIndex).Text
                        ctran.acc_type = ddlType.Value ' "G"
                        ctran.acc_currency_id = txtCurrency.Value
                        ctran.acc_currency_rate = CType(Val(txtConversion.Value), Decimal)
                        ctran.acc_div_id = strdiv
                        ctran.acc_narration = txtNarration.Value
                        ctran.acc_tran_date = cacc.acc_tran_date
                        ctran.acc_tran_lineno = 1 ' 0
                        ctran.acc_tran_type = cacc.acc_tran_type
                        If ddlConAccCode.Value <> "[Select]" Then
                            ctran.pacc_gl_code = ddlConAccCode.Items(ddlConAccCode.SelectedIndex).Text
                        Else
                            ctran.pacc_gl_code = ""
                        End If
                        ctran.acc_ref1 = ""
                        ctran.acc_ref2 = ""
                        ctran.acc_ref3 = ""
                        ctran.acc_ref4 = ""
                        cacc.addtran(ctran)
                        If CheckAdjustBill() = False Then
                            'without adjustbill only posting
                            csubtran = New clsSubTran
                            csubtran.acc_against_tran_id = cacc.acc_tran_id
                            csubtran.acc_against_tran_lineno = 1
                            csubtran.acc_against_tran_type = cacc.acc_tran_type

                            If CType(ViewState("CNDNOpen_type"), String) = "DN" Then
                                csubtran.acc_debit = DecRound(CType(txtCurrTotal.Value, Decimal))
                                csubtran.acc_credit = 0
                                'csubtran.acc_base_debit = DecRound(CType(txtKWDTotal.Value, Decimal))
                                'csubtran.acc_base_credit = 0
                            Else
                                csubtran.acc_credit = DecRound(CType(txtCurrTotal.Value, Decimal))
                                csubtran.acc_debit = 0
                                'csubtran.acc_base_credit = DecRound(CType(txtKWDTotal.Value, Decimal))
                                'csubtran.acc_base_debit = 0
                            End If
                            csubtran.acc_tran_date = cacc.acc_tran_date
                            csubtran.acc_due_date = cacc.acc_tran_date
                            csubtran.acc_field1 = ""
                            csubtran.acc_field2 = ""
                            csubtran.acc_field3 = ""
                            csubtran.acc_field4 = ""
                            csubtran.acc_field5 = ""
                            csubtran.acc_tran_id = cacc.acc_tran_id
                            csubtran.acc_tran_lineno = 1
                            csubtran.acc_tran_type = cacc.acc_tran_type
                            csubtran.acc_narration = txtNarration.Value
                            csubtran.acc_type = ddlType.Value        '"G"
                            csubtran.currate = CType(txtConversion.Value, Decimal)
                            If CType(ViewState("CNDNOpen_type"), String) = "DN" Then
                                csubtran.acc_base_debit = DecRound(CType(txtKWDTotal.Value, Decimal))
                                csubtran.acc_base_credit = 0
                            Else
                                csubtran.acc_base_credit = DecRound(CType(txtKWDTotal.Value, Decimal))
                                csubtran.acc_base_debit = 0
                            End If
                            csubtran.costcentercode = ""
                            cacc.addsubtran(csubtran)
                        End If
                    End If
                    '----------------------------------- Inserting Data To DebitNote Details Table
                    Dim i As Integer = 1
                    Dim ddlAccCode1 As HtmlSelect
                    Dim ddlAccName1 As HtmlSelect
                    Dim ddlCCCode1 As HtmlSelect
                    Dim ddlCCName1 As HtmlSelect

                    Dim txtPartic1, txtCValue1, txtKWValue1 As TextBox
                    Dim lbl As Label
                    For Each gvRow In grdDebitNote.Rows
                        lbl = gvRow.FindControl("lblLineID")
                        ddlAccName1 = gvRow.FindControl("ddlAccountName")
                        ddlCCCode1 = gvRow.FindControl("ddlCostCode")
                        ddlCCName1 = gvRow.FindControl("ddlCostName")
                        txtPartic1 = gvRow.FindControl("txtParticulars")
                        txtCValue1 = gvRow.FindControl("txtCurrValue")
                        txtKWValue1 = gvRow.FindControl("txtKWDValue")
                        ddlAccCode1 = gvRow.FindControl("ddlAccountCode")

                        If ddlAccCode1.Value <> "[Select]" Then
                            i = i + 1
                            myCommand = New SqlCommand("sp_add_trdpurchase_other", SqlConn, sqlTrans)
                            myCommand.CommandType = CommandType.StoredProcedure
                            myCommand.Parameters.Add(New SqlParameter("@div_code", SqlDbType.VarChar, 10)).Value = CType(ViewState("divcode"), String)
                            myCommand.Parameters.Add(New SqlParameter("@tran_id", SqlDbType.VarChar, 20)).Value = CType(txtDocNo.Text.Trim, String)
                            myCommand.Parameters.Add(New SqlParameter("@tran_type", SqlDbType.VarChar, 10)).Value = CType(ViewState("CNDNOpen_type"), String)
                            myCommand.Parameters.Add(New SqlParameter("@sno", SqlDbType.Int, 9)).Value = i
                            myCommand.Parameters.Add(New SqlParameter("@acc_code", SqlDbType.VarChar, 20)).Value = CType(ddlAccCode1.Items(ddlAccCode1.SelectedIndex).Text, String)
                            If ddlCCCode1.Value <> "[Select]" Then
                                myCommand.Parameters.Add(New SqlParameter("@costcenter_code", SqlDbType.VarChar, 20)).Value = CType(ddlCCCode1.Items(ddlCCCode1.SelectedIndex).Text, String)
                            Else
                                myCommand.Parameters.Add(New SqlParameter("@costcenter_code", SqlDbType.VarChar, 20)).Value = "Gen" ' DBNull.Value 'strcostcentercode
                            End If
                            myCommand.Parameters.Add(New SqlParameter("@particulars", SqlDbType.VarChar, 1000)).Value = CType(txtPartic1.Text.Trim, String)
                            myCommand.Parameters.Add(New SqlParameter("@amount", SqlDbType.Money)).Value = CType(Val(txtCValue1.Text), Double)
                            myCommand.Parameters.Add(New SqlParameter("@baseamount", SqlDbType.Money)).Value = CType(Val(txtKWValue1.Text), Double)
                            myCommand.ExecuteNonQuery()
                            If chkPost.Checked = True Then
                                'Detail Level Against Posting
                                ctran = New clstran
                                ctran.acc_tran_id = cacc.acc_tran_id
                                ctran.acc_code = CType(ddlAccCode1.Items(ddlAccCode1.SelectedIndex).Text, String)
                                ctran.acc_type = "G"
                                ctran.acc_currency_id = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", 457)
                                ctran.acc_currency_rate = 1
                                ctran.acc_div_id = strdiv
                                ctran.acc_narration = txtNarration.Value
                                ctran.acc_tran_date = cacc.acc_tran_date
                                ctran.acc_tran_lineno = i
                                ctran.acc_tran_type = cacc.acc_tran_type
                                ctran.pacc_gl_code = ""
                                ctran.acc_ref1 = ""
                                ctran.acc_ref2 = ""
                                ctran.acc_ref3 = ""
                                ctran.acc_ref4 = ""
                                cacc.addtran(ctran)

                                csubtran = New clsSubTran
                                csubtran.acc_against_tran_id = cacc.acc_tran_id
                                csubtran.acc_against_tran_lineno = i
                                csubtran.acc_against_tran_type = cacc.acc_tran_type
                                If CType(ViewState("CNDNOpen_type"), String) = "DN" Then
                                    csubtran.acc_credit = DecRound(IIf(CType(txtKWValue1.Text, Decimal) > 0, CType(txtKWValue1.Text, Decimal), 0))
                                    csubtran.acc_debit = DecRound(IIf(CType(txtKWValue1.Text, Decimal) < 0, Math.Abs(CType(txtKWValue1.Text, Decimal)), 0))
                                    csubtran.acc_base_credit = DecRound(IIf(CType(txtKWValue1.Text, Decimal) > 0, CType(txtKWValue1.Text, Decimal), 0))
                                    csubtran.acc_base_debit = DecRound(IIf(CType(txtKWValue1.Text, Decimal) < 0, Math.Abs(CType(txtKWValue1.Text, Decimal)), 0))
                                Else
                                    csubtran.acc_credit = DecRound(IIf(CType(txtKWValue1.Text, Decimal) < 0, Math.Abs(CType(txtKWValue1.Text, Decimal)), 0))
                                    csubtran.acc_debit = DecRound(IIf(CType(txtKWValue1.Text, Decimal) > 0, CType(txtKWValue1.Text, Decimal), 0))
                                    csubtran.acc_base_credit = DecRound(IIf(CType(txtKWValue1.Text, Decimal) < 0, Math.Abs(CType(txtKWValue1.Text, Decimal)), 0))
                                    csubtran.acc_base_debit = DecRound(IIf(CType(txtKWValue1.Text, Decimal) > 0, CType(txtKWValue1.Text, Decimal), 0))
                                End If
                                csubtran.acc_tran_date = cacc.acc_tran_date
                                csubtran.acc_due_date = cacc.acc_tran_date
                                csubtran.acc_field1 = ""
                                csubtran.acc_field2 = ""
                                csubtran.acc_field3 = ""
                                csubtran.acc_field4 = ""
                                csubtran.acc_field5 = ""
                                csubtran.acc_tran_id = cacc.acc_tran_id
                                csubtran.acc_tran_lineno = i
                                csubtran.acc_tran_type = cacc.acc_tran_type
                                csubtran.acc_narration = txtPartic1.Text
                                csubtran.acc_type = "G"
                                csubtran.currate = 1
                                'if it is blank then post to default 510
                                If ddlCCCode1.Value <> "[Select]" Then
                                    csubtran.costcentercode = ddlCCCode1.Items(ddlCCCode1.SelectedIndex).Text
                                Else
                                    csubtran.costcentercode = strcostcentercode
                                End If
                                cacc.addsubtran(csubtran)
                            End If

                        End If

                    Next
                    If chkPost.Checked = True Then
                        'For Accounts Posting
                        cacc.table_name = ""
                        caccounts.Addaccounts(cacc)
                        If caccounts.saveaccounts(Session("dbconnectionName"), SqlConn, sqlTrans, Me.Page) <> 0 Then
                            Err.Raise(vbObjectError + 100)
                        End If
                        'For Accounts Posting
                        lblPostmsg.Text = "Posted"
                        lblPostmsg.ForeColor = Drawing.Color.Red
                    Else
                        lblPostmsg.Text = "UnPosted"
                        lblPostmsg.ForeColor = Drawing.Color.Green
                    End If
                ElseIf ViewState("DebitNoteState") = "Delete" Then

                    myCommand = New SqlCommand("sp_delvoucher", SqlConn, sqlTrans)
                    myCommand.CommandType = CommandType.StoredProcedure
                    myCommand.Parameters.Add(New SqlParameter("@tablename", SqlDbType.VarChar, 20)).Value = "trdpurchase_master"
                    myCommand.Parameters.Add(New SqlParameter("@tranid", SqlDbType.VarChar, 20)).Value = txtDocNo.Text
                    myCommand.Parameters.Add(New SqlParameter("@trantype ", SqlDbType.VarChar, 10)).Value = CType(ViewState("CNDNOpen_type"), String)
                    myCommand.Parameters.Add(New SqlParameter("@div_id", SqlDbType.VarChar, 10)).Value = CType(ViewState("divcode"), String)
                    myCommand.ExecuteNonQuery()

                    myCommand = New SqlCommand("sp_del_open_detail_new", SqlConn, sqlTrans)
                    myCommand.CommandType = CommandType.StoredProcedure
                    myCommand.Parameters.Add(New SqlParameter("@div_id", SqlDbType.VarChar, 10)).Value = CType(ViewState("divcode"), String)
                    myCommand.Parameters.Add(New SqlParameter("@tran_id", SqlDbType.VarChar, 20)).Value = txtDocNo.Text
                    myCommand.Parameters.Add(New SqlParameter("@tran_type ", SqlDbType.VarChar, 10)).Value = CType(ViewState("CNDNOpen_type"), String)
                    myCommand.ExecuteNonQuery()


                    myCommand = New SqlCommand("sp_del_trdpurchase", SqlConn, sqlTrans)
                    myCommand.CommandType = CommandType.StoredProcedure
                    myCommand.Parameters.Add(New SqlParameter("@tran_id", SqlDbType.VarChar, 20)).Value = CType(txtDocNo.Text.Trim, String)
                    myCommand.Parameters.Add(New SqlParameter("@tran_type", SqlDbType.VarChar, 10)).Value = CType(ViewState("CNDNOpen_type"), String)
                    myCommand.Parameters.Add(New SqlParameter("@div_id", SqlDbType.VarChar, 10)).Value = CType(ViewState("divcode"), String)

                    myCommand.ExecuteNonQuery()
                ElseIf ViewState("DebitNoteState") = "Cancel" Then

                    myCommand = New SqlCommand("sp_delvoucher", SqlConn, sqlTrans)
                    myCommand.CommandType = CommandType.StoredProcedure
                    myCommand.Parameters.Add(New SqlParameter("@tablename", SqlDbType.VarChar, 20)).Value = "trdpurchase_master"
                    myCommand.Parameters.Add(New SqlParameter("@tranid", SqlDbType.VarChar, 20)).Value = txtDocNo.Text
                    myCommand.Parameters.Add(New SqlParameter("@trantype ", SqlDbType.VarChar, 10)).Value = CType(ViewState("CNDNOpen_type"), String)
                    myCommand.Parameters.Add(New SqlParameter("@div_id", SqlDbType.VarChar, 10)).Value = CType(ViewState("divcode"), String)
                    myCommand.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                    myCommand.ExecuteNonQuery()

                    myCommand = New SqlCommand("sp_cancel_trdpurchase_open_detail_new", SqlConn, sqlTrans)
                    myCommand.CommandType = CommandType.StoredProcedure
                    myCommand.Parameters.Add(New SqlParameter("@tran_id", SqlDbType.VarChar, 20)).Value = txtDocNo.Text
                    myCommand.Parameters.Add(New SqlParameter("@tran_type ", SqlDbType.VarChar, 10)).Value = CType(ViewState("CNDNOpen_type"), String)
                    myCommand.Parameters.Add(New SqlParameter("@div_id", SqlDbType.VarChar, 10)).Value = CType(ViewState("divcode"), String)
                    myCommand.ExecuteNonQuery()


                    myCommand = New SqlCommand("sp_cancel_trdpurchase_master", SqlConn, sqlTrans)
                    myCommand.CommandType = CommandType.StoredProcedure
                    myCommand.Parameters.Add(New SqlParameter("@div_id", SqlDbType.VarChar, 10)).Value = CType(ViewState("divcode"), String)
                    myCommand.Parameters.Add(New SqlParameter("@tran_id", SqlDbType.VarChar, 20)).Value = CType(txtDocNo.Text.Trim, String)
                    myCommand.Parameters.Add(New SqlParameter("@tran_type", SqlDbType.VarChar, 10)).Value = CType(ViewState("CNDNOpen_type"), String)
                    myCommand.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                    myCommand.ExecuteNonQuery()

                ElseIf ViewState("DebitNoteState") = "undoCancel" Then
                    myCommand = New SqlCommand("sp_undocancel_trdpurchase", SqlConn, sqlTrans)
                    myCommand.CommandType = CommandType.StoredProcedure
                    myCommand.Parameters.Add(New SqlParameter("@div_id", SqlDbType.VarChar, 10)).Value = CType(ViewState("divcode"), String)
                    myCommand.Parameters.Add(New SqlParameter("@tran_id", SqlDbType.VarChar, 20)).Value = CType(txtDocNo.Text.Trim, String)
                    myCommand.Parameters.Add(New SqlParameter("@tran_type ", SqlDbType.VarChar, 10)).Value = CType(ViewState("CNDNOpen_type"), String)
                    myCommand.ExecuteNonQuery()


                End If

                sqlTrans.Commit()    'SQl Tarn Commit
                clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
                clsDBConnect.dbCommandClose(myCommand)               'sql command disposed
                clsDBConnect.dbConnectionClose(SqlConn)           'connection close

                If ViewState("DebitNoteState") = "Delete" Or ViewState("DebitNoteState") = "Cancel" Or ViewState("DebitNoteState") = "undoCancel" Then
                    'Response.Redirect("DebitNoteSearch.aspx?tran_type=" & CType(Session("CNDNOpen_type"), String) & "", False)
                    'Response.Redirect("~\AccountsModule\DebitNoteSearch.aspx?tran_type=" & CType(ViewState("CNDNOpen_type"), String) & "", False)
                    Dim strscript As String = ""
                    strscript = "window.opener.__doPostBack('DebitNoteWindowPostBack', '');window.opener.focus();window.close();"
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)
                Else
                    If ViewState("DebitNoteState") = "New" Or ViewState("DebitNoteState") = "Copy" Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Record save successfully');", True)
                    ElseIf ViewState("DebitNoteState") = "Edit" Then
                        'ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Record update successfully');", True)
                        Dim strURL As String = ""
                        '
                        strURL = "Accnt_trn_amendlog.aspx?tid=" & txtDocNo.Text.Trim & "&ttype=" & ViewState("CNDNOpen_type").ToString & "&tdate=" & txtDate.Text.Trim
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", String.Format(ScriptOpenModalDialog, strURL, 300), True)
                    End If
                    btnPrint.Visible = True
                    ViewState("DebitNoteState") = "View"
                    DisableControl()
                    'btnPrint_Click(sender, e)
                    btnAdjustBill.Visible = False
                End If

                Session.Remove("Collection" & ":" & txtAdjcolno.Value)
            End If
        Catch ex As Exception
            If SqlConn.State = ConnectionState.Open Then
                sqlTrans.Rollback()
            End If
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("DebitNote.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try

    End Sub
#Region "Public Function checkForDuplicate() As Boolean"
    Public Function checkForDuplicate() As Boolean
        If ViewState("DebitNoteState") = "New" Or ViewState("DebitNoteState") = "Copy" Then
            If objUtils.isDuplicatenewdiv(Session("dbconnectionName"), "trdpurchase_master", "tran_id", CType(txtDocNo.Text.Trim, String), "div_id", ViewState("divcode")) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This record is already present.');", True)
                checkForDuplicate = False
                Exit Function
            End If
            'ElseIf Session("State") = "Edit" Then
            '    If objUtils.isDuplicateForModifynew(Session("dbconnectionName"),"trdpurchase_master", "pkgname", "tran_id", CType(txtPackageName.Text.Trim, String), CType(txtDocNo.Text.Trim, String)) Then
            '        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Package name is already present.');", True)
            '        checkForDuplicate = False
            '        Exit Function
            '    End If
        End If
        checkForDuplicate = True
    End Function
#End Region
    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        'Response.Redirect("DebitNoteSearch.aspx?tran_type=" & CType(Session("CNDNOpen_type"), String) & "", False)
        ' Response.Redirect("~\AccountsModule\DebitNoteSearch.aspx?tran_type=" & CType(ViewState("CNDNOpen_type"), String) & "", False)
        Session.Remove("Collection" & ":" & txtAdjcolno.Value)
        Dim strscript As String = ""
        strscript = "window.opener.__doPostBack('DebitNoteWindowPostBack', '');window.opener.focus();window.close();"
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)
    End Sub
    'Protected Sub grdDebitNote_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdDebitNote.RowDataBound
    '    'Try

    '    '    Dim txtCValue As TextBox
    '    '    Dim txtKWValue As TextBox

    '    '    Dim str, str1, str2 As String

    '    '    Dim strctl As String = Nothing
    '    '    Dim strctl1 As String = Nothing

    '    '    Dim GVrow As GridViewRow
    '    '    Dim ctl() As String = Nothing
    '    '    Dim lngcnt As Long = 0
    '    '    For Each GVrow In grdDebitNote.Rows
    '    '        txtCValue = GVrow.FindControl("txtCurrValue")
    '    '        txtKWValue = GVrow.FindControl("txtKWDValue")
    '    '        If strctl = "" Then
    '    '            strctl = txtCValue.UniqueID
    '    '        Else
    '    '            strctl = strctl & "/" & txtCValue.UniqueID
    '    '        End If
    '    '        If strctl1 = "" Then
    '    '            strctl1 = txtKWValue.UniqueID
    '    '        Else
    '    '            strctl1 = strctl1 & "/" & txtKWValue.UniqueID
    '    '        End If
    '    '        lngcnt = lngcnt + 1
    '    '    Next
    '    '    'For Each GVrow In grdDebitNote.Rows
    '    '    '    txtCValue = GVrow.FindControl("txtCurrValue")
    '    '    '    txtKWValue = GVrow.FindControl("txtKWDValue")
    '    '    '    str = "javascript:calculate_total('" & txtCurrTotal.UniqueID & "','" & strctl & "'," & lngcnt & ");"
    '    '    '    txtCValue.Attributes.Add("onblur", str)
    '    '    '    str1 = "javascript:calculate_total('" & txtKWDTotal.UniqueID & "','" & strctl1 & "'," & lngcnt & ");"
    '    '    '    txtKWValue.Attributes.Add("onblur", str1)
    '    '    '    'str2 = "javascript:calculate_total('" & txtKWDTotal.UniqueID & "','" & strctl1 & "'," & lngcnt & ");"
    '    '    '    'txtConversion.Attributes.Add("onblur", str2)
    '    '    'Next

    '    '    For Each GVrow In grdDebitNote.Rows
    '    '        txtCValue = GVrow.FindControl("txtCurrValue")
    '    '        txtKWValue = GVrow.FindControl("txtKWDValue")

    '    '        str = "javascript:cal_total('" & txtCurrTotal.UniqueID & "','" & strctl & "'," & lngcnt & ",'" & txtKWDTotal.UniqueID & "','" & strctl1 & "');"
    '    '        txtCValue.Attributes.Add("onchange", str)
    '    '        'str1 = "javascript:calculate_total(," & lngcnt & ");"
    '    '        'txtKWValue.Attributes.Add("onblur", str1)

    '    '    Next

    '    'Catch ex As Exception
    '    '    objUtils.WritErrorLog("DebitNote.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
    '    'End Try

    'End Sub
    Protected Sub btnPost_Click(ByVal sender As Object, ByVal e As System.EventArgs)

    End Sub
    Protected Sub btnUnpost_Click(ByVal sender As Object, ByVal e As System.EventArgs)
    End Sub
    Protected Sub btnAdjustBill_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim pass, tran_id, lineno, grdtype, intRows, OLineNo As String
        If ViewState("DebitNoteState") = "Edit" Or ViewState("DebitNoteState") = "Delete" Or ViewState("DebitNoteState") = "View" Or ViewState("DebitNoteState") = "Cancel" Or ViewState("DebitNoteState") = "undoCancel" Then
            tran_id = txtDocNo.Text
            lineno = 1
        Else
            tran_id = ""
            lineno = 1
        End If
        If ViewState("CNDNOpen_type") = "DN" Then
            grdtype = "Debit"
        Else
            grdtype = "Credit"
        End If
        intRows = 1
        OLineNo = 1
        pass = "&TranType=" + ViewState("CNDNOpen_type") + "&AccCode=" + ddlCustomer.Items(ddlCustomer.SelectedIndex).Text + "&ControlCode=" + ddlConAccCode.Items(ddlConAccCode.SelectedIndex).Text + "&AccType=" + ddlType.Value + "&TranId=" + tran_id + "&lineNo=" + lineno + "&currcode=" + txtCurrency.Value + "&currrate=" + txtConversion.Value + "&Amount=" + txtCurrTotal.Value + "&BaseAmount=" + txtKWDTotal.Value + "&Gridtype=" + grdtype + "&MainGrdCount=" + intRows + "&OlineNo=" + OLineNo + "&CNDNsperponcode=" + ddlSalesman.Items(ddlSalesman.SelectedIndex).Text + "&State=" + ViewState("DebitNoteState") + "&RefCode=" + ViewState("DebitNoteRefCode") + "&AdjColno=" + txtAdjcolno.Value

        'If ddlSalesman.Value <> "[Select]" Then
        '    Session.Add("CNDNsperponcode", ddlSalesman.Items(ddlSalesman.SelectedIndex).Text)
        'Else
        '    Session.Add("CNDNsperponcode", "")
        'End If

        'Response.Redirect("OtherServicesCostPriceList2.aspx?&SptypeCode=" & SptypeCode & "&SptypeName=" & SptypeName & "&MarketCode=" & MarketCode & "&MarketName=" & MarketName & "&SupplierCode=" & SupplierCode & "&SupplierName=" & SupplierName & "&SupplierAgentCode=" & SupplierAgentCode & "&SupplierAgentName=" & SupplierAgentName & "&PlcCode=" & PlcCode & "&GroupCode=" & GroupCode & "&GroupName=" & GroupName & "&currcode=" & currcode & "&currname=" & currname & "&SubSeasCode=" & SubSeasCode & "&SubSeasName=" & SubSeasName & "&Acvtive=" & IIf(ChkActive.Checked = True, 1, 0) & "&frmdate=" & frmdate & "&todate=" & todate, False)

        ' //window.open('ReceiptsAdjustBills.aspx?' + pass,'ReceiptsAdjustBills');

        'window.open('ReceiptsAdjustBills.aspx?' + pass,'ReceiptsAdjustBills','toolbar=0,scrollbars=yes,resizable=yes,titlebar=0,menubar=0,locationbar=0,modal=1,statusbar=1,fullscreen=yes');
        'return false;

        '}else
        '{
        'alert('Account type ‘G’ doesn’t  adjust bill .'); 
        'return false;

        Dim ScriptStr As String
        ScriptStr = "<script language=""javascript"">var win=window.open('../AccountsModule/ReceiptsAdjustBills.aspx?" + pass + "&divid=" & ViewState("divcode") & "','ReceiptsAdjustBills','toolbar=0,scrollbars=yes,resizable=yes,titlebar=0,menubar=0,locationbar=0,modal=1,statusbar=1,fullscreen=yes');</script>"


        'ScriptStr = "<script language=""javascript"">var win=window.open('../AccountsModule/ReceiptsAdjustBills.aspx','teste');</script>"
        'ScriptStr = "<script language=""javascript"">var win=window.open('../AccountsModule/ReceiptsAdjustBills.aspx');</script>"
        'ScriptStr = "window.open('../AccountsModule/ReceiptsAdjustBills.aspx?" + pass + "','ReceiptsAdjustBills','toolbar=0,scrollbars=yes,resizable=yes,titlebar=0,menubar=0,locationbar=0,modal=1,statusbar=1,fullscreen=yes')"
        'ScriptStr = "window.open('../AccountsModule/ReceiptsAdjustBills.aspx?" + pass + "','ReceiptsAdjustBills','toolbar=0,scrollbars=yes,resizable=yes,titlebar=0,menubar=0,locationbar=0,modal=1,statusbar=1,fullscreen=yes')"
        'ScriptStr = "<script language=""javascript"">var win=window.open('../PriceListModule/PrintDoc.aspx','printdoc','toolbar=0,scrollbars=yes,resizable=yes,titlebar=0,menubar=0,locationbar=0,modal=1,statusbar=1');</script>"
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", ScriptStr, False)


    End Sub
    Protected Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            Dim backPname As String
            'If MsgBox("Do you want to print", MsgBoxStyle.YesNo, "Doc Print") = MsgBoxResult.No Then
            '    Exit Sub
            'End If
            'Dim strReportTitle As String = ""
            'Dim strSelectionFormula As String = ""
            'Session.Add("RefCode", CType(txtDocNo.Text.Trim, String))
            'Session.Add("Pageame", "DebitNoteDoc")
            'Session.Add("BackPageName", "~\AccountsModule\DebitNoteSearch.aspx?tran_type=" & CType(ViewState("CNDNOpen_type"), String) & "")
            backPname = "~\AccountsModule\DebitNoteSearch.aspx?&divid=" & ViewState("divcode") & "&tran_type=" & CType(ViewState("CNDNOpen_type"), String)

           
            Dim ScriptStr As String

            'ScriptStr = "<script language=""javascript"">var win=window.open('rptReportNew.aspx?Pageame=DebitNoteDoc&BackPageName=" & backPname & "&TranId=" & txtDocNo.Text & "&TranType=" & ViewState("CNDNOpen_type") & "','printdoc','toolbar=0,scrollbars=yes,resizable=yes,titlebar=0,menubar=0,locationbar=0,modal=1,statusbar=1');</script>"
            'ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", ScriptStr, False)


            ScriptStr = "<script language=""javascript"">var win=window.open('PrintDocNew.aspx?Pageame=DebitNoteDoc&BackPageName=" & backPname & "&TranId=" & txtDocNo.Text & "&TranType=" & ViewState("CNDNOpen_type") & "','printdoc','toolbar=0,scrollbars=yes,resizable=yes,titlebar=0,menubar=0,locationbar=0,modal=1,statusbar=1');</script>"
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", ScriptStr, False)



            Dim strpop As String = ""
            'strpop = "window.open('rptReportNew.aspx?Pageame=SupplierOpeningTrailBalance&BackPageName=" & BackPage & "&opentype=" & open_type & "&TranID=" & txtTranId.Text.Trim & "&Supplier=" & Trim(ddlSupplierCode.Items(ddlSupplierCode.SelectedIndex).Text) & "&Curr=" & Trim(ddlCurrCode.Items(ddlCurrCode.SelectedIndex).Text) & "&ConvRate=" & txtConvtRate.Text.Trim & " ','OpeningSupBal','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
            'strpop = "window.open('rptReportNew.aspx?Pageame=SupplierOpeningTrailBalance&BackPageName=" & backPname & "&TranId=" & txtDocNo.Text & "&TranType=" & ViewState("CNDNOpen_type") & "','DebitNote');"
            'ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)


            'strpop = "window.open('PrintDocNew.aspx?Pageame=DebitNoteDoc&BackPageName=" & backPname & "&TranId=" & txtDocNo.Text & "&TranType=" & ViewState("CNDNOpen_type") & "','DebitNote');"
            'ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)


      
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("Debitnote.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub


    Public Function check_Privilege() As Integer
        Try
            Dim strSql As String
            Dim usrCode As String
            usrCode = CType(Session("GlobalUserName"), String)
            'mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            strSql = "select appid from group_privilege_Detail where privilegeid='3' and appid='4' and "
            strSql += "groupid=(SELECT groupid FROM UserMaster WHERE UserCode='" + usrCode + "')"
            'mySqlCmd = New SqlCommand(strSql, mySqlConn)
            'mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)

            Dim ds1 As DataSet = objUtils.ExecuteQuerySqlnew(Session("dbconnectionName"), strSql)
            If ds1.Tables.Count > 0 Then
                If ds1.Tables(0).Rows.Count > 0 Then
                    Return 1
                Else
                    Return 0
                End If
            End If


        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("Debitnote.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            'clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
            'clsDBConnect.dbReaderClose(mySqlReader)             ' sql reader disposed    
            'clsDBConnect.dbConnectionClose(mySqlConn)           'connection close 
        End Try
    End Function
    Public Function DecRound(ByVal Ramt As Decimal) As Decimal
        Dim Rdamt As Decimal
        Rdamt = Math.Round(Val(Ramt), CType(txtdecimal.Value, Integer))
        Return Rdamt
    End Function
#Region "Private Function colexists(ByVal newcol As Collection, ByVal newkey As String) As Boolean"
    Private Function colexists(ByVal newcol As Collection, ByVal newkey As String) As Boolean
        Try
            Dim k As Integer
            colexists = False
            If newcol.Count > 0 Then
                For k = 1 To newcol.Count
                    ' If newcol(newkey).ToString <> "" Then
                    If newcol.Contains(newkey) = True Then
                        colexists = True
                        Exit Function
                    End If
                Next
            End If
        Catch ex As Exception
            colexists = False
        End Try
    End Function
#End Region
#Region "Public Sub AddCollection(ByVal dataCollection As Collection, ByVal strKey As String, ByVal strVal As String)"
    Public Sub AddCollection(ByVal dataCollection As Collection, ByVal strKey As String, ByVal strVal As String)
        If colexists(dataCollection, strKey) = False Then
            dataCollection.Add(strVal, strKey, Nothing, Nothing)
        Else
            dataCollection.Remove(strKey)
            dataCollection.Add(strVal, strKey, Nothing, Nothing)
        End If
    End Sub
#End Region
#Region "Public Sub fillcollection(ByVal tranid As String, ByVal lineno As Integer)"
    Public Sub fillcollection(ByVal tranid As String)

        Dim clAdBill As New Collection
        Dim strLineKey As String
        Dim intLineNo As Long = 1
        Dim MainRowct As Long
        Dim MainRowindex As Long
        Dim myDS As New DataSet
        Dim mySqlReader As SqlDataReader
        ' Dim rowbasetotal As Decimal
        MainRowct = 1
        'objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"),"select count(*) from receipt_detail Where   receipt_acc_type<>'G' and tran_id='" & tranid & "' and tran_type='" & CType(Session("RVPVTranType"), String) & "'")
        SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
        myCommand = New SqlCommand("select * from  open_detail Where div_id='" & ViewState("divcode") & "' and against_tran_id='" & tranid & "' and against_tran_type='" & CType(ViewState("CNDNOpen_type"), String) & "' order by against_tran_lineno,tran_lineno ", SqlConn)
        mySqlReader = myCommand.ExecuteReader()
        If mySqlReader.HasRows Then
            While mySqlReader.Read()
                If MainRowindex <> mySqlReader("against_tran_lineno") Then
                    intLineNo = 1
                End If
                strLineKey = intLineNo & ":" & mySqlReader("against_tran_lineno")
                AddCollection(clAdBill, "AgainstTranLineNo" & strLineKey, mySqlReader("against_tran_lineno")) 'intLineNo.ToString)
                AddCollection(clAdBill, "AccTranLineNo" & strLineKey, mySqlReader("tran_lineno"))
                AddCollection(clAdBill, "TranId" & strLineKey, mySqlReader("tran_id"))
                AddCollection(clAdBill, "TranDate" & strLineKey, Format(CType(mySqlReader("tran_date"), Date), "dd/MM/yyyy"))
                AddCollection(clAdBill, "TranType" & strLineKey, mySqlReader("tran_type"))
                AddCollection(clAdBill, "DueDate" & strLineKey, Format(CType(mySqlReader("open_due_date"), Date), "dd/MM/yyyy"))
                AddCollection(clAdBill, "CurrRate" & strLineKey, mySqlReader("currency_rate"))
                AddCollection(clAdBill, "Credit" & strLineKey, DecRound(mySqlReader("open_credit")))
                AddCollection(clAdBill, "Debit" & strLineKey, DecRound(mySqlReader("open_debit")))
                AddCollection(clAdBill, "BaseCredit" & strLineKey, DecRound(mySqlReader("Base_Credit")))
                AddCollection(clAdBill, "BaseDebit" & strLineKey, DecRound(mySqlReader("Base_Debit")))
                AddCollection(clAdBill, "RefNo" & strLineKey, mySqlReader("open_field1"))
                AddCollection(clAdBill, "Field2" & strLineKey, mySqlReader("open_field2"))
                AddCollection(clAdBill, "Field3" & strLineKey, mySqlReader("open_field3"))
                AddCollection(clAdBill, "Field4" & strLineKey, mySqlReader("open_field4"))
                AddCollection(clAdBill, "Field5" & strLineKey, mySqlReader("open_field5"))
                AddCollection(clAdBill, "OpenMode" & strLineKey, mySqlReader("open_mode"))
                AddCollection(clAdBill, "AccType" & strLineKey, mySqlReader("Acc_Type"))
                AddCollection(clAdBill, "AccCode" & strLineKey, mySqlReader("Acc_Code"))
                AddCollection(clAdBill, "AccGLCode" & strLineKey, mySqlReader("Acc_GL_Code"))
                'rowbasetotal = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"),"select case isnull(tran_type,'') when 'RV' then basecredit  else basedebit end basetotal   from receipt_detail Where   receipt_acc_type<>'G' and tran_id='" & tranid & "' and tran_type='" & CType(Session("RVPVTranType"), String) & "' and tran_lineno='" & mySqlReader("against_tran_lineno") & "'")
                AddCollection(clAdBill, "AdjustBaseTotal" & strLineKey, DecRound(txtKWDTotal.Value))

                MainRowindex = mySqlReader("against_tran_lineno")
                intLineNo = intLineNo + 1
            End While
        End If
        myCommand.Dispose()
        SqlConn.Close()

        'Session.Add("Collection", clAdBill)
        Session.Add("Collection" & ":" & txtAdjcolno.Value, clAdBill)

    End Sub
#End Region
#Region "Public Function validate_AdjustBill(ByVal intReceiptLinNo As String,  ByVal strGlCode As String) as Boolean"
    Public Function validate_AdjustBill(ByVal intReceiptLinNo As String, ByVal strAccCode As String, ByVal strAccType As String, ByVal strGlCode As String, ByVal Adjustamt As Decimal) As Boolean
        validate_AdjustBill = True
        Dim collectionDate As Collection
        Dim strLineKey As String
        Dim MainGrdCount As Integer = 1
        Dim base_debit, base_credit As Decimal
        'If Session("Collection").ToString <> "" Then
        'collectionDate = CType(Session("Collection"), Collection)
        collectionDate = GetCollectionFromSession()
        If collectionDate.Count <> 0 Then
            Dim intcount As Integer = collectionDate.Count / 21
            Dim intLinNo, MainRowidx As Integer
            MainRowidx = 1
            For MainRowidx = 1 To MainGrdCount
                If MainRowidx = intReceiptLinNo Then
                    base_debit = 0
                    base_credit = 0
                    For intLinNo = 1 To intcount
                        strLineKey = intLinNo & ":" & intReceiptLinNo
                        If colexists(collectionDate, "AgainstTranLineNo" & strLineKey) = True Then
                            If collectionDate("OpenMode" & strLineKey).ToString = "B" Then
                                If collectionDate("AccCode" & strLineKey).ToString = strAccCode And collectionDate("AccType" & strLineKey).ToString = strAccType And collectionDate("AccGLCode" & strLineKey).ToString = strGlCode Then
                                    base_debit = DecRound(DecRound(base_debit) + DecRound(CType(collectionDate("BaseDebit" & strLineKey), Decimal)))
                                    base_credit = DecRound(DecRound(base_credit) + DecRound(CType(collectionDate("BaseCredit" & strLineKey), Decimal)))
                                Else
                                    validate_AdjustBill = False
                                    Exit Function

                                End If
                            Else
                                base_debit = DecRound(DecRound(base_debit) + DecRound(CType(collectionDate("BaseDebit" & strLineKey), Decimal)))
                                base_credit = DecRound(DecRound(base_credit) + DecRound(CType(collectionDate("BaseCredit" & strLineKey), Decimal)))
                            End If
                        End If
                    Next
                End If
            Next
        End If
        ' End If
        If DecRound(Adjustamt) = DecRound(base_debit) Or DecRound(Adjustamt) = DecRound(base_credit) Then
        Else
            validate_AdjustBill = False
            Exit Function
        End If
    End Function
#End Region

    Protected Sub btnhelp_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=DebitNote','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub
    Private Function validate_BillAgainst() As Boolean
        Try
            validate_BillAgainst = True
            Dim Alflg As Integer
            Dim ErrMsg, strdiv As String
            strdiv = ViewState("divcode") 'objUtils.GetDBFieldFromLongnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", 511)

            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
            myCommand = New SqlCommand("sp_Check_AgainstBills", SqlConn)
            myCommand.CommandType = CommandType.StoredProcedure
            myCommand.Parameters.Add(New SqlParameter("@div_id", SqlDbType.VarChar, 10)).Value = CType(ViewState("divcode"), String)
            myCommand.Parameters.Add(New SqlParameter("@tran_id", SqlDbType.VarChar, 20)).Value = txtDocNo.Text
            myCommand.Parameters.Add(New SqlParameter("@tran_type ", SqlDbType.VarChar, 10)).Value = CType(ViewState("CNDNOpen_type"), String)

            Dim param1 As SqlParameter
            Dim param2 As SqlParameter
            param1 = New SqlParameter
            param1.ParameterName = "@allowflg"
            param1.Direction = ParameterDirection.Output
            param1.DbType = DbType.Int16
            param1.Size = 9
            myCommand.Parameters.Add(param1)
            param2 = New SqlParameter
            param2.ParameterName = "@errmsg"
            param2.Direction = ParameterDirection.Output
            param2.DbType = DbType.String
            param2.Size = 200
            myCommand.Parameters.Add(param2)
            myDataAdapter = New SqlDataAdapter(myCommand)
            myCommand.ExecuteNonQuery()

            Alflg = param1.Value
            ErrMsg = param2.Value

            If Alflg = 1 And ErrMsg <> "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & ErrMsg & "');", True)
                validate_BillAgainst = False
                Exit Function
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("DebitNote.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            clsDBConnect.dbCommandClose(myCommand)
            clsDBConnect.dbConnectionClose(SqlConn)
        End Try
    End Function
    Private Function GetCollectionFromSession() As Collection
        Dim adjcolnostr As String
        adjcolnostr = txtAdjcolno.Value
        Dim collectionDate1 As New Collection
        If Not Session("Collection" & ":" & adjcolnostr) Is Nothing Then
            If Session("Collection" & ":" & adjcolnostr).ToString <> "" Then
                collectionDate1 = CType(Session("Collection" & ":" & adjcolnostr), Collection)
            End If
        End If
        Return collectionDate1
    End Function
    Private Sub CheckPostUnpostRight(ByVal UserName As String, ByVal UserPwd As String, ByVal AppName As String, ByVal PageName As String)
        Dim PostUnpostFlag As Boolean = False
        PostUnpostFlag = objUser.PostUnpostRight(Session("dbconnectionName"), UserName, UserPwd, AppName, PageName)
        If PostUnpostFlag = True Then
            chkPost.Visible = True
            lblPostmsg.Visible = True
        Else
            chkPost.Visible = False
            lblPostmsg.Visible = False
            If ViewState("DebitNoteState") = "Edit" Then
                If chkPost.Checked = True Then
                    ViewState.Add("DebitNoteState", "View")
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This transaction has been posted, you do not have rights to edit.' );", True)
                End If
            End If
        End If
    End Sub

    Protected Sub grdDebitNote_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdDebitNote.RowDataBound
        If (e.Row.RowType = DataControlRowType.Header) Then
            e.Row.Cells(8).Text = "[" + txtbasecurr.Value + "]" + " Value"
        End If

    End Sub

 
End Class
'Protected Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs)
'    Try
'        Dim strReportTitle As String = ""
'        Dim strSelectionFormula As String = ""
'        Session.Add("RefCode", CType(txtDocNo.Text.Trim, String))
'        Session.Add("Pageame", "DebitNoteDoc")
'        Session.Add("BackPageName", "~\AccountsModule\DebitNoteSearch.aspx?tran_type=" & CType(Session("CNDNOpen_type"), String) & "")

'        strSelectionFormula = ""
'        If txtDocNo.Text.Trim <> "" Then
'            If Trim(strSelectionFormula) = "" Then
'                strReportTitle = "Doc No : " & txtDocNo.Text.Trim
'                strSelectionFormula = " {trdpurchase_master.tran_id}='" & txtDocNo.Text.Trim & "'"
'            Else
'                strReportTitle = strReportTitle & "Doc No : " & txtDocNo.Text.Trim & "'"
'                strSelectionFormula = strSelectionFormula & " {trdpurchase_master.tran_id}='" & txtDocNo.Text.Trim & "'"
'            End If
'        Else

'            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Enter Doc No' );", True)
'            Exit Sub
'        End If
'        If Trim(strSelectionFormula) = "" Then
'            strSelectionFormula = " {trdpurchase_master.tran_type} = '" & Session("CNDNOpen_type") & "' " & _
'            " and  {trdpurchase_master.div_id} = '" & CType(objUtils.GetDBFieldFromLongnew(Session("dbconnectionName"),"reservation_parameters", "option_selected", "param_id", 511), String) & "'"
'        Else
'            strSelectionFormula = strSelectionFormula & " AND {trdpurchase_master.tran_type} = '" & Session("CNDNOpen_type") & "'" & _
'            " and  {trdpurchase_master.div_id} = '" & CType(objUtils.GetDBFieldFromLongnew(Session("dbconnectionName"),"reservation_parameters", "option_selected", "param_id", 511), String) & "'"
'        End If
'        Dim lblstr As String
'        lblstr = ""
'        If Session("CNDNOpen_type") = "DN" Then
'            lblstr = "Debit Note"
'        ElseIf Session("CNDNOpen_type") = "CN" Then
'            lblstr = "Credit Note"
'        End If
'        Session.Add("SelectionFormula", strSelectionFormula)
'        Session.Add("ReportTitle", strReportTitle)
'        Session.Add("PrinDocTitle", lblstr)
'        '  Response.Redirect("~\PriceListModule\PrintDoc.aspx", False)
'        Dim ScriptStr As String
'        ScriptStr = "<script language=""javascript"">var win=window.open('../PriceListModule/PrintDoc.aspx','printdoc','toolbar=0,width=420,height=150,top=100,left=200,scrollbars=yes,resizable=no,titlebar=0,menubar=0,locationbar=0,modal=1,statusbar=1');</script>"
'        'ScriptStr = "<script language=""javascript"">var win=window.open('PrintDoc.aspx','mywindow2','toolbar=0,width=250,height=150,top=100,left=200,scrollbars=yes,resizable=no,titlebar=0,menubar=0,locationbar=0,modal=1,statusbar=1');</script>"
'        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", ScriptStr, False)

'    Catch ex As Exception
'        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
'        objUtils.WritErrorLog("OpeningSupplierBalanceSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
'    End Try
'End Sub
'If checkForDuplicate() = False Then
'    Exit Sub
'End If

'ClientScript.GetPostBackEventReference(Me, String.Empty)
'If (Me.IsPostBack And Me.Request("__EVENTTARGET") = "ChildWindowPostBack") Then
'    '    If Session("PrintDoc") Is Nothing = False Then
'    '        If Session("PrintDoc") = "PrintDoc" Then
'    '            Response.Redirect("~\PriceListModule\rptReport.aspx", False)
'    '        ElseIf Session("PrintDoc") = "Searchwindow" Then
'    '            Response.Redirect("DebitNoteSearch.aspx?tran_type=" & CType(Session("CNDNOpen_type"), String) & "", False)
'    '        End If
'    '    End If
'    'End If