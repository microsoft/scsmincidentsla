using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Collections;
using System.Drawing;
using System.Linq;
using System.Workflow.ComponentModel.Compiler;
using System.Workflow.ComponentModel.Serialization;
using System.Workflow.ComponentModel;
using System.Workflow.ComponentModel.Design;
using System.Workflow.Runtime;
using System.Workflow.Activities;
using System.Workflow.Activities.Rules;
using Microsoft.EnterpriseManagement;
using Microsoft.EnterpriseManagement.Configuration;
using Microsoft.EnterpriseManagement.Common;
using System.Collections.Generic;
using Microsoft.EnterpriseManagement.Workflow.Common;
using System.Threading;


namespace Microsoft.Demo.IncidentSLAManagement
{
    [DisplayName("Get SLA Breaching Incidents")]
    [ToolboxItem(typeof(ActivityToolboxItem))]
    [Designer(typeof(WorkflowActivityBaseDesigner))]
    public partial class GetSLABreachingIncidents : WorkflowActivityBase
	{
        public static DependencyProperty ServerNameProperty = DependencyProperty.Register("ServerName", typeof(string), typeof(GetSLABreachingIncidents));

        [DescriptionAttribute("Specify FQDN of server.  If not specified, localhost will be used.  Normally, don't specify.")]
        [CategoryAttribute("Test configuration")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public string ServerName
        {
            get
            {
                return ((string)(base.GetValue(GetSLABreachingIncidents.ServerNameProperty)));
            }
            set
            {
                base.SetValue(GetSLABreachingIncidents.ServerNameProperty, value);
            }
        }

        public static DependencyProperty UserNameProperty = DependencyProperty.Register("UserName", typeof(string), typeof(GetSLABreachingIncidents));

        [DescriptionAttribute("If not specified, the defined workflow account will be used.")]
        [CategoryAttribute("Test configuration")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public string UserName
        {
            get
            {
                return ((string)(base.GetValue(GetSLABreachingIncidents.UserNameProperty)));
            }
            set
            {
                base.SetValue(GetSLABreachingIncidents.UserNameProperty, value);
            }
        }

        public static DependencyProperty PasswordProperty = DependencyProperty.Register("Password", typeof(string), typeof(GetSLABreachingIncidents));

        [DescriptionAttribute("If not specified, the defined workflow account will be used.")]
        [CategoryAttribute("Test configuration")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public string Password
        {
            get
            {
                return ((string)(base.GetValue(GetSLABreachingIncidents.PasswordProperty)));
            }
            set
            {
                base.SetValue(GetSLABreachingIncidents.PasswordProperty, value);
            }
        }

        public static DependencyProperty BreachedIncidentsProperty = DependencyProperty.Register("BreachedIncidents", typeof(Object[]), typeof(GetSLABreachingIncidents));

        [DescriptionAttribute("Breached Incidents output by workflow.")]
        [CategoryAttribute("Output")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public EnterpriseManagementObject[] BreachedIncidents
        {
            get
            {
                return ((EnterpriseManagementObject[])(base.GetValue(GetSLABreachingIncidents.BreachedIncidentsProperty)));
            }
            private set
            {
                base.SetValue(GetSLABreachingIncidents.BreachedIncidentsProperty, value);
            }
        }

        public static DependencyProperty RevertToBlankIncidentsProperty = DependencyProperty.Register("RevertToBlankIncidents", typeof(Object[]), typeof(GetSLABreachingIncidents));

        [DescriptionAttribute("Revert to Blank Incidents output by workflow.")]
        [CategoryAttribute("Output")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public EnterpriseManagementObject[] RevertToBlankIncidents
        {
            get
            {
                return ((EnterpriseManagementObject[])(base.GetValue(GetSLABreachingIncidents.RevertToBlankIncidentsProperty)));
            }
            private set
            {
                base.SetValue(GetSLABreachingIncidents.RevertToBlankIncidentsProperty, value);
            }
        }


        public static DependencyProperty WarningIncidentsProperty = DependencyProperty.Register("WarningIncidents", typeof(Object[]), typeof(GetSLABreachingIncidents));

        [DescriptionAttribute("Warning Incidents output by workflow.")]
        [CategoryAttribute("Output")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public EnterpriseManagementObject[] WarningIncidents
        {
            get
            {
                return ((EnterpriseManagementObject[])(base.GetValue(GetSLABreachingIncidents.WarningIncidentsProperty)));
            }
            private set
            {
                base.SetValue(GetSLABreachingIncidents.WarningIncidentsProperty, value);
            }
        }
        
        public static DependencyProperty WarningThresholdProperty = DependencyProperty.Register("WarningThreshold", typeof(TimeSpan), typeof(GetSLABreachingIncidents));

        [DescriptionAttribute("Number of minutes prior to breach when incidents should be marked as Warning.  If not speicified (00:00:00), value from database will be used.")]
        [CategoryAttribute("Search Configuration")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public TimeSpan WarningThreshold
        {
            get
            {
                return ((TimeSpan)(base.GetValue(GetSLABreachingIncidents.WarningThresholdProperty)));
            }
            set
            {
                base.SetValue(GetSLABreachingIncidents.WarningThresholdProperty, value);
            }
        }

        private EnterpriseManagementGroup emg;

        protected override ActivityExecutionStatus Execute(ActivityExecutionContext executionContext)
        {
            
            //TEMPORARY - Used for Debugging only
            Thread.Sleep(30000);

            EnterpriseManagementConnectionSettings connSettings;

            if (!String.IsNullOrEmpty(this.ServerName))
            {
                connSettings = new EnterpriseManagementConnectionSettings(this.ServerName);
            }
            else
            {
                connSettings = new EnterpriseManagementConnectionSettings("localhost");
            }

            if (!String.IsNullOrEmpty(this.UserName) && !String.IsNullOrEmpty(this.Password))
            {
                connSettings.UserName = this.UserName;
                connSettings.Password = new System.Security.SecureString();
                foreach (var item in this.Password.ToCharArray())
                {
                    connSettings.Password.AppendChar(item);
                }
            }            
            
            this.emg = new EnterpriseManagementGroup(connSettings);

            // References to management packs that contain classes used in this example.
            ManagementPack mpSystem = emg.ManagementPacks.GetManagementPack(SystemManagementPack.System);
            ManagementPack mpIncident = emg.ManagementPacks.GetManagementPack("ServiceManager.IncidentManagement.Library", mpSystem.KeyToken, mpSystem.Version);

            ManagementPackTypeProjection incidentTypeProjection = emg.EntityTypes.GetTypeProjection("System.WorkItem.Incident.ProjectionType", mpIncident);

            const String INCIDENT_SLA_SETTINGS_OBJECT_GUID =                    "9146FEC8-AE3B-2DE0-6A66-6BCAF4DC68BE";
            const String INCIDENT_SLA_BREACH_WARNING_THRESHOLD_PROPERTY_GUID =  "31FA82A5-6528-ADFC-049B-FB9CD8281CC8";
            const String INCIDENT_SLA_STATUS_WARNING_ENUM_GUID =                "17C744AA-B2B3-9F37-99B9-A34226051EBD";
            const String INCIDENT_SLA_STATUS_BREACHED_ENUM_GUID =               "2119D9C6-256F-2542-DC43-E818B9B30E53";
                        
            //Get the Object using the GUID from above - since this is a singleton object we can get it by GUID
            EnterpriseManagementObject emoIncidentSLASettings = emg.EntityObjects.GetObject<EnterpriseManagementObject>(new Guid(INCIDENT_SLA_SETTINGS_OBJECT_GUID), ObjectQueryOptions.Default);
            
            TimeSpan tsWarningThreshold;
            Int32 intWarningThreshold;
            if (this.WarningThreshold == TimeSpan.Zero) //Not specified
            {
                if (!Int32.TryParse(emoIncidentSLASettings[new Guid(INCIDENT_SLA_BREACH_WARNING_THRESHOLD_PROPERTY_GUID)].ToString(), out intWarningThreshold))
                {
                    intWarningThreshold = 0;
                }
                tsWarningThreshold = new TimeSpan(0, intWarningThreshold, 0);
            }
            else
            {
                tsWarningThreshold = this.WarningThreshold;
            }

            string incidentBreachedCriteria = string.Empty;
            string incidentWarningCriteria = string.Empty;
            string incidentRevertToBlankCriteria = string.Empty;

            // Define the query criteria string for each of the queries
            // This XML validates against the Microsoft.EnterpriseManagement.Core.Criteria schema.                  
                
#region IncidentBreachedCriteria
                /*
                 This criteria allows for the following scenario:
                    *incident is created with Target Resolution Time (TRT)
                    *time passes until TRT < Now() 
                    *incident is marked as SLA Status = Breached
                 
                    * ignore incidents which are already marked Breached
                    * ignore incidents which are already resolved (i.e. Resolved Date is not null)  
                    
                 */
		        incidentBreachedCriteria = String.Format(@"
                <Criteria xmlns=""http://Microsoft.EnterpriseManagement.Core.Criteria/"">
                  <Reference Id=""System.WorkItem.Incident.Library"" PublicKeyToken=""{0}"" Version=""{1}"" Alias=""IncidentManagement"" />
                  <Expression>
                    <And>
                      <Expression>
                        <SimpleExpression>
                          <ValueExpressionLeft>
                            <Property>$Target/Property[Type='IncidentManagement!System.WorkItem.Incident']/TargetResolutionTime$</Property>
                          </ValueExpressionLeft>
                          <Operator>Less</Operator>
                          <ValueExpressionRight>
                            <Value>" + DateTime.Now.ToUniversalTime() + @"</Value>
                          </ValueExpressionRight>
                        </SimpleExpression>
                      </Expression>
                      <Expression>
                        <Or>
                          <Expression>
                            <SimpleExpression>
                              <ValueExpressionLeft>
                                <Property>$Target/Property[Type='IncidentManagement!System.WorkItem.Incident']/SLAStatus$</Property>
                              </ValueExpressionLeft>
                              <Operator>NotEqual</Operator>
                              <ValueExpressionRight>
                                <Value>" + "{{" + INCIDENT_SLA_STATUS_BREACHED_ENUM_GUID + "}}" + @"</Value>
                              </ValueExpressionRight>
                            </SimpleExpression>
                          </Expression>
                          <Expression>
                            <UnaryExpression>
                                <ValueExpression>
                                  <Property>$Target/Property[Type='IncidentManagement!System.WorkItem.Incident']/SLAStatus$</Property>
                                </ValueExpression>
                                <Operator>IsNull</Operator>
                            </UnaryExpression>
                          </Expression>    
                        </Or>
                      </Expression>    
                      <Expression>
                        <UnaryExpression>
                          <ValueExpression>
                            <Property>$Target/Property[Type='IncidentManagement!System.WorkItem.Incident']/ResolvedDate$</Property>
                          </ValueExpression>
                          <Operator>IsNull</Operator>
                        </UnaryExpression>
                      </Expression>
                    </And>
                  </Expression>
                </Criteria>
                ", mpSystem.KeyToken, mpSystem.Version.ToString()); 
#endregion
          
#region IncidentWarningCriteria
                /*
                 This criteria allows for the following scenario:
                    *incident is created with Target Resolution Time (TRT)
                    *time passes until TRT > Now()-Warning Period AND the incident has not already been marked as Breached.
                    *incident is marked as SLA Status = Warning
                 
                    * ignore incidents which are already marked Breached
                    * ignore incidents which are already marked Warning
                    * ignore incidents which are already resolved (i.e. Resolved Date is not null)  
                    
                 */
                incidentWarningCriteria = String.Format(@"
                <Criteria xmlns=""http://Microsoft.EnterpriseManagement.Core.Criteria/"">
                  <Reference Id=""System.WorkItem.Incident.Library"" PublicKeyToken=""{0}"" Version=""{1}"" Alias=""IncidentManagement"" />
                  <Expression>
                    <And>
                      <Expression>
                        <UnaryExpression>
                          <ValueExpression>
                            <Property>$Target/Property[Type='IncidentManagement!System.WorkItem.Incident']/ResolvedDate$</Property>
                          </ValueExpression>
                          <Operator>IsNull</Operator>
                        </UnaryExpression>
                      </Expression>
                      <Expression>
                        <SimpleExpression>
                          <ValueExpressionLeft>
                            <Property>$Target/Property[Type='IncidentManagement!System.WorkItem.Incident']/TargetResolutionTime$</Property>
                          </ValueExpressionLeft>
                          <Operator>Less</Operator>
                          <ValueExpressionRight>
                            <Value>" + DateTime.Now.ToUniversalTime().Add(tsWarningThreshold) + @"</Value>
                          </ValueExpressionRight>
                        </SimpleExpression>
                      </Expression>
                      <Expression>
                        <UnaryExpression>
                            <ValueExpression>
                              <Property>$Target/Property[Type='IncidentManagement!System.WorkItem.Incident']/SLAStatus$</Property>
                            </ValueExpression>
                            <Operator>IsNull</Operator>
                        </UnaryExpression>
                      </Expression>    
                    </And>
                  </Expression>
                </Criteria>
                ", mpSystem.KeyToken, mpSystem.Version.ToString()); 
#endregion

#region IncidentRevertToBlankCriteria
                /*
                 This criteria allows for the following case 
                    *incident is created with Target Resolution Time (TRT)
                    *time passes until TRT > Now()-Warning Period
                    *incident is marked as SLA Status = Warning
                    *incident urgency or priority is changed such that the Target Resolution Time **INCREASES** to a new Target Resolution Time (TRTNew).
                    *if TRTNew < Now() - Warning Period, the incident SLA status should be reverted to blank.
                    
                    *ignore incidents which are already resolved (i.e. Resolved Date is not null)
                */ 
                incidentRevertToBlankCriteria = String.Format(@"
                <Criteria xmlns=""http://Microsoft.EnterpriseManagement.Core.Criteria/"">
                  <Reference Id=""System.WorkItem.Incident.Library"" PublicKeyToken=""{0}"" Version=""{1}"" Alias=""IncidentManagement"" />
                  <Expression>
                    <And>
                      <Expression>
                        <SimpleExpression>
                          <ValueExpressionLeft>
                            <Property>$Target/Property[Type='IncidentManagement!System.WorkItem.Incident']/TargetResolutionTime$</Property>
                          </ValueExpressionLeft>
                          <Operator>Greater</Operator>
                          <ValueExpressionRight>
                            <Value>" + DateTime.Now.ToUniversalTime().Add(tsWarningThreshold) + @"</Value>
                          </ValueExpressionRight>
                        </SimpleExpression>
                      </Expression>
                      <Expression>
                        <SimpleExpression>
                          <ValueExpressionLeft>
                            <Property>$Target/Property[Type='IncidentManagement!System.WorkItem.Incident']/SLAStatus$</Property>
                          </ValueExpressionLeft>
                          <Operator>Equal</Operator>
                          <ValueExpressionRight>
                            <Value>" + "{{" + INCIDENT_SLA_STATUS_WARNING_ENUM_GUID + "}}" + @"</Value>
                          </ValueExpressionRight>
                        </SimpleExpression>
                      </Expression>
                      <Expression>
                        <UnaryExpression>
                          <ValueExpression>
                            <Property>$Target/Property[Type='IncidentManagement!System.WorkItem.Incident']/ResolvedDate$</Property>
                          </ValueExpression>
                          <Operator>IsNull</Operator>
                        </UnaryExpression>
                      </Expression>
                    </And>
                  </Expression>
                </Criteria>
                ", mpSystem.KeyToken, mpSystem.Version.ToString());
#endregion

            //Breached must be processed first, then warning, then revert
            List<EnterpriseManagementObject> listBreachedIncidents = SetSLAStatus(incidentBreachedCriteria, incidentTypeProjection, emg, INCIDENT_SLA_STATUS_BREACHED_ENUM_GUID);
            List<EnterpriseManagementObject> listWarningIncidents = SetSLAStatus(incidentWarningCriteria, incidentTypeProjection, emg, INCIDENT_SLA_STATUS_WARNING_ENUM_GUID);
            List<EnterpriseManagementObject> listRevertToBlankIncidents = SetSLAStatus(incidentRevertToBlankCriteria, incidentTypeProjection, emg, "");

            this.BreachedIncidents = listBreachedIncidents.ToArray();
            this.WarningIncidents = listWarningIncidents.ToArray();
            this.RevertToBlankIncidents = listRevertToBlankIncidents.ToArray();
            
            return ActivityExecutionStatus.Closed;
        }

        private List<EnterpriseManagementObject> SetSLAStatus(String strQueryCriteria, ManagementPackTypeProjection mptp, EnterpriseManagementGroup emg, String strSLAStatusID)
        {
            List<EnterpriseManagementObject> listObjects = new List<EnterpriseManagementObject>();

            ObjectProjectionCriteria opc = new ObjectProjectionCriteria(strQueryCriteria, mptp, emg);
            foreach (EnterpriseManagementObjectProjection emop in emg.EntityObjects.GetObjectProjectionReader<EnterpriseManagementObject>(opc, ObjectQueryOptions.Default))
            {
                if (strSLAStatusID != "")
                {
                    emop.Object[null, "SLAStatus"].Value = emg.EntityTypes.GetEnumeration(new Guid(strSLAStatusID)).Id;
                }
                else
                {
                    emop.Object[null, "SLAStatus"].Value = null;
                }
                emop.Object.Overwrite();
                listObjects.Add(emop.Object);

            }

            return listObjects;
        }
	}
}
