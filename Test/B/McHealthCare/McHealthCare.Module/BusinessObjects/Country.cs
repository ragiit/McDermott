using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using McHealthCare.Module.BusinessObjects.Common;
using System.ComponentModel.DataAnnotations;

namespace McHealthCare.Module.BusinessObjects
{
    // Register this entity in your DbContext (usually in the BusinessObjects folder of your project) with the "public DbSet<Country> Countrys { get; set; }" syntax.
    [DefaultClassOptions]
    public class Country : BaseAuditableEntity
    {
        public Country()
        {
            // In the constructor, initialize collection properties, e.g.:
            // this.AssociatedEntities = new ObservableCollection<AssociatedEntityObject>();
        }

        [StringLength(200)]
        public virtual string Name { get; set; } = string.Empty;

        [StringLength(5)]
        public virtual string Code { get; set; } = string.Empty;

        // You can use the regular Code First syntax.
        // Property change notifications will be created automatically.
        // (see https://learn.microsoft.com/en-us/ef/core/change-tracking/change-detection#change-tracking-proxies for details)
        //public virtual string Name { get; set; }

        // Alternatively, specify more UI options:
        //[XafDisplayName("My display name"), ToolTip("My hint message")]
        //[ModelDefault("EditMask", "(000)-00"), VisibleInListView(false)]
        //[RuleRequiredField(DefaultContexts.Save)]
        //public virtual string Name { get; set; }

        // Collection property:
        //public virtual IList<AssociatedEntityObject> AssociatedEntities { get; set; }

        //[Action(Caption = "My UI Action", ConfirmationMessage = "Are you sure?", ImageName = "Attention", AutoCommit = true)]
        //public void ActionMethod() {
        //    // Trigger custom business logic for the current record in the UI (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112619.aspx).
        //    this.PersistentProperty = "Paid";
        //}
    }
}