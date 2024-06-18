'------------------------------------------------------------------------------------------------
'   Module Name    :    Journal 
'   Developer Name :    Mangesh
'   Date           :    
'   
'
'------------------------------------------------------------------------------------------------
#Region "Namespace"
Imports System.Data
Imports System.Data.SqlClient
Imports System.Globalization
Imports System.Collections.Generic
Imports AjaxControlToolkit.AutoCompleteExtender
#End Region
 
Partial Class Journal
    Inherits System.Web.UI.Page
    'For accounts posting

    Dim caccounts As clssave = Nothing
    Dim cacc As clsAccounts = Nothing
    Dim ctran As clstran = Nothing
    Dim csubtran As clsSubTran = Nothing
    Dim mbasecurrency As String = ""

    Dim ScriptOpenModalDialog As String = "OpenModalDialog('{0}','{1}');"
    'For accounts posting
#Region "Global Declarations"
    Dim objUtils As New clsUtils
    Dim objUser As New clsUser
    Dim objDateTime As New clsDateTime
    Dim strSqlQry As String
    Dim strWhereCond As String
    Dim SqlConn As SqlConnection
    Dim myCommand As SqlCommand
    Dim mySqlReader As SqlDataReader
    Dim myDataAdapter As SqlDataAdapter
    Dim gvRow As GridViewRow
    Dim chckDeletion As CheckBox
    Dim intTabindex = 0

    Enum grd_col
        AccType = 0
        AccCode = 1
        AccName = 2
        ctrolcode = 3
        ctrolname = 4
        costcentercode = 5
        costcentername = 6
        Narration = 7
        Currency = 8
        CnvtRate = 9
        debit = 11
        Credit = 12
        BaseDebit = 9
        BaseCredit = 10
        AdBill = 14
        LienNo = 15
        t1 = 16
        t2 = 17
        t3 = 18
        oldlineno = 19
    End Enum
#End Region


    <System.Web.Script.Services.ScriptMethod()> _
 <System.Web.Services.WebMethod()> _
    Public Shared Function GetSrcCtrylist(ByVal prefixText As String, ByVal contextKey As String) As List(Of String)
        Dim strSqlQry As String = ""
        Dim myDS As New DataSet
        Dim acctnames As New List(Of String)
        Dim agentcode As String = ""
        Dim sqlstr1, acctype As String
        Dim strContext As String()
        Try
            If contextKey <> Nothing Then
                strContext = contextKey.Trim.Split("||")
                If contextKey <> "" Then



                    acctype = contextKey.Trim.Split("||")(0)

                    agentcode = contextKey.Trim.Split("||")(2)


                End If
            End If
            If (acctype = "C") Then
                strSqlQry = "select agentmast_countries.ctrycode,ctryname  from agentmast_countries   join ctrymast on agentmast_countries.ctrycode =ctrymast.ctrycode and agentmast_countries.agentcode='" + agentcode + "'and ctryname like  '" & Trim(prefixText) & "%' "
                'strSqlQry = "select Code,des from view_account where div_code='" + divid + "' and type = '" + acctype + "' order by code"
                '    sqlstr2 = "select des,Code from view_account where div_code='" + txtdivauto.value + "' and type = '" + strtp + "' order by des";
                '    sqlstr3 = "select isnull(controlacctcode,0),code from view_account where div_code='" + txtdivauto.value + "' and type= '" + strtp + "' order by code";
                '    sqlstr4 = " select distinct acctmast.acctname,view_account.controlacctcode  from acctmast ,view_account where  view_account.div_code=acctmast.div_code and view_account.div_code='" + txtdivauto.value + "' and   view_account.controlacctcode= acctmast.acctcode  and type= '" + strtp + "' order by  acctmast.acctname";
                '}

            End If


            'strSqlQry = "select acctcode,acctname from acctmast  where bankyn='Y'   and div_code='01' and acctname like  '" & Trim(prefixText) & "%'"
            'strSqlQry = "    select top 10 Code,des from view_account where view_account.div_code='01'    and des like  '" & Trim(prefixText) & "%'order by des"
            Dim SqlConn As New SqlConnection
            Dim myDataAdapter As New SqlDataAdapter
            ' SqlConn = clsDBConnect.dbConnectionnew(objclsConnectionName.ConnectionName)
            SqlConn = clsDBConnect.dbConnectionnew("strDBConnection")
            'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)
            If myDS.Tables(0).Rows.Count > 0 Then
                For i As Integer = 0 To myDS.Tables(0).Rows.Count - 1
                    acctnames.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(myDS.Tables(0).Rows(i)("ctryname").ToString(), myDS.Tables(0).Rows(i)("ctrycode").ToString()))

                Next
            End If
            Return acctnames
        Catch ex As Exception
            Return acctnames
        End Try

    End Function

    <System.Web.Script.Services.ScriptMethod()> _
<System.Web.Services.WebMethod()> _
    Public Shared Function GetGAcclist(ByVal prefixText As String, ByVal contextKey As String) As List(Of String)
        Dim strSqlQry As String = ""
        Dim myDS As New DataSet
        Dim acctnames As New List(Of String)
        Dim divid As String = ""
        Dim sqlstr1, acctype As String
        Dim strContext As String()
        Try
            'If contextKey = Nothing Then
            '    contextKey = HttpContext.Current.Session("AccType")
            'End If

            If contextKey = "True" Then
                contextKey = "[Select]"
            End If
            strContext = contextKey.Trim.Split("||")

            If contextKey <> "" Then




                acctype = contextKey.Trim.Split("||")(0)

                divid = HttpContext.Current.Session("div_code")


            End If
            divid = HttpContext.Current.Session("div_code")
            If contextKey <> "[Select]" Then
                strSqlQry = "select Code,des from view_account where div_code='" + divid + "' and type = '" + acctype + "' and des like  '" & Trim(prefixText) & "%' order by des"
                '    sqlstr2 = "select des,Code from view_account where div_code='" + txtdivauto.value + "' and type = '" + strtp + "' order by des";
                '    sqlstr3 = "select isnull(controlacctcode,0),code from view_account where div_code='" + txtdivauto.value + "' and type= '" + strtp + "' order by code";
                '    sqlstr4 = " select distinct acctmast.acctname,view_account.controlacctcode  from acctmast ,view_account where  view_account.div_code=acctmast.div_code and view_account.div_code='" + txtdivauto.value + "' and   view_account.controlacctcode= acctmast.acctcode  and type= '" + strtp + "' order by  acctmast.acctname";
                '}
            Else
                strSqlQry = "select top 10 Code,des from view_account where div_code='" + divid + "'   and des like  '" & Trim(prefixText) & "%'   order by des"
                'sqlstr2 = "select top 10  des,Code from view_account where div_code='" + txtdivauto.value + "'  order by des";
                'sqlstr3 = "select top 10  isnull(controlacctcode,0),code from view_account where div_code='" + txtdivauto.value + "'   order by code";
                'sqlstr4 = " select distinct top 10   acctmast.acctname,view_account.controlacctcode  from acctmast ,view_account where view_account.div_code=acctmast.div_code and view_account.div_code='" + txtdivauto.value + "' and    view_account.controlacctcode= acctmast.acctcode  order by  acctmast.acctname";
            End If


            'strSqlQry = "select acctcode,acctname from acctmast  where bankyn='Y'   and div_code='01' and acctname like  '" & Trim(prefixText) & "%'"
            'strSqlQry = "    select top 10 Code,des from view_account where view_account.div_code='01'    and des like  '" & Trim(prefixText) & "%'order by des"
            Dim SqlConn As New SqlConnection
            Dim myDataAdapter As New SqlDataAdapter
            ' SqlConn = clsDBConnect.dbConnectionnew(objclsConnectionName.ConnectionName)
            SqlConn = clsDBConnect.dbConnectionnew("strDBConnection")
            'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)
            If myDS.Tables(0).Rows.Count > 0 Then
                For i As Integer = 0 To myDS.Tables(0).Rows.Count - 1
                    acctnames.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(myDS.Tables(0).Rows(i)("des").ToString(), myDS.Tables(0).Rows(i)("Code").ToString()))

                Next
            End If
            Return acctnames
        Catch ex As Exception
            Return acctnames
        End Try

    End Function
    <System.Web.Script.Services.ScriptMethod()> _
  <System.Web.Services.WebMethod()> _
    Public Shared Function GetCountryDetails(ByVal CustCode As String, ByVal Acctype As String, ByVal SrcCtryautoComp As String, ByVal srcctrycode As String, ByVal srcname As String) As String

        Dim strSqlQry As String = ""
        Dim myDS As New DataSet
        Dim Hotelnames As New List(Of String)
        Try
            If Acctype = "C" Then
                strSqlQry = "select a.ctrycode,c.ctryname from agentmast_countries(nolock) a,ctrymast(nolock) c where a.ctrycode=c.ctrycode and a.agentcode= '" & CustCode.Trim & "'  order by ctryname"
            ElseIf Acctype = "S" Then
                strSqlQry = "select p.ctrycode,c.ctryname from partymast(nolock) p,ctrymast(nolock) c where p.ctrycode=c.ctrycode and p.partycode= '" & CustCode.Trim & "'  order by ctryname"
            ElseIf Acctype = "A" Then
                strSqlQry = " select distinct supplier_agents.ctrycode ,ctrymast.ctryname   from supplier_agents  join ctrymast  on supplier_agents.ctrycode= ctrymast.ctrycode where  supplier_agents.supagentcode='" & CustCode.Trim & "' order by supplier_agents.ctrycode"

            End If
            Dim SqlConn As New SqlConnection
            Dim myDataAdapter As New SqlDataAdapter
            SqlConn = clsDBConnect.dbConnectionnew("strDBConnection")
            'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS, "Countries")

            Return myDS.GetXml()
        Catch ex As Exception
            Return Nothing
        End Try
    End Function



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
    'Tanvir 24102023
    Private Sub createjournaldatatable()

        Dim dataTable As New DataTable()
        dataTable.Columns.Add("TRANID", GetType(String))
        dataTable.Columns.Add("tran_type", GetType(String))
        dataTable.Columns.Add("tran_date", GetType(String))
        dataTable.Columns.Add("tran_lineno", GetType(Integer))
        dataTable.Columns.Add("against_tran_id", GetType(String))
        dataTable.Columns.Add("against_tran_lineno", GetType(Integer))
        dataTable.Columns.Add("against_tran_type", GetType(String))
        dataTable.Columns.Add("against_tran_date", GetType(String))
        dataTable.Columns.Add("open_due_date", GetType(String))
        dataTable.Columns.Add("open_debit", GetType(Decimal))
        dataTable.Columns.Add("open_credit", GetType(Decimal))
        dataTable.Columns.Add("open_field1", GetType(String))
        dataTable.Columns.Add("open_field2", GetType(String))
        dataTable.Columns.Add("open_field3", GetType(String))
        dataTable.Columns.Add("open_field4", GetType(String))
        dataTable.Columns.Add("open_field5", GetType(String))
        dataTable.Columns.Add("open_mode", GetType(String))
        dataTable.Columns.Add("currency_rate", GetType(Decimal))
        dataTable.Columns.Add("div_id", GetType(String))
        dataTable.Columns.Add("base_debit", GetType(Decimal))
        dataTable.Columns.Add("base_CREDIT", GetType(Decimal))
        dataTable.Columns.Add("acc_type", GetType(String))
        dataTable.Columns.Add("acc_code", GetType(String))
        dataTable.Columns.Add("acc_gl_code", GetType(String))
        dataTable.Columns.Add("AgainstTranLineNo", GetType(Integer))
        dataTable.Columns.Add("currcode", GetType(String))
        Session("Adjustedrecords") = dataTable
    End Sub
     'Tanvir 24102023

#Region "Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load"

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

        ViewState.Add("JournalState", Request.QueryString("State"))
        ViewState.Add("JournalTranType", "JV")
        txtMode.Value = ViewState("JournalState")
        ViewState.Add("divcode", Request.QueryString("divid"))
        If Page.IsPostBack = False Then
            Try
                If Not Session("CompanyName") Is Nothing Then
                    Me.Page.Title = CType(Session("CompanyName"), String)
                End If
                'Session("TabIndex") = Nothing
                Session("div_code") = ViewState("divcode")
                txtconnection.Value = Session("dbconnectionName")

                If CType(Session("GlobalUserName"), String) = Nothing Or CType(Session("Userpwd"), String) = Nothing Then
                    Response.Redirect("~/Login.aspx", False)
                    Exit Sub
                End If
                'SetFocus(txtnarration)

                'Session.Add("Collection", "")
                ' Session.Add("TranType", "JV")
                Session("opendetail_records_jv") = Nothing
                Session("Adjustedrecords") = Nothing
                createjournaldatatable()
                Dim adjcolno As String
                adjcolno = objUtils.GetAutoDocNoWTnew(Session("dbconnectionName"), "ADJCOL")
                txtAdjcolno.Value = adjcolno
                Session.Add("Collection" & ":" & adjcolno, "")
                txtdecimal.Value = objUtils.GetDBFieldFromLongnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", 509)

                txtbasecurr.Value = objUtils.GetDBFieldFromLongnew(Session("dbconnectionName"), "reservation_parameters", "option_Selected", "param_id", 457)

                txtDivCode.Value = ViewState("divcode")

                ViewState.Add("JournalRefCode", Request.QueryString("RefCode"))

                txtJDate.Text = Format(objDateTime.GetSystemDateTime(Session("dbconnectionName")), "dd/MM/yyyy")
                txtTDate.Text = txtJDate.Text
                txtTranType.Text = CType(ViewState("JournalTranType"), String)

                'Base Cuurncy
                txtbasecurr.Value = objUtils.GetDBFieldFromLongnew(Session("dbconnectionName"), "reservation_parameters", "option_Selected", "param_id", 457)
                lblBaseTot.Text = txtbasecurr.Value & " Total"
                lblBaseDiff.Text = txtbasecurr.Value & " Diff."
                txtdecimal.Value = objUtils.GetDBFieldFromLongnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", 509)
                strSqlQry = "select  narration,narration from narration where active=1"
                ' objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlNarration, "narration", "narration", strSqlQry, True)

                '15122014

                ' objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSMktCode, "plgrpcode", "plgrpname", "select plgrpcode,plgrpname from plgrpmast where active=1 order by plgrpcode", True)
                ' objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSMktName, "plgrpname", "plgrpcode", "select plgrpname,plgrpcode from plgrpmast where active=1 order by plgrpname", True)


                Dim ds As DataSet = objUtils.ExecuteQuerySqlnew(Session("dbconnectionName"), "select top 1  sealdate from  sealing_master ")
                If ds.Tables.Count > 0 Then
                    If ds.Tables(0).Rows.Count > 0 Then
                        If IsDBNull(ds.Tables(0).Rows(0)("sealdate")) = False Then
                            txtpdate.Text = CType(ds.Tables(0).Rows(0)("sealdate"), String)
                        End If
                    Else
                        txtpdate.Text = objUtils.GetDBFieldFromLongnew(Session("dbconnectionName"), "reservation_parameters", "option_Selected", "param_id", 508)
                    End If
                End If


                If ViewState("JournalState") = "New" Then
                    hdnRows.Value = 0
                    'fillDategrd(grdJournal, False, 5)
                    lblNoofRows.Visible = False
                    txtNoofRows.Visible = False
                    btnGenGrid.Visible = False
                    txtNoofRows.Value = 5
                    HideControls()
                    txtDocNo.Value = ""
                    lblHeading.Text = "Add " & lblHeading.Text
                    btnSave.Text = "Save"
                    chkPost.Checked = True
                    btnGenGrid_Click(sender, e)
                    btnSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to save')==false)return false;")
                ElseIf ViewState("JournalState") = "Copy" Then
                    hdnRows.Value = 0
                    txtDocNo.Value = ""
                    show_record(ViewState("JournalRefCode"))
                    ShowFillGrid(ViewState("JournalRefCode"))
                    lblHeading.Text = "Copy " & lblHeading.Text
                    btnSave.Text = "Save"
                    btnSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to save')==false)return false;")
                ElseIf ViewState("JournalState") = "Edit" Then
                    txtDocNo.Value = ViewState("JournalRefCode")
                    ''''''                   
                    'show_record(ViewState("JournalRefCode"))
                    'ShowFillGrid(ViewState("JournalRefCode"))
                    'fillcollection(ViewState("JournalRefCode"))

                    hdnRows.Value = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "Select count(*) from journal_detail Where tran_id='" & ViewState("JournalRefCode") & "' and  tran_type='" & CType(ViewState("JournalTranType"), String) & "'")
                    'as changed on 26/01/2013
                    Dim norows As Integer = CType(hdnRows.Value, Integer) + 2
                    txtNoofRows.Value = norows


                    show_record(txtDocNo.Value.Trim())
                    'fillDategrd(grdJournal, False, 2)
                    btnGenGrid.Enabled = False
                    ShowControls()
                    ShowFillGridInit(txtDocNo.Value.Trim())
                    fillcollection(txtDocNo.Value.Trim())
                    ''''''

                    lblNoofRows.Visible = False
                    txtNoofRows.Visible = False
                    btnGenGrid.Visible = False
                    'HideControls()
                    lblHeading.Text = "Edit " & lblHeading.Text
                    btnSave.Text = "Update"

                    'Dim strURL As String = ""
                    'strURL = "Accnt_trn_amendlog.aspx?tid=" & txtDocNo.Value & "&ttype=" & txtTranType.Text.Trim & "&tdate=" & txtTDate.Text.Trim
                    'btnSave.Attributes.Add("onclick", String.Format(ScriptOpenModalDialog, strURL, 300))
                    btnSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to update')==false)return false;")
                ElseIf ViewState("JournalState") = "Delete" Then
                    hdnRows.Value = 0
                    txtDocNo.Value = ViewState("JournalRefCode")
                    show_record(ViewState("JournalRefCode"))
                    ShowFillGrid(ViewState("JournalRefCode"))
                    fillcollection(ViewState("JournalRefCode"))
                    lblHeading.Text = "Delete " & lblHeading.Text
                    btnSave.Text = "Delete"
                    btnSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to delete')==false)return false;")
                ElseIf ViewState("JournalState") = "View" Then
                    hdnRows.Value = 0
                    txtDocNo.Value = ViewState("JournalRefCode")
                    show_record(ViewState("JournalRefCode"))
                    ShowFillGrid(ViewState("JournalRefCode"))
                    fillcollection(ViewState("JournalRefCode"))
                    lblHeading.Text = "View " & lblHeading.Text
                ElseIf ViewState("JournalState") = "Cancel" Then
                    hdnRows.Value = 0
                    txtDocNo.Value = ViewState("JournalRefCode")
                    show_record(ViewState("JournalRefCode"))
                    ShowFillGrid(ViewState("JournalRefCode"))
                    fillcollection(ViewState("JournalRefCode"))
                    lblHeading.Text = "Cancel " & lblHeading.Text
                    btnSave.Text = "Cancel"
                    btnSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to Cancel')==false)return false;")
                ElseIf ViewState("JournalState") = "UndoCancel" Then
                    hdnRows.Value = 0
                    txtDocNo.Value = ViewState("JournalRefCode")
                    show_record(ViewState("JournalRefCode"))
                    ShowFillGrid(ViewState("JournalRefCode"))
                    fillcollection(ViewState("JournalRefCode"))
                    lblHeading.Text = "Undo Cancel " & lblHeading.Text
                    btnSave.Text = "Undo"
                    btnSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to Undocancel')==false)return false;")

                End If
                Dim appname As String = ""
                Dim appidnew As String = ""
                If ViewState("divcode") = "01" Then
                    appname = "ColumbusCommon" + " " + CType("Accounts Module", String)
                    appidnew = "4"
                Else
                    appname = "ColumbusCommon Gulf " + CType("Accounts Module", String)
                    appidnew = "14"
                End If

                CheckPostUnpostRight(CType(Session("GlobalUserName"), String), CType(Session("Userpwd"), String), CType(appname, String), "AccountsModule\JournalSearch.aspx?appid=" & appidnew, appidnew)

                Disabled_Control()
                btnExit.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to exit?')==false)return false;")
                'ddlNarration.Attributes.Add("onchange", "javascript:FillCombotoText('" + CType(ddlNarration.ClientID, String) + "','" + CType(txtnarration.ClientID, String) + "')")
                txtJDate.Attributes.Add("onchange", "javascript:changedate('" + CType(txtJDate.ClientID, String) + "','" + txtTDate.ClientID + "')")


                Dim typ As Type
                typ = GetType(DropDownList)
                lblNoofRows.Visible = False
                txtNoofRows.Visible = False
                btnGenGrid.Visible = False
                If (Page.ClientScript.IsClientScriptIncludeRegistered(typ, "TADDScript.js")) = False Then
                    Page.ClientScript.RegisterClientScriptInclude(typ, "TADDScript.js", "TADDScript.js")
                    'ddlNarration.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                End If
                btnPrint.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to print?')==false)return false;")
            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objUtils.WritErrorLog("Journal.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            End Try
        End If

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If check_Privilege() = 1 Then

            chkPost.Enabled = True
            chkPost.Checked = True
        Else

            chkPost.Enabled = False
            chkPost.Checked = True
        End If

        'Added check_Privilege() and chkpost enabled by Archana on 01/04/2015



        If IsPostBack = True Then
            ' FillDpendGridDetails()

            Allowanyway()
            If Session("Allowanyway") = "Yes" Then
                chkadjust.Visible = False
            End If


            ' FillGridConvRate()
            FillGrid()


            Dim txtcr, txtbasecr, txtdr, txtbasedr, txtcrate As HtmlInputText
            Dim rowind As Integer





            If (Me.IsPostBack And Me.Request("__EVENTTARGET") = "AdjBillWindowPostBack") Then
                rowind = Val(Session("LineNo")) - 1

                txtcr = grdJournal.Rows(rowind).FindControl("txtCredit")
                txtbasecr = grdJournal.Rows(rowind).FindControl("txtBaseCredit")
                txtdr = grdJournal.Rows(rowind).FindControl("txtDebit")
                txtbasedr = grdJournal.Rows(rowind).FindControl("txtBaseDebit")
                txtcrate = grdJournal.Rows(rowind).FindControl("txtConvRate")
                If Session("Gridtype") = "Debit" Then
                    txtdr.Value = Session("AmountAdjusted")
                    txtbasedr.Value = Session("BaseAmountAdjusted")
                    txtcrate.Value = CType(DecRound(txtbasedr.Value) / DecRound(txtdr.Value), Decimal)
                ElseIf Session("Gridtype") = "Credit" Then
                    txtcr.Value = Session("AmountAdjusted")
                    txtbasecr.Value = Session("BaseAmountAdjusted")
                    txtcrate.Value = CType(DecRound(txtbasecr.Value) / DecRound(txtcr.Value), Decimal)
                End If
                GrandToatal()
            End If
        Else
            'FillGrid()

        End If



    End Sub
#End Region


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
            objUtils.WritErrorLog("journal.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            'clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
            'clsDBConnect.dbReaderClose(mySqlReader)             ' sql reader disposed    
            'clsDBConnect.dbConnectionClose(mySqlConn)           'connection close 
        End Try
    End Function

    Private Sub GrandToatal()
        Dim gvrow As GridViewRow
        Dim txtbasecr, txtbasedr As HtmlInputText
        Dim totalcr, totaldr As Decimal
        For Each gvrow In grdJournal.Rows
            txtbasecr = gvrow.FindControl("txtBaseCredit")
            totalcr = totalcr + CType(Val(txtbasecr.Value), Decimal)

            txtbasedr = gvrow.FindControl("txtBaseDebit")
            totaldr = totaldr + CType(Val(txtbasedr.Value), Decimal)
        Next
        txtTotBaseCredit.Value = DecRound(totalcr)
        txtTotBaseDebit.Value = DecRound(totaldr)

    End Sub
    Private Sub FillGridConvRate()
        Dim strQry As String = ""
        Dim gvrow As GridViewRow
        Dim ddlAccType As HtmlSelect
        Dim txtAccCode As TextBox
        Dim txtCurrRate As HtmlInputText

        For Each gvrow In grdJournal.Rows
            txtCurrRate = gvrow.FindControl("txtConvRate")
            ddlAccType = gvrow.FindControl("ddlType")
            txtAccCode = gvrow.FindControl("txtacctCode")

            'strQry = "select cur,convrate,controlacctcode from  view_account left outer join " & _
            '                   " currrates on  view_account.cur=currrates.currcode and tocurr = (select option_selected from reservation_parameters where param_id=457) where code = '" & ddlgAccCode.Items(ddlgAccCode.SelectedIndex).Text & "' and type='" & ddlAccType.Items(ddlAccType.SelectedIndex).Text & "' "
            If ddlAccType.Value <> "[Select]" And txtAccCode.Text <> "" Then
                If Val(txtCurrRate.Value) = 0 Then
                    strQry = "select convrate from  view_account left outer join " & _
                                    " currrates on  view_account.cur=currrates.currcode and tocurr = (select option_selected from reservation_parameters where  param_id=457) where view_account.div_code='" & ViewState("divcode") & "' and  code = '" & txtAccCode.Text & "' and type='" & ddlAccType.Value & "' "
                    txtCurrRate.Value = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), strQry)
                End If
            End If
        Next

    End Sub
