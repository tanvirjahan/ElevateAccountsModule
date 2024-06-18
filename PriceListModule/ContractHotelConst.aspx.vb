Imports System.Data
Imports System.Data.SqlClient
Imports System.Drawing.Color
Imports System.Collections.ArrayList
Imports System.Collections.Generic
Imports System.Drawing

Partial Class ContractHotelConst
    Inherits System.Web.UI.Page
    Private Connection As SqlConnection
    Dim objUser As New clsUser


#Region "Global Declaration"
    Dim objUtils As New clsUtils
    Dim strSqlQry As String
    Dim mySqlCmd As SqlCommand
    Dim mySqlReader As SqlDataReader
    Dim mySqlConn As SqlConnection
    Dim sqlTrans As SqlTransaction
    Dim ObjDate As New clsDateTime


    Dim strWhereCond As String
    Dim SqlConn As SqlConnection
    Dim myCommand As SqlCommand
    Dim myDataAdapter As SqlDataAdapter


    Dim CopyRow As Integer = 0
    Dim CopyClick As Integer = 0
    Dim n As Integer = 0
    Dim count As Integer = 0
    Dim Exhnamenew As New ArrayList
    Dim Roomtypenew As New ArrayList
    Dim Mealplannew As New ArrayList
    Dim Suppamountnew As New ArrayList
    Dim Minstaynew As New ArrayList
    Dim withdrawnnew As New ArrayList
    Dim fDatenew As New ArrayList
    Dim tDatenew As New ArrayList


#End Region
#Region "Enum GridCol"
    Enum GridCol
        tranid = 1
        Fromdate = 2
        Todate = 3
        reason = 4
        remarks = 5
        Edit = 6
        View = 7
        Delete = 8
        DateCreated = 9
        UserCreated = 10
        DateModified = 11
        UserModified = 12


    End Enum
#End Region
#Region "Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim RefCode As String
        Dim CalledfromValue As String = ""

        Dim Minappid As String = ""
        Dim Minappname As String = ""


        If IsPostBack = False Then
            Minappid = 1
            Minappname = objUser.GetAppName(Session("dbconnectionName"), Minappid)

            If CType(Session("GlobalUserName"), String) = Nothing Or CType(Session("Userpwd"), String) = Nothing Then
                Response.Redirect("~/Login.aspx", False)
                Exit Sub
            Else


                Me.SubMenuUserControl1.menuidval = objUser.GetMenuId(Session("dbconnectionName"), CType("PriceListModule\ContractsSearch.aspx?appid=1", String), CType(Request.QueryString("appid"), Integer))
                CalledfromValue = Me.SubMenuUserControl1.menuidval
                objUser.CheckUserSubRight(Session("dbconnectionName"), CType(Session("GlobalUserName"), String), CType(Session("Userpwd"), String), _
                                                   CType(Minappname, String), "ContractHotelConst.aspx", CType(CalledfromValue, String), btnAddNew, btnExportToExcel, _
             btnprint, gv_SearchResult, GridCol.Edit, GridCol.Delete, GridCol.View)




            End If



            Me.SubMenuUserControl1.appval = CType(Request.QueryString("appid"), String)
            Me.SubMenuUserControl1.menuidval = objUser.GetMenuId(Session("dbconnectionName"), CType("PriceListModule\ContractsSearch.aspx?appid=1", String), CType(Request.QueryString("appid"), Integer))

            '  objUtils.Clear_All_contract_sessions()
            txtconnection.Value = Session("dbconnectionName")

            hdCurrentDate.Value = Now.ToString("dd/MM/yyyy")

            hdnpartycode.Value = CType(Session("Contractparty"), String)
            SubMenuUserControl1.partyval = hdnpartycode.Value
            SubMenuUserControl1.contractval = CType(Session("contractid"), String)
            hdncontractid.Value = CType(Session("contractid"), String)

            '  hdnpartycode.Value = CType(Request.QueryString("partycode"), String)
            txthotelname.Text = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select partyname from partymast where partycode='" & hdnpartycode.Value & "'")
            ViewState("hotelname") = txthotelname.Text
            Session("partycode") = hdnpartycode.Value

            '  Session("contractid") = CType(Request.QueryString("contractid"), String)
            lblHeading.Text = lblHeading.Text + " - " + txthotelname.Text



            FillGrid("constructionid", hdnpartycode.Value, "Desc")
            '   PanelMain.Visible = False

        Else

        End If

        btnAddNew.Attributes.Add("onclick", "return CheckContract('" & hdncontractid.Value & "')")
        ' btncopycontract.Attributes.Add("onclick", "return CheckContract('" & hdncontractid.Value & "')")


        Dim typ As Type
        typ = GetType(DropDownList)

        If (Page.ClientScript.IsClientScriptIncludeRegistered(typ, "TADDScript.js")) = False Then
            Page.ClientScript.RegisterClientScriptInclude(typ, "TADDScript.js", "TADDScript.js")


        End If
        Session.Add("submenuuser", "ContractsSearch.aspx")
    End Sub
