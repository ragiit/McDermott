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
    [DefaultProperty(nameof(FullAddress))]
    public class Address : BaseObject
    {
        private const string defaultFullAddressFormat = "{Country}; {StateProvince}; {City}; {Street}; {ZipPostal}";

        public virtual String Street { get; set; }

        public virtual String City { get; set; }

        public virtual String StateProvince { get; set; }

        public virtual String ZipPostal { get; set; }

        public virtual String Country { get; set; }

        public String FullAddress
        {
            get { return ObjectFormatter.Format(defaultFullAddressFormat, this, EmptyEntriesMode.RemoveDelimiterWhenEntryIsEmpty); }
        }
    }
}