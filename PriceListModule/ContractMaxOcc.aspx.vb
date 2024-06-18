Imports System.Data
Imports System.Data.SqlClient
Imports System.Drawing.Color
Imports System.Collections.ArrayList
Imports System.Collections.Generic
Imports System.Drawing
Imports AjaxControlToolkit
Imports System.IO

Partial Class ContractMaxOcc
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



#End Region
#Region "Enum GridCol"
    Enum GridCol

        MaxOccid = 1
        Fromdate = 2
        Todate = 3
        approved = 4
        Edit = 8
        View = 9
        Delete = 10
        Copy = 11
        DateCreated = 12
        UserCreated = 13
        DateModified = 14
        UserModified = 15

    End Enum
#End Region
    Protected Sub btnOccupancyOK_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOccupancyOK.Click
        Dim chkSelect As HtmlInputCheckBox
        Dim intAdult, intChild As Integer
        Dim strChkdStringVal As StringBuilder = New StringBuilder()
        Dim tickedornot As Boolean
        Dim strChkdStringValnew As StringBuilder = New StringBuilder()
        ' Dim strChkdageStringVal As StringBuilder = New StringBuilder()

        Dim strChkdageStringVal As String = ""

        tickedornot = False
        For Each grdRow In gvOccupancy.Rows
            chkSelect = CType(grdRow.FindControl("chk"), HtmlInputCheckBox)
            chkSelect = grdRow.FindControl("chk")
            If chkSelect.Checked = True Then
                tickedornot = True
                Exit For
            End If
        Next

        If tickedornot = False Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please select combination');", True)
            ModalPopupOccupancy.Show()
            Exit Sub
        End If

        For Each grdRow In gvOccupancy.Rows
            Dim ddlrmcat As HtmlSelect = grdRow.FindControl("ddlRmcat")
            chkSelect = CType(grdRow.FindControl("chk"), HtmlInputCheckBox)
            If ddlrmcat.Value = "[Select]" And chkSelect.Checked = True Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please select Accommodation Category');", True)
                ModalPopupOccupancy.Show()
                Exit Sub
            End If

        Next

        Dim childagestring As String = ""

        For Each grdRow In gvOccupancy.Rows
            chkSelect = CType(grdRow.FindControl("chk"), HtmlInputCheckBox)
            Dim ddlrmcat As HtmlSelect = grdRow.FindControl("ddlRmcat")
            Dim txtchild1age As TextBox = grdRow.findcontrol("txtchild1age")
            Dim txtchild2age As TextBox = grdRow.findcontrol("txtchild2age")
            Dim txtchild3age As TextBox = grdRow.findcontrol("txtchild3age")
            Dim txtchild4age As TextBox = grdRow.findcontrol("txtchild4age")
            Dim txtchild5age As TextBox = grdRow.findcontrol("txtchild5age")
            Dim txtchild6age As TextBox = grdRow.findcontrol("txtchild6age")
            Dim txtchild7age As TextBox = grdRow.findcontrol("txtchild7age")
            Dim txtchild8age As TextBox = grdRow.findcontrol("txtchild8age")
            Dim txtchild9age As TextBox = grdRow.findcontrol("txtchild9age")

            If chkSelect.Checked = True Then
                intAdult = grdRow.Cells(1).Text
                intChild = grdRow.Cells(2).Text

                strChkdStringVal.AppendFormat("{0}/{1},", intAdult, intChild)
                ' strChkdStringValnew.AppendFormat("{0}/{1}/{2},", intAdult, intChild, ddlrmcat.Value)

                strChkdStringValnew.AppendFormat("{0}/{1}/{2}/{3}/{4}/{5}/{6}/{7}/{8}/{9}/{10}/{11},", intAdult, intChild, ddlrmcat.Value, txtchild1age.Text, txtchild2age.Text, txtchild3age.Text, txtchild4age.Text, IIf(txtchild5age.Text = "", "0.00", txtchild5age.Text), IIf(txtchild6age.Text = "", "0.00", txtchild6age.Text), IIf(txtchild7age.Text = "", "0.00", txtchild7age.Text), IIf(txtchild8age.Text = "", "0.00", txtchild8age.Text), IIf(txtchild9age.Text = "", "0.00", txtchild9age.Text))

                'If Val(txtchild1age.Text) <> 0 Then
                '    childagestring = CType(Val(txtchild1age.Text), String)
                'End If
                'If Val(txtchild2age.Text) <> 0 Then
                '    childagestring = CType(Val(txtchild1age.Text), String) + "/" + CType(Val(txtchild2age.Text), String)
                'End If
                'If Val(txtchild3age.Text) <> 0 Then
                '    childagestring = CType(Val(txtchild1age.Text), String) + "/" + CType(Val(txtchild2age.Text), String) + "/" + CType(Val(txtchild3age.Text), String)
                'End If
                'If Val(txtchild4age.Text) <> 0 Then
                '    childagestring = CType(Val(txtchild1age.Text), String) + "/" + CType(Val(txtchild2age.Text), String) + "/" + CType(Val(txtchild3age.Text), String) + "/" + CType(Val(txtchild4age.Text), String)
                'End If
                'If Val(txtchild5age.Text) <> 0 Then
                '    childagestring = CType(Val(txtchild1age.Text), String) + "/" + CType(Val(txtchild2age.Text), String) + "/" + CType(Val(txtchild3age.Text), String) + "/" + CType(Val(txtchild4age.Text), String) + "/" + CType(Val(txtchild5age.Text), String)
                'End If
                'If Val(txtchild6age.Text) <> 0 Then
                '    childagestring = CType(Val(txtchild1age.Text), String) + "/" + CType(Val(txtchild2age.Text), String) + "/" + CType(Val(txtchild3age.Text), String) + "/" + CType(Val(txtchild4age.Text), String) + "/" + CType(Val(txtchild5age.Text), String) + "/" + CType(Val(txtchild6age.Text), String)
                'End If
                'If Val(txtchild7age.Text) <> 0 Then
                '    childagestring = CType(Val(txtchild1age.Text), String) + "/" + CType(Val(txtchild2age.Text), String) + "/" + CType(Val(txtchild3age.Text), String) + "/" + CType(Val(txtchild4age.Text), String) + "/" + CType(Val(txtchild5age.Text), String) + "/" + CType(Val(txtchild6age.Text), String) + "/" + CType(Val(txtchild7age.Text), String)
                'End If
                'If Val(txtchild8age.Text) <> 0 Then
                '    childagestring = CType(Val(txtchild1age.Text), String) + "/" + CType(Val(txtchild2age.Text), String) + "/" + CType(Val(txtchild3age.Text), String) + "/" + CType(Val(txtchild4age.Text), String) + "/" + CType(Val(txtchild5age.Text), String) + "/" + CType(Val(txtchild6age.Text), String) + "/" + CType(Val(txtchild7age.Text), String) + "/" + CType(Val(txtchild8age.Text), String)
                'End If
                'If Val(txtchild9age.Text) <> 0 Then
                '    childagestring = CType(Val(txtchild1age.Text), String) + "/" + CType(Val(txtchild2age.Text), String) + "/" + CType(Val(txtchild3age.Text), String) + "/" + CType(Val(txtchild4age.Text), String) + "/" + CType(Val(txtchild5age.Text), String) + "/" + CType(Val(txtchild6age.Text), String) + "/" + CType(Val(txtchild7age.Text), String) + "/" + CType(Val(txtchild8age.Text), String) + "/" + CType(Val(txtchild9age.Text), String)
                'End If
            
            End If
        Next

        'Dim arrage As String()
        'If childagestring <> "" Then

        '    arrage = childagestring.Split("/")

        '    If arrage.Length = 1 Then
        '        strChkdageStringVal = (CType(childagestring, String) + "/0.00/0.00/0.00/0.00/0.00/0.00/0.00/0.00")
        '        'strChkdageStringVal.AppendFormat("{0}/{1}/{2}/{3}/4}/{5}/{6}/{7}/{8}/{9},", CType(childagestring, String) + "/0.00/0.00/0.00/0.00/0.00/0.00/0.00/0.00")
        '    ElseIf arrage.Length = 2 Then
        '        strChkdageStringVal = (CType(childagestring, String) + "/0/0/0/0/0/0/0")
        '        ' strChkdageStringVal.AppendFormat("{0}/{1}/{2}/{3}/4}/{5}/{6}/{7}/{8}/{9},", CType(childagestring, String) + "/0/0/0/0/0/0/0")
        '    ElseIf arrage.Length = 3 Then
        '        strChkdageStringVal = (CType(childagestring, String) + "/0/0/0/0/0/0")
        '        ' strChkdageStringVal.AppendFormat("{0}/{1}/{2}/{3}/4}/{5}/{6}/{7}/{8}/{9},", CType(childagestring, String) + "/0/0/0/0/0/0")
        '    ElseIf arrage.Length = 4 Then
        '        strChkdageStringVal = (CType(childagestring, String) + "/0/0/0/0/0")
        '        ' strChkdageStringVal.AppendFormat("{0}/{1}/{2}/{3}/4}/{5}/{6}/{7}/{8}/{9},", CType(childagestring, String) + "/0/0/0/0/0")
        '    ElseIf arrage.Length = 5 Then
        '        strChkdageStringVal = (CType(childagestring, String) + "/0/0/0/0")
        '        'strChkdageStringVal.AppendFormat("{0}/{1}/{2}/{3}/4}/{5}/{6}/{7}/{8}/{9},", CType(childagestring, String) + "/0/0/0/0")
        '    ElseIf arrage.Length = 6 Then
        '        strChkdageStringVal = (CType(childagestring, String) + "/0/0/0")
        '        ' strChkdageStringVal.AppendFormat("{0}/{1}/{2}/{3}/4}/{5}/{6}/{7}/{8}/{9},", CType(childagestring, String) + "/0/0/0")
        '    ElseIf arrage.Length = 7 Then
        '        strChkdageStringVal = (CType(childagestring, String) + "/0/0")
        '        ' strChkdageStringVal.AppendFormat("{0}/{1}/{2}/{3}/4}/{5}/{6}/{7}/{8}/{9},", CType(childagestring, String) + "/0/0")
        '    ElseIf arrage.Length = 8 Then
        '        strChkdageStringVal = (CType(childagestring, String) + "/0")
        '        'strChkdageStringVal.AppendFormat("{0}/{1}/{2}/{3}/4}/{5}/{6}/{7}/{8}/{9},", CType(childagestring, String) + "/0")
        '    ElseIf arrage.Length = 9 Then
        '        strChkdageStringVal = (CType(childagestring, String))
        '        ' strChkdageStringVal.AppendFormat("{0}/{1}/{2}/{3}/4}/{5}/{6}/{7}/{8}/{9},", CType(childagestring, String))
        '    End If
        'End If




        Dim agestring As String()
        If hdnMainGridRowid.Value <> "" Then
            Dim txtMaxocccombination As TextBox = gv_FillData.Rows(hdnMainGridRowid.Value).FindControl("txtMaxocccombination")
            Dim txtcombinationcat As TextBox = gv_FillData.Rows(hdnMainGridRowid.Value).FindControl("txtcombinationcat")
            'Dim txtage1 As TextBox = gv_FillData.Rows(hdnMainGridRowid.Value).FindControl("txtage1")
            'Dim txtage2 As TextBox = gv_FillData.Rows(hdnMainGridRowid.Value).FindControl("txtage2")
            'Dim txtage3 As TextBox = gv_FillData.Rows(hdnMainGridRowid.Value).FindControl("txtage3")
            'Dim txtage4 As TextBox = gv_FillData.Rows(hdnMainGridRowid.Value).FindControl("txtage4")
            'Dim txtage5 As TextBox = gv_FillData.Rows(hdnMainGridRowid.Value).FindControl("txtage5")
            'Dim txtage6 As TextBox = gv_FillData.Rows(hdnMainGridRowid.Value).FindControl("txtage6")
            'Dim txtage7 As TextBox = gv_FillData.Rows(hdnMainGridRowid.Value).FindControl("txtage7")
            'Dim txtage8 As TextBox = gv_FillData.Rows(hdnMainGridRowid.Value).FindControl("txtage8")
            'Dim txtage9 As TextBox = gv_FillData.Rows(hdnMainGridRowid.Value).FindControl("txtage9")

            txtMaxocccombination.Text = strChkdStringVal.ToString
            txtcombinationcat.Text = strChkdStringValnew.ToString

            'agestring = strChkdageStringVal.ToString.Split("/")

            'txtage1.Text = agestring(0)
            'txtage2.Text = agestring(1)
            'txtage3.Text = agestring(2)
            'txtage4.Text = agestring(3)
            'txtage5.Text = agestring(4)
            'txtage6.Text = agestring(5)
            'txtage7.Text = agestring(6)
            'txtage8.Text = agestring(7)
            'txtage9.Text = agestring(8)
        End If
        ModalPopupOccupancy.Hide()

    End Sub
    '*** Danny 3/3/18>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

    Protected UploadFolderPath As String = "~/PriceListModule/RoomImages/"
    Protected Sub FileUploadComplete(ByVal sender As Object, ByVal e As EventArgs)
        Try
            '' ''Dim filename As String = System.IO.Path.GetFileName(AsyncFileUpload1.FileName)
            '' ''AsyncFileUpload1.SaveAs(Server.MapPath("RoomImages/") + filename)

            Dim sPath As String = hdnpartycode.Value + "_" + RoomImgRow.Value.ToString() + AsyncFileUpload1.FileName
            If Not Directory.Exists(Path.GetDirectoryName(Server.MapPath("Roomimages/") + sPath)) Then
                Directory.CreateDirectory(Path.GetDirectoryName(Server.MapPath("Roomimages/") + sPath))

            End If
            AsyncFileUpload1.SaveAs(Server.MapPath("Roomimages/") + sPath)
            AsyncFileUpload1.BackColor = Green


        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ContractMinNights.aspx=>FileUploadComplete()", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try



    End Sub


    Protected Sub RoomImages_Click(ByVal sender As Object, ByVal e As System.EventArgs)

        Dim objbtn As ImageButton = CType(sender, ImageButton)
        Dim rowid As Integer = 0
        Dim row As GridViewRow
        row = CType(objbtn.NamingContainer, GridViewRow)
        rowid = row.RowIndex

        Dim strrmtypename As String = CType(gv_FillData.Rows(rowid).FindControl("txtrmtypename"), TextBox).Text
        Dim strrmclassname As String = CType(gv_FillData.Rows(rowid).FindControl("txtrmclassname"), TextBox).Text
        If strrmtypename.Trim.Length = 0 Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('You must enter Room name first!');", True)
            Exit Sub
        End If

        Dim strRoomCategory As String = CType(strrmclassname, String)
        Dim strRoomType As String = CType(strrmtypename, String)
        lblRoomCategoryTextIMG.Text = strRoomCategory
        lblRoomTypeTextIMG.Text = strRoomType
        'If strrmclassname.Trim.Length = 0 Then
        '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('You must enter Room name first!');", True)
        '    Exit Sub
        'End If
        Dim rmtypcode As String = CType(gv_FillData.Rows(rowid).FindControl("txtrmtype"), TextBox).Text
        RoomImgRow.Value = rowid.ToString()
        ImgRoomImage.ImageUrl = CType(gv_FillData.Rows(rowid).FindControl("RoomImages"), ImageButton).ImageUrl

        SelectedRoomImgPath.Value = String.Empty

        'lblimage.Text = Server.MapPath(CType(gv_FillData.Rows(rowid).FindControl("RoomImages"), ImageButton).ImageUrl)
        If ImgRoomImage.ImageUrl.ToString.Contains("ImageIco") Then
            ImgRoomImage.Visible = False
            btnViewimage.Visible = False
            Btnrmv7.Visible = False
        Else
            btnViewimage.Visible = False
            Btnrmv7.Visible = True
            ImgRoomImage.Visible = True
        End If

        ModalPopupExtender2.Show()

    End Sub
    Protected Sub Btnrmv7_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Btnrmv7.Click
        Try
            Dim rowid As Integer = RoomImgRow.Value
            'Dim s As String = Server.MapPath(CType(gv_FillData.Rows(rowid).FindControl("RoomImages"), ImageButton).ImageUrl)
            File.Delete(Server.MapPath(CType(gv_FillData.Rows(rowid).FindControl("RoomImages"), ImageButton).ImageUrl))
            CType(gv_FillData.Rows(rowid).FindControl("RoomImages"), ImageButton).ImageUrl = "../images/ImageIco.png"
            ModalPopupExtender2.Hide()
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Check Folder/File Permission. Then try again' );", True)
            objUtils.WritErrorLog("ContractMinNights.aspx=>Btnrmv7_Click()", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try

    End Sub
    Protected Sub Btn_IMGCance_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Btn_IMGCance.Click
        Try
            'Dim sPath As String = "~/PriceListModule/RoomImages/" + hdnpartycode.Value + "_" + RoomImgRow.Value.ToString() + "_main.jpg"

            ' ''Dim gvrow As GridViewRow = gv_FillData.Rows(RoomImgRow.Value)

            ' ''Dim sRoomImages As ImageButton = gvrow.FindControl("RoomImages")
            '' ''Dim sRoomImgPathlbl As Label = gvrow.FindControl("RoomImgPathlbl")
            '' ''sRoomImgPathlbl.Text = Server.MapPath("Roomimages/") + sPath

            ' ''sRoomImages.ImageUrl = sPath

            '' ''If RoomImgRow.Value <> "" Then
            '' ''    Dim txtMaxocccombination As ImageButton = gv_FillData.Rows(RoomImgRow.Value).FindControl("RoomImages")
            '' ''    Dim txtcombinationcat As Label = gv_FillData.Rows(RoomImgRow.Value).FindControl("RoomImgPathlbl")
            '' ''    txtMaxocccombination.ImageUrl = Server.MapPath("Roomimages/") + sPath
            '' ''    txtcombinationcat.Text = Server.MapPath("Roomimages/") + sPath
            '' ''End If

            'File.Delete(Server.MapPath(sPath))

            ModalPopupExtender2.Hide()
        Catch ex As Exception
            If mySqlConn.State = ConnectionState.Open Then
                sqlTrans.Rollback()
            End If
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)

            objUtils.WritErrorLog("SupWebInfo.aspx->Btn_IMGCance()", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub

    Protected Sub Btn_Save_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Btn_Save.Click


        Try
            If SelectedRoomImgPath.Value.ToString().Length > 0 Then
                Dim sPath As String = "RoomImages/" + hdnpartycode.Value + "_" + RoomImgRow.Value.ToString() + SelectedRoomImgPath.Value

                Dim gvrow As GridViewRow = gv_FillData.Rows(RoomImgRow.Value)

                Dim sRoomImages As ImageButton = gvrow.FindControl("RoomImages")
                'Dim sRoomImgPathlbl As Label = gvrow.FindsControl("RoomImgPathlbl")
                Dim s As String = Server.MapPath(sPath)
                If File.Exists(Server.MapPath(sPath)) Then
                    sRoomImages.ImageUrl = sPath
                End If


                'If RoomImgRow.Value <> "" Then
                '    Dim txtMaxocccombination As ImageButton = gv_FillData.Rows(RoomImgRow.Value).FindControl("RoomImages")
                '    Dim txtcombinationcat As Label = gv_FillData.Rows(RoomImgRow.Value).FindControl("RoomImgPathlbl")
                '    txtMaxocccombination.ImageUrl = Server.MapPath("Roomimages/") + sPath
                '    txtcombinationcat.Text = Server.MapPath("Roomimages/") + sPath
                'End If
            End If
        Catch ex As Exception
            'If mySqlConn.State = ConnectionState.Open Then
            '    sqlTrans.Rollback()
            'End If
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)

            objUtils.WritErrorLog("SupWebInfo.aspx->Btn_Save_Click()", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try




    End Sub

    '*** Danny 3/3/18<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
    Private Sub fillcondetails()
        Try
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            mySqlCmd = New SqlCommand("Select   min(c.fromdate) fromdate,max(c.todate) todate from view_contracts_search c(nolock) where contractid='" & hdncontractid.Value & "'", mySqlConn)
            mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
            If mySqlReader.HasRows Then
                If mySqlReader.Read() = True Then
                    'End If
                    If IsDBNull(mySqlReader("fromdate")) = False Then
                        hdnconfromdate.Value = Format(mySqlReader("fromdate"), "dd/MM/yyyy")


                    End If
                    If IsDBNull(mySqlReader("todate")) = False Then
                        hdncontodate.Value = Format(mySqlReader("todate"), "dd/MM/yyyy")


                    End If

                End If





                mySqlCmd.Dispose()
                mySqlReader.Close()

                mySqlConn.Close()

            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ContractMinNights.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally

            If mySqlConn.State = ConnectionState.Open Then mySqlConn.Close()
        End Try


    End Sub
#Region "Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim RefCode As String
        Dim CalledfromValue As String = ""

        Dim ConMaxappid As String = ""
        Dim ConMAxappname As String = ""


        Dim Count As Integer
        Dim lngCount As Int16
        Dim strTempUserFunctionalRight As String()
        Dim strRights As String
        Dim functionalrights As String = ""


        Session("Calledfrom") = CType(Request.QueryString("Calledfrom"), String)
        If IsPostBack = False Then

            ConMaxappid = 1
            ConMAxappname = objUser.GetAppName(Session("dbconnectionName"), ConMaxappid)

            If CType(Session("GlobalUserName"), String) = Nothing Or CType(Session("Userpwd"), String) = Nothing Then
                Response.Redirect("~/Login.aspx", False)
                Exit Sub
            Else
                If Session("Calledfrom") = "Contracts" Then
                    Me.SubMenuUserControl1.menuidval = objUser.GetMenuId(Session("dbconnectionName"), CType("PriceListModule\ContractsSearch.aspx?appid=1", String), CType(Request.QueryString("appid"), Integer))
                    CalledfromValue = Me.SubMenuUserControl1.menuidval
                    objUser.CheckUserSubRight(Session("dbconnectionName"), CType(Session("GlobalUserName"), String), CType(Session("Userpwd"), String), _
                                                       CType(ConMAxappname, String), "ContractMaxOcc.aspx", CType(CalledfromValue, String), btnAddNew, btnExportToExcel, _
                 btnprint, gv_SearchResult, GridCol.Edit, GridCol.Delete, GridCol.View, 0, GridCol.Copy)


                ElseIf Session("Calledfrom") = "Offers" Then
                    Me.SubMenuUserControl1.menuidval = objUser.GetMenuId(Session("dbconnectionName"), CType("PriceListModule\OfferSearch.aspx?appid=1", String), CType(Request.QueryString("appid"), Integer))
                    CalledfromValue = Me.SubMenuUserControl1.menuidval
                    objUser.CheckUserSubRight(Session("dbconnectionName"), CType(Session("GlobalUserName"), String), CType(Session("Userpwd"), String), _
                                                                          CType(ConMAxappname, String), "ContractMaxOcc.aspx", CType(CalledfromValue, String), btnAddNew, btnExportToExcel, _
                    btnprint, gv_SearchResult, GridCol.Edit, GridCol.Delete, GridCol.View, 0, GridCol.Copy)
                End If
            End If

            Dim intGroupID As Integer = objUser.GetGroupId(Session("dbconnectionName"), CType(Session("GlobalUserName"), String), CType(Session("Userpwd"), String))
            Dim intMenuID As Integer = objUser.GetCotractofferMenuId(Session("dbconnectionName"), "ContractMaxOcc.aspx", ConMaxappid, CalledfromValue)

            functionalrights = objUser.GetUserFunctionalRight(Session("dbconnectionName"), intGroupID, ConMaxappid, intMenuID)
            If functionalrights <> "" Then

                strTempUserFunctionalRight = functionalrights.Split(";")
                For lngCount = 0 To strTempUserFunctionalRight.Length - 1
                    strRights = strTempUserFunctionalRight.GetValue(lngCount)

                    If strRights = "07" Then
                        Count = 1
                    End If
                Next

                If CalledfromValue = 1030 Then
                    btnselect.Visible = False
                    If Count = 1 Then
                        btncopycontract.Visible = True
                    Else
                        btncopycontract.Visible = False
                    End If

                ElseIf CalledfromValue = 1200 Then
                    btncopycontract.Visible = False
                    If Count = 1 Then
                        btnselect.Visible = True
                    Else
                        btnselect.Visible = False
                    End If
                End If


            Else
                btnselect.Visible = False
                btncopycontract.Visible = False

            End If




            If Session("Calledfrom") = "Offers" Then

                Me.SubMenuUserControl1.appval = CType(Request.QueryString("appid"), String)
                Me.SubMenuUserControl1.menuidval = objUser.GetMenuId(Session("dbconnectionName"), CType("PriceListModule\OfferSearch.aspx?appid=1", String), CType(Request.QueryString("appid"), Integer))
                Me.SubMenuUserControl1.Calledfromval = CType(Request.QueryString("Calledfrom"), String)

                ViewState("Menucalling") = Me.SubMenuUserControl1.menuidval

                '  objUtils.Clear_All_contract_sessions()
                txtconnection.Value = Session("dbconnectionName")
                btnselect.Style.Add("display", "block")

                divoffer.Style.Add("display", "block")
                btncopycontract.Style.Add("display", "block")
                If Not Session("OfferRefCode") Is Nothing Then
                    hdnpromotionid.Value = Session("OfferRefCode")
                    txtpromotionidnew.Text = Session("OfferRefCode")
                    Dim ds As DataSet = objUtils.ExecuteQuerySqlnew(Session("dbconnectionName"), "select isnull(promotionname,'') promotionname , ApplicableTo,commissiontype  from view_offers_header (nolock) where  promotionid='" & txtpromotionidnew.Text & "'")
                    If ds.Tables(0).Rows.Count > 0 Then
                        txtpromotionnamenew.Text = ds.Tables(0).Rows(0).Item("promotionname")

                        hdncommtype.Value = ds.Tables(0).Rows(0).Item("commissiontype")
                    End If

                    Dim ds1 As DataSet = objUtils.ExecuteQuerySqlnew(Session("dbconnectionName"), "select convert(varchar(10),min(convert(datetime,d.fromdate,111)),103) fromdate, convert(varchar(10),max(convert(datetime,d.todate,111)),103) todate  from view_offers_detail d(nolock) where  d.promotionid='" & hdnpromotionid.Value & "'")
                    If ds1.Tables(0).Rows.Count > 0 Then
                        hdnpromofrmdate.Value = ds1.Tables(0).Rows(0).Item("fromdate")
                        hdnpromotodate.Value = ds1.Tables(0).Rows(0).Item("todate")
                    End If


                End If

                wucCountrygroup.sbSetPageState("", "OFFERSMAIN", CType(Session("OfferState"), String))

                hdnpartycode.Value = Session("Offerparty") 'CType(Request.QueryString("partycode"), String)
                SubMenuUserControl1.partyval = hdnpartycode.Value
                SubMenuUserControl1.contractval = Session("contractid") 'CType(Request.QueryString("contractid"), String)


                txthotelname.Text = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select partyname from partymast where partycode='" & hdnpartycode.Value & "'")
                ViewState("hotelname") = txthotelname.Text
                ' Session("partycode") = hdnpartycode.Value

                '    Session("contractid") = CType(Request.QueryString("contractid"), String)
                lblHeading.Text = lblHeading.Text + " - " + txthotelname.Text + " - " + hdnpromotionid.Value
                Page.Title = "Promotion Maxoccupancy "

                gv_SearchResult.Columns(5).Visible = True
                gv_SearchResult.Columns(6).Visible = True

            Else

                Me.SubMenuUserControl1.appval = CType(Request.QueryString("appid"), String)
                Me.SubMenuUserControl1.menuidval = objUser.GetMenuId(Session("dbconnectionName"), CType("PriceListModule\ContractsSearch.aspx?appid=1", String), CType(Request.QueryString("appid"), Integer))
                Me.SubMenuUserControl1.Calledfromval = CType(Request.QueryString("Calledfrom"), String)

                ViewState("Menucalling") = Me.SubMenuUserControl1.menuidval

                '  objUtils.Clear_All_contract_sessions()
                txtconnection.Value = Session("dbconnectionName")
                divoffer.Style.Add("display", "none")
                btnselect.Style.Add("display", "none")
                '' btncopycontract.Style.Add("display", "none")
                'hdCurrentDate.Value = Now.ToString("dd/MM/yyyy")
                hdncontractid.Value = CType(Session("contractid"), String)
                hdnpartycode.Value = Session("Contractparty") 'CType(Request.QueryString("partycode"), String)
                SubMenuUserControl1.partyval = hdnpartycode.Value
                SubMenuUserControl1.contractval = Session("contractid") 'CType(Request.QueryString("contractid"), String)

                wucCountrygroup.sbSetPageState("", "MAXOCCUPANCY", "NEW")

                txthotelname.Text = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select partyname from partymast where partycode='" & hdnpartycode.Value & "'")
                ViewState("hotelname") = txthotelname.Text
                ' Session("partycode") = hdnpartycode.Value

                '    Session("contractid") = CType(Request.QueryString("contractid"), String)
                lblHeading.Text = lblHeading.Text + " - " + txthotelname.Text + " - " + Session("contractid")
                Page.Title = "Contract Maxoccupancy "
                gv_SearchResult.Columns(5).Visible = False
                gv_SearchResult.Columns(6).Visible = False

            End If




            If ddlCopyRoom.Value <> "[Select]" Then

                strSqlQry = " select  partyrmtyp.rmtypname ,partyrmtyp.rmtypcode " & _
                         " from partyrmtyp  " & _
                          " where  partyrmtyp.inactive=0 and partycode='" + hdnpartycode.Value + "' order by rankord"
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCopyRoom, "rmtypname", "rmtypcode", strSqlQry, True, ddlCopyRoom.Value)
            Else

                strSqlQry = " select  partyrmtyp.rmtypname ,partyrmtyp.rmtypcode " & _
                         " from partyrmtyp  " & _
                          " where  partyrmtyp.inactive=0 and partycode='" + hdnpartycode.Value + "' order by rankord"
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCopyRoom, "rmtypname", "rmtypcode", strSqlQry, True)

            End If

            ddlOrder.SelectedIndex = 0
            ddlorderby.SelectedIndex = 1
            FillGrid("view_partymaxacc_header.tranid", hdnpartycode.Value, "Desc")

            'PanelMain.Visible = False

        Else

            If chkctrygrp.Checked = True Then
                divuser.Style.Add("display", "block")
            Else
                divuser.Style.Add("display", "none")
            End If
        End If
        'If ViewState("Menucalling") = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select option_selected  from reservation_parameters where param_id=1138") Then
        '    gv_SearchResult.Columns(5).Visible = False
        '    gv_SearchResult.Columns(6).Visible = False
        'Else
        '    gv_SearchResult.Columns(5).Visible = True
        '    gv_SearchResult.Columns(6).Visible = True
        'End If

        chkctrygrp.Attributes.Add("onChange", "showusercontrol('" & chkctrygrp.ClientID & "')")

        btnViewimage.Attributes.Add("onclick", "return PopUpImageView('" & lblimage.Text & "')") '*** Danny 3/3/18

        'ImgRoomImage.Attributes.Add("onclick", "return PopUpImageView('" & lblimage.Text & "')") '*** Danny 3/3/18
        ' ddlorderby.SelectedIndex = 0

        'btnCancel.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to cancel?')==false)return false;")
        Btnrmv7.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to Delete this Image?')==false)return false;") '*** Danny 13/3/18
        Dim typ As Type
        typ = GetType(DropDownList)

        If (Page.ClientScript.IsClientScriptIncludeRegistered(typ, "TADDScript.js")) = False Then
            Page.ClientScript.RegisterClientScriptInclude(typ, "TADDScript.js", "TADDScript.js")


        End If

        Session.Add("submenuuser", "ContractsSearch.aspx")
    End Sub
