using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class NowLoading : MonoBehaviour
{
    public Slider progressbar;
    public Text loadtext;

    public void Start()
    {
        StartCoroutine(LoadScene());
    }

    IEnumerator LoadScene()
    {

        yield return null;
        AsyncOperation operation = SceneManager.LoadSceneAsync("불러올 씬이름");
        operation.allowSceneActivation = false;

        //반목문 생성 후 Slider의 value를 매 프레임 증가
        while (!operation.isDone) //로딩이 끝나서 isDone 이 true가 되기 전까지 계속 반복
        {
            yield return null;

            if (progressbar.value < 1f)
            {
                progressbar.value = Mathf.MoveTowards(progressbar.value, 1f, Time.deltaTime);
            }

            else
            {
                loadtext.text = "Press SpaceBar";
            }

            if (Input.GetKeyDown(KeyCode.Space) && progressbar.value >= 1f && operation.progress >= 0.9f)
            {
                operation.allowSceneActivation = true;
            }

        }

    }

}
