using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    private float moveSpeed = 5f;

    [SerializeField]
    private Rigidbody2D rb;

    Vector2 movement;

    [SerializeField]
    private SpriteRenderer spriteRenderer;
    [SerializeField]
    private Sprite playerIdle;
    [SerializeField]
    private Sprite playerWalkHorizontal;
    [SerializeField]
    private Sprite playerUpIdle;
    [SerializeField]
    private Sprite playerWalkUp1;
    [SerializeField]
    private Sprite playerWalkUp2;
    [SerializeField]
    private Sprite playerWater1;
    [SerializeField]
    private Sprite playerWater2;
    [SerializeField]
    private Sprite playerWater3;

    private bool playAnimation;

    [HideInInspector]
    public bool playerWatering;

    [HideInInspector]
    public bool noWater;
    [HideInInspector]
    public bool wateringHalfDone;
    [HideInInspector]
    public bool wateringDone;
    [HideInInspector]
    public bool destroyProgressBar;

    private bool instantiateProgressBar;

    /*
    [SerializeField]
    private Transform progressBarlol;*/

    [SerializeField]
    private Sprite progressNone;
    [SerializeField]
    private Sprite progressHalf;
    [SerializeField]
    private Sprite progressFull;

    private GameObject progressBar;
    private SpriteRenderer progressBarSpriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        ChangeSprite(playerIdle);
        progressBar = GameObject.Find("progress bar");
        progressBarSpriteRenderer = progressBar.GetComponent<SpriteRenderer>();
        HideProgressBar();
        playAnimation = false;
        playerWatering = false;
        wateringHalfDone = false;
        wateringDone = false;
        instantiateProgressBar = false;
        destroyProgressBar = false;
        noWater = false;
    }

    // Update is called once per frame
    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        if (!playerWatering)
        {
            AnimationHandler();
        }
        else
        {
            if (instantiateProgressBar)
            {
                instantiateProgressBar = false;
                ShowProgressBar();
                //Instantiate(progressBar, new Vector3(gameObject.transform.localPosition.x, gameObject.transform.localPosition.y + 1, 0), Quaternion.identity);
            }
            
            spriteRenderer.sprite = playerWater1;

            if (wateringHalfDone)
            {
                progressBarSpriteRenderer.sprite = progressHalf;
            }

            ShowProgressBar();
        }

        if (noWater)
        {
            progressBarSpriteRenderer.sprite = progressNone;
        }

        if (wateringDone)
        {
            progressBarSpriteRenderer.sprite = progressFull;
        }

        if (destroyProgressBar)
        {
            destroyProgressBar = false;
            HideProgressBar();
        }

        rb.MovePosition(rb.position + movement.normalized * moveSpeed * Time.fixedDeltaTime);
    }
    /*
    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement.normalized * moveSpeed * Time.fixedDeltaTime);
    }*/

    private void HideProgressBar()
    {
        progressBar.transform.localScale = new Vector3(0, 0, 0);
    }

    private void ShowProgressBar()
    {
        progressBar.transform.localScale = new Vector3(1.895536f, 1.611378f, 1);
    }

    private void AnimationHandler()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            transform.localRotation = Quaternion.Euler(0, 180, 0);
            progressBar.transform.localRotation = Quaternion.Euler(0, 180, 0);
            playAnimation = true;
            StartCoroutine(HorizontalWalkingAnimation());
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            transform.localRotation = Quaternion.Euler(0, 0, 0);
            progressBar.transform.localRotation = Quaternion.Euler(0, 0, 0);
            playAnimation = true;
            StartCoroutine(HorizontalWalkingAnimation());
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            playAnimation = true;
            StartCoroutine(HorizontalWalkingAnimation());
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            playAnimation = true;
            StartCoroutine(HorizontalWalkingAnimation());
        }

        if (!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S))
        {
            playAnimation = false;
            StopCoroutine(HorizontalWalkingAnimation());
            //Debug.Log("changing");
            ChangeSprite(playerIdle);
        }
    }

    private void ChangeSprite(Sprite newSprite)
    {
        spriteRenderer.sprite = newSprite;
    }

    private IEnumerator HorizontalWalkingAnimation()
    {
        while (playAnimation)
        {
            ChangeSprite(playerWalkHorizontal);
            yield return new WaitForSeconds(0.2f);
            ChangeSprite(playerIdle);
            yield return new WaitForSeconds(0.2f);
        }
    }

    private IEnumerator UpWalkingAnimation()
    {
        while (playAnimation)
        {
            ChangeSprite(playerWalkUp1);
            yield return new WaitForSeconds(0.2f);
            
            ChangeSprite(playerWalkUp2);
            yield return new WaitForSeconds(0.2f);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "tree")
        {
            //Debug.Log("here");
            playAnimation = false;
            playerWatering = true;
            noWater = true;
            instantiateProgressBar = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        playerWatering = false;
        HideProgressBar();
    }
}
