Imports System.Data
Imports System.Data.SqlClient
Imports System.Drawing.Color
Imports System.Collections.ArrayList
Imports System.Collections.Generic
Imports System.Linq
Imports System.Drawing
Imports System.IO
Imports Microsoft.Office.Interop
Imports System.Data.OleDb
Imports System.Diagnostics
Imports ADODB
Imports System.IO.File
Imports System.Text
Imports DocumentFormat.OpenXml
Imports DocumentFormat.OpenXml.Packaging
Imports DocumentFormat.OpenXml.Spreadsheet
Imports System

Partial Class ContractFinalRates
    Inherits System.Web.UI.Page
    Private Connection As SqlConnection
    Dim objUser As New clsUser
    Private conn1 As New ADODB.Connection
    Dim objutil As New clsUtils
    Private wbPart As WorkbookPart = Nothing
    Private document As SpreadsheetDocument = Nothing


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
    Shared agentexists As String = ""

#End Region




  
#Region "Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim RefCode As String
        Session("Calledfrom") = CType(Request.QueryString("Calledfrom"), String)

        If IsPostBack = False Then

            Dim CalledfromValue As String = ""

            Dim ConFinappid As String = ""
            Dim ConFinappname As String = ""
            Dim Count As Integer
            Dim lngCount As Int16
            Dim strTempUserFunctionalRight As String()
            Dim strRights As String
            Dim functionalrights As String = ""
            ConFinappid = 1
            ConFinappname = objUser.GetAppName(Session("dbconnectionName"), ConFinappid)

            If CType(Session("GlobalUserName"), String) = Nothing Or CType(Session("Userpwd"), String) = Nothing Then
                Response.Redirect("~/Login.aspx", False)
                Exit Sub
            Else

                If Session("Calledfrom") = "Contracts" Then
                    Me.SubMenuUserControl1.menuidval = objUser.GetMenuId(Session("dbconnectionName"), CType("PriceListModule\ContractsSearch.aspx?appid=1", String), CType(Request.QueryString("appid"), Integer))
                    CalledfromValue = Me.SubMenuUserControl1.menuidval
                    objUser.CheckUserSubRight(Session("dbconnectionName"), CType(Session("GlobalUserName"), String), CType(Session("Userpwd"), String), _
                                                       CType(ConFinappname, String), "ContractFinalRates.aspx", CType(CalledfromValue, String), btnAddNew, btnExportToExcel, _
                 btnrates, gv_SearchResult)


                ElseIf Session("Calledfrom") = "Offers" Then
                    Me.SubMenuUserControl1.menuidval = objUser.GetMenuId(Session("dbconnectionName"), CType("PriceListModule\OfferSearch.aspx?appid=1", String), CType(Request.QueryString("appid"), Integer))
                    CalledfromValue = Me.SubMenuUserControl1.menuidval
                    objUser.CheckUserSubRight(Session("dbconnectionName"), CType(Session("GlobalUserName"), String), CType(Session("Userpwd"), String), _
                                                                          CType(ConFinappname, String), "ContractFinalRates.aspx", CType(CalledfromValue, String), btnAddNew, btnExportToExcel, _
                    btnOfferRates, gv_SearchResult)
                End If

            End If
            Dim intGroupID As Integer = objUser.GetGroupId(Session("dbconnectionName"), CType(Session("GlobalUserName"), String), CType(Session("Userpwd"), String))
            Dim intMenuID As String = objUser.GetCotractofferMenuId(Session("dbconnectionName"), "ContractFinalRates.aspx", ConFinappid, CalledfromValue)

            functionalrights = objUser.GetUserFunctionalRight(Session("dbconnectionName"), intGroupID, ConFinappid, intMenuID)

            If functionalrights <> "" Then
                strTempUserFunctionalRight = functionalrights.Split(";")
                For lngCount = 0 To strTempUserFunctionalRight.Length - 1
                    strRights = strTempUserFunctionalRight.GetValue(lngCount)

                    If strRights = "06" Then
                        Count = 1
                    End If
                Next

                If CalledfromValue = 1030 Then
                    btnOfferRates.Visible = False
                    btnofferothermeal.Visible = False
                    If Count = 1 Then
                        btnrates.Visible = True
                        btnratesothermeal.Visible = True
                    Else
                        btnrates.Visible = False
                        btnratesothermeal.Visible = False
                    End If

                ElseIf CalledfromValue = 1200 Then
                    btnrates.Visible = False
                    btnratesothermeal.Visible = False
                    If Count = 1 Then
                        btnOfferRates.Visible = True
                        btnofferothermeal.Visible = True
                    Else
                        btnOfferRates.Visible = False
                        btnofferothermeal.Visible = False
                    End If
                End If
            Else
                btnrates.Visible = False
                btnratesothermeal.Visible = False

                btnOfferRates.Visible = False
                btnofferothermeal.Visible = False
            End If

            If Session("Calledfrom") = "Offers" Then
                Me.SubMenuUserControl1.appval = CType(Request.QueryString("appid"), String)
                Me.SubMenuUserControl1.menuidval = objUser.GetMenuId(Session("dbconnectionName"), CType("PriceListModule\OfferSearch.aspx?appid=1", String), CType(Request.QueryString("appid"), Integer))
                Me.SubMenuUserControl1.Calledfromval = CType(Request.QueryString("Calledfrom"), String)

                hdnpartycode.Value = CType(Session("Offerparty"), String)
                hdncontractid.Value = CType(Session("contractid"), String)
                If Not Session("OfferRefCode") Is Nothing Then
                    hdnpromotionid.Value = Session("OfferRefCode")
                End If

                Session("partycode") = hdnpartycode.Value
                Dim hotelname1 = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select partyname from partymast where partycode='" & hdnpartycode.Value & "'")
                ViewState("hotelname") = hotelname1
                lblHeading.Text = lblHeading.Text + " - " + hotelname1 + " - " + hdnpromotionid.Value


                AutoCompleteExtender1.ContextKey = hdnpromotionid.Value
                AutoCompleteExtender2.ContextKey = hdnpromotionid.Value


                agentexists = objutil.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select 't' from  view_offers_agents where  promotionid='" & hdnpromotionid.Value & "'")

            Else

                Me.SubMenuUserControl1.appval = CType(Request.QueryString("appid"), String)
                Me.SubMenuUserControl1.menuidval = objUser.GetMenuId(Session("dbconnectionName"), CType("PriceListModule\ContractsSearch.aspx?appid=1", String), CType(Request.QueryString("appid"), Integer))
                Me.SubMenuUserControl1.Calledfromval = CType(Request.QueryString("Calledfrom"), String)

                '  objUtils.Clear_All_contract_sessions()
                txtconnection.Value = Session("dbconnectionName")

                '  hdCurrentDate.Value = Now.ToString("dd/MM/yyyy")

                hdnpartycode.Value = CType(Session("Contractparty"), String)
                SubMenuUserControl1.partyval = hdnpartycode.Value

                SubMenuUserControl1.contractval = CType(Session("contractid"), String)
                hdncontractid.Value = CType(Session("contractid"), String)

                AutoCompleteExtender1.ContextKey = hdncontractid.Value
                AutoCompleteExtender2.ContextKey = hdncontractid.Value



                '  hdnpartycode.Value = CType(Request.QueryString("partycode"), String)
                Dim hotelname = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select partyname from partymast where partycode='" & hdnpartycode.Value & "'")
                ViewState("hotelname") = hotelname
                Session("partycode") = hdnpartycode.Value

                '  Session("contractid") = CType(Request.QueryString("contractid"), String)
                lblHeading.Text = lblHeading.Text + " - " + hotelname + " - " + hdncontractid.Value

                agentexists = objutil.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select 't' from  view_contractagents where  contractid='" & hdncontractid.Value & "'")

                ' FillGrid("constructionid", hdnpartycode.Value, "Desc")
                '   PanelMain.Visible = False


            End If
        Else
        End If


            ' btnchecking.Attributes.Add("onclick", "return CheckContract('" & hdncontractid.Value & "')")

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

    <System.Web.Script.Services.ScriptMethod()> _
    <System.Web.Services.WebMethod()> _
    Public Shared Function Getctrylist(ByVal prefixText As String, ByVal contextKey As String) As List(Of String)

        Dim strSqlQry As String = ""
        Dim myDS As New DataSet
        Dim ctryname As New List(Of String)
        Dim contno As String = ""
        Try



            If HttpContext.Current.Session("Calledfrom") = "Offers" Then
                contno = Convert.ToString(HttpContext.Current.Session("OfferRefCode").ToString())
                strSqlQry = "select c.ctryname, v.ctrycode from   view_offers_countries v,ctrymast c where v.ctrycode=c.ctrycode  and c.active=1  and v.promotionid='" & contextKey & "' and c.ctryname like  '" & Trim(prefixText) & "%' order by ctryname "
            Else
                contno = Convert.ToString(HttpContext.Current.Session("contractid").ToString())
                strSqlQry = "select c.ctryname, v.ctrycode from  view_contractcountry v,ctrymast c where v.ctrycode=c.ctrycode  and c.active=1  and v.contractid='" & contextKey & "' and c.ctryname like  '" & Trim(prefixText) & "%' order by ctryname "
            End If
            Dim SqlConn As New SqlConnection
            Dim myDataAdapter As New SqlDataAdapter

            SqlConn = clsDBConnect.dbConnectionnew("strDBConnection")
            'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)

            If myDS.Tables(0).Rows.Count > 0 Then
                For i As Integer = 0 To myDS.Tables(0).Rows.Count - 1

                    ctryname.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(myDS.Tables(0).Rows(i)("ctryname").ToString(), myDS.Tables(0).Rows(i)("ctrycode").ToString()))
                    'Hotelnames.Add(myDS.Tables(0).Rows(i)("partyname").ToString() & "<span style='display:none'>" & i & "</span>")
                Next

            End If

            Return ctryname
        Catch ex As Exception
            Return ctryname
        End Try

    End Function
    <System.Web.Script.Services.ScriptMethod()> _
   <System.Web.Services.WebMethod()> _
    Public Shared Function Getagentlist(ByVal prefixText As String, ByVal contextKey As String) As List(Of String)

        Dim strSqlQry As String = ""
        Dim myDS As New DataSet
        Dim agentname As New List(Of String)
        Dim contno As String = ""
        Dim constring As String = Convert.ToString(HttpContext.Current.Session("dbconnectionName").ToString())
        Try


            If HttpContext.Current.Session("Calledfrom") = "Offers" Then
                contno = Convert.ToString(HttpContext.Current.Session("OfferRefCode").ToString())

                If Not agentexists Is Nothing = True Then
                    strSqlQry = "select c.agentname, v.agentcode from  view_offers_agents v,agentmast c where v.agentcode=c.agentcode and v.promotionid='" & contextKey & "'  and c.active=1 and c.agentname like  '" & Trim(prefixText) & "%' order by agentname "
                Else
                    strSqlQry = "select c.agentname, c.agentcode from  agentmast c where c.active=1 and c.agentname like  '" & Trim(prefixText) & "%' order by agentname "
                End If



            Else
                contno = Convert.ToString(HttpContext.Current.Session("contractid").ToString())

                If Not agentexists Is Nothing = True Then
                    strSqlQry = "select c.agentname, v.agentcode from  view_contractagents v,agentmast c where v.agentcode=c.agentcode and v.contractid='" & contextKey & "'  and c.active=1 and c.agentname like  '" & Trim(prefixText) & "%' order by agentname "
                Else
                    strSqlQry = "select c.agentname, c.agentcode from  agentmast c where c.active=1 and c.agentname like  '" & Trim(prefixText) & "%' order by agentname "
                End If


            End If

            Dim SqlConn As New SqlConnection
            Dim myDataAdapter As New SqlDataAdapter

            SqlConn = clsDBConnect.dbConnectionnew("strDBConnection")
            'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)

            If myDS.Tables(0).Rows.Count > 0 Then
                For i As Integer = 0 To myDS.Tables(0).Rows.Count - 1

                    agentname.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(myDS.Tables(0).Rows(i)("agentname").ToString(), myDS.Tables(0).Rows(i)("agentcode").ToString()))

                Next

            End If

            Return agentname
        Catch ex As Exception
            Return agentname
        End Try

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
            Dim lbtext As Label = CType(row.FindControl("lblerrormsg"), Label)
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
            objUtils.WritErrorLog("ContractValidate.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub











    Protected Sub btnhelp_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        '   ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=SupMain','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub



#Region "Numberssrvctrl"
    Private Sub Numberssrvctrl(ByVal txtbox As WebControls.TextBox)
        txtbox.Attributes.Add("onkeypress", "return  checkNumber(event)")
    End Sub
#End Region







    Public Sub Numbers(ByVal txtbox As HtmlInputText)
        txtbox.Attributes.Add("onkeypress", "return checkNumber(event)")
        'txtbox.Attributes.Add("onkeypress", "return checkNumberDecimal(event,this)")
    End Sub

    Protected Sub gv_SearchResult_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gv_SearchResult.RowCommand
        Try
            Dim dsmissing As DataSet
            Dim dtmissing As DataTable
            Dim lblTranid As Label = Nothing
            Dim lbloptions As Label = Nothing
            Dim lblErrType As Label

            Dim dvmissingdates As DataView

            Session("tranid") = Nothing
            lblTranid = gv_SearchResult.Rows(CType(e.CommandArgument.ToString, String)).FindControl("lblTranid")
            lbloptions = gv_SearchResult.Rows(CType(e.CommandArgument.ToString, String)).FindControl("lbloptions")
            lblErrType = gv_SearchResult.Rows(CType(e.CommandArgument.ToString, String)).FindControl("lblErrType")

            If e.CommandName = "Details" Then
                gvErrorlist.Columns(1).Visible = True
                gvErrorlist.Columns(2).Visible = True
                gvErrorlist.Columns(3).Visible = True
                gvErrorlist.Columns(4).Visible = True
                gvErrorlist.Columns(3).HeaderText = "Country"

                If Not Session("Missingdates") Is Nothing And lblErrType.Text = 1 Then


                    dtmissing = Session("Missingdates")
                    dvmissingdates = dtmissing.DefaultView
                    dvmissingdates.RowFilter = ("tranid='" & lblTranid.Text & "'  and optionname='" & lbloptions.Text & "'")

                    gvErrorlist.DataSource = dvmissingdates
                    gvErrorlist.DataBind()



                    If Not dtmissing Is Nothing Then
                        If dtmissing.Rows.Count > 0 Then
                            gvErrorlist.Visible = True
                            gvErrorlist.Columns(1).Visible = True
                            gvErrorlist.Columns(2).Visible = True
                            gvErrorlist.Columns(3).Visible = False
                            gvErrorlist.Columns(4).Visible = False

                        End If
                    Else
                        gvErrorlist.Visible = False
                    End If


                End If
                If Not Session("MissingCountryagent") Is Nothing And lblErrType.Text = 2 Then

                    dtmissing = Session("MissingCountryagent")
                    dvmissingdates = dtmissing.DefaultView
                    dvmissingdates.RowFilter = ("tranid='" & lblTranid.Text & "' and errtype=" & lblErrType.Text & " and optionname='" & lbloptions.Text & "' ")

                    gvErrorlist.DataSource = dvmissingdates
                    gvErrorlist.DataBind()

                    If Not dtmissing Is Nothing Then
                        If dtmissing.Rows.Count > 0 Then
                            gvErrorlist.Visible = True
                            gvErrorlist.Columns(1).Visible = False
                            gvErrorlist.Columns(2).Visible = False
                            gvErrorlist.Columns(3).Visible = True
                            gvErrorlist.Columns(4).Visible = True

                        End If
                    Else
                        gvErrorlist.Visible = False
                    End If

                End If

                If Not Session("MissingCountryagent") Is Nothing And lblErrType.Text = 3 Then
                    gvErrorlist.Columns(3).HeaderText = "Room Type"


                    dtmissing = Session("MissingCountryagent")
                    dvmissingdates = dtmissing.DefaultView
                    dvmissingdates.RowFilter = ("tranid='" & lblTranid.Text & "' and errtype=" & lblErrType.Text & " and optionname='" & lbloptions.Text & "'")

                    gvErrorlist.DataSource = dvmissingdates
                    gvErrorlist.DataBind()

                    If Not dtmissing Is Nothing Then
                        If dtmissing.Rows.Count > 0 Then
                            gvErrorlist.Visible = True
                            gvErrorlist.Columns(1).Visible = False
                            gvErrorlist.Columns(2).Visible = False
                            gvErrorlist.Columns(3).Visible = True
                            gvErrorlist.Columns(4).Visible = False

                        End If
                    Else
                        gvErrorlist.Visible = False
                    End If

                End If





            End If
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "BindGv", "BindGrid();", True)



        Catch ex As Exception


            ' ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ContractValidate.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try

    End Sub


    Protected Sub gv_SearchResult_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gv_SearchResult.RowDataBound

        If e.Row.RowType = DataControlRowType.DataRow Then

            Dim lnkDetails As LinkButton = e.Row.FindControl("lnkDetails")
            Dim lblErrType As Label = e.Row.FindControl("lblErrType")

            If lblErrType.Text = "0" Then
                lnkDetails.Enabled = False
            End If


        End If

    End Sub








    Public Sub KillUnusedExcelProcess()
        Dim oXlProcess As Process() = Process.GetProcessesByName("Excel")
        For Each oXLP As Process In oXlProcess
            If Len(oXLP.MainWindowTitle) = 0 Then
                oXLP.Kill()
            End If
        Next
    End Sub
    Private Sub ExcelOpen(ByVal openpath As String)
        GC.Collect()
        GC.WaitForPendingFinalizers()
        Dim oExcel As Object = Nothing
        Try
            If Dir(Trim(openpath)) = "" Then
                MsgBox("No Such Record ", , "Excel Export")
                Exit Sub
            End If
            oExcel = CreateObject("Excel.Application")
            oExcel.Workbooks.Open(Trim(openpath))
            oExcel.Visible = True
            oExcel.UserControl = True
            If oExcel IsNot Nothing Then
                System.Runtime.InteropServices.Marshal.ReleaseComObject(oExcel)
                oExcel = Nothing
            End If
            GC.Collect()
            GC.WaitForPendingFinalizers()
        Catch excc As Exception
            If oExcel IsNot Nothing Then
                System.Runtime.InteropServices.Marshal.ReleaseComObject(oExcel)
                oExcel = Nothing
            End If
            GC.Collect()
            GC.WaitForPendingFinalizers()
        Finally
            If oExcel IsNot Nothing Then
                System.Runtime.InteropServices.Marshal.ReleaseComObject(oExcel)
                oExcel = Nothing
            End If
            GC.Collect()
            GC.WaitForPendingFinalizers()
        End Try

    End Sub
    Public Function RecordSetToDataTable(ByVal objRS As ADODB.Recordset) As DataTable
        '****************************************
        '*** Code from VisibleVisual.com ********
        '****************************************

        Dim objDA As New OleDbDataAdapter()
        Dim objDT As New DataTable()
        objDA.Fill(objDT, objRS)
        Return objDT
    End Function

    Public Function DataTableToRecordSet(ByVal objDT As DataTable) As ADODB.Recordset
        '****************************************
        '*** Code from VisibleVisual.com ********
        '****************************************
        Dim objDA As New OleDbDataAdapter()
        Dim objRS As New ADODB.Recordset()
        '   Dim objDT As New DataTable()
        objDA.Fill(objRS, objDT)
        Return objRS
    End Function


    Private Sub FillCountry()
        Dim myDS As New DataSet

        gv_SearchResult.Visible = True


        If gv_SearchResult.PageIndex < 0 Then
            gv_SearchResult.PageIndex = 0
        End If



        strSqlQry = "" ' "select  '' constructionid , '' fromdate , '' todate , '' reason ,'' Miscellaneous ,'' adddate, '' adduser, ''  moddate ,'' moduser"
        Try
            strSqlQry = "SELECT ctrycode 'Country Code',ctryname 'Country Name' from ctrymast"


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

    Public Function GetResultAsRecordSet(ByVal strSqlQry As String) As ADODB.Recordset
        Try
            If conn1.State = ConnectionState.Open Then
                Call closeconnection()
            End If

            Dim rsNew As New ADODB.Recordset
            Dim oledbcon1 As String = ConfigurationManager.ConnectionStrings("strADODBConnection").ConnectionString
            conn1.Open(oledbcon1)
            conn1.CursorLocation = CursorLocationEnum.adUseClient
            rsNew = conn1.Execute(strSqlQry)
            ' conn1.Close()
            Return rsNew
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ContractHotelConst.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            Return Nothing
        End Try
    End Function
    Public Sub closeconnection()
        If conn1.State = ConnectionState.Open Then conn1.Close()
    End Sub
    Public Function TranslateType(ByRef type As Type) As ADODB.DataTypeEnum
        Try


            Select Case type.UnderlyingSystemType.ToString
                Case "System.Boolean"
                    Return ADODB.DataTypeEnum.adBoolean
                Case "System.Byte"
                    Return ADODB.DataTypeEnum.adUnsignedTinyInt
                Case "System.Char"
                    Return ADODB.DataTypeEnum.adChar
                Case "System.DateTime"
                    Return ADODB.DataTypeEnum.adDate
                Case "System.Decimal"
                    Return ADODB.DataTypeEnum.adCurrency

                Case "System.Double"


                    Return ADODB.DataTypeEnum.adDouble
                Case "System.Int16"
                    Return ADODB.DataTypeEnum.adSmallInt
                Case "System.Int32"
                    Return ADODB.DataTypeEnum.adInteger
                Case "System.Int64"
                    Return ADODB.DataTypeEnum.adBigInt
                Case "System.SByte"
                    Return ADODB.DataTypeEnum.adTinyInt
                Case "System.Single"
                    Return ADODB.DataTypeEnum.adSingle
                Case ("System.UInt16")
                    Return ADODB.DataTypeEnum.adUnsignedSmallInt
                Case "System.UInt32"
                    Return ADODB.DataTypeEnum.adUnsignedInt
                Case "System.UInt64"
                    Return ADODB.DataTypeEnum.adUnsignedBigInt
                Case "System.String"
                    '  Case default
                    Return ADODB.DataTypeEnum.adVarWChar
            End Select
        Catch ex As Exception

        End Try
    End Function
    Public Function convertToADODB(ByRef table As DataTable) As ADODB.Recordset


        Dim result As New ADODB.Recordset
        result.CursorLocation = CursorLocationEnum.adUseClient
        Dim resultFields As ADODB.Fields = result.Fields
        Dim col As DataColumn
        For Each col In table.Columns
            resultFields.Append(col.ColumnName, TranslateType(col.DataType),
                    col.MaxLength, col.AllowDBNull = ADODB.FieldAttributeEnum.adFldIsNullable)
        Next
        'result.Open(System.Reflection.Missing.Value, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockOptimistic, 0)
        result.Open(System.Reflection.Missing.Value, System.Reflection.Missing.Value, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockOptimistic, 0)

        For Each row As DataRow In table.Rows
            result.AddNew(System.Reflection.Missing.Value, System.Reflection.Missing.Value)

            For i As Integer = 0 To table.Columns.Count - 1
                resultFields(i).Value = IIf(IsDBNull(row(i)) = True, "", row(i))
            Next
        Next
        Return (result)
    End Function

    Private Function ValidateSave() As Boolean
        If txtctrycode.Text = "" And txtagentcode.Text = "" Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Select Any Country or Agent.');", True)
            txtctryname.Focus()
            ValidateSave = False
            Exit Function
        End If



        ValidateSave = True
    End Function


    Protected Sub btnOfferRates_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOfferRates.Click
        Try

            If ValidateSave() = False Then
                Exit Sub
            End If

            Dim FolderPath As String = "..\ExcelTemplates\"
            Dim FileName As String = "OfferMarkupRates.xlsx"
            Dim FilePath As String = Server.MapPath(FolderPath + FileName)

            FolderPath = "~\ExcelTemp\"
            Dim FileNameNew As String = "OfferMarkupRates_" & Now.Year & Now.Month & Now.Day & Now.Hour & Now.Minute & Now.Second & Now.Millisecond & ".xlsx"
            Dim outputPath As String = Server.MapPath(FolderPath + FileNameNew)

            File.Copy(FilePath, outputPath, True)
            'document = SpreadsheetDocument.Open(outputPath, True)
            'wbPart = document.WorkbookPart

            Dim wsName As String = "Markup Rates"

            Dim dt As New DataTable
            Dim dt1 As New DataTable
            Dim ds As New DataSet

            strSqlQry = "Exec sp_finalcalculated_rates_offers_withmarkup_daterange " & "'" & hdnpromotionid.Value & "','" & CType(txtctrycode.Text, String) & "','" & CType(txtagentcode.Text, String) & "'"

            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection

            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.SelectCommand.CommandTimeout = 0
            myDataAdapter.Fill(dt)
            SqlConn.Close()


            dt.TableName = "Markup Rates"
            ds.Tables.Add(dt)

            CreateExcelDocument(ds, outputPath)


            GC.Collect()
            GC.WaitForPendingFinalizers()



            Dim strpop1 As String
            strpop1 = "window.open('DownloadFiles.aspx?filename=" & FileNameNew & " &FileLoc=ExcelTemp');"
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop1, True)


            'Dim FolderPath As String = "..\ExcelTemplates\"
            'Dim FileName As String = "OfferMarkupRates.xlsx"
            'Dim FilePath As String = Server.MapPath(FolderPath + FileName)


            'FolderPath = "~\ExcelTemp\"
            'Dim FileNameNew As String = "OfferMarkupRates_" & Now.Year & Now.Month & Now.Day & Now.Hour & Now.Minute & Now.Second & Now.Millisecond & ".xlsx"
            'Dim outputPath As String = Server.MapPath(FolderPath + FileNameNew)

            'File.Copy(FilePath, outputPath, True)
            'document = SpreadsheetDocument.Open(outputPath, True)
            'wbPart = document.WorkbookPart

            'Dim wsName As String = "Markup Rates"


            'Dim iLine2 As Integer = 11
            'Dim ei2 As Integer
            'Dim ei3 As Integer
            'Dim ei4 As Integer

            'UpdateValue(wsName, "B4", ViewState("hotelname"), 2, True, False)
            'UpdateValue(wsName, "B5", hdnpromotionid.Value, 2, True, False)
            'UpdateValue(wsName, "B6", txtctryname.Text, 2, True, False)
            'UpdateValue(wsName, "B7", txtagentname.Text, 2, True, False)


            'Dim dt As New DataTable
            ''strSqlQry = "Exec sp_finalcalculated_rates_offers_withmarkup " & "'" & hdnpromotionid.Value & "','" & CType(txtctrycode.Text, String) & "','" & CType(txtagentcode.Text, String) & "'"

            'strSqlQry = "Exec sp_finalcalculated_rates_offers_withmarkup_daterange " & "'" & hdnpromotionid.Value & "','" & CType(txtctrycode.Text, String) & "','" & CType(txtagentcode.Text, String) & "'"

            'SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection

            'myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            'myDataAdapter.SelectCommand.CommandTimeout = 0
            'myDataAdapter.Fill(dt)
            'SqlConn.Close()

            'UpdateTableValue(wsName, dt, 0, iLine2, 2, True)


            'wsName = "Markup Rates-Other Meal Plan"

            'UpdateValue(wsName, "B4", ViewState("hotelname"), 2, True, False)
            'UpdateValue(wsName, "B5", hdnpromotionid.Value, 3, True, False)
            'UpdateValue(wsName, "B6", txtctryname.Text, 2, True, False)
            'UpdateValue(wsName, "B7", txtagentname.Text, 2, True, False)




            'Dim dt1 As New DataTable
            ''strSqlQry = "Exec sp_finalcalculated_rates_offers_withmarkup_othmeal " & "'" & hdnpromotionid.Value & "','" & CType(txtctrycode.Text, String) & "','" & CType(txtagentcode.Text, String) & "'"
            'strSqlQry = "Exec sp_finalcalculated_rates_offers_withmarkup_othmeal_daterange  " & "'" & hdnpromotionid.Value & "','" & CType(txtctrycode.Text, String) & "','" & CType(txtagentcode.Text, String) & "'"



            'SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection

            'myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            'myDataAdapter.SelectCommand.CommandTimeout = 0
            'myDataAdapter.Fill(dt1)


            'UpdateTableValue(wsName, dt1, 0, iLine2, 2, True)


            'document.Close()

            'Dim contno As String()
            'contno = hdnpromotionid.Value.Split("/")

            'GC.Collect()
            'GC.WaitForPendingFinalizers()



            ''DownloadFiles.aspx

            'Dim strpop As String
            'strpop = "window.open('DownloadFiles.aspx?filename=" & FileNameNew & " &FileLoc=ExcelTemp');"
            'ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)




        Catch ex As Exception
            If mySqlConn.State = ConnectionState.Open Then
                sqlTrans.Rollback()
            End If
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            'objUtils.WritErrorLog("ContractFinalRates.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))

        End Try
    End Sub

    Public Shared Function CreateExcelDocument(ByVal ds As DataSet, ByVal excelFilename As String) As Boolean
        Try
            Using document As SpreadsheetDocument = SpreadsheetDocument.Create(excelFilename, SpreadsheetDocumentType.Workbook)

                Dim workbook As WorkbookPart = document.AddWorkbookPart

                '                document.AddWorkbookPart()
                document.WorkbookPart.Workbook = New DocumentFormat.OpenXml.Spreadsheet.Workbook()

                '  My thanks to James Miera for the following line of code (which prevents crashes in Excel 2010)
                document.WorkbookPart.Workbook.Append(New BookViews(New WorkbookView()))

                '  If we don't add a "WorkbookStylesPart", OLEDB will refuse to connect to this .xlsx file !
                Dim workbookStylesPart As WorkbookStylesPart = document.WorkbookPart.AddNewPart(Of WorkbookStylesPart)("rIdStyles")

                Dim stylesheet As New Stylesheet
                workbookStylesPart.Stylesheet = stylesheet
                workbookStylesPart.Stylesheet.Save()

                '                Dim sp As WorkbookStylesPart = document.WorkbookPart.AddNewPart(Of WorkbookStylesPart)()

                CreateParts(ds, document)

            End Using
            Return True

        Catch ex As Exception
            Return False
        End Try

    End Function
    Private Shared Sub CreateParts(ByVal ds As DataSet, ByVal spreadsheet As SpreadsheetDocument)

        '  Loop through each of the DataTables in our DataSet, and create a new Excel Worksheet for each.
        Dim worksheetNumber As UInt64 = 1
        For Each dt As DataTable In ds.Tables
            '  For each worksheet you want to create
            Dim workSheetID As String = "rId" + worksheetNumber.ToString()
            Dim worksheetName As String = dt.TableName

            Dim newWorksheetPart As WorksheetPart = spreadsheet.WorkbookPart.AddNewPart(Of WorksheetPart)()
            newWorksheetPart.Worksheet = New DocumentFormat.OpenXml.Spreadsheet.Worksheet()

            '  If you want to define the Column Widths, you need to do this *before* appending the SheetData
            '  http://social.msdn.microsoft.com/Forums/en-US/oxmlsdk/thread/1d93eca8-2949-4d12-8dd9-15cc24128b10/
            '
            '  If you want to calculate the column width, it's not easy.  Have a read of this article:
            '  http://polymathprogrammer.com/2010/01/11/custom-column-widths-in-excel-open-xml/
            '

            Dim columnWidthSize As Int32 = 20     ' Replace the following line with your desired Column Width for column # col
            Dim columns As New Columns

            For colInx As Integer = 0 To dt.Columns.Count
                Dim column As Column = CustomColumnWidth(colInx, columnWidthSize)
                columns.Append(column)
            Next
            newWorksheetPart.Worksheet.Append(columns)

            ' create sheet data
            newWorksheetPart.Worksheet.AppendChild(New DocumentFormat.OpenXml.Spreadsheet.SheetData())

            ' save worksheet
            WriteDataTableToExcelWorksheet(dt, newWorksheetPart)
            newWorksheetPart.Worksheet.Save()

            ' create the worksheet to workbook relation
            If (worksheetNumber = 1) Then
                spreadsheet.WorkbookPart.Workbook.AppendChild(New DocumentFormat.OpenXml.Spreadsheet.Sheets())
            End If

            Dim sheet As DocumentFormat.OpenXml.Spreadsheet.Sheet = New DocumentFormat.OpenXml.Spreadsheet.Sheet
            sheet.Id = spreadsheet.WorkbookPart.GetIdOfPart(newWorksheetPart)
            sheet.SheetId = worksheetNumber
            sheet.Name = dt.TableName
            '            Sheets.Append(sheet)

            spreadsheet.WorkbookPart.Workbook.GetFirstChild(Of DocumentFormat.OpenXml.Spreadsheet.Sheets).Append(sheet)
            ' AppendChild(new DocumentFormat.OpenXml.Spreadsheet.Sheet()
        Next
    End Sub
    Private Shared Sub WriteDataTableToExcelWorksheet(ByVal dt As DataTable, ByVal worksheetPart As WorksheetPart)

        Dim worksheet As Worksheet = worksheetPart.Worksheet
        Dim sheetData As SheetData = worksheet.GetFirstChild(Of SheetData)()

        Dim cellValue As String = ""

        '  Create a Header Row in our Excel file, containing one header for each Column of data in our DataTable.
        '
        '  We'll also create an array, showing which type each column of data is (Text or Numeric), so when we come to write the actual
        '  cells of data, we'll know if to write Text values or Numeric cell values.
        Dim numberOfColumns As Integer = dt.Columns.Count
        Dim IsNumericColumn(numberOfColumns) As Boolean

        Dim excelColumnNames([numberOfColumns]) As String

        For n As Integer = 0 To numberOfColumns
            excelColumnNames(numberOfColumns) = GetExcelColumnName(n)
        Next n

        '
        '  Create the Header row in our Excel Worksheet
        '
        Dim rowIndex As UInt32 = 1

        Dim headerRow As Row = New Row
        headerRow.RowIndex = rowIndex            ' add a row at the top of spreadsheet
        sheetData.Append(headerRow)

        For colInx As Integer = 0 To numberOfColumns - 1
            Dim col As DataColumn = dt.Columns(colInx)
            AppendTextCell(excelColumnNames(colInx) + "1", col.ColumnName, headerRow)
            IsNumericColumn(colInx) = (col.DataType.FullName = "System.Decimal") Or (col.DataType.FullName = "System.Int32")
        Next

        '
        '  Now, step through each row of data in our DataTable...
        '
        Dim cellNumericValue As Double = 0

        For Each dr As DataRow In dt.Rows
            ' ...create a new row, and append a set of this row's data to it.
            rowIndex = rowIndex + 1
            Dim newExcelRow As New Row
            newExcelRow.RowIndex = rowIndex         '  add a row at the top of spreadsheet
            sheetData.Append(newExcelRow)

            For colInx As Integer = 0 To numberOfColumns - 1
                cellValue = dr.ItemArray(colInx).ToString()

                ' Create cell with data
                If (IsNumericColumn(colInx)) Then
                    '  For numeric cells, make sure our input data IS a number, then write it out to the Excel file.
                    '  If this numeric value is NULL, then don't write anything to the Excel file.
                    cellNumericValue = 0
                    If (Double.TryParse(cellValue, cellNumericValue)) Then
                        cellValue = cellNumericValue.ToString()
                        AppendNumericCell(excelColumnNames(colInx) + rowIndex.ToString(), cellValue, newExcelRow)
                    End If
                Else
                    '  For text cells, just write the input data straight out to the Excel file.
                    AppendTextCell(excelColumnNames(colInx) + rowIndex.ToString(), cellValue, newExcelRow)
                End If
            Next

        Next

    End Sub
    Private Sub callexcel(ByVal ds As DataSet, ByVal excelFilename As String, ByVal spreadsheet As SpreadsheetDocument, ByVal workbook1 As WorkbookPart)

        Using workbook = SpreadsheetDocument.Open(excelFilename, DocumentFormat.OpenXml.SpreadsheetDocumentType.Workbook)
            Dim workbookPart = workbook1 ' workbook.AddWorkbookPart()
            workbook.WorkbookPart.Workbook = New DocumentFormat.OpenXml.Spreadsheet.Workbook()
            workbook.WorkbookPart.Workbook.Sheets = New DocumentFormat.OpenXml.Spreadsheet.Sheets()

            Dim sheetId As UInteger = 10



            For Each table As DataTable In ds.Tables
                Dim sheetPart = workbook.WorkbookPart.AddNewPart(Of WorksheetPart)()
                Dim sheetData = New DocumentFormat.OpenXml.Spreadsheet.SheetData()
                sheetPart.Worksheet = New DocumentFormat.OpenXml.Spreadsheet.Worksheet(sheetData)


                Dim ws As Worksheet = DirectCast(workbook1.GetPartById(sheetId), WorksheetPart).Worksheet




                Dim sheets As DocumentFormat.OpenXml.Spreadsheet.Sheets = workbook.WorkbookPart.Workbook.GetFirstChild(Of DocumentFormat.OpenXml.Spreadsheet.Sheets)()
                Dim relationshipId As String = workbook.WorkbookPart.GetIdOfPart(ws.WorksheetPart)

                If sheets.Elements(Of DocumentFormat.OpenXml.Spreadsheet.Sheet)().Count() > 0 Then
                    sheetId = sheets.Elements(Of DocumentFormat.OpenXml.Spreadsheet.Sheet)().[Select](Function(s) s.SheetId.Value).Max() + 1
                End If

                Dim sheet1 As New DocumentFormat.OpenXml.Spreadsheet.Sheet()


                sheets.Append(sheet1)

                Dim headerRow As New DocumentFormat.OpenXml.Spreadsheet.Row()

                Dim columns As List(Of [String]) = New List(Of String)()
                For Each column As DataColumn In table.Columns
                    columns.Add(column.ColumnName)

                    Dim cell As New DocumentFormat.OpenXml.Spreadsheet.Cell()
                    cell.DataType = DocumentFormat.OpenXml.Spreadsheet.CellValues.[String]
                    cell.CellValue = New DocumentFormat.OpenXml.Spreadsheet.CellValue(column.ColumnName)
                    headerRow.AppendChild(cell)
                Next

                ws.AppendChild(headerRow)

                For Each dsrow As DataRow In table.Rows
                    Dim newRow As New DocumentFormat.OpenXml.Spreadsheet.Row()
                    For Each col As [String] In columns
                        Dim cell As New DocumentFormat.OpenXml.Spreadsheet.Cell()
                        cell.DataType = DocumentFormat.OpenXml.Spreadsheet.CellValues.[String]
                        cell.CellValue = New DocumentFormat.OpenXml.Spreadsheet.CellValue(dsrow(col).ToString())
                        '
                        newRow.AppendChild(cell)
                    Next

                    ws.AppendChild(newRow)
                Next
            Next
        End Using
    End Sub
    Public Shared Function ListToDataTable(Of T)(ByVal list As List(Of T)) As DataTable

        Dim dt As New DataTable
        Dim row As DataRow
        For Each info As System.Reflection.PropertyInfo In list.GetType().GetProperties()
            dt.Columns.Add(New DataColumn(info.Name, GetNullableType(info.PropertyType)))
        Next

        For Each tValue As T In list

            row = dt.NewRow()
            For Each info As System.Reflection.PropertyInfo In list.GetType().GetProperties()

                If Not IsNullableType(info.PropertyType) Then
                    row(info.Name) = info.GetValue(tValue, Nothing)
                Else
                    row(info.Name) = info.GetValue(tValue, Nothing)
                End If
            Next
            dt.Rows.Add(row)
        Next
        Return dt
    End Function
    Public Shared Function GetNullableType(ByVal t As Type) As Type

        Dim returnType As Type = t

        If (t.IsGenericType Or t.GetGenericTypeDefinition() Is GetType(Nullable(Of ))) Then
            returnType = Nullable.GetUnderlyingType(t)
        End If

        Return returnType

    End Function
    Public Shared Function IsNullableType(ByVal type As Type) As Boolean

        Return (type Is GetType(String) Or
                type.IsArray Or
                (type.IsGenericType And type.GetGenericTypeDefinition() Is GetType(Nullable(Of ))))
    End Function
    Public Shared Function GetExcelColumnName(ByVal columnIndex As Integer) As String
        If (columnIndex < 26) Then
            Return Chr(Asc("A") + columnIndex)
        End If

        Dim firstChar As Char,
            secondChar As Char

        firstChar = Chr(Asc("A") + (columnIndex \ 26) - 1)
        secondChar = Chr(Asc("A") + (columnIndex Mod 26))

        Return firstChar + secondChar
    End Function
    Public Shared Sub AppendTextCell(ByVal cellReference As String, ByVal cellStringValue As String, ByVal excelRow As Row)
        '/  Add a new Excel Cell to our Row 
        Dim cell As New Cell
        cell.CellReference = cellReference
        cell.DataType = CellValues.String

        Dim cellValue As New CellValue
        cellValue.Text = cellStringValue

        cell.Append(cellValue)

        excelRow.Append(cell)
    End Sub
    Public Shared Sub AppendNumericCell(ByVal cellReference As String, ByVal cellStringValue As String, ByVal excelRow As Row)
        '/  Add a new Excel Cell to our Row 
        Dim cell As New Cell
        cell.CellReference = cellReference
        cell.DataType = CellValues.Number

        Dim cellValue As New CellValue
        cellValue.Text = cellStringValue

        cell.Append(cellValue)

        excelRow.Append(cell)
    End Sub
    Private Shared Function CustomColumnWidth(ByVal columnIndex As Integer, ByVal columnWidth As Double) As Column
        ' This creates a Column variable for a zero-based column-index (eg 0 = Excel Column A), with a particular column width.
        Dim column As New Column
        column.Min = columnIndex + 1
        column.Max = columnIndex + 1
        column.Width = columnWidth
        column.CustomWidth = True
        Return column
    End Function

    Protected Sub btnrates_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnrates.Click
        Try

            If ValidateSave() = False Then
                Exit Sub
            End If

            Dim FolderPath As String = "..\ExcelTemplates\"
            Dim FileName As String = "ContractMarkuprates.xlsx"
            Dim FilePath As String = Server.MapPath(FolderPath + FileName)

            FolderPath = "~\ExcelTemp\"
            Dim FileNameNew As String = "ContractMarkuprates_" & Now.Year & Now.Month & Now.Day & Now.Hour & Now.Minute & Now.Second & Now.Millisecond & ".xlsx"
            Dim outputPath As String = Server.MapPath(FolderPath + FileNameNew)

            File.Copy(FilePath, outputPath, True)
            'document = SpreadsheetDocument.Open(outputPath, True)
            'wbPart = document.WorkbookPart

            Dim wsName As String = "Markup Rates"

            Dim dt As New DataTable
            Dim dt1 As New DataTable
            Dim ds As New DataSet

            strSqlQry = "Exec sp_finalcalculated_rates_withmarkup_dateranges " & "'" & hdncontractid.Value & "','" & CType(txtctrycode.Text, String) & "','" & CType(txtagentcode.Text, String) & "'"

            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection

            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.SelectCommand.CommandTimeout = 0
            myDataAdapter.Fill(dt)
            SqlConn.Close()


            dt.TableName = "Markup Rates"
            ds.Tables.Add(dt)

            CreateExcelDocument(ds, outputPath)


            GC.Collect()
            GC.WaitForPendingFinalizers()



            Dim strpop1 As String
            strpop1 = "window.open('DownloadFiles.aspx?filename=" & FileNameNew & " &FileLoc=ExcelTemp');"
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop1, True)


        Catch ex As Exception
            If mySqlConn.State = ConnectionState.Open Then
                sqlTrans.Rollback()
            End If
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            'objUtils.WritErrorLog("ContractFinalRates.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))

        End Try

        'Dim FolderPath As String = "..\ExcelTemplates\"
        'Dim FileName As String = "ContractMarkuprates.xlsx"
        'Dim FilePath As String = Server.MapPath(FolderPath + FileName)


        'FolderPath = "~\ExcelTemp\"
        'Dim FileNameNew As String = "ContractMarkuprates_" & Now.Year & Now.Month & Now.Day & Now.Hour & Now.Minute & Now.Second & Now.Millisecond & ".xlsx"
        'Dim outputPath As String = Server.MapPath(FolderPath + FileNameNew)

        'File.Copy(FilePath, outputPath, True)
        'document = SpreadsheetDocument.Open(outputPath, True)
        'wbPart = document.WorkbookPart

        'Dim wsName As String = "Markup Rates"


        'Dim iLine2 As Integer = 11
        'Dim ei2 As Integer
        'Dim ei3 As Integer
        'Dim ei4 As Integer

        'UpdateValue(wsName, "B4", ViewState("hotelname"), 2, True, False)
        'UpdateValue(wsName, "B5", hdncontractid.Value, 2, True, False)
        'UpdateValue(wsName, "B6", txtctryname.Text, 2, True, False)
        'UpdateValue(wsName, "B7", txtagentname.Text, 2, True, False)


        'Dim dt As New DataTable
        ''strSqlQry = "Exec sp_finalcalculated_rates_withmarkup " & "'" & hdncontractid.Value & "','" & CType(txtctrycode.Text, String) & "','" & CType(txtagentcode.Text, String) & "'"
        'strSqlQry = "Exec sp_finalcalculated_rates_withmarkup_dateranges " & "'" & hdncontractid.Value & "','" & CType(txtctrycode.Text, String) & "','" & CType(txtagentcode.Text, String) & "'"

        'SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection

        'myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
        'myDataAdapter.SelectCommand.CommandTimeout = 0
        'myDataAdapter.Fill(dt)

        'UpdateTableValue(wsName, dt, 0, iLine2, 2, True)




        'iLine2 = 11

        'wsName = "Markup Rates-Other Meal Plan"

        'UpdateValue(wsName, "B4", ViewState("hotelname"), 2, True, False)
        'UpdateValue(wsName, "B5", hdncontractid.Value, 3, True, False)
        'UpdateValue(wsName, "B6", txtctryname.Text, 2, True, False)
        'UpdateValue(wsName, "B7", txtagentname.Text, 2, True, False)




        'Dim dt1 As New DataTable
        ''strSqlQry = "Exec sp_finalcalculated_rates_withmarkup_othmeal " & "'" & hdncontractid.Value & "','" & CType(txtctrycode.Text, String) & "','" & CType(txtagentcode.Text, String) & "'"

        'strSqlQry = "Exec sp_finalcalculated_rates_withmarkup_othmeal_dateranges " & "'" & hdncontractid.Value & "','" & CType(txtctrycode.Text, String) & "','" & CType(txtagentcode.Text, String) & "'"

        'SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection

        'myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
        'myDataAdapter.SelectCommand.CommandTimeout = 0
        'myDataAdapter.Fill(dt1)


        'UpdateTableValue(wsName, dt1, 0, iLine2, 2, True)

        ''Dim rsrates1 As New ADODB.Recordset
        ''rsrates1 = objutil.convertToADODB(dt1)
        ''ei3 = rsrates1.RecordCount
        ''xlSheet.Range("A" & iLine2.ToString).CopyFromRecordset(rsrates1)



        ''strSqlQry = "Exec sp_finalcalculated_rates_withmarkup " & "'" & hdncontractid.Value & "','" & CType(txtctrycode.Text, String) & "','" & CType(txtagentcode.Text, String) & "'"
        ''Dim rse2 As New ADODB.Recordset
        ''rse2 = GetResultAsRecordSet(strSqlQry)
        ''ei2 = rse2.RecordCount
        ''xlSheet.Range("A" & iLine2.ToString).CopyFromRecordset(rse2)

        'document.Close()

        'Dim contno As String()
        'contno = hdncontractid.Value.Split("/")

        'GC.Collect()
        'GC.WaitForPendingFinalizers()



        ''DownloadFiles.aspx

        'Dim strpop As String
        'strpop = "window.open('DownloadFiles.aspx?filename=" & FileNameNew & " &FileLoc=ExcelTemp');"
        'ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)




    End Sub
    ' Given a Worksheet and an address (like "AZ254"), either return a cell reference, or 
    ' create the cell reference and return it.
    Private Function InsertCellInWorksheet(ByVal ws As Worksheet, ByVal addressName As String) As Cell
        Dim sheetData As SheetData = ws.GetFirstChild(Of SheetData)()
        Dim cell As Cell = Nothing

        Dim rowNumber As UInt32 = GetRowIndex(addressName)
        Dim row As Row = GetRow(sheetData, rowNumber)

        ' If the cell you need already exists, return it.
        ' If there is not a cell with the specified column name, insert one.  
        Dim refCell As Cell = row.Elements(Of Cell)().Where(Function(c) c.CellReference.Value = addressName).FirstOrDefault()
        If refCell IsNot Nothing Then
            cell = refCell
        Else
            cell = CreateCell(row, addressName)
        End If
        Return cell
    End Function

    Private Function CreateCell(ByVal row As Row, ByVal address As [String]) As Cell
        Dim cellResult As Cell
        Dim refCell As Cell = Nothing

        ' Cells must be in sequential order according to CellReference. Determine where to insert the new cell.
        For Each cell As Cell In row.Elements(Of Cell)()
            'If String.Compare(cell.CellReference.Value, address, True) > 0 Then
            If GetColumnIndex(cell.CellReference.Value) > GetColumnIndex(address) Then
                refCell = cell
                Exit For
            End If
        Next

        cellResult = New Cell()
        cellResult.CellReference = address

        row.InsertBefore(cellResult, refCell)
        Return cellResult
    End Function
    Private Shared Function GetColumnIndex(ByVal cellRef As String) As System.Nullable(Of Integer)
        If String.IsNullOrEmpty(cellRef) Then
            Return Nothing
        End If

        cellRef = cellRef.ToUpper()

        Dim columnIndex As Integer = -1
        Dim mulitplier As Integer = 1

        For Each c As Char In cellRef.ToCharArray().Reverse()
            If Char.IsLetter(c) Then
                columnIndex += mulitplier * (Asc(c) - 64)
                mulitplier = mulitplier * 26
            End If
        Next

        Return columnIndex
    End Function
    Private Function GetRow(ByVal wsData As SheetData, ByVal rowIndex As UInt32) As Row
        Dim row = wsData.Elements(Of Row)().Where(Function(r) r.RowIndex.Value = rowIndex).FirstOrDefault()
        If row Is Nothing Then
            row = New Row()
            row.RowIndex = rowIndex
            wsData.Append(row)
        End If
        Return row
    End Function

    Private Function GetRowIndex(ByVal address As String) As UInt32
        Dim rowPart As String
        Dim l As UInt32
        Dim result As UInt32 = 0

        For i As Integer = 0 To address.Length - 1
            If UInt32.TryParse(address.Substring(i, 1), l) Then
                rowPart = address.Substring(i, address.Length - i)
                If UInt32.TryParse(rowPart, l) Then
                    result = l
                    Exit For
                End If
            End If
        Next
        Return result
    End Function
    Public Function UpdateValue(ByVal sheetName As String, ByVal addressName As String, ByVal value As String, ByVal styleIndex As Integer, ByVal isString As Boolean, Optional ByVal isMerge As Boolean = False, Optional ByVal toaddressname As String = "", Optional ByVal wrapcell As Boolean = False) As Boolean
        ' Assume failure.
        Dim updated As Boolean = False

        Dim sheet As Sheet = wbPart.Workbook.Descendants(Of Sheet)().Where(Function(s) s.Name = sheetName).FirstOrDefault()

        If sheet IsNot Nothing Then
            Dim ws As Worksheet = DirectCast(wbPart.GetPartById(sheet.Id), WorksheetPart).Worksheet
            Dim cell As Cell = InsertCellInWorksheet(ws, addressName)

            If isString Then
                ' Either retrieve the index of an existing string,
                ' or insert the string into the shared string table
                ' and get the index of the new item.
                Dim stringIndex As Integer = InsertSharedStringItem(wbPart, value)

                cell.CellValue = New CellValue(stringIndex.ToString())
                cell.DataType = New EnumValue(Of CellValues)(CellValues.SharedString)
            Else
                cell.CellValue = New CellValue(value)
                cell.DataType = New EnumValue(Of CellValues)(CellValues.Number)
            End If


            If styleIndex > 0 Then
                cell.StyleIndex = styleIndex

            End If

            If isMerge Then
                Dim MergeCells As New MergeCells()
                If (ws.Elements(Of MergeCells)().Count() > 0) Then
                    MergeCells = ws.Elements(Of MergeCells).First()
                Else
                    MergeCells = New MergeCells()
                    ' Insert a MergeCells object into the specified position.
                    If (ws.Elements(Of CustomSheetView)().Count() > 0) Then
                        ws.InsertAfter(MergeCells, ws.Elements(Of CustomSheetView)().First())
                    ElseIf (ws.Elements(Of DataConsolidate)().Count() > 0) Then
                        ws.InsertAfter(MergeCells, ws.Elements(Of DataConsolidate)().First())
                    ElseIf (ws.Elements(Of SortState)().Count() > 0) Then
                        ws.InsertAfter(MergeCells, ws.Elements(Of SortState)().First())
                    ElseIf (ws.Elements(Of AutoFilter)().Count() > 0) Then
                        ws.InsertAfter(MergeCells, ws.Elements(Of AutoFilter)().First())
                    ElseIf (ws.Elements(Of Scenarios)().Count() > 0) Then
                        ws.InsertAfter(MergeCells, ws.Elements(Of Scenarios)().First())
                    ElseIf (ws.Elements(Of ProtectedRanges)().Count() > 0) Then
                        ws.InsertAfter(MergeCells, ws.Elements(Of ProtectedRanges)().First())
                    ElseIf (ws.Elements(Of SheetProtection)().Count() > 0) Then
                        ws.InsertAfter(MergeCells, ws.Elements(Of SheetProtection)().First())
                    ElseIf (ws.Elements(Of SheetCalculationProperties)().Count() > 0) Then
                        ws.InsertAfter(MergeCells, ws.Elements(Of SheetCalculationProperties)().First())
                    Else
                        ws.InsertAfter(MergeCells, ws.Elements(Of SheetData)().First())
                    End If
                End If
                Dim mergeCell As MergeCell = New MergeCell()

                'append a MergeCell to the mergeCells for each set of merged cells
                mergeCell.Reference = New StringValue(addressName + ":" + toaddressname)
                MergeCells.Append(mergeCell)
            End If

            If wrapcell Then
                cell.StyleIndex = InsertCellFormat(wbPart, GenerateCellFormat())


            End If

            ' Save the worksheet.
            ws.Save()
            updated = True
        End If

        Return updated
    End Function
    Public Function UpdateTableValue(ByVal sheetName As String, ByVal rs As DataTable, ByVal colnumber As Integer, ByVal rownumber As Integer, ByVal styleIndex As Integer, ByVal isString As Boolean) As Boolean
        ' Assume failure.
        Dim updated As Boolean = False

        Dim sheet As Sheet = wbPart.Workbook.Descendants(Of Sheet)().Where(Function(s) s.Name = sheetName).FirstOrDefault()

        If sheet IsNot Nothing Then
            Dim ws As Worksheet = DirectCast(wbPart.GetPartById(sheet.Id), WorksheetPart).Worksheet


            Dim numrows As Integer
            numrows = rs.Rows.Count
            Dim iLineNo As Integer = rownumber
            Dim addressName As String = ""
            Dim Value As String = ""

            If rs.Rows.Count > 0 Then


                For i As Integer = 0 To rs.Rows.Count - 1

                    For k As Integer = 0 To rs.Columns.Count - 1
                        If 65 + colnumber + k > 90 And 65 + colnumber + k <= 116 Then
                            addressName = "A" + Trim(Chr(65 + colnumber + k - 26)) + Trim(Str(iLineNo))
                        ElseIf 65 + colnumber + k > 116 Then
                            addressName = "B" + Trim(Chr(65 + colnumber + k - 52)) + Trim(Str(iLineNo))
                        Else
                            addressName = Trim(Chr(65 + colnumber + k)) + Trim(Str(iLineNo))
                        End If

                        Value = rs.Rows(i).Item(k).ToString()
                        Dim cell As Cell
                        cell = InsertCellInWorksheet(ws, addressName)

                        If isString Then
                            ' Either retrieve the index of an existing string,
                            ' or insert the string into the shared string table
                            ' and get the index of the new item.
                            Dim stringIndex As Integer = InsertSharedStringItem(wbPart, Value)

                            cell.CellValue = New CellValue(stringIndex.ToString())
                            cell.DataType = New EnumValue(Of CellValues)(CellValues.SharedString)
                        Else
                            cell.CellValue = New CellValue(Value)
                            cell.DataType = New EnumValue(Of CellValues)(CellValues.Number)
                        End If


                        If styleIndex > 0 Then
                            cell.StyleIndex = styleIndex
                        End If

                    Next
                    iLineNo = iLineNo + 1

                Next
            End If



            ' Save the worksheet.
            ws.Save()
            updated = True
        End If

        Return updated
    End Function
    ' Given the main workbook part, and a text value, insert the text into the shared
    ' string table. Create the table if necessary. If the value already exists, return
    ' its index. If it doesn't exist, insert it and return its new index.
    Private Function InsertSharedStringItem(ByVal wbPart As WorkbookPart, ByVal value As String) As Integer
        Dim index As Integer = 0
        Dim found As Boolean = False
        Dim stringTablePart = wbPart.GetPartsOfType(Of SharedStringTablePart)().FirstOrDefault()

        ' If the shared string table is missing, something's wrong.
        ' Just return the index that you found in the cell.
        ' Otherwise, look up the correct text in the table.
        If stringTablePart Is Nothing Then
            ' Create it.
            stringTablePart = wbPart.AddNewPart(Of SharedStringTablePart)()
        End If

        Dim stringTable = stringTablePart.SharedStringTable
        If stringTable Is Nothing Then
            stringTable = New SharedStringTable()
        End If

        ' Iterate through all the items in the SharedStringTable. If the text already exists, return its index.
        For Each item As SharedStringItem In stringTable.Elements(Of SharedStringItem)()
            If item.InnerText = value Then
                found = True
                Exit For
            End If
            index += 1
        Next

        If Not found Then
            stringTable.AppendChild(New SharedStringItem(New Text(value)))
            stringTable.Save()
        End If

        Return index
    End Function

    ' Used to force a recalc of cells containing formulas. The
    ' CellValue has a cached value of the evaluated formula. This
    ' will prevent Excel from recalculating the cell even if 
    ' calculation is set to automatic.
    Private Function RemoveCellValue(ByVal sheetName As String, ByVal addressName As String) As Boolean
        Dim returnValue As Boolean = False

        Dim sheet As Sheet = wbPart.Workbook.Descendants(Of Sheet)().Where(Function(s) s.Name = sheetName).FirstOrDefault()
        If sheet IsNot Nothing Then
            Dim ws As Worksheet = DirectCast(wbPart.GetPartById(sheet.Id), WorksheetPart).Worksheet
            Dim cell As Cell = InsertCellInWorksheet(ws, addressName)

            ' If there is a cell value, remove it to force a recalc
            ' on this cell.
            If cell.CellValue IsNot Nothing Then
                cell.CellValue.Remove()
            End If

            ' Save the worksheet.
            ws.Save()
            returnValue = True
        End If

        Return returnValue
    End Function
    Public Function GenerateCellFormats() As CellFormats
        Dim cellFormats1 As New CellFormats() With { _
            .Count = CType(2, UInt32Value) _
           }
        Dim cellFormat1 As New CellFormat() With { _
            .NumberFormatId = CType(0, UInt32Value), _
            .FontId = CType(0, UInt32Value), _
            .FillId = CType(0, UInt32Value), _
            .BorderId = CType(0, UInt32Value), _
            .FormatId = CType(0, UInt32Value), _
            .ApplyAlignment = True _
           }
        Dim alignment1 As New Alignment() With { _
            .WrapText = True _
           }

        cellFormat1.Append(alignment1)
        cellFormats1.Append(cellFormat1)

        Return cellFormats1
    End Function

    Public Function InsertCellFormat(ByVal workbookpart As WorkbookPart, ByVal cellformat As CellFormat) As UInteger
        Dim cellFormats As CellFormats = workbookpart.WorkbookStylesPart.Stylesheet.Elements(Of CellFormats)().First()
        cellFormats.Append(cellformat)
        Dim indexcount As UInteger
        indexcount = cellFormats.Count
        If indexcount > 0 Then indexcount = indexcount - 1
        Return indexcount
    End Function


    Private Function GenerateCellFormat() As CellFormat
        Dim cellFormat1 As New CellFormat() With { _
            .NumberFormatId = CType(0, UInt32Value), _
            .FontId = CType(0, UInt32Value), _
            .FillId = CType(0, UInt32Value), _
            .BorderId = CType(0, UInt32Value), _
            .FormatId = CType(0, UInt32Value), _
            .ApplyAlignment = True _
           }
        Dim alignment1 As New Alignment() With { _
            .WrapText = True _
           }

        cellFormat1.Append(alignment1)
        Return cellFormat1
    End Function

    Protected Sub btnofferothermeal_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnofferothermeal.Click
        Try

            If ValidateSave() = False Then
                Exit Sub
            End If

            Dim FolderPath As String = "..\ExcelTemplates\"
            Dim FileName As String = "OfferMarkupRates.xlsx"
            Dim FilePath As String = Server.MapPath(FolderPath + FileName)

            FolderPath = "~\ExcelTemp\"
            Dim FileNameNew As String = "OfferMarkupRates_" & Now.Year & Now.Month & Now.Day & Now.Hour & Now.Minute & Now.Second & Now.Millisecond & ".xlsx"
            Dim outputPath As String = Server.MapPath(FolderPath + FileNameNew)

            File.Copy(FilePath, outputPath, True)
            'document = SpreadsheetDocument.Open(outputPath, True)
            'wbPart = document.WorkbookPart

            Dim wsName As String = "Markup Rates"

            Dim dt As New DataTable
            Dim dt1 As New DataTable
            Dim ds As New DataSet

            strSqlQry = "Exec sp_finalcalculated_rates_offers_withmarkup_othmeal_daterange  " & "'" & hdnpromotionid.Value & "','" & CType(txtctrycode.Text, String) & "','" & CType(txtagentcode.Text, String) & "'"

            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection

            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.SelectCommand.CommandTimeout = 0
            myDataAdapter.Fill(dt)
            SqlConn.Close()


            dt.TableName = "OtherMealPlan Markup Rates"
            ds.Tables.Add(dt)

            CreateExcelDocument(ds, outputPath)


            GC.Collect()
            GC.WaitForPendingFinalizers()



            Dim strpop1 As String
            strpop1 = "window.open('DownloadFiles.aspx?filename=" & FileNameNew & " &FileLoc=ExcelTemp');"
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop1, True)


           
         
        Catch ex As Exception
            If mySqlConn.State = ConnectionState.Open Then
                sqlTrans.Rollback()
            End If
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            'objUtils.WritErrorLog("ContractFinalRates.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))

        End Try
    End Sub

    Protected Sub btnratesothermeal_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnratesothermeal.Click
        Try

            If ValidateSave() = False Then
                Exit Sub
            End If

            Dim FolderPath As String = "..\ExcelTemplates\"
            Dim FileName As String = "ContractMarkuprates.xlsx"
            Dim FilePath As String = Server.MapPath(FolderPath + FileName)

            FolderPath = "~\ExcelTemp\"
            Dim FileNameNew As String = "ContractMarkuprates_" & Now.Year & Now.Month & Now.Day & Now.Hour & Now.Minute & Now.Second & Now.Millisecond & ".xlsx"
            Dim outputPath As String = Server.MapPath(FolderPath + FileNameNew)

            File.Copy(FilePath, outputPath, True)
            'document = SpreadsheetDocument.Open(outputPath, True)
            'wbPart = document.WorkbookPart

            Dim wsName As String = "Markup Rates"

            Dim dt As New DataTable
            Dim dt1 As New DataTable
            Dim ds As New DataSet

            strSqlQry = "Exec sp_finalcalculated_rates_withmarkup_othmeal_dateranges " & "'" & hdncontractid.Value & "','" & CType(txtctrycode.Text, String) & "','" & CType(txtagentcode.Text, String) & "'"

            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection

            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.SelectCommand.CommandTimeout = 0
            myDataAdapter.Fill(dt)
            SqlConn.Close()


            dt.TableName = "OtherMealPlan Markup Rates"
            ds.Tables.Add(dt)

            CreateExcelDocument(ds, outputPath)


            GC.Collect()
            GC.WaitForPendingFinalizers()



            Dim strpop1 As String
            strpop1 = "window.open('DownloadFiles.aspx?filename=" & FileNameNew & " &FileLoc=ExcelTemp');"
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop1, True)




        Catch ex As Exception
            If mySqlConn.State = ConnectionState.Open Then
                sqlTrans.Rollback()
            End If
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            'objUtils.WritErrorLog("ContractFinalRates.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))

        End Try
    End Sub
End Class
