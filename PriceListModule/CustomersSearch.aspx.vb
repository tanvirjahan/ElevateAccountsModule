'------------================--------------=======================------------------================
'   Module Name    :    CustomerSearch.aspx
'   Developer Name :    Indulkar Sandeep
'   Date           :   
'   
'
'------------================--------------=======================------------------================
Imports System.Data
Imports System.Data.SqlClient

Partial Class CustomersSearch
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
        CustomerCodeTCol = 0
        CustomerCode = 1
        CustomerName = 2
        CategoryCode = 3
        'SellingType = 4
        'OtherSellingType = 5
        Country = 4
        city = 5
        'Market = 8
        AddressLine1 = 6
        AddressLine2 = 7
        AddressLine3 = 8
        Telephone = 9
        Fax = 10
        ControlAcCode = 11
        CreditDays = 12
        CreditLimit = 13
        Active = 14
        DateCreated = 15
        UserCreated = 16
        DateModified = 17
        UserModified = 18
        Edit = 19
        View = 20
        Delete = 21
        Copy = 22




    End Enum
#End Region

    Protected Sub Page_InitComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.InitComplete

    End Sub
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
                    Case 10
                        Me.MasterPageFile = "~/TransferMaster.master"
                    Case 11
                        Me.MasterPageFile = "~/ExcursionMaster.master"
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
        Me.Title = "Customers "
        If Page.IsPostBack = False Then
            'Page.Header.Title = "General"
            Try
                SetFocus(txtcustomercode)
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
                                                       CType(strappname, String), "PriceListModule\CustomersSearch.aspx?appid=" + strappid, btnAddNew, btnExportToExcel, _
                                                       btnPrint, gv_SearchResult, GridCol.Edit, GridCol.Delete, GridCol.View)

                End If
                'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"),ddlctrycodeName, "ctryname", "select ctryname from ctrymast where active=1 order by ctryname", True)
                'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"),ddlCurrencyName, "currname", "select currname from currmast where active=1 order by currname", True)


                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCountryCode, "ctrycode", "ctryname", "select ctrycode,ctryname from ctrymast where active=1  order by ctrycode", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCountryName, "ctryname", "ctrycode", "select ctryname,ctrycode from ctrymast where active=1  order by ctryname", True)
                'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"),ddlctrycode, "ctrycode", "ctryname", "select ctrycode,ctryname from ctrymast where active=1  order by ctrycode", True)
                'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"),ddlctryname, "ctryname", "ctrycode", "select ctryname,ctrycode from ctrymast where active=1  order by ctryname", True)
                'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"),ddlCitycode, "citycode", "cityname", "select citycode,cityname from citymast where active=1  order by citycode", True)
                'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"),ddlCityName, "cityname", "citycode", "select cityname,citycode from citymast where active=1  order by cityname", True)


                Session.Add("strsortExpression", "currmast.currcode")
                Session.Add("strsortdirection", SortDirection.Ascending)
                FillDDL()
                fillorderby()
                FillGrid("agentmast.agentname")

                Dim typ As Type
                typ = GetType(DropDownList)

                If (Page.ClientScript.IsClientScriptIncludeRegistered(typ, "TADDScript.js")) = False Then
                    Page.ClientScript.RegisterClientScriptInclude(typ, "TADDScript.js", "TADDScript.js")

                    ddlSellingType.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlTicketSelling.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlOtherSellingType.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlCountryCode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlCountryName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlSalesPerson.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlSalesPersonName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")

                    ddlMarket.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlCitycode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlCategory.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    'ddlctrycodeName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")

                End If
            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objUtils.WritErrorLog("CustomersSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            End Try
        Else
            Try
                If txtCutomerNameHidden.Text <> "" Then
                    If txtcustomername.Text <> "" Then
                        txtcustomername.Text = txtCutomerNameHidden.Text
                        txtcustomercode.Text = txtCutomerCodeHidden.Text
                        txtCutomerNameHidden.Text = ""
                        txtCutomerCodeHidden.Text = ""
                    Else
                        txtcustomername.Text = ""
                        txtcustomercode.Text = ""
                    End If
                End If


                If ddlCountryCode.Value <> "[Select]" Then
                    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCitycode, "citycode", "cityname", "select citycode,cityname  from citymast where ctrycode='" & ddlCountryName.Value & "' and active=1 order by citycode", True, ddlCitycode.Value)
                    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCityName, "cityname", "citycode", "select cityname,citycode  from citymast where ctrycode='" & ddlCountryName.Value & "' and active=1 order by cityname", True, ddlCityName.Value)
                Else
                    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCitycode, "citycode", "cityname", "select citycode,cityname from citymast where active=1  order by cityname", True, ddlCitycode.Value)
                    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCityName, "cityname", "citycode", "select cityname,citycode from citymast where active=1  order by citycode", True, ddlCityName.Value)
                End If

            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objUtils.WritErrorLog("CustomersSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            End Try

        End If
        btnClear.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to clear search criteria?')==false)return false;")
        'Dim custid, custname As String




        ClientScript.GetPostBackEventReference(Me, String.Empty)
        If (Me.IsPostBack And Me.Request("__EVENTTARGET") = "CustomersWindowPostBack") Then
            btnSearch_Click(sender, e)
        End If

    End Sub
#End Region

#Region " Private Sub FillDDL()"
    Private Sub FillDDL()
        strSqlQry = ""
        strSqlQry = "SELECT citycode,cityname FROM citymast WHERE active=1 ORDER BY citycode"
        objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCitycode, "citycode", "cityname", strSqlQry, True)
        strSqlQry = ""
        strSqlQry = "SELECT cityname,citycode, FROM citymast WHERE active=1 ORDER BY cityname"
        objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCityName, "citycode", "cityname", strSqlQry, True)
        ' strSqlQry = ""
        ' strSqlQry = "SELECT ctrycode,ctryname FROM ctrymast WHERE active=1 ORDER BY ctrycode"
        'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"),ddlctrycode, "ctrycode", "ctryname", strSqlQry, True)
        'strSqlQry = ""
        'strSqlQry = "SELECT ctryname,ctrycode FROM ctrymast WHERE active=1 ORDER BY ctryname"
        'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"),ddlctryname, "ctrycode", "ctryname", strSqlQry, True)
        'strSqlQry = ""
        strSqlQry = "SELECT plgrpcode FROM plgrpmast WHERE active=1 ORDER BY plgrpcode"
        objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlMarket, "plgrpcode", "plgrpcode", strSqlQry, True)
        strSqlQry = ""
        strSqlQry = "SELECT othsellcode FROM othsellmast WHERE active=1 ORDER BY othsellcode"
        objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlOtherSellingType, "othsellcode", "othsellcode", strSqlQry, True)
        strSqlQry = ""
        strSqlQry = "SELECT tktsellcode FROM tktsellmast WHERE active=1 ORDER BY tktsellcode"
        objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlTicketSelling, "tktsellcode", "tktsellcode", strSqlQry, True)
        strSqlQry = ""
        strSqlQry = "SELECT agentcatcode FROM agentcatmast WHERE active=1 ORDER BY agentcatcode"
        objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCategory, "agentcatcode", "agentcatcode", strSqlQry, True)
        strSqlQry = ""
        strSqlQry = "SELECT sellcode FROM sellmast WHERE active=1 ORDER BY sellcode"
        objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSellingType, "sellcode", "sellcode", strSqlQry, True)
        strSqlQry = ""
        strSqlQry = "select UserCode,UserName from UserMaster where active=1  order by UserCode"
        objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSalesPerson, "UserCode", "UserName", strSqlQry, True)
        strSqlQry = ""
        strSqlQry = "select UserCode,UserName from UserMaster where active=1  order by UserName"
        objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSalesPersonName, "UserName", "UserCode", strSqlQry, True)

    End Sub
