'------------================--------------=======================------------------================
'   Module Name    :    CitiesSearch.aspx
'   Developer Name :    D'Silva Azia
'   Date           :    16 july 2008
'------------================--------------=======================------------------================


#Region "Namespace"
Imports System.Data
Imports System.Data.SqlClient
#End Region



#Region "Enum GridCol"
Enum GridCol
    ' CategoryCodeTCol = 0
    FromCurrency = 1
    ToCurrency = 2
    Conversion = 3
    DateCreated = 4
    UserCreated = 5
    DateModified = 6
    UserModified = 7

End Enum
#End Region

Partial Class CurrencyConversionRates
    Inherits System.Web.UI.Page

#Region "Global Declarations"
    Dim objUtils As New clsUtils
    Dim objUser As New clsUser
    Dim strSqlQry As String
    Dim strWhereCond As String
    Dim sqlTrans As SqlTransaction
    Dim SqlConn As New SqlConnection
    Dim myCommand As SqlCommand
    Dim myDataAdapter As SqlDataAdapter
    Dim mySqlReader As SqlDataReader
    Dim GvRow As GridViewRow
    Dim txtconvert As HtmlInputText
#End Region
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack = False Then
            Try
                SetFocus("txtconvert")
                Dim AppId As String = CType(Request.QueryString("appid"), String)
                Dim AppName As HiddenField = CType(Master.FindControl("hdnAppName"), HiddenField)
                Dim strappid As String = ""
                Dim strappname As String = ""

                If AppId Is Nothing = False Then
                    strappid = AppId
                End If

                If AppName Is Nothing = False Then

                    If AppId = "4" Then

                        strappname = AppName.Value
                    ElseIf AppId = "14" Then

                        strappname = AppName.Value
                    Else
                        strappname = AppName.Value
                    End If
                End If

                If CType(Session("GlobalUserName"), String) = Nothing Or CType(Session("Userpwd"), String) = Nothing Then
                    Response.Redirect("~/Login.aspx", False)
                    Exit Sub
                   
                Else
                    objUser.CheckUserRight(Session("dbconnectionName"), CType(Session("GlobalUserName"), String), CType(Session("Userpwd"), String), _
                                                                      CType(strappname, String), "PriceListModule\Currencyconversionrates.aspx?appid=" + strappid, btnadd, btnExport, btnReport, gv_SearchResult)
                End If

                Session.Add("strsortExpression", "currcode")
                Session.Add("strsortdirection", SortDirection.Ascending)
                FillDDL()
                If Request.QueryString("FromCurr") <> "" Then
                    ddlFromCurrency.Value = Request.QueryString("FromCurr")
                    hdnfromcurrency.Value = ddlFromCurrency.Value
                End If
                FillGrid("currcode")
                'fillgrd(gv_SearchResult, True)

                For Each GvRow In gv_SearchResult.Rows
                    txtconvert = GvRow.FindControl("txtConversion")
                    HTMLNumbers(txtconvert)
                Next
            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objUtils.WritErrorLog("CurrencyConversionRates.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            End Try
            btnSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to Save?')==false)return false;")
            btnUpdate.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to Save?')==false)return false;")
            btnExit.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to Exit?')==false)return false;")
            btnClose.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to Exit?')==false)return false;")
        Else
            objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlFromCurrency, "currcode", "currcode", "select distinct currcode from currmast", True)
            objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlToCurrency, "currcode", "currcode", "select distinct currcode from currmast", True)
            If hdnfromcurrency.Value <> "" Then
                ddlFromCurrency.Value = hdnfromcurrency.Value
            End If
            If hdntocurrency.Value <> "" Then
                ddlToCurrency.Value = hdntocurrency.Value
            End If
        End If
    End Sub


    Protected Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
        If Page.IsPostBack = False Then
            If Request.QueryString("appid") Is Nothing = False Then
                Dim appid As String = CType(Request.QueryString("appid"), String)
                Select Case appid
                    Case 1
                        Me.MasterPageFile = "~/PriceListMaster.master"
                    Case 2
                        Me.MasterPageFile = "~/RoomBlock.master"
                    Case 3
                        Me.MasterPageFile = "~/ReservationMaster.master"
                    Case 4
                        Me.MasterPageFile = "~/AccountsMaster.master"
                    Case 5
                        Me.MasterPageFile = "~/UserAdminMaster.master"
                    Case 6
                        Me.MasterPageFile = "~/WebAdminMaster.master"
                    Case 7
                        Me.MasterPageFile = "~/TransferHistoryMaster.master"
                    Case 10
                        Me.MasterPageFile = "~/TransferMaster.master"
                    Case 11
                        Me.MasterPageFile = "~/ExcursionMaster.master"
                    Case 14
                        Me.MasterPageFile = "~/AccountsMaster.master"
                    Case 13
                        Me.MasterPageFile = "~/VisaMaster.master"      'Added by Archana on 05/06/2015 for VisaModule
                    Case 16
                        Me.MasterPageFile = "~/AccountsMaster.master"   'changed by mohamed on 27/08/2018
                    Case Else
                        Me.MasterPageFile = "~/SubPageMaster.master"
                End Select
            Else
                Me.MasterPageFile = "~/SubPageMaster.master"
            End If
        End If
    End Sub
    Private Sub FillDDL()
        objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlFromCurrency, "currcode", "currcode", "select distinct currcode from currmast", True)
        objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlToCurrency, "currcode", "currcode", "select distinct currcode from currmast", True)
    End Sub
