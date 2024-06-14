using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl.EF;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DxBlazorApplication2.Module.BusinessObjects
{
    [DefaultProperty(nameof(Number))]
    public class PhoneNumber : BaseObject
    {
        public virtual String Number { get; set; }

        public virtual String PhoneType { get; set; }

        public override String ToString()
        {
            return Number;
        }

        public virtual Employee Employee { get; set; }
    }
}