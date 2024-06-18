'------------================--------------=======================------------------================
'   Module Name    :    Cities.aspx
'   Developer Name :    Nilesh Sawant
'   Date           :    11 June 2008
'   
'
'------------================--------------=======================------------------================
#Region "namespace"
Imports System.Data
Imports System.Data.SqlClient
Imports System.Collections.Generic
#End Region

Partial Class Sector
    Inherits System.Web.UI.Page

#Region "Global Declaration"
    Dim objUtils As New clsUtils
    Dim strSqlQry As String
    Dim mySqlCmd As SqlCommand
    Dim mySqlReader As SqlDataReader
    Dim mySqlConn As SqlConnection
    Dim sqlTrans As SqlTransaction
#End Region

#Region "Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Page.IsPostBack = False Then
            Try
                'If Not Session("CompanyName") Is Nothing Then
                '    Me.Page.Title = CType(Session("CompanyName"), String)
                'End If

                If CType(Session("GlobalUserName"), String) = "" Then
                    Response.Redirect("~/Login.aspx", False)
                    Exit Sub
                End If

                txtconnection.Value = Session("dbconnectionName")

                'vij 
                Session.Add("trfgroupcode", objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id='1001'"))

                ViewState.Add("SectorsState", Request.QueryString("State"))
                ViewState.Add("SectorsRefCode", Request.QueryString("RefCode"))
          
                If ViewState("SectorsState") = "New" Then
                    SetFocus(txtSectorCode)
                    lblHeading.Text = "Add New Sector Group"
                    Page.Title = Page.Title + " " + "New Sector Group Master"
                    btnSave.Text = "Save"

                    'btnSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to save sector Group?')==false)return false;")
                    btnSave.Attributes.Add("onclick", "return FormValidation('New')")
                ElseIf ViewState("SectorsState") = "Edit" Then
                    SetFocus(txtSectorName)
                    lblHeading.Text = "Edit Sector Group"
                    Page.Title = Page.Title + " " + "Edit Sector Group Master"
                    btnSave.Text = "Update"
                    DisableControl()

                    ShowRecord(CType(ViewState("SectorsRefCode"), String))
                    ' btnSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to update sector Group?')==false)return false;")
                    btnSave.Attributes.Add("onclick", "return FormValidation('Edit')")
                ElseIf ViewState("SectorsState") = "View" Then
                    SetFocus(btnCancel)
                    lblHeading.Text = "View Sector Group"
                    Page.Title = Page.Title + " " + "View Sector Group Master"
                    btnSave.Visible = False
                    btnCancel.Text = "Return to Search"
                    DisableControl()

                    ShowRecord(CType(ViewState("SectorsRefCode"), String))

                ElseIf ViewState("SectorsState") = "Delete" Then
                    SetFocus(btnSave)
                    lblHeading.Text = "Delete Sector Group"
                    Page.Title = Page.Title + " " + "Delete Sector Group Master"
                    btnSave.Text = "Delete"
                    DisableControl()

                    ShowRecord(CType(ViewState("SectorsRefCode"), String))
                    btnSave.Attributes.Add("onclick", "return FormValidation('Delete')")
                    btnSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to delete Sector Group?')==false)return false;")
                End If
                btnCancel.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to cancel?')==false)return false;")



                Dim typ As Type
                typ = GetType(DropDownList)

                If (Page.ClientScript.IsClientScriptIncludeRegistered(typ, "TADDScript.js")) = False Then
                    Page.ClientScript.RegisterClientScriptInclude(typ, "TADDScript.js", "TADDScript.js")
                    'ddlCountryCode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    'ddlCountryName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    'ddlCityCode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    'ddlCityName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                End If


                '                btnSave.Attributes.Add("onclick", "return FormValidation()")

                '   ValidateOnlyNumber()

            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objUtils.WritErrorLog("SectorGroupSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            End Try

        Else
            Try
                '
            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objUtils.WritErrorLog("SectorGroupSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            End Try
        End If

    End Sub
#End Region


#Region "Private Sub DisableControl()"

    Private Sub DisableControl()
        If ViewState("SectorsState") = "View" Or ViewState("SectorsState") = "Delete" Then
            txtSectorCode.Disabled = True
            txtSectorName.Disabled = True
            txtcityname.Enabled = False
            txtcountryname.Enabled = False
            TxtExpenseName.Enabled = False
            txtincomename.Enabled = False

            chkActive.Disabled = True
      
        ElseIf Session("State") = "Edit" Then
            txtSectorCode.Disabled = True
            txtcityname.Enabled = True
            txtcountryname.Enabled = True
            TxtExpenseName.Enabled = True
            txtincomename.Enabled = True
            chkActive.Disabled = False
        End If

    End Sub

#End Region

#Region " Validate Page"
    Public Function ValidatePage() As Boolean
        Try
            If txtSectorCode.Value = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Code can not be blank.');", True)
                SetFocus(txtSectorCode)
                ValidatePage = False
                Exit Function
            End If
            If txtSectorName.Value.Trim = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Name can not be blank.');", True)
                SetFocus(txtSectorName)
                ValidatePage = False
                Exit Function
            End If
            If txtcountrycode.Text.Trim = "" Or txtcountryname.Text.Trim = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please select country .');", True)
                SetFocus(txtcountrycode)
                ValidatePage = False
                Exit Function
            End If
            If txtcitycode.Text.Trim = "" Or txtcityname.Text.Trim = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please select city ');", True)
                SetFocus(txtcitycode)
                ValidatePage = False
                Exit Function
            End If


            If txtincomecode.Text.Trim = "" Or txtincomename.Text.Trim = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please select Income Code ');", True)
                SetFocus(txtcitycode)
                ValidatePage = False
                Exit Function
            End If
            If TxtExpenseCode.Text.Trim = "" Or TxtExpenseName.Text.Trim = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please select Expense Code ');", True)
                SetFocus(txtcitycode)
                ValidatePage = False
                Exit Function
            End If




        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)

        End Try
        ValidatePage = True
    End Function
#End Region

#Region "Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click"
    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click

        Try
            If Page.IsValid = True Then
                If ViewState("SectorsState") = "New" Or ViewState("SectorsState") = "Edit" Then
                    If ValidatePage() = False Then
                        Exit Sub
                    End If
                    If checkForDuplicate() = False Then
                        Exit Sub
                    End If

                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start

                    If ViewState("SectorsState") = "New" Then
                        mySqlCmd = New SqlCommand("sp_add_sectorgroupmaster", mySqlConn, sqlTrans)

                    ElseIf ViewState("SectorsState") = "Edit" Then
                        mySqlCmd = New SqlCommand("sp_mod_sectorgroupmaster", mySqlConn, sqlTrans)

                    End If
                    mySqlCmd.CommandType = CommandType.StoredProcedure

                    'mySqlCmd.Parameters.Add(New SqlParameter("@div_code", SqlDbType.VarChar, 10)).Value = CType(txtSectorCode.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@sectorgroupcode", SqlDbType.VarChar, 20)).Value = CType(txtSectorCode.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@sectorgroupname", SqlDbType.VarChar, 150)).Value = CType(txtSectorName.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@othgrpcode", SqlDbType.VarChar, 20)).Value = CType(Session("trfgroupcode"), String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@ctrycode", SqlDbType.VarChar, 20)).Value = CType(txtcountrycode.Text.Trim, String)

                    mySqlCmd.Parameters.Add(New SqlParameter("@citycode", SqlDbType.VarChar, 20)).Value = CType(txtcitycode.Text.Trim, String)
                    If chkActive.Checked = True Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@active", SqlDbType.Int)).Value = 1
                    ElseIf chkActive.Checked = False Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@active", SqlDbType.Int)).Value = 0
                    End If
                    mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)

            
                    If txtincomecode.Text <> "" Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@incomecode", SqlDbType.VarChar, 20)).Value = CType(txtincomecode.Text.Trim, String)
                    Else
                        mySqlCmd.Parameters.Add(New SqlParameter("@incomecode", SqlDbType.VarChar, 20)).Value = DBNull.Value
                    End If

                    If TxtExpenseCode.Text <> "" Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@expensecode", SqlDbType.VarChar, 20)).Value = CType(TxtExpenseCode.Text.Trim, String)

                    Else
                        mySqlCmd.Parameters.Add(New SqlParameter("@expensecode", SqlDbType.VarChar, 20)).Value = DBNull.Value
                    End If
                 
                    mySqlCmd.ExecuteNonQuery()

                ElseIf ViewState("SectorsState") = "Delete" Then
                    If checkForDeletion() = False Then
                        Exit Sub
                    End If
                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start

                    mySqlCmd = New SqlCommand("sp_del_sectorgroupmaster", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@sectorgroupcode", SqlDbType.VarChar, 20)).Value = CType(txtSectorCode.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@othgrpcode", SqlDbType.VarChar, 20)).Value = CType(Session("trfgroupcode"), String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)

                    mySqlCmd.ExecuteNonQuery()
                End If



                    sqlTrans.Commit()    'SQl Tarn Commit
                    clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
                    clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
                    clsDBConnect.dbConnectionClose(mySqlConn)           'connection close
                    'Response.Redirect("SectorSearch.aspx", False)
                    Dim strscript As String = ""
                    strscript = "window.opener.__doPostBack('SectSupWindowPostBack', '');window.opener.focus();window.close();"
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)


                End If
        Catch ex As Exception
            If mySqlConn.State = ConnectionState.Open Then
                sqlTrans.Rollback()


            End If
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("SectorGroupSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try

    End Sub

#End Region

#Region " Private Sub ShowRecord(ByVal RefCode As String)"

    Private Sub ShowRecord(ByVal RefCode As String)
        Try
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            mySqlCmd = New SqlCommand("Select * from othtypmast Where othtypcode='" & RefCode & "'", mySqlConn)
            mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
            If mySqlReader.HasRows Then
                If mySqlReader.Read() = True Then
                    If IsDBNull(mySqlReader("othtypcode")) = False Then
                        Me.txtSectorCode.Value = CType(mySqlReader("othtypcode"), String)
                    Else
                        Me.txtSectorCode.Value = ""
                    End If
                    If IsDBNull(mySqlReader("othtypname")) = False Then
                        Me.txtSectorName.Value = CType(mySqlReader("othtypname"), String)
                    Else
                        Me.txtSectorName.Value = ""
                    End If


                    If IsDBNull(mySqlReader("ctrycode")) = False Then
                        txtcountrycode.Text = mySqlReader("ctrycode")
                        txtcountryname.Text = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "ctrymast", "ctryname", "ctrycode", mySqlReader("ctrycode"))
                    End If


                    If IsDBNull(mySqlReader("citycode")) = False Then
                        txtcitycode.Text = mySqlReader("citycode")
                        txtcityname.Text = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "citymast", "cityname", "citycode", mySqlReader("citycode"))
                    End If

                    If IsDBNull(mySqlReader("incomecode")) = False Then
                        txtincomecode.Text = mySqlReader("incomecode")
                        txtincomename.Text = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "acctmast", "acctname", "acctcode", mySqlReader("incomecode"))
                    End If


                    If IsDBNull(mySqlReader("expensecode")) = False Then
                        TxtExpenseCode.Text = mySqlReader("expensecode")
                        TxtExpenseName.Text = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "acctmast", "acctname", "acctcode", mySqlReader("expensecode"))
                    End If
               



                    If IsDBNull(mySqlReader("active")) = False Then
                        If CType(mySqlReader("active"), String) = "1" Then
                            chkActive.Checked = True
                        ElseIf CType(mySqlReader("active"), String) = "0" Then
                            chkActive.Checked = False
                        End If
                    End If
                End If
            End If

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("SectorGroupSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
            clsDBConnect.dbReaderClose(mySqlReader)             ' sql reader disposed    
            clsDBConnect.dbConnectionClose(mySqlConn)           'connection close           
        End Try
    End Sub
#End Region
    <System.Web.Script.Services.ScriptMethod()> _
  <System.Web.Services.WebMethod()> _
    Public Shared Function Getcitylist(ByVal prefixText As String) As List(Of String)
        Dim strSqlQry As String = ""
        Dim ctry As String = "" 'contextKey
        Dim myDS As New DataSet
        Dim Citynames As New List(Of String)
        Try

            If HttpContext.Current.Session("sectgroup_ctrycode_for_filter") IsNot Nothing Then
                ctry = HttpContext.Current.Session("sectgroup_ctrycode_for_filter")
            End If

            strSqlQry = "select cityname,citycode from citymast where active=1"
            If Trim(ctry) <> "" Then
                strSqlQry = strSqlQry + " and ctrycode='" & Trim(ctry) & "'"
            End If
            strSqlQry = strSqlQry + " and cityname like  '" & Trim(prefixText) & "%'"

            Dim SqlConn As New SqlConnection
            Dim myDataAdapter As New SqlDataAdapter
            ' SqlConn = clsDBConnect.dbConnectionnew(objclsConnectionName.ConnectionName)
            SqlConn = clsDBConnect.dbConnectionnew("strDBConnection")
            'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)
            If myDS.Tables(0).Rows.Count > 0 Then
                For i As Integer = 0 To myDS.Tables(0).Rows.Count - 1
                    Citynames.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(myDS.Tables(0).Rows(i)("cityname").ToString(), myDS.Tables(0).Rows(i)("citycode").ToString()))
                    'Hotelnames.Add(myDS.Tables(0).Rows(i)("partyname").ToString() & "<span style='display:none'>" & i & "</span>")
                Next
            End If

            Return Citynames
        Catch ex As Exception
            Return Citynames
        End Try
    End Function
    <System.Web.Script.Services.ScriptMethod()> _
  <System.Web.Services.WebMethod()> _
    Public Shared Function GetAccountlist(ByVal prefixText As String) As List(Of String)
        Dim strSqlQry As String = ""
        Dim ctry As String = "" 'contextKey
        Dim myDS As New DataSet
        Dim Acctnames As New List(Of String)
        Try



            strSqlQry = "select acctcode,acctname from acctmast "
            If Trim(ctry) <> "" Then
                strSqlQry = strSqlQry + " where acctcode='" & Trim(ctry) & "'"
            End If
            strSqlQry = strSqlQry + "  where acctname like  '" & Trim(prefixText) & "%'"

            Dim SqlConn As New SqlConnection
            Dim myDataAdapter As New SqlDataAdapter
            ' SqlConn = clsDBConnect.dbConnectionnew(objclsConnectionName.ConnectionName)
            SqlConn = clsDBConnect.dbConnectionnew("strDBConnection")
            'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)
            If myDS.Tables(0).Rows.Count > 0 Then
                For i As Integer = 0 To myDS.Tables(0).Rows.Count - 1
                    Acctnames.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(myDS.Tables(0).Rows(i)("acctname").ToString(), myDS.Tables(0).Rows(i)("acctcode").ToString()))
                    'Hotelnames.Add(myDS.Tables(0).Rows(i)("partyname").ToString() & "<span style='display:none'>" & i & "</span>")
                Next
            End If

            Return Acctnames
        Catch ex As Exception
            Return Acctnames
        End Try
    End Function
    <System.Web.Script.Services.ScriptMethod()> _
