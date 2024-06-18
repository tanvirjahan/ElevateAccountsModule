'------------------------------------------------------------------------------------------------
'   Module Name    :    ReceiptsSearch 
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
Partial Class ReceiptSearchNew
    Inherits System.Web.UI.Page

    Dim objDate As New clsDateTime
    Enum GridCol

        tran_id = 0
        tran_type = 1
        receipt_date = 2
        status = 3
        receipt_currency_id = 4
        receipt_received_from = 5
        other_bank_master_des = 6
        receipt_cheque_number = 7
        receipt_cheque_date = 8
        receipt_customerbank = 9
        receipt_credit = 10
        receipt_narration = 11
        adddate = 12
        adduser = 13
        moddate = 14
        moduser = 15
        Edit = 16
        View = 17
        Delete = 18
        Copy = 19
        chqprint = 21

    End Enum

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

    Dim dtt As DataTable
    Dim reportfilter As String

    Dim mySqlCmd As SqlCommand

    Dim mySqlReader As SqlDataReader

    Dim document As New XLWorkbook

#End Region
    'Tanvir 17012023
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
    'Tanvir 17012023
    Protected Sub btnFlightCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs)

   

    End Sub

    'Tanvir 17012023
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

    'Tanvir 17012023
 
    'Protected Sub Button1_Click(sender As Object, e As System.EventArgs) Handles Button1.Click
    '    Dim strpop As String = ""
    '    Dim actionstr As String = ""

    '    actionstr = "New"
    '    strpop = "window.open('Receipts.aspx?State=" + CType(actionstr, String) + "&RefCode=" + CType("", String) + "&divid=" + txtdivcode.Value + "&RVPVTranType=" + CType(ViewState("ReceiptsSearchRVPVTranType"), String) + "','Receipts','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
    '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
    'End Sub
    'Response.Redirect("Receipts.aspx", False)
    Protected Sub lbEditDate_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim lbEditDate As LinkButton = CType(sender, LinkButton)
        Dim gvr As GridViewRow = lbEditDate.NamingContainer



        Dim lblDocNo As Label = CType(gvr.FindControl("lblDocNo"), Label)
        Dim lblTranType As Label = CType(gvr.FindControl("lblTranType"), Label)
        Dim lbltranid As Label = CType(gvr.FindControl("lbltranid"), Label)

        Dim lblTranTypedate As Label = CType(gvr.FindControl("lblTrandate"), Label)
        hdntranid.Value = lbltranid.Text
        hdntrantypeDate.Value = lblTranTypedate.Text
        txtdate.Text = hdntrantypeDate.Value
        hdntrantype.Value = lblTranType.Text
        'Tanvir 19012024
        If CType(ViewState("ReceiptsSearchRVPVTranType"), String) = "BPV" Then

            lblViewDetailsPopupHeading.Text = "Edit BPV Date"
            lblpopupreceipt.Text = "BPV Date"

        ElseIf CType(ViewState("ReceiptsSearchRVPVTranType"), String) = "RV" Then

            lblViewDetailsPopupHeading.Text = "Edit Receipt Date"
            lblpopupreceipt.Text = "Receipt Date"
        End If
        'End If
        'Tanvir 19012024
        ModalFlightDetails.Show()
    End Sub

    'Tanvir 17012023
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
            objUtils.WritErrorLog("Receipts.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            clsDBConnect.dbCommandClose(myCommand)
            clsDBConnect.dbConnectionClose(SqlConn)
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
        If txtdivcode.Value = "01" Then
            appname = "ColumbusCommon" + " " + CType("Accounts Module", String)

            appidnew = "4"
        Else
            appname = "ColumbusCommon Gulf " + CType("Accounts Module", String)
            appidnew = "14"
        End If

        CheckPostUnpostRight(CType(Session("GlobalUserName"), String), CType(Session("Userpwd"), String), CType(appname, String), "AccountsModule\ReceiptSearchNew.aspx?tran_type=" & hdntrantype.Value & "&appid=" + appidnew, appidnew)

        If Validateseal(hdntranid.Value) Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Document has sealed cannot Edit/Delete...')", True)
            Return
        End If
        If ValidatesealDate()= False  Then
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

        myCommand = New SqlCommand("sp_update_receipt_date", SqlConn, sqlTrans)
        myCommand.CommandType = CommandType.StoredProcedure
        myCommand.Parameters.Add(New SqlParameter("@receipt_div_id", SqlDbType.VarChar, 10)).Value = "01"
        myCommand.Parameters.Add(New SqlParameter("@tran_id", SqlDbType.VarChar, 20)).Value = hdntranid.Value
        myCommand.Parameters.Add(New SqlParameter("@tran_type", SqlDbType.VarChar, 10)).Value = hdntrantype.Value 'CType(ViewState("ReceiptsRVPVTranType"), String)
        myCommand.Parameters.Add(New SqlParameter("@receipt_date", SqlDbType.DateTime)).Value = Format(CType(txtdate.Text, Date), "yyyy/MM/dd")
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
#Region "Protected Sub btnAddNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddNew.Click"
    Protected Sub btnAddNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddNew.Click
        'Session.Add("State", "New")
        'Response.Redirect("Receipts.aspx", False)
        Dim strpop As String = ""
        Dim actionstr As String = ""
        actionstr = "New"
        '
        If ViewState("ReceiptsNewMethod") = "2" Then
            strpop = "window.open('ReceiptsNewChange.aspx?State=" + CType(actionstr, String) + "&RefCode=" + CType("", String) + "&divid=" + txtdivcode.Value + "&RVPVTranType=" + CType(ViewState("ReceiptsSearchRVPVTranType"), String) + "','Receipts');"
        Else
            strpop = "window.open('ReceiptsNew.aspx?State=" + CType(actionstr, String) + "&RefCode=" + CType("", String) + "&divid=" + txtdivcode.Value + "&RVPVTranType=" + CType(ViewState("ReceiptsSearchRVPVTranType"), String) + "','Receipts');"
        End If

        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)

    End Sub



#End Region
#Region "Private Sub FillGrid(ByVal strorderby As String, Optional ByVal strsortorder As String = 'ASC')"
    Private Sub FillGrid(ByVal strorderby As String, Optional ByVal strsortorder As String = "ASC")
        Dim myDS As New DataSet

        gv_SearchResult.Visible = True
        'lblMsg.Visible = False

        If gv_SearchResult.PageIndex < 0 Then
            gv_SearchResult.PageIndex = 0
        End If
        strSqlQry = ""
        Try
            strSqlQry = "SELECT receipt_master_new.tran_id, receipt_master_new.tran_type, " & _
                            " case isnull(receipt_master_new.post_state,'') when 'P' then 'Posted' when 'U' then 'UnPosted' end  as post_state ," & _
                            " receipt_master_new.tran_type AS Expr1,  " & _
                            " convert(varchar(10),receipt_master_new.receipt_date,103)  as receipt_date, " & _
                            "  convert(varchar(10),receipt_master_new.receipt_tran_date,103) as receipt_tran_date, " & _
                            " case when isnull(tran_type,'')='RV' then receipt_master_new.receipt_credit when isnull(tran_type,'')='CPV' then  receipt_master_new.receipt_debit " & _
                            " when isnull(tran_type,'')='BPV' then receipt_master_new.receipt_debit when isnull(tran_type,'')='DEP' then receipt_master_new.receipt_debit else 0 end receipt_credit , " & _
                            " receipt_master_new.receipt_currency_id,   " & _
                            " receipt_master_new.receipt_received_from, receipt_master_new.receipt_cheque_number, receipt_master_new.receipt_cashbank_code, case when isnull(tran_type,'')='RV' then vw_bankname.acctname when isnull(tran_type,'')='CPV' then  acctmast.acctname when isnull(tran_type,'')='BPV' then vw_bankname.acctname  else vw_bankname.acctname end acctname, " & _
                             " customer_bank_master.other_bank_master_des,receipt_master_new.receipt_narration, receipt_master_new.adddate, receipt_master_new.adduser, receipt_master_new.moddate, " & _
                             " receipt_master_new.moduser, receipt_master_new.tran_id + '|' + receipt_master_new.tran_type tranidcomarg " & _
                             " FROM  receipt_master_new LEFT OUTER JOIN  " & _
                             " customer_bank_master ON receipt_master_new.receipt_customer_bank = customer_bank_master.other_bank_master_code  " & _
                             " left join vw_bankname on receipt_master_new.receipt_cashbank_code =vw_bankname.acctcode  and  vw_bankname.div_code=receipt_master_new.receipt_div_id  left outer join acctmast  on acctmast.div_code=receipt_master_new.receipt_div_id and acctmast.acctcode=receipt_master_new.receipt_cashbank_code  where  receipt_master_new.receipt_div_id='" & ViewState("divcode") & "' and  receipt_master_new.tran_type='" & CType(ViewState("ReceiptsSearchRVPVTranType"), String) & "'"

            Dim strBuild As String = Trim(BuildCondition)
            If strBuild <> "" Then
                strSqlQry = strSqlQry & " and " & strBuild & " ORDER BY " & strorderby & " " & strsortorder
            Else
                strSqlQry = strSqlQry & " ORDER BY " & strorderby & " " & strsortorder
            End If


            'If Trim(BuildCondition) <> "" Then
            '    strSqlQry = strSqlQry & " And " & BuildCondition() & " ORDER BY " & strorderby & " " & strsortorder
            'Else
            '    strSqlQry = strSqlQry & " ORDER BY " & strorderby & " " & strsortorder
            'End If

            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)
            gv_SearchResult.DataSource = myDS


            If myDS.Tables(0).Rows.Count > 0 Then
                gv_SearchResult.PageSize = RowsPerPageCUS.SelectedValue
                gv_SearchResult.DataBind()
                lblMsg.Visible = False
            Else
                gv_SearchResult.PageIndex = 0
                gv_SearchResult.DataBind()
                lblMsg.Visible = True
                lblMsg.Text = "Records not found, Please redefine search criteria."
            End If
            gv_SearchResult.Columns(0).Visible = True
            'UpdatePanel2.Update()
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ReceiptSearchNew.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            'clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
            'clsDBConnect.dbConnectionClose(SqlConn)                          'Close connection
        End Try

    End Sub
#End Region

#Region "Private Function BuildCondition1() As String"
    Private Function BuildCondition1() As String
        strWhereCond = ""

        ''If txtFromDate.Text.Trim <> "" And txtTodate.Text.Trim <> "" Then
        ''    If Trim(strWhereCond) = "" Then
        ''        strWhereCond = "( (convert(varchar(10),receipt_master_new.receipt_tran_date,111) between convert(varchar(10), '" & Format(CType(txtFromDate.Text, Date), "yyyy/MM/dd") & "',111) " _
        ''        & "  and  convert(varchar(10), '" & Format(CType(txtTodate.Text, Date), "yyyy/MM/dd") & "',111) ) " _
        ''        & " or (convert(varchar(10),receipt_master_new.receipt_tran_date,111) < convert(varchar(10), '" & Format(CType(txtFromDate.Text, Date), "yyyy/MM/dd") & "',111) " _
        ''        & "  and convert(varchar(10),receipt_master_new.receipt_tran_date,111) > convert(varchar(10), '" & Format(CType(txtTodate.Text, Date), "yyyy/MM/dd") & "',111)) )"
        ''    Else
        ''        strWhereCond = strWhereCond & " and ( (convert(varchar(10),receipt_master_new.receipt_tran_date,111) between convert(varchar(10), '" & Format(CType(txtFromDate.Text, Date), "yyyy/MM/dd") & "',111) " _
        ''        & "  and  convert(varchar(10), '" & Format(CType(txtTodate.Text, Date), "yyyy/MM/dd") & "',111) ) " _
        ''        & " or (convert(varchar(10),receipt_master_new.receipt_tran_date,111) < convert(varchar(10), '" & Format(CType(txtFromDate.Text, Date), "yyyy/MM/dd") & "',111) " _
        ''        & "  and convert(varchar(10),receipt_master_new.receipt_tran_date,111) > convert(varchar(10), '" & Format(CType(txtTodate.Text, Date), "yyyy/MM/dd") & "',111)) )"

        ''    End If
        ''End If


        BuildCondition1 = strWhereCond

    End Function