#End Region
#Region "related to user control wucCountrygroup"
    Protected Sub btnvsprocess_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnvsprocess.Click
        wucCountrygroup.fnbtnVsProcess(txtvsprocesssplit, dlList)
    End Sub

    Protected Sub lnkCodeAndValue_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            wucCountrygroup.fnCodeAndValue_ButtonClick(sender, e, dlList, Nothing, Nothing)
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ContractMain.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub

    Protected Sub lbClose_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            wucCountrygroup.fnCloseButtonClick(sender, e, dlList)
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ContractMain.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
    Protected Sub btnselect_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        If (hdnpartycode.Value.Trim <> "") Then
            Dim myDataAdapter As SqlDataAdapter
            grdpromotion.Visible = True


            Dim MyDs As New DataTable
            Dim countryList As String = ""
            Dim agentList As String = ""
            Dim filterCond As String = ""
            If wucCountrygroup.checkcountrylist.ToString().Trim <> "" Then
                countryList = wucCountrygroup.checkcountrylist.ToString().Trim.Replace(",", "','")
                filterCond = "h.promotionid  in (select promotionid from view_offers_countries where ctrycode in (' " + countryList + "'))"
            End If
            If wucCountrygroup.checkagentlist.ToString().Trim <> "" Then
                agentList = wucCountrygroup.checkagentlist.ToString().Trim.Replace(",", "','")
                If filterCond <> "" Then
                    filterCond = filterCond + " or h.promotionid  in (select promotionid from view_offers_agents where agentcode in ( '" + agentList + "'))"
                Else
                    filterCond = "h.promotionid  in (select promotionid from view_offers_agents where agentcode in ( '" + agentList + "'))"
                End If
            End If
            If filterCond <> "" Then
                filterCond = " and (" + filterCond + ")"
            End If
            filterCond = ""
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
            strSqlQry = " select oh.tranid,h.promotionid,max(h.promotionname) promotionname ,max(h.applicableto)applicableto, convert(varchar(10),min(d.fromdate),103) fromdate,convert(varchar(10),max(d.todate),103) todate,  " _
                & " case when ISNULL(h.approved,0)=0 then 'No' else 'Yes' end  status from view_offers_header h(nolock) join  view_partymaxacc_header oh on h.promotionid =oh.promotionid  join view_offers_detail d(nolock) on h.promotionid =d.promotionid where  isnull(h.active,0)=0 and  h.promotionid<>'" & txtpromotionidnew.Text & "' and h.partycode='" + hdnpartycode.Value.Trim + "'  " + filterCond + " group by oh.tranid,h.promotionid,h.approved  order by convert(varchar(10),min(d.fromdate),111),convert(varchar(10),max(d.todate),111) "
            mySqlCmd = New SqlCommand(strSqlQry, mySqlConn)
            myDataAdapter = New SqlDataAdapter(strSqlQry, mySqlConn)
            myDataAdapter.Fill(MyDs)
            If MyDs.Rows.Count > 0 Then
                grdpromotion.DataSource = MyDs
                grdpromotion.DataBind()
                grdpromotion.Visible = True
            Else
                grdpromotion.Visible = False
            End If

            ModalExtraPopup1.Show()
        Else
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Select Hotel Name' );", True)
            Exit Sub
        End If
    End Sub
    Protected Sub btncopycontract_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btncopycontract.Click
        Dim myds As New DataSet

        Dim sqlstr As String = ""


        Try


            If Session("Calledfrom") = "Offers" Then


                '      strSqlQry = " select '' contractid,oh.tranid plistcode,h.promotionid,max(h.promotionname) promotionname , convert(varchar(10),min(d.fromdate),103) fromdate,convert(varchar(10),max(d.todate),103) todate  " _
                '& "  from view_offers_header h(nolock) join  view_partymaxacc_header oh on h.promotionid =oh.promotionid  join view_offers_detail d(nolock) on h.promotionid =d.promotionid where   h.promotionid<>'" & hdnpromotionid.Value & "' and h.partycode='" + hdnpartycode.Value.Trim + "' group by h.promotionid,oh.tranid  order by convert(varchar(10),min(d.fromdate),111),convert(varchar(10),max(d.todate),111) "


                strSqlQry = " select '' contractid,oh.tranid plistcode,'' promotionid,'' promotionname , convert(varchar(10),oh.frmdate,103) fromdate,convert(varchar(10),oh.todate,103) todate  " _
       & "  from  view_partymaxacc_header oh where   oh.partycode='" + hdnpartycode.Value.Trim + "'  and isnull(oh.promotionid,'')<>'" & hdnpromotionid.Value & "'  order by convert(varchar(10),oh.frmdate,111),convert(varchar(10),oh.todate,111) "
                '*** Danny Moved 03/04/2018>>>>>>>>>>>>>>
                SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
                myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
                myDataAdapter.Fill(myds)
                grdviewrates.DataSource = myds

                If myds.Tables(0).Rows.Count > 0 Then
                    grdviewrates.DataBind()
                Else
                    grdviewrates.PageIndex = 0
                    grdviewrates.DataBind()

                End If
                grdviewrates.Columns(2).Visible = False
                grdviewrates.Columns(4).Visible = False
                grdviewrates.Columns(5).Visible = False

                ModalViewrates.Show()
                clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
                clsDBConnect.dbConnectionClose(SqlConn)                          'Close connection
                '*** Danny Moved 03/04/2018<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
            End If



            

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ContractMaxOcc.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            
        End Try
    End Sub

    Protected Sub grdviewrates_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles grdviewrates.RowCommand
        Try
            If e.CommandName = "moreless" Then
                Exit Sub
            End If
            Dim lbltran As Label
            Dim lblcontract As Label, lblpromotionid As Label
            lbltran = grdviewrates.Rows(CType(e.CommandArgument.ToString, String)).FindControl("lblplistcode")
            lblcontract = grdviewrates.Rows(CType(e.CommandArgument.ToString, String)).FindControl("lblcontract")
            lblpromotionid = grdviewrates.Rows(CType(e.CommandArgument.ToString, String)).FindControl("lblpromotionid")
            If lbltran.Text.Trim = "" Then Exit Sub
            If e.CommandName = "Select" Then

                PanelMain.Visible = True
                ViewState("CopyFrom") = "CopyFrom"
                ViewState("State") = "Copy"
                Session.Add("RefCode", CType(lbltran.Text.Trim, String))
                PanelMain.Style("display") = "block"
                Panelsearch.Enabled = False

                'Session.Add("RefCode", CType(lbltran.Text.Trim, String))
                'Session.Add("Maxid", CType(lbltran.Text.Trim, String))
                'ViewState("MaxId") = Session("Maxid")
                ShowRecordoffer(CType(lbltran.Text.Trim, String))
                ShowRoomdetailsoffers(hdnpartycode.Value, CType(lbltran.Text.Trim, String))
                'fillDategrd(grdDates, True)
                'ShowDates(CType(lbltran.Text.Trim, String))



                If Session("Calledfrom") = "Offers" Then
                    wucCountrygroup.sbSetPageState("", "OFFERSMAIN", CType(Session("OfferState"), String))
                    wucCountrygroup.sbSetPageState(hdnpromotionid.Value, Nothing, "Edit")
                    wucCountrygroup.sbShowCountry()


                    btnSave.Visible = True

                    btnSave.Text = "Save"
                    lblHeading.Text = "Copy Max Occupancy - " + ViewState("hotelname") + " - " + hdnpromotionid.Value
                    Page.Title = "Max Occupancy "

                    DisableControl()
                    divdates.Style.Add("display", "none")
                    lbltext.Visible = False

                Else
                    divdates.Style.Add("display", "block")
                    lbltext.Visible = True
                End If



            End If
        Catch ex As Exception
            objUtils.WritErrorLog("ContractCommission.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
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
    Protected Sub ReadMoreLinkButtonpromotion_Click(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Dim readmore As LinkButton = CType(sender, LinkButton)
            Dim row As GridViewRow = CType((CType(sender, LinkButton)).NamingContainer, GridViewRow)
            Dim lbtext As Label = CType(row.FindControl("lblapplicable"), Label)
            Dim strtemp As String = ""
            strtemp = lbtext.Text
            If readmore.Text.ToUpper = UCase("More") Then

                lbtext.Text = lbtext.ToolTip
                lbtext.ToolTip = strtemp
                readmore.Text = "less"
            Else
                readmore.Text = "More"
                lbtext.ToolTip = lbtext.Text
                lbtext.Text = lbtext.Text.Substring(0, 10)
            End If
            ModalExtraPopup1.Show()
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ContractCommission.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
    <System.Web.Script.Services.ScriptMethod()> _
    <System.Web.Services.WebMethod()> _
    Public Shared Function GetAgentListSearch(ByVal prefixText As String) As List(Of String)

        Dim strSqlQry As String = ""
        Dim myDS As New DataSet
        Dim lsAgentNames As New List(Of String)
        Dim lsCountryList As String
        Try

            strSqlQry = "select a.agentname, a.ctrycode from agentmast a where a.active=1 and a.agentname like  '%" & Trim(prefixText) & "%'"

            'Dim wc As New PriceListModule_Countrygroup
            'wc = wucCountrygroup
            'lsCountryList = wc.fnGetSelectedCountriesList
            If HttpContext.Current.Session("SelectedCountriesList_WucCountryGroupUserControl") IsNot Nothing Then
                lsCountryList = HttpContext.Current.Session("SelectedCountriesList_WucCountryGroupUserControl").ToString.Trim
                If lsCountryList <> "" Then
                    ' strSqlQry += " and a.ctrycode in (" & lsCountryList & ")"
                End If

            End If

            Dim SqlConn As New SqlConnection
            Dim myDataAdapter As New SqlDataAdapter
            ' SqlConn = clsDBConnect.dbConnectionnew(objclsConnectionName.ConnectionName)
            SqlConn = clsDBConnect.dbConnectionnew("strDBConnection")
            'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)

            If myDS.Tables(0).Rows.Count > 0 Then
                For i As Integer = 0 To myDS.Tables(0).Rows.Count - 1
                    'lsAgentNames.Add(myDS.Tables(0).Rows(i)("agentname").ToString())
                    lsAgentNames.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(myDS.Tables(0).Rows(i)("agentname").ToString(), myDS.Tables(0).Rows(i)("ctrycode").ToString()))
                Next
            End If

            Return lsAgentNames
        Catch ex As Exception

            Return lsAgentNames
        End Try

    End Function
#End Region

#Region "Protected Sub gv_SearchResult_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gv_SearchResult.PageIndexChanging"
    Protected Sub gv_SearchResult_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gv_SearchResult.PageIndexChanging
        gv_SearchResult.PageIndex = e.NewPageIndex

        sortgvsearch()
        'FillGrid("view_partymaxacc_header.tranid", hdnpartycode.Value, "DESC")


    End Sub

#End Region
    Protected Sub btnclear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnclear.Click

        Dim chkSelect As CheckBox
        Dim txtadult As TextBox
        Dim txtchild As TextBox
        Dim txtinfant As TextBox
        Dim txtPriceOccupancy As TextBox
        Dim txtMaxEB As TextBox
        Dim txtMaxOccpncy As TextBox
        Dim txtpricepax As TextBox
        Dim txtmaxcombination As TextBox

        For Each grdRow In gv_FillData.Rows
            chkSelect = CType(grdRow.FindControl("chkSelect"), CheckBox)
            If chkSelect.Checked = True Then
                txtadult = grdRow.FindControl("txtadult")
                txtchild = grdRow.FindControl("txtchild")
                txtinfant = grdRow.FindControl("txtinfant")
                txtPriceOccupancy = grdRow.FindControl("txtMaxocctotal")
                txtMaxEB = grdRow.FindControl("txtMaxEB")
                txtMaxOccpncy = grdRow.FindControl("txtExsuppunit")
                txtpricepax = grdRow.FindControl("txtpricepax")
                txtmaxcombination = grdRow.Findcontrol("txtMaxocccombination")

                txtadult.Text = ""
                txtchild.Text = ""
                txtinfant.Text = ""
                txtPriceOccupancy.Text = ""
                txtMaxEB.Text = ""
                txtMaxOccpncy.Text = ""
                txtpricepax.Text = ""
                txtmaxcombination.Text = ""
            End If
        Next


    End Sub
    Protected Sub btnCopySelected_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCopySelected.Click
        Dim intchkdCount As Integer = 0
        Dim chkSelect As CheckBox

        Dim intadultVal As Integer
        Dim intchildVal As Integer
        Dim intinfantVal As Integer
        Dim intPriceOccupancyVal As Integer
        Dim intMaxEBVal As Integer
        Dim intMaxOccpncyVal As Integer
        Dim strhdnMaxOccupancyVal As String
        Dim intpricepax As Integer
        Dim strrmclassname As String = ""
        Dim strclasscode As String = ""
        Dim str0based As String = ""
        Dim strunit As String = ""



        Dim txtadult As TextBox
        Dim txtchild As TextBox
        Dim txtinfant As TextBox
        Dim txtPriceOccupancy As TextBox
        Dim txtMaxEB As TextBox
        Dim txtMaxOccpncy As TextBox
        Dim txtMaxOccupancycom As TextBox
        Dim txtpricepax As TextBox
        Dim txtclasscode As TextBox
        Dim txtrmclassname As TextBox
        Dim chkstart0based As CheckBox
        Dim txtrankorder As TextBox
        Dim chkunit As CheckBox
        Dim txtactive As TextBox

        For i = 0 To gv_FillData.Rows.Count - 1
            chkSelect = CType(gv_FillData.Rows(i).FindControl("chkSelect"), CheckBox)
            If chkSelect.Checked = True Then
                intchkdCount += 1

            End If
        Next

        If intchkdCount = 0 Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Select any Row To Copy!');", True)

            Exit Sub

        ElseIf intchkdCount > 1 Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Only one row can be selected for copying!');", True)

            Exit Sub
        End If
        For i = 0 To gv_FillData.Rows.Count - 1
            chkSelect = CType(gv_FillData.Rows(i).FindControl("chkSelect"), CheckBox)
            strhdnMaxOccupancyVal = CType(gv_FillData.Rows(i).FindControl("txtMaxocccombination"), TextBox).Text
            intMaxOccpncyVal = Val(CType(gv_FillData.Rows(i).FindControl("txtMaxocctotal"), TextBox).Text)
            If chkSelect.Checked = True Then
                If chkoccupancydetail.Checked = True Then
                    If strhdnMaxOccupancyVal = "" Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Ocupancy detail not featured in the selected row cannot copy ocupancy detail');", True)
                        Exit Sub
                    End If
                End If

                If Val(intMaxOccpncyVal) = 0 Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('maximum ocupancy cannot be 0');", True)
                    Exit Sub
                End If
            End If
        Next


        For Each grdRow In gv_FillData.Rows
            chkSelect = CType(grdRow.FindControl("chkSelect"), CheckBox)
            If chkSelect.Checked = True Then



                intadultVal = Val(CType(grdRow.FindControl("txtadult"), TextBox).Text)
                intchildVal = Val(CType(grdRow.FindControl("txtchild"), TextBox).Text)
                intinfantVal = Val(CType(grdRow.FindControl("txtinfant"), TextBox).Text)
                intPriceOccupancyVal = Val(CType(grdRow.FindControl("txtMaxocctotal"), TextBox).Text)
                intMaxEBVal = Val(CType(grdRow.FindControl("txtMaxEB"), TextBox).Text)
                intMaxOccpncyVal = Val(CType(grdRow.FindControl("txtExsuppunit"), TextBox).Text)
                intpricepax = Val(CType(grdRow.FindControl("txtpricepax"), TextBox).Text)

                strrmclassname = CType(grdRow.FindControl("txtrmclassname"), TextBox).Text
                strclasscode = CType(grdRow.FindControl("txtclasscode"), TextBox).Text
                str0based = IIf(CType(grdRow.FindControl("chkstart0based"), CheckBox).Checked = True, "1", "0")
                strunit = IIf(CType(grdRow.FindControl("chkunit"), CheckBox).Checked = True, "1", "0")

                '  txtrankorder = CType(grdRow.FindControl("txtrankorder"), TextBox)
                'chkactive = CType(grdRow.FindControl("chkactive"), CheckBox)
                'txtactive = CType(grdRow.FindControl("txtactive"), TextBox)


                If chkoccupancydetail.Checked = True Then
                    strhdnMaxOccupancyVal = CType(grdRow.FindControl("txtMaxocccombination"), TextBox).Text
                End If

            End If
        Next
        For Each grdRow In gv_FillData.Rows
            chkSelect = CType(grdRow.FindControl("chkSelect"), CheckBox)
            If chkSelect.Checked = False Then
                txtadult = grdRow.FindControl("txtadult")
                txtchild = grdRow.FindControl("txtchild")
                txtinfant = grdRow.FindControl("txtinfant")
                txtPriceOccupancy = grdRow.FindControl("txtMaxocctotal")
                txtMaxEB = grdRow.FindControl("txtMaxEB")
                txtMaxOccpncy = grdRow.FindControl("txtExsuppunit")
                txtMaxOccupancycom = grdRow.FindControl("txtMaxocccombination")
                txtpricepax = grdRow.Findcontrol("txtpricepax")


                txtrmclassname = grdRow.FindControl("txtrmclassname")
                txtclasscode = grdRow.FindControl("txtclasscode")
                chkstart0based = grdRow.FindControl("chkstart0based")
                chkunit = grdRow.FindControl("chkunit")

                Dim strlblRMTypeCode As String = CType(grdRow.FindControl("lblRoomTypeCode"), Label).Text

                If ddlCopyCriteria.SelectedValue = 0 Then
                    'copy to all
                    txtadult.Text = intadultVal
                    txtchild.Text = intchildVal
                    txtinfant.Text = intinfantVal
                    txtPriceOccupancy.Text = intPriceOccupancyVal
                    txtMaxEB.Text = intMaxEBVal
                    txtMaxOccpncy.Text = intMaxOccpncyVal
                    txtMaxOccupancycom.Text = strhdnMaxOccupancyVal
                    txtpricepax.Text = intpricepax

                    txtrmclassname.Text = strrmclassname
                    txtclasscode.Text = strclasscode
                    chkstart0based.Checked = IIf(str0based = "1", True, False)
                    chkunit.Checked = IIf(strunit = "1", True, False)

                ElseIf ddlCopyCriteria.SelectedValue = 1 Then
                    'copy to same selected room types
                    If ddlCopyRoom.Value <> "[Select]" Then
                        If ddlCopyRoom.Value = strlblRMTypeCode Then
                            txtadult.Text = intadultVal
                            txtchild.Text = intchildVal
                            txtinfant.Text = intinfantVal
                            txtPriceOccupancy.Text = intPriceOccupancyVal
                            txtMaxEB.Text = intMaxEBVal
                            txtMaxOccpncy.Text = intMaxOccpncyVal
                            txtMaxOccupancycom.Text = strhdnMaxOccupancyVal
                            txtpricepax.Text = intpricepax

                            txtrmclassname.Text = strrmclassname
                            txtclasscode.Text = strclasscode
                            chkstart0based.Checked = IIf(str0based = "1", True, False)
                            chkunit.Checked = IIf(strunit = "1", True, False)
                        End If
                    End If





                End If
            End If
        Next



    End Sub

    Private Sub ShowRecordoffer(ByVal RefCode As String)

        Try
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            mySqlCmd = New SqlCommand("Select * from View_partymaxacc_header(nolock) Where tranid='" & RefCode & "'", mySqlConn)
            mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
            If mySqlReader.HasRows Then
                If mySqlReader.Read() = True Then
                    If IsDBNull(mySqlReader("partycode")) = False Then
                        hdnpartycode.Value = CType(mySqlReader("partycode"), String)
                    End If
                    If IsDBNull(mySqlReader("countrygroupsyesno")) = False Then
                        chkctrygrp.Checked = IIf(CType(mySqlReader("countrygroupsyesno"), String) = "1", True, False)
                    Else
                        chkctrygrp.Checked = False
                    End If
                    'If IsDBNull(mySqlReader("promotionid")) = False Then
                    '    txtpromotionid.Text = CType(mySqlReader("promotionid"), String)
                    '    txtpromotionname.Text = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select  pricecode from vw_promotion_header where  promotionid ='" & CType(mySqlReader("promotionid"), String) & "'")
                    'Else
                    '    txtpromotionid.Text = ""
                    '    txtpromotionname.Text = ""
                    'End If

                    lblstatustext.Visible = False
                    lblstatus.Visible = False

                End If
            End If


            If chkctrygrp.Checked = True Then
                divuser.Style("display") = "block"
            Else
                divuser.Style("display") = "none"
            End If

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ContractMaxocc.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
            clsDBConnect.dbReaderClose(mySqlReader)             'sql reader disposed    
            clsDBConnect.dbConnectionClose(mySqlConn)           'connection close           
        End Try
    End Sub
    Private Sub ShowRoomdetailsoffers(ByVal partycode As String, ByVal RefCode As String)
        Try
            Dim myDS As New DataSet
            gv_FillData.Visible = True
            strSqlQry = ""

            txtMaxid.Text = RefCode


            strSqlQry = "sp_partymaxaccomodation_copyoffer'" & CType(partycode, String) & "' , '" & CType(RefCode, String) & "' ,'N','" & CType(hdnpromotionid.Value, String) & "'"





            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, mySqlConn)
            myDataAdapter.Fill(myDS)
            gv_FillData.DataSource = myDS

            If myDS.Tables(0).Rows.Count > 0 Then
                gv_FillData.DataBind()

            Else
                gv_FillData.DataBind()

            End If


            If Session("Calledfrom") = "Offers" Then

                For Each gvrow In gv_FillData.Rows
                    Dim imgbEditnew As ImageButton = gvrow.Findcontrol("imgbEditnew")
                    Dim chkunit As CheckBox = gvrow.FindControl("chkunit")
                    Dim txtrmclassname As TextBox = gvrow.Findcontrol("txtrmclassname")
                    Dim txtrankorder As TextBox = gvrow.FindControl("txtrankorder")
                    Dim chkactive As CheckBox = gvrow.Findcontrol("chkactive")

                    imgbEditnew.Visible = False
                    chkunit.Enabled = False
                    txtrmclassname.Enabled = False
                    txtrankorder.Enabled = False
                    chkactive.Enabled = False

                Next

                btnAddrow.Style.Add("display", "none")
                btndeleterow.Style.Add("display", "none")
                btnaddroomclass.Style.Add("display", "none")

            End If


        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ContractMaxocc.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
            clsDBConnect.dbConnectionClose(mySqlConn)                          'Close connection
        End Try
    End Sub
    Private Sub ShowRecord(ByVal RefCode As String)

        Try
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            mySqlCmd = New SqlCommand("Select * from View_partymaxacc_header Where tranid='" & RefCode & "'", mySqlConn)
            mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
            If mySqlReader.HasRows Then
                If mySqlReader.Read() = True Then
                    If IsDBNull(mySqlReader("partycode")) = False Then
                        hdnpartycode.Value = CType(mySqlReader("partycode"), String)
                    End If
                    If IsDBNull(mySqlReader("countrygroupsyesno")) = False Then
                        chkctrygrp.Checked = IIf(CType(mySqlReader("countrygroupsyesno"), String) = "1", True, False)
                    Else
                        chkctrygrp.Checked = False
                    End If
                    If IsDBNull(mySqlReader("promotionid")) = False Then
                        txtpromotionid.Text = CType(mySqlReader("promotionid"), String)
                        txtpromotionname.Text = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select  pricecode from vw_promotion_header where  promotionid ='" & CType(mySqlReader("promotionid"), String) & "'")
                    Else
                        txtpromotionid.Text = ""
                        txtpromotionname.Text = ""
                    End If

                    If IsDBNull(mySqlReader("status")) = False Then
                        lblstatustext.Visible = True
                        lblstatus.Visible = True
                        lblstatus.Text = IIf(mySqlReader("status").ToString.ToUpper = "YES", "APPROVED", "UNAPPROVED")


                    End If

                End If
            End If


            If chkctrygrp.Checked = True Then
                divuser.Style("display") = "block"
            Else
                divuser.Style("display") = "none"
            End If

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ContractMaxocc.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
            clsDBConnect.dbReaderClose(mySqlReader)             'sql reader disposed    
            clsDBConnect.dbConnectionClose(mySqlConn)           'connection close           
        End Try
    End Sub
    Private Sub ShowRoomdetails(ByVal partycode As String, ByVal RefCode As String)
        Try
            Dim myDS As New DataSet
            gv_FillData.Visible = True
            strSqlQry = ""

            txtMaxid.Text = RefCode

            If Session("Calledfrom") = "Offers" Then
                strSqlQry = "sp_partymaxaccomodation'" & CType(partycode, String) & "' , '" & CType(RefCode, String) & "' ,'P','" & CType(hdnpromotionid.Value, String) & "'"
            Else
                strSqlQry = "sp_partymaxaccomodation'" & CType(partycode, String) & "' , '" & CType(RefCode, String) & "','N','' "
            End If

            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, mySqlConn)
            myDataAdapter.Fill(myDS)
            gv_FillData.DataSource = myDS

            If myDS.Tables(0).Rows.Count > 0 Then
                gv_FillData.DataBind()

            Else
                gv_FillData.DataBind()

            End If


            If Session("Calledfrom") = "Offers" Then

                For Each gvrow In gv_FillData.Rows
                    Dim imgbEditnew As ImageButton = gvrow.Findcontrol("imgbEditnew")
                    Dim chkunit As CheckBox = gvrow.FindControl("chkunit")
                    Dim txtrmclassname As TextBox = gvrow.Findcontrol("txtrmclassname")
                    Dim txtrankorder As TextBox = gvrow.FindControl("txtrankorder")
                    Dim chkactive As CheckBox = gvrow.Findcontrol("chkactive")

                    imgbEditnew.Visible = False
                    chkunit.Enabled = False
                    txtrmclassname.Enabled = False
                    txtrankorder.Enabled = False
                    chkactive.Enabled = False

                Next

            End If


        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ContractMaxocc.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
            clsDBConnect.dbConnectionClose(mySqlConn)                          'Close connection
        End Try
    End Sub

    <System.Web.Script.Services.ScriptMethod()> _
  <System.Web.Services.WebMethod()> _
    Public Shared Function Getpromotionlist(ByVal prefixText As String, ByVal contextKey As String) As List(Of String)

        Dim strSqlQry As String = ""
        Dim myDS As New DataSet
        Dim promotionlist As New List(Of String)
        Dim maxstate As String
        Try
            ' contextKey = Convert.ToString(HttpContext.Current.Session("partycode").ToString())
            maxstate = Convert.ToString(HttpContext.Current.Session("State").ToString())

            If CType(maxstate, String) = "Edit" Then
                strSqlQry = "select  pricecode,promotionid from vw_promotion_header where  isnull(pricecode,'')<>'' and  partycode='" & contextKey & "' and pricecode like  '" & prefixText & "%'"
            Else
                strSqlQry = "select  pricecode,promotionid from vw_promotion_header where  isnull(pricecode,'')<>'' and active=1 and convert(varchar(10),frmdate,111) >=GETDATE() and partycode='" & contextKey & "' and pricecode like  '" & prefixText & "%'"
            End If

            Dim SqlConn As New SqlConnection
            Dim myDataAdapter As New SqlDataAdapter
            ' SqlConn = clsDBConnect.dbConnectionnew(objclsConnectionName.ConnectionName)
            SqlConn = clsDBConnect.dbConnectionnew("strDBConnection")
            'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)

            If myDS.Tables(0).Rows.Count > 0 Then
                For i As Integer = 0 To myDS.Tables(0).Rows.Count - 1
                    ' promotionlist.Add(myDS.Tables(0).Rows(i)("pricecode").ToString())
                    promotionlist.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(myDS.Tables(0).Rows(i)("pricecode").ToString(), myDS.Tables(0).Rows(i)("promotionid").ToString()))
                Next

            End If

            Return promotionlist
        Catch ex As Exception
            Return promotionlist
        End Try

    End Function

    <System.Web.Script.Services.ScriptMethod()> _
