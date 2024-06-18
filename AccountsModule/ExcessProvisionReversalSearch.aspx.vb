Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports System.IO.File
Imports System.Globalization
Imports ClosedXML.Excel
Imports System.Net

Partial Class ExcessProvisionReversalSearch
    Inherits System.Web.UI.Page

#Region "Global Declarations"
    Dim objUtils As New clsUtils
    Dim objDateTime As New clsDateTime
    Dim objUser As New clsUser
    Dim strSqlQry As String
    Dim strWhereCond As String
    Dim SqlConn As SqlConnection
    Dim myCommand As SqlCommand
    Dim myDataAdapter As SqlDataAdapter
    Dim strappid As String = ""
    Dim strappname As String = ""

    Dim reportfilter As String
    Dim document As New XLWorkbook
#End Region

#Region "Enum GridCol"
    Enum GridCol

        tran_id = 1
        tran_type = 2
        journal_date = 3
        journal_narration = 4
        reversalAmount = 5
        vatAmount = 6
        DateCreated = 7
        UserCreated = 8
        DateModified = 9
        UserModified = 10
        Edit = 11
        View = 12
        Delete = 13
        Viewlog = 14
    End Enum
#End Region

#Region "Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init"
    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Dim appid As String = CType(Request.QueryString("appid"), String)

        Dim AppName As HiddenField = CType(Master.FindControl("hdnAppName"), HiddenField)

        '*** Danny 04/04/2018>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
        '*** This block is removed from all other 04/04/2018 occurence
        'If appid = "4" Then
        '    strappname = "ColumbusCommon" + " " + AppName.Value
        'Else
        '    strappname = "ColumbusCommon Gulf " + AppName.Value
        'End If
        'strappname = Session("DAppName")


        If appid Is Nothing = False Then
            'If appid = "4" Then
            '    strappname = AppName.Value
            'Else
            '    strappname = AppName.Value
            'End If
            strappname = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select displayname from appmaster where appid='" & appid & "'")
        End If


        'ViewState("Appname") = strappname
        'Dim divid As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select division_master_code from division_master where accountsmodulename='" & ViewState("Appname") & "'")

        '*** Danny 04/04/2018>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
        '  Dim divid As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select division_master_code from division_master where accountsmodulename='" & Session("DAppName") & "'")
        '*** Danny 04/04/2018<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<

        ViewState("Appname") = strappname
        Dim divid As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select division_master_code from division_master where accountsmodulename='" & ViewState("Appname") & "'")


        ViewState.Add("divcode", divid)
        Page.ClientScript.RegisterHiddenField("vdivcode", ViewState("divcode"))

    End Sub
#End Region

