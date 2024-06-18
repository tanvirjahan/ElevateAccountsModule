'------------================--------------=======================------------------================
'   Module Name    :    ContractsTrack.aspx
'   Developer Name :    Abin Paul
'   Date           :    27 Aug 2016
'------------================--------------=======================------------------================


#Region "Namespace"
Imports System.Data
Imports System.Data.SqlClient
Imports System.Collections.Generic
Imports System.Web.Services
Imports System.IO
Imports AjaxControlToolkit
Imports System.Globalization

#End Region



#Region "Enum GridCol"
Enum GridCol
    CategoryCodeTCol = 0
    FromCurrency = 1
    ToCurrency = 2
    Conversion = 3
    DateCreated = 4
    UserCreated = 5
    DateModified = 6
    UserModified = 7

End Enum
#End Region

Partial Class ContractsTrack
    Inherits System.Web.UI.Page

#Region "Global Declarations"
    Dim objUtils As New clsUtils
    Dim objUser As New clsUser
    Dim strSqlQry As String
    Dim strWhereCond As String
    Dim sqlTrans As SqlTransaction
    Dim SqlConn As New SqlConnection
    Dim myCommand As SqlCommand
    Dim myDataAdapter As SqlDataAdapter
    Dim mySqlReader As SqlDataReader
    Dim GvRow As GridViewRow
    Dim txtconvert As HtmlInputText
    Dim iFlag As Integer = 0
    Private PageSize As Integer = 8
