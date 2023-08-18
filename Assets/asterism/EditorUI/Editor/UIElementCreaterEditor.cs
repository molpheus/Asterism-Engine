using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using System.IO;
using Asterism;
using System;
using System.Text;
using Codice.CM.SEIDInfo;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using System.Collections;
using static UnityEditor.AddressableAssets.Build.Layout.BuildLayout;
using Codice.CM.Client.Differences;
using Unity.VisualScripting;
using System.Linq;
using static Codice.CM.Common.Serialization.PacketFileReader;
using UnityEditor.PackageManager;

namespace Asterism.UI
{
    public class UIElementCreaterEditor : EditorWindow
    {
        private const string EDITOR_UIELEMENT_GUID = "ed1017f7acebf214f99b89316148b026";

        private const string EXPORT_FILE_FORMAT = "{0}.binding.cs";
        private const string EXPORT_TEMPLATE_FILE_GUID = "9dffdaa6efd3e4943908ba372faa0bb9";
        
        private const string EXPORT_SAVE_FILE = "{0}.save.txt";


        /// <summary>
        /// ウィンドウを開く
        /// </summary>
        [MenuItem("Assets/Asterism/CreateUIElementScript")]
        private static void OpenWindow()
        {
            var window = GetWindow<UIElementCreaterEditor>();
            window.Show();
        }

        /// <summary>
        /// 変数のアクセス設定
        /// </summary>
        public enum VariableType
        {
            PUBLIC,
            PRIVATE,
            PROTECTED,
            PRIVATE_SerializeField,
            PROTECTED_SerializeField,
            PUBLIC_READONLY,
            PRIVATE_READONLY,
            PROTECTED_READONLY,
        }

        /// <summary>
        /// 保存するデータのチェック用
        /// </summary>
        [Serializable]
        public class CheckItemListContent
        {
            public bool check;
            public string path;
            public string type;
            public string variable;

            public VariableType variableType;

            public string[] pathList;
            public VisualElement element { get; set; }


            public CheckItemListContent(string path, VisualElement element, string[] pathList)
            {
                this.check = true;
                this.path = path;
                this.variable = "";
                this.pathList = pathList;
                this.element = element;
                this.type = element.GetType().Name;
            }

            public void UpdateText(ChangeEvent<string> value)
            {
                variable = value.newValue;
            }

            public void UpdateVariavleType(ChangeEvent<Enum> changeType)
            {
                if (changeType.newValue is VariableType value)
                {
                    variableType = value;
                }
            }
        }

        /// <summary>
        /// 保存機能
        /// </summary>
        [Serializable]
        public class Save
        {
            public string SelectItemPath;
            public string ExportDirectory;
            public string ExportFileName;

            public List<CheckItemListContent> checkList;

            public void UpdateSelectItemPath(string value)
                => SelectItemPath = value;

            public void UpdateExportDIrectoryPath(string value)
                => ExportDirectory = value;

            public void UpdateExportFileName(string value)
                => ExportFileName = value;
        }

        /// <summary> セーブ用データ </summary>
        private Save _saveData;
        /// <summary> 表示用UI </summary>
        VisualTreeAsset _visualTree;

        VisualElement _selectWindow;

        TextField _selectItemField;
        TextField _outputItemField;
        TextField _outputFileNameField;
        Label _outputExportFileName;

        Dictionary<string[], VisualElement> _elementList;

