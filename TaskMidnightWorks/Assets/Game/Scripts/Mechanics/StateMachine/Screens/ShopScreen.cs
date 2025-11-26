using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopScreen : UIScreen
{
    public CarGarageController[] carsController;
    public GameObject[] cars;
    public Transform[] carPositionPonts;
    public Button nextCarButton;
    public Button previosCarButton;
    public Button cashBuyButton;
    public Button gemsBuyButton;
    public TMP_Text cashBuyText;
    public TMP_Text gemsBuyTexts;
    public TMP_Text carNotAvailableText;
    public Button menuButton;
    public CarsPriceConfig carsPriceConfig;
}