#End Region

    Protected Sub Page_Init(sender As Object, e As System.EventArgs) Handles Me.Init
       
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack = False Then
            Try
                'SetFocus("txtconvert")
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

                End If
                FillMailBox(1)
                FillCurrentDayEmailCount()
                FillTracking()
                FillTrackingActioned()

                FillMailBoxIgnore(1)



                hdTrackpopupStatus.Value = "N"

                '' Create a Dynamic datatable ---- Start
                Dim dtDynamic = New DataTable()
                Dim dcCode = New DataColumn("Code", GetType(String))
                Dim dcCountry = New DataColumn("Value", GetType(String))
                dtDynamic.Columns.Add(dcCode)
                dtDynamic.Columns.Add(dcCountry)
                Session("sDtDynamic") = dtDynamic
                '--------end
                '' Create a Dynamic datatable ---- Start
                Dim dtHotelDetails = New DataTable()
                Dim dcGroupDetailsType = New DataColumn("Type", GetType(String))
                Dim dcGroupDetailsCode = New DataColumn("Code", GetType(String))
                Dim dcGroupDetailsValue = New DataColumn("Value", GetType(String))
                Dim dcTrackingStatus = New DataColumn("TrackingStatus", GetType(String))
                dtHotelDetails.Columns.Add(dcGroupDetailsType)
                dtHotelDetails.Columns.Add(dcGroupDetailsCode)
                dtHotelDetails.Columns.Add(dcGroupDetailsValue)
                dtHotelDetails.Columns.Add(dcTrackingStatus)
                Session("sDtHotelDetails") = dtHotelDetails
                '--------end

                '' Create a Dynamic datatable ---- Start
                Dim dtDynamicInbox = New DataTable()
                Dim dcCodeInbox = New DataColumn("Code", GetType(String))
                Dim dcValueInbox = New DataColumn("Value", GetType(String))
                dtDynamicInbox.Columns.Add(dcCodeInbox)
                dtDynamicInbox.Columns.Add(dcValueInbox)
                Session("sDtDynamicInbox") = dtDynamicInbox
                '--------end
                '' Create a Dynamic datatable ---- Start
                Dim dtHotelDetailsInbox = New DataTable()
                Dim dcGroupDetailsTypeInbox = New DataColumn("Type", GetType(String))
                Dim dcGroupDetailsCodeInbox = New DataColumn("Code", GetType(String))
                Dim dcGroupDetailsValueInbox = New DataColumn("Value", GetType(String))
                dtHotelDetailsInbox.Columns.Add(dcGroupDetailsTypeInbox)
                dtHotelDetailsInbox.Columns.Add(dcGroupDetailsCodeInbox)
                dtHotelDetailsInbox.Columns.Add(dcGroupDetailsValueInbox)
                Session("sDtHotelDetailsInbox") = dtHotelDetailsInbox
                '--------end
                '' Create a Dynamic datatable ---- Start
                Dim dtDynamicPopup = New DataTable()
                Dim dcCodePopup = New DataColumn("Code", GetType(String))
                Dim dcValuePopup = New DataColumn("Value", GetType(String))
                dtDynamicPopup.Columns.Add(dcCodePopup)
                dtDynamicPopup.Columns.Add(dcValuePopup)
                Session("sDtDynamicPopup") = dtDynamicPopup
                '--------end
                '' Create a Dynamic datatable ---- Start
                Dim dtHotelDetailsPopup = New DataTable()
                Dim dcGroupDetailsTypePopup = New DataColumn("Type", GetType(String))
                Dim dcGroupDetailsCodePopup = New DataColumn("Code", GetType(String))
                Dim dcGroupDetailsValuePopup = New DataColumn("Value", GetType(String))
                dtHotelDetailsPopup.Columns.Add(dcGroupDetailsTypePopup)
                dtHotelDetailsPopup.Columns.Add(dcGroupDetailsCodePopup)
                dtHotelDetailsPopup.Columns.Add(dcGroupDetailsValuePopup)
                Session("sDtHotelDetailsPopup") = dtHotelDetailsPopup
                '--------end

                '' Create a Dynamic datatable ---- Start
                Dim dtDynamicTrackView = New DataTable()
                Dim dcCodeTrackview = New DataColumn("Code", GetType(String))
                Dim dcValueTrackview = New DataColumn("Value", GetType(String))
                dtDynamicTrackView.Columns.Add(dcCodeTrackview)
                dtDynamicTrackView.Columns.Add(dcValueTrackview)
                Session("sDtDynamicTrackview") = dtDynamicTrackView
                '--------end
                '' Create a Dynamic datatable ---- Start
                Dim dtHotelDetailsTrackview = New DataTable()
                Dim dcGroupDetailsTypeTrackview = New DataColumn("Type", GetType(String))
                Dim dcGroupDetailsCodeTrackview = New DataColumn("Code", GetType(String))
                Dim dcGroupDetailsValueTrackview = New DataColumn("Value", GetType(String))
                dtHotelDetailsTrackview.Columns.Add(dcGroupDetailsTypeTrackview)
                dtHotelDetailsTrackview.Columns.Add(dcGroupDetailsCodeTrackview)
                dtHotelDetailsTrackview.Columns.Add(dcGroupDetailsValueTrackview)
                Session("sDtHotelDetailsTrackview") = dtHotelDetailsTrackview
                '--------end

                '' Create a Dynamic datatable ---- Start
                Dim dtDynamicInboxIgnore = New DataTable()
                Dim dcCodeInboxIgnore = New DataColumn("Code", GetType(String))
                Dim dcValueInboxIgnore = New DataColumn("Value", GetType(String))
                dtDynamicInboxIgnore.Columns.Add(dcCodeInboxIgnore)
                dtDynamicInboxIgnore.Columns.Add(dcValueInboxIgnore)
                Session("sDtDynamicInboxIgnore") = dtDynamicInboxIgnore
                '--------end
                '' Create a Dynamic datatable ---- Start
                Dim dtHotelDetailsInboxIgnore = New DataTable()
                Dim dcGroupDetailsTypeInboxIgnore = New DataColumn("Type", GetType(String))
                Dim dcGroupDetailsCodeInboxIgnore = New DataColumn("Code", GetType(String))
                Dim dcGroupDetailsValueInboxIgnore = New DataColumn("Value", GetType(String))
                dtHotelDetailsInboxIgnore.Columns.Add(dcGroupDetailsTypeInboxIgnore)
                dtHotelDetailsInboxIgnore.Columns.Add(dcGroupDetailsCodeInboxIgnore)
                dtHotelDetailsInboxIgnore.Columns.Add(dcGroupDetailsValueInboxIgnore)
                Session("sDtHotelDetailsInboxIgnore") = dtHotelDetailsInboxIgnore
                '--------end


            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objUtils.WritErrorLog("ContactsTrack.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            End Try

        End If
        If hdTrackpopupStatus Is Nothing Then
            meContractTracking.Hide()
        Else
            If hdTrackpopupStatus.Value = "Y" Then
                meContractTracking.Show()
            Else
                meContractTracking.Hide()
            End If
        End If
        If (Me.IsPostBack And Me.Request("__EVENTTARGET") = "ContractTrackWindowPostBack") Then

            '  lblIdPopup.Text = String.Format("{0:000000}", Convert.ToInt32(lblId.Text))
            Dim strScript As String = "javascript: visualsearchbox();"
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", strScript, True)
            hdPopupStatus.Value = "Y"
            dlList.DataBind()
            FillGridNew()
            mp1.Show()
        End If
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
                    Case 13
                        Me.MasterPageFile = "~/VisaMaster.master"      'Added by Archana on 05/06/2015 for VisaModule
                    Case Else
                        Me.MasterPageFile = "~/SubPageMaster.master"
                End Select
            Else
                Me.MasterPageFile = "~/SubPageMaster.master"
            End If
        End If

    End Sub

#Region "Private Sub FillGrid(ByVal strorderby As String, Optional ByVal strsortorder As String = 'ASC')"
    Private Sub FillMailBox(pageIndex As Integer)
        Dim myDS As New DataSet

        Try

            Dim constring As String = ConfigurationManager.ConnectionStrings("strDBConnection").ConnectionString
            Using con As New SqlConnection(constring)
                Using cmd As New SqlCommand("GetEmailsPageWise", con)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.Parameters.AddWithValue("@PageIndex", pageIndex)
                    cmd.Parameters.AddWithValue("@PageSize", PageSize)
                    cmd.Parameters.Add("@RecordCount", SqlDbType.Int, 4)
                    cmd.Parameters("@RecordCount").Direction = ParameterDirection.Output
                    con.Open()
                    Dim idr As IDataReader = cmd.ExecuteReader()
                    dlMailInbox.DataSource = idr
                    dlMailInbox.DataBind()
                    idr.Close()
                    con.Close()
                    Dim recordCount As Integer = Convert.ToInt32(cmd.Parameters("@RecordCount").Value)
                    Me.PopulatePager(recordCount, pageIndex)
                End Using
            End Using



        Catch ex As Exception
            objUtils.WritErrorLog("ContractTrack.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try

    End Sub
#End Region

#Region "HTMLNumbers"
    Public Sub HTMLNumbers(ByVal txtbox As HtmlInputText)
        Try
            txtbox.Attributes.Add("onkeypress", "return checkNumber(event)")
        Catch ex As Exception

        End Try
    End Sub
#End Region

    ''' <summary>
    ''' PopulatePager
    ''' </summary>
    ''' <param name="recordCount"></param>
    ''' <param name="currentPage"></param>
    ''' <remarks></remarks>
    Private Sub PopulatePager(recordCount As Integer, currentPage As Integer)
        Dim pages As New List(Of ListItem)()
        Dim startIndex As Integer, endIndex As Integer
        Dim pagerSpan As Integer = 3

        'Calculate the Start and End Index of pages to be displayed.
        Dim dblPageCount As Double = CDbl(CDec(recordCount) / Convert.ToDecimal(PageSize))
        Dim pageCount As Integer = CInt(Math.Ceiling(dblPageCount))
        startIndex = If(currentPage > 1 AndAlso currentPage + pagerSpan - 1 < pagerSpan, currentPage, 1)
        endIndex = If(pageCount > pagerSpan, pagerSpan, pageCount)
        If currentPage > pagerSpan Mod 2 Then
            If currentPage = 2 Then
                endIndex = 3
            Else
                endIndex = currentPage + 2
            End If
        Else
            endIndex = (pagerSpan - currentPage) + 1
        End If

        If endIndex - (pagerSpan - 1) > startIndex Then
            startIndex = endIndex - (pagerSpan - 1)
        End If

        If endIndex > pageCount Then
            endIndex = pageCount
            startIndex = If(((endIndex - pagerSpan) + 1) > 0, (endIndex - pagerSpan) + 1, 1)
        End If

        'Add the First Page Button.
        If currentPage > 1 Then
            pages.Add(New ListItem("First", "1"))
        End If

        'Add the Previous Button.
        If currentPage > 1 Then
            pages.Add(New ListItem("<<", (currentPage - 1).ToString()))
        End If

        For i As Integer = startIndex To endIndex
            pages.Add(New ListItem(i.ToString(), i.ToString(), i <> currentPage))
        Next

        Dim iMod As Integer
        iMod = pageCount Mod pagerSpan
        If iMod = 0 Then
            iMod = 2
        Else
            iMod = 1
        End If
        'Add the Next Button.
        If currentPage < pageCount And pagerSpan < pageCount Then


            If (pagerSpan + currentPage) <= pageCount Or currentPage < pageCount - iMod Then
                If pageCount - (pagerSpan - 1) <> currentPage Then
                    pages.Add(New ListItem(">>", (currentPage + 1).ToString()))

                End If
            End If

        End If

        'Add the Last Button.
        If currentPage <> pageCount And pagerSpan < pageCount Then
            If (pagerSpan + currentPage) <= pageCount Or currentPage < pageCount - iMod Then
                If pageCount - (pagerSpan - 1) <> currentPage Then
                    pages.Add(New ListItem("Last", pageCount.ToString()))
                End If

            End If
        End If
        rptPager.DataSource = pages
        rptPager.DataBind()
    End Sub
    ''' <summary>
    ''' Page_Changed
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Page_Changed(sender As Object, e As EventArgs)
        Dim pageIndex As Integer = Integer.Parse(TryCast(sender, LinkButton).CommandArgument)
        Session("sMailBoxPageIndex") = pageIndex.ToString
        FillInboxByVS(pageIndex)
    End Sub
    ''' <summary>
    ''' GetEmailDetails
    ''' </summary>
    ''' <param name="pageIndex"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <WebMethod()> _
    Public Shared Function GetEmailDetails(pageIndex As Integer) As String

        Dim SqlConn As SqlConnection
        'Dim myCommand As SqlCommand
        Dim myDataAdapter As SqlDataAdapter
        Dim myDS As New DataSet
        Dim strSqlQry As String = "select EmailId,EmailFrom,'Sub: '+EmailSubject EmailSubject,CONVERT(VARCHAR(11), EmailDate, 113) + ' ' +right(convert(varchar(32),EmailDate,100),8)EmailDate,EmailFullSource,EmailAttachment,RIGHT('000000'+CAST(EmailId AS VARCHAR(6)),6)EmailNo	  ,case when (select count(C.EmailId) from Contract_Email C where C.EmailId=Email_Inbox.EmailId) >0 then 'True' else 'False' end HotelStatus   from Email_Inbox where EmailId=" & pageIndex
        SqlConn = clsDBConnect.dbConnectionnew(HttpContext.Current.Session("dbconnectionName"))                             'Open connection
        myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
        myDataAdapter.Fill(myDS, "Details")
        Return myDS.GetXml()
    End Function
    ''' <summary>
    ''' btnProcessclickMail_Click
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnProcessclickMail_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnProcessclickMail.Click

        For Each row As DataListItem In dlMailInbox.Items
            Dim lblId As Label = CType(row.FindControl("lblId"), Label)
            Dim tblRow As HtmlControl = CType(row.FindControl("tblRow"), HtmlControl)
            If lblId.Text = hdEmailCode.Value Then
                tblRow.Attributes.Add("class", "EntryLineActive")

            Else
                tblRow.Attributes.Add("class", "EntryLine")
            End If
        Next row


        trMessage.Visible = False
        Dim strCode As String = hdEmailCode.Value
        Dim SqlConn As SqlConnection
        Dim myDataAdapter As SqlDataAdapter
        Dim myDS As New DataSet
        Dim strAttachment As String = ""
        Dim strSqlQry As String = "select EmailId,EmailFrom,'Sub: '+EmailSubject EmailSubject,CONVERT(VARCHAR(11), EmailDate, 113) + ' ' +right(convert(varchar(32),EmailDate,100),8)EmailDate,EmailFullSource,EmailAttachment,RIGHT('000000'+CAST(EmailId AS VARCHAR(6)),6)EmailNo from Email_Inbox where EmailId=" & strCode
        SqlConn = clsDBConnect.dbConnectionnew(HttpContext.Current.Session("dbconnectionName"))                             'Open connection
        myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
        myDataAdapter.Fill(myDS, "Details")
        If myDS.Tables.Count > 0 Then
            If myDS.Tables(0).Rows.Count > 0 Then
                lblFrom.Text = Server.HtmlEncode(myDS.Tables(0).Rows(0)("EmailFrom").ToString)
                lblDate.Text = myDS.Tables(0).Rows(0)("EmailDate").ToString
                lblSubject.Text = myDS.Tables(0).Rows(0)("EmailSubject").ToString
                lblBody.Text = (myDS.Tables(0).Rows(0)("EmailFullSource").ToString.Replace("img", "img style='Display:none;'")).Replace(":blue", ":#06788B")
                strAttachment = myDS.Tables(0).Rows(0)("EmailAttachment").ToString
                'EmailNo
                If strAttachment.Trim = "Y" Then
                    FillAttachments(strCode)
                Else
                    '    DLAttachments.DataSource = myDS
                    DLAttachments.DataBind()
                End If

            End If
        End If

    End Sub
    ''' <summary>
    ''' FillAttachments
    ''' </summary>
    ''' <param name="strCode"></param>
    ''' <remarks></remarks>
    Private Sub FillAttachments(strCode As String)
        Dim SqlConn As SqlConnection
        Dim myDataAdapter As SqlDataAdapter
        Dim myDS As New DataSet
        Dim strAttachment As String = ""
        Dim strSqlQry As String = "select EmailId,FileName,Content from Email_Inbox_Attachment where EmailId=" & strCode
        SqlConn = clsDBConnect.dbConnectionnew(HttpContext.Current.Session("dbconnectionName"))                             'Open connection
        myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
        myDataAdapter.Fill(myDS, "Details")
        DLAttachments.DataSource = myDS
        DLAttachments.DataBind()

        'If myDS.Tables.Count > 0 Then
        '    If myDS.Tables(0).Rows.Count > 0 Then
        '        lblFrom.Text = Server.HtmlEncode(myDS.Tables(0).Rows(0)("EmailFrom").ToString)
        '        lblDate.Text = myDS.Tables(0).Rows(0)("EmailDate").ToString
        '        lblSubject.Text = myDS.Tables(0).Rows(0)("EmailSubject").ToString
        '        lblBody.Text = myDS.Tables(0).Rows(0)("EmailFullSource").ToString.Replace("img", "img style='Display:none;'")
        '        strAttachment = myDS.Tables(0).Rows(0)("EmailAttachment").ToString
        '        If strAttachment.Trim = "Y" Then
        '            FillAttachments(strCode)
        '        End If
        '    End If
        'End If
    End Sub
    ''' <summary>
    ''' lbAttachment_Click
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub lbAttachment_Click(sender As Object, e As System.EventArgs)
        Try
            Dim myButton As LinkButton = CType(sender, LinkButton)
            Dim dlItem As DataListItem = CType((CType(sender, LinkButton)).NamingContainer, DataListItem)
            Dim lblfile As Label = CType(dlItem.FindControl("lblfile"), Label)

            Dim strpop As String
            strpop = "window.open('Download.aspx?filename=" & lblfile.Text.Trim & "');"
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
        Catch ex As Exception
            '   Dim str As String = ex.Message
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        End Try
    End Sub
    ''' <summary>
    ''' DLAttachments_ItemDataBound
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub DLAttachments_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.DataListItemEventArgs) Handles DLAttachments.ItemDataBound
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then

            Dim lblfile As Label = CType(e.Item.FindControl("lblfile"), Label)
            Dim imgAttachmentType As Image = CType(e.Item.FindControl("imgAttachmentType"), Image)
            Dim filePath As String = "~\Attachment\" '+ lblfile.Text
            Dim path As String = Server.MapPath(filePath)
            '   Dim path As String = "C:\Program Files\Mahce\Attachment"
            '  Dim file As System.IO.FileInfo = New System.IO.FileInfo(path)
            ' Dim str As String = lblfile.Text.Replace(vbCrLf, "").Replace(vbCr, "").Replace("\n", "").Replace("\r", "")
            Dim strExt As String = System.IO.Path.GetExtension(lblfile.Text.Replace(vbCrLf, " ")).ToUpper
            '  Dim strExt As String = System.IO.Path.GetExtension(str).ToUpper
            Select Case strExt.Trim
                Case ".PDF"
                    imgAttachmentType.ImageUrl = "~/Images/crystaltoolbar/pdf.png"
                Case ".JPG", ".JPEG", ".GIF", ".PNG"
                    imgAttachmentType.ImageUrl = "~/Images/crystaltoolbar/jpg file.png"
                Case ".DOCX"
                    imgAttachmentType.ImageUrl = "~/Images/crystaltoolbar/Word_2007.png"
                Case ".DOC"
                    imgAttachmentType.ImageUrl = "~/Images/crystaltoolbar/MS_word2003.png"
                Case ".XLSX", ".XLS"
                    imgAttachmentType.ImageUrl = "~/Images/crystaltoolbar/excel.ico"
                Case Else
                    imgAttachmentType.ImageUrl = "~/Images/crystaltoolbar/mail-attachment2.png"
            End Select

        End If

    End Sub
    ''' <summary>
    ''' chkAssigned_CheckedChanged
    ''' </summary>a
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub chkAssigned_CheckedChanged(sender As Object, e As System.EventArgs)
        Dim myCheckBox As CheckBox = CType(sender, CheckBox)
        If myCheckBox.Checked = True Then
            myCheckBox.Checked = False
        Else
            myCheckBox.Checked = True
        End If
        Dim dlItem As DataListItem = CType((CType(sender, CheckBox)).NamingContainer, DataListItem)

        Dim lblId As Label = CType(dlItem.FindControl("lblId"), Label)
        Dim lblSubject As Label = CType(dlItem.FindControl("lblSubject"), Label)
        lblIdPopup.Text = String.Format("{0:000000}", Convert.ToInt32(lblId.Text))
        Dim strScript As String = "javascript: visualsearchbox();"
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", strScript, True)
        hdPopupStatus.Value = "Y"
        dlList.DataBind()

        FillGridNew()

        mp1.Show()
        'If myCheckBox.Checked Then

        'End If

    End Sub
    ''' <summary>
    ''' btnvsprocess_Click
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnvsprocess_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnvsprocess.Click

        Dim lsSearchTxt As String = ""
        lsSearchTxt = txtvsprocesssplit.Text '.Replace(": """, ":""")
        Dim lsProcessText As String = ""
        Dim lsMainArr As String()
        Dim IsProcessType As String = ""
        Dim IsProcessValue As String = ""
        lsMainArr = objUtils.splitWithWords(lsSearchTxt, "|~,")

        For i = 0 To lsMainArr.GetUpperBound(0)
            Select Case lsMainArr(i).Split(":")(0).ToString.ToUpper.Trim
                Case "HOTELS"
                    lsProcessText = lsMainArr(i).Split(":")(1)
                    ' sbAddToDataTable("HOTELS", lsProcessText, "H")
                    sbAddToDataTable("H", "Hotels", "H")
                    IsProcessType = "H"

                Case "TEXT"
                    lsProcessText = lsMainArr(i).Split(":")(1)
                    ' sbAddToDataTable("HOTELS", lsProcessText, "H")
                    sbAddToDataTable("T", lsProcessText, "T")
                    IsProcessType = "T"
            End Select
        Next

        Dim dttDyn As DataTable
        dttDyn = Session("sDtDynamic")
        dlList.DataSource = dttDyn
        dlList.DataBind()
        FillGridByType(IsProcessType, lsProcessText)
        ShowPopup()
        Dim str As String = ""
    End Sub
    ''' <summary>
    ''' sbAddToDataTable
    ''' </summary>
    ''' <param name="lsName"></param>
    ''' <param name="lsValue"></param>
    ''' <param name="lsShortCode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
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
                dtt.Rows.Add(lsName.Trim, lsValue.Trim)
                Session("sDtDynamic") = dtt
            End If
        End If
        Return True
    End Function

    Function sbAddToDataTableInbox(ByVal lsName As String, ByVal lsValue As String, ByVal lsShortCode As String) As Boolean
        Dim iFlag As Integer = 0
        Dim dtt As DataTable
        dtt = Session("sDtDynamicInbox")
        If dtt.Rows.Count >= 0 Then
            For i = 0 To dtt.Rows.Count - 1
                If dtt.Rows(i)("Code").ToString.Trim.ToUpper = lsName.Trim.ToUpper And dtt.Rows(i)("Value").ToString.Trim.ToUpper = lsValue.Trim.ToUpper Then
                    iFlag = 1
                End If
            Next
            If iFlag = 0 Then
                dtt.NewRow()
                dtt.Rows.Add(lsName.Trim, lsValue.Trim)
                Session("sDtDynamicInbox") = dtt
            End If
        End If
        Return True
    End Function
    ''' <summary>
    ''' Page_Unload
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Page_Unload(sender As Object, e As System.EventArgs) Handles Me.Unload
        ShowPopup()
    End Sub
    ''' <summary>
    ''' lbHotel_Click
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub lbHotel_Click(sender As Object, e As System.EventArgs)
        Dim strlbValue As String = ""

        Dim myButton As LinkButton = CType(sender, LinkButton)

        Dim dlItem As DataListItem = CType((CType(sender, LinkButton)).NamingContainer, DataListItem)
        Dim lb As Label = CType(dlItem.FindControl("lblType"), Label)

        If Not myButton Is Nothing Then
            strlbValue = myButton.Text
            If strlbValue = "Hotels" Then
                strlbValue = "%%"
                hdLinkButtonValue.Value = strlbValue
            Else
                'strlbValue = "%"
                hdLinkButtonValue.Value = myButton.Text & "%"
            End If


            Try
                FillGridByLinkButton()
                FillCheckbox()
                ShowPopup()
            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objUtils.WritErrorLog("ContractTrack.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            Finally


            End Try

        End If
    End Sub
    ''' <summary>
    ''' lbClose_Click
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub lbClose_Click(sender As Object, e As System.EventArgs)
        Try

            Dim dtsHotelDetails As New DataTable
            dtsHotelDetails = Session("sDtHotelDetails")

            Dim myButton As LinkButton = CType(sender, LinkButton)
            Dim dlItem As DataListItem = CType((CType(sender, LinkButton)).NamingContainer, DataListItem)
            Dim lb As LinkButton = CType(dlItem.FindControl("lbHotel"), LinkButton)

            If dtsHotelDetails.Rows.Count > 0 Then

                Dim i As Integer
                For i = dtsHotelDetails.Rows.Count - 1 To 0 Step i - 1
                    'If lb.Text.Trim = dtsHotelDetails.Rows(i)("Type").ToString.Trim Then
                    dtsHotelDetails.Rows.Remove(dtsHotelDetails.Rows(i))
                    'End If
                    dtsHotelDetails.AcceptChanges()
                Next
            End If
            Session("sDtHotelDetails") = dtsHotelDetails

            Dim dtDynamics As New DataTable
            dtDynamics = Session("sDtDynamic")

            If dtDynamics.Rows.Count > 0 Then
                Dim j As Integer
                For j = dtDynamics.Rows.Count - 1 To 0 Step j - 1
                    If lb.Text.Trim = dtDynamics.Rows(j)("Value").ToString.Trim Then
                        dtDynamics.Rows.Remove(dtDynamics.Rows(j))
                    End If
                Next

            End If

            Session("sDtDynamic") = dtDynamics
            dlList.DataSource = dtDynamics
            dlList.DataBind()



            '' Create a Dynamic datatable ---- Start
            Dim ClearDataTable = New DataTable()
            Dim dcGroupDetailsType = New DataColumn("Type", GetType(String))
            Dim dcGroupDetailsCode = New DataColumn("Code", GetType(String))
            Dim dcGroupDetailsCountry = New DataColumn("Value", GetType(String))
            ClearDataTable.Columns.Add(dcGroupDetailsType)
            ClearDataTable.Columns.Add(dcGroupDetailsCode)
            ClearDataTable.Columns.Add(dcGroupDetailsCountry)
            gvHotels.DataSource = ClearDataTable
            gvHotels.DataBind()
            ShowPopup()
        Catch ex As Exception

        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="strType"></param>
    ''' <param name="lsProcessValue"></param>
    ''' <remarks></remarks>
    Private Sub FillGridByType(strType As String, lsProcessValue As String)
        Dim strorderby As String = "partyname"
        Dim strsortorder As String = "ASC"

        Dim myDS As New DataSet
        Dim strWhereCond As String = ""

        '  lblMsg.Visible = False

        If gvHotels.PageIndex < 0 Then
            gvHotels.PageIndex = 0
        End If
        strSqlQry = ""
        Try

            '  strSqlQry = "select partycode HotelCode,partyname HotelName,case when (select count(emailid) from Contract_Email where partycode=HotelId and EmailId=" & lblIdPopup.Text & ") >0 then 'Yes' else 'No' end HotelStatus,'' as TrackingStatus,0 sortorder  from partymast where sptypecode='HOT' and active=1 "
            strSqlQry = "select partycode HotelCode,partyname HotelName,(select h.hotelstatusname from hotelstatus h where h.active=1 and h.hotelstatuscode=partymast.hotelstatuscode)  HotelStatus,(select TrackingStatus from Contract_Email TS where TS.HotelId=partymast.partycode and TS.EmailId=" & lblIdPopup.Text & ") as TrackingStatus,0 sortorder  from partymast where sptypecode='HOT' and active=1 "


            If lsProcessValue.Trim <> "" Then
                lsProcessValue = (Trim(lsProcessValue.Trim).Replace("(H)", ""))
                If strType = "H" Then

                    If Trim(strWhereCond) = "" Then
                        strWhereCond = " upper(partyname) LIKE '" & Trim(lsProcessValue.Trim.ToUpper) & "%'"
                    Else

                        strWhereCond = strWhereCond & " AND  upper(partyname) LIKE '" & Trim(lsProcessValue.Trim.ToUpper) & "%'"
                    End If
                End If
                If strType = "T" Then

                    If Trim(strWhereCond) = "" Then
                        strWhereCond = " upper(partyname) LIKE '%" & Trim(lsProcessValue.Trim.ToUpper) & "%'"
                    Else

                        strWhereCond = strWhereCond & " AND  upper(partyname) LIKE '%" & Trim(lsProcessValue.Trim.ToUpper) & "%'"
                    End If
                End If

            End If
            If Trim(strWhereCond) <> "" Then
                strSqlQry = strSqlQry & " and " & strWhereCond & " ORDER BY " & strorderby & " " & strsortorder
            Else
                strSqlQry = strSqlQry & " ORDER BY " & strorderby & " " & strsortorder
            End If

            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)

            gvHotels.DataBind()
            gvHotels.DataSource = myDS

            If myDS.Tables(0).Rows.Count > 0 Then
                gvHotels.DataBind()
                lblMsg.Visible = False
                lblMsg.Text = ""
            Else
                gvHotels.PageIndex = 0
                gvHotels.DataBind()
                lblMsg.Visible = True
                lblMsg.Text = "Records not found."
            End If

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ContractTrack.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            'clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
            'clsDBConnect.dbConnectionClose(SqlConn)                          'Close connection

        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub chkSelect_CheckedChanged(sender As Object, e As System.EventArgs)
        Try

            Dim ChkBoxRows As CheckBox = CType(sender, CheckBox)
            Dim lblHotelCode As Label = CType(ChkBoxRows.FindControl("lblHotelCode"), Label)
            Dim lblHotelName As Label = CType(ChkBoxRows.FindControl("lblHotelName"), Label)
            '  Dim hdnTrackingStatus As HiddenField = CType(ChkBoxRows.FindControl("hdnTrackingStatus"), HiddenField)
            Dim txtTrackCode As TextBox = CType(ChkBoxRows.FindControl("txtTrackCode"), TextBox)
            Dim row As GridViewRow
            Dim iFlag As Integer = 0
            Dim iFlagCheckedAll As Integer = 0
            Dim iFlagUnCheckedAll As Integer = 0
            Dim dtsHotelDetails As DataTable
            dtsHotelDetails = Session("sDtHotelDetails")

            If ChkBoxRows.Checked = True Then
                ChkBoxRows.Checked = True



                If dtsHotelDetails.Rows.Count >= 0 Then
                    For i = 0 To dtsHotelDetails.Rows.Count - 1
                        If dtsHotelDetails.Rows(i)("Code").ToString = lblHotelCode.Text Then
                            iFlag = 1
                            Exit For
                        End If
                    Next
                    If iFlag = 0 Then
                        dtsHotelDetails.NewRow()
                        dtsHotelDetails.Rows.Add("H", lblHotelCode.Text, lblHotelName.Text, txtTrackCode.Text)
                        Session("sDtHotelDetails") = dtsHotelDetails

                    End If

                End If
            Else

                ChkBoxRows.Checked = False
                If dtsHotelDetails.Rows.Count >= 0 Then
                    For i = 0 To dtsHotelDetails.Rows.Count - 1
                        If dtsHotelDetails.Rows(i)("Code").Trim.ToString = lblHotelCode.Text.Trim Then
                            dtsHotelDetails.Rows.Remove(dtsHotelDetails.Rows(i))
                            Exit For
                        End If
                    Next

                End If
                Session("sDtHotelDetails") = dtsHotelDetails
            End If

            ' Check all check box is checked  or not.
            Dim ChkBoxHeader As CheckBox = CType(gvHotels.HeaderRow.FindControl("chkSelectAll"), CheckBox)
            Dim row1 As GridViewRow
            For Each row1 In gvHotels.Rows
                Dim ChkBoxRows1 As CheckBox = CType(row1.FindControl("chkSelect"), CheckBox)
                If ChkBoxRows1.Checked = True Then
                    iFlagCheckedAll = 0
                Else
                    iFlagCheckedAll = 1
                    Exit For
                End If

            Next

            If iFlagCheckedAll = 0 And ChkBoxHeader.Checked = False Then
                ChkBoxHeader.Checked = True
            End If
            If iFlagCheckedAll = 1 And ChkBoxHeader.Checked = True Then
                ChkBoxHeader.Checked = False
            End If


            Dim iFlagS As Integer = 0
            Dim dtt As DataTable
            dtt = Session("sDtDynamic")
            If dtt.Rows.Count >= 0 Then

                If dtsHotelDetails.Rows.Count > 0 Then

                    For j = 0 To dtt.Rows.Count - 1
                        If dtt.Rows(j)("Value").ToString = "Hotels" Then
                            iFlagS = 1
                        End If
                    Next
                    If iFlagS = 0 Then
                        dtt.NewRow()
                        dtt.Rows.Add("H", "Hotels")
                        Session("sDtDynamic") = dtt
                    End If

                End If
            End If
            Session("sDtDynamic") = dtt
            dlList.DataSource = dtt
            dlList.DataBind()



            Dim txttrackingStatus As TextBox = CType(ChkBoxRows.FindControl("txttrackingStatus"), TextBox)
            If ChkBoxRows.Checked = True Then
                txttrackingStatus.Visible = True
                If txttrackingStatus.Text = "" Then
                    txttrackingStatus.Text = "Open"
                    txtTrackCode.Text = "Open"
                End If
            Else
                txttrackingStatus.Visible = False
            End If

            ShowPopup()
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ContractTrack.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub chkSelectAll_CheckedChanged(sender As Object, e As System.EventArgs)
        Try


            Dim ChkBoxHeader As CheckBox = CType(gvHotels.HeaderRow.FindControl("chkSelectAll"), CheckBox)
            Dim row As GridViewRow
            Dim iFlag As Integer = 0
            Dim dtsHotelDetails As DataTable
            dtsHotelDetails = Session("sDtHotelDetails")

            For Each row In gvHotels.Rows
                Dim ChkBoxRows As CheckBox = CType(row.FindControl("chkSelect"), CheckBox)
                Dim lblHotelCode As Label = CType(row.FindControl("lblHotelCode"), Label)
                Dim lblHotelName As Label = CType(row.FindControl("lblHotelName"), Label)
                ' Dim hdnTrackingStatus As HiddenField = CType(ChkBoxRows.FindControl("hdnTrackingStatus"), HiddenField)
                Dim txtTrackCode As TextBox = CType(ChkBoxRows.FindControl("txtTrackCode"), TextBox)
                If ChkBoxHeader.Checked = True Then
                    ChkBoxRows.Checked = True
                    iFlag = 0
                    If dtsHotelDetails.Rows.Count >= 0 Then
                        For i = 0 To dtsHotelDetails.Rows.Count - 1
                            If dtsHotelDetails.Rows(i)("Code").ToString = lblHotelCode.Text Then
                                iFlag = 1
                            End If
                        Next
                        If iFlag = 0 Then
                            dtsHotelDetails.NewRow()

                            dtsHotelDetails.Rows.Add("H", lblHotelCode.Text, lblHotelName.Text, txtTrackCode.Text)
                            Session("sDtHotelDetails") = dtsHotelDetails

                        End If

                    End If

                Else
                    ChkBoxRows.Checked = False
                    If dtsHotelDetails.Rows.Count >= 0 Then
                        For i = 0 To dtsHotelDetails.Rows.Count - 1
                            If dtsHotelDetails.Rows(i)("Code").ToString = lblHotelCode.Text Then
                                dtsHotelDetails.Rows.Remove(dtsHotelDetails.Rows(i))
                                Exit For
                            End If
                        Next

                    End If
                    Session("sDtHotelDetails") = dtsHotelDetails
                End If

                Dim iFlagS As Integer = 0
                Dim dtt As DataTable
                dtt = Session("sDtDynamic")
                If dtt.Rows.Count >= 0 Then

                    If dtsHotelDetails.Rows.Count > 0 Then

                        For j = 0 To dtt.Rows.Count - 1
                            If dtt.Rows(j)("Value").ToString = "Hotels" Then
                                iFlagS = 1
                            End If
                        Next
                        If iFlagS = 0 Then
                            dtt.NewRow()
                            dtt.Rows.Add("H", "Hotels")
                            Session("sDtDynamic") = dtt
                        End If

                    End If
                End If
                Session("sDtDynamic") = dtt
                dlList.DataSource = dtt
                dlList.DataBind()




                Dim txttrackingStatus As TextBox = CType(row.FindControl("txttrackingStatus"), TextBox)
                If ChkBoxRows.Checked = True Then
                    txttrackingStatus.Visible = True
                    If txttrackingStatus.Text = "" Then
                        txttrackingStatus.Text = "Open"
                        txtTrackCode.Text = "Open"
                    End If
                Else
                    txttrackingStatus.Visible = False
                End If

            Next
            ShowPopup()
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ContractTrack.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub FillGridByLinkButton()
        Dim strorderby As String = "partymast.partyname"
        Dim strsortorder As String = "ASC"
        Dim myDS As New DataSet
        Dim strWhereCond As String = ""
        gvHotels.Visible = True

        Dim strlbValue As String = hdLinkButtonValue.Value
        If gvHotels.PageIndex < 0 Then
            gvHotels.PageIndex = 0
        End If


        Dim dtsHotelDetailsFilter As DataTable
        Dim strCodes As String = ""
        dtsHotelDetailsFilter = Session("sDtHotelDetails")
        If dtsHotelDetailsFilter.Rows.Count > 0 Then
            For j As Integer = 0 To dtsHotelDetailsFilter.Rows.Count - 1
                strCodes = strCodes & "'" & dtsHotelDetailsFilter.Rows(j)("Code").ToString().Trim & "',"
            Next
        Else
            If strlbValue = "%" Then
                strCodes = "'''"
            ElseIf strlbValue = "%%" Then
                strCodes = ""
                strlbValue = "%"
            End If

        End If

        strSqlQry = ""

        strSqlQry = "select partycode HotelCode,partyname HotelName,(select h.hotelstatusname from hotelstatus h where h.active=1 and h.hotelstatuscode=partymast.hotelstatuscode)  HotelStatus,(select TrackingStatus from Contract_Email TS where TS.HotelId=partymast.partycode and TS.EmailId=" & lblIdPopup.Text & ") as TrackingStatus,0 sortorder  from partymast where sptypecode='HOT' and active=1 "

        If strlbValue.Trim <> "" Then


            strlbValue = "'" & strlbValue & "'"
            If Trim(strWhereCond) = "" Then
                strWhereCond = " upper(partymast.partyname) LIKE " & Trim(strlbValue) & ""
            Else

                strWhereCond = strWhereCond & " AND upper(partymast.partyname) LIKE " & Trim(strlbValue) & ""
            End If

            If strCodes <> "" Then
                strCodes = strCodes.Remove(strCodes.Length - 1)
                If Trim(strWhereCond) = "" Then
                    strWhereCond = " upper(partymast.partycode) IN (" & Trim(strCodes) & ")"
                Else

                    strWhereCond = strWhereCond & " AND upper(partymast.partycode) IN  (" & Trim(strCodes) & ")"
                End If
            End If


        End If


        If Trim(strWhereCond) <> "" Then
            strSqlQry = strSqlQry & " and " & strWhereCond & " ORDER BY " & strorderby & " " & strsortorder
        Else
            strSqlQry = strSqlQry & " ORDER BY " & strorderby & " " & strsortorder
        End If

        SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
        myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
        myDataAdapter.Fill(myDS)
        gvHotels.DataBind()


        Dim dtsHotelDetails As DataTable
        Dim strValues As String = ""
        Dim strQuery As String = ""
        dtsHotelDetails = Session("sDtHotelDetails")

        If myDS.Tables(0).Rows.Count > 0 Then

            For i As Integer = 0 To myDS.Tables(0).Rows.Count - 1
                strValues = ""
                strValues = myDS.Tables(0).Rows(i)("HotelCode").ToString
                If dtsHotelDetails.Rows.Count > 0 Then
                    For j As Integer = 0 To dtsHotelDetails.Rows.Count - 1
                        If dtsHotelDetails.Rows(j)("Code").ToString().Trim = strValues.Trim Then
                            myDS.Tables(0).Rows(i)("sortorder") = 1
                            Exit For
                        End If
                    Next

                End If
            Next
        End If
        'End If

        Dim dataView As DataView = New DataView(myDS.Tables(0))
        dataView.Sort = "sortorder desc, HotelName asc"
        gvHotels.DataSource = dataView
        If myDS.Tables(0).Rows.Count > 0 Then
            gvHotels.DataBind()
        Else
            gvHotels.PageIndex = 0
            gvHotels.DataBind()
            lblMsg.Visible = True
            lblMsg.Text = "Records not found."
        End If
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub FillCheckbox()
        Dim dtsHotelGroupDetails As DataTable
        dtsHotelGroupDetails = Session("sDtHotelDetails")

        Dim row As GridViewRow
        Dim iFlag As Integer = 0
        If dtsHotelGroupDetails.Rows.Count > 0 Then

            For Each row In gvHotels.Rows

                Dim ChkBoxRows As CheckBox = CType(row.FindControl("chkSelect"), CheckBox)

                Dim lblHotelCode As Label = CType(row.FindControl("lblHotelCode"), Label)
                Dim lblHotelName As Label = CType(row.FindControl("lblHotelName"), Label)
                Dim txttrackingStatus As TextBox = CType(row.FindControl("txttrackingStatus"), TextBox)

                For i As Integer = 0 To dtsHotelGroupDetails.Rows.Count - 1
                    If dtsHotelGroupDetails.Rows(i)("Code").ToString.Trim = lblHotelCode.Text.Trim Then
                        ChkBoxRows.Checked = True
                        txttrackingStatus.Visible = True
                        Exit For
                    Else
                        ChkBoxRows.Checked = False
                        txttrackingStatus.Visible = False
                    End If

                Next
            Next

        End If
    End Sub

    Private Sub ShowPopup()
        If Not hdPopupStatus Is Nothing Then
            If hdPopupStatus.Value = "Y" Then
                mp1.Show()
            Else
                mp1.Hide()
            End If

        End If
    End Sub

    Protected Sub btnClose_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles btnClose.Click
        ClosePopUp()
    End Sub

    Protected Sub btnSave_Click(sender As Object, e As System.EventArgs) Handles btnSave.Click

        Dim result1 As Integer = 0
        Dim frmmode As String = 0
        Dim strPassQry As String = "false"
        Try

            If Page.IsValid = True Then


                Dim dtsHotelDetails As DataTable
                dtsHotelDetails = Session("sDtHotelDetails")
                'If dtsHotelDetails.Rows.Count <= 0 Then
                '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('The hotels are not selected.');", True)
                '    Exit Sub
                'End If

                SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                sqlTrans = SqlConn.BeginTransaction           'SQL  Trans start
                myCommand = New SqlCommand("sp_Contract_Email", SqlConn, sqlTrans)

                Dim strBuffer As New Text.StringBuilder
                Dim strTrackStat As String = ""
                If dtsHotelDetails.Rows.Count > 0 Then

                    strBuffer.Append("<Hotels>")
                    For i = 0 To dtsHotelDetails.Rows.Count - 1
                        strTrackStat = ""
                        strTrackStat = dtsHotelDetails.Rows(i)("TrackingStatus").ToString
                        If strTrackStat = "" Then
                            strTrackStat = "Open"
                        End If
                        strBuffer.Append("<Hotel>")
                        strBuffer.Append(" <PartyCode>" & dtsHotelDetails.Rows(i)("Code").ToString & " </PartyCode>")
                        strBuffer.Append(" <TrackingStatus>" & strTrackStat & " </TrackingStatus>")
                        strBuffer.Append("</Hotel>")
                    Next
                    strBuffer.Append("</Hotels>")
                End If

                myCommand.CommandType = CommandType.StoredProcedure
                myCommand.Parameters.Add(New SqlParameter("@EmailCode", SqlDbType.VarChar, 50)).Value = lblIdPopup.Text.Trim
                myCommand.Parameters.Add(New SqlParameter("@ValidXMLInput", SqlDbType.Xml)).Value = strBuffer.ToString
                myCommand.ExecuteNonQuery()

                sqlTrans.Commit()    'SQl Tarn Commit
                clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
                clsDBConnect.dbCommandClose(myCommand)               'sql command disposed
                clsDBConnect.dbConnectionClose(SqlConn)           'connection close

                If Not Session("sMailBoxPageIndex") Is Nothing Then
                    FillMailBox(Session("sMailBoxPageIndex"))
                Else
                    FillMailBox(1)
                End If

                '  ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Saved Successfully..');", True)
                '  FillGridNew()
                ClosePopUp()

                lblFrom.Text = ""
                lblDate.Text = ""
                lblSubject.Text = ""
                lblBody.Text = ""
                DLAttachments.DataBind()
  
                FillTracking()
                FillMailBoxIgnore(1)

            End If
        Catch ex As Exception
            If SqlConn.State = ConnectionState.Open Then
                sqlTrans.Rollback()

            End If

            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)

            objUtils.WritErrorLog("ContractTrack.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub

    Private Sub FillGridNew()
        hdLinkButtonValue.Value = "%"
        BindDataTable()
        FillGridByLinkButton()
        FillCheckbox()
        ShowPopup()
    End Sub

    Private Sub BindDataTable()


        Dim ClearDataTable = New DataTable()
        Dim dcGroupDetailsType = New DataColumn("Type", GetType(String))
        Dim dcGroupDetailsCode = New DataColumn("Code", GetType(String))
        Dim dcGroupDetailsCountry = New DataColumn("Value", GetType(String))
        Dim dcTrackingStatus = New DataColumn("TrackingStatus", GetType(String))
        ClearDataTable.Columns.Add(dcGroupDetailsType)
        ClearDataTable.Columns.Add(dcGroupDetailsCode)
        ClearDataTable.Columns.Add(dcGroupDetailsCountry)
        ClearDataTable.Columns.Add(dcTrackingStatus)
        Session("sDtHotelDetails") = ClearDataTable

        Dim dtsHotelDetails As DataTable
        dtsHotelDetails = Session("sDtHotelDetails")

        strSqlQry = ""
        Try



            strSqlQry = "select hotelid,TrackingStatus from Contract_Email where  EmailId=" & lblIdPopup.Text
            Dim dt As New DataTable
            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(dt)


            Dim strHotelid As String
            Dim strTrackingStatus As String
            Dim iFlag As Integer
            For Each row In dt.Rows
                strHotelid = row("hotelid").ToString
                strTrackingStatus = row("TrackingStatus").ToString
                iFlag = 0
                If dtsHotelDetails.Rows.Count >= 0 Then
                    For i = 0 To dtsHotelDetails.Rows.Count - 1
                        If dtsHotelDetails.Rows(i)("Code").ToString = strHotelid Then
                            iFlag = 1
                            Exit For
                        End If
                    Next
                    If iFlag = 0 Then
                        dtsHotelDetails.NewRow()

                        dtsHotelDetails.Rows.Add("H", strHotelid, "", strTrackingStatus)
                        Session("sDtHotelDetails") = dtsHotelDetails

                    End If

                End If



                Dim iFlagS As Integer = 0
                Dim dtt As DataTable
                dtt = Session("sDtDynamic")
                If dtt.Rows.Count >= 0 Then
                    dtt.Rows.Clear()
                    If dtsHotelDetails.Rows.Count > 0 Then

                        For j = 0 To dtt.Rows.Count - 1
                            If dtt.Rows(j)("Value").ToString = "Hotels" Then
                                iFlagS = 1
                                Exit For
                            End If
                        Next
                        If iFlagS = 0 Then
                            dtt.NewRow()
                            dtt.Rows.Add("H", "Hotels")
                            Session("sDtDynamic") = dtt
                        End If

                    End If
                End If
                Session("sDtDynamic") = dtt
                dlList.DataSource = dtt
                dlList.DataBind()
            Next
        Catch ex As Exception

        End Try
    End Sub

    <System.Web.Script.Services.ScriptMethod()> _
   <System.Web.Services.WebMethod()> _
    Public Shared Function GetTrackingStatus(ByVal prefixText As String) As List(Of String)
        Dim strSqlQry As String = ""
        Dim myDS As New DataSet
        Dim TrackStatusName As New List(Of String)
        Try
            prefixText = prefixText.Replace(" ", "")
            strSqlQry = "select TrackCode,TrackStatusName from TrackingStatusMaster where TrackStatusName like '" & prefixText & "%' "
            Dim SqlConn As New SqlConnection
            Dim myDataAdapter As New SqlDataAdapter
            SqlConn = clsDBConnect.dbConnectionnew("strDBConnection")
            'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)
            If myDS.Tables(0).Rows.Count > 0 Then
                For i As Integer = 0 To myDS.Tables(0).Rows.Count - 1
                    TrackStatusName.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(myDS.Tables(0).Rows(i)("TrackStatusName").ToString(), myDS.Tables(0).Rows(i)("TrackCode").ToString()))
                Next
            End If
            Return TrackStatusName
        Catch ex As Exception
            Return TrackStatusName
        End Try
    End Function

    <System.Web.Script.Services.ScriptMethod()> _
  <System.Web.Services.WebMethod()> _
    Public Shared Function GetUpdateType(ByVal prefixText As String) As List(Of String)
        Dim strSqlQry As String = ""
        Dim myDS As New DataSet
        Dim strTypeName As New List(Of String)
        Try
            prefixText = prefixText.Replace(" ", "")
            strSqlQry = "select typecode,typename from trackupdatetype where active=1  and typename like '%" & prefixText & "%' "
            Dim SqlConn As New SqlConnection
            Dim myDataAdapter As New SqlDataAdapter
            SqlConn = clsDBConnect.dbConnectionnew("strDBConnection")
            'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)
            If myDS.Tables(0).Rows.Count > 0 Then
                For i As Integer = 0 To myDS.Tables(0).Rows.Count - 1
                    strTypeName.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(myDS.Tables(0).Rows(i)("typename").ToString(), myDS.Tables(0).Rows(i)("typecode").ToString()))
                Next
            End If
            Return strTypeName
        Catch ex As Exception
            Return strTypeName
        End Try
    End Function

    <System.Web.Script.Services.ScriptMethod()> _
  <System.Web.Services.WebMethod()> _
    Public Shared Function GetAssignedTo(ByVal prefixText As String) As List(Of String)
        Dim strSqlQry As String = ""
        Dim myDS As New DataSet
        Dim strUserName As New List(Of String)
        Try
            prefixText = prefixText.Replace(" ", "")
            strSqlQry = " select UserCode,USERNAME from usermaster where deptcode='CON' and active=1 and USERNAME like '%" & prefixText & "%' "
            Dim SqlConn As New SqlConnection
            Dim myDataAdapter As New SqlDataAdapter
            SqlConn = clsDBConnect.dbConnectionnew("strDBConnection")
            'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)
            If myDS.Tables(0).Rows.Count > 0 Then
                For i As Integer = 0 To myDS.Tables(0).Rows.Count - 1
                    strUserName.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(myDS.Tables(0).Rows(i)("USERNAME").ToString(), myDS.Tables(0).Rows(i)("UserCode").ToString()))
                Next
            End If
            Return strUserName
        Catch ex As Exception
            Return strUserName
        End Try
    End Function

    <System.Web.Script.Services.ScriptMethod()> _
  <System.Web.Services.WebMethod()> _
    Public Shared Function GetApprover(ByVal prefixText As String) As List(Of String)
        Dim strSqlQry As String = ""
        Dim myDS As New DataSet
        Dim strUserName As New List(Of String)
        Try
            prefixText = prefixText.Replace(" ", "")
            strSqlQry = " select UserCode,USERNAME from usermaster where deptcode='CON' and active=1 and USERNAME like '%" & prefixText & "%' "
            Dim SqlConn As New SqlConnection
            Dim myDataAdapter As New SqlDataAdapter
            SqlConn = clsDBConnect.dbConnectionnew("strDBConnection")
            'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)
            If myDS.Tables(0).Rows.Count > 0 Then
                For i As Integer = 0 To myDS.Tables(0).Rows.Count - 1
                    strUserName.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(myDS.Tables(0).Rows(i)("USERNAME").ToString(), myDS.Tables(0).Rows(i)("UserCode").ToString()))
                Next
            End If
            Return strUserName
        Catch ex As Exception
            Return strUserName
        End Try
    End Function

    <System.Web.Script.Services.ScriptMethod()> _
