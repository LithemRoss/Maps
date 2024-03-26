using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Splines;


//здесь рассматриваются не все методы, а только те, которые показались
//мне интересными и нужными на данный момент
//Документацию смотри здесь
//https://docs.unity3d.com/Packages/com.unity.splines@2.0/api/UnityEngine.Splines.html
public class TestSplines : MonoBehaviour
{
	[SerializeField] SplineContainer splineContainer1;
	[SerializeField] SplineContainer splineContainer2;
	[SerializeField] SplineContainer splineContainer3;
	[SerializeField] GameObject prefab;

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			Debug.Log($"Info about 1111111111");
			Debug.Log($"Info about SplineContainer");
			Debug.Log($"KnotLinkCollection = {splineContainer1.KnotLinkCollection}");
			Debug.Log($"Spline = {splineContainer1.Spline}");
			Debug.Log($"Splines = {splineContainer1.Splines}");
			Debug.Log($"CalculateLength() = {splineContainer1.CalculateLength()}");
			Debug.Log($"CalculateLength(0) = {splineContainer1.CalculateLength(0)}");
			Debug.Log($"EvaluatePosition(0, 0.5f) = {splineContainer1.EvaluatePosition(0, 0.5f)}");
			Debug.Log($"EvaluatePosition(0.5f) = {splineContainer1.EvaluatePosition(0.5f)}");

			//Лучше вместо Spline использовать Splines[0]
			Debug.Log($"Info about Spline");
			Debug.Log($"Closed = {splineContainer1.Spline.Closed}");
			Debug.Log($"Count = {splineContainer1.Spline.Count}");
			Debug.Log($"Spline[0] = {splineContainer1.Spline[0]}");
			Debug.Log($"Knots = {splineContainer1.Spline.Knots}");
			Debug.Log($"Contains[0]:[0][1] = {splineContainer1.Splines[0].Contains(splineContainer1.Splines[0][1])}");
			Debug.Log($"GetLenght() = {splineContainer1.Spline.GetLength()}");
			Debug.Log($"IndexOf[0]:[0][2] = {splineContainer1.Splines[0].IndexOf(splineContainer1.Splines[0][2])}");
			Debug.Log($"ToArray = {splineContainer1.Spline.ToArray()}");
			
			/*
			//как получить массив всех узлов в сплайне
			var b = splineContainer1.Splines[0];
			var d = b.Knots;
			foreach (var elem in b)
			{
				Debug.Log(elem.Position);
			}
			*/

			Debug.Log($"Info about KnotLinkCollection");
			Debug.Log($"Count = {splineContainer1.KnotLinkCollection.Count}");
			Debug.Log($"GetKnotLinks(0, 1) = {splineContainer1.KnotLinkCollection.GetKnotLinks(new SplineKnotIndex(0, 1))}");

			Debug.Log($"Info about BezierKnot");
			Debug.Log($"Position[0][1] = {splineContainer1.Splines[0][1].Position}");
			Debug.Log($"Rotation[0][1] = {splineContainer1.Splines[0][1].Rotation}");
			Debug.Log($"Equals[0][1]:[0][2] = {splineContainer1.Splines[0][1].Equals(splineContainer1.Splines[0][2])}");

