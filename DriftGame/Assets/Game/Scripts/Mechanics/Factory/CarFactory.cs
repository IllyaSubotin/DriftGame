using Photon.Pun;
using UnityEngine;
using Zenject;

public class CarFactory : MonoBehaviour, ICarFactory
{
    public CarPrefabsConfig carPrefabs;

    private ICarData _carData;
    private DiContainer _container;

    [Inject]
    private void Construct(ICarData carData, DiContainer container)
    {
        _container = container;
        _carData = carData;
    }

    public GameObject SpawnPlayerCar(Transform spawnPoint)
    {
        int carId = _carData.currentCarId;
        var prefab = carPrefabs.carPrefabs[carId];

        GameObject car = _container.InstantiatePrefab(prefab, spawnPoint.position, spawnPoint.rotation, null);

        var customizer = car.GetComponent<CarCustomizer>();
        if (customizer != null)
            customizer.ApplyCustomization(_carData);

        var controller = car.GetComponent<CarController>();
        if (controller != null)
            controller.Init(customizer.carWheelSet[_carData.carSaveDatas[_carData.currentCarId].wheelsIndex]);
            

        return car;
    }

    public GameObject SpawnPlayerCarOnline(Transform spawnPoint)
    {
        
        int carId = _carData.currentCarId;
        var prefab = carPrefabs.carPrefabsName[carId];

        var car = PhotonNetwork.Instantiate(prefab, spawnPoint.position, spawnPoint.rotation);

        var sync = car.GetComponent<CarCustomizationSync>();
        if (sync != null)
            sync.ApplyLocalCustomization(_carData);

        var customizer = car.GetComponent<CarCustomizer>();

        var controller = car.GetComponent<CarController>();
        if (controller != null)
            controller.Init(customizer.carWheelSet[_carData.carSaveDatas[_carData.currentCarId].wheelsIndex]);


        return car;
    }
    
    
}
