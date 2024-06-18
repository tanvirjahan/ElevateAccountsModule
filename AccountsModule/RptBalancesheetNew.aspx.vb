'------------================--------------=======================------------------================
'   Module Name    :    RptBalanceSheetNew.aspx
'   Developer Name :    Ram kumar
'   Date           :    18 AUG 2022
'   
'
'------------================--------------=======================------------------================
#Region "Namespace"
Imports System.Data
Imports System.Data.SqlClient
Imports System.Collections.Generic
Imports System.Globalization
#End Region


Partial Class RptBalancesheetNew
    Inherits System.Web.UI.Page

#Region "Global Declarations"
    Dim objUtils As New clsUtils
    Dim objUser As New clsUser
    Dim strSqlQry As String
    Dim strWhereCond As String
    Dim SqlConn As SqlConnection
    Dim SqlCmd As SqlCommand
    Dim myDataAdapter As SqlDataAdapter
    Dim dpFDate As EclipseWebSolutions.DatePicker.DatePicker
    Dim dpTDate As EclipseWebSolutions.DatePicker.DatePicker
    Dim mySqlReader As SqlDataReader
    Dim mySqlConn As SqlConnection
    Dim ObjDate As New clsDateTime
    Dim sqlTrans As SqlTransaction
    Dim mySqlCmd As SqlCommand
    Shared divcode As String = ""
    Dim strappid As String = ""
    Dim strappname As String = ""
#End Region
    Dim max As Integer

