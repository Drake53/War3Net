namespace War3Net.Runtime.Enums
{
    public sealed class PlayerSlotState : Handle
    {
        private static readonly Dictionary<int, PlayerSlotState> _states = GetTypes().ToDictionary(t => (int)t, t => new PlayerSlotState(t));

        private readonly Type _type;

        private PlayerSlotState(Type type)
        {
            _type = type;
        }

        public enum Type
        {
            Empty = 0,
            Playing = 1,
            Left = 2,
        }

        public static implicit operator Type(PlayerSlotState playerSlotState) => playerSlotState._type;

        public static explicit operator int(PlayerSlotState playerSlotState) => (int)playerSlotState._type;

        public static PlayerSlotState GetPlayerSlotState(int i)
        {
            if (!_states.TryGetValue(i, out var playerSlotState))
            {
                playerSlotState = new PlayerSlotState((Type)i);
                _states.Add(i, playerSlotState);
            }

            return playerSlotState;
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