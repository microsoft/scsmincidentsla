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
using Microsoft.EnterpriseManagement.Common;

namespace TestingHarness
{
    public sealed partial class Workflow1 : SequentialWorkflowActivity
    {
        public Workflow1()
        {
            InitializeComponent();
        }

        private void codeActivity1_ExecuteCode(object sender, EventArgs e)
        {
            /*
            foreach (EnterpriseManagementObject item in this.GetSLABreaches.Incidents)
            {
                Console.WriteLine("--------------------------------------------------------------");
                Console.WriteLine("Name: {0}", item.Name);
                Console.WriteLine("Impact: {0}", item[null, "Impact"].Value.ToString());
                Console.WriteLine("Urgency: {0}", item[null, "Urgency"].Value.ToString());
                Console.WriteLine("Status: {0}", item[null, "Status"].Value.ToString());
                if (item[null, "SLABreachDetected"].Value != null)
                {
                    Console.WriteLine("SLABreachDetected: {0}", item[null, "SLABreachDetected"].Value.ToString());
                }
                else
                {
                    Console.WriteLine("SLABreachDetected: {0}", "null");
                }
                Console.WriteLine("Target Resolution Time: {0}", ((DateTime)item[null, "TargetResolutionTime"].Value).ToLocalTime().ToString());
            }

            Console.WriteLine("----------------- Workflow done! -------------------------");
            Console.ReadLine();
             */ 
        }
    }

}