#End Region

    Private Function BuildConditionNew(ByVal straccountvalue As String, ByVal strdocumentvalue As String, ByVal strcurrvalue As String, ByVal strchequevalue As String, ByVal strbankvalue As String, ByVal strrcvfrmvalue As String, ByVal strnarrationvalue As String, ByVal strStatusvalue As String, ByVal strTextValue As String) As String
        strWhereCond = ""
        If straccountvalue.Trim <> "" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = "upper(receipt_master_new.receipt_customer_bank) IN (" & Trim(straccountvalue.Trim.ToUpper) & ")"
            Else
                strWhereCond = strWhereCond & " AND upper(receipt_master_new.receipt_customer_bank) IN (" & Trim(straccountvalue.Trim.ToUpper) & ")"
            End If
        End If
        If strdocumentvalue.Trim <> "" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = "upper(receipt_master_new.tran_id) IN (" & Trim(strdocumentvalue.Trim.ToUpper) & ")"
            Else
                strWhereCond = strWhereCond & " AND upper(receipt_master_new.tran_id) IN (" & Trim(strdocumentvalue.Trim.ToUpper) & ")"
            End If
        End If
        If strcurrvalue.Trim <> "" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = "upper(receipt_master_new.receipt_currency_id) IN (" & Trim(strcurrvalue.Trim.ToUpper) & ")"
            Else
                strWhereCond = strWhereCond & " AND upper(receipt_master_new.receipt_currency_id) IN (" & Trim(strcurrvalue.Trim.ToUpper) & ")"
            End If
        End If
        If strrcvfrmvalue.Trim <> "" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = "upper(receipt_master_new.receipt_received_from) like ('%" & Trim(strrcvfrmvalue.Trim.ToUpper) & "%')"
            Else
                strWhereCond = strWhereCond & " AND upper(receipt_master_new.receipt_received_from) like ('%" & Trim(strrcvfrmvalue.Trim.ToUpper) & "%')"
            End If
        End If
        If strnarrationvalue.Trim <> "" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = "upper(receipt_master_new.receipt_narration) like  ('%" & Trim(strnarrationvalue.Trim.ToUpper) & "%')"
            Else
                strWhereCond = strWhereCond & " AND upper(receipt_master_new.receipt_narration) Like  ('%" & Trim(strnarrationvalue.Trim.ToUpper) & "%')"
            End If
        End If
        If strchequevalue.Trim <> "" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = "upper(receipt_master_new.receipt_cheque_number) IN (" & Trim(strchequevalue.Trim.ToUpper) & ")"
            Else
                strWhereCond = strWhereCond & " AND upper(receipt_master_new.receipt_cheque_number) IN (" & Trim(strchequevalue.Trim.ToUpper) & ")"
            End If
        End If
        If strbankvalue.Trim <> "" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = "upper(receipt_master_new.receipt_cashbank_code) IN (" & Trim(strbankvalue.Trim.ToUpper) & ")"
            Else
                strWhereCond = strWhereCond & " AND upper(receipt_master_new.receipt_cashbank_code) IN (" & Trim(strbankvalue.Trim.ToUpper) & ")"
            End If
        End If
        If strStatusvalue.Trim <> "" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = "upper(receipt_master_new.post_state) IN (" & Trim(strStatusvalue.Trim.ToUpper) & ")"
            Else
                strWhereCond = strWhereCond & " AND upper(receipt_master_new.post_state) IN (" & Trim(strStatusvalue.Trim.ToUpper) & ")"
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

                            strWhereCond1 = "upper(receipt_master_new.receipt_customer_bank) LIKE '%" & Trim(strPOSTED.Trim.ToUpper) & "%' or  upper(receipt_master_new.tran_id) LIKE '%" & Trim(strPOSTED.Trim.ToUpper) & "%'  or upper(receipt_master_new.receipt_cashbank_code) LIKE '%" & Trim(strPOSTED.Trim.ToUpper) & "%' or upper(receipt_master_new.post_state) LIKE '%" & Trim(strValue2.Trim.ToUpper) & "%' and tran_type='" & ViewState("ReceiptsSearchRVPVTranType") & "'"
                        Else
                            strWhereCond1 = strWhereCond1 & " OR  upper(receipt_master_new.receipt_customer_bank) LIKE '%" & Trim(strPOSTED.Trim.ToUpper) & "%' or  upper(receipt_master_new.tran_id) LIKE '%" & Trim(strPOSTED.Trim.ToUpper) & "%'  or upper(receipt_master_new.receipt_cashbank_code) LIKE '%" & Trim(strPOSTED.Trim.ToUpper) & "%'  or upper(receipt_master_new.post_state) LIKE '%" & Trim(strValue2.Trim.ToUpper) & "%' and tran_type='" & ViewState("ReceiptsSearchRVPVTranType") & "'"
                        End If

                    ElseIf strValue = "UNPOSTED" Then
                        strValue2 = "U"

                        If Trim(strWhereCond1) = "" Then

                            strWhereCond1 = "upper(receipt_master_new.receipt_customer_bank) LIKE '%" & Trim(strUNPOSTED.Trim.ToUpper) & "%' or  upper(receipt_master_new.tran_id) LIKE '%" & Trim(strUNPOSTED.Trim.ToUpper) & "%' or upper(receipt_master_new.receipt_cashbank_code) LIKE '%" & Trim(strUNPOSTED.Trim.ToUpper) & "%'  or upper(receipt_master_new.post_state) LIKE '%" & Trim(strValue2.Trim.ToUpper) & "%' and tran_type='" & ViewState("ReceiptsSearchRVPVTranType") & "'"
                        Else
                            strWhereCond1 = strWhereCond1 & " OR  upper(receipt_master_new.receipt_customer_bank) LIKE '%" & Trim(strUNPOSTED.Trim.ToUpper) & "%' or  upper(receipt_master_new.tran_id) LIKE '%" & Trim(strUNPOSTED.Trim.ToUpper) & "%'  or upper(receipt_master_new.tran_id) LIKE '%" & Trim(strUNPOSTED.Trim.ToUpper) & "%' or  upper(receipt_master_new.receipt_cashbank_code) LIKE '%" & Trim(strUNPOSTED.Trim.ToUpper) & "%' or upper(receipt_master_new.post_state) LIKE '%" & Trim(strValue2.Trim.ToUpper) & "%' and tran_type='" & ViewState("ReceiptsSearchRVPVTranType") & "'"
                        End If
                    Else

                        If Trim(strWhereCond1) = "" Then

                            strWhereCond1 = "upper(receipt_master_new.receipt_customer_bank) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' or  upper(receipt_master_new.tran_id) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%'  or upper(receipt_master_new.receipt_cashbank_code) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' or upper(receipt_master_new.post_state) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' and tran_type='" & ViewState("ReceiptsSearchRVPVTranType") & "' or   upper(receipt_master_new.receipt_cheque_number) like  ('%" & Trim(strValue.Trim.ToUpper) & "%')" & _
                              " or   upper(receipt_master_new.receipt_received_from) like  ('%" & Trim(strValue.Trim.ToUpper) & "%')   or upper(receipt_master_new.receipt_narration) like  ('%" & Trim(strValue.Trim.ToUpper) & "%')  or upper(receipt_master_new.receipt_currency_id) like  ('%" & Trim(strValue.Trim.ToUpper) & "%')  or upper(vw_bankname.acctname ) like ('%" & Trim(strValue.Trim.ToUpper) & "%') or  upper(receipt_master_new.receipt_credit) LIKE ('%" & Trim(strValue.Trim.ToUpper) & "%') or upper(customer_bank_master.other_bank_master_des) like ('%" & Trim(strValue.Trim.ToUpper) & "%') "
                        Else
                            strWhereCond1 = strWhereCond1 & " OR upper(receipt_master_new.receipt_customer_bank) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' or  upper(receipt_master_new.tran_id) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%'  or upper(receipt_master_new.receipt_cashbank_code) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' or upper(receipt_master_new.post_state) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' and tran_type='" & ViewState("ReceiptsSearchRVPVTranType") & "' or   upper(receipt_master_new.receipt_cheque_number) like  ('%" & Trim(strValue.Trim.ToUpper) & "%')" & _
                              " or   upper(receipt_master_new.receipt_received_from) like  ('%" & Trim(strValue.Trim.ToUpper) & "%')   or upper(receipt_master_new.receipt_narration) like  ('%" & Trim(strValue.Trim.ToUpper) & "%')  or upper(receipt_master_new.receipt_currency_id) like  ('%" & Trim(strValue.Trim.ToUpper) & "%')  or upper(vw_bankname.acctname ) like ('%" & Trim(strValue.Trim.ToUpper) & "%') or  upper(receipt_master_new.receipt_credit) LIKE ('%" & Trim(strValue.Trim.ToUpper) & "%') or upper(customer_bank_master.other_bank_master_des) like ('%" & Trim(strValue.Trim.ToUpper) & "%') "
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

                    strWhereCond = " (CONVERT(datetime, convert(varchar(10),receipt_master_new.adddate,103),103)  between CONVERT(datetime, '" + txtFromDate.Text + "',103) and CONVERT(datetime,  '" + txtToDate.Text + "',103)) "
                Else
                    strWhereCond = strWhereCond & "  and (CONVERT(datetime, convert(varchar(10),receipt_master_new.adddate,103),103)  between CONVERT(datetime, '" + txtFromDate.Text + "',103) and CONVERT(datetime,  '" + txtToDate.Text + "',103)) "
                End If
            ElseIf ddlOrder.SelectedValue = "M" Then
                If Trim(strWhereCond) = "" Then

                    strWhereCond = " (CONVERT(datetime, convert(varchar(10),receipt_master_new.moddate,103),103)  between CONVERT(datetime, '" + txtFromDate.Text + "',103) and CONVERT(datetime,  '" + txtToDate.Text + "',103)) "
                Else
                    strWhereCond = strWhereCond & "  and (CONVERT(datetime, convert(varchar(10),receipt_master_new.moddate,103),103)  between CONVERT(datetime, '" + txtFromDate.Text + "',103) and CONVERT(datetime,  '" + txtToDate.Text + "',103)) "
                End If
            ElseIf ddlOrder.SelectedValue = "R" Then

                If Trim(strWhereCond) = "" Then
                    strWhereCond = "(CONVERT(datetime, convert(varchar(10),receipt_master_new.receipt_date ,103),103)  between CONVERT(datetime, '" + txtFromDate.Text + "',103) and CONVERT(datetime,  '" + txtToDate.Text + "',103)) "
                Else
                    strWhereCond = strWhereCond & "  and (CONVERT(datetime, convert(varchar(10),receipt_master_new.receipt_date ,103),103)  between CONVERT(datetime, '" + txtFromDate.Text + "',103) and CONVERT(datetime,  '" + txtToDate.Text + "',103)) "
                End If
 
        ElseIf ddlOrder.SelectedValue = "RT" Then 'ChequeDate
            If Trim(strWhereCond) = "" Then
                strWhereCond = "(CONVERT(datetime, convert(varchar(10),receipt_master_new.receipt_tran_date ,103),103)  between CONVERT(datetime, '" + txtFromDate.Text + "',103) and CONVERT(datetime,  '" + txtToDate.Text + "',103)) "
            Else
                strWhereCond = strWhereCond & "  and (CONVERT(datetime, convert(varchar(10),receipt_master_new.receipt_tran_date ,103),103)  between CONVERT(datetime, '" + txtFromDate.Text + "',103) and CONVERT(datetime,  '" + txtToDate.Text + "',103)) "
            End If
             
            End If
        End If
        BuildConditionNew = strWhereCond
    End Function
    Function SetVisibility(ByVal desc As Object, ByVal maxlen As Integer) As Boolean

        If desc.ToString = "" Then
            Return False
        Else
            If desc.ToString.Length > maxlen Then
                Return True
            Else
                Return False
            End If
        End If


    End Function
    Function Limit(ByVal desc As Object, ByVal maxlen As Integer) As String

        If desc.ToString = "" Then
            Return ""
        Else
            If desc.ToString.Length > maxlen Then
                desc = desc.Substring(0, maxlen)
            Else

                desc = desc
            End If
        End If

        Return desc


    End Function
    Protected Sub ReadMoreLinkButton_Click(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Dim readmore As LinkButton = CType(sender, LinkButton)
            Dim row As GridViewRow = CType((CType(sender, LinkButton)).NamingContainer, GridViewRow)
            Dim lbtext As Label = CType(row.FindControl("lblNarration"), Label)
            Dim strtemp As String = ""
            strtemp = lbtext.Text
            If readmore.Text.ToUpper = UCase("More") Then

                lbtext.Text = lbtext.ToolTip
                lbtext.ToolTip = strtemp
                readmore.Text = "less"
            Else
                readmore.Text = "More"
                lbtext.ToolTip = lbtext.Text
                lbtext.Text = lbtext.Text.Substring(0, 50)
            End If

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ReceiptSearchNew.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
    Private Sub FillGridNew()

        dtt = Session("sDtDynamic")
        Dim strStatusvalue As String = ""
        Dim strbankvalue As String = ""
        Dim strTextValue As String = ""
        Dim strBindCondition As String = ""
        Dim straccountvalue As String = ""
        Dim strdocumentvalue As String = ""
        Dim strcurrvalue As String = ""
        Dim strchequevalue As String = ""
        Dim strrcvfrmvalue As String = ""
        Dim strnarrationvalue As String = ""
        'Session.Add("accountvalue", Request.QueryString("straccountvalue"))
        'Session.Add("documentvalue", Request.QueryString("strdocumentvalue"))
        'Session.Add("bankvalue", Request.QueryString("strbankvalue"))
        'Session.Add("Statusvalue", Request.QueryString("strStatusvalue"))
        'Session.Add("accountvalue", Request.QueryString("straccountvalue"))

        Try
            If dtt.Rows.Count > 0 Then
                For i As Integer = 0 To dtt.Rows.Count - 1
                    If dtt.Rows(i)("Code").ToString = "CUSTOMERBANK" Then

                        Dim dttrans As New DataTable
                        strSqlQry = "select other_bank_master_code as other_bank_master_code from customer_bank_master where  other_bank_master_des ='" & dtt.Rows(i)("value").ToString & "'"
                        SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                        myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
                        myDataAdapter.Fill(dttrans)
                        If dttrans.Rows.Count > 0 Then
                            For b As Integer = 0 To dttrans.Rows.Count - 1

                                If straccountvalue <> "" Then
                                    straccountvalue = straccountvalue + ",'" + dttrans.Rows(b)("other_bank_master_code").ToString + "'"
                                Else
                                    straccountvalue = "'" + dttrans.Rows(b)("other_bank_master_code").ToString + "'"
                                End If
                            Next
                        Else
                            If straccountvalue <> "" Then
                                straccountvalue = straccountvalue + ",'" + dtt.Rows(i)("Value").ToString + "'"
                            Else
                                straccountvalue = "'" + dtt.Rows(i)("Value").ToString + "'"
                            End If

                        End If
                    End If
                    If dtt.Rows(i)("Code").ToString = "RECEIPTNO" Or dtt.Rows(i)("Code").ToString = "BPVNO" Or dtt.Rows(i)("Code").ToString = "CPVNO" Or dtt.Rows(i)("Code").ToString = "CONTRANO" Then
                        If strdocumentvalue <> "" Then
                            strdocumentvalue = strdocumentvalue + ",'" + dtt.Rows(i)("Value").ToString + "'"
                        Else
                            strdocumentvalue = "'" + dtt.Rows(i)("Value").ToString + "'"
                        End If
                    End If
                    If dtt.Rows(i)("Code").ToString = "CURRENCY" Then
                        If strcurrvalue <> "" Then
                            strcurrvalue = strcurrvalue + ",'" + dtt.Rows(i)("Value").ToString + "'"
                        Else
                            strcurrvalue = "'" + dtt.Rows(i)("Value").ToString + "'"
                        End If
                    End If
                    If dtt.Rows(i)("Code").ToString = "RECEIVEDFROM" Or dtt.Rows(i)("Code").ToString = "PAIDTO" Then
                        If strrcvfrmvalue <> "" Then
                            strrcvfrmvalue = strrcvfrmvalue + ",'" + dtt.Rows(i)("Value").ToString + "'"
                        Else
                            strrcvfrmvalue = "" + dtt.Rows(i)("Value").ToString + ""
                        End If
                    End If
                    If dtt.Rows(i)("Code").ToString = "NARRATION" Then
                        If strnarrationvalue <> "" Then
                            strnarrationvalue = strnarrationvalue + ",'" + dtt.Rows(i)("Value").ToString + "'"
                        Else
                            strnarrationvalue = "" + dtt.Rows(i)("Value").ToString + ""
                        End If
                    End If
                    If dtt.Rows(i)("Code").ToString = "BANKNAME" Or dtt.Rows(i)("Code").ToString = "CASHA/C" Then
                        Dim dtbank As New DataTable

                        strSqlQry = " select  acctmast.acctcode as acctcode from acctmast  ,receipt_master_new  where  acctmast.acctcode=receipt_master_new.receipt_cashbank_code and  acctname ='" & dtt.Rows(i)("value").ToString & "' and div_code='" & ViewState("divcode") & "'"
                        SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                        myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
                        myDataAdapter.Fill(dtbank)
                        If dtbank.Rows.Count > 0 Then
                            For c As Integer = 0 To dtbank.Rows.Count - 1
                                If strbankvalue <> "" Then
                                    strbankvalue = strbankvalue + ",'" + dtbank.Rows(c)("acctcode").ToString + "'"
                                Else
                                    strbankvalue = "'" + dtbank.Rows(c)("acctcode").ToString + "'"
                                End If
                            Next
                        Else
                            If strbankvalue <> "" Then
                                strbankvalue = strbankvalue + ",'" + dtt.Rows(i)("Value").ToString + "'"
                            Else
                                strbankvalue = "'" + dtt.Rows(i)("Value").ToString + "'"
                            End If
                        End If
                    End If
                    If dtt.Rows(i)("Code").ToString = "STATUS" Then
                        If dtt.Rows(i)("Value").ToString = "POSTED" Then
                            If strStatusvalue <> "" Then
                                strStatusvalue = strStatusvalue + ",'" + "P" + "'"
                            Else
                                strStatusvalue = "'" + "P".ToString + "'"
                            End If
                        Else
                            If strStatusvalue <> "" Then
                                strStatusvalue = strStatusvalue + ",'" + "U" + "'"
                            Else
                                strStatusvalue = "'" + "U".ToString + "'"
                            End If
                        End If

                    End If




                    If dtt.Rows(i)("Code").ToString = "CHEQUENO" Then

                        If strchequevalue <> "" Then
                            strchequevalue = strchequevalue + "," + dtt.Rows(i)("Value").ToString
                        Else
                            strchequevalue = dtt.Rows(i)("Value").ToString
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
            strBindCondition = BuildConditionNew(straccountvalue, strdocumentvalue, strcurrvalue, strchequevalue, strbankvalue, strrcvfrmvalue, strnarrationvalue, strStatusvalue, strTextValue)
            Dim myDS As New DataSet
            Dim strValue As String
            gv_SearchResult.Visible = True
            lblMsg.Visible = False
            If gv_SearchResult.PageIndex < 0 Then

                gv_SearchResult.PageIndex = 0
            End If
            strSqlQry = "SELECT receipt_master_new.tran_id, receipt_master_new.tran_type, " & _
                             " case isnull(receipt_master_new.post_state,'') when 'P' then 'Posted' when 'U' then 'UnPosted' end  as post_state ," & _
                             " receipt_master_new.tran_type AS Expr1,  " & _
                             " convert(varchar(10),receipt_master_new.receipt_date,103)  as receipt_date, " & _
                             "  convert(varchar(10),receipt_master_new.receipt_tran_date,103) as receipt_tran_date, " & _
                             " case when isnull(tran_type,'')='RV' then receipt_master_new.receipt_credit when isnull(tran_type,'')='CPV' then  receipt_master_new.receipt_debit " & _
                             " when isnull(tran_type,'')='BPV' then receipt_master_new.receipt_debit when isnull(tran_type,'')='DEP' then receipt_master_new.receipt_debit else 0 end receipt_credit , " & _
                             " receipt_master_new.receipt_currency_id,   " & _
                             " receipt_master_new.receipt_received_from, receipt_master_new.receipt_cheque_number, receipt_master_new.receipt_cashbank_code, case when isnull(tran_type,'')='RV' then vw_bankname.acctname when isnull(tran_type,'')='CPV' then  acctmast.acctname when isnull(tran_type,'')='BPV' then vw_bankname.acctname  else vw_bankname.acctname end acctname, " & _
                             " customer_bank_master.other_bank_master_des,receipt_master_new.receipt_narration, receipt_master_new.adddate, receipt_master_new.adduser, receipt_master_new.moddate, " & _
                             " receipt_master_new.moduser, receipt_master_new.tran_id + '|' + receipt_master_new.tran_type tranidcomarg " & _
                             " FROM  receipt_master_new LEFT OUTER JOIN  " & _
                             " customer_bank_master ON receipt_master_new.receipt_customer_bank = customer_bank_master.other_bank_master_code  " & _
                             " left join vw_bankname on receipt_master_new.receipt_cashbank_code =vw_bankname.acctcode  and  vw_bankname.div_code=receipt_master_new.receipt_div_id  left outer join acctmast  on acctmast.div_code=receipt_master_new.receipt_div_id and acctmast.acctcode=receipt_master_new.receipt_cashbank_code  where  receipt_master_new.receipt_div_id='" & ViewState("divcode") & "' and  receipt_master_new.tran_type='" & CType(ViewState("ReceiptsSearchRVPVTranType"), String) & "'"


            Dim strorderby As String = Session("RExpression")
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
                lblMsg.Visible = True
                lblMsg.Text = "Records not found, Please redefine search criteria."
            End If

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ReceiptsSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            'clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
            'clsDBConnect.dbConnectionClose(SqlConn)                          'Close connection
        End Try

    End Sub
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

#Region "Private Function BuildCondition() As String"
    Private Function BuildCondition() As String
        strWhereCond = ""
        ''If txtTranId.Text.Trim <> "" Then
        ''    If Trim(strWhereCond) = "" Then
        ''        strWhereCond = " upper(receipt_master_new.tran_id) = '" & Trim(txtTranId.Text.Trim) & "'"
        ''    Else
        ''        strWhereCond = strWhereCond & " AND upper(receipt_master_new.tran_id) = '" & Trim(txtTranId.Text.Trim) & "'"
        ''    End If
        ''End If

        ''If txtFromDate.Text.Trim <> "" And txtTodate.Text.Trim <> "" Then
        ''    If Trim(strWhereCond) = "" Then
        ''        strWhereCond = "( (convert(varchar(10),receipt_master_new.receipt_tran_date,111) between convert(varchar(10), '" & Format(CType(txtFromDate.Text, Date), "yyyy/MM/dd") & "',111) " _
        ''        & "  and  convert(varchar(10), '" & Format(CType(txtTodate.Text, Date), "yyyy/MM/dd") & "',111) ) " _
        ''        & " or (convert(varchar(10),receipt_master_new.receipt_tran_date,111) < convert(varchar(10), '" & Format(CType(txtFromDate.Text, Date), "yyyy/MM/dd") & "',111) " _
        ''        & "  and convert(varchar(10),receipt_master_new.receipt_tran_date,111) > convert(varchar(10), '" & Format(CType(txtTodate.Text, Date), "yyyy/MM/dd") & "',111)) )"
        ''    Else
        ''        strWhereCond = strWhereCond & " and ( (convert(varchar(10),receipt_master_new.receipt_tran_date,111) between convert(varchar(10), '" & Format(CType(txtFromDate.Text, Date), "yyyy/MM/dd") & "',111) " _
        ''        & "  and  convert(varchar(10), '" & Format(CType(txtTodate.Text, Date), "yyyy/MM/dd") & "',111) ) " _
        ''        & " or (convert(varchar(10),receipt_master_new.receipt_tran_date,111) < convert(varchar(10), '" & Format(CType(txtFromDate.Text, Date), "yyyy/MM/dd") & "',111) " _
        ''        & "  and convert(varchar(10),receipt_master_new.receipt_tran_date,111) > convert(varchar(10), '" & Format(CType(txtTodate.Text, Date), "yyyy/MM/dd") & "',111)) )"

        ''    End If
        ''End If


        ''If rbtnadsearch.Checked = True Then
        ''    If ddlType.Value <> "[Select]" Then
        ''        If Trim(strWhereCond) = "" Then
        ''            strWhereCond = " upper(receipt_master_new.receipt_cashbank_type) = '" & Trim(CType(ddlType.Value, String)) & "'"
        ''        Else
        ''            strWhereCond = strWhereCond & " AND upper(receipt_master_new.receipt_cashbank_type) = '" & Trim(CType(ddlType.Value, String)) & "'"
        ''        End If
        ''    End If
        ''    If ddlAccCode.Value <> "[Select]" Then
        ''        If Trim(strWhereCond) = "" Then
        ''            strWhereCond = " upper(receipt_master_new.receipt_cashbank_code) = '" & Trim(CType(ddlAccName.Value, String)) & "'"
        ''        Else
        ''            strWhereCond = strWhereCond & " AND upper(receipt_master_new.receipt_cashbank_code) = '" & Trim(CType(ddlAccName.Value, String)) & "'"
        ''        End If
        ''    End If

        ''    If txtdesc.Text.Trim <> "" Then
        ''        If Trim(strWhereCond) = "" Then
        ''            strWhereCond = " receipt_master_new.receipt_narration like '%" & Trim(txtdesc.Text.Trim) & "%'"
        ''        Else
        ''            strWhereCond = strWhereCond & " AND  receipt_master_new.receipt_narration like '%" & Trim(txtdesc.Text.Trim) & "%'"
        ''        End If
        ''    End If


        ''    If ddlBankCode.Value <> "[Select]" Then
        ''        If Trim(strWhereCond) = "" Then

        ''            strWhereCond = " upper(customer_bank_master.other_bank_master_code ) = '" & Trim(CType(ddlBankName.Value, String)) & "'"
        ''        Else
        ''            strWhereCond = strWhereCond & " AND upper(customer_bank_master.other_bank_master_code ) = '" & Trim(CType(ddlBankName.Value, String)) & "'"
        ''        End If
        ''    End If
        ''    If txtFromRecvAmt.Text.Trim <> "" And txtToRecvAmt.Text.Trim <> "" Then
        ''        If Trim(strWhereCond) = "" Then
        ''            strWhereCond = " (receipt_master_new.receipt_credit between  " & txtFromRecvAmt.Text & "  and  " & txtToRecvAmt.Text & " ) "
        ''        Else
        ''            strWhereCond = strWhereCond & " AND (receipt_master_new.receipt_credit between  " & txtFromRecvAmt.Text & "  and  " & txtToRecvAmt.Text & " ) "
        ''        End If
        ''    End If
        ''    If ddlStatus.Value <> "[Select]" Then
        ''        If Trim(strWhereCond) = "" Then
        ''            strWhereCond = " isnull(receipt_master_new.post_state,'')  = '" & ddlStatus.Value & "'"
        ''        Else
        ''            strWhereCond = strWhereCond & " AND isnull(receipt_master_new.post_state,'')  = '" & ddlStatus.Value & "'"
        ''        End If
        ''    End If
        ''End If
        BuildCondition = strWhereCond

    End Function
#End Region


    Protected Sub Page_Error(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Error

    End Sub

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        'Try

        '    Dim typ As Type
        '    typ = GetType(DropDownList)


        'Catch ex As Exception
        '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        '    objUtils.WritErrorLog("ReceiptsSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        'End Try

        '' btnClear.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to clear search criteria?')==false)return false;")
        ''   ClientScript.GetPostBackEventReference(Me, String.Empty)


    End Sub
    Protected Sub btnvsprocess_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnvsprocess.Click

        Try
            FilterGrid()
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("CitiesSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try



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

                Case "CPVNO"
                    lsProcessCity = lsMainArr(i).Split(":")(1)
                    sbAddToDataTable("CPVNO", lsProcessCity, "CB")

                Case "CASHA/C"
                    lsProcessCity = lsMainArr(i).Split(":")(1)
                    sbAddToDataTable("CASHA/C", lsProcessCity, "CB")
                Case "PAIDTO"
                    lsProcessCity = lsMainArr(i).Split(":")(1)
                    sbAddToDataTable("PAIDTO", lsProcessCity, "CB")
                Case "BPVNO"
                    lsProcessCity = lsMainArr(i).Split(":")(1)
                    sbAddToDataTable("BPVNO", lsProcessCity, "CB")


                Case "CUSTOMERBANK"
                    lsProcessCity = lsMainArr(i).Split(":")(1)
                    sbAddToDataTable("CUSTOMERBANK", lsProcessCity, "CB")
                Case "RECEIPTNO"
                    lsProcessCity = lsMainArr(i).Split(":")(1)
                    sbAddToDataTable("RECEIPTNO", lsProcessCity, "NO")
                Case "CONTRANO"
                    lsProcessCity = lsMainArr(i).Split(":")(1)
                    sbAddToDataTable("CONTRANO", lsProcessCity, "NO")
                Case "BANKNAME"
                    lsProcessCity = lsMainArr(i).Split(":")(1)
                    sbAddToDataTable("BANKNAME", lsProcessCity, "BANKNAME")
                Case "CURRENCY"
                    lsProcessCity = lsMainArr(i).Split(":")(1)
                    sbAddToDataTable("CURRENCY", lsProcessCity, "C")
                Case "STATUS"
                    lsProcessCity = lsMainArr(i).Split(":")(1)
                    sbAddToDataTable("STATUS", lsProcessCity, "STATUS")
                Case "CHEQUENO"
                    lsProcessAll = lsMainArr(i).Split(":")(1)
                    sbAddToDataTable("CHEQUENO", lsProcessAll, "CH")
                Case "RECEIVEDFROM"
                    lsProcessAll = lsMainArr(i).Split(":")(1)
                    sbAddToDataTable("RECEIVEDFROM", lsProcessAll, "Rcv")
                Case "NARRATION"
                    lsProcessAll = lsMainArr(i).Split(":")(1)
                    sbAddToDataTable("NARRATION", lsProcessAll, "N")
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
#Region "Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try


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
            ViewState.Add("ReceiptsSearchRVPVTranType", Request.QueryString("tran_type"))

            Session.Add("ReceiptsSearchRVPVTranType", Request.QueryString("tran_type"))

            Dim appid As String = CType(Request.QueryString("appid"), String)

            Dim AppName As HiddenField = CType(Master.FindControl("hdnAppName"), HiddenField)
            If appid Is Nothing = False Then
                strappid = appid 'AppId.Value
            End If

            '*** Danny 04/04/2018>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>            
            strappname = Session("AppName")
            '*** Danny 04/04/2018<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<

            If appid Is Nothing = False Then
                'If appid = "4" Then
                '    strappname = AppName.Value
                'Else
                '    strappname = AppName.Value
                'End If
                strappname = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select displayname from appmaster where appid='" & appid & "'")
            End If

            '*** Danny 04/04/2018>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
            '  Dim divid As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select division_master_code from division_master where accountsmodulename='" & Session("DAppName") & "'")
            '*** Danny 04/04/2018<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<

            ViewState("Appname") = strappname
            Dim divid As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select division_master_code from division_master where accountsmodulename='" & ViewState("Appname") & "'")

            'Rosalin 06/11/2023 -- receiptsNewChange page
            Dim ReceiptsNewMethod As String = "0"
            ReceiptsNewMethod = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id =5754")
            ViewState("ReceiptsNewMethod") = ReceiptsNewMethod
            ' end

            ViewState.Add("divcode", divid)
            ' hdntrantype.Value = CType(Request.QueryString("tran_type"), String)
            Page.ClientScript.RegisterHiddenField("vTrantype", ViewState("ReceiptsSearchRVPVTranType"))

            txtdivcode.Value = CType(ViewState("divcode"), String)


            If IsPostBack = False Then
                btnPrint_new.Attributes.Add("onclick", "return FormValidation('')")
                RowsPerPageCUS.SelectedValue = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id='2006'")
                If CType(Session("GlobalUserName"), String) = Nothing Or CType(Session("Userpwd"), String) = Nothing Then
                    Response.Redirect("~/Login.aspx", False)
                    Exit Sub
                Else
                    objUser.CheckUserRight(Session("dbconnectionName"), CType(Session("GlobalUserName"), String), CType(Session("Userpwd"), String), _
                                                       CType(strappname, String), "AccountsModule\ReceiptSearchNew.aspx?tran_type=" & CType(ViewState("ReceiptsSearchRVPVTranType"), String) & "&appid=" + appid, btnAddNew, btnPrint_new, _
                                                       btnPrint_new, gv_SearchResult, GridCol.Edit, GridCol.Delete, GridCol.View, 0, GridCol.Copy)
                End If



                If CType(ViewState("ReceiptsSearchRVPVTranType"), String) = "RV" Then
                    lblHeading.Text = "Receipt List"
                    gv_SearchResult.Columns.Item(GridCol.chqprint).Visible = False
                    hdOPMode.Value = "R"
                ElseIf CType(ViewState("ReceiptsSearchRVPVTranType"), String) = "CPV" Then
                    lblHeading.Text = "Cash Payment List"
                    gv_SearchResult.Columns.Item(GridCol.receipt_customerbank).Visible = False
                    gv_SearchResult.Columns.Item(GridCol.receipt_cheque_date).Visible = False
                    gv_SearchResult.Columns.Item(GridCol.receipt_cheque_number).Visible = False
                    hdOPMode.Value = "C"
                ElseIf CType(ViewState("ReceiptsSearchRVPVTranType"), String) = "BPV" Then
                    lblHeading.Text = "Bank Payment List"

                    gv_SearchResult.Columns.Item(GridCol.receipt_customerbank).Visible = False
                    hdOPMode.Value = "B"
                ElseIf CType(ViewState("ReceiptsSearchRVPVTranType"), String) = "CV" Then
                    lblHeading.Text = "Contra Voucher List"
                    gv_SearchResult.Columns.Item(GridCol.chqprint).Visible = False
                    hdOPMode.Value = "V"
                ElseIf CType(ViewState("ReceiptsSearchRVPVTranType"), String) = "DEP" Then
                    lblHeading.Text = "Deposit List"
                    hdOPMode.Value = "R"
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
                '--------end
                ' fillorderby()
                FillGridNew()
                'If rbtnadsearch.Checked = True Then
                '    FillCashBankDetails()
                'End If





            Else
                If (Me.IsPostBack And Me.Request("__EVENTTARGET") = "ReceiptsWindowPostBack") Then
                    ''btnSearch_Click(sender, e)
                    btnResetSelection_Click(sender, e)
                End If
                ' btnClear_Click(sender, e)
            End If
            'ClientScript.GetPostBackEventReference(Me, String.Empty)
            'If (Me.IsPostBack And Me.Request("__EVENTTARGET") = "bankdetailsWindowPostBack") Then
            '    btnResetSelection_Click(sender, e)
            'End If
            '' FillGridNew()
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ReceiptsSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub

#End Region
    Private Sub FillCashBankDetails()
        Dim strSqlQry1, strSqlQry2, strSqlQry3 As String
        strSqlQry1 = ""
        strSqlQry2 = ""
        strSqlQry3 = ""
        'If ddlType.Items(ddlType.SelectedIndex).Text = "Bank" Then
        '    strSqlQry1 = "select acctcode,acctname from acctmast ,bank_master_type where  acctmast.bank_master_type_code = bank_master_type.bank_master_type_code" & _
        '    " and bank_master_type.cashbanktype='B'  and  bankyn='Y' order by acctcode "
        '    strSqlQry2 = "select acctname, acctcode from acctmast ,bank_master_type where  acctmast.bank_master_type_code = bank_master_type.bank_master_type_code" & _
        '    " and bank_master_type.cashbanktype='B'  and  bankyn='Y'  order by acctname"
        '    strSqlQry3 = "select  Currcode,acctcode  from acctmast ,bank_master_type where  acctmast.bank_master_type_code = bank_master_type.bank_master_type_code" & _
        '    " and bank_master_type.cashbanktype='B'  and  bankyn='Y'  order by acctcode"
        'ElseIf ddlType.Items(ddlType.SelectedIndex).Text = "Cash" Then
        '    strSqlQry1 = "select acctcode,acctname from acctmast ,bank_master_type where  acctmast.bank_master_type_code = bank_master_type.bank_master_type_code" & _
        '    " and bank_master_type.cashbanktype='C'  and  bankyn='Y' order by acctcode "
        '    strSqlQry2 = "select acctname, acctcode from acctmast ,bank_master_type where  acctmast.bank_master_type_code = bank_master_type.bank_master_type_code" & _
        '    " and bank_master_type.cashbanktype='C'  and  bankyn='Y'  order by acctname"
        '    strSqlQry3 = "select  Currcode,acctcode  from acctmast ,bank_master_type where  acctmast.bank_master_type_code = bank_master_type.bank_master_type_code" & _
        '    " and bank_master_type.cashbanktype='C'  and  bankyn='Y'  order by acctcode"
        'Else
        '    strSqlQry1 = "select acctcode,acctname from acctmast ,bank_master_type where  acctmast.bank_master_type_code = bank_master_type.bank_master_type_code" & _
        '    " and  bankyn='Y' order by acctcode "
        '    strSqlQry2 = "select acctname, acctcode from acctmast ,bank_master_type where  acctmast.bank_master_type_code = bank_master_type.bank_master_type_code" & _
        '    " and  bankyn='Y'  order by acctname"
        '    strSqlQry3 = "select  Currcode,acctcode  from acctmast ,bank_master_type where  acctmast.bank_master_type_code = bank_master_type.bank_master_type_code" & _
        '    " and    bankyn='Y'  order by acctcode"
        'End If
        If CType(ViewState("ReceiptsSearchRVPVTranType"), String) = "CPV" Then
            strSqlQry1 = "select acctcode,acctname from acctmast ,bank_master_type where  acctmast.bank_master_type_code = bank_master_type.bank_master_type_code" & _
            " and bank_master_type.cashbanktype='B'  and  bankyn='Y' order by acctcode "
            strSqlQry2 = "select acctname, acctcode from acctmast ,bank_master_type where  acctmast.bank_master_type_code = bank_master_type.bank_master_type_code" & _
            " and bank_master_type.cashbanktype='B'  and  bankyn='Y'  order by acctname"
            strSqlQry3 = "select  Currcode,acctcode  from acctmast ,bank_master_type where  acctmast.bank_master_type_code = bank_master_type.bank_master_type_code" & _
            " and bank_master_type.cashbanktype='B'  and  bankyn='Y'  order by acctcode"
        ElseIf CType(ViewState("ReceiptsSearchRVPVTranType"), String) = "BPV" Then
            strSqlQry1 = "select acctcode,acctname from acctmast ,bank_master_type where  acctmast.bank_master_type_code = bank_master_type.bank_master_type_code" & _
            " and bank_master_type.cashbanktype='C'  and  bankyn='Y' order by acctcode "
            strSqlQry2 = "select acctname, acctcode from acctmast ,bank_master_type where  acctmast.bank_master_type_code = bank_master_type.bank_master_type_code" & _
            " and bank_master_type.cashbanktype='C'  and  bankyn='Y'  order by acctname"
            strSqlQry3 = "select  Currcode,acctcode  from acctmast ,bank_master_type where  acctmast.bank_master_type_code = bank_master_type.bank_master_type_code" & _
            " and bank_master_type.cashbanktype='C'  and  bankyn='Y'  order by acctcode"
        ElseIf CType(ViewState("ReceiptsSearchRVPVTranType"), String) = "DEP" Then
            strSqlQry1 = "select acctcode,acctname from acctmast ,bank_master_type where  acctmast.bank_master_type_code = bank_master_type.bank_master_type_code" & _
            " and  bankyn='Y' order by acctcode "
            strSqlQry2 = "select acctname, acctcode from acctmast ,bank_master_type where  acctmast.bank_master_type_code = bank_master_type.bank_master_type_code" & _
            " and  bankyn='Y'  order by acctname"
            strSqlQry3 = "select  Currcode,acctcode  from acctmast ,bank_master_type where  acctmast.bank_master_type_code = bank_master_type.bank_master_type_code" & _
            " and    bankyn='Y'  order by acctcode"
        End If
        If CType(ViewState("ReceiptsSearchRVPVTranType"), String) = "CPV" Or CType(ViewState("ReceiptsSearchRVPVTranType"), String) = "BPV" Or CType(ViewState("ReceiptsSearchRVPVTranType"), String) = "DEP" Then
            ''objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlAccCode, "acctcode", "acctname", strSqlQry1, True, ddlAccCode.Value)
            ''objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlAccName, "acctname", "acctcode", strSqlQry2, True, ddlAccName.Value)
        End If
    End Sub

#Region "Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click"
    ''Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
    ''    Dim frmdate As String = ""
    ''    Dim todate As String = ""

    ''    'If txtFromDate.Text = "" Or txtTodate.Text = "" Then
    ''    '    'Record list will be according to the Changing the year  
    ''    '    If Not (Session("changeyear") Is Nothing) Then
    ''    '        frmdate = CDate(Session("changeyear") + "/01" + "/01")

    ''    '        If Session("changeyear") = Year(Now).ToString Then
    ''    '            todate = CDate(Session("changeyear") + "/" + Month(Now).ToString + "/" + Day(Now).ToString)
    ''    '        Else
    ''    '            todate = CDate(Session("changeyear") + "/" + "12" + "/" + "31")
    ''    '        End If

    ''    '        txtFromDate.Text = Format(CType(frmdate, Date), "dd/MM/yyy")
    ''    '        txtTodate.Text = Format(CType(todate, Date), "dd/MM/yyy")

    ''    '    Else
    ''    '        txtFromDate.Text = Format(objDate.GetSystemDateOnly(Session("dbconnectionName")), "dd/MM/yyyy")
    ''    '        txtTodate.Text = Format(objDate.GetSystemDateOnly(Session("dbconnectionName")), "dd/MM/yyyy")           ' --
    ''    '    End If

    ''    'End If

    ''    ''Record list will be according to the Changing the year  
    ''    'If Session("changeyear") <> Year(CType(txtFromDate.Text, Date)).ToString Then
    ''    '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Cannot list for the Different year Change year '  );", True)
    ''    '    Exit Sub
    ''    'End If

    ''    'If Session("changeyear") <> Year(CType(txtTodate.Text, Date)).ToString Then
    ''    '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Cannot list for the Different year Change year '  );", True)
    ''    '    Exit Sub
    ''    'End If

    ''    FillGridNew()
    ''    FillGrid("tran_id", "DESC")
    ''End Sub
