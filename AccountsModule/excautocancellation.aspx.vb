#Region "Namespace"
Imports System.Data
Imports System.Data.SqlClient
Imports System.Collections.Generic
#End Region
Partial Class Accounts_excautocancellation
    Inherits System.Web.UI.Page
    Dim objectcl As New clsDateTime
    ' Dim rep As New ReportDocument
    Dim rptcompanyname As String
    Dim rptreportname As String
    Dim objutils As New clsUtils
    Dim servicetype As String
    Dim default_country As String
    Dim strqry As String
    Dim seasonfrom As String
    Dim seasonto As String
    Dim strReportTitle As String = ""
    Dim objdatetime As New clsDateTime

    Dim strsptypecode As String = ""
    Dim strppartycode As String = ""
    Dim strfromdate As String = ""
    Dim strtodate As String = ""
    Dim strplgrpcode As String = ""
    Dim strsellcode As String = ""
    Dim strapprove As Integer
    Dim strrpttype As Integer
    Dim strshowweb As Integer

    Dim strcitycode As String = ""
    Dim strctrycode As String = ""
    Dim strcatcode As String = ""

    Dim strroomtype As String = ""
    Dim strmeal As String = ""
    Dim strselltype As String = ""
    Dim strexcept As String = ""

    Dim strpromtype As String = ""
    Dim strrepfilter As String = ""
    Dim strreportoption As String = ""
    Dim strasondate As String = ""

    Dim objUser As New clsUser
    Dim GroupId, AppId As Integer
    Dim PrivilegeId As String

    Dim strSqlQry As String
    Dim myCommand As SqlCommand
    Dim mySqlReader As SqlDataReader
    Dim mySqlConn As SqlConnection
    Dim sqlTrans As SqlTransaction
    Dim SqlConn As SqlConnection
    Dim myDataAdapter As SqlDataAdapter
    Dim GvRow As GridViewRow
    Dim objdate As New clsDateTime
    Dim Kl As Integer = 0


    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        If Page.IsPostBack = False Then
            Try
                If CType(Session("GlobalUserName"), String) = Nothing Or CType(Session("Userpwd"), String) = Nothing Then
                    Response.Redirect("~/Login.aspx", False)
                    Exit Sub
                End If

                GroupId = objUser.GetGroupId(Session("dbconnectionName"), CType(Session("GlobalUserName"), String), CType(Session("Userpwd"), String))


            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objutils.WritErrorLog("excautocancellation.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            End Try

        End If


        Dim typ As Type
        typ = GetType(DropDownList)

        If (Page.ClientScript.IsClientScriptIncludeRegistered(typ, "TADDScript.js")) = False Then
            Page.ClientScript.RegisterClientScriptInclude(typ, "TADDScript.js", "TADDScript.js")

 
        End If
        ClientScript.GetPostBackEventReference(Me, String.Empty)
        'If (Me.IsPostBack And Me.Request("__EVENTTARGET") = "RepPLWindowPostBack") Then
        '    BtnPrint_Click(sender, e)
        'End If
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Page.IsPostBack = True Then

        Else
            pnlparty.Visible = False
            btnSelectAll.Visible = False
            btnUnselectAll.Visible = False
            btnSave.Visible = False
        End If
    End Sub
#Region "Private Sub FillGridParty()"
    Private Sub FillGridParty()
        Dim parms As New List(Of SqlParameter)
        Dim i As Integer
        Dim parm(8) As SqlParameter

        Try
            If Not (txtReqId.Value.Trim().ToString() = "") Then
                parm(0) = New SqlParameter("@requestid", CType(txtReqId.Value.Trim().ToString(), String))
            Else
                parm(0) = New SqlParameter("@requestid", String.Empty)
            End If

            If Not (txtFromDate.Text.Trim = "") Then
                parm(1) = New SqlParameter("@fromdate", objdatetime.ConvertDateromTextBoxToTextYearMonthDay(CType(txtFromDate.Text.Trim, String)))
            Else
                parm(1) = New SqlParameter("@fromdate", "1900/01/01")
            End If

            If Not (txtToDate.Text.Trim = "") Then
                parm(2) = New SqlParameter("@todate", objdatetime.ConvertDateromTextBoxToTextYearMonthDay(CType(txtToDate.Text.Trim, String)))
            Else
                parm(2) = New SqlParameter("@todate", "1900/01/01")
            End If

            If Not (txtPfromdate.Text.Trim = "") Then
                parm(3) = New SqlParameter("@pfromdate", objdatetime.ConvertDateromTextBoxToTextYearMonthDay(CType(txtPfromdate.Text.Trim, String)))
            Else
                parm(3) = New SqlParameter("@pfromdate", "1900/01/01")
            End If

            If Not (txtPtodate.Text.Trim = "") Then
                parm(4) = New SqlParameter("@ptodate", objdatetime.ConvertDateromTextBoxToTextYearMonthDay(CType(txtPtodate.Text.Trim, String)))
            Else
                parm(4) = New SqlParameter("@ptodate", "1900/01/01")
            End If

            If accSearch.Value <> "" Then
                parm(5) = New SqlParameter("@agentname", CType(accSearch.Value, String))
            Else
                parm(5) = New SqlParameter("@agentname", String.Empty)
            End If



            If Not (txtGFname.Value = "") Then
                parm(6) = New SqlParameter("@fguestnamee", CType(txtGFname.Value, String))
            Else
                parm(6) = New SqlParameter("@fguestname", String.Empty)
            End If

            If Not (txtGLname.Value = "") Then
                parm(7) = New SqlParameter("@lguestname", CType(txtGLname.Value, String))
            Else
                parm(7) = New SqlParameter("@lguestname", String.Empty)
            End If

            For i = 0 To 7
                parms.Add(parm(i))
            Next
            Dim ds As DataSet = objutils.ExecuteQuerynew(Session("dbconnectionName"), "sp_fill_cancellation", parms)

            If ds.Tables.Count > 0 Then
                If ds.Tables(0).Rows.Count > 0 Then
                    grdParty.DataSource = ds.Tables(0)
                    grdParty.DataBind()
                    grdParty.Visible = True
                Else
                    grdParty.Visible = False
                End If
            End If


        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objutils.WritErrorLog("SpecialEventsPriceList.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            clsDBConnect.dbAdapterClose(myDataAdapter)             ' sql reader disposed    
            clsDBConnect.dbConnectionClose(SqlConn)           'connection close           
        End Try

    End Sub
#End Region


#Region "Private Sub Save()"
    Private Sub Save()
        Dim myDS As New DataSet
        Try
            Dim chksel As CheckBox
            Dim chksel1 As CheckBox
            Dim hdncustcode As HiddenField

            'If validate() = False Then
            '    Exit Sub
            'End If

            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
            For Each Me.GvRow In grdParty.Rows
                chksel = GvRow.FindControl("chkSelect")
                chksel1 = GvRow.FindControl("chkselet1")
                hdncustcode = GvRow.FindControl("hdncustcode")

                If chksel1.Checked = True Then
                    myCommand = New SqlCommand("sp_del_markcancellation_mail", mySqlConn)
                    myCommand.CommandType = CommandType.StoredProcedure
                    If chksel1.Checked = True Then
                        myCommand.Parameters.Add(New SqlParameter("@requestid", SqlDbType.VarChar, 20)).Value = CType(GvRow.Cells(1).Text, String)
                        myCommand.ExecuteNonQuery()
                    End If
                End If

                myCommand = New SqlCommand("sp_add_markcancellation_mail", mySqlConn)
                myCommand.CommandType = CommandType.StoredProcedure

                If chksel.Checked = True Then
                    myCommand.Parameters.Add(New SqlParameter("@requestid", SqlDbType.VarChar, 20)).Value = CType(GvRow.Cells(1).Text, String)
                    myCommand.Parameters.Add(New SqlParameter("@agentcode", SqlDbType.VarChar, 20)).Value = CType(hdncustcode.Value, String)
                    myCommand.Parameters.Add(New SqlParameter("@salevalue", SqlDbType.Decimal)).Value = CType(GvRow.Cells(4).Text, Decimal)
                    myCommand.Parameters.Add(New SqlParameter("@datein", SqlDbType.VarChar, 20)).Value = objdatetime.ConvertDateromTextBoxToTextYearMonthDay(CType(GvRow.Cells(2).Text.Trim, String))
                    myCommand.Parameters.Add(New SqlParameter("@guestname", SqlDbType.VarChar, 20)).Value = CType(GvRow.Cells(5).Text, String)
                    myCommand.Parameters.Add(New SqlParameter("@Adddate", SqlDbType.DateTime)).Value = objdate.GetSystemDateTime(Session("dbconnectionName"))
                    myCommand.Parameters.Add(New SqlParameter("@Adduser", SqlDbType.VarChar, 50)).Value = CType(Session("GlobalUserName"), String)
                    myCommand.ExecuteNonQuery()
                End If
            Next

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " & "Error Num" & Kl & ex.Message.Replace("'", " ") & "' );", True)
            objutils.WritErrorLog("excautocancellation.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally

            clsDBConnect.dbConnectionClose(mySqlConn)           'connection close           
        End Try

    End Sub
#End Region

#Region "Public Function validate() As Boolean"
    Public Function validate() As Boolean
        Dim flag As Integer = 0
        Dim chksel As CheckBox

        For Each Me.GvRow In grdParty.Rows
            chksel = GvRow.FindControl("chkSelect")
            If chksel.Checked = True Then
                flag = 1
            End If
        Next

        If flag <> 1 Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Select one line for the exclude cancellation');", True)
            validate = False
            Exit Function
        End If
        validate = True
    End Function
#End Region

    Protected Sub btnSelectAll_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim chksel As CheckBox
        For Each Me.GvRow In grdParty.Rows
            chksel = GvRow.FindControl("chkSelect")
            chksel.Checked = True
        Next
    End Sub

    Protected Sub btnUnselectAll_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim chksel As CheckBox
        For Each Me.GvRow In grdParty.Rows
            chksel = GvRow.FindControl("chkSelect")
            chksel.Checked = False
        Next
    End Sub
    Protected Sub Btndisplay_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Btndisplay.Click
        Try
            Dim tdate, ptdate As Date
            Dim fdate, pfdate As Date


            If txtFromDate.Text = "" And txtToDate.Text = "" Then
                fdate = "1900/01/01"
                tdate = "1900/01/01"
            Else
                fdate = txtFromDate.Text
                tdate = txtToDate.Text
            End If


            If tdate.Date < fdate.Date Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Todate should not be greater than From date' );", True)
                Exit Sub
            End If


            If txtPfromdate.Text = "" And txtPtodate.Text = "" Then
                pfdate = "1900/01/01"
                ptdate = "1900/01/01"
            Else
                pfdate = txtPfromdate.Text
                ptdate = txtPtodate.Text
            End If


            If ptdate.Date < pfdate.Date Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Timelimit Todate should not be greater than From date' );", True)
                Exit Sub
            End If


            pnlparty.Visible = True
            btnSelectAll.Visible = True
            btnUnselectAll.Visible = True
            btnSave.Visible = True
            FillGridParty()

        Catch ex As Exception

        End Try
    End Sub
    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Try
            Save()
            Dim Str As String = "Record Saved"
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & Str & "' );", True)
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " & "Error Num" & Kl & ex.Message.Replace("'", " ") & "' );", True)
        End Try
    End Sub
