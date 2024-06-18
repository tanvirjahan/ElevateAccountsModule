#Region "Namespace"
Imports System.Data
Imports System.Data.SqlClient
Imports System.Collections.Generic
Imports System.Net.Mail
Imports System.Web.Mail
#End Region

Partial Class ExcursionModule_DeskExcursionRequestSearch
    Inherits System.Web.UI.Page

#Region "Global Declarations"
    Dim objUtils As New clsUtils
    Dim objUser As New clsUser
    Dim strSqlQry As String
    Dim strWhereCond As String
    Dim SqlConn As SqlConnection
    Dim myCommand As SqlCommand
    Dim myDataAdapter As SqlDataAdapter
    Dim ObjDate As New clsDateTime
    Dim mySqlCmd As SqlCommand
    Dim mySqlReader As SqlDataReader
    Dim mySqlConn As SqlConnection
#End Region

#Region "Enum GridCol"
    Enum GridCol
        ticketidHidden = 0
        ticketid = 1
        datereceived = 2
        othgrpname = 3
        othtypname = 4
        fromticketno = 5
        toticketno = 6
        ticketdate = 7
        Status = 8
        DateCreated = 9
        UserCreated = 10
        DateModified = 11
        UserModified = 12
        Edit = 13
        Delete = 14
        View = 15
        Assign = 16
        Transfer = 17

        'DateCreated = 8
        'UserCreated = 9
        'DateModified = 10
        'UserModified = 11
        'Edit = 12
        'Delete = 13
        'View = 14
        'Assign = 15
        'Transfer = 16
    End Enum
