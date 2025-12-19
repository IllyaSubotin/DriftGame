using UnityEngine;

public class CarCustomizer : MonoBehaviour
{
    public Renderer[] renderMode;
    public GameObject[] WheelsSets;
    public CarWheelSet[] carWheelSet; 
    public CarMaterialConfig carMaterialConfig;
    public GameObject spoiler;
    public CarWheelSync carWheelSync;


    public void ApplyCustomization(ICarData data)
    {
        CarSaveData save = data.carSaveDatas[data.currentCarId];

        foreach (var render in renderMode)
        {
            render.material = carMaterialConfig.materials[save.materialIndex];
        }

        for (int i = 0; i < WheelsSets.Length; i++)
        {
            WheelsSets[i].SetActive(false);
        }

        WheelsSets[save.wheelsIndex].SetActive(true);

        if (spoiler != null)
        {
            spoiler.SetActive(save.spoilerEnabled);
        }
    }
}
