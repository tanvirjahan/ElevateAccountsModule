#Region "Namespace"
Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports System.IO.File
Imports System.Collections
Imports System.Collections.Generic
Imports System.Globalization
Imports ClosedXML.Excel
Imports System.Net


#End Region
Partial Class AccountsModule_SalesInvoiceFreeFormsearch
    Inherits System.Web.UI.Page

#Region "Global Declaration"
    Dim objUtils As New clsUtils
    Dim objUser As New clsUser
    Dim objectcl As New clsDateTime
    Dim objDateTime As New clsDateTime
    Dim strSqlQry As String
    Dim strWhereCond As String
    Dim SqlConn As SqlConnection
    Dim myCommand As SqlCommand
    Dim myDataAdapter As SqlDataAdapter
    Dim strappid As String = ""
    Dim strappname As String = ""
    Dim dtt As DataTable


   
    Dim mySqlCmd As SqlCommand

    Dim mySqlReader As SqlDataReader

    Dim document As New XLWorkbook

#End Region

#Region "Enum GridCol"
    Enum GridCol
        DocNoCol = 0
        DocNo = 1
        DocType = 2
        status = 3
        Type = 6
        FDate = 4
        Code = 5
        Name = 7
        Amount = 8
        DateCreated = 10
        UserCreated = 11
        DateModified = 12
        UserModified = 13
        Edit = 14
        View = 15
        Delete = 16
        Copy = 17
        Cancel = 18
        undocancel = 19
    End Enum
#End Region

#Region "Protected Sub btnAddNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddNew.Click"
    Protected Sub btnAddNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddNew.Click
       
        Dim strpop As String = ""
        Dim actionstr As String = ""
        actionstr = "New"

        If CType(ViewState("CNDNOpen_type"), String) = "MN" Then
            strpop = "window.open('Manualinvoicefreeform.aspx?State=" + CType(actionstr, String) + "&RefCode=" + CType("", String) + "&div_code=" + txtdivcode.Value + "&CNDNOpen_type=IN','Manualinvoicefreeform');"
        Else
            strpop = "window.open('salesinvoicefreeform.aspx?State=" + CType(actionstr, String) + "&RefCode=" + CType("", String) + "&div_code=" + txtdivcode.Value + "&CNDNOpen_type=" + CType(ViewState("CNDNOpen_type"), String) + "','salesinvoicefreeform');"
        End If

        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
        'Response.Redirect("SuppAgentMain.aspx", False) 'abc
       
    End Sub



#End Region
    Protected Sub lbCloseCategorypending_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Try

   
            Dim myButton As Button = CType(sender, Button)
            Dim dlItem As DataListItem = CType((CType(sender, Button)).NamingContainer, DataListItem)
            Dim lnkcode As Label = CType(dlItem.FindControl("lblType"), Label)
            Dim lnkvalue As Button = CType(dlItem.FindControl("lbInboxCategorypending"), Button)
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
            dlInboxSearch_pending.DataSource = dtDynamics
            dlInboxSearch_pending.DataBind()

            FillGridNew()
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        End Try
    End Sub

      
#Region "Public Sub SortGridColoumn_click()"
    Public Sub SortGridColoumn_click()
        Try
            Dim DataTable As DataTable
            Dim myDS As New DataSet
            'FillGrid (Session("RExpression"), "")
            FillGridNew()
            myDS = gv_SearchResult.DataSource
            DataTable = myDS.Tables(0)
            If IsDBNull(DataTable) = False Then
                Dim dataView As DataView = DataTable.DefaultView
                Session.Add("Rdirection", objUtils.SwapSortDirection(Session("Rdirection")))
                dataView.Sort = Session("RExpression") & " " & objUtils.ConvertSortDirectionToSql(Session("Rdirection"))
                gv_SearchResult.DataSource = dataView
                gv_SearchResult.DataBind()
            End If
        Catch ex As Exception

        End Try
    End Sub
