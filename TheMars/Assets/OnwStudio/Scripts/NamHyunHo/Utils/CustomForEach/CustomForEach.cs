using System.Collections.Generic;
using System;

// .. IEnumberable ForEach메서드가 없기 때문에 ToList나 ToArray를 사용해야하며 해당 메서드는 깊은 복사를 하기 때문에 비효율적.
// .. List 컨테이너에 들어있는 값은 참조형이지만 결국은 참조하는 메모리 주소를 새로운 리스트에 담기때문에 깊은 복사
public static class CustomForEach
{
    public static void ForIndex<T>(int start, List<T> values, Action<int> action)
    {
        for (int i = start; i < values.Count; ++i)
        {
            action.Invoke(i);
        }
    }

    public static void ForIndex<T>(int start, T[] values, Action<int> action)
    {
        for (int i = start; i < values.Length; ++i)
        {
            action.Invoke(i);
        }
    }
}
