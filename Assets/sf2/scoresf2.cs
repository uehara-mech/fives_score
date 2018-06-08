using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;

public class scoresf2 : MonoBehaviour {

	public int playerNum = 2;

	//プレイヤーの各種属性を収めたクラスを12人分作る
	public playerclass[] clplayer = new playerclass[2];
	public Material[] myMaterial = new Material[2];

	public GameObject[] pointCube1 = new GameObject[15];
	public GameObject[] pointCube2 = new GameObject[15];

	public GameObject[] selectcube = new GameObject[2];
	public GameObject qNum;

	//idxは選択中のプレイヤーを表す変数
	int idx;

	//プレイヤークラスを初期化する
	//名前を入れる・スコアのリストの先頭に0を代入
	void init_player(){

		for (int i = 0; i < playerNum; i++) {
			clplayer[i] = gameObject.AddComponent<playerclass> ();
			//前面のシリンダー
		}

		for (int i = 0; i < 15; i++) {
			string pointCube1Name = "sfpoint3" + (i+1).ToString();
			pointCube1[i] = GameObject.Find (pointCube1Name);

			string pointCube2Name = "sfpoint3" + (i+1).ToString ()+"_2";
			pointCube2[i] = GameObject.Find (pointCube2Name);
		}
		//csvファイルの読み込み
		//csvload ();
		readfile();

		//プレイヤー名と学校名・ペーパー順位を表示
		for (int i = 0; i < playerNum; i++) {
			//strScore : オブジェクト名を入れる変数

			string selectcubeName = "Cube (" + (i + 1).ToString () + ")";
			selectcube [i] = GameObject.Find (selectcubeName);
			qNum = GameObject.Find ("qNum");

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
			schoolText.GetComponent<TextMesh> ().text = clplayer [i].school_class;
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

				} else if (i == idx) {
					//選択中のプレイヤー
					clplayer [i].lstClassO.Add (clplayer [i].lstClassO [countLast] + 1);
					clplayer [i].lstClassX.Add (clplayer [i].lstClassX [countLast]);
					clplayer [i].strListOX.Add ("o");
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
				} else if (i == idx) {
					//選択中のプレイヤー
					clplayer [i].lstClassO.Add (clplayer [i].lstClassO [countLast]);
					clplayer [i].lstClassX.Add (clplayer [i].lstClassX [countLast] + 1);
					clplayer [i].strListOX.Add ("x");
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
			clplayer [i].cl_o_num = clplayer [i].lstClassO [countLast]	+ clplayer [playerNum - 1 - i].lstClassX [countLast];
		}

	}

	//csvファイルのロード
	void csvload(){
		int i = 0;
		//ファイル名は2r-1.csv
		TextAsset csv = Resources.Load ("sf")as TextAsset;
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
		FileInfo fi = new FileInfo (Application.dataPath + "/" + "sf.csv");
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
		StreamWriter sw = new StreamWriter(Application.dataPath + "/" + "sfResult.csv",false);
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
		FileInfo fi = new FileInfo (Application.dataPath + "/" + "sfResult.csv");
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

	void display_point(){
		int countLast = clplayer [idx].lstClassO.Count - 1;

		for (int i = 0; i < 15; i++) {
			if (i < clplayer [0].cl_o_num) {
				pointCube1 [i].GetComponent<MeshRenderer> ().material = myMaterial [1];
			} else {
				pointCube1 [i].GetComponent<MeshRenderer> ().material = myMaterial [0];
			}
		}

		for (int i = 0; i < 15; i++) {
			if (i < clplayer [1].cl_o_num) {
				pointCube2 [i].GetComponent<MeshRenderer> ().material = myMaterial [1];
			} else {
				pointCube2 [i].GetComponent<MeshRenderer> ().material = myMaterial [0];
			}
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


	// Use this for initialization
	void Start () {
		idx = 0;
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
		calc_score ();

		display_idx (idx);
		display_qNum ();

		display_score ();
		display_point ();
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