using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl.EF;
using DevExpress.XtraPrinting.Native;
using System.Collections.ObjectModel;

namespace DxBlazorApplication2.Module.BusinessObjects
{
    [DefaultClassOptions]
    //Use this attribute to define the name of the objects of this type in the user interface.
    [ModelDefault("Caption", "Task")]
    public class DemoTask : BaseObject
    {
        public virtual DateTime? DateCompleted { get; set; }

        public virtual String Subject { get; set; }

        [FieldSize(FieldSizeAttribute.Unlimited)]
        public virtual String Description { get; set; }

        public virtual DateTime? DueDate { get; set; }

        public virtual DateTime? StartDate { get; set; }

        public virtual int PercentCompleted { get; set; }

        private TaskStatus status;

        public virtual TaskStatus Status
        {
            get { return status; }
            set
            {
                status = value;
                if (isLoaded)
                {
                    if (value == TaskStatus.Completed)
                    {
                        DateCompleted = DateTime.Now;
                    }
                    else
                    {
                        DateCompleted = null;
                    }
                }
            }
        }

        [Action(ImageName = "State_Task_Completed")]
        public void MarkCompleted()
        {
            Status = TaskStatus.Completed;
        }

        private bool isLoaded = false;

        public override void OnLoaded()
        {
            isLoaded = true;
        }

        public virtual IList<Employee> Employees { get; set; } = new ObservableCollection<Employee>();
        public virtual Priority Priority { get; set; }

        public override void OnCreated()
        {
            Priority = Priority.Normal;

            base.OnCreated();
        }

        [Action(ToolTip = "Postpone the task to the next day", Caption = "Postpone")]
        public void Postpone()
        {
            if (DueDate == DateTime.MinValue)
            {
                DueDate = DateTime.Now;
            }
            DueDate = DueDate + TimeSpan.FromDays(1);
        }
    }

    public enum Priority
    {
        [ImageName("State_Priority_Low")]
        Low,

        [ImageName("State_Priority_Normal")]
        Normal,

        [ImageName("State_Priority_High")]
        High
    }

    public enum TaskStatus
    {
        [ImageName("State_Task_NotStarted")]
        NotStarted,

        [ImageName("State_Task_InProgress")]
        InProgress,

        [ImageName("State_Task_WaitingForSomeoneElse")]
        WaitingForSomeoneElse,

        [ImageName("State_Task_Deferred")]
        Deferred,

        [ImageName("State_Task_Completed")]
        Completed
    }
}