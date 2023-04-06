using UnityEngine;

namespace Asterism
{
    public class ExtendDebug
    {
        public ExtendDebug()
        {
            Application.logMessageReceived += LogMessageReceived;
        }

        private void LogMessageReceived(string condition, string stackTrace, LogType type)
        {
            switch (type)
            {
                case LogType.Log: break;
                case LogType.Warning: break;
                case LogType.Assert: break;
                case LogType.Error: break;
                case LogType.Exception: break;
            }
        }

        static public void Log(object message, Object context)
        {
            Debug.Log(message, context);
        }

        static public void Log(string message, params object[] context)
        {
            Debug.Log(">>> " + string.Format(message, context) + " <<<");
        }

        static public void LogWarning(object message, Object context)
        {
            Debug.LogWarning(message, context);
        }

        static public void LogWarningFormat(string message, params object[] context)
        {
            Debug.LogWarningFormat("*** " + message + " ***", context);
        }

        static public void LogError(object message, Object context)
        {
            Debug.LogError(message, context);
        }

        static public void LogError(string message, params object[] context)
        {
            Debug.LogError("!!! " + string.Format(message, context) + " !!!");
        }

        static public void LogAssertion(object message, Object context)
        {
            Debug.LogAssertion(message, context);
        }

        static public void LogAssertionFormat(string message, params object[] context)
        {
            Debug.LogAssertionFormat(message, context);
        }

        static public void LogException(System.Exception exception, Object context)
        {
            Debug.LogException(exception, context);
        }

        
    }
}
