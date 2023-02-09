using System;
using System.Reflection;

namespace Rpa.Exceptions
{
    [Serializable]
    public class ExceptionBase : Exception
    {
        #region Fields

        private readonly Exception _mostInnerException;

        #endregion

        #region Properties

        public string ErrorCode => $"{_mostInnerException?.GetType()?.Name} {GetMethodName(_mostInnerException?.TargetSite?.ReflectedType)}";
        public string AdditionalMessage { get; }
        public override string Message => $"{base.Message}{(!string.IsNullOrWhiteSpace(AdditionalMessage) ? " | " + AdditionalMessage : string.Empty)}";

        #endregion

        #region Constructors

        public ExceptionBase()
        {
            _mostInnerException = GetTheMostInnerException(this);
        }
        protected ExceptionBase(string additionalMessage = "") : base("")
        {
            AdditionalMessage = additionalMessage;
            _mostInnerException = GetTheMostInnerException(this);
        }
        public ExceptionBase(string message, string additionalMessage = "") : base(message)
        {
            AdditionalMessage = additionalMessage;
            _mostInnerException = GetTheMostInnerException(this);
        }
        public ExceptionBase(string message, Exception innerEx, string additionalMessage = "") : base(message, innerEx)
        {
            AdditionalMessage = additionalMessage;
            _mostInnerException = GetTheMostInnerException(this);
        }
        public ExceptionBase(Exception innerEx, string additionalMessage = "") : base("", innerEx)
        {
            AdditionalMessage = additionalMessage;
            _mostInnerException = GetTheMostInnerException(this);
        }

        #endregion

        #region Methods - Private

        private Exception GetTheMostInnerException(Exception ex)
        {
            if (ex.InnerException != null)
                return GetTheMostInnerException(ex.InnerException);

            return ex;
        }
        private string GetMethodName(MemberInfo memberInfo)
        {
            var result = "";

            if (memberInfo?.ReflectedType != null)
            {
                result = memberInfo.ReflectedType.FullName;

                if (memberInfo.ReflectedType.ReflectedType != null)
                    result += " - " + GetMethodName(memberInfo.ReflectedType.ReflectedType);
            }

            return result;
        }

        #endregion
    }
}