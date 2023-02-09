using Newtonsoft.Json;
using Rpa.Extensions;
using System;

namespace Rpa.Entities
{
    public sealed class NextTransactionStatus<TStatusType> where TStatusType : struct, Enum
    {
        #region Fields

        private int _statusTypeId;
        private TStatusType _statusType;

        #endregion

        #region Properties

        #region StatusType

        public int StatusTypeId
        {
            get => _statusTypeId;
            set
            {
                _statusTypeId = value;
                _statusType = value.Convert<TStatusType>();
            }
        }
        
        [JsonIgnore]
        public TStatusType StatusType
        {
            get => _statusType;
            set
            {
                _statusType = value;
                _statusTypeId = value.Convert<int>();
            }
        }

        #endregion

        public int Order { get; set; }

        #endregion
    }
}