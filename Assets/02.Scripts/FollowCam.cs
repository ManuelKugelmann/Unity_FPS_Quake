﻿using UnityEngine;
using System.Collections;

public class FollowCam : MonoBehaviour {
	public Transform targetTr;  //추적할 타깃 게임오브젝트의 Transform 변수
	public float dist=5.0f;   //카메라와의 일정 거리
	public float height = 3.0f;  //카메라의 높이 설정
	public float dampTrace = 10.0f;  //부드러운 추적을 위한 변수
 	private Transform tr; //카메라 자신의 Transform 변수
	// Use this for initialization
	void Start () {
		//카메라 자신의 Transform 컴포넌트를 tr에 할당
		tr = GetComponent<Transform> ();
	}

	//Update 함수 호출 이후 한 번씩 호출되는 함수인 LateUpdate 사용
	//추적할 타킷의 이동이 종료된 이후에 카메라가 추적하기 위해 LateUpdate 사용
	void Update () {
		//카메라의 위치를 추적대상의 dist 변수만큼 뒤쪽으로 배치하고
		//height 변수만큼 위로 올림
		tr.position = Vector3.Lerp (tr.position, 		//시작 위치
		   	targetTr.position - (targetTr.forward * dist) + (Vector3.up * height) //종료 위치
		    , Time.deltaTime * dampTrace);  //보간 시간
		//카메라가 타깃 게임오브젝트를 바라보게 설정
		tr.LookAt (targetTr.position);
	}
}
