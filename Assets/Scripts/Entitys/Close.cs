using UnityEngine;
using UnityEngine.AI;

public class Close : Enemy
{
    public Animator closeAnimation; //행동 애니메이션

    protected float distance; //자신과 타겟간의 거리

    public HP_UI hp_ui;

    private NavMeshAgent NavMeshAgent;
    private CapsuleCollider capsuleCollider;

    // Start is called before the first frame update
    void Start()
    {
        NavMeshAgent = GetComponent<NavMeshAgent>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        switch (rank)
        {
            case Rank.Nor:
                //체력 배정
                STARTHP = 150;
                NavMeshAgent.speed = 2.75f;
                break;

            case Rank.Eli:
                //체력 배정
                STARTHP = 400;
                NavMeshAgent.speed = 3.5f;
                break;
        }

        this.HITPOINT = this.STARTHP;

        hp_ui.HPSlider.maxValue = STARTHP;

        StartCoroutine(Approach());
    }



    // Update is called once per frame
    void Update()
    {
        hp_ui.HPSlider.value = HITPOINT;

        distance = Vector3.Distance(Barricade.instance.transform.position, this.transform.position);


        if (isDead == false && mapAgent.remainingDistance <= (mapAgent.stoppingDistance + 0.5f))
        {
            closeAnimation.SetTrigger("Attack");//근거리 공격

        }
        else if (isDead == false && mapAgent.remainingDistance > (mapAgent.stoppingDistance + 0.5f))
        {
            //이동
            closeAnimation.SetTrigger("Move");
        }

        if (this.HITPOINT <= 0)
        {
            isDead = true;
        }

        if (isDead == true)
        {
            closeAnimation.SetTrigger("Death");

            Destroy(this.gameObject, 2f);

            //리지드 바디 및 AI 비 활성화
            NavMeshAgent.enabled = false;
            capsuleCollider.enabled = false;
        }


        if (SpawnSystem.instance.REMAIN <= 0 || Time.timeScale == 0)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnDisable()
    {
        if (isDead == true)
        {
            switch (rank)
            {
                case Rank.Nor:
                    SpawnSystem.instance.NUM_CLOSE -= 1;
                    break;

                case Rank.Eli:
                    SpawnSystem.instance.NUM_CLOSE_E -= 1;
                    break;

            }
        }
    }
}
