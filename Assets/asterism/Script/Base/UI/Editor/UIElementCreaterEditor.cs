using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using System.IO;

namespace Asterism.UI
{
    public class UIElementCreaterEditor : EditorWindow
    {
        private const string EDITOR_UIELEMENT_GUID = "ed1017f7acebf214f99b89316148b026";
        private const string EXPORT_FILE_FORMAT = "{0}.binding.cs";

        [MenuItem("Assets/Asterism/CreateUIElementScript")]
        private static void OpenWindow()
        {
            var window = GetWindow<UIElementCreaterEditor>();
            window.Show();
        }

        TextField _selectItemField;
        TextField _outputItemField;
        TextField _outputFileNameField;
        Label _outputExportFileName;

        private void OnEnable()
        {
            var filePath = AssetDatabase.GUIDToAssetPath(EDITOR_UIELEMENT_GUID);
            var visualTree = AssetDatabase.LoadAssetAtPath(filePath, typeof(VisualTreeAsset)) as VisualTreeAsset;

            visualTree.CloneTree(rootVisualElement);

            var button = rootVisualElement.Q<Button>("ExportButton");
            button.clicked += Create;

            var objects = Selection.GetFiltered(typeof(VisualTreeAsset), SelectionMode.Assets);
            var objectPath = AssetDatabase.GetAssetPath(objects[0]);

            _selectItemField = rootVisualElement.Q<TextField>("SelectItemField");
            _selectItemField.value = objectPath;

            _outputItemField = rootVisualElement.Q<TextField>("OutputFilePathField");
            _outputItemField.value = Application.dataPath;

            _outputFileNameField = rootVisualElement.Q<TextField>("OutputFileNameField");
            _outputFileNameField.RegisterValueChangedCallback(OutputFileNameFieldCallBack);
            _outputFileNameField.value = Path.GetFileNameWithoutExtension(objectPath);

            _outputExportFileName = rootVisualElement.Q<Label>("CheckOutputFileNameLabel");
            _outputExportFileName.text = string.Format(EXPORT_FILE_FORMAT, _outputFileNameField.value);

            var outputItemButton = rootVisualElement.Q<Button>("OutputFilePathButton");
            outputItemButton.clicked += () => {
                var path = EditorUtility.OpenFolderPanel("Select Folder", Application.dataPath, string.Empty);
                _outputItemField.value = path;
            };
        }

        private void OutputFileNameFieldCallBack(ChangeEvent<string> value)
        {
            _outputExportFileName.text = string.Format(EXPORT_FILE_FORMAT, value.newValue);
        }

        private void Create()
        {
            VisualElement baseElement = new VisualElement();
            var visualTree = AssetDatabase.LoadAssetAtPath(_selectItemField.value, typeof(VisualTreeAsset)) as VisualTreeAsset;
            visualTree.CloneTree(baseElement);

            Dictionary<string[], VisualElement> elementList = new Dictionary<string[], VisualElement>();
            List<string> paths = new List<string>();
            Search(baseElement, paths, ref elementList);

            Debug.Log("Create Count : " + elementList.Count);
        }

        private void Search(VisualElement baseElement, List<string> paths, ref Dictionary<string[], VisualElement> elementList)
        {
            paths.Add(baseElement.name);
            foreach (var element in baseElement.Children())
            {
                if (GetElement(element, paths, ref elementList))
                {
                    Search(element, paths, ref elementList);
                }
            }
        }

        private bool GetElement(VisualElement element, List<string> paths, ref Dictionary<string[], VisualElement> elementList)
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
                paths.Add(element.name);
                elementList.Add(paths.ToArray(), element);
            }
            else
            {
                isUIElement = true;
            }

            return isUIElement;
        }
    }
}
