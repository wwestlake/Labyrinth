namespace Labyrinth.API.Entities.Characters
{
    public class Stat
    {
        public int BaseValue { get; set; }  // The core value of the stat
        public int BuffValue { get; private set; }  // Temporary positive modifier
        public int DebuffValue { get; private set; }  // Temporary negative modifier
        public DateTime? BuffTimestamp { get; private set; }  // Timestamp when the buff was applied
        public TimeSpan? BuffDuration { get; private set; }  // Duration of the buff
        public DateTime? DebuffTimestamp { get; private set; }  // Timestamp when the debuff was applied
        public TimeSpan? DebuffDuration { get; private set; }  // Duration of the debuff

        public int TotalValue => BaseValue + BuffValue - DebuffValue;  // Total effective value of the stat

        // Apply a buff with a value and duration
        public void ApplyBuff(int value, TimeSpan duration)
        {
            BuffValue = value;
            BuffTimestamp = DateTime.UtcNow;
            BuffDuration = duration;
        }

        // Apply a debuff with a value and duration
        public void ApplyDebuff(int value, TimeSpan duration)
        {
            DebuffValue = value;
            DebuffTimestamp = DateTime.UtcNow;
            DebuffDuration = duration;
        }

        // Check if the buff has expired and reset it if it has
        public void CheckBuffExpiration()
        {
            if (BuffTimestamp.HasValue && BuffDuration.HasValue)
            {
                if (DateTime.UtcNow > BuffTimestamp.Value.Add(BuffDuration.Value))
                {
                    BuffValue = 0;
                    BuffTimestamp = null;
                    BuffDuration = null;
                }
            }
        }

        // Check if the debuff has expired and reset it if it has
        public void CheckDebuffExpiration()
        {
            if (DebuffTimestamp.HasValue && DebuffDuration.HasValue)
            {
                if (DateTime.UtcNow > DebuffTimestamp.Value.Add(DebuffDuration.Value))
                {
                    DebuffValue = 0;
                    DebuffTimestamp = null;
                    DebuffDuration = null;
                }
            }
        }

        // Recalculate the total value of the stat
        public void RecalculateTotalValue()
        {
            CheckBuffExpiration();
            CheckDebuffExpiration();
        }
    }
}
