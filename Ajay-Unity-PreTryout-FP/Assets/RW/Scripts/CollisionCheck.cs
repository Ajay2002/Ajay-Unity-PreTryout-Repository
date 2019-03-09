using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionCheck : MonoBehaviour
{
    
   public GameObject textObject;
   void OnCollisionEnter (Collision collision) {
       if (collision.transform.CompareTag("Bucket")) {
           textObject.SetActive(true);  
       }
    }

}