#End Region

#Region "Private Function BuildCondition() As String"
    Private Function BuildCondition() As String
        strWhereCond = ""
        If txtcustomercode.Text.Trim <> "" Then
            If Trim(strWhereCond) = "" Then

                strWhereCond = " upper(agentmast.agentcode) LIKE '" & Trim(txtcustomercode.Text.Trim.ToUpper) & "%'"
            Else
                strWhereCond = strWhereCond & " AND upper(agentcode) LIKE '" & Trim(txtcustomercode.Text.Trim.ToUpper) & "%'"
            End If
        End If

        If txtcustomername.Text.Trim <> "" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = " upper(agentmast.shortname) LIKE '" & Trim(txtcustomername.Text.Trim.ToUpper) & "%' Or upper(agentmast.agentname) LIKE '" & Trim(txtcustomername.Text.Trim.ToUpper) & "%'"
            Else
                strWhereCond = strWhereCond & " AND upper(agentmast.shortname) LIKE '" & Trim(txtcustomername.Text.Trim.ToUpper) & "%' Or upper(agentmast.agentname) LIKE '" & Trim(txtcustomername.Text.Trim.ToUpper) & "%'"
            End If
        End If
        If ddlCategory.Value <> "[Select]" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = " upper(agentmast.catcode) = '" & Trim(ddlCategory.Items(ddlCategory.SelectedIndex).Text) & "'"
            Else
                strWhereCond = strWhereCond & " AND upper(agentmast.catcode) = '" & Trim(ddlCategory.Items(ddlCategory.SelectedIndex).Text) & "'"
            End If
        End If
        If ddlCitycode.Value <> "[Select]" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = " upper(agentmast.citycode) = '" & Trim(ddlCitycode.Items(ddlCitycode.SelectedIndex).Text) & "'"
            Else
                strWhereCond = strWhereCond & " AND upper(agentmast.citycode) = '" & Trim(ddlCitycode.Items(ddlCitycode.SelectedIndex).Text) & "'"
            End If
        End If
        If ddlCountryCode.Value <> "[Select]" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = " upper(agentmast.ctrycode) = '" & Trim(ddlCountryCode.Items(ddlCountryCode.SelectedIndex).Text) & "'"
            Else
                strWhereCond = strWhereCond & " AND upper(agentmast.ctrycode) = '" & Trim(ddlCountryCode.Items(ddlCountryCode.SelectedIndex).Text) & "'"
            End If
        End If

        If ddlSalesPerson.Value <> "[Select]" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = " upper(agentmast.spersoncode) = '" & Trim(ddlSalesPerson.Items(ddlSalesPerson.SelectedIndex).Text) & "'"
            Else
                strWhereCond = strWhereCond & " AND upper(agentmast.spersoncode) = '" & Trim(ddlSalesPerson.Items(ddlSalesPerson.SelectedIndex).Text) & "'"
            End If
        End If

        If ddlMarket.Value <> "[Select]" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = " upper(plgrpmast.plgrpcode) = '" & Trim(ddlMarket.Items(ddlMarket.SelectedIndex).Text) & "'"
            Else
                strWhereCond = strWhereCond & " AND upper(plgrpmast.plgrpcode) = '" & Trim(ddlMarket.Items(ddlMarket.SelectedIndex).Text) & "'"
            End If
        End If
        If ddlOtherSellingType.Value <> "[Select]" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = " upper(othsellmast.othsellcode) = '" & Trim(ddlOtherSellingType.Items(ddlOtherSellingType.SelectedIndex).Text) & "'"
            Else
                strWhereCond = strWhereCond & " AND upper(othsellmast.othsellcode) = '" & Trim(ddlOtherSellingType.Items(ddlOtherSellingType.SelectedIndex).Text) & "'"
            End If
        End If
        If ddlSellingType.Value <> "[Select]" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = " upper(sellmast.sellcode) = '" & Trim(ddlSellingType.Items(ddlSellingType.SelectedIndex).Text) & "'"
            Else
                strWhereCond = strWhereCond & " AND upper(sellmast.sellcode) = '" & Trim(ddlSellingType.Items(ddlSellingType.SelectedIndex).Text) & "'"
            End If
        End If
        If ddlTicketSelling.Value <> "[Select]" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = " upper(agentmast.tktsellcode) = '" & Trim(ddlTicketSelling.Items(ddlTicketSelling.SelectedIndex).Text) & "'"
            Else
                strWhereCond = strWhereCond & " AND upper(agentmast.tktsellcode) = '" & Trim(ddlTicketSelling.Items(ddlTicketSelling.SelectedIndex).Text) & "'"
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
            'strSqlQry = "select * from agentmast inner join agentcatmast ON agentmast.catcode = agentcatmast.agentcatcode" & _
            '            " INNER JOIN sellmast ON agentmast.sellcode = sellmast.sellcode left join othsellmast ON agentmast.othsellcode = othsellmast.othsellcode " & _
            '            " INNER JOIN  citymast ON agentmast.citycode =citymast.citycode inner join plgrpmast ON agentmast.plgrpcode = plgrpmast.plgrpcode " & _
            '            " INNER JOIN ctrymast on ctrymast.ctrycode = citymast.ctrycode"

            'Changed By Mohamed on 06/04/2016 as per Madam's email on 06/04/2016
            'CHANGED  strSqlQry = "select * from agentmast inner join agentcatmast ON agentmast.catcode = agentcatmast.agentcatcode"
            'CHANGED strSqlQry += " left JOIN sellmast ON agentmast.sellcode = sellmast.sellcode left join othsellmast ON agentmast.othsellcode = othsellmast.othsellcode "
            'CHANGED strSqlQry += " INNER JOIN  citymast ON agentmast.citycode =citymast.citycode inner join plgrpmast ON agentmast.plgrpcode = plgrpmast.plgrpcode "
            'CHANGED strSqlQry += " INNER JOIN ctrymast on ctrymast.ctrycode = citymast.ctrycode"
            strSqlQry = " SELECT AGENTCODE,AGENTNAME, CATCODE,CTRYCODE,CITYCODE,ADD1,ADD2,ADD3,TEL1,FAX,CONTROLACCTCODE,CRDAYS,CRLIMIT,ACTIVE,ADDDATE,ADDUSER,MODDATE,MODUSER FROM AGENTMAST"


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
        'strpop = "window.open('CustMainDet.aspx?appid=" + CType(Request.QueryString("appid"), String) + "','CustMainDet','width='+screen.availWidth+' height='+screen.availHeight+' left=0,top=0 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
        strpop = "window.open('CustMainDet.aspx?appid=" + CType(Request.QueryString("appid"), String) + "','CustMainDet');"
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)

    End Sub
