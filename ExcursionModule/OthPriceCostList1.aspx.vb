#Region "NameSpace"
Imports System
Imports System.Data
Imports System.Data.SqlClient
#End Region

Partial Class TransportModule_OthPriceList1
    Inherits System.Web.UI.Page

#Region "Global Declaration"
    Dim objUtils As New clsUtils
    Dim ObjDate As New clsDateTime

    Dim strSqlQry As String
    Dim mySqlCmd As SqlCommand
    Dim mySqlReader As SqlDataReader
    Dim mySqlConn As SqlConnection

    Dim gvRow1 As GridViewRow
    Dim myDataAdapter As SqlDataAdapter
#End Region

#Region "TextLock"
    Public Sub TextLock(ByVal txtbox As TextBox)
        txtbox.Attributes.Add("onKeyPress", "return chkTextLock(event)")
        txtbox.Attributes.Add("onKeyDown", "return chkTextLock1(event)")
        txtbox.Attributes.Add("onKeyUp", "return chkTextLock(event)")
    End Sub
#End Region

#Region "Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init"

    Protected Sub Page_Error(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Error

    End Sub
    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Try

            If IsPostBack = False Then
                'Session ("OthPLFilter")
                Dim strqry As String = ""
                Dim strOption As String = ""
                Dim strtitle As String = ""
                Dim strSpType As String = ""


                If (Session("OthPListFilter") <> Nothing And Session("OthPListFilter") <> "OTH") Then

                    strOption = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", Session("OthPListFilter"))

                             Dim sptype As String = ""
                    sptype = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", strSpType)
                    If sptype <> "" Then
                        ddlSPTypeName.Value = sptype
                        ddlSPType.Value = ddlSPTypeName.Items(ddlSPTypeName.SelectedIndex).Text
                    End If

                    strqry = "select othgrpcode,othgrpname from othgrpmast where active=1 And othgrpcode=(Select Option_Selected From Reservation_ParaMeters" & _
                        " Where Param_Id='" & Session("OthPListFilter") & "') order by othgrpcode"

                ElseIf Session("OthPListFilter") = "OTH" Then
                    Dim sptypeQry As String = ""
                    sptypeQry = "select othgrpcode , othgrpname  from othgrpmast where active=1 and othgrpcode in (select option_selected from reservation_parameters where param_id=1021)"
                      strtitle = "Other Service "
                    strqry = "select othgrpcode , othgrpname  from othgrpmast where active=1 and othgrpcode in (select option_selected from reservation_parameters where param_id=1021)"

                End If

                strtitle = "Excursion Cost"

                If Request.QueryString("State") = "New" Then
                    Page.Title = Page.Title + " " + "New " + strtitle + " Price List"
                ElseIf Request.QueryString("State") = "Copy" Then
                    Page.Title = Page.Title + " " + "Copy " + strtitle + "  Price List"
                ElseIf Request.QueryString("State") = "Edit" Then
                    Page.Title = Page.Title + " " + "Edit " + strtitle + "  Price List"
                ElseIf Request.QueryString("State") = "View" Then
                    Page.Title = Page.Title + " " + "View" + strtitle + "  Price List"
                ElseIf Request.QueryString("State") = "Delete" Then
                    Page.Title = Page.Title + " " + "Delete " + strtitle + "  Price List"
                End If

                Dim s As String = ""
                ViewState.Add("OthpricelistState", Request.QueryString("State"))
                ViewState.Add("OthpricelistRefCode", Request.QueryString("RefCode"))

                txtconnection.Value = Session("dbconnectionName")
              
                'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSPType, "partycode", "partyname", "select partycode,partyname from partymast  where active=1 and partymast.sptypecode <> (select option_selected from reservation_parameters where param_id ='458') order by partyname", True)
                'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSPTypeName, "partyname", "partycode", "select partycode,partyname from partymast where active=1 and partymast.sptypecode <> (select option_selected from reservation_parameters where param_id ='458')order by partyname", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSPType, "partycode", "partyname", "select partycode,partyname from partymast where   active=1 and partymast.sptypecode not in (select option_selected from reservation_parameters where param_id IN('458','500')) order by partycode ", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSPTypeName, "partyname", "partycode", "select partycode,partyname from partymast where active=1 and partymast.sptypecode not in (select option_selected from reservation_parameters where param_id IN('458','500')) order by partyname", True)


           
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlexccode, "othtypcode", "othtypname", "select othtypcode,othtypname from othtypmast inner join othgrpmast on othgrpmast.othgrpcode =othtypmast.othgrpcode  where othtypmast.active=1 and othmaingrpcode=(select option_selected from reservation_parameters where param_id ='" & Session("OthPListFilter") & "') order by othgrpname", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlexcname, "othtypname", "othtypcode", "select othtypcode,othtypname from othtypmast inner join othgrpmast on othgrpmast.othgrpcode =othtypmast.othgrpcode  where othtypmast.active=1 and othmaingrpcode=(select option_selected from reservation_parameters where param_id ='" & Session("OthPListFilter") & "') order by othgrpname", True)



                'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlGroupCode, "othgrpcode", "othgrpname", "select othgrpcode , othgrpname from othgrpmast  where active=1 and othmaingrpcode=(select option_selected from reservation_parameters where param_id ='" & Session("OthPListFilter") & "') order by othgrpcode", True)
                'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlGroupName, "othgrpname", "othgrpcode", "select othgrpcode , othgrpname from othgrpmast  where active=1 and othmaingrpcode=(select option_selected from reservation_parameters where param_id ='" & Session("OthPListFilter") & "') order by othgrpname", True)


                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlGroupCode, "othgrpcode", "othgrpname", "select othgrpcode , othgrpname from othgrpmast  where active=1 and othmaingrpcode in (select option_selected from reservation_parameters where param_id  in ('1021','1105')) order by othgrpcode", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlGroupName, "othgrpname", "othgrpcode", "select othgrpcode , othgrpname from othgrpmast  where active=1 and othmaingrpcode in (select option_selected from reservation_parameters where param_id in ('1021','1105')) order by othgrpname", True)


               

                btnCancel.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to cancel?')==false)return false;")

                If ViewState("OthpricelistState") = "New" Then
                    btnGenerate.Attributes.Add("onclick", "return FormValidation('New')")
                    Dim obj As New EncryptionDecryption

                    SetFocus(ddlSPType)

                    lblHeading.Text = "Add New " + strtitle + " Price List"

                    'check, becz it gets populated if any value persists in session /qs
                    If Session("ClearPrice") = "Yes" Then
                        If Request.QueryString("SptypeCode") <> Nothing And Request.QueryString("SptypeName") <> Nothing Then
                            s = ""
                            s = Request.QueryString("SptypeName")
                            ddlSPType.Value = obj.Decrypt(s.Replace(" ", "+"), "&%#@?,:*")
                            s = ""
                            s = Request.QueryString("SptypeCode")
                            ddlSPTypeName.Value = obj.Decrypt(s.Replace(" ", "+"), "&%#@?,:*")
                        End If

                        If Request.QueryString("Airportcd") <> Nothing And Request.QueryString("Airportnm") <> Nothing Then
                            s = ""
                            s = Request.QueryString("Airportnm")
                            ddlexccode.Value = obj.Decrypt(s.Replace(" ", "+"), "&%#@?,:*")
                            s = ""
                            s = Request.QueryString("Airportcd")
                            ddlexcname.Value = obj.Decrypt(s.Replace(" ", "+"), "&%#@?,:*")
                        End If

                        If Request.QueryString("GroupCode") <> Nothing And Request.QueryString("GroupName") <> Nothing Then
                            s = ""
                            s = Request.QueryString("GroupName")
                            ddlGroupCode.Value = obj.Decrypt(s.Replace(" ", "+"), "&%#@?,:*")
                            s = ""
                            s = Request.QueryString("GroupCode")
                            ddlGroupName.Value = obj.Decrypt(s.Replace(" ", "+"), "&%#@?,:*")
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

                    End If

                    

                    btnGenerate.Attributes.Add("onclick", "return FormValidation('Edit')")
                ElseIf ViewState("OthpricelistState") = "Edit" Or ViewState("OthpricelistState") = "Copy" Then
                    btnGenerate.Attributes.Add("onclick", "return FormValidation('Edit')")
                    'SetFocus(ddlMarketCode)
                    ShowRecord(ViewState("OthpricelistRefCode"))
                    lblHeading.Text = "Edit " + strtitle + " Price List"
                ElseIf ViewState("OthpricelistState") = "View" Then
                    ShowRecord(ViewState("OthpricelistRefCode"))
                ElseIf ViewState("OthpricelistState") = "Delete" Then
                    ShowRecord(ViewState("OthpricelistRefCode"))
                End If
                If ViewState("OthpricelistState") = "Copy" Then
                    txtPlcCode.Value = ""
                End If
                DisableAllControls()
                TextLock(txtCurrCode)
                TextLock(txtCurrName)

                Dim typ As Type
                typ = GetType(DropDownList)

                'If (Page.ClientScript.IsClientScriptIncludeRegistered(typ, "TADDScript.js")) = False Then
                '    Page.ClientScript.RegisterClientScriptInclude(typ, "TADDScript.js", "TADDScript.js")

                '    ddlSPType.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                '    ddlSPTypeName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")

                '    ddlexccode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                '    ddlexcname.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")


                '    ddlGroupCode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                '    ddlGroupName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")

                'End If
                btnCancel.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to cancel?')==false)return false;")
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("OthPricesList1.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#End Region

#Region "Private Sub DisableAllControls()"
    Private Sub DisableAllControls()
        If ViewState("OthpricelistState") = "New" Then
        ElseIf ViewState("OthpricelistState") = "Edit" Or ViewState("OthpricelistState") = "Copy" Then
            ddlSPType.Disabled = False
            ddlSPTypeName.Disabled = False

            ddlGroupCode.Disabled = False
            ddlGroupName.Disabled = False

            ddlexccode.Disabled = False
            ddlexcname.Disabled = False

            txtRemark.Disabled = False


        ElseIf ViewState("OthpricelistState") = "Delete" Or ViewState("OthpricelistState") = "View" Then
            ddlexccode.Disabled = True
            ddlexcname.Disabled = True

            ddlGroupCode.Disabled = True
            ddlGroupName.Disabled = True
            txtRemark.Disabled = True
            ddlSPType.Disabled = True
            ddlSPTypeName.Disabled = True

        End If

        txtCurrCode.Enabled = False
        txtCurrName.Enabled = False

    End Sub
#End Region


#Region "Private Sub ShowRecord(ByVal RefCode As String)"
    Private Sub ShowRecord(ByVal RefCode As String)
        Try
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            mySqlCmd = New SqlCommand("Select * from exccplist_header Where eplistcode='" & RefCode & "'", mySqlConn)
            mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
            If mySqlReader.HasRows Then
                If mySqlReader.Read() = True Then


                    If IsDBNull(mySqlReader("eplistcode")) = False Then
                        txtPlcCode.Value = mySqlReader("eplistcode")
                    End If

                    If IsDBNull(mySqlReader("partycode")) = False Then
                        ddlSPTypeName.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "exccplist_header", "partycode", "eplistcode", mySqlReader("eplistcode"))
                        ddlSPType.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "partymast", "partyname", "partycode", ddlSPTypeName.Value)

                    End If

                    If IsDBNull(mySqlReader("gpcode")) = False Then
                        ddlGroupName.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "exccplist_header", "gpcode", "eplistcode", mySqlReader("eplistcode"))
                        ddlGroupCode.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "othgrpmast", "othgrpname", "othgrpcode", ddlGroupName.Value)

                    End If




                    If mySqlReader("approved") = "1" Then
                        chkapprove.Checked = True
                    Else
                        chkapprove.Checked = False
                    End If



                    'check
                    'ddlexcname.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "excplist_header", "airportcode", "tplistcode", mySqlReader("tplistcode"))
                    'ddlexccode.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "airportbordersmaster", "airportbordername", "airportbordercode", ddlexcname.Value)








                    If ddlexccode.Value <> "[Select]" Then


                    End If


                    If IsDBNull(mySqlReader("currcode")) = False Then
                        txtCurrCode.Text = mySqlReader("currcode")
                        txtCurrName.Text = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "currmast", "currname", "currcode", mySqlReader("currcode"))
                    End If

                    If IsDBNull(mySqlReader("remarks")) = False Then
                        txtRemark.Value = mySqlReader("remarks")
                    End If
                    If ViewState("OthsercostpricelistState") <> "Copy" Then

                    End If


                End If



            End If


        Catch ex As Exception
            objUtils.WritErrorLog("excPriceList1.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            clsDBConnect.dbCommandClose(mySqlCmd)
            clsDBConnect.dbReaderClose(mySqlReader)
            clsDBConnect.dbConnectionClose(mySqlConn)
        End Try
    End Sub
#End Region

#Region " Validate Page"
    Public Function ValidatePage() As Boolean
        Dim strValue As String = ""
        Try

            If ddlSPType.Items(ddlSPType.SelectedIndex).Text = "[Select]" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Party Code can not be blank.');", True)
                '   ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + ddlSPTypeCD.ClientID + "');", True)
                Return False
                Exit Function
            End If


            If ddlGroupCode.Items(ddlGroupCode.SelectedIndex).Text = "[Select]" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Group Code can not be blank.');", True)
                '   ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + ddlSupplierAgent.ClientID + "');", True)
                Return False
                Exit Function
            End If

            'If ddlGroupCode.Items(ddlGroupCode.SelectedIndex).Text <> "[Select]" Then
            '    Dim ds As New DataSet
            '    Dim dt As New DataTable
            '    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
            '    myDataAdapter = New SqlDataAdapter(strSqlQry, mySqlConn)
            '    myDataAdapter.Fill(ds)

            '    ds = objUtils.GetDataFromDatasetnew(Session("dbconnectionName"), "Exec sp_checkmy_grid '" & ddlGroupCode.Items(ddlGroupCode.SelectedIndex).Text & "' ")



            '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Group Code can not be blank.');", True)
            '    '   ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + ddlSupplierAgent.ClientID + "');", True)
            '    Return False
            '    Exit Function
            'End If

            ValidatePage = True

        Catch ex As Exception
            objUtils.WritErrorLog("OthPriceCostList1.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            ValidatePage = False
        End Try

    End Function
#End Region

#Region "Protected Sub btnGenerate_Click(ByVal sender As Object, ByVal e As System.EventArgs)"
    Protected Sub btnGenerate_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim obj As New EncryptionDecryption
        Try

            'If CType(ViewState("OthpricelistState"), String) = "New" Then
            If ValidatePage() = False Then
                Exit Sub
            End If
            'End If


            Dim SptypeCode As String = obj.Encrypt(CType(ddlSPType.Items(ddlSPType.SelectedIndex).Text, String), "&%#@?,:*")
            Dim SptypeName As String = obj.Encrypt(CType(ddlSPTypeName.Items(ddlSPTypeName.SelectedIndex).Text, String), "&%#@?,:*")

            Dim PlcCode As String = obj.Encrypt(CType(txtPlcCode.Value, String), "&%#@?,:*")
            Dim Groupcd As String = obj.Encrypt(CType(ddlGroupCode.Items(ddlGroupCode.SelectedIndex).Text, String), "&%#@?,:*")
            Dim Groupnm As String = obj.Encrypt(CType(ddlGroupName.Items(ddlGroupName.SelectedIndex).Text, String), "&%#@?,:*")

            Dim currcode As String = obj.Encrypt(CType(txtCurrCode.Text, String), "&%#@?,:*")
            Dim currname As String = obj.Encrypt(CType(txtCurrName.Text, String), "&%#@?,:*")


            Dim exccode As String = obj.Encrypt(CType(ddlexccode.Items(ddlexccode.SelectedIndex).Text, String), "&%#@?,:*")
            Dim excname As String = obj.Encrypt(CType(ddlexcname.Items(ddlexcname.SelectedIndex).Text, String), "&%#@?,:*")


            '"&SptypeCode=" & ViewState("TransferpricelistRefCode") &
            Session("sessionRemark") = txtRemark.Value
            'vij


            Response.Redirect("OthPriceCostList2.aspx?&State=" & ViewState("OthpricelistState") & "&RefCode=" & ViewState("OthpricelistRefCode") &
                              "&SptypeCode=" & SptypeCode & "&SptypeName=" & SptypeName & "&PlcCode=" & PlcCode & "&currcode=" & currcode &
                              "&currname=" & currname & "&GroupCode=" & Groupcd & "&GroupName=" & Groupnm & "&exccode=" & exccode &
                              "&Airportnm=" & excname & "&Acvtive=" & IIf(ChkActive.Checked = True, 1, 0) & "&approved=" & IIf(chkapprove.Checked = True, 1, 0), False)


            '*********************************          **********************      ***************************
        Catch ex As Exception
            objUtils.WritErrorLog("OthPriceCostList1.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#End Region


#Region "Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs)"
    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Session("sessionRemark") = Nothing
        Session("GV_OthHotelData") = Nothing
        ViewState("OthpricelistRefCode") = Nothing
        ViewState("OthpricelistState") = Nothing
        ' Response.Redirect("OtherServicesCostPriceListSearch.aspx")
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", "window.close();", True)
    End Sub
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack() = False Then

            ViewState.Add("OthpricelistState", Request.QueryString("State"))
            ViewState.Add("OthpricelistRefCode", Request.QueryString("RefCode"))


            Dim supagentcode As String
            supagentcode = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id='1021'")

        Else

           

        End If


    End Sub

    Protected Sub btnhelp_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=OthPriceList1','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub

    Protected Sub Page_LoadComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LoadComplete

    End Sub

    Public Sub New()

    End Sub
End Class
