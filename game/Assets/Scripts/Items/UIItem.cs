using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Items
{
    public class UIItem : MonoBehaviour
    {
        private Image _img;
        private TextMeshProUGUI _textMesh;

        void Awake()
        {
            // Set the Panel background to the item's sprite
            _img = GetComponent<Image>();
            _textMesh = GetComponentInChildren<TextMeshProUGUI>();
        }
    
        public void SetItem(Item item, int amount)
        {
            _img.sprite = item.Icon;
            _textMesh.text = amount == 1 ? "" : Item.AmountToString(amount);
        }
    }
}
