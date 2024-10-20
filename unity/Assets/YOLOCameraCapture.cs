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
    public GameObject boundingBoxPrefab;
    public GameObject panel;
    private List<GameObject> currentBoundingBoxes;

    void Start()
    {
        currentBoundingBoxes = new List<GameObject>();
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
        currentBoundingBoxes = new List<GameObject>();
        // Parse the JSON response to extract bounding box information
        // and update the UI or overlay in Unity.
        //Debug.Log("Bounding boxes: " + jsonResponse);
        BoundingBoxList boundingBoxList = JsonUtility.FromJson<BoundingBoxList>(jsonResponse);
        for(int i = 0;i < boundingBoxList.boxes.Length; i++)
        {
            BoundingBox bounding_box = boundingBoxList.boxes[i];
            DrawBoundingBox(bounding_box.x1, bounding_box.y1, bounding_box.x2, bounding_box.y2, i);
        }
        //Debug.Log("Bounding box data: " + boundingBoxList);
    }
    void DrawBoundingBox(float x_min_n, float y_min_n, float x_max_n, float y_max_n, int i)
    {
        // Convert the received bounding box to Viewport space (0 to 1 range)
        Vector3 bottomLeft = cameraToCapture.ScreenToViewportPoint(new Vector3(x_min_n * Screen.width, y_min_n * Screen.height, cameraToCapture.nearClipPlane));
        Vector3 topRight = cameraToCapture.ScreenToViewportPoint(new Vector3(x_max_n * Screen.width, y_max_n * Screen.height, cameraToCapture.nearClipPlane));

        // Calculate center and size of the bounding box
        Vector3 center = (bottomLeft + topRight) / 2;
        Vector3 size = topRight - bottomLeft;

        // If no bounding box exists, create one
        if(currentBoundingBoxes.Count <= i)
        {
            currentBoundingBoxes.Add(Instantiate(boundingBoxPrefab, panel.transform));
        }
        else if (currentBoundingBoxes[i] == null)
        {
            currentBoundingBoxes[i] = Instantiate(boundingBoxPrefab, panel.transform);
        }

        // Set position and size of the bounding box
        Debug.Log(currentBoundingBoxes[i]);
        RectTransform rectTransform = currentBoundingBoxes[i].GetComponent<RectTransform>();
        rectTransform.anchorMin = bottomLeft;
        rectTransform.anchorMax = topRight;
        rectTransform.sizeDelta = new Vector2(size.x, size.y);
        Debug.Log(rectTransform.anchorMin + " " + rectTransform.anchorMax);
    }
}
[System.Serializable]
public class BoundingBox
{
    public float x1, y1, x2, y2;
}

[System.Serializable]
public class BoundingBoxList
{
    public BoundingBox[] boxes;
}
