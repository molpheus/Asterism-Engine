using System;
using System.Collections;
using System.Runtime.CompilerServices;

using UnityEngine;

namespace Asterism
{
    public class Debugger : MonoBehaviour
    {
        public static bool IsLogView { get; set;} = true;

        /// <summary> 最大のログ保持数 </summary>
        private const int _maxLogStock = 100;

        private const string _logFormat = ">>> {0} <<<";
        private const string _logWarningFormat = "*** {0} ***";
        private const string _logErrorFormat = "!!! {0} !!!";

        /// <summary>
        /// ログの保存用
        /// </summary>
        public struct Content
        {
            public string Condition { get; private set; }
            public string StackTrace { get; private set; }
            public LogType Type { get; private set; }

            public Content(string condition, string stackTrace, LogType type)
            {
                Condition = condition;
                StackTrace = stackTrace;
                Type = type;
            }
        }

        /// <summary>
        /// ログを文字列で取得
        /// </summary>
        public string LogString
        {
            get
            {
                string str = string.Empty;
                var list = _logStock.ToArray();
                foreach(Content content in list)
                {
                    str += content.Condition;
                    str += "\n";
                }

                return str;
            }
        }

        public Queue LogStock => _logStock;
        private Queue _logStock;

        private void Awake()
        {
            _logStock = new Queue();
            Application.logMessageReceivedThreaded += logMessageReceivedThreaded;
            ServiceLocator.Register(this);
        }

        private void OnDestroy()
        {
            ServiceLocator.Unregister(this);
            _logStock.Clear();
            Application.logMessageReceivedThreaded -= logMessageReceivedThreaded;
        }

        /// <summary>
        /// ログをキャッチする
        /// </summary>
        private void logMessageReceivedThreaded(string logString, string stackTrace, LogType type)
        {
            var content = new Content(logString, stackTrace, type);
            _logStock.Enqueue(content);
            if (_logStock.Count > _maxLogStock)
            {
                _logStock.Dequeue();
            }
        }

        /// <summary>
        /// ログを削除
        /// </summary>
        public static void Clear()
        {
            var instance = ServiceLocator.GetInstance<Debugger>();
            if (instance != null)
            {
                instance.LogStock.Clear();
            }
            // デベロッパーコンソールからエラーを削除
            Debug.ClearDeveloperConsole();
        }

