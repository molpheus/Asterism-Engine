namespace Asterism
{
    public class ExtraMath
    {
        /// <summary>
        /// ���͂��ꂽ�l���Q�ׂ̂���ƂȂ邩�𔻒肷��
        /// </summary>
        /// <param name="value"> �ׂ��悩�ǂ����A�����l�ȊO�ׂ͂���ł͖����Ɣ��肷�� </param>
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
