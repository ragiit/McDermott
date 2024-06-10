using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Layout;
using DevExpress.ExpressApp.Model.NodeGenerators;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Templates;
using DevExpress.ExpressApp.Utils;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DxBlazorApplication2.Module.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DxBlazorApplication2.Module.Controllers
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class ClearContactTasksController : ViewController
    {
        // Use CodeRush to create Controllers and Actions with a few keystrokes.
        // https://docs.devexpress.com/CodeRushForRoslyn/403133/
        public ClearContactTasksController()
        {
            InitializeComponent();
            // Target required Views (via the TargetXXX properties) and create their Actions.
            TargetViewType = ViewType.DetailView;
            //Specify the type of objects that can use the Controller
            TargetObjectType = typeof(Employee);

            SimpleAction clearTasksAction = new SimpleAction(this, "ClearTaskAction", PredefinedCategory.View)
            {
                //Specify the Action's button caption.
                Caption = "Clear tasks",
                //Specify the confirmation message that pops up when a user executes an Action.
                ConfirmationMessage = "Are you sure you want to clear the Tasks list?",
                //Specify the icon of the Action's button in the interface.
                ImageName = "Action_Clear"
            };
            //This event fires when a user clicks the Simple Action control. Handle this event to execute custom code.
            clearTasksAction.Execute += ClearTasksAction_Execute;
        }

        private void ClearTasksAction_Execute(Object sender, SimpleActionExecuteEventArgs e)
        {
            while (((Employee)View.CurrentObject).DemoTasks.Count > 0)
            {
                ((Employee)View.CurrentObject).DemoTasks.Remove(((Employee)View.CurrentObject).DemoTasks[0]);
                ObjectSpace.SetModified(View.CurrentObject, View.ObjectTypeInfo.FindMember(nameof(Employee.DemoTasks)));
            }
        }

        protected override void OnActivated()
        {
            base.OnActivated();
            // Perform various tasks depending on the target View.
        }

        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            // Access and customize the target View control.
        }

        protected override void OnDeactivated()
        {
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }
    }
}