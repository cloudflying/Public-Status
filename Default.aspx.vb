Imports System.Data
Imports System.Data.SqlClient
Partial Class _Default
    Inherits System.Web.UI.Page

    Dim siteID As Integer
    Dim scheduledMaintenance As Boolean = False
    Dim partialOutage As Boolean = False
    Dim majorOutage As Boolean = False
    Dim performanceIssues As Boolean = False
    Dim unScheduledMaintenance As Boolean = False
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try

            If regenerateConfig() = False Then
                Response.Redirect("/setup", False)
            End If




            If config.dbChecked = False Then
                If checkDatabase.checkDB() = False Then
                    If createDatabase.buildDatabase() Then
                        config.dbChecked = True
                    End If
                Else
                    config.dbChecked = True
                End If
            End If


            Try
                Using conn As New SqlConnection(config.dbConnection)
                    Using cmd As New SqlCommand()
                        With cmd
                            .Connection = conn
                            .Connection.Open()
                            Dim siteData As New DataTable

                            .CommandText = "SELECT TOP(1) * FROM PS_SITES WHERE SITE_DOMAIN=@SITE_DOMAIN ORDER BY PS_SITE_ID DESC;"
                            .Parameters.Clear()
                            .Parameters.AddWithValue("@SITE_DOMAIN", Request.Url.Host)

                            Using adapt As New SqlDataAdapter(cmd)
                                adapt.Fill(siteData)
                            End Using

                            If siteData.Rows.Count = 0 Then
                                .CommandText = "SELECT count(*) FROM PS_SITES;"
                                .Parameters.Clear()
                                Dim siteCount As String = .ExecuteScalar
                                If Len(siteCount) > 0 And IsNumeric(siteCount) Then
                                    If siteCount > 0 Then
                                        Response.Write("CONFIGURATION ERROR.")
                                    Else
                                        Response.Redirect("/setup", False)
                                    End If

                                Else
                                    Response.Redirect("/setup", False)
                                End If

                                Exit Sub
                            End If

                            Dim siteRow As DataRow = siteData.Rows(0)




                            .CommandText = "SELECT * FROM PS_CHECK_LIST WHERE CHECK_RUN=1 AND PS_SITE_ID=@PS_SITE_ID;"
                            .Parameters.Clear()
                            .Parameters.AddWithValue("@PS_SITE_ID", siteRow.Item("PS_SITE_ID"))


                            Using rDr As SqlDataReader = cmd.ExecuteReader
                                Dim statusBuilder As New StringBuilder
                                If rDr.HasRows Then
                                    Do While rDr.Read
                                        statusBuilder.AppendLine(generateStatusLine(rDr("CHECK_PUBLIC_STATUS"), rDr("CHECK_FRIENDLY_NAME").ToString, rDr("CHECK_FRIENDLY_DESC").ToString))
                                    Loop
                                End If
                                plStatusLines.Controls.Add(New Literal With {.Text = statusBuilder.ToString})

                            End Using



                            Dim overallStatusBar As String = "<div class=""row ""><div class=""col-xs-12 status-bar {{STATUSBARCSS}}""><div class=""row""><div class=""col-md-6""><h4>{{STATUSLABEL}}</h4></div><div class=""col-md-6 text-right"">Refreshed <span data-livestamp=""" & Epoch.ToUnix(DateTime.Now.ToUniversalTime) & """></span></div></div></div></div>"

                            ' {{STATUSBARCSS}} - maintenance
                            ' {{STATUSLABEL}} - Operational

                            Dim statusLabel As String
                            Dim statusCSS As String
                            If majorOutage = True Then
                                statusLabel = "Some systems are experiencing issues"
                                statusCSS = "major-outage-red-bg"
                            ElseIf scheduledMaintenance = True
                                statusLabel = "SCHEDULED MAINTENANCE"
                                statusCSS = "scheduled-maintenance-orange-bg"
                            ElseIf unScheduledMaintenance = True
                                statusLabel = "UN SCHEDULED MAINTENANCE"
                                statusCSS = "scheduled-maintenance-orange-bg"
                            ElseIf performanceIssues Then
                                statusLabel = "Some systems are experiencing issues"
                                statusCSS = "performance-issues-blue-bg"
                            ElseIf partialOutage = True Then
                                statusLabel = "Some systems are experiencing issues"
                                statusCSS = "partial-outage-yellow-bg"
                            Else
                                statusLabel = "All Systems Operational"
                                statusCSS = ""
                            End If

                            overallStatusBar = Replace(overallStatusBar, "{{STATUSBARCSS}}", statusCSS)
                            overallStatusBar = Replace(overallStatusBar, "{{STATUSLABEL}}", statusLabel)
                            plOverallStatus.Controls.Add(New Literal With {.Text = overallStatusBar})



                            .Connection.Close()
                        End With
                    End Using
                End Using
            Catch ex As Exception
                Response.Write(ex.Message)
            End Try





        Catch ex As Exception
            Response.Write(ex.Message)
        End Try

    End Sub

    Function generateStatusLine(statusInt As Integer, componentName As String, Optional componentHelp As String = "") As String
        ' Public Status Options:
        ' 1 = OPERATIONAL
        ' 0 = MAJOR OUTAGE
        ' 2 = PERFORMANCE ISSUES
        ' 3 = PARTIAL OUTAGE
        ' 4 = SCHEDULED MAINTENANCE
        ' 5 = UNSCHEDULED MAINTENANCE
        Dim checkCSS As String = String.Empty
        Dim checkStatus As String = String.Empty
        Select Case statusInt
            Case 0
                majorOutage = True
                checkCSS = " major-outage-red"
                checkStatus = "Major Outage"
            Case 1
                checkStatus = "Operational"
            Case 2
                performanceIssues = True
                checkCSS = " performance-issues-blue"
                checkStatus = "Performance Issues"
            Case 3
                partialOutage = True
                checkCSS = " partial-outage-yellow"
                checkStatus = "Partial Outage"
            Case 4
                scheduledMaintenance = True
                checkCSS = " scheduled-maintenance-orange"
                checkStatus = "Scheduled Maintenance"
            Case 5
                unScheduledMaintenance = True
                checkCSS = " scheduled-maintenance-orange"
                checkStatus = "Unscheduled Maintenance"
            Case Else
                checkStatus = "Gathering Operational Information"
        End Select

        Dim StatusTemplate As String = "<div Class=""row""><div Class=""col-md-6""><strong>{{CHECKNAME}}{{HELPBOX}}</strong></div><div Class=""col-md-6 text-right{{CHECKCSS}}"">{{CHECKSTATUS}}</div></div>"
        ' {{CHECKCSS}}
        ' {{CHECKNAME}}
        ' {{CHECKSTATUS}}

        StatusTemplate = Replace(StatusTemplate, "{{CHECKCSS}}", checkCSS)
        StatusTemplate = Replace(StatusTemplate, "{{CHECKNAME}}", componentName)
        StatusTemplate = Replace(StatusTemplate, "{{CHECKSTATUS}}", checkStatus)
        If Len(componentHelp) > 1 Then
            StatusTemplate = Replace(StatusTemplate, "{{HELPBOX}}", " <a href=""javascript:void(0);"" data-toggle=""tooltip"" data-placement=""top"" title=""" & componentHelp & """><i class=""fa fa-question-circle""></i></a>")
        Else
            StatusTemplate = Replace(StatusTemplate, "{{HELPBOX}}", "")

        End If

        Return StatusTemplate
    End Function


End Class