#Region "Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim AppName As HiddenField = CType(Master.FindControl("hdnAppName"), HiddenField)
        Dim AppId As String = CType(Request.QueryString("appid"), String)

        Dim strappid As String = ""
        Dim strappname As String = ""
        If AppId Is Nothing = False Then
            strappid = AppId 'AppId.Value
        End If
        If AppName Is Nothing = False Then
            '*** Danny 04/04/2018>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>            
            strappname = Session("AppName")
            '*** Danny 04/04/2018<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
        End If

        If AppId Is Nothing = False Then
            If AppId = "4" Then
                strappname = AppName.Value
            Else
                strappname = AppName.Value
            End If
        End If

        'Tanvir 10062024
        If txtdivcode.Value = "" Then
            txtdivcode.Value = "01"
        End If
        Dim ds As DataSet = objUtils.ExecuteQuerySqlnew(Session("dbconnectionName"), "select top 1  sealdate from  sealing_master where div_code='" & txtdivcode.Value & "'")
        If ds.Tables.Count > 0 Then
            If ds.Tables(0).Rows.Count > 0 Then
                If IsDBNull(ds.Tables(0).Rows(0)("sealdate")) = False Then
                    txtpdate.Text = CType(ds.Tables(0).Rows(0)("sealdate"), String)
                End If
            Else
                txtpdate.Text = objUtils.GetDBFieldFromLongnew(Session("dbconnectionName"), "reservation_parameters", "option_Selected", "param_id", 508)
            End If
        End If
        'Tanvir 10062024

        Try
            If Page.IsPostBack = False Then
                Dim frmdate As String = ""
                Dim todate As String = ""

                btnPrint_new.Attributes.Add("onclick", "return FormValidation('')")

                RowsPerPageCUS.SelectedValue = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id='2006'")

                If CType(Session("GlobalUserName"), String) = Nothing Or CType(Session("Userpwd"), String) = Nothing Then
                    Response.Redirect("~/Login.aspx", False)
                    Exit Sub
                Else
                    objUser.CheckUserRight(Session("dbconnectionName"), CType(Session("GlobalUserName"), String), CType(Session("Userpwd"), String), _
                                                       CType(strappname, String), "AccountsModule\ExcessProvisionReversalSearch.aspx?appid=" + strappid, btnAddNew, btnPrint_new, _
                                                       btnPrint_new, gv_SearchResult, GridCol.Edit, GridCol.Delete, GridCol.View)

                End If
                Session.Add("jExpression", "tran_id")
                Session.Add("jdirection", SortDirection.Ascending)

                Session("sDtDynamic") = Nothing
                Dim dtDynamic = New DataTable()
                Dim dcCode = New DataColumn("Code", GetType(String))
                Dim dcValue = New DataColumn("Value", GetType(String))
                Dim dcCodeAndValue = New DataColumn("CodeAndValue", GetType(String))
                dtDynamic.Columns.Add(dcCode)
                dtDynamic.Columns.Add(dcValue)
                dtDynamic.Columns.Add(dcCodeAndValue)
                Session("sDtDynamic") = dtDynamic

                Dim decimalPlaces As Integer = Convert.ToInt32(objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id='509'"))
                Session.Add("decimalPlaces", decimalPlaces)
                hdnDecimalplaces.Value = decimalPlaces

                FillGridNew()
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ExcessProvisionReversalSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
        If (Me.IsPostBack And Me.Request("__EVENTTARGET") = "ProvisionReversalPostBack") Then
            FillGridNew()
        End If
    End Sub
#End Region
    Private Sub CheckPostUnpostRight(ByVal UserName As String, ByVal UserPwd As String, ByVal AppName As String, ByVal PageName As String, ByVal appid As String)
        Dim PostUnpostFlag As Boolean = False
        PostUnpostFlag = objUser.PostUnpostRightnew(Session("dbconnectionName"), UserName, UserPwd, AppName, PageName, appid)
        If PostUnpostFlag = True Then
            '    chkPost.Visible = True
            '    lblPostmsg.Visible = True
        Else
            '    chkPost.Visible = False
            '    lblPostmsg.Visible = False
            '    If ViewState("ReceiptsState") = "Edit" Then
            '        If chkPost.Checked = True Then
            '            ViewState.Add("ReceiptsState", "View")
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This transaction has been posted, you do not have rights to edit.' );", True)
            'End If
            '    End If
        End If
    End Sub
    'Tanvir 10062024
    Protected Sub lbEditDate_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim lbEditDate As LinkButton = CType(sender, LinkButton)
        Dim gvr As GridViewRow = lbEditDate.NamingContainer



        Dim lblDocNo As Label = CType(gvr.FindControl("lblTranID"), Label)
        Dim lblTranType As Label = CType(gvr.FindControl("lblTranTypePop"), Label)
        Dim lbltranid As Label = CType(gvr.FindControl("lbltranidPop"), Label)

        Dim lblTranTypedate As Label = CType(gvr.FindControl("lblTrandatePop"), Label)
        hdntranid.Value = lbltranid.Text
        hdntrantypeDate.Value = lblTranTypedate.Text
        txtdate.Text = hdntrantypeDate.Value
        hdntrantype.Value = lblTranType.Text

        lblViewDetailsPopupHeading.Text = "Edit Journal Transaction Date"
        lblpopupreceipt.Text = "Transaction Date"

        ModalFlightDetails.Show()
    End Sub
    'Tanvir 10062024
    Public Function ValidatesealDate() As Boolean
        Try

            ValidatesealDate = True
            Dim invdate As DateTime
            Dim sealdate As DateTime
            Dim MyCultureInfo As New CultureInfo("fr-Fr")
            invdate = DateTime.Parse(txtdate.Text, MyCultureInfo, DateTimeStyles.None)
            sealdate = DateTime.Parse(txtpdate.Text, MyCultureInfo, DateTimeStyles.None)
            If invdate <= sealdate Then
                ' ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Document has sealed in this period cannot make entry.Close the entry and make with another date')", True)
                ValidatesealDate = False
            End If

        Catch ex As Exception
            ValidatesealDate = False
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("requestforinvoicing.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Function

    Private Function validate_BillAgainst() As Boolean
        Try
            validate_BillAgainst = True
            Dim Alflg As Integer
            Dim ErrMsg, strdiv As String
            strdiv = ViewState("divcode") 'objUtils.GetDBFieldFromLongnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", 511)

            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
            myCommand = New SqlCommand("sp_Check_AgainstBills", SqlConn)
            myCommand.CommandType = CommandType.StoredProcedure
            myCommand.Parameters.Add(New SqlParameter("@div_id", SqlDbType.VarChar, 10)).Value = strdiv
            myCommand.Parameters.Add(New SqlParameter("@tran_id", SqlDbType.VarChar, 20)).Value = hdntranid.Value
            myCommand.Parameters.Add(New SqlParameter("@tran_type ", SqlDbType.VarChar, 10)).Value = hdntrantype.Value 'CType(ViewState("ReceiptsRVPVTranType"), String)

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
            myCommand.CommandTimeout = 0 'Tanvir 15062022
            myCommand.ExecuteNonQuery()

            Alflg = param1.Value
            ErrMsg = param2.Value

            If Alflg = 1 And ErrMsg <> "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & ErrMsg & "');", True)
                validate_BillAgainst = False
                Exit Function
            End If

            ErrMsg = objUtils.ExecuteQueryReturnStringValue("exec sp_validate_adjustment '" & strdiv & "','" & hdntranid.Value & "','" & hdntrantype.Value & "'") ''" & CType(ViewState("ReceiptsRVPVTranType"), String) & "'")
            If ErrMsg <> "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & ErrMsg & "');", True)
                validate_BillAgainst = False
                Exit Function
            End If

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("JournalSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            clsDBConnect.dbCommandClose(myCommand)
            clsDBConnect.dbConnectionClose(SqlConn)
        End Try
    End Function
    Protected Sub btnFlightCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs)



    End Sub
    Protected Sub btnUpdate_Click(sender As Object, e As System.EventArgs) Handles btnUpdate.Click
        '   Dim lbEditDate As LinkButton = CType(sender, LinkButton)
        '   Dim gvr As GridViewRow = lbEditDate.NamingContainer
        '    Dim lblDocNo As Label = CType(gvr.FindControl("lblDocNo"), Label)
        '  txtdivcode.Value = ViewState("divcode")
        'btnPrint.Attributes.Add("onclick", "return ReprintDoc()")
        Dim appname As String = ""
        Dim appidnew As String = ""
        Dim appid As String = CType(Request.QueryString("appid"), String)
        If txtdivcode.Value = "" Then
            txtdivcode.Value = "01"
        End If
        If txtdivcode.Value = "01" And hdnappid.Value <> "" Then
            appidnew = hdnappid.Value
        ElseIf txtdivcode.Value = "01" And hdnappid.Value = "" Then
            appidnew = "4"

        End If
        If txtdivcode.Value = "01" Then
            appname = "ColumbusCommon" + " " + CType("Accounts Module", String)

            appidnew = "4"
        Else
            appname = "ColumbusCommon Gulf " + CType("Accounts Module", String)
            appidnew = "14"
        End If
        CheckPostUnpostRight(CType(Session("GlobalUserName"), String), CType(Session("Userpwd"), String), CType(appname, String), "AccountsModule\ExcessProvisionReversalSearch.aspx?appid=" + appidnew, appidnew)

        If Validateseal(hdntranid.Value) Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Document has sealed cannot Edit/Delete...')", True)
            Return
        End If
        If ValidatesealDate() = False Then
            btnUpdate.Enabled = True
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Document has sealed in this period cannot make entry.Close the entry and make with another date')", True)
            ' ModalPopupLoading.Hide()
            Exit Sub
        End If

        'If validate_BillAgainst() = False Then
        '    '  btnSave.Enabled = True
        '    ModalFlightDetails.Hide()
        '    Exit Sub
        'End If

        Dim SqlConn As SqlConnection
        Dim sqlTrans As SqlTransaction
        Dim myCommand As SqlCommand

        SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
        sqlTrans = SqlConn.BeginTransaction

        myCommand = New SqlCommand("sp_update_reversal_date", SqlConn, sqlTrans)
        myCommand.CommandType = CommandType.StoredProcedure
        myCommand.Parameters.Add(New SqlParameter("@journal_div_id", SqlDbType.VarChar, 10)).Value = "01"
        myCommand.Parameters.Add(New SqlParameter("@tran_id", SqlDbType.VarChar, 20)).Value = hdntranid.Value
        myCommand.Parameters.Add(New SqlParameter("@tran_type", SqlDbType.VarChar, 10)).Value = hdntrantype.Value 'CType(ViewState("ReceiptsRVPVTranType"), String)
        myCommand.Parameters.Add(New SqlParameter("@journal_date", SqlDbType.DateTime)).Value = Format(CType(txtdate.Text, Date), "yyyy/MM/dd")
        myCommand.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
        myCommand.Parameters.Add(New SqlParameter("@adddate", SqlDbType.DateTime)).Value = objDateTime.GetSystemDateTime(Session("dbconnectionName"))
        myCommand.ExecuteNonQuery()


        sqlTrans.Commit()
        clsDBConnect.dbSqlTransation(sqlTrans)
        clsDBConnect.dbCommandClose(myCommand)
        clsDBConnect.dbConnectionClose(SqlConn)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Record save successfully');", True)
        ' FillGridNew()
    End Sub
#Region "Protected Sub btnAddNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddNew.Click"
    Protected Sub btnAddNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddNew.Click
        Dim strpop As String = ""
        strpop = "window.open('ExcessProvisionReversal.aspx?State=New&divid=" & ViewState("divcode") & "','ProvisionReversal');"
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
    End Sub
#End Region

#Region "Protected Sub gv_SearchResult_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gv_SearchResult.RowCommand"
    Protected Sub gv_SearchResult_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gv_SearchResult.RowCommand
        If e.CommandName = "" Then Exit Sub 'Tanvir 10062024
        If e.CommandName = "Page" Then Exit Sub
        If e.CommandName = "Sort" Then Exit Sub
        If e.CommandName <> "Page" Then
            Try
                Dim strpop As String = ""
                Dim actionstr As String
                actionstr = ""
                Dim lblId As Label
                lblId = gv_SearchResult.Rows(CType(e.CommandArgument.ToString, Integer)).FindControl("lblTranID")
                Dim lblTranType As Label = gv_SearchResult.Rows(CType(e.CommandArgument.ToString, Integer)).FindControl("lblTranType")

                Dim mindate1 As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select convert(varchar(10),tran_date,111) from provisionReversal_master(nolock)  where divcode='" & ViewState("divcode") & "' and tran_id='" + lblId.Text + "'")

                Dim sealdate As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select sealdate from  sealing_master where div_code='" & ViewState("divcode") & "' ")

                If e.CommandName = "EditRow" Then
                    If Validateseal(lblId.Text) Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Document has sealed cannot Edit/Delete...')", True)
                        Return
                    End If

                    If mindate1 <> "" Then
                        If Convert.ToDateTime(mindate1) <= Convert.ToDateTime(sealdate) Then

                            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Month Already Sealed');", True)
                            Return
                        End If
                    End If

                    strpop = "window.open('ExcessProvisionReversal.aspx?State=Edit&divid=" & ViewState("divcode") & "&RefCode=" + CType(lblId.Text.Trim, String) & "','ProvisionReversal');"
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
                ElseIf e.CommandName = "DeleteRow" Then
                    If Validateseal(lblId.Text) Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Document has sealed cannot Edit/Delete...')", True)
                        Return
                    End If

                    If mindate1 <> "" Then
                        If Convert.ToDateTime(mindate1) <= Convert.ToDateTime(sealdate) Then

                            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Month Already Sealed');", True)
                            Return
                        End If
                    End If

                    strpop = "window.open('ExcessProvisionReversal.aspx?State=Delete&divid=" & ViewState("divcode") & "&RefCode=" + CType(lblId.Text.Trim, String) & "','ProvisionReversal');"
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
                ElseIf e.CommandName = "View" Then
                    actionstr = "View"
                    strpop = "window.open('ExcessProvisionReversal.aspx?State=View&divid=" & ViewState("divcode") & "&RefCode=" + CType(lblId.Text.Trim, String) & "','ProvisionReversal');"
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)

                ElseIf e.CommandName = "ViewLog" Then

                    actionstr = "ViewLog"
                    strpop = "window.open('ViewLog.aspx?State=" + CType(actionstr, String) + "&divid=" & ViewState("divcode") & "&RefCode=" + CType(lblId.Text.Trim, String) + "&trantype=" + CType(lblTranType.Text.Trim, String) + "','Provision Reversal Journal');"
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
                End If

            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objUtils.WritErrorLog("ExcessProvisionReversalSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            End Try
        End If
    End Sub
#End Region

#Region "Public Function Validateseal(ByVal tranid As String) As Boolean"
    Public Function Validateseal(ByVal tranid As String) As Boolean
        Try
            Dim sealdate As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select sealdate from sealing_master")
            If IsDate(sealdate) Then
                Dim ds As DataSet = objUtils.ExecuteQuerySqlnew(Session("dbconnectionName"), "select * from  provisionReversal_master(nolock) where div_id='" & ViewState("divcode") & "' and tran_id='" + tranid + "' ")
                If ds.Tables.Count > 0 Then
                    If ds.Tables(0).Rows.Count > 0 Then
                        If IsDBNull(ds.Tables(0).Rows(0)("tran_date")) = False Then

                            If (CType(ds.Tables(0).Rows(0)("tran_date"), Date) <= CType(sealdate, Date)) Then
                                Return True
                            Else
                                Return False
                            End If
                            'If ds.Tables(0).Rows(0)("tran_state") = "S" Then
                            '    Return True
                            'Else
                            '    Return False
                            'End If
                        End If
                    End If
                End If
            End If
        Catch ex As Exception
            Validateseal = False
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ExcessProvisionReversalSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Function
#End Region

#Region "Protected Sub gv_SearchResult_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gv_SearchResult.PageIndexChanging"
    Protected Sub gv_SearchResult_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gv_SearchResult.PageIndexChanging
        gv_SearchResult.PageIndex = e.NewPageIndex
        FillGridNew()
    End Sub
#End Region

#Region "Protected Sub gv_SearchResult_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gv_SearchResult.Sorting"
    Protected Sub gv_SearchResult_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gv_SearchResult.Sorting
        Session.Add("jExpression", e.SortExpression)
        SortGridColoumn_click()
    End Sub
#End Region

#Region "Public Sub SortGridColoumn_click()"
    Public Sub SortGridColoumn_click()
        Dim DataTable As DataTable
        Dim myDS As New DataSet
        FillGridNew()

        myDS = gv_SearchResult.DataSource
        DataTable = myDS.Tables(0)
        If IsDBNull(DataTable) = False Then
            Dim dataView As DataView = DataTable.DefaultView
            Session.Add("jdirection", objUtils.SwapSortDirection(Session("jdirection")))
            dataView.Sort = Session("jExpression") & " " & objUtils.ConvertSortDirectionToSql(Session("jdirection"))
            gv_SearchResult.DataSource = dataView
            gv_SearchResult.DataBind()
        End If
    End Sub
#End Region

#Region "Protected Sub btnhelp_Click(ByVal sender As Object, ByVal e As System.EventArgs)"
    Protected Sub btnhelp_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=JournalSearch','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub
#End Region

#Region "Protected Sub btnvsprocess_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnvsprocess.Click"
    Protected Sub btnvsprocess_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnvsprocess.Click

        Try
            FilterGrid()
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ExcessProvisionReversalSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try

    End Sub
#End Region

#Region "Private Sub FillGridNew()"
    Private Sub FillGridNew()
        Dim dtt As DataTable
        dtt = Session("sDtDynamic")

        Dim strDocValue As String = ""
        Dim strDescValue As String = ""
        Dim strStatusValue As String = ""
        Dim strTextValue As String = ""
        Dim strrefValue As String = ""
        Dim strBindCondition As String = ""
        Try
            If dtt.Rows.Count > 0 Then
                For i As Integer = 0 To dtt.Rows.Count - 1


                    If dtt.Rows(i)("Code").ToString = "JOURNALNO" Then
                        If strDocValue <> "" Then
                            strDocValue = strDocValue + ",'" + dtt.Rows(i)("Value").ToString + "'"
                        Else
                            strDocValue = "'" + dtt.Rows(i)("Value").ToString + "'"
                        End If
                    End If
                    If dtt.Rows(i)("Code").ToString = "NARRATION" Then
                        If strDescValue <> "" Then
                            strDescValue = strDescValue + ",'" + dtt.Rows(i)("Value").ToString + "'"
                        Else
                            strDescValue = "'" + dtt.Rows(i)("Value").ToString + "'"
                        End If
                    End If
                    If dtt.Rows(i)("Code").ToString = "TEXT" Then
                        If strTextValue <> "" Then
                            strTextValue = strTextValue + "," + dtt.Rows(i)("Value").ToString + ""
                        Else
                            strTextValue = "" + dtt.Rows(i)("Value").ToString + ""
                        End If
                    End If


                Next
            End If
            Dim pagevaluecs = RowsPerPageCUS.SelectedValue
            strBindCondition = BuildConditionNew(strDocValue, strDescValue, strStatusValue, strTextValue, strrefValue)
            Dim myDS As New DataSet
            Dim strValue As String
            gv_SearchResult.Visible = True
            lblMessg.Visible = False
            If gv_SearchResult.PageIndex < 0 Then

                gv_SearchResult.PageIndex = 0
            End If

            strSqlQry = "select tran_id,tran_type, 'Posted' as post_state, tran_date as journal_date ,tran_date as journal_tran_date, " _
            & " '' journal_mrv,'' as journal_salesperson_code, narration as journal_narration, " _
            & " '' journal_tran_state, adddate, adduser, moddate, moduser, divcode as div_id, totalReversalAmount, totalReversalVatAmount " _
            & " from provisionReversal_master(nolock)  where divcode='" & ViewState("divcode") & "'"
            Dim strorderby As String = Session("jExpression")
            Dim strsortorder As String = "DESC"

            If strBindCondition <> "" Then
                strSqlQry = strSqlQry & " and " & strBindCondition & " ORDER BY " & strorderby & " " & strsortorder
            Else
                strSqlQry = strSqlQry & " ORDER BY " & strorderby & " " & strsortorder
            End If
            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)
            'Session("SSqlQuery") = strSqlQry
            'myDS = clsUtils.GetDetailsPageWise(1, 10, strSqlQry)
            gv_SearchResult.DataSource = myDS
            If myDS.Tables(0).Rows.Count > 0 Then
                gv_SearchResult.PageSize = pagevaluecs
                gv_SearchResult.DataBind()
            Else
                gv_SearchResult.PageIndex = 0
                gv_SearchResult.DataBind()
                lblMessg.Visible = True
                lblMessg.Text = "Records not found, Please redefine search criteria."
            End If

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ExcessProvisionReversalSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            'clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
            'clsDBConnect.dbConnectionClose(SqlConn)                          'Close connection
        End Try

    End Sub