<System.Web.Services.WebMethod()> _
    Public Shared Function GetAccountexplist(ByVal prefixText As String) As List(Of String)
        Dim strSqlQry As String = ""
        Dim ctry As String = "" 'contextKey
        Dim myDS As New DataSet
        Dim Acctexpnames As New List(Of String)
        Try



            strSqlQry = "select acctcode,acctname from acctmast "
            If Trim(ctry) <> "" Then
                strSqlQry = strSqlQry + " where acctcode='" & Trim(ctry) & "'"
            End If
            strSqlQry = strSqlQry + "  where acctname like  '" & Trim(prefixText) & "%'"

            Dim SqlConn As New SqlConnection
            Dim myDataAdapter As New SqlDataAdapter
            ' SqlConn = clsDBConnect.dbConnectionnew(objclsConnectionName.ConnectionName)
            SqlConn = clsDBConnect.dbConnectionnew("strDBConnection")
            'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)
            If myDS.Tables(0).Rows.Count > 0 Then
                For i As Integer = 0 To myDS.Tables(0).Rows.Count - 1
                    Acctexpnames.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(myDS.Tables(0).Rows(i)("acctname").ToString(), myDS.Tables(0).Rows(i)("acctcode").ToString()))
                    'Hotelnames.Add(myDS.Tables(0).Rows(i)("partyname").ToString() & "<span style='display:none'>" & i & "</span>")
                Next
            End If

            Return Acctexpnames
        Catch ex As Exception
            Return Acctexpnames
        End Try
    End Function
   
    Protected Sub txtcityname_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtcityname.TextChanged
        Session("sectgroup_citycode_for_filter") = txtcitycode.Text
    End Sub
