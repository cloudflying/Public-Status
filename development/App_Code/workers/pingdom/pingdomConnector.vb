Imports Microsoft.VisualBasic

Imports RestSharp
Imports RestSharp.Authenticators
Imports Newtonsoft.Json

Imports System.Data
Imports System.Data.SqlClient
Public Class pingdomConnector

    Public Shared Function listPingdomChecks() As pingdomCheckResponse
        Try
            Dim client As New RestClient(config.pingdomAPI)
            Dim rest As New RestRequest("api/{version}/checks")
            rest.AddParameter("version", config.pingdomAPIVersion, ParameterType.UrlSegment)
            rest.AddHeader("App-Key", config.pingdomAppKey)
            client.Authenticator = New HttpBasicAuthenticator(config.pingdomEmail, config.pingdomPassword)

            Dim response As RestResponse = client.Execute(rest)

            If response.StatusCode = Net.HttpStatusCode.OK Then
                Return JsonConvert.DeserializeObject(Of pingdomCheckResponse)(response.Content)
            End If
            Throw New ApplicationException("AN Error Occurred with Pingdom : " & response.Content & " : " & response.StatusCode & " : " & response.StatusDescription)

        Catch ex As Exception
            Throw New ApplicationException("Unable to Connect to Pingdom / List Checks : " & ex.Message)
        End Try
    End Function

    Public Shared Function listPingdomRawResults(checkId As String, Optional startTime As Int64 = 0) As pingdomRawResults
        Try
            ' // Verify Start Time is not equal to zero, if it is = grab last 60 minutes
            If startTime <= 0 Then startTime = Epoch.ToUnix(DateTime.Now.AddMinutes(60 * -1))
            Dim endTime As Integer = Epoch.ToUnix(DateTime.Now)


            Dim client As New RestClient(config.pingdomAPI)
            Dim rest As New RestRequest("api/{version}/results/{checkid}")
            rest.AddParameter("version", config.pingdomAPIVersion, ParameterType.UrlSegment)
            rest.AddParameter("checkid", checkId, ParameterType.UrlSegment)
            rest.AddQueryParameter("to", endTime)
            rest.AddQueryParameter("from", startTime)
            rest.AddHeader("App-Key", config.pingdomAppKey)
            client.Authenticator = New HttpBasicAuthenticator(config.pingdomEmail, config.pingdomPassword)

            Dim response As RestResponse = client.Execute(rest)

            If response.StatusCode = Net.HttpStatusCode.OK Then
                Return JsonConvert.DeserializeObject(Of pingdomRawResults)(response.Content)
            End If
            Throw New ApplicationException("AN Error Occurred with Pingdom : " & response.Content & " : " & response.StatusCode & " : " & response.StatusDescription)

        Catch ex As Exception
            Throw New ApplicationException("Unable to Connect to Pingdom / List RAW Checks : " & ex.Message)
        End Try
    End Function


    Public Shared Function pingdomFindAllChecks() As String
        Try

            Dim pingdomChecks As pingdomCheckResponse = pingdomConnector.listPingdomChecks
            Using conn As New SqlConnection(config.dbConnection)
                Using cmd As New SqlCommand
                    With cmd
                        .Connection = conn
                        .Connection.Open()
                        For Each chk As pingdomCheckProperties In pingdomChecks.checks
                            .CommandText = "SELECT CHECK_PUBLIC_STATUS FROM PS_CHECK_LIST WHERE CHECK_API_SOURCE=@CHECK_API_SOURCE AND CHECK_API_ID=@CHECK_API_ID;"
                            .Parameters.Clear()
                            .Parameters.AddWithValue("@CHECK_API_ID", chk.id)
                            .Parameters.AddWithValue("@CHECK_API_SOURCE", "2")
                            Dim publicStatus As String = .ExecuteScalar
                            .Parameters.Clear()
                            If Len(publicStatus) < 1 And IsNumeric(publicStatus) = False Then
                                ' NO INFORMATION - CREATE CHECK IN DB
                                .CommandText = "INSERT INTO PS_CHECK_LIST (PS_SITE_ID, CHECK_NAME, CHECK_API_ID, CHECK_API_SOURCE, CHECK_HOSTNAME, CHECK_RESOLUTION, CHECK_TYPE, CHECK_STATUS, CHECK_STATUS_TEXT, CHECK_LAST_RESPONSE_TIME, CHECK_FRIENDLY_NAME, CHECK_PUBLIC_STATUS) VALUES (@PS_SITE_ID, @CHECK_NAME, @CHECK_API_ID, @CHECK_API_SOURCE, @CHECK_HOSTNAME, @CHECK_RESOLUTION, @CHECK_TYPE, @CHECK_STATUS, @CHECK_STATUS_TEXT, @CHECK_LAST_RESPONSE_TIME, @CHECK_FRIENDLY_NAME, @CHECK_PUBLIC_STATUS);"
                                .Parameters.AddWithValue("@PS_SITE_ID", "0")
                                If chk.status = "up" Or chk.status = "paused" Then
                                    .Parameters.AddWithValue("@CHECK_PUBLIC_STATUS", 1)
                                Else
                                    .Parameters.AddWithValue("@CHECK_PUBLIC_STATUS", 0)
                                End If
                            Else
                                .CommandText = "UPDATE PS_CHECK_LIST SET CHECK_NAME=@CHECK_NAME, CHECK_API_ID=@CHECK_API_ID," &
                                " CHECK_HOSTNAME=@CHECK_HOSTNAME, CHECK_RESOLUTION=@CHECK_RESOLUTION," &
                                " CHECK_TYPE=@CHECK_TYPE, CHECK_STATUS=@CHECK_STATUS, CHECK_STATUS_TEXT=@CHECK_STATUS_TEXT, CHECK_LAST_RESPONSE_TIME=@CHECK_LAST_RESPONSE_TIME, CHECK_PUBLIC_STATUS=@CHECK_PUBLIC_STATUS" &
                                " WHERE CHECK_API_SOURCE=@CHECK_API_SOURCE AND CHECK_API_ID=@CHECK_API_ID;"

                                ' Public Status Options:
                                ' 1 = OPERATIONAL
                                ' 0 = MAJOR OUTAGE
                                ' 2 = PERFORMANCE ISSUES
                                ' 3 = PARTIAL OUTAGE
                                ' 4 = SCHEDULED MAINTENANCE
                                ' 5 = UNSCHEDULED MAINTENANCE
                                If publicStatus = "0" Or publicStatus = "1" Then
                                    If chk.status = "up" Or chk.status = "paused" Then
                                        .Parameters.AddWithValue("@CHECK_PUBLIC_STATUS", 1)
                                    Else
                                        .Parameters.AddWithValue("@CHECK_PUBLIC_STATUS", 0)
                                    End If
                                Else
                                    .Parameters.AddWithValue("@CHECK_PUBLIC_STATUS", publicStatus)
                                End If
                            End If


                            .Parameters.AddWithValue("@CHECK_NAME", chk.name)
                            .Parameters.AddWithValue("@CHECK_FRIENDLY_NAME", chk.name)
                            .Parameters.AddWithValue("@CHECK_API_ID", chk.id)
                            .Parameters.AddWithValue("@CHECK_API_SOURCE", "2")
                            .Parameters.AddWithValue("@CHECK_HOSTNAME", chk.hostname)
                            .Parameters.AddWithValue("@CHECK_RESOLUTION", chk.resolution)
                            .Parameters.AddWithValue("@CHECK_TYPE", "1")

                            If chk.status = "up" Or chk.status = "paused" Then
                                .Parameters.AddWithValue("@CHECK_STATUS", True)
                            Else
                                .Parameters.AddWithValue("@CHECK_STATUS", False)
                            End If

                            .Parameters.AddWithValue("@CHECK_STATUS_TEXT", chk.status)
                            .Parameters.AddWithValue("@CHECK_LAST_RESPONSE_TIME", chk.lastresponsetime)
                            .ExecuteNonQuery()
                        Next
                        .Connection.Close()


                    End With
                End Using
            End Using


            Return "DONE"

        Catch ex As Exception
            Throw New ApplicationException(ex.Message)
        End Try

    End Function


    Public Shared Function pingdomCheckResults() As String
        Try

            Using conn As New SqlConnection(config.dbConnection)
                Using cmd As New SqlCommand
                    With cmd
                        .Connection = conn
                        .Connection.Open()
                        .CommandText = "SELECT CHECK_API_ID, CHECK_ID FROM PS_CHECK_LIST WHERE CHECK_RUN=1;"

                        Using dT As New DataTable
                            Using adapt As New SqlDataAdapter(cmd)
                                adapt.Fill(dT)
                            End Using


                            For Each dr As DataRow In dT.Rows
                                Dim res As pingdomRawResults
                                .CommandText = "SELECT TOP(1) TIME_EPOCH From PS_CHECKS WHERE CHECK_ID=@CHECK_ID ORDER BY TIME_EPOCH DESC;"
                                .Parameters.Clear()
                                .Parameters.AddWithValue("@CHECK_ID", dr.Item("CHECK_ID"))
                                Dim lastChecked As String = .ExecuteScalar

                                If Len(lastChecked) > 0 And IsNumeric(lastChecked) Then
                                    res = pingdomConnector.listPingdomRawResults(dr.Item("CHECK_API_ID"), lastChecked + 1)
                                Else
                                    ' // Last 32 days
                                    res = pingdomConnector.listPingdomRawResults(dr.Item("CHECK_API_ID"), Epoch.ToUnix(DateTime.Now.AddMinutes(43200 * -1)))
                                End If

                                If res.results.Count > 0 Then
                                    For Each rawResult As pingdomRawResultsChecks In res.results
                                        .CommandText = "INSERT INTO PS_CHECKS (CHECK_ID, PROBE_ID, CHECK_TYPE, TIME_EPOCH, TIME_UTC, STATUS, RESPONSE_TIME, STATUSDESC, STATUSDESCLONG) VALUES (@CHECK_ID, @PROBE_ID, @CHECK_TYPE, @TIME_EPOCH, @TIME_UTC, @STATUS, @RESPONSE_TIME, @STATUSDESC, @STATUSDESCLONG);"
                                        .Parameters.Clear()
                                        .Parameters.AddWithValue("@CHECK_ID", dr.Item("CHECK_ID"))
                                        .Parameters.AddWithValue("@PROBE_ID", rawResult.probeid)
                                        .Parameters.AddWithValue("@CHECK_TYPE", "1")
                                        .Parameters.AddWithValue("@TIME_EPOCH", rawResult.time)
                                        .Parameters.AddWithValue("@TIME_UTC", Epoch.FromUnix(rawResult.time, False))
                                        .Parameters.AddWithValue("@STATUS", rawResult.status)
                                        .Parameters.AddWithValue("@RESPONSE_TIME", rawResult.responsetime)
                                        .Parameters.AddWithValue("@STATUSDESC", rawResult.statusdesc)
                                        .Parameters.AddWithValue("@STATUSDESCLONG", rawResult.statusdesclong)
                                        .ExecuteNonQuery()
                                    Next
                                End If
                            Next
                        End Using
                        .Connection.Close()
                    End With
                End Using
            End Using
            Return "DONE"
        Catch ex As Exception
            Throw New ApplicationException(ex.Message)
        End Try

    End Function
End Class
