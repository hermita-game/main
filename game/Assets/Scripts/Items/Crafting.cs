using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Items
{
    public class Crafting : MonoBehaviour
    {
        public GameObject canvas;
        private bool _showing;

        // Start is called before the first frame update
        void Start()
        {
            canvas.SetActive(false);
        }

        // Update is called once per frame
        void Update()
        {
            print("ooo");
            if (!Input.GetKeyDown(KeyCode.C)) return;
            if (_showing) Hide();
            else Show();
        }
    
        private void Show()
        {
            canvas.SetActive(true);
            _showing = true;
        }

        private void Hide()
        {
            canvas.SetActive(false);
            _showing = false;
        }
    }
}

