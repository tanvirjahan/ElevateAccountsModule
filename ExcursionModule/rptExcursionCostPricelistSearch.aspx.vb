Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.CrystalReports
Imports CrystalDecisions.Shared
Imports CrystalDecisions.Web
Imports CrystalDecisions.ReportSource

#Region "Namespace"
Imports System.Data
Imports System.Data.SqlClient
#End Region

Partial Class NewclientsSearch
    Inherits System.Web.UI.Page
    Dim objectcl As New clsDateTime
    Dim rep As New ReportDocument
    Dim rptcompanyname As String
    Dim rptreportname As String
    Dim objutils As New clsUtils
    Dim servicetype As String
    Dim default_country As String
    Dim strqry As String
    Dim seasonfrom As String
    Dim seasonto As String
    Dim objUser As New clsUser

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

        If Page.IsPostBack = False Then
            Try

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
                If CType(Session("GlobalUserName"), String) = Nothing Or CType(Session("Userpwd"), String) = Nothing Then
                    Response.Redirect("~/Login.aspx", False)
                    Exit Sub
                Else
                    objUser.CheckUserRight(Session("dbconnectionName"), CType(Session("GlobalUserName"), String), CType(Session("Userpwd"), String), _
                                                       CType(strappname, String), "ExcursionModule\rptExcursionCostPricelistSearch.aspx?appid=" + strappid, btnadd, Button1, BtnPrint, gv_SearchResult)
                End If
                txtconnection.Value = Session("dbconnectionName")

                If txtFromDate.Text = "" Then
                    txtFromDate.Text = objectcl.GetSystemDateOnly(Session("dbconnectionName"))
                End If


                If txtToDate.Text = "" Then
                    txtToDate.Text = DateAdd(DateInterval.Month, 1, objectcl.GetSystemDateOnly(Session("dbconnectionName")))
                End If



                objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlpartycode, "partycode", "partyname", "select partycode,partyname from partymast where active=1 and partymast.sptypecode <>(select option_selected from reservation_parameters where param_id='458') order by partycode", True)
                objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlpartyname, "partyname", "partycode", "select partycode,partyname from partymast where active=1 and partymast.sptypecode <>(select option_selected from reservation_parameters where param_id='458') order by partycode", True)

                objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlgroupcode, "othgrpcode", "othgrpname", "select othgrpcode,othgrpname from othgrpmast where active=1 and othmaingrpcode in (select option_selected from reservation_parameters where param_id in('1104','1105'))  order by othgrpcode", True)
                objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlgroupname, "othgrpname", "othgrpcode", "select othgrpcode,othgrpname from othgrpmast where active=1 and othmaingrpcode in (select option_selected from reservation_parameters where param_id in('1104','1105')) order by othgrpname", True)

                objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlsubgroup, "othtypcode", "othtypname", "select othtypcode,othtypname from vw_excplist where active=1  order by othtypcode", True)
                objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlsubgroupname, "othtypname", "othtypcode", "select othtypcode,othtypname from vw_excplist where active=1  order by othtypname", True)

                
                default_country = objutils.GetDBFieldFromStringnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", CType("459", String))
                ddlgroupname.Value = CType(default_country, String)
                ddlgroupcode.Value = ddlgroupname.Items(ddlgroupname.SelectedIndex).Text

                ''ddlsectorcode.Style.Add("display", "none")
                ''ddlsectorname.Style.Add("display", "none")

                ''lblsectorcode.Style.Add("display", "none")
                ''lblsectorname.Style.Add("display", "none")


                If Request.QueryString("catcode") <> "" Then
                    ddlpartyname.Value = Request.QueryString("catcode")
                    ddlpartycode.Value = ddlpartyname.Items(ddlpartyname.SelectedIndex).Text
                End If
               
                If Request.QueryString("ctrycode") <> "" Then
                    ''ddlairportname.Value = Request.QueryString("ctrycode")
                    ''ddlairportcode.Value = ddlairportname.Items(ddlairportname.SelectedIndex).Text
                End If
                
                If Request.QueryString("fromdate") <> "" Then
                    txtFromDate.Text = Format("U", CType(Request.QueryString("fromdate"), Date))
                End If
                If Request.QueryString("todate") <> "" Then
                    txtToDate.Text = Format("U", CType(Request.QueryString("todate"), Date))
                End If
                

                txtFromDate.Attributes.Add("onchange", "javascript:ChangeDate();")
            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objutils.WritErrorLog("rptSupplierpoliciesSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))

            End Try









        End If

        Dim typ As Type
        typ = GetType(DropDownList)

        If (Page.ClientScript.IsClientScriptIncludeRegistered(typ, "TADDScript.js")) = False Then
            Page.ClientScript.RegisterClientScriptInclude(typ, "TADDScript.js", "TADDScript.js")


            '' ddlsellcode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
            ''ddlsellname.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")

            ''ddlairportcode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
            ''ddlairportname.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")

          

        End If

        ClientScript.GetPostBackEventReference(Me, String.Empty)
        If (Me.IsPostBack And Me.Request("__EVENTTARGET") = "RepNewClientWindowPostBack") Then
            BtnPrint_Click(sender, e)
        End If


    End Sub

    '#Region "Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        If Page.IsPostBack = False Then

            'objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlairportcode, "ctrycode", "ctryname", "select ctrycode,ctryname from ctrymast where active=1  order by ctrycode", True, ddlairportcode.Value)
            ' objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlairportname, "ctryname", "ctrycode", "select ctryname,ctrycode from ctrymast where active=1  order by ctryname", True, ddlairportname.Value)
          '

            'FillGrid()

      





        End If



    End Sub

    Protected Sub BtnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnClear.Click
        ddlpartycode.Value = "[Select]"
        ddlpartyname.Value = "[Select]"
        ddlgroupcode.Value = "[Select]"
        ddlgroupname.Value = "[Select]"
        ddlsubgroup.Value = "[Select]"
        ddlsubgroupname.Value = "[Select]"
        txtFromDate.Text = objectcl.GetSystemDateOnly(Session("dbconnectionName"))
        txtFromDate.Text = DateAdd(DateInterval.Month, 1, objectcl.GetSystemDateOnly(Session("dbconnectionName")))

    End Sub

#Region " Validate Page"
    Public Function ValidatePage() As Boolean
        'Dim frmdate, todate As Date
        Dim objDateTime As New clsDateTime
        Try

            If txtFromDate.Text = "" Then

                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('From date field can not be blank.');", True)

                SetFocus(txtFromDate)
                ValidatePage = False
                Exit Function
            End If
            If txtToDate.Text = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('As on  date field can not be blank.');", True)

                SetFocus(txtToDate)
                ValidatePage = False
                Exit Function
            End If

            

            If CType(objectcl.ConvertDateromTextBoxToDatabase(txtfromdate.Text), Date) > CType(objectcl.ConvertDateromTextBoxToDatabase(txttodate.Text), Date) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('To date should not less than from date.');", True)
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + txttodate.ClientID + "');", True)
                'SetFocus(txtToDate)
                ValidatePage = False
                Exit Function
            End If


            'If ddlgroupcode.Value = "[Select]" And ddlmarket.Value = "[Select]" Then

            '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('AirportCode Should be Selected.');", True)
            '    ValidatePage = False
            '    Exit Function

            'End If


            If ddlpartycode.Value = "[Select]" Then

                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Party Should be Selected.');", True)
                ValidatePage = False
                Exit Function

            End If

            

            'If ddlgroupcode.Value = "[Select]" Then

            '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Group Should be Selected.');", True)
            '    ValidatePage = False
            '    Exit Function

            'End If


            Dim p As Integer

        

            ValidatePage = True
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        End Try
    End Function
