using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressBarManager : MonoBehaviour
{

    private SpriteRenderer spriteRenderer;
    [SerializeField]
    private Sprite ProgressZero;
    [SerializeField]
    private Sprite ProgressHalf;
    [SerializeField]
    private Sprite ProgressFull;

    private PlayerMovement pm;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        pm = GameObject.Find("Player").GetComponent<PlayerMovement>();
        spriteRenderer.sprite = ProgressZero;
    }

    // Update is called once per frame
    void Update()
    {
        if (pm.noWater)
        {
            spriteRenderer.sprite = ProgressZero;
        }

        if (pm.wateringHalfDone)
        {
            ChangeToProgressHalf();
        }

        if (pm.wateringDone)
        {
            ChangeToProgressFull();
        }

        if (pm.destroyProgressBar)
        {
            pm.destroyProgressBar = false;
            Destroy(gameObject);
        }
    }

    private void ChangeToProgressZero()
    {
        spriteRenderer.sprite = ProgressZero;
    }

    private void ChangeToProgressHalf()
    {
        spriteRenderer.sprite = ProgressHalf;
    }

    private void ChangeToProgressFull()
    {
        spriteRenderer.sprite = ProgressFull;
    }
}
