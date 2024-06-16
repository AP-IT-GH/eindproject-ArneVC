import numpy as np
import tensorflow as tf
from tensorflow.keras.models import Sequential
from tensorflow.keras.layers import Dense
import pandas as pd
from sklearn.model_selection import train_test_split


# Load and preprocess your exported demonstration data
# Load data from CSV
data = pd.read_csv('Assets/ImitationData/training_data.csv')

# Extract features and labels
X = data.iloc[:, :10].values  # First 10 columns as features
y = data.iloc[:, 10].values   # 11th column as label

# Split data into training and testing sets
X_train, X_test, y_train, y_test = train_test_split(X, y, test_size=0.2, random_state=42)

# Define and train your behavioral cloning model
def create_model():
    model = Sequential([
        Dense(32, activation='relu', input_shape=(10,)),  # 10 features
        Dense(16, activation='relu'),
        Dense(1, activation='linear')  # Assuming regression; use 'softmax' if classification with multiple classes
    ])
    model.compile(optimizer='adam', loss='mse', metrics=['mae'])  # 'mse' for regression; use 'sparse_categorical_crossentropy' for classification
    return model

# Create and train the model
model = create_model()
model.fit(X_train, y_train, epochs=10, batch_size=32, validation_split=0.2)

# Evaluate the model
loss, mae = model.evaluate(X_test, y_test)
print(f'Test Loss: {loss}, Test MAE: {mae}')

# Save the model in TensorFlow SavedModel format
model.save('results/Keras', save_format='tf')