#Region "Public Sub Disabled_Control()"
    Public Sub Disabled_Control()
        txtDocNo.Disabled = True
        If ViewState("JournalState") = "New" Or ViewState("JournalState") = "Copy" Then
            btnPrint.Visible = False
            btnPdfReport.Visible = False
        ElseIf ViewState("JournalState") = "Edit" Then
            btnPrint.Visible = False
            btnPdfReport.Visible = False
        ElseIf ViewState("JournalState") = "Delete" Or ViewState("JournalState") = "View" Or ViewState("JournalState") = "Cancel" Or ViewState("JournalState") = "UndoCancel" Then

            If ViewState("JournalState") <> "View" Then
                btnPrint.Visible = False
                btnPdfReport.Visible = False
            End If

            txtnarration.Enabled = False
            txtReference.Disabled = True
            txtJDate.Enabled = False
            txtTDate.Enabled = False
            btnAdd.Visible = False
            btnDelLine.Visible = False

            btnSave.Visible = False
            chkPost.Visible = False


            DisableGrid()
        End If
        If ViewState("JournalState") = "View" Then
            btnPrint.Visible = False
            btnPdfReport.Visible = True
        ElseIf ViewState("JournalState") = "Delete" Then
            btnSave.Visible = True
        ElseIf ViewState("JournalState") = "Cancel" Or ViewState("JournalState") = "UndoCancel" Then
            btnPrint.Visible = False
            btnPdfReport.Visible = False
            btnSave.Visible = True
        End If
    End Sub
    Private Sub DisableGrid()
        Dim ddlAccType As HtmlSelect
        Dim txtacct As TextBox
        Dim txtacctcode As TextBox
        Dim txtcontrolacctcode As TextBox
        Dim txtcontrolacct As TextBox
        Dim txtnarr As TextBox
        Dim txtcostcentercode As TextBox
        Dim txtcostcentername As TextBox
        Dim txtcurrencycode As HtmlInputText
        Dim txtcurrencyname As TextBox
        Dim txtConvRate As HtmlInputText
        Dim txtsourcectrycode As TextBox
        Dim txtsource As TextBox
        Dim txtbookingno As TextBox




        Dim txtCredit As HtmlInputControl
        Dim txtDebit As HtmlInputControl

        Dim txtBaseCredit As HtmlInputText
        Dim txtBaseDebit As HtmlInputText
        Dim btnBill As HtmlInputButton




        For Each gvRow In grdJournal.Rows
            ddlAccType = gvRow.FindControl("ddlType")
            txtacctcode = gvRow.FindControl("txtacctcode")
            txtacct = gvRow.FindControl("txtacct")
            txtcontrolacctcode = gvRow.FindControl("txtcontrolacctcode")
            txtcontrolacct = gvRow.FindControl("txtcontrolacct")
            txtcostcentercode = gvRow.FindControl("txtcostcentercode")
            txtcostcentername = gvRow.FindControl("txtcostcentername")
            txtcurrencycode = gvRow.FindControl("txtcurrencycode")
            txtConvRate = gvRow.FindControl("txtConvRate")
            txtDebit = gvRow.FindControl("txtDebit")
            txtCredit = gvRow.FindControl("txtCredit")
            txtBaseDebit = gvRow.FindControl("txtBaseDebit")
            txtBaseCredit = gvRow.FindControl("txtBaseCredit")
            txtnarr = gvRow.FindControl("txtNarr")
            btnBill = gvRow.FindControl("btnAd")



            btnBill.Disabled = False
            ddlAccType.Disabled = True
            txtacctcode.Enabled = False
            txtacct.Enabled = False
            txtcontrolacctcode.Enabled = False
            txtcontrolacct.Enabled = False
            txtcostcentercode.Enabled = False
            txtcostcentername.Enabled = False
            txtcurrencycode.Disabled = True
            txtConvRate.Disabled = True
            txtCredit.Disabled = True
            txtDebit.Disabled = True
            txtnarr.Enabled = False


        Next

    End Sub
#End Region

