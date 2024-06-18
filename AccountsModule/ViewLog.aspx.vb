

#Region "Namespace"
Imports System.Data
Imports System.Data.SqlClient
Imports System.Globalization
#End Region
Partial Class AccountsModule_ViewLog
    Inherits System.Web.UI.Page


#Region "Global Declaration"
    Dim objUtils As New clsUtils
    Dim objUser As New clsUser
    Dim objDateTime As New clsDateTime
    Dim strSqlQry As String
    Dim strWhereCond As String
    Dim sqlConn As SqlConnection
    Dim myCommand As SqlCommand
    Dim mysqlReader As SqlDataReader
    Dim myDataAdapter As SqlDataAdapter
    Dim gvRow As GridViewRow
    Dim intTabindex As Integer = 0



#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not Session("CompanyName") Is Nothing Then
            Me.Page.Title = CType(Session("CompanyName"), String)
        End If

        If Page.IsPostBack = False Then
            Try
                SetFocus(ddlUser)
                If (CType(Session("GlobalUserName"), String) = Nothing Or CType(Session("userpwd"), String) = Nothing) Then
                    Response.Redirect("~Login.aspx")


                End If
                ViewState.Add("viewRefCode", Request.QueryString("State"))
                ViewState.Add("viewlogRefCode", Request.QueryString("RefCode"))
                ViewState.Add("viewlogTrantype", Request.QueryString("trantype"))

                Session.Add("strsortExpression", "tran_id")
                Session.Add("strsortdirection", SortDirection.Ascending)
                'Dim ddluse As DropDownList = CType(FindControl("ddluser"), String)



                If (ViewState("viewRefCode") = "ViewLog") Then
                    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlUser, "UserName", "UserCode", "select UserName, UserCode from UserMaster where active=1 order by UserName", True)


                End If

                'lblDocNo.Text = ViewState("viewlogRefCode")

                show_record(ViewState("viewlogRefCode"))
                fillorderby()
                FillGrid("tran_date")
            Catch ex As Exception

            End Try
        End If






    End Sub




#Region "Public Sub show_record()"
    Public Sub show_record(ByVal RefCode As String)
        Dim mySqlReader As SqlDataReader
        Try

            Dim myDS As New DataSet
            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            myCommand = New SqlCommand("select * from acc_trn_amend_log Where tran_id='" & RefCode & "' and tran_type='" & ViewState("viewlogTrantype") & "'", sqlConn)
            mySqlReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection)
            If mySqlReader.HasRows Then
                If mySqlReader.Read() = True Then
                    If IsDBNull(mySqlReader("tran_id")) = False Then
                        Me.lblDocNo.Text = CType(mySqlReader("tran_id"), String)
                    Else
                        Me.lblDocNo.Text = ""
                    End If
                    If IsDBNull(mySqlReader("tran_type")) = False Then
                        Me.lblTrantype.Text = CType(mySqlReader("tran_type"), String)
                    Else
                        Me.lblTrantype.Text = ""
                    End If
                    If IsDBNull(mySqlReader("tran_date")) = False Then
                        Me.lblTrandate.Text = CType(mySqlReader("tran_date"), String)
                    Else
                        Me.lblTrandate.Text = ""
                    End If


                End If
            End If
        Catch
        End Try

    End Sub
#End Region


#Region "Private Sub ShowFillGrid(ByVal RefCode As String)"
    Private Sub ShowFillGrid(ByVal RefCode As String)
        Dim mySqlReader As SqlDataReader
        Try
            sqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))





        Catch
        End Try



    End Sub
