using UnityEngine;

namespace GamePlay
{
    public class TestSoundMaker : MonoBehaviour
    {
        [SerializeField] private AudioSource source = null;

        [SerializeField] private float soundRange = 25f;
        
        [SerializeField] private Sound.SoundType soundType = Sound.SoundType.Dangerous;
        void Update()
        {

            // if (Input.GetKeyDown(KeyCode.A))
            // {
            //     Debug.Log("A pressed");
            //     if (source.isPlaying) //If already playing a sound, don't allow overlapping sounds 
            //         return;

            //     source.Play();

            //     var sound = new Sound(transform.position, soundRange, soundType);

            //     Sounds.MakeSound(sound);
            // }


        }
    }
        
}
