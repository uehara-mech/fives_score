using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerclass : MonoBehaviour {
	public string p_nameClass;
	public string paper_class;
	public string school_class;
	public int cl_o_num = 0;
	public int cl_x_num = 0;
	public List<int> lstClassO = new List<int>();
	public List<int> lstClassX = new List<int>();
	public List<string> strListOX = new List<string> ();

	//player_win, player_lose は勝ち負けで１になる
	public int player_win = 0;
	public int player_lose = 0;

	//byのスコア
	public int byScore = 0;

	//updownのsavepoint
	public int savepoint = 0;

	//updownのスコア(list)
	public List<int> updown_score = new List<int>();

	//連答数
	public List<int> succession = new List<int>();

	//combo shotのスコア
	public List<int> combo_score = new List<int>();
	//combo shot の休み数
	public List<int> combo_break = new List<int>();
	//combo_shot の休み状態に入ったかどうか
	public int break_idx = 0;

	//QFで解答権剥奪状態

	public int kill_idx=0;


	// Use this for initialization
	void Start () {
		lstClassO.Add (0);
		lstClassX.Add (0);
		updown_score.Add (0);
		succession.Add (1);
		combo_break.Add (0);
		combo_score.Add (0);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
