using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class select3r : MonoBehaviour {
	public static int count = 0;

	//Escキーで終了
	void exitDisplay(){
		if (Input.GetKeyDown (KeyCode.Escape)) {
			Application.Quit();
		}
	}

//	void OnSceneLoaded(Scene scene, LoadSceneMode sceneMode){
//		Display.displays [0].Activate ();
//		Display.displays [1].Activate ();
//	}

	// Use this for initialization
	void Start () {
		Debug.Log ("bcount = "+count);
		//SceneManager.sceneLoaded += OnSceneLoaded;
		if (count == 0) {
			Display.displays [0].Activate ();
			Debug.Log (Display.displays.Length);
			Display.displays [1].Activate ();
			count += 1;
			//Debug.Log (count);
		}
		Debug.Log ("acount = "+count);
	}
	
	// Update is called once per frame
	void Update () {
		exitDisplay ();

		if (Input.GetKeyDown (KeyCode.Alpha1)) {
			SceneManager.LoadScene("3r-1");
		}else if (Input.GetKeyDown (KeyCode.Alpha2)) {
			SceneManager.LoadScene("3r-2");
		}else if (Input.GetKeyDown (KeyCode.Alpha3)) {
			SceneManager.LoadScene("3r-3");
		}else if (Input.GetKeyDown (KeyCode.Alpha4)) {
			SceneManager.LoadScene("3r-4");
		}
	}
}
