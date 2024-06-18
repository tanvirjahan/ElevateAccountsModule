'------------------------------------------------------------------------------------------------
'   Module Name    :    JournalSearch 
'   Developer Name :    Mangesh
'   Date           :    
'   
'
'------------------------------------------------------------------------------------------------
#Region "Namespace"
Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports System.IO.File
Imports System.Globalization
Imports ClosedXML.Excel
Imports System.Net
#End Region
Partial Class JournalSearch
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
        post_state = 3
        journal_date = 4
        journal_tran_date = 5
        journal_narration = 6
        DateCreated = 7
        UserCreated = 8
        DateModified = 9
        UserModified = 10
        Edit = 11
        View = 12
        Delete = 13
        Copy = 14

    End Enum
#End Region
#Region "Protected Sub rbtnsearch_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)"

    Protected Sub rbtnsearch_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        pnlSearch.Visible = False
    End Sub
#End Region
#Region "Protected Sub rbtnadsearch_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)"

    Protected Sub rbtnadsearch_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        pnlSearch.Visible = True
    End Sub
#End Region
#Region "Protected Sub btnAddNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddNew.Click"
    Protected Sub btnAddNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddNew.Click
        'Session.Add("State", "New")
        'Response.Redirect("Journal.aspx", False)
        Dim strpop As String = ""
        'strpop = "window.open('Journal.aspx?State=New','Journal','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
        strpop = "window.open('Journal.aspx?State=New&divid=" & ViewState("divcode") & "','Journal');"
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)

    End Sub
#End Region
    Protected Sub btnFlightCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs)



    End Sub
    Public Function Validateseal(ByVal tranid As String) As Boolean
        Try
            Dim tranType As String = hdntrantype.Value
            Dim ds As DataSet = objUtils.ExecuteQuerySqlnew(Session("dbconnectionName"), "select * from  journal_master where tran_id='" + tranid + "' and tran_type='" + tranType + "'")  'add tran type by param on 01/07/2021

            If ds.Tables.Count > 0 Then
                If ds.Tables(0).Rows.Count > 0 Then
                    If IsDBNull(ds.Tables(0).Rows(0)("tran_state")) = False Then
                        If ds.Tables(0).Rows(0)("tran_state") = "S" Then
                            Return True
                        Else
                            Return False
                        End If
                    End If
                End If
            End If
        Catch ex As Exception
            Validateseal = False
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ReservationSearchNew.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
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
        CheckPostUnpostRight(CType(Session("GlobalUserName"), String), CType(Session("Userpwd"), String), CType(appname, String), "AccountsModule\JournalSearch.aspx?appid=" + appidnew, appidnew)

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

        If validate_BillAgainst() = False Then
            '  btnSave.Enabled = True
            ModalFlightDetails.Hide()
            Exit Sub
        End If

        Dim SqlConn As SqlConnection
        Dim sqlTrans As SqlTransaction
        Dim myCommand As SqlCommand

        SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
        sqlTrans = SqlConn.BeginTransaction

        myCommand = New SqlCommand("sp_update_journal_date", SqlConn, sqlTrans)
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
        FillGridNew()
    End Sub
  