#Region "Public Function Allowanyway() As Boolean"
    Public Function Allowanyway()
        Try
            Dim strSql As String
            Dim usrCode As String
            usrCode = CType(Session("GlobalUserName"), String)
            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open

            strSql = "select appid from group_privilege_Detail where (privilegeid='2' and appid='4')  and "
            strSql += "groupid=(SELECT groupid FROM UserMaster WHERE UserCode='" + usrCode + "')"
            myCommand = New SqlCommand(strSql, SqlConn)
            mySqlReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection)
            If mySqlReader.HasRows Then
                Session.Add("Allowanyway", "Yes")
            Else
                Session.Add("Allowanyway", "No")
            End If
            Allowanyway = ""
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("Reservation.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            clsDBConnect.dbCommandClose(myCommand)               'sql command disposed
            clsDBConnect.dbReaderClose(mySqlReader)             ' sql reader disposed    
            clsDBConnect.dbConnectionClose(SqlConn)           'connection close 
            Allowanyway = ""
        End Try
    End Function
#End Region

#Region "Public Sub show_record()"
    Public Sub show_record(ByVal RefCode As String)
        Dim mySqlReader As SqlDataReader
        Try
            chkPost.Checked = False
            Dim myDS As New DataSet
            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            myCommand = New SqlCommand("select * from journal_master Where div_id='" & ViewState("divcode") & "' and tran_id='" & RefCode & "' and  tran_type='" & CType(ViewState("JournalTranType"), String) & "'", SqlConn)
            mySqlReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection)

            If mySqlReader.HasRows Then
                If mySqlReader.Read() = True Then
                    If IsDBNull(mySqlReader("post_state")) = False And CType(ViewState("JournalState"), String) <> "Copy" Then
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

                    If IsDBNull(mySqlReader("cancel_state")) = False Then
                        If CType(mySqlReader("cancel_state"), String) = "Y" Then
                            lblPostmsg.Text = "Cancelled"
                            lblPostmsg.ForeColor = Drawing.Color.Green
                        End If
                    End If


                    If IsDBNull(mySqlReader("tran_id")) = False Then
                        If ViewState("JournalState") = "Copy" Then
                            txtDocNo.Value = ""
                        Else
                            txtDocNo.Value = CType(mySqlReader("tran_id"), String)
                        End If
                    End If

                    If IsDBNull(mySqlReader("journal_date")) = False Then
                        txtJDate.Text = Format(mySqlReader("journal_date"), "dd/MM/yyyy")
                    End If
                    If IsDBNull(mySqlReader("journal_tran_date")) = False Then
                        txtTDate.Text = Format(mySqlReader("journal_tran_date"), "dd/MM/yyyy")
                    End If
                    txtReference.Value = CType(mySqlReader("journal_mrv"), String)
                    txtnarration.Text = CType(mySqlReader("journal_narration"), String)
                    txtTranType.Text = CType(ViewState("JournalTranType"), String)

                    '15122014


                End If
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("Journal.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            clsDBConnect.dbCommandClose(myCommand)
            clsDBConnect.dbReaderClose(mySqlReader)
            clsDBConnect.dbConnectionClose(SqlConn)

        End Try
    End Sub
    Private Sub ShowFillGrid(ByVal RefCode As String)
        Dim mySqlReader As SqlDataReader
        Try
            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))


            Dim ddlAccType As HtmlSelect
            Dim txtacct As TextBox
            Dim txtacctcode As TextBox
            Dim txtcontrolacctcode As TextBox
            Dim txtcontrolacct As TextBox
            Dim txtnarr As TextBox
            Dim txtcostcentercode As TextBox
            Dim txtcostcentername As TextBox
            Dim txtcurrencycode As HtmlInputText
            Dim txtcurrencyname As TextBox
            Dim txtConvRate As HtmlInputText
            Dim txtsourcectrycode As TextBox
            Dim txtsource As TextBox
            Dim txtbookingno As TextBox

            'Dim ddldept As HtmlSelect
            Dim txtDebit, txtBaseDebit, txtCredit, txtBaseCredit, txtOldLineno As HtmlInputText


            Dim lblno As HtmlInputText




            Dim sqlstr1, sqlstr2 As String
            Dim lngCnt As Long
            Dim credittot, debittot, basecredittot, basedebittot As Decimal
            If hdnRows.Value < txtNoofRows.Value Then
                lngCnt = CType(hdnRows.Value, Integer) + CType(txtNoofRows.Value, Integer)  'objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"),"journal_detail", "count(tran_id)", "tran_id", RefCode)
            Else
                lngCnt = objUtils.GetDBFieldFromStringnewdiv(Session("dbconnectionName"), "journal_detail", "count(tran_id)", "tran_id", RefCode, "div_id", ViewState("divcode"))
            End If
            If lngCnt <= 5 Then lngCnt = 5
            fillDategrd(grdJournal, False, lngCnt)
            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
            strSqlQry = "Select * from journal_detail Where div_id='" & ViewState("divcode") & "' and tran_id='" & RefCode & "' and  tran_type='" & CType(ViewState("JournalTranType"), String) & "' order by tran_lineno"
            myCommand = New SqlCommand(strSqlQry, SqlConn)
            mySqlReader = myCommand.ExecuteReader()

            If mySqlReader.HasRows Then
                While mySqlReader.Read()
                    For Each gvRow In grdJournal.Rows
                        lblno = gvRow.FindControl("txtlineno")
                        If mySqlReader("tran_lineno") = CType(lblno.Value, Integer) Then
                            txtOldLineno = gvRow.FindControl("txtOldLineno")
                            ddlAccType = gvRow.FindControl("ddlType")
                            txtacctcode = gvRow.FindControl("txtacct")
                            txtacct = gvRow.FindControl("txtacct")
                            txtcostcentercode = gvRow.FindControl("txtcostcentercode")
                            txtcostcentername = gvRow.FindControl("txtcostcentername")
                            txtcurrencycode = gvRow.FindControl("txtcurrencycode")
                            txtcurrencyname = gvRow.FindControl("txtcurrencyname")
                            txtConvRate = gvRow.FindControl("txtConvRate")
                            txtcontrolacctcode = gvRow.FindControl("txtcontrolacctcode")
                            txtcontrolacct = gvRow.FindControl("txtcontrolacct")
                            txtDebit = gvRow.FindControl("txtDebit")
                            txtCredit = gvRow.FindControl("txtCredit")

                            'ddldept = gvRow.FindControl("ddldept")

                            txtBaseDebit = gvRow.FindControl("txtBaseDebit")
                            txtBaseCredit = gvRow.FindControl("txtBaseCredit")

                            txtnarr = gvRow.FindControl("txtnarr")


                            txtsourcectrycode = gvRow.FindControl("txtsourcectrycode")
                            txtsource = gvRow.FindControl("txtsource")
                            txtbookingno = gvRow.FindControl("txtbookingno")

                            txtOldLineno.Value = lblno.Value

                            ddlAccType.Value = mySqlReader("journal_acc_type").ToString.Trim

                            txtacctcode.Text = mySqlReader("journal_acc_code").ToString
                            txtacct.Text = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select des from view_account where  div_code='" & ViewState("divcode") & "' and type = '" & ddlAccType.Value & "' and code ='" & mySqlReader("journal_acc_code").ToString & "' ")


                            txtcurrencycode.Disabled = True
                            txtcontrolacct.Enabled = False
                            txtcostcentercode.Enabled = False
                            txtcostcentername.Enabled = False
                            txtConvRate.Disabled = False

                            If ddlAccType.Value = "G" Then
                                'sqlstr1 = " select ''  as controlacctcode, '' as acctname  "
                                'sqlstr2 = " select  '' as acctname , '' as controlacctcode "
                                'txtcontrolacct.Enabled = False
                                'ddlConAccName.Enabled = False
                                txtcostcentercode.Enabled = False
                                txtcostcentername.Enabled = False
                            ElseIf ddlAccType.Value = "C" Then
                                'sqlstr1 = " select distinct view_account.controlacctcode, acctmast.acctname  from acctmast ,view_account where acctmast.div_code=view_account.div_code and acctmast.div_code='" & ViewState("divcode") & "' and   view_account.controlacctcode= acctmast.acctcode  and type= '" + ddlAccType.Value + "' and view_account.code='" + ddlgAccName.Value + "' order by  view_account.controlacctcode"
                                'sqlstr2 = " select distinct  acctmast.acctname,view_account.controlacctcode  from acctmast ,view_account where  acctmast.div_code=view_account.div_code and acctmast.div_code='" & ViewState("divcode") & "' and   view_account.controlacctcode= acctmast.acctcode  and type= '" + ddlAccType.Value + "' and view_account.code='" + ddlgAccName.Value + "' order by  acctmast.acctname"
                            ElseIf ddlAccType.Value = "S" Then
                                'sqlstr1 = " select distinct partymast.controlacctcode  , acctmast.acctname  from acctmast ,partymast where acctmast.div_code='" & ViewState("divcode") & "' and  partymast.controlacctcode= acctmast.acctcode   and partymast.partycode='" + ddlgAccName.Value + "' order by controlacctcode"
                                'sqlstr2 = " select distinct acctmast.acctname ,partymast.controlacctcode      from acctmast ,partymast where acctmast.div_code='" & ViewState("divcode") & "' and  partymast.controlacctcode= acctmast.acctcode   and partymast.partycode='" + ddlgAccName.Value + "' order by acctmast.acctname"
                            ElseIf ddlAccType.Value = "A" Then
                                'sqlstr1 = " select distinct supplier_agents.controlacctcode    , acctmast.acctname  from acctmast ,supplier_agents where acctmast.div_code='" & ViewState("divcode") & "' and  supplier_agents.controlacctcode= acctmast.acctcode   and supplier_agents.supagentcode='" + ddlgAccName.Value + "' order by controlacctcode"
                                'sqlstr2 = " select distinct acctmast.acctname ,supplier_agents.controlacctcode     from acctmast ,supplier_agents where  acctmast.div_code='" & ViewState("divcode") & "' and  supplier_agents.controlacctcode= acctmast.acctcode   and supplier_agents.supagentcode='" + ddlgAccName.Value + "' order by acctmast.acctname"
                            End If
                            'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlConAccCode, "controlacctcode", "acctname", sqlstr1, True)
                            'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlConAccName, "acctname", "controlacctcode", sqlstr2, True)

                            If ddlAccType.Value <> "G" Then
                                txtcontrolacctcode.Text = mySqlReader("journal_gl_code").ToString
                                txtcontrolacct.Text = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select acctname from acctmast where acctmast.div_code='" & ViewState("divcode") & "' and acctcode ='" & mySqlReader("journal_gl_code").ToString & "' ")

                            Else

                                txtcontrolacctcode.Text = ""
                                txtcontrolacct.Text = ""
                            End If
                            'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCCCode, "costcenter_code", "costcenter_name", "select costcenter_code, costcenter_name from costcenter_master where active=1 order by costcenter_code ", True)
                            'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCCName, "costcenter_name", "costcenter_code", "select costcenter_code, costcenter_name from costcenter_master where active=1 order by costcenter_name ", True)

                            'ddlCCCode.Value = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select costcenter_name from costcenter_master where costcenter_code ='" & mySqlReader("costcenter_code").ToString & "' ")
                            'ddlCCName.Value = mySqlReader("costcenter_code").ToString

                            '15122014

                            'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddldept, "plgrpname", "plgrpcode", "select plgrpname,plgrpcode from plgrpmast where active=1 order by plgrpname", True)
                            ''objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddldept, "othmaingrpname", "othmaingrpcode", "select othmaingrpcode,othmaingrpname from othmaingrpmast where active=1 order by othmaingrpcode ", True)

                            'ddldept.Value = mySqlReader("dept").ToString

                            'Dim glCode As String
                            'glCode = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"),"select isnull(controlacctcode,0) from dbo.view_account where type='" & ddlAccType.Value & "'and code='" & ddlgAccCode.Items(ddlgAccCode.SelectedIndex).Text & "'")
                            'If ddlAccType.Value = "G" Then
                            '    ddlgControlAcc.Value = "Select"
                            'Else
                            '    ddlgControlAcc.Value = mySqlReader("journal_acc_code").ToString
                            'End If

                            txtsourcectrycode.Text = mySqlReader("srcctrycode").ToString
                            txtsource.Text = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select ctryname from ctrymast where  ctrycode ='" & mySqlReader("srcctrycode").ToString & "' ")
                            txtbookingno.Text = mySqlReader("BookingNo").ToString

                            txtcostcentercode.Text = mySqlReader("costcenter_code").ToString

                            txtcurrencycode.Value = mySqlReader("journal_currency_id").ToString
                            txtcurrencyname.Text = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select currname from currmast where  currcode ='" & mySqlReader("journal_currency_id").ToString & "' ")
                            txtConvRate.Value = mySqlReader("journal_currency_rate").ToString
                            txtnarr.Text = mySqlReader("journal_narration").ToString

                            txtCredit.Value = DecRound(mySqlReader("journal_credit").ToString)
                            txtDebit.Value = DecRound(mySqlReader("journal_debit").ToString)

                            txtBaseCredit.Value = DecRound(mySqlReader("basecredit").ToString)
                            txtBaseDebit.Value = DecRound(mySqlReader("basedebit").ToString)


                            credittot = CType(Val(credittot), Decimal) + CType(Val(txtCredit.Value), Decimal)
                            debittot = CType(Val(debittot), Decimal) + CType(Val(txtDebit.Value), Decimal)
                            basecredittot = CType(Val(basecredittot), Decimal) + CType(Val(txtBaseCredit.Value), Decimal)
                            basedebittot = CType(Val(basedebittot), Decimal) + CType(Val(txtBaseDebit.Value), Decimal)

                            If ViewState("JournalState") = "Edit" Or ViewState("JournalState") = "Copy" Then
                                If Trim(txtbasecurr.Value) = Trim(txtcurrencycode.Value) Then
                                    txtConvRate.Disabled = True
                                End If
                            End If
                            Exit For
                        End If
                    Next
                End While
            End If
            txtTotalCredit.Value = DecRound(credittot)
            txtTotalDebit.Value = DecRound(debittot)
            txtTotBaseCredit.Value = DecRound(basecredittot)
            txtTotBaseDebit.Value = DecRound(basedebittot)

            mySqlReader.Close()
            SqlConn.Close()
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("Journal.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            clsDBConnect.dbCommandClose(myCommand)
            clsDBConnect.dbReaderClose(mySqlReader)
            clsDBConnect.dbConnectionClose(SqlConn)
        End Try
    End Sub
#End Region
    Private Sub ShowFillGridInit(ByVal RefCode As String)
        Dim mySqlReader As SqlDataReader
        Try
            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))



            Dim ddlAccType As HtmlSelect
            Dim txtacct As TextBox
            Dim txtacctcode As TextBox
            Dim txtcontrolacctcode As TextBox
            Dim txtcontrolacct As TextBox
            Dim txtnarr As TextBox
            Dim txtcostcentercode As TextBox
            Dim txtcostcentername As TextBox
            Dim txtcurrencycode As HtmlInputText
            Dim txtcurrencyname As TextBox
            Dim txtConvRate As HtmlInputText
            Dim txtsourcectrycode As TextBox
            Dim txtsource As TextBox
            Dim txtbookingno As TextBox

            'Dim ddldept As HtmlSelect
            Dim txtDebit, txtBaseDebit, txtCredit, txtBaseCredit, txtOldLineno As HtmlInputText



            Dim lblno As HtmlInputText





            Dim sqlstr1, sqlstr2 As String
            Dim lngCnt As Long
            Dim credittot, debittot, basecredittot, basedebittot As Decimal
            'If hdnRows.Value < txtNoofRows.Value Then
            '    lngCnt = CType(hdnRows.Value, Integer) + CType(txtNoofRows.Value, Integer)  'objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"),"journal_detail", "count(tran_id)", "tran_id", RefCode)
            'Else
            '    lngCnt = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "journal_detail", "count(tran_id)", "tran_id", RefCode)
            'End If
            'If lngCnt <= 5 Then lngCnt = 5
            fillDategrd(grdJournal, False, CType(txtNoofRows.Value, Integer))
            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
            strSqlQry = "Select * from journal_detail Where  div_id='" & ViewState("divcode") & "' and tran_id='" & RefCode & "' and  tran_type='" & CType(ViewState("JournalTranType"), String) & "' order by tran_lineno"
            myCommand = New SqlCommand(strSqlQry, SqlConn)
            mySqlReader = myCommand.ExecuteReader()

            If mySqlReader.HasRows Then
                While mySqlReader.Read()
                    For Each gvRow In grdJournal.Rows
                        lblno = gvRow.FindControl("txtlineno")
                        If mySqlReader("tran_lineno") = CType(lblno.Value, Integer) Then
                            txtOldLineno = gvRow.FindControl("txtOldLineno")
                            ddlAccType = gvRow.FindControl("ddlType")
                            txtacctcode = gvRow.FindControl("txtacctcode")
                            txtacct = gvRow.FindControl("txtacct")
                            txtcostcentercode = gvRow.FindControl("txtcostcentercode")
                            txtcostcentername = gvRow.FindControl("txtcostcentername")

                            txtcurrencycode = gvRow.FindControl("txtcurrencycode")
                            txtcurrencyname = gvRow.FindControl("txtcurrencyname")

                            txtConvRate = gvRow.FindControl("txtConvRate")
                            txtcontrolacctcode = gvRow.FindControl("txtcontrolacctcode")
                            txtcontrolacct = gvRow.FindControl("txtcontrolacct")
                            txtDebit = gvRow.FindControl("txtDebit")
                            txtCredit = gvRow.FindControl("txtCredit")

                            txtBaseDebit = gvRow.FindControl("txtBaseDebit")
                            txtBaseCredit = gvRow.FindControl("txtBaseCredit")

                            'ddldept = gvRow.FindControl("ddldept")


                            txtnarr = gvRow.FindControl("txtnarr")

                            txtacctcode = gvRow.FindControl("txtacctcode")
                            txtsourcectrycode = gvRow.FindControl("txtsourcectrycode")
                            txtsource = gvRow.FindControl("txtsource")
                            txtbookingno = gvRow.FindControl("txtbookingno")

                            txtOldLineno.Value = lblno.Value

                            ddlAccType.Value = mySqlReader("journal_acc_type").ToString.Trim

                            'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlgAccCode, "Code", "des", "select Code,des from view_account where div_code='" & ViewState("divcode") & "' and type = '" & ddlAccType.Value & "'   order by code", True)
                            'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlgAccName, "des", "Code", "select Code,des from view_account where div_code='" & ViewState("divcode") & "' and type = '" & ddlAccType.Value & "'  order by code", True)


                            txtacctcode.Text = mySqlReader("journal_acc_code").ToString
                            txtacct.Text = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select des from view_account where div_code='" & ViewState("divcode") & "' and type = '" & ddlAccType.Value & "' and code ='" & mySqlReader("journal_acc_code").ToString & "' ")

                            'txtacctname.Value = mySqlReader("journal_acc_code").ToString
                            'txtacctcode.Value = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select des from view_account where div_code='" & ViewState("divcode") & "' and type = '" & ddlAccType.Value & "' and code ='" & mySqlReader("journal_acc_code").ToString & "' ")

                            txtcontrolacctcode.Enabled = False
                            txtcontrolacct.Enabled = False
                            'ddlCCCode.Disabled = True
                            'ddlCCName.Disabled = True
                            txtConvRate.Disabled = False

                            If ddlAccType.Value = "G" Then
                                'sqlstr1 = " select ''  as controlacctcode, '' as acctname  "
                                'sqlstr2 = " select  '' as acctname , '' as controlacctcode "

                                txtcontrolacctcode.Enabled = False
                                txtcontrolacct.Enabled = False
                                'ddlCCCode.Disabled = False
                                'ddlCCName.Disabled = False
                            ElseIf ddlAccType.Value = "C" Then
                                'sqlstr1 = " select distinct view_account.controlacctcode, acctmast.acctname  from acctmast ,view_account where  acctmast.div_code=view_account.div_code and acctmast.div_code='" & ViewState("divcode") & "' and   view_account.controlacctcode= acctmast.acctcode  and type= '" + ddlAccType.Value + "' and view_account.code='" + ddlgAccName.Value + "' order by  view_account.controlacctcode"
                                'sqlstr2 = " select distinct  acctmast.acctname,view_account.controlacctcode  from acctmast ,view_account where acctmast.div_code=view_account.div_code and acctmast.div_code='" & ViewState("divcode") & "' and   view_account.controlacctcode= acctmast.acctcode  and type= '" + ddlAccType.Value + "' and view_account.code='" + ddlgAccName.Value + "' order by  acctmast.acctname"
                            ElseIf ddlAccType.Value = "S" Then
                                'sqlstr1 = " select distinct partymast.controlacctcode  , acctmast.acctname  from acctmast ,partymast where acctmast.div_code='" & ViewState("divcode") & "' and  partymast.controlacctcode= acctmast.acctcode   and partymast.partycode='" + ddlgAccName.Value + "' order by controlacctcode"
                                'sqlstr2 = " select distinct acctmast.acctname ,partymast.controlacctcode      from acctmast ,partymast where  acctmast.div_code='" & ViewState("divcode") & "' and  partymast.controlacctcode= acctmast.acctcode   and partymast.partycode='" + ddlgAccName.Value + "' order by acctmast.acctname"
                            ElseIf ddlAccType.Value = "A" Then
                                'sqlstr1 = " select distinct supplier_agents.controlacctcode    , acctmast.acctname  from acctmast ,supplier_agents where acctmast.div_code='" & ViewState("divcode") & "' and  supplier_agents.controlacctcode= acctmast.acctcode   and supplier_agents.supagentcode='" + ddlgAccName.Value + "' order by controlacctcode"
                                'sqlstr2 = " select distinct acctmast.acctname ,supplier_agents.controlacctcode     from acctmast ,supplier_agents where  acctmast.div_code='" & ViewState("divcode") & "' and  supplier_agents.controlacctcode= acctmast.acctcode   and supplier_agents.supagentcode='" + ddlgAccName.Value + "' order by acctmast.acctname"
                            End If
                            'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlConAccCode, "controlacctcode", "acctname", sqlstr1, True)
                            'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlConAccName, "acctname", "controlacctcode", sqlstr2, True)

                            If ddlAccType.Value <> "G" Then
                                txtcontrolacctcode.Text = mySqlReader("journal_gl_code").ToString
                                txtcontrolacct.Text = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select acctname from acctmast where div_code='" & ViewState("divcode") & "' and  acctcode ='" & mySqlReader("journal_gl_code").ToString & "' ")
                                'txtcontrolacname.Value = mySqlReader("journal_gl_code").ToString
                                'txtctrolaccode.Value = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select acctname from acctmast where div_code='" & ViewState("divcode") & "' and acctcode ='" & mySqlReader("journal_gl_code").ToString & "' ")
                            Else

                                txtcontrolacctcode.Text = ""
                                txtcontrolacct.Text = ""
                            End If
                            'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCCCode, "costcenter_code", "costcenter_name", "select costcenter_code, costcenter_name from costcenter_master where active=1 order by costcenter_code ", True)
                            'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCCName, "costcenter_name", "costcenter_code", "select costcenter_code, costcenter_name from costcenter_master where active=1 order by costcenter_name ", True)

                            txtcostcentercode.Text = mySqlReader("costcenter_code").ToString ' objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select costcenter_name from costcenter_master where costcenter_code ='" & mySqlReader("costcenter_code").ToString & "' ")
                            'ddlCCName.Value = mySqlReader("costcenter_code").ToString

                            '15122014

                            'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddldept, "othmaingrpname", "othmaingrpcode", "select othmaingrpcode,othmaingrpname from othmaingrpmast where active=1 order by othmaingrpcode ", True)
                            'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddldept, "plgrpname", "plgrpcode", "select plgrpname,plgrpcode from plgrpmast where active=1 order by plgrpname", True)

                            'ddldept.Value = mySqlReader("dept").ToString

                            'Dim glCode As String
                            'glCode = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"),"select isnull(controlacctcode,0) from dbo.view_account where type='" & ddlAccType.Value & "'and code='" & ddlgAccCode.Items(ddlgAccCode.SelectedIndex).Text & "'")
                            'If ddlAccType.Value = "G" Then
                            '    ddlgControlAcc.Value = "Select"
                            'Else
                            '    ddlgControlAcc.Value = mySqlReader("journal_acc_code").ToString
                            'End If

                            txtsourcectrycode.Text = mySqlReader("srcctrycode").ToString
                            txtsource.Text = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select ctryname from ctrymast where  ctrycode ='" & mySqlReader("srcctrycode").ToString & "' ")
                            txtbookingno.Text = mySqlReader("BookingNo").ToString

                            txtcurrencycode.Value = mySqlReader("journal_currency_id").ToString
                            txtcurrencyname.Text = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select currname from currmast where  currcode ='" & mySqlReader("journal_currency_id").ToString & "' ")
                            txtConvRate.Value = mySqlReader("journal_currency_rate").ToString
                            txtnarr.Text = mySqlReader("journal_narration").ToString

                            txtCredit.Value = DecRound(mySqlReader("journal_credit").ToString)
                            txtDebit.Value = DecRound(mySqlReader("journal_debit").ToString)

                            txtBaseCredit.Value = DecRound(mySqlReader("basecredit").ToString)
                            txtBaseDebit.Value = DecRound(mySqlReader("basedebit").ToString)


                            credittot = CType(Val(credittot), Decimal) + CType(Val(txtCredit.Value), Decimal)
                            debittot = CType(Val(debittot), Decimal) + CType(Val(txtDebit.Value), Decimal)
                            basecredittot = CType(Val(basecredittot), Decimal) + CType(Val(txtBaseCredit.Value), Decimal)
                            basedebittot = CType(Val(basedebittot), Decimal) + CType(Val(txtBaseDebit.Value), Decimal)

                            If ViewState("JournalState") = "Edit" Or ViewState("JournalState") = "Copy" Then
                                If Trim(txtbasecurr.Value) = Trim(txtcurrencycode.Value) Then
                                    txtConvRate.Disabled = True
                                End If
                            End If
                            Exit For
                        End If
                    Next
                End While
            End If
            txtTotalCredit.Value = DecRound(credittot)
            txtTotalDebit.Value = DecRound(debittot)
            txtTotBaseCredit.Value = DecRound(basecredittot)
            txtTotBaseDebit.Value = DecRound(basedebittot)

            mySqlReader.Close()
            SqlConn.Close()
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("Journal.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            clsDBConnect.dbCommandClose(myCommand)
            clsDBConnect.dbReaderClose(mySqlReader)
            clsDBConnect.dbConnectionClose(SqlConn)
        End Try
    End Sub

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
    '#Region "Public Sub fillcollection(ByVal tranid As String, ByVal lineno As Integer)"
    '    Public Sub fillcollection(ByVal tranid As String)

    '        Dim clAdBill As New Collection
    '        Dim strLineKey As String
    '        Dim intLineNo As Long = 1
    '        Dim myDS As New DataSet
    '        Dim mySqlReader As SqlDataReader

    '        SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
    '        myCommand = New SqlCommand("select * from  open_detail Where against_tran_id='" & tranid & "' ", SqlConn)
    '        mySqlReader = myCommand.ExecuteReader()
    '        If mySqlReader.HasRows Then
    '            While mySqlReader.Read()
    '                strLineKey = intLineNo & ":" & mySqlReader("against_tran_lineno")
    '                AddCollection(clAdBill, "LinNo" & strLineKey, intLineNo.ToString)
    '                AddCollection(clAdBill, "TranId" & strLineKey, mySqlReader("tran_id"))
    '                AddCollection(clAdBill, "TranDate" & strLineKey, mySqlReader("tran_date"))
    '                AddCollection(clAdBill, "TranType" & strLineKey, mySqlReader("tran_type"))
    '                AddCollection(clAdBill, "DueDate" & strLineKey, mySqlReader("open_due_date"))

    '                AddCollection(clAdBill, "Credit" & strLineKey, mySqlReader("open_credit"))
    '                AddCollection(clAdBill, "Debit" & strLineKey, mySqlReader("open_debit"))

    '                AddCollection(clAdBill, "RefNo" & strLineKey, mySqlReader("open_field1"))
    '                AddCollection(clAdBill, "Field2" & strLineKey, mySqlReader("open_field2"))
    '                AddCollection(clAdBill, "Field3" & strLineKey, mySqlReader("open_field3"))
    '                AddCollection(clAdBill, "Field4" & strLineKey, mySqlReader("open_field4"))
    '                AddCollection(clAdBill, "Field5" & strLineKey, mySqlReader("open_field5"))
    '                AddCollection(clAdBill, "OpenMode" & strLineKey, mySqlReader("open_mode"))
    '                intLineNo = intLineNo + 1
    '            End While
    '        End If
    '        myCommand.Dispose()
    '        SqlConn.Close()

    '        Session.Add("Collection", clAdBill)

    '    End Sub
    '#End Region
#Region "Public Sub fillcollection(ByVal tranid As String, ByVal lineno As Integer)"
    Public Sub fillcollection(ByVal tranid As String)
        Dim mySqlReader As SqlDataReader
        Try


            Dim clAdBill As New Collection
            Dim strLineKey As String
            Dim intLineNo As Long = 1
            Dim MainRowct As Long
            Dim MainRowindex As Long
            Dim myDS As New DataSet

            Dim rowbasetotal As Decimal
            MainRowct = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "Select count(*) from journal_detail Where  div_id='" & ViewState("divcode") & "' and  journal_acc_type<>'G' and tran_id='" & tranid & "' and  tran_type='" & CType(ViewState("JournalTranType"), String) & "'")
            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
            myCommand = New SqlCommand("select * from  open_detail Where div_id='" & ViewState("divcode") & "' and  against_tran_id='" & tranid & "' and against_tran_type='" & CType(ViewState("JournalTranType"), String) & "' order by against_tran_lineno,tran_lineno ", SqlConn)


            Dim dataTable As New DataTable()

            Dim myDataAdapter As New SqlDataAdapter
            myDataAdapter = New SqlDataAdapter(myCommand)

            myDataAdapter.Fill(myDS)
            If myDS.Tables(0).Rows.Count > 0 Then

                Session("opendetail_records_jv") = CType(myDS.Tables(0), DataTable)
            End If
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
                    rowbasetotal = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select case  when isnull(basedebit,0)<> 0 then basedebit else   basecredit   end basetotal  from journal_detail Where  div_id='" & ViewState("divcode") & "' and   journal_acc_type <>'G'  and tran_id='" & tranid & "' and  tran_type='" & CType(ViewState("JournalTranType"), String) & "' and tran_lineno='" & mySqlReader("against_tran_lineno") & "'")

                    AddCollection(clAdBill, "AdjustBaseTotal" & strLineKey, DecRound(rowbasetotal)) 'mySqlReader("Acc_GL_Code"))

                    MainRowindex = mySqlReader("against_tran_lineno")
                    intLineNo = intLineNo + 1
                End While
            End If
            'Session.Add("Collection", clAdBill)
            Session.Add("Collection" & ":" & txtAdjcolno.Value, clAdBill)
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("Journal.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            clsDBConnect.dbCommandClose(myCommand)
            clsDBConnect.dbReaderClose(mySqlReader)
            clsDBConnect.dbConnectionClose(SqlConn)
        End Try

    End Sub
#End Region
#Region "Public Sub fillgrd(ByVal grd As GridView, Optional ByVal blnload As Boolean = False, Optional ByVal count As Integer = 1)"
    Public Sub fillDategrd(ByVal grd As GridView, Optional ByVal blnload As Boolean = False, Optional ByVal count As Integer = 1)
        Dim lngcnt As Long
        If blnload = True Then
            lngcnt = 10
        Else
            lngcnt = count
        End If
        grd.DataSource = CreateDataSource(lngcnt)
        grd.DataBind()
        grd.Visible = True
        txtgridrows.Value = grd.Rows.Count
    End Sub
#End Region
#Region "Private Function CreateDataSource(ByVal lngcount As Long) As DataView"
    Private Function CreateDataSource(ByVal lngcount As Long) As DataView
        Dim dt As DataTable
        Dim dr As DataRow
        Dim i As Integer
        dt = New DataTable
        dt.Columns.Add(New DataColumn("LineNo", GetType(Integer)))

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
#Region "Protected Sub grdAcc_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdAcc.RowDataBound"
    Protected Sub grdJournal_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdJournal.RowDataBound

        Dim ddlAccType As HtmlSelect
        Dim txtacct As TextBox
        Dim txtacctcode As TextBox
        Dim txtcontrolacctcode As TextBox
        Dim txtcontrolacct As TextBox
        Dim txtnarr As TextBox
        Dim txtcostcentercode As TextBox
        Dim txtcostcentername As TextBox
        Dim txtcurrencycode As HtmlInputText
        Dim txtcurrencyname As TextBox
        Dim txtConvRate As HtmlInputText
        Dim txtsourcectrycode As TextBox
        Dim txtsource As TextBox
        Dim txtacct_AutoCompleteExtender As New AjaxControlToolkit.AutoCompleteExtender
        Dim ddldept As HtmlSelect
        Dim txtDebit, txtBaseDebit, txtCredit, txtBaseCredit, txtOldLineno As HtmlInputText
        Dim btnBill As HtmlInputButton
        Dim chckDeletion As CheckBox
        Dim lblno As HtmlInputText

        Dim strOpti, sqlstr1, sqlstr2 As String
        Dim i As Integer = 0
        gvRow = e.Row

        gvRow = e.Row
        If e.Row.RowIndex = -1 Then
            strOpti = objUtils.GetDBFieldFromLongnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", 457)
            gvRow.Cells(grd_col.BaseDebit).Text = " Debit(" & strOpti & ")"
            gvRow.Cells(grd_col.BaseCredit).Text = " Credit(" & strOpti & ")"
            Exit Sub
        End If

        txtacctcode = gvRow.FindControl("txtacctcode")
        txtacct = gvRow.FindControl("txtacct")
        txtcontrolacctcode = gvRow.FindControl("txtcontrolacctcode")
        txtcontrolacct = gvRow.FindControl("txtcontrolacct")
        txtnarr = gvRow.FindControl("txtnarr")
        txtcostcentercode = gvRow.FindControl("txtcostcentercode")
        txtcostcentername = gvRow.FindControl("txtcostcentername")
        txtcurrencycode = gvRow.FindControl("txtcurrencycode")
        txtcurrencyname = gvRow.FindControl("txtcurrencyname")
        txtConvRate = gvRow.FindControl("txtConvRate")
        txtsourcectrycode = gvRow.FindControl("txtsourcectrycode")
        txtsource = gvRow.FindControl("txtsource")
        ddlAccType = gvRow.FindControl("ddlType")
        ddldept = gvRow.FindControl("ddldept")
        txtDebit = gvRow.FindControl("txtDebit")
        txtCredit = gvRow.FindControl("txtCredit")
        txtBaseDebit = gvRow.FindControl("txtBaseDebit")
        txtBaseCredit = gvRow.FindControl("txtBaseCredit")
        btnBill = gvRow.FindControl("btnAd")
        lblno = gvRow.FindControl("txtlineno")
        txtOldLineno = gvRow.FindControl("txtOldLineno")
        chckDeletion = gvRow.FindControl("chckDeletion")
        txtacct_AutoCompleteExtender = gvRow.FindControl("txtacct_AutoCompleteExtender")
        'If Session("TabIndex") <> Nothing Then
        '    intTabindex = Session("TabIndex")

        'Else
        '    intTabindex = 10
        'End If
        'intTabindex += 1
        'ddlAccType.Attributes.Add("TabIndex", intTabindex)
        'intTabindex += 1
        'ddlgAccCode.Attributes.Add("TabIndex", intTabindex)
        'intTabindex += 1
        'ddlConAccCode.Attributes.Add("TabIndex", intTabindex)
        'intTabindex += 1
        'txtauto.Attributes.Add("TabIndex", intTabindex)
        'intTabindex += 1
        'ddlgAccName.Attributes.Add("TabIndex", intTabindex)
        'intTabindex += 1
        'ddlConAccName.Attributes.Add("TabIndex", intTabindex)
        'intTabindex += 1
        'ddlCCCode.Attributes.Add("TabIndex", intTabindex)
        'intTabindex += 1
        'ddlCCName.Attributes.Add("TabIndex", intTabindex)
        'intTabindex += 1
        'txtgnarration.Attributes.Add("TabIndex", intTabindex)
        'intTabindex += 1
        'txtCurrCode.Attributes.Add("TabIndex", intTabindex)
        'intTabindex += 1
        'txtCurrRate.Attributes.Add("TabIndex", intTabindex)
        'intTabindex += 1
        'txtDebit.Attributes.Add("TabIndex", intTabindex)
        'intTabindex += 1
        'txtCredit.Attributes.Add("TabIndex", intTabindex)
        'intTabindex += 1
        'txtBaseDebit.Attributes.Add("TabIndex", intTabindex)
        'intTabindex += 1
        'txtBaseCredit.Attributes.Add("TabIndex", intTabindex)
        'intTabindex += 1
        'btnBill.Attributes.Add("TabIndex", intTabindex)
        'intTabindex += 1
        'chckDeletion.Attributes.Add("TabIndex", intTabindex)

        Session("TabIndex") = intTabindex
        objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlAccType, "acc_type_des", "acc_type_name", "select acc_type_des,acc_type_name from  acc_type_master where acc_type_mode<>'G' order by acc_type_name", True)

        ddlAccType.Attributes.Add("onchange", "javascript:fill_acountcode('" + CType(ddlAccType.ClientID, String) + "','" + CType(txtacct_AutoCompleteExtender.ClientID, String) + "','" + CType(txtacctcode.ClientID, String) + "','" + CType(txtacct.ClientID, String) + "','" + CType(txtcontrolacctcode.ClientID, String) + "','" + CType(txtcontrolacct.ClientID, String) + "','" + CType(txtnarr.ClientID, String) + "','" + CType(txtcostcentercode.ClientID, String) + "','" + CType(txtcostcentername.ClientID, String) + "','" + CType(txtcurrencycode.ClientID, String) + "','" + CType(txtcurrencyname.ClientID, String) + "','" + CType(txtConvRate.ClientID, String) + "','" + CType(txtDebit.ClientID, String) + "','" + CType(txtBaseDebit.ClientID, String) + "','" + CType(txtCredit.ClientID, String) + "','" + CType(txtBaseCredit.ClientID, String) + "','" + CType(txtsourcectrycode.ClientID, String) + "','" + CType(txtsource.ClientID, String) + "')") '





        txtDebit.Attributes.Add("onchange", "javascript:convertInRate('" + CType(txtDebit.ClientID, String) + "','" + CType(txtBaseDebit.ClientID, String) + "','" + CType(txtCredit.ClientID, String) + "','" + CType(txtBaseCredit.ClientID, String) + "','" + CType(txtConvRate.ClientID, String) + "','" + CType("Debit", String) + "')")
        txtCredit.Attributes.Add("onchange", "javascript:convertInRate('" + CType(txtDebit.ClientID, String) + "','" + CType(txtBaseDebit.ClientID, String) + "','" + CType(txtCredit.ClientID, String) + "','" + CType(txtBaseCredit.ClientID, String) + "','" + CType(txtConvRate.ClientID, String) + "','" + CType("Credit", String) + "')")
        txtConvRate.Attributes.Add("onchange", "javascript:convertInRate('" + CType(txtDebit.ClientID, String) + "','" + CType(txtBaseDebit.ClientID, String) + "','" + CType(txtCredit.ClientID, String) + "','" + CType(txtBaseCredit.ClientID, String) + "','" + CType(txtConvRate.ClientID, String) + "','" + CType("Currate", String) + "')")
        btnBill.Attributes.Add("onClick", "javascript:openAdjustBill('" + CType(txtacctcode.ClientID, String) + "','" + CType(txtcontrolacctcode.ClientID, String) + "','" + CType(ddlAccType.ClientID, String) + "','" + CType(txtDocNo.ClientID, String) + "','" + CType(lblno.ClientID, String) + "','" + CType(txtcurrencycode.ClientID, String) + "','" + CType(txtConvRate.ClientID, String) + "','" + CType(txtCredit.ClientID, String) + "','" + CType(txtBaseCredit.ClientID, String) + "','" + CType(txtOldLineno.ClientID, String) + "','" + CType(txtDebit.ClientID, String) + "','" + CType(txtBaseDebit.ClientID, String) + "','" + CType(txtsource.ClientID, String) + "')")
        'btnBill.Attributes.Add("onClick", "javascript:openAdjustBill('" + CType(ddlgAccName.ClientID, String) + "','" + CType(ddlAccType.ClientID, String) + "','" + CType(txtDocNo.ClientID, String) + "','" + CType(lblno.ClientID, String) + "','" + CType(txtCurrCode.ClientID, String) + "','" + CType(txtCurrRate.ClientID, String) + "','" + CType(txtBaseCredit.ClientID, String) + "','" + CType(txtBaseDebit.ClientID, String) + "')")


        txtBaseDebit.Attributes.Add("onchange", "javascript:convertInRateBase('" + CType(txtDebit.ClientID, String) + "','" + CType(txtBaseDebit.ClientID, String) + "','" + CType(txtCredit.ClientID, String) + "','" + CType(txtBaseCredit.ClientID, String) + "','" + CType(txtConvRate.ClientID, String) + "','" + CType("Debit", String) + "')")
        txtBaseCredit.Attributes.Add("onchange", "javascript:convertInRateBase('" + CType(txtDebit.ClientID, String) + "','" + CType(txtBaseDebit.ClientID, String) + "','" + CType(txtCredit.ClientID, String) + "','" + CType(txtBaseCredit.ClientID, String) + "','" + CType(txtConvRate.ClientID, String) + "','" + CType("Credit", String) + "')")


        'TextLockhtml(txtBaseDebit)
        'TextLockhtml(txtBaseCredit)
        NumbersHtml(txtBaseDebit)
        NumbersHtml(txtBaseCredit)
        NumbersHtml(txtConvRate)

        NumbersDecimalRoundHtml(txtCredit)
        NumbersDecimalRoundHtml(txtDebit)

        Dim typ As Type
        typ = GetType(DropDownList)
        ' If (Page.ClientScript.IsClientScriptIncludeRegistered(typ, "TADDScript.js")) = False Then
        Page.ClientScript.RegisterClientScriptInclude(typ, "TADDScript.js", "TADDScript.js")
        ddlAccType.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")


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
        cacc.tran_mode = IIf(ViewState("JournalState") = "New", 1, 2)
        mbasecurrency = CType(objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id=457"), String)
        cacc.start()

    End Sub
    'Private Sub Save_Pending_invoices(state As String, sqlconn As SqlConnection, sqltrans As SqlTransaction, strdiv)



    '    If state = "Edit" Then
    '        If Not Session("Purchase_records") Is Nothing Then
    '            Dim JnRecdataTable As DataTable = DirectCast(Session("Journal_records"), DataTable)
    '            Dim filterExpression As String = " divcode =str'  against_tran_id =" & txtDocNo.Value & "  and against_tran_lineno= " & intReceiptLinNo & " and   against_tran_type='" & CType(ViewState("ReceiptsRVPVTranType"), String) & "' And  div_id = " & strdiv & " "
    '            Dim filteredRows() As DataRow = JnRecdataTable.Select(filterExpression)
    '            Dim advpayment As Integer = 0
    '            ' If ViewState("ReceiptsState") = "Edit" Then


    '            For Each row As DataRow In filteredRows
    '                myCommand = New SqlCommand("sp_reverse_pending_invoices", sqlconn, sqltrans)
    '                myCommand.CommandType = CommandType.StoredProcedure
    '                myCommand.Parameters.Add(New SqlParameter("@div_id", SqlDbType.VarChar, 10)).Value = strdiv
    '                myCommand.Parameters.Add(New SqlParameter("@doclineno", SqlDbType.Int)).Value = CType(row("TRAN_LINENO"), String)
    '                'If CType(row("tran_type"), String) = "RV" Then 'collectionDate("TranType" & strLineKey).ToString = "RV" Then
    '                myCommand.Parameters.Add(New SqlParameter("@docno", SqlDbType.VarChar, 20)).Value = CType(row("tran_id"), String) ' collectionDate("TranId" & strLineKey).ToString
    '                myCommand.Parameters.Add(New SqlParameter("@doctype ", SqlDbType.VarChar, 10)).Value = CType(row("tran_type"), String) 'collectionDate("TranType" & strLineKey).ToString
    '                advpayment = 1

    '                myCommand.Parameters.Add(New SqlParameter("@acc_code", SqlDbType.VarChar, 20)).Value = CType(row("ACC_CODE"), String) ' collectionDate("AccCode" & strLineKey).ToString '--ddlgAccCode.Text
    '                myCommand.Parameters.Add(New SqlParameter("@acc_type", SqlDbType.VarChar, 1)).Value = CType(row("ACC_TYPE"), String) 'collectionDate("AccType" & strLineKey).ToString
    '                myCommand.Parameters.Add(New SqlParameter("@debit", SqlDbType.Money)).Value = DecRound(CType(row("open_Debit"), Decimal))   ' DecRound(CType(objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select sum(isnull(open_debit,0))open_debit  from open_detail od(nolock)  where od.against_tran_id =" & txtDocNo.Value & "  and od.against_tran_lineno= " & intReceiptLinNo & " and  od.against_tran_type='" & CType(ViewState("ReceiptsRVPVTranType"), String) & "' And od.div_id = " & strdiv & " "), Decimal))
    '                myCommand.Parameters.Add(New SqlParameter("@credit", SqlDbType.Money)).Value = DecRound(CType(row("open_credit"), Decimal)) 'DecRound(CType(objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select sum(isnull(open_credit,0))open_credit from open_detail od(nolock)  where od.against_tran_id =" & txtDocNo.Value & "  and od.against_tran_lineno= " & intReceiptLinNo & " and  od.against_tran_type='" & CType(ViewState("ReceiptsRVPVTranType"), String) & "' And od.div_id = " & strdiv & " "), Decimal))
    '                myCommand.Parameters.Add(New SqlParameter("@basedebit", SqlDbType.Money)).Value = DecRound(CType(row("base_Debit"), Decimal)) ' DecRound(CType(objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select sum(isnull(base_debit,0))basedebit from open_detail od(nolock)  where od.against_tran_id =" & txtDocNo.Value & "  and od.against_tran_lineno= " & intReceiptLinNo & " and  od.against_tran_type='" & CType(ViewState("ReceiptsRVPVTranType"), String) & "' And od.div_id = " & strdiv & " "), Decimal))
    '                myCommand.Parameters.Add(New SqlParameter("@basecredit", SqlDbType.Money)).Value = DecRound(CType(row("base_credit"), Decimal)) 'DecRound(CType(objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select sum(isnull(base_credit,0))basecredit from open_detail od(nolock)  where od.against_tran_id =" & txtDocNo.Value & "  and od.against_tran_lineno= " & intReceiptLinNo & " and  od.against_tran_type='" & CType(ViewState("ReceiptsRVPVTranType"), String) & "' And od.div_id = " & strdiv & " "), Decimal))

    '                '  End If
    '                myCommand.ExecuteNonQuery()
    '            Next
    '        End If
    '    End If
    '    Dim dataTable As DataTable = DirectCast(Session("Adjustedrecords"), DataTable)
    '    If dataTable.Rows.Count > 0 Then
    '        For Each row As DataRow In dataTable.Rows
    '            myCommand = New SqlCommand("sp_update_Pending_Invoices", sqlconn, sqltrans)
    '            myCommand.CommandType = CommandType.StoredProcedure
    '            myCommand.Parameters.Add(New SqlParameter("@div_id", SqlDbType.VarChar, 10)).Value = strdiv
    '            myCommand.Parameters.Add(New SqlParameter("@doclineno", SqlDbType.Int)).Value = CType(row("tran_lineno"), Integer)
    '            myCommand.Parameters.Add(New SqlParameter("@docno", SqlDbType.VarChar, 20)).Value = CType(row("TRANID"), String)
    '            myCommand.Parameters.Add(New SqlParameter("@doctype ", SqlDbType.VarChar, 10)).Value = CType(row("tran_type"), String)
    '            If ddlCashBank.Value = "Bank" Then
    '                myCommand.Parameters.Add(New SqlParameter("@docdate", SqlDbType.DateTime)).Value = Format(CType(txtChequeDate.Text, Date), "yyyy/MM/dd")
    '            Else
    '                myCommand.Parameters.Add(New SqlParameter("@docdate", SqlDbType.DateTime)).Value = Format(CType(txtDate.Text, Date), "yyyy/MM/dd")
    '            End If


    '            myCommand.Parameters.Add(New SqlParameter("@acc_code", SqlDbType.VarChar, 20)).Value = CType(row("Acc_Code"), String) ' collectionDate("AccCode" & strLineKey).ToString '--ddlgAccCode.Text
    '            myCommand.Parameters.Add(New SqlParameter("@acc_type", SqlDbType.VarChar, 1)).Value = CType(row("acc_type"), String) 'collectionDate("AccType" & strLineKey).ToString
    '            If UCase(CType(row("tran_type"), String).ToString) = "RV" Then
    '                myCommand.Parameters.Add(New SqlParameter("@against_tran_id ", SqlDbType.VarChar, 20)).Value = txtDocNo.Value
    '                myCommand.Parameters.Add(New SqlParameter("@against_tran_type", SqlDbType.VarChar, 10)).Value = CType(ViewState("ReceiptsRVPVTranType"), String)
    '                myCommand.Parameters.Add(New SqlParameter("@against_tran_lineno", SqlDbType.Int)).Value = CType(row("against_tran_lineno"), Integer) ' lblno.Value 'collectionDate("AgainstTranLineNo" & strLineKey) 'intReceiptLinNo
    '            Else
    '                myCommand.Parameters.Add(New SqlParameter("@against_tran_id ", SqlDbType.VarChar, 20)).Value = txtDocNo.Value
    '                myCommand.Parameters.Add(New SqlParameter("@against_tran_type", SqlDbType.VarChar, 10)).Value = ""
    '                myCommand.Parameters.Add(New SqlParameter("@against_tran_lineno", SqlDbType.Int)).Value = DBNull.Value
    '            End If


    '            myCommand.Parameters.Add(New SqlParameter("@acc_gl_code", SqlDbType.VarChar, 20)).Value = CType(row("acc_gl_code"), String)  'collectionDate("AccGLCode" & strLineKey).ToString 'ddlConAccCode.Text

    '            If CType(row("Acc_Code"), String).ToString = "" Then
    '                myCommand.Parameters.Add(New SqlParameter("@DUEDATE ", SqlDbType.DateTime)).Value = Format(CType(txtDate.Text, Date), "yyyy/MM/dd") 'DBNull.Value
    '            Else
    '                myCommand.Parameters.Add(New SqlParameter("@DUEDATE ", SqlDbType.DateTime)).Value = CType(row("open_due_date"), String) 'Format(CType(collectionDate("DueDate" & strLineKey).ToString, Date), "yyyy/MM/dd")
    '            End If

    '            myCommand.Parameters.Add(New SqlParameter("@currrate", SqlDbType.Decimal, 18, 12)).Value = CType(row("currency_rate"), Decimal)   'CType(collectionDate("CurrRate" & strLineKey), Decimal) 'CType(txtCurrRate.Value, Decimal)
    '            myCommand.Parameters.Add(New SqlParameter("@field1", SqlDbType.VarChar, 500)).Value = CType(row("open_field1"), String) '- -collectionDate("AccType" & strLineKey).ToString
    '            myCommand.Parameters.Add(New SqlParameter("@field2", SqlDbType.VarChar, 500)).Value = CType(row("open_field2"), String)
    '            myCommand.Parameters.Add(New SqlParameter("@field3 ", SqlDbType.VarChar, 500)).Value = CType(row("open_field3"), String)
    '            myCommand.Parameters.Add(New SqlParameter("@field4", SqlDbType.VarChar, 500)).Value = CType(row("open_field4"), String)
    '            myCommand.Parameters.Add(New SqlParameter("@field5", SqlDbType.VarChar, 500)).Value = CType(row("open_field5"), String)
    '            If CType(row("Tran_Type"), String).ToString = "RV" Then
    '                'If UCase(collectionDate("TranType" & strLineKey).ToString) = "RV" Then
    '                myCommand.Parameters.Add(New SqlParameter("@accmode", SqlDbType.VarChar, 1)).Value = CType(row("open_mode"), String)
    '            Else
    '                myCommand.Parameters.Add(New SqlParameter("@accmode", SqlDbType.VarChar, 1)).Value = "B"

    '            End If
    '            myCommand.Parameters.Add(New SqlParameter("@open_field3 ", SqlDbType.VarChar, 500)).Value = txtnarration.Text
    '            myCommand.Parameters.Add(New SqlParameter("@currcode", SqlDbType.VarChar, 10)).Value = CType(row("currcode"), String) ' CType(collectionDate("currcode" & strLineKey), Decimal) '"" txtCurrCodegrid.Value.Trim
    '            myCommand.Parameters.Add(New SqlParameter("@acc_State", SqlDbType.VarChar, 10)).Value = "P"
    '            myCommand.Parameters.Add(New SqlParameter("@debit", SqlDbType.Money)).Value = DecRound(CType(row("open_debit"), Decimal)) 'CType(Val(txtDebit.Value), Decimal)
    '            myCommand.Parameters.Add(New SqlParameter("@credit", SqlDbType.Money)).Value = DecRound(CType(row("open_credit"), Decimal))
    '            myCommand.Parameters.Add(New SqlParameter("@basedebit", SqlDbType.Money)).Value = DecRound(CType(row("base_debit"), Decimal))
    '            myCommand.Parameters.Add(New SqlParameter("@basecredit", SqlDbType.Money)).Value = DecRound(CType(row("base_credit"), Decimal))
    '            '  myCommand.Parameters.Add(New SqlParameter("@basecurrency", SqlDbType.Decimal, 18, 12)).Value = 0
    '            myCommand.ExecuteNonQuery()

    '            'Tanvir  02102023 Point 6

    '        Next
    '    End If

    '    ''Tanvir 04102023 point6
    'End Sub
#Region "Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click"
    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Dim intRow As Integer

        Dim ddldept As HtmlSelect

        Dim txtCredit, txtDebit, txtCurrCode, txtCurrRate, txtBaseCredit, txtBaseDebit, lblno As HtmlInputText





        Dim ddlAccType As HtmlSelect
        Dim txtacct As TextBox
        Dim txtacctcode As TextBox
        Dim txtcontrolacctcode As TextBox
        Dim txtcontrolacct As TextBox
        Dim txtnarr As TextBox
        Dim txtcostcentercode As TextBox
        Dim txtcostcentername As TextBox
        Dim txtcurrencycode As HtmlInputText
        Dim txtcurrencyname As TextBox
        Dim txtConvRate As HtmlInputText
        Dim txtsourcectrycode As TextBox
        Dim txtsource As TextBox
        Dim txtbookingno As TextBox

        'Dim ddldept As HtmlSelect


        Dim chckDeletion As CheckBox





        Dim sqlTrans As SqlTransaction
        Dim strdiv, strcostcentercode As String
        Dim docyear As String
        strdiv = objUtils.GetDBFieldFromLongnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", 511)
        Try
            If Page.IsValid = True Then
                If ViewState("JournalState") = "Edit" Or ViewState("JournalState") = "Delete" Or ViewState("JournalState") = "Cancel" Or ViewState("JournalState") = "UndoCancel" Then
                    If validate_BillAgainst() = False Then
                        Exit Sub
                    End If
                End If
                If ViewState("JournalState") = "New" Or ViewState("JournalState") = "Edit" Or ViewState("JournalState") = "Copy" Or ViewState("JournalState") = "UndoCancel" Then
                    If validate_page() = False Then
                        Exit Sub
                    End If
                End If

                If ViewState("JournalState") = "New" Or ViewState("JournalState") = "Edit" Or ViewState("JournalState") = "Delete" Or ViewState("JournalState") = "Cancel" Or ViewState("JournalState") = "UndoCancel" Then
                    If Validateseal() = False Then
                        Exit Sub
                    End If
                End If


                'If ViewState("JournalState") = "New" Or ViewState("JournalState") = "Edit" Or ViewState("JournalState") = "Copy" Then
                '    If chkPost.Checked = True And chkadjust.Checked = False Then
                '        If checkopenmode() = False Then
                '            Exit Sub
                '        End If
                '    End If
                'End If


                strdiv = ViewState("divcode") ' objUtils.GetDBFieldFromLongnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", 511)
                strcostcentercode = objUtils.GetDBFieldFromLongnew(Session("dbconnectionName"), "reservation_parameters", "option_Selected", "param_id", 510)
                SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                sqlTrans = SqlConn.BeginTransaction
                If chkPost.Checked = True Then
                    'For Accounts posting
                    initialclass(SqlConn, sqlTrans)
                    'For Accounts posting
                End If

                If ViewState("JournalState") = "New" Or ViewState("JournalState") = "Edit" Or ViewState("JournalState") = "Copy" Then
                    If ViewState("JournalState") = "New" Or ViewState("JournalState") = "Copy" Then
                        Dim optionval As String
                        optionval = objUtils.GetAutoDocNodiv(CType(ViewState("JournalTranType"), String), SqlConn, sqlTrans, strdiv)

                        'Dim optionval As String
                        'Dim optionType As String = objUtils.ExecuteQueryReturnStringValue("select option_selected from reservation_parameters where param_id=2037")
                        'optionval = objUtils.GetAutoDocNodiv(CType(ViewState("JournalTranType"), String), SqlConn, sqlTrans, strdiv)
                        '' Yearwise number Sequence - Christo.A - 20/12/18
                        ''optionval = objUtils.GetAutoDocNodivYear(CType(ViewState("ReceiptsRVPVTranType"), String), SqlConn, sqlTrans, strdiv, Year(Format(CType(txtDate.Text, Date), "yyyy/MM/dd")))
                        'If LCase(optionType) = "year" Then
                        '    optionval = objUtils.GetAutoDocNodivYear(CType(ViewState("JournalTranType"), String), SqlConn, sqlTrans, strdiv, Year(Format(CType(txtJDate.Text, Date), "yyyy/MM/dd")))
                        'ElseIf LCase(optionType) = "month" Then
                        '    optionval = objUtils.GetAutoDocNodivMonth(CType(ViewState("JournalTranType"), String), SqlConn, sqlTrans, strdiv, UCase(CType(ViewState("JournalTranType"), String)), Month(Format(CType(txtJDate.Text, Date), "yyyy/MM/dd")).ToString("00"), CType(Year(Format(CType(txtJDate.Text, Date), "yyyy/MM/dd")), String))
                        'Else
                        '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('ID generate option type is wrong');", True)
                        '    sqlTrans.Rollback()
                        '    Exit Sub
                        'End If
                        txtDocNo.Value = optionval.Trim

                        'docyear = Trim(Str(Year(txtJDate.Text)))
                        'If CType(docyear, Integer) < 2010 Then 'check if before the 2010 because from 2010 startt new no. for new year
                        '    optionval = objUtils.GetAutoDocNo(CType(ViewState("JournalTranType"), String), SqlConn, sqlTrans)
                        'Else
                        '    optionval = objUtils.GetAutoDocNoyear(CType(ViewState("JournalTranType"), String), SqlConn, sqlTrans, docyear)
                        'End If
                        'txtDocNo.Value = optionval.Trim


                        myCommand = New SqlCommand("sp_add_journal_master", SqlConn, sqlTrans)
                        myCommand.CommandType = CommandType.StoredProcedure
                    ElseIf ViewState("JournalState") = "Edit" Then
                        myCommand = New SqlCommand("sp_mod_journal_master", SqlConn, sqlTrans)
                        myCommand.CommandType = CommandType.StoredProcedure
                    End If


                    myCommand.Parameters.Add(New SqlParameter("@journal_div_id", SqlDbType.VarChar, 10)).Value = strdiv
                    myCommand.Parameters.Add(New SqlParameter("@tran_id", SqlDbType.VarChar, 20)).Value = txtDocNo.Value
                    myCommand.Parameters.Add(New SqlParameter("@tran_type ", SqlDbType.VarChar, 10)).Value = CType(ViewState("JournalTranType"), String)
                    myCommand.Parameters.Add(New SqlParameter("@journal_date ", SqlDbType.DateTime)).Value = objDateTime.ConvertDateromTextBoxToDatabase(txtJDate.Text)
                    myCommand.Parameters.Add(New SqlParameter("@journal_tran_date ", SqlDbType.DateTime)).Value = objDateTime.ConvertDateromTextBoxToDatabase(txtTDate.Text)
                    myCommand.Parameters.Add(New SqlParameter("@journal_mrv", SqlDbType.VarChar, 10)).Value = txtReference.Value.Trim
                    myCommand.Parameters.Add(New SqlParameter("@journal_salesperson_code", SqlDbType.VarChar, 10)).Value = DBNull.Value
                    myCommand.Parameters.Add(New SqlParameter("@journal_narration", SqlDbType.VarChar, 200)).Value = txtnarration.Text
                    myCommand.Parameters.Add(New SqlParameter("@journal_tran_state", SqlDbType.VarChar, 1)).Value = ""
                    myCommand.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                    myCommand.Parameters.Add(New SqlParameter("@adddate", SqlDbType.DateTime)).Value = objDateTime.GetSystemDateTime(Session("dbconnectionName"))

                    If txtTotBaseDebit.Value <> "" Then
                        myCommand.Parameters.Add(New SqlParameter("@basedebit", SqlDbType.Money)).Value = CType(txtTotBaseDebit.Value, Double)
                    Else
                        myCommand.Parameters.Add(New SqlParameter("@basedebit", SqlDbType.Money)).Value = Decimal.Zero
                    End If

                    If txtTotBaseCredit.Value <> "" Then
                        myCommand.Parameters.Add(New SqlParameter("@basecredit", SqlDbType.Money)).Value = CType(txtTotBaseCredit.Value, Double)
                    Else
                        myCommand.Parameters.Add(New SqlParameter("@basecredit", SqlDbType.Money)).Value = Decimal.Zero
                    End If

                    If chkPost.Checked = True Then
                        myCommand.Parameters.Add(New SqlParameter("@post_state", SqlDbType.VarChar, 1)).Value = "P"
                    Else
                        myCommand.Parameters.Add(New SqlParameter("@post_state", SqlDbType.VarChar, 1)).Value = "U"
                    End If

                    '15122014


                    myCommand.Parameters.Add(New SqlParameter("@mktcode", SqlDbType.VarChar, 20)).Value = DBNull.Value


                    myCommand.ExecuteNonQuery()
                    If chkPost.Checked = True Then
                        'For Accounts Posting
                        caccounts.clraccounts()
                        cacc.acc_tran_id = txtDocNo.Value
                        cacc.acc_tran_type = CType(ViewState("JournalTranType"), String)
                        cacc.acc_tran_date = Format(CType(txtTDate.Text, Date), "yyyy/MM/dd")
                        cacc.acc_div_id = strdiv
                    End If


                    If ViewState("JournalState") = "Edit" Then
                        If chkPost.Checked = False Then
                            myCommand = New SqlCommand("sp_delvoucher", SqlConn, sqlTrans)
                            myCommand.CommandType = CommandType.StoredProcedure
                            myCommand.Parameters.Add(New SqlParameter("@tablename", SqlDbType.VarChar, 20)).Value = "journal_master"
                            myCommand.Parameters.Add(New SqlParameter("@tranid", SqlDbType.VarChar, 20)).Value = txtDocNo.Value
                            myCommand.Parameters.Add(New SqlParameter("@trantype ", SqlDbType.VarChar, 10)).Value = CType(ViewState("JournalTranType"), String)
                            myCommand.Parameters.Add(New SqlParameter("@div_id", SqlDbType.VarChar, 10)).Value = strdiv
                            myCommand.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                            myCommand.ExecuteNonQuery()
                        End If

                        'myCommand = New SqlCommand("sp_del_journal_detail", SqlConn, sqlTrans)
                        'myCommand.CommandType = CommandType.StoredProcedure
                        'myCommand.Parameters.Add(New SqlParameter("@div_id", SqlDbType.VarChar, 10)).Value = strdiv
                        'myCommand.Parameters.Add(New SqlParameter("@tran_id", SqlDbType.VarChar, 20)).Value = txtDocNo.Value
                        'myCommand.Parameters.Add(New SqlParameter("@tran_type ", SqlDbType.VarChar, 10)).Value = "JV"
                        'myCommand.Parameters.Add(New SqlParameter("@userlogged ", SqlDbType.VarChar, 10)).Value = DBNull.Value

                        'myCommand.ExecuteNonQuery()

                        myCommand = New SqlCommand("sp_del_open_detail_new", SqlConn, sqlTrans)
                        myCommand.CommandType = CommandType.StoredProcedure
                        myCommand.Parameters.Add(New SqlParameter("@div_id", SqlDbType.VarChar, 10)).Value = strdiv
                        myCommand.Parameters.Add(New SqlParameter("@tran_id", SqlDbType.VarChar, 20)).Value = txtDocNo.Value
                        myCommand.Parameters.Add(New SqlParameter("@tran_type ", SqlDbType.VarChar, 10)).Value = CType(ViewState("JournalTranType"), String)
                        myCommand.ExecuteNonQuery()

                        ' Tanvir 02/10/2023 Point 6

                        If Not Session("opendetail_records_jv") Is Nothing Then
                            Dim dataTable As DataTable = DirectCast(Session("opendetail_records_jv"), DataTable)
                            Dim filterExpression As String = "  div_id = " & strdiv & " " ' and against_tran_lineno=1 ' against_tran_id =" & txtDocNo.Value & "  and   against_tran_type='" & CType(ViewState("ReceiptsRVPVTranType"), String) & "' And 
                            Dim filteredRows() As DataRow = dataTable.Select(filterExpression)
                            Dim advpayment As Integer = 0
                            If ViewState("JournalState") = "Edit" Then
                                If dataTable.Rows.Count > 0 Then


                                    For Each row As DataRow In filteredRows
                                        myCommand = New SqlCommand("sp_reverse_pending_invoices", SqlConn, sqlTrans)
                                        myCommand.CommandType = CommandType.StoredProcedure
                                        myCommand.Parameters.Add(New SqlParameter("@div_id", SqlDbType.VarChar, 10)).Value = strdiv
                                        myCommand.Parameters.Add(New SqlParameter("@doclineno", SqlDbType.Int)).Value = CType(row("TRAN_LINENO"), String)
                                        'If CType(row("tran_type"), String) = "RV" Then 'collectionDate("TranType" & strLineKey).ToString = "RV" Then
                                        myCommand.Parameters.Add(New SqlParameter("@docno", SqlDbType.VarChar, 20)).Value = CType(row("tran_id"), String) ' collectionDate("TranId" & strLineKey).ToString
                                        myCommand.Parameters.Add(New SqlParameter("@doctype ", SqlDbType.VarChar, 10)).Value = CType(row("tran_type"), String) 'collectionDate("TranType" & strLineKey).ToString
                                        advpayment = 1

                                        myCommand.Parameters.Add(New SqlParameter("@acc_code", SqlDbType.VarChar, 20)).Value = CType(row("ACC_CODE"), String) ' collectionDate("AccCode" & strLineKey).ToString '--ddlgAccCode.Text
                                        myCommand.Parameters.Add(New SqlParameter("@acc_type", SqlDbType.VarChar, 1)).Value = CType(row("ACC_TYPE"), String) 'collectionDate("AccType" & strLineKey).ToString
                                        myCommand.Parameters.Add(New SqlParameter("@debit", SqlDbType.Money)).Value = DecRound(CType(row("open_Debit"), Decimal))   ' DecRound(CType(objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select sum(isnull(open_debit,0))open_debit  from open_detail od(nolock)  where od.against_tran_id =" & txtDocNo.Value & "  and od.against_tran_lineno= " & intReceiptLinNo & " and  od.against_tran_type='" & CType(ViewState("ReceiptsRVPVTranType"), String) & "' And od.div_id = " & strdiv & " "), Decimal))
                                        myCommand.Parameters.Add(New SqlParameter("@credit", SqlDbType.Money)).Value = DecRound(CType(row("open_credit"), Decimal)) 'DecRound(CType(objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select sum(isnull(open_credit,0))open_credit from open_detail od(nolock)  where od.against_tran_id =" & txtDocNo.Value & "  and od.against_tran_lineno= " & intReceiptLinNo & " and  od.against_tran_type='" & CType(ViewState("ReceiptsRVPVTranType"), String) & "' And od.div_id = " & strdiv & " "), Decimal))
                                        myCommand.Parameters.Add(New SqlParameter("@basedebit", SqlDbType.Money)).Value = DecRound(CType(row("base_Debit"), Decimal)) ' DecRound(CType(objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select sum(isnull(base_debit,0))basedebit from open_detail od(nolock)  where od.against_tran_id =" & txtDocNo.Value & "  and od.against_tran_lineno= " & intReceiptLinNo & " and  od.against_tran_type='" & CType(ViewState("ReceiptsRVPVTranType"), String) & "' And od.div_id = " & strdiv & " "), Decimal))
                                        myCommand.Parameters.Add(New SqlParameter("@basecredit", SqlDbType.Money)).Value = DecRound(CType(row("base_credit"), Decimal)) 'DecRound(CType(objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select sum(isnull(base_credit,0))basecredit from open_detail od(nolock)  where od.against_tran_id =" & txtDocNo.Value & "  and od.against_tran_lineno= " & intReceiptLinNo & " and  od.against_tran_type='" & CType(ViewState("ReceiptsRVPVTranType"), String) & "' And od.div_id = " & strdiv & " "), Decimal))

                                        '  End If
                                        myCommand.ExecuteNonQuery()
                                    Next
                                End If
                            End If
                        End If
                        ' Tanvir 02/10/2023 Point 6
                    End If

                    'Save In Detail Table
                    For Each gvRow In grdJournal.Rows

                        lblno = gvRow.FindControl("txtlineno")
                        If Val(lblno.Value) = 0 Then
                            lblno.Value = intRow
                        End If







                        lblno = gvRow.FindControl("txtlineno")

                        ddlAccType = gvRow.FindControl("ddlType")
                        txtacct = gvRow.FindControl("txtacct")
                        txtacctcode = gvRow.FindControl("txtacctCode")
                        txtcontrolacctcode = gvRow.FindControl("txtcontrolacctcode")
                        txtcontrolacct = gvRow.FindControl("txtcontrolacct")
                        txtnarr = gvRow.FindControl("txtnarr")
                        txtcostcentercode = gvRow.FindControl("txtcostcentercode")
                        txtcostcentername = gvRow.FindControl("txtcostcentername")
                        txtcurrencycode = gvRow.FindControl("txtcurrencycode")
                        txtcurrencyname = gvRow.FindControl("txtcurrencyname")
                        txtConvRate = gvRow.FindControl("txtConvRate")
                        txtsourcectrycode = gvRow.FindControl("txtsourcectrycode")
                        txtsource = gvRow.FindControl("txtsource")
                        txtbookingno = gvRow.FindControl("txtbookingno")


                        txtDebit = gvRow.FindControl("txtDebit")
                        txtCredit = gvRow.FindControl("txtCredit")
                        txtBaseDebit = gvRow.FindControl("txtBaseDebit")
                        txtBaseCredit = gvRow.FindControl("txtBaseCredit")




                        If ddlAccType.Value <> "[Select]" And txtacctcode.Text <> "" Then
                            intRow = 1 + intRow
                            myCommand = New SqlCommand("sp_add_journal_detail", SqlConn, sqlTrans)
                            myCommand.CommandType = CommandType.StoredProcedure

                            myCommand.Parameters.Add(New SqlParameter("@tran_lineno", SqlDbType.Int)).Value = intRow
                            myCommand.Parameters.Add(New SqlParameter("@tran_id", SqlDbType.VarChar, 20)).Value = txtDocNo.Value
                            myCommand.Parameters.Add(New SqlParameter("@tran_type", SqlDbType.VarChar, 10)).Value = CType(ViewState("JournalTranType"), String)

                            myCommand.Parameters.Add(New SqlParameter("@journal_acc_type", SqlDbType.VarChar, 1)).Value = ddlAccType.Value
                            myCommand.Parameters.Add(New SqlParameter("@journal_acc_code", SqlDbType.VarChar, 20)).Value = txtacctcode.Text
                            myCommand.Parameters.Add(New SqlParameter("@BookingNo", SqlDbType.VarChar, 20)).Value = txtbookingno.Text

                            myCommand.Parameters.Add(New SqlParameter("@srcctrycode", SqlDbType.VarChar, 20)).Value = txtsourcectrycode.Text


                            'If ddlAccType.Value = "G" Then
                            '    myCommand.Parameters.Add(New SqlParameter("@journal_gl_code", SqlDbType.VarChar, 20)).Value = DBNull.Value
                            'Else
                            '    Dim glCode As String
                            '    glCode = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"),"select isnull(controlacctcode,0) from dbo.view_account where type='" & ddlAccType.Value & "'and code='" & ddlgAccCode.Items(ddlgAccCode.SelectedIndex).Text & "'")
                            '    myCommand.Parameters.Add(New SqlParameter("@journal_gl_code", SqlDbType.VarChar, 20)).Value = glCode
                            'End If
                            If ddlAccType.Value <> "G" Then
                                If txtcontrolacctcode.Text <> "" Then
                                    myCommand.Parameters.Add(New SqlParameter("@journal_gl_code", SqlDbType.VarChar, 20)).Value = txtcontrolacctcode.Text
                                Else
                                    myCommand.Parameters.Add(New SqlParameter("@journal_gl_code", SqlDbType.VarChar, 20)).Value = DBNull.Value
                                End If
                            Else
                                myCommand.Parameters.Add(New SqlParameter("@journal_gl_code", SqlDbType.VarChar, 20)).Value = DBNull.Value
                            End If
                            myCommand.Parameters.Add(New SqlParameter("@journal_group", SqlDbType.VarChar, 1)).Value = DBNull.Value
                            myCommand.Parameters.Add(New SqlParameter("@journal_narration", SqlDbType.VarChar, 200)).Value = txtnarr.Text.Trim
                            myCommand.Parameters.Add(New SqlParameter("@journal_currency_id", SqlDbType.VarChar, 20)).Value = txtcurrencycode.Value.Trim
                            myCommand.Parameters.Add(New SqlParameter("@journal_currency_rate", SqlDbType.Decimal, 18.12)).Value = CType(txtConvRate.Value, Decimal)

                            myCommand.Parameters.Add(New SqlParameter("@journal_debit", SqlDbType.Money)).Value = CType(Val(txtDebit.Value), Double)
                            myCommand.Parameters.Add(New SqlParameter("@journal_credit", SqlDbType.Money)).Value = CType(Val(txtCredit.Value), Double)
                            myCommand.Parameters.Add(New SqlParameter("@basedebit", SqlDbType.Money)).Value = CType(Val(txtBaseDebit.Value), Double)
                            myCommand.Parameters.Add(New SqlParameter("@basecredit", SqlDbType.Money)).Value = CType(Val(txtBaseCredit.Value), Double)
                            myCommand.Parameters.Add(New SqlParameter("@div_id", SqlDbType.VarChar, 10)).Value = strdiv

                            '15122014
                            'If ddldept.Items(ddldept.SelectedIndex).Text <> "[Select]" Then
                            '    myCommand.Parameters.Add(New SqlParameter("@dept", SqlDbType.VarChar, 20)).Value = ddldept.Items(ddldept.SelectedIndex).Value
                            'Else
                            myCommand.Parameters.Add(New SqlParameter("@dept", SqlDbType.VarChar, 20)).Value = DBNull.Value
                            'End If



                            If ddlAccType.Value = "G" Then
                                'If txtcostcentercode.Text <> "" Then
                                '    myCommand.Parameters.Add(New SqlParameter("@costcenter_code", SqlDbType.VarChar, 20)).Value = txtcostcentercode.Text
                                'Else
                                myCommand.Parameters.Add(New SqlParameter("@costcenter_code", SqlDbType.VarChar, 20)).Value = strcostcentercode
                                'End If
                            Else
                                myCommand.Parameters.Add(New SqlParameter("@costcenter_code", SqlDbType.VarChar, 20)).Value = DBNull.Value
                            End If

                            myCommand.ExecuteNonQuery()
                            If chkPost.Checked = True Then

                                'Posting for the Grid Accounts
                                ctran = New clstran
                                ctran.acc_tran_id = cacc.acc_tran_id
                                ctran.acc_code = txtacctcode.Text
                                ctran.acc_type = ddlAccType.Value
                                ctran.acc_currency_id = txtcurrencycode.Value.Trim
                                ctran.acc_currency_rate = CType(txtConvRate.Value, Decimal)
                                ctran.acc_div_id = strdiv
                                ctran.acc_narration = txtnarr.Text
                                ctran.acc_tran_date = cacc.acc_tran_date
                                ctran.acc_tran_lineno = lblno.Value
                                ctran.acc_tran_type = cacc.acc_tran_type
                                If txtcontrolacctcode.Text <> "" Then
                                    ctran.pacc_gl_code = txtcontrolacctcode.Text
                                Else
                                    ctran.pacc_gl_code = ""
                                End If
                                ctran.acc_ref1 = ""
                                ctran.acc_ref2 = ""
                                ctran.acc_ref3 = ""
                                ctran.acc_ref4 = ""
                                cacc.addtran(ctran)

                                If ddlAccType.Value = "G" Then
                                    csubtran = New clsSubTran
                                    csubtran.acc_against_tran_id = cacc.acc_tran_id
                                    csubtran.acc_against_tran_lineno = lblno.Value
                                    csubtran.acc_against_tran_type = cacc.acc_tran_type

                                    csubtran.acc_debit = DecRound(CType(txtDebit.Value, Decimal))
                                    csubtran.acc_credit = DecRound(CType(txtCredit.Value, Decimal))
                                    csubtran.acc_base_debit = DecRound(CType(txtBaseDebit.Value, Decimal))
                                    csubtran.acc_base_credit = DecRound(CType(txtBaseCredit.Value, Decimal))
                                    csubtran.acc_tran_date = cacc.acc_tran_date
                                    csubtran.acc_due_date = cacc.acc_tran_date
                                    csubtran.acc_field1 = ""
                                    csubtran.acc_field2 = ""
                                    csubtran.acc_field3 = ""
                                    csubtran.acc_field4 = ""
                                    csubtran.acc_field5 = ""
                                    csubtran.acc_tran_id = cacc.acc_tran_id
                                    csubtran.acc_tran_lineno = lblno.Value
                                    csubtran.acc_tran_type = cacc.acc_tran_type
                                    csubtran.acc_narration = txtnarr.Text
                                    csubtran.acc_type = ddlAccType.Value
                                    csubtran.currate = CType(txtConvRate.Value, Decimal)
                                    'if it is blank then post to default 510
                                    If txtcostcentercode.Text <> "" Then
                                        csubtran.costcentercode = txtcostcentercode.Text
                                    Else
                                        csubtran.costcentercode = strcostcentercode
                                    End If
                                    cacc.addsubtran(csubtran)
                                End If
                            End If
                            Save_Open_detail(lblno.Value, txtacctcode.Text, ddlAccType.Value, txtcontrolacctcode.Text, SqlConn, sqlTrans, txtcurrencycode.Value.Trim)
                        End If
                    Next
                    If chkPost.Checked = True Then
                        'For Accounts Posting
                        cacc.table_name = ""
                        caccounts.Addaccounts(cacc)
                        If caccounts.saveaccounts(Session("dbconnectionName"), SqlConn, sqlTrans, Me.Page) <> 0 Then
                            Err.Raise(vbObjectError + 100)
                        End If


                        Dim dataTable As DataTable = DirectCast(Session("Adjustedrecords"), DataTable)
                        'Dim filterExpression As String = " div_id = '" & strdiv & " '" '  against_tran_id =" & txtDocNo.Value & "  and against_tran_lineno= " & intReceiptLinNo & " and   against_tran_type='" & CType(ViewState("ReceiptsRVPVTranType"), String) & "' And  div_id = " & strdiv & " "
                        'Dim filteredRows() As DataRow = dataTable.Select(filterExpression)
                        'Dim advpayment As Integer = 0
                        '' If ViewState("ReceiptsState") = "Edit" Then
                        If dataTable.Rows.Count > 0 Then
                            For Each row As DataRow In dataTable.Rows
                                myCommand = New SqlCommand("sp_update_Pending_Invoices", SqlConn, sqlTrans)
                                myCommand.CommandType = CommandType.StoredProcedure
                                myCommand.Parameters.Add(New SqlParameter("@div_id", SqlDbType.VarChar, 10)).Value = strdiv
                                myCommand.Parameters.Add(New SqlParameter("@doclineno", SqlDbType.Int)).Value = CType(row("tran_lineno"), Integer)
                                myCommand.Parameters.Add(New SqlParameter("@docno", SqlDbType.VarChar, 20)).Value = CType(row("TRANID"), String)
                                myCommand.Parameters.Add(New SqlParameter("@doctype ", SqlDbType.VarChar, 10)).Value = CType(row("tran_type"), String)
                                'If ddlCashBank.Value = "Bank" Then
                                myCommand.Parameters.Add(New SqlParameter("@docdate", SqlDbType.DateTime)).Value = objDateTime.ConvertDateromTextBoxToDatabase(txtJDate.Text)
                                'Else
                                'myCommand.Parameters.Add(New SqlParameter("@docdate", SqlDbType.DateTime)).Value = Format(CType(txtDate.Text, Date), "yyyy/MM/dd")
                                'End If


                                myCommand.Parameters.Add(New SqlParameter("@acc_code", SqlDbType.VarChar, 20)).Value = CType(row("Acc_Code"), String) ' collectionDate("AccCode" & strLineKey).ToString '--ddlgAccCode.Text
                                myCommand.Parameters.Add(New SqlParameter("@acc_type", SqlDbType.VarChar, 1)).Value = CType(row("acc_type"), String) 'collectionDate("AccType" & strLineKey).ToString
                                If CType(row("Tran_Type"), String).ToString = "JV" Then
                                    myCommand.Parameters.Add(New SqlParameter("@against_tran_id ", SqlDbType.VarChar, 20)).Value = txtDocNo.Value
                                    myCommand.Parameters.Add(New SqlParameter("@against_tran_type", SqlDbType.VarChar, 10)).Value = CType(row("against_tran_type"), String)
                                    myCommand.Parameters.Add(New SqlParameter("@against_tran_lineno", SqlDbType.Int)).Value = CType(row("against_tran_lineno"), Integer) ' lblno.Value 'collectionDate("AgainstTranLineNo" & strLineKey) 'intReceiptLinNo
                                Else
                                    myCommand.Parameters.Add(New SqlParameter("@against_tran_id ", SqlDbType.VarChar, 20)).Value = txtDocNo.Value
                                    myCommand.Parameters.Add(New SqlParameter("@against_tran_type", SqlDbType.VarChar, 10)).Value = ""
                                    myCommand.Parameters.Add(New SqlParameter("@against_tran_lineno", SqlDbType.Int)).Value = DBNull.Value
                                End If


                                myCommand.Parameters.Add(New SqlParameter("@acc_gl_code", SqlDbType.VarChar, 20)).Value = CType(row("acc_gl_code"), String)  'collectionDate("AccGLCode" & strLineKey).ToString 'ddlConAccCode.Text

                                If CType(row("Acc_Code"), String).ToString = "" Then
                                    myCommand.Parameters.Add(New SqlParameter("@DUEDATE ", SqlDbType.DateTime)).Value = objDateTime.ConvertDateromTextBoxToDatabase(txtTDate.Text)
                                Else
                                    myCommand.Parameters.Add(New SqlParameter("@DUEDATE ", SqlDbType.DateTime)).Value = CType(row("open_due_date"), String) 'Format(CType(collectionDate("DueDate" & strLineKey).ToString, Date), "yyyy/MM/dd")
                                End If

                                myCommand.Parameters.Add(New SqlParameter("@currrate", SqlDbType.Decimal, 18, 12)).Value = CType(row("currency_rate"), Decimal)   'CType(collectionDate("CurrRate" & strLineKey), Decimal) 'CType(txtCurrRate.Value, Decimal)
                                myCommand.Parameters.Add(New SqlParameter("@field1", SqlDbType.VarChar, 500)).Value = CType(row("open_field1"), String) '- -collectionDate("AccType" & strLineKey).ToString
                                myCommand.Parameters.Add(New SqlParameter("@field2", SqlDbType.VarChar, 500)).Value = CType(row("open_field2"), String)
                                myCommand.Parameters.Add(New SqlParameter("@field3 ", SqlDbType.VarChar, 500)).Value = CType(row("open_field3"), String)
                                myCommand.Parameters.Add(New SqlParameter("@field4", SqlDbType.VarChar, 500)).Value = CType(row("open_field4"), String)
                                myCommand.Parameters.Add(New SqlParameter("@field5", SqlDbType.VarChar, 500)).Value = CType(row("open_field5"), String)
                                If CType(row("Tran_Type"), String).ToString = "JV" Then
                                    'If UCase(collectionDate("TranType" & strLineKey).ToString) = "RV" Then
                                    myCommand.Parameters.Add(New SqlParameter("@accmode", SqlDbType.VarChar, 1)).Value = CType(row("open_mode"), String)
                                Else
                                    myCommand.Parameters.Add(New SqlParameter("@accmode", SqlDbType.VarChar, 1)).Value = "B"

                                End If
                                myCommand.Parameters.Add(New SqlParameter("@open_field3 ", SqlDbType.VarChar, 500)).Value = txtnarration.Text
                                myCommand.Parameters.Add(New SqlParameter("@currcode", SqlDbType.VarChar, 10)).Value = CType(row("currcode"), String) ' CType(collectionDate("currcode" & strLineKey), Decimal) '"" txtCurrCodegrid.Value.Trim
                                myCommand.Parameters.Add(New SqlParameter("@acc_State", SqlDbType.VarChar, 10)).Value = "P"
                                myCommand.Parameters.Add(New SqlParameter("@debit", SqlDbType.Money)).Value = DecRound(CType(row("open_debit"), Decimal)) 'CType(Val(txtDebit.Value), Decimal)
                                myCommand.Parameters.Add(New SqlParameter("@credit", SqlDbType.Money)).Value = DecRound(CType(row("open_credit"), Decimal))
                                myCommand.Parameters.Add(New SqlParameter("@basedebit", SqlDbType.Money)).Value = DecRound(CType(row("base_debit"), Decimal))
                                myCommand.Parameters.Add(New SqlParameter("@basecredit", SqlDbType.Money)).Value = DecRound(CType(row("base_credit"), Decimal))
                                '  myCommand.Parameters.Add(New SqlParameter("@basecurrency", SqlDbType.Decimal, 18, 12)).Value = 0
                                myCommand.ExecuteNonQuery()

                                'Tanvir  02102023 Point 6

                            Next
                        End If

                        'For Accounts Posting
                        lblPostmsg.Text = "Posted"
                        lblPostmsg.ForeColor = Drawing.Color.Red
                    Else
                        lblPostmsg.Text = "UnPosted"
                        lblPostmsg.ForeColor = Drawing.Color.Green
                    End If


                ElseIf ViewState("JournalState") = "Delete" Then
                    'Delete Record
                    myCommand = New SqlCommand("sp_delvoucher", SqlConn, sqlTrans)
                    myCommand.CommandType = CommandType.StoredProcedure
                    myCommand.Parameters.Add(New SqlParameter("@tablename", SqlDbType.VarChar, 20)).Value = "journal_master"
                    myCommand.Parameters.Add(New SqlParameter("@tranid", SqlDbType.VarChar, 20)).Value = txtDocNo.Value
                    myCommand.Parameters.Add(New SqlParameter("@trantype ", SqlDbType.VarChar, 10)).Value = CType(ViewState("JournalTranType"), String)
                    myCommand.Parameters.Add(New SqlParameter("@div_id", SqlDbType.VarChar, 10)).Value = strdiv
                    myCommand.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                    myCommand.ExecuteNonQuery()

                    ' Tanvir 02/10/2023 Point 6
                    'Tanvir 04102023 point6
                    If Not Session("opendetail_records_jv") Is Nothing Then
                        Dim dataTable As DataTable = DirectCast(Session("opendetail_records_jv"), DataTable)
                        Dim filterExpression As String = " div_id = '" & strdiv & " '" '  against_tran_id =" & txtDocNo.Value & "  and against_tran_lineno= " & intReceiptLinNo & " and   against_tran_type='" & CType(ViewState("ReceiptsRVPVTranType"), String) & "' And  div_id = " & strdiv & " "
                        Dim filteredRows() As DataRow = dataTable.Select(filterExpression)
                        Dim advpayment As Integer = 0
                        ' If ViewState("ReceiptsState") = "Edit" Then
                        If dataTable.Rows.Count > 0 Then


                            For Each row As DataRow In filteredRows
                                myCommand = New SqlCommand("sp_reverse_pending_invoices", SqlConn, sqlTrans)
                                myCommand.CommandType = CommandType.StoredProcedure
                                myCommand.Parameters.Add(New SqlParameter("@div_id", SqlDbType.VarChar, 10)).Value = strdiv
                                myCommand.Parameters.Add(New SqlParameter("@doclineno", SqlDbType.Int)).Value = CType(row("TRAN_LINENO"), String)
                                'If CType(row("tran_type"), String) = "RV" Then 'collectionDate("TranType" & strLineKey).ToString = "RV" Then
                                myCommand.Parameters.Add(New SqlParameter("@docno", SqlDbType.VarChar, 20)).Value = CType(row("tran_id"), String) ' collectionDate("TranId" & strLineKey).ToString
                                myCommand.Parameters.Add(New SqlParameter("@doctype ", SqlDbType.VarChar, 10)).Value = CType(row("tran_type"), String) 'collectionDate("TranType" & strLineKey).ToString
                                advpayment = 1

                                myCommand.Parameters.Add(New SqlParameter("@acc_code", SqlDbType.VarChar, 20)).Value = CType(row("ACC_CODE"), String) ' collectionDate("AccCode" & strLineKey).ToString '--ddlgAccCode.Text
                                myCommand.Parameters.Add(New SqlParameter("@acc_type", SqlDbType.VarChar, 1)).Value = CType(row("ACC_TYPE"), String) 'collectionDate("AccType" & strLineKey).ToString
                                myCommand.Parameters.Add(New SqlParameter("@debit", SqlDbType.Money)).Value = DecRound(CType(row("open_Debit"), Decimal))   ' DecRound(CType(objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select sum(isnull(open_debit,0))open_debit  from open_detail od(nolock)  where od.against_tran_id =" & txtDocNo.Value & "  and od.against_tran_lineno= " & intReceiptLinNo & " and  od.against_tran_type='" & CType(ViewState("ReceiptsRVPVTranType"), String) & "' And od.div_id = " & strdiv & " "), Decimal))
                                myCommand.Parameters.Add(New SqlParameter("@credit", SqlDbType.Money)).Value = DecRound(CType(row("open_credit"), Decimal)) 'DecRound(CType(objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select sum(isnull(open_credit,0))open_credit from open_detail od(nolock)  where od.against_tran_id =" & txtDocNo.Value & "  and od.against_tran_lineno= " & intReceiptLinNo & " and  od.against_tran_type='" & CType(ViewState("ReceiptsRVPVTranType"), String) & "' And od.div_id = " & strdiv & " "), Decimal))
                                myCommand.Parameters.Add(New SqlParameter("@basedebit", SqlDbType.Money)).Value = DecRound(CType(row("base_Debit"), Decimal)) ' DecRound(CType(objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select sum(isnull(base_debit,0))basedebit from open_detail od(nolock)  where od.against_tran_id =" & txtDocNo.Value & "  and od.against_tran_lineno= " & intReceiptLinNo & " and  od.against_tran_type='" & CType(ViewState("ReceiptsRVPVTranType"), String) & "' And od.div_id = " & strdiv & " "), Decimal))
                                myCommand.Parameters.Add(New SqlParameter("@basecredit", SqlDbType.Money)).Value = DecRound(CType(row("base_credit"), Decimal)) 'DecRound(CType(objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select sum(isnull(base_credit,0))basecredit from open_detail od(nolock)  where od.against_tran_id =" & txtDocNo.Value & "  and od.against_tran_lineno= " & intReceiptLinNo & " and  od.against_tran_type='" & CType(ViewState("ReceiptsRVPVTranType"), String) & "' And od.div_id = " & strdiv & " "), Decimal))

                                '  End If
                                myCommand.ExecuteNonQuery()
                            Next
                        End If


                    End If
                    ' Tanvir 02/10/2023 Point 6
                    myCommand = New SqlCommand("sp_del_journal", SqlConn, sqlTrans)
                    myCommand.CommandType = CommandType.StoredProcedure
                    myCommand.Parameters.Add(New SqlParameter("@div_id", SqlDbType.VarChar, 10)).Value = strdiv
                    myCommand.Parameters.Add(New SqlParameter("@tran_id", SqlDbType.VarChar, 10)).Value = txtDocNo.Value
                    myCommand.Parameters.Add(New SqlParameter("@tran_type", SqlDbType.VarChar, 10)).Value = CType(ViewState("JournalTranType"), String)
                    myCommand.Parameters.Add(New SqlParameter("@userlogged ", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                    myCommand.ExecuteNonQuery()
                    'Tanvir 05102023
                    ' Save_Pending_invoices(ViewState("JournalState"), SqlConn, sqlTrans)

                    myCommand = New SqlCommand("sp_del_open_detail_new", SqlConn, sqlTrans)
                    myCommand.CommandType = CommandType.StoredProcedure
                    myCommand.Parameters.Add(New SqlParameter("@div_id", SqlDbType.VarChar, 10)).Value = strdiv
                    myCommand.Parameters.Add(New SqlParameter("@tran_id", SqlDbType.VarChar, 20)).Value = txtDocNo.Value
                    myCommand.Parameters.Add(New SqlParameter("@tran_type ", SqlDbType.VarChar, 10)).Value = CType(ViewState("JournalTranType"), String)
                    myCommand.ExecuteNonQuery()


                ElseIf ViewState("JournalState") = "Cancel" Then

                    'Delete Record
                    myCommand = New SqlCommand("sp_delvoucher", SqlConn, sqlTrans)
                    myCommand.CommandType = CommandType.StoredProcedure
                    myCommand.Parameters.Add(New SqlParameter("@tablename", SqlDbType.VarChar, 20)).Value = "journal_master"
                    myCommand.Parameters.Add(New SqlParameter("@tranid", SqlDbType.VarChar, 20)).Value = txtDocNo.Value
                    myCommand.Parameters.Add(New SqlParameter("@trantype ", SqlDbType.VarChar, 10)).Value = CType(ViewState("JournalTranType"), String)
                    myCommand.Parameters.Add(New SqlParameter("@div_id", SqlDbType.VarChar, 10)).Value = strdiv
                    myCommand.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                    myCommand.ExecuteNonQuery()

                    myCommand = New SqlCommand("sp_cancel_journal_open_detail_new", SqlConn, sqlTrans)
                    myCommand.CommandType = CommandType.StoredProcedure
                    myCommand.Parameters.Add(New SqlParameter("@div_id", SqlDbType.VarChar, 10)).Value = strdiv
                    myCommand.Parameters.Add(New SqlParameter("@tran_id", SqlDbType.VarChar, 20)).Value = txtDocNo.Value
                    myCommand.Parameters.Add(New SqlParameter("@tran_type ", SqlDbType.VarChar, 10)).Value = CType(ViewState("JournalTranType"), String)
                    myCommand.ExecuteNonQuery()

                    myCommand = New SqlCommand("sp_cancel_journal", SqlConn, sqlTrans)
                    myCommand.CommandType = CommandType.StoredProcedure
                    myCommand.Parameters.Add(New SqlParameter("@div_id", SqlDbType.VarChar, 10)).Value = strdiv
                    myCommand.Parameters.Add(New SqlParameter("@tran_id", SqlDbType.VarChar, 20)).Value = txtDocNo.Value
                    myCommand.Parameters.Add(New SqlParameter("@tran_type", SqlDbType.VarChar, 10)).Value = CType(ViewState("JournalTranType"), String)
                    myCommand.Parameters.Add(New SqlParameter("@userlogged ", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                    myCommand.ExecuteNonQuery()
                ElseIf ViewState("JournalState") = "UndoCancel" Then
                    myCommand = New SqlCommand("sp_undocancel_journal", SqlConn, sqlTrans)
                    myCommand.CommandType = CommandType.StoredProcedure
                    myCommand.Parameters.Add(New SqlParameter("@div_id", SqlDbType.VarChar, 10)).Value = strdiv
                    myCommand.Parameters.Add(New SqlParameter("@tran_id", SqlDbType.VarChar, 20)).Value = txtDocNo.Value
                    myCommand.Parameters.Add(New SqlParameter("@tran_type ", SqlDbType.VarChar, 10)).Value = CType(ViewState("JournalTranType"), String)
                    myCommand.ExecuteNonQuery()


                End If
                sqlTrans.Commit()    'SQl Tarn Commit
                clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
                clsDBConnect.dbCommandClose(myCommand)               'sql command disposed
                clsDBConnect.dbConnectionClose(SqlConn)           'connection close
                If ViewState("JournalState") = "Delete" Or ViewState("JournalState") = "Cancel" Or ViewState("JournalState") = "UndoCancel" Then
                    '  Response.Redirect("JournalSearch.aspx", False)
                    Dim strscript As String = ""
                    strscript = "window.opener.__doPostBack('JournalWindowPostBack', '');window.opener.focus();window.close();"
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)

                Else
                    If ViewState("JournalState") = "New" Or ViewState("JournalState") = "Copy" Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Record save successfully');", True)
                    ElseIf ViewState("JournalState") = "Edit" Then
                        'ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Record update successfully');", True)
                        Dim strURL As String = ""
                        strURL = "window.open('Accnt_trn_amendlog.aspx?tid=" & txtDocNo.Value & "&ttype=" & txtTranType.Text.Trim & "&divid=" & ViewState("divcode") & "&tdate=" & txtTDate.Text.Trim + "','Log','width=100,height=100 left=20,top=20 status=1,toolbar=no,menubar=no,resizable=no,scrollbars=yes');"
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strURL, True)

                        'strURL = "Accnt_trn_amendlog.aspx?tid=" & txtDocNo.Value & "&ttype=" & txtTranType.Text.Trim & "&tdate=" & txtTDate.Text.Trim
                        'ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", String.Format(ScriptOpenModalDialog, strURL, 300), True)


                    End If

                    btnPrint.Visible = True
                    ViewState("JournalState") = "View"
                    Disabled_Control()
                    'btnPrint_Click(sender, e)  

                End If
                Session.Remove("Collection" & ":" & txtAdjcolno.Value)
            End If
        Catch ex As Exception
            If SqlConn.State = ConnectionState.Open Then
                sqlTrans.Rollback()
            End If
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("Journal.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#End Region

    Public Function Validateseal() As Boolean
        Try
            Validateseal = True
            Dim invdate As DateTime
            Dim sealdate As DateTime
            Dim MyCultureInfo As New CultureInfo("fr-Fr")
            invdate = DateTime.Parse(txtJDate.Text, MyCultureInfo, DateTimeStyles.None)
            sealdate = DateTime.Parse(txtpdate.Text, MyCultureInfo, DateTimeStyles.None)
            If invdate <= sealdate Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Document has sealed in this period cannot make entry.Close the entry and make with another date')", True)
                Validateseal = False
            End If

        Catch ex As Exception
            Validateseal = False
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("requestforinvoicing.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Function
#Region "Public Function validate_AdjustBill(ByVal intReceiptLinNo As String,  ByVal strGlCode As String) as Boolean"
    Public Function validate_AdjustBill(ByVal intReceiptLinNo As String, ByVal strAccCode As String, ByVal strAccType As String, ByVal strGlCode As String, ByVal Adjustamt As Decimal) As Boolean
        validate_AdjustBill = True
        Dim collectionDate As Collection
        Dim strLineKey As String
        Dim MainGrdCount As Integer = grdJournal.Rows.Count
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
        '  End If
        'If DecRound(Adjustamt) = DecRound(base_debit) Or DecRound(Adjustamt) = DecRound(base_credit) Then
        'Else
        '    validate_AdjustBill = False
        '    Exit Function
        'End If
        If DecRound(Adjustamt) = DecRound(base_debit) Or DecRound(Adjustamt) = DecRound(base_credit) Or Math.Abs(DecRound(base_debit) - DecRound(base_credit)) = DecRound(Adjustamt) Then
        Else
            validate_AdjustBill = False
            Exit Function
        End If

    End Function
#End Region

    Private Function checkopenmode() As Boolean
        checkopenmode = True
        Dim collectionDate As Collection
        Dim spersoncode As String
        Dim strdiv As String
        Dim strLineKey As String
        Dim MainGrdCount As Integer = grdJournal.Rows.Count
        Dim dfalg As Boolean = True

        Dim lblno As HtmlInputText
        Dim ddlgAccName As HtmlSelect
        Dim ddlAccType As HtmlSelect
        Dim ddlConAccCode As HtmlSelect
        Dim openmode, popenmode As String

        For Each gvRow In grdJournal.Rows

            ddlConAccCode = gvRow.FindControl("ddlConAccCode")
            ddlAccType = gvRow.FindControl("ddlType")
            ddlgAccName = gvRow.FindControl("ddlgAccName")
            lblno = gvRow.FindControl("txtlineno")
            If ddlAccType.Value <> "G" And ddlAccType.Value <> "[Select]" Then
                collectionDate = GetCollectionFromSession()
                If collectionDate.Count <> 0 Then
                    strdiv = objUtils.GetDBFieldFromLongnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", 511)
                    Dim intcount As Integer = collectionDate.Count / 21
                    Dim intLinNo, MainRowidx As Integer
                    intLinNo = 1
                    MainRowidx = 1
                    For MainRowidx = 1 To MainGrdCount
                        If MainRowidx = CType(lblno.Value, Integer) Then

                            strLineKey = intLinNo & ":" & CType(lblno.Value, Integer)
                            popenmode = collectionDate("OpenMode" & strLineKey).ToString()
                            For intLinNo = 1 To intcount
                                strLineKey = intLinNo & ":" & CType(lblno.Value, Integer)
                                If colexists(collectionDate, "AgainstTranLineNo" & strLineKey) = True Then
                                    If collectionDate("AccCode" & strLineKey).ToString = CType(ddlgAccName.Value, String) And collectionDate("AccType" & strLineKey).ToString = CType(ddlAccType.Value, String) And collectionDate("AccGLCode" & strLineKey).ToString = CType(ddlConAccCode.Items(ddlConAccCode.SelectedIndex).Text, String) Then
                                        openmode = collectionDate("OpenMode" & strLineKey).ToString()
                                        If popenmode <> openmode Then
                                            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Cannot create the advance due to amount difference..' );", True)
                                            checkopenmode = False
                                            chkadjust.Visible = True
                                            Exit Function
                                        Else
                                            popenmode = collectionDate("OpenMode" & strLineKey).ToString()
                                        End If
                                    End If
                                End If
                            Next
                        End If
                    Next
                End If
            End If
        Next
    End Function


#Region "Public Sub Save_Open_detail(ByVal intReceiptLinNo As String, ByVal SqlConn As SqlConnection, ByVal sqlTrans As SqlTransaction)"
    Public Sub Save_Open_detail(ByVal intReceiptLinNo As String, ByVal strAccCode As String, ByVal strAccType As String, ByVal strGlCode As String, ByVal SqlConn As SqlConnection, ByVal sqlTrans As SqlTransaction, ByVal currcode As String)
        Dim collectionDate As Collection
        Dim spersoncode As String
        Dim strdiv As String
        Dim strLineKey As String
        Dim MainGrdCount As Integer = grdJournal.Rows.Count
        If DirectCast(Session("Adjustedrecords"), DataTable) Is Nothing Then
            createjournaldatatable()
        End If
        Dim dataTable As New DataTable()
        dataTable = Session("Adjustedrecords")

        'If Session("Collection").ToString <> "" Then
        'collectionDate = CType(Session("Collection"), Collection)
        collectionDate = GetCollectionFromSession()
        If collectionDate.Count <> 0 Then
            strdiv = ViewState("divcode") ' objUtils.GetDBFieldFromLongnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", 511)
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

                                myCommand.Parameters.Add(New SqlParameter("@against_tran_id", SqlDbType.VarChar, 20)).Value = txtDocNo.Value
                                myCommand.Parameters.Add(New SqlParameter("@against_tran_lineno", SqlDbType.Int)).Value = intReceiptLinNo 'collectionDate("AgainstTranLineNo" & strLineKey) 'intReceiptLinNo
                                myCommand.Parameters.Add(New SqlParameter("@against_tran_type", SqlDbType.VarChar, 10)).Value = CType(ViewState("JournalTranType"), String)
                                myCommand.Parameters.Add(New SqlParameter("@against_tran_date ", SqlDbType.DateTime)).Value = Format(CType(txtTDate.Text, Date), "yyyy/MM/dd")
                                If collectionDate("DueDate" & strLineKey).ToString = "" Then
                                    myCommand.Parameters.Add(New SqlParameter("@open_due_date ", SqlDbType.DateTime)).Value = DBNull.Value
                                Else
                                    myCommand.Parameters.Add(New SqlParameter("@open_due_date ", SqlDbType.DateTime)).Value = Format(CType(collectionDate("DueDate" & strLineKey).ToString, Date), "yyyy/MM/dd")
                                End If

                                If strAccType = "C" Then
                                    spersoncode = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "agentmast", "spersoncode", "agentcode", strAccCode)
                                    myCommand.Parameters.Add(New SqlParameter("@open_sales_code", SqlDbType.VarChar, 10)).Value = spersoncode
                                Else
                                    myCommand.Parameters.Add(New SqlParameter("@open_sales_code", SqlDbType.VarChar, 10)).Value = ""
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
                                myCommand.Parameters.Add(New SqlParameter("@dr_cr", SqlDbType.Char, 1)).Value = ""
                                myCommand.Parameters.Add(New SqlParameter("@div_id", SqlDbType.VarChar, 10)).Value = strdiv
                                myCommand.Parameters.Add(New SqlParameter("@currency_rate", SqlDbType.Decimal, 18, 12)).Value = CType(collectionDate("CurrRate" & strLineKey), Decimal)
                                myCommand.Parameters.Add(New SqlParameter("@base_debit", SqlDbType.Money)).Value = DecRound(CType(collectionDate("BaseDebit" & strLineKey), Decimal))
                                myCommand.Parameters.Add(New SqlParameter("@base_credit", SqlDbType.Money)).Value = DecRound(CType(collectionDate("BaseCredit" & strLineKey), Decimal))
                                myCommand.Parameters.Add(New SqlParameter("@acc_type", SqlDbType.Char, 1)).Value = collectionDate("AccType" & strLineKey).ToString
                                myCommand.Parameters.Add(New SqlParameter("@acc_code", SqlDbType.VarChar, 20)).Value = collectionDate("AccCode" & strLineKey).ToString
                                myCommand.Parameters.Add(New SqlParameter("@acc_gl_code", SqlDbType.VarChar, 20)).Value = collectionDate("AccGLCode" & strLineKey).ToString

                                myCommand.ExecuteNonQuery()

                                Dim newRow As DataRow = dataTable.NewRow()
                                ' newRow("TRANID") = intReceiptLinNo
                                newRow("TRANID") = collectionDate("TranId" & strLineKey).ToString
                                newRow("tran_type") = collectionDate("TranType" & strLineKey).ToString
                                newRow("tran_lineno") = collectionDate("AccTranLineNo" & strLineKey)
                                newRow("tran_date") = Format(CType(collectionDate("TranDate" & strLineKey).ToString, Date), "yyyy/MM/dd")
                                newRow("against_tran_id") = txtDocNo.Value
                                newRow("against_tran_lineno") = intReceiptLinNo
                                newRow("against_tran_type") = CType(ViewState("JournalTranType"), String)
                                newRow("against_tran_date") = Format(CType(txtTDate.Text, Date), "yyyy/MM/dd")
                                If collectionDate("DueDate" & strLineKey).ToString = "" Then
                                    myCommand.Parameters.Add(New SqlParameter("@open_due_date ", SqlDbType.DateTime)).Value = DBNull.Value
                                Else
                                    myCommand.Parameters.Add(New SqlParameter("@open_due_date ", SqlDbType.DateTime)).Value = Format(CType(collectionDate("DueDate" & strLineKey).ToString, Date), "yyyy/MM/dd")
                                End If
                                If collectionDate("DueDate" & strLineKey).ToString = "" Then
                                    newRow("open_due_date") = DBNull.Value
                                Else
                                    newRow("open_due_date") = Format(CType(collectionDate("DueDate" & strLineKey).ToString, Date), "yyyy/MM/dd")
                                End If

                                newRow("open_debit") = DecRound(CType(collectionDate("Debit" & strLineKey), Decimal))
                                newRow("open_credit") = DecRound(CType(collectionDate("Credit" & strLineKey), Decimal))
                                newRow("open_field1") = collectionDate("RefNo" & strLineKey).ToString
                                newRow("open_field2") = collectionDate("Field2" & strLineKey).ToString
                                newRow("open_field3") = collectionDate("Field3" & strLineKey).ToString
                                newRow("open_field4") = collectionDate("Field4" & strLineKey).ToString
                                newRow("open_field5") = collectionDate("Field5" & strLineKey).ToString

                                newRow("open_mode") = IIf(collectionDate("OpenMode" & strLineKey).ToString <> "", collectionDate("OpenMode" & strLineKey).ToString, "A")
                                newRow("div_id") = strdiv
                                newRow("currency_rate") = CType(collectionDate("CurrRate" & strLineKey), Decimal)
                                newRow("base_debit") = DecRound(CType(collectionDate("BaseDebit" & strLineKey), Decimal))
                                newRow("base_CREDIT") = DecRound(CType(collectionDate("BaseCredit" & strLineKey), Decimal))
                                newRow("acc_type") = collectionDate("AccType" & strLineKey).ToString
                                newRow("acc_code") = collectionDate("AccCode" & strLineKey).ToString
                                '  newRow("base_debit") = DecRound(CType(collectionDate("BaseDebit" & strLineKey), Decimal))
                                newRow("acc_gl_code") = collectionDate("AccGLCode" & strLineKey).ToString
                                newRow("currcode") = currcode 'tanvir 23/11/2023
                                dataTable.Rows.Add(newRow)

                            End If
                        End If
                    Next

                End If
            Next
            If dataTable.Rows.Count > 0 Then
                Session("Adjustedrecords") = dataTable

            End If
        End If

        'End If
    End Sub
#End Region
    '#Region "Public Sub Save_Open_detail(ByVal intReceiptLinNo As String, ByVal SqlConn As SqlConnection, ByVal sqlTrans As SqlTransaction)"
    '    Public Sub Save_Open_detail(ByVal intReceiptLinNo As String, ByVal SqlConn As SqlConnection, ByVal sqlTrans As SqlTransaction)
    '        Dim collectionData As Collection
    '        Dim strdiv As String
    '        Dim strLineKey As String
    '        If Session("Collection").ToString <> "" Then
    '            collectionData = CType(Session("Collection"), Collection)
    '            If collectionData.Count <> 0 Then
    '                strdiv = objUtils.GetDBFieldFromLongnew(Session("dbconnectionName"),"reservation_parameters", "option_selected", "param_id", 511)

    '                Dim intcount As Integer = collectionData.Count / 13
    '                Dim intLinNo As Integer
    '                For intLinNo = 1 To intcount
    '                    strLineKey = intLinNo & ":" & intReceiptLinNo

    '                    If colexists(collectionData, "TranId" & strLineKey) = False Then Exit For

    '                    myCommand = New SqlCommand("sp_add_open_detail_new", SqlConn, sqlTrans)
    '                    myCommand.CommandType = CommandType.StoredProcedure
    '                    myCommand.Parameters.Add(New SqlParameter("@tran_id", SqlDbType.VarChar, 20)).Value = collectionData("TranId" & strLineKey).ToString
    '                    myCommand.Parameters.Add(New SqlParameter("@tran_type", SqlDbType.VarChar, 10)).Value = collectionData("TranType" & strLineKey).ToString
    '                    myCommand.Parameters.Add(New SqlParameter("@tran_date ", SqlDbType.DateTime)).Value = collectionData("TranDate" & strLineKey).ToString
    '                    myCommand.Parameters.Add(New SqlParameter("@tran_lineno", SqlDbType.Int)).Value = collectionData("LinNo" & strLineKey)

    '                    myCommand.Parameters.Add(New SqlParameter("@against_tran_id", SqlDbType.VarChar, 20)).Value = txtDocNo.Value
    '                    myCommand.Parameters.Add(New SqlParameter("@against_tran_lineno", SqlDbType.Int)).Value = intReceiptLinNo
    '                    myCommand.Parameters.Add(New SqlParameter("@against_tran_type", SqlDbType.VarChar, 10)).Value = CType(ViewState("JournalTranType"), String)
    '                    myCommand.Parameters.Add(New SqlParameter("@against_tran_date ", SqlDbType.DateTime)).Value = objDateTime.ConvertDateromTextBoxToDatabase(txtTDate.Text)

    '                    myCommand.Parameters.Add(New SqlParameter("@open_due_date ", SqlDbType.DateTime)).Value = collectionData("DueDate" & strLineKey).ToString
    '                    myCommand.Parameters.Add(New SqlParameter("@open_sales_code", SqlDbType.VarChar, 10)).Value = ""

    '                    myCommand.Parameters.Add(New SqlParameter("@open_debit", SqlDbType.Money)).Value = CType(collectionData("Debit" & strLineKey), Decimal)
    '                    myCommand.Parameters.Add(New SqlParameter("@open_credit", SqlDbType.Money)).Value = CType(collectionData("Credit" & strLineKey), Decimal)

    '                    myCommand.Parameters.Add(New SqlParameter("@open_field1", SqlDbType.VarChar, 100)).Value = collectionData("RefNo" & strLineKey).ToString
    '                    myCommand.Parameters.Add(New SqlParameter("@open_field2", SqlDbType.VarChar, 100)).Value = collectionData("Field2" & strLineKey).ToString
    '                    myCommand.Parameters.Add(New SqlParameter("@open_field3", SqlDbType.VarChar, 100)).Value = collectionData("Field3" & strLineKey).ToString
    '                    myCommand.Parameters.Add(New SqlParameter("@open_field4", SqlDbType.VarChar, 100)).Value = collectionData("Field4" & strLineKey).ToString
    '                    myCommand.Parameters.Add(New SqlParameter("@open_field5", SqlDbType.VarChar, 100)).Value = collectionData("Field5" & strLineKey).ToString
    '                    myCommand.Parameters.Add(New SqlParameter("@open_mode", SqlDbType.Char, 1)).Value = collectionData("OpenMode" & strLineKey).ToString
    '                    myCommand.Parameters.Add(New SqlParameter("@open_exchg_diff", SqlDbType.Money)).Value = 0
    '                    myCommand.Parameters.Add(New SqlParameter("@dr_cr", SqlDbType.Char, 1)).Value = ""
    '                    myCommand.Parameters.Add(New SqlParameter("@div_id", SqlDbType.VarChar, 10)).Value = strdiv

    '                    myCommand.ExecuteNonQuery()
    '                Next
    '            End If
    '        End If
    '    End Sub
    '#End Region
#Region "Private Function colexists(ByVal newcol As Collection, ByVal newkey As String) As Boolean"
    Private Function colexists(ByVal newcol As Collection, ByVal newkey As String) As Boolean
        Try
            Dim k As Integer
            colexists = False
            If newcol.Count > 0 Then
                For k = 1 To newcol.Count
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
#Region "Public Function validate_page() As Boolean"
    Public Function validate_page() As Boolean
        validate_page = True

        If txtTDate.Text = "" Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please enter transaction date.');", True)
            SetFocus(txtTDate)
            validate_page = False
            Exit Function
        End If
        If txtJDate.Text = "[Select]" Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please enter journal date .');", True)
            SetFocus(txtJDate)
            validate_page = False
            Exit Function
        End If


        Dim ddlAccType As HtmlSelect
        Dim txtacct As TextBox
        Dim txtacctcode As TextBox
        Dim txtcontrolacctcode As TextBox
        Dim txtcontrolacct As TextBox
        Dim txtCredit, txtDebit, lblno, txtBaseCredit, txtBaseDebit As HtmlInputText
        Dim baseamt As Decimal
        Dim dfalg As Boolean = True

        Dim txtConvRate As HtmlInputText

        Dim txtcurrencycode As HtmlInputText



        For Each gvRow In grdJournal.Rows

            ddlAccType = gvRow.FindControl("ddlType")
            txtacctcode = gvRow.FindControl("txtacctcode")
            txtacct = gvRow.FindControl("txtacct")
            txtcontrolacctcode = gvRow.FindControl("txtcontrolacctcode")
            txtcurrencycode = gvRow.FindControl("txtcurrencycode")
            txtConvRate = gvRow.FindControl("txtConvRate")
            txtDebit = gvRow.FindControl("txtDebit")
            txtCredit = gvRow.FindControl("txtCredit")
            lblno = gvRow.FindControl("txtlineno")
            txtBaseDebit = gvRow.FindControl("txtBaseDebit")
            txtBaseCredit = gvRow.FindControl("txtBaseCredit")

            If ddlAccType.Value.Trim <> "[Select]" Or txtConvRate.Value.Trim <> "" Or txtacct.Text.Trim <> "" Then
                dfalg = False
                If ddlAccType.Value = "[Select]" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please select account type.');", True)
                    SetFocus(ddlAccType)
                    validate_page = False
                    Exit Function
                End If
                If txtacct.Text = "" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please select account code.');", True)
                    SetFocus(txtacct)
                    validate_page = False
                    Exit Function
                End If
                If txtcurrencycode.Value = "" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please select currancy code.');", True)
                    SetFocus(txtcurrencycode)
                    validate_page = False
                    Exit Function
                End If
                If Val(txtConvRate.Value) <= 0 Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please enter valid exachange rate.');", True)
                    SetFocus(txtConvRate)
                    validate_page = False
                    Exit Function
                End If
                Dim strMsg As String = ""
                strMsg = "Account Currency  and BaseCurrency are same so Conversion rate should be 1. for this account " + txtacctcode.Text
                If txtbasecurr.Value = txtcurrencycode.Value And CType(Val(txtConvRate.Value), Decimal) <> 1 Then
                    'ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Account Currency  and BaseCurrency are same so Conversion rate should be 1.' + ddlgAccName.value);", True)
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & strMsg & "');", True)
                    SetFocus(ddlAccType)
                    validate_page = False
                    Exit Function
                End If


                If Val(txtDebit.Value) <= 0 And Val(txtCredit.Value) <= 0 Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please enter debit or credit amount.');", True)
                    SetFocus(txtDebit)
                    validate_page = False
                    Exit Function
                End If
                If Val(txtBaseDebit.Value) = 0 And Val(txtBaseCredit.Value) = 0 Then
                    strMsg = ""
                    Dim basecurr As String = objUtils.GetDBFieldFromLongnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", 457)
                    strMsg = "Both " & basecurr & " Debit amount and " & basecurr & " Credit amount can not be zero."
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & strMsg & "');", True)
                    SetFocus(txtBaseDebit)
                    validate_page = False
                    Exit Function
                End If

                'If Val(txtDebit.Value) > 0 Then
                '    If Not txtBaseDebit.Value = CType(txtDebit.Value, Decimal) * CType(txtCurrRate.Value, Decimal) Then
                '        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please enter correct base debit amount.');", True)
                '        SetFocus(txtBaseDebit)
                '        validate_page = False
                '        Exit Function
                '    End If
                'End If

                'If Val(txtCredit.Value) > 0 Then
                '    If Not txtBaseCredit.Value = CType(txtCredit.Value, Decimal) * CType(txtCurrRate.Value, Decimal) Then
                '        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please enter correct base credit amount.');", True)
                '        SetFocus(txtBaseCredit)
                '        validate_page = False
                '        Exit Function
                '    End If
                'End If

                If ddlAccType.Value <> "G" Then
                    If txtcontrolacctcode.Text = "" Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please select Control account code,Update Account Master.');", True)
                        SetFocus(ddlAccType)
                        validate_page = False
                        Exit Function
                    End If
                    If Val(txtBaseDebit.Value) <> 0 Then
                        baseamt = DecRound(CType(txtBaseDebit.Value, Decimal))
                    Else
                        baseamt = DecRound(CType(txtBaseCredit.Value, Decimal))
                    End If
                    If validate_AdjustBill(lblno.Value, txtacctcode.Text, ddlAccType.Value, txtcontrolacctcode.Text, CType(Val(baseamt), Decimal)) = False Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please enter valid Bill Adjust amount.');", True)
                        SetFocus(txtCredit)
                        validate_page = False
                        Exit Function
                    End If
                End If

                'Dim crdsqlstr As String = "SELECT COUNT(acctcode) FROM acctgroup Where div_code='" & ViewState("divcode") & "' and childid IN (115,120) and acctcode='" + ddlgAccCode.Items(ddlgAccCode.SelectedIndex).Text + "'"
                'Dim validMarketCount As Integer = GetScalarValue(Session("dbconnectionName"), crdsqlstr)

                'If validMarketCount > 0 And ddldept.Value = "[Select]" Then
                '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please select market name.');", True)
                '    'SetFocus(ddlSMktCode)
                '    validate_page = False
                '    Exit Function
                'End If

            End If
        Next

        'If And chkPost.Checked = False Then
        '    validate_page = True
        'Else
        '    If dfalg = True Then
        '        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please enter more than one journal voucher in grid.');", True)
        '        validate_page = False
        '        Exit Function
        '    End If
        'End If

        'Total of debit and credit in base amount should be equal

        If Val(txtTotBaseCredit.Value) <> Val(txtTotBaseDebit.Value) Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Total of debit and credit in base amount should be equal.');", True)
            validate_page = False
            Exit Function
        End If

    End Function
#End Region
#Region "Protected Sub btnExit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExit.Click"
    Protected Sub btnExit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExit.Click
        'Response.Redirect("JournalSearch.aspx", False)
        'ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", "window.close();", True)
        Session.Remove("Collection" & ":" & txtAdjcolno.Value)
        Dim strscript As String = ""
        strscript = "window.opener.__doPostBack('JournalWindowPostBack', '');window.opener.focus();window.close();"
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)

    End Sub
#End Region
#Region "Protected Sub btnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdd.Click"
    Protected Sub btnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        Dim n As Integer = 0
        Dim count As Integer
        count = grdJournal.Rows.Count + 1
        Dim lineno(count) As String
        Dim Olineno(count) As String
        Dim acctype(count) As String
        Dim acccode(count) As String
        Dim accname(count) As String
        Dim controlcode(count) As String
        Dim controlname(count) As String
        Dim CCCode(count) As String
        Dim CCName(count) As String
        Dim narration(count) As String
        Dim crate(count) As String
        Dim currcode(count) As String
        Dim currname(count) As String
        Dim bookingcode(count) As String
        Dim srcctry(count) As String
        Dim srcctryname(count) As String
        Dim debit(count) As String
        Dim credit(count) As String
        Dim bdebit(count) As String
        Dim bcredit(count) As String
        Dim ckDeletion(count) As String

        Dim dept(count) As String







        Dim ddlAccType As HtmlSelect
        Dim txtacct As TextBox
        Dim txtacctcode As TextBox
        Dim txtcontrolacctcode As TextBox
        Dim txtcontrolacct As TextBox
        Dim txtnarr As TextBox
        Dim txtcostcentercode As TextBox
        Dim txtcostcentername As TextBox
        Dim txtcurrencycode As HtmlInputText
        Dim txtcurrencyname As TextBox
        Dim txtConvRate As HtmlInputText
        Dim txtsourcectrycode As TextBox
        Dim txtsource As TextBox
        Dim txtbookingno As TextBox

        'Dim ddldept As HtmlSelect
        Dim txtDebit, txtBaseDebit, txtCredit, txtBaseCredit, txtOldLineno As HtmlInputText

        Dim chckDeletion As CheckBox
        Dim lblno As HtmlInputText





        Dim sqlstr1, sqlstr2 As String

        For Each gvRow In grdJournal.Rows
            chckDeletion = gvRow.FindControl("chckDeletion")
            lblno = gvRow.FindControl("txtlineno")
            txtOldLineno = gvRow.FindControl("txtOldLineno")

            ddlAccType = gvRow.FindControl("ddlType")
            txtcostcentercode = gvRow.FindControl("txtcostcentercode")
            txtcostcentername = gvRow.FindControl("txtcostcentername")

            ' ddldept = gvRow.FindControl("ddldept")

            txtacct = gvRow.FindControl("txtacct")
            txtacctcode = gvRow.FindControl("txtacctcode")

            txtcontrolacctcode = gvRow.FindControl("txtcontrolacctcode")
            txtcontrolacct = gvRow.FindControl("txtcontrolacct")

            txtcurrencycode = gvRow.FindControl("txtcurrencycode")
            txtcurrencyname = gvRow.FindControl("txtcurrencyname")
            txtConvRate = gvRow.FindControl("txtConvRate")

            txtDebit = gvRow.FindControl("txtDebit")
            txtCredit = gvRow.FindControl("txtCredit")
            txtBaseDebit = gvRow.FindControl("txtBaseDebit")
            txtBaseCredit = gvRow.FindControl("txtBaseCredit")

            txtnarr = gvRow.FindControl("txtnarr")
            'txtacctcode = gvRow.FindControl("txtacctcode")

            txtbookingno = gvRow.FindControl("txtbookingno")
            txtsourcectrycode = gvRow.FindControl("txtsourcectrycode")
            txtsource = gvRow.FindControl("txtsource")
            If txtacctcode.Text <> "" And ddlAccType.Value <> "" Then
                If chckDeletion.Checked = True Then
                    ckDeletion(n) = 1
                Else
                    ckDeletion(n) = 0
                End If
                Olineno(n) = txtOldLineno.Value
                acctype(n) = ddlAccType.Value
                acccode(n) = txtacctcode.Text
                accname(n) = txtacct.Text 'ddlgAccName.Value
                controlcode(n) = txtcontrolacctcode.Text
                controlname(n) = txtcontrolacct.Text
                CCCode(n) = CType(txtcostcentercode.Text, String)
                CCName(n) = CType(txtcostcentername.Text, String)

                currcode(n) = txtcurrencycode.Value
                currname(n) = txtcurrencyname.Text
                crate(n) = txtConvRate.Value

                credit(n) = txtCredit.Value
                debit(n) = txtDebit.Value
                bcredit(n) = txtBaseCredit.Value
                bdebit(n) = txtBaseDebit.Value
                narration(n) = txtnarr.Text

                bookingcode(n) = txtbookingno.Text
                srcctry(n) = txtsourcectrycode.Text
                srcctryname(n) = txtsource.Text


                ' dept(n) = CType(ddldept.Value, String)
                n = n + 1
            End If
        Next

        fillDategrd(grdJournal, False, grdJournal.Rows.Count + 1)
        Dim i As Integer = n
        n = 0

        For Each gvRow In grdJournal.Rows
            If n = i Then
                Exit For
            End If
            chckDeletion = gvRow.FindControl("chckDeletion")

            lblno = gvRow.FindControl("txtlineno")
            txtOldLineno = gvRow.FindControl("txtOldLineno")
            ddlAccType = gvRow.FindControl("ddlType")
            txtacctcode = gvRow.FindControl("txtacctcode")
            txtacct = gvRow.FindControl("txtacct")
            txtcontrolacctcode = gvRow.FindControl("txtcontrolacctcode")
            txtcontrolacct = gvRow.FindControl("txtcontrolacct")

            txtcostcentercode = gvRow.FindControl("txtcostcentercode")
            txtcostcentername = gvRow.FindControl("txtcostcentername")

            'ddldept = gvRow.FindControl("ddldept")

            txtcurrencycode = gvRow.FindControl("txtcurrencycode")
            txtcurrencyname = gvRow.FindControl("txtcurrencyname")
            txtConvRate = gvRow.FindControl("txtConvRate")


            txtDebit = gvRow.FindControl("txtDebit")
            txtCredit = gvRow.FindControl("txtCredit")
            txtBaseDebit = gvRow.FindControl("txtBaseDebit")
            txtBaseCredit = gvRow.FindControl("txtBaseCredit")

            txtnarr = gvRow.FindControl("txtNarr")
            txtbookingno = gvRow.FindControl("txtbookingno")
            txtsourcectrycode = gvRow.FindControl("txtsourcectrycode")
            txtsource = gvRow.FindControl("txtsource")

            txtbookingno.Text = bookingcode(n)
            txtsourcectrycode.Text = srcctry(n)
            txtsource.Text = srcctryname(n)



            If ckDeletion(n) = 1 Then
                chckDeletion.Checked = True
            Else
                chckDeletion.Checked = False
            End If
            txtOldLineno.Value = Olineno(n)

            txtacctcode.Text = acccode(n)
            txtacct.Text = accname(n)
            txtcontrolacctcode.Text = controlcode(n)
            txtcontrolacct.Text = controlname(n)


            ddlAccType.Value = acctype(n)
            'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlgAccCode, "Code", "des", "select Code,des from view_account where div_code=" & ViewState("divcode") & "' and type = '" & ddlAccType.Value & "'   order by code", True)
            'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlgAccName, "des", "Code", "select des,Code from view_account where div_code=" & ViewState("divcode") & "' and type = '" & ddlAccType.Value & "'  order by des", True)


            'ddlgAccName.Value = accname(n)
            'ddlgAccCode.Value = acccode(n)

            'ddlConAccCode.Disabled = False
            'ddlConAccName.Disabled = False
            'ddlCCCode.Disabled = True
            'ddlCCName.Disabled = True
            txtConvRate.Disabled = False

            'If acctype(n) = "G" Then
            '    sqlstr1 = " select ''  as controlacctcode, '' as acctname  "
            '    sqlstr2 = " select  '' as acctname , '' as controlacctcode "
            '    ddlConAccCode.Disabled = True
            '    ddlConAccName.Disabled = True
            '    ddlCCCode.Disabled = False
            '    ddlCCName.Disabled = False
            'ElseIf acctype(n) = "C" Then
            '    sqlstr1 = " select distinct view_account.controlacctcode, acctmast.acctname  from acctmast ,view_account where acctmast.div_code=view_account.div_code  and acctmast.div_code='" & ViewState("divcode") & "' and   view_account.controlacctcode= acctmast.acctcode  and type= '" + acctype(n) + "' and view_account.code='" + accname(n) + "' order by  view_account.controlacctcode"
            '    sqlstr2 = " select distinct  acctmast.acctname,view_account.controlacctcode  from acctmast ,view_account where acctmast.div_code=view_account.div_code  and acctmast.div_code='" & ViewState("divcode") & "' and  view_account.controlacctcode= acctmast.acctcode  and type= '" + acctype(n) + "' and view_account.code='" + accname(n) + "' order by  acctmast.acctname"
            'ElseIf acctype(n) = "S" Then
            '    sqlstr1 = " select distinct partymast.controlacctcode  , acctmast.acctname  from acctmast ,partymast where acctmast.div_code='" & ViewState("divcode") & "' and  partymast.controlacctcode= acctmast.acctcode   and partymast.partycode='" + accname(n) + "' order by controlacctcode"

            '    sqlstr2 = " select distinct acctmast.acctname ,partymast.controlacctcode      from acctmast ,partymast where  acctmast.div_code='" & ViewState("divcode") & "' and  partymast.controlacctcode= acctmast.acctcode   and partymast.partycode='" + accname(n) + "' order by acctmast.acctname"
            'ElseIf acctype(n) = "A" Then
            '    sqlstr1 = " select distinct supplier_agents.controlacctcode    , acctmast.acctname  from acctmast ,supplier_agents where acctmast.div_code='" & ViewState("divcode") & "' and  supplier_agents.controlacctcode= acctmast.acctcode   and supplier_agents.supagentcode='" + accname(n) + "' order by controlacctcode"

            '    sqlstr2 = " select distinct acctmast.acctname ,supplier_agents.controlacctcode     from acctmast ,supplier_agents where acctmast.div_code='" & ViewState("divcode") & "' and  supplier_agents.controlacctcode= acctmast.acctcode   and supplier_agents.supagentcode='" + accname(n) + "' order by acctmast.acctname "
            'End If
            'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlConAccCode, "controlacctcode", "acctname", sqlstr1, True)
            'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlConAccName, "acctname", "controlacctcode", sqlstr2, True)

            'ddlConAccCode.Value = controlcode(n)
            'ddlConAccName.Value = controlname(n)

            'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCCCode, "costcenter_code", "costcenter_name", "select costcenter_code, costcenter_name from costcenter_master where active=1 order by costcenter_code ", True)
            'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCCName, "costcenter_name", "costcenter_code", "select costcenter_code, costcenter_name from costcenter_master where active=1 order by costcenter_name ", True)

            '15122014
            'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddldept, "othmaingrpname", "othmaingrpcode", "select othmaingrpcode,othmaingrpname from othmaingrpmast where active=1 order by othmaingrpcode ", True)
            'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddldept, "plgrpname", "plgrpcode", "select plgrpname,plgrpcode from plgrpmast where active=1 order by plgrpname", True)



            txtcostcentercode.Text = CCCode(n)
            txtcostcentername.Text = CCName(n)
            txtcurrencycode.Value = currcode(n)
            txtcurrencyname.Text = currname(n)
            txtConvRate.Value = crate(n)

            txtCredit.Value = credit(n)
            txtBaseCredit.Value = bcredit(n)
            txtDebit.Value = debit(n)
            txtBaseDebit.Value = bdebit(n)

            txtnarr.Text = narration(n)
            If Trim(txtbasecurr.Value) = Trim(txtcurrencycode.Value) Then
                txtConvRate.Disabled = True
            End If

            'ddldept.Value = dept(n)

            n = n + 1
        Next


    End Sub
    'Dim n As Integer = 0
    'Dim count As Integer
    'count = grdJournal.Rows.Count + 1
    'Dim lineno(count) As String
    'Dim acctype(count) As String
    'Dim acccount(count) As String
    'Dim narration(count) As String
    'Dim crate(count) As String
    'Dim currcode(count) As String
    'Dim debit(count) As String
    'Dim credit(count) As String
    'Dim bdebit(count) As String
    'Dim bcredit(count) As String

    'Dim ddlAccType As HtmlSelect
    'Dim ddlgAccCode As HtmlSelect
    'Dim ddlgAccName As HtmlSelect
    'Dim ddlgControlAcc As HtmlSelect
    'Dim ddlCCCode As HtmlSelect
    'Dim ddlCCName As HtmlSelect
    'Dim txtCredit As HtmlInputControl
    'Dim txtDebit As HtmlInputControl
    'Dim txtCurrCode As HtmlInputControl
    'Dim txtCurrRate As HtmlInputControl

    'Dim txtNarr As HtmlInputControl
    'Dim txtBaseCredit As HtmlInputText
    'Dim txtBaseDebit As HtmlInputText
    'Dim lblno As HtmlInputText


    'For Each gvRow In grdJournal.Rows
    '    lblno = gvRow.FindControl("txtlineno")

    '    ddlAccType = gvRow.FindControl("ddlType")
    '    ddlgAccCode = gvRow.FindControl("ddlgAccCode")
    '    ddlgAccName = gvRow.FindControl("ddlgAccName")
    '    txtCurrCode = gvRow.FindControl("txtCurrency")
    '    txtCurrRate = gvRow.FindControl("txtConvRate")
    '    ddlgControlAcc = gvRow.FindControl("ddlConAcc")
    '    ddlCCCode = gvRow.FindControl("ddlCostCode")
    '    ddlCCName = gvRow.FindControl("ddlCostName")

    '    txtDebit = gvRow.FindControl("txtDebit")
    '    txtCredit = gvRow.FindControl("txtCredit")
    '    txtBaseDebit = gvRow.FindControl("txtBaseDebit")
    '    txtBaseCredit = gvRow.FindControl("txtBaseCredit")
    '    lblno = gvRow.FindControl("txtlineno")
    '    txtNarr = gvRow.FindControl("txtgnarration")

    '    If ddlgAccName.Value <> "[Select]" And ddlAccType.Value <> "[Select]" Then
    '        acctype(n) = ddlAccType.Value
    '        acccount(n) = ddlgAccName.Value
    '        currcode(n) = txtCurrCode.Value
    '        crate(n) = txtCurrRate.Value
    '        narration(n) = txtNarr.Value
    '        credit(n) = txtCredit.Value
    '        debit(n) = txtDebit.Value
    '        bcredit(n) = txtBaseCredit.Value
    '        bdebit(n) = txtBaseDebit.Value
    '        n = n + 1
    '    End If
    'Next

    'fillDategrd(grdJournal, False, grdJournal.Rows.Count + 1)
    'Dim i As Integer = n
    'n = 0

    'For Each gvRow In grdJournal.Rows
    '    If n = i Then
    '        Exit For
    '    End If

    '    ddlAccType = gvRow.FindControl("ddlType")
    '    ddlgAccCode = gvRow.FindControl("ddlgAccCode")
    '    ddlgAccName = gvRow.FindControl("ddlgAccName")
    '    txtCurrCode = gvRow.FindControl("txtCurrency")
    '    txtCurrRate = gvRow.FindControl("txtConvRate")
    '    ddlgControlAcc = gvRow.FindControl("ddlConAcc")
    '    ddlCCCode = gvRow.FindControl("ddlCostCode")
    '    ddlCCName = gvRow.FindControl("ddlCostName")

    '    txtDebit = gvRow.FindControl("txtDebit")
    '    txtCredit = gvRow.FindControl("txtCredit")
    '    txtBaseDebit = gvRow.FindControl("txtBaseDebit")
    '    txtBaseCredit = gvRow.FindControl("txtBaseCredit")
    '    lblno = gvRow.FindControl("txtlineno")
    '    txtNarr = gvRow.FindControl("txtgnarration")

    '    ddlAccType.Value = acctype(n)

    '    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"),ddlgAccCode, "Code", "des", "select Code,des from view_account where type = '" & ddlAccType.Value & "'   order by code", True)
    '    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"),ddlgAccName, "des", "Code", "select Code,des from view_account where type = '" & ddlAccType.Value & "'  order by code", True)

    '    ddlgAccName.Value = acccount(n)
    '    ddlgAccCode.Value = ddlgAccName.Items(ddlgAccName.SelectedIndex).Text
    '    ddlgControlAcc.Value = acccount(n)
    '    txtCurrCode.Value = currcode(n)
    '    txtCurrRate.Value = crate(n)
    '    txtNarr.Value = narration(n)
    '    txtCredit.Value = credit(n)
    '    txtBaseCredit.Value = bcredit(n)
    '    txtDebit.Value = debit(n)
    '    txtBaseDebit.Value = bdebit(n)
    '    n = n + 1
    'Next
    ' End Sub
#End Region
    Private Sub FillGrid()
        Dim n As Integer = 0
        Dim count As Integer
        count = grdJournal.Rows.Count
        Dim lineno(count) As String
        Dim Olineno(count) As String
        Dim acctype(count) As String
        Dim acccode(count) As String
        Dim accname(count) As String
        Dim controlcode(count) As String
        Dim controlname(count) As String
        Dim CCCode(count) As String
        Dim CCName(count) As String
        Dim narration(count) As String
        Dim crate(count) As String
        Dim currcode(count) As String
        Dim currname(count) As String
        Dim debit(count) As String
        Dim credit(count) As String
        Dim bdebit(count) As String
        Dim bcredit(count) As String
        Dim ckDeletion(count) As String
        Dim bookingno(count) As String
        Dim dept(count) As String
        Dim sourcectrycode(count) As String
        Dim sourcectryname(count) As String


        Dim ddlAccType As HtmlSelect
        Dim txtacct As TextBox
        Dim txtacctcode As TextBox
        Dim txtcontrolacctcode As TextBox
        Dim txtcontrolacct As TextBox
        Dim txtnarr As TextBox
        Dim txtcostcentercode As TextBox
        Dim txtcostcentername As TextBox
        Dim txtcurrencycode As HtmlInputText
        Dim txtcurrencyname As TextBox
        Dim txtConvRate As HtmlInputText
        Dim txtsourcectrycode As TextBox
        Dim txtsource As TextBox
        Dim txtbookingno As TextBox

        'Dim ddldept As HtmlSelect
        Dim txtDebit, txtBaseDebit, txtCredit, txtBaseCredit, txtOldLineno As HtmlInputText

        Dim chckDeletion As CheckBox
        Dim lblno As HtmlInputText




        For Each gvRow In grdJournal.Rows
            chckDeletion = gvRow.FindControl("chckDeletion")
            lblno = gvRow.FindControl("txtlineno")
            txtOldLineno = gvRow.FindControl("txtOldLineno")
            ddlAccType = gvRow.FindControl("ddlType")
            txtacct = gvRow.FindControl("txtacct")
            txtacctcode = gvRow.FindControl("txtacctCode")
            txtcontrolacctcode = gvRow.FindControl("txtcontrolacctcode")
            txtcontrolacct = gvRow.FindControl("txtcontrolacct")
            txtnarr = gvRow.FindControl("txtnarr")
            txtcostcentercode = gvRow.FindControl("txtcostcentercode")
            txtcostcentername = gvRow.FindControl("txtcostcentername")
            txtcurrencycode = gvRow.FindControl("txtcurrencycode")
            txtcurrencyname = gvRow.FindControl("txtcurrencyname")
            txtConvRate = gvRow.FindControl("txtConvRate")
            txtsourcectrycode = gvRow.FindControl("txtsourcectrycode")
            txtsource = gvRow.FindControl("txtsource")
            txtbookingno = gvRow.FindControl("txtbookingno")


            txtDebit = gvRow.FindControl("txtDebit")
            txtCredit = gvRow.FindControl("txtCredit")
            txtBaseDebit = gvRow.FindControl("txtBaseDebit")
            txtBaseCredit = gvRow.FindControl("txtBaseCredit")


            'ddldept = gvRow.FindControl("ddldept")

            'If txtacctcode.Text <> "" And ddlAccType.Value <> "[Select]" Then
            If chckDeletion.Checked = True Then
                ckDeletion(n) = 1
            Else
                ckDeletion(n) = 0
            End If
            Olineno(n) = txtOldLineno.Value
            acctype(n) = ddlAccType.Value
            acccode(n) = txtacctcode.Text
            accname(n) = txtacct.Text 'ddlgAccName.Value
            If txtcontrolacctcode.Text = "" Then
                txtcontrolacctcode.Text = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select isnull(controlacctcode,'') from view_account where  div_code='" & ViewState("divcode") & "' and type = '" & ddlAccType.Value & "' and code ='" & txtacctcode.Text & "' ")
                If txtcontrolacctcode.Text <> "" Then
                    txtcontrolacct.Text = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select acctname from acctmast where  div_code='" & ViewState("divcode") & "' and acctcode ='" & txtcontrolacctcode.Text & "' ")
                End If
            End If
            controlcode(n) = txtcontrolacctcode.Text
            controlname(n) = txtcontrolacct.Text
            CCCode(n) = txtcostcentercode.Text
            CCName(n) = txtcostcentername.Text

            If txtcurrencycode.Value = "" Then
                txtcurrencycode.Value = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select isnull(cur,'') from view_account where  div_code='" & ViewState("divcode") & "' and type = '" & ddlAccType.Value & "' and code ='" & txtacctcode.Text & "' ")
                If txtcurrencycode.Value <> "" Then
                    txtcurrencyname.Text = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select currname from currmast where   currcode ='" & txtcurrencycode.Value & "' ")
                End If
            End If

            currcode(n) = txtcurrencycode.Value
            crate(n) = txtConvRate.Value
            currname(n) = txtcurrencyname.Text
            credit(n) = txtCredit.Value
            debit(n) = txtDebit.Value
            bcredit(n) = txtBaseCredit.Value
            bdebit(n) = txtBaseDebit.Value
            narration(n) = txtnarr.Text
            sourcectrycode(n) = txtsourcectrycode.Text
            If txtsourcectrycode.Text <> "" Then
                txtsource.Text = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select ctryname from ctrymast where  ctrycode ='" & txtsourcectrycode.Text & "' ")
            End If
            sourcectryname(n) = txtsource.Text
            bookingno(n) = txtbookingno.Text

            'dept(n) = CType(ddldept.Value, String)
            n = n + 1
            'End If
        Next

        fillDategrd(grdJournal, False, grdJournal.Rows.Count)
        Dim i As Integer = n
        n = 0

        For Each gvRow In grdJournal.Rows
            If n = i Then
                Exit For
            End If
            chckDeletion = gvRow.FindControl("chckDeletion")

            lblno = gvRow.FindControl("txtlineno")
            txtOldLineno = gvRow.FindControl("txtOldLineno")
            ddlAccType = gvRow.FindControl("ddlType")

            txtacct = gvRow.FindControl("txtacct")
            txtacctcode = gvRow.FindControl("txtacctCode")
            txtcontrolacctcode = gvRow.FindControl("txtcontrolacctcode")
            txtcontrolacct = gvRow.FindControl("txtcontrolacct")
            txtcostcentercode = gvRow.FindControl("txtcostcentercode")
            txtcostcentername = gvRow.FindControl("txtcostcentername")
            ' ddldept = gvRow.FindControl("ddldept")
            txtsourcectrycode = gvRow.FindControl("txtsourcectrycode")
            txtsource = gvRow.FindControl("txtsource")
            txtbookingno = gvRow.FindControl("txtbookingno")

            txtcurrencycode = gvRow.FindControl("txtcurrencycode")
            txtcurrencyname = gvRow.FindControl("txtcurrencyname")
            txtConvRate = gvRow.FindControl("txtConvRate")


            txtDebit = gvRow.FindControl("txtDebit")
            txtCredit = gvRow.FindControl("txtCredit")
            txtBaseDebit = gvRow.FindControl("txtBaseDebit")
            txtBaseCredit = gvRow.FindControl("txtBaseCredit")

            txtnarr = gvRow.FindControl("txtnarr")


            If ckDeletion(n) = 1 Then
                chckDeletion.Checked = True
            Else
                chckDeletion.Checked = False
            End If
            txtOldLineno.Value = Olineno(n)
            txtacctcode.Text = acccode(n)
            txtacct.Text = accname(n)
            txtcontrolacctcode.Text = controlcode(n)
            txtcontrolacct.Text = controlname(n)
            txtcostcentercode.Text = CCCode(n)
            txtcostcentername.Text = CCName(n)

            ddlAccType.Value = acctype(n)
            txtConvRate.Disabled = False
            If acctype(n) = "G" Then

                txtcontrolacctcode.Enabled = False
                txtcontrolacct.Enabled = False
                txtcostcentercode.Enabled = False
                txtcostcentername.Enabled = False

            End If



            txtcurrencycode.Disabled = True
            txtcurrencycode.Value = currcode(n)
            txtcurrencyname.Text = currname(n)
            txtConvRate.Value = crate(n)
            txtsource.Text = sourcectryname(n)
            txtsourcectrycode.Text = sourcectrycode(n)
            txtCredit.Value = credit(n)
            txtBaseCredit.Value = bcredit(n)
            txtDebit.Value = debit(n)
            txtBaseDebit.Value = bdebit(n)

            txtnarr.Text = narration(n)
            txtbookingno.Text = bookingno(n)
            'ddldept.Value = dept(n)

            If Trim(txtbasecurr.Value) = Trim(txtcurrencycode.Value) Then
                txtConvRate.Disabled = True
            End If

            n = n + 1
        Next
    End Sub
    Protected Sub btnDelLine_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDelLine.Click
        Dim clAdBill As New Collection
        Dim clAdBillnew As New Collection

        Dim intLineNo As Integer = 1
        Dim strLineKey As String

        Dim MainGrdCount As Integer
        Dim credittot, debittot, basecredittot, basedebittot As Decimal
        Dim n As Integer = 0


        Dim prevcount As Integer = grdJournal.Rows.Count

        Dim count As Integer
        count = grdJournal.Rows.Count + 1
        Dim lineno(count) As String
        Dim Olineno(count) As String
        Dim acctype(count) As String
        Dim acccode(count) As String
        Dim accname(count) As String
        Dim controlcode(count) As String
        Dim controlname(count) As String
        Dim CCCode(count) As String
        Dim CCName(count) As String

        Dim narration(count) As String
        Dim crate(count) As String
        Dim currcode(count) As String
        Dim currname(count) As String
        Dim debit(count) As String
        Dim credit(count) As String
        Dim bdebit(count) As String
        Dim bcredit(count) As String

        Dim BookingNo(count) As String
        Dim Sourccectrycode(count) As String
        Dim Sourccectryname(count) As String





        Dim ddlAccType As HtmlSelect
        Dim txtacct As TextBox
        Dim txtacctcode As TextBox
        Dim txtcontrolacctcode As TextBox
        Dim txtcontrolacct As TextBox
        Dim txtnarr As TextBox
        Dim txtcostcentercode As TextBox
        Dim txtcostcentername As TextBox
        Dim txtcurrencycode As HtmlInputText
        Dim txtcurrencyname As TextBox
        Dim txtConvRate As HtmlInputText
        Dim txtsourcectrycode As TextBox
        Dim txtsource As TextBox
        Dim txtbookingno As TextBox

        'Dim ddldept As HtmlSelect
        Dim txtDebit, txtBaseDebit, txtCredit, txtBaseCredit, txtOldLineno As HtmlInputText


        Dim lblno As HtmlInputText







        Dim sqlstr1, sqlstr2 As String
        Dim cntcont, j As Long
        'If Session("Collection").ToString <> "" Then
        '    clAdBill = CType(Session("Collection"), Collection)
        'End If
        clAdBill = GetCollectionFromSession()

        For Each gvRow In grdJournal.Rows
            chckDeletion = gvRow.FindControl("chckDeletion")
            lblno = gvRow.FindControl("txtlineno")
            txtOldLineno = gvRow.FindControl("txtOldLineno")

            ddlAccType = gvRow.FindControl("ddlType")

            txtacct = gvRow.FindControl("txtacct")
            txtacctcode = gvRow.FindControl("txtacctcode")

            txtcontrolacctcode = gvRow.FindControl("txtcontrolacctcode")
            txtcontrolacct = gvRow.FindControl("txtcontrolacct")


            txtcostcentercode = gvRow.FindControl("txtcostcentercode")
            txtcostcentername = gvRow.FindControl("txtcostcentername")

            txtcurrencycode = gvRow.FindControl("txtcurrencycode")
            txtcurrencyname = gvRow.FindControl("txtcurrencyname")

            txtConvRate = gvRow.FindControl("txtConvRate")
            txtDebit = gvRow.FindControl("txtDebit")
            txtCredit = gvRow.FindControl("txtCredit")
            txtBaseDebit = gvRow.FindControl("txtBaseDebit")
            txtBaseCredit = gvRow.FindControl("txtBaseCredit")

            'ddldept = gvRow.FindControl("ddldept")


            txtnarr = gvRow.FindControl("txtNarr")




            txtsourcectrycode = gvRow.FindControl("txtsourcectrycode")
            txtsource = gvRow.FindControl("txtsource")
            txtbookingno = gvRow.FindControl("txtbookingno")


            If chckDeletion.Checked = True Then

                cntcont = clAdBill.Count / 21
                For j = 1 To cntcont
                    strLineKey = j & ":" & lblno.Value
                    DeleteCollection(clAdBill, "AgainstTranLineNo" & strLineKey)
                    DeleteCollection(clAdBill, "AccTranLineNo" & strLineKey)
                    DeleteCollection(clAdBill, "TranId" & strLineKey)
                    DeleteCollection(clAdBill, "TranDate" & strLineKey)
                    DeleteCollection(clAdBill, "TranType" & strLineKey)
                    DeleteCollection(clAdBill, "DueDate" & strLineKey)
                    DeleteCollection(clAdBill, "CurrRate" & strLineKey)
                    DeleteCollection(clAdBill, "Credit" & strLineKey)
                    DeleteCollection(clAdBill, "Debit" & strLineKey)
                    DeleteCollection(clAdBill, "BaseCredit" & strLineKey)
                    DeleteCollection(clAdBill, "BaseDebit" & strLineKey)
                    DeleteCollection(clAdBill, "RefNo" & strLineKey)
                    DeleteCollection(clAdBill, "Field2" & strLineKey)
                    DeleteCollection(clAdBill, "Field3" & strLineKey)
                    DeleteCollection(clAdBill, "Field4" & strLineKey)
                    DeleteCollection(clAdBill, "Field5" & strLineKey)
                    DeleteCollection(clAdBill, "OpenMode" & strLineKey)
                    DeleteCollection(clAdBill, "AccType" & strLineKey)
                    DeleteCollection(clAdBill, "AccCode" & strLineKey)
                    DeleteCollection(clAdBill, "AccGLCode" & strLineKey)
                    DeleteCollection(clAdBill, "AdjustBaseTotal" & strLineKey)
                Next
            Else
                'If txtacctcode.Text <> "" And ddlAccType.Value <> "[Select]" Then
                lineno(n) = lblno.Value
                Olineno(n) = txtOldLineno.Value
                acctype(n) = ddlAccType.Value
                acccode(n) = txtacctcode.Text
                accname(n) = txtacct.Text 'ddlgAccName.Value
                controlcode(n) = txtcontrolacctcode.Text
                controlname(n) = txtcontrolacct.Text
                CCCode(n) = CType(txtcostcentercode.Text, String)
                CCName(n) = CType(txtcostcentername.Text, String)
                BookingNo(n) = CType(txtbookingno.Text, String)
                Sourccectrycode(n) = CType(txtsourcectrycode.Text, String)
                Sourccectryname(n) = CType(txtsource.Text, String)

                currcode(n) = txtcurrencycode.Value
                currname(n) = txtcurrencyname.Text
                crate(n) = txtConvRate.Value

                credit(n) = txtCredit.Value
                debit(n) = txtDebit.Value
                bcredit(n) = txtBaseCredit.Value
                bdebit(n) = txtBaseDebit.Value

                ' dept(n) = CType(ddldept.Value, String)

                narration(n) = txtnarr.Text
                n = n + 1
                ' End If
            End If
        Next
        'Session.Add("Collection", clAdBill)
        Dim collectionDate As Collection
        Dim strLineKeynew As String
        Dim sno As Integer
        'If Session("Collection").ToString <> "" Then
        '    collectionDate = CType(Session("Collection"), Collection)
        'End If

        collectionDate = clAdBill

        Dim grdct As Long
        grdct = n
        If grdct = 0 Then
            grdct = 1
        End If
        fillDategrd(grdJournal, False, grdct)
        Dim i As Integer = n
        n = 0

        For Each gvRow In grdJournal.Rows
            If n = i Then
                Exit For
            End If

            lblno = gvRow.FindControl("txtlineno")
            txtOldLineno = gvRow.FindControl("txtOldLineno")

            ddlAccType = gvRow.FindControl("ddlType")

            txtacct = gvRow.FindControl("txtacct")
            txtacctcode = gvRow.FindControl("txtacctcode")


            txtcontrolacctcode = gvRow.FindControl("txtcontrolacctcode")
            txtcontrolacct = gvRow.FindControl("txtcontrolacct")


            txtcostcentercode = gvRow.FindControl("txtcostcentercode")
            txtcostcentername = gvRow.FindControl("txtcostcentername")

            txtcurrencycode = gvRow.FindControl("txtcurrencycode")
            txtcurrencyname = gvRow.FindControl("txtcurrencyname")

            txtDebit = gvRow.FindControl("txtDebit")
            txtCredit = gvRow.FindControl("txtCredit")
            txtBaseDebit = gvRow.FindControl("txtBaseDebit")
            txtBaseCredit = gvRow.FindControl("txtBaseCredit")
            txtConvRate = gvRow.FindControl("txtConvRate")
            ' ddldept = gvRow.FindControl("ddldept")

            txtnarr = gvRow.FindControl("txtNarr")
            'txtacctcode = gvRow.FindControl("txtacctcode")
            txtsourcectrycode = gvRow.FindControl("txtsourcectrycode")
            txtsource = gvRow.FindControl("txtsource")
            txtbookingno = gvRow.FindControl("txtbookingno")

            txtOldLineno.Value = Olineno(n)
            txtacctcode.Text = acccode(n)
            txtacct.Text = accname(n)
            txtcontrolacctcode.Text = controlcode(n)
            txtcontrolacct.Text = controlname(n)

            ddlAccType.Value = acctype(n)
            'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlgAccCode, "Code", "des", "select Code,des from view_account where div_code='" & ViewState("divcode") & "' and type = '" & ddlAccType.Value & "'   order by code", True)
            'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlgAccName, "des", "Code", "select Code,des from view_account where div_code='" & ViewState("divcode") & "' and type = '" & ddlAccType.Value & "'  order by des", True)


            txtacct.Text = accname(n)
            txtacctcode.Text = acccode(n)

            'txtcontrolacctcode.Disabled = False
            'txtcontrolacct.Disabled = False
            'txtcostcentercode.Disabled = True
            'txtcostcentername.Disabled = True
            'If acctype(n) = "G" Then
            '    sqlstr1 = " select ''  as controlacctcode, '' as acctname  "
            '    sqlstr2 = " select  '' as acctname , '' as controlacctcode "
            '    ddlConAccCode.Disabled = True
            '    ddlConAccName.Disabled = True
            '    ddlCCCode.Disabled = False
            '    ddlCCName.Disabled = False
            'ElseIf acctype(n) = "C" Then
            '    sqlstr1 = " select distinct view_account.controlacctcode, acctmast.acctname  from acctmast ,view_account where acctmast.div_code=view_account.div_code and acctmast.div_code='" & ViewState("divcode") & "' and   view_account.controlacctcode= acctmast.acctcode  and type= '" + acctype(n) + "' and view_account.code='" + accname(n) + "' order by  view_account.controlacctcode"
            '    sqlstr2 = " select distinct  acctmast.acctname,view_account.controlacctcode  from acctmast ,view_account where acctmast.div_code=view_account.div_code and acctmast.div_code='" & ViewState("divcode") & "' and   view_account.controlacctcode= acctmast.acctcode  and type= '" + acctype(n) + "' and view_account.code='" + accname(n) + "' order by  acctmast.acctname"
            'ElseIf acctype(n) = "S" Then
            '    sqlstr1 = " select distinct partymast.controlacctcode  , acctmast.acctname  from acctmast ,partymast where acctmast.div_code='" & ViewState("divcode") & "' and   partymast.controlacctcode= acctmast.acctcode   and partymast.partycode='" + accname(n) + "' order by controlacctcode"

            '    sqlstr2 = " select distinct acctmast.acctname ,partymast.controlacctcode      from acctmast ,partymast where  acctmast.div_code='" & ViewState("divcode") & "' and  partymast.controlacctcode= acctmast.acctcode   and partymast.partycode='" + accname(n) + "' order by acctmast.acctname"
            'ElseIf acctype(n) = "A" Then
            '    sqlstr1 = " select distinct supplier_agents.controlacctcode    , acctmast.acctname  from acctmast ,supplier_agents where acctmast.div_code='" & ViewState("divcode") & "' and   supplier_agents.controlacctcode= acctmast.acctcode   and supplier_agents.supagentcode='" + accname(n) + "' order by controlacctcode"

            '    sqlstr2 = " select distinct acctmast.acctname ,supplier_agents.controlacctcode     from acctmast ,supplier_agents where acctmast.div_code='" & ViewState("divcode") & "' and  supplier_agents.controlacctcode= acctmast.acctcode   and supplier_agents.supagentcode='" + accname(n) + "' order by acctmast.acctname"

            'End If
            'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlConAccCode, "controlacctcode", "acctname", sqlstr1, True)
            'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlConAccName, "acctname", "controlacctcode", sqlstr2, True)


            txtcontrolacctcode.Text = controlcode(n)
            txtcontrolacct.Text = controlname(n)

            'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCCCode, "costcenter_code", "costcenter_name", "select costcenter_code, costcenter_name from costcenter_master where active=1 order by costcenter_code ", True)
            'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCCName, "costcenter_name", "costcenter_code", "select costcenter_code, costcenter_name from costcenter_master where active=1 order by costcenter_name ", True)

            ''15122014
            ''objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddldept, "othmaingrpname", "othmaingrpcode", "select othmaingrpcode,othmaingrpname from othmaingrpmast where active=1 order by othmaingrpcode ", True)
            'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddldept, "plgrpname", "plgrpcode", "select plgrpname,plgrpcode from plgrpmast where active=1 order by plgrpname", True)

            ' ddldept.Value = dept(n)

            txtcostcentercode.Text = CCCode(n)
            txtcostcentername.Text = CCName(n)

            txtcurrencycode.Value = currcode(n)
            txtcurrencyname.Text = currname(n)
            txtConvRate.Value = crate(n)

            txtCredit.Value = credit(n)
            txtBaseCredit.Value = bcredit(n)
            txtDebit.Value = debit(n)
            txtBaseDebit.Value = bdebit(n)

            txtnarr.Text = narration(n)

            txtsourcectrycode.Text = Sourccectrycode(n)
            txtsource.Text = Sourccectryname(n)
            txtbookingno.Text = BookingNo(n)

            credittot = DecRound(DecRound(credittot) + DecRound(CType(IIf(txtCredit.Value = "", 0, txtCredit.Value), Decimal)))
            basecredittot = DecRound(DecRound(basecredittot) + DecRound(CType(IIf(txtBaseCredit.Value = "", 0, txtBaseCredit.Value), Decimal)))
            debittot = DecRound(DecRound(debittot) + DecRound(CType(IIf(txtDebit.Value = "", 0, txtDebit.Value), Decimal)))
            basedebittot = DecRound(DecRound(basedebittot) + DecRound(CType(IIf(txtBaseDebit.Value = "", 0, txtBaseDebit.Value), Decimal)))

            If Trim(txtbasecurr.Value) = Trim(txtcurrencycode.Value) Then
                txtConvRate.Disabled = True
            End If
            '   lineno(count)


            sno = 1
            If acctype(n) <> "G" Then
                MainGrdCount = grdJournal.Rows.Count  ' lineno.Length
                Dim MainRowidx As Integer
                MainRowidx = 1
                For MainRowidx = 0 To MainGrdCount - 1
                    cntcont = collectionDate.Count / 21
                    sno = 1
                    For j = 1 To cntcont
                        strLineKey = j & ":" & lineno(MainRowidx)
                        strLineKeynew = sno & ":" & lblno.Value
                        If colexists(clAdBillnew, "AgainstTranLineNo" & strLineKeynew) = False Then
                            If colexists(collectionDate, "AgainstTranLineNo" & strLineKey) = True Then
                                'sharfudeen 03/11/2022
                                'If collectionDate("AccCode" & strLineKey).ToString = acccode(n) And collectionDate("AccType" & strLineKey).ToString = acctype(n) And collectionDate("AccGLCode" & strLineKey).ToString = controlcode(n) And (DecRound(CType(collectionDate("AdjustBaseTotal" & strLineKey), Decimal)) = IIf(bcredit(n) = "", 0, bcredit(n))) Or DecRound(CType(collectionDate("AdjustBaseTotal" & strLineKey), Decimal)) = IIf(bdebit(n) = "", 0, bdebit(n)) And CType(collectionDate("CurrRate" & strLineKey), Decimal) = Val(CType(IIf(crate(n) = "", 0, crate(n)), Decimal)) And collectionDate("AgainstTranLineNo" & strLineKey) = lineno(MainRowidx) Then

                                If collectionDate("AccCode" & strLineKey).ToString = acccode(n) And collectionDate("AccType" & strLineKey).ToString = acctype(n) And collectionDate("AccGLCode" & strLineKey).ToString = controlcode(n) And collectionDate("AgainstTranLineNo" & strLineKey) = lineno(MainRowidx) Then
                                    AddCollection(clAdBillnew, "AgainstTranLineNo" & strLineKeynew, lblno.Value) 'collectionDate("AgainstTranLineNo" & strLineKey))
                                    AddCollection(clAdBillnew, "AccTranLineNo" & strLineKeynew, collectionDate("AccTranLineNo" & strLineKey))
                                    AddCollection(clAdBillnew, "TranId" & strLineKeynew, collectionDate("TranId" & strLineKey).ToString)
                                    AddCollection(clAdBillnew, "TranDate" & strLineKeynew, Format(CType(collectionDate("TranDate" & strLineKey).ToString, Date), "dd/MM/yyyy"))
                                    AddCollection(clAdBillnew, "TranType" & strLineKeynew, collectionDate("TranType" & strLineKey).ToString)
                                    AddCollection(clAdBillnew, "DueDate" & strLineKeynew, Format(CType(collectionDate("DueDate" & strLineKey).ToString, Date), "dd/MM/yyyy"))
                                    AddCollection(clAdBillnew, "CurrRate" & strLineKeynew, CType(collectionDate("CurrRate" & strLineKey), Decimal))
                                    AddCollection(clAdBillnew, "Credit" & strLineKeynew, DecRound(CType(collectionDate("Credit" & strLineKey), Decimal)))
                                    AddCollection(clAdBillnew, "Debit" & strLineKeynew, DecRound(CType(collectionDate("Debit" & strLineKey), Decimal)))
                                    AddCollection(clAdBillnew, "BaseCredit" & strLineKeynew, DecRound(CType(collectionDate("BaseCredit" & strLineKey), Decimal)))
                                    AddCollection(clAdBillnew, "BaseDebit" & strLineKeynew, DecRound(CType(collectionDate("BaseDebit" & strLineKey), Decimal)))
                                    AddCollection(clAdBillnew, "RefNo" & strLineKeynew, collectionDate("RefNo" & strLineKey).ToString)
                                    AddCollection(clAdBillnew, "Field2" & strLineKeynew, collectionDate("Field2" & strLineKey).ToString)
                                    AddCollection(clAdBillnew, "Field3" & strLineKeynew, collectionDate("Field3" & strLineKey).ToString)
                                    AddCollection(clAdBillnew, "Field4" & strLineKeynew, collectionDate("Field4" & strLineKey).ToString)
                                    AddCollection(clAdBillnew, "Field5" & strLineKeynew, collectionDate("Field5" & strLineKey).ToString)
                                    AddCollection(clAdBillnew, "OpenMode" & strLineKeynew, collectionDate("OpenMode" & strLineKey).ToString)
                                    AddCollection(clAdBillnew, "AccType" & strLineKeynew, collectionDate("AccType" & strLineKey).ToString)
                                    AddCollection(clAdBillnew, "AccCode" & strLineKeynew, collectionDate("AccCode" & strLineKey).ToString)
                                    AddCollection(clAdBillnew, "AccGLCode" & strLineKeynew, collectionDate("AccGLCode" & strLineKey).ToString)
                                    AddCollection(clAdBillnew, "AdjustBaseTotal" & strLineKeynew, DecRound(CType(collectionDate("AdjustBaseTotal" & strLineKey), Decimal)))


                                    DeleteCollection(collectionDate, "AgainstTranLineNo" & strLineKey)
                                    DeleteCollection(collectionDate, "AccTranLineNo" & strLineKey)
                                    DeleteCollection(collectionDate, "TranId" & strLineKey)
                                    DeleteCollection(collectionDate, "TranDate" & strLineKey)
                                    DeleteCollection(collectionDate, "TranType" & strLineKey)
                                    DeleteCollection(collectionDate, "DueDate" & strLineKey)
                                    DeleteCollection(collectionDate, "CurrRate" & strLineKey)
                                    DeleteCollection(collectionDate, "Credit" & strLineKey)
                                    DeleteCollection(collectionDate, "Debit" & strLineKey)
                                    DeleteCollection(collectionDate, "BaseCredit" & strLineKey)
                                    DeleteCollection(collectionDate, "BaseDebit" & strLineKey)
                                    DeleteCollection(collectionDate, "RefNo" & strLineKey)
                                    DeleteCollection(collectionDate, "Field2" & strLineKey)
                                    DeleteCollection(collectionDate, "Field3" & strLineKey)
                                    DeleteCollection(collectionDate, "Field4" & strLineKey)
                                    DeleteCollection(collectionDate, "Field5" & strLineKey)
                                    DeleteCollection(collectionDate, "OpenMode" & strLineKey)
                                    DeleteCollection(collectionDate, "AccType" & strLineKey)
                                    DeleteCollection(collectionDate, "AccCode" & strLineKey)
                                    DeleteCollection(collectionDate, "AccGLCode" & strLineKey)
                                    sno = sno + 1
                                End If
                            End If
                            'Else
                            '    Exit For
                        End If
                    Next
                Next
            End If


            n = n + 1
        Next

        'Session.Add("Collection", clAdBillnew)
        Session.Add("Collection" & ":" & txtAdjcolno.Value, clAdBillnew)

        txtTotalCredit.Value = DecRound(Val(credittot))
        txtTotalDebit.Value = DecRound(Val(debittot))
        txtTotBaseCredit.Value = DecRound(Val(basecredittot))
        txtTotBaseDebit.Value = DecRound(Val(basedebittot))
    End Sub
    Public Sub DeleteCollection(ByVal dataCollection As Collection, ByVal strKey As String)
        If colexists(dataCollection, strKey) = True Then
            dataCollection.Remove(strKey)
        End If
    End Sub
    Public Function DecRound(ByVal Ramt As Decimal) As Decimal
        Dim Rdamt As Decimal
        Rdamt = Math.Round(Val(Ramt), CType(txtdecimal.Value, Integer))
        Return Rdamt
    End Function
    Protected Sub btnhelp_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=Journal','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub
    Protected Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        Try
            'If MsgBox("Do you want to print", MsgBoxStyle.YesNo, "Doc Print") = MsgBoxResult.No Then
            '    Exit Sub
            'End If
            'ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Do you want to print' );", True)
            'Dim strReportTitle As String = ""
            'Dim strSelectionFormula As String = ""
            'Session.Add("RefCode", CType(txtDocNo.Value.Trim, String))
            'Session.Add("Pageame", "JournalDoc")
            'Session.Add("BackPageName", "~\AccountsModule\Journal.aspx?tran_type=" & CType(Session("RVPVTranType"), String) & "")

            'strSelectionFormula = ""
            'If txtDocNo.Value.Trim <> "" Then
            '    If Trim(strSelectionFormula) = "" Then
            '        'strReportTitle = "Doc No : " & txtDocNo.Value.Trim
            '        strSelectionFormula = " {journal_master.tran_id}='" & txtDocNo.Value.Trim & "'"
            '    Else
            '        'strReportTitle = strReportTitle & "Vocher No : " & txtDocNo.Value.Trim & "'"
            '        strSelectionFormula = strSelectionFormula & " {journal_master.tran_id}='" & txtDocNo.Value.Trim & "'"
            '    End If
            'Else
            '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Enter Tran ID' );", True)
            '    Exit Sub
            'End If

            'If Trim(strSelectionFormula) = "" Then
            '    strSelectionFormula = " {journal_master.tran_type} = '" & txtTranType.Text & "' " & _
            '    " and  {journal_master.journal_div_id} = '" & CType(objUtils.GetDBFieldFromLongnew(Session("dbconnectionName"),"reservation_parameters", "option_selected", "param_id", 511), String) & "'"
            'Else
            '    strSelectionFormula = strSelectionFormula & " AND {journal_master.tran_type} = '" & txtTranType.Text & "'" & _
            '    " and  {journal_master.journal_div_id} = '" & CType(objUtils.GetDBFieldFromLongnew(Session("dbconnectionName"),"reservation_parameters", "option_selected", "param_id", 511), String) & "'"
            'End If
            ''Dim lblstr As String
            ''lblstr = ""
            ''If Session("RVPVTranType") = "RV" Then
            ''    lblstr = "Receipt Voucher"
            ''ElseIf Session("RVPVTranType") = "PV" Then
            ''    lblstr = "Payment Voucher"
            ''End If
            'Session.Add("SelectionFormula", strSelectionFormula)
            'Session.Add("ReportTitle", strReportTitle)
            'Session.Add("PrinDocTitle", "Journal Voucher")

            'Dim ScriptStr As String
            'ScriptStr = "<script language=""javascript"">var win=window.open('../PriceListModule/PrintDoc.aspx','printdoc','toolbar=0,scrollbars=yes,resizable=yes,titlebar=0,menubar=0,locationbar=0,modal=1,statusbar=1');</script>"

            'ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", ScriptStr, False)


            Dim ScriptStr As String
            ScriptStr = "<script language=""javascript"">var win=window.open('PrintDocNew.aspx?Pageame=JournalDoc&BackPageName=~\AccountsModule\JournalSearch.aspx&Tranid=" & txtDocNo.Value & "&divid=" & ViewState("divcode") & "&TranType=" & txtTranType.Text & "&PrntSec=0','printdoc');</script>"
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", ScriptStr, False)


        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("Journal.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
    Private Function validate_BillAgainst() As Boolean
        Try
            validate_BillAgainst = True
            Dim Alflg As Integer
            Dim ErrMsg, strdiv As String
            strdiv = ViewState("divcode") ' objUtils.GetDBFieldFromLongnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", 511)

            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
            myCommand = New SqlCommand("sp_Check_AgainstBills", SqlConn)
            myCommand.CommandType = CommandType.StoredProcedure
            myCommand.Parameters.Add(New SqlParameter("@div_id", SqlDbType.VarChar, 10)).Value = strdiv
            myCommand.Parameters.Add(New SqlParameter("@tran_id", SqlDbType.VarChar, 20)).Value = txtDocNo.Value
            myCommand.Parameters.Add(New SqlParameter("@tran_type ", SqlDbType.VarChar, 10)).Value = CType(ViewState("JournalTranType"), String)

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
            objUtils.WritErrorLog("Journal.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
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
    Private Sub CheckPostUnpostRight(ByVal UserName As String, ByVal UserPwd As String, ByVal AppName As String, ByVal PageName As String, ByVal appid As String)
        Dim PostUnpostFlag As Boolean = False
        PostUnpostFlag = objUser.PostUnpostRightnew(Session("dbconnectionName"), UserName, UserPwd, AppName, PageName, appid)
        If PostUnpostFlag = True Then
            chkPost.Visible = True
            lblPostmsg.Visible = True
        Else
            chkPost.Visible = False
            lblPostmsg.Visible = False
            If ViewState("JournalState") = "Edit" Then
                If chkPost.Checked = True Then
                    ViewState.Add("JournalState", "View")
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This transaction has been posted, you do not have rights to edit.' );", True)
                End If
            End If
        End If
    End Sub

    Protected Sub btnGenGrid_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGenGrid.Click
        If txtNoofRows.Value = "0" Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Cannot create 0 no of rows.' );", True)
            Return
        End If
        fillDategrd(grdJournal, False, txtNoofRows.Value)
        btnGenGrid.Enabled = False
        ShowControls()

        If ViewState("JournalState") = "Edit" Then
            'If txtNoofRows.Value > hdnRows.Value Then
            '    Dim norows As Integer = CType(hdnRows.Value, Integer) + CType(txtNoofRows.Value, Integer)
            '    fillDategrd(grdJournal, False, norows)
            'End If
            btnGenGrid.Enabled = False
            'ShowControls()
            'show_record(txtDocNo.Value.Trim())
            'ShowFillGrid(txtDocNo.Value.Trim())
            'fillcollection(txtDocNo.Value.Trim())
        End If

    End Sub
    Public Sub HideControls()
        'btnSave.Visible = False
        'grdJournal.Visible = False
        'btnAdd.Visible = False
        'btnDelLine.Visible = False
        'Label7.Visible = False
        'txtTotalDebit.Visible = False
        'txtTotalCredit.Visible = False
        'lblBaseTot.Visible = False
        'txtTotBaseDebit.Visible = False
        'txtTotBaseCredit.Visible = False
        'lblBaseDiff.Visible = False
        'txtTotBaseDiff.Visible = False
        'chkBlank.Visible = False
        'lblblank.Visible = False
        'chkPost.Visible = False
        btnSave.Style("visibility") = "hidden"
        grdJournal.Style("visibility") = "hidden"
        btnAdd.Style("visibility") = "hidden"
        btnDelLine.Style("visibility") = "hidden"
        Label7.Style("visibility") = "hidden"
        txtTotalDebit.Style("visibility") = "hidden"
        txtTotalCredit.Style("visibility") = "hidden"
        lblBaseTot.Style("visibility") = "hidden"
        txtTotBaseDebit.Style("visibility") = "hidden"
        txtTotBaseCredit.Style("visibility") = "hidden"
        lblBaseDiff.Style("visibility") = "hidden"
        txtTotBaseDiff.Style("visibility") = "hidden"
        'chkBlank.Style("visibility") = "hidden"
        'lblblank.Style("visibility") = "hidden"
        'chkPost.Style("visibility") = "hidden"

        'Commented chkPost style:hidden by Archana on 01/04/2015

    End Sub
    Public Sub ShowControls()
        'btnSave.Visible = True
        'grdJournal.Visible = True
        'btnAdd.Visible = True
        'btnDelLine.Visible = True
        'Label7.Visible = True
        'txtTotalDebit.Visible = True
        'txtTotalCredit.Visible = True
        'lblBaseTot.Visible = True
        'txtTotBaseDebit.Visible = True
        'txtTotBaseCredit.Visible = True
        'lblBaseDiff.Visible = True
        'txtTotBaseDiff.Visible = True
        'chkBlank.Visible = True
        'lblblank.Visible = True
        'chkPost.Visible = True

        btnSave.Style("visibility") = "visible"
        grdJournal.Style("visibility") = "visible"
        btnAdd.Style("visibility") = "visible"
        btnDelLine.Style("visibility") = "visible"
        Label7.Style("visibility") = "visible"
        txtTotalDebit.Style("visibility") = "visible"
        txtTotalCredit.Style("visibility") = "visible"
        lblBaseTot.Style("visibility") = "visible"
        txtTotBaseDebit.Style("visibility") = "visible"
        txtTotBaseCredit.Style("visibility") = "visible"
        lblBaseDiff.Style("visibility") = "visible"
        txtTotBaseDiff.Style("visibility") = "visible"
        'chkBlank.Style("visibility") = "visible"
        'lblblank.Style("visibility") = "visible"
        chkPost.Style("visibility") = "visible"
    End Sub

    Private Function GetScalarValue(ByVal dbconnection As String, ByVal strQry As String) As Object
        Try
            SqlConn = clsDBConnect.dbConnectionnew(dbconnection)
            myCommand = New SqlCommand(strQry, SqlConn)
            Return myCommand.ExecuteScalar()
        Catch ex As Exception
            Return ""
        End Try
    End Function

    Protected Sub btnPdfReport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPdfReport.Click
        Try
            Dim ScriptStr As String
            ScriptStr = "<script language=""javascript"">var win=window.open('TransactionReports.aspx?printId=JournalDoc&Tranid=" & txtDocNo.Value & "&divid=" & ViewState("divcode") & "&TranType=" & txtTranType.Text & "&PrntSec=0','printdoc');</script>"
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", ScriptStr, False)


        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("Journal.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub

End Class


'txtCurrRate.Attributes.Add("onkeypress", "return checkNumberDecimal(event,this)")
'txtCredit.Attributes.Add("onkeypress", "return checkNumberDecimal(event,this)")
'txtDebit.Attributes.Add("onkeypress", "return checkNumberDecimal(event,this)")



'txtDebit.Attributes.Add("onchange", "javascript:calBaseAmount('" + CType(txtCredit.ClientID, String) + "','" + CType(txtDebit.ClientID, String) + "','" + CType(txtCurrRate.ClientID, String) + "','" + CType(txtBaseDebit.ClientID, String) + "')")
'txtCredit.Attributes.Add("onchange", "javascript:calBaseAmount('" + CType(txtDebit.ClientID, String) + "','" + CType(txtCredit.ClientID, String) + "','" + CType(txtCurrRate.ClientID, String) + "','" + CType(txtBaseCredit.ClientID, String) + "')")
'txtCurrRate.Attributes.Add("onchange", "javascript:CalRateChange('" + CType(txtDebit.ClientID, String) + "','" + CType(txtCredit.ClientID, String) + "','" + CType(txtCurrRate.ClientID, String) + "','" + CType(txtBaseDebit.ClientID, String) + "','" + CType(txtBaseCredit.ClientID, String) + "')")


' ddlAccType.Attributes.Add("onchange", "javascript:FillAccCode('" + CType(ddlAccType.ClientID, String) + "','" + CType(ddlgAccCode.ClientID, String) + "','" + CType(ddlgAccName.ClientID, String) + "','" + CType(ddlgControlAcc.ClientID, String) + "','" + CType(txtgnarration.ClientID, String) + "')")
'ddlgAccCode.Attributes.Add("onchange", "javascript:FillGACode('" + CType(ddlgAccCode.ClientID, String) + "','" + CType(ddlgAccName.ClientID, String) + "','" + CType(txtCurrCode.ClientID, String) + "','" + CType(txtCurrRate.ClientID, String) + "','" + CType(ddlgControlAcc.ClientID, String) + "')")
'ddlgAccName.Attributes.Add("onchange", "javascript:FillGAName('" + CType(ddlgAccCode.ClientID, String) + "','" + CType(ddlgAccName.ClientID, String) + "','" + CType(txtCurrCode.ClientID, String) + "','" + CType(txtCurrRate.ClientID, String) + "','" + CType(ddlgControlAcc.ClientID, String) + "')")
'If objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"),"docgen", "optionname", "optionname", "JV") = Nothing Then
'    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('user couldn’t  save record.');", True)
'    Exit Sub
'End If

