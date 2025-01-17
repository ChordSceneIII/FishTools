using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
/// <summary>
/// 文件夹图标自定义
/// </summary>
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

    static FolderIconsLoad()
    {
        EditorApplication.projectWindowItemOnGUI += OnProjectWindowItemGUI;
    }

    private static void OnProjectWindowItemGUI(string guid, Rect selectionRect)
    {
        string path = AssetDatabase.GUIDToAssetPath(guid);
        if (System.IO.Directory.Exists(path))
        {
            foreach (var entry in folderIconMappings)
            {
                if (path.EndsWith(entry.Key))
                {
                    Rect rect = new Rect(selectionRect.x + selectionRect.width - 16, selectionRect.y, 16, 16);
                    string iconPath = $"Assets/Plugins/FishTools/Editor/FolderIcons/{entry.Value}";
                    GUI.Label(rect, EditorGUIUtility.IconContent(iconPath));
                    break;
                }
            }
        }
    }
}

