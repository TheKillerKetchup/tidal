from ultralytics import YOLO
import comet_ml

if __name__ == "__main__":
    comet_ml.login(api_key="Ghhy4MCuIlxwIteUDKJ3yfAni", project_name="tidal",workspace="edwta")
    # Load a model
    model = YOLO(f"C:/Users/edwin/Desktop/tidal/runs/detect/train7/weights/last.pt")
    # Train the model
    train_results = model.train(
        data="taco.yaml",  # path to dataset YAML
        epochs=500,  # number of training epochs
        imgsz=640  # training image size
    )

    # Export the model to pt format
    path = model.export()  # return path to exported model