#End Region


  


#Region "Protected Sub gv_SearchResult_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gv_SearchResult.PageIndexChanging"
    Protected Sub gv_SearchResult_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gv_SearchResult.PageIndexChanging
        gv_SearchResult.PageIndex = e.NewPageIndex


        '  FillGrid("partymaxacc_header.tranid", hdnpartycode.Value, "Desc")


    End Sub

#End Region


    Private Sub ShowRecord(ByVal RefCode As String)

        Try
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            mySqlCmd = New SqlCommand("Select partycode,Miscellaneous,reason,fromdate, todate,constructionid from hotels_construction Where constructionid='" & RefCode & "'", mySqlConn)
            mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
            If mySqlReader.HasRows Then
                If mySqlReader.Read() = True Then
                    If IsDBNull(mySqlReader("partycode")) = False Then
                        hdnpartycode.Value = CType(mySqlReader("partycode"), String)
                    End If
                    If IsDBNull(mySqlReader("Miscellaneous")) = False Then
                        txtremarks.Value = CType(mySqlReader("Miscellaneous"), String)
                    End If
                    If IsDBNull(mySqlReader("reason")) = False Then
                        txtReason.Value = CType(mySqlReader("reason"), String)
                    End If
                    If IsDBNull(mySqlReader("fromdate")) = False Then
                        txtFromDate.Text = mySqlReader("fromdate")
                    End If
                    If IsDBNull(mySqlReader("todate")) = False Then
                        txtToDate.Text = mySqlReader("todate")
                    End If
                    txttranid.Text = RefCode
                   

                End If
            End If



        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ContractHotelConst.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
            clsDBConnect.dbReaderClose(mySqlReader)             'sql reader disposed    
            clsDBConnect.dbConnectionClose(mySqlConn)           'connection close           
        End Try
    End Sub






#Region "Private Function CreateDataSource(ByVal lngcount As Long) As DataView"
    Private Function CreateDataSource(ByVal lngcount As Long) As DataView
        Dim dt As DataTable
        Dim dr As DataRow
        Dim i As Integer
        dt = New DataTable
        dt.Columns.Add(New DataColumn("SrNo", GetType(Integer)))
        For i = 1 To lngcount
            dr = dt.NewRow()
            dr(0) = i
            dt.Rows.Add(dr)
        Next
        'return a DataView to the DataTable
        CreateDataSource = New DataView(dt)
        'End If
    End Function
#End Region
#Region "Public Sub fillgrd(ByVal grd As GridView, Optional ByVal blnload As Boolean = False, Optional ByVal count As Integer = 1)"
    Public Sub fillDategrd(ByVal grd As GridView, Optional ByVal blnload As Boolean = False, Optional ByVal count As Integer = 1)
        Dim lngcnt As Long
        If blnload = True Then
            lngcnt = 5
        Else
            lngcnt = count
        End If

        grd.DataSource = CreateDataSource(lngcnt)
        grd.DataBind()
        grd.Visible = True

    End Sub