        private void OnEnable()
        {
            var filePath = AssetDatabase.GUIDToAssetPath(EDITOR_UIELEMENT_GUID);
            _visualTree = AssetDatabase.LoadAssetAtPath(filePath, typeof(VisualTreeAsset)) as VisualTreeAsset;
            _visualTree.CloneTree(rootVisualElement);

            _selectItemField = rootVisualElement.Q<TextField>("SelectItemField");
            _outputItemField = rootVisualElement.Q<TextField>("OutputFilePathField");
            _outputFileNameField = rootVisualElement.Q<TextField>("OutputFileNameField");
            _selectWindow = rootVisualElement.Q("SelectWindow");

            // 確認ボタンが押されたときの処理
            rootVisualElement.Q<Button>("CheckButton").clicked += CreateElement;
            // フォルダ選択ボタンが押されたときの処理
            rootVisualElement.Q<Button>("OutputFilePathButton").clicked += () => {
                var path = EditorUtility.OpenFolderPanel("Select Folder", Application.dataPath, string.Empty);
                _outputItemField.value = path;
                _saveData.ExportDirectory = path;
            };
            // 吐き出しボタンが押されたときの処理
            rootVisualElement.Q<Button>("ExportButton").clicked += Export;
            // スクリプトのアタッチボタンが押されたときの処理
            rootVisualElement.Q<Button>("AttachButton").clicked += AttachScript;

            //try
            //{
                var selectObject = Selection.objects[0];
                if (selectObject is TextAsset textFile)
                {
                    LoadSave(textFile.text);
                }
                else if (selectObject is VisualTreeAsset)
                {
                    var objectPath = AssetDatabase.GetAssetPath(selectObject);
                    _saveData = new();
                    _saveData.checkList = new();
                    _selectItemField.value = AssetDatabase.GetAssetPath(selectObject);
                    _outputItemField.value = Application.dataPath;
                    _outputFileNameField.value = Path.GetFileNameWithoutExtension(objectPath);
                    _saveData.SelectItemPath = _selectItemField.value;
                    _saveData.ExportDirectory = _outputItemField.value;
                    _saveData.ExportFileName = _outputFileNameField.value;
                }
                else
                {
                    throw new Exception();
                }

                _selectItemField.RegisterValueChangedCallback(_ => { _saveData.SelectItemPath = _.newValue; });
                _outputItemField.RegisterValueChangedCallback(_ => { _saveData.ExportDirectory = _.newValue; });
                _outputFileNameField.RegisterValueChangedCallback(_ => {
                    _saveData.ExportFileName = _.newValue;
                    _outputExportFileName.text = string.Format(EXPORT_FILE_FORMAT, _.newValue);
                });

                // 出力する予定のファイル名を表示するラベル
                _outputExportFileName = rootVisualElement.Q<Label>("CheckOutputFileNameLabel");
                _outputExportFileName.text = string.Format(EXPORT_FILE_FORMAT, _outputFileNameField.value);
            //}
            //catch(Exception e)
            //{

            //    EditorUtility.DisplayDialog(e.Message, "オブジェクト(uxml, saveファイル)が選択されていないため終了します", "OK");
            //}
        }

        /// <summary>
        /// MultiListに表示するアイテムを生成する
        /// </summary>
        private void CreateElement()
        {
            VisualElement baseElement = new VisualElement();
            var visualTree = AssetDatabase.LoadAssetAtPath(_selectItemField.value, typeof(VisualTreeAsset)) as VisualTreeAsset;
            visualTree.CloneTree(baseElement);

            Dictionary<string[], VisualElement> elementList = new Dictionary<string[], VisualElement>();
            Search(baseElement, ref elementList);
            _elementList = elementList;

            ViewCheckItems();
        }

        /// <summary>
        /// UXMLの内容を検索する再帰処理
        /// </summary>
        /// <param name="baseElement"> Parentとなるエレメント </param>
        /// <param name="elementList"> エレメントのリスト </param>
        private void Search(VisualElement baseElement,　ref Dictionary<string[], VisualElement> elementList)
        {
            foreach (var element in baseElement.Children())
            {
                if (GetElement(element, ref elementList))
                {
                    Search(element, ref elementList);
                }
            }
        }