#Region "Private Sub FillGrid(ByVal strorderby As String, Optional ByVal strsortorder As String = 'ASC')"
    Private Sub FillGrid(ByVal strorderby As String, Optional ByVal strsortorder As String = "ASC")
        Dim myDS As New DataSet

        gv_SearchResult.Visible = True

        If gv_SearchResult.PageIndex < 0 Then
            gv_SearchResult.PageIndex = 0
        End If
        strSqlQry = ""
        Try
            strSqlQry = "select journal_div_id,tran_id,tran_type,  " & _
                        " case isnull(post_state,'') when 'P' then 'Posted' when 'U' then 'UnPosted' end  as post_state ," & _
                        " convert(varchar(10),journal_date ,103) as journal_date ," & _
                        " convert(varchar(10),journal_tran_date,103) as journal_tran_date,journal_mrv,journal_salesperson_code, " & _
                        " journal_narration, journal_tran_state, adddate, adduser, moddate, moduser, div_id, basedebit, basecredit " & _
                        " from journal_master where div_id='" & ViewState("divcode") & "'"
            Dim strBuild As String = Trim(BuildCondition)
            If strBuild <> "" Then
                strSqlQry = strSqlQry & " WHERE " & strBuild & " ORDER BY " & strorderby & " " & strsortorder
            Else
                strSqlQry = strSqlQry & "  ORDER BY " & strorderby & " " & strsortorder
            End If

            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)
            gv_SearchResult.DataSource = myDS


            If myDS.Tables(0).Rows.Count > 0 Then
                gv_SearchResult.DataBind()
                lblMessg.Visible = False
            Else
                gv_SearchResult.PageIndex = 0
                gv_SearchResult.DataBind()
                lblMessg.Visible = True
                lblMessg.Text = "Records not found, Please redefine search criteria."
            End If

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("JournalSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            'clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
            'clsDBConnect.dbConnectionClose(SqlConn)                          'Close connection
        End Try

    End Sub
#End Region

#Region "Private Function BuildCondition1() As String"
    Private Function BuildCondition1() As String
        strWhereCond = ""

        If txtFromDate.Text.Trim <> "" And txtTodate.Text.Trim <> "" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = "( (convert(varchar(10),journal_master.journal_date,111) between convert(varchar(10), '" & Format(CType(txtFromDate.Text, Date), "yyyy/MM/dd") & "',111) " _
                & "  and  convert(varchar(10), '" & Format(CType(txtTodate.Text, Date), "yyyy/MM/dd") & "',111) ) " _
                & " or (convert(varchar(10),journal_master.journal_date,111) < convert(varchar(10), '" & Format(CType(txtFromDate.Text, Date), "yyyy/MM/dd") & "',111) " _
                & "  and convert(varchar(10),journal_master.journal_date,111) > convert(varchar(10), '" & Format(CType(txtTodate.Text, Date), "yyyy/MM/dd") & "',111)) )"
            Else
                strWhereCond = strWhereCond & " and ( (convert(varchar(10),journal_master.journal_date,111) between convert(varchar(10), '" & Format(CType(txtFromDate.Text, Date), "yyyy/MM/dd") & "',111) " _
                & "  and  convert(varchar(10), '" & Format(CType(txtTodate.Text, Date), "yyyy/MM/dd") & "',111) ) " _
                & " or (convert(varchar(10),journal_master.journal_date,111) < convert(varchar(10), '" & Format(CType(txtFromDate.Text, Date), "yyyy/MM/dd") & "',111) " _
                & "  and convert(varchar(10),journal_master.journal_date,111) > convert(varchar(10), '" & Format(CType(txtTodate.Text, Date), "yyyy/MM/dd") & "',111)) )"

            End If
        End If

        BuildCondition1 = strWhereCond
    End Function
#End Region


#Region "Private Function BuildCondition() As String"
    Private Function BuildCondition() As String
        strWhereCond = ""
        If txtTranId.Text.Trim <> "" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = " upper(journal_master.tran_id) = '" & Trim(txtTranId.Text.Trim) & "'"
            Else
                strWhereCond = strWhereCond & " AND upper(journal_master.tran_id) = '" & Trim(txtTranId.Text.Trim) & "'"
            End If
        End If

        If txtdesc.Text.Trim <> "" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = " journal_master.journal_narration like '%" & Trim(txtdesc.Text.Trim) & "%'"
            Else
                strWhereCond = strWhereCond & " AND  journal_master.journal_narration like '%" & Trim(txtdesc.Text.Trim) & "%'"
            End If
        End If


        If txtFromDate.Text.Trim <> "" And txtTodate.Text.Trim <> "" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = "( (convert(varchar(10),journal_master.journal_date,111) between convert(varchar(10), '" & Format(CType(txtFromDate.Text, Date), "yyyy/MM/dd") & "',111) " _
                & "  and  convert(varchar(10), '" & Format(CType(txtTodate.Text, Date), "yyyy/MM/dd") & "',111) ) " _
                & " or (convert(varchar(10),journal_master.journal_date,111) < convert(varchar(10), '" & Format(CType(txtFromDate.Text, Date), "yyyy/MM/dd") & "',111) " _
                & "  and convert(varchar(10),journal_master.journal_date,111) > convert(varchar(10), '" & Format(CType(txtTodate.Text, Date), "yyyy/MM/dd") & "',111)) )"
            Else
                strWhereCond = strWhereCond & " and ( (convert(varchar(10),journal_master.journal_date,111) between convert(varchar(10), '" & Format(CType(txtFromDate.Text, Date), "yyyy/MM/dd") & "',111) " _
                & "  and  convert(varchar(10), '" & Format(CType(txtTodate.Text, Date), "yyyy/MM/dd") & "',111) ) " _
                & " or (convert(varchar(10),journal_master.journal_date,111) < convert(varchar(10), '" & Format(CType(txtFromDate.Text, Date), "yyyy/MM/dd") & "',111) " _
                & "  and convert(varchar(10),journal_master.journal_date,111) > convert(varchar(10), '" & Format(CType(txtTodate.Text, Date), "yyyy/MM/dd") & "',111)) )"

            End If
        End If
        If ddlStatus.Value <> "[Select]" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = " isnull(post_state,'')  = '" & ddlStatus.Value & "'"
            Else
                strWhereCond = strWhereCond & " AND isnull(post_state,'')  = '" & ddlStatus.Value & "'"
            End If
        End If
        BuildCondition = strWhereCond
    End Function
#End Region


    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Dim appid As String = CType(Request.QueryString("appid"), String)
        hdnappid.Value = CType(Request.QueryString("appid"), String) 'Tanvir 10062024
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

#Region "Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
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


        If Page.IsPostBack = False Then
            Try
                SetFocus(txtTranId)

                Dim frmdate As String = ""
                Dim todate As String = ""

                btnPrint_new.Attributes.Add("onclick", "return FormValidation('')")

                RowsPerPageCUS.SelectedValue = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id='2006'")

                If CType(Session("GlobalUserName"), String) = Nothing Or CType(Session("Userpwd"), String) = Nothing Then
                    Response.Redirect("~/Login.aspx", False)
                    Exit Sub
                Else
                    objUser.CheckUserRight(Session("dbconnectionName"), CType(Session("GlobalUserName"), String), CType(Session("Userpwd"), String), _
                                                       CType(strappname, String), "AccountsModule\JournalSearch.aspx?appid=" + strappid, btnAddNew, btnPrint_new, _
                                                       btnPrint_new, gv_SearchResult, GridCol.Edit, GridCol.Delete, GridCol.View, 0, GridCol.Copy)

                End If
                pnlSearch.Visible = False
                Session.Add("jExpression", "tran_id")
                Session.Add("jdirection", SortDirection.Ascending)

                'txtFromDate.Text = ""
                'txtTodate.Text = ""

                ''Record list will be according to the Changing the year  
                'If Not (Session("changeyear") Is Nothing) Then
                '    frmdate = CDate(Session("changeyear") + "/01" + "/01")

                '    If Session("changeyear") = Year(Now).ToString Then
                '        todate = CDate(Session("changeyear") + "/" + Month(Now).ToString + "/" + Day(Now).ToString)
                '    Else
                '        todate = CDate(Session("changeyear") + "/" + "12" + "/" + "31")
                '    End If

                '    txtFromDate.Text = Format(CType(frmdate, Date), "dd/MM/yyy")
                '    txtTodate.Text = Format(CType(todate, Date), "dd/MM/yyy")

                'Else
                '    txtFromDate.Text = ""
                '    txtTodate.Text = ""
                'End If

                '' Sorour asked to  hide 02/02/16
                gv_SearchResult.Columns(14).Visible = False



                Session("sDtDynamic") = Nothing


                Dim dtDynamic = New DataTable()
                Dim dcCode = New DataColumn("Code", GetType(String))
                Dim dcValue = New DataColumn("Value", GetType(String))
                Dim dcCodeAndValue = New DataColumn("CodeAndValue", GetType(String))
                dtDynamic.Columns.Add(dcCode)
                dtDynamic.Columns.Add(dcValue)
                dtDynamic.Columns.Add(dcCodeAndValue)
                Session("sDtDynamic") = dtDynamic
                FillGridNew()
                'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"),ddlType, "acc_type_name", "acc_type_name", "select acc_type_name,acc_type_mode   from  acc_type_master where acc_type_mode<>'G' order by acc_type_name", True)


            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objUtils.WritErrorLog("JournalSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            End Try



        End If

        FillGridNew()
        btnClear.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to clear search criteria?')==false)return false;")
        ClientScript.GetPostBackEventReference(Me, String.Empty)
        If (Me.IsPostBack And Me.Request("__EVENTTARGET") = "JournalWindowPostBack") Then
            btnSearch_Click(sender, e)
        End If
    End Sub
#End Region
#Region "Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click"
    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click

        Dim frmdate As String = ""
        Dim todate As String = ""

        ''Record list will be according to the Changing the year  
        'If Not (Session("changeyear") Is Nothing) Then
        '    frmdate = CDate(Session("changeyear") + "/01" + "/01")

        '    If Session("changeyear") = Year(Now).ToString Then
        '        todate = CDate(Session("changeyear") + "/" + Month(Now).ToString + "/" + Day(Now).ToString)
        '    Else
        '        todate = CDate(Session("changeyear") + "/" + "12" + "/" + "31")
        '    End If

        '    txtFromDate.Text = Format(CType(frmdate, Date), "dd/MM/yyy")
        '    txtTodate.Text = Format(CType(todate, Date), "dd/MM/yyy")

        'Else
        '    txtFromDate.Text = ""
        '    txtTodate.Text = ""
        'End If



        ''Record list will be according to the Changing the year  
        'If Session("changeyear") <> Year(CType(txtFromDate.Text, Date)).ToString Then
        '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Cannot list for the Different year Change year '  );", True)
        '    Exit Sub
        'End If

        'If Session("changeyear") <> Year(CType(txtTodate.Text, Date)).ToString Then
        '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Cannot list for the Different year Change year '  );", True)
        '    Exit Sub
        'End If


        FillGrid("tran_id", "DESC")
    End Sub
