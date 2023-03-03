using Items;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Tooltip : MonoBehaviour
{
    private Image _img;
    private TextMeshProUGUI _idText;
    private TextMeshProUGUI _title;
    private TextMeshProUGUI _description;
    private GameObject _stats;
    private TextMeshProUGUI _statsText;

    private void Awake()
    {
        _img = transform.Find("Icon").GetComponent<Image>();
        _idText = transform.Find("ID").GetComponent<TextMeshProUGUI>();
        _title = transform.Find("Title").GetComponent<TextMeshProUGUI>();
        _description = transform.Find("Description").GetComponent<TextMeshProUGUI>();
        _stats = transform.Find("Stats").gameObject;
        _statsText = _stats.transform.Find("Text").GetComponent<TextMeshProUGUI>();
        gameObject.SetActive(false);
    }
    
    public void GenerateTooltip(Item item)
    {
        gameObject.SetActive(true);
        _img.sprite = item.Icon;
        _idText.text = $"ID: {item.Id}";
        _title.text = item.Name;
        _description.text = item.Description;
        
        // If the item is an equipment or a consumable, show the stats
        if (item is Equipment or Consumable)
        {
            _stats.SetActive(true);
            var text = "";
            if (item is Consumable { Duration: > 0 } consumable)
                text += $"Duration: {consumable.Duration}s\n";
            text += item is Equipment equipment ?
                equipment.BaseStats.ToString() : ((Consumable) item).Stats.ToString();
            _statsText.text = text;
        }
        else _stats.SetActive(false);
    }
}
