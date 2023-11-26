using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockDash : MonoBehaviour
{
    [SerializeField] GameObject particle;
    [SerializeField] GameObject canvasUI;
    bool used;

    // Start is called before the first frame update
    void Start()
    {
        if (Player.Instance.unlockedDash)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D _collision)
    {
        if (_collision.CompareTag("Player") && !used)
        {
            used = true;
            StartCoroutine(ShowUI());

        }
    }

    IEnumerator ShowUI()
    {
        GameObject _particles = Instantiate(particle, transform.position, Quaternion.identity);
        Destroy(_particles, 0.5f);
        yield return new WaitForSeconds(0.5f);

        canvasUI.SetActive(true);

        yield return new WaitForSeconds(4f);
        Player.Instance.unlockedDash = true;
        SaveData.Instance.SavePlayerData();
        canvasUI.SetActive(false);
        Destroy(gameObject);
    }
}