#End Region

#Region "Protected Sub gv_SearchResult_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gv_SearchResult.PageIndexChanging"
    Protected Sub gv_SearchResult_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gv_SearchResult.PageIndexChanging
        gv_SearchResult.PageIndex = e.NewPageIndex
        'FillGrid("agentcode")
        Select Case ddlOrderBy.SelectedIndex
            Case 0
                FillGrid("agentmast.agentname")
            Case 1
                FillGrid("agentmast.agentcode")
            Case 2
                FillGrid("agentmast.catcode")
            Case 3
                FillGrid("sellmast.sellcode")
            Case 4
                FillGrid("ctrymast.ctrycode")
            Case 5
                FillGrid("plgrpmast.plgrpcode")
        End Select
    End Sub
#End Region

#Region "Protected Sub gv_SearchResult_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gv_SearchResult.RowCommand"
    Protected Sub gv_SearchResult_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gv_SearchResult.RowCommand
        Try

            If e.CommandName = "Page" Then Exit Sub

            Dim lblId As Label


            If e.CommandName = "Editrow" Then
                lblId = gv_SearchResult.Rows(CType(e.CommandArgument.ToString, String)).FindControl("lblCode")
                Session.Add("custState", "Edit")
                Session.Add("custRefCode", CType(lblId.Text.Trim, String))
                ''Response.Redirect("Customers.aspx", False)

                Dim strpop As String = ""
                'strpop = "window.open('CustMainDet.aspx?appid=" + CType(Request.QueryString("appid"), String) + "','CustMainDet','width='+screen.availWidth+' height='+screen.availHeight+' left=0,top=0 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                strpop = "window.open('CustMainDet.aspx?appid=" + CType(Request.QueryString("appid"), String) + "','CustMainDet');"

                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
            ElseIf e.CommandName = "View" Then
                lblId = gv_SearchResult.Rows(CType(e.CommandArgument.ToString, String)).FindControl("lblCode")
                Session.Add("custState", "View")
                Session.Add("custRefCode", CType(lblId.Text.Trim, String))

                Dim strpop As String = ""
                'strpop = "window.open('CustMainDet.aspx?appid=" + CType(Request.QueryString("appid"), String) + "','CustMainDet','width='+screen.availWidth+' height='+screen.availHeight+' left=0,top=0 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                strpop = "window.open('CustMainDet.aspx?appid=" + CType(Request.QueryString("appid"), String) + "','CustMainDet');"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
            ElseIf e.CommandName = "Deleterow" Then
                lblId = gv_SearchResult.Rows(CType(e.CommandArgument.ToString, String)).FindControl("lblCode")
                Session.Add("custState", "Delete")
                Session.Add("custRefCode", CType(lblId.Text.Trim, String))
                Dim strpop As String = ""
                'strpop = "window.open('CustMainDet.aspx?appid=" + CType(Request.QueryString("appid"), String) + "','CustMainDet','width='+screen.availWidth+' height='+screen.availHeight+' left=0,top=0 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                strpop = "window.open('CustMainDet.aspx?appid=" + CType(Request.QueryString("appid"), String) + "','CustMainDet');"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
            ElseIf e.CommandName = "Copy" Then
                lblId = gv_SearchResult.Rows(CType(e.CommandArgument.ToString, String)).FindControl("lblCode")
                Session.Add("custState", "Copy")
                Session.Add("custRefCode", CType(lblId.Text.Trim, String))
                ''Response.Redirect("Customers.aspx", False)
                Dim strpop As String = ""
                'strpop = "window.open('CustMainDet.aspx?appid=" + CType(Request.QueryString("appid"), String) + "','CustMainDet','width='+screen.availWidth+' height='+screen.availHeight+' left=0,top=0 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                strpop = "window.open('CustMainDet.aspx?appid=" + CType(Request.QueryString("appid"), String) + "','CustMainDet');"
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
                FillGrid("agentmast.agentname")
            Case 1
                FillGrid("agentmast.agentcode")
            Case 2
                FillGrid("agentmast.catcode")
            Case 3
                FillGrid("sellmast.sellcode")
            Case 4
                FillGrid("ctrymast.ctrycode")
            Case 5
                FillGrid("plgrpmast.plgrpcode")
        End Select
    End Sub
