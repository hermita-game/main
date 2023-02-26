using System;
using Unity.VisualScripting;
using UnityEngine;

namespace Housing
{
    public class Building : MonoBehaviour 
    {
        public Canvas canvas;
        
        // Start is called before the first frame update
        void Start()
        {
            canvas.enabled = false;
        }

        void OnMouseDown()
        {
            canvas.enabled = true;
        }
    }
}
