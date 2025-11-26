using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VariantPanel : MonoBehaviour
{
    public TMP_Text variantName;
    public Button variantButton;
    
    public void OnDisable()
    {
        variantButton.onClick.RemoveAllListeners();
        Destroy(gameObject);
    }
}