#End Region

#Region "Protected Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClear.Click"
    Protected Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClear.Click
        txtcustomercode.Text = ""
        txtcustomername.Text = ""
        ddlCategory.Value = "[Select]"
        ddlCitycode.Value = "[Select]"
        ddlCountryCode.Value = "[Select]"
        ddlCountryName.Value = "[Select]"
        ddlSalesPerson.Value = "[Select]"
        ddlSalesPersonName.Value = "[Select]"
        ddlMarket.Value = "[Select]"
        ddlOtherSellingType.Value = "[Select]"
        ddlSellingType.Value = "[Select]"
        ddlTicketSelling.Value = "[Select]"
        'ddlctryname.Value = "[Select]"
        ddlCityName.Value = "[Select]"
        'FillGrid("agentcode")
        ddlOrderBy.SelectedIndex = 0
        FillGrid("agentmast.agentname")

    End Sub
#End Region

#Region "Protected Sub gv_SearchResult_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles gv_SearchResult.RowDeleting"

    Protected Sub gv_SearchResult_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles gv_SearchResult.RowDeleting

    End Sub
#End Region

#Region "Protected Sub gv_SearchResult_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles gv_SearchResult.RowEditing"
    Protected Sub gv_SearchResult_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles gv_SearchResult.RowEditing

    End Sub
