using System;
using System.Collections.Generic;

namespace Asterism.Common
{
    public static class ServiceLocator
    {
        /// <summary>
        /// インスタンスを登録する辞書。
        /// </summary>
        static readonly Dictionary<Type, object> _instances = new Dictionary<Type, object>();

        /// <summary>
        /// インスタンスの登録をすべて解除します。
        /// </summary>
        static void Clear()
        {
            _instances.Clear();
        }

        /// <summary>
        /// インスタンスを登録します。すでに同じ型のインスタンスが登録されている場合は登録できませんので、先に Unregister を行ってください。
        /// </summary>
        /// <param name="instance">登録するインスタンス。</param>
        /// <typeparam name="T">登録するインスタンスの型。</typeparam>
        public static void Register<T>(T instance) where T : class
        {
            var type = typeof(T);

            if (_instances.ContainsKey(type))
                throw new Exception($"すでに同じ型のインスタンスが登録されています：{type.Name}");

            _instances[type] = instance;
        }

        /// <summary>
        /// インスタンスの登録を解除します。インスタンスが登録されていなかった場合は警告が出ます。
        /// </summary>
        /// <param name="instance">登録を解除するインスタンス。</param>
        /// <typeparam name="T">登録を解除するインスタンスの型。</typeparam>
        public static void Unregister<T>(T instance) where T : class
        {
            var type = typeof(T);

            if (!_instances.ContainsKey(type))
                throw new Exception($"要求された型のインスタンスが登録されていません：{type.Name}");

            if (!Equals(_instances[type], instance))
                throw new Exception($"登録されている要求された型のインスタンスと渡されたインスタンスが一致しません：{type.Name}");

            _instances.Remove(type);
        }

        /// <summary>
        /// 指定された型のインスタンスがすでに登録されているかをチェックします。
        /// </summary>
        /// <typeparam name="T">登録を確認するインスタンスの型。</typeparam>
        /// <returns>指定された型のインスタンスがすでに登録されている場合は true を返します。</returns>
        public static bool IsRegistered<T>() where T : class
        {
            return _instances.ContainsKey(typeof(T));
        }

        /// <summary>
        /// 渡されたインスタンスがすでに登録されているかをチェックします。
        /// </summary>
        /// <param name="instance">登録を確認するインスタンス。</param>
        /// <typeparam name="T">登録を確認するインスタンスの型。</typeparam>
        /// <returns>渡されたインスタンスが既に登録されている場合は true を返します。</returns>
        public static bool IsRegistered<T>(T instance) where T : class
        {
            var type = typeof(T);

            return _instances.ContainsKey(type) && Equals(_instances[type], instance);
        }

        /// <summary>
        /// インスタンスを取得します。
        /// </summary>
        /// <typeparam name="T">取得したいインスタンスの型。</typeparam>
        /// <returns>取得したインスタンスを返します。取得できなかった場合は null を返します。</returns>
        public static T GetInstance<T>() where T : class
        {
            TryGetInstance<T>(out var instance);

            return instance;
        }

        /// <summary>
        /// インスタンスを取得し、渡された引数に代入します。
        /// </summary>
        /// <param name="instance">取得したインスタンスを入れる変数。</param>
        /// <typeparam name="T">取得したいインスタンスの型。</typeparam>
        /// <returns>取得が成功したら true を返します。</returns>
        public static bool TryGetInstance<T>(out T instance) where T : class
        {
            var type = typeof(T);

            instance = _instances.ContainsKey(type) ? _instances[type] as T : null;

            return instance != null;
        }
    }

}
