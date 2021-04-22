using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageRandomizer : MonoBehaviour
{

    public SpriteRenderer randomSpriteRenderer; //The sprite renderer that will display the random image
    public Sprite[] randomSprites;              //An Array of sprites that will be used for the image randomization

    public float timeBetweenChange = 0.2f;      //The amount of time between switching the image
    public float timeUntilStopping = 3.0f;      //The total amount of time until randomization stops

    private int RandomImageIndex = 0;           //Keep track of which image we're currently displaying
    private float ImageChangeTimer;             //How much time is left until we switch to new image

    // Start is called before the first frame update
    void Start()
    {
        // Set this timer variable as soon as the script starts so it's ready for the update function
        ImageChangeTimer = timeBetweenChange;
    }

    // Update is called once per frame
    void Update()
    {

        // Remove delta time from both active timers. This subtracts a small amount of time from the
        // overall time we described in "timeBetweenChange" and "timeUntilStopping"
        ImageChangeTimer -= Time.deltaTime;
        timeUntilStopping -= Time.deltaTime;

        // If we've fully run out of time we need to select a final random image
        if (timeUntilStopping <= 0.0f)
        {
            // The final image will be selected at random
            randomSpriteRenderer.sprite = randomSprites[Random.Range(0, randomSprites.Length)];

            // Destroy this script immediately stopping it from running anymore in the future
            DestroyImmediate(this);

            // Return so no other code runs in this Update function call
            return;
        }

        // If RandomizationTimer is less than or equal to zero it's time for a new image
        if (ImageChangeTimer <= 0.0f)
        {
            // To ensure we see all the images we increase the RandomImageIndex to see the next
            // image in the array
            RandomImageIndex++;

            // If our index has gone past the end of the array, reset it to zero so the cycle
            // can start again
            if (RandomImageIndex >= randomSprites.Length)
            {
                RandomImageIndex = 0;
            }

            //Assign the new sprite to the sprite renderer
            randomSpriteRenderer.sprite = randomSprites[RandomImageIndex];

            //Reset the "RandomizationTimer" to start counting down again
            ImageChangeTimer = timeBetweenChange;
        }
    }

}