        /// <summary>
        /// 条件をアサ―トし、失敗したときに Unity コンソールにエラーメッセージをログします。
        /// LogType.Assert タイプのメッセージはログに記録されます。
        /// </summary>
        /// <param name="condition"> true となる条件 </param>
        static public void Assert(bool condition)
        {
            if (!IsLogView) return;

            Debug.Assert(condition);
        }
        /// <summary>
        /// 条件をアサ―トし、失敗したときに Unity コンソールにエラーメッセージをログします。
        /// LogType.Assert タイプのメッセージはログに記録されます。
        /// </summary>
        /// <param name="condition"> true となる条件 </param>
        /// <param name="context"> メッセージが適用されるオブジェクト </param>
        static public void Assert(bool condition, UnityEngine.Object context)
        {
            if (!IsLogView) return;

            Debug.Assert(condition, context);
        }
        /// <summary>
        /// 条件をアサ―トし、失敗したときに Unity コンソールにエラーメッセージをログします。
        /// LogType.Assert タイプのメッセージはログに記録されます。
        /// </summary>
        /// <param name="condition"> true となる条件 </param>
        /// <param name="message"> 表示の際に文字列として変換対象となる文字列やオブジェクト </param>
        static public void Assert(bool condition, object message)
        {
            if (!IsLogView) return;

            Debug.Assert(condition, message);
        }
        /// <summary>
        /// 条件をアサ―トし、失敗したときに Unity コンソールにエラーメッセージをログします。
        /// LogType.Assert タイプのメッセージはログに記録されます。
        /// </summary>
        /// <param name="condition"> true となる条件 </param>
        /// <param name="message"> 表示の際に文字列として変換対象となる文字列やオブジェクト </param>
        /// <param name="context"> メッセージが適用されるオブジェクト </param>
        static public void Assert(bool condition, object message, UnityEngine.Object context)
        {
            if (!IsLogView) return;

            Debug.Assert(condition, message, context);
        }
        /// <summary>
        /// 条件を定義し、失敗したときに Unity コンソールにフォーマット済みのエラーメッセージをログします。
        /// LogType.Assert タイプのメッセージはログに記録されます。
        /// </summary>
        /// <param name="condition"> true となる条件 </param>
        /// <param name="format"> フォーマット文字列 </param>
        /// <param name="args"> フォーマットで使用する値 </param>
        public static void AssertFormat(bool condition, string format, params object[] args)
        {
            if (!IsLogView) return;

            Debug.AssertFormat(condition, format, args);
        }
        /// <summary>
        /// 条件を定義し、失敗したときに Unity コンソールにフォーマット済みのエラーメッセージをログします。
        /// LogType.Assert タイプのメッセージはログに記録されます。
        /// </summary>
        /// <param name="condition"> true となる条件 </param>
        /// <param name="context"> メッセージが適用されるオブジェクト </param>
        /// <param name="format"> フォーマット文字列 </param>
        /// <param name="args"> フォーマットで使用する値 </param>
        public static void AssertFormat(bool condition, UnityEngine.Object context, string format, params object[] args)
        {
            if (!IsLogView) return;

            Debug.AssertFormat(condition, context, format, args);
        }
        /// <summary>
        /// Logs a message to the Unity Console.
        /// </summary>
        /// <param name="message"> 表示の際に文字列として変換対象となる文字列やオブジェクト </param>
        public static void Log(object message)
        {
            if (!IsLogView) return;

            LogFormat(_logFormat, message);
        }
        /// <summary>
        /// Logs a message to the Unity Console.
        /// </summary>
        /// <param name="message"> 表示の際に文字列として変換対象となる文字列やオブジェクト </param>
        /// <param name="context"> メッセージが適用されるオブジェクト </param>
        public static void Log(object message, UnityEngine.Object context)
        {
            if (!IsLogView) return;

            LogFormat(context, _logFormat, message);
        }
        /// <summary>
        /// Debug.Log の派生でアサーションメッセージをコンソールに出力します。
        /// LogType.Assert タイプのメッセージはログに記録されます。
        /// </summary>
        /// <param name="message"> 表示の際に文字列として変換対象となる文字列やオブジェクト </param>
        public static void LogAssertion(object message)
        {
            if (!IsLogView) return;

            LogAssertionFormat(_logFormat, message);
        }
        /// <summary>
        /// Debug.Log の派生でアサーションメッセージをコンソールに出力します。
        /// LogType.Assert タイプのメッセージはログに記録されます。
        /// </summary>
        /// <param name="message"> 表示の際に文字列として変換対象となる文字列やオブジェクト </param>
        /// <param name="context"> メッセージが適用されるオブジェクト </param>
        public static void LogAssertion(object message, UnityEngine.Object context)
        {
            if (!IsLogView) return;

            LogAssertionFormat(context, _logFormat, message);
        }
        /// <summary>
        /// Unity Console にフォーマットされたアサーションメッセージを記録します。
        /// </summary>
        /// <param name="format"> フォーマット文字列 </param>
        /// <param name="args"> フォーマットで使用する値 </param>
        public static void LogAssertionFormat(string format, params object[] args)
        {
            if (!IsLogView) return;

            Debug.LogAssertionFormat(format, args);
        }
        /// <summary>
        /// Unity Console にフォーマットされたアサーションメッセージを記録します。
        /// </summary>
        /// <param name="context"> メッセージが適用されるオブジェクト </param>
        /// <param name="format"> フォーマット文字列 </param>
        /// <param name="args"> フォーマットで使用する値 </param>
        public static void LogAssertionFormat(UnityEngine.Object context, string format, params object[] args)
        {
            if (!IsLogView) return;

            Debug.LogAssertionFormat(context, format, args);
        }
        /// <summary>
        /// Debug.Log の派生でエラーメッセージをコンソールに出力します。
        /// </summary>
        /// <param name="message"> 表示の際に文字列として変換対象となる文字列やオブジェクト </param>
        public static void LogError(object message)
        {
            if (!IsLogView) return;

            Debug.LogError(string.Format(_logErrorFormat, message));
        }
        /// <summary>
        /// Debug.Log の派生でエラーメッセージをコンソールに出力します。
        /// </summary>
        /// <param name="message"> 表示の際に文字列として変換対象となる文字列やオブジェクト </param>
        /// <param name="context"> メッセージが適用されるオブジェクト </param>
        public static void LogError(object message, UnityEngine.Object context)
        {
            if (!IsLogView) return;

            Debug.LogError(string.Format(_logErrorFormat, message), context);
        }
        /// <summary>
        /// Unity Console にフォーマットされたエラーメッセージを記録します。
        /// </summary>
        /// <param name="format"> フォーマット文字列 </param>
        /// <param name="args"> フォーマットで使用する値 </param>
        public static void LogErrorFormat(string format, params object[] args)
        {
            if (!IsLogView) return;

            Debug.LogErrorFormat(format, args);
        }
        /// <summary>
        /// Unity Console にフォーマットされたエラーメッセージを記録します。
        /// </summary>
        /// <param name="context"> メッセージが適用されるオブジェクト </param>
        /// <param name="format"> フォーマット文字列 </param>
        /// <param name="args"> フォーマットで使用する値 </param>
        public static void LogErrorFormat(UnityEngine.Object context, string format, params object[] args)
        {
            if (!IsLogView) return;

            Debug.LogErrorFormat(context, format, args);
        }
        /// <summary>
        /// Debug.Log の派生でエラーメッセージをコンソールに出力します。
        /// </summary>
        /// <param name="exception"> Runtime Exception </param>
        public static void LogException(Exception exception)
        {
            if (!IsLogView) return;

            Debug.LogException(exception);
        }
        /// <summary>
        /// Debug.Log の派生でエラーメッセージをコンソールに出力します。
        /// </summary>
        /// <param name="exception"> Runtime Exception </param>
        /// <param name="context"> メッセージが適用されるオブジェクト </param>
        public static void LogException(Exception exception, UnityEngine.Object context)
        {
            if (!IsLogView) return;

            Debug.LogException(exception, context);
        }
        /// <summary>
        /// Unity Console にフォーマットされたメッセージを記録します。
        /// </summary>
        /// <param name="format"> フォーマット文字列 </param>
        /// <param name="args"> フォーマットで使用する値 </param>
        public static void LogFormat(string format, params object[] args)
        {
            if (!IsLogView) return;

            Debug.LogFormat(format, args);
        }
        /// <summary>
        /// Unity Console にフォーマットされたメッセージを記録します。
        /// </summary>
        /// <param name="context"> メッセージが適用されるオブジェクト </param>
        /// <param name="format"> フォーマット文字列 </param>
        /// <param name="args"> フォーマットで使用する値 </param>
        public static void LogFormat(UnityEngine.Object context, string format, params object[] args)
        {
            if (!IsLogView) return;

            Debug.LogFormat(context, format, args);
        }
        /// <summary>
        /// Unity Console にフォーマットされたメッセージを記録します。
        /// </summary>
        /// <param name="logType"> Type of message e.g. warn or error etc. </param>
        /// <param name="logOptions"> Option flags to treat the log message special. </param>
        /// <param name="context"> メッセージが適用されるオブジェクト </param>
        /// <param name="format"> フォーマット文字列 </param>
        /// <param name="args"> フォーマットで使用する値 </param>
        public static void LogFormat(LogType logType, LogOption logOptions, UnityEngine.Object context, string format, params object[] args)
        {
            if (!IsLogView) return;

            Debug.LogFormat(logType, logOptions, context, format, args);
        }
        /// <summary>
        /// Debug.Log の派生で警告メッセージをコンソール出力します。
        /// </summary>
        /// <param name="message"> 表示の際に文字列として変換対象となる文字列やオブジェクト </param>
        public static void LogWarning(object message)
        {
            if (!IsLogView) return;

            LogWarningFormat(_logWarningFormat, message);
        }
        /// <summary>
        /// Debug.Log の派生で警告メッセージをコンソール出力します。
        /// </summary>
        /// <param name="message"> 表示の際に文字列として変換対象となる文字列やオブジェクト </param>
        /// <param name="context"> メッセージが適用されるオブジェクト </param>
        public static void LogWarning(object message, UnityEngine.Object context)
        {
            if (!IsLogView) return;

            LogWarningFormat(context, _logWarningFormat, message);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="format"> フォーマット文字列 </param>
        /// <param name="args"> フォーマットで使用する値 </param>
        public static void LogWarningFormat(string format, params object[] args)
        {
            if (!IsLogView) return;

            Debug.LogWarningFormat(format, args);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"> メッセージが適用されるオブジェクト </param>
        /// <param name="format"> フォーマット文字列 </param>
        /// <param name="args"> フォーマットで使用する値 </param>
        public static void LogWarningFormat(UnityEngine.Object context, string format, params object[] args)
        {
            if (!IsLogView) return;

            Debug.LogWarningFormat(context, format, args);
        }



    }
}
