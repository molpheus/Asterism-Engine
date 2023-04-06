using System;
using System.Collections.Generic;
using UnityEngine;

namespace Asterism
{
    public static class ServiceLocator
    {
        /// <summary>
        /// �C���X�^���X��o�^���鎫���B
        /// </summary>
        static readonly Dictionary<Type, object> instances = new Dictionary<Type, object>();

        /// <summary>
        /// �C���X�^���X�̓o�^�����ׂĉ������܂��B
        /// </summary>
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        static void Initialize()
        {
            instances.Clear();
        }

        /// <summary>
        /// �C���X�^���X��o�^���܂��B���łɓ����^�̃C���X�^���X���o�^����Ă���ꍇ�͓o�^�ł��܂���̂ŁA��� Unregister ���s���Ă��������B
        /// </summary>
        /// <param name="instance">�o�^����C���X�^���X�B</param>
        /// <typeparam name="T">�o�^����C���X�^���X�̌^�B</typeparam>
        public static void Register<T>(T instance) where T : class
        {
            var type = typeof(T);

            if (instances.ContainsKey(type)) {
                Debugger.LogWarning($"���łɓ����^�̃C���X�^���X���o�^����Ă��܂��F{type.Name}");
                return;
            }

            instances[type] = instance;
        }

        /// <summary>
        /// �C���X�^���X�̓o�^���������܂��B�C���X�^���X���o�^����Ă��Ȃ������ꍇ�͌x�����o�܂��B
        /// </summary>
        /// <param name="instance">�o�^����������C���X�^���X�B</param>
        /// <typeparam name="T">�o�^����������C���X�^���X�̌^�B</typeparam>
        public static void Unregister<T>(T instance) where T : class
        {
            var type = typeof(T);

            if (!instances.ContainsKey(type)) {
                Debugger.LogWarning($"�v�����ꂽ�^�̃C���X�^���X���o�^����Ă��܂���F{type.Name}");
                return;
            }

            if (!Equals(instances[type], instance)) {
                Debugger.LogWarning($"�o�^����Ă���v�����ꂽ�^�̃C���X�^���X�Ɠn���ꂽ�C���X�^���X����v���܂���F{type.Name}");
                return;
            }

            instances.Remove(type);
        }

        /// <summary>
        /// �w�肳�ꂽ�^�̃C���X�^���X�����łɓo�^����Ă��邩���`�F�b�N���܂��B
        /// </summary>
        /// <typeparam name="T">�o�^���m�F����C���X�^���X�̌^�B</typeparam>
        /// <returns>�w�肳�ꂽ�^�̃C���X�^���X�����łɓo�^����Ă���ꍇ�� true ��Ԃ��܂��B</returns>
        public static bool IsRegistered<T>() where T : class
        {
            return instances.ContainsKey(typeof(T));
        }

        /// <summary>
        /// �n���ꂽ�C���X�^���X�����łɓo�^����Ă��邩���`�F�b�N���܂��B
        /// </summary>
        /// <param name="instance">�o�^���m�F����C���X�^���X�B</param>
        /// <typeparam name="T">�o�^���m�F����C���X�^���X�̌^�B</typeparam>
        /// <returns>�n���ꂽ�C���X�^���X�����ɓo�^����Ă���ꍇ�� true ��Ԃ��܂��B</returns>
        public static bool IsRegistered<T>(T instance) where T : class
        {
            var type = typeof(T);

            return instances.ContainsKey(type) && Equals(instances[type], instance);
        }

        /// <summary>
        /// �C���X�^���X���擾���܂��B�擾�ł��Ȃ������ꍇ�̓G���[�ɂȂ�܂��B
        /// </summary>
        /// <typeparam name="T">�擾�������C���X�^���X�̌^�B</typeparam>
        /// <returns>�擾�����C���X�^���X��Ԃ��܂��B�擾�ł��Ȃ������ꍇ�� null ��Ԃ��܂��B</returns>
        public static T GetInstance<T>() where T : class
        {
            var type = typeof(T);

            if (instances.ContainsKey(type)) {
                return instances[type] as T;
            }

            Debugger.LogError($"�v�����ꂽ�^�̃C���X�^���X���o�^����Ă��܂���F{type.Name}");
            return null;
        }

        /// <summary>
        /// �C���X�^���X���擾���A�n���ꂽ�����ɑ�����܂��B�擾�ł��Ȃ������ꍇ�� null ������܂��B
        /// </summary>
        /// <param name="instance">�擾�����C���X�^���X������ϐ��B</param>
        /// <typeparam name="T">�擾�������C���X�^���X�̌^�B</typeparam>
        /// <returns>�擾������������ true ��Ԃ��܂��B</returns>
        public static bool TryGetInstance<T>(out T instance) where T : class
        {
            var type = typeof(T);

            instance = instances.ContainsKey(type) ? instances[type] as T : null;

            return instance != null;
        }
    }
}
