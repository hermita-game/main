using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Items
{
    public class UIItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        private Image _img;
        private TextMeshProUGUI _textMesh;
        private Item _item;
        private Tooltip _tooltip;

        void Awake()
        {
            // Set the Panel background to the item's sprite
            _img = GetComponent<Image>();
            _textMesh = GetComponentInChildren<TextMeshProUGUI>();
            _tooltip = FindObjectOfType<Tooltip>();
        }
    
        public void SetItem(Item item, int amount)
        {
            _item = item;
            _img.sprite = item.Icon;
            _textMesh.text = amount == 1 ? "" : Item.AmountToString(amount);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _tooltip.GenerateTooltip(_item);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _tooltip.gameObject.SetActive(false);
        }
    }
}
