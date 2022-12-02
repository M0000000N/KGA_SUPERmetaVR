using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RewardPopup : MonoBehaviour
{
    [SerializeField] private GameObject popupUI;

    [SerializeField] private Transform itemObjectParent;
    [SerializeField] private Image gradeImage;
    // [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private Button button;

    private CanvasGroup canvasGroup;

    private void Start()
    {
        canvasGroup = popupUI.transform.GetComponent<CanvasGroup>();
        button.onClick.AddListener(OnPress);
    }

    public void SetPopupUI(int _itemID)
    {
        GameObject prefab = Resources.Load<GameObject>("InventoryItem/Inventory" + StaticData.GetItemSheet(_itemID).Prefabname);
        GameObject item = Instantiate(prefab, itemObjectParent);
        item.transform.localPosition = Vector3.zero;
        item.transform.localScale = new Vector3(50, 50, 50);
        item.SetActive(false);

        string[] itemImage = {"Normal","HighClass","Unique","Epic"};

        gradeImage.sprite = Resources.Load<Sprite>("Item/Grade/" + itemImage[StaticData.GetItemSheet(_itemID).Grade]);

        // itemName.text = StaticData.GetItemSheet(_itemID).Name;
        popupUI.gameObject.SetActive(true);
        StartCoroutine(OpenUICoroutine(item));
    }

    public void OnPress()
    {
        StartCoroutine(CloseUICoroutine());
    }

    IEnumerator OpenUICoroutine(GameObject _item)
    {
        canvasGroup.alpha = 0;
        while (canvasGroup.alpha < 1)
        {
            canvasGroup.alpha += 0.1f;
            yield return new WaitForSeconds(0.1f);
        }

        _item.SetActive(true);
    }

    IEnumerator CloseUICoroutine()
    {
        if (itemObjectParent.childCount > 0)
        {
            GameObject item = itemObjectParent.GetChild(0).gameObject;
        
            //Material[] itemMaterial = item.GetComponentsInChildren<Material>();
            Destroy(item);
        }
        
        canvasGroup.alpha = 1;
        while (canvasGroup.alpha > 0)
        {
            //for (int i = 0; i < itemMaterial.Length; i++)
            //{
            //    Color color = itemMaterial[i].color;
            //    color.a -= 0.1f;
            //    itemMaterial[i].color = color;
            //}
            canvasGroup.alpha -= 0.1f;
            yield return new WaitForSeconds(0.1f);
        }

        popupUI.gameObject.SetActive(false);
    }
}
