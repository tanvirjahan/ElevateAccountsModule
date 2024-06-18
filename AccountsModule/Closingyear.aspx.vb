#Region "Namespace"
Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports System.IO.File
Imports System.Collections
Imports System.Collections.Generic
Imports System.Globalization
#End Region

Partial Class AccountsModule_closingyear
    Inherits System.Web.UI.Page

    Dim objUtils As New clsUtils
    Dim objDate As New clsDateTime
    Dim strSqlQry As String
    Dim myDataAdapter As SqlDataAdapter
    Dim mySqlReader As SqlDataReader
    Dim mySqlConn As SqlConnection
    Dim sqlTrans As SqlTransaction
    Dim mySqlCmd As SqlCommand
    Dim strappid As String = ""
    Dim strappname As String = ""
    Shared divcode As String = ""


#Region "Global Declarations"
    Dim msealdate As Date
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try

            Dim AppId As HiddenField = CType(Master.FindControl("hdnAppId"), HiddenField)
            Dim AppName As HiddenField = CType(Master.FindControl("hdnAppName"), HiddenField)
            Dim appidnew As String = CType(Request.QueryString("appid"), String)

            If appidnew Is Nothing = False Then
                '*** Danny 04/04/2018>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>            
                strappname = Session("AppName")
                '*** Danny 04/04/2018<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
            End If
            '*** Danny 04/04/2018>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
            Dim divid As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select division_master_code from division_master where accountsmodulename='" & Session("DAppName") & "'")
            '*** Danny 04/04/2018<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
            ViewState.Add("divcode", divid)
            divcode = ViewState("divcode")

            If Page.IsPostBack = False Then
                '----------------------------- Default Dates
                Dim acccode As String
                Dim desc As String
                Dim fdate As String = ""
                Dim endmonth As String
                Dim enddate As String
                Dim yearend As String = ""



                txtDivCode.Value = ViewState("divcode")

                'objUtils.GetDBFieldFromLongnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", 511)

                Dim ds As DataSet
                ds = objUtils.ExecuteQuerySqlnew(Session("dbconnectionName"), "select fdate  from toursmaster ")


                If ds.Tables(0).Rows.Count > 0 Then
                    If IsDBNull(ds.Tables(0).Rows(0)(0)) = False Then
                        fdate = ds.Tables(0).Rows(0)("fdate")

                    Else
                        fdate = ""
                    End If
                Else
                    fdate = ""

                End If
                txtacname.Attributes.Add("readonly", "readonly")
                endmonth = objUtils.GetDBFieldFromLongnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", 521)
                enddate = objUtils.GetDBFieldFromLongnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", 529)

                'acccode = CType(objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected From reservation_parameters where param_id=522"), String)
                'ddlaccnamefrom.Value = acccode
                'ddlacccodefrom.Items(ddlacccodefrom.SelectedIndex).Text = acccode

                Dim dsval As DataSet

                dsval = objUtils.ExecuteQuerySqlnew(Session("dbconnectionName"), "select max(enddate) enddate from closeyear where division_code='" & ViewState("divcode") & "'")
                If dsval.Tables.Count > 0 Then
                    If IsDBNull(dsval.Tables(0).Rows(0)(0)) = False Then
                        yearend = CDate(enddate + "/" + endmonth + "/" + Trim(Str(Year(dsval.Tables(0).Rows(0)("enddate")) + 1)))
                    Else
                        yearend = CDate(enddate + "/" + endmonth + "/" + Trim(Str(Year(fdate))))
                    End If
                Else
                    yearend = CDate(enddate + "/" + endmonth + "/" + Trim(Str(Year(fdate))))
                End If

                If yearend = "" Then
                    txtFromDate.Text = Format(CType(objDate.GetSystemDateOnly(Session("dbconnectionName")), Date), "dd/MM/yyyy")
                Else
                    txtFromDate.Text = yearend
                End If


                Dim typ As Type
                typ = GetType(DropDownList)

                If (Page.ClientScript.IsClientScriptIncludeRegistered(typ, "TADDScript.js")) = False Then
                    Page.ClientScript.RegisterClientScriptInclude(typ, "TADDScript.js", "TADDScript.js")

                End If

                ClientScript.GetPostBackEventReference(Me, String.Empty)
                If (Me.IsPostBack And Me.Request("__EVENTTARGET") = "RepTBWindowPostBack") Then
                    btnseal_Click(sender, e)
                End If


                txtFromDate.Attributes.Add("onchange", "javascript:ChangeDate();")

                desc = "You are about to close the previous year, a journal entry for the closing will be passed automatically as on the date shown above."
                If dsval.Tables.Count > 0 Then
                    If IsDBNull(dsval.Tables(0).Rows(0)(0)) = False Then
                        desc = desc + " Last Closing date is : " + dsval.Tables(0).Rows(0)("enddate")
                    End If
                End If

                desc = desc + " Warning - All Data Upto the Date given above will be sealed and cannot be edited or deleted. Also no entries can be made prior to the date given above."
                DIV1.InnerText = desc

                ''description()
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Error Description: - " & ex.Message & "');", True)
            objUtils.WritErrorLog("sealdate.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub

    Private Sub description()
        Dim desc As String
        Try
            desc = "You are about to close the previous year, a journal entry for the closing will be passed automatically as on the date shown above."
            desc = desc + "Warning - All Data Upto the Date given above will be sealed and cannot be edited or deleted. Also no entries can be made prior to the date given above."

            DIV1.InnerText = desc

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("sealdata.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try

    End Sub
    <System.Web.Script.Services.ScriptMethod()> _
     <System.Web.Services.WebMethod()> _
    Public Shared Function Getactype(ByVal prefixText As String) As List(Of String)

        Dim strSqlQry As String = ""
        Dim myDS As New DataSet
        Dim agent As New List(Of String)
        Try


            strSqlQry = "select des acctname,code acctcode from view_account where type='G'  and des like  '" & Trim(prefixText) & "%'"
            Dim SqlConn As New SqlConnection
            Dim myDataAdapter As New SqlDataAdapter
            ' SqlConn = clsDBConnect.dbConnectionnew(objclsConnectionName.ConnectionName)
            SqlConn = clsDBConnect.dbConnectionnew("strDBConnection")
            'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)

            If myDS.Tables(0).Rows.Count > 0 Then
                For i As Integer = 0 To myDS.Tables(0).Rows.Count - 1

                    agent.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(myDS.Tables(0).Rows(i)("acctname").ToString(), myDS.Tables(0).Rows(i)("acctcode").ToString()))
                    'Hotelnames.Add(myDS.Tables(0).Rows(i)("partyname").ToString() & "<span style='display:none'>" & i & "</span>")
                Next
            End If

            Return agent
        Catch ex As Exception
            Return agent
        End Try

    End Function
    Protected Sub btnseal_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            Dim parm(7) As SqlParameter
            Dim i As Integer = 0
            Dim jvno As String = ""
            Dim mbasecurrency As String = ""

            If validation() = False Then
                Exit Sub
            End If

            jvno = "CLOSE/" + Trim(Str(Month(txtFromDate.Text))) + "/" + Trim(Str(Year(txtFromDate.Text)))
            mbasecurrency = CType(objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id=457"), String)

            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            mySqlCmd = New SqlCommand
            '  sqlTrans = mySqlConn.BeginTransaction()
            sqlTrans = mySqlConn.BeginTransaction
            mySqlCmd.Connection = mySqlConn
            mySqlCmd.Transaction = sqlTrans
            mySqlCmd.CommandType = CommandType.StoredProcedure
            mySqlCmd.CommandText = "sp_closeyear"

            parm(0) = New SqlParameter("@division", CType(txtDivCode.Value, String))
            parm(1) = New SqlParameter("@enddate", Format(CType(txtFromDate.Text, Date), "yyyy/MM/dd"))
            parm(2) = New SqlParameter("@jvno", jvno)
            parm(3) = New SqlParameter("@placctcode", CType(txtaccode.Text, String))
            parm(4) = New SqlParameter("@currcode", mbasecurrency)
            parm(5) = New SqlParameter("@adddate", objDate.GetSystemDateTime(Session("dbconnectionName")))
            parm(6) = New SqlParameter("@adduser", CType(Session("GlobalUserName"), String))
            For i = 0 To 6
                mySqlCmd.Parameters.Add(parm(i))
            Next

            'mySqlCmd.ExecuteNonQuery()
            'sqlTrans.Commit()    'SQl Tarn Commit
            'sqlTrans.Dispose()
            'mySqlConn.Close()
            'mySqlConn.Dispose()

            mySqlCmd.ExecuteNonQuery()
            sqlTrans.Commit()
            clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
            clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
            clsDBConnect.dbConnectionClose(mySqlConn)
            'Response.Redirect("AcctCodesSearch.aspx", False)
            '' description()
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Closing year completed');", True)

        Catch ex As Exception
            If mySqlConn.State = ConnectionState.Open Then
                sqlTrans.Rollback()
            End If
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("sealdata.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))

        End Try
    End Sub

#Region "Public Function validation() As Boolean"
    Public Function validation() As Boolean
        Dim parms As New List(Of SqlParameter)
        Dim parm(2) As SqlParameter

        Dim frmdate As DateTime
        'Dim todate As DateTime
        Dim MyCultureInfo As New CultureInfo("fr-Fr")
        'frmdate = DateTime.Parse(txtpdate.Text, MyCultureInfo, DateTimeStyles.None)
        frmdate = DateTime.Parse(txtFromDate.Text, MyCultureInfo, DateTimeStyles.None)

        validation = True



        If txtFromDate.Text = "" Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Closing Date cannot be empty.');", True)
            SetFocus(txtFromDate)
            validation = False
            Exit Function
        End If

        If txtaccode.Text = "" Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Transfer account cannot be blank');", True)
            validation = False
            Exit Function
        End If

        Dim ds As DataSet

        ds = objUtils.ExecuteQuerySqlnew(Session("dbconnectionName"), "select max(enddate) enddate from closeyear ")
        If ds.Tables.Count > 0 Then
            If IsDBNull(ds.Tables(0).Rows(0)(0)) = False Then
                If frmdate <= ds.Tables(0).Rows(0)("enddate") Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Closing already done upto  " + CType(ds.Tables(0).Rows(0)("enddate"), String) + " choose a later date');", True)
                    validation = False
                    Exit Function
                End If
            End If
        End If

        ' Tanvir(26072022)
        mySqlCmd = New SqlCommand()
        mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
        mySqlCmd.Connection = mySqlConn
        mySqlCmd.CommandType = CommandType.StoredProcedure

        mySqlCmd.CommandText = "sp_validate_Posted"
        parm(0) = New SqlParameter("@sealdate", Format(CType(txtFromDate.Text, Date), "yyyy/MM/dd"))
        parm(1) = New SqlParameter("@div_code", ViewState("divcode"))
        parm(2) = New SqlParameter("@errmsg", SqlDbType.VarChar, 500)
        parm(2).Direction = ParameterDirection.Output
        mySqlCmd.Parameters.Add(parm(0))
        mySqlCmd.Parameters.Add(parm(1))
        mySqlCmd.Parameters.Add(parm(2))
        mySqlCmd.CommandTimeout = 0
        mySqlCmd.ExecuteNonQuery()
        Dim strError As String = ""
        strError = parm(2).Value.ToString()
        If strError <> "" Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" + strError + "');", True)
            validation = False
            Exit Function
        End If
        mySqlConn.Close()
        mySqlConn.Dispose()
        '  Tanvir(26072022)
    End Function

#End Region
End Class
