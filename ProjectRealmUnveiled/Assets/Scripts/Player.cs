using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;


public class Player : MonoBehaviour
{
    [Header("Horizontal Movement Settings:")]
    [SerializeField] private float walkSpeed = 1; //sets the players movement speed on the ground
    [Space(5)]



    [Header("Vertical Movement Settings")]
    [SerializeField] private float jumpForce = 45f; //sets how hight the player can jump
    private int jumpBufferCounter = 0; //stores the jump button input
    [SerializeField] private int jumpBufferFrames; //sets the max amount of frames the jump buffer input is stored
    private float coyoteTimeCounter = 0; //stores the Grounded() bool
    [SerializeField] private float coyoteTime; ////sets the max amount of frames the Grounded() bool is stored
    private int airJumpCounter = 0; //keeps track of how many times the player has jumped in the air
    [SerializeField] private int maxAirJumps; //the max no. of air jumps
    private float gravity; //stores the gravity scale at start
    [Space(5)]

    [Header("Wall Jump Settings")]
    [SerializeField] private float wallSlidingSpeed = 2f;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private float wallJumpingDuration;
    [SerializeField] private Vector2 wallJumpingPower;
    float wallJumpingDirection;
    bool isWallSliding;
    bool isWallJumping;
    [Space(5)]

    [Header("Ground Check Settings:")]
    [SerializeField] private Transform groundCheckPoint; //point at which ground check happens
    [SerializeField] private float groundCheckY = 0.5f; //how far down from ground chekc point is Grounded() checked
    [SerializeField] private float groundCheckX = 1f; //how far horizontally from ground chekc point to the edge of the player is
    [SerializeField] private LayerMask whatIsGround; //sets the ground layer
    [Space(5)]


    [Header("Dash Settings")]
    [SerializeField] private float dashSpeed; //speed of the dash
    [SerializeField] private float dashTime; //amount of time spent dashing
    [SerializeField] private float dashCooldown; //amount of time between dashes
    [SerializeField] GameObject dashEffect;
    private bool canDash = true, dashed;
    [Space(5)]



    [Header("Attack Settings:")]
    [SerializeField] private Transform SideAttackTransform; //the middle of the side attack area
    [SerializeField] private Vector2 SideAttackArea; //how large the area of side attack is
    [SerializeField] private LayerMask attackableLayer; //the layer the player can attack and recoil off of
    [SerializeField] private float timeBetweenAttack;
    private float timeSinceAttck;
    [SerializeField] private float damage;
    [SerializeField] private GameObject slashEffect;
    bool restoreTime;
    float restoreTimeSpeed;
    [Space(5)]


    [Header("Recoil Settings:")]
    [SerializeField] private int recoilXSteps = 5;
    [SerializeField] private float recoilXSpeed = 100; //the speed of horizontal recoil
    private int stepsXRecoiled;
    [Space(5)]


    [Header("Health Settings")]
    public int health;
    public int maxHealth;
    public int maxTotalHealth = 6;
    public int heartShards;
    [SerializeField] GameObject bloodSpurt;
    [SerializeField] float hitFlashSpeed;
    public delegate void OnHealthChangedDelegate();
    [HideInInspector] public OnHealthChangedDelegate onHealthChangedCallback;
    float healTimer;
    [SerializeField] float timeToHeal;
    [Space(5)]

    [Header("Mana Settings")]
    [SerializeField] UnityEngine.UI.Image manaStorage;
    [SerializeField] float mana;
    [SerializeField] float manaDrainSpeed;
    [SerializeField] float manaGain;
    public bool halfMana;
    [Space(5)]

    [Header("Camera Stuff")]
    [SerializeField] private float playerFallSpeedThtreshold = -10;
    /*[SerializeField] private GameObject _cameraFollowGo;

    private CameraFollowObject _cameraFollowObject;*/
    [Space(5)]

    [Header("Audio")]
    [SerializeField] AudioClip jumpSound;
    [SerializeField] AudioClip hitSound;
    [SerializeField] AudioClip dashSound;
    [SerializeField] AudioClip attackSound;
    [SerializeField] AudioClip deathSound;
    [SerializeField] AudioClip footstepsSound;
    [SerializeField] AudioClip healSound;


