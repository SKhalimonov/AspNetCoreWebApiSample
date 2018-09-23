using System;

namespace WebApiSample.Data.Core
{
    /// <summary>
    /// Static class for checking other condition and raising exceptions.
    /// </summary>
    public static class Check
    {
        /// <summary>
        /// Requires the specified assertion.
        /// </summary>
        /// <typeparam name="T">Is type of message</typeparam>
        /// <param name="assertion">if assertion is <c>false</c> throws T message.</param>
        /// <param name="message">The message.</param>
        public static void Require<T>(bool assertion, string message) where T : Exception
        {
            if (!assertion)
            {
                throw (T)typeof(T).GetConstructor(new[] { typeof(string) }).Invoke(new object[] { message });
            }
        }

        /// <summary>
        /// Requires the specified assertion.
        /// </summary>
        /// <typeparam name="T">Is type of message</typeparam>
        /// <param name="assertion">if assertion is <c>false</c> throws T message.</param>
        /// <param name="message">The message.</param>
        /// <param name="parameters">optional params</param>
        public static void Require<T>(bool assertion, string message, params object[] parameters) where T : Exception
        {
            if (!assertion)
            {
                throw (T)typeof(T).GetConstructor(new[] { typeof(string) }).Invoke(new object[] { string.Format(message, parameters) });
            }
        }
    }
}
