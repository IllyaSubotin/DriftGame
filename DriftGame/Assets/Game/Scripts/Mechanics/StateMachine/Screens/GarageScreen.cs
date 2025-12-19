using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GarageScreen : UIScreen
{
    public CarGarageController[] carsController;
    public GameObject[] cars;
    public Transform[] carPositionPonts;
    public String[] variantTitleTexts;
    public String[] variantTexts;
    public CarMaterialConfig[] carDataConfigs;
    public GameObject variantPrefab;
    public GameObject variantPanel;
    public GameObject variantButtonsPanel;
    public TMP_Text variantText;
    public Button carButton;
    public Button colorButton;
    public Button wheelButton;
    public Button spoilerButton;
    public Button menuButton;
}
