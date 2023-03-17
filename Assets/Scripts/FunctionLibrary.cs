using System;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace ICKT
{
    public class FunctionLibrary : MonoBehaviour
    {
        #region Pseudo-Random Number Generators
        public static bool GetRandomBool()
        {
            return GetRandomNumber(0, 1) == 1;
        }

        public static int GetRandomNumber(int min, int max)
        {
            var random = new System.Random();
            return random.Next(min, max + 1);
        }

        public static int[] GetRandomNumbers(int amount, int min, int max, bool allowRepeats = true)
        {
            int[] numbers = Enumerable.Range(min, max - min + 1).ToArray();

            int[] result = new int[amount];
            if (allowRepeats)
            {
                var random = new System.Random();
                for (int i = 0; i < amount; i++)
                {
                    int index = random.Next(numbers.Length);
                    result[i] = numbers[index];
                }
            }
            else
            {
                for (int i = 0; i < amount; i++)
                {
                    int index = GetRandomNumber(i, numbers.Length - 1);
                    result[i] = numbers[index];
                    numbers[index] = numbers[i];
                }
            }

            return result;
        }
		#endregion

		public static void SwapElements(MonoBehaviour[] obj, int index1, int index2)
		{
			try
			{
				if (index1 < 0 || index1 >= obj.Length || index2 < 0 || index2 >= obj.Length)
				{
					throw new ArgumentOutOfRangeException("index1 and index2 must be valid indices within the array.");
				}

				if (index1 == index2)
				{
					throw new ArgumentException("index1 and index2 must be different.");
				}

				if (obj[index1] == null && obj[index2] == null)
				{
					throw new ArgumentException("Both elements at the specified indices are null.");
				}

				(obj[index2], obj[index1]) = (obj[index1], obj[index2]);
			}
			catch (Exception ex)
			{
				Debug.LogError("An exception occurred while swapping elements: " + ex.Message);
			}
		}

		public static void FillZeroMemberFields<T>(T[] array)
        {
            T tempData = array[0];
            for (int i = 0; i < array.Length; i++)
            {
                FieldInfo[] fields = typeof(T).GetFields(BindingFlags.Public | BindingFlags.Instance);
                foreach (FieldInfo field in fields)
                {
                    object currentValue = field.GetValue(array[i]);
                    object tempValue = field.GetValue(tempData);
                    if ((currentValue is int notZero && notZero != 0) || (currentValue is float nonZero && nonZero != 0f))
                    {
                        field.SetValue(tempData, currentValue);
                    }
                    else if ((tempValue is int notZero2 && notZero2 != 0) || (tempValue is float nonZero2 && nonZero2 != 0f))
                    {
                        field.SetValue(array[i], tempValue);
                    }
                }
            }
        }
    }
}
