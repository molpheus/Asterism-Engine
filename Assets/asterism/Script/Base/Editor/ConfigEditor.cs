using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;
#pragma warning disable CS0234 // 型または名前空間の名前 'UIElements' が名前空間 'UnityEditor' に存在しません (アセンブリ参照があることを確認してください)
using UnityEditor.UIElements;
#pragma warning restore CS0234 // 型または名前空間の名前 'UIElements' が名前空間 'UnityEditor' に存在しません (アセンブリ参照があることを確認してください)

namespace Asterism.Common
{
#pragma warning disable CS0246 // 型または名前空間の名前 'EditorWindow' が見つかりませんでした (using ディレクティブまたはアセンブリ参照が指定されていることを確認してください)
    public class ConfigEditor : EditorWindow
#pragma warning restore CS0246 // 型または名前空間の名前 'EditorWindow' が見つかりませんでした (using ディレクティブまたはアセンブリ参照が指定されていることを確認してください)
    {
        [SerializeField] private ConfigObject data; // データ

#pragma warning disable CS0246 // 型または名前空間の名前 'MenuItem' が見つかりませんでした (using ディレクティブまたはアセンブリ参照が指定されていることを確認してください)
#pragma warning disable CS0246 // 型または名前空間の名前 'MenuItemAttribute' が見つかりませんでした (using ディレクティブまたはアセンブリ参照が指定されていることを確認してください)
        [MenuItem("Config/App")]
#pragma warning restore CS0246 // 型または名前空間の名前 'MenuItemAttribute' が見つかりませんでした (using ディレクティブまたはアセンブリ参照が指定されていることを確認してください)
#pragma warning restore CS0246 // 型または名前空間の名前 'MenuItem' が見つかりませんでした (using ディレクティブまたはアセンブリ参照が指定されていることを確認してください)
        static void Open()
        {
#pragma warning disable CS0103 // 現在のコンテキストに 'GetWindow' という名前は存在しません
            var wnd = GetWindow<ConfigEditor>();
#pragma warning restore CS0103 // 現在のコンテキストに 'GetWindow' という名前は存在しません
            wnd.titleContent = new GUIContent("システム設定");
        }

        private void OnEnable()
        {
            if (this.data == null) { this.data = LoadData(); }

#pragma warning disable CS0103 // 現在のコンテキストに 'rootVisualElement' という名前は存在しません
            var root = rootVisualElement;
#pragma warning restore CS0103 // 現在のコンテキストに 'rootVisualElement' という名前は存在しません
            //var visualTree = LoadDataTree();
            //visualTree.CloneTree(root);

#pragma warning disable CS0246 // 型または名前空間の名前 'InspectorElement' が見つかりませんでした (using ディレクティブまたはアセンブリ参照が指定されていることを確認してください)
            var inspector = new InspectorElement(this.data);
#pragma warning restore CS0246 // 型または名前空間の名前 'InspectorElement' が見つかりませんでした (using ディレクティブまたはアセンブリ参照が指定されていることを確認してください)
            root.Add(inspector);
        }

        /// <summary>
        /// ウィンドウの描画
        /// </summary>
        void OnGUI()
        {
#pragma warning disable CS0103 // 現在のコンテキストに 'EditorGUI' という名前は存在しません
            EditorGUI.BeginChangeCheck();
#pragma warning restore CS0103 // 現在のコンテキストに 'EditorGUI' という名前は存在しません

            //EditorGUILayout.HelpBox("システム設定", MessageType.Info);

            //EditorGUILayout.

            //data.AddressablePath = EditorGUILayout.TextField(data.AddressablePath);

            //this.data.ViewGraphy = EditorGUILayout.Toggle("GRAPHYを表示する", this.data.ViewGraphy);

            //string text = this.data.Text; // ScriptableObjectからテキストを取り出す
            //text = EditorGUILayout.TextField("テキスト", text); // テキスト入力

#pragma warning disable CS0103 // 現在のコンテキストに 'EditorGUI' という名前は存在しません
            if (EditorGUI.EndChangeCheck()) // テキストが変更された場合は
            {
                //this.data.Text = text; // データへテキストを保存する
#pragma warning disable CS0103 // 現在のコンテキストに 'EditorUtility' という名前は存在しません
                EditorUtility.SetDirty(this.data); // データの変更をUnityに教える
#pragma warning restore CS0103 // 現在のコンテキストに 'EditorUtility' という名前は存在しません
            }
#pragma warning restore CS0103 // 現在のコンテキストに 'EditorGUI' という名前は存在しません

        }

        /// <summary>
        /// データのロード
        /// </summary>
        static ConfigObject LoadData()
        {
#pragma warning disable CS0103 // 現在のコンテキストに 'AssetDatabase' という名前は存在しません
#pragma warning disable CS0103 // 現在のコンテキストに 'AssetDatabase' という名前は存在しません
#pragma warning disable CS0103 // 現在のコンテキストに 'AssetDatabase' という名前は存在しません
            return (ConfigObject)AssetDatabase.FindAssets("t:ScriptableObject") // プロジェクトに存在する全ScriptableObjectのGUIDを取得
               .Select(guid => AssetDatabase.GUIDToAssetPath(guid)) // GUIDをパスに変換
               .Select(path => AssetDatabase.LoadAssetAtPath(path, typeof(ConfigObject))) // パスからPermanentDataの取得を試みる
               .Where(obj => obj != null) // null要素は取り除く
               .FirstOrDefault(); // 取得したPermanentDataのうち、最初の一つだけを取り出す
#pragma warning restore CS0103 // 現在のコンテキストに 'AssetDatabase' という名前は存在しません
#pragma warning restore CS0103 // 現在のコンテキストに 'AssetDatabase' という名前は存在しません
#pragma warning restore CS0103 // 現在のコンテキストに 'AssetDatabase' という名前は存在しません
        }

        static VisualTreeAsset LoadDataTree()
        {
#pragma warning disable CS0103 // 現在のコンテキストに 'AssetDatabase' という名前は存在しません
#pragma warning disable CS0103 // 現在のコンテキストに 'AssetDatabase' という名前は存在しません
#pragma warning disable CS0103 // 現在のコンテキストに 'AssetDatabase' という名前は存在しません
            return (VisualTreeAsset)AssetDatabase.FindAssets("t:VisualTreeAsset") // プロジェクトに存在する全ScriptableObjectのGUIDを取得
               .Select(guid => AssetDatabase.GUIDToAssetPath(guid)) // GUIDをパスに変換
               .Where(path => path.IndexOf("Asterism/Script/UI/Config/config.uxml") >= 0)
               .Select(path => AssetDatabase.LoadAssetAtPath(path, typeof(VisualTreeAsset))) // パスからPermanentDataの取得を試みる
               .Where(obj => obj != null) // null要素は取り除く
               .FirstOrDefault(); // 取得したPermanentDataのうち、最初の一つだけを取り出す
#pragma warning restore CS0103 // 現在のコンテキストに 'AssetDatabase' という名前は存在しません
#pragma warning restore CS0103 // 現在のコンテキストに 'AssetDatabase' という名前は存在しません
#pragma warning restore CS0103 // 現在のコンテキストに 'AssetDatabase' という名前は存在しません
        }
    }
}