using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    
    public SceneFader sceneFader;
    [SerializeField] GameObject deathScreen;
    [SerializeField] GameObject halfMana, fullMana;

    public enum ManaState
    {
        FullMana,
        HalfMana
    }
    public ManaState manaState;

    public static UIManager Instance;
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

        if (Application.isPlaying)
            UnityEditor.SceneVisibilityManager.instance.Show(gameObject, false);

        DontDestroyOnLoad(gameObject);

        sceneFader = GetComponentInChildren<SceneFader>();
    }

    public void SwitchMana(ManaState _manaState)
    {
        switch(_manaState)
        {
            case ManaState.FullMana:
                halfMana.SetActive(false);
                fullMana.SetActive(true);
                break;


            case ManaState.HalfMana:
                fullMana.SetActive(false);
                halfMana.SetActive(true) ;
                break;
        }
        manaState = _manaState;
    }

    public IEnumerator ActiveDeathScreen()
    {
        yield return new WaitForSeconds(0.8f);
        StartCoroutine(sceneFader.Fade(SceneFader.FadeDirection.In));

        yield return new WaitForSeconds(0.8f);
        deathScreen.SetActive(true);

    }

    public IEnumerator DeactiveDeathScene()
    {
        yield return new WaitForSeconds(0.5f);
        deathScreen.SetActive(false);
        StartCoroutine(sceneFader.Fade(SceneFader.FadeDirection.Out));
    }
}
