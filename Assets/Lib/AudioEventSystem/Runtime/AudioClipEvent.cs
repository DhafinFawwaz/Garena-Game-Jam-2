using UnityEngine;
using System;
using DhafinFawwaz.ActionExtension;
namespace DhafinFawwaz.AudioEventSystem
{
[Serializable]
public class AudioClipEvent
{
    public StaticAction Action;
    public AudioClip Clip;
    public float Volume = 1;
}

#if UNITY_EDITOR
[UnityEditor.CustomPropertyDrawer(typeof(AudioClipEvent))]
public class AudioEventClipDrawer : UnityEditor.PropertyDrawer
{
    public override void OnGUI(Rect position, UnityEditor.SerializedProperty property, GUIContent label)
    {
        int moveLeft = 10;
        position.x -= moveLeft;
        position.width += moveLeft;

        int volumeWidth = 25;
        int gap = 2;
        float width = (position.width - volumeWidth - gap)/2;
        Rect actionRect = new Rect(position.x, position.y, width - gap/2, position.height);
        Rect clipRect = new Rect(position.x + width + gap/2, position.y, width, position.height);
        Rect volumeRect = new Rect(position.x + position.width - volumeWidth, position.y, volumeWidth, position.height);

        UnityEditor.EditorGUI.PropertyField(actionRect, property.FindPropertyRelative("Action"), GUIContent.none);
        UnityEditor.EditorGUI.PropertyField(clipRect, property.FindPropertyRelative("Clip"), GUIContent.none);
        UnityEditor.EditorGUI.PropertyField(volumeRect, property.FindPropertyRelative("Volume"), GUIContent.none);
    }

}
#endif

}