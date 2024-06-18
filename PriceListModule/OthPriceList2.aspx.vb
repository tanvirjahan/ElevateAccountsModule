#Region "NameSpace"
Imports System
Imports System.Data
Imports System.Data.SqlClient
#End Region

Partial Class PriceListModule_OthPriceList2
    Inherits System.Web.UI.Page

#Region "Global Declaration"
    Dim objUtils As New clsUtils
    Dim ObjDate As New clsDateTime
    Dim strSqlQry As String
    Dim SqlCmd As SqlCommand
    Dim SqlReader As SqlDataReader
    Dim sqlTrans As SqlTransaction
    Dim StrQry As String
    Dim SqlCon As SqlConnection
    Dim mySqlCmd As SqlCommand
    Dim mySqlReader As SqlDataReader
    Dim mySqlAdapter As SqlDataAdapter
    Dim mySqlConn As SqlConnection
    Dim ObjCon As New clsDBConnect
    Dim myDataAdapter As SqlDataAdapter
    Dim gvRow1 As GridViewRow
    Dim MyDS As New DataSet
    Dim Table As New DataTable()
    Dim ParameterArray As New ArrayList()
    Private dt As New DataTable
    Private cnt As Long
    Private arr(1) As String
    Private arrRName(1) As String
    Dim GvRow As String

    Dim dpFDate As New TextBox
    Dim dpTDate As New TextBox
#End Region

#Region "TextLock"
    Public Sub TextLock(ByVal txtbox As TextBox)
        txtbox.Attributes.Add("onKeyPress", "return chkTextLock(event)")
        txtbox.Attributes.Add("onKeyDown", "return chkTextLock1(event)")
        txtbox.Attributes.Add("onKeyUp", "return chkTextLock(event)")
    End Sub
#End Region

#Region "Numbers"
    Public Sub Numbers(ByVal txtbox As TextBox)
        Try
            txtbox.Attributes.Add("onkeypress", "return checkNumber(event)")
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        End Try
    End Sub
#End Region

#Region "NumbersInt"
    Public Sub NumbersInt(ByVal txtbox As TextBox)
        Try
            txtbox.Attributes.Add("onkeypress", "return checkNumber1(event)")
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        End Try
    End Sub
#End Region

#Region "NumbersDateInt"
    Public Sub NumbersDateInt(ByVal txtbox As TextBox)
        Try
            txtbox.Attributes.Add("onkeypress", "return checkNumber2(event)")
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        End Try
    End Sub
#End Region

#Region "HTMLNumbers"
    Public Sub HTMLNumbers(ByVal txtbox As HtmlInputText)
        Try
            txtbox.Attributes.Add("onkeypress", "return checkNumber(event)")
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & ex.Message & " ');", True)
        End Try
    End Sub
#End Region

#Region "Characters"
    Public Sub Characters(ByVal txtbox As HtmlInputText)
        Try
            txtbox.Attributes.Add("onkeypress", "return checkCharacter(event)")
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & ex.Message & " ');", True)
        End Try
    End Sub
#End Region


