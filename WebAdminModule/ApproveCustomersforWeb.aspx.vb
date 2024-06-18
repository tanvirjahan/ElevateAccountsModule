'------------================--------------=======================------------------================
'   Module Name    :    ApproveCustomersforWeb.aspx
'   Developer Name :    Govardhan
'   Date           :    
'   
'
'------------================--------------=======================------------------================

#Region "Namespace"
Imports System.Data
Imports System.Data.SqlClient
Imports System.Net.Mail
Imports System.Net.Mail.SmtpClient
Imports System.Globalization
#End Region

Partial Class ApproveCustomersforWeb
    Inherits System.Web.UI.Page

#Region "Global Declaration"
    Dim objEmail As New clsEmail
    Dim objUtils As New clsUtils
    Dim objUser As New clsUser
    Dim strSqlQry As String
    Dim strWhereCond As String
    Dim SqlConn As SqlConnection
    Dim myCommand As SqlCommand
    Dim myDataAdapter As SqlDataAdapter
    Dim mySqlReader As SqlDataReader
    Dim sqlTrans As SqlTransaction
    Dim mySqlCmd As SqlCommand
    Dim mySqlConn As SqlConnection

#End Region

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
    Private Sub FilterGrid()
        Dim lsSearchTxt As String = ""
        lsSearchTxt = txtvsprocesssplit.Text
        Dim lsProcessCity As String = ""
        Dim lsProcessCountry As String = ""
        Dim lsProcessCat As String = ""
        Dim lsProcessSector As String = ""
        Dim lsProcessAll As String = ""



        Dim lsMainArr As String()
        lsMainArr = objUtils.splitWithWords(lsSearchTxt, "|~,")
        For i = 0 To lsMainArr.GetUpperBound(0)
            Select Case lsMainArr(i).Split(":")(0).ToString.ToUpper.Trim
                Case "CUSTOMER"
                    lsProcessCountry = lsMainArr(i).Split(":")(1)
                    sbAddToDataTable("CUSTOMER", lsProcessCountry, "C")
                Case "CUSTOMERGROUP"
                    lsProcessCountry = lsMainArr(i).Split(":")(1)
                    sbAddToDataTable("CUSTOMERGROUP", lsProcessCountry, "CSG")
                Case "COUNTRY"
                    lsProcessCountry = lsMainArr(i).Split(":")(1)
                    sbAddToDataTable("COUNTRY", lsProcessCountry, "CTY")
                Case "COUNTRYGROUP"
                    lsProcessCountry = lsMainArr(i).Split(":")(1)
                    sbAddToDataTable("COUNTRYGROUP", lsProcessCountry, "CG")
                Case "SECTOR"
                    lsProcessCountry = lsMainArr(i).Split(":")(1)
                    sbAddToDataTable("SECTOR", lsProcessCountry, "S")

                Case "CITY"
                    lsProcessCity = lsMainArr(i).Split(":")(1)
                    sbAddToDataTable("CITY", lsProcessCity, "CT")
                Case "CATEGORY"
                    lsProcessCat = lsMainArr(i).Split(":")(1)
                    sbAddToDataTable("CATEGORY", lsProcessCat, "CTG")

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
    Protected Sub btnExportToExcel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExportToExcel.Click

        Dim DS As New DataSet
        Dim DA As SqlDataAdapter
        Dim con As SqlConnection
        Dim objcon As New clsDBConnect

        Try
            If grdUploadClients.Rows.Count <> 0 Then

                strSqlQry = " SELECT agentmast.agentcode as[Agent Code],agentname as [Agent Name],shortname as [Short Name],bookingengineratetype as [Booking Engine Format],agentmast.webusername as [Web User],webemail as [Web Email],webcontact as [Web Contact]," & _
"[webApprove]=case isnull(webapprove,0) when 1 then 'Yes' else 'No' end ,dbo.pwddecript(webpassword)  as[Web Password]," & _
 " case when isnull(agents_locked.agentcode,'')='' then 'No' else 'Yes' end  as [Status],appuser as[User Approved],appdate as[Approved Date] FROM agentmast  " & _
 "  inner join   ctrymast on agentmast.ctrycode=ctrymast.ctrycode  inner join citymast on agentmast.citycode= citymast.citycode " & _
  " inner join agentcatmast on agentmast.catcode=agentcatmast.agentcatcode left outer join agents_locked" & _
   " on agentmast.agentcode =agents_locked.agentcode where agentmast.active = 1 and agentmast.webapprove=1 "

                'If Trim(BuildCondition) <> "" Then
                '    strSqlQry = strSqlQry & " AND " & BuildCondition() & " ORDER BY partycode "
                'Else
                '    strSqlQry = strSqlQry & " ORDER BY partycode "
                'End If
                con = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))

                DA = New SqlDataAdapter(strSqlQry, con)

                DA.Fill(DS, "Approvedcustomers")

                If DS.Tables(0).Rows.Count > 0 Then

                    objUtils.ExportToExcel(DS, Response)
                    con.Close()
                End If
            Else
                objUtils.MessageBox("Sorry , No data availabe for export to excel.", Page)
            End If

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        End Try
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try

            If Page.IsPostBack = False Then

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
                ' SetFocus(txtBanktypecode)

                If CType(Session("GlobalUserName"), String) = Nothing Or CType(Session("Userpwd"), String) = Nothing Then
                    Response.Redirect("~/Login.aspx", False)
                    Exit Sub
                Else
                    objUser.CheckUserRight(Session("dbconnectionName"), CType(Session("GlobalUserName"), String), CType(Session("Userpwd"), String), _
                                                     CType(strappname, String), "WebAdminModule\ApproveCustomersforWeb.aspx?appid=" + strappid, btnAddNew, btnExportToExcel, _
                                                     btnPrint, grdUploadClients)

                End If
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
                Session.Add("strAgentsortExpression", "agentcode")
                Session.Add("strAgentsortdirection", SortDirection.Ascending)

                FillGridNew()



            Else
                Dim strstring As String = "'"
            End If
            If (Me.IsPostBack And Me.Request("__EVENTTARGET") = "ApproveCustomersforWeb") Then
                ' btnSearch_Click(sender, e)
                FillGridNew()
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        End Try
    End Sub



#Region "  Private Function BuildCondition() As String"
    Private Function BuildCondition() As String
        'Dim strWhereCond As String
        'strWhereCond = ""
        ''agentmast.agentcode
        'If txtCustomerCode.Value <> "" Then
        '    If Trim(strWhereCond) = "" Then
        '        strWhereCond = " agentmast.agentcode LIKE '" & txtCustomerCode.Value & "%'"
        '    Else
        '        strWhereCond = strWhereCond & " AND agentmast.agentcode LIKE '" & txtCustomerCode.Value & "%'"
        '    End If
        'End If

        'If txtcustomername.Text <> "" Then
        '    If Trim(strWhereCond) = "" Then
        '        strWhereCond = " agentmast.agentname LIKE '" & txtcustomername.Text & "%'"
        '    Else
        '        strWhereCond = strWhereCond & " AND agentmast.agentname LIKE '" & txtcustomername.Text & "%'"
        '    End If
        'End If

        'If ddlMarket.Value <> "[Select]" Then
        '    If Trim(strWhereCond) = "" Then
        '        strWhereCond = " agentmast.plgrpcode = '" & ddlMarket.Items(ddlMarket.SelectedIndex).Text & "'"
        '    Else
        '        strWhereCond = strWhereCond & " AND agentmast.plgrpcode = '" & ddlMarket.Items(ddlMarket.SelectedIndex).Text & "'"
        '    End If
        'End If

        'If ddlSellingType.Value <> "[Select]" Then
        '    If Trim(strWhereCond) = "" Then
        '        strWhereCond = " agentmast.sellcode = '" & ddlSellingType.Items(ddlSellingType.SelectedIndex).Text & "'"
        '    Else
        '        strWhereCond = strWhereCond & " AND agentmast.sellcode = '" & ddlSellingType.Items(ddlSellingType.SelectedIndex).Text & "'"
        '    End If
        'End If

        'If ddlCategory.Value <> "[Select]" Then
        '    If Trim(strWhereCond) = "" Then
        '        strWhereCond = " agentmast.catcode = '" & ddlCategory.Items(ddlCategory.SelectedIndex).Text & "'"
        '    Else
        '        strWhereCond = strWhereCond & " AND agentmast.catcode = '" & ddlCategory.Items(ddlCategory.SelectedIndex).Text & "'"
        '    End If
        'End If

        'If ddlCountry.Value <> "[Select]" Then
        '    If Trim(strWhereCond) = "" Then
        '        strWhereCond = " agentmast.ctrycode = '" & ddlCountry.Items(ddlCountry.SelectedIndex).Text & "'"
        '    Else
        '        strWhereCond = strWhereCond & " AND agentmast.ctrycode = '" & ddlCountry.Items(ddlCountry.SelectedIndex).Text & "'"
        '    End If
        'End If

        'If ddlCity.Value <> "[Select]" Then
        '    If Trim(strWhereCond) = "" Then
        '        strWhereCond = " agentmast.citycode = '" & ddlCity.Items(ddlCity.SelectedIndex).Text & "'"
        '    Else
        '        strWhereCond = strWhereCond & " AND agentmast.citycode = '" & ddlCity.Items(ddlCity.SelectedIndex).Text & "'"
        '    End If
        'End If

        'BuildCondition = strWhereCond
    End Function
