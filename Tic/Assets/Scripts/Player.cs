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

    private float life;
    private float maxLife;
    private float startLife = 1.0f;

    private bool ignoreInput = false;
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
        spriteR = GetComponent<SpriteRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        transform.position = GameManager.Instance.PlaceOnGrid(transform.position);
        startPos = transform.position;
        
        maxLife = startLife + GameData.instance.extraTime;
        life = maxLife;

        RestartPlayer();
    }

    // Update is called once per frame
    void Update()
    {
        if (!wonLevel && !ignoreInput)
        {
            //            WASD                           IJKL                           Arrows
            bool left   = kb.aKey.wasPressedThisFrame || kb.jKey.wasPressedThisFrame || kb.leftArrowKey.wasPressedThisFrame;
            bool right  = kb.dKey.wasPressedThisFrame || kb.lKey.wasPressedThisFrame || kb.rightArrowKey.wasPressedThisFrame;
            bool up     = kb.wKey.wasPressedThisFrame || kb.iKey.wasPressedThisFrame || kb.upArrowKey.wasPressedThisFrame;
            bool down   = kb.sKey.wasPressedThisFrame || kb.kKey.wasPressedThisFrame || kb.downArrowKey.wasPressedThisFrame;

            if (left)   MovePlayer(Vector2.left);
            if (right)  MovePlayer(Vector2.right);
            if (up)     MovePlayer(Vector2.up);
            if (down)   MovePlayer(Vector2.down);
        }
    }

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
            Vector2 newPosition = hit.point - new Vector2(moveDirection.x * tileGrid.cellSize.x * 0.5f, moveDirection.y * tileGrid.cellSize.y * 0.5f);

            // check if we actually moved
            if ((Vector2)transform.position != newPosition)
            {
                // move
                transform.position = newPosition;

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
            levelUI.SetHealthBarValue(life / maxLife);
            levelUI.SetTimerText(life);
            yield return null;
        }

        // make sure we always end with exactly 0 life.
        life = 0f;
        levelUI.SetHealthBarValue(life / maxLife);
        levelUI.SetTimerText(life);

        RestartPlayer();

        // play tick sound
        float randomPitch = Random.Range(0.8f, 1.2f);
        AudioManager.instance.PlayWithPitch("tick", randomPitch);
    }

    IEnumerator RefillLife()
    {
        while (life < maxLife)
        {
            life += Time.deltaTime / refillDuration;
            levelUI.SetHealthBarValue(Mathf.Sin(life / maxLife * Mathf.PI * 0.5f));
            levelUI.SetTimerText(life);
            yield return null;
        }

        // make sure we always end with exactly max life.
        life = maxLife;
        levelUI.SetHealthBarValue(life / maxLife);
        levelUI.SetTimerText(life);

        spriteR.sprite = activeSprite;
        ignoreInput = false;
    }

    public void RestartPlayer()
    {
        // reset player
        transform.position = startPos;
        madeFirstMove = false;
        hitCount = 0;
        // ignore input until life is refilled. RefillLife routine sets ignoreInput true
        spriteR.sprite = inActiveSprite;
        ignoreInput = true;
        StartCoroutine(RefillLife());

        Debug.Log("Player died");
    }

    public void WinLevel()
    {
        wonLevel = true;

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

        // make happy sound
        //string randomImpactSound = "win_" + Random.Range(1, 4).ToString();
        //AudioManager.instance.Play(randomImpactSound);
        float randomPitch = Random.Range(0.8f, 1.2f);
        AudioManager.instance.PlayWithPitch("win", randomPitch);

        Debug.Log("Player won!");
    }
}
