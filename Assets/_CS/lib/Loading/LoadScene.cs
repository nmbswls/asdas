using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Load 
{ 
	public static string SceneName; 
} 

public class LoadScene : MonoBehaviour { 
	public Text loadingText;//加载进度 

	float changeTime = 0;
	int numDot = 3;

	float loadingTime = 0f;

	private AsyncOperation asyncOperation ;//异步对象 
	public float nowProcess; 
	void Start () 
	{ 
		Time.timeScale = 1;
		asyncOperation = null;
		StartCoroutine(loadAsync()); 
		loadingTime = 0f;
		changeTime = 0f;
		numDot = 3;
	} 


	void Update() 
	{ 
		if (asyncOperation == null) 
		{ 
			return; 
		} 
		changeTime += Time.deltaTime;
		loadingTime += Time.deltaTime;
		if (changeTime > 0.3f) {
			changeTime = 0f;
			numDot = ((numDot - 3) + 1) % 3 + 3;
			string subfix = "";
			for (int i = 0; i < numDot; i++) {
				subfix += ".";
			}
			loadingText.text = "Loading" + subfix;
		}
		int toProcess; 
		if (asyncOperation .progress < 0.9f) 
		{ 
			toProcess = (int)asyncOperation .progress * 100; 
		} 
		else 
		{ 
			toProcess = 100; 
		} 
		if (nowProcess < toProcess) 
		{ 
			nowProcess++; 
		} 

		// 设置为true的时候，如果场景数据加载完毕，就可以自动跳转场景 
		if (loadingTime>0.5f&&nowProcess == 100) 
		{ 
			asyncOperation .allowSceneActivation = true; 
		} 
	} 
	IEnumerator loadAsync() 
	{ 
		asyncOperation = SceneManager.LoadSceneAsync(Load.SceneName); 
		asyncOperation.allowSceneActivation = false; 
		yield return asyncOperation ; 
	} 
}
