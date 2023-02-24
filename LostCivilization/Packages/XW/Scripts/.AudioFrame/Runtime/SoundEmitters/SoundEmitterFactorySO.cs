
using TTM.FactoryExtension;

namespace TTM.AudioFrame
{
	public class SoundEmitterFactorySO : FactorySO<SoundEmitter>
	{
		public SoundEmitter prefab = default;

		public override SoundEmitter Create()
		{
			return Instantiate(prefab);
		}
	}
}