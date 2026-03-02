namespace War3Net.Runtime.Enums
{
    public sealed class RealGameState : GameState
    {
        private static readonly Dictionary<int, RealGameState> _states = GetTypes().ToDictionary(t => (int)t, t => new RealGameState(t));

        private readonly Type _type;

        private RealGameState(Type type)
        {
            _type = type;
        }

        public enum Type
        {
            TimeOfDay = 2,
        }

        public static implicit operator Type(RealGameState realGameState) => realGameState._type;

        public static explicit operator int(RealGameState realGameState) => (int)realGameState._type;

        public static RealGameState GetRealGameState(int i)
        {
            if (!_states.TryGetValue(i, out var realGameState))
            {
                realGameState = new RealGameState((Type)i);
                _states.Add(i, realGameState);
            }

            return realGameState;
        }

        private static IEnumerable<Type> GetTypes()
        {
            foreach (Type type in Enum.GetValues(typeof(Type)))
            {
                yield return type;
            }
        }
    }
}