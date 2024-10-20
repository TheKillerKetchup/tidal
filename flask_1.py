from flask import Flask, request, jsonify
import numpy as np
import cv2
from PIL import Image
import io
from ultralytics import YOLO # Assuming you are using PyTorch for YOLO
from flask_cors import CORS

app = Flask(__name__)
CORS(app)

# Load your YOLO model (this could be YOLOv5, v8, etc.)

# Load a model
model = YOLO("yolo11n.pt")

#Train the model


@app.route('/detect', methods=['POST'])
def detect():
    # Get image from the request
    image_file = request.files['image'].read()
    image = Image.open(io.BytesIO(image_file))
    
    # Convert image to numpy array
    img_np = np.array(image)

    # Run YOLO inference
    results = model(img_np)

    # Get bounding boxes
    #boxes = results.xyxy[0].tolist()  # [x1, y1, x2, y2, confidence, class]
    print(results[0].boxes)
    
    return jsonify({"message": "Hello from CORS-enabled Flask!"})
    #return jsonify(boxes)

if __name__ == '__main__':
    app.run()
