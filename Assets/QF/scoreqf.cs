﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;

public class scoreqf : MonoBehaviour {

	public int playerNum = 13;
	public int switchSetNum;
	public int secondStartNum;

	//プレイヤーの各種属性を収めたクラスを12人分作る
	public playerclass[] clplayer = new playerclass[13];
	public Material[] myMaterial = new Material[16];


	//idxは選択中のプレイヤーを表す変数
	int idx;
	public GameObject[] backCircle = new GameObject[13];
	public GameObject[] backCube = new GameObject[13];
	public GameObject[] textScore = new GameObject[13];
	public GameObject[] breaktext = new GameObject[13];
	public GameObject[] frontbreakCube = new GameObject[13];

	public GameObject[] xNum = new GameObject[13];
	public GameObject[] xCube = new GameObject[13];

	public GameObject[] loseCube = new GameObject[13];

	public GameObject[] selectcube = new GameObject[13];
	public GameObject qNum;
	public GameObject roundName;

	public GameObject[] objscoreName = new GameObject[13];
	public GameObject[] killCube = new GameObject[13];

	//プレイヤークラスを初期化する
	//名前を入れる・スコアのリストの先頭に0を代入
	void init_player(){

		for (int i = 0; i < playerNum; i++) {
			clplayer[i] = gameObject.AddComponent<playerclass> ();
			//前面のシリンダー
			string backCircleName = "backcircle" + (i+1).ToString();
			backCircle[i] = GameObject.Find (backCircleName);
			string backCubeName = "breakcube (" + (i + 1).ToString () + ")2";
			backCube[i] = GameObject.Find (backCubeName);
			string loseCubeName = "loseCube (" + (i + 1).ToString () + ")";
			loseCube[i] = GameObject.Find (loseCubeName);
			string strScore = "scoreO" + (i + 1).ToString ();
			textScore[i] = GameObject.Find (strScore);
			string strBreak = "breaknum" + (i + 1).ToString ();
			breaktext[i] = GameObject.Find (strBreak);
			string strFrontbreak = "breakcube (" + (i + 1).ToString () + ")";
			frontbreakCube [i] = GameObject.Find (strFrontbreak);

			string strXback = "xcubeback" + (i + 1).ToString ();
			xCube [i] = GameObject.Find (strXback);
			string strXnum = "xnum" + (i + 1).ToString ();
			xNum [i] = GameObject.Find (strXnum);

			string selectcubeName = "Cube (" + (i + 1).ToString () + ")";
			selectcube [i] = GameObject.Find (selectcubeName);
			qNum = GameObject.Find ("qNum");
			roundName = GameObject.Find ("roundName");

			string playerText = "scoreName" + (i + 1).ToString ();
			objscoreName[i] = GameObject.Find (playerText);

			string killCubeText = "breakcube (" + (i + 1).ToString () + ")3";
			killCube [i] = GameObject.Find (killCubeText);
			killCube [i].SetActive (false);

		}
		//csvファイルの読み込み
		//csvload ();
		readfile();

		//プレイヤー名と学校名・ペーパー順位を表示
		for (int i = 0; i < playerNum; i++) {
			//strScore : オブジェクト名を入れる変数
			string strScore = "scoreName" + (i + 1).ToString ();
			string strSchool = "school" + (i + 1).ToString ();
			string strPaper = "paper" + (i + 1).ToString ();
			//strname : プレイヤー名
			string strName = "";
			//scoretext : オブジェクト(3Dtext)をいれる変数
			GameObject scoretext = GameObject.Find (strScore);
			GameObject schoolText = GameObject.Find (strSchool);
			GameObject paperText = GameObject.Find (strPaper);
			char[] chName = clplayer[i].p_nameClass.ToCharArray ();
			for (int j = 0; j < chName.Length - 1; j++) {
				strName = strName + chName [j] + "\n";
			}
			strName = strName + chName [chName.Length - 1];
			scoretext.GetComponent<TextMesh> ().text = strName;
			paperText.GetComponent<TextMesh> ().text = AddOrdinal(int.Parse(clplayer [i].paper_class));
			schoolText.GetComponentInChildren<Text> ().text = clplayer [i].school_class;
		}
	}