#End Region

#Region "Private Function BuildConditionNew(ByVal strDocValue As String, ByVal strDescValue As String, ByVal strStatusValue As String, ByVal strTextValue As String, ByVal strrefValue As String) As String"
    Private Function BuildConditionNew(ByVal strDocValue As String, ByVal strDescValue As String, ByVal strStatusValue As String, ByVal strTextValue As String, ByVal strrefValue As String) As String
        strWhereCond = ""
        If strDocValue.Trim <> "" Then
            If Trim(strWhereCond) = "" Then

                strWhereCond = "upper(tran_id) IN (" & Trim(strDocValue.Trim.ToUpper) & ")"
            Else
                strWhereCond = strWhereCond & " AND  upper(tran_id) IN (" & Trim(strDocValue.Trim.ToUpper) & ")"
            End If

        End If

        If strDescValue <> "" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = "upper(narration)  IN (" & Trim(strDescValue.Trim.ToUpper) & ")"
            Else
                strWhereCond = strWhereCond & " AND  upper(narration) IN (" & Trim(strDescValue.Trim.ToUpper) & ")"
            End If
        End If

        'If strStatusValue <> "" Then
        '    If Trim(strWhereCond) = "" Then
        '        strWhereCond = "upper(provisionReversal_master.post_state)  IN (" & Trim(strStatusValue.Trim.ToUpper) & ")"
        '    Else
        '        strWhereCond = strWhereCond & " AND  upper(provisionReversal_master.post_state) IN (" & Trim(strStatusValue.Trim.ToUpper) & ")"
        '    End If
        'End If


        If strTextValue <> "" Then
            Dim strValue2 As String = ""
            Dim strUNPOSTED = "UNPOSTED"
            Dim strPOSTED = "POSTED"
            Dim lsMainArr As String()
            Dim strValue As String = ""
            Dim strWhereCond1 As String = ""
            lsMainArr = objUtils.splitWithWords(strTextValue, ",")
            For i = 0 To lsMainArr.GetUpperBound(0)
                strValue = ""
                strValue = lsMainArr(i)
                If strValue <> "" Then

                    If strValue = "POSTED" Then

                        strValue2 = "P"
                        If Trim(strWhereCond1) = "" Then

                            strWhereCond1 = "upper(provisionReversal_master.tran_id) LIKE '%" & Trim(strPOSTED.Trim.ToUpper) & "%' or  upper(journal_narration) LIKE '%" & Trim(strPOSTED.Trim.ToUpper) & "%'  or upper(provisionReversal_master.post_state) LIKE '%" & Trim(strValue2.Trim.ToUpper) & "%'"
                        Else
                            strWhereCond1 = strWhereCond1 & " OR  upper(provisionReversal_master.tran_id) LIKE '%" & Trim(strPOSTED.Trim.ToUpper) & "%' or  upper(journal_narration) LIKE '%" & Trim(strPOSTED.Trim.ToUpper) & "%'  or upper(provisionReversal_master.post_state) LIKE '%" & Trim(strValue2.Trim.ToUpper) & "%'"
                        End If

                    ElseIf strValue = "UNPOSTED" Then
                        strValue2 = "U"

                        If Trim(strWhereCond1) = "" Then

                            strWhereCond1 = "upper(provisionReversal_master.tran_id) LIKE '%" & Trim(strUNPOSTED.Trim.ToUpper) & "%' or  upper(narration) LIKE '%" & Trim(strUNPOSTED.Trim.ToUpper) & "%'  or upper(provisionReversal_master.post_state) LIKE '%" & Trim(strValue2.Trim.ToUpper) & "%'"
                        Else
                            strWhereCond1 = strWhereCond1 & " OR  upper(provisionReversal_master.tran_id) LIKE '%" & Trim(strUNPOSTED.Trim.ToUpper) & "%' or  upper(narration) LIKE '%" & Trim(strUNPOSTED.Trim.ToUpper) & "%'  or upper(provisionReversal_master.post_state) LIKE '%" & Trim(strValue2.Trim.ToUpper) & "%'"
                        End If
                    Else

                        If Trim(strWhereCond1) = "" Then

                            strWhereCond1 = " upper(provisionReversal_master.tran_id) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' or  upper(narration) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' or  upper(provisionReversal_master.post_state) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%'"
                        Else
                            strWhereCond1 = strWhereCond1 & " OR  upper(provisionReversal_master.tran_id) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' or  upper(narration) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' or  upper(provisionReversal_master.post_state) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%'"
                        End If
                    End If
                End If
            Next
            If Trim(strWhereCond) = "" Then
                strWhereCond = "(" & strWhereCond1 & ")"
            Else


                strWhereCond = strWhereCond & " AND (" & strWhereCond1 & ")"
            End If

        End If

        If txtFromDate.Text.Trim <> "" And txtToDate.Text <> "" Then

            If ddlOrder.SelectedValue = "C" Then
                If Trim(strWhereCond) = "" Then

                    strWhereCond = " (CONVERT(datetime, convert(varchar(10),provisionReversal_master.adddate,103),103)  between CONVERT(datetime, '" + txtFromDate.Text + "',103) and CONVERT(datetime,  '" + txtToDate.Text + "',103)) "
                Else
                    strWhereCond = strWhereCond & "  and (CONVERT(datetime, convert(varchar(10),provisionReversal_master.adddate,103),103)  between CONVERT(datetime, '" + txtFromDate.Text + "',103) and CONVERT(datetime,  '" + txtToDate.Text + "',103)) "
                End If
            ElseIf ddlOrder.SelectedValue = "M" Then
                If Trim(strWhereCond) = "" Then

                    strWhereCond = " (CONVERT(datetime, convert(varchar(10),provisionReversal_master.moddate,103),103)  between CONVERT(datetime, '" + txtFromDate.Text + "',103) and CONVERT(datetime,  '" + txtToDate.Text + "',103)) "
                Else
                    strWhereCond = strWhereCond & "  and (CONVERT(datetime, convert(varchar(10),provisionReversal_master.moddate,103),103)  between CONVERT(datetime, '" + txtFromDate.Text + "',103) and CONVERT(datetime,  '" + txtToDate.Text + "',103)) "
                End If
            ElseIf ddlOrder.SelectedValue = "T" Then
                If Trim(strWhereCond) = "" Then

                    strWhereCond = " (CONVERT(datetime, convert(varchar(10),provisionReversal_master.tran_date,103),103)  between CONVERT(datetime, '" + txtFromDate.Text + "',103) and CONVERT(datetime,  '" + txtToDate.Text + "',103)) "
                Else
                    strWhereCond = strWhereCond & "  and (CONVERT(datetime, convert(varchar(10),provisionReversal_master.tran_date,103),103)  between CONVERT(datetime, '" + txtFromDate.Text + "',103) and CONVERT(datetime,  '" + txtToDate.Text + "',103)) "
                End If
            End If



        End If



        BuildConditionNew = strWhereCond
    End Function
