namespace Asterism
{
    public class ExtraMath
    {
        /// <summary>
        /// 入力された値が２のべき乗となるかを判定する
        /// </summary>
        /// <param name="value"> べき乗かどうか、整数値以外はべき乗では無いと判定する </param>
        /// <returns></returns>
        public static bool IsPowerOfTwo(int value)
        {
            if (value <= 0)
            {
                return false;
            }
            else
            {
                return (value & (value - 1)) == 0;
            }
        }
    }
}
