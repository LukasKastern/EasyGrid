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
            
            //--Some Performance Testing--//
            /*
            for (int i = 0; i < 100 ; ++i)
            {
                var bigTexture = new GridTexture(new Cell(-10, -10), 40, 20, Texture2D.whiteTexture) {IsInteractable = true, CanDrag = true};

                //texture.Transform.Parent = bigTexture.Transform;
            
                var label = new GridLabel(new Cell(1, 0));

                label.SetFontSize(3);

                label.Content = "I'm a Label!";
            
                label.Transform.Parent = bigTexture.Transform;
            }
            */
        }

        protected void OnGUI()
        {
            EditorGrid.Update();
        }


    }
}