import numpy as np
import tensorflow as tf
from tensorflow.keras.models import Sequential
from tensorflow.keras.layers import Dense
import pandas as pd
from sklearn.model_selection import train_test_split
import tf2onnx
import onnx

data = pd.read_csv('Assets/ImitationData/training_datacopy.csv', index_col=False)

X = data.iloc[:, :10].values
y = data.iloc[:, 10].values

# Define and train your model
model = tf.keras.Sequential([
    tf.keras.layers.Dense(32, activation='relu', input_shape=(10,)),
    tf.keras.layers.Dense(16, activation='relu'),
    tf.keras.layers.Dense(1, activation='linear')  # Adjust activation based on task (linear for regression)
])

model.compile(optimizer='adam', loss='mse')  # Use 'mse' for regression

# Train on the entire dataset
model.fit(X, y, epochs=10, batch_size=32, validation_split=0.2)

model.save('results/Keras', save_format='tf')

# model_proto, external_tensor_storage = tf2onnx.convert.from_keras(model)

# with open('Assets/results/Keras/model.onnx', 'wb') as f:
#     f.write(model_proto.SerializeToString())