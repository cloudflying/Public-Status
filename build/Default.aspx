<%@ page language="VB" autoeventwireup="false" inherits="_Default, App_Web_0zbwsm4t" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="/css/bootstrap.min.css" rel="stylesheet" />
    <link href='https://fonts.googleapis.com/css?family=Lato:400,700' rel='stylesheet' type='text/css'/>
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/font-awesome/4.4.0/css/font-awesome.min.css"/>
       <style>
        body {
    /*background-color: #f3f3f3;*/
    color: #333333;
    padding-top: 55px;
    font-family: 'Lato', sans-serif;
}
        .container {
            max-width: 960px;
        }
        .status-bar {
            background-color: #2ecc71;
            font-weight: 500;
    border-radius: 4px;
    -moz-border-radius: 4px;
    -webkit-border-radius: 4px;
    -o-border-radius: 4px;
    -ms-border-radius: 4px;
    border: 1px solid rgba(0,0,0,0.3);
    text-shadow: 0 1px 0 rgba(0,0,0,0.1);
    margin-bottom: 70px;
    padding: .75rem 1.25rem;
    color: #fff;
      
        }
        .status-bar h4 {
            margin:0;
            padding:0;
        }
        .status-line .row {
            border-color: rgba(0,0,0,0.3);
            border-width: 1px;
            border-right-style: solid ;
            border-left-style: solid;
            border-top-style: solid;
            padding: 1.1rem 1.25rem 1rem;
            background-color: #fff;
        }
        .status-line .row:first-of-type {
            border-top-style: solid;
            border-top-left-radius: 5px;
            border-top-right-radius: 5px;
        }
        .status-line .row:last-of-type {
            border-bottom-style: solid;
            border-bottom-left-radius: 5px;
            border-bottom-right-radius: 5px;
        }
        .maintenance {
            background-color: #f39c12!important;
            color: #fff;

            border: 2px solid #f39c12;
            border-bottom:none;
            border-bottom-left-radius:0;
            border-bottom-right-radius:0;
        }
         .status-bar-sub-mx {
             border: 2px solid #f39c12;
             border-bottom-left-radius:5px;
            border-bottom-right-radius:5px;
            margin-bottom: 70px;
            padding: 1.1rem 1.25rem 1rem;
            
         }
         .maintenance + .status-bar-sub-mx {
             margin-top: -70px;
         }
        .scheduled-maintenance { color: #f39c12!important;
        }
        .major-outage-red { color: #e74c3c; font-weight: bold;}
        .performance-issues-blue { color: #2980b9; font-weight:bold;}
        .partial-outage-yellow { color: #f1c40f; font-weight: bold;}
        .scheduled-maintenance-orange {color: #f39c12!important; font-weight:bold;}
        .performance-issues-blue-bg { background-color: #2980b9; color: #fff;}
        .major-outage-red-bg { background-color: #e74c3c; color: #fff;}
        .partial-outage-yellow-bg { background-color: #f1c40f; color: #fff;}
        .scheduled-maintenance-orange-bg {background-color: #f39c12!important; color: #fff;}
        .incident-list {
            margin-top: 5rem;
        }
        .incident-list h4 {
            padding-bottom: 10px;
            border-bottom: 1px solid rgba(0,0,0,0.3);
        }
        .incident-list p {
            color: #aaa;
            line-height: 1.8em;
        }
        .incident-list p.update {
            color: #333;
        }
        .incident-list .row {
            margin-top: 2rem;
        }
        .incident-list .row, .incident-list .col-xs-12, footer .col-xs-12 {
            padding-left: 0px;
            padding-right: 0px;
        }
        .scheduled-mx-link {
            color: #f39c12;
            font-weight: 700;
            font-size: 1.5em;
           line-height: 1.7em;
        }
        @media (max-width:650px) {
            .text-right {
                text-align:left!important;
            }
        }
    </style>
</head>
<body>
    <div class="container">
        <%-- // Overall Status Bar --%>
        <asp:PlaceHolder ID="plOverallStatus" runat="server"></asp:PlaceHolder>


         <%-- // Overall Status Bar --%>
        <%--<div class="row ">
            <div class="col-xs-12 status-bar maintenance">
                <div class="row">
                    <div class="col-md-6">
                        <h4>Scheduled Maintenance</h4>
                    </div>
                    <div class="col-md-6 text-right">
                        Refreshed less than one minute ago
                    </div>
                </div>
                           </div>
             <div class="col-xs-12 status-bar-sub-mx">
                 <div class="row">
                     <div class="col-xs-12">
                          <p class="update"><strong>Update</strong> - Windows Updates Completed. Installing required Windows hotfixes prior to Exchange upgrade.<br /><small>Nov 8, 10:40 AM</small></p>
                         <p class="update"><strong>Update</strong> - Updating Windows Server - prepping for Exchage upgrade.<br /><small>Nov 8, 7:45 AM</small></p>
                       <p class="update"><strong>Began</strong> - Scheduled Maintenance Started.<br /><small>Nov 8, 7:45 AM</small></p>
                     </div>
                 </div>
                </div>
        </div>--%>

        <%-- // Individual Status Labels --%>
        <div class="row">
            <div class="col-xs-12 status-line">
                <asp:PlaceHolder ID="plStatusLines" runat="server"></asp:PlaceHolder>
                <%--<div class="row">
                    <div class="col-md-6">
                         <strong>Outlook Web Access / Active Sync</strong>
                    </div>
                    <div class="col-md-6 text-right scheduled-maintenance">
                       Scheduled Maintenance
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-6">
                        <strong>STMP - Mail Delivery</strong>
                    </div>
                    <div class="col-md-6 text-right">
                        Operational
                    </div>
                </div>
                 <div class="row">
                    <div class="col-md-6">
                        <strong>KLGPC.com - Website</strong>
                    </div>
                    <div class="col-md-6 text-right">
                        Operational
                    </div>
                </div>--%>

            </div>
        </div>

        <%-- Incident Listings --%>
            <div class="incident-list">
                <div class="row"><div class="col-xs-12"><h2>Past Incidents</h2></div></div>
                <div class="row">
                    <div class="col-xs-12">
                        <h4>Nov 8, 2015</h4>
                        <a href="#" class="scheduled-mx-link">[Scheduled] Exchange CU10 Update</a>
                        <p class="update"><strong>Update</strong> - Windows Updates Completed. Installing required Windows hotfixes prior to Exchange upgrade.<br /><small>Nov 8, 10:40 AM</small></p>
                         <p class="update"><strong>Update</strong> - Updating Windows Server - prepping for Exchage upgrade.<br /><small>Nov 8, 7:45 AM</small></p>
                       <p class="update"><strong>Began</strong> - Scheduled Maintenance Started.<br /><small>Nov 8, 7:45 AM</small></p>
                   </div>
                </div>
                 <div class="row">
                    <div class="col-xs-12">
                        <h4>Nov 7, 2015</h4>
                        <p>No Incidents reported today.</p>
                   </div>
                </div>
                 <div class="row">
                    <div class="col-xs-12">
                        <h4>Nov 6, 2015</h4>
                        <p>No Incidents reported today.</p>
                   </div>
                </div>
                 <div class="row">
                    <div class="col-xs-12">
                        <h4>Nov 5, 2015</h4>
                        <p>No Incidents reported today.</p>
                   </div>
                </div>
            </div>



    </div>
    <footer class="container-fluid" style="border-top: 1px solid rgba(0,0,0,0.1);padding-top: 25px; padding-bottom:25px;">
        <div class="container">
            <div class="row">
            <div class="col-xs-12">
                <asp:Label ID="lblTitle" runat="server"></asp:Label> Status Page is powered by <a href="#">Public Status</a>.

            </div>
                </div>
        </div>
    </footer>

      <script src="https://ajax.googleapis.com/ajax/libs/jquery/2.1.4/jquery.min.js"></script>
    <script src="/js/moment.js"></script>
    <script src="js/livestamp.min.js"></script>
    <!-- Include all compiled plugins (below), or include individual files as needed -->
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.5/js/bootstrap.min.js" integrity="sha512-K1qjQ+NcF2TYO/eI3M6v8EiNYZfA95pQumfvcVrTHtwQVDG+aHRqLi/ETn2uB+1JqwYqVG3LIvdm9lj6imS/pQ==" crossorigin="anonymous"></script>
    <script type="text/javascript">
        $(function () {
            $('[data-toggle="tooltip"]').tooltip()
        });
    </script>
</body>
</html>