#End Region

    Protected Sub btnFillList_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ' FillGrid(ddlOrderBy.Value)
    End Sub

    Protected Sub btnSendMail_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSendMail.Click
        Dim txtEmailId As TextBox
        Dim strEmailText As String = ""
        Dim strEmailText_cc As String = ""
        Dim strSubject As String = ""
        Dim strfootertext As String = ""
        Dim to_email As String = ""
        Dim strFromEmailID As String = ""
        Dim strcc As String = ""
        Dim tocc As String = ""
        Dim flg As Boolean = True
        Dim chkApp As CheckBox
        Dim txtusername, txtshortname As TextBox
        Dim lblWPassword As Label
        Dim chkSendMail As CheckBox
        Dim lblagentcode As Label
        Dim txtcontact As TextBox
        Dim lblwebapprove As Label
        Dim lblagentname As Label
        Dim strToCC As String = ""
        Dim strFullpath As String = ""


        Dim strEmailText1 As String = ""
        Dim strEmailText2 As String = ""
        Dim strEmailTextnew As String = ""
        Dim strEmailText_cc1 As String = ""
        Dim strSubject1 As String = ""
        Dim strfootertext1 As String = ""

        Dim flag = True
        Dim cnt As Integer = 0
        Try


            For Each GVRow In grdUploadClients.Rows
                chkSendMail = GVRow.FindControl("chkSendMail")
                If chkSendMail.Checked = True Then
                    cnt = cnt + 1
                End If
            Next

            If cnt > 1 Then
                flag = False
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('You cannot send multiple mail...');", True)
                Exit Sub
            End If




            If flag = True Then

                If ValidateContinue_sendmail() = True Then



                    SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    myCommand = New SqlCommand("Select * from email_text where emailtextfor=0", SqlConn)
                    mySqlReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection)
                    If mySqlReader.HasRows Then
                        If mySqlReader.Read() = True Then
                            If IsDBNull(mySqlReader("emailtext")) = False Then
                                strEmailText_cc1 = CType(mySqlReader("emailtext"), String)
                            End If
                            If IsDBNull(mySqlReader("subject")) = False Then
                                strSubject1 = CType(mySqlReader("subject"), String)
                            End If
                            If IsDBNull(mySqlReader("fromemailid")) = False Then
                                strFromEmailID = CType(mySqlReader("fromemailid"), String)
                            End If
                            If IsDBNull(mySqlReader("footertext")) = False Then
                                strfootertext1 = CType(mySqlReader("footertext"), String)
                            End If

                        End If
                    End If
                    Dim name As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select cotel from columbusmaster")
                    Dim mailid As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select option_selected  from reservation_parameters where param_id=1006") '
                    strToCC = mailid
                    'Email to Agent
                    If strEmailText_cc1 <> "" Then

                        strcc = "Select option_selected from reservation_parameters where param_id =552" ' "Select option_selected from reservation_parameters where param_id =1006"

                        Dim ds1 As DataSet = objUtils.ExecuteQuerySqlnew(Session("dbconnectionName"), strcc)
                        If ds1.Tables.Count > 0 Then
                            If ds1.Tables(0).Rows.Count > 0 Then
                                If IsDBNull(ds1.Tables(0).Rows(0)(0)) = False Then
                                    tocc = CType(ds1.Tables(0).Rows(0)(0), String)
                                End If
                            End If
                        End If
                        For Each GVRow In grdUploadClients.Rows
                            lblwebapprove = GVRow.FindControl("lblwebapprove")
                            chkApp = GVRow.FindControl("chkSendMail")
                            chkSendMail = GVRow.FindControl("chkSendMail")
                            txtEmailId = GVRow.FindControl("txtEmailId")
                            txtusername = GVRow.FindControl("txtwebuser")
                            lblWPassword = GVRow.FindControl("lblWPassword")
                            lblagentcode = GVRow.FindControl("lblagentcode")
                            txtcontact = GVRow.FindControl("txtContactPerson")
                            txtshortname = GVRow.FindControl("txtshortname")
                            lblagentname = GVRow.FindControl("lblagentname")
                            If CType(lblwebapprove.Text, String) = "Approved" Then


                                Dim onlineURL As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select onlineURL from division_master d,agentmast a where a.divcode=d.division_master_code  and a.agentcode='" & lblagentcode.Text & "'")


                                strEmailText1 = "<center> <p class='MsoNormal' style='text-align: left;'><span style='font-family: calibri, sans-serif; color: #0d0d0d; font-size: 12pt;'>Dear " + txtcontact.Text + ",</span><span style='font-family:calibri,sans-serif;color:#0d0d0d'><o:p /></span><br /></p><p class='MsoNormal'><b><span style='font-family:calibri,sans-serif;color:#002060'><o:p /></span></b></p></center>"
                                strEmailText1 = strEmailText1 + "&nbsp;</span></b></p> <div></div>"

                                strEmailText1 = strEmailText1 + strEmailText_cc1


                                ' strEmailText1 = strEmailText1 + vbCrLf + onlineURL


                                'strEmailText1 = strEmailText1 + "<p class='MsoNormal' style='margin: 0'><br /></p> <p class='MsoNormal' style='margin: 0'><b> <span style='font-family:calibri,sans-serif;color:#0d0d0d'>Agency Name  : &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + lblagentname.Text + "</span></b></p>"

                                'strEmailText1 = strEmailText1 + "<p class='MsoNormal' style='margin: 0'><br /></p> <p class='MsoNormal' style='margin: 0'><b> <span style='font-family:calibri,sans-serif;color:#0d0d0d'>Username    : &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + txtusername.Text + "</span></b></p>"
                                'strEmailText1 = strEmailText1 + "<p class='MsoNormal' style='margin: 0'><b><span style='font-family:calibri,sans-serif;color:#0d0d0d'>Password    : &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + lblWPassword.Text + "</span></b></p>"
                                'strEmailText1 = strEmailText1 + "<p class='MsoNormal' style='margin: 0'><b><span style='font-family:calibri,sans-serif;color:#0d0d0d'>Short Name  : &nbsp;&nbsp;&nbsp;&nbsp;" + txtshortname.Text + "</span></b></p> <p class='MsoNormal' style='margin: 0'><font face='Calibri,sans-serif'>&nbsp;</font></p>"


                                strEmailText1 = strEmailText1 + "<table class='MsoNormalTable' border='0' cellspacing='0' cellpadding='0' style='margin-left:30.3pt;border-collapse:collapse;mso-yfti-tbllook:1184; mso-padding-alt:0in 0in 0in 0in' > "
                                strEmailText1 = strEmailText1 + "<tbody><tr style='mso-yfti-irow:0;mso-yfti-firstrow:yes;height:23.45pt'>"
                                strEmailText1 = strEmailText1 + "<td width='121' style='width:90.9pt;border:solid windowtext 1.0pt;background:  #d9d9d9;padding:0in 5.4pt 0in 5.4pt;height:23.45pt'>  "
                                strEmailText1 = strEmailText1 + "<p class='MsoPlainText' align='center' style='text-align:center'>AGENT<o:p /></p>  </td>  "
                                strEmailText1 = strEmailText1 + "<td width='486' style='width:364.55pt;border:solid windowtext 1.0pt;border-left:  none;padding:0in 5.4pt 0in 5.4pt;height:23.45pt'>"
                                strEmailText1 = strEmailText1 + "<p class='MsoPlainText' align='center' style='text-align:center'><b>" + lblagentname.Text + "<o:p /></b></p>  </td> </tr> <tr style='mso-yfti-irow:1;height:23.45pt'>"
                                strEmailText1 = strEmailText1 + "<td width='121' style='width:90.9pt;border:solid windowtext 1.0pt;border-top:  none;background:#d9d9d9;padding:0in 5.4pt 0in 5.4pt;height:23.45pt'>  "
                                strEmailText1 = strEmailText1 + "<p class='MsoPlainText' align='center' style='text-align:center'>URL<o:p /></p>  </td>"
                                strEmailText1 = strEmailText1 + "<td width='486' style='width:364.55pt;border-top:none;border-left:none;  border-bottom:solid windowtext 1.0pt;border-right:solid windowtext 1.0pt;  padding:0in 5.4pt 0in 5.4pt;height:23.45pt'>"
                                strEmailText1 = strEmailText1 + "<p class='MsoPlainText' align='center' style='text-align:center'><b>" + onlineURL + "<o:p /></b></p>  </td> </tr> <tr style='mso-yfti-irow:2;height:23.45pt'>  "
                                strEmailText1 = strEmailText1 + "<td width='121' style='width:90.9pt;border:solid windowtext 1.0pt;border-top:  none;background:#d9d9d9;padding:0in 5.4pt 0in 5.4pt;height:23.45pt'>  "
                                strEmailText1 = strEmailText1 + "<p class='MsoPlainText' align='center' style='text-align:center'>USERNAME<o:p /></p>  </td>  "


                                strEmailText2 = "<td width='486' style='width:364.55pt;border-top:none;border-left:none;  border-bottom:solid windowtext 1.0pt;border-right:solid windowtext 1.0pt;  padding:0in 5.4pt 0in 5.4pt;height:23.45pt'> "
                                strEmailText2 = strEmailText2 + "<p class='MsoPlainText' align='center' style='text-align:center'><b>" + txtusername.Text + "<o:p /></b></p>  </td> </tr> <tr style='mso-yfti-irow:3;height:23.45pt'> "
                                strEmailText2 = strEmailText2 + "<td width='121' style='width:90.9pt;border:solid windowtext 1.0pt;border-top:  none;background:#d9d9d9;padding:0in 5.4pt 0in 5.4pt;height:23.45pt'>"
                                strEmailText2 = strEmailText2 + "<p class='MsoPlainText' align='center' style='text-align:center'>PASSWORD<o:p /></p>  </td>   "
                                strEmailText2 = strEmailText2 + "<td width='486' style='width:364.55pt;border-top:none;border-left:none;  border-bottom:solid windowtext 1.0pt;border-right:solid windowtext 1.0pt;  padding:0in 5.4pt 0in 5.4pt;height:23.45pt'>"
                                strEmailText2 = strEmailText2 + "<p class='MsoPlainText' align='center' style='text-align:center'><b>" + lblWPassword.Text + "<o:p /></b></p>  </td> </tr><tr style='mso-yfti-irow:4;mso-yfti-lastrow:yes;height:23.45pt'>   "
                                strEmailText2 = strEmailText2 + "<td width='121' style='width:90.9pt;border:solid windowtext 1.0pt;border-top:  none;background:#d9d9d9;padding:0in 5.4pt 0in 5.4pt;height:23.45pt'>  "
                                strEmailText2 = strEmailText2 + "<p class='MsoPlainText' align='center' style='text-align:center'>SHORT NAME<o:p /></p>  </td>    "
                                strEmailText2 = strEmailText2 + "<td width='486' style='width:364.55pt;border-top:none;border-left:none;  border-bottom:solid windowtext 1.0pt;border-right:solid windowtext 1.0pt;  padding:0in 5.4pt 0in 5.4pt;height:23.45pt'>"
                                strEmailText2 = strEmailText2 + "<p class='MsoPlainText' align='center' style='text-align:center'><b>" + txtshortname.Text + "<o:p /></b></p>  </td> </tr></tbody></table><br />   "

                                Dim strpswd As String = CType(txtusername.Text, String) 'objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select dbo.pwdencript('" & CType(lblWPassword.Text, String) & "')")

                                strEmailTextnew = strEmailText1 + strEmailText2

                                strEmailTextnew = strEmailTextnew + "<br /> " + strfootertext1


                                If chkSendMail.Checked = True And lblwebapprove.Text = "Approved" Then
                   
                                    to_email = txtEmailId.Text.Trim
                                    'End If

                                    If objEmail.SendEmailCCust(strFromEmailID, to_email, tocc, strSubject1, strEmailTextnew, Server.MapPath("~/images/logo.png")) Then
                                        ' If objEmail.SendCDOMessage(strFullpath, strFromEmailID, to_email, strToCC, strSubject1, strEmailText1) Then
                                        PwdSendmailLog_Entry(CType(lblagentcode.Text, String), to_email + ";" + tocc, "Approve for web- Send mail")
                                        flg = True
                                    Else
                                        mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))          'connection open
                                        mySqlCmd = New SqlCommand("sp_mail_fail", mySqlConn)
                                        mySqlCmd.CommandType = CommandType.StoredProcedure
                                        mySqlCmd.Parameters.Add(New SqlParameter("@agentcode", SqlDbType.VarChar, 20)).Value = CType(lblagentcode.Text, String)
                                        mySqlCmd.Parameters.Add(New SqlParameter("@agentname", SqlDbType.VarChar, 20)).Value = CType(GVRow.Cells(2).Text.Trim, String)
                                        mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                                        mySqlCmd.ExecuteNonQuery()

                                        clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
                                        clsDBConnect.dbConnectionClose(mySqlConn) 'connection close
                                        flg = False




                                    End If
                                    to_email = ""

                                End If
                            End If
                        Next
                    End If
                    If flg = False Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Some mails are fail to Send');", True)
                        SetFocus(btnSendMail)
                    Else
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('E-Mail send successfully.');", True)
                        SetFocus(btnSendMail)
                    End If

                    'FillGridmailfail(ddlOrderBy.Value)

                    'imgicon.Style("visibility") = "hidden"


                    clsDBConnect.dbConnectionClose(SqlConn)           'connection close
                    clsDBConnect.dbCommandClose(myCommand)               'sql command disposed
                    clsDBConnect.dbReaderClose(mySqlReader)             ' sql reader disposed    

                End If
            End If ''flag=true

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ApproveCustomersforWeb.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try

    End Sub

    Protected Sub btnSelectforApprove_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim GVRow As GridViewRow
        Dim chkApp As CheckBox
        For Each GVRow In grdUploadClients.Rows
            chkApp = GVRow.FindControl("chkApprove")
            chkApp.Checked = True
        Next
    End Sub

    Protected Sub btnApprove_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnApprove.Click
        Dim GVRow As GridViewRow
        Dim chkApp As CheckBox
        Dim ddlbookingengformat As DropDownList
        Dim txtEmailId, txtContactPerson As TextBox, txtwebuser, txtshortname As TextBox
        Dim lblwebapprove, lblWPassword, lbldate As Label
        Dim flg As Boolean = False

        Dim result1 As Integer = 0
        Dim frmmode As String = 0
        Dim strPassQry As String = "false"
        Dim strUsrnm As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select option_selected    from reservation_parameters where param_id =1088")
        Dim strPwd As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select option_selected    from reservation_parameters where param_id =1089")

        Try
            If ValidateContinue() = True Then


                For Each GVRow In grdUploadClients.Rows
                    chkApp = GVRow.FindControl("chkApprove")
                    txtEmailId = GVRow.FindControl("txtEmailId")
                    txtContactPerson = GVRow.FindControl("txtContactPerson")
                    txtwebuser = GVRow.FindControl("txtwebuser")
                    txtshortname = GVRow.FindControl("txtshortname")
                    ddlbookingengformat = GVRow.FindControl("ddlbookingengformat")
                    lblwebapprove = GVRow.FindControl("lblwebapprove")
                    lblWPassword = GVRow.FindControl("lblWPassword")
                    lbldate = GVRow.FindControl("lbldate")

                    If chkApp.Checked = True Then
                        SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                        sqlTrans = SqlConn.BeginTransaction           'SQL  Trans start
                        myCommand = New SqlCommand("sp_updateweb_agentmast", SqlConn, sqlTrans)
                        myCommand.CommandType = CommandType.StoredProcedure
                        frmmode = 2
                        myCommand.Parameters.Add(New SqlParameter("@agentcode", SqlDbType.VarChar, 20)).Value = CType(GVRow.Cells(1).Text, String)
                        myCommand.Parameters.Add(New SqlParameter("@webapprove", SqlDbType.Int, 9)).Value = 1
                        myCommand.Parameters.Add(New SqlParameter("@webusername", SqlDbType.VarChar, 20)).Value = CType(txtwebuser.Text, String)
                        myCommand.Parameters.Add(New SqlParameter("@bookingengineratetype", SqlDbType.VarChar, 20)).Value = CType(ddlbookingengformat.SelectedValue, String)
                        myCommand.Parameters.Add(New SqlParameter("@shortname", SqlDbType.VarChar, 20)).Value = CType(txtshortname.Text, String)
                        myCommand.Parameters.Add(New SqlParameter("@webpassword", SqlDbType.VarChar, 10)).Value = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select dbo.pwdencript('" & CType(lblWPassword.Text, String) & "')")
                        myCommand.Parameters.Add(New SqlParameter("@webcontact", SqlDbType.VarChar, 100)).Value = CType(txtContactPerson.Text, String)
                        myCommand.Parameters.Add(New SqlParameter("@webemail", SqlDbType.VarChar, 100)).Value = CType(txtEmailId.Text, String)
                        myCommand.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                        myCommand.Parameters.Add(New SqlParameter("@mode", SqlDbType.Int, 9)).Value = 1

                        myCommand.ExecuteNonQuery()



                        strPassQry = ""

                        If strPassQry = "" Then
                            lblwebapprove.Text = "Approved"
                            'lblWPassword.Text = CType(txtwebuser.Text, String)
                            lbldate.Text = Now.Date
                            sqlTrans.Commit()    'SQl Tarn Commit
                            clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
                            clsDBConnect.dbCommandClose(myCommand)               'sql command disposed
                            clsDBConnect.dbConnectionClose(SqlConn)           'connection close
                            ''Response.Redirect("MarketsSearch.aspx", False)
                            'Dim strscript As String = ""
                            'strscript = "window.opener.__doPostBack('MktWindowPostBack', '');window.opener.focus();window.close();"
                            'ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)

                        End If
                        ''End If


                        flg = True
                    End If

                Next
                If flg = False Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Select atleast one customer for approve.');", True)
                    SetFocus(btnApprove)
                    Exit Sub
                Else
                    Dim strscript As String = ""
                    '   FillGrid(ddlOrderBy.Value)
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Selected customers are successfully approved for web.');", True)
                    strscript = "window.opener.__doPostBack('ApproveCustomersforWeb', '');window.opener.focus();window.close();"
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)
                    SetFocus(btnApprove)
                End If

            End If
            ' imgicon.Style("visibility") = "hidden"

        Catch ex As Exception
            If SqlConn.State = ConnectionState.Open Then
                sqlTrans.Rollback()
                clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
                clsDBConnect.dbCommandClose(myCommand)               'sql command disposed
                clsDBConnect.dbConnectionClose(SqlConn)
            End If
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ApproveCustomersforWeb.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally

        End Try
    End Sub