#Region "Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click"
    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        'Response.Redirect("SectorSearch.aspx", False)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", "window.close();", True)
    End Sub
#End Region
    <System.Web.Script.Services.ScriptMethod()> _
     <System.Web.Services.WebMethod()> _
    Public Shared Function Getcountrylist(ByVal prefixText As String) As List(Of String)
        Dim strSqlQry As String = ""
        Dim myDS As New DataSet
        Dim Countrynames As New List(Of String)
        Try
            strSqlQry = "select ctryname,ctrycode from ctrymast  where  ctryname like  '" & Trim(prefixText) & "%'"
            Dim SqlConn As New SqlConnection
            Dim myDataAdapter As New SqlDataAdapter
            ' SqlConn = clsDBConnect.dbConnectionnew(objclsConnectionName.ConnectionName)
            SqlConn = clsDBConnect.dbConnectionnew("strDBConnection")
            'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)
            If myDS.Tables(0).Rows.Count > 0 Then
                For i As Integer = 0 To myDS.Tables(0).Rows.Count - 1
                    Countrynames.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(myDS.Tables(0).Rows(i)("ctryname").ToString(), myDS.Tables(0).Rows(i)("ctrycode").ToString()))
                    'Hotelnames.Add(myDS.Tables(0).Rows(i)("partyname").ToString() & "<span style='display:none'>" & i & "</span>")
                Next
            End If

            Return Countrynames
        Catch ex As Exception
            Return Countrynames
        End Try
    End Function
    Protected Sub txtcountryname_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtcountryname.TextChanged
        Session("sectgroup_ctrycode_for_filter") = txtcountrycode.Text
    End Sub