#End Region

#Region "Protected Sub lbClose_Click(ByVal sender As Object, ByVal e As System.EventArgs)"
    Protected Sub lbClose_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            Dim myButton As Button = CType(sender, Button)
            Dim dlItem As DataListItem = CType((CType(sender, Button)).NamingContainer, DataListItem)
            Dim lnkcode As Button = CType(dlItem.FindControl("lnkcode"), Button)
            Dim lnkvalue As Button = CType(dlItem.FindControl("lnkvalue"), Button)
            Dim dtDynamics As New DataTable
            dtDynamics = Session("sDtDynamic")
            If dtDynamics.Rows.Count > 0 Then
                Dim j As Integer
                For j = dtDynamics.Rows.Count - 1 To 0 Step j - 1
                    If lnkcode.Text.Trim.ToUpper = dtDynamics.Rows(j)("code").ToString.Trim.ToUpper And lnkvalue.Text.Trim.ToUpper = dtDynamics.Rows(j)("Value").ToString.Trim.ToUpper Then
                        dtDynamics.Rows.Remove(dtDynamics.Rows(j))
                    End If
                Next
            End If
            Session("sDtDynamic") = dtDynamics
            dlList.DataSource = dtDynamics
            dlList.DataBind()

            FillGridNew()
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        End Try
    End Sub