#Region "Private Sub FillRecord()"
    Private Sub FillRecord()
        Dim myDS As New DataSet
        Try
            Dim chksel As CheckBox
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
            For Each Me.GvRow In grdParty.Rows
                myCommand = New SqlCommand("Sp_Add_Update_PlistParty", mySqlConn)
                myCommand.CommandType = CommandType.StoredProcedure
                myCommand.Parameters.Add(New SqlParameter("@PartyCode", SqlDbType.VarChar, 50)).Value = CType(GvRow.Cells(1).Text, String)
                chksel = GvRow.FindControl("chkSelect")
                If chksel.Checked = True Then
                    myCommand.Parameters.Add(New SqlParameter("@Printyn", SqlDbType.Int, 1)).Value = CType(1, Integer)
                Else
                    myCommand.Parameters.Add(New SqlParameter("@Printyn", SqlDbType.Int, 1)).Value = CType(0, Integer)
                End If
                myCommand.Parameters.Add(New SqlParameter("@Adddate", SqlDbType.DateTime)).Value = objdate.GetSystemDateTime(Session("dbconnectionName"))
                myCommand.Parameters.Add(New SqlParameter("@Adduser", SqlDbType.VarChar, 50)).Value = CType(Session("GlobalUserName"), String)
                myCommand.ExecuteNonQuery()
            Next
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objutils.WritErrorLog("excautocancellation.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            clsDBConnect.dbConnectionClose(SqlConn)           'connection close           
        End Try

    End Sub
#End Region

    Protected Sub grdParty_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdParty.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim btn As Button
            Dim chk As CheckBox
            btn = e.Row.FindControl("btn")
            chk = e.Row.FindControl("chkSelect")
            chk.Attributes.Add("onclick", "javascript:selectchk('" + btn.ClientID + "')")
        End If
    End Sub

    Protected Sub btn_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim objbtn As Button = CType(sender, Button)
        Dim rowid As Integer = 0
        Dim row As GridViewRow
        row = CType(objbtn.NamingContainer, GridViewRow)
        rowid = row.RowIndex
        Dim chk1 As CheckBox
        Dim chk As CheckBox
        chk1 = grdParty.Rows(rowid).FindControl("chkselet1")
        chk = grdParty.Rows(rowid).FindControl("chkSelect")

        If chk.Checked = True Then
            chk1.Checked = True
        End If
    End Sub
End Class