#End Region

    Private Sub fillorderby()
        ddlOrderBy.Items.Clear()
        ddlOrderby.Items.Add("tran_date")
        ddlOrderby.Items.Add("UserName")
        ddlOrderby.SelectedIndex = 0
    End Sub

    Private Sub FillGrid(ByVal strorderby As String, Optional ByVal strsortorder As String = "ASC")
        Dim myDS As New DataSet

        gv_SearchResult.Visible = True
        lblMsg.Visible = False

        If gv_SearchResult.PageIndex < 0 Then
            gv_SearchResult.PageIndex = 0
        End If
        strSqlQry = ""
        Try
            
            strSqlQry = "select UserMaster.userName,acc_trn_amend_log.tran_id, acc_trn_amend_log.moddate,acc_trn_amend_log.modtime, acc_trn_amend_log.description  from UserMaster,acc_trn_amend_log where UserMaster.UserCode=acc_trn_amend_log.moduser " _
                & " and acc_trn_amend_log.tran_id='" & ViewState("viewlogRefCode") & " ' and acc_trn_amend_log.tran_type='" & ViewState("viewlogTrantype") & "'"
            If Trim(BuildCondition) <> "" Then
                'strSqlQry = strSqlQry & " WHERE " & BuildCondition() & " ORDER BY " & strorderby & " " & strsortorder
                strSqlQry = strSqlQry & " and  " & BuildCondition() & " ORDER BY " & strorderby & " " & strsortorder
            Else
                strSqlQry = strSqlQry & " ORDER BY " & strorderby & " " & strsortorder
            End If

            sqlConn = clsDBConnect.dbConnection                             'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, sqlConn)
            myDataAdapter.Fill(myDS)
            gv_SearchResult.DataSource = myDS

            If myDS.Tables(0).Rows.Count > 0 Then
                gv_SearchResult.DataBind()
            Else
                gv_SearchResult.PageIndex = 0
                gv_SearchResult.DataBind()
                lblMsg.Visible = True
                lblMsg.Text = "Records not found, Please redefine search criteria."
            End If

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("unitsSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            'clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
            'clsDBConnect.dbConnectionClose(SqlConn)                          'Close connection
        End Try

    End Sub


    Private Function BuildCondition() As String

        strWhereCond = ""
        
        If ddlUser.Value <> "[Select]" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = " upper(acc_trn_amend_log.moduser) = '" & Trim(ddlUser.Items(ddlUser.SelectedIndex).Text.Trim.ToUpper) & "' and acc_trn_amend_log.tran_id='" & ViewState("viewlogRefCode") & "'"
            Else
                strWhereCond = strWhereCond & " AND upper(cc_trn_amend_log.moduser) = '" & Trim(ddlUser.Items(ddlUser.SelectedIndex).Text.Trim.ToUpper) & "' and acc_trn_amend_log.tran_id='" & ViewState("viewlogRefCode") & "'"
            End If
        End If

       
        BuildCondition = strWhereCond
    End Function







    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        Select Case ddlOrderby.SelectedIndex
            Case 0
                FillGrid("acc_trn_amend_log.tran_id")
            Case 1
                FillGrid("UserMaster.UserName")

        End Select
    End Sub

    Protected Sub btnReport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReport.Click

        Try
            Dim strReportTitle As String = ""
            Dim strSelectionFormula As String = ""
            Session("ColReportParams") = Nothing
           
            Dim strpop As String = ""

            strpop = "window.open('rptReportNew.aspx?Pageame=Logdetails&BackPageName=ViewLog.aspx&Tid=" & lblDocNo.Text.Trim & "&User=" & ddlUser.Value & "&Ttdate=" & lblTrandate.Text.Trim & "','Logdetails','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"

           

            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
        Catch ex As Exception

            objUtils.WritErrorLog("ViewLog.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))


        End Try



    End Sub

   

   

    
    Protected Sub ddlOrderBy_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlOrderBy.SelectedIndexChanged
        Select Case ddlOrderBy.SelectedIndex
            Case 0
                FillGrid("acc_trn_amend_log.tran_id")
            Case 1
                FillGrid("UserMaster.UserName")

        End Select
    End Sub

    Protected Sub gv_SearchResult_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gv_SearchResult.Sorting
        Session.Add("strsortexpression", e.SortExpression)
        SortGridColoumn_click()
    End Sub


#Region "Public Sub SortGridColoumn_click()"
    Public Sub SortGridColoumn_click()
        Dim DataTable As DataTable
        Dim myDS As New DataSet
        FillGrid(Session("strsortexpression"), "")

        myDS = gv_SearchResult.DataSource
        DataTable = myDS.Tables(0)
        If IsDBNull(DataTable) = False Then
            Dim dataView As DataView = DataTable.DefaultView
            Session.Add("strsortdirection", objUtils.SwapSortDirection(Session("strsortdirection")))
            dataView.Sort = Session("strsortexpression") & " " & objUtils.ConvertSortDirectionToSql(Session("strsortdirection"))
            gv_SearchResult.DataSource = dataView
            gv_SearchResult.DataBind()
        End If
    End Sub
#End Region

#Region "Protected Sub gv_SearchResult_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gv_SearchResult.PageIndexChanging"
    Protected Sub gv_SearchResult_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gv_SearchResult.PageIndexChanging
        gv_SearchResult.PageIndex = e.NewPageIndex
        FillGrid("tran_id", "DESC")
    End Sub
#End Region
End Class
