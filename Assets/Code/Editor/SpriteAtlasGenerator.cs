using UnityEngine;
using UnityEditor;
using UnityEngine.U2D;
using UnityEditor.U2D;
using System.IO;
using System.Collections.Generic;
using System.Linq;

public class SpriteAtlasGenerator : EditorWindow
{
    private string selectedFolderPath = "";
    private Vector2 scrollPosition;
    
    [MenuItem("Tools/Sprite Atlas Generator")]
    public static void ShowWindow()
    {
        GetWindow<SpriteAtlasGenerator>("Sprite Atlas Generator");
    }
    
    private void OnGUI()
    {
        GUILayout.Space(10);
        
        EditorGUILayout.LabelField("Sprite Atlas Generator", EditorStyles.boldLabel);
        GUILayout.Space(10);
        
        // Folder selection
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Selected Folder:", GUILayout.Width(100));
        EditorGUILayout.LabelField(selectedFolderPath.Length > 0 ? selectedFolderPath : "No folder selected");
        EditorGUILayout.EndHorizontal();
        
        GUILayout.Space(5);
        
        if (GUILayout.Button("Select Folder"))
        {
            string path = EditorUtility.OpenFolderPanel("Select folder containing sprite folders", Application.dataPath, "");
            if (!string.IsNullOrEmpty(path))
            {
                // Convert absolute path to relative path
                if (path.StartsWith(Application.dataPath))
                {
                    selectedFolderPath = "Assets" + path.Substring(Application.dataPath.Length).Replace('\\', '/');
                }
                else
                {
                    EditorUtility.DisplayDialog("Invalid Folder", "Please select a folder within the Assets directory.", "OK");
                    selectedFolderPath = "";
                }
            }
        }
        
        GUILayout.Space(10);
        
        // Generate button
        GUI.enabled = !string.IsNullOrEmpty(selectedFolderPath);
        if (GUILayout.Button("Generate Sprite Atlases", GUILayout.Height(30)))
        {
            GenerateSpriteAtlases();
        }
        GUI.enabled = true;
        
        GUILayout.Space(10);
        
        // Display subfolders that will be processed
        if (!string.IsNullOrEmpty(selectedFolderPath))
        {
            DisplaySubfolders();
        }
    }
    