<System.Web.Services.WebMethod()> _
    Public Shared Function Getroomclasslist(ByVal prefixText As String) As List(Of String)

        Dim strSqlQry As String = ""
        Dim myDS As New DataSet
        Dim roomclasslist As New List(Of String)

        Try



            strSqlQry = "select roomclassname,roomclasscode from  room_classification where active=1  and  roomclassname like  '" & Trim(prefixText) & "%' order by roomclassname "


            Dim SqlConn As New SqlConnection
            Dim myDataAdapter As New SqlDataAdapter
            ' SqlConn = clsDBConnect.dbConnectionnew(objclsConnectionName.ConnectionName)
            SqlConn = clsDBConnect.dbConnectionnew("strDBConnection")
            'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)

            If myDS.Tables(0).Rows.Count > 0 Then
                For i As Integer = 0 To myDS.Tables(0).Rows.Count - 1
                    'roomclasslist.Add(myDS.Tables(0).Rows(i)("roomclassname").ToString())
                    roomclasslist.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(myDS.Tables(0).Rows(i)("roomclassname").ToString(), myDS.Tables(0).Rows(i)("roomclasscode").ToString()))
                Next

            End If

            Return roomclasslist
        Catch ex As Exception
            Return roomclasslist
        End Try

    End Function

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
#Region "Public Sub fillDategrdnew(ByVal grd As GridView, Optional ByVal blnload As Boolean = False, Optional ByVal count As Integer = 1)"
    Public Sub fillDategrdnew(ByVal grd As GridView, Optional ByVal blnload As Boolean = False, Optional ByVal count As Integer = 1)
        Dim lngcnt As Long
        If blnload = True Then
            lngcnt = 1
        Else
            lngcnt = count
        End If

        grd.DataSource = CreateDataSource(lngcnt)
        grd.DataBind()
        grd.Visible = True
    End Sub
