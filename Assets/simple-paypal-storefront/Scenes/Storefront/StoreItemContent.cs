using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Globalization;
using TMPro;

public class StoreItemContent : MonoBehaviour {

	/* expose these values to user for convinience 
	 * (NOTE: these values will override any values set on the store item fields in the inspector)
	 */
	public Sprite itemImage;
	public string itemName;
	public string itemCost;
	public string itemDesc;

	private Image itemImageField;
	private TMP_Text itemNameTextField;
	private TMP_Text itemCostTextField;
	private TMP_Text itemCurCodeTextField;
	private TMP_Text itemDescTextField;

	// Use this for initialization
	void Start () {

		itemImageField = transform.Find ("ItemImage").GetComponent<Image> ();
		itemNameTextField = transform.Find ("ItemName").GetComponent<TMP_Text> ();
		itemCostTextField = transform.Find ("ItemCost").GetComponent<TMP_Text> ();
		itemCurCodeTextField = transform.Find ("ItemCurCode").GetComponent<TMP_Text> ();
		itemDescTextField = transform.Find ("ItemDesc").GetComponent<TMP_Text> ();
			
		if (itemImage == null) {
			itemImage = Resources.Load <Sprite> ("ItemSprites/DefaultImage");
		}

		itemImageField.sprite = itemImage;

		if (itemName.Length > 100) {
			itemName = itemName.Substring(0,99);
		}

		itemNameTextField.text = itemName;

		itemCostTextField.text = string.Format("{0:N}", itemCost);
		itemCostTextField.text = CurrencyCodeMapper.GetCurrencySymbol (GlobalPayPalProperties.INSTANCE.currencyCode) + itemCostTextField.text;

		itemCurCodeTextField.text = "(" + GlobalPayPalProperties.INSTANCE.currencyCode + ")";

		itemDescTextField.text = itemDesc;

	}
		
	public void BuyItemAction() {
		StoreActions.INSTANCE.OpenPurchaseItemScreen (this);
	}
	
}
