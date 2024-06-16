import numpy as np
import tensorflow as tf
from tensorflow.keras.models import Sequential
from tensorflow.keras.layers import Dense
import pandas as pd
from sklearn.model_selection import train_test_split
import tf2onnx
import onnx

data = pd.read_csv('Assets/ImitationData/training_data.csv', index_col=False)

X = data.iloc[:, :10].values
y = data.iloc[:, 10].values

# Split data into training and testing sets
X_train, X_test, y_train, y_test = train_test_split(X, y, test_size=0.2, random_state=42)

def create_model():
    model = Sequential([
        Dense(32, activation='relu', input_shape=(10,)),
        Dense(16, activation='relu'),
        Dense(1, activation='linear')
    ])
    model.compile(optimizer='adam', loss='mse', metrics=['mae'])
    return model


model = create_model()
model.fit(X_train, y_train, epochs=10, batch_size=32, validation_split=0.2)

loss, mae = model.evaluate(X_test, y_test)
print(f'Test Loss: {loss}, Test MAE: {mae}')

model.save('results/Keras', save_format='tf')

# model_proto, external_tensor_storage = tf2onnx.convert.from_keras(model)

# with open('Assets/results/Keras/model.onnx', 'wb') as f:
#     f.write(model_proto.SerializeToString())