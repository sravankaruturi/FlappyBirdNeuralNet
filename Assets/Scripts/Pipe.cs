using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pipe : MonoBehaviour
{
    [SerializeField]
    float leftBarrierX;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        this.transform.Translate(new Vector3(-1 * Time.deltaTime, 0, 0));

        if ( this.transform.position.x < leftBarrierX)
        {
            Destroy(this.gameObject);
        }
        
    }
}
