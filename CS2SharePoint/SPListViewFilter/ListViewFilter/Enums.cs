using System;

namespace ListViewFilter
{
    /// <summary>
    /// Filter type
    /// </summary>
    [Flags]
    public enum FilterType
    {
        /// <summary>
        /// Text filter
        /// </summary>
        Text = 1,
        /// <summary>
        /// Text filter with selection (Equal, Contains, BeginsWith, etc)
        /// </summary>
        TextWithOptions = 2,
        /// <summary>
        /// Drop down
        /// </summary>
        DropDownSingleValue = 4,
        /// <summary>
        /// Drop down with multi select
        /// </summary>
        DropDownMultiValue = 8,
        /// <summary>
        /// Text with autocomplete
        /// </summary>
        AutoComplete = 16,
        /// <summary>
        /// Date
        /// </summary>
        Date = 32,
        /// <summary>
        /// DateRange
        /// </summary>
        DateRange = 64,
        /// <summary>
        /// People or Group
        /// </summary>
        PeoplePicker = 128,
        /// <summary>
        /// People or Group with multi select
        /// </summary>
        PeoplePickerMulti = 256,
        /// <summary>
        /// Flag
        /// </summary>
        Boolean = 512,
        /// <summary>
        /// Taxonomy Term
        /// </summary>
        TaxonomyTerm = 1024,
        /// <summary>
        /// Taxonomy Term with multi select
        /// </summary>
        TaxonomyMultiTerm = 2048,
        /// <summary>
        /// "Wide" controls
        /// </summary>
        Wide = (DateRange | PeoplePickerMulti | TaxonomyTerm | TaxonomyMultiTerm)
    }

    internal enum CAMLOperator
    {
        Eq = 0,
        Neq = 1,
        Gt = 2,
        Geq = 3,
        Lt = 4,
        Leq = 5,
        IsNull = 6,
        BeginsWith = 7,
        Contains = 8
    }

    internal enum CAMLFieldType
    {
        Text = 0,
        Note = 1,
        DateTime = 2,
        Counter = 3,
        Boolean = 4,
        Number = 5,
        Currency = 6,
        Attachments,
        User,
        ModStat,
        Integer,
        TaxonomyField,
        TaxonomyFieldMultiValue
    }

    /// <summary>
    /// Type of filter' panel
    /// </summary>
    public enum FilterPanelType
    {
        /// <summary>
        /// WrapPanel
        /// </summary>
        WrapPanel = 0,
        /// <summary>
        /// Grid
        /// </summary>
        Grid = 1,
        /// <summary>
        /// Horizontally oriented StackPanel
        /// </summary>
        StackPanel = 2
    }
}
