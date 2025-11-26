public class RuntimeCarData : ICarData
{
    public CarSaveData[] carSaveDatas { get; set; }
    public int currentCarId { get; set; }

    public RuntimeCarData(int carId, int mat, int wheels, bool spoiler)
    {
        currentCarId = carId;

        carSaveDatas = new CarSaveData[5]; // або динамічно, якщо потрібно
        carSaveDatas[carId] = new CarSaveData()
        {
            isOwned = true,
            materialIndex = mat,
            wheelsIndex = wheels,
            spoilerEnabled = spoiler
        };
    }
}
