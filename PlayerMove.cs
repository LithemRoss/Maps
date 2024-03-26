using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;

public class PlayerMove : MonoBehaviour
{
	[SerializeField] private SplineContainer splineContainer;
	[SerializeField] private float speed = 3f;
	[SerializeField] private float accuracy = 0.01f;

	//индекс текущего узла (то есть индексы нотов)
	private List<SplineKnotIndex> currentIndexes;
	//индекс того узла, на который хотим переместиться
	private List<SplineKnotIndex> targetIndexes;
	private Vector3 targetKnotPosition;

	private bool isMoving;
	private bool beginningMovement;
	private bool endingMovement;

	private float distancePercentage;
	//хранит тройку номер сплайна-номер индекса откуда идем-номер индекса куда идем
	private int3 wayData;

	private void Start()
	{
		//Хотим вычислить currentIndexes
		currentIndexes = new List<SplineKnotIndex>();
		targetIndexes = new List<SplineKnotIndex>();
		for (int i = 0; i < splineContainer.Splines.Count; i++)
		{
			for (int j = 0; j < splineContainer.Splines[i].Count; j++)
			{
				if (transform.position == (Vector3)splineContainer.Splines[i][j].Position)
				{
					currentIndexes.Add(new SplineKnotIndex(i, j));
					targetIndexes.Add(new SplineKnotIndex(i, j));
				}
			}
		}
		isMoving = false;
		beginningMovement = false;
		endingMovement = false;
	}

	private void Update()
	{
		//начинаем движение, если узлы не равны, соседние и мы не двигаемся
		if (!AreEqual(currentIndexes, targetIndexes)
			&& AreAdjacent(currentIndexes, targetIndexes)
			&& !isMoving)
		{
			isMoving = true;
			beginningMovement = true;
		}

		if (isMoving)
		{
			Move();
			if (endingMovement)
			{
				isMoving = false;
				endingMovement = false;
			}
		}
	}

	//Двигаемся из current (wayData.y) в target (wayData.z)
	private void Move()
	{
		//в начале движения надо установить, где мы находимся
		if (beginningMovement)
		{
			distancePercentage = splineContainer.Splines[wayData.x].CurveToSplineT(wayData.y);
			beginningMovement = false;
		}

		//идем по направлению сплайна
		if (wayData.y < wayData.z)
		{
			distancePercentage += speed * Time.deltaTime / splineContainer.Splines[wayData.x].GetLength();

			if (distancePercentage > splineContainer.Splines[wayData.x].CurveToSplineT(wayData.z))
			{
				//перемещаемся в нужный узел и заканчиваем движение
				EndingMovement();
			}
		}
		//идем против направления сплайна
		else
		{
			distancePercentage -= speed * Time.deltaTime / splineContainer.Splines[wayData.x].GetLength();

			if (distancePercentage < splineContainer.Splines[wayData.x].CurveToSplineT(wayData.z))
			{
				//перемещаемся в нужный узел и заканчиваем движение
				EndingMovement();
			}
		}

		Vector3 newPosition = splineContainer.Splines[wayData.x].EvaluatePosition(distancePercentage);
		transform.position = newPosition;

		//при достижении нужного узла
		if ((transform.position - targetKnotPosition).magnitude < accuracy)
		{
			EndingMovement();
		}
	}

	//устанавливает значение wayData
	private void SetWayData(int3 triple)
	{
		foreach (var index in currentIndexes)
		{
			if (index == new SplineKnotIndex(triple.x, triple.y))
			{
				wayData = triple;
				break;
			}
			if (index == new SplineKnotIndex(triple.x, triple.z))
			{
				wayData = new int3(triple.x, triple.z, triple.y);
				break;
			}
		}
	}

	//переместиться в таргет-узел (вызывается, если подошли к нему близко)
	private void TranslateToTarget()
	{
		transform.position = targetKnotPosition;
		currentIndexes.Clear();
		foreach (var index in targetIndexes)
		{
			currentIndexes.Add(index);
		}
	}

	//окончание Move()
	private void EndingMovement()
	{
		endingMovement = true;
		TranslateToTarget();
	}

	//имеют ли узлы с индексами list1 и list2 общий сплайн
	private bool HaveCommonSplines(out List<int3> indexes, List<SplineKnotIndex> list1, List<SplineKnotIndex> list2)
	{
		indexes = new List<int3>();
		bool haveCommonSplines = false;
		foreach (var index1 in list1)
		{
			foreach (var index2 in list2)
			{
				if (index1.Spline == index2.Spline)
				{
					haveCommonSplines = true;
					if (index2.Knot > index1.Knot)
					{
						indexes.Add(new int3(index1.Spline, index1.Knot, index2.Knot));
					}
					else
					{
						indexes.Add(new int3(index1.Spline, index2.Knot, index1.Knot));
					}
				}
			}
		}

		return haveCommonSplines;
	}

	//попытаться установить индексы узла-цели
	//узел-цель - это узел, к которому мы хотим переместиться
	public bool TrySetTarget(List<SplineKnotIndex> newIndexes, Vector3 newPosition)
	{
		if (isMoving)
		{
			return false;
		}

		targetIndexes.Clear();
		foreach (var index in newIndexes)
		{
			targetIndexes.Add(index);
		}
		targetKnotPosition = newPosition;
		return true;
	}

	//проверяет, равны ли узлы
	//через проверку соответствующих индексов
	private bool AreEqual(List<SplineKnotIndex> list1, List<SplineKnotIndex> list2)
	{
		if (list1.Count != list2.Count)
		{
			return false;
		}

		foreach (var index in list1)
		{
			if (!list2.Contains(index))
			{
				return false;
			}
		}

		return true;
	}

	//соседние ли узлы с индексами list1 и list2?
	private bool AreAdjacent(List<SplineKnotIndex> list1, List<SplineKnotIndex> list2)
	{
		//список троек номер сплайна-меньший узел-больший узел (по номеру на сплайне)
		List<int3> indexes;
		//если они не имеют общих сплайнов, то они не соседние
		if (!HaveCommonSplines(out indexes, list1, list2))
		{
			return false;
		}

		//тройка номеров общий сплайн - меньший индекс - больший индекс
		int3 resultTriple;
		//надо определить, на каком сплайне слинкованных узлов между ними нет
		foreach (var triple in indexes)
		{
			bool linkedKnotExist = false;
			for (int i = triple.y + 1; i < triple.z; i++)
			{
				//если между интересующими нас узлами найдем линк-узел, то узлы не adjacent
				//если линк-узлов не найдем, то adjacent (соседние)
				if (splineContainer.KnotLinkCollection.GetKnotLinks(new SplineKnotIndex(triple.x, i)).Count != 1)
				{
					linkedKnotExist = true;
					break;
				}
			}
			if (!linkedKnotExist)
			{
				resultTriple = triple;
				SetWayData(resultTriple);
				return true;
			}
		}

		return false;
	}
}