#End Region
    ''#Region "Protected Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClear.Click"
    ''    Protected Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClear.Click
    ''        ''txtTranId.Text = ""
    ''        ''ddlType.Value = "[Select]"
    ''        ''ddlStatus.Value = "[Select]"
    ''        ''ddlAccCode.Value = "[Select]"
    ''        ''ddlAccName.Value = "[Select]"
    ''        ''ddlBankCode.Value = "[Select]"
    ''        ''ddlBankName.Value = "[Select]"
    ''        ' ''    txtFromDate.Text = ""
    ''        ' ''   txtTodate.Text = ""
    ''        ''txtFromRecvAmt.Text = ""
    ''        ''txtToRecvAmt.Text = ""
    ''        ''txtdesc.Text = ""
    ''        ''FillGrid("tran_id", "DESC")
    ''    End Sub
    ''#End Region
#Region "Protected Sub gv_SearchResult_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gv_SearchResult.RowCommand"
    Protected Sub gv_SearchResult_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gv_SearchResult.RowCommand
        Try
            Dim strpop As String = ""
            Dim actionstr As String
            actionstr = ""

            If e.CommandName = "" Then Exit Sub
            If e.CommandName = "Page" Then Exit Sub
            If e.CommandName = "Sort" Then Exit Sub
            Dim lblId As String
            Dim lbltrantype As String

            lblId = e.CommandArgument.ToString.Split("|")(0).ToString
            lbltrantype = e.CommandArgument.ToString.Split("|")(1).ToString

            Dim mindate1 As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select convert(varchar(10),receipt_tran_date,111) from receipt_master_new where tran_id='" + lblId + "'" + " and tran_type='" + lbltrantype + "'")

            Dim sealdate As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select sealdate from  sealing_master")

            If e.CommandName = "EditRow" Then


                If Validateseal(lblId) Then
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
                'Rosalin 06/11/2023
                If ViewState("ReceiptsNewMethod") = "2" Then
                    '  strpop = "window.open('ReceiptsNew.aspx?State=" + CType(actionstr, String) + "&RefCode=" + CType(lblId.Trim, String) + "&divid=" + txtdivcode.Value + "&RVPVTranType=" + CType(ViewState("ReceiptsSearchRVPVTranType"), String) + "','ReceiptsNew');"
                    strpop = "window.open('ReceiptsNewChange.aspx?State=" + CType(actionstr, String) + "&RefCode=" + CType(lblId.Trim, String) + "&divid=" + txtdivcode.Value + "&RVPVTranType=" + CType(ViewState("ReceiptsSearchRVPVTranType"), String) + "','ReceiptsNew');"
                Else
                    strpop = "window.open('ReceiptsNew.aspx?State=" + CType(actionstr, String) + "&RefCode=" + CType(lblId.Trim, String) + "&divid=" + txtdivcode.Value + "&RVPVTranType=" + CType(ViewState("ReceiptsSearchRVPVTranType"), String) + "','ReceiptsNew');"
                End If
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)

            ElseIf e.CommandName = "DeleteRow" Then
                'Session.Add("State", "Delete")
                'Session.Add("RefCode", CType(lblId.Trim, String))
                'Response.Redirect("Receipts.aspx", False)

                If Validateseal(lblId) Then
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
                strpop = "window.open('ReceiptsNew.aspx?State=" + CType(actionstr, String) + "&RefCode=" + CType(lblId.Trim, String) + "&divid=" + txtdivcode.Value + "&RVPVTranType=" + CType(ViewState("ReceiptsSearchRVPVTranType"), String) + "','Receipts');"
                ' strpop = "window.open('ReceiptsNewChange.aspx?State=" + CType(actionstr, String) + "&RefCode=" + CType(lblId.Trim, String) + "&divid=" + txtdivcode.Value + "&RVPVTranType=" + CType(ViewState("ReceiptsSearchRVPVTranType"), String) + "','Receipts');"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)

            ElseIf e.CommandName = "View" Then
                'Session.Add("State", "View")
                'Session.Add("RefCode", CType(lblId.Trim, String))
                'Response.Redirect("Receipts.aspx", False)
                actionstr = "View"

                'Rosalin 06/11/2023
                If ViewState("ReceiptsNewMethod") = "2" Then
                    strpop = "window.open('ReceiptsNewChange.aspx?State=" + CType(actionstr, String) + "&RefCode=" + CType(lblId.Trim, String) + "&divid=" + txtdivcode.Value + "&RVPVTranType=" + CType(ViewState("ReceiptsSearchRVPVTranType"), String) + "','Receipts');"
                Else
                    strpop = "window.open('ReceiptsNew.aspx?State=" + CType(actionstr, String) + "&RefCode=" + CType(lblId.Trim, String) + "&divid=" + txtdivcode.Value + "&RVPVTranType=" + CType(ViewState("ReceiptsSearchRVPVTranType"), String) + "','Receipts');"
                End If
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)

                ElseIf e.CommandName = "Copy" Then
                    'Session.Add("State", "Copy")
                    'Session.Add("RefCode", CType(lblId.Trim, String))
                    'Response.Redirect("Receipts.aspx", False)
                    actionstr = "Copy"
                strpop = "window.open('ReceiptsNew.aspx?State=" + CType(actionstr, String) + "&RefCode=" + CType(lblId.Trim, String) + "&divid=" + txtdivcode.Value + "&RVPVTranType=" + CType(ViewState("ReceiptsSearchRVPVTranType"), String) + "','Receipts');"
                'strpop = "window.open('ReceiptsNewChange.aspx?State=" + CType(actionstr, String) + "&RefCode=" + CType(lblId.Trim, String) + "&divid=" + txtdivcode.Value + "&RVPVTranType=" + CType(ViewState("ReceiptsSearchRVPVTranType"), String) + "','Receipts');"
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
                ElseIf e.CommandName = "ViewLog" Then

                    actionstr = "ViewLog"
                    strpop = "window.open('ViewLog.aspx?State=" + CType(actionstr, String) + "&divid=" + txtdivcode.Value + "&RefCode=" + CType(lblId.Trim, String) + "&trantype=" + CType(ViewState("ReceiptsSearchRVPVTranType"), String) + "','Journal');"
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
 

                ElseIf e.CommandName = "lnkChkPrint" And CType(ViewState("ReceiptsSearchRVPVTranType"), String) = "BPV" Or CType(ViewState("ReceiptsSearchRVPVTranType"), String) = "RV" _
                     Or CType(ViewState("ReceiptsSearchRVPVTranType"), String) = "CV" Then

                    actionstr = "print"
                    strpop = "window.open('chequeprint.aspx?type=" + CType(lbltrantype.Trim, String) + "&divid=" + txtdivcode.Value + "&tranid=" + CType(lblId.Trim, String) + "','Journal');"
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)

            End If

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ReceiptsSearchNew.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#End Region
#Region "Protected Sub gv_SearchResult_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gv_SearchResult.PageIndexChanging"
    Protected Sub gv_SearchResult_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gv_SearchResult.PageIndexChanging
        gv_SearchResult.PageIndex = e.NewPageIndex
        FillGrid("tran_id", "DESC")
    End Sub