#Region "Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            Dim AppId As HiddenField = CType(Master.FindControl("hdnAppId"), HiddenField)
            Dim AppName As HiddenField = CType(Master.FindControl("hdnAppName"), HiddenField)
            Dim appidnew As String = CType(Request.QueryString("appid"), String)
            strappid = appidnew
            If appidnew Is Nothing = False Then
                '*** Danny 04/04/2018>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>            
                strappname = Session("AppName")
                '*** Danny 04/04/2018<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
            End If
            If CType(Session("GlobalUserName"), String) = Nothing Or CType(Session("Userpwd"), String) = Nothing Then
                Response.Redirect("~/Login.aspx", False)
                Exit Sub
            Else
                objUser.CheckUserRight(Session("dbconnectionName"), CType(Session("GlobalUserName"), String), CType(Session("Userpwd"), String),
                                                   CType(strappname, String), "AccountsModule\RptBalanceSheetNew.aspx?appid=" + strappid, btnadd, Button1, btnLoadReprt, gv_SearchResult)
            End If

            '*** Danny 04/04/2018>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
            Dim divid As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select division_master_code from division_master where accountsmodulename='" & Session("DAppName") & "'")
            '*** Danny 04/04/2018<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
            ViewState.Add("divcode", divid)
            divcode = ViewState("divcode")

            If Page.IsPostBack = False Then
                max = CType(objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected From reservation_parameters where param_id=507"), String)

                Dim k As ListItem
                For i As Integer = 1 To max
                    k = New ListItem(i, i)
                    ddlselect.Items.Add(k)

                Next
                ddlselect.SelectedValue = max
                txtFromDate.Text = Format(ObjDate.GetSystemDateOnly(Session("dbconnectionName")), "dd/MM/yyyy")


                If Request.QueryString("fromdate") <> "" Then
                    txtFromDate.Text = Format(Request.QueryString("fromdate"), "dd/MM/yyyy")
                End If

                If Request.QueryString("level") <> "" Then
                    ddlselect.SelectedValue = Request.QueryString("level")
                End If

                Dim typ As Type
                typ = GetType(DropDownList)

                ddlselect.Visible = True
                Button1.Visible = False
            End If
            ClientScript.GetPostBackEventReference(Me, String.Empty)
            If (Me.IsPostBack And Me.Request("__EVENTTARGET") = "RepBalSheetWindowPostBack") Then
                btnLoadReprt_Click(sender, e)
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Error Description: - " & ex.Message & "');", True)
            objUtils.WritErrorLog("RptCashBankBook.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#End Region

    Protected Sub btnLoadReprt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnLoadReprt.Click
        Try
            'If ValidatePage() = True Then
            'Session.Add("Pageame", "RptBalancesheet")
            'Session.Add("BackPageName", "RptBalancesheet.aspx")

            If ValidatePage() = False Then
                Exit Sub
            End If

            Dim strtodate, strlevel As String
            strtodate = Mid(Format(CType(txtFromDate.Text, Date), "yyyy/MM/dd"), 1, 10)
            strlevel = ddlselect.SelectedIndex + 1
            '            Response.Redirect("rptgltrialbalReport.aspx?pagetype=B&todate=" & strtodate & "&level=" & strlevel, False)



            If chknew.Checked = True Then

                mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                sqlTrans = mySqlConn.BeginTransaction

                mySqlCmd = New SqlCommand
                mySqlCmd.Connection = mySqlConn
                mySqlCmd.Transaction = sqlTrans

                mySqlCmd.CommandType = CommandType.StoredProcedure
                mySqlCmd.CommandText = "sp_Balance"
                mySqlCmd.CommandTimeout = 0
                Dim parms As New List(Of SqlParameter)
                Dim parm(1) As SqlParameter

                parm(0) = New SqlParameter("@date", strtodate)
                parm(1) = New SqlParameter("@division", ViewState("divcode"))

                For i = 0 To 1
                    mySqlCmd.Parameters.Add(parm(i))
                Next
                mySqlCmd.ExecuteNonQuery()
                sqlTrans.Commit()

            End If



            Dim strpop As String = ""
            'strpop = "window.open('rptgltrialbalReport.aspx?pagetype=B&todate=" & strtodate & "&newformat=" & IIf(chknew.Checked = True, 1, 0) & "&level=" & strlevel & "','RepBalSheet','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes' );"
            strpop = "window.open('rptgltrialbalReport.aspx?pagetype=B&todate=" & strtodate & "&newformat=" & IIf(chknew.Checked = True, 1, 0) & "&divid=" & divcode & "&level=" & strlevel & "','RepBalSheet');"
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
            'End If
        Catch ex As Exception
            If mySqlConn.State = ConnectionState.Open Then
                sqlTrans.Rollback()
                clsDBConnect.dbSqlTransation(sqlTrans)                      ' sql Transaction disposed 
                clsDBConnect.dbCommandClose(mySqlCmd)                       'sql command disposed
                clsDBConnect.dbConnectionClose(mySqlConn)                   'connection close
            End If
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("RptBalancesheet.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try

    End Sub



    Protected Sub btnhelp_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=RptBalanceSheet','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub

    Public Function ValidatePage() As Boolean
        'Dim frmdate, todate As Date
        Dim objDateTime As New clsDateTime
        Dim strfromcode, strtocode As Integer

        Dim frmdate As DateTime
        Dim MyCultureInfo As New CultureInfo("fr-Fr")
        Dim ds As DataSet

        Try
            frmdate = DateTime.Parse(txtFromDate.Text, MyCultureInfo, DateTimeStyles.None)
            ds = objUtils.ExecuteQuerySqlnew(Session("dbconnectionName"), "select option_selected  from reservation_parameters where param_id=1103")
            If ds.Tables.Count > 0 Then
                If ds.Tables(0).Rows.Count > 0 Then
                    If IsDBNull(ds.Tables(0).Rows(0)(0)) = False Then
                        If frmdate < ds.Tables(0).Rows(0)("option_selected") Then
                            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('From date cannot enter below the " & ds.Tables(0).Rows(0)("option_selected") & " ' );", True)
                            ValidatePage = False
                            Exit Function
                        End If
                    End If
                End If
            End If

            ValidatePage = True

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)

        End Try
    End Function


    Protected Sub Button1_Click1(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button1.Click
        Try

            Dim parms As New List(Of SqlParameter)
            Dim i As Integer
            Dim parm(2) As SqlParameter

            Dim strtodate, strlevel As String
            strtodate = Mid(Format(CType(txtFromDate.Text, Date), "yyyy/MM/dd"), 1, 10)
            strlevel = ddlselect.SelectedValue

            parm(0) = New SqlParameter("@todate", strtodate)
            parm(1) = New SqlParameter("@div_code", divcode)
            parm(2) = New SqlParameter("@level", strlevel)

            For i = 0 To 2
                parms.Add(parm(i))
            Next

            Dim ds As New DataSet
            ds = objUtils.ExecuteQuerynew(Session("dbconnectionName"), "sp_rep_balancesheet_xls", parms)

            If ds.Tables.Count > 0 Then
                If ds.Tables(0).Rows.Count > 0 Then
                    objUtils.ExportToExcel(ds, Response)
                End If
            Else
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('No Record found' );", True)
            End If

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("RptBalancesheet.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))

        End Try

    End Sub




    Protected Sub chknew_CheckedChanged1(ByVal sender As Object, ByVal e As System.EventArgs) Handles chknew.CheckedChanged
        If chknew.Checked = True Then
            ddlselect.Visible = True
            ddlselect.Enabled = True
        Else
            ddlselect.Visible = False
            ddlselect.Enabled = False
        End If
    End Sub
    Protected Sub btnPdfReport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPdfReport.Click
        Try
            If ValidatePage() = False Then
                Exit Sub
            End If

            Dim strtodate, strlevel As String
            strtodate = Mid(Format(CType(txtFromDate.Text, Date), "yyyy/MM/dd"), 1, 10)
            strlevel = ddlselect.SelectedIndex + 1
            If chknew.Checked = True Then

                mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                sqlTrans = mySqlConn.BeginTransaction

                mySqlCmd = New SqlCommand
                mySqlCmd.Connection = mySqlConn
                mySqlCmd.Transaction = sqlTrans

                mySqlCmd.CommandType = CommandType.StoredProcedure
                mySqlCmd.CommandText = "sp_Balance"
                mySqlCmd.CommandTimeout = 0
                Dim parms As New List(Of SqlParameter)
                Dim parm(1) As SqlParameter

                parm(0) = New SqlParameter("@date", strtodate)
                parm(1) = New SqlParameter("@division", ViewState("divcode"))

                For i = 0 To 1
                    mySqlCmd.Parameters.Add(parm(i))
                Next
                mySqlCmd.ExecuteNonQuery()
                sqlTrans.Commit()

            End If



            Dim strpop As String = ""
            strpop = "window.open('TransactionReports.aspx?printId=BalanceSheet&reportsType=pdf&pagetype=B&todate=" & strtodate & "&newformat=" & IIf(chknew.Checked = True, 1, 0) & "&divid=" & divcode & "&level=" & strlevel & "','RepBalSheet');"
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
            'End If
        Catch ex As Exception
            If mySqlConn.State = ConnectionState.Open Then
                sqlTrans.Rollback()
                clsDBConnect.dbSqlTransation(sqlTrans)                      ' sql Transaction disposed 
                clsDBConnect.dbCommandClose(mySqlCmd)                       'sql command disposed
                clsDBConnect.dbConnectionClose(mySqlConn)                   'connection close
            End If
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("RptBalancesheet.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try

    End Sub

    Protected Sub btnExcelReport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExcelReport.Click
        Try
            If ValidatePage() = False Then
                Exit Sub
            End If

            Dim strtodate, strlevel As String
            strtodate = Mid(Format(CType(txtFromDate.Text, Date), "yyyy/MM/dd"), 1, 10)
            strlevel = ddlselect.SelectedIndex + 1

            'If chknew.Checked = True Then

            '    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
            '    sqlTrans = mySqlConn.BeginTransaction

            '    mySqlCmd = New SqlCommand
            '    mySqlCmd.Connection = mySqlConn
            '    mySqlCmd.Transaction = sqlTrans

            '    mySqlCmd.CommandType = CommandType.StoredProcedure
            '    mySqlCmd.CommandText = "sp_Balance"
            '    mySqlCmd.CommandTimeout = 0
            '    Dim parms As New List(Of SqlParameter)
            '    Dim parm(1) As SqlParameter

            '    parm(0) = New SqlParameter("@date", strtodate)
            '    parm(1) = New SqlParameter("@division", ViewState("divcode"))
            '    For i = 0 To 1
            '        mySqlCmd.Parameters.Add(parm(i))
            '    Next
            '    mySqlCmd.ExecuteNonQuery()
            '    sqlTrans.Commit()

            'End If



            Dim strpop As String = ""
            strpop = "window.open('TransactionReports.aspx?printId=BalanceSheetNew&reportsType=excel&pagetype=B&todate=" & strtodate & "&newformat=" & IIf(chknew.Checked = True, 1, 0) & "&divid=" & divcode & "&level=" & strlevel & "','RepBalSheet');"
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
            'End If
        Catch ex As Exception
            If mySqlConn.State = ConnectionState.Open Then
                sqlTrans.Rollback()
                clsDBConnect.dbSqlTransation(sqlTrans)                      ' sql Transaction disposed 
                clsDBConnect.dbCommandClose(mySqlCmd)                       'sql command disposed
                clsDBConnect.dbConnectionClose(mySqlConn)                   'connection close
            End If
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("RptBalancesheet.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try

    End Sub

End Class
