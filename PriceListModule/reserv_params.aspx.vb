Imports System.Data
Imports System.Data.SqlClient
Partial Class PriceListModule_reserv_params
    Inherits System.Web.UI.Page
    Dim objUtils As New clsUtils
    Dim mySqlConn As SqlConnection
    Dim mydataadapter As SqlDataAdapter
    Dim sqlTrans As SqlTransaction
    Dim mySqlCmd As SqlCommand



    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Page.IsPostBack = False Then
            Try

                If CType(Session("GlobalUserName"), String) = "" Then
                    Response.Redirect("Login.aspx", False)
                    Exit Sub
                End If


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


                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlcurrcode, "currcode", "currname", "select currcode,currname from currmast  where active =1 order by currcode", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlcurrname, "currname", "currcode", "select currname,currcode from currmast where active =1 order by currname", True)
                'select  sptypecode,sptypename from sptypemast where active=1

                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlcostcentercode, "costcenter_code", "costcenter_name", "select costcenter_code,costcenter_name  from costcenter_master where active =1 order by costcenter_code", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlcostcentername, "costcenter_name", "costcenter_code", "select costcenter_name,costcenter_code  from costcenter_master where active =1 order by costcenter_name", True)


                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlcountrycode, "ctrycode", "ctryname", "select ctrycode,ctryname from ctrymast where active=1 order by ctrycode", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlcountryname, "ctryname", "ctrycode", "select ctryname ,ctrycode from ctrymast where active =1 order by ctryname", True)


                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlproair, "sptypecode", "sptypename", "select  sptypecode,sptypename from sptypemast where active=1 order by sptypecode", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlproairname, "sptypename", "sptypecode", "select  sptypename,sptypecode from sptypemast where active=1 order by sptypename", True)


                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlprovidercode, "sptypecode", "sptypename", "select  sptypecode,sptypename from sptypemast where active=1 order by sptypecode", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlprovidername, "sptypename", "sptypecode", "select  sptypename,sptypecode from sptypemast where active=1 order by sptypename", True)

                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlsupagent, "supagentcode", "supagentname", "select  supagentcode,supagentname from supplier_agents where active=1 order by supagentcode", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlsupagentname, "supagentname", "supagentcode", "select  supagentname,supagentcode from supplier_agents where active=1 order by supagentname", True)


                fillparamvalue()

                btnsave_11.Attributes.Add("onclick", "return FormValidation()")




            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objUtils.WritErrorLog("reserv_params.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))


            End Try





        End If

    End Sub

    Protected Sub fillparamvalue()

        Dim myDS As New DataSet
        Dim strsqlqry As String

        strsqlqry = "select param_id,option_selected from reservation_parameters where param_id  in (457,458,459,500, 501 ,502, 503, 504, 505 ,506, 507 ,508, 509, 510 ,512 ,513, 514, 515, 516, 517, 518, 519,520,547,548,552) "


        mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
        mydataadapter = New SqlDataAdapter(strsqlqry, mySqlConn)
        mydataadapter.Fill(myDS)


        If myDS.Tables(0).Rows.Count > 0 Then
            For i As Integer = 0 To myDS.Tables(0).Rows.Count - 1

                Select Case myDS.Tables(0).Rows(i).Item(0)
                    Case 457
                        If myDS.Tables(0).Rows(i).Item(0) = Txt_paramid1.Text Then
                            ' txtparamval_1.Text = myDS.Tables(0).Rows(i).Item(1)

                            ddlcurrname.Value = myDS.Tables(0).Rows(i).Item(1)
                            ddlcurrcode.Value = CType(ddlcurrname.Items(ddlcurrname.SelectedIndex).Text, String)
                        End If
                    Case 458
                        If myDS.Tables(0).Rows(i).Item(0) = Txt_paramid2.Text Then
                            ' txtparamval_2.Text = myDS.Tables(0).Rows(i).Item(1)
                            ddlprovidername.Value = myDS.Tables(0).Rows(i).Item(1)
                            ddlprovidercode.Value = CType(ddlprovidername.Items(ddlprovidername.SelectedIndex).Text, String)

                        End If
                    Case 459
                        If myDS.Tables(0).Rows(i).Item(0) = Txt_paramid3.Text Then
                            ' txtparamval_3.Text = myDS.Tables(0).Rows(i).Item(1)
                            ddlcountryname.Value = myDS.Tables(0).Rows(i).Item(1)

                            ddlcountrycode.Value = CType(ddlcountryname.Items(ddlcountryname.SelectedIndex).Text, String)

                        End If
                    Case 500
                        If myDS.Tables(0).Rows(i).Item(0) = Txt_paramid4.Text Then
                            'txtparamval_4.Text = myDS.Tables(0).Rows(i).Item(1)
                            ddlproairname.Value = myDS.Tables(0).Rows(i).Item(1)
                            ddlproair.Value = CType(ddlproairname.Items(ddlproairname.SelectedIndex).Text, String)


                        End If
                    Case 501
                        If myDS.Tables(0).Rows(i).Item(0) = Txt_paramid5.Text Then
                            txtparamval_5.Text = myDS.Tables(0).Rows(i).Item(1)

                        End If
                    Case 502
                        If myDS.Tables(0).Rows(i).Item(0) = Txt_paramid6.Text Then
                            txtparamval_6.Text = myDS.Tables(0).Rows(i).Item(1)

                        End If
                    Case 503
                        If myDS.Tables(0).Rows(i).Item(0) = Txt_paramid7.Text Then
                            ddlres_reqdate.SelectedValue = myDS.Tables(0).Rows(i).Item(1)

                        End If
                    Case 504
                        If myDS.Tables(0).Rows(i).Item(0) = Txt_paramid8.Text Then
                            txtparamval_8.Text = myDS.Tables(0).Rows(i).Item(1)

                        End If
                    Case 505
                        If myDS.Tables(0).Rows(i).Item(0) = Txt_paramid9.Text Then
                            txtparamval_9.Text = myDS.Tables(0).Rows(i).Item(1)

                        End If
                    Case 506
                        If myDS.Tables(0).Rows(i).Item(0) = Txt_paramid10.Text Then
                            txtparamval_10.Text = myDS.Tables(0).Rows(i).Item(1)

                        End If
                    Case 507
                        If myDS.Tables(0).Rows(i).Item(0) = Txt_paramid11.Text Then
                            txtparamval_11.Text = myDS.Tables(0).Rows(i).Item(1)
                            Dim myreader As SqlDataReader

                            myreader = objUtils.GetDataFromReadernew(Session("dbconnectionName"), "select count(acctcode) count1 ,min(acctcode) code  from acctgroup")

                            myreader.Read()

                            If myreader.Item("count1") = 1 And myreader.Item("code") = "0" Then
                            Else
                                If myreader.Item("count1") <> 0 Then

                                    txtparamval_11.Enabled = False
                                    btnsave_11.Enabled = False

                                End If

                            End If

                        End If

                    Case 508
                        If myDS.Tables(0).Rows(i).Item(0) = Txt_paramid12.Text Then
                            dtpdate.txtDate.Text = myDS.Tables(0).Rows(i).Item(1)
                        End If
                    Case 509
                        If myDS.Tables(0).Rows(i).Item(0) = Txt_paramid13.Text Then
                            txtparamval_13.Text = myDS.Tables(0).Rows(i).Item(1)

                        End If
                    Case 510
                        If myDS.Tables(0).Rows(i).Item(0) = Txt_paramid14.Text Then
                            'txtparamval_14.Text = myDS.Tables(0).Rows(i).Item(1)
                            ddlcostcentername.Value = myDS.Tables(0).Rows(i).Item(1)
                            ddlcostcentercode.Value = CType(ddlcostcentername.Items(ddlcostcentername.SelectedIndex).Text, String)
                        End If
                    Case 512
                        If myDS.Tables(0).Rows(i).Item(0) = Txt_paramid15.Text Then
                            txtparamval_15.Text = myDS.Tables(0).Rows(i).Item(1)
                        End If
                    Case 513
                        If myDS.Tables(0).Rows(i).Item(0) = Txt_paramid16.Text Then
                            txtparamval_16.Text = myDS.Tables(0).Rows(i).Item(1)
                        End If
                    Case 514
                        If myDS.Tables(0).Rows(i).Item(0) = Txt_paramid17.Text Then
                            txtparamval_17.Text = myDS.Tables(0).Rows(i).Item(1)
                        End If
                    Case 515
                        If myDS.Tables(0).Rows(i).Item(0) = Txt_paramid18.Text Then
                            txtparamval_18.Text = myDS.Tables(0).Rows(i).Item(1)

                        End If
                    Case 516
                        If myDS.Tables(0).Rows(i).Item(0) = Txt_paramid19.Text Then
                            ddlenforce.SelectedValue = myDS.Tables(0).Rows(i).Item(1)
                        End If
                    Case 517
                        If myDS.Tables(0).Rows(i).Item(0) = Txt_paramid20.Text Then
                            txtparamval_20.Text = myDS.Tables(0).Rows(i).Item(1)

                        End If
                    Case 518
                        If myDS.Tables(0).Rows(i).Item(0) = Txt_paramid21.Text Then
                            txtparamval_21.Text = myDS.Tables(0).Rows(i).Item(1)

                        End If
                    Case 519
                        If myDS.Tables(0).Rows(i).Item(0) = Txt_paramid22.Text Then
                            txtparamval_22.Text = myDS.Tables(0).Rows(i).Item(1)

                        End If
                    Case 520
                        If myDS.Tables(0).Rows(i).Item(0) = Txt_paramid23.Text Then
                            'txtparamval_4.Text = myDS.Tables(0).Rows(i).Item(1)
                            ddlsupagentname.Value = myDS.Tables(0).Rows(i).Item(1)
                            ddlsupagent.Value = CType(ddlsupagentname.Items(ddlsupagentname.SelectedIndex).Text, String)
                        End If
                    Case 547
                        If CType(myDS.Tables(0).Rows(i).Item(0), String) = Txt_paramid547.Text Then
                            txtparamval_547.Text = CType(myDS.Tables(0).Rows(i).Item(1), String)
                        End If
                    Case 548
                        If CType(myDS.Tables(0).Rows(i).Item(0), String) = Txt_paramid548.Text Then
                            txtparamval_548.Text = CType(myDS.Tables(0).Rows(i).Item(1), String)
                        End If
                    Case 552
                        If CType(myDS.Tables(0).Rows(i).Item(0), String) = Txt_paramid552.Text Then
                            txtparamval_552.Text = CType(myDS.Tables(0).Rows(i).Item(1), String)
                        End If

                End Select
            Next
        End If





    End Sub
    Protected Sub save(ByVal id As Integer, ByVal opt_val As String)
        Try

            If Page.IsValid = True Then
                mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                sqlTrans = mySqlConn.BeginTransaction
                mySqlCmd = New SqlCommand("update_param_values", mySqlConn, sqlTrans)
                'SQL  Trans start
                mySqlCmd.CommandType = CommandType.StoredProcedure
                mySqlCmd.Parameters.Add(New SqlParameter("@id", SqlDbType.Int)).Value = id
                mySqlCmd.Parameters.Add(New SqlParameter("@op_sel", SqlDbType.VarChar, 100)).Value = opt_val
                mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                mySqlCmd.ExecuteNonQuery()
            End If



            sqlTrans.Commit()    'SQl Tarn Commit
            clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
            clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
            clsDBConnect.dbConnectionClose(mySqlConn)           'connection close


        Catch ex As Exception
            If mySqlConn.State = ConnectionState.Open Then
                sqlTrans.Rollback()
            End If
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("reserv_params.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))

        End Try



    End Sub


    Protected Sub btnsave_1_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        save(457, ddlcurrcode.Items(ddlcurrcode.SelectedIndex).ToString)

    End Sub

    Protected Sub btnsave_2_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        save(458, ddlprovidercode.Items(ddlprovidercode.SelectedIndex).ToString)

    End Sub

    Protected Sub btnsave_3_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        save(459, ddlcountrycode.Items(ddlcountrycode.SelectedIndex).ToString)
    End Sub

    Protected Sub btnsave_4_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        save(500, ddlproair.Items(ddlproair.SelectedIndex).ToString)
    End Sub
    Protected Sub btnsave_5_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        save(501, txtparamval_5.Text)
    End Sub
    Protected Sub btnsave_6_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        save(502, txtparamval_6.Text)
    End Sub
    Protected Sub btnsave_7_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        save(503, ddlres_reqdate.SelectedValue.ToString)
    End Sub
    Protected Sub btnsave_8_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        save(504, txtparamval_8.Text)

    End Sub
    Protected Sub btnsave_9_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        save(505, txtparamval_9.Text)
    End Sub
    Protected Sub btnsave_10_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        save(506, txtparamval_10.Text)
    End Sub
    Protected Sub btnsave_11_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        save(507, txtparamval_11.Text)
    End Sub
    Protected Sub btnsave_12_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        save(508, dtpdate.txtDate.Text)
    End Sub
    Protected Sub btnsave_13_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        save(509, txtparamval_13.Text)
    End Sub
    Protected Sub btnsave_14_Click(ByVal sender As Object, ByVal e As System.EventArgs)

        save(510, ddlcostcentercode.Items(ddlcostcentercode.SelectedIndex).ToString)
    End Sub
    Protected Sub btnsave_15_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        save(512, txtparamval_15.Text)
    End Sub
    Protected Sub btnsave_16_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        save(513, txtparamval_16.Text)
    End Sub
    Protected Sub btnsave_17_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        save(514, txtparamval_17.Text)
    End Sub
    Protected Sub btnsave_18_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        save(515, txtparamval_18.Text)
    End Sub
    Protected Sub btnsave_19_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        save(516, ddlenforce.SelectedValue.ToString)
    End Sub
    Protected Sub btnsave_20_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        save(517, txtparamval_20.Text)
    End Sub
    Protected Sub btnsave_21_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        save(518, txtparamval_21.Text)
    End Sub
    Protected Sub btnsave_22_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        save(519, txtparamval_22.Text)
    End Sub

    Protected Sub ddlprovidercode_ServerChange(ByVal sender As Object, ByVal e As System.EventArgs)

    End Sub


    Protected Sub btnsave_23_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        save(520, ddlsupagent.Items(ddlsupagent.SelectedIndex).ToString)
    End Sub
    Protected Sub btnsave_547_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        save(547, txtparamval_547.Text)
    End Sub
    Protected Sub btnsave_548_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        save(548, txtparamval_548.Text)
    End Sub

    Protected Sub btnsave_552_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        save(552, txtparamval_552.Text)
    End Sub
End Class