#Region "Public Function ValidateContinue()"
    Public Function ValidateContinue() As Boolean
        Dim GVRow As GridViewRow
        Dim txtEmailId, txtContactPerson As TextBox, txtwebuser, txtshortname As TextBox
        Dim flg As Boolean = False
        Dim chkApp As CheckBox
        Dim chkSendMail As CheckBox
        Dim ddlbookingengformat As DropDownList
        Try
            For Each GVRow In grdUploadClients.Rows
                chkApp = GVRow.FindControl("chkApprove")
                txtEmailId = GVRow.FindControl("txtEmailId")
                txtContactPerson = GVRow.FindControl("txtContactPerson")
                txtwebuser = GVRow.FindControl("txtwebuser")
                chkSendMail = GVRow.FindControl("chkSendMail")
                txtshortname = GVRow.FindControl("txtshortname")

                ddlbookingengformat = GVRow.FindControl("ddlbookingengformat")


                If chkApp.Checked = True Then


                    If CType(txtshortname.Text, String) = "&nbsp;" Or CType(txtshortname.Text, String) = "" Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Some ShortName is  blank ..');", True)
                        SetFocus(txtshortname)
                        ValidateContinue = False
                        Exit Function
                    End If


                    If CType(txtEmailId.Text, String) = "&nbsp;" Or CType(txtEmailId.Text, String) = "" Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Some Email is  blank ..');", True)
                        SetFocus(btnApprove)
                        ValidateContinue = False
                        Exit Function
                    End If

                    If CType(ddlbookingengformat.Text, String) = "[Select]" Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Some Booking Engine Format is  not Selected ..');", True)
                        SetFocus(txtshortname)
                        ValidateContinue = False
                        Exit Function
                    End If
                    'If CType(txtContactPerson.Text, String) = "&nbsp;" Or CType(txtContactPerson.Text, String) = "" Then
                    '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Contact is  blank ..');", True)
                    '    SetFocus(btnApprove)
                    '    ValidateContinue = False
                    '    Exit Function
                    'End If

                    If CType(txtwebuser.Text, String) = "&nbsp;" Or CType(txtwebuser.Text, String) = "" Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Some WebUser is  blank ..');", True)
                        SetFocus(btnApprove)
                        ValidateContinue = False
                        Exit Function
                    End If

                    'If CType(GVRow.Cells(11).Text, String) = "&nbsp;" Or CType(GVRow.Cells(11).Text, String) = "" Then
                    '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Some Password is  blank ..');", True)
                    '    SetFocus(btnApprove)
                    '    ValidateContinue = False
                    '    Exit Function
                    'End If

                    If CType(GVRow.Cells(15).Text, String) <> "No" Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" + GVRow.Cells(2).Text + "'+ ' ' + 'Locked Customer  can not  be  approved .');", True)
                        ValidateContinue = False
                        Exit Function
                    End If


                End If

            Next
            ValidateContinue = True
        Catch ex As Exception
            ValidateContinue = False
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ApproveCustomersforWeb.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally

        End Try

    End Function
