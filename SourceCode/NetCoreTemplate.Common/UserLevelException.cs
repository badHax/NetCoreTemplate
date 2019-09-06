using System;

namespace NetCoreTemplate.Common
{
    /// <summary>
    /// For errors that can be displayed to the user
    /// </summary>
    public class UserLevelException : Exception
    {
        public UserLevelException(string message) : base(message)
        { 
        }
    }
}
