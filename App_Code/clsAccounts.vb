Imports System.Data
Imports System.Data.SqlClient

Public Class clsAccounts
    Public mgroups As New Collection
    Public mopens As New Collection
    Public mdiffs As New Collection '+Debit,- Credit
    Public cgroups As New Collection
    Private Acctrans As Collection 'changed from private to public
    Private Accsubtrans As Collection 'changed from private to public
    Private entry As Collection
    Public acccode As String
    Public acctype As String
    Public accname As String
    Public curcode As String
    Public currate As Double
    Public amount As Double
    Public group_dr_cr As Integer
    Public tran_mode As Integer
    Public acc_div_id As String
    Public acc_tran_id As String
    Public acc_tran_lineno As String
    Public acc_tran_type As String
    Public acc_tran_date As Date
    Public entry_id As Integer
    Public entry_detail_id As Integer
    Public table_name As String
    Public crdays As Integer
    Public salescode As String
    Public salesname As String
    Public acc_narration As String
    Private subopen As Collection
    Private exchg_tran_id As String
    Public acc_exchg_Acc_code As String
    Public acc_base_currency As String
    Public fromdate As String
    Public todate As String
    Public glcode As String

    Public ReadOnly Property getAccsubtrans() As Collection
        Get
            Return Accsubtrans
        End Get
    End Property

    Sub start()
        Acctrans = Nothing
        Acctrans = New Collection
        Accsubtrans = Nothing
        Accsubtrans = New Collection
        mdiffs = Nothing
        mdiffs = New Collection
        mopens = Nothing
        mopens = New Collection
        subopen = New Collection
        mtran_mode = tran_mode

        Dim objutils As New clsUtils
        mdecimal = objutils.ExecuteQueryReturnStringValuenew("strDBConnection", "select option_selected from reservation_parameters where param_id=509") '2
    End Sub
    Sub addtran(ByVal tran As clstran)
        Acctrans.Add(tran)
    End Sub
    Sub addsubtran(ByVal tran As clsSubTran)
        Accsubtrans.Add(tran)
    End Sub
    Sub saveerr()
        Acctrans = Nothing
        Accsubtrans = Nothing
        Acctrans = New Collection
        Accsubtrans = New Collection
    End Sub
    Sub addopen(ByVal tran As clsOpen)
        On Error Resume Next
        subopen.Add(tran)
        mopens.Remove(CStr(acc_tran_lineno))
        mopens.Add(subopen, CStr(acc_tran_lineno))
    End Sub
    Function saveentry(ByVal constr As String, ByRef scon As SqlConnection, ByRef stran As SqlTransaction, ByVal accpage As Page) As Integer
        On Error GoTo errsave
        Dim tran_code As String
        Dim stproc1 As String
        saveentry = 0
        Dim i, j As Integer
        Dim objutils As New clsUtils

        createopenscollection(constr, acc_div_id, acc_tran_id, acc_tran_type, scon, stran)

        If validateamount(constr, accpage) = False Then
            saveentry = 2
            acc_tran_id = ""
            Exit Function
        End If

        If validateaccount(constr, scon, stran, accpage) = False Then
            saveentry = 3
            acc_tran_id = ""
            Exit Function
        End If

        stproc1 = "sp_delacctran"
        Dim scmd As New SqlCommand(stproc1, scon, stran)
        scmd.CommandText = stproc1
        scmd.CommandType = CommandType.StoredProcedure
        scmd.Connection = scon
        scmd.Parameters.Add(New SqlParameter("@tablename", ""))
        scmd.Parameters.Add(New SqlParameter("@tranid", acc_tran_id))
        scmd.Parameters.Add(New SqlParameter("@trantype", acc_tran_type))
        scmd.Parameters.Add(New SqlParameter("@acc_div_id", acc_div_id))
        scmd.CommandTimeout = 0 'Tanvir 15062022
        scmd.ExecuteNonQuery()
        scmd = Nothing

        For i = 1 To Acctrans.Count
            stproc1 = "sp_add_acc_tran"
            scmd = New SqlCommand(stproc1, scon, stran)
            scmd.CommandText = stproc1
            scmd.CommandType = CommandType.StoredProcedure
            scmd.Connection = scon

            scmd.Parameters.Add(New SqlParameter("@acc_tran_id", Trim(Acctrans(i).acc_tran_id)))
            scmd.Parameters.Add(New SqlParameter("@acc_div_id", Acctrans(i).acc_div_id))
            scmd.Parameters.Add(New SqlParameter("@acc_tran_lineno", Acctrans(i).acc_tran_lineno))
            scmd.Parameters.Add(New SqlParameter("@acc_tran_type", Acctrans(i).acc_tran_type))
            scmd.Parameters.Add(New SqlParameter("@acc_tran_date", Acctrans(i).acc_tran_date))
            scmd.Parameters.Add(New SqlParameter("@acc_type", Acctrans(i).acc_type))
            scmd.Parameters.Add(New SqlParameter("@acc_code", Acctrans(i).acc_code))
            scmd.Parameters.Add(New SqlParameter("@acc_gl_code", Acctrans(i).pacc_gl_code))
            scmd.Parameters.Add(New SqlParameter("@acc_narration", Acctrans(i).acc_narration))
            scmd.Parameters.Add(New SqlParameter("@currcode", Acctrans(i).acc_currency_id))
            scmd.Parameters.Add(New SqlParameter("@acc_currency_rate", Acctrans(i).acc_currency_rate))
            scmd.Parameters.Add(New SqlParameter("@acc_ref1", Acctrans(i).acc_ref1))
            scmd.Parameters.Add(New SqlParameter("@acc_ref2", Acctrans(i).acc_ref2))
            scmd.Parameters.Add(New SqlParameter("@acc_ref3", Acctrans(i).acc_ref3))
            scmd.Parameters.Add(New SqlParameter("@acc_ref4", Acctrans(i).acc_ref4))
            scmd.CommandTimeout = 0 'Tanvir 15062022
            scmd.ExecuteNonQuery()
            scmd = Nothing

        Next


        For i = 1 To Accsubtrans.Count
            stproc1 = "sp_add_acc_sub_tran"
            scmd = New SqlCommand(stproc1, scon, stran)
            scmd.CommandText = stproc1
            scmd.CommandType = CommandType.StoredProcedure
            scmd.Connection = scon
            scmd.Parameters.Add(New SqlParameter("@acc_tran_id", Trim(acc_tran_id)))
            scmd.Parameters.Add(New SqlParameter("@acc_tran_lineno", Accsubtrans(i).acc_tran_lineno))
            scmd.Parameters.Add(New SqlParameter("@acc_tran_type", Accsubtrans(i).acc_tran_type))
            scmd.Parameters.Add(New SqlParameter("@acc_against_tran_id", Trim(acc_tran_id)))
            scmd.Parameters.Add(New SqlParameter("@acc_against_tran_lineno", Accsubtrans(i).acc_against_tran_lineno))
            scmd.Parameters.Add(New SqlParameter("@acc_against_tran_type", Accsubtrans(i).acc_against_tran_type))
            scmd.Parameters.Add(New SqlParameter("@acc_debit", Accsubtrans(i).acc_debit))
            scmd.Parameters.Add(New SqlParameter("@acc_credit", Accsubtrans(i).acc_credit))
            scmd.Parameters.Add(New SqlParameter("@acc_tran_date", IIf(Accsubtrans(i).acc_tran_date < DateAdd(DateInterval.Year, -1000, Now), DBNull.Value, Accsubtrans(i).acc_tran_date)))
            scmd.Parameters.Add(New SqlParameter("@acc_due_date", IIf(Accsubtrans(i).acc_tran_date < DateAdd(DateInterval.Year, -1000, Now), DBNull.Value, Accsubtrans(i).acc_due_date)))
            scmd.Parameters.Add(New SqlParameter("@acc_field1", Accsubtrans(i).acc_field1))
            scmd.Parameters.Add(New SqlParameter("@acc_field2", Accsubtrans(i).acc_field2))
            scmd.Parameters.Add(New SqlParameter("@acc_field3", Accsubtrans(i).acc_field3))
            scmd.Parameters.Add(New SqlParameter("@acc_field4", Accsubtrans(i).acc_field4))
            scmd.Parameters.Add(New SqlParameter("@acc_field5", Accsubtrans(i).acc_field5))
            scmd.Parameters.Add(New SqlParameter("@acc_type", Accsubtrans(i).acc_type))
            scmd.Parameters.Add(New SqlParameter("@acc_div_id", acc_div_id))
            scmd.Parameters.Add(New SqlParameter("@acc_base_debit", Accsubtrans(i).acc_base_debit))
            scmd.Parameters.Add(New SqlParameter("@acc_base_credit", Accsubtrans(i).acc_base_credit))
            scmd.Parameters.Add(New SqlParameter("@costcentercode", Accsubtrans(i).costcentercode))
            scmd.Parameters.Add(New SqlParameter("@acc_currency_rate", Accsubtrans(i).currate))
            scmd.CommandTimeout = 0 'Tanvir 15062022
            scmd.ExecuteNonQuery()
            scmd = Nothing
        Next

        For j = 1 To mopens.Count
            For i = 1 To mopens(j).Count
                If mopens(j).Item(i).acc_debit + mopens(j).Item(i).acc_credit <> 0 Then

                    stproc1 = "sp_add_open_detail_acc"
                    scmd = New SqlCommand(stproc1, scon, stran)
                    scmd.CommandText = stproc1
                    scmd.CommandType = CommandType.StoredProcedure
                    scmd.Connection = scon
                    scmd.Parameters.Add(New SqlParameter("@tran_id", IIf(Trim(mopens(j).Item(i).tran_id) = "", Trim(acc_tran_id), Trim(mopens(j).Item(i).tran_id))))
                    scmd.Parameters.Add(New SqlParameter("@tran_date", mopens(j).Item(i).tran_date))
                    scmd.Parameters.Add(New SqlParameter("@tran_type", mopens(j).Item(i).tran_type))
                    scmd.Parameters.Add(New SqlParameter("@tran_lineno", mopens(j).Item(i).tran_lineno))
                    scmd.Parameters.Add(New SqlParameter("@against_tran_id", Trim(acc_tran_id)))
                    scmd.Parameters.Add(New SqlParameter("@against_tran_lineno", mopens(j).Item(i).against_tran_lineno))
                    scmd.Parameters.Add(New SqlParameter("@against_tran_type", acc_tran_type))
                    scmd.Parameters.Add(New SqlParameter("@against_tran_date", acc_tran_date))
                    scmd.Parameters.Add(New SqlParameter("@open_due_date", mopens(j).Item(i).acc_due_date))
                    scmd.Parameters.Add(New SqlParameter("@open_sales_code", mopens(j).Item(i).tran_sales_code))
                    scmd.Parameters.Add(New SqlParameter("@open_debit", Val(mopens(j).Item(i).acc_debit)))
                    scmd.Parameters.Add(New SqlParameter("@open_credit", Val(mopens(j).Item(i).acc_credit)))
                    scmd.Parameters.Add(New SqlParameter("@open_field1", Trim(mopens(j).Item(i).acc_field1)))
                    scmd.Parameters.Add(New SqlParameter("@open_field2", Trim(mopens(j).Item(i).acc_field2)))
                    scmd.Parameters.Add(New SqlParameter("@open_field3", Trim(mopens(j).Item(i).acc_field3)))
                    scmd.Parameters.Add(New SqlParameter("@open_field4", Trim(mopens(j).Item(i).acc_field4)))
                    scmd.Parameters.Add(New SqlParameter("@open_field5", Trim(mopens(j).Item(i).acc_field5)))
                    scmd.Parameters.Add(New SqlParameter("@open_narration", Trim(mopens(j).Item(i).tran_narration)))
                    scmd.Parameters.Add(New SqlParameter("@acc_type", mopens(j).Item(i).acc_type))
                    scmd.Parameters.Add(New SqlParameter("@open_mode", mopens(j).Item(i).open_mode))
                    scmd.Parameters.Add(New SqlParameter("@open_exchg_diff", mopens(j).Item(i).open_Exchg_diff))
                    scmd.Parameters.Add(New SqlParameter("@div_id", acc_div_id))
                    scmd.Parameters.Add(New SqlParameter("@acc_base_debit", mopens(j).Item(i).acc_base_debit))
                    scmd.Parameters.Add(New SqlParameter("@acc_base_credit", mopens(j).Item(i).acc_base_credit))
                    scmd.Parameters.Add(New SqlParameter("@acc_currency_rate", mopens(j).Item(i).cur_Rate))
                    scmd.CommandTimeout = 0 'Tanvir 15062022
                    scmd.ExecuteNonQuery()
                    scmd = Nothing
                End If
            Next
        Next



        Exit Function
