using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RectangleSizer : MonoBehaviour
{
    public RectTransform rectTransform;
    // Start is called before the first frame update
    void Start()
    {
        int width = Screen.width;
        int height = Screen.height;

        print("Screen size: " + width + "x" + height);

        if (width > height)
        {
            rectTransform.sizeDelta = new Vector2(((height / 3) * 4), height);
            print("Rectangle size:" + ((height / 3) * 4) + "x" + height);
        }

        if (height > width)
        {
            rectTransform.sizeDelta = new Vector2(width, ( (width / 4) * 3 ));
            print("Rectangle size:" + width + "x" + ((width / 4) * 3));
        }

    }

    // Update is called once per frame
    void Update()
    {
        int width = Screen.width;
        int height = Screen.height;

        if (width > height)
        {
            rectTransform.sizeDelta = new Vector2(((height / 3) * 4), height);
        }

        if (height > width)
        {
            rectTransform.sizeDelta = new Vector2(width, ((width / 4) * 3));
        }
    }
}