#End Region

#Region "Protected Sub btnClearDate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClearDate.Click"
    Protected Sub btnClearDate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClearDate.Click
        txtFromDate.Text = ""
        txtToDate.Text = ""
        FillGridNew()
    End Sub
#End Region

#Region "Public Function getRowpage() As String"
    Public Function getRowpage() As String
        Dim rowpagecs As String
        If RowsPerPageCUS.SelectedValue = "20" Then
            rowpagecs = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id='2006'")
        Else
            rowpagecs = RowsPerPageCUS.SelectedValue

        End If
        Return rowpagecs
    End Function
#End Region

#Region "Protected Sub btnFilter_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnFilter.Click"
    Protected Sub btnFilter_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnFilter.Click
        Try
            FillGridNew()
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ExcessProvisionReversalSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#End Region

#Region "Private Sub FilterGrid()"
    Private Sub FilterGrid()
        Dim lsSearchTxt As String = ""
        lsSearchTxt = txtvsprocesssplit.Text
        Dim lsProcessCity As String = ""
        Dim lsProcessCountry As String = ""
        Dim lsProcessGroup As String = ""
        Dim lsProcessSector As String = ""
        Dim lsProcessAll As String = ""



        Dim lsMainArr As String()
        lsMainArr = objUtils.splitWithWords(lsSearchTxt, "|~,")
        For i = 0 To lsMainArr.GetUpperBound(0)
            Select Case lsMainArr(i).Split(":")(0).ToString.ToUpper.Trim
                Case "JOURNALNO"
                    lsProcessCity = lsMainArr(i).Split(":")(1)
                    sbAddToDataTable("JOURNALNO", lsProcessCity, "JOURNALNO")
                Case "NARRATION"
                    lsProcessCity = lsMainArr(i).Split(":")(1)
                    sbAddToDataTable("NARRATION", lsProcessCity, "NARRATION")
                Case "STATUS"
                    lsProcessCity = lsMainArr(i).Split(":")(1)
                    sbAddToDataTable("STATUS", lsProcessCity, "STATUS")
                Case "REFERENCE"
                    lsProcessAll = lsMainArr(i).Split(":")(1)
                    sbAddToDataTable("REFERENCE", lsProcessAll, "REFERENCE")
                Case "TEXT"
                    lsProcessAll = lsMainArr(i).Split(":")(1)
                    sbAddToDataTable("TEXT", lsProcessAll, "TEXT")
            End Select

        Next

        Dim dtt As DataTable
        dtt = Session("sDtDynamic")
        dlList.DataSource = dtt
        dlList.DataBind()

        FillGridNew() 'Bind Gird based selection 

    End Sub
