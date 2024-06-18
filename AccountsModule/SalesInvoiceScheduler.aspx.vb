Imports System.Data
Imports System.Data.SqlClient
Imports System.Linq
Imports System.IO
Imports System.Collections.Generic

Partial Class SalesInvoiceScheduler
    Inherits System.Web.UI.Page

#Region "Global Declaration"
    Dim objUtils As New clsUtils
    Dim strSqlQry As String
    Dim mySqlCmd As SqlCommand
    Dim mySqlReader As SqlDataReader
    Dim myDataAdapter As SqlDataAdapter
    Dim sqlConn As SqlConnection
    Dim sqlTrans As SqlTransaction
#End Region



#Region "Protected Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit"
    Protected Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
        If Convert.ToString(Request.QueryString("State")) = "Amend" Then
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
                    Case 14
                        Me.MasterPageFile = "~/AccountsMaster.master"
                    Case 16
                        Me.MasterPageFile = "~/AccountsMaster.master"   '' Added shahul MCP accounts
                    Case Else
                        Me.MasterPageFile = "~/SubPageMaster.master"
                End Select
            Else
                Me.MasterPageFile = "~/SubPageMaster.master"
            End If
        End If
    End Sub
#End Region

#Region "Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack = False Then
            Try
                If CType(Session("GlobalUserName"), String) = "" Then
                    Response.Redirect("~/Login.aspx", False)
                    Exit Sub
                End If
                If Not Session("CompanyName") Is Nothing Then
                    Me.Page.Title = CType(Session("CompanyName"), String)
                End If

                DisplaySchedulerGrid("PageLoad", 0)


                
            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objUtils.WritErrorLog("SalesInvoiceScheduler.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            End Try
        End If
    End Sub
#End Region

  
    Protected Sub gvScheduler_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvScheduler.RowDataBound
        Try
            If e.Row.RowType = DataControlRowType.DataRow Then
                Dim lblprocessflag As Label = CType(e.Row.FindControl("lblprocessflag"), Label)
                Dim lbtnViewProcessed As LinkButton = CType(e.Row.FindControl("lbtnViewProcessed"), LinkButton)
                Dim lbtnViewError As LinkButton = CType(e.Row.FindControl("lbtnViewError"), LinkButton)
                If lblprocessflag.Text = "2" Then
                    lbtnViewProcessed.Visible = True
                    lbtnViewError.Visible = True
                Else
                    lbtnViewProcessed.Visible = False
                    lbtnViewError.Visible = False
                End If
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("SalesInvoiceScheduler.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub

    Protected Sub gvScheduler_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvScheduler.RowCommand
        Try
            If e.CommandName = "Processed" Then
                Dim rowIndex As Integer = Convert.ToInt32(e.CommandArgument)
                Dim row As GridViewRow = gvScheduler.Rows(rowIndex)
                Dim lblScheduleId As Label = CType(row.FindControl("lblScheduleId"), Label)
                If lblScheduleId.Text.Trim <> "" Then
                    DisplaySchedulerGrid("Processed", Convert.ToInt64(lblScheduleId.Text))
                End If
            ElseIf e.CommandName = "Error" Then
                Dim rowIndex As Integer = Convert.ToInt32(e.CommandArgument)
                Dim row As GridViewRow = gvScheduler.Rows(rowIndex)
                Dim lblScheduleId As Label = CType(row.FindControl("lblScheduleId"), Label)
                If lblScheduleId.Text.Trim <> "" Then
                    DisplaySchedulerGrid("Error", Convert.ToInt64(lblScheduleId.Text))
                End If

            End If


        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("SalesInvoiceScheduler.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub

    Protected Sub DisplaySchedulerGrid(ByVal mode As String, ByVal Sid As Integer)
        Try
            lblMsg.Visible = False
            lblErrorMsg.Visible = False
            sqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))          'connection open
            mySqlCmd = New SqlCommand("view_salesInvoice_Scheduler", sqlConn)
            mySqlCmd.CommandType = CommandType.StoredProcedure
            mySqlCmd.Parameters.Add(New SqlParameter("@mode", SqlDbType.VarChar, 20)).Value = mode
            mySqlCmd.Parameters.Add(New SqlParameter("@Sid", SqlDbType.BigInt)).Value = Sid
            mySqlCmd.CommandTimeout = 0

            Using myDataAdapter As New SqlDataAdapter(mySqlCmd)
                Using dt As New DataTable()
                    myDataAdapter.Fill(dt)

                    If mode = "PageLoad" Then
                        gvScheduler.DataSource = dt
                        gvScheduler.DataBind()
                    ElseIf mode = "Processed" Then
                        gvProcessedInvoice.DataSource = dt
                        gvProcessedInvoice.DataBind()

                        gvProcessError.DataSource = Nothing
                        gvProcessError.DataBind()
                        If dt.Rows.Count = 0 Then
                            lblMsg.Visible = True
                        End If
                        TabInvoice.ActiveTabIndex = 0
                    Else
                        gvProcessError.DataSource = dt
                        gvProcessError.DataBind()

                        gvProcessedInvoice.DataSource = Nothing
                        gvProcessedInvoice.DataBind()

                        TabInvoice.ActiveTabIndex = 1
                        If dt.Rows.Count = 0 Then
                            lblErrorMsg.Visible = True
                        End If
                    End If


                End Using
            End Using
            clsDBConnect.dbCommandClose(mySqlCmd)
            clsDBConnect.dbConnectionClose(sqlConn)
        Catch ex As Exception
            'ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + HttpUtility.JavaScriptStringEncode(ex.Message) & "' );", True)
            objUtils.WritErrorLog("SalesInvoiceScheduler.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub

End Class