	//score_payer：プレイヤーのスコアを操作
	//lstClassO, lstClassXは「正解数、誤答数」を表すリスト (! ポイント数ではないので注意 !)
	void score_player(int idx/*idx : 選択中のプレイヤーを表す変数*/){
		//countLast : lstClassの末尾のインデックス
		int countLast = clplayer [idx].lstClassO.Count - 1;
		//正解時はoキーを押す

		if (Input.GetKeyDown (KeyCode.O)) {
			for (int i = 0; i < playerNum; i++) {
				if (i != idx) {
					//idxで指定されている以外のプレイヤー
					clplayer [i].lstClassO.Add (clplayer [i].lstClassO [countLast]);
					clplayer [i].lstClassX.Add (clplayer [i].lstClassX [countLast]);
					clplayer [i].strListOX.Add ("");
					if (clplayer [i].combo_break [countLast] > 0) {
						clplayer [i].combo_break.Add (clplayer [i].combo_break [countLast] - 1);
					} else {
						clplayer [i].combo_break.Add (0);
					}
				} else if (i == idx) {
					//選択中のプレイヤー
					clplayer [i].lstClassO.Add (clplayer [i].lstClassO [countLast] + 1);
					clplayer [i].lstClassX.Add (clplayer [i].lstClassX [countLast]);
					clplayer [i].strListOX.Add ("o");
					clplayer [i].combo_break.Add (0);
				}
			}
			savefile ();
		}
		
		//誤答時はxキーを押す
		if (Input.GetKeyDown (KeyCode.X)) {
			for (int i = 0; i < playerNum; i++) {
				if (i != idx) {
					//idxで指定されている以外のプレイヤー
					clplayer [i].lstClassO.Add (clplayer [i].lstClassO [countLast]);
					clplayer [i].lstClassX.Add (clplayer [i].lstClassX [countLast]);
					clplayer [i].strListOX.Add ("");
					if (clplayer [i].combo_break [countLast] > 0) {
						clplayer [i].combo_break.Add (clplayer [i].combo_break [countLast] - 1);
					} else {
						clplayer [i].combo_break.Add (0);
					}
				} else if (i == idx) {
					//選択中のプレイヤー
					clplayer [i].lstClassO.Add (clplayer [i].lstClassO [countLast]);
					clplayer [i].lstClassX.Add (clplayer [i].lstClassX [countLast] + 1);
					clplayer [i].strListOX.Add ("x");
					clplayer [i].combo_break.Add (clplayer [i].lstClassX [countLast]+1);
				}
			}
			savefile ();
		}

		//スルー時はtキーを押す
		if (Input.GetKeyDown (KeyCode.T)) {
			for (int i = 0; i < playerNum; i++) {
				clplayer [i].lstClassO.Add (clplayer [i].lstClassO [countLast]);
				clplayer [i].lstClassX.Add (clplayer [i].lstClassX [countLast]);
				clplayer [i].strListOX.Add ("t");
				if (clplayer [i].combo_break[countLast] > 0) {
					clplayer [i].combo_break.Add (clplayer [i].combo_break [countLast] - 1);
				} else {
					clplayer [i].combo_break.Add (0);
				}
			}
			savefile ();
		}
	}

	//undo() : Zキーでアンドゥ
	void undo(){
		//countLast : lstClassの末尾のインデックス
		int countLast = clplayer [idx].lstClassO.Count - 1;
		int countLastStr = clplayer [idx].strListOX.Count - 1;
		if (countLast > 0) {
			if (Input.GetKeyDown (KeyCode.Z)) {
				for (int j = 0; j < playerNum; j++) {
					clplayer [j].lstClassO.RemoveAt (countLast);
					clplayer [j].lstClassX.RemoveAt (countLast);
					clplayer [j].strListOX.RemoveAt (countLastStr);
					clplayer [j].combo_break.RemoveAt (countLast);
				}
				savefile ();
			}
		}
	}

	//スコア計算
	void calc_score(){
		//countLast : lstClassの末尾のインデックス
		int countLast = clplayer [idx].lstClassO.Count - 1;

		for (int i = 0; i < playerNum; i++) {
			clplayer [i].cl_o_num = clplayer [i].lstClassO [countLast]-clplayer[i].lstClassX[countLast];
		}

	}

	//csvファイルのロード
	void csvload(){
		int i = 0;
		//ファイル名は2r-1.csv
		TextAsset csv = Resources.Load ("qf")as TextAsset;
		StringReader reader = new StringReader (csv.text);
		while (reader.Peek () > -1) {
			string line = reader.ReadLine ();
			string[] values = line.Split (',');
			clplayer [i].paper_class = values [0];
			clplayer [i].p_nameClass = values [1];
			clplayer [i].school_class = values [2];
			i++;
		}
	}

