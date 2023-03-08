using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Items
{
    public class UIItem : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
    {
        private Image _img;
        private TextMeshProUGUI _textMesh;
        private Item _item;
        public Tooltip tooltip;

        void Awake()
        {
            // Set the Panel background to the item's sprite
            _img = GetComponent<Image>();
            _textMesh = GetComponentInChildren<TextMeshProUGUI>();
        }
    
        public void SetItem(Item item, int amount)
        {
            _item = item;
            _img.sprite = item.Icon;
            _textMesh.text = amount == 1 ? "" : Item.AmountToString(amount);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (_item is null) return;
            tooltip.GenerateTooltip(_item);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            tooltip.gameObject.SetActive(false);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            // on right click
            if (eventData.button != PointerEventData.InputButton.Right) return;
            // if the item is a weapon
            if (_item is Equipment eq)
            {
                Tools.GetInventory().Remove(eq);
                var old = Tools.GetPlayer().GetComponent<Fighting.Player>().Equip(eq);
                if (old != null) Tools.GetInventory().Loot(old);
            } else if (_item is Consumable potion)
            {
                Tools.GetInventory().Remove(potion.Id);
                Tools.GetPlayer().UseConsumable(potion);
            }
        }
    }
}
