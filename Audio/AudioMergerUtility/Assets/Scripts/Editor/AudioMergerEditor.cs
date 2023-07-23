#if UNITY_EDITOR
using System;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(AudioMerger)), CanEditMultipleObjects]
    public class AudioMergerEditor : UnityEditor.Editor
    {

        #region Serialized Properties

        private SerializedProperty _soundToMerge;
        private SerializedProperty _mergingButton;
        
        #endregion
        
        private GUIStyle _titleStyle;
        private readonly Color _labelColor = new Color(0.4f,0.4f,0.4f,1);
        private RectOffset _rectOffset;

        private void OnEnable()
        {
            _soundToMerge = serializedObject.FindProperty("soundToMerge");
            _mergingButton = serializedObject.FindProperty("mergingButton");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            
            _rectOffset = new RectOffset(7, 7, 7, 7);
            _titleStyle = new GUIStyle(GUI.skin.label)
            {
                alignment = TextAnchor.MiddleCenter,
                //margin = _rectOffset,
                //padding = _rectOffset,
                fontSize = 20,
                fontStyle = FontStyle.Bold,
                normal =
                {
                    textColor = Color.white,
                    background = MakeTex(2,2,_labelColor)
                },
                padding = _rectOffset
            };

            GUILayout.Space(5f);
            GUILayout.Box(" Audio Merger", _titleStyle);
            GUILayout.Space(10f);

            EditorGUILayout.PropertyField(_soundToMerge);
            GUILayout.Space(5f);
            EditorGUILayout.PropertyField(_mergingButton);

            serializedObject.ApplyModifiedProperties();
        }
    
        private Texture2D MakeTex( int width, int height, Color col )
        {
            Color[] pix = new Color[width * height];
            for( int i = 0; i < pix.Length; ++i )
            {
                pix[ i ] = col;
            }
            var result = new Texture2D( width, height );
            result.SetPixels( pix );
            result.Apply();
            return result;
        }
    }
}
#endif