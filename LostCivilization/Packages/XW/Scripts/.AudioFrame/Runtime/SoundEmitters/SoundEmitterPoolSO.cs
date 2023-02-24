using UnityEngine;
using TTM.FactoryExtension;

namespace TTM.AudioFrame
{
	public class SoundEmitterPoolSO : ComponentPoolSO<SoundEmitter>
	{
		[SerializeField]
		private SoundEmitterFactorySO _factory;

		public override IFactory<SoundEmitter> Factory
		{
			get
			{
				return _factory;
			}
			set
			{
				_factory = value as SoundEmitterFactorySO;
			}
		}
	}
}