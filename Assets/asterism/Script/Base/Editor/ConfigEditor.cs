using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;
using UnityEditor.UIElements;

namespace Asterism.Common
{
    public class ConfigEditor : EditorWindow
    {
        [SerializeField] private ConfigObject _data; // データ

        [MenuItem("Config/App")]
        static void Open()
        {
            var wnd = GetWindow<ConfigEditor>();
            wnd.titleContent = new GUIContent("システム設定");
        }

        private void OnEnable()
        {
            if (this._data == null) { this._data = LoadData(); }

            var root = rootVisualElement;

            var inspector = new InspectorElement(this._data);
            root.Add(inspector);
        }

        /// <summary>
        /// ウィンドウの描画
        /// </summary>
        void OnGUI()
        {
            EditorGUI.BeginChangeCheck();

            if (EditorGUI.EndChangeCheck()) // テキストが変更された場合は
            {
                //this.data.Text = text; // データへテキストを保存する
                EditorUtility.SetDirty(this._data); // データの変更をUnityに教える
            }
        }

        /// <summary>
        /// データのロード
        /// </summary>
        static ConfigObject LoadData()
        {
            return (ConfigObject)AssetDatabase.FindAssets("t:ScriptableObject") // プロジェクトに存在する全ScriptableObjectのGUIDを取得
               .Select(guid => AssetDatabase.GUIDToAssetPath(guid)) // GUIDをパスに変換
               .Select(path => AssetDatabase.LoadAssetAtPath(path, typeof(ConfigObject))) // パスからPermanentDataの取得を試みる
               .Where(obj => obj != null) // null要素は取り除く
               .FirstOrDefault(); // 取得したPermanentDataのうち、最初の一つだけを取り出す
        }

        static VisualTreeAsset LoadDataTree()
        {
            return (VisualTreeAsset)AssetDatabase.FindAssets("t:VisualTreeAsset") // プロジェクトに存在する全ScriptableObjectのGUIDを取得
               .Select(guid => AssetDatabase.GUIDToAssetPath(guid)) // GUIDをパスに変換
               .Where(path => path.IndexOf("Asterism/Script/UI/Config/config.uxml") >= 0)
               .Select(path => AssetDatabase.LoadAssetAtPath(path, typeof(VisualTreeAsset))) // パスからPermanentDataの取得を試みる
               .Where(obj => obj != null) // null要素は取り除く
               .FirstOrDefault(); // 取得したPermanentDataのうち、最初の一つだけを取り出す
        }
    }
}