#End Region


    Private Function BuildConditionNew(ByVal strCustomerGroupValue As String, ByVal strCountryGroupValue As String, ByVal strCustomerValue As String, ByVal strCountryValue As String, ByVal strCityValue As String, ByVal strCategoryValue As String, ByVal strsectorvalue As String, ByVal strTextValue As String) As String
        strWhereCond = ""
        If strCountryValue.Trim <> "" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = "upper(ctrymast.ctryname) IN (" & Trim(strCountryValue.Trim.ToUpper) & ")"
            Else
                strWhereCond = strWhereCond & " AND upper(ctrymast.ctryname) IN (" & Trim(strCountryValue.Trim.ToUpper) & ")"
            End If
        End If
        If strCustomerGroupValue <> "" Then

            If Trim(strWhereCond) = "" Then
                strWhereCond = " agentmast.agentcode in (select customergroup_detail.agentcode   from customergroup ,customergroup_detail where customergroup.customergroupcode = customergroup_detail.customergroupcode and customergroupname in (" & Trim(strCustomerGroupValue.Trim.ToUpper) & "))"
            Else

                strWhereCond = strWhereCond & " AND agentmast.agentcode in (select customergroup_detail.agentcode  from customergroup ,customergroup_detail where customergroup.customergroupcode = customergroup_detail.customergroupcode and customergroupname in  (" & Trim(strCustomerGroupValue.Trim.ToUpper) & "))"
            End If
        End If
        If strCountryGroupValue <> "" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = " ctrymast.ctrycode in (select countrygroup_detail.ctrycode   from countrygroup ,countrygroup_detail where countrygroup.countrygroupcode = countrygroup_detail.countrygroupcode and countrygroupname in (" & Trim(strCountryGroupValue.Trim.ToUpper) & "))"
            Else
                strWhereCond = strWhereCond & "  AND ctrymast.ctrycode in (select countrygroup_detail.ctrycode    from countrygroup ,countrygroup_detail where countrygroup.countrygroupcode = countrygroup_detail.countrygroupcode and countrygroupname in (" & Trim(strCountryGroupValue.Trim.ToUpper) & "))"
            End If
        End If

        If strCustomerValue <> "" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = " upper(agentmast.agentname) in (" & Trim(strCustomerValue) & ")"
            Else
                strWhereCond = strWhereCond & " and  upper(agentmast.agentname) in (" & Trim(strCustomerValue) & ")"
            End If
        End If
        If strCityValue.Trim <> "" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = "upper(citymast. cityname) IN (" & Trim(strCityValue.Trim.ToUpper) & ")"
            Else
                strWhereCond = strWhereCond & " AND upper(citymast.cityname) IN (" & Trim(strCityValue.Trim.ToUpper) & ")"
            End If
        End If
        If strCategoryValue.Trim <> "" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = "upper(agentcatmast. agentcatname) IN (" & Trim(strCategoryValue.Trim.ToUpper) & ")"
            Else
                strWhereCond = strWhereCond & " AND upper(agentcatmast.agentcatname) IN (" & Trim(strCategoryValue.Trim.ToUpper) & ")"
            End If
        End If
        If strsectorvalue.Trim <> "" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = "upper(agent_sectormaster. sectorname) IN (" & Trim(strsectorvalue.Trim.ToUpper) & ")"
            Else
                strWhereCond = strWhereCond & " AND upper(agent_sectormaster.sectorname) IN (" & Trim(strsectorvalue.Trim.ToUpper) & ")"
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
                        strWhereCond1 = "upper(agentmast.agentname) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' or upper(agentmast.agentcode) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' or upper(agentmast.webusername) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' or upper(agentmast.webcontact) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%'  or upper(agentmast.webemail) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' "
                    Else

                        strWhereCond1 = strWhereCond1 & " OR upper(agentmast.agentname) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' or upper(agentmast.agentcode) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' or upper(agentmast.webusername) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' or upper(agentmast.webcontact) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%'  or upper(agentmast.webemail) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' "
                    End If
                End If
            Next
            If Trim(strWhereCond) = "" Then
                strWhereCond = "(" & strWhereCond1 & ")"
            Else
                strWhereCond = strWhereCond & " AND (" & strWhereCond1 & ")"
            End If

        End If
        'If txtFromDate.Text.Trim <> "" And txtToDate.Text <> "" Then

        '    If ddlOrder.SelectedValue = "C" Then
        '        If Trim(strWhereCond) = "" Then

        '            strWhereCond = " (CONVERT(datetime, convert(varchar(10),partymast.adddate,103),103)  between CONVERT(datetime, '" + txtFromDate.Text + "',103) and CONVERT(datetime,  '" + txtToDate.Text + "',103)) "
        '        Else
        '            strWhereCond = strWhereCond & "  and (CONVERT(datetime, convert(varchar(10),partymast.adddate,103),103)  between CONVERT(datetime, '" + txtFromDate.Text + "',103) and CONVERT(datetime,  '" + txtToDate.Text + "',103)) "
        '        End If
        '    ElseIf ddlOrder.SelectedValue = "M" Then
        '        If Trim(strWhereCond) = "" Then

        '            strWhereCond = " (CONVERT(datetime, convert(varchar(10),partymast.moddate,103),103)  between CONVERT(datetime, '" + txtFromDate.Text + "',103) and CONVERT(datetime,  '" + txtToDate.Text + "',103)) "
        '        Else
        '            strWhereCond = strWhereCond & "  and (CONVERT(datetime, convert(varchar(10),partymast.moddate,103),103)  between CONVERT(datetime, '" + txtFromDate.Text + "',103) and CONVERT(datetime,  '" + txtToDate.Text + "',103)) "
        '        End If
        '    End If
        'End If


        BuildConditionNew = strWhereCond
    End Function

    Protected Sub btnResetSelection_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnResetSelection.Click
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
    Protected Sub btnvsprocess_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnvsprocess.Click

        Try
            FilterGrid()
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("BankTypesSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try



    End Sub
    Private Sub FillGridNew()
        Dim dtt As DataTable
        Dim ii As Integer
        ' Dim strRowsPerPage = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id='2006'")
        dtt = Session("sDtDynamic")
        Dim strCountryValue As String = ""
        Dim strCityValue As String = ""
        Dim strCategoryValue As String = ""
        Dim strTextValue As String = ""
        Dim strBindCondition As String = ""
        Dim strCustomerGroupValue As String = ""
        Dim strCustomerValue As String = ""
        Dim strCountryGroupValue As String = ""
        Dim strSectorValue As String = ""

        Try
            If dtt.Rows.Count > 0 Then
                For i As Integer = 0 To dtt.Rows.Count - 1
                    If dtt.Rows(i)("Code").ToString = "COUNTRY" Then
                        If strCountryValue <> "" Then
                            strCountryValue = strCountryValue + ",'" + dtt.Rows(i)("Value").ToString + "'"
                        Else
                            strCountryValue = "'" + dtt.Rows(i)("Value").ToString + "'"
                        End If
                    End If
                    If dtt.Rows(i)("Code").ToString = "CITY" Then
                        If strCityValue <> "" Then
                            strCityValue = strCityValue + ",'" + dtt.Rows(i)("Value").ToString + "'"
                        Else
                            strCityValue = "'" + dtt.Rows(i)("Value").ToString + "'"
                        End If
                    End If
                    If dtt.Rows(i)("Code").ToString = "CUSTOMERGROUP" Then
                        If strCustomerGroupValue <> "" Then
                            strCustomerGroupValue = strCustomerGroupValue + ",'" + dtt.Rows(i)("Value").ToString + "'"
                        Else
                            strCustomerGroupValue = "'" + dtt.Rows(i)("Value").ToString + "'"
                        End If
                    End If
                    If dtt.Rows(i)("Code").ToString = "CUSTOMER" Then
                        If strCustomerValue <> "" Then
                            strCustomerValue = strCustomerValue + ",'" + dtt.Rows(i)("Value").ToString + "'"
                        Else
                            strCustomerValue = "'" + dtt.Rows(i)("Value").ToString + "'"
                        End If
                    End If
                    If dtt.Rows(i)("Code").ToString = "COUNTRYGROUP" Then
                        If strCountryGroupValue <> "" Then
                            strCountryGroupValue = strCountryGroupValue + ",'" + dtt.Rows(i)("Value").ToString + "'"
                        Else
                            strCountryGroupValue = "'" + dtt.Rows(i)("Value").ToString + "'"
                        End If
                    End If
                    If dtt.Rows(i)("Code").ToString = "CATEGORY" Then
                        If strCategoryValue <> "" Then
                            strCategoryValue = strCategoryValue + ",'" + dtt.Rows(i)("Value").ToString + "'"
                        Else
                            strCategoryValue = "'" + dtt.Rows(i)("Value").ToString + "'"
                        End If
                    End If
                    If dtt.Rows(i)("Code").ToString = "SECTOR" Then
                        If strSectorValue <> "" Then
                            strSectorValue = strSectorValue + ",'" + dtt.Rows(i)("Value").ToString + "'"
                        Else
                            strSectorValue = "'" + dtt.Rows(i)("Value").ToString + "'"
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

            strBindCondition = BuildConditionNew(strCustomerGroupValue, strCountryGroupValue, strCustomerValue, strCountryValue, strCityValue, strCategoryValue, strSectorValue, strTextValue)
            Dim myDS As New DataSet
            Dim strValue As String
            grdUploadClients.Visible = True
            lblMsg.Visible = False
            If grdUploadClients.PageIndex < 0 Then
                grdUploadClients.PageIndex = 0
            End If
            strSqlQry = ""
            Dim strorderby As String = Session("strAgentsortExpression")
            Dim strsortorder As String = "ASC"
            grdUploadClients.DataSource = Nothing

            If strBindCondition <> "" Then
                '  strSqlQry = "SELECT *,[IsActive]=case when active=1 then 'Active' when active=0 then 'InActive'end  FROM plgrpmast"dbo.pwddecript(webpassword) webpassword 
                strSqlQry = " SELECT agentmast.agentcode,agentname,webusername=case when isnull(webcontact,'')='' then isnull(contact1,'') else webusername  end,agentmast.shortname,AGENTMAST.bookingengineratetype, " _
                    & " webemail=case when isnull(webemail,'')='' then isnull(email,'') else webemail  end,webcontact=case when isnull(webcontact,'')='' then isnull(contact1,'') else webcontact  end," & _
    "[webapprove]=case isnull(webapprove,0) when 1 then 'Approved' else '' end,dbo.pwddecript(webpassword) webpassword   ," & _
     " case when isnull(agents_locked.agentcode,'')='' then 'No' else 'Yes' end  locked_status, isnull(dbo. fn_get_customergroup(agentmast.agentcode),'')Customergroup," & _
     " isnull(dbo.fn_get_countrygroup(ctrymast.ctrycode),'')countrygroups ,appuser,appdate FROM agentmast  " & _
     "  inner join   ctrymast on agentmast.ctrycode=ctrymast.ctrycode  inner join citymast on agentmast.citycode= citymast.citycode " & _
      " inner join agentcatmast on agentmast.catcode=agentcatmast.agentcatcode left outer join agents_locked" & _
       " on agentmast.agentcode =agents_locked.agentcode left outer join agent_sectormaster on " & _
     " agentmast.sectorcode=agent_sectormaster.sectorcode where agentmast.active = 1 "

                strSqlQry = strSqlQry & " and " & strBindCondition & " ORDER BY " & strorderby & " " & strsortorder
                'Else
                '    strSqlQry = strSqlQry & " ORDER BY " & strorderby & " " & strsortorder

                SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
                myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
                myDataAdapter.Fill(myDS)

                'Session("SSqlQuery") = strSqlQry
                'myDS = clsUtils.GetDetailsPageWise(1, 10, strSqlQry)
                If myDS.Tables(0).Rows.Count > 0 Then
                    grdUploadClients.DataSource = myDS.Tables(0)
                    grdUploadClients.DataBind()
                    Dim GVRow As GridViewRow
                    'Dim chkApp As CheckBox
                    Dim lblPwd As Label
                    For Each GVRow In grdUploadClients.Rows
                        'If GVRow.Cells(7).Text.Trim = "1" Then
                        '    chkApp = GVRow.FindControl("chkApprove")
                        '    chkApp.Checked = True
                        'End If
                        lblPwd = GVRow.FindControl("lblWPassword")
                        If lblPwd.Text = "" Then
                            'Dim str As String
                            'SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                            'sqlTrans = SqlConn.BeginTransaction           'SQL  Trans start

                            myCommand = New SqlCommand("GenerateRandomString", SqlConn)
                            myCommand.CommandType = CommandType.StoredProcedure

                            myCommand.Parameters.Add(New SqlParameter("@useNumbers", SqlDbType.Bit)).Value = 1
                            myCommand.Parameters.Add(New SqlParameter("@useLowerCase", SqlDbType.Bit)).Value = 0
                            myCommand.Parameters.Add(New SqlParameter("@useUpperCase", SqlDbType.Bit)).Value = 1
                            myCommand.Parameters.Add(New SqlParameter("@charactersToUse", SqlDbType.VarChar, 100)).Value = System.DBNull.Value
                            myCommand.Parameters.Add(New SqlParameter("@passwordLength", SqlDbType.SmallInt, 9)).Value = 7

                            Dim param As SqlParameter
                            param = New SqlParameter
                            param.ParameterName = "@password"
                            param.Direction = ParameterDirection.Output
                            param.DbType = DbType.String
                            param.Size = 50
                            myCommand.Parameters.Add(param)
                            myDataAdapter = New SqlDataAdapter(myCommand)
                            myCommand.ExecuteNonQuery()
                            lblPwd.Text = param.Value

                            ''       If IsDBNull(myDS.Tables(0).Rows(i)("webapprove")) = False Then
                            If myDS.Tables(0).Rows(ii)("webapprove") <> "Approved" Then
                                GVRow.Cells(13).Text = param.Value
                                'GVRow.Cells(12).Text = param.Value
                            End If
                            ''End If

                            'sqlTrans.Commit()    'SQl Tarn Commit
                            'clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
                            clsDBConnect.dbCommandClose(myCommand)               'sql command disposed
                            clsDBConnect.dbConnectionClose(SqlConn)           'connection close
                        Else
                            'GVRow.Cells(12).Text = myDS.Tables(0).Rows(ii).Item("webpassword").ToString
                        End If

                        ii = ii + 1
                    Next

                    Dim chksel As CheckBox
                    Dim ddlbookingengformat As DropDownList
                    Dim lblwpassword As Label
                    For i = 0 To myDS.Tables(0).Rows.Count - 1
                        For Each GVRow In grdUploadClients.Rows
                            If myDS.Tables(0).Rows(i)("agentcode").ToString = GVRow.Cells(1).Text.ToString Then
                                chksel = GVRow.FindControl("chkApprove")
                                ddlbookingengformat = GVRow.FindControl("ddlbookingengformat")
                                If myDS.Tables(0).Rows(i).Item("bookingengineratetype").ToString = "INDIVIDUAL" Then

                                    ddlbookingengformat.SelectedIndex = 1
                                ElseIf myDS.Tables(0).Rows(i).Item("bookingengineratetype").ToString = "CUMULATIVE" Then
                                    ddlbookingengformat.SelectedIndex = 2
                                End If

                                If myDS.Tables(0).Rows(i).Item("webapprove").ToString = "Approved" Then
                                    chksel.Checked = True
                                    Exit For
                                Else
                                    chksel.Checked = False
                                    Exit For
                                End If
                            End If
                        Next
                    Next
                Else
                    lblMsg.Visible = True
                    lblMsg.Text = "Records not found, Please redefine search criteria."

                End If
            Else
                grdUploadClients.DataBind()
                lblMsg.Visible = False
                lblMsg.Text = "Records not found, Please redefine search criteria."

            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ApproveCustomersForWebSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            'clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
            'clsDBConnect.dbConnectionClose(SqlConn)                          'Close connection
        End Try

    End Sub
#Region "Public Function ValidateContinue_sendmail()"
    Public Function ValidateContinue_sendmail() As Boolean
        Dim GVRow As GridViewRow
        Dim txtEmailId, txtContactPerson As TextBox, txtwebuser As TextBox
        Dim flg As Boolean = True
        Dim chkApp As CheckBox
        Dim chkSendMail As CheckBox
        Dim lblwpassword As Label
        ValidateContinue_sendmail = True
        Try
            For Each GVRow In grdUploadClients.Rows
                chkApp = GVRow.FindControl("chkApprove")
                txtEmailId = GVRow.FindControl("txtEmailId")
                txtContactPerson = GVRow.FindControl("txtContactPerson")
                txtwebuser = GVRow.FindControl("txtwebuser")
                chkSendMail = GVRow.FindControl("chkSendMail")
                lblwpassword = GVRow.FindControl("lblWPassword")
                If chkSendMail.Checked = True Then

                    If CType(txtEmailId.Text, String) = "&nbsp;" Or CType(txtEmailId.Text, String) = "" Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Some Email is  blank ..');", True)
                        SetFocus(btnApprove)
                        ValidateContinue_sendmail = False
                        Exit Function
                    End If

                    If CType(txtContactPerson.Text, String) = "&nbsp;" Or CType(txtContactPerson.Text, String) = "" Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Some Contact is  blank ..');", True)
                        SetFocus(btnApprove)
                        ValidateContinue_sendmail = False
                        Exit Function
                    End If

                    If CType(txtwebuser.Text, String) = "&nbsp;" Or CType(txtwebuser.Text, String) = "" Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" + GVRow.Cells(2).Text + "'+' '+Some WebUser is  blank ..');", True)
                        SetFocus(btnApprove)
                        ValidateContinue_sendmail = False
                        Exit Function
                    End If

                    'If CType(lblwpassword.Text, String) = "&nbsp;" Or CType(lblwpassword.Text, String) = "" Then
                    '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" + GVRow.Cells(2).Text + "'+''+Some Password is  blank ..');", True)
                    '    SetFocus(btnApprove)
                    '    ValidateContinue_sendmail = False
                    '    Exit Function
                    'End If
                    '  If CType(GVRow.Cells(13).Text, String) = "&nbsp;" Or CType(GVRow.Cells(11).Text, String) = "" Then
                    If CType(lblwpassword.Text, String) = "&nbsp;" Or CType(lblwpassword.Text, String) = "" Then

                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" + GVRow.Cells(2).Text + "'+''+Some Password is  blank ...');", True)
                        SetFocus(btnApprove)
                        ValidateContinue_sendmail = False
                        Exit Function
                    End If
                    If CType(GVRow.Cells(15).Text, String) <> "No" Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" + GVRow.Cells(2).Text + "'+ ' ' + 'Locked Customer  can not  Send Mail .');", True)
                        ValidateContinue_sendmail = False
                        Exit Function
                    End If


                End If

            Next
        Catch ex As Exception
            If SqlConn.State = ConnectionState.Open Then
                sqlTrans.Rollback()
            End If
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ApproveCustomersforWeb.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally

        End Try

    End Function
