using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [Header("Collision")]
    [SerializeField] public LayerMask wallLayer;

    [Header("Life")]
    [SerializeField] private float refillDuration = 0.5f;

    [Header("Audio")]
    [SerializeField] private float startPitch = 1.0f;
    [SerializeField] private float pitchIncrease = 0.2f;

    [Header("Animation")]
    public Sprite activeSprite;
    public Sprite inActiveSprite;
    [SerializeField] private float squashFactor = 0.25f;
    [SerializeField] private float squashDuration = 0.1f;
    private Coroutine squashRoutine;

    private float life;
    private float maxLife;
    private float startLife = 1.0f;

     public bool ignoreInput = false;
    bool madeFirstMove = false;
    bool wonLevel = false;
    private Vector2 startPos;
    int hitCount;

    private Coroutine lifeTimer;

    private Grid tileGrid;
    private Keyboard kb;
    private LevelUI levelUI;
    private SpriteRenderer spriteR;

    private void Awake()
    {
        //spawn = FindObjectOfType<PlayerSpawn>();
        tileGrid = FindObjectOfType<Grid>();
        kb = InputSystem.GetDevice<Keyboard>();
        levelUI = FindObjectOfType<LevelUI>();
        spriteR = GetComponentInChildren<SpriteRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        transform.position = GameManager.Instance.PlaceOnGrid(transform.position);
        startPos = transform.position;
        
        maxLife = startLife + GameData.instance.extraTime;
        life = maxLife;

        ResetPlayer();
    }

    // Update is called once per frame
    void Update()
    {

    }

    #region Input

    void OnLeft()
    {
        if (!ignoreInput) MovePlayer(Vector2.left);
    }

    void OnRight()
    {
        if (!ignoreInput) MovePlayer(Vector2.right);
    }

    void OnUp()
    {
        if (!ignoreInput) MovePlayer(Vector2.up);
    }

    void OnDown()
    {
        if (!ignoreInput) MovePlayer(Vector2.down);
    }

    void OnBack()
    {
        GameManager.Instance.ReturnToMenu();
    }

    #endregion

    void MovePlayer(Vector2 moveDirection)
    {
        if (!madeFirstMove)
        {
            madeFirstMove = true;
            lifeTimer = StartCoroutine(LifeTimer());
        }

        RaycastHit2D hit = Physics2D.Raycast(transform.position, moveDirection, Mathf.Infinity, wallLayer);
        if (hit)
        {
            Vector2 oldPosition = transform.position;
            Vector2 newPosition = hit.point - new Vector2(moveDirection.x * tileGrid.cellSize.x * 0.5f, moveDirection.y * tileGrid.cellSize.y * 0.5f);

            // check if we actually moved
            if (oldPosition != newPosition)
            {
                // move
                transform.position = newPosition;

                // Do squash animation
                StopSquashAnimation();
                squashRoutine = StartCoroutine(SquashAnimation(moveDirection));

                // make sound
                hitCount++;
                string randomImpactSound = "impact_" + Random.Range(1, 4).ToString();
                float risingPitch = startPitch + hitCount * pitchIncrease;
                AudioManager.instance.PlayWithPitch(randomImpactSound, risingPitch);

                // check if hit finish
                if (transform.position == FindObjectOfType<Finish>().transform.position)
                {
                    WinLevel();
                }
            }
            
        }
    }

    IEnumerator LifeTimer()
    {
        while (life > 0f)
        {
            life -= Time.deltaTime;
            //levelUI.SetHealthBarValue(life / maxLife);
            //levelUI.SetTimerText(life);
            yield return null;
        }
        life = 0f;

        //// make sure we always end with exactly 0 life.
        //life = 0f;
        //levelUI.SetHealthBarValue(life / maxLife);
        //levelUI.SetTimerText(life);

        ResetPlayer();

        // play tick sound
        float randomPitch = Random.Range(0.8f, 1.2f);
        AudioManager.instance.PlayWithPitch("tick", randomPitch);
    }

    IEnumerator RefillLife()
    {
        spriteR.sprite = inActiveSprite;
        ignoreInput = true;

        while (life < maxLife)
        {
            life += Time.deltaTime / refillDuration;
            //levelUI.SetHealthBarValue(Mathf.Sin(life / maxLife * Mathf.PI * 0.5f));
            //levelUI.SetTimerText(life);
            yield return null;
        }

        // make sure we always end with exactly max life.
        life = maxLife;
        //levelUI.SetHealthBarValue(life / maxLife);
        //levelUI.SetTimerText(life);

        spriteR.sprite = activeSprite;
        ignoreInput = false;
    }

    public void ResetPlayer()
    {
        // reset player
        transform.position = startPos;
        madeFirstMove = false;
        hitCount = 0;
        StopSquashAnimation();
        StartCoroutine(RefillLife());

        Debug.Log("Resetting Player");
    }

    public void WinLevel()
    {
        wonLevel = true;
        ignoreInput = true;

        // When life is below 0 here the player won during the last execution of the lifeTimer coroutine, but before the routine reached RestartPlayer. Insane!
        if (life < 0)
        {
            Debug.LogWarning("Photo finish! You reached teh goal during the last execution of the timer routine. Epic!");
            // This will be here to prevent confusion by the player.
            life = 0.001f;
        }

        StopCoroutine(lifeTimer);
        Debug.Log(life);

        levelUI.ShowWinScreen();
        levelUI.SetTotalTimeText(maxLife - life, maxLife);
        //GameManager.Instance.LevelWon();

        // make happy sound
        //string randomImpactSound = "win_" + Random.Range(1, 4).ToString();
        //AudioManager.instance.Play(randomImpactSound);
        float randomPitch = Random.Range(0.8f, 1.2f);
        AudioManager.instance.PlayWithPitch("win", randomPitch);

        Debug.Log("Player won!");
    }

    IEnumerator SquashAnimation(Vector2 direction)
    {
        // some vars
        float duration = squashDuration;
        float timer = 0;

        // squash player
        Vector2 squashVector = Vector2.one;
        if (direction.y == 0) squashVector = new Vector2(-1, 1); // horizontal squash
        if (direction.x == 0) squashVector = new Vector2(1, -1); // vertical squash
        Vector2 squashScale = Vector2.one + squashVector * squashFactor;
        spriteR.transform.localScale = squashScale;
        Vector2 squashPosition = Vector2.zero + direction * squashFactor * 0.5f;
        spriteR.transform.localPosition = squashPosition;

        // squash back animation
        while (timer < duration)
        {
            timer += Time.deltaTime;
            float prc = timer / duration;

            spriteR.transform.localPosition = Vector2.Lerp(squashPosition, Vector2.zero, prc);
            spriteR.transform.localScale = Vector2.Lerp(squashScale, Vector2.one, prc);

            yield return null;
        }

        //reset
        spriteR.transform.localPosition = Vector2.zero;
        spriteR.transform.localScale = Vector2.one;
    }

    void StopSquashAnimation()
    {
        if (squashRoutine != null) StopCoroutine(squashRoutine);
        spriteR.transform.localPosition = Vector2.zero;
        spriteR.transform.localScale = Vector2.one;
    }
}