#End Region

#Region "Protected Sub gv_SearchResult_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gv_SearchResult.Sorting"

    Protected Sub gv_SearchResult_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gv_SearchResult.Sorting
        'Session.Add("strsortexpression", e.SortExpression)
        Select Case e.SortExpression
            Case "agentname"
                Session.Add("strsortexpression", "agentmast.agentname")
            Case "agentcode"
                Session.Add("strsortexpression", "agentmast.agentcode")
            Case "catcode"
                Session.Add("strsortexpression", "agentmast.catcode")
            Case "sellcode"
                Session.Add("strsortexpression", "sellmast.sellcode")
            Case "ctrycode"
                Session.Add("strsortexpression", "ctrymast.ctrycode")
            Case "plgrpcode"
                Session.Add("strsortexpression", "plgrpmast.plgrpcode")
        End Select

        SortGridColoumn_click(e.SortExpression)

    End Sub
#End Region

#Region " Public Sub SortGridColoumn_click()"
    Public Sub SortGridColoumn_click(ByVal sortby As String)
        Dim DataTable As DataTable
        Dim myDS As New DataSet
        FillGrid(Session("strsortexpression"), "")

        myDS = gv_SearchResult.DataSource
        DataTable = myDS.Tables(0)
        If IsDBNull(DataTable) = False Then
            Dim dataView As DataView = DataTable.DefaultView
            Session.Add("strsortdirection", objUtils.SwapSortDirection(Session("strsortdirection")))
            dataView.Sort = sortby & " " & objUtils.ConvertSortDirectionToSql(Session("strsortdirection"))
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
                'Response.ContentType = "application/vnd.ms-excel"
                'Response.Charset = ""
                'Me.EnableViewState = False

                'Dim tw As New System.IO.StringWriter()
                'Dim hw As New System.Web.UI.HtmlTextWriter(tw)
                'Dim frm As HtmlForm = New HtmlForm()
                'Me.Controls.Add(frm)
                'frm.Controls.Add(gv_SearchResult)
                'frm.RenderControl(hw)
                'Response.Write(tw.ToString())
                'Response.End()
                'Response.Clear()
              

                strSqlQry = "select agentcode as [Customer Code], agentname as [Customer Name]," & _
 "shortname	as [Short Name],  agentcatmast.agentcatname	as [Category Code], sellmast.sellname	as [Selling Type], " & _
 "othsellmast.othsellname as [Handling Fee Selling Type],s.othsellname as [Other selling Selling Type], " & _
 "ctrymast.ctryname	as [Country]," & _
