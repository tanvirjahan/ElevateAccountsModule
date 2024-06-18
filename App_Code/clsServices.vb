Imports System.Web
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.Web.Script.Services
Imports System.Collections
Imports System.Collections.Generic
Imports System.Data.SqlClient
Imports System.Data
Imports System.Text
Imports MyClasses
Imports System.Xml.Serialization
Imports System.IO

Namespace ColServices

    <ScriptService()> _
    <WebService(Namespace:="http://tempuri.org/")> _
    <WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Public Class clsServices
        Inherits System.Web.Services.WebService

        Dim objUtils As New clsUtils
        Dim objDate As New clsDateTime
        Dim ClsGuestInfo As New List(Of clsAgentsOnline.BookNow.GuestInfo)
        Dim ClsAdultChildInfo As New List(Of clsAgentsOnline.BookNow.GuestInfo.AdultChildlts)

        <WebMethod()> _
        Public Function cleargroupings(ByVal constr As String, ByVal requestid As String) As String
            Dim sqlTrans As SqlTransaction
            Dim SqlConn As SqlConnection
            Dim myCommand As SqlCommand
            Try

                cleargroupings = "False"

                SqlConn = clsDBConnect.dbConnectionnew(constr)
                sqlTrans = SqlConn.BeginTransaction

                myCommand = New SqlCommand("sp_delete_reservation_group_FITconfirmation_tmp", SqlConn, sqlTrans)
                myCommand.CommandType = CommandType.StoredProcedure
                myCommand.Parameters.Add(New SqlParameter("@requestid", SqlDbType.VarChar, 20)).Value = CType(requestid, String)
                myCommand.ExecuteNonQuery()


                myCommand = New SqlCommand("sp_del_reservation_groupdetails_temp", SqlConn, sqlTrans)
                myCommand.CommandType = CommandType.StoredProcedure
                myCommand.Parameters.Add(New SqlParameter("@requestid", SqlDbType.VarChar, 20)).Value = CType(requestid, String)
                myCommand.ExecuteNonQuery()


                myCommand = New SqlCommand("sp_delete_reservation_group_priceterms_temp", SqlConn, sqlTrans)
                myCommand.CommandType = CommandType.StoredProcedure
                myCommand.Parameters.Add(New SqlParameter("@requestid", SqlDbType.VarChar, 20)).Value = CType(requestid, String)
                myCommand.ExecuteNonQuery()

                sqlTrans.Commit()
                clsDBConnect.dbSqlTransation(sqlTrans)
                clsDBConnect.dbCommandClose(myCommand)
                clsDBConnect.dbConnectionClose(SqlConn)

            Catch ex As Exception
                If SqlConn.State = ConnectionState.Open Then
                    sqlTrans.Rollback()

                End If

            End Try
            Return "true"
        End Function
        <WebMethod()> _
        Public Function GetAcctcurr(ByVal constr As String, ByVal acctcode As String) As String
            Dim result As String
            Dim sqlstr As String
            sqlstr = "select currcode from acctmast where acctcode='" & Trim(acctcode) & "'"
            result = objUtils.ExecuteQueryReturnStringValuenew(constr, sqlstr)

            Return result
        End Function

        <WebMethod()> _
        Public Function GetFlightcode(ByVal constr As String, ByVal requestid As String) As String
            Dim result As String
            Dim sqlstr As String
            sqlstr = "select arrflightno from reservation_guestnewtemp where requestid='" & Trim(requestid) & "' and glineno= (select min(glineno) from reservation_guestnewtemp where requestid='" & Trim(requestid) & "')"
            result = objUtils.ExecuteQueryReturnStringValuenew(constr, sqlstr)

            Return result
        End Function

        <WebMethod()> _
        Public Function fillitenary(ByVal constr As String, ByVal othtyp As String) As String
            'Dim retlist As New List(Of clsMaster)
            Dim result As String
            Dim sqlstr As String
            sqlstr = "select remarks from othtypmast where othtypcode='" & othtyp & "'"

            result = objUtils.ExecuteQueryReturnStringValuenew(constr, sqlstr)
            If result = Nothing Or IsDBNull(result) Then
                result = ""

            End If



            Return result
        End Function
        <WebMethod()> _
        Public Function Fillcombination(ByVal constr As String, ByVal maxadults As String, ByVal maxchild As String, ByVal maxocc As String, ByVal startbase As String, ByVal partycode As String, ByVal rmtypcode As String) As String

            Dim result As String
            Dim sqlstr As String
            sqlstr = "select dbo.fn_max_occupancylist (" & Val(maxadults) & "," & Val(maxchild) & "," & Val(maxocc) & "," & Val(startbase) & ",'" & partycode & "','" & rmtypcode & "')"

            result = objUtils.ExecuteQueryReturnStringValuenew(constr, sqlstr)
            If result = Nothing Or IsDBNull(result) Then
                result = ""

            End If



            Return result
        End Function

        <WebMethod()> _
       Public Function GetActivePromotions(ByVal constr As String, ByVal codeid As String, ByVal mktcode As String) As clsMaster()
            Dim retlist As New List(Of clsMaster)
            Dim sqlstr As String
            ''for approval promotion_header replace to Edit_promotion_header
            sqlstr = "select pricecode,promotionid from vw_promotion_header where promotionid in (select promotionid from vw_promotionmarket_detail where marketcode='" & mktcode & "') "

            'If Trim(codeid) <> "" And Trim(UCase(codeid)) <> Trim(UCase("[Select]")) Then
            sqlstr = sqlstr + " and isnull(pricecode,'')<>'' and ISNULL(active,0)=1 and partycode='" & codeid & "'"
            'End If
            sqlstr = sqlstr + " order by promotionid"

            retlist = objUtils.FillclsMasternew(constr, sqlstr, True)
            Return retlist.ToArray()
        End Function



        <WebMethod()> _
               Public Function GetActivePromotionsnew(ByVal constr As String, ByVal codeid As String, ByVal mktcode As String) As clsMaster()
            Dim retlist As New List(Of clsMaster)
            Dim sqlstr As String
            sqlstr = "select distinct pricecode,promotionid from vw_promotion_header where  isnull(pricecode,'')<>'' "

            'If Trim(codeid) <> "" And Trim(UCase(codeid)) <> Trim(UCase("[Select]")) Then
            sqlstr = sqlstr + "   and partycode='" & codeid & "' and ISNULL(active,0)=1 "
            'End If
            If mktcode <> "" Then
                sqlstr = sqlstr + " and promotionid in (select promotionid from vw_promotionmarket_detail where marketcode  in (" & mktcode & "))"
            End If

            sqlstr = sqlstr + " order by promotionid"

            retlist = objUtils.FillclsMasternew(constr, sqlstr, True)
            Return retlist.ToArray()
        End Function
        <WebMethod()> _
        Public Function FillPickups(ByVal constr As String, ByVal othtyp As String) As String
            'Dim retlist As New List(Of clsMaster)
            Dim result As String
            Dim sqlstr As String
            sqlstr = "select code,name from view_getpickups where ptype='" & othtyp & "'"

            result = objUtils.ExecuteQueryReturnStringValuenew(constr, sqlstr)
            If result = Nothing Or IsDBNull(result) Then
                result = ""

            End If

            Return result

        End Function

'changed by mohamed on 19/04/2016
        <WebMethod(EnableSession:=True)> _
        Public Function GetDestinationByCtryOnlineCombined(ByVal asCtryCode As String) As clsMaster()
            Dim retlist As New List(Of clsMaster)
            Dim sqlstr As String
            sqlstr = "select destname, desttype+destcode destcode from view_destination_search c where basecountry=(select option_selected from reservation_parameters where param_id=459)"
            If Trim(asCtryCode) <> "" And Trim(UCase(asCtryCode)) <> Trim(UCase("[Select]")) Then
                sqlstr = sqlstr + " and c.ctrycode='" & Trim(asCtryCode) & "'"
            End If
            sqlstr = sqlstr + " order by c.destname"
            retlist = objUtils.FillclsMasternew(Session("dbConnectionName"), sqlstr, True)
            Return retlist.ToArray()
        End Function

        ''changed by mohamed on 19/04/2016
        '<WebMethod(EnableSession:=True)> _
        'Public Function GetDestinationByCtryOnline(ByVal asCtryCode As String) As ClassCityArea()
        '    Dim retlist As New List(Of ClassCityArea)
        '    Dim sqlstr As String
        '    sqlstr = "select destname, destcode, desttype from view_destination_search where ctrycode='" & asCtryCode & "' and basecountry=(select option_selected from reservation_parameters where param_id=459) order by destname"
        '    If Trim(asCtryCode) <> "" And Trim(UCase(asCtryCode)) <> Trim(UCase("[Select]")) Then
        '        sqlstr = sqlstr + " and a.citycode='" & Trim(asCtryCode) & "'"
        '    End If
        '    sqlstr = sqlstr + " order by a.areaname"
        '    retlist = objUtils.FillClassCityAreaOnline(Session("dbConnectionName"), sqlstr, True)
        '    Return retlist.ToArray()
        'End Function

        'changed by mohamed on 19/04/2016
        <WebMethod(EnableSession:=True)> _
        Public Function fnGetCityCodeByDestinationCombination(ByVal lsDestinationCode As String) As String()
            'Dim retlist As New List(Of ClassCityArea)
            'Dim lclscity As New ClassCityArea
            'Dim ds As DataSet
            'ds = objUtils.ExecuteQuerySqlnew(Session("dbConnectionName"), "select * from view_destination_search where desttype+destcode='" & lsDestinationCode & "'")
            'If ds.Tables.Count > 0 Then
            '    If ds.Tables(0).Rows.Count > 0 Then
            '        lclscity.ListCity = ds.Tables(0).Rows(0)("citycode")
            '        lclscity.ListText = ds.Tables(0).Rows(0)("destcode")
            '        lclscity.ListType = ds.Tables(0).Rows(0)("desttype")
            '        retlist.Add(lclscity)
            '    End If
            'End If

            Dim mStr() As String = objUtils.FillArraynew(Session("dbConnectionName"), "select citycode, destcode, desttype, destname, cityname, ctrycode, ctryname, basecountry from view_destination_search where desttype+destcode='" & lsDestinationCode & "'", 8)
            Return mStr

            'Return retlist.ToArray()
        End Function

        'changed by mohamed on 19/04/2016
        <WebMethod(EnableSession:=True)> _
        Public Function fnGetSearchPageStatus() As String()
            Dim mStr(0) As String
            mStr(0) = ""
            If Session("page") IsNot Nothing Then
                If Session("page") = "login" Then
                    mStr(0) = Session("page")
                End If
            End If
            Return mStr
        End Function

        <WebMethod()> _
        Public Function GetCityByPartyName(ByVal constr As String, ByVal partyname As String) As String
            Dim retlist As String = String.Empty
            Dim sqlstr As String
            sqlstr = "select citycode from partymast where showinweb =1 and active=1 and partycode ='" & partyname & "'"
            retlist = objUtils.GetString(constr, sqlstr)
            Return retlist
        End Function

        <WebMethod()> _
        Public Function fillShuttleItinarary(ByVal constr As String, ByVal vehicletype As String) As String
            'Dim retlist As New List(Of clsMaster)
            Dim result As String
            Dim sqlstr As String
            Dim shutlval() As String
            sqlstr = "select options_available  from reservation_parameters where param_id =1082" 'getting list of shutl trfs

            result = objUtils.ExecuteQueryReturnStringValuenew(constr, sqlstr)
            If result = Nothing Or IsDBNull(result) Then
                result = ""
            Else
                shutlval = result.Split("/")
                result = ""
            End If
            If shutlval IsNot Nothing Then
                For i As Integer = 0 To shutlval.Length - 1
                    If vehicletype = shutlval(i).ToString Then
                        sqlstr = "select remarks  from othcatmast where othgrpcode ='TRFS' and othcatcode ='" & vehicletype & "'"
                        result = objUtils.ExecuteQueryReturnStringValuenew(constr, sqlstr)
                        If result = Nothing Or IsDBNull(result) Then
                            result = ""
                        End If
                        Exit For
                    End If
                Next
            End If


            Return result
        End Function
        <WebMethod()> _
       Public Function GetMktCodeListnew(ByVal constr As String, ByVal agentcode As String) As clsMaster()
            Dim retlist As New List(Of clsMaster)
            Dim sqlstr As String
            sqlstr = "select distinct p.plgrpcode,p.plgrpname from plgrpmast p left join agentmast a on a.plgrpcode=p.plgrpcode  where p.active=1 "
            If Trim(agentcode) <> "" And Trim(UCase(agentcode)) <> Trim(UCase("[Select]")) Then
                sqlstr = sqlstr + " and agentcode='" & Trim(agentcode) & "'"
            End If
            sqlstr = sqlstr + " order by plgrpcode"
            retlist = objUtils.FillclsMasternew(constr, sqlstr, True)
            Return retlist.ToArray()
        End Function

        <WebMethod()> _
              Public Function GetMktNameListnew(ByVal constr As String, ByVal agentcode As String) As clsMaster()
            Dim retlist As New List(Of clsMaster)
            Dim sqlstr As String
            sqlstr = "select distinct p.plgrpname,p.plgrpcode from plgrpmast p left join agentmast a on a.plgrpcode=p.plgrpcode  where p.active=1 "
            If Trim(agentcode) <> "" And Trim(UCase(agentcode)) <> Trim(UCase("[Select]")) Then
                sqlstr = sqlstr + " and agentcode='" & Trim(agentcode) & "'"
            End If
            sqlstr = sqlstr + " order by plgrpname"
            retlist = objUtils.FillclsMasternew(constr, sqlstr, True)
            Return retlist.ToArray()
        End Function

        <WebMethod()> _
              Public Function GetSellingTypeCodeListnew(ByVal constr As String, ByVal agentcode As String) As clsMaster()
            Dim retlist As New List(Of clsMaster)
            Dim sqlstr As String
            sqlstr = "select distinct s.sellcode,s.sellname from sellmast s Inner join agentmast a on a.sellcode=s.sellcode where a.active=1 "
            If Trim(agentcode) <> "" And Trim(UCase(agentcode)) <> Trim(UCase("[Select]")) Then
                sqlstr = sqlstr + " and agentcode='" & Trim(agentcode) & "'"
            End If
            sqlstr = sqlstr + " order by sellcode"
            retlist = objUtils.FillclsMasternew(constr, sqlstr, True)
            Return retlist.ToArray()
        End Function


        <WebMethod()> _
             Public Function GetSellingTypeNameListnew(ByVal constr As String, ByVal agentcode As String) As clsMaster()
            Dim retlist As New List(Of clsMaster)
            Dim sqlstr As String
            sqlstr = "select distinct s.sellname,s.sellcode from sellmast s Inner join agentmast a on a.sellcode=s.sellcode where a.active=1 "
            If Trim(agentcode) <> "" And Trim(UCase(agentcode)) <> Trim(UCase("[Select]")) Then
                sqlstr = sqlstr + " and agentcode='" & Trim(agentcode) & "'"
            End If
            sqlstr = sqlstr + " order by sellname"
            retlist = objUtils.FillclsMasternew(constr, sqlstr, True)
            Return retlist.ToArray()
        End Function



        <WebMethod()> _
        Public Function GetToDate(ByVal constr As String, ByVal fromdt As String, ByVal units As String) As String
            'Dim retlist As New List(Of clsMaster)
            Dim result As String
            Dim sqlstr As New StringBuilder
            Dim fromdttemp As Date = fromdt

            Dim todatetemp As Date

            fromdt = Format(fromdttemp, "yyyy/MM/dd")
            'todate = Format(todatetemp, "yyyy/MM/dd")

            sqlstr.AppendFormat("select DATEADD (d,{0},'{1}')", units, fromdt)


            result = objUtils.ExecuteQueryReturnStringValuenew(constr, sqlstr.ToString)
            If result = Nothing Or IsDBNull(result) Then
                result = ""

            End If
            todatetemp = result
            result = Format(todatetemp, "dd/MM/yyyy")


            Return result
        End Function

        <WebMethod()> _
        Public Function GetDateDiff(ByVal constr As String, ByVal fromdt As String, ByVal todate As String) As String
            'Dim retlist As New List(Of clsMaster)
            Dim result As String
            Dim sqlstr As New StringBuilder
            Dim fromdttemp As Date = fromdt
            Dim todatetemp As Date = todate


            fromdt = Format(fromdttemp, "yyyy/MM/dd")
            todate = Format(todatetemp, "yyyy/MM/dd")

            sqlstr.AppendFormat("select DATEDIFF (d,'{0}','{1}')", fromdt, todate)


            result = objUtils.ExecuteQueryReturnStringValuenew(constr, sqlstr.ToString)
            If result = Nothing Or IsDBNull(result) Then
                result = ""

            End If

            Return result
        End Function

        <WebMethod()> _
       Public Function GetCategoryLocationByHotel(ByVal constr As String, ByVal partycode As String) As String()
            Dim retlist As New List(Of clsMaster)
            Dim sqlstr As String
            sqlstr = "SELECT cm.citycode,cm.cityname,ctm.catcode,ctm.catname FROM partymast pm INNER JOIN citymast cm ON pm.citycode = cm.citycode INNER JOIN catmast ctm ON ctm.catcode = pm.catcode where showinweb =1 and pm.active=1 and partycode='" & partycode & "'"

            'retlist = objUtils.FillclsMasternew(constr, sqlstr, True)
            'Return retlist.ToArray()
            Dim mStr() As String = objUtils.FillArraynew(constr, sqlstr, 4)
            Return mStr
        End Function

        <WebMethod(EnableSession:=True)> _
         Public Function AccountsAutoComplete(ByVal para1 As String, ByVal para2 As String) As List(Of AutocompleteClass)
            Try
                Dim CustomerAutoCompleteClass As New List(Of AutocompleteClass)
                Dim CustomerAutoCompleteClassAdd As New AutocompleteClass
                Dim strSql As String = "select top 10 des, Code from view_account where  isnull(des,'')<>'' order by des"


                If (para1 <> "[Select]") Then

                    strSql = "select des,Code from view_account where type = '" & para1 & "'  and   isnull(des,'')<>'' order by des"

                End If


                'If para1 <> "" Then
                '    strSql = strSql + " and  plgrpcode='" & para1 & "'"
                'End If
                'If para2 <> "" Then
                '    strSql = strSql + " and  sellcode='" & para2 & "'"
                'End If
                'strSql = strSql + " order by Agentname "

                Dim MYCommand As New SqlCommand(strSql, clsDBConnect.dbConnectionnew(Session("dbconnectionName").ToString()))

                Dim Dr As SqlDataReader
                Dr = MYCommand.ExecuteReader
                While Dr.Read
                    If Not IsDBNull("code") Then
                        CustomerAutoCompleteClassAdd = New AutocompleteClass
                        CustomerAutoCompleteClassAdd.Id = Dr("Code")
                        CustomerAutoCompleteClassAdd.Name = Dr("des")
                        If para2 = "1" Then
                            CustomerAutoCompleteClassAdd.IsCode = "1"
                        Else
                            CustomerAutoCompleteClassAdd.IsCode = "0"
                        End If
                        CustomerAutoCompleteClass.Add(CustomerAutoCompleteClassAdd)
                    End If
                End While
                Dr.Close()
                MYCommand.Cancel()
                Return CustomerAutoCompleteClass
                'Dim retlist(2) As String
                'Dim min As String
                'Dim max As String

                'min = objUtils.ExecuteQueryReturnStringValuenew(constr, "select minpax from othcatmast where othgrpcode='TRFS' and othcatcode='" & vehicletype & "'")
                'max = objUtils.ExecuteQueryReturnStringValuenew(constr, "select maxpax from othcatmast where othgrpcode='TRFS' and othcatcode='" & vehicletype & "'")

                'retlist(0) = min
                'retlist(1) = max


                'Return retlist
            Catch ex As Exception

            End Try
        End Function
        <WebMethod(EnableSession:=True)> _
        Public Function AccountsAutoCompletenew(ByVal para1 As String, ByVal para3 As String, ByVal para2 As String) As List(Of AutocompleteClass)
            Try
                Dim CustomerAutoCompleteClass As New List(Of AutocompleteClass)
                Dim CustomerAutoCompleteClassAdd As New AutocompleteClass
                Dim strSql As String = "select top 10 des, Code from view_account where div_code='" & para3 & "' and  isnull(des,'')<>'' order by des"


                If (para1 <> "[Select]") Then

                    strSql = "select des,Code from view_account where div_code='" & para3 & "' and type = '" & para1 & "'  and   isnull(des,'')<>'' order by des"

                End If


                'If para1 <> "" Then
                '    strSql = strSql + " and  plgrpcode='" & para1 & "'"
                'End If
                'If para2 <> "" Then
                '    strSql = strSql + " and  sellcode='" & para2 & "'"
                'End If
                'strSql = strSql + " order by Agentname "

                Dim MYCommand As New SqlCommand(strSql, clsDBConnect.dbConnectionnew(Session("dbconnectionName").ToString()))

                Dim Dr As SqlDataReader
                Dr = MYCommand.ExecuteReader
                While Dr.Read
                    If Not IsDBNull("code") Then
                        CustomerAutoCompleteClassAdd = New AutocompleteClass
                        CustomerAutoCompleteClassAdd.Id = Dr("Code")
                        CustomerAutoCompleteClassAdd.Name = Dr("des")
                        If para2 = "1" Then
                            CustomerAutoCompleteClassAdd.IsCode = "1"
                        Else
                            CustomerAutoCompleteClassAdd.IsCode = "0"
                        End If
                        CustomerAutoCompleteClass.Add(CustomerAutoCompleteClassAdd)
                    End If
                End While
                Dr.Close()
                MYCommand.Cancel()
                Return CustomerAutoCompleteClass
                'Dim retlist(2) As String
                'Dim min As String
                'Dim max As String

                'min = objUtils.ExecuteQueryReturnStringValuenew(constr, "select minpax from othcatmast where othgrpcode='TRFS' and othcatcode='" & vehicletype & "'")
                'max = objUtils.ExecuteQueryReturnStringValuenew(constr, "select maxpax from othcatmast where othgrpcode='TRFS' and othcatcode='" & vehicletype & "'")

                'retlist(0) = min
                'retlist(1) = max


                'Return retlist
            Catch ex As Exception

            End Try
        End Function


        <WebMethod(EnableSession:=True)> _
        Public Function AccountsallAutoComplete(ByVal para1 As String, ByVal para2 As String) As List(Of AutocompleteClass)
            Try
                Dim CustomerAutoCompleteClass As New List(Of AutocompleteClass)
                Dim CustomerAutoCompleteClassAdd As New AutocompleteClass
                Dim strSql As String = "select acctname,acctcode from  acctmast order by acctname"


                ''If (para1 <> "[Select]") Then

                ''    strSql = "select des,Code from view_account where type = '" & para1 & "' order by des"


                ''End If
                'If para1 <> "" Then
                '    strSql = strSql + " and  plgrpcode='" & para1 & "'"
                'End If
                'If para2 <> "" Then
                '    strSql = strSql + " and  sellcode='" & para2 & "'"
                'End If
                'strSql = strSql + " order by Agentname "

                Dim MYCommand As New SqlCommand(strSql, clsDBConnect.dbConnectionnew(Session("dbconnectionName").ToString()))

                Dim Dr As SqlDataReader
                Dr = MYCommand.ExecuteReader
                While Dr.Read
                    If Not IsDBNull("acctcode") Then
                        CustomerAutoCompleteClassAdd = New AutocompleteClass
                        CustomerAutoCompleteClassAdd.Id = Dr("acctcode")
                        CustomerAutoCompleteClassAdd.Name = Dr("acctname")
                        CustomerAutoCompleteClass.Add(CustomerAutoCompleteClassAdd)
                    End If
                End While
                Dr.Close()
                MYCommand.Cancel()
                Return CustomerAutoCompleteClass
                'Dim retlist(2) As String
                'Dim min As String
                'Dim max As String

                'min = objUtils.ExecuteQueryReturnStringValuenew(constr, "select minpax from othcatmast where othgrpcode='TRFS' and othcatcode='" & vehicletype & "'")
                'max = objUtils.ExecuteQueryReturnStringValuenew(constr, "select maxpax from othcatmast where othgrpcode='TRFS' and othcatcode='" & vehicletype & "'")

                'retlist(0) = min
                'retlist(1) = max


                'Return retlist
            Catch ex As Exception

            End Try
        End Function




        <WebMethod(EnableSession:=True)> _
        Public Function AccountsdebitnoteAutoComplete(ByVal para1 As String, ByVal para2 As String) As List(Of AutocompleteClass)
            Try
                Dim CustomerAutoCompleteClass As New List(Of AutocompleteClass)
                Dim CustomerAutoCompleteClassAdd As New AutocompleteClass
                Dim strSql As String = "select acctcode, acctname from acctmast where div_code='" & para1 & "' and controlyn='N' and bankyn='N' order by acctname "


                ''If (para1 <> "[Select]") Then

                ''    strSql = "select des,Code from view_account where type = '" & para1 & "' order by des"


                ''End If
                'If para1 <> "" Then
                '    strSql = strSql + " and  plgrpcode='" & para1 & "'"
                'End If
                'If para2 <> "" Then
                '    strSql = strSql + " and  sellcode='" & para2 & "'"
                'End If
                'strSql = strSql + " order by Agentname "

                Dim MYCommand As New SqlCommand(strSql, clsDBConnect.dbConnectionnew(Session("dbconnectionName").ToString()))

                Dim Dr As SqlDataReader
                Dr = MYCommand.ExecuteReader
                While Dr.Read
                    If Not IsDBNull("acctcode") Then
                        CustomerAutoCompleteClassAdd = New AutocompleteClass
                        CustomerAutoCompleteClassAdd.Id = Dr("acctcode")
                        CustomerAutoCompleteClassAdd.Name = Dr("acctname")
                        CustomerAutoCompleteClass.Add(CustomerAutoCompleteClassAdd)
                    End If
                End While
                Dr.Close()
                MYCommand.Cancel()
                Return CustomerAutoCompleteClass
                'Dim retlist(2) As String
                'Dim min As String
                'Dim max As String

                'min = objUtils.ExecuteQueryReturnStringValuenew(constr, "select minpax from othcatmast where othgrpcode='TRFS' and othcatcode='" & vehicletype & "'")
                'max = objUtils.ExecuteQueryReturnStringValuenew(constr, "select maxpax from othcatmast where othgrpcode='TRFS' and othcatcode='" & vehicletype & "'")

                'retlist(0) = min
                'retlist(1) = max


                'Return retlist
            Catch ex As Exception

            End Try
        End Function
        <WebMethod(EnableSession:=True)> _
        Public Function GetQuotPartydetails(ByVal strCityCode As String, ByVal strchoice As String, ByVal sptypecode As String) As List(Of AutocompleteClass)
            'Dim retlist As New List(Of clsMaster)
            Dim CustomerAutoCompleteClass As New List(Of AutocompleteClass)
            Dim CustomerAutoCompleteClassAdd As New AutocompleteClass
            Try
                Dim result As String
                Dim sqlstr As String
                sqlstr = "select partycode,partyname  from PartyMast where active=1 and sptypecode='" & sptypecode & "'"

                If Trim(strCityCode) <> "" Then
                    sqlstr = sqlstr + " and  citycode='" & Trim(strCityCode) & "'"
                End If
                If Trim(strchoice) <> "" Then
                    sqlstr = sqlstr + " and  catcode='" & Trim(strchoice) & "'"
                End If
                sqlstr = sqlstr

                ''+ " order by citycode"

                Dim MYCommand As New SqlCommand(sqlstr, clsDBConnect.dbConnectionnew(Session("dbconnectionName").ToString()))

                Dim Dr As SqlDataReader
                Dr = MYCommand.ExecuteReader
                While Dr.Read
                    If Not IsDBNull("partycode") Then
                        CustomerAutoCompleteClassAdd = New AutocompleteClass
                        CustomerAutoCompleteClassAdd.Id = Dr("partycode")
                        CustomerAutoCompleteClassAdd.Name = Dr("partyname")

                        CustomerAutoCompleteClass.Add(CustomerAutoCompleteClassAdd)
                    End If
                End While
                Dr.Close()
                MYCommand.Cancel()
                Return CustomerAutoCompleteClass
                'Dim retlist(2) As String
                'Dim min As String
                'Dim max As String

                'min = objUtils.ExecuteQueryReturnStringValuenew(constr, "select minpax from othcatmast where othgrpcode='TRFS' and othcatcode='" & vehicletype & "'")
                'max = objUtils.ExecuteQueryReturnStringValuenew(constr, "select maxpax from othcatmast where othgrpcode='TRFS' and othcatcode='" & vehicletype & "'")

                'retlist(0) = min
                'retlist(1) = max


                'Return retlist
            Catch ex As Exception

            End Try


        End Function
        <WebMethod(EnableSession:=True)> _
        Public Function CustomerAutoComplete(ByVal para1 As String, ByVal para2 As String) As List(Of AutocompleteClass)
            Try
                Dim CustomerAutoCompleteClass As New List(Of AutocompleteClass)
                Dim CustomerAutoCompleteClassAdd As New AutocompleteClass
                Dim strSql As String = "Select  Agentcode,Agentname From AgentMast where agentcode not in (select agentcode from agents_locked) and active=1 "
                If para1 <> "" Then
                    strSql = strSql + " and  plgrpcode='" & para1 & "'"
                End If
                If para2 <> "" Then
                    strSql = strSql + " and  sellcode='" & para2 & "'"
                End If
                strSql = strSql + " order by Agentname "

                Dim MYCommand As New SqlCommand(strSql, clsDBConnect.dbConnectionnew(Session("dbconnectionName").ToString()))

                Dim Dr As SqlDataReader
                Dr = MYCommand.ExecuteReader
                While Dr.Read
                    If Not IsDBNull("Agentcode") Then
                        CustomerAutoCompleteClassAdd = New AutocompleteClass
                        CustomerAutoCompleteClassAdd.Id = Dr("AgentCode")
                        CustomerAutoCompleteClassAdd.Name = Dr("AgentName")
                        CustomerAutoCompleteClass.Add(CustomerAutoCompleteClassAdd)
                    End If
                End While
                Dr.Close()
                MYCommand.Cancel()
                Return CustomerAutoCompleteClass
                'Dim retlist(2) As String
                'Dim min As String
                'Dim max As String

                'min = objUtils.ExecuteQueryReturnStringValuenew(constr, "select minpax from othcatmast where othgrpcode='TRFS' and othcatcode='" & vehicletype & "'")
                'max = objUtils.ExecuteQueryReturnStringValuenew(constr, "select maxpax from othcatmast where othgrpcode='TRFS' and othcatcode='" & vehicletype & "'")

                'retlist(0) = min
                'retlist(1) = max


                'Return retlist
            Catch ex As Exception

            End Try
        End Function

        <WebMethod(EnableSession:=True)> _
        Public Function CustomerSearchAutoComplete() As List(Of AutocompleteClass)
            Try
                Dim CustomerAutoCompleteClass As New List(Of AutocompleteClass)
                Dim CustomerAutoCompleteClassAdd As New AutocompleteClass
                Dim strSql As String
                'If para1 <> "" Then
                '    strSql = "Select  Agentcode,Agentname From AgentMast where active=1 and Agentname like '" & para1 & "'%"
                'End If

                strSql = "Select  Agentcode,Agentname From AgentMast where active=1 "

                Dim MYCommand As New SqlCommand(strSql, clsDBConnect.dbConnectionnew(Session("dbconnectionName").ToString()))

                Dim Dr As SqlDataReader
                Dr = MYCommand.ExecuteReader
                While Dr.Read
                    If Not IsDBNull("Agentcode") Then
                        CustomerAutoCompleteClassAdd = New AutocompleteClass
                        CustomerAutoCompleteClassAdd.Id = Dr("AgentCode")
                        CustomerAutoCompleteClassAdd.Name = Dr("AgentName")
                        CustomerAutoCompleteClass.Add(CustomerAutoCompleteClassAdd)
                    End If
                End While
                Dr.Close()
                MYCommand.Cancel()
                Return CustomerAutoCompleteClass
            Catch ex As Exception
            End Try
        End Function

        <WebMethod(EnableSession:=True)> _
        Public Function RouteAutoComplete(ByVal para1 As String) As List(Of AutocompleteStringClass)
            Try
                'Dim RouteAutoCompleteClass As New List(Of String)
                Dim RouteAutoCompleteClass As New List(Of AutocompleteStringClass)
                Dim RouteAutoCompleteClassAdd As New AutocompleteStringClass
                'Dim CustomerAutoCompleteClassAdd As New AutocompleteClass
                Dim strSql As String = "Select othtypname  from othtypmast Where  othgrpcode='TRFS' and active=1 "
                If para1 <> "" Then
                    strSql = strSql + " and  othtypname like'" & para1 & "%'"
                End If
                'If para2 <> "" Then
                '    strSql = strSql + " and  sellcode='" & para2 & "'"
                'End If
                strSql = strSql + " order by othtypname "

                Dim MYCommand As New SqlCommand(strSql, clsDBConnect.dbConnectionnew(Session("dbconnectionName").ToString()))

                Dim Dr As SqlDataReader
                Dr = MYCommand.ExecuteReader
                While Dr.Read
                    If Not IsDBNull("othtypname") Then
                        RouteAutoCompleteClassAdd = New AutocompleteStringClass
                        'RouteAutoCompleteClassAdd.Id = Dr("AgentCode")
                        RouteAutoCompleteClassAdd.Name = Dr("othtypname")
                        RouteAutoCompleteClass.Add(RouteAutoCompleteClassAdd)
                    End If
                End While
                Dr.Close()
                MYCommand.Cancel()
                Return RouteAutoCompleteClass
                'Dim retlist(2) As String
                'Dim min As String
                'Dim max As String

                'min = objUtils.ExecuteQueryReturnStringValuenew(constr, "select minpax from othcatmast where othgrpcode='TRFS' and othcatcode='" & vehicletype & "'")
                'max = objUtils.ExecuteQueryReturnStringValuenew(constr, "select maxpax from othcatmast where othgrpcode='TRFS' and othcatcode='" & vehicletype & "'")

                'retlist(0) = min
                'retlist(1) = max


                'Return retlist
            Catch ex As Exception

            End Try
        End Function

        <WebMethod()> _
        Public Function getMinMaxPax(ByVal constr As String, ByVal vehicletype As String) As String()
            Dim retlist(2) As String
            Dim min As String
            Dim max As String

            min = objUtils.ExecuteQueryReturnStringValuenew(constr, "select minpax from othcatmast where othgrpcode='TRFS' and othcatcode='" & vehicletype & "'")
            max = objUtils.ExecuteQueryReturnStringValuenew(constr, "select maxpax from othcatmast where othgrpcode='TRFS' and othcatcode='" & vehicletype & "'")

            retlist(0) = min
            retlist(1) = max


            Return retlist
        End Function

        <WebMethod()> _
        Public Function Filldates(ByVal constr As String, ByVal exhicode As String, ByVal contractid As String) As String()
            'Dim retlist As String
            'Dim fromdate As String
            'Dim todate As String
            'Dim sqlstr As String
            'Dim result As String

            'sqlstr = "select convert(varchar(10),e.fromdate,103) fromdate,convert(varchar(10),e.todate,103) todate  from  contracts c(nolock),exhibition_detail e(nolock)"
            'sqlstr += " where e.exhibitioncode ='" & exhicode & "' and c.contractid ='" & contractid & "'"
            'sqlstr += " and((convert(varchar(10),e.fromdate,111) between c.fromdate   and c.todate ) or "
            'sqlstr += "  (convert(varchar(10),e.todate,111) between c.fromdate   and c.todate)  "
            'sqlstr += "  or (convert(varchar(10),e.fromdate,111) < c.fromdate and  convert(varchar(10),e.todate,111)>c.todate))   "

            '' fromdate = objUtils.ExecuteQueryReturnStringValuenew(constr, "select top 1 CONVERT(varchar(10),fromdate,103) fromdate from exhibition_detail where exhibitioncode='" & exhicode & "'")
            ''todate = objUtils.ExecuteQueryReturnStringValuenew(constr, "select top 1 CONVERT(varchar(10),todate,103) todate from exhibition_detail where exhibitioncode='" & exhicode & "'")

            'retlist = objUtils.ExecuteQueryReturnStringValuenew(constr, sqlstr)

            Dim retlist As Array
            Dim cnt As Long = 2

            Dim sqlstr As StringBuilder = New StringBuilder()
            sqlstr.AppendFormat("exec sp_get_exhidates '{0}','{1}' ", exhicode, contractid)

            retlist = objUtils.FillArraynew(constr, sqlstr.ToString, 2)



            Return retlist
        End Function

        <WebMethod()> _
        Public Function FillRemarks(ByVal constr As String, ByVal othtyp As String) As String
            'Dim retlist As New List(Of clsMaster)
            Dim result As String
            Dim sqlstr As String
            sqlstr = "select remarks from othtypmast where othtypcode='" & othtyp & "'"

            result = objUtils.ExecuteQueryReturnStringValuenew(constr, sqlstr)
            If result = Nothing Or IsDBNull(result) Then
                result = ""

            End If
            Return result
        End Function
        <WebMethod()> _
        Public Function Fillhtldate_adults(ByVal constr As String, ByVal requestid As String, ByVal rlineno As Int16) As String()
            Dim retlist(3) As String
            Dim partycode As String
            Dim chkindate As String
            Dim chkoutdate As String
            Dim adult, child As String
            partycode = objUtils.ExecuteQueryReturnStringValuenew(constr, "select partycode from reservation_detailnewtemp where requestid='" & requestid & "' and rlineno='" & rlineno & "'")
            chkindate = objUtils.ExecuteQueryReturnStringValuenew(constr, "select checkin from reservation_online_hotelstemp where  requestid='" & requestid & "' and partycode='" & partycode & "'")
            adult = objUtils.ExecuteQueryReturnStringValuenew(constr, "select COUNT(*) from reservation_guestnewtemp where requestid='" & requestid & "' and rlineno='" & rlineno & "' and guesttype='Adult'")
            child = objUtils.ExecuteQueryReturnStringValuenew(constr, "select COUNT(*) from reservation_guestnewtemp where requestid='" & requestid & "' and rlineno='" & rlineno & "' and guesttype='Child'")
            chkoutdate = objUtils.ExecuteQueryReturnStringValuenew(constr, "select dateadd(d,-1,checkout) from reservation_online_hotelstemp where  requestid='" & requestid & "' and partycode='" & partycode & "'")
            retlist(0) = chkindate
            retlist(1) = adult
            retlist(2) = child
            retlist(3) = chkoutdate
            Return retlist
        End Function
        <WebMethod()> _
        Public Function Fillhtldate_adultsMA(ByVal constr As String, ByVal requestid As String, ByVal rlineno As Int16) As String()
            Dim retlist(3) As String
            Dim partycode As String
            Dim chkindate, chkoutdate As String
            Dim adult, child As String
            'partycode = objUtils.ExecuteQueryReturnStringValuenew(constr, "select partycode from meetassisttemp where requestid='" & requestid & "' and sno='" & rlineno & "'")
            chkindate = objUtils.ExecuteQueryReturnStringValuenew(constr, "select frmdate from meetassisttemp where  requestid='" & requestid & "' and sno='" & rlineno & "'")
            adult = objUtils.ExecuteQueryReturnStringValuenew(constr, "select adults from meetassisttemp where  requestid='" & requestid & "' and sno='" & rlineno & "'")
            child = objUtils.ExecuteQueryReturnStringValuenew(constr, "select child from meetassisttemp where  requestid='" & requestid & "' and sno='" & rlineno & "'")
            chkoutdate = objUtils.ExecuteQueryReturnStringValuenew(constr, "select todate from meetassisttemp where  requestid='" & requestid & "' and sno='" & rlineno & "'")

            retlist(0) = chkindate
            retlist(1) = adult
            retlist(2) = child
            retlist(3) = chkoutdate

            Return retlist
        End Function

        <WebMethod()> _
        Public Function FillTransferDateAdultChild(ByVal constr As String, ByVal requestid As String, ByVal rlineno As Int16) As String()
            Dim retlist As Array
            Dim cnt As Long = 4
            Dim sqlstr As StringBuilder = New StringBuilder()
            sqlstr.AppendFormat("exec sp_get_htldate_adults '{0}',{1} ", requestid, rlineno)
            retlist = objUtils.FillArraynew(constr, sqlstr.ToString, cnt)
            Return retlist

        End Function

        <WebMethod()> _
        Public Function fillvehibontype(ByVal constr As String, ByVal mytype As String) As clsMaster()
            '04082014
            Dim retlist As New List(Of clsMaster)
            Dim cnt As Long = 4
            Dim SqlStr As String = String.Empty
            SqlStr = "select vehiclename,vehiclecode from vehiclemaster where isnull(usedfor,0)=" & mytype & ""
            retlist = objUtils.FillclsMasternew(constr, SqlStr.ToString, True)
            Return retlist.ToArray()

        End Function

        <WebMethod()> _
         Public Function getvehiclenameweb(ByVal constr As String, ByVal mytype As String) As clsMaster()
            '07082014
            Dim retlist As New List(Of clsMaster)
            Dim cnt As Long = 4
            Dim SqlStr As String = String.Empty

            If Trim(mytype) <> "" And Trim(UCase(mytype)) <> Trim(UCase("[Select]")) Then
                SqlStr = "select vehiclemaster.vehiclename,vehiclemaster.vehiclecode  from drivermaster inner join vehiclemaster on vehiclemaster.vehiclecode=drivermaster.vehiclecode where isnull(vehiclemaster.usedfor,0)=0 and drivermaster.drivercode='" & mytype & "' union all  " _
                     & " select vehiclemaster.vehiclename,vehiclemaster.vehiclecode  from vehiclemaster where isnull(vehiclemaster.usedfor,0)=0 and vehiclecode not in (select vehiclecode from  drivermaster  where drivermaster.drivercode='" & mytype & "') "

            Else
                SqlStr = "select vehiclename,vehiclecode from vehiclemaster where active=1"
            End If


            'If Trim(mytype) <> "" And Trim(UCase(mytype)) <> Trim(UCase("[Select]")) Then
            '    SqlStr = "select vehiclemaster.vehiclename,vehiclemaster.vehiclecode from drivermaster inner join vehiclemaster on vehiclemaster.vehiclecode=drivermaster.vehiclecode where drivermaster.drivercode='" & mytype & "'"
            'Else
            '    SqlStr = "select vehiclename,vehiclecode from vehiclemaster where active=1"
            'End If

            retlist = objUtils.FillclsMasternew(constr, SqlStr.ToString, True)
            Return retlist.ToArray()

        End Function

        <WebMethod()> _
        Public Function FillCarratetype(ByVal constr As String, ByVal fromdate As String, ByVal todate As String, ByVal requestid As String, ByVal othgrpcode As String, ByVal adults As Integer, ByVal child As Integer, ByVal freeform As Integer) As String()
            '   Dim retlist As New List(Of clsMaster)
            Dim retlist As Array
            Dim frmdate As Date = fromdate
            Dim tdate As Date = todate
            fromdate = Format(frmdate, "yyyy/MM/dd")
            todate = Format(tdate, "yyyy/MM/dd")

            Dim sqlstr As StringBuilder = New StringBuilder()
            sqlstr.AppendFormat("exec sp_get_carrental_type '{0}','{1}','{2}','{3}',{4},{5},{6} ", fromdate, todate, requestid, othgrpcode, adults, child, freeform)

            retlist = objUtils.FillArraynew(constr, sqlstr.ToString, 2)

            Return retlist

        End Function
        <WebMethod()> _
        Public Function Check_Creditlimit(ByVal constr As String, ByVal requestid As String, ByVal custcode As String, ByVal userid As String, ByVal groups As String) As String()

            Dim allowcashuser As String
            allowcashuser = ""
            Dim retlist(3) As String
            Dim ds As DataSet
            Dim sqlstr As String
            If Trim(requestid) <> "" And Trim(UCase(custcode)) <> Trim(UCase("[Select]")) Then
                sqlstr = "select allowcashuser from agentmast(nolock) where agentcode='" & custcode & "' "
                allowcashuser = objUtils.ExecuteQueryReturnStringValuenew(constr, sqlstr.ToString)
            End If

            sqlstr = "execute sp_chk_credit_limits_prebooking" + " '" & requestid & "'," & 2 & ",'" & custcode & "'," & groups & ""
            Dim cnt As Long = 4

            ds = objUtils.ExecuteQuerySqlnew(constr, sqlstr)

            If ds.Tables.Count > 0 Then
                If ds.Tables(0).Rows.Count > 0 Then
                    If IsDBNull(ds.Tables(0).Rows(0)("CreditAction")) = False Then
                        If ds.Tables(0).Rows(0)("CreditAction") = "Warn" Then
                            Dim strwarn As String = CType(ds.Tables(0).Rows(0)("Message"), String)
                            '  ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" + strwarn + "');", True)
                            'Return
                        Else
                            If ds.Tables(0).Rows(0)("CreditAction") = "Stop" Then
                                If allowcashuser = CType(userid, String) Then
                                Else
                                    If groups = 1 Then
                                        retlist(0) = "Permission required from Accounts Dept. to book (Groups)"
                                    Else
                                        retlist(0) = "Permission required from Accounts Dept. to book"

                                    End If

                                    retlist(1) = "False"
                                End If
                            Else
                                If ds.Tables(0).Rows(0)("CreditAction") = "Ewarn" Then
                                    Dim strwarn As String = CType(ds.Tables(0).Rows(0)("Message"), String)
                                    If groups = 1 Then
                                        retlist(0) = "Permission required from Accounts Dept. to book (Groups)"
                                    Else
                                        retlist(0) = "Permission required from Accounts Dept. to book"

                                    End If
                                    retlist(1) = "False"
                                End If
                            End If
                        End If
                    End If
                End If
            Else
                retlist(0) = ""
                retlist(1) = "True"
            End If

            Return retlist
        End Function

        <WebMethod()> _
        Public Function FillCarType(ByVal constr As String, ByVal requestid As String, ByVal rlineno As Int16, ByVal olineno As Int16, ByVal Otherdt As String,
                                    ByVal grpCode As String, ByVal freeform As Integer) As clsMaster()
            Dim retlist As New List(Of clsMaster)

            Dim otherdtemp As Date = Otherdt
            Otherdt = Format(otherdtemp, "yyyy/MM/dd")


            Dim sqlstr As StringBuilder = New StringBuilder()
            'sqlstr.AppendFormat("exec sp_get_other_types '{0}',{1},{2},'{3}','{4}',{5} ", requestid, rlineno, olineno, Otherdt, grpCode, freeform)
            sqlstr.AppendFormat("sp_get_other_types_all '{0}',{1},{2},'{3}','{4}',{5} ", requestid, rlineno, olineno, Otherdt, grpCode, freeform)
            retlist = objUtils.FillclsMasternew(constr, sqlstr.ToString, True)
            Return retlist.ToArray()

        End Function
        <WebMethod()> _
        Public Function FillCarTypeExc(ByVal constr As String, ByVal requestid As String, ByVal rlineno As Int16, ByVal olineno As Int16, ByVal Otherdt As String,
                                    ByVal grpCode As String, ByVal freeform As Integer) As clsMaster()
            Dim retlist As New List(Of clsMaster)

            Dim otherdtemp As Date = Otherdt
            Otherdt = Format(otherdtemp, "yyyy/MM/dd")


            Dim sqlstr As StringBuilder = New StringBuilder()
            sqlstr.AppendFormat("exec sp_get_other_types '{0}',{1},{2},'{3}','{4}',{5} ", requestid, rlineno, olineno, Otherdt, grpCode, freeform)
            retlist = objUtils.FillclsMasternew(constr, sqlstr.ToString, True)
            Return retlist.ToArray()

        End Function

        <WebMethod()> _
        Public Function GetGuideNameList(ByVal constr As String, ByVal langcode As String) As clsMaster()
            Dim retlist As New List(Of clsMaster)
            Dim sqlstr As String
            If Trim(langcode) <> "" And Trim(UCase(langcode)) <> Trim(UCase("[Select]")) Then
                sqlstr = "select gm.guidecode,gm.guidename from guide_master gm inner join guide_details gd on gm.guidecode=gd.guidecode where gd.nationalitycode='" & langcode & "' order by gm.guidename"
            Else
                sqlstr = "select guidecode,guidename from guide_master where active=1 order by guidename"
            End If
            retlist = objUtils.FillclsMasternew(constr, sqlstr.ToString, True)
            Return retlist.ToArray()
        End Function

        <WebMethod()> _
        Public Function getflightdetfromairlinecode(ByVal constr As String, ByVal airlinecode As String) As clsMaster()
            Dim retlist As New List(Of clsMaster)
            Dim sqlstr As String
            If Trim(airlinecode) <> "" And Trim(UCase(airlinecode)) <> Trim(UCase("[Select]")) Then
                sqlstr = "select flightcode,flightcode As name  from flightmast  where flightmast.airlinecode='" & airlinecode & "' order by flightmast.flightcode"
            Else
                sqlstr = "select flightcode,flightcode As name from flightmast where active=1 order by flightcode"
            End If
            retlist = objUtils.FillclsMasternew(constr, sqlstr.ToString, True)
            Return retlist.ToArray()
        End Function
        <WebMethod()> _
        Public Function getsubgroupfrommaingroup(ByVal constr As String, ByVal groupcode As String) As clsMaster()
            Dim retlist As New List(Of clsMaster)
            Dim sqlstr As String
            If Trim(groupcode) <> "" And Trim(UCase(groupcode)) <> Trim(UCase("[Select]")) Then
                sqlstr = "select othtypcode,othtypname from vw_excplist where active=1 and othgrpcode='" & groupcode & "' order by othtypcode"
            Else
                sqlstr = "select othtypcode,othtypname from vw_excplist where active=1  order by othtypcode"
            End If
            retlist = objUtils.FillclsMasternew(constr, sqlstr.ToString, True)
            Return retlist.ToArray()
        End Function


        <WebMethod()> _
        Public Function GetGuideMobileList(ByVal constr As String, ByVal langcode As String) As String
            Dim retlist As String = ""
            Dim sqlstr As String
            If Trim(langcode) <> "" And Trim(UCase(langcode)) <> Trim(UCase("[Select]")) Then
                sqlstr = "select gm.mobile from guide_master gm inner join guide_details gd on gm.guidecode=gd.guidecode where gd.guidecode='" & langcode & "'"
                retlist = objUtils.ExecuteQueryReturnStringValuenew(constr, sqlstr.ToString)
            End If

            Return retlist
        End Function
        <WebMethod()> _
        Public Function Getcostcode(ByVal constr As String, ByVal invoiceno As String, ByVal partycode As String, ByVal grpcode As String) As String
            Dim retlist As String = ""
            Dim sqlstr As String
            sqlstr = "sp_assign_costcode '" + invoiceno + "','" + partycode + "','" + grpcode + "'"
            retlist = objUtils.ExecuteQueryReturnStringValuenew(constr, sqlstr.ToString)

            Return retlist
        End Function
        <WebMethod()> _
        Public Function GetLanguageList(ByVal constr As String, ByVal guidecode As String) As clsMaster()
            Dim retlist As New List(Of clsMaster)
            Dim sqlstr As String
            If Trim(guidecode) <> "" And Trim(UCase(guidecode)) <> Trim(UCase("[Select]")) Then
                sqlstr = "select n.nationalitycode,n.nationLanguage from nationality_master n inner join guide_details gd on n.nationalitycode=gd.nationalitycode where gd.guidecode='" & guidecode & "' order by n.nationLanguage"
            Else
                sqlstr = "select distinct n.nationalitycode,n.nationLanguage from nationality_master n inner join guide_details gd on n.nationalitycode=gd.nationalitycode order by n.nationLanguage"

            End If
            retlist = objUtils.FillclsMasternew(constr, sqlstr.ToString, True)
            Return retlist.ToArray()
        End Function

        <WebMethod()> _
        Public Function GetLanguageListOther(ByVal constr As String, ByVal guidecode As String) As clsMaster()
            Dim retlist As New List(Of clsMaster)
            Dim sqlstr As String
            If Trim(guidecode) <> "" And Trim(UCase(guidecode)) <> Trim(UCase("[Select]")) Then
                sqlstr = "select distinct n.nationalitycode,n.nationalityname from nationality_master n " & _
                            " inner join reservation_online_hotels_others ot on ot.nationalitycode=n.nationalitycode" & _
                            " where n.active=1 and ot.guidecode='" & guidecode & "'"
            Else
                sqlstr = "select distinct n.nationalitycode,n.nationLanguage from nationality_master n inner join guide_details gd on n.nationalitycode=gd.nationalitycode order by n.nationLanguage"

            End If
            retlist = objUtils.FillclsMasternew(constr, sqlstr.ToString, True)
            Return retlist.ToArray()
        End Function
        <WebMethod()> _
        Public Function FillCarType_Meals(ByVal constr As String, ByVal requestid As String, ByVal Otherdt As String,
                                    ByVal grpCode As String, ByVal freeform As Integer, ByVal partycode As String) As clsMaster()
            Dim retlist As New List(Of clsMaster)

            Dim otherdtemp As Date = Otherdt
            Otherdt = Format(otherdtemp, "yyyy/MM/dd")


            Dim sqlstr As StringBuilder = New StringBuilder()
            sqlstr.AppendFormat("exec sp_get_other_types_Meals '{0}','{1}','{2}','{3}',{4} ", requestid, partycode, Otherdt, grpCode, freeform)
            retlist = objUtils.FillclsMasternew(constr, sqlstr.ToString, True)
            Return retlist.ToArray()

        End Function

        <WebMethod()> _
        Public Function FillTypeForApirport(ByVal constr As String, ByVal requestid As String, ByVal rlineno As Int16, ByVal olineno As Int16, ByVal Otherdt As String,
                                    ByVal grpCode As String, ByVal freeform As Integer, ByVal airportbordercode As String) As clsMaster()
            Dim retlist As New List(Of clsMaster)
            Dim otherdtemp As Date = Otherdt
            Otherdt = Format(otherdtemp, "yyyy/MM/dd")
            If airportbordercode = "" Then
                Exit Function
            End If

            Dim sqlstr As StringBuilder = New StringBuilder()
            sqlstr.AppendFormat("exec sp_get_airportma_types '{0}',{1},{2},'{3}','{4}',{5},'{6}' ", requestid, rlineno, olineno, Otherdt, grpCode, freeform, airportbordercode)
            retlist = objUtils.FillclsMasternew(constr, sqlstr.ToString, True)
            Return retlist.ToArray()

        End Function

        <WebMethod()> _
        Public Function FillRateType(ByVal constr As String, ByVal requestid As String, ByVal rlineno As Int16, ByVal olineno As Int16, ByVal Otherdt As String,
                                    ByVal grpCode As String, ByVal adult As Int32, ByVal child As Int32, ByVal freeform As Integer) As clsMaster()
            Dim retlist As New List(Of clsMaster)
            Dim otherdtemp As Date = Otherdt
            Otherdt = Format(otherdtemp, "yyyy/MM/dd")
            Dim sqlstr As StringBuilder = New StringBuilder()
            'sqlstr.AppendFormat("exec sp_get_other_cat '{0}',{1},{2},'{3}','{4}',{5},{6},{7}  ", requestid, rlineno, olineno, Otherdt, grpCode, adult, child, freeform)
            sqlstr.AppendFormat("exec sp_get_other_cat_all '{0}',{1},{2},'{3}','{4}',{5},{6},{7}  ", requestid, rlineno, olineno, Otherdt, grpCode, adult, child, freeform)
            retlist = objUtils.FillclsMasternew(constr, sqlstr.ToString, True)
            Return retlist.ToArray()

        End Function
        <WebMethod()> _
        Public Function FillRateTypeExc(ByVal constr As String, ByVal requestid As String, ByVal rlineno As Int16, ByVal olineno As Int16, ByVal Otherdt As String,
                                    ByVal grpCode As String, ByVal adult As Int32, ByVal child As Int32, ByVal freeform As Integer) As clsMaster()
            Dim retlist As New List(Of clsMaster)
            Dim otherdtemp As Date = Otherdt
            Otherdt = Format(otherdtemp, "yyyy/MM/dd")
            Dim sqlstr As StringBuilder = New StringBuilder()
            sqlstr.AppendFormat("exec sp_get_other_cat '{0}',{1},{2},'{3}','{4}',{5},{6},{7}  ", requestid, rlineno, olineno, Otherdt, grpCode, adult, child, freeform)

            retlist = objUtils.FillclsMasternew(constr, sqlstr.ToString, True)
            Return retlist.ToArray()

        End Function

        <WebMethod()> _
        Public Function FillRateType_Meals(ByVal constr As String, ByVal requestid As String, ByVal Otherdt As String,
                                    ByVal grpCode As String, ByVal adult As Int32, ByVal child As Int32,
                                    ByVal freeform As Integer, ByVal partycode As String) As clsMaster()
            Dim retlist As New List(Of clsMaster)
            Dim otherdtemp As Date = Otherdt
            Otherdt = Format(otherdtemp, "yyyy/MM/dd")
            Dim sqlstr As StringBuilder = New StringBuilder()
            sqlstr.AppendFormat("exec sp_get_other_cat_Meals '{0}','{1}','{2}','{3}',{4},{5},{6}  ", requestid, partycode,
                                Otherdt, grpCode, adult, child, freeform)
            retlist = objUtils.FillclsMasternew(constr, sqlstr.ToString, True)
            Return retlist.ToArray()

        End Function

        <WebMethod()> _
        Public Function FillGridPrices(ByVal constr As String, ByVal requestid As String, ByVal rlineno As Int16, ByVal olineno As Int16, ByVal Otherdt As String,
                                    ByVal grpCode As String, ByVal othtypcode As String, ByVal othcatcode As String, ByVal unitpax As Integer) As String()
            Dim retlist As Array
            Dim otherdtemp As Date = Otherdt
            Otherdt = Format(otherdtemp, "yyyy/MM/dd")
            Dim sqlstr As StringBuilder = New StringBuilder()
            If (othtypcode <> "[Select]" And othcatcode <> "[Select]") Then

                sqlstr.AppendFormat("exec sp_get_othplist_prices '{0}','{1}','{2}','{3}',{4},{5},{6},'{7}'  ", Otherdt, othtypcode, othcatcode, requestid, rlineno, olineno, unitpax, grpCode)
                Dim cnt As Long = 6

                retlist = objUtils.FillArraynew(constr, sqlstr.ToString, cnt)

                Return retlist

            End If
        End Function
        <WebMethod()> _
        Public Function FillGridPricescaarrental(ByVal constr As String, ByVal requestid As String, ByVal rlineno As Int16, ByVal olineno As Int16, ByVal Otherdt As String,
                                    ByVal grpCode As String, ByVal othtypcode As String, ByVal othcatcode As String, ByVal unitpax As Integer) As String()
            Dim retlist As Array = Nothing
            Dim otherdtemp As Date = Otherdt
            Otherdt = Format(otherdtemp, "yyyy/MM/dd")
            Dim sqlstr As StringBuilder = New StringBuilder()
            If (othtypcode <> "[Select]" And othcatcode <> "[Select]") Then

                sqlstr.AppendFormat("exec sp_get_othplist_prices_carrental '{0}','{1}','{2}','{3}',{4},{5},{6},'{7}'  ", Otherdt, othtypcode, othcatcode, requestid, rlineno, olineno, unitpax, grpCode)
                Dim cnt As Long = 6

                retlist = objUtils.FillArraynew(constr, sqlstr.ToString, cnt)

                'If retlist.Length > 0 Then
                '    Return retlist
                'Else
                '    Return Nothing
                'End If

                Return retlist
            End If
        End Function
        <WebMethod()> _
        Public Function FillGridPricescaarrentalcost(ByVal constr As String, ByVal requestid As String, ByVal rlineno As Int16,
                 ByVal olineno As Int16, ByVal Otherdt As String,
                                    ByVal grpCode As String, ByVal othtypcode As String, ByVal othcatcode As String,
                                    ByVal unitpax As Integer, ByVal suppliercode As String, ByVal freeform As Integer) As String()
            Dim retlist As Array = Nothing
            Dim otherdtemp As Date = Otherdt
            Otherdt = Format(otherdtemp, "yyyy/MM/dd")
            Dim sqlstr As StringBuilder = New StringBuilder()
            If (othtypcode <> "[Select]" And othcatcode <> "[Select]") Then

                sqlstr.AppendFormat("exec sp_get_othplist_prices_carrental_cost '{0}','{1}','{2}','{3}',{4},{5},{6},'{7}' ,'{8}',{9} ", Otherdt, othtypcode, othcatcode, requestid, rlineno, olineno, unitpax, grpCode, suppliercode, freeform)
                Dim cnt As Long = 4

                retlist = objUtils.FillArraynew(constr, sqlstr.ToString, cnt)

                'If retlist.Length > 0 Then
                '    Return retlist
                'Else
                '    Return Nothing
                'End If

                Return retlist
            End If
        End Function

        <WebMethod()> _
        Public Function FillGridPricesTransfercost(ByVal constr As String, ByVal requestid As String, ByVal Otherdt As String,
                                     ByVal othtypcode As String, ByVal othcatcode As String, ByVal suppliercode As String) As String()
            Dim retlist As Array = Nothing
            Dim otherdtemp As Date = Otherdt
            Otherdt = Format(otherdtemp, "yyyy/MM/dd")
            Dim sqlstr As StringBuilder = New StringBuilder()
            If (othtypcode <> "[Select]" And othcatcode <> "[Select]") Then

                sqlstr.AppendFormat("exec sp_get_tplist_prices_Transfers_cost '{0}','{1}','{2}','{3}','{4}'", Otherdt, othtypcode, othcatcode, requestid,
                                      suppliercode)
                Dim cnt As Long = 6

                retlist = objUtils.FillArraynew(constr, sqlstr.ToString, cnt)

                'If retlist.Length > 0 Then
                '    Return retlist
                'Else
                '    Return Nothing
                'End If

                Return retlist
            End If
        End Function

        <WebMethod()> _
        Public Function FillGridPricesAirportcost(ByVal constr As String, ByVal requestid As String, ByVal Otherdt As String,
                                     ByVal othtypcode As String, ByVal othgrpcode As String,
                                     ByVal suppliercode As String) As String()
            Dim retlist As Array = Nothing
            Dim otherdtemp As Date = Otherdt
            Otherdt = Format(otherdtemp, "yyyy/MM/dd")
            Dim sqlstr As StringBuilder = New StringBuilder()
            If (othtypcode <> "[Select]") Then

                sqlstr.AppendFormat("exec sp_get_airportma_pricesForSupplier '{0}','{1}','{2}','{3}','{4}'", Otherdt, othtypcode,
                                    requestid, othgrpcode, suppliercode)
                Dim cnt As Long = 9

                retlist = objUtils.FillArraynew(constr, sqlstr.ToString, cnt)

                'If retlist.Length > 0 Then
                '    Return retlist
                'Else
                '    Return Nothing
                'End If

                Return retlist
            End If
        End Function

        <WebMethod()> _
        Public Function FillGridPricesExcursioncost(ByVal constr As String, ByVal requestid As String, ByVal Otherdt As String,
                                     ByVal othtypcode As String, ByVal othgrpcode As String,
                                     ByVal suppliercode As String) As String()
            Dim retlist As Array = Nothing
            Dim otherdtemp As Date = Otherdt
            Otherdt = Format(otherdtemp, "yyyy/MM/dd")
            Dim sqlstr As StringBuilder = New StringBuilder()
            If (othtypcode <> "[Select]") Then

                sqlstr.AppendFormat("exec sp_get_Excursion_pricesForSupplier '{0}','{1}','{2}','{3}' ", Otherdt, othtypcode,
                                    requestid, suppliercode)
                Dim cnt As Long = 9

                retlist = objUtils.FillArraynew(constr, sqlstr.ToString, cnt)

                'If retlist.Length > 0 Then
                '    Return retlist
                'Else
                '    Return Nothing
                'End If

                Return retlist
            End If
        End Function

        <WebMethod()> _
        Public Function GetTransferType(ByVal constr As String, ByVal othtypcode As String) As Integer
            Dim intTrnsfrtype As Integer

            Dim sqlstr As StringBuilder = New StringBuilder()
            If (othtypcode <> "[Select]") Then

                ' sqlstr.AppendFormat("exec sp_get_airportma_prices '{0}','{1}','{2}',{3},{4}", Otherdt, othtypcode, requestid, rlineno, olineno)

                intTrnsfrtype = objUtils.ExecuteQueryReturnSingleValuenew(constr, "select transfertype from othtypmast where othtypcode='" & othtypcode & "'")
                'retlist = objUtils.FillArraynew(constr, sqlstr.ToString, cnt)

                Return intTrnsfrtype

            End If
        End Function

       <WebMethod()> _
        Public Function FillAirportMAGridPrices(ByVal constr As String, ByVal requestid As String, ByVal rlineno As Int16,
                                                ByVal olineno As Int16, ByVal Otherdt As String, ByVal othtypcode As String, ByVal ddlSupplier As String) As String()
            Dim retlist As Array
            Dim otherdtemp As Date = Otherdt
            Otherdt = Format(otherdtemp, "yyyy/MM/dd")
            Dim sqlstr As StringBuilder = New StringBuilder()
            If (othtypcode <> "[Select]") Then

                sqlstr.AppendFormat("exec sp_get_airportma_prices '{0}','{1}','{2}',{3},{4},'{5}'", Otherdt, othtypcode, requestid, rlineno, olineno, ddlSupplier)
                Dim cnt As Long = 13

                retlist = objUtils.FillArraynew(constr, sqlstr.ToString, cnt)
            End If
            Return retlist
        End Function


        <WebMethod()> _
        Public Function FillExcursionGridPrices(ByVal constr As String, ByVal requestid As String,
                                                 ByVal Otherdt As String,
                                                ByVal othtypcode As String) As String()
            Dim retlist As Array
            Dim otherdtemp As Date = Otherdt
            Otherdt = Format(otherdtemp, "yyyy/MM/dd")
            Dim sqlstr As StringBuilder = New StringBuilder()
            If (othtypcode <> "[Select]") Then

                sqlstr.AppendFormat("exec sp_get_Excursion_prices '{0}','{1}','{2}' ", Otherdt, othtypcode, requestid)
                Dim cnt As Long = 11

                retlist = objUtils.FillArraynew(constr, sqlstr.ToString, cnt)

                Return retlist

            End If
        End Function
        'FillChkInOutDate  requestid.value, hdngrdrlineno.value,inttype,  
        <WebMethod()> _
        Public Function FillChkInOutDate(ByVal constr As String, ByVal prm_requestId As String, ByVal prm_intRlineno As Integer,
                                         ByVal inttype As String, ByVal prm_blnMeetAssist As Boolean) As String
            Dim result As String
            Dim sqlstr As String
            ' select datein ,dateout,*  from reservation_detailnewtemp where requestid ='' and rlineno =

            'sqlstr = "select name,code  from view_getpickups order by ptype+name  "
            If prm_blnMeetAssist = False Then
                If inttype = 1 Then
                    sqlstr = "select datein from reservation_detailnewtemp where requestid ='" & prm_requestId & "' and rlineno =" & prm_intRlineno & ""
                End If

                If inttype = 0 Then
                    sqlstr = "select dateout from reservation_detailnewtemp where requestid ='" & prm_requestId & "' and rlineno =" & prm_intRlineno & ""
                End If

            Else
                If inttype = 1 Then
                    sqlstr = "select frmdate from meetassisttemp where requestid ='" & prm_requestId & "' and sno =" & prm_intRlineno & ""
                End If

                If inttype = 0 Then
                    sqlstr = "select todate from meetassisttemp where requestid ='" & prm_requestId & "' and sno =" & prm_intRlineno & ""
                End If
            End If
            result = objUtils.ExecuteQueryReturnStringValuenew(constr, sqlstr)
            If result = Nothing Or IsDBNull(result) Then
                result = ""

            End If
            Return result
        End Function

        <WebMethod()> _
        Public Function FillSuplrSupAgent(ByVal constr As String, ByVal prm_trplistcode As String) As String()
            Dim retlist As Array
            Dim sqlstr As String

            sqlstr = "  select partycode ,supagentcode  from trfplisth  where tplistcode ='" & prm_trplistcode & "' "
            Dim cnt As Long = 2

            retlist = objUtils.FillArraynew(constr, sqlstr, cnt)

            Return retlist
        End Function

        <WebMethod()> _
        Public Function FillOtherServSuplrSupAgent(ByVal constr As String, ByVal prm_oplistcode As String) As String()
            Dim retlist As Array
            Dim sqlstr As String

            sqlstr = "select partycode ,supagentcode  from othplisth  where oplistcode ='" & prm_oplistcode & "' "
            Dim cnt As Long = 2

            retlist = objUtils.FillArraynew(constr, sqlstr, cnt)

            Return retlist
        End Function


        <WebMethod()> _
        Public Function FillSupplier_trfs(ByVal constr As String, ByVal Otherdt As String, ByVal typecode As String,
                                     ByVal catcode As String, ByVal requestid As String, ByVal isfreeform As Integer,
                                     ByVal isTrfsGrpCode As Integer, ByVal prm_strGrpcode As String,
                                     ByVal prm_intHotlTrfsProvided As Integer) As clsMaster()
            Dim retlist As New List(Of clsMaster)

            If Otherdt <> "" Then
                Dim otherdtemp As Date = Otherdt
                Otherdt = Format(otherdtemp, "yyyy/MM/dd")
            End If
            'Optional ByVal prm_intHotlTrfsProvided As Integer = 0

            Dim sqlstr As StringBuilder = New StringBuilder()
            If isTrfsGrpCode = 1 Then
                sqlstr.AppendFormat("exec sp_get_transfer_Supplier '{0}','{1}','{2}','{3}',{4},{5}", Otherdt, typecode, catcode, requestid, isfreeform, prm_intHotlTrfsProvided)

            End If

            retlist = objUtils.FillclsMasternew(constr, sqlstr.ToString, True)
            Return retlist.ToArray()
        End Function



        <WebMethod()> _
        Public Function FillSupplier(ByVal constr As String, ByVal Otherdt As String, ByVal typecode As String,
                                     ByVal catcode As String, ByVal requestid As String, ByVal isfreeform As Integer,
                                     ByVal isTrfsGrpCode As Integer, ByVal prm_strGrpcode As String) As clsMaster()
            Dim retlist As New List(Of clsMaster)

            If Otherdt <> "" Then
                Dim otherdtemp As Date = Otherdt
                Otherdt = Format(otherdtemp, "yyyy/MM/dd")
            End If
            'Optional ByVal prm_intHotlTrfsProvided As Integer = 0

            Dim sqlstr As StringBuilder = New StringBuilder()
            If isTrfsGrpCode = 1 Then
                sqlstr.AppendFormat("exec sp_get_transfer_Supplier '{0}','{1}','{2}','{3}',{4}", Otherdt.ToString, typecode, catcode, requestid, isfreeform)
            Else

                If (prm_strGrpcode <> "AIRPORTMA") And (prm_strGrpcode <> "EXU") Then 'For both exc and ariport same fnc
                    sqlstr.AppendFormat("exec sp_get_oth_Supplier '{0}','{1}','{2}','{3}',{4},'{5}'", Otherdt.ToString, typecode, catcode, requestid, isfreeform, prm_strGrpcode)
                Else
                    sqlstr.AppendFormat("exec sp_get_Airport_Supplier '{0}','{1}','{2}','{3}',{4},'{5}'", Otherdt.ToString, typecode, catcode, requestid, isfreeform, prm_strGrpcode)
                End If

            End If
            ''dtFiltered = dtNotFiltered.DefaultView.ToTable(True, "RAMI_ISSA")

            retlist = objUtils.FillclsMasternew(constr, sqlstr.ToString, True)
            Return retlist.ToArray()

        End Function

        <WebMethod()> _
        Public Function FillSupplier_Meals(ByVal constr As String, ByVal Otherdt As String, ByVal requestid As String,
                                           ByVal isfreeform As Integer, ByVal prm_strGrpcode As String) As clsMaster()
            Dim retlist As New List(Of clsMaster)

            If Otherdt <> "" Then
                Dim otherdtemp As Date = Otherdt
                Otherdt = Format(otherdtemp, "yyyy/MM/dd")
            End If


            Dim sqlstr As StringBuilder = New StringBuilder()

            sqlstr.AppendFormat("exec sp_get_oth_Supplier_Meals '{0}','{1}',{2},'{3}'", Otherdt.ToString, requestid, isfreeform, prm_strGrpcode)


            retlist = objUtils.FillclsMasternew(constr, sqlstr.ToString, True)
            Return retlist.ToArray()
        End Function

        <WebMethod()> _
        Public Function Fillflight(ByVal constr As String, ByVal type As Integer, ByVal Otherdt As String, ByVal filter As String) As clsMaster()
            Dim retlist As New List(Of clsMaster)
            'Dim sqlstr As String
            'sqlstr = "select [Select] flightcode,[Select] flightcode"
            'If type = "A" Then
            '    sqlstr = "select flightcode,flightcode from  flightmast where active=1 and type=1"
            '    retlist = objUtils.FillclsMasternew(constr, sqlstr, True)
            'End If

            'If type = "D" Then
            '    sqlstr = "select flightcode,flightcode from  flightmast where active=1 and type=0"
            '    retlist = objUtils.FillclsMasternew(constr, sqlstr, True)
            'End If

            'If type = "B" Then
            '    sqlstr = "select '[Select]' flightcode,'[Select]' flightcode"
            '    retlist = objUtils.FillclsMasternew(constr, sqlstr, False)
            'End If     
            If Otherdt <> "" Then
                Dim otherdtemp As Date = Otherdt
                Otherdt = Format(otherdtemp, "yyyy/MM/dd")
            End If


            Dim sqlstr As StringBuilder = New StringBuilder()
            sqlstr.AppendFormat("exec sp_get_flightnos {0},'{1}','{2}'", type, Otherdt, filter)
            retlist = objUtils.FillclsMasternew(constr, sqlstr.ToString, True)
            Return retlist.ToArray()
        End Function

        <WebMethod()> _
        Public Function Fillflight_New(ByVal constr As String, ByVal type As Integer, ByVal Otherdt As String,
                                       ByVal filter As String) As clsMaster()
            Dim retlist As New List(Of clsMaster)

            If Otherdt <> "" And Otherdt <> Nothing Then
                Dim otherdtemp As Date = Otherdt
                Otherdt = Format(otherdtemp, "yyyy/MM/dd")
            End If
            Dim sqlstr As StringBuilder = New StringBuilder()
            sqlstr.AppendFormat("exec sp_get_flightnos_new {0},'{1}','{2}'", type, Otherdt, filter)
            retlist = objUtils.FillclsMasternew(constr, sqlstr.ToString, True)
            Return retlist.ToArray()
        End Function

        <WebMethod()> _
        Public Function GetHotel(ByVal constr As String, ByVal type As String, ByVal frmdate As String, ByVal requestid As String) As String
            Dim retlist As String = ""
            Dim sqlstr As String


            If frmdate <> "" And frmdate <> Nothing Then
                Dim otherdtemp As Date = frmdate
                frmdate = Format(otherdtemp, "yyyy/MM/dd")

                If type = "H" Then
                    sqlstr = "select 'H-'+partycode from reservation_online_hotelstemp where requestid='" & requestid & "' and '" & frmdate & "' between checkin and checkout order by checkin"
                End If
                retlist = objUtils.ExecuteQueryReturnStringValuenew(constr, sqlstr)
            End If
            Return retlist
        End Function

        <WebMethod()> _
        Public Function FillPickup(ByVal constr As String, ByVal type As String) As clsMaster()
            Dim retlist As New List(Of clsMaster)
            Dim sqlstr As String
            sqlstr = "select name,code  from view_getpickups order by ptype+name  "
            If type = "A" Then
                sqlstr = "select name,code  from view_getpickups where ptype='A' order by ptype+name  "
            End If

            If type = "H" Then
                sqlstr = "select name,code from view_getpickups where ptype='H' order by ptype+name  "
            End If
            retlist = objUtils.FillclsMasternew(constr, sqlstr, True)
            Return retlist.ToArray()
        End Function

        'Changed by Mohamed on 25/07/2016
        <WebMethod(EnableSession:=True)> _
        Public Function GetListOfArrayVisualSearch(ByVal arSqlStr) As String()
            Dim retlist As New List(Of String)
            'Dim sqlstr As String
            'sqlstr = "select ltrim(rtrim(cityname)) cityname  from citymast where active=1"
            'retlist = FillStringArray(constr, sqlstr)
            Dim lsconStr As String = Session("dbconnectionName")
            retlist = objUtils.FillStringArray(lsconStr, arSqlStr)
            Return retlist.ToArray()
        End Function

        'Changed by Mohamed on 25/07/2016
        <WebMethod(EnableSession:=True)> _
        Public Function getCommonArrayOfCodeAndNameFromSqlQuery(ByVal constr As String, ByVal arSqlStr As String) As clsMaster()
            Dim retlist As New List(Of clsMaster)
            'Dim lsconStr As String = Session("dbconnectionName")
            retlist = objUtils.FillclsMasternew(constr, arSqlStr, False)
            Return retlist.ToArray()
        End Function
        

        <WebMethod()> _
        Public Function GetCityCodeListnew(ByVal constr As String, ByVal ctrycode As String) As clsMaster()
            Dim retlist As New List(Of clsMaster)
            Dim sqlstr As String
            sqlstr = "select ltrim(rtrim(citycode)) citycode,ltrim(rtrim(cityname)) cityname  from citymast where active=1"
            If Trim(ctrycode) <> "" And Trim(UCase(ctrycode)) <> Trim(UCase("[Select]")) Then
                sqlstr = sqlstr + " and ctrycode='" & Trim(ctrycode) & "'"
            End If
            sqlstr = sqlstr + " order by citycode"
            retlist = objUtils.FillclsMasternew(constr, sqlstr, True)
            Return retlist.ToArray()

        End Function

        <WebMethod(EnableSession:=True)> _
        Public Function getGuestDetails(ByVal hotelLineNo() As String, ByVal ddlTitle() As String, ByVal txtLeadFirstName() As String,
                                        ByVal txtLeadLastName() As String, ByVal txtarrFlightNo() As String, ByVal txtarrTime() As String, ByVal txtarrAirport() As String,
                                        ByVal txtdepFlightNo() As String, ByVal txtdepTime() As String, ByVal txtDepAirport() As String,
                                        ByVal txtremarks() As String, ByVal txtDepartureRemarks() As String, ByVal txtRoomcode() As String, ByVal rowkey() As String, ByVal RIndex() As Integer, ByVal hotelLineNoa() As String, ByVal ddlTitleAdult() As String,
                                      ByVal txtFirstName() As String, ByVal txtLastName() As String, ByVal txtAge() As Double, ByVal rowkeya() As String, ByVal clientref As String) As Boolean

            ClsGuestInfo = New List(Of clsAgentsOnline.BookNow.GuestInfo)
            Dim clsGuestInfoAdd As New clsAgentsOnline.BookNow.GuestInfo
            If Not IsNothing(Session("GuestInfo")) Then
                'removing the hotellineno for guestdetails

                ClsGuestInfo = New List(Of clsAgentsOnline.BookNow.GuestInfo)
                ClsGuestInfo = Session("GuestInfo")
                Dim searchCriteria As SearchCreteria
                searchCriteria = Session("searchCreteria")
                ClsGuestInfo = RemoveGuestInfo(ClsGuestInfo, searchCriteria.hotellineno)
                'For Each guestrow In Session("GuestInfo")
                '    Dim searchCriteria As New SearchCreteria
                '    searchCriteria = Session("searchCreteria")
                '    If guestrow.hotellineno = searchCriteria.hotellineno Then
                '        ClsGuestInfo.RemoveAll(AddressOf fn_RemoveGHotelLineNo)
                '        Session("GuestInfo") = ClsGuestInfo
                '    End If
                'Next
                'ClsGuestInfo = Session("GuestInfo")
            End If

            For i = 1 To hotelLineNo.Length - 1
                clsGuestInfoAdd = New clsAgentsOnline.BookNow.GuestInfo
                clsGuestInfoAdd.hotellineno = Val(hotelLineNo(i))
                clsGuestInfoAdd.Title = ddlTitle(i)
                clsGuestInfoAdd.FirstName = txtLeadFirstName(i)
                clsGuestInfoAdd.LastName = txtLeadLastName(i)
                clsGuestInfoAdd.ArrFlightNo = txtarrFlightNo(i)
                clsGuestInfoAdd.ArrTime = txtarrTime(i)
                clsGuestInfoAdd.ArrAirport = txtarrAirport(i)
                clsGuestInfoAdd.DepFlightNo = txtdepFlightNo(i)
                clsGuestInfoAdd.DepTime = txtdepTime(i)
                clsGuestInfoAdd.DepAirport = txtDepAirport(i)
                clsGuestInfoAdd.Remarks = txtremarks(i)
                clsGuestInfoAdd.DepartureRemarks = txtDepartureRemarks(i)
                clsGuestInfoAdd.RoomTypeCode = txtRoomcode(i)
                clsGuestInfoAdd.rowkey = rowkey(i)
                ClsGuestInfo.Add(clsGuestInfoAdd)
            Next
            Session("GuestInfo") = New List(Of clsAgentsOnline.BookNow.GuestInfo)
            Session("GuestInfo") = ClsGuestInfo
            getAdultChild(RIndex, hotelLineNoa, ddlTitleAdult, txtFirstName, txtLastName, txtAge, rowkeya)

            Return SaveGuestInfo(clientref)

        End Function
        <WebMethod()> _
        Public Function RemoveAdultAndChild(ByVal listOfAdultAndChild As List(Of clsAgentsOnline.BookNow.GuestInfo.AdultChildlts), ByVal hotelLineNo As Integer) As List(Of clsAgentsOnline.BookNow.GuestInfo.AdultChildlts)
            For i = 0 To listOfAdultAndChild.Count - 1
                If listOfAdultAndChild(i).hotellineno.Equals(hotelLineNo) Then
                    listOfAdultAndChild.Remove(listOfAdultAndChild(i))
                    RemoveAdultAndChild(listOfAdultAndChild, hotelLineNo)
                    Exit For
                End If
            Next
            Return listOfAdultAndChild
        End Function
        <WebMethod()> _
        Public Function RemoveGuestInfo(ByVal listOfGuestInfo As List(Of clsAgentsOnline.BookNow.GuestInfo), ByVal hotelLineNo As Integer) As List(Of clsAgentsOnline.BookNow.GuestInfo)
            For i = 0 To listOfGuestInfo.Count - 1
                If i <= listOfGuestInfo.Count - 1 Then
                    If listOfGuestInfo(i).hotellineno.Equals(hotelLineNo) Then
                        listOfGuestInfo.Remove(listOfGuestInfo(i))
                        RemoveGuestInfo(listOfGuestInfo, hotelLineNo)
                        Exit For
                    End If
                End If
            Next
            Return listOfGuestInfo
        End Function
        <WebMethod(EnableSession:=True)> _
        Public Function getAdultChild(ByVal RIndex() As Integer, ByVal hotelLineNoa() As String, ByVal ddlTitle() As String,
                                      ByVal txtFirstName() As String, ByVal txtLastName() As String, ByVal txtAge() As Double, ByVal rowkeya() As String) As Boolean

            ClsAdultChildInfo = New List(Of clsAgentsOnline.BookNow.GuestInfo.AdultChildlts)
            Dim ClsAdultChildInfoAdd As New clsAgentsOnline.BookNow.GuestInfo.AdultChildlts
            If Not IsNothing(Session("AdultChildInfo")) Then
                'Session("AdultChildInfo") = Nothing
                'ClsAdultChildInfo = Session("AdultChildInfo")
                'ClsAdultChildInfo = New List(Of clsAgentsOnline.BookNow.GuestInfo.AdultChildlts)

                'Session("AdultChildInfo") = Nothing
                ClsAdultChildInfo = New List(Of clsAgentsOnline.BookNow.GuestInfo.AdultChildlts)
                ClsAdultChildInfo = Session("AdultChildInfo")
                Dim searchCriteria As SearchCreteria
                searchCriteria = Session("searchCreteria")
                ClsAdultChildInfo = RemoveAdultAndChild(ClsAdultChildInfo, searchCriteria.hotellineno)
                'removing the hotellineno for adultschild
                'For Each guestrow In Session("AdultChildInfo")
                '    If guestrow.hotellineno = Session("hotellineno") Then
                '        ClsAdultChildInfo.RemoveAll(AddressOf fn_RemoveAcHotelLineNo)
                '        Session("AdultChildInfo") = ClsAdultChildInfo
                '    End If
                'Next
                'ClsAdultChildInfo = Session("AdultChildInfo")
            End If
            For i = 0 To RIndex.Length - 1
                ClsAdultChildInfoAdd = New clsAgentsOnline.BookNow.GuestInfo.AdultChildlts
                ClsAdultChildInfoAdd.hotellineno = Val(hotelLineNoa(i))
                ClsAdultChildInfoAdd.Title = ddlTitle(i)
                ClsAdultChildInfoAdd.FirstName = txtFirstName(i)
                ClsAdultChildInfoAdd.LastName = txtLastName(i)
                ClsAdultChildInfoAdd.Age = txtAge(i)
                ClsAdultChildInfoAdd.rowkey = rowkeya(i)
                ClsAdultChildInfo.Add(ClsAdultChildInfoAdd)
            Next
            Session("AdultChildInfo") = New List(Of clsAgentsOnline.BookNow.GuestInfo.AdultChildlts)
            Session("AdultChildInfo") = ClsAdultChildInfo
            Return True

        End Function


        Protected Function SaveGuestInfo(ByVal clientref As String) As Boolean

            Dim mySqlConn As SqlConnection
            Dim sqlTrans As SqlTransaction
            Dim mySqlCmd As SqlCommand
            Dim mySqlReader As SqlDataReader
            Dim ClsBookRoom As New List(Of clsAgentsOnline.BookRoom)

            Try



                mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                sqlTrans = mySqlConn.BeginTransaction()

                ClsGuestInfo = Session("GuestInfo")
                ClsAdultChildInfo = Session("AdultChildInfo")

                Dim searchCriteria As New SearchCreteria
                Dim bookForHotel As New clsAgentsOnline.BookForHotel
                If Not Session("searchCreteria") Is Nothing Then
                    searchCriteria = Session("searchCreteria")
                    If Not Session("BookForHotel") Is Nothing Then
                        bookForHotel = Session("BookForHotel")
                        For i = 0 To bookForHotel.Hotels.Count - 1
                            Dim hotel As New Hotel
                            If bookForHotel.Hotels(i).SearchDetails.hotellineno.Equals(searchCriteria.hotellineno) Then
                                ClsBookRoom = bookForHotel.Hotels(i).Room
                                For j = 0 To ClsBookRoom.Count - 1
                                    Dim listOfGuestDetails As New List(Of clsAgentsOnline.BookNow.GuestInfo)
                                    Dim listOfAdultAndChildDetails As New List(Of clsAgentsOnline.BookNow.AdultChild)
                                    Dim guestDetails As New clsAgentsOnline.BookNow.GuestInfo
                                    For k = 0 To ClsGuestInfo.Count - 1
                                        If ClsBookRoom(j).RoomTypeCode.Equals(ClsGuestInfo(k).rowkey.Split(",")(2)) And ClsBookRoom(j).mealcode.Equals(ClsGuestInfo(k).rowkey.Split(",")(3)) And ClsBookRoom(j).optionno.Equals(ClsGuestInfo(k).rowkey.Split(",")(1)) And ClsBookRoom(j).Rmcatcode.Equals(ClsGuestInfo(k).rowkey.Split(",")(4)) Then
                                            guestDetails.Title = ClsGuestInfo(k).Title
                                            guestDetails.FirstName = ClsGuestInfo(k).FirstName.ToUpper()
                                            guestDetails.LastName = ClsGuestInfo(k).LastName.ToUpper()
                                            guestDetails.hotellineno = ClsGuestInfo(k).hotellineno
                                            guestDetails.Remarks = ClsGuestInfo(k).Remarks
                                            guestDetails.RoomTypeCode = ClsGuestInfo(k).RoomTypeCode
                                            guestDetails.rowkey = ClsGuestInfo(k).rowkey
                                            guestDetails.ArrAirport = ClsGuestInfo(k).ArrAirport.ToUpper()
                                            guestDetails.ArrFlightNo = ClsGuestInfo(k).ArrFlightNo
                                            guestDetails.ArrTime = ClsGuestInfo(k).ArrTime
                                            guestDetails.DepAirport = ClsGuestInfo(k).DepAirport.ToUpper()
                                            guestDetails.DepFlightNo = ClsGuestInfo(k).DepFlightNo
                                            guestDetails.DepTime = ClsGuestInfo(k).DepTime
                                            guestDetails.Remarks = ClsGuestInfo(k).Remarks.ToUpper()
                                            guestDetails.DepartureRemarks = ClsGuestInfo(k).DepartureRemarks.ToUpper()
                                            'listOfGuestDetails.Add(guestDetails)
                                            Exit For
                                        End If
                                    Next
                                    ClsBookRoom(j).GuestDetails = guestDetails

                                    If Not ClsAdultChildInfo Is Nothing Then
                                        For l = 0 To ClsAdultChildInfo.Count - 1
                                            If ClsBookRoom(j).RoomTypeCode.Equals(ClsAdultChildInfo(l).rowkey.Split(",")(2)) And ClsBookRoom(j).mealcode.Equals(ClsAdultChildInfo(l).rowkey.Split(",")(3)) And ClsBookRoom(j).optionno.Equals(ClsAdultChildInfo(l).rowkey.Split(",")(1)) And ClsBookRoom(j).Rmcatcode.Equals(ClsAdultChildInfo(l).rowkey.Split(",")(4)) Then
                                                For ll = 0 To ClsBookRoom(j).ListOfAdultAndChildDetails.Count - 1
                                                    If ClsBookRoom(j).ListOfAdultAndChildDetails(0).rowkey.Split(",")(2).Equals(ClsAdultChildInfo(l).rowkey.Split(",")(2)) And ClsBookRoom(j).ListOfAdultAndChildDetails(0).rowkey.Split(",")(3).Equals(ClsAdultChildInfo(l).rowkey.Split(",")(3)) And ClsBookRoom(j).ListOfAdultAndChildDetails(0).rowkey.Split(",")(4).Equals(ClsAdultChildInfo(l).rowkey.Split(",")(4)) Then
                                                        ClsBookRoom(j).ListOfAdultAndChildDetails(ll).FirstName = ClsAdultChildInfo(l).FirstName.ToUpper()
                                                        ClsBookRoom(j).ListOfAdultAndChildDetails(ll).LastName = ClsAdultChildInfo(l).LastName.ToUpper()
                                                        ClsBookRoom(j).ListOfAdultAndChildDetails(ll).Age = ClsAdultChildInfo(l).Age
                                                    End If
                                                Next
                                                Exit For
                                            End If
                                        Next
                                    End If

                                Next
                            End If
                        Next
                    End If
                End If

                Session("BookForHotel") = bookForHotel

                Dim obj As New StringWriter()
                Dim xmlstring As String

                Dim strHotelLineNoString As String = objUtils.GetString(Session("dbconnectionName"), "select replace((select ' ' + hotellinenostring from reservation_online_mainhoteltemp(nolock) where requestid='" & searchCriteria.requestid & "' and mainhotellineno =" & CType(searchCriteria.hotellineno, Integer) & "  for xml path('')),' ','')")
                Dim strArrayOfHotelLineNo As New List(Of String)
                strArrayOfHotelLineNo.AddRange(strHotelLineNoString.Trim().Split(";"))

                'commented
                Dim NewclsGuestInfo As New List(Of clsAgentsOnline.BookNow.GuestInfo)
                For existingGuest = 0 To ClsGuestInfo.Count - 1
                    If ClsGuestInfo(existingGuest).hotellineno.ToString().Equals(searchCriteria.hotellineno.ToString()) Then
                        'clsGuestInfo(existingGuest).HotelLineNoString = strArrayOfHotelLineNo(existingGuest)
                        NewclsGuestInfo.Add(ClsGuestInfo(existingGuest))
                    End If
                Next

                For i = 0 To NewclsGuestInfo.Count - 1
                    NewclsGuestInfo(i).HotelLineNoString = strArrayOfHotelLineNo(i)
                Next

                Dim x As New XmlSerializer(NewclsGuestInfo.GetType)
                x.Serialize(obj, NewclsGuestInfo)
                xmlstring = obj.ToString()
                xmlstring = Mid(xmlstring, xmlstring.IndexOf("<ArrayOfGuestInfo"))


                ClsAdultChildInfo = Session("AdultChildInfo")
                Dim objadt As New StringWriter()
                Dim xmladtstring As String

                Dim NewClsAdultChildInfo As New List(Of clsAgentsOnline.BookNow.GuestInfo.AdultChildlts)
                If Not ClsAdultChildInfo Is Nothing Then
                    For existingACDetails = 0 To ClsAdultChildInfo.Count - 1
                        If ClsAdultChildInfo(existingACDetails).hotellineno.ToString().Equals(searchCriteria.hotellineno.ToString()) Then
                            NewClsAdultChildInfo.Add(ClsAdultChildInfo(existingACDetails))
                        End If
                    Next
                End If
                Dim xadt As New XmlSerializer(NewClsAdultChildInfo.GetType)
                xadt.Serialize(objadt, NewClsAdultChildInfo)
                xmladtstring = objadt.ToString()
                xmladtstring = Mid(xmladtstring, xmladtstring.IndexOf("<ArrayOfAdultChildlts"))


                Dim parms10 As New List(Of SqlParameter)
                Dim parm10(5) As SqlParameter
                mySqlCmd = New SqlCommand
                mySqlCmd.CommandText = "sp_add_reservation_online_hotels_guesttemp_xml_online"
                mySqlCmd.Connection = mySqlConn
                mySqlCmd.Transaction = sqlTrans
                mySqlCmd.CommandType = CommandType.StoredProcedure
                parm10(0) = New SqlParameter("@requestid", CType(searchCriteria.requestid, String))
                'parm10(0) = New SqlParameter("@requestid", CType(Session("requestid"), String))
                parm10(1) = New SqlParameter("@mainhotellineno", CType(searchCriteria.hotellineno, Integer))
                parm10(2) = New SqlParameter("@guestinfoxml", xmlstring)
                parm10(3) = New SqlParameter("@adultchildxml", xmladtstring)
                parm10(4) = New SqlParameter("@basketid", CType(searchCriteria.basketid, Integer))
                'parm10(4) = New SqlParameter("@basketid", CType(Val(Session("BasketId")), Integer))
                For p = 0 To 4
                    mySqlCmd.Parameters.Add(parm10(p))
                Next

                mySqlCmd.ExecuteNonQuery()

                'Dim parms11 As New List(Of SqlParameter)
                'Dim parm11(2) As SqlParameter
                'mySqlCmd = New SqlCommand
                'mySqlCmd.CommandText = "sp_save_hotel_online_rmtypes"
                'mySqlCmd.Connection = mySqlConn
                'mySqlCmd.Transaction = sqlTrans
                'mySqlCmd.CommandType = CommandType.StoredProcedure
                'parm11(0) = New SqlParameter("@requestid", CType(searchCriteria.requestid, String))
                'parm11(1) = New SqlParameter("@basketid", CType(searchCriteria.basketid, Integer))
                '        'parm10(4) = New SqlParameter("@basketid", CType(Val(Session("BasketId")), Integer))
                'For m = 0 To 1
                '    mySqlCmd.Parameters.Add(parm11(m))
                'Next

                'mySqlCmd.ExecuteNonQuery()

                Dim parmClientRef(2) As SqlParameter
                mySqlCmd = New SqlCommand
                mySqlCmd.CommandText = "sp_update_agentRef"
                mySqlCmd.Connection = mySqlConn
                mySqlCmd.Transaction = sqlTrans
                mySqlCmd.CommandType = CommandType.StoredProcedure
                parmClientRef(0) = New SqlParameter("@requestid", CType(searchCriteria.requestid, String))
                parmClientRef(1) = New SqlParameter("@agentref", CType(clientref, String))
                mySqlCmd.Parameters.Add(parmClientRef(0))
                mySqlCmd.Parameters.Add(parmClientRef(1))
                mySqlCmd.ExecuteNonQuery()

                sqlTrans.Commit() 'for testing
                Return True
            Catch ex As Exception
                If mySqlConn.State = ConnectionState.Open Then
                    sqlTrans.Rollback()
                    'calling javascript function on load to assign the values if guest details exists in the guestclass

                    'fnGuestdtls()
                End If
                Return False
            End Try
        End Function

        ' removing the hotellineno for the guestdetails
        <WebMethod()> _
        Public Function fn_RemoveGHotelLineNo(ByVal MyObj As clsAgentsOnline.BookNow.GuestInfo) As Boolean

            Dim searchCriteria As New SearchCreteria
            searchCriteria = Session("searchCreteria")
            If MyObj.hotellineno = searchCriteria.hotellineno Then
                Return True
            Else
                Return False
            End If
        End Function
        ' removing the hotellineno for the adultschild
        <WebMethod()> _
        Public Function fn_RemoveAcHotelLineNo(ByVal MyObj As clsAgentsOnline.BookNow.GuestInfo.AdultChildlts) As Boolean
            Dim searchCriteria As New SearchCreteria
            searchCriteria = Session("searchCreteria")
            If MyObj.hotellineno = searchCriteria.hotellineno Then
                Return True
            Else
                Return False
            End If
        End Function

        <WebMethod()> _
        Public Function GetCityCodeListnewtst(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As String()
            Dim retlist As New List(Of String)

            Dim sqlstr As String
            sqlstr = "select ltrim(rtrim(cityname)) cityname,citycode  from citymast where active=1 "
            If prefixText.Trim <> "" Then
                sqlstr = sqlstr + "and cityname like '" & prefixText & "%'"
            End If
            sqlstr = sqlstr + " order by citycode"
            'retlist = objUtils.FillclsMasternew("strDBConnection", sqlstr, True)
            'Return retlist

            Dim ds As New DataSet
            ds = objUtils.ExecuteQuerySqlnew("strDBConnection", sqlstr)
            Dim dt As DataTable = ds.Tables(0)
            Dim items As New List(Of String)(10)

            For i As Integer = 0 To dt.Rows.Count - 1
                Dim strName As String = dt.Rows(i)(0).ToString()
                items.Add(strName)
            Next
            Return items.ToArray()




        End Function


        <WebMethod()> _
        Public Function GetwebCityCodeListnew(ByVal constr As String, ByVal ctrycode As String) As clsMaster()
            Dim retlist As New List(Of clsMaster)
            Dim sqlstr As String
            sqlstr = "select ltrim(rtrim(id)) id,ltrim(rtrim(city)) city  from webcities  "
            If Trim(ctrycode) <> "" And Trim(UCase(ctrycode)) <> Trim(UCase("[Select]")) Then
                sqlstr = sqlstr + " where  country='" & Trim(ctrycode) & "'"
            End If
            sqlstr = sqlstr + " order by id"
            retlist = objUtils.FillclsMasternew(constr, sqlstr, True)
            Return retlist.ToArray()
        End Function
        <WebMethod()> _
        Public Function GetSellingCategoryCodeBySupplier(ByVal constr As String, ByVal SupplierCode As String) As String
            'Dim retlist As New List(Of clsMaster)
            Dim result As String
            Dim sqlstr As String
            sqlstr = "select ScatCode  from PartyMast where active=1 "
            If Trim(SupplierCode) <> "" And Trim(UCase(SupplierCode)) <> Trim(UCase("[Select]")) Then
                sqlstr = sqlstr + " and PartyCode='" & Trim(SupplierCode) & "'"
            End If
            sqlstr = sqlstr '+ " order by citycode"
            ' retlist = objUtils.FillclsMasternew(constr, sqlstr, True)
            result = objUtils.ExecuteQueryReturnStringValuenew(constr, sqlstr)
            If result = "" Or result = Nothing Then result = "[Select]"
            Return result
        End Function
        <WebMethod()> _
        Public Function GetCategoryCodeBySupplier(ByVal constr As String, ByVal SupplierCode As String) As String
            'Dim retlist As New List(Of clsMaster)
            Dim result As String
            Dim sqlstr As String
            sqlstr = "select catcode  from PartyMast where active=1 "
            If Trim(SupplierCode) <> "" And Trim(UCase(SupplierCode)) <> Trim(UCase("[Select]")) Then
                sqlstr = sqlstr + " and PartyCode='" & Trim(SupplierCode) & "'"
            End If
            sqlstr = sqlstr '+ " order by citycode"
            ' retlist = objUtils.FillclsMasternew(constr, sqlstr, True)
            result = objUtils.ExecuteQueryReturnStringValuenew(constr, sqlstr)
            If result = "" Or result = Nothing Then result = "[Select]"
            Return result
        End Function
        ' function for returning the flight terminal name
        <WebMethod(EnableSession:=True)> _
        Public Function GetFlightTerminal(ByVal constr As String, ByVal airportCode As String) As String
            'Dim retlist As New List(Of clsMaster)
            Dim result As String
            Dim sqlstr As String
            sqlstr = "select airportbordername from airportbordersmaster where airportbordercode='" & airportCode & "'"
            result = objUtils.ExecuteQueryReturnStringValuenew(constr, sqlstr)
            If result = "" Or result = Nothing Then result = ""
            Return result

        End Function

        <WebMethod()> _
        Public Function GetCityNameListnew(ByVal constr As String, ByVal ctrycode As String) As clsMaster()
            Dim retlist As New List(Of clsMaster)
            Dim sqlstr As String
            sqlstr = "select ltrim(rtrim(cityname)) cityname,ltrim(rtrim(citycode)) citycode  from citymast where active=1 "
            If Trim(ctrycode) <> "" And Trim(UCase(ctrycode)) <> Trim(UCase("[Select]")) Then
                sqlstr = sqlstr + " and ctrycode='" & Trim(ctrycode) & "'"
            End If
            sqlstr = sqlstr + " order by cityname"
            retlist = objUtils.FillclsMasternew(constr, sqlstr, True)
            Return retlist.ToArray()
        End Function


        <WebMethod()> _
        Public Function GetwebCityNameListnew(ByVal constr As String, ByVal ctrycode As String) As clsMaster()
            Dim retlist As New List(Of clsMaster)
            Dim sqlstr As String
            sqlstr = "select ltrim(rtrim(city)) city,ltrim(rtrim(id)) id  from webcities  "
            If Trim(ctrycode) <> "" And Trim(UCase(ctrycode)) <> Trim(UCase("[Select]")) Then
                sqlstr = sqlstr + " where country='" & Trim(ctrycode) & "'"
            End If
            sqlstr = sqlstr + " order by city"
            retlist = objUtils.FillclsMasternew(constr, sqlstr, True)
            Return retlist.ToArray()
        End Function


        <WebMethod()> _
        Public Function Getlblcurrnew(ByVal constr As String, ByVal sellcode As String) As String
            Dim result As String
            Dim sqlstr As String
            sqlstr = "select currcode from sellmast where sellcode='" & Trim(sellcode) & "'"
            result = objUtils.ExecuteQueryReturnStringValuenew(constr, sqlstr)


            Return result
        End Function
        <WebMethod()> _
        Public Function Getcurrnew(ByVal constr As String, ByVal partycode As String) As String
            Dim result As String
            Dim sqlstr As String
            sqlstr = "select currcode from partymast where partycode='" & Trim(partycode) & "'"
            result = objUtils.ExecuteQueryReturnStringValuenew(constr, sqlstr)

            Return result
        End Function
        <WebMethod()> _
        Public Function GetCurr4Reservation(ByVal constr As String, ByVal custcode As String) As String
            Dim result As String
            Dim sqlstr As String
            sqlstr = "SELECT agentmast.currcode FROM agentmast INNER JOIN currrates ON "
            sqlstr += "agentmast.currcode = currrates.currcode WHERE tocurr=(select option_selected from reservation_parameters where param_id=457) "
            If Trim(custcode) <> "" And Trim(UCase(custcode)) <> Trim(UCase("[Select]")) Then
                sqlstr = sqlstr + " and agentcode='" & Trim(custcode) & "'"
            End If
            result = objUtils.ExecuteQueryReturnStringValuenew(constr, sqlstr)
            Return result
        End Function
        <WebMethod()> _
        Public Function GetExchRate4Reservation(ByVal constr As String, ByVal custcode As String) As String
            Dim result As String
            Dim sqlstr As String
            sqlstr = "SELECT currrates.convrate FROM agentmast INNER JOIN currrates ON "
            sqlstr += "agentmast.currcode = currrates.currcode WHERE tocurr=(select option_selected from reservation_parameters where param_id=457) "
            If Trim(custcode) <> "" And Trim(UCase(custcode)) <> Trim(UCase("[Select]")) Then
                sqlstr = sqlstr + " and agentcode='" & Trim(custcode) & "'"
            End If
            result = objUtils.ExecuteQueryReturnStringValuenew(constr, sqlstr)
            Return result
        End Function

        <WebMethod()> _
        Public Function getconceirgedet(ByVal constr As String, ByVal custcode As String) As String
            Dim result As String
            Dim sqlstr As String
            sqlstr = "SELECT conspcode  FROM agentmast  "
            sqlstr += "WHERE  "
            If Trim(custcode) <> "" And Trim(UCase(custcode)) <> Trim(UCase("[Select]")) Then
                sqlstr = sqlstr + "  agentcode='" & Trim(custcode) & "'"
            End If
            result = objUtils.ExecuteQueryReturnStringValuenew(constr, sqlstr)
            Return result
        End Function

        <WebMethod()> _
        Public Function getsalesexpdet(ByVal constr As String, ByVal custcode As String) As String
            Dim result As String
            Dim sqlstr As String
            sqlstr = "SELECT sespcode  FROM agentmast  "
            sqlstr += "WHERE  "
            If Trim(custcode) <> "" And Trim(UCase(custcode)) <> Trim(UCase("[Select]")) Then
                sqlstr = sqlstr + "  agentcode='" & Trim(custcode) & "'"
            End If
            result = objUtils.ExecuteQueryReturnStringValuenew(constr, sqlstr)
            Return result
        End Function

        <WebMethod()> _
        Public Function getspersonname(ByVal constr As String, ByVal custcode As String) As String
            Dim result As String
            Dim sqlstr As String
            sqlstr = "SELECT spersoncode FROM agentmast where "
            If Trim(custcode) <> "" And Trim(UCase(custcode)) <> Trim(UCase("[Select]")) Then
                sqlstr = sqlstr + "  agentcode='" & Trim(custcode) & "'"
            End If
            result = objUtils.ExecuteQueryReturnStringValuenew(constr, sqlstr)
            Return result
        End Function

        <WebMethod()> _
        Public Function GetCatCodeListnew(ByVal constr As String, ByVal sptypecode As String) As clsMaster()
            Dim retlist As New List(Of clsMaster)
            Dim sqlstr As String
            sqlstr = "select catcode ,catname from catmast where active=1 "
            If Trim(sptypecode) <> "" And Trim(UCase(sptypecode)) <> Trim(UCase("[Select]")) Then
                sqlstr = sqlstr + " and sptypecode='" & Trim(sptypecode) & "'"
            End If
            sqlstr = sqlstr + " order by catcode"

            retlist = objUtils.FillclsMasternew(constr, sqlstr, True)
            Return retlist.ToArray()
        End Function


        <WebMethod()> _
        Public Function fillprices(ByVal constr As String, ByVal transferdate As String, ByVal othtypcode As String, ByVal othcatcode As String, ByVal requestid As String, ByVal hotellineno As String, ByVal rmtyplineno As String, ByVal rmcatcode As String, ByVal rmno As String, ByVal arrdeplineno As String, ByVal unitpax As String) As String()
            Dim retlist As Array
            Dim sqlstr As String
            Dim dat As Date = transferdate
            transferdate = Format(dat, "yyyy/MM/dd")

            sqlstr = "execute sp_get_transfer_prices" + " '" & transferdate & "','" & othtypcode & "','" & othcatcode & "','" & requestid & "','" & hotellineno & "','" & rmtyplineno & "','" & rmcatcode & "','" & rmno & "','" & arrdeplineno & "','" & unitpax & "'"
            Dim cnt As Long = 6

            retlist = objUtils.FillArraynew(constr, sqlstr, cnt)

            Return retlist
        End Function


        <WebMethod()> _
        Public Function fillpkgprices(ByVal constr As String, ByVal pkgid As String, ByVal othtypcode As String, ByVal othcatcode As String, ByVal requestid As String, ByVal hotellineno As String, ByVal rmtyplineno As String, ByVal rmcatcode As String, ByVal rmno As String, ByVal arrdeplineno As String, ByVal unitpax As String) As String()
            Dim retlist As Array
            Dim sqlstr As String

            sqlstr = "select 0, price,plistcode ,'',(select option_selected from reservation_parameters where  param_id=457) from packagehotel_others where packageid='" & CType(pkgid, String) & "' and othtypcode='" & othtypcode & "' and othcatcode='" & othcatcode & "' and includedinpkg=1"

            Dim cnt As Long = 5

            retlist = objUtils.FillArraynew(constr, sqlstr, cnt)

            Return retlist
        End Function

        <WebMethod()> _
        Public Function GetCatNameListnew(ByVal constr As String, ByVal sptypecode As String) As clsMaster()
            Dim retlist As New List(Of clsMaster)
            Dim sqlstr As String
            sqlstr = "select catname,catcode from catmast where active=1 "
            If Trim(sptypecode) <> "" And Trim(UCase(sptypecode)) <> Trim(UCase("[Select]")) Then
                sqlstr = sqlstr + " and sptypecode='" & Trim(sptypecode) & "'"
            End If
            sqlstr = sqlstr + " order by catname"

            retlist = objUtils.FillclsMasternew(constr, sqlstr, True)
            Return retlist.ToArray()
        End Function
        <WebMethod()> _
        Public Function GetSellCatCodeListnew(ByVal constr As String, ByVal sptypecode As String) As clsMaster()
            Dim retlist As New List(Of clsMaster)
            Dim sqlstr As String
            sqlstr = "select  scatcode,scatname from sellcatmast where active=1 "
            If Trim(sptypecode) <> "" And Trim(UCase(sptypecode)) <> Trim(UCase("[Select]")) Then
                sqlstr = sqlstr + " and sptypecode='" & Trim(sptypecode) & "'"
            End If
            sqlstr = sqlstr + " order by scatcode"

            retlist = objUtils.FillclsMasternew(constr, sqlstr, True)
            Return retlist.ToArray()
        End Function
        <WebMethod()> _
        Public Function GetSellCatNameListnew(ByVal constr As String, ByVal sptypecode As String) As clsMaster()
            Dim retlist As New List(Of clsMaster)
            Dim sqlstr As String
            sqlstr = "select scatname,scatcode from sellcatmast where active=1 "
            If Trim(sptypecode) <> "" And Trim(UCase(sptypecode)) <> Trim(UCase("[Select]")) Then
                sqlstr = sqlstr + " and sptypecode='" & Trim(sptypecode) & "'"
            End If
            sqlstr = sqlstr + " order by scatname"

            retlist = objUtils.FillclsMasternew(constr, sqlstr, True)
            Return retlist.ToArray()
        End Function


        <WebMethod()> _
        Public Function GetSellCatScatCodeList(ByVal constr As String, ByVal sptypecodeid As String, ByVal catcodeid As String, ByVal scatcodeid As String) As clsMaster()
            Dim retlist As New List(Of clsMaster)
            Dim sqlstr As String
            sqlstr = "select  partycode,partyname from partymast where active=1 "

            If Trim(sptypecodeid) <> "" And Trim(UCase(sptypecodeid)) <> Trim(UCase("[Select]")) Then
                sqlstr = sqlstr + " and sptypecode='" & Trim(sptypecodeid) & "'"
            End If
            If Trim(catcodeid) <> "" And Trim(UCase(catcodeid)) <> Trim(UCase("[Select]")) Then
                sqlstr = sqlstr + " and catcode='" & Trim(catcodeid) & "'"
            End If
            If Trim(scatcodeid) <> "" And Trim(UCase(scatcodeid)) <> Trim(UCase("[Select]")) Then
                sqlstr = sqlstr + " and scatcode='" & Trim(scatcodeid) & "'"
            End If

            sqlstr = sqlstr + " order by partycode"

            retlist = objUtils.FillclsMasternew(constr, sqlstr, True)
            Return retlist.ToArray()
        End Function
        <WebMethod()> _
        Public Function GetSellCatScatNameList(ByVal constr As String, ByVal sptypecodeid As String, ByVal catcodeid As String, ByVal scatcodeid As String) As clsMaster()
            Dim retlist As New List(Of clsMaster)
            Dim sqlstr As String
            sqlstr = "select  partyname,partycode from partymast where active=1 "

            If Trim(sptypecodeid) <> "" And Trim(UCase(sptypecodeid)) <> Trim(UCase("[Select]")) Then
                sqlstr = sqlstr + " and sptypecode='" & Trim(sptypecodeid) & "'"
            End If
            If Trim(catcodeid) <> "" And Trim(UCase(catcodeid)) <> Trim(UCase("[Select]")) Then
                sqlstr = sqlstr + " and catcode='" & Trim(catcodeid) & "'"
            End If
            If Trim(scatcodeid) <> "" And Trim(UCase(scatcodeid)) <> Trim(UCase("[Select]")) Then
                sqlstr = sqlstr + " and scatcode='" & Trim(scatcodeid) & "'"
            End If

            sqlstr = sqlstr + " order by partyname"


            retlist = objUtils.FillclsMasternew(constr, sqlstr, True)
            Return retlist.ToArray()
        End Function


        <WebMethod()> _
        Public Function GetSectorCodeCtryListnew(ByVal constr As String, ByVal ctrycode As String) As clsMaster()
            Dim retlist As New List(Of clsMaster)
            Dim sqlstr As String
            sqlstr = "select sectorcode ,sectorname from sectormaster where active=1 "
            If Trim(ctrycode) <> "" And Trim(UCase(ctrycode)) <> Trim(UCase("[Select]")) Then
                sqlstr = sqlstr + " and ctrycode='" & Trim(ctrycode) & "'"
            End If
            sqlstr = sqlstr + " order by sectorcode"
            retlist = objUtils.FillclsMasternew(constr, sqlstr, True)
            Return retlist.ToArray()
        End Function
        <WebMethod()> _
        Public Function GetSectorNameCtryListnew(ByVal constr As String, ByVal ctrycode As String) As clsMaster()
            Dim retlist As New List(Of clsMaster)
            Dim sqlstr As String
            sqlstr = "select sectorname,sectorcode from sectormaster where active=1 "
            If Trim(ctrycode) <> "" And Trim(UCase(ctrycode)) <> Trim(UCase("[Select]")) Then
                sqlstr = sqlstr + " and ctrycode='" & Trim(ctrycode) & "'"
            End If
            sqlstr = sqlstr + " order by sectorname"
            retlist = objUtils.FillclsMasternew(constr, sqlstr, True)
            Return retlist.ToArray()
        End Function
        <WebMethod()> _
        Public Function GetSectorCodeListnew(ByVal constr As String, ByVal ctrycode As String, ByVal citycode As String) As clsMaster()
            Dim retlist As New List(Of clsMaster)
            Dim sqlstr As String
            sqlstr = "select sectorcode ,sectorname from sectormaster where active=1 "
            If Trim(ctrycode) <> "" And Trim(UCase(ctrycode)) <> Trim(UCase("[Select]")) Then
                sqlstr = sqlstr + " and ctrycode='" & Trim(ctrycode) & "'"
            End If
            If Trim(citycode) <> "" And Trim(UCase(citycode)) <> Trim(UCase("[Select]")) Then
                sqlstr = sqlstr + " and citycode='" & Trim(citycode) & "'"
            End If
            sqlstr = sqlstr + " order by sectorcode"

            retlist = objUtils.FillclsMasternew(constr, sqlstr, True)
            Return retlist.ToArray()
        End Function
        <WebMethod()> _
        Public Function GetSectorNameListnew(ByVal constr As String, ByVal ctrycode As String, ByVal citycode As String) As clsMaster()
            Dim retlist As New List(Of clsMaster)
            Dim sqlstr As String
            sqlstr = "select sectorname,sectorcode from sectormaster where active=1 "
            If Trim(ctrycode) <> "" And Trim(UCase(ctrycode)) <> Trim(UCase("[Select]")) Then
                sqlstr = sqlstr + " and ctrycode='" & Trim(ctrycode) & "'"
            End If
            If Trim(citycode) <> "" And Trim(UCase(citycode)) <> Trim(UCase("[Select]")) Then
                sqlstr = sqlstr + " and citycode='" & Trim(citycode) & "'"
            End If
            sqlstr = sqlstr + " order by sectorname"
            retlist = objUtils.FillclsMasternew(constr, sqlstr, True)
            Return retlist.ToArray()
        End Function

        <WebMethod()> _
        Public Function GetPartyPostCodeListnew(ByVal constr As String, ByVal ctrycode As String, ByVal citycode As String, ByVal partycode As String) As clsMaster()
            Dim retlist As New List(Of clsMaster)
            Dim sqlstr As String
            sqlstr = "select partycode,partyname from partymast where active=1 "
            If Trim(ctrycode) <> "" And Trim(UCase(ctrycode)) <> Trim(UCase("[Select]")) Then
                sqlstr = sqlstr + " and ctrycode='" & Trim(ctrycode) & "'"
            End If
            If Trim(citycode) <> "" And Trim(UCase(citycode)) <> Trim(UCase("[Select]")) Then
                sqlstr = sqlstr + " and citycode='" & Trim(citycode) & "'"
            End If
            If Trim(partycode) <> "" And Trim(UCase(partycode)) <> Trim(UCase("[Select]")) Then
                sqlstr = sqlstr + " and partycode <>'" & Trim(partycode) & "'"
            End If
            sqlstr = sqlstr + " order by partycode "

            retlist = objUtils.FillclsMasternew(constr, sqlstr, True)
            Return retlist.ToArray()
        End Function
        <WebMethod()> _
        Public Function GetPartyPostNameListnew(ByVal constr As String, ByVal ctrycode As String, ByVal citycode As String, ByVal partycode As String) As clsMaster()
            Dim retlist As New List(Of clsMaster)
            Dim sqlstr As String
            sqlstr = "select partyname, partycode  from partymast where active=1 "
            If Trim(ctrycode) <> "" And Trim(UCase(ctrycode)) <> Trim(UCase("[Select]")) Then
                sqlstr = sqlstr + " and ctrycode='" & Trim(ctrycode) & "'"
            End If
            If Trim(citycode) <> "" And Trim(UCase(citycode)) <> Trim(UCase("[Select]")) Then
                sqlstr = sqlstr + " and citycode='" & Trim(citycode) & "'"
            End If
            If Trim(partycode) <> "" And Trim(UCase(partycode)) <> Trim(UCase("[Select]")) Then
                sqlstr = sqlstr + " and partycode <>'" & Trim(partycode) & "'"
            End If
            sqlstr = sqlstr + " order by partyname "
            retlist = objUtils.FillclsMasternew(constr, sqlstr, True)
            Return retlist.ToArray()
        End Function
        <WebMethod()> _
        Public Function GetSupplierCodeListnew(ByVal connstr As String, ByVal sptypecode As String) As clsMaster()
            Dim retlist As New List(Of clsMaster)
            Dim sqlstr As String
            sqlstr = "select partycode,partyname from partymast where active=1"
            If Trim(sptypecode) <> "" And Trim(UCase(sptypecode)) <> Trim(UCase("[Select]")) Then
                sqlstr = sqlstr + " and sptypecode='" & Trim(sptypecode) & "'"
            End If
            sqlstr = sqlstr + " order by partycode"
            retlist = objUtils.FillclsMasternew(connstr, sqlstr, True)
            Return retlist.ToArray()
        End Function
        <WebMethod()> _
        Public Function GetSupplierNameListnew(ByVal connstr As String, ByVal sptypecode As String) As clsMaster()
            Dim retlist As New List(Of clsMaster)
            Dim sqlstr As String
            sqlstr = "select partyname,partycode  from partymast where active=1"
            If Trim(sptypecode) <> "" And Trim(UCase(sptypecode)) <> Trim(UCase("[Select]")) Then
                sqlstr = sqlstr + " and sptypecode='" & Trim(sptypecode) & "'"
            End If
            sqlstr = sqlstr + " order by partyname"
            retlist = objUtils.FillclsMasternew(connstr, sqlstr, True)
            Return retlist.ToArray()
        End Function
        <WebMethod()> _
        Public Function GetSupplierCodeAllListnew(ByVal constr As String, ByVal sptypecode As String, ByVal catcode As String, ByVal sellcatcode As String, ByVal ctrycode As String, ByVal citycode As String) As clsMaster()
            Dim retlist As New List(Of clsMaster)
            Dim sqlstr As String
            sqlstr = "select partycode ,partyname from partymast where active=1 "
            If Trim(sptypecode) <> "" And Trim(UCase(sptypecode)) <> Trim(UCase("[Select]")) Then
                sqlstr = sqlstr + " and sptypecode='" & Trim(sptypecode) & "'"
            End If
            If Trim(catcode) <> "" And Trim(UCase(catcode)) <> Trim(UCase("[Select]")) Then
                sqlstr = sqlstr + " and catcode='" & Trim(catcode) & "'"
            End If

            If Trim(sellcatcode) <> "" And Trim(UCase(sellcatcode)) <> Trim(UCase("[Select]")) Then
                sqlstr = sqlstr + " and scatcode='" & Trim(sellcatcode) & "'"
            End If
            If Trim(ctrycode) <> "" And Trim(UCase(ctrycode)) <> Trim(UCase("[Select]")) Then
                sqlstr = sqlstr + " and ctrycode='" & Trim(ctrycode) & "'"
            End If
            If Trim(citycode) <> "" And Trim(UCase(citycode)) <> Trim(UCase("[Select]")) Then
                sqlstr = sqlstr + " and citycode='" & Trim(citycode) & "'"
            End If
            sqlstr = sqlstr + " order by partycode "
            retlist = objUtils.FillclsMasternew(constr, sqlstr, True)
            Return retlist.ToArray()
        End Function
        <WebMethod()> _
        Public Function GetSupplierNameAllListnew(ByVal constr As String, ByVal sptypecode As String, ByVal catcode As String, ByVal sellcatcode As String, ByVal ctrycode As String, ByVal citycode As String) As clsMaster()
            Dim retlist As New List(Of clsMaster)
            Dim sqlstr As String
            sqlstr = "select partyname, partycode from partymast where active=1 "
            If Trim(sptypecode) <> "" And Trim(UCase(sptypecode)) <> Trim(UCase("[Select]")) Then
                sqlstr = sqlstr + " and sptypecode='" & Trim(sptypecode) & "'"
            End If
            If Trim(catcode) <> "" And Trim(UCase(catcode)) <> Trim(UCase("[Select]")) Then
                sqlstr = sqlstr + " and catcode='" & Trim(catcode) & "'"
            End If

            If Trim(sellcatcode) <> "" And Trim(UCase(sellcatcode)) <> Trim(UCase("[Select]")) Then
                sqlstr = sqlstr + " and scatcode='" & Trim(sellcatcode) & "'"
            End If
            If Trim(ctrycode) <> "" And Trim(UCase(ctrycode)) <> Trim(UCase("[Select]")) Then
                sqlstr = sqlstr + " and ctrycode='" & Trim(ctrycode) & "'"
            End If
            If Trim(citycode) <> "" And Trim(UCase(citycode)) <> Trim(UCase("[Select]")) Then
                sqlstr = sqlstr + " and citycode='" & Trim(citycode) & "'"
            End If
            sqlstr = sqlstr + " order by partyname "

            retlist = objUtils.FillclsMasternew(constr, sqlstr, True)
            Return retlist.ToArray()
        End Function
        <WebMethod()> _
        Public Function GetSuppAgentCodeListnew(ByVal constr As String, ByVal sptypecode As String) As clsMaster()
            Dim retlist As New List(Of clsMaster)
            Dim sqlstr As String
            sqlstr = "select  supagentcode,supagentname  from  supplier_agents   where active=1"
            If Trim(sptypecode) <> "" And Trim(UCase(sptypecode)) <> Trim(UCase("[Select]")) Then
                sqlstr = sqlstr + " and sptypecode='" & Trim(sptypecode) & "'"
            End If
            sqlstr = sqlstr + " order by supagentcode "

            retlist = objUtils.FillclsMasternew(constr, sqlstr, True)
            Return retlist.ToArray()
        End Function
        <WebMethod()> _
        Public Function GetSuppAgentNameListnew(ByVal constr As String, ByVal sptypecode As String) As clsMaster()
            Dim retlist As New List(Of clsMaster)
            Dim sqlstr As String
            sqlstr = "select  supagentname,supagentcode  from  supplier_agents   where active=1"
            If Trim(sptypecode) <> "" And Trim(UCase(sptypecode)) <> Trim(UCase("[Select]")) Then
                sqlstr = sqlstr + " and sptypecode='" & Trim(sptypecode) & "'"
            End If
            sqlstr = sqlstr + " order by supagentname "

            retlist = objUtils.FillclsMasternew(constr, sqlstr, True)
            Return retlist.ToArray()
        End Function
        <WebMethod()> _
        Public Function GetSellCurrCodeListnew(ByVal constr As String, ByVal sellcode As String) As clsMaster()
            Dim retlist As New List(Of clsMaster)
            Dim sqlstr As String
            sqlstr = "select Distinct  currmast.currcode,currmast.currname  from  currmast inner join sellmast on currmast.currcode=sellmast.currcode   where currmast.active=1 "

            If Trim(sellcode) <> "" And Trim(UCase(sellcode)) <> Trim(UCase("[Select]")) Then
                sqlstr = sqlstr + "and sellmast.sellcode='" & sellcode & "'"
            End If
            sqlstr = sqlstr + " order by currmast.currcode "
            retlist = objUtils.FillclsMasternew(constr, sqlstr, True)
            Return retlist.ToArray()
        End Function
        <WebMethod()> _
        Public Function GetSellCurrNameListnew(ByVal constr As String, ByVal sellcode As String) As clsMaster()
            Dim retlist As New List(Of clsMaster)
            Dim sqlstr As String
            sqlstr = "select  Distinct currmast.currname,currmast.currcode  from  currmast inner join sellmast on currmast.currcode=sellmast.currcode   where currmast.active=1 "

            If Trim(sellcode) <> "" And Trim(UCase(sellcode)) <> Trim(UCase("[Select]")) Then
                sqlstr = sqlstr + "and sellmast.sellcode='" & sellcode & "'"
            End If
            sqlstr = sqlstr + " order by currmast.currname "
            retlist = objUtils.FillclsMasternew(constr, sqlstr, True)
            Return retlist.ToArray()
        End Function
        <WebMethod()> _
        Public Function GettrfsellcodefrmCurrCode(ByVal constr As String, ByVal currcode As String) As clsMaster()
            Dim retlist As New List(Of clsMaster)
            Dim sqlstr As String
            sqlstr = "select trfsellcode,trfsellname  from  trfsellmast  where active=1 "
            If Trim(currcode) <> "" And Trim(UCase(currcode)) <> Trim(UCase("[Select]")) Then
                sqlstr = sqlstr + "and trfsellmast.currcode='" & currcode & "'"
            End If
            sqlstr = sqlstr + " order by trfsellmast.trfsellcode "
            retlist = objUtils.FillclsMasternew(constr, sqlstr, True)
            Return retlist.ToArray()
        End Function
        <WebMethod()> _
        Public Function GettrfsellnamefrmCurrName(ByVal constr As String, ByVal currname As String) As clsMaster()
            Dim retlist As New List(Of clsMaster)
            Dim sqlstr As String
            sqlstr = "select  trfsellname,trfsellcode from trfsellmast   where active=1 "

            If Trim(currname) <> "" And Trim(UCase(currname)) <> Trim(UCase("[Select]")) Then
                sqlstr = sqlstr + "and trfsellmast.currcode='" & currname & "'"
            End If
            sqlstr = sqlstr + " order by trfsellmast.trfsellname "
            retlist = objUtils.FillclsMasternew(constr, sqlstr, True)
            Return retlist.ToArray()
        End Function

        <WebMethod()> _
        Public Function GetexcsellcodefrmCurrCode(ByVal constr As String, ByVal currcode As String) As clsMaster()
            Dim retlist As New List(Of clsMaster)
            Dim sqlstr As String
            sqlstr = "select excsellcode,excsellname  from  excsellmast  where active=1 "
            If Trim(currcode) <> "" And Trim(UCase(currcode)) <> Trim(UCase("[Select]")) Then
                sqlstr = sqlstr + "and excsellmast.currcode='" & currcode & "'"
            End If
            sqlstr = sqlstr + " order by excsellmast.excsellcode "
            retlist = objUtils.FillclsMasternew(constr, sqlstr, True)
            Return retlist.ToArray()
        End Function
        <WebMethod()> _
        Public Function GetexcsellnamefrmCurrName(ByVal constr As String, ByVal currname As String) As clsMaster()
            Dim retlist As New List(Of clsMaster)
            Dim sqlstr As String
            sqlstr = "select  excsellname,excsellcode from excsellmast   where active=1 "

            If Trim(currname) <> "" And Trim(UCase(currname)) <> Trim(UCase("[Select]")) Then
                sqlstr = sqlstr + "and excsellmast.currcode='" & currname & "'"
            End If
            sqlstr = sqlstr + " order by excsellmast.excsellname "
            retlist = objUtils.FillclsMasternew(constr, sqlstr, True)
            Return retlist.ToArray()
        End Function

        <WebMethod()> _
        Public Function GetCustomerCodeAllListnew(ByVal constr As String, ByVal catcode As String, ByVal sellcode As String, ByVal othsellcode As String, ByVal tkthsellcode As String, ByVal ctrycode As String, ByVal citycode As String, ByVal plgrpcode As String) As clsMaster()
            Dim retlist As New List(Of clsMaster)
            Dim sqlstr As String
            sqlstr = "select agentcode ,agentname from agentmast where active=1 "

            If Trim(catcode) <> "" And Trim(UCase(catcode)) <> Trim(UCase("[Select]")) Then
                sqlstr = sqlstr + " and catcode='" & Trim(catcode) & "'"
            End If
            If Trim(sellcode) <> "" And Trim(UCase(sellcode)) <> Trim(UCase("[Select]")) Then
                sqlstr = sqlstr + " and sellcode='" & Trim(sellcode) & "'"
            End If
            If Trim(othsellcode) <> "" And Trim(UCase(othsellcode)) <> Trim(UCase("[Select]")) Then
                sqlstr = sqlstr + " and othsellcode='" & Trim(othsellcode) & "'"
            End If
            If Trim(tkthsellcode) <> "" And Trim(UCase(tkthsellcode)) <> Trim(UCase("[Select]")) Then
                sqlstr = sqlstr + " and tktsellcode='" & Trim(tkthsellcode) & "'"
            End If
            If Trim(ctrycode) <> "" And Trim(UCase(ctrycode)) <> Trim(UCase("[Select]")) Then
                sqlstr = sqlstr + " and ctrycode='" & Trim(ctrycode) & "'"
            End If
            If Trim(citycode) <> "" And Trim(UCase(citycode)) <> Trim(UCase("[Select]")) Then
                sqlstr = sqlstr + " and citycode='" & Trim(citycode) & "'"
            End If
            If Trim(plgrpcode) <> "" And Trim(UCase(plgrpcode)) <> Trim(UCase("[Select]")) Then
                sqlstr = sqlstr + " and plgrpcode='" & Trim(plgrpcode) & "'"
            End If
            sqlstr = sqlstr + " order by agentcode "
            retlist = objUtils.FillclsMasternew(constr, sqlstr, True)
            Return retlist.ToArray()
        End Function
        <WebMethod()> _
        Public Function GetCustomerNameAllListnew(ByVal constr As String, ByVal catcode As String, ByVal sellcode As String, ByVal othsellcode As String, ByVal tkthsellcode As String, ByVal ctrycode As String, ByVal citycode As String, ByVal plgrpcode As String) As clsMaster()
            Dim retlist As New List(Of clsMaster)
            Dim sqlstr As String
            sqlstr = "select agentname, agentcode  from agentmast where active=1 "

            If Trim(catcode) <> "" And Trim(UCase(catcode)) <> Trim(UCase("[Select]")) Then
                sqlstr = sqlstr + " and catcode='" & Trim(catcode) & "'"
            End If
            If Trim(sellcode) <> "" And Trim(UCase(sellcode)) <> Trim(UCase("[Select]")) Then
                sqlstr = sqlstr + " and sellcode='" & Trim(sellcode) & "'"
            End If
            If Trim(othsellcode) <> "" And Trim(UCase(othsellcode)) <> Trim(UCase("[Select]")) Then
                sqlstr = sqlstr + " and othsellcode='" & Trim(othsellcode) & "'"
            End If
            If Trim(tkthsellcode) <> "" And Trim(UCase(tkthsellcode)) <> Trim(UCase("[Select]")) Then
                sqlstr = sqlstr + " and tktsellcode='" & Trim(tkthsellcode) & "'"
            End If
            If Trim(ctrycode) <> "" And Trim(UCase(ctrycode)) <> Trim(UCase("[Select]")) Then
                sqlstr = sqlstr + " and ctrycode='" & Trim(ctrycode) & "'"
            End If
            If Trim(citycode) <> "" And Trim(UCase(citycode)) <> Trim(UCase("[Select]")) Then
                sqlstr = sqlstr + " and citycode='" & Trim(citycode) & "'"
            End If
            If Trim(plgrpcode) <> "" And Trim(UCase(plgrpcode)) <> Trim(UCase("[Select]")) Then
                sqlstr = sqlstr + " and plgrpcode='" & Trim(plgrpcode) & "'"
            End If

            sqlstr = sqlstr + " order by agentname "
            retlist = objUtils.FillclsMasternew(constr, sqlstr, True)
            Return retlist.ToArray()
        End Function
        <WebMethod()> _
        Public Function GetCustSectorCodeListnew(ByVal constr As String, ByVal ctrycode As String, ByVal plgrpcode As String) As clsMaster()
            Dim retlist As New List(Of clsMaster)
            Dim sqlstr As String
            sqlstr = "select sectorcode ,sectorname from agent_sectormaster where active=1 "
            If Trim(ctrycode) <> "" And Trim(UCase(ctrycode)) <> Trim(UCase("[Select]")) Then
                sqlstr = sqlstr + " and ctrycode='" & Trim(ctrycode) & "'"
            End If
            If Trim(plgrpcode) <> "" And Trim(UCase(plgrpcode)) <> Trim(UCase("[Select]")) Then
                sqlstr = sqlstr + " and plgrpcode='" & Trim(plgrpcode) & "'"
            End If
            sqlstr = sqlstr + " order by sectorcode"

            retlist = objUtils.FillclsMasternew(constr, sqlstr, True)
            Return retlist.ToArray()
        End Function
        <WebMethod()> _
        Public Function GetCustSectorNameListnew(ByVal constr As String, ByVal ctrycode As String, ByVal plgrpcode As String) As clsMaster()
            Dim retlist As New List(Of clsMaster)
            Dim sqlstr As String
            sqlstr = "select sectorname,sectorcode from agent_sectormaster where active=1 "
            If Trim(ctrycode) <> "" And Trim(UCase(ctrycode)) <> Trim(UCase("[Select]")) Then
                sqlstr = sqlstr + " and ctrycode='" & Trim(ctrycode) & "'"
            End If
            If Trim(plgrpcode) <> "" And Trim(UCase(plgrpcode)) <> Trim(UCase("[Select]")) Then
                sqlstr = sqlstr + " and plgrpcode='" & Trim(plgrpcode) & "'"
            End If
            sqlstr = sqlstr + " order by sectorname"
            retlist = objUtils.FillclsMasternew(constr, sqlstr, True)
            Return retlist.ToArray()
        End Function
        <WebMethod()> _
        Public Function Getemptranoldno(ByVal connstr As String, ByVal empcode As String, ByVal tran_type As String) As String
            Dim retoldno As String = ""
            retoldno = CType(objUtils.ExecuteQueryReturnSingleValuenew(connstr, "select emp_tran.tran_newno  from  emp_tran,emp_master where emp_tran.tran_empno=emp_master.emp_code and isnull(emp_master.current_status,'')<>'G' and emp_tran.tran_type='" & tran_type & "'" _
           & " and emp_master.emp_code='" & empcode & "'"), String)
            Return retoldno
        End Function
        <WebMethod()> _
        Public Function Getemptrandate(ByVal connstr As String, ByVal empcode As String, ByVal tran_type As String) As String
            Dim retolddate As String = ""
            retolddate = CType(objUtils.ExecuteQueryReturnSingleValuenew(connstr, "select convert(varchar(10),emp_tran.tran_rejoin,103) as tran_rejoin  from  emp_tran,emp_master where emp_tran.tran_empno=emp_master.emp_code and isnull(emp_master.current_status,'')<>'G' and emp_tran.tran_type='" & tran_type & "'" _
           & " and emp_master.emp_code='" & empcode & "'"), String)
            Return retolddate
        End Function
        <WebMethod(EnableSession:=True)> _
        Public Function GetPartyCurrCodenew(ByVal connstr As String, ByVal partycode As String) As String
            Dim retcurrcode As String = ""
            retcurrcode = CType(objUtils.GetDBFieldFromStringnew(Session("dbConnectionName"), "partymast", "currcode", "partycode", partycode), String)
            Return retcurrcode
        End Function

        <WebMethod()> _
        Public Function GetPartyCurrNamenew(ByVal connstr As String, ByVal partycode As String) As String
            Dim retcurrname As String = ""
            retcurrname = CType(objUtils.ExecuteQueryReturnSingleValuenew(connstr, "select c.currname from currmast c,partymast p where p.currcode=c.currcode " _
            & " and p.partycode='" & partycode & "'"), String)
            Return retcurrname
        End Function
        <WebMethod()> _
        Public Function GetSellingCurrCodenew(ByVal connstr As String, ByVal sellcode As String) As String
            Dim retcurrcode As String = ""
            retcurrcode = CType(objUtils.GetDBFieldFromStringnew(connstr, "trfsellmast", "currcode", "trfsellcode", sellcode), String)
            Return retcurrcode
        End Function

        <WebMethod()> _
        Public Function GetSellingCurrNamenew(ByVal connstr As String, ByVal sellcode As String) As String
            Dim retcurrname As String = ""
            retcurrname = CType(objUtils.ExecuteQueryReturnSingleValuenew(connstr, "select c.currname from currmast c, trfsellmast sc where c.currcode  = sc.currcode " _
            & " and sc.trfsellcode='" & sellcode & "'"), String)
            Return retcurrname
        End Function

        <WebMethod()> _
        Public Function GetSellingCurrCodeexc(ByVal connstr As String, ByVal sellcode As String) As String
            Dim retcurrcode As String = ""
            retcurrcode = CType(objUtils.GetDBFieldFromStringnew(connstr, "excsellmast", "currcode", "excsellcode", sellcode), String)
            Return retcurrcode
        End Function

        <WebMethod()> _
        Public Function GetSellingCurrNameexc(ByVal connstr As String, ByVal sellcode As String) As String
            Dim retcurrname As String = ""
            retcurrname = CType(objUtils.ExecuteQueryReturnSingleValuenew(connstr, "select c.currname from currmast c, excsellmast sc where c.currcode  = sc.currcode " _
            & " and sc.excsellname='" & sellcode & "'"), String)
            Return retcurrname
        End Function

        <WebMethod()> _
        Public Function GetSellingCurrCodecost(ByVal connstr As String, ByVal sellcode As String) As clsMaster()
            'Dim retcurrcode As String = ""
            'retcurrcode = CType(objUtils.GetDBFieldFromStringnew(connstr, "partymast", "currcode", "partycode", sellcode), String)
            'Return retcurrcode

            Dim retlist As New List(Of clsMaster)
            Dim sqlstr As String
            sqlstr = "select P.currcode,currname  from partymast P Inner Join Currmast C On C.Currcode=P.Currcode Where partycode='" & sellcode & "'"
            Dim MyDataset As New DataSet
            Dim MyDataTable As New DataTable
            MyDataset = objUtils.GetDataFromDatasetnew(connstr, sqlstr)
            If MyDataset.Tables.Count > 0 Then
                MyDataTable = MyDataset.Tables(0)
            End If
            If MyDataTable.Rows.Count > 0 Then
                Dim cmaster As New clsMaster
                cmaster.ListValue = MyDataTable.Rows(0).Item("currcode")
                cmaster.ListText = MyDataTable.Rows(0).Item("currname")

                retlist.Add(cmaster)
            End If
            Return retlist.ToArray()


        End Function
        <WebMethod()> _
        Public Function GetSellingCurrNamecost(ByVal connstr As String, ByVal sellcode As String) As String
            Dim retcurrname As String = ""
            retcurrname = CType(objUtils.ExecuteQueryReturnSingleValuenew(connstr, "select c.currname from currmast c, partymast sc where c.currcode  = sc.currcode " _
            & " and sc.partycode='" & sellcode & "'"), String)
            Return retcurrname
        End Function

        <WebMethod()> _
        Public Function GetSellCodeListnew(ByVal constr As String, ByVal plgrpcode As String) As clsMaster()
            Dim retlist As New List(Of clsMaster)
            Dim sqlstr As String
            sqlstr = "select sellcode,sellname  from sellmast where active=1 "
            If Trim(plgrpcode) <> "" And Trim(UCase(plgrpcode)) <> Trim(UCase("[Select]")) Then
                sqlstr = sqlstr + " and plgrpcode='" & Trim(plgrpcode) & "'"
            End If
            sqlstr = sqlstr + " order by sellcode"
            retlist = objUtils.FillclsMasternew(constr, sqlstr, True)
            Return retlist.ToArray()
        End Function

        <WebMethod()> _
        Public Function GetSellNameListnew(ByVal constr As String, ByVal plgrpcode As String) As clsMaster()
            Dim retlist As New List(Of clsMaster)
            Dim sqlstr As String
            sqlstr = "select sellname,sellcode  from sellmast where active=1 "
            If Trim(plgrpcode) <> "" And Trim(UCase(plgrpcode)) <> Trim(UCase("[Select]")) Then
                sqlstr = sqlstr + " and plgrpcode='" & Trim(plgrpcode) & "'"
            End If
            sqlstr = sqlstr + " order by sellname"
            retlist = objUtils.FillclsMasternew(constr, sqlstr, True)
            Return retlist.ToArray()
        End Function
        <WebMethod()> _
        Public Function GetRoomTypeCodeListnew(ByVal constr As String, ByVal sptypecode As String, ByVal partycode As String) As clsMaster()
            Dim retlist As New List(Of clsMaster)
            Dim sqlstr As String
            sqlstr = "select rmtypmast.rmtypcode,rmtypmast.rmtypname from rmtypmast,partyrmtyp where  partyrmtyp.inactive=0  and rmtypmast.active=1 and rmtypmast.rmtypcode=partyrmtyp.rmtypcode "
            If Trim(partycode) <> "" And Trim(UCase(partycode)) <> Trim(UCase("[Select]")) Then
                sqlstr = sqlstr + "and partyrmtyp.partycode='" & Trim(partycode) & "'"
            End If
            If Trim(sptypecode) <> "" And Trim(UCase(sptypecode)) <> Trim(UCase("[Select]")) Then
                sqlstr = sqlstr + " and rmtypmast.sptypecode='" & Trim(sptypecode) & "'"
            End If
            sqlstr = sqlstr + "order by rmtypmast.rmtypcode"

            retlist = objUtils.FillclsMasternew(constr, sqlstr, True)
            Return retlist.ToArray()
        End Function
        <WebMethod()> _
        Public Function GetRoomTypeNameListnew(ByVal constr As String, ByVal sptypecode As String, ByVal partycode As String) As clsMaster()
            Dim retlist As New List(Of clsMaster)
            Dim sqlstr As String
            sqlstr = "select rmtypmast.rmtypname , rmtypmast.rmtypcode from rmtypmast,partyrmtyp where partyrmtyp.inactive=0 and rmtypmast.active=1 and rmtypmast.rmtypcode=partyrmtyp.rmtypcode "
            If Trim(partycode) <> "" And Trim(UCase(partycode)) <> Trim(UCase("[Select]")) Then
                sqlstr = sqlstr + "and partyrmtyp.partycode='" & Trim(partycode) & "' "
            End If
            If Trim(sptypecode) <> "" And Trim(UCase(sptypecode)) <> Trim(UCase("[Select]")) Then
                sqlstr = sqlstr + " and rmtypmast.sptypecode='" & Trim(sptypecode) & "'"
            End If

            sqlstr = sqlstr + " order by rmtypmast.rmtypname"
            retlist = objUtils.FillclsMasternew(constr, sqlstr, True)
            Return retlist.ToArray()
        End Function

        <WebMethod()> _
        Public Function GetSellingCategoryCodeListnew(ByVal constr As String, ByVal sptypecode As String, ByVal partycode As String) As clsMaster()
            Dim retlist As New List(Of clsMaster)
            Dim sqlstr As String
            sqlstr = "select rmtypcode ,rmtypname from rmtypmast where active=1 "
            If Trim(partycode) <> "" And Trim(UCase(partycode)) <> Trim(UCase("[Select]")) Then
                sqlstr = "  select rmtypmast.rmtypcode , rmtypmast.rmtypname from rmtypmast,partyrmtyp where  partyrmtyp.inactive='0'  and rmtypmast.active='1'and rmtypmast.rmtypcode=partyrmtyp.rmtypcode and partycode='" & Trim(partycode) & "' "
            End If
            If Trim(sptypecode) <> "" And Trim(UCase(sptypecode)) <> Trim(UCase("[Select]")) Then
                sqlstr = sqlstr + " and rmtypmast.sptypecode='" & Trim(sptypecode) & "'"
            End If
            sqlstr = sqlstr + " order by rmtypmast.rmtypcode"

            retlist = objUtils.FillclsMasternew(constr, sqlstr, True)
            Return retlist.ToArray()
        End Function
        <WebMethod()> _
        Public Function GetSellingCategoryNameListnew(ByVal constr As String, ByVal sptypecode As String, ByVal partycode As String) As clsMaster()
            Dim retlist As New List(Of clsMaster)
            Dim sqlstr As String
            sqlstr = "select scatname ,scatcode from sellcatmast where active=1 "
            If Trim(partycode) <> "" And Trim(UCase(partycode)) <> Trim(UCase("[Select]")) Then
                sqlstr = "  select rmtypmast.rmtypname , rmtypmast.rmtypcode from rmtypmast,partyrmtyp where partyrmtyp.inactive='0' and rmtypmast.active='1'and rmtypmast.rmtypcode=partyrmtyp.rmtypcode  and partycode='" & Trim(partycode) & "' "
            End If
            If Trim(sptypecode) <> "" And Trim(UCase(sptypecode)) <> Trim(UCase("[Select]")) Then
                sqlstr = sqlstr + " and rmtypmast.sptypecode='" & Trim(sptypecode) & "'"
            End If

            sqlstr = sqlstr + " order by rmtypmast.rmtypname"
            retlist = objUtils.FillclsMasternew(constr, sqlstr, True)
            Return retlist.ToArray()
        End Function
        <WebMethod()> _
        Public Function GetSellingCategoryCodeListnew1(ByVal constr As String, ByVal partycode As String) As String
            Dim result As String = ""
            Dim sqlstr As String
            sqlstr = "SELECT DISTINCT partymast.scatcode,sellcatmast.scatname FROM partymast INNER JOIN sellcatmast ON partymast.scatcode = sellcatmast.scatcode where sellcatmast.active=1 "

            If partycode <> "[Select]" Then
                sqlstr = sqlstr + " and partymast.partycode='" & Trim(partycode) & "'"
            End If

            sqlstr = sqlstr + " order by partymast.scatcode"
            result = objUtils.ExecuteQueryReturnStringValuenew(constr, sqlstr)

            Return result
        End Function
        <WebMethod()> _
        Public Function GetSellingCategoryNameListnew1(ByVal constr As String, ByVal partycode As String) As String
            Dim result As String = ""
            Dim sqlstr As String
            sqlstr = "SELECT DISTINCT sellcatmast.scatname, partymast.scatcode FROM partymast INNER JOIN sellcatmast ON partymast.scatcode = sellcatmast.scatcode where sellcatmast.active=1 "

            If partycode <> "[Select]" Then
                sqlstr = sqlstr + " and partymast.partycode='" & Trim(partycode) & "'"
            End If
            sqlstr = sqlstr + " order by sellcatmast.scatname"
            result = objUtils.ExecuteQueryReturnStringValuenew(constr, sqlstr)
            Return result
        End Function
        <WebMethod()> _
        Public Function GetSeasdatenew(ByVal constr As String, ByVal seascode As String) As clsMaster()
            Dim retlist As New List(Of clsMaster)
            Dim sqlstr As String
            sqlstr = "select frmdate,todate from seasmast where active=1 "
            If Trim(seascode) <> "" And Trim(UCase(seascode)) <> Trim(UCase("[Select]")) Then
                sqlstr = sqlstr + " and seascode='" & Trim(seascode) & "'"
            End If

            retlist = objUtils.FillclsMasternew(constr, sqlstr, True)
            Return retlist.ToArray()
        End Function
        <WebMethod()> _
        Public Function GetCountryCodeListnew(ByVal constr As String, ByVal plgrpcode As String) As clsMaster()
            Dim retlist As New List(Of clsMaster)
            Dim sqlstr As String
            sqlstr = "select  ctrycode,ctryname  from  ctrymast   where active=1"
            If Trim(plgrpcode) <> "" And Trim(UCase(plgrpcode)) <> Trim(UCase("[Select]")) Then
                sqlstr = sqlstr + " and plgrpcode='" & Trim(plgrpcode) & "'"
            End If
            sqlstr = sqlstr + " order by ctrycode "

            retlist = objUtils.FillclsMasternew(constr, sqlstr, True)
            Return retlist.ToArray()
        End Function
        <WebMethod()> _
        Public Function GetCountryNameListnew(ByVal constr As String, ByVal plgrpcode As String) As clsMaster()
            Dim retlist As New List(Of clsMaster)
            Dim sqlstr As String
            sqlstr = "select  ctryname,ctrycode  from  ctrymast   where active=1"
            If Trim(plgrpcode) <> "" And Trim(UCase(plgrpcode)) <> Trim(UCase("[Select]")) Then
                sqlstr = sqlstr + " and plgrpcode='" & Trim(plgrpcode) & "'"
            End If
            sqlstr = sqlstr + " order by ctryname "

            retlist = objUtils.FillclsMasternew(constr, sqlstr, True)
            Return retlist.ToArray()
        End Function
        <WebMethod()> _
        Public Function GetQueryReturnStringValuenew(ByVal constr As String, ByVal sqlstr As String) As String
            Dim retstr As String = ""
            retstr = CType(objUtils.ExecuteQueryReturnStringValuenew(constr, sqlstr), String)
            Return retstr
        End Function
        <WebMethod()> _
        Public Function GetMarketSelltypenew(ByVal constr As String, ByVal custcode As String) As clsMaster()
            Dim retlist As New List(Of clsMaster)
            Dim sqlstr As String
            sqlstr = "SELECT agentmast.plgrpcode,agentmast.sellcode FROM "
            sqlstr += "agentmast INNER JOIN plgrpmast ON agentmast.plgrpcode = plgrpmast.plgrpcode INNER JOIN "
            sqlstr += "sellmast ON agentmast.sellcode = sellmast.sellcode "
            If Trim(custcode) <> "" And Trim(UCase(custcode)) <> Trim(UCase("[Select]")) Then
                sqlstr = sqlstr + " WHERE agentmast.agentcode = '" & Trim(custcode) & "'"
            End If
            retlist = objUtils.FillclsMasternew(constr, sqlstr, True)
            Return retlist.ToArray()
        End Function
        <WebMethod()> _
        Public Function GetSelltypeCodeListnew(ByVal constr As String, ByVal mrktcode As String) As clsMaster()
            Dim retlist As New List(Of clsMaster)
            Dim sqlstr As String
            sqlstr = "select sellcode,sellname from sellmast where active=1  "


            If Trim(mrktcode) <> "" And Trim(UCase(mrktcode)) <> Trim(UCase("[Select]")) Then
                sqlstr = sqlstr + " and plgrpcode='" & Trim(mrktcode) & "'"
            End If
            sqlstr = sqlstr + " ORDER BY sellcode"
            retlist = objUtils.FillclsMasternew(constr, sqlstr, True)
            Return retlist.ToArray()
        End Function
        <WebMethod()> _
        Public Function GetSelltypeNameListnew(ByVal constr As String, ByVal mrktcode As String) As clsMaster()
            Dim retlist As New List(Of clsMaster)
            Dim sqlstr As String
            sqlstr = "select sellname,sellcode from sellmast where active=1 "
            If Trim(mrktcode) <> "" And Trim(UCase(mrktcode)) <> Trim(UCase("[Select]")) Then
                sqlstr = sqlstr + " and plgrpcode='" & Trim(mrktcode) & "'"
            End If
            sqlstr = sqlstr + " ORDER BY sellname"
            retlist = objUtils.FillclsMasternew(constr, sqlstr, True)
            Return retlist.ToArray()
        End Function
        <WebMethod()> _
        Public Function GetCustSubCodeListnew(ByVal constr As String, ByVal custcode As String) As clsMaster()
            Dim retlist As New List(Of clsMaster)
            Dim sqlstr As String
            sqlstr = "SELECT AGENT_SUB_CODE,Sub_User_Name FROM agents_subusers "
            'sqlstr += "agents_subusers ON agentmast.agentcode =agents_subusers.AGENTCODE"
            If Trim(custcode) <> "" And Trim(UCase(custcode)) <> Trim(UCase("[Select]")) Then
                sqlstr = sqlstr + " where agentcode='" & Trim(custcode) & "'"
            End If
            sqlstr = sqlstr + " ORDER BY AGENT_SUB_CODE "
            retlist = objUtils.FillclsMasternew(constr, sqlstr, True)
            Return retlist.ToArray()
        End Function

        <WebMethod()> _
        Public Function GetCustSubNameListnew(ByVal constr As String, ByVal custcode As String) As clsMaster()
            Dim retlist As New List(Of clsMaster)
            Dim sqlstr As String
            sqlstr = "SELECT Sub_User_Name,AGENT_SUB_CODE FROM agents_subusers "
            ' sqlstr += "agents_subusers ON agentmast.agentcode =agents_subusers.AGENTCODE"
            If Trim(custcode) <> "" And Trim(UCase(custcode)) <> Trim(UCase("[Select]")) Then
                sqlstr = sqlstr + " where agentcode='" & Trim(custcode) & "'"
            End If
            sqlstr = sqlstr + " ORDER BY Sub_User_Name "
            retlist = objUtils.FillclsMasternew(constr, sqlstr, True)
            Return retlist.ToArray()
        End Function
        <WebMethod()> _
        Public Function GetCurruncynew(ByVal constr As String, ByVal custcode As String) As clsMaster()
            Dim retlist As New List(Of clsMaster)
            Dim sqlstr As String
            'sqlstr = "SELECT currcode,ctrycode FROM agentmast WHERE active = 1"
            sqlstr = "SELECT agentmast.currcode,currrates.convrate FROM agentmast INNER JOIN currrates ON "
            sqlstr += "agentmast.currcode = currrates.currcode WHERE tocurr=(select option_selected from reservation_parameters where param_id=457) "
            If Trim(custcode) <> "" And Trim(UCase(custcode)) <> Trim(UCase("[Select]")) Then
                sqlstr = sqlstr + " and agentcode='" & Trim(custcode) & "'"
            End If
            retlist = objUtils.FillclsMasternew(constr, sqlstr, True)
            Return retlist.ToArray()
        End Function
        <WebMethod()> _
        Public Function GetCashnew(ByVal constr As String, ByVal custcode As String) As clsMaster()
            Dim retlist As New List(Of clsMaster)
            Dim sqlstr As String
            'sqlstr = "SELECT currcode,ctrycode FROM agentmast WHERE active = 1"
            sqlstr = "SELECT isnull(cashclient,0) as cash,agentcode FROM agentmast"
            If Trim(custcode) <> "" And Trim(UCase(custcode)) <> Trim(UCase("[Select]")) Then
                sqlstr = sqlstr + " where agentcode='" & Trim(custcode) & "'"
            End If
            retlist = objUtils.FillclsMasternew(constr, sqlstr, True)
            Return retlist.ToArray()
        End Function
        <WebMethod()> _
        Public Function GetSupOthGroupCodeListnew(ByVal constr As String, ByVal partycode As String) As clsMaster()
            Dim retlist As New List(Of clsMaster)
            Dim sqlstr As String
            sqlstr = "select othgrpmast.othgrpcode,othgrpmast.othgrpname  from  othgrpmast left outer join  partyothgrp on othgrpmast.othgrpcode=partyothgrp.othgrpcode  where active=1 "

            If Trim(partycode) <> "" And Trim(UCase(partycode)) <> Trim(UCase("[Select]")) Then
                sqlstr = sqlstr + "and partyothgrp.partycode='" & partycode & "'"
            End If
            sqlstr = sqlstr + " order by  othgrpmast.othgrpcode"
            retlist = objUtils.FillclsMasternew(constr, sqlstr, True)
            Return retlist.ToArray()
        End Function
        <WebMethod()> _
        Public Function GetSupOthGroupNameListnew(ByVal constr As String, ByVal partycode As String) As clsMaster()
            Dim retlist As New List(Of clsMaster)
            Dim sqlstr As String
            sqlstr = "select othgrpmast.othgrpname,othgrpmast.othgrpcode  from  othgrpmast left outer join  partyothgrp on othgrpmast.othgrpcode=partyothgrp.othgrpcode  where active=1 "

            If Trim(partycode) <> "" And Trim(UCase(partycode)) <> Trim(UCase("[Select]")) Then
                sqlstr = sqlstr + "and partyothgrp.partycode='" & partycode & "'"
            End If
            sqlstr = sqlstr + " order by  othgrpmast.othgrpname"
            retlist = objUtils.FillclsMasternew(constr, sqlstr, True)
            Return retlist.ToArray()
        End Function

        <WebMethod()> _
        Public Function GetSellTypeCodeListbymarketnew(ByVal constr As String, ByVal custcode As String) As clsMaster()
            Dim retlist As New List(Of clsMaster)
            Dim sqlstr As String
            sqlstr = "SELECT sellmast.sellcode, sellmast.sellname FROM agentmast INNER JOIN plgrpmast ON "
            sqlstr += "agentmast.plgrpcode = plgrpmast.plgrpcode INNER JOIN sellmast ON plgrpmast.plgrpcode "
            sqlstr += " = sellmast.plgrpcode WHERE  sellmast.active = 1 "


            If Trim(custcode) <> "" And Trim(UCase(custcode)) <> Trim(UCase("[Select]")) Then
                sqlstr = sqlstr + "and agentmast.agentcode='" & Trim(custcode) & "'"
            End If
            sqlstr = sqlstr + " ORDER BY sellmast.sellcode"
            retlist = objUtils.FillclsMasternew(constr, sqlstr, True)
            Return retlist.ToArray()
        End Function
        <WebMethod()> _
        Public Function GetSelltypeNameListbymarketnew(ByVal constr As String, ByVal custcode As String) As clsMaster()
            Dim retlist As New List(Of clsMaster)
            Dim sqlstr As String
            sqlstr = "SELECT sellmast.sellname,sellmast.sellcode FROM agentmast INNER JOIN plgrpmast ON "
            sqlstr += "agentmast.plgrpcode = plgrpmast.plgrpcode INNER JOIN sellmast ON plgrpmast.plgrpcode "
            sqlstr += " = sellmast.plgrpcode WHERE  sellmast.active = 1 "

            If Trim(custcode) <> "" And Trim(UCase(custcode)) <> Trim(UCase("[Select]")) Then
                sqlstr = sqlstr + "and agentmast.agentcode='" & Trim(custcode) & "'"
            End If
            sqlstr = sqlstr + " ORDER BY sellmast.sellname"
            retlist = objUtils.FillclsMasternew(constr, sqlstr, True)
            Return retlist.ToArray()
        End Function
        <WebMethod()> _
        Public Function GetMarketNameListnew(ByVal constr As String, ByVal partycode As String) As clsMaster()
            Dim retlist As New List(Of clsMaster)

            Dim sqlstr As String
            sqlstr = "select  distinct plgrpmast.plgrpname,partyallot.plgrpcode  from partyallot inner join plgrpmast on partyallot.plgrpcode = plgrpmast.plgrpcode  "

            If Trim(partycode) <> "" And Trim(UCase(partycode)) <> Trim(UCase("[Select]")) Then
                sqlstr = sqlstr + " where  partyallot.partycode='" & partycode & "'"
            End If
            sqlstr = sqlstr + "  order by plgrpmast.plgrpname "
            retlist = objUtils.FillclsMasternew(constr, sqlstr, True)
            Return retlist.ToArray()
        End Function
        <WebMethod()> _
        Public Function GetMarketCodeListnew(ByVal constr As String, ByVal partycode As String) As clsMaster()
            Dim retlist As New List(Of clsMaster)
            Dim sqlstr As String
            sqlstr = "select distinct partyallot.plgrpcode,plgrpmast.plgrpname from partyallot inner join plgrpmast on partyallot.plgrpcode = plgrpmast.plgrpcode  "

            If Trim(partycode) <> "" And Trim(UCase(partycode)) <> Trim(UCase("[Select]")) Then
                sqlstr = sqlstr + " where  partyallot.partycode='" & partycode & "'"
            End If
            sqlstr = sqlstr + "  order by partyallot.plgrpcode"

            retlist = objUtils.FillclsMasternew(constr, sqlstr, True)

            Return retlist.ToArray()
        End Function
        <WebMethod()> _
        Public Function GetSellingCodeListnew(ByVal constr As String, ByVal plgrpcode As String) As clsMaster()
            Dim retlist As New List(Of clsMaster)
            retlist = objUtils.FillclsMasternew(constr, "select sellcode, sellname from sellmast where plgrpcode='" & plgrpcode & "' order by sellcode", True)
            Return retlist.ToArray()
        End Function
        <WebMethod()> _
        Public Function GetSellingNameListnew(ByVal constr As String, ByVal plgrpcode As String) As clsMaster()
            Dim retlist As New List(Of clsMaster)
            retlist = objUtils.FillclsMasternew(constr, "select sellname,sellcode from sellmast where plgrpcode='" & plgrpcode & "' order by sellname", True)
            Return retlist.ToArray()
        End Function
        <WebMethod()> _
        Public Function GetSupRmCatCodeListnew(ByVal constr As String, ByVal partycode As String) As clsMaster()
            Dim retlist As New List(Of clsMaster)
            Dim sqlstr As String
            'sqlstr = "select rmcatcode,  from partyrmcat  "
            sqlstr = "select rmcatmast.rmcatcode,rmcatmast.rmcatcode as rmcatcode1 from rmcatmast ,partyrmcat " & _
            " where rmcatmast.rmcatcode = partyrmcat.rmcatcode"

            If Trim(partycode) <> "" And Trim(UCase(partycode)) <> Trim(UCase("[Select]")) Then
                sqlstr = sqlstr + " and partyrmcat.partycode='" & Trim(partycode) & "'"
            End If
            sqlstr = sqlstr + " order by rmcatmast.rmcatcode"

            retlist = objUtils.FillclsMasternew(constr, sqlstr, True)
            Return retlist.ToArray()
        End Function
        <WebMethod()> _
        Public Function GetSupRmCatCodeListallotnew(ByVal constr As String, ByVal partycode As String) As clsMaster()
            Dim retlist As New List(Of clsMaster)
            Dim sqlstr As String
            'sqlstr = "select rmcatcode,  from partyrmcat  "
            sqlstr = "select rmcatmast.rmcatcode,rmcatmast.rmcatcode as rmcatcode1 from rmcatmast ,partyrmcat " & _
            " where rmcatmast.rmcatcode = partyrmcat.rmcatcode and rmcatmast.allotreqd='Yes'"

            If Trim(partycode) <> "" And Trim(UCase(partycode)) <> Trim(UCase("[Select]")) Then
                sqlstr = sqlstr + " and partyrmcat.partycode='" & Trim(partycode) & "'"
            End If
            sqlstr = sqlstr + " order by rmcatmast.rmcatcode"

            retlist = objUtils.FillclsMasternew(constr, sqlstr, True)
            Return retlist.ToArray()
        End Function
        <WebMethod()> _
        Public Function GetSupRmMealCodeListnew(ByVal constr As String, ByVal partycode As String) As clsMaster()
            Dim retlist As New List(Of clsMaster)
            Dim sqlstr As String
            'sqlstr = "select mealcode,mealcode from Partymeal  "
            sqlstr = "select mealmast.mealcode,mealmast.mealcode as mealcode1 from mealmast,Partymeal " & _
            " where mealmast.mealcode = partymeal.mealcode"
            If Trim(partycode) <> "" And Trim(UCase(partycode)) <> Trim(UCase("[Select]")) Then
                sqlstr = sqlstr + " and  partymeal.partycode='" & Trim(partycode) & "'"
            End If
            sqlstr = sqlstr + " order by mealmast.mealcode"

            retlist = objUtils.FillclsMasternew(constr, sqlstr, True)
            Return retlist.ToArray()
        End Function
        <WebMethod()> _
        Public Function GetQueryReturnStringListnew(ByVal constr As String, ByVal sqlstr As String) As clsMaster()
            Dim retlist As New List(Of clsMaster)
            retlist = objUtils.FillclsMasternew(constr, sqlstr, True)
            Return retlist.ToArray()
        End Function

        <WebMethod()> _
        Public Function GetQueryReturnStringnew(ByVal constr As String, ByVal sqlstr As String) As String
            Dim result As String
            result = objUtils.ExecuteQueryReturnStringValuenew(constr, sqlstr)
            Return result
        End Function

        <WebMethod()> _
        Public Function GetQueryReturnStringnewForHandling(ByVal constr As String, ByVal AgentCode As String) As String
            Dim result As String
            result = objUtils.ExecuteQueryReturnStringValuenew(constr, "Select AgentCode From AgentMast Where AgentCode='" & AgentCode & "' And HandlingYesNo=1")
            If result <> "" Then
                result = "block"
            Else
                result = "none"
            End If
            Return result
        End Function

        <WebMethod()> _
        Public Function GetArrTimenew(ByVal constr As String, ByVal code As String) As clsMaster()
            Dim retlist As New List(Of clsMaster)
            Dim sqlstr As String
            sqlstr = "select arrivetime1,flightcode from flightmast where active=1 "
            If Trim(code) <> "" And Trim(UCase(code)) <> Trim(UCase("[Select]")) Then
                sqlstr = sqlstr + " and  flightcode='" & Trim(code) & "'"
            End If

            retlist = objUtils.FillclsMasternew(constr, sqlstr, True)
            Return retlist.ToArray()
        End Function
        <WebMethod()> _
        Public Function GetDetpTimenew(ByVal constr As String, ByVal code As String) As clsMaster()
            Dim retlist As New List(Of clsMaster)
            Dim sqlstr As String
            sqlstr = "select Departtime1,flightcode from flightmast where active=1 "
            If Trim(code) <> "" And Trim(UCase(code)) <> Trim(UCase("[Select]")) Then
                sqlstr = sqlstr + " and  flightcode='" & Trim(code) & "'"
            End If
            retlist = objUtils.FillclsMasternew(constr, sqlstr, True)
            Return retlist.ToArray()
        End Function
        '---------------------------updated on 100908 as ref to pune.
        <WebMethod()> _
        Public Function GetCategoryCodeListnew(ByVal constr As String, ByVal sellcode As String) As clsMaster()
            Dim retlist As New List(Of clsMaster)
            Dim sqlstr As String
            sqlstr = "select agentcatcode,agentcatname  from agentcatmast where active=1 "
            If Trim(sellcode) <> "" And Trim(UCase(sellcode)) <> Trim(UCase("[Select]")) Then
                sqlstr = sqlstr + " and sellcode='" & Trim(sellcode) & "'"
            End If
            sqlstr = sqlstr + " order by agentcatcode"
            retlist = objUtils.FillclsMasternew(constr, sqlstr, True)
            Return retlist.ToArray()
        End Function

        <WebMethod()> _
        Public Function GetCategoryNameListnew(ByVal constr As String, ByVal sellcode As String) As clsMaster()
            Dim retlist As New List(Of clsMaster)
            Dim sqlstr As String
            sqlstr = "select agentcatname,agentcatcode  from agentcatmast where active=1 "
            If Trim(sellcode) <> "" And Trim(UCase(sellcode)) <> Trim(UCase("[Select]")) Then
                sqlstr = sqlstr + " and sellcode='" & Trim(sellcode) & "'"
            End If
            sqlstr = sqlstr + " order by agentcatname"
            retlist = objUtils.FillclsMasternew(constr, sqlstr, True)
            Return retlist.ToArray()
        End Function
        '---------------------------End of updated on 100908 as ref to pune.
        '---------------------------updated on 160908 as ref to pune.
        <WebMethod()> _
        Public Function GetQueryReturnDatenew(ByVal constr As String, ByVal sqlstr As String, ByVal strDate As String) As String
            Dim retstr As String = ""
            Dim crdy As Long
            Dim frDate, duDate As Date
            crdy = CType(Val(objUtils.ExecuteQueryReturnStringValuenew(constr, sqlstr)), Long)
            frDate = CType(strDate, Date)
            duDate = DateAdd(DateInterval.Weekday, crdy, frDate).ToShortDateString()
            retstr = CType(duDate, String)
            Return retstr
        End Function
        '---------------------------End of updated on 160908 as ref to pune.
        '---------------------------updated on 230908 as ref to pune.
        <WebMethod()> _
        Public Function GetSuppRoomCodeListnew(ByVal constr As String, ByVal partycode As String) As clsMaster()
            Dim retlist As New List(Of clsMaster)
            Dim sqlstr As String
            sqlstr = " select distinct partyrmtyp.rmtypcode,rmtypmast.rmtypname " & _
                     " from partyrmtyp inner join rmtypmast on partyrmtyp.rmtypcode = rmtypmast.rmtypcode " & _
                     " where  partyrmtyp.inactive=0 "

            If partycode <> "[Select]" Then
                sqlstr = sqlstr + " and partyrmtyp.partycode='" & partycode & "'"
            End If
            sqlstr = sqlstr + " order by partyrmtyp.rmtypcode "
            retlist = objUtils.FillclsMasternew(constr, sqlstr, True)
            Return retlist.ToArray()
        End Function
        <WebMethod()> _
        Public Function GetSuppRoomNameListnew(ByVal constr As String, ByVal partycode As String) As clsMaster()
            Dim retlist As New List(Of clsMaster)
            Dim sqlstr As String

            sqlstr = " select distinct rmtypmast.rmtypname ,partyrmtyp.rmtypcode " & _
                     " from partyrmtyp inner join rmtypmast on partyrmtyp.rmtypcode = rmtypmast.rmtypcode " & _
                      " where  partyrmtyp.inactive=0 "
            If partycode <> "[Select]" Then
                sqlstr = sqlstr + " and partyrmtyp.partycode='" & partycode & "'"
            End If
            sqlstr = sqlstr + " order by partyrmtyp.rmtypcode "
            retlist = objUtils.FillclsMasternew(constr, sqlstr, True)
            Return retlist.ToArray()
        End Function
        <WebMethod()> _
        Public Function GetSuppCatCodeListnew(ByVal constr As String, ByVal partycode As String) As clsMaster()
            Dim retlist As New List(Of clsMaster)
            Dim sqlstr As String
            sqlstr = " SELECT DISTINCT partymast.catcode, catmast.catname " & _
                     " FROM partymast INNER JOIN catmast ON partymast.catcode = catmast.catcode "

            If partycode <> "[Select]" Then
                sqlstr = sqlstr + " WHERE partymast.partycode = '" & partycode & "'"
            End If
            sqlstr = sqlstr + "   order by partymast.catcode "

            retlist = objUtils.FillclsMasternew(constr, sqlstr, True)
            Return retlist.ToArray()
        End Function
        <WebMethod()> _
        Public Function GetSuppCityCodeListnew(ByVal constr As String, ByVal partycode As String) As clsMaster()
            Dim retlist As New List(Of clsMaster)
            Dim sqlstr As String
            sqlstr = " SELECT DISTINCT partymast.citycode, citymast.cityname " & _
                     " FROM partymast INNER JOIN citymast ON partymast.citycode = citymast.citycode "

            If partycode <> "[Select]" Then
                sqlstr = sqlstr + " WHERE partymast.partycode = '" & partycode & "'"
            End If
            sqlstr = sqlstr + "   order by partymast.citycode "

            retlist = objUtils.FillclsMasternew(constr, sqlstr, True)
            Return retlist.ToArray()
        End Function
        <WebMethod()> _
        Public Function GetSuppCatNameListnew(ByVal constr As String, ByVal partycode As String) As clsMaster()
            Dim retlist As New List(Of clsMaster)
            Dim sqlstr As String
            sqlstr = " SELECT DISTINCT catmast.catname, partymast.catcode " & _
                     " FROM partymast INNER JOIN catmast ON partymast.catcode = catmast.catcode "

            If partycode <> "[Select]" Then
                sqlstr = sqlstr + " WHERE partymast.partycode = '" & partycode & "'"
            End If
            sqlstr = sqlstr + "   order by partymast.catcode "
            retlist = objUtils.FillclsMasternew(constr, sqlstr, True)
            Return retlist.ToArray()
        End Function
        <WebMethod()> _
        Public Function GetSuppCityNameListnew(ByVal constr As String, ByVal partycode As String) As clsMaster()
            Dim retlist As New List(Of clsMaster)
            Dim sqlstr As String
            sqlstr = " SELECT DISTINCT citymast.cityname, partymast.citycode " & _
                     " FROM partymast INNER JOIN citymast ON partymast.citycode = citymast.citycode "

            If partycode <> "[Select]" Then
                sqlstr = sqlstr + " WHERE partymast.partycode = '" & partycode & "'"
            End If
            sqlstr = sqlstr + "   order by partymast.citycode "
            retlist = objUtils.FillclsMasternew(constr, sqlstr, True)
            Return retlist.ToArray()
        End Function
        <WebMethod()> _
        Public Function GetMarketCListnew(ByVal constr As String, ByVal partycode As String) As clsMaster()
            Dim retlist As New List(Of clsMaster)
            Dim sqlstr As String
            sqlstr = "SELECT DISTINCT partyallot.plgrpcode, plgrpmast.plgrpname " & _
                     " FROM partyallot INNER JOIN plgrpmast ON partyallot.plgrpcode = plgrpmast.plgrpcode "

            If partycode <> "[Select]" Then
                sqlstr = sqlstr + " WHERE partyallot.partycode = '" & partycode & "'"
            End If
            sqlstr = sqlstr + "   order by partyallot.plgrpcode "
            retlist = objUtils.FillclsMasternew(constr, sqlstr, True)
            Return retlist.ToArray()
        End Function
        <WebMethod()> _
        Public Function GetMarketNListnew(ByVal constr As String, ByVal partycode As String) As clsMaster()
            Dim retlist As New List(Of clsMaster)
            Dim sqlstr As String
            sqlstr = " SELECT DISTINCT plgrpmast.plgrpname, partyallot.plgrpcode " & _
                     " FROM partyallot INNER JOIN plgrpmast ON partyallot.plgrpcode = plgrpmast.plgrpcode "

            If partycode <> "[Select]" Then
                sqlstr = sqlstr + " WHERE partyallot.partycode = '" & partycode & "'"
            End If
            sqlstr = sqlstr + "   order by partyallot.plgrpcode "
            retlist = objUtils.FillclsMasternew(constr, sqlstr, True)
            Return retlist.ToArray()
        End Function
        <WebMethod()> _
        Public Function GetSellingCListnew(ByVal constr As String, ByVal partycode As String) As clsMaster()
            Dim retlist As New List(Of clsMaster)
            Dim sqlstr As String
            sqlstr = " SELECT DISTINCT sellmast.sellcode,sellmast.sellname from sellmast " & _
                     " INNER JOIN partyallot on sellmast.plgrpcode = partyallot.plgrpcode "

            If partycode <> "[Select]" Then
                sqlstr = sqlstr + " WHERE partyallot.partycode = '" & partycode & "'"
            End If
            sqlstr = sqlstr + "   order by sellmast.sellcode "

            retlist = objUtils.FillclsMasternew(constr, sqlstr, True)
            Return retlist.ToArray()
        End Function
        <WebMethod()> _
        Public Function GetSellingNListnew(ByVal constr As String, ByVal partycode As String) As clsMaster()
            Dim retlist As New List(Of clsMaster)
            Dim sqlstr As String
            sqlstr = " SELECT DISTINCT sellmast.sellname,sellmast.sellcode from sellmast " & _
                     " INNER JOIN partyallot on sellmast.plgrpcode = partyallot.plgrpcode "

            If partycode <> "[Select]" Then
                sqlstr = sqlstr + " WHERE partyallot.partycode = '" & partycode & "'"
            End If
            sqlstr = sqlstr + "   order by sellmast.sellname "

            retlist = objUtils.FillclsMasternew(constr, sqlstr, True)
            Return retlist.ToArray()
        End Function
        '---------------------------End of updated on 230908 as ref to pune.
        <WebMethod()> _
        Public Function GetQueryReturnStringArraynew(ByVal connstr As String, ByVal sqlstr As String, ByVal cnt As Long) As String()
            Dim retlist As Array
            retlist = objUtils.FillArraynew(connstr, sqlstr, cnt)
            Return retlist
        End Function
        <WebMethod()> _
        Public Function GetFromCitynew(ByVal constr As String, ByVal partycode As String) As clsMaster()
            Dim retlist As New List(Of clsMaster)
            Dim sqlstr As String
            sqlstr = "select distinct  fromcity,fromcity from flightmast where " & _
                   "active=1 and showinplist=1 and airlinecode='" & partycode & "' "
            retlist = objUtils.FillclsMasternew(constr, sqlstr, True)
            Return retlist.ToArray()
        End Function
        <WebMethod()> _
        Public Function GetToCitynew(ByVal constr As String, ByVal partycode As String) As clsMaster()
            Dim retlist As New List(Of clsMaster)
            Dim sqlstr As String
            sqlstr = "select distinct  tocity,tocity from flightmast where " & _
                    "active=1 and showinplist=1 and airlinecode='" & partycode & "' "
            retlist = objUtils.FillclsMasternew(constr, sqlstr, True)
            Return retlist.ToArray()
        End Function
        <WebMethod()> _
        Public Function GetFromCityByAirlinenew(ByVal constr As String, ByVal airlinecode As String) As clsMaster()
            Dim retlist As New List(Of clsMaster)
            Dim sqlstr As String
            If airlinecode = "" Or airlinecode = "[Select]" Then
                sqlstr = "sp_get_flightcity '','F'"
            Else
                sqlstr = "sp_get_flightcity '" & airlinecode & "','F'"
            End If
            'sqlstr = "select distinct  fromcity,fromcity from flightmast where " & _
            '       "active=1 and showinplist=1 and airlinecode='" & partycode & "' "
            retlist = objUtils.FillclsMasternew(constr, sqlstr, True)
            Return retlist.ToArray()
        End Function
        <WebMethod()> _
        Public Function GetToCityByAirlinenew(ByVal constr As String, ByVal airlinecode As String) As clsMaster()
            Dim retlist As New List(Of clsMaster)
            Dim sqlstr As String
            If airlinecode = "" Or airlinecode = "[Select]" Then
                sqlstr = "sp_get_flightcity '','T'"
            Else
                sqlstr = "sp_get_flightcity '" & airlinecode & "','T'"
            End If
            'sqlstr = "select distinct  tocity,tocity from flightmast where " & _
            '        "active=1 and showinplist=1 and airlinecode='" & partycode & "' "
            retlist = objUtils.FillclsMasternew(constr, sqlstr, True)
            Return retlist.ToArray()
        End Function
        <WebMethod()> _
        Public Function GetEmailandContactnew(ByVal constr As String, ByVal supagent As String, ByVal catcodeid As String, ByVal type As String) As clsMaster()
            Dim retlist As New List(Of clsMaster)
            Dim sqlstr As String = ""
            If catcodeid = "" Or catcodeid = "[Select]" = False Then
                If type = "S" Then
                    sqlstr = "select email,contact1 as contactperson from view_supagents_multiemail where supagentcode='" + supagent + "' and contact1='" + catcodeid + "'"
                ElseIf type = "P" Then
                    sqlstr = "select email,contact1 as contactperson from view_partymast_mulltiemail where partycode='" + supagent + "' and contact1='" + catcodeid + "'"
                Else
                    sqlstr = "select email,contact1 as contactperson from view_agentmast_multiemail where agentcode='" + supagent + "' and contact1='" + catcodeid + "'"
                End If
            End If
            retlist = objUtils.FillclsMasternew(constr, sqlstr, True)
            Return retlist.ToArray()
        End Function
        <WebMethod()> _
        Public Function GetEmailandContactAgent(ByVal constr As String, ByVal supagent As String, ByVal catcodeid As String, ByVal type As String) As String()
            Dim retlist As Array
            Dim sqlstr As String = ""

            sqlstr = "select email,contact1 from view_agentmast_multiemail where agentcode='" + supagent + "' and contact1='" + catcodeid + "'"


            retlist = GetQueryReturnStringArraynew(constr, sqlstr, 2)
            Return retlist
        End Function
        <WebMethod()> _
        Public Function GetEmailandContactnew1(ByVal constr As String, ByVal agentcode As String, ByVal contact1 As String) As String()
            Dim retlist As Array
            Dim sqlstr As String = ""
            'If catcodeid = "" Or catcodeid = "[Select]" = False Then
            'If Type = "S" Then
            sqlstr = "select email,contactperson from agentmast_mulltiemail where agentcode='" + agentcode + "' and ContactPerson='" + contact1 + "'"
            'ElseIf Type = "P" Then
            'sqlstr = "select email,contact1 as contactperson from view_partymast_mulltiemail where partycode='" + supagent + "' and contact1='" + catcodeid + "'"
            'Else
            'sqlstr = "select email,contact1 as contactperson from view_agentmast_multiemail where agnetcode='" + supagent + "' and contact1='" + catcodeid + "'"
            'End If
            'End If
            retlist = GetQueryReturnStringArraynew(constr, sqlstr, 2)
            Return retlist
        End Function


        <WebMethod()> _
        Public Function GetCtyCountryCodeListnew(ByVal constr As String, ByVal ctycode As String) As clsMaster()
            Dim retlist As New List(Of clsMaster)
            Dim sqlstr As String
            sqlstr = "select distinct ctrymast.ctrycode,ctrymast.ctryname from ctrymast,citymast where ctrymast.ctrycode=citymast.ctrycode"
            If Trim(ctycode) <> "" And Trim(UCase(ctycode)) <> Trim(UCase("[Select]")) Then
                sqlstr = sqlstr + " and citymast.citycode='" & Trim(ctycode) & "'"
            End If
            sqlstr = sqlstr + " order by ctrymast.ctrycode "

            retlist = objUtils.FillclsMasternew(constr, sqlstr, True)
            Return retlist.ToArray()
        End Function
        <WebMethod()> _
        Public Function GetCtyCountryNameListnew(ByVal constr As String, ByVal ctyname As String) As clsMaster()
            Dim retlist As New List(Of clsMaster)
            Dim sqlstr As String
            sqlstr = "select distinct ctrymast.ctryname,ctrymast.ctrycode from ctrymast,citymast where ctrymast.ctrycode=citymast.ctrycode"
            If Trim(ctyname) <> "" And Trim(UCase(ctyname)) <> Trim(UCase("[Select]")) Then
                sqlstr = sqlstr + " and citymast.cityname='" & Trim(ctyname) & "'"
            End If
            sqlstr = sqlstr + " order by ctrymast.ctryname "

            retlist = objUtils.FillclsMasternew(constr, sqlstr, True)
            Return retlist.ToArray()
        End Function
        <WebMethod()> _
        Public Function CalcusaleValuenew(ByVal constr As String, ByVal group As String, ByVal adult As String, ByVal child As String, ByVal unit As String, ByVal othercatcode As String, ByVal txtSalePrice As String, ByVal pkgnights As String) As String
            Dim retlist As String
            Dim sqlstr As String

            sqlstr = "sp_calculate_other_price_freeform '" + group + "','" + othercatcode + "'," + txtSalePrice + "," + unit + "," + pkgnights + "," + adult + "," + child


            retlist = objUtils.ExecuteQueryReturnStringValuenew(constr, sqlstr)
            Return retlist.ToString()
        End Function
        <WebMethod()> _
        Public Function Calcupricenew(ByVal constr As String, ByVal supcode As String, ByVal partycode As String, ByVal rmtypcode As String, ByVal rmcatcode As String, ByVal checkin As String, ByVal checkout As String, ByVal sellcode As String, ByVal plgrpcode As String, ByVal custcode As String, ByVal Unit As String, ByVal accom As String, ByVal basketid As String, ByVal requestid As String, ByVal rlineno As String) As String
            Dim retlist As String
            Dim sqlstr As String
            '"+ supcode.value +"','"+ partycode.value +"','"+ rmtypcode.innerHTML +"','"+ rmcatcode.innerHTML +"','','"+ checkin.value +"','"+ checkout.value +"','"+ sellcode.value +"','"+ plgrpcode.value +"','"+ custcode.value +"','',1,0,2,1,"+ unit.value +",'"+ accom.value +"',"+ basketid.value +",'"+ requestid.value +"',"+ rlineno.value +",0";

            sqlstr = "sp_get_web_prices_hdr '" + supcode + "','" + partycode + "','" + rmtypcode + "','" + rmcatcode + "','','" + checkin + "','" + checkout + "','" + sellcode + "','" + plgrpcode + "','" + custcode + "','',1,0,2,1," + Unit + ",'" + accom + "','',0," + basketid + ",0"


            retlist = objUtils.ExecuteQueryReturnStringValuenew(constr, sqlstr)
            Return retlist.ToString()
        End Function

        <WebMethod()> _
        Public Function GetConvertRatenew(ByVal constr As String, ByVal supagent As String) As clsMaster()
            Dim retlist As New List(Of clsMaster)
            Dim sqlstr As String = ""

            If Not (supagent = "" Or supagent = "[Select]") Then
                sqlstr += "select convrate,currcode from currrates where currcode=(select currcode from supplier_agents where supagentcode='" + supagent + "')"
                sqlstr += " and tocurr=(select option_selected from reservation_parameters where param_id=457)"
                retlist = objUtils.FillclsMasternew(constr, sqlstr, )
            End If

            Return retlist.ToArray()
        End Function
        <WebMethod()> _
        Public Function GetFlightCodeListnew(ByVal constr As String, ByVal code As String) As clsMaster()
            Dim retlist As New List(Of clsMaster)
            Dim sqlstr As String = "select distinct flightcode,flightcode from flightmast where active=1  and isnull(showinplist,0)=1 "

            If Not (code = "" Or code = "[Select]") Then
                sqlstr += "and airlinecode='" & code & "'"
            End If
            retlist = objUtils.FillclsMasternew(constr, sqlstr, True)
            Return retlist.ToArray()
        End Function
        <WebMethod()> _
        Public Function GetQueryReturnFromToDate(ByVal strDateType As String, ByVal noDays As Long, ByVal strDate As String) As String
            Dim retstr As String = ""
            'Dim crdy As Long
            Dim frDate, duDate As Date
            ' crdy = noDays
            frDate = CType(strDate, Date)
            If strDateType = "FromDate" Then
                noDays = noDays
            ElseIf strDateType = "ToDate" Then
                noDays = -noDays
            End If
            duDate = DateAdd(DateInterval.Day, noDays, frDate).ToShortDateString()
            retstr = CType(duDate, String)
            Return retstr
        End Function
        <WebMethod()> _
        Public Function GetQueryReturnToDate(ByVal strDate As String) As String
            Dim retstr As String = ""
            'Dim crdy As Long
            Dim frDate, duDate As Date
            ' crdy = noDays
            frDate = CType(strDate, Date)

            duDate = DateAdd(DateInterval.Year, -1, frDate).ToShortDateString()
            retstr = CType(duDate, String)
            Return retstr
        End Function
        <WebMethod()> _
        Public Function GetCatListnew(ByVal constr As String, ByVal codeid As String) As clsMaster()
            Dim retlist As New List(Of clsMaster)
            Dim sqlstr As String
            sqlstr = "select othcatname,othcatcode from othcatmast where active=1"
            If Trim(codeid) <> "" And Trim(UCase(codeid)) <> Trim(UCase("[Select]")) Then
                sqlstr += " and othgrpcode='" & Trim(codeid) & "' "
            End If

            sqlstr = sqlstr + " order by othcatname"
            retlist = objUtils.FillclsMasternew(constr, sqlstr, True)
            Return retlist.ToArray()
        End Function
        <WebMethod()> _
        Public Function GetTypeListnew(ByVal constr As String, ByVal codeid As String) As clsMaster()
            Dim retlist As New List(Of clsMaster)
            Dim sqlstr As String
            sqlstr = "select othtypname,othtypcode from othtypmast where active=1"
            If Trim(codeid) <> "" And Trim(UCase(codeid)) <> Trim(UCase("[Select]")) Then
                sqlstr += " and othgrpcode='" & Trim(codeid) & "' "
            End If

            sqlstr = sqlstr + " order by othtypname"
            retlist = objUtils.FillclsMasternew(constr, sqlstr, True)
            Return retlist.ToArray()
        End Function
        <WebMethod()> _
        Public Function GetSellTypenew(ByVal constr As String, ByVal codeid As String, ByVal mktcode As String) As clsMaster()
            Dim retlist As New List(Of clsMaster)
            Dim sqlstr As String
            sqlstr = "select sellcode, sellname from Sellmast where active=1 "


            If Trim(codeid) <> "" And Trim(UCase(codeid)) <> Trim(UCase("[Select]")) Then
                sqlstr = sqlstr + " and currcode='" & Trim(codeid) & "'"
            End If
            If Trim(mktcode) <> "" And Trim(UCase(mktcode)) <> Trim(UCase("[Select]")) Then
                sqlstr = sqlstr + " and plgrpcode='" & Trim(mktcode) & "'"
            End If
            sqlstr = sqlstr + " ORDER BY sellcode"
            retlist = objUtils.FillclsMasternew(constr, sqlstr, True)
            Return retlist.ToArray()
        End Function
        <WebMethod()> _
        Public Function GettktSellTypenew(ByVal constr As String, ByVal codeid As String, ByVal mktcode As String) As clsMaster()
            Dim retlist As New List(Of clsMaster)
            Dim sqlstr As String
            sqlstr = "select tktsellcode, tktsellname from tktsellmast where active=1 "


            If Trim(codeid) <> "" And Trim(UCase(codeid)) <> Trim(UCase("[Select]")) Then
                sqlstr = sqlstr + " and currcode='" & Trim(codeid) & "'"
            End If
            If Trim(mktcode) <> "" And Trim(UCase(mktcode)) <> Trim(UCase("[Select]")) Then
                sqlstr = sqlstr + " and plgrpcode='" & Trim(mktcode) & "'"
            End If
            sqlstr = sqlstr + " ORDER BY tktsellcode"
            retlist = objUtils.FillclsMasternew(constr, sqlstr, True)
            Return retlist.ToArray()
        End Function
        <WebMethod()> _
        Public Function GetothSellTypenew(ByVal constr As String, ByVal codeid As String) As clsMaster()
            Dim retlist As New List(Of clsMaster)
            Dim sqlstr As String
            sqlstr = "select othsellcode, othsellname from othsellmast where active=1 "


            If Trim(codeid) <> "" And Trim(UCase(codeid)) <> Trim(UCase("[Select]")) Then
                sqlstr = sqlstr + " and currcode='" & Trim(codeid) & "'"
            End If

            sqlstr = sqlstr + " ORDER BY othsellcode"
            retlist = objUtils.FillclsMasternew(constr, sqlstr, True)
            Return retlist.ToArray()
        End Function
        <WebMethod()> _
        Public Function GetSellTypenamenew(ByVal constr As String, ByVal codeid As String, ByVal mktcode As String) As clsMaster()
            Dim retlist As New List(Of clsMaster)
            Dim sqlstr As String
            sqlstr = "select sellname,sellcode  from Sellmast where active=1 "


            If Trim(codeid) <> "" And Trim(UCase(codeid)) <> Trim(UCase("[Select]")) Then
                sqlstr = sqlstr + " and currcode='" & Trim(codeid) & "'"
            End If
            If Trim(mktcode) <> "" And Trim(UCase(mktcode)) <> Trim(UCase("[Select]")) Then
                sqlstr = sqlstr + " and plgrpcode='" & Trim(mktcode) & "'"
            End If
            sqlstr = sqlstr + " ORDER BY sellname"
            retlist = objUtils.FillclsMasternew(constr, sqlstr, True)
            Return retlist.ToArray()
        End Function
        <WebMethod()> _
        Public Function GettktSellTypenamenew(ByVal constr As String, ByVal codeid As String, ByVal mktcode As String) As clsMaster()
            Dim retlist As New List(Of clsMaster)
            Dim sqlstr As String
            sqlstr = "select tktsellname,tktsellcode from tktsellmast where active=1 "


            If Trim(codeid) <> "" And Trim(UCase(codeid)) <> Trim(UCase("[Select]")) Then
                sqlstr = sqlstr + " and currcode='" & Trim(codeid) & "'"
            End If
            If Trim(mktcode) <> "" And Trim(UCase(mktcode)) <> Trim(UCase("[Select]")) Then
                sqlstr = sqlstr + " and plgrpcode='" & Trim(mktcode) & "'"
            End If
            sqlstr = sqlstr + " ORDER BY tktsellname"
            retlist = objUtils.FillclsMasternew(constr, sqlstr, True)
            Return retlist.ToArray()
        End Function
        <WebMethod()> _
        Public Function GetothSellTypenamenew(ByVal constr As String, ByVal codeid As String) As clsMaster()
            Dim retlist As New List(Of clsMaster)
            Dim sqlstr As String
            sqlstr = "select othsellname,othsellcode from othsellmast where active=1 "


            If Trim(codeid) <> "" And Trim(UCase(codeid)) <> Trim(UCase("[Select]")) Then
                sqlstr = sqlstr + " and currcode='" & Trim(codeid) & "'"
            End If
            sqlstr = sqlstr + " ORDER BY othsellname"
            retlist = objUtils.FillclsMasternew(constr, sqlstr, True)
            Return retlist.ToArray()
        End Function
        <WebMethod()> _
        Public Function GetConvnew(ByVal constr As String, ByVal codeid As String) As clsMaster()
            Dim retlist As New List(Of clsMaster)
            Dim sqlstr As String
            sqlstr = "select convrate,tocurr from currrates where currcode=(select option_selected from reservation_parameters where param_id=457) and tocurr='" + codeid + "'"
            retlist = objUtils.FillclsMasternew(constr, sqlstr)
            Return retlist.ToArray()
        End Function
        <WebMethod()> _
        Public Function GetCurruncyRevnew(ByVal constr As String, ByVal custcode As String) As clsMaster()
            Dim retlist As New List(Of clsMaster)
            Dim sqlstr As String
            sqlstr = "SELECT agentmast.currcode,currrates.convrate FROM agentmast INNER JOIN currrates ON "
            sqlstr += "agentmast.currcode = currrates.tocurr WHERE currrates.currcode=(select option_selected from reservation_parameters where param_id=457) "
            If Trim(custcode) <> "" And Trim(UCase(custcode)) <> Trim(UCase("[Select]")) Then
                sqlstr = sqlstr + " and agentcode='" & Trim(custcode) & "'"
            End If
            retlist = objUtils.FillclsMasternew(constr, sqlstr, True)
            Return retlist.ToArray()
        End Function
        <WebMethod()> _
        Public Function GetArrDepTimenew(ByVal constr As String, ByVal Code As String) As clsMaster()
            Dim retlist As New List(Of clsMaster)
            Dim sqlstr As String
            sqlstr = "select departtime1,arrivetime1 from flightmast where flightcode='" + Code + "'"
            retlist = objUtils.FillclsMasternew(constr, sqlstr, True)
            Return retlist.ToArray()
        End Function
        <WebMethod()> _
        Public Function GetGroupCodeListbyCustnew(ByVal constr As String, ByVal custcode As String) As clsMaster()
            Dim retlist As New List(Of clsMaster)
            Dim sqlstr As String
            sqlstr = "select groupquoteid,groupname from groupquote_header where isnull(requestid,'')='' and active=1  " ' 

            If Trim(custcode) <> "" And Trim(UCase(custcode)) <> Trim(UCase("[Select]")) Then
                sqlstr = sqlstr + "and agentcode='" & Trim(custcode) & "'"
            End If
            sqlstr = sqlstr + " ORDER BY groupquoteid"
            retlist = objUtils.FillclsMasternew(constr, sqlstr, True)
            Return retlist.ToArray()
        End Function
        <WebMethod()> _
        Public Function GetGroupNameListbyCustnew(ByVal constr As String, ByVal custcode As String) As clsMaster()
            Dim retlist As New List(Of clsMaster)
            Dim sqlstr As String
            sqlstr = "select groupname,groupquoteid from groupquote_header where  isnull(requestid,'')='' and active=1" '

            If Trim(custcode) <> "" And Trim(UCase(custcode)) <> Trim(UCase("[Select]")) Then
                sqlstr = sqlstr + "and agentcode='" & Trim(custcode) & "'"
            End If
            sqlstr = sqlstr + " ORDER BY groupname"
            retlist = objUtils.FillclsMasternew(constr, sqlstr, True)
            Return retlist.ToArray()
        End Function
        <WebMethod()> _
        Public Function GetSubusernew(ByVal constr As String, ByVal custcode As String) As clsMaster()
            Dim retlist As New List(Of clsMaster)
            Dim sqlstr As String
            sqlstr = "select subusercode,subusercode from groupquote_header"
            If Trim(custcode) <> "" And Trim(UCase(custcode)) <> Trim(UCase("[Select]")) Then
                sqlstr = sqlstr + "  where groupquoteid='" & Trim(custcode) & "'"
            End If
            retlist = objUtils.FillclsMasternew(constr, sqlstr, True)
            Return retlist.ToArray()
        End Function
        <WebMethod()> _
        Public Function GetGroupDetailsnew(ByVal constr As String, ByVal custcode As String) As String()
            Dim retlist As Array
            Dim sqlstr As String

            sqlstr = "SELECT plgrpmast.plgrpcode,plgrpmast.plgrpname,sellmast.sellcode,sellmast.sellname,convert(varchar(10),groupquote_header.validfrm,103),"
            sqlstr += "convert(varchar(10),groupquote_header.validto,103), groupquote_header.agentref,groupquote_header.adults,groupquote_header.child,"
            sqlstr += "groupquote_header.infant,groupquote_header.nights FROM groupquote_header LEFT OUTER JOIN"
            sqlstr += " sellmast ON groupquote_header.sellcode = sellmast.sellcode LEFT OUTER JOIN"
            sqlstr += "  plgrpmast ON groupquote_header.plgrpcode = plgrpmast.plgrpcode "

            If Trim(custcode) <> "" And Trim(UCase(custcode)) <> Trim(UCase("[Select]")) Then
                sqlstr = sqlstr + "  where groupquote_header.groupquoteid='" & Trim(custcode) & "'"
            End If
            retlist = GetQueryReturnStringArraynew(constr, sqlstr, 11)
            Return retlist
        End Function
        <WebMethod()> _
        Public Function GetFlightnew(ByVal constr As String, ByVal code As String) As clsMaster()
            Dim retlist As New List(Of clsMaster)
            Dim sqlstr As String
            sqlstr = "select flightcode,flightcode from flightmast where active=1 and type=1 "
            If Trim(code) <> "" And Trim(UCase(code)) <> Trim(UCase("[Select]")) Then
                sqlstr = sqlstr + " and  airlinecode='" & Trim(code) & "'"
            End If
            retlist = objUtils.FillclsMasternew(constr, sqlstr, True)
            Return retlist.ToArray()
        End Function
        <WebMethod()> _
        Public Function GetRetFlightnew(ByVal constr As String, ByVal code As String) As clsMaster()
            Dim retlist As New List(Of clsMaster)
            Dim sqlstr As String
            sqlstr = "select flightcode,flightcode from flightmast where active=1 and type=0 "
            If Trim(code) <> "" And Trim(UCase(code)) <> Trim(UCase("[Select]")) Then
                sqlstr = sqlstr + " and  airlinecode='" & Trim(code) & "'"
            End If
            retlist = objUtils.FillclsMasternew(constr, sqlstr, True)
            Return retlist.ToArray()
        End Function
        <WebMethod()> _
        Public Function RoundwithParameter(ByVal numbertoround As Decimal) As String
            Dim strsql As String = ""
            Dim roundednumber As Decimal = 0
            Dim roundoff As Integer = 0
            roundoff = 3
            'strsql = "select dbo.roundwithparameter(" & numbertoround & ")"
            'roundednumber = CType(objUtils.ExecuteQueryReturnSingleValuenew(constr,strsql), Decimal)
            roundednumber = Math.Round(numbertoround, roundoff)
            Return roundednumber
        End Function
        <WebMethod()> _
        Public Function RoundwithParameter_2(ByVal numbertoround As Decimal) As String
            Dim strsql As String = ""
            Dim roundednumber As Decimal = 0
            Dim roundoff As Integer = 0
            roundoff = 2
            'strsql = "select dbo.roundwithparameter(" & numbertoround & ")"
            'roundednumber = CType(objUtils.ExecuteQueryReturnSingleValuenew(constr,strsql), Decimal)
            roundednumber = Math.Round(numbertoround, roundoff)
            Return roundednumber
        End Function

        <WebMethod()> _
        Public Function GetRefundDetailsnew(ByVal constr As String, ByVal reqid As String) As String()
            Dim retlist As Array
            Dim sqlstr As String

            sqlstr = "select h.requestid,ih.invoiceno,h.refundtype,h.agentcode,a.des agentname,h.currcode,h.refundsalecurrency from refund_request_header h left outer join view_account a "
            sqlstr += " on h.agentcode= a.code ,reservation_invoice_header ih where h.creditnoteno is null and ih.requestid=h.requestid "

            If Trim(reqid) <> "" And Trim(UCase(reqid)) <> Trim(UCase("[Select]")) Then
                sqlstr = sqlstr + "  and h.refundreqid='" & Trim(reqid) & "'"
            End If

            retlist = GetQueryReturnStringArraynew(constr, sqlstr, 7)
            Return retlist
        End Function
        <WebMethod()> _
        Public Function GetCtrlAccCodenew(ByVal constr As String, ByVal code As String) As String
            Dim strsql As String = ""
            Dim acccode As String = ""
            Dim roundoff As Integer = 0
            roundoff = 3
            strsql = "select controlacctcode from agentmast "
            If code <> "" Or code <> "[Select]" Then
                strsql += " where agentcode='" + code + "'"
            End If

            acccode = objUtils.ExecuteQueryReturnSingleValuenew(constr, strsql)
            Return acccode
        End Function
        <WebMethod()> _
        Public Function GetCtrlAccCodeSuppnew(ByVal constr As String, ByVal code As String) As clsMaster()
            Dim strsql As String = ""
            Dim acccode As String = ""
            Dim retlist As New List(Of clsMaster)
            If code <> "" Or code <> "[Select]" Then
                strsql = "select p.controlacctcode,p.controlacctcode from partymast p where p.partycode='" + code + "' "
                strsql += " union all  select p.accrualacctcode,p.accrualacctcode from partymast p where p.partycode='" + code + "'"
            End If
            retlist = objUtils.FillclsMasternew(constr, strsql, False)
            Return retlist.ToArray()
        End Function
        <WebMethod()> _
        Public Function GetRoomCatCodeInGroupRequestnew(ByVal constr As String, ByVal codeid As String) As clsMaster()
            Dim retlist As New List(Of clsMaster)
            Dim sqlstr As String
            sqlstr = "select r.rmcatcode,r.prnname from rmcatmast r where r.active=1 and r.allotreqd='Yes' "

            If Trim(codeid) <> "" And Trim(UCase(codeid)) <> Trim(UCase("[Select]")) Then
                sqlstr = sqlstr + "and r.rmcatcode in(select distinct rmcatcode from partyrmcat where " & _
                         "partycode in(SELECT distinct partycode FROM grouprequest_detailnew where grouprequestid='" & codeid & "'))"

            End If
            sqlstr = sqlstr + "   order by r.rmcatcode"

            retlist = objUtils.FillclsMasternew(constr, sqlstr, True)
            Return retlist.ToArray()
        End Function
        <WebMethod()> _
        Public Function GetotherServiceCatnew(ByVal constr As String, ByVal codeid As String) As clsMaster()
            Dim retlist As New List(Of clsMaster)
            Dim sqlstr As String
            sqlstr = "select othcatname,othcatcode from othcatmast where active <> 0 and othgrpcode not in (select option_selected from reservation_parameters where param_id in (1001,1025)) "

            If Trim(codeid) <> "" And Trim(UCase(codeid)) <> Trim(UCase("[Select]")) Then
                sqlstr = sqlstr + "and othgrpcode='" & codeid & "'"
            End If
            sqlstr = sqlstr + "   order by othcatname"

            retlist = objUtils.FillclsMasternew(constr, sqlstr)
            Return retlist.ToArray()
        End Function

        <WebMethod()> _
        Public Function GetTrfJeepCatnew(ByVal constr As String, ByVal codeid As String) As clsMaster()
            Dim retlist As New List(Of clsMaster)
            Dim sqlstr As String
            'sqlstr = "select slabno,rtrim(ltrim(str(slabno)+ '-'+othcatcode)) as othcatname from othcat_slabs"

            'If Trim(codeid) <> "" And Trim(UCase(codeid)) <> Trim(UCase("[Select]")) Then
            '    sqlstr = sqlstr + " where othgrpcode='" & codeid & "'"
            'End If
            'sqlstr = sqlstr + "   order by othcatname"

            sqlstr = "sp_filltrfsJeepCatCode '" & IIf(codeid <> "[Select]", codeid, "") & "'"

            retlist = objUtils.FillclsMasternew(constr, sqlstr)
            Return retlist.ToArray()
        End Function

        <WebMethod()> _
        Public Function GetBookingValidityFrom(ByVal connstr As String, ByVal promotioncode As String) As String
            Dim retfrmdate As String = ""
            ''for approval promotion_header replace to Edit_promotion_header
            retfrmdate = CType(objUtils.ExecuteQueryReturnSingleValuenew(connstr, "select (case ISNULL(bookingvaliditytype,'') when 'Book Before'  then isnull(convert(varchar,bookfrom,103),'') when 'Range of Dates' then isnull(convert(varchar,bookfrom,103),'')  else '' end) AS [DD-MM-YYYY]  from vw_promotion_header where promotionid='" & promotioncode & "'"), String)
            Return retfrmdate
        End Function
        <WebMethod()> _
        Public Function GetBookingValidityTo(ByVal connstr As String, ByVal promotioncode As String) As String
            Dim retfrmdate As String = ""
            retfrmdate = CType(objUtils.ExecuteQueryReturnSingleValuenew(connstr, "select isnull(convert(varchar,bookto,103),'') AS [DD-MM-YYYY] from vw_promotion_header where promotionid='" & promotioncode & "'"), String)
            Return retfrmdate
        End Function
        <WebMethod()> _
        Public Function GetBookingValidityType(ByVal connstr As String, ByVal promotioncode As String) As String
            Dim retfrmdate As String = ""
            retfrmdate = CType(objUtils.ExecuteQueryReturnSingleValuenew(connstr, "select bookingvaliditytype from vw_promotion_header where promotionid='" & promotioncode & "'"), String)
            Return retfrmdate
        End Function
        <WebMethod()> _
        Public Function GetBookingDaysMonths(ByVal connstr As String, ByVal promotioncode As String) As String
            Dim retfrmdate As String = ""
            retfrmdate = CType(objUtils.ExecuteQueryReturnSingleValuenew(connstr, "select isnull(convert(varchar,bookbeforenew),'') from vw_promotion_header where promotionid='" & promotioncode & "'"), String)
            Return retfrmdate
        End Function
        <WebMethod()> _
        Public Function GetBookingValidityReq(ByVal connstr As String, ByVal promotioncode As String) As String
            Dim retfrmdate As String = ""
            retfrmdate = CType(objUtils.ExecuteQueryReturnSingleValuenew(connstr, "select bookingvalidityreqd from vw_promotion_header where promotionid='" & promotioncode & "'"), String)
            Return retfrmdate
        End Function
        <WebMethod()> _
        Public Function GetBookingPriceCode(ByVal connstr As String, ByVal promotioncode As String) As String
            ''for approval promotion_header replace to Edit_promotion_header
            Dim retfrmdate As String = ""
            retfrmdate = CType(objUtils.ExecuteQueryReturnSingleValuenew(connstr, "select pricecode from vw_promotion_header where promotionid='" & promotioncode & "'"), String)
            Return retfrmdate
        End Function
        <WebMethod()> _
        Public Function GetPromotions(ByVal constr As String, ByVal codeid As String, ByVal mktcode As String) As clsMaster()
            Dim retlist As New List(Of clsMaster)
            Dim sqlstr As String
            ''for approval promotion_header replace to Edit_promotion_header
            sqlstr = "select pricecode,promotionid from vw_promotion_header where promotionid in (select promotionid from vw_promotionmarket_detail where marketcode='" & mktcode & "') "

            'If Trim(codeid) <> "" And Trim(UCase(codeid)) <> Trim(UCase("[Select]")) Then
            sqlstr = sqlstr + " and isnull(pricecode,'')<>'' and partycode='" & codeid & "'"
            'End If
            sqlstr = sqlstr + " order by promotionid"

            retlist = objUtils.FillclsMasternew(constr, sqlstr, True)
            Return retlist.ToArray()
        End Function
        <WebMethod()> _
        Public Function GetPromotionsnew(ByVal constr As String, ByVal codeid As String, ByVal mktcode As String) As clsMaster()
            Dim retlist As New List(Of clsMaster)
            Dim sqlstr As String
            sqlstr = "select distinct pricecode,promotionid from vw_promotion_header where  isnull(pricecode,'')<>'' "

            'If Trim(codeid) <> "" And Trim(UCase(codeid)) <> Trim(UCase("[Select]")) Then
            sqlstr = sqlstr + "   and partycode='" & codeid & "'"
            'End If
            If mktcode <> "" Then
                sqlstr = sqlstr + " and promotionid in (select promotionid from vw_promotionmarket_detail where marketcode  in (" & mktcode & "))"
            End If
            sqlstr = sqlstr + " order by promotionid"

            retlist = objUtils.FillclsMasternew(constr, sqlstr, True)
            Return retlist.ToArray()
        End Function
        <WebMethod()> _
        Public Function GetPromotionsMarketnew(ByVal constr As String, ByVal codeid As String) As clsMaster()
            Dim retlist As New List(Of clsMaster)
            Dim sqlstr As String
            sqlstr = "select marketcode,promotionid   from vw_promotionmarket_detail where promotionid='" + codeid + "' "

            sqlstr = sqlstr + " order by marketcode"

            retlist = objUtils.FillclsMasternew(constr, sqlstr, True)
            Return retlist.ToArray()
        End Function
        <WebMethod()> _
        Public Function GetSerPointName(ByVal connstr As String, ByVal type As String) As clsMaster()
            Dim retlist As New List(Of clsMaster)
            Dim sqlstr As String
            sqlstr = "select name,code from view_pickupdropoff where Pickuptype='" + type + "'"
            sqlstr = sqlstr + " order by name "
            retlist = objUtils.FillclsMasternew(connstr, sqlstr, True)
            Return retlist.ToArray()
        End Function
        <WebMethod()> _
        Public Function GetAgentDetails(ByVal connstr As String, ByVal agentcode As String) As String()
            Dim retlist As Array
            Dim sqlstr As String = ""
            If agentcode <> "[Select]" Then
                sqlstr = "select shortname,catcode,currcode,sellcode,agentcode,ctrycode,citycode,spersoncode,plgrpcode,sectorcode,agentname,commperc,voucheradd,controlacctcode from agentmast where agentcode='" & agentcode & "'"
                Dim cnt As Long = 14

                retlist = objUtils.FillArraynew(connstr, sqlstr, cnt)

                Return retlist

            End If

        End Function
        <WebMethod()> _
        Public Function GetTransferpointName(ByVal connstr As String, ByVal type As String) As clsMaster()
            Dim retlist As New List(Of clsMaster)
            Dim sqlstr As String = ""

            If type = "H" Then
                sqlstr = "select partyname as name,'H-'+partycode as code from partymast where sptypecode =(select option_selected  from reservation_parameters where param_id=458) order by partyname "
            ElseIf type = "A" Then
                sqlstr = "select airportbordername name,'A-'+airportbordercode code from airportbordersmaster where active=1   order by airportbordername" 'and airportbordercode in (select airportbordercode  from flightmast)
            End If

            retlist = objUtils.FillclsMasternew(connstr, sqlstr, True)
            Return retlist.ToArray()
        End Function
        <WebMethod()> _
        Public Function GetTransfers(ByVal connstr As String, ByVal prm_strReqId As String, ByVal type As String, ByVal pickup As String, ByVal dropoff As String, ByVal prm_transferdate As String) As clsMaster()
            Dim retlist As New List(Of clsMaster)
            Dim sqlstr As String = ""

            'sqlstr = "select othtypname ,othtypcode from othtypmast where othgrpcode =(select option_selected from reservation_parameters where param_id=1001)" ' 564

            'If pickup <> "[Select]" And pickup <> "" Then
            '    If type = "1" Or type = "4" Then
            '        sqlstr = sqlstr + " and pickuppoint ='" + pickup + "'"
            '    ElseIf type = "2" Then
            '        sqlstr = sqlstr + " and pickuppoint in (select sectorcode from partymast where partycode ='" + pickup + "')"
            '    ElseIf type = "3" Then
            '        sqlstr = sqlstr + " and pickuppoint in (select sectorcode from partymast where partycode ='" + pickup + "')"
            '    End If
            'End If
            'If dropoff <> "[Select]" And dropoff <> "" Then
            '    If type = "1" Then
            '        sqlstr = sqlstr + " and dropoffpoint in (select sectorcode from partymast where partycode ='" + dropoff + "') "
            '    ElseIf type = "2" Or type = "4" Then
            '        sqlstr = sqlstr + " and dropoffpoint='" + dropoff + "'"
            '    ElseIf type = "3" Then
            '        sqlstr = sqlstr + " and dropoffpoint in (select sectorcode from partymast where partycode ='" + dropoff + "') "
            '    End If
            'End If
            If type <> "[Select]" And type <> "" Then


                Dim intPickUpType, intDropOffType As Integer
                If type = 1 Then
                    intPickUpType = 1
                    intDropOffType = 2
                ElseIf type = 2 Then
                    intPickUpType = 2
                    intDropOffType = 1
                ElseIf type = 3 Then
                    intPickUpType = 3
                    intDropOffType = 3
                ElseIf type = 4 Then
                    intPickUpType = 1
                    intDropOffType = 2

                End If



                'Dim parms As New List(Of SqlParameter)
                'Dim parm(15) As SqlParameter
                'parm(0) = New SqlParameter("@requestid", CType(prm_strReqId, String))
                'parm(1) = New SqlParameter("@hotellineno", "")
                'parm(2) = New SqlParameter("@rmtyplineno", "")
                'parm(3) = New SqlParameter("@rmcatcode", "")
                'parm(4) = New SqlParameter("@roomno", "")
                'parm(5) = New SqlParameter("@transfertype", type)
                'parm(6) = New SqlParameter("@pickuptype", intPickUpType)
                'parm(7) = New SqlParameter("@droptype", intDropOffType)
                'parm(8) = New SqlParameter("@arrdeplineno", "")
                'parm(9) = New SqlParameter("@pickuppoint", pickup)
                'parm(10) = New SqlParameter("@dropoffpoint", dropoff)
                'parm(11) = New SqlParameter("@transferdate", Format(prm_transferdate, "yyyy/MM/dd"))
                'parm(12) = New SqlParameter("@freeform", 0)
                'Dim p As Integer
                'For p = 0 To 12
                '    parms.Add(parm(p))
                'Next
                Dim ObjDate As New clsDateTime
                'ds_div = objUtils.ExecuteQuerynew(Session("dbconnectionName"), "sp_get_transfer_routes", parms)

                Dim strQuery As StringBuilder = New StringBuilder
                strQuery.AppendFormat("sp_get_transfer_routes {0},{1},{2},'{3}','{4}','{5}',{6},{7},'{8}',{9},{10},'{11}',{12}", type, intPickUpType, intDropOffType, pickup, dropoff, prm_strReqId, 0, 0, "", 0, 0, ObjDate.ConvertDateromTextBoxToTextYearMonthDay(prm_transferdate), 0)



                retlist = objUtils.FillclsMasternew(connstr, strQuery.ToString, True)
                Return retlist.ToArray()
            End If
        End Function

        <WebMethod()> _
        Public Function GetVehicleType(ByVal connstr As String, ByVal prm_strReqId As String, ByVal type As String,
                                       ByVal pickup As String, ByVal dropoff As String, ByVal prm_transferdate As String,
                                        ByVal adult As Integer, ByVal child As Integer, ByVal freeform As Integer, ByVal unit As Integer) As clsMaster()
            Dim retlist As New List(Of clsMaster)
            Dim sqlstr As String = ""


            Dim intPickUpType, intDropOffType As Integer
            If type <> "[Select]" And type <> "" Then


                If type = 1 Then
                    intPickUpType = 1
                    intDropOffType = 2
                ElseIf type = 2 Then
                    intPickUpType = 2
                    intDropOffType = 1
                ElseIf type = 3 Then
                    intPickUpType = 3
                    intDropOffType = 3
                ElseIf type = 4 Then
                    intPickUpType = 1
                    intDropOffType = 2

                End If

                Dim ObjDate As New clsDateTime
                If unit = 0 Then 'unti cannot be 0 passing as 1 for proc 
                    unit = 1
                End If
                Dim strQuery As StringBuilder = New StringBuilder
                strQuery.AppendFormat("sp_get_transfer_vehicletype {0},{1},{2},'{3}','{4}','{5}',{6},{7},'{8}',{9},{10},'{11}',{12},{13},{14},{15}",
                                      type, intPickUpType, intDropOffType, pickup, dropoff, prm_strReqId, 0, 0, "", 0, 0,
                                      ObjDate.ConvertDateromTextBoxToTextYearMonthDay(prm_transferdate), adult, child, freeform, unit)



                retlist = objUtils.FillclsMasternew(connstr, strQuery.ToString, True)
                Return retlist.ToArray()
            End If
        End Function

        'commented by csn, new tables used so same procedure below with new table. 13052012
        '<WebMethod()> _
        'Public Function GetTransferDate(ByVal connstr As String, ByVal type As String, ByVal requestId As String, ByVal rlineno As String) As String
        '    Dim retfrmdate As String = ""
        '    Dim strSql As String = ""
        '    If type = "1" Then
        '        strSql = "select convert(varchar,datein,103) from reservation_detailnewtemp where requestid='" + requestId + "' and rlineno=" + rlineno
        '    ElseIf type = "2" Then
        '        strSql = "select convert(varchar,dateout,103) from reservation_detailnewtemp where requestid='" + requestId + "' and rlineno=" + rlineno
        '    End If
        '    retfrmdate = CType(objUtils.ExecuteQueryReturnSingleValuenew(connstr, strSql), String)
        '    Return retfrmdate
        'End Function

        <WebMethod()> _
        Public Function GetTransferDate(ByVal connstr As String, ByVal type As String, ByVal requestId As String, ByVal rlineno As String) As String
            Dim retfrmdate As String = ""
            Dim strSql As String = ""
            If type = "1" Then
                strSql = "select convert(varchar,checkin ,103) from  reservation_online_hotelstemp where requestid='" + requestId + "' and hotellineno=" + rlineno
            ElseIf type = "2" Then
                strSql = "select convert(varchar,checkout ,103) from  reservation_online_hotelstemp where requestid='" + requestId + "' and hotellineno=" + rlineno
            End If
            retfrmdate = CType(objUtils.ExecuteQueryReturnSingleValuenew(connstr, strSql), String)
            Return retfrmdate
        End Function
        <WebMethod()> _
        Public Function GetSupplierNameAllListwithsector(ByVal constr As String, ByVal sptypecode As String, ByVal catcode As String, ByVal sellcatcode As String, ByVal ctrycode As String, ByVal citycode As String, ByVal sectorcode As String) As clsMaster()
            Dim retlist As New List(Of clsMaster)
            Dim sqlstr As String
            sqlstr = "select partyname, partycode from partymast where active=1 "
            If Trim(sptypecode) <> "" And Trim(UCase(sptypecode)) <> Trim(UCase("[Select]")) Then
                sqlstr = sqlstr + " and sptypecode='" & Trim(sptypecode) & "'"
            End If
            If Trim(catcode) <> "" And Trim(UCase(catcode)) <> Trim(UCase("[Select]")) Then
                sqlstr = sqlstr + " and catcode='" & Trim(catcode) & "'"
            End If

            If Trim(sellcatcode) <> "" And Trim(UCase(sellcatcode)) <> Trim(UCase("[Select]")) Then
                sqlstr = sqlstr + " and scatcode='" & Trim(sellcatcode) & "'"
            End If
            If Trim(ctrycode) <> "" And Trim(UCase(ctrycode)) <> Trim(UCase("[Select]")) Then
                sqlstr = sqlstr + " and ctrycode='" & Trim(ctrycode) & "'"
            End If
            If Trim(citycode) <> "" And Trim(UCase(citycode)) <> Trim(UCase("[Select]")) Then
                sqlstr = sqlstr + " and citycode='" & Trim(citycode) & "'"
            End If

            If Trim(sectorcode) <> "" And Trim(UCase(sectorcode)) <> Trim(UCase("[Select]")) Then
                sqlstr = sqlstr + " and sectorcode='" & Trim(sectorcode) & "'"
            End If
            sqlstr = sqlstr + " order by partyname "

            retlist = objUtils.FillclsMasternew(constr, sqlstr, True)
            Return retlist.ToArray()
        End Function
        <WebMethod()> _
        Public Function GetSuppliments(ByVal constr As String, ByVal partycode As String) As clsMaster()
            Dim retlist As New List(Of clsMaster)
            Dim sqlstr As String
            sqlstr = "select p.rmcatcode,p.rmcatcode from partyrmcat p,rmcatmast r where p.rmcatcode =r.rmcatcode and r.accom_extra<>'A'"
            If Trim(partycode) <> "" And Trim(UCase(partycode)) <> Trim(UCase("[Select]")) Then
                sqlstr = sqlstr + " and p.partycode='" & Trim(partycode) & "'"
            End If
            retlist = objUtils.FillclsMasternew(constr, sqlstr, True)
            Return retlist.ToArray()
        End Function
        <WebMethod()> _
        Public Function GetCatCityCode(ByVal constr As String, ByVal partycode As String) As clsMaster()
            Dim retlist As New List(Of clsMaster)
            Dim sqlstr As String
            sqlstr = "select ltrim(rtrim(catcode)) catcode,ltrim(rtrim(citycode)) citycode from partymast "
            If Trim(partycode) <> "" And Trim(UCase(partycode)) <> Trim(UCase("[Select]")) Then
                sqlstr = sqlstr + " where  partycode='" & Trim(partycode) & "'"
            End If
            retlist = objUtils.FillclsMasternew(constr, sqlstr, True)
            Return retlist.ToArray()
        End Function
        <WebMethod()> _
        Public Function GetCancellationPolicy(ByVal constr As String, ByVal partycode As String, ByVal mktcode As String, ByVal frmdate As String, ByVal todate As String) As String
            Dim retlist As String = ""
            Dim sqlstr As String
            Dim ds As DataSet
            Dim objdate As New clsDateTime
            sqlstr = "sp_get_cancelpolicy_resrmks '" + partycode + "','" + mktcode + "','" + objdate.ConvertDateromTextBoxToTextYearMonthDay(frmdate) + "','" + objdate.ConvertDateromTextBoxToTextYearMonthDay(todate) + "'"
            ds = objUtils.ExecuteQuerySqlnew(constr, sqlstr)
            If ds.Tables.Count > 0 Then
                For i = 0 To ds.Tables(0).Rows.Count - 1
                    retlist = retlist & ds.Tables(0).Rows(i)("policy") & "<br /><br />"
                    '  retlist = retlist & "<br /><br />" & ds.Tables(0).Rows(i)("remarks") & "<br />"
                Next
                retlist = retlist.Replace(Chr(13), "<br/>")
            End If
            If retlist.Trim = "" Then
                retlist = "<center> ------- No Cancellation Policy Entered ------- </center>"
            End If
            Return retlist.ToString()
        End Function

        <WebMethod()> _
        Public Function GetOthersCancellationPolicy(ByVal constr As String, ByVal requestid As String, ByVal othgrpcode As String) As String
            Dim retlist As String = ""
            Dim sqlstr As String
            Dim ds As DataSet
            Dim objdate As New clsDateTime
            sqlstr = "sp_get_other_cancelpolicy_resrmks '" + requestid + "','" + othgrpcode + "'"
            ds = objUtils.ExecuteQuerySqlnew(constr, sqlstr)
            If ds.Tables.Count > 0 Then
                For i = 0 To ds.Tables(0).Rows.Count - 1
                    retlist = retlist & ds.Tables(0).Rows(i)("policy") & "<br /><br />"
                    '  retlist = retlist & "<br /><br />" & ds.Tables(0).Rows(i)("remarks") & "<br />"
                Next
                retlist = retlist.Replace(Chr(13), "<br/>")
            End If
            If retlist.Trim = "" Then
                retlist = "<center> ------- No Cancellation Policy Entered ------- </center>"
            End If
            Return retlist.ToString()
        End Function

        <WebMethod()> _
        Public Function GetTransfersCancellationPolicy(ByVal constr As String, ByVal requestid As String) As String
            Dim retlist As String = ""
            Dim sqlstr As String
            Dim ds As DataSet
            Dim objdate As New clsDateTime
            sqlstr = "sp_get_transfer_cancelpolicy_resrmks  '" + requestid + "'"
            ds = objUtils.ExecuteQuerySqlnew(constr, sqlstr)
            If ds.Tables.Count > 0 Then
                For i = 0 To ds.Tables(0).Rows.Count - 1
                    retlist = retlist & ds.Tables(0).Rows(i)("policy") & "<br /><br />"
                    '  retlist = retlist & "<br /><br />" & ds.Tables(0).Rows(i)("remarks") & "<br />"
                Next
                retlist = retlist.Replace(Chr(13), "<br/>")
            End If
            If retlist.Trim = "" Then
                retlist = "<center> ------- No Cancellation Policy Entered ------- </center>"
            End If
            Return retlist.ToString()
        End Function


        <WebMethod()> _
        Public Function GetGeneralPolicy(ByVal constr As String, ByVal partycode As String, ByVal mktcode As String) As String
            Dim retlist As String = ""
            Dim sqlstr As String
            Dim ds As DataSet
            Dim objdate As New clsDateTime
            sqlstr = "sp_get_excel_resnrmks '" + partycode + "','" + mktcode + "'"
            ds = objUtils.ExecuteQuerySqlnew(constr, sqlstr)
            If ds.Tables.Count > 0 Then
                For i = 0 To ds.Tables(0).Rows.Count - 1
                    retlist = retlist & ds.Tables(0).Rows(i)("items") & "<br />"
                    '  retlist = retlist & "<br /><br />" & ds.Tables(0).Rows(i)("remarks") & "<br />"
                Next
                retlist = retlist.Replace(Chr(13), "<br/>")
            End If
            If retlist.Trim = "" Then
                retlist = "<center> ------- No General Policy Entered ------- </center>"
            End If
            Return retlist.ToString()
        End Function

        <WebMethod(EnableSession:=True)> _
        Public Function GetChildPolicy(ByVal constr As String, ByVal partycode As String, ByVal mktcode As String, ByVal frmdate As String, ByVal todate As String) As String
            Dim retlist As String = ""
            Dim sqlstr As String
            Dim ds As DataSet
            Dim objdate As New clsDateTime
            'sqlstr = "sp_get_childpolicy_resrmks '" + partycode + "','" + mktcode + "','" + objdate.ConveotrtDateromTextBoxToTextYearMonthDay(frmdate) + "','" + objdate.ConvertDateromTextBoxToTextYearMonthDay(todate) + "'"
            'If Session("promopolicy") <> "" Then
            '    Session("RoomTypePolicy") = Session("RoomTypePolicy") + ";" + Session("promopolicy")
            'End If

            sqlstr = "sp_get_childpolicy_resrmks_new '" + partycode + "','" + mktcode + "','" + objdate.ConvertDateromTextBoxToTextYearMonthDay(frmdate) + "','" + objdate.ConvertDateromTextBoxToTextYearMonthDay(todate) + "','" + Session("RoomTypePolicy") + "','" + Session("Rmcatpolicy") + "','" + Session("promopolicy") + "'"

            ds = objUtils.ExecuteQuerySqlnew(constr, sqlstr)
            If ds.Tables.Count > 0 Then
                For i = 0 To ds.Tables(0).Rows.Count - 1
                    retlist = retlist & ds.Tables(0).Rows(i)("child") & "<br />"
                    '  retlist = retlist & "<br /><br />" & ds.Tables(0).Rows(i)("remarks") & "<br />"
                Next
                retlist = retlist.Replace(Chr(13), "<br/>")
            End If
            If retlist.Trim = "" Then
                retlist = "<center> ------- No Child Policy Entered ------- </center>"
            End If
            Return retlist.ToString()
        End Function


        <WebMethod()> _
        Public Function GetCompRemarkPolicy(ByVal constr As String, ByVal partycode As String, ByVal mktcode As String, ByVal frmdate As String, ByVal todate As String, ByVal roomstring As String) As String
            Dim retlist As String = ""
            Dim sqlstr As String
            Dim ds As DataSet
            Dim objdate As New clsDateTime
            sqlstr = "sp_get_compulsory_resremks '" + partycode + "','" + mktcode + "','" + objdate.ConvertDateromTextBoxToTextYearMonthDay(frmdate) + "','" + objdate.ConvertDateromTextBoxToTextYearMonthDay(todate) + "',1,'" + roomstring + "'"
            ds = objUtils.ExecuteQuerySqlnew(constr, sqlstr)
            If ds.Tables.Count > 0 Then
                If ds.Tables(0).Rows.Count > 0 Then
                    retlist = "<table  border=1 width=850 > <tr>"
                    retlist += "<th> <center> From Date </center> </th>" + "<th> <center> To Date </center></th>" + "<th> <center>Remarks </center></th>" + "</tr>"
                    For i = 0 To ds.Tables(0).Rows.Count - 1
                        retlist += "<tr>"
                        retlist += "<td>" + "<center>" + ds.Tables(0).Rows(i)("frmdate") + "</center>" + "</td>"
                        retlist += "<td>" + "<center>" + ds.Tables(0).Rows(i)("todate") + "</center>" + "</td>"
                        retlist += "<td>" + "<left>" + ds.Tables(0).Rows(i)("remarks") + "</left>" + "</td>" '& "<br /><br />"
                        retlist += "</tr>"
                    Next
                    retlist += "</table>"
                    retlist = retlist.Replace(Chr(13), "<br/>")
                End If
            End If
            If retlist.Trim = "" Then
                retlist = "<center> ------- No Compulsory Remarks Entered ------- </center>"
            End If
            Return retlist.ToString()
        End Function

        <WebMethod()> _
        Public Function GetCompRemarkPolicy_Web(ByVal constr As String, ByVal partycode As String, ByVal mktcode As String,
                                                ByVal frmdate As String, ByVal todate As String, ByVal roomstring As String) As String
            Dim retlist As String = ""
            Dim sqlstr As String
            Dim ds As DataSet
            Dim objdate As New clsDateTime
            sqlstr = "sp_get_compulsory_resremks_web '" + partycode + "','" + mktcode + "','" + objdate.ConvertDateromTextBoxToTextYearMonthDay(frmdate) + "','" + objdate.ConvertDateromTextBoxToTextYearMonthDay(todate) + "',1,'" + roomstring + "'"
            ds = objUtils.ExecuteQuerySqlnew(constr, sqlstr)
            If ds.Tables.Count > 0 Then
                If ds.Tables(0).Rows.Count > 0 Then
                    retlist = "<table  border=1 width=850 > <tr>"
                    retlist += "<th> <center> From Date </center> </th>" + "<th> <center> To Date </center></th>" + "<th> <center>Remarks </center></th>" + "</tr>"
                    For i = 0 To ds.Tables(0).Rows.Count - 1
                        retlist += "<tr>"
                        retlist += "<td>" + "<center>" + ds.Tables(0).Rows(i)("frmdate") + "</center>" + "</td>"
                        retlist += "<td>" + "<center>" + ds.Tables(0).Rows(i)("todate") + "</center>" + "</td>"
                        retlist += "<td>" + "<left>" + ds.Tables(0).Rows(i)("remarks") + "</left>" + "</td>" '& "<br /><br />"
                        retlist += "</tr>"
                    Next
                    retlist += "</table>"
                    retlist = retlist.Replace(Chr(13), "<br/>")
                End If
            End If
            If retlist.Trim = "" Then
                retlist = "<center> ------- No Compulsory Remarks Entered ------- </center>"
            End If
            Return retlist.ToString()
        End Function

        <WebMethod()> _
        Public Function FillGridPrices_otherservice(ByVal constr As String, ByVal requestid As String, ByVal rlineno As Int16, ByVal olineno As Int16, ByVal Otherdt As String,
                                    ByVal grpCode As String, ByVal othtypcode As String, ByVal othcatcode As String, ByVal unitpax As Decimal) As String()
            Dim retlist As Array
            Dim otherdtemp As Date = Otherdt
            Otherdt = Format(otherdtemp, "yyyy/MM/dd")
            Dim sqlstr As StringBuilder = New StringBuilder()
            If (othtypcode <> "[Select]" And othcatcode <> "[Select]") Then

                sqlstr.AppendFormat("exec sp_get_othplist_prices '{0}','{1}','{2}','{3}',{4},{5},{6},'{7}'  ", Otherdt, othtypcode, othcatcode, requestid, rlineno, olineno, unitpax, grpCode)
                Dim cnt As Long = 5

                retlist = objUtils.FillArraynew(constr, sqlstr.ToString, cnt)

                Return retlist

            End If
        End Function

        <WebMethod()> _
        Public Function GetBlockSalesRemarkPolicy(ByVal constr As String, ByVal partycode As String, ByVal mktcode As String, ByVal frmdate As String, ByVal todate As String, ByVal roomstring As String) As String
            Dim retlist As String = ""
            Dim sqlstr As String
            Dim ds As DataSet
            Dim objdate As New clsDateTime
            sqlstr = "sp_get_blocksales_resrmks '" + partycode + "','" + mktcode + "','" + objdate.ConvertDateromTextBoxToTextYearMonthDay(frmdate) + "','" + objdate.ConvertDateromTextBoxToTextYearMonthDay(todate) + "',1,'" + roomstring + "'"
            ds = objUtils.ExecuteQuerySqlnew(constr, sqlstr)
            If ds.Tables.Count > 0 Then
                If ds.Tables(0).Rows.Count > 0 Then
                    retlist = "<table  border=1 width=850 > <tr>"
                    retlist += "<th> <center> From Date </center> </th>" + "<th> <center> To Date </center></th>" + "<th> <center>Remarks </center></th>" + "</tr>"
                    For i = 0 To ds.Tables(0).Rows.Count - 1
                        retlist += "<tr>"
                        retlist += "<td>" + "<center>" + ds.Tables(0).Rows(i)("frmdate") + "</center>" + "</td>"
                        retlist += "<td>" + "<center>" + ds.Tables(0).Rows(i)("todate") + "</center>" + "</td>"
                        retlist += "<td>" + "<left>" + ds.Tables(0).Rows(i)("remarks") + "</left>" + "</td>" '& "<br /><br />"
                        retlist += "</tr>"
                    Next
                    retlist += "</table>"
                    retlist = retlist.Replace(Chr(13), "<br/>")
                End If
            End If
            If retlist.Trim = "" Then
                retlist = "<center> ------- No Block Sales Entered ------- </center>"
            End If
            Return retlist.ToString()
        End Function

        <WebMethod()> _
        Public Function GetMinNightsPolicy(ByVal constr As String, ByVal partycode As String, ByVal mktcode As String, ByVal frmdate As String, ByVal todate As String, ByVal roomstring As String) As String
            Dim retlist As String = ""
            Dim sqlstr As String
            Dim ds As DataSet
            Dim objdate As New clsDateTime

            sqlstr = "sp_get_minnights_resrmks '" + partycode + "','" + mktcode + "','" + objdate.ConvertDateromTextBoxToTextYearMonthDay(frmdate) + "','" + objdate.ConvertDateromTextBoxToTextYearMonthDay(todate) + "',1,'" + roomstring + "'"
            ds = objUtils.ExecuteQuerySqlnew(constr, sqlstr)
            If ds.Tables.Count > 0 Then
                If ds.Tables(0).Rows.Count > 0 Then
                    retlist = "<table  border=1 width=850 > <tr>"
                    retlist += "<th> <center> From Date </center> </th>" + "<th> <center> To Date </center></th>" + "<th> <center>Remarks </center></th>" + "</tr>"
                    For i = 0 To ds.Tables(0).Rows.Count - 1
                        retlist += "<tr>"
                        retlist += "<td>" + "<center>" + ds.Tables(0).Rows(i)("frmdate") + "</center>" + "</td>"
                        retlist += "<td>" + "<center>" + ds.Tables(0).Rows(i)("todate") + "</center>" + "</td>"
                        retlist += "<td>" + "<left>" + ds.Tables(0).Rows(i)("remarks") + "</left>" + "</td>" '& "<br /><br />"
                        retlist += "</tr>"
                    Next
                    retlist += "</table>"
                    retlist = retlist.Replace(Chr(13), "<br/>")
                End If
            End If
            If retlist.Trim = "" Then
                retlist = "<center> ------- No Minimum Nights Policy Entered ------- </center>"
            End If
            Return retlist.ToString()
        End Function
        <WebMethod()> _
        Public Function GetHotelConstructionPolicy(ByVal constr As String, ByVal partycode As String, ByVal frmdate As String, ByVal todate As String) As String
            Dim retlist As String = ""
            Dim sqlstr As String
            Dim ds As DataSet
            Dim objdate As New clsDateTime
            sqlstr = "sp_get_hotels_construction_resrmks '" + partycode + "','" + objdate.ConvertDateromTextBoxToTextYearMonthDay(frmdate) + "','" + objdate.ConvertDateromTextBoxToTextYearMonthDay(todate) + "'"
            ds = objUtils.ExecuteQuerySqlnew(constr, sqlstr)
            If ds.Tables.Count > 0 Then
                For i = 0 To ds.Tables(0).Rows.Count - 1
                    retlist = retlist & ds.Tables(0).Rows(i)(0) & "<br />"
                    '  retlist = retlist & "<br /><br />" & ds.Tables(0).Rows(i)("remarks") & "<br />"
                Next
                retlist = retlist.Replace(Chr(13), "<br/>")
            End If
            If retlist.Trim = "" Or retlist.Trim = "<br />" Then
                retlist = "<center> ------- No Hotel Construction Details Entered ------- </center>"
            End If
            Return retlist.ToString()
        End Function
        <WebMethod()> _
        Public Function GetOtherservicePolicy(ByVal constr As String, ByVal grpcode As String, ByVal mktcode As String, ByVal policy As String) As String
            Dim retlist As String = ""
            Dim sqlstr As String
            Dim ds As DataSet
            Dim objdate As New clsDateTime
            sqlstr = "sp_get_OthservicePolicy '" + grpcode + "','" + mktcode + "','" + policy + "'"
            ds = objUtils.ExecuteQuerySqlnew(constr, sqlstr)
            If ds.Tables.Count > 0 Then
                For i = 0 To ds.Tables(0).Rows.Count - 1
                    retlist = retlist & ds.Tables(0).Rows(i)("Policy") & "<br />"
                    '  retlist = retlist & "<br /><br />" & ds.Tables(0).Rows(i)("remarks") & "<br />"
                Next
                retlist = retlist.Replace(Chr(13), "<br/>")
            End If
            If retlist.Trim = "" Then
                retlist = "<center> ------- No General Policy Entered ------- </center>"
            End If
            Return retlist.ToString()
        End Function

        <WebMethod()> _
        Public Function CalcDOBFromAge(ByVal age As String, ByVal calcwithdate As String) As String
            Try
                Dim cmp As Date = CType(calcwithdate, Date)
                Dim agearray As String() = age.Split(".")

                If agearray.Length > 1 Then
                    Dim mn As String = agearray(1)
                    If mn <> "" And mn <> "0" Then
                        cmp = cmp.AddMonths(-CType(mn, Integer))
                    End If
                End If
                If agearray.Length > 0 Then
                    Dim yr As String = agearray(0)
                    If yr <> "" And yr <> "0" Then
                        cmp = cmp.AddYears(-CType(yr, Integer))
                    End If
                End If
                Return cmp.ToString("dd/MM/yyyy")

            Catch ex As Exception
                Return ""
            End Try
        End Function
        <WebMethod()> _
        Public Function GetCustGroupList(ByVal constr As String, ByVal codeid As String) As clsMaster()
            Dim retlist As New List(Of clsMaster)
            Dim sqlstr As String
            sqlstr = "select agentname, agentcode from agentmast where active=1"

            If Trim(codeid) <> "" And Trim(UCase(codeid)) <> Trim(UCase("[Select]")) Then
                sqlstr = sqlstr + " and plgrpcode='" & codeid & "'"
            End If
            sqlstr = sqlstr + "   order by plgrpcode"

            retlist = objUtils.FillclsMasternew(constr, sqlstr)
            Return retlist.ToArray()
        End Function
        <WebMethod(EnableSession:=True)> _
        Public Function CustAutoComplete(ByVal para1 As String) As List(Of AutocompleteClass)
            Try
                Dim CustomerAutoCompleteClass As New List(Of AutocompleteClass)
                Dim CustomerAutoCompleteClassAdd As New AutocompleteClass
                Dim strSql As String = "Select  Agentcode,Agentname From AgentMast where active=1 "
                If para1 <> "" And Trim(UCase(para1)) <> Trim(UCase("[Select]")) Then
                    strSql = strSql + " and  plgrpcode='" & para1 & "'"
                End If

                strSql = strSql + " order by Agentname "

                Dim MYCommand As New SqlCommand(strSql, clsDBConnect.dbConnectionnew(Session("dbconnectionName").ToString()))

                Dim Dr As SqlDataReader
                Dr = MYCommand.ExecuteReader
                While Dr.Read
                    If Not IsDBNull("Agentcode") Then
                        CustomerAutoCompleteClassAdd = New AutocompleteClass
                        CustomerAutoCompleteClassAdd.Id = Dr("AgentCode")
                        CustomerAutoCompleteClassAdd.Name = Dr("AgentName")
                        CustomerAutoCompleteClass.Add(CustomerAutoCompleteClassAdd)
                    End If
                End While
                Dr.Close()
                MYCommand.Cancel()
                Return CustomerAutoCompleteClass
                'Dim retlist(2) As String
                'Dim min As String
                'Dim max As String

                'min = objUtils.ExecuteQueryReturnStringValuenew(constr, "select minpax from othcatmast where othgrpcode='TRFS' and othcatcode='" & vehicletype & "'")
                'max = objUtils.ExecuteQueryReturnStringValuenew(constr, "select maxpax from othcatmast where othgrpcode='TRFS' and othcatcode='" & vehicletype & "'")

                'retlist(0) = min
                'retlist(1) = max


                'Return retlist
            Catch ex As Exception

            End Try
        End Function



        <WebMethod(EnableSession:=True)> _
        Public Function reservationgrppackageAutoComplete(ByVal para1 As String) As List(Of AutocompleteStringClass)
            Try
                Dim CustomerAutoCompleteClass As New List(Of AutocompleteStringClass)
                Dim CustomerAutoCompleteClassAdd As New AutocompleteStringClass
                Dim strSql As String = "select distinct packagename from reservation_headernewtemp where packagename is not null and packagename<>'' "
                Dim MYCommand As New SqlCommand(strSql, clsDBConnect.dbConnectionnew(Session("dbconnectionName").ToString()))

                Dim Dr As SqlDataReader
                Dr = MYCommand.ExecuteReader
                While Dr.Read
                    If Not IsDBNull("packagename") Then
                        CustomerAutoCompleteClassAdd = New AutocompleteStringClass

                        CustomerAutoCompleteClassAdd.Name = Dr("packagename")
                        CustomerAutoCompleteClass.Add(CustomerAutoCompleteClassAdd)
                    End If
                End While
                Dr.Close()
                MYCommand.Cancel()
                Return CustomerAutoCompleteClass
                'Dim retlist(2) As String
                'Dim min As String
                'Dim max As String

                'min = objUtils.ExecuteQueryReturnStringValuenew(constr, "select minpax from othcatmast where othgrpcode='TRFS' and othcatcode='" & vehicletype & "'")
                'max = objUtils.ExecuteQueryReturnStringValuenew(constr, "select maxpax from othcatmast where othgrpcode='TRFS' and othcatcode='" & vehicletype & "'")

                'retlist(0) = min
                'retlist(1) = max


                'Return retlist
            Catch ex As Exception

            End Try
        End Function


        <WebMethod(EnableSession:=True)> _
        Public Function supplierAutoComplete(ByVal para1 As String, ByVal para2 As String) As List(Of AutocompleteClass)
            Try
                Dim CustomerAutoCompleteClass As New List(Of AutocompleteClass)
                Dim CustomerAutoCompleteClassAdd As New AutocompleteClass
                Dim strSql As String = "Select  partyname,partycode From partymast where  active=1 "

                If para2 <> "" Then
                    strSql = strSql + " and sptypecode ='" + para2 + "'"
                End If
                If para1 <> "" Then
                    strSql = strSql + " and partyname like '" + para1 + "'%"
                End If

                strSql = strSql + " order by partyname "

                Dim MYCommand As New SqlCommand(strSql, clsDBConnect.dbConnectionnew(Session("dbconnectionName").ToString()))

                Dim Dr As SqlDataReader
                Dr = MYCommand.ExecuteReader
                While Dr.Read
                    If Not IsDBNull("partycode") Then
                        CustomerAutoCompleteClassAdd = New AutocompleteClass
                        CustomerAutoCompleteClassAdd.Id = Dr("partycode")
                        CustomerAutoCompleteClassAdd.Name = Dr("partyname")
                        CustomerAutoCompleteClass.Add(CustomerAutoCompleteClassAdd)
                    End If
                End While
                Dr.Close()
                MYCommand.Cancel()
                Return CustomerAutoCompleteClass
                'Dim retlist(2) As String
                'Dim min As String
                'Dim max As String

                'min = objUtils.ExecuteQueryReturnStringValuenew(constr, "select minpax from othcatmast where othgrpcode='TRFS' and othcatcode='" & vehicletype & "'")
                'max = objUtils.ExecuteQueryReturnStringValuenew(constr, "select maxpax from othcatmast where othgrpcode='TRFS' and othcatcode='" & vehicletype & "'")

                'retlist(0) = min
                'retlist(1) = max


                'Return retlist
            Catch ex As Exception

            End Try
        End Function


        <WebMethod(EnableSession:=True)> _
        Public Function cityAutoCompletefilter(ByVal para1 As String) As List(Of AutocompleteClass)
            Try
                Dim CustomerAutoCompleteClass As New List(Of AutocompleteClass)
                Dim CustomerAutoCompleteClassAdd As New AutocompleteClass
                Dim strSql As String = "Select  cityname,citycode From citymast where  active=1 "

                If para1 <> "" Then
                    strSql = strSql + " and ctrycode = '" + para1 + "'"
                End If


                strSql = strSql + " order by cityname "

                Dim MYCommand As New SqlCommand(strSql, clsDBConnect.dbConnectionnew(Session("dbconnectionName").ToString()))

                Dim Dr As SqlDataReader
                Dr = MYCommand.ExecuteReader
                While Dr.Read
                    If Not IsDBNull("citycode") Then
                        CustomerAutoCompleteClassAdd = New AutocompleteClass
                        CustomerAutoCompleteClassAdd.Id = Dr("citycode")
                        CustomerAutoCompleteClassAdd.Name = Dr("cityname")
                        CustomerAutoCompleteClass.Add(CustomerAutoCompleteClassAdd)
                    End If
                End While
                Dr.Close()
                MYCommand.Cancel()
                Return CustomerAutoCompleteClass

            Catch ex As Exception

            End Try
        End Function


        <WebMethod(EnableSession:=True)> _
        Public Function supplierAutoCompletefilter(ByVal para1 As String, ByVal para2 As String, ByVal para3 As String, ByVal para4 As String, ByVal para5 As String) As List(Of AutocompleteClass)
            Try
                Dim CustomerAutoCompleteClass As New List(Of AutocompleteClass)
                Dim CustomerAutoCompleteClassAdd As New AutocompleteClass
                Dim strSql As String = "Select  partyname,partycode From partymast where  active=1 "

                If para1 <> "" Then
                    strSql = strSql + " and partyname like '" + para1 + "'%"
                End If

                If para2 <> "" Then
                    strSql = strSql + " and sptypecode ='" + para2 + "'"
                End If

                If para3 <> "" Then
                    strSql = strSql + " and ctrycode ='" + para3 + "'"
                End If

                If para4 <> "" Then
                    strSql = strSql + " and citycode ='" + para4 + "'"
                End If
                If para5 <> "" Then
                    strSql = strSql + " and catcode ='" + para5 + "'"
                End If


                strSql = strSql + " order by partyname "

                Dim MYCommand As New SqlCommand(strSql, clsDBConnect.dbConnectionnew(Session("dbconnectionName").ToString()))

                Dim Dr As SqlDataReader
                Dr = MYCommand.ExecuteReader
                While Dr.Read
                    If Not IsDBNull("partycode") Then
                        CustomerAutoCompleteClassAdd = New AutocompleteClass
                        CustomerAutoCompleteClassAdd.Id = Dr("partycode")
                        CustomerAutoCompleteClassAdd.Name = Dr("partyname")
                        CustomerAutoCompleteClass.Add(CustomerAutoCompleteClassAdd)
                    End If
                End While
                Dr.Close()
                MYCommand.Cancel()
                Return CustomerAutoCompleteClass
                'Dim retlist(2) As String
                'Dim min As String
                'Dim max As String

                'min = objUtils.ExecuteQueryReturnStringValuenew(constr, "select minpax from othcatmast where othgrpcode='TRFS' and othcatcode='" & vehicletype & "'")
                'max = objUtils.ExecuteQueryReturnStringValuenew(constr, "select maxpax from othcatmast where othgrpcode='TRFS' and othcatcode='" & vehicletype & "'")

                'retlist(0) = min
                'retlist(1) = max


                'Return retlist
            Catch ex As Exception

            End Try
        End Function

        <WebMethod()> _
        Public Function GetFlightList(ByVal constr As String, ByVal codeid As String) As clsMaster()
            Dim retlist As New List(Of clsMaster)
            Dim sqlstr As String


            If Trim(codeid) <> "" Then
                sqlstr = "select flightcode,code= flightcode from flightmast where active=1"
                If codeid = "1" Then
                    sqlstr = sqlstr + " and type=1"
                    sqlstr = sqlstr + "   order by flightcode"
                ElseIf codeid = "2" Then
                    sqlstr = sqlstr + " and type=0"
                    sqlstr = sqlstr + "   order by flightcode"
                ElseIf codeid = "4" Then
                    sqlstr = sqlstr + "   order by flightcode"
                Else
                    sqlstr = "select flightcode,code= flightcode from flightmast where active=-1"
                End If

            End If


            retlist = objUtils.FillclsMasternew(constr, sqlstr)
            Return retlist.ToArray()
        End Function

        <WebMethod()> _
        Public Function GetDriverList(ByVal constr As String, ByVal codeid As String) As clsMaster()
            Dim retlist As New List(Of clsMaster)
            Dim sqlstr As String
            If Trim(codeid) <> "" Then
                sqlstr = "select distinct d.drivercode,d.drivername from reservation_online_hotels_transfers t left join drivermaster d on d.drivercode=t.drivercode  where requestid ='" & codeid & "' and ISNULL(d.drivercode,'')<>''"
            End If
            retlist = objUtils.FillclsMasternew(constr, sqlstr)
            Return retlist.ToArray()
        End Function
        <WebMethod()> _
        Public Function GetVehicleList(ByVal constr As String, ByVal codeid As String) As clsMaster()
            Dim retlist As New List(Of clsMaster)
            Dim sqlstr As String

            If Trim(codeid) <> "" Then
                sqlstr = "select vehiclecode,vehiclename from vehiclemaster where othcatcode ='" & codeid & "' and  active=1"

            End If

            retlist = objUtils.FillclsMasternew(constr, sqlstr)
            Return retlist.ToArray()
        End Function
        <WebMethod()> _
        Public Function GetSupplierfleet(ByVal constr As String, ByVal frmdate As String, ByVal todate As String) As clsMaster()

            Dim retlist As New List(Of clsMaster)

            Dim sqlstr As StringBuilder = New StringBuilder()

            sqlstr.AppendFormat("exec sp_Get_supplierfleet '{0}','{1}'", CType(frmdate, Date).ToString("yyyy/MM/dd"), CType(todate, Date).ToString("yyyy/MM/dd"))
            retlist = objUtils.FillclsMasternew(constr, sqlstr.ToString, True)

            Return retlist.ToArray()
        End Function
        <WebMethod()> _
        Public Function GetSupplierDetails(ByVal constr As String, ByVal partycode As String, ByVal frmdate As String, ByVal todate As String) As String
            Dim retlist As String = ""
            Dim sqlstr As String
            Dim ds As DataSet
            Dim objdate As New clsDateTime

            sqlstr = "sp_get_supplier_details '" + partycode + "','" + objdate.ConvertDateromTextBoxToTextYearMonthDay(frmdate) + "','" + objdate.ConvertDateromTextBoxToTextYearMonthDay(todate) + "'"
            ds = objUtils.ExecuteQuerySqlnew(constr, sqlstr)
            If ds.Tables.Count > 0 Then
                If ds.Tables(0).Rows.Count = 1 Then
                    'retlist = "<table  border=1 width=850 > <tr>"
                    'retlist += "<th> <center> Supplier Name </center> </th>" + "<th> <center> Category Name </center></th>" + "<th> <center>Remarks </center></th>" + "</tr>"
                    'For i = 0 To ds.Tables(0).Rows.Count - 1
                    '    retlist += "<tr>"
                    retlist += "</br></br><b>Supplier Name : </b>" + ds.Tables(0).Rows(0)("partyname") + "</br>"
                    retlist += "<b>Category Name : </b>" + ds.Tables(0).Rows(0)("catname") + "</br>"
                    retlist += "<b>Country Name  : </b>" + ds.Tables(0).Rows(0)("ctryname") + "</br>"
                    retlist += "<b>Sector Name   : </b>" + ds.Tables(0).Rows(0)("sectorname") + "</br>"
                    retlist += "<b>Address 1     : </b>" + ds.Tables(0).Rows(0)("add1") + "</br>"
                    retlist += "<b>Address 2     : </b>" + ds.Tables(0).Rows(0)("add2") + "</br>"
                    retlist += "<b>Address 3     : </b>" + ds.Tables(0).Rows(0)("add3") + "</br>"
                    retlist += "<b>Telephone     : </b>" + ds.Tables(0).Rows(0)("tel2") + "</br>"
                    retlist += "<b>Fax           : </b>" + ds.Tables(0).Rows(0)("fax") + "</br>"
                    retlist += "<b>Contact 1     : </b>" + ds.Tables(0).Rows(0)("contact1") + "</br>"
                    retlist += "<b>Contact 2     : </b>" + ds.Tables(0).Rows(0)("contact2") + "</br>"
                    retlist += "<b>Email         : </b>" + ds.Tables(0).Rows(0)("email") + "</br>"

                    'Next
                    'retlist += "</table>"
                    ' retlist = retlist.Replace(Chr(13), "<br/>")
                End If
            End If
            If retlist.Trim = "" Then
                retlist = "<center> ------- No Supplier Details ------- </center>"
            End If
            Return retlist.ToString()
        End Function
        <WebMethod()> _
        Public Function GetDivisionFactorNo(ByVal constr As String, ByVal rmcatcode As String) As String
            Dim strSql As String = ""
            Dim strresult As String = ""
            Try

                If rmcatcode <> "[Select]" Then
                    strSql = "select isnull(units,'') from rmcatmast where rmcatcode='" & rmcatcode & "'"
                    strresult = CType(objUtils.ExecuteQueryReturnSingleValuenew(constr, strSql), String)

                Else
                    strresult = ""
                End If
                Return strresult
            Catch ex As Exception
                Return strresult
            End Try
        End Function
        <WebMethod()> _
        Public Function GetItineraryService(ByVal constr As String, ByVal routecode As String) As String
            Dim strSql As String = ""
            Dim strresult As String = ""
            Try

                If routecode <> "[Select]" Then
                    strSql = "select isnull(remarks,'') from othtypmast where othtypcode='" + routecode + "'"

                    strresult = CType(objUtils.ExecuteQueryReturnSingleValuenew(constr, strSql), String)

                Else
                    strresult = ""
                End If
                Return strresult
            Catch ex As Exception
                Return strresult
            End Try
        End Function
        <WebMethod()> _
        Public Function GetSellCodeCategoryListnew(ByVal constr As String, ByVal suppcode As String) As clsMaster()
            Dim retlist As New List(Of clsMaster)
            Dim sqlstr As String
            sqlstr = "select scatcode, scatname from sellcatmast where active=1"
            If Trim(suppcode) <> "" And Trim(UCase(suppcode)) <> Trim(UCase("[Select]")) Then
                sqlstr = sqlstr + " and sptypecode='" & Trim(suppcode) & "'"
            End If
            sqlstr = sqlstr + " order by scatcode"
            retlist = objUtils.FillclsMasternew(constr, sqlstr, True)
            Return retlist.ToArray()
        End Function
        <WebMethod()> _
        Public Function GetSellNameCategoryListnew(ByVal constr As String, ByVal suppcode As String) As clsMaster()
            Dim retlist As New List(Of clsMaster)
            Dim sqlstr As String
            sqlstr = "select scatname, scatcode from sellcatmast where active=1"
            If Trim(suppcode) <> "" And Trim(UCase(suppcode)) <> Trim(UCase("[Select]")) Then
                sqlstr = sqlstr + " and sptypecode='" & Trim(suppcode) & "'"
            End If
            sqlstr = sqlstr + " order by scatname"
            retlist = objUtils.FillclsMasternew(constr, sqlstr, True)
            Return retlist.ToArray()
        End Function

        <WebMethod()> _
        Public Function GetRoomList(ByVal constr As String, ByVal roomcode As String) As String()
            Dim retlist As Array
            Dim sqlstr As String
            sqlstr = "select rankorder, active from rmtypmast"
            If Trim(roomcode) <> "" And Trim(UCase(roomcode)) <> Trim(UCase("[Select]")) Then
                sqlstr = sqlstr + " where rmtypcode='" & Trim(roomcode) & "'"
            End If
            sqlstr = sqlstr

            retlist = GetQueryReturnStringArraynew(constr, sqlstr, 2)
            Return retlist
        End Function

        <WebMethod(EnableSession:=True)> _
        Public Function GetRoomdetails(ByVal sptypecode As String) As List(Of AutocompleteClass)
            Dim CustomerAutoCompleteClass As New List(Of AutocompleteClass)
            Dim CustomerAutoCompleteClassAdd As New AutocompleteClass
            Try
                Dim result As String
                Dim sqlstr As String
                sqlstr = "select rmtypcode,rmtypname  from rmtypmast where active=1 and sptypecode='" & sptypecode & "'"



                ''+ " order by rmtypname"

                Dim MYCommand As New SqlCommand(sqlstr, clsDBConnect.dbConnectionnew(Session("dbconnectionName").ToString()))

                Dim Dr As SqlDataReader
                Dr = MYCommand.ExecuteReader
                While Dr.Read
                    If Not IsDBNull("rmtypcode") Then
                        CustomerAutoCompleteClassAdd = New AutocompleteClass
                        CustomerAutoCompleteClassAdd.Id = Dr("rmtypcode")
                        CustomerAutoCompleteClassAdd.Name = Dr("rmtypname")

                        CustomerAutoCompleteClass.Add(CustomerAutoCompleteClassAdd)
                    End If
                End While
                Dr.Close()
                MYCommand.Cancel()
                Return CustomerAutoCompleteClass

            Catch ex As Exception

            End Try


        End Function

        <WebMethod()> _
        Public Function GetCtryCodeCategoryListnew(ByVal constr As String, ByVal ctrycode As String) As clsMaster()
            Dim retlist As New List(Of clsMaster)
            Dim sqlstr As String
            sqlstr = "select ctrycode,citycode from othtypmast"
            If Trim(ctrycode) <> "" And Trim(UCase(ctrycode)) <> Trim(UCase("[Select]")) Then
                sqlstr = sqlstr + " where  othtypmast.othtypcode='" & Trim(ctrycode) & "'"
            End If
            sqlstr = sqlstr + " order by othtypmast.othtypcode"
            retlist = objUtils.FillclsMasternew(constr, sqlstr, True)
            Return retlist.ToArray()
        End Function

        <WebMethod()> _
        Public Function GetTrfSelType(ByVal constr As String, ByVal custcode As String) As clsMaster()
            Dim retlist As New List(Of clsMaster)
            Dim sqlstr As String

            sqlstr = "select t.trfsellname,t.trfsellcode  from trfsellmast t inner join agentmast ag on ag.trfsellcode =t.trfsellcode"

            If Trim(custcode) <> "" And Trim(UCase(custcode)) <> Trim(UCase("[Select]")) Then
                sqlstr = sqlstr + " and ag.agentcode='" & Trim(custcode) & "'"
            End If
            retlist = objUtils.FillclsMasternew(constr, sqlstr, True)
            Return retlist.ToArray()
        End Function




        <WebMethod()> _
        Public Function GetSellingCurrNamecostvalue(ByVal connstr As String, ByVal sellcode As String) As String
            Dim retcurrname As String = ""
            retcurrname = CType(objUtils.ExecuteQueryReturnSingleValuenew(connstr, "select c.currname from currmast c, agentmast sc where c.currcode  = sc.currcode " _
            & " and sc.agentcode='" & sellcode & "'"), String)
            Return retcurrname
        End Function

        <WebMethod()> _
        Public Function GetEndingPoint(ByVal connstr As String, ByVal sellcode As String) As clsMaster()
            Dim retlist As New List(Of clsMaster)
            Dim sqlstr As String
            sqlstr = "select airportbordername,airportbordercode from view_airportsectormast where airportbordercode =(select sectorgroupcode from sectormaster where sectorcode= (select  sectorcode from partymast where partycode='" & sellcode & "'))"
            retlist = objUtils.FillclsMasternew(connstr, sqlstr, True)
            Return retlist.ToArray()
        End Function



        <WebMethod()> _
        Public Function GetStartingPointList(ByVal constr As String) As clsMaster()
            Dim retlist As New List(Of clsMaster)
            Dim sqlstr As String
            sqlstr = "select airportbordername,airportbordercode from view_airportsectormast where shifting in (0,1)"

            sqlstr = sqlstr + " order by airportbordername"
            retlist = objUtils.FillclsMasternew(constr, sqlstr, True)
            Return retlist.ToArray()
        End Function
        <WebMethod()> _
        Public Function GetEndingPointList(ByVal constr As String) As clsMaster()
            Dim retlist As New List(Of clsMaster)
            Dim sqlstr As String
            sqlstr = "select airportbordername,airportbordercode from view_airportsectormast where shifting=0"

            sqlstr = sqlstr + " order by airportbordername"
            retlist = objUtils.FillclsMasternew(constr, sqlstr, True)
            Return retlist.ToArray()
        End Function

        <WebMethod()> _
        Public Function FillPriceList(ByVal constr As String, ByVal sellingCode As String, ByVal tdate As String,
                                     ByVal ttypecode As String, ByVal cartype As String,
                                     ByVal startingpoint As String, ByVal endingpoint As String) As String()
            Dim retlist As Array
            Dim cnt As Long = 1
            Dim frmdate As Date = tdate
            tdate = Format(frmdate, "yyyy/MM/dd")
            Dim sqlstr As StringBuilder = New StringBuilder()
            sqlstr.AppendFormat("exec sp_get_transfer_saleprice '{0}','{1}','{2}','{3}','{4}','{5}'", sellingCode, tdate, ttypecode, cartype, startingpoint, endingpoint)
            retlist = objUtils.FillArraynew(constr, sqlstr.ToString, cnt)
            Return retlist
        End Function

        <WebMethod(EnableSession:=True)> _
        Public Function FillCostList(ByVal constr As String, ByVal tdate As String,
                                     ByVal ttypecode As Integer, ByVal cartype As String,
                                     ByVal startingpoint As String, ByVal endingpoint As String, ByVal partycode As String) As String()
            Dim retlist As Array
            Dim cnt As Long = 1
            Dim frmdate As Date = tdate
            tdate = Format(frmdate, "yyyy/MM/dd")
            Dim sqlstr As StringBuilder = New StringBuilder()
            sqlstr.AppendFormat("exec sp_get_transfer_costprice '{0}',{1},'{2}','{3}','{4}','{5}'", tdate, ttypecode, cartype, startingpoint, endingpoint, partycode)
            retlist = objUtils.FillArraynew(Session("dbConnectionName"), sqlstr.ToString, cnt)
            Return retlist
        End Function






        <WebMethod()> _
        Public Function GetFlightArrivalList(ByVal constr As String, ByVal codeid As String, ByVal datevalue As String, ByVal code As String) As clsMaster()
            Dim retlist As New List(Of clsMaster)
            If datevalue <> "" Then
                Dim otherdtemp As Date = datevalue
                datevalue = Format(otherdtemp, "yyyy/MM/dd")
            End If
            Dim sqlstr As StringBuilder = New StringBuilder()
            sqlstr.AppendFormat("exec sp_get_flightlist '{0}','{1}','{2}'", codeid, datevalue, code)


            retlist = objUtils.FillclsMasternew(constr, sqlstr.ToString, True)
            Return retlist.ToArray()
        End Function


        <WebMethod()> _
        Public Function GetFlightTime(ByVal constr As String, ByVal tranfertype As String, ByVal datevalue As String, ByVal code As String) As String()
            Dim retlist As Array
            Dim cnt As Long = 1
            If datevalue <> "" Then
                Dim otherdtemp As Date = datevalue
                datevalue = Format(otherdtemp, "yyyy/MM/dd")
            End If
            Dim sqlstr As StringBuilder = New StringBuilder()
            sqlstr.AppendFormat("exec sp_get_flighttime  '{0}','{1}','{2}'", tranfertype, datevalue, code)
            retlist = objUtils.FillArraynew(constr, sqlstr.ToString, cnt)
            Return retlist
        End Function





        <WebMethod()> _
        Public Function GetSellingCurrName(ByVal connstr As String, ByVal sellcode As String) As clsMaster()

            Dim retlist As New List(Of clsMaster)
            Dim sqlstr As String

            sqlstr = "select c.currname,c.currcode from currmast c, agentmast sc where c.currcode  = sc.currcode " _
            & " and sc.agentcode='" & sellcode & "'"

            sqlstr = sqlstr + " order by currcode"
            retlist = objUtils.FillclsMasternew(connstr, sqlstr, True)

            Return retlist.ToArray()
        End Function



        <WebMethod()> _
        Public Function GetValue(ByVal constr As String) As clsMaster()
            Dim retlist As New List(Of clsMaster)
            Dim sqlstr As String
            sqlstr = "select airportbordername,airportbordercode from view_airportsectormast where shifting=0"

            sqlstr = sqlstr + " order by airportbordername"
            retlist = objUtils.FillclsMasternew(constr, sqlstr, True)
            Return retlist.ToArray()
        End Function

        <WebMethod()> _
        Public Function GetFlightValue(ByVal constr As String) As clsMaster()
            Dim retlist As New List(Of clsMaster)
            Dim sqlstr As String
            sqlstr = "select flightcode,flight_tranid from flightmast"

            sqlstr = sqlstr + " order by flightcode"
            retlist = objUtils.FillclsMasternew(constr, sqlstr, True)
            Return retlist.ToArray()
        End Function


        <WebMethod(EnableSession:=True)> _
        Public Function CustomerAutoCompleteExcursion(ByVal para1 As String, ByVal para2 As String) As List(Of AutocompleteClass)
            Try
                Dim CustomerAutoCompleteClass As New List(Of AutocompleteClass)
                Dim CustomerAutoCompleteClassAdd As New AutocompleteClass
                Dim strSql As String = "Select  Agentcode,Agentname From AgentMast where active=1 "
                If para1 <> "" Then
                    strSql = strSql + " and  Agentname Like '%" & para1 & "%'"
                End If

                strSql = strSql + " order by Agentname "

                Dim MYCommand As New SqlCommand(strSql, clsDBConnect.dbConnectionnew(Session("dbconnectionName").ToString()))

                Dim Dr As SqlDataReader
                Dr = MYCommand.ExecuteReader
                While Dr.Read
                    If Not IsDBNull("Agentcode") Then
                        CustomerAutoCompleteClassAdd = New AutocompleteClass
                        CustomerAutoCompleteClassAdd.Id = Dr("AgentCode")
                        CustomerAutoCompleteClassAdd.Name = Dr("AgentName")
                        CustomerAutoCompleteClass.Add(CustomerAutoCompleteClassAdd)
                    End If
                End While
                Dr.Close()
                MYCommand.Cancel()
                Return CustomerAutoCompleteClass
                'Dim retlist(2) As String
                'Dim min As String
                'Dim max As String

                'min = objUtils.ExecuteQueryReturnStringValuenew(constr, "select minpax from othcatmast where othgrpcode='TRFS' and othcatcode='" & vehicletype & "'")
                'max = objUtils.ExecuteQueryReturnStringValuenew(constr, "select maxpax from othcatmast where othgrpcode='TRFS' and othcatcode='" & vehicletype & "'")

                'retlist(0) = min
                'retlist(1) = max


                'Return retlist
            Catch ex As Exception

            End Try
        End Function

        <WebMethod(EnableSession:=True)> _
        Public Function SpersonmastAutoCompleteExcursion(ByVal para1 As String, ByVal para2 As String) As List(Of AutocompleteClass)
            Try
                Dim SpersonmastAutoCompleteClass As New List(Of AutocompleteClass)
                Dim SpersonmastAutoCompleteClassAdd As New AutocompleteClass
                Dim strSql As String = "Select  spersoncode,spersonname From spersonmast where active=1 "
                If para1 <> "" Then
                    strSql = strSql + " and  spersonname  Like '%" & para1 & "%'"
                End If

                strSql = strSql + " order by spersonname "

                Dim MYCommand As New SqlCommand(strSql, clsDBConnect.dbConnectionnew(Session("dbconnectionName").ToString()))

                Dim Dr As SqlDataReader
                Dr = MYCommand.ExecuteReader
                While Dr.Read
                    If Not IsDBNull("spersoncode") Then
                        SpersonmastAutoCompleteClassAdd = New AutocompleteClass
                        SpersonmastAutoCompleteClassAdd.Id = Dr("spersoncode")
                        SpersonmastAutoCompleteClassAdd.Name = Dr("spersonname")
                        SpersonmastAutoCompleteClass.Add(SpersonmastAutoCompleteClassAdd)
                    End If
                End While
                Dr.Close()
                MYCommand.Cancel()
                Return SpersonmastAutoCompleteClass
                'Dim retlist(2) As String
                'Dim min As String
                'Dim max As String

                'min = objUtils.ExecuteQueryReturnStringValuenew(constr, "select minpax from othcatmast where othgrpcode='TRFS' and othcatcode='" & vehicletype & "'")
                'max = objUtils.ExecuteQueryReturnStringValuenew(constr, "select maxpax from othcatmast where othgrpcode='TRFS' and othcatcode='" & vehicletype & "'")

                'retlist(0) = min
                'retlist(1) = max


                'Return retlist
            Catch ex As Exception

            End Try
        End Function
        <WebMethod(EnableSession:=True)> _
        Public Function CustomerAutoCompleteExcursionRequest(ByVal para1 As String, ByVal para2 As String) As List(Of AutocompleteClass)
            Try
                Dim CustomerAutoCompleteClass As New List(Of AutocompleteClass)
                Dim CustomerAutoCompleteClassAdd As New AutocompleteClass
                Dim strSql As String = "Select  Agentcode,Agentname From AgentMast where active=1 and agentcode not in( select Agentcode from agents_locked)"
                'If para1 <> "" Then
                '    strSql = strSql + " and  Agentname Like '%" & para1 & "%'"
                'End If

                strSql = strSql + " order by Agentname "

                Dim MYCommand As New SqlCommand(strSql, clsDBConnect.dbConnectionnew(Session("dbconnectionName").ToString()))

                Dim Dr As SqlDataReader
                Dr = MYCommand.ExecuteReader
                While Dr.Read
                    If Not IsDBNull("Agentcode") Then
                        CustomerAutoCompleteClassAdd = New AutocompleteClass
                        CustomerAutoCompleteClassAdd.Id = Dr("AgentCode")
                        CustomerAutoCompleteClassAdd.Name = Dr("AgentName")
                        CustomerAutoCompleteClass.Add(CustomerAutoCompleteClassAdd)
                    End If
                End While
                Dr.Close()
                MYCommand.Cancel()
                Return CustomerAutoCompleteClass

            Catch ex As Exception

            End Try
        End Function
        <WebMethod(EnableSession:=True)> _
        Public Function SupAutoComplete(ByVal para1 As String, ByVal para2 As String) As List(Of AutocompleteClass)
            Try
                Dim CustomerAutoCompleteClass As New List(Of AutocompleteClass)
                Dim CustomerAutoCompleteClassAdd As New AutocompleteClass
                Dim strSql As String = "Select  partycode,partyname From partymast where active=1"
                If para1 <> "" Then
                    strSql = strSql + " and  partyname Like '%" & para1 & "%'"
                End If

                strSql = strSql + " order by partyname "

                Dim MYCommand As New SqlCommand(strSql, clsDBConnect.dbConnectionnew(Session("dbconnectionName").ToString()))

                Dim Dr As SqlDataReader
                Dr = MYCommand.ExecuteReader
                While Dr.Read
                    If Not IsDBNull("Agentcode") Then
                        CustomerAutoCompleteClassAdd = New AutocompleteClass
                        CustomerAutoCompleteClassAdd.Id = Dr("partycode")
                        CustomerAutoCompleteClassAdd.Name = Dr("partyname")
                        CustomerAutoCompleteClass.Add(CustomerAutoCompleteClassAdd)
                    End If
                End While
                Dr.Close()
                MYCommand.Cancel()
                Return CustomerAutoCompleteClass

            Catch ex As Exception

            End Try
        End Function
        <WebMethod(EnableSession:=True)> _
        Public Function GetDriverPhone(ByVal drivercode As String) As String
            Dim retcurrcode As String = ""
            retcurrcode = CType(objUtils.GetDBFieldFromStringnew(Session("dbConnectionName"), "drivermaster", "mobileno", "drivercode", drivercode), String)
            Return retcurrcode
        End Function


        <WebMethod()> _
        Public Function GetMarketExcursion(ByVal constr As String, ByVal partycode As String) As clsMaster()
            Dim retlist As New List(Of clsMaster)

            Dim sqlstr As String
            sqlstr = "select plgrpname,plgrpcode from plgrpmast where active=1"

            'If Trim(partycode) <> "" And Trim(UCase(partycode)) <> Trim(UCase("[Select]")) Then
            '    sqlstr = sqlstr + " where  partyallot.partycode='" & partycode & "'"
            'End If
            'sqlstr = sqlstr + "  order by plgrpmast.plgrpname "
            retlist = objUtils.FillclsMasternew(constr, sqlstr, True)
            Return retlist.ToArray()
        End Function

        <WebMethod()> _
        Public Function GetMarketExcursionCustomer(ByVal constr As String, ByVal partycode As String) As String
            Dim result As String

            Dim sqlstr As String
            sqlstr = "select isnull(plgrpmast.plgrpcode,'') from plgrpmast inner join agentmast on agentmast.plgrpcode=plgrpmast.plgrpcode"
            If Trim(partycode) <> "" And Trim(UCase(partycode)) <> Trim(UCase("[Select]")) Then
                sqlstr = sqlstr + " where plgrpmast.active=1 and agentmast.agentcode='" & partycode & "'"
            End If
            sqlstr = sqlstr + "  order by plgrpmast.plgrpname "
            result = objUtils.ExecuteQueryReturnStringValuenew(constr, sqlstr.ToString)
            Return result
        End Function

        <WebMethod()> _
        Public Function GetSellingTypeExcursion(ByVal constr As String, ByVal plgrpcode As String) As clsMaster()
            Dim retlist As New List(Of clsMaster)
            retlist = objUtils.FillclsMasternew(constr, "select distinct excsellname,excsellcode from excsellmast where active=1 order by excsellname", True)
            Return retlist.ToArray()
        End Function

        <WebMethod()> _
        Public Function GetSellingTypeExcursionCustomer(ByVal constr As String, ByVal partycode As String) As String
            Dim result As String = ""

            Dim sqlstr As String
            sqlstr = "select isnull(excsellmast.excsellcode,'') from excsellmast inner join agentmast on agentmast.excsellcode=excsellmast.excsellcode"
            If Trim(partycode) <> "" And Trim(UCase(partycode)) <> Trim(UCase("[Select]")) Then
                sqlstr = sqlstr + " where excsellmast.active=1 and agentmast.agentcode='" & partycode & "'"
            End If
            sqlstr = sqlstr + "  order by excsellmast.excsellname "
            result = objUtils.ExecuteQueryReturnStringValuenew(constr, sqlstr.ToString)
            Return result
        End Function

        <WebMethod()> _
        Public Function GetCurrencyExcursionCustomer(ByVal constr As String, ByVal custcode As String) As String
            Dim result As String
            Dim sqlstr As String
            sqlstr = "SELECT isnull(agentmast.currcode,'') FROM agentmast INNER JOIN currrates ON "
            sqlstr += "agentmast.currcode = currrates.currcode WHERE tocurr=(select option_selected from reservation_parameters where param_id=457) "
            If Trim(custcode) <> "" And Trim(UCase(custcode)) <> Trim(UCase("[Select]")) Then
                sqlstr = sqlstr + " and agentcode='" & Trim(custcode) & "'"
            End If
            result = objUtils.ExecuteQueryReturnStringValuenew(constr, sqlstr.ToString)
            Return result
        End Function

        <WebMethod()> _
        Public Function GetExchRate4Currency(ByVal constr As String, ByVal custcode As String, ByVal tocurr As String) As String
            Dim result As String
            Dim sqlstr As String
            sqlstr = "SELECT currrates.convrate FROM agentmast INNER JOIN currrates ON "
            sqlstr += "agentmast.currcode = currrates.currcode  "
            If Trim(custcode) <> "" And Trim(UCase(custcode)) <> Trim(UCase("[Select]")) Then
                sqlstr = sqlstr + " where agentcode='" & Trim(custcode) & "'"
            End If
            If Trim(tocurr) <> "" And Trim(UCase(tocurr)) <> Trim(UCase("[Select]")) Then
                sqlstr = sqlstr + " and tocurr='" & Trim(tocurr) & "'"
            End If
            result = objUtils.ExecuteQueryReturnStringValuenew(constr, sqlstr)
            Return result
        End Function



        <WebMethod()> _
        Public Function GetExcursionSubGroup(ByVal constr As String, ByVal code As String) As clsMaster()
            Dim retlist As New List(Of clsMaster)

            Dim sqlstr As String
            Dim otypecode1, otypecode2 As String
            otypecode1 = objUtils.ExecuteQueryReturnStringValuenew(constr, "select option_selected from reservation_parameters where param_id=1104")
            otypecode2 = objUtils.ExecuteQueryReturnStringValuenew(constr, "select option_selected from reservation_parameters where param_id=1105")

            sqlstr = "select rtrim(ltrim(othgrpname))othgrpname,rtrim(ltrim(othgrpcode))othgrpcode from othgrpmast"
            If Trim(code) <> "" And Trim(UCase(code)) <> Trim(UCase("[Select]")) Then
                sqlstr = sqlstr + " where active=1 and  othmaingrpcode='" & code & "'"
            Else
                sqlstr = sqlstr + " where othmaingrpcode in('" & otypecode1 & "'" & ",'" & otypecode2 & "') and active=1"
            End If
            sqlstr = sqlstr + "  order by othgrpname "
            retlist = objUtils.FillclsMasternew(constr, sqlstr, True)
            Return retlist.ToArray()
        End Function


        <WebMethod()> _
        Public Function GetExcursionGroupType(ByVal constr As String, ByVal code As String) As clsMaster()
            Dim retlist As New List(Of clsMaster)

            Dim sqlstr As String
            Dim otypecode1, otypecode2 As String
            otypecode1 = objUtils.ExecuteQueryReturnStringValuenew(constr, "select option_selected from reservation_parameters where param_id=1104")
            otypecode2 = objUtils.ExecuteQueryReturnStringValuenew(constr, "select option_selected from reservation_parameters where param_id=1105")

            If Trim(code) <> "" And Trim(UCase(code)) <> Trim(UCase("[Select]")) Then
                sqlstr = " select rtrim(ltrim(othtypname))othtypname,rtrim(ltrim(othtypcode))othtypcode from othtypmast a  inner join othgrpmast b on a.othgrpcode=b.othgrpcode and b.othmaingrpcode in('" & code & "' ) and a.active=1 order by a.othtypname"
            Else
                sqlstr = " select rtrim(ltrim(othtypname))othtypname,rtrim(ltrim(othtypcode))othtypcode from othtypmast a  inner join othgrpmast b on a.othgrpcode=b.othgrpcode and b.othmaingrpcode in('" & otypecode1 & "'" & ",'" & otypecode2 & "') and a.active=1 order by a.othtypname"
            End If

            retlist = objUtils.FillclsMasternew(constr, sqlstr, True)
            Return retlist.ToArray()
        End Function

        <WebMethod()> _
        Public Function GetMain_SubGroupByType(ByVal constr As String, ByVal excursionType As String) As String()
            Dim retlist(2) As String
            Dim maingroup, subgroup As String
            'partycode = objUtils.ExecuteQueryReturnStringValuenew(constr, "select partycode from meetassisttemp where requestid='" & requestid & "' and sno='" & rlineno & "'")
            maingroup = objUtils.ExecuteQueryReturnStringValuenew(constr, "select distinct a.othmaingrpcode from othmaingrpmast a inner join othgrpmast b on a.othmaingrpcode=b.othmaingrpcode inner join othtypmast c on b.othgrpcode= c.othgrpcode where c.othtypcode='" & excursionType & "'")
            subgroup = objUtils.ExecuteQueryReturnStringValuenew(constr, "select distinct b.othgrpcode from othmaingrpmast a inner join othgrpmast b on a.othmaingrpcode=b.othmaingrpcode inner join othtypmast c on b.othgrpcode= c.othgrpcode where c.othtypcode='" & excursionType & "'")

            retlist(0) = maingroup
            retlist(1) = subgroup

            Return retlist
        End Function

        <WebMethod()> _
        Public Function GetCostCurr_CostConvRate(ByVal constr As String, ByVal excursionProvider As String) As String()
            Dim retlist(2) As String
            Dim costCurr, costCurrConvRate As String
            costCurr = objUtils.ExecuteQueryReturnStringValuenew(constr, "select isnull(currcode,'') from partymast where partycode ='" & excursionProvider & "'")
            costCurrConvRate = objUtils.ExecuteQueryReturnStringValuenew(constr, "select isnull(convrate,0) from currrates where  currcode=(select option_selected from reservation_parameters where param_id=457) and tocurr='" & costCurr & "'")

            retlist(0) = costCurr
            retlist(1) = costCurrConvRate

            Return retlist
        End Function

        <WebMethod()> _
        Public Function GetSellingPrices(ByVal constr As String, ByVal tourdate As String, ByVal requestdate As String, ByVal othtypcode As String, ByVal excsellcode As String, ByVal spersoncode As String) As String()
            Dim retlist As Array
            Dim cnt As Long = 4

            Dim sqlstr As StringBuilder = New StringBuilder()
            sqlstr.AppendFormat("exec sp_get_exc_sellprices  '{0}','{1}','{2}','{3}','{4}'", objDate.ConvertDateromTextBoxToTextYearMonthDay(tourdate), objDate.ConvertDateromTextBoxToTextYearMonthDay(requestdate), othtypcode, excsellcode, spersoncode)
            retlist = objUtils.FillArraynew(constr, sqlstr.ToString, cnt)
            Return retlist
        End Function




        <WebMethod(EnableSession:=True)> _
        Public Function hotelAutoComplete(ByVal para1 As String, ByVal para2 As String) As List(Of AutocompleteClass)
            Try
                Dim CustomerAutoCompleteClass As New List(Of AutocompleteClass)
                Dim CustomerAutoCompleteClassAdd As New AutocompleteClass
                Dim strSql As String = "Select  partycode,partyname From partymast where active=1"
                'If para1 <> "" Then
                '    strSql = strSql + " and  Agentname Like '%" & para1 & "%'"
                'End If

                strSql = strSql + " order by partyname"

                Dim MYCommand As New SqlCommand(strSql, clsDBConnect.dbConnectionnew(Session("dbconnectionName").ToString()))

                Dim Dr As SqlDataReader
                Dr = MYCommand.ExecuteReader
                While Dr.Read
                    If Not IsDBNull("partycode") Then
                        CustomerAutoCompleteClassAdd = New AutocompleteClass
                        CustomerAutoCompleteClassAdd.Id = Dr("partycode")
                        CustomerAutoCompleteClassAdd.Name = Dr("partyname")
                        CustomerAutoCompleteClass.Add(CustomerAutoCompleteClassAdd)
                    End If
                End While
                Dr.Close()
                MYCommand.Cancel()
                Return CustomerAutoCompleteClass

            Catch ex As Exception

            End Try
        End Function

        <WebMethod()> _
        Public Function GetExcursionProvider(ByVal constr As String, ByVal txtTourDate As String, ByVal txtRequestDate As String, ByVal ddlExcursionType As String) As clsMaster()
            Dim retlist As New List(Of clsMaster)
            Dim sqlstr As String

            If Trim(txtTourDate) <> "" And Trim(txtRequestDate) <> "" And Trim(ddlExcursionType) <> "[Select]" Then
                sqlstr = "exec sp_get_exc_providers '" & objDate.ConvertDateromTextBoxToTextYearMonthDay(txtTourDate.Trim) & "','" & objDate.ConvertDateromTextBoxToTextYearMonthDay(txtTourDate.Trim) & "','" & ddlExcursionType.Trim & "'"
            Else
                sqlstr = "exec sp_get_exc_providers_nofilter "
            End If

            retlist = objUtils.FillclsMasterSP(constr, sqlstr, True)
            Return retlist.ToArray()
        End Function


        <WebMethod()> _
        Public Function GetCostPrices(ByVal constr As String, ByVal tourdate As String, ByVal requestdate As String, ByVal othtypcode As String, ByVal partycode As String) As String()
            Dim retlist As Array
            Dim cnt As Long = 4

            Dim sqlstr As StringBuilder = New StringBuilder()
            sqlstr.AppendFormat("exec sp_get_exc_costprices  '{0}','{1}','{2}','{3}'", objDate.ConvertDateromTextBoxToTextYearMonthDay(tourdate), objDate.ConvertDateromTextBoxToTextYearMonthDay(requestdate), othtypcode, partycode)
            retlist = objUtils.FillArraynew(constr, sqlstr.ToString, cnt)
            Return retlist
        End Function


        <WebMethod()> _
        Public Function GetFlightCodeExcursion(ByVal constr As String, ByVal type As Integer) As clsMaster()
            Dim retlist As New List(Of clsMaster)
            Dim sqlstr As String
            sqlstr = "select flightcode,flight_tranid from flightmast where active=1 and type=" & type

            sqlstr = sqlstr + " order by flightcode"
            retlist = objUtils.FillclsMasternew(constr, sqlstr, True)
            Return retlist.ToArray()
        End Function

        <WebMethod()> _
        Public Function GetFlightTimeExcursion(ByVal constr As String, ByVal tranfertype As String, ByVal datevalue As String, ByVal code As String) As String()
            Dim retlist As Array
            Dim cnt As Long = 2
            If datevalue <> "" Then
                Dim otherdtemp As Date = datevalue
                datevalue = Format(otherdtemp, "yyyy/MM/dd")
            End If
            Dim sqlstr As StringBuilder = New StringBuilder()
            sqlstr.AppendFormat("exec sp_get_flighttime_Excursion  '{0}','{1}','{2}'", tranfertype, datevalue, code)
            retlist = objUtils.FillArraynew(constr, sqlstr.ToString, cnt)
            Return retlist
        End Function

        <WebMethod()> _
        Public Function GetExcursionTypeCodeByGroupCode(ByVal constr As String, ByVal code As String) As clsMaster()
            Dim retlist As New List(Of clsMaster)

            Dim sqlstr As String
            Dim otypecode1, otypecode2 As String
            otypecode1 = objUtils.ExecuteQueryReturnStringValuenew(constr, "select option_selected from reservation_parameters where param_id=1104")
            otypecode2 = objUtils.ExecuteQueryReturnStringValuenew(constr, "select option_selected from reservation_parameters where param_id=1105")

            If Trim(code) <> "" And Trim(UCase(code)) <> Trim(UCase("[Select]")) Then
                sqlstr = " select rtrim(ltrim(othtypcode))othtypcode,rtrim(ltrim(othtypname))othtypname from othtypmast a  inner join othgrpmast b on a.othgrpcode=b.othgrpcode and b.othmaingrpcode in('" & otypecode1 & "'" & ",'" & otypecode2 & "') and a.active=1 and isnull(ticketsreqd,0)=1 and a.othgrpcode='" & code.Trim & "' order by a.othtypcode"
            End If

            retlist = objUtils.FillclsMasternew(constr, sqlstr, True)
            Return retlist.ToArray()
        End Function

        <WebMethod()> _
        Public Function GetExcursionTypeNameByGroupCode(ByVal constr As String, ByVal code As String) As clsMaster()
            Dim retlist As New List(Of clsMaster)

            Dim sqlstr As String
            Dim otypecode1, otypecode2 As String
            otypecode1 = objUtils.ExecuteQueryReturnStringValuenew(constr, "select option_selected from reservation_parameters where param_id=1104")
            otypecode2 = objUtils.ExecuteQueryReturnStringValuenew(constr, "select option_selected from reservation_parameters where param_id=1105")

            If Trim(code) <> "" And Trim(UCase(code)) <> Trim(UCase("[Select]")) Then
                sqlstr = " select rtrim(ltrim(othtypname))othtypname,rtrim(ltrim(othtypcode))othtypcode from othtypmast a  inner join othgrpmast b on a.othgrpcode=b.othgrpcode and b.othmaingrpcode in('" & otypecode1 & "'" & ",'" & otypecode2 & "') and a.active=1 and isnull(ticketsreqd,0)=1 and a.othgrpcode='" & code.Trim & "' order by a.othtypcode"
            End If

            retlist = objUtils.FillclsMasternew(constr, sqlstr, True)
            Return retlist.ToArray()
        End Function

        <WebMethod()> _
        Public Function FillCarTypeNew(ByVal constr As String, ByVal requestid As String, ByVal ddlarr As Integer, ByVal ddlpickup As Integer, ByVal ddldropoff As Integer,
                                    ByVal hotellineno As Integer, ByVal rmtyplineno As Integer, ByVal rmcatcode As String,
                                    ByVal roomno As String, ByVal arrdeplineno As Integer,
                                    ByVal hdnTransferdate As String, ByVal txtadults As String, ByVal txtchild As String,
                                    ByVal chkoverride As String, ByVal txtnoveh As String, ByVal ddlpickupPoint As String, ByVal ddldropoffPoint As String) As clsMaster()
            Dim retlist As New List(Of clsMaster)

            Dim otherdtemp As Date = hdnTransferdate
            hdnTransferdate = Format(otherdtemp, "yyyy/MM/dd")
            If roomno = "" Then
                roomno = 0
            End If
            'roomno = "'" & roomno & "'"
            'rmcatcode = "'" & rmcatcode & "'"

            If txtadults = "" Or txtadults = 0 Then
                txtadults = 1
            End If
            If txtchild = "" Then
                txtchild = 0
            End If

            If txtnoveh = "" Then
                txtnoveh = 1
            End If

            Dim sqlstr As StringBuilder = New StringBuilder()
            sqlstr.AppendFormat("exec sp_get_transfer_vehicletype {0},{1},{2},'{3}','{4}','{5}',{6},{7},'{8}',{9},{10},'{11}',{12},{13},{14},{15} ", Val(ddlarr), ddlpickup, ddldropoff, ddlpickupPoint, ddldropoffPoint, requestid, hotellineno, rmtyplineno, rmcatcode, roomno, arrdeplineno, hdnTransferdate, txtadults, txtchild, chkoverride, txtnoveh)

            'sqlstr.AppendFormat("exec sp_get_transfer_vehicletype 1,1,2,'A-DXBT3','H-000051','00000739',0,0,'',0,1,'2014/06/19',2,0,1")
            retlist = objUtils.FillclsMasternew(constr, sqlstr.ToString, True)
            Return retlist.ToArray()

        End Function

        <WebMethod()> _
        Public Function FillsupplierTypenew(ByVal constr As String, ByVal requestid As String, ByVal transferdate1 As String, ByVal ddlcartype1 As String, ByVal freeform As Integer, ByVal htltrnsf As Integer, ByVal ddlarr As Integer, ByVal ddlpickup As String, ByVal ddldropoff As String, ByVal ddlpick As String, ByVal ddldrop As String
                                    ) As clsMaster()
            Dim retlist As New List(Of clsMaster)

            Dim otherdtemp As Date = transferdate1
            transferdate1 = Format(otherdtemp, "yyyy/MM/dd")



            Dim sqlstr As StringBuilder = New StringBuilder()
            sqlstr.AppendFormat("exec sp_get_transfer_supplier '{0}','{1}','{2}',{3},{4},{5},{6},{7},'{8}','{9}' ", transferdate1, ddlcartype1, requestid, freeform, htltrnsf, ddlarr, ddlpickup, ddldropoff, ddlpick, ddldrop)

            'sqlstr.AppendFormat("exec sp_get_transfer_vehicletype 1,1,2,'A-DXBT3','H-000051','00000739',0,0,'',0,1,'2014/06/19',2,0,1")
            retlist = objUtils.FillclsMasternew(constr, sqlstr.ToString, True)
            Return retlist.ToArray()

        End Function
        <WebMethod()> _
        Public Function fillpricesnew(ByVal constr As String, ByVal requestid As String, ByVal transferdate1 As String, ByVal ddlcartype1 As String,
                                   ByVal hotellineno As Integer, ByVal rmtyplineno As Integer, ByVal rmcatcode As String,
                                   ByVal roomno As String, ByVal arrdeplineno As Integer, ByVal txtnoveh As String, ByVal webprice As Integer, ByVal ddlarr As Integer, ByVal picktype As Integer, ByVal droptype As Integer,
                                   ByVal ddlpickupPoint As String, ByVal ddldropoffPoint As String) As String()

            Dim otherdtemp As Date = transferdate1
            transferdate1 = Format(otherdtemp, "yyyy/MM/dd")
            'Dim webprice As Integer
            'webprice = 0
            If roomno = "" Then
                roomno = 0
            End If
            'roomno = "'" & roomno & "'"

            If txtnoveh = "" Then
                txtnoveh = 0
            End If

            txtnoveh = Val(txtnoveh).ToString

            Dim sqlstr As StringBuilder = New StringBuilder()
            sqlstr.AppendFormat("exec sp_get_transfer_prices '{0}','{1}','{2}',{3},{4},'{5}',{6},{7},{8},{9},{10},{11},{12},'{13}','{14}' ", transferdate1, ddlcartype1, requestid, hotellineno, rmtyplineno, rmcatcode, roomno, arrdeplineno, txtnoveh, webprice, ddlarr, picktype, droptype, ddlpickupPoint, ddldropoffPoint)
            Dim retlist As Array
            'sqlstr.AppendFormat("exec sp_get_transfer_vehicletype 1,1,2,'A-DXBT3','H-000051','00000739',0,0,'',0,1,'2014/06/19',2,0,1")
            retlist = objUtils.FillArraynew(constr, sqlstr.ToString, 8)
            Return retlist

        End Function
        <WebMethod()> _
        Public Function fillcostpricesnew(ByVal constr As String, ByVal transferdate1 As String, ByVal ddlcartype1 As String, ByVal requestid As String,
                                     ByVal ddlsupplier As String, ByVal ddlarr As Integer, ByVal picktype As Integer, ByVal droptype As Integer,
                                   ByVal ddlpickupPoint As String, ByVal ddldropoffPoint As String) As String()

            Dim otherdtemp As Date = transferdate1
            transferdate1 = Format(otherdtemp, "yyyy/MM/dd")



            Dim sqlstr As StringBuilder = New StringBuilder()
            sqlstr.AppendFormat("exec sp_get_tplist_prices_Transfers_cost '{0}','{1}','{2}','{3}',{4},{5},{6},'{7}','{8}'", transferdate1, ddlcartype1, requestid, ddlsupplier, ddlarr, picktype, droptype, ddlpickupPoint, ddldropoffPoint)
            Dim retlist As Array
            'sqlstr.AppendFormat("exec sp_get_transfer_vehicletype 1,1,2,'A-DXBT3','H-000051','00000739',0,0,'',0,1,'2014/06/19',2,0,1")
            retlist = objUtils.FillArraynew(constr, sqlstr.ToString, 9)
            Return retlist

        End Function
        '<WebMethod()> _
        'Public Function fillpricesnew1(ByVal constr As String, ByVal requestid As String) As clsMaster()
        '    Dim retlist As New List(Of clsMaster)


        '    'Dim otherdtemp As Date = transferdate1
        '    'transferdate1 = Format(otherdtemp, "yyyy/MM/dd")




        '    Dim sqlstr As StringBuilder = New StringBuilder()
        '    'sqlstr.AppendFormat("exec sp_get_transfer_prices, '{0}','{1}','{2}',{3},{4},'{5}',{6},{7},{8},{9},{10},{11},{12},'{13}','{14}' ", transferdate1, ddlcartype1, requestid, hotellineno, rmtyplineno, rmcatcode, roomno, arrdeplineno, txtnoveh, ddlarr, picktype, droptype, ddlpickupPoint, ddldropoffPoint)

        '    'sqlstr.AppendFormat("exec sp_get_transfer_vehicletype 1,1,2,'A-DXBT3','H-000051','00000739',0,0,'',0,1,'2014/06/19',2,0,1")
        '    retlist = objUtils.FillclsMasternew(constr, sqlstr.ToString, True)
        '    Return retlist.ToArray()

        'End Function
        <WebMethod(EnableSession:=True)> _
        Public Function GetCurrNamefromOthsellingcodetype(ByVal connstr As String, ByVal othsellcode As String) As String
            Dim retcurrcode As String = ""
            retcurrcode = CType(objUtils.GetDBFieldFromStringnew(Session("dbConnectionName"), "othsellmast", "currcode", "othsellcode", othsellcode), String)
            Return retcurrcode
        End Function

        <WebMethod(EnableSession:=True)> _
        Public Function GetCurrNamefromOthsellingNametype(ByVal connstr As String, ByVal othsellcode As String) As String
            Dim retcurrname As String = ""
            retcurrname = CType(objUtils.ExecuteQueryReturnSingleValuenew(Session("dbConnectionName"), "select c.currname from currmast c,othsellmast p where p.currcode=c.currcode " _
           & " and p.othsellcode='" & othsellcode & "'"), String)
            Return retcurrname
        End Function

        <WebMethod(EnableSession:=True)> _
        Public Function GetCurrNamefromVisasellingcodetype(ByVal connstr As String, ByVal othsellcode As String) As String
            Dim retcurrcode As String = ""
            retcurrcode = CType(objUtils.GetDBFieldFromStringnew(Session("dbConnectionName"), "visasellmast", "currcode", "visasellcode", othsellcode), String)
            Return retcurrcode
        End Function

        <WebMethod(EnableSession:=True)> _
        Public Function GetCurrNamefromVisasellingNametype(ByVal connstr As String, ByVal othsellcode As String) As String
            Dim retcurrname As String = ""
            retcurrname = CType(objUtils.ExecuteQueryReturnSingleValuenew(Session("dbConnectionName"), "select c.currname from currmast c,visasellmast p where p.currcode=c.currcode " _
           & " and p.visasellcode='" & othsellcode & "'"), String)
            Return retcurrname
        End Function






        <WebMethod()> _
        Public Function GetHandlingsellcodefrmCurrCode(ByVal constr As String, ByVal currcode As String) As clsMaster()
            Dim retlist As New List(Of clsMaster)
            Dim sqlstr As String
            Dim filter As String
            Dim default_group As String = ""
            default_group = objUtils.ExecuteQueryReturnStringValuenew(constr, "select option_selected from reservation_parameters where param_id=1108")
            sqlstr = "select othsellcode,othsellname  from  othsellmast  where active=1 and othertype='" & default_group & "'"
            If Trim(currcode) <> "" And Trim(UCase(currcode)) <> Trim(UCase("[Select]")) Then
                sqlstr = sqlstr + "and othsellmast.currcode='" & currcode & "'"
            End If
            sqlstr = sqlstr + " order by othsellmast.othsellcode"
            retlist = objUtils.FillclsMasternew(constr, sqlstr, True)
            Return retlist.ToArray()
        End Function
        <WebMethod()> _
        Public Function GettHandlingsellnamefrmCurrName(ByVal constr As String, ByVal currname As String) As clsMaster()
            Dim retlist As New List(Of clsMaster)
            Dim sqlstr As String
            Dim filter As String
            Dim default_group As String = ""
            default_group = objUtils.ExecuteQueryReturnStringValuenew(constr, "select option_selected from reservation_parameters where param_id=1108")

            sqlstr = "select  othsellname,othsellcode from othsellmast where active=1  and othertype='" & default_group & "'"

            If Trim(currname) <> "" And Trim(UCase(currname)) <> Trim(UCase("[Select]")) Then
                sqlstr = sqlstr + "and othsellmast.currcode='" & currname & "'"
            End If
            sqlstr = sqlstr + " order by othsellmast.othsellname"
            retlist = objUtils.FillclsMasternew(constr, sqlstr, True)
            Return retlist.ToArray()
        End Function


        <WebMethod()> _
        Public Function GetOthersellnamefrmCurrCode(ByVal constr As String, ByVal currcode As String) As clsMaster()
            Dim retlist As New List(Of clsMaster)
            Dim sqlstr As String
            Dim filter As String
            Dim default_group As String = ""
            default_group = objUtils.ExecuteQueryReturnStringValuenew(constr, "select option_selected from reservation_parameters where param_id=1109")
            sqlstr = "select othsellcode,othsellname  from  othsellmast  where active=1 and othertype='" & default_group & "'"
            If Trim(currcode) <> "" And Trim(UCase(currcode)) <> Trim(UCase("[Select]")) Then
                sqlstr = sqlstr + "and othsellmast.currcode='" & currcode & "'"
            End If
            sqlstr = sqlstr + " order by othsellmast.othsellcode"
            retlist = objUtils.FillclsMasternew(constr, sqlstr, True)
            Return retlist.ToArray()
        End Function
        <WebMethod()> _
        Public Function GetOthersellnamefrmCurrName(ByVal constr As String, ByVal currname As String) As clsMaster()
            Dim retlist As New List(Of clsMaster)
            Dim sqlstr As String
            Dim filter As String
            Dim default_group As String = ""
            default_group = objUtils.ExecuteQueryReturnStringValuenew(constr, "select option_selected from reservation_parameters where param_id=1109")

            sqlstr = "select  othsellname,othsellcode from othsellmast where active=1  and othertype='" & default_group & "'"

            If Trim(currname) <> "" And Trim(UCase(currname)) <> Trim(UCase("[Select]")) Then
                sqlstr = sqlstr + "and othsellmast.currcode='" & currname & "'"
            End If
            sqlstr = sqlstr + " order by othsellmast.othsellname"
            retlist = objUtils.FillclsMasternew(constr, sqlstr, True)
            Return retlist.ToArray()
        End Function


        <WebMethod()> _
        Public Function GetvisasellcodefrmCurrCode(ByVal constr As String, ByVal currcode As String) As clsMaster()
            Dim retlist As New List(Of clsMaster)
            Dim sqlstr As String
            sqlstr = "select visasellcode,visasellname  from  visasellmast  where active=1 "
            If Trim(currcode) <> "" And Trim(UCase(currcode)) <> Trim(UCase("[Select]")) Then
                sqlstr = sqlstr + "and visasellmast.currcode='" & currcode & "'"
            End If
            sqlstr = sqlstr + " order by visasellmast.visasellcode "
            retlist = objUtils.FillclsMasternew(constr, sqlstr, True)
            Return retlist.ToArray()
        End Function
        <WebMethod()> _
        Public Function GetvisasellnamefrmCurrName(ByVal constr As String, ByVal currname As String) As clsMaster()
            Dim retlist As New List(Of clsMaster)
            Dim sqlstr As String
            sqlstr = "select  visasellname,visasellcode from visasellmast   where active=1 "

            If Trim(currname) <> "" And Trim(UCase(currname)) <> Trim(UCase("[Select]")) Then
                sqlstr = sqlstr + "and visasellmast.currcode='" & currname & "'"
            End If
            sqlstr = sqlstr + " order by visasellmast.visasellname "
            retlist = objUtils.FillclsMasternew(constr, sqlstr, True)
            Return retlist.ToArray()
        End Function

        <WebMethod(EnableSession:=True)> _
        Public Function GetVisa(ByVal constr As String) As String
            Dim result As String
            Dim sqlstr As String
            sqlstr = "select option_selected from reservation_parameters where param_id='1002'"
            result = objUtils.ExecuteQueryReturnStringValuenew(Session("dbConnectionName"), sqlstr)

            Return result
        End Function



        <WebMethod()> _
        Public Function GetPartymastCatCodeListnew(ByVal constr As String, ByVal sptypecode As String) As clsMaster()
            Dim retlist As New List(Of clsMaster)
            Dim sqlstr As String
            sqlstr = "select distinct ltrim(rtrim(partymast.catcode)) as catcode, ltrim(rtrim(catname)) as catname from catmast, partymast where partymast.catcode = catmast.catcode and partymast.active=1 "
            If Trim(sptypecode) <> "" And Trim(UCase(sptypecode)) <> Trim(UCase("[Select]")) Then
                sqlstr = sqlstr + " and catmast.sptypecode='" & Trim(sptypecode) & "'"
            End If
            sqlstr = sqlstr + " order by catcode"

            retlist = objUtils.FillclsMasternew(constr, sqlstr, True)
            Return retlist.ToArray()
        End Function


        <WebMethod()> _
        Public Function GetPartymastCatNameListnew(ByVal constr As String, ByVal sptypecode As String) As clsMaster()
            Dim retlist As New List(Of clsMaster)
            Dim sqlstr As String
            sqlstr = "select distinct ltrim(rtrim(catname)) as catname , ltrim(rtrim(partymast.catcode)) as catcode from catmast, partymast where partymast.catcode = catmast.catcode and partymast.active=1"
            If Trim(sptypecode) <> "" And Trim(UCase(sptypecode)) <> Trim(UCase("[Select]")) Then
                sqlstr = sqlstr + " and catmast.sptypecode='" & Trim(sptypecode) & "'"
            End If
            sqlstr = sqlstr + " order by catname"

            retlist = objUtils.FillclsMasternew(constr, sqlstr, True)
            Return retlist.ToArray()
        End Function


        <WebMethod()> _
        Public Function GetPartyMastCityCodeListnew(ByVal constr As String, ByVal ctrycode As String) As clsMaster()
            Dim retlist As New List(Of clsMaster)
            Dim sqlstr As String
            sqlstr = "select distinct ltrim(rtrim(partymast.citycode)) as citycode , ltrim(rtrim(cityname)) as cityname from citymast, partymast where partymast.citycode = citymast.citycode and partymast.active=1"
            If Trim(ctrycode) <> "" And Trim(UCase(ctrycode)) <> Trim(UCase("[Select]")) Then
                sqlstr = sqlstr + " and citymast.ctrycode='" & Trim(ctrycode) & "'"
            End If
            sqlstr = sqlstr + " order by citycode"

            retlist = objUtils.FillclsMasternew(constr, sqlstr, True)
            Return retlist.ToArray()
        End Function


        <WebMethod()> _
        Public Function GetPartyMastCityNameListnew(ByVal constr As String, ByVal ctrycode As String) As clsMaster()
            Dim retlist As New List(Of clsMaster)
            Dim sqlstr As String
            sqlstr = "select distinct ltrim(rtrim(cityname)) as cityname,ltrim(rtrim(partymast.citycode)) as citycode from citymast, partymast where partymast.citycode = citymast.citycode and partymast.active=1"
            If Trim(ctrycode) <> "" And Trim(UCase(ctrycode)) <> Trim(UCase("[Select]")) Then
                sqlstr = sqlstr + " and citymast.ctrycode='" & Trim(ctrycode) & "'"
            End If
            sqlstr = sqlstr + " order by cityname"

            retlist = objUtils.FillclsMasternew(constr, sqlstr, True)
            Return retlist.ToArray()
        End Function

        <WebMethod()> _
        Public Function GetPartyMastSectorCodeListnew(ByVal constr As String, ByVal ctrycode As String, ByVal citycode As String) As clsMaster()
            Dim retlist As New List(Of clsMaster)
            Dim sqlstr As String
            sqlstr = "select distinct ltrim(rtrim(partymast.sectorcode)) as sectorcode, ltrim(rtrim(sectorname)) as sectorname from sectormaster, partymast where partymast.sectorcode = sectormaster.sectorcode and partymast.active=1"
            If Trim(ctrycode) <> "" And Trim(UCase(ctrycode)) <> Trim(UCase("[Select]")) Then
                sqlstr = sqlstr + " and sectormaster.ctrycode='" & Trim(ctrycode) & "'"
            End If
            If Trim(citycode) <> "" And Trim(UCase(citycode)) <> Trim(UCase("[Select]")) Then
                sqlstr = sqlstr + " and sectormaster.citycode='" & Trim(citycode) & "'"
            End If
            sqlstr = sqlstr + " order by sectorcode"

            retlist = objUtils.FillclsMasternew(constr, sqlstr, True)
            Return retlist.ToArray()
        End Function


        <WebMethod()> _
        Public Function GetPartyMastSectorNameListnew(ByVal constr As String, ByVal ctrycode As String, ByVal citycode As String) As clsMaster()
            Dim retlist As New List(Of clsMaster)
            Dim sqlstr As String
            sqlstr = "select distinct ltrim(rtrim(sectorname)) as sectorname, ltrim(rtrim(partymast.sectorcode)) as sectorcode from sectormaster, partymast where partymast.sectorcode = sectormaster.sectorcode and partymast.active=1"
            If Trim(ctrycode) <> "" And Trim(UCase(ctrycode)) <> Trim(UCase("[Select]")) Then
                sqlstr = sqlstr + " and sectormaster.ctrycode='" & Trim(ctrycode) & "'"
            End If
            If Trim(citycode) <> "" And Trim(UCase(citycode)) <> Trim(UCase("[Select]")) Then
                sqlstr = sqlstr + " and sectormaster.citycode='" & Trim(citycode) & "'"
            End If
            sqlstr = sqlstr + " order by sectorname"

            retlist = objUtils.FillclsMasternew(constr, sqlstr, True)
            Return retlist.ToArray()
        End Function

        <WebMethod(EnableSession:=True)> _
        Public Function HotelAutoCompleteOnline(ByVal sptypecode As String, ByVal catcode As String, ByVal sellcatcode As String, ByVal ctrycode As String, ByVal citycode As String, ByVal sectorcode As String) As List(Of AutocompleteClass)
            Try
                Dim HotelAutoCompleteClass As New List(Of AutocompleteClass)
                Dim HotelAutoCompleteClassAdd As New AutocompleteClass

                Dim sqlstr As String
                sqlstr = "select partyname, partycode from partymast where showinweb =1 and active=1 "
                If Trim(sptypecode) <> "" And Trim(UCase(sptypecode)) <> Trim(UCase("[Select]")) Then
                    sqlstr = sqlstr + " and sptypecode='" & Trim(sptypecode) & "'"
                End If
                If Trim(catcode) <> "" And Trim(UCase(catcode)) <> Trim(UCase("[Select]")) Then
                    sqlstr = sqlstr + " and catcode='" & Trim(catcode) & "'"
                End If

                If Trim(sellcatcode) <> "" And Trim(UCase(sellcatcode)) <> Trim(UCase("[Select]")) Then
                    sqlstr = sqlstr + " and scatcode='" & Trim(sellcatcode) & "'"
                End If
                If Trim(ctrycode) <> "" And Trim(UCase(ctrycode)) <> Trim(UCase("[Select]")) Then
                    sqlstr = sqlstr + " and ctrycode='" & Trim(ctrycode) & "'"
                End If
                If Trim(citycode) <> "" And Trim(UCase(citycode)) <> Trim(UCase("[Select]")) Then
                    sqlstr = sqlstr + " and citycode='" & Trim(citycode) & "'"
                End If

                If Trim(sectorcode) <> "" And Trim(UCase(sectorcode)) <> Trim(UCase("[Select]")) Then
                    sqlstr = sqlstr + " and sectorcode='" & Trim(sectorcode) & "'"
                End If
                sqlstr = sqlstr + " order by partyname "

                Dim MYCommand As New SqlCommand(sqlstr, clsDBConnect.dbConnectionnew(Session("dbconnectionName").ToString()))

                Dim Dr As SqlDataReader
                Dr = MYCommand.ExecuteReader
                If Dr.HasRows Then
                    While Dr.Read
                        If Not IsDBNull("partycode") Then
                            HotelAutoCompleteClassAdd = New AutocompleteClass
                            HotelAutoCompleteClassAdd.Id = Dr("partycode")
                            HotelAutoCompleteClassAdd.Name = Dr("partyname")
                            HotelAutoCompleteClass.Add(HotelAutoCompleteClassAdd)
                        End If
                    End While
                Else
                    HotelAutoCompleteClassAdd = New AutocompleteClass
                    HotelAutoCompleteClassAdd.Id = "No Records"
                    HotelAutoCompleteClassAdd.Name = "No Records"
                    HotelAutoCompleteClass.Add(HotelAutoCompleteClassAdd)


                End If

                Dr.Close()
                MYCommand.Cancel()
                Return HotelAutoCompleteClass

            Catch ex As Exception

            End Try
        End Function

        <WebMethod()> _
        Public Function GetSectorLocationByCity(ByVal constr As String, ByVal citycode As String) As clsMaster()
            Dim retlist As New List(Of clsMaster)
            Dim sqlstr As String
            sqlstr = "select sectorname,sectorcode from sectormaster"
            If Trim(citycode) <> "" And Trim(UCase(citycode)) <> Trim(UCase("[Select]")) Then
                sqlstr = sqlstr + " where active='1' and citycode='" & Trim(citycode) & "'"
            End If
            sqlstr = sqlstr + " order by sectorname"
            retlist = objUtils.FillclsMasternew(constr, sqlstr, True)
            Return retlist.ToArray()
        End Function
        <WebMethod()> _
        Public Function GetSectorLocationByCityonline(ByVal constr As String, ByVal citycode As String) As clsMaster()
            Dim retlist As New List(Of clsMaster)
            Dim sqlstr As String
            sqlstr = "select sectorname,sectorcode from sectormaster "
            If Trim(citycode) <> "" And Trim(UCase(citycode)) <> Trim(UCase("[Select]")) Then
                sqlstr = sqlstr + " where sectorcode in (select sectorcode from partymast where sptypecode ='HOT') and active='1' and citycode='" & Trim(citycode) & "'"
            End If
            sqlstr = sqlstr + " order by sectorname"
            retlist = objUtils.FillclsMasternew(constr, sqlstr, True)
            Return retlist.ToArray()
        End Function




        <WebMethod()> _
        Public Function GetPromotionsList(ByVal constr As String, ByVal codeid As String, ByVal mktcode As String) As clsMaster()
            Dim retlist As New List(Of clsMaster)
            Dim sqlstr As String
            sqlstr = "select distinct promotionid,pricecode from vw_promotion_header where  isnull(pricecode,'')<>'' "

            'If Trim(codeid) <> "" And Trim(UCase(codeid)) <> Trim(UCase("[Select]")) Then
            sqlstr = sqlstr + "   and partycode='" & codeid & "'"
            'End If

            sqlstr = sqlstr + " order by promotionid"

            retlist = objUtils.FillclsMasternew(constr, sqlstr, True)
            Return retlist.ToArray()
        End Function

        <WebMethod(EnableSession:=True)> _
        Public Function filldriverfromsup(ByVal supcode As String) As clsMaster()
            Dim retlist As New List(Of clsMaster)
            'Dim retcurrcode As String = ""
            'retcurrcode = CType(objUtils.GetDBFieldFromStringnew(Session("dbConnectionName"), "drivermaster", "drivercode", "suppcode", supcode), String)
            'Return retcurrcode

            retlist = objUtils.FillclsMasternew(Session("dbConnectionName"), "select drivername,drivercode from drivermaster where suppcode='" & supcode & "'", True)


            Return retlist.ToArray()
        End Function

        <WebMethod(EnableSession:=True)> _
        Public Function filldriverowntransport() As clsMaster()
            Dim retlist As New List(Of clsMaster)
            'Dim retcurrcode As String = ""
            'retcurrcode =  CType(objUtils.GetDBFieldFromStringnew(Session("dbConnectionName"), "drivermaster", "drivercode", "suppcode", supcode), String)
            'Return retcurrcode

            retlist = objUtils.FillclsMasternew(Session("dbConnectionName"), "select drivername,drivercode from drivermaster where isnull(trfmode,0)=0", True)


            Return retlist.ToArray()
        End Function


        <WebMethod(EnableSession:=True)> _
        Public Function GetrepPhone(ByVal repcode As String) As String
            Dim retcurrcode As String = ""
            retcurrcode = CType(objUtils.GetDBFieldFromStringnew(Session("dbConnectionName"), "Airportrep", "mobileno", "repcode", repcode), String)
            Return retcurrcode
        End Function



        <WebMethod()> _
        Public Function GetExchRate4Currencynew(ByVal constr As String, ByVal custcode As String, ByVal tocurr As String) As String
            Dim result As String
            Dim sqlstr As String
            sqlstr = "SELECT currrates.convrate FROM  currrates "

            'If Trim(custcode) <> "" And Trim(UCase(custcode)) <> Trim(UCase("[Select]")) Then
            '    sqlstr = sqlstr + " where currcode='" & Trim(custcode) & "'"
            'End If
            If Trim(tocurr) <> "" And Trim(UCase(tocurr)) <> Trim(UCase("[Select]")) Then
                sqlstr = sqlstr + " where tocurr=(select option_selected from reservation_parameters where param_id=457) and currcode='" & Trim(tocurr) & "'"
            End If
            result = objUtils.ExecuteQueryReturnStringValuenew(constr, sqlstr)
            Return result
        End Function
        <WebMethod()> _
        Public Function GetAgentNameListnew(ByVal constr As String, ByVal catcode As String, ByVal sellcode As String, ByVal ctrycode As String, ByVal citycode As String, ByVal plgrpcode As String) As clsMaster()
            Dim retlist As New List(Of clsMaster)
            Dim sqlstr As String
            sqlstr = "select agentname,agentcode  from agentmast where active=1 "
            If Trim(catcode) <> "" And Trim(UCase(catcode)) <> Trim(UCase("[Select]")) Then
                sqlstr = sqlstr + " and catcode='" + catcode + "' "
            End If
            If Trim(sellcode) <> "" And Trim(UCase(sellcode)) <> Trim(UCase("[Select]")) Then
                sqlstr = sqlstr + " and sellcode='" + sellcode + "' "
            End If
            If Trim(ctrycode) <> "" And Trim(UCase(ctrycode)) <> Trim(UCase("[Select]")) Then
                sqlstr = sqlstr + " and ctrycode='" + ctrycode + "' "
            End If
            If Trim(citycode) <> "" And Trim(UCase(citycode)) <> Trim(UCase("[Select]")) Then
                sqlstr = sqlstr + " and citycode='" + citycode + "' "
            End If
            If Trim(plgrpcode) <> "" And Trim(UCase(plgrpcode)) <> Trim(UCase("[Select]")) Then
                sqlstr = sqlstr + " and plgrpcode='" + plgrpcode + "' "
            End If

            sqlstr = sqlstr + " order by agentname"


            retlist = objUtils.FillclsMasternew(constr, sqlstr, True)
            Return retlist.ToArray()
        End Function



        <WebMethod()> _
        Public Function GetAgentCodeListnew(ByVal constr As String, ByVal catcode As String, ByVal sellcode As String, ByVal ctrycode As String, ByVal citycode As String, ByVal plgrpcode As String) As clsMaster()
            Dim retlist As New List(Of clsMaster)
            Dim sqlstr As String
            sqlstr = "select agentcode,agentname  from agentmast where active=1 "
            If Trim(catcode) <> "" And Trim(UCase(catcode)) <> Trim(UCase("[Select]")) Then
                sqlstr = sqlstr + " and catcode='" + catcode + "' "
            End If
            If Trim(sellcode) <> "" And Trim(UCase(sellcode)) <> Trim(UCase("[Select]")) Then
                sqlstr = sqlstr + " and sellcode='" + sellcode + "' "
            End If
            If Trim(ctrycode) <> "" And Trim(UCase(ctrycode)) <> Trim(UCase("[Select]")) Then
                sqlstr = sqlstr + " and ctrycode='" + ctrycode + "' "
            End If
            If Trim(citycode) <> "" And Trim(UCase(citycode)) <> Trim(UCase("[Select]")) Then
                sqlstr = sqlstr + " and citycode='" + citycode + "' "
            End If
            If Trim(plgrpcode) <> "" And Trim(UCase(plgrpcode)) <> Trim(UCase("[Select]")) Then
                sqlstr = sqlstr + " and plgrpcode='" + plgrpcode + "' "
            End If

            sqlstr = sqlstr + " order by agentcode"


            retlist = objUtils.FillclsMasternew(constr, sqlstr, True)
            Return retlist.ToArray()
        End Function


        Public Function SendCDOMessage(ByVal attach_filename As String, ByVal strFrom As String, ByVal strTo As String, ByVal strToCC As String, ByVal strSubject As String, ByVal strMsg As String) As Boolean
            Try
                Dim yourEmail As String = ""
                Dim message As New CDO.Message()
                Dim configuration As CDO.IConfiguration = message.Configuration
                Dim fields As ADODB.Fields = configuration.Fields
                Dim field As ADODB.Field = fields("http://schemas.microsoft.com/cdo/configuration/smtpserver")

                field.Value = "mail3.gridhost.co.uk"

                field = fields("http://schemas.microsoft.com/cdo/configuration/smtpserverport")
                field.Value = 465
                'field.Value = 25

                field = fields("http://schemas.microsoft.com/cdo/configuration/sendusing")
                field.Value = CDO.CdoSendUsing.cdoSendUsingPort

                field = fields("http://schemas.microsoft.com/cdo/configuration/smtpauthenticate")
                field.Value = CDO.CdoProtocolsAuthentication.cdoBasic

                field = fields("http://schemas.microsoft.com/cdo/configuration/sendusername")
                field.Value = yourEmail

                field = fields("http://schemas.microsoft.com/cdo/configuration/sendpassword")
                field.Value = "s123"

                field = fields("http://schemas.microsoft.com/cdo/configuration/smtpusessl")
                field.Value = "true"
                fields.Update()
                message.From = strFrom
                message.To = strTo
                message.CC = strToCC '

                message.Subject = strSubject
                message.HTMLBody = strMsg
                If attach_filename <> "" Then message.AddAttachment(attach_filename)

                ' Send message.
                message.Send()
                SendCDOMessage = True
            Catch ex As Exception
                SendCDOMessage = False
            End Try

        End Function

        <WebMethod()> _
        Public Function FillGridPricesAirportcostAMG(ByVal constr As String, ByVal requestid As String, ByVal Otherdt As String,
                                     ByVal othtypcode As String, ByVal othgrpcode As String,
                                     ByVal suppliercode As String) As String()
            Dim objUtils As New clsUtils
            Dim retlist As Array = Nothing
            Dim otherdtemp As Date = Otherdt
            Otherdt = Format(otherdtemp, "yyyy/MM/dd")
            Dim sqlstr As StringBuilder = New StringBuilder()
            If (othtypcode <> "[Select]") Then

                sqlstr.AppendFormat("exec sp_get_airportma_pricesForSupplier_AMG '{0}','{1}','{2}','{3}','{4}'", Otherdt, othtypcode,
                                    requestid, othgrpcode, suppliercode)
                Dim cnt As Long = 9

                retlist = objUtils.FillArraynew(constr, sqlstr.ToString, cnt)
                retlist(9) = "FlgSupplierAMG"
                'If retlist.Length > 0 Then
                '    Return retlist
                'Else
                '    Return Nothing
                'End If
            End If
            Return retlist
        End Function


        'This is added by Riswan - 27/05/2015
        <WebMethod()> _
        Public Function GetAreaLocationByCityonline(ByVal constr As String, ByVal citycode As String) As clsMaster()
            Dim retlist As New List(Of clsMaster)
            Dim sqlstr As String
            sqlstr = "select areaname,areacode from areamaster "
            If Trim(citycode) <> "" And Trim(UCase(citycode)) <> Trim(UCase("[Select]")) Then
                sqlstr = sqlstr + " where active='1' and citycode='" & Trim(citycode) & "'"
            End If
            sqlstr = sqlstr + " order by areaname"
            retlist = objUtils.FillclsMasternew(constr, sqlstr, True)
            Return retlist.ToArray()
        End Function

        'Added By Riswan 09/01/2015
        <WebMethod()> _
        Public Function GetPromoCodeForAllotment(ByVal constr As String, ByVal partycode As String, ByVal rmtyp As String) As clsMaster()
            Dim retlist As New List(Of clsMaster)
            Dim sqlstr As String
            If rmtyp = "" Then rmtyp = "[Select]"
            If partycode = "" Then partycode = "[Select]"

            If rmtyp <> "[Select]" Then
                sqlstr = "SELECT DISTINCT a.promotionid,a.pricecode promoname FROM promotion_header a INNER JOIN promotion_rmtyp b ON a.promotionid = b.promotionid  Where a.partycode = '" & partycode & "' and b.rmtypcode='" & rmtyp & "' "
            ElseIf partycode <> "[Select]" Then
                sqlstr = "SELECT DISTINCT a.promotionid,a.pricecode promoname FROM promotion_header a INNER JOIN promotion_rmtyp b ON a.promotionid = b.promotionid  Where a.partycode = '" & partycode & "' "
            End If

            sqlstr = sqlstr + " and ISNULL(a.pricecode,'') <> '' and a.active = 1 order by a.promotionid  "
            retlist = objUtils.FillclsMasternew(constr, sqlstr, True)
            Return retlist.ToArray()
        End Function

        'Added By Riswan
        <WebMethod()> _
        Public Function GetPriceCodeByPromo(ByVal constr As String, ByVal promocode As String) As String
            Dim retlist As String = ""
            Dim sqlstr As String = ""
            If promocode <> "" And promocode <> "[Select]" Then
                sqlstr = "SELECT ISNULL(pricecode,'') pricecode,ISNULL(webdesc,'') webdesc FROM promotion_header Where promotionid='" & promocode & "' and active=1"
            End If
            Dim ds As DataSet = objUtils.ExecuteQuerySqlnew(constr, sqlstr)
            If ds.Tables.Count > 0 Then
                If ds.Tables(0).Rows.Count > 0 Then
                    If ds.Tables(0).Rows(0).Item("pricecode") = "" Then
                        retlist = ds.Tables(0).Rows(0).Item("webdesc")
                    Else
                        retlist = ds.Tables(0).Rows(0).Item("pricecode")
                    End If
                End If
            End If
            'retlist = objUtils.ExecuteQueryReturnStringValuenew(constr, sqlstr)
            Return retlist
        End Function



        'Added By Riswan 16/01/2016
        <WebMethod()> _
        Public Function GetCancellationPolicypromo(ByVal constr As String, ByVal partycode As String, ByVal mktcode As String, ByVal frmdate As String, ByVal todate As String, _
                                              ByVal promostng As String) As String
            Dim retlist As String = ""
            Dim sqlstr As String
            Dim ds As DataSet
            Dim objdate As New clsDateTime
            sqlstr = "sp_get_cancelpolicy_resrmks '" + partycode + "','" + mktcode + "','" + objdate.ConvertDateromTextBoxToTextYearMonthDay(frmdate) + "','" + objdate.ConvertDateromTextBoxToTextYearMonthDay(todate) + "','" + promostng + "'"
            ds = objUtils.ExecuteQuerySqlnew(constr, sqlstr)
            If ds.Tables.Count > 0 Then
                For i = 0 To ds.Tables(0).Rows.Count - 1
                    retlist = retlist & ds.Tables(0).Rows(i)("policy") & "<br /><br />"
                    '  retlist = retlist & "<br /><br />" & ds.Tables(0).Rows(i)("remarks") & "<br />"
                Next
                retlist = retlist.Replace(Chr(13), "<br/>")
            End If
            If retlist.Trim = "" Then
                retlist = "<center> ------- No Cancellation Policy Entered ------- </center>"
            End If
            Return retlist.ToString()
        End Function

        '<WebMethod()> _
        'Public Function GetMealPlans(ByVal constr As String, ByVal codeid As String) As clsMaster()
        '    Dim retlist As New List(Of clsMaster)
        '    Dim sqlstr As String = "SELECT mealname,a.mealcode FROM mealmast a INNER JOIN partymeal b ON a.mealcode = b.mealcode Where b.partycode ='" & codeid & "' Order By a.mealcode"
        '    retlist = objUtils.FillclsMasternew(constr, sqlstr, True)
        '    Return retlist.ToArray()
        'End Function

        'Added by Riswan
        <WebMethod()> _
        Public Function GetMealPlans(ByVal constr As String, ByVal codeid As String) As clsMaster()
            Dim retlist As New List(Of clsMaster)
            Dim sqlstr As String = "SELECT mealname,a.mealcode FROM mealmast a INNER JOIN partymeal b ON a.mealcode = b.mealcode Where b.partycode ='" & codeid & "' Order By a.mealcode"
            retlist = objUtils.FillclsMasternew(constr, sqlstr, True)
            Return retlist.ToArray()
        End Function
        'Added By Riswan 
        <WebMethod()> _
        Public Function GetMealPlansForPromotion(ByVal constr As String, ByVal promoid As String) As clsMaster()
            Dim retlist As New List(Of clsMaster)
            Dim sqlstr As String = "SELECT b.mealname,a.mealcode FROM vw_promotion_meal a INNER JOIN mealmast b ON a.mealcode = b.mealcode Where promotionid='" & promoid & "'"
            retlist = objUtils.FillclsMasternew(constr, sqlstr, True)
            Return retlist.ToArray()
        End Function
        <WebMethod(EnableSession:=True)> _
        Public Function HotelAutoCompleteOnline_Area(ByVal sptypecode As String, ByVal catcode As String, ByVal sellcatcode As String, ByVal ctrycode As String, ByVal citycode As String, ByVal areacode As String) As List(Of AutocompleteClass)
            Try
                Dim HotelAutoCompleteClass As New List(Of AutocompleteClass)
                Dim HotelAutoCompleteClassAdd As New AutocompleteClass

                Dim sqlstr As String
                sqlstr = "select partyname, partycode from partymast where showinweb =1 and active=1 "
                If Trim(sptypecode) <> "" And Trim(UCase(sptypecode)) <> Trim(UCase("[Select]")) Then
                    sqlstr = sqlstr + " and sptypecode='" & Trim(sptypecode) & "'"
                End If
                If Trim(catcode) <> "" And Trim(UCase(catcode)) <> Trim(UCase("[Select]")) Then
                    sqlstr = sqlstr + " and catcode='" & Trim(catcode) & "'"
                End If

                If Trim(sellcatcode) <> "" And Trim(UCase(sellcatcode)) <> Trim(UCase("[Select]")) Then
                    sqlstr = sqlstr + " and scatcode='" & Trim(sellcatcode) & "'"
                End If
                If Trim(ctrycode) <> "" And Trim(UCase(ctrycode)) <> Trim(UCase("[Select]")) Then
                    sqlstr = sqlstr + " and ctrycode='" & Trim(ctrycode) & "'"
                End If
                If Trim(citycode) <> "" And Trim(UCase(citycode)) <> Trim(UCase("[Select]")) Then
                    sqlstr = sqlstr + " and citycode='" & Trim(citycode) & "'"
                End If

                If Trim(areacode) <> "" And Trim(UCase(areacode)) <> Trim(UCase("[Select]")) Then
                    sqlstr = sqlstr + " and areacode='" & Trim(areacode) & "'"
                End If
                sqlstr = sqlstr + " order by partyname "

                Dim MYCommand As New SqlCommand(sqlstr, clsDBConnect.dbConnectionnew(Session("dbconnectionName").ToString()))

                Dim Dr As SqlDataReader
                Dr = MYCommand.ExecuteReader
                If Dr.HasRows Then
                    While Dr.Read
                        If Not IsDBNull("partycode") Then
                            HotelAutoCompleteClassAdd = New AutocompleteClass
                            HotelAutoCompleteClassAdd.Id = Dr("partycode")
                            HotelAutoCompleteClassAdd.Name = Dr("partyname")
                            HotelAutoCompleteClass.Add(HotelAutoCompleteClassAdd)
                        End If
                    End While
                Else
                    HotelAutoCompleteClassAdd = New AutocompleteClass
                    HotelAutoCompleteClassAdd.Id = "No Records"
                    HotelAutoCompleteClassAdd.Name = "No Records"
                    HotelAutoCompleteClass.Add(HotelAutoCompleteClassAdd)


                End If

                Dr.Close()
                MYCommand.Cancel()
                Return HotelAutoCompleteClass

            Catch ex As Exception

            End Try
        End Function


        <WebMethod()> _
        Public Function GetCategoryLocationAndAreaByHotel(ByVal constr As String, ByVal partycode As String) As String()
            Dim retlist As New List(Of clsMaster)
            Dim sqlstr As String
            sqlstr = "SELECT cm.citycode,cm.cityname,ctm.catcode,ctm.catname,am.areacode,am.areaname FROM partymast pm INNER JOIN citymast cm ON pm.citycode = cm.citycode INNER JOIN catmast ctm ON ctm.catcode = pm.catcode  INNER JOIN areamaster am ON pm.areacode = am.areacode where showinweb =1 and pm.active=1 and partycode='" & partycode & "'"

            'retlist = objUtils.FillclsMasternew(constr, sqlstr, True)
            'Return retlist.ToArray()
            Dim mStr() As String = objUtils.FillArraynew(constr, sqlstr, 6)
            Return mStr
        End Function



        <WebMethod(EnableSession:=True)> _
        Public Function GetSearchGridXML(ByVal searchTerm As String, ByVal lsProcedureName As String, ByVal pageIndex As Integer, ByVal PageSize As Integer) As String
            Dim query As String = lsProcedureName ' "sp_get_supplierlist"
            Dim cmd As New SqlCommand(query)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@SearchTerm", searchTerm)
            cmd.Parameters.AddWithValue("@PageIndex", pageIndex)
            cmd.Parameters.AddWithValue("@PageSize", PageSize)
            cmd.Parameters.Add("@RecordCount", SqlDbType.Int, 4).Direction = ParameterDirection.Output

            Return GetData(cmd, pageIndex, Session("dbconnectionName"), PageSize).GetXml()

        End Function

        Private Function GetData(ByVal cmd As SqlCommand, ByVal pageIndex As Integer, ByVal connectstr As String, ByVal PageSize As Integer) As DataSet
            Dim strConnString As String = connectstr
            Dim con As New SqlConnection
            con = clsDBConnect.dbConnectionnew(strConnString)

            Dim sda As New SqlDataAdapter()
            cmd.Connection = con
            sda.SelectCommand = cmd
            Dim ds As New DataSet()
            sda.Fill(ds, "table1")
            Dim dt As New DataTable("Pager")
            dt.Columns.Add("PageIndex")
            dt.Columns.Add("PageSize")
            dt.Columns.Add("RecordCount")
            dt.Rows.Add()
            dt.Rows(0)("PageIndex") = pageIndex
            dt.Rows(0)("PageSize") = PageSize
            dt.Rows(0)("RecordCount") = cmd.Parameters("@RecordCount").Value
            ds.Tables.Add(dt)
            Return ds




        End Function

        <WebMethod(EnableSession:=True)> _
        Public Function setSessionValue(ByVal asSessionName As String, ByVal asSessionValue As String) As String
            If asSessionValue.ToString.ToUpper = "NOTHING" Then
                Session(asSessionName) = Nothing
            Else
                Session(asSessionName) = asSessionValue
            End If
            Return ""
        End Function
    End Class
End Namespace