#End Region


    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        If Page.IsPostBack = False Then
            Try
                Dim AppId As HiddenField = CType(Master.FindControl("hdnAppId"), HiddenField)
                Dim AppName As HiddenField = CType(Master.FindControl("hdnAppName"), HiddenField)
                Dim strappid As String = ""
                Dim strappname As String = ""
                Dim otypecode1, otypecode2 As String

                If AppId Is Nothing = False Then
                    strappid = AppId.Value
                End If
                If AppName Is Nothing = False Then
                    strappname = AppName.Value
                End If


                SetFocus(txtExcursionID)
                If CType(Session("GlobalUserName"), String) = Nothing Or CType(Session("Userpwd"), String) = Nothing Then
                    Response.Redirect("~/Login.aspx", False)
                    Exit Sub
                Else
                    'objUser.CheckUserRight(Session("dbconnectionName"), CType(Session("GlobalUserName"), String), CType(Session("Userpwd"), String), _
                    'CType(strappname, String), "ExcursionModule\DeskExcursionsRequestSearch.aspx?appid=11", btnAddNew, btnExportToExcel, _
                    'btnExportToExcel, gv_SearchResult, GridCol.Edit, GridCol.Delete, GridCol.View, 0, 0, 0, 0, 0, GridCol.Transfer, GridCol.Assign)
                End If


                otypecode1 = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id=1104")
                otypecode2 = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id=1105")


                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlExTypeCode, "othtypcode", "othtypname", "select rtrim(ltrim(othtypcode))othtypcode,rtrim(ltrim(othtypname))othtypname from othtypmast a  inner join othgrpmast b on a.othgrpcode=b.othgrpcode and b.othmaingrpcode in('" & otypecode1 & "','" & otypecode2 & "') and a.active=1 order by a.othtypcode", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlExTypeName, "othtypname", "othtypcode", "select rtrim(ltrim(othtypcode))othtypcode,rtrim(ltrim(othtypname))othtypname from othtypmast a  inner join othgrpmast b on a.othgrpcode=b.othgrpcode and b.othmaingrpcode in('" & otypecode1 & "','" & otypecode2 & "') and a.active=1 order by a.othtypname", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCustomer, "agentname", "agentcode", "select agentname,agentcode from agentmast where active=1 order by agentname", True)


                Panel1.Visible = False

                FillGridWithOrderByValues()

                Dim typ As Type
                typ = GetType(DropDownList)

                If (Page.ClientScript.IsClientScriptIncludeRegistered(typ, "TADDScript.js")) = False Then
                    Page.ClientScript.RegisterClientScriptInclude(typ, "TADDScript.js", "TADDScript.js")
                    ddlExTypeName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlExTypeCode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                End If

            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objUtils.WritErrorLog("DeskExcursionsRequestSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            End Try
        End If
        btnClear.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to clear search criteria?')==false)return false;")

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If IsPostBack = True Then


            End If
            ClientScript.GetPostBackEventReference(Me, String.Empty)
            If (Me.IsPostBack And Me.Request("__EVENTTARGET") = "ExcursionsRequestWindowPostBack") Then
                btnSearch_Click(sender, e)
            End If


        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("DeskExcursionsRequestSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try

    End Sub

#Region "  Private Function BuildCondition() As String"
    Private Function BuildCondition() As String
        Dim objDateTime As New clsDateTime

        Dim viewall As Integer = 0
        viewall = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select isnull(resstatus,0)  from  usermaster(nolock)  where  usercode='" & CType(Session("GlobalUserName"), String) & "' ")

        If viewall = 0 Then

            strWhereCond = "excursions_header.adduser like '" & CType(Session("GlobalUserName"), String) & "'"
        End If
        'Added strwherecond by Archana on 07/03/2015 to display only the login users
        If txtExcursionID.Text.Trim <> "" Then
            If Trim(strWhereCond) = "" Then

                strWhereCond = " (excursions_header.excid) LIKE '" & Trim(txtExcursionID.Text.Trim.ToUpper) & "%'"
            Else
                strWhereCond = strWhereCond & " AND (excursions_header.excid) LIKE '" & Trim(txtExcursionID.Text.Trim.ToUpper) & "%'"
            End If
        End If

        If txtTicketNo.Text.Trim <> "" Then
            If Trim(strWhereCond) = "" Then

                strWhereCond = " (excursions_header.ticketno) LIKE '" & Trim(txtTicketNo.Text.Trim.ToUpper) & "%'"
            Else
                strWhereCond = strWhereCond & " AND (excursions_header.ticketno) LIKE '" & Trim(txtTicketNo.Text.Trim.ToUpper) & "%'"
            End If
        End If



        If ddlExTypeCode.Value <> "[Select]" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = " (othtypmast.othtypcode) = '" & Trim(CType(ddlExTypeCode.Items(ddlExTypeCode.SelectedIndex).Text, String)) & "'"
            Else
                strWhereCond = strWhereCond & " AND (othtypmast.othtypcode) = '" & Trim(CType(ddlExTypeCode.Items(ddlExTypeCode.SelectedIndex).Text, String)) & "'"
            End If
        End If


        If txtFromDate.Text <> "" And txtTodate.Text <> "" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = "( (convert(varchar(10),(excursions_header.requestdate),111) between convert(varchar(10), '" & Format(CType(txtFromDate.Text, Date), "yyyy/MM/dd") & "',111) " _
                & "  and  convert(varchar(10), '" & Format(CType(txtTodate.Text, Date), "yyyy/MM/dd") & "',111) ) " _
                & "  or (convert(varchar(10),(excursions_header.requestdate),111)  between convert(varchar(10), '" & Format(CType(txtFromDate.Text, Date), "yyyy/MM/dd") & "',111) " _
                & "  and  convert(varchar(10), '" & Format(CType(txtTodate.Text, Date), "yyyy/MM/dd") & "',111) )" _
                & " or (convert(varchar(10),(excursions_header.requestdate),111) < convert(varchar(10), '" & Format(CType(txtFromDate.Text, Date), "yyyy/MM/dd") & "',111) " _
                & "  and convert(varchar(10),(excursions_header.requestdate),111) > convert(varchar(10), '" & Format(CType(txtTodate.Text, Date), "yyyy/MM/dd") & "',111)) )"
            Else
                strWhereCond = strWhereCond & " and ( (convert(varchar(10),(excursions_header.requestdate),111) between convert(varchar(10), '" & Format(CType(txtFromDate.Text, Date), "yyyy/MM/dd") & "',111) " _
                & "  and  convert(varchar(10), '" & Format(CType(txtTodate.Text, Date), "yyyy/MM/dd") & "',111) ) " _
                & "  or (convert(varchar(10),(excursions_header.requestdate),111)  between convert(varchar(10), '" & Format(CType(txtFromDate.Text, Date), "yyyy/MM/dd") & "',111) " _
                & "  and  convert(varchar(10), '" & Format(CType(txtTodate.Text, Date), "yyyy/MM/dd") & "',111) )" _
                & " or (convert(varchar(10),(excursions_header.requestdate),111) < convert(varchar(10), '" & Format(CType(txtFromDate.Text, Date), "yyyy/MM/dd") & "',111) " _
                & "  and convert(varchar(10),(excursions_header.requestdate),111) > convert(varchar(10), '" & Format(CType(txtTodate.Text, Date), "yyyy/MM/dd") & "',111)) )"

            End If
        End If

        If txtFromTourDate.Text <> "" And txtToTourdate.Text <> "" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = "( (convert(varchar(10),(excursions_detail.tourdate),111) between convert(varchar(10), '" & Format(CType(txtFromTourDate.Text, Date), "yyyy/MM/dd") & "',111) " _
                & "  and  convert(varchar(10), '" & Format(CType(txtToTourdate.Text, Date), "yyyy/MM/dd") & "',111) ) " _
                & "  or (convert(varchar(10),(excursions_detail.tourdate),111)  between convert(varchar(10), '" & Format(CType(txtFromTourDate.Text, Date), "yyyy/MM/dd") & "',111) " _
                & "  and  convert(varchar(10), '" & Format(CType(txtToTourdate.Text, Date), "yyyy/MM/dd") & "',111) )" _
                & " or (convert(varchar(10),(excursions_detail.tourdate),111) < convert(varchar(10), '" & Format(CType(txtFromTourDate.Text, Date), "yyyy/MM/dd") & "',111) " _
                & "  and convert(varchar(10),(excursions_detail.tourdate),111) > convert(varchar(10), '" & Format(CType(txtToTourdate.Text, Date), "yyyy/MM/dd") & "',111)) )"
            Else
                strWhereCond = strWhereCond & " and ( (convert(varchar(10),(excursions_detail.tourdate),111) between convert(varchar(10), '" & Format(CType(txtFromTourDate.Text, Date), "yyyy/MM/dd") & "',111) " _
                & "  and  convert(varchar(10), '" & Format(CType(txtToTourdate.Text, Date), "yyyy/MM/dd") & "',111) ) " _
                & "  or (convert(varchar(10),(excursions_detail.tourdate),111)  between convert(varchar(10), '" & Format(CType(txtFromTourDate.Text, Date), "yyyy/MM/dd") & "',111) " _
                & "  and  convert(varchar(10), '" & Format(CType(txtToTourdate.Text, Date), "yyyy/MM/dd") & "',111) )" _
                & " or (convert(varchar(10),(excursions_detail.tourdate),111) < convert(varchar(10), '" & Format(CType(txtFromTourDate.Text, Date), "yyyy/MM/dd") & "',111) " _
                & "  and convert(varchar(10),(excursions_detail.tourdate),111) > convert(varchar(10), '" & Format(CType(txtToTourdate.Text, Date), "yyyy/MM/dd") & "',111)) )"

            End If
        End If



        If txtGuestName.Text.Trim <> "" Then
            If Trim(strWhereCond) = "" Then

                strWhereCond = " (excursions_detail.guestname) LIKE '%" & Trim(txtGuestName.Text.Trim.ToUpper) & "%'"
            Else
                strWhereCond = strWhereCond & " AND (excursions_detail.guestname) LIKE '%" & Trim(txtGuestName.Text.Trim.ToUpper) & "%'"
            End If
        End If


        If txtPrepaidID.Text.Trim <> "" Then
            If Trim(strWhereCond) = "" Then

                strWhereCond = " (isnull(excursions_header.prepaidid,'')) LIKE '" & Trim(txtPrepaidID.Text.Trim.ToUpper) & "%'"
            Else
                strWhereCond = strWhereCond & " AND (isnull(excursions_header.prepaidid,'')) LIKE '" & Trim(txtPrepaidID.Text.Trim.ToUpper) & "%'"
            End If
        End If


        If ddlCustomer.Value <> "[Select]" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = " (agentmast.agentname) = '" & Trim(CType(ddlCustomer.Items(ddlCustomer.SelectedIndex).Text, String)) & "'"
            Else
                strWhereCond = strWhereCond & " AND (agentmast.agentname) = '" & Trim(CType(ddlCustomer.Items(ddlCustomer.SelectedIndex).Text, String)) & "'"
            End If
        End If


        BuildCondition = strWhereCond
    End Function

#End Region

#Region "Numbers"
    Public Sub Numbers(ByVal txtbox As HtmlInputText)
        txtbox.Attributes.Add("onkeypress", "return checkNumber(event)")
    End Sub
#End Region

#Region "NumbersForTextbox"
    Public Sub NumbersForTextbox(ByVal txtbox As TextBox)
        txtbox.Attributes.Add("onkeypress", "return checkNumber(event)")
    End Sub
#End Region

#Region "charcters"
    Public Sub charcters(ByVal txtbox As TextBox)
        txtbox.Attributes.Add("onkeypress", "return checkCharacter(event)")
    End Sub
#End Region

#Region "TextLock"
    Public Sub TextLock(ByVal txtbox As TextBox)
        txtbox.Attributes.Add("onKeyPress", "return chkTextLock(event)")
        txtbox.Attributes.Add("onKeyDown", "return chkTextLock(event)")
        txtbox.Attributes.Add("onKeyUp", "return chkTextLock(event)")
    End Sub
#End Region

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

            'strSqlQry = "select max(excursions_detail.rlineno)rlineno," & _
            '            "max(excursions_header.excid)excid," & _
            '            "max(excursions_header.ticketno)ticketno," & _
            '            "max(isnull(excursions_header.prepaidid,''))prepaidid," & _
            '            "max(excursions_header.paycode) PaymentMode," & _
            '            "max(excursions_detail.adults)adults," & _
            '            "max(excursions_detail.child)child," & _
            '            "max(plgrpmast.plgrpname) market," & _
            '            "max(convert(varchar(10),excursions_header.requestdate,103))RequestDate," & _
            '            "max(agentmast.agentname)agentname," & _
            '            "max(othtypmast.othtypcode) ServiceCode," & _
            '            "max(othtypmast.othtypname) ServiceName," & _
            '            "max(excursions_detail.guestname)guestname," & _
            '            "max(convert(varchar(10),excursions_detail.tourdate,103))TourDate," & _
            '            "max(partymast.partyname)partyname," & _
            '            "max(excursions_detail.roomno)roomno," & _
            '            "max(convert(varchar(10),excursions_detail.arrdate,103))ArrivalDate," & _
            '            "max(excursions_header.spersoncode)spersoncode," & _
            '            "max(excursions_header.adddate)adddate," & _
            '            "max(excursions_header.adduser)adduser," & _
            '            "max(isnull(convert(varchar(10),excursions_header.moddate,103),''))moddate," & _
            '            "max(isnull(excursions_header.moduser,''))moduser " & _
            '            "from excursions_header " & _
            '            "inner join excursions_detail(nolock) on excursions_header.excid=excursions_detail.excid " & _
            '            "left join agentmast on agentmast.agentcode=excursions_header.agentcode " & _
            '            "left join partymast on partymast.partycode=excursions_detail.hotel " & _
            '            "inner join othtypmast on othtypmast.othtypcode= excursions_detail.othtypcode " & _
            '            "inner join plgrpmast on plgrpmast.plgrpcode=excursions_header.plgrpcode " & _
            '            "group by excursions_detail.excid "

            strSqlQry = "select (excursions_detail.rlineno)rlineno," & _
                       "(excursions_header.excid)excid," & _
                       "(excursions_header.ticketno)ticketno," & _
                       "(isnull(excursions_header.prepaidid,''))prepaidid," & _
                       "(excursions_header.paycode) PaymentMode," & _
                       "(excursions_detail.adults)adults," & _
                       "(excursions_detail.child)child," & _
                       "(plgrpmast.plgrpname) market," & _
                       "(convert(varchar(10),excursions_header.requestdate,103))RequestDate," & _
                       "(agentmast.agentname)agentname," & _
                       "(othtypmast.othtypcode) ServiceCode," & _
                       "(othtypmast.othtypname) ServiceName," & _
                       "(excursions_detail.guestname)guestname," & _
                       "(convert(varchar(10),excursions_detail.tourdate,103))TourDate," & _
                       "(partymast.partyname)partyname," & _
                       "(excursions_detail.roomno)roomno," & _
                       "(convert(varchar(10),excursions_detail.arrdate,103))ArrivalDate," & _
                       "(excursions_header.spersoncode)spersoncode," & _
                       "(excursions_header.adddate)adddate," & _
                       "(excursions_header.adduser)adduser," & _
                       "(isnull(convert(varchar(10),excursions_header.moddate,103),''))moddate," & _
                       "(isnull(excursions_header.moduser,''))moduser, " & _
                       "status=case isnull(excursions_detail.cancelled,0) when 1 then  'Cancelled' else (   " & _
                       " case isnull(excursions_detail.confirmed,0) when 1 then 'Confirmed' else 'Pending'  end )  end " & _
                       "from excursions_header " & _
                       "inner join excursions_detail(nolock) on excursions_header.excid=excursions_detail.excid " & _
                       "left join agentmast on agentmast.agentcode=excursions_header.agentcode " & _
                       "left join partymast on partymast.partycode=excursions_detail.hotel " & _
                       "inner join othtypmast on othtypmast.othtypcode= excursions_detail.othtypcode " & _
                       "inner join plgrpmast on plgrpmast.plgrpcode=excursions_header.plgrpcode "


            If Trim(BuildCondition) <> "" Then
                strSqlQry = strSqlQry & " where " & BuildCondition() & " ORDER BY " & strorderby & " " & strsortorder
            Else
                strSqlQry = strSqlQry & " ORDER BY " & strorderby & " " & strsortorder
            End If

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
            objUtils.WritErrorLog("DeskExcursionsRequestSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
            clsDBConnect.dbConnectionClose(SqlConn)                          'Close connection
        End Try

    End Sub
#End Region

#Region "Protected Sub btnAddNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddNew.Click"
    Protected Sub btnAddNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddNew.Click
        Session("RowState") = ""
        Session("NewRLineNo") = 0
        Dim strpop As String = ""
        'strpop = "window.open('DeskExcursionRequestEntry.aspx?State=New','ExcursionRequestEntry','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
        strpop = "window.open('DeskExcursionRequestEntry.aspx?State=New','ExcursionRequestEntry');"
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
    End Sub

#End Region

#Region "Protected Sub gv_SearchResult_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gv_SearchResult.PageIndexChanging"
    Protected Sub gv_SearchResult_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gv_SearchResult.PageIndexChanging
        gv_SearchResult.PageIndex = e.NewPageIndex
        FillGridWithOrderByValues()
    End Sub

#End Region

    Private Function ChkAdjusted(ByVal requestid As String) As String
        ChkAdjusted = ""
        Dim invoiceno As String
        invoiceno = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select invno from  excursions_header(nolock)  where  excid='" & requestid & "' ")
        mySqlCmd = New SqlCommand
        mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
        mySqlCmd.Connection = mySqlConn

        Dim parms As New List(Of SqlParameter)
        Dim parm(3) As SqlParameter
        mySqlCmd.CommandType = CommandType.StoredProcedure

        mySqlCmd.CommandText = "sp_check_adjusted"
        parm(0) = New SqlParameter("@acc_tran_id", CType(invoiceno, String))
        parm(1) = New SqlParameter("@acc_tran_type", "IN")
        parm(2) = New SqlParameter("@errmessage", SqlDbType.VarChar, 1000)
        parm(2).Direction = ParameterDirection.Output
        parm(2).Value = ""
        mySqlCmd.Parameters.Clear()
        For i = 0 To 2
            mySqlCmd.Parameters.Add(parm(i))
        Next
        mySqlCmd.ExecuteNonQuery()

        Dim strError As String = ""
        strError = parm(2).Value.ToString()
        If strError = "" Then


        Else
            strError = strError.Remove(strError.Length - 1, 1)
            ChkAdjusted = strError
        End If
    End Function
#Region "Protected Sub gv_SearchResult_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gv_SearchResult.RowCommand"
    Protected Sub gv_SearchResult_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gv_SearchResult.RowCommand
        Try
            If e.CommandName = "Page" Then
                Exit Sub
            End If

            Dim sqlstr As String
            'Dim sqlcon As SqlConnection
            'Dim sqlcommand As SqlCommand
            Dim sqlreader As SqlDataReader
            Session("Excursion_ID") = ""
            Dim lblId As Label
            Dim lblrlineno As Label
            lblId = gv_SearchResult.Rows(CType(e.CommandArgument.ToString, String)).FindControl("lblExcursionId")
            lblrlineno = gv_SearchResult.Rows(CType(e.CommandArgument.ToString, String)).FindControl("lblrlineno")

            '19082014
            sqlstr = "select * from excursions_header where excid='" & lblId.Text & "'"
            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
            myCommand = New SqlCommand(sqlstr, SqlConn)
            sqlreader = myCommand.ExecuteReader


            Session("Excursion_ID") = lblId.Text
            Session("Rlineno") = lblrlineno.Text
            Dim groupid As Integer

            If e.CommandName = "EditRow" Then

                groupid = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select groupid from  usermaster(nolock)  where usercode='" & CType(Session("GlobalUserName"), String) & "'")
                If groupid = 8 Then

                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This  Edit options not Allow for Desk Sales');", True)
                Else
                    If sqlreader.Read = True Then

                        Dim amendno As String = 0

                        amendno = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select isnull(max(amendno),0) from  reservation_invoice_amendments  where amended=0 and requestid='" & lblId.Text & "'")

                        ''25/01/15 sorur asked to remove ths condition

                        'If amendno = 0 Then
                        '    Dim strmsg As String = ChkAdjusted(lblId.Text)
                        '    If strmsg <> "" Then

                        '        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & strmsg & "');", True)

                        '        Exit Sub
                        '    End If

                        'End If

                        Dim mindate1 As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select convert(varchar(10),min(tourdate),111) from excursions_detail(nolock) where excid='" & lblId.Text & "'")

                        Dim sealdate As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select sealdate from  sealing_master(nolock)")

                        If mindate1 <> "" Then
                            If Convert.ToDateTime(mindate1) <= Convert.ToDateTime(sealdate) Then

                                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Month Already Sealed');", True)
                                Return
                            End If
                        End If

                        Dim strmsg As String = ChkAdjusted(lblId.Text)
                        If strmsg <> "" Then

                            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & strmsg & "');", True)

                            Exit Sub
                        End If


                        If IsDBNull(sqlreader("prepaidid")) = False Then
                            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Booking Can't Edit');", True)
                        ElseIf sqlreader("invno") <> "" Then

                            Session("RowState") = ""
                            Dim strpop As String = ""
                            strpop = "DeskExcursionRequestEntry.aspx?State=EditRow&RefCode=" + CType(lblId.Text.Trim, String) + "','ExcursionRequestEntry','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes"
                            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "javascript:confirmInvoice('" & strpop & "');", True)

                            'If MsgBox("This Booking Already Invoices Do You want to Edit", MsgBoxStyle.YesNo, "Doc Edit") = MsgBoxResult.No Then
                            '    Exit Sub
                            'Else
                            '    Session("RowState") = ""
                            '    Dim strpop As String = ""
                            '    strpop = "window.open('ExcursionRequestEntry.aspx?State=EditRow&RefCode=" + CType(lblId.Text.Trim, String) + "','ExcursionRequestEntry','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                            '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
                            'End If
                        Else
                            Session("RowState") = ""
                            Dim strpop As String = ""
                            'strpop = "window.open('DeskExcursionRequestEntry.aspx?State=EditRow&RefCode=" + CType(lblId.Text.Trim, String) + "','ExcursionRequestEntry','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                            strpop = "window.open('DeskExcursionRequestEntry.aspx?State=EditRow&RefCode=" + CType(lblId.Text.Trim, String) + "','ExcursionRequestEntry');"
                            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
                        End If
                    End If
                End If

                'Session("RowState") = ""
                'Dim strpop As String = ""
                'strpop = "window.open('ExcursionRequestEntry.aspx?State=EditRow&RefCode=" + CType(lblId.Text.Trim, String) + "','ExcursionRequestEntry','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                'ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)

            ElseIf e.CommandName = "ViewRow" Then
                Session("RowState") = "ViewRow"
                Dim strpop As String = ""
                'strpop = "window.open('DeskExcursionRequestEntry.aspx?State=ViewRow&RefCode=" + CType(lblId.Text.Trim, String) + "','ExcursionRequestEntry','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                strpop = "window.open('DeskExcursionRequestEntry.aspx?State=ViewRow&RefCode=" + CType(lblId.Text.Trim, String) + "','ExcursionRequestEntry');"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
            ElseIf e.CommandName = "PrintRow" Then

                divExcursion.Style("display") = "block"
                divExcursion.Style("position") = "absolute"
                divExcursion.Style("top") = "400px"
                divExcursion.Style("left") = "500px"
                divExcursion.Style("z-index") = "100px"

            End If

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("DeskExcursionsRequestSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#End Region

#Region " Protected Sub gv_SearchResult_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gv_SearchResult.Sorting"
    Protected Sub gv_SearchResult_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gv_SearchResult.Sorting
        Session.Add("strsortexpression", e.SortExpression)
        SortGridColoumn_click()
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

                strSqlQry = "select max(excursions_header.excid) [Excursion ID]," & _
                      "max(convert(varchar(10),excursions_header.requestdate,103)) [RequestDate]," & _
                      "max(convert(varchar(10),excursions_detail.tourdate,103)) [TourDate]," & _
                      "max(othtypmast.othtypcode) [ServiceCode]," & _
                      "max(othtypmast.othtypname) [ServiceName]," & _
                      "max(partymast.partyname) [Hotel Name]," & _
                      "max(excursions_detail.guestname) [Guest Name]," & _
                      "max(excursions_detail.roomno) [Room No]," & _
                      "max(excursions_detail.adults) [Adult]," & _
                      "max(excursions_detail.child) [Child]," & _
                      "max(plgrpmast.plgrpname) [Market]," & _
                      "max(excursions_header.ticketno) [Ticket No]," & _
                      "max(excursions_header.paycode) [PaymentMode]," & _
                      "max(isnull(excursions_header.prepaidid,'')) [Prepaid Id]," & _
                      "max(agentmast.agentname) [Agent Name]," & _
                      "max(convert(varchar(10),excursions_detail.arrdate,103)) [ArrivalDate]," & _
                      "max(excursions_header.spersoncode) [SpersonCode]," & _
                      "max(excursions_header.adddate) [Date Created]," & _
                      "max(excursions_header.adduser) [User Created]," & _
                      "max(isnull(convert(varchar(10),excursions_header.moddate,103),'')) [Date Modified]," & _
                      "max(isnull(excursions_header.moduser,'')) [User Modified] " & _
                      "from excursions_header " & _
                      "inner join excursions_detail(nolock) on excursions_header.excid=excursions_detail.excid " & _
                      "left join agentmast on agentmast.agentcode=excursions_header.agentcode " & _
                      "left join partymast on partymast.partycode=excursions_detail.hotel " & _
                      "inner join othtypmast on othtypmast.othtypcode= excursions_detail.othtypcode " & _
                      "inner join plgrpmast on plgrpmast.plgrpcode=excursions_header.plgrpcode " & _
                      "group by excursions_detail.excid "

                If Trim(BuildCondition) <> "" Then
                    strSqlQry = strSqlQry & " HAVING " & BuildCondition() & " ORDER BY " & ExportWithOrderByValues()
                Else
                    strSqlQry = strSqlQry & " ORDER BY " & ExportWithOrderByValues()
                End If
                con = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))

                DA = New SqlDataAdapter(strSqlQry, con)
                DA.Fill(DS, "Manage_Recieved")

                objUtils.ExportToExcel(DS, Response)
                con.Close()

            Else
                objUtils.MessageBox("Sorry , No data availabe for export to excel.", Page)
            End If

        Catch ex As Exception

        End Try
    End Sub