#End Region

#Region "Public Function ValidateContinue_sendmailcontract()"
    Public Function ValidateContinue_sendmailcontract() As Boolean
        Dim GVRow As GridViewRow
        Dim txtEmailId, txtContactPerson As TextBox, txtwebuser As TextBox
        Dim flg As Boolean = False
        Dim lblwpassword As Label
        Dim chkApp As CheckBox
        Dim chkSendMailcontract As CheckBox
        ValidateContinue_sendmailcontract = True
        Try
            For Each GVRow In grdUploadClients.Rows
                chkApp = GVRow.FindControl("chkApprove")
                txtEmailId = GVRow.FindControl("txtEmailId")
                txtContactPerson = GVRow.FindControl("txtContactPerson")
                txtwebuser = GVRow.FindControl("txtwebuser")
                chkSendMailcontract = GVRow.FindControl("chkSendMailcontract")
                lblwpassword = GVRow.FindControl("lblWpassword")
                If chkSendMailcontract.Checked = True Then



                    If CType(txtEmailId.Text, String) = "&nbsp;" Or CType(txtEmailId.Text, String) = "" Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Some Email is  blank ..');", True)
                        SetFocus(btnApprove)
                        ValidateContinue_sendmailcontract = False
                        Exit Function
                    End If

                    If CType(txtContactPerson.Text, String) = "&nbsp;" Or CType(txtContactPerson.Text, String) = "" Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Some Contact is  blank ..');", True)
                        SetFocus(btnApprove)
                        ValidateContinue_sendmailcontract = False
                        Exit Function
                    End If

                    If CType(txtwebuser.Text, String) = "&nbsp;" Or CType(txtwebuser.Text, String) = "" Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Some WebUser is  blank ..');", True)
                        SetFocus(btnApprove)
                        ValidateContinue_sendmailcontract = False
                        Exit Function
                    End If

                    'If CType(lblwpassword.Text, String) = "&nbsp;" Or CType(lblwpassword.Text, String) = "" Then
                    '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" + GVRow.Cells(2).Text + "'+''+'Some Password is  blank ..');", True)
                    '    SetFocus(btnApprove)
                    '    ValidateContinue_sendmailcontract = False
                    '    Exit Function
                    'End If

                    ' If CType(GVRow.Cells(11).Text, String) = "&nbsp;" Or CType(GVRow.Cells(11).Text, String) = "" Then
                    If CType(lblwpassword.Text, String) = "&nbsp;" Or CType(lblwpassword.Text, String) = "" Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" + GVRow.Cells(2).Text + "'+''+'Some Password is  blank ..');", True)
                        SetFocus(btnApprove)
                        ValidateContinue_sendmailcontract = False
                        Exit Function
                    End If

                    If CType(GVRow.Cells(15).Text, String) <> "No" Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" + GVRow.Cells(2).Text + "'+ ' ' + 'Locked Customer  can not  Send Mail .');", True)
                        ValidateContinue_sendmailcontract = False
                        Exit Function
                    End If


                End If

            Next
        Catch ex As Exception
            If SqlConn.State = ConnectionState.Open Then
                sqlTrans.Rollback()
            End If
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ApproveCustomersforWeb.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally

        End Try

    End Function
