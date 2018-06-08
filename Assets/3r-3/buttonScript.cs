using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class buttonScript : MonoBehaviour {

	string str;
	public InputField inputField;
	public GameObject score33Obj;
	public score33 score33Script;

	public void savePoint () {
		str = inputField.text;
		score33Obj = GameObject.Find ("GameObject");
		score33Script = score33Obj.GetComponent<score33> ();
		score33Script.clplayer [score33Script.idx].savepoint = int.Parse (str);
		inputField.text = "";
	}
}