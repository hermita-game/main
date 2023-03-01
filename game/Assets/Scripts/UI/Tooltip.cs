using Items;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Tooltip : MonoBehaviour
{
    private Image _img;
    private TextMeshProUGUI _title;
    private TextMeshProUGUI _description;
    private GameObject _stats;
    private TextMeshProUGUI _statsText;

    private void Start()
    {
        _img = transform.Find("Icon").GetComponent<Image>();
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
        _title.text = item.Name;
        _description.text = item.Description;
        if (item is Equipment or Consumable)
        {
            _stats.SetActive(true);
            _statsText.text = item is Equipment equipment ?
                equipment.BaseStats.ToString() : ((Consumable) item).Stats.ToString();
        }
        else _stats.SetActive(false);
    }
}
