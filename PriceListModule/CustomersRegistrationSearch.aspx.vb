'------------================--------------=======================------------------================
'   Module Name    :    CustomerSearch.aspx
'   Developer Name :    Indulkar Sandeep
'   Date           :   
'   
'
'------------================--------------=======================------------------================
Imports System.Data
Imports System.Data.SqlClient

Partial Class CustomersRegistrationSearch
    Inherits System.Web.UI.Page

#Region "Global Declaration"
    Dim objUtils As New clsUtils
    Dim objUser As New clsUser
    Dim strSqlQry As String
    Dim strWhereCond As String
    Dim SqlConn As SqlConnection
    Dim myCommand As SqlCommand
    Dim myDataAdapter As SqlDataAdapter
#End Region

#Region "Enum GridCol"
    Enum GridCol
        agentname = 0
        webusername = 1
        contact1 = 2
        designation = 3
        ctrycode = 4
        citycode = 5
        add1 = 6
        tel1 = 7
        fax = 8
        email = 9
        registerdate = 10
        Edit = 0
        Delete = 0
        
        Approve = 14
        View = 15
        print = 16
    End Enum
#End Region

    Protected Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
        If Page.IsPostBack = False Then
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
                    Case Else
                        Me.MasterPageFile = "~/SubPageMaster.master"
                End Select
            Else
                Me.MasterPageFile = "~/SubPageMaster.master"
            End If
        End If
    End Sub



#Region "Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Page.IsPostBack = False Then
            'Page.Header.Title = "General"
            Try

                SetFocus(txtcustomername)
                Dim AppId As HiddenField = CType(Master.FindControl("hdnAppId"), HiddenField)
                Dim AppName As HiddenField = CType(Master.FindControl("hdnAppName"), HiddenField)
                Dim strappid As String = ""
                Dim strappname As String = ""
                If AppId Is Nothing = False Then
                    strappid = AppId.Value
                End If
                If AppName Is Nothing = False Then
                    strappname = AppName.Value
                End If

                txtconnection.Value = Session("dbconnectionName")

                If CType(Session("GlobalUserName"), String) = Nothing Or CType(Session("Userpwd"), String) = Nothing Then
                    Response.Redirect("~/Login.aspx", False)
                    Exit Sub
                Else
                    objUser.CheckUserRight(Session("dbconnectionName"), CType(Session("GlobalUserName"), String), CType(Session("Userpwd"), String), _
                                                       CType(strappname, String), "PriceListModule\CustomersRegistrationSearch.aspx?appid=" + strappid, btnAddNew, btnExportToExcel, _
                                                       btnPrint, gv_SearchResult, Nothing, Nothing, GridCol.View, GridCol.print, 0, 0, 0, 0, GridCol.Approve)
                End If
                'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"),ddlctrycodeName, "ctryname", "select ctryname from ctrymast where active=1 order by ctryname", True)
                'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"),ddlCurrencyName, "currname", "select currname from currmast where active=1 order by currname", True)


                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCountryCode, "country", "country", "select country from webcountries   order by country", True)
                'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCountryName, "ctryname", "ctrycode", "select ctryname,ctrycode from ctrymast where active=1  order by ctryname", True)
                ''objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"),ddlctrycode, "ctrycode", "ctryname", "select ctrycode,ctryname from ctrymast where active=1  order by ctrycode", True)
                'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"),ddlctryname, "ctryname", "ctrycode", "select ctryname,ctrycode from ctrymast where active=1  order by ctryname", True)
                'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"),ddlCitycode, "citycode", "cityname", "select citycode,cityname from citymast where active=1  order by citycode", True)
                'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"),ddlCityName, "cityname", "citycode", "select cityname,citycode from citymast where active=1  order by cityname", True)


                Session.Add("strsortdirection", SortDirection.Ascending)
                FillDDL()
                fillorderby()
                FillGrid("regno", "DESC")

                txtFromDate.Text = ""
                txtTodate.Text = ""

                Dim typ As Type
                typ = GetType(DropDownList)

                If (Page.ClientScript.IsClientScriptIncludeRegistered(typ, "TADDScript.js")) = False Then
                    Page.ClientScript.RegisterClientScriptInclude(typ, "TADDScript.js", "TADDScript.js")

                    ddlCountryCode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ' ddlCountryName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")

                    ddlCitycode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    'ddlctrycodeName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")

                End If
            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objUtils.WritErrorLog("CustomersSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            End Try
        Else
            Try
                If ddlCountryCode.Value <> "[Select]" Then
                    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCitycode, "city", "id", "select city,id  from webcities where country='" & ddlCountryCode.Value & "'  order by id", True, ddlCitycode.Value)
                    ' objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCityName, "cityname", "citycode", "select cityname,citycode  from citymast where ctrycode='" & ddlCountryName.Value & "' and active=1 order by cityname", True, ddlCityName.Value)
                Else
                    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCitycode, "city", "id", "select id,city from webcities   order by id", True, ddlCitycode.Value)
                    ' objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCityName, "cityname", "citycode", "select cityname,citycode from citymast where active=1  order by citycode", True, ddlCityName.Value)
                End If

            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objUtils.WritErrorLog("CustomersSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            End Try
        End If
        btnClear.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to clear search criteria?')==false)return false;")

        ClientScript.GetPostBackEventReference(Me, String.Empty)
        If (Me.IsPostBack And Me.Request("__EVENTTARGET") = "CustomersWindowPostBack") Then
            btnSearch_Click(sender, e)
        End If
        btnPrint.Visible = False
    End Sub
#End Region

#Region "Public Function FormatDate()"
    Public Function FormatDate(ByVal obj As Object) As String
        If Not (obj Is Nothing) Then
            Return CType(obj.ToString(), Date).ToShortDateString()
        Else
            Return ""
        End If
    End Function
#End Region


#Region " Private Sub FillDDL()"
    Private Sub FillDDL()
        strSqlQry = ""
        strSqlQry = "SELECT id,city FROM webcities ORDER BY id"
        objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCitycode, "city", "id", strSqlQry, True)
        strSqlQry = ""
        'strSqlQry = "SELECT cityname,citycode, FROM citymast WHERE active=1 ORDER BY cityname"
        'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCityName, "citycode", "cityname", strSqlQry, True)
        'strSqlQry = ""


    End Sub
