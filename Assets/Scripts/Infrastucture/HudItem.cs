using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Controllers;
using UnityEngine;

namespace Assets.Scripts.Infrastucture
{
    public abstract class HudItem : MonoBehaviour
    {
        public Font font;

        private Camera _camera;
        protected abstract float posX { get; }
        protected abstract float posY { get; }
        protected string text;
        protected PlayerController player;
        protected GUIStyle style;

        public abstract void SetValue();

        void Start()
        {
            style = new GUIStyle
            {
                fontSize = 60
            };
            style.normal.textColor = Color.white;
            player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        }

        void Awake()
        {
            _camera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        }

        void OnGUI()
        {
            SetValue();
            GUI.skin.font = font;
            GUI.color = Color.white;
            GUI.Label(new Rect(posX + 30, posY - 30, 200, 60), text, style);
        }

        public void Redraw()
        {
            transform.position = _camera.ScreenToWorldPoint(new Vector3(posX, Screen.height - posY, 10));   
        }
    }
}
