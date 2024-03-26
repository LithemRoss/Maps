using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class KnotInformation : MonoBehaviour
{
	private SplineContainer splineContainer;
	private PlayerMove playerMove;
	//список индексов, которые соответствуют этому узлу
	private List<SplineKnotIndex> myIndexes;

	//В старте получаем ссылки на сплайн контейнер и скрипт движения игрока
	private void Start()
	{
		playerMove = FindObjectOfType<PlayerMove>();
		if (playerMove == null)
		{
			Debug.Log("Не получил компонент PlayerMove");
		}
		splineContainer = FindObjectOfType<SplineContainer>();
		if (splineContainer == null)
		{
			Debug.Log("Не получил компонент SplineContainer");
		}
	}

	private void OnMouseDown()
	{
		//если индексы не установлены, то установить
		if (myIndexes == null)
		{
			SetListOfKnotIndex();
		}

		//попытаться установить индексы узла-цели у игрока
		playerMove.TrySetTarget(myIndexes, transform.position);
	}

	//устанавливаем индексы данного узла
	//то есть индексы knots, которые закреплены за этим узлом
	private void SetListOfKnotIndex()
	{
		myIndexes = new List<SplineKnotIndex>();
		for (int i = 0; i < splineContainer.Splines.Count; i++)
		{
			for (int j = 0; j < splineContainer.Splines[i].Count; j++)
			{
				if (transform.position == (Vector3)splineContainer.Splines[i][j].Position)
				{
					myIndexes.Add(new SplineKnotIndex(i, j));
				}
			}
		}
	}
}
