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

	//������ �������� ���� (�� ���� ������� �����)
	private List<SplineKnotIndex> currentIndexes;
	//������ ���� ����, �� ������� ����� �������������
	private List<SplineKnotIndex> targetIndexes;
	private Vector3 targetKnotPosition;

	private bool isMoving;
	private bool beginningMovement;
	private bool endingMovement;

	private float distancePercentage;
	//������ ������ ����� �������-����� ������� ������ ����-����� ������� ���� ����
	private int3 wayData;

	private void Start()
	{
		//����� ��������� currentIndexes
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
		//�������� ��������, ���� ���� �� �����, �������� � �� �� ���������
		if (!AreEqual(currentIndexes, targetIndexes)
			&& AreAdjacent(currentIndexes, targetIndexes)
			&& !isMoving)
		{
			isMoving = true;
			beginningMovement = true;
		}

		if (isMoving)
		{
			Move();//������� ������, ��� �� ����������� �������� (������ = ���)
			if (endingMovement)
			{
				isMoving = false;
				endingMovement = false;
			}
		}
	}

	//��������� �� current (wayData.y) � target (wayData.z)
	private void Move()
	{
		//� ������ �������� ���� ����������, ��� �� ���������
		if (beginningMovement)
		{
			distancePercentage = splineContainer.Splines[wayData.x].CurveToSplineT(wayData.y);
			beginningMovement = false;
		}

		//���� �� ����������� �������
		if (wayData.y < wayData.z)
		{
			distancePercentage += speed * Time.deltaTime / splineContainer.Splines[wayData.x].GetLength();

			if (distancePercentage > splineContainer.Splines[wayData.x].CurveToSplineT(wayData.z))
			{
				//������������ � ������ ���� � ����������� ��������
				EndingMovement();
			}
		}
		//���� ������ ����������� �������
		else
		{
			distancePercentage -= speed * Time.deltaTime / splineContainer.Splines[wayData.x].GetLength();

			if (distancePercentage < splineContainer.Splines[wayData.x].CurveToSplineT(wayData.z))
			{
				//������������ � ������ ���� � ����������� ��������
				EndingMovement();
			}
		}

		Vector3 newPosition = splineContainer.Splines[wayData.x].EvaluatePosition(distancePercentage);
		transform.position = newPosition;

		//��� ���������� ������� ����
		if ((transform.position - targetKnotPosition).magnitude < accuracy)
		{
			EndingMovement();
		}
	}

	//������������� �������� wayData
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

	//������������� � ������-���� (����������, ���� ������� � ���� ������)
	private void TranslateToTarget()
	{
		transform.position = targetKnotPosition;
		currentIndexes.Clear();
		foreach (var index in targetIndexes)
		{
			currentIndexes.Add(index);
		}
	}

	//��������� Move()
	private void EndingMovement()
	{
		endingMovement = true;
		TranslateToTarget();
	}

	//����� �� ���� � ��������� list1 � list2 ����� ������
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

	//���������� ���������� ������� ����-����
	//����-���� - ��� ����, � �������� �� ����� �������������
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

	//���������, ����� �� ����
	//����� �������� ��������������� ��������
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

	//�������� �� ���� � ��������� list1 � list2?
	private bool AreAdjacent(List<SplineKnotIndex> list1, List<SplineKnotIndex> list2)
	{
		//������ ����� ����� �������-������� ����-������� ���� (�� ������ �� �������)
		List<int3> indexes;
		//���� ��� �� ����� ����� ��������, �� ��� �� ��������
		if (!HaveCommonSplines(out indexes, list1, list2))
		{
			return false;
		}

		//������ ������� ����� ������ - ������� ������ - ������� ������
		int3 resultTriple;
		//���� ����������, �� ����� ������� ������������ ����� ����� ���� ���
		foreach (var triple in indexes)
		{
			bool linkedKnotExist = false;
			for (int i = triple.y + 1; i < triple.z; i++)
			{
				//���� ����� ������������� ��� ������ ������ ����-����, �� ���� �� adjacent
				//���� ����-����� �� ������, �� adjacent (��������)
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
