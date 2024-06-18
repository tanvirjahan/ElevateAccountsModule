'----------------------------------------------------------------------------------------------------
'   Module Name    :    OpeningTrailBalanceSearch
'   Developer Name :    Mangesh 
'   Date           :    
'   
'
'----------------------------------------------------------------------------------------------------
#Region "Namespace"
Imports System.Data
Imports System.Data.SqlClient
#End Region
Partial Class OpeningTrailBalanceSearch
    Inherits System.Web.UI.Page

#Region "Global Declarations"
    Dim objUtils As New clsUtils
    Dim objUser As New clsUser
    Dim objDateTime As New clsDateTime
    Dim strSqlQry As String
    Dim strWhereCond As String
    Dim SqlConn As SqlConnection
    Dim myCommand As SqlCommand
    Dim myDataAdapter As SqlDataAdapter
    Dim strappid As String = ""
    Dim strappname As String = ""
#End Region
#Region "Enum GridCol"
    Enum GridCol
        Edit = 16
        View = 17
        Delete = 18
    End Enum
#End Region

#Region "Protected Sub btnAddNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddNew.Click"
    Protected Sub btnAddNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddNew.Click
        If objUtils.GetDBFieldFromStringnewdiv(Session("dbconnectionName"), "acccommon_master", "type", "type", "G", "div_id", ViewState("divcode")) <> Nothing Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Opening balance already exists.');", True)
            Exit Sub
        End If
        'Session.Add("State", "New")
        'Response.Redirect("OpeningTrailBalance.aspx", False)
        Dim strpop As String = ""
        Dim actionstr As String
        Dim type As String
        actionstr = ""
        actionstr = "New"
        type = "OB1"
        'strpop = "window.open('OpeningTrailBalance.aspx?State=" + CType(actionstr, String) + "&type=" + CType(type, String) + "','OpeningTrailBalance','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
        strpop = "window.open('OpeningTrailBalance.aspx?State=" + CType(actionstr, String) + "&divid=" & ViewState("divcode") & "&type=" + CType(type, String) + "','OpeningTrailBalance');"
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
    End Sub
