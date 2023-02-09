using System;

namespace Rpa.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class ColumnIndexAttribute : Attribute
    {
        #region Properties

        public int Value { get; set; }

        #endregion

        #region Constructors

        public ColumnIndexAttribute(int value)
        {
            Value = value;
        }

        #endregion
    }
}