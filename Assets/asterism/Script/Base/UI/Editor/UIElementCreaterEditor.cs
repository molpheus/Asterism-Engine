using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using System.IO;
using Asterism;

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

        VisualElement _selectWindow;

        TextField _selectItemField;
        TextField _outputItemField;
        TextField _outputFileNameField;
        Label _outputExportFileName;

        Dictionary<string[], VisualElement> _elementList;

        private void OnEnable()
        {
            var filePath = AssetDatabase.GUIDToAssetPath(EDITOR_UIELEMENT_GUID);
            var visualTree = AssetDatabase.LoadAssetAtPath(filePath, typeof(VisualTreeAsset)) as VisualTreeAsset;

            visualTree.CloneTree(rootVisualElement);

            var button = rootVisualElement.Q<Button>("CheckButton");
            button.clicked += CreateElement;

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

            _selectWindow = rootVisualElement.Q("SelectWindow");
        }

        private void OutputFileNameFieldCallBack(ChangeEvent<string> value)
        {
            _outputExportFileName.text = string.Format(EXPORT_FILE_FORMAT, value.newValue);
        }

        private void CreateElement()
        {
            VisualElement baseElement = new VisualElement();
            var visualTree = AssetDatabase.LoadAssetAtPath(_selectItemField.value, typeof(VisualTreeAsset)) as VisualTreeAsset;
            visualTree.CloneTree(baseElement);

            Dictionary<string[], VisualElement> elementList = new Dictionary<string[], VisualElement>();
            List<string> paths = new List<string>();
            Search(baseElement, paths, ref elementList);

            Debug.Log("Create Count : " + elementList.Count);

            _elementList = elementList;


            _selectWindow.visible = true;

            if (elementList.Count > 0)
            {
                var selectContent = rootVisualElement.Q("SelectContent");
                var container = selectContent.Q("unity-content-container");

                while(container.hierarchy.childCount > 0)
                {
                    container.hierarchy.RemoveAt(0);
                }

                foreach(var e in elementList)
                {
                    var rootElement = new VisualElement();
                    rootElement.style.flexDirection = FlexDirection.Row;

                    var checkBox = new Toggle();
                    checkBox.value = true;
                    rootElement.hierarchy.Add(checkBox);

                    var label = new Label();
                    label.style.flexGrow = 1;
                    label.text = "";
                    foreach(var e2 in e.Key)
                    {
                        if (string.IsNullOrEmpty(e2)) continue;
                        if (!string.IsNullOrEmpty(label.text))
                            label.text += " < ";
                        label.text += e2;
                    }
                    rootElement.hierarchy.Add(label);

                    var contentType = new Label();
                    
                    var uiElement = CreateUIElement(e.Key, e.Value);
                    contentType.text = uiElement.ToString();
                    rootElement.hierarchy.Add(contentType);

                    container.hierarchy.Add(rootElement);
                }
            }

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
                bool isEnd = false;
                var parent = element.parent;
                var pathList = new List<string>();
                pathList.Add(element.name);
                while(!isEnd)
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

        private UIElement CreateUIElement(string[] pathList, VisualElement element)
        {
            return element switch {
                Label => new UIElementLabel() { TagNameList = pathList },
                Button => new UIElementButton() { TagNameList = pathList },
                Toggle => new UIElementToggle() { TagNameList = pathList },
                Scroller => new UIElementScroller() { TagNameList = pathList },
                TextField => new UIElementTextField() { TagNameList = pathList },
                //Foldout => ,
                Slider => new UIElementSlider() { TagNameList = pathList },
                SliderInt => new UIElementSliderInt() { TagNameList = pathList },
                MinMaxSlider => new UIElementMinMaxSlider() { TagNameList = pathList },
                ProgressBar => new UIElementProgressBar() { TagNameList = pathList },
                DropdownField => new UIElementDropdown() { TagNameList = pathList },
                EnumField => new UIElementEnum() { TagNameList = pathList },
                RadioButton => new UIElementRadioButton() { TagNameList = pathList },
                RadioButtonGroup => new UIElementRadioButtonGroup() { TagNameList = pathList },
                _ => null
            };
        }
    }
}