#End Region
#Region "Public Sub fillgrd(ByVal grd As GridView, Optional ByVal blnload As Boolean = False, Optional ByVal count As Integer = 1)"
    Public Sub fillDategrd(ByVal grd As GridView, Optional ByVal blnload As Boolean = False, Optional ByVal count As Integer = 1)
        'Dim lngcnt As Long
        'If blnload = True Then
        '    lngcnt = 1
        'Else
        '    lngcnt = count
        'End If

        'grd.DataSource = CreateDataSource(lngcnt)
        'grd.DataBind()
        'grd.Visible = True
        Dim lngcnt As Long
        Dim cnt As Integer
        If ViewState("State") = "New" Then
            cnt = 1
        Else
            If Session("RefCode") <> "" Then
                mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))

                strSqlQry = "select count(*) from partymaxaccom_dates where tranid='" + Session("RefCode") + "'"

                mySqlCmd = New SqlCommand(strSqlQry, mySqlConn)
                cnt = mySqlCmd.ExecuteScalar
                mySqlConn.Close()
            End If
        End If

        If blnload = True Then
            lngcnt = IIf(cnt = 0, 1, cnt) '10
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
    Protected Sub btnAddLinesDates_Click1(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddLinesDates.Click


        'Dim dt As New DataTable
        'Dim dr As DataRow

        'Dim gvRow As GridViewRow

        'dt.Columns.Add(New DataColumn("Frmdate", GetType(String)))
        'dt.Columns.Add(New DataColumn("todate", GetType(String)))

        'For Each gvRow In grdDates.Rows

        '    dr = dt.NewRow



        '    Dim txtfromdate As TextBox = GVRow.FindControl("txtfromDate")
        '    Dim txttodate As TextBox = GVRow.FindControl("txtToDate")

        '    dr("Frmdate") = txtfromdate.Text
        '    dr("todate") = txttodate.Text
        '    dt.Rows.Add(dr)
        'Next

        'dr = dt.NewRow
        'dt.Rows.Add(dr)
        'grdDates.DataSource = dt
        'grdDates.DataBind()


        Dim count As Integer
        Dim GVRow As GridViewRow
        count = grdDates.Rows.Count + 1
        Dim fDate(count) As String
        Dim tDate(count) As String
        Dim n As Integer = 0
        Dim dpFDate As TextBox
        Dim dpTDate As TextBox

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
            Dim gridNewrow As GridViewRow
            gridNewrow = grdDates.Rows(grdDates.Rows.Count - 1)
            Dim strRowId As String = gridNewrow.ClientID
            Dim strGridName As String = grdDates.ClientID
            Dim strFoucsColumnIndex As String = "0"
            Dim str As String = String.Format("javascript:LastSelectRow('" + strRowId + "','" + CType(grdDates.Rows.Count - 1, String) + "','" + strGridName + "','" + strFoucsColumnIndex + "');")
            ScriptManager.RegisterStartupScript(Page, GetType(Page), "Script", str, True)
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            ' .writerrorlog(Server.MapPath("Errorlog.txt"), ex.ToString, 1, 1)
        End Try



    End Sub
    Private Sub Createdatacolumns(ByVal mode As String)

        Dim dt As New DataTable
        Dim dr As DataRow

        Dim gvRow As GridViewRow

        dt.Columns.Add(New DataColumn("RoomImages", GetType(String))) ''*** Danny 8/3/18

        dt.Columns.Add(New DataColumn("rmtypcode", GetType(String)))
        dt.Columns.Add(New DataColumn("rmtypname", GetType(String)))
        dt.Columns.Add(New DataColumn("roomclasscode", GetType(String)))
        dt.Columns.Add(New DataColumn("roomclassname", GetType(String)))
        dt.Columns.Add(New DataColumn("unityesno", GetType(String)))
        dt.Columns.Add(New DataColumn("pricepax", GetType(String)))
        dt.Columns.Add(New DataColumn("maxadults", GetType(String)))
        dt.Columns.Add(New DataColumn("maxchilds", GetType(String)))
        dt.Columns.Add(New DataColumn("maxinfant", GetType(String)))
        dt.Columns.Add(New DataColumn("maxeb", GetType(String)))
        dt.Columns.Add(New DataColumn("noofextraperson", GetType(String)))
        dt.Columns.Add(New DataColumn("maxoccupancy", GetType(String)))
        dt.Columns.Add(New DataColumn("start0based", GetType(String)))
        dt.Columns.Add(New DataColumn("combinations", GetType(String)))
        dt.Columns.Add(New DataColumn("rankord", GetType(String)))
        dt.Columns.Add(New DataColumn("inactive", GetType(String)))
        dt.Columns.Add(New DataColumn("combinationscat", GetType(String)))


        For Each gvRow In gv_FillData.Rows

            dr = dt.NewRow

            Dim chkSelect As CheckBox = gvRow.FindControl("chkSelect")
            If mode.Trim.ToUpper <> "DELETE" Or (mode.Trim.ToUpper = "DELETE" And chkSelect.Checked = False) Then




                Dim txtadult As TextBox = gvRow.FindControl("txtadult")
                Dim txtchild As TextBox = gvRow.FindControl("txtchild")
                Dim txtinfant As TextBox = gvRow.FindControl("txtinfant")
                Dim txtPriceOccupancy As TextBox = gvRow.FindControl("txtMaxocctotal")
                Dim txtMaxEB As TextBox = gvRow.FindControl("txtMaxEB")
                Dim txtMaxOccpncy As TextBox = gvRow.FindControl("txtExsuppunit")
                Dim txtMaxOccupancycom As TextBox = gvRow.FindControl("txtMaxocccombination")
                Dim txtpricepax As TextBox = gvRow.FindControl("txtpricepax")

                Dim txtrmtype As TextBox = gvRow.FindControl("txtrmtype")
                Dim txtrmtypename As TextBox = gvRow.FindControl("txtrmtypename")

                Dim txtrmclasscode As TextBox = gvRow.FindControl("txtrmclasscode")
                Dim txtclasscodenew As TextBox = gvRow.FindControl("txtclasscode")

                Dim txtrmclassname As TextBox = gvRow.FindControl("txtrmclassname")
                Dim lblunit As Label = gvRow.FindControl("lblunit")
                Dim chkstart0based As CheckBox = gvRow.FindControl("chkstart0based")
                Dim txtrankorder As TextBox = gvRow.FindControl("txtrankorder")
                Dim txtactive As TextBox = gvRow.FindControl("txtactive")
                Dim txtcombinationcat As TextBox = gvRow.FindControl("txtcombinationcat")


                Dim sRoomImages As ImageButton = gvRow.FindControl("RoomImages") ''*** Danny 8/3/18
                dr("RoomImages") = sRoomImages.ImageUrl  ''*** Danny 8/3/18



                dr("rmtypcode") = txtrmtype.Text
                dr("rmtypname") = txtrmtypename.Text
                If txtclasscodenew.Text <> "" And txtclasscodenew.Text <> txtrmclasscode.Text Then
                    dr("roomclasscode") = txtclasscodenew.Text
                Else
                    dr("roomclasscode") = txtrmclasscode.Text
                End If

                dr("roomclassname") = txtrmclassname.Text
                dr("unityesno") = IIf(txtrmtypename.Text = "", "", lblunit.Text)
                dr("pricepax") = txtpricepax.Text
                dr("maxadults") = txtadult.Text
                dr("maxchilds") = txtchild.Text
                dr("maxinfant") = txtinfant.Text
                dr("maxeb") = txtMaxEB.Text
                dr("noofextraperson") = txtMaxOccpncy.Text
                dr("maxoccupancy") = txtPriceOccupancy.Text
                dr("start0based") = IIf(chkstart0based.Checked = False, 0, 1)
                dr("combinations") = txtMaxOccupancycom.Text
                dr("rankord") = txtrankorder.Text
                dr("inactive") = txtactive.Text
                dr("combinationscat") = txtcombinationcat.Text

                dt.Rows.Add(dr)
            End If


        Next

        If mode.Trim.ToUpper = "ADD" Then
            dr = dt.NewRow
            dr("RoomImages") = "../images/ImageIco.png" ''*** Danny 8/3/18
            dt.Rows.Add(dr)

        End If

        gv_FillData.DataSource = dt
        gv_FillData.DataBind()

    End Sub


    Private Sub Createdatacolumnschild(ByVal mode As String)

        Dim dt As New DataTable
        Dim dr As DataRow

        Dim gvRow As GridViewRow
        Dim chkSelect As HtmlInputCheckBox
        Dim ddlRmcat As HtmlSelect
        dt.Columns.Add(New DataColumn("adult", GetType(String)))
        dt.Columns.Add(New DataColumn("child", GetType(String)))
        dt.Columns.Add(New DataColumn("rmcatgory", GetType(String)))

        For Each gvRow In gvOccupancy.Rows

            dr = dt.NewRow

            chkSelect = gvRow.FindControl("chk")
            If mode.Trim.ToUpper <> "DELETE" Or (mode.Trim.ToUpper = "DELETE" And chkSelect.Checked = False) Then

                ddlRmcat = gvRow.FindControl("ddlRmcat")



                dr("adult") = gvRow.Cells(1).Text
                dr("child") = gvRow.Cells(2).Text
                dr("rmcatgory") = ddlRmcat.Value

                dt.Rows.Add(dr)
            End If


        Next

        If mode.Trim.ToUpper = "ADD" Then
            dr = dt.NewRow
            dt.Rows.Add(dr)

        End If

        gvOccupancy.DataSource = dt
        gvOccupancy.DataBind()

        'For Each gvRow In gvOccupancy.Rows

        '    ddlRmcat = gvRow.FindControl("ddlRmcat")
        '    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlRmcat, "rmcatcode", "rmcatcode", "select rmcatcode,rmcatcode from  rmcatmast  where  isnull(allotreqd,'')='Yes' and active=1  order by isnull(rankorder,99)", True)

        'Next


        Dim rowIndex As Integer = 0
        If dt.Rows.Count > 0 Then

            For Each gvRow In gvOccupancy.Rows
                ddlRmcat = gvRow.FindControl("ddlRmcat")
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlRmcat, "rmcatcode", "rmcatcode", "select pr.rmcatcode,pr.rmcatcode from  rmcatmast r,partyrmcat pr  where  r.rmcatcode=pr.rmcatcode and  " _
                                                 & " pr.partycode='" & hdnpartycode.Value & "' and   isnull(r.allotreqd,'')='Yes' and r.active=1  order by isnull(r.rankorder,99)", True)
                For i As Integer = 0 To dt.Rows.Count - 1
                    If gvRow.Cells(1).Text = dt.Rows(i)("adult").ToString() And gvRow.Cells(2).Text = dt.Rows(i)("child").ToString() Then
                        ddlRmcat.Value = dt.Rows(i)("rmcatgory").ToString()
                        Exit For
                    End If
                Next

            Next

        End If


        ModalPopupOccupancy.Show()


    End Sub

    Protected Sub btnAddrow_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddrow.Click

        Createdatacolumns("add")
        Dim gridNewrow As GridViewRow
        gridNewrow = gv_FillData.Rows(gv_FillData.Rows.Count - 1)
        Dim strRowId As String = gridNewrow.ClientID
        Dim strGridName As String = gv_FillData.ClientID
        Dim strFoucsColumnIndex As String = "5"
        Dim str As String = String.Format("javascript:LastSelectRow('" + strRowId + "','" + CType(gv_FillData.Rows.Count - 1, String) + "','" + strGridName + "','" + strFoucsColumnIndex + "');")
        ScriptManager.RegisterStartupScript(Page, GetType(Page), "Script", str, True)


    End Sub



    Protected Sub grdDates_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdDates.RowDataBound
        Try
            If e.Row.RowType = DataControlRowType.DataRow AndAlso (e.Row.RowState = DataControlRowState.Normal OrElse e.Row.RowState = DataControlRowState.Alternate) Then
                Dim strGridName As String = grdDates.ClientID
                Dim strRowId As String = e.Row.RowIndex
                Dim strFoucsColumnIndex = "0"
                e.Row.Attributes("onclick") = "javascript:SelectRow(this,'" + strRowId + "','" + strGridName + "','" + strFoucsColumnIndex + "');"
                e.Row.Attributes("onkeydown") = "javascript:return SelectSibling(event,'" + strGridName + "','" + strFoucsColumnIndex + "',this);"
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        End Try
    End Sub
    Private Sub FillGrid(ByVal strorderby As String, ByVal partycode As String, Optional ByVal strsortorder As String = "ASC")

        Dim myDS As New DataSet

        gv_SearchResult.Visible = True


        If gv_SearchResult.PageIndex < 0 Then
            gv_SearchResult.PageIndex = 0
        End If



        strSqlQry = ""
        Try

            ' If ViewState("Menucalling") = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select option_selected  from reservation_parameters where param_id=1138") Then
            If Session("Calledfrom") <> "Offers" Then

                strSqlQry = "SELECT view_partymaxacc_header.tranid,convert(varchar(10),view_partymaxacc_header.frmdate,103) frmdate,convert(varchar(10),view_partymaxacc_header.todate,103) todate," & _
              "ISNULL(view_partymaxacc_header.countrygroups,'') countrygroups,'' promotionid,'' promotionname, " & _
              "view_partymaxacc_header.status, view_partymaxacc_header.adddate, view_partymaxacc_header.adduser,view_partymaxacc_header.moddate,view_partymaxacc_header.moduser FROM view_partymaxacc_header " & _
                      " where  view_partymaxacc_header.partycode='" & partycode & "' and ISNULL(promotionid,'')='' order by  " & strorderby & "  " & strsortorder & ""


            Else
                strSqlQry = "SELECT view_partymaxacc_header.tranid,convert(varchar(10),view_partymaxacc_header.frmdate,103) frmdate,convert(varchar(10),view_partymaxacc_header.todate,103) todate," & _
              "ISNULL(view_partymaxacc_header.countrygroups,'') countrygroups,ISNULL(view_partymaxacc_header.promotionid,'') promotionid,ISNULL(view_offers_header.promotionname,'') promotionname, " & _
              "view_partymaxacc_header.status,view_partymaxacc_header.adddate, view_partymaxacc_header.adduser,view_partymaxacc_header.moddate,view_partymaxacc_header.moduser FROM view_partymaxacc_header " & _
                      "left JOIN view_offers_header ON view_partymaxacc_header.promotionid=view_offers_header.promotionid where view_partymaxacc_header.promotionid='" & hdnpromotionid.Value & "' and    view_partymaxacc_header.partycode='" & partycode & "' order by  " & strorderby & ""


            End If


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
                'btnAddNew.Enabled = False
                'btnAddNew.BackColor = Gray
            Else
                gv_SearchResult.PageIndex = 0
                gv_SearchResult.DataBind()

            End If

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ContractMaxOcc.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
            clsDBConnect.dbConnectionClose(SqlConn)                          'Close connection
        End Try
    End Sub
    <System.Web.Script.Services.ScriptMethod()> _
