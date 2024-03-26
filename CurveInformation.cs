using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class CurveInformation : MonoBehaviour
{
	[SerializeField] SplineContainer splineContainer;
	[SerializeField] int numberOfParts = 10;
	[SerializeField] int integerPart = 0;
	[SerializeField] int fractionalPart = 0;

	private int countOfCurves;
	private float sumOfLengths;

	private void Start()
	{
		countOfCurves = splineContainer.Splines[0].GetCurveCount<Spline>();
		sumOfLengths = 0;
	}

	// Update is called once per frame
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.C))
		{
			Spline spline = splineContainer.Splines[0];
			if (integerPart > countOfCurves - 1)
			{
				Debug.Log("НЕДОПУСТИМОЕ ЗНАЧЕНИЕ integerPart");
			}
			float t = integerPart + (fractionalPart / (float)100);
			Debug.Log($"t = {t}");
			Debug.Log($"CTS({integerPart}.{fractionalPart}) = {spline.CurveToSplineT(t)}");
		}

		if (Input.GetKeyDown(KeyCode.G))
		{
			Debug.Log($"CurveCount = {countOfCurves}");
			for (int i = 0; i < countOfCurves; i++)
			{
				Debug.Log($"CurveLength{i} = {splineContainer.Splines[0].GetCurveLength(i)}");
				sumOfLengths += splineContainer.Splines[0].GetCurveLength(i);
			}
			Debug.Log($"SumCurveLength = {sumOfLengths}");
			Debug.Log($"SplineLength = {splineContainer.Splines[0].GetLength()}");

			//Если оставить нижнюю строку, то код будет работать
			//а логироваться будет какое-то мусорное значение
			//то есть происходит неведомая х
			//Debug.Log($"SplineLength = {splineContainer.Splines[0].GetCurveLength(countOfCurves)}");
		}
	}
}