#End Region
    Private Function BuildCondition() As String
        strWhereCond = ""
        'If txtSupplierCode.Text.Trim <> "" Then
        '    If Trim(strWhereCond) = "" Then

        '        strWhereCond = " upper(partymaxacc_header.partycode) LIKE '" & Trim(txtSupplierCode.Text.Trim.ToUpper) & "%'"
        '    Else
        '        strWhereCond = strWhereCond & " AND upper(partymaxacc_header.partycode) LIKE '" & Trim(txtSupplierCode.Text.Trim.ToUpper) & "%'"
        '    End If
        'End If

        'If txtSupplierName.Text.Trim <> "" Then
        '    If Trim(strWhereCond) = "" Then
        '        strWhereCond = " upper(partymast.partyname) LIKE '" & Trim(txtSupplierName.Text.Trim.ToUpper) & "%'"
        '    Else
        '        strWhereCond = strWhereCond & " AND upper(partymast.partyname) LIKE '" & Trim(txtSupplierName.Text.Trim.ToUpper) & "%'"
        '    End If
        'End If
        'If ddlSupplierType.Value.Trim <> "[Select]" Then
        '    If Trim(strWhereCond) = "" Then
        '        strWhereCond = " partymaxacc_header.sptypecode= '" & Trim(ddlSupplierType.Items(ddlSupplierType.SelectedIndex).Text) & "'"
        '    Else
        '        strWhereCond = strWhereCond & " AND partymaxacc_header.sptypecode= '" & Trim(ddlSupplierType.Items(ddlSupplierType.SelectedIndex).Text) & "'"
        '    End If
        'End If
        'If ddlSupplierTypeName.Value.Trim <> "[Select]" Then
        '    If Trim(strWhereCond) = "" Then
        '        strWhereCond = " sptypemast.sptypename = '" & Trim(ddlSupplierTypeName.Items(ddlSupplierTypeName.SelectedIndex).Text) & "'"
        '    Else
        '        strWhereCond = strWhereCond & " AND sptypemast.sptypename = '" & Trim(ddlSupplierTypeName.Items(ddlSupplierTypeName.SelectedIndex).Text) & "'"
        '    End If
        'End If
        BuildCondition = strWhereCond
    End Function






    Private Sub FillGrid(ByVal strorderby As String, ByVal partycode As String, Optional ByVal strsortorder As String = "ASC")
        Dim myDS As New DataSet

        gv_SearchResult.Visible = True


        If gv_SearchResult.PageIndex < 0 Then
            gv_SearchResult.PageIndex = 0
        End If



        strSqlQry = "" ' "select  '' constructionid , '' fromdate , '' todate , '' reason ,'' Miscellaneous ,'' adddate, '' adduser, ''  moddate ,'' moduser"
        Try
            strSqlQry = "SELECT constructionid ,convert(varchar(10),fromdate,103) fromdate,convert(varchar(10),todate,103) todate," & _
                "ISNULL(reason,'') reason,ISNULL(Miscellaneous,'') Miscellaneous, " & _
                "adddate, adduser,moddate,moduser FROM hotels_construction where partycode='" & partycode & "'"

            'If Trim(BuildCondition) <> "" Then
            '    strSqlQry = strSqlQry & " WHERE " & BuildCondition() & " ORDER BY " & strorderby & " " & strsortorder
            'Else
            '    strSqlQry = strSqlQry & " ORDER BY " & strorderby & " " & strsortorder
            'End If

            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)
            gv_SearchResult.DataSource = myDS

            If myDS.Tables(0).Rows.Count > 0 Then
                gv_SearchResult.DataBind()
            Else
                gv_SearchResult.PageIndex = 0
                gv_SearchResult.DataBind()

            End If

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ContractHotelConst.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
            clsDBConnect.dbConnectionClose(SqlConn)                          'Close connection
        End Try
    End Sub



    'Private Function ValidateSave() As Boolean
    '    Dim gvRow As GridViewRow


    '    Dim lnCnt As Integer = 0

    '    Dim txtchild As TextBox

    '    Dim ToDt As Date = Nothing
    '    Dim flgdt As Boolean = False

    '    For Each gvRow In gv_Filldata.Rows
    '        lnCnt += 1



    '        txtchild = gvRow.FindControl("txtchild")
    '        Dim txtadult As TextBox = gvRow.FindControl("txtadult")
    '        Dim txtPriceOccupancy As TextBox = gvRow.FindControl("txtMaxocctotal")
    '        Dim txtMaxOccpncy As TextBox = gvRow.FindControl("txtExsuppunit")
    '        Dim txtMaxOccupancycom As TextBox = gvRow.FindControl("txtMaxocccombination")
    '        Dim txtpricepax As TextBox = gvRow.FindControl("txtpricepax")
    '        Dim hdncombination As HiddenField = gvRow.FindControl("hdncombination")
    '        Dim txtrmtypename As TextBox = gvRow.FindControl("txtrmtypename")




    '        If txtadult.Text <> "" Then



    '            If Val(txtadult.Text) > 0 Then

    '                If (Val(txtPriceOccupancy.Text) > 0) = False Then
    '                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Price Occupancy cannot be 0 in line no :" & lnCnt & "');", True)
    '                    ValidateSave = False
    '                    Exit Function
    '                End If

    '                'If (Val(txtMaxOccpncy.Text) > 0) = False Then
    '                '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Max Occupancy should be entered in line no :" & lnCnt & "');", True)
    '                '    ValidateSave = False
    '                '    Exit Function
    '                'End If
    '                If txtMaxOccupancycom.Text = "" And hdncombination.Value = "" Then
    '                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Occupancy Combination not Selected  in line no :" & lnCnt & "');", True)
    '                    ValidateSave = False
    '                    Exit Function
    '                End If

    '                'If Val(txtchild.Text) > 0 Then

    '                '    If (hdnMaxOccupancy.Value = "") Then
    '                '        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Occupancy should be entered in line no :" & lnCnt & "');", True)
    '                '        ValidateSave = False
    '                '        Exit Function
    '                '    End If

    '                'End If
    '            End If
    '        End If

    '    Next




    '    ValidateSave = True
    'End Function
    Public Function checkforexisting() As Boolean

        checkforexisting = True
        Try
            If txtFromDate.Text = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Enter From Date.');", True)
                SetFocus(txtFromDate)
                checkforexisting = False
                Exit Function
            End If

            If txtToDate.Text = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Enter To Date.');", True)
                SetFocus(txtToDate)
                checkforexisting = False
                Exit Function
            End If

            If txtFromDate.Text <> "" And txtToDate.Text <> "" Then
                If Left(Right(txtFromDate.Text, 4), 2) <> "20" Or Left(Right(txtToDate.Text, 4), 2) <> "20" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Enter From Date and To Date Belongs to 21 st century  ');", True)
                    checkforexisting = False
                    SetFocus(txtFromDate)
                    Exit Function
                End If
            End If

            If FindDatePeriod() = False Then
                checkforexisting = False
                Exit Function
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Error Description: - " & ex.Message & "');", True)
            objUtils.WritErrorLog("ContractHotelConst.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try


    End Function

    Public Function FindDatePeriod() As Boolean



        FindDatePeriod = True
        Dim strMsg As String = ""
        Try
            Dim ds As DataSet
            Dim parms3 As New List(Of SqlParameter)
            Dim parm3(5) As SqlParameter
            parm3(0) = New SqlParameter("@partycode", CType(hdnpartycode.Value, String))
            parm3(1) = New SqlParameter("@mode", CType(ViewState("State"), String))
            parm3(2) = New SqlParameter("@tranid", CType(txttranid.Text.Trim, String))
            parm3(3) = New SqlParameter("@fromdate", Format(CType(txtFromDate.Text, Date), "yyyy/MM/dd"))
            parm3(4) = New SqlParameter("@todate", Format(CType(txtToDate.Text, Date), "yyyy/MM/dd"))

         

            parms3.Add(parm3(0))
            parms3.Add(parm3(1))
            parms3.Add(parm3(2))
            parms3.Add(parm3(3))
            parms3.Add(parm3(4))
           

            ds = New DataSet()
            ds = objUtils.ExecuteQuerynew(Session("dbconnectionName"), "sp_duplicate_hotelconstr", parms3)


            If ds.Tables.Count > 0 Then
                If ds.Tables(0).Rows.Count > 0 Then
                    If IsDBNull(ds.Tables(0).Rows(0)("constructionid")) = False Then
                        strMsg = "Hotel Construction already exists For this Supplier " + ds.Tables(0).Rows(0)("constructionid")
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & strMsg & "' );", True)
                        FindDatePeriod = False
                        Exit Function
                    End If
                End If
            End If


        Catch ex As Exception
            FindDatePeriod = False
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Error Description: - " & ex.Message & "');", True)
            objUtils.WritErrorLog("ContractHotelConst.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try


    End Function

#Region "Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs)"
    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click

     

        Dim strMsg As String = ""

        Try
            If Page.IsValid = True Then

                If ViewState("State") = "New" Or ViewState("State") = "Edit" Or ViewState("State") = "Copy" Then

                    
                    If checkforexisting() = False Then
                        Exit Sub
                    End If

                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start

                    If ViewState("State") = "New" Or ViewState("State") = "Copy" Then

                        Dim optionval As String
                        optionval = objUtils.GetAutoDocNo("HOTCONSTR", mySqlConn, sqlTrans)
                        txttranid.Text = optionval.Trim

                        mySqlCmd = New SqlCommand("sp_add_hotels_constructionnew", mySqlConn, sqlTrans)
                        mySqlCmd.CommandType = CommandType.StoredProcedure

                        mySqlCmd.Parameters.Add(New SqlParameter("@tranid", SqlDbType.VarChar, 20)).Value = CType(txttranid.Text.Trim, String)
                        mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(hdnpartycode.Value.Trim, String)
                      
                        mySqlCmd.Parameters.Add(New SqlParameter("@fromdate", SqlDbType.DateTime)).Value = ObjDate.ConvertDateromTextBoxToDatabase(txtFromDate.Text)
                        mySqlCmd.Parameters.Add(New SqlParameter("@todate", SqlDbType.DateTime)).Value = ObjDate.ConvertDateromTextBoxToDatabase(txtToDate.Text)
                        mySqlCmd.Parameters.Add(New SqlParameter("@Miscellaneous", SqlDbType.Text)).Value = CType(txtremarks.Value, String)
                        mySqlCmd.Parameters.Add(New SqlParameter("@reason", SqlDbType.Text)).Value = CType(txtReason.Value, String)
                        mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)

                        mySqlCmd.ExecuteNonQuery()
                        mySqlCmd.Dispose()                  'command disposed
                    ElseIf ViewState("State") = "Edit" Then
                        mySqlCmd = New SqlCommand("sp_mod_hotels_constructionnew", mySqlConn, sqlTrans)
                        mySqlCmd.CommandType = CommandType.StoredProcedure

                       mySqlCmd.Parameters.Add(New SqlParameter("@tranid", SqlDbType.VarChar, 20)).Value = CType(txttranid.Text.Trim, String)
                        mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(hdnpartycode.Value.Trim, String)

                        mySqlCmd.Parameters.Add(New SqlParameter("@fromdate", SqlDbType.DateTime)).Value = ObjDate.ConvertDateromTextBoxToDatabase(txtFromDate.Text)
                        mySqlCmd.Parameters.Add(New SqlParameter("@todate", SqlDbType.DateTime)).Value = ObjDate.ConvertDateromTextBoxToDatabase(txtToDate.Text)
                        mySqlCmd.Parameters.Add(New SqlParameter("@Miscellaneous", SqlDbType.Text)).Value = CType(txtremarks.Value, String)
                        mySqlCmd.Parameters.Add(New SqlParameter("@reason", SqlDbType.Text)).Value = CType(txtReason.Value, String)
                        mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)



                        mySqlCmd.ExecuteNonQuery()
                        mySqlCmd.Dispose()




                    End If

              
                    strMsg = "Saved Succesfully!!"

                ElseIf ViewState("State") = "Delete" Then


                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start
                    'delete for row tables present in sp
                    mySqlCmd = New SqlCommand("sp_del_hotels_constructionnew", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@tranid", SqlDbType.VarChar, 20)).Value = CType(txttranid.Text.Trim, String)
                    mySqlCmd.ExecuteNonQuery()
                    strMsg = "Delete  Succesfull!!"
                End If



                sqlTrans.Commit()    'SQl Tarn Commit
                clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
                clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed

                clsDBConnect.dbConnectionClose(mySqlConn)           'connection close

                ' Divmain.Style("display") = "none"

                ViewState("State") = ""
                btnreset1_Click(sender, e)
                FillGrid("constructionid", hdnpartycode.Value, "Desc")
                'ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & strMsg & " ', please click return to search and refresh the search page );", True)
                'Dim strscript As String = ""
                'strscript = "window.opener.__doPostBack('MaxaccWindowPostBack', '');window.opener.focus();window.close();"
                'ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)
            End If


        Catch ex As Exception
            If mySqlConn.State = ConnectionState.Open Then
                sqlTrans.Rollback()
            End If
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ContractHotelConst.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try

    End Sub

