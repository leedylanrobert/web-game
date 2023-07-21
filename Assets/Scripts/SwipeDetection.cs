using System.Collections;
using UnityEngine;

public class SwipeDetection : MonoBehaviour
{

    [SerializeField]
    private float minimumDistance = .2f;

    [SerializeField]
    private float maximumTime = 1f;

    [SerializeField]
    private GameObject trail;

   private InputManager inputManager;

   public SwipeMove swipeMove;

   private Vector2 startPosition;
   private float startTime;
   private Vector2 endPosition;
   private float endTime;

   private Coroutine coroutine;

   private void Awake()
   {
        inputManager = InputManager.Instance;
   } 

   private void OnEnable()
   {
        inputManager.OnStartTouch += SwipeStart;
        inputManager.OnEndTouch += SwipeEnd;
   }

   private void OnDisable()
   {
        inputManager.OnStartTouch -= SwipeStart;
        inputManager.OnEndTouch -= SwipeEnd;
   }
   
   private void SwipeStart(Vector2 position, float time)
   {
        startPosition = position;
        startTime = time;
        trail.SetActive(true);
        trail.transform.position = position;
        coroutine = StartCoroutine(Trail());
   }

    private void SwipeEnd(Vector2 position, float time)
   {
        trail.SetActive(false);
        StopCoroutine(coroutine);
        endPosition = position;
        endTime = time;
        DetectSwipe();
   }

   private IEnumerator Trail()
   {
     while (true)
     {
          trail.transform.position = inputManager.PrimaryPosition();
          yield return null;
     }
   }

   private void DetectSwipe()
   {
        if (Vector3.Distance(startPosition, endPosition) >= minimumDistance &&
            (endTime - startTime) <= maximumTime)
        {
          // Dylan Code Start
          Debug.Log("Swept!");
          Vector2 spiderPosition = GameObject.Find("Spider").transform.position;
          // RigidBody spiderBody = GameObject.Find("Spider").GetComponent<Rigidbody>();
          Debug.Log("Adding force");
          GameObject.Find("Spider").GetComponent<Rigidbody2D>().AddForce(new Vector2(endPosition.x - startPosition.x, endPosition.y - startPosition.y), ForceMode2D.Impulse);
          Debug.Log("Added force");
          // swipeMove.targetPosition = new Vector2(spiderPosition.x + (endPosition.x - startPosition.x)*2, spiderPosition.y + (endPosition.y - startPosition.y)*2);
          // swipeMove.moving = true;
          // Dylan Code End
            Debug.DrawLine(startPosition, endPosition, Color.red, 5f);
        }
   }
}