#End Region

#Region "Function sbAddToDataTable(ByVal lsName As String, ByVal lsValue As String, ByVal lsShortCode As String) As Boolean"
    Function sbAddToDataTable(ByVal lsName As String, ByVal lsValue As String, ByVal lsShortCode As String) As Boolean
        Dim iFlag As Integer = 0
        Dim dtt As DataTable
        dtt = Session("sDtDynamic")

        If dtt.Rows.Count >= 0 Then
            For i = 0 To dtt.Rows.Count - 1
                If dtt.Rows(i)("Code").ToString.Trim.ToUpper = lsName.Trim.ToUpper And dtt.Rows(i)("Value").ToString.Trim.ToUpper = lsValue.Trim.ToUpper Then
                    iFlag = 1
                End If
            Next
            If iFlag = 0 Then
                dtt.NewRow()
                dtt.Rows.Add(lsName.Trim.ToUpper, lsValue.Trim.ToUpper, lsShortCode.Trim.ToUpper & ": " & lsValue.Trim)
                Session("sDtDynamic") = dtt
            End If
        End If
        Return True
    End Function
#End Region

#Region "Protected Sub RowsPerPageCUS_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RowsPerPageCUS.SelectedIndexChanged"
    Protected Sub RowsPerPageCUS_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RowsPerPageCUS.SelectedIndexChanged
        FillGridNew()
    End Sub
