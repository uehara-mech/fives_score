﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;

public class score21 : MonoBehaviour {

	public int playerNum = 9;
	public int switch_set_idx;

	//プレイヤーの各種属性を収めたクラスを9人分作る
	public playerclass[] clplayer = new playerclass[9];
	public GameObject[,] x_display = new GameObject[9, 2];
	public Material[] myMaterial = new Material[5];
	//idxは選択中のプレイヤーを表す変数
	int idx;
	public GameObject[] backCircle = new GameObject[9];

	public GameObject[] selectcube = new GameObject[9];
	public GameObject qNum;
	public GameObject roundName;

	//(o_max) O (x_max) X
	public int o_max;
	public int x_max;

	//プレイヤークラスを初期化する
	//名前を入れる・スコアのリストの先頭に0を代入
	void init_player(int round_idx){
		
		if (switch_set_idx == 1) {
			for (int i = 0; i < playerNum; i++) {
				clplayer [i] = gameObject.AddComponent<playerclass> ();
				//前面のシリンダーを消す
				string backCircleName = "backcircle" + (i + 1).ToString ();
				backCircle [i] = GameObject.Find (backCircleName);

				string selectcubeName = "Cube (" + (i + 1).ToString () + ")";
				selectcube [i] = GameObject.Find (selectcubeName);
				qNum = GameObject.Find ("qNum");

				roundName=GameObject.Find("RoundName");
			}
		}
		//csvファイルの読み込み
		//csvload ();
		readfile(round_idx);

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

		for (int i = 0; i < playerNum; i++) {
			for (int j = 0; j < 2; j++) {
				string str_x = "x" + (i + 1).ToString () + (j + 1).ToString ();
				x_display [i, j] = GameObject.Find (str_x);
			}
		}
	}

