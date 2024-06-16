import time
import os
import pandas as pd
import tensorflow as tf
from watchdog.observers import Observer
from watchdog.events import FileSystemEventHandler

# Load the pre-trained Keras model
model = tf.keras.models.load_model('results/Keras')

class CSVHandler(FileSystemEventHandler):
    def __init__(self, input_csv, output_csv):
        print("init")
        self.input_csv = os.path.abspath(input_csv)
        self.output_csv = os.path.abspath(output_csv)
        self.last_row = None

    def on_modified(self, event):
        print("modified")
        print(f"Event path: {event.src_path}")
        print(f"Input CSV path: {self.input_csv}")
        if os.path.abspath(event.src_path) == self.input_csv:
            self.process_new_rows()

    def process_new_rows(self):
        print("process")
        df = pd.read_csv(self.input_csv, index_col=False)
        
        # Check for new rows
        if self.last_row is None:
            self.last_row = len(df) - 1

        new_rows = df.iloc[self.last_row + 1:]
        self.last_row = len(df) - 1

        if not new_rows.empty:
            self.make_predictions(new_rows)

    def make_predictions(self, new_rows):
        print("predict")
        # Replace this with your actual feature columns
        feature_columns = ["position.x","position.y","position.z","rotation.x","rotation.y","rotation.z","rotation.w","velocity.x","velocity.y","velocity.z"] 

        # Make predictions
        predictions = model.predict(new_rows[feature_columns])

        # Create a DataFrame to store results
        results_df = pd.DataFrame({
            'input_data': new_rows.values.tolist(),
            'prediction': predictions.flatten()
        })

        # Append predictions to the output CSV
        with open(self.output_csv, 'a', newline='') as f:
            results_df.to_csv(f, header=False, index=False)

def main(input_csv, output_csv):
    event_handler = CSVHandler(input_csv, output_csv)
    observer = Observer()
    observer.schedule(event_handler, path=os.path.dirname(os.path.abspath(input_csv)), recursive=False)
    observer.start()

    try:
        while True:
            time.sleep(1)
    except KeyboardInterrupt:
        observer.stop()
    observer.join()

if __name__ == "__main__":
    input_csv = 'Assets/ImitationData/training_data.csv'
    output_csv = 'Assets/ImitationData/output_data.csv'
    main(input_csv, output_csv)