#Region "Public Function checkForDuplicate() As Boolean"
    Public Function checkForDuplicate() As Boolean
        If ViewState("SectorsState") = "New" Then
            If objUtils.isDuplicatenew(Session("dbconnectionName"), "othtypmast", "othtypcode", CType(txtSectorCode.Value.Trim, String)) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This sector Group code is already present.');", True)
                checkForDuplicate = False
                Exit Function
            End If
            If objUtils.isDuplicatenew(Session("dbconnectionName"), "othtypmast", "othtypname", txtSectorName.Value.Trim) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This sector Group name is already present.');", True)
                checkForDuplicate = False
                Exit Function
            End If
        ElseIf ViewState("SectorsState") = "Edit" Then
            If objUtils.isDuplicateForModifynew(Session("dbconnectionName"), "othtypmast", "othtypcode", "othtypname", txtSectorName.Value.Trim, CType(txtSectorCode.Value.Trim, String)) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This sector name is already present.');", True)
                checkForDuplicate = False
                Exit Function
            End If
        End If
        checkForDuplicate = True
    End Function
#End Region

    '#Region "Protected Sub ddlcountry_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)"
    '    Protected Sub ddlcountry_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
    '        Try
    '            strSqlQry = ""
    '            TxtCountryName.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"),"ctrymast", "ctryname", "ctrycode", ddlCountryCode.SelectedValue)
    '        Catch ex As Exception

    '        End Try
    '    End Sub
    '#End Region

    '#Region "Protected Sub ddlCityCode_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)"
    '    Protected Sub ddlCityCode_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
    '        Try
    '            strSqlQry = ""
    '            TxtCityName.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"),"citymast", "cityname", "citycode", ddlCityCode.SelectedValue)
    '        Catch ex As Exception

    '        End Try
    '    End Sub
    '#End Region

    Public Sub New()

    End Sub

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub
#Region "Public Function checkForDeletion() As Boolean"
    Public Function checkForDeletion() As Boolean
        checkForDeletion = True
        If objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "partymast", "sectorcode", CType(txtSectorCode.Value.Trim, String)) Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This SupplierSector is already used for a Suppliers, cannot delete this Sector');", True)
            checkForDeletion = False
            Exit Function
        End If
        checkForDeletion = True
    End Function
#End Region

    
    Protected Sub btnhelp_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=Sector','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub
    Private Function trfgroupcode()


    End Function
End Class