	//score_payer：プレイヤーのスコアを操作
	//lstClassO, lstClassXは「正解数、誤答数」を表すリスト (! ポイント数ではないので注意 !)
	void score_player(int idx/*idx : 選択中のプレイヤーを表す変数*/){
		//countLast : lstClassの末尾のインデックス
		int countLast = clplayer [idx].lstClassO.Count - 1;
		//正解時はoキーを押す
		if (clplayer [idx].lstClassO [countLast] < o_max) {
			if (Input.GetKeyDown (KeyCode.O)) {
				for (int i = 0; i < playerNum; i++) {
					if (i != idx) {
						//idxで指定されている以外のプレイヤー
						clplayer [i].lstClassO.Add (clplayer [i].lstClassO [countLast]);
						clplayer [i].lstClassX.Add (clplayer [i].lstClassX [countLast]);
						clplayer [i].strListOX.Add ("");
					} else if (i == idx) {
						//選択中のプレイヤー
						clplayer [i].lstClassO.Add (clplayer [i].lstClassO [countLast] + 1);
						clplayer [i].lstClassX.Add (clplayer [i].lstClassX [countLast]);
						clplayer [i].strListOX.Add ("o");
					}
				}
				savefile ();
			}
		}
		//誤答時はxキーを押す
		if (clplayer [idx].lstClassX [countLast] < x_max) {
			if (Input.GetKeyDown (KeyCode.X)) {
				for (int i = 0; i < playerNum; i++) {
					if (i != idx) {
						//idxで指定されている以外のプレイヤー
						clplayer [i].lstClassO.Add (clplayer [i].lstClassO [countLast]);
						clplayer [i].lstClassX.Add (clplayer [i].lstClassX [countLast]);
						clplayer [i].strListOX.Add ("");
					} else if (i == idx) {
						//選択中のプレイヤー
						clplayer [i].lstClassO.Add (clplayer [i].lstClassO [countLast]);
						clplayer [i].lstClassX.Add (clplayer [i].lstClassX [countLast] + 1);
						clplayer [i].strListOX.Add ("x");
					}
				}
				savefile ();
			}
		}
		//スルー時はtキーを押す
		if (Input.GetKeyDown (KeyCode.T)) {
			for (int i = 0; i < playerNum; i++) {
				clplayer [i].lstClassO.Add (clplayer [i].lstClassO [countLast]);
				clplayer [i].lstClassX.Add (clplayer [i].lstClassX [countLast]);
				clplayer [i].strListOX.Add ("t");
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
			clplayer [i].cl_o_num = clplayer [i].lstClassO [countLast];
			clplayer [i].cl_x_num = clplayer [i].lstClassX [countLast];
		}
		
	}

	//csvファイルのロード
	void csvload(){
		int i = 0;
		//ファイル名は2r-1.csv
		TextAsset csv = Resources.Load ("2r-1")as TextAsset;
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
	void readfile(int round_idx){
		int i = 0;
		FileInfo fi = new FileInfo (Application.dataPath + "/" + "2r-1_" + round_idx + ".csv");
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
		StreamWriter sw = new StreamWriter (Application.dataPath + "/" + "2r-1Result.csv", false);
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
		FileInfo fi = new FileInfo (Application.dataPath + "/" + "2r-1Result.csv");
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
			string strScore = "";
			strScore = "scoreO" + (i + 1).ToString ();
			GameObject textScore = GameObject.Find (strScore);
			textScore.GetComponent<TextMesh> ().text = clplayer [i].cl_o_num.ToString();
		}
	}

	//×の表示
	void display_X(){
		for (int i = 0; i < playerNum; i++) {
			if (clplayer [i].cl_x_num == 0) {
				for (int j = 0; j < 2; j++) {
					x_display [i, j].GetComponent<Renderer> ().material = myMaterial [0];
				}
			} else if (clplayer [i].cl_x_num == 1) {
				x_display [i, 0].GetComponent<Renderer> ().material = myMaterial [1];
				x_display [i, 1].GetComponent<Renderer> ().material = myMaterial [0];
			} else if (clplayer [i].cl_x_num == 2) {
				x_display [i, 0].GetComponent<Renderer> ().material = myMaterial [1];
				x_display [i, 1].GetComponent<Renderer> ().material = myMaterial [1];
			}
		}
	}

	//負けたときの処理
	void losePlayer(){
		int countLast = clplayer [idx].lstClassO.Count - 1;
		for (int i = 0; i < playerNum; i++) {
			string playerText = "scoreName" + (i + 1).ToString ();
			GameObject objscoreName = GameObject.Find (playerText);
			if (clplayer [i].lstClassX [countLast] >= x_max) {
				clplayer [i].player_lose = 1;
				backCircle [i].GetComponent<MeshRenderer> ().material = myMaterial [4];
				objscoreName.GetComponent<TextMesh> ().color = new Color (63f / 255f, 63f / 255f, 63f / 255f);
			} else {
				if (clplayer [i].player_win == 0) {
					objscoreName.GetComponent<TextMesh> ().color = Color.white;
					backCircle [i].GetComponent<MeshRenderer> ().material = myMaterial [3];
				}
				clplayer [i].player_lose = 0;
			}
		}
	}

	//勝ったときの処理
	void winPlayer(){
		int countLast = clplayer [idx].lstClassO.Count - 1;
		for (int i = 0; i < playerNum; i++) {
			string playerText = "scoreName" + (i + 1).ToString ();
			GameObject objscoreName = GameObject.Find (playerText);
			if (clplayer [i].lstClassO [countLast] >= o_max) {
				clplayer [i].player_win = 1;
				objscoreName.GetComponent<TextMesh> ().color = new Color (255f / 255f, 119f / 255f, 119f / 255f);
				backCircle [i].GetComponent<MeshRenderer> ().material = myMaterial [2];
			} else {
				objscoreName.GetComponent<TextMesh> ().color = Color.white;
				clplayer [i].player_win = 0;
				backCircle [i].GetComponent<MeshRenderer> ().material = myMaterial [3];
			}
		}
	}

	//Escキーで終了
	void exitDisplay(){
		if (Input.GetKeyDown (KeyCode.Escape)) {
			Application.Quit();
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
		qNum.GetComponent<TextMesh> ().text = "Q. " + (countLast + 1).ToString ();
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

	//表示セット切り替え
	void switch_set(){
		int countLast = clplayer [0].lstClassO.Count - 1;
		if (countLast == 0) {
			if (switch_set_idx == 0) {
				if (Input.GetKeyDown (KeyCode.Alpha1)) {
					init_player (1);
					roundName.GetComponent<TextMesh> ().text = "2R-1 1st Set";
				} else if (Input.GetKeyDown (KeyCode.Alpha2)) {
					init_player (2);
					roundName.GetComponent<TextMesh> ().text = "2R-1 2nd Set";
				} else if (Input.GetKeyDown (KeyCode.Alpha3)) {
					init_player (3);
					roundName.GetComponent<TextMesh> ().text = "2R-1 3rd Set";
				} else if (Input.GetKeyDown (KeyCode.Alpha4)) {
					init_player (4);
					roundName.GetComponent<TextMesh> ().text = "2R-1 4th Set";
				} 
			}
			if (Input.GetKeyDown (KeyCode.S)) {
				switch_set_idx = 1;
			} else if (Input.GetKeyDown (KeyCode.R)) {
				switch_set_idx = 0;
			}
		}
	}

	//advantageの設定
	void advantage(){
		int countLast = clplayer [0].lstClassO.Count - 1;
		if (countLast == 0) {
			if (Input.GetKeyDown (KeyCode.UpArrow)) {
				clplayer [idx].lstClassO [countLast] += 1;
			} else if (Input.GetKeyDown (KeyCode.DownArrow)) {
				clplayer [idx].lstClassO [countLast] -= 1;
			}
		}
	}

	// Use this for initialization
	void Start () {
		idx = 0;
		switch_set_idx = 1;
		o_max = 5;
		x_max = 2;
		init_player (1);
		init_file ();
		Display.displays [0].Activate ();
		Debug.Log (Display.displays.Length);
		Display.displays [1].Activate ();

	}
	
	// Update is called once per frame
	void Update () {

		switch_set ();
		//右矢印・左矢印で移動
		if (Input.GetKeyDown (KeyCode.RightArrow)) {
			if (idx >= 0 && idx < 8) {
				idx++;
			}
			Debug.Log (idx);
		}
		if (Input.GetKeyDown (KeyCode.LeftArrow)) {
			if (idx > 0 && idx <= 8) {
				idx--;
			}
			Debug.Log (idx);
		}
		//o・xキーでスコア処理
		advantage ();

		score_player (idx);
		calc_score ();
		winPlayer ();
		losePlayer ();
		
		display_score ();

		display_idx (idx);
		display_qNum ();

		display_X ();
			
			
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
