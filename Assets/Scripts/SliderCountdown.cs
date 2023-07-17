using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderCountdown : MonoBehaviour
{
    public float value = 0f;
    public float actualValue = 0f;
    GameObject timer;
    public bool startCount = false;
    public Vector3 position;

    int lastTouchCount = 0;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        // value = 0f;
        if (startCount)
        {
            if (actualValue < 1f)
            {
                if (lastTouchCount == GameObject.Find("Spider").GetComponent<Dash>().touchCount)
                {
                    actualValue += Time.deltaTime * 0.75f;
                }
                else
                {
                    actualValue = 0f;
                }

                this.gameObject.GetComponent<Slider>().value = actualValue;
            }
            else
            {
                GameObject.Find("Spider").GetComponent<Dash>().timesUp = true;
                this.gameObject.transform.GetChild(1).GetChild(0).GetComponent<Image>().color = Color.red;
            }
            this.gameObject.transform.position = position;
        }
        lastTouchCount = GameObject.Find("Spider").GetComponent<Dash>().touchCount;
    }
}
