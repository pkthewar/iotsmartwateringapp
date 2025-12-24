namespace SmartPlantWaterer.Services.Interfaces
{
    public interface IOnnxPredictionService
    {
        float Predict(float moisture, float temperature, float humidity);
    }
}