#End Region

#Region "Protected Sub btnResetSelection_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnResetSelection.Click"
    Protected Sub btnResetSelection_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnResetSelection.Click
        Dim dtt As DataTable
        dtt = Session("sDtDynamic")
        If dtt.Rows.Count > 0 Then
            dtt.Clear()
        End If
        Session("sDtDynamic") = dtt
        dlList.DataSource = dtt
        dlList.DataBind()
        FillGridNew()

    End Sub
#End Region

#Region "Private Sub detailedReport()"
    Private Sub detailedReport()
        Dim FolderPath As String = "..\ExcelTemplates\"
        Dim FileName As String = "accountsTransactionDetailed_template.xlsx"
        Dim FilePath As String = Server.MapPath(FolderPath + FileName)
        Dim rptcompanyname, basecurrency As String

        rptcompanyname = CType(objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select division_master_des from division_master where division_master_code='" & ViewState("divcode") & "'"), String)
        basecurrency = CType(objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id=457"), String)

        Dim FileNameNew As String
        FileNameNew = "ProvisionReversalDetailed_" & Now.Year & Now.Month & Now.Day & Now.Hour & Now.Minute & Now.Second & Now.Millisecond & ".xlsx"

        document = New XLWorkbook(FilePath)
        Dim ws As IXLWorksheet = document.Worksheet("register")
        ws.Style.Font.FontName = "arial"

        Dim LastLine As Integer
        ws.Cell(1, 1).Value = rptcompanyname

        'create header
        ws.Cell(2, 1).Value = "Provision Reversal Journal Register:Detailed Report"
        ws.Cell(6, 1).Value = "JVNo"
        ws.Cell(6, 3).Value = "Reference"
        ws.Cell(6, 4).Value = "Type"
        ws.Cell(6, 2).Value = "Date"


        ws.Cell(6, 5).Value = "Account"
        ws.Cell(6, 6).Value = "Booking No"
        ws.Cell(6, 7).Value = "Currency"
        ws.Cell(6, 8).Value = "Rate"

        ws.Cell(6, 9).Value = "Debit"
        ws.Cell(6, 10).Value = "Credit"
        ws.Cell(6, 11).Value = "Debit"
        ws.Cell(6, 11).Value = ws.Cell(6, 11).Value & " (" & basecurrency & ")"
        ws.Cell(6, 12).Value = "Credit"
        ws.Cell(6, 12).Value = ws.Cell(6, 12).Value & " (" & basecurrency & ")"
        ws.Cell(6, 13).Value = "Narration"

        ws.Column(13).Width = 57.29
        'ws.Column(14).Delete()
        'ws.Column(14).Delete()
        Dim sql As String

        sql = FillGridNew_report()
        ws.Cell(4, 1).Value = reportfilter

        Dim myDS As New DataSet
        SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))          'connection open
        myCommand = New SqlCommand("sp_rpt_provisionReversal", SqlConn)
        myCommand.CommandType = CommandType.StoredProcedure
        myCommand.Parameters.Add(New SqlParameter("@divCode", SqlDbType.VarChar, 20)).Value = Convert.ToString(ViewState("divcode"))
        myCommand.Parameters.Add(New SqlParameter("@bindCond", SqlDbType.VarChar)).Value = strSqlQry
        Using myDataAdapter As New SqlDataAdapter(myCommand)
            myDataAdapter.Fill(myDS)
        End Using
        clsDBConnect.dbCommandClose(myCommand)
        clsDBConnect.dbConnectionClose(SqlConn)
        Dim dt As New DataTable
        Dim dt_detail_ds As New DataTable
        Dim dt_row As New DataTable
        dt = myDS.Tables(0)
        dt_detail_ds = myDS.Tables(1)

        LastLine = 7
        Dim total_basedebit, total_basecredit As Double

        total_basedebit = 0
        total_basecredit = 0
        If dt.Rows.Count > 0 Then
            For i As Integer = 0 To dt.Rows.Count - 1
                
                Dim dt_detail As New DataTable
                Dim id As String = dt.Rows(i).Item("tran_id").ToString

                dt_detail = (From n In dt_detail_ds.AsEnumerable Where n.Field(Of String)("filterid") = id Select n Order By n.Field(Of Integer)("acc_tran_lineno")).CopyToDataTable()

                Dim RateSheet_detail As IXLRange

                RateSheet_detail = ws.Cell(LastLine, 1).InsertTable(dt_detail.AsEnumerable).SetShowHeaderRow(False).AsRange()
                ws.Rows(LastLine).Clear()
                ws.Rows(LastLine).Delete()
                RateSheet_detail.Style.Font.FontName = "arial"
                RateSheet_detail.Columns("8:12").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right
                Dim style3 As IXLBorder = RateSheet_detail.Cells.Style.Border
                style3.BottomBorder = XLBorderStyleValues.Thin
                style3.LeftBorder = XLBorderStyleValues.Thin
                RateSheet_detail.Style.Border.OutsideBorder = XLBorderStyleValues.Thin
                total_basedebit = total_basedebit + Convert.ToDouble(dt_detail.Compute("SUM(basedebit)", String.Empty))
                total_basecredit = total_basecredit + Convert.ToDouble(dt_detail.Compute("SUM(basecredit)", String.Empty))

                LastLine = LastLine + dt_detail.Rows.Count
                LastLine = LastLine + 1

            Next

            ws.Cell(LastLine, 10).Value = "Total"
            ws.Cell(LastLine, 10).Style.Font.Bold = True
            ws.Cell(LastLine, 10).Style.Font.FontSize = 14
            ws.Cell(LastLine, 10).Style.Font.FontName = "arial"

            ws.Cell(LastLine, 11).Value = total_basedebit
            ws.Cell(LastLine, 11).Style.Font.Bold = True
            ws.Cell(LastLine, 11).Style.Font.FontSize = 14
            ws.Cell(LastLine, 11).Style.Font.FontName = "arial"

            ws.Cell(LastLine, 12).Value = total_basecredit
            ws.Cell(LastLine, 12).Style.Font.Bold = True
            ws.Cell(LastLine, 12).Style.Font.FontSize = 14
            ws.Cell(LastLine, 12).Style.Font.FontName = "arial"

            ws.Column(14).Delete()
            ws.Column(14).Delete()
        End If

        Using MyMemoryStream As New MemoryStream()
            document.SaveAs(MyMemoryStream)
            document.Dispose()
            Response.Clear()
            Response.Buffer = True
            Response.AddHeader("content-disposition", "attachment;filename=" + FileNameNew)
            Response.AddHeader("Content-Length", MyMemoryStream.Length.ToString())
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
            MyMemoryStream.WriteTo(Response.OutputStream)
            Response.Cookies.Add(New HttpCookie("Downloaded", "True"))
            Response.Flush()
            HttpContext.Current.ApplicationInstance.CompleteRequest()
        End Using
    End Sub