errsave:
        objutils.MessageBox("Exception Error from Accounts Posting " & Err.Description, accpage)
        saveentry = 1
        acc_tran_id = ""
    End Function
    Function validateamount(ByVal constr As String, ByVal accpage As Page) As Boolean
        Dim val_credit As Double
        Dim val_debit As Double
        Dim i, j As Integer
        Dim objutils As New clsUtils
        validateamount = True
        Dim mbasecurrency, acccurrency As String
        mbasecurrency = objutils.ExecuteQueryReturnStringValuenew(constr, "select option_selected from reservation_parameters where param_id=457")
        val_credit = 0
        val_debit = 0

        For i = 1 To Accsubtrans.Count
            If Math.Abs(DecRound(Accsubtrans(i).acc_base_debit, mdecimal) - DecRound(Accsubtrans(i).acc_debit * Accsubtrans(i).currate, mdecimal)) > 1 Then
                objutils.MessageBox("There is a mis match in the Debit and Debit Base Amount for line no " & Accsubtrans(i).acc_tran_lineno, accpage)
                validateamount = False
            End If
            If Math.Abs(DecRound(Accsubtrans(i).acc_base_credit, mdecimal) - DecRound(Accsubtrans(i).acc_credit * Accsubtrans(i).currate, mdecimal)) > 1 Then
                objutils.MessageBox("There is a mis match in the Credit and Credit Base Amount for line no " & Accsubtrans(i).acc_tran_lineno, accpage)
                validateamount = False
            End If
            acccurrency = objutils.ExecuteQueryReturnStringValuenew(constr, "select cur from view_account where type='" & Accsubtrans(i).acc_type & "' and code='" & Accsubtrans(i).acc_code & "'")
            If acccurrency = mbasecurrency And Accsubtrans(i).currate <> 1 Then
                objutils.MessageBox("Conversion Rate for " & mbasecurrency & "  Accounts should be 1", accpage)
                validateamount = False
            End If
            val_debit = DecRound(val_debit, mdecimal) + DecRound(Accsubtrans(i).acc_base_debit, mdecimal)
            val_credit = DecRound(val_credit, mdecimal) + DecRound(Accsubtrans(i).acc_base_credit, mdecimal)
        Next

        'sharfudeen test
        Dim stproc1 As String = ""
        For i = 1 To mopens.Count
            For j = 1 To mopens(i).Count
                ''Madam and Sharfudeen 01/11/2022
                'If DecRound(mopens(i).Item(j).acc_base_debit, mdecimal) <> DecRound(mopens(i).Item(j).acc_debit * mopens(i).Item(j).cur_rate, mdecimal) Then
                '    objutils.MessageBox("There is a mis match in the Debit and Debit Base Amount for line no " & mopens(i).Item(j).against_tran_lineno, accpage)
                '    validateamount = False
                'End If

                'If DecRound(mopens(i).Item(j).acc_base_credit, mdecimal) <> DecRound(mopens(i).Item(j).acc_credit * mopens(i).Item(j).cur_rate, mdecimal) Then
                '    objutils.MessageBox("There is a mis match in the Credit and Credit Base Amount for line no " & mopens(i).Item(j).against_tran_lineno, accpage)
                '    validateamount = False
                'End If

                If Not (DecRound(mopens(i).Item(j).acc_base_debit, mdecimal) - DecRound(mopens(i).Item(j).acc_debit * mopens(i).Item(j).cur_rate, mdecimal) >= -0.05 And DecRound(mopens(i).Item(j).acc_base_debit, mdecimal) - DecRound(mopens(i).Item(j).acc_debit * mopens(i).Item(j).cur_rate, mdecimal) <= 0.05) Then
                    objutils.MessageBox("There is a mis match in the Debit and Debit Base Amount for line no " & mopens(i).Item(j).against_tran_lineno, accpage)
                    validateamount = False
                End If
                If Not (DecRound(mopens(i).Item(j).acc_base_credit, mdecimal) - DecRound(mopens(i).Item(j).acc_credit * mopens(i).Item(j).cur_rate, mdecimal) >= -0.05 And DecRound(mopens(i).Item(j).acc_base_credit, mdecimal) - DecRound(mopens(i).Item(j).acc_credit * mopens(i).Item(j).cur_rate, mdecimal) <= 0.05) Then
                    objutils.MessageBox("There is a mis match in the Credit and Credit Base Amount for line no " & mopens(i).Item(j).against_tran_lineno, accpage)
                    validateamount = False
                End If

                If mopens(i).Item(j).cur = mbasecurrency And mopens(i).Item(j).cur_rate <> 1 Then
                    objutils.MessageBox("Conversion Rate for " & mbasecurrency & "  Accounts should be 1", accpage)
                    validateamount = False
                End If
                val_debit = DecRound(val_debit, mdecimal) + DecRound(mopens(i).Item(j).acc_base_debit, mdecimal)
                val_credit = DecRound(val_credit, mdecimal) + DecRound(mopens(i).Item(j).acc_base_credit, mdecimal)
            Next

        Next

        ' If DecRound(val_debit, mdecimal) <> DecRound(val_credit, mdecimal) Then
        If Not DecRound(val_debit, mdecimal) - DecRound(val_credit, mdecimal) >= -0.05 And DecRound(val_debit, mdecimal) - DecRound(val_credit, mdecimal) <= 0.05 Then 'Tanvir 12062024

            validateamount = False
            objutils.MessageBox("There is a mis match in the Credit and Debit Amount", accpage)
        End If

        If val_debit = 0 Then
            validateamount = False
            objutils.MessageBox("Debit & Credit cannot be Zero", accpage)
        End If

    End Function


    Public Function DecRound(ByVal Ramt As Decimal, ByVal intmdecimal As Integer) As Decimal
        Dim Rdamt As Decimal
        Rdamt = Math.Round(Val(Ramt), CType(intmdecimal, Integer), MidpointRounding.AwayFromZero)
        Return Rdamt
    End Function

    'Function validateamount(ByVal constr As String, ByVal accpage As Page) As Boolean
    '    Dim val_credit As Double
    '    Dim val_debit As Double
    '    Dim i, j As Integer
    '    Dim objutils As New clsUtils
    '    validateamount = True
    '    Dim mbasecurrency, acccurrency As String
    '    mbasecurrency = objutils.ExecuteQueryReturnStringValuenew(constr, "select option_selected from reservation_parameters where param_id=457")
    '    val_credit = 0
    '    val_debit = 0

    '    For i = 1 To Accsubtrans.Count
    '        If Math.Abs(Math.Round(Accsubtrans(i).acc_base_debit, mdecimal, MidpointRounding.AwayFromZero) - Math.Round(Accsubtrans(i).acc_debit * Accsubtrans(i).currate, mdecimal, MidpointRounding.AwayFromZero)) > 1 Then
    '            objutils.MessageBox("There is a mis match in the Debit and Debit Base Amount for line no " & Accsubtrans(i).acc_tran_lineno, accpage)
    '            validateamount = False
    '        End If
    '        If Math.Abs(Math.Round(Accsubtrans(i).acc_base_credit, mdecimal, MidpointRounding.AwayFromZero) - Math.Round(Accsubtrans(i).acc_credit * Accsubtrans(i).currate, mdecimal, MidpointRounding.AwayFromZero)) > 1 Then
    '            objutils.MessageBox("There is a mis match in the Credit and Credit Base Amount for line no " & Accsubtrans(i).acc_tran_lineno, accpage)
    '            validateamount = False
    '        End If
    '        acccurrency = objutils.ExecuteQueryReturnStringValuenew(constr, "select cur from view_account where type='" & Accsubtrans(i).acc_type & "' and code='" & Accsubtrans(i).acc_code & "'")
    '        If acccurrency = mbasecurrency And Accsubtrans(i).currate <> 1 Then
    '            objutils.MessageBox("Conversion Rate for " & mbasecurrency & "  Accounts should be 1", accpage)
    '            validateamount = False
    '        End If
    '        val_debit = Math.Round(val_debit, mdecimal, MidpointRounding.AwayFromZero) + Math.Round(Accsubtrans(i).acc_base_debit, mdecimal, MidpointRounding.AwayFromZero)
    '        val_credit = Math.Round(val_credit, mdecimal, MidpointRounding.AwayFromZero) + Math.Round(Accsubtrans(i).acc_base_credit, mdecimal, MidpointRounding.AwayFromZero)
    '    Next
    '    For i = 1 To mopens.Count
    '        For j = 1 To mopens(i).Count
    '            If Math.Round(mopens(i).Item(j).acc_base_debit, mdecimal, MidpointRounding.AwayFromZero) <> Math.Round(mopens(i).Item(j).acc_debit * mopens(i).Item(j).cur_rate, mdecimal, MidpointRounding.AwayFromZero) Then
    '                objutils.MessageBox("There is a mis match in the Debit and Debit Base Amount for line no " & mopens(i).Item(j).against_tran_lineno, accpage)
    '                validateamount = False
    '            End If
    '            If Math.Round(mopens(i).Item(j).acc_base_credit, mdecimal, MidpointRounding.AwayFromZero) <> Math.Round(mopens(i).Item(j).acc_credit * mopens(i).Item(j).cur_rate, mdecimal, MidpointRounding.AwayFromZero) Then
    '                objutils.MessageBox("There is a mis match in the Credit and Credit Base Amount for line no " & mopens(i).Item(j).against_tran_lineno, accpage)
    '                validateamount = False
    '            End If
    '            If mopens(i).Item(j).cur = mbasecurrency And mopens(i).Item(j).cur_rate <> 1 Then
    '                objutils.MessageBox("Conversion Rate for " & mbasecurrency & "  Accounts should be 1", accpage)
    '                validateamount = False
    '            End If
    '            val_debit = Math.Round(val_debit, mdecimal, MidpointRounding.AwayFromZero) + Math.Round(mopens(i).Item(j).acc_base_debit, mdecimal, MidpointRounding.AwayFromZero)
    '            val_credit = Math.Round(val_credit, mdecimal, MidpointRounding.AwayFromZero) + Math.Round(mopens(i).Item(j).acc_base_credit, mdecimal, MidpointRounding.AwayFromZero)
    '        Next
    '    Next


    '    If Math.Round(val_debit, mdecimal, MidpointRounding.AwayFromZero) <> Math.Round(val_credit, mdecimal, MidpointRounding.AwayFromZero) Then
    '        validateamount = False
    '        objutils.MessageBox("There is a mis match in the Credit and Debit Amount", accpage)
    '    End If

    '    If val_debit = 0 Then
    '        validateamount = False
    '        objutils.MessageBox("Debit & Credit cannot be Zero", accpage)
    '    End If

    'End Function
    Function validateaccount(ByVal constr As String, ByRef scon As SqlConnection, ByRef stran As SqlTransaction, ByVal accpage As Page) As Boolean
        validateaccount = True
        mvalid_controlacct = True
        Dim i As Integer
        Dim objutils As New clsUtils
        Dim temp_rec As DataTable

        If Left(acc_tran_id, 5) = "CLOSE" Then
            objutils.MessageBox("Document Number cannot start with CLOSE", accpage)
            validateaccount = False
            Exit Function
        End If
        For i = 1 To Acctrans.Count
            temp_rec = getdatatable("select code from view_account where code='" & Trim(Acctrans(i).acc_code) & "' and type='" & Trim(Acctrans(i).acc_type) & "'", scon, stran)
            If temp_rec.Rows.Count = 0 Then
                objutils.MessageBox("Select the proper Account Code & Type", accpage)
                validateaccount = False
                Exit For
            End If
            temp_rec = Nothing
            If mvalid_controlacct = True Then 'nv 15/2/2004
                temp_rec = getdatatable("select controlyn from view_account where code='" & Trim(Acctrans(i).acc_code) & "' and type='" & Trim(Acctrans(i).acc_type) & "'", scon, stran)
                If Not temp_rec.Rows.Count = 0 Then
                    If temp_rec.Rows(0).Item(0) = "Y" Then
                        objutils.MessageBox("Do not Select Control Accounts", accpage)
                        validateaccount = False
                        Exit For
                    End If
                End If
                temp_rec = Nothing
                If Trim(Acctrans(i).acc_type) <> "G" Then
                    temp_rec = getdatatable("select acctcode from acctmast where acctcode='" & Trim(Acctrans(i).pacc_gl_code) & "'", scon, stran)
                    If temp_rec.Rows.Count = 0 Then
                        objutils.MessageBox("Select the Control Account Code", accpage)
                        validateaccount = False
                        Exit For
                    End If
                End If
            End If
        Next
    End Function
    Sub mopenremove(ByVal lineno As String)
        On Error Resume Next
        mopens.Remove(CStr(lineno))
    End Sub
    Sub clropencol()
        mopens = Nothing
    End Sub
    Function colexists(ByVal col As Collection, ByVal lineno As Integer) As Boolean
        On Error GoTo diff_err
        colexists = True
        If col.Item(CStr(lineno)).Count <> 0 Then
            colexists = True
        Else
            colexists = False
        End If
        Exit Function
