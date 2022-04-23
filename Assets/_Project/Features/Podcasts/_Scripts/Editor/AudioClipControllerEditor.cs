using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Multiplatform.Podcasts.Editors
{
#if UNITY_EDITOR
    [CustomEditor(typeof(AudioClipController))]
    public class AudioClipControllerEditor : Editor
    {
        AudioClipController audio;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            GUILayout.Space(10f);

            if (GUILayout.Button("Play"))
            {
                audio.Play();
            }
            if (GUILayout.Button("Pause"))
            {
                audio.Pause();
            }
            if (GUILayout.Button("Stop"))
            {
                audio.Stop();
            }
        }

        private void OnEnable()
        {
            if (audio == null) audio = (target as AudioClipController);
        }
    }
#endif
}