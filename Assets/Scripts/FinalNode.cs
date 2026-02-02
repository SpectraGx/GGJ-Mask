using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalNode : MonoBehaviour
{
    [SerializeField] private float uploadTime = 3f;

    private bool isUploading = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isUploading)
        {
            StartCoroutine(UploadProcess(other.gameObject));
        }
    }

    IEnumerator UploadProcess(GameObject player)
    {
        isUploading = true;
        Debug.Log("Uploading...");

        GameManager.instance.SetGameState(GameState.Win);

        if (player.GetComponent<RollingMovement>()) player.GetComponent<RollingMovement>().enabled = false;

        float timer = 0f;
        Vector3 startPos = player.transform.position;

        while (timer < uploadTime)
        {
            timer += Time.deltaTime;
            float progress = timer / uploadTime;
            player.transform.position = Vector3.Lerp(startPos, startPos + Vector3.up * 5f, progress);
            player.transform.Rotate(0, 720f * Time.deltaTime, 0);
            player.transform.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, progress);

            yield return null;
        }

        Debug.Log("NIVEL COMPLETADO");
        //SceneLoader.Instance.LoadLevel(sceneToLoad);
        LevelUIManager.Instance.ShowWinScreen();
        //SceneManager.LoadScene(sceneToLoad);
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