#End Region
#Region "Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load"

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Dim appid As String = CType(Request.QueryString("appid"), String)

        Dim AppName As HiddenField = CType(Master.FindControl("hdnAppName"), HiddenField)
        '*** Danny 04/04/2018>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
        'Dim divid As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select division_master_code from division_master where accountsmodulename='" & Session("DAppName") & "'")
        ''*** Danny 04/04/2018<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
        'ViewState.Add("divcode", divid)

        If appid Is Nothing = False Then
            strappname = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select displayname from appmaster where appid='" & appid & "'")
        End If
        ViewState("Appname") = strappname
        Dim divid As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select division_master_code from division_master where accountsmodulename='" & ViewState("Appname") & "'")
        ViewState.Add("divcode", divid)
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Page.IsPostBack = False Then
            Try

                'If CType(Session("GlobalUserName"), String) = Nothing Or CType(Session("Userpwd"), String) = Nothing Or CType(Session("AppName"), String) = Nothing Then
                '    Response.Redirect("Login.aspx", False)
                '    Exit Sub
                'End If
                Dim AppId As HiddenField = CType(Master.FindControl("hdnAppId"), HiddenField)
                Dim AppName As HiddenField = CType(Master.FindControl("hdnAppName"), HiddenField)
                Dim appidnew As String = CType(Request.QueryString("appid"), String)

                Dim strappid As String = ""
                Dim strappname As String = ""

                If appidnew Is Nothing = False Then
                    strappid = appidnew 'AppId.Value
                End If
                If AppName Is Nothing = False Then
                    '*** Danny 04/04/2018>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>            
                    strappname = Session("AppName")
                    '*** Danny 04/04/2018<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
                End If

                If CType(Session("GlobalUserName"), String) = Nothing Or CType(Session("Userpwd"), String) = Nothing Then
                    Response.Redirect("~/Login.aspx", False)
                    Exit Sub
                Else
                    objUser.CheckUserRight(Session("dbconnectionName"), CType(Session("GlobalUserName"), String), CType(Session("Userpwd"), String), _
                                                       CType(strappname, String), "AccountsModule\OpeningTrailBalanceSearch.aspx?appid=" + appidnew, btnAddNew, btnExportToExcel, _
                                                       btnPrint, gv_SearchResult, GridCol.Edit, GridCol.Delete, GridCol.View)

                End If

                FillGrid("tran_id")

            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objUtils.WritErrorLog("OpeningTrailBalanceSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            End Try
        End If
        ClientScript.GetPostBackEventReference(Me, String.Empty)
        If (Me.IsPostBack And Me.Request("__EVENTTARGET") = "OpeningTrailBalanceWindowPostBack") Then
            'btnSearch_Click(sender, e)
            FillGrid("tran_id")
        End If
    End Sub
#End Region
#Region "Protected Sub gv_SearchResult_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gv_SearchResult.RowCommand"
    Protected Sub gv_SearchResult_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gv_SearchResult.RowCommand
        Try
            Dim strpop As String = ""
            Dim actionstr As String
            actionstr = ""

            If e.CommandName = "Page" Then Exit Sub
            If e.CommandName = "Sort" Then Exit Sub
            Dim lblId As Label
            Dim lbltrantype As Label
            lblId = gv_SearchResult.Rows(CType(e.CommandArgument.ToString, String)).FindControl("lblTranID")
            lbltrantype = gv_SearchResult.Rows(CType(e.CommandArgument.ToString, String)).FindControl("lbltrantype")

            If e.CommandName = "EditRow" Then
                'Session.Add("State", "Edit")
                'Session.Add("RefCode", CType(lblId.Text.Trim, String))
                'Response.Redirect("OpeningTrailBalance.aspx", False)
                If Validateseal(lblId.Text) Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Document has sealed cannot Edit/Delete...')", True)
                    Return
                End If


                actionstr = "Edit"
                'strpop = "window.open('OpeningTrailBalance.aspx?State=" + CType(actionstr, String) + "&RefCode=" + CType(lblId.Text.Trim, String) + "&type=" + CType(lbltrantype.Text, String) + "','OpeningTrailBalance','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                strpop = "window.open('OpeningTrailBalance.aspx?State=" + CType(actionstr, String) + "&divid=" & ViewState("divcode") & "&RefCode=" + CType(lblId.Text.Trim, String) + "&type=" + CType(lbltrantype.Text, String) + "','OpeningTrailBalance');"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)

            ElseIf e.CommandName = "View" Then
                'Session.Add("State", "View")
                'Session.Add("RefCode", CType(lblId.Text.Trim, String))
                'Response.Redirect("OpeningTrailBalance.aspx", False)
                actionstr = "View"
                'strpop = "window.open('OpeningTrailBalance.aspx?State=" + CType(actionstr, String) + "&RefCode=" + CType(lblId.Text.Trim, String) + "&type=" + CType(lbltrantype.Text, String) + "','OpeningTrailBalance','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                strpop = "window.open('OpeningTrailBalance.aspx?State=" + CType(actionstr, String) + "&divid=" & ViewState("divcode") & "&RefCode=" + CType(lblId.Text.Trim, String) + "&type=" + CType(lbltrantype.Text, String) + "','OpeningTrailBalance');"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)

            ElseIf e.CommandName = "DeleteRow" Then
                'Session.Add("State", "Delete")
                'Session.Add("RefCode", CType(lblId.Text.Trim, String))
                'Response.Redirect("OpeningTrailBalance.aspx", False)
                If Validateseal(lblId.Text) Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Document has sealed cannot Edit/Delete...')", True)
                    Return
                End If
                actionstr = "Delete"
                'strpop = "window.open('OpeningTrailBalance.aspx?State=" + CType(actionstr, String) + "&RefCode=" + CType(lblId.Text.Trim, String) + "&type=" + CType(lbltrantype.Text, String) + " ','OpeningTrailBalance','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                strpop = "window.open('OpeningTrailBalance.aspx?State=" + CType(actionstr, String) + "&divid=" & ViewState("divcode") & "&RefCode=" + CType(lblId.Text.Trim, String) + "&type=" + CType(lbltrantype.Text, String) + " ','OpeningTrailBalance');"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)

            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("OpeningTrailBalance.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#End Region

    Public Function Validateseal(ByVal tranid) As Boolean
        Try
            Dim ds As DataSet = objUtils.ExecuteQuerySqlnew(Session("dbconnectionName"), "select * from  acccommon_master where div_id='" & ViewState("divcode") & "' and tran_id='" + tranid + "' ")
            If ds.Tables.Count > 0 Then
                If ds.Tables(0).Rows.Count > 0 Then
                    If IsDBNull(ds.Tables(0).Rows(0)("acccommon_tran_state")) = False Then
                        If ds.Tables(0).Rows(0)("acccommon_tran_state") = "S" Then
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
            objUtils.WritErrorLog("ReservationInvoiceSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Function

#Region "Private Sub FillGrid(ByVal strorderby As String, Optional ByVal strsortorder As String = 'ASC')"
    Private Sub FillGrid(ByVal strorderby As String, Optional ByVal strsortorder As String = "ASC")
        Dim myDS As New DataSet
        gv_SearchResult.Visible = True
        lblMsg.Visible = False
        If gv_SearchResult.PageIndex < 0 Then
            gv_SearchResult.PageIndex = 0
        End If
        strSqlQry = ""
        Try
            '   strSqlQry = " SELECT  *   FROM acccommon_master where isnull(acccommon_master.acccommon_tran_state,'')<>'D' and acccommon_master.tran_type='OB1' "
            strSqlQry = " select acccommon_master.tran_id ,acccommon_master.tran_type , " & _
                         " convert(varchar(10),acccommon_master.tran_date,103) AS  tran_date,acccommon_master.type ," & _
                         " acccommon_master.code ,acccommon_master.currcode , " & _
                          " acccommon_master.currency_rate ,acccommon_master.amount,  acccommon_master.baseamount ,acccommon_master.adddate ,acccommon_master.adduser ,acccommon_master.moddate  ,acccommon_master.moduser from acccommon_master where div_id='" & ViewState("divcode") & "'"

            strSqlQry = strSqlQry & "  ORDER BY " & strorderby & " " & strsortorder

            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
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
            objUtils.WritErrorLog("OpeningTrailBalanceSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            'clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
            'clsDBConnect.dbConnectionClose(SqlConn)                          'Close connection
        End Try

    End Sub
#End Region
#Region "  Protected Sub gv_SearchResult_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gv_SearchResult.PageIndexChanging"
    Protected Sub gv_SearchResult_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gv_SearchResult.PageIndexChanging
        gv_SearchResult.PageIndex = e.NewPageIndex
        FillGrid("tranid")
    End Sub
#End Region
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
#Region "Protected Sub btnExportToExcel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExportToExcel.Click"
    Protected Sub btnExportToExcel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExportToExcel.Click
        Dim DS As New DataSet
        Dim DA As SqlDataAdapter
        Dim con As SqlConnection
        Dim objcon As New clsDBConnect
        Try
            If gv_SearchResult.Rows.Count <> 0 Then

                strSqlQry = " select acccommon_master.tran_id AS   [Transaction ID],acccommon_master.tran_type AS   [Transaction Type], " & _
                            "  convert(varchar(10),acccommon_master.tran_date,103)  AS   [Transaction Date],acccommon_master.type AS   [Type]," & _
                            " acccommon_master.code AS   [Code],acccommon_master.currcode AS   [Currency], " & _
                             " acccommon_master.currency_rate AS   [Conversion Rate],acccommon_master.amount AS   [Amount],acccommon_master.baseamount AS   [Base Amount],acccommon_master.adddate AS   [Date Created],acccommon_master.adduser AS   [User Created],acccommon_master.moddate AS   [Date Modified],acccommon_master.moduser AS   [User Modified] from acccommon_master where div_id='" & ViewState("divcode") & "'"

                strSqlQry = strSqlQry & " ORDER BY tran_id "


                con = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))

                DA = New SqlDataAdapter(strSqlQry, con)
                DA.Fill(DS, "cancellation")

                objUtils.ExportToExcel(DS, Response)
                con.Close()
            Else
                objUtils.MessageBox("Sorry , No data availabe for export to excel.", Page)
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        End Try
    End Sub
#End Region

    Protected Sub gv_SearchResult_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gv_SearchResult.Sorting
        Session.Add("strsortexpression", e.SortExpression)
        SortGridColoumn_click()
    End Sub
    Protected Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        Try
            Dim strReportTitle As String = ""
            'Dim colReportParams As New Collection
            'Dim creportparam As New clsReportParam
            'Dim lblId, lblTranType, lblType As Label
            Dim lblid As Label
            Dim lbltrantype As Label
            Dim lbltype As Label
            'Session.Add("Pageame", "OpeningTrailBalance")
            'Session.Add("BackPageName", "~\AccountsModule\OpeningTrailBalanceSearch.aspx")

            Dim Gvrow As GridViewRow

            For Each Gvrow In gv_SearchResult.Rows
                lblid = Gvrow.FindControl("lblTranID")
                lbltype = Gvrow.FindControl("lblType")
                lbltrantype = Gvrow.FindControl("lblTranType")
                Dim strpop As String = ""
                'strpop = "window.open('rptReportNew.aspx?Pageame=OpeningTrailBalance&BackPageName=OpeningTrailBalanceSearch.aspx&TranID=" + CType(lblid.Text.Trim, String) + "&TranType=" & CType(lbltrantype.Text.Trim, String) & "&Type=" & CType(lbltype.Text.Trim, String) & "','RepGenPol','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                strpop = "window.open('rptReportNew.aspx?Pageame=OpeningTrailBalance&BackPageName=OpeningTrailBalanceSearch.aspx&TranID=" + CType(lblid.Text.Trim, String) + "&divid=" & ViewState("divcode") & "&TranType=" & CType(lbltrantype.Text.Trim, String) & "&Type=" & CType(lbltype.Text.Trim, String) & "','RepGenPol');"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
            Next

            '    creportparam = New clsReportParam
            '    creportparam.rep_parametername = "@tran_id"
            '    If CType(lblId.Text.Trim, String) <> "" Then
            '        strReportTitle = "Transaction ID : " & CType(lblId.Text.Trim, String)
            '        creportparam.rep_parametervalue = CType(lblId.Text.Trim, String)
            '    Else
            '        creportparam.rep_parametervalue = ""
            '    End If
            '    colReportParams.Add(creportparam)

            '    creportparam = New clsReportParam
            '    creportparam.rep_parametername = "@tran_type"
            'lbltrantype = Gvrow.FindControl("lblTranType")
            '    If CType(lblTranType.Text.Trim, String) <> "" Then
            '        creportparam.rep_parametervalue = CType(lblTranType.Text.Trim, String)
            '    Else
            '        creportparam.rep_parametervalue = ""
            '    End If
            '    colReportParams.Add(creportparam)


            '    creportparam = New clsReportParam
            '    creportparam.rep_parametername = "@type"
            ' lbltype = Gvrow.FindControl("lblType")
            '    If CType(lblType.Text.Trim, String) <> "" Then
            '        creportparam.rep_parametervalue = CType(lblType.Text.Trim, String)
            '    Else
            '        creportparam.rep_parametervalue = ""
            '    End If
            '    colReportParams.Add(creportparam)


            '    creportparam = New clsReportParam
            '    creportparam.rep_parametername = "@orderby"
            '    creportparam.rep_parametervalue = 0
            '    colReportParams.Add(creportparam)
            '    Exit For
            'Next



        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("OpeningTrailBalanceSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try

    End Sub

    Protected Sub btnhelp_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnhelp.Click
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=OpeningTrailBalanceSearch','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub

    Protected Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
       

        'If Page.IsPostBack = False Then
        '    If Request.QueryString("appid") Is Nothing = False Then


        '        Select Case appid
        '            Case 1
        '                Me.MasterPageFile = "~/PriceListMaster.master"
        '            Case 2
        '                Me.MasterPageFile = "~/RoomBlock.master"
        '            Case 3
        '                Me.MasterPageFile = "~/ReservationMaster.master"
        '            Case 4
        '                Me.MasterPageFile = "~/AccountsMaster.master"
        '            Case 5
        '                Me.MasterPageFile = "~/UserAdminMaster.master"
        '            Case 6
        '                Me.MasterPageFile = "~/WebAdminMaster.master"
        '            Case 7
        '                Me.MasterPageFile = "~/TransferHistoryMaster.master"
        '            Case 10
        '                Me.MasterPageFile = "~/TransferMaster.master"
        '            Case 11
        '                Me.MasterPageFile = "~/ExcursionMaster.master"
        '            Case 14
        '                Me.MasterPageFile = "~/AccountsMaster.master"
        '            Case 13
        '                Me.MasterPageFile = "~/VisaMaster.master"      'Added by Archana on 05/06/2015 for VisaModule
        '            Case Else
        '                Me.MasterPageFile = "~/SubPageMaster.master"
        '        End Select
        '    Else
        '        Me.MasterPageFile = "~/SubPageMaster.master"
        '    End If
        'End If
    End Sub
End Class