#End Region
#Region "Protected Sub gv_SearchResult_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gv_SearchResult.Sorting"
    Protected Sub gv_SearchResult_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gv_SearchResult.Sorting
        Session.Add("RExpression", e.SortExpression)
        SortGridColoumn_click()
    End Sub
#End Region
#Region "Public Sub SortGridColoumn_click()"
    Public Sub SortGridColoumn_click()
        Dim DataTable As DataTable
        Dim myDS As New DataSet
        FillGrid(Session("RExpression"), "")

        myDS = gv_SearchResult.DataSource
        DataTable = myDS.Tables(0)
        If IsDBNull(DataTable) = False Then
            Dim dataView As DataView = DataTable.DefaultView
            Session.Add("Rdirection", objUtils.SwapSortDirection(Session("Rdirection")))
            dataView.Sort = Session("RExpression") & " " & objUtils.ConvertSortDirectionToSql(Session("Rdirection"))
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

    '                'strSqlQry = "SELECT  receipt_master_new.tran_id as [Tran ID], receipt_master_new.tran_type as [Tran Type] , " & _
    '                '      "  convert(varchar(10),receipt_master_new.receipt_date,103) as [Receipt Date], " & _
    '                '     " convert(varchar(10),receipt_master_new.receipt_tran_date,103) as [Receipt Tran Date],  " & _
    '                '     " receipt_master_new.receipt_credit as [Receipt Credit] , receipt_master_new.receipt_currency_id as [Currency],   " & _
    '                '     " receipt_master_new.receipt_received_from as [Receipt Received From], receipt_master_new.receipt_cheque_number as [Receipt Cheque Number], " & _
    '                '     " receipt_master_new.receipt_cashbank_code as [Receipt Cash Bank Code], " & _
    '                '     " customer_bank_master.other_bank_master_des as [Other Bank Master Des ], receipt_master_new.adddate as [Date Created], receipt_master_new.adduser as[User Created] ," & _
    '                '     "  receipt_master_new.moddate as [Date Modified], receipt_master_new.moduser  as [User Modified]" & _
    '                '     " FROM  receipt_master_new LEFT OUTER JOIN  " & _
    '                '     " customer_bank_master ON receipt_master_new.receipt_customer_bank = customer_bank_master.other_bank_master_code  " & _
    '                '     " where receipt_master_new.tran_type='" & CType(ViewState("ReceiptsSearchRVPVTranType"), String) & "'"

    '                If CType(ViewState("ReceiptsSearchRVPVTranType"), String) = "RV" Then
    '                    strSqlQry = "SELECT  receipt_master_new.tran_id as [ReceiptNo.], receipt_master_new.tran_type as [Tran Type] , convert(varchar(10),receipt_master_new.receipt_date,103) as [Receipt Date],  " & _
    '                      " case isnull(receipt_master_new.post_state,'') when 'P' then 'Posted' when 'U' then 'UnPosted' end  as Status, " & _
    '                     " receipt_master_new.receipt_currency_id as [Currency], receipt_master_new.receipt_received_from as [Received From],  " & _
    '                     "  vw_bankname.acctname as [Bank Name],receipt_master_new.receipt_cheque_number as [Cheque Number],   " & _
    '                    " convert(varchar(10), receipt_master_new.receipt_tran_date,103) as [Cheque Date], " & _
    '                            " customer_bank_master.other_bank_master_des as [Customer Bank ],receipt_master_new.receipt_credit as [Amount Received] , receipt_master_new.adddate as [Date Created], receipt_master_new.adduser as[User Created] ," & _
    '                     "  receipt_master_new.moddate as [Date Modified], receipt_master_new.moduser  as [User Modified]" & _
    '                     " FROM  receipt_master_new LEFT OUTER JOIN  " & _
    '                     " customer_bank_master ON receipt_master_new.receipt_customer_bank = customer_bank_master.other_bank_master_code  " & _
    '                     "  join vw_bankname on receipt_master_new.receipt_cashbank_code =vw_bankname.acctcode  where vw_bankname.div_code='" & ViewState("divcode") & "'  and receipt_master_new.tran_type='" & CType(ViewState("ReceiptsSearchRVPVTranType"), String) & "'"
    '                ElseIf CType(ViewState("ReceiptsSearchRVPVTranType"), String) = "CPV" Then
    '                    strSqlQry = "SELECT  receipt_master_new.tran_id as [Tran ID], receipt_master_new.tran_type as [Tran Type] ,case isnull(receipt_master_new.post_state,'') when 'P' then 'Posted' when 'U' then 'UnPosted' end  as Status , " & _
    '                      "  convert(varchar(10),receipt_master_new.receipt_date,103) as [Cash Payment Date], " & _
    '                     " convert(varchar(10),receipt_master_new.receipt_tran_date,103) as [Tran Date],  " & _
    '                     " receipt_master_new.receipt_debit as [Amount Paid] , receipt_master_new.receipt_currency_id as [Currency],   " & _
    '                     " receipt_master_new.receipt_received_from as [Paid To], receipt_master_new.receipt_cheque_number as [Cheque Number], " & _
    '                     " receipt_master_new.receipt_cashbank_code as [Cash Bank Code], " & _
    '                     " customer_bank_master.other_bank_master_des as [Other Bank Master Des ], receipt_master_new.adddate as [Date Created], receipt_master_new.adduser as[User Created] ," & _
    '                     "  receipt_master_new.moddate as [Date Modified], receipt_master_new.moduser  as [User Modified]" & _
    '                     " FROM  receipt_master_new LEFT OUTER JOIN  " & _
    '                     " customer_bank_master ON receipt_master_new.receipt_customer_bank = customer_bank_master.other_bank_master_code  " & _
    '                     " where receipt_master_new.tran_type='" & CType(ViewState("ReceiptsSearchRVPVTranType"), String) & "'"
    '                ElseIf CType(ViewState("ReceiptsSearchRVPVTranType"), String) = "BPV" Then
    '                    strSqlQry = "SELECT  receipt_master_new.tran_id as [Tran ID], receipt_master_new.tran_type as [Tran Type] ,case isnull(receipt_master_new.post_state,'') when 'P' then 'Posted' when 'U' then 'UnPosted' end  as Status , " & _
    '                      "  convert(varchar(10),receipt_master_new.receipt_date,103) as [Bank Payment Date], " & _
    '                     " convert(varchar(10),receipt_master_new.receipt_tran_date,103) as [Tran Date],  " & _
    '                     " receipt_master_new.receipt_debit as [Amount Paid] , receipt_master_new.receipt_currency_id as [Currency],   " & _
    '                     " receipt_master_new.receipt_received_from as [Paid To], receipt_master_new.receipt_cheque_number as [Cheque Number], " & _
    '                     " receipt_master_new.receipt_cashbank_code as [Cash Bank Code], " & _
    '                     " customer_bank_master.other_bank_master_des as [Other Bank Master Des ], receipt_master_new.adddate as [Date Created], receipt_master_new.adduser as[User Created] ," & _
    '                     "  receipt_master_new.moddate as [Date Modified], receipt_master_new.moduser  as [User Modified]" & _
    '                     " FROM  receipt_master_new LEFT OUTER JOIN  " & _
    '                     " customer_bank_master ON receipt_master_new.receipt_customer_bank = customer_bank_master.other_bank_master_code  " & _
    '                     " where receipt_master_new.tran_type='" & CType(ViewState("ReceiptsSearchRVPVTranType"), String) & "'"
    '                ElseIf CType(ViewState("ReceiptsSearchRVPVTranType"), String) = "DEP" Then
    '                    strSqlQry = "SELECT  receipt_master_new.tran_id as [Tran ID], receipt_master_new.tran_type as [Tran Type] ,case isnull(receipt_master_new.post_state,'') when 'P' then 'Posted' when 'U' then 'UnPosted' end  as Status , " & _
    '                      "  convert(varchar(10),receipt_master_new.receipt_date,103) as [Deposit Date], " & _
    '                     " convert(varchar(10),receipt_master_new.receipt_tran_date,103) as [Tran Date],  " & _
    '                     " receipt_master_new.receipt_debit as [Amount Deposit] , receipt_master_new.receipt_currency_id as [Currency],   " & _
    '                     " receipt_master_new.receipt_received_from as [Deposit To], receipt_master_new.receipt_cheque_number as [Cheque Number], " & _
    '                     " receipt_master_new.receipt_cashbank_code as [Cash Bank Code], " & _
    '                     " customer_bank_master.other_bank_master_des as [Other Bank Master Des ], receipt_master_new.adddate as [Date Created], receipt_master_new.adduser as[User Created] ," & _
    '                     "  receipt_master_new.moddate as [Date Modified], receipt_master_new.moduser  as [User Modified]" & _
    '                     " FROM  receipt_master_new LEFT OUTER JOIN  " & _
    '                     " customer_bank_master ON receipt_master_new.receipt_customer_bank = customer_bank_master.other_bank_master_code  " & _
    '                     " where receipt_master_new.tran_type='" & CType(ViewState("ReceiptsSearchRVPVTranType"), String) & "'"
    '                End If


    '                If Trim(BuildCondition) <> "" Then
    '                    strSqlQry = strSqlQry & " and  " & BuildCondition() & " ORDER BY tran_id "
    '                Else
    '                    strSqlQry = strSqlQry & " ORDER BY tran_id "
    '                End If

    '                con = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))

    '                DA = New SqlDataAdapter(strSqlQry, con)
    '                DA.Fill(DS, "cancellation")

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
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=ReceiptsSearch','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub
    Protected Function validaterpt() As Boolean
        ''If txtFromDate.Text = "" Or txtTodate.Text = "" Then

        ''    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Fill the From and To date');", True)

        ''    Return False
        ''End If
        ''Return True


    End Function

    Public Function Validateseal(ByVal tranid As String) As Boolean
        Try
            Dim tranType As String = ViewState("ReceiptsSearchRVPVTranType")
            Dim ds As DataSet = objUtils.ExecuteQuerySqlnew(Session("dbconnectionName"), "select * from  receipt_master_new where tran_id='" + tranid + "' and tran_type='" + tranType + "'")  'add tran type by param on 01/07/2021
            'Dim ds As DataSet = objUtils.ExecuteQuerySqlnew(Session("dbconnectionName"), "select * from  receipt_master_new where tran_id='" + tranid + "' ")
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


    'Protected Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrint.Click
    '    Dim Pageame As String = ""
    '    'If validaterpt() Then
    '    If CType(ViewState("ReceiptsSearchRVPVTranType"), String) = "RV" Then
    '        ViewState.Add("Pageame", "RecieptsReport")
    '        Pageame = "ReceiptsReport"
    '    ElseIf CType(ViewState("ReceiptsSearchRVPVTranType"), String) = "PV" Then
    '        ViewState.Add("Pageame", "PaymentsReport")
    '        Pageame = "PaymentsReport"
    '    End If

    '    ViewState.Add("Pageame", "GLtrialbalReport")
    '    ViewState.Add("BackPageName", "rptRV_PVreport.aspx")
    '    Try
    '        Dim strfromdate, strtodate, strtrantype As String


    '        Dim strbanktype As String = ""
    '        Dim strbank As String = ""
    '        Dim poststate As String = ""
    '        Dim strtranid As String = ""
    '        Dim strbankcode As String = ""
    '        Dim strreporttype As String = ""
    '        Dim strvoucher As String = ""
    '        Dim strfromamt As String = ""
    '        Dim strtoamt As String = ""
    '        Dim divcode As String = ViewState("divcode")

    '        If dtt Is Nothing Then


    '            strbanktype = ""
    '            poststate = ""
    '            strtranid = ""
    '            strbank = ""

    '        ElseIf dtt.Rows.Count > 0 Then

    '            For i As Integer = 0 To dtt.Rows.Count - 1

    '                If dtt.Rows(i)("Code").ToString = "TRANSFER" Then



    '                    If strbanktype <> "" Then
    '                        strbanktype = strbanktype + "," + dtt.Rows(i)("Value").ToString

    '                    Else
    '                        strbanktype = dtt.Rows(i)("value").ToString

    '                    End If
    '                End If
    '                If dtt.Rows(i)("Code").ToString = "STATUS" Then


    '                    If dtt.Rows(i)("Value").ToString = "POSTED" Then
    '                        If poststate <> "" Then
    '                            poststate = poststate + "," + "P"
    '                        Else
    '                            poststate = "P"
    '                        End If
    '                    Else
    '                        If poststate <> "" Then
    '                            poststate = poststate + "," + "U"
    '                        Else
    '                            poststate = "U"
    '                        End If

    '                    End If

    '                End If
    '                If dtt.Rows(i)("Code").ToString = "RECEIPTNO" Then
    '                    If strtranid <> "" Then
    '                        strtranid = strtranid + ",'" + dtt.Rows(i)("Value").ToString + "'"
    '                    Else
    '                        strtranid = dtt.Rows(i)("value").ToString

    '                    End If
    '                End If

    '                If dtt.Rows(i)("Code").ToString = "BANKNAME" Then
    '                    Dim dtbank2 As New DataTable
    '                    strSqlQry = " select  acctmast.acctcode as acctcode from acctmast  ,receipt_master_new  where  acctmast.acctcode=receipt_master_new.receipt_cashbank_code and  acctname ='" & dtt.Rows(i)("value").ToString & "'"
    '                    SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
    '                    myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
    '                    myDataAdapter.Fill(dtbank2)
    '                    If dtbank2.Rows.Count > 0 Then
    '                        For c As Integer = 0 To dtbank2.Rows.Count - 1
    '                            If strbank <> "" Then
    '                                strbank = strbank + "," + dtbank2.Rows(c)("acctcode").ToString + ""
    '                            Else
    '                                strbank = "'" + dtbank2.Rows(c)("acctcode").ToString + "'"
    '                            End If
    '                        Next
    '                    Else
    '                        strbank = ""
    '                    End If
    '                End If



    '            Next
    '        Else

    '            strbanktype = ""
    '            poststate = ""
    '            strtranid = ""
    '            strbank = ""

    '        End If
    '        If txtFromDate.Text = "" Then
    '            strfromdate = "2017/01/01"
    '        Else
    '            strfromdate = txtFromDate.Text
    '        End If
    '        If txtToDate.Text = "" Then
    '            strtodate = "2020/12/31"
    '        Else
    '            strtodate = txtToDate.Text
    '        End If


    '        strtrantype = Session("ReceiptsSearchRVPVTranType")
    '        'strclosing = ddlclosing.SelectedIndex.ToString

    '        ''strfromdate = Mid(Format(CType(txtFromDate.Text, Date), "yyyy/MM/dd"), 1, 10)
    '        ''strtodate = Mid(Format(CType(txtTodate.Text, Date), "yyyy/MM/dd"), 1, 10)
    '        ''strreporttype = ddlrpttype.SelectedIndex()
    '        ''strtrantype = CType(ViewState("ReceiptsSearchRVPVTranType"), String)
    '        ''strtranid = txtTranId.Text
    '        ''strbanktype = IIf(ddlType.Value = "[Select]", "", ddlType.Value)
    '        ''strbankcode = IIf(ddlAccCode.Items(ddlAccCode.SelectedIndex).Text = "[Select]", "", ddlAccCode.Items(ddlAccCode.SelectedIndex).Text)
    '        ''strbank = IIf(ddlBankCode.Items(ddlBankCode.SelectedIndex).Text = "[Select]", "", ddlBankCode.Items(ddlBankCode.SelectedIndex).Text)
    '        ''poststate = IIf(ddlStatus.Value = "[Select]", "", ddlStatus.Value)
    '        strvoucher = "All"
    '        'Response.Redirect("rptRV_PVreport.aspx?frmdate=" & strfromdate & "&todate=" & strtodate & "&trantype=" & strtrantype & "&tranid=" & strtranid & "&C_Btype=" & strbanktype & "&accfrm=" & strbankcode & "&bank=" & strbank & "&type=" & strreporttype, False)
    '        Dim strpop As String = ""

    '        strpop = "window.open('rptRV_PVreport.aspx?BackPageName=rptRV_PVreport.aspx&Pageame=" & Pageame & "&frmdate=" & strfromdate & "&todate=" & strtodate & "&trantype=" & strtrantype & "&tranid=" & strtranid & "&C_Btype=" & strbanktype & "&accfrm=" & strbankcode & "&bank=" & strbank & "&divid=" & divcode & "&type=" & strreporttype & "&poststate=" & poststate & "&voucher=" & strvoucher & "&fromamt=" & strfromamt & "&toamt=" & strtoamt & "' ,'RepRVPV');"
    '        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)

    '    Catch ex As Exception
    '        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
    '        objUtils.WritErrorLog("rptRV_PVreport.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))

    '    End Try

    '    'End If

    'End Sub

    Protected Sub gv_SearchResult_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gv_SearchResult.RowDataBound

        If e.Row.RowIndex = -1 Then
            If e.Row.RowType = DataControlRowType.Header Then
                If CType(ViewState("ReceiptsSearchRVPVTranType"), String) = "RV" Then
                    ' e.Row.Cells(GridCol.receipt_date).Text = "Receipt Date"
                    'e.Row.Cells(GridCol.receipt_credit).Text = "Amount Received"
                    'e.Row.Cells(GridCol.receipt_received_from).Text = "Received From"
                ElseIf CType(ViewState("ReceiptsSearchRVPVTranType"), String) = "CV" Then
                    e.Row.Cells(GridCol.tran_id).Text = "Contra No"
                    e.Row.Cells(GridCol.receipt_date).Text = "Contra Date"
                ElseIf CType(ViewState("ReceiptsSearchRVPVTranType"), String) = "CPV" Then
                    e.Row.Cells(GridCol.tran_id).Text = "Cash Payment No"
                    e.Row.Cells(GridCol.receipt_date).Text = "Cash Payment Date"
                    e.Row.Cells(GridCol.receipt_received_from).Text = "Paid To"
                    'e.Row.Cells(GridCol.receipt_received_from).Text = "Paid To"
                    e.Row.Cells(GridCol.other_bank_master_des).Text = "Cash A/c"
                    e.Row.Cells(GridCol.receipt_credit).Text = "Amount Paid"

                ElseIf CType(ViewState("ReceiptsSearchRVPVTranType"), String) = "BPV" Then
                    e.Row.Cells(GridCol.tran_id).Text = "Bank Payment No"
                    e.Row.Cells(GridCol.receipt_date).Text = "Bank Payment Date"
                    e.Row.Cells(GridCol.receipt_received_from).Text = "Paid To"
                    'e.Row.Cells(GridCol.receipt_received_from).Text = "Paid To"

                    e.Row.Cells(GridCol.receipt_credit).Text = "Amount Paid"

                ElseIf CType(ViewState("ReceiptsSearchRVPVTranType"), String) = "DEP" Then
                    e.Row.Cells(GridCol.receipt_date).Text = "Deposit Date"
                    e.Row.Cells(GridCol.receipt_credit).Text = "Amount Deposit"
                    e.Row.Cells(GridCol.receipt_received_from).Text = "Deposit To"

        
                    'Tanvir 17012024
                End If
                Exit Sub
            End If
        End If
        'Tanvir 17012024
        'If CType(ViewState("ReceiptsSearchRVPVTranType"), String) = "BPV" Then

        '    gv_SearchResult.Columns(17).Visible = False
        'ElseIf CType(ViewState("ReceiptsSearchRVPVTranType"), String) = "RV" Then

        '    gv_SearchResult.Columns(17).Visible = True
        'End If

        'Tanvir 17012024
        If e.Row.RowType = DataControlRowType.DataRow Then

            Dim lblcustbank As Label = e.Row.FindControl("lblcustbank")
            Dim lblbankname As Label = e.Row.FindControl("lblbankname")
            Dim lblDocNo As Label = e.Row.FindControl("lblDocNo")
            Dim lblStatus As Label = e.Row.FindControl("lblStatus")
            Dim lblCurrency As Label = e.Row.FindControl("lblCurrency")
            Dim lblRecievedFrom As Label = e.Row.FindControl("lblRecievedFrom")
            Dim lblChequeNo As Label = e.Row.FindControl("lblChequeNo")
            'Dim lblAmtRcv As Label = e.Row.FindControl("lblAmtRcv")
            Dim lbEditDate As LinkButton = e.Row.FindControl("lbEditDate") 'Tanvir 17012023

            Dim lsChequeNo As String = ""
            Dim lsBankName As String = ""
            Dim lsRecievedFrom As String = ""
            Dim lsStatus As String = ""
            Dim lscustbank As String = ""
            Dim lsDocNo As String = ""
            Dim lsCurrency As String = ""
            Dim lstextname As String = ""
           

            Dim dtDynamics As New DataTable
            dtDynamics = Session("sDtDynamic")
            If Session("sDtDynamic") IsNot Nothing Then
                If dtDynamics.Rows.Count > 0 Then
                    Dim j As Integer
                    For j = 0 To dtDynamics.Rows.Count - 1
                        lsBankName = ""
                        If "CHEQUENO" = dtDynamics.Rows(j)("code").ToString.Trim.ToUpper Then
                            lsChequeNo = dtDynamics.Rows(j)("value").ToString.Trim.ToUpper
                        End If
                        If "RECEIVEDFROM" = dtDynamics.Rows(j)("code").ToString.Trim.ToUpper Then
                            lsRecievedFrom = dtDynamics.Rows(j)("value").ToString.Trim.ToUpper
                        End If
                        If "CURRENCY" = dtDynamics.Rows(j)("code").ToString.Trim.ToUpper Then
                            lsCurrency = dtDynamics.Rows(j)("value").ToString.Trim.ToUpper
                        End If
                        If "STATUS" = dtDynamics.Rows(j)("code").ToString.Trim.ToUpper Then
                            lsStatus = dtDynamics.Rows(j)("value").ToString.Trim.ToUpper
                        End If
                        If "CUSTOMERBANK" = dtDynamics.Rows(j)("code").ToString.Trim.ToUpper Then
                            lscustbank = dtDynamics.Rows(j)("value").ToString.Trim.ToUpper
                        End If
                        If "RECEIPTNO" = dtDynamics.Rows(j)("code").ToString.Trim.ToUpper Then
                            lsDocNo = dtDynamics.Rows(j)("value").ToString.Trim.ToUpper
                        End If
                        If "BANKNAME" = dtDynamics.Rows(j)("code").ToString.Trim.ToUpper Then
                            lsBankName = dtDynamics.Rows(j)("value").ToString.Trim.ToUpper
                        End If

                        If "TEXT" = dtDynamics.Rows(j)("code").ToString.Trim.ToUpper Then
                            lstextname = dtDynamics.Rows(j)("value").ToString.Trim.ToUpper
                        End If

                        If lsBankName.Trim <> "" Then
                            lblbankname.Text = Regex.Replace(lblbankname.Text.Trim, lsBankName.Trim(), _
                                Function(match As Match) String.Format("<span style = 'background-color:#ffcc99'>{0}</span>", match.Value), _
                                            RegexOptions.IgnoreCase)
                        End If
                        If lsDocNo.Trim <> "" Then
                            lblDocNo.Text = Regex.Replace(lblDocNo.Text.Trim, lsDocNo.Trim(), _
                                Function(match As Match) String.Format("<span style = 'background-color:#ffcc99'>{0}</span>", match.Value), _
                                            RegexOptions.IgnoreCase)
                        End If
                        If lscustbank.Trim <> "" Then
                            lblcustbank.Text = Regex.Replace(lblcustbank.Text.Trim, lscustbank.Trim(), _
                                Function(match As Match) String.Format("<span style = 'background-color:#ffcc99'>{0}</span>", match.Value), _
                                            RegexOptions.IgnoreCase)
                        End If
                        If lsStatus.Trim <> "" Then
                            lblStatus.Text = Regex.Replace(lblStatus.Text.Trim, lsStatus.Trim(), _
                                Function(match As Match) String.Format("<span style = 'background-color:#ffcc99'>{0}</span>", match.Value), _
                                            RegexOptions.IgnoreCase)
                        End If
                        If lsCurrency.Trim <> "" Then
                            lblCurrency.Text = Regex.Replace(lblCurrency.Text.Trim, lsCurrency.Trim(), _
                                Function(match As Match) String.Format("<span style = 'background-color:#ffcc99'>{0}</span>", match.Value), _
                                            RegexOptions.IgnoreCase)
                        End If
                        If lsRecievedFrom.Trim <> "" Then
                            lblRecievedFrom.Text = Regex.Replace(lblRecievedFrom.Text.Trim, lsRecievedFrom.Trim(), _
                                Function(match As Match) String.Format("<span style = 'background-color:#ffcc99'>{0}</span>", match.Value), _
                                            RegexOptions.IgnoreCase)
                        End If
                        If lsRecievedFrom.Trim <> "" Then
                            lblRecievedFrom.Text = Regex.Replace(lblRecievedFrom.Text.Trim, lsRecievedFrom.Trim(), _
                                Function(match As Match) String.Format("<span style = 'background-color:#ffcc99'>{0}</span>", match.Value), _
                                            RegexOptions.IgnoreCase)
                        End If
                        If lsChequeNo.Trim <> "" Then
                            lblChequeNo.Text = Regex.Replace(lblChequeNo.Text.Trim, lsChequeNo.Trim(), _
                                Function(match As Match) String.Format("<span style = 'background-color:#ffcc99'>{0}</span>", match.Value), _
                                            RegexOptions.IgnoreCase)
                        End If
                        If lstextname.Trim <> "" Then
                            lblbankname.Text = Regex.Replace(lblbankname.Text.Trim, lstextname.Trim(), _
                                            Function(match As Match) String.Format("<span style = 'background-color:#ffcc99'>{0}</span>", match.Value), _
                                                        RegexOptions.IgnoreCase)
                            lblDocNo.Text = Regex.Replace(lblDocNo.Text.Trim, lstextname.Trim(), _
            Function(match As Match) String.Format("<span style = 'background-color:#ffcc99'>{0}</span>", match.Value), _
                        RegexOptions.IgnoreCase)
                            lblcustbank.Text = Regex.Replace(lblcustbank.Text.Trim, lstextname.Trim(), _
                             Function(match As Match) String.Format("<span style = 'background-color:#ffcc99'>{0}</span>", match.Value), _
                                         RegexOptions.IgnoreCase)
                            lblStatus.Text = Regex.Replace(lblStatus.Text.Trim, lstextname.Trim(), _
                       Function(match As Match) String.Format("<span style = 'background-color:#ffcc99'>{0}</span>", match.Value), _
                                   RegexOptions.IgnoreCase)
                            lblRecievedFrom.Text = Regex.Replace(lblRecievedFrom.Text.Trim, lstextname.Trim(), _
                       Function(match As Match) String.Format("<span style = 'background-color:#ffcc99'>{0}</span>", match.Value), _
                                   RegexOptions.IgnoreCase)
                            lblChequeNo.Text = Regex.Replace(lblChequeNo.Text.Trim, lstextname.Trim(), _
         Function(match As Match) String.Format("<span style = 'background-color:#ffcc99'>{0}</span>", match.Value), _
                     RegexOptions.IgnoreCase)
                            lblCurrency.Text = Regex.Replace(lblCurrency.Text.Trim, lstextname.Trim(), _
        Function(match As Match) String.Format("<span style = 'background-color:#ffcc99'>{0}</span>", match.Value), _
                    RegexOptions.IgnoreCase)
                            '                    lblAmtRcv.Text = Regex.Replace(lblCurrency.Text.Trim, lstextname.Trim(), _
                            'Function(match As Match) String.Format("<span style = 'background-color:#ffcc99'>{0}</span>", match.Value), _
                            '            RegexOptions.IgnoreCase)

                        End If

                    Next
                End If
            End If



        End If
        'If CType(ViewState("ReceiptsSearchRVPVTranType"), String) = "RV" Then
        '    e.Row.Cells(GridCol.chqprint).Width = "0"
        'End If



    End Sub

    Protected Sub TemplateFieldBind(ByVal sender As Object, ByVal e As EventArgs)

        Try
            Select Case sender.ID

                Case "lblRecievedFrom"
                    sender.Text = Eval("receipt_received_from")

                Case "lnkChkPrint"

                    If CType(ViewState("ReceiptsSearchRVPVTranType"), String) = "BPV" Then
                        sender.Visible = True
                    Else
                        sender.Visible = False
                    End If
                    'Tanvir 17012024
                Case "lbEditDate"
                    If CType(ViewState("ReceiptsSearchRVPVTranType"), String) = "BPV" Then
                        sender.Visible = False
                    ElseIf CType(ViewState("ReceiptsSearchRVPVTranType"), String) = "RV" Then
                        sender.Visible = True
                    End If
                    'Tanvir 17012024
                    'If CType(ViewState("ReceiptsSearchRVPVTranType"), String) = "BPV" Then
                    '    For Each row As GridViewRow In gv_SearchResult.Rows
                    '        row.Cells(20).Visible = True
                    '    Next
                    'Else
                    '    For Each row As GridViewRow In gv_SearchResult.Rows
                    '        row.Cells(20).Visible = False
                    '    Next
                    'End If

            End Select

        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try


    End Sub

    Protected Sub btnChecking_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Session.Add("State", "New")
        Response.Redirect("Receipts.aspx", False)
        Dim strpop As String = ""
        Dim actionstr As String = ""
        actionstr = "New"
        strpop = "window.open('Receipts_old.aspx?State=" + CType(actionstr, String) + "&RefCode=" + CType("", String) + "&RVPVTranType=" + CType(ViewState("ReceiptsSearchRVPVTranType"), String) + "','ReceiptsNew','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
        'ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", "jsfunction('" & btnChecking.ClientID & "','true')", True)

    End Sub

    Protected Sub btnClearDate_Click(sender As Object, e As System.EventArgs) Handles btnClearDate.Click
        txtFromDate.Text = ""
        txtToDate.Text = ""
        FillGridNew()
    End Sub


    Protected Sub btnFilter_Click(sender As Object, e As System.EventArgs) Handles btnFilter.Click
        Try
            FillGridNew()
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ReceiptSearchNew.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub

    Protected Sub btnResetSelection_Click(sender As Object, e As System.EventArgs) Handles btnResetSelection.Click
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

   
    Protected Sub gv_SearchResult_RowUpdated(sender As Object, e As System.Web.UI.WebControls.GridViewUpdatedEventArgs) Handles gv_SearchResult.RowUpdated

    End Sub

    Protected Sub RowsPerPageCUS_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RowsPerPageCUS.SelectedIndexChanged
        FillGridNew()

    End Sub

    Private Function FillGridNew_report() As String

      
        dtt = Session("sDtDynamic")
        Dim strStatusvalue As String = ""
        Dim strbankvalue As String = ""
        Dim strTextValue As String = ""
        Dim strBindCondition As String = ""
        Dim straccountvalue As String = ""
        Dim strdocumentvalue As String = ""
        Dim strcurrvalue As String = ""
        Dim strchequevalue As String = ""
        Dim strrcvfrmvalue As String = ""
        Dim strnarrationvalue As String = ""
        Dim reportfiltercustomerBank, reportfilterText, reportfilterchequeno, reportfilterstatus, reportfilterrecievedfrom, reportfiltercurrency, reportfilterrecieptno, reportfilternarration, reportfilterbankname As String
        If txtFromDate.Text.Trim <> "" And txtToDate.Text <> "" Then
            reportfilter = "Transaction From  " & txtFromDate.Text.Trim & " To " & txtToDate.Text
        End If

        Try
            If dtt.Rows.Count > 0 Then
                For i As Integer = 0 To dtt.Rows.Count - 1
                    If dtt.Rows(i)("Code").ToString = "CUSTOMERBANK" Then

                        Dim dttrans As New DataTable
                        strSqlQry = "select other_bank_master_code as other_bank_master_code from customer_bank_master where  other_bank_master_des ='" & dtt.Rows(i)("value").ToString & "'"
                        SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                        myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
                        myDataAdapter.Fill(dttrans)
                        If dttrans.Rows.Count > 0 Then
                            For b As Integer = 0 To dttrans.Rows.Count - 1

                                If straccountvalue <> "" Then
                                    straccountvalue = straccountvalue + ",'" + dttrans.Rows(b)("other_bank_master_code").ToString + "'"
                                Else
                                    straccountvalue = "'" + dttrans.Rows(b)("other_bank_master_code").ToString + "'"
                                End If
                            Next
                        Else
                            If straccountvalue <> "" Then
                                straccountvalue = straccountvalue + ",'" + dtt.Rows(i)("Value").ToString + "'"
                            Else
                                straccountvalue = "'" + dtt.Rows(i)("Value").ToString + "'"
                            End If

                        End If


                        reportfiltercustomerBank = dtt.Rows(i)("Code").ToString + ":" + straccountvalue
                        reportfilter = reportfilter & " " & reportfiltercustomerBank

                    End If
                    If dtt.Rows(i)("Code").ToString = "RECEIPTNO" Or dtt.Rows(i)("Code").ToString = "BPVNO" Or dtt.Rows(i)("Code").ToString = "CPVNO" Then
                        If strdocumentvalue <> "" Then
                            strdocumentvalue = strdocumentvalue + ",'" + dtt.Rows(i)("Value").ToString + "'"
                        Else
                            strdocumentvalue = "'" + dtt.Rows(i)("Value").ToString + "'"
                        End If
                        reportfilterrecieptno = dtt.Rows(i)("Code").ToString + ":" + strdocumentvalue
                    End If

                    If dtt.Rows(i)("Code").ToString = "CURRENCY" Then
                        If strcurrvalue <> "" Then
                            strcurrvalue = strcurrvalue + ",'" + dtt.Rows(i)("Value").ToString + "'"
                        Else
                            strcurrvalue = "'" + dtt.Rows(i)("Value").ToString + "'"
                        End If
                        reportfiltercurrency = dtt.Rows(i)("Code").ToString + ":" + strcurrvalue
                        reportfilter = reportfilter & " " & reportfiltercurrency
                    End If
                    If dtt.Rows(i)("Code").ToString = "RECEIVEDFROM" Or dtt.Rows(i)("Code").ToString = "PAIDTO" Then
                        If strrcvfrmvalue <> "" Then
                            strrcvfrmvalue = strrcvfrmvalue + ",'" + dtt.Rows(i)("Value").ToString + "'"
                        Else
                            strrcvfrmvalue = "" + dtt.Rows(i)("Value").ToString + ""
                        End If

                        reportfilterrecievedfrom = dtt.Rows(i)("Code").ToString + ":" + strrcvfrmvalue
                        reportfilter = reportfilter & " " & reportfilterrecievedfrom
                    End If
                    If dtt.Rows(i)("Code").ToString = "NARRATION" Then
                        If strnarrationvalue <> "" Then
                            strnarrationvalue = strnarrationvalue + ",'" + dtt.Rows(i)("Value").ToString + "'"
                        Else
                            strnarrationvalue = "" + dtt.Rows(i)("Value").ToString + ""
                        End If
                        reportfilternarration = dtt.Rows(i)("Code").ToString + ":" + strnarrationvalue
                        reportfilter = reportfilter & " " & reportfilternarration

                    End If
                    If dtt.Rows(i)("Code").ToString = "BANKNAME" Or dtt.Rows(i)("Code").ToString = "CASHA/C" Then
                        Dim dtbank As New DataTable

                        strSqlQry = " select  acctmast.acctcode as acctcode from acctmast  ,receipt_master_new  where  acctmast.acctcode=receipt_master_new.receipt_cashbank_code and  acctname ='" & dtt.Rows(i)("value").ToString & "' and div_code='" & ViewState("divcode") & "'"
                        SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                        myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
                        myDataAdapter.Fill(dtbank)
                        If dtbank.Rows.Count > 0 Then
                            For c As Integer = 0 To dtbank.Rows.Count - 1
                                If strbankvalue <> "" Then
                                    strbankvalue = strbankvalue + ",'" + dtbank.Rows(c)("acctcode").ToString + "'"
                                Else
                                    strbankvalue = "'" + dtbank.Rows(c)("acctcode").ToString + "'"
                                End If
                            Next
                        Else
                            If strbankvalue <> "" Then
                                strbankvalue = strbankvalue + ",'" + dtt.Rows(i)("Value").ToString + "'"
                            Else
                                strbankvalue = "'" + dtt.Rows(i)("Value").ToString + "'"
                            End If
                        End If
                        reportfilterbankname = dtt.Rows(i)("Code").ToString + ":" + strbankvalue
                        reportfilter = reportfilter & " " & reportfilterbankname
                    End If
                    If dtt.Rows(i)("Code").ToString = "STATUS" Then
                        If dtt.Rows(i)("Value").ToString = "POSTED" Then
                            If strStatusvalue <> "" Then
                                strStatusvalue = strStatusvalue + ",'" + "P" + "'"
                            Else
                                strStatusvalue = "'" + "P".ToString + "'"
                            End If
                        Else
                            If strStatusvalue <> "" Then
                                strStatusvalue = strStatusvalue + ",'" + "U" + "'"
                            Else
                                strStatusvalue = "'" + "U".ToString + "'"
                            End If
                        End If
                        reportfilterchequeno = dtt.Rows(i)("Code").ToString + ":" + strStatusvalue
                        reportfilter = reportfilter & " " & reportfilterchequeno
                    End If




                    If dtt.Rows(i)("Code").ToString = "CHEQUENO" Then

                        If strchequevalue <> "" Then
                            strchequevalue = strchequevalue + "," + dtt.Rows(i)("Value").ToString
                        Else
                            strchequevalue = dtt.Rows(i)("Value").ToString
                        End If
                        reportfilterstatus = dtt.Rows(i)("Code").ToString + ":" + strchequevalue
                        reportfilter = reportfilter & " " & reportfilterstatus
                    End If
                    If dtt.Rows(i)("Code").ToString = "TEXT" Then

                        If strTextValue <> "" Then
                            strTextValue = strTextValue + "," + dtt.Rows(i)("Value").ToString
                        Else
                            strTextValue = dtt.Rows(i)("Value").ToString

                        End If

                        reportfilterText = dtt.Rows(i)("Code").ToString + " like " + strTextValue
                        reportfilter = reportfilter & " " & reportfilterText

                    End If
                Next
            End If
            strBindCondition = BuildConditionNew(straccountvalue, strdocumentvalue, strcurrvalue, strchequevalue, strbankvalue, strrcvfrmvalue, strnarrationvalue, strStatusvalue, strTextValue)
            'customer_bank_master.other_bank_master_des, 
            If ddlrpt.SelectedValue = "Detailed" Then
                strSqlQry = "SELECT receipt_master_new.tran_id,convert(varchar(10),receipt_master_new.receipt_date,103)  as receipt_date,  " & _