	//csvファイルのロード・外部ファイル使用バージョン
	void readfile(){
		int i = 0;
		FileInfo fi = new FileInfo (Application.dataPath + "/" + "qf.csv");
		StreamReader sr = new StreamReader (fi.OpenRead (), Encoding.UTF8);
		while (sr.Peek () > -1) {
			string line = sr.ReadLine ();
			string[] values = line.Split (',');
			clplayer [i].paper_class = values [0];
			clplayer[i].p_nameClass = values [1];
			clplayer [i].school_class = values [2];
			i++;
		}
		sr.Close ();
	}

	//csvファイルの初期化
	//Start時にのみ実行
	void init_file(){
		StreamWriter sw = new StreamWriter(Application.dataPath + "/" + "qfResult.csv",false);
		sw.WriteLine ();
		sw.Flush ();
		sw.Close ();
	}

	//csvファイルへの書き出し
	void savefile(){
		init_file ();
		int i = 0;
		//o_textは"o","x"を一行ずつ書き込む
		string o_text = "";
		int countLast = clplayer [0].strListOX.Count - 1;
		FileInfo fi = new FileInfo (Application.dataPath + "/" + "qfResult.csv");
		StreamWriter sw = fi.AppendText ();
		//player_text は　プレイヤー名を書き込む
		string player_text = "";
		for (i = 0; i < playerNum; i++) {
			player_text = player_text + clplayer [i].p_nameClass + ",";
		}
		sw.WriteLine (player_text);

		for (i = 0; i <= countLast; i++) {
			o_text = "";
			for (int j = 0; j < playerNum; j++) {
				o_text = o_text + clplayer [j].strListOX [i].ToString () + ",";
			}
			sw.WriteLine (o_text);
		}
		sw.Flush ();
		sw.Close ();
	}

	//ポイントを表示
	void display_score(){

		for (int i = 0; i < playerNum; i++) {
			textScore[i].GetComponent<TextMesh> ().text = clplayer [i].cl_o_num.ToString();
		}
	}

	//休み数表示
	void display_break(){
		//countLast : lstClassの末尾のインデックス
		int countLast = clplayer [idx].lstClassO.Count - 1;
		for (int i = 0; i < playerNum; i++) {
			breaktext[i].GetComponent<TextMesh> ().text = clplayer [i].combo_break [countLast].ToString();
		}

	}

	//x数表示
	void display_x(){
		//countLast : lstClassの末尾のインデックス
		int countLast = clplayer [idx].lstClassO.Count - 1;
		for (int i = 0; i < playerNum; i++) {
			xNum[i].GetComponent<TextMesh> ().text = clplayer [i].lstClassX [countLast].ToString();
		}
	}

	//選択中のプレイヤーを示す
	void display_idx(int idx){
		for (int i = 0; i < playerNum; i++) {
			if (i == idx) {
				selectcube [i].GetComponent<MeshRenderer> ().material = myMaterial [1];
			} else {
				selectcube [i].GetComponent<MeshRenderer> ().material = myMaterial [0];
			}

		}
	}

	//問題数表示
	void display_qNum(){
		int countLast = clplayer [0].lstClassO.Count - 1;
		if (switchSetNum == 1) {
			qNum.GetComponent<TextMesh> ().text = "Q. " + (countLast + 1).ToString ();
			roundName.GetComponent<TextMesh> ().text = "QF 1st Set";
		} else if (switchSetNum == 2) {
			qNum.GetComponent<TextMesh> ().text = "Q. " + (countLast - secondStartNum).ToString ();
			roundName.GetComponent<TextMesh> ().text = "QF 2nd Set";
		}
	}

	//休み中の処理
	void breakPlayer(){
		int countLast = clplayer [idx].lstClassO.Count - 1;
		for (int i = 0; i < playerNum; i++) {
			if (clplayer [i].kill_idx == 0) {
				if (clplayer [i].combo_break [countLast] > 0) {
					backCircle [i].GetComponent<MeshRenderer> ().material = myMaterial [8];
					objscoreName [i].GetComponent<MeshRenderer> ().material = myMaterial [11];
					if (clplayer [i].kill_idx == 0) {
						backCube [i].SetActive (true);
						breaktext [i].SetActive (true);
						frontbreakCube [i].SetActive (true);
					}
				} else {
					objscoreName [i].GetComponent<MeshRenderer> ().material = myMaterial [10];
					breaktext [i].SetActive (false);
					backCube [i].SetActive (false);
					frontbreakCube [i].SetActive (false);
					backCircle [i].GetComponent<MeshRenderer> ().material = myMaterial [9];
				}
			}
		}
	}