<System.Web.Services.WebMethod()> _
    Public Shared Function GetSeasonlist(ByVal prefixText As String) As List(Of String)

        Dim strSqlQry As String = ""
        Dim myDS As New DataSet
        Dim Hotelnames As New List(Of String)
        Try

            strSqlQry = "select distinct seasonname from view_contractseasons"
            Dim SqlConn As New SqlConnection
            Dim myDataAdapter As New SqlDataAdapter
            ' SqlConn = clsDBConnect.dbConnectionnew(objclsConnectionName.ConnectionName)
            SqlConn = clsDBConnect.dbConnectionnew("strDBConnection")
            'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)

            If myDS.Tables(0).Rows.Count > 0 Then
                For i As Integer = 0 To myDS.Tables(0).Rows.Count - 1
                    Hotelnames.Add(myDS.Tables(0).Rows(i)("seasonname").ToString())
                Next

            End If

            Return Hotelnames
        Catch ex As Exception
            Return Hotelnames
        End Try

    End Function

    Public Function checkForDuplicate() As Boolean
        Dim checkres As String
        checkres = ""

        If ViewState("MaxaccState") = "New" Then

            For Each gvRow In grdDates.Rows
                'dpFDate = gvRow.FindControl("FrmDate")
                'dpTDate = gvRow.FindControl("ToDate")
                Dim txtFromDate As TextBox = gvRow.FindControl("txtfromdate")
                Dim txtToDate As TextBox = gvRow.FindControl("txttodate")
                If grdDates.Rows.Count > 0 And txtFromDate.Text = "" And txtToDate.Text = "" Then

                    strSqlQry = "select 't' from view_partymaxacc_header(nolock) where  partycode='" & hdnpartycode.Value & "'"
                    checkres = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), strSqlQry)
                    If checkres = "t" Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Max Accomodation is already defined for the same Supplier');", True)
                        checkForDuplicate = False
                        Exit Function
                    End If
                End If
            Next


        End If
        checkForDuplicate = True
    End Function
    Private Function ValidateSave() As Boolean
        Dim gvRow As GridViewRow


        Dim lnCnt As Integer = 0

        Dim txtchild As TextBox

        Dim ToDt As Date = Nothing
        Dim flgdt As Boolean = False
        Dim maxocccombination As String
        For Each gvRow In gv_FillData.Rows
            lnCnt += 1



            txtchild = gvRow.FindControl("txtchild")
            Dim txtadult As TextBox = gvRow.FindControl("txtadult")
            Dim txtPriceOccupancy As TextBox = gvRow.FindControl("txtMaxocctotal")
            Dim txtMaxOccpncy As TextBox = gvRow.FindControl("txtExsuppunit")
            Dim txtMaxOccupancycom As TextBox = gvRow.FindControl("txtMaxocccombination")
            Dim txtpricepax As TextBox = gvRow.FindControl("txtpricepax")
            Dim hdncombination As HiddenField = gvRow.FindControl("hdncombination")
            Dim txtrmtypename As TextBox = gvRow.FindControl("txtrmtypename")


            Dim txtrmclasscode As TextBox = gvRow.FindControl("txtrmclasscode")


            Dim txtrmclasscodenew As TextBox = gvRow.FindControl("txtclasscode")

            Dim txtcombinationcat As TextBox = gvRow.FindControl("txtcombinationcat")

            If txtadult.Text <> "" Then



                If Val(txtadult.Text) > 0 Then

                    If (Val(txtPriceOccupancy.Text) > 0) = False Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Max Total Occupancy cannot be 0 in line no :" & lnCnt & "');", True)
                        ValidateSave = False
                        Exit Function
                    End If

                    'If (Val(txtMaxOccpncy.Text) > 0) = False Then
                    '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Max Occupancy should be entered in line no :" & lnCnt & "');", True)
                    '    ValidateSave = False
                    '    Exit Function
                    'End If
                    If txtMaxOccupancycom.Text = "" And hdncombination.Value = "" Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Occupancy Combination not Selected  in line no :" & lnCnt & "');", True)
                        ValidateSave = False
                        Exit Function
                    End If

                    If txtrmclasscode.Text = "" And txtrmclasscodenew.Text = "" Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Room Classification not Entered  in line no :" & lnCnt & "');", True)
                        ValidateSave = False
                        Exit Function
                    End If

                    If txtcombinationcat.Text.Trim <> "" Then
                        maxocccombination = txtcombinationcat.Text
                        Dim arrOcpncy As String() = maxocccombination.ToString.Trim.Split(",")

                        For i = 0 To arrOcpncy.Length - 1
                            If arrOcpncy(i) <> "" Then
                                Dim arrAdultChild As String() = arrOcpncy(i).Split("/")
                                If CType(arrAdultChild(2), String) = "" Then
                                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Accomodation Category not Entered  in line no :" & lnCnt & "');", True)
                                    ValidateSave = False
                                    Exit Function
                                End If
                            End If
                        Next
                    Else
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Accomodation Category not Entered  in line no :" & lnCnt & "');", True)
                        ValidateSave = False
                        Exit Function

                    End If
                Else
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Max Adults cannot be 0 in line no :" & lnCnt & "');", True)
                    ValidateSave = False
                    Exit Function

                End If
            Else
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Max Adults cannot be 0 in line no :" & lnCnt & "');", True)
                ValidateSave = False
                Exit Function
            End If

        Next

        '--------------------------------------------- Validate Date Grid
        For Each gvRow In grdDates.Rows
            'dpFDate = gvRow.FindControl("FrmDate")
            'dpTDate = gvRow.FindControl("ToDate")
            Dim txtFromDate As TextBox = gvRow.FindControl("txtfromdate")
            Dim txtToDate As TextBox = gvRow.FindControl("txttodate")
            ' If grdDates.Rows.Count > 0 And txtFromDate.Text <> "" And txtToDate.Text <> "" Then
            If grdDates.Rows.Count > 0 Then
                If txtFromDate.Text <> "" And txtToDate.Text <> "" Then

                    If Left(Right(txtFromDate.Text, 4), 2) <> "20" Or Left(Right(txtToDate.Text, 4), 2) <> "20" Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Enter From Date and To Date Belongs to 21 st century  ');", True)
                        ValidateSave = False
                        SetFocus(txtFromDate)
                        Exit Function
                    End If

                    If ObjDate.ConvertDateromTextBoxToDatabase(txtToDate.Text) <= ObjDate.ConvertDateromTextBoxToDatabase(txtFromDate.Text) Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('All the To Dates should be greater than From Dates.');", True)
                        SetFocus(txtToDate)
                        txtToDate.Text = ""
                        ValidateSave = False
                        Exit Function
                    End If
                    If ToDt <> Nothing Then
                        If ObjDate.ConvertDateromTextBoxToDatabase(txtFromDate.Text) <= ToDt Then
                            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Date Overlapping.');", True)
                            SetFocus(txtFromDate)
                            ValidateSave = False
                            Exit Function
                        End If
                    End If
                    ToDt = ObjDate.ConvertDateromTextBoxToDatabase(txtToDate.Text)
                    flgdt = True


                    If Format(CType(txtFromDate.Text, Date), "yyyy/MM/dd") > ObjDate.ConvertDateromTextBoxToDatabase(hdncontodate.Value) Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert(' From Date Should belongs to the Contracts Period   " & hdnconfromdate.Value & " to  " & hdncontodate.Value & "  ');", True)
                        txtFromDate.Text = ""
                        SetFocus(txtFromDate)
                        ValidateSave = False
                        Exit Function
                    End If
                    If Format(CType(txtFromDate.Text, Date), "yyyy/MM/dd") < Format(CType(ObjDate.ConvertDateromTextBoxToDatabase(hdnconfromdate.Value), Date), "yyyy/MM/dd") Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert(' From Date Should belong to the Contracts Period   " & hdnconfromdate.Value & " to  " & hdncontodate.Value & " ');", True)
                        txtFromDate.Text = ""
                        SetFocus(txtFromDate)
                        ValidateSave = False
                        Exit Function
                    End If

                    If (Format(CType(txtToDate.Text, Date), "yyyy/MM/dd") > Format(CType(ObjDate.ConvertDateromTextBoxToDatabase(hdncontodate.Value), Date), "yyyy/MM/dd")) Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert(' To Date Should belong to the Contracts Period   " & hdnconfromdate.Value & " to  " & hdncontodate.Value & " ');", True)
                        txtToDate.Text = ""
                        SetFocus(txtToDate)
                        ValidateSave = False
                        Exit Function
                    End If



                ElseIf txtFromDate.Text <> "" And txtToDate.Text = "" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Enter To Date.');", True)
                    SetFocus(txtToDate)
                    ValidateSave = False
                    Exit Function
                ElseIf txtFromDate.Text = "" And txtToDate.Text <> "" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Enter From Date.');", True)
                    SetFocus(txtFromDate)
                    ValidateSave = False
                    Exit Function
                End If

            End If
        Next
        'If flgdt = False Then
        '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Dates grid should not be blank.');", True)
        '    SetFocus(grdDates)
        '    ValidateSave = False

        '    Exit Function
        'End If


        ValidateSave = True
    End Function
    Public Function checkforexisting() As Boolean

        checkforexisting = True
        Try
            If FindDatePeriod() = False Then
                checkforexisting = False
                Exit Function
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Error Description: - " & ex.Message & "');", True)
            objUtils.WritErrorLog("ContractMaxocc.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try


    End Function

    Public Function FindDatePeriod() As Boolean
        Dim GVRow As GridViewRow

        Dim strMsg As String = ""
        FindDatePeriod = True
        Try

            Session("CountryList") = Nothing
            Session("AgentList") = Nothing

            Session("CountryList") = wucCountrygroup.checkcountrylist
            Session("AgentList") = wucCountrygroup.checkagentlist

            If dpTxtFromDate.Text <> "" And dptxtTodate.Text <> "" Then



                'For Each GVRow In grdDates.Rows
                '    Dim txtfromdate As TextBox = GVRow.FindControl("txtfromdate")
                '    Dim txttodate As TextBox = GVRow.FindControl("txttodate")

                '    If txtfromdate.Text <> "" And txttodate.Text <> "" Then
                '        Dim ds As DataSet
                '        Dim parms2 As New List(Of SqlParameter)
                '        Dim parm2(8) As SqlParameter
                '        parm2(0) = New SqlParameter("@partycode", CType(hdnpartycode.Value, String))
                '        parm2(1) = New SqlParameter("@fromdate", Format(CType(txtfromdate.Text, Date), "yyyy/MM/dd"))
                '        parm2(2) = New SqlParameter("@todate", Format(CType(txttodate.Text, Date), "yyyy/MM/dd"))
                '        parm2(3) = New SqlParameter("@mode", CType(ViewState("State"), String))
                '        parm2(4) = New SqlParameter("@tranid", CType(txtMaxid.Text.Trim, String))
                '        parm2(5) = New SqlParameter("@country", IIf(chkctrygrp.Checked = True, CType(Session("CountryList"), String), ""))
                '        parm2(6) = New SqlParameter("@agent", IIf(chkctrygrp.Checked = True, CType(Session("AgentList"), String), ""))
                '        parm2(7) = New SqlParameter("@promotionid", CType(hdnpromotionid.Value, String))

                '        'For i = 0 To 7
                '        '    parms2.Add(parms2(i))
                '        'Next


                '        parms2.Add(parm2(0))
                '        parms2.Add(parm2(1))
                '        parms2.Add(parm2(2))
                '        parms2.Add(parm2(3))
                '        parms2.Add(parm2(4))
                '        parms2.Add(parm2(5))
                '        parms2.Add(parm2(6))
                '        parms2.Add(parm2(7))


                '        ds = New DataSet()
                '        ds = objUtils.ExecuteQuerynew(Session("dbconnectionName"), "sp_duplicate_maxaccom_withdates", parms2)


                '        If ds.Tables.Count > 0 Then
                '            If ds.Tables(0).Rows.Count > 0 Then
                '                If IsDBNull(ds.Tables(0).Rows(0)("tranid")) = False Then
                '                    strMsg = "Max accomodation already exists For this Supplier " + ds.Tables(0).Rows(0)("tranid") + " - Country " + ds.Tables(0).Rows(0)("ctryname") + " - Agent - " + ds.Tables(0).Rows(0)("agentname")
                '                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & strMsg & "' );", True)
                '                    ' ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Max accomodation already exists For this Period ');", True)
                '                    FindDatePeriod = False
                '                    Exit Function
                '                End If
                '            End If
                '        End If
                '    End If
                'Next

            Else

                Dim ds As DataSet
                Dim parms3 As New List(Of SqlParameter)
                Dim parm3(6) As SqlParameter
                parm3(0) = New SqlParameter("@partycode", CType(hdnpartycode.Value, String))
                parm3(1) = New SqlParameter("@country", IIf(chkctrygrp.Checked = True, CType(Session("CountryList"), String), ""))
                parm3(2) = New SqlParameter("@agent", IIf(chkctrygrp.Checked = True, CType(Session("AgentList"), String), ""))
                parm3(3) = New SqlParameter("@promotionid", CType(hdnpromotionid.Value, String))
                parm3(4) = New SqlParameter("@mode", CType(ViewState("State"), String))
                parm3(5) = New SqlParameter("@tranid", CType(txtMaxid.Text.Trim, String))

                'For i = 0 To 5
                '    parms3.Add(parm3(i))
                'Next

                parms3.Add(parm3(0))
                parms3.Add(parm3(1))
                parms3.Add(parm3(2))
                parms3.Add(parm3(3))
                parms3.Add(parm3(4))
                parms3.Add(parm3(5))

                ds = New DataSet()
                ds = objUtils.ExecuteQuerynew(Session("dbconnectionName"), "sp_maxocc_duplicate", parms3)


                If ds.Tables.Count > 0 Then
                    If ds.Tables(0).Rows.Count > 0 Then
                        If IsDBNull(ds.Tables(0).Rows(0)("tranid")) = False Then
                            'strMsg = "Max accomodation already exists For this Supplier " + ds.Tables(0).Rows(0)("tranid") + " - Country " + ds.Tables(0).Rows(0)("ctryname") + " - Agent - " + ds.Tables(0).Rows(0)("agentname")

                            strMsg = "Max accomodation already exists For this Supplier " + ds.Tables(0).Rows(0)("tranid")
                            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & strMsg & "' );", True)
                            ' ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Max accomodation already exists For this Supplier Please check this transaction   ');", True)
                            FindDatePeriod = False
                            Exit Function
                        End If
                    End If
                End If
            End If



        Catch ex As Exception
            FindDatePeriod = False
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Error Description: - " & ex.Message & "');", True)
            objUtils.WritErrorLog("ContractMaxocc.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try


    End Function
#Region "SetDate"
    Public Sub SetDate()
        Try
            Dim mCount As Integer = 0
            For i As Integer = 0 To grdDates.Rows.Count - 1
                Dim txtFromDate As TextBox = grdDates.Rows(i).FindControl("txtfromdate")
                Dim txtToDate As TextBox = grdDates.Rows(i).FindControl("txttodate")
                If mCount = 0 Then
                    dpTxtFromDate.Text = txtFromDate.Text
                    dptxtTodate.Text = txtToDate.Text
                Else
                    If txtFromDate.Text <> "" Then
                        If CDate(txtFromDate.Text) < CDate(dpTxtFromDate.Text) Then
                            dpTxtFromDate.Text = txtFromDate.Text
                        End If
                    End If
                    If txtToDate.Text <> "" Then
                        If CDate(txtToDate.Text) > CDate(dptxtTodate.Text) Then
                            dptxtTodate.Text = txtToDate.Text
                        End If
                    End If
                End If
                mCount = 1
            Next

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("MaxAccomodation.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#End Region
#Region "Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs)"
    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click

        Dim txtadult As TextBox
        Dim txtchild As TextBox
        Dim txtinfant As TextBox
        Dim txtPriceOccupancy As TextBox
        Dim txtMaxEB As TextBox
        Dim txtMaxoccupancy As TextBox
        Dim txtrankorder As TextBox
        Dim chkunit As CheckBox
        Dim lblunit As Label
        Dim chkactive As CheckBox
        Dim txtactive As TextBox
        Dim txtExsuppunit As TextBox
        Dim txtrmtypename As TextBox
        Dim lblstart0based As Label
        Dim chkstart0based As CheckBox
        Dim txtpricepax As TextBox
        Dim txtrmtype As TextBox
        Dim txtMaxocccombination As TextBox
        Dim txtrmclasscode As TextBox
        Dim txtrmclasscodenew As TextBox
        Dim maxocccombination As String
        Dim hdncombination As HiddenField
        Dim sRoomImages As ImageButton ''*** Danny 8/3/18
        Dim sptypecode As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select sptypecode from partymast where partycode='" & hdnpartycode.Value & "'")
        Dim strMsg As String = ""


        Try
            If Page.IsValid = True Then

                If ViewState("State") = "New" Or ViewState("State") = "Edit" Or ViewState("State") = "Copy" Then
                    'If checkForDuplicate() = False Then
                    '    Exit Sub
                    'End If
                    SetDate()
                    If ValidateSave() = False Then
                        Exit Sub
                    End If

                    ''' commented 07/11/2016  becoz Country groups and agents are optional
                    'If wucCountrygroup.checkcountrylist.ToString = "" Then
                    '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Select the Country');", True)
                    '    Exit Sub
                    'End If
                    'If wucCountrygroup.checkagentlist.ToString = "" Then
                    '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Select the Agent');", True)
                    '    Exit Sub
                    'End If

                    '''''''''''''''''
                    If checkforexisting() = False Then
                        Exit Sub
                    End If

                    If chkctrygrp.Checked = True And Session("CountryList") = Nothing And Session("AgentList") = Nothing Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Country and Agent Should not be Empty Please select .');", True)
                        Exit Sub
                    End If

                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start
                    If Session("Calledfrom") = "Offers" Then
                        '''' Insert Main tables entry to Edit Table
                        'mySqlCmd = New SqlCommand("sp_insertrecords_edit_offers", mySqlConn, sqlTrans)
                        'mySqlCmd.CommandType = CommandType.StoredProcedure


                        'mySqlCmd.Parameters.Add(New SqlParameter("@promotionid", SqlDbType.VarChar, 20)).Value = CType(hdnpromotionid.Value, String)

                        'mySqlCmd.ExecuteNonQuery()
                        'mySqlCmd.Dispose()
                        '''''''''''''''''''''''

                    End If



                 

                    If ViewState("State") = "New" Or ViewState("State") = "Copy" Then

                        Dim optionval As String
                        optionval = objUtils.GetAutoDocNo("MAXOCC", mySqlConn, sqlTrans)
                        txtMaxid.Text = optionval.Trim

                        '------------------------for add partymaxacc_header
                        mySqlCmd = New SqlCommand("sp_add_edit_partymaxacc_header", mySqlConn, sqlTrans)
                        mySqlCmd.CommandType = CommandType.StoredProcedure

                        mySqlCmd.Parameters.Add(New SqlParameter("@sptypecode", SqlDbType.VarChar, 20)).Value = sptypecode
                        mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(hdnpartycode.Value.Trim, String)
                        If dpTxtFromDate.Text = "" Then
                            mySqlCmd.Parameters.Add(New SqlParameter("@frmdate", SqlDbType.DateTime)).Value = DBNull.Value
                        Else
                            mySqlCmd.Parameters.Add(New SqlParameter("@frmdate", SqlDbType.DateTime)).Value = ObjDate.ConvertDateromTextBoxToDatabase(dpTxtFromDate.Text)
                        End If
                        If dptxtTodate.Text = "" Then
                            mySqlCmd.Parameters.Add(New SqlParameter("@todate", SqlDbType.DateTime)).Value = DBNull.Value
                        Else
                            mySqlCmd.Parameters.Add(New SqlParameter("@todate", SqlDbType.DateTime)).Value = ObjDate.ConvertDateromTextBoxToDatabase(dptxtTodate.Text)
                        End If

                        mySqlCmd.Parameters.Add(New SqlParameter("@remarks", SqlDbType.Text)).Value = ""
                        mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                        mySqlCmd.Parameters.Add(New SqlParameter("@tranid", SqlDbType.VarChar, 20)).Value = CType(txtMaxid.Text.Trim, String)
                        mySqlCmd.Parameters.Add(New SqlParameter("@countrygroupyn", SqlDbType.Int)).Value = IIf(chkctrygrp.Checked = True, 1, 0)
                        mySqlCmd.Parameters.Add(New SqlParameter("@countrygroups", SqlDbType.VarChar, 5000)).Value = ""

                        If Session("Calledfrom") = "Offers" Then
                            mySqlCmd.Parameters.Add(New SqlParameter("@promotionid", SqlDbType.VarChar, 20)).Value = CType(hdnpromotionid.Value, String)

                        Else
                            mySqlCmd.Parameters.Add(New SqlParameter("@promotionid", SqlDbType.VarChar, 20)).Value = DBNull.Value
                        End If
                        mySqlCmd.Parameters.Add(New SqlParameter("@approved", SqlDbType.Int)).Value = 0


                        mySqlCmd.ExecuteNonQuery()
                        mySqlCmd.Dispose()                  'command disposed
                    ElseIf ViewState("State") = "Edit" Then
                        mySqlCmd = New SqlCommand("sp_mod_edit_partymaxacc_header", mySqlConn, sqlTrans)
                        mySqlCmd.CommandType = CommandType.StoredProcedure

                        mySqlCmd.Parameters.Add(New SqlParameter("@sptypecode", SqlDbType.VarChar, 20)).Value = sptypecode
                        mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(hdnpartycode.Value.Trim, String)
                        If dpTxtFromDate.Text = "" Then
                            mySqlCmd.Parameters.Add(New SqlParameter("@frmdate", SqlDbType.DateTime)).Value = DBNull.Value
                        Else
                            mySqlCmd.Parameters.Add(New SqlParameter("@frmdate", SqlDbType.DateTime)).Value = ObjDate.ConvertDateromTextBoxToDatabase(dpTxtFromDate.Text)
                        End If
                        If dptxtTodate.Text = "" Then
                            mySqlCmd.Parameters.Add(New SqlParameter("@todate", SqlDbType.DateTime)).Value = DBNull.Value
                        Else
                            mySqlCmd.Parameters.Add(New SqlParameter("@todate", SqlDbType.DateTime)).Value = ObjDate.ConvertDateromTextBoxToDatabase(dptxtTodate.Text)
                        End If

                        mySqlCmd.Parameters.Add(New SqlParameter("@remarks", SqlDbType.Text)).Value = ""
                        mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                        mySqlCmd.Parameters.Add(New SqlParameter("@tranid", SqlDbType.VarChar, 20)).Value = CType(txtMaxid.Text.Trim, String)
                        mySqlCmd.Parameters.Add(New SqlParameter("@countrygroupyn", SqlDbType.Int, 9)).Value = IIf(chkctrygrp.Checked = True, 1, 0)
                        mySqlCmd.Parameters.Add(New SqlParameter("@countrygroups", SqlDbType.VarChar, 5000)).Value = ""
                        'mySqlCmd.Parameters.Add(New SqlParameter("@promotionid", SqlDbType.VarChar, 20)).Value = CType(IIf(txtpromotionid.Text = "", DBNull.Value, txtpromotionid.Text), String)

                        If Session("Calledfrom") = "Offers" Then
                            mySqlCmd.Parameters.Add(New SqlParameter("@promotionid", SqlDbType.VarChar, 20)).Value = CType(hdnpromotionid.Value, String)

                        Else
                            mySqlCmd.Parameters.Add(New SqlParameter("@promotionid", SqlDbType.VarChar, 20)).Value = DBNull.Value
                        End If

                        mySqlCmd.Parameters.Add(New SqlParameter("@approved", SqlDbType.Int)).Value = 0

                        mySqlCmd.ExecuteNonQuery()
                        mySqlCmd.Dispose()


                        ' '' Remove from maxaccom_details
                        'mySqlCmd = New SqlCommand("sp_del_maxaccom_detailsnew", mySqlConn, sqlTrans)
                        'mySqlCmd.CommandType = CommandType.StoredProcedure
                        'mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(hdnpartycode.Value, String)
                        'mySqlCmd.Parameters.Add(New SqlParameter("@tranid", SqlDbType.VarChar, 20)).Value = CType(txtMaxid.Text.Trim, String)

                        'mySqlCmd.ExecuteNonQuery()
                        'mySqlCmd.Dispose()


                    End If

                    ' '''''' INSERT ROOM TYPES
                    'For Each GVRow In gv_FillData.Rows
                    '    txtrmtypename = GVRow.FindControl("txtrmtypename")
                    '    txtrmtype = GVRow.FindControl("txtrmtype")
                    '    txtrmclasscode = GVRow.Findcontrol("txtclasscode")

                    '    mySqlCmd = New SqlCommand("sp_add_newpartyrmtyp", mySqlConn, sqlTrans)
                    '    mySqlCmd.CommandType = CommandType.StoredProcedure

                    '    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(hdnpartycode.Value, String)
                    '    mySqlCmd.Parameters.Add(New SqlParameter("@roomtypname", SqlDbType.VarChar, 200)).Value = CType(txtrmtypename.Text.Trim, String)
                    '    mySqlCmd.Parameters.Add(New SqlParameter("@roomclasscode", SqlDbType.VarChar, 20)).Value = CType(txtrmclasscode.Text.Trim, String)

                    '    mySqlCmd.ExecuteNonQuery()
                    '    mySqlCmd.Dispose() 'command disposed
                    'Next

                    ' '''''''


                    mySqlCmd = New SqlCommand("DELETE FROM New_edit_RoomOccupancy Where Occupany_ID='" & CType(txtMaxid.Text.Trim, String) & "'", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.Text
                    mySqlCmd.ExecuteNonQuery()


                    Dim sqlstr As String = ""
                    '------------------------for add partymaxaccomodation
                    For Each GVRow In gv_FillData.Rows

                        txtadult = GVRow.FindControl("txtadult")
                        txtchild = GVRow.FindControl("txtchild")
                        txtinfant = GVRow.FindControl("txtinfant")
                        txtPriceOccupancy = GVRow.FindControl("txtpricepax")
                        txtMaxEB = GVRow.FindControl("txtMaxEB")
                        txtMaxoccupancy = GVRow.FindControl("txtMaxocctotal")


                        txtExsuppunit = GVRow.FindControl("txtExsuppunit")

                        txtrmtypename = GVRow.FindControl("txtrmtypename")
                        lblstart0based = GVRow.FindControl("lblstart0based")
                        chkstart0based = GVRow.FindControl("chkstart0based")
                        txtpricepax = GVRow.FindControl("txtpricepax")
                        txtrmtype = GVRow.FindControl("txtrmtype")
                        txtrmclasscode = GVRow.Findcontrol("txtrmclasscode")

                        txtMaxocccombination = GVRow.FindControl("txtMaxocccombination")
                        txtrmclasscodenew = GVRow.Findcontrol("txtclasscode")
                        hdncombination = GVRow.Findcontrol("hdncombination")
                        txtrankorder = GVRow.findcontrol("txtrankorder")

                        chkactive = GVRow.findcontrol("chkactive")
                        chkunit = GVRow.Findcontrol("chkunit")
                        Dim txtage1 As TextBox = GVRow.Findcontrol("txtage1")
                        Dim txtage2 As TextBox = GVRow.Findcontrol("txtage2")
                        Dim txtage3 As TextBox = GVRow.Findcontrol("txtage3")
                        Dim txtage4 As TextBox = GVRow.Findcontrol("txtage4")
                        Dim txtage5 As TextBox = GVRow.Findcontrol("txtage5")
                        Dim txtage6 As TextBox = GVRow.Findcontrol("txtage6")
                        Dim txtage7 As TextBox = GVRow.Findcontrol("txtage7")
                        Dim txtage8 As TextBox = GVRow.Findcontrol("txtage8")
                        Dim txtage9 As TextBox = GVRow.Findcontrol("txtage9")

                        Dim txtcombinationcat As TextBox = GVRow.FindControl("txtcombinationcat")




                        sqlstr = "select dbo.fn_max_occupancylist (" & Val(txtadult.Text) & "," & Val(txtchild.Text) & "," & Val(txtMaxoccupancy.Text) & "," & IIf(chkstart0based.Checked = True, 1, 0) & ",'" & CType(hdnpartycode.Value, String) & "','" & CType(txtrmtype.Text.Trim, String) & "')"
                        If txtMaxocccombination.Text = "" And Val(txtadult.Text) > 0 And Val(txtMaxoccupancy.Text) <> 0 Then

                            maxocccombination = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), sqlstr)



                        Else
                            'maxocccombination = hdncombination.Value
                            maxocccombination = txtcombinationcat.Text ' txtMaxocccombination.Text
                        End If

                        If maxocccombination.ToString.Trim <> "" Then

                            ''Value in hdn variable , so splting to get string correctly
                            Dim arrOcpncy As String() = maxocccombination.ToString.Trim.Split(",")
                            For i = 0 To arrOcpncy.Length - 1

                                If arrOcpncy(i) <> "" Then


                                    Dim arrAdultChild As String() = arrOcpncy(i).Split("/")

                                    If CType(txtrmtype.Text.Trim, String) = "" Then
                                        txtrmtype.Text = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select  rmtypecode from New_edit_RoomOccupancy(nolock) where Occupany_ID='" & txtMaxid.Text & "' and hotel_id='" & hdnpartycode.Value & "' and room_name='" & CType(txtrmtypename.Text.Trim.ToUpper, String) & "'")
                                    End If

                                    '    mySqlCmd = New SqlCommand("sp_mod_partymaxaccomodation", mySqlConn, sqlTrans)
                                    mySqlCmd = New SqlCommand("sp_add_New_edit_RoomOccupancy", mySqlConn, sqlTrans)
                                    mySqlCmd.CommandType = CommandType.StoredProcedure

                                    mySqlCmd.Parameters.Add(New SqlParameter("@sptypecode", SqlDbType.VarChar, 20)).Value = sptypecode
                                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(hdnpartycode.Value, String)
                                    mySqlCmd.Parameters.Add(New SqlParameter("@rmtypcode", SqlDbType.VarChar, 20)).Value = CType(txtrmtype.Text.Trim, String)
                                    mySqlCmd.Parameters.Add(New SqlParameter("@rmcatcode", SqlDbType.VarChar, 20)).Value = IIf(CType(arrAdultChild(2), String) = "", DBNull.Value, CType(arrAdultChild(2), String)) ' DBNull.Value

                                    If txtadult.Text <> "" Then
                                        mySqlCmd.Parameters.Add(New SqlParameter("@maxadults", SqlDbType.Int, 9)).Value = CType(txtadult.Text.Trim, Integer)
                                    Else
                                        mySqlCmd.Parameters.Add(New SqlParameter("@maxadults", SqlDbType.Int, 9)).Value = 0
                                    End If

                                    If txtchild.Text <> "" Then
                                        mySqlCmd.Parameters.Add(New SqlParameter("@maxchilds", SqlDbType.Int, 9)).Value = CType(txtchild.Text.Trim, Integer)
                                    Else
                                        mySqlCmd.Parameters.Add(New SqlParameter("@maxchilds", SqlDbType.Int, 9)).Value = 0
                                    End If

                                    If txtinfant.Text <> "" Then
                                        mySqlCmd.Parameters.Add(New SqlParameter("@maxinfant", SqlDbType.Int, 9)).Value = CType(txtinfant.Text.Trim, Integer)
                                    Else
                                        mySqlCmd.Parameters.Add(New SqlParameter("@maxinfant", SqlDbType.Int, 9)).Value = 0
                                    End If

                                    If txtPriceOccupancy.Text <> "" Then
                                        mySqlCmd.Parameters.Add(New SqlParameter("@pricepax", SqlDbType.Int, 9)).Value = CType(txtPriceOccupancy.Text.Trim, Integer)
                                    Else
                                        mySqlCmd.Parameters.Add(New SqlParameter("@pricepax", SqlDbType.Int, 9)).Value = 0
                                    End If

                                    If txtMaxEB.Text <> "" Then
                                        mySqlCmd.Parameters.Add(New SqlParameter("@maxeb", SqlDbType.Int, 9)).Value = CType(txtMaxEB.Text.Trim, Integer)
                                    Else
                                        mySqlCmd.Parameters.Add(New SqlParameter("@maxeb", SqlDbType.Int, 9)).Value = 0
                                    End If

                                    If txtMaxoccupancy.Text <> "" Then
                                        mySqlCmd.Parameters.Add(New SqlParameter("@maxoccpancy", SqlDbType.Int, 9)).Value = CType(txtMaxoccupancy.Text.Trim, Integer)
                                    Else
                                        mySqlCmd.Parameters.Add(New SqlParameter("@maxoccpancy", SqlDbType.Int, 9)).Value = 0
                                    End If
                                    mySqlCmd.Parameters.Add(New SqlParameter("@tranid", SqlDbType.VarChar, 20)).Value = CType(txtMaxid.Text.Trim, String)
                                    mySqlCmd.Parameters.Add(New SqlParameter("@noofextraperson", SqlDbType.Int, 9)).Value = CType(Val(txtExsuppunit.Text.Trim), Integer)
                                    mySqlCmd.Parameters.Add(New SqlParameter("@start0based", SqlDbType.Int, 9)).Value = IIf(chkstart0based.Checked = True, 1, 0)
                                    mySqlCmd.Parameters.Add(New SqlParameter("@roomtypname", SqlDbType.VarChar, 200)).Value = CType(txtrmtypename.Text.Trim.ToUpper, String)

                                    If (CType(txtrmclasscode.Text.Trim, String) <> CType(txtrmclasscodenew.Text.Trim, String) And CType(txtrmclasscodenew.Text.Trim, String) <> "") Then
                                        mySqlCmd.Parameters.Add(New SqlParameter("@roomclasscode", SqlDbType.VarChar, 20)).Value = CType(txtrmclasscodenew.Text.Trim, String)
                                    Else
                                        mySqlCmd.Parameters.Add(New SqlParameter("@roomclasscode", SqlDbType.VarChar, 20)).Value = CType(txtrmclasscode.Text.Trim, String)
                                    End If
                                    mySqlCmd.Parameters.Add(New SqlParameter("@rankorder", SqlDbType.Int, 9)).Value = CType(Val(txtrankorder.Text.Trim), Integer)
                                    mySqlCmd.Parameters.Add(New SqlParameter("@inactive", SqlDbType.Int, 9)).Value = CType(IIf(chkactive.Checked = True, 1, 0), Integer)
                                    mySqlCmd.Parameters.Add(New SqlParameter("@unityesno", SqlDbType.Int, 9)).Value = CType(IIf(chkunit.Checked = True, 1, 0), Integer)

                                    mySqlCmd.Parameters.Add(New SqlParameter("@noofadults", SqlDbType.Int, 9)).Value = CType(arrAdultChild(0), Integer)
                                    mySqlCmd.Parameters.Add(New SqlParameter("@noofchild", SqlDbType.Int, 9)).Value = CType(arrAdultChild(1), Integer)
                                    mySqlCmd.Parameters.Add(New SqlParameter("@contractid", SqlDbType.VarChar, 200)).Value = CType(hdncontractid.Value, String)
                                    mySqlCmd.Parameters.Add(New SqlParameter("@promotionid", SqlDbType.VarChar, 200)).Value = CType(hdnpromotionid.Value, String)

                               

                                    mySqlCmd.Parameters.Add(New SqlParameter("@age1", SqlDbType.Decimal, 9)).Value = CType(arrAdultChild(3), Decimal)
                                    mySqlCmd.Parameters.Add(New SqlParameter("@age2", SqlDbType.Decimal, 9)).Value = CType(arrAdultChild(4), Decimal)
                                    mySqlCmd.Parameters.Add(New SqlParameter("@age3", SqlDbType.Decimal, 9)).Value = CType(arrAdultChild(5), Decimal)
                                    mySqlCmd.Parameters.Add(New SqlParameter("@age4", SqlDbType.Decimal, 9)).Value = CType(arrAdultChild(6), Decimal)
                                    mySqlCmd.Parameters.Add(New SqlParameter("@age5", SqlDbType.Decimal, 9)).Value = CType(arrAdultChild(7), Decimal)
                                    mySqlCmd.Parameters.Add(New SqlParameter("@age6", SqlDbType.Decimal, 9)).Value = CType(arrAdultChild(8), Decimal)
                                    mySqlCmd.Parameters.Add(New SqlParameter("@age7", SqlDbType.Decimal, 9)).Value = CType(arrAdultChild(9), Decimal)
                                    mySqlCmd.Parameters.Add(New SqlParameter("@age8", SqlDbType.Decimal, 9)).Value = CType(arrAdultChild(10), Decimal)
                                    mySqlCmd.Parameters.Add(New SqlParameter("@age9", SqlDbType.Decimal, 9)).Value = CType(arrAdultChild(11), Decimal)


                                    '*** Danny 8/3/18 >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
                                    Dim dtResult As New DataTable
                                    Dim dtAdapter = New SqlDataAdapter
                                    dtAdapter.SelectCommand = mySqlCmd
                                    dtAdapter.Fill(dtResult)

                                    '  mySqlCmd.ExecuteNonQuery()
                                    '*** Danny 8/3/18 >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
                                    If Not dtResult Is Nothing Then
                                        If dtResult.Rows.Count > 0 Then
                                            sRoomImages = GVRow.FindControl("RoomImages") ''*** Danny 8/3/18
                                            Dim sImgPath = "RoomImages/Uploded/" + CType(dtResult.Rows(0).Item(0), String) + "_" + CType(dtResult.Rows(0).Item(1), String) + "_main.jpg"
                                            If Path.GetFileName(sRoomImages.ImageUrl.ToString) <> Path.GetFileName(sImgPath) Then
                                                Try
                                                    If Not Directory.Exists(Path.GetDirectoryName(Server.MapPath(sImgPath))) Then
                                                        Directory.CreateDirectory(Path.GetDirectoryName(Server.MapPath(sImgPath)))

                                                    End If
                                                    'Dim s As String = Server.MapPath(sRoomImages.ImageUrl)
                                                    If sImgPath.Contains("RoomImages/Uploded/") Then
                                                        File.Delete(Server.MapPath(sImgPath))
                                                    End If

                                                    If sRoomImages.ImageUrl.Contains("../images/ImageIco.png") Then
                                                        File.Copy(Server.MapPath(sRoomImages.ImageUrl), Server.MapPath(sImgPath))
                                                    Else
                                                        File.Move(Server.MapPath(sRoomImages.ImageUrl), Server.MapPath(sImgPath))
                                                    End If

                                                Catch ex As Exception
                                                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                                                    objUtils.WritErrorLog("ContractMaxocc.aspx=>btnSave_Click()", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
                                                End Try

                                            End If
                                        End If
                                    End If
                                    '*** Danny 8/3/18 <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<

                                    ' mySqlCmd.ExecuteNonQuery()
                                    'mySqlCmd.Dispose() 'command disposed


                                End If
                            Next

                        End If



                        'If maxocccombination.ToString.Trim <> "" Then

                        '    ''Value in hdn variable , so splting to get string correctly
                        '    Dim arrOcpncy As String() = maxocccombination.ToString.Trim.Split(",")
                        '    For i = 0 To arrOcpncy.Length - 1

                        '        If arrOcpncy(i) <> "" Then


                        '            Dim arrAdultChild As String() = arrOcpncy(i).Split("/")

                        '            mySqlCmd = New SqlCommand("sp_add_New_edit_RoomOccupancy", mySqlConn, sqlTrans)
                        '            mySqlCmd.CommandType = CommandType.StoredProcedure

                        '            mySqlCmd.Parameters.Add(New SqlParameter("@sptypecode", SqlDbType.VarChar, 20)).Value = sptypecode
                        '            mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(hdnpartycode.Value, String)
                        '            mySqlCmd.Parameters.Add(New SqlParameter("@rmtypcode", SqlDbType.VarChar, 20)).Value = CType(txtrmtype.Text.Trim, String)
                        '            mySqlCmd.Parameters.Add(New SqlParameter("@rmcatcode", SqlDbType.VarChar, 20)).Value = IIf(CType(arrAdultChild(2), String) = "", DBNull.Value, CType(arrAdultChild(2), String))


                        '            mySqlCmd.Parameters.Add(New SqlParameter("@maxadults", SqlDbType.Int, 9)).Value = CType(arrAdultChild(0), Integer)
                        '            mySqlCmd.Parameters.Add(New SqlParameter("@maxchilds", SqlDbType.Int, 9)).Value = CType(arrAdultChild(1), Integer)
                        '            mySqlCmd.Parameters.Add(New SqlParameter("@tranid", SqlDbType.VarChar, 20)).Value = CType(txtMaxid.Text.Trim, String)
                        '            mySqlCmd.Parameters.Add(New SqlParameter("@roomtypname", SqlDbType.VarChar, 200)).Value = CType(txtrmtypename.Text.Trim, String)

                        '            mySqlCmd.ExecuteNonQuery()
                        '            mySqlCmd.Dispose() 'command disposed
                        '        End If
                        '    Next

                        'End If


                        Next
                    'mySqlCmd = New SqlCommand("sp_Del_partymaxaccom", mySqlConn, sqlTrans)
                    'mySqlCmd.CommandType = CommandType.StoredProcedure
                    'mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(hdnpartycode.Value, String)
                    'mySqlCmd.Parameters.Add(New SqlParameter("@tranid", SqlDbType.VarChar, 20)).Value = CType(txtMaxid.Text.Trim, String)
                    'mySqlCmd.ExecuteNonQuery()

                            'For Each GVRow In grdDates.Rows
                            '    Dim txtfromdate As TextBox = GVRow.FindControl("txtfromdate")
                            '    Dim txttodate As TextBox = GVRow.FindControl("txttodate")
                            '    'dpFDate = GVRow.FindControl("FrmDate")
                            '    'dpTDate = GVRow.FindControl("ToDate")
                            '    If txtfromdate.Text <> "" And txttodate.Text <> "" Then
                            '        mySqlCmd = New SqlCommand("sp_add_partymaxaccom_dates", mySqlConn, sqlTrans)
                            '        mySqlCmd.CommandType = CommandType.StoredProcedure
                            '        mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(hdnpartycode.Value, String)
                            '        mySqlCmd.Parameters.Add(New SqlParameter("@frmdate", SqlDbType.DateTime)).Value = ObjDate.ConvertDateromTextBoxToDatabase(txtfromdate.Text)
                            '        mySqlCmd.Parameters.Add(New SqlParameter("@todate", SqlDbType.DateTime)).Value = ObjDate.ConvertDateromTextBoxToDatabase(txttodate.Text)
                            '        mySqlCmd.Parameters.Add(New SqlParameter("@lineno", SqlDbType.Int, 9)).Value = 0
                            '        mySqlCmd.Parameters.Add(New SqlParameter("@tranid", SqlDbType.VarChar, 20)).Value = CType(txtMaxid.Text.Trim, String)
                            '        mySqlCmd.ExecuteNonQuery()
                            '    End If
                            'Next

                            '''  User cotrol country saving
                            ''' 

                    mySqlCmd = New SqlCommand("DELETE FROM edit_partymaxacc_countries Where tranid='" & CType(txtMaxid.Text.Trim, String) & "'", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.Text
                    mySqlCmd.ExecuteNonQuery()

                    mySqlCmd = New SqlCommand("DELETE FROM edit_partymaxacc_agents Where tranid='" & CType(txtMaxid.Text.Trim, String) & "'", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.Text
                    mySqlCmd.ExecuteNonQuery()

                    If wucCountrygroup.checkcountrylist.ToString <> "" And chkctrygrp.Checked = True Then

                        ''Value in hdn variable , so splting to get string correctly
                        Dim arrcountry As String() = wucCountrygroup.checkcountrylist.ToString.Trim.Split(",")
                        For i = 0 To arrcountry.Length - 1

                            If arrcountry(i) <> "" Then




                                mySqlCmd = New SqlCommand("sp_add_partycountries", mySqlConn, sqlTrans)
                                mySqlCmd.CommandType = CommandType.StoredProcedure


                                mySqlCmd.Parameters.Add(New SqlParameter("@countrycode", SqlDbType.VarChar, 20)).Value = CType(arrcountry(i), String)
                                mySqlCmd.Parameters.Add(New SqlParameter("@tranid", SqlDbType.VarChar, 20)).Value = CType(txtMaxid.Text.Trim, String)

                                mySqlCmd.ExecuteNonQuery()
                                mySqlCmd.Dispose() 'command disposed
                            End If
                        Next

                    End If

                    If wucCountrygroup.checkagentlist.ToString <> "" And chkctrygrp.Checked = True Then

                        ''Value in hdn variable , so splting to get string correctly
                        Dim arragents As String() = wucCountrygroup.checkagentlist.ToString.Trim.Split(",")
                        For i = 0 To arragents.Length - 1

                            If arragents(i) <> "" Then




                                mySqlCmd = New SqlCommand("sp_add_partyagents", mySqlConn, sqlTrans)
                                mySqlCmd.CommandType = CommandType.StoredProcedure


                                mySqlCmd.Parameters.Add(New SqlParameter("@agentcode", SqlDbType.VarChar, 20)).Value = CType(arragents(i), String)
                                mySqlCmd.Parameters.Add(New SqlParameter("@tranid", SqlDbType.VarChar, 20)).Value = CType(txtMaxid.Text.Trim, String)

                                mySqlCmd.ExecuteNonQuery()
                                mySqlCmd.Dispose() 'command disposed
                            End If
                        Next

                    End If

                          

                            strMsg = "Saved Succesfully!!"

                        ElseIf ViewState("State") = "Delete" Then

                            '------------------------for delete partymaxacc_header,partymaxaccomodation
                            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                            sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start
                            'delete for row tables present in sp
                            mySqlCmd = New SqlCommand("sp_del_partymaxacc_header", mySqlConn, sqlTrans)
                            mySqlCmd.CommandType = CommandType.StoredProcedure
                            mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(hdnpartycode.Value, String)
                            mySqlCmd.Parameters.Add(New SqlParameter("@tranid", SqlDbType.VarChar, 20)).Value = CType(txtMaxid.Text.Trim, String)
                            mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                            mySqlCmd.ExecuteNonQuery()
                    strMsg = "Delete  Successfull!!"
                        End If



                        sqlTrans.Commit()    'SQl Tarn Commit
                        clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
                        clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed

                        clsDBConnect.dbConnectionClose(mySqlConn)           'connection close

                        ' Divmain.Style("display") = "none"

                        ViewState("State") = ""
                        '   wucCountrygroup.clearsessions()
                        btnreset1_Click(sender, e)

                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & strMsg & "' );", True)
                        'Dim strscript As String = ""
                        'strscript = "window.opener.__doPostBack('MaxaccWindowPostBack', '');window.opener.focus();window.close();"
                        'ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)
            End If


        Catch ex As Exception
            If mySqlConn.State = ConnectionState.Open Then
                sqlTrans.Rollback()
            End If
            objUtils.WritErrorLog("ContractMaxocc.aspx=>btnSave_Click()", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName")) '*** Danny 8/3/18
            'ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Error while saving. Please try again later.');", True)
        End Try

    End Sub

#End Region


#Region "Numbers"
    Public Sub Numbers(ByVal txtbox As HtmlInputText)
        txtbox.Attributes.Add("onkeypress", "return checkNumber(event)")
    End Sub
#End Region



    Protected Sub btnhelp_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        '   ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=SupMain','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub




    Protected Sub imgbEditnew_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        'Protected Sub imgbEditnew_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim clickedRow As GridViewRow = TryCast(DirectCast(sender, ImageButton).NamingContainer, GridViewRow)

        Dim txtrmtypename As TextBox = DirectCast(clickedRow.FindControl("txtrmtypename"), TextBox)

        txtrmtypename.Enabled = True



        'Dim objbtn As Button = CType(sender, Button)
        'Dim rowid As Integer = 0
        'Dim row As GridViewRow
        'row = CType(objbtn.NamingContainer, GridViewRow)
        'rowid = row.RowIndex
        'Dim txtrmtypename As String = CType(gv_FillData.Rows(rowid).FindControl("txtrmtypename"), TextBox).Text
        'txtrmtypename. = True




    End Sub


#Region "Numberssrvctrl"
    Private Sub Numberssrvctrl(ByVal txtbox As WebControls.TextBox)
        txtbox.Attributes.Add("onkeypress", "return  checkNumber(event)")
    End Sub
#End Region
    Protected Sub gv_FillData_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gv_FillData.RowDataBound

        Dim imgRoom As ImageButton '*** Danny 12/03/18
        Dim txtadult As TextBox
        Dim txtchild As TextBox
        Dim txtinfant As TextBox
        Dim txtPriceOccupancy As TextBox
        Dim txtMaxEB As TextBox
        Dim txtMaxoccupancy As TextBox
        Dim txtrankorder As TextBox
        Dim chkunit As CheckBox
        Dim lblunit As Label
        Dim chkactive As CheckBox
        Dim txtactive As TextBox
        Dim txtExsuppunit As TextBox
        Dim txtrmtypename As TextBox
        Dim lblstart0based As Label
        Dim chkstart0based As CheckBox
        Dim txtpricepax As TextBox
        Dim txtrmtype As TextBox
        Dim txtMaxocccombination As TextBox
        Dim hdncombination As HiddenField



        If (e.Row.RowType = DataControlRowType.DataRow) Then
            imgRoom = e.Row.FindControl("RoomImages") '*** Danny 12/03/18
            If Not File.Exists(Server.MapPath(imgRoom.ImageUrl)) Then
                imgRoom.ImageUrl = "../images/ImageIco.png"
            End If


            txtadult = e.Row.FindControl("txtadult")
            txtchild = e.Row.FindControl("txtchild")
            txtinfant = e.Row.FindControl("txtinfant")
            txtPriceOccupancy = e.Row.FindControl("txtpricepax")
            txtMaxEB = e.Row.FindControl("txtMaxEB")
            txtMaxoccupancy = e.Row.FindControl("txtMaxocctotal")
            txtrankorder = e.Row.FindControl("txtrankorder")
            lblunit = e.Row.FindControl("lblunit")
            txtExsuppunit = e.Row.FindControl("txtExsuppunit")
            chkactive = e.Row.FindControl("chkactive")
            txtactive = e.Row.FindControl("txtactive")
            txtrmtypename = e.Row.FindControl("txtrmtypename")
            lblstart0based = e.Row.FindControl("lblstart0based")
            chkstart0based = e.Row.FindControl("chkstart0based")
            txtpricepax = e.Row.FindControl("txtpricepax")
            txtrmtype = e.Row.FindControl("txtrmtype")
            txtrmtypename.Enabled = False
            chkunit = e.Row.FindControl("chkunit")
            txtMaxocccombination = e.Row.FindControl("txtMaxocccombination")
            hdncombination = e.Row.FindControl("hdncombination")


            If lblunit.Text = "0" Or lblunit.Text = "No" Or lblunit.Text = "" Then
                lblunit.Text = "No"
                txtExsuppunit.Enabled = False
                txtpricepax.Enabled = False
                chkunit.Checked = False
            Else
                lblunit.Text = "Yes"
                txtExsuppunit.Enabled = True
                txtpricepax.Enabled = True
                chkunit.Checked = True
            End If

            If txtactive.Text = "0" Or txtactive.Text = "" Then
                chkactive.Checked = False
            Else
                chkactive.Checked = True
                e.Row.BackColor = ColorTranslator.FromHtml("#F8CBAD")

            End If

            If lblstart0based.Text = "0" Or lblstart0based.Text = "" Then
                chkstart0based.Checked = False
            Else
                chkstart0based.Checked = True
            End If

            If txtrmtypename.Text = "" Then
                txtrmtypename.Enabled = True
            End If

            If txtExsuppunit.Text = "0" Then
                txtExsuppunit.Text = ""
            End If


            Numberssrvctrl(txtadult)
            Numberssrvctrl(txtchild)
            Numberssrvctrl(txtinfant)
            Numberssrvctrl(txtPriceOccupancy)
            Numberssrvctrl(txtMaxEB)
            Numberssrvctrl(txtMaxoccupancy)
            Numberssrvctrl(txtrankorder)


            '  If Fillcombination(

            'txtadult.Attributes.Add("onchange", "alert('TextBox1 onchange fired');")
            txtadult.Attributes.Add("onChange", "getmaxcombination('" & txtadult.ClientID & "','" & txtchild.ClientID & "','" & txtMaxoccupancy.ClientID & "','" & hdnpartycode.ClientID & "','" & txtrmtype.ClientID & "','" & hdncombination.ClientID & "','" & txtMaxocccombination.ClientID & "','" & lblstart0based.ClientID & "','" + CType(e.Row.RowIndex, String) + "')")
            txtchild.Attributes.Add("onChange", "getmaxcombination('" & txtadult.ClientID & "','" & txtchild.ClientID & "','" & txtMaxoccupancy.ClientID & "','" & hdnpartycode.ClientID & "','" & txtrmtype.ClientID & "','" & hdncombination.ClientID & "','" & txtMaxocccombination.ClientID & "','" & lblstart0based.ClientID & "','" + CType(e.Row.RowIndex, String) + "')")
            txtMaxoccupancy.Attributes.Add("onChange", "getmaxcombination('" & txtadult.ClientID & "','" & txtchild.ClientID & "','" & txtMaxoccupancy.ClientID & "','" & hdnpartycode.ClientID & "','" & txtrmtype.ClientID & "','" & hdncombination.ClientID & "','" & txtMaxocccombination.ClientID & "','" & lblstart0based.ClientID & "','" + CType(e.Row.RowIndex, String) + "')")


            chkunit.Attributes.Add("onChange", "Enablepricepax('" & chkunit.ClientID & "','" + txtpricepax.ClientID + "','" + txtExsuppunit.ClientID + "','" + CType(e.Row.RowIndex, String) + "')")

            ' chkactive.Attributes.Add("onChange", "Showcolor('" & chkactive.ClientID & "','" + CType(e.Row.RowIndex, String) + "')")

            'chkactive.Attributes.Add("onClick", "ChangeColor('" + "gv_FillData','" & chkactive.ClientID & "','" + (e.Row.RowIndex + 1).ToString() + "')")

        End If



        If e.Row.RowType = DataControlRowType.DataRow AndAlso (e.Row.RowState = DataControlRowState.Normal OrElse e.Row.RowState = DataControlRowState.Alternate) Then
            Dim strGridName As String = gv_FillData.ClientID
            Dim strRowId As String = e.Row.RowIndex
            Dim strFoucsColumnIndex = "5"
            e.Row.Attributes("onclick") = "javascript:SelectRow(this,'" + strRowId + "','" + strGridName + "','" + strFoucsColumnIndex + "');"
            e.Row.Attributes("onkeydown") = "javascript:return SelectSibling(event,'" + strGridName + "','" + strFoucsColumnIndex + "',this);"
        End If

    End Sub


    Protected Sub btncombination_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim ds As DataSet
        Dim objbtn As Button = CType(sender, Button)
        Dim rowid As Integer = 0
        Dim row As GridViewRow
        row = CType(objbtn.NamingContainer, GridViewRow)
        rowid = row.RowIndex

        Dim strMaxoccupancy As String = CType(gv_FillData.Rows(rowid).FindControl("txtMaxocctotal"), TextBox).Text
        Dim strChkdVal As String = CType(gv_FillData.Rows(rowid).FindControl("txtMaxocccombination"), TextBox).Text
        Dim stradult As String = CType(gv_FillData.Rows(rowid).FindControl("txtadult"), TextBox).Text
        Dim strchild As String = CType(gv_FillData.Rows(rowid).FindControl("txtchild"), TextBox).Text
        Dim strrmtypename As String = CType(gv_FillData.Rows(rowid).FindControl("txtrmtypename"), TextBox).Text
        Dim strrmclassname As String = CType(gv_FillData.Rows(rowid).FindControl("txtrmclassname"), TextBox).Text
        Dim strstartwith0based As String = IIf(CType(gv_FillData.Rows(rowid).FindControl("chkstart0based"), CheckBox).Checked = True, 1, 0)

        Dim chkunit As String = IIf(CType(gv_FillData.Rows(rowid).FindControl("chkunit"), CheckBox).Checked = True, 1, 0)

        Dim rmtypcode As String = CType(gv_FillData.Rows(rowid).FindControl("txtrmtype"), TextBox).Text

        Dim strChkdValnew As String = CType(gv_FillData.Rows(rowid).FindControl("txtcombinationcat"), TextBox).Text

        'Dim strage1 As String = CType(gv_FillData.Rows(rowid).FindControl("txtage1"), TextBox).Text
        'Dim strage2 As String = CType(gv_FillData.Rows(rowid).FindControl("txtage2"), TextBox).Text
        'Dim strage3 As String = CType(gv_FillData.Rows(rowid).FindControl("txtage3"), TextBox).Text
        'Dim strage4 As String = CType(gv_FillData.Rows(rowid).FindControl("txtage4"), TextBox).Text
        'Dim strage5 As String = CType(gv_FillData.Rows(rowid).FindControl("txtage5"), TextBox).Text
        'Dim strage6 As String = CType(gv_FillData.Rows(rowid).FindControl("txtage6"), TextBox).Text
        'Dim strage7 As String = CType(gv_FillData.Rows(rowid).FindControl("txtage7"), TextBox).Text
        'Dim strage8 As String = CType(gv_FillData.Rows(rowid).FindControl("txtage8"), TextBox).Text
        'Dim strage9 As String = CType(gv_FillData.Rows(rowid).FindControl("txtage9"), TextBox).Text


        Dim unityesno As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id=1141 ")

        Dim strRoomType As String = CType(strrmtypename, String)
        Dim strRoomCategory As String = CType(strrmclassname, String)

        If strMaxoccupancy <> "" And stradult <> "" Then
            If Val(strMaxoccupancy) <> 0 And Val(stradult) <> 0 Then


                Dim parms As New List(Of SqlParameter)
                Dim parm(7) As SqlParameter
                parm(0) = New SqlParameter("@adult", CType(Val(stradult), Integer))
                parm(1) = New SqlParameter("@child", CType(Val(strchild), Integer))
                parm(2) = New SqlParameter("@maxoccupancy", CType(Val(strMaxoccupancy), Integer))
                parm(3) = New SqlParameter("@startwith0based", CType(Val(strstartwith0based), Integer))
                parm(4) = New SqlParameter("@partycode", CType(hdnpartycode.Value, String))
                parm(5) = New SqlParameter("@rmtypcode", CType(rmtypcode, String))
                parm(6) = New SqlParameter("@tranid", CType(txtMaxid.Text, String))

                'parm(6) = New SqlParameter("@age1", CType(Val(strage1), Decimal))
                'parm(7) = New SqlParameter("@age2", CType(Val(strage2), Decimal))
                'parm(8) = New SqlParameter("@age3", CType(Val(strage3), Decimal))
                'parm(9) = New SqlParameter("@age4", CType(Val(strage4), Decimal))
                'parm(10) = New SqlParameter("@age5", CType(Val(strage5), Decimal))
                'parm(11) = New SqlParameter("@age6", CType(Val(strage6), Decimal))
                'parm(12) = New SqlParameter("@age7", CType(Val(strage7), Decimal))
                'parm(13) = New SqlParameter("@age8", CType(Val(strage8), Decimal))
                'parm(14) = New SqlParameter("@age9", CType(Val(strage9), Decimal))

                For i = 0 To 6
                    parms.Add(parm(i))
                Next
                ds = objUtils.ExecuteQuerynew(Session("dbconnectionName"), "sp_max_occupancy", parms)
                If Not ds Is Nothing Then
                    If ds.Tables.Count > 0 Then
                        Session("AdultChildCombntn") = ds.Tables(0)
                        gvOccupancy.DataSource = ds.Tables(0)
                        gvOccupancy.DataBind()
                        hdnMainGridRowid.Value = rowid
                    Else
                        Session("AdultChildCombntn") = ""
                    End If
                End If

                Select Case strchild
                    Case 1
                        gvOccupancy.Columns(4).Visible = True


                        gvOccupancy.Columns(5).Visible = False
                        gvOccupancy.Columns(6).Visible = False
                        gvOccupancy.Columns(7).Visible = False
                        gvOccupancy.Columns(8).Visible = False
                        gvOccupancy.Columns(9).Visible = False
                        gvOccupancy.Columns(10).Visible = False
                        gvOccupancy.Columns(11).Visible = False
                        gvOccupancy.Columns(12).Visible = False
                    Case 2
                        gvOccupancy.Columns(4).Visible = True
                        gvOccupancy.Columns(5).Visible = True


                        gvOccupancy.Columns(6).Visible = False
                        gvOccupancy.Columns(7).Visible = False
                        gvOccupancy.Columns(8).Visible = False
                        gvOccupancy.Columns(9).Visible = False
                        gvOccupancy.Columns(10).Visible = False
                        gvOccupancy.Columns(11).Visible = False
                        gvOccupancy.Columns(12).Visible = False
                    Case 3
                        gvOccupancy.Columns(4).Visible = True
                        gvOccupancy.Columns(5).Visible = True
                        gvOccupancy.Columns(6).Visible = True

                       
                        gvOccupancy.Columns(7).Visible = False
                        gvOccupancy.Columns(8).Visible = False
                        gvOccupancy.Columns(9).Visible = False
                        gvOccupancy.Columns(10).Visible = False
                        gvOccupancy.Columns(11).Visible = False
                        gvOccupancy.Columns(12).Visible = False
                    Case 4
                        gvOccupancy.Columns(4).Visible = True
                        gvOccupancy.Columns(5).Visible = True
                        gvOccupancy.Columns(6).Visible = True
                        gvOccupancy.Columns(7).Visible = True

                     
                        gvOccupancy.Columns(8).Visible = False
                        gvOccupancy.Columns(9).Visible = False
                        gvOccupancy.Columns(10).Visible = False
                        gvOccupancy.Columns(11).Visible = False
                        gvOccupancy.Columns(12).Visible = False
                    Case 5
                        gvOccupancy.Columns(4).Visible = True
                        gvOccupancy.Columns(5).Visible = True
                        gvOccupancy.Columns(6).Visible = True
                        gvOccupancy.Columns(7).Visible = True
                        gvOccupancy.Columns(8).Visible = True

                     
                        gvOccupancy.Columns(9).Visible = False
                        gvOccupancy.Columns(10).Visible = False
                        gvOccupancy.Columns(11).Visible = False
                        gvOccupancy.Columns(12).Visible = False
                    Case 6
                        gvOccupancy.Columns(4).Visible = True
                        gvOccupancy.Columns(5).Visible = True
                        gvOccupancy.Columns(6).Visible = True
                        gvOccupancy.Columns(7).Visible = True
                        gvOccupancy.Columns(8).Visible = True
                        gvOccupancy.Columns(9).Visible = True

                       
                        gvOccupancy.Columns(10).Visible = False
                        gvOccupancy.Columns(11).Visible = False
                        gvOccupancy.Columns(12).Visible = False

                    Case 7
                        gvOccupancy.Columns(4).Visible = True
                        gvOccupancy.Columns(5).Visible = True
                        gvOccupancy.Columns(6).Visible = True
                        gvOccupancy.Columns(7).Visible = True
                        gvOccupancy.Columns(8).Visible = True
                        gvOccupancy.Columns(9).Visible = True
                        gvOccupancy.Columns(10).Visible = True

                       
                        gvOccupancy.Columns(11).Visible = False
                        gvOccupancy.Columns(12).Visible = False

                    Case 8
                        gvOccupancy.Columns(4).Visible = True
                        gvOccupancy.Columns(5).Visible = True
                        gvOccupancy.Columns(6).Visible = True
                        gvOccupancy.Columns(7).Visible = True
                        gvOccupancy.Columns(8).Visible = True
                        gvOccupancy.Columns(9).Visible = True
                        gvOccupancy.Columns(10).Visible = True
                        gvOccupancy.Columns(11).Visible = True
                        gvOccupancy.Columns(12).Visible = False

                    Case 9
                        gvOccupancy.Columns(4).Visible = True
                        gvOccupancy.Columns(5).Visible = True
                        gvOccupancy.Columns(6).Visible = True
                        gvOccupancy.Columns(7).Visible = True
                        gvOccupancy.Columns(8).Visible = True
                        gvOccupancy.Columns(9).Visible = True
                        gvOccupancy.Columns(10).Visible = True
                        gvOccupancy.Columns(11).Visible = True
                        gvOccupancy.Columns(12).Visible = True


                End Select
                    lblRoomCategoryText.Text = strRoomCategory
                    lblRoomTypeText.Text = strRoomType
                    'strChkdVal = "1/0,1/1,2/1"

                    For Each gvRow In gvOccupancy.Rows

                        Dim ddlRmcat As HtmlSelect = gvRow.FindControl("ddlRmcat")
                        objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlRmcat, "rmcatcode", "rmcatcode", "select pr.rmcatcode,pr.rmcatcode from  rmcatmast  r,partyrmcat pr where r.rmcatcode=pr.rmcatcode and " _
                                                         & " pr.partycode='" & hdnpartycode.Value & "' and   isnull(allotreqd,'')='Yes' and r.active=1  order by isnull(r.rankorder,99)", True)

                        Dim typ As Type
                        typ = GetType(HtmlSelect)
                        If (Page.ClientScript.IsClientScriptIncludeRegistered(typ, "TADDScript.js")) = False Then
                            Page.ClientScript.RegisterClientScriptInclude(typ, "TADDScript.js", "TADDScript.js")

                            ddlRmcat.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")


                        End If


                    Next

                    ' ChkExistingVal(strChkdVal)
                    ChkExistingVal(strChkdValnew)

                    For Each grdRow In gvOccupancy.Rows
                        Dim ddlRmcat As HtmlSelect = grdRow.FindControl("ddlRmcat")
                    Dim chkselect As HtmlInputCheckBox = grdRow.FindControl("chk")
                    Dim txtchild1age As TextBox = grdRow.Findcontrol("txtchild1age")
                    Dim txtchild2age As TextBox = grdRow.Findcontrol("txtchild2age")
                    Dim txtchild3age As TextBox = grdRow.Findcontrol("txtchild3age")
                    Dim txtchild4age As TextBox = grdRow.Findcontrol("txtchild4age")
                    Dim txtchild5age As TextBox = grdRow.Findcontrol("txtchild5age")
                    Dim txtchild6age As TextBox = grdRow.Findcontrol("txtchild6age")
                    Dim txtchild7age As TextBox = grdRow.Findcontrol("txtchild7age")
                    Dim txtchild8age As TextBox = grdRow.Findcontrol("txtchild8age")
                    Dim txtchild9age As TextBox = grdRow.Findcontrol("txtchild9age")


                        If chkunit = "1" Then
                            ddlRmcat.Value = CType(unityesno, String)
                            ddlRmcat.Disabled = True
                    End If


                    Select Case (grdRow.cells(2).text)
                        Case "0"
                            txtchild1age.Enabled = False
                            txtchild2age.Enabled = False
                            txtchild3age.Enabled = False
                            txtchild4age.Enabled = False

                        Case "1"
                            txtchild1age.Enabled = True
                            txtchild2age.Enabled = False
                            txtchild3age.Enabled = False
                            txtchild4age.Enabled = False
                        Case "2"
                            txtchild1age.Enabled = True
                            txtchild2age.Enabled = True
                            txtchild3age.Enabled = False
                            txtchild4age.Enabled = False
                        Case "3"
                            txtchild1age.Enabled = True
                            txtchild2age.Enabled = True
                            txtchild3age.Enabled = True
                            txtchild4age.Enabled = False
                        Case "4"
                            txtchild1age.Enabled = True
                            txtchild2age.Enabled = True
                            txtchild3age.Enabled = True
                            txtchild4age.Enabled = True
                        Case "5"
                            txtchild1age.Enabled = True
                            txtchild2age.Enabled = True
                            txtchild3age.Enabled = True
                            txtchild4age.Enabled = True
                            txtchild5age.Enabled = True
                        Case "6"
                            txtchild1age.Enabled = True
                            txtchild2age.Enabled = True
                            txtchild3age.Enabled = True
                            txtchild4age.Enabled = True
                            txtchild5age.Enabled = True
                            txtchild6age.Enabled = True
                        Case "7"
                            txtchild1age.Enabled = True
                            txtchild2age.Enabled = True
                            txtchild3age.Enabled = True
                            txtchild4age.Enabled = True
                            txtchild5age.Enabled = True
                            txtchild6age.Enabled = True
                            txtchild7age.Enabled = True
                        Case "8"
                            txtchild1age.Enabled = True
                            txtchild2age.Enabled = True
                            txtchild3age.Enabled = True
                            txtchild4age.Enabled = True
                            txtchild5age.Enabled = True
                            txtchild6age.Enabled = True
                            txtchild7age.Enabled = True
                            txtchild8age.Enabled = True
                        Case "9"
                            txtchild1age.Enabled = True
                            txtchild2age.Enabled = True
                            txtchild3age.Enabled = True
                            txtchild4age.Enabled = True
                            txtchild5age.Enabled = True
                            txtchild6age.Enabled = True
                            txtchild7age.Enabled = True
                            txtchild8age.Enabled = True
                            txtchild9age.Enabled = True

                    End Select



                Next

                    ModalPopupOccupancy.Show()
                End If
            End If
            'Else
            If strMaxoccupancy = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Max Occupancy cannot be empty!');", True)
            ElseIf strMaxoccupancy = 0 Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Max Occupancy cannot be 0!');", True)

            ElseIf stradult = "" Or Val(stradult) = 0 Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Adult cannot be empty!');", True)
            ElseIf Val(stradult) = 0 Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Adult cannot be 0!');", True)

            End If
            'End If
    End Sub
    Private Sub ChkExistingVal(ByVal prm_strChkdVal As String)
        Dim chkSelect As HtmlInputCheckBox
        Dim intAdult, intChild As Integer

        Dim arrString As String()
        Dim arrlist As String()
        Dim j As Integer = 0 'string in the form "a,b;c,d;...."

        If prm_strChkdVal <> "" Then
            arrString = prm_strChkdVal.Split(",") 'spliting for ';' 1st



            'Dim i As Integer = 0
            'For Each grdRow In gvOccupancy.Rows
            '    Dim ddlRmcat As HtmlSelect = grdRow.FindControl("ddlRmcat")
            '    If arrString.Length = i Then
            '        Exit For
            '    Else
            '        If arrString(i) <> "" Then
            '            arrlist = arrString(i).Split("/")

            '            ddlRmcat.Value = CType(arrlist(2), String)

            '        End If
            '    End If

            '    i += 1
            'Next


            ''first case when not selected , chk all
            'For Each grdRow In gvOccupancy.Rows
            '    chkSelect = CType(grdRow.FindControl("chk"), HtmlInputCheckBox)
            '    chkSelect.Checked = True

            'Next

            For k = 0 To arrString.Length - 1
                If arrString(k) <> "" Then

                    Dim arrAdultChild As String() = arrString(k).Split("/") 'spliting for ',' 2nd

                    For Each grdRow In gvOccupancy.Rows
                        chkSelect = CType(grdRow.FindControl("chk"), HtmlInputCheckBox)
                        Dim ddlRmcat As HtmlSelect = grdRow.FindControl("ddlRmcat")
                        Dim txtchild1age As TextBox = grdRow.findcontrol("txtchild1age")
                        Dim txtchild2age As TextBox = grdRow.findcontrol("txtchild2age")
                        Dim txtchild3age As TextBox = grdRow.findcontrol("txtchild3age")
                        Dim txtchild4age As TextBox = grdRow.findcontrol("txtchild4age")
                        Dim txtchild5age As TextBox = grdRow.findcontrol("txtchild5age")
                        Dim txtchild6age As TextBox = grdRow.findcontrol("txtchild6age")
                        Dim txtchild7age As TextBox = grdRow.findcontrol("txtchild7age")
                        Dim txtchild8age As TextBox = grdRow.findcontrol("txtchild8age")
                        Dim txtchild9age As TextBox = grdRow.findcontrol("txtchild9age")
                        'Get  adult n child for comparing string
                        intAdult = grdRow.Cells(1).Text
                        intChild = grdRow.Cells(2).Text

                        'adult                              child
                        If arrAdultChild(0) = intAdult And arrAdultChild(1) = intChild Then 'And arrAdultChild(2) = ddlRmcat.Value Then
                            chkSelect.Checked = True
                            ddlRmcat.Value = arrAdultChild(2)
                            If arrAdultChild.Length > 3 Then
                                txtchild1age.Text = arrAdultChild(3)

                                txtchild2age.Text = arrAdultChild(4)
                                txtchild3age.Text = arrAdultChild(5)
                                txtchild4age.Text = arrAdultChild(6)
                                txtchild5age.Text = arrAdultChild(7)
                                txtchild6age.Text = arrAdultChild(8)
                                txtchild7age.Text = arrAdultChild(9)
                                txtchild8age.Text = arrAdultChild(10)
                                txtchild9age.Text = arrAdultChild(11)
                            End If

                        End If
                    Next
                End If
            Next


            'For Each gvRow In gvOccupancy.Rows
            '    Dim ddlRmcat As HtmlSelect = gvRow.FindControl("ddlRmcat")

            '    If ddlRmcat.Value = "[Select]" Then
            '        ddlRmcat.Value = CType(mySqlReader("rmcatcode"), String)
            '        Exit For
            '    End If
            'Next


        Else
            'first case when not selected , chk all
            For Each grdRow In gvOccupancy.Rows
                chkSelect = CType(grdRow.FindControl("chk"), HtmlInputCheckBox)
                chkSelect.Checked = True

            Next


        End If
    End Sub
    Protected Sub btnSelectAll_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSelectAll.Click
        Dim chksel As HtmlInputCheckBox
        For Each grdRow In gvOccupancy.Rows
            chksel = grdRow.FindControl("chk")
            chksel.Checked = True
        Next
        ModalPopupOccupancy.Show()
    End Sub

    Protected Sub btnDeleteRowcomb_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDeleteRowcomb.Click
        Createdatacolumnschild("delete")

        '  ModalPopupOccupancy.Show()




        'Dim dtGridVal As DataTable
        'Dim chkSelect As HtmlInputCheckBox

        'If Session("AdultChildCombntn") IsNot Nothing Then
        '    dtGridVal = Session("AdultChildCombntn")
        '    dtGridVal.Rows.Clear()

        '    For Each grdRow In gvOccupancy.Rows
        '        chkSelect = CType(grdRow.FindControl("chk"), HtmlInputCheckBox)
        '        Dim ddlRmcat As HtmlSelect = CType(grdRow.FindControl("ddlRmcat"), HtmlSelect)
        '        If chkSelect.Checked = False Then
        '            dtGridVal.Rows.Add(grdRow.Cells(1).Text, grdRow.Cells(2).Text)
        '        End If
        '    Next

        '    gvOccupancy.DataSource = dtGridVal
        '    gvOccupancy.DataBind()

        'End If
        'ModalPopupOccupancy.Show()

    End Sub
    Protected Sub btnUnselectAll_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUnselectAll.Click
        Dim chksel As HtmlInputCheckBox
        For Each grdRow In gvOccupancy.Rows
            chksel = grdRow.FindControl("chk")
            chksel.Checked = False
        Next
        ModalPopupOccupancy.Show()
    End Sub
    Protected Sub btnOccupancyCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOccupancyCancel.Click
        ModalPopupOccupancy.Hide()

    End Sub
    Protected Sub btnaddroomclass_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnaddroomclass.Click
        Dim strpop As String = ""
        strpop = "window.open('Roomclassification.aspx?State=New','Roomclassification');"
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
    End Sub
    Private Sub gv_DisableControl()
        Dim GVRow As GridViewRow
        Dim txtadult As TextBox
        Dim txtchild As TextBox
        Dim txtinfant As TextBox
        Dim txtPriceOccupancy As TextBox
        Dim txtMaxEB As TextBox
        Dim txtMaxOccpncy As TextBox
        Dim btnOccupancy As Button


        For Each GVRow In gv_FillData.Rows
            txtadult = GVRow.FindControl("txtadult")
            txtchild = GVRow.FindControl("txtchild")
            txtinfant = GVRow.FindControl("txtinfant")
            txtPriceOccupancy = GVRow.FindControl("txtPriceOccupancy")
            txtMaxEB = GVRow.FindControl("txtMaxEB")
            txtMaxOccpncy = GVRow.FindControl("txtMaxoccupancy")
            btnOccupancy = GVRow.FindControl("btnoccupancy")

            txtPriceOccupancy.Enabled = False
            txtMaxEB.Enabled = False
            txtadult.Enabled = False
            txtchild.Enabled = False
            txtinfant.Enabled = False
            txtMaxOccpncy.Enabled = False
            btnOccupancy.Enabled = False
        Next

    End Sub
    Private Sub DisableControl()
        If ViewState("State") = "New" Then
            btnAddLinesDates.Enabled = True
            btnclear.Enabled = True
            btnAddLinesDates.Enabled = True
            gv_FillData.Enabled = True
            btnAddrow.Enabled = True
            btndeleterow.Enabled = True
            btnaddroomclass.Enabled = True
            btnCopySelected.Enabled = True
            wucCountrygroup.Disable(True)

        ElseIf ViewState("State") = "View" Or ViewState("State") = "Delete" Then


            ' gv_DisableControl()
            btnclear.Enabled = False
            btnAddLinesDates.Enabled = False
            gv_FillData.Enabled = False
            btnAddrow.Enabled = False
            btndeleterow.Enabled = False
            btnaddroomclass.Enabled = False
            btnCopySelected.Enabled = False
            wucCountrygroup.Disable(False)


            For Each gvRow In Me.grdDates.Rows
                Dim txtFromDate As TextBox = gvRow.FindControl("txtfromdate")
                Dim txtToDate As TextBox = gvRow.FindControl("txttodate")
                Dim ImgPBtnFrmDt As ImageButton = gvRow.FindControl("ImgBtnFrmDt")
                Dim ImgPBtnToDt As ImageButton = gvRow.FindControl("ImgBtnToDt")
                ImgPBtnFrmDt.Enabled = False
                ImgPBtnToDt.Enabled = False
                txtFromDate.Enabled = False
                txtToDate.Enabled = False

            Next


        ElseIf ViewState("State") = "Edit" Then

            btnAddLinesDates.Enabled = True
            btnclear.Enabled = True
            btnAddLinesDates.Enabled = True
            gv_FillData.Enabled = True
            btnAddrow.Enabled = True
            btndeleterow.Enabled = True
            btnaddroomclass.Enabled = True
            btnCopySelected.Enabled = True
            wucCountrygroup.Disable(True)


        End If
    End Sub
#Region "Private Sub ShowDates(ByVal RefCode As String)"

    Private Sub ShowDates(ByVal RefCode As String)
        Try
            'Dim myds As New DataSet

            'If RefCode = "" Then
            '    strSqlQry = "Select '' frmdate,'' todate "
            'Else
            '    strSqlQry = "Select frmdate,todate from partymaxaccom_dates(nolock) Where tranid='" & RefCode & "'"
            'End If



            'SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
            'myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            'myDataAdapter.Fill(myDS)
            'grdDates.DataSource = myds

            'If myDS.Tables(0).Rows.Count > 0 Then
            '    grdDates.DataBind()
            'Else
            '    grdDates.PageIndex = 0
            '    grdDates.DataBind()

            'End If


            Dim gvRow As GridViewRow
            Dim dpFDate As TextBox
            Dim dpTDate As TextBox
            Dim ct As Integer = 0

            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open            
            mySqlCmd = New SqlCommand("Select * from view_partymaxaccom_dates(nolock) Where tranid='" & RefCode & "'", mySqlConn)
            mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
            If mySqlReader.HasRows Then
                ct = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "Select count(*) from view_partymaxaccom_dates(nolock) Where tranid='" & RefCode & "'")
                fillDategrd(grdDates, False, ct)
                While mySqlReader.Read()

                    For Each gvRow In grdDates.Rows
                        dpFDate = gvRow.FindControl("txtfromdate")
                        dpTDate = gvRow.FindControl("txttodate")
                        If dpFDate.Text = "" And dpFDate.Text = "" Then
                            If IsDBNull(mySqlReader("frmdate")) = False Then
                                dpFDate.Text = CType(Format(CType(mySqlReader("frmdate"), Date), "dd/MM/yyyy"), String)

                            End If
                            If IsDBNull(mySqlReader("todate")) = False Then
                                dpTDate.Text = CType(Format(CType(mySqlReader("todate"), Date), "dd/MM/yyyy"), String)
                            End If
                            Exit For
                        End If
                    Next
                End While
            End If


            'If chkctrygrp.Checked = True Then
            '    divuser.Style("display") = "block"
            'Else
            '    divuser.Style("display") = "none"
            'End If


        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ContractMaxocc.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            ' clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
            'clsDBConnect.dbConnectionClose(SqlConn)                          'Close connection  
            clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
            clsDBConnect.dbReaderClose(mySqlReader)             ' sql reader disposed    
            clsDBConnect.dbConnectionClose(mySqlConn)           'connection close   
        End Try
    End Sub
#End Region
    Protected Sub gv_SearchResult_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gv_SearchResult.RowCommand
        If e.CommandName = "Page" Then Exit Sub
        If e.CommandName = "Sort" Then Exit Sub

        Dim lbltran As Label
        Session("Maxid") = Nothing
        ViewState("MaxId") = Nothing
        lbltran = gv_SearchResult.Rows(CType(e.CommandArgument.ToString, String)).FindControl("lbltran")
        ' wucCountrygroup.clearsessions()

        'If ViewState("Menucalling") = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select option_selected  from reservation_parameters where param_id=1138") Then
        '    lblpromotion.Style.Add("display", "none")
        '    txtpromotionname.Style.Add("display", "none")


        'Else
        '    lblpromotion.Style.Add("display", "block")
        '    txtpromotionname.Style.Add("display", "block")

        'End If
        If e.CommandName <> "View" Then

            If Session("Calledfrom") = "Offers" Then
                Dim offerexists As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select 't' from  edit_offers_header(nolock) where promotionid='" & hdnpromotionid.Value & "'")

                If offerexists Is Nothing Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Save the Offer Main Details First');", True)
                    Exit Sub

                End If
            Else
                Dim contexists As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select 't' from  edit_contracts(nolock) where contractid='" & hdncontractid.Value & "'")

                If contexists Is Nothing Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Save the Contract Main Details First');", True)
                    Exit Sub

                End If
            End If
        End If

        If e.CommandName = "EditRow" Then

            ViewState("State") = "Edit"

            wucCountrygroup.sbSetPageState(lbltran.Text.Trim, Nothing, ViewState("State"))

            PanelMain.Visible = True
            PanelMain.Style("display") = "block"
            Panelsearch.Enabled = False
            Session.Add("RefCode", CType(lbltran.Text.Trim, String))
            Session.Add("Maxid", CType(lbltran.Text.Trim, String))
            ViewState("MaxId") = Session("Maxid")
            ShowRecord(CType(lbltran.Text.Trim, String))
            ShowRoomdetails(hdnpartycode.Value, CType(lbltran.Text.Trim, String))
            fillDategrd(grdDates, True)
            ShowDates(CType(lbltran.Text.Trim, String))
            Session("isAutoTick_wuccountrygroupusercontrol") = 1
            wucCountrygroup.sbShowCountry()
            DisableControl()
            fillcondetails()
            btnSave.Visible = True
            btnSave.Text = "Update"
            lbltext.Visible = True
            lblHeading.Text = "Edit Max Occupancy - " + ViewState("hotelname") + "- " + hdncontractid.Value
            Page.Title = "Contract Max Occupancy "

            If Session("Calledfrom") = "Offers" Then

                lblHeading.Text = "Edit Max Occupancy - " + ViewState("hotelname") + "- " + hdnpromotionid.Value
                Page.Title = "Promotion Max Occupancy "
                divdates.Style.Add("display", "none")
                lbltext.Visible = False
            Else
                divdates.Style.Add("display", "block")
                lbltext.Visible = True
            End If



        ElseIf e.CommandName = "View" Then
            ViewState("State") = "View"

            wucCountrygroup.sbSetPageState(lbltran.Text.Trim, Nothing, ViewState("State"))

            PanelMain.Visible = True
            PanelMain.Style("display") = "block"
            Panelsearch.Enabled = False
            Session.Add("RefCode", CType(lbltran.Text.Trim, String))
            Session.Add("Maxid", CType(lbltran.Text.Trim, String))
            ViewState("MaxId") = Session("Maxid")
            ShowRecord(CType(lbltran.Text.Trim, String))
            fillcondetails()
            ShowRoomdetails(hdnpartycode.Value, CType(lbltran.Text.Trim, String))
            fillDategrd(grdDates, True)
            ShowDates(CType(lbltran.Text.Trim, String))
            lbltext.Visible = True
            Session("isAutoTick_wuccountrygroupusercontrol") = 1
            wucCountrygroup.sbShowCountry()
            DisableControl()
            btnSave.Visible = False
            lblHeading.Text = "View Max Occupancy - " + ViewState("hotelname") + "- " + hdncontractid.Value
            Page.Title = "Contract Max Occupancy "

            If Session("Calledfrom") = "Offers" Then

                lblHeading.Text = "View Max Occupancy - " + ViewState("hotelname") + "- " + hdnpromotionid.Value
                Page.Title = "Promotion Max Occupancy "
                divdates.Style.Add("display", "none")
                lbltext.Visible = False
            Else
                divdates.Style.Add("display", "block")
                lbltext.Visible = True
            End If

        ElseIf e.CommandName = "DeleteRow" Then
            PanelMain.Visible = True
            ViewState("State") = "Delete"

            wucCountrygroup.sbSetPageState(lbltran.Text.Trim, Nothing, ViewState("State"))

            PanelMain.Style("display") = "block"
            Panelsearch.Enabled = False
            Session.Add("RefCode", CType(lbltran.Text.Trim, String))
            Session.Add("Maxid", CType(lbltran.Text.Trim, String))
            ViewState("MaxId") = Session("Maxid")
            ShowRecord(CType(lbltran.Text.Trim, String))
            fillcondetails()
            ShowRoomdetails(hdnpartycode.Value, CType(lbltran.Text.Trim, String))
            fillDategrd(grdDates, True)
            ShowDates(CType(lbltran.Text.Trim, String))
            Session("isAutoTick_wuccountrygroupusercontrol") = 1
            wucCountrygroup.sbShowCountry()
            DisableControl()

            lbltext.Visible = True
            btnSave.Visible = True
            btnSave.Text = "Delete"
            lblHeading.Text = "Delete Max Occupancy - " + ViewState("hotelname") + "- " + hdncontractid.Value
            Page.Title = "Contract Max Occupancy "
            If Session("Calledfrom") = "Offers" Then

                lblHeading.Text = "Delete Max Occupancy - " + ViewState("hotelname") + "- " + hdnpromotionid.Value
                Page.Title = "Promotion Max Occupancy "
                divdates.Style.Add("display", "none")
                lbltext.Visible = False
            Else
                divdates.Style.Add("display", "block")
                lbltext.Visible = True
            End If

        ElseIf e.CommandName = "Copy" Then
            PanelMain.Visible = True
            ViewState("State") = "Copy"

            wucCountrygroup.sbSetPageState(lbltran.Text.Trim, Nothing, ViewState("State"))

            PanelMain.Style("display") = "block"
            Panelsearch.Enabled = False
            Session.Add("RefCode", CType(lbltran.Text.Trim, String))
            Session.Add("Maxid", CType(lbltran.Text.Trim, String))
            ViewState("MaxId") = Session("Maxid")
            ShowRecord(CType(lbltran.Text.Trim, String))
            ShowRoomdetails(hdnpartycode.Value, CType(lbltran.Text.Trim, String))
            fillDategrd(grdDates, True)
            ShowDates(CType(lbltran.Text.Trim, String))
            fillcondetails()
            Session("isAutoTick_wuccountrygroupusercontrol") = 1
            wucCountrygroup.sbShowCountry()
            grdDates.Visible = True
            btnSave.Visible = True
            lbltext.Visible = True
            txtMaxid.Text = ""
            btnSave.Text = "Save"
            lblHeading.Text = "Copy Max Occupancy - " + ViewState("hotelname") + "- " + hdncontractid.Value
            Page.Title = "Contract Max Occupancy "

            If Session("Calledfrom") = "Offers" Then

                lblHeading.Text = "Copy Max Occupancy - " + ViewState("hotelname") + "- " + hdnpromotionid.Value
                Page.Title = "Promotion Max Occupancy "
                divdates.Style.Add("display", "none")
                lbltext.Visible = False
            Else
                divdates.Style.Add("display", "block")
                lbltext.Visible = True
            End If

        End If
    End Sub
    Protected Sub btndeletedates_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btndeletedates.Click

        'Createdatacolumns("delete")
        Dim count As Integer
        Dim GVRow As GridViewRow
        count = grdDates.Rows.Count + 1
        Dim fDate(count) As String
        Dim tDate(count) As String
        Dim excl(count) As String
        Dim n As Integer = 0
        Dim dpFDate As TextBox
        Dim dpTDate As TextBox
        Dim ddlExcl As HtmlSelect
        Dim chkSelect As CheckBox
        Dim delrows(count) As String
        Dim deletedrow As Integer = 0
        Dim k As Integer = 0

        Try
            For Each GVRow In grdDates.Rows
                chkSelect = GVRow.FindControl("chkSelect")
                If chkSelect.Checked = False Then
                    dpFDate = GVRow.FindControl("txtfromDate")
                    fDate(n) = CType(dpFDate.Text, String)
                    dpTDate = GVRow.FindControl("txtToDate")
                    tDate(n) = CType(dpTDate.Text, String)

                Else
                    deletedrow = deletedrow + 1
                End If
                n = n + 1
            Next

            count = n
            If count = 0 Then
                count = 1
            End If

            If grdDates.Rows.Count > 1 Then
                fillDategrd(grdDates, False, grdDates.Rows.Count - deletedrow)
            Else
                fillDategrd(grdDates, False, grdDates.Rows.Count)
            End If


            Dim i As Integer = n
            n = 0
            For Each GVRow In grdDates.Rows
                If GVRow.RowIndex < count Then
                    dpFDate = GVRow.FindControl("txtfromDate")
                    dpFDate.Text = fDate(n)
                    dpTDate = GVRow.FindControl("txtToDate")
                    dpTDate.Text = tDate(n)

                    n = n + 1
                End If
            Next

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            ' .writerrorlog(Server.MapPath("Errorlog.txt"), ex.ToString, 1, 1)
        End Try
    End Sub
    Protected Sub btndeleterow_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btndeleterow.Click
        'Dim dtGridVal As New DataTable
        'Dim chkselect As New CheckBox



        'dtGridVal.Rows.Clear()

        'For Each grdRow In gv_FillData.Rows
        '    chkselect = CType(grdRow.FindControl("chkSelect"), CheckBox)
        '    If chkSelect.Checked = False Then
        '        dtGridVal.Rows.Add(grdRow.Cells(1).Text, grdRow.Cells(2).Text)
        '    End If
        'Next

        'gv_FillData.DataSource = dtGridVal
        'gv_FillData.DataBind()


        For Each gvRow In gv_FillData.Rows
            Dim chkselect As CheckBox = CType(gvRow.FindControl("chkSelect"), CheckBox)
            Dim lblRoomTypeCode As Label = CType(gvRow.FindControl("lblRoomTypeCode"), Label)

            If chkselect.Checked = True And lblRoomTypeCode.Text <> "" Then

                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Existng Room type Please Make Inactive instead of Delete');", True)
                Exit Sub

            End If


        Next


        Createdatacolumns("delete")
    End Sub

    Protected Sub btnAddNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddNew.Click
        ViewState("State") = "New"

        wucCountrygroup.sbSetPageState("", Nothing, ViewState("State"))

        'If ViewState("Menucalling") = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select option_selected  from reservation_parameters where param_id=1138") Then
        '    lblpromotion.Style.Add("display", "none")
        '    txtpromotionname.Style.Add("display", "none")

        '    ' divpromotion.Style.Add("display", "none")
        'Else
        '    lblpromotion.Style.Add("display", "block")
        '    txtpromotionname.Style.Add("display", "block")
        '    'divpromotion.Style.Add("display", "block")
        'End If

        If Session("Calledfrom") = "Offers" Then

            divoffer.Style.Add("display", "block")
            lblstatus.Visible = False
            lblstatustext.Visible = False

            PanelMain.Visible = True
            PanelMain.Style("display") = "block"
            Panelsearch.Enabled = False
            ShowRoomdetails(hdnpartycode.Value, "")
            '  Session("contractid") = CType(Request.QueryString("contractid"), String)

            fillDategrd(grdDates, True)
            grdDates.Visible = True
            gv_FillData.Enabled = True
            btnSave.Visible = True
            lbltext.Visible = True
            btnSave.Text = "Save"
            fillcondetails()
            lblHeading.Text = "New Max Occupancy - " + ViewState("hotelname")
            Page.Title = Page.Title + " " + "New Max Occupancy -" + ViewState("hotelname")
            wucCountrygroup.sbSetPageState("", "OFFERSMAIN", CType(Session("OfferState"), String))
            wucCountrygroup.sbSetPageState(hdnpromotionid.Value, Nothing, "Edit")


            Session("isAutoTick_wuccountrygroupusercontrol") = 1
            wucCountrygroup.sbShowCountry()
            btnAddrow.Style.Add("display", "none")
            btndeleterow.Style.Add("display", "none")
            btnaddroomclass.Style.Add("display", "none")
            divdates.Style.Add("display", "none")
            lbltext.Visible = False

        Else
            divoffer.Style.Add("display", "none")
            lblstatus.Visible = False
            lblstatustext.Visible = False

            PanelMain.Visible = True
            PanelMain.Style("display") = "block"
            Panelsearch.Enabled = False
            ShowRoomdetails(hdnpartycode.Value, "")
            '  Session("contractid") = CType(Request.QueryString("contractid"), String)

            fillDategrd(grdDates, True)
            grdDates.Visible = True
            gv_FillData.Enabled = True
            btnSave.Visible = True
            lbltext.Visible = True
            btnSave.Text = "Save"
            fillcondetails()
            lblHeading.Text = "New Max Occupancy - " + ViewState("hotelname")
            Page.Title = Page.Title + " " + "New Max Occupancy -" + ViewState("hotelname")
            Session("isAutoTick_wuccountrygroupusercontrol") = 1
            wucCountrygroup.sbShowCountry()

            btnAddrow.Style.Add("display", "block")
            btndeleterow.Style.Add("display", "block")
            btnaddroomclass.Style.Add("display", "block")
            divdates.Style.Add("display", "block")
            lbltext.Visible = True
        End If



    End Sub

    Protected Sub btnreset1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnreset1.Click
        '   Divmain.Style("display") = "none"
        Panelsearch.Enabled = True
        Panelsearch.Enabled = True
        PanelMain.Style("display") = "none"


        If Session("Calledfrom") = "Offers" Then
            lblHeading.Text = "Max Occupancy -" + ViewState("hotelname") + "- " + hdnpromotionid.Value
            '  wucCountrygroup.clearsessions()
            wucCountrygroup.sbSetPageState("", "MAXOCCUPANCY", CType(Session("ContractState"), String))
            ddlOrder.SelectedIndex = 0
            ddlorderby.SelectedIndex = 1


            FillGrid("view_partymaxacc_header.tranid", hdnpartycode.Value, "DESC")

        Else
            lblHeading.Text = "Max Occupancy -" + ViewState("hotelname") + "- " + hdncontractid.Value
            '  wucCountrygroup.clearsessions()
            wucCountrygroup.sbSetPageState("", "MAXOCCUPANCY", CType(Session("ContractState"), String))
            ddlOrder.SelectedIndex = 0
            ddlorderby.SelectedIndex = 1


            FillGrid("view_partymaxacc_header.tranid", hdnpartycode.Value, "DESC")
        End If






        ' Response.Redirect(Request.RawUrl)
    End Sub




    Public Sub sortgvsearch()
        Select Case ddlOrder.SelectedIndex
            Case 0
                FillGrid("view_partymaxacc_header.tranid", hdnpartycode.Value, ddlorderby.SelectedItem.Text.Trim)
            Case 1
                FillGrid("view_partymaxacc_header.frmdate", hdnpartycode.Value, ddlorderby.SelectedItem.Text.Trim)
            Case 2
                FillGrid("view_partymaxacc_header.todate", hdnpartycode.Value, ddlorderby.SelectedItem.Text.Trim)
            Case 3
                FillGrid("view_partymaxacc_header.status", hdnpartycode.Value, ddlorderby.SelectedItem.Text.Trim)
            Case 4
                FillGrid("view_partymaxacc_header.adddate", hdnpartycode.Value, ddlorderby.SelectedItem.Text.Trim)
            Case 5
                FillGrid("view_partymaxacc_header.adduser", hdnpartycode.Value, ddlorderby.SelectedItem.Text.Trim)
            Case 6
                FillGrid("view_partymaxacc_header.moddate", hdnpartycode.Value, ddlorderby.SelectedItem.Text.Trim)
            Case 7
                FillGrid("view_partymaxacc_header.moduser", hdnpartycode.Value, ddlorderby.SelectedItem.Text.Trim)
        End Select
    End Sub


    Protected Sub ddlOrder_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlOrder.SelectedIndexChanged
        sortgvsearch()
    End Sub

    Protected Sub ddlorderby_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlorderby.SelectedIndexChanged
        sortgvsearch()
    End Sub


    Protected Sub grdpromotion_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles grdpromotion.RowCommand
        Try
            If e.CommandName = "moreless" Then
                Exit Sub
            End If
            Dim lblpromotionid As Label
            Dim lblpromotionname As Label, lblapplicable As Label, lblplistcode As Label
            ViewState("OfferCopy") = 0
            Dim lbltran As Label

            lbltran = grdpromotion.Rows(CType(e.CommandArgument.ToString, String)).FindControl("lbltranid")
            lblpromotionid = grdpromotion.Rows(CType(e.CommandArgument.ToString, String)).FindControl("lblpromotionid")
            lblpromotionname = grdpromotion.Rows(CType(e.CommandArgument.ToString, String)).FindControl("lblpromotionname")
            lblapplicable = grdpromotion.Rows(CType(e.CommandArgument.ToString, String)).FindControl("lblapplicableto")
            lblplistcode = grdpromotion.Rows(CType(e.CommandArgument.ToString, String)).FindControl("lblplistcode")
            If lblpromotionid.Text.Trim = "" Then Exit Sub
            If e.CommandName = "Select" Then

                PanelMain.Visible = True
                ViewState("State") = "Copy"
                Session.Add("RefCode", CType(lblpromotionid.Text.Trim, String))
                PanelMain.Style("display") = "block"

                ShowRecord(CType(lbltran.Text.Trim, String))

                ShowRoomdetailsoffers(hdnpartycode.Value, CType(lbltran.Text.Trim, String))
                fillDategrd(grdDates, True)
                ShowDates(CType(lbltran.Text.Trim, String))






                lblHeading.Text = "Copy Max Occupancy - - " + ViewState("hotelname") + " - " + hdnpromotionid.Value

                Page.Title = "Promotion Max Occupancy - "



                btnSave.Visible = True
                PanelMain.Visible = True

                wucCountrygroup.sbSetPageState(hdnpromotionid.Value, Nothing, ViewState("State"))

                Session("isAutoTick_wuccountrygroupusercontrol") = 1
                wucCountrygroup.sbShowCountry()

                btnSave.Visible = True
                btnSave.Text = "Save"





            End If
        Catch ex As Exception
            objUtils.WritErrorLog("ContractMealSupplements.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub

    Protected Sub gvOccupancy_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvOccupancy.RowDataBound
      
    End Sub
End Class
