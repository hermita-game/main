using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Items
{
    public class Crafting : MonoBehaviour
    {
        public GameObject canvas;
        private GameObject _showing;
        private GameObject _workshop;
        private GameObject _workbench;
        private GameObject _labo;

        // Start is called before the first frame update
        void Start()
        {
            _workshop = canvas.transform.Find("Workshop UI").gameObject;
            _workbench = canvas.transform.Find("Workbench UI").gameObject;
            _labo = canvas.transform.Find("Labo UI").gameObject;
            canvas.SetActive(true);
            _workshop.SetActive(false);
            _workbench.SetActive(false);
            _labo.SetActive(false);
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.K))
            {
                if (_showing == _workshop) Hide();
                else
                {
                    Hide();
                    Show_workshop();
                }
            }
            if (Input.GetKeyDown(KeyCode.L))
            {
                if (_showing == _labo) Hide();
                else
                {
                    Hide();
                    Show_labo();
                }
            }
            if (Input.GetKeyDown(KeyCode.M))
            {
                if (_showing == _workbench) Hide();
                else
                {
                    Hide();
                    Show_workbench();
                }
            }
        }
    
        private void Show_workbench()
        {
            _workbench.SetActive(true);
            _showing = _workbench;
        }
        
        private void Show_workshop()
        {
            _workshop.SetActive(true);
            _showing = _workshop;
        }
        
        private void Show_labo()
        {
            _labo.SetActive(true);
            _showing = _labo;
        }

        private void Hide()
        {
            _workshop.SetActive(false);
            _workbench.SetActive(false);
            _labo.SetActive(false);
            _showing = null;
        }
    }
}

