using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowObject : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform _playerTransform;
    public AlexStateList aState;
    [Header("Flip Rotation Stats")]
    [SerializeField] private float _flipYRotationTime = 0.5f;

    private Coroutine _turnCoroutine;
    private Player _player;
    private bool _isFacingRight;
    
    public static CameraFollowObject Instance;
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

        _player = _playerTransform.gameObject.GetComponent<Player>();
        _isFacingRight = aState.lookingRight;
    }

    private void Update()
    {
        transform.position = _playerTransform.position;
    }

    public void CallTurn()
    {
        _turnCoroutine = StartCoroutine(FlipYLerp());

        //LeanTween.rotateY(gameObject, DeterminationRotation(), _flipYRotationTime).setEaseInOutSine();
    }
    public IEnumerator FlipYLerp()
    {
        float startRotation = transform.localEulerAngles.y;
        float endRotationAmount = DeterminationRotation();
        float yRotation = 0f;

        float elapsedTime = 0f;
        while(elapsedTime < _flipYRotationTime)
        {
            elapsedTime += Time.deltaTime;
            //lerp the y rotation
            yRotation = Mathf.Lerp(startRotation, endRotationAmount, (elapsedTime / _flipYRotationTime));
            transform.rotation = Quaternion.Euler(0f, yRotation, 0f);

            yield return null;  
        }
    }

    private float DeterminationRotation()
    {
        _isFacingRight = !_isFacingRight;
        if(_isFacingRight )
        {
            return 180f;
        }
        else
        {
            return 0f;
        }
    }
    
}