#End Region

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If IsPostBack = False Then
                ViewState.Add("CNDNOpen_type", Request.QueryString("tran_type"))
                Dim appid As String = CType(Request.QueryString("appid"), String)

                Dim AppName As HiddenField = CType(Master.FindControl("hdnAppName"), HiddenField)
                If appid Is Nothing = False Then
                    strappid = appid 'AppId.Value
                End If
                If AppName Is Nothing = False Then
                    strappname = Session("AppName")
                End If
                ddlOrder.SelectedValue = "T"
                btnPrint.Attributes.Add("onclick", "return FormValidation('')")
                If appid = "4" Then
                    ' strappname = AppName.Value
                    strappname = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select displayname from appmaster where appid='" & appid & "'")
                Else

                    'strappname = AppName.Value
                    strappname = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select displayname from appmaster where appid='" & appid & "'")
                End If

                ViewState("Appname") = strappname

                Select Case ViewState("CNDNOpen_type")
                    Case "IN"
                        lblHeading.Text = "Sales Invoice Free Form Search"
                    Case "PI"
                        lblHeading.Text = "Purchase Invoice Free Form Search"
                    Case "PE"
                        lblHeading.Text = "Purchase Expense Search"
                    Case "DN"
                        lblHeading.Text = "Other Debit Note  Search"
                    Case "CN"
                        lblHeading.Text = "Other Credit Note Search"
                    Case "MN"
                        lblHeading.Text = "Manual Invoice Free Form Search"
                End Select
                Dim divid As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select division_master_code from division_master where accountsmodulename='" & ViewState("Appname") & "'")

                ViewState.Add("divcode", divid)
                ' hdntrantype.Value = CType(Request.QueryString("tran_type"), String)
                'Page.ClientScript.RegisterHiddenField("vTrantype", ViewState("ReceiptsSearchRVPVTranType"))

                txtdivcode.Value = CType(ViewState("divcode"), String)
                txttrantype.Value = CType(ViewState("CNDNOpen_type"), String)

                'If IsPostBack = False Then

                RowsPerPageCUS.SelectedValue = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id='2006'")
                If CType(Session("GlobalUserName"), String) = Nothing Or CType(Session("Userpwd"), String) = Nothing Then
                    Response.Redirect("~/Login.aspx", False)
                    Exit Sub
                Else
                    objUser.CheckUserRight(Session("dbconnectionName"), CType(Session("GlobalUserName"), String), CType(Session("Userpwd"), String), _
                                                       CType(strappname, String), "AccountsModule\salesinvoicefreeformsearch.aspx?tran_type=" & CType(ViewState("CNDNOpen_type"), String) & "&appid=" + appid, btnAddNew, btnExportToExcel, _
                                                       btnPrint, gv_SearchResult, GridCol.Edit, GridCol.Delete, GridCol.View, 0, GridCol.Copy, GridCol.Cancel, GridCol.undocancel)
                End If

                If ViewState("CNDNOpen_type") = "MN" Then
                    gv_SearchResult.Columns(20).Visible = False
                    gv_SearchResult.Columns(21).Visible = False
                    gv_SearchResult.Columns(22).Visible = False
                    gv_SearchResult.Columns(23).Visible = False
                    'gv_SearchResult.Columns(GridCol.undocancel + 2).Visible = False
                    'gv_SearchResult.Columns(GridCol.Copy + 2).Visible = False
                End If




                Session.Add("RExpression", "tran_id")
                Session.Add("Rdirection", SortDirection.Ascending)
                'charcters(txtcitycode)
                'charcters(txtcityname)
                '' Create a Dynamic datatable ---- Start
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
            End If
            ClientScript.GetPostBackEventReference(Me, String.Empty)
            If (Me.IsPostBack And (Me.Request("__EVENTTARGET") = "DebitNoteWindowPostBack" Or Me.Request("__EVENTTARGET") = "salesinvoicefreeformWindowPostBack")) Then
                FillGridNew()
            End If


            'FillGridNew()
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("salesinvoicefreeformSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub

    Private Sub FillGridNew()

        dtt = Session("sDtDynamic")
        Dim strInvoiceType As String = ""
        Dim strType As String = ""
        Dim strBindCondition As String = ""
        Dim strInvoiceNovalue As String = ""

        Dim strCustomer As String = ""
        Dim strSupplier As String = ""
        Dim strSalesMan As String = ""
        Dim strReferenceNo As String = ""
        Dim strSourceCountry As String = ""
        Dim strBookingNo As String = ""

        Dim strTextValue As String = ""
        Dim strnarration As String = ""


        Try
            If dtt.Rows.Count > 0 Then
                For i As Integer = 0 To dtt.Rows.Count - 1
                    If dtt.Rows(i)("Code").ToString = "INVOICENO" Or dtt.Rows(i)("Code").ToString = "CNNO" Or dtt.Rows(i)("Code").ToString = "DNNO" Or dtt.Rows(i)("Code").ToString = "PINO" Or dtt.Rows(i)("Code").ToString = "PENO" Then


                        If strInvoiceNovalue <> "" Then
                            strInvoiceNovalue = strInvoiceNovalue + ",'" + dtt.Rows(i)("Value").ToString + "'"
                        Else
                            strInvoiceNovalue = "'" + dtt.Rows(i)("Value").ToString + "'"
                        End If


                    End If
                    If dtt.Rows(i)("Code").ToString = "NARRATION" Then


                        If strnarration <> "" Then
                            strnarration = strnarration + ",'" + dtt.Rows(i)("Value").ToString + "'"
                        Else
                            strnarration = "'" + dtt.Rows(i)("Value").ToString + "'"
                        End If


                    End If
                    If dtt.Rows(i)("Code").ToString = "INVOICETYPE" Then
                        If strInvoiceType <> "" Then
                            strInvoiceType = strInvoiceType + ",'" + dtt.Rows(i)("Value").ToString + "'"
                        Else
                            strInvoiceType = "'" + dtt.Rows(i)("Value").ToString + "'"
                        End If
                    End If

                    If dtt.Rows(i)("Code").ToString = "TYPE" Then

                        If strType <> "" Then
                            strType = strType + ",'" + dtt.Rows(i)("Value").ToString + "'"
                        Else
                            strType = "'" + dtt.Rows(i)("Value").ToString + "'"
                        End If

                    End If
                    If dtt.Rows(i)("Code").ToString = "CUSTOMER" Then


                        If strCustomer <> "" Then
                            strCustomer = strCustomer + ",'" + dtt.Rows(i)("Value").ToString + "'"
                        Else
                            strCustomer = "'" + dtt.Rows(i)("Value").ToString + "'"
                        End If

                    End If
                    If dtt.Rows(i)("Code").ToString = "SUPPLIER" Then



                        If strSupplier <> "" Then
                            strSupplier = strSupplier + ",'" + dtt.Rows(i)("Value").ToString + "'"
                        Else
                            strSupplier = "'" + dtt.Rows(i)("Value").ToString + "'"
                        End If

                    End If
                    If dtt.Rows(i)("Code").ToString = "SALESMAN" Then



                        If strSalesMan <> "" Then
                            strSalesMan = strSalesMan + ",'" + dtt.Rows(i)("Value").ToString + "'"
                        Else
                            strSalesMan = "'" + dtt.Rows(i)("Value").ToString + "'"
                        End If

                    End If
                    If dtt.Rows(i)("Code").ToString = "REFERENCENO" Then



                        If strReferenceNo <> "" Then
                            strReferenceNo = strReferenceNo + ",'" + dtt.Rows(i)("Value").ToString + "'"
                        Else
                            strReferenceNo = "'" + dtt.Rows(i)("Value").ToString + "'"
                        End If

                    End If
                    If dtt.Rows(i)("Code").ToString = "SOURCECOUNTRY" Then



                        If strSourceCountry <> "" Then
                            strSourceCountry = strSourceCountry + ",'" + dtt.Rows(i)("Value").ToString + "'"
                        Else
                            strSourceCountry = "'" + dtt.Rows(i)("Value").ToString + "'"
                        End If

                    End If
                    If dtt.Rows(i)("Code").ToString = "BOOKINGNO" Then



                        If strBookingNo <> "" Then
                            strBookingNo = strBookingNo + ",'" + dtt.Rows(i)("Value").ToString + "'"
                        Else
                            strBookingNo = "'" + dtt.Rows(i)("Value").ToString + "'"
                        End If


                    End If
                    If dtt.Rows(i)("Code").ToString = "TEXT" Then

                        If strTextValue <> "" Then
                            strTextValue = strTextValue + "," + dtt.Rows(i)("Value").ToString
                        Else
                            strTextValue = dtt.Rows(i)("Value").ToString
                        End If
                    End If
                Next
            End If
            Dim pagevaluecs = RowsPerPageCUS.SelectedValue

            strBindCondition = BuildConditionNew(strInvoiceType, strType, strInvoiceNovalue, strCustomer, strSupplier, strSalesMan, strReferenceNo, strSourceCountry, strBookingNo, strnarration, strTextValue)
            Dim myDS As New DataSet
            Dim strValue As String
            gv_SearchResult.Visible = True
            lblMsg.Visible = False
            If gv_SearchResult.PageIndex < 0 Then

                gv_SearchResult.PageIndex = 0
            End If

            Dim divid As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select division_master_code from division_master where accountsmodulename='" & ViewState("Appname") & "'")

            strSqlQry = " select tran_id invoiceno,cancel_state,invoicetype doctype,case isnull(freeforminvoice_master.cancel_state,'') when 'Y' then 'Cancelled' else (case isnull(freeforminvoice_master.post_state,'') when 'P' then 'Posted' when 'U' then 'UnPosted' end) end  as post_state, " & _
                        "[acctype]=case when acc_type='C' then 'Customer' when acc_type='S' then 'Supplier' when acc_type='A' then 'Supplier Agent' end ," & _
                        "invoice_date,referenceno,view_account.des as supname, Basetotal, usermaster.username, ctrymast.ctryname,narration, bookingno, freeforminvoice_master.adddate, freeforminvoice_master.adduser,freeforminvoice_master.moddate,freeforminvoice_master.moduser from freeforminvoice_master left join ctrymast on ctrymast.ctrycode=freeforminvoice_master.sourcecountry left join usermaster on usermaster.usercode=freeforminvoice_master.sperson , view_account " & _
                        "where (freeforminvoice_master.supcode = view_account.code And freeforminvoice_master.acc_type = view_account.type)" & _
                        "and freeforminvoice_master.tran_type = '" & IIf(CType(ViewState("CNDNOpen_type"), String) = "MN", "IN", CType(ViewState("CNDNOpen_type"), String)) & "' and freeforminvoice_master.div_code='" & divid & "' and view_account.div_code='" & divid & "'"
            If CType(ViewState("CNDNOpen_type"), String) = "MN" Then
                strSqlQry = strSqlQry & " and freeforminvoice_master.Manualinvoice=1"
            Else
                strSqlQry = strSqlQry & " and ISNULL(freeforminvoice_master.Manualinvoice,0)=0"
            End If

            Dim strorderby As String = "convert(varchar(10),invoice_date,111) desc," + Session("RExpression")
            Dim strsortorder As String = IIf(Session("Rdirection") = 0, "Asc", "DESC")

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
                lblMsg.Visible = True
                lblMsg.Text = "Records not found, Please redefine search criteria."
            End If

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("salesfreeformSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            'clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
            'clsDBConnect.dbConnectionClose(SqlConn)                          'Close connection
        End Try

    End Sub

    Private Function BuildConditionNew(ByVal strInvoiceType As String, ByVal strType As String, ByVal strInvoiceNovalue As String, ByVal strCustomer As String, ByVal strSupplier As String, ByVal strSalesMan As String, ByVal strReferenceNo As String, ByVal strSourceCountry As String, ByVal strBookingNo As String, ByVal strnarration As String, ByVal strTextValue As String) As String
        strWhereCond = ""

        If strnarration.Trim <> "" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = "upper(freeforminvoice_master.narration) IN (" & Trim(strnarration.Trim.ToUpper) & ")"
            Else
                strWhereCond = strWhereCond & " AND upper(freeforminvoice_master.narration) IN (" & Trim(strnarration.Trim.ToUpper) & ")"
            End If
        End If
        If strInvoiceType.Trim <> "" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = "upper(freeforminvoice_master.InvoiceType) IN (" & Trim(strInvoiceType.Trim.ToUpper) & ")"
            Else
                strWhereCond = strWhereCond & " AND upper(freeforminvoice_master.InvoiceType) IN (" & Trim(strInvoiceType.Trim.ToUpper) & ")"
            End If
        End If
        If strType.Trim <> "" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = "upper( case ltrim(rtrim(acc_type)) when 'C' Then 'Customer' else 'Supplier' end) IN (" & Trim(strType.Trim.ToUpper) & ")"
            Else
                strWhereCond = strWhereCond & " AND upper( case ltrim(rtrim(acc_type)) when 'C' Then 'Customer' else 'Supplier' end) IN (" & Trim(strType.Trim.ToUpper) & ")"
            End If
        End If

        If strInvoiceNovalue.Trim <> "" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = "upper(freeforminvoice_master.tran_id) IN (" & Trim(strInvoiceNovalue.Trim.ToUpper) & ")"
            Else
                strWhereCond = strWhereCond & " AND upper(freeforminvoice_master.tran_id) IN (" & Trim(strInvoiceNovalue.Trim.ToUpper) & ")"
            End If
        End If
        If strCustomer.Trim <> "" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = "view_account.type='C' and upper(view_account.des) IN (" & Trim(strCustomer.Trim.ToUpper) & ")"
            Else
                strWhereCond = strWhereCond & " AND view_account.type='C' and  upper(view_account.des) IN (" & Trim(strCustomer.Trim.ToUpper) & ")"
            End If
        End If

        If strSupplier.Trim <> "" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = "view_account.type='S' and upper(view_account.des) IN (" & Trim(strSupplier.Trim.ToUpper) & ")"
            Else
                strWhereCond = strWhereCond & " AND view_account.type='S' and upper(view_account.des) IN (" & Trim(strSupplier.Trim.ToUpper) & ")"
            End If
        End If

        If strSalesMan.Trim <> "" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = "upper(usermaster.username) IN (" & Trim(strSalesMan.Trim.ToUpper) & ")"
            Else
                strWhereCond = strWhereCond & " AND upper(usermaster.username) IN (" & Trim(strSalesMan.Trim.ToUpper) & ")"
            End If
        End If
        If strReferenceNo.Trim <> "" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = "upper(freeforminvoice_master.referenceno) IN (" & Trim(strReferenceNo.Trim.ToUpper) & ")"
            Else
                strWhereCond = strWhereCond & " AND upper(freeforminvoice_master.referenceno) IN (" & Trim(strReferenceNo.Trim.ToUpper) & ")"
            End If
        End If

        If strSourceCountry.Trim <> "" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = "upper(ctrymast.ctryname) IN (" & Trim(strSourceCountry.Trim.ToUpper) & ")"
            Else
                strWhereCond = strWhereCond & " AND upper(ctrymast.ctryname) IN (" & Trim(strSourceCountry.Trim.ToUpper) & ")"
            End If
        End If
        If strBookingNo.Trim <> "" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = "upper(freeforminvoice_master.bookingno) IN (" & Trim(strBookingNo.Trim.ToUpper) & ")"
            Else
                strWhereCond = strWhereCond & " AND upper(freeforminvoice_master.bookingno) IN (" & Trim(strBookingNo.Trim.ToUpper) & ")"
            End If
        End If
        If strTextValue <> "" Then

            Dim lsMainArr As String()
            Dim strValue As String = ""
            Dim strWhereCond1 As String = ""
            lsMainArr = objUtils.splitWithWords(strTextValue, ",")
            For i = 0 To lsMainArr.GetUpperBound(0)
                strValue = ""
                strValue = lsMainArr(i)
                If strValue <> "" Then


                    If Trim(strWhereCond1) = "" Then
                        strWhereCond1 = "  upper(freeforminvoice_master.tran_id) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' or upper(freeforminvoice_master.InvoiceType) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%'  or upper(freeforminvoice_master.acc_type) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%'  or   upper(view_account.des)  LIKE '%" & Trim(strValue.Trim.ToUpper) & "%'   or   upper(freeforminvoice_master.sperson) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%'  or  upper(freeforminvoice_master.referenceno) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%'  or  upper(freeforminvoice_master.sourcecountry) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' or upper(freeforminvoice_master.bookingno) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' or upper(freeforminvoice_master.narration) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%'  "
                    Else
                        strWhereCond1 = strWhereCond1 & " OR   upper(freeforminvoice_master.tran_id) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' or upper(freeforminvoice_master.InvoiceType) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' or upper(freeforminvoice_master.acc_type) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%'  or   upper(view_account.des)  LIKE '%" & Trim(strValue.Trim.ToUpper) & "%'   or   upper(freeforminvoice_master.sperson) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%'  or  upper(freeforminvoice_master.referenceno) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%'  or   upper(freeforminvoice_master.sourcecountry) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' or upper(freeforminvoice_master.bookingno) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' or upper(freeforminvoice_master.narration) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%'   "
                    End If


                    'strcontractid = strcontractid & "OR v.contractid like '%" & Trim(strValue.Trim.ToUpper) & "%'"
                    'strofferid = strofferid & "OR v.promotionid like '%" & Trim(strValue.Trim.ToUpper) & "%'"
                End If
            Next

            If Trim(strWhereCond) = "" Then
                strWhereCond = " (" & strWhereCond1 & ")"
            Else
                strWhereCond = strWhereCond & " AND (" & strWhereCond1 & ")"
            End If

        End If




        If txtFromDate.Text.Trim <> "" And txtToDate.Text <> "" Then

            If ddlOrder.SelectedValue = "C" Then
                If Trim(strWhereCond) = "" Then

                    strWhereCond = " (CONVERT(datetime, convert(varchar(10),freeforminvoice_master.adddate,103),103)  between CONVERT(datetime, '" + txtFromDate.Text + "',103) and CONVERT(datetime,  '" + txtToDate.Text + "',103)) "
                Else
                    strWhereCond = strWhereCond & "  and (CONVERT(datetime, convert(varchar(10),freeforminvoice_master.adddate,103),103)  between CONVERT(datetime, '" + txtFromDate.Text + "',103) and CONVERT(datetime,  '" + txtToDate.Text + "',103)) "
                End If
            ElseIf ddlOrder.SelectedValue = "M" Then
                If Trim(strWhereCond) = "" Then

                    strWhereCond = " (CONVERT(datetime, convert(varchar(10),freeforminvoice_master.moddate,103),103)  between CONVERT(datetime, '" + txtFromDate.Text + "',103) and CONVERT(datetime,  '" + txtToDate.Text + "',103)) "
                Else
                    strWhereCond = strWhereCond & "  and (CONVERT(datetime, convert(varchar(10),freeforminvoice_master.moddate,103),103)  between CONVERT(datetime, '" + txtFromDate.Text + "',103) and CONVERT(datetime,  '" + txtToDate.Text + "',103)) "
                End If
            ElseIf ddlOrder.SelectedValue = "T" Then

                If Trim(strWhereCond) = "" Then
                    strWhereCond = "(CONVERT(datetime, convert(varchar(10),freeforminvoice_master.invoice_date ,103),103)  between CONVERT(datetime, '" + txtFromDate.Text + "',103) and CONVERT(datetime,  '" + txtToDate.Text + "',103)) "
                Else
                    strWhereCond = strWhereCond & "  and (CONVERT(datetime, convert(varchar(10),freeforminvoice_master.invoice_date ,103),103)  between CONVERT(datetime, '" + txtFromDate.Text + "',103) and CONVERT(datetime,  '" + txtToDate.Text + "',103)) "
                End If

            End If
        End If
        BuildConditionNew = strWhereCond
    End Function
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
                    Case 14
                        Me.MasterPageFile = "~/AccountsMaster.master"
                    Case 13
                        Me.MasterPageFile = "~/VisaMaster.master"      'Added by Archana on 05/06/2015 for VisaModule
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
    Protected Sub btnvsprocess_pending_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnvsprocess_pending.Click

        Try
            FilterGrid()
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("salesinvoicefreeformSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try



    End Sub
    Private Sub FilterGrid()
        Dim lsSearchTxt As String = ""
        lsSearchTxt = txtvsprocesssplit_pending.Text
        Dim lsProcessINVOICENO As String = ""
        Dim lsProcessINVOICETYPE As String = ""
        Dim lsProcessTYPE As String = ""
        Dim lsProcessCUSTOMER As String = ""
        Dim lsProcessSUPPLIER As String = ""
        Dim lsProcessSALESMAN As String = ""
        Dim lsProcessREFERENCENO As String = ""
        Dim lsProcessSOURCECOUNTRY As String = ""
        Dim lsProcessBOOKINGNO As String = ""
        Dim lsProcessAll As String = ""
        Dim lsMainArr As String()
        lsMainArr = objUtils.splitWithWords(lsSearchTxt, "|~,")
        For i = 0 To lsMainArr.GetUpperBound(0)
            Select Case lsMainArr(i).Split(":")(0).ToString.ToUpper.Trim
                Case "INVOICENO"
                    lsProcessINVOICENO = lsMainArr(i).Split(":")(1)
                    sbAddToDataTable("INVOICENO", lsProcessINVOICENO, "INVOICENO")
                Case "CNNo"
                    lsProcessINVOICENO = lsMainArr(i).Split(":")(1)
                    sbAddToDataTable("CNNo", lsProcessINVOICENO, "CNNo")
                Case "DNNo"
                    lsProcessINVOICENO = lsMainArr(i).Split(":")(1)
                    sbAddToDataTable("DNNo", lsProcessINVOICENO, "DNNo")
                Case "PINo"
                    lsProcessINVOICENO = lsMainArr(i).Split(":")(1)
                    sbAddToDataTable("PINo", lsProcessINVOICENO, "PINo")
                Case "PENo"
                    lsProcessINVOICENO = lsMainArr(i).Split(":")(1)
                    sbAddToDataTable("PENo", lsProcessINVOICENO, "PENo")

                Case "INVOICETYPE"
                    lsProcessINVOICETYPE = lsMainArr(i).Split(":")(1)
                    sbAddToDataTable("INVOICETYPE", lsProcessINVOICETYPE, "INVOICETYPE")
                Case "TYPE"
                    lsProcessTYPE = lsMainArr(i).Split(":")(1)
                    sbAddToDataTable("TYPE", lsProcessTYPE, "TYPE")
                Case "CUSTOMER"
                    lsProcessCUSTOMER = lsMainArr(i).Split(":")(1)
                    sbAddToDataTable("CUSTOMER", lsProcessCUSTOMER, "CUSTOMER")
                Case "SUPPLIER"
                    lsProcessSUPPLIER = lsMainArr(i).Split(":")(1)
                    sbAddToDataTable("SUPPLIER", lsProcessSUPPLIER, "SUPPLIER")
                Case "SALESMAN"
                    lsProcessSALESMAN = lsMainArr(i).Split(":")(1)
                    sbAddToDataTable("SALESMAN", lsProcessSALESMAN, "SALESMAN")
                Case "REFERENCENO"
                    lsProcessREFERENCENO = lsMainArr(i).Split(":")(1)
                    sbAddToDataTable("REFERENCENO", lsProcessREFERENCENO, "REFERENCENO")
                Case "SOURCECOUNTRY"
                    lsProcessSOURCECOUNTRY = lsMainArr(i).Split(":")(1)
                    sbAddToDataTable("SOURCECOUNTRY", lsProcessSOURCECOUNTRY, "SOURCECOUNTRY")
                Case "BOOKINGNO"
                    lsProcessBOOKINGNO = lsMainArr(i).Split(":")(1)
                    sbAddToDataTable("BOOKINGNO", lsProcessBOOKINGNO, "BOOKINGNO")
                Case "NARRATION"
                    lsProcessBOOKINGNO = lsMainArr(i).Split(":")(1)
                    sbAddToDataTable("NARRATION", lsProcessBOOKINGNO, "NARRATION")
                Case "TEXT"
                    lsProcessAll = lsMainArr(i).Split(":")(1)
                    sbAddToDataTable("TEXT", lsProcessAll, "TEXT")
            End Select
        Next

        Dim dtt As DataTable
        dtt = Session("sDtDynamic")
        dlInboxSearch_pending.DataSource = dtt
        dlInboxSearch_pending.DataBind()

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

    Protected Sub btnClearDate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClearDate.Click
        txtFromDate.Text = ""
        txtToDate.Text = ""
        FillGridNew()
    End Sub


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
            Dim divid As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select division_master_code from division_master where accountsmodulename='" & ViewState("Appname") & "'")
            strSqlQry = " select tran_id invoiceno,invoicetype doctype,case isnull(freeforminvoice_master.cancel_state,'') when 'Y' then 'Cancelled' else (case isnull(freeforminvoice_master.post_state,'') when 'P' then 'Posted' when 'U' then 'UnPosted' end) end  as post_state, " & _
                        "[acctype]=case when acc_type='C' then 'Customer' when acc_type='S' then 'Supplier' when acc_type='A' then 'Supplier Agent' end ," & _
                        "invoice_date,referenceno,view_account.des as supname, Basetotal, sperson, sourcecountry, bookingno, adddate, adduser, moddate, moduser from freeforminvoice_master, view_account " & _
                        "where(freeforminvoice_master.supcode = view_account.code And freeforminvoice_master.acc_type = view_account.type)" & _
                        "and freeforminvoice_master.tran_type = '" & IIf(CType(ViewState("CNDNOpen_type"), String) = "MN", "IN", CType(ViewState("CNDNOpen_type"), String)) & "' and freeforminvoice_master.div_code='" & divid & "' and view_account.div_code='" & divid & "'"



            If CType(ViewState("CNDNOpen_type"), String) = "MN" Then
                strSqlQry = strSqlQry & " and freeforminvoice_master.Manualinvoice=1"
            Else
                strSqlQry = strSqlQry & " and freeforminvoice_master.Manualinvoice=0"
            End If

            strSqlQry = strSqlQry & " ORDER BY " & strorderby & " " & strsortorder


            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)
            gv_SearchResult.DataSource = myDS

            If myDS.Tables(0).Rows.Count > 0 Then
                gv_SearchResult.PageSize = RowsPerPageCUS.SelectedValue
                gv_SearchResult.DataBind()
            Else
                gv_SearchResult.PageIndex = 0
                gv_SearchResult.DataBind()
                lblMsg.Visible = True
                lblMsg.Text = "Records not found, Please redefine search criteria."
            End If

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("SalesInvoiceSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            'clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
            'clsDBConnect.dbConnectionClose(SqlConn)                          'Close connection
        End Try

    End Sub
#End Region
    Public Function Validatecancelled(ByVal tranid) As Boolean
        Try
            Dim ds As DataSet = objUtils.ExecuteQuerySqlnew(Session("dbconnectionName"), "select * from  freeforminvoice_master where tran_id='" + tranid + "' ")
            If ds.Tables.Count > 0 Then
                If ds.Tables(0).Rows.Count > 0 Then
                    If IsDBNull(ds.Tables(0).Rows(0)("cancel_state")) = False Then
                        If ds.Tables(0).Rows(0)("cancel_state") = "Y" Then
                            Return True
                        Else
                            Return False
                        End If
                    End If
                End If
            End If
        Catch ex As Exception
            Validatecancelled = False
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ReservationInvoiceSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Function
    Public Function Validateseal(ByVal tranid) As Boolean
        Try
            Dim ds As DataSet = objUtils.ExecuteQuerySqlnew(Session("dbconnectionName"), "select * from  freeforminvoice_master  where tran_id='" + tranid + "' ")
            If ds.Tables.Count > 0 Then
                If ds.Tables(0).Rows.Count > 0 Then
                    If IsDBNull(ds.Tables(0).Rows(0)("trans_state")) = False Then
                        If ds.Tables(0).Rows(0)("trans_state") = "S" Then
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
    Protected Sub gv_SearchResult_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gv_SearchResult.RowCommand
        Try
            If e.CommandName = "Page" Then Exit Sub
            If e.CommandName = "Sort" Then Exit Sub
            Dim lblId As Label
            lblId = gv_SearchResult.Rows(CType(e.CommandArgument.ToString, Integer)).FindControl("lblCode")

            If e.CommandName = "EditRow" Then
                'Session.Add("State", "Edit")
                'Session.Add("RefCode", CType(lblId.Text.Trim, String))
                'Response.Redirect("DebitNote.aspx", False)

                If Validatecancelled(lblId.Text) Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Cancelled Transaction cannot edit...')", True)
                    Return
                End If

                If Validateseal(lblId.Text) Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Document has sealed cannot Edit/Delete...')", True)
                    Return
                End If

                Dim strpop As String = ""
                If CType(ViewState("CNDNOpen_type"), String) = "MN" Then
                    strpop = "window.open('Manualinvoicefreeform.aspx?State=Edit&RefCode=" + CType(lblId.Text.Trim, String) + "&CNDNOpen_type=IN&div_code=" & txtdivcode.Value & "','ManualInvoicefreeform');"

                Else
                    strpop = "window.open('SalesInvoicefreeform.aspx?State=Edit&RefCode=" + CType(lblId.Text.Trim, String) + "&CNDNOpen_type=" & ViewState("CNDNOpen_type") & "&div_code=" & txtdivcode.Value & "','SalesInvoicefreeform');"
                End If



                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)

            ElseIf e.CommandName = "View" Then
                'Session.Add("State", "View")
                'Session.Add("RefCode", CType(lblId.Text.Trim, String))
                'Response.Redirect("DebitNote.aspx", False)
                Dim strpop As String = ""
                If CType(ViewState("CNDNOpen_type"), String) = "MN" Then
                    strpop = "window.open('Manualinvoicefreeform.aspx?State=View&RefCode=" + CType(lblId.Text.Trim, String) + "&CNDNOpen_type=IN&div_code=" & txtdivcode.Value & "','ManualInvoicefreeform');"
                Else
                    strpop = "window.open('SalesInvoicefreeform.aspx?State=View&RefCode=" + CType(lblId.Text.Trim, String) + "&CNDNOpen_type=" & ViewState("CNDNOpen_type") & "&div_code=" & txtdivcode.Value & "','SalesInvoicefreeform');"
                End If


                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)

            ElseIf e.CommandName = "DeleteRow" Then
                'Session.Add("State", "Delete")
                'Session.Add("RefCode", CType(lblId.Text.Trim, String))
                'Response.Redirect("DebitNote.aspx", False)

                If Validatecancelled(lblId.Text) Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Cancelled Transaction cannot Delete...')", True)
                    Return
                End If



                If Validateseal(lblId.Text) Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Document has sealed cannot Edit/Delete...')", True)
                    Return
                End If

                Dim strpop As String = ""
                If CType(ViewState("CNDNOpen_type"), String) = "MN" Then
                    strpop = "window.open('Manualinvoicefreeform.aspx?State=Delete&RefCode=" + CType(lblId.Text.Trim, String) + "&CNDNOpen_type=IN&div_code=" & txtdivcode.Value & "','Manualinvoicefreeform');"
                Else
                    strpop = "window.open('SalesInvoicefreeform.aspx?State=Delete&RefCode=" + CType(lblId.Text.Trim, String) + "&CNDNOpen_type=" & ViewState("CNDNOpen_type") & "&div_code=" & txtdivcode.Value & "','SalesInvoicefreeform');"
                End If

                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)

            ElseIf e.CommandName = "Copy" Then
                'Session.Add("State", "Copy")
                'Session.Add("RefCode", CType(lblId.Text.Trim, String))
                'Response.Redirect("DebitNote.aspx", False)

                If Validatecancelled(lblId.Text) Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Cancelled Transaction cannot Copy...')", True)
                    Return
                End If

                Dim strpop As String = ""
                If CType(ViewState("CNDNOpen_type"), String) = "MN" Then
                    strpop = "window.open('MAnualInvoicefreeform.aspx?State=Copy&RefCode=" + CType(lblId.Text.Trim, String) + "&CNDNOpen_type=IN&div_code=" & txtdivcode.Value & "','ManualInvoicefreeform');"
                Else
                    strpop = "window.open('SalesInvoicefreeform.aspx?State=Copy&RefCode=" + CType(lblId.Text.Trim, String) + "&CNDNOpen_type=" & ViewState("CNDNOpen_type") & "&div_code=" & txtdivcode.Value & "','SalesInvoicefreeform');"
                End If

                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
            ElseIf e.CommandName = "Cancelrow" Then
                'Session.Add("State", "Delete")
                'Session.Add("RefCode", CType(lblId.Text.Trim, String))
                'Response.Redirect("DebitNote.aspx", False)

                If Validatecancelled(lblId.Text) Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Cancelled Transaction cannot Delete...')", True)
                    Return
                End If



                If Validateseal(lblId.Text) Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Document has sealed cannot Edit/Delete...')", True)
                    Return
                End If

                Dim strpop As String = ""
                If CType(ViewState("CNDNOpen_type"), String) = "MN" Then
                    strpop = "window.open('manualInvoicefreeform.aspx?State=Cancel&RefCode=" + CType(lblId.Text.Trim, String) + "&CNDNOpen_type=IN&div_code=" & txtdivcode.Value & "','ManualInvoicefreeform');"
                Else
                    strpop = "window.open('SalesInvoicefreeform.aspx?State=Cancel&RefCode=" + CType(lblId.Text.Trim, String) + "&CNDNOpen_type=" & ViewState("CNDNOpen_type") & "&div_code=" & txtdivcode.Value & "','SalesInvoicefreeform');"
                End If

                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)

            ElseIf e.CommandName = "undoCancel" Then
                'Session.Add("State", "Delete")
                'Session.Add("RefCode", CType(lblId.Text.Trim, String))
                'Response.Redirect("DebitNote.aspx", False)
                Dim strpop As String = ""
                If CType(ViewState("CNDNOpen_type"), String) = "MN" Then
                    strpop = "window.open('manualInvoicefreeform.aspx?State=undoCancel&RefCode=" + CType(lblId.Text.Trim, String) + "&CNDNOpen_type=IN&div_code=" & txtdivcode.Value & "','MAnualInvoicefreeform');"

                Else
                    strpop = "window.open('SalesInvoicefreeform.aspx?State=undoCancel&RefCode=" + CType(lblId.Text.Trim, String) + "&CNDNOpen_type=" & ViewState("CNDNOpen_type") & "&div_code=" & txtdivcode.Value & "','SalesInvoicefreeform');"

                End If
             
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
            ElseIf e.CommandName = "ViewLog" Then
                Dim strpo As String
                Dim actionstr As String
                actionstr = "ViewLog"
                strpo = "window.open('ViewLog.aspx?State=" + CType(actionstr, String) + "&RefCode=" + CType(lblId.Text.Trim, String) + "&div_code=" & txtdivcode.Value & "&trantype=" + CType(ViewState("CNDNOpen_type"), String) + "','SalesInvoicefreeform');"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpo, True)





                If Validatecancelled(lblId.Text) = False Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Cancelled Transaction cannot Delete...')", True)
                    Return
                End If



                If Validateseal(lblId.Text) Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Document has sealed cannot Edit/Delete...')", True)
                    Return
                End If

                Dim strpop As String = ""
                If CType(ViewState("CNDNOpen_type"), String) = "MN" Then
                    strpop = "window.open('ManualInvoiceFreeForm.aspx?State=undoCancel&RefCode=" + CType(lblId.Text.Trim, String) + "&CNDNOpen_type=IN&div_code=" & txtdivcode.Value & "','DebitNote');"
                Else
                    strpop = "window.open('SalesInvoicefreeform.aspx?State=undoCancel&RefCode=" + CType(lblId.Text.Trim, String) + "&CNDNOpen_type=" & ViewState("CNDNOpen_type") & "&div_code=" & txtdivcode.Value & "','DebitNote');"
                End If

                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)


            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("SalesInvoicefreeformSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub



#Region "Protected Sub gv_SearchResult_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gv_SearchResult.PageIndexChanging"
    Protected Sub gv_SearchResult_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gv_SearchResult.PageIndexChanging
        gv_SearchResult.PageIndex = e.NewPageIndex
        'FillGrid("tran_id", "DESC")
        FillGridNew()
    End Sub
#End Region
#Region "Protected Sub gv_SearchResult_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gv_SearchResult.Sorting"
    Protected Sub gv_SearchResult_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gv_SearchResult.Sorting
        Session.Add("RExpression", e.SortExpression)
        SortGridColoumn_click()
    End Sub
#End Region

    Protected Sub btnFilter_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnFilter.Click

    End Sub

    Protected Sub gv_SearchResult_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gv_SearchResult.RowDataBound

        If e.Row.RowType = DataControlRowType.Header Then
            Select Case ViewState("CNDNOpen_type")

                Case "PI"
                    e.Row.Cells(GridCol.DocNo).Text = "PI No"
                    e.Row.Cells(GridCol.DocType).Text = "PI Type"
                Case "PE"
                    e.Row.Cells(GridCol.DocNo).Text = "PE No"
                    e.Row.Cells(GridCol.DocType).Text = "PE Type"
                Case "DN"
                    e.Row.Cells(GridCol.DocNo).Text = "DN No"
                    e.Row.Cells(GridCol.DocType).Text = "DN Type"
                Case "CN"
                    e.Row.Cells(GridCol.DocNo).Text = "CN No"
                    e.Row.Cells(GridCol.DocType).Text = "CN Type"
            End Select

            Exit Sub
        Else
            'Dim cancelst As String

            'cancelst = e.Row.Cells(4).Text

            'If cancelst.ToUpper = "CANCELLED" Then
            '    e.Row.Cells(21).Enabled = False
            '    e.Row.Cells(21).ForeColor = Drawing.Color.DarkGray
            '    e.Row.Cells(22).Enabled = True

            'Else

            '    e.Row.Cells(22).Enabled = False
            '    e.Row.Cells(22).ForeColor = Drawing.Color.DarkGray
            '    e.Row.Cells(21).Enabled = True

            'End If
        End If


        If e.Row.RowType = DataControlRowType.DataRow Then

            Dim cancelst As String

            cancelst = e.Row.Cells(4).Text

            If cancelst.ToUpper = "CANCELLED" Then
                e.Row.Cells(21).Enabled = False
                e.Row.Cells(21).ForeColor = Drawing.Color.DarkGray
                e.Row.Cells(22).Enabled = True

            Else

                e.Row.Cells(22).Enabled = False
                e.Row.Cells(22).ForeColor = Drawing.Color.DarkGray
                e.Row.Cells(21).Enabled = True

            End If
            Exit Sub
        End If

    End Sub

 
    Protected Sub btnResetSelection_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnResetSelection.Click
        Dim dtt As DataTable
        dtt = Session("sDtDynamic")
        If dtt.Rows.Count > 0 Then
            dtt.Clear()
        End If
        Session("sDtDynamic") = dtt
        dlInboxSearch_pending.DataSource = dtt
        dlInboxSearch_pending.DataBind()
        FillGridNew()
    End Sub
    Private Function FillGridNew_report() As String

        dtt = Session("sDtDynamic")
        Dim strInvoiceType As String = ""
        Dim strType As String = ""
        Dim strBindCondition As String = ""
        Dim strInvoiceNovalue As String = ""

        Dim strCustomer As String = ""
        Dim strSupplier As String = ""
        Dim strSalesMan As String = ""
        Dim strReferenceNo As String = ""
        Dim strSourceCountry As String = ""
        Dim strBookingNo As String = ""

        Dim strTextValue As String = ""
        Dim strnarration As String = ""
        Dim strSqlQry1 As String
        Dim reportfilt As String
        Dim reportfilt_date, reportfilt_invoice, reportfilt_NARRATION, reportfilt_type, reportfilt_INVOICETYPE, reportfilt_CUSTOMER, reportfilt_SUPPLIER, reportfilt_SALESMAN, reportfilt_REFERENCENO, reportfilt_SOURCECOUNTRY, reportfilt_BOOKINGNO, reportfilt_text As String

        reportfilt_invoice = ""
        reportfilt_NARRATION = ""
        reportfilt_type = ""
        reportfilt_INVOICETYPE = ""
        reportfilt_CUSTOMER = ""
        reportfilt_SUPPLIER = ""
        reportfilt_SALESMAN = ""
        reportfilt_REFERENCENO = ""
        reportfilt_SOURCECOUNTRY = ""
        reportfilt_BOOKINGNO = ""
        reportfilt_text = ""
        reportfilt_date = ""

        Try
            If dtt.Rows.Count > 0 Then
                For i As Integer = 0 To dtt.Rows.Count - 1
                    If dtt.Rows(i)("Code").ToString = "INVOICENO" Or dtt.Rows(i)("Code").ToString = "CNNO" Or dtt.Rows(i)("Code").ToString = "DNNO" Or dtt.Rows(i)("Code").ToString = "PINO" Or dtt.Rows(i)("Code").ToString = "PENO" Then


                        If strInvoiceNovalue <> "" Then
                            strInvoiceNovalue = strInvoiceNovalue + ",'" + dtt.Rows(i)("Value").ToString + "'"
                        Else
                            strInvoiceNovalue = "'" + dtt.Rows(i)("Value").ToString + "'"

                        End If
                        reportfilt_invoice = dtt.Rows(i)("Code").ToString + ":" + strInvoiceNovalue
                    Else

                    End If
                    If dtt.Rows(i)("Code").ToString = "NARRATION" Then


                        If strnarration <> "" Then

                            strnarration = strnarration + ",'" + dtt.Rows(i)("Value").ToString + "'"

                        Else
                            strnarration = "'" + dtt.Rows(i)("Value").ToString + "'"

                        End If
                        reportfilt_NARRATION = dtt.Rows(i)("Code").ToString + ":" + strnarration

                    End If
                    If dtt.Rows(i)("Code").ToString = "INVOICETYPE" Then
                        If strInvoiceType <> "" Then
                            strInvoiceType = strInvoiceType + ",'" + dtt.Rows(i)("Value").ToString + "'"

                        Else
                            strInvoiceType = "'" + dtt.Rows(i)("Value").ToString + "'"

                        End If
                        reportfilt_INVOICETYPE = dtt.Rows(i)("Code").ToString + ":" + strInvoiceType
                    End If

                    If dtt.Rows(i)("Code").ToString = "TYPE" Then

                        If strType <> "" Then
                            strType = strType + ",'" + dtt.Rows(i)("Value").ToString + "'"

                        Else
                            strType = "'" + dtt.Rows(i)("Value").ToString + "'"

                        End If
                        reportfilt_type = dtt.Rows(i)("Code").ToString + ":" + strType
                    End If
                    If dtt.Rows(i)("Code").ToString = "CUSTOMER" Then


                        If strCustomer <> "" Then
                            strCustomer = strCustomer + ",'" + dtt.Rows(i)("Value").ToString + "'"

                        Else
                            strCustomer = "'" + dtt.Rows(i)("Value").ToString + "'"

                        End If
                        reportfilt_CUSTOMER = dtt.Rows(i)("Code").ToString + ":" + strCustomer

                    End If
                    If dtt.Rows(i)("Code").ToString = "SUPPLIER" Then



                        If strSupplier <> "" Then
                            strSupplier = strSupplier + ",'" + dtt.Rows(i)("Value").ToString + "'"

                        Else
                            strSupplier = "'" + dtt.Rows(i)("Value").ToString + "'"

                        End If
                        reportfilt_SUPPLIER = dtt.Rows(i)("Code").ToString + ":" + strSupplier

                    End If
                    If dtt.Rows(i)("Code").ToString = "SALESMAN" Then



                        If strSalesMan <> "" Then
                            strSalesMan = strSalesMan + ",'" + dtt.Rows(i)("Value").ToString + "'"

                        Else
                            strSalesMan = "'" + dtt.Rows(i)("Value").ToString + "'"

                        End If
                        reportfilt_SALESMAN = dtt.Rows(i)("Code").ToString + ":" + strSalesMan
                    End If
                    If dtt.Rows(i)("Code").ToString = "REFERENCENO" Then



                        If strReferenceNo <> "" Then
                            strReferenceNo = strReferenceNo + ",'" + dtt.Rows(i)("Value").ToString + "'"

                        Else
                            strReferenceNo = "'" + dtt.Rows(i)("Value").ToString + "'"

                        End If
                        reportfilt_REFERENCENO = dtt.Rows(i)("Code").ToString + ":" + strReferenceNo
                    End If
                    If dtt.Rows(i)("Code").ToString = "SOURCECOUNTRY" Then



                        If strSourceCountry <> "" Then
                            strSourceCountry = strSourceCountry + ",'" + dtt.Rows(i)("Value").ToString + "'"

                        Else
                            strSourceCountry = "'" + dtt.Rows(i)("Value").ToString + "'"

                        End If
                        reportfilt_SOURCECOUNTRY = dtt.Rows(i)("Code").ToString + ":" + strSourceCountry
                    End If
                    If dtt.Rows(i)("Code").ToString = "BOOKINGNO" Then



                        If strBookingNo <> "" Then
                            strBookingNo = strBookingNo + ",'" + dtt.Rows(i)("Value").ToString + "'"

                        Else
                            strBookingNo = "'" + dtt.Rows(i)("Value").ToString + "'"

                        End If

                        reportfilt_BOOKINGNO = dtt.Rows(i)("Code").ToString + ":" + strBookingNo
                    End If
                    If dtt.Rows(i)("Code").ToString = "TEXT" Then

                        If strTextValue <> "" Then
                            strTextValue = strTextValue + "," + dtt.Rows(i)("Value").ToString

                        Else
                            strTextValue = dtt.Rows(i)("Value").ToString

                        End If
                        reportfilt_text = dtt.Rows(i)("Code").ToString + " like " + strTextValue
                    End If
                Next
            End If
            If txtFromDate.Text.Trim <> "" And txtToDate.Text <> "" Then

                If ddlOrder.SelectedValue = "C" Then
                    reportfilt_date = "Created  FromDate:" & txtFromDate.Text & " ToDate:" & txtToDate.Text
                   
                ElseIf ddlOrder.SelectedValue = "M" Then
                    reportfilt_date = "Modified  FromDate:" & txtFromDate.Text & " ToDate:" & txtToDate.Text
                ElseIf ddlOrder.SelectedValue = "T" Then
                    reportfilt_date = "Transaction  FromDate:" & txtFromDate.Text & " ToDate:" & txtToDate.Text
                  
                End If
            End If

            strBindCondition = BuildConditionNew(strInvoiceType, strType, strInvoiceNovalue, strCustomer, strSupplier, strSalesMan, strReferenceNo, strSourceCountry, strBookingNo, strnarration, strTextValue)
            reportfilt = ""

            Dim strorderby As String = "d.tran_id"
            Dim strsortorder As String = "DESC"
            strSqlQry1 = ""
            If strBindCondition <> "" Then
                strSqlQry1 = " and " & strBindCondition



                If reportfilt_invoice <> "" Then
                    reportfilt = reportfilt_invoice
                End If
                If reportfilt_NARRATION <> "" Then
                    reportfilt = reportfilt + " " + reportfilt_NARRATION
                End If
                If reportfilt_type <> "" Then
                    reportfilt = reportfilt + " " + reportfilt_type
                End If

                If reportfilt_INVOICETYPE <> "" Then
                    reportfilt = reportfilt + " " + reportfilt_INVOICETYPE
                End If
                If reportfilt_CUSTOMER <> "" Then
                    reportfilt = reportfilt + " " + reportfilt_CUSTOMER
                End If
                If reportfilt_SUPPLIER <> "" Then
                    reportfilt = reportfilt + " " + reportfilt_SUPPLIER
                End If
                If reportfilt_SALESMAN <> "" Then
                    reportfilt = reportfilt + " " + reportfilt_SALESMAN
                End If
                If reportfilt_REFERENCENO <> "" Then
                    reportfilt = reportfilt + " " + reportfilt_REFERENCENO
                End If
                If reportfilt_SOURCECOUNTRY <> "" Then
                    reportfilt = reportfilt + " " + reportfilt_SOURCECOUNTRY
                End If
                If reportfilt_BOOKINGNO <> "" Then
                    reportfilt = reportfilt + " " + reportfilt_BOOKINGNO
                End If
                If reportfilt_text <> "" Then
                    reportfilt = reportfilt + " " + reportfilt_text
                End If


                reportfilt = "ReportFilter:" + reportfilt + " " + reportfilt_date
            Else
                reportfilt = "ReportFilter:All" + " " + reportfilt_date

            End If
            strorderby = " ORDER BY " & strorderby & " " & strsortorder
            FillGridNew_report = strSqlQry1 & ";" & strorderby & ";" & reportfilt

        Catch ex As Exception
            FillGridNew_report = ""
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("salesfreeformSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            'clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
            'clsDBConnect.dbConnectionClose(SqlConn)                          'Close connection
        End Try

    End Function
    Private Sub detailedReport()
        Dim FolderPath As String = "..\ExcelTemplates\"
        Dim FileName As String = "salesinvoicefreeformDETAILED_template.xlsx"
        Dim FilePath As String = Server.MapPath(FolderPath + FileName)
        Dim RandomCls As New Random()
        Dim RandomNo As String = RandomCls.Next(100000, 9999999).ToString
        Dim rptcompanyname, basecurrency As String

        rptcompanyname = CType(objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select division_master_des from division_master where division_master_code='" & txtdivcode.Value & "'"), String)
        basecurrency = CType(objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id=457"), String)


        Dim FileNameNew As String



        Select Case ViewState("CNDNOpen_type")
            Case "IN", "MN"
                FileNameNew = "salesinvoicefreeform_DETAILED" & Now.Year & Now.Month & Now.Day & Now.Hour & Now.Minute & Now.Second & Now.Millisecond & ".xlsx"


            Case "PI"
                FileNameNew = "PurchaseInvoice_DETAILED" & Now.Year & Now.Month & Now.Day & Now.Hour & Now.Minute & Now.Second & Now.Millisecond & ".xlsx"


            Case "PE"
                FileNameNew = "PurchaseExpense_DETAILED" & Now.Year & Now.Month & Now.Day & Now.Hour & Now.Minute & Now.Second & Now.Millisecond & ".xlsx"


            Case "DN"
                FileNameNew = "DebitNote_DETAILED" & Now.Year & Now.Month & Now.Day & Now.Hour & Now.Minute & Now.Second & Now.Millisecond & ".xlsx"


            Case "CN"
                FileNameNew = "CreditNote_DETAILED" & Now.Year & Now.Month & Now.Day & Now.Hour & Now.Minute & Now.Second & Now.Millisecond & ".xlsx"


        End Select



        document = New XLWorkbook(FilePath)
        Dim ws As IXLWorksheet = document.Worksheet("sales_register")
        ws.Style.Font.FontName = "arial"

        'Dim SheetTemplate As IXLWorksheet = New XLWorkbook(FilePath).Worksheet("Offer Template")
        'SheetTemplate.Style.Font.FontName = "Trebuchet MS"
        'Dim PartyName As String = ""
        'Dim CatName As String = ""
        'Dim SectorCityName As String = ""

        Dim LastLine As Integer
        ws.Cell(1, 1).Value = rptcompanyname



        Select Case ViewState("CNDNOpen_type")
            Case "IN", "MN"
                ws.Cell(2, 1).Value = "Sales Register:Detailed Report"
                ws.Cell(6, 1).Value = "InvoiceNo"
            Case "PI"
                ws.Cell(2, 1).Value = "Purchase Invoice:Detailed Report"
                ws.Cell(6, 1).Value = "PINo"
            Case "PE"
                ws.Cell(2, 1).Value = "Purchase Expense:Detailed Report"
                ws.Cell(6, 1).Value = "PENo"
            Case "DN"
                ws.Cell(2, 1).Value = "Debit Note:Detailed Report"
                ws.Cell(6, 1).Value = "DNNo"
            Case "CN"
                ws.Cell(2, 1).Value = "Credit Note:Detailed Report"
                ws.Cell(6, 1).Value = "CNNo"
        End Select



        ws.Cell(6, 10).Value = ws.Cell(6, 10).Value & " (" & basecurrency & ")"
        ws.Cell(6, 11).Value = ws.Cell(6, 11).Value & " (" & basecurrency & ")"
        ws.Cell(6, 14).Value = ws.Cell(6, 14).Value & " (" & basecurrency & ")"
        ws.Cell(6, 15).Value = ws.Cell(6, 15).Value & " (" & basecurrency & ")"


        Dim wherecond As String()

        wherecond = FillGridNew_report().Split(";")
        ws.Cell(4, 1).Value = wherecond(2)

        SqlConn = clsDBConnect.dbConnectionnew("strDBConnection") 'Tanvir 17052023
        mySqlCmd = New SqlCommand("sp_rep_salesinvfreeform_register", SqlConn)
        mySqlCmd.CommandType = CommandType.StoredProcedure
        mySqlCmd.Parameters.Add(New SqlParameter("@div_code", SqlDbType.VarChar, 20)).Value = txtdivcode.Value.Trim
        mySqlCmd.Parameters.Add(New SqlParameter("@tran_type", SqlDbType.VarChar, 20)).Value = IIf(CType(ViewState("CNDNOpen_type"), String) = "MN", "IN", CType(ViewState("CNDNOpen_type"), String))  'ViewState("CNDNOpen_type") 'Tanvir 17052023
        mySqlCmd.Parameters.Add(New SqlParameter("@wherecond", SqlDbType.NVarChar)).Value = wherecond(0)
        mySqlCmd.Parameters.Add(New SqlParameter("@orderby", SqlDbType.NVarChar)).Value = wherecond(1)
        mySqlCmd.Parameters.Add(New SqlParameter("@reporttype", SqlDbType.NVarChar)).Value = ddlrpt.SelectedValue

        mySqlCmd.CommandTimeout = 0
        myDataAdapter = New SqlDataAdapter
        myDataAdapter.SelectCommand = mySqlCmd
        Dim ds As New DataSet
        Dim dt As New DataTable
        Dim dt_row As New DataTable
        Dim dt_sum As New DataTable
        myDataAdapter.Fill(ds)

        dt = ds.Tables(0)
        dt_sum = ds.Tables(1)
        LastLine = 7

        If dt.Rows.Count > 0 Then
            For i As Integer = 0 To dt.Rows.Count - 1


                dt_row = dt.Clone()
                dt_row.ImportRow(dt.Rows(i))




                Dim RateSheet As IXLRange
                RateSheet = ws.Cell(LastLine, 1).InsertTable(dt_row.AsEnumerable).SetShowHeaderRow(False).AsRange()
                ws.Rows(LastLine).Clear()
                ws.Rows(LastLine).Delete()
                RateSheet.Style.Font.FontName = "arial"
                RateSheet.Columns("9:15").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right
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
                sqlst = "select '','','',a.acctname,'','','','',d.amount,d.taxamt*m.convrate,d.nontaxamt*m.convrate,d.vattype,d.vatperc,d.vatamt*m.convrate,d.baseamount,d.particulars " _
                & " from freeforminvoice_detail d left join acctmast a on  a.div_code= d.div_code and a.acctcode=d.acc_code" _
                & " left join freeforminvoice_master  m on m.div_code=d.div_code and m.tran_id=d.tran_id and m.tran_type=d.tran_type" _
                & " where d.div_code='" & txtdivcode.Value.Trim & "' and d.tran_type='" & IIf(ViewState("CNDNOpen_type") = "MN", "IN", ViewState("CNDNOpen_type")) & "' and d.tran_id='" & id & "'"
                dt_detail_ds = objUtils.GetDataFromDatasetnew(Session("dbconnectionName"), sqlst)
                dt_detail = dt_detail_ds.Tables(0)

                Dim RateSheet_detail As IXLRange
                RateSheet_detail = ws.Cell(LastLine, 1).InsertTable(dt_detail.AsEnumerable).SetShowHeaderRow(False).AsRange()
                ws.Rows(LastLine).Clear()
                ws.Rows(LastLine).Delete()
                RateSheet_detail.Style.Font.FontName = "arial"
                RateSheet_detail.Columns("9:15").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right
                Dim style3 As IXLBorder = RateSheet_detail.Cells.Style.Border
                style3.BottomBorder = XLBorderStyleValues.Thin
                style3.LeftBorder = XLBorderStyleValues.Thin
                RateSheet_detail.Style.Border.OutsideBorder = XLBorderStyleValues.Thin



                LastLine = LastLine + dt_detail.Rows.Count
                LastLine = LastLine + 1

            Next



            Dim RateSheet_summ As IXLRange
            RateSheet_summ = ws.Cell(LastLine, 1).InsertTable(dt_sum.AsEnumerable).SetShowHeaderRow(False).AsRange()
            ws.Rows(LastLine).Clear()
            ws.Rows(LastLine).Delete()
            RateSheet_summ.Style.Font.FontName = "arial"
            RateSheet_summ.Columns("9:15").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right
            Dim style2 As IXLBorder = RateSheet_summ.Cells.Style.Border
            style2.BottomBorder = XLBorderStyleValues.Thin
            style2.LeftBorder = XLBorderStyleValues.Thin
            RateSheet_summ.Style.Border.OutsideBorder = XLBorderStyleValues.Thin

            RateSheet_summ.Style.Font.Bold = True


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
    Protected Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrint.Click

        Try
            If ddlrpt.SelectedValue = "Detailed" Then
                detailedReport()

            Else



                Dim FolderPath As String = "..\ExcelTemplates\"
                Dim FileName As String = "salesinvoicefreeform_template.xlsx"
                Dim FilePath As String = Server.MapPath(FolderPath + FileName)
                Dim RandomCls As New Random()
                Dim RandomNo As String = RandomCls.Next(100000, 9999999).ToString
                Dim rptcompanyname, basecurrency As String

                rptcompanyname = CType(objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select division_master_des from division_master where division_master_code='" & txtdivcode.Value & "'"), String)
                basecurrency = CType(objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id=457"), String)


                Dim FileNameNew As String



                Select Case ViewState("CNDNOpen_type")
                    Case "IN", "MN"
                        FileNameNew = "salesinvoicefreeform_Brief" & Now.Year & Now.Month & Now.Day & Now.Hour & Now.Minute & Now.Second & Now.Millisecond & ".xlsx"


                    Case "PI"
                        FileNameNew = "PurchaseInvoice_Brief" & Now.Year & Now.Month & Now.Day & Now.Hour & Now.Minute & Now.Second & Now.Millisecond & ".xlsx"


                    Case "PE"
                        FileNameNew = "PurchaseExpense_Brief" & Now.Year & Now.Month & Now.Day & Now.Hour & Now.Minute & Now.Second & Now.Millisecond & ".xlsx"


                    Case "DN"
                        FileNameNew = "DebitNote_Brief" & Now.Year & Now.Month & Now.Day & Now.Hour & Now.Minute & Now.Second & Now.Millisecond & ".xlsx"


                    Case "CN"
                        FileNameNew = "CreditNote_Brief" & Now.Year & Now.Month & Now.Day & Now.Hour & Now.Minute & Now.Second & Now.Millisecond & ".xlsx"


                End Select
                document = New XLWorkbook(FilePath)
                Dim ws As IXLWorksheet = document.Worksheet("sales_register")
                ws.Style.Font.FontName = "arial"

                'Dim SheetTemplate As IXLWorksheet = New XLWorkbook(FilePath).Worksheet("Offer Template")
                'SheetTemplate.Style.Font.FontName = "Trebuchet MS"
                'Dim PartyName As String = ""
                'Dim CatName As String = ""
                'Dim SectorCityName As String = ""

                Dim LastLine As Integer
                ws.Cell(1, 1).Value = rptcompanyname



                Select Case ViewState("CNDNOpen_type")
                    Case "IN", "MN"
                        ws.Cell(2, 1).Value = "Sales Register:Brief Report"
                        ws.Cell(6, 1).Value = "InvoiceNo"
                    Case "PI"
                        ws.Cell(2, 1).Value = "Purchase Invoice:Brief Report"
                        ws.Cell(6, 1).Value = "PINo"
                    Case "PE"
                        ws.Cell(2, 1).Value = "Purchase Expense:Brief Report"
                        ws.Cell(6, 1).Value = "PENo"
                    Case "DN"
                        ws.Cell(2, 1).Value = "Debit Note:Brief Report"
                        ws.Cell(6, 1).Value = "DNNo"
                    Case "CN"
                        ws.Cell(2, 1).Value = "Credit Note:Brief Report"
                        ws.Cell(6, 1).Value = "CNNo"
                End Select



                ws.Cell(6, 10).Value = ws.Cell(6, 10).Value & " (" & basecurrency & ")"
                ws.Cell(6, 11).Value = ws.Cell(6, 11).Value & " (" & basecurrency & ")"
                ws.Cell(6, 12).Value = ws.Cell(6, 12).Value & " (" & basecurrency & ")"
                ws.Cell(6, 13).Value = ws.Cell(6, 13).Value & " (" & basecurrency & ")"

                Dim sqlConn As New SqlConnection
                Dim mySqlCmd As New SqlCommand
                Dim myDataAdapter As New SqlDataAdapter
                Dim wherecond As String()
                sqlConn = clsDBConnect.dbConnectionnew("strDBConnection") 'Tanvir 17052023
                wherecond = FillGridNew_report().Split(";")

                ws.Cell(4, 1).Value = wherecond(2)
                mySqlCmd = New SqlCommand("sp_rep_salesinvfreeform_register", SqlConn)
                mySqlCmd.CommandType = CommandType.StoredProcedure
                mySqlCmd.Parameters.Add(New SqlParameter("@div_code", SqlDbType.VarChar, 20)).Value = txtdivcode.Value.Trim
                mySqlCmd.Parameters.Add(New SqlParameter("@tran_type", SqlDbType.VarChar, 20)).Value = IIf(CType(ViewState("CNDNOpen_type"), String) = "MN", "IN", CType(ViewState("CNDNOpen_type"), String))  'ViewState("CNDNOpen_type") 'Tanvir 17052023
                mySqlCmd.Parameters.Add(New SqlParameter("@wherecond", SqlDbType.NVarChar)).Value = wherecond(0)
                mySqlCmd.Parameters.Add(New SqlParameter("@orderby", SqlDbType.NVarChar)).Value = wherecond(1)
                mySqlCmd.Parameters.Add(New SqlParameter("@reporttype", SqlDbType.NVarChar)).Value = ddlrpt.SelectedValue

                mySqlCmd.CommandTimeout = 0
                myDataAdapter = New SqlDataAdapter
                myDataAdapter.SelectCommand = mySqlCmd
                Dim ds As New DataSet
                Dim dt As New DataTable

                myDataAdapter.Fill(ds)

                dt = ds.Tables(0)

                Dim RateSheet As IXLRange
                RateSheet = ws.Cell(7, 1).InsertTable(dt.AsEnumerable).SetShowHeaderRow(False).AsRange()
                ws.Rows(7).Clear()
                ws.Rows(7).Delete()
                LastLine = 7 + dt.Rows.Count

                If dt.Rows.Count > 1 Then
                    ws.Range(LastLine, 1, LastLine, 14).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                    ws.Range(LastLine, 1, LastLine, 14).Style.Border.SetLeftBorder(XLBorderStyleValues.Thin)
                    ws.Range(LastLine, 1, LastLine, 14).Style.Font.Bold = True
                    ws.Cell(LastLine, 7).Value = "Total"
                    ws.Cell(LastLine, 7).Style.Font.Bold = True
                    RateSheet.Columns("9:13").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right

                    RateSheet.Columns("9:13").SetDataType(XLCellValues.Number)
                    ws.Cell(LastLine, 10).SetFormulaR1C1("=SUM(J7:J" & LastLine - 1 & ")")
                    ws.Cell(LastLine, 11).SetFormulaR1C1("=SUM(K7:K" & LastLine - 1 & ")")
                    ws.Cell(LastLine, 12).SetFormulaR1C1("=SUM(L7:L" & LastLine - 1 & ")")
                    ws.Cell(LastLine, 13).SetFormulaR1C1("=SUM(M7:M" & LastLine - 1 & ")")
                End If


                RateSheet.Style.Font.FontName = "arial"

                RateSheet.Columns("9:13").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right
                Dim style1 As IXLBorder = RateSheet.Cells.Style.Border
                style1.BottomBorder = XLBorderStyleValues.Thin
                style1.LeftBorder = XLBorderStyleValues.Thin

                RateSheet.Style.Border.OutsideBorder = XLBorderStyleValues.Medium










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
            End If
        Catch ex As Exception

            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("RptPriceList.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try



    End Sub

#Region "Protected Sub RowsPerPageCUS_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RowsPerPageCUS.SelectedIndexChanged"
    Protected Sub RowsPerPageCUS_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RowsPerPageCUS.SelectedIndexChanged
        FillGridNew()
    End Sub
#End Region

End Class