#End Region

#Region "Protected Sub rbtnsearch_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)"
    Protected Sub rbtnsearch_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Panel1.Visible = False
        ddlExTypeCode.Visible = True
        ddlExTypeName.Visible = True
        SetFocus(txtExcursionID)
    End Sub
#End Region

#Region "Protected Sub rbtnadsearch_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)"
    Protected Sub rbtnadsearch_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Panel1.Visible = True
        ddlExTypeCode.Visible = True
        ddlExTypeName.Visible = True
        SetFocus(txtExcursionID)
    End Sub
#End Region

#Region "Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs)"
    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        If txtFromDate.Text <> "" Then
            If txtTodate.Text = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('To Date field can not be left blank.');", True)
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + txtTodate.ClientID + "');", True)
                Exit Sub
            End If
        End If
        If txtTodate.Text <> "" Then
            If txtFromDate.Text = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('From Date field can not be left blank.');", True)
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + txtFromDate.ClientID + "');", True)
                Exit Sub
            End If
        End If


        If txtFromTourDate.Text <> "" Then
            If txtToTourdate.Text = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('To Date field can not be left blank.');", True)
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + txtToTourdate.ClientID + "');", True)
                Exit Sub
            End If
        End If
        If txtToTourdate.Text <> "" Then
            If txtFromTourDate.Text = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('From Date field can not be left blank.');", True)
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + txtFromTourDate.ClientID + "');", True)
                Exit Sub
            End If
        End If

        FillGridWithOrderByValues()
        SetFocus(btnSearch)
    End Sub