			Debug.Log($"Using SplineUtility");
			//Аналогично работает Previous
			Debug.Log($"PositionOfNext[0][0] = {SplineUtility.Next<Spline>(splineContainer1.Splines[0], 0).Position}");
			Debug.Log($"PositionOfNext[0][0] = {splineContainer1.Splines[0].Next<Spline>(0).Position}");
		}

		if (Input.GetKeyDown(KeyCode.Alpha2))
		{
			Debug.Log($"Info about 2222222222");
			Debug.Log($"Info about SplineContainer");
			Debug.Log($"KnotLinkCollection = {splineContainer2.KnotLinkCollection}");
			Debug.Log($"Spline = {splineContainer2.Spline}");
			Debug.Log($"Splines = {splineContainer2.Splines}");
			Debug.Log($"CalculateLength() = {splineContainer2.CalculateLength()}");
			Debug.Log($"CalculateLength(0) = {splineContainer2.CalculateLength(0)}");
			Debug.Log($"EvaluatePosition(0, 0.5f) = {splineContainer2.EvaluatePosition(0, 0.5f)}");
			Debug.Log($"EvaluatePosition(0.5f) = {splineContainer2.EvaluatePosition(0.5f)}");

			//Лучше вместо Spline использовать Splines[0]
			Debug.Log($"Info about Spline");
			Debug.Log($"Closed = {splineContainer2.Spline.Closed}");
			Debug.Log($"Count = {splineContainer2.Spline.Count}");
			Debug.Log($"Spline[0] = {splineContainer2.Spline[0]}");
			Debug.Log($"Knots = {splineContainer2.Spline.Knots}");
			Debug.Log($"Contains[1]:[0][3] = {splineContainer2.Splines[1].Contains(splineContainer2.Splines[0][3])}");
			Debug.Log($"GetLenght() = {splineContainer2.Spline.GetLength()}");
			Debug.Log($"IndexOf[0]:[0][1] = {splineContainer2.Splines[0].IndexOf(splineContainer2.Splines[0][1])}");
			Debug.Log($"ToArray = {splineContainer2.Spline.ToArray()}");

			Debug.Log($"Info about KnotLinkCollection");
			Debug.Log($"Count = {splineContainer2.KnotLinkCollection.Count}");
			Debug.Log($"GetKnotLinks(0, 1) = {splineContainer2.KnotLinkCollection.GetKnotLinks(new SplineKnotIndex(0, 1))}");

			Debug.Log($"Info about BezierKnot");
			Debug.Log($"Position[0][3] = {splineContainer2.Splines[0][3].Position}");
			Debug.Log($"Rotation[0][3] = {splineContainer2.Splines[0][3].Rotation}");
			Debug.Log($"Equals[0][1]:[1][0] = {splineContainer2.Splines[0][1].Equals(splineContainer2.Splines[1][0])}");

			Debug.Log($"Using SplineUtility");
			//Аналогично работает Previous
			Debug.Log($"PositionOfNext[0][0] = {SplineUtility.Next<Spline>(splineContainer2.Splines[0], 0).Position}");
			Debug.Log($"PositionOfNext[0][0] = {splineContainer2.Splines[0].Next<Spline>(0).Position}");
		}

		if (Input.GetKeyDown(KeyCode.Alpha3))
		{
			Debug.Log($"Info about 3333333333");
			Debug.Log($"Info about SplineContainer");
			Debug.Log($"KnotLinkCollection = {splineContainer3.KnotLinkCollection}");
			Debug.Log($"Spline = {splineContainer3.Spline}");
			Debug.Log($"Splines = {splineContainer3.Splines}");
			Debug.Log($"CalculateLength() = {splineContainer3.CalculateLength()}");
			Debug.Log($"CalculateLength(0) = {splineContainer3.CalculateLength(0)}");
			Debug.Log($"EvaluatePosition(0, 0.5f) = {splineContainer3.EvaluatePosition(0, 0.5f)}");
			Debug.Log($"EvaluatePosition(0.5f) = {splineContainer3.EvaluatePosition(0.5f)}");

			//Лучше вместо Spline использовать Splines[0]
			Debug.Log($"Info about Spline");
			Debug.Log($"Closed = {splineContainer3.Spline.Closed}");
			Debug.Log($"Count = {splineContainer3.Spline.Count}");
			Debug.Log($"Spline[0] = {splineContainer3.Spline[0]}");
			Debug.Log($"Knots = {splineContainer3.Spline.Knots}");
			Debug.Log($"Contains[0]:[1][1] = {splineContainer3.Splines[0].Contains(splineContainer3.Splines[1][1])}");
			Debug.Log($"GetLenght() = {splineContainer3.Spline.GetLength()}");
			Debug.Log($"IndexOf[3]:[3][0] = {splineContainer3.Splines[3].IndexOf(splineContainer3.Splines[3][0])}");
			Debug.Log($"ToArray = {splineContainer3.Spline.ToArray()}");

			Debug.Log($"Info about KnotLinkCollection");
			Debug.Log($"Count = {splineContainer3.KnotLinkCollection.Count}");
			Debug.Log($"GetKnotLinks(0, 1) = {splineContainer3.KnotLinkCollection.GetKnotLinks(new SplineKnotIndex(0, 1))}");

			Debug.Log($"Info about BezierKnot");
			Debug.Log($"Position[2][0] = {splineContainer3.Splines[2][0].Position}");
			Debug.Log($"Rotation[2][0] = {splineContainer3.Splines[2][0].Rotation}");
			Debug.Log($"Equals[0][0]:[2][0] = {splineContainer3.Splines[0][0].Equals(splineContainer3.Splines[2][0])}");

			Debug.Log($"Using SplineUtility");
			//Аналогично работает Previous
			Debug.Log($"PositionOfNext[0][1] = {SplineUtility.Next<Spline>(splineContainer3.Splines[0], 1).Position}");
			Debug.Log($"PositionOfNext[0][1] = {splineContainer3.Splines[0].Next<Spline>(1).Position}");
		}
	}


}