#Region "Private Sub FillGrid(ByVal strorderby As String, Optional ByVal strsortorder As String = 'ASC')"
    Private Sub FillGrid(ByVal strorderby As String, Optional ByVal strsortorder As String = "ASC")
        Dim myDS As New DataSet

        gv_SearchResult.Visible = True
        gv_SearchResult.Columns(3).Visible = True
        ' lblMsg.Visible = False

        If gv_SearchResult.PageIndex < 0 Then
            gv_SearchResult.PageIndex = 0
        End If

        Try


            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))

            myCommand = New SqlCommand("sp_get_currrates", SqlConn)
            myCommand.CommandType = CommandType.StoredProcedure
            ' myCommand.ExecuteNonQuery()
            If ddlFromCurrency.Value <> "[Select]" Then
                myCommand.Parameters.Add(New SqlParameter("@frmcurrcode", SqlDbType.VarChar, 20)).Value = CType(ddlFromCurrency.Value, String)
            Else
                myCommand.Parameters.Add(New SqlParameter("@frmcurrcode", SqlDbType.VarChar, 20)).Value = ""
            End If
            If ddlToCurrency.Value <> "[Select]" Then
                myCommand.Parameters.Add(New SqlParameter("@tocurrcode", SqlDbType.VarChar, 20)).Value = CType(ddlToCurrency.Value, String)
            Else
                myCommand.Parameters.Add(New SqlParameter("@tocurrcode", SqlDbType.VarChar, 20)).Value = ""
            End If

            mySqlReader = myCommand.ExecuteReader()

            'myDataAdapter.Fill(myDS)
            gv_SearchResult.DataSource = mySqlReader
            If mySqlReader.HasRows Then
                gv_SearchResult.DataBind()
            Else

                gv_SearchResult.PageIndex = 0
                gv_SearchResult.DataBind()

            End If
            clsDBConnect.dbCommandClose(myCommand)               'sql command disposed
            clsDBConnect.dbReaderClose(mySqlReader)             ' sql reader disposed    
            clsDBConnect.dbConnectionClose(SqlConn)           'connection close   

        Catch ex As Exception
            objUtils.WritErrorLog("CurrencyConversionRates.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try



        '''' TXTCONVERSION


        Try
            Dim count As Long
            Dim GVRow As GridViewRow
            Dim txt As HtmlInputText

            count = gv_SearchResult.Rows.Count

            If count > 0 Then
                'fillgrd(gv_SearchResult, False, count)
                'gv_SearchResult.Columns(2).Visible = True

                For Each GVRow In gv_SearchResult.Rows
                    txt = GVRow.FindControl("txtConversion")
                    txt.Value = GVRow.Cells(3).Text
                Next

                gv_SearchResult.Columns(3).Visible = False
            End If
            clsDBConnect.dbCommandClose(myCommand)               'sql command disposed
            clsDBConnect.dbReaderClose(mySqlReader)             ' sql reader disposed    
            clsDBConnect.dbConnectionClose(SqlConn)           'connection close           

        Catch ex As Exception
            objUtils.WritErrorLog("CurrencyConversionRates.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
        Page.Title = "Currency Rate"
    End Sub
#End Region


#Region " Protected Sub btnExport_Click(ByVal sender As Object, ByVal e As System.EventArgs)"
    Protected Sub btnExport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExport.Click

        'Dim DS As New DataSet
        'Dim DA As SqlDataAdapter
        'Dim con As SqlConnection
        'Dim objcon As New clsDBConnect

        'Try
        '    If gv_SearchResult.Rows.Count <> 0 Then
        '        strSqlQry = "SELECT  currcode AS [From Currency] , tocurr AS [To Currency], convrate as [Conversion Rate],(Convert(Varchar, Datepart(DD,adddate))+ '/'+ Convert(Varchar, Datepart(MM,adddate))+ '/'+ Convert(Varchar, Datepart(YY,adddate)) + ' ' + Convert(Varchar, Datepart(hh,adddate))+ ':' + Convert(Varchar, Datepart(m,adddate))+ ':'+ Convert(Varchar, Datepart(ss,adddate))) as [Date Created],adduser as [User Created],(Convert(Varchar, Datepart(DD,moddate))+ '/'+ Convert(Varchar, Datepart(MM,moddate))+ '/'+ Convert(Varchar, Datepart(YY,moddate)) + ' ' + Convert(Varchar, Datepart(hh,moddate))+ ':' + Convert(Varchar, Datepart(m,moddate))+ ':'+ Convert(Varchar, Datepart(ss,moddate))) as [Date Modified],moduser as [User Modified]  FROM currrates"

        '        con = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
        '        DA = New SqlDataAdapter(strSqlQry, con)
        '        DA.Fill(DS, "currrates")
        '        objUtils.ExportToExcel(DS, Response)
        '        con.Close()
        '    Else
        '        objUtils.MessageBox("Sorry , No data availabe for export to excel.", Page)
        '    End If
        'Catch ex As Exception
        'End Try

        Dim myDS As New DataSet

        gv_SearchResult.Visible = True
        ' lblMsg.Visible = False

        If gv_SearchResult.PageIndex < 0 Then
            gv_SearchResult.PageIndex = 0
        End If


        Try
            'Dim SqlDA As SqlDataAdapter
            'SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
            'SqlDA = New SqlDataAdapter("exec sp_get_currrates", SqlConn)
            'SqlDA.Fill(myDS, "get_currrates")
            ''SqlDA = New SqlDataAdapter("Select * from currrates", SqlConn)
            ''SqlDA.Fill(myDS, "curr")
            'objUtils.ExportToExcel(myDS, Response)

            Dim SqlDA As SqlDataAdapter
            strSqlQry = ""
            strSqlQry = "exec sp_get_currrates"

            If ddlFromCurrency.Value <> "[Select]" Then
                strSqlQry = strSqlQry & " " & CType(ddlFromCurrency.Value, String)
            Else
                strSqlQry = strSqlQry & " " & "''"
            End If
            If ddlToCurrency.Value <> "[Select]" Then
                strSqlQry = strSqlQry & " ," & CType(ddlToCurrency.Value, String)
            Else
                strSqlQry = strSqlQry & " ," & "''"
            End If

            strSqlQry = strSqlQry & " ," & "'E'"

            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
            SqlDA = New SqlDataAdapter(strSqlQry, SqlConn)
            '@typ


            SqlDA.Fill(myDS, "get_currrates")
            objUtils.ExportToExcel(myDS, Response)

            clsDBConnect.dbConnectionClose(SqlConn)           'connection close

        Catch ex As Exception
            'objUtils.WritErrorLog("CurrencyConversionRates.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try


    End Sub
#End Region

    Private Sub SavrRecord()
        Dim lblcurrcode As Label
        Try

            'If ValidateNum() = False Then
            '    Exit Sub
            'End If
            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            sqlTrans = SqlConn.BeginTransaction           'SQL  Trans start

            myCommand = New SqlCommand("sp_Del_currates", SqlConn, sqlTrans)
            myCommand.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
            myCommand.CommandType = CommandType.StoredProcedure
            If ddlFromCurrency.Value <> "[Select]" Then
                myCommand.Parameters.Add(New SqlParameter("@frmcurrcode", SqlDbType.VarChar, 20)).Value = CType(ddlFromCurrency.Value, String)
            Else
                myCommand.Parameters.Add(New SqlParameter("@frmcurrcode", SqlDbType.VarChar, 20)).Value = ""
            End If
            If ddlToCurrency.Value <> "[Select]" Then
                myCommand.Parameters.Add(New SqlParameter("@tocurrcode", SqlDbType.VarChar, 20)).Value = CType(ddlToCurrency.Value, String)
            Else
                myCommand.Parameters.Add(New SqlParameter("@tocurrcode", SqlDbType.VarChar, 20)).Value = ""
            End If
            myCommand.ExecuteNonQuery()


            For Each GvRow In gv_SearchResult.Rows
                txtconvert = GvRow.FindControl("txtConversion")
                lblcurrcode = GvRow.FindControl("lblFromCurrency")
                If CType(txtconvert.Value, String) <> "" Then
                    myCommand = New SqlCommand("sp_add_currrates", SqlConn, sqlTrans)
                    myCommand.CommandType = CommandType.StoredProcedure

                    myCommand.Parameters.Add(New SqlParameter("@currcode", SqlDbType.VarChar, 20)).Value = CType(lblcurrcode.Text.Trim, String)
                    myCommand.Parameters.Add(New SqlParameter("@tocurr", SqlDbType.VarChar, 20)).Value = CType(GvRow.Cells(2).Text.Trim, String)
                    myCommand.Parameters.Add(New SqlParameter("@convrate", SqlDbType.Decimal, 18, 12)).Value = CType(txtconvert.Value.Trim, String)
                    myCommand.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                    myCommand.ExecuteNonQuery()

                    'connection close

                End If
            Next
            sqlTrans.Commit()                'SQl Tarn Commit
            clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
            clsDBConnect.dbCommandClose(myCommand)               'sql command disposed
            clsDBConnect.dbConnectionClose(SqlConn)

        Catch ex As Exception
            If SqlConn.State = ConnectionState.Open Then
                sqlTrans.Rollback()
            End If

            objUtils.WritErrorLog("CurrencyConversionRates.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))

        End Try
    End Sub
    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        '  Dim GvRow As GridViewRow
        Try
            If ValidateNum() = False Then
                Exit Sub
            End If
            SavrRecord()
            If Request.QueryString("FromCurr") <> "" Then
                Dim strscript As String = ""
                strscript = "window.opener.__doPostBack('CurrWindowPostBack', '');window.opener.focus();window.close();"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)
            Else
                Response.Redirect("~/MainPage.aspx", False)
            End If
            'objUtils.MessageBox("Record Saved Successfully", Page)


            'SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            'sqlTrans = SqlConn.BeginTransaction           'SQL  Trans start

            'myCommand = New SqlCommand("sp_Del_currates", SqlConn, sqlTrans)
            'myCommand.CommandType = CommandType.StoredProcedure
            'myCommand.ExecuteNonQuery()


            'For Each GvRow In gv_SearchResult.Rows
            '    txtconvert = GvRow.FindControl("txtConversion")

            '    If CType(txtconvert.Value, String) <> "" Then
            '        myCommand = New SqlCommand("sp_add_currrates", SqlConn, sqlTrans)
            '        myCommand.CommandType = CommandType.StoredProcedure

            '        myCommand.Parameters.Add(New SqlParameter("@currcode", SqlDbType.VarChar, 20)).Value = CType(GvRow.Cells(0).Text.Trim, String)
            '        myCommand.Parameters.Add(New SqlParameter("@tocurr", SqlDbType.VarChar, 20)).Value = CType(GvRow.Cells(1).Text.Trim, String)
            '        myCommand.Parameters.Add(New SqlParameter("@convrate", SqlDbType.Decimal, 18, 12)).Value = CType(txtconvert.Value.Trim, String)
            '        myCommand.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
            '        myCommand.ExecuteNonQuery()

            '        'connection close

            '    End If
            'Next
            'sqlTrans.Commit()                'SQl Tarn Commit
            'clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
            'clsDBConnect.dbCommandClose(myCommand)               'sql command disposed
            'clsDBConnect.dbConnectionClose(SqlConn)
        Catch ex As Exception
            'If SqlConn.State = ConnectionState.Open Then
            '    sqlTrans.Rollback()
            'End If
            objUtils.WritErrorLog("CurrencyConversionRates.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try

    End Sub

    Protected Function ValidateNum() As Boolean
        Dim convalue As String
        For Each GvRow In gv_SearchResult.Rows
            Dim valArr() As String
            txtconvert = GvRow.FindControl("txtConversion")
            convalue = txtconvert.Value
            valArr = convalue.Split(".")
            If valArr.Length > 2 Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please enter a valid Conversion value.');", True)
                'objUtils.MessageBox("Please enter a valid Conversion value.", Page)
                SetFocus("txtconvert")
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + txtconvert.ClientID + "');", True)
                ValidateNum = False
                Exit Function
            End If
        Next
        ValidateNum = True
    End Function

#Region "HTMLNumbers"
    Public Sub HTMLNumbers(ByVal txtbox As HtmlInputText)
        Try
            txtbox.Attributes.Add("onkeypress", "return checkNumber(event)")
        Catch ex As Exception

        End Try
    End Sub
#End Region

    '#Region "Protected Sub BtnUserSave_Click(ByVal sender As Object, ByVal e As System.EventArgs)"
    '    Protected Sub BtnUserSave_Click(ByVal sender As Object, ByVal e As System.EventArgs)
    '        Dim txtName As HtmlInputText
    '        Dim txtEmail As HtmlInputText
    '        Dim txtContact As HtmlInputText
    '        Dim GvRow As GridViewRow
    '        Try
    '            If Page.IsValid = True Then
    '                If Session("State") = "New" Then
    '                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Save First Main Details.');", True)
    '                    Exit Sub
    '                ElseIf Session("State") = "Edit" Then

    '                    If ValidateEmail() = False Then
    '                        Exit Sub
    '                    End If
    '                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
    '                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start
    '                    mySqlCmd = New SqlCommand("delete from agentmast_mulltiemail where agentcode='" & txtCustomerCode.Value & "'", mySqlConn, sqlTrans)
    '                    mySqlCmd.CommandType = CommandType.Text
    '                    mySqlCmd.ExecuteNonQuery()

    '                    For Each GvRow In gv_Email.Rows
    '                        txtName = GvRow.FindControl("txtPerson")
    '                        txtEmail = GvRow.FindControl("txtEmail")
    '                        txtContact = GvRow.FindControl("txtContactNo")
    '                        If CType(txtName.Value, String) <> "" And CType(txtEmail.Value, String) <> "" And CType(txtContact.Value, String) <> "" Then
    '                            mySqlCmd = New SqlCommand("sp_add_agentmast_mulltiemail", mySqlConn, sqlTrans)
    '                            mySqlCmd.CommandType = CommandType.StoredProcedure
    '                            mySqlCmd.Parameters.Add(New SqlParameter("@agentcode", SqlDbType.VarChar, 20)).Value = CType(txtCustomerCode.Value.Trim, String)
    '                            mySqlCmd.Parameters.Add(New SqlParameter("@contactperson", SqlDbType.VarChar, 100)).Value = CType(txtName.Value.Trim, String)
    '                            mySqlCmd.Parameters.Add(New SqlParameter("@email", SqlDbType.VarChar, 100)).Value = CType(txtEmail.Value.Trim, String)
    '                            mySqlCmd.Parameters.Add(New SqlParameter("@contactno", SqlDbType.VarChar, 50)).Value = CType(txtContact.Value.Trim, String)
    '                            '  mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
    '                            mySqlCmd.ExecuteNonQuery()
    '                        End If
    '                    Next
    '                ElseIf Session("State") = "Delete" Then
    '                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
    '                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start

    '                    mySqlCmd = New SqlCommand("sp_del_supagents", mySqlConn, sqlTrans)
    '                    mySqlCmd.CommandType = CommandType.StoredProcedure
    '                    mySqlCmd.Parameters.Add(New SqlParameter("@agentcode", SqlDbType.VarChar, 20)).Value = CType(txtCustomerCode.Value.Trim, String)
    '                    mySqlCmd.ExecuteNonQuery()

    '                    mySqlCmd = New SqlCommand("delete from agentmast_mulltiemail where agentcode='" & txtCustomerCode.Value & "'", mySqlConn, sqlTrans)
    '                    mySqlCmd.CommandType = CommandType.Text
    '                    mySqlCmd.ExecuteNonQuery()
    '                End If
    '                sqlTrans.Commit()    'SQl Tarn Commit
    '                clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
    '                clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
    '                clsDBConnect.dbConnectionClose(mySqlConn)           'connection close

    '                If Session("State") = "New" Then
    '                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Record Saved Successfully.');", True)
    '                ElseIf Session("State") = "Edit" Then
    '                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Record Updated Successfully.');", True)
    '                End If
    '                If Session("State") = "Delete" Then
    '                    Response.Redirect("CustomersSearch.aspx", False)
    '                End If
    '            End If
    '        Catch ex As Exception
    '            If mySqlConn.State = ConnectionState.Open Then
    '                sqlTrans.Rollback()
    '            End If
    '            objUtils.WritErrorLog("Customers.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
    '        End Try
    '    End Sub
    '#End Region

    Protected Sub btnExit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Response.Redirect("~/MainPage.aspx")
    End Sub


    Protected Sub btnRep_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRep.Click
        Try
            Dim strReportTitle As String = ""
            Dim strSelectionFormula As String = ""

            'Session.Add("Pageame", "Currency Conversion Rates")
            'Session.Add("BackPageName", "CurrencyConversionRates.aspx")

            'If ddlFromCurrency.Value <> "[Select]" Then
            '    If strSelectionFormula <> "" Then
            '        strReportTitle = strReportTitle & " ; From Currency : " & ddlFromCurrency.Value.Trim
            '        strSelectionFormula = strSelectionFormula & " and {currrates.currcode} = '" & ddlFromCurrency.Value.Trim & "'"
            '    Else
            '        strReportTitle = "From Currency : " & ddlFromCurrency.Value.Trim
            '        strSelectionFormula = "{currrates.currcode} = '" & ddlFromCurrency.Value.Trim & "'"
            '    End If

            'End If
            'If ddlToCurrency.Value <> "[Select]" Then
            '    If strSelectionFormula <> "" Then
            '        strReportTitle = strReportTitle & " ; To Currency : " & ddlToCurrency.Value.Trim
            '        strSelectionFormula = strSelectionFormula & " and {currrates.tocurr} = '" & ddlToCurrency.Value.Trim & "'"
            '    Else
            '        strReportTitle = "To Currency : " & ddlToCurrency.Value.Trim
            '        strSelectionFormula = "{currrates.tocurr} = '" & ddlToCurrency.Value.Trim & "'"
            '    End If

            'End If
            'Session.Add("SelectionFormula", strSelectionFormula)
            'Session.Add("ReportTitle", strReportTitle)
            'Response.Redirect("rptReportNew.aspx", False)
            Dim strpop As String = ""
            strpop = "window.open('rptReportNew.aspx?Pageame=Currency Conversion Rates&BackPageName=CurrencyConversionRates.aspx&FromCurr=" & ddlFromCurrency.Value.Trim & "&ToCurr=" & ddlToCurrency.Value.Trim & "','RepCurrConv','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
        Catch ex As Exception
            objUtils.WritErrorLog("CurrencyConversionRates.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try

    End Sub

    Protected Sub btnClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Response.Redirect("~/MainPage.aspx")
    End Sub

    Protected Sub btnUpdate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUpdate.Click
        Try
            If ValidateNum() = False Then
                Exit Sub
            End If
            SavrRecord()
            'objUtils.MessageBox("Record Saved Successfully", Page)
            Response.Redirect("~/MainPage.aspx", False)
        Catch ex As Exception

            objUtils.WritErrorLog("CurrencyConversionRates.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub

    Protected Sub btnExcel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExcel.Click
        Dim myDS As New DataSet

        gv_SearchResult.Visible = True
        ' lblMsg.Visible = False

        If gv_SearchResult.PageIndex < 0 Then
            gv_SearchResult.PageIndex = 0
        End If


        Try
            'If gv_SearchResult.Rows.Count > 0 Then
            '    Response.ContentType = "application/vnd.ms-excel"
            '    Response.Charset = ""
            '    Me.EnableViewState = False

            '    Dim tw As New System.IO.StringWriter()
            '    Dim hw As New System.Web.UI.HtmlTextWriter(tw)
            '    Dim frm As HtmlForm = New HtmlForm()
            '    Me.Controls.Add(frm)
            '    frm.Controls.Add(gv_SearchResult)
            '    frm.RenderControl(hw)
            '    Response.Write(tw.ToString())
            '    Response.End()
            '    Response.Clear()
            'Else
            '    objUtils.MessageBox("Sorry , No data availabe for export to excel.", Page)
            'End If
            Dim SqlDA As SqlDataAdapter
            strSqlQry = ""
            strSqlQry = "exec sp_get_currrates"

            If ddlFromCurrency.Value <> "[Select]" Then
                strSqlQry = strSqlQry & " " & CType(ddlFromCurrency.Value, String)
            Else
                strSqlQry = strSqlQry & " " & "''"
            End If
            If ddlToCurrency.Value <> "[Select]" Then
                strSqlQry = strSqlQry & " ," & CType(ddlToCurrency.Value, String)
            Else
                strSqlQry = strSqlQry & " ," & "''"
            End If

            strSqlQry = strSqlQry & " ," & "'E'"

            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
            SqlDA = New SqlDataAdapter(strSqlQry, SqlConn)
            '@typ


            SqlDA.Fill(myDS, "get_currrates")
            objUtils.ExportToExcel(myDS, Response)
            clsDBConnect.dbConnectionClose(SqlConn)           'connection close  

            'SqlConn.Close()
        Catch ex As Exception
            'objUtils.WritErrorLog("CurrencyConversionRates.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try



    End Sub

    Protected Sub btnhelp_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnhelp.Click
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=CurrencyConversionRates','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub

    Protected Sub btnFill_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnFill.Click
        FillGrid("currcode")
    End Sub
End Class