#End Region

#Region "Protected Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs)"
    Protected Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClear.Click
        txtExcursionID.Text = ""
        ddlExTypeName.Value = "[Select]"
        ddlExTypeCode.Value = "[Select]"
        txtTicketNo.Text = ""
        txtFromDate.Text = ""
        txtTodate.Text = ""
        txtGuestName.Text = ""
        txtPrepaidID.Text = ""
        ddlOrderBy.SelectedIndex = 0
        FillGridWithOrderByValues()
        ddlCustomer.Value = "[Select]"
        accSearch.Value = ""
        SetFocus(txtExcursionID)
        txtFromTourDate.Text = ""
        txtToTourdate.Text = ""
    End Sub
#End Region

    Private Sub FillGridWithOrderByValues()
        Select Case ddlOrderBy.SelectedIndex
            Case 0
                FillGrid(" (excursions_header.adddate)", "DESC")
            Case 1
                FillGrid(" (excursions_header.excid)", "ASC")
            Case 2
                FillGrid(" (othtypmast.othtypcode)", "ASC")

        End Select
    End Sub

    Private Function ExportWithOrderByValues() As String
        ExportWithOrderByValues = ""
        Select Case ddlOrderBy.SelectedIndex
            Case 0
                ExportWithOrderByValues = "(excursions_header.adddate) DESC"
            Case 1
                ExportWithOrderByValues = "(excursions_header.excid) ASC"
            Case 2
                ExportWithOrderByValues = "(othtypmast.othtypcode) ASC"

        End Select
    End Function

    Protected Sub cmdhelp_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=MainStopSalesSearch','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub

    Protected Sub ddlOrderBy_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlOrderBy.SelectedIndexChanged
        FillGridWithOrderByValues()
    End Sub


    Protected Sub btnReport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReport.Click
        Try

            Dim strpop As String = ""
            Dim strtemp As String = ""
            Dim MyDsCompanyDetails As DataSet
            Dim conm, coadd1, coadd2, copobox, cotel, cofax, coemail, coweb As String
            MyDsCompanyDetails = objUtils.GetDataFromDatasetnew(Session("dbconnectionName"), "select conm,coadd1,coadd2,copobox,cotel,cofax,coemail,coweb from columbusmaster")

            If MyDsCompanyDetails.Tables(0).Rows.Count > 0 Then
                conm = MyDsCompanyDetails.Tables(0).Rows(0).Item("conm")
                coadd1 = MyDsCompanyDetails.Tables(0).Rows(0).Item("coadd1")
                coadd2 = MyDsCompanyDetails.Tables(0).Rows(0).Item("coadd2")
                copobox = MyDsCompanyDetails.Tables(0).Rows(0).Item("copobox")
                cotel = MyDsCompanyDetails.Tables(0).Rows(0).Item("cotel")
                cofax = MyDsCompanyDetails.Tables(0).Rows(0).Item("cofax")
                coemail = MyDsCompanyDetails.Tables(0).Rows(0).Item("coemail")
                coweb = MyDsCompanyDetails.Tables(0).Rows(0).Item("coweb")
            End If


            If rbNormalRequest.Checked = True Then
                'strpop = "window.open('rptReportNew.aspx?Pageame=Ex_NormalRequest&BackPageName=DeskExcursionsRequestSearch.aspx&Refcode=" & Session("Excursion_ID") & "','RepPrint','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                strpop = "window.open('rptReportNew.aspx?Pageame=Ex_NormalRequest&BackPageName=DeskExcursionsRequestSearch.aspx&Refcode=" & Session("Excursion_ID") & "','RepPrint');"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
            End If

            If rbBurjAlArabRequest.Checked = True Then
                'strpop = "window.open('rptReportNew.aspx?Pageame=Ex_BurjAlArabRequest&BackPageName=DeskExcursionsRequestSearch.aspx&Refcode=" & Session("Excursion_ID") & "&pcotel=" & cotel & "&pcofax=" & cofax & "&pcoemail=" & coemail & "&pcoweb=" & coweb & "&pcoadd2=" & coadd2 & "&pcocmt=" & strtemp & "','RepPrint','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                strpop = "window.open('rptReportNew.aspx?Pageame=Ex_BurjAlArabRequest&BackPageName=DeskExcursionsRequestSearch.aspx&Refcode=" & Session("Excursion_ID") & "&pcotel=" & cotel & "&pcofax=" & cofax & "&pcoemail=" & coemail & "&pcoweb=" & coweb & "&pcoadd2=" & coadd2 & "&pcocmt=" & strtemp & "','RepPrint');"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
            End If


            If rbDowCruiseRequest.Checked = True Then
                ' strpop = "window.open('rptReportNew.aspx?Pageame=Ex_DowCruiseRequest&BackPageName=DeskExcursionsRequestSearch.aspx&Refcode=" & Session("Excursion_ID") & "&pcotel=" & cotel & "&pcofax=" & cofax & "&pcoemail=" & coemail & "&pcoweb=" & coweb & "&pcoadd2=" & coadd2 & "&pcocmt=" & strtemp & "','RepPrint','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                strpop = "window.open('rptReportNew.aspx?Pageame=Ex_DowCruiseRequest&BackPageName=DeskExcursionsRequestSearch.aspx&Refcode=" & Session("Excursion_ID") & "&pcotel=" & cotel & "&pcofax=" & cofax & "&pcoemail=" & coemail & "&pcoweb=" & coweb & "&pcoadd2=" & coadd2 & "&pcocmt=" & strtemp & "','RepPrint');"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
            End If


            If rbRentCarRequest.Checked = True Then
                ' strpop = "window.open('rptReportNew.aspx?Pageame=Ex_RentCarRequest&BackPageName=DeskExcursionsRequestSearch.aspx&Refcode=" & Session("Excursion_ID") & "&pcotel=" & cotel & "&pcofax=" & cofax & "&pcoemail=" & coemail & "&pcoweb=" & coweb & "&pcoadd2=" & coadd2 & "&pcocmt=" & strtemp & "','RepPrint','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                strpop = "window.open('rptReportNew.aspx?Pageame=Ex_RentCarRequest&BackPageName=DeskExcursionsRequestSearch.aspx&Refcode=" & Session("Excursion_ID") & "&pcotel=" & cotel & "&pcofax=" & cofax & "&pcoemail=" & coemail & "&pcoweb=" & coweb & "&pcoadd2=" & coadd2 & "&pcocmt=" & strtemp & "','RepPrint');"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
            End If


            If radexcdet.Checked = True Then
                'strpop = "window.open('rptReportNew.aspx?Pageame=Ex_det&BackPageName=DeskExcursionsRequestSearch.aspx&Refcode=" & Session("Excursion_ID") & "&rlineno=" & Session("Rlineno") & "&pcotel=" & cotel & "&pcofax=" & cofax & "&pcoemail=" & coemail & "&pcoweb=" & coweb & "&pcoadd2=" & coadd2 & "&pcocmt=" & strtemp & "','RepPrint','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                strpop = "window.open('rptReportNew.aspx?Pageame=Ex_det&BackPageName=DeskExcursionsRequestSearch.aspx&Refcode=" & Session("Excursion_ID") & "&rlineno=" & Session("Rlineno") & "&pcotel=" & cotel & "&pcofax=" & cofax & "&pcoemail=" & coemail & "&pcoweb=" & coweb & "&pcoadd2=" & coadd2 & "&pcocmt=" & strtemp & "','RepPrint');"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
            End If


            If rbTicket.Checked = True Then
                ' strpop = "window.open('rptReportNew.aspx?Pageame=Ex_Ticket1&BackPageName=DeskExcursionsRequestSearch.aspx&Refcode=" & Session("Excursion_ID") & "&rlineno=" & Session("Rlineno") & "&pcotel=" & cotel & "&pcofax=" & cofax & "&pcoemail=" & coemail & "&pcoweb=" & coweb & "&pcopobox=" & strtemp & "&pcoadd1=" & coadd1 & "&pcoadd2=" & coadd2 & "','RepPrint','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                strpop = "window.open('rptReportNew.aspx?Pageame=Ex_Ticket1&BackPageName=DeskExcursionsRequestSearch.aspx&Refcode=" & Session("Excursion_ID") & "&rlineno=" & Session("Rlineno") & "&pcotel=" & cotel & "&pcofax=" & cofax & "&pcoemail=" & coemail & "&pcoweb=" & coweb & "&pcopobox=" & strtemp & "&pcoadd1=" & coadd1 & "&pcoadd2=" & coadd2 & "','RepPrint');"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
            End If

            If rbInvoice.Checked = True Then
                'strpop = "window.open('rptReportNew.aspx?Pageame=Ex_Invoice&BackPageName=DeskExcursionsRequestSearch.aspx&tranid=" & Session("Excursion_ID") & "&trantype=" & strtemp & "&amtinwrds=" & strtemp & "&divid=" & strtemp & "&rephead=" & strtemp & "&repfilter=" & strtemp & "&pcoweb=" & coweb & "&pcoemail=" & coemail & "&pcofax=" & cofax & "&pcotel=" & cotel & "','RepPrint','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                strpop = "window.open('rptReportNew.aspx?Pageame=Ex_Invoice&BackPageName=DeskExcursionsRequestSearch.aspx&tranid=" & Session("Excursion_ID") & "&trantype=" & strtemp & "&amtinwrds=" & strtemp & "&divid=" & strtemp & "&rephead=" & strtemp & "&repfilter=" & strtemp & "&pcoweb=" & coweb & "&pcoemail=" & coemail & "&pcofax=" & cofax & "&pcotel=" & cotel & "','RepPrint');"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
            End If

            If rbproforma.Checked = True Then
                'strpop = "window.open('rptReportNew.aspx?Pageame=Ex_ProformaInvoice&BackPageName=DeskExcursionsRequestSearch.aspx&tranid=" & Session("Excursion_ID") & "&trantype=" & strtemp & "&amtinwrds=" & strtemp & "&divid=" & strtemp & "&rephead=" & strtemp & "&repfilter=" & strtemp & "&pcoweb=" & coweb & "&pcoemail=" & coemail & "&pcofax=" & cofax & "&pcotel=" & cotel & "','RepPrint','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                strpop = "window.open('rptReportNew.aspx?Pageame=Ex_ProformaInvoice&BackPageName=DeskExcursionsRequestSearch.aspx&tranid=" & Session("Excursion_ID") & "&trantype=" & strtemp & "&amtinwrds=" & strtemp & "&divid=" & strtemp & "&rephead=" & strtemp & "&repfilter=" & strtemp & "&pcoweb=" & coweb & "&pcoemail=" & coemail & "&pcofax=" & cofax & "&pcotel=" & cotel & "','RepPrint');"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
            End If

            'If rbemail.Checked = True Then
            '    strpop = "window.open('rptReportNew.aspx?Pageame=Ex_Email&BackPageName=DeskExcursionsRequestSearch.aspx&tranid=" & Session("Excursion_ID") & "&trantype=" & strtemp & "&amtinwrds=" & strtemp & "&divid=" & strtemp & "&rephead=" & strtemp & "&repfilter=" & strtemp & "&pcoweb=" & coweb & "&pcoemail=" & coemail & "&pcofax=" & cofax & "&pcotel=" & cotel & "','RepPrint','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
            '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
            'End If


        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("PrintPopUp.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))

        End Try

    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        divExcursion.Style("display") = "none"
    End Sub

End Class
