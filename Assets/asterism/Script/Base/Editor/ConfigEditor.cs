using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;
#pragma warning disable CS0234 // �^�܂��͖��O��Ԃ̖��O 'UIElements' �����O��� 'UnityEditor' �ɑ��݂��܂��� (�A�Z���u���Q�Ƃ����邱�Ƃ��m�F���Ă�������)
using UnityEditor.UIElements;
#pragma warning restore CS0234 // �^�܂��͖��O��Ԃ̖��O 'UIElements' �����O��� 'UnityEditor' �ɑ��݂��܂��� (�A�Z���u���Q�Ƃ����邱�Ƃ��m�F���Ă�������)

namespace Asterism.Common
{
#pragma warning disable CS0246 // �^�܂��͖��O��Ԃ̖��O 'EditorWindow' ��������܂���ł��� (using �f�B���N�e�B�u�܂��̓A�Z���u���Q�Ƃ��w�肳��Ă��邱�Ƃ��m�F���Ă�������)
    public class ConfigEditor : EditorWindow
#pragma warning restore CS0246 // �^�܂��͖��O��Ԃ̖��O 'EditorWindow' ��������܂���ł��� (using �f�B���N�e�B�u�܂��̓A�Z���u���Q�Ƃ��w�肳��Ă��邱�Ƃ��m�F���Ă�������)
    {
        [SerializeField] private ConfigObject data; // �f�[�^

#pragma warning disable CS0246 // �^�܂��͖��O��Ԃ̖��O 'MenuItem' ��������܂���ł��� (using �f�B���N�e�B�u�܂��̓A�Z���u���Q�Ƃ��w�肳��Ă��邱�Ƃ��m�F���Ă�������)
#pragma warning disable CS0246 // �^�܂��͖��O��Ԃ̖��O 'MenuItemAttribute' ��������܂���ł��� (using �f�B���N�e�B�u�܂��̓A�Z���u���Q�Ƃ��w�肳��Ă��邱�Ƃ��m�F���Ă�������)
        [MenuItem("Config/App")]
#pragma warning restore CS0246 // �^�܂��͖��O��Ԃ̖��O 'MenuItemAttribute' ��������܂���ł��� (using �f�B���N�e�B�u�܂��̓A�Z���u���Q�Ƃ��w�肳��Ă��邱�Ƃ��m�F���Ă�������)
#pragma warning restore CS0246 // �^�܂��͖��O��Ԃ̖��O 'MenuItem' ��������܂���ł��� (using �f�B���N�e�B�u�܂��̓A�Z���u���Q�Ƃ��w�肳��Ă��邱�Ƃ��m�F���Ă�������)
        static void Open()
        {
#pragma warning disable CS0103 // ���݂̃R���e�L�X�g�� 'GetWindow' �Ƃ������O�͑��݂��܂���
            var wnd = GetWindow<ConfigEditor>();
#pragma warning restore CS0103 // ���݂̃R���e�L�X�g�� 'GetWindow' �Ƃ������O�͑��݂��܂���
            wnd.titleContent = new GUIContent("�V�X�e���ݒ�");
        }

        private void OnEnable()
        {
            if (this.data == null) { this.data = LoadData(); }

#pragma warning disable CS0103 // ���݂̃R���e�L�X�g�� 'rootVisualElement' �Ƃ������O�͑��݂��܂���
            var root = rootVisualElement;
#pragma warning restore CS0103 // ���݂̃R���e�L�X�g�� 'rootVisualElement' �Ƃ������O�͑��݂��܂���
            //var visualTree = LoadDataTree();
            //visualTree.CloneTree(root);

#pragma warning disable CS0246 // �^�܂��͖��O��Ԃ̖��O 'InspectorElement' ��������܂���ł��� (using �f�B���N�e�B�u�܂��̓A�Z���u���Q�Ƃ��w�肳��Ă��邱�Ƃ��m�F���Ă�������)
            var inspector = new InspectorElement(this.data);
#pragma warning restore CS0246 // �^�܂��͖��O��Ԃ̖��O 'InspectorElement' ��������܂���ł��� (using �f�B���N�e�B�u�܂��̓A�Z���u���Q�Ƃ��w�肳��Ă��邱�Ƃ��m�F���Ă�������)
            root.Add(inspector);
        }

        /// <summary>
        /// �E�B���h�E�̕`��
        /// </summary>
        void OnGUI()
        {
#pragma warning disable CS0103 // ���݂̃R���e�L�X�g�� 'EditorGUI' �Ƃ������O�͑��݂��܂���
            EditorGUI.BeginChangeCheck();
#pragma warning restore CS0103 // ���݂̃R���e�L�X�g�� 'EditorGUI' �Ƃ������O�͑��݂��܂���

            //EditorGUILayout.HelpBox("�V�X�e���ݒ�", MessageType.Info);

            //EditorGUILayout.

            //data.AddressablePath = EditorGUILayout.TextField(data.AddressablePath);

            //this.data.ViewGraphy = EditorGUILayout.Toggle("GRAPHY��\������", this.data.ViewGraphy);

            //string text = this.data.Text; // ScriptableObject����e�L�X�g�����o��
            //text = EditorGUILayout.TextField("�e�L�X�g", text); // �e�L�X�g����

#pragma warning disable CS0103 // ���݂̃R���e�L�X�g�� 'EditorGUI' �Ƃ������O�͑��݂��܂���
            if (EditorGUI.EndChangeCheck()) // �e�L�X�g���ύX���ꂽ�ꍇ��
            {
                //this.data.Text = text; // �f�[�^�փe�L�X�g��ۑ�����
#pragma warning disable CS0103 // ���݂̃R���e�L�X�g�� 'EditorUtility' �Ƃ������O�͑��݂��܂���
                EditorUtility.SetDirty(this.data); // �f�[�^�̕ύX��Unity�ɋ�����
#pragma warning restore CS0103 // ���݂̃R���e�L�X�g�� 'EditorUtility' �Ƃ������O�͑��݂��܂���
            }
#pragma warning restore CS0103 // ���݂̃R���e�L�X�g�� 'EditorGUI' �Ƃ������O�͑��݂��܂���

        }

        /// <summary>
        /// �f�[�^�̃��[�h
        /// </summary>
        static ConfigObject LoadData()
        {
#pragma warning disable CS0103 // ���݂̃R���e�L�X�g�� 'AssetDatabase' �Ƃ������O�͑��݂��܂���
#pragma warning disable CS0103 // ���݂̃R���e�L�X�g�� 'AssetDatabase' �Ƃ������O�͑��݂��܂���
#pragma warning disable CS0103 // ���݂̃R���e�L�X�g�� 'AssetDatabase' �Ƃ������O�͑��݂��܂���
            return (ConfigObject)AssetDatabase.FindAssets("t:ScriptableObject") // �v���W�F�N�g�ɑ��݂���SScriptableObject��GUID���擾
               .Select(guid => AssetDatabase.GUIDToAssetPath(guid)) // GUID���p�X�ɕϊ�
               .Select(path => AssetDatabase.LoadAssetAtPath(path, typeof(ConfigObject))) // �p�X����PermanentData�̎擾�����݂�
               .Where(obj => obj != null) // null�v�f�͎�菜��
               .FirstOrDefault(); // �擾����PermanentData�̂����A�ŏ��̈���������o��
#pragma warning restore CS0103 // ���݂̃R���e�L�X�g�� 'AssetDatabase' �Ƃ������O�͑��݂��܂���
#pragma warning restore CS0103 // ���݂̃R���e�L�X�g�� 'AssetDatabase' �Ƃ������O�͑��݂��܂���
#pragma warning restore CS0103 // ���݂̃R���e�L�X�g�� 'AssetDatabase' �Ƃ������O�͑��݂��܂���
        }

        static VisualTreeAsset LoadDataTree()
        {
#pragma warning disable CS0103 // ���݂̃R���e�L�X�g�� 'AssetDatabase' �Ƃ������O�͑��݂��܂���
#pragma warning disable CS0103 // ���݂̃R���e�L�X�g�� 'AssetDatabase' �Ƃ������O�͑��݂��܂���
#pragma warning disable CS0103 // ���݂̃R���e�L�X�g�� 'AssetDatabase' �Ƃ������O�͑��݂��܂���
            return (VisualTreeAsset)AssetDatabase.FindAssets("t:VisualTreeAsset") // �v���W�F�N�g�ɑ��݂���SScriptableObject��GUID���擾
               .Select(guid => AssetDatabase.GUIDToAssetPath(guid)) // GUID���p�X�ɕϊ�
               .Where(path => path.IndexOf("Asterism/Script/UI/Config/config.uxml") >= 0)
               .Select(path => AssetDatabase.LoadAssetAtPath(path, typeof(VisualTreeAsset))) // �p�X����PermanentData�̎擾�����݂�
               .Where(obj => obj != null) // null�v�f�͎�菜��
               .FirstOrDefault(); // �擾����PermanentData�̂����A�ŏ��̈���������o��
#pragma warning restore CS0103 // ���݂̃R���e�L�X�g�� 'AssetDatabase' �Ƃ������O�͑��݂��܂���
#pragma warning restore CS0103 // ���݂̃R���e�L�X�g�� 'AssetDatabase' �Ƃ������O�͑��݂��܂���
#pragma warning restore CS0103 // ���݂̃R���e�L�X�g�� 'AssetDatabase' �Ƃ������O�͑��݂��܂���
        }
    }
}