<System.Web.Services.WebMethod()> _
    Public Shared Function GetReApprover(ByVal prefixText As String) As List(Of String)
        Dim strSqlQry As String = ""
        Dim myDS As New DataSet
        Dim strUserName As New List(Of String)
        Try
            prefixText = prefixText.Replace(" ", "")
            strSqlQry = " select UserCode,USERNAME from usermaster where deptcode='CON' and active=1 and USERNAME like '%" & prefixText & "%' "
            Dim SqlConn As New SqlConnection
            Dim myDataAdapter As New SqlDataAdapter
            SqlConn = clsDBConnect.dbConnectionnew("strDBConnection")
            'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)
            If myDS.Tables(0).Rows.Count > 0 Then
                For i As Integer = 0 To myDS.Tables(0).Rows.Count - 1
                    strUserName.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(myDS.Tables(0).Rows(i)("USERNAME").ToString(), myDS.Tables(0).Rows(i)("UserCode").ToString()))
                Next
            End If
            Return strUserName
        Catch ex As Exception
            Return strUserName
        End Try
    End Function

    Protected Sub gvHotels_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvHotels.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then

            Dim txtTrackingStatus As TextBox = e.Row.FindControl("txtTrackingStatus")
            Dim txtTrackCode As TextBox = e.Row.FindControl("txtTrackCode")
            Dim chkSelect As CheckBox = e.Row.FindControl("chkSelect")
            If chkSelect.Checked = True Then
                txtTrackingStatus.Visible = True
                If txtTrackingStatus.Text = "" Then
                    txtTrackingStatus.Text = "Open"
                    txtTrackCode.Text = "Open"
                End If
            Else
                txtTrackingStatus.Visible = False
            End If
        End If
    End Sub

    Protected Sub txtTrackingStatus_TextChanged(sender As Object, e As System.EventArgs)
        Dim TextBoxRows As TextBox = CType(sender, TextBox)
        Dim lblHotelCode As Label = CType(TextBoxRows.FindControl("lblHotelCode"), Label)
        Dim lblHotelName As Label = CType(TextBoxRows.FindControl("lblHotelName"), Label)
        Dim chkSelect As CheckBox = CType(TextBoxRows.FindControl("chkSelect"), CheckBox)
        '  Dim hdnTrackingStatus As HiddenField = CType(ChkBoxRows.FindControl("hdnTrackingStatus"), HiddenField)
        Dim txtTrackCode As TextBox = CType(TextBoxRows.FindControl("txtTrackCode"), TextBox)
        Dim txtTrackingStatus As TextBox = CType(TextBoxRows.FindControl("txtTrackingStatus"), TextBox)
        Dim row As GridViewRow
        Dim iFlag As Integer = 0
        Dim iFlagCheckedAll As Integer = 0
        Dim iFlagUnCheckedAll As Integer = 0
        Dim dtsHotelDetails As DataTable
        dtsHotelDetails = Session("sDtHotelDetails")
        Dim strQuery As String = ""
        Dim obj As New clsUtils
        If chkSelect.Checked = True Then
            If dtsHotelDetails.Rows.Count >= 0 Then
                For i = 0 To dtsHotelDetails.Rows.Count - 1
                    If dtsHotelDetails.Rows(i)("Code").ToString.Trim = lblHotelCode.Text.Trim Then
                        strQuery = "select TrackCode from trackingstatusmaster where TrackStatusName='" & txtTrackingStatus.Text.Trim & "'"
                        If txtTrackingStatus.Text = obj.GetString((HttpContext.Current.Session("dbconnectionName")).ToString, strQuery) Then
                            dtsHotelDetails.Rows(i)("TrackingStatus") = txtTrackCode.Text
                        Else
                            txtTrackCode.Text = ""
                            txtTrackingStatus.Text = ""
                        End If
                    End If
                Next
            End If
            Session("sDtHotelDetails") = dtsHotelDetails
        End If
        ShowPopup()
    End Sub

    Private Sub ClosePopUp()

        hdPopupStatus.Value = "N"
        mp1.Hide()
        Dim dtsHotelDetails As New DataTable
        dtsHotelDetails = Session("sDtHotelDetails")
        '' Clear Dynamic datatable ---- Start
        Dim ClearDataTable = New DataTable()
        Dim dcGroupDetailsType = New DataColumn("Type", GetType(String))
        Dim dcGroupDetailsCode = New DataColumn("Code", GetType(String))
        Dim dcGroupDetailsCountry = New DataColumn("Value", GetType(String))
        Dim dcTrackingStatus = New DataColumn("TrackingStatus", GetType(String))
        ClearDataTable.Columns.Add(dcGroupDetailsType)
        ClearDataTable.Columns.Add(dcGroupDetailsCode)
        ClearDataTable.Columns.Add(dcGroupDetailsCountry)
        ClearDataTable.Columns.Add(dcTrackingStatus)
        Session("sDtHotelDetails") = ClearDataTable

        Dim dtDynamic = New DataTable()
        Dim dcCode = New DataColumn("Code", GetType(String))
        Dim dcCountry = New DataColumn("Value", GetType(String))
        dtDynamic.Columns.Add(dcCode)
        dtDynamic.Columns.Add(dcCountry)
        Session("sDtDynamic") = dtDynamic
        dlList.DataSource = dtDynamic
        dlList.DataBind()

    End Sub

    Protected Sub btnvsprocessInbox_Click(sender As Object, e As System.EventArgs) Handles btnvsprocessInbox.Click
        Dim lsSearchTxt As String = ""
        lsSearchTxt = txtvsprocesssplitInbox.Text.Replace("___", "<").Replace("...", ">")
        Dim lsProcessText As String = ""
        Dim lsMainArr As String()
        Dim IsProcessType As String = ""
        Dim IsProcessValue As String = ""
        lsMainArr = objUtils.splitWithWords(lsSearchTxt, "|~,")

        For i = 0 To lsMainArr.GetUpperBound(0)
            Select Case lsMainArr(i).Split(":")(0).ToString.ToUpper.Trim
                Case "HOTELS"
                    lsProcessText = lsMainArr(i).Split(":")(1)
                    sbAddToDataTableInbox("HOTELS", lsProcessText, "H")
                    IsProcessType = "H"
                Case "TICKECT NO"
                    lsProcessText = lsMainArr(i).Split(":")(1)
                    sbAddToDataTableInbox("TICKECT NO", lsProcessText, "TN")
                    IsProcessType = "TN"
                Case "HOTEL_STATUS"
                    lsProcessText = lsMainArr(i).Split(":")(1)
                    sbAddToDataTableInbox("HOTEL_STATUS", lsProcessText, "HS")
                    IsProcessType = "HS"
                Case "TRACKING_STATUS"
                    lsProcessText = lsMainArr(i).Split(":")(1)
                    sbAddToDataTableInbox("TRACKING_STATUS", lsProcessText, "TS")
                    IsProcessType = "TS"
                Case "FROM_EMAIL"
                    lsProcessText = lsMainArr(i).Split(":")(1)
                    sbAddToDataTableInbox("FROM_EMAIL", lsProcessText, "FM")
                    IsProcessType = "FM"
                Case "TEXT"
                    lsProcessText = lsMainArr(i).Split(":")(1)
                    sbAddToDataTableInbox("TEXT", lsProcessText, "T")
                    IsProcessType = "T"
            End Select
        Next

        Dim dttDyn As DataTable
        dttDyn = Session("sDtDynamicInbox")
        dlInboxSearch.DataSource = dttDyn
        dlInboxSearch.DataBind()
        FillInboxByVS(1)

    End Sub

    Protected Sub lbCloseInboxCategory_Click(sender As Object, e As System.EventArgs)
        Try

            'Dim dtsHotelDetails As New DataTable
            'dtsHotelDetails = Session("sDtHotelDetails")

            Dim myButton As LinkButton = CType(sender, LinkButton)
            Dim dlItem As DataListItem = CType((CType(sender, LinkButton)).NamingContainer, DataListItem)
            Dim lb As LinkButton = CType(dlItem.FindControl("lbInboxCategory"), LinkButton)

            'If dtsHotelDetails.Rows.Count > 0 Then

            '    Dim i As Integer
            '    For i = dtsHotelDetails.Rows.Count - 1 To 0 Step i - 1
            '        'If lb.Text.Trim = dtsHotelDetails.Rows(i)("Type").ToString.Trim Then
            '        dtsHotelDetails.Rows.Remove(dtsHotelDetails.Rows(i))
            '        'End If
            '        dtsHotelDetails.AcceptChanges()
            '    Next
            'End If
            'Session("sDtHotelDetails") = dtsHotelDetails

            Dim dtDynamicsInbox As New DataTable
            dtDynamicsInbox = Session("sDtDynamicInbox")

            If dtDynamicsInbox.Rows.Count > 0 Then
                Dim j As Integer
                For j = dtDynamicsInbox.Rows.Count - 1 To 0 Step j - 1
                    If lb.Text.Trim = dtDynamicsInbox.Rows(j)("Value").ToString.Trim Then
                        dtDynamicsInbox.Rows.Remove(dtDynamicsInbox.Rows(j))
                    End If
                Next

            End If

            Session("sDtDynamicInbox") = dtDynamicsInbox
            dlInboxSearch.DataSource = dtDynamicsInbox
            dlInboxSearch.DataBind()

            FillInboxByVS(1)

            ' '' Create a Dynamic datatable ---- Start
            'Dim ClearDataTable = New DataTable()
            'Dim dcGroupDetailsType = New DataColumn("Type", GetType(String))
            'Dim dcGroupDetailsCode = New DataColumn("Code", GetType(String))
            'Dim dcGroupDetailsCountry = New DataColumn("Value", GetType(String))
            'ClearDataTable.Columns.Add(dcGroupDetailsType)
            'ClearDataTable.Columns.Add(dcGroupDetailsCode)
            'ClearDataTable.Columns.Add(dcGroupDetailsCountry)
            'gvHotels.DataSource = ClearDataTable
            'gvHotels.DataBind()
            'ShowPopup()
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub lbInboxCategory_Click(sender As Object, e As System.EventArgs)
        Dim strlbValue As String = ""

        Dim myButton As LinkButton = CType(sender, LinkButton)

        Dim dlItem As DataListItem = CType((CType(sender, LinkButton)).NamingContainer, DataListItem)
        Dim lb As Label = CType(dlItem.FindControl("lblType"), Label)

        If Not myButton Is Nothing Then
            strlbValue = myButton.Text
            If strlbValue = "Hotels" Then
                strlbValue = "%%"
                hdLinkButtonValue.Value = strlbValue
            Else
                'strlbValue = "%"
                hdLinkButtonValue.Value = myButton.Text & "%"
            End If


            Try
                FillGridByLinkButton()
                FillCheckbox()
                ShowPopup()
            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objUtils.WritErrorLog("ContractTrack.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            Finally


            End Try

        End If
    End Sub

    Public Shared Function GetDetailsPageWise(ByVal pageIndex As Integer, ByVal pageSize As Integer, ByVal strQuery As String) As DataSet
        Dim strConnName As String = (HttpContext.Current.Session("dbconnectionName")).ToString
        Dim constring As String = ConfigurationManager.ConnectionStrings(strConnName).ConnectionString
        Using con As New SqlConnection(constring)
            Using cmd As New SqlCommand("[GetDetailsPageWise]")
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.AddWithValue("@PageIndex", pageIndex)
                cmd.Parameters.AddWithValue("@PageSize", pageSize)
                cmd.Parameters.AddWithValue("@SqlQuery", strQuery)
                cmd.Parameters.Add("@PageCount", SqlDbType.Int, 4).Direction = ParameterDirection.Output
                Using sda As New SqlDataAdapter()
                    cmd.Connection = con
                    sda.SelectCommand = cmd
                    Using ds As New DataSet()
                        sda.Fill(ds, "Customers")
                        Dim dt As New DataTable("PageCount")
                        dt.Columns.Add("PageCount")
                        dt.Rows.Add()
                        dt.Rows(0)(0) = cmd.Parameters("@PageCount").Value
                        ds.Tables.Add(dt)
                        Return ds
                    End Using
                End Using
            End Using
        End Using
    End Function

    Private Sub FillInboxByVS(pageIndex As Integer)
        Dim dtt As DataTable
        dtt = Session("sDtDynamicInbox")



        Dim strHotelValue As String = ""
        Dim strTicketValue As String = ""
        Dim strHotelStatusValue As String = ""
        Dim strTrackingStatusValue As String = ""
        Dim strFromEmailValue As String = ""
        Dim strTextValue As String = ""
        Dim strBindCondition As String = ""


        If dtt.Rows.Count > 0 Then
            For i As Integer = 0 To dtt.Rows.Count - 1
                If dtt.Rows(i)("Code").ToString = "HOTELS" Then
                    If strHotelValue <> "" Then
                        strHotelValue = strHotelValue + ",'" + dtt.Rows(i)("Value").ToString + "'"
                    Else
                        strHotelValue = "'" + dtt.Rows(i)("Value").ToString + "'"
                    End If
                End If

                If dtt.Rows(i)("Code").ToString = "TICKECT NO" Then
                    If strTicketValue <> "" Then
                        strTicketValue = strTicketValue + ",'" + dtt.Rows(i)("Value").ToString + "'"
                    Else
                        strTicketValue = "'" + dtt.Rows(i)("Value").ToString + "'"
                    End If
                End If
                If dtt.Rows(i)("Code").ToString = "HOTEL_STATUS" Then
                    If strHotelStatusValue <> "" Then
                        strHotelStatusValue = strHotelStatusValue + ",'" + dtt.Rows(i)("Value").ToString + "'"
                    Else
                        strHotelStatusValue = "'" + dtt.Rows(i)("Value").ToString + "'"
                    End If
                End If
                If dtt.Rows(i)("Code").ToString = "TRACKING_STATUS" Then
                    If strTrackingStatusValue <> "" Then
                        strTrackingStatusValue = strTrackingStatusValue + ",'" + dtt.Rows(i)("Value").ToString + "'"
                    Else
                        strTrackingStatusValue = "'" + dtt.Rows(i)("Value").ToString + "'"
                    End If
                End If
                If dtt.Rows(i)("Code").ToString = "FROM_EMAIL" Then
                    If strFromEmailValue <> "" Then
                        strFromEmailValue = strFromEmailValue + ",'" + dtt.Rows(i)("Value").ToString + "'"
                    Else
                        strFromEmailValue = "'" + dtt.Rows(i)("Value").ToString + "'"
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


        Dim strWhereCond As String = ""
        lblMsg.Visible = False


        strSqlQry = ""
        Try

            strSqlQry = "SELECT ROW_NUMBER() OVER (ORDER BY [EmailId] desc)AS RowNumber ,EmailId,EmailFrom,'Sub: '+EmailSubject EmailSubject,case when CONVERT(VARCHAR(11), EmailDate, 111)=CONVERT(VARCHAR(11), getdate(), 111) then right(convert(varchar(32),EmailDate,100),8)else CONVERT(VARCHAR(11), EmailDate, 113) end EmailDate,RIGHT('000000'+CAST(EmailId AS VARCHAR(6)),6) EmailNo ,case when (select count(C.EmailId) from Contract_Email C where C.EmailId=Email_Inbox.EmailId) >0 then 'True' else 'False' end HotelStatus from Email_Inbox "
            strWhereCond = " ((select count(C.EmailId) from Contract_Email C where C.EmailId=Email_Inbox.EmailId) < 1)  and  EmailId not in (select distinct AdditionalEmailId   from Contract_Email_Additional) "
            If strHotelValue <> "" Then
                If Trim(strWhereCond) = "" Then

                    strWhereCond = " EmailId in (select C.EmailId from Contract_Email C where C.HotelId in (select P.partycode from partymast P where upper(P.partyname)= " & Trim(strHotelValue.Trim.ToUpper) & ")) "

                Else
                    strWhereCond = strWhereCond & " AND EmailId in (select C.EmailId from Contract_Email C where C.HotelId in (select P.partycode from partymast P where upper(P.partyname)= " & Trim(strHotelValue.Trim.ToUpper) & ")) "
                End If
            End If


            If strTicketValue <> "" Then
                If Trim(strWhereCond) = "" Then
                    strWhereCond = " RIGHT('000000'+CAST(EmailId AS VARCHAR(6)),6)  IN ( " & Trim(strTicketValue) & ")"
                Else

                    strWhereCond = strWhereCond & " AND RIGHT('000000'+CAST(EmailId AS VARCHAR(6)),6)  IN ( " & Trim(strTicketValue) & ")"
                End If
            End If

            If strHotelStatusValue <> "" Then
                If Trim(strWhereCond) = "" Then
                    strWhereCond = " EmailId in (select distinct EmailId from Contract_Email where HotelId in(select distinct partycode from partymast where hotelstatuscode in (select h.hotelstatuscode  from hotelstatus h where h.active=1 and h.hotelstatusname=" & Trim(strHotelStatusValue) & ")))"
                Else
                    strWhereCond = strWhereCond & " AND EmailId in (select distinct EmailId from Contract_Email where HotelId in(select distinct partycode from partymast where hotelstatuscode in (select h.hotelstatuscode  from hotelstatus h where h.active=1 and h.hotelstatusname=" & Trim(strHotelStatusValue) & ")))"
                End If
            End If

            If strTrackingStatusValue <> "" Then
                If Trim(strWhereCond) = "" Then
                    strWhereCond = "  EmailId in (select C.EmailId from Contract_Email C where C.TrackingStatus=(" & Trim(strTrackingStatusValue) & ")) "
                Else

                    strWhereCond = strWhereCond & " AND   EmailId in (select C.EmailId from Contract_Email C where C.TrackingStatus=(" & Trim(strTrackingStatusValue) & ")) "
                End If
            End If

            If strFromEmailValue <> "" Then
                If Trim(strWhereCond) = "" Then
                    strWhereCond = " upper(EmailFrom) IN ( " & Trim(strFromEmailValue) & ")"
                Else

                    strWhereCond = strWhereCond & " AND upper(EmailFrom) IN (" & Trim(strFromEmailValue) & ")"
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
                            strWhereCond1 = " (EmailId in (select C.EmailId from Contract_Email C where C.HotelId in (select P.partycode from partymast P where upper(P.partyname) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%'  or  RIGHT('000000'+CAST(EmailId AS VARCHAR(6)),6)  LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' or   EmailId in (select distinct EmailId from Contract_Email where HotelId in(select distinct partycode from partymast where hotelstatuscode in (select h.hotelstatuscode  from hotelstatus h where h.active=1 and h.hotelstatusname like '%" & Trim(strValue.Trim.ToUpper) & "%')))  or  EmailId in (select C.EmailId from Contract_Email C where C.TrackingStatus LIKE ('%" & Trim(strValue.Trim.ToUpper) & "%')) or  upper(EmailFrom) LIKE ('%" & Trim(strValue.Trim.ToUpper) & "%'))))"
                        Else
                            strWhereCond1 = strWhereCond1 & " OR    (EmailId in (select C.EmailId from Contract_Email C where C.HotelId in (select P.partycode from partymast P where upper(P.partyname) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%'  or  RIGHT('000000'+CAST(EmailId AS VARCHAR(6)),6)  LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' or  EmailId in (select distinct EmailId from Contract_Email where HotelId in(select distinct partycode from partymast where hotelstatuscode in (select h.hotelstatuscode  from hotelstatus h where h.active=1 and h.hotelstatusname like '%" & Trim(strValue.Trim.ToUpper) & "%')))  or  EmailId in (select C.EmailId from Contract_Email C where C.TrackingStatus LIKE ('%" & Trim(strValue.Trim.ToUpper) & "%')) or  upper(EmailFrom) LIKE ('%" & Trim(strValue.Trim.ToUpper) & "%'))))"
                        End If
                    End If
                Next
                If Trim(strWhereCond) = "" Then
                    strWhereCond = " (" & strWhereCond1 & ")"
                Else
                    strWhereCond = strWhereCond & " AND (" & strWhereCond1 & ")"
                End If

            End If



            If Trim(strWhereCond) <> "" Then
                strSqlQry = strSqlQry & " Where " & strWhereCond
            Else
                strSqlQry = strSqlQry
            End If



            Dim myDS As New DataSet
            Dim constring As String = ConfigurationManager.ConnectionStrings("strDBConnection").ConnectionString
            Using con As New SqlConnection(constring)
                Using cmd As New SqlCommand("GetEmailsPageWise_VisualSearch", con)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.Parameters.AddWithValue("@PageIndex", pageIndex)
                    cmd.Parameters.AddWithValue("@PageSize", PageSize)
                    cmd.Parameters.AddWithValue("@SqlQuery", strSqlQry)
                    cmd.Parameters.Add("@RecordCount", SqlDbType.Int, 4)
                    cmd.Parameters("@RecordCount").Direction = ParameterDirection.Output
                    con.Open()
                    Dim idr As IDataReader = cmd.ExecuteReader()
                    dlMailInbox.DataSource = idr
                    dlMailInbox.DataBind()
                    idr.Close()
                    con.Close()
                    Dim recordCount As Integer = Convert.ToInt32(cmd.Parameters("@RecordCount").Value)
                    Me.PopulatePager(recordCount, pageIndex)
                End Using
            End Using





            'SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
            'myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            'myDataAdapter.Fill(myDS)

            'dlMailInbox.DataBind()
            'dlMailInbox.DataSource = myDS

            'If myDS.Tables(0).Rows.Count > 0 Then
            '    dlMailInbox.DataBind()
            'Else
            '    '  dlMailInbox.PageIndex = 0
            '    dlMailInbox.DataBind()
            '    lblMsg.Visible = True
            '    lblMsg.Text = "Records not found, Please redefine search criteria."
            'End If

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ContractsTrack.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally


        End Try
    End Sub

    Private Sub FillCurrentDayEmailCount()
        Dim strQuery As String = "select COUNT(EmailId)CNT  from Email_Inbox where convert(varchar(10),EmailDate,101)=convert(varchar(10),getdate(),101)"
        Dim obj As New clsUtils
        lblEmailCount.Text = "Todays Email Count: " & obj.GetString((HttpContext.Current.Session("dbconnectionName")).ToString, strQuery)
    End Sub

    Private Sub FillTracking()
        Dim strSqlQry As String = ""
        Dim myDS As New DataSet
        Dim TrackStatusName As New List(Of String)
        Try
            'strSqlQry = "select EmailLineNo,EmailId,EmailNo,HotelCode,rtrim(HotelName)HotelName,HotelStatus,TrackingStatus,EmailSubject,EmailDate,EmailTime,ProgressStage,UpdateStart,UpdateEnd,ApprovalStart,ApprovalEnd,UpdateTypeCode,UpdateTypeName,convert(varchar(10),ValidFrom,103)ValidFrom,convert(varchar(10),ValidTo,103)ValidTo,convert(varchar(16),AssignedDate,103)+ ' ' + convert(varchar(5),AssignedDate,108) AssignedDate,AssignedTo,AssignedToName,Approver from VIEW_TRACKING  where TrackingStatus<>'Ignore' and ApprovalEnd is null order by convert(datetime,isnull(AssignedDate,getdate())) desc "
            strSqlQry = "select EmailLineNo,EmailId,EmailNo,HotelCode,rtrim(HotelName)HotelName,HotelStatus,TrackingStatus,EmailSubject,EmailDate,EmailTime,ProgressStage,UpdateStart,UpdateEnd,ApprovalStart,ApprovalEnd,UpdateTypeCode,UpdateTypeName,convert(varchar(10),ValidFrom,103)ValidFrom,convert(varchar(10),ValidTo,103)ValidTo,convert(varchar(16),AssignedDate,103) AssignedDate,AssignedTo,AssignedToName,Approver from VIEW_TRACKING  where TrackingStatus<>'Ignore' and ApprovalEnd is null order by convert(datetime,isnull(AssignedDate,getdate())) desc "
            Dim SqlConn As New SqlConnection
            Dim myDataAdapter As New SqlDataAdapter
            SqlConn = clsDBConnect.dbConnectionnew("strDBConnection")
            'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)
            gvTracking.DataSource = myDS
            gvTracking.DataBind()

        Catch ex As Exception
        End Try
    End Sub

    Private Sub FillTrackingActioned()
        Dim strSqlQry As String = ""
        Dim myDS As New DataSet
        Dim TrackStatusName As New List(Of String)
        Try
            strSqlQry = "select EmailLineNo,EmailId,EmailNo,HotelCode,rtrim(HotelName)HotelName,HotelStatus,TrackingStatus,EmailSubject,EmailDate,EmailTime,ProgressStage,UpdateStart,UpdateEnd,ApprovalStart,ApprovalEnd,UpdateTypeCode,UpdateTypeName,convert(varchar(10),ValidFrom,103)ValidFrom,convert(varchar(10),ValidTo,103)ValidTo,convert(varchar(16),AssignedDate,103) AssignedDate,AssignedTo,AssignedToName,Approver from VIEW_TRACKING where ApprovalEnd is not null order by EmailLineNo"
            Dim SqlConn As New SqlConnection
            Dim myDataAdapter As New SqlDataAdapter
            SqlConn = clsDBConnect.dbConnectionnew("strDBConnection")
            'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)
            gvTrackingView.DataSource = myDS
            gvTrackingView.DataBind()

        Catch ex As Exception
        End Try
    End Sub

    Protected Sub gvTracking_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvTracking.RowDataBound
        Try

            'gvTracking.Columns("yourColumnName").Frozen = True
            If e.Row.RowType = DataControlRowType.Header Then
                e.Row.Cells(0).CssClass = "lockedHeader"
                e.Row.Cells(2).CssClass = "lockedHeader"
                e.Row.Cells(4).CssClass = "lockedHeader"
                e.Row.Cells(6).CssClass = "lockedHeader"
                e.Row.Cells(8).CssClass = "lockedHeaderLast"
                ' e.Row.Cells(8).CssClass = "lockedHeaderNext"


            Else
                If iFlag = 0 Then
                    e.Row.Cells(0).CssClass = "locked"
                    e.Row.Cells(2).CssClass = "locked"
                    e.Row.Cells(4).CssClass = "locked"
                    e.Row.Cells(6).CssClass = "locked"
                    e.Row.Cells(8).CssClass = "lockedLast"
                    iFlag = 1
                Else
                    e.Row.Cells(0).CssClass = "lockedAlternative"
                    e.Row.Cells(2).CssClass = "lockedAlternative"
                    e.Row.Cells(4).CssClass = "lockedAlternative"
                    e.Row.Cells(6).CssClass = "lockedAlternative"
                    e.Row.Cells(8).CssClass = "lockedAlternativeLast"
                    'e.Row.Cells(8).CssClass = "lockedAlternativeNext"
                    iFlag = 0
                End If

            End If

            If e.Row.RowType = DataControlRowType.DataRow Then

                Dim lblProgressStage As Label = CType(e.Row.FindControl("lblProgressStage"), Label)
                If lblProgressStage.Text = "Clarification Pending" Then
                    'e.Row.CssClass = "Pending"
                    e.Row.BackColor = System.Drawing.Color.PeachPuff
                End If
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        End Try
    End Sub

    Public Sub SetDate()
        Try
            'Dim mCount As Integer = 0
            'For i As Integer = 0 To gvTracking.Rows.Count - 1
            '    Dim txtFromDate As TextBox = gvTracking.Rows(i).FindControl("txtvalidFrom")
            '    Dim txtToDate As TextBox = gvTracking.Rows(i).FindControl("txtvalidto")
            '    If mCount = 0 Then
            '        dpTxtFromDate.Text = txtFromDate.Text
            '        dptxtTodate.Text = txtToDate.Text
            '    Else
            '        If txtFromDate.Text <> "" Then
            '            If CDate(txtFromDate.Text) < CDate(dpTxtFromDate.Text) Then
            '                dpTxtFromDate.Text = txtFromDate.Text
            '            End If
            '        End If
            '        If txtToDate.Text <> "" Then
            '            If CDate(txtToDate.Text) > CDate(dptxtTodate.Text) Then
            '                dptxtTodate.Text = txtToDate.Text
            '            End If
            '        End If
            '    End If
            '    mCount = 1
            'Next

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ContractTracking.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub

    Protected Sub lbUpdate_Click(sender As Object, e As System.EventArgs)

        Try

            btnUpdatePopup.Visible = True
            btnCancelPopup.Visible = True


            txtUpdateType.ReadOnly = False
            txtValidFrom.ReadOnly = False
            txtValidTo.ReadOnly = False
            txtAssignedDate.ReadOnly = False
            txtAssignedTo.ReadOnly = False
            chkAssign.Enabled = True
            ImgValidFrom.Enabled = True
            ImgtxtValidTo.Enabled = True

            Dim lbUpdate As LinkButton = CType(sender, LinkButton)
            Dim gvRow As GridViewRow = CType(lbUpdate.NamingContainer, GridViewRow)
            Dim hdEmailId_ As HiddenField = CType(gvRow.FindControl("hdEmailId_"), HiddenField)
            Dim hdCHotelCode As HiddenField = CType(gvRow.FindControl("hdCHotelCode"), HiddenField)
            Dim lblAssignedToCode As Label = CType(gvRow.FindControl("lblAssignedToCode"), Label)
            Dim lblApprover As Label = CType(gvRow.FindControl("lblApprover"), Label)     
            hdPopupHotelCode.Value = hdCHotelCode.Value
            hdPopupMailId.Value = hdEmailId_.Value


            BindTrackingPopupFields(hdPopupMailId.Value, hdPopupHotelCode.Value)

            Dim lblEmailCode As Label = CType(gvRow.FindControl("lblEmailCode"), Label)
            Dim lblCHotelName As Label = CType(gvRow.FindControl("lblCHotelName"), Label)
            Dim lblTrackingStatus As Label = CType(gvRow.FindControl("lblTrackingStatus"), Label)
            If lblTrackingStatus.Text.Trim = "In Progress" Or lblTrackingStatus.Text.Trim = "Ignore" Then
                btnUpdatePopup.Enabled = False
                txtUpdateType.ReadOnly = True
                txtValidFrom.ReadOnly = True
                txtValidTo.ReadOnly = True
                txtAssignedDate.ReadOnly = True
                txtAssignedTo.ReadOnly = True
                chkAssign.Enabled = False
                ImgValidFrom.Enabled = False
                ImgtxtValidTo.Enabled = False
            Else
                btnUpdatePopup.Enabled = True
                txtUpdateType.ReadOnly = False
                txtValidFrom.ReadOnly = False
                txtValidTo.ReadOnly = False
                txtAssignedDate.ReadOnly = False
                txtAssignedTo.ReadOnly = False
                chkAssign.Enabled = True
                ImgValidFrom.Enabled = True
                ImgtxtValidTo.Enabled = True

            End If

            '  txtFromDate.Attributes.Add("onchange", "setdate();")
            txtValidTo.Attributes.Add("onchange", "checkdates('" & txtValidFrom.ClientID & "','" & txtValidTo.ClientID & "');")
            txtValidFrom.Attributes.Add("onchange", "checkfromdates('" & txtValidFrom.ClientID & "','" & txtValidTo.ClientID & "');")
            Dim str As String = Date.Now.ToString("dd/MM/yyyy") '& " " & Date.Now.ToString("hh:mm")
            If txtAssignedDate.Text <> "" Then
                str = txtAssignedDate.Text
            Else
                txtAssignedDate.Text = str
            End If

            chkAssign.Attributes.Add("onchange", "setCurrentDate('" & txtAssignedDate.ClientID & "','" & chkAssign.ClientID & "','" & str & "');")
            chkApprovalTime.Attributes.Add("onchange", "setCurrentAppDate('" & txtApprovalAssDateTime.ClientID & "','" & chkApprovalTime.ClientID & "','" & str & "');")
            Dim SqlConnNew As SqlConnection
            Dim myDataAdapterNew As SqlDataAdapter
            Dim myDT As New DataTable
            Dim strSqlQryNew As String = "select ClarifyRemarks from Contract_Clarify where HotelId='" & hdPopupHotelCode.Value & "' and MailId='" & hdPopupMailId.Value & "' and AssignedUser='" & lblAssignedToCode.Text & "' and Clarified=0"
            SqlConnNew = clsDBConnect.dbConnectionnew(HttpContext.Current.Session("dbconnectionName"))                             'Open connection
            myDataAdapterNew = New SqlDataAdapter(strSqlQryNew, SqlConnNew)
            myDataAdapterNew.Fill(myDT)

            If myDT.Rows.Count = 1 Then
                chkClarified.Visible = True
                tr1.Visible = True
                tr2.Visible = False
                tr3.Visible = False
                tr4.Visible = False

                trApprovalAssdate.Visible = False
                trApproverAss.Visible = False
                tr7.Visible = False
                tr8.Visible = False
                tr9.Visible = False
                tr10.Visible = False
            Else
                chkClarified.Visible = False
                tr1.Visible = False
                tr2.Visible = False
                tr3.Visible = False
                tr4.Visible = False

                trApprovalAssdate.Visible = False
                trApproverAss.Visible = False
                tr7.Visible = False
                tr8.Visible = False
                tr9.Visible = False
                tr10.Visible = False
            End If


            Dim strSqlQryAppr As String = "select count(EmailId)cnt from Contract_Email where TaskCompleteDate is not null and HotelId='" & hdPopupHotelCode.Value & "' and EMailId='" & hdPopupMailId.Value & "'"
            Dim iCntAppr As Integer = objUtils.ExecuteQueryReturnStringValuenew(HttpContext.Current.Session("dbconnectionName"), strSqlQryAppr)
            If iCntAppr > 0 Then
                trApprovalAssdate.Visible = True
                trApproverAss.Visible = True
                tr7.Visible = False
                tr8.Visible = False
                tr9.Visible = False
                tr10.Visible = False
                Dim strSqlQryClar As String = "select count(ClarifyId)cnt from Contract_ClarifyForApproval where HotelId='" & hdPopupHotelCode.Value & "' and MailId='" & hdPopupMailId.Value & "' and AssignedUser='" & lblApprover.Text & "' and Clarified=0"
                ' Dim strSqlQryClar As String = "select (ClarifyId)cnt from Contract_Clarify"
                Dim iCntApprClar As Integer = objUtils.ExecuteQueryReturnStringValuenew(HttpContext.Current.Session("dbconnectionName"), strSqlQryClar)
                If iCntApprClar > 0 Then
                    tr7.Visible = True
                    tr8.Visible = True
                    tr9.Visible = True
                    tr10.Visible = True
                End If
            End If

            Dim strCode As String = hdEmailId_.Value
            Dim SqlConn As SqlConnection
            Dim myDataAdapter As SqlDataAdapter
            Dim myDS As New DataSet
            Dim strAttachment As String = ""
            lblEmailCodePopup.Text = lblEmailCode.Text
            lblHotelNamePopup.Text = "Hotel Name: " & lblCHotelName.Text
            Dim strSqlQry As String = "select EmailId,EmailFrom,'Sub: '+EmailSubject EmailSubject,CONVERT(VARCHAR(11), EmailDate, 113) + ' ' +right(convert(varchar(32),EmailDate,100),8)EmailDate,EmailFullSource,EmailAttachment,RIGHT('000000'+CAST(EmailId AS VARCHAR(6)),6)EmailNo from Email_Inbox where EmailId=" & strCode
            SqlConn = clsDBConnect.dbConnectionnew(HttpContext.Current.Session("dbconnectionName"))                             'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS, "Details")
            If myDS.Tables.Count > 0 Then
                If myDS.Tables(0).Rows.Count > 0 Then
                    lblFromPopup.Text = Server.HtmlEncode(myDS.Tables(0).Rows(0)("EmailFrom").ToString)
                    lblDatePopup.Text = myDS.Tables(0).Rows(0)("EmailDate").ToString
                    lblSubjectPopup.Text = myDS.Tables(0).Rows(0)("EmailSubject").ToString
                    lblBodyPopup.Text = (myDS.Tables(0).Rows(0)("EmailFullSource").ToString.Replace("img", "img style='Display:none;'")).Replace(":blue", ":#06788B")
                    strAttachment = myDS.Tables(0).Rows(0)("EmailAttachment").ToString
                    'EmailNo
                    If strAttachment.Trim = "Y" Then
                        FillAttachmentsPopup(strCode)
                    Else
                        DLAttachmentPopup.DataBind()
                    End If

                    'strAttachment = myDS.Tables(0).Rows(0)("EmailAttachment").ToString
                    ''EmailNo
                    'If strAttachment.Trim = "Y" Then
                    '    FillAttachments(strCode)
                    'Else
                    '    '    DLAttachments.DataSource = myDS
                    '    DLAttachments.DataBind()
                    'End If

                End If
            End If

            FillClarifyGrid()
            FillClarifyForApprovalGrid()
            FillAdditionalEmails(strCode, hdCHotelCode.Value)

            hdTrackpopupStatus.Value = "Y"
            txtUpdateType.Focus()
            meContractTracking.Show()

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ContractTracking.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub

    Protected Sub btnCancelPopup_Click(sender As Object, e As System.EventArgs) Handles btnCancelPopup.Click
        txtAssignedTo.Text = ""
        txtUpdateType.Text = ""
        txtUpdateTypeCode.Text = ""
        txtValidFrom.Text = ""
        txtValidTo.Text = ""
        txtAssignedDate.Text = ""
        txtReAssignedDate.Text = ""
        txtReAssignedToCode.Text = ""
        txtApprovalAssDateTime.Text = ""
        txtApprovalReassignTime.Text = ""
        txtApprover.Text = ""
        txtApproverCode.Text = ""
        txtReApproverCode.Text = ""
        txtReApprover.Text = ""
        meContractTracking.Hide()
    End Sub

    Protected Sub lbCloseTrack_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles lbCloseTrack.Click
        txtAssignedTo.Text = ""
        txtUpdateType.Text = ""
        txtUpdateTypeCode.Text = ""
        txtValidFrom.Text = ""
        txtValidTo.Text = ""
        txtAssignedDate.Text = ""
        txtReAssignedToCode.Text = ""
        txtApprovalAssDateTime.Text = ""
        txtApprovalReassignTime.Text = ""
        txtApprover.Text = ""
        txtApproverCode.Text = ""
        txtReApproverCode.Text = ""
        txtReApprover.Text = ""
        hdTrackpopupStatus.Value = "N"
        meContractTracking.Hide()
    End Sub

    Protected Sub btnvsprocessPopup_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnvsprocessPopup.Click
        Dim lsSearchTxt As String = ""
        lsSearchTxt = txtvsprocesssplitPopup.Text.Replace("___", "<").Replace("...", ">")
        Dim lsProcessText As String = ""
        Dim lsMainArr As String()
        Dim IsProcessType As String = ""
        Dim IsProcessValue As String = ""
        lsMainArr = objUtils.splitWithWords(lsSearchTxt, "|~,")

        For i = 0 To lsMainArr.GetUpperBound(0)
            Select Case lsMainArr(i).Split(":")(0).ToString.ToUpper.Trim
                Case "HOTELS"
                    lsProcessText = lsMainArr(i).Split(":")(1)
                    sbAddToDataTablePopup("HOTELS", lsProcessText, "H")
                    IsProcessType = "H"
                Case "EMAIL CODE"
                    lsProcessText = lsMainArr(i).Split(":")(1)
                    sbAddToDataTablePopup("EMAIL CODE", lsProcessText, "EC")
                    IsProcessType = "TN"
                Case "HOTEL STATUS"
                    lsProcessText = lsMainArr(i).Split(":")(1)
                    sbAddToDataTablePopup("HOTEL STATUS", lsProcessText, "HS")
                    IsProcessType = "HS"
                Case "TRACKING STATUS"
                    lsProcessText = lsMainArr(i).Split(":")(1)
                    sbAddToDataTablePopup("TRACKING STATUS", lsProcessText, "TS")
                    IsProcessType = "TS"
                Case "FROM EMAIL"
                    lsProcessText = lsMainArr(i).Split(":")(1)
                    sbAddToDataTablePopup("FROM EMAIL", lsProcessText, "FM")
                    IsProcessType = "FM"
                Case "TEXT"
                    lsProcessText = lsMainArr(i).Split(":")(1)
                    sbAddToDataTablePopup("TEXT", lsProcessText, "T")
                    IsProcessType = "T"
                Case "EMAIL DATE"
                    lsProcessText = lsMainArr(i).Split(":")(1)
                    sbAddToDataTablePopup("EMAIL DATE", lsProcessText, "ED")
                    IsProcessType = "ED"
                Case "EMAIL SUBJECT"
                    lsProcessText = lsMainArr(i).Split(":")(1)
                    sbAddToDataTablePopup("EMAIL SUBJECT", lsProcessText, "ES")
                    IsProcessType = "ES"
                Case "UPDATE TYPE"
                    lsProcessText = lsMainArr(i).Split(":")(1)
                    sbAddToDataTablePopup("UPDATE TYPE", lsProcessText, "UT")
                    IsProcessType = "UT"
                Case "ASSIGNED TO"
                    lsProcessText = lsMainArr(i).Split(":")(1)
                    sbAddToDataTablePopup("ASSIGNED TO", lsProcessText, "AT")
                    IsProcessType = "AT"
                Case "PROGRESS STAGE"
                    lsProcessText = lsMainArr(i).Split(":")(1)
                    sbAddToDataTablePopup("PROGRESS STAGE", lsProcessText, "PS")
                    IsProcessType = "PS"
            End Select
        Next

        Dim dttDyn As DataTable
        dttDyn = Session("sDtDynamicPopup")
        dlTrackingVS.DataSource = dttDyn
        dlTrackingVS.DataBind()
        FillTrackingForVS()
    End Sub

    Private Sub FillTrackingForVS()


        Dim dtt As DataTable
        dtt = Session("sDtDynamicPopup")

        Dim strHotelValue As String = ""
        Dim strEmailcodeValue As String = ""
        Dim strHotelStatusValue As String = ""
        Dim strTrackingStatusValue As String = ""
        Dim strFromEmailValue As String = ""
        Dim strEmailDateValue As String = ""
        Dim strEmailSubjectValue As String = ""
        Dim strUpdateType As String = ""
        Dim strAssignedTo As String = ""
        Dim strProgressStage As String = ""
        Dim strTextValue As String = ""
        Dim strBindCondition As String = ""


        If dtt.Rows.Count > 0 Then
            For i As Integer = 0 To dtt.Rows.Count - 1
                If dtt.Rows(i)("Code").ToString = "HOTELS" Then
                    If strHotelValue <> "" Then
                        strHotelValue = strHotelValue + ",'" + dtt.Rows(i)("Value").ToString + "'"
                    Else
                        strHotelValue = "'" + dtt.Rows(i)("Value").ToString + "'"
                    End If
                End If

                If dtt.Rows(i)("Code").ToString = "EMAIL CODE" Then
                    If strEmailcodeValue <> "" Then
                        strEmailcodeValue = strEmailcodeValue + ",'" + dtt.Rows(i)("Value").ToString + "'"
                    Else
                        strEmailcodeValue = "'" + dtt.Rows(i)("Value").ToString + "'"
                    End If
                End If
                If dtt.Rows(i)("Code").ToString = "HOTEL STATUS" Then
                    If strHotelStatusValue <> "" Then
                        strHotelStatusValue = strHotelStatusValue + ",'" + dtt.Rows(i)("Value").ToString + "'"
                    Else
                        strHotelStatusValue = "'" + dtt.Rows(i)("Value").ToString + "'"
                    End If
                End If
                If dtt.Rows(i)("Code").ToString = "TRACKING STATUS" Then
                    If strTrackingStatusValue <> "" Then
                        strTrackingStatusValue = strTrackingStatusValue + ",'" + dtt.Rows(i)("Value").ToString + "'"
                    Else
                        strTrackingStatusValue = "'" + dtt.Rows(i)("Value").ToString + "'"
                    End If
                End If
                If dtt.Rows(i)("Code").ToString = "FROM EMAIL" Then
                    If strFromEmailValue <> "" Then
                        strFromEmailValue = strFromEmailValue + ",'" + dtt.Rows(i)("Value").ToString + "'"
                    Else
                        strFromEmailValue = "'" + dtt.Rows(i)("Value").ToString + "'"
                    End If
                End If
                If dtt.Rows(i)("Code").ToString = "EMAIL DATE" Then
                    If strEmailDateValue <> "" Then
                        strEmailDateValue = strEmailDateValue + ",'" + dtt.Rows(i)("Value").ToString + "'"
                    Else
                        strEmailDateValue = "'" + dtt.Rows(i)("Value").ToString + "'"
                    End If
                End If
                If dtt.Rows(i)("Code").ToString = "EMAIL SUBJECT" Then
                    If strEmailSubjectValue <> "" Then
                        strEmailSubjectValue = strEmailSubjectValue + ",'" + dtt.Rows(i)("Value").ToString + "'"
                    Else
                        strEmailSubjectValue = "'" + dtt.Rows(i)("Value").ToString + "'"
                    End If
                End If

                If dtt.Rows(i)("Code").ToString = "TEXT" Then
                    If strTextValue <> "" Then
                        strTextValue = strTextValue + "," + dtt.Rows(i)("Value").ToString
                    Else
                        strTextValue = dtt.Rows(i)("Value").ToString
                    End If
                End If
                If dtt.Rows(i)("Code").ToString = "UPDATE TYPE" Then
                    If strUpdateType <> "" Then
                        strUpdateType = strUpdateType + ", '" + dtt.Rows(i)("Value").ToString + "'"
                    Else
                        strUpdateType = "'" + dtt.Rows(i)("Value").ToString + "'"
                    End If
                End If
                If dtt.Rows(i)("Code").ToString = "ASSIGNED TO" Then
                    If strAssignedTo <> "" Then
                        strAssignedTo = strAssignedTo + "," + dtt.Rows(i)("Value").ToString
                    Else
                        strAssignedTo = dtt.Rows(i)("Value").ToString
                    End If
                End If
                If dtt.Rows(i)("Code").ToString = "PROGRESS STAGE" Then
                    If strProgressStage <> "" Then
                        strProgressStage = strProgressStage + ", '" + dtt.Rows(i)("Value").ToString + "'"
                    Else
                        strProgressStage = "'" + dtt.Rows(i)("Value").ToString + "'"
                    End If
                End If
            Next
        End If


        Dim strWhereCond As String = ""
        lblMsg.Visible = False


        Dim strSqlQry As String = ""


        strSqlQry = "select EmailLineNo,EmailId,EmailNo,HotelCode,rtrim(HotelName)HotelName,HotelStatus,TrackingStatus,EmailSubject,EmailDate,EmailTime,ProgressStage,UpdateStart,UpdateEnd,ApprovalStart,ApprovalEnd,UpdateTypeCode,UpdateTypeName,convert(varchar(10),ValidFrom,103)ValidFrom,convert(varchar(10),ValidTo,103)ValidTo,convert(varchar(16),AssignedDate,103) AssignedDate,AssignedTo,AssignedToName,Approver from VIEW_TRACKING "

        If strHotelValue <> "" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = " upper(HotelName) = " & Trim(strHotelValue.Trim.ToUpper) & " "
            Else
                strWhereCond = strWhereCond & " AND upper(HotelName) = " & Trim(strHotelValue.Trim.ToUpper) & " "
            End If
        End If


        If strEmailcodeValue <> "" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = " RIGHT('000000'+CAST(EmailId AS VARCHAR(6)),6)  IN ( " & Trim(strEmailcodeValue) & ")"
            Else

                strWhereCond = strWhereCond & " AND RIGHT('000000'+CAST(EmailId AS VARCHAR(6)),6)  IN ( " & Trim(strEmailcodeValue) & ")"
            End If
        End If

        If strHotelStatusValue <> "" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = " upper(HotelStatus) =" & Trim(strHotelStatusValue) & ""
            Else
                strWhereCond = strWhereCond & " AND HotelStatus =" & Trim(strHotelStatusValue) & ""
            End If
        End If

        If strTrackingStatusValue <> "" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = " upper(TrackingStatus)=" & Trim(strTrackingStatusValue) & ""
            Else

                strWhereCond = strWhereCond & " AND   TrackingStatus=" & Trim(strTrackingStatusValue) & ""
            End If
        End If

        If strFromEmailValue <> "" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = " upper(EmailFrom) IN ( " & Trim(strFromEmailValue) & ")"
            Else

                strWhereCond = strWhereCond & " AND upper(EmailFrom) IN (" & Trim(strFromEmailValue) & ")"
            End If
        End If

        If strEmailDateValue <> "" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = " upper(EmailDate) IN ( " & Trim(strEmailDateValue) & ")"
            Else

                strWhereCond = strWhereCond & " AND upper(EmailDate) IN (" & Trim(strEmailDateValue) & ")"
            End If
        End If
        If strEmailSubjectValue <> "" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = " upper(EmailSubject) IN ( " & Trim(strEmailSubjectValue) & ")"
            Else

                strWhereCond = strWhereCond & " AND upper(EmailSubject) IN (" & Trim(strEmailSubjectValue) & ")"
            End If
        End If
        If strProgressStage <> "" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = " upper(ProgressStage) IN ( " & Trim(strProgressStage) & ")"
            Else

                strWhereCond = strWhereCond & " AND upper(ProgressStage) IN (" & Trim(strProgressStage) & ")"
            End If
        End If

        If strUpdateType <> "" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = " upper(UpdateTypeName) IN ( " & Trim(strUpdateType) & ")"
            Else

                strWhereCond = strWhereCond & " AND upper(UpdateTypeName) IN (" & Trim(strUpdateType) & ")"
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
                        strWhereCond1 = "  upper(HotelName) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%'  or  RIGHT('000000'+CAST(EmailId AS VARCHAR(6)),6)  LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' or   HotelStatus like '%" & Trim(strValue.Trim.ToUpper) & "%' or TrackingStatus LIKE ('%" & Trim(strValue.Trim.ToUpper) & "%') or  upper(EmailFrom) LIKE ('%" & Trim(strValue.Trim.ToUpper) & "%') or  upper(EmailSubject) LIKE ('%" & Trim(strValue.Trim.ToUpper) & "%')  or  upper(EmailDate) LIKE ('%" & Trim(strValue.Trim.ToUpper) & "%') "
                    Else
                        strWhereCond1 = strWhereCond1 & " OR  upper(HotelName) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%'  or  RIGHT('000000'+CAST(EmailId AS VARCHAR(6)),6)  LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' or   HotelStatus like '%" & Trim(strValue.Trim.ToUpper) & "%' or TrackingStatus LIKE ('%" & Trim(strValue.Trim.ToUpper) & "%') or  upper(EmailFrom) LIKE ('%" & Trim(strValue.Trim.ToUpper) & "%') or  upper(EmailSubject) LIKE ('%" & Trim(strValue.Trim.ToUpper) & "%')  or  upper(EmailDate) LIKE ('%" & Trim(strValue.Trim.ToUpper) & "%') "
                    End If
                End If
            Next
            If Trim(strWhereCond) = "" Then
                strWhereCond = " (" & strWhereCond1 & ")"
            Else
                strWhereCond = strWhereCond & " AND (" & strWhereCond1 & ")"
            End If

        End If



        If Trim(strWhereCond) <> "" Then
            strSqlQry = strSqlQry & " Where  TrackingStatus<>'Ignore' and ApprovalEnd is null and " & strWhereCond & "  order by AssignedDate desc"
        Else
            strSqlQry = strSqlQry & " Where  TrackingStatus<>'Ignore' and ApprovalEnd is null  order by convert(datetime,isnull(AssignedDate,getdate())) desc "
        End If



        Dim myDS As New DataSet
        Try
            '   strSqlQry = "select EmailLineNo,EmailId,EmailNo,HotelCode,rtrim(HotelName)HotelName,HotelStatus,TrackingStatus,EmailSubject,EmailDate,EmailTime,ProgressStage,UpdateStart,UpdateEnd,ApprovalStart,ApprovalEnd from VIEW_TRACKING order by EmailLineNo"
            Dim SqlConn As New SqlConnection
            Dim myDataAdapter As New SqlDataAdapter
            SqlConn = clsDBConnect.dbConnectionnew("strDBConnection")
            'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)
            gvTracking.DataSource = myDS
            gvTracking.DataBind()

        Catch ex As Exception
        End Try
    End Sub

    Function sbAddToDataTablePopup(ByVal lsName As String, ByVal lsValue As String, ByVal lsShortCode As String) As Boolean
        Dim iFlag As Integer = 0
        Dim dtt As DataTable
        dtt = Session("sDtDynamicPopup")
        If dtt.Rows.Count >= 0 Then
            For i = 0 To dtt.Rows.Count - 1
                If dtt.Rows(i)("Code").ToString.Trim.ToUpper = lsName.Trim.ToUpper And dtt.Rows(i)("Value").ToString.Trim.ToUpper = lsValue.Trim.ToUpper Then
                    iFlag = 1
                End If
            Next
            If iFlag = 0 Then
                dtt.NewRow()
                dtt.Rows.Add(lsName.Trim, lsValue.Trim)
                Session("sDtDynamicPopup") = dtt
            End If
        End If
        Return True
    End Function

    Protected Sub lbClosePopup_Click(sender As Object, e As System.EventArgs)
        Try

            Dim dtsHotelDetailsPopup As New DataTable
            dtsHotelDetailsPopup = Session("sDtHotelDetailsPopup")

            Dim myButton As LinkButton = CType(sender, LinkButton)
            Dim dlItem As DataListItem = CType((CType(sender, LinkButton)).NamingContainer, DataListItem)
            Dim lb As LinkButton = CType(dlItem.FindControl("lbHotelPopup"), LinkButton)

            If dtsHotelDetailsPopup.Rows.Count > 0 Then

                Dim i As Integer
                For i = dtsHotelDetailsPopup.Rows.Count - 1 To 0 Step i - 1
                    'If lb.Text.Trim = dtsHotelDetails.Rows(i)("Type").ToString.Trim Then
                    dtsHotelDetailsPopup.Rows.Remove(dtsHotelDetailsPopup.Rows(i))
                    'End If
                    dtsHotelDetailsPopup.AcceptChanges()
                Next
            End If
            Session("sDtHotelDetailsPopup") = dtsHotelDetailsPopup

            Dim dtDynamicsPopup As New DataTable
            dtDynamicsPopup = Session("sDtDynamicPopup")

            If dtDynamicsPopup.Rows.Count > 0 Then
                Dim j As Integer
                For j = dtDynamicsPopup.Rows.Count - 1 To 0 Step j - 1
                    If lb.Text.Trim = dtDynamicsPopup.Rows(j)("Value").ToString.Trim Then
                        dtDynamicsPopup.Rows.Remove(dtDynamicsPopup.Rows(j))
                    End If
                Next

            End If

            Session("sDtDynamicPopup") = dtDynamicsPopup
            dlTrackingVS.DataSource = dtDynamicsPopup
            dlTrackingVS.DataBind()



            '' Create a Dynamic datatable ---- Start
            Dim ClearDataTable = New DataTable()
            Dim dcGroupDetailsType = New DataColumn("Type", GetType(String))
            Dim dcGroupDetailsCode = New DataColumn("Code", GetType(String))
            Dim dcGroupDetailsCountry = New DataColumn("Value", GetType(String))
            ClearDataTable.Columns.Add(dcGroupDetailsType)
            ClearDataTable.Columns.Add(dcGroupDetailsCode)
            ClearDataTable.Columns.Add(dcGroupDetailsCountry)
            'gvHotels.DataSource = ClearDataTable
            'gvHotels.DataBind()

            FillTrackingForVS()
            hdTrackpopupStatus.Value = "N"
            meContractTracking.Hide()

        Catch ex As Exception

        End Try
    End Sub

    Protected Sub btnUpdatePopup_Click(sender As Object, e As System.EventArgs) Handles btnUpdatePopup.Click

        Try

            If txtUpdateType.Text.Trim = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please enter update type');", True)
                Exit Sub
            End If
            If txtValidFrom.Text.Trim = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please enter valid from');", True)
                Exit Sub
            End If
            If txtValidTo.Text.Trim = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please enter valid to');", True)
                Exit Sub
            End If
            If txtAssignedDate.Text.Trim = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please enter Assigned date');", True)
                Exit Sub
            End If
            If txtAssignedTo.Text.Trim = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please enter assigned to');", True)
                Exit Sub
            End If

            Dim iClarified As Integer = 0
            If chkClarified.Checked = True Then
                iClarified = 1
                If txtReAssignedDate.Text.Trim = "" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please enter Reassigned date');", True)
                    Exit Sub
                End If
                If txtReAssignedToCode.Text.Trim = "" Then
                    txtReAssignedToCode.Text = ""
                    txtReAssignedTo.Text = ""
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please enter Reassigned to');", True)
                    Exit Sub
                End If
            Else
                iClarified = 0
            End If

            Dim iApprovalClarified As Integer = 0
            If chkApprovalClarify.Checked = True Then
                iApprovalClarified = 1
                If txtApprovalReassignTime.Text.Trim = "" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please enter Reassign date for approval');", True)
                    Exit Sub
                End If
                If txtReApproverCode.Text.Trim = "" Then
                    txtReApproverCode.Text = ""
                    txtReApprover.Text = ""

                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please enter approver for reassignment');", True)
                    Exit Sub
                End If
            Else
                iApprovalClarified = 0
            End If

            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            sqlTrans = SqlConn.BeginTransaction           'SQL  Trans start
            myCommand = New SqlCommand("sp_Contract_Email_Popup", SqlConn, sqlTrans)

       
            '   Dim strAssdate() As String = txtAssignedDate.Text.Split(" ")
            '  Dim strAssignedDate As String = Convert.ToDateTime(strAssdate(0)).ToString("yyyy-MM-dd") & " " & strAssdate(1)
            Dim strAssignedDate As String = Convert.ToDateTime(txtAssignedDate.Text).ToString("yyyy-MM-dd")
            myCommand.CommandType = CommandType.StoredProcedure
            myCommand.Parameters.Add(New SqlParameter("@MailId", SqlDbType.VarChar, 10)).Value = hdPopupMailId.Value.Trim
            myCommand.Parameters.Add(New SqlParameter("@HotelId", SqlDbType.VarChar, 10)).Value = hdPopupHotelCode.Value.Trim
            myCommand.Parameters.Add(New SqlParameter("@UpdateTypeCode", SqlDbType.VarChar, 10)).Value = txtUpdateTypeCode.Text.Trim
            myCommand.Parameters.Add(New SqlParameter("@ValidFrom", SqlDbType.VarChar, 10)).Value = txtValidFrom.Text.Trim
            myCommand.Parameters.Add(New SqlParameter("@ValidTo", SqlDbType.VarChar, 10)).Value = txtValidTo.Text.Trim
            myCommand.Parameters.Add(New SqlParameter("@AssignedDate", SqlDbType.VarChar, 20)).Value = txtAssignedDate.Text
            myCommand.Parameters.Add(New SqlParameter("@AssignedTo", SqlDbType.VarChar, 15)).Value = txtAssignedToCode.Text.Trim
            myCommand.Parameters.Add(New SqlParameter("@ReassignedDate", SqlDbType.VarChar, 20)).Value = txtReAssignedDate.Text
            myCommand.Parameters.Add(New SqlParameter("@ReassignedTo", SqlDbType.VarChar, 50)).Value = txtReAssignedToCode.Text.Trim
            myCommand.Parameters.Add(New SqlParameter("@Clarified", SqlDbType.Int, 50)).Value = iClarified
            myCommand.Parameters.Add(New SqlParameter("@ReAssignedComment", SqlDbType.VarChar, 1000)).Value = txtComment.Text.Replace("'", "''")
            myCommand.Parameters.Add(New SqlParameter("@ApprovalAssignedDate", SqlDbType.VarChar, 20)).Value = txtApprovalAssDateTime.Text
            myCommand.Parameters.Add(New SqlParameter("@Approver", SqlDbType.VarChar, 15)).Value = txtApproverCode.Text.Trim
            myCommand.Parameters.Add(New SqlParameter("@ReApprovalAssignedDate", SqlDbType.VarChar, 20)).Value = txtApprovalReassignTime.Text
            myCommand.Parameters.Add(New SqlParameter("@ReApprover", SqlDbType.VarChar, 50)).Value = txtReApproverCode.Text.Trim
            myCommand.Parameters.Add(New SqlParameter("@ApprovalClarified", SqlDbType.Int, 50)).Value = iApprovalClarified
            myCommand.Parameters.Add(New SqlParameter("@ApprovalReAssignedComment", SqlDbType.VarChar, 1000)).Value = txtReapproveComments.Text.Replace("'", "''")
            myCommand.ExecuteNonQuery()

            '            @ApprovalAssignedDate varchar(20),
            '@Approver varchar(50),
            '@ReApprovalAssignedDate varchar(20),
            '@ReApprover varchar(50),
            '@ApprovalClarified int,
            '@ApprovalReAssignedComment varchar(1000)

            sqlTrans.Commit()    'SQl Tarn Commit
            clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
            clsDBConnect.dbCommandClose(myCommand)               'sql command disposed
            clsDBConnect.dbConnectionClose(SqlConn)           'connection close

            txtUpdateType.Text = ""
            txtValidFrom.Text = ""
            txtValidTo.Text = ""
            txtAssignedTo.Text = ""
            txtAssignedDate.Text = ""
            chkAssign.Checked = False
            txtAssignedToCode.Text = ""
            txtUpdateTypeCode.Text = ""
            txtReAssignedToCode.Text = ""
            txtReAssignedTo.Text = ""
            txtApprovalAssDateTime.Text = ""
            txtApprovalReassignTime.Text = ""
            txtApprover.Text = ""
            txtApproverCode.Text = ""
            txtReApproverCode.Text = ""
            txtReApprover.Text = ""
            hdPopupStatus.Value = "N"
            hdTrackpopupStatus.Value = "N"
            meContractTracking.Hide()
            FillTrackingForVS()

        Catch ex As Exception
            If SqlConn.State = ConnectionState.Open Then
                sqlTrans.Rollback()

            End If

            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)

            objUtils.WritErrorLog("ContractTrack.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub

    Private Sub FillAttachmentsPopup(strCode As String)
        Dim SqlConn As SqlConnection
        Dim myDataAdapter As SqlDataAdapter
        Dim myDS As New DataSet
        Dim strAttachment As String = ""
        Dim strSqlQry As String = "select EmailId,FileName,Content from Email_Inbox_Attachment where EmailId=" & strCode
        SqlConn = clsDBConnect.dbConnectionnew(HttpContext.Current.Session("dbconnectionName"))                             'Open connection
        myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
        myDataAdapter.Fill(myDS, "Details")
        DLAttachmentPopup.DataSource = myDS
        DLAttachmentPopup.DataBind()


    End Sub

    Private Sub BindTrackingPopupFields(strMailId As String, strHotelId As String)
        Dim strSqlQry As String = ""
        Dim myDt As New DataTable
        Dim strUserName As New List(Of String)
        Try
            txtUpdateType.Text = ""
            txtValidFrom.Text = ""
            txtValidTo.Text = ""
            txtAssignedTo.Text = ""
            txtAssignedDate.Text = ""
            chkAssign.Checked = False
            txtAssignedToCode.Text = ""
            txtUpdateTypeCode.Text = ""
            txtApprovalAssDateTime.Text = ""
            txtApprovalReassignTime.Text = ""
            txtApprover.Text = ""
            txtApproverCode.Text = ""
            txtReApproverCode.Text = ""
            txtReApprover.Text = ""
            strSqlQry = "select EmailId,HotelId,TrackingStatus,(select TrackStatusName from TrackingStatusMaster where TrackCode=TrackingStatus)TrackStatusName,UpdateTypeCode, (select typename from trackupdatetype where TypeCode=UpdateTypeCode)UpdateTypeName,convert(varchar(10),ValidFrom,103)ValidFrom,convert(varchar(10),ValidTo,103)ValidTo,AssignedTo,convert(varchar(10),case when ReassignedDate is not null then ReassignedDate else AssignedDate end,103) AssignedDate ,case when ReassignedTo is not null then ReassignedTo else AssignedTo end AssignedTo,(select USERNAME from usermaster where deptcode='CON' and active=1 and UserCode=(case when ReassignedTo is not null then ReassignedTo else AssignedTo end))AssignedToName,case when ApproverReassign is not null then ApproverReassign else Approver end  Approver, convert(varchar(10),case when ApprovalReassignmentDate is not null then ApprovalReassignmentDate else ApprovalAssignmentDate end,103) ApprovalAssignmentDate, (select USERNAME from usermaster where deptcode='CON' and active=1  and UserCode=( case when ApproverReassign is not null then ApproverReassign else Approver end)) ApproverName from Contract_Email where EmailId='" & strMailId & "' and HotelId='" & strHotelId & "'"
            ' strSqlQry = "select EmailId,HotelId,TrackingStatus,(select TrackStatusName from TrackingStatusMaster where TrackCode=TrackingStatus)TrackStatusName,UpdateTypeCode, (select typename from trackupdatetype where TypeCode=UpdateTypeCode)UpdateTypeName,convert(varchar(10),ValidFrom,103)ValidFrom,convert(varchar(10),ValidTo,103)ValidTo,AssignedTo,AssignedDate,ReassignedDate ,AssignedTo,(select USERNAME from usermaster where deptcode='CON' and active=1 and UserCode=AssignedTo)AssignedToName,(select USERNAME from usermaster where deptcode='CON' and active=1 and UserCode=ReassignedTo)ReassignedToName,Approver,(select USERNAME from usermaster where deptcode='CON' and active=1 and UserCode=Approver)ApproverName,ApproverReassign,(select USERNAME from usermaster where deptcode='CON' and active=1 and UserCode=ApproverReassign)ApproverReassignName, ApprovalAssignmentDate,ApprovalReassignmentDate from Contract_Email where EmailId='" & strMailId & "' and HotelId='" & strHotelId & "'"
            Dim SqlConn As New SqlConnection
            Dim myDataAdapter As New SqlDataAdapter
            SqlConn = clsDBConnect.dbConnectionnew("strDBConnection")
            'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDt)
            If myDt.Rows.Count > 0 Then
                For i As Integer = 0 To myDt.Rows.Count - 1
                    txtUpdateType.Text = myDt.Rows(i)("UpdateTypeName").ToString
                    txtValidFrom.Text = myDt.Rows(i)("ValidFrom").ToString.Trim
                    txtValidTo.Text = myDt.Rows(i)("ValidTo").ToString.Trim
                    txtAssignedTo.Text = myDt.Rows(i)("AssignedToName").ToString
                    txtAssignedDate.Text = myDt.Rows(i)("AssignedDate").ToString
                    txtAssignedToCode.Text = myDt.Rows(i)("AssignedTo").ToString
                    txtUpdateTypeCode.Text = myDt.Rows(i)("UpdateTypeCode").ToString
                    txtApprover.Text = myDt.Rows(i)("ApproverName").ToString
                    txtApprovalAssDateTime.Text = myDt.Rows(i)("ApprovalAssignmentDate").ToString
                    txtApproverCode.Text = myDt.Rows(i)("Approver").ToString


                Next
            End If

        Catch ex As Exception

        End Try
    End Sub

    Protected Sub gvTracking_PageIndexChanging(sender As Object, e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvTracking.PageIndexChanging
        gvTracking.PageIndex = e.NewPageIndex
        FillTrackingForVS()
    End Sub

    Protected Sub DLAttachmentPopup_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.DataListItemEventArgs) Handles DLAttachmentPopup.ItemDataBound
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then

            Dim lblfile As Label = CType(e.Item.FindControl("lblfile"), Label)
            Dim imgAttachmentType As Image = CType(e.Item.FindControl("imgAttachmentType"), Image)
            Dim filePath As String = "~\Attachment\" '+ lblfile.Text
            Dim path As String = Server.MapPath(filePath)
            '   Dim path As String = "C:\Program Files\Mahce\Attachment"
            '  Dim file As System.IO.FileInfo = New System.IO.FileInfo(path)
            ' Dim str As String = lblfile.Text.Replace(vbCrLf, "").Replace(vbCr, "").Replace("\n", "").Replace("\r", "")
            Dim strExt As String = System.IO.Path.GetExtension(lblfile.Text.Replace(vbCrLf, " ")).ToUpper
            '  Dim strExt As String = System.IO.Path.GetExtension(str).ToUpper
            Select Case strExt.Trim
                Case ".PDF"
                    imgAttachmentType.ImageUrl = "~/Images/crystaltoolbar/pdf.png"
                Case ".JPG", ".JPEG", ".GIF", ".PNG"
                    imgAttachmentType.ImageUrl = "~/Images/crystaltoolbar/jpg file.png"
                Case ".DOCX"
                    imgAttachmentType.ImageUrl = "~/Images/crystaltoolbar/Word_2007.png"
                Case ".DOC"
                    imgAttachmentType.ImageUrl = "~/Images/crystaltoolbar/MS_word2003.png"
                Case ".XLSX", ".XLS"
                    imgAttachmentType.ImageUrl = "~/Images/crystaltoolbar/excel.ico"
                Case Else
                    imgAttachmentType.ImageUrl = "~/Images/crystaltoolbar/mail-attachment2.png"
            End Select

        End If

    End Sub

    Protected Sub lbAttachmentPopup_Click(sender As Object, e As System.EventArgs)
        Try
            Dim myButton As LinkButton = CType(sender, LinkButton)
            Dim dlItem As DataListItem = CType((CType(sender, LinkButton)).NamingContainer, DataListItem)
            Dim lblfile As Label = CType(dlItem.FindControl("lblfile"), Label)

            Dim strpop As String
            strpop = "window.open('Download.aspx?filename=" & lblfile.Text.Trim & "');"
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
        Catch ex As Exception
            '   Dim str As String = ex.Message
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        End Try
    End Sub

    Protected Sub chkClarified_CheckedChanged(sender As Object, e As System.EventArgs) Handles chkClarified.CheckedChanged

        'Dim SqlConnNew As SqlConnection
        'Dim myDataAdapterNew As SqlDataAdapter
        'Dim myDT As New DataTable
        'Dim strSqlQryNew As String = "select ClarifyRemarks from Contract_Clarify where HotelId='" & hdPopupHotelCode.Value & "' and MailId='" & hdPopupMailId.Value & "' and Clarified=0"
        'SqlConnNew = clsDBConnect.dbConnectionnew(HttpContext.Current.Session("dbconnectionName"))                             'Open connection
        'myDataAdapterNew = New SqlDataAdapter(strSqlQryNew, SqlConnNew)
        'myDataAdapterNew.Fill(myDT)
        ' If myDT.Rows.Count = 1 Then

        If chkClarified.Checked Then

            tr2.Visible = True
            tr3.Visible = True
            tr4.Visible = True
            Dim str As String = Date.Now.ToString("dd/MM/yyyy") ' & " " & Date.Now.ToString("hh:mm")
            txtReAssignedDate.Text = str
            txtReAssignedTo.Text = txtAssignedTo.Text
            txtReAssignedToCode.Text = txtAssignedToCode.Text
            txtReAssignedTo.Focus()
        Else

            tr2.Visible = False
            tr3.Visible = False
            tr4.Visible = False
        End If
    End Sub

    Protected Sub chkApproverClarified_CheckedChanged(sender As Object, e As System.EventArgs) Handles chkApprovalClarify.CheckedChanged

        If chkApprovalClarify.Checked Then


            tr8.Visible = True
            tr9.Visible = True
            tr10.Visible = True
            Dim str As String = Date.Now.ToString("dd/MM/yyyy") ' & " " & Date.Now.ToString("hh:mm")
            txtApprovalReassignTime.Text = str
            txtReApprover.Text = txtApprover.Text
            txtReApproverCode.Text = txtApproverCode.Text
            txtReApprover.Focus()
        Else

            tr8.Visible = False
            tr9.Visible = False
            tr10.Visible = False
        End If
    End Sub

    Private Sub FillClarifyGrid()
        Dim strSqlQry As String = ""
        Dim myDS As New DataSet

        Try
            strSqlQry = "select (select U.UserName from UserMaster U where U.UserCode=L.UserName)UserName,ProgressStatus,convert(varchar(16),LogDate,103) + ' ' + convert(varchar(5),LogDate,108)LogDate,Comments  from ContractsClarifyLogs L  where L.HotelId='" & hdPopupHotelCode.Value & "' and L.MailId='" & hdPopupMailId.Value & "' order by LogDate desc"
            Dim SqlConn As New SqlConnection
            Dim myDataAdapter As New SqlDataAdapter
            SqlConn = clsDBConnect.dbConnectionnew("strDBConnection")
            'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)
            gvClarify.DataSource = myDS
            gvClarify.DataBind()

        Catch ex As Exception
        End Try
    End Sub
    Private Sub FillClarifyForApprovalGrid()
        Dim strSqlQry As String = ""
        Dim myDS As New DataSet

        Try
            strSqlQry = "select (select U.UserName from UserMaster U where U.UserCode=L.UserName)UserName,ProgressStatus,convert(varchar(16),LogDate,103) + ' ' + convert(varchar(5),LogDate,108)LogDate,Comments  from ContractsClarifyForApprovalLogs L  where L.HotelId='" & hdPopupHotelCode.Value & "' and L.MailId='" & hdPopupMailId.Value & "' order by LogDate desc"
            Dim SqlConn As New SqlConnection
            Dim myDataAdapter As New SqlDataAdapter
            SqlConn = clsDBConnect.dbConnectionnew("strDBConnection")
            'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)
            gvClarifyForApproval.DataSource = myDS
            gvClarifyForApproval.DataBind()

        Catch ex As Exception
        End Try
    End Sub



    Protected Sub gvTrackingView_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvTrackingView.RowDataBound
        Try

            'gvTracking.Columns("yourColumnName").Frozen = True
            If e.Row.RowType = DataControlRowType.Header Then
                e.Row.Cells(0).CssClass = "lockedHeader"
                e.Row.Cells(2).CssClass = "lockedHeader"
                e.Row.Cells(4).CssClass = "lockedHeader"
                e.Row.Cells(6).CssClass = "lockedHeader"
                e.Row.Cells(8).CssClass = "lockedHeaderLast"
                ' e.Row.Cells(8).CssClass = "lockedHeaderNext"


            Else
                If iFlag = 0 Then
                    e.Row.Cells(0).CssClass = "locked"
                    e.Row.Cells(2).CssClass = "locked"
                    e.Row.Cells(4).CssClass = "locked"
                    e.Row.Cells(6).CssClass = "locked"
                    e.Row.Cells(8).CssClass = "lockedLast"
                    iFlag = 1
                Else
                    e.Row.Cells(0).CssClass = "lockedAlternative"
                    e.Row.Cells(2).CssClass = "lockedAlternative"
                    e.Row.Cells(4).CssClass = "lockedAlternative"
                    e.Row.Cells(6).CssClass = "lockedAlternative"
                    e.Row.Cells(8).CssClass = "lockedAlternativeLast"
                    'e.Row.Cells(8).CssClass = "lockedAlternativeNext"
                    iFlag = 0
                End If

            End If

            If e.Row.RowType = DataControlRowType.DataRow Then
              

            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        End Try
    End Sub


    Protected Sub lbUpdateTrackview_Click(sender As Object, e As System.EventArgs)

        Try


            Dim lbUpdate As LinkButton = CType(sender, LinkButton)
            Dim gvRow As GridViewRow = CType(lbUpdate.NamingContainer, GridViewRow)
            Dim hdEmailId_ As HiddenField = CType(gvRow.FindControl("hdEmailId_"), HiddenField)
            Dim hdCHotelCode As HiddenField = CType(gvRow.FindControl("hdCHotelCode"), HiddenField)
            Dim lblAssignedToCode As Label = CType(gvRow.FindControl("lblAssignedToCode"), Label)
            Dim lblApprover As Label = CType(gvRow.FindControl("lblApprover"), Label)
            hdPopupHotelCode.Value = hdCHotelCode.Value
            hdPopupMailId.Value = hdEmailId_.Value


            BindTrackingPopupFields(hdPopupMailId.Value, hdPopupHotelCode.Value)

            Dim lblEmailCode As Label = CType(gvRow.FindControl("lblEmailCode"), Label)
            Dim lblCHotelName As Label = CType(gvRow.FindControl("lblCHotelName"), Label)
            Dim lblTrackingStatus As Label = CType(gvRow.FindControl("lblTrackingStatus"), Label)
            If lblTrackingStatus.Text.Trim = "In Progress" Then
                btnUpdatePopup.Enabled = False
            Else
                btnUpdatePopup.Enabled = True

            End If

            btnUpdatePopup.Visible = False
            btnCancelPopup.Visible = False
            txtUpdateType.ReadOnly = True
            txtValidFrom.ReadOnly = True
            txtValidTo.ReadOnly = True
            txtAssignedDate.ReadOnly = True
            txtAssignedTo.ReadOnly = True
            chkAssign.Enabled = False
            ImgValidFrom.Enabled = False
            ImgtxtValidTo.Enabled = False

            '  txtFromDate.Attributes.Add("onchange", "setdate();")
            txtValidTo.Attributes.Add("onchange", "checkdates('" & txtValidFrom.ClientID & "','" & txtValidTo.ClientID & "');")
            txtValidFrom.Attributes.Add("onchange", "checkfromdates('" & txtValidFrom.ClientID & "','" & txtValidTo.ClientID & "');")
            Dim str As String = Date.Now.ToString("dd/MM/yyyy") & " " & Date.Now.ToString("hh:mm")
            If txtAssignedDate.Text <> "" Then
                str = txtAssignedDate.Text
            Else
                txtAssignedDate.Text = str
            End If

            chkAssign.Attributes.Add("onchange", "setCurrentDate('" & txtAssignedDate.ClientID & "','" & chkAssign.ClientID & "','" & str & "');")


            Dim SqlConnNew As SqlConnection
            Dim myDataAdapterNew As SqlDataAdapter
            Dim myDT As New DataTable
            Dim strSqlQryNew As String = "select ClarifyRemarks from Contract_Clarify where HotelId='" & hdPopupHotelCode.Value & "' and MailId='" & hdPopupMailId.Value & "' and Clarified=0"
            SqlConnNew = clsDBConnect.dbConnectionnew(HttpContext.Current.Session("dbconnectionName"))                             'Open connection
            myDataAdapterNew = New SqlDataAdapter(strSqlQryNew, SqlConnNew)
            myDataAdapterNew.Fill(myDT)


            If myDT.Rows.Count = 1 Then
                chkClarified.Visible = True
                tr1.Visible = True
                tr2.Visible = False
                tr3.Visible = False
                tr4.Visible = False

                trApprovalAssdate.Visible = False
                trApproverAss.Visible = False
                tr7.Visible = False
                tr8.Visible = False
                tr9.Visible = False
                tr10.Visible = False
            Else
                chkClarified.Visible = False
                tr1.Visible = False
                tr2.Visible = False
                tr3.Visible = False
                tr4.Visible = False

                trApprovalAssdate.Visible = False
                trApproverAss.Visible = False
                tr7.Visible = False
                tr8.Visible = False
                tr9.Visible = False
                tr10.Visible = False
            End If


            Dim strSqlQryAppr As String = "select count(EmailId)cnt from Contract_Email where TaskCompleteDate is not null and HotelId='" & hdPopupHotelCode.Value & "' and EMailId='" & hdPopupMailId.Value & "'"
            Dim iCntAppr As Integer = objUtils.ExecuteQueryReturnStringValuenew(HttpContext.Current.Session("dbconnectionName"), strSqlQryAppr)
            If iCntAppr > 0 Then
                trApprovalAssdate.Visible = True
                trApproverAss.Visible = True
                tr7.Visible = False
                tr8.Visible = False
                tr9.Visible = False
                tr10.Visible = False
                Dim strSqlQryClar As String = "select count(ClarifyId)cnt from Contract_ClarifyForApproval where HotelId='" & hdPopupHotelCode.Value & "' and MailId='" & hdPopupMailId.Value & "' and AssignedUser='" & lblApprover.Text & "' and Clarified=0"
                ' Dim strSqlQryClar As String = "select (ClarifyId)cnt from Contract_Clarify"
                Dim iCntApprClar As Integer = objUtils.ExecuteQueryReturnStringValuenew(HttpContext.Current.Session("dbconnectionName"), strSqlQryClar)
                If iCntApprClar > 0 Then
                    tr7.Visible = True
                    tr8.Visible = True
                    tr9.Visible = True
                    tr10.Visible = True
                End If
            End If

            If myDT.Rows.Count = 1 Then
                chkClarified.Visible = True
                tr1.Visible = True
                tr2.Visible = False
                tr3.Visible = False
                tr4.Visible = False
            Else
                chkClarified.Visible = False
                tr1.Visible = False
                tr2.Visible = False
                tr3.Visible = False
                tr4.Visible = False
            End If

            '   Dim strSqlQryAppr As String = "select count(EmailId)cnt from Contract_Email where TaskCompleteDate is not null and HotelId='" & hdPopupHotelCode.Value & "' and EMailId='" & hdPopupMailId.Value & "'"
            '   Dim iCntAppr As Integer = objUtils.ExecuteQueryReturnStringValuenew(HttpContext.Current.Session("dbconnectionName"), strSqlQryAppr)

            'If iCntAppr > 0 Then
            '       trApprovalAssdate.Visible = True
            '       trApproverAss.Visible = True
            '       tr7.Visible = False
            '       tr8.Visible = False
            '       tr9.Visible = False
            '       tr10.Visible = False
            '       Dim strSqlQryClar As String = "select count(ClarifyId)cnt from Contract_ClarifyForApproval where HotelId='" & hdPopupHotelCode.Value & "' and MailId='" & hdPopupMailId.Value & "' and AssignedUser='" & lblAssignedToCode.Text & "' and Clarified=0"
            '       ' Dim strSqlQryClar As String = "select (ClarifyId)cnt from Contract_Clarify"
            '       Dim iCntApprClar As Integer = objUtils.ExecuteQueryReturnStringValuenew(HttpContext.Current.Session("dbconnectionName"), strSqlQryClar)
            '       If iCntApprClar > 0 Then
            '           tr7.Visible = True
            '           tr8.Visible = True
            '           tr9.Visible = True
            '           tr10.Visible = True
            '       End If
            '   End If
            Dim strCode As String = hdEmailId_.Value
            Dim SqlConn As SqlConnection
            Dim myDataAdapter As SqlDataAdapter
            Dim myDS As New DataSet
            Dim strAttachment As String = ""
            lblEmailCodePopup.Text = lblEmailCode.Text
            lblHotelNamePopup.Text = "Hotel Name: " & lblCHotelName.Text
            Dim strSqlQry As String = "select EmailId,EmailFrom,'Sub: '+EmailSubject EmailSubject,CONVERT(VARCHAR(11), EmailDate, 113) + ' ' +right(convert(varchar(32),EmailDate,100),8)EmailDate,EmailFullSource,EmailAttachment,RIGHT('000000'+CAST(EmailId AS VARCHAR(6)),6)EmailNo from Email_Inbox where EmailId=" & strCode
            SqlConn = clsDBConnect.dbConnectionnew(HttpContext.Current.Session("dbconnectionName"))                             'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS, "Details")
            If myDS.Tables.Count > 0 Then
                If myDS.Tables(0).Rows.Count > 0 Then
                    lblFromPopup.Text = Server.HtmlEncode(myDS.Tables(0).Rows(0)("EmailFrom").ToString)
                    lblDatePopup.Text = myDS.Tables(0).Rows(0)("EmailDate").ToString
                    lblSubjectPopup.Text = myDS.Tables(0).Rows(0)("EmailSubject").ToString
                    lblBodyPopup.Text = (myDS.Tables(0).Rows(0)("EmailFullSource").ToString.Replace("img", "img style='Display:none;'")).Replace(":blue", ":#06788B")
                    strAttachment = myDS.Tables(0).Rows(0)("EmailAttachment").ToString

                    'EmailNo
                    If strAttachment.Trim = "Y" Then
                        FillAttachmentsPopup(strCode)
                    Else
                        DLAttachmentPopup.DataBind()
                    End If

                    'strAttachment = myDS.Tables(0).Rows(0)("EmailAttachment").ToString
                    ''EmailNo
                    'If strAttachment.Trim = "Y" Then
                    '    FillAttachments(strCode)
                    'Else
                    '    '    DLAttachments.DataSource = myDS
                    '    DLAttachments.DataBind()
                    'End If

                End If
            End If

            FillClarifyGrid()
            FillClarifyForApprovalGrid()
            FillAdditionalEmails(strCode, hdCHotelCode.Value)


            hdTrackpopupStatus.Value = "Y"
            txtUpdateType.Focus()
            meContractTracking.Show()

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ContractTracking.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub


    Function sbAddToDataTableTrackview(ByVal lsName As String, ByVal lsValue As String, ByVal lsShortCode As String) As Boolean
        Dim iFlag As Integer = 0
        Dim dtt As DataTable
        dtt = Session("sDtDynamicTrackview")
        If dtt.Rows.Count >= 0 Then
            For i = 0 To dtt.Rows.Count - 1
                If dtt.Rows(i)("Code").ToString.Trim.ToUpper = lsName.Trim.ToUpper And dtt.Rows(i)("Value").ToString.Trim.ToUpper = lsValue.Trim.ToUpper Then
                    iFlag = 1
                End If
            Next
            If iFlag = 0 Then
                dtt.NewRow()
                dtt.Rows.Add(lsName.Trim, lsValue.Trim)
                Session("sDtDynamicTrackview") = dtt
            End If
        End If
        Return True
    End Function

    Private Sub FillTrackViewForVS()


        Dim dtt As DataTable
        dtt = Session("sDtDynamicTrackview")

        Dim strHotelValue As String = ""
        Dim strEmailcodeValue As String = ""
        Dim strHotelStatusValue As String = ""
        Dim strTrackingStatusValue As String = ""
        Dim strFromEmailValue As String = ""
        Dim strEmailDateValue As String = ""
        Dim strEmailSubjectValue As String = ""
        Dim strUpdateType As String = ""
        Dim strAssignedTo As String = ""
        Dim strProgressStage As String = ""
        Dim strTextValue As String = ""
        Dim strBindCondition As String = ""


        If dtt.Rows.Count > 0 Then
            For i As Integer = 0 To dtt.Rows.Count - 1
                If dtt.Rows(i)("Code").ToString = "HOTELS" Then
                    If strHotelValue <> "" Then
                        strHotelValue = strHotelValue + ",'" + dtt.Rows(i)("Value").ToString + "'"
                    Else
                        strHotelValue = "'" + dtt.Rows(i)("Value").ToString + "'"
                    End If
                End If

                If dtt.Rows(i)("Code").ToString = "EMAIL CODE" Then
                    If strEmailcodeValue <> "" Then
                        strEmailcodeValue = strEmailcodeValue + ",'" + dtt.Rows(i)("Value").ToString + "'"
                    Else
                        strEmailcodeValue = "'" + dtt.Rows(i)("Value").ToString + "'"
                    End If
                End If
                If dtt.Rows(i)("Code").ToString = "HOTEL STATUS" Then
                    If strHotelStatusValue <> "" Then
                        strHotelStatusValue = strHotelStatusValue + ",'" + dtt.Rows(i)("Value").ToString + "'"
                    Else
                        strHotelStatusValue = "'" + dtt.Rows(i)("Value").ToString + "'"
                    End If
                End If
                If dtt.Rows(i)("Code").ToString = "TRACKING STATUS" Then
                    If strTrackingStatusValue <> "" Then
                        strTrackingStatusValue = strTrackingStatusValue + ",'" + dtt.Rows(i)("Value").ToString + "'"
                    Else
                        strTrackingStatusValue = "'" + dtt.Rows(i)("Value").ToString + "'"
                    End If
                End If
                If dtt.Rows(i)("Code").ToString = "FROM EMAIL" Then
                    If strFromEmailValue <> "" Then
                        strFromEmailValue = strFromEmailValue + ",'" + dtt.Rows(i)("Value").ToString + "'"
                    Else
                        strFromEmailValue = "'" + dtt.Rows(i)("Value").ToString + "'"
                    End If
                End If
                If dtt.Rows(i)("Code").ToString = "EMAIL DATE" Then
                    If strEmailDateValue <> "" Then
                        strEmailDateValue = strEmailDateValue + ",'" + dtt.Rows(i)("Value").ToString + "'"
                    Else
                        strEmailDateValue = "'" + dtt.Rows(i)("Value").ToString + "'"
                    End If
                End If
                If dtt.Rows(i)("Code").ToString = "EMAIL SUBJECT" Then
                    If strEmailSubjectValue <> "" Then
                        strEmailSubjectValue = strEmailSubjectValue + ",'" + dtt.Rows(i)("Value").ToString + "'"
                    Else
                        strEmailSubjectValue = "'" + dtt.Rows(i)("Value").ToString + "'"
                    End If
                End If

                If dtt.Rows(i)("Code").ToString = "TEXT" Then
                    If strTextValue <> "" Then
                        strTextValue = strTextValue + "," + dtt.Rows(i)("Value").ToString
                    Else
                        strTextValue = dtt.Rows(i)("Value").ToString
                    End If
                End If
     
                If dtt.Rows(i)("Code").ToString = "UPDATE TYPE" Then
                    If strUpdateType <> "" Then
                        strUpdateType = strUpdateType + ", '" + dtt.Rows(i)("Value").ToString + "'"
                    Else
                        strUpdateType = "'" + dtt.Rows(i)("Value").ToString + "'"
                    End If
                End If
                If dtt.Rows(i)("Code").ToString = "ASSIGNED TO" Then
                    If strAssignedTo <> "" Then
                        strAssignedTo = strAssignedTo + "," + dtt.Rows(i)("Value").ToString
                    Else
                        strAssignedTo = dtt.Rows(i)("Value").ToString
                    End If
                End If
                If dtt.Rows(i)("Code").ToString = "PROGRESS STAGE" Then
                    If strProgressStage <> "" Then
                        strProgressStage = strProgressStage + "," + dtt.Rows(i)("Value").ToString
                    Else
                        strProgressStage = dtt.Rows(i)("Value").ToString
                    End If
                End If
            Next
        End If


        Dim strWhereCond As String = ""
        lblMsg.Visible = False


        Dim strSqlQry As String = ""

        'select EmailLineNo,EmailId,EmailNo,HotelCode,rtrim(HotelName)HotelName,HotelStatus,TrackingStatus,EmailSubject,EmailDate,EmailTime,ProgressStage,UpdateStart,UpdateEnd,ApprovalStart,ApprovalEnd,UpdateTypeCode,UpdateTypeName,convert(varchar(10),ValidFrom,103)ValidFrom,convert(varchar(10),ValidTo,103)ValidTo,convert(varchar(16),AssignedDate,103)+ ' ' + convert(varchar(5),AssignedDate,108) AssignedDate,AssignedTo,AssignedToName
        strSqlQry = "select EmailLineNo,EmailId,EmailNo,HotelCode,rtrim(HotelName)HotelName,HotelStatus,TrackingStatus,EmailSubject,EmailDate,EmailTime,ProgressStage,UpdateStart,UpdateEnd,ApprovalStart,ApprovalEnd,UpdateTypeCode,UpdateTypeName,convert(varchar(10),ValidFrom,103)ValidFrom,convert(varchar(10),ValidTo,103)ValidTo,convert(varchar(16),AssignedDate,103) AssignedDate,AssignedTo,AssignedToName,Approver from VIEW_TRACKING where ApprovalEnd is not null "

        If strHotelValue <> "" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = " upper(HotelName) = " & Trim(strHotelValue.Trim.ToUpper) & " "
            Else
                strWhereCond = strWhereCond & " AND upper(HotelName) = " & Trim(strHotelValue.Trim.ToUpper) & " "
            End If
        End If


        If strEmailcodeValue <> "" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = " RIGHT('000000'+CAST(EmailId AS VARCHAR(6)),6)  IN ( " & Trim(strEmailcodeValue) & ")"
            Else

                strWhereCond = strWhereCond & " AND RIGHT('000000'+CAST(EmailId AS VARCHAR(6)),6)  IN ( " & Trim(strEmailcodeValue) & ")"
            End If
        End If

        If strHotelStatusValue <> "" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = " upper(HotelStatus) =" & Trim(strHotelStatusValue) & ""
            Else
                strWhereCond = strWhereCond & " AND HotelStatus =" & Trim(strHotelStatusValue) & ""
            End If
        End If

        If strTrackingStatusValue <> "" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = " upper(TrackingStatus)=" & Trim(strTrackingStatusValue) & ""
            Else

                strWhereCond = strWhereCond & " AND   TrackingStatus=" & Trim(strTrackingStatusValue) & ""
            End If
        End If

        If strFromEmailValue <> "" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = " upper(EmailFrom) IN ( " & Trim(strFromEmailValue) & ")"
            Else

                strWhereCond = strWhereCond & " AND upper(EmailFrom) IN (" & Trim(strFromEmailValue) & ")"
            End If
        End If

        If strEmailDateValue <> "" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = " upper(EmailDate) IN ( " & Trim(strEmailDateValue) & ")"
            Else

                strWhereCond = strWhereCond & " AND upper(EmailDate) IN (" & Trim(strEmailDateValue) & ")"
            End If
        End If
        If strEmailSubjectValue <> "" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = " upper(EmailSubject) IN ( " & Trim(strEmailSubjectValue) & ")"
            Else

                strWhereCond = strWhereCond & " AND upper(EmailSubject) IN (" & Trim(strEmailSubjectValue) & ")"
            End If
        End If
        If strUpdateType <> "" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = " upper(UpdateTypeName) IN ( " & Trim(strUpdateType) & ")"
            Else

                strWhereCond = strWhereCond & " AND upper(UpdateTypeName) IN (" & Trim(strUpdateType) & ")"
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
                        strWhereCond1 = "  upper(HotelName) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%'  or  RIGHT('000000'+CAST(EmailId AS VARCHAR(6)),6)  LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' or   HotelStatus like '%" & Trim(strValue.Trim.ToUpper) & "%' or TrackingStatus LIKE ('%" & Trim(strValue.Trim.ToUpper) & "%') or  upper(EmailFrom) LIKE ('%" & Trim(strValue.Trim.ToUpper) & "%') or  upper(EmailSubject) LIKE ('%" & Trim(strValue.Trim.ToUpper) & "%')  or  upper(EmailDate) LIKE ('%" & Trim(strValue.Trim.ToUpper) & "%') "
                    Else
                        strWhereCond1 = strWhereCond1 & " OR  upper(HotelName) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%'  or  RIGHT('000000'+CAST(EmailId AS VARCHAR(6)),6)  LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' or   HotelStatus like '%" & Trim(strValue.Trim.ToUpper) & "%' or TrackingStatus LIKE ('%" & Trim(strValue.Trim.ToUpper) & "%') or  upper(EmailFrom) LIKE ('%" & Trim(strValue.Trim.ToUpper) & "%') or  upper(EmailSubject) LIKE ('%" & Trim(strValue.Trim.ToUpper) & "%')  or  upper(EmailDate) LIKE ('%" & Trim(strValue.Trim.ToUpper) & "%') "
                    End If
                End If
            Next
            If Trim(strWhereCond) = "" Then
                strWhereCond = " (" & strWhereCond1 & ")"
            Else
                strWhereCond = strWhereCond & " AND (" & strWhereCond1 & ")"
            End If

        End If



        If Trim(strWhereCond) <> "" Then
            strSqlQry = strSqlQry & " and " & strWhereCond
        Else
            strSqlQry = strSqlQry
        End If



        Dim myDS As New DataSet
        Try
            '   strSqlQry = "select EmailLineNo,EmailId,EmailNo,HotelCode,rtrim(HotelName)HotelName,HotelStatus,TrackingStatus,EmailSubject,EmailDate,EmailTime,ProgressStage,UpdateStart,UpdateEnd,ApprovalStart,ApprovalEnd from VIEW_TRACKING order by EmailLineNo"
            Dim SqlConn As New SqlConnection
            Dim myDataAdapter As New SqlDataAdapter
            SqlConn = clsDBConnect.dbConnectionnew("strDBConnection")
            'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)
            gvTrackingView.DataSource = myDS
            gvTrackingView.DataBind()

        Catch ex As Exception
        End Try
    End Sub

    Protected Sub btnvsprocess_TrackView_Click(sender As Object, e As System.EventArgs) Handles btnvsprocess_TrackView.Click
        Dim lsSearchTxt As String = ""
        lsSearchTxt = txtvsprocesssplit_TrackView.Text.Replace("___", "<").Replace("...", ">")
        Dim lsProcessText As String = ""
        Dim lsMainArr As String()
        Dim IsProcessType As String = ""
        Dim IsProcessValue As String = ""
        lsMainArr = objUtils.splitWithWords(lsSearchTxt, "|~,")

        For i = 0 To lsMainArr.GetUpperBound(0)
            Select Case lsMainArr(i).Split(":")(0).ToString.ToUpper.Trim
                Case "HOTELS"
                    lsProcessText = lsMainArr(i).Split(":")(1)
                    sbAddToDataTableTrackview("HOTELS", lsProcessText, "H")
                    IsProcessType = "H"
                Case "EMAIL CODE"
                    lsProcessText = lsMainArr(i).Split(":")(1)
                    sbAddToDataTableTrackview("EMAIL CODE", lsProcessText, "EC")
                    IsProcessType = "TN"
                Case "HOTEL STATUS"
                    lsProcessText = lsMainArr(i).Split(":")(1)
                    sbAddToDataTableTrackview("HOTEL STATUS", lsProcessText, "HS")
                    IsProcessType = "HS"
                Case "TRACKING STATUS"
                    lsProcessText = lsMainArr(i).Split(":")(1)
                    sbAddToDataTableTrackview("TRACKING STATUS", lsProcessText, "TS")
                    IsProcessType = "TS"
                Case "FROM EMAIL"
                    lsProcessText = lsMainArr(i).Split(":")(1)
                    sbAddToDataTableTrackview("FROM EMAIL", lsProcessText, "FM")
                    IsProcessType = "FM"
                Case "TEXT"
                    lsProcessText = lsMainArr(i).Split(":")(1)
                    sbAddToDataTableTrackview("TEXT", lsProcessText, "T")
                    IsProcessType = "T"
                Case "EMAIL DATE"
                    lsProcessText = lsMainArr(i).Split(":")(1)
                    sbAddToDataTableTrackview("EMAIL DATE", lsProcessText, "ED")
                    IsProcessType = "ED"
                Case "EMAIL SUBJECT"
                    lsProcessText = lsMainArr(i).Split(":")(1)
                    sbAddToDataTableTrackview("EMAIL SUBJECT", lsProcessText, "ES")
                    IsProcessType = "ES"
                Case "UPDATE TYPE"
                    lsProcessText = lsMainArr(i).Split(":")(1)
                    sbAddToDataTableTrackview("UPDATE TYPE", lsProcessText, "UT")
                    IsProcessType = "UT"
                Case "ASSIGNED TO"
                    lsProcessText = lsMainArr(i).Split(":")(1)
                    sbAddToDataTableTrackview("ASSIGNED TO", lsProcessText, "AT")
                    IsProcessType = "AT"
                Case "PROGRESS STAGE"
                    lsProcessText = lsMainArr(i).Split(":")(1)
                    sbAddToDataTableTrackview("PROGRESS STAGE", lsProcessText, "PS")
                    IsProcessType = "PS"
            End Select
        Next

        Dim dttDyn As DataTable
        dttDyn = Session("sDtDynamicTrackview")
        dlTrackViewSerach.DataSource = dttDyn
        dlTrackViewSerach.DataBind()
        FillTrackViewForVS()
    End Sub

    Protected Sub lbCloseTrackview_Click(sender As Object, e As System.EventArgs)
        Try

            Dim dtsHotelDetailsTrackview As New DataTable
            dtsHotelDetailsTrackview = Session("sDtHotelDetailsTrackview")

            Dim myButton As LinkButton = CType(sender, LinkButton)
            Dim dlItem As DataListItem = CType((CType(sender, LinkButton)).NamingContainer, DataListItem)
            Dim lb As LinkButton = CType(dlItem.FindControl("lbHotelTrackview"), LinkButton)

            If dtsHotelDetailsTrackview.Rows.Count > 0 Then

                Dim i As Integer
                For i = dtsHotelDetailsTrackview.Rows.Count - 1 To 0 Step i - 1
                    'If lb.Text.Trim = dtsHotelDetails.Rows(i)("Type").ToString.Trim Then
                    dtsHotelDetailsTrackview.Rows.Remove(dtsHotelDetailsTrackview.Rows(i))
                    'End If
                    dtsHotelDetailsTrackview.AcceptChanges()
                Next
            End If
            Session("sDtHotelDetailsTrackview") = dtsHotelDetailsTrackview

            Dim dtDynamicsTrackview As New DataTable
            dtDynamicsTrackview = Session("sDtDynamicTrackview")

            If dtDynamicsTrackview.Rows.Count > 0 Then
                Dim j As Integer
                For j = dtDynamicsTrackview.Rows.Count - 1 To 0 Step j - 1
                    If lb.Text.Trim = dtDynamicsTrackview.Rows(j)("Value").ToString.Trim Then
                        dtDynamicsTrackview.Rows.Remove(dtDynamicsTrackview.Rows(j))
                    End If
                Next

            End If

            Session("sDtDynamicTrackview") = dtDynamicsTrackview
            dlTrackViewSerach.DataSource = dtDynamicsTrackview
            dlTrackViewSerach.DataBind()



            '' Create a Dynamic datatable ---- Start
            Dim ClearDataTable = New DataTable()
            Dim dcGroupDetailsType = New DataColumn("Type", GetType(String))
            Dim dcGroupDetailsCode = New DataColumn("Code", GetType(String))
            Dim dcGroupDetailsCountry = New DataColumn("Value", GetType(String))
            ClearDataTable.Columns.Add(dcGroupDetailsType)
            ClearDataTable.Columns.Add(dcGroupDetailsCode)
            ClearDataTable.Columns.Add(dcGroupDetailsCountry)
            'gvHotels.DataSource = ClearDataTable
            'gvHotels.DataBind()

            FillTrackViewForVS()
            hdTrackpopupStatus.Value = "N"
            meContractTracking.Hide()

        Catch ex As Exception

        End Try
    End Sub

    Protected Sub InboxIgnore_Click(sender As Object, e As System.EventArgs)
        Dim strlbValue As String = ""

        Dim myButton As LinkButton = CType(sender, LinkButton)

        Dim dlItem As DataListItem = CType((CType(sender, LinkButton)).NamingContainer, DataListItem)
        Dim lb As Label = CType(dlItem.FindControl("lblType"), Label)

        If Not myButton Is Nothing Then
            strlbValue = myButton.Text
            If strlbValue = "Hotels" Then
                strlbValue = "%%"
                hdLinkButtonValue.Value = strlbValue
            Else
                'strlbValue = "%"
                hdLinkButtonValue.Value = myButton.Text & "%"
            End If


            Try
                'FillGridByLinkButtonForIgnore()
                ' FillCheckbox()
                ' ShowPopup()
            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objUtils.WritErrorLog("ContractTrack.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            Finally


            End Try

        End If
    End Sub

    Private Sub FillInboxIgnoreByVS(pageIndex As Integer)
        Dim dtt As DataTable
        dtt = Session("sDtDynamicInboxIgnore")



        Dim strHotelValue As String = ""
        Dim strTicketValue As String = ""
        Dim strHotelStatusValue As String = ""
        Dim strTrackingStatusValue As String = ""
        Dim strFromEmailValue As String = ""
        Dim strTextValue As String = ""
        Dim strBindCondition As String = ""


        If dtt.Rows.Count > 0 Then
            For i As Integer = 0 To dtt.Rows.Count - 1
                If dtt.Rows(i)("Code").ToString = "HOTELS" Then
                    If strHotelValue <> "" Then
                        strHotelValue = strHotelValue + ",'" + dtt.Rows(i)("Value").ToString + "'"
                    Else
                        strHotelValue = "'" + dtt.Rows(i)("Value").ToString + "'"
                    End If
                End If

                If dtt.Rows(i)("Code").ToString = "TICKECT NO" Then
                    If strTicketValue <> "" Then
                        strTicketValue = strTicketValue + ",'" + dtt.Rows(i)("Value").ToString + "'"
                    Else
                        strTicketValue = "'" + dtt.Rows(i)("Value").ToString + "'"
                    End If
                End If
                If dtt.Rows(i)("Code").ToString = "HOTEL_STATUS" Then
                    If strHotelStatusValue <> "" Then
                        strHotelStatusValue = strHotelStatusValue + ",'" + dtt.Rows(i)("Value").ToString + "'"
                    Else
                        strHotelStatusValue = "'" + dtt.Rows(i)("Value").ToString + "'"
                    End If
                End If
                If dtt.Rows(i)("Code").ToString = "TRACKING_STATUS" Then
                    If strTrackingStatusValue <> "" Then
                        strTrackingStatusValue = strTrackingStatusValue + ",'" + dtt.Rows(i)("Value").ToString + "'"
                    Else
                        strTrackingStatusValue = "'" + dtt.Rows(i)("Value").ToString + "'"
                    End If
                End If
                If dtt.Rows(i)("Code").ToString = "FROM_EMAIL" Then
                    If strFromEmailValue <> "" Then
                        strFromEmailValue = strFromEmailValue + ",'" + dtt.Rows(i)("Value").ToString + "'"
                    Else
                        strFromEmailValue = "'" + dtt.Rows(i)("Value").ToString + "'"
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


        Dim strWhereCond As String = ""
        lblMsg.Visible = False


        strSqlQry = ""
        Try

            strSqlQry = "SELECT ROW_NUMBER() OVER (ORDER BY [EmailId] desc)AS RowNumber ,EmailId,EmailFrom,'Sub: '+EmailSubject EmailSubject,case when CONVERT(VARCHAR(11), EmailDate, 111)=CONVERT(VARCHAR(11), getdate(), 111) then right(convert(varchar(32),EmailDate,100),8)else CONVERT(VARCHAR(11), EmailDate, 113) end EmailDate,RIGHT('000000'+CAST(EmailId AS VARCHAR(6)),6) EmailNo ,case when (select count(C.EmailId) from Contract_Email C where C.EmailId=Email_Inbox.EmailId) >0 then 'True' else 'False' end HotelStatus from Email_Inbox "
            strWhereCond = "   EmailId in (select EmailId from Contract_Email where TrackingStatus='Ignore') "
            If strHotelValue <> "" Then
                If Trim(strWhereCond) = "" Then

                    strWhereCond = " EmailId in (select C.EmailId from Contract_Email C where C.HotelId in (select P.partycode from partymast P where upper(P.partyname)= " & Trim(strHotelValue.Trim.ToUpper) & ")) "

                Else
                    strWhereCond = strWhereCond & " AND EmailId in (select C.EmailId from Contract_Email C where C.HotelId in (select P.partycode from partymast P where upper(P.partyname)= " & Trim(strHotelValue.Trim.ToUpper) & ")) """
                End If
            End If


            If strTicketValue <> "" Then
                If Trim(strWhereCond) = "" Then
                    strWhereCond = " RIGHT('000000'+CAST(EmailId AS VARCHAR(6)),6)  IN ( " & Trim(strTicketValue) & ")"
                Else

                    strWhereCond = strWhereCond & " AND RIGHT('000000'+CAST(EmailId AS VARCHAR(6)),6)  IN ( " & Trim(strTicketValue) & ")"
                End If
            End If

            If strHotelStatusValue <> "" Then
                If Trim(strWhereCond) = "" Then
                    strWhereCond = " EmailId in (select distinct EmailId from Contract_Email where HotelId in(select distinct partycode from partymast where hotelstatuscode in (select h.hotelstatuscode  from hotelstatus h where h.active=1 and h.hotelstatusname=" & Trim(strHotelStatusValue) & ")))"
                Else
                    strWhereCond = strWhereCond & " AND EmailId in (select distinct EmailId from Contract_Email where HotelId in(select distinct partycode from partymast where hotelstatuscode in (select h.hotelstatuscode  from hotelstatus h where h.active=1 and h.hotelstatusname=" & Trim(strHotelStatusValue) & ")))"
                End If
            End If

            If strTrackingStatusValue <> "" Then
                If Trim(strWhereCond) = "" Then
                    strWhereCond = "  EmailId in (select C.EmailId from Contract_Email C where C.TrackingStatus=(" & Trim(strTrackingStatusValue) & ")) "
                Else

                    strWhereCond = strWhereCond & " AND   EmailId in (select C.EmailId from Contract_Email C where C.TrackingStatus=(" & Trim(strTrackingStatusValue) & ")) "
                End If
            End If

            If strFromEmailValue <> "" Then
                If Trim(strWhereCond) = "" Then
                    strWhereCond = " upper(EmailFrom) IN ( " & Trim(strFromEmailValue) & ")"
                Else

                    strWhereCond = strWhereCond & " AND upper(EmailFrom) IN (" & Trim(strFromEmailValue) & ")"
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
                            strWhereCond1 = " (EmailId in (select C.EmailId from Contract_Email C where C.HotelId in (select P.partycode from partymast P where upper(P.partyname) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%'  or  RIGHT('000000'+CAST(EmailId AS VARCHAR(6)),6)  LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' or   EmailId in (select distinct EmailId from Contract_Email where HotelId in(select distinct partycode from partymast where hotelstatuscode in (select h.hotelstatuscode  from hotelstatus h where h.active=1 and h.hotelstatusname like '%" & Trim(strValue.Trim.ToUpper) & "%')))  or  EmailId in (select C.EmailId from Contract_Email C where C.TrackingStatus LIKE ('%" & Trim(strValue.Trim.ToUpper) & "%')) or  upper(EmailFrom) LIKE ('%" & Trim(strValue.Trim.ToUpper) & "%'))))"
                        Else
                            strWhereCond1 = strWhereCond1 & " OR    (EmailId in (select C.EmailId from Contract_Email C where C.HotelId in (select P.partycode from partymast P where upper(P.partyname) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%'  or  RIGHT('000000'+CAST(EmailId AS VARCHAR(6)),6)  LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' or  EmailId in (select distinct EmailId from Contract_Email where HotelId in(select distinct partycode from partymast where hotelstatuscode in (select h.hotelstatuscode  from hotelstatus h where h.active=1 and h.hotelstatusname like '%" & Trim(strValue.Trim.ToUpper) & "%')))  or  EmailId in (select C.EmailId from Contract_Email C where C.TrackingStatus LIKE ('%" & Trim(strValue.Trim.ToUpper) & "%')) or  upper(EmailFrom) LIKE ('%" & Trim(strValue.Trim.ToUpper) & "%'))))"
                        End If
                    End If
                Next
                If Trim(strWhereCond) = "" Then
                    strWhereCond = " (" & strWhereCond1 & ")"
                Else
                    strWhereCond = strWhereCond & " AND (" & strWhereCond1 & ")"
                End If

            End If



            If Trim(strWhereCond) <> "" Then
                strSqlQry = strSqlQry & " Where " & strWhereCond
            Else
                strSqlQry = strSqlQry
            End If



            Dim myDS As New DataSet
            Dim constring As String = ConfigurationManager.ConnectionStrings("strDBConnection").ConnectionString
            Using con As New SqlConnection(constring)
                Using cmd As New SqlCommand("GetEmailsPageWise_VisualSearch", con)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.Parameters.AddWithValue("@PageIndex", pageIndex)
                    cmd.Parameters.AddWithValue("@PageSize", PageSize)
                    cmd.Parameters.AddWithValue("@SqlQuery", strSqlQry)
                    cmd.Parameters.Add("@RecordCount", SqlDbType.Int, 4)
                    cmd.Parameters("@RecordCount").Direction = ParameterDirection.Output
                    con.Open()
                    Dim idr As IDataReader = cmd.ExecuteReader()
                    dlMailInboxIgnore.DataSource = idr
                    dlMailInboxIgnore.DataBind()
                    idr.Close()
                    con.Close()
                    Dim recordCount As Integer = Convert.ToInt32(cmd.Parameters("@RecordCount").Value)
                    Me.PopulateIgnorePager(recordCount, pageIndex)
                End Using
            End Using



        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ContractsTrack.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally


        End Try
    End Sub

    Protected Sub lbCloseInboxIgnore_Click(sender As Object, e As System.EventArgs)
        Try


            Dim myButton As LinkButton = CType(sender, LinkButton)
            Dim dlItem As DataListItem = CType((CType(sender, LinkButton)).NamingContainer, DataListItem)
            Dim lb As LinkButton = CType(dlItem.FindControl("lbInboxIgnore"), LinkButton)


            Dim dtDynamicsInbox As New DataTable
            dtDynamicsInbox = Session("sDtDynamicInboxIgnore")

            If dtDynamicsInbox.Rows.Count > 0 Then
                Dim j As Integer
                For j = dtDynamicsInbox.Rows.Count - 1 To 0 Step j - 1
                    If lb.Text.Trim = dtDynamicsInbox.Rows(j)("Value").ToString.Trim Then
                        dtDynamicsInbox.Rows.Remove(dtDynamicsInbox.Rows(j))
                    End If
                Next

            End If

            Session("sDtDynamicInboxIgnore") = dtDynamicsInbox
            dlIgnore.DataSource = dtDynamicsInbox
            dlIgnore.DataBind()

            FillInboxIgnoreByVS(1)


        Catch ex As Exception

        End Try
    End Sub

    'Private Sub FillGridByLinkButtonForIgnore()
    '    Dim strorderby As String = "partymast.partyname"
    '    Dim strsortorder As String = "ASC"
    '    Dim myDS As New DataSet
    '    Dim strWhereCond As String = ""
    '    gvHotels.Visible = True

    '    Dim strlbValue As String = hdLinkButtonValue.Value
    '    If gvHotels.PageIndex < 0 Then
    '        gvHotels.PageIndex = 0
    '    End If


    '    Dim dtsHotelDetailsFilter As DataTable
    '    Dim strCodes As String = ""
    '    dtsHotelDetailsFilter = Session("sDtHotelDetails")
    '    If dtsHotelDetailsFilter.Rows.Count > 0 Then
    '        For j As Integer = 0 To dtsHotelDetailsFilter.Rows.Count - 1
    '            strCodes = strCodes & "'" & dtsHotelDetailsFilter.Rows(j)("Code").ToString().Trim & "',"
    '        Next
    '    Else
    '        If strlbValue = "%" Then
    '            strCodes = "'''"
    '        ElseIf strlbValue = "%%" Then
    '            strCodes = ""
    '            strlbValue = "%"
    '        End If

    '    End If

    '    strSqlQry = ""

    '    strSqlQry = "select partycode HotelCode,partyname HotelName,(select h.hotelstatusname from hotelstatus h where h.active=1 and h.hotelstatuscode=partymast.hotelstatuscode)  HotelStatus,(select TrackingStatus from Contract_Email TS where TS.HotelId=partymast.partycode and TS.EmailId=" & lblIdPopup.Text & ") as TrackingStatus,0 sortorder  from partymast where sptypecode='HOT' and active=1 "

    '    If strlbValue.Trim <> "" Then


    '        strlbValue = "'" & strlbValue & "'"
    '        If Trim(strWhereCond) = "" Then
    '            strWhereCond = " upper(partymast.partyname) LIKE " & Trim(strlbValue) & ""
    '        Else

    '            strWhereCond = strWhereCond & " AND upper(partymast.partyname) LIKE " & Trim(strlbValue) & ""
    '        End If

    '        If strCodes <> "" Then
    '            strCodes = strCodes.Remove(strCodes.Length - 1)
    '            If Trim(strWhereCond) = "" Then
    '                strWhereCond = " upper(partymast.partycode) IN (" & Trim(strCodes) & ")"
    '            Else

    '                strWhereCond = strWhereCond & " AND upper(partymast.partycode) IN  (" & Trim(strCodes) & ")"
    '            End If
    '        End If


    '    End If


    '    If Trim(strWhereCond) <> "" Then
    '        strSqlQry = strSqlQry & " and " & strWhereCond & " ORDER BY " & strorderby & " " & strsortorder
    '    Else
    '        strSqlQry = strSqlQry & " ORDER BY " & strorderby & " " & strsortorder
    '    End If

    '    SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
    '    myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
    '    myDataAdapter.Fill(myDS)
    '    gvHotels.DataBind()


    '    Dim dtsHotelDetails As DataTable
    '    Dim strValues As String = ""
    '    Dim strQuery As String = ""
    '    dtsHotelDetails = Session("sDtHotelDetails")

    '    If myDS.Tables(0).Rows.Count > 0 Then

    '        For i As Integer = 0 To myDS.Tables(0).Rows.Count - 1
    '            strValues = ""
    '            strValues = myDS.Tables(0).Rows(i)("HotelCode").ToString
    '            If dtsHotelDetails.Rows.Count > 0 Then
    '                For j As Integer = 0 To dtsHotelDetails.Rows.Count - 1
    '                    If dtsHotelDetails.Rows(j)("Code").ToString().Trim = strValues.Trim Then
    '                        myDS.Tables(0).Rows(i)("sortorder") = 1
    '                        Exit For
    '                    End If
    '                Next

    '            End If
    '        Next
    '    End If
    '    'End If

    '    Dim dataView As DataView = New DataView(myDS.Tables(0))
    '    dataView.Sort = "sortorder desc, HotelName asc"
    '    gvHotels.DataSource = dataView
    '    If myDS.Tables(0).Rows.Count > 0 Then
    '        gvHotels.DataBind()
    '    Else
    '        gvHotels.PageIndex = 0
    '        gvHotels.DataBind()
    '        lblMsg.Visible = True
    '        lblMsg.Text = "Records not found."
    '    End If
    'End Sub
    ' ''' <summary>
    ' ''' 
    ' ''' </summary>
    ' ''' <remarks></remarks>
    'Private Sub FillCheckbox()
    '    Dim dtsHotelGroupDetails As DataTable
    '    dtsHotelGroupDetails = Session("sDtHotelDetails")

    '    Dim row As GridViewRow
    '    Dim iFlag As Integer = 0
    '    If dtsHotelGroupDetails.Rows.Count > 0 Then

    '        For Each row In gvHotels.Rows

    '            Dim ChkBoxRows As CheckBox = CType(row.FindControl("chkSelect"), CheckBox)

    '            Dim lblHotelCode As Label = CType(row.FindControl("lblHotelCode"), Label)
    '            Dim lblHotelName As Label = CType(row.FindControl("lblHotelName"), Label)
    '            Dim txttrackingStatus As TextBox = CType(row.FindControl("txttrackingStatus"), TextBox)

    '            For i As Integer = 0 To dtsHotelGroupDetails.Rows.Count - 1
    '                If dtsHotelGroupDetails.Rows(i)("Code").ToString.Trim = lblHotelCode.Text.Trim Then
    '                    ChkBoxRows.Checked = True
    '                    txttrackingStatus.Visible = True
    '                    Exit For
    '                Else
    '                    ChkBoxRows.Checked = False
    '                    txttrackingStatus.Visible = False
    '                End If

    '            Next
    '        Next

    '    End If
    'End Sub

    Private Sub FillMailBoxIgnore(pageIndex As Integer)
        Dim myDS As New DataSet

        Try

            Dim constring As String = ConfigurationManager.ConnectionStrings("strDBConnection").ConnectionString
            Using con As New SqlConnection(constring)
                Using cmd As New SqlCommand("GetEmailsPageWise_Ignore", con)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.Parameters.AddWithValue("@PageIndex", pageIndex)
                    cmd.Parameters.AddWithValue("@PageSize", PageSize)
                    cmd.Parameters.Add("@RecordCount", SqlDbType.Int, 4)
                    cmd.Parameters("@RecordCount").Direction = ParameterDirection.Output
                    con.Open()
                    Dim idr As IDataReader = cmd.ExecuteReader()
                    dlMailInboxIgnore.DataSource = idr
                    dlMailInboxIgnore.DataBind()
                    idr.Close()
                    con.Close()
                    Dim recordCount As Integer = Convert.ToInt32(cmd.Parameters("@RecordCount").Value)
                    Me.PopulateIgnorePager(recordCount, pageIndex)
                End Using
            End Using



        Catch ex As Exception
            objUtils.WritErrorLog("ContractTrack.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try

    End Sub

    Private Sub PopulateIgnorePager(recordCount As Integer, currentPage As Integer)
        Dim pages As New List(Of ListItem)()
        Dim startIndex As Integer, endIndex As Integer
        Dim pagerSpan As Integer = 3

        'Calculate the Start and End Index of pages to be displayed.
        Dim dblPageCount As Double = CDbl(CDec(recordCount) / Convert.ToDecimal(PageSize))
        Dim pageCount As Integer = CInt(Math.Ceiling(dblPageCount))
        startIndex = If(currentPage > 1 AndAlso currentPage + pagerSpan - 1 < pagerSpan, currentPage, 1)
        endIndex = If(pageCount > pagerSpan, pagerSpan, pageCount)
        If currentPage > pagerSpan Mod 2 Then
            If currentPage = 2 Then
                endIndex = 3
            Else
                endIndex = currentPage + 2
            End If
        Else
            endIndex = (pagerSpan - currentPage) + 1
        End If

        If endIndex - (pagerSpan - 1) > startIndex Then
            startIndex = endIndex - (pagerSpan - 1)
        End If

        If endIndex > pageCount Then
            endIndex = pageCount
            startIndex = If(((endIndex - pagerSpan) + 1) > 0, (endIndex - pagerSpan) + 1, 1)
        End If

        'Add the First Page Button.
        If currentPage > 1 Then
            pages.Add(New ListItem("First", "1"))
        End If

        'Add the Previous Button.
        If currentPage > 1 Then
            pages.Add(New ListItem("<<", (currentPage - 1).ToString()))
        End If

        For i As Integer = startIndex To endIndex
            pages.Add(New ListItem(i.ToString(), i.ToString(), i <> currentPage))
        Next

        Dim iMod As Integer
        iMod = pageCount Mod pagerSpan
        If iMod = 0 Then
            iMod = 2
        Else
            iMod = 1
        End If
        'Add the Next Button.
        If currentPage < pageCount And pagerSpan < pageCount Then


            If (pagerSpan + currentPage) <= pageCount Or currentPage < pageCount - iMod Then
                If pageCount - (pagerSpan - 1) <> currentPage Then
                    pages.Add(New ListItem(">>", (currentPage + 1).ToString()))

                End If
            End If

        End If

        'Add the Last Button.
        If currentPage <> pageCount And pagerSpan < pageCount Then
            If (pagerSpan + currentPage) <= pageCount Or currentPage < pageCount - iMod Then
                If pageCount - (pagerSpan - 1) <> currentPage Then
                    pages.Add(New ListItem("Last", pageCount.ToString()))
                End If

            End If
        End If
        rptrIgnore.DataSource = pages
        rptrIgnore.DataBind()
    End Sub

    Protected Sub btnvsprocessInboxIngnore_Click(sender As Object, e As System.EventArgs) Handles btnvsprocessInboxIgnore.Click
        Dim lsSearchTxt As String = ""
        lsSearchTxt = txtvsprocesssplitInbox_Ignore.Text.Replace("___", "<").Replace("...", ">")
        Dim lsProcessText As String = ""
        Dim lsMainArr As String()
        Dim IsProcessType As String = ""
        Dim IsProcessValue As String = ""
        lsMainArr = objUtils.splitWithWords(lsSearchTxt, "|~,")

        For i = 0 To lsMainArr.GetUpperBound(0)
            Select Case lsMainArr(i).Split(":")(0).ToString.ToUpper.Trim
                Case "HOTELS"
                    lsProcessText = lsMainArr(i).Split(":")(1)
                    sbAddToDataTableInboxIgnore("HOTELS", lsProcessText, "H")
                    IsProcessType = "H"
                Case "TICKECT NO"
                    lsProcessText = lsMainArr(i).Split(":")(1)
                    sbAddToDataTableInboxIgnore("TICKECT NO", lsProcessText, "TN")
                    IsProcessType = "TN"
                Case "HOTEL_STATUS"
                    lsProcessText = lsMainArr(i).Split(":")(1)
                    sbAddToDataTableInboxIgnore("HOTEL_STATUS", lsProcessText, "HS")
                    IsProcessType = "HS"
                Case "TRACKING_STATUS"
                    lsProcessText = lsMainArr(i).Split(":")(1)
                    sbAddToDataTableInboxIgnore("TRACKING_STATUS", lsProcessText, "TS")
                    IsProcessType = "TS"
                Case "FROM_EMAIL"
                    lsProcessText = lsMainArr(i).Split(":")(1)
                    sbAddToDataTableInboxIgnore("FROM_EMAIL", lsProcessText, "FM")
                    IsProcessType = "FM"
                Case "TEXT"
                    lsProcessText = lsMainArr(i).Split(":")(1)
                    sbAddToDataTableInboxIgnore("TEXT", lsProcessText, "T")
                    IsProcessType = "T"
            End Select
        Next

        Dim dttDyn As DataTable
        dttDyn = Session("sDtDynamicInboxIgnore")
        dlIgnore.DataSource = dttDyn
        dlIgnore.DataBind()
        FillInboxIgnoreByVS(1)

    End Sub

    Function sbAddToDataTableInboxIgnore(ByVal lsName As String, ByVal lsValue As String, ByVal lsShortCode As String) As Boolean
        Dim iFlag As Integer = 0
        Dim dtt As DataTable
        dtt = Session("sDtDynamicInboxIgnore")
        If dtt.Rows.Count >= 0 Then
            For i = 0 To dtt.Rows.Count - 1
                If dtt.Rows(i)("Code").ToString.Trim.ToUpper = lsName.Trim.ToUpper And dtt.Rows(i)("Value").ToString.Trim.ToUpper = lsValue.Trim.ToUpper Then
                    iFlag = 1
                End If
            Next
            If iFlag = 0 Then
                dtt.NewRow()
                dtt.Rows.Add(lsName.Trim, lsValue.Trim)
                Session("sDtDynamicInboxIgnore") = dtt
            End If
        End If
        Return True
    End Function

    Protected Sub btnProcessIgnoreclickMail_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnProcessIgnoreclickMail.Click

        For Each row As DataListItem In dlMailInboxIgnore.Items
            Dim lblId As Label = CType(row.FindControl("lblId"), Label)
            Dim tblRow As HtmlControl = CType(row.FindControl("tblRowIgnore"), HtmlControl)
            If lblId.Text = hdEmailCodeIgnore.Value Then
                tblRow.Attributes.Add("class", "EntryLineActive")

            Else
                tblRow.Attributes.Add("class", "EntryLine")
            End If
        Next row


        trMessage.Visible = False
        Dim strCode As String = hdEmailCodeIgnore.Value
        Dim SqlConn As SqlConnection
        Dim myDataAdapter As SqlDataAdapter
        Dim myDS As New DataSet
        Dim strAttachment As String = ""
        Dim strSqlQry As String = "select EmailId,EmailFrom,'Sub: '+EmailSubject EmailSubject,CONVERT(VARCHAR(11), EmailDate, 113) + ' ' +right(convert(varchar(32),EmailDate,100),8)EmailDate,EmailFullSource,EmailAttachment,RIGHT('000000'+CAST(EmailId AS VARCHAR(6)),6)EmailNo from Email_Inbox where EmailId=" & strCode
        SqlConn = clsDBConnect.dbConnectionnew(HttpContext.Current.Session("dbconnectionName"))                             'Open connection
        myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
        myDataAdapter.Fill(myDS, "Details")
        If myDS.Tables.Count > 0 Then
            If myDS.Tables(0).Rows.Count > 0 Then
                lblFromIgnore.Text = Server.HtmlEncode(myDS.Tables(0).Rows(0)("EmailFrom").ToString)
                lblDateIgnore.Text = myDS.Tables(0).Rows(0)("EmailDate").ToString
                lblSubjectIgnore.Text = myDS.Tables(0).Rows(0)("EmailSubject").ToString
                lblBodyIgnore.Text = (myDS.Tables(0).Rows(0)("EmailFullSource").ToString.Replace("img", "img style='Display:none;'")).Replace(":blue", ":#06788B")
                strAttachment = myDS.Tables(0).Rows(0)("EmailAttachment").ToString
                'EmailNo
                If strAttachment.Trim = "Y" Then
                    FillAttachmentsIgnore(strCode)
                Else
                    '    DLAttachments.DataSource = myDS
                    dlIgnoreAttachment.DataBind()
                End If

            End If
        End If

    End Sub

    Private Sub FillAttachmentsIgnore(strCode As String)
        Dim SqlConn As SqlConnection
        Dim myDataAdapter As SqlDataAdapter
        Dim myDS As New DataSet
        Dim strAttachment As String = ""
        Dim strSqlQry As String = "select EmailId,FileName,Content from Email_Inbox_Attachment where EmailId=" & strCode
        SqlConn = clsDBConnect.dbConnectionnew(HttpContext.Current.Session("dbconnectionName"))                             'Open connection
        myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
        myDataAdapter.Fill(myDS, "Details")
        dlIgnoreAttachment.DataSource = myDS
        dlIgnoreAttachment.DataBind()


    End Sub

    Protected Sub dlIgnoreAttachment_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.DataListItemEventArgs) Handles dlIgnoreAttachment.ItemDataBound
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then

            Dim lblfile As Label = CType(e.Item.FindControl("lblfile"), Label)
            Dim imgAttachmentType As Image = CType(e.Item.FindControl("imgAttachmentType"), Image)
            Dim filePath As String = "~\Attachment\" '+ lblfile.Text
            Dim path As String = Server.MapPath(filePath)
            '   Dim path As String = "C:\Program Files\Mahce\Attachment"
            '  Dim file As System.IO.FileInfo = New System.IO.FileInfo(path)
            ' Dim str As String = lblfile.Text.Replace(vbCrLf, "").Replace(vbCr, "").Replace("\n", "").Replace("\r", "")
            Dim strExt As String = System.IO.Path.GetExtension(lblfile.Text.Replace(vbCrLf, " ")).ToUpper
            '  Dim strExt As String = System.IO.Path.GetExtension(str).ToUpper
            Select Case strExt.Trim
                Case ".PDF"
                    imgAttachmentType.ImageUrl = "~/Images/crystaltoolbar/pdf.png"
                Case ".JPG", ".JPEG", ".GIF", ".PNG"
                    imgAttachmentType.ImageUrl = "~/Images/crystaltoolbar/jpg file.png"
                Case ".DOCX"
                    imgAttachmentType.ImageUrl = "~/Images/crystaltoolbar/Word_2007.png"
                Case ".DOC"
                    imgAttachmentType.ImageUrl = "~/Images/crystaltoolbar/MS_word2003.png"
                Case ".XLSX", ".XLS"
                    imgAttachmentType.ImageUrl = "~/Images/crystaltoolbar/excel.ico"
                Case Else
                    imgAttachmentType.ImageUrl = "~/Images/crystaltoolbar/mail-attachment2.png"
            End Select

        End If

    End Sub

    Protected Sub IgnorePage_Changed(sender As Object, e As EventArgs)
        Dim pageIndex As Integer = Integer.Parse(TryCast(sender, LinkButton).CommandArgument)
        ' Session("sMailBoxPageIndex") = pageIndex.ToString
        FillInboxIgnoreByVS(pageIndex)
    End Sub

    <System.Web.Script.Services.ScriptMethod()> _
  <System.Web.Services.WebMethod()> _
    Public Shared Function GetTrackingNumbers(ByVal prefixText As String) As List(Of String)
        Dim strSqlQry As String = ""
        Dim myDS As New DataSet
        Dim strTypeName As New List(Of String)
        Try
            prefixText = prefixText.Replace(" ", "")
            strSqlQry = "select EmailLineNo +','+ C.EmailId+','+ rtrim(c.HotelId) +'' TrackingCode,EmailLineNo +' [Email Code: '+ RIGHT('000000'+CAST(C.EmailId AS VARCHAR(6)),6)+', Hotel Code: '+ rtrim(c.HotelId) +', Hotel Name: '+ isnull((select partyname from partymast where partymast.partycode=c.HotelId),'') +']' TrackNo from VIEW_CONTRACT_EMAIL_WITH_LINENO V, Contract_Email C where v.EmailId=c.EmailId and v.HotelId=c.HotelId and ltrim(rtrim(TrackingStatus)) <> 'On Hold' and EmailLineNo +' [Email Code: '+ RIGHT('000000'+CAST(C.EmailId AS VARCHAR(6)),6)+', Hotel Code: '+ rtrim(c.HotelId) +', Hotel Name: '+ isnull((select partyname from partymast where partymast.partycode=c.HotelId),'') +']'  like '%" & prefixText & "%' "
            Dim SqlConn As New SqlConnection
            Dim myDataAdapter As New SqlDataAdapter
            SqlConn = clsDBConnect.dbConnectionnew("strDBConnection")
            'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)
            If myDS.Tables(0).Rows.Count > 0 Then
                For i As Integer = 0 To myDS.Tables(0).Rows.Count - 1
                    strTypeName.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(myDS.Tables(0).Rows(i)("TrackNo").ToString(), myDS.Tables(0).Rows(i)("TrackingCode").ToString()))
                Next
            End If
            Return strTypeName
        Catch ex As Exception
            Return strTypeName
        End Try
    End Function

    Protected Sub btnAssign_Click(sender As Object, e As System.EventArgs) Handles btnAssign.Click
        Dim result1 As Integer = 0
        Dim frmmode As String = 0
        Dim strPassQry As String = "false"
        Try

            If Page.IsValid = True Then


                SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                sqlTrans = SqlConn.BeginTransaction           'SQL  Trans start
                myCommand = New SqlCommand("sp_Contract_Email_Additional", SqlConn, sqlTrans)

                Dim strTrackCode As String = txtTrackingcode.Text
                Dim strTrackCodes() As String = txtTrackingcode.Text.Split(",")

                myCommand.CommandType = CommandType.StoredProcedure
                myCommand.Parameters.Add(New SqlParameter("@EmailCode", SqlDbType.VarChar, 50)).Value = strTrackCodes(1).Trim
                myCommand.Parameters.Add(New SqlParameter("@HotelCode", SqlDbType.VarChar, 50)).Value = strTrackCodes(2).Trim
                myCommand.Parameters.Add(New SqlParameter("@AdditionalEmailCode", SqlDbType.VarChar, 50)).Value = lblIdPopup.Text.Trim
                myCommand.ExecuteNonQuery()

                sqlTrans.Commit()    'SQl Tarn Commit
                clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
                clsDBConnect.dbCommandClose(myCommand)               'sql command disposed
                clsDBConnect.dbConnectionClose(SqlConn)           'connection close

                If Not Session("sMailBoxPageIndex") Is Nothing Then
                    FillMailBox(Session("sMailBoxPageIndex"))
                Else
                    FillMailBox(1)
                End If
                ClosePopUp()

                txtTrackingcode.Text = ""
                txtTrackingNo.Text = ""
                FillTracking()
                FillMailBoxIgnore(1)

            End If
        Catch ex As Exception
            If SqlConn.State = ConnectionState.Open Then
                sqlTrans.Rollback()

            End If

            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)

            objUtils.WritErrorLog("ContractTrack.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub


    Private Sub FillAdditionalEmails(strCode As String, strhotelCode As String)
        Dim SqlConn As SqlConnection
        Dim myDataAdapter As SqlDataAdapter
        Dim myDS As New DataSet
        Dim strAttachment As String = ""
        Dim strSqlQry As String = "select " & strCode & " AdditionalEmailId,RIGHT('000000'+CAST(" & strCode & " AS VARCHAR(6)),6)+'(Original)' as AdditionalEmailIdName union select AdditionalEmailId,RIGHT('000000'+CAST(AdditionalEmailId AS VARCHAR(6)),6)AdditionalEmailIdName from Contract_Email_Additional where EmailId='" & strCode & "' and HotelId='" & strhotelCode & "'  order by AdditionalEmailId desc"

        SqlConn = clsDBConnect.dbConnectionnew(HttpContext.Current.Session("dbconnectionName"))                             'Open connection
        myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
        myDataAdapter.Fill(myDS, "Details")

        If myDS.Tables.Count > 0 Then
            If myDS.Tables(0).Rows.Count > 0 Then
                dlAdditionalEmails.DataSource = myDS
                dlAdditionalEmails.DataBind()
            Else
                dlAdditionalEmails.DataBind()
            End If
        Else
            dlAdditionalEmails.DataBind()
        End If


    End Sub

    Protected Sub lbAdditionalEmails_Click(sender As Object, e As System.EventArgs)

        Dim lbAdditionalEmails As LinkButton = CType(sender, LinkButton)
        Dim dlRow As DataListItem = CType(lbAdditionalEmails.NamingContainer, DataListItem)
        Dim hdAdditionalEmails As HiddenField = CType(dlRow.FindControl("hdAdditionalEmails"), HiddenField)

        Dim strCode As String = hdAdditionalEmails.Value
        Dim SqlConn As SqlConnection
        Dim myDataAdapter As SqlDataAdapter
        Dim myDS As New DataSet
        Dim strAttachment As String = ""
        Dim strSqlQry As String = "select EmailId,EmailFrom,'Sub: '+EmailSubject EmailSubject,CONVERT(VARCHAR(11), EmailDate, 113) + ' ' +right(convert(varchar(32),EmailDate,100),8)EmailDate,EmailFullSource,EmailAttachment,RIGHT('000000'+CAST(EmailId AS VARCHAR(6)),6)EmailNo from Email_Inbox where EmailId=" & strCode
        SqlConn = clsDBConnect.dbConnectionnew(HttpContext.Current.Session("dbconnectionName"))                             'Open connection
        myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
        myDataAdapter.Fill(myDS, "Details")
        If myDS.Tables.Count > 0 Then
            If myDS.Tables(0).Rows.Count > 0 Then
                lblFromPopup.Text = Server.HtmlEncode(myDS.Tables(0).Rows(0)("EmailFrom").ToString)
                lblDatePopup.Text = myDS.Tables(0).Rows(0)("EmailDate").ToString
                lblSubjectPopup.Text = myDS.Tables(0).Rows(0)("EmailSubject").ToString
                lblBodyPopup.Text = (myDS.Tables(0).Rows(0)("EmailFullSource").ToString.Replace("img", "img style='Display:none;'")).Replace(":blue", ":#06788B")
                strAttachment = myDS.Tables(0).Rows(0)("EmailAttachment").ToString
                'EmailNo
                If strAttachment.Trim = "Y" Then
                    FillAttachmentsPopup(strCode)
                Else
                    DLAttachmentPopup.DataBind()
                End If

                'strAttachment = myDS.Tables(0).Rows(0)("EmailAttachment").ToString
                ''EmailNo
                'If strAttachment.Trim = "Y" Then
                '    FillAttachments(strCode)
                'Else
                '    '    DLAttachments.DataSource = myDS
                '    DLAttachments.DataBind()
                'End If

            End If
        End If
    End Sub
    Protected Sub btnAddHotel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddHotel.Click
        Try

            Session.Add("SupState", "New")
            Dim strpop As String = ""
            Dim iMailId As Integer = lblIdPopup.Text
            strpop = "window.open('SupMain.aspx?appid=" + CType(Request.QueryString("appid"), String) + "&State=New&Popup=" & iMailId & "','Suppliers');"
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)

        Catch ex As Exception
            objUtils.WritErrorLog("ContractTrack.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
End Class
