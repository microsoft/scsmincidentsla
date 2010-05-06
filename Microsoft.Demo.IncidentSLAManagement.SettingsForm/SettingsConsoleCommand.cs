using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Microsoft.Win32;

//Requires Microsoft.EnterpriseManagement.UI.WpfWizardFramework reference
using Microsoft.EnterpriseManagement.UI.WpfWizardFramework;

//Requires Microsoft.EnterpriseManagement.UI.SdkDataAccess reference
using Microsoft.EnterpriseManagement.UI.SdkDataAccess;      // Has the ConsoleCommand class in it

//Requires Microsoft.EnterpriseManagement.UI.Foundation reference
using Microsoft.EnterpriseManagement.ConsoleFramework;      //Has the NavigationModelNodeBase and NavigationModelNodeTask in it

//Requires Microsoft.EnterpriseManagement.Core reference
using Microsoft.EnterpriseManagement;
using Microsoft.EnterpriseManagement.Common;
using Microsoft.EnterpriseManagement.Configuration;

//Requires Microsoft.EnterpriseManagement.Packaging reference
//using Microsoft.EnterpriseManagement.Packaging;

namespace Microsoft.Demo.IncidentSLAManagement.SettingsForm
{
    public class SettingsConsoleCommand :ConsoleCommand
    {
        public SettingsConsoleCommand()
        {
        }

        public override void ExecuteCommand(IList<NavigationModelNodeBase> nodes, NavigationModelNodeTask task, ICollection<string> parameters)
        {
            /*
              This GUID is generated automatically when  you import the Management Pack with the singleton admin setting class in it.
              You can get this GUID by running a query like: 
              Select BaseManagedEntityID, FullName from BaseManagedEntity where FullName like '%<enter your class ID here>%'
              where the GUID you want is returned in the BaseManagedEntityID column in the result set
            */
            String strSingletonBaseManagedObjectID = "9146FEC8-AE3B-2DE0-6A66-6BCAF4DC68BE";

            //Get the server name to connect to and connect to the server
            String strServerName = Registry.GetValue("HKEY_CURRENT_USER\\Software\\Microsoft\\System Center\\2010\\Service Manager\\Console\\User Settings", "SDKServiceMachine", "localhost").ToString();
            EnterpriseManagementGroup emg = new EnterpriseManagementGroup(strServerName);
            
            //Get the Object using the GUID from above - since this is a singleton object we can get it by GUID
            EnterpriseManagementObject emoIncidentSLASettings = emg.EntityObjects.GetObject<EnterpriseManagementObject>(new Guid(strSingletonBaseManagedObjectID), ObjectQueryOptions.Default);
            
            //Create a new "wizard" (also used for property dialogs as in this case), set the title bar, create the data, and add the pages
            WizardStory wizard = new WizardStory();
            wizard.WizardWindowTitle = "Edit Incident SLA Settings";
            WizardData data = new IncidentSLASettingsWizardData(emoIncidentSLASettings, emg);
            wizard.WizardData = data;
            wizard.AddLast(new WizardStep("Configuration", typeof(Settings), wizard.WizardData));

            //Show the property page
            PropertySheetDialog wizardWindow = new PropertySheetDialog(wizard);
            wizardWindow.ShowDialog();
        }
    }

    class IncidentSLASettingsWizardData : WizardData
    {
        #region Variables

        private Int32 intWarningThreshold;
        private Guid guidEnterpriseManagementObjectID = Guid.Empty;

        public Int32 WarningThreshold
        {
            get
            {
                return this.intWarningThreshold;
            }
            set
            {
                if (this.intWarningThreshold != value)
                {
                    this.intWarningThreshold = value;
                }
            }
        }

        public Guid EnterpriseManagementObjectID
        {
            get
            {
                return this.guidEnterpriseManagementObjectID;
            }
            set
            {
                if (this.guidEnterpriseManagementObjectID != value)
                {
                    this.guidEnterpriseManagementObjectID = value;
                }
            }
        }

        #endregion

        internal IncidentSLASettingsWizardData(EnterpriseManagementObject emoIncidentSLASettings, EnterpriseManagementGroup emg)
        {
            //Get the IncidentSLAManagement MP so you can get the Admin Setting class
            ManagementPack mpIncidentSLAManagement = emg.GetManagementPack("Microsoft.Demo.IncidentSLAManagement", null, new Version("1.0.0.0"));
            ManagementPackClass classIncidentSLAManagementSettings = mpIncidentSLAManagement.GetClass("Microsoft.Demo.IncidentSLAManagement.Settings.ClassType");
      
            Int32 intResult;
            bool bIsNumber = Int32.TryParse(emoIncidentSLASettings[classIncidentSLAManagementSettings, "IncidentSLABreachWarningThreshold"].ToString(), out intResult);
            if (bIsNumber)
            {
                this.intWarningThreshold = intResult;
            }
            else
            {
                this.intWarningThreshold = 0;
            }
            this.EnterpriseManagementObjectID = emoIncidentSLASettings.Id;
        }

        public override void AcceptChanges(WizardMode wizardMode)
        {
            //Get the server name to connect to and connect
            String strServerName = Registry.GetValue("HKEY_CURRENT_USER\\Software\\Microsoft\\System Center\\2010\\Service Manager\\Console\\User Settings", "SDKServiceMachine", "localhost").ToString();
            EnterpriseManagementGroup emg = new EnterpriseManagementGroup(strServerName);

            //Get the AdminSettings MP so you can get the Admin Setting class
            ManagementPack mpIncidentSLAManagement = emg.GetManagementPack("Microsoft.Demo.IncidentSLAManagement", null, new Version("1.0.0.0"));
            ManagementPackClass classIncidentSLAManagementSettings = mpIncidentSLAManagement.GetClass("Microsoft.Demo.IncidentSLAManagement.Settings.ClassType");

            //Get the object using the object ID
            EnterpriseManagementObject emoIncidentSLASettings = emg.EntityObjects.GetObject<EnterpriseManagementObject>(this.EnterpriseManagementObjectID, ObjectQueryOptions.Default);

            //Set the property value to the new value
            emoIncidentSLASettings[classIncidentSLAManagementSettings, "IncidentSLABreachWarningThreshold"].Value = this.WarningThreshold;
            
            //Update object
            emoIncidentSLASettings.Commit();
            
            this.WizardResult = WizardResult.Success;
        }
    }
}