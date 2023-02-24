using XWMessageQueue;

namespace XWScene
{
    [MessageQueue]
    public enum SceneMessage
    {
        /// <summary>
        /// 场景初始化加载完成
        /// </summary>
        OnSceneInited,
        /// <summary>
        /// 第一个窗口加载完成
        /// </summary>
        OnFirstWindowLoaded,
        /// <summary>
        /// 场景加载中
        /// </summary>
        OnSceneLoading,
        /// <summary>
        /// 场景加载完成
        /// </summary>
        OnSceneLoaded,
        /// <summary>
        /// 加载场景
        /// </summary>
        LoadScene,
        /// <summary>
        /// 场景卸载完成
        /// </summary>
        OnSceneUnLoaded
    }
}
