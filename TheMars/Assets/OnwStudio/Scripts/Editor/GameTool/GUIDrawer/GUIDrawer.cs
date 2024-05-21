#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheMarsGUITool
{
    internal sealed partial class TheMarsGUIToolDrawer
    {
        /// <summary>
        /// .. 플레이팹 데이터 테이블을 업로드 하는 gui기능을 구현하는 인터페이스입니다 
        /// </summary>
        interface IGUIDrawer
        {
            int Page { get; set; }
            int MaxPage { get; }

            bool HasErrors { get; set; }
            bool IsSuccess { get; set; }
            string Message { get; set; }

            void LoadDataFromLocal();
            void SaveDataToLocal();

            void OnDraw();
            void OnEnable();
        }

        // .. 페이징 기능을 구현하는 인터페이스 입니다.
        interface IGUIPagingHandler
        {
            void OnLeft();
            void OnRight();
            void OnFirst();
            void OnLast();
        }
    }
}
#endif
