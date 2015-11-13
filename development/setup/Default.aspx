<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Default.aspx.vb" Inherits="setup_Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
   <meta charset="utf-8"/>
    <meta http-equiv="X-UA-Compatible" content="IE=edge"/>
    <meta name="viewport" content="width=device-width, initial-scale=1"/>
    <title>Status Page Setup</title>
    <link href="/css/bootstrap.min.css" rel="stylesheet" />
    <link rel="stylesheet" href="//maxcdn.bootstrapcdn.com/font-awesome/4.4.0/css/font-awesome.min.css"/>
   <style>
       body { background-color: #ececec; font-size:14px;}
       .container { margin-top: 35px; margin-bottom: 35px;}
       .row-pad {padding-top: 20px; padding-bottom: 30px;}
       .row {
           padding-top: 15px;
           padding-bottom: 15px;
       }
   </style>
   
</head>
<body>
    <form id="form1" runat="server">
    <div class="container">
        <div class="row">
            <div class="col-md-8 col-md-offset-2" style="background-color: #fff; border: 1px solid #e8e8e8;">
                <div class="row row-pad">
                    <div class="col-md-12 text-center">
                       <h1>Setup</h1>
                    </div>
                </div>

                <asp:PlaceHolder ID="plWarning" runat="server"></asp:PlaceHolder>


                <div class="row row-pad">
                    <div class="col-md-12">
                        Please enter the following information to configure your status application. If you need assistance, refer to our github page.
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-3">
                        <label>Database Name</label>
                    </div>
                    <div class="col-md-9">
                        <asp:TextBox ID="txtDatabaseName" runat="server" CssClass="form-control"  aria-describedby="dbHelp"></asp:TextBox><span id="dbHelp" class="help-block"></span>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-3">
                        <label>Username</label>
                    </div>
                    <div class="col-md-9">
                        <asp:TextBox ID="txtDatabaseUsername" runat="server" CssClass="form-control"  aria-describedby="dbUserHelp"></asp:TextBox><span id="dbUserHelp" class="help-block">Your SQL Server username. <br />**NOTE** We do not support Windows Login.</span>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-3">
                        <label>Password</label>
                    </div>
                    <div class="col-md-9">
                        <asp:TextBox ID="txtDatabasePassword" runat="server" CssClass="form-control"  aria-describedby="dbPasswordHelp" TextMode="Password"></asp:TextBox><span id="dbPasswordHelp" class="help-block">SQL Server password.</span>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-3">
                        <label>Database Host</label>
                    </div>
                    <div class="col-md-9">
                        <asp:TextBox ID="txtDatabaseHost" runat="server" CssClass="form-control"  aria-describedby="dbHostHelp"></asp:TextBox><span id="dbHostHelp" class="help-block">SQL Server hostname and port if needed.</span>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-3">
                        <label>Pingdom Email Address</label>
                    </div>
                    <div class="col-md-9">
                        <asp:TextBox ID="txtpingdomEmail" runat="server" CssClass="form-control"  aria-describedby="pingdomHelp"></asp:TextBox><span id="pingdomHelp" class="help-block">OPTIONAL</span>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-3">
                        <label>Pingdom Password</label>
                    </div>
                    <div class="col-md-9">
                        <asp:TextBox ID="txtPingdomPassword" runat="server" CssClass="form-control"  aria-describedby="pingdomPassHelp"></asp:TextBox><span id="pingdomPassHelp" class="help-block">OPTIONAL</span>
                    </div>
                </div>
                 <div class="row">
                    <div class="col-md-3">
                        <label>Pingdom App Key</label>
                    </div>
                    <div class="col-md-9">
                        <asp:TextBox ID="txtPingdomAppKey" runat="server" CssClass="form-control"  aria-describedby="pingdomAppKeyHelp"></asp:TextBox><span id="pingdomAppKeyHelp" class="help-block">OPTIONAL</span>
                    </div>
                </div>
                <div class="row row-pad">
                    <div class="col-md-4 col-md-offset-8">
                        <asp:Button ID="btnSave" runat="server" Text="Save"  CssClass="btn btn-info btn-block btn-lg"/>
                    </div>
                </div>
            </div>
        </div>
    </div>
    </form>
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/2.1.3/jquery.min.js"></script>
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.2/js/bootstrap.min.js"></script>
</body>
</html>
