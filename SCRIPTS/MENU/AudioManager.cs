using System.Collections;
using UnityEngine;

namespace MENU
{
    // ВИСЕТЬ ДОЛЖНО КАЖЫСЬ НА КАМЕРЕ

    public class AudioManager : MonoBehaviour
    {
        // поля куда потянем звуки руками
        [Header("General")] [SerializeField] private AudioClip winSound;
        [SerializeField] private AudioClip noSound;
        [SerializeField] private AudioClip yesSound;
        [SerializeField] private AudioClip tickSound;
        [SerializeField] private AudioClip pumSound;
        [SerializeField] private AudioClip pressSound;
        [SerializeField] private AudioClip shuhSound;

        [Space] [Header("HOG")] [SerializeField]
        private AudioClip dogSound;

        [Space] [Header("Залупливаемые")] [SerializeField]
        private AudioClip rainSound;

        [SerializeField] private AudioClip nightSound;
        [SerializeField] private AudioClip stepSound;
        [SerializeField] private AudioClip music1;
        [SerializeField] private AudioClip music2;
        [SerializeField] private AudioClip music3;
        [SerializeField] private AudioClip clockSound;

        public bool doTopat; // не топает пока не скажут
        private float stepSpeed = 0.4f; // между шагами
        private AudioSource _mainAudiosource;

        private AudioClip _curSoundToLoop;
        private bool clockIsTicking = false;

        private void Awake()
        {
            _mainAudiosource = GetComponent<AudioSource>(); // добавить его руками ADD!
        }


        //------------ отдельные функции со звуками --------------------
        public void PlaySound(string whichSound)
        {
            _mainAudiosource.pitch = 1; // восстанавливаем тональность

            if (whichSound == null)
            {
                Debug.Log("Ошибка аудиоменеджера - Не назначен звук");
            }

            switch (whichSound)
            {
                case "WIN":
                    _mainAudiosource.PlayOneShot(winSound);
                    break;

                case "YES":
                    _mainAudiosource.PlayOneShot(yesSound);
                    break;

                case "NO":
                    _mainAudiosource.PlayOneShot(noSound);
                    break;

                case "TICK":
                    _mainAudiosource.PlayOneShot(tickSound);
                    break;

                case "PUM":
                    _mainAudiosource.PlayOneShot(pumSound);
                    break;

                case "PRESS":
                    _mainAudiosource.PlayOneShot(pressSound);
                    break;

                case "SHUH":
                    _mainAudiosource.PlayOneShot(shuhSound);
                    break;

                case "DOG":
                    _mainAudiosource.PlayOneShot(dogSound);
                    break;


                // ------
                case "STEPS":
                    StepSoundFunction(stepSound, stepSpeed, 0.8f, 1.2f);
                    break;

                case "NIGHT":
                    StepSoundFunction(nightSound, 0.1f, 0.8f, 1.2f);
                    break;

                case "RAIN":
                    StepSoundFunction(rainSound, 0.1f, 0.8f, 1.2f);
                    break;

                case "MUSIC1":
                    StepSoundFunction(music1, 0.1f, 1, 1.1f);
                    break;

                case "MUSIC2":
                    StepSoundFunction(music2, 0.1f, 1, 1.1f);
                    break;

                case "MUSIC3":
                    StepSoundFunction(music3, 0.1f, 1, 1.1f);
                    break;

                case "CLOCK":
                    SoundLoopFunction(clockSound);
                    break;

                default:
                    Debug.Log(" Нет такого звука, вращаем барабан!");
                    break;
            }
        }


        //-------------------------- шаги ----------------------------------------
        public void StopTopat()
        {
            doTopat = false;
        }

        public void StartTopat()
        {
            doTopat = true;
        }


        private void StepSoundFunction(AudioClip Sound, float delay, float rndFrom, float rindTo)
            // ссылка на звук,   пауза между,   рандом тональности от, до ( шаги: (0.8f, 1.2f)
        {
            if (Sound == stepSound && !doTopat) // для этого звука нужна проверка
            {
                Debug.Log("Если условие - не топатьs");
                return;
            }

            if (Sound == null)
            {
                Debug.Log("Этот звук еще не назначили");
            }

            StartCoroutine(StepStep());


            IEnumerator StepStep()
            {
                // yield return new WaitForSeconds(delay); // пока разгоняется

                while (doTopat) // бесконечный цикл топания
                {
                    _mainAudiosource.pitch = Random.Range(rndFrom, rindTo); // не унылая тональность топания
                    _mainAudiosource.PlayOneShot(Sound);
                    yield return new WaitForSeconds(stepSpeed);
                }
            }
        }
        //---------------------------------------------------

        // AAN

        private void SoundLoopFunction(AudioClip loopClip)
        {
            _curSoundToLoop = loopClip;
            // StartCoroutine(LoopAudio());
        }

        IEnumerator LoopAudio()
        {
            //audio = GetComponent<AudioSource>();
            float length = _curSoundToLoop.length;

            while (true)
            {
                if (clockIsTicking)
                {
                    clockIsTicking = true;
                    _mainAudiosource.PlayOneShot(_curSoundToLoop);
                    yield return new WaitForSeconds(length);
                }
            }
        }

        public void StopLoopedSound()
        {
            clockIsTicking = false;
        }


        public void StopSound()
        {
            _mainAudiosource.Stop();
        }
        
    } // close class
}