diff_err:
        colexists = False
    End Function
    Function groupexists(ByVal i As Integer, ByVal j As Integer) As Boolean
        On Error GoTo diff_err
        groupexists = True
        If cgroups(i).Item("Group" & j).Count > 0 Then
            groupexists = True
        Else
            groupexists = False
        End If
        Exit Function
diff_err:
        groupexists = False
    End Function
    Sub createopenscollection(ByVal constr As String, ByVal divid As String, ByVal tranid As String, ByVal trantype As String, ByRef scon As SqlConnection, ByRef stran As SqlTransaction)
        mopens.Clear()
        Dim subopen As New Collection
        Dim temp_rec As DataTable, i As Integer
        temp_rec = getdatatable("select * from open_detail where div_id='" & divid & "' and against_tran_id='" & tranid _
        & "' and against_tran_type='" & trantype & "'", scon, stran)
        If Not temp_rec.Rows.Count = 0 Then
            For i = 0 To temp_rec.Rows.Count - 1
                Dim copen As New clsOpen
                copen.div_id = temp_rec.Rows(i).Item("div_id")
                copen.tran_id = temp_rec.Rows(i).Item("tran_id")
                copen.tran_lineno = temp_rec.Rows(i).Item("tran_lineno")
                copen.tran_type = temp_rec.Rows(i).Item("tran_type")
                copen.against_tran_lineno = temp_rec.Rows(i).Item("against_tran_lineno")
                copen.acc_due_date = temp_rec.Rows(i).Item("open_due_date")
                copen.tran_sales_code = IIf(IsDBNull(temp_rec.Rows(i).Item("open_sales_code")) = False, temp_rec.Rows(i).Item("open_sales_code"), "")
                copen.acc_debit = temp_rec.Rows(i).Item("open_debit")
                copen.acc_credit = temp_rec.Rows(i).Item("open_credit")
                copen.acc_base_debit = temp_rec.Rows(i).Item("base_debit")
                copen.acc_base_credit = temp_rec.Rows(i).Item("base_credit")
                copen.acc_base_amount = temp_rec.Rows(i).Item("base_debit") + temp_rec.Rows(i).Item("base_credit")
                copen.acc_field1 = temp_rec.Rows(i).Item("open_field1")
                copen.acc_field2 = temp_rec.Rows(i).Item("open_field2")
                copen.acc_field3 = temp_rec.Rows(i).Item("open_field3")
                copen.acc_field4 = temp_rec.Rows(i).Item("open_field4")
                copen.acc_field5 = temp_rec.Rows(i).Item("open_field5")
                copen.acc_type = temp_rec.Rows(i).Item("acc_type")
                copen.cur_Rate = temp_rec.Rows(i).Item("currency_rate")
                copen.tran_date = temp_rec.Rows(i).Item("tran_date")
                copen.open_mode = temp_rec.Rows(i).Item("open_mode")
                copen.open_Exchg_diff = 0
                copen.adjusted_amount = temp_rec.Rows(i).Item("open_debit") + temp_rec.Rows(i).Item("open_credit")
                copen.acccode = temp_rec.Rows(i).Item("acc_code")
                copen.acc_gl_code = temp_rec.Rows(i).Item("acc_gl_code")
                subopen.Add(copen)
            Next
            mopens.Add(subopen)
        End If


    End Sub

    Protected Overrides Sub Finalize()
        mgroups = Nothing
        mopens = Nothing
        Acc_Mode = 0
        cgroups = Nothing
        mtran_mode = 0
        macc_tran_id = ""
        macc_tran_lineno = 0
        macc_tran_type = ""
        MyBase.Finalize()
    End Sub
End Class
