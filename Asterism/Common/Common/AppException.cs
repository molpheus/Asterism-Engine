using System;

namespace Asterism.Common
{
    public partial class AppException : Exception
    {
        public AppException(string message) : base(message) { }
    }
}