#End Region

#Region "Private Function BuildCondition() As String"
    Private Function BuildCondition() As String
        strWhereCond = ""

        If txtFromDate.Text <> "" And txtTodate.Text <> "" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = "( (convert(varchar(10),registration.registerdate,111) between convert(varchar(10), '" & Format(CType(txtFromDate.Text, Date), "yyyy/MM/dd") & "',111) " _
                & "  and  convert(varchar(10), '" & Format(CType(txtTodate.Text, Date), "yyyy/MM/dd") & "',111) ) " _
                & " or (convert(varchar(10),registration.registerdate,111) < convert(varchar(10), '" & Format(CType(txtFromDate.Text, Date), "yyyy/MM/dd") & "',111) " _
                & "  and convert(varchar(10),registration.registerdate,111) > convert(varchar(10), '" & Format(CType(txtTodate.Text, Date), "yyyy/MM/dd") & "',111)) )"
            End If
        End If

        If txtcustomername.Text.Trim <> "" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = " upper(registration.agentname) LIKE '" & Trim(txtcustomername.Text.Trim.ToUpper) & "%'"
            Else
                strWhereCond = strWhereCond & " AND upper(registration.agentname) LIKE '" & Trim(txtcustomername.Text.Trim.ToUpper) & "%'"
            End If
        End If


        If txtregno.Text.Trim <> "" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = " registration.regno LIKE '" & Trim(txtregno.Text.Trim.ToUpper) & "%'"
            Else
                strWhereCond = strWhereCond & " AND registration.regno LIKE '" & Trim(txtregno.Text.Trim.ToUpper) & "%'"
            End If
        End If

        If ddlStatus.Value = "A" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = strWhereCond & " registration.approve=1"
            Else
                strWhereCond = strWhereCond & " AND registration.approve=1"
            End If

        ElseIf ddlStatus.Value = "U" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = strWhereCond & "  registration.approve=0"
            Else
                strWhereCond = strWhereCond & " AND registration.approve=0"
            End If
        End If

        If ddlCitycode.Value <> "[Select]" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = " upper(registration.citycode) = '" & Trim(ddlCitycode.Value) & "'"
            Else
                strWhereCond = strWhereCond & " AND upper(registration.citycode) = '" & Trim(ddlCitycode.Value) & "'"
            End If
        End If
        If ddlCountryCode.Value <> "[Select]" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = " upper(registration.ctrycode) = '" & Trim(ddlCountryCode.Items(ddlCountryCode.SelectedIndex).Text) & "'"
            Else
                strWhereCond = strWhereCond & " AND upper(registration.ctrycode) = '" & Trim(ddlCountryCode.Items(ddlCountryCode.SelectedIndex).Text) & "'"
            End If
        End If

        BuildCondition = strWhereCond
    End Function
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
            strSqlQry = "select regno,agentname,webusername,contact1,designation,ctrycode,citycode,add1,tel1,tel2,fax,email,registerdate,case approve when 0 then 'Unapproved' else 'Approved' end approve from registration "


            If Trim(BuildCondition) <> "" Then
                strSqlQry = strSqlQry & " WHERE " & BuildCondition() & " ORDER BY " & strorderby & " " & strsortorder
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
                lblMsg.Text = "Records not found. Please redefine search criteria."
            End If

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("CustomersSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
            clsDBConnect.dbConnectionClose(SqlConn)                          'Close connection
        End Try
    End Sub
#End Region

#Region "Protected Sub btnAddNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddNew.Click"
    Protected Sub btnAddNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddNew.Click
        Session.Add("custState", "New")
        'Response.Redirect("CustMainDet.aspx", False)

        Dim strpop As String = ""
        strpop = "window.open('CustMainDet.aspx?appid=" + CType(Request.QueryString("appid"), String) + "','CustMainDet','width='+screen.availWidth+' height='+screen.availHeight+' left=0,top=0 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)

    End Sub
#End Region

#Region "Protected Sub gv_SearchResult_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gv_SearchResult.PageIndexChanging"
    Protected Sub gv_SearchResult_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gv_SearchResult.PageIndexChanging
        gv_SearchResult.PageIndex = e.NewPageIndex
        'FillGrid("agentcode")
        Select Case ddlOrderBy.SelectedIndex
            Case 0
                FillGrid("registration.regno", "DESC")
            Case 1
                FillGrid("registration.agentname")
            Case 1
                FillGrid("registration.ctrycode")
            Case 2
                FillGrid("registration.citycode")
        End Select
    End Sub
#End Region



#Region "Protected Sub gv_SearchResult_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gv_SearchResult.RowCommand"
    Protected Sub gv_SearchResult_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gv_SearchResult.RowCommand
        Try

            If e.CommandName = "Page" Then Exit Sub

            Dim lblregno As Label
            Dim lblapprove As Label

            lblregno = gv_SearchResult.Rows(CType(e.CommandArgument.ToString, String)).FindControl("lblregno")
            lblapprove = gv_SearchResult.Rows(CType(e.CommandArgument.ToString, String)).FindControl("lblapprove")

            If e.CommandName = "Addrow" Then
                If lblapprove.Text = "Approved" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Registration form Already approved' );", True)
                    Exit Sub
                End If


                Session.Add("custState", "Addclient")
                Session.Add("regno", CType(lblregno.Text.Trim, String))

                ''Response.Redirect("Customers.aspx", False)

                Dim strpop As String = ""
                strpop = "window.open('CustMainDet.aspx?appid=" + CType(Request.QueryString("appid"), String) + "','CustMainDet','width='+screen.availWidth+' height='+screen.availHeight+' left=0,top=0 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
            ElseIf e.CommandName = "View" Then
                Session.Add("custState", "View")
                Session.Add("lblregno", CType(lblregno.Text.Trim, String))

                ' Session.Add("custRefCode", CType(lblagentname.Text.Trim, String))

                Dim strpop As String = ""
                Dim t As String = System.AppDomain.CurrentDomain.BaseDirectory
                't = 
                '' strpop = "window.open('" & t & "Content\ Registrationform.aspx ?regno=" + CType(lblregno.Text.Trim, String) + "','CustMainDet','width='+screen.availWidth+' height='+screen.availHeight+' left=0,top=0 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                strpop = "window.open('../PriceListModule/registrationform.aspx?regno=" + CType(lblregno.Text.Trim, String) + "','CustMainDet','width='+screen.availWidth+' height='+screen.availHeight+' left=0,top=0 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"

                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
            ElseIf e.CommandName = "Print" Then
                Session.Add("custState", "Print")
                Session.Add("lblregno", CType(lblregno.Text.Trim, String))

                ' Session.Add("custRefCode", CType(lblagentname.Text.Trim, String))

                Dim strpop As String = ""
                strpop = "window.open('rptRgprint.aspx?regno=" + CType(lblregno.Text.Trim, String) + "','CustMainDet','width='+screen.availWidth+' height='+screen.availHeight+' left=0,top=0 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)

            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("CustMainDet.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#End Region

#Region "Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click"
    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        'FillGrid("agentcode")
        Select Case ddlOrderBy.SelectedIndex
            Case 0
                FillGrid("registration.regno", "DESC")
            Case 1
                FillGrid("registration.agentname")
            Case 1
                FillGrid("registration.ctrycode")
            Case 2
                FillGrid("registration.citycode")
        End Select
    End Sub
#End Region

#Region "Protected Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClear.Click"
    Protected Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClear.Click

        txtcustomername.Text = ""
        ddlCitycode.Value = "[Select]"
        ddlCountryCode.Value = "[Select]"
        'ddlCountryName.Value = "[Select]"
        'ddlCityName.Value = "[Select]"
        txtFromDate.Text = ""
        txtTodate.Text = ""
        'FillGrid("agentcode")
        ddlOrderBy.SelectedIndex = 0
        FillGrid("regno", "DESC")

    End Sub
#End Region

#Region "Protected Sub gv_SearchResult_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles gv_SearchResult.RowDeleting"

    Protected Sub gv_SearchResult_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gv_SearchResult.RowDataBound



        'If e.Row.RowType = DataControlRowType.DataRow Then
        '    Dim lnkapprove As LinkButton = CType(e.Row.FindControl("approve"), LinkButton)
        '    Dim lblStatus As Label = CType(e.Row.FindControl("lblStatus"), Label)
        '    If UCase(lblStatus.Text) = UCase("Approved") Then
        '        lnkapprove.Enabled = False
        '    End If


        'End If

    End Sub

    Protected Sub gv_SearchResult_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles gv_SearchResult.RowDeleting

    End Sub
#End Region

#Region "Protected Sub gv_SearchResult_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles gv_SearchResult.RowEditing"
    Protected Sub gv_SearchResult_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles gv_SearchResult.RowEditing

    End Sub
#End Region

#Region "Protected Sub gv_SearchResult_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gv_SearchResult.Sorting"

    Protected Sub gv_SearchResult_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gv_SearchResult.Sorting
        Session.Add("strsortexpression", e.SortExpression)
        SortGridColoumn_click()
    End Sub
#End Region

#Region " Public Sub SortGridColoumn_click()"
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
                strSqlQry = "select registration.regno as [RegNo],registration.webusername as [UserId],registration.contact1 as " & _
                " [Contact Person1],registration.contact2 as [Contact Person2]," & _
                "registration.designation as [Designation],registration.agentname as [FirstName],registration.iatano as [IATA No],registration.add1 as [Address1]," & _
                "registration.add2 as [Address2],registration.add3 as [Address3],cry.ctryname as [Country],cty.cityname as [City],tel1 as [Tel1]," & _
                "registration.tel2 as [Tel2],registration.fax as [Fax],registration.email as [E-Mail], registration.approve as [Approved],registration.registerdate as " & _
                "[Reg Date],registration.approvedate as [Approved Date],registration.approveuser as [Approved User]," & _
                " registration.catcode as [CatCode],registration.crlimit as [CategoryCode],registration.currcode as [Currancy]" & _
                "from registration registration left outer join ctrymast cry on registration.ctrycode=cry.ctrycode" & _
                " left outer join citymast cty on registration.citycode=cty.citycode"

                If Trim(BuildCondition) <> "" Then
                    strSqlQry = strSqlQry & " WHERE " & BuildCondition() & " ORDER BY registration.webusername"
                Else
                    strSqlQry = strSqlQry & " ORDER BY registration.webusername"
                End If
                con = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))

                DA = New SqlDataAdapter(strSqlQry, con)
                DA.Fill(DS, "agentmast")

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

#Region "Protected Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrint.Click"
    Protected Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        Try
            Dim strReportTitle As String = ""
            Dim strSelectionFormula As String = ""
            Dim strpop As String = ""
            Dim strcustomer, strfromdate, strtodate, struserid As String
            Dim strregno, strstatus, strcitycode, strcountry As String

            strfromdate = ""
            strtodate = ""

            If txtFromDate.Text <> "" Then
                strfromdate = Format(CType(txtFromDate.Text, Date), "yyyy-MM-dd 00:00:00 ")
            End If
            If txtTodate.Text <> "" Then
                strtodate = Format(CType(txtTodate.Text, Date), "yyyy-MM-dd 00:00:00 ")
            End If

            strcustomer = txtcustomername.Text.Trim

            strregno = txtregno.Text.Trim
            strstatus = IIf(UCase(ddlStatus.Items(ddlStatus.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlStatus.Value, "")
            strcitycode = IIf(UCase(ddlCitycode.Items(ddlCitycode.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlCitycode.Items(ddlCitycode.SelectedIndex).Text, "")
            strcountry = IIf(UCase(ddlCountryCode.Items(ddlCountryCode.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlCountryCode.Items(ddlCountryCode.SelectedIndex).Text, "")

            strpop = "window.open('rptReportNew.aspx?Pageame=clientregistration&BackPageName=CustomersRegistrationSearch.aspx&userid=" & struserid & "&fromdate=" & strfromdate & "&todate=" & strtodate & "&regno=" & strregno & "&status=" & strstatus & "&CustName=" & strcustomer & "&CtryCode=" & strcountry & "&CityCode=" & strcitycode & "','RepSupAgent','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)


        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("CustomersSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#End Region

#Region " Protected Sub rbtnadsearch_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)"
    'Protected Sub rbtnadsearch_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
    '    PnlCustomer.Visible = True
    'End Sub
#End Region

#Region " Protected Sub rbnadsearch_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)"
    Protected Sub rbnadsearch_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        'PnlCustomer.Visible = True
        lblcity.Visible = True
        lblCtryCode.Visible = True
        'lblCityName.Visible = True
        'lblCtryName.Visible = True
        ddlCitycode.Visible = True
        'ddlctrycode.Visible = True
        'ddlctryname.Visible = True
        'ddlCityName.Visible = True
        'ddlctrycode.Visible = True
        'ddlctryname.Visible = True
        ddlCountryCode.Visible = True
        ' ddlCountryName.Visible = True



    End Sub
#End Region

#Region " Protected Sub rbsearch_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)"
    Protected Sub rbsearch_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        'PnlCustomer.Visible = False
        lblcity.Visible = False
        lblCtryCode.Visible = False
        ' lblCityName.Visible = False
        'lblCtryName.Visible = False
        ddlCitycode.Visible = False
        ddlCountryCode.Visible = False
        ' ddlCityName.Visible = False
        'ddlCountryName.Visible = False
    End Sub
#End Region
    Private Sub fillorderby()
        ddlOrderBy.Items.Clear()
        ddlOrderBy.Items.Add("Registration no.")
        ddlOrderBy.Items.Add("Clinet Name")
        ddlOrderBy.Items.Add("Country Code")
        ddlOrderBy.Items.Add("City Code")
        ddlOrderBy.SelectedIndex = 0
    End Sub

    Protected Sub ddlOrderBy_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlOrderBy.SelectedIndexChanged
        Select Case ddlOrderBy.SelectedIndex
            Case 0
                FillGrid("registration.regno", "DESC")
            Case 1
                FillGrid("registration.webusername")
            Case 1
                FillGrid("registration.ctrycode")
            Case 2
                FillGrid("registration.citycode")
        End Select
    End Sub

    Protected Sub btnHelp_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=CustomersSearch','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub

    Protected Sub ImgBtnFrmDt_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImgBtnFrmDt.Click

    End Sub
End Class
