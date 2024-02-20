using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaiderView : MonoBehaviour
{
    Transform raiderTransform;
    Scanner scanner;

    // Start is called before the first frame update
    void Start()
    {
        raiderTransform = GetComponent<Transform>();
        scanner = GetComponentInParent<Scanner>();

        raiderTransform.localScale = new Vector3(scanner.scanRange * 2, scanner.scanRange * 2, 1);
    }

}
