using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Linq;

[InitializeOnLoad]
internal static class FolderIconsLoad
{
    private static readonly Dictionary<string, string> folderIconMappings = new Dictionary<string, string>
        {
            {"Editor", "c_icon_editor.png"},
            {"Resources", "c_icon_resources.png"},
            {"Plugins", "c_icon_plugins.png"},
            {"FishTools", "c_icon_fishtools.png"},
            {"Gizmos", "c_icon_gizmos.png"},
            {"Scripts","c_icon_scripts.png"},
            {"Assets","c_icon_assets.png"},
            {"Scenes","c_icon_scenes.png"},
            {"Fonts","c_icon_fonts.png"},
            {"Prefabs","c_icon_prefabs.png"},
            {"Materials","c_icon_materials.png"},
            {"Shaders","c_icon_shaders.png"},
            {"Textures","c_icon_textures.png"},
            {"Audios","c_icon_audios.png"},
            {"Animations","c_icon_animations.png"},
            {"Images","c_icon_images.png"},
            {"Configs","c_icon_configs.png"},
        };

    private static string iconsBasePath;

    static FolderIconsLoad()
    {
        InitializeIconsPath();
        EditorApplication.projectWindowItemOnGUI += OnProjectWindowItemGUI;
    }

    private static void InitializeIconsPath()
    {
        // 获取当前脚本的路径
        string scriptPath = GetScriptPath(typeof(FolderIconsLoad));
        if (!string.IsNullOrEmpty(scriptPath))
        {
            // 找到Editor目录的父路径
            string editorFolderPath = Path.GetDirectoryName(scriptPath)
                .Split(new[] { "Editor" }, System.StringSplitOptions.None)[0] + "Editor";

            // 组合成folderIcons完整路径
            iconsBasePath = Path.Combine(editorFolderPath, "folderIcons")
                .Replace("\\", "/") + "/";
        }

    }

    private static string GetScriptPath(System.Type type)
    {
        MonoScript script = null;
        var guids = AssetDatabase.FindAssets($"t:MonoScript {type.Name}");

        foreach (var guid in guids)
        {
            var path = AssetDatabase.GUIDToAssetPath(guid);
            var checkScript = AssetDatabase.LoadAssetAtPath<MonoScript>(path);
            if (checkScript != null && checkScript.GetClass() == type)
            {
                script = checkScript;
                break;
            }
        }
        return script != null ? AssetDatabase.GetAssetPath(script) : null;
    }

    private static void OnProjectWindowItemGUI(string guid, Rect selectionRect)
    {
        string path = AssetDatabase.GUIDToAssetPath(guid);
        if (Directory.Exists(path))
        {
            foreach (var entry in folderIconMappings)
            {
                if (path.EndsWith(entry.Key))
                {
                    Rect rect = new Rect(selectionRect.x + selectionRect.width - 16, selectionRect.y, 16, 16);
                    string iconPath = $"{iconsBasePath}{entry.Value}";

                    // 添加路径有效性检查
                    if (!File.Exists(iconPath))
                    {
                        Debug.LogWarning($"找不到图标文件: {iconPath}");
                        return;
                    }

                    GUI.Label(rect, EditorGUIUtility.IconContent(iconPath));
                    break;
                }
            }
        }
    }
}