#End Region






    Protected Sub btnhelp_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        '   ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=SupMain','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub



#Region "Numberssrvctrl"
    Private Sub Numberssrvctrl(ByVal txtbox As WebControls.TextBox)
        txtbox.Attributes.Add("onkeypress", "return  checkNumber(event)")
    End Sub
#End Region

    



    
    Private Sub DisableControl()
        If ViewState("State") = "New" Then

            txtReason.Disabled = False
            txtremarks.Disabled = False
            txtFromDate.Enabled = True
            txtToDate.Enabled = True
            ImgBtnFrmDt.Enabled = True
            ImgBtnToDt.Enabled = True


            txtReason.Value = ""
            txtremarks.Value = ""
            txttranid.Text = ""
            txtFromDate.Text = ""
            txtToDate.Text = ""



        ElseIf ViewState("State") = "View" Or ViewState("State") = "Delete" Then


            txtReason.Disabled = True
            txtremarks.Disabled = True
            txtFromDate.Enabled = False
            txtToDate.Enabled = False
            ImgBtnFrmDt.Enabled = False
            ImgBtnToDt.Enabled = False


        ElseIf ViewState("State") = "Edit" Then

            txtReason.Disabled = False
            txtremarks.Disabled = False
            txtFromDate.Enabled = True
            txtToDate.Enabled = True
            ImgBtnFrmDt.Enabled = True
            ImgBtnToDt.Enabled = True

        End If
    End Sub
    Public Sub Numbers(ByVal txtbox As HtmlInputText)
        txtbox.Attributes.Add("onkeypress", "return checkNumber(event)")
        'txtbox.Attributes.Add("onkeypress", "return checkNumberDecimal(event,this)")
    End Sub

    Protected Sub gv_SearchResult_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gv_SearchResult.RowCommand
        If e.CommandName = "Page" Then Exit Sub
        If e.CommandName = "Sort" Then Exit Sub

        Dim lbltran As Label
        Session("tranid") = Nothing
        lbltran = gv_SearchResult.Rows(CType(e.CommandArgument.ToString, String)).FindControl("lbltran")



        If e.CommandName = "EditRow" Then

            ViewState("State") = "Edit"
            PanelMain.Visible = True
            PanelMain.Style("display") = "block"
            Panelsearch.Enabled = False
            Session.Add("RefCode", CType(lbltran.Text.Trim, String))
            Session.Add("tranid", CType(lbltran.Text.Trim, String))
            ShowRecord(CType(lbltran.Text.Trim, String))
            ' ShowRoomdetails(hdnpartycode.Value, CType(lbltran.Text.Trim, String))



            DisableControl()
            btnSave.Visible = True
            btnSave.Text = "Update"
            lblHeading.Text = "Edit Construction For - " + ViewState("hotelname")
            Page.Title = Page.Title + " " + " Construction "
        ElseIf e.CommandName = "View" Then
            ViewState("State") = "View"
            PanelMain.Visible = True
            PanelMain.Style("display") = "block"
            Panelsearch.Enabled = False
            Session.Add("RefCode", CType(lbltran.Text.Trim, String))
            Session.Add("tranid", CType(lbltran.Text.Trim, String))
            ShowRecord(CType(lbltran.Text.Trim, String))
            'ShowRoomdetails(hdnpartycode.Value, CType(lbltran.Text.Trim, String))



            DisableControl()
            btnSave.Visible = False
            lblHeading.Text = "View Construction For - " + ViewState("hotelname")
            Page.Title = Page.Title + " " + " Construction "
        ElseIf e.CommandName = "DeleteRow" Then
            PanelMain.Visible = True
            ViewState("State") = "Delete"
            PanelMain.Style("display") = "block"
            Panelsearch.Enabled = False
            Session.Add("RefCode", CType(lbltran.Text.Trim, String))
            Session.Add("tranid", CType(lbltran.Text.Trim, String))
            ShowRecord(CType(lbltran.Text.Trim, String))
            ' ShowRoomdetails(hdnpartycode.Value, CType(lbltran.Text.Trim, String))



            DisableControl()
            btnSave.Visible = True
            btnSave.Text = "Delete"
            lblHeading.Text = "Delete Construction For - " + ViewState("hotelname")
            Page.Title = Page.Title + " " + " Construction  "
        ElseIf e.CommandName = "Copy" Then
            PanelMain.Visible = True
            ViewState("State") = "Copy"
            PanelMain.Style("display") = "block"
            Panelsearch.Enabled = False
            Session.Add("RefCode", CType(lbltran.Text.Trim, String))
            Session.Add("tranid", CType(lbltran.Text.Trim, String))
            ShowRecord(CType(lbltran.Text.Trim, String))
            ' ShowRoomdetails(hdnpartycode.Value, CType(lbltran.Text.Trim, String))




            btnSave.Visible = True
            txttranid.Text = ""
            btnSave.Text = "Save"
            lblHeading.Text = "Copy Construction For - " + ViewState("hotelname")
            Page.Title = Page.Title + " " + " Construction "
        End If
    End Sub

    Private Sub fillheader()
        Try
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            mySqlCmd = New SqlCommand("Select c.applicableto,a.subseasname from contracts c(nolock),contracts_seasons a Where c.contractid=a.contractid and c.contractid='" & hdncontractid.Value & "'", mySqlConn)
            mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
            If mySqlReader.HasRows Then
                If mySqlReader.Read() = True Then
                    If IsDBNull(mySqlReader("applicableto")) = False Then
                        'txtApplicableTo.Text = mySqlReader("applicableto")


                    End If
                    'If IsDBNull(mySqlReader("subseasname")) = False Then
                    '    txtseasonname.Text = mySqlReader("subseasname")


                    'End If
                End If

                mySqlCmd.Dispose()
                mySqlReader.Close()
                mySqlConn.Close()
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ContractRoomrates.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            If mySqlConn.State = ConnectionState.Open Then mySqlConn.Close()
        End Try


    End Sub
    Protected Sub btnAddNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddNew.Click
        ViewState("State") = "New"
        PanelMain.Visible = True
           PanelMain.Style("display") = "block"
        Panelsearch.Enabled = False
        

        '  fillheader()

        btnSave.Visible = True
        DisableControl()
       

        btnSave.Text = "Save"
        lblHeading.Text = "New Construction  - " + ViewState("hotelname")
        Page.Title = Page.Title + " " + "New Construction  -" + ViewState("hotelname")

        
    End Sub

    Protected Sub btnreset1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnreset1.Click
        '   Divmain.Style("display") = "none"
        '  Panelsearch.Enabled = True


        'gv_Filldata.Visible = False


        Panelsearch.Enabled = True

        PanelMain.Style("display") = "none"
        'Panelsearch.Style("display")="block")
        lblHeading.Text = "Construction  -" + ViewState("hotelname")

    End Sub


    


  
 
  
End Class
