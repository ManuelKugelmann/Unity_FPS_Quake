﻿using UnityEngine;
using System.Collections;
//List 자료형을 사용하기 위해 추가해야하는 네임스페이스
using System.Collections.Generic;

public class GameMgr : MonoBehaviour {
	//몬스터가 출현할 위치를 담을 배열
	public Transform[] points;
	//몬스터 프리팹을 할당할 변수
	public GameObject monsterPrefab;
	//몬스터를 미리 생성해 저장할 리스트 자료형
	public List<GameObject> monsterPool = new List<GameObject>();

	//몬스터를 발생시킬 주기
	public float createTime = 2.0f;
	//몬스터의 최대 발생 개수
	public int maxMonster = 10;
	//게임 종료 여부 변수
	public bool isGameOver = false;
	//싱글턴 패턴을 위한 인스턴스 변수 선언
	public static GameMgr instance = null;
	//사운드의 볼륨 설정 변수
	public float sfxVolumn = 1.0f;
	//사운드 뮤트 기능
	public bool isSfMute = false;

	void Awake(){
		//GameMgr 클래스를 인스턴스에 대입
		instance = this;
	}


	// Use this for initialization
	void Start () {
		points = GameObject.Find ("SpawnPoint").GetComponentsInChildren<Transform> ();

		//몬스터를 생성해 오브젝트 풀에 저장
		for (int i = 0; i<maxMonster; i++) {
			GameObject monster = (GameObject)Instantiate(monsterPrefab);
			//생성한 몬스터의 이름 설정
			monster.name = "Monster_" + i.ToString();
			//생성한 몬스터를 비활성화
			monster.SetActive (false);
			//생성한 몬스터를 오브젝트풀에 추가
			monsterPool.Add (monster);
		}

		if (points.Length > 0) {
			StartCoroutine(this.CreateMonster());
		}
	}
	//몬스터 생성 코루틴 함수
	IEnumerator CreateMonster() {
		while (!isGameOver) {
			//몬스터 생성 주기 시간만큼 메인 루프에 양보
			yield return new WaitForSeconds(createTime);
			//플레이어가 사망했을 때 코루틴을 종료해 다음 루틴을 진행하지 않음
			if(isGameOver) yield break;

			//오브젝트 풀의 처음부터 끝까지 순회
			foreach(GameObject monster in monsterPool){
				//비활성화 여부로 사용 가능한 몬스터를 판단
				if(!monster.activeSelf) {
					//몬스터를 출현시킬 위치의 인덱스값을 추출
					int idx = Random.Range (1, points.Length);
					//몬스터의 출현 위치를 설정
					monster.transform.position = points[idx].position;
					//몬스터를 활성화함
					monster.SetActive(true);
					//오브젝트 풀에서 몬스터 프리팹하나를 활성화 한후 for루프를 빠져나감
					break;
				}
			}
			/*
			int monsterCount = (int)GameObject.FindGameObjectsWithTag("MONSTER").Length;
			if(monsterCount < maxMonster){
				yield return new WaitForSeconds(createTime);
				int idx = Random.Range (1, points.Length);
				Instantiate(monsterPrefab, points[idx].position, points[idx].rotation);
			}else {
				yield return null;
			}
			*/
		}
	}

	//사운드 공용 함수
	public void PlaySfx(Vector3 pos, AudioClip sfx)
	{
		//음소거 옵션이 설정되면 바로 빠져나감
		if (isSfMute)
			return;
		//게임 오브젝트를 동적으로 생성
		GameObject soundObj = new GameObject ("Sfx");
		//사운드 발생 위치 지정
		soundObj.transform.position = pos;
		//생성한 게임 오브젝트에 AudioSource 컴포넌트 추가
		AudioSource audioSource = soundObj.AddComponent<AudioSource> ();
		//AudioSource 속성 설정
		audioSource.clip = sfx;
		audioSource.minDistance = 10.0f;
		audioSource.maxDistance = 30.0f;
		//sfxVolumn 변수로 게임의 전체적인 볼륨 설정 가능
		audioSource.volume = sfxVolumn;
		//사운드 실행
		audioSource.Play ();

		//사운드의 플레이가 종료되면 동적으로 생성한 게임 오브젝트를 삭제
		Destroy (soundObj, sfx.length);
	}
}
