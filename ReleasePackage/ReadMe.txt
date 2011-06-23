New Deployment Instructions (Upgrade Instructions are below)
=================================
1) Ensure you have the correct files in C:\Program Files\Microsoft System Center\Service Manager 2010 on all of the management servers in your manage

ment group:
Microsoft.ServiceManager.WorkflowAuthoring.dll (Version: 7.0.6555.0)
Microsoft.ServiceManager.WorkflowAuthoring.Common.dll (Version: 7.0.6555.0)
Microsoft.ServiceManager.WorkflowAuthoring.ActivityLibrary.dll (Version: 7.0.6555.0)

If you don't have these files, you can get them from installing the SCSM Authoring Tool and copying them from the C:\Program Files (x86)\Microsoft System Center\Service Manager 2010 Authoring\PackagesToLoad directory.

You can download the SCSM Authoring Tool from here:
http://www.microsoft.com/download/en/details.aspx?id=10639

2) Copy the following files  to the C:\Program Files\Microsoft System Center Service Manager directory:
Microsoft.Demo.IncidentSLAManagement.Activities.dll
ProcessIncidents.dll

3) Copy the Microsoft.Demo.IncidentSLAManagement.SettingsForm.dll to the same directory on any computer running the Service Manager console where people will be configuring SLA management settings (i.e. admins only)
4) Import the Microsoft.Demo.IncidentSLAManagement.Library management pack.
5) Import the Microsoft.Demo.IncidentSLAManagement.Configuration.xml Management Pack.
6) Configure the Warning threshold in Administration/Settings/Incident SLA Management Settings.  By default the threshold is zero minutes (no warning threshold).

**Note: the workflow will immediately start running when you import the MP.  If you want to disable the workflow so that the workflow doesnt run until you enable it change the Rule Enabled attribute in the .xml file to "false".  You can then enable it in the Administration/Workflows/Configuration view after import.

**Note: the workflow will run every 15 minutes by default.  If you want it to run more or less frequently than that change the schedule in the XML file in the source and then seal the MP again.  Search for "Minutes" in the XML file and you will see where to change it.  Do this **BEFORE** you import the MP.

Upgrade Instructions
================================
To upgrade from version 0.2 of the Incident SLA solution just do steps 1 and 2 above.


Feedback
=================================
Please file any issues at:
http://scsmincidentsla.codeplex.com/workitem/list/basic

History
=================================
0.3 Bug Fixes
--------------
Fixed a bug where the date time values were not being converted to the correct format on a non-EN-US operating system.  The error in the event log would be manifest as 'String was not recognized as a valid DateTime'.

Built the solution binaries by compiling against SCSM 2010 SP1 and Authoring Tool SP1 binaries.
