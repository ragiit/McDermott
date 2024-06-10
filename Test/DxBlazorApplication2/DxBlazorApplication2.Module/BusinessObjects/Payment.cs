using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl.EF;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DxBlazorApplication2.Module.BusinessObjects
{
    [DefaultClassOptions]
    public class Payment : BaseObject
    {
        //Use this attribute to specify the display format pattern for the property value.
        [ModelDefault("DisplayFormat", "{0:c}")]
        public virtual double Rate { get; set; }

        public virtual double Hours { get; set; }

        //Use this attribute to exclude the property from database mapping.
        [NotMapped]
        [ModelDefault("DisplayFormat", "{0:c}")]
        public double Amount
        {
            get { return Rate * Hours; }
        }
    }
}