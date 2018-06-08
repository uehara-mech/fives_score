using UnityEngine;
using UnityEditor;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class AlignEditor : Editor {

	[MenuItem("Editor/均等割り付け(X軸)")]
	static void DistVert()
	{
		int count = Selection.transforms.Length;

		if (count < 3)
		{
			return;
		}

		Transform[] transforms = Selection.transforms.OrderBy(a => a.position.x).ToArray();

		Undo.RecordObjects(transforms, "modify position");

		float min = transforms[0].position.x;
		float max = transforms[count - 1].position.x;
		float d = (max - min) / (count - 1);

		for (int i = 1; i < count - 1; i++)
		{
			var t = transforms[i];
			Vector3 p = t.position;
			p.x = d * i + min;
			t.position = p;
			Undo.RegisterCompleteObjectUndo(t, "");
		}
	}

	[MenuItem("Editor/Y座標揃え")]
	static void AlignAxisY()
	{
		if (Selection.transforms.Length < 2)
		{
			return;
		}

		Undo.RecordObjects(Selection.transforms, "modify position");

		float y = Selection.transforms[0].position.y;

		foreach (var t in Selection.transforms)
		{
			Vector3 p = t.position;
			p.y = y;
			t.position = p;
		}
	}

	[MenuItem("Editor/X座標揃え")]
	static void AlignAxisX()
	{
		if (Selection.transforms.Length < 2)
		{
			return;
		}

		Undo.RecordObjects(Selection.transforms, "modify position");

		float x = Selection.transforms[0].position.x;
		
		foreach (var t in Selection.transforms)
		{
			Vector3 p = t.position;
			p.x = x;
			t.position = p;
		}
	}
}
