using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

namespace Ursaanimation.CubicFarmAnimals
{
    public class AnimationController : MonoBehaviour
    {
        public Animator animator;
        public string walkForwardAnimation = "walk_forward";
        public string walkBackwardAnimation = "walk_backwards";
        public string runForwardAnimation = "run_forward";
        public string turn90LAnimation = "turn_90_L";
        public string turn90RAnimation = "turn_90_R";
        public string trotAnimation = "trot_forward";
        public string sittostandAnimation = "sit_to_stand";
        public string standtositAnimation = "stand_to_sit";
        public NavMeshAgent agent;
        public float waitTimeAtPoint = 5f;
        public float wanderRadius = 10f;
        public int heat = 10;
        bool isDead = false;
        int currentHeat;
        public Image heatBar;
        void Start()
        {
            animator = GetComponent<Animator>();
            StartCoroutine(MoveToRandomPosition());
            UpdateHeatBar();
            currentHeat = heat;
        }

        void Update()
        {
            #region
            //if (Input.GetKeyDown(KeyCode.W))
            //{
            //    animator.Play(walkForwardAnimation);
            //}
            //else if (Input.GetKeyDown(KeyCode.S))
            //{
            //    animator.Play(walkBackwardAnimation);
            //}
            //else if (Input.GetKeyDown(KeyCode.Alpha1))
            //{
            //    animator.Play(runForwardAnimation);
            //}
            //else if (Input.GetKeyDown(KeyCode.A))
            //{
            //    animator.Play(turn90LAnimation);
            //}
            //else if (Input.GetKeyDown(KeyCode.D))
            //{
            //    animator.Play(turn90RAnimation);
            //}
            //else if (Input.GetKeyDown(KeyCode.Alpha2))
            //{
            //    animator.Play(trotAnimation);
            //}
            //else if (Input.GetKeyDown(KeyCode.Alpha4))
            //{
            //    animator.Play(sittostandAnimation);
            //}
            //else if (Input.GetKeyDown(KeyCode.Alpha3))
            //{
            //    animator.Play(standtositAnimation);
            //}
            #endregion

            if (!agent.pathPending && agent.remainingDistance < 0.5f)
            {
                //MoveToRandomPosition();
            }
        }

        public void TakeDamage(int damage)
        {
            if (currentHeat > 0 && !isDead)
            {
                currentHeat -= damage;
                currentHeat = Mathf.Clamp(currentHeat, 0, heat);
                UpdateHeatBar();
            } else if( currentHeat <= 0)
            {
                isDead = true;
                gameObject.SetActive(false);
                StartCoroutine(Respawn());
            }
        }

        IEnumerator Respawn()
        {
            yield return new WaitForSeconds(5f);
            gameObject.SetActive(true);
            isDead = false;
            heat = 10;
            currentHeat = heat;
            UpdateHeatBar();
        }
        void UpdateHeatBar()
        {

            heatBar.fillAmount = (float)currentHeat / heat;
        }

        IEnumerator MoveToRandomPosition()
        {
            while (true)
            {
                Vector3 randomDirection = Random.insideUnitSphere * wanderRadius;
                randomDirection += transform.position;

                NavMeshHit hit;
                if (NavMesh.SamplePosition(randomDirection, out hit, wanderRadius, NavMesh.AllAreas))
                {
                    agent.SetDestination(hit.position);
                    animator.SetBool("isWalk", true);

                    // ∆дЄм, пока агент дойдЄт до точки
                    while (!agent.pathPending && agent.remainingDistance > 0.5f)
                    {
                        yield return null;
                    }

                    // ќжидаем 5 секунд на месте
                    animator.SetBool("isWalk", false);
                    yield return new WaitForSeconds(waitTimeAtPoint);
                }

                // —ледующа€ итераци€
                yield return null;
            }
        }
    }
}
