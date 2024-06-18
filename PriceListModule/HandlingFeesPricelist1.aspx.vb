#Region "NameSpace"
Imports System
Imports System.Data
Imports System.Data.SqlClient
#End Region
Partial Class PriceListModule_HandlingFeesPricelist1
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


    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Try

            If IsPostBack = False Then
                'Session ("OthPLFilter")
                Dim strqry As String = ""
                Dim strOption As String = ""
                Dim strtitle As String = ""
                Dim strSpType As String = ""


                     strOption = "HFEES"
                    Select Case strOption
                        Case "CAR RENTAL"
                            strtitle = "Car Rental"
                            strSpType = "1031,1039"
                            'lblHeading.Text = "Car Rental Selling Formula"
                        Case "VISA"
                            strtitle = "Visa "
                            strSpType = "1032,1039"
                            'lblHeading.Text = "Visa Selling Formula"

                        Case "EXC"
                            strtitle = "Excursion  "
                            strSpType = "1033,1039"
                            'lblHeading.Text = "Excursion Selling Formula"
                        Case "MEALS"
                            strtitle = "Restaurant  "
                            strSpType = "1034,1039"
                            'lblHeading.Text = "Restaurant Selling Formula"
                        Case "GUIDES"
                            strtitle = "Guide  "
                            strSpType = "1035,1039"
                            'lblHeading.Text = "Guide Selling Formula"
                        Case "ENTRANCE"
                            strtitle = "Entrance "
                            strSpType = "1036,1039"
                            'lblHeading.Text = "Guide Selling Formula"
                        Case "JEEPWADI"
                            strtitle = "Jeepwadi "
                            strSpType = "1037,1039"
                            'lblHeading.Text = "Guide Selling Formula"

                        Case "HFEES"
                            strtitle = "Handling Fee"
                            strSpType = "1041,1039"

                        Case "AIRPORTMA"
                            strtitle = "Airport Meet & Assist"
                            strSpType = "1042,1039"
                    End Select
                  
                strqry = "select othgrpcode,othgrpname from othgrpmast where active=1 And othgrpcode='HFEES' order by othgrpcode"

              
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
                'Dim supagentcode As String
                'Dim strSPType As String = "select sptypecode,sptypename from sptypemast where active=1 and sptypecode =(select option_selected  from " & _
                '" reservation_parameters where param_id=564)  order by sptypecode"

                'Dim strSPType As String = "select sptypecode,sptypename from sptypemast where active=1 "



                    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlGroupCode, "othgrpcode", "othgrpname", strqry, True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlGroupName, "othgrpname", "othgrpcode", strqry, True)
                'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlGroupCode, "othgrpcode", "othgrpname", "select othgrpcode,othgrpname from othgrpmast where active=1 And othgrpcode=(Select Option_Selected From Reservation_ParaMeters Where Param_Id='1001') order by othgrpcode", True)





                Dim default_group As String
                default_group = ""
                default_group = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", CType("1108", String))
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlOtherSell, "othsellcode", "othsellname", "select othsellcode,othsellname from othsellmast where active=1 and othertype='" & default_group & "' order by othsellcode", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlOtherSellName, "othsellname", "othsellcode", "select othsellname,othsellcode from othsellmast where active=1 and othertype='" & default_group & "' order by othsellname", True)


              
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSubSeasCode, "subseascode", "subseasname", "select subseascode,subseasname from subseasmast where active=1 order by subseascode", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSubSeasName, "subseasname", "subseascode", "select subseasname,subseascode from subseasmast where active=1 order by subseasname", True)


                If Session("OthPListFilter") <> "OTH" Then
                    ddlGroupCode.SelectedIndex = 0
                    ddlGroupName.SelectedIndex = 0
                    ddlGroupCode.Disabled = True
                    ddlGroupName.Disabled = True
                End If

                'supagentcode = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id='520'")
                'If supagentcode <> "" Then
                '    ddlSupplierAgentName.Value = CType(supagentcode, String)
                '    ddlSupplierAgentCode.Value = ddlSupplierAgentName.Items(ddlSupplierAgentName.SelectedIndex).Text
                'End If

                btnCancel.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to cancel?')==false)return false;")

                If ViewState("OthpricelistState") = "New" Then
                    btnGenerate.Attributes.Add("onclick", "return FormValidation('New')")
                    Dim obj As New EncryptionDecryption



                    lblHeading.Text = "Add New " + strtitle + " Price List"

                    'check, becz it gets populated if any value persists in session /qs
                    If Session("ClearPrice") = "Yes" Then
                        If Request.QueryString("SptypeCode") <> Nothing And Request.QueryString("SptypeName") <> Nothing Then
                            s = ""
                            s = Request.QueryString("SptypeName")
                                ' ddlSPType.Value = obj.Decrypt(s.Replace(" ", "+"), "&%#@?,:*")
                            s = ""
                            s = Request.QueryString("SptypeCode")
                                ' ddlSPTypeName.Value = obj.Decrypt(s.Replace(" ", "+"), "&%#@?,:*")
                        End If


                 
                        If Request.QueryString("othsellcurrCode") <> Nothing Then
                            s = ""
                            s = Request.QueryString("othsellcurrCode")
                            TxtSellCurr.Text = obj.Decrypt(s.Replace(" ", "+"), "&%#@?,:*")
                        
                        End If





                        If Request.QueryString("othsellconvrate") <> Nothing Then
                            s = ""
                            s = Request.QueryString("othsellconvrate")
                            TxtSellConvRate.Text = obj.Decrypt(s.Replace(" ", "+"), "&%#@?,:*")

                        End If





                        If Request.QueryString("othsellcode") <> Nothing And Request.QueryString("othsellname") <> Nothing Then
                            s = ""
                            s = Request.QueryString("othsellname")
                            ddlOtherSell.Value = obj.Decrypt(s.Replace(" ", "+"), "&%#@?,:*")
                            s = ""
                            s = Request.QueryString("othsellcode")
                            ddlOtherSellName.Value = obj.Decrypt(s.Replace(" ", "+"), "&%#@?,:*")
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



                        If Request.QueryString("Acvtive") <> Nothing Then
                            If Request.QueryString("Acvtive") = "1" Then
                                ChkActive.Checked = True
                            ElseIf Request.QueryString("Acvtive") = "0" Then
                                ChkActive.Checked = False
                            End If
                        End If

                    End If
                    'If ddlSPType.Value <> "[Select]" Then
                    '    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSupplierCode, "partycode", "partyname", "select partycode, partyname from partymast where active=1 and sptypecode='" & ddlSPType.Items(ddlSPType.SelectedIndex).Text & "' order by partycode", True, ddlSupplierCode.Value)
                    '    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSupplierName, "partyname", "partycode", "select partyname,partycode from partymast where active=1 and sptypecode='" & ddlSPType.Items(ddlSPType.SelectedIndex).Text & "' order by partyname", True, ddlSupplierName.Value)
                    'Else
                    '    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSupplierCode, "partycode", "partyname", "select partycode, partyname from partymast where active=1 order by partycode", True, ddlSupplierCode.Value)
                    '    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSupplierName, "partyname", "partycode", "select partyname,partycode from partymast where active=1 order by partyname", True, ddlSupplierName.Value)

                    'End If


                    btnGenerate.Attributes.Add("onclick", "return FormValidation('Edit')")
                ElseIf ViewState("OthpricelistState") = "Edit" Or ViewState("OthpricelistState") = "Copy" Then
                    btnGenerate.Attributes.Add("onclick", "return FormValidation('Edit')")
                    'SetFocus(ddlMarketCode)
                    ShowRecord(ViewState("OthpricelistRefCode"))
                    '  ShowMarket(CType(ViewState("OthpricelistRefCode"), String))
                    lblHeading.Text = "Edit " + strtitle + " Price List"
                ElseIf ViewState("OthpricelistState") = "View" Then
                ElseIf ViewState("OthpricelistState") = "Delete" Then
                End If
                If ViewState("OthpricelistState") = "Copy" Then
                    txtPlcCode.Value = ""
                End If
                DisableAllControls()
               
                Dim typ As Type
                typ = GetType(DropDownList)

                If (Page.ClientScript.IsClientScriptIncludeRegistered(typ, "TADDScript.js")) = False Then
                    Page.ClientScript.RegisterClientScriptInclude(typ, "TADDScript.js", "TADDScript.js")

                        'ddlSPType.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                        ' ddlSPTypeName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")

                              ddlGroupCode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlGroupName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")




                   



                    ddlOtherSell.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlOtherSellName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")



                    ddlSubSeasCode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlSubSeasName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                End If
                btnCancel.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to cancel?')==false)return false;")
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("OthPricesList1.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub


#Region "Private Sub DisableAllControls()"
    Private Sub DisableAllControls()
        If ViewState("OthpricelistState") = "New" Then
        ElseIf ViewState("OthpricelistState") = "Edit" Or ViewState("OthpricelistState") = "Copy" Then
            'ddlSPType.Disabled = True
            'ddlSPTypeName.Disabled = True

            

            'ddlGroupCode.Disabled = True
            'ddlGroupName.Disabled = True
        ElseIf ViewState("OthpricelistState") = "Delete" Or ViewState("OthpricelistState") = "View" Then
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
                        ddlOtherSellName.Value = mySqlReader("othsellcode")
                        ddlOtherSell.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "othsellmast", "othsellname", "othsellcode", mySqlReader("othsellcode"))
                    End If

                    'If IsDBNull(mySqlReader("plgrpcode")) = False Then
                    '    ddlMarketName.Value = mySqlReader("plgrpcode")
                    '    ddlMarketCode.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "plgrpmast", "plgrpname", "plgrpcode", mySqlReader("plgrpcode"))
                    'End If



                    If IsDBNull(mySqlReader("currcode")) = False Then
                        TxtSellCurr.Text = mySqlReader("currcode")
                        TxtSellConvRate.Text = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "currmast", "convrate", "currcode", mySqlReader("currcode"))
                    End If
                 


                    If IsDBNull(mySqlReader("subseascode")) = False Then
                        ddlSubSeasName.Value = mySqlReader("subseascode")
                        ddlSubSeasCode.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "subseasmast", "subseasname", "subseascode", mySqlReader("subseascode"))
                    End If
                    If IsDBNull(mySqlReader("remarks")) = False Then
                        txtRemark.Value = mySqlReader("remarks")
                    End If
                    If ViewState("OthsercostpricelistState") <> "Copy" Then
                        'If IsDBNull(mySqlReader("frmdate")) = False Then
                        '    dpFromdate.txtDate.Text = Format(CType(mySqlReader("frmdate"), Date), "dd/MM/yyyy")
                        '    Session("sessionFrmdate") = dpFromdate.txtDate.Text
                        'End If
                        'If IsDBNull(mySqlReader("todate")) = False Then
                        '    dpToDate.txtDate.Text = Format(CType(mySqlReader("todate"), Date), "dd/MM/yyyy")
                        '    Session("sessionTodate") = dpToDate.txtDate.Text
                        'End If
                    End If



                    If IsDBNull(mySqlReader("active")) = False Then
                        If mySqlReader("active") = "1" Then
                            ChkActive.Checked = True
                        ElseIf mySqlReader("active") = "0" Then
                            ChkActive.Checked = False
                        End If
                    End If
                End If

                If IsDBNull(mySqlReader("approve")) = False Then
                    If mySqlReader("approve") = 1 Then
                        chkapprove.Checked = True
                    ElseIf mySqlReader("approve") = 0 Then
                        chkapprove.Checked = False
                    End If
                End If

                'If IsDBNull(mySqlReader("considerformarkup")) = False Then
                '    If mySqlReader("considerformarkup") = 1 Then
                '        chkConsdierForMarkUp.Checked = True
                '    ElseIf mySqlReader("considerformarkup") = 0 Then
                '        chkConsdierForMarkUp.Checked = False
                '    End If
                'End If




                clsDBConnect.dbCommandClose(mySqlCmd)
                clsDBConnect.dbReaderClose(mySqlReader)
                clsDBConnect.dbConnectionClose(mySqlConn)


            End If


          
            'mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open

            'mySqlCmd = New SqlCommand("select * from othplisth_convrates where oplistcode='" & RefCode & "'", mySqlConn)
            'mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
            'If mySqlReader.HasRows Then
            '    If mySqlReader.Read() = True Then
            '        If IsDBNull(mySqlReader("sellcode")) = False Then

            '            If IsDBNull(mySqlReader("sellcode")) = False Then
            '                ddlOtherSellName.Value = mySqlReader("sellcode")
            '                ddlOtherSell.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "othsellmast", "othsellname", "othsellcode", mySqlReader("sellcode"))

            '            End If


            '            If IsDBNull(mySqlReader("sellcurrcode")) = False Then
            '                TxtSellCurr.Text = mySqlReader("sellcurrcode")
            '            End If



            '            If IsDBNull(mySqlReader("convrate")) = False Then
            '                TxtSellConvRate.Text = mySqlReader("convrate")
            '            End If
            '        End If
            '    End If
            'End If

            'clsDBConnect.dbCommandClose(mySqlCmd)
            'clsDBConnect.dbReaderClose(mySqlReader)
            'clsDBConnect.dbConnectionClose(mySqlConn)




        Catch ex As Exception
            objUtils.WritErrorLog("TrfPriceList1.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            clsDBConnect.dbCommandClose(mySqlCmd)
            clsDBConnect.dbReaderClose(mySqlReader)
            clsDBConnect.dbConnectionClose(mySqlConn)
        End Try
    End Sub
#End Region

    '#Region "Private Sub FillGridMarket(ByVal strorderby As String, Optional ByVal strsortorder As String = 'ASC')"
    '    Private Sub FillGridMarket(ByVal strorderby As String, Optional ByVal strsortorder As String = "ASC")
    '        Dim myDS As New DataSet
    '        gv_Market.Visible = True
    '        If gv_Market.PageIndex < 0 Then
    '            gv_Market.PageIndex = 0
    '        End If
    '        strSqlQry = ""
    '        Try
    '            strSqlQry = "select plgrpcode,plgrpname from plgrpmast where active=1 "
    '            strSqlQry = strSqlQry & " ORDER BY " & strorderby & " " & strsortorder
    '            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
    '            myDataAdapter = New SqlDataAdapter(strSqlQry, mySqlConn)
    '            myDataAdapter.Fill(myDS)
    '            gv_Market.DataSource = myDS

    '            If myDS.Tables(0).Rows.Count > 0 Then
    '                gv_Market.DataBind()
    '                txtrowcnt.Value = gv_Market.Rows.Count
    '            Else
    '                gv_Market.DataBind()
    '            End If
    '        Catch ex As Exception
    '            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
    '            objUtils.WritErrorLog("OthPriceList1.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
    '        Finally
    '        End Try
    '    End Sub
    '#End Region

    '#Region "Private Sub ShowMarket(ByVal RefCode As String)"
    '    Private Sub ShowMarket(ByVal RefCode As String)
    '        Try
    '            Dim chkSel As CheckBox
    '            Dim lblcode As Label
    '            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
    '            mySqlCmd = New SqlCommand("Select * from othplisth_market  Where oplistcode='" & RefCode & "'", mySqlConn)
    '            mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
    '            'If mySqlReader.HasRows Then
    '            '    While mySqlReader.Read()
    '            '        If IsDBNull(mySqlReader("plgrpcode")) = False Then
    '            '            For Each Me.gvRow1 In gv_Market.Rows
    '            '                chkSel = gvRow1.FindControl("chkSelect")
    '            '                lblcode = gvRow1.FindControl("lblcode")
    '            '                If CType(mySqlReader("plgrpcode"), String) = CType(lblcode.Text, String) Then
    '            '                    chkSel.Checked = True
    '            '                    Exit For
    '            '                End If
    '            '            Next
    '            '        End If
    '            '    End While
    '            'End If

    '        Catch ex As Exception
    '            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
    '            objUtils.WritErrorLog("OthPricelistSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
    '        Finally
    '            clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
    '            clsDBConnect.dbReaderClose(mySqlReader)             ' sql reader disposed    
    '            clsDBConnect.dbConnectionClose(mySqlConn)           'connection close           
    '        End Try
    '    End Sub
    '#End Region


#Region " Validate Page"
    Public Function ValidatePage() As Boolean
        Dim strValue As String = ""
        Try

            If ddlOtherSell.Items(ddlOtherSell.SelectedIndex).Text = "[Select]" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Selling Type can not be blank.');", True)
                '  ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + ddlSubSeas.ClientID + "');", True)
                ValidatePage = False


                Exit Function
            End If
            If ddlSubSeasCode.Items(ddlSubSeasCode.SelectedIndex).Text = "[Select]" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Sub season can not be blank.');", True)
                '  ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + ddlSubSeas.ClientID + "');", True)
                ValidatePage = False
                Exit Function
            End If

            ValidatePage = True
        Catch ex As Exception
            objUtils.WritErrorLog("HandlingFeesPricelist1.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Function
#End Region

#Region "Protected Sub btnGenerate_Click(ByVal sender As Object, ByVal e As System.EventArgs)"
    Protected Sub btnGenerate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGenerate.Click
        Dim obj As New EncryptionDecryption
        Try

            'If CType(ViewState("OthpricelistState"), String) = "New" Then
            If ValidatePage() = False Then
                Exit Sub
            End If
            'End If


        
            
            Dim PlcCode As String = obj.Encrypt(CType(txtPlcCode.Value, String), "&%#@?,:*")
            Dim GroupCode As String = obj.Encrypt(CType(ddlGroupCode.Items(ddlGroupCode.SelectedIndex).Text, String), "&%#@?,:*")
            Dim GroupName As String = obj.Encrypt(CType(ddlGroupName.Items(ddlGroupName.SelectedIndex).Text, String), "&%#@?,:*")
           
            Dim SubSeasCode As String = obj.Encrypt(CType(ddlSubSeasCode.Items(ddlSubSeasCode.SelectedIndex).Text, String), "&%#@?,:*")
            Dim SubSeasName As String = obj.Encrypt(CType(ddlSubSeasName.Items(ddlSubSeasName.SelectedIndex).Text, String), "&%#@?,:*")
           


            Dim sellingType As String = obj.Encrypt(CType(ddlOtherSell.Items(ddlOtherSell.SelectedIndex).Text, String), "&%#@?,:*")
            Dim sellingtypeName As String = obj.Encrypt(CType(ddlOtherSellName.Items(ddlOtherSellName.SelectedIndex).Text, String), "&%#@?,:*")


            Dim sellcurr As String = obj.Encrypt(CType(TxtSellCurr.Text, String), "&%#@?,:*")
            Dim sellconvrate As String = obj.Encrypt(CType(TxtSellConvRate.Text, String), "&%#@?,:*")




            Dim chksel As CheckBox

            Dim lblcode As Label

            'For Each Me.gvRow1 In gv_Market.Rows
            '    chksel = gvRow1.FindControl("chkSelect")
            '    lblcode = gvRow1.FindControl("lblcode")
            '    If chksel.Checked = True Then
            '        marketstr = marketstr + ";" + lblcode.Text
            '    End If
            'Next


          
            'Dim frmdate As String = obj.Encrypt(dpFromdate.txtDate.Text, "&%#@?,:*")
            'Dim todate As String = obj.Encrypt(dpToDate.txtDate.Text, "&%#@?,:*")
            '"&SptypeCode=" & ViewState("TransferpricelistRefCode") &
            Session("sessionRemark") = txtRemark.Value
            Response.Redirect("HandlingFeesPricelist2.aspx?&State=" & ViewState("OthpricelistState") & "&RefCode=" & ViewState("OthpricelistRefCode") &
                               "&PlcCode=" & PlcCode & "&othsellcode=" & sellingType & "&othsellname=" & sellingtypeName & "&othsellcurrCode=" & sellcurr & "&othsellconvrate=" & sellconvrate &
                              "&SubSeasCode=" & SubSeasCode &
                              "&SubSeasName=" & SubSeasName & "&Acvtive=" & IIf(ChkActive.Checked = True, 1, 0) & "&approved=" & IIf(chkapprove.Checked = True, 1, 0) &
                              "&GroupCode=" & GroupCode & "&GroupName=" & GroupName, False)
            '"&MarketCode=" & MarketCode & "&MarketName=" & MarketName & "&GroupCode=" & GroupCode & "&GroupName=" & GroupName &"&frmdate=" & frmdate & "&todate=" & todate
            '*********************************          **********************      ***************************
        Catch ex As Exception
            objUtils.WritErrorLog("OthPriceList1.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
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

            'Dim sptype As String = ""
            'sptype = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", "564")
            'If sptype <> "" Then
            '    ddlSPTypeName.Value = sptype
            '    ddlSPType.Value = ddlSPTypeName.Items(ddlSPTypeName.SelectedIndex).Text
            'End If
            'ddlGroupCode.SelectedIndex = 0
          


            'If ddlSPType.Value <> "[Select]" Then
            '    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSupplierCode, "partycode", "partyname", "select partycode, partyname from partymast where active=1 and sptypecode='" & ddlSPType.Items(ddlSPType.SelectedIndex).Text & "' order by partycode", True, ddlSupplierCode.Value)
            '    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSupplierName, "partyname", "partycode", "select partyname,partycode from partymast where active=1 and sptypecode='" & ddlSPType.Items(ddlSPType.SelectedIndex).Text & "' order by partyname", True, ddlSupplierName.Value)
            'Else
            '    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSupplierCode, "partycode", "partyname", "select partycode, partyname from partymast where active=1 order by partycode", True, ddlSupplierCode.Value)
            '    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSupplierName, "partyname", "partycode", "select partyname,partycode from partymast where active=1 order by partyname", True, ddlSupplierName.Value)

            'End If
            'If ddlSupplierCode.Value <> "[Select]" Then
            '    strSqlQry = "select othgrpmast.othgrpcode,othgrpmast.othgrpname  from  othgrpmast left outer join  partyothgrp on othgrpmast.othgrpcode=partyothgrp.othgrpcode  where active=1 " & _
            '    "and partyothgrp.partycode='" & ddlSupplierCode.Items(ddlSupplierCode.SelectedIndex).Text & "' order by othgrpcode"
            '    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlGroupCode, "othgrpcode", "othgrpname", strSqlQry, True, ddlGroupCode.Value)

            '    strSqlQry = "select othgrpmast.othgrpname,othgrpmast.othgrpcode  from  othgrpmast left outer join  partyothgrp on othgrpmast.othgrpcode=partyothgrp.othgrpcode  where active=1 " & _
            '    "and partyothgrp.partycode='" & ddlSupplierCode.Items(ddlSupplierCode.SelectedIndex).Text & "'  order by othgrpname"
            '    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlGroupName, "othgrpname", "othgrpcode", strSqlQry, True, ddlGroupName.Value)
        Else

            TxtSellCurr.Text = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select Distinct  currmast.currcode from  currmast inner join othsellmast on currmast.currcode=othsellmast.currcode   where currmast.active=1 and othsellmast.othsellcode='" & CType(ddlOtherSell.Items(ddlOtherSell.SelectedIndex).Text.Trim, String) & "'")
            TxtSellConvRate.Text = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select convrate from currrates where currcode=(select option_selected from reservation_parameters where param_id=457) and tocurr='" & TxtSellCurr.Text & "'")
        End If




    End Sub





    Protected Sub btnhelp_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=OthPriceList1','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub

  
    

End Class