#End Region
#Region "Protected Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClear.Click"
    Protected Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClear.Click
        txtTranId.Text = ""
        ''   txtFromDate.Text = ""
        ''   txtTodate.Text = ""
        ddlStatus.Value = "[Select]"
        FillGrid("tran_id", "DESC")
    End Sub
#End Region
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

                Dim mindate1 As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select convert(varchar(10),journal_date,111) from journal_master  where div_id='" & ViewState("divcode") & "' and tran_id='" + lblId.Text + "'")

                Dim sealdate As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select sealdate from  sealing_master where div_code='" & ViewState("divcode") & "' ")

                If e.CommandName = "EditRow" Then
                    ' Session.Add("State", "Edit")
                    ' Session.Add("RefCode", CType(lblId.Text.Trim, String))
                    'Response.Redirect("Journal.aspx", False)
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

                    actionstr = "Edit"
                    'strpop = "window.open('Journal.aspx?State=" + CType(actionstr, String) + "&RefCode=" + CType(lblId.Text.Trim, String) + "','Journal','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                    strpop = "window.open('Journal.aspx?State=" + CType(actionstr, String) + "&divid=" & ViewState("divcode") & "&RefCode=" + CType(lblId.Text.Trim, String) + "','Journal');"
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
                ElseIf e.CommandName = "DeleteRow" Then
                    'Session.Add("State", "Delete")
                    'Session.Add("RefCode", CType(lblId.Text.Trim, String))
                    'Response.Redirect("Journal.aspx", False)
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

                    actionstr = "Delete"
                    'strpop = "window.open('Journal.aspx?State=" + CType(actionstr, String) + "&RefCode=" + CType(lblId.Text.Trim, String) + "','Journal','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                    strpop = "window.open('Journal.aspx?State=" + CType(actionstr, String) + "&divid=" & ViewState("divcode") & "&RefCode=" + CType(lblId.Text.Trim, String) + "','Journal');"
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
                ElseIf e.CommandName = "View" Then
                    'Session.Add("State", "View")
                    'Session.Add("RefCode", CType(lblId.Text.Trim, String))
                    'Response.Redirect("Journal.aspx", False)
                    actionstr = "View"
                    'strpop = "window.open('Journal.aspx?State=" + CType(actionstr, String) + "&RefCode=" + CType(lblId.Text.Trim, String) + "','Journal','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                    strpop = "window.open('Journal.aspx?State=" + CType(actionstr, String) + "&divid=" & ViewState("divcode") & "&RefCode=" + CType(lblId.Text.Trim, String) + "','Journal');"
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
                ElseIf e.CommandName = "Copy" Then
                    'Session.Add("State", "Copy")
                    'Session.Add("RefCode", CType(lblId.Text.Trim, String))
                    'Response.Redirect("Journal.aspx", False)
                    actionstr = "Copy"
                    'strpop = "window.open('Journal.aspx?State=" + CType(actionstr, String) + "&RefCode=" + CType(lblId.Text.Trim, String) + "','Journal','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                    strpop = "window.open('Journal.aspx?State=" + CType(actionstr, String) + "&divid=" & ViewState("divcode") & "&RefCode=" + CType(lblId.Text.Trim, String) + "','Journal');"
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
                    'ElseIf e.CommandName = "ViewLog" Then
                    '    'Session.Add("State", "Copy")
                    '    'Session.Add("RefCode", CType(lblId.Text.Trim, String))
                    '    'Response.Redirect("Journal.aspx", False)
                    '    actionstr = "ViewLog"
                    '    strpop = "window.open('Accnt_trn_amendlog.aspx?State=" + CType(actionstr, String) + "&RefCode=" + CType(lblId.Text.Trim, String) + "','Journal','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                    '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)


                ElseIf e.CommandName = "ViewLog" Then

                    actionstr = "ViewLog"
                    ' strpop = "window.open('ViewLog.aspx?State=" + CType(actionstr, String) + "&RefCode=" + CType(lblId.Text.Trim, String) + "','Journal','width=500,height=300 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                    strpop = "window.open('ViewLog.aspx?State=" + CType(actionstr, String) + "&divid=" & ViewState("divcode") & "&RefCode=" + CType(lblId.Text.Trim, String) + "&trantype=" + CType(lblTranType.Text.Trim, String) + "','Journal');"
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
                End If




            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objUtils.WritErrorLog("JournalSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            End Try
        End If
    End Sub
#End Region

    Public Function Validateseal(ByVal tranid) As Boolean
        Try
            Dim ds As DataSet = objUtils.ExecuteQuerySqlnew(Session("dbconnectionName"), "select * from  journal_master where div_id='" & ViewState("divcode") & "' and tran_id='" + tranid + "' ")
            If ds.Tables.Count > 0 Then
                If ds.Tables(0).Rows.Count > 0 Then
                    If IsDBNull(ds.Tables(0).Rows(0)("tran_state")) = False Then
                        If ds.Tables(0).Rows(0)("tran_state") = "S" Then
                            Return True
                        Else
                            Return False
                        End If
                    End If
                End If
            End If
        Catch ex As Exception
            Validateseal = False
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("JournalSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Function
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
        FillGrid(Session("jExpression"), "")

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

    '#Region "Protected Sub btnExportToExcel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExportToExcel.Click"
    '    Protected Sub btnExportToExcel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExportToExcel.Click
    '        Dim DS As New DataSet
    '        Dim DA As SqlDataAdapter
    '        Dim con As SqlConnection
    '        Dim objcon As New clsDBConnect
    '        Try
    '            If gv_SearchResult.Rows.Count <> 0 Then
    '                strSqlQry = "select  tran_id as	[Document No], tran_type as	[Doc Type]," & _
    '                             " case isnull(post_state,'') when 'P' then 'Posted' when 'U' then 'UnPosted' end  as [Status] ," & _
    '                            "   convert(varchar(10),journal_date,103) as [Journal Date]," & _
    '                            " convert(varchar(10),journal_tran_date,103) as	[Posted Date], journal_narration as	[Narration],adddate   as	[Date Created], adduser as	[User Created]," & _
    '                            " moddate as	[Date Modified], moduser	as [User Modified]  from    journal_master where div_id='" & ViewState("divcode") & "'"
    '                If Trim(BuildCondition) <> "" Then
    '                    strSqlQry = strSqlQry & " WHERE " & BuildCondition() & " ORDER BY tran_id "
    '                Else
    '                    strSqlQry = strSqlQry & " ORDER BY tran_id "
    '                End If
    '                con = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
    '                DA = New SqlDataAdapter(strSqlQry, con)
    '                DA.Fill(DS, "Journal")
    '                objUtils.ExportToExcel(DS, Response)
    '                con.Close()
    '            Else
    '                objUtils.MessageBox("Sorry , No data availabe for export to excel.", Page)
    '            End If
    '        Catch ex As Exception
    '        End Try
    '    End Sub
    '#End Region

    Protected Sub btnhelp_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=JournalSearch','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub

    'Protected Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrint.Click
    '    Try
    '        'Session.Add("CurrencyCode", txtgroupid.Text.Trim)
    '        'Session.Add("CurrencyName", txtmealname.Text.Trim)
    '        'Response.Redirect("rptCurrencies.aspx", False)


    '        Dim poststate As String

    '        poststate = IIf(ddlStatus.Value = "[Select]", "", ddlStatus.Value)
    '        Dim strReportTitle As String = ""
    '        Dim strSelectionFormula As String = ""
    '        'Session("ColReportParams") = Nothing
    '        'Session.Add("Pageame", "Journal")
    '        'Session.Add("BackPageName", "JournalSearch.aspx")
    '        Dim strpop As String = ""
    '        'strpop = "window.open('rptReportNew.aspx?Pageame=Journal&BackPageName=JournalSearch.aspx&JVTranId=" & txtTranId.Text.Trim & "&Fromdate=" & strfromdate & "&Todate=" & strtodate & "&poststate=" & poststate & "','RepJV','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
    '        strpop = "window.open('rptReportNew.aspx?divid=" & ViewState("divcode") & "&Pageame=Journal&BackPageName=JournalSearch.aspx&JVTranId=" & txtTranId.Text.Trim & "&poststate=" & poststate & "','RepJV','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
    '        'strpop = "window.open('rptReportNew.aspx?Pageame=Journal&BackPageName=JournalSearch.aspx&divid=" & ViewState("divcode") & "','Journalreport');"
    '        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)

    '    Catch ex As Exception
    '        objUtils.WritErrorLog("JournalSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
    '    End Try

    'End Sub

    'Protected Function validaterpt() As Boolean
    '    If txtFromDate.Text = "" Or txtTodate.Text = "" Then

    '        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Fill the From and To date');", True)

    '        Return False
    '    End If
    '    Return True


    'End Function

    Protected Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit



    End Sub

    Protected Sub btnvsprocess_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnvsprocess.Click

        Try
            FilterGrid()
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("JournalSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try



    End Sub

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
                    If dtt.Rows(i)("Code").ToString = "REFERENCE" Then
                        If strrefValue <> "" Then
                            strrefValue = strrefValue + ",'" + dtt.Rows(i)("Value").ToString + "'"
                        Else
                            strrefValue = "'" + dtt.Rows(i)("Value").ToString + "'"
                        End If
                    End If
                    If dtt.Rows(i)("Code").ToString = "STATUS" Then
                        If dtt.Rows(i)("Value").ToString = "POSTED" Then
                            If strStatusValue <> "" Then
                                strStatusValue = strStatusValue + ",'" + "P" + "'"
                            Else
                                strStatusValue = "'" + "P".ToString + "'"
                            End If
                        Else
                            If strStatusValue <> "" Then
                                strStatusValue = strStatusValue + ",'" + "U" + "'"
                            Else
                                strStatusValue = "'" + "U".ToString + "'"
                            End If
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
     
            strSqlQry = "select tran_id,tran_type,  " & _
                        " case isnull(post_state,'') when 'P' then 'Posted' when 'U' then 'UnPosted' end  as post_state ," & _
                        " convert(varchar(10),journal_date ,103) as journal_date ," & _
                        " convert(varchar(10),journal_tran_date,103) as journal_tran_date,journal_mrv,journal_salesperson_code, " & _
                        " journal_narration, journal_tran_state, adddate, adduser, moddate, moduser, div_id, basedebit, basecredit " & _
                        " from journal_master where div_id='" & ViewState("divcode") & "'"
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
            objUtils.WritErrorLog("JournalSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            'clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
            'clsDBConnect.dbConnectionClose(SqlConn)                          'Close connection
        End Try

    End Sub

    
    Private Function BuildConditionNew(ByVal strDocValue As String, ByVal strDescValue As String, ByVal strStatusValue As String, ByVal strTextValue As String, ByVal strrefValue As String) As String
        strWhereCond = ""
        If strDocValue.Trim <> "" Then
            If Trim(strWhereCond) = "" Then

                strWhereCond = "upper(journal_master.tran_id) IN (" & Trim(strDocValue.Trim.ToUpper) & ")"
            Else
                strWhereCond = strWhereCond & " AND  upper(journal_master.tran_id) IN (" & Trim(strDocValue.Trim.ToUpper) & ")"
            End If

        End If
        If strrefValue.Trim <> "" Then
            If Trim(strWhereCond) = "" Then

                strWhereCond = "upper(journal_master.journal_mrv) IN (" & Trim(strrefValue.Trim.ToUpper) & ")"
            Else
                strWhereCond = strWhereCond & " AND  upper(journal_master.journal_mrv) IN (" & Trim(strrefValue.Trim.ToUpper) & ")"
            End If

        End If

        If strDescValue <> "" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = "upper(journal_master.journal_narration)  IN (" & Trim(strDescValue.Trim.ToUpper) & ")"
            Else
                strWhereCond = strWhereCond & " AND  upper(journal_master.journal_narration) IN (" & Trim(strDescValue.Trim.ToUpper) & ")"
            End If
        End If

        If strStatusValue <> "" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = "upper(journal_master.post_state)  IN (" & Trim(strStatusValue.Trim.ToUpper) & ")"
            Else
                strWhereCond = strWhereCond & " AND  upper(journal_master.post_state) IN (" & Trim(strStatusValue.Trim.ToUpper) & ")"
            End If
        End If


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

                            strWhereCond1 = "upper(journal_master.tran_id) LIKE '%" & Trim(strPOSTED.Trim.ToUpper) & "%' or  upper(journal_narration) LIKE '%" & Trim(strPOSTED.Trim.ToUpper) & "%'  or upper(journal_master.post_state) LIKE '%" & Trim(strValue2.Trim.ToUpper) & "%'"
                        Else
                            strWhereCond1 = strWhereCond1 & " OR  upper(journal_master.tran_id) LIKE '%" & Trim(strPOSTED.Trim.ToUpper) & "%' or  upper(journal_narration) LIKE '%" & Trim(strPOSTED.Trim.ToUpper) & "%'  or upper(journal_master.post_state) LIKE '%" & Trim(strValue2.Trim.ToUpper) & "%'"
                        End If

                    ElseIf strValue = "UNPOSTED" Then
                        strValue2 = "U"

                        If Trim(strWhereCond1) = "" Then

                            strWhereCond1 = "upper(journal_master.tran_id) LIKE '%" & Trim(strUNPOSTED.Trim.ToUpper) & "%' or  upper(journal_narration) LIKE '%" & Trim(strUNPOSTED.Trim.ToUpper) & "%'  or upper(journal_master.post_state) LIKE '%" & Trim(strValue2.Trim.ToUpper) & "%'"
                        Else
                            strWhereCond1 = strWhereCond1 & " OR  upper(journal_master.tran_id) LIKE '%" & Trim(strUNPOSTED.Trim.ToUpper) & "%' or  upper(journal_narration) LIKE '%" & Trim(strUNPOSTED.Trim.ToUpper) & "%'  or upper(journal_master.post_state) LIKE '%" & Trim(strValue2.Trim.ToUpper) & "%'"
                        End If
                    Else

                        If Trim(strWhereCond1) = "" Then

                            strWhereCond1 = " upper(journal_master.tran_id) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' or  upper(journal_narration) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' or  upper(journal_master.post_state) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' or  upper(journal_master.journal_mrv) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%'"
                        Else
                            strWhereCond1 = strWhereCond1 & " OR  upper(journal_master.tran_id) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' or  upper(journal_narration) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' or  upper(journal_master.post_state) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' or  upper(journal_master.journal_mrv) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%'"
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

                    strWhereCond = " (CONVERT(datetime, convert(varchar(10),journal_master.adddate,103),103)  between CONVERT(datetime, '" + txtFromDate.Text + "',103) and CONVERT(datetime,  '" + txtToDate.Text + "',103)) "
                Else
                    strWhereCond = strWhereCond & "  and (CONVERT(datetime, convert(varchar(10),journal_master.adddate,103),103)  between CONVERT(datetime, '" + txtFromDate.Text + "',103) and CONVERT(datetime,  '" + txtToDate.Text + "',103)) "
                End If
            ElseIf ddlOrder.SelectedValue = "M" Then
                If Trim(strWhereCond) = "" Then

                    strWhereCond = " (CONVERT(datetime, convert(varchar(10),journal_master.moddate,103),103)  between CONVERT(datetime, '" + txtFromDate.Text + "',103) and CONVERT(datetime,  '" + txtToDate.Text + "',103)) "
                Else
                    strWhereCond = strWhereCond & "  and (CONVERT(datetime, convert(varchar(10),journal_master.moddate,103),103)  between CONVERT(datetime, '" + txtFromDate.Text + "',103) and CONVERT(datetime,  '" + txtToDate.Text + "',103)) "
                End If
            ElseIf ddlOrder.SelectedValue = "T" Then
                If Trim(strWhereCond) = "" Then

                    strWhereCond = " (CONVERT(datetime, convert(varchar(10),journal_master.journal_date,103),103)  between CONVERT(datetime, '" + txtFromDate.Text + "',103) and CONVERT(datetime,  '" + txtToDate.Text + "',103)) "
                Else
                    strWhereCond = strWhereCond & "  and (CONVERT(datetime, convert(varchar(10),journal_master.journal_date,103),103)  between CONVERT(datetime, '" + txtFromDate.Text + "',103) and CONVERT(datetime,  '" + txtToDate.Text + "',103)) "
                End If
            End If



        End If



        BuildConditionNew = strWhereCond
    End Function

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





    Protected Sub btnClearDate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClearDate.Click
        txtFromDate.Text = ""
        txtToDate.Text = ""
        FillGridNew()
    End Sub

    Public Function getRowpage() As String
        Dim rowpagecs As String
        If RowsPerPageCUS.SelectedValue = "20" Then
            rowpagecs = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id='2006'")
        Else
            rowpagecs = RowsPerPageCUS.SelectedValue

        End If
        Return rowpagecs
    End Function

    Protected Sub btnFilter_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnFilter.Click
        Try
            FillGridNew()
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("JournalSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub


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

    Protected Sub RowsPerPageCUS_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RowsPerPageCUS.SelectedIndexChanged
        FillGridNew()
    End Sub
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
    Private Sub detailedReport()
        Dim FolderPath As String = "..\ExcelTemplates\"
        Dim FileName As String = "accountsTransactionDetailed_template.xlsx"
        Dim FilePath As String = Server.MapPath(FolderPath + FileName)
        Dim RandomCls As New Random()
        Dim RandomNo As String = RandomCls.Next(100000, 9999999).ToString
        Dim rptcompanyname, basecurrency As String

        rptcompanyname = CType(objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select division_master_des from division_master where division_master_code='" & ViewState("divcode") & "'"), String)
        basecurrency = CType(objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id=457"), String)

        Dim FileNameNew As String
        FileNameNew = "journalDetailed_" & Now.Year & Now.Month & Now.Day & Now.Hour & Now.Minute & Now.Second & Now.Millisecond & ".xlsx"

        document = New XLWorkbook(FilePath)
        Dim ws As IXLWorksheet = document.Worksheet("register")
        ws.Style.Font.FontName = "arial"

        'Dim SheetTemplate As IXLWorksheet = New XLWorkbook(FilePath).Worksheet("Offer Template")
        'SheetTemplate.Style.Font.FontName = "Trebuchet MS"
        'Dim PartyName As String = ""
        'Dim CatName As String = ""
        'Dim SectorCityName As String = ""

        Dim LastLine As Integer
        ws.Cell(1, 1).Value = rptcompanyname



       


        'create header
        ws.Cell(2, 1).Value = "Journal Register:Detailed Report"
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
        ws.Column(14).Delete()
        ws.Column(14).Delete()
        Dim sql As String

        sql = FillGridNew_report()
        ws.Cell(4, 1).Value = reportfilter

        Dim myDS As New DataSet

        SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
        myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
        myDataAdapter.Fill(myDS)



        Dim dt As New DataTable

        Dim dt_row As New DataTable

        dt = myDS.Tables(0)

    

        'dt_sum = ds.Tables(1)
        LastLine = 7
        Dim total_basedebit, total_basecredit As Double
     
        total_basedebit = 0
        total_basecredit = 0
        If dt.Rows.Count > 0 Then
            For i As Integer = 0 To dt.Rows.Count - 1


                dt_row = dt.Clone()
                dt_row.ImportRow(dt.Rows(i))




                Dim RateSheet As IXLRange
                RateSheet = ws.Cell(LastLine, 1).InsertTable(dt_row.AsEnumerable).SetShowHeaderRow(False).AsRange()
                ws.Rows(LastLine).Clear()
                ws.Rows(LastLine).Delete()
                RateSheet.Style.Font.FontName = "arial"
                RateSheet.Columns("8:12").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right
                Dim style1 As IXLBorder = RateSheet.Cells.Style.Border
                style1.BottomBorder = XLBorderStyleValues.Thin
                style1.LeftBorder = XLBorderStyleValues.Thin
                RateSheet.Style.Border.OutsideBorder = XLBorderStyleValues.Thin
                RateSheet.Style.Font.Bold = True

                LastLine = LastLine + 1


                Dim dt_detail_ds As New DataSet
                Dim dt_detail As New DataTable
                Dim sqlst As String
                Dim id As String = dt.Rows(i).Item(0).ToString



                sqlst = "select '','','',Case journal_acc_type when 'S' then'Supplier'  when 'C' then  'Customer'  when 'G' then 'General' when 'A' then 'Agent' end,view_account.des,bookingno,journal_currency_id,journal_currency_rate,journal_debit,journal_credit,basedebit,basecredit,journal_narration" & _
               " from journal_detail left join  view_account on journal_detail.journal_acc_code=view_account.code and journal_detail.journal_acc_type=type and journal_detail.div_id= view_account.div_code" & _
                "  where journal_detail.div_id='" & ViewState("divcode") & "' and journal_detail.tran_id='" & id & "'"
                dt_detail_ds = objUtils.GetDataFromDatasetnew(Session("dbconnectionName"), sqlst)
                dt_detail = dt_detail_ds.Tables(0)

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



            'Dim RateSheet_summ As IXLRange
            'RateSheet_summ = ws.Range(LastLine, 1, LastLine, 15)

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
            'ws.Cell(LastLine, 14).Value = total
            'ws.Cell(LastLine, 14).Style.Font.Bold = True
            'ws.Cell(LastLine, 14).Style.Font.FontSize = 14

            'RateSheet_summ.Style.Font.FontName = "arial"
            'RateSheet_summ.Columns("9:14").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right
            'Dim style2 As IXLBorder = RateSheet_summ.Cells.Style.Border
            'style2.BottomBorder = XLBorderStyleValues.Thin
            'style2.LeftBorder = XLBorderStyleValues.Thin
            'RateSheet_summ.Style.Border.OutsideBorder = XLBorderStyleValues.Thin

            '    RateSheet_summ.Style.Font.Bold = True




        End If










        'ws.Protect(RandomNo)
        'ws.Protection.FormatColumns = True
        'ws.Protection.FormatRows = True

        Using MyMemoryStream As New MemoryStream()
            document.SaveAs(MyMemoryStream)
            document.Dispose()
            Response.Clear()
            Response.Buffer = True
            Response.AddHeader("content-disposition", "attachment;filename=" + FileNameNew)
            Response.AddHeader("Content-Length", MyMemoryStream.Length.ToString())
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
            'Response.BinaryWrite(MyMemoryStream.GetBuffer())
            MyMemoryStream.WriteTo(Response.OutputStream)
            Response.Cookies.Add(New HttpCookie("Downloaded", "True"))
            Response.Flush()
            HttpContext.Current.ApplicationInstance.CompleteRequest()
        End Using
    End Sub
    Protected Sub btnPrint_new_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrint_new.Click
        'Try
        '    If ddlrpt.SelectedValue = "Detailed" Then
        detailedReport()

        '    Else



        '        Dim FolderPath As String = "..\ExcelTemplates\"
        '        Dim FileName As String = "accountsTransaction_template.xlsx"
        '        Dim FilePath As String = Server.MapPath(FolderPath + FileName)
        '        Dim RandomCls As New Random()
        '        Dim RandomNo As String = RandomCls.Next(100000, 9999999).ToString
        '        Dim rptcompanyname, basecurrency As String

        '        rptcompanyname = CType(objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select division_master_des from division_master where division_master_code='" & ViewState("divcode") & "'"), String)
        '        basecurrency = CType(objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id=457"), String)
        '        Dim FileNameNew As String

        '        FileNameNew = "journalBrief_" & Now.Year & Now.Month & Now.Day & Now.Hour & Now.Minute & Now.Second & Now.Millisecond & ".xlsx"





        '        document = New XLWorkbook(FilePath)
        '        Dim ws As IXLWorksheet = document.Worksheet("register")
        '        ws.Style.Font.FontName = "arial"

        '        'Dim SheetTemplate As IXLWorksheet = New XLWorkbook(FilePath).Worksheet("Offer Template")
        '        'SheetTemplate.Style.Font.FontName = "Trebuchet MS"
        '        'Dim PartyName As String = ""
        '        'Dim CatName As String = ""
        '        'Dim SectorCityName As String = ""

        '        Dim LastLine As Integer
        '        ws.Cell(1, 1).Value = rptcompanyname
        '        ws.Column("7").Delete()

        '        ws.Column("7").Delete()


        '        ws.Column("7").Delete()

        '        ws.Column("7").Delete()



        '        ws.Column("6").Width = 57





        '        ws.Cell(2, 1).Value = "Journal:Brief Report"


        '        'create header

        '        ws.Cell(6, 2).Value = "Date"
        '        ws.Cell(6, 1).Value = "JV No"
        '        ws.Cell(6, 3).Value = "Reference"
        '        ws.Cell(6, 4).Value = "Debit"
        '        ws.Cell(6, 4).Value = ws.Cell(6, 4).Value & " (" & basecurrency & ")"
        '        ws.Cell(6, 5).Value = "Credit"
        '        ws.Cell(6, 5).Value = ws.Cell(6, 5).Value & " (" & basecurrency & ")"
        '        ws.Cell(6, 6).Value = "Narration"


        '        Dim sql As String

        '        sql = FillGridNew_report()
        '        ws.Cell(4, 1).Value = reportfilter

        '        Dim myDS As New DataSet

        '        SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
        '        myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
        '        myDataAdapter.Fill(myDS)



        '        Dim dt As New DataTable



        '        dt = myDS.Tables(0)

        '        Dim RateSheet As IXLRange
        '        RateSheet = ws.Cell(7, 1).InsertTable(dt.AsEnumerable).SetShowHeaderRow(False).AsRange()
        '        ws.Rows(7).Clear()
        '        ws.Rows(7).Delete()
        '        LastLine = 7 + dt.Rows.Count

        '        If dt.Rows.Count > 1 Then
        '            ws.Range(LastLine, 1, LastLine, 6).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin)
        '            ws.Range(LastLine, 1, LastLine, 6).Style.Border.SetLeftBorder(XLBorderStyleValues.Thin)
        '            ws.Range(LastLine, 1, LastLine, 6).Style.Font.Bold = True
        '            ws.Cell(LastLine, 3).Value = "Total"
        '            ws.Cell(LastLine, 3).Style.Font.Bold = True
        '            RateSheet.Columns("4:5").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right

        '            RateSheet.Columns("4:5").SetDataType(XLCellValues.Number)
        '            ws.Cell(LastLine, 4).SetFormulaR1C1("=SUM(d7:d" & LastLine - 1 & ")")
        '            ws.Cell(LastLine, 5).SetFormulaR1C1("=SUM(e7:e" & LastLine - 1 & ")")
        '            ws.Cell(LastLine, 4).Style.Font.FontName = "arial"
        '            ws.Cell(LastLine, 5).Style.Font.FontName = "arial"
        '        End If



        '        RateSheet.Style.Font.FontName = "arial"

        '        'RateSheet.Columns("9:13").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right
        '        Dim style1 As IXLBorder = RateSheet.Cells.Style.Border
        '        style1.BottomBorder = XLBorderStyleValues.Thin
        '        style1.LeftBorder = XLBorderStyleValues.Thin

        '        RateSheet.Style.Border.OutsideBorder = XLBorderStyleValues.Medium










        '        'ws.Protect(RandomNo)
        '        'ws.Protection.FormatColumns = True
        '        'ws.Protection.FormatRows = True

        '        Using MyMemoryStream As New MemoryStream()
        '            document.SaveAs(MyMemoryStream)
        '            document.Dispose()
        '            Response.Clear()
        '            Response.Buffer = True
        '            Response.AddHeader("content-disposition", "attachment;filename=" + FileNameNew)
        '            Response.AddHeader("Content-Length", MyMemoryStream.Length.ToString())
        '            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
        '            'Response.BinaryWrite(MyMemoryStream.GetBuffer())
        '            MyMemoryStream.WriteTo(Response.OutputStream)
        '            Response.Cookies.Add(New HttpCookie("Downloaded", "True"))
        '            Response.Flush()
        '            HttpContext.Current.ApplicationInstance.CompleteRequest()
        '        End Using
        '    End If
        'Catch ex As Exception

        '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        '    objUtils.WritErrorLog("RptPriceList.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        'End Try

    End Sub



    Private Function FillGridNew_report() As String


        Dim dtt As DataTable
        dtt = Session("sDtDynamic")

        Dim strDocValue As String = ""
        Dim strDescValue As String = ""
        Dim strStatusValue As String = ""
        Dim strTextValue As String = ""
        Dim strrefValue As String = ""
        Dim strBindCondition As String = ""
        Dim reportfilterjournalno, reportfilterText, reportfilternarration, reportfilterreference, reportfilterstatus As String
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
                        reportfilterjournalno = dtt.Rows(i)("Code").ToString + ":" + strDocValue
                        reportfilter = reportfilter & " " & reportfilterjournalno
                    End If
                    If dtt.Rows(i)("Code").ToString = "NARRATION" Then
                        If strDescValue <> "" Then
                            strDescValue = strDescValue + ",'" + dtt.Rows(i)("Value").ToString + "'"
                        Else
                            strDescValue = "'" + dtt.Rows(i)("Value").ToString + "'"
                        End If

                        reportfilternarration = dtt.Rows(i)("Code").ToString + ":" + strDescValue
                        reportfilter = reportfilter & " " & reportfilternarration
                    End If
                    If dtt.Rows(i)("Code").ToString = "REFERENCE" Then
                        If strrefValue <> "" Then
                            strrefValue = strrefValue + ",'" + dtt.Rows(i)("Value").ToString + "'"
                        Else
                            strrefValue = "'" + dtt.Rows(i)("Value").ToString + "'"
                        End If

                        reportfilterreference = dtt.Rows(i)("Code").ToString + ":" + strrefValue
                        reportfilter = reportfilter & " " & reportfilterreference
                    End If
                    If dtt.Rows(i)("Code").ToString = "STATUS" Then
                        If dtt.Rows(i)("Value").ToString = "POSTED" Then
                            If strStatusValue <> "" Then
                                strStatusValue = strStatusValue + ",'" + "P" + "'"
                            Else
                                strStatusValue = "'" + "P".ToString + "'"
                            End If
                        Else
                            If strStatusValue <> "" Then
                                strStatusValue = strStatusValue + ",'" + "U" + "'"
                            Else
                                strStatusValue = "'" + "U".ToString + "'"
                            End If
                        End If

                        reportfilterstatus = dtt.Rows(i)("Code").ToString + ":" + strStatusValue
                        reportfilter = reportfilter & " " & reportfilterstatus
                    End If
                    If dtt.Rows(i)("Code").ToString = "TEXT" Then
                        If strTextValue <> "" Then
                            strTextValue = strTextValue + "," + dtt.Rows(i)("Value").ToString + ""
                        Else
                            strTextValue = "" + dtt.Rows(i)("Value").ToString + ""
                        End If

                        reportfilterText = dtt.Rows(i)("Code").ToString + "like" + strTextValue
                        reportfilter = reportfilter & " " & reportfilterText
                    End If


                Next
            End If

            strBindCondition = BuildConditionNew(strDocValue, strDescValue, strStatusValue, strTextValue, strrefValue)
            'If ddlrpt.SelectedValue = "Detailed" Then
            strSqlQry = "select tran_id,  " & _
                   " convert(varchar(10),journal_date ,103) as journal_date ," & _
                   "journal_mrv,'','','','','','','',basedebit,basecredit ," & _
                   " journal_narration   " & _
             " from journal_master where div_id='" & ViewState("divcode") & "'"
            'Else
            '    strSqlQry = "select tran_id,  " & _
            '           " convert(varchar(10),journal_date ,103) as journal_date ," & _
            '           "journal_mrv,basedebit,basecredit ," & _
            '           " journal_narration   " & _
            '           " from journal_master where div_id='" & ViewState("divcode") & "'"
            'End If

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
            objUtils.WritErrorLog("salesfreeformSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            'clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
            'clsDBConnect.dbConnectionClose(SqlConn)                          'Close connection
        End Try

    End Function

End Class