#End Region

    Protected Sub BtnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            If ValidatePage() = True Then
                'Session.Add("Pageame", "Newclient")
                'Session.Add("BackPageName", "NewclientSearch.aspx")
                'Session("ColReportParams") = Nothing

                Dim strReportTitle As String = ""
                Dim param1 As String = ""
                Dim param2 As String = ""
                Dim param3 As String = ""
                Dim param4 As String = ""
                Dim param5 As String = ""
                Dim param6 As String = ""

                Dim strfromdate As String = ""
                Dim strtodate As String = ""


                Dim strsellcode As String = ""
                Dim strairportcode As String = ""

                Dim strrepfilter As String = ""
                Dim strreportoption As String = ""
                Dim strsectorcode As String = ""
                Dim strgroupcode As String = ""
                Dim strsubcode As String = ""
                Dim strarr(6) As String
                Dim k As Integer
                Dim P As Integer
                strsellcode = IIf(UCase(ddlpartycode.Items(ddlpartycode.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlpartycode.Items(ddlpartycode.SelectedIndex).Text, "")
                strgroupcode = IIf(UCase(ddlgroupcode.Items(ddlgroupcode.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlgroupcode.Items(ddlgroupcode.SelectedIndex).Text, "")
                strfromdate = Mid(Format(CType(txtfromdate.Text, Date), "u"), 1, 10)
                strtodate = Mid(Format(CType(txttodate.Text, Date), "u"), 1, 10)
                strsubcode = IIf(UCase(ddlsubgroup.Items(ddlsubgroup.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlsubgroup.Items(ddlsubgroup.SelectedIndex).Text, "")

                strreportfilter()
                strReportTitle = " Excursion Cost Pricelist"
               
                Dim strpop As String = ""

                '    strpop = "window.open('rptexcursionCostPricelistReport.aspx?Pageame=TranferPricelist&BackPageName=rptTransferPricelistReport.aspx&sellcode=" & strsellcode & "&airportcode=" & strsectorcode _
                '& "&fromdate=" & strfromdate & "&todate=" & strtodate _
                '& "&mysellcode=" & strsellcode & "&groupcode=" & strgroupcode _
                '& "&subcode=" & strsubcode _
                '& "&repfilter=" & strreportfilter() & "&reportoption=" & strreportoption & "&reporttitle=" & strReportTitle & "','RepNewClient','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes' );"

                strpop = "window.open('rptexcursionCostPricelistReport.aspx?Pageame=TranferPricelist&BackPageName=rptTransferPricelistReport.aspx&sellcode=" & strsellcode & "&airportcode=" & strsectorcode _
           & "&fromdate=" & strfromdate & "&todate=" & strtodate _
           & "&mysellcode=" & strsellcode & "&groupcode=" & strgroupcode _
           & "&subcode=" & strsubcode _
           & "&repfilter=" & strreportfilter() & "&reportoption=" & strreportoption & "&reporttitle=" & strReportTitle & "','RepNewClient');"



                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objutils.WritErrorLog("NewclientsSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))

        End Try
    End Sub
    Private Function strreportfilter()
        Dim strnew As String
        Try
            strnew = " From : " & txtfromdate.Text & " To :" & txttodate.Text
            If ddlgroupcode.Items(ddlgroupcode.SelectedIndex).Text <> "[Select]" Then
                strnew = strnew + " Group Code :" & ddlgroupcode.Items(ddlgroupcode.SelectedIndex).Text
            End If

            If ddlsubgroup.Items(ddlsubgroup.SelectedIndex).Text <> "[Select]" Then
                strnew = strnew + " Sub Group Code :" & ddlsubgroup.Items(ddlsubgroup.SelectedIndex).Text
            End If

            If ddlpartycode.Items(ddlpartycode.SelectedIndex).Text <> "[Select]" Then
                strnew = strnew + " Party Code :" & ddlpartycode.Items(ddlpartycode.SelectedIndex).Text
            End If

            strreportfilter = strnew
        Catch ex As Exception

        End Try
    End Function
#Region "Private Sub FillGrid(ByVal strorderby As String, Optional ByVal strsortorder As String = 'ASC')"
    Private Sub FillGrid()
        Dim myDS As New DataSet
        Dim MyCommand As SqlCommand
        Dim SqlConn1 As SqlConnection
        Dim myDataAdapter As SqlDataAdapter
        Dim ObjDate As New clsDateTime
        Dim strSqlQry As String
      

        Try


            strSqlQry = "select othcatcode,othcatname from othcatmast where othgrpcode='TRFS' "


            SqlConn1 = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                            'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn1)
            myDataAdapter.Fill(myDS)


           


        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objutils.WritErrorLog("NewclientsSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            'clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
            'clsDBConnect.dbConnectionClose(SqlConn)                          'Close connection
        End Try

    End Sub
#End Region

    Protected Sub btndisplay_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btndisplay.Click

    End Sub


    



#Region "Public Sub SortGridColoumn_click()"
    Public Sub SortGridColoumn_click()
        Dim DataTable As DataTable
        Dim myDS As New DataSet


        ''myDS = gv_SearchResult.DataSource
        DataTable = myDS.Tables(0)
        'If IsDBNull(DataTable) = False Then
        '    Dim dataView As DataView = DataTable.DefaultView
        '    Session.Add("strsortdirection", objutils.SwapSortDirection(Session("strsortdirection")))
        '    dataView.Sort = Session("strsortexpression") & " " & objutils.ConvertSortDirectionToSql(Session("strsortdirection"))
        '    gv_SearchResult.DataSource = dataView
        '    gv_SearchResult.DataBind()
        'End If
    End Sub
#End Region

    Protected Sub btnhelp_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=NewclientsSearch','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub

   
End Class
