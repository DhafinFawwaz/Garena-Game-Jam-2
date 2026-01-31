using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class MeshRendererScalerByTexture : MonoBehaviour
{
    enum FitMode
    {
        PreserveX,
        PreserveY,
    }
    [SerializeField] FitMode _fitMode = FitMode.PreserveY;
    [SerializeField] Texture2D _texture;
    void Reset()
    {
        AutoAssignTexture();
    }

    public void AutoAssignTexture()
    {
        var _renderer = GetComponent<Renderer>();
        var _material = _renderer.sharedMaterial;
        _texture = _material.mainTexture as Texture2D;
    }

    public void Scale()
    {
        if(!IsValid()) return;

        Vector2 textureSize = new Vector2(_texture.width, _texture.height);

#if UNITY_EDITOR
        Undo.RecordObject(transform, "Scale");
#endif

        if(_fitMode == FitMode.PreserveX)
        {
            float xScale = transform.localScale.x;
            transform.localScale = new Vector3(
                xScale, // textureSize.x * xScale / textureSize.x,
                textureSize.y * xScale / textureSize.x,
                transform.localScale.z
            );
        }
        else if(_fitMode == FitMode.PreserveY)
        {
            float yScale = transform.localScale.y;
            transform.localScale = new Vector3(
                textureSize.x * yScale / textureSize.y,
                yScale, // textureSize.y * yScale / textureSize.y,
                transform.localScale.z
            );
        }

    }

    bool IsValid()
    {
        if (_texture == null)
        {
            Debug.LogError("Texture is null");
            return false;
        }
        return true;
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(MeshRendererScalerByTexture))]
public class MeshRendererScalerByTextureEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        MeshRendererScalerByTexture script = (MeshRendererScalerByTexture)target;
        if (GUILayout.Button("Scale"))
        {
            script.Scale();
        }
        else if (GUILayout.Button("Auto Assign Texture"))
        {
            script.AutoAssignTexture();
        }
    }
}

#endif
