using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
	public string sceneName;

	void Start()
	{
		GameState.Instance.IsLoading = true;
		SceneManager.LoadScene(sceneName);
	}
}
