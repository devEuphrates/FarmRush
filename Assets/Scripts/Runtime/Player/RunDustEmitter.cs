using UnityEngine;

namespace Euphrates
{
    public class RunDustEmitter : MonoBehaviour
	{
		[SerializeField] ParticleSystem _leftFootParticles;
		[SerializeField] ParticleSystem _rightFootParticles;

		public void RightFoot()
        {
			PlayParticle(true);
        }

		public void LeftFoot()
        {
			PlayParticle(false);
		}

		void PlayParticle(bool right)
        {
			SoundManager.PlayClip("step", 0, .2f, true);

			if (right)
            {
				_rightFootParticles.Play();
				return;
            }

			_leftFootParticles.Play();
        }
	}
}