	//Lキーで敗退、Wキーで勝ち抜け、Nキーで復帰
	void winlosePlayer(int idx){
		if (Input.GetKeyDown (KeyCode.L)) {
			clplayer [idx].player_win = 0;
			clplayer [idx].player_lose = 1;
		}
		if (Input.GetKeyDown (KeyCode.W)) {
			clplayer [idx].player_win = 1;
			clplayer [idx].player_lose = 0;
		}
		if (Input.GetKeyDown (KeyCode.N)) {
			clplayer [idx].player_win = 0;
			clplayer [idx].player_lose = 0;
		}
		for (int i = 0; i < playerNum; i++) {
			if (clplayer [i].player_win == 1) {
				loseCube [i].SetActive (true);
				loseCube [i].GetComponent<MeshRenderer> ().material = myMaterial [13];
			} else if (clplayer [i].player_lose == 1) {
				loseCube [i].SetActive (true);
				loseCube [i].GetComponent<MeshRenderer> ().material = myMaterial [13];
			} else {
				loseCube [i].SetActive (false);
			}
		}
	}

	//セット切り替え
	void switch_set(){
		int countLast = clplayer [0].lstClassO.Count - 1;
		if (Input.GetKeyDown (KeyCode.Alpha2)) {
			switchSetNum = 2;
			secondStartNum = clplayer [0].lstClassO.Count-2;
			for (int i = 0; i < playerNum; i++) {
				clplayer [i].combo_break [countLast] = 0;
			}
		} else if (Input.GetKeyDown (KeyCode.Alpha1)) {
			switchSetNum = 1;
		}
	}

	//予備問使用中の解答権剥奪(Rキー）　元に戻す時はAキー
	void kill_player(){
		if (Input.GetKeyDown (KeyCode.R)) {
			objscoreName [idx].GetComponent<MeshRenderer> ().material = myMaterial [11];
			killCube [idx].SetActive (true);
			breaktext [idx].SetActive (false);
			backCube [idx].SetActive (false);
			frontbreakCube [idx].SetActive (false);
			clplayer [idx].kill_idx = 1;
		} else if (Input.GetKeyDown (KeyCode.A)) {
			objscoreName [idx].GetComponent<MeshRenderer> ().material = myMaterial [10];
			killCube [idx].SetActive (false);
			clplayer [idx].kill_idx = 0;
		}
	}

	//Escキーで終了
	void exitDisplay(){
		if (Input.GetKeyDown (KeyCode.Escape)) {
			Application.Quit();
		}
	}

	string AddOrdinal(int num){
		if( num <= 0 ) return num.ToString();

		switch(num % 100)
		{
		case 11:
		case 12:
		case 13:
			return num + "th";
		}

		switch(num % 10)
		{
		case 1:
			return num + "st";
		case 2:
			return num + "nd";
		case 3:
			return num + "rd";
		default:
			return num + "th";
		}
	}

	// Use this for initialization
	void Start () {
		idx = 0;
		switchSetNum = 1;
		init_player ();
		init_file ();
		Display.displays [0].Activate ();
		Debug.Log (Display.displays.Length);
		Display.displays [1].Activate ();
	}

	// Update is called once per frame
	void Update () {
		//右矢印・左矢印で移動
		if (Input.GetKeyDown (KeyCode.RightArrow)) {
			if (idx >= 0 && idx < playerNum-1) {
				idx++;
			}
			Debug.Log (idx);
		}
		if (Input.GetKeyDown (KeyCode.LeftArrow)) {
			if (idx > 0 && idx <= playerNum-1) {
				idx--;
			}
			Debug.Log (idx);
		}
		//o・xキーでスコア処理
		score_player (idx);
		winlosePlayer (idx);
		calc_score ();
		display_score ();
		display_x ();

		display_idx (idx);
		display_qNum ();

		switch_set ();
		kill_player ();

		display_break ();
		breakPlayer ();
		exitDisplay ();
		undo ();
		if (Input.GetKeyDown (KeyCode.Space)) {
			savefile ();
		}
	}

	void OnApplicationQuit(){
		savefile ();
	}



}