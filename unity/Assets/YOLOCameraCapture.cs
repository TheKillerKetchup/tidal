using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;  // For sending HTTP requests

public class YOLOCameraCapture : MonoBehaviour
{
    public Camera cameraToCapture;
    private Texture2D screenShot;

    void Start()
    {
        StartCoroutine(CaptureAndSendImage());
    }

    IEnumerator CaptureAndSendImage()
    {
        while (true)
        {
            yield return new WaitForEndOfFrame();  // Wait until the frame rendering is done

            // Capture the frame from the camera
            RenderTexture renderTexture = new RenderTexture(Screen.width, Screen.height, 24);
            cameraToCapture.targetTexture = renderTexture;
            Texture2D texture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);

            cameraToCapture.Render();
            RenderTexture.active = renderTexture;
            texture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
            texture.Apply();

            // Convert the texture to PNG
            byte[] imageBytes = texture.EncodeToPNG();

            // Clean up
            cameraToCapture.targetTexture = null;
            RenderTexture.active = null;
            Destroy(renderTexture);

            // Send image to the YOLO API
            yield return StartCoroutine(SendImageToAPI(imageBytes));
        }
    }

    IEnumerator SendImageToAPI(byte[] imageData)
    {
        // Prepare form for POST request
        WWWForm form = new WWWForm();
        form.AddBinaryData("image", imageData, "frame.png", "image/png");

        // Send POST request
        using (UnityWebRequest www = UnityWebRequest.Post("http://127.0.0.1:5000/detect", form))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error while sending image: " + www.error);
            }
            else
            {
                // Process the response (bounding box data)
                Debug.Log("Response: " + www.downloadHandler.text);

                // Parse and handle bounding boxes from the response
                HandleBoundingBoxResponse(www.downloadHandler.text);
            }
        }
    }
    public MovingToTrash movingtotrash;
    void HandleBoundingBoxResponse(string jsonResponse)


    {
        // Parse the JSON response to extract bounding box information
        // and update the UI or overlay in Unity.
        Debug.Log("Bounding boxes: " + jsonResponse);

    }
}
