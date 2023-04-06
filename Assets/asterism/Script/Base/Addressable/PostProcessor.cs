#if UNITY_EDITOR
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;

using Asterism.Common;

namespace Asterism.Addressable
{
    // https://yuumekou.net/addressable-asset-system-auto/
    public class PostProcessor : AssetPostprocessor
    {
        static string AddressableResources {
            get {
                var data = (ConfigObject)AssetDatabase.FindAssets("t:ScriptableObject") // プロジェクトに存在する全ScriptableObjectのGUIDを取得
               .Select(guid => AssetDatabase.GUIDToAssetPath(guid)) // GUIDをパスに変換
               .Where(path => path.IndexOf("asterism/Config/Config.asset") >= 0)
               .Select(path => AssetDatabase.LoadAssetAtPath(path, typeof(ConfigObject))) // パスからPermanentDataの取得を試みる
               .Where(obj => obj != null) // null要素は取り除く
               .FirstOrDefault();
                return data.AddressablePath;
            }
        }

        static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssets)
        {
            AddressableAssetSettings settings = AddressableAssetSettingsDefaultObject.Settings;

            ImportAddressableAssets(settings, importedAssets);
            DeleteAddressableAssets(settings, deletedAssets);
            MoveAddressableAssets(settings, movedAssets, movedFromAssets);
        }

        static void ImportAddressableAssets(AddressableAssetSettings settings, string[] assets)
        {
            bool isRegistered = false;
            foreach (string asset in assets) {
                if (!File.Exists(asset)) continue;

                int folderNameIndex = asset.IndexOf(AddressableResources);
                if (folderNameIndex < 0) continue;

                string assetPath = asset.Remove(0, folderNameIndex + AddressableResources.Length + 1);
                int assetPathIndex = assetPath.IndexOf("/");
                if (assetPathIndex < 0) continue;

                string groupName = assetPath.Substring(0, assetPathIndex);
                string assetName = assetPath.Remove(0, assetPathIndex + 1);

                AddressableAssetGroup group = settings.FindGroup(groupName);
                if (group == null) group = CreateAddressableGroup(settings, groupName);

                if (group == null) {
                    Debug.LogError($"グループの作成に失敗. {groupName}");
                    continue;
                }

                AddressableAssetEntry entry = RegisterAssetPath(settings, group, asset);
                RegisterAssetLabel(entry, Path.GetDirectoryName(assetPath), assetName);
                isRegistered = true;
            }
            if (isRegistered) AssetDatabase.SaveAssets();
        }

        static void DeleteAddressableAssets(AddressableAssetSettings settings, string[] assets)
        {
            bool isDeleted = false;
            foreach (string asset in assets) {
                if (!File.Exists(asset)) continue;

                int folderNameindex = asset.IndexOf(AddressableResources);
                if (folderNameindex < 0) continue;

                string assetPath = asset.Remove(0, folderNameindex + AddressableResources.Length + 1);
                int assetPathIndex = assetPath.IndexOf("/");
                if (assetPathIndex < 0) continue;

                string guid = AssetDatabase.AssetPathToGUID(asset);
                if (string.IsNullOrEmpty(guid)) continue;

                settings.RemoveAssetEntry(guid);
                isDeleted = true;
            }
            if (isDeleted) AssetDatabase.SaveAssets();
        }

        static void MoveAddressableAssets(AddressableAssetSettings settings, string[] movedAssets, string[] movedFromAssets)
        {
            RemoveOldAddressableAssets(settings, movedAssets, movedFromAssets);
            ImportAddressableAssets(settings, movedAssets);
        }

        static void RemoveOldAddressableAssets(AddressableAssetSettings settings, string[] movedAssets, string[] movedFromAssets)
        {
            bool isRemoved = false;
            for (int i = 0; i < movedFromAssets.Length; i++) {
                if (!File.Exists(movedAssets[i])) continue;

                if (movedFromAssets[i].IndexOf(AddressableResources) < 0) continue;

                string guid = AssetDatabase.AssetPathToGUID(movedAssets[i]);
                if (string.IsNullOrEmpty(guid)) continue;

                settings.RemoveAssetEntry(guid);
                isRemoved = true;
            }
            if (isRemoved) AssetDatabase.SaveAssets();
        }

        static AddressableAssetGroup CreateAddressableGroup(AddressableAssetSettings settings, string groupName)
        {
            var groupTemplate = settings.GetGroupTemplateObject(0) as AddressableAssetGroupTemplate;
            AddressableAssetGroup group = settings.CreateGroup(groupName, false, false, true, null, groupTemplate.GetTypes());
            groupTemplate.ApplyToAddressableAssetGroup(group);
            return group;
        }

        static AddressableAssetEntry RegisterAssetPath(AddressableAssetSettings settings, AddressableAssetGroup group, string asset)
        {
            string guid = AssetDatabase.AssetPathToGUID(asset);
            AddressableAssetEntry entry = settings.CreateOrMoveEntry(guid, group);
            return entry;
        }

        static void RegisterAssetLabel(AddressableAssetEntry entry, string assetDirectoryName, string assetName)
        {
            string label = assetDirectoryName.Replace('\\', '/');
            if (string.IsNullOrEmpty(label)) return;

            entry.SetAddress(Path.GetFileNameWithoutExtension(assetName));
            entry.SetLabel(label, true, true);
        }
    }
}

#endif