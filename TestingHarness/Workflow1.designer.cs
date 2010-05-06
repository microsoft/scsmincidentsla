using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Collections;
using System.Drawing;
using System.Reflection;
using System.Workflow.ComponentModel.Compiler;
using System.Workflow.ComponentModel.Serialization;
using System.Workflow.ComponentModel;
using System.Workflow.ComponentModel.Design;
using System.Workflow.Runtime;
using System.Workflow.Activities;
using System.Workflow.Activities.Rules;

namespace TestingHarness
{
    partial class Workflow1
    {
        #region Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCode]
        private void InitializeComponent()
        {
            this.CanModifyActivities = true;
            this.DebugOutput = new System.Workflow.Activities.CodeActivity();
            this.GetSLABreaches = new Microsoft.Demo.IncidentSLAManagement.SLAGetBreachingIncidents();
            // 
            // DebugOutput
            // 
            this.DebugOutput.Name = "DebugOutput";
            this.DebugOutput.ExecuteCode += new System.EventHandler(this.codeActivity1_ExecuteCode);
            // 
            // GetSLABreaches
            // 
            this.GetSLABreaches.Name = "GetSLABreaches";
            this.GetSLABreaches.Password = "Password!";
            this.GetSLABreaches.ServerName = "192.168.10.101";
            this.GetSLABreaches.UserName = "litware\\administrator";
            // 
            // Workflow1
            // 
            this.Activities.Add(this.GetSLABreaches);
            this.Activities.Add(this.DebugOutput);
            this.Name = "Workflow1";
            this.CanModifyActivities = false;

        }

        #endregion

        private CodeActivity DebugOutput;
        private Microsoft.Demo.IncidentSLAManagement.SLAGetBreachingIncidents GetSLABreaches;













































    }
}
