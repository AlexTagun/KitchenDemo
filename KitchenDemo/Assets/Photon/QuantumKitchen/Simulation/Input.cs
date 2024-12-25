using Photon.Deterministic;

namespace Quantum
{
    partial struct Input
    {
        private static readonly FP ByteEncodeMultiplier = FP._0_50;

        public FPVector2 MovementDirection
        {
            get => GetRotatedVector(MovementDirectionEncoded);
            set => MovementDirectionEncoded = GetEncodedAngle(value);
        }

        private static FPVector2 GetRotatedVector(byte encodedAngle)
        {
            if (encodedAngle == default)
                return default;

            FP angle = encodedAngle;
            angle -= 1;
            angle /= ByteEncodeMultiplier;

            return FPVector2.Rotate(FPVector2.Up, angle * FP.Deg2Rad);
        }

        private static byte GetEncodedAngle(FPVector2 direction)
        {
            if (direction == default)
                return default;

            var angle = GetAngle(direction) * ByteEncodeMultiplier;
            angle += 1;
            return (byte)angle.AsInt;
        }

        private static FP GetAngle(FPVector2 direction)
        {
            var degrees = FPVector2.RadiansSigned(FPVector2.Up, direction) * FP.Rad2Deg;
            degrees += 360;
            degrees %= 360;
            return degrees;
        }
    }
}