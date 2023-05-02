using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Circle : MonoBehaviour
{
    public LineRenderer circleRenderer;

    // Start is called before the first frame update
    void Start()
    {
        circleRenderer.material = new Material(Shader.Find("Sprites/Default"));
        circleRenderer.SetColors(Color.red, Color.red);
        DrawCircle(100, 20);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void DrawCircle(int steps, float radius)
    {
        circleRenderer.positionCount = steps;
        
        for (int currentStep = 0; currentStep < steps; currentStep++)
        {
            float circumreferenceProgress = (float) currentStep / steps;

            float currentRadian = circumreferenceProgress * 2 * Mathf.PI;

            float xScaled = Mathf.Cos(currentRadian);
            float yScaled = Mathf.Sin(currentRadian);

            float x = xScaled * radius;
            float y = yScaled * radius;

            Vector3 currentPosition = new Vector3(x, y, 0);

            circleRenderer.SetPosition(currentStep, currentPosition);
        }
    }
}