    private void DisplaySubfolders()
    {
        EditorGUILayout.LabelField("Subfolders to process:", EditorStyles.boldLabel);
        
        if (Directory.Exists(selectedFolderPath))
        {
            string[] subDirectories = Directory.GetDirectories(selectedFolderPath);
            
            if (subDirectories.Length == 0)
            {
                EditorGUILayout.LabelField("No subfolders found in the selected directory.");
                return;
            }
            
            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, GUILayout.Height(150));
            
            foreach (string dir in subDirectories)
            {
                string folderName = Path.GetFileName(dir);
                
                // Count sprites in this folder
                int spriteCount = CountSpritesInFolder(dir);
                
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField($"• {folderName}", GUILayout.Width(150));
                EditorGUILayout.LabelField($"({spriteCount} sprites)", EditorStyles.miniLabel);
                EditorGUILayout.EndHorizontal();
            }
            
            EditorGUILayout.EndScrollView();
        }
        else
        {
            EditorGUILayout.LabelField("Selected folder does not exist.");
        }
    }
    
    private int CountSpritesInFolder(string folderPath)
    {
        if (!Directory.Exists(folderPath))
            return 0;
            
        string[] files = Directory.GetFiles(folderPath, "*.*", SearchOption.TopDirectoryOnly)
            .Where(f => f.EndsWith(".png", System.StringComparison.OrdinalIgnoreCase) || 
                       f.EndsWith(".jpg", System.StringComparison.OrdinalIgnoreCase) || 
                       f.EndsWith(".jpeg", System.StringComparison.OrdinalIgnoreCase) || 
                       f.EndsWith(".tga", System.StringComparison.OrdinalIgnoreCase) || 
                       f.EndsWith(".psd", System.StringComparison.OrdinalIgnoreCase))
            .ToArray();
            
        return files.Length;
    }
    
    private void GenerateSpriteAtlases()
    {
        if (string.IsNullOrEmpty(selectedFolderPath) || !Directory.Exists(selectedFolderPath))
        {
            EditorUtility.DisplayDialog("Error", "Please select a valid folder first.", "OK");
            return;
        }
        
        string[] subDirectories = Directory.GetDirectories(selectedFolderPath);
        
        if (subDirectories.Length == 0)
        {
            EditorUtility.DisplayDialog("No Subfolders", "No subfolders found in the selected directory.", "OK");
            return;
        }
        
        int totalFolders = subDirectories.Length;
        int processedFolders = 0;
        
        try
        {
            AssetDatabase.StartAssetEditing();
            
            foreach (string dir in subDirectories)
            {
                string folderName = Path.GetFileName(dir);
                string relativeFolderPath = ConvertToRelativePath(dir);
                
                // Update progress bar
                EditorUtility.DisplayProgressBar("Generating Sprite Atlases", 
                    $"Processing folder: {folderName}", 
                    (float)processedFolders / totalFolders);
                
                CreateSpriteAtlasForFolder(relativeFolderPath, folderName);
                processedFolders++;
            }
            
            AssetDatabase.StopAssetEditing();
            AssetDatabase.Refresh();
            
            // Now pack all the created atlases
            EditorUtility.DisplayProgressBar("Packing Sprite Atlases", 
                "Packing atlases...", 0.5f);
            
            PackAllCreatedAtlases();
        }
        finally
        {
            EditorUtility.ClearProgressBar();
        }
        
        EditorUtility.DisplayDialog("Complete", 
            $"Successfully generated {processedFolders} sprite atlases!", "OK");
    }
    
    private void CreateSpriteAtlasForFolder(string folderPath, string folderName)
    {
        // Get all sprite assets in the folder
        string[] guids = AssetDatabase.FindAssets("t:Sprite", new[] { folderPath });
        
        if (guids.Length == 0)
        {
            Debug.LogWarning($"No sprites found in folder: {folderPath}");
            return;
        }
        
        // Create new Sprite Atlas
        SpriteAtlas spriteAtlas = new SpriteAtlas();
        
        // Set atlas settings
        SpriteAtlasPackingSettings packingSettings = new SpriteAtlasPackingSettings()
        {
            blockOffset = 1,
            enableRotation = false,
            enableTightPacking = false,
            padding = 2,
        };
        spriteAtlas.SetPackingSettings(packingSettings);
        
        // Set texture settings with Crunch compression
        SpriteAtlasTextureSettings textureSettings = new SpriteAtlasTextureSettings()
        {
            readable = false,
            generateMipMaps = false,
            sRGB = true,
            filterMode = FilterMode.Bilinear,
        };
        spriteAtlas.SetTextureSettings(textureSettings);
        
        // Set platform settings for crunch compression
        TextureImporterPlatformSettings platformSettings = new TextureImporterPlatformSettings()
        {
            name = "DefaultTexturePlatform",
            overridden = false,
            maxTextureSize = 2048,
            format = TextureImporterFormat.Automatic,
            compressionQuality = 100,
            crunchedCompression = true,
            allowsAlphaSplitting = false
        };
        spriteAtlas.SetPlatformSettings(platformSettings);
        
        // Add sprites to atlas
        List<Object> sprites = new List<Object>();
        foreach (string guid in guids)
        {
            string spritePath = AssetDatabase.GUIDToAssetPath(guid);
            Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(spritePath);
            if (sprite != null)
            {
                sprites.Add(sprite);
            }
        }
        
        spriteAtlas.Add(sprites.ToArray());
        
        // Save the atlas
        string atlasPath = Path.Combine(folderPath, $"{folderName}.spriteatlas");
        AssetDatabase.CreateAsset(spriteAtlas, atlasPath);
        
        // Add to the list of created atlases for packing
        createdAtlasPaths.Add(atlasPath);
        
        Debug.Log($"Created Sprite Atlas: {atlasPath} with {sprites.Count} sprites");
    }
    
    private string ConvertToRelativePath(string absolutePath)
    {
        if (absolutePath.StartsWith(Application.dataPath))
        {
            return "Assets" + absolutePath.Substring(Application.dataPath.Length).Replace('\\', '/');
        }
        return absolutePath;
    }
    
    private List<string> createdAtlasPaths = new List<string>();
    
    private void PackAllCreatedAtlases()
    {
        if (createdAtlasPaths.Count == 0)
        {
            Debug.LogWarning("No sprite atlases were created to pack.");
            return;
        }
        
        List<SpriteAtlas> atlasesToPack = new List<SpriteAtlas>();
        
        foreach (string atlasPath in createdAtlasPaths)
        {
            SpriteAtlas atlas = AssetDatabase.LoadAssetAtPath<SpriteAtlas>(atlasPath);
            if (atlas != null)
            {
                atlasesToPack.Add(atlas);
            }
        }
        
        if (atlasesToPack.Count > 0)
        {
            // Pack the atlases
            SpriteAtlasUtility.PackAtlases(atlasesToPack.ToArray(), EditorUserBuildSettings.activeBuildTarget, false);
            
            // Force refresh to show the packed results
            AssetDatabase.Refresh();
            
            Debug.Log($"Successfully packed {atlasesToPack.Count} sprite atlases.");
        }
        
        // Clear the list for next time
        createdAtlasPaths.Clear();
    }
}