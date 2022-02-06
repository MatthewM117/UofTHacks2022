using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeManager : MonoBehaviour
{
    private GameManager gameManager;
    
    private SpriteRenderer spriteRenderer;
    [SerializeField]
    private Sprite tree1;
    [SerializeField]
    private Sprite tree2;
    [SerializeField]
    private Sprite tree3;
    [SerializeField]
    private Sprite tree4;

    [SerializeField]
    private Sprite deadTree1;
    [SerializeField]
    private Sprite deadTree2;

    [SerializeField]
    private Sprite superDeadTree1;
    [SerializeField]
    private Sprite superDeadTree2;

    private SpriteRenderer playerSpriteRenderer;
    [SerializeField]
    private Sprite playerWateringSprite;

    [SerializeField]
    private Transform progressBar;

    private SpriteRenderer progressRenderer; 
    [SerializeField]
    private Sprite ProgressZero; 
    [SerializeField]
    private Sprite ProgressHalf; 
    [SerializeField]
    private Sprite ProgressFull;

    private PlayerMovement playerMovement;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        playerSpriteRenderer = GameObject.Find("Player").GetComponent<SpriteRenderer>();
        playerMovement = GameObject.Find("Player").GetComponent<PlayerMovement>();
        progressRenderer = progressBar.GetComponent<SpriteRenderer>();
        PickSprite();
        StartCoroutine(CheckIfTreeWatered());
    }

    private void PickSprite()
    {
        int random = Random.Range(1, 3);
        switch (random)
        {
            case 1:
                ChangeSprite(deadTree1);
                break;
            case 2:
                ChangeSprite(deadTree2);
                break;
        }
    }

    private void PickFinalDeadTreeSprite()
    {
        int random = Random.Range(1, 3);
        switch (random)
        {
            case 1:
                ChangeSprite(superDeadTree1);
                break;
            case 2:
                ChangeSprite(superDeadTree2);
                break;
        }
    }

    private void ChangeSpriteToGreenTree()
    {
        int random = Random.Range(1, 3);
        switch (random)
        {
            case 1:
                ChangeSprite(tree1);
                break;
            case 2:
                ChangeSprite(tree2);
                break;
            case 3:
                ChangeSprite(tree3);
                break;
            case 4:
                ChangeSprite(tree4);
                break;
        }
    }

    private void ChangeSprite(Sprite newSprite)
    {
        spriteRenderer.sprite = newSprite;
    }

    // Update is called once per frame
    void Update()
    {
        // lol test
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log("something hit the tree lol");

        // ensures that no trees spawn on top of each other (this doesnt actually work)
        if (collision.gameObject.tag == "tree")
        {
            Debug.Log("tree hit tree");
            gameManager.SpawnTree();
        }

        if (collision.gameObject.tag == "player")
        {

            //Debug.Log("player hit tree");
            //Destroy(gameObject);
            //gameManager.spawnTree = true;
            StartCoroutine(CheckIfPlayerStayingOnTree());
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "player")
        {
            //Debug.Log("player left");
            playerMovement.playerWatering = false;
            //playerMovement.destroyProgressBar = true;
        }
    }

    private IEnumerator CheckIfPlayerStayingOnTree()
    {
        playerMovement.playerWatering = true;
        playerMovement.noWater = true;
        // Progress bar here
        //Instantiate(progressBar, new Vector3(gameObject.transform.localPosition.x, gameObject.transform.localPosition.y + 1.5f, 0), Quaternion.identity); 
        //progressRenderer.sprite = ProgressZero; 

        for (int i = 0; i < 30; i++)
        {
            if (i == 14)
            {
                playerMovement.noWater = false;
                playerMovement.wateringHalfDone = true;
                //progressRenderer.sprite = ProgressHalf; 
            }

            playerSpriteRenderer.sprite = playerWateringSprite;
            if (i == 29)
            {
                playerMovement.playerWatering = false;
                playerMovement.wateringHalfDone = false;
                playerMovement.wateringDone = true;
                //progressRenderer.sprite = ProgressFull; 
                //Debug.Log("watered");
                ChangeSpriteToGreenTree();
                StartCoroutine(WaitBeforeDestroyingTree());
                break;
            }

            if (!playerMovement.playerWatering)
            {
                //Debug.Log("stopped watering");
                break;
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    private IEnumerator CheckIfTreeWatered()
    {
        yield return new WaitForSeconds(20);
        Debug.Log("not watered in time");
        PickFinalDeadTreeSprite();
        yield return new WaitForSeconds(5);
        Destroy(gameObject);
        gameManager.SpawnTree();
        gameManager.playerScore -= 10;
        //ChangeSprite(deadTree1);
    }

    private IEnumerator WaitBeforeDestroyingTree()
    {
        yield return new WaitForSeconds(2);
        Destroy(gameObject);
        //Destroy(progressBar);
        playerMovement.destroyProgressBar = true;
        playerMovement.wateringDone = false;
        gameManager.spawnTree = true;
        gameManager.playerScore += 3;
    }
}
