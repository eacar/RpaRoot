using System.Collections.Generic;

namespace Rpa.Contracts
{
    public interface ITreeViewList<T>
    {
        #region Properties

        T Parent { get; set; }
        IList<T> Children { get; set; }
        bool HasChildren { get; }

        #endregion
    }
    public interface ITreeViewList<T, TId>
    {
        #region Properties

        TId ParentId { get; set; }
        IList<T> Children { get; set; }
        bool HasChildren { get; }

        #endregion
    }
}