#End Region

#Region "Protected Sub btnPrint_new_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrint_new.Click"
    Protected Sub btnPrint_new_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrint_new.Click
        detailedReport()
    End Sub
#End Region

#Region "Private Function FillGridNew_report() As String"
    Private Function FillGridNew_report() As String

        Dim dtt As DataTable
        dtt = Session("sDtDynamic")

        Dim strDocValue As String = ""
        Dim strDescValue As String = ""
        Dim strStatusValue As String = ""
        Dim strTextValue As String = ""
        Dim strrefValue As String = ""
        Dim strBindCondition As String = ""
        If txtFromDate.Text.Trim <> "" And txtToDate.Text <> "" Then
            reportfilter = "Transaction From  " & txtFromDate.Text.Trim & " To " & txtToDate.Text
        End If

        Try
            If dtt.Rows.Count > 0 Then
                For i As Integer = 0 To dtt.Rows.Count - 1

                    If dtt.Rows(i)("Code").ToString = "JOURNALNO" Then
                        If strDocValue <> "" Then
                            strDocValue = strDocValue + ",'" + dtt.Rows(i)("Value").ToString + "'"
                        Else
                            strDocValue = "'" + dtt.Rows(i)("Value").ToString + "'"
                        End If
                    End If
                    If dtt.Rows(i)("Code").ToString = "NARRATION" Then
                        If strDescValue <> "" Then
                            strDescValue = strDescValue + ",'" + dtt.Rows(i)("Value").ToString + "'"
                        Else
                            strDescValue = "'" + dtt.Rows(i)("Value").ToString + "'"
                        End If
                    End If
                    If dtt.Rows(i)("Code").ToString = "TEXT" Then
                        If strTextValue <> "" Then
                            strTextValue = strTextValue + "," + dtt.Rows(i)("Value").ToString + ""
                        Else
                            strTextValue = "" + dtt.Rows(i)("Value").ToString + ""
                        End If
                    End If
                Next
            End If

            strBindCondition = BuildConditionNew(strDocValue, strDescValue, strStatusValue, strTextValue, strrefValue)
            strSqlQry = " "
            Dim strorderby As String = Session("jExpression")
            Dim strsortorder As String = "DESC"

            If strBindCondition <> "" Then
                strSqlQry = strSqlQry & " and " & strBindCondition & " ORDER BY " & strorderby & " " & strsortorder
            Else
                strSqlQry = strSqlQry & " ORDER BY " & strorderby & " " & strsortorder
            End If

            FillGridNew_report = strSqlQry

        Catch ex As Exception
            FillGridNew_report = ""
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ExcessProvisionReversalSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try

    End Function
#End Region

#Region "Protected Sub gv_SearchResult_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gv_SearchResult.RowDataBound"
    Protected Sub gv_SearchResult_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gv_SearchResult.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim decimalPlaces As Integer = Convert.ToInt32(hdnDecimalplaces.Value)
            Dim lblReversalAmt As Label = CType(e.Row.FindControl("lblReversalAmt"), Label)
            Dim lblReversalVatAmt As Label = CType(e.Row.FindControl("lblReversalVatAmt"), Label)
            If IsNumeric(lblReversalAmt.Text) Then
                lblReversalAmt.Text = Math.Round(Convert.ToDecimal(lblReversalAmt.Text), decimalPlaces).ToString
            End If
            If IsNumeric(lblReversalVatAmt.Text) Then
                lblReversalVatAmt.Text = Math.Round(Convert.ToDecimal(lblReversalVatAmt.Text), decimalPlaces).ToString
            End If
        End If
    End Sub
#End Region

End Class
