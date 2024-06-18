Imports System.Web
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.Data
Imports System.Data.SqlClient
Imports System.Xml
Imports System.Text
Imports System.IO
Imports System.Configuration
Imports System.Runtime.Serialization.Formatters.Binary
Imports System.Collections
Imports System.Collections.Generic



' To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
' <System.Web.Script.Services.ScriptService()> _
<WebService(Namespace:="http://tempuri.org/")> _
<WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Public Class WebService
    Inherits System.Web.Services.WebService
    Dim objUtils As New clsUtils


    <WebMethod()> _
    Public Function UpdateMarkets(ByVal constr As String, ByVal plgrpcode As String, ByVal plgrpname As String, ByVal active As Integer, ByVal showinweb As Integer, ByVal userlogged As String, ByVal frmmode As String) As String
        'Dim retlist As New List(Of clsMaster)




        If Trim(ConfigurationManager.AppSettings("CustomerStopService")) <> "" Then
            UpdateMarkets = Trim(ConfigurationManager.AppSettings("CustomerStopService"))
            Exit Function
        End If

        Dim ClientIP As String = Context.Request.ServerVariables("REMOTE_ADDR")


        If objUtils.ExecuteQueryReturnStringValuenew(constr, "select 1 from service_userpermissions  where userip='" & ClientIP & "' and active=1") <> "1" Then
            UpdateMarkets = "Permission Denied"
            Exit Function
        End If



        Dim result_temp As String

        result_temp = objUtils.ExecuteQueryReturnSingleValuenew(constr, "delete from tempservicesave_plgrpmast")

        Dim result1 As Integer
        Dim sqlstr As String

        Dim spname As String

        Dim p As Integer

        Dim parms As New List(Of SqlParameter)
        Dim parm(5) As SqlParameter
        parm(5) = New SqlParameter
        parm(0) = New SqlParameter("@plgrpcode", CType(plgrpcode, String))
        parm(1) = New SqlParameter("@plgrpname", CType(plgrpname, String))
        parm(2) = New SqlParameter("@active", CType(active, Integer))
        parm(3) = New SqlParameter("@showinweb", CType(showinweb, Integer))
        parm(4) = New SqlParameter("@userlogged", CType(userlogged, String))



        If frmmode = 1 Then
            For p = 0 To 4
                parms.Add(parm(p))
            Next
            spname = "sp_add_plgrp"
        End If


        If frmmode = 2 Then

            For p = 0 To 4
                parms.Add(parm(p))
            Next
            spname = "sp_mod_plgrp"
            result_temp = objUtils.ExecuteQueryReturnSingleValuenew(constr, "insert into  tempservicesave_plgrpmast select * from plgrpmast where plgrpcode='" & CType(plgrpcode, String) & "'")


        End If


        If frmmode = 3 Then
            parms.Add(parm(0))
            spname = "sp_del_plgrp"
            result_temp = objUtils.ExecuteQueryReturnSingleValuenew(constr, "insert into  tempservicesave_plgrpmast select * from plgrpmast where plgrpcode='" & CType(plgrpcode, String) & "'")

        End If

        result1 = objUtils.ExecuteNonQuerynew(constr, spname, parms)

        If result1 = Nothing Or IsDBNull(result1) Then
            result1 = ""

        End If



        Return result1
    End Function


    <WebMethod()> _
    Public Function Reverse_UpdateMarkets(ByVal constr As String, ByVal plgrpcode As String, ByVal frmmode As String) As String
        'Dim retlist As New List(Of clsMaster)




        If Trim(ConfigurationManager.AppSettings("CustomerStopService")) <> "" Then
            Reverse_UpdateMarkets = Trim(ConfigurationManager.AppSettings("CustomerStopService"))
            Exit Function
        End If


        Dim ClientIP As String = Context.Request.ServerVariables("REMOTE_ADDR")


        If objUtils.ExecuteQueryReturnStringValuenew(constr, "select 1 from service_userpermissions  where userip='" & ClientIP & "' and active=1") <> "1" Then
            Reverse_UpdateMarkets = "Permission Denied"
            Exit Function
        End If





        Dim result1 As Integer
        'Dim sqlstr As String

        Dim spname As String

        'Dim p As Integer

        Dim parms As New List(Of SqlParameter)
        Dim parm(5) As SqlParameter
        parm(5) = New SqlParameter
        parm(0) = New SqlParameter("@plgrpcode", CType(plgrpcode, String))
        If frmmode > 1 Then
            Dim plgrpname, userlogged, active, showinweb As String


            Dim ds As New DataSet
            ds = objUtils.GetDataFromDatasetnew(constr, "select * from tempservicesave_plgrpmast where plgrpcode='" & CType(plgrpcode, String) & "'")

            If ds.Tables(0).Rows.Count > 0 Then
                plgrpname = ds.Tables(0).Rows(0).Item("plgrpname")
                userlogged = ds.Tables(0).Rows(0).Item("moduser")
                active = ds.Tables(0).Rows(0).Item("active")
                showinweb = ds.Tables(0).Rows(0).Item("showinweb")
            End If

            parm(1) = New SqlParameter("@plgrpname", CType(plgrpname, String))
            parm(2) = New SqlParameter("@active", CType(active, Integer))
            parm(3) = New SqlParameter("@showinweb", CType(showinweb, Integer))
            parm(4) = New SqlParameter("@userlogged", CType(userlogged, String))

        End If

        If frmmode = 1 Then
            parms.Add(parm(0))
            spname = "sp_del_plgrp"
        End If


        If frmmode = 2 Then
            For p = 0 To 4
                parms.Add(parm(p))
            Next

            spname = "sp_mod_plgrp"

        End If


        If frmmode = 3 Then

            For p = 0 To 4
                parms.Add(parm(p))
            Next
            spname = "sp_add_plgrp"
        End If

        result1 = objUtils.ExecuteNonQuerynew(constr, spname, parms)

        If result1 = Nothing Or IsDBNull(result1) Then
            result1 = ""

        End If



        Return result1
    End Function
    <WebMethod()> _
    Public Function UpdateCurrency(ByVal constr As String, ByVal currcode As String, ByVal currname As String, ByVal active As Integer, ByVal convrate As Double, ByVal currcoin As String, ByVal userlogged As String, ByVal frmmode As String, ByVal webcurrcode As String) As String
        'Dim retlist As New List(Of clsMaster)

        If Trim(ConfigurationManager.AppSettings("CustomerStopService")) <> "" Then
            UpdateCurrency = Trim(ConfigurationManager.AppSettings("CustomerStopService"))
            Exit Function
        End If

        Dim ClientIP As String = Context.Request.ServerVariables("REMOTE_ADDR")


        If objUtils.ExecuteQueryReturnStringValuenew(constr, "select 1 from service_userpermissions  where userip='" & ClientIP & "' and active=1") <> "1" Then
            UpdateCurrency = "Permission Denied"
            Exit Function
        End If



        Dim result_temp As String

        result_temp = objUtils.ExecuteQueryReturnSingleValuenew(constr, "delete from tempservicesave_currmast")

        Dim result1 As Integer
        Dim sqlstr As String

        Dim spname As String

        Dim p As Integer





        Dim parms As New List(Of SqlParameter)
        Dim parm(7) As SqlParameter
        parm(7) = New SqlParameter
        parm(0) = New SqlParameter("@currcode", CType(currcode, String))
        parm(1) = New SqlParameter("@currname", CType(currname, String))
        parm(2) = New SqlParameter("@currcoin", CType(currcoin, String))
        parm(3) = New SqlParameter("@active", CType(active, Integer))
        parm(4) = New SqlParameter("@userlogged", CType(userlogged, String))
        parm(5) = New SqlParameter("@convrate", CType(convrate, Decimal))
        parm(6) = New SqlParameter("@webcurrcode", CType(webcurrcode, String))


        If frmmode = 1 Then
            For p = 0 To 5
                parms.Add(parm(p))
            Next
            spname = "sp_add_curr"
        End If


        If frmmode = 2 Then

            For p = 0 To 5
                parms.Add(parm(p))
            Next
            spname = "sp_mod_curr"
            result_temp = objUtils.ExecuteQueryReturnSingleValuenew(constr, "insert into  tempservicesave_currmast select * from currmast where currcode='" & CType(currcode, String) & "'")


        End If


        If frmmode = 3 Then
            parms.Add(parm(0))
            spname = "sp_del_curr"
            result_temp = objUtils.ExecuteQueryReturnSingleValuenew(constr, "insert into  tempservicesave_currmast select * from currmast where currcode='" & CType(currcode, String) & "'")

        End If

        result1 = objUtils.ExecuteNonQuerynew(constr, spname, parms)

        If result1 = Nothing Or IsDBNull(result1) Then
            result1 = ""

        End If



        Return result1
    End Function
    <WebMethod()> _
    Public Function Reverse_UpdateCurrency(ByVal constr As String, ByVal currcode As String, ByVal frmmode As String) As String
        'Dim retlist As New List(Of clsMaster)




        If Trim(ConfigurationManager.AppSettings("CustomerStopService")) <> "" Then
            Reverse_UpdateCurrency = Trim(ConfigurationManager.AppSettings("CustomerStopService"))
            Exit Function
        End If


        Dim ClientIP As String = Context.Request.ServerVariables("REMOTE_ADDR")


        If objUtils.ExecuteQueryReturnStringValuenew(constr, "select 1 from service_userpermissions  where userip='" & ClientIP & "' and active=1") <> "1" Then
            Reverse_UpdateCurrency = "Permission Denied"
            Exit Function
        End If





        Dim result1 As Integer
        'Dim sqlstr As String

        Dim spname As String

        'Dim p As Integer



        Dim parms As New List(Of SqlParameter)
        Dim parm(6) As SqlParameter
        parm(6) = New SqlParameter
        parm(0) = New SqlParameter("@currcode", CType(currcode, String))
        If frmmode > 1 Then
            Dim currname, userlogged, active, currcoin As String

            Dim convrate As Double


            Dim ds As New DataSet
            ds = objUtils.GetDataFromDatasetnew(constr, "select * from tempservicesave_currmast where currcode='" & CType(currcode, String) & "'")

            If ds.Tables(0).Rows.Count > 0 Then
                currname = ds.Tables(0).Rows(0).Item("currname")
                userlogged = ds.Tables(0).Rows(0).Item("moduser")
                active = ds.Tables(0).Rows(0).Item("active")
                currcoin = ds.Tables(0).Rows(0).Item("currcoin")
                convrate = ds.Tables(0).Rows(0).Item("convrate")
            End If

            parm(1) = New SqlParameter("@currname", CType(currname, String))
            parm(2) = New SqlParameter("@active", CType(active, Integer))
            parm(3) = New SqlParameter("@currcoin", CType(currcoin, Integer))
            parm(4) = New SqlParameter("@userlogged", CType(userlogged, String))
            parm(5) = New SqlParameter("@convrate", CType(convrate, Decimal))



        End If

        If frmmode = 1 Then
            parms.Add(parm(0))
            spname = "sp_del_curr"
        End If


        If frmmode = 2 Then
            For p = 0 To 5
                parms.Add(parm(p))
            Next

            spname = "sp_mod_curr"

        End If


        If frmmode = 3 Then

            For p = 0 To 5
                parms.Add(parm(p))
            Next
            spname = "sp_add_curr"
        End If

        result1 = objUtils.ExecuteNonQuerynew(constr, spname, parms)

        If result1 = Nothing Or IsDBNull(result1) Then
            result1 = ""

        End If



        Return result1
    End Function
    <WebMethod()> _
    Public Function UpdateNationality(ByVal constr As String, ByVal nationalitycode As String, ByVal nationalityname As String, ByVal active As Integer, ByVal userlogged As String, ByVal frmmode As String) As String
        'Dim retlist As New List(Of clsMaster)



        If Trim(ConfigurationManager.AppSettings("CustomerStopService")) <> "" Then
            UpdateNationality = Trim(ConfigurationManager.AppSettings("CustomerStopService"))
            Exit Function
        End If

        Dim ClientIP As String = Context.Request.ServerVariables("REMOTE_ADDR")


        If objUtils.ExecuteQueryReturnStringValuenew(constr, "select 1 from service_userpermissions  where userip='" & ClientIP & "' and active=1") <> "1" Then
            UpdateNationality = "Permission Denied"
            Exit Function
        End If



        Dim result_temp As String

        result_temp = objUtils.ExecuteQueryReturnSingleValuenew(constr, "delete from tempservicesave_nationality_master")

        Dim result1 As Integer
        Dim sqlstr As String

        Dim spname As String

        Dim p As Integer





        Dim parms As New List(Of SqlParameter)
        Dim parm(4) As SqlParameter
        parm(4) = New SqlParameter
        parm(0) = New SqlParameter("@nationalitycode", CType(nationalitycode, String))
        parm(1) = New SqlParameter("@nationalityname", CType(nationalityname, String))

        parm(2) = New SqlParameter("@active", CType(active, Integer))
        parm(3) = New SqlParameter("@userlogged", CType(userlogged, String))



        If frmmode = 1 Then
            For p = 0 To 3
                parms.Add(parm(p))
            Next
            spname = "sp_add_nationality"
        End If


        If frmmode = 2 Then

            For p = 0 To 3
                parms.Add(parm(p))
            Next
            spname = "sp_mod_Nationality"
            result_temp = objUtils.ExecuteQueryReturnSingleValuenew(constr, "insert into  tempservicesave_nationality_master select * from nationality_master where nationalitycode='" & CType(nationalitycode, String) & "'")


        End If


        If frmmode = 3 Then
            parms.Add(parm(0))
            parms.Add(parm(1))
            spname = "sp_del_Nationality"
            result_temp = objUtils.ExecuteQueryReturnSingleValuenew(constr, "insert into  tempservicesave_nationality_master select * from nationality_master where nationalitycode='" & CType(nationalitycode, String) & "'")

        End If

        result1 = objUtils.ExecuteNonQuerynew(constr, spname, parms)

        If result1 = Nothing Or IsDBNull(result1) Then
            result1 = ""

        End If



        Return result1
    End Function
    <WebMethod()> _
    Public Function Reverse_UpdateNationality(ByVal constr As String, ByVal nationalitycode As String, ByVal frmmode As String) As String
        'Dim retlist As New List(Of clsMaster)




        If Trim(ConfigurationManager.AppSettings("CustomerStopService")) <> "" Then
            Reverse_UpdateNationality = Trim(ConfigurationManager.AppSettings("CustomerStopService"))
            Exit Function
        End If


        Dim ClientIP As String = Context.Request.ServerVariables("REMOTE_ADDR")


        If objUtils.ExecuteQueryReturnStringValuenew(constr, "select 1 from service_userpermissions  where userip='" & ClientIP & "' and active=1") <> "1" Then
            Reverse_UpdateNationality = "Permission Denied"
            Exit Function
        End If





        Dim result1 As Integer
        'Dim sqlstr As String

        Dim spname As String

        'Dim p As Integer

        Dim parms As New List(Of SqlParameter)
        Dim parm(4) As SqlParameter
        parm(4) = New SqlParameter
        parm(0) = New SqlParameter("@nationalitycode", CType(nationalitycode, String))
        If frmmode > 1 Then
            Dim nationalityname, userlogged, active As String


            Dim ds As New DataSet
            ds = objUtils.GetDataFromDatasetnew(constr, "select * from tempservicesave_nationality_master where nationalitycode='" & CType(nationalitycode, String) & "'")

            If ds.Tables(0).Rows.Count > 0 Then
                nationalityname = ds.Tables(0).Rows(0).Item("nationalityname")
                userlogged = ds.Tables(0).Rows(0).Item("moduser")
                active = ds.Tables(0).Rows(0).Item("active")

            End If

            parm(1) = New SqlParameter("@nationalityname", CType(nationalityname, String))
            parm(2) = New SqlParameter("@active", CType(active, Integer))

            parm(3) = New SqlParameter("@userlogged", CType(userlogged, String))

        End If

        If frmmode = 1 Then
            parms.Add(parm(0))
            parms.Add(parm(1))

            spname = "sp_del_Nationality"
        End If


        If frmmode = 2 Then
            For p = 0 To 3
                parms.Add(parm(p))
            Next

            spname = "sp_mod_Nationality"

        End If


        If frmmode = 3 Then

            For p = 0 To 3
                parms.Add(parm(p))
            Next
            spname = "sp_add_nationality"
        End If

        result1 = objUtils.ExecuteNonQuerynew(constr, spname, parms)

        If result1 = Nothing Or IsDBNull(result1) Then
            result1 = ""

        End If



        Return result1
    End Function
    <WebMethod()> _
    Public Function Updatectrymast(ByVal constr As String, ByVal ctrycode As String, ByVal ctryname As String, ByVal currcode As String, ByVal wkfrmday1 As String, ByVal wktoday1 As String, ByVal wkfrmday2 As String, ByVal wktoday2 As String, ByVal plgrpcode As String, ByVal nationalitycode As String, ByVal nationalityname As String, ByVal inclpromotion As Integer, ByVal inclearlypromotion As Integer, ByVal active As Integer, ByVal userlogged As String, ByVal frmmode As String) As String




        If Trim(ConfigurationManager.AppSettings("CustomerStopService")) <> "" Then
            Updatectrymast = Trim(ConfigurationManager.AppSettings("CustomerStopService"))
            Exit Function
        End If

        Dim ClientIP As String = Context.Request.ServerVariables("REMOTE_ADDR")


        If objUtils.ExecuteQueryReturnStringValuenew(constr, "select 1 from service_userpermissions  where userip='" & ClientIP & "' and active=1") <> "1" Then
            Updatectrymast = "Permission Denied"
            Exit Function
        End If



        Dim result_temp As String

        result_temp = objUtils.ExecuteQueryReturnSingleValuenew(constr, "delete from tempservicesave_ctrymast")

        Dim result1 As Integer
        Dim sqlstr As String

        Dim spname As String

        Dim p As Integer

        Dim parms As New List(Of SqlParameter)
        Dim parm(14) As SqlParameter
        parm(14) = New SqlParameter
        parm(0) = New SqlParameter("@ctrycode", CType(ctrycode, String))
        parm(1) = New SqlParameter("@ctryname", CType(ctryname, String))
        parm(2) = New SqlParameter("@currcode", CType(currcode, String))
        parm(3) = New SqlParameter("@wkfrmday1", CType(wkfrmday1, String))
        parm(4) = New SqlParameter("@wktoday1", CType(wktoday1, String))
        parm(5) = New SqlParameter("@wkfrmday2", CType(wkfrmday2, String))
        parm(6) = New SqlParameter("@wktoday2", CType(wktoday2, String))

        parm(7) = New SqlParameter("@plgrpcode", CType(plgrpcode, String))
        parm(8) = New SqlParameter("@nationalitycode", CType(nationalitycode, String))
        parm(9) = New SqlParameter("@nationalityname", CType(nationalityname, String))
        parm(10) = New SqlParameter("@inclpromotion", CType(inclpromotion, Integer))
        parm(11) = New SqlParameter("@inclearlypromotion", CType(inclearlypromotion, Integer))

        parm(12) = New SqlParameter("@active", CType(active, Integer))
        parm(13) = New SqlParameter("@userlogged", CType(userlogged, String))



        If frmmode = 1 Then
            For p = 0 To 13
                parms.Add(parm(p))
            Next
            spname = "sp_add_ctry"





        End If


        If frmmode = 2 Then

            For p = 0 To 13
                parms.Add(parm(p))
            Next
            spname = "sp_mod_ctry"
            result_temp = objUtils.ExecuteQueryReturnSingleValuenew(constr, "insert into  tempservicesave_ctrymast select * from ctrymast where ctrycode='" & CType(ctrycode, String) & "'")


        End If


        If frmmode = 3 Then
            parms.Add(parm(0))
            spname = "sp_del_ctry"
            result_temp = objUtils.ExecuteQueryReturnSingleValuenew(constr, "insert into  tempservicesave_ctrymast select * from ctrymast where ctrycode='" & CType(ctrycode, String) & "'")

        End If

        result1 = objUtils.ExecuteNonQuerynew(constr, spname, parms)
        Dim parm1 As New List(Of SqlParameter)

        Dim parmeter1(2) As SqlParameter
        If frmmode = 1 Then
            parmeter1(0) = New SqlParameter("@ctrycode", CType(ctrycode, String))
            parmeter1(1) = New SqlParameter("@marketcode", CType(plgrpcode, String))

            parm1.Add(parmeter1(0))

            parm1.Add(parmeter1(1))

            spname = "add_country_promotion"


            result1 = objUtils.ExecuteNonQuerynew(constr, spname, parm1)

        End If

        If result1 = Nothing Or IsDBNull(result1) Then
            result1 = ""

        End If



        Return result1
    End Function
    <WebMethod()> _
    Public Function Reverse_Updatectrymast(ByVal constr As String, ByVal ctrycode As String, ByVal frmmode As String) As String
        'Dim retlist As New List(Of clsMaster)




        If Trim(ConfigurationManager.AppSettings("CustomerStopService")) <> "" Then
            Reverse_Updatectrymast = Trim(ConfigurationManager.AppSettings("CustomerStopService"))
            Exit Function
        End If


        Dim ClientIP As String = Context.Request.ServerVariables("REMOTE_ADDR")


        If objUtils.ExecuteQueryReturnStringValuenew(constr, "select 1 from service_userpermissions  where userip='" & ClientIP & "' and active=1") <> "1" Then
            Reverse_Updatectrymast = "Permission Denied"
            Exit Function
        End If





        Dim result1 As Integer
        'Dim sqlstr As String

        Dim spname As String

        'Dim p As Integer



        Dim parms As New List(Of SqlParameter)
        Dim parm(14) As SqlParameter
        parm(14) = New SqlParameter
        parm(0) = New SqlParameter("@ctrycode", CType(ctrycode, String))
        If frmmode > 1 Then
            Dim ctryname, currcode, wkfrmday1, wktoday1, wkfrmday2, wktoday2, plgrpcode, nationalitycode, nationalityname, inclpromotion, inclearlypromotion, userlogged, active As String


            Dim ds As New DataSet
            ds = objUtils.GetDataFromDatasetnew(constr, "select * from tempservicesave_ctrymast where ctrycode='" & CType(ctrycode, String) & "'")

            If ds.Tables(0).Rows.Count > 0 Then
                ctryname = ds.Tables(0).Rows(0).Item("ctryname")
                currcode = ds.Tables(0).Rows(0).Item("currcode")
                wkfrmday1 = ds.Tables(0).Rows(0).Item("wkfrmday1")
                wktoday1 = ds.Tables(0).Rows(0).Item("wktoday1")
                wkfrmday2 = ds.Tables(0).Rows(0).Item("wkfrmday2")
                wktoday2 = ds.Tables(0).Rows(0).Item("wktoday2")
                plgrpcode = ds.Tables(0).Rows(0).Item("plgrpcode")
                nationalitycode = ds.Tables(0).Rows(0).Item("nationalitycode")
                nationalityname = ds.Tables(0).Rows(0).Item("nationality")
                inclpromotion = ds.Tables(0).Rows(0).Item("inclpromotion")
                inclearlypromotion = ds.Tables(0).Rows(0).Item("inclearlypromotion")
                userlogged = ds.Tables(0).Rows(0).Item("moduser")
                active = ds.Tables(0).Rows(0).Item("active")

            End If
            parm(1) = New SqlParameter("@ctryname", CType(ctryname, String))
            parm(2) = New SqlParameter("@currcode", CType(currcode, String))
            parm(3) = New SqlParameter("@wkfrmday1", CType(wkfrmday1, String))
            parm(4) = New SqlParameter("@wktoday1", CType(wktoday1, String))
            parm(5) = New SqlParameter("@wkfrmday2", CType(wkfrmday2, String))
            parm(6) = New SqlParameter("@wktoday2", CType(wktoday2, String))

            parm(7) = New SqlParameter("@plgrpcode", CType(plgrpcode, String))
            parm(8) = New SqlParameter("@nationalitycode", CType(nationalitycode, String))
            parm(9) = New SqlParameter("@nationalityname", CType(nationalityname, String))
            parm(10) = New SqlParameter("@inclpromotion", CType(inclpromotion, Integer))
            parm(11) = New SqlParameter("@inclearlypromotion", CType(inclearlypromotion, Integer))

            parm(12) = New SqlParameter("@active", CType(active, Integer))
            parm(13) = New SqlParameter("@userlogged", CType(userlogged, String))

        End If

        If frmmode = 1 Then
            parms.Add(parm(0))
            spname = "sp_del_ctry"
            Dim result_temp As String
            result_temp = objUtils.ExecuteQueryReturnSingleValuenew(constr, "delete from Promo_countries  where ctrycode='" & CType(ctrycode, String) & "'")

        End If


        If frmmode = 2 Then
            For p = 0 To 13
                parms.Add(parm(p))
            Next

            spname = "sp_mod_ctry"

        End If


        If frmmode = 3 Then

            For p = 0 To 13
                parms.Add(parm(p))
            Next
            spname = "sp_add_ctry"
        End If




        result1 = objUtils.ExecuteNonQuerynew(constr, spname, parms)

        If result1 = Nothing Or IsDBNull(result1) Then
            result1 = ""

        End If



        Return result1
    End Function


    <WebMethod()> _
    Public Function Updatecity(ByVal constr As String, ByVal citycode As String, ByVal cityname As String, ByVal ctrycode As String, ByVal rankord As Integer, ByVal showinweb As Integer, ByVal active As Integer, ByVal userlogged As String, ByVal frmmode As String) As String
        'Dim retlist As New List(Of clsMaster)




        If Trim(ConfigurationManager.AppSettings("CustomerStopService")) <> "" Then
            Updatecity = Trim(ConfigurationManager.AppSettings("CustomerStopService"))
            Exit Function
        End If

        Dim ClientIP As String = Context.Request.ServerVariables("REMOTE_ADDR")


        If objUtils.ExecuteQueryReturnStringValuenew(constr, "select 1 from service_userpermissions  where userip='" & ClientIP & "' and active=1") <> "1" Then
            Updatecity = "Permission Denied"
            Exit Function
        End If



        Dim result_temp As String

        result_temp = objUtils.ExecuteQueryReturnSingleValuenew(constr, "delete from tempservicesave_citymast")

        Dim result1 As Integer
        Dim sqlstr As String

        Dim spname As String

        Dim p As Integer




        Dim parms As New List(Of SqlParameter)
        Dim parm(7) As SqlParameter
        parm(7) = New SqlParameter
        parm(0) = New SqlParameter("@citycode", CType(citycode, String))
        parm(1) = New SqlParameter("@cityname", CType(cityname, String))
        parm(2) = New SqlParameter("@ctrycode", CType(ctrycode, String))
        parm(3) = New SqlParameter("@rnkord", CType(rankord, Integer))
        parm(4) = New SqlParameter("@active", CType(active, Integer))
        parm(5) = New SqlParameter("@showweb", CType(showinweb, Integer))
        parm(6) = New SqlParameter("@userlogged", CType(userlogged, String))



        If frmmode = 1 Then
            For p = 0 To 6
                parms.Add(parm(p))
            Next
            spname = "sp_add_city"
        End If


        If frmmode = 2 Then

            For p = 0 To 6
                parms.Add(parm(p))
            Next
            spname = "sp_mod_city"
            result_temp = objUtils.ExecuteQueryReturnSingleValuenew(constr, "insert into  tempservicesave_citymast select * from citymast where citycode='" & CType(citycode, String) & "'")


        End If


        If frmmode = 3 Then
            parms.Add(parm(0))
            spname = "sp_del_city"
            result_temp = objUtils.ExecuteQueryReturnSingleValuenew(constr, "insert into  tempservicesave_citymast select * from citymast where citycode='" & CType(citycode, String) & "'")

        End If

        result1 = objUtils.ExecuteNonQuerynew(constr, spname, parms)

        If result1 = Nothing Or IsDBNull(result1) Then
            result1 = ""

        End If



        Return result1
    End Function



    <WebMethod()> _
    Public Function Reverse_UpdateCity(ByVal constr As String, ByVal citycode As String, ByVal frmmode As String) As String
        'Dim retlist As New List(Of clsMaster)




        If Trim(ConfigurationManager.AppSettings("CustomerStopService")) <> "" Then
            Reverse_UpdateCity = Trim(ConfigurationManager.AppSettings("CustomerStopService"))
            Exit Function
        End If


        Dim ClientIP As String = Context.Request.ServerVariables("REMOTE_ADDR")


        If objUtils.ExecuteQueryReturnStringValuenew(constr, "select 1 from service_userpermissions  where userip='" & ClientIP & "' and active=1") <> "1" Then
            Reverse_UpdateCity = "Permission Denied"
            Exit Function
        End If





        Dim result1 As Integer
        'Dim sqlstr As String

        Dim spname As String

        'Dim p As Integer



        Dim parms As New List(Of SqlParameter)
        Dim parm(7) As SqlParameter
        parm(7) = New SqlParameter
        parm(0) = New SqlParameter("@citycode", CType(citycode, String))
        If frmmode > 1 Then
            Dim cityname, ctrycode, userlogged As String

            Dim rnkord, showweb, active As String

            Dim ds As New DataSet
            ds = objUtils.GetDataFromDatasetnew(constr, "select * from tempservicesave_citymast where citycode='" & CType(citycode, String) & "'")

            If ds.Tables(0).Rows.Count > 0 Then

                cityname = ds.Tables(0).Rows(0).Item("cityname")
                ctrycode = ds.Tables(0).Rows(0).Item("ctrycode")
                rnkord = ds.Tables(0).Rows(0).Item("rankorder")
                showweb = ds.Tables(0).Rows(0).Item("showweb")
                userlogged = ds.Tables(0).Rows(0).Item("moduser")
                active = ds.Tables(0).Rows(0).Item("active")

            End If
            parm(1) = New SqlParameter("@cityname", CType(cityname, String))
            parm(2) = New SqlParameter("@ctrycode", CType(ctrycode, String))
            parm(3) = New SqlParameter("@rnkord", CType(rnkord, String))
            parm(4) = New SqlParameter("@showweb", CType(showweb, String))
            parm(5) = New SqlParameter("@active", CType(active, Integer))
            parm(6) = New SqlParameter("@userlogged", CType(userlogged, String))

        End If

        If frmmode = 1 Then
            parms.Add(parm(0))
            spname = "sp_del_city"

        End If


        If frmmode = 2 Then
            For p = 0 To 6
                parms.Add(parm(p))
            Next

            spname = "sp_mod_city"

        End If


        If frmmode = 3 Then

            For p = 0 To 6
                parms.Add(parm(p))
            Next
            spname = "sp_add_city"
        End If




        result1 = objUtils.ExecuteNonQuerynew(constr, spname, parms)

        If result1 = Nothing Or IsDBNull(result1) Then
            result1 = ""

        End If



        Return result1
    End Function
    <WebMethod()> _
    Public Function UpdateSectors(ByVal constr As String, ByVal sectorcode As String, ByVal sectorname As String, ByVal ctrycode As String, ByVal marketcode As String, ByVal active As Integer, ByVal userlogged As String, ByVal frmmode As String) As String
        'Dim retlist As New List(Of clsMaster)




        If Trim(ConfigurationManager.AppSettings("CustomerStopService")) <> "" Then
            UpdateSectors = Trim(ConfigurationManager.AppSettings("CustomerStopService"))
            Exit Function
        End If

        Dim ClientIP As String = Context.Request.ServerVariables("REMOTE_ADDR")


        If objUtils.ExecuteQueryReturnStringValuenew(constr, "select 1 from service_userpermissions  where userip='" & ClientIP & "' and active=1") <> "1" Then
            UpdateSectors = "Permission Denied"
            Exit Function
        End If



        Dim result_temp As String

        result_temp = objUtils.ExecuteQueryReturnSingleValuenew(constr, "delete from tempservicesave_sectormaster")

        Dim result1 As Integer
        Dim sqlstr As String

        Dim spname As String

        Dim p As Integer




        Dim parms As New List(Of SqlParameter)
        Dim parm(6) As SqlParameter
        parm(6) = New SqlParameter
        parm(0) = New SqlParameter("@sectorcode", CType(sectorcode, String))
        parm(1) = New SqlParameter("@sectorname", CType(sectorname, String))
        parm(2) = New SqlParameter("@ctrycode", CType(ctrycode, String))
        parm(3) = New SqlParameter("@plgrpcode", CType(marketcode, String))
        parm(4) = New SqlParameter("@active", CType(active, Integer))
        parm(5) = New SqlParameter("@userlogged", CType(userlogged, String))



        If frmmode = 1 Then
            For p = 0 To 5
                parms.Add(parm(p))
            Next
            spname = "sp_add_agent_secMast"
        End If


        If frmmode = 2 Then

            For p = 0 To 5
                parms.Add(parm(p))
            Next
            spname = "sp_mod_agent_secmast"
            result_temp = objUtils.ExecuteQueryReturnSingleValuenew(constr, "insert into  tempservicesave_sectormaster select * from agent_sectormaster where sectorcode='" & CType(sectorcode, String) & "'")


        End If


        If frmmode = 3 Then
            parms.Add(parm(0))
            spname = "sp_del_agent_secmast"
            result_temp = objUtils.ExecuteQueryReturnSingleValuenew(constr, "insert into  tempservicesave_sectormaster select * from agent_sectormaster where sectorcode='" & CType(sectorcode, String) & "'")


        End If

        result1 = objUtils.ExecuteNonQuerynew(constr, spname, parms)

        If result1 = Nothing Or IsDBNull(result1) Then
            result1 = ""

        End If



        Return result1
    End Function



    <WebMethod()> _
    Public Function Reverse_UpdateSectors(ByVal constr As String, ByVal sectorcode As String, ByVal frmmode As String) As String
        'Dim retlist As New List(Of clsMaster)




        If Trim(ConfigurationManager.AppSettings("CustomerStopService")) <> "" Then
            Reverse_UpdateSectors = Trim(ConfigurationManager.AppSettings("CustomerStopService"))
            Exit Function
        End If


        Dim ClientIP As String = Context.Request.ServerVariables("REMOTE_ADDR")


        If objUtils.ExecuteQueryReturnStringValuenew(constr, "select 1 from service_userpermissions  where userip='" & ClientIP & "' and active=1") <> "1" Then
            Reverse_UpdateSectors = "Permission Denied"
            Exit Function
        End If





        Dim result1 As Integer
        'Dim sqlstr As String

        Dim spname As String




        Dim parms As New List(Of SqlParameter)
        Dim parm(6) As SqlParameter
        parm(6) = New SqlParameter
        parm(0) = New SqlParameter("@sectorcode", CType(sectorcode, String))
        If frmmode > 1 Then
            Dim sectorname, ctrycode, marketcode, userlogged As String

            Dim active As Integer

            Dim ds As New DataSet
            ds = objUtils.GetDataFromDatasetnew(constr, "select * from tempservicesave_sectormaster where sectorcode='" & CType(sectorcode, String) & "'")

            If ds.Tables(0).Rows.Count > 0 Then

                sectorname = ds.Tables(0).Rows(0).Item("sectorname")
                ctrycode = ds.Tables(0).Rows(0).Item("ctrycode")
                marketcode = ds.Tables(0).Rows(0).Item("plgrpcode")

                userlogged = ds.Tables(0).Rows(0).Item("moduser")
                active = ds.Tables(0).Rows(0).Item("active")

            End If
            parm(1) = New SqlParameter("@sectorname", CType(sectorname, String))
            parm(2) = New SqlParameter("@ctrycode", CType(ctrycode, String))
            parm(3) = New SqlParameter("@plgrpcode", CType(marketcode, String))

            parm(4) = New SqlParameter("@active", CType(active, Integer))
            parm(5) = New SqlParameter("@userlogged", CType(userlogged, String))

        End If

        If frmmode = 1 Then
            parms.Add(parm(0))
            spname = "sp_del_agent_secmast"

        End If


        If frmmode = 2 Then
            For p = 0 To 5
                parms.Add(parm(p))
            Next

            spname = "sp_mod_agent_secmast"

        End If


        If frmmode = 3 Then

            For p = 0 To 5
                parms.Add(parm(p))
            Next
            spname = "sp_add_agent_secMast"
        End If




        result1 = objUtils.ExecuteNonQuerynew(constr, spname, parms)

        If result1 = Nothing Or IsDBNull(result1) Then
            result1 = ""

        End If



        Return result1
    End Function

    <WebMethod()> _
    Public Function UpdateSellingTypes(ByVal constr As String, ByVal sellingtypecode As String, ByVal sellingtypename As String, ByVal dispname As String, ByVal currcode As String, ByVal plgrpcode As String, ByVal Order As Integer, ByVal active As Integer, ByVal userlogged As String, ByVal commyyn As String, ByVal frmmode As String) As String
        'Dim retlist As New List(Of clsMaster)




        If Trim(ConfigurationManager.AppSettings("CustomerStopService")) <> "" Then
            UpdateSellingTypes = Trim(ConfigurationManager.AppSettings("CustomerStopService"))
            Exit Function
        End If

        Dim ClientIP As String = Context.Request.ServerVariables("REMOTE_ADDR")


        If objUtils.ExecuteQueryReturnStringValuenew(constr, "select 1 from service_userpermissions  where userip='" & ClientIP & "' and active=1") <> "1" Then
            UpdateSellingTypes = "Permission Denied"
            Exit Function
        End If



        Dim result_temp As String

        result_temp = objUtils.ExecuteQueryReturnSingleValuenew(constr, "delete from tempservicesave_sellmast")

        Dim result1 As Integer
        Dim sqlstr As String

        Dim spname As String

        Dim p As Integer




        Dim parms As New List(Of SqlParameter)
        Dim parm(9) As SqlParameter
        parm(9) = New SqlParameter
        parm(0) = New SqlParameter("@sellcode", CType(sellingtypecode, String))
        parm(1) = New SqlParameter("@sellname", CType(sellingtypename, String))
        parm(2) = New SqlParameter("@dispname", CType(dispname, String))
        parm(3) = New SqlParameter("@currcode", CType(currcode, String))

        parm(4) = New SqlParameter("@plgrpcode", CType(plgrpcode, String))
        parm(5) = New SqlParameter("@rankorder", CType(Order, Integer))
        parm(6) = New SqlParameter("@active", CType(active, Integer))
        parm(7) = New SqlParameter("@userlogged", CType(userlogged, String))
        parm(8) = New SqlParameter("@commyn", CType(commyyn, Integer))



        If frmmode = 1 Then
            For p = 0 To 8
                parms.Add(parm(p))
            Next
            spname = "sp_add_sell"
        End If


        If frmmode = 2 Then

            For p = 0 To 8
                parms.Add(parm(p))
            Next
            spname = "sp_mod_sell"
            result_temp = objUtils.ExecuteQueryReturnSingleValuenew(constr, "insert into   tempservicesave_sellmast select * from sellmast where sellcode='" & CType(sellingtypecode, String) & "'")


        End If


        If frmmode = 3 Then
            parms.Add(parm(0))
            spname = "sp_del_sell"
            result_temp = objUtils.ExecuteQueryReturnSingleValuenew(constr, "insert into   tempservicesave_sellmast select * from sellmast where sellcode='" & CType(sellingtypecode, String) & "'")

        End If

        result1 = objUtils.ExecuteNonQuerynew(constr, spname, parms)

        If result1 = Nothing Or IsDBNull(result1) Then
            result1 = ""

        End If



        Return result1
    End Function

    <WebMethod()> _
    Public Function Reverse_UpdateSellingTypes(ByVal constr As String, ByVal sellingtypecode As String, ByVal frmmode As String) As String
        'Dim retlist As New List(Of clsMaster)




        If Trim(ConfigurationManager.AppSettings("CustomerStopService")) <> "" Then
            Reverse_UpdateSellingTypes = Trim(ConfigurationManager.AppSettings("CustomerStopService"))
            Exit Function
        End If


        Dim ClientIP As String = Context.Request.ServerVariables("REMOTE_ADDR")


        If objUtils.ExecuteQueryReturnStringValuenew(constr, "select 1 from service_userpermissions  where userip='" & ClientIP & "' and active=1") <> "1" Then
            Reverse_UpdateSellingTypes = "Permission Denied"
            Exit Function
        End If





        Dim result1 As Integer
        'Dim sqlstr As String

        Dim spname As String






        Dim parms As New List(Of SqlParameter)
        Dim parm(9) As SqlParameter
        parm(9) = New SqlParameter
        parm(0) = New SqlParameter("@sellcode", CType(sellingtypecode, String))
        If frmmode > 1 Then
            Dim sellname, dispname, plgrpcode, currcode, userlogged As String

            Dim rankorder, active, commyn As Integer

            Dim ds As New DataSet
            ds = objUtils.GetDataFromDatasetnew(constr, "select * from tempservicesave_sellmast where sellcode='" & CType(sellingtypecode, String) & "'")

            If ds.Tables(0).Rows.Count > 0 Then

                sellname = ds.Tables(0).Rows(0).Item("sellname")
                dispname = ds.Tables(0).Rows(0).Item("dispname")
                plgrpcode = ds.Tables(0).Rows(0).Item("plgrpcode")
                currcode = ds.Tables(0).Rows(0).Item("currcode")

                userlogged = ds.Tables(0).Rows(0).Item("moduser")

                rankorder = ds.Tables(0).Rows(0).Item("rankorder")
                active = ds.Tables(0).Rows(0).Item("active")
                commyn = ds.Tables(0).Rows(0).Item("commyn")

            End If



            parm(1) = New SqlParameter("@sellname", CType(sellname, String))
            parm(2) = New SqlParameter("@dispname", CType(dispname, String))
            parm(3) = New SqlParameter("@currcode", CType(currcode, String))
            parm(4) = New SqlParameter("@plgrpcode", CType(plgrpcode, String))
            parm(5) = New SqlParameter("@rankorder", CType(rankorder, Integer))
            parm(6) = New SqlParameter("@active", CType(active, Integer))
            parm(7) = New SqlParameter("@userlogged", CType(userlogged, String))
            parm(8) = New SqlParameter("@commyn", CType(commyn, Integer))
        End If

        If frmmode = 1 Then
            parms.Add(parm(0))
            spname = "sp_del_sell"

        End If


        If frmmode = 2 Then
            For p = 0 To 8
                parms.Add(parm(p))
            Next

            spname = "sp_mod_sell"

        End If


        If frmmode = 3 Then

            For p = 0 To 8
                parms.Add(parm(p))
            Next
            spname = "sp_add_sell"
        End If




        result1 = objUtils.ExecuteNonQuerynew(constr, spname, parms)

        If result1 = Nothing Or IsDBNull(result1) Then
            result1 = ""

        End If



        Return result1
    End Function


    <WebMethod()> _
    Public Function UpdateCustCategories(ByVal constr As String, ByVal code As String, ByVal name As String,
                                          ByVal sellcode As String, ByVal creditaction As String, ByVal Commision As Decimal,
                                          ByVal active As Integer, ByVal userlogged As String, ByVal inclPromo As Integer,
                                          ByVal inclearlpromo As Integer, ByVal frmmode As String) As String
        Dim result1 As Integer



        'Dim retlist As New List(Of clsMaster)

        If Trim(ConfigurationManager.AppSettings("CustomerStopService")) <> "" Then
            UpdateCustCategories = Trim(ConfigurationManager.AppSettings("CustomerStopService"))
            Exit Function
        End If
        Dim ClientIP As String = Context.Request.ServerVariables("REMOTE_ADDR")
        If objUtils.ExecuteQueryReturnStringValuenew(constr, "select 1 from service_userpermissions  where userip='" & ClientIP & "' and active=1") <> "1" Then
            UpdateCustCategories = "Permission Denied"
            Exit Function
        End If
        Dim result_temp As String
        result_temp = objUtils.ExecuteQueryReturnSingleValuenew(constr, "delete from tempservicesave_agentcat")

        Dim sqlstr As String

        Dim spname As String

        Dim p As Integer
        Dim parms As New List(Of SqlParameter)
        Dim parm(9) As SqlParameter
        parm(9) = New SqlParameter
        parm(0) = New SqlParameter("@agentcatcode", CType(code, String))
        parm(1) = New SqlParameter("@agentcatname", CType(name, String))
        parm(2) = New SqlParameter("@sellcode", CType(sellcode, String))
        parm(3) = New SqlParameter("@creditaction", CType(creditaction, String))
        parm(4) = New SqlParameter("@commissionperc", CType(Commision, Decimal))
        parm(5) = New SqlParameter("@active", CType(active, Integer))

        parm(6) = New SqlParameter("@userlogged", CType(userlogged, String))
        parm(7) = New SqlParameter("@incl_promo", CType(inclPromo, Integer))
        parm(8) = New SqlParameter("@incl_earlpromo", CType(inclearlpromo, Integer))


        If frmmode = 1 Then
            For p = 0 To 8
                parms.Add(parm(p))
            Next
            spname = "sp_add_agentcat"


        End If


        If frmmode = 2 Then

            For p = 0 To 6
                parms.Add(parm(p))
            Next
            spname = "sp_mod_agentcat"
            result_temp = objUtils.ExecuteQueryReturnSingleValuenew(constr, "insert into  tempservicesave_agentcat select * from agentcatmast where agentcatcode='" & CType(code, String) & "'")


        End If


        If frmmode = 3 Then
            parms.Add(parm(0))
            spname = "sp_del_agentcat"
            result_temp = objUtils.ExecuteQueryReturnSingleValuenew(constr, "insert into  tempservicesave_agentcat select * from agentcatmast where agentcatcode='" & CType(code, String) & "'")

        End If

        result1 = objUtils.ExecuteNonQuerynew(constr, spname, parms)

        If result1 = Nothing Or IsDBNull(result1) Then

            result1 = ""

        End If

        If frmmode = 1 Then

            If inclPromo = 1 Then
                Dim parms1 As New List(Of SqlParameter)
                Dim parmeter1(2) As SqlParameter
                ' EventLog.WriteEntry("websrv:", "Entrd 1st: ")
                Dim s As String
                s = Trim(CType(objUtils.ExecuteQueryReturnStringValuenew(constr, "select plgrpcode from sellmast where active=1 and  sellcode='" & CType(sellcode, String) & "'"), String))

                parmeter1(0) = New SqlParameter("@catcode", CType(code, String))
                parmeter1(1) = New SqlParameter("@marketcode", CType(s, String))

                parms1.Add(parmeter1(0))

                parms1.Add(parmeter1(1))
                spname = "add_category_promotion"
                result_temp = objUtils.ExecuteNonQuerynew(constr, spname, parms1)

            End If
            If inclearlpromo = 1 Then



                Dim parms2 As New List(Of SqlParameter)
                Dim parmeter2(2) As SqlParameter

                Dim s As String
                s = Trim(CType(objUtils.ExecuteQueryReturnStringValuenew(constr, "select plgrpcode from sellmast where active=1 and  sellcode='" & CType(sellcode, String) & "'"), String))

                parmeter2(0) = New SqlParameter("@catcode", CType(code, String))
                parmeter2(1) = New SqlParameter("@marketcode", CType(s, String))

                parms2.Add(parmeter2(0))

                parms2.Add(parmeter2(1))
                spname = "add_category_earlpromotion"
                result_temp = objUtils.ExecuteNonQuerynew(constr, spname, parms2)

            End If

        End If
        Return result1
    End Function

    <WebMethod()> _
    Public Function Reverse_UpdateCustCategories(ByVal constr As String, ByVal code As String, ByVal frmmode As String) As String
        'Dim retlist As New List(Of clsMaster)




        If Trim(ConfigurationManager.AppSettings("CustomerStopService")) <> "" Then
            Reverse_UpdateCustCategories = Trim(ConfigurationManager.AppSettings("CustomerStopService"))
            Exit Function
        End If


        Dim ClientIP As String = Context.Request.ServerVariables("REMOTE_ADDR")


        If objUtils.ExecuteQueryReturnStringValuenew(constr, "select 1 from service_userpermissions  where userip='" & ClientIP & "' and active=1") <> "1" Then
            Reverse_UpdateCustCategories = "Permission Denied"
            Exit Function
        End If





        Dim result1 As Integer
        'Dim sqlstr As String

        Dim spname As String

        'Dim p As Integer

        Dim parms As New List(Of SqlParameter)
        Dim parm(9) As SqlParameter
        parm(9) = New SqlParameter
        parm(0) = New SqlParameter("@agentcatcode", CType(code, String))
        If frmmode > 1 Then
            Dim name, sellcode, creditaction, commision, userlogged As String
            Dim active, inclpromo, incl_earlpromo As Integer

            Dim ds As New DataSet
            ds = objUtils.GetDataFromDatasetnew(constr, "select * from tempservicesave_agentcat where agentcatcode='" & CType(code, String) & "'")

            If ds.Tables(0).Rows.Count > 0 Then
                name = ds.Tables(0).Rows(0).Item("agentcatname")
                sellcode = ds.Tables(0).Rows(0).Item("sellcode")
                creditaction = ds.Tables(0).Rows(0).Item("creditaction")
                commision = ds.Tables(0).Rows(0).Item("commissionperc")
                inclpromo = ds.Tables(0).Rows(0).Item("incl_promo")
                incl_earlpromo = ds.Tables(0).Rows(0).Item("incl_earlpromo")

                userlogged = ds.Tables(0).Rows(0).Item("moduser")
                active = ds.Tables(0).Rows(0).Item("active")

            End If

            parm(1) = New SqlParameter("@agentcatname", CType(name, String))
            parm(2) = New SqlParameter("@sellcode", CType(sellcode, String))
            parm(3) = New SqlParameter("@creditaction", CType(creditaction, String))
            parm(4) = New SqlParameter("@commissionperc", CType(commision, Decimal))
            parm(5) = New SqlParameter("@active", CType(active, Integer))

            parm(6) = New SqlParameter("@userlogged", CType(userlogged, String))
            parm(7) = New SqlParameter("@incl_promo", CType(inclPromo, Integer))
            parm(8) = New SqlParameter("@incl_earlpromo", CType(incl_earlpromo, Integer))
        End If

        If frmmode = 1 Then
            parms.Add(parm(0))
            spname = "sp_del_agentcat"


            Dim result_temp As String
            result_temp = objUtils.ExecuteQueryReturnSingleValuenew(constr, "delete from promo_agentcat  where agentcatcode='" & CType(code, String) & "'")
            result_temp = objUtils.ExecuteQueryReturnSingleValuenew(constr, "delete from earlypromagentcat_detail  where agentcatcode='" & CType(code, String) & "'")

        End If


        If frmmode = 2 Then
            For p = 0 To 6
                parms.Add(parm(p))
            Next

            spname = "sp_mod_agentcat"

        End If


        If frmmode = 3 Then

            For p = 0 To 8
                parms.Add(parm(p))
            Next
            spname = "sp_add_agentcat"
        End If

        result1 = objUtils.ExecuteNonQuerynew(constr, spname, parms)

        If result1 = Nothing Or IsDBNull(result1) Then
            result1 = ""

        End If



        Return result1
    End Function

    <WebMethod()> _
    Public Function UpdateCustmaindet(ByVal constr As String, ByVal lastno As String, ByVal custcode As String, ByVal custname As String, ByVal shortname As String, ByVal category As String, ByVal sellingtype As String, ByVal currencycode As String, ByVal ctrycode As String, ByVal cityhdncode As String, ByVal salesperson As String, ByVal market As String, ByVal sector As String, ByVal active As Integer, ByVal commission As Decimal, ByVal userlogged As String, ByVal ctrlacctcode As String, ByVal webcurrency As String, ByVal regno As String, ByVal frmmode As String) As String
        'Dim retlist As New List(Of clsMaster)

        Dim curr_lastno As String

        If Trim(ConfigurationManager.AppSettings("CustomerStopService")) <> "" Then
            UpdateCustmaindet = Trim(ConfigurationManager.AppSettings("CustomerStopService"))
            Exit Function
        End If

        Dim ClientIP As String = Context.Request.ServerVariables("REMOTE_ADDR")


        If objUtils.ExecuteQueryReturnStringValuenew(constr, "select 1 from service_userpermissions  where userip='" & ClientIP & "' and active=1") <> "1" Then
            UpdateCustmaindet = "Permission Denied"
            Exit Function
        End If




        If frmmode = 1 Or frmmode = 4 Then
            curr_lastno = objUtils.ExecuteQueryReturnSingleValuenew(constr, "select lastno from docgen where optionname='CUSTCODE'")
            If curr_lastno = lastno Then
                Dim optionval As String
                optionval = objUtils.GetAutoDocNoWTnew(constr, "CUSTCODE")
            Else
                UpdateCustmaindet = "CustomerAutoNumber"
                Exit Function
            End If
          
        End If




        Dim result_temp As String

        result_temp = objUtils.ExecuteQueryReturnSingleValuenew(constr, "delete from tempservicesave_agentmast")

        Dim result1 As Integer
        Dim sqlstr As String

        Dim spname As String

        Dim p As Integer




        Dim parms As New List(Of SqlParameter)
        Dim parm(19) As SqlParameter
        parm(19) = New SqlParameter
        parm(0) = New SqlParameter("@agentcode", CType(custcode, String))
        parm(1) = New SqlParameter("@agentname", CType(custname, String))
        parm(2) = New SqlParameter("@shortname", CType(shortname, String))
        parm(3) = New SqlParameter("@catcode", CType(category, String))
        parm(4) = New SqlParameter("@sellcode", CType(sellingtype, String))
        parm(5) = New SqlParameter("@othsellcode", DBNull.Value)
        parm(6) = New SqlParameter("@tktsellcode", DBNull.Value)
        parm(7) = New SqlParameter("@currcode", CType(currencycode, String))
        parm(8) = New SqlParameter("@ctrycode", CType(ctrycode, String))
        parm(9) = New SqlParameter("@citycode", CType(cityhdncode, String))
        parm(10) = New SqlParameter("@spersoncode", CType(salesperson, String))
        parm(11) = New SqlParameter("@plgrpcode", CType(market, String))
        parm(12) = New SqlParameter("@sectorcode", CType(sector, String))
        parm(13) = New SqlParameter("@active", CType(active, Integer))
        parm(14) = New SqlParameter("@commperc", CType(commission, Decimal))
        parm(15) = New SqlParameter("@userlogged", CType(userlogged, String))
        parm(16) = New SqlParameter("@controlacctcode", CType(ctrlacctcode, String))
        parm(17) = New SqlParameter("@webcurrcode", CType(webcurrency, String))
        parm(18) = New SqlParameter("@regno", CType(regno, String))


        If frmmode = 1 Or frmmode = 4 Then
            For p = 0 To 17
                parms.Add(parm(p))
            Next
            spname = "sp_add_agentmast"
        End If


        If frmmode = 2 Then

            For p = 0 To 17
                parms.Add(parm(p))
            Next
            spname = "sp_mod_agentmast"
            result_temp = objUtils.ExecuteQueryReturnSingleValuenew(constr, "insert into  tempservicesave_agentmast select * from agentmast where agentcode='" & CType(custcode, String) & "'")


        End If


        If frmmode = 3 Then
            parms.Add(parm(0))
            spname = "sp_del_agentmast"
            result_temp = objUtils.ExecuteQueryReturnSingleValuenew(constr, "insert into  tempservicesave_agentmast select * from agentmast where agentcode='" & CType(custcode, String) & "'")
            result_temp = objUtils.ExecuteQueryReturnSingleValuenew(constr, "insert into  tempservicesaveagentmast_mulltiemail select * from agentmast_mulltiemail where agentcode='" & CType(custcode, String) & "'")
            result_temp = objUtils.ExecuteQueryReturnSingleValuenew(constr, "insert into  tempservicesave_agentmast_visit select * from agentmast_visit where agentcode='" & CType(custcode, String) & "'")
            result_temp = objUtils.ExecuteQueryReturnSingleValuenew(constr, "insert into  tempservicesave_agentmast_survey select * from agentmast_survey   where agentcode='" & CType(custcode, String) & "'")
            result_temp = objUtils.ExecuteQueryReturnSingleValuenew(constr, "delete from agentmast_mulltiemail  where agentcode='" & CType(custcode, String) & "'")

        End If


        result1 = objUtils.ExecuteNonQuerynew(constr, spname, parms)

        If frmmode = 4 Then

            spname = "sp_update_registration"

            Dim parms1 As New List(Of SqlParameter)
            For p = 0 To 17
                parms1.Add(parm(0))
                parms1.Add(parm(15))
                parms1.Add(parm(18))
            Next

            result_temp = objUtils.ExecuteNonQuerynew(constr, spname, parms1)

        End If



        If result1 = Nothing Or IsDBNull(result1) Then
            result1 = ""

        End If



        Return result1
    End Function




    <WebMethod()> _
    Public Function Reverse_UpdateCustmaindet(ByVal constr As String, ByVal lastno As String, ByVal code As String, ByVal regno As String, ByVal frmmode As String) As String
        'Dim retlist As New List(Of clsMaster)




        If Trim(ConfigurationManager.AppSettings("CustomerStopService")) <> "" Then
            Reverse_UpdateCustmaindet = Trim(ConfigurationManager.AppSettings("CustomerStopService"))
            Exit Function
        End If


        Dim ClientIP As String = Context.Request.ServerVariables("REMOTE_ADDR")


        If objUtils.ExecuteQueryReturnStringValuenew(constr, "select 1 from service_userpermissions  where userip='" & ClientIP & "' and active=1") <> "1" Then
            Reverse_UpdateCustmaindet = "Permission Denied"
            Exit Function
        End If

        Dim curr_lastno As String
        Dim t As String

        If frmmode = 1 Or frmmode = 4 Then
            curr_lastno = objUtils.ExecuteQueryReturnSingleValuenew(constr, "select lastno from docgen where optionname='CUSTCODE'")
            If curr_lastno - 1 = lastno Then
                t = objUtils.ExecuteQueryReturnStringValuenew(constr, "update docgen set lastno=" & lastno & " where optionname='CUSTCODE'")

            End If

        End If



        Dim result1 As Integer
        'Dim sqlstr As String

        Dim spname As String

        'Dim p As Integer

        Dim parms As New List(Of SqlParameter)
        Dim parm(19) As SqlParameter
        parm(19) = New SqlParameter
        parm(0) = New SqlParameter("@agentcode", CType(code, String))
        If frmmode > 1 Then
            Dim name, shortname, catcode, sellcode, othsellcode, tktsellcode, currcode, ctrycode, citycode, spersoncode,
                plgrpcode, sectorcode, userlogged, controlacctcode, webcurrcode As String
            Dim active, commission As Integer

            Dim ds As New DataSet
            ds = objUtils.GetDataFromDatasetnew(constr, "select * from tempservicesave_agentmast where agentcode='" & CType(code, String) & "'")

            If ds.Tables(0).Rows.Count > 0 Then
                name = ds.Tables(0).Rows(0).Item("agentname")
                shortname = ds.Tables(0).Rows(0).Item("shortname")
                catcode = ds.Tables(0).Rows(0).Item("catcode")
                sellcode = ds.Tables(0).Rows(0).Item("sellcode")
                othsellcode = ds.Tables(0).Rows(0).Item("othsellcode")
                tktsellcode = ds.Tables(0).Rows(0).Item("tktsellcode")
                currcode = ds.Tables(0).Rows(0).Item("currcode")
                ctrycode = ds.Tables(0).Rows(0).Item("ctrycode")
                citycode = ds.Tables(0).Rows(0).Item("citycode")
                spersoncode = ds.Tables(0).Rows(0).Item("spersoncode")
                plgrpcode = ds.Tables(0).Rows(0).Item("plgrpcode")
                sectorcode = ds.Tables(0).Rows(0).Item("sectorcode")
                controlacctcode = ds.Tables(0).Rows(0).Item("controlacctcode")

                webcurrcode = ds.Tables(0).Rows(0).Item("webcurrcode")

                'regno = ds.Tables(0).Rows(0).Item("incl_earlpromo")
                userlogged = ds.Tables(0).Rows(0).Item("moduser")
                active = ds.Tables(0).Rows(0).Item("active")
                commission = ds.Tables(0).Rows(0).Item("commperc")

            End If

            parm(1) = New SqlParameter("@agentname", CType(name, String))
            parm(2) = New SqlParameter("@shortname", CType(shortname, String))
            parm(3) = New SqlParameter("@catcode", CType(catcode, String))
            parm(4) = New SqlParameter("@sellcode", CType(sellcode, String))
            parm(5) = New SqlParameter("@othsellcode", DBNull.Value)
            parm(6) = New SqlParameter("@tktsellcode", DBNull.Value)
            parm(7) = New SqlParameter("@currcode", CType(currcode, String))
            parm(8) = New SqlParameter("@ctrycode", CType(ctrycode, String))
            parm(9) = New SqlParameter("@citycode", CType(citycode, String))
            parm(10) = New SqlParameter("@spersoncode", CType(spersoncode, String))
            parm(11) = New SqlParameter("@plgrpcode", CType(plgrpcode, String))
            parm(12) = New SqlParameter("@sectorcode", CType(sectorcode, String))
            parm(13) = New SqlParameter("@active", CType(active, Integer))
            parm(14) = New SqlParameter("@commperc", CType(commission, Decimal))
            parm(15) = New SqlParameter("@userlogged", CType(userlogged, String))
            parm(16) = New SqlParameter("@controlacctcode", CType(controlacctcode, String))
            parm(17) = New SqlParameter("@webcurrcode", CType(webcurrcode, String))
            parm(18) = New SqlParameter("@regno", CType(regno, String))

        End If

        If frmmode = 1 Or frmmode = 4 Then
            parms.Add(parm(0))
            spname = "sp_del_agentmast"


            Dim result_temp As String
            result_temp = objUtils.ExecuteQueryReturnSingleValuenew(constr, "update registration  set approve=null,approvedate=null,approveuser=null  where regno='" & CType(code, String) & "'")

        End If


        If frmmode = 2 Then
            For p = 0 To 17
                parms.Add(parm(p))
            Next

            spname = "sp_mod_agentmast"

        End If


        If frmmode = 3 Then

            For p = 0 To 17
                parms.Add(parm(p))
            Next
            spname = "sp_add_agentmast"
            Dim result_temp As String
            result_temp = objUtils.ExecuteQueryReturnSingleValuenew(constr, "insert into agentmast_mulltiemail  select * from tempservicesaveagentmast_mulltiemail where agentcode='" & CType(code, String) & "'")
            result_temp = objUtils.ExecuteQueryReturnSingleValuenew(constr, "insert into  agentmast_visit select * from tempservicesave_agentmast_visit where agentcode='" & CType(code, String) & "'")
            result_temp = objUtils.ExecuteQueryReturnSingleValuenew(constr, "insert into  agentmast_survey  select * from tempservicesave_agentmast_survey   where agentcode='" & CType(code, String) & "'")

        End If

        result1 = objUtils.ExecuteNonQuerynew(constr, spname, parms)

        If result1 = Nothing Or IsDBNull(result1) Then
            result1 = ""

        End If



        Return result1
    End Function


    <WebMethod()> _
    Public Function UpdateResrvDet(ByVal constr As String, ByVal custcode As String, ByVal resadd1 As String, ByVal resadd2 As String, ByVal resadd3 As String, ByVal ResPhone1 As String, ByVal ResPhone2 As String, ByVal ResFax As String, ByVal ResContact1 As String, ByVal ResContact2 As String, ByVal designation As String, ByVal iatano As String, ByVal ResEmail As String, ByVal web As String, ByVal Commby As Integer, ByVal userlogged As String, ByVal resmob As String, ByVal frmmode As String) As String
        'Dim retlist As New List(Of clsMaster)




        If Trim(ConfigurationManager.AppSettings("CustomerStopService")) <> "" Then
            UpdateResrvDet = Trim(ConfigurationManager.AppSettings("CustomerStopService"))
            Exit Function
        End If

        Dim ClientIP As String = Context.Request.ServerVariables("REMOTE_ADDR")


        If objUtils.ExecuteQueryReturnStringValuenew(constr, "select 1 from service_userpermissions  where userip='" & ClientIP & "' and active=1") <> "1" Then
            UpdateResrvDet = "Permission Denied"
            Exit Function
        End If



        Dim result_temp As String

        result_temp = objUtils.ExecuteQueryReturnSingleValuenew(constr, "delete from tempservicesave_agentmast")

        Dim result1 As Integer
        Dim sqlstr As String

        Dim spname As String

        Dim p As Integer




        Dim parms As New List(Of SqlParameter)
        Dim parm(16) As SqlParameter
        parm(16) = New SqlParameter
        parm(0) = New SqlParameter("@agentcode", CType(custcode, String))
        parm(1) = New SqlParameter("@add1", CType(resadd1, String))
        parm(2) = New SqlParameter("@add2", CType(resadd2, String))
        parm(3) = New SqlParameter("@add3", CType(resadd3, String))
        parm(4) = New SqlParameter("@tel1", CType(ResPhone1, String))
        parm(5) = New SqlParameter("@tel2", CType(ResPhone2, String))
        parm(6) = New SqlParameter("@fax", CType(ResFax, String))
        parm(7) = New SqlParameter("@contact1", CType(ResContact1, String))
        parm(8) = New SqlParameter("@contact2", CType(ResContact2, String))
        parm(9) = New SqlParameter("@designation", CType(designation, String))
        parm(10) = New SqlParameter("@iatano", CType(iatano, String))
        parm(11) = New SqlParameter("@email", CType(ResEmail, String))
        parm(12) = New SqlParameter("@web", CType(web, String))
        parm(13) = New SqlParameter("@commmode", CType(Commby, Integer))
        parm(14) = New SqlParameter("@moduser", CType(userlogged, String))
        parm(15) = New SqlParameter("@mobileno", CType(resmob, String))


        If frmmode = 1 Then
            For p = 0 To 15
                parms.Add(parm(p))
            Next
            spname = "sp_updateres_agentmast"
            result_temp = objUtils.ExecuteQueryReturnSingleValuenew(constr, "insert into  tempservicesave_agentmast select * from agentmast where agentcode='" & CType(custcode, String) & "'")

        End If


        If frmmode = 2 Then

            For p = 0 To 15
                parms.Add(parm(p))
            Next
            spname = "sp_updateres_agentmast"

            result_temp = objUtils.ExecuteQueryReturnSingleValuenew(constr, "insert into  tempservicesave_agentmast select * from agentmast where agentcode='" & CType(custcode, String) & "'")


        End If


        If frmmode = 3 Then
            parms.Add(parm(0))
            spname = "sp_del_agentmast"
            result_temp = objUtils.ExecuteQueryReturnSingleValuenew(constr, "insert into  tempservicesave_agentmast select * from agentmast where agentcode='" & CType(custcode, String) & "'")
            result_temp = objUtils.ExecuteQueryReturnSingleValuenew(constr, "insert into  tempservicesaveagentmast_mulltiemail select * from agentmast_mulltiemail where agentcode='" & CType(custcode, String) & "'")
            result_temp = objUtils.ExecuteQueryReturnSingleValuenew(constr, "insert into  tempservicesave_agentmast_visit select * from agentmast_visit where agentcode='" & CType(custcode, String) & "'")
            result_temp = objUtils.ExecuteQueryReturnSingleValuenew(constr, "insert into  tempservicesave_agentmast_survey select * from agentmast_survey   where agentcode='" & CType(custcode, String) & "'")

        End If


        result1 = objUtils.ExecuteNonQuerynew(constr, spname, parms)



        If result1 = Nothing Or IsDBNull(result1) Then
            result1 = ""

        End If



        Return result1
    End Function


    <WebMethod()> _
    Public Function Reverse_UpdateResrvDet(ByVal constr As String, ByVal code As String, ByVal frmmode As String) As String
        'Dim retlist As New List(Of clsMaster)



        If Trim(ConfigurationManager.AppSettings("CustomerStopService")) <> "" Then
            Reverse_UpdateResrvDet = Trim(ConfigurationManager.AppSettings("CustomerStopService"))
            Exit Function
        End If


        Dim ClientIP As String = Context.Request.ServerVariables("REMOTE_ADDR")


        If objUtils.ExecuteQueryReturnStringValuenew(constr, "select 1 from service_userpermissions  where userip='" & ClientIP & "' and active=1") <> "1" Then
            Reverse_UpdateResrvDet = "Permission Denied"
            Exit Function
        End If





        Dim result1 As Integer
        'Dim sqlstr As String

        Dim spname As String

        'Dim p As Integer

        Dim parms As New List(Of SqlParameter)
        Dim parm(16) As SqlParameter
        parm(16) = New SqlParameter
        parm(0) = New SqlParameter("@agentcode", CType(code, String))
        If frmmode > 1 Then
            Dim add1, add2, add3, tel1, tel2, fax, contact1, contact2, designation, iatano, email, web, moduser, mobileno As String
            Dim commmode As Integer

            Dim ds As New DataSet
            ds = objUtils.GetDataFromDatasetnew(constr, "select * from tempservicesave_agentmast where agentcode='" & CType(code, String) & "'")

            If ds.Tables(0).Rows.Count > 0 Then
                add1 = ds.Tables(0).Rows(0).Item("add1")
                add2 = ds.Tables(0).Rows(0).Item("add2")
                add3 = ds.Tables(0).Rows(0).Item("add3")
                tel1 = ds.Tables(0).Rows(0).Item("tel1")
                tel2 = ds.Tables(0).Rows(0).Item("tel2")
                contact1 = ds.Tables(0).Rows(0).Item("contact1")
                contact2 = ds.Tables(0).Rows(0).Item("contact2")
                designation = ds.Tables(0).Rows(0).Item("designation")
                iatano = ds.Tables(0).Rows(0).Item("iatano")
                email = ds.Tables(0).Rows(0).Item("email")
                web = ds.Tables(0).Rows(0).Item("web")
                moduser = ds.Tables(0).Rows(0).Item("moduser")
                mobileno = ds.Tables(0).Rows(0).Item("mobileno")

            End If

            parm(1) = New SqlParameter("@add1", CType(add1, String))
            parm(2) = New SqlParameter("@add2", CType(add2, String))
            parm(3) = New SqlParameter("@add3", CType(add3, String))
            parm(4) = New SqlParameter("@tel1", CType(tel1, String))
            parm(5) = New SqlParameter("@tel2", CType(tel2, String))

            parm(6) = New SqlParameter("@fax", CType(fax, String))
            parm(7) = New SqlParameter("@contact1", CType(contact1, String))
            parm(8) = New SqlParameter("@contact2", CType(contact2, String))
            parm(9) = New SqlParameter("@designation", CType(designation, String))
            parm(10) = New SqlParameter("@iatano", CType(iatano, String))
            parm(11) = New SqlParameter("@email", CType(email, String))
            parm(12) = New SqlParameter("@web", CType(web, String))
            parm(13) = New SqlParameter("@commmode", CType(commmode, Integer))
            parm(14) = New SqlParameter("@moduser", CType(moduser, String))
            parm(15) = New SqlParameter("@mobileno", CType(mobileno, String))
        End If

        If frmmode = 1 Then
            For p = 0 To 15
                parms.Add(parm(p))
            Next

            spname = "sp_updateres_agentmast"

        End If


        If frmmode = 2 Then
            For p = 0 To 15
                parms.Add(parm(p))
            Next

            spname = "sp_updateres_agentmast"

        End If



        If frmmode = 3 Then

            For p = 0 To 17
                parms.Add(parm(p))
            Next

            Dim result_temp As String
            result1 = objUtils.ExecuteQueryReturnSingleValuenew(constr, "insert into agentmast  select * from  where agentcode='" & CType(code, String) & "'")
            result_temp = objUtils.ExecuteQueryReturnSingleValuenew(constr, "insert into agentmast_mulltiemail  select * from tempservicesaveagentmast_mulltiemail where agentcode='" & CType(code, String) & "'")
            result_temp = objUtils.ExecuteQueryReturnSingleValuenew(constr, "insert into  agentmast_visit select * from tempservicesave_agentmast_visit where agentcode='" & CType(code, String) & "'")
            result_temp = objUtils.ExecuteQueryReturnSingleValuenew(constr, "insert into  agentmast_survey  select * from tempservicesave_agentmast_survey   where agentcode='" & CType(code, String) & "'")

        End If

        If frmmode = 1 Or frmmode = 2 Then


            result1 = objUtils.ExecuteNonQuerynew(constr, spname, parms)

        End If
        If result1 = Nothing Or IsDBNull(result1) Then
            result1 = ""

        End If



        Return result1
    End Function


    <WebMethod()> _
    Public Function UpdateCustSalesDet(ByVal constr As String, ByVal custcode As String, ByVal SaleRecommended As String,
                    ByVal SaleTelephone1 As String, ByVal SaleTelephone2 As String, ByVal salesmob As String,
                    ByVal SaleFax As String, ByVal SaleContact1 As String, ByVal SaleContact2 As String, ByVal SaleEmail As String,
                    ByVal userlogged As String, ByVal frmmode As String) As String

        'Dim retlist As New List(Of clsMaster)

        If Trim(ConfigurationManager.AppSettings("CustomerStopService")) <> "" Then
            UpdateCustSalesDet = Trim(ConfigurationManager.AppSettings("CustomerStopService"))
            Exit Function
        End If

        Dim ClientIP As String = Context.Request.ServerVariables("REMOTE_ADDR")


        If objUtils.ExecuteQueryReturnStringValuenew(constr, "select 1 from service_userpermissions  where userip='" & ClientIP & "' and active=1") <> "1" Then
            UpdateCustSalesDet = "Permission Denied"
            Exit Function
        End If
        Dim result_temp As String

        result_temp = objUtils.ExecuteQueryReturnSingleValuenew(constr, "delete from tempservicesave_agentmast")

        Dim result1 As Integer
        Dim sqlstr As String

        Dim spname As String

        Dim p As Integer
        Dim parms As New List(Of SqlParameter)
        Dim parm(10) As SqlParameter
        parm(10) = New SqlParameter
        parm(0) = New SqlParameter("@agentcode", CType(custcode, String))
        parm(1) = New SqlParameter("@recommby", CType(SaleRecommended, String))
        parm(2) = New SqlParameter("@stel1", CType(SaleTelephone1, String))
        parm(3) = New SqlParameter("@stel2", CType(SaleTelephone2, String))
        parm(4) = New SqlParameter("@smobile", CType(salesmob, String))
        parm(5) = New SqlParameter("@sfax", CType(SaleFax, String))
        parm(6) = New SqlParameter("@scontact1", CType(SaleContact1, String))
        parm(7) = New SqlParameter("@scontact2", CType(SaleContact2, String))
        parm(8) = New SqlParameter("@semail", CType(SaleEmail, String))
        parm(9) = New SqlParameter("@userlogged", CType(userlogged, String))

        If frmmode = 1 Then
            For p = 0 To 9
                parms.Add(parm(p))
            Next
            spname = "sp_updatesales_agentmast"
            result_temp = objUtils.ExecuteQueryReturnSingleValuenew(constr, "insert into  tempservicesave_agentmast select * from agentmast where agentcode='" & CType(custcode, String) & "'")

        End If

        If frmmode = 2 Then

            For p = 0 To 9
                parms.Add(parm(p))
            Next
            spname = "sp_updatesales_agentmast"
            result_temp = objUtils.ExecuteQueryReturnSingleValuenew(constr, "insert into  tempservicesave_agentmast select * from agentmast where agentcode='" & CType(custcode, String) & "'")

        End If
        If frmmode = 3 Then
            parms.Add(parm(0))
            spname = "sp_del_agentmast"
            result_temp = objUtils.ExecuteQueryReturnSingleValuenew(constr, "insert into  tempservicesave_agentmast select * from agentmast where agentcode='" & CType(custcode, String) & "'")
            result_temp = objUtils.ExecuteQueryReturnSingleValuenew(constr, "insert into  tempservicesaveagentmast_mulltiemail select * from agentmast_mulltiemail where agentcode='" & CType(custcode, String) & "'")
            result_temp = objUtils.ExecuteQueryReturnSingleValuenew(constr, "insert into  tempservicesave_agentmast_visit select * from agentmast_visit where agentcode='" & CType(custcode, String) & "'")
            result_temp = objUtils.ExecuteQueryReturnSingleValuenew(constr, "insert into  tempservicesave_agentmast_survey select * from agentmast_survey   where agentcode='" & CType(custcode, String) & "'")

        End If



        result1 = objUtils.ExecuteNonQuerynew(constr, spname, parms)
        If result1 = Nothing Or IsDBNull(result1) Then
            result1 = ""

        End If

        Return result1
    End Function
    <WebMethod()> _
    Public Function Reverse_UpdateCustSalesDet(ByVal constr As String, ByVal code As String, ByVal frmmode As String) As String

        If Trim(ConfigurationManager.AppSettings("CustomerStopService")) <> "" Then
            Reverse_UpdateCustSalesDet = Trim(ConfigurationManager.AppSettings("CustomerStopService"))
            Exit Function
        End If


        Dim ClientIP As String = Context.Request.ServerVariables("REMOTE_ADDR")


        If objUtils.ExecuteQueryReturnStringValuenew(constr, "select 1 from service_userpermissions  where userip='" & ClientIP & "' and active=1") <> "1" Then
            Reverse_UpdateCustSalesDet = "Permission Denied"
            Exit Function
        End If

        Dim result1 As Integer
        Dim spname As String


        Dim parms As New List(Of SqlParameter)
        Dim parm(10) As SqlParameter
        parm(10) = New SqlParameter
        parm(0) = New SqlParameter("@agentcode", CType(code, String))
        If frmmode > 1 Then
            Dim recomndby, stel1, stel2, smobile, sfax, scontact1, scontact2, semail, userlogged As String

            Dim active As Integer

            Dim ds As New DataSet
            ds = objUtils.GetDataFromDatasetnew(constr, "select * from tempservicesave_agentmast where agentcode='" & CType(code, String) & "'")

            If ds.Tables(0).Rows.Count > 0 Then
                recomndby = ds.Tables(0).Rows(0).Item("recommby")
                stel1 = ds.Tables(0).Rows(0).Item("stel1")
                stel2 = ds.Tables(0).Rows(0).Item("stel2")
                smobile = ds.Tables(0).Rows(0).Item("smobile")
                sfax = ds.Tables(0).Rows(0).Item("sfax")
                scontact1 = ds.Tables(0).Rows(0).Item("scontact1")
                scontact2 = ds.Tables(0).Rows(0).Item("scontact2")
                semail = ds.Tables(0).Rows(0).Item("semail")
                userlogged = ds.Tables(0).Rows(0).Item("moduser")
            End If
            parm(1) = New SqlParameter("@recommby", CType(recomndby, String))
            parm(2) = New SqlParameter("@stel1", CType(stel1, String))
            parm(3) = New SqlParameter("@stel2", CType(stel2, String))
            parm(4) = New SqlParameter("@smobile", CType(smobile, String))
            parm(5) = New SqlParameter("@sfax", CType(sfax, String))
            parm(6) = New SqlParameter("@scontact1", CType(scontact1, String))
            parm(7) = New SqlParameter("@scontact2", CType(scontact2, String))
            parm(8) = New SqlParameter("@semail", CType(semail, String))
            parm(9) = New SqlParameter("@userlogged", CType(userlogged, String))

        End If
        If frmmode = 1 Then
            For p = 0 To 9
                parms.Add(parm(p))
            Next

            spname = "sp_updatesales_agentmast"
        End If
        If frmmode = 2 Then
            For p = 0 To 9
                parms.Add(parm(p))
            Next

            spname = "sp_updatesales_agentmast"

        End If

        If frmmode = 3 Then
            Dim result_temp As String
            result1 = objUtils.ExecuteQueryReturnSingleValuenew(constr, "insert into  agentmast select * from tempservicesave_agentmast where agentcode='" & CType(code, String) & "'")
            result_temp = objUtils.ExecuteQueryReturnSingleValuenew(constr, "insert into agentmast_mulltiemail  select * from tempservicesaveagentmast_mulltiemail where agentcode='" & CType(code, String) & "'")
            result_temp = objUtils.ExecuteQueryReturnSingleValuenew(constr, "insert into  agentmast_visit select * from tempservicesave_agentmast_visit where agentcode='" & CType(code, String) & "'")
            result_temp = objUtils.ExecuteQueryReturnSingleValuenew(constr, "insert into  agentmast_survey  select * from tempservicesave_agentmast_survey   where agentcode='" & CType(code, String) & "'")

        End If

        If frmmode = 1 Or frmmode = 2 Then
            result1 = objUtils.ExecuteNonQuerynew(constr, spname, parms)

        End If

        If result1 = Nothing Or IsDBNull(result1) Then
            result1 = ""

        End If

        Return result1
    End Function


    <WebMethod()> _
    Public Function UpdateCustGen(ByVal constr As String, ByVal custcode As String, ByVal custGeneral As String,
                    ByVal userlogged As String, ByVal frmmode As String) As String

        'Dim retlist As New List(Of clsMaster)

        If Trim(ConfigurationManager.AppSettings("CustomerStopService")) <> "" Then
            UpdateCustGen = Trim(ConfigurationManager.AppSettings("CustomerStopService"))
            Exit Function
        End If

        Dim ClientIP As String = Context.Request.ServerVariables("REMOTE_ADDR")


        If objUtils.ExecuteQueryReturnStringValuenew(constr, "select 1 from service_userpermissions  where userip='" & ClientIP & "' and active=1") <> "1" Then
            UpdateCustGen = "Permission Denied"
            Exit Function
        End If
        Dim result_temp As String

        result_temp = objUtils.ExecuteQueryReturnSingleValuenew(constr, "delete from tempservicesave_agentmast")

        Dim result1 As Integer
        Dim sqlstr As String

        Dim spname As String



        Dim p As Integer
        Dim parms As New List(Of SqlParameter)
        Dim parm(3) As SqlParameter
        parm(3) = New SqlParameter
        parm(0) = New SqlParameter("@agentcode", CType(custcode, String))
        parm(1) = New SqlParameter("@general", CType(custGeneral, String))
        parm(2) = New SqlParameter("@userlogged", CType(userlogged, String))

        If frmmode = 1 Then
            For p = 0 To 2
                parms.Add(parm(p))
            Next
            spname = "sp_updatecom_agentmast"
            result_temp = objUtils.ExecuteQueryReturnSingleValuenew(constr, "insert into  tempservicesave_agentmast select * from agentmast where agentcode='" & CType(custcode, String) & "'")

        End If

        If frmmode = 2 Then

            For p = 0 To 2
                parms.Add(parm(p))
            Next
            spname = "sp_updatecom_agentmast"
            result_temp = objUtils.ExecuteQueryReturnSingleValuenew(constr, "insert into  tempservicesave_agentmast select * from agentmast where agentcode='" & CType(custcode, String) & "'")

        End If
        If frmmode = 3 Then
            parms.Add(parm(0))
            spname = "sp_del_agentmast"
            result_temp = objUtils.ExecuteQueryReturnSingleValuenew(constr, "insert into  tempservicesave_agentmast select * from agentmast where agentcode='" & CType(custcode, String) & "'")
            result_temp = objUtils.ExecuteQueryReturnSingleValuenew(constr, "insert into  tempservicesaveagentmast_mulltiemail select * from agentmast_mulltiemail where agentcode='" & CType(custcode, String) & "'")
            result_temp = objUtils.ExecuteQueryReturnSingleValuenew(constr, "insert into  tempservicesave_agentmast_visit select * from agentmast_visit where agentcode='" & CType(custcode, String) & "'")
            result_temp = objUtils.ExecuteQueryReturnSingleValuenew(constr, "insert into  tempservicesave_agentmast_survey select * from agentmast_survey   where agentcode='" & CType(custcode, String) & "'")

        End If



        result1 = objUtils.ExecuteNonQuerynew(constr, spname, parms)
        If result1 = Nothing Or IsDBNull(result1) Then
            result1 = ""

        End If

        Return result1
    End Function


    <WebMethod()> _
    Public Function Reverse_UpdateCustGen(ByVal constr As String, ByVal code As String, ByVal frmmode As String) As String
        If Trim(ConfigurationManager.AppSettings("CustomerStopService")) <> "" Then
            Reverse_UpdateCustGen = Trim(ConfigurationManager.AppSettings("CustomerStopService"))
            Exit Function
        End If


        Dim ClientIP As String = Context.Request.ServerVariables("REMOTE_ADDR")


        If objUtils.ExecuteQueryReturnStringValuenew(constr, "select 1 from service_userpermissions  where userip='" & ClientIP & "' and active=1") <> "1" Then
            Reverse_UpdateCustGen = "Permission Denied"
            Exit Function
        End If

        Dim result1 As Integer
        Dim spname As String


        Dim parms As New List(Of SqlParameter)
        Dim parm(3) As SqlParameter
        parm(3) = New SqlParameter
        parm(0) = New SqlParameter("@agentcode", CType(code, String))
        If frmmode > 1 Then
            Dim general As String


            Dim userlogged As String

            Dim ds As New DataSet
            ds = objUtils.GetDataFromDatasetnew(constr, "select * from tempservicesave_agentmast where agentcode='" & CType(code, String) & "'")

            If ds.Tables(0).Rows.Count > 0 Then
                general = ds.Tables(0).Rows(0).Item("general")
                userlogged = ds.Tables(0).Rows(0).Item("moduser")
            End If
            parm(1) = New SqlParameter("@general", CType(general, String))

            parm(2) = New SqlParameter("@userlogged", CType(userlogged, String))

        End If
        If frmmode = 1 Then
            For p = 0 To 2
                parms.Add(parm(p))
            Next

            spname = "sp_updatecom_agentmast"
        End If
        If frmmode = 2 Then
            For p = 0 To 2
                parms.Add(parm(p))
            Next

            spname = "sp_updatecom_agentmast"

        End If

        If frmmode = 3 Then
            Dim result_temp As String
            result1 = objUtils.ExecuteQueryReturnSingleValuenew(constr, "insert into  agentmast select * from tempservicesave_agentmast where agentcode='" & CType(code, String) & "'")
            result_temp = objUtils.ExecuteQueryReturnSingleValuenew(constr, "insert into agentmast_mulltiemail  select * from tempservicesaveagentmast_mulltiemail where agentcode='" & CType(code, String) & "'")
            result_temp = objUtils.ExecuteQueryReturnSingleValuenew(constr, "insert into  agentmast_visit select * from tempservicesave_agentmast_visit where agentcode='" & CType(code, String) & "'")
            result_temp = objUtils.ExecuteQueryReturnSingleValuenew(constr, "insert into  agentmast_survey  select * from tempservicesave_agentmast_survey   where agentcode='" & CType(code, String) & "'")

        End If

        If frmmode = 1 Or frmmode = 2 Then
            result1 = objUtils.ExecuteNonQuerynew(constr, spname, parms)

        End If

        If result1 = Nothing Or IsDBNull(result1) Then
            result1 = ""

        End If

        Return result1
    End Function

    <WebMethod()> _
    Public Function UpdateCustAcctDet(ByVal constr As String, ByVal custcode As String, ByVal AccTelephone1 As String,
                    ByVal AccTelephone2 As String, ByVal AccMobile As String, ByVal AccFax As String, ByVal AccContact1 As String,
                     ByVal AccContact2 As String, ByVal AccEmail As String, ByVal Acc_ccEmail As String, ByVal ctrlacctcode As String,
                     ByVal AccCreditDays As String, ByVal AccCreditLimit As String, ByVal CashSup As Integer, ByVal AccBooking As Decimal,
                     ByVal PostCode As String, ByVal AccBooking2 As Integer, ByVal userlogged As String, ByVal remarks As String,
                     ByVal clientop As String, ByVal frmmode As String) As String

        'Dim retlist As New List(Of clsMaster)

        If Trim(ConfigurationManager.AppSettings("CustomerStopService")) <> "" Then
            UpdateCustAcctDet = Trim(ConfigurationManager.AppSettings("CustomerStopService"))
            Exit Function
        End If

        Dim ClientIP As String = Context.Request.ServerVariables("REMOTE_ADDR")


        If objUtils.ExecuteQueryReturnStringValuenew(constr, "select 1 from service_userpermissions  where userip='" & ClientIP & "' and active=1") <> "1" Then
            UpdateCustAcctDet = "Permission Denied"
            Exit Function
        End If
        Dim result_temp As String

        result_temp = objUtils.ExecuteQueryReturnSingleValuenew(constr, "delete from tempservicesave_agentmast")

        Dim result1 As Integer
        Dim sqlstr As String

        Dim spname As String
        Dim p As Integer
        Dim parms As New List(Of SqlParameter)
        Dim parm(19) As SqlParameter
        parm(19) = New SqlParameter
        parm(0) = New SqlParameter("@agentcode", CType(custcode, String))
        'parm(1) = New SqlParameter("@general", CType(custGeneral, String))

        parm(1) = New SqlParameter("@atel1", CType(AccTelephone1.Trim, String))
        parm(2) = New SqlParameter("@atel2", CType(AccTelephone2.Trim, String))
        parm(3) = New SqlParameter("@amobile", CType(AccMobile.Trim, String))
        parm(4) = New SqlParameter("@afax", CType(AccFax.Trim, String))
        parm(5) = New SqlParameter("@acontact1", CType(AccContact1.Trim, String))
        parm(6) = New SqlParameter("@acontact2", CType(AccContact2.Trim, String))
        parm(7) = New SqlParameter("@aemail", CType(AccEmail.Trim, String))
        parm(8) = New SqlParameter("@accemail", CType(Acc_ccEmail.Trim, String))

        parm(9) = New SqlParameter("@controlacctcode", CType(ctrlacctcode, String))
        parm(10) = New SqlParameter("@crdays", CType(AccCreditDays.Trim, Long))
        parm(11) = New SqlParameter("@crlimit", CType(AccCreditLimit.Trim, Long))
        parm(12) = New SqlParameter("@cashclient", CashSup)

        parm(13) = New SqlParameter("@bookingcrlimit", CType(AccBooking, Decimal))
        parm(14) = New SqlParameter("@postaccount", CType(PostCode.Trim, String))

        parm(15) = New SqlParameter("@bookcrlimitchk", AccBooking2)


        parm(16) = New SqlParameter("@remarks", CType(remarks, String))
        parm(17) = New SqlParameter("@openion", CType(clientop, String))
        parm(18) = New SqlParameter("@userlogged", CType(userlogged, String))

        If frmmode = 1 Then
            For p = 0 To 18
                parms.Add(parm(p))
            Next
            spname = "sp_updateacc_agentmast"
            result_temp = objUtils.ExecuteQueryReturnSingleValuenew(constr, "insert into  tempservicesave_agentmast select * from agentmast where agentcode='" & CType(custcode, String) & "'")

        End If

        If frmmode = 2 Then

            For p = 0 To 18
                parms.Add(parm(p))
            Next
            spname = "sp_updateacc_agentmast"
            result_temp = objUtils.ExecuteQueryReturnSingleValuenew(constr, "insert into  tempservicesave_agentmast select * from agentmast where agentcode='" & CType(custcode, String) & "'")

        End If
        If frmmode = 3 Then
            parms.Add(parm(0))
            spname = "sp_del_agentmast"
            result_temp = objUtils.ExecuteQueryReturnSingleValuenew(constr, "insert into  tempservicesave_agentmast select * from agentmast where agentcode='" & CType(custcode, String) & "'")
            result_temp = objUtils.ExecuteQueryReturnSingleValuenew(constr, "insert into  tempservicesaveagentmast_mulltiemail select * from agentmast_mulltiemail where agentcode='" & CType(custcode, String) & "'")
            result_temp = objUtils.ExecuteQueryReturnSingleValuenew(constr, "insert into  tempservicesave_agentmast_visit select * from agentmast_visit where agentcode='" & CType(custcode, String) & "'")
            result_temp = objUtils.ExecuteQueryReturnSingleValuenew(constr, "insert into  tempservicesave_agentmast_survey select * from agentmast_survey   where agentcode='" & CType(custcode, String) & "'")

        End If



        result1 = objUtils.ExecuteNonQuerynew(constr, spname, parms)
        If result1 = Nothing Or IsDBNull(result1) Then
            result1 = ""

        End If

        Return result1
    End Function


    <WebMethod()> _
    Public Function Reverse_UpdateCustAcctDet(ByVal constr As String, ByVal code As String, ByVal frmmode As String) As String





        If Trim(ConfigurationManager.AppSettings("CustomerStopService")) <> "" Then
            Reverse_UpdateCustAcctDet = Trim(ConfigurationManager.AppSettings("CustomerStopService"))
            Exit Function
        End If


        Dim ClientIP As String = Context.Request.ServerVariables("REMOTE_ADDR")


        If objUtils.ExecuteQueryReturnStringValuenew(constr, "select 1 from service_userpermissions  where userip='" & ClientIP & "' and active=1") <> "1" Then
            Reverse_UpdateCustAcctDet = "Permission Denied"
            Exit Function
        End If

        Dim result1 As Integer
        Dim spname As String


        Dim parms As New List(Of SqlParameter)
        Dim parm(19) As SqlParameter
        parm(19) = New SqlParameter
        parm(0) = New SqlParameter("@agentcode", CType(code, String))
        If frmmode > 1 Then
            Dim atel1, atel2, amobile, afax, acontact1, acontact2, aemail, accemail, controlacctcode, crdays, crlimit, cashclient, bookingcrlimit, postaccount, bookcrlimitchk, userlogged, remarks, openion As String


            Dim ds As New DataSet
            ds = objUtils.GetDataFromDatasetnew(constr, "select * from tempservicesave_agentmast where agentcode='" & CType(code, String) & "'")

            If ds.Tables(0).Rows.Count > 0 Then
                atel1 = ds.Tables(0).Rows(0).Item("atel1")
                atel2 = ds.Tables(0).Rows(0).Item("atel2")
                amobile = ds.Tables(0).Rows(0).Item("amobile")
                afax = ds.Tables(0).Rows(0).Item("afax")
                acontact1 = ds.Tables(0).Rows(0).Item("acontact1")
                acontact2 = ds.Tables(0).Rows(0).Item("acontact2")
                aemail = ds.Tables(0).Rows(0).Item("aemail")
                accemail = ds.Tables(0).Rows(0).Item("accemail")
                controlacctcode = ds.Tables(0).Rows(0).Item("controlacctcode")
                crdays = ds.Tables(0).Rows(0).Item("crdays")
                crlimit = ds.Tables(0).Rows(0).Item("crlimit")
                cashclient = ds.Tables(0).Rows(0).Item("cashclient")
                bookingcrlimit = ds.Tables(0).Rows(0).Item("bookingcrlimit")
                postaccount = ds.Tables(0).Rows(0).Item("postaccount")
                bookcrlimitchk = ds.Tables(0).Rows(0).Item("bookcrlimitchk")
                remarks = ds.Tables(0).Rows(0).Item("agentremarks")
                openion = ds.Tables(0).Rows(0).Item("openion")
                userlogged = ds.Tables(0).Rows(0).Item("moduser")
            End If
            parm(1) = New SqlParameter("@atel1", CType(atel1, String))
            parm(2) = New SqlParameter("@atel2", CType(atel2, String))
            parm(3) = New SqlParameter("@amobile", CType(amobile, String))
            parm(4) = New SqlParameter("@afax", CType(afax, String))
            parm(5) = New SqlParameter("@acontact1", CType(acontact1, String))
            parm(6) = New SqlParameter("@acontact2", CType(acontact2, String))
            parm(7) = New SqlParameter("@aemail", CType(aemail, String))
            parm(8) = New SqlParameter("@accemail", CType(accemail, String))
            parm(9) = New SqlParameter("@controlacctcode", CType(controlacctcode, String))
            parm(10) = New SqlParameter("@crdays", CType(crdays, String))
            parm(11) = New SqlParameter("@crlimit", CType(crlimit, String))
            parm(12) = New SqlParameter("@cashclient", CType(cashclient, String))
            parm(13) = New SqlParameter("@bookingcrlimit", CType(bookcrlimitchk, String))
            parm(14) = New SqlParameter("@postaccount", CType(postaccount, String))
            parm(15) = New SqlParameter("@bookcrlimitchk", CType(bookingcrlimit, String))
            parm(16) = New SqlParameter("@remarks", CType(remarks, String))
            parm(17) = New SqlParameter("@openion", CType(openion, String))
            parm(18) = New SqlParameter("@userlogged", CType(userlogged, String))

        End If
        If frmmode = 1 Then
            For p = 0 To 18
                parms.Add(parm(p))
            Next

            spname = "sp_updateacc_agentmast"
            'Dim result_temp As String
            'result_temp = objUtils.ExecuteQueryReturnSingleValuenew(constr, "update registration  set approve=null,approvedate=null,approveuser=null  where regno='" & CType(code, String) & "'")

        End If
        If frmmode = 2 Then
            For p = 0 To 18
                parms.Add(parm(p))
            Next

            spname = "sp_updateacc_agentmast"

        End If

        If frmmode = 3 Then
            Dim result_temp As String
            result1 = objUtils.ExecuteQueryReturnSingleValuenew(constr, "insert into  agentmast select * from tempservicesave_agentmast where agentcode='" & CType(code, String) & "'")
            result_temp = objUtils.ExecuteQueryReturnSingleValuenew(constr, "insert into agentmast_mulltiemail  select * from tempservicesaveagentmast_mulltiemail where agentcode='" & CType(code, String) & "'")
            result_temp = objUtils.ExecuteQueryReturnSingleValuenew(constr, "insert into  agentmast_visit select * from tempservicesave_agentmast_visit where agentcode='" & CType(code, String) & "'")
            result_temp = objUtils.ExecuteQueryReturnSingleValuenew(constr, "insert into  agentmast_survey  select * from tempservicesave_agentmast_survey   where agentcode='" & CType(code, String) & "'")

        End If

        If frmmode = 1 Or frmmode = 2 Then
            result1 = objUtils.ExecuteNonQuerynew(constr, spname, parms)

        End If

        If result1 = Nothing Or IsDBNull(result1) Then
            result1 = ""

        End If

        Return result1
    End Function
    <WebMethod()> _
    Public Function UpdateWebApprvl(ByVal constr As String, ByVal custcode As String, ByVal WebAppUsername As String,
                    ByVal strpwd As String, ByVal WebAppContact As String, ByVal WebAppEmail As String, ByVal WebApprove As Integer,
                    ByVal mode As Integer, ByVal userlogged As String, ByVal frmmode As String) As String

        Dim retlist As New List(Of clsMaster)

        If Trim(ConfigurationManager.AppSettings("CustomerStopService")) <> "" Then
            UpdateWebApprvl = Trim(ConfigurationManager.AppSettings("CustomerStopService"))
            Exit Function
        End If

        Dim ClientIP As String = Context.Request.ServerVariables("REMOTE_ADDR")


        If objUtils.ExecuteQueryReturnStringValuenew(constr, "select 1 from service_userpermissions  where userip='" & ClientIP & "' and active=1") <> "1" Then
            UpdateWebApprvl = "Permission Denied"
            Exit Function
        End If
        Dim result_temp As String

        result_temp = objUtils.ExecuteQueryReturnSingleValuenew(constr, "delete from tempservicesave_agentmast")

        Dim result1 As Integer
        Dim sqlstr As String

        Dim spname As String
        Dim p As Integer
        Dim parms As New List(Of SqlParameter)
        Dim parm(8) As SqlParameter
        parm(8) = New SqlParameter
        parm(0) = New SqlParameter("@agentcode", CType(custcode, String))
        parm(1) = New SqlParameter("@webusername", CType(WebAppUsername, String))

        parm(2) = New SqlParameter("@webpassword", CType(strpwd.Trim, String))
        parm(3) = New SqlParameter("@webcontact", CType(WebAppContact.Trim, String))
        parm(4) = New SqlParameter("@webemail", CType(WebAppEmail.Trim, String))
        parm(5) = New SqlParameter("@webapprove", CType(WebApprove, Integer))
        parm(6) = New SqlParameter("@userlogged", CType(userlogged, String))
        parm(7) = New SqlParameter("@mode", CType(mode, Integer))

        If frmmode = 1 Then
            For p = 0 To 7
                parms.Add(parm(p))
            Next
            spname = "sp_updateweb_agentmast"
            result_temp = objUtils.ExecuteQueryReturnSingleValuenew(constr, "insert into  tempservicesave_agentmast select * from agentmast where agentcode='" & CType(custcode, String) & "'")

        End If

        If frmmode = 2 Then

            For p = 0 To 7
                parms.Add(parm(p))
            Next
            spname = "sp_updateweb_agentmast"
            result_temp = objUtils.ExecuteQueryReturnSingleValuenew(constr, "insert into  tempservicesave_agentmast select * from agentmast where agentcode='" & CType(custcode, String) & "'")

        End If
        If frmmode = 3 Then
            parms.Add(parm(0))
            spname = "sp_del_agentmast"
            result_temp = objUtils.ExecuteQueryReturnSingleValuenew(constr, "insert into  tempservicesave_agentmast select * from agentmast where agentcode='" & CType(custcode, String) & "'")
            result_temp = objUtils.ExecuteQueryReturnSingleValuenew(constr, "insert into  tempservicesaveagentmast_mulltiemail select * from agentmast_mulltiemail where agentcode='" & CType(custcode, String) & "'")
            result_temp = objUtils.ExecuteQueryReturnSingleValuenew(constr, "insert into  tempservicesave_agentmast_visit select * from agentmast_visit where agentcode='" & CType(custcode, String) & "'")
            result_temp = objUtils.ExecuteQueryReturnSingleValuenew(constr, "insert into  tempservicesave_agentmast_survey select * from agentmast_survey   where agentcode='" & CType(custcode, String) & "'")

        End If



        If frmmode = 1 Or frmmode = 2 Then
            result1 = objUtils.ExecuteNonQuerynew(constr, spname, parms)

        End If
        If result1 = Nothing Or IsDBNull(result1) Then
            result1 = ""

        End If

        Return result1
    End Function

    <WebMethod()> _
    Public Function Reverse_UpdateWebApprvl(ByVal constr As String, ByVal code As String, ByVal mode As String, ByVal frmmode As String) As String





        If Trim(ConfigurationManager.AppSettings("CustomerStopService")) <> "" Then
            Reverse_UpdateWebApprvl = Trim(ConfigurationManager.AppSettings("CustomerStopService"))
            Exit Function
        End If


        Dim ClientIP As String = Context.Request.ServerVariables("REMOTE_ADDR")


        If objUtils.ExecuteQueryReturnStringValuenew(constr, "select 1 from service_userpermissions  where userip='" & ClientIP & "' and active=1") <> "1" Then
            Reverse_UpdateWebApprvl = "Permission Denied"
            Exit Function
        End If

        Dim result1 As Integer
        Dim spname As String


        Dim parms As New List(Of SqlParameter)
        Dim parm(8) As SqlParameter
        parm(8) = New SqlParameter
        parm(0) = New SqlParameter("@agentcode", CType(code, String))
        If frmmode > 1 Then
            Dim webapprove, webusername, webpassword, webcontact, webemail, userlogged As String


            Dim ds As New DataSet
            ds = objUtils.GetDataFromDatasetnew(constr, "select * from tempservicesave_agentmast where agentcode='" & CType(code, String) & "'")

            If ds.Tables(0).Rows.Count > 0 Then
                webapprove = ds.Tables(0).Rows(0).Item("webapprove")
                webusername = ds.Tables(0).Rows(0).Item("webusername")
                webpassword = ds.Tables(0).Rows(0).Item("webpassword")
                webcontact = ds.Tables(0).Rows(0).Item("webcontact")
                webemail = ds.Tables(0).Rows(0).Item("webemail")
                userlogged = ds.Tables(0).Rows(0).Item("moduser")
            End If
            parm(1) = New SqlParameter("@webapprove", CType(webapprove, String))
            parm(2) = New SqlParameter("@webusername", CType(webusername, String))
            parm(3) = New SqlParameter("@webpassword", CType(webpassword, String))
            parm(4) = New SqlParameter("@webcontact", CType(webcontact, String))
            parm(5) = New SqlParameter("@webemail", CType(webemail, String))
            parm(6) = New SqlParameter("@mode", CType(mode, Integer))
            parm(7) = New SqlParameter("@userlogged", CType(userlogged, String))

        End If
        If frmmode = 1 Then
            For p = 0 To 7
                parms.Add(parm(p))
            Next

            spname = "sp_updateweb_agentmast"
            'Dim result_temp As String
            'result_temp = objUtils.ExecuteQueryReturnSingleValuenew(constr, "update registration  set approve=null,approvedate=null,approveuser=null  where regno='" & CType(code, String) & "'")

        End If
        If frmmode = 2 Then
            For p = 0 To 7
                parms.Add(parm(p))
            Next

            spname = "sp_updateweb_agentmast"

        End If

        If frmmode = 3 Then
            Dim result_temp As String
            result1 = objUtils.ExecuteQueryReturnSingleValuenew(constr, "insert into  agentmast select * from tempservicesave_agentmast where agentcode='" & CType(code, String) & "'")
            result_temp = objUtils.ExecuteQueryReturnSingleValuenew(constr, "insert into agentmast_mulltiemail  select * from tempservicesaveagentmast_mulltiemail where agentcode='" & CType(code, String) & "'")
            result_temp = objUtils.ExecuteQueryReturnSingleValuenew(constr, "insert into  agentmast_visit select * from tempservicesave_agentmast_visit where agentcode='" & CType(code, String) & "'")
            result_temp = objUtils.ExecuteQueryReturnSingleValuenew(constr, "insert into  agentmast_survey  select * from tempservicesave_agentmast_survey   where agentcode='" & CType(code, String) & "'")

        End If

        If frmmode = 1 Or frmmode = 2 Then
            result1 = objUtils.ExecuteNonQuerynew(constr, spname, parms)

        End If


        If result1 = Nothing Or IsDBNull(result1) Then
            result1 = ""

        End If

        Return result1
    End Function



End Class