"citymast.cityname	as [City], plgrpmast.plgrpname	as [Market]," & _
"usermaster.UserName AS [Reservation Contact]," & _
"u.UserName AS [Contact]," & _
"trfsellmast.trfsellname AS [Transfer Selling Type]," & _
"excsellmast.excsellname AS [Excursion Selling Type]," & _
"visasellmast.visasellname AS [Visa Selling Type]," & _
 "acctmast.acctname AS [Account Type]," & _
 "agentmast.add1	as [Address Line 1], agentmast.add2	as [Address Line 2]," & _
 "agentmast.add3	as [Address Line 3], agentmast.tel1	as [Telephone], agentmast.fax	as [Fax]," & _
 "agentmast.active	as [Active], agentmast.controlacctcode	as [Control A/c Code]," & _
 "agentmast.crdays	as [Credit Days], agentmast.crlimit	as [Credit Limit]," & _
 "agentmast.adddate	as [Date Created], agentmast.adduser	as [User Created]," & _
 "agentmast.moddate	as [Date Modified],  agentmast.moduser	as [User Modified] from agentmast " & _
 " inner join agentcatmast ON agentmast.catcode = agentcatmast.agentcatcode " & _
 " INNER JOIN sellmast ON agentmast.sellcode = sellmast.sellcode " & _
 " INNER JOIN  citymast ON agentmast.citycode =citymast.citycode" & _
 " inner join plgrpmast ON agentmast.plgrpcode = plgrpmast.plgrpcode " & _
 " INNER JOIN ctrymast on ctrymast.ctrycode = citymast.ctrycode" & _
 " LEFT JOIN othsellmast on agentmast.othsellcode= othsellmast.othsellcode" & _
 " LEFT join othsellmast as s  on agentmast.othsellcode1 =s.othsellcode " & _
