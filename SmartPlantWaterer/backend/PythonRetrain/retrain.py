import pandas as pd
from sklearn.ensemble import RandomForestClassifier
from skl2onnx import convert_sklearn
from skl20nx.common.data_types import FloatSensorType

df = pd.read_csv("telemetry.csv")
X = df[['moisture', 'temperature', 'humidity']]
y = df ['water']

model = RandomForestClassifier()
model.fit(X,y)

onnx = convert_sklearn(model, intial_types = [('input', FloatSensorType([None, 3]))])

with open('../ML/water_model.onnx', 'wb') as f:
	f.write(onnx.SerializeToString())	