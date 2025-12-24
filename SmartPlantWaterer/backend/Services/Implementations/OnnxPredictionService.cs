using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;
using SmartPlantWaterer.Services.Interfaces;

namespace SmartPlantWaterer.Services.Implementations
{
    public class OnnxPredictionService : IOnnxPredictionService
    {
        private readonly InferenceSession inferenceSession;

        public OnnxPredictionService()
        {
            inferenceSession = new InferenceSession("ML/water_model.onnx");
        }

        public float Predict(float moisture, float temperature, float humidity)
        {
            DenseTensor<float> tensor = new([1, 3]);

            tensor[0, 0] = moisture;
            tensor[0, 1] = temperature;
            tensor[0, 2] = humidity;

            IDisposableReadOnlyCollection<DisposableNamedOnnxValue> result = inferenceSession.Run([
            NamedOnnxValue.CreateFromTensor("input", tensor)
            ]);

            return result != null ? result.FirstOrDefault()!.AsEnumerable<float>().FirstOrDefault() : 0.0f;
        }
    }
}
