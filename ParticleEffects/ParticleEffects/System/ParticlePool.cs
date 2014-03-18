namespace ParticleEffects.System
{
    public class ParticlePool
    {
        private readonly Particle[] _particles;

        public Particle this[int index]
        {
            get { return _particles[index]; }
            set { _particles[index] = value; }
        }

        public ParticlePool()
        {
            _particles = new Particle[10000];
        }

        public int[] GetRange(int amount)
        {
            var range = new int[amount];
            for (var i = 0; i < _particles.Length && amount > 0; ++i, --amount)
            {
                var particle = _particles[i];
                if (particle == null || !particle.IsAlive)
                    range[range.Length - amount] = i;
            }

            return range;
        }
    }
}
