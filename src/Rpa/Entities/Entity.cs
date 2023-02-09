using System;
using System.Collections.Generic;
using Rpa.Contracts;

namespace Rpa.Entities
{
    [Serializable]
    public abstract class Entity<T> : IEntity<T> where T : struct
    {
        #region Properties - Mapped - IEntity

        public T Id { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }

        #endregion

        #region Constructors

        protected Entity()
        {
            CreatedOn = DateTime.UtcNow;
            ModifiedOn = DateTime.UtcNow;
        }

        #endregion

        #region Methods Public - IEntity

        public virtual bool IsValid()
        {
            return !EqualityComparer<T>.Default.Equals(Id, default(T));
        }

        #endregion
    }
}