        /// <summary>
        /// 現在選択されたエレメントが上げているコンテンツに引っかかるかのチェック
        /// </summary>
        /// <param name="element"> 確認対象のエレメント </param>
        /// <param name="elementList"> エレメントのリスト </param>
        /// <returns></returns>
        private bool GetElement(VisualElement element, ref Dictionary<string[], VisualElement> elementList)
        {
            var isUIElement = false;
            var isContent = element switch {
                Label => true,
                Button => true,
                Toggle => true,
                Scroller => true,
                TextField => true,
                Foldout => true,
                Slider => true,
                SliderInt => true,
                MinMaxSlider => true,
                ProgressBar => true,
                DropdownField => true,
                EnumField => true,
                RadioButton => true,
                RadioButtonGroup => true,
                _ => false
            };

            if (isContent)
            {
                bool isEnd = false;
                var parent = element.parent;
                var pathList = new List<string>();
                pathList.Add(element.name);
                while (!isEnd)
                {
                    if (parent is not null)
                    {
                        pathList.Add(parent.name);
                        parent = parent.parent;
                    }

                    isEnd = parent == null;
                }
                pathList.Reverse();

                elementList.Add(pathList.ToArray(), element);
            }
            else
            {
                isUIElement = true;
            }

            return isUIElement;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        private UIElement CreateUIElement(VisualElement element)
        {
            return element switch
            {
                Label => new UIElementLabel(),
                Button => new UIElementButton(),
                Toggle => new UIElementToggle(),
                Scroller => new UIElementScroller(),
                TextField => new UIElementTextField(),
                //Foldout => ,
                Slider => new UIElementSlider(),
                SliderInt => new UIElementSliderInt(),
                MinMaxSlider => new UIElementMinMaxSlider(),
                ProgressBar => new UIElementProgressBar(),
                DropdownField => new UIElementDropdown(),
                EnumField => new UIElementEnum(),
                RadioButton => new UIElementRadioButton(),
                RadioButtonGroup => new UIElementRadioButtonGroup(),
                _ => null
            };
        }

        private void ViewCheckItems()
        {
            _selectWindow.visible = _elementList.Count > 0;

            if (_selectWindow.visible)
            {
                var selectContent = rootVisualElement.Q<MultiColumnListView>("SelectContent");

                selectContent.Clear();

                var saveList = new List<CheckItemListContent>(_saveData.checkList);

                _saveData.checkList.Clear();
                foreach (var e in _elementList)
                {
                    var label = "";
                    foreach (var e2 in e.Key)
                    {
                        if (string.IsNullOrEmpty(e2)) continue;
                        if (!string.IsNullOrEmpty(label))
                            label += " < ";
                        label += e2;
                    }
                    var content = saveList.Any(e => e.path == label)
                        ? saveList.First(e => e.path == label)
                        : new CheckItemListContent(label, e.Value, e.Key);

                    content.element = e.Value;
                    _saveData.checkList.Add( content );
                }

                selectContent.itemsSource = _saveData.checkList;
                var checkboxColumn = selectContent.columns["check"];
                var pathColumn = selectContent.columns["path"];
                var typeColumn = selectContent.columns["type"];
                var variableTypeColumn = selectContent.columns["variable_type"];
                var variableColumn = selectContent.columns["variable"];

                // レイアウトを作成する処理
                checkboxColumn.makeCell = () => new Toggle();
                pathColumn.makeCell = () => new Label();
                typeColumn.makeCell = () => new Label();
                variableTypeColumn.makeCell = () => new EnumField(VariableType.PUBLIC);
                variableColumn.makeCell = () => new TextField();

                // 内容を設定する処理
                checkboxColumn.bindCell = (e, i) => (e as Toggle).value = _saveData.checkList[i].check;
                pathColumn.bindCell = (e, i) => (e as Label).text = _saveData.checkList[i].path;
                typeColumn.bindCell = (e, i) => (e as Label).text = _saveData.checkList[i].type;
                variableTypeColumn.bindCell = (e, i) => {
                    (e as EnumField).value = _saveData.checkList[i].variableType;
                    (e as EnumField).RegisterValueChangedCallback<Enum>(_saveData.checkList[i].UpdateVariavleType);
                };
                variableColumn.bindCell = (e, i) => {
                    (e as TextField).value = _saveData.checkList[i].variable;
                    (e as TextField).RegisterValueChangedCallback<string>(_saveData.checkList[i].UpdateText);
                };

                checkboxColumn.visible = false;
            }
        }

        private void Export()
        {
            var tabSpace = "    ";

            var filePath = AssetDatabase.GUIDToAssetPath(EXPORT_TEMPLATE_FILE_GUID);
            var fileData = System.IO.File.ReadAllText(filePath);

            fileData = fileData.Replace("{CLASSNAME}", _outputFileNameField.value);

            var content = new StringBuilder();
            foreach (var item in _saveData.checkList)
            {
                if (string.IsNullOrEmpty(item.variable)) continue;

                var variableType = item.variableType switch {
                    VariableType.PUBLIC => "public",
                    VariableType.PRIVATE => "private",
                    VariableType.PROTECTED => "protected",
                    VariableType.PRIVATE_SerializeField => "[UnityEngine.SerializeField] private",
                    VariableType.PROTECTED_SerializeField => "[UnityEngine.SerializeField] protected",
                    VariableType.PUBLIC_READONLY => "public readonly",
                    VariableType.PRIVATE_READONLY => "private readonly",
                    VariableType.PROTECTED_READONLY => "protected readonly",
                    _ => ""
                };

                content.AppendLine($"{tabSpace}{variableType} {CreateUIElement(item.element).GetType().Name} {item.variable} = new() {{");
                content.AppendLine($"{tabSpace}{tabSpace}TagNameList = new[] {{ {StringArrWithOpen(item.pathList)} }}");
                content.AppendLine($"{tabSpace}}};");
            }
            var contentStr = content.ToString();
            fileData = fileData.Replace("{CONTENT}", contentStr);

            System.IO.File.WriteAllText(
                Path.Combine(
                    _outputItemField.value,
                    string.Format(EXPORT_FILE_FORMAT, _outputFileNameField.value)
                ),
                fileData
            );

            var assetPath = _outputItemField.value.Replace(Application.dataPath, "Assets");
            var prefabPath = Path.Combine( assetPath, string.Format("{0}.prefab", _outputFileNameField.value) );
            try
            {
                var prefab = PrefabUtility.LoadPrefabContents(prefabPath);
                PrefabUtility.UnloadPrefabContents(prefab);
                AttachScript();
            }
            catch
            {
                var generateAssetPath = AssetDatabase.GenerateUniqueAssetPath(prefabPath);
                var prefab = new GameObject(_outputFileNameField.value);

                var uiDocument = prefab.AddComponent<UIDocument>();
                uiDocument.visualTreeAsset = _visualTree;

                PrefabUtility.SaveAsPrefabAssetAndConnect(prefab, generateAssetPath, InteractionMode.AutomatedAction);
            }
            ExportSave();

            AssetDatabase.Refresh();
        }

        private string StringArrWithOpen(string[] list)
        {
            var str = "";
            foreach(var item in list)
            {
                if (string.IsNullOrEmpty(item)) continue;

                if (!string.IsNullOrEmpty(str))
                    str += ",";

                str += $"\"{item}\"";
            }
            return str;
        }


        private void AttachScript()
        {
            try
            {
                var assetPath = _outputItemField.value.Replace(Application.dataPath, "Assets");
                var prefabPath = Path.Combine( assetPath, string.Format("{0}.prefab", _outputFileNameField.value) );
                if (!System.IO.File.Exists(prefabPath))
                {
                    
                    throw new Exception("対象になるプレハブが生成されていません");
                }

                var scriptPath = Path.Combine( assetPath, string.Format(EXPORT_FILE_FORMAT, _outputFileNameField.value) );
                if (!System.IO.File.Exists(scriptPath))
                {
                    throw new Exception("対象になるスクリプトが生成されていません");
                }

                var getType = Type.GetType($"{_outputFileNameField.value}, Assembly-CSharp");
                if (getType is null)
                {
                    throw new Exception("Domainのリロードをお待ち下さい");
                }

                var prefab = PrefabUtility.LoadPrefabContents(prefabPath);

                if (!prefab.TryGetComponent(getType, out var component))
                {
                    prefab.AddComponent(getType);
                }

                PrefabUtility.SaveAsPrefabAsset(prefab, prefabPath);
            }
            catch(Exception ex)
            {
                EditorUtility.DisplayDialog("", ex.Message, "OK");
            }
        }

        private void ExportSave()
        {
            System.IO.File.WriteAllText(
                Path.Combine(
                    _outputItemField.value,
                    string.Format(EXPORT_SAVE_FILE, _outputFileNameField.value)
                ),
                JsonUtility.ToJson(_saveData)
            );
        }
        
        private void LoadSave(string text)
        {
            _saveData = JsonUtility.FromJson<Save>(text);
            _selectItemField.value = _saveData.SelectItemPath;
            _outputItemField.value = _saveData.ExportDirectory;
            _outputFileNameField.value = _saveData.ExportFileName;

            CreateElement();
        }

    }
}
