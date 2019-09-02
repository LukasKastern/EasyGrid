using System;
using System.Linq;
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
            //
        }

        private void OnEnable()
        {
            EditorGrid.BackgroundColor = new Color(0f, 0, 0, 0.5f);
            EditorGrid.BackGroundGridLineColor = new Color(0, 0, 0, 0.3f);

            EditorGrid.Initialize(this);
            EditorGrid.SetViewPort(new ViewPort(0.1f, 0.1f, 0.8f, 0.8f));;
            EditorGrid.IsOutlineActive = true;
            EditorGrid.GridOutlineColor = Color.blue;
            
           // CreateATextureGraphic(Texture2D.whiteTexture, new Cell(0, 0), 1, 1);
            
            CreateATextureGraphic(GetNodeTexture(), new Cell(0, 0), 20, 10);
            //CreateATextureGraphic(Texture2D.whiteTexture, new Cell(0, 0), 10, 5);

        }

        private Texture2D GetNodeTexture()
        {
            return Resources.Load<Texture2D>("DefaultNodeTexture");
        }

        private void CreateATextureGraphic(Texture texture, Cell cell, int width, int length)
        {
            new GridTexture(cell, width, length, texture) {IsInteractable = true, CanDrag = true};
        }

    
        protected void OnGUI()
        {
            EditorGrid.Update();
        }


    }
}