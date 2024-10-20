import numpy as np
import cv2
import os

def letterbox_image(img, size):
    h, w = img.shape[:2]
    c = img.shape[2] if len(img.shape)>2 else 1

    if h == w: 
        return cv2.resize(img, size, cv2.INTER_AREA)

    dif = h if h > w else w

    interpolation = cv2.INTER_AREA if dif > (size+size)//2 else cv2.INTER_CUBIC

    x_pos = (dif - w)//2
    y_pos = (dif - h)//2

    if len(img.shape) == 2:
        mask = np.zeros((dif, dif), dtype=img.dtype)
        mask[y_pos:y_pos+h, x_pos:x_pos+w] = img[:h, :w]
    else:
        mask = np.zeros((dif, dif, c), dtype=img.dtype)
        mask[y_pos:y_pos+h, x_pos:x_pos+w, :] = img[:h, :w, :]

    return cv2.resize(mask, [size, size], interpolation)

def resize_images(input_dir, output_dir, size):
    """Resizes all images in the input directory and saves them to the output directory."""

    if not os.path.exists(output_dir):
        os.makedirs(output_dir)

    for filename in os.listdir(input_dir):
        if filename.endswith(('.jpg', '.jpeg', '.png', '.bmp')):
            img_path = os.path.join(input_dir, filename)
            img = cv2.imread(img_path)

            if img is not None:
                resized_img = letterbox_image(img, size)
                output_path = os.path.join(output_dir, filename)
                cv2.imwrite(output_path, resized_img)

if __name__ == "__main__":
    input_dir = "images"
    output_dir = "images"
    size = 640
    resize_images(input_dir, output_dir, size)