#End Region


    Protected Sub btnSelectforRemove_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSelectforRemove.Click
        Dim GVRow As GridViewRow
        Dim chkRem As CheckBox
        For Each GVRow In grdUploadClients.Rows
            chkRem = GVRow.FindControl("chkRemove")
            chkRem.Checked = True
        Next
    End Sub

    Protected Sub btnDeletefromWeb_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim GVRow As GridViewRow
        Dim chkRem, chkApprove As CheckBox
        Dim txtEmailId, txtContactPerson, txtshortname As TextBox, txtwebuser As TextBox
        Dim lblWPassword, lblwebapprove As Label
        Dim ddlbookingengformat As DropDownList
        Dim flg As Boolean = False


        Dim result1 As Integer = 0
        Dim frmmode As String = 0
        Dim strPassQry As String = "false"
        Dim strUsrnm As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select option_selected    from reservation_parameters where param_id =1088")
        Dim strPwd As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select option_selected    from reservation_parameters where param_id =1089")



        Try
            For Each GVRow In grdUploadClients.Rows
                chkRem = GVRow.FindControl("chkRemove")
                txtEmailId = GVRow.FindControl("txtEmailId")
                txtContactPerson = GVRow.FindControl("txtContactPerson")
                txtshortname = GVRow.FindControl("txtshortname")
                txtwebuser = GVRow.FindControl("txtwebuser")
                lblWPassword = GVRow.FindControl("lblWPassword")
                lblwebapprove = GVRow.FindControl("lblwebapprove")
                chkApprove = GVRow.FindControl("chkApprove")
                ddlbookingengformat = GVRow.FindControl("ddlbookingengformat")
                If chkRem.Checked = True Then
                    If CType(txtEmailId.Text, String) = "&nbsp;" Or CType(txtContactPerson.Text, String) = "&nbsp;" Or CType(txtwebuser.Text, String) = "&nbsp;" Or CType(lblWPassword.Text, String) = "&nbsp;" Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Contact name, email-id and password should not be blank for remove customer.');", True)
                        SetFocus(btnApprove)
                        Exit Sub
                    End If
                    'If CType(lblWPassword.Text, String) = "&nbsp;" Or CType(lblWPassword.Text, String) = "" Or CType(lblwebapprove.Text, String) = "&nbsp;" Or CType(lblwebapprove.Text, String) = "" Then
                    '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Customer is Not Approved For Web.');", True)
                    '    chkRem.Checked = False
                    '    SetFocus(btnApprove)
                    '    Exit Sub
                    'End If
                    ' If CType(GVRow.Cells(13).Text, String) = "&nbsp;" Or CType(GVRow.Cells(13).Text, String) = "" Or CType(GVRow.Cells(13).Text, String) = "&nbsp;" Or CType(GVRow.Cells(13).Text, String) = "" Then
                    If CType(lblWPassword.Text, String) = "&nbsp;" Or CType(lblWPassword.Text, String) = "" Or CType(lblwebapprove.Text, String) = "&nbsp;" Or CType(lblwebapprove.Text, String) = "" Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Customer is Not Approved For Web.');", True)
                        chkRem.Checked = False
                        SetFocus(btnApprove)
                        Exit Sub
                    End If

                    If CType(ddlbookingengformat.Text, String) = "[Select]" Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Booking Engine Format should  be Selected for remove customer.');", True)
                        chkRem.Checked = False
                        SetFocus(btnApprove)
                        Exit Sub
                    End If


                    If CType(GVRow.Cells(15).Text, String) <> "No" Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Locked Customer  can not  be  approved .');", True)
                        Exit Sub
                    End If
                    SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = SqlConn.BeginTransaction           'SQL  Trans start

                    myCommand = New SqlCommand("sp_updateweb_agentmast", SqlConn, sqlTrans)
                    myCommand.CommandType = CommandType.StoredProcedure
                    frmmode = 2
                    myCommand.Parameters.Add(New SqlParameter("@agentcode", SqlDbType.VarChar, 20)).Value = CType(GVRow.Cells(1).Text, String)
                    myCommand.Parameters.Add(New SqlParameter("@webapprove", SqlDbType.Int, 9)).Value = 0
                    myCommand.Parameters.Add(New SqlParameter("@webusername", SqlDbType.VarChar, 20)).Value = CType(txtwebuser.Text, String)
                    
                    myCommand.Parameters.Add(New SqlParameter("@bookingengineratetype", SqlDbType.VarChar, 20)).Value = CType(ddlbookingengformat.SelectedValue, String)
                    myCommand.Parameters.Add(New SqlParameter("@shortname", SqlDbType.VarChar, 20)).Value = CType(txtshortname.Text, String)
                    myCommand.Parameters.Add(New SqlParameter("@webpassword", SqlDbType.VarChar, 10)).Value = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select dbo.pwdencript('" & CType(lblWPassword.Text, String) & "')")
                    myCommand.Parameters.Add(New SqlParameter("@webcontact", SqlDbType.VarChar, 100)).Value = CType(txtContactPerson.Text, String)
                    myCommand.Parameters.Add(New SqlParameter("@webemail", SqlDbType.VarChar, 100)).Value = CType(txtEmailId.Text, String)
                    myCommand.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                    myCommand.Parameters.Add(New SqlParameter("@mode", SqlDbType.Int, 9)).Value = 2
                    myCommand.ExecuteNonQuery()

                    strPassQry = ""

                    ' result1 = strPassQry

                    If strPassQry = "" Then
                        lblwebapprove.Text = ""
                        '  lblWPassword.Text = ""
                        chkRem.Checked = False
                        chkApprove.Checked = False
                        sqlTrans.Commit()    'SQl Tarn Commit
                        clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
                        clsDBConnect.dbCommandClose(myCommand)               'sql command disposed
                        clsDBConnect.dbConnectionClose(SqlConn)           'connection close
                        ''Response.Redirect("MarketsSearch.aspx", False)
                        'Dim strscript As String = ""
                        'strscript = "window.opener.__doPostBack('MktWindowPostBack', '');window.opener.focus();window.close();"
                        'ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)

                    End If



                    'sqlTrans.Commit()    'SQl Tarn Commit
                    'clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
                    'clsDBConnect.dbCommandClose(myCommand)               'sql command disposed
                    'clsDBConnect.dbConnectionClose(SqlConn)           'connection close
                    flg = True
                End If
            Next
            If flg = False Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Select atleast one customer for remove.');", True)
                SetFocus(btnDeletefromWeb)
                Exit Sub
            Else
                'FillGrid(ddlOrderBy.Value)
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Selected customers are successfully removed from web.');", True)
                SetFocus(btnDeletefromWeb)
            End If
        Catch ex As Exception

            If SqlConn.State = ConnectionState.Open Then
                sqlTrans.Rollback()
                clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
                clsDBConnect.dbCommandClose(myCommand)               'sql command disposed
                clsDBConnect.dbConnectionClose(SqlConn)           'connection close
            End If
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ApproveCustomersforWeb.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            'clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
            'clsDBConnect.dbConnectionClose(SqlConn)                          'Close connection

        End Try

    End Sub

    Protected Sub btnExit_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Response.Redirect("~/MainPage.aspx", False)
    End Sub

    '#Region "Public Sub SortGridColoumn_click()"
    '    Public Sub SortGridColoumn_click()
    '        Dim DataTable As DataTable
    '        Dim myDS As New DataSet
    '        FillGrid(Session("strAgentsortExpression"), "")

    '        myDS = grdUploadClients.DataSource
    '        DataTable = myDS.Tables(0)
    '        If IsDBNull(DataTable) = False Then
    '            Dim dataView As DataView = DataTable.DefaultView
    '            Session.Add("strAgentsortdirection", objUtils.SwapSortDirection(Session("strAgentsortdirection")))
    '            dataView.Sort = Session("strAgentsortExpression") & " " & objUtils.ConvertSortDirectionToSql(Session("strAgentsortdirection"))
    '            grdUploadClients.DataSource = dataView
    '            grdUploadClients.DataBind()
    '        End If
    '    End Sub
    '#End Region

    Protected Sub grdUploadClients_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles grdUploadClients.Sorting
        '  Session.Add("strAgentsortExpression", e.SortExpression)
        '  SortGridColoumn_click()
    End Sub

    Protected Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        '    FillGrid(ddlOrderBy.Value)
    End Sub

    Protected Sub btnGo_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ' FillGrid(ddlOrderBy.Value)
    End Sub

    Protected Sub BtnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrint.Click


        Try

            Dim strReportTitle As String = ""
            Dim strSelectionFormula As String = ""
            Dim strpop As String = ""



            ' strpop = "window.open('rptReportNew.aspx?Pageame=Sectorgroup&BackPageName=SectorGroupSearch.aspx&othtypcode=""&othtypname=""&CtryCode=""','RepSector','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"


            strpop = "window.open('rptReportNew.aspx?Pageame=ApproveCustomersforWeb&BackPageName=ApproveCustomersforWeb.aspx','ApprovedCust');"
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)


        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ApproveCustomersforWeb.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub


    Protected Sub btnHelp_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=ApproveCustomersforWeb','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub

