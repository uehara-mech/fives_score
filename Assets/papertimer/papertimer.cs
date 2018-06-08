using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class papertimer : MonoBehaviour {
	public GameObject startbutton;
	public GameObject posebutton;
	public GameObject stopbutton;
	public GameObject enterbutton;
	public InputField minuteInput;
	public InputField secondInput;
	public GameObject minuteText1;
	public GameObject minuteText2;
	public GameObject secondText1;
	public GameObject secondText2;
	public int minute;
	public int second;
	public int InitialMinute;
	public int InitialSecond;
	public int InitialTime;
	public GameObject copiedSecond1;
	public float countTime = 0;
	public int timer_display;
	public int flagtimer = 0;
	public Material[] myMaterial = new Material[2];
	public GameObject[] timercube = new GameObject[24];
	public int timer_cubeidx;

	public void enterClick(){
		InitialMinute = int.Parse(minuteInput.text);
		InitialSecond = int.Parse (secondInput.text);
		InitialTime = InitialMinute * 60 + InitialSecond;
		minuteText1.GetComponent<TextMesh> ().text = (InitialMinute / 10).ToString ();
		minuteText2.GetComponent<TextMesh> ().text = (InitialMinute % 10).ToString ();
		secondText1.GetComponent<TextMesh> ().text = (InitialSecond / 10).ToString ();
		secondText2.GetComponent<TextMesh> ().text = (InitialSecond % 10).ToString ();
		minuteInput.text = "";
		secondInput.text = "";
	}

	public void startClick(){
		timerStart ();
	}

	public void timerStart(){
		flagtimer = 1;
	}

	public void stopClick(){
		init_timer ();

	}

	public void poseClick(){
		flagtimer = 0;
	}

	void init_timer(){
		countTime = 0;
		flagtimer = 0;
		minuteText1.GetComponent<TextMesh> ().text = 0.ToString ();
		minuteText2.GetComponent<TextMesh> ().text = 0.ToString ();
		secondText1.GetComponent<TextMesh> ().text = 0.ToString ();
		secondText2.GetComponent<TextMesh> ().text = 0.ToString ();
	}

	void display_timer(){
		if (flagtimer == 1) {
			countTime += Time.deltaTime;
			timer_display = InitialTime - Mathf.FloorToInt (countTime);
			if (timer_display >= 0) {
				second = timer_display % 60;
				minute = timer_display / 60;
				minuteText1.GetComponent<TextMesh> ().text = (minute / 10).ToString ();
				minuteText2.GetComponent<TextMesh> ().text = (minute % 10).ToString ();
				secondText1.GetComponent<TextMesh> ().text = (second / 10).ToString ();
				secondText2.GetComponent<TextMesh> ().text = (second % 10).ToString ();
				timer_cubeidx = Mathf.CeilToInt (((float)timer_display / 60f) * 2f);

				for (int j = 0; j < 24; j++) {
					if (j < timer_cubeidx) {
						timercube [j].GetComponent<MeshRenderer> ().material = myMaterial [1];
					} else {
						timercube [j].GetComponent<MeshRenderer> ().material = myMaterial [0];
					}
				}
			} else {
				flagtimer = 0;
			}
		}
	}

	//Escキーで終了
	void exitDisplay(){
		if (Input.GetKeyDown (KeyCode.Escape)) {
			Application.Quit();
		}
	}

	// Use this for initialization
	void Start () {
		startbutton = GameObject.Find ("startbutton");
		stopbutton = GameObject.Find ("stopbutton");
		posebutton = GameObject.Find ("posebutton");
		minuteText1 = GameObject.Find ("minute1");
		minuteText2 = GameObject.Find ("minute2");
		secondText1 = GameObject.Find ("second1");
		secondText2 = GameObject.Find ("second2");

		for (int i = 0; i < 24; i++) {
			string timercubeText = "timer" + (i + 1).ToString ();
			timercube [i] = GameObject.Find (timercubeText);
		}

		init_timer ();

		Display.displays [0].Activate ();
		Debug.Log (Display.displays.Length);
		Display.displays [1].Activate ();
	}
	
	// Update is called once per frame
	void Update () {
		
		exitDisplay ();
		display_timer ();

	}
}
