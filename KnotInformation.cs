using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class KnotInformation : MonoBehaviour
{
	private SplineContainer splineContainer;
	private PlayerMove playerMove;
	//������ ��������, ������� ������������� ����� ����
	private List<SplineKnotIndex> myIndexes;

	//� ������ �������� ������ �� ������ ��������� � ������ �������� ������
	private void Start()
	{
		playerMove = FindObjectOfType<PlayerMove>();
		if (playerMove == null)
		{
			Debug.Log("�� ������� ��������� PlayerMove");
		}
		splineContainer = FindObjectOfType<SplineContainer>();
		if (splineContainer == null)
		{
			Debug.Log("�� ������� ��������� SplineContainer");
		}
	}

	private void OnMouseDown()
	{
		//���� ������� �� �����������, �� ����������
		if (myIndexes == null)
		{
			SetListOfKnotIndex();
		}

		//���������� ���������� ������� ����-���� � ������
		playerMove.TrySetTarget(myIndexes, transform.position);
	}

	//������������� ������� ������� ����
	//�� ���� ������� knots, ������� ���������� �� ���� �����
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
