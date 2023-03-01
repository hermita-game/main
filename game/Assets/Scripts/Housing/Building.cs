using System;
using Unity.VisualScripting;
using UnityEngine;

namespace Housing
{
    public class Building : MonoBehaviour 
    {
        public Canvas canvas;
        
        // Start is called before the first frame update
        private void Start()
        {
            canvas.enabled = false;
        }

        private void OnMouseDown()
        {
            var position = transform.position;
            var task = PlayerMovement.GetPlayer().GetComponent<PlayerMovement>().MoveTo(position.x, position.y);
            task.ContinueWith(t =>
            {
                if (t.Result)
                    canvas.enabled = true;
                else Debug.Log($"No path to building {name}");
            });
        }
    }
}
