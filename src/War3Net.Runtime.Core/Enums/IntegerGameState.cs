namespace War3Net.Runtime.Enums
{
    public sealed class IntegerGameState : GameState
    {
        private static readonly Dictionary<int, IntegerGameState> _states = GetTypes().ToDictionary(t => (int)t, t => new IntegerGameState(t));

        private readonly Type _type;

        private IntegerGameState(Type type)
        {
            _type = type;
        }

        public enum Type
        {
            DivineIntervention = 0,
            Disconnected = 1,
        }

        public static implicit operator Type(IntegerGameState integerGameState) => integerGameState._type;

        public static explicit operator int(IntegerGameState integerGameState) => (int)integerGameState._type;

        public static IntegerGameState GetIntegerGameState(int i)
        {
            if (!_states.TryGetValue(i, out var integerGameState))
            {
                integerGameState = new IntegerGameState((Type)i);
                _states.Add(i, integerGameState);
            }

            return integerGameState;
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