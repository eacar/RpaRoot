using System;

namespace Rpa.Contracts
{
    public interface IEntity<T> where T : struct
    {
        #region Properties

        T Id { get; set; }
        DateTime CreatedOn { get; set; }
        DateTime ModifiedOn { get; set; }

        #endregion

        #region Methods

        bool IsValid();

        #endregion
    }
}