    [HideInInspector] public AlexStateList aState;
    private Animator anim;
    public Rigidbody2D rb;
    private SpriteRenderer sr;
    private AudioSource audioSource;
    private AudioSource healSource;
    private float nextFootstepTime;

    //Input Variables
    private float xAxis, yAxis;
    private bool attack = false;
    public float footstepInterval = 0f;

    //Heal Variables
    private bool canFlash = true;
    public bool deathCauseSpike = false;

    public static Player Instance;

    //unlocking
    [HideInInspector] public bool unlockedWallJump;
    [HideInInspector] public bool unlockedDash;
    [HideInInspector] public bool unlockedVarJump;

    [HideInInspector] public bool isFacingRight;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
        DontDestroyOnLoad(gameObject);
        
        
    }


    // Start is called before the first frame update
    void Start()
    {
        aState = GetComponent<AlexStateList>();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        healSource = GetComponent<AudioSource>();
        /*audioSource = gameObject.AddComponent<AudioSource>();*/
        healSource.loop = true; // Mengatur AudioSource agar dapat diulang (loop)
        healSource.clip = healSound;

        
        /*_cameraFollowObject = _cameraFollowGo.GetComponent<CameraFollowObject>();*/
        SaveData.Instance.LoadPlayerData();

        gravity = rb.gravityScale;
        Mana = mana;
        manaStorage.fillAmount = Mana;
        

        if (halfMana)
        {
            UIManager.Instance.SwitchMana(UIManager.ManaState.HalfMana);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(SideAttackTransform.position, SideAttackArea);

        Debug.DrawRay(groundCheckPoint.position, Vector2.down * groundCheckY, Grounded() ? Color.green : Color.red);
        Debug.DrawRay(groundCheckPoint.position + new Vector3(groundCheckX, 0, 0), Vector2.down * groundCheckY, Grounded() ? Color.green : Color.red);
        Debug.DrawRay(groundCheckPoint.position + new Vector3(-groundCheckX, 0, 0), Vector2.down * groundCheckY, Grounded() ? Color.green : Color.red);

    }

    // Update is called once per frame
    void Update()
    {
        
        if (pauseMenu.isPaused) return;
        //TestCameraShake();
        //ResetPlayerData();
        //UnlockJumpDash();
        if (aState.cutscene) return;
        if(aState.alive)
        {
            GetInputs();
        }

        RestoreTimeScale();
        UpdateJumpVariables();
        UpdateCameraYDampForPlayerFall();

        if (aState.dashing ) return;
        if (aState.alive)
        {
            if(DialogueManager.Instance.isDialogueActive)
            {
                DialogueActive();
                anim.SetBool("Dialogue", true);
            }
            else if (!DialogueManager.Instance.isDialogueActive)
            {
                anim.SetBool("Dialogue", false);
                if (!isWallJumping)
                {
                    Facing();
                    Move();
                    Jump();
                }

                if (unlockedWallJump)
                {
                    WallSlide();
                    WallJump();
                }

                if (unlockedDash)
                {
                    StartDash();
                }


                Attack();
                Heal();
            }         
        }        
        FlashWhileInvincible();

        

    }
    void DialogueActive()
    {
        rb.velocity= new Vector2(0 , 0);
        
    }
    void TestCameraShake()
    {
        if(Input.GetKeyDown(KeyCode.S))
        {
            CameraShake.instance.ShakeCamera(1f, 0.1f);
        }
    }
    private void FixedUpdate()
    {
        if(aState.cutscene) return;

        if (aState.dashing) return;
        Recoil();
    }

    void GetInputs()
    {
        xAxis = Input.GetAxisRaw("Horizontal");
        yAxis = Input.GetAxisRaw("Vertical");
        attack = Input.GetButtonDown("Attack");
        
        if ((xAxis != 0f || yAxis != 0f) && Time.time >= nextFootstepTime)
        {
            PlayFootstepSound();
            nextFootstepTime = Time.time + footstepInterval;
        }
    }

    void PlayFootstepSound()
    {
        // Memeriksa apakah audio source sudah tersedia
        if (audioSource != null && footstepsSound != null)
        {
            // Memainkan suara langkah
            audioSource.PlayOneShot(footstepsSound);
        }
    }

    void Facing()
    {
        if (xAxis < 0)
        {
            Vector3 rotator = new Vector3(transform.rotation.x, 180f, transform.rotation.z);
            transform.rotation = Quaternion.Euler(rotator);
            aState.lookingRight = false;
            /*isFacingRight = aState.lookingRight;*/
            //_cameraFollowObject.CallTurn();
        }
        else if (xAxis > 0)
        {
            Vector3 rotator = new Vector3(transform.rotation.x, 0f, transform.rotation.z);
            transform.rotation = Quaternion.Euler(rotator);
            aState.lookingRight = true;
            /*isFacingRight = aState.lookingRight;*/
            //_cameraFollowObject.CallTurn();
        }
    }

    private void ResetPlayerData()
    {
        if(Input.GetKeyDown(KeyCode.O))
        {
            Debug.Log("Ability berhasil di reset !");
            unlockedDash = false;
            unlockedVarJump = false;
            unlockedWallJump = false;
            maxHealth = 5;
            heartShards = 0;
            
  
        }
    }

    private void UnlockJumpDash()
    {
        if(Input.GetKeyDown(KeyCode.J))
        {
            unlockedDash = true;
            unlockedVarJump = true;
        }
        
    }
    
    private void Move()
    {
        rb.velocity = new Vector2(walkSpeed * xAxis, rb.velocity.y);
        anim.SetBool("Walking", rb.velocity.x != 0 && Grounded());
    }

    void UpdateCameraYDampForPlayerFall()
    {
        //if falling past a certain speed threshold
        if(rb.velocity.y < playerFallSpeedThtreshold && !CameraManager.Instance.isLerpingYDamping && !CameraManager.Instance.hasLerpedYDamping)
        {
            StartCoroutine(CameraManager.Instance.LerpYDamping(true));
        }
        //if standing still or moving up
        if(rb.velocity.y >= 0 && !CameraManager.Instance.isLerpingYDamping && CameraManager.Instance.hasLerpedYDamping)
        {
            //reset camera function
            CameraManager.Instance.hasLerpedYDamping = false;
            StartCoroutine(CameraManager.Instance.LerpYDamping(false));
        }
    }
    void StartDash()
    {
        if (Input.GetButtonDown("Dash") && canDash && !dashed)
        {
            StartCoroutine(Dash());
            dashed = true;
        }

        if (Grounded())
        {
            dashed = false;
        }
    }

    IEnumerator Dash()
    {
        canDash = false;
        aState.dashing = true;
        anim.SetTrigger("Dashing");
        audioSource.PlayOneShot(dashSound);
        rb.gravityScale = 0;
        int _dir = aState.lookingRight ? 1 : -1;
        rb.velocity = new Vector2(_dir * dashSpeed, 0);
        if (Grounded()) Instantiate(dashEffect, transform);
        yield return new WaitForSeconds(dashTime);
        rb.gravityScale = gravity;
        aState.dashing = false;
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    void Attack()
    {
        timeSinceAttck += Time.deltaTime;
        if (attack && timeSinceAttck >= timeBetweenAttack)
        {
            timeSinceAttck = 0;
            anim.SetTrigger("Attack");
            audioSource.PlayOneShot(attackSound);
            Instantiate(slashEffect, SideAttackTransform);

            if (yAxis == 0 || yAxis < 0 && Grounded())
            {
                int _recoilLeftOrRight = aState.lookingRight ? 1 : -1;

                Hit(SideAttackTransform, SideAttackArea, ref aState.recoilingX,Vector2.right * _recoilLeftOrRight ,recoilXSpeed);
                HitBoss(SideAttackTransform, SideAttackArea, ref aState.recoilingX, Vector2.right * _recoilLeftOrRight, recoilXSpeed);

            }
            
        }
    }

    void Hit(Transform _attackTransform, Vector2 _attackArea,  ref bool _recoilBool, Vector2 _recoilDir, float _recoilStrength)
    {
        Collider2D[] objectsToHit = Physics2D.OverlapBoxAll(_attackTransform.position, _attackArea, 0, attackableLayer);

        if (objectsToHit.Length > 0)
        {
            _recoilBool = true;
        }

        for (int i = 0; i < objectsToHit.Length; i++)
        {
            if (objectsToHit[i].GetComponent<Enemy>() != null)
            {
                objectsToHit[i].GetComponent<Enemy>().EnemyGetsHit(damage, _recoilDir, _recoilStrength);
                audioSource.PlayOneShot(hitSound);

                if (objectsToHit[i].CompareTag("Enemy"))
                {
                    Mana += manaGain;
                }
            }

        }

    }

    void HitBoss(Transform _attackTransform, Vector2 _attackArea, ref bool _recoilBool, Vector2 _recoilDir, float _recoilStrength)
    {
        Collider2D[] objectsToHit = Physics2D.OverlapBoxAll(_attackTransform.position, _attackArea, 0, attackableLayer);

        if (objectsToHit.Length > 0)
        {
            _recoilBool = true;
        }

        for (int i = 0; i < objectsToHit.Length; i++)
        {
            if (objectsToHit[i].GetComponent<GruzMother>() != null)
            {
                objectsToHit[i].GetComponent<GruzMother>().EnemyGetsHit(damage, _recoilDir, _recoilStrength);
                audioSource.PlayOneShot(hitSound);

                if (objectsToHit[i].CompareTag("GruzMother"))
                {
                    Mana += manaGain;
                }
            }
        }

    }

    void Recoil()
    {
        if (aState.recoilingX)
        {
            if (aState.lookingRight)
            {
                rb.velocity = new Vector2(-recoilXSpeed, 0);
            }
            else
            {
                rb.velocity = new Vector2(recoilXSpeed, 0);
            }
        }

        if (aState.recoilingX && stepsXRecoiled < recoilXSteps)
        {
            stepsXRecoiled++;
        }
        else
        {
            StopRecoilX();
        }
    }

    void StopRecoilX()
    {
        stepsXRecoiled = 0;
        aState.recoilingX = false;
    }

    public void TakeDamage(float _damage)
    {
        if(aState.alive)
        {
            audioSource.PlayOneShot(hitSound);

            Health -= Mathf.RoundToInt(_damage);
            if(Health <= 0)
            {
                Health = 0;
                StartCoroutine(Death());

            }
            else
            {
                StartCoroutine(StopTakingDamage());
            }
        }
    }


    IEnumerator StopTakingDamage()
    {
        aState.invincible = true;
        GameObject _bloodSpurtParticles = Instantiate(bloodSpurt, transform.position, Quaternion.identity);
        Destroy(_bloodSpurtParticles, 1.5f);
        anim.SetTrigger("TakeDamage");
        
        yield return new WaitForSeconds(1f);
        aState.invincible = false;
    }

    void RestoreTimeScale()
    {
        if (restoreTime)
        {
            if (Time.timeScale < 1)
            {
                Time.timeScale += Time.deltaTime * restoreTimeSpeed;
            }
            else
            {
                Time.timeScale = 1;
                restoreTime = false;
            }
        }
    }

    public void HitStopTime(float _newTimeScale, int _restoreSpeed, float _delay)
    {
        restoreTimeSpeed = _restoreSpeed;
        if (_delay > 0)
        {
            StopCoroutine(StartTimeAgain(_delay));
            StartCoroutine(StartTimeAgain(_delay));
        }
        else
        {
            restoreTime = true;
        }
        Time.timeScale = _newTimeScale;
    }

    IEnumerator StartTimeAgain(float _delay)
    {
        restoreTime = true;
        yield return new WaitForSeconds(_delay);
    }


    IEnumerator Death()
    {
        aState.alive = false;
        Time.timeScale = 1;
        GameObject _bloodSpurtParticles = Instantiate(bloodSpurt, transform.position, Quaternion.identity);
        Destroy(_bloodSpurtParticles, 1.5f);
        anim.SetTrigger("Death");
        audioSource.PlayOneShot(deathSound);

        yield return new WaitForSeconds(0.9f);
        StartCoroutine(UIManager.Instance.ActiveDeathScreen());

        yield return new WaitForSeconds(0.9f);
        Instantiate(GameManager.Instance.shade, transform.position, Quaternion.identity);
    }

    public IEnumerator DeathSpike()
    {
        deathCauseSpike = true;
        aState.alive = false;
        Time.timeScale = 1;
        GameObject _bloodSpurtParticles = Instantiate(bloodSpurt, transform.position, Quaternion.identity);
        Destroy(_bloodSpurtParticles, 1.5f);
        anim.SetTrigger("Death");

        yield return new WaitForSeconds(0.5f);
        StartCoroutine(UIManager.Instance.ActiveDeathScreenSpike());
    }

    public void Respawned()
    {
        if (!aState.alive)
        {
            aState.alive = true;
            halfMana = true;
            UIManager.Instance.SwitchMana(UIManager.ManaState.HalfMana);
            Mana = 0;
            Health = maxHealth;
            anim.Play("Alex_IDLE");
        }
    }

    public void RespawnedFromSpike()
    {
        if (!aState.alive)
        {
            aState.alive = true;
            Mana = 0;
            Health = maxHealth;
            anim.Play("Alex_IDLE");
        }
    }
    IEnumerator Flash()
    {
        sr.enabled = !sr.enabled;
        canFlash = false;
        yield return new WaitForSeconds(0.1f);
        canFlash = true;
    }

    void FlashWhileInvincible()
    {   
        if (aState.invincible)
        {
            if (Time.timeScale > 0.2 && canFlash)
            {
                StartCoroutine(Flash());
            }
        }
        else
        {
            sr.enabled = true;
        }
    }

    public void RestoredMana()
    {
        halfMana = false;
        UIManager.Instance.SwitchMana(UIManager.ManaState.FullMana);
    }

    public int Health
    {
        get { return health; }
        set
        {
            if (health != value)
            {
                health = Mathf.Clamp(value, 0, maxHealth);

                if (onHealthChangedCallback != null)
                {
                    onHealthChangedCallback.Invoke();
                    /*if (health == maxHealth)
                    {
                        StopHealAudio();
                    }*/
                }
            }
        }
    }

    void Heal()
    {
        if (Input.GetButton("Healing") && Health < maxHealth && !aState.jumping && !aState.dashing && rb.velocity.x == 0 && Mana != 0)
        {
            aState.healing = true;
            anim.SetBool("Healing", true);

            if (!audioSource.isPlaying)
            {
                PlayHealAudio();
            }

            //healing
            healTimer += Time.deltaTime;
            if (healTimer >= timeToHeal)
            {
                Health++;
                healTimer = 0;
            }
            Mana -= Time.deltaTime * manaDrainSpeed;
        }
        else
        {
            aState.healing = false;
            anim.SetBool("Healing", false);
            healTimer = 0;
            /*if (!Input.GetButton("Healing") || Health == maxHealth)
            {
                StopHealAudio();
            }*/
        }
    }

    void PlayHealAudio()
    {
        // Memulai pemutaran audio
        if (audioSource != null && healSound != null)
        {
            audioSource.clip = healSound;
            audioSource.loop = true;
            audioSource.PlayOneShot(healSound);
        }
    }


    public float Mana
    {
        get { return mana; }
        set
        {
            //if mana stats change
            if (mana != value)
            {
                if(!halfMana)
                {
                    mana = Mathf.Clamp(value, 0, 1);
                }
                else
                {
                    mana = Mathf.Clamp(value, 0, 0.5f);
                }
                
                manaStorage.fillAmount = Mana;

            }
        }
    }

    public bool Grounded()
    {
        if (Physics2D.Raycast(groundCheckPoint.position, Vector2.down, groundCheckY, whatIsGround)
            || Physics2D.Raycast(groundCheckPoint.position + new Vector3(groundCheckX, 0, 0), Vector2.down, groundCheckY, whatIsGround)
            || Physics2D.Raycast(groundCheckPoint.position + new Vector3(-groundCheckX, 0, 0), Vector2.down, groundCheckY, whatIsGround))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    /*public bool Grounded()
    {
        bool isGrounded = Physics2D.Raycast(groundCheckPoint.position, Vector2.down, groundCheckY, whatIsGround)
            || Physics2D.Raycast(groundCheckPoint.position + new Vector3(groundCheckX, 0, 0), Vector2.down, groundCheckY, whatIsGround)
            || Physics2D.Raycast(groundCheckPoint.position + new Vector3(-groundCheckX, 0, 0), Vector2.down, groundCheckY, whatIsGround);

        // Draw raycasts for visualization
        Debug.DrawRay(groundCheckPoint.position, Vector2.down * groundCheckY, isGrounded ? Color.green : Color.red);
        Debug.DrawRay(groundCheckPoint.position + new Vector3(groundCheckX, 0, 0), Vector2.down * groundCheckY, isGrounded ? Color.green : Color.red);
        Debug.DrawRay(groundCheckPoint.position + new Vector3(-groundCheckX, 0, 0), Vector2.down * groundCheckY, isGrounded ? Color.green : Color.red);

        return isGrounded;
    }*/

    void Jump()
    {

        
        if (jumpBufferCounter > 0 && coyoteTimeCounter > 0 && !aState.jumping)
        {
            audioSource.PlayOneShot(jumpSound);

            rb.velocity = new Vector3(rb.velocity.x, jumpForce);

            aState.jumping = true;
        }
        else if (!Grounded() && airJumpCounter < maxAirJumps && Input.GetButtonDown("Jump") && unlockedVarJump)
        {
            audioSource.PlayOneShot(jumpSound);

            aState.jumping = true;

            airJumpCounter++;

            rb.velocity = new Vector3(rb.velocity.x, jumpForce);
        }

        if (!Input.GetButton("Jump") && rb.velocity.y > 3)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0);

            aState.jumping = false;
        }

        anim.SetBool("Jumping", !Grounded());
        anim.SetFloat("yVelocity", rb.velocity.y);
    }

    void UpdateJumpVariables()
    {
        if (Grounded())
        {
            aState.jumping = false;
            coyoteTimeCounter = coyoteTime;
            airJumpCounter = 0;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

        if (Input.GetButtonDown("Jump"))
        {
            jumpBufferCounter = jumpBufferFrames;
        }
        else
        {
            jumpBufferCounter--;
        }
    }

    private bool Walled()
    {
        return Physics2D.OverlapCircle(wallCheck.position, 0.2f, wallLayer);
    }

    void WallSlide()
    {
        if(Walled() && !Grounded() && xAxis != 0 )
        {
            isWallSliding = true;
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed, float.MaxValue));
        }
        else
        {
            isWallSliding = false;
        }
    }

    /*void WallJump()
    {
        if(isWallSliding)
        {
            isWallJumping = false;
            wallJumpingDirection = !aState.lookingRight ? 1 : -1;

            CancelInvoke(nameof(StopWallJumping));
        }

        if(Input.GetButtonDown("Jump") && isWallSliding)
        {
            isWallJumping = true;
            rb.velocity = new Vector2(wallJumpingDirection * wallJumpingPower.x , wallJumpingPower.y);

            dashed = false;
            airJumpCounter = 0;

            if(aState.lookingRight && transform.eulerAngles.y == 0 || (!aState.lookingRight && transform.eulerAngles.y != 0))
            {
                aState.lookingRight = !aState.lookingRight;
                int _yRotation = aState.lookingRight ? 0 : 180;

                transform.eulerAngles = new Vector2(transform.eulerAngles.x, _yRotation);
            }

            Invoke(nameof(StopWallJumping), wallJumpingDuration);
        }
    }*/

    void WallJump()
    {
        if (isWallSliding)
        {
            isWallJumping = false;
            wallJumpingDirection = !aState.lookingRight ? 1 : -1;

            CancelInvoke(nameof(StopWallJumping));
        }

        if (Input.GetButtonDown("Jump") && isWallSliding)
        {
            isWallJumping = true;
            rb.velocity = new Vector2(wallJumpingDirection * wallJumpingPower.x, wallJumpingPower.y);

            dashed = false;
            airJumpCounter = 0;

            // Flip the character by changing its local scale
            Vector3 newScale = transform.localScale;
            newScale.x *= -1;
            transform.localScale = newScale;

            Invoke(nameof(StopWallJumping), wallJumpingDuration);
        }
    }

    void StopWallJumping()
    {
        isWallJumping = false;
    }

    public IEnumerator WalkIntoNewScene(Vector2 _exitDir, float _delay)
    {
        //If exit direction is upwards
        if (_exitDir.y != 0)
        {
            rb.velocity = jumpForce * _exitDir;
        }

        //If exit direction requires horizontal movement
        if (_exitDir.x != 0)
        {
            xAxis = _exitDir.x > 0 ? 1 : -1;

            Move();
        }

        Facing();
        yield return new WaitForSeconds(_delay);
        aState.cutscene = false;
    }
}


