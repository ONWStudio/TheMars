#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace TMGUITool
{
    internal sealed partial class GUIToolDrawer : EditorWindow
    {
        private interface IPagable
        {
            /// <summary>
            /// .. 현재 페이지입니다
            /// </summary>
            int Page { get; set; }
            int PrevPage { get; set; }
            /// <summary>
            /// .. 최대 페이지를 반환해야합니다
            /// </summary>
            int MaxPage { get; }
        }

        private interface ILoadable
        {
            /// <summary>
            /// .. 저장 버튼을 눌렀을때 호출되는 메서드 입니다
            /// </summary>
            void SaveDataToLocal();
        }

        private interface ISaveable
        {
            /// <summary>
            /// .. 불러오기 버튼을 눌렀을때 호출되는 메서드 입니다
            /// </summary>
            void LoadDataFromLocal();
        }

        private interface IDataHandler : ILoadable, ISaveable {}

        /// <summary>
        /// .. gui기능을 구현하는 인터페이스입니다 
        /// </summary>
        private interface IGUIDrawer
        {
            /// <summary>
            /// .. 에러 발생이 되는 경우 해당 프로퍼티를 true로 변경하면 에러 헬프 박스가 생성됩니다. Ok버튼을 누를 시 false가 됩니다
            /// </summary>
            bool HasErrors { get; set; }
            /// <summary>
            /// .. 저장, 불러오기 작업을 할때 성공여부가 해당 프로퍼티에 담깁니다
            /// </summary>
            bool IsSuccess { get; set; }
            /// <summary>
            /// .. 저장, 불러오기 작업을 할때 메세지가 담깁니다
            /// </summary>
            string Message { get; set; }
            /// <summary>
            /// .. OnGUI에서 호출되는 메서드 입니다 해당 메서드 내에서 GUILayout 관련 메서드를 호출해야합니다
            /// </summary>
            void OnDraw();
            /// <summary>
            /// .. 카테고리가 활성화 될때 호출되는 메서드 입니다
            /// </summary>
            void OnEnable();
            /// <summary>
            /// .. TheMarsGUIToolDrawer의 창이 생성되었을때 처음 호출 됩니다.
            /// </summary>
            void Awake();
        }

        /// <summary>
        /// .. 페이징시 발생하는 이벤트 함수를 구현하는 인터페이스 입니다
        /// </summary>
        private interface IGUIPagingHandler<T> where T : IPagable
        {
            /// <summary>
            /// .. 왼쪽 페이지로 넘어갈때 발생하는 이벤트 입니다
            /// </summary>
            void OnLeft();
            /// <summary>
            /// .. 오른쪽 페이지로 넘어갈때 발생하는 이벤트 입니다
            /// </summary>
            void OnRight();
            /// <summary>
            /// .. 첫번째 페이지로 넘어갈때 발생하는 이벤트 입니다
            /// </summary>
            void OnFirst();
            /// <summary>
            /// .. 마지막 페이지로 넘어갈때 발생하는 이벤트 입니다
            /// </summary>
            void OnLast();
        }

        private interface IMovedPage<T> where T : IPagable
        {
            void OnMove();
        }
    }
}
#endif
