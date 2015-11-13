<%@ WebHandler Language="VB" Class="startPingdomMonitoring" %>

Imports System
Imports System.Web
Imports Hangfire
Public Class startPingdomMonitoring : Implements IHttpHandler

    Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
        context.Response.ContentType = "text/plain"


        RecurringJob.AddOrUpdate(Sub() pingdomConnector.pingdomFindAllChecks(), Cron.Minutely)
        RecurringJob.AddOrUpdate(Sub() pingdomConnector.pingdomCheckResults(), Cron.Minutely)
        context.Response.Write("DONE")
    End Sub

    Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property

End Class