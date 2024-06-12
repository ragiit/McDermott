using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Filtering;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl.EF;
using DevExpress.Persistent.Validation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace DxBlazorApplication2.Module.BusinessObjects
{
    [DefaultClassOptions]
    //Use this attribute to specify the caption format for the objects of the entity class.
    [ObjectCaptionFormat("{0:FullName}")]
    [DefaultProperty(nameof(FullName))]
    public class Employee : BaseObject
    {//...
        [SearchMemberOptions(SearchMemberMode.Exclude)]
        public String FullName
        {
            get { return ObjectFormatter.Format(FullNameFormat, this, EmptyEntriesMode.RemoveDelimiterWhenEntryIsEmpty); }
        }

        public static String FullNameFormat = "{FirstName} {MiddleName} {LastName}";
        public virtual String FirstName { get; set; }

        public virtual String LastName { get; set; }

        public virtual String MiddleName { get; set; }

        public virtual DateTime? Birthday { get; set; }

        //Use this attribute to specify the maximum number of characters that users can type in the editor of this property.
        [FieldSize(255)]
        public virtual String Email { get; set; }

        //Use this attribute to define a pattern that the property value must match.
        [RuleRegularExpression(@"(((http|https)\://)[a-zA-Z0-9\-\.]+\.[a-zA-Z]{2,3}(:[a-zA-Z0-9]*)?/?([a-zA-Z0-9\-\._\?\,\'/\\\+&amp;amp;%\$#\=~])*)|([a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,6})", CustomMessageTemplate = @"Invalid ""Web Page Address"".")]
        public virtual string WebPageAddress { get; set; }

        //Use this attribute to specify the maximum string length allowed for this data field.
        [StringLength(4096)]
        public virtual string Notes { get; set; }

        //Use this attribute to hide or show the editor of this property in the UI.
        [Browsable(false)]
        public virtual int TitleOfCourtesy_Int { get; set; }

        //Use this attribute to exclude the property from database mapping.
        [NotMapped]
        public virtual TitleOfCourtesy TitleOfCourtesy { get; set; }

        public virtual Department Department { get; set; }
        public virtual IList<PhoneNumber> PhoneNumbers { get; set; } = new ObservableCollection<PhoneNumber>();
        public virtual IList<DemoTask> DemoTasks { get; set; } = new ObservableCollection<DemoTask>();
        public virtual Position Position { get; set; }
        public virtual Address Address { get; set; }

        [DataSourceProperty("Department.Employees", DataSourcePropertyIsNullMode.SelectAll), DataSourceCriteria("Position.Title = 'Manager'")]
        public virtual Employee Manager { get; set; }
    }

    public enum TitleOfCourtesy
    {
        Dr,
        Miss,
        Mr,
        Mrs,
        Ms
    }
}