#Region "Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init"
    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Try
            Dim strqry As String = ""
            Dim strOption As String = ""
            Dim strtitle As String = ""
            Dim strSPType As String = ""

            If (Session("OthPListFilter") <> Nothing And Session("OthPListFilter") <> "OTH") Then

                strOption = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", Session("OthPListFilter"))
                Select Case strOption
                    Case "CAR RENTAL"
                        strtitle = "Car Rental"
                        strSPType = "1031,1039"
                        'lblHeading.Text = "Car Rental Selling Formula"
                    Case "VISA"
                        strtitle = "Visa "
                        strSPType = "1032,1039"
                        'lblHeading.Text = "Visa Selling Formula"
                    Case "EXC"
                        strtitle = "Excursion  "
                        strSPType = "1033,1039"
                        'lblHeading.Text = "Excursion Selling Formula"
                    Case "MEALS"
                        strtitle = "Restaurant  "
                        strSPType = "1034,1039"
                        'lblHeading.Text = "Restaurant Selling Formula"
                    Case "GUIDES"
                        strtitle = "Guide  "
                        strSPType = "1035,1039"
                        'lblHeading.Text = "Guide Selling Formula"
                    Case "ENTRANCE"
                        strtitle = "Entrance "
                        strSPType = "1036,1039"
                        'lblHeading.Text = "Guide Selling Formula"
                    Case "JEEPWADI"
                        strtitle = "Jeepwadi "
                        strSPType = "1037,1039"
                        'lblHeading.Text = "Guide Selling Formula"
                    Case "HFEES"
                        strtitle = "Handling Fee "
                        strSPType = "1041,1039"
                    Case "AIRPORTMA"
                        strtitle = "Airport Meet & Assist"
                        strSPType = "1028,1039"
                End Select
                strqry = "select othgrpcode,othgrpname from othgrpmast where active=1 And othgrpcode=(Select Option_Selected From Reservation_ParaMeters" & _
                    " Where Param_Id='" & Session("OthPListFilter") & "') order by othgrpcode"

            ElseIf Session("OthPListFilter") = "OTH" Then
                strtitle = "Other Service "
                strqry = "select othgrpcode,othgrpname from othgrpmast,othmaingrpmast where othgrpmast.active=1 and othgrpmast.othgrpcode=othmaingrpmast.othmaingrpcode And othgrpmast.othmaingrpcode not in (Select Option_Selected From Reservation_ParaMeters" & _
                    " Where Param_Id  in  (1001,1002,1105,1021,1027,1028)) order by othgrpcode"
            End If

            If Request.QueryString("State") = "New" Then
                Page.Title = Page.Title + " " + "New " + strtitle + " Price List"
                lblHeading.Text = "Add New " + strtitle + " Price List"
            ElseIf Request.QueryString("State") = "Copy" Then
                Page.Title = Page.Title + " " + "Copy " + strtitle + " Price List"

                lblHeading.Text = "Copy " + strtitle + " Price List"
            ElseIf Request.QueryString("State") = "Edit" Then
                Page.Title = Page.Title + " " + "Edit " + strtitle + " Price List"
                lblHeading.Text = "Edit " + strtitle + " Price List"
            ElseIf Request.QueryString("State") = "View" Then
                Page.Title = Page.Title + " " + "View " + strtitle + " Price List"
                lblHeading.Text = "View " + strtitle + " Price List"
            ElseIf Request.QueryString("State") = "Delete" Then
                Page.Title = Page.Title + " " + "Delete " + strtitle + " Price List"
                lblHeading.Text = "Delete " + strtitle + " Price List"
            End If


            Dim s As String = ""
            ViewState.Add("OthpricelistState2", Request.QueryString("State"))
            ViewState.Add("OthpricelistRefCode2", Request.QueryString("RefCode"))
            txtconnection.Value = Session("dbconnectionName")
            If IsPostBack = False Then
                Dim SPType As String = ""
                Session("PlistSaved") = False
                If Session("OthPListFilter") <> "OTH" Then
                    SPType = "select sptypecode,sptypename from sptypemast where active=1 and sptypecode in (select option_selected  from " & _
                                            " reservation_parameters where param_id in (" + strSPType + "))  order by sptypecode"
                Else
                    SPType = "select sptypecode,sptypename from sptypemast where active=1 and sptypecode not in (select option_selected  from " & _
                                           " reservation_parameters where param_id  in (564,1031,1032,1033,1034,1035,1036,1037,1041))  order by sptypecode"

                End If
                Dim default_group As String
                default_group = ""
                default_group = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", CType("1109", String))
                If strOption <> "VISA" Then
                    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlOtherSellingTypeCode, "othsellcode", "othsellname", "select othsellcode,othsellname from othsellmast where active=1 and othertype='" & default_group & "' order by othsellcode", True)
                    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlOtherSellingTypeName, "othsellname", "othsellcode", "select othsellname,othsellcode from othsellmast where active=1 and othertype='" & default_group & "' order by othsellname", True)

                Else

                    lblsellingtypeName.Text = "Visa Selling Type Code"

                    lblSellingTypeCode.Text = "Visa Selling Type Name"

                    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlOtherSellingTypeCode, "visasellcode", "visasellname", "select visasellcode,visasellname from visasellmast where active=1  order by visasellcode", True)
                    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlOtherSellingTypeName, "visasellname", "visasellcode", "select visasellname,visasellcode from visasellmast where active=1  order by visasellname", True)

                End If


                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlGroupCode, "othgrpcode", "othgrpname", strqry, True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlGroupName, "othgrpname", "othgrpcode", strqry, True)


                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSubSeasCode, "subseascode", "subseasname", "select subseascode,subseasname from subseasmast where active=1 order by subseascode", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSubSeasName, "subseasname", "subseascode", "select subseasname,subseascode from subseasmast where active=1 order by subseasname", True)


                btnCancel.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to exit?')==false)return false;")


                ddlGroupCode.SelectedIndex = 0
                If ViewState("OthpricelistState2") = "New" Or ViewState("OthpricelistState2") = "Edit" Or ViewState("OthpricelistState2") = "Copy" Then

                    btnSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to save price list?')==false)return false;")

                    'If ViewState("TrfpricelistState2") <> "Edit" Then
                    'btnSave.Attributes.Add("onclick", "javascript:if(confirmMarkUp()==false) return false;")
                    'End If
                    Dim obj As New EncryptionDecryption
                    gv_SearchResult.Visible = True
                    BtnClearFormula.Visible = True



                    If ViewState("OthpricelistState2") = "Edit" Then
                        btnSave.Text = "Update"
                    End If



                    If Request.QueryString("OthSellCode") <> Nothing And Request.QueryString("OthSellName") <> Nothing Then
                        s = ""
                        s = Request.QueryString("OthSellCode")
                        ddlOtherSellingTypeName.Value = obj.Decrypt(s.Replace(" ", "+"), "&%#@?,:*")
                        s = ""
                        s = Request.QueryString("OthSellName")
                        ddlOtherSellingTypeCode.Value = obj.Decrypt(s.Replace(" ", "+"), "&%#@?,:*")
                    End If






                    If Request.QueryString("GroupCode") <> Nothing And Request.QueryString("GroupName") <> Nothing Then
                        s = ""
                        s = Request.QueryString("GroupName")
                        ddlGroupCode.Value = obj.Decrypt(s.Replace(" ", "+"), "&%#@?,:*")
                        s = ""
                        s = Request.QueryString("GroupCode")
                        ddlGroupName.Value = obj.Decrypt(s.Replace(" ", "+"), "&%#@?,:*")
                    End If

                    If Request.QueryString("SubSeasCode") <> Nothing And Request.QueryString("SubSeasName") <> Nothing Then
                        s = ""
                        s = Request.QueryString("SubSeasName")
                        ddlSubSeasCode.Value = obj.Decrypt(s.Replace(" ", "+"), "&%#@?,:*")
                        s = ""
                        s = Request.QueryString("SubSeasCode")
                        ddlSubSeasName.Value = obj.Decrypt(s.Replace(" ", "+"), "&%#@?,:*")
                    End If


                    txtRemark.Value = CType(Session("sessionRemark"), String)
                    If Request.QueryString("PlcCode") <> Nothing Then
                        s = ""
                        s = Request.QueryString("PlcCode")
                        txtPlcCode.Value = obj.Decrypt(s.Replace(" ", "+"), "&%#@?,:*")
                    End If
                    If Request.QueryString("currcode") <> Nothing Then
                        s = ""
                        s = Request.QueryString("currcode")
                        txtCurrCode.Text = obj.Decrypt(s.Replace(" ", "+"), "&%#@?,:*")
                    End If
                    If Request.QueryString("currname") <> Nothing Then
                        s = ""
                        s = Request.QueryString("currname")
                        txtCurrName.Text = obj.Decrypt(s.Replace(" ", "+"), "&%#@?,:*")
                    End If

                    If Request.QueryString("Acvtive") <> Nothing Then
                        If Request.QueryString("Acvtive") = "1" Then
                            ChkActive.Checked = True
                        ElseIf Request.QueryString("Acvtive") = "0" Then
                            ChkActive.Checked = False
                        End If
                    End If

                    If ViewState("OthpricelistState2") <> "Copy" Then
                        If Request.QueryString("approved") <> Nothing Then
                            If Request.QueryString("approved") = "1" Then
                                chkapprove.Checked = True
                            ElseIf Request.QueryString("approved") = "0" Then
                                chkapprove.Checked = False
                            End If
                        End If
                    End If

                    If Request.QueryString("conMarkUp") <> Nothing Then
                        If Request.QueryString("conMarkUp") = "1" Then
                            chkConsdierForMarkUp.Checked = True
                        ElseIf Request.QueryString("conMarkUp") = "0" Then
                            chkConsdierForMarkUp.Checked = False
                        End If
                    End If



                ElseIf ViewState("OthpricelistState2") = "View" Then

                    ShowRecord(ViewState("OthpricelistRefCode2"))



                ElseIf ViewState("OthpricelistState2") = "Delete" Then
                    'lblHeading.Text = "Delete " + strtitle + " Price List"
                    btnSave.Text = "Delete"
                    ShowRecord(ViewState("OthpricelistRefCode2"))

                    btnSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to delete  price list?')==false)return false;")
                End If

                TextLock(txtCurrCode)
                TextLock(txtCurrName)
                DisableAllControls()

                BtnClearFormula.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to clear prices?')==false)return false;")
                createdatatable()

                'fill controls from previous form
                ' Now  Bind Column Dynamically 

                Dim fld2 As String = ""
                Dim col As DataColumn
                For Each col In dt.Columns
                    If col.ColumnName <> "Sr No" And col.ColumnName <> "Service Type Code" And col.ColumnName <> "Service Type Name" Then
                        Dim bfield As New TemplateField
                        'Call Function
                        bfield.HeaderTemplate = New ClsOtherServicePriceList(ListItemType.Header, col.ColumnName, fld2)
                        bfield.ItemTemplate = New ClsOtherServicePriceList(ListItemType.Item, col.ColumnName, fld2)
                        gv_SearchResult.Columns.Add(bfield)

                    End If

                Next
                gv_SearchResult.Visible = True
                gv_SearchResult.DataSource = dt
                'InstantiateIn Grid View
                gv_SearchResult.DataBind()


                'Dim header As Long = 0
                'Dim heading(cnt + 1) As String
                'For header = 0 To cnt - 5
                '    txt1 = gv_SearchResult.HeaderRow.FindControl("txtHead" & header)
                '    heading(header) = txt1.Text
                'Next


            Else
                'dt = Session("GV_HotelData")
                dt = Session("GV_OthPLData")

                ' createdatatable()

                'fill controls from previous form
                ' Now  Bind Column Dynamically 

                Dim fld2 As String = ""
                Dim col As DataColumn
                For Each col In dt.Columns
                    If col.ColumnName <> "Sr No" And col.ColumnName <> "Service Type Code" And col.ColumnName <> "Service Type Name" Then
                        Dim bfield As New TemplateField
                        'Call Function
                        bfield.HeaderTemplate = New ClsOtherServicePriceList(ListItemType.Header, col.ColumnName, fld2)
                        bfield.ItemTemplate = New ClsOtherServicePriceList(ListItemType.Item, col.ColumnName, fld2)
                        gv_SearchResult.Columns.Add(bfield)

                    End If

                Next




                gv_SearchResult.Visible = True
                gv_SearchResult.DataSource = dt
                'InstantiateIn Grid View
                gv_SearchResult.DataBind()


                BtnClearFormula.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to Clear formula?')==false)return false;")
                btnCancel.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to cancel?')==false)return false;")


            End If


        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & ex.Message & " ');", True)
            objUtils.WritErrorLog("OthPriceList1.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#End Region

#Region "Public Function checkIsPrivilege() As Boolean"
    Public Sub checkIsPrivilege()
        Try
            Dim strSql As String
            Dim usrCode As String
            usrCode = CType(Session("GlobalUserName"), String)
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            strSql = "select appid from group_privilege_Detail where privilegeid='8' and appid='1' and "
            strSql += "groupid=(SELECT groupid FROM UserMaster WHERE UserCode='" + usrCode + "')"
            mySqlCmd = New SqlCommand(strSql, mySqlConn)
            mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
            If mySqlReader.HasRows Then
                Session.Add("Showapprove", "Yes")
            Else
                Session.Add("Showapprove", "No")
            End If

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("OthPriceList1.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
            clsDBConnect.dbReaderClose(mySqlReader)             ' sql reader disposed    
            clsDBConnect.dbConnectionClose(mySqlConn)           'connection close 
        End Try

    End Sub
#End Region

#Region "Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack = False Then
            Try

                checkIsPrivilege()
                If Session("Showapprove") <> "Yes" Then
                    chkapprove.Visible = False
                End If

                ViewState.Add("OthpricelistState2", Request.QueryString("State"))
                ViewState.Add("OthpricelistRefCode2", Request.QueryString("RefCode"))

                fillDategrd(grdDates, True)


                If Session("CheckGridColumn") = "Not Present" Then
                    Panel1.Visible = False
                    lblError.Visible = True
                    lblError.Text = "Rates are not present of this selected Group Code. Click Clear Prices for Changing Group."
                Else
                    Panel1.Visible = True
                    createdatarows()
                    lblError.Visible = False
                End If
                DisableGrid()

                If ViewState("OthpricelistState2") = "View" Then
                    'lblHeading.Text = "View Transfer Price List"
                    'ShowRecord(ViewState("TrfpricelistRefCode2"))

                    ShowDates(CType(ViewState("OthpricelistRefCode2"), String))
                    'ShowSellingRecord(CType(ViewState("OthpricelistRefCode2"), String))
                End If
                If ViewState("OthpricelistState2") = "Edit" Then
                    'lblHeading.Text = "View Transfer Price List"
                    'ShowRecord(ViewState("TrfpricelistRefCode2"))

                    ShowDates(CType(ViewState("OthpricelistRefCode2"), String))
                    'ShowSellingRecord(CType(ViewState("OthpricelistRefCode2"), String))
                End If
                If ViewState("OthpricelistState2") = "Delete" Then
                    'lblHeading.Text = "View Transfer Price List"
                    'ShowRecord(ViewState("TrfpricelistRefCode2"))

                    ShowDates(CType(ViewState("OthpricelistRefCode2"), String))
                    'ShowSellingRecord(CType(ViewState("OthpricelistRefCode2"), String))
                End If

                If ViewState("OthpricelistState2") = "Copy" Then
                    ShowDates(CType(ViewState("OthpricelistRefCode2"), String))
                End If


            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & ex.Message & " ');", True)
                objUtils.WritErrorLog("OthPricesList1.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            End Try
        End If
    End Sub
#End Region

#Region "Private Sub ShowRecord(ByVal RefCode As String)"
    Private Sub ShowRecord(ByVal RefCode As String)
        Try
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            mySqlCmd = New SqlCommand("Select * from othplisth Where oplistcode='" & RefCode & "'", mySqlConn)
            mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
            If mySqlReader.HasRows Then
                If mySqlReader.Read() = True Then
                    If IsDBNull(mySqlReader("oplistcode")) = False Then
                        txtPlcCode.Value = mySqlReader("oplistcode")
                    End If


                    If IsDBNull(mySqlReader("othgrpcode")) = False Then
                        ddlGroupName.Value = mySqlReader("othgrpcode")
                        ddlGroupCode.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "othgrpmast", "othgrpname", "othgrpcode", mySqlReader("othgrpcode"))
                    End If

                    If IsDBNull(mySqlReader("othsellcode")) = False Then
                        ddlOtherSellingTypeName.Value = mySqlReader("othsellcode")
                        ' ddlOtherSellingTypeCode.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "othsellmast", "othsellname", "othsellcode", mySqlReader("othsellcode"))
                        ddlOtherSellingTypeCode.Value = ddlOtherSellingTypeName.Items(ddlOtherSellingTypeName.SelectedIndex).Text

                    End If


                    If IsDBNull(mySqlReader("currcode")) = False Then
                        txtCurrCode.Text = mySqlReader("currcode")
                        txtCurrName.Text = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "currmast", "currname", "currcode", mySqlReader("currcode"))
                    End If



                    If IsDBNull(mySqlReader("subseascode")) = False Then
                        ddlSubSeasName.Value = mySqlReader("subseascode")
                        ddlSubSeasCode.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "subseasmast", "subseasname", "subseascode", mySqlReader("subseascode"))
                    End If
                    If IsDBNull(mySqlReader("remarks")) = False Then
                        txtRemark.Value = mySqlReader("remarks")
                    End If

                    If IsDBNull(mySqlReader("active")) = False Then
                        If mySqlReader("active") = "1" Then
                            ChkActive.Checked = True
                        ElseIf mySqlReader("active") = "0" Then
                            ChkActive.Checked = False
                        End If
                    End If

                    If IsDBNull(mySqlReader("approve")) = False Then
                        If mySqlReader("approve") = 1 Then
                            chkapprove.Checked = True
                        ElseIf mySqlReader("approve") = 0 Then
                            chkapprove.Checked = False
                        End If
                    End If

                    If IsDBNull(mySqlReader("considerformarkup")) = False Then
                        If mySqlReader("considerformarkup") = 1 Then
                            chkConsdierForMarkUp.Checked = True
                        ElseIf mySqlReader("considerformarkup") = 0 Then
                            chkConsdierForMarkUp.Checked = False
                        End If
                    End If

                End If
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & ex.Message & " ');", True)
            objUtils.WritErrorLog("OthPriceList1.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            clsDBConnect.dbCommandClose(mySqlCmd)
            clsDBConnect.dbReaderClose(mySqlReader)
            clsDBConnect.dbConnectionClose(mySqlConn)
        End Try
    End Sub
#End Region


#Region " Private Sub ShowDates(ByVal RefCode As String)"
    Private Sub ShowDates(ByVal RefCode As String)
        Try
            Dim gvRow As GridViewRow
            'Dim dpFDate As EclipseWebSolutions.DatePicker.DatePicker
            'Dim dpTDate As EclipseWebSolutions.DatePicker.DatePicker
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            mySqlCmd = New SqlCommand("Select * from othplisth_dates Where oplistcode='" & RefCode & "'", mySqlConn)
            mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
            If mySqlReader.HasRows Then
                While mySqlReader.Read()
                    For Each gvRow In grdDates.Rows
                        dpFDate = gvRow.FindControl("txtfromDate")
                        dpTDate = gvRow.FindControl("txtToDate")
                        If dpFDate.Text = "" And dpFDate.Text = "" Then
                            If IsDBNull(mySqlReader("frmdate")) = False Then
                                dpFDate.Text = Format("U", mySqlReader("frmdate")) 'CType(ObjDate.ConvertDateFromDatabaseToTextBoxFormat(mySqlReader("frmdate")), String)
                            End If
                            If IsDBNull(mySqlReader("todate")) = False Then
                                dpTDate.Text = Format("U", mySqlReader("todate")) 'CType(ObjDate.ConvertDateFromDatabaseToTextBoxFormat(mySqlReader("todate")), String)
                            End If
                            Exit For
                        End If
                    Next
                End While
            End If

        Catch ex As Exception
            objUtils.WritErrorLog("OthPriceList1.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
            clsDBConnect.dbReaderClose(mySqlReader)             ' sql reader disposed    
            clsDBConnect.dbConnectionClose(mySqlConn)           'connection close           
        End Try
    End Sub
#End Region


#Region "Private Sub DisableAllControls()"
    Private Sub DisableAllControls()
        btnselling.Visible = False
        If ViewState("OthpricelistState2") = "New" Then


            ddlGroupCode.Disabled = True
            ddlGroupName.Disabled = True
            ddlOtherSellingTypeCode.Disabled = True
            ddlOtherSellingTypeName.Disabled = True
            ddlSubSeasCode.Disabled = True
            ddlSubSeasName.Disabled = True
            txtRemark.Disabled = True
            ChkActive.Disabled = True

            txtCurrCode.Enabled = False
            txtCurrName.Enabled = False
            ' gv_Market.Enabled = False

        ElseIf ViewState("OthpricelistState2") = "Edit" Or ViewState("OthpricelistState2") = "Copy" Then


            ddlGroupCode.Disabled = True
            ddlGroupName.Disabled = True

            ddlSubSeasCode.Disabled = True
            ddlSubSeasName.Disabled = True
            txtRemark.Disabled = True
            ChkActive.Disabled = True
            ddlOtherSellingTypeCode.Disabled = True
            ddlOtherSellingTypeName.Disabled = True
            txtCurrCode.Enabled = False
            txtCurrName.Enabled = False

            If ViewState("OthpricelistState2") = "Edit" Then
                If chkConsdierForMarkUp.Checked = True Then
                    chkConsdierForMarkUp.Enabled = False
                Else
                    chkConsdierForMarkUp.Enabled = True
                End If
            End If
        ElseIf ViewState("OthpricelistState2") = "Delete" Or ViewState("OthpricelistState2") = "View" Then

            ddlGroupCode.Disabled = True
            ddlGroupName.Disabled = True
            ddlOtherSellingTypeCode.Disabled = True
            ddlOtherSellingTypeName.Disabled = True
            ddlSubSeasCode.Disabled = True
            ddlSubSeasName.Disabled = True
            txtRemark.Disabled = True
            ChkActive.Disabled = True

            txtCurrCode.Enabled = False
            txtCurrCode.Enabled = False
            gv_SearchResult.Enabled = False
            grdDates.Enabled = False

            btnAddLines.Visible = False
            BtnClearFormula.Visible = False
            btnSave.Visible = False

            chkapprove.Enabled = False
            chkConsdierForMarkUp.Enabled = False
            btnselling.Visible = True
            btnselling.Enabled = False
            btnPreviousRates.Enabled = False
            txtCurrName.Enabled = False

        End If
        If ViewState("OthpricelistState2") = "Delete" Then
            btnSave.Visible = True
        End If


    End Sub
#End Region



#Region "Private Sub DisableGrid()"
    Private Sub DisableGrid()
        Dim i As Long
        Dim k As Long = 0
        Dim j As Long = 1
        Dim txt As TextBox
        Dim gvrow As GridViewRow
        Dim n As Long = 0
        Dim m As Long = 0
        Try
            If ViewState("OthpricelistState2") = "View" Or ViewState("OthpricelistState2") = "Delete" Then
                j = 0
                cnt = gv_SearchResult.Columns.Count
                i = 0
                k = 0
                For Each gvrow In gv_SearchResult.Rows
                    If n = 0 Then
                        For i = 0 To cnt - 5
                            txt = gvrow.FindControl("txt" & i)
                            txt.Enabled = False
                            TextLock(txt)
                        Next
                        m = i
                    Else
                        k = 0
                        For i = n To (m + n) - 1
                            txt = gvrow.FindControl("txt" & i)
                            txt.Enabled = False
                            TextLock(txt)
                            k = k + 1
                        Next

                    End If
                    n = i
                Next

            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        End Try
    End Sub
#End Region

#Region "Private Sub createdatatable()"
    Private Sub createdatatable()
        Try

            cnt = 0
            strSqlQry = "SELECT  count(dbo.othcatmast.othcatcode) FROM dbo.othcatmast  WHERE (dbo.othcatmast.othgrpcode = '" & ddlGroupCode.Items(ddlGroupCode.SelectedIndex).Text & "') and othcatmast.active=1 "
            cnt = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), strSqlQry)
            If cnt <= 0 Then
                Session("CheckGridColumn") = "Not Present"
            Else
                Session("CheckGridColumn") = ""
            End If

            ReDim arr(cnt + 1)
            ReDim arrRName(cnt + 1)
            Dim i As Long = 0
            Dim Column As Long = 0

            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))

            strSqlQry = "SELECT  distinct dbo.othcatmast.othcatcode, dbo.othcatmast.othgrpcode,othcatmast.grporder FROM  dbo.othcatmast  WHERE (dbo.othcatmast.othgrpcode = '" & ddlGroupCode.Items(ddlGroupCode.SelectedIndex).Text & "')  and othcatmast.active=1 Order by othcatmast.grporder"

            mySqlCmd = New SqlCommand(strSqlQry, mySqlConn)
            mySqlReader = mySqlCmd.ExecuteReader()
            While mySqlReader.Read = True
                arr(Column) = mySqlReader("othcatcode")
                Column = Column + 1
            End While


            'Here in Array store room types
            '-------------------------------------
            Dim tf As New TemplateField
            dt.Columns.Add(New DataColumn("Sr No", GetType(String)))
            dt.Columns.Add(New DataColumn("Service Type Code", GetType(String)))
            dt.Columns.Add(New DataColumn("Service Type Name", GetType(String)))

            'create columns of this room types in data table
            For i = 0 To Column - 1
                dt.Columns.Add(New DataColumn(arr(i), GetType(String)))
            Next

            dt.Columns.Add(New DataColumn("Pkg", GetType(String)))

            Session("GV_OthPLData") = dt


        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("OthPricesList1.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            clsDBConnect.dbCommandClose(mySqlCmd)
            clsDBConnect.dbReaderClose(mySqlReader)
            clsDBConnect.dbConnectionClose(mySqlConn)
        End Try
    End Sub
#End Region

#Region "Private Sub createdatarows()"
    Private Sub createdatarows()
        Dim i As Long
        Dim k As Long = 0
        Try

            strSqlQry = "SELECT  count(othtypmast.othtypcode) FROM othtypmast  WHERE (othtypmast.othgrpcode = '" & ddlGroupCode.Items(ddlGroupCode.SelectedIndex).Text & "')  and othtypmast.active=1"
            cnt = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), strSqlQry)

            Dim arr_rnkorder(cnt + 1) As String
            Dim arr_rows(cnt + 1) As String
            Dim arr_rname(cnt + 1) As String

            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
            strSqlQry = "SELECT distinct othtypmast.othtypcode, othtypmast.othtypname,othtypmast.othgrpcode,rankorder FROM othtypmast  WHERE (othtypmast.othgrpcode = '" & ddlGroupCode.Items(ddlGroupCode.SelectedIndex).Text & "')  and othtypmast.active=1 order by  rankorder"
            mySqlCmd = New SqlCommand(strSqlQry, mySqlConn)
            mySqlReader = mySqlCmd.ExecuteReader()
            While mySqlReader.Read = True
                arr_rnkorder(k) = mySqlReader("rankorder")
                arr_rows(k) = mySqlReader("othtypcode")
                arr_rname(k) = mySqlReader("othtypname")
                k = k + 1
            End While


            'Here add rows.....
            'Now Get Rows From sellmast Based on SPType
            'Session("GV_HotelData") = dt
            Session("GV_OthPLData") = dt
            Dim dr As DataRow

            Dim row As Long

            For row = 0 To k - 1
                dr = dt.NewRow
                'For i = 1 To cnt - 1

                ' dr("Sr No") = row + 1   ' 
                ''taken from the rankorder instead of sno due to show the rank order in the pricelists      
                dr("Sr No") = row + 1 'arr_rnkorder(row)
                dr("Service Type Code") = arr_rows(row)
                dr("Service Type Name") = arr_rname(row)

                dr("Pkg") = 1
                'Next
                dt.Rows.Add(dr)
            Next


            gv_SearchResult.Visible = True
            gv_SearchResult.DataSource = dt
            'InstantiateIn Grid View
            gv_SearchResult.DataBind()

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("OthPriceList1.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            clsDBConnect.dbCommandClose(mySqlCmd)
            clsDBConnect.dbReaderClose(mySqlReader)
            clsDBConnect.dbConnectionClose(mySqlConn)
        End Try
        '-======================||||||||||||||||||||||||||||||||||||||||==========================!!!!!!!!!!!!!!!
        ''''Herer Only Numbers Can enterd in textboxes
        Dim j As Long = 1
        Dim txt As TextBox
        Dim gvrow As GridViewRow
        j = 0
        cnt = gv_SearchResult.Columns.Count
        Dim n As Long = 0
        Dim m As Long = 0
        i = 0
        k = 0
        Try
            For Each gvrow In gv_SearchResult.Rows
                If n = 0 Then
                    For i = 0 To cnt - 5
                        txt = gvrow.FindControl("txt" & i)
                        Numbers(txt)
                        txt.CssClass = "field_input"
                        txt.Width = 60
                    Next
                    m = i
                Else
                    k = 0
                    For i = n To (m + n) - 1
                        txt = gvrow.FindControl("txt" & i)
                        Numbers(txt)
                        txt.CssClass = "field_input"
                        txt.Width = 60
                        k = k + 1
                    Next

                End If
                n = i


            Next

            ''''Herer Only Interger Numbers Can enterd in textboxes from and todate


            Dim header As Long = 0
            cnt = gv_SearchResult.Columns.Count
            Dim heading(cnt + 1) As String
            '----------------------------------------------------------------------------
            '           Stoaring heading column values in the array

            For header = 0 To cnt - 5
                txt = gv_SearchResult.HeaderRow.FindControl("txtHead" & header)
                heading(header) = txt.Text
                If txt.Text = "From Date" Or txt.Text = "To Date" Then
                    txt.Width = 60
                ElseIf txt.Text = "Pkg" Then
                    gv_SearchResult.Columns(cnt - 1).Visible = False
                    '  txt.Width = 30
                Else
                    'If Len(txt.Text) < 20 Then
                    '    txt.Columns = 0
                    '    txt.Width = 100
                    'Else
                    txt.Columns = Len(txt.Text)
                    'txt.Width = Len(txt.Text) * 10
                    ' End If
                End If
                txt.CssClass = "field_input"
            Next

            Dim a, b As Long


            a = cnt - 7
            j = 0
            b = 0
            m = 0
            n = 0


            For Each gvrow In gv_SearchResult.Rows
                If n = 0 Then
                    For j = 0 To cnt - 5
                        If heading(j) = "From Date" Or heading(j) = "To Date" Or heading(j) = "Pkg" Then

                        Else
                            'txt = gvrow.FindControl("txt" & b + a + 1)
                            'NumbersDateInt(txt)
                            'txt.CssClass = "field_input"
                            'txt.Width = 60
                            txt = gvrow.FindControl("txt" & b + a + 2)
                            'NumbersDateInt(txt)
                            NumbersInt(txt)
                            txt.CssClass = "field_input"
                            txt.Width = 60
                            'txt = gvrow.FindControl("txt" & b + a + 3)
                            'NumbersInt(txt)
                            'txt.CssClass = "field_input"
                            'txt.Width = 30
                        End If
                    Next

                    m = j
                Else
                    k = 0
                    For j = n To (m + n) - 1
                        If heading(k) = "From Date" Or heading(k) = "To Date" Or heading(k) = "Pkg" Then
                        Else
                            'txt = gvrow.FindControl("txt" & b + a + 1)
                            'NumbersDateInt(txt)
                            'txt.CssClass = "field_input"
                            'txt.Width = 60
                            txt = gvrow.FindControl("txt" & b + a + 2)
                            NumbersInt(txt) 'NumbersDateInt(txt)
                            txt.CssClass = "field_input"
                            txt.Width = 60
                            'txt = gvrow.FindControl("txt" & b + a + 3)
                            'NumbersInt(txt)
                            'txt.CssClass = "field_input"
                            'txt.Width = 30
                            k = k + 1
                        End If
                    Next
                End If
                b = j
                n = j
            Next

            If ViewState("OthpricelistState2") <> "New" Then
                ShowDynamicGrid(ViewState("OthpricelistRefCode2"))
            End If

        Catch ex As Exception
            objUtils.WritErrorLog("OthPriceList1.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try

    End Sub
#End Region

#Region "Private Sub ShowDynamicGrid()"

    Private Sub ShowDynamicGrid(ByVal RefCode As String)
        Try
            cnt = 0
            Dim StrQry As String = ""

            '--------------------------------------------------------
            ' Show Records From Details Table 
            Dim GVROW As GridViewRow
            cnt = gv_SearchResult.Columns.Count
            Dim i As Long = 0
            'Dim n As Long = 0
            'Dim m As Long = 0
            Dim txt1 As TextBox
            Dim header As Long = 0
            Dim heading(cnt + 1) As String

            Dim othcatcode As String
            Dim value As String

            Dim Linno As Integer
            Dim StrQryTemp As String = ""
            Dim myConn As New SqlConnection
            Dim myCmd As New SqlCommand
            Dim myReader As SqlDataReader
            Dim j As Long = 0
            Dim s As Long

            Try
                For header = 0 To cnt - 5
                    txt1 = gv_SearchResult.HeaderRow.FindControl("txtHead" & header)
                    heading(header) = txt1.Text
                Next
            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            End Try


            'StrQry = "select oplist_costd.ocplistcode,oplist_costd.plgrpcode,oplist_costd.othgrpcode,othtypmast.rankorder oclineno,oplist_costd.othtypcode, " & _
            '         "oplist_costd.othcatcode,oplist_costd.ocostprice,oplist_costd.frmdate,oplist_costd.todate,oplist_costd.pkgnights  " & _
            '         "from  oplist_costd ,othcatmast,othtypmast  where " & _
            '         " oplist_costd.othcatcode=othcatmast.othcatcode and oplist_costd.othgrpcode=othcatmast.othgrpcode and " & _
            '         " oplist_costd.othtypcode=othtypmast.othtypcode and oplist_costd.othgrpcode = othtypmast.othgrpcode and " & _
            '         " ocplistcode='" & RefCode & "' order by oplist_costd.othgrpcode,oplist_costd.othtypcode,othtypmast.rankorder,othcatmast.grporder"
            'chnged trfplist_costd.oclineno from othtypmast.rankorder
            'StrQry = "select trfplist_costd.tplistcode ,trfplist_costd.plgrpcode,trfplist_costd.othgrpcode,trfplist_costd.oclineno, " & _
            '    " trfplist_costd.othtypcode, trfplist_costd.othcatcode, trfplist_costd.tcostprice, trfplist_costd.pkgnights " & _
            '    " from  trfplist_costd ,othcatmast,othtypmast  where  trfplist_costd.othcatcode=othcatmast.othcatcode " & _
            '    "  and trfplist_costd.othgrpcode=othcatmast.othgrpcode and  trfplist_costd.othtypcode=othtypmast.othtypcode and  " & _
            '    "  trfplist_costd.othgrpcode = othtypmast.othgrpcode and  tplistcode='" & RefCode & "' order by trfplist_costd.othgrpcode, " & _
            '    " trfplist_costd.othtypcode, othtypmast.rankorder, othcatmast.grporder "

            StrQry = "select othplist_selld.oplistcode,othplist_selld.plgrpcode,othplist_selld.othgrpcode,othplist_selld.oclineno, " & _
                " othplist_selld.othtypcode, othplist_selld.othcatcode, othplist_selld.tcostprice, othplist_selld.tprice, othplist_selld.pkgnights from  othplist_selld ,othcatmast,othtypmast " & _
                " where  othplist_selld.othcatcode=othcatmast.othcatcode   and othplist_selld.othgrpcode=othcatmast.othgrpcode and " & _
                " othplist_selld.othtypcode=othtypmast.othtypcode and    othplist_selld.othgrpcode = othtypmast.othgrpcode and  oplistcode ='" & RefCode & "'" & _
                " order by othplist_selld.othgrpcode,   othplist_selld.othtypcode, othtypmast.rankorder, othcatmast.grporder "


            Dim txt As TextBox
            Dim headerlabel As New TextBox
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
            mySqlCmd = New SqlCommand(StrQry, mySqlConn)
            mySqlReader = mySqlCmd.ExecuteReader()

            If mySqlReader.HasRows Then
                While mySqlReader.Read
                    For Each GVROW In gv_SearchResult.Rows
                        If IsDBNull(mySqlReader("oclineno")) = False Then
                            Linno = mySqlReader("oclineno")
                        End If
                        'If GVROW.Cells(1).Text = Linno Then
                        If GVROW.Cells(2).Text = mySqlReader("othtypcode") Then
                            StrQryTemp = "select othplist_selld.oplistcode ,othplist_selld.plgrpcode,othplist_selld.othgrpcode,othplist_selld.oclineno,  " & _
                                " othplist_selld.othtypcode, othplist_selld.othcatcode, othplist_selld.tcostprice,othplist_selld.tcostprice, othplist_selld.tprice, othplist_selld.pkgnights  " & _
                                "    from  othplist_selld ,othcatmast,othtypmast  where  othplist_selld.othcatcode=othcatmast.othcatcode  " & _
                                "  and othplist_selld.othgrpcode=othcatmast.othgrpcode and  othplist_selld.othtypcode=othtypmast.othtypcode and   " & _
                                "  othplist_selld.othgrpcode = othtypmast.othgrpcode and  oplistcode='" & RefCode & "' and othplist_selld.oclineno='" & Linno & "' order by  " & _
                                "  othplist_selld.othgrpcode, othplist_selld.othtypcode, othtypmast.rankorder, othcatmast.grporder"

                            myConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                            myCmd = New SqlCommand(StrQryTemp, myConn)
                            myReader = myCmd.ExecuteReader
                            If myReader.HasRows Then
                                While myReader.Read
                                    othcatcode = myReader("othcatcode")

                                    If IsDBNull(myReader("tcostprice")) = False Then
                                        value = myReader("tcostprice")
                                    Else
                                        value = ""
                                    End If
                                    If IsDBNull(myReader("tprice")) = False Then
                                        value = myReader("tprice")
                                    Else
                                        value = ""
                                    End If


                                    For j = 0 To cnt - 5
                                        If heading(i) = "From Date" Or heading(i) = "To Date" Or heading(i) = "Pkg" Then
                                            '
                                        Else
                                            For s = 0 To gv_SearchResult.Columns.Count - 5
                                                headerlabel = gv_SearchResult.HeaderRow.FindControl("txtHead" & s)
                                                If headerlabel.Text <> Nothing Then
                                                    If headerlabel.Text = othcatcode Then
                                                        If GVROW.RowIndex = 0 Then
                                                            txt = GVROW.FindControl("txt" & s)
                                                        Else
                                                            txt = GVROW.FindControl("txt" & ((gv_SearchResult.Columns.Count - 5) * GVROW.RowIndex) + s + GVROW.RowIndex)
                                                        End If
                                                        If txt Is Nothing Then
                                                        Else
                                                            If value = "" Or value = "0.000" Then
                                                                txt.Text = ""
                                                            Else
                                                                txt.Text = value
                                                            End If
                                                        End If
                                                        txt = GVROW.FindControl("txt" & ((gv_SearchResult.Columns.Count - 5) * GVROW.RowIndex) + gv_SearchResult.Columns.Count - 5 + GVROW.RowIndex)
                                                        If txt Is Nothing Then
                                                        Else
                                                            If IsDBNull(myReader("pkgnights")) = False Then
                                                                txt.Text = myReader("pkgnights")
                                                            Else
                                                                txt.Text = ""
                                                            End If
                                                        End If
                                                        GoTo go1
                                                    End If
                                                End If
                                            Next
                                        End If

                                    Next
go1:                            End While
                            End If
                            clsDBConnect.dbConnectionClose(myConn)
                            clsDBConnect.dbCommandClose(myCmd)
                            clsDBConnect.dbReaderClose(myReader)
                        End If
                    Next
                End While
            End If


            clsDBConnect.dbConnectionClose(mySqlConn)
            clsDBConnect.dbCommandClose(mySqlCmd)
            clsDBConnect.dbReaderClose(mySqlReader)
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & ex.Message & " ');", True)
        End Try

    End Sub
#End Region


    ''6 th change

#Region "Protected Sub BtnClearFormula_Click(ByVal sender As Object, ByVal e As System.EventArgs)"
    Protected Sub BtnClearFormula_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnClearFormula.Click
        Dim obj As New EncryptionDecryption
        Try


            Dim PlcCode As String = obj.Encrypt(CType(txtPlcCode.Value, String), "&%#@?,:*")

            Dim GroupCode As String = obj.Encrypt(CType(ddlGroupCode.Items(ddlGroupCode.SelectedIndex).Text, String), "&%#@?,:*")
            Dim GroupName As String = obj.Encrypt(CType(ddlGroupName.Items(ddlGroupName.SelectedIndex).Text, String), "&%#@?,:*")

            Dim currcode As String = obj.Encrypt(CType(txtCurrCode.Text, String), "&%#@?,:*")
            Dim currname As String = obj.Encrypt(CType(txtCurrName.Text, String), "&%#@?,:*")

            Dim SubSeasCode As String = obj.Encrypt(CType(ddlSubSeasCode.Items(ddlSubSeasCode.SelectedIndex).Text, String), "&%#@?,:*")
            Dim SubSeasName As String = obj.Encrypt(CType(ddlSubSeasName.Items(ddlSubSeasName.SelectedIndex).Text, String), "&%#@?,:*")




            Session("sessionRemark") = txtRemark.Value
            Session("ClearPrice") = "Yes"
            'Dim frmdate As String = obj.Encrypt(dpFromDate.Text, "&%#@?,:*")
            'Dim todate As String = obj.Encrypt(dpToDate.Text, "&%#@?,:*")
            Response.Redirect("OthPriceList1.aspx?&State=" & ViewState("OthpricelistState2") & "&RefCode=" & ViewState("OthpricelistRefCode2") &
                              "&PlcCode=" & PlcCode & "&GroupCode=" & GroupCode & "&GroupName=" & GroupName &
                              "&currcode=" & currcode & "&currname=" & currname & "&SubSeasCode=" & SubSeasCode & "&SubSeasName=" & SubSeasName &
                              "&Acvtive=" & IIf(ChkActive.Checked = True, 1, 0), False)

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        End Try
    End Sub
#End Region

#Region "Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs)"
    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Session("sessionRemark") = ""
        'Session("GV_HotelData") = ""
        Session("GV_OthPLData") = ""
        ViewState("OthpricelistRefCode2") = ""
        ViewState("OthpricelistState2") = ""
        'Response.Redirect("OtherServicesCostPriceListSearch.aspx", False)

        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", "window.close();", True)
    End Sub
#End Region

    Private Function GetNoGenName(ByVal prm_type As String) As String
        GetNoGenName = ""
        Dim strOption As String = ""

        If prm_type <> "OTH" Then
            strOption = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", prm_type)
            Select Case strOption
                Case "CAR RENTAL"
                    GetNoGenName = "CARPLIST"
                    Exit Function
                Case "VISA"
                    GetNoGenName = "VISAPLIST"
                    Exit Function
                Case "EXC"
                    GetNoGenName = "EXCPLIST"
                    Exit Function
                Case "MEALS"
                    GetNoGenName = "MEALSPLIST"
                    Exit Function
                Case "GUIDES"
                    GetNoGenName = "GUIDEPLIST"
                    Exit Function
                Case "ENTRANCE"
                    GetNoGenName = "ENTRPLIST"
                    Exit Function
                Case "JEEPWADI"
                    GetNoGenName = "JEEPWPLIST"
                    Exit Function
                Case "HFEES"
                    GetNoGenName = "HFEESPLIST"
                    Exit Function
                Case "AIRPORTMA"
                    GetNoGenName = "AIRMAPLIST"
                    Exit Function
            End Select
        Else
            GetNoGenName = "OTHPLIST"

        End If
    End Function

#Region " Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs)"
    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Try
            'If Not Session("PlistSaved") = True Then ''this variable because response.redirect is causing a postback and saving twice

            Dim GvRow As GridViewRow
            If Page.IsValid = True Then
                If ViewState("OthpricelistState2") = "New" Or ViewState("OthpricelistState2") = "Edit" Or ViewState("OthpricelistState2") = "Copy" Then
                    If ValidatePage() = False Then
                        Exit Sub
                    End If
                    If CheckDecimalInGrid() = False Then
                        Exit Sub
                    End If
                    '********unchk later csn************
                    'If chkConsdierForMarkUp.Checked = True Then
                    '    If ViewState("OthpricelistState2") = "Edit" And chkConsdierForMarkUp.Enabled = False Then
                    '    Else

                    '    End If
                    'End If




                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start

                    If ViewState("OthpricelistState2") = "New" Or ViewState("OthpricelistState2") = "Copy" Then
                        Dim optionval As String
                        Dim optionName As String

                        optionName = GetNoGenName(Session("OthPListFilter"))
                        optionval = objUtils.GetAutoDocNo(optionName, mySqlConn, sqlTrans)
                        txtPlcCode.Value = optionval.Trim
                        mySqlCmd = New SqlCommand("sp_add_OthPListh", mySqlConn, sqlTrans)
                    ElseIf ViewState("OthpricelistState2") = "Edit" Then
                        'Inserting Into Logs
                        mySqlCmd = New SqlCommand("sp_visapriceslist_logs", mySqlConn, sqlTrans)
                        mySqlCmd.CommandType = CommandType.StoredProcedure
                        mySqlCmd.Parameters.Add(New SqlParameter("@plistcode", SqlDbType.VarChar, 20)).Value = CType(txtPlcCode.Value.Trim, String)
                        mySqlCmd.ExecuteNonQuery()

                        mySqlCmd = New SqlCommand("sp_mod_OthPListh", mySqlConn, sqlTrans)
                    End If

                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@oplistcode", SqlDbType.VarChar, 20)).Value = CType(txtPlcCode.Value.Trim, String)

                    If CType(ddlSubSeasCode.Items(ddlSubSeasCode.SelectedIndex).Text.Trim, String) = "[Select]" Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@subseascode", SqlDbType.VarChar, 20)).Value = DBNull.Value
                    Else
                        mySqlCmd.Parameters.Add(New SqlParameter("@subseascode", SqlDbType.VarChar, 20)).Value = CType(ddlSubSeasCode.Items(ddlSubSeasCode.SelectedIndex).Text.Trim, String)
                    End If


                    If CType(ddlOtherSellingTypeCode.Items(ddlOtherSellingTypeCode.SelectedIndex).Text.Trim, String) = "[Select]" Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@othsellcode", SqlDbType.VarChar, 20)).Value = DBNull.Value
                    Else
                        mySqlCmd.Parameters.Add(New SqlParameter("@othsellcode", SqlDbType.VarChar, 20)).Value = CType(ddlOtherSellingTypeCode.Items(ddlOtherSellingTypeCode.SelectedIndex).Text.Trim, String)
                    End If

                    If CType(ddlGroupCode.Items(ddlGroupCode.SelectedIndex).Text.Trim, String) = "[Select]" Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@othgrpcode", SqlDbType.VarChar, 20)).Value = DBNull.Value
                    Else
                        mySqlCmd.Parameters.Add(New SqlParameter("@othgrpcode", SqlDbType.VarChar, 20)).Value = CType(ddlGroupCode.Items(ddlGroupCode.SelectedIndex).Text.Trim, String)
                    End If



                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = DBNull.Value

                    If CType(txtCurrCode.Text.Trim, String) = "" Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@currcode", SqlDbType.VarChar, 20)).Value = DBNull.Value
                    Else
                        mySqlCmd.Parameters.Add(New SqlParameter("@currcode", SqlDbType.VarChar, 20)).Value = CType(txtCurrCode.Text.Trim, String)
                    End If

                    mySqlCmd.Parameters.Add(New SqlParameter("@remarks", SqlDbType.Text)).Value = CType(txtRemark.Value.Trim, String)

                    If ChkActive.Checked = True Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@active", SqlDbType.Int)).Value = 1
                    ElseIf ChkActive.Checked = False Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@active", SqlDbType.Int)).Value = 0
                    End If

                    mySqlCmd.Parameters.Add(New SqlParameter("@chkMarkUp", SqlDbType.Int)).Value = 0



                    If ViewState("OthpricelistState2") = "New" Or ViewState("OthpricelistState2") = "Copy" Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                    Else
                        mySqlCmd.Parameters.Add(New SqlParameter("@moduser", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                    End If


                    mySqlCmd.Parameters.Add(New SqlParameter("@supagentcode", SqlDbType.VarChar, 20)).Value = DBNull.Value


                    If chkapprove.Checked = True Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@approve", SqlDbType.Int, 9)).Value = 1
                    Else
                        mySqlCmd.Parameters.Add(New SqlParameter("@approve", SqlDbType.Int, 9)).Value = 0
                    End If

                    mySqlCmd.ExecuteNonQuery()
                    '-----------------------------------------------------------
                    '----------------------------------- Deleting Data From Dates Table
                    mySqlCmd = New SqlCommand("sp_del_othplisth_dates", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@oplistcode", SqlDbType.VarChar, 20)).Value = CType(txtPlcCode.Value.Trim, String)
                    mySqlCmd.ExecuteNonQuery()
                    '----------------------------------- Inserting Data To Dates Table
                    For Each GvRow In grdDates.Rows
                        dpFDate = GvRow.FindControl("txtfromDate")
                        dpTDate = GvRow.FindControl("txtToDate")
                        If dpFDate.Text <> "" And dpTDate.Text <> "" Then
                            mySqlCmd = New SqlCommand("sp_add_othplisth_dates", mySqlConn, sqlTrans)
                            mySqlCmd.CommandType = CommandType.StoredProcedure
                            mySqlCmd.Parameters.Add(New SqlParameter("@oplistcode", SqlDbType.VarChar, 20)).Value = CType(txtPlcCode.Value.Trim, String)
                            mySqlCmd.Parameters.Add(New SqlParameter("@frmdate", SqlDbType.DateTime)).Value = ObjDate.ConvertDateromTextBoxToDatabase(dpFDate.Text)
                            mySqlCmd.Parameters.Add(New SqlParameter("@todate", SqlDbType.DateTime)).Value = ObjDate.ConvertDateromTextBoxToDatabase(dpTDate.Text)
                            If CType(ddlSubSeasCode.Items(ddlSubSeasCode.SelectedIndex).Text, String) = "[Select]" Then
                                mySqlCmd.Parameters.Add(New SqlParameter("@subseascode", SqlDbType.VarChar, 20)).Value = DBNull.Value
                            Else
                                mySqlCmd.Parameters.Add(New SqlParameter("@subseascode", SqlDbType.VarChar, 20)).Value = CType(ddlSubSeasCode.Items(ddlSubSeasCode.SelectedIndex).Text, String)
                            End If

                            mySqlCmd.ExecuteNonQuery()
                        End If
                    Next




                    mySqlCmd = New SqlCommand("sp_del_othplist_selld", mySqlConn, sqlTrans)

                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@oplistcode", SqlDbType.VarChar, 20)).Value = CType(txtPlcCode.Value.Trim, String)
                    mySqlCmd.ExecuteNonQuery()

                    Dim j As Long = 1
                    Dim txt As TextBox
                    Dim cnt As Long
                    Dim srno As Long = 0
                    Dim hotelcategory As String = ""
                    j = 0

                    gv_SearchResult.Visible = True
                    cnt = gv_SearchResult.Columns.Count
                    Dim n As Long = 0
                    Dim k As Long = 0
                    Dim a As Long = cnt - 7
                    Dim b As Long = 0
                    Dim header As Long = 0
                    Dim heading(cnt + 1) As String
                    '----------------------------------------------------------------------------
                    '           Stoaring heading column values in the array
                    For header = 0 To cnt - 5
                        txt = gv_SearchResult.HeaderRow.FindControl("txtHead" & header)
                        heading(header) = txt.Text
                    Next
                    '----------------------------------------------------------------------------


                    Dim m As Long = 0
                    j = 0
                    n = 0
                    'm = 0
                    b = 0

                    For Each GvRow In gv_SearchResult.Rows
                        If n = 0 Then
                            For j = 0 To cnt - 5

                                If heading(j) = "From Date" Or heading(j) = "To Date" Or heading(j) = "Pkg" Then

                                Else
                                    txt = GvRow.FindControl("txt" & j)
                                    If txt.Text = "" Then
                                        txt.Text = "0"
                                    End If


                                    mySqlCmd = New SqlCommand("sp_add_Othplist_Selld", mySqlConn, sqlTrans)
                                    mySqlCmd.CommandType = CommandType.StoredProcedure
                                    mySqlCmd.Parameters.Add(New SqlParameter("@oplistcode", SqlDbType.VarChar, 20)).Value = CType(txtPlcCode.Value.Trim, String)
                                    'Here pass othsellcode


                                    mySqlCmd.Parameters.Add(New SqlParameter("@plgrpcode", SqlDbType.VarChar, 20)).Value = DBNull.Value


                                    If CType(ddlOtherSellingTypeCode.Items(ddlOtherSellingTypeCode.SelectedIndex).Text.Trim, String) = "[Select]" Then
                                        mySqlCmd.Parameters.Add(New SqlParameter("@othsellcode", SqlDbType.VarChar, 20)).Value = DBNull.Value
                                    Else
                                        mySqlCmd.Parameters.Add(New SqlParameter("@othsellcode", SqlDbType.VarChar, 20)).Value = CType(ddlOtherSellingTypeCode.Items(ddlOtherSellingTypeCode.SelectedIndex).Text.Trim, String)
                                    End If




                                    If CType(ddlGroupCode.Items(ddlGroupCode.SelectedIndex).Text.Trim, String) = "[Select]" Then
                                        mySqlCmd.Parameters.Add(New SqlParameter("@othgrpcode", SqlDbType.VarChar, 20)).Value = DBNull.Value
                                    Else
                                        mySqlCmd.Parameters.Add(New SqlParameter("@othgrpcode", SqlDbType.VarChar, 20)).Value = CType(ddlGroupCode.Items(ddlGroupCode.SelectedIndex).Text.Trim, String)
                                    End If
                                    mySqlCmd.Parameters.Add(New SqlParameter("@oclineno", SqlDbType.Int)).Value = CType(GvRow.Cells(1).Text, Long)

                                    mySqlCmd.Parameters.Add(New SqlParameter("@othtypcode", SqlDbType.VarChar, 20)).Value = CType(GvRow.Cells(2).Text, String)
                                    mySqlCmd.Parameters.Add(New SqlParameter("@othcatcode", SqlDbType.VarChar, 20)).Value = CType(heading(j), String)


                                    'txt = GvRow.FindControl("txt" & j)

                                    mySqlCmd.Parameters.Add(New SqlParameter("@tcostprice", SqlDbType.Decimal, 18, 3)).Value = 0


                                    'txt = GvRow.FindControl("txt" & b + a + 1)

                                    txt = GvRow.FindControl("txt" & j)


                                    mySqlCmd.Parameters.Add(New SqlParameter("@tprice", SqlDbType.Decimal, 18, 3)).Value = CType(Val(txt.Text), Long)

                                    txt = GvRow.FindControl("txt" & b + a + 2)

                                    mySqlCmd.Parameters.Add(New SqlParameter("@pknights", SqlDbType.Int)).Value = 1

                                    mySqlCmd.ExecuteNonQuery()
                                End If


                            Next
                            m = j
                        Else
                            k = 0


                            For j = n To (m + n) - 1
                                If heading(k) = "From Date" Or heading(k) = "To Date" Or heading(k) = "Pkg" Then

                                Else
                                    txt = GvRow.FindControl("txt" & j)
                                    If txt.Text = "" Then
                                        txt.Text = "0"
                                    End If

                                    mySqlCmd = New SqlCommand("sp_add_Othplist_Selld", mySqlConn, sqlTrans)
                                    mySqlCmd.CommandType = CommandType.StoredProcedure
                                    mySqlCmd.Parameters.Add(New SqlParameter("@oplistcode", SqlDbType.VarChar, 20)).Value = CType(txtPlcCode.Value.Trim, String)


                                    mySqlCmd.Parameters.Add(New SqlParameter("@plgrpcode", SqlDbType.VarChar, 20)).Value = DBNull.Value


                                    If CType(ddlGroupCode.Items(ddlGroupCode.SelectedIndex).Text.Trim, String) = "[Select]" Then
                                        mySqlCmd.Parameters.Add(New SqlParameter("@othgrpcode", SqlDbType.VarChar, 20)).Value = DBNull.Value
                                    Else
                                        mySqlCmd.Parameters.Add(New SqlParameter("@othgrpcode", SqlDbType.VarChar, 20)).Value = CType(ddlGroupCode.Items(ddlGroupCode.SelectedIndex).Text.Trim, String)
                                    End If
                                    If CType(ddlOtherSellingTypeCode.Items(ddlOtherSellingTypeCode.SelectedIndex).Text.Trim, String) = "[Select]" Then
                                        mySqlCmd.Parameters.Add(New SqlParameter("@othsellcode", SqlDbType.VarChar, 20)).Value = DBNull.Value
                                    Else
                                        mySqlCmd.Parameters.Add(New SqlParameter("@othsellcode", SqlDbType.VarChar, 20)).Value = CType(ddlOtherSellingTypeCode.Items(ddlOtherSellingTypeCode.SelectedIndex).Text.Trim, String)
                                    End If


                                    mySqlCmd.Parameters.Add(New SqlParameter("@othcatcode", SqlDbType.VarChar, 20)).Value = CType(heading(k), String)
                                    mySqlCmd.Parameters.Add(New SqlParameter("@othtypcode", SqlDbType.VarChar, 20)).Value = CType(GvRow.Cells(2).Text, String)
                                    'txt = GvRow.FindControl("txt" & j)

                                    'Test here

                                    mySqlCmd.Parameters.Add(New SqlParameter("@tcostprice", SqlDbType.Decimal, 18, 3)).Value = 0


                                    mySqlCmd.Parameters.Add(New SqlParameter("@oclineno", SqlDbType.Int)).Value = CType(GvRow.Cells(1).Text, Long)


                                    'txt = GvRow.FindControl("txt" & b + a + 1)

                                    txt = GvRow.FindControl("txt" & j)


                                    mySqlCmd.Parameters.Add(New SqlParameter("@tprice", SqlDbType.Decimal, 18, 3)).Value = CType(txt.Text, Long)

                                    txt = GvRow.FindControl("txt" & b + a + 2)

                                    mySqlCmd.Parameters.Add(New SqlParameter("@pknights", SqlDbType.Int)).Value = 1

                                    mySqlCmd.ExecuteNonQuery()
                                    k = k + 1
                                End If  'if condition


                            Next 'For llop
                        End If
                        b = j
                        n = j
                    Next  'gv_search Result
                    'End If

                    '---------------------------------------------End of save/edit/copy------------------------
                ElseIf ViewState("OthpricelistState2") = "Delete" Then
                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start

                    'Inserting Into Logs
                    mySqlCmd = New SqlCommand("sp_visapriceslist_logs", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@plistcode", SqlDbType.VarChar, 20)).Value = CType(txtPlcCode.Value.Trim, String)
                    mySqlCmd.ExecuteNonQuery()

                    'first deleting all detail tabl values and thn deleting main table values
                    mySqlCmd = New SqlCommand("sp_del_othplisth_dates", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@oplistcode", SqlDbType.VarChar, 20)).Value = CType(txtPlcCode.Value.Trim, String)
                    mySqlCmd.ExecuteNonQuery()





                    mySqlCmd = New SqlCommand("sp_del_othplist_selld", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@oplistcode", SqlDbType.VarChar, 20)).Value = CType(txtPlcCode.Value.Trim, String)
                    mySqlCmd.ExecuteNonQuery()
                    'Carefully checking whether it is required or not

                    'mySqlCmd = New SqlCommand("sp_del_othplist_othcat_slabs", mySqlConn, sqlTrans)
                    'mySqlCmd.CommandType = CommandType.StoredProcedure
                    'mySqlCmd.Parameters.Add(New SqlParameter("@oplistcode", SqlDbType.VarChar, 20)).Value = CType(txtPlcCode.Value.Trim, String)
                    'mySqlCmd.ExecuteNonQuery()

                    'delete of main tbl to be written
                    mySqlCmd = New SqlCommand("sp_del_Othplisth", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@oplistcode", SqlDbType.VarChar, 20)).Value = CType(txtPlcCode.Value.Trim, String)
                    mySqlCmd.ExecuteNonQuery()

                End If

                sqlTrans.Commit()    'SQl Tarn Commit
                clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
                clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
                clsDBConnect.dbConnectionClose(mySqlConn)           'connection close
                Session("PlistSaved") = True
                Session("sessionRemark") = Nothing
                'Session("GV_HotelData") = Nothing
                Session("GV_OthPLData") = Nothing
                'ViewState("TrfpricelistRefCode2") = Nothing
                'ViewState("TrfpricelistState2") = Nothing
                Session("SesionPlistCode") = txtPlcCode.Value

                If chkConsdierForMarkUp.Checked = False Then

                    ViewState("OthpricelistRefCode2") = Nothing
                    ViewState("OthpricelistState2") = Nothing
                    Response.Redirect("OthPriceListSearch.aspx?Type=" + Session("OthPListFilter"), False)

                Else
                    '********unchk later csn************
                    If ViewState("OthpricelistState2") = "Delete" Then
                        'Response.Redirect("PriceList.aspx", False)
                        Dim strscript1 As String = ""
                        strscript1 = "window.opener.__doPostBack('OthPriceListWindowPostBack', '');window.opener.focus();window.close();"
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript1, True)

                    Else
                        'check if in oth paxcalc reqd thn pax slab page else selling page
                        SqlReader = Nothing
                        Dim strentryQuery As String
                        If Session("OthPListFilter") <> Nothing And Session("OthPListFilter") <> "OTH" Then
                            strentryQuery = "select 1  from othgrpmast where othgrpcode =(select option_selected  from reservation_parameters where param_id=" + Session("OthPListFilter") + ")" & _
                                                                                            " and paxcalcreqd=1"
                            SqlReader = objUtils.GetDataFromReadernew(Session("dbconnectionName"), strentryQuery)
                        Else

                        End If

                        '-------------------market common for both funtnlities-------------------------------
                        '13th change
                        'Dim chksel As CheckBox
                        'Dim marketstr As String = ""
                        'Dim lblcode As Label

                        'For Each Me.gvRow1 In gv_Market.Rows
                        '    chksel = gvRow1.FindControl("chkSelect")
                        '    lblcode = gvRow1.FindControl("lblcode")
                        '    If chksel.Checked = True Then
                        '        marketstr = marketstr + ";" + lblcode.Text
                        '    End If
                        'Next
                        'If marketstr.Length > 0 Then
                        '    marketstr = marketstr.Substring(1, marketstr.Length - 1)
                        'End If
                        '---------------------------------------------
                        If SqlReader IsNot Nothing Then
                            If SqlReader.HasRows = True Then
                                'to pax slab sellling pages
                                '14th change
                                '& "&Market=" & marketstr
                                ViewState.Add("OthpricelistRefCode2", txtPlcCode.Value)
                                Response.Redirect("OthSellingRatePaxSlab.aspx?State=" & CType(ViewState("OthpricelistState2"), String) &
                                                  "&RefCode=" & CType(ViewState("OthpricelistRefCode2"), String) &
                                                  "&CurrencyCode=" & txtCurrCode.Text & "&CurrencyName=" & txtCurrName.Text &
                                                  "&SubSeasonCode=" & ddlSubSeasCode.Items(ddlSubSeasCode.SelectedIndex).Text &
                                                  "&SubSeasonName=" & ddlSubSeasName.Items(ddlSubSeasName.SelectedIndex).Text, False)

                            End If


                        Else
                        End If

                        If SqlReader IsNot Nothing Then
                            If SqlReader.HasRows = False Then

                                'to normal seling pages

                                ViewState.Add("OthpricelistRefCode2", txtPlcCode.Value)
                                'Response.Redirect("HederRsellingcode1new.aspx?State=" & CType(ViewState("HeaderinfoState1"), String) & "&RefCode=" & CType(ViewState("HeaderinfoRefCode1"), String) & "&supplier=" & ddlSuppierCD.Items(ddlSuppierCD.SelectedIndex).Text & "&suppliername=" & ddlSuppierNM.Items(ddlSuppierNM.SelectedIndex).Text & "&SupplierType=" & ddlSPTypeCD.Items(ddlSPTypeCD.SelectedIndex).Text & "&SupplierTypeName=" & ddlSPTypeNM.Items(ddlSPTypeNM.SelectedIndex).Text & "&Market=" & marketstr & "&SuppierAgent=" & ddlSupplierAgent.Items(ddlSupplierAgent.SelectedIndex).Text & "&SupplierAgentName=" & ddlSuppierAgentNM.Items(ddlSuppierAgentNM.SelectedIndex).Text & "&CurrencyCode=" & ddlCurrencyCD.Value & "&CurrencyName=" & ddlCurrencyNM.Value & "&SubSeasonCode=" & ddlSubSeas.Items(ddlSubSeas.SelectedIndex).Text & "&SubSeasonName=" & ddlSubSeasNM.Items(ddlSubSeasNM.SelectedIndex).Text & "&RevisionDate=" & dpRevsiondate.Text & "&PListCode=" & txtBlockCode.Value & "&hdnpricelist=" & hdnpricelist.Value & "&pricelist=" & ddlPriceList.SelectedValue & "&week1=" & week1 & "&week2=" & week2 & "&manual=" & manual & "&promotionname=" & ddlPromotion.Items(ddlPromotion.SelectedIndex).Text & "&promotioncode=" & txtPromotionCode.Text, False)


                                Response.Redirect("OthPricelistSellingRates1.aspx?State=" & CType(ViewState("OthpricelistState2"), String) &
                                                  "&RefCode=" & CType(ViewState("OthpricelistRefCode2"), String) &
                                                  "&CurrencyCode=" & txtCurrCode.Text & "&CurrencyName=" & txtCurrName.Text &
                                                  "&SubSeasonCode=" & ddlSubSeasCode.Items(ddlSubSeasCode.SelectedIndex).Text &
                                                  "&SubSeasonName=" & ddlSubSeasName.Items(ddlSubSeasName.SelectedIndex).Text, False)
                            End If
                            'Response.Redirect("TrfPricelistSellingRates.aspx?State=New&RefCode=TRPL/000015&supplier=T-000002&suppliername=FADI ECRS" & _
                            '                  "&SupplierType=Transfers&SupplierTypeName=Transfers&Market=CIS;&SuppierAgent=WONINF&SupplierAgentName=" & _
                            '                  "&CurrencyCode=AED&CurrencyName=DIRHAM&SubSeasonCode=ALL SEASONS&SubSeasonName=ALL", False)
                        Else
                            'same as above since, smtms sql= nothng 
                            'to normal seling pages

                            ViewState.Add("OthpricelistRefCode2", txtPlcCode.Value)
                            'Response.Redirect("HederRsellingcode1new.aspx?State=" & CType(ViewState("HeaderinfoState1"), String) & "&RefCode=" & CType(ViewState("HeaderinfoRefCode1"), String) & "&supplier=" & ddlSuppierCD.Items(ddlSuppierCD.SelectedIndex).Text & "&suppliername=" & ddlSuppierNM.Items(ddlSuppierNM.SelectedIndex).Text & "&SupplierType=" & ddlSPTypeCD.Items(ddlSPTypeCD.SelectedIndex).Text & "&SupplierTypeName=" & ddlSPTypeNM.Items(ddlSPTypeNM.SelectedIndex).Text & "&Market=" & marketstr & "&SuppierAgent=" & ddlSupplierAgent.Items(ddlSupplierAgent.SelectedIndex).Text & "&SupplierAgentName=" & ddlSuppierAgentNM.Items(ddlSuppierAgentNM.SelectedIndex).Text & "&CurrencyCode=" & ddlCurrencyCD.Value & "&CurrencyName=" & ddlCurrencyNM.Value & "&SubSeasonCode=" & ddlSubSeas.Items(ddlSubSeas.SelectedIndex).Text & "&SubSeasonName=" & ddlSubSeasNM.Items(ddlSubSeasNM.SelectedIndex).Text & "&RevisionDate=" & dpRevsiondate.Text & "&PListCode=" & txtBlockCode.Value & "&hdnpricelist=" & hdnpricelist.Value & "&pricelist=" & ddlPriceList.SelectedValue & "&week1=" & week1 & "&week2=" & week2 & "&manual=" & manual & "&promotionname=" & ddlPromotion.Items(ddlPromotion.SelectedIndex).Text & "&promotioncode=" & txtPromotionCode.Text, False)


                            Response.Redirect("OthPricelistSellingRates1.aspx?State=" & CType(ViewState("OthpricelistState2"), String) &
                                              "&RefCode=" & CType(ViewState("OthpricelistRefCode2"), String) &
                                              "&CurrencyCode=" & txtCurrCode.Text & "&CurrencyName=" & txtCurrName.Text &
                                              "&SubSeasonCode=" & ddlSubSeasCode.Items(ddlSubSeasCode.SelectedIndex).Text &
                                              "&SubSeasonName=" & ddlSubSeasName.Items(ddlSubSeasName.SelectedIndex).Text, False)
                        End If

                    End If
                    'End If
                    Dim strscript As String = ""
                    strscript = "window.opener.__doPostBack('OthPriceListWindowPostBack', '');window.opener.focus();window.close();"
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)

                End If
            End If
        Catch ex As Exception
            If mySqlConn.State = ConnectionState.Open Then
                sqlTrans.Rollback()
            End If
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("OthPriceList2.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#End Region

#Region "Private Function CheckDecimalInGrid() As Boolean"
    Private Function CheckDecimalInGrid() As Boolean
        Dim j As Long = 0
        Dim txt As TextBox
        Dim cnt As Long
        Dim srno As Long = 0
        Dim hotelcategory As String = ""
        Dim n As Long = 0
        Dim k As Long = 0
        Dim a As Long = cnt - 7
        Dim b As Long = 0
        Dim header As Long = 0
        Dim arr() As String
        Dim GvRow As GridViewRow
        Try
            gv_SearchResult.Visible = True
            cnt = gv_SearchResult.Columns.Count
            Dim heading(cnt + 1) As String
            '----------------------------------------------------------------------------
            '           Stoaring heading column values in the array
            For header = 0 To cnt - 5
                txt = gv_SearchResult.HeaderRow.FindControl("txtHead" & header)
                heading(header) = txt.Text
            Next
            '----------------------------------------------------------------------------
            Dim m As Long = 0
            For Each GvRow In gv_SearchResult.Rows
                If n = 0 Then
                    For j = 0 To cnt - 5
                        If heading(j) = "From Date" Or heading(j) = "To Date" Or heading(j) = "Pkg" Then
                        Else
                            txt = GvRow.FindControl("txt" & j)
                            If txt.Text <> "" Then
                                arr = txt.Text.Split(".")
                                If arr.Length > 2 Then
                                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Enter Valid decimal values in Grid Rates.');", True)
                                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + gv_SearchResult.ClientID + "');", True)
                                    CheckDecimalInGrid = False
                                    Exit Function
                                End If
                            End If
                        End If
                    Next
                    m = j
                Else
                    k = 0
                    For j = n To (m + n) - 1
                        If heading(k) = "From Date" Or heading(k) = "To Date" Or heading(k) = "Pkg" Then
                        Else
                            txt = GvRow.FindControl("txt" & j)
                            If txt.Text <> "" Then
                                arr = txt.Text.Split(".")
                                If arr.Length > 2 Then
                                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Enter Valid decimal values in Grid Rates.');", True)
                                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + gv_SearchResult.ClientID + "');", True)
                                    CheckDecimalInGrid = False
                                    Exit Function
                                End If
                            End If
                            k = k + 1
                        End If
                    Next
                End If
                b = j
                n = j
            Next
            CheckDecimalInGrid = True
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("OtherServicesCostPriceList2.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Function
#End Region

#Region "Private Function FindFooterControl(ByVal Controlname As String, ByVal value As String)"
    Private Function FindHeaderTextbox(ByVal Controlname As String, ByVal value As String) As String
        Dim footerlabel As New TextBox
        Dim irow As Integer
        For irow = 0 To gv_SearchResult.Controls(0).Controls.Count - 1
            If CType(gv_SearchResult.Controls(0).Controls(irow), GridViewRow).RowType = DataControlRowType.Header Then
                Dim footer As GridViewRow = CType(gv_SearchResult.Controls(0).Controls(irow), GridViewRow)
                footerlabel = footer.FindControl(Controlname)
                Return footerlabel.Text
                Exit For
            End If
        Next
        Return footerlabel.Text
    End Function
#End Region

#Region "Private Function ValidatePage() As Boolean"
    Private Function ValidatePage() As Boolean
        Dim j As Long = 1
        Dim txt As TextBox, txtnights As TextBox
        Dim cnt As Long
        Dim GvRow As GridViewRow
        'Dim GvRowRMType As GridViewRow
        'Dim gvrowfrmla As GridViewRow
        'Dim srno As Long = 0
        'Dim hotelcategory As String = ""
        j = 0
        gv_SearchResult.Visible = True
        cnt = gv_SearchResult.Columns.Count
        Dim n As Long = 0
        Dim k As Long = 0
        Dim a As Long = cnt - 7
        Dim b As Long = 0
        Dim header As Long = 0
        Dim heading(cnt + 1) As String
        Dim flag As Boolean = False
        Dim lblcode As Label
        Try
            For header = 0 To cnt - 5
                heading(header) = FindHeaderTextbox("txtHead" & header, "")
            Next
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)

        End Try

        Dim m As Long = 0

        'Dim GvRow1 As GridViewRow
        Try
            '--------------------   Date Validations    ---------------------------------------
            Dim flgdt As Boolean = False
            Dim ToDt As Date = Nothing

            'Dim dtValidate As New DataTable
            'dtValidate = Session("GV_HotelData")
            Dim cntStartCol As Integer = 5
            Dim cntEndCol As Integer = gv_SearchResult.Columns.Count - 1
            Dim cntCol As Integer = 5
            Dim AllowFlg As Integer
            Dim ErrMsg As String
            Dim txtRMType As TextBox
            Dim cntN As Integer = 0
            Dim cntJ As Integer = 0

           

            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open

            For Each GvRow In grdDates.Rows
                dpFDate = GvRow.FindControl("txtfromDate")
                dpTDate = GvRow.FindControl("txtToDate")
                If dpFDate.Text <> "" And dpTDate.Text <> "" Then
                    If ObjDate.ConvertDateromTextBoxToDatabase(dpTDate.Text) < ObjDate.ConvertDateromTextBoxToDatabase(dpFDate.Text) Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('All the To Dates should be greater than From Dates.');", True)
                        SetFocus(dpTDate)
                        ValidatePage = False
                        Exit Function
                    End If

                    If ToDt <> Nothing Then
                        If ObjDate.ConvertDateromTextBoxToDatabase(dpFDate.Text) <= ToDt Then
                            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Date Overlapping.');", True)
                            SetFocus(dpFDate)
                            ValidatePage = False
                            Exit Function
                        End If
                    End If
                    ToDt = ObjDate.ConvertDateromTextBoxToDatabase(dpTDate.Text)
                    flgdt = True
                    '''''''''''''''
                    ''''''''''''''''''
                End If


                If dpFDate.Text <> "" And dpTDate.Text <> "" Then


                    j = 0
                    n = 0
                    m = 0
                    b = 0
                    For Each GvRowRMType In gv_SearchResult.Rows
                        If n = 0 Then
                            For j = 0 To cnt - 5
                                If heading(j) = "From Date" Or heading(j) = "To Date" Or heading(j) = "Pkg" Then

                                Else
                                    txt = GvRowRMType.FindControl("txt" & j)
                                    If txt Is Nothing Then
                                    Else
                                        If txt.Text <> "" Then
                                            If CType(Val(txt.Text), String) <> "0" Then
                                                'txtRMType = GvRowRMType.FindControl("txtrmtypcode")
                                                'If ddlPriceList.SelectedValue = "Weekend Rates 1 Night" Or ddlPriceList.SelectedValue = "Weekend Rates > 1 Night" Then
                                                '    mySqlCmd = New SqlCommand("sp_chkplwkdatenew", mySqlConn)
                                                'Else
                                                mySqlCmd = New SqlCommand("sp_chkOthpldatenew", mySqlConn)
                                                'End If
                                                mySqlCmd.CommandType = CommandType.StoredProcedure


                                                mySqlCmd.Parameters.Add(New SqlParameter("@othgrpcode", SqlDbType.VarChar, 20)).Value = CType(ddlGroupCode.Items(ddlGroupCode.SelectedIndex).Text.Trim, String)
                                                'mySqlCmd.Parameters.Add(New SqlParameter("@rmtypcode", SqlDbType.VarChar, 20)).Value = CType(txtRMType.Text.Trim, String)
                                                mySqlCmd.Parameters.Add(New SqlParameter("@selltype", SqlDbType.VarChar, 20)).Value = CType(ddlOtherSellingTypeCode.Items(ddlOtherSellingTypeCode.SelectedIndex).Text.Trim, String)
                                                'mySqlCmd.Parameters.Add(New SqlParameter("@mealcode", SqlDbType.VarChar, 20)).Value = CType(gv_SearchResult.Rows(GvRowRMType.RowIndex).Cells(3).Text.Trim, String)
                                                'mySqlCmd.Parameters.Add(New SqlParameter("@rmcatcode", SqlDbType.VarChar, 20)).Value = CType(dtValidate.Columns(cntCol).ColumnName.Trim, String)
                                                mySqlCmd.Parameters.Add(New SqlParameter("@frmdate", SqlDbType.DateTime)).Value = CType(ObjDate.ConvertDateromTextBoxToDatabase(dpFDate.Text.Trim), Date)
                                                mySqlCmd.Parameters.Add(New SqlParameter("@todate", SqlDbType.DateTime)).Value = CType(ObjDate.ConvertDateromTextBoxToDatabase(dpTDate.Text.Trim), Date)
                                                If ViewState("OthpricelistState2") = "New" Or ViewState("OthpricelistState2") = "Copy" Then
                                                    mySqlCmd.Parameters.Add(New SqlParameter("@oplistcode", SqlDbType.VarChar, 20)).Value = ""
                                                Else
                                                    mySqlCmd.Parameters.Add(New SqlParameter("@oplistcode", SqlDbType.VarChar, 20)).Value = CType(txtPlcCode.Value.Trim, String)
                                                End If


                                                txtnights = GvRowRMType.FindControl("txt" & b + a + 2)
                                                If Not txtnights Is Nothing Then
                                                    mySqlCmd.Parameters.Add(New SqlParameter("@pkgnights", SqlDbType.Int, 20)).Value = CType(txtnights.Text.Trim, Integer)

                                                End If


                                                'If (txtPromotionCode.Text.Trim = "") Then
                                                '    mySqlCmd.Parameters.Add(New SqlParameter("@promotionid", SqlDbType.VarChar, 20)).Value = ""
                                                'Else
                                                '    mySqlCmd.Parameters.Add(New SqlParameter("@promotionid", SqlDbType.VarChar, 20)).Value = CType(txtPromotionCode.Text.Trim, String)
                                                'End If
                                                Dim paramAllowFlg As New SqlParameter
                                                paramAllowFlg.ParameterName = "@allowflg"
                                                paramAllowFlg.Direction = ParameterDirection.Output
                                                paramAllowFlg.DbType = DbType.Int64
                                                paramAllowFlg.Size = 20
                                                mySqlCmd.Parameters.Add(paramAllowFlg)

                                                Dim paramErrMsg As New SqlParameter
                                                paramErrMsg.ParameterName = "@errmsg"
                                                paramErrMsg.Direction = ParameterDirection.Output
                                                paramErrMsg.DbType = DbType.String
                                                paramErrMsg.Size = 100
                                                mySqlCmd.Parameters.Add(paramErrMsg)

                                                mySqlCmd.ExecuteNonQuery()

                                                AllowFlg = paramAllowFlg.Value
                                                ErrMsg = paramErrMsg.Value

                                                mySqlCmd = Nothing

                                                If CType(AllowFlg, String) = "1" Then
                                                    ValidatePage = False
                                                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & ErrMsg.Replace("'", " ") & "');", True)
                                                    Exit Function
                                                End If
                                            End If
                                        End If
                                    End If

                                End If
                            Next
                            m = j
                        Else
                            k = 0
                            For j = n To (m + n) - 1
                                If heading(k) = "From Date" Or heading(k) = "To Date" Or heading(k) = "Pkg" Then
                                Else
                                    txt = GvRowRMType.FindControl("txt" & j)
                                    If txt Is Nothing Then
                                    Else
                                        If txt.Text <> "" Then
                                            If CType(Val(txt.Text), String) <> "0" Then
                                                'txtRMType = GvRowRMType.FindControl("txtrmtypcode")
                                                'If ddlPriceList.SelectedValue = "Weekend Rates 1 Night" Or ddlPriceList.SelectedValue = "Weekend Rates > 1 Night" Then
                                                '    mySqlCmd = New SqlCommand("sp_chkplwkdatenew", mySqlConn)
                                                'Else
                                                mySqlCmd = New SqlCommand("sp_chkOthpldatenew", mySqlConn)
                                                'End If
                                                mySqlCmd.CommandType = CommandType.StoredProcedure


                                                mySqlCmd.Parameters.Add(New SqlParameter("@othgrpcode", SqlDbType.VarChar, 20)).Value = CType(ddlGroupCode.Items(ddlGroupCode.SelectedIndex).Text.Trim, String)
                                                mySqlCmd.Parameters.Add(New SqlParameter("@selltype", SqlDbType.VarChar, 20)).Value = CType(ddlOtherSellingTypeCode.Items(ddlOtherSellingTypeCode.SelectedIndex).Text.Trim, String)

                                                mySqlCmd.Parameters.Add(New SqlParameter("@frmdate", SqlDbType.DateTime)).Value = CType(ObjDate.ConvertDateromTextBoxToDatabase(dpFDate.Text.Trim), Date)
                                                mySqlCmd.Parameters.Add(New SqlParameter("@todate", SqlDbType.DateTime)).Value = CType(ObjDate.ConvertDateromTextBoxToDatabase(dpTDate.Text.Trim), Date)
                                                If ViewState("OthpricelistState2") = "New" Or ViewState("OthpricelistState2") = "Copy" Then
                                                    mySqlCmd.Parameters.Add(New SqlParameter("@oplistcode", SqlDbType.VarChar, 20)).Value = ""
                                                Else
                                                    mySqlCmd.Parameters.Add(New SqlParameter("@oplistcode", SqlDbType.VarChar, 20)).Value = CType(txtPlcCode.Value.Trim, String)
                                                End If



                                                txtnights = GvRowRMType.FindControl("txt" & b + a + 2)
                                                If Not txtnights Is Nothing Then
                                                    mySqlCmd.Parameters.Add(New SqlParameter("@pkgnights", SqlDbType.Int, 20)).Value = CType(txtnights.Text.Trim, Integer)
                                                End If



                                                'If (txtPromotionCode.Text.Trim = "") Then
                                                '    mySqlCmd.Parameters.Add(New SqlParameter("@promotionid", SqlDbType.VarChar, 20)).Value = ""
                                                'Else
                                                '    mySqlCmd.Parameters.Add(New SqlParameter("@promotionid", SqlDbType.VarChar, 20)).Value = CType(txtPromotionCode.Text.Trim, String)
                                                'End If

                                                Dim paramAllowFlg As New SqlParameter
                                                paramAllowFlg.ParameterName = "@allowflg"
                                                paramAllowFlg.Direction = ParameterDirection.Output
                                                paramAllowFlg.DbType = DbType.Int64
                                                paramAllowFlg.Size = 20
                                                mySqlCmd.Parameters.Add(paramAllowFlg)

                                                Dim paramErrMsg As New SqlParameter
                                                paramErrMsg.ParameterName = "@errmsg"
                                                paramErrMsg.Direction = ParameterDirection.Output
                                                paramErrMsg.DbType = DbType.String
                                                paramErrMsg.Size = 100
                                                mySqlCmd.Parameters.Add(paramErrMsg)

                                                mySqlCmd.ExecuteNonQuery()

                                                AllowFlg = paramAllowFlg.Value
                                                ErrMsg = paramErrMsg.Value

                                                mySqlCmd = Nothing

                                                If CType(AllowFlg, String) = "1" Then
                                                    ValidatePage = False
                                                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & ErrMsg.Replace("'", " ") & "');", True)
                                                    Exit Function
                                                End If
                                            End If
                                        End If
                                    End If

                                End If
                                k = k + 1
                            Next
                        End If
                        b = j
                        n = j
                    Next

                End If
            Next

            ' End If
            ' Next


            'clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
            clsDBConnect.dbConnectionClose(mySqlConn)           'connection close
            '-----------------------------------------------------------------------***

            If flgdt = False Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Enter Dates grid should not be blank.');", True)
                SetFocus(grdDates)
                ValidatePage = False
                Exit Function
            End If
            '------------------------------------------------------------------------------------
            b = 0
            n = 0
            For Each GvRow In gv_SearchResult.Rows
                If n = 0 Then
                    For j = 0 To cnt - 5
                        If heading(j) = "From Date" Or heading(j) = "To Date" Or heading(j) = "Pkg" Then
                            '
                        Else
                            txt = GvRow.FindControl("txt" & j)
                            If txt Is Nothing Then
                            Else
                                If txt.Text <> "" Then
                                    flag = True
                                    GoTo Err
                                End If
                            End If

                        End If
                    Next
                    m = j
                Else
                    k = 0
                    For j = n To (m + n) - 1
                        If heading(k) = "From Date" Or heading(k) = "To Date" Or heading(k) = "Pkg" Then
                        Else
                            txt = GvRow.FindControl("txt" & j)
                            If txt Is Nothing Then
                            Else
                                If txt.Text <> "" Then
                                    flag = True
                                    GoTo Err
                                End If
                            End If

                        End If
                        k = k + 1
                    Next
                End If
                b = j
                n = j
            Next

Err:        If flag = False Then
                'This is commented in AlThuarya_Travels
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Atleast 1 Entry for prices should be done in net Sale.');", True)
                ValidatePage = False
                Exit Function
            End If

            flag = False
            b = 0
            n = 0
            For Each GvRow In gv_SearchResult.Rows
                If n = 0 Then
                    For j = 0 To cnt - 5
                        If heading(j) = "From Date" Or heading(j) = "To Date" Or heading(j) = "Pkg" Then
                            '
                        Else
                            txt = GvRow.FindControl("txt" & b + a + 2)
                            If txt Is Nothing Then
                            Else
                                If txt.Text = "" Then
                                    'Package days can not be left blank.
                                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Package days can not be left blank.');", True)
                                    SetFocus(txt)
                                    ValidatePage = False
                                    Exit Function
                                    'Else
                                    '    If GvRow.Cells(b + a).Text = "0" Then
                                    '' pricelistddl fnctns
                                    'End If
                                    '    'flag = True
                                    '    'ValidatePage = True
                                    '    'Exit Function
                                End If
                            End If


                            'txt = GvRow.FindControl("txt" & b + a + 2)
                            'If txt Is Nothing Then
                            'Else
                            '    If txt.Text = "" Then
                            '    End If
                            'End If
                            'txt = GvRow.FindControl("txt" & j)
                            'If txt Is Nothing Then
                            'Else
                            '    If txt.Text = "" Then
                            '    End If
                            'End If

                            'txt = GvRow.FindControl("txt" & b + a + 3)
                            'If txt Is Nothing Then
                            'Else
                            '    If txt.Text = "" Then
                            '    End If
                            'End If
                            'txt = GvRow.FindControl("txt" & b + a + 3)
                            'If txt Is Nothing Then
                            'Else
                            '    If txt.Text = "" Then
                            '        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Compalsory nights can not be left blank.');", True)
                            '        SetFocus(txt)
                            '        ValidatePage = False
                            '        Exit Function
                            '    End If
                            'End If
                        End If
                    Next
                    m = j
                Else
                    k = 0
                    For j = n To (m + n) - 1
                        If heading(k) = "From Date" Or heading(k) = "To Date" Or heading(k) = "Pkg" Then
                        Else
                            txt = GvRow.FindControl("txt" & b + a + 2)
                            If Not txt Is Nothing Then
                                If txt.Text = "" Then
                                    'Package days can not be left blank.
                                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Package days can not be left blank.');", True)
                                    SetFocus(txt)
                                    ValidatePage = False
                                    Exit Function
                                Else
                                    'ValidatePage = True
                                    'Exit Function
                                End If
                            End If

                            'txt = GvRow.FindControl("txt" & b + a + 2)
                            'If txt Is Nothing Then
                            'Else
                            '    If txt.Text = "" Then
                            '    End If
                            'End If

                            'txt = GvRow.FindControl("txt" & j)
                            'If txt Is Nothing Then
                            'Else
                            '    If txt.Text = "" Then
                            '    End If
                            'End If
                            'txt = GvRow.FindControl("txt" & b + a + 3)
                            'If txt Is Nothing Then
                            'Else
                            '    If txt.Text = "" Then
                            '        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Compalsory nights can not be left blank.');", True)
                            '        SetFocus(txt)
                            '        ValidatePage = False
                            '        Exit Function
                            '    End If
                            'End If
                            ''txt = GvRow.FindControl("txt" & b + a + 4)
                            ''If txt Is Nothing Then
                            ''Else
                            ''    If txt.Text = "" Then
                            ''    End If
                            ''End If
                        End If
                        k = k + 1
                    Next
                End If
                b = j
                n = j
            Next
            ValidatePage = True
        Catch ex As Exception
            objUtils.WritErrorLog("OthPriceList2.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
        ValidatePage = True
    End Function
#End Region
    'commented 
    '#Region " ValidateChkMarkUp "
    '    Private Function ValidateChkMarkUp() As Boolean
    '        ValidateChkMarkUp = True
    '        mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
    '        Dim AllowFlg As Integer
    '        Dim ErrMsg As String
    '        Dim GvRow As GridViewRow
    '        Dim gvMarketRow As GridViewRow
    '        Dim chksel As CheckBox
    '        Dim lblcode As Label

    '        If chkConsdierForMarkUp.Checked = True Then
    '            'check whether any other supplier is already marked up for same time period
    '            For Each GvRow In grdDates.Rows
    '                dpFDate = GvRow.FindControl("txtfromDate")
    '                dpTDate = GvRow.FindControl("txtToDate")
    '                If dpFDate.Text <> "" And dpTDate.Text <> "" Then
    '                    For Each gvMarketRow In gv_Market.Rows
    '                        chksel = gvMarketRow.FindControl("chkSelect")
    '                        lblcode = gvMarketRow.FindControl("lblcode")
    '                        If chksel.Checked = True Then
    '                            mySqlCmd = New SqlCommand("sp_OthchkExistMarkUpSuplier", mySqlConn)
    '                            mySqlCmd.CommandType = CommandType.StoredProcedure
    '                            mySqlCmd.Parameters.Add(New SqlParameter("@frmdate", SqlDbType.DateTime)).Value = CType(ObjDate.ConvertDateromTextBoxToDatabase(dpFDate.Text.Trim), Date)
    '                            mySqlCmd.Parameters.Add(New SqlParameter("@todate", SqlDbType.DateTime)).Value = CType(ObjDate.ConvertDateromTextBoxToDatabase(dpTDate.Text.Trim), Date)
    '                            mySqlCmd.Parameters.Add(New SqlParameter("@plgrpcode", SqlDbType.VarChar, 20)).Value = lblcode.Text.Trim
    '                            'If Session("OthPListFilter") <> "OTH" Then
    '                            mySqlCmd.Parameters.Add(New SqlParameter("@othgrpcode", SqlDbType.VarChar, 20)).Value = ddlGroupCode.Items(ddlGroupCode.SelectedIndex).Text
    '                            mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = ddlSupplierName.Items(ddlSupplierName.SelectedIndex).Value
    '                            'Else


    '                            'End If
    '                            Dim paramAllowFlg As New SqlParameter
    '                            paramAllowFlg.ParameterName = "@allowflg"
    '                            paramAllowFlg.Direction = ParameterDirection.Output
    '                            paramAllowFlg.DbType = DbType.Int64
    '                            paramAllowFlg.Size = 20
    '                            mySqlCmd.Parameters.Add(paramAllowFlg)

    '                            Dim paramErrMsg As New SqlParameter
    '                            paramErrMsg.ParameterName = "@errmsg"
    '                            paramErrMsg.Direction = ParameterDirection.Output
    '                            paramErrMsg.DbType = DbType.String
    '                            paramErrMsg.Size = 100
    '                            mySqlCmd.Parameters.Add(paramErrMsg)


    '                            mySqlCmd.ExecuteNonQuery()

    '                            AllowFlg = paramAllowFlg.Value
    '                            ErrMsg = paramErrMsg.Value

    '                            mySqlCmd = Nothing

    '                            If CType(AllowFlg, String) = "1" Then
    '                                ValidateChkMarkUp = False
    '                                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & ErrMsg.Replace("'", " ") & "');", True)
    '                                Exit Function
    '                            End If
    '                        End If
    '                    Next
    '                End If
    '            Next
    '        End If

    '    End Function

    '#End Region

    Protected Sub btnhelp_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnhelp.Click
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=OthPriceList2','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub


#Region "Public Sub fillgrd(ByVal grd As GridView, Optional ByVal blnload As Boolean = False, Optional ByVal count As Integer = 1)"
    Public Sub fillDategrd(ByVal grd As GridView, Optional ByVal blnload As Boolean = False, Optional ByVal count As Integer = 1)
        Dim lngcnt As Long
        Dim cnt As Integer
        If ViewState("OthpricelistState2") = "New" Then
            cnt = 7
        Else
            If ViewState("OthpricelistRefCode2") <> Nothing Then
                mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                strSqlQry = "select count(*) from othplisth_dates where oplistcode='" + ViewState("OthpricelistRefCode2") + "'"
                mySqlCmd = New SqlCommand(strSqlQry, mySqlConn)
                cnt = mySqlCmd.ExecuteScalar
                mySqlConn.Close()
            End If
        End If

        If blnload = True Then
            lngcnt = cnt '10
        Else
            lngcnt = count
        End If
        grd.DataSource = CreateDataSource(lngcnt)
        grd.DataBind()
        grd.Visible = True
    End Sub
#End Region

   
#Region "Private Function CreateDataSource(ByVal lngcount As Long) As DataView"
    Private Function CreateDataSource(ByVal lngcount As Long) As DataView
        Dim dt As DataTable
        Dim dr As DataRow
        Dim i As Integer
        dt = New DataTable
        dt.Columns.Add(New DataColumn("no", GetType(Integer)))
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

    Protected Sub btnAddLines_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        AddLines()
    End Sub

    Private Sub AddLines()
        Dim count As Integer
        Dim GVRow As GridViewRow
        count = grdDates.Rows.Count + 1
        Dim fDate(count) As String
        Dim tDate(count) As String
        Dim n As Integer = 0

        Try
            For Each GVRow In grdDates.Rows
                dpFDate = GVRow.FindControl("txtfromDate")
                fDate(n) = CType(dpFDate.Text, String)
                dpTDate = GVRow.FindControl("txtToDate")
                tDate(n) = CType(dpTDate.Text, String)
                n = n + 1
            Next
            fillDategrd(grdDates, False, grdDates.Rows.Count + 1)
            Dim i As Integer = n
            n = 0
            For Each GVRow In grdDates.Rows
                If n = i Then
                    Exit For
                End If
                dpFDate = GVRow.FindControl("txtfromDate")
                dpFDate.Text = fDate(n)
                dpTDate = GVRow.FindControl("txtToDate")
                dpTDate.Text = tDate(n)
                n = n + 1
            Next
        Catch ex As Exception
            objUtils.WritErrorLog("OthPriceList2.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub



    Protected Sub btnPreviousRates_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPreviousRates.Click
        Dim j As Long = 1
        Dim txt As TextBox
        Dim cnt As Long
        Dim GvRow As GridViewRow

        Dim srno As Long = 0
        Dim hotelcategory As String = ""
        j = 0
        gv_SearchResult.Visible = True
        cnt = gv_SearchResult.Columns.Count
        Dim n As Long = 0
        Dim k As Long = 0

        Dim header As Long = 0
        Dim heading(cnt + 1) As String
        Try
            For header = 0 To cnt - 6
                txt = gv_SearchResult.HeaderRow.FindControl("txtHead" & header)
                heading(header) = txt.Text
            Next
        Catch ex As Exception
            objUtils.WritErrorLog("OthPriceList1.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
        Dim arr_room(header + 1) As String
        Dim m As Long = 0
        Dim a As Long = cnt - 10
        Dim b As Long = 0

        Dim chk As HtmlInputCheckBox
        Dim cnt_checked As Long
        'Dim GvRow1 As GridViewRow
        Try
            For Each GvRow In gv_SearchResult.Rows
                chk = GvRow.FindControl("ChkSelect")
                If chk.Checked = True Then
                    cnt_checked = cnt_checked + 1
                End If
            Next
            If cnt_checked = 0 Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Select at least one row');", True)
                SetFocus(cnt_checked)
                Exit Sub
            End If


            Dim arr(3) As String
            'Dim arr_pkg(3) As String
            'Dim arr_cancdays(3) As String

            Dim room As Long = 0
            Dim row_id As Long
            'Dim pkg As Long = 0

            For Each GvRow In gv_SearchResult.Rows
                chk = GvRow.FindControl("ChkSelect")
                If n = 0 Then
                    For j = 0 To cnt - 5
                        If chk.Checked = True Then
                            row_id = GvRow.RowIndex
                            If heading(j) = "From Date" Or heading(j) = "To Date" Or heading(j) = "Pkg" Then
                                '
                            Else
                                'If pkg = 0 Then
                                '    txt = GvRow.FindControl("txt" & b + a + 1)
                                '    If txt Is Nothing Then
                                '    Else
                                '        If txt.Text <> Nothing Then
                                '            arr_pkg(pkg) = txt.Text
                                '        End If
                                '    End If

                                '    txt = GvRow.FindControl("txt" & b + a + 2)
                                '    If txt Is Nothing Then
                                '    Else
                                '        If txt.Text <> Nothing Then
                                '            arr_cancdays(pkg) = txt.Text
                                '        End If
                                '    End If

                                'End If
                                txt = GvRow.FindControl("txt" & j)
                                If txt Is Nothing Then
                                Else
                                    If txt.Text <> Nothing Then
                                        arr_room(room) = txt.Text
                                    End If
                                End If
                                'pkg = pkg + 1
                                room = room + 1
                            End If
                        End If
                    Next

                    m = j
                    'a = j
                Else
                    k = 0
                    'pkg = 0
                    For j = n To (m + n) - 1
                        If chk.Checked = True Then
                            row_id = GvRow.RowIndex
                            If heading(k) = "From Date" Or heading(k) = "To Date" Or heading(k) = "Pkg" Then
                            Else
                                'If pkg = 0 Then
                                '    txt = GvRow.FindControl("txt" & b + a + 1)
                                '    If txt Is Nothing Then
                                '    Else
                                '        If txt.Text <> Nothing Then
                                '            arr_pkg(pkg) = txt.Text
                                '        End If
                                '    End If

                                '    txt = GvRow.FindControl("txt" & b + a + 2)
                                '    If txt Is Nothing Then
                                '    Else
                                '        If txt.Text <> Nothing Then
                                '            arr_cancdays(pkg) = txt.Text
                                '        End If
                                '    End If
                                'End If
                                txt = GvRow.FindControl("txt" & j)
                                If txt Is Nothing Then
                                Else
                                    If txt.Text <> Nothing Then
                                        arr_room(room) = txt.Text
                                    End If
                                End If
                                'pkg = pkg + 1
                                room = room + 1
                            End If
                        End If
                        k = k + 1
                    Next

                End If

                b = j
                n = j
            Next
            '--------------------------------------------------------------------------------------------
            'Noe Fill Record to Cell
            room = 0
            'pkg = 0
            n = 0
            b = 0

            For Each GvRow In gv_SearchResult.Rows
                chk = GvRow.FindControl("ChkSelect")
                If n = 0 Then
                    For j = 0 To cnt - 5
                        If GvRow.RowIndex = row_id + 1 Then
                            If heading(j) = "From Date" Or heading(j) = "To Date" Or heading(j) = "Pkg" Then
                                '
                            Else
                                'txt = GvRow.FindControl("txt" & b + a + 1)
                                'If txt Is Nothing Then
                                'Else
                                '    If txt.Text <> Nothing Then
                                '        txt.Text = arr_pkg(pkg)
                                '    End If
                                'End If

                                txt = GvRow.FindControl("txt" & j)
                                If txt Is Nothing Then
                                Else
                                    txt.Text = arr_room(room)
                                End If

                                'txt = GvRow.FindControl("txt" & b + a + 2)
                                'If txt Is Nothing Then
                                'Else
                                '    If txt.Text <> Nothing Then
                                '        txt.Text = arr_cancdays(pkg)
                                '    End If
                                'End If
                                room = room + 1
                            End If
                        End If
                    Next
                    m = j
                Else
                    k = 0
                    For j = n To (m + n) - 1
                        If GvRow.RowIndex = row_id + 1 Then
                            If heading(k) = "From Date" Or heading(k) = "To Date" Or heading(k) = "Pkg" Then
                            Else
                                'txt = GvRow.FindControl("txt" & b + a + 1)
                                'If txt Is Nothing Then
                                'Else
                                '    If txt.Text <> Nothing Then
                                '        txt.Text = arr_pkg(pkg)
                                '    End If
                                'End If

                                txt = GvRow.FindControl("txt" & j)
                                If txt Is Nothing Then
                                Else
                                    txt.Text = arr_room(room)
                                End If

                                'txt = GvRow.FindControl("txt" & b + a + 2)
                                'If txt Is Nothing Then
                                'Else
                                '    If txt.Text <> Nothing Then
                                '        txt.Text = arr_cancdays(pkg)
                                '    End If
                                'End If
                                room = room + 1
                            End If
                        End If
                        k = k + 1
                    Next
                End If
                b = j
                n = j
            Next
        Catch ex As Exception
            objUtils.WritErrorLog("HeaderInfo1new.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))

        End Try
    End Sub

    Protected Sub btnselling_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnselling.Click
        Dim strentryQuery As String = "select 1  from othgrpmast where othgrpcode =(select option_selected  from reservation_parameters where param_id=" & Session("OthPListFilter") & ")" & _
                                                              " and paxcalcreqd=1"
        SqlReader = objUtils.GetDataFromReadernew(Session("dbconnectionName"), strentryQuery)
        '-------------------market common for both funtnlities-------------------------------
       
       
        '---------------------------------------------
        If SqlReader IsNot Nothing Then


            If SqlReader.HasRows = True Then
               
                ViewState.Add("OthpricelistRefCode2", txtPlcCode.Value)
                Response.Redirect("OthSellingRatePaxSlab.aspx?State=" & CType(ViewState("OthpricelistState2"), String) &
                                  "&RefCode=" & CType(ViewState("OthpricelistRefCode2"), String) & 
                                  "&CurrencyCode=" & txtCurrCode.Text & "&CurrencyName=" & txtCurrName.Text &
                                  "&SubSeasonCode=" & ddlSubSeasCode.Items(ddlSubSeasCode.SelectedIndex).Text &
                                  "&SubSeasonName=" & ddlSubSeasName.Items(ddlSubSeasName.SelectedIndex).Text, False)
            End If

        Else
            'to normal seling pages

            ViewState.Add("OthpricelistRefCode2", txtPlcCode.Value)
            'Response.Redirect("HederRsellingcode1new.aspx?State=" & CType(ViewState("HeaderinfoState1"), String) & "&RefCode=" & CType(ViewState("HeaderinfoRefCode1"), String) & "&supplier=" & ddlSuppierCD.Items(ddlSuppierCD.SelectedIndex).Text & "&suppliername=" & ddlSuppierNM.Items(ddlSuppierNM.SelectedIndex).Text & "&SupplierType=" & ddlSPTypeCD.Items(ddlSPTypeCD.SelectedIndex).Text & "&SupplierTypeName=" & ddlSPTypeNM.Items(ddlSPTypeNM.SelectedIndex).Text & "&Market=" & marketstr & "&SuppierAgent=" & ddlSupplierAgent.Items(ddlSupplierAgent.SelectedIndex).Text & "&SupplierAgentName=" & ddlSuppierAgentNM.Items(ddlSuppierAgentNM.SelectedIndex).Text & "&CurrencyCode=" & ddlCurrencyCD.Value & "&CurrencyName=" & ddlCurrencyNM.Value & "&SubSeasonCode=" & ddlSubSeas.Items(ddlSubSeas.SelectedIndex).Text & "&SubSeasonName=" & ddlSubSeasNM.Items(ddlSubSeasNM.SelectedIndex).Text & "&RevisionDate=" & dpRevsiondate.Text & "&PListCode=" & txtBlockCode.Value & "&hdnpricelist=" & hdnpricelist.Value & "&pricelist=" & ddlPriceList.SelectedValue & "&week1=" & week1 & "&week2=" & week2 & "&manual=" & manual & "&promotionname=" & ddlPromotion.Items(ddlPromotion.SelectedIndex).Text & "&promotioncode=" & txtPromotionCode.Text, False)


            Response.Redirect("OthPricelistSellingRates1.aspx?State=" & CType(ViewState("OthpricelistState2"), String) &
                              "&RefCode=" & CType(ViewState("OthpricelistRefCode2"), String) & 
                              "&CurrencyCode=" & txtCurrCode.Text & "&CurrencyName=" & txtCurrName.Text &
                              "&SubSeasonCode=" & ddlSubSeasCode.Items(ddlSubSeasCode.SelectedIndex).Text &
                              "&SubSeasonName=" & ddlSubSeasName.Items(ddlSubSeasName.SelectedIndex).Text, False)


            'Response.Redirect("TrfPricelistSellingRates.aspx?State=New&RefCode=TRPL/000015&supplier=T-000002&suppliername=FADI ECRS" & _
            '                  "&SupplierType=Transfers&SupplierTypeName=Transfers&Market=CIS;&SuppierAgent=WONINF&SupplierAgentName=" & _
            '                  "&CurrencyCode=AED&CurrencyName=DIRHAM&SubSeasonCode=ALL SEASONS&SubSeasonName=ALL", False)

        End If
    End Sub

End Class
