using System.Linq;

using UnityEditor;
using UnityEditor.UIElements;

using UnityEngine;
using UnityEngine.UIElements;

namespace Asterism.Common
{
    public class ConfigEditor : EditorWindow
    {
        [SerializeField] private ConfigObject _data; // �f�[�^

        [MenuItem("Config/App")]
        static void Open()
        {
            var wnd = GetWindow<ConfigEditor>();
            wnd.titleContent = new GUIContent("�V�X�e���ݒ�");
        }

        private void OnEnable()
        {
            if (this._data == null) { this._data = LoadData(); }

            var root = rootVisualElement;
            //var visualTree = LoadDataTree();
            //visualTree.CloneTree(root);

            var inspector = new InspectorElement(this._data);
            root.Add(inspector);
        }

        /// <summary>
        /// �E�B���h�E�̕`��
        /// </summary>
        void OnGUI()
        {
            EditorGUI.BeginChangeCheck();

            if (EditorGUI.EndChangeCheck()) // �e�L�X�g���ύX���ꂽ�ꍇ��
            {
                //this.data.Text = text; // �f�[�^�փe�L�X�g��ۑ�����
                EditorUtility.SetDirty(this._data); // �f�[�^�̕ύX��Unity�ɋ�����
            }
        }

        /// <summary>
        /// �f�[�^�̃��[�h
        /// </summary>
        static ConfigObject LoadData()
        {
            return (ConfigObject)AssetDatabase.FindAssets("t:ScriptableObject") // �v���W�F�N�g�ɑ��݂���SScriptableObject��GUID���擾
               .Select(guid => AssetDatabase.GUIDToAssetPath(guid)) // GUID���p�X�ɕϊ�
               .Select(path => AssetDatabase.LoadAssetAtPath(path, typeof(ConfigObject))) // �p�X����PermanentData�̎擾�����݂�
               .Where(obj => obj != null) // null�v�f�͎�菜��
               .FirstOrDefault(); // �擾����PermanentData�̂����A�ŏ��̈���������o��
        }

        static VisualTreeAsset LoadDataTree()
        {
            return (VisualTreeAsset)AssetDatabase.FindAssets("t:VisualTreeAsset") // �v���W�F�N�g�ɑ��݂���SScriptableObject��GUID���擾
               .Select(guid => AssetDatabase.GUIDToAssetPath(guid)) // GUID���p�X�ɕϊ�
               .Where(path => path.IndexOf("Asterism/Script/UI/Config/config.uxml") >= 0)
               .Select(path => AssetDatabase.LoadAssetAtPath(path, typeof(VisualTreeAsset))) // �p�X����PermanentData�̎擾�����݂�
               .Where(obj => obj != null) // null�v�f�͎�菜��
               .FirstOrDefault(); // �擾����PermanentData�̂����A�ŏ��̈���������o��
        }
    }
}