"  Inner join UserMaster on agentmast.spersoncode=usermaster.UserCode " & _
 " Inner join UserMaster as u on agentmast.salescontact =u.UserCode" & _
 " LEFT join trfsellmast on agentmast.trfsellcode=trfsellmast.trfsellcode" & _
 " LEFT join excsellmast on agentmast.excsellcode=excsellmast.excsellcode" & _
 " Inner join acctmast on agentmast.controlacctcode=acctmast.acctcode" & _
 " LEFT join visasellmast on agentmast.visasellingtype=visasellmast.visasellcode"










                If Trim(BuildCondition) <> "" Then
                    strSqlQry = strSqlQry & " WHERE " & BuildCondition() & " ORDER BY agentcode"
                Else
                    strSqlQry = strSqlQry & " ORDER BY agentcode"
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
            'strpop = "window.open('rptReportNew.aspx?Pageame=Customers&BackPageName=CustomersSearch.aspx&CustCode=" & txtcustomercode.Text.Trim & "&CustName=" & txtcustomername.Text.Trim & "&SellType=" & Trim(ddlSellingType.Items(ddlSellingType.SelectedIndex).Text) & "&OthSellType=" & Trim(ddlOtherSellingType.Items(ddlOtherSellingType.SelectedIndex).Text) & "&TktSellType=" & Trim(ddlTicketSelling.Items(ddlTicketSelling.SelectedIndex).Text) & "&CtryCode=" & Trim(ddlCountryCode.Items(ddlCountryCode.SelectedIndex).Text) & "&CityCode=" & Trim(ddlCitycode.Items(ddlCitycode.SelectedIndex).Text) & "&MktCode=" & Trim(ddlMarket.Items(ddlMarket.SelectedIndex).Text) & "&CatCode=" & Trim(ddlCategory.Items(ddlCategory.SelectedIndex).Text) & "&spersoncode=" & Trim(ddlSalesPerson.Items(ddlSalesPerson.SelectedIndex).Text) & "','RepSupAgent','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
            strpop = "window.open('rptReportNew.aspx?Pageame=Customers&BackPageName=CustomersSearch.aspx&CustCode=" & txtcustomercode.Text.Trim & "&CustName=" & txtcustomername.Text.Trim & "&TktSellType=" & Trim(ddlTicketSelling.Items(ddlTicketSelling.SelectedIndex).Text) & "&CtryCode=" & Trim(ddlCountryCode.Items(ddlCountryCode.SelectedIndex).Text) & "&CityCode=" & Trim(ddlCitycode.Items(ddlCitycode.SelectedIndex).Text) & "&CatCode=" & Trim(ddlCategory.Items(ddlCategory.SelectedIndex).Text) & "&spersoncode=" & Trim(ddlSalesPerson.Items(ddlSalesPerson.SelectedIndex).Text) & "','RepSupAgent');"
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
        lblcategory.Visible = True
        lblcity.Visible = True
        lblCtryCode.Visible = True
        lblmarket.Visible = True
        lblothersellingtype.Visible = True
        lblsellingtype.Visible = True
        lblsellingtypecode.Visible = True
        lblCityName.Visible = True
        lblCtryName.Visible = True
        lblspersoncode.Visible = True
        lblspersonname.Visible = True

        ddlSalesPerson.Visible = True
        ddlSalesPersonName.Visible = True
        ddlCategory.Visible = True
        ddlCitycode.Visible = True
        'ddlctrycode.Visible = True
        ddlMarket.Visible = True
        ddlOtherSellingType.Visible = True
        ddlSellingType.Visible = True
        ddlTicketSelling.Visible = True
        'ddlctryname.Visible = True
        ddlCityName.Visible = True
        'ddlctrycode.Visible = True
        'ddlctryname.Visible = True
        ddlCountryCode.Visible = True
        ddlCountryName.Visible = True



    End Sub
#End Region

#Region " Protected Sub rbsearch_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)"
    Protected Sub rbsearch_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        'PnlCustomer.Visible = False
        lblcategory.Visible = False
        lblcity.Visible = False
        lblCtryCode.Visible = False
        lblmarket.Visible = False
        lblothersellingtype.Visible = False
        lblsellingtype.Visible = False
        lblsellingtypecode.Visible = False
        lblCityName.Visible = False
        lblCtryName.Visible = False
        lblspersoncode.Visible = False
        lblspersonname.Visible = False

        ddlCategory.Visible = False
        ddlCitycode.Visible = False
        ddlCountryCode.Visible = False
        ddlMarket.Visible = False
        ddlOtherSellingType.Visible = False
        ddlSellingType.Visible = False
        ddlTicketSelling.Visible = False
        ddlCityName.Visible = False
        ddlCountryName.Visible = False
        ddlSalesPerson.Visible = False
        ddlSalesPersonName.Visible = False

    End Sub
#End Region
    Private Sub fillorderby()
        ddlOrderBy.Items.Clear()
        ddlOrderBy.Items.Add("Customer Name")
        ddlOrderBy.Items.Add("Customer Code")
        ddlOrderBy.Items.Add("Category Code")
        ddlOrderBy.Items.Add("SellingType Code")
        ddlOrderBy.Items.Add("Country Code")
        ddlOrderBy.Items.Add("Market Code")
        ddlOrderBy.SelectedIndex = 0
    End Sub

    Protected Sub ddlOrderBy_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlOrderBy.SelectedIndexChanged
        Select Case ddlOrderBy.SelectedIndex
            Case 0
                FillGrid("agentmast.agentname")
            Case 1
                FillGrid("agentmast.agentcode")
            Case 2
                FillGrid("agentmast.catcode")
            Case 3
                FillGrid("sellmast.sellcode")
            Case 4
                FillGrid("ctrymast.ctrycode")
            Case 5
                FillGrid("plgrpmast.plgrpcode")
        End Select
    End Sub

    Protected Sub btnHelp_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=CustomersSearch','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub
End Class