"   '',receipt_master_new.receipt_received_from, case when isnull(tran_type,'')='RV' then vw_bankname.acctname when isnull(tran_type,'')='CPV' then  acctmast.acctname when isnull(tran_type,'')='BPV' then vw_bankname.acctname  else vw_bankname.acctname end acctname,receipt_master_new.receipt_cheque_number,convert(varchar(10),receipt_master_new.receipt_tran_date,103) as receipt_tran_date, " & _
            " '',receipt_master_new.receipt_currency_id,'','','','','', case when isnull(tran_type,'')='RV' then receipt_master_new.receipt_credit when isnull(tran_type,'')='CPV' then  receipt_master_new.receipt_debit when isnull(tran_type,'')='BPV' then receipt_master_new.receipt_debit  when isnull(tran_type,'')='DEP' then receipt_master_new.receipt_debit  else 0 end receipt_credit ," & _
           "  case when isnull(tran_type,'')='RV' then receipt_master_new.receipt_credit* receipt_currency_rate  when isnull(tran_type,'')='CPV' then  receipt_master_new.receipt_debit* receipt_currency_rate  when isnull(tran_type,'')='BPV' then receipt_master_new.receipt_debit*receipt_currency_rate  when isnull(tran_type,'')='DEP' then receipt_master_new.receipt_debit*receipt_currency_rate  else 0 end receipt_credit_base,  receipt_master_new.receipt_narration " & _
