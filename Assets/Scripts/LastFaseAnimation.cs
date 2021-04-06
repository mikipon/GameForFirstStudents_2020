using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LastFaseAnimation : MonoBehaviour
{
    Rigidbody2D rig;
    StickerManager stManager;
    AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        this.stManager = GameObject.Find("Manager").GetComponent<StickerManager>();
        this.rig = this.GetComponent<Rigidbody2D>();
        this.rig.velocity = new Vector2(10, 0);
        this.audioSource = this.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(this.transform.position.x > 20)
        {
            Destroy(this.gameObject);
            this.stManager.ResetSticker();
        }
    }
}
