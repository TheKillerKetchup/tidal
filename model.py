from ultralytics import YOLO

if __name__ == "__main__":
    # Load a model
    model = YOLO(r"runs\detect\train3\weights\best.pt")

    # Evaluate model performance on the validation set
    metrics = model.val()

    # Perform object detection on an image
    results = model("test2.jpg")
    results[0].show()
    print(results)
