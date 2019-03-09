using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundFX : MonoBehaviour
{

    //List of AudioClips, the first is rolling and second is impact
    public List<AudioClip> audioClips = new List<AudioClip>();

    //The AudioSource Component
    public AudioSource source;

    //Correlation between speed and pitch 
    public float pitchVelocityCorrelation;

    //Simple booleans
    bool performingCollisions = false;
    bool ballStopped = false;
    
    void OnCollisionStay (Collision collision) {

        //Check for collision against Ramp or Bucket
        if (collision.transform.CompareTag("Ramp") || collision.transform.CompareTag("Bucket")) {
             
             //If so then play the first audio clip which is the ball rolling SFX
             if (!source.isPlaying) {
                source.clip = audioClips[0];
                source.Play();
            }

            if (!performingCollisions) {
                source.Stop();
                source.pitch = 1.2f; 
                source.clip = audioClips[1];
                source.Play();
            }

            //Manipulate the pitch based on how fast the ball is rolling
            
            source.pitch = Mathf.Clamp(GetComponent<Rigidbody>().velocity.magnitude/pitchVelocityCorrelation,0.6f,2f);
            
            //Tell the game it is rolling
            performingCollisions = true;

           
            
        }
    }

    void OnCollisionExit (Collision collision) {
        //Set 'false' when collisions are being done
        performingCollisions = false;
    }

    void Update() {

        //Check if any collisions occurring, if not disable audio
        if (!performingCollisions) {
            source.Stop();
        }

        //If speed of Rigidbody is less than 0.65 and it's in the bucket then gradually stop the SFX
        if (GetComponent<Rigidbody>().velocity.magnitude < 0.65f & performingCollisions) {
            StartCoroutine(ReduceNoiseGradually(0.003f));
        }
    }

    IEnumerator ReduceNoiseGradually(float speed) {
        
        while (source.volume > 0.05f) {
            source.volume = Mathf.Lerp(source.volume,0,speed);
            
            yield return new WaitForFixedUpdate();

        }

        source.Stop();

    }

}
