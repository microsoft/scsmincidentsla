using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.EnterpriseManagement.UI.WpfWizardFramework;

namespace Microsoft.Demo.IncidentSLAManagement.SettingsForm
{
    public partial class Settings : WizardRegularPageBase
    {
        private IncidentSLASettingsWizardData incidentSLASettingsWizardData = null;

        public Settings(WizardData wizardData)
        {
            InitializeComponent();

            this.DataContext = wizardData;
            this.incidentSLASettingsWizardData = this.DataContext as IncidentSLASettingsWizardData;
        }
    }
}
