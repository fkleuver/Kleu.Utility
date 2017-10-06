using System;
using System.Linq;

namespace Kleu.Utility.Common
{
    public static class GenericComparer
    {
        public static bool AreEqual(object objA, object objB, params string[] ignoreList)
        {
            bool result;

            if (objA != null && objB != null)
            {
                var objectType = objA.GetType();

                result = true;

                foreach (var fieldInfo in objectType.GetInstanceFields().Where(f => !ignoreList.Contains(f.Name)))
                {
                    var valueA = fieldInfo.GetValue(objA);
                    var valueB = fieldInfo.GetValue(objB);

                    if (!AreValuesEqual(valueA, valueB))
                    {
                        result = false;
                    }
                }
            }
            else
                result = Equals(objA, objB);

            return result;
        }


        private static bool AreValuesEqual(object valueA, object valueB)
        {
            bool result;

            var selfValueComparer = valueA as IComparable;

            if (valueA == null && valueB != null || valueA != null && valueB == null)
                result = false;
            else if (selfValueComparer != null && selfValueComparer.CompareTo(valueB) != 0)
                result = false;
            else if (!Equals(valueA, valueB))
                result = false;
            else
                result = true;

            return result;
        }
    }
}
