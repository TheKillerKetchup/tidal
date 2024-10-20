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
    print(image)
    
    # Convert image to numpy array
    img_np = np.array(image)

    # Run YOLO inference
    results = model(img_np)

    # Get bounding boxes
    dummy ={
        "boxes":[
            {"x1": 0.5052, "y1": 0.5763, "x2": 0.0088, "y2": 0.0446},
            {"x1": 0.5, "y1": 0.5, "x2": 0.7, "y2": 0.7},
            {"x1": 0.1, "y1": 0.1, "x2": 0.9, "y2": 0.9}
        ]
    }
    bounding_box_list = []
    for result in results: 
        if result.boxes.xyxyn.size(0) > 0:
            bounding_box = result.boxes.xyxyn[0].tolist()  # [x1, y1, x2, y2]
            bounding_box_list.append(bounding_box)
        #else:
            #
    
   
    return jsonify(dummy)
    #return jsonify(bounding_box_list)

if __name__ == '__main__':
    app.run()
