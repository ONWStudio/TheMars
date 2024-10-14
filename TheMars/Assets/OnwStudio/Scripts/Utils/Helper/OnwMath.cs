using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Onw.Helper
{
    public static class OnwMath
    {
        public static int GetArithmeticSeriesSum(int target)
        {
            return target * (target + 1) / 2;
        }
    }
}