#Region "Public Function FormatDate()"
    Public Function FormatDate(ByVal obj As Object) As String
        If Not (obj Is Nothing) Then
            If (obj.ToString() = "") = False Then
                Return CType(obj.ToString(), Date).ToShortDateString()
            End If
        Else
            Return ""
        End If
    End Function
#End Region

    Protected Sub Btnmailcontract_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Btnmailcontract.Click
        Dim txtEmailId As TextBox
        Dim strEmailText As String = ""
        Dim strEmailText_cc As String = ""
        Dim strSubject As String = ""
        Dim strfootertext As String = ""
        Dim to_email As String = ""
        Dim strFromEmailID As String = ""
        Dim strcc As String = ""
        Dim tocc As String = ""
        Dim flg As Boolean = True
        Dim lblwebapprove As Label
        Dim GVRow As GridViewRow
        Dim chkApp As CheckBox
        Dim txtusername As TextBox
        Dim lblWPassword As Label
        Dim chkSendMailcontract As CheckBox
        Dim lblagentcode As Label
        Dim attachment As String = ""
        Dim cmd3 As SqlCommand
        Dim strEmailText1 As String = ""
        Dim strEmailText_cc1 As String = ""
        Dim strSubject1 As String = ""
        Dim strfootertext1 As String = ""
        Dim txtContactPerson As TextBox

        Dim flag = True
        Dim cnt As Integer = 0
        Try


            For Each GVRow In grdUploadClients.Rows
                chkSendMailcontract = GVRow.FindControl("chkSendMailcontract")
                If chkSendMailcontract.Checked = True Then
                    cnt = cnt + 1
                End If
            Next

            If cnt > 1 Then
                flag = False
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('You cannot send multiple mail...');", True)
                Exit Sub
            End If

            cnt = 0
            For Each GVRow In grdUploadClients.Rows
                chkSendMailcontract = GVRow.FindControl("chkSendMailcontract")
                If chkSendMailcontract.Checked = True Then
                    cnt = cnt + 1
                    Exit For
                End If
            Next

            If cnt = 0 Then
                flag = False
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please select Send Mail Contract ');", True)
                Exit Sub
            End If




            If flag = True Then

                If ValidateContinue_sendmailcontract() = False Then
                    Exit Sub
                End If



                Dim scon3 As New SqlConnection(ConfigurationManager.ConnectionStrings(Session("dbconnectionName")).ConnectionString)
                If scon3.State = Data.ConnectionState.Closed Then
                    scon3.Open()
                End If

                cmd3 = New SqlCommand("Select param_id,option_selected from reservation_parameters where param_id in (547)   ", scon3)
                mySqlReader = cmd3.ExecuteReader(CommandBehavior.CloseConnection)
                cmd3 = Nothing


                If Not mySqlReader.HasRows Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please enter a valid username.');", True)
                    Exit Sub
                End If

                While mySqlReader.Read()
                    If mySqlReader.Item("param_id") = 547 Then
                        If mySqlReader.Item("option_selected") = "" Then
                            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Email address unavailable.');", True)
                            Exit Sub
                        Else
                            tocc = mySqlReader.Item("option_selected")
                        End If
                    End If

                End While
                scon3.Close()



                mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))          'connection open
                'sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start
                mySqlCmd = New SqlCommand("select * from email_text where emailtextfor=1", mySqlConn)
                mySqlCmd.CommandType = CommandType.Text
                Dim sdr As SqlDataReader = mySqlCmd.ExecuteReader



                While sdr.Read()
                    strEmailText_cc1 = sdr.Item(0).ToString
                    strSubject1 = sdr.Item(1).ToString
                    strfootertext1 = sdr.Item(2).ToString
                    strFromEmailID = sdr.Item(3).ToString
                End While
                sdr.Close()
                'sqlTrans.Commit()    'SQl Tarn Commit
                'clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
                clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
                clsDBConnect.dbConnectionClose(mySqlConn)




                Dim ds As New DataSet
                ds = objUtils.ExecuteQuerySqlnew(Session("dbconnectionName"), "select ImageUrl from agentcontract")
                If ds.Tables.Count > 0 Then
                    If ds.Tables(0).Rows.Count > 0 Then
                        attachment = Server.MapPath(ds.Tables(0).Rows(0)("ImageUrl"))
                    Else
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Attachment not exists.');", True)
                        Exit Sub
                    End If
                End If
                For Each GVRow In grdUploadClients.Rows

                    chkApp = GVRow.FindControl("chkSendMail")
                    chkSendMailcontract = GVRow.FindControl("chkSendMailcontract")
                    txtEmailId = GVRow.FindControl("txtEmailId")
                    txtusername = GVRow.FindControl("txtwebuser")
                    lblWPassword = GVRow.FindControl("lblWPassword")
                    lblagentcode = GVRow.FindControl("lblagentcode")
                    txtContactPerson = GVRow.FindControl("txtContactPerson")
                    lblwebapprove = GVRow.FindControl("lblwebapprove")
                    If chkSendMailcontract.Checked = True And lblwebapprove.Text = "Approved" Then




                        strEmailText1 = "<center> <p class='MsoNormal' style='text-align: left;'><span style='font-family: calibri, sans-serif; color: #0d0d0d; font-size: 12pt;'>Dear  Partner,</span><span style='font-family:calibri,sans-serif;color:#0d0d0d'><o:p /></span><br /></p><p class='MsoNormal'><b><span style='font-family:calibri,sans-serif;color:#002060'><o:p /></span></b></p></center>"
                        strEmailText1 = strEmailText1 + "&nbsp;</span></b></p> <div></div>"

                        strEmailText1 = strEmailText1 + strEmailText_cc1


                        'strEmailText1 = strEmailText1 + "<p class='MsoNormal' style='margin: 0'><br /></p> <p class='MsoNormal' style='margin: 0'><b><span style='font-family:calibri,sans-serif;color:#0d0d0d'>Username:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + txtusername.Text + "</span></b></p>"
                        'strEmailText1 = strEmailText1 + "<p class='MsoNormal' style='margin: 0'><b><span style='font-family:calibri,sans-serif;color:#0d0d0d'>Password: &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + lblWPassword.Text + "</span></b></p> <p class='MsoNormal' style='margin: 0'><font face='Calibri,sans-serif'>&nbsp;</font></p>"


                        Dim strpswd As String = CType(txtusername.Text, String) 'objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select dbo.pwdencript('" & CType(lblWPassword.Text, String) & "')")


                        strEmailText1 = strEmailText1 + "<br /> " + strfootertext1

                        strcc = "Select option_selected from reservation_parameters where param_id =552" ' "Select option_selected from reservation_parameters where param_id =1006"

                        Dim ds1 As DataSet = objUtils.ExecuteQuerySqlnew(Session("dbconnectionName"), strcc)
                        If ds1.Tables.Count > 0 Then
                            If ds1.Tables(0).Rows.Count > 0 Then
                                If IsDBNull(ds1.Tables(0).Rows(0)(0)) = False Then
                                    tocc = CType(ds1.Tables(0).Rows(0)(0), String)
                                End If
                            End If
                        End If


                        'to_email = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select option_value from reservation_parameters where param_id=2007 and option_selected='Test'") 
                        'If to_email = "" Then
                        '    to_email = txtEmailId.Text.Trim
                        'End If


                        ''chk strFromEmailID = "' objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id='563'")
                        '   tocc = "shahul@mahce.com"

                        If objEmail.SendEmailCCust(strFromEmailID, txtEmailId.Text, tocc, strSubject1, strEmailText1, attachment) = False Then
                            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))          'connection open
                            'sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start
                            mySqlCmd = New SqlCommand("sp_mail_fail", mySqlConn)
                            mySqlCmd.CommandType = CommandType.StoredProcedure
                            mySqlCmd.Parameters.Add(New SqlParameter("@agentcode", SqlDbType.VarChar, 20)).Value = CType(lblagentcode.Text, String)
                            mySqlCmd.Parameters.Add(New SqlParameter("@agentname", SqlDbType.VarChar, 20)).Value = CType(GVRow.Cells(2).Text.Trim, String)
                            mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                            mySqlCmd.ExecuteNonQuery()

                            'sqlTrans.Commit()    'SQl Tarn Commit
                            'clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
                            clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
                            clsDBConnect.dbConnectionClose(mySqlConn)           'connection close
                            flg = False
                            '' ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "3", "alert('Failed to Send the mail to " + to_email + "');", True)
                        Else


                        End If
                        to_email = ""

                    End If
                Next

            End If ''flag=true

            If flg = False Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Some mails are fail to Send');", True)
                SetFocus(btnSendMail)
            Else
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('E-Mail sending successfully.');", True)
                SetFocus(btnSendMail)
            End If


        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ApproveCustomersforWeb.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try

    End Sub

    Private Sub PwdSendmailLog_Entry(ByVal prm_agentcode As String, ByVal prm_stremails As String, ByVal prm_pagename As String)
        mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
        'sqlTrans = mySqlConn.BeginTransaction

        mySqlCmd = New SqlCommand("sp_add_PwdMailSend_Log", mySqlConn)
        mySqlCmd.CommandType = CommandType.StoredProcedure
        mySqlCmd.Parameters.Add(New SqlParameter("@agentcode", SqlDbType.VarChar, 20)).Value = CType(prm_agentcode, String)
        mySqlCmd.Parameters.Add(New SqlParameter("@mailDateTime", SqlDbType.DateTime)).Value = CType(System.DateTime.Now, DateTime)
        mySqlCmd.Parameters.Add(New SqlParameter("@mailSendPageName", SqlDbType.VarChar, 200)).Value = prm_pagename.ToString

        mySqlCmd.Parameters.Add(New SqlParameter("@usercode", SqlDbType.VarChar, 20)).Value = CType(Session("GlobalUserName"), String)
        mySqlCmd.Parameters.Add(New SqlParameter("@agent_email", SqlDbType.VarChar, 50)).Value = CType(prm_stremails.Trim, String)

        mySqlCmd.ExecuteNonQuery()

        'sqlTrans.Commit()    'SQl Tarn Commit
        'clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
        clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
        clsDBConnect.dbConnectionClose(mySqlConn)

    End Sub
End Class

'clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
'clsDBConnect.dbConnectionClose(SqlConn)                          'Close connection

' If CType(GVRow.Cells(5).Text, String) = "&nbsp;" Or CType(GVRow.Cells(6).Text, String) = "&nbsp;" Or CType(GVRow.Cells(9).Text, String) = "&nbsp;" Then
' If CType(GVRow.Cells(5).Text, String) = "&nbsp;" Or CType(GVRow.Cells(6).Text, String) = "&nbsp;" Or CType(GVRow.Cells(9).Text, String) = "&nbsp;" Then