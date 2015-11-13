<%@ WebHandler Language="VB" Class="cronUpdateChecks" %>

Imports System
Imports System.Web
Imports System.Data
Imports System.Data.SqlClient
Public Class cronUpdateChecks : Implements IHttpHandler

    Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
        context.Response.ContentType = "text/plain"
        If config.enableCronTasks = False Then
            context.Response.Write("...")
        End If

        Try
            context.Response.Write(pingdomConnector.pingdomFindAllChecks() & vbNewLine)
            context.Response.Write(pingdomConnector.pingdomCheckResults())
        Catch ex As Exception
            context.Response.Write(ex.Message)
        End Try
    End Sub

    Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property

End Class