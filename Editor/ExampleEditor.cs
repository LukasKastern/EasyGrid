using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

namespace EasyGrid
{
    public class ExampleEditor : EditorWindow
    {
        [MenuItem("Example/Editor")]
        private static void Init()
        {
            ExampleEditor window = (ExampleEditor)EditorWindow.GetWindow(typeof(ExampleEditor));

            window.titleContent = new GUIContent("EasyGrid");
            
            window.Show();
        }

        private void Awake()
        {
        }

        private void OnEnable()
        {
            EditorGrid.Initialize(this);
            EditorGrid.SetViewPort(new ViewPort(0.1f, 0.1f, 0.8f, 0.8f));;
            EditorGrid.IsOutlineActive = true;
            EditorGrid.GridOutlineColor = Color.blue;
        }

        private void CreateATextureGraphic(Texture texture, Cell cell, int width, int length)
        {
            new GridTexture(cell, width, length, texture);
        }

    
        protected void OnGUI()
        {
            EditorGrid.Update();
        }


    }
}