" FROM  receipt_master_new LEFT OUTER JOIN  " & _
           " customer_bank_master ON receipt_master_new.receipt_customer_bank = customer_bank_master.other_bank_master_code  " & _
           " left join vw_bankname on receipt_master_new.receipt_cashbank_code =vw_bankname.acctcode  and  vw_bankname.div_code=receipt_master_new.receipt_div_id  left outer join acctmast  on acctmast.div_code=receipt_master_new.receipt_div_id and acctmast.acctcode=receipt_master_new.receipt_cashbank_code  where  receipt_master_new.receipt_div_id='" & ViewState("divcode") & "' and  receipt_master_new.tran_type='" & CType(ViewState("ReceiptsSearchRVPVTranType"), String) & "'"

            Else
                strSqlQry = "SELECT receipt_master_new.tran_id,convert(varchar(10),receipt_master_new.receipt_date,103)  as receipt_date,  " & _
               "   receipt_master_new.receipt_received_from, case when isnull(tran_type,'')='RV' then vw_bankname.acctname when isnull(tran_type,'')='CPV' then  acctmast.acctname when isnull(tran_type,'')='BPV' then vw_bankname.acctname  else vw_bankname.acctname end acctname,receipt_master_new.receipt_cheque_number,convert(varchar(10),receipt_master_new.receipt_tran_date,103) as receipt_tran_date, " & _
                            "receipt_master_new.receipt_currency_id,receipt_currency_rate, case when isnull(tran_type,'')='RV' then receipt_master_new.receipt_credit when isnull(tran_type,'')='CPV' then  receipt_master_new.receipt_debit when isnull(tran_type,'')='BPV' then receipt_master_new.receipt_debit  when isnull(tran_type,'')='DEP' then receipt_master_new.receipt_debit  else 0 end receipt_credit ," & _
                           "  case when isnull(tran_type,'')='RV' then receipt_master_new.receipt_credit* receipt_currency_rate  when isnull(tran_type,'')='CPV' then  receipt_master_new.receipt_debit* receipt_currency_rate  when isnull(tran_type,'')='BPV' then receipt_master_new.receipt_debit*receipt_currency_rate  when isnull(tran_type,'')='DEP' then receipt_master_new.receipt_debit*receipt_currency_rate  else 0 end receipt_credit_base,  receipt_master_new.receipt_narration " & _
               " FROM  receipt_master_new LEFT OUTER JOIN  " & _
                           " customer_bank_master ON receipt_master_new.receipt_customer_bank = customer_bank_master.other_bank_master_code  " & _
                           " left join vw_bankname on receipt_master_new.receipt_cashbank_code =vw_bankname.acctcode  and  vw_bankname.div_code=receipt_master_new.receipt_div_id  left outer join acctmast  on acctmast.div_code=receipt_master_new.receipt_div_id and acctmast.acctcode=receipt_master_new.receipt_cashbank_code  where  receipt_master_new.receipt_div_id='" & ViewState("divcode") & "' and  receipt_master_new.tran_type='" & CType(ViewState("ReceiptsSearchRVPVTranType"), String) & "'"

            End If



            Dim strorderby As String = Session("RExpression")
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

    Private Sub detailedReport()
        Dim FolderPath As String = "..\ExcelTemplates\"
        Dim FileName As String = "accountsTransactionDetailed_template.xlsx"
        Dim FilePath As String = Server.MapPath(FolderPath + FileName)
        Dim RandomCls As New Random()
        Dim RandomNo As String = RandomCls.Next(100000, 9999999).ToString
        Dim rptcompanyname, basecurrency As String

        rptcompanyname = CType(objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select division_master_des from division_master where division_master_code='" & txtdivcode.Value & "'"), String)
        basecurrency = CType(objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id=457"), String)


        Dim FileNameNew As String
        Select Case ViewState("ReceiptsSearchRVPVTranType")
            Case "RV"
                FileNameNew = "RecieptsRegisterDetailed_" & Now.Year & Now.Month & Now.Day & Now.Hour & Now.Minute & Now.Second & Now.Millisecond & ".xlsx"
            Case "CPV"
                FileNameNew = "CashPaymentsRegisterDetailed_" & Now.Year & Now.Month & Now.Day & Now.Hour & Now.Minute & Now.Second & Now.Millisecond & ".xlsx"

            Case "BPV"
                FileNameNew = "BankPaymentsRegisterDetailed_" & Now.Year & Now.Month & Now.Day & Now.Hour & Now.Minute & Now.Second & Now.Millisecond & ".xlsx"

            Case "DEP"
                FileNameNew = "DepositListDetailed_" & Now.Year & Now.Month & Now.Day & Now.Hour & Now.Minute & Now.Second & Now.Millisecond & ".xlsx"
            Case "CV"
                FileNameNew = "ContraRegisterDetailed_" & Now.Year & Now.Month & Now.Day & Now.Hour & Now.Minute & Now.Second & Now.Millisecond & ".xlsx"

        End Select
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



        Select Case ViewState("ReceiptsSearchRVPVTranType")
            Case "RV"
                ws.Cell(2, 1).Value = "Reciept Register:Detailed Report"
                ws.Cell(6, 1).Value = "RecieptNo"

                ws.Cell(6, 4).Value = "Recieved From /Detail Account"
                ws.Cell(6, 5).Value = "Bank Name"
                ws.Cell(6, 15).Value = "Amount Recieved"
                ws.Cell(6, 16).Value = "Amount Recieved"
                ws.Cell(6, 16).Value = ws.Cell(6, 16).Value & " (" & basecurrency & ")"
            Case "CV"
                ws.Cell(2, 1).Value = "Contra Register:Detailed Report"
                ws.Cell(6, 1).Value = "Contra No"

                ws.Cell(6, 4).Value = "Recieved From /Detail Account"
                ws.Cell(6, 5).Value = "Bank Name"
                ws.Cell(6, 15).Value = "Amount Recieved"
                ws.Cell(6, 16).Value = "Amount Recieved"
                ws.Cell(6, 16).Value = ws.Cell(6, 16).Value & " (" & basecurrency & ")"
            Case "CPV"
                ws.Cell(2, 1).Value = "Cash Payment Register:Detailed Report"
                ws.Cell(6, 1).Value = "CPVNo"
                ws.Cell(6, 4).Value = "Paid To /Detail Account"
                ws.Cell(6, 5).Value = "Cash A/c"
                ws.Cell(6, 15).Value = "Amount Paid"
                ws.Cell(6, 16).Value = "Amount Paid"
                ws.Cell(6, 16).Value = ws.Cell(6, 16).Value & " (" & basecurrency & ")"

            Case "BPV"
                ws.Cell(2, 1).Value = "Bank Payment Register:Detailed Report"
                ws.Cell(6, 1).Value = "BPVNo"
                ws.Cell(6, 4).Value = "Paid To /Detail Account"
                ws.Cell(6, 5).Value = "Bank Name"
                ws.Cell(6, 15).Value = "Amount Paid"
                ws.Cell(6, 16).Value = "Amount Paid"
                ws.Cell(6, 16).Value = ws.Cell(6, 16).Value & " (" & basecurrency & ")"
            Case "DEP"
                ws.Cell(2, 1).Value = "Deposit List:Detailed Report"
                ws.Cell(6, 1).Value = "DEPNo"
                ws.Cell(6, 4).Value = "Recieved From /Detail Account"
                ws.Cell(6, 5).Value = "Bank Name"
                ws.Cell(6, 15).Value = "Amount Recieved"
                ws.Cell(6, 16).Value = "Amount Recieved"
                ws.Cell(6, 16).Value = ws.Cell(6, 16).Value & " (" & basecurrency & ")"

        End Select

        'create header

        ws.Cell(6, 2).Value = "Date"

        ws.Cell(6, 3).Value = "AccountType"

        ws.Cell(6, 6).Value = "Cheque No"
        ws.Cell(6, 7).Value = "Cheque Date(Posting Date)"
        ws.Cell(6, 8).Value = "SourceCountry"
        ws.Cell(6, 9).Value = "Currency"
        ws.Cell(6, 10).Value = "Conversion Rate"
        ws.Cell(6, 11).Value = "Debit"
        ws.Cell(6, 12).Value = "Credit"
        ws.Cell(6, 13).Value = "Debit"
        ws.Cell(6, 13).Value = ws.Cell(6, 13).Value & " (" & basecurrency & ")"
        ws.Cell(6, 14).Value = "Credit"
        ws.Cell(6, 14).Value = ws.Cell(6, 14).Value & " (" & basecurrency & ")"
       
        ws.Cell(6, 17).Value = "Narration"
        ws.Cell(6, 17).Style.Font.Bold = True
        ws.Cell(6, 17).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin)
        ws.Cell(6, 17).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center
        ws.Cell(6, 17).Style.Alignment.Vertical = XLAlignmentVerticalValues.Top
        ws.Cell(6, 16).Style.Alignment.WrapText = True


        ws.Cell(6, 16).Style.Font.Bold = True
        ws.Cell(6, 16).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin)
        ws.Cell(6, 16).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center
        ws.Cell(6, 16).Style.Alignment.Vertical = XLAlignmentVerticalValues.Top




        ws.Column("3").Width = 16
        ws.Column("4").Width = 40
        ws.Column("5").Width = 32
        ws.Column("15").Width = 16.29
        ws.Column("16").Width = 16.29
        ws.Column("17").Width = 57.29
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

        If (ViewState("ReceiptsSearchRVPVTranType") = "CPV") Then
            ws.Columns(6).Hide()
            ws.Columns(7).Hide()

        End If


        'dt_sum = ds.Tables(1)
        LastLine = 7
        Dim total_basedebit, total_basecredit, total As Double
        total = Convert.ToDouble(dt.Compute("SUM(receipt_credit_base)", String.Empty))

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
                RateSheet.Columns("10:16").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right
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



                sqlst = "select '','',case receipt_acc_type when 'C' then 'Customer' when 'S' then 'Supplier' when 'A' then 'Agent' when 'G' then 'General ' end,view_account.des,'','','' ,ctrymast.ctryname,receipt_currency_id,receipt_currency_rate,receipt_debit,receipt_credit,basedebit,basecredit,'','',receipt_narration " & _
                        " from receipt_detail left join view_account on receipt_detail.receipt_acc_code=view_account.code and view_account.div_code=receipt_detail.div_id" & _
                        " and receipt_detail.receipt_acc_type=view_account.type" & _
                            " left join ctrymast on ctrymast.ctrycode=dept  where receipt_detail.div_id='" & txtdivcode.Value.Trim & "' and receipt_detail.tran_type='" & ViewState("ReceiptsSearchRVPVTranType") & "' and receipt_detail.tran_id='" & id & "'"
                dt_detail_ds = objUtils.GetDataFromDatasetnew(Session("dbconnectionName"), sqlst)
                dt_detail = dt_detail_ds.Tables(0)

                Dim RateSheet_detail As IXLRange
                RateSheet_detail = ws.Cell(LastLine, 1).InsertTable(dt_detail.AsEnumerable).SetShowHeaderRow(False).AsRange()
                ws.Rows(LastLine).Clear()
                ws.Rows(LastLine).Delete()
                RateSheet_detail.Style.Font.FontName = "arial"
                RateSheet_detail.Columns("10:16").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right
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

            ws.Cell(LastLine, 12).Value = "Total"
            ws.Cell(LastLine, 12).Style.Font.Bold = True
            ws.Cell(LastLine, 12).Style.Font.FontSize = 14
            ws.Cell(LastLine, 12).Style.Font.FontName = "arial"


            ws.Cell(LastLine, 13).Value = total_basedebit
            ws.Cell(LastLine, 13).Style.Font.Bold = True
            ws.Cell(LastLine, 13).Style.Font.FontSize = 14
            ws.Cell(LastLine, 13).Style.Font.FontName = "arial"


            ws.Cell(LastLine, 14).Value = total_basecredit
            ws.Cell(LastLine, 14).Style.Font.Bold = True
            ws.Cell(LastLine, 14).Style.Font.FontSize = 14
            ws.Cell(LastLine, 14).Style.Font.FontName = "arial"

            ws.Cell(LastLine, 16).Value = total
            ws.Cell(LastLine, 16).Style.Font.Bold = True
            ws.Cell(LastLine, 16).Style.Font.FontSize = 14
            ws.Cell(LastLine, 16).Style.Font.FontName = "arial"

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

        Try
            If ddlrpt.SelectedValue = "Detailed" Then
                detailedReport()

            Else



                Dim FolderPath As String = "..\ExcelTemplates\"
                Dim FileName As String = "accountsTransaction_template.xlsx"
                Dim FilePath As String = Server.MapPath(FolderPath + FileName)
                Dim RandomCls As New Random()
                Dim RandomNo As String = RandomCls.Next(100000, 9999999).ToString
                Dim rptcompanyname, basecurrency As String

                rptcompanyname = CType(objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select division_master_des from division_master where division_master_code='" & txtdivcode.Value & "'"), String)
                basecurrency = CType(objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id=457"), String)
                Dim FileNameNew As String
                Select Case ViewState("ReceiptsSearchRVPVTranType")
                    Case "RV"
                        FileNameNew = "RecieptsRegisterBrief_" & Now.Year & Now.Month & Now.Day & Now.Hour & Now.Minute & Now.Second & Now.Millisecond & ".xlsx"
                    Case "CPV"
                        FileNameNew = "CashPaymentsRegisterBrief_" & Now.Year & Now.Month & Now.Day & Now.Hour & Now.Minute & Now.Second & Now.Millisecond & ".xlsx"

                    Case "BPV"
                        FileNameNew = "BankPaymentsRegisterBrief_" & Now.Year & Now.Month & Now.Day & Now.Hour & Now.Minute & Now.Second & Now.Millisecond & ".xlsx"

                    Case "DEP"
                        FileNameNew = "DepositListBrief_" & Now.Year & Now.Month & Now.Day & Now.Hour & Now.Minute & Now.Second & Now.Millisecond & ".xlsx"
                    Case "CV"
                        FileNameNew = "ContraRegisterBrief_" & Now.Year & Now.Month & Now.Day & Now.Hour & Now.Minute & Now.Second & Now.Millisecond & ".xlsx"
                End Select



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

                ws.Column(10).Width = 16.29
                ws.Column(11).Width = 57.3

                Select Case ViewState("ReceiptsSearchRVPVTranType")
                    Case "RV"
                        ws.Cell(2, 1).Value = "Reciept Register:Brief Report"
                        ws.Cell(6, 1).Value = "RecieptNo"
                        ws.Cell(6, 3).Value = "Recieved From"
                        ws.Cell(6, 4).Value = "Bank Name"
                        ws.Cell(6, 7).Value = "Bank Currency"
                        ws.Cell(6, 8).Value = "Bank Conversion Rate"
                        ws.Cell(6, 9).Value = "Amount Recieved"
                        ws.Cell(6, 10).Value = "Amount Recieved"
                        ws.Cell(6, 10).Value = ws.Cell(6, 10).Value & " (" & basecurrency & ")"
                    Case "CV"
                        ws.Cell(2, 1).Value = "Contra Register:Brief Report"
                        ws.Cell(6, 1).Value = "Contra No"
                        ws.Cell(6, 3).Value = "Recieved From"
                        ws.Cell(6, 4).Value = "Bank Name"
                        ws.Cell(6, 7).Value = "Bank Currency"
                        ws.Cell(6, 8).Value = "Bank Conversion Rate"
                        ws.Cell(6, 9).Value = "Amount Recieved"
                        ws.Cell(6, 10).Value = "Amount Recieved"
                        ws.Cell(6, 10).Value = ws.Cell(6, 10).Value & " (" & basecurrency & ")"
                    Case "CPV"
                        ws.Cell(2, 1).Value = "Cash Payment Register:Brief Report"
                        ws.Cell(6, 1).Value = "CPVNo"
                        ws.Cell(6, 3).Value = "Paid To"
                        ws.Cell(6, 4).Value = "Cash A/c"
                        ws.Cell(6, 7).Value = "Currency"
                        ws.Cell(6, 8).Value = "Conversion Rate"
                        ws.Cell(6, 9).Value = "Amount Paid"
                        ws.Cell(6, 10).Value = "Amount Paid"
                        ws.Cell(6, 10).Value = ws.Cell(6, 10).Value & " (" & basecurrency & ")"

                    Case "BPV"
                        ws.Cell(2, 1).Value = "Bank Payment Register:Brief Report"
                        ws.Cell(6, 1).Value = "BPVNo"
                        ws.Cell(6, 3).Value = "Paid To"
                        ws.Cell(6, 4).Value = "Bank Name"
                        ws.Cell(6, 7).Value = "Bank Currency"
                        ws.Cell(6, 8).Value = "Bank Conversion Rate"
                        ws.Cell(6, 9).Value = "Amount Paid"
                        ws.Cell(6, 10).Value = "Amount Paid"
                        ws.Cell(6, 10).Value = ws.Cell(6, 10).Value & " (" & basecurrency & ")"

                    Case "DEP"
                        ws.Cell(2, 1).Value = "Deposit List:Brief Report"
                        ws.Cell(6, 1).Value = "DEPNo"
                        ws.Cell(6, 4).Value = "Bank Name"
                        ws.Cell(6, 7).Value = "Bank Currency"
                        ws.Cell(6, 8).Value = "Bank Conversion Rate"
                        ws.Cell(6, 9).Value = "Amount Recieved"
                        ws.Cell(6, 10).Value = "Amount Recieved"
                        ws.Cell(6, 10).Value = ws.Cell(6, 10).Value & " (" & basecurrency & ")"

                End Select




                'create header

                ws.Cell(6, 2).Value = "Date"


                ws.Cell(6, 5).Value = "Cheque No"
                ws.Cell(6, 6).Value = "Cheque Date(Posting Date)"

               
                ws.Cell(6, 11).Value = "Narration"
                ws.Cell(6, 11).Style.Font.Bold = True
                ws.Cell(6, 11).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                ws.Cell(6, 11).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center
                ws.Cell(6, 11).Style.Alignment.Vertical = XLAlignmentVerticalValues.Top

                Dim sql As String

                sql = FillGridNew_report()
                ws.Cell(4, 1).Value = reportfilter

                Dim myDS As New DataSet

                SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
                myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
                myDataAdapter.Fill(myDS)
                If (ViewState("ReceiptsSearchRVPVTranType") = "CPV") Then
                    ws.Columns(5).Hide()
                    ws.Columns(6).Hide()

                End If


                Dim dt As New DataTable



                dt = myDS.Tables(0)

                Dim RateSheet As IXLRange
                RateSheet = ws.Cell(7, 1).InsertTable(dt.AsEnumerable).SetShowHeaderRow(False).AsRange()
                ws.Rows(7).Clear()
                ws.Rows(7).Delete()
                LastLine = 7 + dt.Rows.Count

                If dt.Rows.Count > 1 Then
                    ws.Range(LastLine, 1, LastLine, 11).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                    ws.Range(LastLine, 1, LastLine, 11).Style.Border.SetLeftBorder(XLBorderStyleValues.Thin)
                    ws.Range(LastLine, 1, LastLine, 11).Style.Font.Bold = True
                    ws.Cell(LastLine, 9).Value = "Total"
                    ws.Cell(LastLine, 9).Style.Font.FontName = "arial"
                    ws.Cell(LastLine, 9).Style.Font.Bold = True
                    RateSheet.Columns("8:10").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right

                    RateSheet.Columns("8:10").SetDataType(XLCellValues.Number)
                    ws.Cell(LastLine, 10).SetFormulaR1C1("=SUM(j7:j" & LastLine - 1 & ")")
                    ws.Cell(LastLine, 10).Style.Font.FontName = "arial"
                    ws.Cell(LastLine, 10).Style.Font.Bold = True
                End If



                RateSheet.Style.Font.FontName = "arial"

                'RateSheet.Columns("9:13").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right
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
End Class
